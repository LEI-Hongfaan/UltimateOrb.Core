using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Defines methods for performing division and remainder operations on types that support double-width arithmetic.
    /// </summary>
    /// <typeparam name="TSelf">The type that implements this interface.</typeparam>
    public interface IDoubleArithmeticBigDivRemFunctions<TSelf> where TSelf : IDoubleArithmeticBigDivRemFunctions<TSelf> {

        /// <summary>
        /// Computes the quotient and remainder of the division of a double-width dividend by a divisor.
        /// </summary>
        /// <param name="dividend_lo">The lower part of the dividend.</param>
        /// <param name="dividend_hi">The higher part of the dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>A tuple containing the quotient and remainder.</returns>
        public static abstract (TSelf Quotient, TSelf Remainder) BigDivRem(TSelf dividend_lo, TSelf dividend_hi, TSelf divisor);

        /// <summary>
        /// Computes the quotient of the division of a double-width dividend by a divisor.
        /// </summary>
        /// <param name="dividend_lo">The lower part of the dividend.</param>
        /// <param name="dividend_hi">The higher part of the dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The quotient.</returns>
        public static virtual TSelf BigDivide(TSelf dividend_lo, TSelf dividend_hi, TSelf divisor) => TSelf.BigDivRem(dividend_lo, dividend_hi, divisor).Quotient;

        /// <summary>
        /// Computes the remainder of the division of a double-width dividend by a divisor.
        /// </summary>
        /// <param name="dividend_lo">The lower part of the dividend.</param>
        /// <param name="dividend_hi">The higher part of the dividend.</param>
        /// <param name="divisor">The divisor.</param>
        /// <returns>The remainder.</returns>
        public static virtual TSelf BigRemainder(TSelf dividend_lo, TSelf dividend_hi, TSelf divisor) => TSelf.BigDivRem(dividend_lo, dividend_hi, divisor).Remainder;
    }
}
