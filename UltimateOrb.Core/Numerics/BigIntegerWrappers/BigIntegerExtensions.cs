using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Extensions;

namespace UltimateOrb.Numerics.BigIntegerWrappers {
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using UltimateOrb.Mathematics.Elementary;
    using UltimateOrb.Runtime.CompilerServices;
    using UltimateOrb.Utilities;

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
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_sign")]
        internal extern static ref readonly int GetSignField(this ref readonly System.Numerics.BigInteger obj);

#if NET11_0_OR_GREATER
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static ref readonly nuint[] GetBitsField(this ref readonly System.Numerics.BigInteger obj);
        
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        internal extern static System.Numerics.BigInteger CreateBigIntegerInternal(int sign, nuint[]? bits);
#else
        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static ref readonly uint[] GetBitsField(this ref readonly System.Numerics.BigInteger obj);

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        internal extern static System.Numerics.BigInteger CreateBigIntegerInternal(int sign, uint[]? bits);
#endif
#endif

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static ref readonly uint[] GetBitsFieldUInt(this ref readonly System.Numerics.BigInteger obj);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_bits")]
        internal extern static ref readonly nuint[] GetBitsFieldUIntPtr(this ref readonly System.Numerics.BigInteger obj);

        internal static bool? BigIntegerHasUIntBits { get; } = GetBigIntegerHasUIntBits();

        private static bool? GetBigIntegerHasUIntBits() {
            try {
                var elemType = typeof(BigInteger).GetField("_bits")!.FieldType.GetElementType();
                if (elemType == null) {
                    return null;
                }
                return typeof(uint) == elemType;
            } catch (Exception) {
                return null;
            }
        }

        internal static bool? BigIntegerHasUIntPtrBits { get; } = GetBigIntegerHasUIntPtrBits();

        private static bool? GetBigIntegerHasUIntPtrBits() {
            try {
                var elemType = typeof(BigInteger).GetField("_bits")!.FieldType.GetElementType();
                if (elemType == null) {
                    return null;
                }
                return typeof(nuint) == elemType;
            } catch (Exception) {
                return null;
            }
        }


        private static FieldInfo GetFieldInfo(string name) {
            var r = typeof(BigInteger).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
            if (r is null) {
                throw new PlatformNotSupportedException();
            }
            return r;
        }

        [Obsolete]
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

        /// <summary>
        /// Gets the unsigned magnitude representation of a <see cref="BigInteger"/> as a span of
        /// native unsigned integers.
        /// </summary>
        /// <param name="value">
        /// The <see cref="BigInteger"/> value whose magnitude bits are retrieved.
        /// The sign is ignored; the returned representation is always non-negative.
        /// </param>
        /// <param name="buffer">
        /// Storage used when <paramref name="value"/> can be represented by a single native word.
        /// When used, the returned span references this variable directly.
        /// </param>
        /// <returns>
        /// A read-only span containing the magnitude limbs of <paramref name="value"/> in little-endian
        /// order, where each element contains <see cref="nuint"/> bits.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method avoids allocations by returning a span over the internal magnitude storage of
        /// <paramref name="value"/> whenever possible.
        /// </para>
        /// <para>
        /// For values that fit in a single machine word, the magnitude is written to
        /// <paramref name="buffer"/> and a one-element span referencing that storage is returned.
        /// </para>
        /// <para>
        /// The internal representation of <see cref="BigInteger"/> differs between runtime versions.
        /// This method supports runtimes exposing either <c>uint</c>-based or <c>nuint</c>-based
        /// magnitude storage. Unsupported layouts cause a <see cref="NotSupportedException"/> to be thrown.
        /// </para>
        /// <para>
        /// On 64-bit systems using <c>uint</c>-based storage, two consecutive 32-bit limbs are combined
        /// into a single <see cref="nuint"/> value. This requires a little-endian architecture.
        /// </para>
        /// <para>
        /// The returned span may reference memory owned by <paramref name="value"/> or the supplied
        /// <paramref name="buffer"/>. The span must not be used after the lifetime of the referenced
        /// storage has ended.
        /// </para>
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Thrown when the current runtime's <see cref="BigInteger"/> internal representation is not
        /// supported, or when a supported representation requires an unsupported endianness.
        /// </exception>
        public static ReadOnlySpan<nuint> GetMagnitudeBitsAsSpan(this BigInteger value, ref nuint buffer) {
            if (BigIntegerHasUIntBits is true) {
                var bits = value.GetBitsFieldUInt();
                if (bits == null) {
                    buffer = Math.AbsAsUnsigned(value.GetSignField());
                    return new (ref buffer);
                } else {
                    Span<uint> s = bits.AsSpan();
                    Debug.Assert(s.Length >= 1); 
                    if (nuint.Size > sizeof(uint)) {
                        Debug.Assert(nuint.Size == 2 * sizeof(uint));
                        if (!BitConverter.IsLittleEndian) {
                            throw new NotSupportedException();
                        }
                        var l = ((s.Length.ToUnsignedUnchecked() + 1u) / 2u).ToSignedUnchecked();
                        var t = MemoryMarshal.CreateSpan(ref Unsafe.As<uint, nuint>(ref MemoryMarshal.GetReference(s)), l);
                        if (0 != (1 & s.Length)) {
                            Unsafe.Add(ref MemoryMarshal.GetReference(s), s.Length) = 0;
                        }
                        return t;
                    }
                    Debug.Assert(nuint.Size == sizeof(uint));
                    return MemoryMarshal.CreateSpan(ref Unsafe.As<uint, nuint>(ref MemoryMarshal.GetReference(s)), s.Length);
                }
            }
            if (BigIntegerHasUIntPtrBits is true) {
                var bits = value.GetBitsFieldUIntPtr();
                if (bits == null) {
                    buffer = Math.AbsAsUnsigned(value.GetSignField());
                    return new(ref buffer);
                } else {
                    Span<nuint> s = bits.AsSpan();
                    Debug.Assert(s.Length >= 1);
                    return s;
                }
            }
            throw new NotSupportedException();
        }
    }
}
