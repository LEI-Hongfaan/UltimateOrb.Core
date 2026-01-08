using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics;
using DoubleArithmetic = UltimateOrb.Numerics.DoubleArithmetic;

namespace UltimateOrb {

    partial class UInt128Exp10Module {

        // C# 12 collection expression for ReadOnlySpan<ulong>
        public static ReadOnlySpan<ulong> Exp10Interleaved => [
            0X0000000000000001UL, 0X0000000000000000UL, // 10^0
            0X000000000000000aUL, 0X0000000000000000UL, // 10^1
            0X0000000000000064UL, 0X0000000000000000UL, // 10^2
            0X00000000000003e8UL, 0X0000000000000000UL, // 10^3
            0X0000000000002710UL, 0X0000000000000000UL, // 10^4
            0X00000000000186a0UL, 0X0000000000000000UL, // 10^5
            0X00000000000f4240UL, 0X0000000000000000UL, // 10^6
            0X0000000000989680UL, 0X0000000000000000UL, // 10^7
            0X0000000005f5e100UL, 0X0000000000000000UL, // 10^8
            0X000000003b9aca00UL, 0X0000000000000000UL, // 10^9
            0X00000002540be400UL, 0X0000000000000000UL, // 10^10
            0X000000174876e800UL, 0X0000000000000000UL, // 10^11
            0X000000e8d4a51000UL, 0X0000000000000000UL, // 10^12
            0X000009184e72a000UL, 0X0000000000000000UL, // 10^13
            0X00005af3107a4000UL, 0X0000000000000000UL, // 10^14
            0X00038d7ea4c68000UL, 0X0000000000000000UL, // 10^15
            0X002386f26fc10000UL, 0X0000000000000000UL, // 10^16
            0X016345785d8a0000UL, 0X0000000000000000UL, // 10^17
            0X0de0b6b3a7640000UL, 0X0000000000000000UL, // 10^18
            0X8ac7230489e80000UL, 0X0000000000000000UL, // 10^19
            0X6bc75e2d63100000UL, 0X0000000000000005UL, // 10^20
            0X35c9adc5dea00000UL, 0X0000000000000036UL, // 10^21
            0X19e0c9bab2400000UL, 0X000000000000021eUL, // 10^22
            0X02c7e14af6800000UL, 0X000000000000152dUL, // 10^23
            0X1bcecceda1000000UL, 0X000000000000d3c2UL, // 10^24
            0X161401484a000000UL, 0X0000000000084595UL, // 10^25
            0Xdcc80cd2e4000000UL, 0X000000000052b7d2UL, // 10^26
            0X9fd0803ce8000000UL, 0X00000000033b2e3cUL, // 10^27
            0X3e25026110000000UL, 0X00000000204fce5eUL, // 10^28
            0X6d7217caa0000000UL, 0X00000001431e0faeUL, // 10^29
            0X4674edea40000000UL, 0X0000000c9f2c9cd0UL, // 10^30
            0Xc0914b2680000000UL, 0X0000007e37be2022UL, // 10^31
            0X85acef8100000000UL, 0X000004ee2d6d415bUL, // 10^32
            0X38c15b0a00000000UL, 0X0000314dc6448d93UL, // 10^33
            0X378d8e6400000000UL, 0X0001ed09bead87c0UL, // 10^34
            0X2b878fe800000000UL, 0X0013426172c74d82UL, // 10^35
            0Xb34b9f1000000000UL, 0X00c097ce7bc90715UL, // 10^36
            0X00f436a000000000UL, 0X0785ee10d5da46d9UL, // 10^37
            0X098a224000000000UL, 0X4b3b4ca85a86c47aUL, // 10^38
        ];
    }

    internal static partial class UInt128Exp10Module {

        public static UltimateOrb.UInt128 Exp10(int index) {
            if ((uint)index >= Exp10Interleaved.Length) throw new ArgumentOutOfRangeException(nameof(index));
            int baseIdx = index * 2;
            var span = Exp10Interleaved;

            ulong lower = span[baseIdx];       // stored little-endian low word
            ulong upper = span[baseIdx + 1];   // stored little-endian high word

            if (!BitConverter.IsLittleEndian) {
                lower = BinaryPrimitives.ReverseEndianness(lower);
                upper = BinaryPrimitives.ReverseEndianness(upper);
            }

            return lower | ((UltimateOrb.UInt128)upper << 64);
        }

        const double Log10Of2 = 0.3010299956639811952;

        public static int ILog10(UltimateOrb.UInt128 value) {
            unchecked {
                var m = value;
                --m;
                if (m >= (0X098a223fffffffffUL | ((UltimateOrb.UInt128)0X4b3b4ca85a86c47aUL << 64))) {
                    if (value.IsZero) {
                        return ILogSpecialResults.ILog0;
                    }
                    return 38;
                }
                var a = (double)value;
                var a2 = Math.Log2(a);
                var b = (int)(Log10Of2 * a2 + 0.125);
                var c = Exp10(b);
                var f = value.CompareTo(c);
                if (f == 0) return b;
                if (f > 0) {
                    // value > c
                    ++b;
                    c = Exp10(b);
                    if (value < c) {
                        return --b;
                    }
                    return b;
                } else {
                    // value < c
                    --b;
                    c = Exp10(b);
                    if (value >= c) {
                        return b;
                    }
                    return --b;
                }
            }
        }

        static int TrailingZeroCountBase10_Stub4(UInt32 value) {
            Debug.Assert(value != 0);
            Debug.Assert(value < 0X100U);
            Int64 m = 0 != (value & 64u) ?
                0B_00000001_00000000_01000000_00010000_00000100_00000001_00000000_01000000 :
                0B_00010000_00000100_00000001_00000000_01000000_00010000_00000100_00000001;
            return 1 & unchecked((int)(m >> (int)value));
        }

        static int TrailingZeroCountBase10_Stub3(UInt32 value) {
            Debug.Assert(value < 0X2710U);
            var (q, r) = Math.DivRem(value, 0X64U);
            if (r != 0) {
                return TrailingZeroCountBase10_Stub4(r);
            }
            return 2 + TrailingZeroCountBase10_Stub4(q);
        }

        static int TrailingZeroCountBase10_Stub2(UInt32 value) {
            Debug.Assert(value < 0X05f5e100U);
            var (q, r) = Math.DivRem(value, 0X2710U);
            if (r != 0) {
                return TrailingZeroCountBase10_Stub3(r);
            }
            Debug.Assert(q != 0);
            return 4 + TrailingZeroCountBase10_Stub3(q);
        }

        static int TrailingZeroCountBase10_Stub1(UInt64 value) {
            Debug.Assert(value < 0X002386f26fc10000UL);
            var (q, r) = Math.DivRem(value, 0X05f5e100U);
            if (r != 0) {
                return TrailingZeroCountBase10_Stub2(unchecked((UInt32)r));
            }
            Debug.Assert(q != 0);
            return 8 + TrailingZeroCountBase10_Stub2(unchecked((UInt32)q));
        }

        public static int UInt96TrailingZeroCountBase10(UltimateOrb.UInt128 value) {
            Debug.Assert(value < UltimateOrb.UInt128.One << 96);
            var lo = (ulong)value.LoInt64Bits;
            var hi = (ulong)value.HiInt64Bits;
            var q = DoubleArithmetic.BigDivRem(lo, hi, 0X002386f26fc10000UL, out var r);
            if (r != 0) {
                return TrailingZeroCountBase10_Stub1(r);
            }
            if (q == 0) {
                return int.MinValue;
            }
            return 16 + TrailingZeroCountBase10_Stub1(q);
        }

        public static int TrailingZeroCountBase10(UltimateOrb.UInt128 value) {
            unchecked {
                var lo = (ulong)value.LoInt64Bits;
                var hi = (ulong)value.HiInt64Bits;
                uint s = 0;
                if (hi >= 0X8ac7230489e80000UL) {
                    hi -= 0X8ac7230489e80000UL;
                    s = 1;
                }
                var q = DoubleArithmetic.BigDivRem(lo, hi, 0X8ac7230489e80000UL, out var r);
                if (r != 0) {
                    return UInt96TrailingZeroCountBase10(r);
                }
                if ((q | s) == 0) {
                    return int.MinValue;
                }
                return 19 + UInt96TrailingZeroCountBase10(q | ((UltimateOrb.UInt128)s << 64));
            }
        }
    }
}
