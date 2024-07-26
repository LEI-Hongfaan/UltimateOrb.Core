
using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class CilVerifiable {

        #region Address Comparison
        #region Managed Pointer
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ceq
            ret
        ")]
        public static bool Equals<T>(in T firstPtr, in T secondPtr) {
            return Unsafe.AreSame(ref Unsafe.AsRef(in firstPtr), ref Unsafe.AsRef(in secondPtr));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ret
        ")]
        public static bool LessThan<T>(in T firstPtr, in T secondPtr) {
            return Unsafe.IsAddressLessThan(ref Unsafe.AsRef(in firstPtr), ref Unsafe.AsRef(in secondPtr));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ret
        ")]
        public static bool LessThanSigned<T>(in T firstPtr, in T secondPtr) {
            Miscellaneous.IgnoreInParameter(firstPtr);
            Miscellaneous.IgnoreInParameter(secondPtr);
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
        public static bool LessThanOrEqual<T>(in T firstPtr, in T secondPtr) {
            return !GreaterThan(in firstPtr, in secondPtr);
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
        public static bool LessThanOrEqualSigned<T>(in T firstPtr, in T secondPtr) {
            Miscellaneous.IgnoreInParameter(firstPtr);
            Miscellaneous.IgnoreInParameter(secondPtr);
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool GreaterThan<T>(in T firstPtr, in T secondPtr) {
            return Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef(in firstPtr), ref Unsafe.AsRef(in secondPtr));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ret
        ")]
        public static bool GreaterThanSigned<T>(in T firstPtr, in T secondPtr) {
            Miscellaneous.IgnoreInParameter(firstPtr);
            Miscellaneous.IgnoreInParameter(secondPtr);
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
        public static bool GreaterThanOrEqual<T>(in T firstPtr, in T secondPtr) {
            return !LessThan(in firstPtr, in secondPtr);
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
        public static bool GreaterThanOrEqualSinged<T>(in T firstPtr, in T secondPtr) {
            Miscellaneous.IgnoreInParameter(firstPtr);
            Miscellaneous.IgnoreInParameter(secondPtr);
            throw null!;
        }
        #endregion

        #region Object Reference
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ceq
            ret
        ")]
        public static new bool Equals(object? firstObjRef, object? secondObjRef) {
            return firstObjRef == secondObjRef;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool GreaterThan(object? firstObjRef, object? secondObjRef) {
            Miscellaneous.IgnoreParameter(firstObjRef);
            Miscellaneous.IgnoreParameter(secondObjRef);
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.1
            ldarg.0
            cgt.un
            ret
        ")]
        public static bool LessThan(object? firstObjRef, object? secondObjRef) {
            Miscellaneous.IgnoreParameter(firstObjRef);
            Miscellaneous.IgnoreParameter(secondObjRef);
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
        public static bool GreaterThanOrEqual(object? firstObjRef, object? secondObjRef) {
            Miscellaneous.IgnoreParameter(firstObjRef);
            Miscellaneous.IgnoreParameter(secondObjRef);
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.1
            ldarg.0
            cgt.un
            ldc.i4.0
            ceq
            ret
        ")]
        public static bool LessThanOrEqual(object? firstObjRef, object? secondObjRef) {
            Miscellaneous.IgnoreParameter(firstObjRef);
            Miscellaneous.IgnoreParameter(secondObjRef);
            throw null!;
        }
        #endregion
        #endregion

        #region Boxing
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            unbox !!0
            ret
        ")]
        public static ref readonly T UnboxRef<T>(object box) where T : struct {
            Miscellaneous.IgnoreParameter(box);
            throw null!;
        }
        #endregion

        #region Native Integer
        #region Array
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            newarr !!0
            ret
        ")]
        public static T[] CreateArray<T>(nint length) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldlen
            ret
        ")]
        public static nint/* nuint */ GetLength<T>(T[] array) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ldelem !!0
            ret
        ")]
        public static T GetValue<T>(this T[] array, nint index) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ldelema !!0
            ret
        ")]
        public static ref T GetValueRef<T>(this T[] array, nint index) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            ldarg.2
            stelem !!0
            ret
        ")]
        public static void SetValue<T>(this T[] array, nint index, T value) {
            throw null!;
        }
        #endregion

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt.un
            ret
        ")]
        public static bool LessThan(nuint first, nuint second) {
            return first < second;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            clt
            ret
        ")]
        public static bool LessThan(nint first, nint second) {
            return first < second;
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
        public static bool LessThanOrEqual(nuint first, nuint second) {
            return first <= second;
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
        public static bool LessThanOrEqual(nint first, nint second) {
            return first <= second;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool GreaterThan(nuint first, nuint second) {
            return first > second;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            cgt
            ret
        ")]
        public static bool GreaterThan(nint first, nint second) {
            return first > second;
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
        public static bool GreaterThanOrEqual(nuint first, nuint second) {
            return first >= second;
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
        public static bool GreaterThanOrEqual(nint first, nint second) {
            return first >= second;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldc.i4.0
            conv.i
            ldarg.0
            sub.ovf
            ret
        ")]
        public static nint Negate(nint value) {
            return checked(-value);
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
        public static nuint Negate(nuint value) {
            return checked(0 - value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf
            ret
        ")]
        public static nint Add(nint first, nint second) {
            return checked(first + second);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            add.ovf.un
            ret
        ")]
        public static nuint Add(nuint first, nuint second) {
            return checked(first + second);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf
            ret
        ")]
        public static nint Subtract(nint first, nint second) {
            return checked(first - second);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            sub.ovf.un
            ret
        ")]
        public static nuint Subtract(nuint first, nuint second) {
            return checked(first - second);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf
            ret
        ")]
        public static nint Multiply(nint first, nint second) {
            return checked(first * second);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            mul.ovf.un
            ret
        ")]
        public static nuint Multiply(nuint first, nuint second) {
            return checked(first * second);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div
            ret
        ")]
        public static nint Divide(nint first, nint second) {
            return checked(first / second);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            div.un
            ret
        ")]
        public static nuint Divide(nuint first, nuint second) {
            return checked(first / second);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem
            ret
        ")]
        public static nint Remainder(nint first, nint second) {
            return checked(first % second);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            rem.un
            ret
        ")]
        public static nuint Remainder(nuint first, nuint second) {
            return checked(first % second);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static nint ShiftRight(nint value, nint count) {
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
        public static nuint ShiftRight(nuint value, nint count) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ldarg.1
            shr
            ret
        ")]
        public static nint ShiftRight(nint value, int count) {
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
        public static nuint ShiftRight(nuint value, int count) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static nint ToIntPtr(nuint value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static nint ToIntPtr(long value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i
            ret
        ")]
        public static nint ToIntPtr(int value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static nint ToIntPtr(ulong value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static nint ToIntPtr(uint value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static nuint ToUIntPtr(nuint value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static nuint ToUIntPtr(long value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static nuint ToUIntPtr(int value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static nuint ToUIntPtr(ulong value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u.un
            ret
        ")]
        public static nuint ToUIntPtr(uint value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.r.un
            conv.r8
            ret
        ")]
        public static Double TruncateToDouble(nuint value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.r8
            ret
        ")]
        public static Double TruncateToDouble(nint value) {
            throw null!;
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.r.un
            conv.r4
            ret
        ")]
        public static Single TruncateToSingle(nuint value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.r4
            ret
        ")]
        public static Single TruncateToSingle(nint value) {
            throw null!;
        }
        #endregion

        #region Sign Conversions
        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly uint AsUnsignedReadOnly(in int value) {
            return ref Unsafe.As<int, uint>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly ulong AsUnsignedReadOnly(in long value) {
            return ref Unsafe.As<long, ulong>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly nuint AsUnsignedReadOnly(in nint value) {
            return ref Unsafe.As<nint, nuint>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref uint AsUnsigned(ref int value) {
            return ref Unsafe.As<int, uint>(ref value);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref ulong AsUnsigned(ref long value) {
            return ref Unsafe.As<long, ulong>(ref value);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref nuint AsUnsigned(ref nint value) {
            return ref Unsafe.As<nint, nuint>(ref value);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly int AsSignedReadOnly(in uint value) {
            return ref Unsafe.As<uint, int>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly long AsSignedReadOnly(in ulong value) {
            return ref Unsafe.As<ulong, long>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref readonly nint AsSignedReadOnly(in nuint value) {
            return ref Unsafe.As<nuint, nint>(ref Unsafe.AsRef(in value));
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref int AsSigned(ref uint value) {
            return ref Unsafe.As<uint, int>(ref value);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref long AsSigned(ref ulong value) {
            return ref Unsafe.As<ulong, long>(ref value);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static ref nint AsSigned(ref nuint value) {
            return ref Unsafe.As<nuint, nint>(ref value);
        }
        #endregion

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            sizeof !!0
            ret
        ")]
        public static int SizeOf<T>() {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.r8
            dup
            ldarg.1
            conv.r8
            add
            neg
            add
            neg
            conv.r8
            ret
        ")]
        public static Double AddThenSubtractFirst(Double first, Double second) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ckfinite
            ret
        ")]
        public static Double CheckFinite(Double value) {
            throw null!;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ckfinite
            ret
        ")]
        public static Single CheckFinite(Single value) {
            throw null!;
        }
    }
}
