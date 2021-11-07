using System;
using System.Collections.Generic;
using UltimateOrb.Utilities;

namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyList<out T> {

        T System.Collections.Generic.IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        new T this[int index] {

            get;
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IReadOnlyList<out T> {

        T Core.IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        T this[long index] {

            get;
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyList<T> {

        T Core.IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        bool IReadOnlyCollection<T>.Contains(T item) {
            return -1 != IndexOf(item);
        }

        void IReadOnlyCollection<T>.CopyTo(Span<T> buffer) {
            var count = Count;
            if (count > buffer.Length) {
                throw new ArgumentException();
            }
            for (var i = 0; count > i; ++i) {
                buffer[i] = this[i];
            }
        }
        /*
        struct Enumerator : IReadOnlyEnumerator<T> {
            Func<int, T> GetValue;
        }
        
        IReadOnlyEnumerator<T> IReadOnlyEnumerable<T>.GetEnumerator() {
            return new Enumerator((int i) => ref this[i]);
        }
        */
        int IndexOf(T item) {
            var count = Count;
            var comparer = EqualityComparer<T>.Default;
            for (var i = 0; count > i; ++i) {
                if (comparer.Equals(item, this[i])) {
                    return i;
                }
            }
            return -1;
        }

        new ref readonly T this[int index] {

            get;
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {


    public partial interface IReadOnlyList<T> {

        T Core.IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        new ref readonly T this[long index] {

            get;
        }

        int RefReturn.IReadOnlyList<T>.IndexOf(T item) {
            return checked((int)LongIndexOf(item));
        }

        long LongIndexOf(T item);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyList<T, out TEnumerator> {

        int IndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>;
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyList<T, out TEnumerator> {

        long LongIndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Huge.IEqualityComparer<T>;
    }
}
