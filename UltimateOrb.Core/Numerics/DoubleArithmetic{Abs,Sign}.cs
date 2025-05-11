using System;
using System.Runtime.CompilerServices;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

namespace UltimateOrb.Numerics {

    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

        /// <summary>
        ///     <para>
        ///         Computes the absolute value of the specified value of a signed operand with double-precision data.
        ///         The result is an unsigned value with double-precision data.
        ///     </para>
        /// </summary>
        /// <param name="value_lo">
        ///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="value_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="result_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
        /// </param>
        /// <returns>
        ///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong AbsSignedAsUnsigned(ULong value_lo, Long value_hi, out ULong result_hi) {
            unchecked {
                // Create a mask: if value_hi is negative, mask will be all 1's; otherwise, it is 0.
                var mask = (ULong)(((Long)value_hi) >> (SizeOfModule.BitSizeOf<ULong>() - 1));
                // XOR with mask flips bits if negative.
                var t_lo = value_lo ^ mask;
                var t_hi = ((ULong)value_hi) ^ mask;
                // Add (mask & 1) to complete the two's complement if negative.
                var absLo = t_lo + (mask & 1U);
                // Check for a carry (overflow from the low part).
                var absHi = t_hi + (absLo < t_lo ? 1U : 0U);
                result_hi = absHi;
                return absLo;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Sign(ULong value_lo, Long value_hi) {
            return 0 <= value_hi ? (0 != (value_hi | (Long)value_lo)).AsIntegerUnsafe() : -1;
        }
    }
}

#if NET7_0_OR_GREATER
namespace UltimateOrb.Numerics {

    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.UInt128;

    using Math = global::Internal.System.Math;
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

        /// <summary>
        ///     <para>
        ///         Computes the absolute value of the specified value of a signed operand with double-precision data.
        ///         The result is an unsigned value with double-precision data.
        ///     </para>
        /// </summary>
        /// <param name="value_lo">
        ///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="value_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="result_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
        /// </param>
        /// <returns>
        ///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong AbsSignedAsUnsigned(ULong value_lo, Long value_hi, out ULong result_hi) {
            unchecked {
                // Create a mask: if value_hi is negative, mask will be all 1's; otherwise, it is 0.
                var mask = (ULong)(((Long)value_hi) >> (SizeOfModule.BitSizeOf<ULong>() - 1));
                // XOR with mask flips bits if negative.
                var t_lo = value_lo ^ mask;
                var t_hi = ((ULong)value_hi) ^ mask;
                // Add (mask & 1) to complete the two's complement if negative.
                var absLo = t_lo + (mask & 1U);
                // Check for a carry (overflow from the low part).
                var absHi = t_hi + (absLo < t_lo ? 1U : 0U);
                result_hi = absHi;
                return absLo;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Sign(ULong value_lo, Long value_hi) {
            /*
            var t = (0 <= value_hi).AsIntegerUnsafe();
            var s = unchecked(t + t - 1);
            return (0 == (unchecked((Long)value_lo) | value_hi)) ? 0 : s;
            */
            return 0 <= value_hi ? (0 != (value_hi | (Long)value_lo)).AsIntegerUnsafe() : -1;
        }
    }
}
#endif

namespace UltimateOrb.Numerics {

    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.UInt128;

    using Math = global::Internal.System.Math;
    using MathEx = DoubleArithmetic;

    public static partial class DoubleArithmetic {

        /// <summary>
        ///     <para>
        ///         Computes the absolute value of the specified value of a signed operand with double-precision data.
        ///         The result is an unsigned value with double-precision data.
        ///     </para>
        /// </summary>
        /// <param name="value_lo">
        ///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="value_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="result_hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
        /// </param>
        /// <returns>
        ///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong AbsSignedAsUnsigned(ULong value_lo, Long value_hi, out ULong result_hi) {
            unchecked {
                // Create a mask: if value_hi is negative, mask will be all 1's; otherwise, it is 0.
                var mask = (ULong)(((Long)value_hi) >> (SizeOfModule.BitSizeOf<ULong>() - 1));
                // XOR with mask flips bits if negative.
                var t_lo = value_lo ^ mask;
                var t_hi = ((ULong)value_hi) ^ mask;
                // Add (mask & 1) to complete the two's complement if negative.
                var absLo = t_lo + (mask & 1U);
                // Check for a carry (overflow from the low part).
                var absHi = t_hi + (absLo < t_lo ? 1U : 0U);
                result_hi = absHi;
                return absLo;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Sign(ULong value_lo, Long value_hi) {
            return 0 <= value_hi ? (0 != (value_hi | (Long)value_lo)).AsIntegerUnsafe() : -1;
        }
    }
}
