using System;

namespace UltimateOrb.Runtime.Caching {

    public class PerTypeCache<T> : KeyedCache<Type, T> {

        public PerTypeCache(int capacity, Func<Type, T> valueCreator) : base(capacity, valueCreator) {
        }
    }
}
