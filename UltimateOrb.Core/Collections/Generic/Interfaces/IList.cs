namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IList<T> {

        T System.Collections.Generic.IList<T>.this[int index] {

            get => this[index];

            set => this[index] = value;
        }

        T IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        new T this[int index] {

            get;

            set;
        }

        int System.Collections.Generic.IList<T>.IndexOf(T item) {
            return IndexOf(item);
        }

        new int IndexOf(T item);

        void System.Collections.Generic.IList<T>.Insert(int index, T item) {
            Insert(index, item);
        }

        new void Insert(int index, T item);

        void System.Collections.Generic.IList<T>.RemoveAt(int index) {
            RemoveAt(index);
        }

        new void RemoveAt(int index);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IList<T> {



        T IReadOnlyList<T>.this[long index] {

            get => this[index];
        }

        new T this[long index] {

            get;

            set;
        }

        void Insert(long index, T item);

        long LongIndexOf(T item);

        void RemoveAt(long index);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IList<T> {

        T Core.IList<T>.this[int index] {

            get => this[index];

            set => this[index] = value;
        }
        
        new ref T this[int index] {

            get;
        }

        void Core.IList<T>.Insert(int index, T item) {
            Insert(index, item);
        }

        new ref T Insert(int index, T item);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IList<T> {

        T Huge.IList<T>.this[long index] {

            get => this[index];

            set => this[index] = value;
        }

        new ref T this[long index] {

            get;
        }

        void Huge.IList<T>.Insert(long index, T item) {
            Insert(index, item);
        }

        new ref T Insert(long index, T item);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IList<T, out TEnumerator> {

        int IndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>;
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IList<T, out TEnumerator> {

        long LongIndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Huge.IEqualityComparer<T>;
    }
}
