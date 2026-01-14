#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using System;
/*
namespace UltimateOrb.Numerics.Specialized {

    [AttributeUsage(AttributeTargets.Struct)]
    public class GenerateFixedDecimal32Attribute : Attribute {

        public int ExponentBias { get; }

        public GenerateFixedDecimal32Attribute(int exponentBias) {
            ExponentBias = exponentBias;
        }
    }
}
*/
using UltimateOrb.Numerics.Extensions;

namespace UltimateOrb.Core.Tests {
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;
    using System.Text.RegularExpressions;
    using System.Threading;
    using UltimateOrb.Mathematics.Exact;
    using UltimateOrb.Mathematics.Geometry;
    using UltimateOrb.Numerics;
    using UltimateOrb.Numerics.Specialized;
    using UltimateOrb.Plain.ValueTypes;
    using UltimateOrb.Utilities.InterfaceExtensions.System;

    // using UltimateOrb.Runtime.CompilerServices.Tests;

    public static partial class WhitespaceRemover {

        // This attribute tells the compiler to generate efficient regex code at compile time.
        [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
        private static partial Regex WhitespaceRegex();

        /// <summary>
        /// Removes all Unicode whitespace characters from the input string.
        /// </summary>
        /// <param name="input">The string from which to remove whitespace.</param>
        /// <returns>The string without any whitespace characters.</returns>
        [return: NotNullIfNotNull(nameof(input))]
        public static string? RemoveWhitespace(this string? input) {
            if (string.IsNullOrEmpty(input)) {
                return input;
            }

            return WhitespaceRegex().Replace(input, "");
        }
    }

    internal static class Win32DecimalHelpers {

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_flags")]
        internal static extern ref readonly Int32 GetFlagsInternal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_hi32")]
        internal static extern ref readonly UInt32 GetHigh32Internal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lo64")]
        internal static extern ref readonly UInt64 GetLow64Internal(this in decimal dec);
    }

    [UltimateOrb.Numerics.Specialized.GenerateFixedDecimal32(3)]
    public readonly partial struct MilliDecimal {

    }


    internal static class Program {


        private static int aasfd() {
            var rr = new Random();
            var a = 0;

            for (var i = 0; 100 > i; ++i) {

                var asdf = rr.Next(0x8000);
                var b = new byte[asdf];
                a ^= b.GetHashCode();
            }

            for (var i = 0; GC.MaxGeneration > i; ++i) {
                if (3 > rr.Next(100)) {
                    GC.Collect();
                }
            }
            return a;
        }

        static readonly int ff = 33;

        static int ff2 = 33;
        static CanonicalIntegerBoolean ff3;
        static double aaffaa = -3.0;

        const int dssaf = 16 * 1024 * 1024;

        [StructLayout(LayoutKind.Sequential, Size = dssaf)]
        struct Bits8388608 {
            internal unsafe fixed UInt64 b[dssaf / sizeof(UInt64)];

        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static T UInt128ConversionTest1<T>(decimal a) where T : INumberBase<T> {
            return T.CreateChecked(a);
        }


        struct asfa : ICloneableDerivedByNongeneric<asfa> {
            public int a;
            public int b;

            public object Clone() {
                var t = this;
                ++b;
                return t;
            }

            public override string ToString() {
                return (a, b).ToString();
            }
        }
        static T Clone1<T>(ref T @this) where T : struct, ICloneable<T> {
            return @this.Clone();
        }

        static T Clone1b<T>(ref T @this) where T : ICloneable<T> {
            return @this.Clone();
        }

        static T Clone0<T>(ref T @this) where T : struct, ICloneable {
            return (T)@this.Clone();
        }
        static T Clone0a<T>(T @this) where T : struct, ICloneable {
            return (T)@this.Clone();
        }
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static int Main(string[] args) {



            {
                Console.WriteLine($"Scale10(0, -7) = {Decimal128Bid.Scale10(0, -7)}");
                Console.WriteLine($"Scale10(0, +7) = {Decimal128Bid.Scale10(0, +7)}");
                Console.WriteLine($"Scale10(0, +7000) = {Decimal128Bid.Scale10(0, +7000)}");
                Console.WriteLine($"Scale10(0, -7000) = {Decimal128Bid.Scale10(0, -7000)}");

                Console.WriteLine($"IsNegative(-0) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("-0"))}");


                Decimal128Bid[] testData1 = [
                    Decimal128Bid.Parse("+qNaN(0x4000000000000000000000000009)"),
                    Decimal128Bid.Parse("+qNaN(0X3ffffffffffffffffffffffffffF)"),
                    Decimal128Bid.Parse("sNaN(111)"),
                    Decimal128Bid.Parse("qNaN(222)"),
                    Decimal128Bid.Parse("NaN(333)"),
                    Decimal128Bid.Parse("+sNaN(444)"),
                    Decimal128Bid.Parse("+qNaN(555)"),
                    Decimal128Bid.Parse("+NaN(666)"),
                    Decimal128Bid.Parse("-sNaN(777)"),
                    Decimal128Bid.Parse("-qNaN(888)"),
                    Decimal128Bid.Parse("-NaN(999)"),

                    Decimal128Bid.Parse("+inF"),
                    Decimal128Bid.Parse("-∞"),
                    Decimal128Bid.Parse("Infinity"),
                    Decimal128Bid.Parse("1919810"),
                    Decimal128Bid.ToCoarsestCohort(Decimal128Bid.Parse("1919810")),
                    Decimal128Bid.ToFinestCohort(Decimal128Bid.Parse("1919810")),

                    Decimal128Bid.Parse("-10100"),
                    Decimal128Bid.ToCoarsestCohort(Decimal128Bid.Parse("-10100")),
                    Decimal128Bid.ToFinestCohort(Decimal128Bid.Parse("-10100")),
                    Decimal128Bid.ToCohort(Decimal128Bid.Parse("-10100"), qExponent: 1),
                    Decimal128Bid.Pi,
                    Decimal128Bid.Tau,
                    Decimal128Bid.Epsilon,
                    -Decimal128Bid.Epsilon,
                    -Decimal128Bid.AdditiveIdentity,
                    Decimal128Bid.AdditiveIdentity,
                    Decimal128Bid.BitDecrement(Decimal128Bid.Epsilon),
                    Decimal128Bid.BitIncrement(Decimal128Bid.Epsilon),
                    Decimal128Bid.BitDecrement(-Decimal128Bid.Epsilon),
                    Decimal128Bid.BitIncrement(-Decimal128Bid.Epsilon),

                    Decimal128Bid.Parse("-0"),
                    Decimal128Bid.Parse("+0"),
                    Decimal128Bid.Parse("-.0E-9000"),

                    Decimal128Bid.Parse("-∞"),
                    Decimal128Bid.Parse("Infinity"), ];

                foreach (var item in testData1.OrderBy(x => x, new TotalOrderIeee754Comparer<Decimal128Bid>())) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(Decimal128Extensions.TotalOrderIeee754_192BitsKeySelector)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(x => x)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(Decimal128Extensions.TotalOrderDefaultSystemInt128KeySelector)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            {
                Decimal128Bid.IsZero(Decimal128Bid.Parse("+sNaN"));
                Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+sNaN"));
                Console.WriteLine($"IsSignalingNaN(+sNaN) = {Decimal128Bid.IsSignalingNaN(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsQuietNaN(+sNaN) = {Decimal128Bid.IsQuietNaN(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsNegative(+sNaN) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsSignalingNaN(+qNaN(0x4243)) = {Decimal128Bid.IsSignalingNaN(Decimal128Bid.Parse("+qNaN(0x4243)"))}");
                Console.WriteLine($"IsQuietNaN(+qNaN(0x4243)) = {Decimal128Bid.IsQuietNaN(Decimal128Bid.Parse("+qNaN(0x4243)"))}");
                Console.WriteLine($"IsNegative(+qNaN(0x4243)) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("+qNaN(0x4243)"))}");

                Console.WriteLine($"Decimal128Bid(1919810) = {(Decimal128Bid)1919810}");
                Console.WriteLine($"CoarsestCohort(1919810) = {Decimal128Bid.ToCoarsestCohort(1919810)}");
                Console.WriteLine($"FinestCohort(1919810) = {Decimal128Bid.ToFinestCohort(1919810)}");

            }
            {
                var comparer = new TotalOrderIeee754Comparer<Decimal128Bid>();

                Console.WriteLine($"TotalOrderIeee754(+sNaN, +qNaN(0x4243)) = {int.Sign(comparer.Compare(
                    Decimal128Bid.Parse("+sNaN"), Decimal128Bid.Parse("+qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(+sNaN, +qNaN(0x4243)) = {int.Sign(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+sNaN")).CompareTo(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+qNaN(0x4243)"))))}");

                Console.WriteLine($"TotalOrderIeee754(sNaN, qNaN(0x4243)) = {int.Sign(comparer.Compare(
                    Decimal128Bid.Parse("sNaN"), Decimal128Bid.Parse("qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(sNaN, qNaN(0x4243)) = {int.Sign(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("sNaN")).CompareTo(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("qNaN(0x4243)"))))}");

                Console.WriteLine($"TotalOrderIeee754(-sNaN, -qNaN(0x4243)) = {int.Sign(comparer.Compare(Decimal128Bid.Parse("-sNaN"), Decimal128Bid.Parse("-qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(+sNaN(10), +sNaN(0X10)) = {int.Sign(comparer.Compare(Decimal128Bid.Parse("+sNaN(10)"), Decimal128Bid.Parse("+sNaN(0X10)")))}");

            }
            {
                Console.WriteLine($"{Decimal128Bid.Parse("7.102030405E13")}");
                Console.WriteLine($"{Decimal128Bid.Parse("sNaN")}");
                Console.WriteLine($"{Decimal128Bid.Parse("qNaN(0x4243)")}");
                Console.WriteLine($"{Decimal128Bid.Parse("Inf")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-Inf")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-0E-444")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-1000E-444")}");
                Console.WriteLine();

            }
            {
                Console.WriteLine($"{BigRational.Parse("7.102030405E13", null)}");

                Console.WriteLine($"{BigRational.Parse(".1", null)}");

                var sdfs = BigRational.Parse("423123.23423423567657657657567657657657657991112E-32", null);

                Console.WriteLine($"{sdfs}");
                Console.WriteLine();

            }
            {
                Console.WriteLine($"default(Decimal128) = {default(Decimal128)}");
                Console.WriteLine($"Decimal128Bid(Double.MaxValue) = {(Decimal128Bid)double.MaxValue}");




                Console.WriteLine($"Decimal128(-0E-3M) = {(Decimal128Bid)(-0E-3M)}");
                Console.WriteLine($"Quantum(Decimal128(-0E-900M)) = {Decimal128Bid.Quantum((Decimal128Bid)(-0E-900M))}");

                Console.WriteLine($"Zero - 0E-3 = {Decimal128Bid.Zero - Decimal128Bid.Scale10(0, -3)}");
                Console.WriteLine($"Zero + AdditiveIdentity = {Decimal128Bid.Zero + Decimal128Bid.AdditiveIdentity}");

                Console.WriteLine($"Zero = {Decimal128Bid.Zero}");
                Console.WriteLine($"NegativeZero = {Decimal128Bid.NegativeZero}");
                Console.WriteLine($"AdditiveIdentity = {Decimal128Bid.AdditiveIdentity}");
                Console.WriteLine($"BitIncrement({-Decimal128Bid.Epsilon}) = {Decimal128Bid.BitIncrement(-Decimal128Bid.Epsilon)}");

                Console.WriteLine($"BitDecrement({Decimal128Bid.Epsilon}) = {Decimal128Bid.BitDecrement(Decimal128Bid.Epsilon)}");
                Console.WriteLine($"BitDecrement(-1) = {Decimal128Bid.BitDecrement(-1)}");
                Console.WriteLine($"BitDecrement({Decimal128Bid.MinValue}) = {Decimal128Bid.BitDecrement(Decimal128Bid.MinValue)}");

                Console.WriteLine($"AtanPi({Decimal128Bid.MaxValue}) = {Decimal128Bid.AtanPi(Decimal128Bid.MaxValue)}");
                Console.WriteLine($"AtanPi({Decimal128Bid.NegativeInfinity}) = {Decimal128Bid.AtanPi(Decimal128Bid.NegativeInfinity)}");
                Console.WriteLine($"AtanPi(1) = {Decimal128Bid.AtanPi(1)}");
                Console.WriteLine($"Atan(-1E-10M) = {Decimal128Bid.Atan(-1E-10M)}");
                Console.WriteLine();

                Console.WriteLine($"Decimal128(0.1D) = {(Decimal128Bid)0.1D}");
                Console.WriteLine($"Decimal128(0.1M) = {(Decimal128Bid)0.1M}");
                Console.WriteLine($"Atan(0.1D) = {Decimal128Bid.Atan(0.1D)}");
                Console.WriteLine($"Atan(0.1M) = {Decimal128Bid.Atan(0.1M)}");
                Console.WriteLine();

                for (Decimal128Bid x = 0.1M; x <= 0.9M; x += 0.1M) {
                    Console.WriteLine($"Atan({x}) = {Decimal128Bid.Atan(x)}");
                }

                Console.WriteLine();
                Console.WriteLine($"4 * Atan(1) = {4 * Decimal128Bid.Atan(1)}");
                Console.WriteLine($"Pi = {Decimal128Bid.Pi}");
                return 0;
            }

            {
                Console.WriteLine(BigRational.Math.ILog10((BigRational)9.9));
                Decimal128Bid a = Decimal128Bid.Pi;
                var b = (BigRational)a;
                var dsfa = (Decimal128Bid)b + Decimal128Bid.One;
                var d = (BigRational)dsfa;
                var dd = Decimal128Bid.Tau / 2;
                var rr = (BigRational)dd;
                var dd1 = -Decimal128Bid.MaxValue % Decimal128Bid.Tau;
                var rTau = (BigRational)(-Decimal128Bid.Tau);
                Console.WriteLine(rTau);
                var rr1 = (BigRational)dd1;
                var xx1 = (double)rr1;
                Console.WriteLine(Decimal128Bid.Epsilon);
                Console.WriteLine(Decimal128Bid.One / 1000);
                Console.WriteLine(Decimal128Bid.Epsilon * 1000);
                Console.WriteLine(Decimal128Bid.Epsilon * 1001);
                Console.WriteLine(Decimal128Bid.Epsilon / 2);
                Console.WriteLine(Decimal128Bid.Epsilon / 1.99999999);

                Console.WriteLine(Decimal128Bid.Scale10(100000000, -7));


                Console.WriteLine((Decimal128Bid)801E5M); // System.Decimal does not have strictly positive qExponent
                Console.WriteLine(Decimal128Bid.Scale10(801, 5));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / 10);
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(1, 1));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(100, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(100000000, -7));

                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / 10);
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(1, 1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(100, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(100000000, -7));





                Console.WriteLine((Decimal128Bid)1234D);
                Console.WriteLine((Decimal128Bid)1234E7D);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D}");
                Console.WriteLine((Decimal128Bid)1234E7D / 10);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 10}");
                Console.WriteLine((Decimal128Bid)1234E7D / 1E1M);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 1E1M}");
                Console.WriteLine((Decimal128Bid)1234E7D / 100E-1M);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 100E-1M}");

                Console.WriteLine((Decimal128Bid)(-1.25D));
                Console.WriteLine((Decimal128Bid)4.2D);
                Console.WriteLine((Decimal128Bid)(-4.20M));

                Console.WriteLine((Decimal128Bid)1 / 0.1M);
                Console.WriteLine((Decimal128Bid)1 % 0.1M);

                Console.WriteLine((Decimal128Bid)1 / 0.1D);
                Console.WriteLine((Decimal128Bid)1 % 0.1D);

                Console.WriteLine(Decimal128Bid.Scale10(-9.05M, -33));
                Console.WriteLine((1 + Decimal128Bid.Scale10(1, -32)) * (1 - Decimal128Bid.Scale10(1, -33)));

                Console.WriteLine(Decimal128Bid.FusedMultiplyAdd(1 + Decimal128Bid.Scale10(1, -32), 1 - Decimal128Bid.Scale10(1, -33), Decimal128Bid.Scale10(-9.05M, -33)));
                Console.WriteLine(Decimal128Bid.FusedMultiplyAdd(1 + Decimal128Bid.Scale10(1, -32), 1 - Decimal128Bid.Scale10(1, -33), Decimal128Bid.Scale10(-9.049999999999M, -33)));
                Console.WriteLine(Decimal128Bid.Hypot(3, 4));
                Console.WriteLine(Decimal128Bid.Hypot(30, 40));
                Console.WriteLine(Decimal128Bid.Hypot(0, 1));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.Scale10(1, 1), 1));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(9, 6127)));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(10, 6127)));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34)));
                Console.WriteLine(Decimal128Bid.Scale10(10, 6127));
                Console.WriteLine(Decimal128Bid.Scale10(1, 6128));
                Console.WriteLine(Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34));
                Console.WriteLine(Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34) + Decimal128Bid.Scale10(1, 6127 - 33));

                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6176));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6176)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6143)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) - Decimal128Bid.Scale10(1, -6176));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6143) - Decimal128Bid.Scale10(1, -6176)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(9, -6144));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(9, -6144)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6144));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6144)):x32}");

                Console.WriteLine(Decimal128Bid.Scale10(1, -6143));
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143)));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * Decimal128Bid.Scale10(1, -100));
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * Decimal128Bid.Scale10(1, -100)));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * .95D);
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * .95D));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * .95M);
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * .95M));


                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6177));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6144));

                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6179));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6146));


                Console.WriteLine((double)Decimal128Bid.MaxValue);


                return 0;
            }






            {
                var sdfad = Rational64.FromFraction(27, -512);
                var sdfasdad = Rational64.Cbrt(sdfad);
                Console.WriteLine(sdfasdad);
            }
            {

                var v1 = Unsafe.BitCast<Int64, Long<int, uint>>(0X0700002000000100L);
                var v2 = Unsafe.BitCast<Int64, Long<int, uint>>(0X0001001000000100L);
                var v3 = v1 | v2;
                Console.WriteLine(Unsafe.BitCast<Long<int, uint>, Int64>(v3));

            }
            if (false) {
                asfa sadfa = new asfa() { a = 1919810, b = 114514 };
                Console.WriteLine($"{nameof(sadfa)} = {sadfa}");
                var t = Clone1(ref sadfa);
                Console.WriteLine($"{nameof(t)} = {t}");
                Console.WriteLine($"{nameof(sadfa)} = {sadfa}");
                Console.WriteLine($"===");
                var t2 = Clone1b(ref sadfa);
                Console.WriteLine($"{nameof(t2)} = {t2}");



                return 0;
            }

            {
                var d = (decimal)BigRational.FromFraction(22, 7);
                Console.WriteLine(d);
                var d1 = (decimal)(double)BigRational.FromFraction(22, 7);
                Console.WriteLine(d1);

                Console.WriteLine((BigRational)d);

                return 0;
            }


            {

                Console.WriteLine(new BitMatrix16x16(Vector256<UInt16>.One) << 9.AsRowShiftCount());
                return 0;
            }
            {

                Console.WriteLine((BigRational)Math.PI);
                Console.WriteLine(BigRational.FromDoubleByContinuedFraction(Math.PI));


                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromRationalByContinuedFraction((BigRational)Math.PI, i));

                }
                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromRationalByContinuedFraction((BigRational)0.1, i));

                }
                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromDoubleByContinuedFraction(0.1, i));

                }
                Console.WriteLine(BigRational.FromDoubleByContinuedFraction(float.PositiveInfinity));

                return 0;
            }
            {


                string output = $$$"""{{{3:V:<s:aaa>}}}""";
                Console.WriteLine(output);
                return 0;
            }
            {
                {
                    var a = """
                194286934960954013505219131142675643538134782692201101987417450051523093244750612407244445269834128729581999001097698837
                059113628401747382012597604790968895348450733898006365028455976752552318787766900421989105252601990430439871601888440842
                224149126597339343184859242253914800956534425991653876968765566900786049362862971570561111/11588945817616684075008826067
                085048249736465095545485270761101056576578952644709576992975689593185437220926517277187699610446278876498377104974586240
                406864862576323161803360152645281736589031288050365377966313964390654862478447629844776743834403928717405761405728126068
                257954320624252883485392554162544380618209422131790230847488
                """;
                    a = a.RemoveWhitespace();
                    var b = a.Split('/', 2, StringSplitOptions.TrimEntries);
                    BigInteger n;
                    var m = BigInteger.One;
                    if (b.Length == 1) {
                        n = BigInteger.Parse(b[0]);
                    } else {
                        n = BigInteger.Parse(b[0]);
                        m = BigInteger.Parse(b[1]);
                    }
                    var p = BigRational.FromFraction(n, m);
                    Console.WriteLine(p);
                    Console.WriteLine((double)p);

                    return 0;

                }

            }
            {
                var sdfasdf = 34325451245454352453523453453.735354345m;
                sdfasdf /= 100m;
                Console.WriteLine($@"{sdfasdf.GetFlagsInternal():R} {sdfasdf.GetHigh32Internal():R} {sdfasdf.GetLow64Internal():R}");
                Console.WriteLine($@"{sdfasdf:R}");
                var aaa = UInt128ConversionTest1<UltimateOrb.UInt128>(sdfasdf);

                Console.WriteLine($@"{aaa:R}");

                return 0;
            }
            {

                var t = new Rational64ExactTests();
                for (long i = 0; i < 330000000; i++) {
                    t.FractionPart_OfIntegersIsZero((uint)i);
                }
                return 0;

            }


            {
                var s = 0;
                for (var i = 0L; 1_000_000_000_000 > i; ++i) {
#pragma warning disable UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    var sdfs = DoubleArithmetic.BigMulUnsigned<uint>(0x40000001, 0x00030002, out var aa);
#pragma warning restore UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    s += sdfs.GetHashCode();
                    s ^= aa.GetHashCode();

                }
                Console.WriteLine(s);
            }


            {
                Console.WriteLine(RuntimeEnvironment.GetSystemVersion());

                var netCoreVer = System.Environment.Version;
                Console.WriteLine(netCoreVer);

                var runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
                Console.WriteLine(runtimeVer);

                Console.WriteLine(MilliDecimal.MaxValue);

                // UnsafeTests.UnboxNullableTest();
                return 0;
            }
            {
                for (uint i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MaxValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = uint.MaxValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;

            }
            {


                for (var i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MaxValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;




            }
            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        p0 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        unsafe {
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 1] = 11000000 + (UInt64)i;
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 2] = 22000000 + (UInt64)i;

                        }


                        if (i == 7) {
                            p1 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 77) {
                            p2 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 777) {
                            p3 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 7777) {
                            p4 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;




            }

            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        p0 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        affffffffs[^1] = 11000000 + (UInt64)i;
                        affffffffs[^2] = 22000000 + (UInt64)i;

                        if (i == 7) {
                            p1 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 77) {
                            p2 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 777) {
                            p3 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 7777) {
                            p4 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;
            }
            {
                {
                    Console.WriteLine(Math.ILogB(-3));
                    Console.WriteLine(Math.ScaleB(double.MaxValue, 1000000000));


                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        Console.WriteLine(Math.ILogB(Math.Abs(Volatile.Read(ref aaffaa))));
                    }
                    aaaa();
                    aaaa();

                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var afa = new UltimateOrb.Numerics.QuaternionD(101, 102, 103, (double)104);
                        Console.WriteLine(afa.X);
                    }
                    aaaa();
                    aaaa();
                }
            }
            {

                if (false) {
                    Console.WriteLine($@"{new Vector3D(4, 3, 5).GetNormalizedSafe()}");
                    Console.WriteLine($@"{new Vector3D(1, 1, 1).GetNormalizedSafe()}");
                    {
                        var aa = new Vector3D(4 * double.Epsilon, 3 * double.Epsilon, 5 * double.Epsilon);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.MaxValue);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, 1);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.NaN);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    return 0;

                }


                const bool Extrinsic = false;
                const bool Intrinsic = true;
                System.Numerics.Vector3 X = new System.Numerics.Vector3(1, 0, 0);
                System.Numerics.Vector3 Y = new System.Numerics.Vector3(0, 1, 0);
                System.Numerics.Vector3 Z = new System.Numerics.Vector3(0, 0, 1);

                static System.Numerics.Matrix4x4 M0(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, double angle0, double angle1, double angle2) {
                    var t0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis0, angle0.ToStandardF());
                    var t1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis1, angle1.ToStandardF());
                    var t2 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis2, angle2.ToStandardF());
                    return isIntrinsic ? t2 * t1 * t0 : t0 * t1 * t2;
                }

                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.SystemNumericsExtensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var m0 = aa.ToStandardF();
                        UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(
                            x, y, z, a,
                            out var a1, out var a2, out var a3);
                        var m1 = M0(Intrinsic, X, Y, Z, a1, a2, a3);


                        var d = m0 - m1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 3.5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;
                }
                if (false) {




                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        new System.Numerics.Vector3(0, 0, 1),
                        MathF.PI / 3.0F);
                    Console.WriteLine(m0);
                    return 0;

                }


                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToExtrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;

                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }


                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.SystemNumericsExtensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var a0 = aa.ToStandardF();
                        var a1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                            System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x, y, z)), a);

                        var d = a0 - a1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 7e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 12e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;





                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }
                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);

                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);

                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }











                    /*
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }*/
                    return 0;
                }




                {


                    static System.Numerics.Matrix4x4 M1(Converter<Vector3D, Vector4D> converter, double angle0, double angle1, double angle2) {
                        var s = converter(new Vector3D(angle0, angle1, angle2));
                        var t = System.Numerics.Matrix4x4.CreateFromAxisAngle(s.E012.GetNormalized().ToStandardF(), s.E3.ToStandardF());
                        return t;
                    }

                    static int sss(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, Converter<Vector3D, Vector4D> converter) {
                        var m0 = (double angle0, double angle1, double angle2)
                            => M0(isIntrinsic, axis0, axis1, axis2, angle0, angle1, angle2);
                        var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                        var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                        var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                        var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                        var rr = new Random();

                        var eMax = float.NaN;
                        var e1Max = float.NaN;
                        for (var i = 0; 10_000_000 > i; ++i) {
                            var angle0 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle1 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle2 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var a0 = m0(angle0, angle1, angle2);
                            var a1 = M1(converter, angle0, angle1, angle2);

                            var d = a0 - a1;
                            var dx = System.Numerics.Vector4.Transform(vx, d);
                            var dy = System.Numerics.Vector4.Transform(vy, d);
                            var dz = System.Numerics.Vector4.Transform(vz, d);
                            var dw = System.Numerics.Vector4.Transform(vw, d);
                            var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                            var e1 = dw.LengthSquared();
                            if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                                eMax = e;
                            }
                            if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                                e1Max = e1;
                            }
                            if (e >= 3.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }
                            if (e1 >= 5.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }

                        }

                        Console.WriteLine("...");
                        Console.WriteLine(eMax);
                        Console.WriteLine(e1Max);
                        Console.WriteLine();
                        return 0;
                    }




                    if (true) {

                        sss(Extrinsic, X, Y, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXYZEulerAngles);
                        sss(Intrinsic, X, Y, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicXYZEulerAngles);
                        sss(Extrinsic, X, Z, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXZYEulerAngles);
                        sss(Intrinsic, X, Z, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicXZYEulerAngles);
                        sss(Extrinsic, Y, X, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicYXZEulerAngles);
                        sss(Intrinsic, Y, X, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYXZEulerAngles);
                        sss(Extrinsic, Y, Z, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicYZXEulerAngles);
                        sss(Intrinsic, Y, Z, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYZXEulerAngles);
                        sss(Extrinsic, Z, X, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZXYEulerAngles);
                        sss(Intrinsic, Z, X, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicZXYEulerAngles);
                        sss(Extrinsic, Z, Y, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles);
                        sss(Intrinsic, Z, Y, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicZYXEulerAngles);



                        return 0;
                    }
                }


                {
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    var q = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{rb}");
                    Console.WriteLine($@"{p}");
                    Console.WriteLine($@"{q}");
                    // p |--> p . rb


                }
                {


                    var ra = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 1, 0), 0.5F * MathF.PI);
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    p = System.Numerics.Vector4.Transform(p, ra);
                    p = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{p}");
                }


                {
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI));
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(2, 0, 0), 0.5F * MathF.PI)); // incorrect


                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXYZEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYZXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdaf1 = new Vector4D(3, 3, 3, 3);
                        var sdaf2 = new Vector4D(7, 17, 27, 37);
                        var sdaf = sdaf1 + sdaf2;
                        Console.WriteLine(sdaf);
                    }
                    aaaa();
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensionsD.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensions.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var m = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(0.9F, -1.3F, 0.6F);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());

                }




            }
            {
                Thread.MemoryBarrier();
                ff2 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff2 = unchecked((int)(byte)ff);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = Unsafe.As<int, CanonicalIntegerBoolean>(ref Unsafe.AsRef(ff));
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = true;
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = false;
                Thread.MemoryBarrier();
                System.Diagnostics.Debugger.Break();
                var sds12 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Console.WriteLine($@"{sds12}");
                var sds = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(3, 7);
                Console.WriteLine($@"{sds}");
                var sds1 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(1, 0);
                Console.WriteLine($@"{sds1}");
                return 0;
            }
            {

                UltimateOrb.Numerics.Generic.Long<Int64, UInt64> ggg = new(3, 6);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);

                return 0;
            }
            {
                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }

                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }
            }

            {

                var threads = new Thread[10];
                var a = true;
                for (int i = 0; i < threads.Length; i++) {
                    threads[i] = new Thread(() => {
                        for (; Volatile.Read(ref a);) {
                            aasfd();
                        }
                    });
                }
                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Start();
                }
                Thread.Sleep(100);
                try {
                    aasfd();
                    var sdfas = new int[333];
                    aasfd();
                    var sdafsdf = sdfas.AsMemorySpan(2, 42);
                    aasfd();
                    var sdfasdf = sdafsdf.Span;
                    aasfd();

                    for (var i = 0; sdfas.Length > i; ++i) {
                        aasfd();
                        sdfasdf[i] = -i;
                        aasfd();
                    }
                    Volatile.Write(ref a, false);
                    var sadfa = sdfas[44];
                    Console.WriteLine(sadfa);
                } finally {
                    Volatile.Write(ref a, false);
                }

                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Join();
                }
                return 0;
            }
            {

                var func_x = (Func<double, double>)((x) => x * x);
                var func_y = (Func<double, double>)((x) => x / 2 - 100);

                var curve_x = func_x.ToParametricCurve((-2, 3)).Reparametrization(x => 2 * x, x => x / 2);
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(1).ToString());
                return 0;

            }

            {

                var func_x = (Func<double, double>)((x) => -x);
                var func_y = (Func<double, double>)((x) => 2 * x);

                var curve_x = func_x.ToParametricCurve((-2, 3));
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(2).ToString());
                return 0;
            }



            {
                var vdsadf = new BigUIntegerBuilder(333);
                vdsadf.MultiplyExp10(20);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Multiply(1000000000);
                var sadf = vdsadf.ToString();
                Console.WriteLine($@"{sadf}");
                return 0;
            }
            {
                var a = typeof(MathF);
                Console.WriteLine($@"{(Quadruple)1.5}");
                return 0;
            }
            {
                var a = -2.8;
                var b = 1.0;
                Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{Math.IEEERemainder(a, b):R}");

                // Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{(Double)Quadruple.Math.IEEERemainder(a, b):R}");

                return 0;

            }
            {
                var a1 = 1;
                var a2 = 2;
                var a3 = 3;
                var a4 = 4;
                var b = (double)(1L << (52 + 2));
                for (var i = 0; i < 64; i++) {
                    var bb = Math.ScaleB(b, i);

                    for (var j = 0; j < 8; ++j) {
                        Console.WriteLine($@"{UltimateOrb.Utilities.CilVerifiable.AddThenSubtractFirst(bb, j):R}");
                    }
                }
                return 0;

            }
            {
                var sdf = (object)(-4);

                Console.WriteLine(UltimateOrb.Utilities.CilVerifiable.UnboxRef<int>(sdf));

                return 0;
            }
            {
                var aaa = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var bbb = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var a = 0L;
                for (var i = 0L; i < 400000000000L; i++) {
                    a ^= UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(aaa, bbb);

                }
                Console.WriteLine(a);

                return 0;
            }
            {
                var sdffa = new int[] {
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, Double.NaN),
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, 0.2),
                };

                foreach (var item in sdffa) {
                    Console.WriteLine(item);
                }


                return 0;
            }
            {
                var a = (1UL << 52) + 1;
                var b = (1UL << 52);
                var c = (1UL << 52) - 1;
                var d = (1UL << 52) - 1;

                var p = a + 0.5;
                var q = b + 0.5;
                var r = c + 0.5;
                var s = d + 0.75;
                var sp = d + 0.76;
                var sm = d + 0.74;

                Console.WriteLine($@"{double.Epsilon:R}");
                Console.WriteLine($@"{double.Epsilon * d:R}");
                Console.WriteLine($@"{p:R}");
                Console.WriteLine($@"{q:R}");
                Console.WriteLine($@"{r:R}");
                Console.WriteLine($@"{s:R}");
                Console.WriteLine($@"{sp:R}");
                Console.WriteLine($@"{sm:R}");
                return 0;
            }
            {

                var sdfada = 0u;
                sdfada ^= sdfada;
                var sdaf = System.Numerics.BitOperations.LeadingZeroCount(sdfada);
                _ = sdaf.GetHashCode();

            }
            {
                var sfassss = BitConverter.Int64BitsToDouble(0x7FFF400000000000);
                var sfas = sfassss;
                var vdsa = 0.0 - sfas;
                var sfad = BitConverter.DoubleToInt64Bits(sfas);
                var asdsd = BitConverter.DoubleToInt64Bits(vdsa);
                _ = (sfad ^ asdsd).GetHashCode();

            }
            {


                var ccc = (Quadruple)0.0 < (Quadruple)Double.NegativeInfinity;
                Console.WriteLine(ccc);
            }
            {
                var ccc = Double.NaN != Double.NaN;
                var a = Quadruple.IsNaN(Quadruple.NaN);
                Console.WriteLine(a);
                var sdfa = +Quadruple.MinValue;
                Console.WriteLine(sdfa);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            return 0;
        }
    }
}


namespace UltimateOrb.Plain.ValueTypes {

    public readonly struct NodeId_A
        : IEquatable<NodeId_A>, IFormattable {

        private readonly int m_value;

        internal NodeId_A(int value) {
            this.m_value = value;
        }

        public static NodeId_A operator +(NodeId_A value) {
            return value;
        }

        public static NodeId_A operator -(NodeId_A value) {
            return new NodeId_A(unchecked(~value.m_value - 1));
        }

        public static NodeId_A operator <<(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value << shift);
        }

        public static NodeId_A operator >>(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value >> shift);
        }

        public static NodeId_A operator +(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(1 + first.m_value + second.m_value));
        }

        public static NodeId_A operator -(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(first.m_value + ~second.m_value));
        }

        public static NodeId_A operator *(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value * ~second.m_value)));
        }

        public static NodeId_A operator /(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value / ~second.m_value)));
        }

        public static NodeId_A operator %(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value % ~second.m_value)));
        }

        public static NodeId_A operator &(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value | second.m_value);
        }

        public static NodeId_A operator |(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value & second.m_value);
        }

        public static NodeId_A operator ^(NodeId_A first, NodeId_A second) {
            return new NodeId_A(~(first.m_value ^ second.m_value));
        }

        public static NodeId_A operator ~(NodeId_A value) {
            return new NodeId_A(~value.m_value);
        }

        public static NodeId_A operator ++(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value - 1));
        }

        public static NodeId_A operator --(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value + 1));
        }

        public static implicit operator int(NodeId_A value) {
            return ~value.m_value;
        }

        public static implicit operator NodeId_A(int value) {
            return new NodeId_A(~value);
        }

        public override bool Equals(object obj) {
            if (obj is NodeId_A other) {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(NodeId_A other) {
            return this.m_value == other.m_value;
        }

        public override int GetHashCode() {
            return this.m_value;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            return (~this.m_value).ToString(format, formatProvider);
        }

        public override string ToString() {
            return (~this.m_value).ToString();
        }

        public static bool operator ==(NodeId_A left, NodeId_A right) {
            return left.m_value == right.m_value;
        }

        public static bool operator !=(NodeId_A left, NodeId_A right) {
            return left.m_value != right.m_value;
        }
    }
}
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
