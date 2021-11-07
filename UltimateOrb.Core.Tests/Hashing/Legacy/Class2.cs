using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;


namespace UltimateOrb.Core.Tests.Hashing.Legacy {
    using static BitOperations;

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
            // round 1 left rotates
            const int s11 = 7;
            const int s12 = 12;
            const int s13 = 17;
            const int s14 = 22;

            // round 2 left rotates
            const int s21 = 5;
            const int s22 = 9;
            const int s23 = 14;
            const int s24 = 20;

            // round 3 left rotates
            const int s31 = 4;
            const int s32 = 11;
            const int s33 = 16;
            const int s34 = 23;

            // round 4 left rotates
            const int s41 = 6;
            const int s42 = 10;
            const int s43 = 15;
            const int s44 = 21;

            static UInt32 F(UInt32 u, UInt32 v, UInt32 w) => (u & v) | (~u & w);

            static UInt32 G(UInt32 u, UInt32 v, UInt32 w) => (u & w) | (v & ~w);

            static UInt32 H(UInt32 u, UInt32 v, UInt32 w) => u ^ v ^ w;

            static UInt32 K(UInt32 u, UInt32 v, UInt32 w) => v ^ (u | ~w);

            unchecked {
                Debug.Assert(0 == source.Length % 16);
                var a = hashCode.GetElement(0);
                var b = hashCode.GetElement(1);
                var c = hashCode.GetElement(2);
                var d = hashCode.GetElement(3);
                for (; 16 <= source.Length; source = source.Slice(16)) {
                    // Round 1 - F cycle, 16 times.
                    a = RotateLeft((a + F(b, c, d) + source[0] + 0Xd76aa478U), s11) + b;
                    d = RotateLeft((d + F(a, b, c) + source[1] + 0Xe8c7b756U), s12) + a;
                    c = RotateLeft((c + F(d, a, b) + source[2] + 0X242070dbU), s13) + d;
                    b = RotateLeft((b + F(c, d, a) + source[3] + 0Xc1bdceeeU), s14) + c;
                    a = RotateLeft((a + F(b, c, d) + source[4] + 0Xf57c0fafU), s11) + b;
                    d = RotateLeft((d + F(a, b, c) + source[5] + 0X4787c62aU), s12) + a;
                    c = RotateLeft((c + F(d, a, b) + source[6] + 0Xa8304613U), s13) + d;
                    b = RotateLeft((b + F(c, d, a) + source[7] + 0Xfd469501U), s14) + c;
                    a = RotateLeft((a + F(b, c, d) + source[8] + 0X698098d8U), s11) + b;
                    d = RotateLeft((d + F(a, b, c) + source[9] + 0X8b44f7afU), s12) + a;
                    c = RotateLeft((c + F(d, a, b) + source[10] + 0Xffff5bb1U), s13) + d;
                    b = RotateLeft((b + F(c, d, a) + source[11] + 0X895cd7beU), s14) + c;
                    a = RotateLeft((a + F(b, c, d) + source[12] + 0X6b901122U), s11) + b;
                    d = RotateLeft((d + F(a, b, c) + source[13] + 0Xfd987193U), s12) + a;
                    c = RotateLeft((c + F(d, a, b) + source[14] + 0Xa679438eU), s13) + d;
                    b = RotateLeft((b + F(c, d, a) + source[15] + 0X49b40821U), s14) + c;

                    // Round 2 - G cycle, 16 times.
                    a = RotateLeft((a + G(b, c, d) + source[1] + 0Xf61e2562U), s21) + b;
                    d = RotateLeft((d + G(a, b, c) + source[6] + 0Xc040b340U), s22) + a;
                    c = RotateLeft((c + G(d, a, b) + source[11] + 0X265e5a51U), s23) + d;
                    b = RotateLeft((b + G(c, d, a) + source[0] + 0Xe9b6c7aaU), s24) + c;
                    a = RotateLeft((a + G(b, c, d) + source[5] + 0Xd62f105dU), s21) + b;
                    d = RotateLeft((d + G(a, b, c) + source[10] + 0X02441453U), s22) + a;
                    c = RotateLeft((c + G(d, a, b) + source[15] + 0Xd8a1e681U), s23) + d;
                    b = RotateLeft((b + G(c, d, a) + source[4] + 0Xe7d3fbc8U), s24) + c;
                    a = RotateLeft((a + G(b, c, d) + source[9] + 0X21e1cde6U), s21) + b;
                    d = RotateLeft((d + G(a, b, c) + source[14] + 0Xc33707d6U), s22) + a;
                    c = RotateLeft((c + G(d, a, b) + source[3] + 0Xf4d50d87U), s23) + d;
                    b = RotateLeft((b + G(c, d, a) + source[8] + 0X455a14edU), s24) + c;
                    a = RotateLeft((a + G(b, c, d) + source[13] + 0Xa9e3e905U), s21) + b;
                    d = RotateLeft((d + G(a, b, c) + source[2] + 0Xfcefa3f8U), s22) + a;
                    c = RotateLeft((c + G(d, a, b) + source[7] + 0X676f02d9U), s23) + d;
                    b = RotateLeft((b + G(c, d, a) + source[12] + 0X8d2a4c8aU), s24) + c;

                    // Round 3 - H cycle, 16 times.
                    a = RotateLeft((a + H(b, c, d) + source[5] + 0Xfffa3942U), s31) + b;
                    d = RotateLeft((d + H(a, b, c) + source[8] + 0X8771f681U), s32) + a;
                    c = RotateLeft((c + H(d, a, b) + source[11] + 0X6d9d6122U), s33) + d;
                    b = RotateLeft((b + H(c, d, a) + source[14] + 0Xfde5380cU), s34) + c;
                    a = RotateLeft((a + H(b, c, d) + source[1] + 0Xa4beea44U), s31) + b;
                    d = RotateLeft((d + H(a, b, c) + source[4] + 0X4bdecfa9U), s32) + a;
                    c = RotateLeft((c + H(d, a, b) + source[7] + 0Xf6bb4b60U), s33) + d;
                    b = RotateLeft((b + H(c, d, a) + source[10] + 0Xbebfbc70U), s34) + c;
                    a = RotateLeft((a + H(b, c, d) + source[13] + 0X289b7ec6U), s31) + b;
                    d = RotateLeft((d + H(a, b, c) + source[0] + 0Xeaa127faU), s32) + a;
                    c = RotateLeft((c + H(d, a, b) + source[3] + 0Xd4ef3085U), s33) + d;
                    b = RotateLeft((b + H(c, d, a) + source[6] + 0X04881d05U), s34) + c;
                    a = RotateLeft((a + H(b, c, d) + source[9] + 0Xd9d4d039U), s31) + b;
                    d = RotateLeft((d + H(a, b, c) + source[12] + 0Xe6db99e5U), s32) + a;
                    c = RotateLeft((c + H(d, a, b) + source[15] + 0X1fa27cf8U), s33) + d;
                    b = RotateLeft((b + H(c, d, a) + source[2] + 0Xc4ac5665U), s34) + c;

                    // Round 4 - K cycle, 16 times.
                    a = RotateLeft((a + K(b, c, d) + source[0] + 0Xf4292244U), s41) + b;
                    d = RotateLeft((d + K(a, b, c) + source[7] + 0X432aff97U), s42) + a;
                    c = RotateLeft((c + K(d, a, b) + source[14] + 0Xab9423a7U), s43) + d;
                    b = RotateLeft((b + K(c, d, a) + source[5] + 0Xfc93a039U), s44) + c;
                    a = RotateLeft((a + K(b, c, d) + source[12] + 0X655b59c3U), s41) + b;
                    d = RotateLeft((d + K(a, b, c) + source[3] + 0X8f0ccc92U), s42) + a;
                    c = RotateLeft((c + K(d, a, b) + source[10] + 0Xffeff47dU), s43) + d;
                    b = RotateLeft((b + K(c, d, a) + source[1] + 0X85845dd1U), s44) + c;
                    a = RotateLeft((a + K(b, c, d) + source[8] + 0X6fa87e4fU), s41) + b;
                    d = RotateLeft((d + K(a, b, c) + source[15] + 0Xfe2ce6e0U), s42) + a;
                    c = RotateLeft((c + K(d, a, b) + source[6] + 0Xa3014314U), s43) + d;
                    b = RotateLeft((b + K(c, d, a) + source[13] + 0X4e0811a1U), s44) + c;
                    a = RotateLeft((a + K(b, c, d) + source[4] + 0Xf7537e82U), s41) + b;
                    d = RotateLeft((d + K(a, b, c) + source[11] + 0Xbd3af235U), s42) + a;
                    c = RotateLeft((c + K(d, a, b) + source[2] + 0X2ad7d2bbU), s43) + d;
                    b = RotateLeft((b + K(c, d, a) + source[9] + 0Xeb86d391U), s44) + c;

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
        private Vector128<uint> ComputeFinalHashCode_Rest(Span<byte> buffer, Vector128<UInt32> hashCode) {
            ComputeFinalHashCode_BitLength(buffer);
            return ComputeHashCode(buffer, hashCode);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ComputeFinalHashCode_BitLength(Span<byte> buffer) {
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(56), unchecked((UInt32)(_BytePosition * 8))); // 8 bits
            BinaryPrimitives.WriteUInt32LittleEndian(buffer.Slice(60), checked((UInt32)(_BytePosition >> 29)));
        }

        public void ComputeHashCode(ReadOnlySpan<byte> source) {
            var buffer = _BufferSpan;
            var hashCode = _HashCode;
            ulong bytePosition = _BytePosition;
            var r = unchecked((int)(63 & bytePosition));
            var sourceRest = source;
            if (0 != r) {
                var p = unchecked(64 - r);
                if (source.Length <= p) {
                    source.CopyTo(buffer.Slice(r));
                    if (source.Length == p) {
                        _HashCode = ComputeHashCode(buffer, hashCode);
                    }
                    _BytePosition = bytePosition + unchecked((uint)source.Length);
                    return;
                } else {
                    source.Slice(0, p).CopyTo(buffer.Slice(r));
                    sourceRest = source.Slice(p);
                    hashCode = ComputeHashCode(buffer, hashCode);
                }
            }
            var b = ~63 & sourceRest.Length;
            _HashCode = ComputeHashCode(sourceRest.Slice(0, b), hashCode);
            sourceRest.Slice(b).CopyTo(buffer);
            _BytePosition = bytePosition + unchecked((uint)source.Length);
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
