#define BYREFERENCE_USE_IMPL_SPAN
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

    public readonly ref struct ByReference<T> {

#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
        private readonly System.ByReference<T> impl;
#elif BYREFERENCE_USE_IMPL_SPAN
        private readonly Span<T> impl;
#else
        private readonly ref T impl;
#endif

        public ref T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
            get => ref impl.Value;
#elif BYREFERENCE_USE_IMPL_SPAN
            get => ref MemoryMarshal.GetReference(impl);
#else
            get => ref impl;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReference(ref T valueRef) {
#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
            impl = new System.ByReference<T>(ref valueRef);
#elif BYREFERENCE_USE_IMPL_SPAN
            impl = MemoryMarshal.CreateSpan(ref valueRef, 0);
#else
            ref impl = ref valueRef;
#endif
        }
    }

    public readonly ref struct ReadOnlyByReference<T> {

#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
        private readonly System.ByReference<T> impl;
#elif BYREFERENCE_USE_IMPL_SPAN
        private readonly ReadOnlySpan<T> impl;
#else
        private readonly ref readonly T impl;
#endif

        public ref readonly T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
            get => ref impl.Value;
#elif BYREFERENCE_USE_IMPL_SPAN
            get => ref MemoryMarshal.GetReference(impl);
#else
            get => ref impl;
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyByReference(in T valueRef) {
#if BYREFERENCE_USE_IMPL_PRIVATE_CORELIB
            impl = new System.ByReference<T>(ref Unsafe.AsRef(in valueRef));
#elif BYREFERENCE_USE_IMPL_SPAN
            impl = MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in valueRef), 0);
#else
            ref impl = ref valueRef;
#endif
        }
    }

    [Obsolete("Use ByReference<T> instead.")]
    public readonly ref struct ByReferenceFat<T> {

        private readonly Span<T> impl;

        public ref T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref impl[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ByReferenceFat(ref T valueRef) {
            impl = MemoryMarshal.CreateSpan(ref valueRef, 1);
        }
    }

    [Obsolete("Use ReadOnlyByReference<T> instead.")]
    public readonly ref struct ReadOnlyByReferenceFat<T> {

        private readonly ReadOnlySpan<T> impl;

        public ref readonly T Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref impl[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyByReferenceFat(in T valueRef) {
            impl = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in valueRef), 1);
        }
    }
}
