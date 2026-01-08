using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                var n = BigInteger.CreateChecked(value);
                int b = checked((int)n.GetBitLength());
                const int doubleSignificand = 53;
                Debug.Assert(b >= doubleSignificand);

                // make shift even so sqrt scaling is integer power of two
                int shift = (b - doubleSignificand) & ~1;
                BigInteger m = n >> shift;               // now m fits in ~53 bits
                double md = (double)m;                   // safe conversion (no Inf)
                double sd = Math.Sqrt(md);
                BigInteger x = new BigInteger(sd);

                var root = T.CreateTruncating(x) << (shift / 2);
                for (; ; ) {
                    var d = ((value / root) - root) >> 1;
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

        public static T SqrtRem<T>(T value, out T remainder)
            where T :
                IBinaryInteger<T> {
            unchecked {
                if (T.IsNegative(value)) {
                    throw new DivideByZeroException("Radicand must be non-negative");
                }
                if (ISqrtTT<T>.NeedComparison && value <= ISqrtTT<T>.Value) {
                    var r = T.CreateTruncating(unchecked((UInt64)Math.Sqrt(double.CreateTruncating(value))));
                    remainder = value - r * r;
                    return r;
                }

                var n = BigInteger.CreateChecked(value);
                int b = checked((int)n.GetBitLength());
                const int doubleSignificand = 53;
                Debug.Assert(b >= doubleSignificand);

                // make shift even so sqrt scaling is integer power of two
                int shift = (b - doubleSignificand) & ~1;
                BigInteger m = n >> shift;               // now m fits in ~53 bits
                double md = (double)m;                   // safe conversion (no Inf)
                double sd = Math.Sqrt(md);
                BigInteger x = new BigInteger(sd);

                var root = T.CreateTruncating(x) << (shift / 2);
                for (; ; ) {
                    var d = ((value / root) - root) >> 1;
                    var next = root + d;
                    if (T.IsZero(d)) {
                        remainder = value - root * root;
                        return root;
                    } else if (d == T.One) {
                        return Adjust(value, next, root, out remainder);
                    } else if (d == ISqrtTT<T>.MinusOne) {
                        return Adjust(value, root, next, out remainder);
                    }
                    root = next;
                }
            }

            static T Adjust(T value, T root, T next, out T remainder) {
                unchecked {
                    var q = root * root;
                    if (q > value) {
                        q -= root + next;
                        remainder = value - q;
                        return next;
                    } else {
                        remainder = value - q;
                        return root;
                    }
                }
            }
        }
    }

    public static partial class GenericMath {
    }
}
