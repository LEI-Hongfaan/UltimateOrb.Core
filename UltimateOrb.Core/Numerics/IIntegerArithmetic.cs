using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Functional.DataTypes;

namespace UltimateOrb.Numerics {
    using UltimateOrb.Runtime.CompilerServices;


    interface IDoubleReader<T, TDouble> {

        void GetLow(in TDouble @this, out T value);

        void GetHigh(in TDouble @this, out T value);

        void SetLow(ref TDouble @this, in T value);

        void SetHigh(ref TDouble @this, in T value);
    }

    public struct Double<T> : IUnsafeDouble<T> {

        T lo;

        T hi;

        ref T IUnsafeDouble<T>.Low {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ILMethodBody(@"
                LdArg.0
                Ret
            ")]
            get => throw null!;
        }

        ref T IUnsafeDouble<T>.High {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [ILMethodBody(@"
                LdArg.0
                SizeOf !0
                Add
                Ret
            ")]
            get => throw null!;
        }
    }

    public interface IUnsafeDouble<T>
        : IReadOnlyUnsafeDouble<T> {

        new ref T Low {

            get;
        }

        new ref T High {

            get;
        }

        ref readonly T IReadOnlyUnsafeDouble<T>.Low {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Low;
        }

        ref readonly T IReadOnlyUnsafeDouble<T>.High {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref High;
        }
    }

    public interface IReadOnlyUnsafeDouble<T> {

        ref readonly T Low {

            get;
        }

        ref readonly T High {

            get;
        }
    }

    //public interface IDouble<T>
    //    : IReadOnlyDouble<T>
    //    , IWriteOnlyDouble<T> {
    //}

    //public interface IWriteOnlyDouble<T> {

    //    void SetLow(in T value);

    //    void SetHigh(in T value);
    //}

    //public interface IReadOnlyDouble<T> {

    //    void GetLow(out T result);

    //    void GetHigh(out T result);
    //}


    // void (?<v>[A-Za-z]+)\(in T first, in T second, out T result\);
    // T ${v}(T first, T second);
    // void (?<v>[A-Za-z]+)\(in T value, long count, out T result\);
    // T ${v}(T value, long count);

    // void (?<v>[A-Za-z]+)\(long value, out T result\);
    // T ${v}(long value);
    // void (?<v>[A-Za-z]+)\(in T value, out T result\);
    // T ${v}(T value);
    // long (?<v>[A-Za-z]+)\(in T value\);
    // long ${v}(T value);
    #region MyRegion
    //public interface IIntegerArithmetic<T> {

    //    int BitSize {

    //        get;
    //    }

    //    T Zero {

    //        get;
    //    }

    //    T One {

    //        get;
    //    }

    //    bool GetHighestBit(T value);

    //    T Copy(T value);

    //    T OnesComplement(T value);

    //    T BitwiseAnd(T first, T second);

    //    T BitwiseOr(T first, T second);

    //    T ExclusiveOr(T first, T second);

    //    T ShiftLeft(T value, long count);

    //    T ShiftRightSigned(T value, long count);

    //    T ShiftRightUnigned(T value, long count);

    //    void BigMulSigned(T first, T second, out T lowResult, out T highResult);

    //    void BigMulUnsigned(T first, T second, out T lowResult, out T highResult);

    //    T FromIntegerUnchecked(T value);

    //    T FromUIntegerUnchecked(T value);

    //    T ToIntegerUnchecked(T value);

    //    T ToUIntegerUnchecked(T value);

    //    T FromIntegerUnchecked(long value);

    //    T FromUIntegerUnchecked(long value);

    //    long ToIntegerUnchecked(T value);

    //    long ToUIntegerUnchecked(T value);

    //    T NegateUnchecked(T value);

    //    T AddUnchecked(T first, T second);

    //    T SubtractUnchecked(T first, T second);

    //    T MultiplyUnchecked(T first, T second);

    //    T DivideUnchecked(T first, T second);

    //    T RemainderUnchecked(T first, T second);

    //    T FromIntegerSigned(T value);

    //    T FromUIntegerSigned(T value);

    //    T ToIntegerSigned(T value);

    //    T ToUIntegerSigned(T value);

    //    T FromIntegerSigned(long value);

    //    T FromUIntegerSigned(long value);

    //    long ToIntegerSigned(T value);

    //    long ToUIntegerSigned(T value);

    //    T NegateSigned(T value);

    //    T AddSigned(T first, T second);

    //    T SubtractSigned(T first, T second);

    //    T MultiplySigned(T first, T second);

    //    T DivideSigned(T first, T second);

    //    T RemainderSigned(T first, T second);

    //    T FromIntegerUnsigned(T value);

    //    T FromUIntegerUnsigned(T value);

    //    T ToIntegerUnsigned(T value);

    //    T ToUIntegerUnsigned(T value);

    //    T FromIntegerUnsigned(long value);

    //    T FromUIntegerUnsigned(long value);

    //    long ToIntegerUnsigned(T value);

    //    long ToUIntegerUnsigned(T value);

    //    T NegateUnsigned(T value);

    //    T AddUnsigned(T first, T second);

    //    T SubtractUnsigned(T first, T second);

    //    T MultiplyUnsigned(T first, T second);

    //    T DivideUnsigned(T first, T second);

    //    T RemainderUnsigned(T first, T second);

    //    bool LessThanUnsigned(T value, T second);

    //    T IncreaseUnchecked(T value);

    //    T DecreaseUnchecked(T value);

    //    T IncreaseSigned(T value);

    //    T DecreaseSigned(T value);

    //    T IncreaseUnsigned(T value);

    //    T DecreaseUnsigned(T value);



    //    bool IsZero(T value);

    //    public TDouble IncreaseUnchecked<TDouble>(TDouble value)
    //        where TDouble : IUnsafeDouble<T> {
    //        var result_lo_ = value.Low;
    //        result_lo_ = IncreaseUnchecked(result_lo_);
    //        var result_hi_ = value.High;
    //        if (IsZero(result_hi_)) {
    //            result_hi_ = IncreaseUnchecked(result_hi_);
    //        }
    //        Unsafe.SkipInit(out result);
    //        result.High = result_hi_;
    //        result.Low = result_lo_;
    //    }

    //    //public void BigMulUnsigned<TDouble>(in TDouble first, in TDouble second, out TDouble result_lo, out TDouble result_hi)
    //    //    where TDouble : IUnsafeDouble<T> {
    //    //    unchecked {
    //    //        ref readonly var fl = ref first.Low;
    //    //        ref readonly var fh = ref first.High;
    //    //        ref readonly var sl = ref second.Low;
    //    //        ref readonly var sh = ref second.High;

    //    //        BigMulUnsigned(in fl, in sl, out var lll, out var llh);
    //    //        BigMulUnsigned(in fh, in sh, out var hhl, out var hhh);
    //    //        AddUnchecked(in fh, in fl, out var fm);
    //    //        AddUnchecked(in sh, in sl, out var sm);
    //    //        AddUnchecked(in hhl, in hhh, in lll, in llh, out var tl, out var th);
    //    //        BigMulUnsigned(fm, sm, out var mml, out var mmh);
    //    //        var dhi = 0;
    //    //        if (LessThanUnsigned(fm, fl)) {
    //    //            unchecked {
    //    //                ++dhi;
    //    //            }
    //    //            AddUnchecked(in mmh, in sm, out mmh);
    //    //        }
    //    //        if (LessThanUnsigned(sm, sl)) {
    //    //            unchecked {
    //    //                ++dhi;
    //    //            }
    //    //            AddUnchecked(in mmh, in sm, out mmh);
    //    //        }
    //    //        SubtractUnchecked(mml, mmh, tl, th, out mml, out mmh);
    //    //        AddUnchecked(in fm, in sm, out var dl);
    //    //        if (LessThanUnsigned(dl, fm)) {
    //    //            unchecked {
    //    //                ++dhi;
    //    //            }
    //    //        }
    //    //        AddUnchecked(in llh, in mml, out llh);
    //    //        if (LessThanUnsigned(llh, mml)) {
    //    //            IncreaseUnchecked(in hhl, in hhh, out hhl, out hhh);
    //    //        }
    //    //        Zero(out var zero);
    //    //        AddUnchecked(in hhl, in hhh, in mmh, in zero, out hhl, out hhh);
    //    //        FromIntegerUnsigned(dhi, out var dh);
    //    //        ShiftRightUnsigned(in dl, in dh, out dl, out dh);
    //    //        SubtractUnchecked(in dl, in dh, in mmh, in zero, out dl, out dh);
    //    //        AddUnchecked(in hhh, in dh, out hhh);
    //    //        Unsafe.SkipInit(out result_hi);
    //    //        result_hi.High = hhh;
    //    //        result_hi.Low = hhl;
    //    //        Unsafe.SkipInit(out result_lo);
    //    //        result_lo.High = llh;
    //    //        result_lo.Low = lll;
    //    //    }
    //    //}
    //}
    #endregion


    #region aaaa
    public interface IIntegerArithmetic<T> {

        int BitSize {

            get;
        }

        void Zero(out T value);

        void One(out T value);

        bool GetHighestBit(in T value);

        void Copy(in T value, out T result);

        void OnesComplement(in T value, out T result);

        void BitwiseAnd(in T first, in T second, out T result);

        void BitwiseOr(in T first, in T second, out T result);

        void ExclusiveOr(in T first, in T second, out T result);

        void ShiftLeft(in T value, long count, out T result);

        void ShiftRightSigned(in T value, long count, out T result);

        void ShiftRightUnigned(in T value, long count, out T result);

        void BigMulSigned(in T first, in T second, out T lowResult, out T highResult);

        void BigMulUnsigned(in T first, in T second, out T lowResult, out T highResult);

        void FromIntegerUnchecked(in T value, out T result);

        void FromUIntegerUnchecked(in T value, out T result);

        void ToIntegerUnchecked(in T value, out T result);

        void ToUIntegerUnchecked(in T value, out T result);

        void FromIntegerUnchecked(long value, out T result);

        void FromUIntegerUnchecked(long value, out T result);

        long ToIntegerUnchecked(in T value);

        long ToUIntegerUnchecked(in T value);

        void NegateUnchecked(in T value, out T result);

        void AddUnchecked(in T first, in T second, out T result);

        void SubtractUnchecked(in T first, in T second, out T result);

        void MultiplyUnchecked(in T first, in T second, out T result);

        void DivideUnchecked(in T first, in T second, out T result);

        void RemainderUnchecked(in T first, in T second, out T result);

        void FromIntegerSigned(in T value, out T result);

        void FromUIntegerSigned(in T value, out T result);

        void ToIntegerSigned(in T value, out T result);

        void ToUIntegerSigned(in T value, out T result);

        void FromIntegerSigned(long value, out T result);

        void FromUIntegerSigned(long value, out T result);

        long ToIntegerSigned(in T value);

        long ToUIntegerSigned(in T value);

        void NegateSigned(in T value, out T result);

        void AddSigned(in T first, in T second, out T result);

        void SubtractSigned(in T first, in T second, out T result);

        void MultiplySigned(in T first, in T second, out T result);

        void DivideSigned(in T first, in T second, out T result);

        void RemainderSigned(in T first, in T second, out T result);

        void FromIntegerUnsigned(in T value, out T result);

        void FromUIntegerUnsigned(in T value, out T result);

        void ToIntegerUnsigned(in T value, out T result);

        void ToUIntegerUnsigned(in T value, out T result);

        void FromIntegerUnsigned(long value, out T result);

        void FromUIntegerUnsigned(long value, out T result);

        long ToIntegerUnsigned(in T value);

        long ToUIntegerUnsigned(in T value);

        void NegateUnsigned(in T value, out T result);

        void AddUnsigned(in T first, in T second, out T result);

        void SubtractUnsigned(in T first, in T second, out T result);

        void MultiplyUnsigned(in T first, in T second, out T result);

        void DivideUnsigned(in T first, in T second, out T result);

        void RemainderUnsigned(in T first, in T second, out T result);

        bool LessThanUnsigned(in T first, in T second);

        void IncreaseUnchecked(in T value, out T result);

        void DecreaseUnchecked(in T value, out T result);

        void IncreaseSigned(in T value, out T result);

        void DecreaseSigned(in T value, out T result);

        void IncreaseUnsigned(in T value, out T result);

        void DecreaseUnsigned(in T value, out T result);



        bool IsZero(in T value);

        public void IncreaseUnchecked<TDouble>(in TDouble value, out TDouble result)
            where TDouble : IUnsafeDouble<T> {
            var result_lo_ = value.Low;
            IncreaseUnchecked(in result_lo_, out result_lo_);
            var result_hi_ = value.High;
            if (IsZero(in result_hi_)) {
                IncreaseUnchecked(in result_hi_, out result_hi_);
            }
            Unsafe.SkipInit(out result);
            result.High = result_hi_;
            result.Low = result_lo_;
        }

        //public void BigMulUnsigned<TDouble>(in TDouble first, in TDouble second, out TDouble result_lo, out TDouble result_hi)
        //    where TDouble : IUnsafeDouble<T> {
        //    unchecked {
        //        TDouble f;
        //        Unsafe.SkipInit(out f);
        //        TDouble s;
        //        Unsafe.SkipInit(out s);
        //        TDouble hh;
        //        Unsafe.SkipInit(out hh);
        //        TDouble ll;
        //        Unsafe.SkipInit(out ll);
        //        ref readonly var fl = ref first.Low;
        //        ref readonly var fh = ref first.High;
        //        ref readonly var sl = ref second.Low;
        //        ref readonly var sh = ref second.High;

        //        BigMulUnsigned(in fl, in sl, out var lll, out var llh);
        //        BigMulUnsigned(in fh, in sh, out var hhl, out var hhh);
        //        AddUnchecked(in fh, in fl, out var fm);
        //        AddUnchecked(in sh, in sl, out var sm);
        //        AddUnchecked(in hhl, in hhh, in lll, in llh, out var tl, out var th);
        //        BigMulUnsigned(fm, sm, out var mml, out var mmh);
        //        var dhi = 0;
        //        if (LessThanUnsigned(fm, fl)) {
        //            unchecked {
        //                ++dhi;
        //            }
        //            AddUnchecked(in mmh, in sm, out mmh);
        //        }
        //        if (LessThanUnsigned(sm, sl)) {
        //            unchecked {
        //                ++dhi;
        //            }
        //            AddUnchecked(in mmh, in sm, out mmh);
        //        }
        //        SubtractUnchecked(mml, mmh, tl, th, out mml, out mmh);
        //        AddUnchecked(in fm, in sm, out var dl);
        //        if (LessThanUnsigned(dl, fm)) {
        //            unchecked {
        //                ++dhi;
        //            }
        //        }
        //        AddUnchecked(in llh, in mml, out llh);
        //        if (LessThanUnsigned(llh, mml)) {
        //            IncreaseUnchecked(in hhl, in hhh, out hhl, out hhh);
        //        }
        //        Zero(out var zero);
        //        AddUnchecked(in hhl, in hhh, in mmh, in zero, out hhl, out hhh);
        //        FromIntegerUnsigned(dhi, out var dh);
        //        ShiftRightUnsigned(in dl, in dh, out dl, out dh);
        //        SubtractUnchecked(in dl, in dh, in mmh, in zero, out dl, out dh);
        //        AddUnchecked(in hhh, in dh, out hhh);
        //        Unsafe.SkipInit(out result_hi);
        //        result_hi.High = hhh;
        //        result_hi.Low = hhl;
        //        Unsafe.SkipInit(out result_lo);
        //        result_lo.High = llh;
        //        result_lo.Low = lll;
        //    }
        //}
    }
#endregion
}
