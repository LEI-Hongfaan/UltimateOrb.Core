using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;

namespace UltimateOrb.Hashing.Legacy {

    public class MD5Managed : HashAlgorithm {

        MD5HashCore _Core;

        protected MD5Managed() : base() {
            HashSizeValue = 128;
            // HashValue: Leave it to HashAlgorithm.
            // State: Not used.
            Initialize();
        }

        public static new MD5Managed Create() {
#pragma warning disable HAA0502 // Explicit new reference type allocation
            return new MD5Managed();
#pragma warning restore HAA0502 // Explicit new reference type allocation
        }

        public override void Initialize() {
            _Core = MD5HashCore.Create();
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            HashCore0(array.AsSpan(ibStart, cbSize));
        }

        private void HashCore0(ReadOnlySpan<byte> span) {
            _Core.ComputeHashCode(span);
        }

        public override bool CanTransformMultipleBlocks => true;

        public override int HashSize => 128;

        protected override void HashCore(ReadOnlySpan<byte> source) {
            HashCore0(source);
        }

        protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten) {
            var h = _Core.ComputeHashCodeFinal();
            if (BinaryPrimitives.TryWriteUInt32LittleEndian(destination[12..], h.GetElement(3))) {
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination[8..], h.GetElement(2));
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination[4..], h.GetElement(1));
                BinaryPrimitives.TryWriteUInt32LittleEndian(destination[0..], h.GetElement(0));
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
