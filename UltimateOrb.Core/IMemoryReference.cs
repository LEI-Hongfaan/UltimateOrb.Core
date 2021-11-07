using System.Runtime.CompilerServices;

namespace UltimateOrb {

    public interface IMemoryReference {

        public nint ByteOffset {

            get;
        }

        public object? PinnableReference {

            get;
        }
    }

    public static partial class MemoryReferenceExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint get_ByteOffset<TMemoryReference>(this ref TMemoryReference memoryReference) where TMemoryReference : struct, IMemoryReference {
            return memoryReference.ByteOffset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint get_ByteOffset<TMemoryReference>(this TMemoryReference memoryReference) where TMemoryReference : class, IMemoryReference {
            return memoryReference.ByteOffset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object? get_PinnableReference<TMemoryReference>(this ref TMemoryReference memoryReference) where TMemoryReference : struct, IMemoryReference {
            return memoryReference.PinnableReference;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object? get_PinnableReference<TMemoryReference>(this TMemoryReference memoryReference) where TMemoryReference : class, IMemoryReference {
            return memoryReference.PinnableReference;
        }
    }
}
