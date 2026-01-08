using NUnit;
using FsCheck.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Collections;
using UltimateOrb;
using UltimateOrb.Mathematics;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.FSharp.Core;
using UltimateOrb.Numerics;
using InternalMath = Internal.System.Math;

namespace UltimateOrb.Core.Tests {


    /*
    public static class Assert {

        public static void True(bool v) {
            if (!v) {
                throw new Exception();
            }
        }

        public static void False(bool v) {
            True(!v);
        }
    }
    */

    public struct a {
        int _Exponent;
        UInt64 _FractionHi;
        UInt64 _FractionLo;
        sbyte _Infinity;
        sbyte _NaN;
        sbyte _NonZero;
        sbyte _Sign;
    }

    public static partial class TestModule {

        [ThreadStatic]
        static QuadrupleReferenceProcessor _pppp;

        static QuadrupleReferenceProcessor pppp {

            get {
                var t = _pppp;
                if (null == t) {
                    t = new QuadrupleReferenceProcessor();
                    _pppp = t;
                }
                return t;
            }
        }

        public static IEnumerable<Memory<TSource>> CopyToBuffers<TSource>(this IEnumerable<TSource> source, int bufferLength) {
            return source.CopyToBuffers((z) => new TSource[bufferLength].AsMemory());
        }

        public static IEnumerable<Memory<TSource>> CopyToBuffers<TSource>(this IEnumerable<TSource> source, Func<(long ElementIndex, long BufferIndex), Memory<TSource>> bufferSelector) {
            var i = 0L;
            var j = 0L;
            var k = 0;
            var c = 0;
            Memory<TSource> buffer = default;
            Span<TSource> span = default;
            foreach (var item in source) {
                if (0 == c) {
                    for (; ; ) {
                        buffer = bufferSelector.Invoke((i, j));
                        span = buffer.Span;
                        c = span.Length;
                        if (c > 0) {
                            break;
                        }
                        yield return buffer;
                    }
                }
                buffer.Span[k++] = item;
                --c;
                if (buffer.Length <= k) {
                    yield return buffer;
                    k = 0;
                }
            }
            {
                if (k > 0) {
                    yield return buffer.Slice(0, k);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 DivRem_A_F(UInt64 dividend, UInt64 divisor, out UInt64 remainder) {
            unchecked {
                if (0 == divisor) {
                    goto L_Z;
                }
                var r_lo = ToPackedDoubleUnchecked_A(dividend, out var r_hi);
                var d_lo = ToPackedDoubleUnchecked_A(divisor, out var d_hi);
                var q_lo = DoubleArithmeticF.DividePartial(r_lo, r_hi, d_lo, d_hi, out var q_hi);
                var q = FromPackedDoubleUnchecked(q_lo, q_hi);
                var p = divisor * q;
                var r = dividend - p;
                //if (dividend < p) {
                //    r += divisor;
                //    --q;
                //} else {
                //    var t = r - divisor;
                //    if (r >= divisor) {
                //        r = t;
                //        ++q;
                //    }
                //}
                remainder = r;
                return q;
            L_Z:;
                {
                    _ = dividend / 0u;
                    throw (DivideByZeroException)null;
                }
            }
        }

        public static Quadruple Quadruple_Add_A(Quadruple x, Quadruple y) {
            return pppp.Add(x, y);
        }

        public static IEnumerable<Quadruple> Quadruple_Add_A(IEnumerable<(Quadruple First, Quadruple Second)> ps) {
            return pppp.Add(ps);
        }

        public static IEnumerable<Quadruple> Quadruple_Multiply_A(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            return pppp.Multiply(ps, rounding);
        }

        public static IEnumerable<Quadruple> Quadruple_Divide_A(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            return pppp.Divide(ps, rounding);
        }

        public static IEnumerable<Quadruple> Quadruple_Remainder_A(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            return pppp.Remainder(ps, rounding);
        }

        public static IEnumerable<Quadruple> Quadruple_IEEERemainder_A(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            return pppp.IEEERemainder(ps, rounding);
        }

        public static Quadruple Quadruple_Sqrt_A(Quadruple x, CFloatingPointRounding rounding) {
            return pppp.Sqrt(x, rounding);
        }

        public static IEnumerable<Quadruple> Quadruple_Sqrt_A(IEnumerable<Quadruple> ps, CFloatingPointRounding rounding) {
            return pppp.Sqrt(ps, rounding);
        }

        public static void sgafdsa(UInt64 value_lo, UInt64 value_hi) {

            var d_lo = 0x0000000000000000u;
            var d_hi = 0x4000000000000000u;
            var r_lo = DoubleArithmetic.ShiftLeft(value_lo, value_hi, 128 - 112 - 1, out var r_hi);
            r_hi &= 0x7FFFFFFFFFFFFFffu;

            var i = 0;
            throw new NotImplementedException();
            for (; ; ) {
                var c = unchecked(1 + i);
                if (DoubleArithmetic.GreaterThanOrEqual(r_lo, r_hi, d_lo, d_hi)) {
                    r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, d_lo, d_hi, out r_hi);
                    // * L(1 + i)


                    var f_lo = DoubleArithmetic.ShiftLeft(r_lo, r_hi, 4, out var f_hi);


                }
                r_lo = DoubleArithmetic.ShiftLeft(r_lo, r_hi, out r_hi);
            }

        }

        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, long count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (0 > count) {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            return GetSkipEnumerable<TSource>(source, count);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_BinaryNumerals_CountLeadingZero_1() {
            for (long i = 0; 0x100000000 > i; i += 0x40000000) {
                if (!Enumerable.Range(0, 0x40000000).AsParallel().All((j) => {
                    var k = unchecked((UInt32)(i + j));
                    return UltimateOrb.Mathematics.BinaryNumerals.CountLeadingZeros(k) == UltimateOrb.Mathematics.BinaryNumerals.CountLeadingZeros(k);
                })) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_DoubleArithmetic_DivRem_A_F_1(UInt64 x, UInt64 y) {
            if (0 == y) {
                return true;
            }
            var q0 = InternalMath.DivRem(x, y, out var r0);
            var q1 = /*DoubleArithmetic.*/DivRem_A_F(x, y, out var r1);

            if (r0.Equals(r1) && q0.Equals(q1)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Internal_aaa() {
            int c = int.MaxValue;
            long a = 0;
            foreach (var buffer in Enumerable.Range(0, c).CopyToBuffers(256)) {
                foreach (var item in buffer.Span) {
                    checked {
                        ++a;
                    }
                }
            }
            return c == a;
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_1(int x, int y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            var r = (long)x + (long)y;
            var r0 = (Quadruple)r;
            var r1 = x1 + y1;
            if (r0.Equals(r1)) {
                return true;
            } else {
                return false;
            }
        }

        public static bool Test_Quadruple_Add_10000(Quadruple x, Quadruple y) {
            var r0 = Quadruple_Add_A(x, y);
            var r1 = x + y;
            if (r0.Equals(r1)) {
                return true;
            } else {
                // ???
                return false;
            }
        }

        public static bool Test_Quadruple_Sqrt_10000(Quadruple x) {
            var r0 = Quadruple_Sqrt_A(x, CFloatingPointRounding.ToNearest);
            var r1 = Quadruple.Math.Sqrt(x);
            if (r0.Equals(r1)) {
                return true;
            } else {
                if (Quadruple.IsFinite(r1 - r0) && r1 <= Quadruple.Math.BitIncrement(r0) && r1 >= Quadruple.Math.BitDecrement(r0)) {
                    return true;
                }
                // ???
                return false;
            }
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_10002() {
            var x = (1.0 + (1L << 52)) / (1L << 52);
            var y = 0.5;
            Quadruple x1 = x;
            Quadruple y1 = y;
            var r = x + y;
            var r0 = (Quadruple)r;
            var r1 = x1 + y1;
            if (r0.Equals(r1)) {
                return true;
            } else {

                return false;
            }
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_10003() {
            var x = (1.0 + (1L << 52)) / (1L << 52);
            var y = 0.499;
            Quadruple x1 = x;
            Quadruple y1 = y;
            var r = x + y;
            var r0 = (Quadruple)r;
            var r1 = x1 + y1;
            if (r0.Equals(r1)) {
                return false;
            } else {
                return true;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_2() {
            return GetQuadruple2TestSamples().AsParallel().All((p) => Test_Quadruple_Add_10000(p.First, p.Second));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Sqrt_2() {
            return GetQuadrupleTestSamples(1, 0).AsParallel().All((x) => Test_Quadruple_Sqrt_10000(x));
        }

        public static bool Test_Quadruple_Add_20000(IEnumerable<(Quadruple First, Quadruple Second)> ps) {
            var r0 = Quadruple_Add_A(ps);
            var r1 = ps.Select(p => p.First + p.Second);
            if (r0.SequenceEqual(r1)) {
                return true;
            } else {
                // ???
                return false;
            }
        }

        public static bool Test_Quadruple_Multiply_20000(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            var r0 = Quadruple_Multiply_A(ps, rounding);
            var r1 = ps.Select(p => Quadruple.Multiply(p.First, p.Second, FromC(rounding)));
            if (r0.SequenceEqual(r1)) {
                return true;
            } else {
                var r = r0.Zip(r1);
                long i = 0;
                foreach (var item in r) {
                    if (!item.First.Equals(item.Second)) {
                        var sdfa = ps.Skip(i).FirstOrDefault();
                        var asdfs = Quadruple.Multiply(sdfa.First, sdfa.Second, FromC(rounding));
                        return false;
                    }
                    ++i;
                }
                // ???
                return false;
            }
        }

        public static bool Test_Quadruple_Divide_20000(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            var r0 = Quadruple_Divide_A(ps, rounding);
            var r1 = ps.Select(p => Quadruple.Divide(p.First, p.Second, FromC(rounding)));
            if (r0.SequenceEqual(r1)) {
                return true;
            } else {
                var r = r0.Zip(r1);
                long i = 0;
                foreach (var item in r) {
                    if (!item.First.Equals(item.Second)) {
                        var sdfa = ps.Skip(i).FirstOrDefault();
                        var asdfs = Quadruple.Divide(sdfa.First, sdfa.Second, FromC(rounding));
                        return false;
                    }
                    ++i;
                }
                // ???
                return false;
            }
        }

        public static bool Test_Quadruple_Remainder_20000(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            var r0 = Quadruple_Remainder_A(ps, rounding);
            var r1 = ps.Select(p => Quadruple.Remainder(p.First, p.Second, FromC(rounding)));
            if (r0.SequenceEqual(r1)) {
                return true;
            } else {
                var r = r0.Zip(r1);
                long i = 0;
                foreach (var item in r) {
                    if (!item.First.Equals(item.Second)) {
                        var sdfa = ps.Skip(i).FirstOrDefault();
                        var asdfs = Quadruple.Remainder(sdfa.First, sdfa.Second, FromC(rounding));
                        return false;
                    }
                    ++i;
                }
                // ???
                return false;
            }
        }

        public static bool Test_Quadruple_IEEERemainder_20000(IEnumerable<(Quadruple First, Quadruple Second)> ps, CFloatingPointRounding rounding = CFloatingPointRounding.ToNearest) {
            var r0 = Quadruple_IEEERemainder_A(ps, rounding);
            var r1 = ps.Select(p => Quadruple.Math.IEEERemainder(p.First, p.Second, FromC(rounding)));
            if (r0.SequenceEqual(r1)) {
                return true;
            } else {
                var r = r0.Zip(r1);
                long i = 0;
                foreach (var item in r) {
                    if (!item.First.Equals(item.Second)) {
                        var sdfa = ps.Skip(i).FirstOrDefault();
                        var asdfs = Quadruple.Math.IEEERemainder(sdfa.First, sdfa.Second, FromC(rounding));
                        return false;
                    }
                    ++i;
                }
                // ???
                return false;
            }
        }


        public static bool Test_Quadruple_Sqrt_20000(IEnumerable<Quadruple> ps, CFloatingPointRounding rounding) {
            var r0 = Quadruple_Sqrt_A(ps, rounding);
            var r1 = ps.Select(p => Quadruple.Math.Sqrt(p, FromC(rounding)));
            if (r0.SequenceEqual(r1, new ssss())) {
                return true;
            } else {
                // ???
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_4() {
            return Test_Quadruple_Add_20000(GetQuadruple2TestSamples());
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_4() {
            return
                Test_Quadruple_Multiply_20000(GetQuadruple2TestSamples(0, 1)) &&
                Test_Quadruple_Multiply_20000(GetQuadruple2TestSamples(1, 0));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_104() {
            return Test_Quadruple_Multiply_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.ToNearest);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_105() {
            return Test_Quadruple_Multiply_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Downward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_106() {
            return Test_Quadruple_Multiply_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Upward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_107() {
            return Test_Quadruple_Multiply_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.TowardZero);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Divide_104() {
            return Test_Quadruple_Divide_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.ToNearest);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Divide_105() {
            return Test_Quadruple_Divide_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Downward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Divide_106() {
            return Test_Quadruple_Divide_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Upward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Remainder_104() {
            return Test_Quadruple_Remainder_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.ToNearest);
        }


        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_IEEERemainder_104() {
            return Test_Quadruple_IEEERemainder_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.ToNearest);
        }

        /*
        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_IEEERemainder_105() {
            return Test_Quadruple_IEEERemainder_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Downward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_IEEERemainder_106() {
            return Test_Quadruple_IEEERemainder_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.Upward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_IEEERemainder_107() {
            return Test_Quadruple_IEEERemainder_20000(GetRandomQuadruple2Enumerable(GetRandom(), 10000000).ToArray(), CFloatingPointRounding.TowardZero);
        }
        */

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_12() {
            return Test_Quadruple_Multiply_20000(GetDouble2TestSamples(1, 1).Select((x) => ((Quadruple)x.First, (Quadruple)x.Second)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_13() {
            return Test_Quadruple_Multiply_20000(new (Quadruple, Quadruple)[] {
                (21149740236874377UL, 491003369344660409UL),
                (9734174361238150513UL, 1066818132868207UL),
                (217703621183571985UL, 47700601674021587UL),
            });
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_14() {
            return Test_Quadruple_Multiply_20000(new (Quadruple, Quadruple)[] {
                (21149740236874377UL, 491003369344660409UL),
                (9734174361238150513UL, 1066818132868207UL),
                (217703621183571985UL, 47700601674021587UL),
            }, CFloatingPointRounding.TowardZero);
        }

        public enum CFloatingPointRounding {
            ToNearest = 0,
            Downward = 1,
            Upward = 2,
            TowardZero = 3,
        }

        public static CFloatingPointRounding ToC(FloatingPointRounding rounding) {
            switch (rounding) {
            case FloatingPointRounding.ToNearestWithMidpointToEven:
                return CFloatingPointRounding.ToNearest;
            case FloatingPointRounding.Upward:
                return CFloatingPointRounding.Upward;
            case FloatingPointRounding.Downward:
                return CFloatingPointRounding.Downward;
            case FloatingPointRounding.TowardZero:
                return CFloatingPointRounding.TowardZero;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static FloatingPointRounding FromC(CFloatingPointRounding rounding) {
            switch (rounding) {
            case CFloatingPointRounding.ToNearest:
                return FloatingPointRounding.ToNearestWithMidpointToEven;
            case CFloatingPointRounding.Upward:
                return FloatingPointRounding.Upward;
            case CFloatingPointRounding.Downward:
                return FloatingPointRounding.Downward;
            case CFloatingPointRounding.TowardZero:
                return FloatingPointRounding.TowardZero;
            default:
                throw new ArgumentOutOfRangeException();
            }
        }

        public static IEnumerable<Quadruple> GetRandomQuadrupleEnumerable(this Random random, int count) {
            var b = new Memory<byte>(new byte[16]);
            for (var i = count; i > 0; --i) {
                random.NextBytes(b.Span);
                var lo = BitConverter.ToUInt64(b.Span);
                var hi = BitConverter.ToInt64(b.Slice(8).Span);
                yield return Quadruple.BitConverter.Int128BitsToQuadruple(Int128.FromBits(lo, hi));
            }
        }

        public static IEnumerable<(Quadruple First, Quadruple Second)> GetRandomQuadruple2Enumerable(this Random random, int count) {
            using (var en = GetRandomQuadrupleEnumerable(random, checked(2 * count)).GetEnumerator()) {
                for (var i = count; i > 0; --i) {
                    en.MoveNext();
                    var f = en.Current;
                    en.MoveNext();
                    var s = en.Current;
                    yield return (f, s);
                }
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Sqrt_4() {
            return Test_Quadruple_Sqrt_20000(GetQuadrupleTestSamples(2, 2), CFloatingPointRounding.ToNearest);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Sqrt_5() {
            return Test_Quadruple_Sqrt_20000(GetQuadrupleTestSamples(2, 2), CFloatingPointRounding.Downward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Sqrt_6() {
            return Test_Quadruple_Sqrt_20000(GetQuadrupleTestSamples(2, 2), CFloatingPointRounding.Upward);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Sqrt_7() {
            return Test_Quadruple_Sqrt_20000(GetQuadrupleTestSamples(2, 2), CFloatingPointRounding.TowardZero);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_6() {
            return GetDouble2TestSamples(0, 1).AsParallel().All((p) => Test_Quadruple_Add_10000(p.First, p.Second));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Add_8() {
            return Test_Quadruple_Add_20000(GetDouble2TestSamples(1, 1).Select((x) => ((Quadruple)x.First, (Quadruple)x.Second)));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Double_1(Double d) {
            var q = (Quadruple)d;
            var t = (Double)q;
            if (d.Equals(t)) {
                return true;
            } else {
                return false;
            }
        }

        public readonly struct Try<T> {
            public readonly Exception? Exception;
            public readonly T ValueOrDefault;
            //
            // Summary:
            //     Initializes a new instance of the Try`1 structure to the specified
            //     value.
            //
            // Parameters:
            //   value:
            //     A value type.
            public Try(T value) {
                Exception = null;
                ValueOrDefault = value;
            }

            public Try(Exception exception) {
                if (exception is null) {
                    throw new ArgumentNullException(nameof(exception));
                }

                Exception = exception;
                ValueOrDefault = default;
            }

            //
            // Summary:
            //     Gets a value indicating whether the current Try`1 object has a valid
            //     value of its underlying type.
            //
            // Returns:
            //     true if the current Try`1 object has a value; false if the current
            //     Try`1 object has no value.
            public bool HasValue {

                get => null == Exception;
            }
            //
            // Summary:
            //     Gets the value of the current Try`1 object if it has been assigned
            //     a valid underlying value.
            //
            // Returns:
            //     The value of the current Try`1 object if the Try`1.HasValue
            //     property is true. An exception is thrown if the Try`1.HasValue property
            //     is false.
            //
            // Exceptions:
            //   T:System.InvalidOperationException:
            //     The Try`1.HasValue property is false.
            public T Value {

                get {
                    if (null == Exception) {
                        return ValueOrDefault;
                    }
                    throw new InvalidOperationException();
                }
            }

            //
            // Summary:
            //     Indicates whether the current Try`1 object is equal to a specified
            //     object.
            //
            // Parameters:
            //   other:
            //     An object.
            //
            // Returns:
            //     true if the other parameter is equal to the current Try`1 object;
            //     otherwise, false. This table describes how equality is defined for the compared
            //     values: Return Value Description true The Try`1.HasValue property
            //     is false, and the other parameter is null. That is, two null values are equal
            //     by definition. -or- The Try`1.HasValue property is true, and the
            //     value returned by the Try`1.Value property is equal to the other
            //     parameter. false The Try`1.HasValue property for the current Try`1
            //     structure is true, and the other parameter is null. -or- The Try`1.HasValue
            //     property for the current Try`1 structure is false, and the other
            //     parameter is not null. -or- The Try`1.HasValue property for the current
            //     Try`1 structure is true, and the value returned by the Try`1.Value
            //     property is not equal to the other parameter.
            public override bool Equals(object? other) {
                if (other is Try<T> t) {
                    return Equals(t);
                }
                return base.Equals(other);
            }

            public bool Equals(Try<T> other) {
                if (null == Exception) {
                    return null == other.Exception && ((ValueOrDefault is null && other.ValueOrDefault is null) || ValueOrDefault.Equals(other.ValueOrDefault));
                } else {
                    return null != other.Exception && Exception.GetType().Equals(other.Exception.GetType());
                }
            }

            //
            // Summary:
            //     Retrieves the hash code of the object returned by the Try`1.Value
            //     property.
            //
            // Returns:
            //     The hash code of the object returned by the Try`1.Value property
            //     if the Try`1.HasValue property is true, or zero if the Try`1.HasValue
            //     property is false.
            public override int GetHashCode() {
                if (null == Exception) {
                    if (ValueOrDefault is null) {
                        return 0;
                    }
                    return ValueOrDefault.GetHashCode();
                }
                return Exception.GetHashCode();
            }
            //
            // Summary:
            //     Retrieves the value of the current Try`1 object, or the default value
            //     of the underlying type.
            //
            // Returns:
            //     The value of the Try`1.Value property if the Try`1.HasValue
            //     property is true; otherwise, the default value of the underlying type.
            public T GetValueOrDefault() {
                return ValueOrDefault;
            }
            //
            // Summary:
            //     Retrieves the value of the current Try`1 object, or the specified
            //     default value.
            //
            // Parameters:
            //   defaultValue:
            //     A value to return if the Try`1.HasValue property is false.
            //
            // Returns:
            //     The value of the Try`1.Value property if the Try`1.HasValue
            //     property is true; otherwise, the defaultValue parameter.
            public T GetValueOrDefault(T defaultValue) {
                return null == Exception ? defaultValue : ValueOrDefault;
            }
            //
            // Summary:
            //     Returns the text representation of the value of the current Try`1
            //     object.
            //
            // Returns:
            //     The text representation of the value of the current Try`1 object
            //     if the Try`1.HasValue property is true, or an empty string ("") if
            //     the Try`1.HasValue property is false.
            public override string? ToString() {
                return null != Exception ? Exception.ToString() : (ValueOrDefault is null ? "" : ValueOrDefault.ToString());
            }

            public static implicit operator Try<T>(T value) {
                return new Try<T>(value);
            }

            public static explicit operator T(Try<T> value) {
                return value.Value;
            }
        }

        public static Try<TResult> TryInvoke<TResult>(this Func<TResult> func) {
            if (func is null) {
                throw new ArgumentNullException(nameof(func));
            }

            try {
                return func.Invoke();
            } catch (Exception ex) {
                return new Try<TResult>(ex);
            }
        }

        public static Try<TResult> TryInvoke<T, TResult>(this Func<T, TResult> func, T arg) {
            if (func is null) {
                throw new ArgumentNullException(nameof(func));
            }

            try {
                return func.Invoke(arg);
            } catch (Exception ex) {
                return new Try<TResult>(ex);
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Int32_201() {
            var rr = GetRandom();
            for (int i = 0; i < 10000000; i++) {
                var d = (Double)(rr.NextDouble() * 200.0 - 100.0);
                var j = (Int32)d;
                var k = (Int32)(Quadruple)d;
                if (!j.Equals(k)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Int64_201() {
            var rr = GetRandom();
            for (int i = 0; i < 10000000; i++) {
                var d = (Double)(rr.NextDouble() * 200.0 - 100.0);
                var j = (Int64)d;
                var k = (Int64)(Quadruple)d;
                if (!j.Equals(k)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt32_201() {
            var rr = GetRandom();
            for (int i = 0; i < 100000; i++) {
                var d = (Double)(rr.NextDouble() * 200.0 - 100.0);
                if (d < -4) {
                    continue;
                }
                var j = TryInvoke((d) => (UInt32)(Double)d, d);
                var k = TryInvoke((d) => (UInt32)(Quadruple)d, d);
                if (!j.Equals(k)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt64_201() {
            var rr = GetRandom();
            for (int i = 0; i < 100000; i++) {
                var d = (Double)(rr.NextDouble() * 200.0 - 100.0);
                if (d < -4) {
                    continue;
                }
                var j = TryInvoke((d) => (UInt64)(Double)d, d);
                var k = TryInvoke((d) => (UInt64)(Quadruple)d, d);
                if (!j.Equals(k)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Double_2() {
            return GetDoubleTestSamples().AsParallel().All((p) => Test_Quadruple_Conversion_Double_1(p));
        }

        public static bool Test_Quadruple_Conversion_Double_3(Quadruple q) {
            var d = (Double)q;
            var c = (Quadruple)d;
            if (q.Equals(c)) {
                return true;
            } else {
                if (q >= (Quadruple)double.MaxValue + BitConverter.Int64BitsToDouble(0x7c90000000000000) && Quadruple.IsPositiveInfinity(c)) {
                    return true;
                }
                if (q <= -((Quadruple)double.MaxValue + BitConverter.Int64BitsToDouble(0x7c90000000000000)) && Quadruple.IsNegativeInfinity(c)) {
                    return true;
                }
                var b = Quadruple.Math.Abs(c) <= Quadruple.Math.Abs(q);

                var d1 = BitConverter.Int64BitsToDouble(checked((b ? 1 : -1) + BitConverter.DoubleToInt64Bits(Math.Abs(d))));
                var k1 = b ? b : Quadruple.Math.Abs(q) <= Quadruple.Math.Abs(c);
                var k2 = b ? Quadruple.Math.Abs(q) <= Quadruple.Math.Abs(d1) : Quadruple.Math.Abs(d1) <= Quadruple.Math.Abs(q);
                var t1 = Quadruple.Math.Abs(c - q);
                var t2 = Quadruple.Math.Abs(d1 - q);

                var k3 = t1 < t2;
                var k4 = t1 == t2;

                if (
                    k1 &&
                    k2 &&
                    (k3 ||
                    (k4 &&
                    (0u == (1u & BitConverter.DoubleToInt64Bits(d)))))) {
                    return true;
                }
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Double_4() {
            return GetQuadrupleTestSamples(4, 2).AsParallel().All((p) => Test_Quadruple_Conversion_Double_3(p));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_Int64_1(Int64 u) {
            var q = (Quadruple)u;
            var w = (Int64)q;
            if (u == w) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt32_1(UInt32 u) {
            var q = (Quadruple)u;
            var w = (UInt32)q;
            if (u == w) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt32_2() {
            return GetUInt32TestSamples().AsParallel().All((p) => Test_Quadruple_Conversion_UInt32_1(p));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt64_1(UInt64 u) {
            var q = (Quadruple)u;
            var w = (UInt64)q;
            if (u == w) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt64_2() {
            return GetUInt64TestSamples().AsParallel().All((p) => Test_Quadruple_Conversion_UInt64_1(p));
        }

        public static bool Test_Quadruple_Conversion_UInt64_3(Quadruple q) {
            UInt64 u;
            try {
                u = (UInt64)q;
            } catch (ArithmeticException) {
                if (q <= -1) {
                    return true;
                }
                if (Quadruple.IsNaN(q)) {
                    return true;
                }
                if (((Quadruple)1 + UInt64.MaxValue) <= q) {
                    return true;
                }
                throw;
            } catch (Exception) {
                throw;
            }
            var c = (Quadruple)u;
            if (Quadruple.Math.Abs(c) <= Quadruple.Math.Abs(q) && (1 + Quadruple.Math.Abs(c)) > Quadruple.Math.Abs(q)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Conversion_UInt64_4() {
            return GetQuadrupleTestSamples(4, 2).AsParallel().All((p) => Test_Quadruple_Conversion_UInt64_3(p));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Divide_70001(int m, short n) {
            var x = m * n;
            var y = m;

            Quadruple x1 = x;
            Quadruple y1 = y;
            var r = (double)x / (double)y;
            var r0 = (Quadruple)r;
            var r1 = x1 / y1;
            if (r0.Equals(r1)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Equals_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            return (x == y) == (x1 == y1);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Equals_2() {
            return GetDouble2TestSamples().AsParallel().All((p) => Test_Quadruple_Equals_1(p.First, p.Second));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_EqualsTotalOrder_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            return x.Equals(y) == x1.Equals(y1);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_EqualsTotalOrder_2() {
            return GetDouble2TestSamples().AsParallel().All((p) => Test_Quadruple_EqualsTotalOrder_1(p.First, p.Second));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_GreaterThan_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            return (x > y) == (x1 > y1);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_GreaterThan_2() {
            return GetDouble2TestSamples().AsParallel().All((p) => Test_Quadruple_GreaterThan_1(p.First, p.Second));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_GreaterThanOrEqual_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            return (x >= y) == (x1 >= y1);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_GreaterThanOrEqual_2() {
            return GetDouble2TestSamples().AsParallel().All((p) => Test_Quadruple_GreaterThanOrEqual_1(p.First, p.Second));
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_LessThan_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            var a = System.BitConverter.DoubleToInt64Bits(x);
            var b = System.BitConverter.DoubleToInt64Bits(y);
            (a ^ b).GetHashCode();
            if ((x < y) == (x1 < y1)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_LessThan_2() {
            var asf = GetDouble2TestSamples();
            for (var i = 0L; ; i += 0x40000000) {
                var a = false;
                if (!asf.Skip(i).Take(0x40000000).AsParallel().All((p) => {
                    a = true;
                    return Test_Quadruple_LessThan_1(p.First, p.Second);
                })) {
                    return false;
                }
                if (!a) {
                    break;
                }
            }
            return true;
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_LessThanOrEqual_1(double x, double y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            if ((x <= y) == (x1 <= y1)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_LessThanOrEqual_2() {
            return GetDouble2TestSamples().AsParallel().All((p) => Test_Quadruple_LessThanOrEqual_1(p.First, p.Second));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_MathSign_1() {
            Assert.True(Quadruple.IsNaN(Quadruple.NaN));
            var z = (Quadruple)0;
            Assert.True(Quadruple.IsZero(z));
            Assert.True(Quadruple.IsZero(-z));

            var m5 = (Quadruple)(-5L);
            var p5_a = (Quadruple)5U;
            var p5_b = (Quadruple)5;
            var p5_c = (Quadruple)5UL;
            var p5_d = (Quadruple)5L;


            var a = (Quadruple)(-1.0);
            var b = (Quadruple)(-(((UInt64)1 << 52) + 1.0) / ((UInt64)1 << 52));
            var b1 = (Quadruple)(-(((UInt64)1 << 53) + 1.0) / ((UInt64)1 << 53));
            
            Assert.True(Quadruple.MaxValue == -Quadruple.MinValue);
            Assert.True(Quadruple.PositiveInfinity == -Quadruple.NegativeInfinity);

            Assert.True(Quadruple.IsFinite(Double.Epsilon));
            Assert.False(Quadruple.IsInfinity(Double.Epsilon));
            Assert.False(Quadruple.IsNaN(Double.Epsilon));
            Assert.False(Quadruple.IsNegative(Double.Epsilon));
            Assert.False(Quadruple.IsNegativeInfinity(Double.Epsilon));
            Assert.True(Quadruple.IsNormal(Double.Epsilon));
            Assert.False(Quadruple.IsPositiveInfinity(Double.Epsilon));
            Assert.False(Quadruple.IsSubnormal(Double.Epsilon));

            Assert.True(Quadruple.IsFinite(-Double.Epsilon));
            Assert.False(Quadruple.IsInfinity(-Double.Epsilon));
            Assert.False(Quadruple.IsNaN(-Double.Epsilon));
            Assert.True(Quadruple.IsNegative(-Double.Epsilon));
            Assert.False(Quadruple.IsNegativeInfinity(-Double.Epsilon));
            Assert.True(Quadruple.IsNormal(-Double.Epsilon));
            Assert.False(Quadruple.IsPositiveInfinity(-Double.Epsilon));
            Assert.False(Quadruple.IsSubnormal(-Double.Epsilon));

            Assert.True(Quadruple.IsFinite(Double.MaxValue));
            Assert.False(Quadruple.IsInfinity(Double.MaxValue));
            Assert.False(Quadruple.IsNaN(Double.MaxValue));
            Assert.False(Quadruple.IsNegative(Double.MaxValue));
            Assert.False(Quadruple.IsNegativeInfinity(Double.MaxValue));
            Assert.True(Quadruple.IsNormal(Double.MaxValue));
            Assert.False(Quadruple.IsPositiveInfinity(Double.MaxValue));
            Assert.False(Quadruple.IsSubnormal(Double.MaxValue));

            Assert.True(Quadruple.IsFinite(Double.MinValue));
            Assert.False(Quadruple.IsInfinity(Double.MinValue));
            Assert.False(Quadruple.IsNaN(Double.MinValue));
            Assert.True(Quadruple.IsNegative(Double.MinValue));
            Assert.False(Quadruple.IsNegativeInfinity(Double.MinValue));
            Assert.True(Quadruple.IsNormal(Double.MinValue));
            Assert.False(Quadruple.IsPositiveInfinity(Double.MinValue));
            Assert.False(Quadruple.IsSubnormal(Double.MinValue));

            Assert.False(Quadruple.IsFinite(Double.NaN));
            Assert.False(Quadruple.IsInfinity(Double.NaN));
            Assert.True(Quadruple.IsNaN(Double.NaN));

            Assert.False(Quadruple.IsNegativeInfinity(Double.NaN));
            Assert.False(Quadruple.IsNormal(Double.NaN));
            Assert.False(Quadruple.IsPositiveInfinity(Double.NaN));
            Assert.False(Quadruple.IsSubnormal(Double.NaN));

            Assert.False(Quadruple.IsFinite(-Double.NaN));
            Assert.False(Quadruple.IsInfinity(-Double.NaN));
            Assert.True(Quadruple.IsNaN(-Double.NaN));

            Assert.False(Quadruple.IsNegativeInfinity(-Double.NaN));
            Assert.False(Quadruple.IsNormal(-Double.NaN));
            Assert.False(Quadruple.IsPositiveInfinity(-Double.NaN));
            Assert.False(Quadruple.IsSubnormal(-Double.NaN));

            Assert.False(Quadruple.IsFinite(Double.PositiveInfinity));
            Assert.True(Quadruple.IsInfinity(Double.PositiveInfinity));
            Assert.False(Quadruple.IsNaN(Double.PositiveInfinity));
            Assert.False(Quadruple.IsNegative(Double.PositiveInfinity));
            Assert.False(Quadruple.IsNegativeInfinity(Double.PositiveInfinity));
            Assert.False(Quadruple.IsNormal(Double.PositiveInfinity));
            Assert.True(Quadruple.IsPositiveInfinity(Double.PositiveInfinity));
            Assert.False(Quadruple.IsSubnormal(Double.PositiveInfinity));

            Assert.False(Quadruple.IsFinite(Double.NegativeInfinity));
            Assert.True(Quadruple.IsInfinity(Double.NegativeInfinity));
            Assert.False(Quadruple.IsNaN(Double.NegativeInfinity));
            Assert.True(Quadruple.IsNegative(Double.NegativeInfinity));
            Assert.True(Quadruple.IsNegativeInfinity(Double.NegativeInfinity));
            Assert.False(Quadruple.IsNormal(Double.NegativeInfinity));
            Assert.False(Quadruple.IsPositiveInfinity(Double.NegativeInfinity));
            Assert.False(Quadruple.IsSubnormal(Double.NegativeInfinity));

            Assert.True(Quadruple.IsFinite(Quadruple.MaxValue));
            Assert.False(Quadruple.IsInfinity(Quadruple.MaxValue));
            Assert.False(Quadruple.IsNaN(Quadruple.MaxValue));
            Assert.False(Quadruple.IsNegative(Quadruple.MaxValue));
            Assert.False(Quadruple.IsNegativeInfinity(Quadruple.MaxValue));
            Assert.True(Quadruple.IsNormal(Quadruple.MaxValue));
            Assert.False(Quadruple.IsPositiveInfinity(Quadruple.MaxValue));
            Assert.False(Quadruple.IsSubnormal(Quadruple.MaxValue));

            Assert.True(Quadruple.IsFinite(Quadruple.MinValue));
            Assert.False(Quadruple.IsInfinity(Quadruple.MinValue));
            Assert.False(Quadruple.IsNaN(Quadruple.MinValue));
            Assert.True(Quadruple.IsNegative(Quadruple.MinValue));
            Assert.False(Quadruple.IsNegativeInfinity(Quadruple.MinValue));
            Assert.True(Quadruple.IsNormal(Quadruple.MinValue));
            Assert.False(Quadruple.IsPositiveInfinity(Quadruple.MinValue));
            Assert.False(Quadruple.IsSubnormal(Quadruple.MinValue));

            Assert.False(Quadruple.IsFinite(Quadruple.NaN));
            Assert.False(Quadruple.IsInfinity(Quadruple.NaN));
            Assert.True(Quadruple.IsNaN(Quadruple.NaN));

            Assert.False(Quadruple.IsNegativeInfinity(Quadruple.NaN));
            Assert.False(Quadruple.IsNormal(Quadruple.NaN));
            Assert.False(Quadruple.IsPositiveInfinity(Quadruple.NaN));
            Assert.False(Quadruple.IsSubnormal(Quadruple.NaN));

            Assert.False(Quadruple.IsFinite(-Quadruple.NaN));
            Assert.False(Quadruple.IsInfinity(-Quadruple.NaN));
            Assert.True(Quadruple.IsNaN(-Quadruple.NaN));

            Assert.False(Quadruple.IsNegativeInfinity(-Quadruple.NaN));
            Assert.False(Quadruple.IsNormal(-Quadruple.NaN));
            Assert.False(Quadruple.IsPositiveInfinity(-Quadruple.NaN));
            Assert.False(Quadruple.IsSubnormal(-Quadruple.NaN));

            Assert.False(Quadruple.IsFinite(Quadruple.PositiveInfinity));
            Assert.True(Quadruple.IsInfinity(Quadruple.PositiveInfinity));
            Assert.False(Quadruple.IsNaN(Quadruple.PositiveInfinity));
            Assert.False(Quadruple.IsNegative(Quadruple.PositiveInfinity));
            Assert.False(Quadruple.IsNegativeInfinity(Quadruple.PositiveInfinity));
            Assert.False(Quadruple.IsNormal(Quadruple.PositiveInfinity));
            Assert.True(Quadruple.IsPositiveInfinity(Quadruple.PositiveInfinity));
            Assert.False(Quadruple.IsSubnormal(Quadruple.PositiveInfinity));

            Assert.False(Quadruple.IsFinite(Quadruple.NegativeInfinity));
            Assert.True(Quadruple.IsInfinity(Quadruple.NegativeInfinity));
            Assert.False(Quadruple.IsNaN(Quadruple.NegativeInfinity));
            Assert.True(Quadruple.IsNegative(Quadruple.NegativeInfinity));
            Assert.True(Quadruple.IsNegativeInfinity(Quadruple.NegativeInfinity));
            Assert.False(Quadruple.IsNormal(Quadruple.NegativeInfinity));
            Assert.False(Quadruple.IsPositiveInfinity(Quadruple.NegativeInfinity));
            Assert.False(Quadruple.IsSubnormal(Quadruple.NegativeInfinity));

            Assert.True(Quadruple.IsFinite(Quadruple.Epsilon));
            Assert.False(Quadruple.IsInfinity(Quadruple.Epsilon));
            Assert.False(Quadruple.IsNaN(Quadruple.Epsilon));
            Assert.False(Quadruple.IsNegative(Quadruple.Epsilon));
            Assert.False(Quadruple.IsNegativeInfinity(Quadruple.Epsilon));
            Assert.False(Quadruple.IsNormal(Quadruple.Epsilon));
            Assert.False(Quadruple.IsPositiveInfinity(Quadruple.Epsilon));
            Assert.True(Quadruple.IsSubnormal(Quadruple.Epsilon));

            Assert.True(Quadruple.IsFinite(-Quadruple.Epsilon));
            Assert.False(Quadruple.IsInfinity(-Quadruple.Epsilon));
            Assert.False(Quadruple.IsNaN(-Quadruple.Epsilon));
            Assert.True(Quadruple.IsNegative(-Quadruple.Epsilon));
            Assert.False(Quadruple.IsNegativeInfinity(-Quadruple.Epsilon));
            Assert.False(Quadruple.IsNormal(-Quadruple.Epsilon));
            Assert.False(Quadruple.IsPositiveInfinity(-Quadruple.Epsilon));
            Assert.True(Quadruple.IsSubnormal(-Quadruple.Epsilon));


            return true;
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Quadruple_Multiply_1(int x, int y) {
            Quadruple x1 = x;
            Quadruple y1 = y;
            var r = (long)x * (long)y;
            var r0 = (Quadruple)r;
            var r1 = x1 * y1;
            if (r0.Equals(r1)) {
                return true;
            } else {
                return false;
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Quadruple_True_2() {
            // var a = unchecked((int)Double.NegativeInfinity);
            var b = checked((int)-0.9);
            var t = (Double)((1L << 52) + (1L << 32) + (1L << 30));
            var a = unchecked((int)t);
            // var a = unchecked((int)-2.6);
            // -2
            // var a = unchecked((int)-2.4);
            // -2

            a.GetHashCode();
            try {
                return GetUInt64TestSamples().AsParallel().All((p) => {
                    return true;
                });
            } catch (Exception ex) {
                throw ex;
            }

        }

        private static UInt64 FromPackedDoubleUnchecked(Double value_lo, Double value_hi) {
            unchecked {
                return (UInt64)value_lo + (UInt64)value_hi;
            }
        }

        static IEnumerable<(double First, double Second)> GetDouble2TestSamples(int e, int f) {
            var a = GetDoubleTestSamples(e, f).ToArray();
            foreach (var x in a) {
                foreach (var y in a) {
                    yield return (x, y);
                }
            }
        }

        static IEnumerable<(double First, double Second)> GetDouble2TestSamples() {
            return GetDouble2TestSamples(3, 1);
        }

        static IEnumerable<double> GetDoubleTestSamples(int e, int f) {
            var es = GetUInt64PermutationsLEC(11, e).ToArray();
            var fs = GetUInt64PermutationsLEC(52, f).ToArray();
            foreach (var fi in fs) {
                foreach (var ei in es) {
                    var i = unchecked((Int64)((ei << 52) | fi));
                    yield return System.BitConverter.Int64BitsToDouble(i);
                    yield return System.BitConverter.Int64BitsToDouble(Int64.MinValue | i);
                }
            }
        }

        static IEnumerable<double> GetDoubleTestSamples() {
            return GetDoubleTestSamples(4, 4);
        }

        static IEnumerable<(Quadruple First, Quadruple Second)> GetQuadruple2TestSamples() {
            var a = GetQuadrupleTestSamples(1, 1).ToArray();
            foreach (var x in a) {
                foreach (var y in a) {
                    yield return (x, y);
                }
            }
        }

        static IEnumerable<(Quadruple First, Quadruple Second)> GetQuadruple2TestSamples(int e, int f) {
            var a = GetQuadrupleTestSamples(e, f).ToArray();
            foreach (var x in a) {
                foreach (var y in a) {
                    yield return (x, y);
                }
            }
        }

        static IEnumerable<Quadruple> GetQuadrupleTestSamples(int e, int f) {
            var es = GetUInt128PermutationsLEC(15, e).ToArray();
            var fs = GetUInt128PermutationsLEC(112, f).ToArray();
            foreach (var fi in fs) {
                foreach (var ei in es) {
                    var i = Int128.op_ExplicitUnchecked((ei << 112) | fi);
                    yield return Quadruple.BitConverter.Int128BitsToQuadruple(i);
                    yield return Quadruple.BitConverter.Int128BitsToQuadruple(Int128.MinValue | i);
                }
            }
        }

        static IEnumerable<Quadruple> GetQuadrupleTestSamples() {
            return GetQuadrupleTestSamples(4, 2);
        }

        static IEnumerable<TSource> GetSkipEnumerable<TSource>(IEnumerable<TSource> source, long count) {
            using (var e = source.GetEnumerator()) {
                for (; count > 0 && e.MoveNext(); --count) {
                }
                if (count <= 0) {
                    for (; e.MoveNext();) {
                        yield return e.Current;
                    }
                }
            }
        }

        static IEnumerable<UInt128> GetUInt128Permutations(int m, int n) {
            var a = unchecked(((UInt128)1 << n) - 1);
            var b = a << unchecked(m - n);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = UInt128.BinaryNumerals.NextPermutation(a);
            }
        }

        static IEnumerable<UInt128> GetUInt128PermutationsLE(int m, int n) {
            yield return 0;
            for (var i = 1; i <= n; ++i) {
                foreach (var item in GetUInt128Permutations(m, i)) {
                    yield return item;
                }
            }
        }

        static IEnumerable<UInt128> GetUInt128PermutationsLEC(int m, int n) {
            foreach (var item in GetUInt128PermutationsLE(m, n)) {
                yield return item;
                yield return ~item;
            }
        }

        static IEnumerable<UInt32> GetUInt32Permutations(int m, int n) {
            var a = unchecked(((UInt32)1 << n) - 1);
            var b = a << unchecked(m - n);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = UltimateOrb.Mathematics.BinaryNumerals.NextPermutation(a);
            }
        }

        static IEnumerable<UInt32> GetUInt32PermutationsLE(int m, int n) {
            yield return 0;
            for (var i = 1; i <= n; ++i) {
                foreach (var item in GetUInt32Permutations(m, i)) {
                    yield return item;
                }
            }
        }

        static IEnumerable<UInt32> GetUInt32PermutationsLEC(int m, int n) {
            foreach (var item in GetUInt32PermutationsLE(m, n)) {
                yield return item;
                yield return ~item;
            }
        }

        static IEnumerable<UInt32> GetUInt32TestSamples() {
            return GetUInt32PermutationsLEC(32, 8);
        }

        static IEnumerable<UInt64> GetUInt64Permutations(int m, int n) {
            var a = unchecked(((UInt64)1 << n) - 1);
            var b = a << unchecked(m - n);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = UltimateOrb.Mathematics.BinaryNumerals.NextPermutation(a);
            }
        }
        static IEnumerable<UInt64> GetUInt64PermutationsLE(int m, int n) {
            yield return 0;
            for (var i = 1; i <= n; ++i) {
                foreach (var item in GetUInt64Permutations(m, i)) {
                    yield return item;
                }
            }
        }
        static IEnumerable<UInt64> GetUInt64PermutationsLEC(int m, int n) {
            foreach (var item in GetUInt64PermutationsLE(m, n)) {
                yield return item;
                yield return ~item;
            }
        }
        static IEnumerable<UInt64> GetUInt64TestSamples() {
            return GetUInt64PermutationsLEC(64, 6);
        }
        static IEnumerable<(double First, double Second)> GetUInt64Vector2TestSamples() {
            throw new NotImplementedException();
            // return GetUInt64Vector2TestSamples(4);
        }
        private static Double ToPackedDoubleUnchecked_A(UInt64 value, out Double result_hi) {
            unchecked {
                var hi = (Double)value;
                var lo = (Double)(Int64)(value - (UInt64)hi);
                result_hi = hi;
                return lo;
            }
        }
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public unsafe ref struct UInt256Builder {

            // The longest binary mantissa requires: explicit mantissa bits + abs(min exponent)
            // * Half:     10 +    14 =    24
            // * Single:   23 +   126 =   149
            // * Double:   52 +  1022 =  1074
            // * Quad:    112 + 16382 = 16494
            private const int BitsForLongestBinaryMantissa = 1074;

            // The longest digit sequence requires: ceil(log2(pow(10, max significant digits + 1 rounding digit)))
            // * Half:    ceil(log2(pow(10,    21 + 1))) =    74
            // * Single:  ceil(log2(pow(10,   112 + 1))) =   376
            // * Double:  ceil(log2(pow(10,   767 + 1))) =  2552
            // * Quad:    ceil(log2(pow(10, 11563 + 1))) = 38415
            private const int BitsForLongestDigitSequence = 2552;

            // We require BitsPerBlock additional bits for shift space used during the pre-division preparation
            private const int MaxBits = BitsForLongestBinaryMantissa + BitsForLongestDigitSequence + BitsPerBlock;

            private const int BitsPerBlock = sizeof(int) * 8;
            // private const int MaxBlockCount = (MaxBits + (BitsPerBlock - 1)) / BitsPerBlock;
            private const int MaxBlockCount = 8;

            private static readonly uint[] s_Exp10UInt32Table = new uint[]
            {
                1,          // 10^0
                10,         // 10^1
                100,        // 10^2
                1000,       // 10^3
                10000,      // 10^4
                100000,     // 10^5
                1000000,    // 10^6
                10000000,   // 10^7
            };

            private static readonly int[] s_Exp10BigNumTableIndices = new int[]
            {
                0,          // 10^8
                2,          // 10^16
                5,          // 10^32
                10,         // 10^64
                18,         // 10^128
                33,         // 10^256
                61,         // 10^512
                116,        // 10^1024
            };

            private static readonly uint[] s_Exp10BigNumTable = new uint[]
            {
                // 10^8
                1,          // _length
                100000000,  // _blocks

                // 10^16
                2,          // _length
                0x6FC10000, // _blocks
                0x002386F2,

                // 10^32
                4,          // _length
                0x00000000, // _blocks
                0x85ACEF81,
                0x2D6D415B,
                0x000004EE,

                // 10^64
                7,          // _length
                0x00000000, // _blocks
                0x00000000,
                0xBF6A1F01,
                0x6E38ED64,
                0xDAA797ED,
                0xE93FF9F4,
                0x00184F03,

                // 10^128
                14,         // _length
                0x00000000, // _blocks
                0x00000000,
                0x00000000,
                0x00000000,
                0x2E953E01,
                0x03DF9909,
                0x0F1538FD,
                0x2374E42F,
                0xD3CFF5EC,
                0xC404DC08,
                0xBCCDB0DA,
                0xA6337F19,
                0xE91F2603,
                0x0000024E,

                // 10^256
                27,         // _length
                0x00000000, // _blocks
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x982E7C01,
                0xBED3875B,
                0xD8D99F72,
                0x12152F87,
                0x6BDE50C6,
                0xCF4A6E70,
                0xD595D80F,
                0x26B2716E,
                0xADC666B0,
                0x1D153624,
                0x3C42D35A,
                0x63FF540E,
                0xCC5573C0,
                0x65F9EF17,
                0x55BC28F2,
                0x80DCC7F7,
                0xF46EEDDC,
                0x5FDCEFCE,
                0x000553F7,

                // 10^512
                54,         // _length
                0x00000000, // _blocks
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0xFC6CF801,
                0x77F27267,
                0x8F9546DC,
                0x5D96976F,
                0xB83A8A97,
                0xC31E1AD9,
                0x46C40513,
                0x94E65747,
                0xC88976C1,
                0x4475B579,
                0x28F8733B,
                0xAA1DA1BF,
                0x703ED321,
                0x1E25CFEA,
                0xB21A2F22,
                0xBC51FB2E,
                0x96E14F5D,
                0xBFA3EDAC,
                0x329C57AE,
                0xE7FC7153,
                0xC3FC0695,
                0x85A91924,
                0xF95F635E,
                0xB2908EE0,
                0x93ABADE4,
                0x1366732A,
                0x9449775C,
                0x69BE5B0E,
                0x7343AFAC,
                0xB099BC81,
                0x45A71D46,
                0xA2699748,
                0x8CB07303,
                0x8A0B1F13,
                0x8CAB8A97,
                0xC1D238D9,
                0x633415D4,
                0x0000001C,

                // 10^1024
                107,        // _length
                0x00000000, // _blocks
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x2919F001,
                0xF55B2B72,
                0x6E7C215B,
                0x1EC29F86,
                0x991C4E87,
                0x15C51A88,
                0x140AC535,
                0x4C7D1E1A,
                0xCC2CD819,
                0x0ED1440E,
                0x896634EE,
                0x7DE16CFB,
                0x1E43F61F,
                0x9FCE837D,
                0x231D2B9C,
                0x233E55C7,
                0x65DC60D7,
                0xF451218B,
                0x1C5CD134,
                0xC9635986,
                0x922BBB9F,
                0xA7E89431,
                0x9F9F2A07,
                0x62BE695A,
                0x8E1042C4,
                0x045B7A74,
                0x1ABE1DE3,
                0x8AD822A5,
                0xBA34C411,
                0xD814B505,
                0xBF3FDEB3,
                0x8FC51A16,
                0xB1B896BC,
                0xF56DEEEC,
                0x31FB6BFD,
                0xB6F4654B,
                0x101A3616,
                0x6B7595FB,
                0xDC1A47FE,
                0x80D98089,
                0x80BDA5A5,
                0x9A202882,
                0x31EB0F66,
                0xFC8F1F90,
                0x976A3310,
                0xE26A7B7E,
                0xDF68368A,
                0x3CE3A0B8,
                0x8E4262CE,
                0x75A351A2,
                0x6CB0B6C9,
                0x44597583,
                0x31B5653F,
                0xC356E38A,
                0x35FAABA6,
                0x0190FBA0,
                0x9FC4ED52,
                0x88BC491B,
                0x1640114A,
                0x005B8041,
                0xF4F3235E,
                0x1E8D4649,
                0x36A8DE06,
                0x73C55349,
                0xA7E6BD2A,
                0xC1A6970C,
                0x47187094,
                0xD2DB49EF,
                0x926C3F5B,
                0xAE6209D4,
                0x2D433949,
                0x34F4A3C6,
                0xD4305D94,
                0xD9D61A05,
                0x00000325,

                // 9 Trailing blocks to ensure MaxBlockCount
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
                0x00000000,
            };

            private unsafe fixed UInt32 _blocks[MaxBlockCount];

            private int _length {

                get => MaxBlockCount;

                set {
                }
            }

            private ref UInt64 GetDigit(int index) {
                return ref Unsafe.Add(ref Unsafe.As<UInt32, UInt64>(ref _blocks[0]), index);
            }

            public static explicit operator UInt256Builder(BigInteger value) {
                UInt256Builder result = default;
                if (!value.TryWriteBytes(new Span<byte>(&result._blocks[0], 32), out _, true)) {
                    _ = checked(0 - 1);
                }
                return result;
            }

            public static implicit operator BigInteger(UInt256Builder value) {
                Span<byte> buffer = stackalloc byte[32];
                ref var a = ref Unsafe.As<byte, UInt64>(ref buffer[0]);
                ref var b = ref Unsafe.As<UInt32, UInt64>(ref value._blocks[0]);

                Unsafe.Add(ref a, 0) = Unsafe.Add(ref b, 0);
                Unsafe.Add(ref a, 1) = Unsafe.Add(ref b, 1);
                Unsafe.Add(ref a, 2) = Unsafe.Add(ref b, 2);
                Unsafe.Add(ref a, 3) = Unsafe.Add(ref b, 3);

                return new BigInteger(buffer, true);
            }

            public UInt256Builder(UInt32 value) {
                _blocks[0] = value;
                _blocks[1] = 0;

                GetDigit(1) = default;
                GetDigit(2) = default;
                GetDigit(3) = default;

                _length = (value == 0) ? 0 : 1;
            }

            public UInt256Builder(UInt64 value) {
                UInt32 lower = (UInt32)(value);
                UInt32 upper = (UInt32)(value >> 32);

                _blocks[0] = lower;
                _blocks[1] = upper;

                GetDigit(1) = default;
                GetDigit(2) = default;
                GetDigit(3) = default;

                _length = (upper == 0) ? 1 : 2;
            }

            public static int Add(in UInt256Builder lhs, in UInt256Builder rhs, out UInt256Builder result) {
                // determine which operand has the smaller length
                ref readonly UInt256Builder large = ref (lhs._length < rhs._length) ? ref rhs : ref lhs;
                ref readonly UInt256Builder small = ref (lhs._length < rhs._length) ? ref lhs : ref rhs;

                int largeLength = large._length;
                int smallLength = small._length;

                // The output will be at least as long as the largest input
                result = new UInt256Builder(0);
                result._length = largeLength;

                // Add each block and add carry the overflow to the next block
                ulong carry = 0;

                int largeIndex = 0;
                int smallIndex = 0;
                int resultIndex = 0;

                while (smallIndex < smallLength) {
                    ulong sum = carry + large._blocks[largeIndex] + small._blocks[smallIndex];
                    carry = sum >> 32;
                    result._blocks[resultIndex] = (uint)(sum);

                    largeIndex++;
                    smallIndex++;
                    resultIndex++;
                }

                // Add the carry to any blocks that only exist in the large operand
                while (largeIndex < largeLength) {
                    ulong sum = carry + large._blocks[largeIndex];
                    carry = sum >> 32;
                    result._blocks[resultIndex] = (uint)(sum);

                    largeIndex++;
                    resultIndex++;
                }

                // If there's still a carry, append a new block
                //if (carry != 0) {
                //    Debug.Assert(carry == 1);
                //    Debug.Assert((resultIndex == largeLength) && (largeLength < MaxBlockCount));

                //    result._blocks[resultIndex] = 1;
                //    result._length++;
                //}
                return unchecked((int)carry);
            }

            public static int Compare(in UInt256Builder lhs, in UInt256Builder rhs) {
                Debug.Assert(unchecked((uint)(lhs._length)) <= MaxBlockCount);
                Debug.Assert(unchecked((uint)(rhs._length)) <= MaxBlockCount);

                int lhsLength = lhs._length;
                int rhsLength = rhs._length;

                int lengthDelta = (lhsLength - rhsLength);

                if (lengthDelta != 0) {
                    return lengthDelta;
                }

                if (lhsLength == 0) {
                    Debug.Assert(rhsLength == 0);
                    return 0;
                }

                for (int index = (lhsLength - 1); index >= 0; index--) {
                    long delta = (long)(lhs._blocks[index]) - rhs._blocks[index];

                    if (delta != 0) {
                        return delta > 0 ? 1 : -1;
                    }
                }

                return 0;
            }

            public static uint CountSignificantBits(uint value) {
                return 32 - (uint)BinaryNumerals.CountLeadingZeros(value);
            }

            public static uint CountSignificantBits(ulong value) {
                return 64 - (uint)BinaryNumerals.CountLeadingZeros(value);
            }

            public static uint CountSignificantBits(in UInt256Builder value) {
                if (value.IsZero()) {
                    return 0;
                }

                // We don't track any unused blocks, so we only need to do a BSR on the
                // last index and add that to the number of bits we skipped.

                uint lastIndex = (uint)(value._length - 1);
                return (lastIndex * BitsPerBlock) + CountSignificantBits(value._blocks[lastIndex]);
            }

            public static void DivRem(in UInt256Builder lhs, in UInt256Builder rhs, out UInt256Builder quo, out UInt256Builder rem) {
                // This is modified from the CoreFX BigInteger.DivRem.cs implementation:
                // https://github.com/dotnet/corefx/blob/0bb106232745aedfc0d0c5a84ff2b244bf190317/src/System.Runtime.Numerics/src/System/Numerics/BigInteger.DivRem.cs

                Debug.Assert(!rhs.IsZero());

                quo = new UInt256Builder(0);
                rem = new UInt256Builder(0);

                if (lhs.IsZero()) {
                    return;
                }

                int lhsLength = lhs._length;
                int rhsLength = rhs._length;

                if ((lhsLength == 1) && (rhsLength == 1)) {
                    uint quotient = InternalMath.DivRem(lhs._blocks[0], rhs._blocks[0], out uint remainder);
                    quo = new UInt256Builder(quotient);
                    rem = new UInt256Builder(remainder);
                    return;
                }

                if (0 == (rhs.GetDigit(1) | rhs.GetDigit(2) | rhs.GetDigit(3))/* rhsLength == 1 */) {
                    // We can make the computation much simpler if the rhs is only one block

                    int quoLength = lhsLength;

                    ulong rhsValue = rhs._blocks[0];
                    ulong carry = 0;

                    for (int i = quoLength - 1; i >= 0; i--) {
                        ulong value = (carry << 32) | lhs._blocks[i];
                        ulong digit = InternalMath.DivRem(value, rhsValue, out carry);

                        if ((digit == 0) && (i == (quoLength - 1))) {
                            quoLength--;
                        } else {
                            quo._blocks[i] = (uint)(digit);
                        }
                    }

                    quo._length = quoLength;
                    rem.SetUInt32((uint)(carry));

                    return;
                } else if (rhsLength > lhsLength) {
                    // Handle the case where we have no quotient
                    rem.SetValue(in lhs);
                    return;
                } else {
                    int quoLength = lhsLength - rhsLength + 1;
                    rem.SetValue(in lhs);
                    int remLength = lhsLength;

                    // Executes the "grammar-school" algorithm for computing q = a / b.
                    // Before calculating q_i, we get more bits into the highest bit
                    // block of the divisor. Thus, guessing digits of the quotient
                    // will be more precise. Additionally we'll get r = a % b.

                    uint divHi = rhs._blocks[rhsLength - 1];
                    uint divLo = rhs._blocks[rhsLength - 2];

                    // We measure the leading zeros of the divisor
                    int shiftLeft = BinaryNumerals.CountLeadingZeros(divHi);
                    int shiftRight = 32 - shiftLeft;

                    // And, we make sure the most significant bit is set
                    if (shiftLeft > 0) {
                        divHi = (divHi << shiftLeft) | (divLo >> shiftRight);
                        divLo <<= shiftLeft;

                        if (rhsLength > 2) {
                            divLo |= (rhs._blocks[rhsLength - 3] >> shiftRight);
                        }
                    }

                    // Then, we divide all of the bits as we would do it using
                    // pen and paper: guessing the next digit, subtracting, ...
                    for (int i = lhsLength; i >= rhsLength; i--) {
                        int n = i - rhsLength;
                        uint t = i < lhsLength ? rem._blocks[i] : 0;

                        ulong valHi = ((ulong)(t) << 32) | rem._blocks[i - 1];
                        uint valLo = i > 1 ? rem._blocks[i - 2] : 0;

                        // We shifted the divisor, we shift the dividend too
                        if (shiftLeft > 0) {
                            valHi = (valHi << shiftLeft) | (valLo >> shiftRight);
                            valLo <<= shiftLeft;

                            if (i > 2) {
                                valLo |= (rem._blocks[i - 3] >> shiftRight);
                            }
                        }

                        // First guess for the current digit of the quotient,
                        // which naturally must have only 32 bits...
                        ulong digit = valHi / divHi;

                        if (digit > uint.MaxValue) {
                            digit = uint.MaxValue;
                        }

                        // Our first guess may be a little bit to big
                        while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo)) {
                            digit--;
                        }

                        if (digit > 0) {
                            // Now it's time to subtract our current quotient
                            uint carry = SubtractDivisor(ref rem, n, in rhs, digit);

                            if (carry != t) {
                                Debug.Assert(carry == t + 1);

                                // Our guess was still exactly one too high
                                carry = AddDivisor(ref rem, n, in rhs);
                                digit--;

                                Debug.Assert(carry == 1);
                            }
                        }

                        // We have the digit!
                        if (quoLength != 0) {
                            if ((digit == 0) && (n == (quoLength - 1))) {
                                quoLength--;
                            } else {
                                quo._blocks[n] = (uint)(digit);
                            }
                        }

                        if (i < remLength) {
                            remLength--;
                        }
                    }

                    quo._length = quoLength;

                    // We need to check for the case where remainder is zero

                    for (int i = remLength - 1; i >= 0; i--) {
                        if (rem._blocks[i] == 0) {
                            remLength--;
                        }
                    }

                    rem._length = remLength;
                }
            }

            public static uint HeuristicDivide(ref UInt256Builder dividend, in UInt256Builder divisor) {
                int divisorLength = divisor._length;

                if (dividend._length < divisorLength) {
                    return 0;
                }

                // This is an estimated quotient. Its error should be less than 2.
                // Reference inequality:
                // a/b - floor(floor(a)/(floor(b) + 1)) < 2
                int lastIndex = (divisorLength - 1);
                uint quotient = dividend._blocks[lastIndex] / (divisor._blocks[lastIndex] + 1);

                if (quotient != 0) {
                    // Now we use our estimated quotient to update each block of dividend.
                    // dividend = dividend - divisor * quotient
                    int index = 0;

                    ulong borrow = 0;
                    ulong carry = 0;

                    do {
                        ulong product = ((ulong)(divisor._blocks[index]) * quotient) + carry;
                        carry = product >> 32;

                        ulong difference = (ulong)(dividend._blocks[index]) - (uint)(product) - borrow;
                        borrow = (difference >> 32) & 1;

                        dividend._blocks[index] = (uint)(difference);

                        index++;
                    }
                    while (index < divisorLength);

                    // Remove all leading zero blocks from dividend
                    while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0)) {
                        divisorLength--;
                    }

                    dividend._length = divisorLength;
                }

                // If the dividend is still larger than the divisor, we overshot our estimate quotient. To correct,
                // we increment the quotient and subtract one more divisor from the dividend (Because we guaranteed the error range).
                if (Compare(in dividend, in divisor) >= 0) {
                    quotient++;

                    // dividend = dividend - divisor
                    int index = 0;
                    ulong borrow = 0;

                    do {
                        ulong difference = (ulong)(dividend._blocks[index]) - divisor._blocks[index] - borrow;
                        borrow = (difference >> 32) & 1;

                        dividend._blocks[index] = (uint)(difference);

                        index++;
                    }
                    while (index < divisorLength);

                    // Remove all leading zero blocks from dividend
                    while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0)) {
                        divisorLength--;
                    }

                    dividend._length = divisorLength;
                }

                return quotient;
            }

            public static void Multiply(in UInt256Builder lhs, uint value, out UInt256Builder result) {
                if (lhs.IsZero() || (value == 1)) {
                    result.SetValue(in lhs);
                    return;
                }

                if (value == 0) {
                    result.SetZero();
                    return;
                }

                int lhsLength = lhs._length;
                int index = 0;
                uint carry = 0;

                while (index < lhsLength) {
                    ulong product = ((ulong)(lhs._blocks[index]) * value) + carry;
                    result._blocks[index] = (uint)(product);
                    carry = (uint)(product >> 32);

                    index++;
                }

                if (carry != 0) {
                    Debug.Assert(unchecked((uint)(lhsLength)) + 1 <= MaxBlockCount);
                    result._blocks[index] = carry;
                    result._length = (lhsLength + 1);
                } else {
                    result._length = lhsLength;
                }
            }

            public static void Multiply(in UInt256Builder lhs, in UInt256Builder rhs, out UInt256Builder result) {
                if (lhs.IsZero() || rhs.IsOne()) {
                    result.SetValue(in lhs);
                    return;
                }

                if (rhs.IsZero()) {
                    result.SetZero();
                    return;
                }

                ref readonly UInt256Builder large = ref lhs;
                int largeLength = lhs._length;

                ref readonly UInt256Builder small = ref rhs;
                int smallLength = rhs._length;

                if (largeLength < smallLength) {
                    large = ref rhs;
                    largeLength = rhs._length;

                    small = ref lhs;
                    smallLength = lhs._length;
                }

                int maxResultLength = smallLength + largeLength;
                Debug.Assert(unchecked((uint)(maxResultLength)) <= MaxBlockCount);

                // Zero out result internal blocks.
                for (var i = 0; maxResultLength > i; ++i) {
                    result._blocks[i] = 0;
                }

                int smallIndex = 0;
                int resultStartIndex = 0;

                while (smallIndex < smallLength) {
                    // Multiply each block of large BigNum.
                    if (small._blocks[smallIndex] != 0) {
                        int largeIndex = 0;
                        int resultIndex = resultStartIndex;

                        ulong carry = 0;

                        do {
                            ulong product = result._blocks[resultIndex] + ((ulong)(small._blocks[smallIndex]) * large._blocks[largeIndex]) + carry;
                            carry = product >> 32;
                            result._blocks[resultIndex] = (uint)(product);

                            resultIndex++;
                            largeIndex++;
                        }
                        while (largeIndex < largeLength);

                        result._blocks[resultIndex] = (uint)(carry);
                    }

                    smallIndex++;
                    resultStartIndex++;
                }

                if ((maxResultLength > 0) && (result._blocks[maxResultLength - 1] == 0)) {
                    result._length = (maxResultLength - 1);
                } else {
                    result._length = maxResultLength;
                }
            }

            public static void Pow2(uint exponent, out UInt256Builder result) {
                uint blocksToShift = DivRem32(exponent, out uint remainingBitsToShift);
                result._length = (int)blocksToShift + 1;
                Debug.Assert(unchecked((uint)result._length) <= MaxBlockCount);

                for (var i = 0; blocksToShift > i; ++i) {
                    result._blocks[i] = 0;
                }

                result._blocks[blocksToShift] = 1U << (int)(remainingBitsToShift);
            }

            public static void Exp10(uint exponent, out UInt256Builder result) {
                // We leverage two arrays - s_Exp10UInt32Table and s_Exp10BigNumTable to speed up the Exp10 calculation.
                //
                // s_Exp10UInt32Table stores the results of 10^0 to 10^7.
                // s_Exp10BigNumTable stores the results of 10^8, 10^16, 10^32, 10^64, 10^128, 10^256, and 10^512
                //
                // For example, let's say exp = 0b111111. We can split the exp to two parts, one is small exp,
                // which 10^smallExp can be represented as uint, another part is 10^bigExp, which must be represented as BigNum.
                // So the result should be 10^smallExp * 10^bigExp.
                //
                // Calculating 10^smallExp is simple, we just lookup the 10^smallExp from s_Exp10UInt32Table.
                // But here's a bad news: although uint can represent 10^9, exp 9's binary representation is 1001.
                // That means 10^(1011), 10^(1101), 10^(1111) all cannot be stored as uint, we cannot easily say something like:
                // "Any bits <= 3 is small exp, any bits > 3 is big exp". So instead of involving 10^8, 10^9 to s_Exp10UInt32Table,
                // consider 10^8 and 10^9 as a bigNum, so they fall into s_Exp10BigNumTable. Now we can have a simple rule:
                // "Any bits <= 3 is small exp, any bits > 3 is big exp".
                //
                // For 0b111111, we first calculate 10^(smallExp), which is 10^(7), now we can shift right 3 bits, prepare to calculate the bigExp part,
                // the exp now becomes 0b000111.
                //
                // Apparently the lowest bit of bigExp should represent 10^8 because we have already shifted 3 bits for smallExp, so s_Exp10BigNumTable[0] = 10^8.
                // Now let's shift exp right 1 bit, the lowest bit should represent 10^(8 * 2) = 10^16, and so on...
                //
                // That's why we just need the values of s_Exp10BigNumTable be power of 2.
                //
                // More details of this implementation can be found at: https://github.com/dotnet/coreclr/pull/12894#discussion_r128890596

                // Validate that `s_Exp10BigNumTable` has exactly enough trailing elements to fill a UInt256Builder (which contains MaxBlockCount + 1 elements)
                // We validate here, since this is the only current consumer of the array
                Debug.Assert((s_Exp10BigNumTableIndices[^1] + MaxBlockCount + 2) == s_Exp10BigNumTable.Length);

                UInt256Builder temp1 = new UInt256Builder(s_Exp10UInt32Table[exponent & 0x7]);
                ref UInt256Builder lhs = ref temp1;

                UInt256Builder temp2 = new UInt256Builder(0);
                ref UInt256Builder product = ref temp2;

                exponent >>= 3;
                uint index = 0;

                while (exponent != 0) {
                    // If the current bit is set, multiply it with the corresponding power of 10
                    if ((exponent & 1) != 0) {
                        // Multiply into the next temporary
                        fixed (uint* pBigNumEntry = &s_Exp10BigNumTable[s_Exp10BigNumTableIndices[index]]) {
                            ref UInt256Builder rhs = ref *(UInt256Builder*)(pBigNumEntry);
                            Multiply(in lhs, in rhs, out product);
                        }

                        // Swap to the next temporary
                        ref UInt256Builder temp = ref product;
                        product = ref lhs;
                        lhs = ref temp;
                    }

                    // Advance to the next bit
                    ++index;
                    exponent >>= 1;
                }

                result = new UInt256Builder(0);
                result.SetValue(in lhs);
            }

            private static uint AddDivisor(ref UInt256Builder lhs, int lhsStartIndex, in UInt256Builder rhs) {
                int lhsLength = lhs._length;
                int rhsLength = rhs._length;

                Debug.Assert(lhsLength >= 0);
                Debug.Assert(rhsLength >= 0);
                Debug.Assert(lhsLength >= rhsLength);

                // Repairs the dividend, if the last subtract was too much

                ulong carry = 0UL;

                for (int i = 0; i < rhsLength; i++) {
                    ref uint lhsValue = ref lhs._blocks[lhsStartIndex + i];

                    ulong digit = lhsValue + carry + rhs._blocks[i];
                    lhsValue = unchecked((uint)digit);
                    carry = digit >> 32;
                }

                return (uint)(carry);
            }

            private static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo, uint divHi, uint divLo) {
                Debug.Assert(q <= 0xFFFFFFFF);

                // We multiply the two most significant limbs of the divisor
                // with the current guess for the quotient. If those are bigger
                // than the three most significant limbs of the current dividend
                // we return true, which means the current guess is still too big.

                ulong chkHi = divHi * q;
                ulong chkLo = divLo * q;

                chkHi += (chkLo >> 32);
                chkLo &= uint.MaxValue;

                if (chkHi < valHi)
                    return false;

                if (chkHi > valHi)
                    return true;

                if (chkLo < valLo)
                    return false;

                if (chkLo > valLo)
                    return true;

                return false;
            }

            private static uint SubtractDivisor(ref UInt256Builder lhs, int lhsStartIndex, in UInt256Builder rhs, ulong q) {
                int lhsLength = lhs._length - lhsStartIndex;
                int rhsLength = rhs._length;

                Debug.Assert(lhsLength >= 0);
                Debug.Assert(rhsLength >= 0);
                Debug.Assert(lhsLength >= rhsLength);
                Debug.Assert(q <= uint.MaxValue);

                // Combines a subtract and a multiply operation, which is naturally
                // more efficient than multiplying and then subtracting...

                ulong carry = 0;

                for (int i = 0; i < rhsLength; i++) {
                    carry += rhs._blocks[i] * q;
                    uint digit = unchecked((uint)carry);
                    carry >>= 32;

                    ref uint lhsValue = ref lhs._blocks[lhsStartIndex + i];

                    if (lhsValue < digit) {
                        carry++;
                    }

                    lhsValue = unchecked(lhsValue - digit);
                }

                return (uint)(carry);
            }

            public void Add(uint value) {
                int length = _length;
                if (length == 0) {
                    SetUInt32(value);
                    return;
                }

                _blocks[0] += value;
                if (_blocks[0] >= value) {
                    // No carry
                    return;
                }

                for (int index = 1; index < length; index++) {
                    _blocks[index]++;
                    if (_blocks[index] > 0) {
                        // No carry
                        return;
                    }
                }

                Debug.Assert(unchecked((uint)(length)) + 1 <= MaxBlockCount);
                _blocks[length] = 1;
                _length = length + 1;
            }

            public uint GetBlock(uint index) {
                Debug.Assert(index < _length);
                return _blocks[index];
            }

            public bool IsOne() {
                return
                    1 == GetDigit(0) &&
                    0 == GetDigit(1) &&
                    0 == GetDigit(2) &&
                    0 == GetDigit(3);
            }

            public bool IsZero() {
                // return _length == 0;
                return
                    0 == GetDigit(0) &&
                    0 == GetDigit(1) &&
                    0 == GetDigit(2) &&
                    0 == GetDigit(3);
            }

            public void Multiply(uint value) {
                Multiply(in this, value, out this);
            }

            public void Multiply(in UInt256Builder value) {
                UInt256Builder temp = new UInt256Builder(0);
                temp.SetValue(in this);
                Multiply(in temp, in value, out this);
            }

            public void Multiply10() {
                if (IsZero()) {
                    return;
                }

                int index = 0;
                int length = _length;
                ulong carry = 0;

                while (index < length) {
                    ulong block = (ulong)(_blocks[index]);
                    ulong product = (block << 3) + (block << 1) + carry;
                    carry = product >> 32;
                    _blocks[index] = (uint)(product);

                    index++;
                }

                if (carry != 0) {
                    Debug.Assert(unchecked((uint)(_length)) + 1 <= MaxBlockCount);
                    _blocks[index] = (uint)carry;
                    _length++;
                }
            }

            public void MultiplyExp10(uint exponent) {
                if (IsZero()) {
                    return;
                }

                Exp10(exponent, out UInt256Builder poweredValue);

                if (poweredValue._length == 1) {
                    Multiply(poweredValue._blocks[0]);
                } else {
                    Multiply(in poweredValue);
                }
            }

            public void SetUInt32(uint value) {
                _blocks[0] = value;
                _blocks[1] = 0;
                GetDigit(1) = 0;
                GetDigit(2) = 0;
                GetDigit(3) = 0;
            }

            public void SetUInt64(ulong value) {
                GetDigit(0) = value;
                GetDigit(1) = 0;
                GetDigit(2) = 0;
                GetDigit(3) = 0;
            }

            public void SetValue(in UInt256Builder rhs) {
                int rhsLength = rhs._length;
                Unsafe.As<UInt32, UInt64>(ref this._blocks[0]) = Unsafe.As<UInt32, UInt64>(ref Unsafe.AsRef(in rhs._blocks[0]));
                Unsafe.As<UInt32, UInt64>(ref this._blocks[2]) = Unsafe.As<UInt32, UInt64>(ref Unsafe.AsRef(in rhs._blocks[2]));
                Unsafe.As<UInt32, UInt64>(ref this._blocks[4]) = Unsafe.As<UInt32, UInt64>(ref Unsafe.AsRef(in rhs._blocks[4]));
                Unsafe.As<UInt32, UInt64>(ref this._blocks[6]) = Unsafe.As<UInt32, UInt64>(ref Unsafe.AsRef(in rhs._blocks[6]));
                _length = rhsLength;
            }

            public void SetZero() {
                GetDigit(0) = 0;
                GetDigit(1) = 0;
                GetDigit(2) = 0;
                GetDigit(3) = 0;
            }

            public void ShiftLeft(uint shift) {
                // Process blocks high to low so that we can safely process in place
                int length = _length;

                if ((length == 0) || (shift == 0)) {
                    return;
                }

                uint blocksToShift = DivRem32(shift, out uint remainingBitsToShift);

                // Copy blocks from high to low
                int readIndex = (length - 1);
                int writeIndex = readIndex + (int)(blocksToShift);

                // Check if the shift is block aligned
                if (remainingBitsToShift == 0) {
                    Debug.Assert(writeIndex < MaxBlockCount);

                    while (readIndex >= 0) {
                        _blocks[writeIndex] = _blocks[readIndex];
                        readIndex--;
                        writeIndex--;
                    }

                    _length += (int)(blocksToShift);

                    // Zero the remaining low blocks
                    for (var i = 0; blocksToShift > i; ++i) {
                        _blocks[i] = 0;
                    }

                } else {
                    // We need an extra block for the partial shift
                    writeIndex++;
                    Debug.Assert(writeIndex < MaxBlockCount);

                    // Set the length to hold the shifted blocks
                    _length = writeIndex + 1;

                    // Output the initial blocks
                    uint lowBitsShift = (32 - remainingBitsToShift);
                    uint highBits = 0;
                    uint block = _blocks[readIndex];
                    uint lowBits = block >> (int)(lowBitsShift);
                    while (readIndex > 0) {
                        _blocks[writeIndex] = highBits | lowBits;
                        highBits = block << (int)(remainingBitsToShift);

                        --readIndex;
                        --writeIndex;

                        block = _blocks[readIndex];
                        lowBits = block >> (int)lowBitsShift;
                    }

                    // Output the final blocks
                    _blocks[writeIndex] = highBits | lowBits;
                    _blocks[writeIndex - 1] = block << (int)(remainingBitsToShift);

                    // Zero the remaining low blocks
                    for (var i = 0; blocksToShift > i; ++i) {
                        _blocks[i] = 0;
                    }

                    // Check if the terminating block has no set bits
                    if (_blocks[_length - 1] == 0) {
                        _length--;
                    }
                }
            }

            private static uint DivRem32(uint value, out uint remainder) {
                remainder = value & 31;
                return value >> 5;
            }
        }
        public struct WrappedEnumerable<T> : IEnumerable<T> {

            readonly IEnumerable<T> _underlaying;

            WrappedEnumerator<T> _cachedEnumerator;

            public WrappedEnumerable(IEnumerable<T> enumerable) {
                _underlaying = enumerable;
                _cachedEnumerator = null;
            }

            public IEnumerator<T> GetEnumerator() {
                return _cachedEnumerator = new WrappedEnumerator<T>(_underlaying.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            IEnumerator<T> GetEnumerator(long offset) {
                var s = _cachedEnumerator;
                if (offset == checked(1 + s._currentIndex)) {
                    unchecked {
                        s._maxIndex += 0x40000000;
                    }
                    return s;
                }
                var t = GetEnumerator() as WrappedEnumerator<T>;
                for (var i = 0; offset > i; ++i) {
                    t.MoveNext();
                }
                if (t._underlaying != s._underlaying) {
                    s.Dispose();
                }
                return _cachedEnumerator = t;
            }
        }

        public class WrappedEnumerator<T> : IEnumerator<T> {

            internal readonly IEnumerator<T> _underlaying;

            internal long _currentIndex;
            internal long _maxIndex;
            public T Current {

                get => _underlaying.Current;
            }

            object IEnumerator.Current {

                get => _underlaying.Current;
            }

            public bool MoveNext() {
                if (_currentIndex <= _maxIndex) {
                    var a = _underlaying.MoveNext();
                    checked {
                        ++_currentIndex;
                    }
                    return a;
                }
                return false;
            }

            public void Reset() {
                _underlaying.Reset();
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            public WrappedEnumerator(IEnumerator<T> enumerator) {
                _underlaying = enumerator;
                _currentIndex = -1;
                _maxIndex = 0x3FFFFFFF;
            }

            // This code added to correctly implement the disposable pattern.
            public void Dispose() {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing) {
                if (!disposedValue) {
                    if (disposing) {
                        // TODO: dispose managed state (managed objects).

                        // _underlaying.Dispose();
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~aaaabss()
            // {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }
            #endregion
        }

        private class ssss : IEqualityComparer<Quadruple> {

            public bool Equals([AllowNull] Quadruple x, [AllowNull] Quadruple y) {
                if (x.Equals(y)) {
                    return true;
                } else {
                    if (Quadruple.IsFinite(y - x) && y <= Quadruple.Math.BitIncrement(x) && y >= Quadruple.Math.BitDecrement(x)) {
                        return true;
                    }
                    // ???
                    return false;
                }
            }

            public int GetHashCode([DisallowNull] Quadruple obj) {
                return obj.GetHashCode();
            }
        }
    }
}
