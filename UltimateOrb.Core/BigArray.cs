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

    public abstract class BigArray {

        internal protected readonly nint _length;

        public nint Length {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (nint.Size == sizeof(int)) {
                    return _length;
                }
                var l = unchecked((Int32)_length);
                var f = unchecked((Int32)(_length >> 32));
                if (BitConverter.IsLittleEndian) {
                    return unchecked((nint)(((Int64)l << f) | ((Int64)f >>> 8)));
                } else {
                    return unchecked((nint)(((Int64)f << l) | ((Int64)l >>> 8)));
                }
            }
        }

        protected BigArray() {
            throw new NotSupportedException();
        }

        internal protected ref byte DataReference {

            get => ref MemoryMarshal.GetArrayDataReference(Unsafe.As<Array>(this));
        }

        const int ArrayLengthThreshold = 0X40000000;

        public static BigArray<T> CreateInstance<T>(nint length) {
            Debug.Assert(nint.IsPow2(ArrayLengthThreshold));
            Debug.Assert(ArrayLengthThreshold <= Array.MaxLength);
            if (nint.Size == sizeof(int)) {
                return Unsafe.As<BigArray<T>>(new T[length]);
            }
            if (length <= ArrayLengthThreshold) {
                return Unsafe.As<BigArray<T>>(new T[unchecked((int)length)]);
            }
            var m = (ArrayLengthThreshold - 1 + length) / ArrayLengthThreshold;
            if (m <= 2) {
                var a = new ArrayBlock2<T>[unchecked((int)((2 - 1 + (nuint)length) / 2))];
                var b = 1 | (((2 - 1) & length) << 8);
                var c = a.Length;
                var d = Unsafe.As<BigArray<T>>(a);
                Unsafe.AsRef(in d._length) = (b << 32) | c;
                return d;

            } else if (m <= 4) {
                var a = new ArrayBlock4<T>[unchecked((int)((4 - 1 + (nuint)length) / 4))];
                var b = 2 | (((4 - 1) & length) << 8);
                var c = a.Length;
                var d = Unsafe.As<BigArray<T>>(a);
                Unsafe.AsRef(in d._length) = (b << 32) | c;
                return d;
            } else if (m <= 8) {
                var a = new ArrayBlock8<T>[unchecked((int)((8 - 1 + (nuint)length) / 8))];
                var b = 3 | (((8 - 1) & length) << 8);
                var c = a.Length;
                var d = Unsafe.As<BigArray<T>>(a);
                Unsafe.AsRef(in d._length) = (b << 32) | c;
                return d;
            } else if (m <= 16) {
                var a = new ArrayBlock16<T>[unchecked((int)((16 - 1 + (nuint)length) / 16))];
                var b = 4 | (((16 - 1) & length) << 8);
                var c = a.Length;
                var d = Unsafe.As<BigArray<T>>(a);
                Unsafe.AsRef(in d._length) = (b << 32) | c;
                return d;
            } else if (m <= 32) {

            } else if (m <= 64) {

            } else if (m <= 128) {

            } else if (m <= 256) {

            } else if (m <= 512) {

            } else if (m <= 1024) {

            } else if (m <= 2048) {

            } else if (m <= 4096) {

            } else if (m <= 8192) {

            } else if (m <= 16384) {

            } else if (m <= 32768) {

            } else if (m <= 65536) {

            } else if (m <= 131072) {

            } else if (m <= 262144) {

            } else if (m <= 524288) {

            } else if (m <= 524288) {

            } else if (m <= 1048576) {

            }
            _ = new T[length]; // throws
            throw null!;
        }
    }

    public sealed class BigArray<T> : BigArray {

        internal protected ref T DataReference {

            get => ref Unsafe.As<byte, T>(ref base.DataReference);
        }

        private BigArray() {
            throw new NotSupportedException();
        }

        public ref T this[nint index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(Length.ToUnsignedUnchecked(), index.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref DataReference, index);
            }
        }
    }
}
