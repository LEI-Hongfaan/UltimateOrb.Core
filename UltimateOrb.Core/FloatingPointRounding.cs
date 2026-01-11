using System;
using System.Diagnostics.CodeAnalysis;

namespace UltimateOrb {

    /// <summary>
    /// Specifies the rounding mode used for floating-point operations.
    /// </summary>
    public enum FloatingPointRounding {

        /// <summary>
        /// The IEEE Std 754 <c>roundTiesToEven</c> mode: round to the nearest value;
        /// if the value is exactly halfway between two representable values, choose the one with an even significand.
        /// </summary>
        ToNearestWithMidpointToEven,

        /// <summary>
        /// The IEEE Std 754 <c>roundTiesToAway</c> mode: round to the nearest value;
        /// if the value is exactly halfway between two representable values, choose the one farther from zero.
        /// </summary>
        ToNearestWithMidpointAwayFromZero,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardPositive</c> mode: round toward positive infinity.
        /// </summary>
        Upward,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardNegative</c> mode: round toward negative infinity.
        /// </summary>
        Downward = 1 + Upward,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardZero</c> mode: round toward zero (truncate fractional bits).
        /// </summary>
        TowardZero,

        /// <summary>
        /// Sticky rounding, also known as rounding to odd.
        /// </summary>
        /// <remarks>
        /// In binary arithmetic, this mode rounds the intermediate result toward zero and, when the result is inexact,
        /// sets the least significant bit of the significand to 1. If the intermediate result is exactly representable,
        /// it is returned unchanged; otherwise the nearest representable value with an odd significand is returned.
        /// </remarks>
        ToOdd,

        /// <summary>
        /// Similar to <see cref="ToNearestWithMidpointToEven"/> but rounds half values to the nearest representable value
        /// whose significand is odd. Support in UltimateOrb APIs is partial and may be limited.
        /// </summary>
        [Experimental("UoWIP")]
        ToNearestWithMidpointToOdd,

        /// <summary>
        /// Round to the nearest value; if the value is exactly halfway between two representable values,
        /// choose the nearest value that is rounded upward (toward positive infinity). Support in UltimateOrb APIs is partial and may be limited.
        /// </summary>
        [Experimental("UoWIP")]
        ToNearestWithMidpointUpward,

        /// <summary>
        /// Round to the nearest value; if the value is exactly halfway between two representable values,
        /// choose the nearest value that is rounded downward (toward negative infinity). Support in UltimateOrb APIs is partial and may be limited.
        /// </summary>
        [Experimental("UoWIP")]
        ToNearestWithMidpointDownward,

        /// <summary>
        /// Similar to <see cref="ToNearestWithMidpointAwayFromZero"/> but rounds half values toward zero.
        /// Support in UltimateOrb APIs is partial and may be limited.
        /// </summary>
        [Experimental("UoWIP")]
        ToNearestWithMidpointTowardZero,

        /// <summary>
        /// Round away from zero.
        /// Support in UltimateOrb APIs is partial and may be limited.
        /// </summary>
        [Experimental("UoWIP")]
        TowardInfinity,
    }

    [Experimental("UoWIP")]
    public static partial class RoundingExtensions {

        extension(System.MidpointRounding @this) {

            public FloatingPointRounding ToFloatingPointRounding() {
                return @this switch {
                    System.MidpointRounding.ToEven => FloatingPointRounding.ToNearestWithMidpointToEven,
                    System.MidpointRounding.AwayFromZero => FloatingPointRounding.ToNearestWithMidpointAwayFromZero,
                    System.MidpointRounding.ToZero => FloatingPointRounding.ToNearestWithMidpointTowardZero,
                    System.MidpointRounding.ToNegativeInfinity => FloatingPointRounding.ToNearestWithMidpointDownward,
                    System.MidpointRounding.ToPositiveInfinity => FloatingPointRounding.ToNearestWithMidpointUpward,
                    _ => throw new NotSupportedException($"The specified {nameof(System.MidpointRounding)} value '{@this}' is not supported."),
                };
            }
        }
    }
}
