using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb {

    struct ArrayBlock2<T> {
        T Lower;
        T Upper;
    }
    struct ArrayBlock4<T> {
        ArrayBlock2<T> Lower;
        ArrayBlock2<T> Upper;
    }
    struct ArrayBlock8<T> {
        ArrayBlock4<T> Lower;
        ArrayBlock4<T> Upper;
    }
    struct ArrayBlock16<T> {
        ArrayBlock8<T> Lower;
        ArrayBlock8<T> Upper;
    }
    struct ArrayBlock32<T> {
        ArrayBlock16<T> Lower;
        ArrayBlock16<T> Upper;
    }
    struct ArrayBlock64<T> {
        ArrayBlock32<T> Lower;
        ArrayBlock32<T> Upper;
    }
    struct ArrayBlock128<T> {
        ArrayBlock64<T> Lower;
        ArrayBlock64<T> Upper;
    }
    struct ArrayBlock256<T> {
        ArrayBlock128<T> Lower;
        ArrayBlock128<T> Upper;
    }
    struct ArrayBlock512<T> {
        ArrayBlock256<T> Lower;
        ArrayBlock256<T> Upper;
    }
    struct ArrayBlock1024<T> {
        ArrayBlock512<T> Lower;
        ArrayBlock512<T> Upper;
    }
    struct ArrayBlock2048<T> {
        ArrayBlock1024<T> Lower;
        ArrayBlock1024<T> Upper;
    }
    struct ArrayBlock4096<T> {
        ArrayBlock2048<T> Lower;
        ArrayBlock2048<T> Upper;
    }
    struct ArrayBlock8192<T> {
        ArrayBlock4096<T> Lower;
        ArrayBlock4096<T> Upper;
    }
    struct ArrayBlock16384<T> {
        ArrayBlock8192<T> Lower;
        ArrayBlock8192<T> Upper;
    }
    struct ArrayBlock32768<T> {
        ArrayBlock16384<T> Lower;
        ArrayBlock16384<T> Upper;
    }
    struct ArrayBlock65536<T> {
        ArrayBlock32768<T> Lower;
        ArrayBlock32768<T> Upper;
    }
    struct ArrayBlock131072<T> {
        ArrayBlock65536<T> Lower;
        ArrayBlock65536<T> Upper;
    }
    struct ArrayBlock262144<T> {
        ArrayBlock131072<T> Lower;
        ArrayBlock131072<T> Upper;
    }
    struct ArrayBlock524288<T> {
        ArrayBlock262144<T> Lower;
        ArrayBlock262144<T> Upper;
    }
    struct ArrayBlock1048576<T> {
        ArrayBlock524288<T> Lower;
        ArrayBlock524288<T> Upper;
    }

    public abstract class BigArray {

        internal protected readonly nint _length;

        public nint Length {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (nint.Size == sizeof(int)) {
                    return _length;
                }
                var blockCount = unchecked((Int32)_length);
                var b = unchecked((Int32)(_length >> 32));
                if (!BitConverter.IsLittleEndian) {
                    (blockCount, b) = (b, blockCount);
                }
                var log2N = b & 0x3F;
                var difference = b >> 8;
                return unchecked(((nint)blockCount << log2N) - difference);
            }
        }

        protected BigArray() {
            throw new NotSupportedException();
        }

        internal protected ref byte DataReference {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref MemoryMarshal.GetArrayDataReference(Unsafe.As<Array>(this));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BigArray<T> DangerousSetIsPinned<T>(BigArray<T> array, bool pinned) {
            var f = pinned ? FlagPinned : 0;
            Unsafe.AsRef(in array._length) |= BitConverter.IsLittleEndian
                ? ((nint)f << 32)
                : ((nint)f << 0);
            return array;
        }

        const int ArrayLengthThreshold = 0X40000000;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigArray<T> CreateInstance<T>(nint length, bool pinned = false) {
            Debug.Assert(nint.IsPow2(ArrayLengthThreshold));
            Debug.Assert(ArrayLengthThreshold <= Array.MaxLength);
            if (nint.Size == sizeof(int) || length <= ArrayLengthThreshold) {
                return DangerousSetIsPinned(Unsafe.As<BigArray<T>>(GC.AllocateArray<T>(unchecked((int)length))), pinned);
            }
            return CreateBlockArray<T>(length, pinned);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigArray<T> CreateInstanceUninitialized<T>(nint length, bool pinned = false) {
            Debug.Assert(nint.IsPow2(ArrayLengthThreshold));
            Debug.Assert(ArrayLengthThreshold <= Array.MaxLength);
            if (nint.Size == sizeof(int) || length <= ArrayLengthThreshold) {
                return DangerousSetIsPinned(Unsafe.As<BigArray<T>>(GC.AllocateUninitializedArray<T>(unchecked((int)length))), pinned);
            }
            return CreateUninitializedBlockArray<T>(length, pinned);
        }

        private static BigArray<T> CreateBlockArray<T>(nint length, bool pinned) {
            var m = (ArrayLengthThreshold.ToUnsignedUnchecked() - 1 + length.ToUnsignedUnchecked()) / ArrayLengthThreshold.ToUnsignedUnchecked();
            if (m <= 2) {
                return CreateBlockArray<T, ArrayBlock2<T>>(1, length, pinned);
            } else if (m <= 4) {
                return CreateBlockArray<T, ArrayBlock4<T>>(2, length, pinned);
            } else if (m <= 8) {
                return CreateBlockArray<T, ArrayBlock8<T>>(3, length, pinned);
            } else if (m <= 16) {
                return CreateBlockArray<T, ArrayBlock16<T>>(4, length, pinned);
            } else if (m <= 32) {
                return CreateBlockArray<T, ArrayBlock32<T>>(5, length, pinned);
            } else if (m <= 64) {
                return CreateBlockArray<T, ArrayBlock64<T>>(6, length, pinned);
            } else if (m <= 128) {
                return CreateBlockArray<T, ArrayBlock128<T>>(7, length, pinned);
            } else if (m <= 256) {
                return CreateBlockArray<T, ArrayBlock256<T>>(8, length, pinned);
            } else if (m <= 512) {
                return CreateBlockArray<T, ArrayBlock512<T>>(9, length, pinned);
            } else if (m <= 1024) {
                return CreateBlockArray<T, ArrayBlock1024<T>>(10, length, pinned);
            } else if (m <= 2048) {
                return CreateBlockArray<T, ArrayBlock2048<T>>(11, length, pinned);
            } else if (m <= 4096) {
                return CreateBlockArray<T, ArrayBlock4096<T>>(12, length, pinned);
            } else if (m <= 8192) {
                return CreateBlockArray<T, ArrayBlock8192<T>>(13, length, pinned);
            } else if (m <= 16384) {
                return CreateBlockArray<T, ArrayBlock16384<T>>(14, length, pinned);
            } else if (m <= 32768) {
                return CreateBlockArray<T, ArrayBlock32768<T>>(15, length, pinned);
            } else if (m <= 65536) {
                return CreateBlockArray<T, ArrayBlock65536<T>>(16, length, pinned);
            } else if (m <= 131072) {
                return CreateBlockArray<T, ArrayBlock131072<T>>(17, length, pinned);
            } else if (m <= 262144) {
                return CreateBlockArray<T, ArrayBlock262144<T>>(18, length, pinned);
            } else if (m <= 524288) {
                return CreateBlockArray<T, ArrayBlock524288<T>>(19, length, pinned);
            } else if (m <= 1048576) {
                return CreateBlockArray<T, ArrayBlock1048576<T>>(20, length, pinned);
            }
            _ = new T[length]; // throws
            throw null!;
        }

        private static BigArray<T> CreateUninitializedBlockArray<T>(nint length, bool pinned) {
            var m = (ArrayLengthThreshold.ToUnsignedUnchecked() - 1 + length.ToUnsignedUnchecked()) / ArrayLengthThreshold.ToUnsignedUnchecked();
            if (m <= 2) {
                return CreateUninitializedBlockArray<T, ArrayBlock2<T>>(1, length, pinned);
            } else if (m <= 4) {
                return CreateUninitializedBlockArray<T, ArrayBlock4<T>>(2, length, pinned);
            } else if (m <= 8) {
                return CreateUninitializedBlockArray<T, ArrayBlock8<T>>(3, length, pinned);
            } else if (m <= 16) {
                return CreateUninitializedBlockArray<T, ArrayBlock16<T>>(4, length, pinned);
            } else if (m <= 32) {
                return CreateUninitializedBlockArray<T, ArrayBlock32<T>>(5, length, pinned);
            } else if (m <= 64) {
                return CreateUninitializedBlockArray<T, ArrayBlock64<T>>(6, length, pinned);
            } else if (m <= 128) {
                return CreateUninitializedBlockArray<T, ArrayBlock128<T>>(7, length, pinned);
            } else if (m <= 256) {
                return CreateUninitializedBlockArray<T, ArrayBlock256<T>>(8, length, pinned);
            } else if (m <= 512) {
                return CreateUninitializedBlockArray<T, ArrayBlock512<T>>(9, length, pinned);
            } else if (m <= 1024) {
                return CreateUninitializedBlockArray<T, ArrayBlock1024<T>>(10, length, pinned);
            } else if (m <= 2048) {
                return CreateUninitializedBlockArray<T, ArrayBlock2048<T>>(11, length, pinned);
            } else if (m <= 4096) {
                return CreateUninitializedBlockArray<T, ArrayBlock4096<T>>(12, length, pinned);
            } else if (m <= 8192) {
                return CreateUninitializedBlockArray<T, ArrayBlock8192<T>>(13, length, pinned);
            } else if (m <= 16384) {
                return CreateUninitializedBlockArray<T, ArrayBlock16384<T>>(14, length, pinned);
            } else if (m <= 32768) {
                return CreateUninitializedBlockArray<T, ArrayBlock32768<T>>(15, length, pinned);
            } else if (m <= 65536) {
                return CreateUninitializedBlockArray<T, ArrayBlock65536<T>>(16, length, pinned);
            } else if (m <= 131072) {
                return CreateUninitializedBlockArray<T, ArrayBlock131072<T>>(17, length, pinned);
            } else if (m <= 262144) {
                return CreateUninitializedBlockArray<T, ArrayBlock262144<T>>(18, length, pinned);
            } else if (m <= 524288) {
                return CreateUninitializedBlockArray<T, ArrayBlock524288<T>>(19, length, pinned);
            } else if (m <= 1048576) {
                return CreateUninitializedBlockArray<T, ArrayBlock1048576<T>>(20, length, pinned);
            }
            _ = new T[length]; // throws
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BigArray<T> CreateBlockArray<T, TBlock>(int log2BlockSize, nint length, bool pinned) {
            var blockSize = Unsafe.SizeOf<TBlock>().ToUnsignedUnchecked() / Unsafe.SizeOf<T>().ToUnsignedUnchecked();
            var mask = blockSize - 1;
            var a = GC.AllocateArray<TBlock>(unchecked((int)((mask + (nuint)length) / blockSize)), pinned);
            var b = log2BlockSize | (pinned ? FlagPinned : 0) | (((int)mask & unchecked(-(int)length)) << 8);
            var c = a.Length;
            var d = Unsafe.As<BigArray<T>>(a);
            Unsafe.AsRef(in d._length) = BitConverter.IsLittleEndian
                ? ((nint)b << 32) | c
                : ((nint)c << 32) | b;
            return d;
        }

        const int FlagPinned = 128;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static BigArray<T> CreateUninitializedBlockArray<T, TBlock>(int log2BlockSize, nint length, bool pinned) {
            var blockSize = Unsafe.SizeOf<TBlock>().ToUnsignedUnchecked() / Unsafe.SizeOf<T>().ToUnsignedUnchecked();
            var mask = blockSize - 1;
            var a = GC.AllocateUninitializedArray<TBlock>(unchecked((int)((mask + (nuint)length) / blockSize)), pinned);
            var b = log2BlockSize | (pinned ? FlagPinned : 0) | (((int)mask & unchecked(-(int)length)) << 8);
            var c = a.Length;
            var d = Unsafe.As<BigArray<T>>(a);
            Unsafe.AsRef(in d._length) = BitConverter.IsLittleEndian
                ? ((nint)b << 32) | c
                : ((nint)c << 32) | b;
            return d;
        }

        internal protected bool IsPinned {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var blockCount = unchecked((Int32)_length);
                var b = unchecked((Int32)(_length >> 32));
                if (!BitConverter.IsLittleEndian) {
                    (blockCount, b) = (b, blockCount);
                }
                return 0 != (FlagPinned & b);
            }
        }
    }

    public sealed class BigArray<T> : BigArray {

        internal protected ref T DataReference {

            get => ref Unsafe.As<byte, T>(ref base.DataReference);
        }

        private BigArray() {
            throw new NotSupportedException();
        }

        public ref T this[int index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ThrowHelper.ThrowOnLessThanOrEqual(Length.ToUnsignedUnchecked(), index.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref DataReference, index);
            }
        }

        public ref T this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ThrowHelper.ThrowOnLessThanOrEqual(Length.ToUnsignedUnchecked(), index.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref DataReference, (nuint)index);
            }
        }

        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ThrowHelper.ThrowOnLessThanOrEqual(Length.ToUnsignedUnchecked(), index.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref DataReference, index);
            }
        }
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                ThrowHelper.ThrowOnLessThanOrEqual(Length.ToUnsignedUnchecked(), index.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref DataReference, index);
            }
        }

        public ref T this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nint)index)];
        }

        public ref T this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nuint)index)];
        }
    }
}

namespace UltimateOrb {

    public static partial class BigArrayExtensions {

        extension(GC) {

            /// <inheritdoc cref="GC.AllocateArray{T}(int, bool)"/>
            public static BigArray<T> AllocateUninitializedBigArray<T>(nint length, bool pinned = false) {
                return BigArray.CreateInstanceUninitialized<T>(length, pinned);
            }

            /// <inheritdoc cref="GC.AllocateArray{T}(int, bool)"/>
            public static BigArray<T> AllocateBigArray<T>(nint length, bool pinned = false) {
                return BigArray.CreateInstance<T>(length, pinned);
            }
        }
    }
}