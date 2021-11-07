using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Collections.Generic.Interfaces;
using UltimateOrb.Collections.Generic.Interfaces.Typed;

namespace UltimateOrb.Collections.Generic {
    using Fields = UltimateOrb.Runtime.CompilerServices.TypeTokens;
    using Local = UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;
    using ProviderLocal = UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;

    /*struct sasds<T, TEnumerator>
        : Local.IList<T, TEnumerator>
        where TEnumerator : Local.IEnumerator<T> {
        public ref T this[long index] => throw new NotImplementedException();

        public ref T this[int index] => throw new NotImplementedException();

        public long LongCount => throw new NotImplementedException();

        public ref T Add(T item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Interfaces.Huge.IEqualityComparer<T> {
            throw new NotImplementedException();
        }

        public bool Contains(T item) {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, long arrayIndex) {
            throw new NotImplementedException();
        }

        public void CopyTo(Span<T> buffer) {
            throw new NotImplementedException();
        }

        public Interfaces.RefReturn.IEnumerator<T> GetEnumerator() {
            throw new NotImplementedException();
        }

        public int IndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
            throw new NotImplementedException();
        }

        public int IndexOf(T item) {
            throw new NotImplementedException();
        }

        public ref T Insert(long index, T item) {
            throw new NotImplementedException();
        }

        public ref T Insert(int index, T item) {
            throw new NotImplementedException();
        }

        public long LongIndexOf<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : Interfaces.Huge.IEqualityComparer<T> {
            throw new NotImplementedException();
        }

        public long LongIndexOf(T item) {
            throw new NotImplementedException();
        }

        public bool Remove<TEqualityComparer>(T item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
            throw new NotImplementedException();
        }

        public bool Remove(T item) {
            throw new NotImplementedException();
        }

        public void RemoveAt(long index) {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        Interfaces.RefReturn.IReadOnlyEnumerator<T> Interfaces.RefReturn.IReadOnlyEnumerable<T>.GetEnumerator() {
            throw new NotImplementedException();
        }

        TEnumerator IEnumerable<T, TEnumerator>.GetEnumerator() {
            throw new NotImplementedException();
        }

        TEnumerator IReadOnlyEnumerable<T, TEnumerator>.GetEnumerator() {
            throw new NotImplementedException();
        }

        Interfaces.Core.IEnumerator<T> Interfaces.Core.IEnumerable<T>.GetEnumerator() {
            throw new NotImplementedException();
        }

        Interfaces.Core.IReadOnlyEnumerator<T> Interfaces.Core.IReadOnlyEnumerable<T>.GetEnumerator() {
            throw new NotImplementedException();
        }
    }*/

    struct List<T, TEnumerator, TCore>
        where TCore : ProviderLocal.IValueProvider<Fields.Array, Array<T>>, ProviderLocal.IValueProvider<Fields.Count, nint> {

        
    }



}
