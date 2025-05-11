using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    static class HalfHelpers {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(byte value) {
            return BitConverter.Int16BitsToHalf(unchecked((Int16)ToHalfPartialU8(value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(ushort value) {
            return 65520 <= value ? Half.PositiveInfinity : BitConverter.Int16BitsToHalf(unchecked((Int16)ToHalfPartial(value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(uint value) {
            return 65520 <= value ? Half.PositiveInfinity : BitConverter.Int16BitsToHalf(unchecked((Int16)ToHalfPartial(value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(ulong value) {
            return 65520 <= value ? Half.PositiveInfinity : BitConverter.Int16BitsToHalf(unchecked((Int16)ToHalfPartial((uint)value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToHalfPartial(uint value) {
            Debug.Assert(value <= 65519);
            unchecked {
                const int exponentBias = 15;
                const int mantissaBits = 10;

                if (value == 0) {
                    return 0;
                }

                // Count leading zeros to find the position of the highest set bit
                var leadingZeros = Mathematics.BinaryNumerals.CountLeadingZeros(value);

                int exponent = 32 - 1 + exponentBias - leadingZeros;

                uint shifted = value << (1 + leadingZeros);

                uint mantissa = shifted >>> (32 - mantissaBits);

                // Remaining bits for rounding (bits 21-0)
                uint remaining = shifted & 0x3FFFFF;

                // Apply rounding (round to nearest, ties to even)
                if (remaining > 0x200000 || (remaining == 0x200000 && 0 != (1 & mantissa))) {
                    ++mantissa;
                }

                // Combine into half-precision format (sign bit is 0 for unsigned input)
                return (exponent << mantissaBits) + (int)mantissa;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToHalfPartialU8(uint value) {
            Debug.Assert(value < 256);
            unchecked {
                const int exponentBias = 15;
                const int mantissaBits = 10;

                if (value == 0) {
                    return 0;
                }

                // Count leading zeros to find the position of the highest set bit
                var leadingZeros = Mathematics.BinaryNumerals.CountLeadingZeros(value);

                int exponent = 32 - 1 + exponentBias - leadingZeros;

                uint shifted = value << (1 + leadingZeros);

                uint mantissa = shifted >>> (32 - mantissaBits);

                // Combine into half-precision format (sign bit is 0 for unsigned input)
                return (exponent << mantissaBits) + (int)mantissa;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(int value) {

        }

         [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int ToHalfPartial(int value) {
            Debug.Assert(value <= 65519);
            unchecked {
                const int exponentBias = 15;
                const int mantissaBits = 10;

                if (value == 0) {
                    return 0;
                }
                UInt32;
                // Count leading zeros to find the position of the highest set bit
                var leadingZeros = Mathematics.BinaryNumerals.CountLeadingZeros(value);

                int exponent = 32 - 1 + exponentBias - leadingZeros;

                uint shifted = value << (1 + leadingZeros);

                uint mantissa = shifted >>> (32 - mantissaBits);

                // Remaining bits for rounding (bits 21-0)
                uint remaining = shifted & 0x3FFFFF;

                // Apply rounding (round to nearest, ties to even)
                if (remaining > 0x200000 || (remaining == 0x200000 && 0 != (1 & mantissa))) {
                    ++mantissa;
                }

                // Combine into half-precision format (sign bit is 0 for unsigned input)
                return (exponent << mantissaBits) + (int)mantissa;
            }
        }
    }
}
