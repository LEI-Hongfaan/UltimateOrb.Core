using System;
using System.Collections.Generic;
using UltimateOrb.Utilities;

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public static partial class Extensions {

        public static int get_Count<T, TReadOnlyCollection>(this ref TReadOnlyCollection value)
            where TReadOnlyCollection : struct, IReadOnlyCollection<T> {
            return value.Count;
        }

        public static int get_Count<T, TReadOnlyCollection>(this TReadOnlyCollection value)
            where TReadOnlyCollection : class, IReadOnlyCollection<T> {
            return value.Count;
        }
    }

    public partial interface IReadOnlyCollection<out T> {

        int System.Collections.Generic.IReadOnlyCollection<T>.Count {

            get => checked((int)LongCount); 
        }

        long LongCount {

            get;
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyCollection<T> {

        bool Contains(T item);

        void CopyTo(T[] array, int arrayIndex) {
            CopyTo(array.AsSpan(arrayIndex));
        }

        void CopyTo(Span<T> buffer);

        //void CopyTo(StridedMemorySpan<T> buffer) {
        //    CopyTo(buffer.Span);
        //}
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    struct sdfds : IReadOnlyCollection<Guid> {
        public long LongCount => throw new NotImplementedException();

        public bool Contains(Guid item) {
            throw new NotImplementedException();
        }

        public void CopyTo(Guid[] array, long arrayIndex) {
            throw new NotImplementedException();
        }

        public void CopyTo(Span<Guid> buffer) {
            throw new NotImplementedException();
        }

        public RefReturn.IReadOnlyEnumerator<Guid> GetEnumerator() {
            throw new NotImplementedException();
        }
    }

    public partial interface IReadOnlyCollection<T> {

        void CopyTo(T[] array, long arrayIndex) {
            var count = this.LongCount;

            throw new NotImplementedException();
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyCollection<T> {

        void CopyTo(Array<T> array, int arrayIndex) {
            CopyTo(array.Value, arrayIndex);
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyCollection<T> {

        void CopyTo(Array<T> array, long arrayIndex) {
            CopyTo(array.Value, arrayIndex);
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyCollection<T, out TEnumerator> {

        bool RefReturn.IReadOnlyCollection<T>.Contains(T item) {
            return Contains(item, EqualityComparer<T>.Default);
        }

        bool Contains<TEqualityComparer>(T item) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>, new()
            => Contains(item, new TEqualityComparer());

        bool Contains<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T>;
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyCollection<T, out TEnumerator> {

        new bool Contains<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Huge.IEqualityComparer<T> {
            return ((Typed_RefReturn.IReadOnlyCollection<T, TEnumerator>)this).Contains(item, comparer);
        }
    }
}
