using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {


    public static partial class GenericMath {

        struct ISqrtTT<T>
            where T :
                INumberBase<T> {

            private static readonly (T Value, bool NeedComparison) r = GetC();
            internal static readonly T Value = r.Value;
            internal static readonly bool NeedComparison = r.NeedComparison;

            internal static readonly T MinusOne = unchecked(-T.One);

            private static (T, bool) GetC() {
                T value;
                try {
                    value = T.CreateChecked(4503599761588223);
                    return (value, true);
                } catch (OverflowException) {
                    return (default!, false);
                }
            }
        }

        public static T ISqrt<T>(T value)
            where T :
                IBinaryInteger<T> {
            unchecked {
                if (T.IsNegative(value)) {
                    throw new DivideByZeroException("Radicand must be non-negative");
                }
                if (ISqrtTT<T>.NeedComparison && value <= ISqrtTT<T>.Value) {
                    return T.CreateTruncating(unchecked((UInt64)Math.Sqrt(double.CreateTruncating(value))));
                }
                var root = T.CreateTruncating(Math.Sqrt(double.CreateSaturating(value)));
                for (; ; ) {
                    var d = (root - (value / root)) >> 1;
                    var next = root + d;
                    if (T.IsZero(d)) {
                        return root;
                    } else if (d == T.One) {
                        return next * next > value ? root : next;
                    } else if (d == ISqrtTT<T>.MinusOne) {
                        return root * root > value ? next : next;
                    }
                    root = next;
                }
            }
        }
    }
}
