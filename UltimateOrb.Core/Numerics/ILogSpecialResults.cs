using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Special return values used by ILog methods to indicate exceptional results.
    /// </summary>
    public static partial class ILogSpecialResults {

        /// <summary>
        /// Returned when an ILog method receives a negative input.
        /// </summary>
        /// <remarks>
        /// This constant equals <c>int.MinValue</c> and is used to signal argument-out-of-range for logarithm operations.
        /// </remarks>
        public const int ILogNaN = int.MinValue;

        /// <summary>
        /// Returned when an ILog method receives an input of zero.
        /// </summary>
        /// <remarks>
        /// This constant equals <c>int.MinValue + 1</c> and is used to signal the logarithm of zero.
        /// </remarks>
        public const int ILog0 = int.MinValue + 1;

        /// <summary>
        /// Returned when an ILog method receives an input of infinity.
        /// </summary>
        public const int ILogInfinity = ILog0;
    }
}
