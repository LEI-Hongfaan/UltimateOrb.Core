using System;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Defines methods for performing double-width multiplication operations.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDoubleArithmeticBigMulFunctions<TSelf> where TSelf : IDoubleArithmeticBigMulFunctions<TSelf> {

        /// <summary>
        /// Computes the product of two values, returning the lower and higher parts of the result.
        /// </summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>A tuple containing the lower and higher parts of the product.</returns>
        public static abstract (TSelf Low, TSelf High) BigMultiply(TSelf left, TSelf right);

        /// <summary>
        /// Computes the lower part of the product of two values.
        /// </summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The lower part of the product.</returns>
        public static virtual TSelf BigMultiplyLow(TSelf left, TSelf right) => TSelf.BigMultiply(left, right).Low;

        /// <summary>
        /// Computes the higher part of the product of two values.
        /// </summary>
        /// <param name="left">The first value to multiply.</param>
        /// <param name="right">The second value to multiply.</param>
        /// <returns>The higher part of the product.</returns>
        public static virtual TSelf BigMultiplyHigh(TSelf left, TSelf right) => TSelf.BigMultiply(left, right).High;
    }
}
