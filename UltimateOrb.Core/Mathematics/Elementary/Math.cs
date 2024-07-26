using System;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Diagnostics.Contracts;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

namespace UltimateOrb.Mathematics.Elementary {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class Math {

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 Sqrt_A_I(UInt64 radicand) {
            unchecked {
                var res = (UInt64)0;
                var bit = (UInt64)1 << (64 - 2);
                while (bit != 0) {
                    if (radicand >= res + bit) {
                        radicand -= res + bit;
                        res = (res >> 1) + bit;
                    } else {
                        res >>= 1;
                    }
                    bit >>= 2;
                }
                return res;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 Sqrt_A_I(UInt32 radicand) {
            unchecked {
                var res = (UInt32)0;
                var bit = (UInt32)1 << (32 - 2);
                while (bit != 0) {
                    if (radicand >= res + bit) {
                        radicand -= res + bit;
                        res = (res >> 1) + bit;
                    } else {
                        res >>= 1;
                    }
                    bit >>= 2;
                }
                return (UInt16)res;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 SqrtRem_A_I(UInt64 radicand, out UInt64 remainder) {
            unchecked {
                var res = (UInt64)0;
                var bit = (UInt64)1 << (64 - 2);
                while (bit != 0) {
                    if (radicand >= res + bit) {
                        radicand -= res + bit;
                        res = (res >> 1) + bit;
                    } else {
                        res >>= 1;
                    }
                    bit >>= 2;
                }
                remainder = radicand;
                return (UInt32)res;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SqrtRem_A_I(UInt32 radicand, out UInt32 remainder) {
            unchecked {
                var res = (UInt32)0;
                var bit = (UInt32)1 << (32 - 2);
                while (bit != 0) {
                    if (radicand >= res + bit) {
                        radicand -= res + bit;
                        res = (res >> 1) + bit;
                    } else {
                        res >>= 1;
                    }
                    bit >>= 2;
                }
                remainder = radicand;
                return res;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 Sqrt_A_F(UInt64 radicand) {
            unchecked {
                // truncated
                var t = (UInt32)System.Math.Sqrt(radicand);
                return 0 == t ? (0 == radicand ? 0u : ~(UInt32)0u) : ((UInt64)t * t > radicand ? --t : t);
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 Sqrt_A_F(UInt32 radicand) {
            unchecked {
                // truncated
                return (UInt16)(System.Math.Sqrt(radicand));
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 SqrtRem_A_F(UInt64 radicand, out UInt64 remainder) {
            unchecked {
                UInt64 s;
                var t = (UInt32)System.Math.Sqrt(radicand);
                if (0 == t) {
                    if (0 == radicand) {
                        remainder = 0;
                        return 0;
                    } else {
                        remainder = radicand - (UInt64)(~(UInt32)0u) * (~(UInt32)0u);
                        return ~(UInt32)0u;
                    }
                }
                s = (UInt64)t * t;
                if (s > radicand) {
                    remainder = radicand - 1 - (s - (t << 1));
                    return t - 1;
                } else {
                    remainder = radicand - s;
                    return t;
                }
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SqrtRem_A_F(UInt32 radicand, out UInt32 remainder) {
            unchecked {
                var r = Sqrt_A_F(radicand);
                remainder = radicand - r * r;
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        // ~34.9 Cyc
        public static UInt64 Sqrt(UInt64 radicand) {
            return Sqrt_A_F(radicand);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 Sqrt(UInt32 radicand) {
            return Sqrt_A_F(radicand);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 SqrtRem(UInt64 radicand, out UInt64 remainder) {
            return SqrtRem_A_F(radicand, out remainder);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SqrtRem(UInt32 radicand, out UInt32 remainder) {
            return SqrtRem_A_F(radicand, out remainder);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 Pow(UInt64 @base, uint exponent) {
            if (0u == exponent) {
                return (UInt64)1u;
            }
            var result = (UInt64)1u;
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    checked {
                        result *= @base;
                    }
                    exponent >>= 1;
                    if (0u == exponent) {
                        return result;
                    }
                } else {
                    exponent >>= 1;
                }
                checked {
                    @base *= @base;
                }
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 Pow(UInt32 @base, uint exponent) {
            if (0u == exponent) {
                return (UInt32)1u;
            }
            var result = (UInt32)1u;
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    checked {
                        result *= @base;
                    }
                    exponent >>= 1;
                    if (0u == exponent) {
                        return result;
                    }
                } else {
                    exponent >>= 1;
                }
                checked {
                    @base *= @base;
                }
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint Ceiling(uint value, uint divisor) {
            unchecked {
                if (divisor > 1u) {
                    var t = value % divisor;
                    return (t == 0u ? value : checked(unchecked(divisor - t) + value));
                }
                return value;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint Floor(uint value, uint divisor) {
            unchecked {
                if (divisor > 1u) {
                    var t = value % divisor;
                    return value - t;
                }
                return value;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int Ceiling(int value, int divisor) {
            unchecked {
                var d = AbsAsUnsigned(divisor);
                return 0 > value ? -(int)Floor((uint)(-value), d) : (int)Ceiling((uint)value, d);
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int Floor(int value, int divisor) {
            unchecked {
                var d = AbsAsUnsigned(divisor);
                return 0 > value ? -(int)Ceiling((uint)(-value), d) : (int)Floor((uint)value, d);
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong Ceiling(ulong value, ulong divisor) {
            unchecked {
                if (divisor > 1u) {
                    var t = value % divisor;
                    return (t == 0u ? value : checked(unchecked(divisor - t) + value));
                }
                return value;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong Floor(ulong value, ulong divisor) {
            unchecked {
                if (divisor > 1u) {
                    var t = value % divisor;
                    return value - t;
                }
                return value;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static long Ceiling(long value, long divisor) {
            unchecked {
                var d = AbsAsUnsigned(divisor);
                return 0 > value ? -(long)Floor((ulong)(-value), d) : (long)Ceiling((ulong)value, d);
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static long Floor(long value, long divisor) {
            unchecked {
                var d = AbsAsUnsigned(divisor);
                return 0 > value ? -(long)Ceiling((ulong)(-value), d) : (long)Floor((ulong)value, d);
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint DivideCeiling(uint dividend, uint divisor) {
            unchecked {
                ulong t = (divisor - 1u) + (ulong)dividend;
                return (uint)(t / divisor);
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong DivideCeiling(ulong dividend, ulong divisor) {
            unchecked {
#if NET6_0_OR_GREATER
                var (q, r) = System.Math.DivRem(dividend, divisor);
#else
                var q = global::Internal.System.Math.DivRem(dividend, divisor, out var r);
#endif
                return q + unchecked((uint)(0 < r).AsIntegerUnsafe());
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint DivideCeilingNoThrow(uint dividend, uint divisor) {
            unchecked {
                if (divisor > 0u) {
                    ulong t = (divisor - 1u) + (ulong)dividend;
                    return (uint)(t / divisor);
                }
                return 0u;
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong DivideCeilingNoThrow(ulong dividend, ulong divisor) {
            unchecked {
                if (divisor > 0u) {
#if NET6_0_OR_GREATER
                    var (q, r) = System.Math.DivRem(dividend, divisor);
#else
                    var q = global::Internal.System.Math.DivRem(dividend, divisor, out var r);
#endif
                    return q + unchecked((uint)(0 < r).AsIntegerUnsafe());
                }
                return 0u;
            }
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint AbsAsUnsigned(int value) {
            return 0 > value ? unchecked((uint)-value) : unchecked((uint)value);
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static uint AbsAsUnsigned(uint value) {
            return value;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static nuint AbsAsUnsigned(nint value) {
            return 0 > value ? unchecked((nuint)(-value)) : unchecked((nuint)value);
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static nuint AbsAsUnsigned(nuint value) {
            return value;
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong AbsAsUnsigned(long value) {
            return 0 > value ? unchecked((ulong)-value) : unchecked((ulong)value);
        }

        [CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static ulong AbsAsUnsigned(ulong value) {
            return value;
        }
    }
}
