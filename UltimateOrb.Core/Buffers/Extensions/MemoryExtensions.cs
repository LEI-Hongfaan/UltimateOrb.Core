using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Buffers.Extensions {

    /// <summary>
    /// Provides extension methods for working with memory and span types.
    /// </summary>
    [Discardable]
    public static partial class MemoryExtensions {

        /// <summary>
        /// Returns a writable <see cref="Span{T}"/> from a <see cref="ReadOnlySpan{T}"/>.
        /// This is dangerous and should only be used when it is guaranteed that the underlying data is not actually read-only.
        /// </summary>
        /// <typeparam name="T">The type of elements in the span.</typeparam>
        /// <param name="span">The read-only span to convert.</param>
        /// <returns>A writable span over the same memory.</returns>
        [Discardable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> DangerousAsWritable<T>(this ReadOnlySpan<T> span) {
            return MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(span), span.Length);
        }

        /// <summary>
        /// Returns a <see cref="ReadOnlySpan{T}"/> from a <see cref="Span{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the span.</typeparam>
        /// <param name="span">The span to convert.</param>
        /// <returns>A read-only span over the same memory.</returns>
        [Discardable]
        public static ReadOnlySpan<T> AsReadOnly<T>(this Span<T> span) {
            return span;
        }

        /// <summary>
        /// Returns a <see cref="ReadOnlyMemory{T}"/> from a <see cref="Memory{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the memory.</typeparam>
        /// <param name="memory">The memory to convert.</param>
        /// <returns>A read-only memory over the same memory region.</returns>
        [Discardable]
        public static ReadOnlyMemory<T> AsReadOnly<T>(this Memory<T> memory) {
            return memory;
        }

        /// <summary>
        /// Returns a writable <see cref="Memory{T}"/> from a <see cref="ReadOnlyMemory{T}"/>.
        /// This is dangerous and should only be used when it is guaranteed that the underlying data is not actually read-only.
        /// </summary>
        /// <typeparam name="T">The type of elements in the memory.</typeparam>
        /// <param name="memory">The read-only memory to convert.</param>
        /// <returns>A writable memory over the same memory region.</returns>
        [Discardable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<T> DangerousAsWritable<T>(this ReadOnlyMemory<T> memory) {
            return Unsafe.As<ReadOnlyMemory<T>, Memory<T>>(ref memory);
        }
    }
}
