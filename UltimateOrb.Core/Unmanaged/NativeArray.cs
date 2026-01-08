using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Unmanaged {


    public static partial class NativeMemoryHelpers {

        [CLSCompliant(false)]
        public static unsafe T* AllocZeroed<T>() where T : unmanaged {
            return (T*)NativeMemory.AllocZeroed(unchecked((nuint)Unsafe.SizeOf<T>()));
        }

        [CLSCompliant(false)]
        public static unsafe T* Alloc<T>() where T : unmanaged {
            return (T*)NativeMemory.Alloc(unchecked((nuint)Unsafe.SizeOf<T>()));
        }
    }

    public readonly partial struct ZeroedT {
    }

    public readonly partial struct NativeArray<T> where T : unmanaged {
        internal unsafe readonly T* _Base;
        internal readonly nuint _Length;
    }

    public readonly partial struct AlignedNativeArray<T> where T : unmanaged {
        internal unsafe readonly T* _Base;
        internal readonly nuint _Length;
    }

    public readonly partial struct NativeArray<T> {

        public unsafe NativeArray(void* ptr, nuint length) {
            this._Base = (T*)ptr;
            this._Length = length;
        }
    }
    public readonly partial struct AlignedNativeArray<T> {

        public unsafe AlignedNativeArray(void* ptr, nuint length) {
            this._Base = (T*)ptr;
            this._Length = length;
        }
    }

    public readonly partial struct NativeArray<T> {

        public NativeArray(int length) {
            nuint length_checked = checked((uint)length);
            unsafe {
                this._Base = (T*)NativeMemory.Alloc(length_checked, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length_checked;
        }

        // constructor with nint parameter
        public NativeArray(nint length) {
            nuint length_checked = checked((uint)length);
            unsafe {
                this._Base = (T*)NativeMemory.Alloc(length_checked, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length_checked;
        }

        // constructor with uint parameter
        public NativeArray(uint length) {
            unsafe {
                this._Base = (T*)NativeMemory.Alloc(length, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length;
        }

        public NativeArray(nuint length) {
            unsafe {
                this._Base = (T*)NativeMemory.Alloc(length, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length;
        }
    }
    public readonly partial struct NativeArray<T> {

        public NativeArray(int length, ZeroedT ignored) {
            nuint length_checked = checked((uint)length);
            unsafe {
                this._Base = (T*)NativeMemory.AllocZeroed(length_checked, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length_checked;
        }

        // constructor with nint parameter
        public NativeArray(nint length, ZeroedT ignored) {
            nuint length_checked = checked((uint)length);
            unsafe {
                this._Base = (T*)NativeMemory.AllocZeroed(length_checked, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length_checked;
        }

        // constructor with uint parameter
        public NativeArray(uint length, ZeroedT ignored) {
            unsafe {
                this._Base = (T*)NativeMemory.AllocZeroed(length, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length;
        }

        public NativeArray(nuint length, ZeroedT ignored) {
            unsafe {
                this._Base = (T*)NativeMemory.AllocZeroed(length, unchecked((uint)Unsafe.SizeOf<T>()));
            }
            this._Length = length;
        }
    }

    public readonly partial struct AlignedNativeArray<T> {

        public AlignedNativeArray(int length, nuint alignment) : this(checked((uint)length), alignment) {
        }

        // constructor with nint parameter
        public AlignedNativeArray(nint length, nuint alignment) : this(checked((nuint)length), alignment) {
        }

        // constructor with uint parameter
        public AlignedNativeArray(uint length, nuint alignment) {
            unsafe {
                this._Base = (T*)NativeMemory.AlignedAlloc(checked((nuint)length * unchecked((uint)Unsafe.SizeOf<T>())), alignment);
            }
            this._Length = length;
        }

        public AlignedNativeArray(nuint length, nuint alignment) {
            unsafe {
                this._Base = (T*)NativeMemory.AlignedAlloc(checked(length * unchecked((uint)Unsafe.SizeOf<T>())), alignment);
            }
            this._Length = length;
        }
    }

    public readonly partial struct AlignedNativeArray<T> {

        public AlignedNativeArray(int length, nuint alignment, ZeroedT ignored) : this(checked((uint)length), alignment, default(ZeroedT)) {
        }

        // constructor with nint parameter
        public AlignedNativeArray(nint length, nuint alignment, ZeroedT ignored) : this(checked((nuint)length), alignment, default(ZeroedT)) {
        }

        // constructor with uint parameter
        public AlignedNativeArray(uint length, nuint alignment, ZeroedT ignored) : this((nuint)length, alignment, default(ZeroedT)) {
        }

        public AlignedNativeArray(nuint length, nuint alignment, ZeroedT ignored) {
            unsafe {
                var byte_count = checked(length * unchecked((uint)Unsafe.SizeOf<T>()));
                var ptr = (T*)NativeMemory.AlignedAlloc(byte_count, alignment);
                NativeMemory.Clear(ptr, byte_count);
                this._Base = ptr;
            }
            this._Length = length;
        }
    }

    public readonly partial struct AlignedNativeArray<T> : IDisposable {

        /// <summary>
        /// WARNING: Must not invoke Dispose multiple times.
        /// </summary>
        public void Dispose() {
            unsafe {
                NativeMemory.AlignedFree(this._Base);
            }
        }
    }

    public readonly partial struct NativeArray<T> : IDisposable {

        /// <summary>
        /// WARNING: Must not invoke Dispose multiple times.
        /// </summary>
        public void Dispose() {
            unsafe {
                NativeMemory.Free(this._Base);
            }
        }
    }

    internal enum DisposeLockState {
        Unlocked,
        Locked,
        Disposed
    }

    public struct SafeDisposable<T> : IDisposable where T : IDisposable {

        T resource;

        int state; // DisposeLockState state;

        public SafeDisposable(T resource) {
            this.resource = resource;
            this.state = (int)DisposeLockState.Unlocked;
        }

        public void Dispose() {
            var expected = (int)DisposeLockState.Unlocked;
            var desired = (int)DisposeLockState.Locked;
            var disposed = (int)DisposeLockState.Disposed;
            for (; ; ) {
                var original = Interlocked.CompareExchange(ref this.state, desired, expected);
                if (original == expected) {
                    int new_state;
                    try {
                        resource.Dispose();
                    } catch {
                        // ...
                    } finally {
                        Volatile.Write(ref this.state, desired); // Interlocked.Exchange(ref this.state, disposed);
                    }

                    break;
                } else if (original == disposed) {
                    break;
                } else {
                    for (var spin_wait = new SpinWait(); Volatile.Read(ref this.state) == desired; spin_wait.SpinOnce()) {
                    }
                }
            }
        }
    }

    public readonly partial struct NativeArray<T> {

        public int Length => checked((int)this._Length);

        public long LongLength => checked((long)this._Length);

        public nint NativeLength => checked((nint)this._Length);

        public nuint NativeUnsignedLength => this._Length;
    }

    public readonly partial struct NativeArray<T> {

        public Ptr<T> DangerousGetBasePointer() {
            unsafe {
                return this._Base;
            }
        }
    }

    public readonly partial struct NativeArray<T> {

        public unsafe ref T this[int index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index.ToUnsignedChecked());
                return ref this._Base[index];
            }
        }

        public unsafe ref T this[uint index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index);
                return ref this._Base[index];
            }
        }

        public unsafe ref T this[nint index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index.ToUnsignedChecked());
                return ref this._Base[index];
            }
        }

        public unsafe ref T this[nuint index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index);
                return ref this._Base[index];
            }
        }

        public unsafe ref T this[long index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index.ToUnsignedChecked());
                return ref this._Base[index];
            }
        }

        public unsafe ref T this[ulong index] {

            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Length, index);
                return ref this._Base[index];
            }
        }
    }


    public readonly partial struct NativeArray<T> : IList<T>, IReadOnlyList<T> {
        // implemented from ICollection<TBase>
        public bool IsReadOnly => false;

        int ICollection<T>.Count => Length;

        int IReadOnlyCollection<T>.Count => Length;

        T IReadOnlyList<T>.this[int index] => this[index];

        T IList<T>.this[int index] { get => this[index]; set => this[index] = value; }

        public void Add(T item) {
            throw new NotSupportedException();
        }

        public void Clear() {
            throw new NotSupportedException();
        }

        public bool Contains(T item) {
            return this.IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex) {
            if (array == null) {
                throw new ArgumentNullException(nameof(array));
            }

            if (arrayIndex < 0 || arrayIndex > array.Length) {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < Length) {
                throw new ArgumentException("The number of elements in the source NativeArray<TBase> is greater than the available space from arrayIndex to the end of the destination array.");
            }

            for (var i = 0; i < Length; i++) {
                array[arrayIndex + i] = this[i];
            }
        }

        public bool Remove(T item) {
            throw new NotSupportedException();
        }

        // implemented from IEnumerable<TBase>
        public IEnumerator<T> GetEnumerator() {
            for (int i = 0; i < Length; i++) {
                yield return this[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        // implemented from IList<TBase>
        public int IndexOf(T item) {
            for (int i = 0; i < Length; i++) {
                if (EqualityComparer<T>.Default.Equals(this[i], item)) {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item) {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index) {
            throw new NotSupportedException();
        }
    }


    public readonly partial struct NativeArray<T> {
        // added methods
        public Span<T> AsSpan(int start, int length) {
            if (unchecked((nuint)(nint)start) > this._Length) {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (unchecked((nuint)(nint)length) > unchecked(this._Length - (nuint)(nint)start)) {
                throw new ArgumentOutOfRangeException(nameof(length));
            }
            return this.AsSpanUnchecked(start, length);
        }

        public Span<T> AsSpanUnchecked(int start, int length) {
            unsafe {
                return new Span<T>(this._Base + start, length);
            }
        }
    }


    public struct NativeList<T> : IDisposable where T : unmanaged {
        private NativeArray<T> _Array; // The native array that stores the elements
        private nuint _Count; // The number of elements in the list

        unsafe readonly T* _Data => this._Array._Base;
        nuint _Capacity => this._Array._Length;


        // Constructs a new list with the given initial capacity and allocator
        public NativeList(nuint initialCapacity) {
            _Array = new NativeArray<T>(initialCapacity);
            _Count = 0;
        }

        // Gets or sets the element at the given index
        public unsafe ref T this[int index] {
            get {
                ThrowHelper.ThrowOnLessThanOrEqual(this._Capacity, index.ToUnsignedChecked());
                return ref this._Data[index];
            }
        }

        public int Count => checked((int)this._Count);

        public long LongCount => checked((long)this._Count);

        public nint NativeCount => checked((nint)this._Count);

        public nuint NativeUnsignedCount => this._Count;

        // Gets the capacity of the list
        public int Capacity => _Array.Length;

        // Clears the list
        public void Clear() {
            _Count = 0;
        }

        // Disposes the list and frees the memory
        public void Dispose() {
            _Array.Dispose();
        }
    }

}
