using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    public readonly struct BigFloat {

        internal readonly long RawExponent;

        internal readonly nint RawPrecision;

        internal readonly nuint RawSignificandFast;

        internal readonly BigArray<nuint> RawSignificandArray;

        [UnscopedRef]
        internal ReadOnlyBigSpan<nuint> RawSignificand {

            get {
                var array = RawSignificandArray;
                return array == null ? new ReadOnlySpan<nuint>(in RawSignificandFast) : array;
            }
        }
    }

    internal ref struct BigFloatBuilderCore {

        // -----------------------------------------------------------------
        // RawExponent sentinels
        // -----------------------------------------------------------------
        //   long.MinValue       : Signaling NaN
        //   long.MinValue + 1   : Quiet NaN
        //   long.MaxValue - 1   : Infinity  (sign bit selects +/-)
        //   long.MaxValue       : preserved
        //   long.MinValue + 2   : preserved
        // -----------------------------------------------------------------
        private const long RawExponentSignalingNaN = long.MinValue;
        private const long RawExponentQuietNaN = long.MinValue + 1;
        private const long RawExponentInfinity = long.MaxValue - 1;
        private const long RawExponentReservedMax = long.MaxValue;
        private const long RawExponentReservedMinP2 = long.MinValue + 2;

        // -----------------------------------------------------------------
        // RawPrecision sentinels (0 = auto; 1, 2, MaxValue-1, MaxValue preserved)
        // -----------------------------------------------------------------
        private const nint RawPrecisionAuto = 0;

        long RawExponent;   // see above

        nint RawPrecision;  // shares storage with the sign bit

        internal nint PrecisionInternal {
            get {
                var p = RawPrecision;
                return p < 0 ? ~p : p;
            }
            set {
                Debug.Assert(value >= 0);
                RawPrecision = (RawPrecision < 0 ? ~value : value);
            }
        }

        internal bool SignBitInternal {
            get => RawPrecision < 0;
            set => RawPrecision ^= (RawPrecision >> (8 * nint.Size - 1)) ^ ((nint)0 - (value ? 1 : 0));
        }

        // limb order: little-endian. unsigned.
        BigSpan<nuint> RawSignificand;

        // Canonical value: 0 or odd RawSignificand with RawSignificand[^1] != 0.
        // Reduced value:   0 or RawSignificand[0] != 0 and RawSignificand[^1] != 0.
        // Zero:            RawSignificand.Length == 0.
        // Canonical/reduced: RawSignificand < 2 ** (PrecisionInternal + 1).
        // Value:           (-1 ** SignBitInternal) * RawSignificand * (2 ** RawExponent).

        Span<nuint> RawSignificandSpan => (Span<nuint>)RawSignificand;

        // =================================================================
        //  Entry points
        // =================================================================

        public static int Add(
            in BigFloatContext context,
            ref BigFloatBuilderCore result,
            BigFloatBuilderCore first,
            BigFloatBuilderCore second,
            MidpointRounding mode)
            => Add(in context, ref result, first, second, mode.ToFloatingPointRounding());

        /// <summary>
        /// Returns 0 (exact), +1 (result &gt; exact) or -1 (result &lt; exact).
        /// Prefers result.PrecisionInternal over context.Precision.
        /// </summary>
        public static int Add(
            in BigFloatContext context,
            ref BigFloatBuilderCore result,
            BigFloatBuilderCore first,
            BigFloatBuilderCore second,
            FloatingPointRounding mode) {

            // -------------------------------------------------------------
            // 0. Effective precision.
            // -------------------------------------------------------------
            nint precision = result.PrecisionInternal;
            if (precision == RawPrecisionAuto) precision = context.Precision;
            if (precision == RawPrecisionAuto) precision = Math.Min(first.PrecisionInternal, second.PrecisionInternal);
            Debug.Assert(precision > 0);
            int p = checked((int)precision);

            // -------------------------------------------------------------
            // 1. Special values.
            //    NaN:      either operand.
            //    Infinity: either operand.
            // -------------------------------------------------------------
            if (IsNaN(first) || IsNaN(second)) {
                SetQuietNaN(ref result, precision);
                return 0;
            }
            if (IsInfinity(first) || IsInfinity(second)) {
                return AddInfinity(ref result, first, second, precision);
            }

            Debug.Assert(!IsReservedExponent(first.RawExponent));
            Debug.Assert(!IsReservedExponent(second.RawExponent));

            // -------------------------------------------------------------
            // 2. Zero handling.
            // -------------------------------------------------------------
            bool z1 = first.RawSignificand.Length == 0;
            bool z2 = second.RawSignificand.Length == 0;

            if (z1 && z2) {
                SetZero(ref result, precision);
                return 0;
            }
            if (z1) return SetFromRounded(ref result, second, precision, mode);
            if (z2) return SetFromRounded(ref result, first, precision, mode);

            // -------------------------------------------------------------
            // 3. Extract signed magnitudes.
            // -------------------------------------------------------------
            BigInteger m1 = ToBigInteger(first.RawSignificand);
            BigInteger m2 = ToBigInteger(second.RawSignificand);
            if (first.SignBitInternal) m1 = -m1;
            if (second.SignBitInternal) m2 = -m2;

            long e1 = first.RawExponent;
            long e2 = second.RawExponent;

            // -------------------------------------------------------------
            // 4. Align exponents with bounded guard region + sticky bit.
            // -------------------------------------------------------------
            long eCommon = Math.Min(e1, e2);
            long d1 = e1 - eCommon;
            long d2 = e2 - eCommon;

            int guard = p + 2;
            bool sticky = false;

            if (d1 > guard) {
                int drop = checked((int)(d1 - guard));
                if (!(m1 & ((BigInteger.One << drop) - 1)).IsZero) sticky = true;
                m1 >>= drop;
                d1 = guard;
            }
            if (d2 > guard) {
                int drop = checked((int)(d2 - guard));
                if (!(m2 & ((BigInteger.One << drop) - 1)).IsZero) sticky = true;
                m2 >>= drop;
                d2 = guard;
            }

            BigInteger sum = (m1 << (int)d1) + (m2 << (int)d2);

            if (sum.IsZero) {
                // With sticky set, the true sum is a non-zero infinitesimal.
                // Directed modes would round away from zero; we currently
                // return canonical zero (extension point).
                SetZero(ref result, precision);
                return 0;
            }

            int sign = sum.Sign;
            BigInteger mag = BigInteger.Abs(sum);
            long exp = eCommon;
            int roundDir = 0;

            // -------------------------------------------------------------
            // 5. Round to at most (precision + 1) bits.
            // -------------------------------------------------------------
            int maxBits = p + 1;
            int bitLen = BitLength(mag);

            if (bitLen > maxBits) {
                int drop = bitLen - maxBits;
                BigInteger rem = mag & ((BigInteger.One << drop) - 1);
                BigInteger kept = mag >> drop;
                exp += drop;

                if (sticky) rem |= BigInteger.One;

                if (!rem.IsZero) {
                    BigInteger half = BigInteger.One << (drop - 1);
                    bool above = rem > half;
                    bool atHalf = rem == half;
                    bool up = mode switch {
                        FloatingPointRounding.ToNearestWithMidpointToEven
                            => above || (atHalf && !kept.IsEven),
                        FloatingPointRounding.ToNearestWithMidpointAwayFromZero
                            => above || atHalf,
                        FloatingPointRounding.Upward
                            => sign > 0,
                        FloatingPointRounding.Downward
                            => sign < 0,
                        FloatingPointRounding.TowardZero
                            => false,
                        FloatingPointRounding.ToOdd
                            => kept.IsEven,
                        _ => above || (atHalf && !kept.IsEven),
                    };
                    if (up) { kept += 1; roundDir = sign; } else { roundDir = -sign; }
                    mag = kept;
                }
            } else if (sticky) {
                // Magnitude exactly representable, but alignment dropped bits.
                // Directed modes would bias; extension point.
            }

            // -------------------------------------------------------------
            // 6. Canonicalize; handles rounding overflow automatically.
            // -------------------------------------------------------------
            // TODO: Use BigInteger.TrailingZeroCount(i)
            while (!mag.IsZero && mag.IsEven) {
                mag >>= 1;
                exp += 1;
            }

            // -------------------------------------------------------------
            // 7. Write back.
            // -------------------------------------------------------------
            WriteBigInteger(ref result, mag);
            result.RawExponent = exp;
            result.SignBitInternal = sign < 0;
            result.PrecisionInternal = precision;

            return roundDir;
        }

        // =================================================================
        //  Special-value helpers
        // =================================================================

        private static bool IsSignalingNaN(BigFloatBuilderCore x)
            => x.RawExponent == RawExponentSignalingNaN;

        private static bool IsQuietNaN(BigFloatBuilderCore x)
            => x.RawExponent == RawExponentQuietNaN;

        private static bool IsNaN(BigFloatBuilderCore x)
            => x.RawExponent == RawExponentSignalingNaN
            || x.RawExponent == RawExponentQuietNaN;

        private static bool IsInfinity(BigFloatBuilderCore x)
            => x.RawExponent == RawExponentInfinity;

        private static bool IsReservedExponent(long e)
            => e == RawExponentReservedMax || e == RawExponentReservedMinP2;

        private static void SetQuietNaN(ref BigFloatBuilderCore result, nint precision) {
            result.RawExponent = RawExponentQuietNaN;
            result.RawSignificand = default;
            result.SignBitInternal = false;
            result.PrecisionInternal = precision;
        }

        private static void SetInfinity(
            ref BigFloatBuilderCore result, bool negative, nint precision) {
            result.RawExponent = RawExponentInfinity;
            result.RawSignificand = default;
            result.PrecisionInternal = precision;
            result.SignBitInternal = negative;
        }

        private static int AddInfinity(
            ref BigFloatBuilderCore result,
            BigFloatBuilderCore first,
            BigFloatBuilderCore second,
            nint precision) {

            bool inf1 = IsInfinity(first);
            bool inf2 = IsInfinity(second);
            bool s1 = first.SignBitInternal;
            bool s2 = second.SignBitInternal;

            if (inf1 && inf2) {
                if (s1 != s2) {
                    // +Inf + -Inf  =>  quiet NaN
                    SetQuietNaN(ref result, precision);
                    return 0;
                }
                SetInfinity(ref result, s1, precision);
                return 0;
            }
            if (inf1) { SetInfinity(ref result, s1, precision); return 0; }
            /* inf2 */
            SetInfinity(ref result, s2, precision); return 0;
        }

        private static void SetZero(ref BigFloatBuilderCore result, nint precision) {
            result.RawExponent = 0;
            result.RawSignificand = default;
            result.SignBitInternal = false;
            result.PrecisionInternal = precision;
        }

        // =================================================================
        //  Single-operand rounding (for x + 0)
        // =================================================================

        private static int SetFromRounded(
            ref BigFloatBuilderCore result,
            BigFloatBuilderCore src,
            nint precision,
            FloatingPointRounding mode) {

            BigInteger mag = ToBigInteger(src.RawSignificand);
            if (mag.IsZero) { SetZero(ref result, precision); return 0; }

            int sign = src.SignBitInternal ? -1 : +1;
            long exp = src.RawExponent;

            int p = checked((int)precision);
            int maxBits = p + 1;
            int bitLen = BitLength(mag);
            int roundDir = 0;

            if (bitLen > maxBits) {
                int drop = bitLen - maxBits;
                BigInteger rem = mag & ((BigInteger.One << drop) - 1);
                BigInteger kept = mag >> drop;
                exp += drop;

                if (!rem.IsZero) {
                    BigInteger half = BigInteger.One << (drop - 1);
                    bool above = rem > half;
                    bool atHalf = rem == half;
                    bool up = mode switch {
                        FloatingPointRounding.ToNearestWithMidpointToEven
                            => above || (atHalf && !kept.IsEven),
                        FloatingPointRounding.ToNearestWithMidpointAwayFromZero
                            => above || atHalf,
                        FloatingPointRounding.Upward
                            => sign > 0,
                        FloatingPointRounding.Downward
                            => sign < 0,
                        FloatingPointRounding.TowardZero
                            => false,
                        FloatingPointRounding.ToOdd
                            => kept.IsEven,
                        _ => above || (atHalf && !kept.IsEven),
                    };
                    if (up) { kept += 1; roundDir = sign; } else { roundDir = -sign; }
                }
                mag = kept;
            }

            while (!mag.IsZero && mag.IsEven) { mag >>= 1; exp += 1; }

            WriteBigInteger(ref result, mag);
            result.RawExponent = exp;
            result.SignBitInternal = sign < 0;
            result.PrecisionInternal = precision;
            return roundDir;
        }

        // =================================================================
        //  Limb interop
        // =================================================================

        private static int BitLength(BigInteger v) {
            if (v.IsZero) return 0;
            return checked((int)BigInteger.Abs(v).GetBitLength());
        }

        /// <summary>
        /// Little-endian nuint limbs → non-negative BigInteger.
        /// </summary>
        private static BigInteger ToBigInteger(BigSpan<nuint> span) {
            var s = (Span<nuint>)span;
            if (s.Length == 0) return BigInteger.Zero;

            if (BitConverter.IsLittleEndian) {
                return new BigInteger(
                    MemoryMarshal.AsBytes(s),
                    isUnsigned: true,
                    isBigEndian: false);
            }

            // Portable big-endian host path (rare; not perf-critical).
            BigInteger acc = BigInteger.Zero;
            for (int i = s.Length - 1; i >= 0; i--)
                acc = (acc << (8 * IntPtr.Size)) | s[i];
            return acc;
        }

        /// <summary>
        /// Non-negative BigInteger → little-endian nuint limbs in
        /// <see cref="result"/>.RawSignificand. Assumes sufficient capacity.
        /// </summary>
        private static void WriteBigInteger(
            ref BigFloatBuilderCore result,
            BigInteger value) {

            Debug.Assert(value.Sign >= 0);

            if (value.IsZero) {
                result.RawSignificand = default;
                return;
            }

            int byteCount = value.GetByteCount(isUnsigned: true);
            int limbCount = (byteCount + nint.Size - 1) / nint.Size;

            var dst = (Span<nuint>)result.RawSignificand;
            if (dst.Length < limbCount)
                throw new InvalidOperationException(
                    "Result significand buffer is too small for the requested precision.");

            var bytesDst = MemoryMarshal.AsBytes(dst);
            bytesDst.Slice(0, limbCount * nint.Size).Clear();

            if (BitConverter.IsLittleEndian) {
                if (!value.TryWriteBytes(
                        bytesDst,
                        out int written,
                        isUnsigned: true,
                        isBigEndian: false)) {
                    throw new InvalidOperationException(
                        "BigInteger.TryWriteBytes unexpectedly failed.");
                }
                Debug.Assert(written == byteCount);
            } else {
                // Portable big-endian host path.
                BigInteger v = value;
                BigInteger mask = (BigInteger.One << (8 * IntPtr.Size)) - 1;
                for (int i = 0; i < limbCount; i++) {
                    dst[i] = (nuint)(v & mask);
                    v >>= 8 * IntPtr.Size;
                }
            }

            // Trim logical length to the exact limb count.
            // Adapt to your BigSpan<T> constructor / length setter.
            result.RawSignificand = dst.Slice(0, limbCount);
        }
    }

    internal readonly record struct BigFloatContext {
        public readonly long Int64RawEMin; // RawEMin == EMin - Precision + 2
        public readonly long Int64RawEMax; // RawEMax == EMax + 1
        public readonly nint Precision;
    }
}
