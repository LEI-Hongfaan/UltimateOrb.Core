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
                m2 = SpecializedArithmeticHelpers.PowChecked(m2, 20);
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

                // Find the upper bound by doubling
                long high = 1;
                while (!T.IsZero(T.One << checked((int)high))) {
                    high *= 2;
                }
                // Binary search to find the exact bit size
                var low = high / 2;
                while (low < high) {
                    var mid = (low + high) / 2;
                    if (T.IsZero(T.One << checked((int)mid))) {
                        high = mid;
                    } else {
                        low = mid + 1;
                    }
                }
                return low;
            }
        L_Error:;
            return -1;
        }
    }
}
