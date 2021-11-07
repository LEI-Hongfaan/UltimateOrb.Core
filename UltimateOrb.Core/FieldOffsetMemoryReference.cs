using System;
using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb {
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using UltimateOrb.Runtime.CompilerServices;

    [SerializableAttribute()]
    public readonly struct FieldOffsetMemoryReference<T> : IMemoryReference<T> {

        public readonly nint FieldOffset;

        public readonly object? Owner;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal FieldOffsetMemoryReference(object? owner, nint fieldOffset) {
            Owner = owner;
            FieldOffset = fieldOffset;
        }

        nint IMemoryReference.ByteOffset {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Owner is null ? FieldOffset : unchecked(CilVerifiable.SizeOf<nint>() + FieldOffset);
        }

        object? IMemoryReference.PinnableReference {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Owner;
        }

        public ref T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                var owner = Owner;
                var offset = FieldOffset;
                if (null != owner) {
                    unsafe {
                        fixed (byte* pData = &Unsafe.As<ObjectRawView>(owner).Data) {
                            return ref Unsafe.AsRef<T>(unchecked(pData + offset));
                        }
                    }
                }
                unsafe {
                    return ref Unsafe.AsRef<T>(unchecked((void*)offset));
                }
            }
        }
    }
}
