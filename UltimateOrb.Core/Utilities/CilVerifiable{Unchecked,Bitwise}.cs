
using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Utilities {

    public static partial class CilVerifiable {

        #region Native Integer
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ceq
            ret
        ")]
        public static bool Equals(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ret
        ")]
        public static bool LessThanUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ret
        ")]
        public static bool LessThanSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool LessThanOrEqualUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool LessThanOrEqualSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool GreaterThanUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ret
        ")]
        public static bool GreaterThanSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool GreaterThanOrEqualUnsinged(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool GreaterThanOrEqualSinged(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            neg
            ret
        ")]
        public static IntPtr NegateUnchecked(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldc.i4.0
            conv.i
            ldarg.0
            sub.ovf
            ret
        ")]
        public static IntPtr NegateSigned(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldc.i4.0
            conv.i
            ldarg.0
            sub.ovf.un
            ret
        ")]
        public static IntPtr NegateUnsigned(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add
            ret
        ")]
        public static IntPtr AddUnchecked(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf
            ret
        ")]
        public static IntPtr AddSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf.un
            ret
        ")]
        public static IntPtr AddUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub
            ret
        ")]
        public static IntPtr SubtractUnchecked(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf
            ret
        ")]
        public static IntPtr SubtractSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf.un
            ret
        ")]
        public static IntPtr SubtractUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul
            ret
        ")]
        public static IntPtr MultiplyUnchecked(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf
            ret
        ")]
        public static IntPtr MultiplySigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf.un
            ret
        ")]
        public static IntPtr MultiplyUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div
            ret
        ")]
        public static IntPtr DivideSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div.un
            ret
        ")]
        public static IntPtr DivideUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem
            ret
        ")]
        public static IntPtr RemainderSigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem.un
            ret
        ")]
        public static IntPtr RemainderUnsigned(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shl
            ret
        ")]
        public static IntPtr ShiftLeft(IntPtr value, IntPtr count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static IntPtr ShiftRightSigned(IntPtr value, IntPtr count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr.un
            ret
        ")]
        public static IntPtr ShiftRightUnsigned(IntPtr value, IntPtr count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shl
            ret
        ")]
        public static IntPtr ShiftLeft(IntPtr value, int count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static IntPtr ShiftRightSigned(IntPtr value, int count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr.un
            ret
        ")]
        public static IntPtr ShiftRightUnsigned(IntPtr value, int count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            not
            ret
        ")]
        public static IntPtr Not(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            and
            ret
        ")]
        public static IntPtr And(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            or
            ret
        ")]
        public static IntPtr Or(IntPtr first, IntPtr second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            xor
            ret
        ")]
        public static IntPtr Xor(IntPtr first, IntPtr second) {
            throw null!;
        }
        #endregion
    }
}
namespace UltimateOrb.Utilities {

    public static partial class CilVerifiable {

        #region Native Integer
        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ceq
            ret
        ")]
        public static bool Equals(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ret
        ")]
        public static bool LessThanUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ret
        ")]
        public static bool LessThanSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool LessThanOrEqualUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool LessThanOrEqualSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool GreaterThanUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ret
        ")]
        public static bool GreaterThanSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool GreaterThanOrEqualUnsinged(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool GreaterThanOrEqualSinged(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            neg
            ret
        ")]
        public static UIntPtr NegateUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldc.i4.0
            conv.i
            ldarg.0
            sub.ovf
            ret
        ")]
        public static UIntPtr NegateSigned(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldc.i4.0
            conv.i
            ldarg.0
            sub.ovf.un
            ret
        ")]
        public static UIntPtr NegateUnsigned(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add
            ret
        ")]
        public static UIntPtr AddUnchecked(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf
            ret
        ")]
        public static UIntPtr AddSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf.un
            ret
        ")]
        public static UIntPtr AddUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub
            ret
        ")]
        public static UIntPtr SubtractUnchecked(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf
            ret
        ")]
        public static UIntPtr SubtractSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf.un
            ret
        ")]
        public static UIntPtr SubtractUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul
            ret
        ")]
        public static UIntPtr MultiplyUnchecked(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf
            ret
        ")]
        public static UIntPtr MultiplySigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf.un
            ret
        ")]
        public static UIntPtr MultiplyUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div
            ret
        ")]
        public static UIntPtr DivideSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div.un
            ret
        ")]
        public static UIntPtr DivideUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem
            ret
        ")]
        public static UIntPtr RemainderSigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem.un
            ret
        ")]
        public static UIntPtr RemainderUnsigned(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shl
            ret
        ")]
        public static UIntPtr ShiftLeft(UIntPtr value, IntPtr count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static UIntPtr ShiftRightSigned(UIntPtr value, IntPtr count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr.un
            ret
        ")]
        public static UIntPtr ShiftRightUnsigned(UIntPtr value, IntPtr count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shl
            ret
        ")]
        public static UIntPtr ShiftLeft(UIntPtr value, int count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static UIntPtr ShiftRightSigned(UIntPtr value, int count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr.un
            ret
        ")]
        public static UIntPtr ShiftRightUnsigned(UIntPtr value, int count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            not
            ret
        ")]
        public static UIntPtr Not(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            and
            ret
        ")]
        public static UIntPtr And(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            or
            ret
        ")]
        public static UIntPtr Or(UIntPtr first, UIntPtr second) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            xor
            ret
        ")]
        public static UIntPtr Xor(UIntPtr first, UIntPtr second) {
            throw null!;
        }
        #endregion
    }
}

namespace UltimateOrb.Utilities {

    public static partial class CilVerifiable {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(Int32 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(Int32 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(Int32 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(Int32 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(Int32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(UInt32 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(UInt32 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(Int64 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(Int64 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(Int64 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(Int64 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(Int64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(UInt64 value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(UInt64 value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(IntPtr value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(IntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static IntPtr ToIntPtrSignedUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.i
            ret
        ")]
        public static UIntPtr ToUIntPtrSignedUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnsignedUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsignedUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static IntPtr ToIntPtrUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static UIntPtr ToUIntPtrUnchecked(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static IntPtr ToIntPtrSigned(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUIntPtrSigned(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToIntPtrUnsigned(UIntPtr value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static UIntPtr ToUIntPtrUnsigned(UIntPtr value) {
            throw null!;
        }
    }
}