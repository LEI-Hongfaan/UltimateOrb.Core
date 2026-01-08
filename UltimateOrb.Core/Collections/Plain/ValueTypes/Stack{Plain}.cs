using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using IO = UltimateOrb;

namespace UltimateOrb.Collections.Plain.ValueTypes {
    using UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;

    /// <summary>
    ///     <para>Represents a variable size last-in-first-out (LIFO) collection of instances of the same specified type. This type is a value type.</para>
    /// </summary>
    /// <typeparam name="T">
    ///     <para>Specifies the type of elements in the stack.</para>
    /// </typeparam>
    /// <remarks>
    ///     <para>Value assignments of the <see cref="Stack{T}"/> have move semantics.</para>
    /// </remarks>
    public partial struct Stack<T>
        : IEnumerable<T, Stack<T>.Enumerator>
        , IInitializable, IInitializable<int> {

        public T[] m_buffer;

        public int m_count;

        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Stack{T}"/> type that is empty and has an initial capacity at least the value specified.</para>
        /// </summary>
        /// <param name="capacity">
        ///     <para>The initial number of elements that the <see cref="Stack{T}"/> can contain.</para>
        /// </param>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Stack(int capacity) {
            this.m_buffer = capacity > 0 ? Array_Empty<T>.Value : new T[capacity];
            this.m_count = 0;
            return;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Stack(T[] buffer, int count) {
            this.m_buffer = buffer;
            this.m_count = count;
        }

        /// <summary>
        ///     <para>Gets the number of elements contained in the <see cref="Stack{T}"/>.</para>
        /// </summary>
        /// <returns>
        ///     <para>The number of elements contained in the <see cref="Stack{T}"/>.</para>
        /// </returns>
        /// <exception cref="OverflowException">
        ///     <para>The result can not be represented in the result type.</para>
        /// </exception>
        public int Count {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get {
                return checked((int)this.m_count);
            }
        }

        public bool IsEmpty {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => 0 == this.m_count;
        }

        public bool IsFinite {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        /// <summary>
        ///     <para>Gets the number of elements contained in the <see cref="Stack{T}"/>.</para>
        /// </summary>
        /// <returns>
        ///     <para>The number of elements contained in the <see cref="Stack{T}"/>.</para>
        /// </returns>
        public long LongCount {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get {
                return this.m_count;
            }
        }

        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Stack{T}"/> type that is empty and has the default initial capacity.</para>
        /// </summary>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Stack<T> Create() {
            return new Stack<T>(20);
        }

        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Stack{T}"/> type that is empty and has an initial capacity at least the value specified.</para>
        /// </summary>
        /// <param name="capacity">
        ///     <para>The initial number of elements that the <see cref="Stack{T}"/> can contain.</para>
        /// </param>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <remarks>
        ///     <para>A dummy value is returned if <paramref name="capacity"/> is negative.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Stack<T> Create(int capacity) {
            return new Stack<T>(capacity);
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        /// <summary>
        ///     <para>Increases the total number of elements the internal data structure can hold without resizing.</para>
        /// </summary>
        /// <exception cref="OverflowException">
        ///     <para>The resulting number of elements contained in the <see cref="Stack{T}"/> can not be represented in the internal data type. -or- There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void IncreaseCapacity() {
            Array.Resize(ref this.m_buffer, checked((int)(22 + 1.6180339887498948482045868343656 * this.m_count)));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Initialize() {
            var default_capacity = 4;
            this.Initialize(default_capacity);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Initialize(int capacity) {
            this = new Stack<T>(capacity);
        }

        /// <summary>
        ///     <para>Removes all objects from the <see cref="Stack{T}"/>.</para>
        /// </summary>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Clear() {
            if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                this.m_count = 0;
                return;
            }
            var count = this.m_count;
            this.m_count = 0;
            this.m_buffer.AsSpan(count).Clear();
        }

        /// <summary>
        ///     <para>Returns the object at the top of the <see cref="Stack{T}"/> without removing it.</para>
        /// </summary>
        /// <returns>
        ///     <para>The object at the top of the <see cref="Stack{T}"/>.</para>
        /// </returns>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref T Peek() {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                return ref this.m_buffer[a];
            }
            // TODO
            throw new InvalidOperationException();
        }

        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        //TBase IStack<TBase>.Peek() {
        //    var a = unchecked(this.m_count - 1);
        //    if (0 <= a) {
        //        return this.m_buffer[a];
        //    }
        //    // TODO
        //    throw new InvalidOperationException();
        //}

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public T PeekPop() {
            return this.Pop();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public T PeekPopPush(T item) {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                var b = this.m_buffer;
                var result = b[a];
                b[a] = item;
                return result;
            }
            // TODO
            throw new InvalidOperationException();
        }

        /// <summary>
        ///     <para>Removes and returns the object at the top of the <see cref="Stack{T}"/>.</para>
        /// </summary>
        /// <returns>
        ///     <para>The object removed from the top of the <see cref="Stack{T}"/>.</para>
        /// </returns>
        /// <remarks>
        ///     <para>A dummy value is returned if the stack is empty. The value can be null for reference types.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public T Pop() {
            var a = this.m_count;
            if (0 < a) {
                unchecked {
                    --a;
                }
                this.m_count = a;
                return this.m_buffer[a];
            }
            // TODO
            throw new InvalidOperationException();
        }

        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        //void Collections.Generic.IStack_A1<TBase>.Pop() {
        //    var a = this.m_count;
        //    if (0 < a) {
        //        unchecked {
        //            --a;
        //        }
        //        this.m_count = a;
        //        return;
        //    }
        //    // TODO
        //    throw new InvalidOperationException();
        //}

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void PopPush(T item) {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                this.m_buffer[a] = item;
            }
            // TODO
            throw new InvalidOperationException();
        }

        /// <summary>
        ///     <para>Inserts an object at the top of the <see cref="Stack{T}"/>.</para>
        /// </summary>
        /// <param name="item">
        ///     <para>The object to push onto the <see cref="Stack{T}"/>. The value can be null for reference types.</para>
        /// </param>
        /// <exception cref="OverflowException">
        ///     <para>The resulting number of elements contained in the <see cref="Stack{T}"/> can not be represented in the internal data type. -or- There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void Push(T item) {
            var @this = this;
            var c = checked(@this.m_count + 1);
            if (null == @this.m_buffer || c > @this.m_buffer.Length) {
                @this.IncreaseCapacity();
                @this.m_buffer![unchecked(c - 1)] = item;
                this.m_buffer = @this.m_buffer;
                this.m_count = c;
                return;
            }
            {
                @this.m_buffer[unchecked(c - 1)] = item;
                this.m_count = c;
                return;
            }
        }

        /// <summary>
        ///     <para>Inserts a dummy value of type <typeparamref name="T"/> at the top of <see cref="Stack{T}"/>.</para>
        /// </summary>
        /// <returns>
        ///     <para>A value of type ref <typeparamref name="T"/> (managed pointer to type <typeparamref name="T"/>) can be used to store the object being insert.</para>
        /// </returns>
        /// <exception cref="OverflowException">
        ///     <para>The resulting number of elements contained in the <see cref="Stack{T}"/> can not be represented in the internal data type. -or- There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <remarks>
        ///     <para>Must store the object through the return value before any subsequent modification to the collection.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref T Push() {
            var @this = this;
            var c = checked(@this.m_count + 1);
            if (null == @this.m_buffer || c > @this.m_buffer.Length) {
                @this.IncreaseCapacity();
                this.m_buffer = @this.m_buffer!;
            }
            this.m_count = c;
            return ref @this.m_buffer![@this.m_count];
        }

        //[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        //public Stack<TBase> Select() => this.Select<TBase, MoveFunctor>();

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Stack<TResult> Select<TResult, TSelector>(TSelector selector) where TSelector : IO.IFunc<T, TResult> {
            var c = this.m_count;
            var b = this.m_buffer;
            var a = new TResult[c];
            for (var i = 0; c > i && this.m_buffer.Length > i; ++i) {
                a[i] = selector.Invoke(b[i]);
            }
            return new Stack<TResult>(a, c);
        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        //public Stack<TResult> Select<TResult, TSelector>() where TSelector : IO.IFunc<TBase, TResult>, new() => this.Select<TResult, TSelector>(DefaultConstructor.Invoke<TSelector>());

        /// <summary>
        ///     <para>Sets the total number of elements the internal data structure can hold without resizing.</para>
        /// </summary>
        /// <param name="capacity">
        ///     <para>The new capacity.</para>
        /// </param>
        /// <exception cref="OutOfMemoryException">
        ///     <para>There is insufficient memory to satisfy the request.</para>
        /// </exception>
        /// <remarks>
        ///     <para>Sets this instance to dummy value if <paramref name="capacity"/> is negative.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public void SetCapacity(int capacity) {
            ref var buffer = ref this.m_buffer;
            if (0 <= capacity) {
                if (capacity > 0) {
                    Array.Resize(ref this.m_buffer, capacity);
                } else {
                    this.m_buffer = Array_Empty<T>.Value;
                }
                if (capacity < this.m_count) {
                    this.m_count = capacity;
                }
                return;
            }
            this.m_buffer = null!;
            this.m_count = 0;
        }

        // [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => this.m_buffer[..this.m_count];

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public TResult[] ToArray<TResult, TSelector>(TSelector selector) where TSelector : IO.IFunc<T, TResult> {
            var a = this.m_buffer;
            if (null != a) {
                var c = this.m_count;
                var r = new TResult[c];
                {
                    var i = c;
                    var j = 0;
                    for (; a.Length > j && i > 0;) {
                        r[--i] = selector.Invoke(a[j++]);
                    }
                }
                return r;
            }
            throw (NullReferenceException)null!;
        }

        //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        //public TResult[] ToArray<TResult, TSelector>() where TSelector : IO.IFunc<TBase, TResult>, new() => this.ToArray<TResult, TSelector>(DefaultConstructor.Invoke<TSelector>());

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryInitialize() {
            var default_capacity = 4;
            return this.TryInitialize(default_capacity);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryInitialize(int capacity) {
            var buffer = (T[])null!;
            buffer = GetBuffer(capacity);
            if (null != buffer) {
                this.m_buffer = buffer;
                this.m_count = 0;
                return true;
            }
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPeek(out T item) {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                item = this.m_buffer[a];
                return true;
            }
            Miscellaneous.IgnoreOutParameter(out item);
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekPop(out T result) {
            var a = this.m_count;
            if (0 < a) {
                unchecked {
                    --a;
                }
                this.m_count = a;
                result = this.m_buffer[a];
                return true;
            }
            Miscellaneous.IgnoreOutParameter(out result);
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPeekPopPush(T item, out T result) {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                var b = this.m_buffer;
                var result0 = b[a];
                b[a] = item;
                result = result0;
                return true;
            }
            Miscellaneous.IgnoreOutParameter(out result);
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPop(out T item) {
            var a = this.m_count;
            if (a > 0) {
                unchecked {
                    --a;
                }
                this.m_count = a;
                item = this.m_buffer[a];
                return true;
            }
            Miscellaneous.IgnoreOutParameter(out item);
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPop() {
            var a = this.m_count;
            if (0 < a) {
                unchecked {
                    --a;
                }
                this.m_count = a;
                return true;
            }
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPopPush(T item) {
            var a = unchecked(this.m_count - 1);
            if (0 <= a) {
                this.m_buffer[a] = item;
                return true;
            }
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool TryPush(T item) {
            var c = checked(this.m_count + 1);
            var buffer = this.m_buffer;
            if (null == buffer || c > buffer.Length) {
                try {
                    this.IncreaseCapacity();
                } catch (Exception) {
                    return false;
                }
                this.m_buffer[unchecked(c - 1)] = item;
                this.m_count = c;
                return true;
            }
            buffer[unchecked(c - 1)] = item;
            this.m_count = c;
            return true;
        }
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static T[] GetBuffer(int capacity) {
            try {
                return capacity <= 0 ? Array_Empty<T>.Value : new T[capacity];
            } catch (OutOfMemoryException) {
                return null!;
            }
        }


        private partial struct MoveFunctor : IO.IFunc<T, T> {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public T Invoke(T arg) {
                return arg;
            }
        }
    }
}
