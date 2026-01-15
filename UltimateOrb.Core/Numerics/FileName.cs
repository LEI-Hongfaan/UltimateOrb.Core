using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Mathematics.NumberTheory;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb.Numerics {

    internal static partial class NumberBaseExtensions {

        public static (T1, T2) Pow<T1, T2>((T1, T2) @base, uint exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>? {
            return Pow<T1, T2, uint>(@base, exponent);
        }

        public static (T1, T2) Pow<T1, T2>((T1, T2) @base, ulong exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>? {
            return Pow<T1, T2, ulong>(@base, exponent);
        }

        public static (T1, T2) Pow<T1, T2>((T1, T2) @base, nuint exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>? {
            return Pow<T1, T2, nuint>(@base, exponent);
        }

        public static (T1, T2) Pow<T1, T2>((T1, T2) @base, UltimateOrb.UInt128 exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>? {
            return Pow<T1, T2, UltimateOrb.UInt128>(@base, exponent);
        }

        public static (T1, T2) Pow<T1, T2>((T1, T2) @base, System.UInt128 exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>? {
            return Pow<T1, T2, System.UInt128>(@base, exponent);
        }

        public static (T1, T2) Pow<T1, T2, TExponent>((T1, T2) @base, TExponent exponent)
            where T1 : INumberBase<T1>?
            where T2 : INumberBase<T2>?
            where TExponent : IBinaryInteger<TExponent>, IUnsignedNumber<TExponent>? {
            return (Pow<T1, TExponent>(@base.Item1, exponent), Pow<T2, TExponent>(@base.Item2, exponent));
        }

        public static T Pow<T>(T @base, uint exponent)
            where T : INumberBase<T>? {
            return Pow<T, uint>(@base, exponent);
        }

        public static T Pow<T>(T @base, ulong exponent)
            where T : INumberBase<T>? {
            return Pow<T, ulong>(@base, exponent);
        }

        public static T Pow<T>(T @base, nuint exponent)
            where T : INumber<T>? {
            return Pow<T, nuint>(@base, exponent);
        }
        public static T Pow<T>(T @base, UltimateOrb.UInt128 exponent)
            where T : INumberBase<T>? {
            return Pow<T, UltimateOrb.UInt128>(@base, exponent);
        }

        public static T Pow<T>(T @base, System.UInt128 exponent)
            where T : INumberBase<T>? {
            return Pow<T, System.UInt128>(@base, exponent);
        }

        public static T Pow<T, TExponent>(T @base, TExponent exponent)
            where T : INumberBase<T>?
            where TExponent : IBinaryInteger<TExponent>, IUnsignedNumber<TExponent>? {
            if (typeof(T) == typeof(BigInteger)) {
                var c = BinaryIntegerTypeTraits<TExponent>.BitSizeOrNegativeOne;
                if (c >= 0 && c <= 32) {
                    int e = int.CreateTruncating(exponent);
                    bool fix = false;
                    if (c == 32) {
                        if (int.IsNegative(e)) {
                            fix = true;
                            e >>>= 1;
                        }
                    }
                    var bi = (BigInteger)(object)@base!;
                    var p = BigInteger.Pow(bi, e);
                    if (c == 32 && fix) {
                        p *= p;
                        if (TExponent.IsOddInteger(exponent)) {
                            p *= bi;
                        }
                    }
                    return (T)(object)p;
                }
            }
            if (null == @base || null == exponent) {
                return default!;
            }
            T j = T.One;
            for (; ; ) {
                if (TExponent.IsOddInteger(exponent)) {
                    j *= @base;
                }
                if (TExponent.IsZero(exponent >>= 1)) {
                    break;
                }
                @base *= @base;
            }
            return j;
        }
    }
}
