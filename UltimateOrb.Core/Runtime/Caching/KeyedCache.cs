using System;
using System.Collections.Generic;
using System.Threading;

namespace UltimateOrb.Runtime.Caching {

    public class KeyedCache<TKey, TValue> where TKey : notnull {

        struct Entry {

            public int Flags;

            public int HashCode;

            public TKey Key;

            public TValue Value;
        }

        readonly Entry[] data;

        readonly IEqualityComparer<TKey> keyComparer;

        readonly Func<TKey, TValue> valueCreator;

        public KeyedCache(int capacity, Func<TKey, TValue> valueCreator, IEqualityComparer<TKey>? keyComparer = null) {
            if (1 > capacity) {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }
            if (valueCreator is null) {
                throw new ArgumentNullException(nameof(valueCreator));
            }
            data = new Entry[capacity];
            this.valueCreator = valueCreator;
            this.keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
        }

        public TValue this[TKey key] {

            get {
                if (key is null) {
                    throw new ArgumentNullException();
                }
                var h = keyComparer.GetHashCode(key);
                ref var p = ref data[unchecked((uint)h % (uint)data.Length)];
                Entry t;
                SpinWait w = new SpinWait();
                do {
                    if (0 == Interlocked.CompareExchange(ref p.Flags, 1, 0)) {
                        t = p;
                        Volatile.Write(ref p.Flags, 0);
                        break;
                    }
                    w.SpinOnce();
                } while (true);
                if (h == t.HashCode && keyComparer.Equals(key, t.Key)) {
                    return t.Value;
                }
                var v = valueCreator(key);
                w.Reset();
                do {
                    if (0 == Interlocked.CompareExchange(ref p.Flags, 2, 0)) {
                        p.Key = key;
                        p.HashCode = h;
                        p.Value = v;
                        Volatile.Write(ref p.Flags, 0);
                        break;
                    }
                    w.SpinOnce();
                } while (true);
                return v;
            }
        }
    }
}
