using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CilVerifiable = UltimateOrb.Utilities.CilVerifiable;

namespace UltimateOrb {

    public static class MemorySpanExtensions {

        internal static class ArrayDataOffset {

            internal static readonly int Value = GetValue();

            private static unsafe int GetValue() {
                var array = new int[1];
                GCHandle handle = default;
                try {
                    handle = GCHandle.Alloc(array, GCHandleType.Pinned);
                    IntPtr address = handle.AddrOfPinnedObject();
                    return unchecked((int)((nint)Unsafe.AsPointer(ref array[0]) - (nint)address));
                } finally {
                    if (handle.IsAllocated) {
                        handle.Free();
                    }
                }
            }
        }

        static class ThrowHelper {

            [MethodImplAttribute(MethodImplOptions.NoInlining)]
            internal static ArgumentOutOfRangeException ThrowArgumentOutOfRangeException() {
                throw new ArgumentOutOfRangeException();
            }

            [MethodImplAttribute(MethodImplOptions.NoInlining)]
            internal static ArrayTypeMismatchException ThrowArrayTypeMismatchException() {
                throw new ArrayTypeMismatchException();
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe Span<T> DangerousCreateSpan<T>(object? manager, IntPtr byteOffset, int count) {
            if (null != manager) {
                var handle = GCHandle.Alloc(manager, GCHandleType.Pinned);
                try {
                    IntPtr address = handle.AddrOfPinnedObject();
                    return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(CilVerifiable.AddUnchecked(byteOffset, address).ToPointer()), count);
                } finally {
                    handle.Free();
                }
            }
            return MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(byteOffset.ToPointer()), count);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static MemorySpan<byte> AsBytes<T>(this MemorySpan<T> buffer) where T : unmanaged {
            var sizeOfT = CilVerifiable.SizeOf<T>();
            var count = checked((int)(sizeOfT * buffer.Count));
            return new MemorySpan<byte>(buffer.Manager, buffer.ByteOffset, count);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MemorySpan<T> AsMemorySpan<T>(this T[]? array) {
            if (array == null) {
                return default;
            }

            if (!typeof(T).IsValueType && array.GetType() != typeof(T[])) {
                throw ThrowHelper.ThrowArrayTypeMismatchException();
            }

            return new MemorySpan<T>(array, (IntPtr)ArrayDataOffset.Value, CilVerifiable.GetLength(array));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MemorySpan<T> AsMemorySpan<T>(this T[]? array, int start) {
            if (array == null) {
                if (start != 0) {
                    throw ThrowHelper.ThrowArgumentOutOfRangeException();
                }
                return default;
            }
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[])) {
                throw ThrowHelper.ThrowArrayTypeMismatchException();
            }
            if ((uint)start > (uint)array.Length) {
                throw ThrowHelper.ThrowArgumentOutOfRangeException();
            }

            return new MemorySpan<T>(array, CilVerifiable.AddUnchecked((IntPtr)ArrayDataOffset.Value, CilVerifiable.MultiplyUnchecked((IntPtr)CilVerifiable.SizeOf<T>(), (IntPtr)start)), unchecked(array.Length - start));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static unsafe MemorySpan<T> AsMemorySpan<T>(this T[]? array, int start, int length) {
            if (array == null) {
                if (start != 0 || length != 0) {
                    throw ThrowHelper.ThrowArgumentOutOfRangeException();
                }
                return default;
            }
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[])) {
                throw ThrowHelper.ThrowArrayTypeMismatchException();
            }

            if (sizeof(ulong) <= IntPtr.Size) {
                if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)array.Length) {
                    throw ThrowHelper.ThrowArgumentOutOfRangeException();
                }
            } else {
                if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start)) {
                    throw ThrowHelper.ThrowArgumentOutOfRangeException();
                }
            }

            return new MemorySpan<T>(array, CilVerifiable.AddUnchecked((IntPtr)ArrayDataOffset.Value, CilVerifiable.MultiplyUnchecked((IntPtr)CilVerifiable.SizeOf<T>(), (IntPtr)start)), array.Length);
        }
    }
}
