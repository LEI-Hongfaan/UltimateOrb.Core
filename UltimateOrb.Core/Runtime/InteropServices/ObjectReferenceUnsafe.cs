using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb {
    using System;
    using System.Runtime.InteropServices;
    using UltimateOrb.Runtime.CompilerServices;

    internal static class ObjectReferenceUnsafe {

        internal sealed class ObjectBaseLayout {

            public byte FirstUserByte;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static nint AsIntPtr(object? handle) {
            unsafe {
                return handle is null ? 0 : (nint)(-1 + (IntPtr**)Unsafe.AsPointer(ref Unsafe.As<ObjectBaseLayout>(handle).FirstUserByte));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void* AsPointer(object? handle) {
            return unchecked((void*)(nuint)AsIntPtr(handle));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe T* AsPointer<T>(object? handle) where T : unmanaged {
            return unchecked((T*)(nuint)AsIntPtr(handle));
        }
    }
}
