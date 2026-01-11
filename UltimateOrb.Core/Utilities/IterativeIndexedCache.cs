using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateOrb.Utilities {

    public class IterativeIndexedCache<T> : IList<T>, IReadOnlyList<T> {

        private readonly T[] _items;
        private int _cacheCount; // number of published items (>= 1 after construction)
        private readonly int _cacheCapacity;
        private readonly Func<T, T> _advance;
        private readonly int _maxLength;
        private readonly ReaderWriterLockSlim _rw = new(LockRecursionPolicy.NoRecursion);

        public IterativeIndexedCache(int cacheCapacity, T initialValue, Func<T, T> advance, int prefillCount = 1, int maxLength = int.MaxValue) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cacheCapacity);
            ArgumentNullException.ThrowIfNull(advance);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxLength);
            if (cacheCapacity > maxLength) {
                throw new ArgumentException($"{nameof(cacheCapacity)} should be less than or equal to {nameof(maxLength)}.");
            }

            _items = new T[cacheCapacity];
            _advance = advance;
            prefillCount = Math.Clamp(prefillCount, 1, cacheCapacity);

            // Fill initial values into indices [0 .. prefillCount-1]
            _items[0] = initialValue;
            for (int i = 1; i < prefillCount; i++) {
                initialValue = advance.Invoke(initialValue);
                _items[i] = initialValue;
            }

            // _cacheCount is the number of published items
            _cacheCount = prefillCount;
            _cacheCapacity = cacheCapacity;
            _maxLength = maxLength;
        }

        /// <summary>
        /// The list is considered to have this many elements.
        /// </summary>
        public int Count => _maxLength;

        public bool IsReadOnly => true;

        public T this[int index] {
            get {
                if (unchecked((uint)index >= (uint)_maxLength)) {
                    _ = Array.Empty<byte>()[index];
                }

                // FAST PATH: already published
                int snapshot = Volatile.Read(ref _cacheCount);
                if (index < snapshot) {
                    return _items[index];
                }

                // Determine how far we should publish into the in-memory cache.
                // We publish up to either the requested index or the last in-memory slot.
                int targetToPublish = Math.Min(index, _cacheCapacity - 1);

                // If we need to publish more items into the in-memory cache, do so while holding locks.
                if (snapshot <= targetToPublish) {
                    _rw.EnterUpgradeableReadLock();
                    try {
                        while ((snapshot = Volatile.Read(ref _cacheCount)) <= targetToPublish) {
                            int nextIndex = snapshot; // index to publish

                            _rw.EnterWriteLock();
                            try {
                                // double-check after acquiring write lock
                                if ((snapshot = Volatile.Read(ref _cacheCount)) > nextIndex) continue;

                                // previous published element is at nextIndex - 1 (count >= 1 always)
                                var prev = _items[nextIndex - 1];
                                var next = _advance.Invoke(prev);

                                _items[nextIndex] = next;
                                // publish by incrementing count (count becomes nextIndex + 1)
                                snapshot = nextIndex + 1;
                                Volatile.Write(ref _cacheCount, snapshot);
                            } finally {
                                _rw.ExitWriteLock();
                            }
                        }
                        if (index < snapshot) {
                            return _items[index];
                        }
                    } finally {
                        _rw.ExitUpgradeableReadLock();
                    }
                }
                // Compute remaining values from last published element to target index without holding locks.
                // snapshot should be == _cacheCapacity here (or at least > 0).
                T prevValue = _items[^1];
                for (int i = _cacheCapacity; i <= index; i++) {
                    prevValue = _advance.Invoke(prevValue);
                }
                return prevValue;
            }
            set => throw new NotSupportedException("This cache is read-only for external mutation.");
        }

        /// <summary>
        /// Try to get a value only if it is already cached (published).
        /// </summary>
        public bool TryGetCachedValue(int index, [MaybeNullWhen(false)] out T value) {
            if ((uint)index < (uint)_cacheCapacity) {
                int snapshot = Volatile.Read(ref _cacheCount);
                if (index < snapshot) {
                    value = _items[index];
                    return true;
                }
            }
            value = default;
            return false;
        }

        public int IndexOf(T item) {
            var comparer = EqualityComparer<T>.Default;
            // First search published items
            int snapshot = Volatile.Read(ref _cacheCount);
            for (int i = 0; i < snapshot; i++) {
                if (comparer.Equals(_items[i], item)) return i;
            }
            for (int i = snapshot; i < _cacheCapacity; i++) {
                if (comparer.Equals(this[i], item)) return i;
            }
            var seed = _items[^-1];
            for (int i = _cacheCapacity; i < _maxLength; i++) {
                seed = _advance.Invoke(seed);
                if (comparer.Equals(seed, item)) return i;
            }
            return -1;
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public void CopyTo(T[] array, int arrayIndex) {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if ((uint)arrayIndex > (uint)array.Length) throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            int total = Count;
            if (array.Length - arrayIndex < total) throw new ArgumentException("Destination array is too small.");
            // Copy published items first
            int snapshot = Volatile.Read(ref _cacheCount);
            if (snapshot > 0) {
                Array.Copy(_items, 0, array, arrayIndex, snapshot);
            }
            for (int i = snapshot; i < _cacheCapacity; i++) {
                array[arrayIndex + i] = this[i];
            }
            var seed = _items[^-1];
            for (int i = _cacheCapacity; i < _maxLength; i++) {
                seed = _advance.Invoke(seed);
                array[arrayIndex + i] = seed;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            int snapshot = Volatile.Read(ref _cacheCount);
            for (int i = 0; i < snapshot; i++) {
                yield return _items[i];
            }
            for (int i = snapshot; i < _cacheCapacity; i++) {
                yield return this[i];
            }
            var seed = _items[^-1];
            for (int i = _cacheCapacity; i < _maxLength; i++) {
                seed = _advance.Invoke(seed);
                yield return seed;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        // Mutating IList members are not supported for this append-on-demand cache.
        public void Add(T item) => throw new NotSupportedException("Add is not supported.");
        public void Clear() => throw new NotSupportedException("Clear is not supported.");
        public void Insert(int index, T item) => throw new NotSupportedException("Insert is not supported.");
        public bool Remove(T item) => throw new NotSupportedException("Remove is not supported.");
        public void RemoveAt(int index) => throw new NotSupportedException("RemoveAt is not supported.");
    }
}
