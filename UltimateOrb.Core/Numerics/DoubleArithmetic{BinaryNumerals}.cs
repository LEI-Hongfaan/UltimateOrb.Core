using System;
using UltimateOrb.Mathematics;
using static UltimateOrb.Utilities.SignConverter;

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

namespace UltimateOrb.Numerics {

    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = System.Math;
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

        public static int CountLeadingZeros(UInt64 lo, UInt64 hi) {
            if (0 != hi) {
                return BinaryNumerals.CountLeadingZeros(hi);
            }
            return unchecked(64 + BinaryNumerals.CountLeadingZeros(lo));
        }

        public static int CountTrailingZeros(UInt64 lo, UInt64 hi) {
            if (0 != hi) {
                return BinaryNumerals.CountTrailingZeros(hi);
            }
            return unchecked(64 + BinaryNumerals.CountTrailingZeros(lo));
        }

        public static int Log2Floor(UInt64 lo, UInt64 hi) {
            if (0 == hi) {
                return BinaryNumerals.Log2Floor(lo);
            }
            return 64 + BinaryNumerals.Log2Floor(hi);
        }

        public static int CountStorageBits(UInt64 lo, UInt64 hi) {
            if (0 == hi) {
                if (0 == lo) {
                    return 0;
                }
                return 1 + BinaryNumerals.Log2Floor(lo);
            }
            return 1 + 64 + BinaryNumerals.Log2Floor(hi);
        }

        public static int CountStorageBits(UInt64 lo, Int64 hi) {
            if (0 > hi) {
                lo = ~lo;
                hi = ~hi;
            }
            if (0 == hi) {
                if (0 == lo) {
                    return 0;
                }
                return 2 + BinaryNumerals.Log2Floor(lo);
            }
            return 2 + 64 + BinaryNumerals.Log2Floor(hi.ToUnsignedUnchecked());
        }

        public static bool IsPowerOfTwo(UInt64 lo, UInt64 hi) {
            return 1 == BinaryNumerals.PopulationCount(lo) + BinaryNumerals.PopulationCount(hi);
        }

        public static bool IsPowerOfTwo(UInt64 lo, Int64 hi) {
            if (0 > hi) {
                return false;
            }
            return IsPowerOfTwo(lo, hi.ToUnsignedUnchecked());
        }
    }
}
