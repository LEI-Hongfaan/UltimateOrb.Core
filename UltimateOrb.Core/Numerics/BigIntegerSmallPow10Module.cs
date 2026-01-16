using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb.Numerics {

    partial class NumberBaseExtensions {

        extension<TSelf>(TSelf)
            where TSelf : INumberBase<TSelf>? {

            public static bool IsOne(TSelf value) {
                return TSelf.One == value;
            }

            internal static bool IsTen(TSelf value) {
                return TSelf.CreateChecked((byte)10) == value;
            }
        }
    }

    internal static partial class BigIntegerSmallPowModule {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Pow(BigInteger value, int exponent) {
            if (exponent < 0) {
                throw new ArgumentOutOfRangeException(nameof(exponent));
            }
            if (exponent == 0) {
                return BigInteger.One;
            }
            var n = BigInteger.IsNegative(value);
            var a = BigInteger.Abs(value);
            if (a.IsZero) {
                return BigInteger.Zero;
            }
            BigInteger p;
            if (a.IsOne) {
                p = BigInteger.One;
            } else if (a == 10) {
                p = BigIntegerSmallExp10Module.Exp10(exponent);
            } else if (BigInteger.IsPow2(a)) {
                var c = checked((int)BigInteger.TrailingZeroCount(a));
                p = BigInteger.One << checked(c * exponent);
            } else {
                return BigInteger.Pow(value, exponent);
            }
            return n && int.IsOddInteger(exponent) ? -p : p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Pow([ConstantExpected(Min = 2)] int value, int exponent) {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(value, 1);
            if (exponent < 0) {
                throw new ArgumentOutOfRangeException(nameof(exponent));
            }
            if (exponent == 0) {
                return BigInteger.One;
            }
            var n = int.IsNegative(value);
            var a = int.Abs(value);
            if (0 == a) {
                return BigInteger.Zero;
            }
            BigInteger p;
            if (1 == a) {
                p = BigInteger.One;
            } else if (a == 10) {
                p = BigIntegerSmallExp10Module.Exp10(exponent);
            } else if (int.IsPow2(a)) {
                var c = int.TrailingZeroCount(a);
                p = BigInteger.One << checked(c * exponent);
            } else {
                return BigInteger.Pow(value, exponent);
            }
            return n && int.IsOddInteger(exponent) ? -p : p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Pow<T>([ConstantExpected(Min = 2)] T value, int exponent)
            where T: IBinaryInteger<T>?, ISignedNumber<T>? {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }
            if (value <= T.One) {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            if (exponent < 0) {
                throw new ArgumentOutOfRangeException(nameof(exponent));
            }
            if (exponent == 0) {
                return BigInteger.One;
            }
            var n = T.IsNegative(value);
            var a = T.CopySign(value, T.One);
            if (T.IsZero(a)) {
                return BigInteger.Zero;
            }
            BigInteger p;
            if (NumberBaseExtensions.IsOne(a)) {
                p = BigInteger.One;
            } else if (NumberBaseExtensions.IsTen(a)) {
                p = BigIntegerSmallExp10Module.Exp10(exponent);
            } else if (T.IsPow2(a)) {
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
                var c = int.CreateChecked(T.TrailingZeroCount(a));
#pragma warning restore CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
                p = BigInteger.One << checked(c * exponent);
            } else {
#pragma warning disable CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
                return BigInteger.Pow(BigInteger.CreateChecked(value), exponent);
#pragma warning restore CS8631 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match constraint type.
            }
            return n && int.IsOddInteger(exponent) ? -p : p;
        }
    }

    internal static partial class BigIntegerSmallExp10Module {

        private static readonly BigInteger[] _small = InitSmall();

        private static BigInteger[] InitSmall() {
            var a = new BigInteger[40]; // 10^39 > 2^128
            a[0] = BigInteger.One;
            for (int i = 1; i < a.Length; i++) {
                a[i] = a[i - 1] * 10;
            }
            return a;
        }

        private static readonly BigInteger[] _table5_4 = InitTable5_4();

        private static BigInteger[] InitTable5_4() {
            // n => 10^(2^5 * n)  where 2^5 = 32  => exponent = 32 * n
            const int entries = 16;
            var result = new BigInteger[entries];

            BigInteger step = BigInteger.Pow(10, 32); // 10^(32)
            BigInteger current = BigInteger.One;      // 10^(32*0) == 1

            for (int i = 0; i < entries; i++) {
                result[i] = current;
                current *= step;
            }
            return result;
        }

        private static readonly BigInteger[] _table9_4 = InitTable9_4();

        private static BigInteger[] InitTable9_4() {
            // n => 10^(2^9 * n)  where 2^9 = 512  => exponent = 512 * n
            const int entries = 16;
            var result = new BigInteger[entries];

            BigInteger step = BigInteger.Pow(10, 512); // 10^(512)
            BigInteger current = BigInteger.One;       // 10^(512*0) == 1

            for (int i = 0; i < entries; i++) {
                result[i] = current;
                current *= step;
            }
            return result;
        }

        /*
        static readonly BigInteger Ten = 10;
        // _table9_4[8] == 10^(2^9 * 8) == 10^(2^12)
        // 10^(2^13) == (10^(2^12))^2
        private static readonly BigInteger _Exp10_2Pow13 = _table9_4[8] * _table9_4[8]; // 10^(2^13)
        */
        private static readonly BigInteger _Exp10_2Pow12 = _table9_4[8]; // _table9_4[8] == 10^(2^9 * 8) == 10^(2^12)

        [ModuleInitializer]
        internal static void ModuleInit() {
            RuntimeHelpers.RunClassConstructor(typeof(BigIntegerExtensions).TypeHandle);
        }

        public static BigInteger Exp10(int exponent) {
            if (exponent < 0) {
                throw new ArgumentOutOfRangeException(nameof(exponent));
            }
            if (exponent < _small.Length) {
                return _small[exponent];
            }
            int remaining = exponent;
            BigInteger result;
            {
                int low5 = remaining & 0x1F;
                result = _small[low5];
                remaining >>= 5;
            }
            if (remaining == 0) {
                return result;
            }
            {
                int low4 = remaining & 0xF;
                result *= _table5_4[low4];
                remaining >>= 4;
            }
            if (remaining == 0) {
                return result;
            }
            {
                int low4 = remaining & 0xF;
                result *= _table9_4[low4];
                remaining >>= 4;
            }
            if (remaining == 0) {
                return result;
            }
            /*
            // example: remaining == 0B_1001
            // 10^(2^13 * 9) == (10^(2^13))^9
            return result * BigInteger.Pow(_Exp10_2Pow13, remaining);
            */
            BigInteger current = _Exp10_2Pow12;
            for (int bit = 1; ; bit <<= 1) {
                current *= current;
                if ((remaining & bit) != 0) {
                    result *= current;
                    remaining -= bit;
                    if (remaining == 0) {
                        return result;
                    }
                }
            }
        }
    }
}
