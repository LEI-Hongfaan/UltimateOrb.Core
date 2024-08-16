﻿using System;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

namespace UltimateOrb.Numerics {

    using Math = global::Internal.System.Math;

    public static partial class DoubleArithmetic {

        // TODO: Move to ...
        private static System.Double ToDoubleFUnchecked(UInt64 value, out System.Double result_hi) {
            unchecked {
                result_hi = (System.Double)(value >> (64 - (1 + 52)));
                return (System.Double)((((UInt64)1 << (64 - (1 + 52))) - 1) & value);
            }
        }
        
        // TODO: Move to ...
        private static UInt64 FromDoubleFUnchecked(System.Double value_lo, System.Double value_hi) {
            unchecked {
                return (UInt64)value_lo + (UInt64)value_hi;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static UInt64 DivRem_A_F(UInt64 dividend, UInt64 divisor, out UInt64 remainder) {
            unchecked {
                if (0 == divisor) {
                    goto L_Z;
                }
                var r_lo = ToDoubleFUnchecked(dividend, out var r_hi);
                var d_lo = ToDoubleFUnchecked(divisor, out var d_hi);
                var q_lo = Numerics.DoubleArithmeticF.DividePartial(r_lo, r_hi, d_lo, d_hi, out var q_hi);
                var q = FromDoubleFUnchecked(q_lo, q_hi);
                var p = divisor * q;
                var r = dividend - p;
                if (dividend < p) {
                    r += divisor;
                    --q;
                } else {
                    var t = r - divisor;
                    if (r >= divisor) {
                        r = t;
                        ++q;
                    }
                }
                remainder = r;
                return q;
            L_Z:;
                {
                    throw UltimateOrb.Utilities.ThrowHelper.ThrowDivideByZeroException();
                }
            }
        }
    }
}

namespace UltimateOrb.Numerics {
	using UInt = UInt32;
	using ULong = UInt64;
	using Int = Int32;
	using Long = Int64;

    using Math = global::Internal.System.Math;

    public static partial class DoubleArithmetic {

        /*
         * BigDivRem, BigDiv, BigRem:
         *   - The dividend has bigger size than the divisor's.
         *   - The dividend should be a fused-bigmul-add result.
         *   - The quotient can not overflow.
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * DivRem, Divide, Remainder:
         *   - (Unsigned cases) The quotient can not overflow. 
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * ~Unchecked:
         *   - Overflowed results are truncated.
         *   - Lower bits of results are correct on valid inputs.
         * ~Unsafe:
         *   - Used as a dependent routine.
         *   - No check on exceptionl conditions.
         *   - Undefined behavior on invalid inputs or exceptionl conditions.
         * ~Partial:
         *   - Used as a subroutine.
         *   - Works only on specialized conditions.
         *   - Undefined behavior on invalid inputs.
         * ~Internal:
         *   - Users should not use them directly.
         * ~NoThrowOnDivideByZero:
         *   - No throw on divide-by-zero conditions.
         * ~NoThrow:
         *   - No throw on exceptionl conditions.
         */
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRem(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
#if NET8_0_OR_GREATER
            if (System.Runtime.Intrinsics.X86.X86Base.X64.IsSupported) {
                var (q, r) = System.Runtime.Intrinsics.X86.X86Base.X64.DivRem(lowDividend, highDividend, divisor);
                remainder = r;
                return q;
            }
#endif
            unchecked {
                ULong p, ql, qh;
                // 2020Jan01
                if (0u != highDividend) {
                    if (UInt.MaxValue <= divisor) {
                        // 2013Dec24, 2014Jan08
                        //if (divisor <= highDividend) {
                        //    highDividend = checked(0u - highDividend);
                        //    throw (System.OverflowException)null;
                        //}
                        {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        int c = 0;
                        if (0 <= (Long)divisor) {
                            do {
                                ++c;
                                divisor <<= 1;
                            } while (0 <= (Long)divisor);
                            highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                            lowDividend <<= c;
                        }
                        var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                        var dl = (ULong)(UInt)divisor;
                        qh = Math.DivRem(highDividend, dh, out highDividend);
                        p = qh * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        if (highDividend < p) {
                            {
                                --qh;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --qh;
                                    highDividend += divisor;
                                }
                            }
                        }
                        highDividend -= p;
                        ql = Math.DivRem(highDividend, dh, out highDividend);
                        p = ql * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        if (highDividend < p) {
                            {
                                --ql;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --ql;
                                    highDividend += divisor;
                                }
                            }
                        }
                        remainder = (highDividend - p) >> c;
                        return (qh << Misc.UInt.BitSize) | ql;
                    } else {
                        // 2014Jan08
                        if (0u != divisor) {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        qh = Math.DivRem(highDividend, divisor, out highDividend);
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        ql = Math.DivRem(highDividend, divisor, out remainder);
                        return (qh << Misc.UInt.BitSize) | ql;
                    }
                } else {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                            }
                        }
                    }
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(ULong first, ULong second, out ULong highResult) {
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            highResult = global::System.Math.BigMul(first, second, out var lowResult);
            return lowResult;
#else
            unchecked {
                // 2019Dec15
                // Currently, we have fast multiplication instructions on most platforms.
                if (true || Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04, 2013Dec24
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    // if (lh < hl) {
                    //     hh += (ULong)1u << Misc.UInt.BitSize;
                    // }
                    // 2020Jan01
                    hh += (ULong)((lh < hl).AsIntegerUnsafe()) << Misc.UInt.BitSize;
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04, 2013Dec24
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
#endif
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(Long first, Long second, out Long highResult) {
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NET5_0_OR_GREATER
            highResult = global::System.Math.BigMul(first, second, out var lowResult);
            return unchecked((ULong)lowResult);
#else
            ULong r;
            var q = BigMul(unchecked((ULong)first), unchecked((ULong)second), out r);
            highResult = unchecked((Long)r - (-(Long)((ULong)first >> (Misc.ULong.BitSize - 1)) & second) - (-(Long)((ULong)second >> (Misc.ULong.BitSize - 1)) & first));
            return q;
#endif
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Karatsuba(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var hh = (ULong)fh * sh;
                    var fm = (UInt)fh + fl;
                    var sm = (UInt)sh + sl;
                    var mm = (ULong)(UInt)fm * (UInt)sm - (hh + ll);
                    var mh = (UInt)(mm >> Misc.UInt.BitSize) + (((UInt)fm < fl) ? (UInt)sm : 0u) + (((UInt)sm < sl) ? (UInt)fm : 0u);
                    var fs = ((ULong)fh + fl) + ((ULong)sh + sl);
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + ((ULong)mh + ((ll < ml) ? 1u : 0u)) + (((fs >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    // Bad for jitter:
                    // var mh = (ULong)(UInt)(mm >> Misc.UInt.BitSize);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    // Bad for jitter:
                    // highResult = hh + mh + ((ll < ml) ? 1u : 0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Long(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    // Bad for jitter:
                    // fl = (UInt)first;
                    var fl = (ULong)(UInt)first;
                    // Bad for jitter:
                    // var fh = (ULong)(first >> Misc.UInt.BitSize);
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // var ll = (ULong)(UInt)fl * (ULong)(UInt)sl;
                    var ll = fl * sl;
                    var lh = fl * sh;
                    var hl = fh * sl;
                    var hh = fh * sh;
                    // Bad for jitter:
                    // lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
            /* // old version
            var xh = first >> Misc.UInt.BitSize;
            var yh = second >> Misc.UInt.BitSize;
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var xl = unchecked((UInt)first * yh);
            var yl = unchecked(xh * (UInt)second);
            xl = unchecked(xl + yl);
            xh = unchecked(xh * yh);
            yh = unchecked(lo + (xl << Misc.UInt.BitSize));
            highResult = unchecked((xh + (UInt)(xl >> Misc.UInt.BitSize)) + ((lo > yh ? (ULong)1 : 0) + (yl > xl ? (ULong)1 << Misc.UInt.BitSize : 0)));
            return yh;
            */
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static UInt BigMul_M(ULong first, UInt second, out ULong lowResult) {
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            return unchecked((UInt)global::System.Math.BigMul(first, second, out lowResult));
#else
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var yl = unchecked((first >> Misc.UInt.BitSize) * (UInt)second);
            var yh = unchecked(lo + (yl << Misc.UInt.BitSize));
            lowResult = yh;
            return unchecked(((UInt)(yl >> Misc.UInt.BitSize)) + ((lo > yh ? (UInt)1 : 0)));
#endif
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static Long BigMulAsSigned(Long first, Long second, out Long highResult) {
            return unchecked((Long)BigMul(first, second, out highResult));
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            | System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
#endif
        )]
        public static ULong BigMul(ULong first_lo, ULong first_hi, ULong second_lo, ULong second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = first_hi;
                var sl = second_lo;
                var sh = second_hi;
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                result_hi_hi = hhh;
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            | System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
#endif
        )]

        public static ULong BigMul(ULong first_lo, Long first_hi, ULong second_lo, Long second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out Long result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = unchecked((ULong)first_hi);
                var sl = second_lo;
                var sh = unchecked((ULong)second_hi);
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                if (0 > unchecked((Long)fh)) {
                    hhl = SubtractUnchecked(hhl, hhh, sl, sh, out hhh);
                }
                if (0 > unchecked((Long)sh)) {
                    hhl = SubtractUnchecked(hhl, hhh, fl, fh, out hhh);
                }
                result_hi_hi = unchecked((Long)hhh);
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend %= divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            | System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
#endif
        )]
        public static ULong BigRemNoThrow(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong result_hi) {
            unchecked {
                if (0u == dividend_hi_lo && 0u == dividend_hi_hi) {
                    return Remainder(dividend_lo_lo, dividend_lo_hi, divisor_lo, divisor_hi, out result_hi);
                } else if (0u == divisor_hi && ULong.MaxValue > divisor_lo) {
                    result_hi = 0;
                    return Remainder(dividend_lo_lo, Remainder(dividend_lo_hi, dividend_hi_lo, divisor_lo), divisor_lo);
                } else {
                    // 2017Nov01
                    var dividend_lo_lo_ = dividend_lo_lo;
                    var dividend_lo_hi_ = dividend_lo_hi;
                    var dividend_hi_lo_ = dividend_hi_lo;
                    var dividend_hi_hi_ = dividend_hi_hi;
                    var divisor_lo_ = divisor_lo;
                    var divisor_hi_ = divisor_hi;
                    var c = 0;
                    if (0 <= (Long)divisor_hi_) {
                        do {
                            divisor_lo_ = ShiftLeft(divisor_lo_, divisor_hi_, out divisor_hi_);
                            ++c;
                        } while (0 <= (Long)divisor_hi_);
                        dividend_hi_hi_ = ShiftLeft(dividend_hi_lo_, dividend_hi_hi_, c);
                        dividend_hi_lo_ = ShiftLeft(dividend_lo_hi_, dividend_hi_lo_, c);
                        dividend_lo_lo_ = ShiftLeft(dividend_lo_lo_, dividend_lo_hi_, c, out dividend_lo_hi_);
                    }
                    ULong p_lo;
                    ULong p_hi;
                    if (dividend_hi_hi_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_hi_lo_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, 0, out dividend_hi_lo_, out var ignored0, out var q_hi), divisor_lo_, out p_hi);
                    if (1 == q_hi) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                        {
                            dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                                dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                            }
                        }
                    }
                    dividend_lo_hi_ = SubtractUnchecked(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi, out dividend_hi_lo_);
                    if (dividend_hi_lo_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_lo_hi_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, 0, out dividend_lo_hi_, out var ignored1, out var q_lo), divisor_lo_, out p_hi);
                    if (1 == q_lo) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                        {
                            dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                                dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                            }
                        }
                    }
                    dividend_lo_lo_ = SubtractUnchecked(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi, out dividend_lo_hi_);
                    return ShiftRightUnsigned(dividend_lo_lo_, dividend_lo_hi_, c, out result_hi);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigSquare(ULong value, out ULong highResult) {
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            highResult = global::System.Math.BigMul(value, value, out var lowResult);
            return lowResult;
#else
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    var fl = (UInt)value;
                    var fh = (UInt)(value >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * fl;
                    var lh = (ULong)fl * fh;
                    var hl = lh;
                    var hh = (ULong)fh * fh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04
                    var fl = (ULong)(UInt)value;
                    var fh = (ULong)(UInt)(value >> Misc.UInt.BitSize);
                    var ll = fl * fl;
                    var lh = fl * fh;
                    var hl = lh;
                    var hh = fh * fh;
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
#endif
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            | System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining
#endif
        )]
        public static ULong BigSquare(ULong value_lo, ULong value_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                {
                    // 2017Nov18
                    // 2020Dec10
                    //   Change BigMul to BigSquare
                    var fl = value_lo;
                    var fh = value_hi;
                    var lll = BigSquare(fl, out ULong llh); // 2020Dec10
                    var lhl = BigMul(fl, fh, out ULong lhh);
                    var hll = lhl;
                    var hlh = lhh;
                    var hhl = BigSquare(fh, out ULong hhh); // 2020Dec10
                    lhl = AddUnchecked(lhl, lhh, llh, 0, out lhh);
                    lhl = AddUnchecked(lhl, lhh, hll, hlh, out lhh);
                    if (LessThan(lhl, lhh, hll, hlh)) {
                        unchecked {
                            ++hhh;
                        }
                    }
                    result_hi_lo = AddUnchecked(hhl, hhh, lhh, 0, out result_hi_hi);
                    result_lo_hi = lhl;
                    return lll;
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivRemInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend <<= c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigRemInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep19
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend = highDividend % divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep13
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                // highDividend += divisor;
                            }
                        }
                    }
                    // remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            System.Diagnostics.Debug.Assert(0 != highDividend);
            System.Diagnostics.Debug.Assert(divisor > highDividend);
            unchecked {
                var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                var dl = (ULong)(UInt)divisor;
                var qh = Math.DivRem(highDividend, dh, out highDividend);
                var p = qh * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                if (highDividend < p) {
                    {
                        --qh;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --qh;
                            highDividend += divisor;
                        }
                    }
                }
                highDividend -= p;
                var ql = Math.DivRem(highDividend, dh, out highDividend);
                p = ql * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                if (highDividend < p) {
                    {
                        --ql;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --ql;
                            highDividend += divisor;
                        }
                    }
                }
                remainder = highDividend - p;
                return (qh << Misc.UInt.BitSize) | ql;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi, out ULong quotient_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi_hi);
            unchecked {
                var rl = dividend_hi_lo;
                var rh = dividend_hi_hi;
                var rc = dividend_lo_hi;
                var qh = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                rc = dividend_lo_lo;
                var ql = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                quotient_hi = qh;
                return ql;
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        static ULong DivRemPartialInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi);
            unchecked {
                var rl = dividend_lo;
                var rh = dividend_hi;
                var quotient = BigDivRemPartialInternal(rl, rh, divisor_hi, out rh);
                rl = dividend_cy;
                var pl = BigMul(quotient, divisor_hi, out var ph);
                if (GreaterThan(pl, ph, rl, rh)) {
                    {
                        --quotient;
                        rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                    }
                    if (LessThanOrEqual(divisor_lo, divisor_hi, rl, rh)) {
                        if (GreaterThan(pl, ph, rl, rh)) {
                            --quotient;
                            rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                        }
                    }
                }
                rl = SubtractUnchecked(rl, rh, pl, ph, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                return quotient;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigDivRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                ph += 1 + dividend_hi;
                var r = divisor * (ULong)(-(Long)ph);
                var mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += mask;
                r += mask & divisor;
                remainder = r;
                return ph;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                var _mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                r += _mask & (divisor);
                if (r >= divisor) {
                    goto L_C;
                }
            L_1:;
                remainder = r;
                return ph;
            L_C:;
                r -= divisor;
                ph++;
                goto L_1;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                var r = divisor * ~(ULong)(dividend_hi + ph);
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                if (r >= divisor) {
                    r -= divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, dividend_hi, out ph);

                var rh = dividend_lo - divisor_hi * ph;
                var rl = SubtractUnchecked(dividend_cy, rh, divisor_lo, divisor_hi, out rh);
                var tl = BigMul(divisor_lo, ph, out var th);
                rl = SubtractUnchecked(rl, rh, tl, th, out rh);
                ++ph;

                var _mask = rh >= pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                rl = AddUnchecked(rl, rh, _mask & divisor_lo, _mask & divisor_hi, out rh);
                if (rh >= divisor_hi) {
                    goto L_C;
                }
            L_1:;
                remainder_lo = rl;
                remainder_hi = rh;
                return ph;
            L_C:;
                if (rh > divisor_hi || rl >= divisor_lo) {
                    ++ph;
                    rl = SubtractUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                }
                goto L_1;
            }
        }
    }
}

#if NET7_0_OR_GREATER
namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

    using Math = global::Internal.System.Math;

    public static partial class DoubleArithmetic {

        /*
         * BigDivRem, BigDiv, BigRem:
         *   - The dividend has bigger size than the divisor's.
         *   - The dividend should be a fused-bigmul-add result.
         *   - The quotient can not overflow.
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * DivRem, Divide, Remainder:
         *   - (Unsigned cases) The quotient can not overflow. 
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * ~Unchecked:
         *   - Overflowed results are truncated.
         *   - Lower bits of results are correct on valid inputs.
         * ~Unsafe:
         *   - Used as a dependent routine.
         *   - No check on exceptionl conditions.
         *   - Undefined behavior on invalid inputs or exceptionl conditions.
         * ~Partial:
         *   - Used as a subroutine.
         *   - Works only on specialized conditions.
         *   - Undefined behavior on invalid inputs.
         * ~Internal:
         *   - Users should not use them directly.
         * ~NoThrowOnDivideByZero:
         *   - No throw on divide-by-zero conditions.
         * ~NoThrow:
         *   - No throw on exceptionl conditions.
         */
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRem(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                // 2020Jan01
                if (0u != highDividend) {
                    if (UInt.MaxValue <= divisor) {
                        // 2013Dec24, 2014Jan08
                        //if (divisor <= highDividend) {
                        //    highDividend = checked(0u - highDividend);
                        //    throw (System.OverflowException)null;
                        //}
                        {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        int c = 0;
                        if (0 <= (Long)divisor) {
                            do {
                                ++c;
                                divisor <<= 1;
                            } while (0 <= (Long)divisor);
                            highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                            lowDividend <<= c;
                        }
                        var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                        var dl = (ULong)(UInt)divisor;
                        qh = Math.DivRem(highDividend, dh, out highDividend);
                        p = qh * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        if (highDividend < p) {
                            {
                                --qh;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --qh;
                                    highDividend += divisor;
                                }
                            }
                        }
                        highDividend -= p;
                        ql = Math.DivRem(highDividend, dh, out highDividend);
                        p = ql * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        if (highDividend < p) {
                            {
                                --ql;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --ql;
                                    highDividend += divisor;
                                }
                            }
                        }
                        remainder = (highDividend - p) >> c;
                        return (qh << Misc.UInt.BitSize) | ql;
                    } else {
                        // 2014Jan08
                        if (0u != divisor) {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        qh = Math.DivRem(highDividend, divisor, out highDividend);
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        ql = Math.DivRem(highDividend, divisor, out remainder);
                        return (qh << Misc.UInt.BitSize) | ql;
                    }
                } else {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                            }
                        }
                    }
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(ULong first, ULong second, out ULong highResult) {
            unchecked {
                // 2019Dec15
                // Currently, we have fast multiplication instructions on most platforms.
                if (true || Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04, 2013Dec24
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    // if (lh < hl) {
                    //     hh += (ULong)1u << Misc.UInt.BitSize;
                    // }
                    // 2020Jan01
                    hh += (ULong)((lh < hl).AsIntegerUnsafe()) << Misc.UInt.BitSize;
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04, 2013Dec24
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(Long first, Long second, out Long highResult) {
            ULong r;
            var q = BigMul(unchecked((ULong)first), unchecked((ULong)second), out r);
            highResult = unchecked((Long)r - (-(Long)((ULong)first >> (Misc.ULong.BitSize - 1)) & second) - (-(Long)((ULong)second >> (Misc.ULong.BitSize - 1)) & first));
            return q;
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Karatsuba(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var hh = (ULong)fh * sh;
                    var fm = (UInt)fh + fl;
                    var sm = (UInt)sh + sl;
                    var mm = (ULong)(UInt)fm * (UInt)sm - (hh + ll);
                    var mh = (UInt)(mm >> Misc.UInt.BitSize) + (((UInt)fm < fl) ? (UInt)sm : 0u) + (((UInt)sm < sl) ? (UInt)fm : 0u);
                    var fs = ((ULong)fh + fl) + ((ULong)sh + sl);
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + ((ULong)mh + ((ll < ml) ? 1u : 0u)) + (((fs >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    // Bad for jitter:
                    // var mh = (ULong)(UInt)(mm >> Misc.UInt.BitSize);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    // Bad for jitter:
                    // highResult = hh + mh + ((ll < ml) ? 1u : 0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Long(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    // Bad for jitter:
                    // fl = (UInt)first;
                    var fl = (ULong)(UInt)first;
                    // Bad for jitter:
                    // var fh = (ULong)(first >> Misc.UInt.BitSize);
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // var ll = (ULong)(UInt)fl * (ULong)(UInt)sl;
                    var ll = fl * sl;
                    var lh = fl * sh;
                    var hl = fh * sl;
                    var hh = fh * sh;
                    // Bad for jitter:
                    // lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
            /* // old version
            var xh = first >> Misc.UInt.BitSize;
            var yh = second >> Misc.UInt.BitSize;
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var xl = unchecked((UInt)first * yh);
            var yl = unchecked(xh * (UInt)second);
            xl = unchecked(xl + yl);
            xh = unchecked(xh * yh);
            yh = unchecked(lo + (xl << Misc.UInt.BitSize));
            highResult = unchecked((xh + (UInt)(xl >> Misc.UInt.BitSize)) + ((lo > yh ? (ULong)1 : 0) + (yl > xl ? (ULong)1 << Misc.UInt.BitSize : 0)));
            return yh;
            */
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static UInt BigMul_M(ULong first, UInt second, out ULong lowResult) {
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var yl = unchecked((first >> Misc.UInt.BitSize) * (UInt)second);
            var yh = unchecked(lo + (yl << Misc.UInt.BitSize));
            lowResult = yh;
            return unchecked(((UInt)(yl >> Misc.UInt.BitSize)) + ((lo > yh ? (UInt)1 : 0)));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static Long BigMulAsSigned(Long first, Long second, out Long highResult) {
            return unchecked((Long)BigMul(first, second, out highResult));
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigMul(ULong first_lo, ULong first_hi, ULong second_lo, ULong second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = first_hi;
                var sl = second_lo;
                var sh = second_hi;
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                result_hi_hi = hhh;
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]

        public static ULong BigMul(ULong first_lo, Long first_hi, ULong second_lo, Long second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out Long result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = unchecked((ULong)first_hi);
                var sl = second_lo;
                var sh = unchecked((ULong)second_hi);
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                if (0 > unchecked((Long)fh)) {
                    hhl = SubtractUnchecked(hhl, hhh, sl, sh, out hhh);
                }
                if (0 > unchecked((Long)sh)) {
                    hhl = SubtractUnchecked(hhl, hhh, fl, fh, out hhh);
                }
                result_hi_hi = unchecked((Long)hhh);
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend %= divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigRemNoThrow(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong result_hi) {
            unchecked {
                if (0u == dividend_hi_lo && 0u == dividend_hi_hi) {
                    return Remainder(dividend_lo_lo, dividend_lo_hi, divisor_lo, divisor_hi, out result_hi);
                } else if (0u == divisor_hi && ULong.MaxValue > divisor_lo) {
                    result_hi = 0;
                    return Remainder(dividend_lo_lo, Remainder(dividend_lo_hi, dividend_hi_lo, divisor_lo), divisor_lo);
                } else {
                    // 2017Nov01
                    var dividend_lo_lo_ = dividend_lo_lo;
                    var dividend_lo_hi_ = dividend_lo_hi;
                    var dividend_hi_lo_ = dividend_hi_lo;
                    var dividend_hi_hi_ = dividend_hi_hi;
                    var divisor_lo_ = divisor_lo;
                    var divisor_hi_ = divisor_hi;
                    var c = 0;
                    if (0 <= (Long)divisor_hi_) {
                        do {
                            divisor_lo_ = ShiftLeft(divisor_lo_, divisor_hi_, out divisor_hi_);
                            ++c;
                        } while (0 <= (Long)divisor_hi_);
                        dividend_hi_hi_ = ShiftLeft(dividend_hi_lo_, dividend_hi_hi_, c);
                        dividend_hi_lo_ = ShiftLeft(dividend_lo_hi_, dividend_hi_lo_, c);
                        dividend_lo_lo_ = ShiftLeft(dividend_lo_lo_, dividend_lo_hi_, c, out dividend_lo_hi_);
                    }
                    ULong p_lo;
                    ULong p_hi;
                    if (dividend_hi_hi_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_hi_lo_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, 0, out dividend_hi_lo_, out var ignored0, out var q_hi), divisor_lo_, out p_hi);
                    if (1 == q_hi) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                        {
                            dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                                dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                            }
                        }
                    }
                    dividend_lo_hi_ = SubtractUnchecked(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi, out dividend_hi_lo_);
                    if (dividend_hi_lo_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_lo_hi_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, 0, out dividend_lo_hi_, out var ignored1, out var q_lo), divisor_lo_, out p_hi);
                    if (1 == q_lo) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                        {
                            dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                                dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                            }
                        }
                    }
                    dividend_lo_lo_ = SubtractUnchecked(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi, out dividend_lo_hi_);
                    return ShiftRightUnsigned(dividend_lo_lo_, dividend_lo_hi_, c, out result_hi);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigSquare(ULong value, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    var fl = (UInt)value;
                    var fh = (UInt)(value >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * fl;
                    var lh = (ULong)fl * fh;
                    var hl = lh;
                    var hh = (ULong)fh * fh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04
                    var fl = (ULong)(UInt)value;
                    var fh = (ULong)(UInt)(value >> Misc.UInt.BitSize);
                    var ll = fl * fl;
                    var lh = fl * fh;
                    var hl = lh;
                    var hh = fh * fh;
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigSquare(ULong value_lo, ULong value_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                {
                    // 2017Nov18
                    // 2020Dec10
                    //   Change BigMul to BigSquare
                    var fl = value_lo;
                    var fh = value_hi;
                    var lll = BigSquare(fl, out ULong llh); // 2020Dec10
                    var lhl = BigMul(fl, fh, out ULong lhh);
                    var hll = lhl;
                    var hlh = lhh;
                    var hhl = BigSquare(fh, out ULong hhh); // 2020Dec10
                    lhl = AddUnchecked(lhl, lhh, llh, 0, out lhh);
                    lhl = AddUnchecked(lhl, lhh, hll, hlh, out lhh);
                    if (LessThan(lhl, lhh, hll, hlh)) {
                        unchecked {
                            ++hhh;
                        }
                    }
                    result_hi_lo = AddUnchecked(hhl, hhh, lhh, 0, out result_hi_hi);
                    result_lo_hi = lhl;
                    return lll;
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivRemInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend <<= c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigRemInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep19
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend = highDividend % divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep13
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                // highDividend += divisor;
                            }
                        }
                    }
                    // remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            System.Diagnostics.Debug.Assert(0 != highDividend);
            System.Diagnostics.Debug.Assert(divisor > highDividend);
            unchecked {
                var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                var dl = (ULong)(UInt)divisor;
                var qh = Math.DivRem(highDividend, dh, out highDividend);
                var p = qh * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                if (highDividend < p) {
                    {
                        --qh;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --qh;
                            highDividend += divisor;
                        }
                    }
                }
                highDividend -= p;
                var ql = Math.DivRem(highDividend, dh, out highDividend);
                p = ql * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                if (highDividend < p) {
                    {
                        --ql;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --ql;
                            highDividend += divisor;
                        }
                    }
                }
                remainder = highDividend - p;
                return (qh << Misc.UInt.BitSize) | ql;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi, out ULong quotient_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi_hi);
            unchecked {
                var rl = dividend_hi_lo;
                var rh = dividend_hi_hi;
                var rc = dividend_lo_hi;
                var qh = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                rc = dividend_lo_lo;
                var ql = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                quotient_hi = qh;
                return ql;
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        static ULong DivRemPartialInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi);
            unchecked {
                var rl = dividend_lo;
                var rh = dividend_hi;
                var quotient = BigDivRemPartialInternal(rl, rh, divisor_hi, out rh);
                rl = dividend_cy;
                var pl = BigMul(quotient, divisor_hi, out var ph);
                if (GreaterThan(pl, ph, rl, rh)) {
                    {
                        --quotient;
                        rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                    }
                    if (LessThanOrEqual(divisor_lo, divisor_hi, rl, rh)) {
                        if (GreaterThan(pl, ph, rl, rh)) {
                            --quotient;
                            rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                        }
                    }
                }
                rl = SubtractUnchecked(rl, rh, pl, ph, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                return quotient;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigDivRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                ph += 1 + dividend_hi;
                var r = divisor * (ULong)(-(Long)ph);
                var mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += mask;
                r += mask & divisor;
                remainder = r;
                return ph;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                var _mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                r += _mask & (divisor);
                if (r >= divisor) {
                    goto L_C;
                }
            L_1:;
                remainder = r;
                return ph;
            L_C:;
                r -= divisor;
                ph++;
                goto L_1;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                var r = divisor * ~(ULong)(dividend_hi + ph);
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                if (r >= divisor) {
                    r -= divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, dividend_hi, out ph);

                var rh = dividend_lo - divisor_hi * ph;
                var rl = SubtractUnchecked(dividend_cy, rh, divisor_lo, divisor_hi, out rh);
                var tl = BigMul(divisor_lo, ph, out var th);
                rl = SubtractUnchecked(rl, rh, tl, th, out rh);
                ++ph;

                var _mask = rh >= pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                rl = AddUnchecked(rl, rh, _mask & divisor_lo, _mask & divisor_hi, out rh);
                if (rh >= divisor_hi) {
                    goto L_C;
                }
            L_1:;
                remainder_lo = rl;
                remainder_hi = rh;
                return ph;
            L_C:;
                if (rh > divisor_hi || rl >= divisor_lo) {
                    ++ph;
                    rl = SubtractUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                }
                goto L_1;
            }
        }
    }
}
#endif

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    public static partial class DoubleArithmetic {

        /*
         * BigDivRem, BigDiv, BigRem:
         *   - The dividend has bigger size than the divisor's.
         *   - The dividend should be a fused-bigmul-add result.
         *   - The quotient can not overflow.
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * DivRem, Divide, Remainder:
         *   - (Unsigned cases) The quotient can not overflow. 
         *   - Throws DivideByZeroException on 0 == divisor.
         *   - Throws OverflowException on quotient overflowed.
         * ~Unchecked:
         *   - Overflowed results are truncated.
         *   - Lower bits of results are correct on valid inputs.
         * ~Unsafe:
         *   - Used as a dependent routine.
         *   - No check on exceptionl conditions.
         *   - Undefined behavior on invalid inputs or exceptionl conditions.
         * ~Partial:
         *   - Used as a subroutine.
         *   - Works only on specialized conditions.
         *   - Undefined behavior on invalid inputs.
         * ~Internal:
         *   - Users should not use them directly.
         * ~NoThrowOnDivideByZero:
         *   - No throw on divide-by-zero conditions.
         * ~NoThrow:
         *   - No throw on exceptionl conditions.
         */
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRem(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                // 2020Jan01
                if (0u != highDividend) {
                    if (UInt.MaxValue <= divisor) {
                        // 2013Dec24, 2014Jan08
                        //if (divisor <= highDividend) {
                        //    highDividend = checked(0u - highDividend);
                        //    throw (System.OverflowException)null;
                        //}
                        {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        int c = 0;
                        if (0 <= (Long)divisor) {
                            do {
                                ++c;
                                divisor <<= 1;
                            } while (0 <= (Long)divisor);
                            highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                            lowDividend <<= c;
                        }
                        var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                        var dl = (ULong)(UInt)divisor;
                        qh = Math.DivRem(highDividend, dh, out highDividend);
                        p = qh * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        if (highDividend < p) {
                            {
                                --qh;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --qh;
                                    highDividend += divisor;
                                }
                            }
                        }
                        highDividend -= p;
                        ql = Math.DivRem(highDividend, dh, out highDividend);
                        p = ql * dl;
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        if (highDividend < p) {
                            {
                                --ql;
                                highDividend += divisor;
                            }
                            if (highDividend >= divisor) {
                                if (highDividend < p) {
                                    --ql;
                                    highDividend += divisor;
                                }
                            }
                        }
                        remainder = (highDividend - p) >> c;
                        return (qh << Misc.UInt.BitSize) | ql;
                    } else {
                        // 2014Jan08
                        if (0u != divisor) {
                            // 2020Jan01
                            _ = checked(divisor - highDividend);
                        }
                        highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                        qh = Math.DivRem(highDividend, divisor, out highDividend);
                        highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                        ql = Math.DivRem(highDividend, divisor, out remainder);
                        return (qh << Misc.UInt.BitSize) | ql;
                    }
                } else {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            ++c;
                            divisor <<= 1;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                            }
                        }
                    }
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(ULong first, ULong second, out ULong highResult) {
            unchecked {
                // 2019Dec15
                // Currently, we have fast multiplication instructions on most platforms.
                if (true || Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04, 2013Dec24
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    // if (lh < hl) {
                    //     hh += (ULong)1u << Misc.UInt.BitSize;
                    // }
                    // 2020Jan01
                    hh += (ULong)((lh < hl).AsIntegerUnsafe()) << Misc.UInt.BitSize;
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04, 2013Dec24
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigMul(Long first, Long second, out Long highResult) {
            ULong r;
            var q = BigMul(unchecked((ULong)first), unchecked((ULong)second), out r);
            highResult = unchecked((Long)r - (-(Long)((ULong)first >> (Misc.ULong.BitSize - 1)) & second) - (-(Long)((ULong)second >> (Misc.ULong.BitSize - 1)) & first));
            return q;
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Karatsuba(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var hh = (ULong)fh * sh;
                    var fm = (UInt)fh + fl;
                    var sm = (UInt)sh + sl;
                    var mm = (ULong)(UInt)fm * (UInt)sm - (hh + ll);
                    var mh = (UInt)(mm >> Misc.UInt.BitSize) + (((UInt)fm < fl) ? (UInt)sm : 0u) + (((UInt)sm < sl) ? (UInt)fm : 0u);
                    var fs = ((ULong)fh + fl) + ((ULong)sh + sl);
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    highResult = hh + ((ULong)mh + ((ll < ml) ? 1u : 0u)) + (((fs >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (ULong)(UInt)first;
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    var ll = fl * sl;
                    var hh = fh * sh;
                    var fm = fh + fl;
                    var sm = sh + sl;
                    var mm = fm * sm - (hh + ll);
                    // Bad for jitter:
                    // var mh = (ULong)(UInt)(mm >> Misc.UInt.BitSize);
                    var mh = mm >> Misc.UInt.BitSize;
                    var ml = mm << Misc.UInt.BitSize;
                    ll += ml;
                    // Bad for jitter:
                    // highResult = hh + mh + ((ll < ml) ? 1u : 0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    highResult = hh + mh + ((ll < ml) ? (ULong)1u : (ULong)0u) + ((((fm + sm) >> 1) - mh) & ((ULong)UInt.MaxValue << Misc.UInt.BitSize));
                    return ll;
                }
            }
        }

        [ObsoleteAttribute("Use BigMul instead.")]
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        // primary overload
        public static ULong BigMul_A_Long(ULong first, ULong second, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct03
                    // 我很滿意。
                    var fl = (UInt)first;
                    var fh = (UInt)(first >> Misc.UInt.BitSize);
                    var sl = (UInt)second;
                    var sh = (UInt)(second >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * sl;
                    var lh = (ULong)fl * sh;
                    var hl = (ULong)fh * sl;
                    var hh = (ULong)fh * sh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct03
                    // 我很滿意。
                    // Bad for jitter:
                    // fl = (UInt)first;
                    var fl = (ULong)(UInt)first;
                    // Bad for jitter:
                    // var fh = (ULong)(first >> Misc.UInt.BitSize);
                    var fh = (ULong)(UInt)(first >> Misc.UInt.BitSize);
                    var sl = (ULong)(UInt)second;
                    var sh = (ULong)(UInt)(second >> Misc.UInt.BitSize);
                    // Bad for jitter:
                    // var ll = (ULong)(UInt)fl * (ULong)(UInt)sl;
                    var ll = fl * sl;
                    var lh = fl * sh;
                    var hl = fh * sl;
                    var hh = fh * sh;
                    // Bad for jitter:
                    // lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
            /* // old version
            var xh = first >> Misc.UInt.BitSize;
            var yh = second >> Misc.UInt.BitSize;
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var xl = unchecked((UInt)first * yh);
            var yl = unchecked(xh * (UInt)second);
            xl = unchecked(xl + yl);
            xh = unchecked(xh * yh);
            yh = unchecked(lo + (xl << Misc.UInt.BitSize));
            highResult = unchecked((xh + (UInt)(xl >> Misc.UInt.BitSize)) + ((lo > yh ? (ULong)1 : 0) + (yl > xl ? (ULong)1 << Misc.UInt.BitSize : 0)));
            return yh;
            */
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static UInt BigMul_M(ULong first, UInt second, out ULong lowResult) {
            var lo = unchecked((ULong)(UInt)first * (UInt)second);
            var yl = unchecked((first >> Misc.UInt.BitSize) * (UInt)second);
            var yh = unchecked(lo + (yl << Misc.UInt.BitSize));
            lowResult = yh;
            return unchecked(((UInt)(yl >> Misc.UInt.BitSize)) + ((lo > yh ? (UInt)1 : 0)));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
                    System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static Long BigMulAsSigned(Long first, Long second, out Long highResult) {
            return unchecked((Long)BigMul(first, second, out highResult));
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigMul(ULong first_lo, ULong first_hi, ULong second_lo, ULong second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = first_hi;
                var sl = second_lo;
                var sh = second_hi;
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                result_hi_hi = hhh;
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]

        public static ULong BigMul(ULong first_lo, Long first_hi, ULong second_lo, Long second_hi, out ULong result_lo_hi, out ULong result_hi_lo, out Long result_hi_hi) {
            unchecked {
                var fl = first_lo;
                var fh = unchecked((ULong)first_hi);
                var sl = second_lo;
                var sh = unchecked((ULong)second_hi);
                var lll = BigMul(fl, sl, out ULong llh);
                var hhl = BigMul(fh, sh, out ULong hhh);
                var fm = unchecked(fh + fl);
                var sm = unchecked(sh + sl);
                var tl = AddUnchecked(hhl, hhh, lll, llh, out ULong th);
                var mml = BigMul(fm, sm, out ULong mmh);
                var dh = (ULong)0;
                if (fm < fl) {
                    unchecked {
                        ++dh;
                        mmh += sm;
                    }
                }
                if (sm < sl) {
                    unchecked {
                        ++dh;
                        mmh += fm;
                    }
                }
                mml = SubtractUnchecked(mml, mmh, tl, th, out mmh);
                var dl = unchecked(fm + sm);
                if (dl < fm) {
                    unchecked {
                        ++dh;
                    }
                }
                llh = unchecked(llh + mml);
                if (llh < mml) {
                    hhl = IncreaseUnchecked(hhl, hhh, out hhh);
                }
                hhl = AddUnchecked(hhl, hhh, mmh, 0, out hhh);
                dl = ShiftRightUnsigned(dl, dh, out dh);
                dl = SubtractUnchecked(dl, dh, mmh, 0, out dh);
                hhh = unchecked(hhh + dh);
                if (0 > unchecked((Long)fh)) {
                    hhl = SubtractUnchecked(hhl, hhh, sl, sh, out hhh);
                }
                if (0 > unchecked((Long)sh)) {
                    hhl = SubtractUnchecked(hhl, hhh, fl, fh, out hhh);
                }
                result_hi_hi = unchecked((Long)hhh);
                result_hi_lo = hhl;
                result_lo_hi = llh;
                return lll;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemNoThrow(ULong lowDividend, ULong highDividend, ULong divisor) {
            unchecked {
                ULong p;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend %= divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    p = Math.DivRem(highDividend, dh, out highDividend) * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigRemNoThrow(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong result_hi) {
            unchecked {
                if (0u == dividend_hi_lo && 0u == dividend_hi_hi) {
                    return Remainder(dividend_lo_lo, dividend_lo_hi, divisor_lo, divisor_hi, out result_hi);
                } else if (0u == divisor_hi && ULong.MaxValue > divisor_lo) {
                    result_hi = 0;
                    return Remainder(dividend_lo_lo, Remainder(dividend_lo_hi, dividend_hi_lo, divisor_lo), divisor_lo);
                } else {
                    // 2017Nov01
                    var dividend_lo_lo_ = dividend_lo_lo;
                    var dividend_lo_hi_ = dividend_lo_hi;
                    var dividend_hi_lo_ = dividend_hi_lo;
                    var dividend_hi_hi_ = dividend_hi_hi;
                    var divisor_lo_ = divisor_lo;
                    var divisor_hi_ = divisor_hi;
                    var c = 0;
                    if (0 <= (Long)divisor_hi_) {
                        do {
                            divisor_lo_ = ShiftLeft(divisor_lo_, divisor_hi_, out divisor_hi_);
                            ++c;
                        } while (0 <= (Long)divisor_hi_);
                        dividend_hi_hi_ = ShiftLeft(dividend_hi_lo_, dividend_hi_hi_, c);
                        dividend_hi_lo_ = ShiftLeft(dividend_lo_hi_, dividend_hi_lo_, c);
                        dividend_lo_lo_ = ShiftLeft(dividend_lo_lo_, dividend_lo_hi_, c, out dividend_lo_hi_);
                    }
                    ULong p_lo;
                    ULong p_hi;
                    if (dividend_hi_hi_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_hi_lo_, divisor_hi_, out dividend_hi_lo_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_hi_lo_, dividend_hi_hi_, divisor_hi_, 0, out dividend_hi_lo_, out var ignored0, out var q_hi), divisor_lo_, out p_hi);
                    if (1 == q_hi) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                        {
                            dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi)) {
                                dividend_lo_hi_ = AddUnchecked(dividend_lo_hi_, dividend_hi_lo_, divisor_lo_, divisor_hi_, out dividend_hi_lo_);
                            }
                        }
                    }
                    dividend_lo_hi_ = SubtractUnchecked(dividend_lo_hi_, dividend_hi_lo_, p_lo, p_hi, out dividend_hi_lo_);
                    if (dividend_hi_lo_ < divisor_hi_) {
                        p_lo = BigMul(BigDivRemPartialInternal(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                    } else {
                        p_lo = BigMul(Math.DivRem(dividend_lo_hi_, divisor_hi_, out dividend_lo_hi_), divisor_lo_, out p_hi);
                        p_hi += divisor_lo_;
                    }
                    /*
                    p_lo = BigMul(DivRem(dividend_lo_hi_, dividend_hi_lo_, divisor_hi_, 0, out dividend_lo_hi_, out var ignored1, out var q_lo), divisor_lo_, out p_hi);
                    if (1 == q_lo) {
                        p_hi += divisor_lo_;
                    }
                    */
                    if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                        {
                            dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                        }
                        if (GreaterThanOrEqual(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_)) {
                            if (LessThan(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi)) {
                                dividend_lo_lo_ = AddUnchecked(dividend_lo_lo_, dividend_lo_hi_, divisor_lo_, divisor_hi_, out dividend_lo_hi_);
                            }
                        }
                    }
                    dividend_lo_lo_ = SubtractUnchecked(dividend_lo_lo_, dividend_lo_hi_, p_lo, p_hi, out dividend_lo_hi_);
                    return ShiftRightUnsigned(dividend_lo_lo_, dividend_lo_hi_, c, out result_hi);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigSquare(ULong value, out ULong highResult) {
            unchecked {
                if (Misc.ULong.Size > Misc.UIntPtr.Size) {
                    // 2013Oct04
                    var fl = (UInt)value;
                    var fh = (UInt)(value >> Misc.UInt.BitSize);
                    var ll = (ULong)fl * fl;
                    var lh = (ULong)fl * fh;
                    var hl = lh;
                    var hh = (ULong)fh * fh;
                    lh += (UInt)(ll >> Misc.UInt.BitSize);
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (UInt)(lh >> Misc.UInt.BitSize);
                    return ((ULong)(UInt)lh << Misc.UInt.BitSize) | (UInt)(ll);
                }
                {
                    // 2013Oct04
                    var fl = (ULong)(UInt)value;
                    var fh = (ULong)(UInt)(value >> Misc.UInt.BitSize);
                    var ll = fl * fl;
                    var lh = fl * fh;
                    var hl = lh;
                    var hh = fh * fh;
                    lh += ll >> Misc.UInt.BitSize;
                    lh += hl;
                    if (lh < hl) {
                        hh += (ULong)1u << Misc.UInt.BitSize;
                    }
                    highResult = hh + (lh >> Misc.UInt.BitSize);
                    return (lh << Misc.UInt.BitSize) + (UInt)(ll);
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization
        )]
        public static ULong BigSquare(ULong value_lo, ULong value_hi, out ULong result_lo_hi, out ULong result_hi_lo, out ULong result_hi_hi) {
            unchecked {
                {
                    // 2017Nov18
                    // 2020Dec10
                    //   Change BigMul to BigSquare
                    var fl = value_lo;
                    var fh = value_hi;
                    var lll = BigSquare(fl, out ULong llh); // 2020Dec10
                    var lhl = BigMul(fl, fh, out ULong lhh);
                    var hll = lhl;
                    var hlh = lhh;
                    var hhl = BigSquare(fh, out ULong hhh); // 2020Dec10
                    lhl = AddUnchecked(lhl, lhh, llh, 0, out lhh);
                    lhl = AddUnchecked(lhl, lhh, hll, hlh, out lhh);
                    if (LessThan(lhl, lhh, hll, hlh)) {
                        unchecked {
                            ++hhh;
                        }
                    }
                    result_hi_lo = AddUnchecked(hhl, hhh, lhh, 0, out result_hi_hi);
                    result_lo_hi = lhl;
                    return lll;
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivRemInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return Math.DivRem(lowDividend, divisor, out remainder);
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = Math.DivRem(highDividend, divisor, out remainder);
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend <<= c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                highDividend += divisor;
                            }
                        }
                    }
                    remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigRemInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep19
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend % divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    highDividend = highDividend % divisor;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    return highDividend % divisor;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                highDividend += divisor;
                            }
                        }
                    }
                    return (highDividend - p) >> c;
                }
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        internal static ULong BigDivInternal(ULong lowDividend, ULong highDividend, ULong divisor) {
            // 2014Sep13
            unchecked {
                ULong p, ql, qh;
                if (0u == highDividend) {
                    return lowDividend / divisor;
                } else if (UInt.MaxValue > divisor) {
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    qh = Math.DivRem(highDividend, divisor, out highDividend);
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    ql = highDividend / divisor;
                    return (qh << Misc.UInt.BitSize) | ql;
                } else {
                    // 2013Dec24
                    int c = 0;
                    if (0 <= (Long)divisor) {
                        do {
                            divisor <<= 1;
                            ++c;
                        } while (0 <= (Long)divisor);
                        highDividend = (highDividend << c) | (lowDividend >> (Misc.ULong.BitSize - c));
                        lowDividend = lowDividend << c;
                    }
                    var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                    var dl = (ULong)(UInt)divisor;
                    qh = Math.DivRem(highDividend, dh, out highDividend);
                    p = qh * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                    if (highDividend < p) {
                        {
                            --qh;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --qh;
                                highDividend += divisor;
                            }
                        }
                    }
                    highDividend -= p;
                    ql = Math.DivRem(highDividend, dh, out highDividend);
                    p = ql * dl;
                    highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                    if (highDividend < p) {
                        {
                            --ql;
                            highDividend += divisor;
                        }
                        if (highDividend >= divisor) {
                            if (highDividend < p) {
                                --ql;
                                // highDividend += divisor;
                            }
                        }
                    }
                    // remainder = (highDividend - p) >> c;
                    return (qh << Misc.UInt.BitSize) | ql;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong lowDividend, ULong highDividend, ULong divisor, out ULong remainder) {
            System.Diagnostics.Debug.Assert(0 != highDividend);
            System.Diagnostics.Debug.Assert(divisor > highDividend);
            unchecked {
                var dh = (ULong)(UInt)(divisor >> Misc.UInt.BitSize);
                var dl = (ULong)(UInt)divisor;
                var qh = Math.DivRem(highDividend, dh, out highDividend);
                var p = qh * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (lowDividend >> Misc.UInt.BitSize);
                if (highDividend < p) {
                    {
                        --qh;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --qh;
                            highDividend += divisor;
                        }
                    }
                }
                highDividend -= p;
                var ql = Math.DivRem(highDividend, dh, out highDividend);
                p = ql * dl;
                highDividend = (highDividend << Misc.UInt.BitSize) | (UInt)lowDividend;
                if (highDividend < p) {
                    {
                        --ql;
                        highDividend += divisor;
                    }
                    if (highDividend >= divisor) {
                        if (highDividend < p) {
                            --ql;
                            highDividend += divisor;
                        }
                    }
                }
                remainder = highDividend - p;
                return (qh << Misc.UInt.BitSize) | ql;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemPartialInternal(ULong dividend_lo_lo, ULong dividend_lo_hi, ULong dividend_hi_lo, ULong dividend_hi_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi, out ULong quotient_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi_hi);
            unchecked {
                var rl = dividend_hi_lo;
                var rh = dividend_hi_hi;
                var rc = dividend_lo_hi;
                var qh = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                rc = dividend_lo_lo;
                var ql = DivRemPartialInternal(rc, rl, rh, divisor_lo, divisor_hi, out rl, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                quotient_hi = qh;
                return ql;
            }
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        static ULong DivRemPartialInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi);
            System.Diagnostics.Debug.Assert(divisor_hi > dividend_hi);
            unchecked {
                var rl = dividend_lo;
                var rh = dividend_hi;
                var quotient = BigDivRemPartialInternal(rl, rh, divisor_hi, out rh);
                rl = dividend_cy;
                var pl = BigMul(quotient, divisor_hi, out var ph);
                if (GreaterThan(pl, ph, rl, rh)) {
                    {
                        --quotient;
                        rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                    }
                    if (LessThanOrEqual(divisor_lo, divisor_hi, rl, rh)) {
                        if (GreaterThan(pl, ph, rl, rh)) {
                            --quotient;
                            rl = AddUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                        }
                    }
                }
                rl = SubtractUnchecked(rl, rh, pl, ph, out rh);
                remainder_lo = rl;
                remainder_hi = rh;
                return quotient;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigDivRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                ph += 1 + dividend_hi;
                var r = divisor * (ULong)(-(Long)ph);
                var mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += mask;
                r += mask & divisor;
                remainder = r;
                return ph;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor, out ULong remainder) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                var _mask = r >/*=*/ pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                r += _mask & (divisor);
                if (r >= divisor) {
                    goto L_C;
                }
            L_1:;
                remainder = r;
                return ph;
            L_C:;
                r -= divisor;
                ph++;
                goto L_1;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
#if STANDALONE_XINTN_LIBRARY
        internal
#else
        public
#endif
            static ULong BigRemByInverseInternal(Void _, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                var r = divisor * ~(ULong)(dividend_hi + ph);
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigRemByInverseInternal(ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, 1 + dividend_hi, out ph);
                var r = dividend_lo - divisor * ph;
                if (r >/*=*/ pl) {
                    r += divisor;
                }
                if (r >= divisor) {
                    r -= divisor;
                }
                return r;
            }
        }

        [CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining |
            System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        public static ULong BigDivRemByInverseInternal(ULong dividend_cy, ULong dividend_lo, ULong dividend_hi, ULong divisorReciprocal, ULong divisor_lo, ULong divisor_hi, out ULong remainder_lo, out ULong remainder_hi) {
            unchecked {
                var pl = BigMul(divisorReciprocal, dividend_hi, out var ph);
                pl = AddUnchecked(pl, ph, dividend_lo, dividend_hi, out ph);

                var rh = dividend_lo - divisor_hi * ph;
                var rl = SubtractUnchecked(dividend_cy, rh, divisor_lo, divisor_hi, out rh);
                var tl = BigMul(divisor_lo, ph, out var th);
                rl = SubtractUnchecked(rl, rh, tl, th, out rh);
                ++ph;

                var _mask = rh >= pl ? (ULong)(-(Long)(ULong)1) : 0;
                ph += _mask;
                rl = AddUnchecked(rl, rh, _mask & divisor_lo, _mask & divisor_hi, out rh);
                if (rh >= divisor_hi) {
                    goto L_C;
                }
            L_1:;
                remainder_lo = rl;
                remainder_hi = rh;
                return ph;
            L_C:;
                if (rh > divisor_hi || rl >= divisor_lo) {
                    ++ph;
                    rl = SubtractUnchecked(rl, rh, divisor_lo, divisor_hi, out rh);
                }
                goto L_1;
            }
        }
    }
}
