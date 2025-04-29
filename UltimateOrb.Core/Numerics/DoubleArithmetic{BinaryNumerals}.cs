using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
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
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

#if STANDALONE_XINTN_LIBRARY
#else
#if NET7_0_OR_GREATER
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
        public static T NextPermutation<T>(T lo, T hi, out T result_hi) where T: unmanaged, IUnsignedNumber<T>, IBinaryInteger<T> {
            unchecked {
                T loT;
                T hiT;
                T loV;
                T hiV;
                if (T.Zero == lo) {
                    if (T.Zero == hi) {
                        result_hi = T.AllBitsSet;
                        return T.AllBitsSet;
                    }
                    var tHi = hi;
                    --tHi;
                    hiT = (hi | tHi);
                    ++hiT;
                    hiV = hi & (T.Zero - hi);
                    loV = ((hiT & (T.Zero - hiT)) / hiV) >> 1;
                    hiV = T.Zero;
                    if (T.IsZero(loV)) {
                        result_hi = T.AllBitsSet;
                        return T.AllBitsSet;
                    } else {
                        result_hi = hiT;
                        return --loV;
                    }
                } else {
                    var tLo = lo;
                    --tLo;
                    loT = (lo | tLo);
                    hiT = hi;
                    ++loT;
                    if (T.IsZero(loT)) {
                        ++hiT;
                    }
                    loV = lo & (T.Zero - lo);
                    hiV = hi & (T.AllBitsSet - hi);
                    loV = MathEx.DivideUnsigned(loT & (T.Zero - loT), hiT & (T.IsZero(loT) ? (T.Zero - hiT) : (T.AllBitsSet - hiT)), loV, hiV, out hiV);
                    unsafe {
                        loV = (loV >> 1) | (hiV << (sizeof(T) * 8 - 1));
                    }
                    hiV = hiV >> 1;
                    if (T.IsZero(loV--)) {
                        --hiV;
                    }
                    result_hi = hiT | hiV;
                    return loT | loV;
                }
            }
        }
#endif
#endif
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

        public static int CountLeadingZeros(UInt128 lo, UInt128 hi) {
            if (0 != hi) {
                return BinaryNumerals.CountLeadingZeros(hi);
            }
            return unchecked(128 + BinaryNumerals.CountLeadingZeros(lo));
        }

#if NET7_0_OR_GREATER
        public static int CountLeadingZeros(System.UInt128 lo, System.UInt128 hi) {
            if (0 != hi) {
                return BinaryNumerals.CountLeadingZeros(hi);
            }
            return unchecked(128 + BinaryNumerals.CountLeadingZeros(lo));
        }
#endif


        public static int CountTrailingZeros(UInt64 lo, UInt64 hi) {
            if (0 != lo) {
                return BinaryNumerals.CountTrailingZeros(lo);
            }
            return unchecked(64 + BinaryNumerals.CountTrailingZeros(hi));
        }

        public static int CountTrailingZeros(UInt128 lo, UInt128 hi) {
            if (0 != lo) {
                return BinaryNumerals.CountTrailingZeros(lo);
            }
            return unchecked(128 + BinaryNumerals.CountTrailingZeros(hi));
        }

#if NET7_0_OR_GREATER
        public static int CountTrailingZeros(System.UInt128 lo, System.UInt128 hi) {
            if (0 != lo) {
                return BinaryNumerals.CountTrailingZeros(lo);
            }
            return unchecked(128 + BinaryNumerals.CountTrailingZeros(hi));
        }
#endif

        public static int Log2Floor(UInt64 lo, UInt64 hi) {
            if (0 == hi) {
                return BinaryNumerals.Log2Floor(lo);
            }
            return 64 + BinaryNumerals.Log2Floor(hi);
        }

        public static int Log2Floor(UltimateOrb.UInt128 lo, UltimateOrb.UInt128 hi) {
            if (0 == hi) {
                return BinaryNumerals.Log2Floor(lo);
            }
            return 128 + BinaryNumerals.Log2Floor(hi);
        }

#if NET7_0_OR_GREATER
        public static int Log2Floor(System.UInt128 lo, System.UInt128 hi) {
            if (0 == hi) {
                return BinaryNumerals.Log2Floor(lo);
            }
            return 128 + BinaryNumerals.Log2Floor(hi);
        }
#endif

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
            // return lo != hi && 1 == BinaryNumerals.PopulationCount(lo | hi);
            return 1 == BinaryNumerals.PopulationCount(lo) + BinaryNumerals.PopulationCount(hi);
        }

        public static bool IsPowerOfTwo(UInt128 lo, UInt128 hi) {
            // (number & (number - 1)) == 0;
            return lo != hi && 1 == BinaryNumerals.PopulationCount(lo | hi);
        }

        public static bool IsPowerOfTwo(UInt64 lo, Int64 hi) {
            if (0 > hi) {
                return false;
            }
            return IsPowerOfTwo(lo, hi.ToUnsignedUnchecked());
        }
    }
}
