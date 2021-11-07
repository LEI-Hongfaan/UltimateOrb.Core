using System;

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IEqualityComparer<in T> {

        int System.Collections.Generic.IEqualityComparer<T>.GetHashCode(T obj) {
            return GetLongHashCode(obj).GetHashCode();
        }

        long GetLongHashCode(T obj);
    }
}
