using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    public static partial class BigIntegerMath {
    }

    partial class BigIntegerMath {

        /// <summary>
        /// Integer base-10 logarithm: floor(log10(value)) for value > 0.
        /// Returns FP_ILOGNAN for value &lt; 0 and FP_ILOG0 for value == 0.
        /// Uses (int)BigInteger.ILog10(value) as an initial guess and refines it with exact comparisons.
        /// </summary>
        /// <remarks>
        /// This routine should handle huge BigInteger which beyond current CoreCLR design.
        /// </remarks>
        public static int ILog10(this BigInteger value) {
            var s = value.Sign;
            if (s == 0) return ILogSpecialResults.ILog0;
            if (s < 0) return ILogSpecialResults.ILogNaN;

            // initial guess from BigInteger.ILog10, but guard against huge double values
            double d = BigInteger.Log10(value);
            d += 0.25;
            Debug.Assert(d >= 0);
            Debug.Assert(d < 9007199254740991);
            // ILog10(2^(8 * 2^48)) == 677859288149823
            // Current CoreCLR BigInteger max == 2^2147483584 - 1, ILog10 == 646456973
            long g = (long)d;
            Debug.Assert(g >= 0);
            if (g > (long)1 + int.MaxValue) {
                return ILogSpecialResults.ILogNaN;
            }
            bool t = false;
            if (g == (long)1 + int.MaxValue) {
                g = int.MaxValue;
                t = true;
            }
            // Compute 10^g using helper (optimized for small exponents)
            BigInteger Exp10 = BigIntegerSmallExp10Module.Exp10(unchecked((int)g));
            if (t) {
                Exp10 *= 10;
            }
            // If Exp10 > value, step down
            if (Exp10 > value) {
                g--;
            }
            Debug.Assert(g >= 0);
            return g > int.MaxValue ? ILogSpecialResults.ILogNaN : unchecked((int)g);
        }
    }
}
