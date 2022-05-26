using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    public interface IReadOnlyIndexable<TKey, TValue> {

        public ref readonly TValue this[TKey index] {

            get;
        }

        public Type KeyType {

            get => typeof(TKey);
        }

        public Type ValueType {

            get => typeof(TValue);
        }
    }

    public interface IIndexable<TKey, TValue> : IReadOnlyIndexable<TKey, TValue> {

        ref readonly TValue IReadOnlyIndexable<TKey, TValue>.this[TKey index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[index];
        }

        public new ref TValue this[TKey index] {

            get;
        }
    }

    public interface IArrayLikeBase {

        public int Length {

            get;
        }

        public nint NativeLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Length;
        }

        public long LongLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NativeLength;
        }
    }

    


    public interface IArrayLike<T>: IArrayLikeBase {

        public ref T this[int index] {

            get;
        }

        [CLSCompliant(false)]
        public ref T this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((int)index)];
        }

        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((int)index)];
        }

        [CLSCompliant(false)]
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nint)index)];
        }

        public ref T this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nint)index)];
        }

        [CLSCompliant(false)]
        public ref T this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((long)index)];
        }
    }
}
