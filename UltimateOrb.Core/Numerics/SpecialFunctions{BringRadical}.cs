using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {
    using static Double;

    public static partial class DoubleExtensions {

        extension(double) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool IsNormalOrSubnormal(double value) {
                var bits = BitConverter.DoubleToUInt64Bits(Abs(value));
                return (bits - 1) < BitConverter.DoubleToUInt64Bits(double.MaxValue);
            }
        }
    }
}

namespace UltimateOrb.Extensions {

    public static partial class CorrectRoundingExtensions {
    
        extension(double) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Single ToSingle(double value) {
                return ToSingle(value, MidpointRounding.ToEven);
            }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Single ToSingle(double value, [ConstantExpected(Max = FloatingPointRounding.TowardInfinity)] FloatingPointRounding rounding) {
                var nearest = (Single)value;
                if (double.IsNormalOrSubnormal(value)) {
                    // ── Fast path: exact representation ─────────────────────────────
                    if ((double)nearest == value) {
                        // For ToOdd, an exact value is returned unchanged.
                        return nearest;
                    }
                    // ── Determine the two bracketing floats ────────────────────────
                    // The ordering is always:  below < value < above
                    Single below, above;
                    if ((double)nearest < value) {
                        below = nearest;
                        above = MathF.BitIncrement(nearest);
                    } else {
                        below = MathF.BitDecrement(nearest);
                        above = nearest;
                    }

                    // ── Overflow / underflow guards ────────────────────────────────
                    // For positive values the overflow threshold is the midpoint
                    // between Single.MaxValue and +∞; for negative values the
                    // symmetric threshold is used.
                    const double UlpOfMax = 1.0 / (1L << 23); // 2^-23 in the exponent scale
                    double overflowThreshold = (value > 0 ? 1 : -1) * (Single.MaxValue + Math.Pow(2, 103));
                    if (Math.Abs(value) >= Math.Abs(overflowThreshold)) {
                        // The value is closer to infinity than to the largest finite Single.
                        return value > 0
                            ? (rounding switch {
                                FloatingPointRounding.Upward => Single.PositiveInfinity,
                                FloatingPointRounding.TowardInfinity => Single.PositiveInfinity,
                                FloatingPointRounding.ToNearestWithMidpointToEven => Single.PositiveInfinity,
                                FloatingPointRounding.ToNearestWithMidpointAwayFromZero => Single.PositiveInfinity,
                                FloatingPointRounding.ToNearestWithMidpointUpward => Single.PositiveInfinity,
                                FloatingPointRounding.ToNearestWithMidpointDownward => Single.MaxValue,
                                FloatingPointRounding.ToNearestWithMidpointTowardZero => Single.MaxValue,
                                FloatingPointRounding.ToNearestWithMidpointToOdd => Single.PositiveInfinity,
                                FloatingPointRounding.Downward => Single.MaxValue,
                                FloatingPointRounding.TowardZero => Single.MaxValue,
                                FloatingPointRounding.ToOdd => Single.MaxValue,
                                _ => throw new ArgumentOutOfRangeException(nameof(rounding))
                            })
                            : (rounding switch {
                                FloatingPointRounding.Upward => -Single.MaxValue,
                                FloatingPointRounding.TowardInfinity => Single.NegativeInfinity,
                                FloatingPointRounding.ToNearestWithMidpointToEven => Single.NegativeInfinity,
                                FloatingPointRounding.ToNearestWithMidpointAwayFromZero => Single.NegativeInfinity,
                                FloatingPointRounding.ToNearestWithMidpointUpward => -Single.MaxValue,
                                FloatingPointRounding.ToNearestWithMidpointDownward => Single.NegativeInfinity,
                                FloatingPointRounding.ToNearestWithMidpointTowardZero => -Single.MaxValue,
                                FloatingPointRounding.ToNearestWithMidpointToOdd => Single.NegativeInfinity,
                                FloatingPointRounding.Downward => Single.NegativeInfinity,
                                FloatingPointRounding.TowardZero => -Single.MaxValue,
                                FloatingPointRounding.ToOdd => -Single.MaxValue,
                                _ => throw new NotSupportedException()
                            });
                    }

                    // Underflow: value is smaller than the smallest positive subnormal.
                    // The midpoint between 0 and Single.Epsilon is Single.Epsilon / 2.
                    double underflowThreshold = Single.Epsilon / 2.0;
                    if (Math.Abs(value) < underflowThreshold) {
                        // Round to zero or to the smallest subnormal depending on mode.
                        return value > 0
                            ? (rounding switch {
                                FloatingPointRounding.Upward => Single.Epsilon,
                                FloatingPointRounding.TowardInfinity => Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointToEven => 0.0f,
                                FloatingPointRounding.ToNearestWithMidpointAwayFromZero => Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointUpward => Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointDownward => 0.0f,
                                FloatingPointRounding.ToNearestWithMidpointTowardZero => 0.0f,
                                FloatingPointRounding.ToNearestWithMidpointToOdd => Single.Epsilon,
                                FloatingPointRounding.Downward => 0.0f,
                                FloatingPointRounding.TowardZero => 0.0f,
                                FloatingPointRounding.ToOdd => Single.Epsilon,
                                _ => throw new NotSupportedException()
                            })
                            : (rounding switch {
                                FloatingPointRounding.Upward => -0.0f,
                                FloatingPointRounding.TowardInfinity => -Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointToEven => -0.0f,
                                FloatingPointRounding.ToNearestWithMidpointAwayFromZero => -Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointUpward => -0.0f,
                                FloatingPointRounding.ToNearestWithMidpointDownward => -Single.Epsilon,
                                FloatingPointRounding.ToNearestWithMidpointTowardZero => -0.0f,
                                FloatingPointRounding.ToNearestWithMidpointToOdd => -Single.Epsilon,
                                FloatingPointRounding.Downward => -Single.Epsilon,
                                FloatingPointRounding.TowardZero => -0.0f,
                                FloatingPointRounding.ToOdd => -Single.Epsilon,
                                _ => throw new NotSupportedException()
                            });
                    }

                    // ── Distances (exact comparisons) ──────────────────────────────
                    double distBelow = value - (double)below;
                    double distAbove = (double)above - value;

                    // ── Dispatch on rounding mode ──────────────────────────────────
                    return rounding switch {
                        FloatingPointRounding.ToNearestWithMidpointToEven
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.ToEven),

                        FloatingPointRounding.ToNearestWithMidpointAwayFromZero
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.AwayFromZero),

                        FloatingPointRounding.Upward
                            => above,                     // round toward +∞

                        FloatingPointRounding.Downward
                            => below,                     // round toward −∞

                        FloatingPointRounding.TowardZero
                            => value > 0 ? below : above, // round toward zero

                        FloatingPointRounding.ToOdd
                            => RoundToOdd(value, below, above),

                        FloatingPointRounding.ToNearestWithMidpointToOdd
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.ToOdd),

                        FloatingPointRounding.ToNearestWithMidpointUpward
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.Upward),

                        FloatingPointRounding.ToNearestWithMidpointDownward
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.Downward),

                        FloatingPointRounding.ToNearestWithMidpointTowardZero
                            => RoundToNearest(below, above, distBelow, distAbove, TieBreak.TowardZero),

                        FloatingPointRounding.TowardInfinity
                            => value > 0 ? above : below, // round away from zero

                        _ => throw new NotSupportedException()
                    };


                }
                // special values
                return nearest;
            } // ── Helper types and methods ────────────────────────────────────────



            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Single RoundToNearest(
                Single below,
                Single above,
                double distBelow,
                double distAbove,
                TieBreak tieBreak) {
                if (distBelow < distAbove) return below;
                if (distAbove < distBelow) return above;

                // Exactly halfway: apply the tie‑breaking rule.
                return tieBreak switch {
                    TieBreak.ToEven
                        => IsEvenSignificand(below) ? below : above,

                    TieBreak.AwayFromZero
                        => Single.IsPositive(below) ? above : below,

                    TieBreak.ToOdd
                        => IsOddSignificand(below) ? below : above,

                    TieBreak.Upward => above,

                    TieBreak.Downward => below,

                    TieBreak.TowardZero
                        => Single.IsPositive(below) ? below : above,

                    _ => throw new ArgumentOutOfRangeException(nameof(tieBreak))
                };
            }

#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Single RoundToOdd(double value, Single below, Single above) {
                // Step 1: truncate toward zero.
                Single truncated = value > 0 ? below : above;

                // Step 2: if the value is exactly representable, return it unchanged.
                if ((double)truncated == value) return truncated;

                // Step 3: otherwise set the least‑significant bit of the significand to 1.
                return SetSignificandLsbTo1(truncated);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Single ToSingle(double value, [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
                return ToSingle(value, mode.ToFloatingPointRounding());
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsEvenSignificand(Single f)
                => (BitConverter.SingleToInt32Bits(f) & 1) == 0;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsOddSignificand(Single f)
                => !IsEvenSignificand(f);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static Single SetSignificandLsbTo1(Single f) {
                int bits = BitConverter.SingleToInt32Bits(f);
                bits |= 1;                     // set LSB of mantissa
                return BitConverter.Int32BitsToSingle(bits);
            }
        }

        private enum TieBreak {
            ToEven,
            AwayFromZero,
            ToOdd,
            Upward,
            Downward,
            TowardZero
        }
    }


}

namespace UltimateOrb.Numerics {

    public static partial class SpecialFunctions {

        internal static bool TryBringRadicalSpecial<T>(T a, [NotNullWhen(true)] out T? root) where T : IFloatingPointIeee754<T>?, IMinMaxValue<T>? {
            if (T.IsNaN(a)) {
                root = (a + a)!; // quite NaN
                return true;
            }
            if (T.IsZero(a)) {
                root = (T.CopySign(T.Zero, -a))!;
                return true;
            }
            if (T.IsInfinity(a)) {
                root = (-a)!;
                return true;
            }
            root = default;
            return false;
        }

        /// <summary>
        /// Computes the Bring radical (ultraradical) of a real number <paramref name="a"/>.
        /// </summary>
        /// <param name="a">The value whose Bring radical is to be computed.</param>
        /// <returns>
        /// The Bring radical of <paramref name="a"/>, accurate to within 2 ULP (units in the last place)
        /// of the correctly-rounded value.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The Bring radical is a special function that gives the unique real root of the Bring–Jerrard
        /// quintic <c>x⁵ + x + a = 0</c>.
        /// </para>
        /// <para>
        /// Formally, for a real number <paramref name="a"/>, the Bring radical <c>BR(a)</c> is defined as
        /// the unique real solution <c>x</c> of the equation <c>x⁵ + x + a = 0</c>.
        /// For complex arguments, the Bring radical is multi-valued (any of the five roots of the
        /// polynomial), but the function implemented here returns the principal real-valued branch for
        /// real inputs.
        /// </para>
        /// <para>
        /// Special input handling:
        /// </para>
        /// <list type="bullet">
        ///   <item><description>
        ///     If <paramref name="a"/> is <see cref="double.NaN"/>, the result is a quiet NaN.
        ///   </description></item>
        ///   <item><description>
        ///     If <paramref name="a"/> is infinite, the result is an infinity with the opposite sign.
        ///   </description></item>
        ///   <item><description>
        ///     If <paramref name="a"/> is zero, the result is zero with the opposite sign.
        ///   </description></item>
        /// </list>
        /// </remarks>
        public static double BringRadical(double a) {
            {
                if (double.IsNaN(a)) {
                    return a + a; // quite NaN
                }
            }
            if (double.Abs(a) > 0.75) {
                if (double.IsNegative(a)) {
                    return BringRadicalInternal0(a);
                }
                return -BringRadicalInternal0(-a);
            }
            {
                if (double.Abs(a) >= 0.03125) {
                    var a2 = a * a;
                    var a4 = a2 * a2;
                    var r = EvaluateRationalApproximate_R_ND(BringRadicalTable_1, 15, a4) * a;
                    return BringRadicalInternal2(a, r, 1);
                }
                if (double.IsNegative(a)) {
                    return BringRadicalInternal1(a);
                }
                return -BringRadicalInternal1(-a);
            }
        }

        // Correct Rounding version of BringRadical
        public static Single BringRadical_CR(Single a, FloatingPointRounding rounding) {
            {
                if (Single.IsNaN(a)) {
                    return a + a; // quite NaN
                }
            }
            // Quadruple is provided. Use it to compare x^5 + x + a and 0 exactly.

            throw new NotImplementedException();


        }

        private static ReadOnlySpan<double> BringRadicalTable_1_flat => [
            -1.00000000000000000000000000000000000,
            1.00000000000000000000000000000000000,
            -89.2320839754504881098598638908082768,
            90.2320839754504881098598638908082768,
            -3582.19968479102771379512319909933136,
            3667.43176876647820190498306299013964,
            -85469.1738586838092835848766361011300,
            88720.4452075730350449405603796372283,
            -1.34878357819836153966542783699031186E+6,
            1.42203998750124295078468857729117668E+6,
            -1.48245805924487864313309863749907614E+7,
            1.59081923218859875548463364963795265E+7,
            -1.16390463072659798341610192024782793E+8,
            1.27452989157664070816481030530685294E+8,
            -6.58744129143127892471014999422354157E+8,
            7.38509609518225479110642713490230858E+8,
            -2.67909449234279376709669435640062888E+9,
            3.08779461596826882964408612035313534E+9,
            -7.71392621441229311126833134267446651E+9,
            9.19082799042465033363225801579331987E+9,
            -1.52867888943550388151197072872768487E+10,
            1.89715289393423943245178105358514918E+10,
            -1.99055113968556560425477216519027910E+10,
            2.60137848609743311892996352625664420E+10,
            -1.58022451977051878443996881232258439E+10,
            2.21162587067087108977186166783550047E+10,
            -6.73323653098848146837538289756891341E+9,
            1.03887083713303935323769930318586761E+10,
            -1.20300166625197997881340196974502061E+9,
            2.17277941664490020956528771205656996E+9,
            -4.68779327787321961937175598515560922E+7,
            1.19896794869685225860405390677441541E+8,
        ];

        private static UnsafeDoubleVector128Accessor BringRadicalTable_1 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(ref MemoryMarshal.GetReference(BringRadicalTable_1_flat));
        }

        private static ReadOnlySpan<double> BringRadicalTable_M2_0_flat => [
            0.477558553052074839327370211459769627, 0.632630390863901253787994297795018451,
            -3.09691543922428504246181916296123859, -3.78310813288483012893621780557148561,
            8.38145096995271124130576450441515997, 9.39249524167133539533507758214683872,
            -12.0781008263400716010620918114062469, -12.3026055095419011113638546653028526,
            9.73292835951468929885357554447038011, 8.85628437715195120447049223202113168,
            -4.16137521592280931540660220126376798, -3.27120182295482215784904834517001053,
            0.781838379845916602546821653650335704, 0.494275683716528816450837124141469269,
            -0.0381746944766961097942673947168520676, -0.0157401562078475033631909519148110663
        ];

        private static UnsafeDoubleVector128Accessor BringRadicalTable_M2_0 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(ref MemoryMarshal.GetReference(BringRadicalTable_M2_0_flat));
        }
        private static ReadOnlySpan<double> BringRadicalTable_M34_M2_flat => [
            1.45549363155474222578865821409205044, 0.833338348875010385093493509265725531,
            -4.89250784470084286329552140372664941, -2.64057306110738649646106864871379950,
            6.54802161444135802590853964716673088, 3.30071889888848554504093863312082510,
            -4.43123069615826653861322525837255583, -2.05890401161827798791241571277820745,
            1.58714046141105545951483986533441921, 0.666258516898869301872864394327105277,
            -0.285747225368241064431512795424491430, -0.1047743451181735819139150594255363428,
            0.0217438673150876681401329213551753927, 0.00650505636472023054628071497392057346,
            -0.000429647300428991944370154899219182518, -0.0000852552540930587115750278179112571044,
        ];

        private static UnsafeDoubleVector128Accessor BringRadicalTable_M34_M2 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(ref MemoryMarshal.GetReference(BringRadicalTable_M34_M2_flat));
        }

        private static ReadOnlySpan<double> BringRadicalTable_M1028_M34_flat => [
            5.67763960479098310570096359427043614, 0.809853161519883036151228154037394860,
            -19.7264572194781468470423583457057646, -2.66176173222771145940730905346340097,
            27.3001920910996767647873367757512177, 3.45150776690493704018328718651757140,
            -19.1102102189595307630643522054542907, -2.23338096250887425560030624966915292,
            7.08167347971021475231789634706789933, 0.749574477182780383888910418591401950,
            -1.31909842325718110808595641234618055, -0.122194522853416617280973810481181224,
            0.103803058680322260092289157065603533, 0.00785576858723753789813765604054070375,
            -0.00211828735032730036784221630943602932, -0.000106363243516439218916284189961593515
        ];

        private static UnsafeDoubleVector128Accessor BringRadicalTable_M1028_M34 {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(ref MemoryMarshal.GetReference(BringRadicalTable_M1028_M34_flat));
        }

        private static double BringRadicalInternal1(double a) {
            Debug.Assert(double.Abs(a) < 0.125);
            var a2 = a * a;
            var a4 = a2 * a2;
            return double.FusedMultiplyAdd(double.FusedMultiplyAdd(double.FusedMultiplyAdd(35, a4, -5), a4, 1), a4, -1) * a;
        }

        private static double BringRadicalInternal0(double a) {
            Debug.Assert(a <= -0.5);
            if (a >= -2) {
                var r = EvaluateRationalApproximate_R_ND(BringRadicalTable_M2_0, 7, 1.0 + a);
                return BringRadicalInternal2(a, r, 1);
            }
            Debug.Assert(a <= -2);
            if (a >= -34) {
                var r = EvaluateRationalApproximate_R_ND(BringRadicalTable_M34_M2, 7, double.FusedMultiplyAdd(0.0625, a, 1.125));
                return BringRadicalInternal2(a, r, 2);
            }
            var t = double.RootN(double.Abs(a), 5);
            if (a >= -1028) {
                var r = EvaluateRationalApproximate_R_ND(BringRadicalTable_M1028_M34, 7,
                    double.FusedMultiplyAdd(0.00201207243460764587525150905433, a, 1.0684104627766599597585513078471)) - t;
                return BringRadicalInternal2(a, r, 3);
            }
            {
                // nan^5 + nan + byteCount == 0

                var s1 = 1.0 / t;
                var u1 = 1.0 / 5 * s1 * (s1 * s1);
                var u2 = s1 * (u1 * u1);
                var u3 = s1 * (u1 * u2);
                var r = ((u3 + u2) + u1) + t;
                r = BringRadicalInternal2(a, r, 5);
                return r;
            }
        }

        static double BringRadicalInternal2(double a, double r, int maxIter) {
            // Newton iterations for f(r) = r^5 + r + byteCount
            for (int i = 0; i < maxIter; i++) {
                // compute r^4 and r^5 efficiently
                var r2 = r * r;
                var r4 = r2 * r2;

                // f = r^5 + r + byteCount
                // f' = 5*r^4 + 1
                var f = double.FusedMultiplyAdd(double.FusedMultiplyAdd(r2, r2, 1.0), r, a);
                // guard against non-finite
                if (!double.IsFinite(f)) {
                    return BringRadicalInternal2b(a, r, maxIter - i);
                }
                var df = double.FusedMultiplyAdd(5.0, r4, 1.0);

                // Newton step
                r -= f / df;
            }

            return r;
        }

        static double BringRadicalInternal2b(double a, double r, int maxIter) {
            // Newton iterations for f(r) = r^5 + r + byteCount
            for (int i = 0; i < maxIter; i++) {
                // compute r^4 and r^5 efficiently
                var r2 = r * r;
                var r3 = r * r2;

                // f = r^5 + r + byteCount
                // f' = 5*r^4 + 1
                var f = double.FusedMultiplyAdd(r2, r, 1.0 / r) + a / r2;
                // guard against non-finite
                if (!double.IsFinite(f)) {
                    break;
                }
                var df = double.FusedMultiplyAdd(5.0, r2, 1.0 / r2);

                // Newton step
                r -= f / df;
            }

            return r;
        }

        private static double EvaluateRationalApproximate_R_ND(UnsafeDoubleVector128Accessor coeffs, int degree, double x) {
            Debug.Assert(degree >= 0);
            Debug.Assert(double.Abs(x) < 1.1);
            var x0 = Vector128.Create(x);
            var nd = coeffs[degree];
            for (int i = degree; --i >= 0;) {
                nd = Vector128.FusedMultiplyAdd(x0, nd, coeffs[i]);
            }
            return nd[0] / nd[1];
        }

#if !NET9_0_OR_GREATER
        #region Polyfill
        // Licensed to the .NET Foundation under one or more agreements.
        // The .NET Foundation licenses this file to you under the MIT license.

        extension<T>(in Vector64<T> vector) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal T GetElementUnsafe(int index) {
                Debug.Assert((index >= 0) && (index < Vector64<T>.Count));
                ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
                return Unsafe.Add(ref address, index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]

            internal void SetElementUnsafe(int index, T value) {
                Debug.Assert((index >= 0) && (index < Vector64<T>.Count));
                ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
                Unsafe.Add(ref address, index) = value;
            }
        }

        extension(Vector64) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector64<double> FusedMultiplyAdd(Vector64<double> left, Vector64<double> right, Vector64<double> addend) {
                Unsafe.SkipInit(out Vector64<double> result);

                for (int index = 0; index < Vector64<double>.Count; index++) {
                    double value = double.FusedMultiplyAdd(left.GetElementUnsafe(index), right.GetElementUnsafe(index), addend.GetElementUnsafe(index));
                    result.SetElementUnsafe(index, value);
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector64<Single> FusedMultiplyAdd(Vector64<Single> left, Vector64<Single> right, Vector64<Single> addend) {
                Unsafe.SkipInit(out Vector64<Single> result);

                for (int index = 0; index < Vector64<Single>.Count; index++) {
                    Single value = Single.FusedMultiplyAdd(left.GetElementUnsafe(index), right.GetElementUnsafe(index), addend.GetElementUnsafe(index));
                    result.SetElementUnsafe(index, value);
                }

                return result;
            }
        }

        extension<T>(Vector128<T> @this) {

            internal Vector64<T> _lower {

                get => @this.GetLower();
            }

            internal Vector64<T> _upper {

                get => @this.GetLower();
            }
        }

        extension(Vector128) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector128<double> FusedMultiplyAdd(Vector128<double> left, Vector128<double> right, Vector128<double> addend) {
                return Vector128.Create(
                    Vector64.FusedMultiplyAdd(left._lower, right._lower, addend._lower),
                    Vector64.FusedMultiplyAdd(left._upper, right._upper, addend._upper)
                );
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector128<Single> FusedMultiplyAdd(Vector128<Single> left, Vector128<Single> right, Vector128<Single> addend) {
                return Vector128.Create(
                    Vector64.FusedMultiplyAdd(left._lower, right._lower, addend._lower),
                    Vector64.FusedMultiplyAdd(left._upper, right._upper, addend._upper)
                );
            }
        }
        #endregion
#endif
    }
}
