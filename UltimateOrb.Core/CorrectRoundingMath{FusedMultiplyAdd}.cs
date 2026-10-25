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
            _ => throw FloatingPointIeee754SpecialInternal.ThrowNotSupported(mode),
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
            MidpointRounding.ToZero => exactPositive
                ? (exactGreaterThanS ? s : Math.BitDecrement(s))
                : (exactGreaterThanS ? Math.BitIncrement(s) : s),
            MidpointRounding.ToNegativeInfinity => exactGreaterThanS ? s : Math.BitDecrement(s),
            MidpointRounding.ToPositiveInfinity => exactGreaterThanS ? Math.BitIncrement(s) : s,
            // MidpointRounding.AwayFromZero
            _ => RoundAwayFromZero(s, error, exactGreaterThanS, exactPositive),
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
            MidpointRounding.ToZero => Multiply_Directed(x, y, MidpointRounding.ToZero),
            MidpointRounding.ToNegativeInfinity => Multiply_Directed(x, y, MidpointRounding.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity => Multiply_Directed(x, y, MidpointRounding.ToPositiveInfinity),
            MidpointRounding.AwayFromZero => Multiply_AwayFromZero(x, y),
            _ => throw FloatingPointIeee754SpecialInternal.ThrowNotSupported(mode),
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


    private static readonly double HalfUlpMantissa0 = BitConverter.UInt64BitsToDouble(0x3CA0000000000000UL); // 2^-53
    private static readonly double HalfUlpMantissa1 = BitConverter.UInt64BitsToDouble(0x3CB0000000000000UL); // 2^-52

    // ─────────────────────────────────────────────────────────────────────────
    //  AwayFromZero: exact via scaling + FMA + TwoSum. No UInt128 on the path.
    // ─────────────────────────────────────────────────────────────────────────
    // Returns  x / 2^ILogB(x)  ∈  (-2, -1] ∪ [1, 2),  for finite x ≠ 0.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FastResetExponent(double x) {
        UInt64 bits = BitConverter.DoubleToUInt64Bits(x);
        UInt64 sign = bits & 0x8000000000000000UL;
        int expField = (int)((bits >> 52) & 0x7FF);
        if (expField != 0)                       // normal: just stamp bias 1023
            return BitConverter.UInt64BitsToDouble(sign | (1023UL << 52) | (bits & 0xFFFFFFFFFFFFFUL));
        UInt64 frac = bits & 0xFFFFFFFFFFFFFUL;   // subnormal: normalize in place
        int p = 63 - BitOperations.LeadingZeroCount(frac);   // MSB of frac
        return BitConverter.UInt64BitsToDouble(sign | (1023UL << 52) | ((frac << (52 - p)) & 0xFFFFFFFFFFFFFUL));
    }

    // Step toward larger magnitude by one ulp (used both for BitIncrement of a
    // non-negative t and BitDecrement of a non-positive t).
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FastStepAwayFromZero(double t)
        => BitConverter.UInt64BitsToDouble(BitConverter.DoubleToUInt64Bits(t) + 1);

    internal static double Multiply_AwayFromZero(double x, double y) {
        unchecked {
            if (!double.IsFinite(x) || !double.IsFinite(y)) return x * y;
            if (x == 0.0 || y == 0.0) return SignsDiffer(x, y) ? -0.0 : +0.0;

            double t = x * y;
            if (double.IsInfinity(t)) return HandleOverflow(t < 0, MidpointRounding.AwayFromZero);
            bool positive = !SignsDiffer(x, y);

            UInt64 tBits = BitConverter.DoubleToUInt64Bits(t);

            // ═════════════════════════════════════════════════════════════════
            //  Case A: t normal.
            // ═════════════════════════════════════════════════════════════════
            if (((tBits >> 52) & 0x7FF) != 0) {
                double mx = FastResetExponent(x);          // |mx| ∈ [1, 2)
                double my = FastResetExponent(y);          // |my| ∈ [1, 2)
                double tm = mx * my;                       // |tm| ∈ [1, 4)
                                                           // FMA residual is exact: residual is a multiple of 2^-104 ≥ 2^-1074.
                double rm = double.FusedMultiplyAdd(mx, my, -tm);

                double halfUlpM = Math.Abs(tm) < 2.0 ? HalfUlpMantissa0 : HalfUlpMantissa1;

                // Step away from zero only when tm sits on the toward-zero side of
                // the tie (rm has the same sign as the exact result).
                if (rm == (positive ? halfUlpM : -halfUlpM))
                    return FastStepAwayFromZero(t);
                return t;
            }

            // ═════════════════════════════════════════════════════════════════
            //  Case B/C: t subnormal (or zero).
            //  Here |x|, |y| ≤ 2^52, so xp = x·2^537, yp = y·2^537 are normal.
            // ═════════════════════════════════════════════════════════════════
            double xp = x * 4.4989137945431964E+161;
            double yp = y * 4.4989137945431964E+161;

            // tScaled = t · 2^1074 = t's raw mantissa bits (exact, in a double).
            double tScaled = (double)(tBits & 0xFFFFFFFFFFFFFUL);
            if ((tBits & 0x8000000000000000UL) != 0) tScaled = -tScaled;

            // rp = exact residual (x·y − t) · 2^1074,  |rp| ≤ 0.5.
            double rp = double.FusedMultiplyAdd(xp, yp, -tScaled);

            if (positive && rp == 0.5) {
                // FMA can round a near-tie to 0.5, so verify.  The follow-up FMA
                // yields a multiple of 2^-H (H = −E_x−E_y−1074 ≤ 1074), so it is 0
                // iff the residual is exactly +0.5.
                if (double.FusedMultiplyAdd(xp, yp, -(tScaled + 0.5)) == 0.0)
                    return FastStepAwayFromZero(t);
            } else if (!positive && rp == -0.5) {
                if (double.FusedMultiplyAdd(xp, yp, -(tScaled - 0.5)) == 0.0)
                    return FastStepAwayFromZero(t);
            }
            return t;
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  Directed rounding: ToZero / ToNegativeInfinity / ToPositiveInfinity.
    //  Same exact-residual trick as RNA, but the step decision comes from
    //  sign(residual) instead of an exact-tie test — no verification FMA.
    // ─────────────────────────────────────────────────────────────────────────

    private static double Multiply_Directed(double x, double y, MidpointRounding mode) {
        unchecked {
            if (!double.IsFinite(x) || !double.IsFinite(y)) return x * y;
            if (x == 0.0 || y == 0.0) return SignsDiffer(x, y) ? -0.0 : +0.0;

            double t = x * y;
            if (double.IsInfinity(t)) return HandleOverflow(t < 0, mode);
            bool positive = !SignsDiffer(x, y);

            UInt64 tBits = BitConverter.DoubleToUInt64Bits(t);
            int expField = (int)((tBits >> 52) & 0x7FF);
            UInt64 mantField = tBits & 0xFFFFFFFFFFFFFUL;

            double rm;

            // Case B: t subnormal OR t = ±NormalMin.  At the boundary, subnormal
            // rounding is coarser than normal rounding, so t = tm·2^(ex+ey) can
            // fail and sign(rm) would no longer match sign(x·y − t).
            if (expField == 0 || (expField == 1 && mantField == 0)) {
                double xp = x * 4.4989137945431964E+161;   // × 2^537
                double yp = y * 4.4989137945431964E+161;

                // tScaled = t · 2^1074, exactly representable for expField ≤ 1.
                double tScaled = expField == 0
                    ? (double)mantField                 // subnormal: mantissa is t·2^1074
                    : 4503599627370496.0;               // 2^52 = NormalMin · 2^1074
                if ((tBits >> 63) != 0) tScaled = -tScaled;

                // rp = (x·y − t) · 2^1074, exact, |rp| ≤ 0.5.
                rm = double.FusedMultiplyAdd(xp, yp, -tScaled);
            } else {
                // Case A: t normal and |t| > NormalMin.  Scaling invariance holds,
                // so tm·2^(ex+ey) = t and sign(rm) = sign(x·y − t).
                double mx = FastResetExponent(x);
                double my = FastResetExponent(y);
                double tm = mx * my;
                rm = double.FusedMultiplyAdd(mx, my, -tm);
            }

            if (rm == 0.0) return t;          // exact — nothing to round.

            return mode switch {
                MidpointRounding.ToPositiveInfinity =>
                    rm > 0.0 ? UpByOne(t) : t,
                MidpointRounding.ToNegativeInfinity =>
                    rm < 0.0 ? DownByOne(t) : t,
                MidpointRounding.ToZero =>
                    (positive ? rm < 0.0 : rm > 0.0) ? TowardZeroByOne(t) : t,
                _ => throw new ArgumentOutOfRangeException(nameof(mode)),
            };
        }
    }

    // One ulp toward +∞.  t finite, not NaN, not −0.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double UpByOne(double t) {
        UInt64 bits = BitConverter.DoubleToUInt64Bits(t);
        unchecked {
            bits += (bits >> 63) != 0 ? unchecked((UInt64)(-1)) : 1UL;
        }
        return BitConverter.UInt64BitsToDouble(bits);
    }

    // One ulp toward −∞.  t finite, not NaN, not +0.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double DownByOne(double t) {
        UInt64 bits = BitConverter.DoubleToUInt64Bits(t);
        unchecked {
            bits += (bits >> 63) != 0 ? 1UL : unchecked((UInt64)(-1));
        }
        return BitConverter.UInt64BitsToDouble(bits);
    }

    // One ulp toward zero.  t finite, nonzero.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double TowardZeroByOne(double t)
        => t == 0.0 ? t : BitConverter.UInt64BitsToDouble(unchecked(BitConverter.DoubleToUInt64Bits(t) - 1));

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

            MidpointRounding.ToZero => FusedMultiplyAdd_Directed(first, second, addend, MidpointRounding.ToZero),
            MidpointRounding.ToNegativeInfinity => FusedMultiplyAdd_Directed(first, second, addend, MidpointRounding.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity => FusedMultiplyAdd_Directed(first, second, addend, MidpointRounding.ToPositiveInfinity),
            MidpointRounding.AwayFromZero => FusedMultiplyAdd_AwayFromZero(first, second, addend),

            _ => throw FloatingPointIeee754SpecialInternal.ThrowNotSupported(mode),
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

    internal static double FusedMultiplyAdd_Directed(double first, double second, double addend,
        [ConstantExpected(Min = MidpointRounding.ToZero, Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
        unchecked {
            if (!double.IsFinite(first) || !double.IsFinite(second) || !double.IsFinite(addend))
                return double.FusedMultiplyAdd(first, second, addend);

            double w = double.FusedMultiplyAdd(first, second, addend);

            if (double.IsPositiveInfinity(w))
                return mode switch {
                    MidpointRounding.ToZero => double.MaxValue,
                    MidpointRounding.ToNegativeInfinity => double.MaxValue,
                    _ => w,
                };
            if (double.IsNegativeInfinity(w))
                return mode switch {
                    MidpointRounding.ToZero => -double.MaxValue,
                    MidpointRounding.ToPositiveInfinity => -double.MaxValue,
                    _ => w,
                };
            if (!double.IsFinite(w)) return w;
            if (first == 0.0 || second == 0.0) return w;

            double p = first * second;
            int sign = ResidualSign(first, second, addend, w, p);
            if (sign == 0) return w;

            UInt64 wBits = BitConverter.DoubleToUInt64Bits(w);
            bool wNeg = (wBits >> 63) != 0;
            bool wZero = (wBits & 0x7FFF_FFFF_FFFF_FFFFUL) == 0;

            bool stepUp, stepDown;
            switch (mode) {
            case MidpointRounding.ToPositiveInfinity:
                stepUp = sign > 0;
                stepDown = false;
                break;
            case MidpointRounding.ToNegativeInfinity:
                stepUp = false;
                stepDown = sign < 0;
                break;
            default:
                stepUp = wNeg && !wZero && sign > 0;
                stepDown = !wNeg && !wZero && sign < 0;
                break;
            }

            if (stepUp) {
                if (wZero) return double.Epsilon;
                return BitConverter.UInt64BitsToDouble(wNeg ? wBits - 1 : wBits + 1);
            }
            if (stepDown) {
                if (wZero) return -double.Epsilon;
                return BitConverter.UInt64BitsToDouble(wNeg ? wBits + 1 : wBits - 1);
            }
            return w;
        }
    }

    private static int ResidualSign(double a, double b, double c, double w, double p) {
        // a*b overflowed, but w is finite. Scale everything by 2^-k so that
        // a2*b2 fits, and recurse. sign(E - w) is unchanged by the scaling.
        if (!double.IsFinite(p)) {
            int la = Math.ILogB(a);
            int lb = Math.ILogB(b);
            int k = la + lb - 1022;
            if (k <= 0) return 0;

            int ka = k / 2;
            int kb = k - ka;

            double a2 = Math.ScaleB(a, -ka);
            double b2 = Math.ScaleB(b, -kb);
            double c2 = Math.ScaleB(c, -k);
            double w2 = Math.ScaleB(w, -k);

            if (!double.IsFinite(a2) || !double.IsFinite(b2) ||
                a2 == 0.0 || b2 == 0.0 ||
                !double.IsFinite(c2) || w2 == 0.0)
                return 0;

            double p2 = a2 * b2;
            if (!double.IsFinite(p2)) return 0;

            return ResidualSign(a2, b2, c2, w2, p2);
        }

        // a*b underflowed to zero (a, b nonzero ⇒ exact a*b ≠ 0, |a*b| ≤ 2^-1075).
        // Then |a*b| ≤ ulp(c)/2, w − c is exact (Sterbenz), and the sign is either
        // sign(a)*sign(b) (when w == c) or sign(c − w).
        if (p == 0.0) {
            double d0 = w - c;
            if (d0 == 0.0) return ((a > 0) == (b > 0)) ? 1 : -1;
            return Math.Sign(c - w);
        }

        int Na = -Math.ILogB(a);
        int Nb = -Math.ILogB(b);
        int N = Na + Nb;

        double as_ = Math.ScaleB(a, Na);
        double bs_ = Math.ScaleB(b, Nb);
        if (as_ == 0.0 || !double.IsFinite(as_) ||
            bs_ == 0.0 || !double.IsFinite(bs_))
            return SimpleSignFromProduct(a, b, c, w);

        double ps = Math.ScaleB(p, N);
        double cs = Math.ScaleB(c, N);
        double ws = Math.ScaleB(w, N);
        if (ps == 0.0 || !double.IsFinite(ps) ||
            !double.IsFinite(cs) || !double.IsFinite(ws))
            return SimpleSignFromProduct(a, b, c, w);

        bool csUnderflow = (c != 0.0) && (cs == 0.0);

        double rps = double.FusedMultiplyAdd(as_, bs_, -ps);
        double ss = ps + cs;
        double bps = ss - ps;
        double es = (ps - (ss - bps)) + (cs - bps);
        double ts = ss - ws;
        double bts = ts - ss;
        double tes = (ss - (ts - bts)) + (-ws - bts);

        Exp4 ex = default;
        ex.Add(rps);
        ex.Add(es);
        ex.Add(ts);
        ex.Add(tes);

        int sgn = ex.Sign();

        if (sgn == 0 && csUnderflow && rps == 0.0 && ts == 0.0 && tes == 0.0)
            sgn = Math.Sign(c);

        return sgn;
    }

    private static int SimpleSignFromProduct(double a, double b, double c, double w) {
        double d0 = w - c;
        if (d0 == 0.0) return ((a > 0) == (b > 0)) ? 1 : -1;
        return Math.Sign(c - w);
    }

    public static double FusedMultiplyAdd_AwayFromZero(
        double first, double second, double addend) {
        unchecked {
            double w = double.FusedMultiplyAdd(first, second, addend);
            if (!double.IsFinite(first) || !double.IsFinite(second) ||
                !double.IsFinite(addend) || !double.IsFinite(w)) {
                return w;
            }

            // If a factor is zero, the exact result is exactly addend.
            // addend is already a double, so no midpoint tie is possible.
            if (first == 0.0 || second == 0.0)
                return w;

            UInt64 wBits = BitConverter.DoubleToUInt64Bits(w);

            // A midpoint rounded to even must have an even significand.
            if ((wBits & 1UL) != 0UL)
                return w;

            int rawW = (int)((wBits >> 52) & 0x7FFUL);

            // h = half ULP of w.
            // For w == 0, subnormal w, and min-normal w: h = 2^-1075.
            // Otherwise h = 2^(rawW - 1076).
            int S = rawW <= 1 ? -1075 : rawW - 1076;

            // Scale a, b, c, w by 2^-S so the target half-ULP becomes 1.0.
            // Split the product scaling between a and b to avoid overflow.
            int Ea = GetExponent(first);
            int Eb = GetExponent(second);
            int Esum = Ea + Eb - S;
            int Ea2 = Esum >> 1;          // floor(Esum / 2)
            int sa = Ea - Ea2;
            int sb = S - sa;

            double a2 = Math.ScaleB(first, -sa);
            double b2 = Math.ScaleB(second, -sb);

            if (a2 == 0.0 || b2 == 0.0 ||
                !double.IsFinite(a2) || !double.IsFinite(b2)) {
                return w;
            }

            double c2 = Math.ScaleB(addend, -S);

            // If c is non-zero but scaled to zero, it is far below the
            // rounding bit and cannot participate in an exact tie.
            if (addend != 0.0 && c2 == 0.0)
                return w;

            if (!double.IsFinite(c2))
                return w;

            double w2 = Math.ScaleB(w, -S);
            if (!double.IsFinite(w2))
                return w;

            double p = a2 * b2;
            if (!double.IsFinite(p) || p == 0.0)
                return w;

            // For a possible tie, the scaled product is normal.
            UInt64 pBits = BitConverter.DoubleToUInt64Bits(p);
            if (((pBits >> 52) & 0x7FFUL) == 0UL)
                return w;

            // Exact product residual: a2 * b2 == p + rp.
            double rp = double.FusedMultiplyAdd(a2, b2, -p);

            // Exact sums:
            //   p + c2       == s + e
            //   s - w2       == t + te
            // E - w = (a2*b2 + c2 - w2) = t + te + e + rp.
            TwoSum(p, c2, out double s, out double e);
            TwoSum(s, -w2, out double t, out double te);

            Exp4 exp = default;
            exp.Add(t);
            exp.Add(te);
            exp.Add(e);
            exp.Add(rp);

            double target = (wBits >> 63) == 0 ? 1.0 : -1.0;

            if (exp.IsSingle(target)) {
                // Step one ULP away from zero.
                // Also handles +0 -> +min-subnormal and -0 -> -min-subnormal.
                wBits += 1UL;
                return BitConverter.UInt64BitsToDouble(wBits);
            }

            return w;
        }
    }

    private static int GetExponent(double value) {
        UInt64 bits = BitConverter.DoubleToUInt64Bits(value);
        int raw = (int)((bits >> 52) & 0x7FFUL);
        UInt64 frac = bits & 0x000FFFFFFFFFFFFFUL;

        if (raw == 0) {
            int msb = 63 - BitOperations.LeadingZeroCount(frac);
            return -1074 + msb;
        }

        return raw - 1023;
    }

    private static void TwoSum(double a, double b, out double s, out double e) {
        s = a + b;
        double bb = s - a;
        e = (a - (s - bb)) + (b - bb);
    }

    private struct Exp4 {
        public double E0;
        public double E1;
        public double E2;
        public double E3;
        public int Count;

        public void Add(double b) {
            double q = b;

            if (Count > 0) TwoSum(q, E0, out q, out E0);
            if (Count > 1) TwoSum(q, E1, out q, out E1);
            if (Count > 2) TwoSum(q, E2, out q, out E2);
            if (Count > 3) TwoSum(q, E3, out q, out E3);

            if (q != 0.0) {
                switch (Count) {
                case 0: E0 = q; break;
                case 1: E1 = q; break;
                case 2: E2 = q; break;
                case 3: E3 = q; break;
                }

                Count++;
            }
        }

        public bool IsSingle(double target) {
            int nonZero = 0;
            double last = 0.0;

            if (E0 != 0.0) { nonZero++; last = E0; }
            if (Count > 1 && E1 != 0.0) { nonZero++; last = E1; }
            if (Count > 2 && E2 != 0.0) { nonZero++; last = E2; }
            if (Count > 3 && E3 != 0.0) { nonZero++; last = E3; }

            return nonZero == 1 && last == target;
        }

        public int Sign() {
            // Shewchuk expansions are non-overlapping: highest nonzero term wins.
            if (Count > 3 && E3 != 0.0) return Math.Sign(E3);
            if (Count > 2 && E2 != 0.0) return Math.Sign(E2);
            if (Count > 1 && E1 != 0.0) return Math.Sign(E1);
            if (Count > 0 && E0 != 0.0) return Math.Sign(E0);
            return 0;
        }
    }

#if false
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
                // MidpointRounding.AwayFromZero
                _ => atMid ? ceilM : (belowMid ? floorM : ceilM),
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
#endif

    private static double HandleOverflow(
        bool negative,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            MidpointRounding.ToEven => negative ? double.NegativeInfinity : double.PositiveInfinity,
            MidpointRounding.ToZero => negative ? -double.MaxValue : double.MaxValue,
            MidpointRounding.ToNegativeInfinity => negative ? double.NegativeInfinity : double.MaxValue,
            MidpointRounding.ToPositiveInfinity => negative ? -double.MaxValue : double.PositiveInfinity,
            // MidpointRounding.AwayFromZero
            _ => negative ? double.NegativeInfinity : double.PositiveInfinity,
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