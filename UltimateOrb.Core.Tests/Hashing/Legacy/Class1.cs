using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;

namespace UltimateOrb.Core.Tests.Hashing.Legacy.E {

    internal interface IDataType<T> {
    }

    internal interface IConstantDataType<T> : IDataType<T> {

        T Value {

            get;
        }
    }

    internal struct TrueT : IConstantDataType<bool> {

        public bool Value {

            get => true;
        }
    }

    internal struct FalseT : IConstantDataType<bool> {

        public bool Value {
            get => false;
        }
    }

    internal unsafe struct MD5HashCore {

        private static readonly UInt32[] k = new UInt32[64] {
            0Xd76aa478U, 0Xe8c7b756U, 0X242070dbU, 0Xc1bdceeeU,
            0Xf57c0fafU, 0X4787c62aU, 0Xa8304613U, 0Xfd469501U,
            0X698098d8U, 0X8b44f7afU, 0Xffff5bb1U, 0X895cd7beU,
            0X6b901122U, 0Xfd987193U, 0Xa679438eU, 0X49b40821U,
            0Xf61e2562U, 0Xc040b340U, 0X265e5a51U, 0Xe9b6c7aaU,
            0Xd62f105dU, 0X02441453U, 0Xd8a1e681U, 0Xe7d3fbc8U,
            0X21e1cde6U, 0Xc33707d6U, 0Xf4d50d87U, 0X455a14edU,
            0Xa9e3e905U, 0Xfcefa3f8U, 0X676f02d9U, 0X8d2a4c8aU,
            0Xfffa3942U, 0X8771f681U, 0X6d9d6122U, 0Xfde5380cU,
            0Xa4beea44U, 0X4bdecfa9U, 0Xf6bb4b60U, 0Xbebfbc70U,
            0X289b7ec6U, 0Xeaa127faU, 0Xd4ef3085U, 0X04881d05U,
            0Xd9d4d039U, 0Xe6db99e5U, 0X1fa27cf8U, 0Xc4ac5665U,
            0Xf4292244U, 0X432aff97U, 0Xab9423a7U, 0Xfc93a039U,
            0X655b59c3U, 0X8f0ccc92U, 0Xffeff47dU, 0X85845dd1U,
            0X6fa87e4fU, 0Xfe2ce6e0U, 0Xa3014314U, 0X4e0811a1U,
            0Xf7537e82U, 0Xbd3af235U, 0X2ad7d2bbU, 0Xeb86d391U,
        };
        /*
        // 0~9 0a 0b 0c 0d 0e 0f 10 11 12 13 14 15 16 17 18 19 1a 1b 1c 1d 1e 1f
        //     10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31
        // 0X16110c07
        // 0X140e0905
        // 0X17100b04
        // 0X150f0a06
        private static readonly int[] r = new int[64] {
            7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22, 7, 12, 17, 22,
            5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20, 5,  9, 14, 20,
            4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23, 4, 11, 16, 23,
            6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21, 6, 10, 15, 21
        };
        */

        private Vector128<UInt32> _HashCode;

        private fixed UInt32 _Buffer[16];

        private Span<byte> _BufferSpan {

            get {
                fixed (UInt32* p = &_Buffer[0]) {
                    return new Span<byte>(p, 64);
                }
            }
        }

        private UInt64 _BytePosition;

        private MD5HashCore(int ignored = default) {
            _HashCode = Vector128.Create(0X67452301U, 0Xefcdab89U, 0X98badcfeU, 0X10325476U);
            _BytePosition = 0;
        }


        private static readonly bool IsLittleEndianness = GetIsLittleEndianness();

        private static bool GetIsLittleEndianness() {
            var buffer = (stackalloc byte[sizeof(UInt32)]);
            for (int i = 0; buffer.Length > i; ++i) {
                buffer[i] = (byte)i;
            }
            var buffer2 = MemoryMarshal.Cast<byte, UInt32>(buffer);
            var result = (0X03020100U == buffer2[0]);
            if (!result) {
                if (0X00010203U != buffer2[0]) {
                    throw new PlatformNotSupportedException("Platform endianness not supported.");
                }
            }
            return result;
        }


        private static Vector128<UInt32> ComputeHashCode(ReadOnlySpan<byte> source, Vector128<UInt32> hashCode) {
            Debug.Assert(0 == source.Length % 64);
            if (IsLittleEndianness) {
                return ComputeHashCode<FalseT>(MemoryMarshal.Cast<byte, UInt32>(source), hashCode);
            } else {
                return ComputeHashCode<TrueT>(MemoryMarshal.Cast<byte, UInt32>(source), hashCode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt32 ReadWithEndianness<TReverseEndianness>(UInt32 v)
            where TReverseEndianness : struct, IConstantDataType<bool> {
            return typeof(TReverseEndianness) == typeof(TrueT) ? BinaryPrimitives.ReverseEndianness(v) : v;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static Vector128<UInt32> ComputeHashCode<TReverseEndianness>(ReadOnlySpan<UInt32> source, Vector128<UInt32> hashCode)
            where TReverseEndianness : struct, IConstantDataType<bool> {
            unchecked {
                Debug.Assert(0 == source.Length % 16);
                var a = hashCode.GetElement(0);
                var b = hashCode.GetElement(1);
                var c = hashCode.GetElement(2);
                var d = hashCode.GetElement(3);
                for (; 16 <= source.Length; source = source.Slice(16)) {
                    {
                        var g = -1;
                        for (var i = 0; 16 > i; ++i) {
                            var f = (b & c) | ((~b) & d);
                            g += 1;
                            {
                                f += a + k[i] + ReadWithEndianness<TReverseEndianness>(source[g]);
                                a = d;
                                d = c;
                                c = b;
                                b += BitOperations.RotateLeft(f, 0X16110c07 >> ((3 & i) << 3));
                            }
                        }
                    }
                    {
                        var g = -4;
                        for (var i = 0; 16 > i; ++i) {
                            var f = (d & b) | ((~d) & c);
                            g += 5;
                            {
                                f += a + k[16 + i] + ReadWithEndianness<TReverseEndianness>(source[15 & g]);
                                a = d;
                                d = c;
                                c = b;
                                b += BitOperations.RotateLeft(f, 0X17100b04 >> ((3 & i) << 3));
                            }
                        }
                    }
                    {
                        var g = 2;
                        for (var i = 0; 16 > i; ++i) {
                            var f = b ^ c ^ d;
                            g += 3;
                            {
                                f += a + k[32 + i] + ReadWithEndianness<TReverseEndianness>(source[15 & g]);
                                a = d;
                                d = c;
                                c = b;
                                b += BitOperations.RotateLeft(f, 0X140e0905 >> ((3 & i) << 3));
                            }
                        }
                    }
                    {
                        var g = -7;
                        for (var i = 0; 16 > i; ++i) {
                            var f = c ^ (b | (~d));
                            g += 7;
                            {
                                f += a + k[48 + i] + ReadWithEndianness<TReverseEndianness>(source[15 & g]);
                                a = d;
                                d = c;
                                c = b;
                                b += BitOperations.RotateLeft(f, 0X150f0a06 >> ((3 & i) << 3));
                            }
                        }
                    }
                    a += hashCode.GetElement(0);
                    b += hashCode.GetElement(1);
                    c += hashCode.GetElement(2);
                    d += hashCode.GetElement(3);
                    hashCode = Vector128.Create(a, b, c, d);
                }
                return hashCode;
            }
        }

        public Vector128<UInt32> ComputeHashCodeFinal() {
            //  Padding:
            //      {source_bits} 1 0 0 0 ... {bit_length} |
            //                  # of 0 >= 0      64 bits   | <== next 512-bit boundary
            Vector128<UInt32> hashCode = _HashCode;
            var p = unchecked((int)(_BytePosition % 64));
            var buffer = _BufferSpan;
            buffer[p++] = (byte)0X80U;
            if (p <= 56) {
                buffer.Slice(p, 56 - p).Clear();
                ComputeFinalHashCode_BitLength(buffer);
                return ComputeHashCode(buffer, hashCode);
            }
            {
                for (; 64 > p;) {
                    buffer[p++] = (byte)0X00U;
                }
                hashCode = ComputeHashCode(buffer, hashCode);
                var buffer2 = (stackalloc byte[64]);
                return ComputeFinalHashCode_Rest(buffer2, hashCode);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly Vector128<uint> ComputeFinalHashCode_Rest(Span<byte> buffer, Vector128<uint> hashCode) {
            ComputeFinalHashCode_BitLength(buffer);
            return ComputeHashCode(buffer, hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void ComputeFinalHashCode_BitLength(Span<byte> buffer2) {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer2.Slice(56), unchecked((UInt32)(_BytePosition * 8))); // 8 bits
            BinaryPrimitives.WriteUInt32LittleEndian(buffer2.Slice(60), checked((UInt32)(_BytePosition >> 29)));
        }

        public void ComputeHashCode(ReadOnlySpan<byte> source) {
            var buffer = _BufferSpan;
            var hashCode = _HashCode;
            var r = unchecked((int)(63 & _BytePosition));
            if (0 != r) {
                var p = unchecked(64 - r);
                if (source.Length <= p) {
                    source.CopyTo(buffer.Slice(r));
                    _BytePosition += unchecked((uint)source.Length);
                    return;
                } else {
                    source.Slice(0, p).CopyTo(buffer.Slice(r));
                    source = source.Slice(p);
                    hashCode = ComputeHashCode(_BufferSpan, hashCode);
                }
            }
            var b = ~63 & source.Length;
            _HashCode = ComputeHashCode(source.Slice(0, b), hashCode);
            source.Slice(b).CopyTo(buffer);
            _BytePosition += unchecked((uint)source.Length);
        }

        public static MD5HashCore Create() {
            return new MD5HashCore(default(int));
        }
    }

    public class MD5Managed : HashAlgorithm {

        MD5HashCore _Core;

        protected MD5Managed() : base() {
            HashSizeValue = 128;
            // HashValue: Leave it to HashAlgorithm.
            // State: Not used.
            Initialize();
        }

        public static MD5Managed Create() {
            return new MD5Managed();
        }

        public override void Initialize() {
            _Core = MD5HashCore.Create();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            HashCore0(array.AsSpan(ibStart, cbSize));
        }

        public override bool CanReuseTransform => base.CanReuseTransform;

        private void HashCore0(ReadOnlySpan<byte> span) {
            _Core.ComputeHashCode(span);
        }

        public override bool CanTransformMultipleBlocks => true;

        public override int HashSize => 128;

        public override int InputBlockSize => 64;

        public override int OutputBlockSize => 64;

        protected override void HashCore(ReadOnlySpan<byte> source) {
            HashCore0(source);
        }

        public override byte[] Hash => HashFinal();

        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten) {
            var h = _Core.ComputeHashCodeFinal();
            if (BinaryPrimitives.TryWriteUInt32LittleEndian(destination.Slice(12), h.GetElement(3))) {
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination.Slice(8), h.GetElement(2));
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination.Slice(4), h.GetElement(1));
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination.Slice(0), h.GetElement(0));
                bytesWritten = 16;
                return true;
            }
            bytesWritten = 0;
            return false;
        }

        protected override byte[] HashFinal() {
            var hashCode = new byte[16];
            if (TryHashFinal(hashCode, out var bytesWritten)) {
                Debug.Assert(hashCode.Length == bytesWritten);
            } else {
                Debug.Assert(false);
            }
            return hashCode;
        }
    }
}
