using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace UltimateOrb.Plain.ValueTypes {
    using UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;

    //public partial struct FoldingStack<T, TAccumulate, TFunc, TStack>
    //    : IStack<T>
    //    where TAccumulate : new()
    //    where TFunc : struct, IO.IFunc<TAccumulate, T, TAccumulate>
    //    where TStack : IStack<(T Item, TAccumulate Accumulate)> {

    //    public TStack data;

    //    public TAccumulate default_accumulate;

    //    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    internal FoldingStack(TStack data, TAccumulate default_accumulate) {
    //        this.data = data;
    //        this.default_accumulate = default_accumulate;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public static FoldingStack<T, TAccumulate, TFunc, TStack> Create<TStackConstructor>(int capacity) where TStackConstructor : IO.IFunc<int, TStack>, new() {
    //        return new FoldingStack<T, TAccumulate, TFunc, TStack>(DefaultConstructor.Invoke<TStackConstructor>().Invoke(capacity), DefaultConstructor.Invoke<TAccumulate>());
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public static FoldingStack<T, TAccumulate, TFunc, TStack> Create<TStackConstructor>() where TStackConstructor : IO.IFunc<TStack>, new() {
    //        return new FoldingStack<T, TAccumulate, TFunc, TStack>(DefaultConstructor.Invoke<TStackConstructor>().Invoke(), DefaultConstructor.Invoke<TAccumulate>());
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public static FoldingStack<T, TAccumulate, TFunc, TStack> Create<TStackConstructor>(int capacity, TAccumulate accumulate) where TStackConstructor : IO.IFunc<int, TStack>, new() {
    //        return new FoldingStack<T, TAccumulate, TFunc, TStack>(DefaultConstructor.Invoke<TStackConstructor>().Invoke(capacity), accumulate);
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public static FoldingStack<T, TAccumulate, TFunc, TStack> Create<TStackConstructor>(TAccumulate accumulate) where TStackConstructor : IO.IFunc<TStack>, new() {
    //        return new FoldingStack<T, TAccumulate, TFunc, TStack>(DefaultConstructor.Invoke<TStackConstructor>().Invoke(), accumulate);
    //    }

    //    private TAccumulate accumulate {

    //        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //        get {
    //            var b = this.data;
    //            return b.IsEmpty ? this.default_accumulate : b.Peek().Accumulate;
    //        }
    //    }

    //    public TAccumulate Accumulate {

    //        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //        get => this.accumulate;
    //    }

    //    public bool IsEmpty {

    //        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //        get => this.data.IsEmpty;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public ref T Peek() {
    //        return ref this.data.Peek().Item;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    T Collections.Generic.IStack<T>.Peek() {
    //        return this.data.Peek().Item;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public T Pop() {
    //        return this.data.Pop().Item;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public void Push(T item) {
    //        this.data.Push((item, default(TFunc).Invoke(this.accumulate, item)));
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public bool TryPeek(out T item) {
    //        if (this.data.TryPeek(out var t)) {
    //            item = t.Item;
    //            return true;
    //        }
    //        item = default;
    //        return false;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public bool TryPop(out T item) {
    //        if (this.data.TryPop(out var t)) {
    //            item = t.Item;
    //            return true;
    //        }
    //        item = default;
    //        return false;
    //    }

    //    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
    //    public bool TryPush(T item) {
    //        return this.data.TryPush((item, default(TFunc).Invoke(this.accumulate, item)));
    //    }
    //}
}
