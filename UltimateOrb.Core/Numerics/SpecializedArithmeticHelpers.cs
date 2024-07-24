using System.Numerics;

namespace UltimateOrb.Numerics {
    static partial class SpecializedArithmeticHelpers {

        public static T PowChecked<T>(T @base, ulong exponent)
            where T :
                IMultiplicativeIdentity<T, T>,
                IMultiplyOperators<T, T, T> {
            var result = T.MultiplicativeIdentity;
            var currentBase = @base;
            var currentExponent = exponent;

            for (; currentExponent > 0;) {
                // If the exponent is odd 
                if ((currentExponent & 1) != 0) {
                    checked {
                        result *= currentBase;
                    }
                }
                checked {
                    currentBase *= currentBase; // Square the base
                }
                currentExponent >>= 1; // Divide the exponent by 2
            }

            return result;
        }
    }
}
