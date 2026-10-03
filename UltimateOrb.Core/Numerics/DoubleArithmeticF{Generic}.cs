using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb.Numerics {

    static partial class DoubleArithmeticF {
        // -----------------------------------------------------------------
        // Per-T constants.  Derived once, on first use of ArithmeticF<T>.
        //
        // For T with a p-bit significand, Veltkamp splitting uses a split
        // point s = ceil(p/2), and the splitter is 2^s + 1.  We recover p
        // by binary-searching for the smallest eps with 1 + eps != 1,
        // which is 2^-(p-1).
        // -----------------------------------------------------------------
        internal static class Constants<T>
            where T : IBinaryFloatingPointIeee754<T>? {

            public static readonly T MaxValue = T.BitDecrement(T.PositiveInfinity); // 2^emax * (2 - 2^-p)

            public static readonly int Precision = -T.ILogB(T.One - T.BitDecrement(T.One)); // == number of bits in significand including the implicit leading 1

            public static readonly int SplitBits = (Precision + 1) / 2; // == ceil(p / 2)

            public static readonly T Splitter = T.ScaleB(T.One, SplitBits) + T.One; // 2^s + 1

            public static readonly T SplitThreshold = MaxValue / (Splitter + Splitter); // 2^emax * (2 - 2^-p) / (2^(s+1) + 2)

            public static readonly T Prescale = T.ScaleB(T.One, -(SplitBits + 1));             // 2^-(s+1)

            public static readonly T Postscale = T.ScaleB(T.One, SplitBits + 1);            // 2^(s+1)
        }

        // =================================================================
        // Error-free transformations
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigAddPartial<T>(T first, T second, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            System.Diagnostics.Debug.Assert(T.Abs(first) >= T.Abs(second) || !T.IsFinite(first) || !T.IsFinite(second));
            var t = first + second;
            result_hi = t;
            return (first - t) + second;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigSubtractPartial<T>(T first, T second, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            System.Diagnostics.Debug.Assert(T.Abs(first) >= T.Abs(second) || !T.IsFinite(first) || !T.IsFinite(second));
            var t = first - second;
            result_hi = t;
            return (first - t) - second;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigAdd<T>(T first, T second, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var t = first + second;
            var s = t - first;
            result_hi = t;
            return (first - (t - s)) + (second - s);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigSubtract<T>(T first, T second, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var t = first - second;
            var s = t - first;
            result_hi = t;
            return (first - (t - s)) - (second + s);
        }

        // =================================================================
        // dd + plain, plain + dd, dd + dd
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(T first_lo, T first_hi, T second, Void _, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigAdd(first_hi, second, out var th);
            tl += first_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(T first, Void _, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigAdd(first, second_hi, out var th);
            tl += second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Add<T>(T first_lo, T first_hi, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
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
        public static T AddRough<T>(T first_lo, T first_hi, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigAdd(first_hi, second_hi, out var th);
            tl += first_lo + second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        // =================================================================
        // dd - plain, plain - dd, dd - dd
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Subtract<T>(T first_lo, T first_hi, T second, Void _, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigSubtract(first_hi, second, out var th);
            tl += first_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Subtract<T>(T first, Void _, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigSubtract(first, second_hi, out var th);
            tl -= second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Subtract<T>(T first_lo, T first_hi, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
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
        public static T SubtractRough<T>(T first_lo, T first_hi, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigSubtract(first_hi, second_hi, out var th);
            tl += first_lo - second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        // =================================================================
        // Veltkamp splitting
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Split<T>(T value, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            if (T.Abs(value) <= Constants<T>.SplitThreshold) {
                var t = Constants<T>.Splitter * value;
                var hi = t - (t - value);
                var lo = value - hi;
                result_hi = hi;
                return lo;
            } else {
                value *= Constants<T>.Prescale;
                var t = Constants<T>.Splitter * value;
                var hi = t - (t - value);
                var lo = value - hi;
                hi *= Constants<T>.Postscale;
                lo *= Constants<T>.Postscale;
                result_hi = hi;
                return lo;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T SplitPartial<T>(T value, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var t = Constants<T>.Splitter * value;
            var hi = t - (t - value);
            var lo = value - hi;
            result_hi = hi;
            return lo;
        }

        // =================================================================
        // Dekker exact product / square
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigMul_A_Dekker<T>(T first, T second, out T product_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var p = first * second;
            var first_lo = Split(first, out var first_hi);
            var second_lo = Split(second, out var second_hi);
            product_hi = p;
            return (first_hi * second_hi - p) + first_hi * second_lo + first_lo * second_hi + first_lo * second_lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigMul<T>(T first, T second, out T product_hi) where T : IBinaryFloatingPointIeee754<T>? {
            return BigMul_A_Dekker(first, second, out product_hi);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigSquare_A_Dekker<T>(T value, out T product_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var p = value * value;
            var value_lo = Split(value, out var value_hi);
            product_hi = p;
            var m = value_lo * value_hi;
            return (value_hi * value_hi - p) + m + m + value_lo * value_lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigSquare<T>(T value, out T product_hi) where T : IBinaryFloatingPointIeee754<T>? {
            return BigSquare_A_Dekker(value, out product_hi);
        }

        // =================================================================
        // dd * plain, plain * dd, dd * dd
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Multiply<T>(T first_lo, T first_hi, Void _, T second, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigMul(first_hi, second, out var th);
            tl += first_lo * second;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Multiply<T>(Void _, T first, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigMul(first, second_hi, out var th);
            tl += first * second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Multiply<T>(T first_lo, T first_hi, T second_lo, T second_hi, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var tl = BigMul(first_hi, second_hi, out var th);
            tl += first_lo * second_hi + first_hi * second_lo;
            tl = BigAddPartial(th, tl, out th);
            result_hi = th;
            return tl;
        }

        // =================================================================
        // Division
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T Divide<T>(T dividend_lo, T dividend_hi, T divisor_lo, T divisor_hi, out T quotient_hi) where T : IBinaryFloatingPointIeee754<T>? {
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
        public static T DivideRough<T>(T dividend_lo, T dividend_hi, T divisor_lo, T divisor_hi, out T quotient_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var q1 = dividend_hi / divisor_hi;
            var rl = Multiply(divisor_lo, divisor_hi, _: default, q1, out var rh);

            var s2 = BigSubtract(dividend_hi, rh, out var s1);
            s2 -= rl;
            s2 += dividend_lo;

            var q2 = (s1 + s2) / divisor_hi;

            return BigAddPartial(q1, q2, out quotient_hi);
        }

        // =================================================================
        // "Partial" variants — no overflow protection on Split
        // =================================================================

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigMulPartial<T>(T first, T second, out T product_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var p = first * second;
            var first_lo = SplitPartial(first, out var first_hi);
            var second_lo = SplitPartial(second, out var second_hi);
            product_hi = p;
            return (first_hi * second_hi - p) + first_hi * second_lo + first_lo * second_hi + first_lo * second_lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T BigSquarePartial<T>(T value, out T result_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var p = value * value;
            var value_lo = SplitPartial(value, out var value_hi);
            result_hi = p;
            var m = value_lo * value_hi;
            return (value_hi * value_hi - p) + m + m + value_lo * value_lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T DividePartial<T>(T dividend_lo, T dividend_hi, T divisor_lo, T divisor_hi, out T quotient_hi) where T : IBinaryFloatingPointIeee754<T>? {
            var s = dividend_hi / divisor_hi;
            var t_lo = BigMulPartial(s, divisor_hi, out var t_hi);
            var e = (dividend_hi - t_hi - t_lo + dividend_lo - s * divisor_lo) / divisor_hi;
            var qh = s + e;
            quotient_hi = qh;
            return e - (qh - s);
        }
    }
}
