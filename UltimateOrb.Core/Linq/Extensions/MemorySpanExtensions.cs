using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Linq.Extensions {

    public static class MemorySpanExtensions {

#if NET7_0_OR_GREATER
        [Obsolete("Use Span<T> constructor instead")]
#endif
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static Span<T> AsSingletonSpan<T>(ref T value) {
#if NET5_0_OR_GREATER
            return MemoryMarshal.CreateSpan(ref value, 1);
#else
            ref byte b = ref Unsafe.As<T, byte>(ref value);
            unsafe {
                fixed (byte* p = &b) {
                    return new Span<T>(p, 1);
                }
            }
#endif
        }

#if NET7_0_OR_GREATER
        [Obsolete("Use ReadOnlySpan<T> constructor instead")]
#endif
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ReadOnlySpan<T> AsSingletonReadOnlySpan<T>(in T value) {
#if NET5_0_OR_GREATER
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in value), 1);
#else
            ref byte b = ref Unsafe.As<T, byte>(ref Unsafe.AsRef(in value));
            unsafe {
                fixed (byte* p = &b) {
                    return new ReadOnlySpan<T>(p, 1);
                }
            }
#endif
        }
    }
}
