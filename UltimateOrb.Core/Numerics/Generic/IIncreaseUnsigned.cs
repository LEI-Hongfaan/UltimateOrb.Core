using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

using IL = InlineIL.IL;
using static InlineIL.IL.Emit;
using static UltimateOrb.Utilities.Extensions.CanonicalIntegerBooleanExtensions;
using System.Diagnostics;
using UltimateOrb.Functional.DataTypes;

namespace UltimateOrb.Numerics.Generic {

    public struct TIntegerArithmaticPrimtive : IIntegerArithmatic<Void, ulong> {


        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in ulong first, in ulong second, out ulong result) {
            return MathWithCarrying.AddUnsigned(carry, first, second, out result);
        }

        public void BigMul(in ulong first, in ulong second, out ulong result_lo, out ulong result_hi) {
            result_lo = DoubleArithmetic.BigMul(first, second, out result_hi);
        }

        public void ExtendedShiftRightUnsigned(in ulong value, uint value_ex, out ulong result, out uint result_ex) {
            result = DoubleArithmetic.ShiftRightUnsigned(value, value_ex, out var result_ex_);
            result_ex = unchecked((uint)result_ex_);
        }

        public void SubtractUnchecked(in ulong first, in ulong second, out ulong result) {
            result = unchecked(first - second);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean carry, in ulong first, in ulong second, out ulong result) {
            return MathWithCarrying.SubtractUnsigned(carry, first, second, out result);
        }

        public void ToIntegerUnchecked(uint value, out ulong result) {
            result = value;
        }
    }

    public interface IIntegerArithmatic<TT, T> {

        public long BitSize { get => SizeOfModule.BitSizeOf<T>(); }

        public void BigMul(in T first, in T second, out T result_lo, out T result_hi);
        public void SubtractUnchecked(in T first, in T second, out T result);

        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(in T value, out T result) {
            ToIntegerUnchecked(1, out var one);
            return AddUnsignedNoThrow(in value, in one, out result);
        }

        public CanonicalIntegerBoolean DecreaseUnsignedNoThrow(in T value, out T result) {
            ToIntegerUnchecked(1, out var one);
            return SubtractUnsignedNoThrow(in value, in one, out result);
        }

        public CanonicalIntegerBoolean ConditionalIncreaseUnsignedNoThrow(CanonicalIntegerBoolean carry, in T value, out T result) {
            if (carry) {
                return IncreaseUnsignedNoThrow(in value, out result);
            } else {
                Copy(in value, out result);
                return false;
            }
        }

        public CanonicalIntegerBoolean ConditionalDecreaseUnsignedNoThrow(CanonicalIntegerBoolean carry, in T value, out T result) {
            if (carry) {
                return DecreaseUnsignedNoThrow(in value, out result);
            } else {
                Copy(in value, out result);
                return false;
            }
        }

        public void ConditionalIncreaseUnchecked(CanonicalIntegerBoolean carry, in T value, out T result) {
            if (carry) {
                IncreaseUnchecked(in value, out result);
            } else {
                Copy(in value, out result);
            }
        }

        public void ConditionalDecreaseUnchecked(CanonicalIntegerBoolean carry, in T value, out T result) {
            if (carry) {
                DecreaseUnchecked(in value, out result);
            } else {
                Copy(in value, out result);
            }
        }

        public void ExtendedShiftRightUnsigned(in T value, uint value_ex, out T result, out uint result_ex);

        public void ExtendedShiftRightUnsigned(ref T value_result, ref uint value_result_ex) {
            ExtendedShiftRightUnsigned(in value_result, value_result_ex, out value_result, out value_result_ex);
        }

        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in T first, in T second, out T result);

        public CanonicalIntegerBoolean AddUnsignedNoThrow(in T first, in T second, out T result) {
            return AddUnsignedNoThrow(false, in first, in second, out result);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean carry, in T first, in T second, out T result);

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(in T first, in T second, out T result) {
            return SubtractUnsignedNoThrow(false, in first, in second, out result);
        }

        // public CanonicalIntegerBoolean ToIntegerUnsignedNoThrow(uint value, out T result);
        public void ToIntegerUnchecked(uint value, out T result);



        public void IncreaseUnchecked(in T value, out T result) {
            IncreaseUnsignedNoThrow(in value, out result);
        }

        public void DecreaseUnchecked(in T value, out T result) {
            DecreaseUnsignedNoThrow(in value, out result);
        }

        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(ref T value_result) {
            return IncreaseUnsignedNoThrow(in value_result, out value_result);
        }

        public CanonicalIntegerBoolean DecreaseUnsignedNoThrow(ref T value_result) {
            return DecreaseUnsignedNoThrow(in value_result, out value_result);
        }

        public CanonicalIntegerBoolean ConditionalIncreaseUnsignedNoThrow(CanonicalIntegerBoolean carry, ref T value_result) {
            return ConditionalIncreaseUnsignedNoThrow(carry, in value_result, out value_result);
        }

        public CanonicalIntegerBoolean ConditionalDecreaseUnsignedNoThrow(CanonicalIntegerBoolean carry, ref T value_result) {
            return ConditionalDecreaseUnsignedNoThrow(carry, in value_result, out value_result);
        }

        public void ConditionalIncreaseUnchecked(CanonicalIntegerBoolean carry, ref T value_result) {
            ConditionalIncreaseUnsignedNoThrow(carry, in value_result, out value_result);
        }

        public void ConditionalDecreaseUnchecked(CanonicalIntegerBoolean carry, ref T value_result) {
            ConditionalDecreaseUnsignedNoThrow(carry, in value_result, out value_result);
        }

        public void Copy(in T value, out T result) {
            result = value;
        }

        public void AddUnchecked(ref T first_result, uint second) {
            ToIntegerUnchecked(second, out var second_);
            AddUnchecked(ref first_result, in second_);
        }

        public CanonicalIntegerBoolean AddUnsignedNoThrow(ref T first_result, in T second) {
            Copy(first_result, out var first);
            return AddUnsignedNoThrow(in first, in second, out first_result);
        }

        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, ref T first_result, in T second) {
            Copy(first_result, out var first);
            return AddUnsignedNoThrow(carry, in first, in second, out first_result);
        }

        public void AddUnchecked(ref T first_result, in T second) {
            Copy(first_result, out var first);
            AddUnchecked(in first, in second, out first_result);
        }

        public void AddUnchecked(in T first, in T second, out T result) {
            AddUnsignedNoThrow(in first, in second, out result);
        }

        public void SubtractUnchecked(ref T first_result, uint second) {
            ToIntegerUnchecked(second, out var second_);
            SubtractUnchecked(ref first_result, in second_);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(ref T first_result, in T second) {
            Copy(first_result, out var first);
            return SubtractUnsignedNoThrow(in first, in second, out first_result);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean carry, ref T first_result, in T second) {
            Copy(first_result, out var first);
            return SubtractUnsignedNoThrow(carry, in first, in second, out first_result);
        }

        public void SubtractUnchecked(ref T first_result, in T second) {
            Copy(first_result, out var first);
            SubtractUnchecked(in first, in second, out first_result);
        }

        public void ExtendedSubtractUnchecked(in T first, uint first_ex, in T second, ZeroT second_ex, out T result, out uint result_ex) {
            var carry = SubtractUnsignedNoThrow(in first, in second, out result);
            result_ex = first_ex - unchecked((uint)(int)carry);
        }

        public void ExtendedSubtractUnchecked(ref T first_result, ref uint first_result_ex, in T second, ZeroT second_ex) {
            ExtendedSubtractUnchecked(in first_result, first_result_ex, in second, default(ZeroT), out first_result, out first_result_ex);
        }

        public bool IsOverlappedWith(in T first, in T second) {
            return Unsafe.AreSame(ref Unsafe.AsRef(in first), ref Unsafe.AsRef(in second));
        }

        public bool IsSameAs(in T first, in T second) {
            return !Unsafe.AreSame(ref Unsafe.AsRef(in first), ref Unsafe.AsRef(in second)); ;
        }

        public bool IsSameAsOrNotOverlappedWith(in T first, in T second) {
            return true;
        }
    }

    public readonly struct ZeroT {
    }

    public struct DoubleArithmaticChangeTag<TFrom, TTo, T, TStructure> : IDoubleArithmatic<TTo, T>
        where TStructure : struct, IDoubleArithmatic<TFrom, T> {

        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in T first, in T second, out T result) {
            return default(TStructure).AddUnsignedNoThrow(carry, in first, in second, out result);
        }

        public void BigMul(in T first, in T second, out T result_lo, out T result_hi) {
            default(TStructure).BigMul(first, in second, out result_lo, out result_hi);
        }

        public void ExtendedShiftRightUnsigned(in T value, uint value_ex, out T result, out uint result_ex) {
            default(TStructure).ExtendedShiftRightUnsigned(in value, value_ex, out result, out result_ex);
        }

        public void SubtractUnchecked(in T first, in T second, out T result) {
            default(TStructure).SubtractUnchecked(in first, in second, out result);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean carry, in T first, in T second, out T result) {
            return default(TStructure).SubtractUnsignedNoThrow(in first, in second, out result);
        }

        public void ToIntegerUnchecked(uint value, out T result) {
            default(TStructure).ToIntegerUnchecked(value, out result);
        }
    }

    public readonly struct TDoubleArithmatic<T, TDouble, TIntegerArithmaticOfT>
        : IDoubleArithmatic<Void, T>
        , IDoubleArithmatic<Void<Void>, TDouble>
        where TDouble : IDouble<T>
        where TIntegerArithmaticOfT : struct, IIntegerArithmatic<Void, T> {


        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in T first, in T second, out T result) {
            return default(TIntegerArithmaticOfT).AddUnsignedNoThrow(carry, in first, in second, out result);
        }

        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in TDouble first, in TDouble second, out TDouble result) {
            // ((IDoubleArithmatic<Void, T>)this).AddUnsignedNoThrow(carry, in first.GetLoRef(), in first.GetHiRef(), in second.GetLoRef(), in second.GetHiRef(), out result);
            throw new NotImplementedException();
        }

        public void BigMul(in T first, in T second, out T result_lo, out T result_hi) {
            default(TIntegerArithmaticOfT).BigMul(in first, in second, out result_lo, out result_hi);
        }

        public void BigMul(in TDouble first, in TDouble second, out TDouble result_lo, out TDouble result_hi) {
            Unsafe.SkipInit(out result_lo);
            Unsafe.SkipInit(out result_hi);
            ((IDoubleArithmatic<Void, T>)this).BigMul(in first.GetLoRef(), in first.GetHiRef(), in second.GetLoRef(), in second.GetHiRef(), out result_lo.GetLoRef(), out result_lo.GetHiRef(), out result_hi.GetLoRef(), out result_hi.GetHiRef());
        }

        public void ExtendedShiftRightUnsigned(in T value, uint value_ex, out T result, out uint result_ex) {
            default(TIntegerArithmaticOfT).ExtendedShiftRightUnsigned(in value, value_ex, out result, out result_ex);
        }

        public void ExtendedShiftRightUnsigned(in TDouble value, uint value_ex, out TDouble result, out uint result_ex) {
            Unsafe.SkipInit(out result);
            throw new NotImplementedException();

            // ((IDoubleArithmatic<Void, T>)this).ExtendedShiftRightUnsigned(in value.GetLoRef(), in value.GetHiRef(), in second.GetLoRef(), in second.GetHiRef(), out result.GetLoRef(), out result.GetHiRef());
        }

        public void SubtractUnchecked(in T first, in T second, out T result) {
            default(TIntegerArithmaticOfT).SubtractUnchecked(in first, in second, out result);
        }

        public void SubtractUnchecked(in TDouble first, in TDouble second, out TDouble result) {
            Unsafe.SkipInit(out result);
            ((IDoubleArithmatic<Void, T>)this).SubtractUnchecked(in first.GetLoRef(), in first.GetHiRef(), in second.GetLoRef(), in second.GetHiRef(), out result.GetLoRef(), out result.GetHiRef());
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean borrow, in T first, in T second, out T result) {
            return default(TIntegerArithmaticOfT).SubtractUnsignedNoThrow(in first, in second, out result);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean borrow, in TDouble first, in TDouble second, out TDouble result) {
            Unsafe.SkipInit(out result);
            CanonicalIntegerBoolean b;
            TDouble result_;
            throw new NotImplementedException();
            if (borrow) {
                // b =((IDoubleArithmatic<Void, T>)this).DecreaseUnsignedNoThrow(first.GetLoRef(), in first.GetHiRef(), result.
            }


            // return ((IDoubleArithmatic<Void, T>)this).SubtractUnsignedNoThrow(in first.GetLoRef(), in first.GetHiRef(), in second.GetLoRef(), in second.GetHiRef(), out result.GetLoRef(), out result.GetHiRef());
        }

        public void ToIntegerUnchecked(uint value, out T result) {
            default(TIntegerArithmaticOfT).ToIntegerUnchecked(value, out result);
        }

        public void ToIntegerUnchecked(uint value, out TDouble result) {
            Unsafe.SkipInit(out result);
            ToIntegerUnchecked(value, out result.GetLoRef());
            ToIntegerUnchecked(0, out result.GetHiRef());
        }
    }




    public interface IIntegerNSignUnspecifiedOperations<Tag, T> {

        static abstract T Add(T first, T second);

        static abstract T Subtract(T first, T second);

        static abstract T Multiply(T first, T second);

        static abstract T Increase(T first);

        static abstract T Decrease(T first);
    }

    public interface IIntegerN2<Tag, T> {

        static abstract T Divide(T first, T second);

    }

    public interface IIntegerN3<Tag, T> {

        static abstract T Remainder(T first, T second);
    }


    public readonly struct UncheckedTag {
    }

    public readonly struct CheckedTag {
    }

    public interface IIntegerNSignSpecifiedOperations<Tag, T>
        : IIntegerN2<Void<Tag, UncheckedTag>, T>
        , IIntegerN2<Void<Tag, CheckedTag>, T>
        , IIntegerN3<Void<Tag, UncheckedTag>, T>
        , IIntegerN3<Void<Tag, CheckedTag>, T> {

        static abstract T BigMul(T first, T second, out T result_hi);
        static abstract T ShiftRight(T value, int count);



    }
    public interface IIntegerNUncheckedOperations<in TIntegerNUncheckedOperations, Tag, T>
        where TIntegerNUncheckedOperations : IIntegerNUncheckedOperations<TIntegerNUncheckedOperations, Tag, T> {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static abstract int BitSize { get; }

        static abstract T One { get; }

        static abstract T Zero { get; }

        static abstract T ShiftLeftCore(T value, int count);

        static T ShiftLeft(T value) {
            return TIntegerNUncheckedOperations.ShiftLeftCore(value, 1);
        }
    }

    public interface ISIntegerNBase<in TSIntegerNBase, Tag, T>
        : IIntegerNSignUnspecifiedOperations<Void<Tag, UncheckedTag>, T>
        , IIntegerNUncheckedOperations<TSIntegerNBase, Tag, T>
        where TSIntegerNBase : ISIntegerNBase<TSIntegerNBase, Tag, T> {




    }


    public interface IXIntegerN<in TXIntegerN, Tag, Signed, T>
        : ISIntegerNBase<TXIntegerN, Tag, T>
        , IIntegerNSignSpecifiedOperations<Void<Tag, Signed>, T>
        where TXIntegerN : IXIntegerN<TXIntegerN, Tag, Signed, T> {




    }

    public interface IIntegerN<in TIntegerN, Tag, T>
        : IXIntegerN<TIntegerN, Tag, TrueT, T>
        where TIntegerN : IIntegerN<TIntegerN, Tag, T> {

        static abstract T MinusOne { get; }

    }

    public interface IUIntegerN<in TUIntegerN, Tag, T>
        : IXIntegerN<TUIntegerN, Tag, FalseT, T>
        where TUIntegerN : IUIntegerN<TUIntegerN, Tag, T> {

        static abstract T Two { get; }
    }

    public interface IZIntegerN<in TZIntegerN, Tag, T>
        : IXIntegerN<TZIntegerN, Tag, TrueT, T>
        , IXIntegerN<TZIntegerN, Tag, FalseT, T>
        where TZIntegerN : IZIntegerN<TZIntegerN, Tag, T> {


    }
    public interface IDouble<T> {
        void SetLo(in T value);
        void GetLo(out T result);
        void SetHi(in T value);
        void GetHi(out T result);
        ref T GetLoRef();
        ref T GetHiRef();
    }

    public interface IDoubleArithmatic<TT, T> : IIntegerArithmatic<TT, T> {

        public bool IsOverlapped(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in second_lo, in second_hi));
            return
                IsOverlappedWith(in first_lo, in second_lo) ||
                IsOverlappedWith(in first_lo, in second_hi) ||
                IsOverlappedWith(in first_hi, in second_lo) ||
                IsOverlappedWith(in first_hi, in second_hi);
        }

        public bool IsSameAs(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in second_lo, in second_hi));
            return
                IsSameAs(in first_lo, in second_lo) &&
                IsSameAs(in first_hi, in second_hi);
        }

        public bool IsSameAsOrNotOverlapped(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in second_lo, in second_hi));
            return
                IsSameAs(in first_lo, in first_hi, in second_lo, in second_hi) ||
                !IsOverlapped(in first_lo, in first_hi, in second_lo, in second_hi);
        }

        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(in T value_lo, in T value_hi, out T result_lo, out T result_hi) {
            return ConditionalIncreaseUnsignedNoThrow(IncreaseUnsignedNoThrow(in value_lo, out result_lo), in value_hi, out result_hi);
        }



        public CanonicalIntegerBoolean IncreaseUnchecked(ref T value_result_lo, ref T value_result_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlappedWith(in value_result_lo, in value_result_hi));
            return ConditionalIncreaseUnsignedNoThrow(IncreaseUnsignedNoThrow(ref value_result_lo), ref value_result_hi);
        }

        public void AddUnchecked(in T first_lo, in T first_hi, in T second_lo, in T second_hi, out T result_lo, out T result_hi) {
            var carry = AddUnsignedNoThrow(in first_lo, in second_lo, out result_lo);
            AddUnsignedNoThrow(carry, in first_hi, in second_hi, out result_hi);
        }

        public void SubtractUnchecked(in T first_lo, in T first_hi, in T second_lo, in T second_hi, out T result_lo, out T result_hi) {
            var carry = SubtractUnsignedNoThrow(in first_lo, in second_lo, out result_lo);
            SubtractUnsignedNoThrow(carry, in first_hi, in second_hi, out result_hi);
        }

        public void AddUnchecked(in T first_lo, in T first_hi, in T second_lo, ZeroT second_hi, out T result_lo, out T result_hi) {
            var carry = AddUnsignedNoThrow(in first_lo, in second_lo, out result_lo);
            ConditionalIncreaseUnchecked(carry, in first_hi, out result_hi);
        }

        public void AddUnchecked(ref T first_result_lo, ref T first_result_hi, in T second_lo, ZeroT second_hi) {
            AddUnchecked(in first_result_lo, in first_result_hi, in second_lo, default(ZeroT), out first_result_lo, out first_result_hi);
        }

        public void SubtractUnchecked(in T first_lo, in T first_hi, in T second_lo, ZeroT second_hi, out T result_lo, out T result_hi) {
            var borrow = SubtractUnsignedNoThrow(in first_lo, in second_lo, out result_lo);
            ConditionalDecreaseUnchecked(borrow, in first_hi, out result_hi);
        }

        public void SubtractUnchecked(ref T first_result_lo, ref T first_result_hi, in T second_lo, in T second_hi) {
            SubtractUnchecked(in first_result_lo, in first_result_hi, in second_lo, in second_hi, out first_result_lo, out first_result_hi);
        }



        public void BigMul(in T first_lo, in T first_hi, in T second_lo, in T second_hi, out T result_lo_lo, out T result_lo_hi, out T result_hi_lo, out T result_hi_hi) {
            var fl = first_lo;
            var fh = first_hi;
            var sl = second_lo;
            var sh = second_hi;
            BigMul(in fl, in sl, out var lll, out var llh);
            BigMul(in fh, in sh, out var hhl, out var hhh);
            var fc = AddUnsignedNoThrow(in fh, in fl, out var fm);
            var sc = AddUnsignedNoThrow(in sh, in sl, out var sm);
            AddUnchecked(in hhl, in hhh, in lll, in llh, out var tl, out var th);
            BigMul(in fm, in sm, out var mml, out var mmh);
            var dh = 0u;
            if (fc) {
                unchecked {
                    ++dh;
                    AddUnchecked(ref mmh, in sm);
                }
            }
            if (sc) {
                unchecked {
                    ++dh;
                    AddUnchecked(ref mmh, in fm);
                }
            }
            SubtractUnchecked(ref mml, ref mmh, in tl, in th);

            if (AddUnsignedNoThrow(in fm, in sm, out var dl)) {
                unchecked {
                    ++dh;
                }
            }
            if (AddUnsignedNoThrow(ref llh, in mml)) {
                IncreaseUnchecked(ref hhl, ref hhh);
            }
            AddUnchecked(ref hhl, ref hhh, in mmh, default(ZeroT));
            ExtendedShiftRightUnsigned(ref dl, ref dh);
            ExtendedSubtractUnchecked(ref dl, ref dh, in mmh, default(ZeroT));
            AddUnchecked(ref hhh, dh);
            result_hi_hi = hhh;
            result_hi_lo = hhl;
            result_lo_hi = llh;
            result_lo_lo = lll;
        }
    }

    public readonly struct Long<TInt, TUInt> {

        public readonly TInt D0;

        public readonly TInt D1;

        ref readonly TInt D0Ref {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Ldarg_0();
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D0)));
                Ret();
                throw IL.Unreachable();
            }
        }

        ref readonly TInt D1Ref {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                Ldarg_0();
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D1)));
                Ret();
                throw IL.Unreachable();
            }
        }

        public ref readonly TUInt Lo {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref Unsafe.As<TInt, TUInt>(ref Unsafe.AsRef(in BitConverter.IsLittleEndian ? ref D1Ref : ref D0Ref));
        }

        public ref readonly TInt Hi {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref BitConverter.IsLittleEndian ? ref D1Ref : ref D0Ref;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Long(TUInt lo, TInt hi, int ignored = default) {
            if (BitConverter.IsLittleEndian) {
                D0 = Unsafe.As<TUInt, TInt>(ref lo);
                D1 = hi;
            } else {
                D1 = Unsafe.As<TUInt, TInt>(ref lo);
                D0 = hi;
            }
        }
    }

    public readonly struct ULong<TInt, TUInt> {

        public readonly TInt D0;

        public readonly TInt D1;

        public ref readonly TUInt Lo {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                Ldarg_0();
                Ldsfld(InlineIL.FieldRef.Field(typeof(BitConverter), nameof(BitConverter.IsLittleEndian)));
                Brfalse_S("L");
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D0)));
                Ret();
                IL.MarkLabel("L");
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D1)));
                Ret();
                throw IL.Unreachable();
            }
        }

        public ref readonly TUInt Hi {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                Ldarg_0();
                Ldsfld(InlineIL.FieldRef.Field(typeof(BitConverter), nameof(BitConverter.IsLittleEndian)));
                Brfalse_S("L");
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D1)));
                Ret();
                IL.MarkLabel("L");
                Ldflda(InlineIL.FieldRef.Field(typeof(Long<TInt, TUInt>), nameof(D0)));
                Ret();
                throw IL.Unreachable();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ULong(TUInt lo, TUInt hi, int ignored = default) {
            if (BitConverter.IsLittleEndian) {
                D0 = Unsafe.As<TUInt, TInt>(ref lo);
                D1 = Unsafe.As<TUInt, TInt>(ref hi);
            } else {
                D1 = Unsafe.As<TUInt, TInt>(ref lo);
                D0 = Unsafe.As<TUInt, TInt>(ref hi);
            }
        }
    }

    public readonly struct Double<T> {
        public readonly T Lo;
        public readonly T Hi;
    }

    public struct MutableDouble<T> {
        public T Lo;
        public T Hi;
    }

    interface IIsZero<T> {

        public CanonicalIntegerBoolean IsZero(in T value);
    }

    interface IDoubleIsZero<T> : IIsZero<T> {

        public CanonicalIntegerBoolean IsZero(in Double<T> value) {
            return IsZero(in value.Lo) && IsZero(in value.Hi);
        }
    }

    interface IIsComplementZero<T> {

        public CanonicalIntegerBoolean IsComplementZero(in T value);
    }

    interface IDoubleIsComplementZero<T> : IIsComplementZero<T> {

        public CanonicalIntegerBoolean IsComplementZero(in Double<T> value) {
            return IsComplementZero(in value.Lo) && IsComplementZero(in value.Hi);
        }
    }

    public interface IIncreaseUnsignedNoThrow<T> {
        /*
		public T IncreaseUnsigned(T value) {
			IncreaseUnsignedInPlace(ref value);
			return value;
		}

		public T IncreaseUnsigned(in T value) {
			IncreaseUnsigned(in value, out var result);
			return result;
		}

		public void IncreaseUnsignedInPlace(ref T value);

		public void IncreaseUnsigned(in T value, out T result) {
			_ = checked(0 - unchecked((uint)(int)IncreaseUnsignedNoThrow(in value, out result)));
		}
		*/

        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(in T value, out T result);
    }

    interface IDoubleIncreaseUnsigned<T> : IIncreaseUnsignedNoThrow<T> {

        [MethodImpl(MethodImplOptions.AggressiveOptimization | MethodImplOptions.AggressiveInlining)]
        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(in Double<T> value, out MutableDouble<T> result) {
            Unsafe.SkipInit(out result);
            return IncreaseUnsignedNoThrow(in value.Lo, out result.Lo) ? IncreaseUnsignedNoThrow(in value.Hi, out result.Hi) : (result.Hi = value.Hi).Comma(false);
        }
    }
}
