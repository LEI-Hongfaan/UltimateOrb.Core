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

namespace UltimateOrb.Numerics.Generic {

    public struct TIntegerArithmaticPrimtive : IIntegerArithmatic<ulong> {

        public CanonicalIntegerBoolean AddUnsignedNoThrow(CanonicalIntegerBoolean carry, in ulong first, in ulong second, out ulong result) {
            var result_ = unchecked(first + second);
            var c= CanonicalIntegerBooleanModule.LessThan(result_, first);
            if (carry) {
                c |= IncreaseUnsignedNoThrow(in result_, out result_);
            }

            result = result_;
            return c;
        }

        public void BigMul(in ulong first, in ulong second, out ulong result_lo, out ulong result_hi) {
            result_lo = DoubleArithmetic.BigMul(first, second, out result_hi);
        }

        public void ExtendedShiftRightUnsigned(in ulong value, uint value_ex, out ulong result, out uint result_ex) {
            throw new NotImplementedException();
        }

        public void SubtractUnchecked(in ulong first, in ulong second, out ulong result) {
            result = unchecked(first - second);
        }

        public CanonicalIntegerBoolean SubtractUnsignedNoThrow(CanonicalIntegerBoolean carry, in ulong first, in ulong second, out ulong result) {
            return NewMethod(carry, first, second, out result);
        }

        

        public void ToIntegerUnchecked(uint value, out ulong result) {
            result = value;
        }


    }
    public interface IIntegerArithmatic<T> {


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

        public bool IsOverlapped(in T first, in T second) {
            return Unsafe.AreSame(ref Unsafe.AsRef(in first), ref Unsafe.AsRef(in second));
        }

        public bool IsSameAs(in T first, in T second) {
            return !Unsafe.AreSame(ref Unsafe.AsRef(in first), ref Unsafe.AsRef(in second)); ;
        }

        public bool IsSameAsOrNotOverlapped(in T first, in T second) {
            return true;
        }
    }

    public readonly struct ZeroT {
    }

    public interface IDoubleArithmatic<T> : IIntegerArithmatic<T> {

        public bool IsOverlapped(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlapped(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlapped(in second_lo, in second_hi));
            return
                IsOverlapped(in first_lo, in second_lo) ||
                IsOverlapped(in first_lo, in second_hi) ||
                IsOverlapped(in first_hi, in second_lo) ||
                IsOverlapped(in first_hi, in second_hi);
        }

        public bool IsSameAs(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlapped(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlapped(in second_lo, in second_hi));
            return
                IsSameAs(in first_lo, in second_lo) &&
                IsSameAs(in first_hi, in second_hi);
        }

        public bool IsSameAsOrNotOverlapped(in T first_lo, in T first_hi, in T second_lo, in T second_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlapped(in first_lo, in first_hi));
            System.Diagnostics.Debug.Assert(!IsOverlapped(in second_lo, in second_hi));
            return
                IsSameAs(in first_lo, in first_hi, in second_lo, in second_hi) ||
                !IsOverlapped(in first_lo, in first_hi, in second_lo, in second_hi);
        }

        public CanonicalIntegerBoolean IncreaseUnsignedNoThrow(in T value_lo, in T value_hi, out T result_lo, out T result_hi) {
            return ConditionalIncreaseUnsignedNoThrow(IncreaseUnsignedNoThrow(in value_lo, out result_lo), in value_hi, out result_hi);
        }



        public CanonicalIntegerBoolean IncreaseUnchecked(ref T value_result_lo, ref T value_result_hi) {
            System.Diagnostics.Debug.Assert(!IsOverlapped(in value_result_lo, in value_result_hi));
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
            var carry = SubtractUnsignedNoThrow(in first_lo, in second_lo, out result_lo);
            ConditionalDecreaseUnchecked(carry, in first_hi, out result_hi);
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
