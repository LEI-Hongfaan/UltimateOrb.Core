using System;

namespace UltimateOrb.Numerics {

    public static partial class DoubleArithmeticF {

        // Veltkamp’s splitter (= 1 + Pow(2, 27))
        const System.Double SplitterVeltkamp = 134217729;

        const System.Double SplitThreshold = 6.69692879491417e+299;

        // Møller’s and Knuth’s summation
        [System.CLSCompliantAttribute(false)]
        public static System.Double ToDouble(UInt64 value, out System.Double result_hi) {
            unchecked {
                var lo = (System.Double)(UInt32)value;
                var hi = (System.Double)(0xFFFFFFFF00000000u & value);
                var fp_hi = hi + lo;
                var t = fp_hi - lo;
                result_hi = fp_hi;
                return (hi - t) + (lo - (fp_hi - t));
            }
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static bool IsFinite(System.Double value) {
#if NETSTANDARD2_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NET5_0_OR_GREATER
            return Double.IsFinite(value);
#else
            return 0x7FF0000000000000 > (0x7FFFFFFFFFFFFFFF & BitConverter.DoubleToInt64Bits(value));
#endif
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigAddPartial(System.Double first, System.Double second, out System.Double result_hi) {
            System.Diagnostics.Debug.Assert(Math.Abs(first) >= Math.Abs(second) || !IsFinite(first) || !IsFinite(second));
            var t = first + second;
            result_hi = t;
            return (first - t) + second;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigSubtractPartial(System.Double first, System.Double second, out System.Double result_hi) {
            System.Diagnostics.Debug.Assert(Math.Abs(first) >= Math.Abs(second) || !IsFinite(first) || !IsFinite(second));
            var t = first - second;
            result_hi = t;
            return (first - t) - second;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigAdd(System.Double first, System.Double second, out System.Double result_hi) {
            var t = first + second;
            var s = t - first;
            result_hi = t;
            return (first - (t - s)) + (second - s);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigSubtract(System.Double first, System.Double second, out System.Double result_hi) {
            var t = first - second;
            var s = t - first;
            result_hi = t;
            return (first - (t - s)) - (second + s);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Add(System.Double first_lo, System.Double first_hi, System.Double second, Void _, out System.Double result_hi) {
            var tl = BigAdd(first_hi, second, out var th);
            tl += first_lo;
            th = BigAddPartial(th, tl, out tl);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Add(System.Double first, Void _, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigAdd(first, second_hi, out var th);
            tl += second_lo;
            th = BigAddPartial(th, tl, out tl);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Add(System.Double first_lo, System.Double first_hi, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            // K. Briggs and W. Kahan.
            var tl = BigAdd(first_hi, second_hi, out var th);
            var el = BigAdd(first_lo, second_lo, out var eh);
            tl += eh;
            tl = BigAddPartial(th, tl, out th);
            tl += el;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double AddRough(System.Double first_lo, System.Double first_hi, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigAdd(first_hi, second_hi, out var th);
            tl += first_lo + second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Subtract(System.Double first_lo, System.Double first_hi, System.Double second, Void _, out System.Double result_hi) {
            var tl = BigSubtract(first_hi, second, out var th);
            tl += first_lo;
            th = BigAddPartial(th, tl, out tl);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Subtract(System.Double first, Void _, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigSubtract(first, second_hi, out var th);
            tl -= second_lo;
            th = BigAddPartial(th, tl, out tl);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Subtract(System.Double first_lo, System.Double first_hi, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigSubtract(first_hi, second_hi, out var th);
            var el = BigSubtract(first_lo, second_lo, out var eh);
            tl += eh;
            tl = BigAddPartial(th, tl, out th);
            tl += el;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double SubtractRough(System.Double first_lo, System.Double first_hi, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigSubtract(first_hi, second_hi, out var th);
            tl += first_lo - second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Split(System.Double value, out System.Double result_hi) {
            // ToDo: CopySign, ScaleBPartial
            if (Math.Abs(value) <= SplitThreshold) {
                var t = SplitterVeltkamp * value;
                var hi = t - (t - value);
                var lo = value - hi;
                result_hi = hi;
                return lo;
            } else {
                value *= 3.7252902984619140625e-09; // Pow(2, -28)
                var t = SplitterVeltkamp * value;
                var hi = t - (t - value);
                var lo = value - hi;
                hi *= 268435456.0; // Pow(2, 28)
                lo *= 268435456.0; // Pow(2, 28)
                result_hi = hi;
                return lo;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double SplitPartial(System.Double value, out System.Double result_hi) {
            var t = SplitterVeltkamp * value;
            var hi = t - (t - value);
            var lo = value - hi;
            result_hi = hi;
            return lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigMul_A_Dekker(System.Double first, System.Double second, out System.Double product_hi) {
            var p = first * second;
            var first_lo = Split(first, out var first_hi);
            var second_lo = Split(second, out var second_hi);
            product_hi = p;
            return first_lo * second_lo + (first_lo * second_hi + first_hi * second_lo + (first_hi * second_hi - p));
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigMul(System.Double first, System.Double second, out System.Double product_hi) {
            return BigMul_A_Dekker(first, second, out product_hi);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigSquare_A_Dekker(System.Double value, out System.Double product_hi) {
            var p = value * value;
            var value_lo = Split(value, out var value_hi);
            product_hi = p;
            var m = value_lo * value_hi;
            return value_lo * value_lo + (m + m + (value_hi * value_hi - p));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigSquare(System.Double first, System.Double second, out System.Double product_hi) {
            return BigSquare_A_Dekker(first, out product_hi);
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Multiply(System.Double first_lo, System.Double first_hi, Void _, System.Double second, out System.Double result_hi) {
            var tl = BigMul(first_hi, second, out var th);
            tl += first_lo * second;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Multiply(Void _, System.Double first, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigMul(first, second_hi, out var th);
            tl += first * second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Multiply(System.Double first_lo, System.Double first_hi, System.Double second_lo, System.Double second_hi, out System.Double result_hi) {
            var tl = BigMul(first_hi, second_hi, out var th);
            tl += first_lo * second_hi + first_hi * second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double Divide(System.Double dividend_lo, System.Double dividend_hi, System.Double divisor_lo, System.Double divisor_hi, out System.Double quotient_hi) {
            var q1 = dividend_hi / divisor_hi;
            var pl = Multiply(_: default, q1, divisor_lo, divisor_hi, out var ph);
            SubtractRough(dividend_lo, dividend_hi, pl, ph, out var rh);
            var q2 = rh / divisor_hi;
            pl = Multiply(_: default, q2, divisor_lo, divisor_hi, out ph);
            SubtractRough(dividend_lo, dividend_hi, pl, ph, out rh);
            var q3 = rh / divisor_hi;
            q2 = BigAddPartial(q1, q2, out q1);
            return Add(q2, q1, q3, _: default, result_hi: out quotient_hi);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double DivideRough(System.Double dividend_lo, System.Double dividend_hi, System.Double divisor_lo, System.Double divisor_hi, out System.Double quotient_hi) {
            var q1 = dividend_hi / divisor_hi;
            var rl = Multiply(divisor_lo, divisor_hi, _: default, q1, out var rh);

            var s2 = BigSubtract(dividend_hi, rh, out var s1);
            s2 -= rl;
            s2 += dividend_lo;

            var q2 = (s1 + s2) / divisor_hi;

            return BigAddPartial(q1, q2, out quotient_hi);
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigMulPartial(System.Double first, System.Double second, out System.Double product_hi) {
            var p = first * second;
            var first_lo = SplitPartial(first, out var first_hi);
            var second_lo = SplitPartial(second, out var second_hi);
            product_hi = p;
            return first_lo * second_lo + (first_lo * second_hi + first_hi * second_lo + (first_hi * second_hi - p));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double BigSquarePartial(System.Double value, out System.Double result_hi) {
            var p = value * value;
            var value_lo = SplitPartial(value, out var value_hi);
            result_hi = p;
            var m = value_lo * value_hi;
            return value_lo * value_lo + (m + m + (value_hi * value_hi - p));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static System.Double DividePartial(System.Double dividend_lo, System.Double dividend_hi, System.Double divisor_lo, System.Double divisor_hi, out System.Double quotient_hi) {
            var s = dividend_hi / divisor_hi;
            var t_lo = BigMulPartial(s, divisor_hi, out var t_hi);
            var e = (dividend_hi - t_hi - t_lo + dividend_lo - s * divisor_lo) / divisor_hi;
            var qh = s + e;
            quotient_hi = qh;
            return e - (qh - s);
        }
    }
}
