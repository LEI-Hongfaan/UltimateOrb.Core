using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using StructLayoutHelpers = UltimateOrb.Runtime.CompilerServices.StructLayoutHelpers;

namespace UltimateOrb.Numerics {

    static partial class MultiArithmetic {

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        public static UInt<TStorage> AddUnchecked<TStorage>(UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged {
            if (sizeof(TStorage) == sizeof(uint)) {
                return AddUncheckedPrimitive<TStorage, uint>(first, second);
            }
            if (sizeof(TStorage) == nuint.Size) {
                return AddUncheckedPrimitive<TStorage, nuint>(first, second);
            }
            if (sizeof(TStorage) == sizeof(ulong)) {
                return AddUncheckedPrimitive<TStorage, ulong>(first, second);
            }
            if (sizeof(TStorage) == sizeof(System.UInt128)) {
                return AddUncheckedPrimitiveWidening<TStorage, System.UInt128>(first, second);
            }
            if (sizeof(TStorage) <= sizeof(uint)) {
                return AddUncheckedPrimitiveWidening<TStorage, uint>(first, second);
            }
            if (sizeof(TStorage) <= nuint.Size) {
                return AddUncheckedPrimitiveWidening<TStorage, nuint>(first, second);
            }
            if (sizeof(TStorage) <= sizeof(ulong)) {
                return AddUncheckedPrimitiveWidening<TStorage, ulong>(first, second);
            }
            if (sizeof(TStorage) <= sizeof(System.UInt128)) {
                return AddUncheckedPrimitiveWidening<TStorage, System.UInt128>(first, second);
            }
            {
                return AddUncheckedNaive<TStorage>(first, second);
            }
        }

        // ---- primitive add with carry ----
        internal static TUInt AddWithCarry<TUInt>(TUInt first, TUInt second, nuint carry, out nuint newCarry)
            where TUInt : unmanaged, IBinaryInteger<TUInt>, IMinMaxValue<TUInt> {

            var c = TUInt.CreateTruncating(carry);
            var s1 = unchecked(first + second);
            var carry1 = s1 < first ? (nuint)1 : (nuint)0;   // overflow iff wrapped below either operand

            var s2 = unchecked(s1 + c);
            var carry2 = s2 < s1 ? (nuint)1 : (nuint)0;

            newCarry = carry1 + carry2;
            return s2;
        }

        // ---- span-based, endian-aware addition ----
        internal static nuint AddUnsignedNoThrow<TUInt>(
            Span<TUInt> sum, ReadOnlySpan<TUInt> first, ReadOnlySpan<TUInt> second, bool isBigEndian)
            where TUInt : unmanaged, IBinaryInteger<TUInt>?, IUnsignedNumber<TUInt>?, IMinMaxValue<TUInt>? {

            Debug.Assert(sum.Length == first.Length);
            Debug.Assert(sum.Length == second.Length);

            int n = sum.Length;
            nuint carry = 0;

            if (!isBigEndian) {
                // Little-endian limbs: index 0 is least-significant.
                // Carry propagates from low index toward high index.
                for (int i = 0; i < n; i++) {
                    sum[i] = AddWithCarry(first[i], second[i], carry, out carry);
                }
            } else {
                // Big-endian limbs: index n-1 is least-significant.
                // Carry propagates from high index toward low index.
                for (int i = n - 1; i >= 0; i--) {
                    sum[i] = AddWithCarry(first[i], second[i], carry, out carry);
                }
            }

            return carry;
        }


        internal static nuint AddUnsignedNoThrow<TUInt>(Span<TUInt> sum, ReadOnlySpan<TUInt> first, ReadOnlySpan<TUInt> second)
            where TUInt : unmanaged, IBinaryInteger<TUInt>?, IUnsignedNumber<TUInt>?, IMinMaxValue<TUInt>? {
            return AddUnsignedNoThrow(sum, first, second, !BitConverter.IsLittleEndian);
        }


        internal static UInt<TStorage> AddUncheckedNaive<TStorage>(
            UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged {
            // Reference implementation routing through AddUnsignedNoThrow<TLimb>.
            //
            // Tier 1 (fast):  view UInt _storage in place as Span<nuint>.
            // Tier 2 (fast):  view UInt _storage in place as Span<uint>.
            // Tier 3 (slow):  copy into a padded (q+1)-limb nuint buffer, add, copy back.
            //
            // Each tier requires sizeof(TStorage) to be a multiple of sizeof(TLimb) and
            // that a TStorage-aligned address can be viewed as TLimb-aligned
            // (AlignOf<TStorage> % AlignOf<TLimb> == 0). Tier 1 is preferred when it
            // applies because fewer limbs means less carry-chain work; tier 2 catches
            // the cases tier 1 rejects on size or alignment grounds (e.g. 12-byte
            // TStorage on a 64-bit host).

            UInt<TStorage> result = default;
            bool isBigEndian = !BitConverter.IsLittleEndian;

            uint sizeTStorage = unchecked((uint)sizeof(TStorage));
            uint alignTStorage = unchecked((uint)StructLayoutHelpers.AlignOf<TStorage>());

            // ---- Tier 1: nuint limbs -------------------------------------------------

            uint alignNuint = unchecked((uint)StructLayoutHelpers.AlignOf<nuint>());
            var (qNuint, rNuint) = Math.DivRem(sizeTStorage, unchecked((uint)sizeof(nuint)));

            if (rNuint == 0 && 0u == alignTStorage % alignNuint) {
                int n = (int)qNuint;
                AddUnsignedNoThrow(
                    MemoryMarshal.CreateSpan(
                        ref Unsafe.As<TStorage, nuint>(ref Unsafe.AsRef(in result._storage)), n),
                    MemoryMarshal.CreateReadOnlySpan(
                        ref Unsafe.As<TStorage, nuint>(ref Unsafe.AsRef(in first._storage)), n),
                    MemoryMarshal.CreateReadOnlySpan(
                        ref Unsafe.As<TStorage, nuint>(ref Unsafe.AsRef(in second._storage)), n),
                    isBigEndian);
                return result;
            }

            // ---- Tier 2: uint limbs --------------------------------------------------

            uint alignUint = unchecked((uint)StructLayoutHelpers.AlignOf<uint>());
            var (qUint, rUint) = Math.DivRem(sizeTStorage, unchecked((uint)sizeof(uint)));

            if (rUint == 0 && 0u == alignTStorage % alignUint) {
                int n = (int)qUint;
                AddUnsignedNoThrow(
                    MemoryMarshal.CreateSpan(
                        ref Unsafe.As<TStorage, uint>(ref Unsafe.AsRef(in result._storage)), n),
                    MemoryMarshal.CreateReadOnlySpan(
                        ref Unsafe.As<TStorage, uint>(ref Unsafe.AsRef(in first._storage)), n),
                    MemoryMarshal.CreateReadOnlySpan(
                        ref Unsafe.As<TStorage, uint>(ref Unsafe.AsRef(in second._storage)), n),
                    isBigEndian);
                return result;
            }

            // ---- Tier 3: padded nuint buffer ----------------------------------------
            //
            // Reached when sizeof(TStorage) is not a multiple of sizeof(uint) (or
            // TStorage is more weakly aligned than uint). The padded buffer is
            // stackalloc'd as nuint[] so it is nuint-aligned and zero-initialized by
            // the runtime; the zeroed padding bytes are exactly the zero-extension we
            // need, and paddingSum is fully written by AddUnsignedNoThrow.
            //
            // Padding placement (srcOffset = where byte 0 of the value lands):
            //   LE: byte 0 is the LSB, so pad on the top    -> offset 0
            //   BE: byte 0 is the MSB, so pad on the bottom -> offset paddedBytes - sizeof(TStorage)

            int limbCount2 = (int)(rNuint == 0 ? qNuint : qNuint + 1);
            int paddedBytes = limbCount2 * sizeof(nuint);
            int srcOffset = isBigEndian ? paddedBytes - sizeof(TStorage) : 0;

            Span<nuint> paddedFirstN = stackalloc nuint[limbCount2];
            Span<nuint> paddedSecondN = stackalloc nuint[limbCount2];
            Span<nuint> paddedSumN = stackalloc nuint[limbCount2];

            Span<byte> paddedFirst = MemoryMarshal.AsBytes(paddedFirstN);
            Span<byte> paddedSecond = MemoryMarshal.AsBytes(paddedSecondN);
            Span<byte> paddedSum = MemoryMarshal.AsBytes(paddedSumN);

            ref byte pFirst = ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in first._storage));
            ref byte pSecond = ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in second._storage));
            for (int i = 0; i < sizeof(TStorage); i++) {
                paddedFirst[srcOffset + i] = Unsafe.Add(ref pFirst, i);
                paddedSecond[srcOffset + i] = Unsafe.Add(ref pSecond, i);
            }

            AddUnsignedNoThrow(paddedSumN, paddedFirstN, paddedSecondN, isBigEndian);

            ref byte pResult = ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in result._storage));
            for (int i = 0; i < sizeof(TStorage); i++) {
                Unsafe.Add(ref pResult, i) = paddedSum[srcOffset + i];
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt<TStorage> AddUncheckedPrimitive<TStorage, TUInt>(
            UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged
            where TUInt : unmanaged, IBinaryInteger<TUInt>, IMinMaxValue<TUInt> {
            Debug.Assert(sizeof(TStorage) == sizeof(TUInt));

            // Same-width specialization of AddUncheckedPrimitiveWidening.
            //
            // With sizeof(TStorage) == sizeof(TUInt), CreateWideningInternal and
            // CreateTruncating each collapse to a bit-reinterpret in one direction:
            // the (sizeof(TUInt) - sizeof(TStorage)) offset is 0, so the
            // BitConverter.IsLittleEndian test and the second (offset-alignment)
            // clause in those helpers are dead. The only remaining correctness
            // concern is whether a TStorage-aligned address can be viewed as TUInt.

            UInt<TStorage> r = default;
            ref TStorage dst = ref Unsafe.AsRef(in r._storage);

            TUInt s;
            if (0u == unchecked((uint)StructLayoutHelpers.AlignOf<TStorage>()) %
                        unchecked((uint)StructLayoutHelpers.AlignOf<TUInt>())) {
                // TStorage alignment is at least TUInt's: reinterpret in place.
                TUInt a = Unsafe.As<TStorage, TUInt>(ref Unsafe.AsRef(in first._storage));
                TUInt b = Unsafe.As<TStorage, TUInt>(ref Unsafe.AsRef(in second._storage));
                s = unchecked(a + b);
                Unsafe.As<TStorage, TUInt>(ref dst) = s;
            } else {
                // TStorage is more weakly aligned than TUInt: go through unaligned.
                TUInt a = Unsafe.ReadUnaligned<TUInt>(
                    ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in first._storage)));
                TUInt b = Unsafe.ReadUnaligned<TUInt>(
                    ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in second._storage)));
                s = unchecked(a + b);
                Unsafe.WriteUnaligned(ref Unsafe.As<TStorage, byte>(ref dst), s);
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt<TStorage> AddUncheckedPrimitiveWidening<TStorage, TUInt>(UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged
            where TUInt : unmanaged, IBinaryInteger<TUInt>, IMinMaxValue<TUInt> {
            Debug.Assert(sizeof(TStorage) <= sizeof(TUInt));

            // Zero-extend both operands into TUInt (wider limb), add without overflow,
            // then truncate back down to TStorage.
            TUInt a = CreateWideningInternal<TStorage, TUInt>(first._storage);
            TUInt b = CreateWideningInternal<TStorage, TUInt>(second._storage);
            TUInt s = unchecked(a + b);

            return CreateTruncating<TStorage, TUInt>(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TUInt CreateWideningInternal<TStorage, TUInt>(TStorage bits)
            where TStorage : unmanaged
            where TUInt : unmanaged {
            TUInt r = default;
            ref TStorage dst = ref (!BitConverter.IsLittleEndian ?
                ref Unsafe.AddByteOffset(ref Unsafe.As<TUInt, TStorage>(ref r), (nuint)checked(unchecked((uint)sizeof(TUInt)) - unchecked((uint)sizeof(TStorage)))) :
                ref Unsafe.As<TUInt, TStorage>(ref r));
            ref readonly TStorage src = ref bits;
            if (0u == unchecked((uint)StructLayoutHelpers.AlignOf<TUInt>()) % unchecked((uint)StructLayoutHelpers.AlignOf<TStorage>()) &&
                (BitConverter.IsLittleEndian || 0u == checked(unchecked((uint)sizeof(TUInt)) - unchecked((uint)sizeof(TStorage))) % (uint)StructLayoutHelpers.AlignOf<TStorage>())) {
                dst = src;
                return r;
            } else {
                Unsafe.WriteUnaligned(ref Unsafe.As<TStorage, byte>(ref dst), src);
                return r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt<TStorage> CreateTruncating<TStorage, TUInt>(TUInt value)
            where TStorage : unmanaged
            where TUInt : unmanaged {
            ref TStorage src = ref (!BitConverter.IsLittleEndian ?
                ref Unsafe.AddByteOffset(ref Unsafe.As<TUInt, TStorage>(ref value), (nuint)checked(unchecked((uint)sizeof(TUInt)) - unchecked((uint)sizeof(TStorage)))) :
                ref Unsafe.As<TUInt, TStorage>(ref value));
            UInt<TStorage> r = default;
            ref TStorage dst = ref Unsafe.AsRef(in r._storage);
            if (0u == unchecked((uint)StructLayoutHelpers.AlignOf<TUInt>()) % unchecked((uint)StructLayoutHelpers.AlignOf<TStorage>()) &&
              (BitConverter.IsLittleEndian || 0u == checked(unchecked((uint)sizeof(TUInt)) - unchecked((uint)sizeof(TStorage))) % (uint)StructLayoutHelpers.AlignOf<TStorage>())) {
                dst = src;
                return r;
            } else {
                dst = Unsafe.ReadUnaligned<TStorage>(ref Unsafe.As<TStorage, byte>(ref src));
                return r;
            }
        }

        public static BigInteger ToBigInteger<TStorage>(UInt<TStorage> value)
            where TStorage : unmanaged {
            ReadOnlySpan<byte> bytes = MemoryMarshal.CreateSpan(
                ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in value._storage)),
                sizeof(TStorage));

            // _storage holds the raw host-endian bit pattern of the unsigned value.
            // BigInteger's span ctor wants little-endian bytes, so tell it the host
            // endianness and mark the value unsigned.
            return new BigInteger(bytes,
                isUnsigned: true,
                isBigEndian: !BitConverter.IsLittleEndian);
        }

        public static BigInteger ToBigInteger<TStorage>(Int<TStorage> value)
            where TStorage : unmanaged {
            ReadOnlySpan<byte> bytes = MemoryMarshal.CreateSpan(
                ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in value._storage)),
                sizeof(TStorage));

            // _storage holds the raw two's-complement host-endian bit pattern of the
            // signed value. BigInteger interprets the bytes as a two's-complement
            // number of exactly sizeof(TStorage)*8 _storage and sign-extends from the
            // top byte, which is precisely Int's value.
            return new BigInteger(bytes,
                isUnsigned: false,
                isBigEndian: !BitConverter.IsLittleEndian);
        }

        public static Int<TStorage> ToSignedUnchecked<TStorage>(Int<TStorage> value)
            where TStorage : unmanaged {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int<TStorage> ToSignedUnchecked<TStorage>(UInt<TStorage> value)
            where TStorage : unmanaged {
            return Unsafe.BitCast<UInt<TStorage>, Int<TStorage>>(value);
        }

        public static UInt<TStorage> ToUnsignedUnchecked<TStorage>(Int<TStorage> value)
          where TStorage : unmanaged {
            return Unsafe.BitCast<Int<TStorage>, UInt<TStorage>>(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TStorage> ToUnsignedUnchecked<TStorage>(UInt<TStorage> value)
            where TStorage : unmanaged {
            return value;
        }

        private static class BigIntegerTraits<TStorage>
            where TStorage : unmanaged {
            // sizeof(TStorage) is a JIT-time constant per instantiation, so each closed
            // generic gets exactly one copy of these and the bounds are computed once.
            // BigInteger is a struct, so static readonly costs no heap.
            public static readonly int BitCount = checked(sizeof(TStorage) * 8);

            // Signed range: [-(2^(N-1)), 2^(N-1) - 1]
            public static readonly BigInteger SignedMinValue = -(BigInteger.One << (BitCount - 1));
            public static readonly BigInteger SignedMaxValue = (BigInteger.One << (BitCount - 1)) - 1;

            // Unsigned range: [0, 2^N - 1]
            public static readonly BigInteger UnsignedMaxValue = (BigInteger.One << BitCount) - 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int<TStorage> ToSignedUnchecked<TStorage>(BigInteger value)
            where TStorage : unmanaged {
            if (TryToSignedChecked<TStorage>(value, out var result)) {
                return result;
            }
            return ToSignedUnchecked(ToUnsignedUnchecked<TStorage>(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TStorage> ToUnsignedUnchecked<TStorage>(BigInteger value)
            where TStorage : unmanaged {
            if (TryToUnsignedChecked<TStorage>(value, out var result)) {
                return result;
            }
            return ToUnsignedUnchecked<TStorage>(value & BigIntegerTraits<TStorage>.UnsignedMaxValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryToSignedChecked<TStorage>(BigInteger value, out Int<TStorage> result)
            where TStorage : unmanaged {
            int size = sizeof(TStorage);

            // GetByteCount(isUnsigned: false) is the minimal 2's-complement byte count,
            // and it is exactly the number of bytes TryWriteBytes will produce. Comparing
            // it against `size` is the entire range check for the signed case.
            int c = value.GetByteCount(isUnsigned: false);
            if (c > size) {
                result = default;
                return false;
            }

            Int<TStorage> r = default;
            Span<byte> bytes = MemoryMarshal.CreateSpan(
                ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in r._storage)),
                size);

            // _storage is stored in host byte order, and TryWriteBytes writes at the start
            // of the destination span:
            //   LE host: LSB of the value goes at bytes[0]  -> write to bytes
            //   BE host: MSB of the value goes at bytes[0], so the LSB (last byte of
            //            the written sequence) must land on bytes[size-1] -> shift the
            //            destination so the whole `c`-byte write sits at the tail.
            Span<byte> shifted = !BitConverter.IsLittleEndian
                ? bytes.Slice(size - c)
                : bytes;

            var ok = value.TryWriteBytes(
                shifted, out var written,
                isUnsigned: false,
                isBigEndian: !BitConverter.IsLittleEndian);
            Debug.Assert(ok);
            Debug.Assert(written == c);

            if (written < size && value.Sign < 0) {
                // Sign-extend the bytes TryWriteBytes did not touch. `r = default`
                // already zeroed them, which is exactly the right fill for non-negative
                // values, so only negatives need work.
                if (BitConverter.IsLittleEndian) {
                    // Written bytes occupy [0, written); untouched tail is [written, size).
                    bytes.Slice(written).Fill(0xFF);
                } else {
                    // Written bytes occupy [size-written, size); untouched head is [0, size-written).
                    bytes.Slice(0, size - written).Fill(0xFF);
                }
            }

            result = r;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryToUnsignedChecked<TStorage>(BigInteger value, out UInt<TStorage> result)
            where TStorage : unmanaged {
            if (BigInteger.IsNegative(value)) {
                result = default;
                return false;
            }

            int size = sizeof(TStorage);
            int c = value.GetByteCount(isUnsigned: true);
            if (c > size) {
                result = default;
                return false;
            }

            UInt<TStorage> r = default;
            Span<byte> bytes = MemoryMarshal.CreateSpan(
                ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in r._storage)),
                size);

            Span<byte> shifted = !BitConverter.IsLittleEndian
                ? bytes.Slice(size - c)
                : bytes;

            var ok = value.TryWriteBytes(
                shifted, out var written,
                isUnsigned: true,
                isBigEndian: !BitConverter.IsLittleEndian);
            Debug.Assert(ok);
            Debug.Assert(written == c);

            // No fill block: value is non-negative, so any bytes TryWriteBytes didn't
            // touch must be zero. `r = default` already left them zeroed. If the
            // default-init ever goes away, this needs an explicit
            //   BitConverter.IsLittleEndian ? bytes.Slice(written) : bytes.Slice(0, size - written)
            // clear.

            result = r;
            return true;
        }


        // -----------------------------------------------------------------------------
        // Checked addition (unsigned)
        // -----------------------------------------------------------------------------

        public static UInt<TStorage> AddChecked<TStorage>(UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged {
            if (sizeof(TStorage) == sizeof(uint)) {
                return AddCheckedPrimitive<TStorage, uint>(first, second);
            }
            if (sizeof(TStorage) == nuint.Size) {
                return AddCheckedPrimitive<TStorage, nuint>(first, second);
            }
            if (sizeof(TStorage) == sizeof(ulong)) {
                return AddCheckedPrimitive<TStorage, ulong>(first, second);
            }
            if (sizeof(TStorage) == sizeof(System.UInt128)) {
                return AddCheckedPrimitive<TStorage, System.UInt128>(first, second);
            }
            if (sizeof(TStorage) < sizeof(uint)) {
                return AddCheckedPrimitiveWidening<TStorage, uint>(first, second);
            }
            if (sizeof(TStorage) < nuint.Size) {
                return AddCheckedPrimitiveWidening<TStorage, nuint>(first, second);
            }
            if (sizeof(TStorage) < sizeof(ulong)) {
                return AddCheckedPrimitiveWidening<TStorage, ulong>(first, second);
            }
            if (sizeof(TStorage) < sizeof(System.UInt128)) {
                return AddCheckedPrimitiveWidening<TStorage, System.UInt128>(first, second);
            }

            // Fallback (TStorage > 16 bytes): do the addition once, then verify via
            // BigInteger that the truncated result actually equals the exact sum.
            // O(n) for add/convert; only reached for very wide TStorage.
            var sum = AddUnchecked(first, second);
            if (ToBigInteger(sum) < ToBigInteger(first)) {
                throw new OverflowException();
            }
            return sum;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt<TStorage> AddCheckedPrimitive<TStorage, TUInt>(UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged
            where TUInt : unmanaged, IBinaryInteger<TUInt>, IMinMaxValue<TUInt> {
            Debug.Assert(sizeof(TStorage) == sizeof(TUInt));

            UInt<TStorage> r = default;
            ref TStorage dst = ref Unsafe.AsRef(in r._storage);

            TUInt s;
            if (0u == unchecked((uint)StructLayoutHelpers.AlignOf<TStorage>()) %
                        unchecked((uint)StructLayoutHelpers.AlignOf<TUInt>())) {
                TUInt a = Unsafe.As<TStorage, TUInt>(ref Unsafe.AsRef(in first._storage));
                TUInt b = Unsafe.As<TStorage, TUInt>(ref Unsafe.AsRef(in second._storage));
                s = checked(a + b);                 // <-- throws OverflowException
                Unsafe.As<TStorage, TUInt>(ref dst) = s;
            } else {
                TUInt a = Unsafe.ReadUnaligned<TUInt>(ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in first._storage)));
                TUInt b = Unsafe.ReadUnaligned<TUInt>(ref Unsafe.As<TStorage, byte>(ref Unsafe.AsRef(in second._storage)));
                s = checked(a + b);
                Unsafe.WriteUnaligned(ref Unsafe.As<TStorage, byte>(ref dst), s);
            }
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt<TStorage> AddCheckedPrimitiveWidening<TStorage, TUInt>(UInt<TStorage> first, UInt<TStorage> second)
            where TStorage : unmanaged
            where TUInt : unmanaged, IBinaryInteger<TUInt>, IMinMaxValue<TUInt> {
            Debug.Assert(sizeof(TStorage) < sizeof(TUInt));

            // The wide add cannot overflow (2 * (2^(8*sizeof(TStorage)) - 1) < 2^(8*sizeof(TUInt))).
            // The only failure mode is that the truncation back to TStorage loses _storage.
            TUInt a = CreateWideningInternal<TStorage, TUInt>(first._storage);
            TUInt b = CreateWideningInternal<TStorage, TUInt>(second._storage);
            TUInt s = unchecked(a + b);

            UInt<TStorage> truncated = CreateTruncating<TStorage, TUInt>(s);
            TUInt roundTrip = CreateWideningInternal<TStorage, TUInt>(truncated._storage);
            if (roundTrip != s) {
                throw new OverflowException();
            }
            return truncated;
        }

        // -----------------------------------------------------------------------------
        // Signed addition
        // -----------------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int<TStorage> AddUnchecked<TStorage>(Int<TStorage> first, Int<TStorage> second)
            where TStorage : unmanaged {
            // Two's-complement addition is bit-identical to unsigned addition.
            return Unsafe.BitCast<UInt<TStorage>, Int<TStorage>>(
                AddUnchecked(
                    Unsafe.BitCast<Int<TStorage>, UInt<TStorage>>(first),
                    Unsafe.BitCast<Int<TStorage>, UInt<TStorage>>(second)));
        }

        public static Int<TStorage> AddChecked<TStorage>(Int<TStorage> first, Int<TStorage> second)
            where TStorage : unmanaged {
            // Signed overflow is not detectable from the carry out of an unsigned add.
            // Using BigInteger here is simple and correct; the branch is rare relative
            // to the unchecked path used inside arbitrary-precision hot loops.
            var a = ToBigInteger(first);
            var b = ToBigInteger(second);
            var s = a + b;
            if (s < BigIntegerTraits<TStorage>.SignedMinValue ||
                s > BigIntegerTraits<TStorage>.SignedMaxValue) {
                throw new OverflowException();
            }
            return ToSignedUnchecked<TStorage>(s);
        }

        // -----------------------------------------------------------------------------
        // Checked conversions
        // -----------------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int<TStorage> ToSignedChecked<TStorage>(UInt<TStorage> value)
            where TStorage : unmanaged {
            if (ToBigInteger(value) > BigIntegerTraits<TStorage>.SignedMaxValue) {
                throw new OverflowException();
            }
            return ToSignedUnchecked<TStorage>(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TStorage> ToUnsignedChecked<TStorage>(Int<TStorage> value)
            where TStorage : unmanaged {
            if (BigInteger.IsNegative(ToBigInteger(value))) {
                throw new OverflowException();
            }
            return ToUnsignedUnchecked<TStorage>(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int<TStorage> ToSignedChecked<TStorage>(BigInteger value)
            where TStorage : unmanaged {
            if (!TryToSignedChecked<TStorage>(value, out var result)) {
                throw new OverflowException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TStorage> ToUnsignedChecked<TStorage>(BigInteger value)
            where TStorage : unmanaged {
            if (!TryToUnsignedChecked<TStorage>(value, out var result)) {
                throw new OverflowException();
            }
            return result;
        }
        
        #pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }

    public static partial class XIntNExtensions {



    }
    [Experimental("UoWIP")]
    public readonly partial struct Int<TStorage>
        where TStorage : unmanaged {

        internal readonly TStorage _storage;
    }

    [Experimental("UoWIP")]
    public readonly partial struct UInt<TStorage>
        where TStorage : unmanaged {

        internal readonly TStorage _storage;
    }

   
    public readonly partial struct Int<TStorage>
        where TStorage : unmanaged {
        // ---- conversions ----

        public static implicit operator BigInteger(Int<TStorage> value)
            => MultiArithmetic.ToBigInteger(value);

        public static explicit operator Int<TStorage>(BigInteger value)
            => MultiArithmetic.ToSignedUnchecked<TStorage>(value);

        public static explicit operator checked Int<TStorage>(BigInteger value)
            => MultiArithmetic.ToSignedChecked<TStorage>(value);

        public static explicit operator UInt<TStorage>(Int<TStorage> value)
            => MultiArithmetic.ToUnsignedUnchecked<TStorage>(value);

        public static explicit operator checked UInt<TStorage>(Int<TStorage> value)
            => MultiArithmetic.ToUnsignedChecked<TStorage>(value);

        // ---- arithmetic ----

        public static Int<TStorage> operator +(Int<TStorage> first, Int<TStorage> second)
            => MultiArithmetic.AddUnchecked(first, second);

        public static Int<TStorage> operator checked +(Int<TStorage> first, Int<TStorage> second)
            => MultiArithmetic.AddChecked(first, second);
    }
    public readonly partial struct UInt<TStorage>
        where TStorage : unmanaged {

        // ---- conversions ----

        public static implicit operator BigInteger(UInt<TStorage> value)
            => MultiArithmetic.ToBigInteger(value);

        public static explicit operator UInt<TStorage>(BigInteger value)
            => MultiArithmetic.ToUnsignedUnchecked<TStorage>(value);

        public static explicit operator checked UInt<TStorage>(BigInteger value)
            => MultiArithmetic.ToUnsignedChecked<TStorage>(value);

        public static explicit operator Int<TStorage>(UInt<TStorage> value)
            => MultiArithmetic.ToSignedUnchecked<TStorage>(value);

        public static explicit operator checked Int<TStorage>(UInt<TStorage> value)
            => MultiArithmetic.ToSignedChecked<TStorage>(value);

        // ---- arithmetic ----

        public static UInt<TStorage> operator +(UInt<TStorage> first, UInt<TStorage> second)
            => MultiArithmetic.AddUnchecked(first, second);

        public static UInt<TStorage> operator checked +(UInt<TStorage> first, UInt<TStorage> second)
            => MultiArithmetic.AddChecked(first, second);
    }
}
