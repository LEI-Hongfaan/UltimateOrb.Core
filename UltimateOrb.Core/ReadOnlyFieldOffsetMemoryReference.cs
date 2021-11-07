using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

    [SerializableAttribute()]
    public readonly struct ReadOnlyFieldOffsetMemoryReference<T> : IReadOnlyMemoryReference<T> {

        private readonly FieldOffsetMemoryReference<T> Base;

        public readonly nint FieldOffset {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Base.FieldOffset;
        }

        public readonly object? Owner {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Base.Owner;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyFieldOffsetMemoryReference(object? manager, nint byteOffset) {
            Base = new FieldOffsetMemoryReference<T>(manager, byteOffset);
        }

        nint IMemoryReference.ByteOffset {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.AsRef(in Base).get_ByteOffset();
        }

        object? IMemoryReference.PinnableReference {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Unsafe.AsRef(in Base).get_PinnableReference();
        }

        public ref readonly T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                return ref Base.Value;
            }
        }
    }
}
