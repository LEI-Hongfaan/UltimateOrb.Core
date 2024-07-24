using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Utilities {

    static partial class UnsafeParameterHelpers {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref To UnsafeAsForOut<TFrom, To>([UnscopedRef] out TFrom value) {
            Unsafe.SkipInit(out value);
            return ref Unsafe.As<TFrom, To>(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly To UnsafeAsForIn<TFrom, To>(in TFrom value) {
            return ref Unsafe.As<TFrom, To>(ref Unsafe.AsRef(in value));
        }
    }
}
