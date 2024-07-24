using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UltimateOrb.Utilities.Extensions;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

namespace UltimateOrb.Utilities {

    [DiscardableAttribute()]
#if INDEPENDENT_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class ThrowHelper {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Ignore<T>(this T value) {
#pragma warning restore IDE0060 // Remove unused parameter
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static void IgnoreOutParameter<T>(out T value) {
            Miscellaneous.IgnoreOutParameter(out value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static T Nop<T>(this T value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
#pragma warning disable IDE0060 // Remove unused parameter
        public static T2 Comma<T1, T2>(this T1 first, T2 second) {
#pragma warning restore IDE0060 // Remove unused parameter
            return second;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNull<T>(this T value) {
            if (null == value) {
                throw null!;
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdC.I4.1
            LdC.I4.0
            Div
            Pop
            LdNull
            Throw
        ")]
        [DoesNotReturnAttribute()]
        public static DivideByZeroException ThrowDivideByZeroException() {
            var zero = 0;
            _ = 1 / zero;
            throw null!;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNegative(this int value) {
            checked((uint)value).Ignore();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNegative(this long value) {
            checked((ulong)value).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNegative(this uint value) {
            checked((uint)value).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNegative(this ulong value) {
            checked((ulong)value).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessThan(this uint first, uint second) {
            checked(first - second).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessThan(this ulong first, ulong second) {
            checked(first - second).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessThan(this int first, uint second) {
            checked((uint)first - second).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnLessThan(this nint first, nuint second) {
            checked((nuint)first - second).Ignore();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNotEqual(this int first, int second) {
            checked(0u - unchecked(first - second).ToUnsignedUnchecked()).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNotEqual(this uint first, uint second) {
            checked(0u - unchecked(first - second).ToUnsignedUnchecked()).Ignore();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNotEqual(this long first, long second) {
            checked(0u - unchecked(first - second).ToUnsignedUnchecked()).Ignore();
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnNotEqual(this ulong first, ulong second) {
            checked(0u - unchecked(first - second).ToUnsignedUnchecked()).Ignore();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static void Throw<TException>() where TException : Exception, new() {
            throw new TException();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        public static T Throw<TException, T>() where TException : Exception, new() {
            throw new TException();
        }

        [Obsolete]
        public static void ThrowOnInfinite(double value) {
            CilVerifiable.CheckFinite(value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnTrue(bool value) {
            _ = checked(0u - BooleanIntegerExtensions.AsUIntegerUnsafe(value));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ThrowOnFalse(bool value) {
            _ = checked(0u - BooleanIntegerExtensions.AsUIntegerUnsafe(value));
        }

        public static void ThrowOnNonFinite(double value) {
            CilVerifiable.CheckFinite(value);
        }

        public static void ThrowOnNonZero(int value) {
            _ = checked(0u - value.ToUnsignedUnchecked());
        }

        public static void ThrowOnNonZero(uint value) {
            _ = checked(0u - value.ToUnsignedUnchecked());
        }

        public static void ThrowOnNonZero(long value) {
            _ = checked(0u - value.ToUnsignedUnchecked());
        }

        public static void ThrowOnNonZero(ulong value) {
            _ = checked(0u - value.ToUnsignedUnchecked());
        }

        public static void ThrowOnLessThanOrEqual(double first, double second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnLessThanOrEqual(nuint first, nuint second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        public static void ThrowOnLessThanOrEqual(nint first, nint second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnLessThanOrEqual(uint first, uint second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        public static void ThrowOnLessThanOrEqual(int first, int second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnLessThanOrEqual(ulong first, ulong second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        public static void ThrowOnLessThanOrEqual(long first, long second) {
            ThrowOnNonZero(BooleanIntegerModule.LessThanOrEqual(first, second));
        }


        public static void ThrowOnGreaterThan(double first, double second) {
            ThrowOnNonZero(BooleanIntegerModule.GreaterThan(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnGreaterThan(nuint first, nuint second) {
            _ = checked(second - first);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdC.I4.0
            LdArg.0
            LdArg.1
            CGT
            Sub.Ovf.Un
            Ret
        ")]
        public static void ThrowOnGreaterThan(nint first, nint second) {
            ThrowOnNonZero(BooleanIntegerModule.GreaterThan(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnGreaterThan(uint first, uint second) {
            _ = checked(second - first);
        }

        public static void ThrowOnGreaterThan(int first, int second) {
            ThrowOnNonZero(BooleanIntegerModule.GreaterThan(first, second));
        }

        [CLSCompliant(false)]
        public static void ThrowOnGreaterThan(ulong first, ulong second) {
            _ = checked(second - first);
        }

        public static void ThrowOnGreaterThan(long first, long second) {
            ThrowOnNonZero(BooleanIntegerModule.GreaterThan(first, second));
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }
    }
}
