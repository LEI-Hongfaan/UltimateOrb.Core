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

    /*struct sasds<TBase, TEnumerator>
        : Local.IList<TBase, TEnumerator>
        where TEnumerator : Local.IEnumerator<TBase> {
        public ref TBase this[long index] => throw new NotImplementedException();

        public ref TBase this[int index] => throw new NotImplementedException();

        public long LongCount => throw new NotImplementedException();

        public ref TBase Add(TBase item) {
            throw new NotImplementedException();
        }

        public void Clear() {
            throw new NotImplementedException();
        }

        public bool Contains<TEqualityComparer>(TBase item, TEqualityComparer comparer) where TEqualityComparer : Interfaces.Huge.IEqualityComparer<TBase> {
            throw new NotImplementedException();
        }

        public bool Contains(TBase item) {
            throw new NotImplementedException();
        }

        public void CopyTo(TBase[] array, long arrayIndex) {
            throw new NotImplementedException();
        }

        public void CopyTo(Span<TBase> buffer) {
            throw new NotImplementedException();
        }

        public Interfaces.RefReturn.IEnumerator<TBase> GetEnumerator() {
            throw new NotImplementedException();
        }

        public int IndexOf<TEqualityComparer>(TBase item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<TBase> {
            throw new NotImplementedException();
        }

        public int IndexOf(TBase item) {
            throw new NotImplementedException();
        }

        public ref TBase Insert(long index, TBase item) {
            throw new NotImplementedException();
        }

        public ref TBase Insert(int index, TBase item) {
            throw new NotImplementedException();
        }

        public long LongIndexOf<TEqualityComparer>(TBase item, TEqualityComparer comparer) where TEqualityComparer : Interfaces.Huge.IEqualityComparer<TBase> {
            throw new NotImplementedException();
        }

        public long LongIndexOf(TBase item) {
            throw new NotImplementedException();
        }

        public bool Remove<TEqualityComparer>(TBase item, TEqualityComparer comparer) where TEqualityComparer : System.Collections.Generic.IEqualityComparer<TBase> {
            throw new NotImplementedException();
        }

        public bool Remove(TBase item) {
            throw new NotImplementedException();
        }

        public void RemoveAt(long index) {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index) {
            throw new NotImplementedException();
        }

        Interfaces.RefReturn.IReadOnlyEnumerator<TBase> Interfaces.RefReturn.IReadOnlyEnumerable<TBase>.GetEnumerator() {
            throw new NotImplementedException();
        }

        TEnumerator IEnumerable<TBase, TEnumerator>.GetEnumerator() {
            throw new NotImplementedException();
        }

        TEnumerator IReadOnlyEnumerable<TBase, TEnumerator>.GetEnumerator() {
            throw new NotImplementedException();
        }

        Interfaces.Core.IEnumerator<TBase> Interfaces.Core.IEnumerable<TBase>.GetEnumerator() {
            throw new NotImplementedException();
        }

        Interfaces.Core.IReadOnlyEnumerator<TBase> Interfaces.Core.IReadOnlyEnumerable<TBase>.GetEnumerator() {
            throw new NotImplementedException();
        }
    }*/

    struct List<T, TEnumerator, TCore>
        where TCore : ProviderLocal.IValueProvider<Fields.Array, Array<T>>, ProviderLocal.IValueProvider<Fields.Count, nint> {

        
    }



}
