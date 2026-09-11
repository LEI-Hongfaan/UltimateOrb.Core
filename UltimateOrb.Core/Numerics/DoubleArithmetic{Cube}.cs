using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {

    public static partial class DoubleArithmetic {

        [Experimental("UoWIP_GenericMath")]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T Cube<T>(T lo, T hi,
            out T w1, out T w2, out T w3, out T w4, out T w5)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            Square2(out T sq0, out T sq1, out T sq2, out T sq3, lo, hi);

            T p0_lo = BigMulUnsigned(sq0, lo, out T p0_hi);
            T p1_lo = BigMulUnsigned(sq1, lo, out T p1_hi);
            T p2_lo = BigMulUnsigned(sq2, lo, out T p2_hi);
            T p3_lo = BigMulUnsigned(sq3, lo, out T p3_hi);
            T p4_lo = BigMulUnsigned(sq0, hi, out T p4_hi);
            T p5_lo = BigMulUnsigned(sq1, hi, out T p5_hi);
            T p6_lo = BigMulUnsigned(sq2, hi, out T p6_hi);
            T p7_lo = BigMulUnsigned(sq3, hi, out T p7_hi);

            T w0 = p0_lo;
            w1 = Sum3(p0_hi, p1_lo, p4_lo, out T carry1);
            w2 = Sum5(p1_hi, p2_lo, p4_hi, p5_lo, carry1, out T carry2);
            w3 = Sum5(p2_hi, p3_lo, p5_hi, p6_lo, carry2, out T carry3);
            w4 = Sum4(p3_hi, p6_hi, p7_lo, carry3, out T carry4);
            w5 = p7_hi + carry4;

            return w0;
        }

        // truncating upper T words of the result
        [Experimental("UoWIP_GenericMath")]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T CubeUnchecked4<T>(T lo, T hi,
            out T result_lo_hi, out T result_hi_lo, out T result_hi_hi)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            Square2(out T sq0, out T sq1, out T sq2, out T sq3, lo, hi);

            T p0_lo = BigMulUnsigned(sq0, lo, out T p0_hi);
            T p1_lo = BigMulUnsigned(sq1, lo, out T p1_hi);
            T p2_lo = BigMulUnsigned(sq2, lo, out T p2_hi);
            T p3_lo = sq3 * lo;      // only low half needed for truncated result
            T p4_lo = BigMulUnsigned(sq0, hi, out T p4_hi);
            T p5_lo = BigMulUnsigned(sq1, hi, out T p5_hi);
            T p6_lo = sq2 * hi;      // only low half needed for truncated result

            T w0 = p0_lo;
            result_lo_hi = Sum3(p0_hi, p1_lo, p4_lo, out T carry1);
            result_hi_lo = Sum5(p1_hi, p2_lo, p4_hi, p5_lo, carry1, out T carry2);
            result_hi_hi = Sum5(p2_hi, p3_lo, p5_hi, p6_lo, carry2, out T _); // carry → discarded (w4)
            return w0;
        }

        // truncating upper T words of the result
        [Experimental("UoWIP_GenericMath")]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static T CubeUnchecked2<T>(T lo, T hi, out T result_hi)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            T ll_lo = BigMulUnsigned(lo, lo, out T ll_hi);
            T lh_lo = lo * hi;       // only .lo needed for d0

            T sq0 = ll_lo;
            T sq1 = ll_hi + (lh_lo << 1);   // carry → result_hi_lo discarded

            T p0_lo = BigMulUnsigned(sq0, lo, out T p0_hi);
            T p1_lo = sq1 * lo;
            T p4_lo = sq0 * hi;

            result_hi = p0_hi + p1_lo + p4_lo;  // carry → result_hi_lo discarded
            return p0_lo;
        }
    }
}
