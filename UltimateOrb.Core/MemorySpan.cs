using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb {

    [SerializableAttribute()]
    public readonly struct MemorySpan<T>/* : IMemorySpan, ISpanProvider<T>*/ {

        public readonly nint ByteOffset;

        public readonly nint Count;

        public readonly object? Manager;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal MemorySpan(object? manager, nint byteOffset, nint count) {
            Manager = manager;
            ByteOffset = byteOffset;
            Count = count;
        }

        public Span<T> Span {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => MemorySpanExtensions.DangerousCreateSpan<T>(Manager, ByteOffset, checked((int)Count));
        }
        /*
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MemorySpan<T> DangerousCreate<TMemorySpan>(TMemorySpan value)
            where TMemorySpan : IMemorySpan {
            return new MemorySpan<T>(value.Manager, value.ByteOffset, value.Count);
        }

        object? IMemorySpan.Manager {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Manager;
        }

        IntPtr IMemorySpan.ByteOffset {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ByteOffset;
        }

        int IMemorySpan.Count {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => checked((int)Count);
        }
        */
    }
}
