using System;
using System.Collections.Generic;
using System.Linq;

namespace UltimateOrb.Collections.Generic {

    public struct ReadOnlyListView<T, TResult> : IList<TResult>, IReadOnlyList<TResult> {

        readonly IList<T> list;

        readonly Func<T, TResult> selector;

        public ReadOnlyListView(IList<T> list, Func<T, TResult> selector) {
            this.list = list ?? throw new ArgumentNullException(nameof(list));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public TResult this[int index] {

            get => selector(this.list[index]);

            set => throw new NotSupportedException();
        }

        public int Count => this.list.Count;

        public bool IsReadOnly => true;

        public void Add(TResult item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(TResult item) {
            return -1 != IndexOf(item);
        }

        public void CopyTo(TResult[] array, int arrayIndex) {
            // TODO: Perf
            this.list.Select(selector).ToList().CopyTo(array, arrayIndex);
        }

        public IEnumerator<TResult> GetEnumerator() {
            foreach (var item in this.list) {
                yield return selector(item);
            }
        }

        public int IndexOf(TResult item) {
            var comparer = EqualityComparer<TResult>.Default;
            var i = 0;
            foreach (var j in this.list) {
                if (comparer.Equals(selector(j), item)) {
                    return i;
                }
                ++i;
            }
            return -1;
        }

        public void Insert(int index, TResult item) {
            throw new NotSupportedException();
        }

        public bool Remove(TResult item) {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index) {
            throw new NotSupportedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
