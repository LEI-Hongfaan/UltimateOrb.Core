using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb;
using UltimateOrb.Mathematics;
using UltimateOrb.Utilities;
using UltimateOrb.Utilities.Extensions;
using static UltimateOrb.Utilities.BooleanIntegerModule;
using static UltimateOrb.Utilities.ThrowHelper;
namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

#if NET8_0_OR_GREATER
    using U256 = InlineArray2<System.UInt128>;
    using U384 = InlineArray3<System.UInt128>;
#else 
    using U256 = InlineArray2<UInt128>;
    using U384 = InlineArray3<UInt128>;
#endif

    partial class Binary128Arithmetic {

        // =====================================================================
        //  Hypot:  sqrt(x^2 + y^2)  on binary128 bit patterns
        // =====================================================================

        /* ---- 128-bit primitives used by the main path ---- */

        /* high 128 bits of a 64x128 product */
        static UInt128 MultiplyHighApproximate(UInt64 y, UInt128 x) {
            unchecked {
                UInt64 xl = (UInt64)x, xh = (UInt64)(x >> 64);
                UInt128 xy0 = (UInt128)xl * y;
                UInt128 xy1 = (UInt128)xh * y;
                return xy1 + (xy0 >> 64);
            }
        }

        /* approximate high 128 bits of a 128-bit square; at most 2 units short */
        static UInt128 SquareHighApproximate(UInt128 a) {
            unchecked {
                UInt64 al = (UInt64)a, ah = (UInt64)(a >> 64);
                UInt128 a10 = (UInt128)al * ah;
                UInt128 a11 = (UInt128)ah * ah;
                a11 += a10 >> 64;
                a11 += a10 >> 64;
                return a11;
            }
        }

        /* full product of a 128-bit square; returns the high 128 bits and writes
           the low 128 through *lo.  The double-add is the C original's two
           cross-term accumulations. */
        static UInt128 BigSquare(UInt128 a, out UInt128 lo) {
            unchecked {
                UInt64 al = (UInt64)a, ah = (UInt64)(a >> 64);
                UInt128 a10 = (UInt128)al * ah;
                UInt128 a11 = (UInt128)ah * ah;
                UInt128 a00 = (UInt128)al * al;

                UInt64 a00h = (UInt64)(a00 >> 64);
                UInt64 a10l = (UInt64)a10, a10h = (UInt64)(a10 >> 64);
                UInt64 a11l = (UInt64)a11, a11h = (UInt64)(a11 >> 64);

                nuint c;
                a00h = AddWithCarry(a00h, a10l, 0, out c);
                a11l = AddWithCarry(a11l, a10h, c, out c);
                a11h = AddWithCarry(a11h, 0, c, out c);
                a00h = AddWithCarry(a00h, a10l, 0, out c);
                a11l = AddWithCarry(a11l, a10h, c, out c);
                a11h = AddWithCarry(a11h, 0, c, out c);

                lo = ((UInt128)a00h << 64) | (UInt64)a00;
                return ((UInt128)a11h << 64) | a11l;
            }
        }

        /* 128 + 128 with carry-out bit */
        static UInt128 AddWithCarry(UInt128 a, UInt128 b, out nuint c) {
            unchecked {
                UInt64 al = (UInt64)a, ah = (UInt64)(a >> 64);
                UInt64 bl = (UInt64)b, bh = (UInt64)(b >> 64);
                nuint d;
                al = AddWithCarry(al, bl, 0, out d);
                ah = AddWithCarry(ah, bh, d, out d);
                c = d;
                return ((UInt128)ah << 64) | al;
            }
        }

        /* (u128 x i128) high 128, given a mask that is all-ones when b is negative */
        static Int128 MultiplyHighApproximateMasked(UInt128 a, Int128 b, UInt64 mask) {
            unchecked {
                UInt64 sublo = (UInt64)a & mask;
                UInt64 subhi = (UInt64)(a >> 64) & mask;
                UInt128 sub = ((UInt128)subhi << 64) | sublo;
                return (Int128)(MultiplyHighApproximate(a, (UInt128)b) - sub);
            }
        }

        static Int128 MultiplyHighApproximate(Int128 b, UInt128 a) {
            unchecked { return MultiplyHighApproximateMasked(a, b, (UInt64)(b >> 127)); }
        }

        /* ---- rsqrt9: 64x64 -> 64 reciprocal square root, one Newton step ---- */

        static ReadOnlySpan<UInt32> RSQRT9_TABLE => [
            0xffffffff, 0xfffff780, 0xbff55815, 0x9bb5b6e7, 0xfc0bd889, 0xfa1d6e7d, 0xb8a95a89, 0x938bf8f0,
    0xf82ec882, 0xf473bea9, 0xb1bf4705, 0x8bed0079, 0xf467f280, 0xeefff2a1, 0xab309d4a, 0x84cdb431,
    0xf0b6848c, 0xe9bf46f4, 0xa4f76232, 0x7e24037b, 0xed19b75e, 0xe4af2628, 0x9f0e1340, 0x77e6ca62,
    0xe990cdad, 0xdfcd2521, 0x996f9b96, 0x720db8df, 0xe61b138e, 0xdb16ffde, 0x94174a00, 0x6c913cff,
    0xe2b7dddf, 0xd68a967b, 0x8f00c812, 0x676a6f92, 0xdf6689b7, 0xd225ea80, 0x8a281226, 0x62930308,
    0xdc267bea, 0xcde71c63, 0x8589702c, 0x5e05343e, 0xd8f7208e, 0xc9cc6948, 0x81216f2e, 0x59bbbcf8,
    0xd5d7ea91, 0xc5d428ee, 0x7cecdb76, 0x55b1c7d6, 0xd2c8534e, 0xc1fccbc9, 0x78e8bb45, 0x51e2e592,
    0xcfc7da32, 0xbe44d94a, 0x75124a0a, 0x4e4b0369, 0xccd6045f, 0xbaaaee41, 0x7166f40f, 0x4ae66284,
    0xc9f25c5c, 0xb72dbb69, 0x6de45288, 0x47b19045, 0xc71c71c7, 0xb3cc040f, 0x6a882804, 0x44a95f5f,
    0xc453d90f, 0xb0849cd4, 0x67505d2a, 0x41cae1a0, 0xc1982b2e, 0xad566a85, 0x643afdc8, 0x3f13625c,
    0xbee9056f, 0xaa406113, 0x6146361f, 0x3c806169, 0xbc46092e, 0xa7418293, 0x5e70506d, 0x3a0f8e8e,
    0xb9aedba5, 0xa458de58, 0x5bb7b2b1, 0x37bec572, 0xb72325b7, 0xa1859022, 0x591adc9a, 0x358c09e2,
    0xb4a293c2, 0x9ec6bf52, 0x569865a7, 0x33758476, 0xb22cd56d, 0x9c1b9e36, 0x542efb6a, 0x31797f8a,
    0xafc19d86, 0x9983695c, 0x51dd5ffb, 0x2f96647a, 0xad60a1d1, 0x96fd66f7, 0x4fa2687c, 0x2dcab91f,
    0xab099ae9, 0x9488e64b, 0x4d7cfbc9, 0x2c151d8a, 0xa8bc441a, 0x92253f20, 0x4b6c1139, 0x2a7449ef,
    0xa6785b42, 0x8fd1d14a, 0x496eaf82, 0x28e70cc3, 0xa43da0ae, 0x8d8e042a, 0x4783eba7, 0x276c4900,
    0xa20bd701, 0x8b594648, 0x45aae80a, 0x2602f493, 0x9fe2c315, 0x89330ce4, 0x43e2d382, 0x24aa16ec,
    0x9dc22be4, 0x871ad399, 0x422ae88c, 0x2360c7af, 0x9ba9da6c, 0x85101c05, 0x40826c88, 0x22262d7b,
    0x99999999, 0x83126d70, 0x3ee8af07, 0x20f97cd2, 0x97913630, 0x81215480, 0x3d5d0922, 0x1fd9f714,
    0x95907eb8, 0x7f3c62ef, 0x3bdedce0, 0x1ec6e994, 0x93974369, 0x7d632f45, 0x3a6d94a9, 0x1dbfacbb,
    0x91a55615, 0x7b955498, 0x3908a2be, 0x1cc3a33b, 0x8fba8a1c, 0x79d2724e, 0x37af80bf, 0x1bd23960,
    0x8dd6b456, 0x781a2be4, 0x3661af39, 0x1aeae458, 0x8bf9ab07, 0x766c28ba, 0x351eb539, 0x1a0d21a2,
    0x8a2345cc, 0x74c813dd, 0x33e61feb, 0x19387676, 0x88535d90, 0x732d9bdc, 0x32b7823a, 0x186c6f3e,
    0x8689cc7e, 0x719c7297, 0x3192747d, 0x17a89f21, 0x84c66df1, 0x70144d19, 0x30769424, 0x16ec9f89,
    0x83091e6a, 0x6e94e36c, 0x2f63836f, 0x16380fbf, 0x8151bb87, 0x6d1df079, 0x2e58e925, 0x158a9484,
    0x7fa023f1, 0x6baf31de, 0x2d567053, 0x14e3d7ba, 0x7df43758, 0x6a4867d3, 0x2c5bc811, 0x1443880e,
    0x7c4dd664, 0x68e95508, 0x2b68a346, 0x13a958ab, 0x7aace2b0, 0x6791be86, 0x2a7cb871, 0x131500ee,
    0x79113ebc, 0x66416b95, 0x2997c17a, 0x12863c29, 0x777acde8, 0x64f825a1, 0x28b97b82, 0x11fcc95c,
    0x75e9746a, 0x63b5b822, 0x27e1a6b4, 0x11786b03, 0x745d1746, 0x6279f081, 0x2710061d, 0x10f8e6da,
    0x72d59c46, 0x61449e06, 0x26445f86, 0x107e05ac, 0x7152e9f4, 0x601591be, 0x257e7b4d, 0x10079327,
    0x6fd4e793, 0x5eec9e6b, 0x24be2445, 0x0f955da9, 0x6e5b7d16, 0x5dc9986e, 0x24032795, 0x0f273620,
    0x6ce6931d, 0x5cac55b7, 0x234d5496, 0x0ebcefdb, 0x6b7612ec, 0x5b94adb2, 0x229c7cbc, 0x0e56606e,
];

        static UInt64 Rsqrt9(UInt64 m) {
            unchecked {
                ReadOnlySpan<uint> c = RSQRT9_TABLE;
                int indx = (int)(m >> 58);
                UInt64 c3 = c[indx * 4 + 3], c0 = c[indx * 4 + 0], c1 = c[indx * 4 + 1], c2 = c[indx * 4 + 2];
                c0 <<= 31;
                c0 |= 1UL << 63;
                c1 <<= 25;
                UInt64 d = (m << 6) >> 32;
                UInt64 d2 = ((UInt64)d * d) >> 32;
                UInt64 re = c0 + ((d2 * c2) >> 13);
                UInt64 ro = d * ((c1 + ((d2 * c3) >> 19)) >> 26) >> 6;
                UInt64 r = re - ro;
                UInt64 r2 = MultiplyHigh(r, r);
                Int64 h = (Int64)(MultiplyHigh(m, r2) + r2);
                Int64 hr = MultiplyHigh(h, (Int64)(r >> 1));
                r = (UInt64)((Int64)r - hr);
                if (r == 0) r--;
                return r;
            }
        }

        /* ---- the port ---- */

        public static UInt128 Hypot(UInt128 x, UInt128 y) => Hypot(x, y, MidpointRounding.ToEven);

        public static UInt128 Hypot(UInt128 x, UInt128 y, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;
                UInt64 xh = (UInt64)(x >> 64), xl = (UInt64)x;
                UInt64 yh = (UInt64)(y >> 64), yl = (UInt64)y;
                xh &= ~smsk;
                yh &= ~smsk;
                UInt128 X = ((UInt128)xh << 64) | xl;
                UInt128 Y = ((UInt128)yh << 64) | yl;

                /* order so that a >= b (unsigned bit-pattern compare) */
                Int128 dab = (Int128)X - (Int128)Y;
                dab &= dab >> 127;                          /* 0 if X>=Y, all-ones if X<Y */
                UInt128 a = (UInt128)((Int128)X - dab);
                UInt128 b = (UInt128)((Int128)Y + dab);

                int xn = (int)((UInt64)(a >> 64) >> 48);
                int yn = (int)((UInt64)(b >> 64) >> 48);

                if (xn == 0x7fff) {                         /* a is inf or NaN */
                    int xnan = GetClass(a);
                    int ynan = GetClass(b);
                    if (xnan == 2 || ynan == 2) {           /* sNaN: invalid, return qNaN */
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        // return ((UInt128)0x7fff << 112) | ((UInt128)1 << 111);
                        return xnan == 2 ? GetNaN(a) : GetNaN(b);
                    } else if (xnan + ynan == 4) {          /* both quiet: +inf */
                        return (UInt128)0x7fff << 112;
                    } else if (xnan == 3) {
                        return a;
                    } else if (ynan == 3) {
                        return b;
                    } else {
                        return (UInt128)0x7fff << 112;
                    }
                }

                if (yn == 0) {                              /* b is subnormal or zero */
                    int ns = -15;
                    UInt64 bh = (UInt64)(b >> 64), bl = (UInt64)b;
                    if (bh != 0) {
                        ns += (int)UInt64.LeadingZeroCount(bh);
                    } else {
                        if (bl != 0) {
                            ns += (int)UInt64.LeadingZeroCount(bl) + 64;
                        } else {
                            return a;                       /* b == 0: hypot = a */
                        }
                    }
                    yn = 1 - ns;
                    b <<= ns;                               /* always ns >= 1 for nonzero subnormal b */

                    if (xn == 0) {                          /* a is subnormal too */
                        ns = -15;
                        UInt64 ah = (UInt64)(a >> 64), al = (UInt64)a;
                        if (ah != 0) {
                            ns += (int)UInt64.LeadingZeroCount(ah);
                        } else {
                            ns += (int)UInt64.LeadingZeroCount(al) + 64;
                        }
                        xn = 1 - ns;
                        a <<= ns;
                    }
                }

                int dn = xn - yn;

                /* normalize mantissas: top bit of a at position 127 */
                a <<= 15;
                a |= (UInt128)1 << 127;

                UInt128 v;
                if (dn > 56) {
                    /* b is negligibly small: make the result not exactly a */
                    v = a | 1;
                } else {
                    b <<= 15;
                    b |= (UInt128)1 << 127;

                    UInt128 a2hi = SquareHighApproximate(a);
                    UInt128 b2hi = SquareHighApproximate(b >> dn);

                    nuint overflow;
                    a2hi = AddWithCarry(a2hi, b2hi, out overflow);

                    int clz = (int)UInt64.LeadingZeroCount((UInt64)(a2hi >> 64));
                    int nz = (int)(((UInt64)(~0u) + overflow) & (UInt64)clz);
                    int i = ((overflow == 0 ? 1 : 0) + nz) & 1;
                    int s = 1 - (int)overflow + nz;
                    a2hi <<= s;
                    xn += (int)overflow;

                    UInt64 rx = (UInt64)(a2hi >> 64);
                    UInt64 r = Rsqrt9(rx);

                    UInt64 rsqrt2 = i == 0 ? ~0UL : 0xb504f333f9de6484UL;    /* 2^64/sqrt(2) */
                    UInt128 r2 = (UInt128)r * rsqrt2;

                    int shft = 2 - i;
                    a2hi >>= shft;
                    a2hi |= (UInt128)1 << (126 + i);

                    r = (UInt64)(r2 >> 64);

                    UInt128 sx = MultiplyHighApproximate(r, a2hi);
                    Int128 h = (Int128)MultiplyHighApproximate(r, sx) << 2;
                    Int128 ds = MultiplyHighApproximate(h, sx);
                    sx <<= 1;
                    v = (UInt128)((Int128)sx - ds);

                    short dd = (short)(((UInt64)v) << 2);
                    if (dd > -37 && dd < 13) {              /* near a rounding boundary: refine */
                        v += (UInt128)1 << 13;
                        v &= ~(UInt128)0x3fff;

                        UInt64 over2 = v == 0 ? 1UL : 0UL;
                        UInt128 c = v >> 13;
                        UInt128 c2 = c * c;

                        a >>= 13;
                        UInt128 al;
                        UInt128 ah = BigSquare(a, out al);

                        b >>= 13;
                        UInt128 bl;
                        UInt128 bh = BigSquare(b, out bl);

                        if (dn != 0) {
                            int sh2 = 2 * dn;
                            int shift = 128 - sh2;
                            UInt128 bl_old = bl;
                            bl = (bl_old >> sh2) | (bh << shift);
                            if ((bl_old << shift) != 0) bl |= 1;
                            bh >>= sh2;
                        }

                        UInt128 alBefore = al;
                        al += bl;
                        UInt64 carryAdd = (al < alBefore) ? 1UL : 0UL;
                        ah += bh + (UInt128)carryAdd;

                        s = (int)((ah >> 102) * 2);
                        if (s != 0) {
                            UInt128 al_old2 = al;
                            int shift2 = 128 - s;
                            al = (al_old2 >> s) | (ah << shift2);
                            if ((al_old2 << shift2) != 0) al |= 1;
                        }

                        al -= c2;
                        v |= (UInt128)over2 << 127;
                        xn += (int)over2;

                        if ((Int128)al < 0) v -= 1;
                        else if ((Int128)al > 0) v += 1;
                    }
                }

                /* round and place */
                xn--;
                if (xn > 0) {
                    if (xn >= 32766) {
                        /* overflow range */
                        UInt64 over = (UInt64)(v >> 127);
                        int rnd;
                        if (rm == MidpointRounding.ToEven || rm == MidpointRounding.ToPositiveInfinity) rnd = 1;
                        else rnd = 0;
                        v = ((UInt128)0x7fff << 112) - 1;
                        v += (UInt128)(UInt64)rnd;
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                        if ((UInt64)(v >> 64) == (0x7fffUL << 48) || over != 0) {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Overflow);
                        }
                    } else {
                        /* normal range */
                        UInt64 vlow = (UInt64)v;
                        UInt64 frac = vlow & 0x7fff;
                        int rnd;
                        if (rm == MidpointRounding.ToEven) {
                            if (frac == 0x4000) rnd = (int)((vlow >> 15) & 1);
                            else rnd = (int)(frac >> 14);
                        } else if (rm == MidpointRounding.ToPositiveInfinity) {
                            rnd = frac != 0 ? 1 : 0;
                        } else {
                            rnd = 0;
                        }
                        v >>= 15;
                        v += (UInt128)(UInt64)rnd;
                        v += (UInt128)(UInt64)xn << 112;
                        if (frac != 0) RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                    }
                } else {
                    /* subnormal range */
                    UInt128 frac = v & (((UInt128)1 << (15 - xn)) - 1);
                    int rnd;
                    if (rm == MidpointRounding.ToEven) {
                        UInt128 threshold = (UInt128)1 << (14 - xn);
                        if (frac == threshold) rnd = (int)((v >> (15 - xn)) & 1);
                        else rnd = (int)(frac >> (14 - xn));
                    } else if (rm == MidpointRounding.ToPositiveInfinity) {
                        rnd = frac != 0 ? 1 : 0;
                    } else {
                        rnd = 0;
                    }
                    v >>= (15 - xn);
                    v += (UInt128)(UInt64)rnd;

                    if (frac != 0) {
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                        if ((UInt64)(v >> 64) < (1UL << 48)) {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);
                        }
                    }
                }

                return v;
            }
        }

    }
}