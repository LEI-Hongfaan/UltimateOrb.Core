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

    //public interface IDouble<TBase>
    //    : IReadOnlyDouble<TBase>
    //    , IWriteOnlyDouble<TBase> {
    //}

    //public interface IWriteOnlyDouble<TBase> {

    //    void SetLow(in TBase value);

    //    void SetHigh(in TBase value);
    //}

    //public interface IReadOnlyDouble<TBase> {

    //    void GetLow(out TBase result);

    //    void GetHigh(out TBase result);
    //}


    // void (?<v>[A-Za-z]+)\(in TBase first, in TBase second, out TBase result\);
    // TBase ${v}(TBase first, TBase second);
    // void (?<v>[A-Za-z]+)\(in TBase value, long count, out TBase result\);
    // TBase ${v}(TBase value, long count);

    // void (?<v>[A-Za-z]+)\(long value, out TBase result\);
    // TBase ${v}(long value);
    // void (?<v>[A-Za-z]+)\(in TBase value, out TBase result\);
    // TBase ${v}(TBase value);
    // long (?<v>[A-Za-z]+)\(in TBase value\);
    // long ${v}(TBase value);
    #region MyRegion
    //public interface IIntegerArithmetic<TBase> {

    //    int BitSize {

    //        get;
    //    }

    //    TBase Zero {

    //        get;
    //    }

    //    TBase One {

    //        get;
    //    }

    //    bool GetHighestBit(TBase value);

    //    TBase Copy(TBase value);

    //    TBase OnesComplement(TBase value);

    //    TBase BitwiseAnd(TBase first, TBase second);

    //    TBase BitwiseOr(TBase first, TBase second);

    //    TBase ExclusiveOr(TBase first, TBase second);

    //    TBase ShiftLeft(TBase value, long count);

    //    TBase ShiftRightSigned(TBase value, long count);

    //    TBase ShiftRightUnigned(TBase value, long count);

    //    void BigMulSigned(TBase first, TBase second, out TBase lowResult, out TBase highResult);

    //    void BigMulUnsigned(TBase first, TBase second, out TBase lowResult, out TBase highResult);

    //    TBase FromIntegerUnchecked(TBase value);

    //    TBase FromUIntegerUnchecked(TBase value);

    //    TBase ToIntegerUnchecked(TBase value);

    //    TBase ToUIntegerUnchecked(TBase value);

    //    TBase FromIntegerUnchecked(long value);

    //    TBase FromUIntegerUnchecked(long value);

    //    long ToIntegerUnchecked(TBase value);

    //    long ToUIntegerUnchecked(TBase value);

    //    TBase NegateUnchecked(TBase value);

    //    TBase AddUnchecked(TBase first, TBase second);

    //    TBase SubtractUnchecked(TBase first, TBase second);

    //    TBase MultiplyUnchecked(TBase first, TBase second);

    //    TBase DivideUnchecked(TBase first, TBase second);

    //    TBase RemainderUnchecked(TBase first, TBase second);

    //    TBase FromIntegerSigned(TBase value);

    //    TBase FromUIntegerSigned(TBase value);

    //    TBase ToIntegerSigned(TBase value);

    //    TBase ToUIntegerSigned(TBase value);

    //    TBase FromIntegerSigned(long value);

    //    TBase FromUIntegerSigned(long value);

    //    long ToIntegerSigned(TBase value);

    //    long ToUIntegerSigned(TBase value);

    //    TBase NegateSigned(TBase value);

    //    TBase AddSigned(TBase first, TBase second);

    //    TBase SubtractSigned(TBase first, TBase second);

    //    TBase MultiplySigned(TBase first, TBase second);

    //    TBase DivideSigned(TBase first, TBase second);

    //    TBase RemainderSigned(TBase first, TBase second);

    //    TBase FromIntegerUnsigned(TBase value);

    //    TBase FromUIntegerUnsigned(TBase value);

    //    TBase ToIntegerUnsigned(TBase value);

    //    TBase ToUIntegerUnsigned(TBase value);

    //    TBase FromIntegerUnsigned(long value);

    //    TBase FromUIntegerUnsigned(long value);

    //    long ToIntegerUnsigned(TBase value);

    //    long ToUIntegerUnsigned(TBase value);

    //    TBase NegateUnsigned(TBase value);

    //    TBase AddUnsignedWithOverflow(TBase first, TBase second);

    //    TBase SubtractUnsigned(TBase first, TBase second);

    //    TBase MultiplyUnsigned(TBase first, TBase second);

    //    TBase DivideUnsigned(TBase first, TBase second);

    //    TBase RemainderUnsigned(TBase first, TBase second);

    //    bool LessThanUnsigned(TBase value, TBase second);

    //    TBase IncreaseUnchecked(TBase value);

    //    TBase DecreaseUnchecked(TBase value);

    //    TBase IncreaseSigned(TBase value);

    //    TBase DecreaseSigned(TBase value);

    //    TBase IncreaseUnsigned(TBase value);

    //    TBase DecreaseUnsigned(TBase value);



    //    bool IsZero(TBase value);

    //    public TDouble IncreaseUnchecked<TDouble>(TDouble value)
    //        where TDouble : IUnsafeDouble<TBase> {
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
    //    //    where TDouble : IUnsafeDouble<TBase> {
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
        //    where TDouble : IUnsafeDouble<TBase> {
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
