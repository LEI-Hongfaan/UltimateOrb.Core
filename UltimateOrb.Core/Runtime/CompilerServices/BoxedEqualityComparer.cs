using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UltimateOrb.Runtime.CompilerServices {

    public struct BoxedEqualityComparer<T, TComparer> : IEqualityComparer<T>, IEqualityComparer<Boxed<T>>
        where T : struct
        where TComparer : IEqualityComparer<T> {

        TComparer comparer;

        public BoxedEqualityComparer(TComparer comparer) {
            this.comparer = comparer;
        }

        public bool Equals(T x, T y) {
            return comparer.Equals(x, y);
        }

        public bool Equals(Boxed<T>? x, Boxed<T>? y) {
            return x != null ? y != null && comparer.Equals(x.Value, y.Value) : y == null;
        }

        public int GetHashCode([DisallowNull] T obj) {
            return comparer.GetHashCode(obj);
        }

        public int GetHashCode([DisallowNull] Boxed<T> obj) {
            return obj is null ? 0 : comparer.GetHashCode(obj.Value);
        }
    }
}
