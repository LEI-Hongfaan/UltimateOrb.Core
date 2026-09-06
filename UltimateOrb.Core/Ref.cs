using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

#if NET7_0_OR_GREATER
    /// <summary>
    /// Represents a guaranteed wrapper over an actual <see langword="ref"/> reference
    /// to a value of type <typeparamref name="T"/>. Unlike <seealso cref="ByReference{T}"/>,
    /// which may emulate by-reference semantics on older runtimes using object+offset
    /// tricks, <see cref="Ref{T}"/> always preserves true by-reference identity and
    /// aliasing semantics provided by the CLR.
    ///
    /// <para>
    /// This type is a <see langword="ref struct"/>, ensuring that the reference cannot
    /// escape to the managed heap and is always stack-bound, similar to <see cref="Span{T}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the referenced value.</typeparam>
    public readonly ref struct Ref<T> {

        private readonly ref T impl;

        /// <summary>
        /// Gets a direct <see langword="ref"/> reference to the underlying value.
        /// This reference is guaranteed to be the same reference originally supplied
        /// to the constructor, preserving CLR aliasing and identity.
        /// </summary>
        public ref T Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref impl;
        }

        /// <summary>
        /// Initializes a new <see cref="Ref{T}"/> that wraps an existing
        /// <see langword="ref"/> reference. The wrapper does not copy the value;
        /// it preserves the exact reference identity.
        /// </summary>
        /// <param name="valueRef">
        /// A reference to the value to wrap. The caller must ensure that the referenced
        /// storage remains valid for the lifetime of this <see cref="Ref{T}"/>.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Ref(ref T valueRef) {
            impl = ref valueRef;
        }
    }

    /// <summary>
    /// Represents a guaranteed wrapper over an actual <see langword="ref readonly"/>
    /// reference to a value of type <typeparamref name="T"/>. Unlike <seealso cref="ReadOnlyByReference{T}"/>
    /// or other emulated ref-like constructs, <see cref="ReadOnlyRef{T}"/> always preserves
    /// true CLR read-only by-reference semantics.
    ///
    /// <para>
    /// This type is a <see langword="ref struct"/>, ensuring that the reference cannot
    /// escape to the managed heap and is always stack-bound, similar to <see cref="ReadOnlySpan{T}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of the referenced value.</typeparam>
    public readonly ref struct ReadOnlyRef<T> {

        private readonly ref readonly T impl;

        /// <summary>
        /// Gets a <see langword="ref readonly"/> reference to the underlying value.
        /// This reference is guaranteed to be the same reference originally supplied
        /// to the constructor, preserving CLR aliasing and identity while preventing mutation.
        /// </summary>
        public ref readonly T Value {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref impl;
        }

        /// <summary>
        /// Initializes a new <see cref="ReadOnlyRef{T}"/> that wraps an existing
        /// <see langword="in"/> (read-only) reference. The wrapper does not copy the value;
        /// it preserves the exact reference identity while enforcing read-only access.
        /// </summary>
        /// <param name="valueRef">
        /// A read-only reference to the value to wrap. The caller must ensure that the
        /// referenced storage remains valid for the lifetime of this <see cref="ReadOnlyRef{T}"/>.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyRef(in T valueRef) {
            impl = ref valueRef;
        }
    }
#endif
}
