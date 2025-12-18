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
        public static nint AsIntPtr(object? o) {
            unsafe {
                return o is null ? 0 : (nint)(-1 + (IntPtr**)Unsafe.AsPointer(ref Unsafe.As<ObjectBaseLayout>(o).FirstUserByte));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void* AsPointer(object? o) {
            return unchecked((void*)(nuint)AsIntPtr(o));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe T* AsPointer<T>(object? o) where T : unmanaged {
            return unchecked((T*)(nuint)AsIntPtr(o));
        }
    }
}
