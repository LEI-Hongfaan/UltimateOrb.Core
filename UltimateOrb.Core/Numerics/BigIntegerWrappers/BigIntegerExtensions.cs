using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Extensions;

namespace UltimateOrb.Numerics.BigIntegerWrappers {
    using UltimateOrb.Runtime.CompilerServices;

    public static class BigIntegerExtensions {

        private readonly static int BitsFieldOffset = GetBitsFieldOffset();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetBitsFieldOffset() {
            return GetFieldInfo("_bits").GetFieldOffset();
        }

        private readonly static int SignFieldOffset = GetSignFieldOffset();

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GetSignFieldOffset() {
            return GetFieldInfo("_sign").GetFieldOffset();
        }

#if NET8_0_OR_GREATER && USE_UNSAFE_ACCESS_TO_STD_BIGINTEGER
        /*
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_sign")]
        internal extern static ref readonly int GetSignField(this ref readonly System.Numerics.BigInteger obj);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static ref readonly uint[] GetBitsField(this ref readonly System.Numerics.BigInteger obj);
        */

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_sign")]
        internal extern static int GetSignField(this System.Numerics.BigInteger obj);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static uint[] GetBitsField(this System.Numerics.BigInteger obj);

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        internal extern static System.Numerics.BigInteger CreateBigIntegerInternal(int sign, uint[]? bits);
#endif

        private static FieldInfo GetFieldInfo(string name) {
            var r = typeof(BigInteger).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (r is null) {
                throw new PlatformNotSupportedException();
            }
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static void GetInternalFields(this BigInteger value, out int sign, out uint[]? bits) {
            sign = Unsafe.As<BigInteger, int>(ref Unsafe.AddByteOffset(ref value, SignFieldOffset));
            bits = Unsafe.As<BigInteger, uint[]?>(ref Unsafe.AddByteOffset(ref value, BitsFieldOffset));
        }

        /// <summary>
        /// Gets the number of bits required for shortest two's complement representation of the current instance without the sign bit.
        /// </summary>
        /// <returns>The minimum non-negative number of bits in two's complement notation without the sign bit.</returns>
        /// <remarks>This method returns 0 iff the value of current object is equal to <see cref="Zero"/> or <see cref="MinusOne"/>. For positive integers the return value is equal to the ordinary binary representation string length.</remarks>
        [Obsolete]
        internal static long GetBitLength(this BigInteger value) {
            uint highValue;
            int bitsArrayLength;
            value.GetInternalFields(out var sign, out var bits);

            if (bits == null) {
                bitsArrayLength = 1;
                highValue = (uint)(sign < 0 ? -sign : sign);
            } else {
                bitsArrayLength = bits.Length;
                highValue = bits[bitsArrayLength - 1];
            }

            long bitLength = bitsArrayLength * 32L - BitOperations.LeadingZeroCount(highValue);

            if (sign >= 0) {
                return bitLength;
            }

            // When negative and IsPowerOfTwo, the answer is (bitLength - 1)

            // Check highValue
            if ((highValue & (highValue - 1)) != 0) {
                return bitLength;
            }

            // Check the rest of the bits (if present)
            for (int i = bitsArrayLength - 2; i >= 0; i--) {
                // bits array is always non-null when bitsArrayLength >= 2
                if (bits![i] == 0) {
                    continue;
                }

                return bitLength;
            }

            return bitLength - 1;
        }
    }
}
