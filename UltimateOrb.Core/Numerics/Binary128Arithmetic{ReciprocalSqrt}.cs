using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Misc = UltimateOrb.Miscellaneous;

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    partial class Binary128Arithmetic {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int64 MultiplyHigh(UInt64 y, Int64 x) {
            return (Int64)((Math.BigMul((UInt64)x, y) >> 64) - ((UInt64)(x >> 63) & y));
        }

        public static UInt64 ReciprocalSqrt(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => ReciprocalSqrt(lo, hi, MidpointRounding.ToEven, out result_hi);

        public static UInt64 ReciprocalSqrt(UInt64 lo, UInt64 hi, MidpointRounding rm, out UInt64 result_hi) {
            unchecked {
                nint e = (nint)(hi >> 48); // exponent
                if (Misc.Unlikely(e == 0)) { // x is subnormal or x=+0
                    nint ns = -15;
                    if (0 != hi) {
                        ns += (nint)UInt64.LeadingZeroCount(hi);
                    } else {
                        if (0 != lo) {
                            ns += (nint)UInt64.LeadingZeroCount(lo) + 64;
                        } else {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.DivideByZero);
                            return GetPositiveInfinity(out result_hi); // x = +0
                        }
                    }
                    e = 1 - ns;
                    // TODO: nint shiftAmount
                    lo = DoubleArithmetic.ShiftLeft(lo, hi, (int)ns, out hi); // normalize mantissa
                    hi ^= (UInt64)(ns & 1) << 48; // set proper last bit of exponent
                }
                if (Misc.Unlikely(e >= 0x7fff)) { // other special cases: NAN, inf, negative numbers
                    var a = (lo | ((UInt128)hi << 64));
                    if (0u == (a << 1)) { // x = -0
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.DivideByZero);
                        return GetNegativeInfinity(out result_hi);
                    }
                    if (a == (UInt128)0x7fff << 112) {
                        // x = +Inf
                        result_hi = 0;
                        return 0;
                    }
                    if (a <= (UInt128)0xffff << 112 && a > (UInt128)0x8000 << 112) {
                        // -inf <= x < -0
                        // x < 0
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        return GetNaN(out result_hi);
                    } else {
                        return GetNaN(lo, hi, out result_hi); // snan -> qnan
                    }
                }
                {
                    e += 121; // bias in case of denormal number
                    nuint q2 = (nuint)(e / 2), i = (nuint)(e & 1);
                    Int64 e2 = (Int64)(((UInt64)0X5ffd - q2 + 60) << 48);
                    lo = DoubleArithmetic.ShiftLeft(lo, hi, 16, out hi);
                    if (Misc.Unlikely(0 == (lo | hi))) { // no inexact exception
                        if (0 != (~e & 1)) {
                            hi = (UInt64)e2 + ((UInt64)2 << 48); // place exponent
                            result_hi = hi;
                            return lo;
                        }
                    }

                    UInt64 rx = hi, r = Rsqrt9(rx);

                    UInt128 r2 = Math.BigMul(r, rsqrt2_64[(int)i]);
                    nuint shft = 4 - i;
                    r2 >>= (int)shft;
                    hi |= (UInt64)1u << (int)(60 + i);
                    r = (UInt64)(r2 >> 64);
                    UInt128 R2 = Math.BigMul(r, r);

                    var a = (lo | ((UInt128)hi << 64));

                    Int64 h = (Int64)MultiplyHighApproximate(R2, a), ds = MultiplyHigh(r, h);

                    UInt128 v = ((UInt128)r << 64) - ((UInt128)(Int128)ds << 3);
                    //UInt64 v_lo = (UInt64)v, v_hi = (UInt64)(v >> 64);
                    bool nrst = IsNearest(rm);
                    Int16 dd = (Int16)((UInt64)v << 2);
                    if (Misc.Unlikely(!(dd < -4 || dd > 96))) { // can round correctly?
                        v += 1 << 13;
                        UInt128 m = v >> 14, t0, t1, k0, k1;
                        t1 = DoubleArithmetic.BigMul(m, a, out t0);
                        k1 = DoubleArithmetic.BigMul(t0, m, out k0);
                        k1 += t1 * m;
                        k1 |= 0 != k0 ? 1u : 0u;
                        v &= ~(UInt128)0x3fff;
                        Int128 D = (Int128)k1;
                        if (D < 0) v++;
                        if (D > 0) v--;
                    }
                    nuint frac = (nuint)v & 0x7fffu; // fractional part
                    UInt64 rnd = 0;
                    if (Misc.Likely(nrst)) {
                        rnd = frac >> 14;  // round to nearest tie to even
                    } else if (rm == MidpointRounding.ToPositiveInfinity) {
                        rnd = 1; // round up
                    } else {
                        // round down and to zero
                    }
                    v >>= 15; // position mantissa
                    v += rnd; // round
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                    v += (UInt64)(Int64)e2 << 64; // place exponent
                    result_hi = (UInt64)(v >> 64);
                    return (UInt64)v;
                }
            }
        }
    }
}
