namespace UltimateOrb {

    public enum FloatingPointRounding {

        /// <summary>
        /// The IEEE Std 754 <c>roundTiesToEven</c>.
        /// </summary>
        ToNearestWithMidpointToEven,

        /// <summary>
        /// The IEEE Std 754 <c>roundTiesToAway</c>.
        /// </summary>
        ToNearestWithMidpointAwayFromZero,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardPositive</c>.
        /// </summary>
        Upward,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardNegative</c>.
        /// </summary>
        Downward = 1 + Upward,

        /// <summary>
        /// The IEEE Std 754 <c>roundTowardZero</c>.
        /// </summary>
        TowardZero,

        /// <summary>
        /// The sticky rounding, a.k.a. rounding to odd.
        /// </summary>
        /// <remarks>
        /// In binary arithmetic, this rounding is to round the result toward zero, and set the least significant bit to 1 if the rounded result is inexact.
        /// Equivalently, it consists in returning the intermediate result when it is exactly representable, and the nearest floating-point number with an odd significand otherwise.
        /// </remarks>
        ToOdd
    }
}
