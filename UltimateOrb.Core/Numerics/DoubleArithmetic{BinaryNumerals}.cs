using System;

namespace UltimateOrb.Numerics {

    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = System.Math;
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

        /// <summary>
        ///     <para>
        ///         Computes the bit pattern of the next permutation in the lexicographical order.
        ///         If the current permutation is already the maximum, the result will be the pattern with all bits <c>1</c>.
        ///     </para>
        /// </summary>
        /// <param name="lo">
        ///     <para>The <c>lo</c> part of the current bit pattern.</para>
        /// </param>
        /// <param name="hi">
        ///     <para>The <c>hi</c> part of the current bit pattern.</para>
        /// </param>
        /// <param name="result_hi">
        ///     <para>The <c>hi</c> part of the next bit pattern.</para>
        /// </param>
        /// <returns>
        ///     <para>The <c>lo</c> part of the next bit pattern.</para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong NextPermutation(ULong lo, ULong hi, out ULong result_hi) {
            unchecked {
                ULong loT;
                ULong hiT;
                ULong loV;
                ULong hiV;
                if (0u == lo) {
                    if (0u == hi) {
                        result_hi = ULong.MaxValue;
                        return ULong.MaxValue;
                    }
                    hiT = (hi | (hi - 1u)) + 1u;
                    hiV = hi & (ULong)(-(Long)hi);
                    loV = ((hiT & (ULong)(-(Long)hiT)) / hiV) >> 1;
                    hiV = 0u;
                    if (0u == loV) {
                        result_hi = ULong.MaxValue;
                        return ULong.MaxValue;
                    } else {
                        result_hi = hiT;
                        return loV - 1u;
                    }
                } else {
                    loT = (lo | (lo - 1u));
                    hiT = hi;
                    if (ULong.MaxValue == loT++) {
                        ++hiT;
                    }
                    loV = lo & (ULong)(-(Long)lo);
                    hiV = hi & ((ULong)(-1u) - hi);
                    loV = MathEx.Divide(loT & (ULong)(-(Long)loT), hiT & ((0u == loT) ? (ULong)(-(Long)hiT) : ((ULong)(-1u) - hiT)), loV, hiV, out hiV);
                    loV = (loV >> 1) | (hiV << (Misc.ULong.BitSize - 1));
                    hiV = hiV >> 1;
                    if (0u == loV--) {
                        --hiV;
                    }
                    result_hi = hiT | hiV;
                    return loT | loV;
                }
            }
        }
    }
}
