using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

    internal static class ObjectReferenceUnsafe {

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static nint AsIntPtr(object? handle) {
            unsafe {
                return handle is null ? default : unchecked((nint)Unsafe.AsPointer(ref Unsafe.As<ObjectRawView>(handle).Data) - CilVerifiable.SizeOf<nint>());
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static unsafe void* AsPointer(object? handle) {
            return unchecked((void*)(nuint)AsIntPtr(handle));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static unsafe T* AsPointer<T>(object? handle) where T : unmanaged {
            return unchecked((T*)(nuint)AsIntPtr(handle));
        }
    }
}
