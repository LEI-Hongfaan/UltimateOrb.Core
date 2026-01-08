using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using UltimateOrb.Numerics;

namespace UltimateOrb.Numerics {

    using Int = Int32;
    using Long = Int64;
    using Math = System.Math;
    using MathEx = DoubleArithmetic;
    using Misc = DoubleArithmetic.Misc32;
    using UInt = UInt32;
    using ULong = UInt64;

    public static partial class DoubleArithmetic {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static ulong AdjustSquareRoot(ulong radicand_lo, ulong radicand_hi, ulong candidate) {
            unchecked {
                var lo = DoubleArithmetic.BigSquare(candidate, out var hi);
                var rem_lo = DoubleArithmetic.SubtractUnchecked(radicand_lo, radicand_hi, lo, hi, out var rem_hi);
                lo = DoubleArithmetic.ShiftLeft(candidate, 0, out hi);
                return DoubleArithmetic.LessThanOrEqual(rem_lo, rem_hi, lo, hi) ? candidate : 1 + candidate;
            }
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 Sqrt_A_F2(UInt64 radicand_lo, UInt64 radicand_hi) {
            unchecked {
                if (0 == radicand_hi) {
                    return Mathematics.Elementary.Math.Sqrt_A_F(radicand_lo);
                }
                UInt64 t;
                if (radicand_hi < (UInt64)0XFFFFFFFFFFFFFC00) {
                    // truncated
                    t = (UInt64)System.Math.Sqrt((double)new UInt128(lo: radicand_lo, hi: radicand_hi));
                } else {
                    t = (UInt64)((Int64)radicand_hi >> 1);
                }
                
                var q = DoubleArithmetic.BigDivNoThrowWhenOverflow(radicand_lo, radicand_hi, t);
                var e = (UInt64)((Int64)(q - t) >> 1);
                t += e;
                q = DoubleArithmetic.BigDivNoThrowWhenOverflow(radicand_lo, radicand_hi, t);
                e = (UInt64)((Int64)(q - t) >> 1);
                if (0 == e) {
                    return t;
                }
                t += e;
                return AdjustSquareRoot(radicand_lo, radicand_hi, t - 1);
                //Debugger.Log(3, "LOGIC", $"{e}\n");
            }
        }

        /*
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        private static ULong Abs(Long value) {
            unchecked {
                return 0 > value ? (ULong)(-value) : (ULong)value;
            }
        }
        */

        /// <summary>
        ///     <para>
        ///         Returns the square root of the specified value of an operand with double-precision data.
        ///     </para>
        /// </summary>
        /// <param name="lo">
        ///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <param name="hi">
        ///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
        /// </param>
        /// <returns>
        ///     <para>The square root.</para>
        /// </returns>
        /// <remarks>
        ///     <para>The result is rounded towards zero.</para>
        /// </remarks>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong BigSqrt(ULong lo, UInt hi) {
            unchecked {
                if (0u == hi) {
                    return Mathematics.Elementary.Math.Sqrt(lo);
                }
                var old = (ULong)Math.Sqrt((0.0 + ((ULong)1u << (Misc.ULong.BitSize - 1)) + ((ULong)1u << (Misc.ULong.BitSize - 1))) * hi + lo);
                ULong h;
                var l = MathEx.BigSquare(old, out h);
                l += lo;
                h += hi;
                if (l < lo) {
                    ++h;
                }
                var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                l = MathEx.BigSquare(@new, out h);
                return (h > hi) || ((h == hi) && (l > lo)) ? @new - (ULong)1u : @new;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ULong BigSqrtRem(ULong lo, UInt hi, out ULong remainder) {
            unchecked {
                if (0u == hi) {
                    return Mathematics.Elementary.Math.SqrtRem(lo, out remainder);
                }
                var old = (ULong)Math.Sqrt((0.0 + ((ULong)1u << (Misc.ULong.BitSize - 1)) + ((ULong)1u << (Misc.ULong.BitSize - 1))) * hi + lo);
                ULong h;
                var l = MathEx.BigSquare(old, out h);
                l += lo;
                h += hi;
                if (l < lo) {
                    ++h;
                }
                var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                l = MathEx.BigSquare(@new, out h);
                if ((h > hi) || ((h == hi) && (l > lo))) {
                    remainder = lo - l - ((@new << 1) - 1u);
                    return @new - (ULong)1u;
                } else {
                    remainder = lo - l;
                    return @new;
                }
            }
        }

        /// <summary>
        ///     <para>Returns the square root of a specified value of an operand with double-precision data.</para>
        /// </summary>
        /// <param name="lo">
        ///     <para>The <c>lo</c> bits of the radicand.</para>
        /// </param>
        /// <param name="hi">
        ///     <para>The <c>hi</c> bits of the radicand.</para>
        /// </param>
        /// <returns>
        ///     <para>The square root.</para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ULong BigSqrt(ULong lo, ULong hi) {
            unchecked {
                if (0u == hi) {
                    return Mathematics.Elementary.Math.Sqrt(lo);
                }
                if (hi <= ~(ULong)0 >> 2) {
                    var old = (ULong)Math.Sqrt((0.0 + ((ULong)1u << (Misc.ULong.BitSize - 1)) + ((ULong)1u << (Misc.ULong.BitSize - 1))) * hi + lo);
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    l += lo;
                    h += hi;
                    if (l < lo) {
                        ++h;
                    }
                    var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);
                    return (h > hi) || ((h == hi) && (l > lo)) ? @new - (ULong)1u : @new;
                }
                {
                    if (hi == ~(ULong)0) {
                        return ~(ULong)0;
                    }
                    var old = (ULong)(((ULong)1u << (Misc.UInt.BitSize - 1)) * Math.Sqrt(hi));
                    var a = lo;
                    lo = (lo >> 2) | (hi << (Misc.ULong.BitSize - 2));
                    hi = hi >> 2;
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    l += lo;
                    h += hi;
                    if (l < lo) {
                        ++h;
                    }
                    var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);
                    if ((h > hi) || ((h == hi) && (l > lo))) {
                        --@new;
                        @new <<= 1;
                        hi = (hi << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        h = (h << 2) + (ULong)(((l < a) ? -1 : 0) + ((0 > (Long)@new) ? -1 : 0) + ((int)(b >> (Misc.ULong.BitSize - 2))));
                        l -= a;
                    } else {
                        @new <<= 1;
                        hi = (hi << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        l += a;
                        h = (h << 2) + (((l < a) ? 1u : 0u) + ((0 > (Long)@new) ? 1u : 0u) + ((uint)(b >> (Misc.ULong.BitSize - 2))));
                    }
                    if ((h > hi) || ((h == hi) && (l > lo))) {
                    } else {
                        ++@new;
                    }
                    return @new;
                }
            }
        }

        public static ULong BigSqrtRem(ULong lo, ULong hi, out ULong remainder_lo, out uint remainder_hi) {
            unchecked {
                if (0u == hi) {
                    var root = Mathematics.Elementary.Math.SqrtRem(lo, out remainder_lo);
                    remainder_hi = 0u;
                    return root;
                }
                if (hi <= ~(ULong)0 >> 2) {
                    var old = (ULong)Math.Sqrt((0.0 + ((ULong)1u << (Misc.ULong.BitSize - 1)) + ((ULong)1u << (Misc.ULong.BitSize - 1))) * hi + lo);
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    l += lo;
                    h += hi;
                    if (l < lo) {
                        ++h;
                    }
                    var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);
                    if ((h > hi) || ((h == hi) && (l > lo))) {
                        l = SubtractUnchecked(l, h, @new, default, out h);
                        --@new;
                        l = SubtractUnchecked(l, h, @new, default, out h);
                    } else {
                    }
                    var rem_lo = SubtractUnchecked(lo, hi, l, h, out var rem_hi);
                    remainder_lo = rem_lo;
                    remainder_hi = (uint)rem_hi;
                    return @new;
                }
                {
                    if (hi == ~(ULong)0) {
                        remainder_lo = lo - 1;
                        remainder_hi = lo >= 1 ? 1u : 0u;
                        return ~(ULong)0;
                    }
                    var old = (ULong)(((ULong)1u << (Misc.UInt.BitSize - 1)) * Math.Sqrt(hi));
                    var a = lo;
                    lo = (lo >> 2) | (hi << (Misc.ULong.BitSize - 2));
                    hi = hi >> 2;
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    l += lo;
                    h += hi;
                    if (l < lo) {
                        ++h;
                    }
                    var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);

                    if ((h > hi) || ((h == hi) && (l > lo))) {
                        --@new;
                        @new <<= 1;
                        hi = (hi << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        h = (h << 2) + (ULong)(((l < a) ? -1 : 0) + ((0 > (Long)@new) ? -1 : 0) + ((int)(b >> (Misc.ULong.BitSize - 2))));
                        l -= a;
                    } else {
                        @new <<= 1;
                        hi = (hi << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        l += a;
                        h = (h << 2) + (((l < a) ? 1u : 0u) + ((0 > (Long)@new) ? 1u : 0u) + ((uint)(b >> (Misc.ULong.BitSize - 2))));
                    }
                    if ((h > hi) || ((h == hi) && (l > lo))) {
                    } else {
                        l = AddUnchecked(l, h, @new, default, out h);
                        ++@new;
                        l = AddUnchecked(l, h, @new, default, out h);
                    }
                    var rem_lo = SubtractUnchecked(lo, hi, l, h, out var rem_hi);
                    remainder_lo = rem_lo;
                    remainder_hi = (uint)rem_hi;
                    return @new;
                }
            }
        }

        /// <summary>
        ///     <para>Returns the square root of a specified number.</para>
        /// </summary>
        /// <param name="radicand">
        ///     <para>The radicand.</para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         The truncated (upto double precision) value of the positive square root of <paramref name="radicand"/>;
        ///         that is, (of the return value)
        ///         <list type="bullet">
        ///             <item><term>the higher half</term><description>: the integral part of the square root -and-</description></item>
        ///             <item><term>the lower half</term><description>: the fractional part of the square root.</description></item>
        ///         </list>
        ///     </para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        // ~327. Cyc
        public static ULong SqrtDouble(ULong radicand) {
            unchecked {
                if (0u == radicand) {
                    return 0u;
                }
                /*
                if (radicand <= ~(ULong)0 >> 2) {
                    var old = (ULong)(((ULong)1u << (Misc.UInt.BitSize - 0)) * Math.Sqrt(radicand));
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    h += radicand;
                    var @new = MathEx.BigDivUnchecked(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);
                    if ((h > radicand) || ((h == radicand) && (l > 0u))) {
                        return @new - (ULong)1u;
                    } else {
                        return @new;
                    }
                }
                */
                {
                    if (radicand == ~(ULong)0) {
                        return ~(ULong)0;
                    }
                    var old = (ULong)(((ULong)1u << (Misc.UInt.BitSize - 1)) * Math.Sqrt(radicand));
                    var a = (ULong)0u;
                    var lo = radicand << (Misc.ULong.BitSize - 2);
                    radicand = radicand >> 2;
                    ULong h;
                    var l = MathEx.BigSquare(old, out h);
                    l += lo;
                    h += radicand;
                    if (l < lo) {
                        ++h;
                    }
                    var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
                    l = MathEx.BigSquare(@new, out h);
                    if ((h > radicand) || ((h == radicand) && (l > lo))) {
                        --@new;
                        @new <<= 1;
                        radicand = (radicand << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        h = (h << 2) + (ULong)(((l < a) ? -1 : 0) + ((0 > (Long)@new) ? -1 : 0) + ((int)(b >> (Misc.ULong.BitSize - 2))));
                        l -= a;
                    } else {
                        @new <<= 1;
                        radicand = (radicand << 2) | (lo >> (Misc.ULong.BitSize - 2));
                        lo = a;
                        a = (@new << 1) + 1u;
                        var b = l;
                        l <<= 2;
                        l += a;
                        h = (h << 2) + (((l < a) ? 1u : 0u) + ((0 > (Long)@new) ? 1u : 0u) + ((uint)(b >> (Misc.ULong.BitSize - 2))));
                    }
                    if ((h > radicand) || ((h == radicand) && (l > lo))) {
                    } else {
                        ++@new;
                    }
                    return @new;
                }
            }
        }
    }
}