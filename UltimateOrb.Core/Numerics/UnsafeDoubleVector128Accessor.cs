using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace UltimateOrb.Numerics {
    internal readonly ref struct UnsafeDoubleVector128Accessor {

        readonly ref readonly double item0Ref;

        public UnsafeDoubleVector128Accessor(ref readonly double item0Ref) {
            this.item0Ref = ref item0Ref;
        }

        public Vector128<double> this[int index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[(nint)index];
        }

        public Vector128<double> this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[(nuint)index];
        }

        public Vector128<double> this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector128.Create(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AddByteOffset(ref Unsafe.AsRef(in item0Ref), unchecked(index * sizeof(Vector128<double>))), 2));
        }

        public Vector128<double> this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vector128.Create(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AddByteOffset(ref Unsafe.AsRef(in item0Ref), unchecked(index * (uint)sizeof(Vector128<double>))), 2));
        }

        public Vector128<double> this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[unchecked((nint)index)];
        }

        public Vector128<double> this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this[unchecked((nuint)index)];
        }
    }
}
