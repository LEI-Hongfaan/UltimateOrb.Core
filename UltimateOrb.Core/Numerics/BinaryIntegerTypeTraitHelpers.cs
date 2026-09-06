using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {


    public static partial class BinaryIntegerTypeTraitHelpers {

        static partial class HasInfinitePrecisionCache<T>
            where T :
                IAdditiveIdentity<T, T>,
                IAdditionOperators<T, T, T>,
                IMultiplicativeIdentity<T, T>,
                IMultiplyOperators<T, T, T> {

            public static readonly bool Value = HasInfinitePrecisionImpl<T>();
        }

        public static bool HasInfinitePrecision<T>()
            where T :
                IAdditiveIdentity<T, T>,
                IAdditionOperators<T, T, T>,
                IMultiplicativeIdentity<T, T>,
                IMultiplyOperators<T, T, T> {
            return HasInfinitePrecisionCache<T>.Value;
        }

        static bool HasInfinitePrecisionImpl<T>()
            where T :
                IAdditiveIdentity<T, T>,
                IAdditionOperators<T, T, T>,
                IMultiplicativeIdentity<T, T>,
                IMultiplyOperators<T, T, T> {
            var a = T.MultiplicativeIdentity;
            if (T.AdditiveIdentity.Equals(a)) {
                return true;
            }
            try {
                var m2 = checked(a + a);
                m2 = SpecializedArithmeticHelpers.PowChecked(m2, 0X01000000);
                // Good guess
                return true;
            } catch (ArithmeticException) {
                return false;
            } catch (OutOfMemoryException) {
                return true;
            }
        }

        static partial class GetBitSizeCache<T> where T : IBinaryInteger<T> {

            public static readonly long Value = GetBitSizeImpl<T>();
        }

        public static long GetBitSize<T>() where T : IBinaryInteger<T> {
            return GetBitSizeCache<T>.Value;
        }

        static long GetBitSizeImpl<T>() where T : IBinaryInteger<T> {
            if (HasInfinitePrecision<T>()) {
                return -1;
            }
            var a = T.Zero;
            var b = false;
            try {
                a = T.AllBitsSet;
                b = true;
            } catch (ArithmeticException) {
            } catch (InvalidOperationException) {
            } catch (NotSupportedException) {
            } catch (NotImplementedException) {
            } catch (OutOfMemoryException) {
            }
            if (b && T.IsFinite(a)) {
                if (T.IsZero(a)) {
                    return 0;
                }

                // Determine if the type is signed (MinValue is negative).
                bool isSigned = BinaryIntegerTypeTraits<T>.IsSigned;

                // Check whether 2^1 = 2 is representable.
                T two;
                try {
                    two = checked(T.One + T.One);
                } catch (OverflowException) {
                    // Only 2^0 = 1 is representable. The maximum power of two exponent is 0.
                    // For unsigned: 1 bit (values 0,1). For signed: 2 bits (values -1,0,1?).
                    return isSigned ? 2 : 1;
                }

                // We now have 2^1 representable. Find an upper bound for the largest
                // representable power-of-two exponent by repeatedly squaring the power.
                // This generates exponents that are powers of two: 1, 2, 4, 8, ...
                long exponent = 1;          // current exponent = 1
                T power = two;              // current power = 2^1

                while (true) {
                    T nextPower;
                    try {
                        nextPower = checked(power * power);
                    } catch (OverflowException) {
                        // The next squaring overflowed, so 2^(exponent*2) is not representable.
                        break;
                    }
                    long nextExponent = checked(exponent * 2);

                    // If multiplication produced zero (unexpected), stop.
                    if (T.IsZero(nextPower))
                        break;

                    exponent = nextExponent;
                    power = nextPower;
                }

                // Now 'exponent' is the largest power-of-two exponent that we know is representable.
                // The true maximum exponent (e_max) lies in [exponent, exponent*2).
                long low = exponent;      // known representable
                long high = exponent * 2; // not necessarily representable (will be refined)

                // Binary search for the largest e in [low, high) such that 2^e is representable.
                while (low < high) {
                    long mid = low + (high - low + 1) / 2; // upper mid to find maximum
                    if (IsPowerOfTwoRepresentable<T>(mid))
                        low = mid;
                    else
                        high = mid - 1;
                }

                // low is now e_max (the largest exponent for which 2^e fits in T).
                // Total bit size:
                //   - unsigned: bits = e_max + 1
                //   - signed:   bits = e_max + 2 (one extra sign bit)
                return isSigned ? low + 2 : low + 1;
            }
        L_Error:;
            return -1;
        }

        // Helper: returns true if 2^exponent can be represented in T without overflow.
        private static bool IsPowerOfTwoRepresentable<T>(long exponent) where T : IBinaryInteger<T> {
            // Compute 2^exponent using exponentiation by squaring with checked multiplication.
            T result = T.One;
            T baseVal = T.One + T.One; // 2
            long exp = exponent;

            while (exp > 0) {
                if ((exp & 1) == 1) {
                    try {
                        result = checked(result * baseVal);
                    } catch (OverflowException) {
                        return false;
                    }
                }
                exp >>= 1;
                // Only square the base if there are remaining bits to process.
                if (exp > 0) {
                    try {
                        baseVal = checked(baseVal * baseVal);
                    } catch (OverflowException) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
