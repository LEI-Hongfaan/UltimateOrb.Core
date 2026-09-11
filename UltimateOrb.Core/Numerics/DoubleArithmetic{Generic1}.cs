using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    public static partial class DoubleArithmetic {

#if NET7_0_OR_GREATER
        internal static class Cached<T>
            where T : IBinaryInteger<T> {

            public static T Zero { get; } = T.Zero;
            public static T One { get; } = T.One;

            // Must be set correctly for each closed T.
            // For unmanaged value types, this can be Unsafe.SizeOf<T>() * 8.
            // For reference types, use an explicit mapping or another reliable source.
            public static int BitWidthOfT { get; } = checked((int)BinaryIntegerTypeTraitHelpers.GetBitSize<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Sum3<T>(T a, T b, T c, out T carry)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            T carries = Cached<T>.BitWidthOfT <= 128
                ? Cached<T>.Zero
                : +Cached<T>.Zero;

            T s = a;
            T t;

            t = s + b;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + c;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            carry = carries;
            return s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Sum4<T>(T a, T b, T c, T d, out T carry)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            T carries = Cached<T>.BitWidthOfT <= 128
                ? Cached<T>.Zero
                : +Cached<T>.Zero;

            T s = a;
            T t;

            t = s + b;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + c;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + d;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            carry = carries;
            return s;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T Sum5<T>(T a, T b, T c, T d, T e, out T carry)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            T carries = Cached<T>.BitWidthOfT <= 128
                ? Cached<T>.Zero
                : +Cached<T>.Zero;

            T s = a;
            T t;

            t = s + b;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + c;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + d;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            t = s + e;
            if (Cached<T>.BitWidthOfT <= 128) {
                carries += t < s ? Cached<T>.One : Cached<T>.Zero;
            } else
                if (t < s) {
                    ++carries;
                }

            s = t;

            carry = carries;
            return s;
        }

        [Experimental("UoWIP_GenericMath")]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static void Square2<T>(
            out T result_lo_lo, out T result_lo_hi,
            out T result_hi_lo, out T result_hi_hi, T lo, T hi)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            int w = Cached<T>.BitWidthOfT;
            T zero = Cached<T>.Zero;
            T one = Cached<T>.One;

            T ll_lo = BigMulUnsigned(lo, lo, out T ll_hi);
            T lh_lo = BigMulUnsigned(lo, hi, out T lh_hi);
            T hh_lo = BigMulUnsigned(hi, hi, out T hh_hi);

            // 2·(lo·hi) = d0 + d1·B + d2·B²
            T d0 = lh_lo << 1;
            T d1 = (lh_hi << 1) + (lh_lo >> (w - 1));
            T d2 = lh_hi >> (w - 1);

            result_lo_lo = ll_lo;

            T t = ll_hi + d0;
            T c = w <= 128 ? +zero : zero;   // fresh copy when ++ will be used
            if (t < d0) {
                if (w <= 128) {
                    ++c;
                } else {
                    c += one;
                }
            }
            result_lo_hi = t;

            T t2 = d1 + hh_lo;
            bool carry2a = t2 < hh_lo;

            T t2b = t2 + c;
            bool carry2b = t2b < c;
            result_hi_lo = t2b;

            // result_hi_hi = d2 + hh_hi + c2a + c2b
            result_hi_hi = d2 + hh_hi;

            if (carry2a) {
                if (w <= 128) {
                    ++result_hi_hi;
                } else {
                    result_hi_hi += one;
                }
            }

            if (carry2b) {
                if (w <= 128) {
                    ++result_hi_hi;
                } else {
                    result_hi_hi += one;
                }
            }
        }
#endif
    }
}
