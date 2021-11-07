using System;

namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface ICollection<T> {

        bool System.Collections.Generic.ICollection<T>.IsReadOnly {

            get => false;
        }

        void System.Collections.Generic.ICollection<T>.CopyTo(T[] array, int arrayIndex) {
            CopyTo(array.AsSpan(arrayIndex));
        }

        void CopyTo(Span<T> buffer);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface ICollection<T> {

        int System.Collections.Generic.ICollection<T>.Count {

            get => checked((int)LongCount);
        }

        void CopyTo(T[] array, long arrayIndex);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface ICollection<T> {

        void System.Collections.Generic.ICollection<T>.Add(T item) {
            Add(item);
        }

        new ref T Add(T item);
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface ICollection<T, out TEnumerator> {

        bool Contains<TEqualityComparer>(T item) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>, new() {
            return Contains(item, new TEqualityComparer());
        }

        bool Contains<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>;
        
        bool Remove<TEqualityComparer>(T item) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>, new() {
            return Remove(item, new TEqualityComparer());
        }

        bool Remove<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>;
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface ICollection<T, out TEnumerator> {

        new bool Contains<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Huge.IEqualityComparer<T> {
            return ((Typed.ICollection<T, TEnumerator>)this).Contains(item, comparer);
        }

        new bool Remove<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Huge.IEqualityComparer<T> {
            return ((Typed.ICollection<T, TEnumerator>)this).Remove(item, comparer);
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface ICollection<T> {

        void CopyTo(Array<T> array, int arrayIndex) {
            CopyTo(array.Value, arrayIndex);
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface ICollection<T> {

        void CopyTo(Array<T> array, long arrayIndex) {
            CopyTo(array.Value, arrayIndex);
        }
    }
}
