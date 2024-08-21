using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Mathematics;

namespace UltimateOrb.Utilities.Extensions {

    public static partial class BitPatternEnumerable {

        static IEnumerable<UInt32> GetUInt32BitsCore(int bitLength, int popCount) {
            Debug.Assert(bitLength > 0);
            Debug.Assert(bitLength <= 32);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= bitLength);
            var a = unchecked(((UInt32)1 << popCount) - 1);
            var b = a << unchecked(bitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = BinaryNumerals.NextPermutation(a);
            }
        }

        public static IEnumerable<UInt32> GetUInt32Bits(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 32);
            if (1 <= bitLength && 0 <= popCount && popCount <= bitLength) {
                foreach (var v in GetUInt32BitsCore(bitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }

        public static IEnumerable<UInt32> GetUInt32BitsWithPopCountLessThanOrEqual(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 32);
            if (1 <= bitLength && 0 <= popCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(bitLength, popCount); ++i) {
                    foreach (var item in GetUInt32BitsCore(bitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }

        static IEnumerable<UInt64> GetUInt64BitsCore(int bitLength, int popCount) {
            Debug.Assert(bitLength > 0);
            Debug.Assert(bitLength <= 64);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= bitLength);
            var a = unchecked(((UInt64)1 << popCount) - 1);
            var b = a << unchecked(bitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = BinaryNumerals.NextPermutation(a);
            }
        }

        public static IEnumerable<UInt64> GetUInt64Bits(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 64);
            if (1 <= bitLength && 0 <= popCount && popCount <= bitLength) {
                foreach (var v in GetUInt64BitsCore(bitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }

        public static IEnumerable<UInt64> GetUInt64BitsWithPopCountLessThanOrEqual(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 64);
            if (1 <= bitLength && 0 <= popCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(bitLength, popCount); ++i) {
                    foreach (var item in GetUInt64BitsCore(bitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }

        static IEnumerable<UInt128> GetUInt128BitsCore(int bitLength, int popCount) {
            Debug.Assert(bitLength > 0);
            Debug.Assert(bitLength <= 128);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= bitLength);
            var a = unchecked(((UInt128)1 << popCount) - 1);
            var b = a << unchecked(bitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = UInt128.BinaryNumerals.NextPermutation(a);
            }
        }

        public static IEnumerable<UInt128> GetUInt128Bits(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 128);
            if (1 <= bitLength && 0 <= popCount && popCount <= bitLength) {
                foreach (var v in GetUInt128BitsCore(bitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }

        public static IEnumerable<UInt128> GetUInt128BitsWithPopCountLessThanOrEqual(int bitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(bitLength, 128);
            if (1 <= bitLength && 0 <= popCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(bitLength, popCount); ++i) {
                    foreach (var item in GetUInt128BitsCore(bitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (bitLength | popCount)) {
                yield return default;
            }
        }
    }
}
