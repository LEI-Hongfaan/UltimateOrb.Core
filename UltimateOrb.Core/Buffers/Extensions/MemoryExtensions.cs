using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Buffers.Extensions {

    [Discardable]
    public static partial class MemoryExtensions {

        [Discardable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> DangerousAsWritable<T>(this ReadOnlySpan<T> span) {
            return MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(span), span.Length);
        }

        [Discardable]
        public static ReadOnlySpan<T> AsReadOnly<T>(this Span<T> span) {
            return span;
        }

        [Discardable]
        public static ReadOnlyMemory<T> AsReadOnly<T>(this Memory<T> memory) {
            return memory;
        }

        [Discardable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Memory<T> DangerousAsWritable<T>(this ReadOnlyMemory<T> memory) {
            return Unsafe.As<ReadOnlyMemory<T>, Memory<T>>(ref memory);
        }
    }
}
