using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Misc = UltimateOrb.Miscellaneous;

#if NET8_0_OR_GREATER
using UInt128 = System.UInt128;
using Int128 = System.Int128;
#endif

namespace UltimateOrb.Numerics {
    public static partial class Binary128Arithmetic {

        // ============================================================================
        //  Raw data tables (matching `coef_bind` and `rcbrt_i` in the C code)
        // ============================================================================

        // coef_bind.knst  (u64[16]) — main coefficient words C
        static ReadOnlySpan<UInt64> RcbrtKnst => [
            0xd3ed1e72efd6337bUL, 0xb8bcbb918f877572UL, 0xa244650b6f3e7f80UL, 0x8f8418c89efa9d5eUL,
            0x7fb3d5217ebb3895UL, 0x724398c31e7fd263UL, 0x66c3629b4e47ff19UL, 0x5cd331ca2e136288UL,
            0x543b0596ede1ad3bUL, 0x4cbadd683db29a42UL, 0x4622b8bd8d85ed78UL, 0x4052972a5d5b7219UL,
            0x3b327852ad32f99eUL, 0x369a5be7fd0c5ad6UL, 0x328241a6bce7711bUL, 0x2eda2954acc41bb2UL,
        ];

        // coef_bind.knsth  (unsigned char[16]) — extra low-order byte for c3
        static ReadOnlySpan<byte> RcbrtKnsth => [
            0xa0, 0x83, 0x6d, 0x5c, 0x4e, 0x42, 0x39, 0x31, 0x2b, 0x25, 0x21, 0x1d, 0x1a, 0x17, 0x15, 0x13,
        ];

        // rcbrt_i[] : 2^63 / cbrt(2^i)
        static ReadOnlySpan<UInt64> RcbrtI => [
            0x8000000000000000UL, 0x6597fa94f5b8f20bUL, 0x50a28be635ca2b89UL,
        ];

        // ============================================================================
        //  MultiplyHigh — high 64-bit word of a 64x64 product.
        //  (Rare: use only when the low word is genuinely not needed.)
        // ============================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt64 MultiplyHigh(UInt64 a, UInt64 b) {
            unchecked {
                DoubleArithmetic.BigMul(a, b, out UInt64 hi);
                return hi;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Int64 MultiplyHigh(Int64 a, Int64 b) {
            unchecked {
                DoubleArithmetic.BigMul(a, b, out Int64 hi);
                return hi;
            }
        }

        // ============================================================================
        //  Full 256-bit product of two UInt128 values (analogue of `mUU` in the C code).
        // ============================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 MulFull128(UInt128 a, UInt128 b, out UInt128 lo) {
            unchecked {
                UInt64 lo_word = DoubleArithmetic.BigMul(
                    (UInt64)a, (UInt64)(a >> 64),
                    (UInt64)b, (UInt64)(b >> 64),
                    out UInt64 hi_lo, out UInt64 hi_hi, out UInt64 top_hi);
                lo = ((UInt128)hi_lo << 64) | lo_word;
                return ((UInt128)top_hi << 64) | hi_hi;
            }
        }

        // ============================================================================
        //  as_rcbrt : initial reciprocal-cube-root approximation for one UInt64
        // ============================================================================

        static Int64 AsRcbrt(UInt64 m) {
            unchecked {
                const int n0 = 28, n1 = 23, n2 = 13, s = 22;

                int indx = (int)(m >> 60);
                UInt64 C = RcbrtKnst[indx];
                UInt64 c3 = RcbrtKnsth[indx];

                UInt64 c0 = C << (64 - n0);
                UInt64 c1 = (C << (64 - n0 - n1)) >> (5 + 32);
                UInt64 c2 = (C << (64 - n0 - n1 - n2)) >> (10 + 32);

                Int64 d = (Int64)((m << 4) - 0x8000000000000000UL) >> 32;
                UInt64 d2 = (UInt64)(d * d) >> 32;

                UInt64 re = c0 + d2 * c2;
                UInt64 ro = (UInt64)d * (c1 + ((d2 * c3) >> 22));
                UInt64 r = re - ro;  // error < 1e-7

                UInt64 r2 = MultiplyHigh(r, r);
                UInt64 r3 = MultiplyHigh(r2, r);
                Int64 h = (Int64)(MultiplyHigh(m, r3) + r3);

                h <<= s;
                h = MultiplyHigh(h, 0x5555555555555555L);
                UInt64 h2 = (UInt64)MultiplyHigh(h, h);
                h -= (Int64)(h2 >> (s - 1));
                Int64 hr = MultiplyHigh(h, (Int64)r) + h;

                return (Int64)(r - (UInt64)(hr >> s));
            }
        }

        // ============================================================================
        //  as_icbrt : refine a 128-bit argument `x` (already shifted) given `i`
        // ============================================================================

        static UInt128 AsIcbrt(UInt128 x, uint i) {
            unchecked {
                // r = as_rcbrt(x.hi); clamp non-negative to all-ones so top bit is set.
                UInt64 r = (UInt64)AsRcbrt((UInt64)(x >> 64));
                if (Misc.Unlikely((Int64)r >= 0))
                    r = 0xFFFFFFFFFFFFFFFFUL;
                r = MultiplyHigh(r, RcbrtI[(int)i]);
                UInt128 r2 = (UInt128)r * r;

                x >>= (int)(3 - i);
                x |= (UInt128)(1UL << (61 + (int)i)) << 64;   // set bit 61+i of hi half

                // mhUU(r2, x) = r2_hi*x_hi + high64(r2_hi*x_lo) + high64(r2_lo*x_hi)
                UInt64 r2_hi = (UInt64)(r2 >> 64), r2_lo = (UInt64)r2;
                UInt64 x_hi = (UInt64)(x >> 64), x_lo = (UInt64)x;
                UInt128 sx = (UInt128)r2_hi * x_hi
                           + MultiplyHigh(r2_hi, x_lo)
                           + MultiplyHigh(r2_lo, x_hi);

                // mhuU(r, sx) = high 128 bits of (UInt64)r * sx
                //             = r * sx_hi + high64(r * sx_lo)
                UInt128 H = (UInt128)r * (UInt64)(sx >> 64)
                          + MultiplyHigh(r, (UInt64)sx);

                Int64 h = (Int64)(UInt64)H;
                Int64 m = h >> 63;                          // 0 or -1

                // mhium(h, const, m) = high64((UInt64)h * const) - (m & const)
                UInt64 h3u = MultiplyHigh((UInt64)h, 0xAAAAAAAAAAAAAAABUL);
                Int64 h3 = (Int64)(h3u - ((UInt64)m & 0xAAAAAAAAAAAAAAABUL));

                // mhiUm(h3, sx, m) = (UInt64)h3 * sx_hi - (m & sx) + high64((UInt64)h3 * sx_lo)
                UInt128 m_u128 = (UInt128)(Int128)m;        // sign-extended mask
                UInt64 h3u_u = (UInt64)h3;
                UInt128 dsVal = (UInt128)h3u_u * (UInt64)(sx >> 64)
                               - (m_u128 & sx)
                               + MultiplyHigh(h3u_u, (UInt64)sx);
                Int128 ds = (Int128)dsVal >> (58 - 5);

                sx <<= 5;
                UInt128 sx1 = sx - (UInt128)ds;

                short dd = (Int16)((UInt64)sx1 << 1);
                if (Misc.Unlikely(dd > -512 && dd < 512)) {                // can round?
                    UInt128 c = ((sx1 + 0x4000) >> 15) | ((UInt128)1 << 113);

                    UInt128 R2h = MulFull128(c, c, out UInt128 R2l);
                    UInt128 R3h = MulFull128(R2l, c, out UInt128 R30);
                    UInt128 R31 = R2h * c + R3h;
                    R31 -= x << 86;

                    sx1 = c << 15;
                    if ((Int128)R31 < 0)
                        sx1 += 1;
                    else if ((Int128)R31 > 0)
                        sx1 -= 1;
                    else if (R30 != 0)
                        sx1 -= 1;
                }
                return sx1;
            }
        }

        // ============================================================================
        //  Cbrt overloads
        // ============================================================================

        [CLSCompliant(false)]
        public static UInt64 Cbrt(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Cbrt(lo, hi, MidpointRounding.ToEven, out result_hi);

        [CLSCompliant(false)]
        public static UInt64 Cbrt(UInt64 lo, UInt64 hi, MidpointRounding rounding, out UInt64 result_hi) {
            unchecked {
                UInt64 sign = hi >> 63;                      // 0 or 1
                UInt64 e = (hi >> 48) & 0x7FFF;           // 15-bit raw exponent
                int i;
                Int64 e3;

                UInt128 u = ((UInt128)hi << 64) | lo;

                if (Misc.Unlikely(e == 0)) {
                    // Subnormal or zero
                    int ns = -15;
                    UInt64 hiNoSign = hi & 0x7FFFFFFFFFFFFFFFUL;
                    u = ((UInt128)hiNoSign << 64) | lo;

                    if (hiNoSign != 0) {
                        ns += (int)UInt64.LeadingZeroCount(hiNoSign);
                    } else if (lo != 0) {
                        ns += (int)UInt64.LeadingZeroCount(lo) + 64;
                    } else {
                        // ±0
                        result_hi = hi;
                        return lo;
                    }
                    e = (UInt64)(121 - ns);
                    e3 = (Int64)(e / 3 + 10882);
                    i = (int)(e % 3);
                    u <<= ns;
                } else if (Misc.Unlikely(e == 0x7FFF)) {
                    // NaN / Inf
                    UInt64 frac = (hi & 0x0000FFFFFFFFFFFFUL) | lo;
                    if (frac != 0 && (hi & (1UL << 47)) == 0) {
                        hi |= 1UL << 47;                     // sNaN -> qNaN
                    }
                    result_hi = hi;
                    return lo;
                } else {
                    e3 = (Int64)(e / 3 + 10922);
                    i = (int)(e % 3);
                }

                u <<= 16;
                UInt128 v = AsIcbrt(u, (uint)i);

                UInt64 b1 = (UInt64)(v >> 64);
                UInt64 b0 = (UInt64)v;

                UInt64 sbit = (b0 << 48) != 0 ? 1UL : 0UL; // sticky: low 16 bits nonzero
                UInt64 rbit = (b0 >> 15) & 1UL;            // MSB of discarded 16 bits
                UInt64 mantissa_lsb = (b0 >> 16) & 1UL;            // LSB of retained mantissa

                b0 >>= 16;

                UInt64 rnd;
                if (Misc.Likely(rounding == MidpointRounding.ToEven)) {
                    // Ties-to-even: round up iff above half, or exactly half with LSB odd.
                    rnd = (rbit != 0 && (sbit != 0 || mantissa_lsb != 0)) ? 1UL : 0UL;
                } else switch (rounding) {
                    case MidpointRounding.AwayFromZero:
                        // The C source has no native "ties-away-from-zero" mode. Per
                        // project convention, approximate it by choosing the directional
                        // mode from the result's sign: positive -> ToPositiveInfinity
                        // semantics, negative -> ToNegativeInfinity semantics. Both
                        // reduce to `sbit`.
                        rnd = sbit;
                        break;

                    case MidpointRounding.ToPositiveInfinity:
                        // Toward +inf: positive -> up in magnitude; negative -> toward zero.
                        rnd = (sign == 0) ? sbit : 0UL;
                        break;

                    case MidpointRounding.ToNegativeInfinity:
                        // Toward -inf: negative -> up in magnitude; positive -> toward zero.
                        rnd = (sign != 0) ? sbit : 0UL;
                        break;

                    case MidpointRounding.ToZero:
                    default:
                        rnd = 0UL;
                        break;
                    }

                b0 |= b1 << 48;
                b1 >>= 16;
                UInt128 v2 = ((UInt128)b1 << 64) | b0;

                // Pack sign + adjusted exponent into hi half; put the rounding
                // increment into the lo half.
                UInt64 e3_bits = (UInt64)(e3 << 48) | (sign << 63);
                UInt128 dv = ((UInt128)e3_bits << 64) | rnd;
                v2 += dv;

                result_hi = (UInt64)(v2 >> 64);
                return (UInt64)v2;
            }
        }
    }
}