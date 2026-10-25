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

    // ─────────────────────────────────────────────────────────────────────────
    //  Sqrt(x, mode)  —  IEEE 754 §6.3 correctly rounded for every mode.
    //
    //  RNE hardware sqrt is the starting point.  For directed modes we
    //  need sign(sqrt(x) − w), where w = RNE(sqrt(x)); equivalently
    //  sign(x − w²).  That is computed exactly by the FMA residual in a
    //  scaled space so nothing under- or overflows.
    //
    //  AwayFromZero == ToEven for sqrt: the square root of a finite
    //  positive double is never exactly a midpoint between two adjacent
    //  doubles.  Proof sketch: a midpoint m has 54 mantissa bits, so m²
    //  needs ~108 bits and cannot be a 53-bit double — yet x = m² must
    //  be a double.  Hence no ties, hence "round to nearest, ties away"
    //  coincides with "round to nearest, ties to even."
    // ─────────────────────────────────────────────────────────────────────────
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sqrt(
        double value,
        [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode)
        => mode switch {
            // Fast paths — hardware sqrtsd is correctly rounded to RNE, and
            // AwayFromZero has no ties to break on sqrt.
            MidpointRounding.ToEven => Math.Sqrt(value),
            MidpointRounding.AwayFromZero => Math.Sqrt(value),

#if NET9_0_OR_GREATER
            MidpointRounding.ToZero when Avx512F.IsSupported => SqrtAvx512(value, FloatRoundingMode.ToZero),
            MidpointRounding.ToNegativeInfinity when Avx512F.IsSupported => SqrtAvx512(value, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity when Avx512F.IsSupported => SqrtAvx512(value, FloatRoundingMode.ToPositiveInfinity),
            MidpointRounding.ToZero when Avx10v1.IsSupported => SqrtAvx10(value, FloatRoundingMode.ToZero),
            MidpointRounding.ToNegativeInfinity when Avx10v1.IsSupported => SqrtAvx10(value, FloatRoundingMode.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity when Avx10v1.IsSupported => SqrtAvx10(value, FloatRoundingMode.ToPositiveInfinity),
#endif
            MidpointRounding.ToZero => Sqrt_Directed(value, MidpointRounding.ToZero),
            MidpointRounding.ToNegativeInfinity => Sqrt_Directed(value, MidpointRounding.ToNegativeInfinity),
            MidpointRounding.ToPositiveInfinity => Sqrt_Directed(value, MidpointRounding.ToPositiveInfinity),

            _ => FloatingPointIeee754SpecialInternal.ThrowNotSupported<double>(mode),
        };

#if NET9_0_OR_GREATER
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double SqrtAvx512(double x, [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode) {
        var t = Vector128.CreateScalarUnsafe(x);
        return Avx512F.SqrtScalar(t, t, mode).ToScalar();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double SqrtAvx10(double x, [ConstantExpected(Max = FloatRoundingMode.ToZero)] FloatRoundingMode mode) {
        var t = Vector128.CreateScalarUnsafe(x);
        return Avx10v1.SqrtScalar(t, t, mode).ToScalar();
    }
#endif

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static double Sqrt_Directed(double value, MidpointRounding mode) {
        unchecked {
            // Special cases — NaN, negative (except −0), +Inf, ±0 all
            // round identically under every mode.  Math.Sqrt handles all
            // of them per IEEE 754.
            if (!(value >= 0.0) || double.IsInfinity(value))
                return Math.Sqrt(value);
            if (value == 0.0)
                return value;                       // ±0, sign preserved

            // Correctly rounded RNE result (hardware sqrtsd).
            double w = Math.Sqrt(value);

            // sign(value − w²) determines whether w lies below (r > 0) or
            // above (r < 0) the exact sqrt.  Rescale to keep everything in
            // [1, 4) so the FMA residual cannot underflow (near subnormal x)
            // or overflow (near maxvalue x) — the uniform 2^(−2k) scaling
            // preserves the sign of x − w².
            int e = Math.ILogB(value);
            int k = e >> 1;                        // floor(e / 2)
            int n2 = -2 * k;

            double xs = Math.ScaleB(value, n2);     // ∈ [1, 4)
            double ws = Math.ScaleB(w, -k);         // ∈ [1, 2)

            // r = xs − ws², correctly rounded.  For nonzero δ = xs − ws²:
            //   δ is at least 2^-104 in magnitude (below this we have δ = 0),
            //   while FMA rounding error is ≤ 2^-157, so sign(r) = sign(δ).
            double r = double.FusedMultiplyAdd(ws, -ws, xs);

            if (r == 0.0)
                return w;                           // exact: sqrt(x) == w

            bool below = r > 0.0;                   // w < sqrt(value)

            return mode switch {
                MidpointRounding.ToPositiveInfinity => below ? UpByOne(w) : w,
                // MidpointRounding.ToNegativeInfinity or MidpointRounding.ToZero
                _ => below ? w : DownByOne(w),
            };
        }

        // One ulp toward +∞.  w > 0 and finite.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double UpByOne(double w)
            => BitConverter.UInt64BitsToDouble(
                unchecked(BitConverter.DoubleToUInt64Bits(w) + 1UL));

        // One ulp toward −∞.  w > 0 and finite.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double DownByOne(double w)
            => BitConverter.UInt64BitsToDouble(
                unchecked(BitConverter.DoubleToUInt64Bits(w) - 1UL));
    }
}