using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
namespace UltimateOrb.Numerics.Generic {


    static partial class _Internal {

        public static Vector128<UInt64> BitwiseOr(Vector128<UInt64> first, Vector128<UInt64> second) {
            return Vector128.BitwiseOr(first, second);
        }
    }
}
namespace UltimateOrb.Numerics.Generic {
   
    public interface IDoubleArithmetic<TSelf, T> where TSelf : IDoubleArithmetic<TSelf, T> {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CheckUInt1(int condition) {
            Debug.Assert(0 == condition || 1 == condition);
            return condition;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static CanonicalIntegerBoolean CheckUInt1(CanonicalIntegerBoolean condition) {
            Debug.Assert(0 == condition.IntegerValue || 1 == condition.IntegerValue);
            return condition;
        }

        public static abstract int ByteSize {

            get;
        }

        internal static int CheckByteSize(int size) {
            Debug.Assert(1024 * 1024 > unchecked((uint)size));
            return size;
        }

        internal static int ByteSize_Checked {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => CheckByteSize(TSelf.ByteSize);
        }

        public static virtual int BitSize {

            get => unchecked(ByteSize_Checked << 3);
        }

        internal static int CheckBitSize(int size) {
            Debug.Assert(8 * 1024 * 1024 > unchecked((uint)size));
            return size;
        }

        internal static int BitSize_Checked {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => CheckBitSize(TSelf.BitSize);
        }

        public static abstract void BitwiseOr(out T result, T first, T second);

        public static virtual void BitwiseOr(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            TSelf.BitwiseOr(out result_lo, first_lo, second_lo);
            TSelf.BitwiseOr(out result_hi, first_hi, second_hi);
        }

        public static abstract bool IsZero(T value);

        public static virtual bool IsZero(T value_lo, T value_hi) {
            return ByteSize_Checked > 256 ? TSelf.IsZero_Unlikely(value_lo, value_hi) : TSelf.IsZero_Likely(value_lo, value_hi);
        }

        public static virtual bool IsZero_Likely(T value) {
            return TSelf.IsZero(value);
        }

        public static virtual bool IsZero_Likely(T value_lo, T value_hi) {
            TSelf.BitwiseOr(out var bits, value_lo, value_hi);
            return TSelf.IsZero_Likely(bits);
        }

        public static virtual bool IsZero_Unlikely(T value) {
            return TSelf.IsZero(value);
        }

        public static virtual bool IsZero_Unlikely(T value_lo, T value_hi) {
            return TSelf.IsZero_Unlikely(value_lo) && TSelf.IsZero_Unlikely(value_hi);
        }

        public static abstract int CountLeadingZeros(T value);

        static int CountLeadingZeros_Checked(T value) {
            return CheckBitSize(TSelf.CountLeadingZeros(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int CountLeadingZeros(T value_lo, T value_hi) {
            if (TSelf.IsZero_Unlikely(value_hi)) {
                return TSelf.CountLeadingZeros(value_hi);
            }
            return unchecked(BitSize_Checked + TSelf.CountLeadingZeros(value_lo));
        }

        public static abstract bool IsNegativeSigned(T value);

        public static abstract void ShiftLeft(out T result, T value, int count);

        public static abstract void ShiftRightUnsigned(out T result, T value, int count);

        public static abstract void ShiftRightSigned(out T result, T value, int count);

        public static abstract void BigShiftLeftUnsigned(out T result, T value, T carries, int count);

        public static abstract void BigShiftRightUnsigned(out T result, T value, T borrows, int count);

        public static abstract void BigShiftRightSigned(out T result, T value, T borrows, int count);

        public static abstract void DivRemUnchecked(out T result, out T remainder, T dividend, T divisor);

        public static abstract void MultiplyUnchecked(out T result, T first, T second);

        public static abstract void BigMul(out T result_lo, out T result_hi, T first, T second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void BigMul(out T result_lo_lo, out T result_lo_hi, out T result_hi_lo, out T result_hi_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            if (unchecked((uint)Unsafe.SizeOf<T>()) <= 2 * unchecked((uint)Unsafe.SizeOf<nint>())) {
                TSelf.BigMul_A_Naive(out result_lo_lo, out result_lo_hi, out result_hi_lo, out result_hi_hi, first_lo, first_hi, second_lo, second_hi);
            } else {
                TSelf.BigMul_A_Karatsuba(out result_lo_lo, out result_lo_hi, out result_hi_lo, out result_hi_hi, first_lo, first_hi, second_lo, second_hi);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void BigMul_A_Naive(out T result_lo_lo, out T result_lo_hi, out T result_hi_lo, out T result_hi_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            TSelf.BigMul(out var lll, out var llh, first_lo, second_lo);
            TSelf.BigMul(out var hll, out var hlh, first_hi, second_lo);
            TSelf.AddUnchecked(out hll, out hlh, hll, hlh, llh, default(ZeroT));
            TSelf.BigMul(out var lhl, out var lhh, first_lo, second_hi);
            TSelf.AddUnchecked(out lhl, out lhh, lhl, lhh, hll, default(ZeroT));
            TSelf.BigMul(out var hhl, out var hhh, first_hi, second_hi);
            var c = TSelf.AddUnsignedNoThrow(out var th, hlh, lhh);
            Debug.Assert(0 == c || 1 == c);
            TSelf.AddUnchecked(out hhl, out hhh, hhl, hhh, th, default(ZeroT));
            TSelf.ConditionalIncreaseUnchecked(out hhh, hhh, c);
            result_lo_lo = lll;
            result_lo_hi = lhl;
            result_hi_lo = hhl;
            result_hi_hi = hhh;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void BigMul_A_Karatsuba(out T result_lo_lo, out T result_lo_hi, out T result_hi_lo, out T result_hi_hi, T first_lo, T first_hi, T second_lo, T second_hi) {

            TSelf.BigMul(out var lll, out var llh, first_lo, second_lo);
            TSelf.BigMul(out var hhl, out var hhh, first_hi, second_hi);
            var fc = TSelf.AddUnsignedNoThrow(out var fm, first_hi, first_lo);
            var sc = TSelf.AddUnsignedNoThrow(out var sm, second_hi, second_lo);
            TSelf.BigMul(out var mml, out var mmh, fm, sm);
            var mc = unchecked(CheckUInt1(TSelf.AddUnsignedNoThrow(out mml, out mmh, mml, mmh, llh, hhl)) - CheckUInt1(TSelf.AddUnsignedNoThrow(out var nnl, out var nnh, hhl, hhh, lll, llh)));
            mc = unchecked(mc - CheckUInt1(TSelf.SubtractUnsignedNoThrow(out llh, out hhl, mml, mmh, nnl, nnh)));
            uint h = unchecked((uint)(fc & sc));
            if (0 != fc) {
                TSelf.ExtendedAddUnchecked(out hhl, out h, hhl, h, sm, default(ZeroT));
            }
            if (0 != sc) {
                TSelf.ExtendedAddUnchecked(out hhl, out h, hhl, h, fm, default(ZeroT));
            }
            h = unchecked(h + (uint)mc);
            if (0 > unchecked((int)h)) {
                TSelf.DecreaseUnchecked(out hhh, hhh);
            } else {
                TSelf.AddUnchecked(out hhh, hhh, h);
            }
            result_lo_lo = lll;
            result_lo_hi = llh;
            result_hi_lo = hhl;
            result_hi_hi = hhh;
            /*

            TSelf.BigMul(out var lll, out var llh, first_lo, second_lo);
            TSelf.BigMul(out var hhl, out var hhh, first_hi, second_hi);
            var fc = CheckUInt1(TSelf.AddUnsignedNoThrow(out var fm, first_hi, first_lo));
            var sc = CheckUInt1(TSelf.AddUnsignedNoThrow(out var sm, second_hi, second_lo));
            TSelf.AddUnchecked(out var tl, out var th, hhl, hhh, lll, llh);
            var dh = unchecked((uint)fc + (uint)sc);
            TSelf.BigMul(out var mml, out var mmh, fm, sm);
            if (0 != fc) {
                TSelf.AddUnchecked(out mmh, mmh, sm);
            }
            if (0 != sc) {
                TSelf.AddUnchecked(out mmh, mmh, fm);
            }
            TSelf.SubtractUnchecked(out mml, out mmh, mml, mmh, tl, th);
            var mc = CheckUInt1(TSelf.AddUnsignedNoThrow(out var dl, fm, sm));
            unchecked {
                dh += (uint)mc;
            }
            TSelf.ConditionalIncreaseUnchecked(out hhl, out hhh, hhl, hhh, CheckUInt1(TSelf.AddUnsignedNoThrow(out llh, llh, mml)));
            TSelf.AddUnchecked(out hhl, out hhh, hhl, hhh, mmh, default(ZeroT));
            TSelf.ExtendedShiftRightUnsigned(out dl, out dh, dl, dh);
            TSelf.ExtendedSubtractUnchecked(out dl, out dh, dl, dh, mmh, default(ZeroT));
            TSelf.AddUnchecked(out hhh, hhh, dh);
            result_lo_lo = lll;
            result_lo_hi = llh;
            result_hi_lo = hhl;
            result_hi_hi = hhh;
            */
        }

        public static abstract int ShiftRightExactUnsigned(out T result, T value, int borrow);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int ShiftRightExactUnsigned(out T result_lo, out T result_hi, T value_lo, T value_hi, int borrow) {
            var b = CheckUInt1(TSelf.ShiftRightExactUnsigned(out result_lo, value_lo, CheckUInt1(TSelf.ShiftRightExactUnsigned(out var t, value_hi, borrow))));
            result_hi = t;
            return b;
        }

        public static abstract void ShiftRightUnsigned(out T result, T value, CanonicalIntegerBoolean borrow);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ShiftRightUnsigned(out T result_lo, out T result_hi, T value_lo, T value_hi, int borrow) {
            TSelf.ShiftRightUnsigned(out result_lo, value_lo, TSelf.ShiftRightExactUnsigned(out var t, value_hi, borrow));
            result_hi = t;
        }

        public static abstract void ShiftRightUnsigned(out T result, T value);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ShiftRightUnsigned(out T result_lo, out T result_hi, T value_lo, T value_hi) {
            TSelf.ShiftRightUnsigned(out result_hi, value_hi, CheckUInt1(TSelf.IncreaseUnsignedNoThrow(out result_lo, value_lo)));
        }

        public static abstract int ExtendedShiftRightExact(out T result, out uint result_ex, T value, uint value_ex, int borrow);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int ExtendedShiftRightExact(out T result_lo, out T result_hi, out uint result_ex, T value_lo, T value_hi, uint value_ex, int borrow) {
            var b = CheckUInt1(TSelf.ShiftRightExactUnsigned(out result_lo, value_lo, CheckUInt1(TSelf.ExtendedShiftRightExact(out var hi, out var ex, value_hi, value_ex, borrow))));
            (result_hi, result_ex) = (hi, ex);
            return b;
        }

        public static abstract int ExtendedShiftRightExactUnsigned(out T result, out uint result_ex, T value, uint value_ex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int ExtendedShiftRightExactUnsigned(out T result_lo, out T result_hi, out uint result_ex, T value_lo, T value_hi, uint value_ex) {
            var b = CheckUInt1(TSelf.ShiftRightExactUnsigned(out result_lo, value_lo, CheckUInt1(TSelf.ExtendedShiftRightExactUnsigned(out var hi, out var ex, value_hi, value_ex))));
            (result_hi, result_ex) = (hi, ex);
            return b;
        }

        public static abstract void ExtendedShiftRightUnsigned(out T result, out uint result_ex, T value, uint value_ex);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ExtendedShiftRightUnsigned(out T result_lo, out T result_hi, out uint result_ex, T value_lo, T value_hi, uint value_ex) {
            TSelf.ShiftRightUnsigned(out result_lo, value_lo, CheckUInt1(TSelf.ExtendedShiftRightExactUnsigned(out var hi, out var ex, value_hi, value_ex)));
            (result_hi, result_ex) = (hi, ex);
        }

        public static abstract void AddWithCarryUnchecked(out T result, T first, T second, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void AddWithCarryUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi, int condition) {
            TSelf.AddWithCarryUnchecked(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.AddWithCarryUnsignedNoThrow(out result_lo, first_lo, second_lo, condition)));
        }

        public static abstract void AddUnchecked(out T result, T first, uint second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void AddUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, uint second) {
            TSelf.ConditionalIncreaseUnchecked(out result_hi, first_hi, CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second)));
        }

        public static abstract int AddUnsignedNoThrow(out T result, T first, uint second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int AddUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, uint second) {
            return TSelf.ConditionalIncreaseUnsignedNoThrow(out result_hi, first_hi, CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second)));
        }

        public static abstract void AddUnchecked(out T result, T first, T second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void AddUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            TSelf.AddWithCarryUnchecked(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void AddUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, ZeroT second_hi) {
            TSelf.ConditionalIncreaseUnchecked(out result_hi, first_hi, CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }

        public static abstract int AddUnsignedNoThrow(out T result, T first, T second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int AddUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            return CheckUInt1(TSelf.AddWithCarryUnsignedNoThrow(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second_lo))));
        }

        public static abstract int AddWithCarryUnsignedNoThrow(out T result, T first, T second, int condition);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int AddWithCarryUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi, int condition) {
            return CheckUInt1(TSelf.AddWithCarryUnsignedNoThrow(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.AddWithCarryUnsignedNoThrow(out result_lo, first_lo, second_lo, condition))));
        }

        public static abstract int IncreaseUnsignedNoThrow(out T result, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int IncreaseUnsignedNoThrow(out T result_lo, out T result_hi, T value_lo, T value_hi) {
            return CheckUInt1(TSelf.ConditionalIncreaseUnsignedNoThrow(out result_hi, value_hi, CheckUInt1(TSelf.IncreaseUnsignedNoThrow(out result_lo, value_lo))));
        }

        public static abstract void IncreaseUnchecked(out T result, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void IncreaseUnchecked(out T result_lo, out T result_hi, T value_lo, T value_hi) {
            TSelf.ConditionalIncreaseUnchecked(out result_hi, value_hi, CheckUInt1(TSelf.IncreaseUnsignedNoThrow(out result_lo, value_lo)));
        }

        public static abstract void ConditionalIncreaseUnchecked(out T result, T value, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ConditionalIncreaseUnchecked(out T result_lo, out T result_hi, T value_lo, T value_hi, int condition) {
            CheckUInt1(condition);
            if (0 != condition) {
                TSelf.ConditionalIncreaseUnchecked(out result_hi, value_hi, CheckUInt1(TSelf.IncreaseUnsignedNoThrow(out result_lo, value_lo)));
                return;
            }
            result_lo = value_lo;
            result_hi = value_hi;
        }

        public static abstract int ConditionalIncreaseUnsignedNoThrow(out T result, T value, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int ConditionalIncreaseUnsignedNoThrow(out T result_lo, out T result_hi, T value_lo, T value_hi, int condition) {
            CheckUInt1(condition);
            if (0 != condition) {
                return CheckUInt1(TSelf.ConditionalIncreaseUnsignedNoThrow(out result_hi, value_hi, CheckUInt1(TSelf.IncreaseUnsignedNoThrow(out result_lo, value_lo))));
            }
            result_lo = value_lo;
            result_hi = value_hi;
            return 0;
        }





        public static virtual void ExtendedAddUnchecked(out T result, out uint result_ex, T first, uint first_ex, T second, ZeroT second_ex, int borrow) {
            var b = TSelf.AddWithCarryUnsignedNoThrow(out result, first, second, borrow);
            CheckUInt1(b);
            result_ex = unchecked(first_ex + (uint)b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ExtendedAddUnchecked(out T result_lo, out T result_hi, out uint result_ex, T first_lo, T first_hi, uint first_ex, T second_lo, T second_hi, ZeroT second_ex, int condition) {
            TSelf.ExtendedAddUnchecked(out result_hi, out result_ex, first_hi, first_ex, second_hi, default(ZeroT), CheckUInt1(TSelf.AddWithCarryUnsignedNoThrow(out result_lo, first_lo, second_lo, condition)));
        }


        public static virtual void ExtendedAddUnchecked(out T result, out uint result_ex, T first, uint first_ex, T second, ZeroT second_ex) {
            var b = TSelf.AddUnsignedNoThrow(out result, first, second);
            CheckUInt1(b);
            result_ex = unchecked(first_ex + (uint)b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ExtendedAddUnchecked(out T result_lo, out T result_hi, out uint result_ex, T first_lo, T first_hi, uint first_ex, T second_lo, T second_hi, ZeroT second_ex) {
            TSelf.ExtendedAddUnchecked(out result_hi, out result_ex, first_hi, first_ex, second_hi, default(ZeroT), CheckUInt1(TSelf.AddUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }




        public static virtual void ExtendedSubtractUnchecked(out T result, out uint result_ex, T first, uint first_ex, T second, ZeroT second_ex, int borrow) {
            var b = TSelf.SubtractWithBorrowUnsignedNoThrow(out result, first, second, borrow);
            CheckUInt1(b);
            result_ex = unchecked(first_ex - (uint)b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ExtendedSubtractUnchecked(out T result_lo, out T result_hi, out uint result_ex, T first_lo, T first_hi, uint first_ex, T second_lo, T second_hi, ZeroT second_ex, int condition) {
            TSelf.ExtendedSubtractUnchecked(out result_hi, out result_ex, first_hi, first_ex, second_hi, default(ZeroT), CheckUInt1(TSelf.SubtractWithBorrowUnsignedNoThrow(out result_lo, first_lo, second_lo, condition)));
        }


        public static virtual void ExtendedSubtractUnchecked(out T result, out uint result_ex, T first, uint first_ex, T second, ZeroT second_ex) {
            var b = TSelf.SubtractUnsignedNoThrow(out result, first, second);
            CheckUInt1(b);
            result_ex = unchecked(first_ex - (uint)b);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ExtendedSubtractUnchecked(out T result_lo, out T result_hi, out uint result_ex, T first_lo, T first_hi, uint first_ex, T second_lo, T second_hi, ZeroT second_ex) {
            TSelf.ExtendedSubtractUnchecked(out result_hi, out result_ex, first_hi, first_ex, second_hi, default(ZeroT), CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }

        public static abstract void SubtractWithBorrowUnchecked(out T result, T first, T second, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void SubtractWithBorrowUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi, int condition) {
            TSelf.SubtractWithBorrowUnchecked(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.SubtractWithBorrowUnsignedNoThrow(out result_lo, first_lo, second_lo, condition)));
        }

        public static abstract void SubtractUnchecked(out T result, T first, uint second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void SubtractUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, uint second) {
            TSelf.ConditionalDecreaseUnchecked(out result_hi, first_hi, CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second)));
        }

        public static abstract int SubtractUnsignedNoThrow(out T result, T first, uint second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int SubtractUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, uint second) {
            return TSelf.ConditionalDecreaseUnsignedNoThrow(out result_hi, first_hi, CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second)));
        }

        public static abstract void SubtractUnchecked(out T result, T first, T second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void SubtractUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            TSelf.SubtractWithBorrowUnchecked(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void SubtractUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, ZeroT second_hi) {
            TSelf.ConditionalDecreaseUnchecked(out result_hi, first_hi, CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second_lo)));
        }

        public static abstract int SubtractUnsignedNoThrow(out T result, T first, T second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int SubtractUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            return CheckUInt1(TSelf.SubtractWithBorrowUnsignedNoThrow(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.SubtractUnsignedNoThrow(out result_lo, first_lo, second_lo))));
        }

        public static abstract int SubtractWithBorrowUnsignedNoThrow(out T result, T first, T second, int condition);


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int SubtractWithBorrowUnsignedNoThrow(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi, int condition) {
            return CheckUInt1(TSelf.SubtractWithBorrowUnsignedNoThrow(out result_hi, first_hi, second_hi, CheckUInt1(TSelf.SubtractWithBorrowUnsignedNoThrow(out result_lo, first_lo, second_lo, condition))));
        }

        public static abstract int DecreaseUnsignedNoThrow(out T result, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int DecreaseUnsignedNoThrow(out T result_lo, out T result_hi, T value_lo, T value_hi) {
            return CheckUInt1(TSelf.ConditionalDecreaseUnsignedNoThrow(out result_hi, value_hi, CheckUInt1(TSelf.DecreaseUnsignedNoThrow(out result_lo, value_lo))));

        }

        public static abstract void DecreaseUnchecked(out T result, T value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void DecreaseUnchecked(out T result_lo, out T result_hi, T value_lo, T value_hi) {
            TSelf.ConditionalDecreaseUnchecked(out result_hi, value_hi, CheckUInt1(TSelf.DecreaseUnsignedNoThrow(out result_lo, value_lo)));
        }

        public static abstract void ConditionalDecreaseUnchecked(out T result, T value, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void ConditionalDecreaseUnchecked(out T result_lo, out T result_hi, T value_lo, T value_hi, int condition) {
            CheckUInt1(condition);
            if (0 != condition) {
                TSelf.ConditionalDecreaseUnchecked(out result_hi, value_hi, CheckUInt1(TSelf.DecreaseUnsignedNoThrow(out result_lo, value_lo)));
                return;
            }
            result_lo = value_lo;
            result_hi = value_hi;
        }

        public static abstract int ConditionalDecreaseUnsignedNoThrow(out T result, T value, int condition);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual int ConditionalDecreaseUnsignedNoThrow(out T result_lo, out T result_hi, T value_lo, T value_hi, int condition) {
            CheckUInt1(condition);
            if (0 != condition) {
                return CheckUInt1(TSelf.ConditionalDecreaseUnsignedNoThrow(out result_hi, value_hi, CheckUInt1(TSelf.DecreaseUnsignedNoThrow(out result_lo, value_lo))));
            }
            result_lo = value_lo;
            result_hi = value_hi;
            return 0;
        }

        public static virtual void MultiplyUnchecked(out T result_lo, out T result_hi, T first_lo, T first_hi, T second_lo, T second_hi) {
            TSelf.BigMul(out result_lo, out var result_hi_, first_lo, second_lo);
            TSelf.MultiplyUnchecked(out var flsh, first_lo, second_hi);
            TSelf.MultiplyUnchecked(out var fhsl, first_hi, second_lo);
            TSelf.AddUnchecked(out var t, flsh, fhsl);
            TSelf.AddUnchecked(out result_hi, t, result_hi_);
        }
        




        
        /*


        public static virtual void DivRemUnchecked(out TBase result_lo, out TBase result_hi, out TBase remainder_lo, out TBase remainder_hi, TBase dividend_lo, TBase dividend_hi, TBase divisor_lo, TBase divisor_hi) {
            unchecked {
                if (TSelf.IsZero_Unlikely(divisor_hi)) {
                    if (TSelf.IsNegativeSigned(divisor_hi)) {
                        TBase product_lo;
                        TBase product_hi;
                        TBase q_lo;
                        TBase t;
                        {
                            int cc;
                            cc = TSelf.CountLeadingZeros(divisor_hi);
                            t = divisor_hi << cc;
                            q_lo = TSelf.BigDivInternal(
                                dividend_lo >> (BitSize_Checked - cc) | (dividend_hi << cc),
                                dividend_hi >> (BitSize_Checked - cc),
                                (divisor_lo >> (BitSize_Checked - cc)) | t);
                        }
                        t = q_lo * divisor_hi;
                        product_lo = MathEx.BigMul(q_lo, divisor_lo, out product_hi);
                        product_hi += t;
                        if (t > product_hi || product_hi > dividend_hi) {
                            goto L_0001;
                        }
                        if (dividend_hi > product_hi || product_lo <= dividend_lo) {
                            goto L_0002;
                        }
                    L_0001:
                        --q_lo;
                        product_hi = ((divisor_lo > product_lo) ? (product_hi - divisor_hi - 1u) : (product_hi - divisor_hi));
                        product_lo -= divisor_lo;
                    L_0002:
                        highResult = 0u;
                        highRemainder = ((product_lo > dividend_lo) ? (dividend_hi - product_hi - 1u) : (dividend_hi - product_hi));
                        lowRemainder = dividend_lo - product_lo;
                        return q_lo;
                    } else {
                        highResult = 0u;
                        if (divisor_hi <= dividend_hi && (divisor_hi != dividend_hi || divisor_lo <= dividend_lo)) {
                            lowRemainder = MathEx.SubtractUnchecked(dividend_lo, dividend_hi, divisor_lo, divisor_hi, out highRemainder);
                            return 1u;
                        } else {
                            highRemainder = dividend_hi;
                            lowRemainder = dividend_lo;
                            return 0u;
                        }
                    }
                } else {
                    TBase t;
                    TSelf.DivRemUnchecked(out var highResult, out t, dividend_hi, divisor_lo);
                    highRemainder = 0u;
                    TSelf.BigDivRemInternal(out var q_lo, out lowRemainder, dividend_lo, t, divisor_lo);
                }
            }
        }
        */
    }
}
