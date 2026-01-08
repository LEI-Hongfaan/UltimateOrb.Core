using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;

namespace UltimateOrb {

    internal static partial class BinaryIntegerTypeTraits<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T> where T : IBinaryInteger<T>? {

        public static readonly bool IsUnsigned = ((Func<bool>)(static () => {
            if (!(T.Zero != T.AllBitsSet)) return false;
            var m1 = unchecked(T.Zero - T.One);
            if (T.Zero <= m1) return true;
            return false;
        }))();

        public static readonly bool IsSigned = ((Func<bool>)(static () => {
            if (!(T.Zero != T.AllBitsSet)) return false;
            var m1 = unchecked(T.Zero - T.One);
            if (T.Zero > m1) return true;
            return false;
        }))();

        public static readonly bool IsBounded = ((Func<bool>)(static () => {
            if (typeof(T).GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMinMaxValue<>) && x.GenericTypeArguments[0] == typeof(T))) {
                return true;
            }

            try {
                if (T.AdditiveIdentity == T.MultiplicativeIdentity) { return true; }
                {
                    var a = T.One;
                    for (int i = 0; i <= 128; ++i) {
                        a <<= 1;
                        if (a == T.Zero) return true;
                    }
                }
                {
                    var m1 = T.AllBitsSet;
                    if (m1 == T.Zero) { return true; }
                    var m1s1 = m1 >>> 1;
                    var t1 = m1 ^ m1s1;
                    var t2 = t1 << 1;
                    if (m1 != T.Zero && t1 != T.Zero && t2 == T.Zero) {
                        return true;
                    }
                }
            } catch (NotSupportedException) {
            } catch (NotImplementedException) {
            } catch (InvalidOperationException) {
            } catch (InvalidCastException) {
            } catch (ArithmeticException) {
            }
            return false;
        }))();

        public static readonly T? MaxNonnegativePowerOf2Default = ((Func<T>)(static () => {
            if (!IsBounded) {
                return default!;
            }
            try {
                var b = T.One;
                if (b > T.Zero) {
                    for (; ; ) {
                        var t = b << 1;
                        if (t > b) {
                            b = t;
                            continue;
                        }
                        return b;
                    }
                }
            } catch (NotSupportedException) {
            } catch (NotImplementedException) {
            } catch (InvalidOperationException) {
            } catch (InvalidCastException) {
            } catch (ArithmeticException) {
            }
            return default!;
        }))();

        public static readonly T? MinNonpositivePowerOf2Default = ((Func<T>)(static () => {
            if (!IsBounded) {
                return default!;
            }
            try {
                var b = ~T.Zero;
                if (b < T.Zero) {
                    for (; ; ) {
                        var t = b << 1;
                        if (t < b) {
                            b = t;
                            continue;
                        }
                        return b;
                    }
                }
            } catch (NotSupportedException) {
            } catch (NotImplementedException) {
            } catch (InvalidOperationException) {
            } catch (InvalidCastException) {
            } catch (ArithmeticException) {
            }
            return default!;
        }))();

        public static readonly T? HighestBitSetOrDefault = ((Func<T>)(static () => {
            if (!IsBounded) {
                return default!;
            }
            if (IsSigned) {
                return MinNonpositivePowerOf2Default;
            }
            if (IsUnsigned) {
                return MaxNonnegativePowerOf2Default;
            }
            return default!;
        }))();



        public static readonly int BitSizeOrNegativeOne = ((Func<int>)(static () => {
            if (!IsBounded) {
                return -1;
            }
            if (T.AdditiveIdentity == T.MultiplicativeIdentity) { return 0; }
            if (null != HighestBitSetOrDefault && !T.IsZero(HighestBitSetOrDefault)) {
                try {
                    return 1 + int.CreateChecked(T.TrailingZeroCount(HighestBitSetOrDefault));
                } catch (NotSupportedException) {
                } catch (NotImplementedException) {
                } catch (InvalidOperationException) {
                } catch (InvalidCastException) {
                } catch (ArithmeticException) {
                }
            }
            return -1;
        }))();

        public static readonly int ByteSizeOrNegativeOne = ((Func<int>)(static () => {
            if (!IsBounded) {
                return -1;
            }
            if (T.AdditiveIdentity == T.MultiplicativeIdentity) { return 0; }
            if (null != HighestBitSetOrDefault && !T.IsZero(HighestBitSetOrDefault)) {
                try {
                    return (int)((1 + long.CreateChecked(T.TrailingZeroCount(HighestBitSetOrDefault))) / 8);
                } catch (NotSupportedException) {
                } catch (NotImplementedException) {
                } catch (InvalidOperationException) {
                } catch (InvalidCastException) {
                } catch (ArithmeticException) {
                }
            }
            return -1;
        }))();

        internal static readonly bool IsInitialized = true;
    }
}