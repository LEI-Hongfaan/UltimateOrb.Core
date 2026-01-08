using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

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
