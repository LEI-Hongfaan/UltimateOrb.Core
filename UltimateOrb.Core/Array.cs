using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;
using UltimateOrb.Utilities;

namespace UltimateOrb {

    public static partial class StandardExtensions {

        public static nint GetNativeLength<T>(this T[] array) {
            return unchecked((nint)CilVerifiable.GetLength(array));
        }
    }
}

namespace UltimateOrb {
    using Local = UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;
    using UltimateOrb.Runtime.CompilerServices;
    using global::Internal.System.Collections.Generic;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Collections;

    public class NativeArray {

        protected readonly nint m_pData;

        protected readonly nint m_Length;

        internal NativeArray(nint byteCount) {
            if (byteCount < 0) {
                _ = new byte[byteCount]; // throw
            }
            var pData = Marshal.AllocHGlobal(byteCount);
            ZeroMemory(pData, byteCount);
            m_pData = pData;
            m_Length = byteCount;
        }

        internal IndexOutOfRangeException ThrowIndexOutOfRangeException() {
            _ = ref Array_Empty<byte>.Value[0];
            throw null!;
        }

        internal static void ZeroMemory(nint pData, nint byteCount) {
            nint i = 0;
            for (; i <= byteCount - 0x10000; i += 0x10000) {
                unsafe {
                    new Span<byte>(i + (byte*)pData, 0x10000).Clear();
                }
            }
            if (byteCount > i) {
                unsafe {
                    new Span<byte>(i + (byte*)pData, unchecked((int)(byteCount - i))).Clear();
                }
            }
        }
    }




    [Serializable]
    public sealed class NativeArray<T>
        : NativeArray
        , IDisposable
        , IFixedSizeList<T>
        , System.Collections.Generic.IList<T>
        , System.Collections.Generic.IReadOnlyList<T>
        where T : unmanaged {

        
        [Serializable]
        public struct Enumerator : System.Collections.Generic.IEnumerator<T> {

            private readonly NativeArray<T> m_KeepAlive;

            private readonly nint m_pData;

            private readonly nint m_Length;

            private nint m_Index;
            
            // Passing -1 for endIndex so that MoveNext always returns false without mutating _index
            internal static readonly Enumerator Empty = new Enumerator(null!, -1);

            internal Enumerator(NativeArray<T> array, nint length) {
                // We allow passing null array in case of empty enumerator. 
                Debug.Assert(array == null && length == -1,
                   "Enumerator<TBase> only works on single dimension arrays w/ a lower bound of zero or with empty array for null enumerator.");
                m_KeepAlive = array!;
                m_pData = array is null ? default : array.m_pData;
                m_Index = -1;
                m_Length = length;
            }

            public bool MoveNext() {
                var index = m_Index;
                var length = m_Length;
                if (length > index) {
                    m_Index = ++index;
                    return length > index;
                }
                return false;
            }

            public T Current {

                get {
                    var index = m_Index;
                    if (m_Length > index) {
                        throw new InvalidOperationException();
                    }
                    return GetValueRefCore(m_pData, index);
                }
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset() {
                m_Index = -1;
            }

            public void Dispose() {
            }
        }

        public nint Length {

            get => m_Length;
        }

        public long LongLength {

            get => m_Length;
        }

        int System.Collections.Generic.IReadOnlyCollection<T>.Count {

            get => checked((int)m_Length);
        }

        int System.Collections.Generic.ICollection<T>.Count {

            get => checked((int)m_Length);
        }

        public bool IsReadOnly {

            get => false;
        }

        public bool IsSynchronized {

            get => true;
        }

        public bool IsFixedSize {

            get => true;
        }

        #region Indexers
        T IList<T>.this[int index] {

            get => this[index];

            set => this[index] = value;
        }

        T IReadOnlyList<T>.this[int index] {

            get => this[index];
        }

        public ref T this[int index] {

            get => ref this[(nint)index];
        }

        [CLSCompliant(false)]
        public ref T this[uint index] {

            get => ref this[(nuint)index];
        }

        public ref T this[long index] {

            get => ref this[checked((nint)index)];
        }

        [CLSCompliant(false)]
        public ref T this[ulong index] {

            get => ref this[checked((nuint)index)];
        }

        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                return ref this[unchecked((nuint)index)];
            }
        }

        [CLSCompliant(false)]
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                if (unchecked((nuint)m_Length) > index) {
                    return ref GetValueRefCore(m_pData, unchecked((nint)index));
                }
                throw ThrowIndexOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static ref T GetValueRefCore(nint pData, nint index) {
            unsafe {
                return ref Unsafe.AsRef<T>(unchecked((void*)((nuint)pData + (uint)CilVerifiable.SizeOf<T>() * (nuint)index)));
            }
        }
        #endregion



        public NativeArray(nint length) : base(checked(CilVerifiable.SizeOf<T>() * length)) {
        }

        #region Finalizer, IDisposable
        void Dispose(bool disposing) {
            nint pData;
            if (default != (pData = Interlocked.Exchange(ref Unsafe.AsRef(in m_pData), default))) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Volatile.Write(ref Unsafe.AsRef(in m_Length), default);
                Marshal.FreeHGlobal(pData);
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~NativeArray() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        void IDisposable.Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            GC.SuppressFinalize(this);
            Dispose(disposing: true);
        }
        #endregion



        int IList<T>.IndexOf(T item) {
            return checked((int)IndexOf(item));
        }

        public nint IndexOf(T item) {
            // TODO:
            throw new NotImplementedException();
        }

        public bool Contains(T item) {
            return 0 != m_Length && -1 != IndexOf(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            // TODO:
            throw new NotImplementedException();
        }
        #region IEnumerable
        public Enumerator GetEnumerator() {
            return new Enumerator(this, this.m_Length);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion
    }

    /*
    public readonly struct Array {

        public readonly System.Array Value;
    }
    */

    public readonly struct Array<T> : ISZArray<T>/*, Local.IList<TBase, Array<TBase>.Enumerator>*/ {

        public readonly struct Enumerator : Local.IEnumerator<T> {
            public ref T Current => throw new NotImplementedException();

            public void Dispose() {
                throw new NotImplementedException();
            }

            public bool MoveNext() {
                throw new NotImplementedException();
            }
        }

        public readonly T[] Value;

        internal Array(T[] array) {
            Value = array;
        }

        public Array(int length) {
            Value = new T[length];
        }

        [CLSCompliant(false)]
        public Array(uint length) {
            Value = new T[length];
        }

        public ref T this[int index] {

            get => ref Value[index];
        }

        [CLSCompliant(false)]
        public ref T this[uint index] {

            get => ref Value[index];
        }

        public ref T this[nint index] {

            get => ref CilVerifiable.GetValueRef(Value, index);
        }

        [CLSCompliant(false)]
        public ref T this[nuint index] {

            get => ref CilVerifiable.GetValueRef(Value, unchecked((nint)index));
        }

        public ref T this[long index] {

            get => ref CilVerifiable.GetValueRef(Value, checked((nint)index));
        }

        [CLSCompliant(false)]
        public ref T this[ulong index] {

            get => ref CilVerifiable.GetValueRef(Value, checked((nint)unchecked((long)index)));
        }

        public nint Length {

            get => Value.Length;
        }

        public nint NativeLength {

            get => CilVerifiable.GetLength(Value);
        }

        public long LongLength {

            get => Value.LongLength;
        }

        public static implicit operator Array<T>(T[] array) {
            return new Array<T>(array);
        }
    }
}
