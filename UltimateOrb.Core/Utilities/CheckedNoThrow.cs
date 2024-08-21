using System;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using DoubleArithmetic = UltimateOrb.Numerics.DoubleArithmetic;

namespace UltimateOrb.Utilities {

    public static partial class CheckedNoThrow {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static bool IsLongArithmeticPreferred() {
            return SizeOfModule.SizeOf<nint>() >= SizeOfModule.SizeOf<long>();
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryNegate(uint value, out uint result) {
            result = unchecked((uint)(-(int)value));
            return value == uint.MinValue;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryNegate(int value, out int result) {
            result = unchecked(-value);
            return value != int.MinValue;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryNegate(ulong value, out ulong result) {
            result = unchecked((ulong)(-(long)value));
            return value == ulong.MinValue;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryNegate(long value, out long result) {
            result = unchecked(-value);
            return value != long.MinValue;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryAdd(uint first, uint second, out uint result) {
            var result_ = unchecked(first + second);
            result = result_;
            return first <= result_;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [PureAttribute()]
        public static bool TryAdd(int first, int second, out int result) {
            if (IsLongArithmeticPreferred()) {
                long t = (long)first + second;
                result = unchecked((int)t);
                return t == result;
            } else {
                result = unchecked(first + second);
                return 0 > ((first ^ second) | (unchecked((first ^ (~(first ^ second) & int.MinValue)) + second) ^ second));
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryAdd(ulong first, ulong second, out ulong result) {
            return first <= (result = unchecked(first + second));
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryAdd(long first, long second, out long result) {
            result = unchecked(first + second);
            return 0 > ((first ^ second) | (unchecked((first ^ (~(first ^ second) & long.MinValue)) + second) ^ second));
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TrySubtract(uint first, uint second, out uint result) {
            result = unchecked(first - second);
            return first >= second;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        [PureAttribute()]
        public static bool TrySubtract(int first, int second, out int result) {
            if (IsLongArithmeticPreferred()) {
                long t = (long)first - second;
                result = unchecked((int)t);
                return t == result;
            } else {
                var t = unchecked(first - second);
                result = t;
                if (0 <= first) {
                    if ((0 <= second) || (first <= t)) {
                        return true;
                    }
                } else {
                    if ((0 > second) || (t <= first)) {
                        return true;
                    }
                }
                return false;
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TrySubtract(ulong first, ulong second, out ulong result) {
            result = unchecked(first - second);
            return first >= second;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TrySubtract(long first, long second, out long result) {
            var t = unchecked(first - second);
            result = t;
            if (0 <= first) {
                if ((0 <= second) || (first <= t)) {
                    return true;
                }
            } else {
                if ((0 > second) || (t <= first)) {
                    return true;
                }
            }
            return false;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryMultiply(uint first, uint second, out uint result) {
            ulong t = (ulong)first * second;
            result = unchecked((uint)t);
            return t == result;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryMultiply(int first, int second, out int result) {
            long t = (long)first * second;
            result = unchecked((int)t);
            return t == result;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryMultiply(ulong first, ulong second, out ulong result) {
            ulong t;
            result = DoubleArithmetic.BigMul(first, second, out t);
            return 0 == t;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryMultiply(long first, long second, out long result) {
            long t;
            var r = DoubleArithmetic.BigMul(first, second, out t);
            result = unchecked((long)r);
            if (0 > (first ^ second)) {
                if ((-1 == t && 0 > r) || (0 == t && 0 == r)) {
                    return true;
                }
            } else {
                if (0 == t && r <= (ulong)long.MaxValue) {
                    return true;
                }
            }
            return false;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryDivide(uint first, uint second, out uint result) {
            result = first / second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryDivide(int first, int second, out int result) {
            result = unchecked(first / second);
            return int.MinValue != first || -1 != second;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryDivide(ulong first, ulong second, out ulong result) {
            result = first / second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryDivide(long first, long second, out long result) {
            result = unchecked(first / second);
            return long.MinValue != first || -1 != second;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder(uint first, uint second, out uint result) {
            result = first % second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder(int first, int second, out int result) {
            result = unchecked(first % second);
            return int.MinValue != first || -1 != second;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder(ulong first, ulong second, out ulong result) {
            result = first % second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder(long first, long second, out long result) {
            result = unchecked(first % second);
            return long.MinValue != first || -1 != second;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder_IgnoreDivideOverflow(uint first, uint second, out uint result) {
            result = first % second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder_IgnoreDivideOverflow(int first, int second, out int result) {
            result = unchecked(first % second);
            return true;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder_IgnoreDivideOverflow(ulong first, ulong second, out ulong result) {
            result = first % second;
            return true;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryRemainder_IgnoreDivideOverflow(long first, long second, out long result) {
            result = unchecked(first % second);
            return true;
        }
    }
}
