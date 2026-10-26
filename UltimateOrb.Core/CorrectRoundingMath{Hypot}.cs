using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UltimateOrb.Numerics;
using Misc = UltimateOrb.Miscellaneous;

namespace UltimateOrb {

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    using static Numerics.Binary128Arithmetic;
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    
    public static partial class CorrectRoundingMath {

        public static double Hypot(double x, double y) {
            return CorrectRoundingMathInternal.Hypot(x, y);
        }

        public static double Hypot(double x, double y, [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
            return CorrectRoundingMathInternal.Hypot(x, y, mode);
        }
    }

    static partial class CorrectRoundingMathInternal {

        // ════════════════════════════════════════════════════════════════════
        //  as_hypot_denorm — both |x| and |y| subnormal.
        //  a, b are the raw b64 bit patterns of |x| and |y| with a >= b > 0.
        // ════════════════════════════════════════════════════════════════════
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static double AsHypotDenorm(UInt64 a, UInt64 b, [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
            unchecked {
                double af = (Int64)a, bf = (Int64)b;
                a <<= 1;
                b <<= 1;
                UInt64 rm = (UInt64)Math.Sqrt(af * af + bf * bf);
                Int64 tm = (Int64)rm << 1;
                Int64 D = (Int64)(a * a + b * b - (UInt64)tm * (UInt64)tm);

                while (D > 2 * tm) {
                    D -= 2 * tm + 1;
                    tm++;
                }
                while (D < 0) {
                    D += 2 * tm - 1;
                    tm--;
                }

                // D == 0: exact case, no exception flags raised.

                int rb = (int)(tm & 1);
                int rb2 = D >= tm ? 1 : 0;
                int sb = D != 0 ? 1 : 0;

                rm = (UInt64)(tm >> 1);
                bool underflow = rm < 0x10000000000000UL;

                if (sb != 0) {
                    if (mode == MidpointRounding.ToEven || mode == MidpointRounding.AwayFromZero) {
                        // Nearest rounding.  rb == 1 ⟺ true value ≥ rm+0.5 (past halfway);
                        // add it to round up.  (For AwayFromZero a tie also rounds up —
                        // consistent with +rb, so no extra branch is needed for positive
                        // results.)
                        rm += (UInt64)rb;
                        if ((rm >> 52) != 0 && rb2 != 0) underflow = false;
                    } else if (mode == MidpointRounding.ToPositiveInfinity) {
                        rm++;
                        if ((rm >> 52) != 0 && (tm & 1) != 0) underflow = false;
                    }
                    if (underflow)
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow | FloatingPointExceptionFlags.Inexact);
                    else
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                }
                return BitConverter.UInt64BitsToDouble(rm);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  as_hypot_hard — accurate fallback.
        //  x, y are already scaled to x ∈ [1,2), y ∈ (0,1].
        // ════════════════════════════════════════════════════════════════════
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static double AsHypotHard(double x, double y, [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
            unchecked {
                UInt64 xi = BitConverter.DoubleToUInt64Bits(x);
                UInt64 yi = BitConverter.DoubleToUInt64Bits(y);

                UInt64 bm = (xi & (~(UInt64)0 >> 12)) | (1UL << 52);
                UInt64 lm = (yi & (~(UInt64)0 >> 12)) | (1UL << 52);
                int be = (int)(xi >> 52);
                int le = (int)(yi >> 52);

                UInt64 ri = BitConverter.DoubleToUInt64Bits(Math.Sqrt(x * x + y * y));
                const int bs = 2;
                UInt64 rm = ri & (~(UInt64)0 >> 12);
                int re = (int)(ri >> 52) - 0x3FF;
                rm |= 1UL << 52;

                for (int i = 0; i < 3; i++) {
                    if (rm == (1UL << 52)) {
                        rm = ~(UInt64)0 >> 11;
                        re--;
                    } else {
                        rm--;
                    }
                }

                bm <<= bs;
                UInt64 m2 = bm * bm;

                int de = be - le;
                int ls = bs - de;
                if (ls >= 0) {
                    lm <<= ls;
                    m2 += lm * lm;
                } else {
                    UInt128 lm2 = (UInt128)lm * lm;
                    ls *= 2;
                    m2 += (UInt64)(lm2 >> (-ls));
                    m2 |= (lm2 << (128 + ls)) != 0 ? 1UL : 0UL;
                }

                int k = bs + re;
                Int64 D;
                do {
                    rm += (UInt64)(1 + (rm >= (1UL << 53) ? 1 : 0));
                    UInt64 tm = rm << k, rm2 = tm * tm;
                    D = (Int64)(m2 - rm2);
                } while (D > 0);

                if (D == 0) {
                    // Exact — no exception flags (matches C: set_flags(flag) restores).
                } else {
                    if (mode == MidpointRounding.ToEven) {
                        UInt64 tm = (rm << k) - (1UL << (k - (rm <= (1UL << 53) ? 1 : 0)));
                        D = (Int64)(m2 - tm * tm);
                        if (D != 0)
                            rm += (UInt64)(D >> 63);
                        else
                            rm -= rm & (1UL << (1 - (rm <= (1UL << 53) ? 1 : 0)));   // tie → even
                    } else if (mode == MidpointRounding.AwayFromZero) {
                        // Nearest, but ties go to the away-from-zero side — which for the
                        // (non-negative) hypot result means keeping the ceiling `rm`.
                        UInt64 tm = (rm << k) - (1UL << (k - (rm <= (1UL << 53) ? 1 : 0)));
                        D = (Int64)(m2 - tm * tm);
                        if (D != 0)
                            rm += (UInt64)(D >> 63);                                 // >0: keep, <0: rm-1
                                                                                     // D == 0 (tie) → keep rm
                    } else {
                        bool down = mode == MidpointRounding.ToNegativeInfinity
                                 || mode == MidpointRounding.ToZero;
                        if (down)
                            rm -= (UInt64)(1 + (rm > (1UL << 53) ? 1 : 0));
                    }
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                }

                if (rm >= (1UL << 53)) {
                    rm >>= 1;
                    re++;
                }

                Int64 e = be - 1 + re;
                UInt64 result = (UInt64)(e << 52) + rm;
                return BitConverter.UInt64BitsToDouble(result);
            }
        }

        // ════════════════════════════════════════════════════════════════════
        //  as_hypot_overflow — result overflows to +Inf.
        // ════════════════════════════════════════════════════════════════════
        static double AsHypotOverflow([ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Overflow | FloatingPointExceptionFlags.Inexact);
            return (mode == MidpointRounding.ToZero || mode == MidpointRounding.ToNegativeInfinity)
                ? double.MaxValue
                : double.PositiveInfinity;
        }


        // ════════════════════════════════════════════════════════════════════
        //  cr_hypot — main entry point.
        // ════════════════════════════════════════════════════════════════════
        public static double Hypot(double x, double y) => Hypot(x, y, MidpointRounding.ToEven);

        public static double Hypot(double x, double y, [ConstantExpected(Max = MidpointRounding.ToPositiveInfinity)] MidpointRounding mode) {
            unchecked {
                UInt64 xi = BitConverter.DoubleToUInt64Bits(x);
                UInt64 yi = BitConverter.DoubleToUInt64Bits(y);
                const UInt64 emsk = 0x7FFUL << 52;
                UInt64 ex = xi & emsk, ey = yi & emsk;

                x = double.Abs(x);
                y = double.Abs(y);

                // ── NaN / Inf fast handling ──
                if (Misc.Unlikely(ex == emsk || ey == emsk)) {
                    UInt64 wx = xi << 1, wy = yi << 1, wm = emsk << 1;
                    bool ninf = (wx == wm) ^ (wy == wm);
                    bool nqnn = ((wx >> 52) == 0xFFF) ^ ((wy >> 52) == 0xFFF);
                    // IEEE: hypot(+/-Inf, qNaN) = hypot(qNaN, +/-Inf) = +Inf
                    if (ninf && nqnn) return (wx == wm) ? x * x : y * y;
                    return x + y;
                }

                // Now both are finite; u >= v >= 0
                double u = x > y ? x : y, v = x > y ? y : x;
                UInt64 xd = BitConverter.DoubleToUInt64Bits(u);
                UInt64 yd = BitConverter.DoubleToUInt64Bits(v);

                // ── subnormal handling ──
                ey = yd;
                if (Misc.Unlikely((ey >> 52) == 0)) {
                    if (ey == 0) return BitConverter.UInt64BitsToDouble(xd); // y == 0

                    ex = xd;
                    if (Misc.Unlikely((ex >> 52) == 0))
                        return AsHypotDenorm(ex, ey, mode);

                    // x normal, y subnormal — normalize y
                    int nz = (int)UInt64.LeadingZeroCount(ey);
                    ey <<= nz - 11;
                    ey &= ~(UInt64)0 >> 12;
                    ey -= (UInt64)((Int64)nz - 12) << 52;
                    yd = ey;
                }

                // ── very disparate magnitudes → just u ──
                // ── very disparate magnitudes → u, except under round-toward-+∞ ──
                UInt64 de = xd - yd;
                if (Misc.Unlikely(de > (27UL << 52))) {
                    // v is >2^27 orders of magnitude below u; the correction
                    // δ = √(u²+v²) − u satisfies 0 < δ < 0.5·ULP(u), so the exact
                    // result lies strictly inside (u, nextUp(u)) — never a tie.
                    //   ToEven / AwayFromZero / ToZero / ToNegativeInfinity  →  u
                    //   ToPositiveInfinity                                   →  nextUp(u)
                    // nextUp(u) overflows to +∞ iff u == MaxValue.
                    if (mode == MidpointRounding.ToPositiveInfinity) {
                        UInt64 ru = xd + 1;                         // nextUp(u)
                        if (Misc.Unlikely(ru >= 0x7FF0000000000000UL)) {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Overflow
                                                   | FloatingPointExceptionFlags.Inexact);
                            return double.PositiveInfinity;
                        }
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                        return BitConverter.UInt64BitsToDouble(ru);
                    }
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                    return u;
                }

                // ── scale so that x ∈ [1, 2) ──
                Int64 off = (Int64)((0x3FFUL << 52) - (xd & emsk));
                xd += (UInt64)off;
                yd += (UInt64)off;
                x = BitConverter.UInt64BitsToDouble(xd);
                y = BitConverter.UInt64BitsToDouble(yd);

                // ── square, sqrt, Newton correction ──
                double x2 = x * x, dx2 = double.FusedMultiplyAdd(x, x, -x2);
                double y2 = y * y, dy2 = double.FusedMultiplyAdd(y, y, -y2);
                double r2 = x2 + y2, ir2 = 0.5 / r2;
                double dr2 = ((x2 - r2) + y2) + (dx2 + dy2);
                double th = Math.Sqrt(r2), rsqrt = th * ir2;
                double dz = dr2 - double.FusedMultiplyAdd(th, th, -r2), tl = rsqrt * dz;
                th = BigAddDDPartial(th, tl, out tl);

                UInt64 thd = BitConverter.DoubleToUInt64Bits(th);
                UInt64 tld = BitConverter.DoubleToUInt64Bits(double.Abs(tl));
                ex = thd & emsk;
                ey = tld;
                UInt64 aidr = ey + (0x3FEUL << 52) - ex;
                UInt64 mid = (aidr - 0x3C90000000000000UL + 16) >> 5;
                UInt64 midm = (aidr - 0x3C80000000000000UL + 16) >> 5;

                if (Misc.Unlikely(mid == 0 || midm == 0
                  || aidr < 0x39B0000000000000UL
                  || aidr > 0X3c9fffffffffff80UL)) {
                    thd = BitConverter.DoubleToUInt64Bits(AsHypotHard(x, y, mode));
                } else {
                    // Fast path: thd is the correctly-rounded-to-nearest result.
                    // For directed rounding modes, adjust by one ULP in the residual's
                    // direction.  |tl| ≤ 0.5 ULP(thd) holds by DD normalization, and the
                    // exact-tie case has already been diverted to AsHypotHard above, so
                    // this is always the right correction.  (AwayFromZero == ToEven on the
                    // fast path because no tie can occur here.)
                    if (tl != 0
                        && mode != MidpointRounding.ToEven
                        && mode != MidpointRounding.AwayFromZero) {
                        if (tl > 0) {
                            if (mode == MidpointRounding.ToPositiveInfinity) thd++;
                        } else {
                            if (mode == MidpointRounding.ToZero
                                || mode == MidpointRounding.ToNegativeInfinity) thd--;
                        }
                    }
                }

                thd -= (UInt64)off;

                if (Misc.Unlikely(thd >= (0x7FFUL << 52)))
                    return AsHypotOverflow(mode);

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return BitConverter.UInt64BitsToDouble(thd);
            }
        }
    }
}