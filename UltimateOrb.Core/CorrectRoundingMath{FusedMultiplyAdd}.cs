using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using UltimateOrb.Numerics;
#pragma warning disable UoWIP
using static UltimateOrb.Numerics.Binary128Arithmetic;
#pragma warning restore UoWIP

namespace UltimateOrb;


public static partial class CorrectRoundingMath {

    // ─────────────────────────────────────────────────────────────────────
    //  NOTE — FloatS128E31Internal is NOT used here.
    //
    //  Its arithmetic (AddDint, MulDint, DivDint, ...) always rounds to
    //  nearest-even and carries no excess-precision ("sticky bit")
    //  information.  Correct directed rounding (ToZero / To±Infinity /
    //  AwayFromZero) requires knowing the sign of the residual outside the
    //  representable precision — information FloatS128E31Internal discards
    //  after every operation.  Converting to/from it would be both slower
    //  and less correct than the explicit sticky-bit tracking below.
    // ─────────────────────────────────────────────────────────────────────

    private readonly record struct DoubleParts(bool Neg, UInt128 Mant, int Exp);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DoubleParts Decompose(double value) {
        unchecked {
            UInt64 bits = BitConverter.DoubleToUInt64Bits(value);
            bool neg = (bits >> 63) != 0;
            int expField = (int)((bits >> 52) & 0x7FF);
            UInt64 frac = bits & 0x000FFFFFFFFFFFFFUL;

            return expField == 0
                ? (frac == 0 ? new(neg, 0, 0) : new(neg, frac, -1074))
                : new(neg, (1UL << 52) | frac, expField - 1075);
        }
    }

    // =====================================================================
    //  ADD
    // =====================================================================

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Add(
        double x, double y,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            MidpointRounding.ToEven => x + y,
#if NET9_0_OR_GREATER
            MidpointRounding.ToZero when Avx512F.IsSupported => AddAvx512(x, y, FloatRoundingMode.ToZero),
            MidpointRounding.ToZero when Avx10v1.IsSupported => AddAvx10(x, y, FloatRoundingMode.ToZero),
            MidpointRounding.ToNegativeInfinity when Avx512F.IsSupported => AddAvx512(x, y, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToNegativeInfinity when Avx10v1.IsSupported => AddAvx10(x, y, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity when Avx512F.IsSupported => AddAvx512(x, y, FloatRoundingMode.ToPositiveInfinity),
            MidpointRounding.ToPositiveInfinity when Avx10v1.IsSupported => AddAvx10(x, y, FloatRoundingMode.ToPositiveInfinity),
#endif
            MidpointRounding.ToZero or
            MidpointRounding.ToNegativeInfinity or
            MidpointRounding.ToPositiveInfinity or
            MidpointRounding.AwayFromZero => AddSoftware(x, y, mode),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double AddAvx512(double x, double y,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx512F.AddScalar(Vector128.CreateScalarUnsafe(x),
                             Vector128.CreateScalarUnsafe(y), mode).ToScalar();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double AddAvx10(double x, double y,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx10v1.AddScalar(Vector128.CreateScalarUnsafe(x),
                             Vector128.CreateScalarUnsafe(y), mode).ToScalar();
#endif

    // All arithmetic is over doubles; no integer overflow concerns.
    private static double AddSoftware(
        double x, double y,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        double s = x + y;

        if (!double.IsFinite(x) || !double.IsFinite(y))
            return s;

        if (double.IsPositiveInfinity(s)) return HandleOverflow(false, mode);
        if (double.IsNegativeInfinity(s)) return HandleOverflow(true, mode);

        if (Math.Abs(x) < Math.Abs(y))
            (x, y) = (y, x);

        double z = s - x;

        if (z == y) {
            // Exact result.  Signed-zero for cancellation:
            //   roundTowardNegative → −0, all others → +0.
            if (s == 0.0 && SignsDiffer(x, y))
                return mode == MidpointRounding.ToNegativeInfinity ? -0.0 : +0.0;
            return s;
        }

        double error = y - z;
        bool exactGreaterThanS = error > 0.0;
        bool exactPositive = s > 0.0;

        return mode switch {
            MidpointRounding.ToEven => s,
            MidpointRounding.ToPositiveInfinity => exactGreaterThanS ? Math.BitIncrement(s) : s,
            MidpointRounding.ToNegativeInfinity => exactGreaterThanS ? s : Math.BitDecrement(s),
            MidpointRounding.ToZero => exactPositive
                ? (exactGreaterThanS ? s : Math.BitDecrement(s))
                : (exactGreaterThanS ? Math.BitIncrement(s) : s),
            MidpointRounding.AwayFromZero => RoundAwayFromZero(s, error, exactGreaterThanS, exactPositive),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };
    }

    // =====================================================================
    //  MULTIPLY
    // =====================================================================

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Multiply(
        double x, double y,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            MidpointRounding.ToEven => x * y,
#if NET9_0_OR_GREATER
            MidpointRounding.ToZero when Avx512F.IsSupported => MultiplyAvx512(x, y, FloatRoundingMode.ToZero),
            MidpointRounding.ToZero when Avx10v1.IsSupported => MultiplyAvx10(x, y, FloatRoundingMode.ToZero),
            MidpointRounding.ToNegativeInfinity when Avx512F.IsSupported => MultiplyAvx512(x, y, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToNegativeInfinity when Avx10v1.IsSupported => MultiplyAvx10(x, y, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity when Avx512F.IsSupported => MultiplyAvx512(x, y, FloatRoundingMode.ToPositiveInfinity),
            MidpointRounding.ToPositiveInfinity when Avx10v1.IsSupported => MultiplyAvx10(x, y, FloatRoundingMode.ToPositiveInfinity),
#endif
            MidpointRounding.ToZero or
            MidpointRounding.ToNegativeInfinity or
            MidpointRounding.ToPositiveInfinity or
            MidpointRounding.AwayFromZero => MultiplySoftware(x, y, mode),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double MultiplyAvx512(double x, double y,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx512F.MultiplyScalar(Vector128.CreateScalarUnsafe(x),
                                  Vector128.CreateScalarUnsafe(y), mode).ToScalar();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double MultiplyAvx10(double x, double y,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx10v1.MultiplyScalar(Vector128.CreateScalarUnsafe(x),
                                  Vector128.CreateScalarUnsafe(y), mode).ToScalar();
#endif

    private static double MultiplySoftware(
        double x, double y,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            if (!double.IsFinite(x) || !double.IsFinite(y))
                return x * y;

            double p = x * y;

            if (double.IsPositiveInfinity(p)) return HandleOverflow(false, mode);
            if (double.IsNegativeInfinity(p)) return HandleOverflow(true, mode);

            if (x == 0.0 || y == 0.0)
                return SignsDiffer(x, y) ? -0.0 : +0.0;

            var a = Decompose(x);
            var b = Decompose(y);

            bool negP = a.Neg ^ b.Neg;
            UInt128 mant = a.Mant * b.Mant;   // ≤ 2^106, fits UInt128
            int exp = a.Exp + b.Exp;          // range [-2148, 1942]

            if (mant == 0) return negP ? -0.0 : +0.0;

            return RoundMantissa(negP, mant, exp, sticky: 0, mode);
        }
    }

    // =====================================================================
    //  Shared helpers
    // =====================================================================

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double RoundAwayFromZero(double s, double error,
        bool exactGreaterThanS, bool exactPositive) {
        double neighbor = exactGreaterThanS ? Math.BitIncrement(s) : Math.BitDecrement(s);
        double gap = Math.Abs(neighbor - s);

        // RNE already delivered the nearest neighbour; AwayFromZero differs
        // only at exact midpoints.
        if (Math.Abs(error) != gap * 0.5)
            return s;

        return exactPositive
            ? (exactGreaterThanS ? Math.BitIncrement(s) : s)
            : (exactGreaterThanS ? s : Math.BitDecrement(s));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SignsDiffer(double x, double y)
        => unchecked(((BitConverter.DoubleToInt64Bits(x) ^
                       BitConverter.DoubleToInt64Bits(y)) & long.MinValue) != 0);

    // =====================================================================
    //  FUSED MULTIPLY-ADD
    // =====================================================================

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double FusedMultiplyAdd(
        double first, double second, double addend,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            // Fast path — hardware RNE (JIT emits vfmadd231sd without {er}).
            MidpointRounding.ToEven => double.FusedMultiplyAdd(first, second, addend),

#if NET9_0_OR_GREATER
            MidpointRounding.ToZero when Avx512F.IsSupported => FusedMultiplyAddAvx512(first, second, addend, FloatRoundingMode.ToZero),
            MidpointRounding.ToZero when Avx10v1.IsSupported => FusedMultiplyAddAvx10(first, second, addend, FloatRoundingMode.ToZero),
            MidpointRounding.ToNegativeInfinity when Avx512F.IsSupported => FusedMultiplyAddAvx512(first, second, addend, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToNegativeInfinity when Avx10v1.IsSupported => FusedMultiplyAddAvx10(first, second, addend, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity when Avx512F.IsSupported => FusedMultiplyAddAvx512(first, second, addend, FloatRoundingMode.ToPositiveInfinity),
            MidpointRounding.ToPositiveInfinity when Avx10v1.IsSupported => FusedMultiplyAddAvx10(first, second, addend, FloatRoundingMode.ToPositiveInfinity),
#endif

            MidpointRounding.ToZero or
            MidpointRounding.ToNegativeInfinity or
            MidpointRounding.ToPositiveInfinity or
            MidpointRounding.AwayFromZero => FusedMultiplyAddSoftware(first, second, addend, mode),

            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FusedMultiplyAddAvx512(double a, double b, double c,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx512F.FusedMultiplyAddScalar(
               Vector128.CreateScalarUnsafe(a),
               Vector128.CreateScalarUnsafe(b),
               Vector128.CreateScalarUnsafe(c),
               mode).ToScalar();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FusedMultiplyAddAvx10(double a, double b, double c,
        [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode)
        => Avx10v1.FusedMultiplyAddScalar(
               Vector128.CreateScalarUnsafe(a),
               Vector128.CreateScalarUnsafe(b),
               Vector128.CreateScalarUnsafe(c),
               mode).ToScalar();
#endif

    private static double FusedMultiplyAddSoftware(
        double first, double second, double addend,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            // Non-finite inputs: NaN/Inf result is mode-independent, so RNE is safe.
            if (!double.IsFinite(first) ||
                !double.IsFinite(second) ||
                !double.IsFinite(addend)) {
                return double.FusedMultiplyAdd(first, second, addend);
            }

            var a = Decompose(first);
            var b = Decompose(second);
            var c = Decompose(addend);

            var p = new DoubleParts(
                Neg: a.Neg ^ b.Neg,
                Mant: a.Mant * b.Mant,      // ≤ 2^106, fits UInt128
                Exp: a.Exp + b.Exp);        // range [-2148, 1942]

            return RoundTwoTerms(p, c, mode);
        }
    }

    // =====================================================================
    //  Rounding core
    // =====================================================================

    // NOTE: the unchecked block is correctness-critical.  The carry-detection
    // idiom  M = LM_int + SM_int; if (M < LM_int) ...  requires wraparound,
    // and  M = (M << 1) - 1  in the "just below a power of two" branch
    // also relies on wrap when M == 2^127.
    private static double RoundTwoTerms(
        DoubleParts p, DoubleParts c,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            var (signP, MP, eP) = p;
            var (signC, MC, eC) = c;

            if (MP == 0 && MC == 0) {
                if (signP == signC) return signP ? -0.0 : 0.0;
                return mode == MidpointRounding.ToNegativeInfinity ? -0.0 : 0.0;
            }
            if (MP == 0) return RoundMantissa(signC, MC, eC, 0, mode);
            if (MC == 0) return RoundMantissa(signP, MP, eP, 0, mode);

            int cmp = CompareMagnitude(MP, eP, MC, eC);
            bool signL, signS;
            UInt128 LM, SM;
            int Le, Se;

            if (cmp >= 0) {
                (signL, LM, Le) = (signP, MP, eP);
                (signS, SM, Se) = (signC, MC, eC);
            } else {
                (signL, LM, Le) = (signC, MC, eC);
                (signS, SM, Se) = (signP, MP, eP);
            }

            if (cmp == 0 && signL != signS)
                return mode == MidpointRounding.ToNegativeInfinity ? -0.0 : 0.0;

            int E = Le + LM.GetBitLength() - 128;   // safe: [-2275, 1920]
            UInt128 LM_int = LM << (Le - E);         // shift ∈ [0, 127]

            UInt128 SM_int;
            bool stickySM = false;
            UInt128 remSM = 0;
            int dropSM = 0;

            if (Se >= E) {
                SM_int = SM << (Se - E);             // shift ∈ [0, 127]
            } else {
                dropSM = E - Se;
                if (dropSM >= 128) {
                    SM_int = 0;
                    stickySM = SM != 0;
                    remSM = SM;
                } else {
                    SM_int = SM >> dropSM;
                    remSM = SM & (((UInt128)1 << dropSM) - 1);
                    stickySM = remSM != 0;
                }
            }

            UInt128 M;
            int sticky;

            if (signL == signS) {
                // Wraparound intentional (carry detection below).
                M = LM_int + SM_int;
                sticky = stickySM ? 1 : 0;
                if (M < LM_int) {                    // carry out of 128 bits
                    bool lostBit = (M & 1) != 0;
                    M = (M >> 1) | ((UInt128)1 << 127);
                    E += 1;
                    if (lostBit) sticky = 1;
                }
            } else {
                // Underflow impossible: cmp > 0 ⇒ LM_int > SM_int.
                M = LM_int - SM_int;
                sticky = stickySM ? -1 : 0;
            }

            // "Just below a power of two": M == 2^k, sticky < 0.
            // Shift into the lower binade: 2^k − δ ↦ (2M − 1) · 2^(k−1).
            if (sticky < 0 && M != 0 && (M & (M - 1)) == 0) {
                int newSticky;
                if (dropSM == 0) {
                    newSticky = 0;
                } else if (dropSM >= 128) {
                    newSticky = 1;
                } else {
                    UInt128 half = (UInt128)1 << (dropSM - 1);
                    newSticky = remSM < half ? 1 : remSM == half ? 0 : -1;
                }
                // Wraparound intentional when M was 2^127.
                M = (M << 1) - 1;
                E -= 1;
                sticky = newSticky;
            }

            return RoundMantissa(signL, M, E, sticky, mode);
        }
    }

    private static int CompareMagnitude(UInt128 m1, int e1, UInt128 m2, int e2) {
        unchecked {
            if (m1 == 0) return m2 == 0 ? 0 : -1;
            if (m2 == 0) return 1;

            int bl1 = m1.GetBitLength() + e1;
            int bl2 = m2.GetBitLength() + e2;
            if (bl1 != bl2) return bl1.CompareTo(bl2);

            if (e1 == e2) return m1.CompareTo(m2);
            return e1 > e2
                ? (m1 << (e1 - e2)).CompareTo(m2)     // shift ∈ [0, 127]
                : m1.CompareTo(m2 << (e2 - e1));      // shift ∈ [0, 127]
        }
    }

    /// <summary>
    /// Rounds the exact value  (−1)^negative · (M · 2^E + sticky·ε),
    /// 0 &lt; ε &lt; 2^E, sticky ∈ {−1, 0, +1}, to double with the given mode.
    /// </summary>
    private static double RoundMantissa(
        bool negative, UInt128 M, int E, int sticky,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            if (M == 0) return negative ? -0.0 : 0.0;

            int L = M.GetBitLength();
            int valueExp = E + L - 1;

            int q = valueExp < -1022 ? -1074 : E + Math.Max(0, L - 53);
            int drop = Math.Max(0, q - E);

            UInt128 Q, rem;
            if (drop == 0) { Q = M; rem = 0; } else if (drop >= 128) { Q = 0; rem = M; } else { Q = M >> drop; rem = M & (((UInt128)1 << drop) - 1); }

            if (rem == 0 && sticky == 0)
                return MakeDouble(negative, Q, q, mode);

            UInt128 floorM, ceilM;
            bool belowMid, atMid;

            if (rem != 0) {
                if (drop > 128) {
                    belowMid = true; atMid = false;
                } else {
                    UInt128 half = drop == 128
                        ? (UInt128)1 << 127
                        : (UInt128)1 << (drop - 1);
                    if (rem < half) { belowMid = true; atMid = false; } else if (rem > half) { belowMid = false; atMid = false; } else { belowMid = sticky < 0; atMid = sticky == 0; }
                }
                floorM = Q;
                ceilM = Q + 1;
            } else if (sticky > 0) {
                floorM = Q;
                ceilM = Q + 1;
                belowMid = true;
                atMid = false;
            } else {
                // rem == 0, sticky < 0.  Q ≥ 1 here.
                floorM = Q - 1;
                ceilM = Q;
                belowMid = false;
                atMid = false;
            }

            UInt128 N = mode switch {
                MidpointRounding.ToZero => floorM,
                MidpointRounding.ToPositiveInfinity => negative ? floorM : ceilM,
                MidpointRounding.ToNegativeInfinity => negative ? ceilM : floorM,
                MidpointRounding.ToEven =>
                    atMid ? ((floorM & 1) == 0 ? floorM : ceilM)
                          : (belowMid ? floorM : ceilM),
                MidpointRounding.AwayFromZero =>
                    atMid ? ceilM : (belowMid ? floorM : ceilM),
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
            };

            return MakeDouble(negative, N, q, mode);
        }
    }

    private static double MakeDouble(
        bool negative, UInt128 N, int q,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            if (N == 0) return negative ? -0.0 : 0.0;

            int L = N.GetBitLength();
            int valueExp = q + L - 1;

            if (valueExp > 1023) return HandleOverflow(negative, mode);

            if (valueExp < -1022) {
                // True subnormal: N fits the 52-bit trailing significand.
                UInt64 fracSub = (UInt64)N;
                UInt64 bitsSub = (negative ? 1UL << 63 : 0) | fracSub;
                return BitConverter.UInt64BitsToDouble(bitsSub);
            }

            if (L > 53) {
                int s = L - 53;   // s ∈ {1, 2}: N ≤ 2^53 after rounding
                N >>= s;
                q += s;
                L = 53;
                // valueExp = q + L - 1 is invariant under this normalisation.
            }

            int shiftLeft = 53 - L;    // ≥ 0 (L ≤ 53 here)
            UInt128 M = N << shiftLeft;
            UInt64 fracN = (UInt64)(M - ((UInt128)1 << 52));
            int expField = valueExp + 1023;
            if (expField >= 2047) return HandleOverflow(negative, mode);

            UInt64 bitsOut =
                (negative ? 1UL << 63 : 0) |
                ((UInt64)expField << 52) |
                (fracN & 0x000FFFFFFFFFFFFFUL);
            return BitConverter.UInt64BitsToDouble(bitsOut);
        }
    }

    private static double HandleOverflow(
        bool negative,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            MidpointRounding.ToPositiveInfinity => negative ? -double.MaxValue : double.PositiveInfinity,
            MidpointRounding.ToNegativeInfinity => negative ? double.NegativeInfinity : double.MaxValue,
            MidpointRounding.ToZero => negative ? -double.MaxValue : double.MaxValue,
            MidpointRounding.AwayFromZero => negative ? double.NegativeInfinity : double.PositiveInfinity,
            MidpointRounding.ToEven => negative ? double.NegativeInfinity : double.PositiveInfinity,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
        };

    extension(UInt128 @this) {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetBitLength() {
            unchecked {
                // Both UInt128 → UInt64 narrowing casts are provably in-range.
                // Wrapping in unchecked eliminates the range-check branches
                // that a checked-default build would otherwise emit on every
                // call to this hot helper.
                UInt64 hi = (UInt64)(@this >> 64);
                if (hi != 0) return 128 - BitOperations.LeadingZeroCount(hi);
                UInt64 lo = (UInt64)@this;
                return lo == 0 ? 0 : 64 - BitOperations.LeadingZeroCount(lo);
            }
        }
    }
}