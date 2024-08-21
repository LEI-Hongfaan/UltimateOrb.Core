using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    static partial class IntegerGenericMathImpl {

        public static T MaxMagnitude<T>(T x, T y)
            where T :
                INumberBase<T>,
                IUnaryNegationOperators<T, T>,
                IComparisonOperators<T, T, bool> {
            var absX = x;

            if (T.IsNegative(absX)) {
                absX = -absX;

                if (T.IsNegative(absX)) {
                    return x;
                }
            }

            var absY = y;

            if (T.IsNegative(absY)) {
                absY = -absY;

                if (T.IsNegative(absY)) {
                    return y;
                }
            }

            if (absX > absY) {
                return x;
            }

            if (absX == absY) {
                return T.IsNegative(x) ? y : x;
            }

            return y;
        }

        public static T MinMagnitude<T>(T x, T y)
            where T :
                INumberBase<T>,
                IUnaryNegationOperators<T, T>,
                IComparisonOperators<T, T, bool> {
            var absX = x;

            if (T.IsNegative(absX)) {
                absX = -absX;

                if (T.IsNegative(absX)) {
                    return y;
                }
            }

            var absY = y;

            if (T.IsNegative(absY)) {
                absY = -absY;

                if (T.IsNegative(absY)) {
                    return x;
                }
            }

            if (absX < absY) {
                return x;
            }

            if (absX == absY) {
                return T.IsNegative(x) ? x : y;
            }

            return y;
        }
    }
}
