using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;
using UltimateOrb.Utilities;
using Unsafe = System.Runtime.CompilerServices.Unsafe;

namespace UltimateOrb.Collections.Generic {


    public readonly struct NativeArrayCore {


        public static NativeArrayCore<T> CreateNewInstance<T>(nint length) where T : unmanaged {
            return CreateNewInstance<T>(length.ToUnsignedChecked());
        }

        public static NativeArrayCore<T> CreateNewInstance<T>(nuint length) where T : unmanaged {
            unsafe {
                return new NativeArrayCore<T>((T*)NativeMemory.AlignedAlloc(
                    Miscellaneous.MulSaturating(length.ToUnsignedUnchecked(), Unsafe.SizeOf<T>().ToUnsignedUnchecked()),
                    StructLayoutHelpers.AlignOf<T>().ToUnsignedUnchecked()), length.ToSignedUnchecked());
            }
        }
    }

    public readonly struct NativeArrayCore<T> where T : unmanaged {

        internal readonly unsafe T* _ptr;

        internal readonly nint _length;

        public nint Length => _length;

        public unsafe NativeArrayCore(T* ptr, nint length)  {
            _ptr = ptr;
            _length = length;
        }


        ref T this[nint index] {

            get {
                
                return _ptr[index.ToUnsignedChecked()];
            }
        }
    }

    public abstract class NativeList {

    }

    // Implements a variable-size NativeList that uses an array of objects to store the
    // elements. A NativeList has a capacity, which is the allocated length
    // of the internal array. As elements are added to a NativeList, the capacity
    // of the NativeList is automatically increased as required by reallocating the
    // internal array.
    //
    //[DebuggerTypeProxy(typeof(ICollectionDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public partial class NativeList<T> : NativeList, IList<T>, IList, IReadOnlyList<T>
        where T : unmanaged {

        private const int DefaultCapacity = 4;

        internal NativeArrayCore<T> _items; // Do not rename (binary serialization) [cDAC] [ComWrappers] : Contract depends on this exact name
        internal nint _size; // Do not rename (binary serialization) [cDAC] [ComWrappers] : Contract depends on this exact name
        
        // internal int _version; // Do not rename (binary serialization)

#pragma warning disable CA1825, IDE0300 // avoid the extra generic instantiation for Array.Empty<T>()
        private unsafe static readonly NativeArrayCore<T> s_emptyArray = NativeArrayCore.CreateNewInstance<T>(0);
#pragma warning restore CA1825, IDE0300

        // Constructs a NativeList. The list is initially empty and has a capacity
        // of zero. Upon adding the first element to the list the capacity is
        // increased to DefaultCapacity, and then increased in multiples of two
        // as required.
        public NativeList() {
            unsafe {
                _items = s_emptyArray;
            }
        }

        // Constructs a NativeList with a given initial capacity. The list is
        // initially empty, but will have room for the given number of elements
        // before any reallocations are required.
        //
        public NativeList(nint capacity) {
            if (capacity < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);

            if (capacity == 0)
                unsafe {
                    _items = s_emptyArray;
                }
            else
                unsafe {
                    _items = NativeArrayCore.CreateNewInstance<T>(capacity.ToUnsignedUnchecked());
                }
        }

        // Constructs a NativeList, copying the contents of the given collection. The
        // size and capacity of the new list will both be equal to the size of the
        // given collection.
        //
        public NativeList(IEnumerable<T> collection) {
            if (collection == null)
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);

            unsafe {
                /*if (collection is ICollection<T> c) {
                    int count = c.Count;
                    if (count == 0) {
                        _ptr = s_emptyArray;
                    } else {
                        _ptr = CreateNewArray(count);
                        c.CopyTo(_ptr, 0);
                        _size = count;
                    }
                } else */{
                    _items = s_emptyArray;
                    using (IEnumerator<T> en = collection.GetEnumerator()) {
                        while (en.MoveNext()) {
                            Add(en.Current);
                        }
                    }
                }
            }
        }

        // Gets and sets the capacity of this list.  The capacity is the size of
        // the internal array used to hold items.  When set, the internal
        // array of the list is reallocated to the given capacity.
        //
        public nint Capacity {
            get => _items.Length;
            set {
                if (value < _size) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
                }

                if (value != _items.Length) {
                    if (value > 0) {
                        var newItems = NativeArrayCore.CreateNewInstance<T>(value.ToUnsignedUnchecked());
                        if (_size > 0) {
                            NativeArrayCore.Copy(_items, newItems, _size);
                        }
                        _items = newItems;
                    } else {
                        _items = s_emptyArray;
                    }
                }
            }
        }

        // Read-only property describing how many elements are in the NativeList.
        public nint Count => _size;

        bool IList.IsFixedSize => false;

        // Is this NativeList read-only?
        bool ICollection<T>.IsReadOnly => false;

        bool IList.IsReadOnly => false;

        // Is this NativeList synchronized (thread-safe)?
        bool ICollection.IsSynchronized => false;

        // Synchronization root for this object.
        object ICollection.SyncRoot => this;

        // Sets or Gets the element at the given index.
        public T this[nint index] {
            get {
                // Following trick can reduce the range check by one
                if ((uint)index >= (uint)_size) {
                    ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
                }
                return _items[index];
            }

            set {
                if ((uint)index >= (uint)_size) {
                    ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
                }
                _items[index] = value;
                _version++;
            }
        }

        private static bool IsCompatibleObject(object? value) {
            // Non-null values are fine.  Only accept nulls if T is a class or Nullable<U>.
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
            return (value is T) || (value == null && default(T) == null);
        }

        object? IList.this[int index] {
            get => this[index];
            set {
                ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);

                try {
                    this[index] = (T)value!;
                } catch (InvalidCastException) {
                    ThrowHelper.ThrowWrongValueTypeArgumentException(value, typeof(T));
                }
            }
        }

        // Adds the given object to the end of this list. The size of the list is
        // increased by one. If required, the capacity of the list is doubled
        // before adding the new element.
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item) {
            _version++;
            T[] array = _items;
            int size = _size;
            if ((uint)size < (uint)array.Length) {
                _size = size + 1;
                array[size] = item;
            } else {
                AddWithResize(item);
            }
        }

        // Non-inline from NativeList.Add to improve its code quality as uncommon path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddWithResize(T item) {
            Debug.Assert(_size == _items.Length);
            int size = _size;
            Grow(size + 1);
            _size = size + 1;
            _items[size] = item;
        }

        int IList.Add(object? item) {
            ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);

            try {
                Add((T)item!);
            } catch (InvalidCastException) {
                ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
            }

            return Count - 1;
        }

        // Adds the elements of the given collection to the end of this list. If
        // required, the capacity of the list is increased to twice the previous
        // capacity or the new size, whichever is larger.
        //
        public void AddRange(IEnumerable<T> collection) {
            if (collection == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            }

            if (collection is ICollection<T> c) {
                int count = c.Count;
                if (count > 0) {
                    if (_items.Length - _size < count) {
                        Grow(checked(_size + count));
                    }

                    c.CopyTo(_items, _size);
                    _size += count;
                    _version++;
                }
            } else {
                using (IEnumerator<T> en = collection.GetEnumerator()) {
                    while (en.MoveNext()) {
                        Add(en.Current);
                    }
                }
            }
        }

        public ReadOnlyCollection<T> AsReadOnly()
            => new ReadOnlyCollection<T>(this);

        // Searches a section of the list for a given element using a binary search
        // algorithm. Elements of the list are compared to the search value using
        // the given IComparer interface. If comparer is null, elements of
        // the list are compared to the search value using the IComparable
        // interface, which in that case must be implemented by all elements of the
        // list and the given search value. This method assumes that the given
        // section of the list is already sorted; if this is not the case, the
        // result will be incorrect.
        //
        // The method returns the index of the given value in the list. If the
        // list does not contain the given value, the method returns a negative
        // integer. The bitwise complement operator (~) can be applied to a
        // negative result to produce the index of the first element (if any) that
        // is larger than the given search value. This is also the index at which
        // the search value should be inserted into the list in order for the list
        // to remain sorted.
        //
        // The method uses the Array.BinarySearch method to perform the
        // search.
        //
        public int BinarySearch(int index, int count, T item, IComparer<T>? comparer) {
            if (index < 0)
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            if (count < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            if (_size - index < count)
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);

            return NativeArrayCore.BinarySearch(_items, index, count, item, comparer);
        }

        public int BinarySearch(T item)
            => BinarySearch(0, Count, item, null);

        public int BinarySearch(T item, IComparer<T>? comparer)
            => BinarySearch(0, Count, item, comparer);

        // Clears the contents of NativeList.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() {
            //_version++;
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                int size = _size;
                _size = 0;
                if (size > 0) {
                    Array.Clear(_items, 0, size); // Clear the elements so that the gc can reclaim the references.
                }
            } else {
                _size = 0;
            }
        }

        // Contains returns true if the specified element is in the NativeList.
        // It does a linear, O(n) search.  Equality is determined by calling
        // EqualityComparer<T>.Default.Equals().
        //
        public bool Contains(T item) {
            // PERF: IndexOf calls Array.IndexOf, which internally
            // calls EqualityComparer<T>.Default.IndexOf, which
            // is specialized for different types. This
            // boosts performance since instead of making a
            // virtual method call each iteration of the loop,
            // via EqualityComparer<T>.Default.Equals, we
            // only make one virtual call to EqualityComparer.IndexOf.

            return _size != 0 && IndexOf(item) >= 0;
        }

        bool IList.Contains(object? item) {
            if (IsCompatibleObject(item)) {
                return Contains((T)item!);
            }
            return false;
        }

        public NativeList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) {
            if (converter == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
            }

            NativeList<TOutput> list = new NativeList<TOutput>(_size);
            for (int i = 0; i < _size; i++) {
                list._items[i] = converter(_items[i]);
            }
            list._size = _size;
            return list;
        }

        // Copies this NativeList into array, which must be of a
        // compatible array type.
        public void CopyTo(T[] array)
            => CopyTo(array, 0);

        // Copies this NativeList into array, which must be of a
        // compatible array type.
        void ICollection.CopyTo(Array array, int arrayIndex) {
            if ((array != null) && (array.Rank != 1)) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
            }

            try {
                // Array.Copy will check for NULL.
                Array.Copy(_items, 0, array!, arrayIndex, _size);
            } catch (ArrayTypeMismatchException) {
                ThrowHelper.ThrowArgumentException_Argument_IncompatibleArrayType();
            }
        }

        // Copies a section of this list to the given array at the given index.
        //
        // The method uses the Array.Copy method to copy the elements.
        //
        public void CopyTo(int index, T[] array, int arrayIndex, int count) {
            if (_size - index < count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            }

            // Delegate rest of error checking to Array.Copy.
            Array.Copy(_items, index, array, arrayIndex, count);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            // Delegate rest of error checking to Array.Copy.
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        /// <summary>
        /// Ensures that the capacity of this list is at least the specified <paramref name="capacity"/>.
        /// If the current capacity of the list is less than specified <paramref name="capacity"/>,
        /// the capacity is increased to at least <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        /// <returns>The new capacity of this list.</returns>
        public int EnsureCapacity(int capacity) {
            if (capacity < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }
            if (_items.Length < capacity) {
                Grow(capacity);
            }

            return _items.Length;
        }

        /// <summary>
        /// Increase the capacity of this list to at least the specified <paramref name="capacity"/>.
        /// </summary>
        /// <param name="capacity">The minimum capacity to ensure.</param>
        internal void Grow(int capacity) {
            Capacity = GetNewCapacity(capacity);
        }

        /// <summary>
        /// Enlarge this list so it may contain at least <paramref name="insertionCount"/> more elements
        /// And copy data to their after-insertion positions.
        /// This method is specifically for insertion, as it avoids 1 extra array copy.
        /// You should only call this method when Count + insertionCount > Capacity.
        /// </summary>
        /// <param name="indexToInsert">Index of the first insertion.</param>
        /// <param name="insertionCount">How many elements will be inserted.</param>
        internal void GrowForInsertion(int indexToInsert, int insertionCount = 1) {
            Debug.Assert(insertionCount > 0);

            int requiredCapacity = checked(_size + insertionCount);
            int newCapacity = GetNewCapacity(requiredCapacity);

            // Inline and adapt logic from set_Capacity

            T[] newItems = new T[newCapacity];
            if (indexToInsert != 0) {
                Array.Copy(_items, newItems, length: indexToInsert);
            }

            if (_size != indexToInsert) {
                Array.Copy(_items, indexToInsert, newItems, indexToInsert + insertionCount, _size - indexToInsert);
            }

            _items = newItems;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetNewCapacity(int capacity) {
            Debug.Assert(_items.Length < capacity);

            int newCapacity = _items.Length == 0 ? DefaultCapacity : 2 * _items.Length;

            // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
            // Note that this check works even when _ptr.Length overflowed thanks to the (uint) cast
            if ((uint)newCapacity > Array.MaxLength) newCapacity = Array.MaxLength;

            // If the computed capacity is still less than specified, set to the original argument.
            // Capacities exceeding Array.MaxLength will be surfaced as OutOfMemoryException by Array.Resize.
            if (newCapacity < capacity) newCapacity = capacity;

            return newCapacity;
        }

        public bool Exists(Predicate<T> match)
            => FindIndex(match) != -1;

        public T? Find(Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            for (int i = 0; i < _size; i++) {
                if (match(_items[i])) {
                    return _items[i];
                }
            }
            return default;
        }

        public NativeList<T> FindAll(Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            NativeList<T> list = new NativeList<T>();
            for (int i = 0; i < _size; i++) {
                if (match(_items[i])) {
                    list.Add(_items[i]);
                }
            }
            return list;
        }

        public int FindIndex(Predicate<T> match)
            => FindIndex(0, _size, match);

        public int FindIndex(int startIndex, Predicate<T> match)
            => FindIndex(startIndex, _size - startIndex, match);

        public int FindIndex(int startIndex, int count, Predicate<T> match) {
            if ((uint)startIndex > (uint)_size) {
                ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLessOrEqual();
            }

            if (count < 0 || startIndex > _size - count) {
                ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
            }

            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            int endIndex = startIndex + count;
            for (int i = startIndex; i < endIndex; i++) {
                if (match(_items[i])) return i;
            }
            return -1;
        }

        public T? FindLast(Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            for (int i = _size - 1; i >= 0; i--) {
                if (match(_items[i])) {
                    return _items[i];
                }
            }
            return default;
        }

        public int FindLastIndex(Predicate<T> match)
            => FindLastIndex(_size - 1, _size, match);

        public int FindLastIndex(int startIndex, Predicate<T> match)
            => FindLastIndex(startIndex, startIndex + 1, match);

        public int FindLastIndex(int startIndex, int count, Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            if (_size == 0) {
                // Special case for 0 length NativeList
                if (startIndex != -1) {
                    ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess();
                }
            } else {
                // Make sure we're not out of range
                if ((uint)startIndex >= (uint)_size) {
                    ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess();
                }
            }

            // 2nd have of this also catches when startIndex == MAXINT, so MAXINT - 0 + 1 == -1, which is < 0.
            if (count < 0 || startIndex - count + 1 < 0) {
                ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
            }

            int endIndex = startIndex - count;
            for (int i = startIndex; i > endIndex; i--) {
                if (match(_items[i])) {
                    return i;
                }
            }
            return -1;
        }

        public void ForEach(Action<T> action) {
            if (action == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
            }

            int version = _version;

            for (int i = 0; i < _size; i++) {
                if (version != _version) {
                    break;
                }
                action(_items[i]);
            }

            if (version != _version)
                ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        }

        // Returns an enumerator for this list with the given
        // permission for removal of elements. If modifications made to the list
        // while an enumeration is in progress, the MoveNext and
        // GetObject methods of the enumerator will throw an exception.
        //
        public Enumerator GetEnumerator() => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator() =>
            Count == 0 ? SZGenericArrayEnumerator<T>.Empty :
            GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<T>)this).GetEnumerator();

        public NativeList<T> GetRange(int index, int count) {
            if (index < 0) {
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            }

            if (count < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (_size - index < count) {
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
            }

            NativeList<T> list = new NativeList<T>(count);
            Array.Copy(_items, index, list._items, 0, count);
            list._size = count;
            return list;
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="NativeList{T}" />.
        /// </summary>
        /// <param name="start">The zero-based <see cref="NativeList{T}" /> index at which the range starts.</param>
        /// <param name="length">The length of the range.</param>
        /// <returns>A shallow copy of a range of elements in the source <see cref="NativeList{T}" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="start" /> is less than 0.
        /// -or-
        /// <paramref name="length" /> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="start" /> and <paramref name="length" /> do not denote a valid range of elements in the <see cref="NativeList{T}" />.</exception>
        public NativeList<T> Slice(int start, int length) => GetRange(start, length);

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards from beginning to end.
        // The elements of the list are compared to the given value using the
        // Object.Equals method.
        //
        // This method uses the Array.IndexOf method to perform the
        // search.
        //
        public int IndexOf(T item)
            => Array.IndexOf(_items, item, 0, _size);

        int IList.IndexOf(object? item) {
            if (IsCompatibleObject(item)) {
                return IndexOf((T)item!);
            }
            return -1;
        }

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards, starting at index
        // index and ending at count number of elements. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        //
        // This method uses the Array.IndexOf method to perform the
        // search.
        //
        public int IndexOf(T item, int index) {
            if (index > _size)
                ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();
            return Array.IndexOf(_items, item, index, _size - index);
        }

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards, starting at index
        // index and upto count number of elements. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        //
        // This method uses the Array.IndexOf method to perform the
        // search.
        //
        public int IndexOf(T item, int index, int count) {
            if (index > _size)
                ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();

            if (count < 0 || index > _size - count)
                ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();

            return Array.IndexOf(_items, item, index, count);
        }

        // Inserts an element into this list at a given index. The size of the list
        // is increased by one. If required, the capacity of the list is doubled
        // before inserting the new element.
        //
        public void Insert(int index, T item) {
            // Note that insertions at the end are legal.
            if ((uint)index > (uint)_size) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
            }
            if (_size == _items.Length) {
                GrowForInsertion(index, 1);
            } else if (index < _size) {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            _version++;
        }

        void IList.Insert(int index, object? item) {
            ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);

            try {
                Insert(index, (T)item!);
            } catch (InvalidCastException) {
                ThrowHelper.ThrowWrongValueTypeArgumentException(item, typeof(T));
            }
        }

        // Inserts the elements of the given collection at a given index. If
        // required, the capacity of the list is increased to twice the previous
        // capacity or the new size, whichever is larger.  Ranges may be added
        // to the end of the list by setting index to the NativeList's size.
        //
        public void InsertRange(int index, IEnumerable<T> collection) {
            if (collection == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
            }

            if ((uint)index > (uint)_size) {
                ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();
            }

            if (collection is ICollection<T> c) {
                int count = c.Count;
                if (count > 0) {
                    if (_items.Length - _size < count) {
                        GrowForInsertion(index, count);
                    } else if (index < _size) {
                        Array.Copy(_items, index, _items, index + count, _size - index);
                    }

                    // If we're inserting a NativeList into itself, we want to be able to deal with that.
                    if (this == c) {
                        // Copy first part of _ptr to insert location
                        Array.Copy(_items, 0, _items, index, index);
                        // Copy last part of _ptr back to inserted location
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    } else {
                        c.CopyTo(_items, index);
                    }
                    _size += count;
                    _version++;
                }
            } else {
                using (IEnumerator<T> en = collection.GetEnumerator()) {
                    while (en.MoveNext()) {
                        Insert(index++, en.Current);
                    }
                }
            }
        }

        // Returns the index of the last occurrence of a given value in a range of
        // this list. The list is searched backwards, starting at the end
        // and ending at the first element in the list. The elements of the list
        // are compared to the given value using the Object.Equals method.
        //
        // This method uses the Array.LastIndexOf method to perform the
        // search.
        //
        public int LastIndexOf(T item) {
            if (_size == 0) {  // Special case for empty list
                return -1;
            } else {
                return LastIndexOf(item, _size - 1, _size);
            }
        }

        // Returns the index of the last occurrence of a given value in a range of
        // this list. The list is searched backwards, starting at index
        // index and ending at the first element in the list. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        //
        // This method uses the Array.LastIndexOf method to perform the
        // search.
        //
        public int LastIndexOf(T item, int index) {
            if (index >= _size)
                ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
            return LastIndexOf(item, index, index + 1);
        }

        // Returns the index of the last occurrence of a given value in a range of
        // this list. The list is searched backwards, starting at index
        // index and upto count elements. The elements of
        // the list are compared to the given value using the Object.Equals
        // method.
        //
        // This method uses the Array.LastIndexOf method to perform the
        // search.
        //
        public int LastIndexOf(T item, int index, int count) {
            if ((Count != 0) && (index < 0)) {
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            }

            if ((Count != 0) && (count < 0)) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (_size == 0) {  // Special case for empty list
                return -1;
            }

            if (index >= _size) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
            }

            if (count > index + 1) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
            }

            return Array.LastIndexOf(_items, item, index, count);
        }

        // Removes the first occurrence of the given element, if found.
        // The size of the list is decreased by one if successful.
        public bool Remove(T item) {
            int index = IndexOf(item);
            if (index >= 0) {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        void IList.Remove(object? item) {
            if (IsCompatibleObject(item)) {
                Remove((T)item!);
            }
        }

        // This method removes all items which matches the predicate.
        // The complexity is O(n).
        public int RemoveAll(Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            int freeIndex = 0;   // the first free slot in items array

            // Find the first item which needs to be removed.
            while (freeIndex < _size && !match(_items[freeIndex])) freeIndex++;
            if (freeIndex >= _size) return 0;

            int current = freeIndex + 1;
            while (current < _size) {
                // Find the first item which needs to be kept.
                while (current < _size && match(_items[current])) current++;

                if (current < _size) {
                    // copy item to the free slot.
                    _items[freeIndex++] = _items[current++];
                }
            }

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                Array.Clear(_items, freeIndex, _size - freeIndex); // Clear the elements so that the gc can reclaim the references.
            }

            int result = _size - freeIndex;
            _size = freeIndex;
            _version++;
            return result;
        }

        // Removes the element at the given index. The size of the list is
        // decreased by one.
        public void RemoveAt(int index) {
            if ((uint)index >= (uint)_size) {
                ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
            }
            _size--;
            if (index < _size) {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                _items[_size] = default!;
            }
            _version++;
        }

        // Removes a range of elements from this list.
        public void RemoveRange(int index, int count) {
            if (index < 0) {
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            }

            if (count < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (_size - index < count)
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);

            if (count > 0) {
                _size -= count;
                if (index < _size) {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }

                _version++;
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    Array.Clear(_items, _size, count);
                }
            }
        }

        // Reverses the elements in this list.
        public void Reverse()
            => Reverse(0, Count);

        // Reverses the elements in a range of this list. Following a call to this
        // method, an element in the range given by index and count
        // which was previously located at index i will now be located at
        // index index + (index + count - i - 1).
        //
        public void Reverse(int index, int count) {
            if (index < 0) {
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            }

            if (count < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (_size - index < count)
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);

            if (count > 1) {
                Array.Reverse(_items, index, count);
            }
            _version++;
        }

        // Sorts the elements in this list.  Uses the default comparer and
        // Array.Sort.
        public void Sort()
            => Sort(0, Count, null);

        // Sorts the elements in this list.  Uses Array.Sort with the
        // provided comparer.
        public void Sort(IComparer<T>? comparer)
            => Sort(0, Count, comparer);

        // Sorts the elements in a section of this list. The sort compares the
        // elements to each other using the given IComparer interface. If
        // comparer is null, the elements are compared to each other using
        // the IComparable interface, which in that case must be implemented by all
        // elements of the list.
        //
        // This method uses the Array.Sort method to sort the elements.
        //
        public void Sort(int index, int count, IComparer<T>? comparer) {
            if (index < 0) {
                ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
            }

            if (count < 0) {
                ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
            }

            if (_size - index < count)
                ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);

            if (count > 1) {
                Array.Sort(_items, index, count, comparer);
            }
            //_version++;
        }

        public void Sort(Comparison<T> comparison) {
            if (comparison == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparison);
            }

            if (_size > 1) {
                ArraySortHelper<T>.Sort(new Span<T>(_items, 0, _size), comparison);
            }
            //_version++;
        }

        // ToArray returns an array containing the contents of the NativeList.
        // This requires copying the NativeList, which is an O(n) operation.
        public T[] ToArray() {
            if (_size == 0) {
                return s_emptyArray;
            }

            T[] array = new T[_size];
            Array.Copy(_items, array, _size);
            return array;
        }

        // Sets the capacity of this list to the size of the list. This method can
        // be used to minimize a list's memory overhead once it is known that no
        // new elements will be added to the list. To completely clear a list and
        // release all memory referenced by the list, execute the following
        // statements:
        //
        // list.Clear();
        // list.TrimExcess();
        //
        public void TrimExcess() {
            int threshold = (int)(((double)_items.Length) * 0.9);
            if (_size < threshold) {
                Capacity = _size;
            }
        }

        public bool TrueForAll(Predicate<T> match) {
            if (match == null) {
                ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
            }

            for (int i = 0; i < _size; i++) {
                if (!match(_items[i])) {
                    return false;
                }
            }
            return true;
        }

        public struct Enumerator : IEnumerator<T>, IEnumerator {
            private readonly NativeList<T> _list;
            // private readonly int _version;

            private int _index;
            private T? _current;

            internal Enumerator(NativeList<T> list) {
                _list = list;
                // _version = list._version;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                NativeList<T> localList = _list;

                // if (_version != _list._version) {
                //     ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
                // }

                if ((uint)_index < (uint)localList._size) {
                    _current = localList._items[_index];
                    _index++;
                    return true;
                }

                _current = default;
                _index = -1;
                return false;
            }

            public T Current => _current!;

            object? IEnumerator.Current {
                get {
                    if (_index <= 0) {
                        ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
                    }

                    return _current;
                }
            }

            void IEnumerator.Reset() {
                // if (_version != _list._version) {
                //     ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
                // }

                _index = 0;
                _current = default;
            }
        }
    }
}