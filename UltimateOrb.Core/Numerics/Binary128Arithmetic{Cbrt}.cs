using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Misc = UltimateOrb.Miscellaneous;
using System.Diagnostics;

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    // Part I: Helpers

    public static partial class Binary128Arithmetic {

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 AddWithCarry(UInt64 a, UInt64 b, nuint carry, out nuint newCarry) {
            unchecked {
                UInt64 s0 = a + b;
                nuint c0 = s0 < a ? 1u : 0u;
                UInt64 s1 = s0 + carry;
                nuint c1 = s1 < s0 ? 1u : 0u;
                newCarry = c0 + c1;
                return s1;
            }
        }

        // -------- Multiplication helpers (unified names) --------

        // C: mhuu(u64, u64) — high 64 bits of an unsigned 64x64 product.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 MultiplyHigh(UInt64 a, UInt64 b) {
            unchecked {
                return (UInt64)((UInt128)a * b >> 64);
            }
        }

        // C: mhii(i64, i64) — signed high 64 bits.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Int64 MultiplyHigh(Int64 a, Int64 b) {
            unchecked {
                return (Int64)((Int128)a * b >> 64);
            }
        }

        // C: mhium(i64 x, u64 y, i64 masky)
        //     z = (u64)x;  s = (u64)masky & y;
        //     r = (i64)(((u128)z * y) >> 64);
        //     r -= s;
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Int64 MultiplyHighMasked(Int64 x, UInt64 y, Int64 mask) {
            unchecked {
                UInt64 z = (UInt64)x;
                UInt64 s = (UInt64)mask & y;
                Int64 r = (Int64)((UInt128)z * y >> 64);
                return (Int64)((UInt64)r - s);
            }
        }

        // C: mhiUm(i64 y, u128 x, i64 m)
        //     xy1 = (u128)x.hi * (u64)y;
        //     xy0 = (u128)x.lo * (u64)y;
        //     return xy1 - (m & x) + (xy0 >> 64);
        //     (m&x uses sign-extended (u128)m per C promotion rules.)
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static Int128 MultiplyHighApproximateMasked(Int64 y, UInt128 x, Int64 m) {
            unchecked {
                UInt64 uy = (UInt64)y;
                UInt64 x_lo = (UInt64)x;
                UInt64 x_hi = (UInt64)(x >> 64);
                UInt128 xy1 = (UInt128)x_hi * uy;
                UInt128 xy0 = (UInt128)x_lo * uy;
                UInt128 m_ext = (UInt128)(Int128)m;   // sign-extend to 128 bits
                UInt128 m_and_x = m_ext & x;
                return (Int128)(xy1 - m_and_x + (xy0 >> 64));
            }
        }

        // C: mhUU(u128, u128) — high 128 bits of an unsigned 128x128 product.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt128 MultiplyHighApproximate(UInt128 a, UInt128 b) {
            unchecked {
                UInt64 a_lo = (UInt64)a, a_hi = (UInt64)(a >> 64);
                UInt64 b_lo = (UInt64)b, b_hi = (UInt64)(b >> 64);
                UInt128 a1b0 = Math.BigMul(a_hi, b_lo);
                UInt128 a0b1 = Math.BigMul(a_lo, b_hi);
                UInt128 a1b1 = Math.BigMul(a_hi, b_hi);
                a1b1 += a1b0 >> 64;
                a1b1 += a0b1 >> 64;
                return a1b1;
            }
        }

        // C: mhuU(u64 y, u128 x) — high 128 bits of an unsigned 64x128 product.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt128 MultiplyHigh(UInt64 y, UInt128 x) {
            unchecked {
                UInt64 x_lo = (UInt64)x, x_hi = (UInt64)(x >> 64);
                UInt128 xy0 = (UInt128)x_lo * y;
                UInt128 xy1 = (UInt128)x_hi * y;
                return xy1 + (xy0 >> 64);
            }
        }

        // C: mUU(u128, u128, u128 *t) — full 128x128 product.
        //     Returns the high 128 bits, stores the low 128 bits in `low`.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt128 BigMul(UInt128 a, UInt128 b, out UInt128 low) {
            unchecked {
                low = DoubleArithmetic.BigMul(a, b, out var hi);
                return hi;
            }
        }
    }

    public static partial class Binary128Arithmetic {
        // -------- Reciprocal-cube-root approximation (initial refinement) --------

        // from `coef_bind.knst`
        static ReadOnlySpan<UInt64> CbrtCoefficients => [
            0xd3ed1e72efd6337b, 0xb8bcbb918f877572, 0xa244650b6f3e7f80, 0x8f8418c89efa9d5e,
            0x7fb3d5217ebb3895, 0x724398c31e7fd263, 0x66c3629b4e47ff19, 0x5cd331ca2e136288,
            0x543b0596ede1ad3b, 0x4cbadd683db29a42, 0x4622b8bd8d85ed78, 0x4052972a5d5b7219,
            0x3b327852ad32f99e, 0x369a5be7fd0c5ad6, 0x328241a6bce7711b, 0x2eda2954acc41bb2
        ];

        // from `coef_bind.knsth`
        static ReadOnlySpan<Byte> CbrtCoefficientsHi => [
            0xa0, 0x83, 0x6d, 0x5c, 0x4e, 0x42, 0x39, 0x31, 0x2b, 0x25, 0x21, 0x1d, 0x1a, 0x17, 0x15, 0x13
        ];

        // from `rcbrt_i`
        static ReadOnlySpan<UInt64> ReciprocalCbrtTable => [
            0x8000000000000000, 0x6597fa94f5b8f20b, 0x50a28be635ca2b89
        ];

        // C: as_rcbrt(u64 m) -> i64
        // Returns 1 + r, with r being a small signed residual (|r| < 1e-7 scaled).
        internal static Int64 ReciprocalCbrtApprox(UInt64 m) {
            unchecked {
                const int n0 = 28, n1 = 23, n2 = 13, s = 22;

                UInt64 indx = m >> 60;
                UInt64 C = CbrtCoefficients[(int)indx];
                UInt64 c3 = CbrtCoefficientsHi[(int)indx];

                UInt64 c0 = C << (64 - n0);
                UInt64 c1 = (C << (64 - n0 - n1)) >> (5 + 32);
                UInt64 c2 = (C << (64 - n0 - n1 - n2)) >> (10 + 32);

                // d = (i64)((m<<4) - 2^63) >> 32   (arithmetic shift)
                Int64 d = (Int64)((m << 4) - (1UL << 63)) >> 32;

                // d2 = ((u64)(d*d)) >> 32
                UInt64 d2 = (UInt64)(d * d) >> 32;

                UInt64 re = c0 + d2 * c2;
                UInt64 ro = (UInt64)d * (c1 + ((d2 * c3) >> 22));
                UInt64 r = re - ro;                    // error < 1e-7

                UInt64 r2 = MultiplyHigh(r, r);
                UInt64 r3 = MultiplyHigh(r2, r);

                Int64 h = (Int64)(MultiplyHigh(m, r3) + r3);
                h <<= s;
                h = MultiplyHigh(h, 0x5555555555555555L);       // 2/3
                UInt64 h2 = (UInt64)MultiplyHigh(h, h);
                h = (Int64)((UInt64)h - (h2 >> (s - 1)));
                Int64 hr = MultiplyHigh(h, (Int64)r) + h;

                return (Int64)(r - (UInt64)(hr >> s));
            }
        }

        // C: as_icbrt(b128u128_u x, unsigned i) -> b128u128_u
        //    `x` holds the mantissa in the high bits (already left-shifted by 16 at call site).
        internal static UInt128 InitialCbrt(UInt128 x, UInt32 i) {
            unchecked {
                // r = as_rcbrt(x.b[1]); if ((i64)r >= 0) r = ~0;
                UInt64 r = (UInt64)ReciprocalCbrtApprox((UInt64)(x >> 64));
                if ((Int64)r >= 0) r = ~0UL;

                r = MultiplyHigh(r, ReciprocalCbrtTable[(int)i]);

                UInt128 r2 = (UInt128)r * r;

                x >>= (int)(3 - i);
                // x.b[1] |= 1ull << (61+i)   =>   bit (61+i+64) of the u128
                x |= (UInt128)1 << (61 + 64 + (int)i);

                UInt128 sx = MultiplyHighApproximate(r2, x);

                UInt128 H = MultiplyHigh(r, sx);
                Int64 h = (Int64)(UInt64)H;           // truncation to 64 bits (low word of H)
                Int64 m = h >> 63;                    // sign mask: 0 or -1

                Int64 h3 = MultiplyHighMasked(h, 0xAAAAAAAAAAAAAAABUL, m);   // h *= 2/3
                Int128 ds = MultiplyHighApproximateMasked(h3, sx, m) >> (58 - 5);

                sx <<= 5;
                UInt128 sx1 = sx - (UInt128)ds;

                // dd = (short)((low-64-bits of sx1) << 1)
                Int16 dd = unchecked((Int16)((UInt64)sx1 << 1));
                if (dd > -512 && dd < 512) {
                    // can round: perform one extra Newton step at higher precision
                    UInt128 c = ((sx1 + ((UInt128)1 << 14)) >> 15) | ((UInt128)1 << 113);

                    UInt128 R2l;
                    UInt128 R2h = BigMul(c, c, out R2l);
                    UInt128 R30;
                    UInt128 R31 = BigMul(R2l, c, out R30) + R2h * c;
                    R31 -= x << 86;

                    sx1 = c << 15;
                    if (unchecked((Int128)R31) < 0)
                        sx1 += 1;
                    else if (unchecked((Int128)R31) > 0)
                        sx1 -= 1;
                    else if (R30 != 0)
                        sx1 -= 1;
                }

                return sx1;
            }
        }
    }
    // Part II: Cbrt overloads

    public static partial class Binary128Arithmetic {

        public static UInt64 Cbrt(UInt64 lo, UInt64 hi, out UInt64 result_hi) {
            return CbrtCore(lo, hi, MidpointRounding.ToEven, out result_hi);
        }

        public static UInt64 Cbrt(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi) {
            return CbrtCore(lo, hi, mode, out result_hi);
        }

        [Conditional("DEBUG")]
        private static void RaiseExceptionFlagsDummy(FloatingPointExceptionFlags flags) {
        }


        private static NotSupportedException ThrowNotSupportedException_MidpointRounding(MidpointRounding value) {
            throw new NotSupportedException($"The specified {nameof(System.MidpointRounding)} value '{value}' is not supported.");
        }

        private static UInt64 CbrtCore(UInt64 x_lo, UInt64 x_hi, MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                int sign = GetRawSignFromHi64Bits(x_hi);
                uint e = unchecked((uint)GetRawExponentFromHi64Bits(x_hi));

                UInt128 u = ((UInt128)x_hi << 64) | x_lo;
                Int64 e3;
                uint i;

                if (e == 0) {
                    // Subnormal or ±0.  The sign bit is cleared, and the
                    // significand is normalized so that its top bit ends up at
                    // position 47 of `u`'s high word.
                    int ns = -15;
                    u &= ~((UInt128)1 << 127);

                    UInt64 u_hi = (UInt64)(u >> 64);
                    UInt64 u_lo = (UInt64)u;
                    if (u_hi != 0) {
                        ns += (int)UInt64.LeadingZeroCount(u_hi);
                    } else if (u_lo != 0) {
                        ns += (int)UInt64.LeadingZeroCount(u_lo) + 64;
                    } else {
                        // ±0
                        result_hi = x_hi;
                        return x_lo;
                    }
                    e = unchecked((uint)(121 - ns));
                    e3 = (Int64)(e / 3 + 10882);
                    i = e % 3;
                    u <<= ns;
                } else if (e == 0x7FFF) {
                    // NaN or Inf.
                    UInt64 frac_hi = (UInt64)(u >> 64) & 0x0000FFFFFFFFFFFFUL;
                    UInt64 frac_lo = (UInt64)u;
                    if (((frac_hi | frac_lo) != 0) && ((frac_hi & (1UL << 47)) == 0)) {
                        // sNaN -> qNaN, signal FE_INVALID.
                        u |= (UInt128)1 << (64 + 47);
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        result_hi = (UInt64)(u >> 64);
                        return (UInt64)u;
                    }
                    // ±Inf or qNaN: return unchanged.
                    result_hi = x_hi;
                    return x_lo;
                } else {
                    e3 = (Int64)(e / 3 + 10922);
                    i = e % 3;
                }

                // Bring the significand to the layout expected by InitialCbrt.
                u <<= 16;
                UInt128 v = InitialCbrt(u, i);

                UInt64 b1 = (UInt64)(v >> 64);
                UInt64 b0 = (UInt64)v;

                // `sbit`: any of the low 16 bits of b0 set (sticky bit).
                // `rbit`: bit 15 of b0 (round bit).
                UInt64 sbit = ((b0 << 48) != 0) ? 1UL : 0UL;
                if (sbit != 0) RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);          // FE_INEXACT

                UInt64 rbit = (b0 >> 15) & 1UL;
                b0 >>= 16;

                // -------- Rounding decision --------
                UInt64 rnd;
                switch (mode) {
                case MidpointRounding.ToEven:
#if false
                    // Correct round-half-to-even.  (For f128 cbrt the exact-tie
                    // case essentially never occurs because a cube root of a
                    // representable f128 value is either exactly representable
                    // with far fewer than 113 significant bits, or irrational
                    // with >113 significant bits.  The branch is still here
                    // for complete correctness.)
                    if (rbit == 0) rnd = 0;
                    else if (sbit != 0) rnd = 1;
                    else rnd = b0 & 1;   // exact tie -> round to even
                    break;
#endif
                case MidpointRounding.AwayFromZero:
                    // The C code's "nearest" branch rounds up whenever the
                    // round bit is set, which in sign-magnitude corresponds to
                    // round-half-away-from-zero.  (Equivalently: choose
                    // ToPositiveInfinity for positive results and
                    // ToNegativeInfinity for negative results on ties; here we
                    // simply use the round bit for all nearest cases.)
                    rnd = rbit;
                    break;
                case MidpointRounding.ToZero:
                    rnd = 0;
                    break;
                case MidpointRounding.ToNegativeInfinity:
                    rnd = (UInt64)sign & sbit;
                    break;
                case MidpointRounding.ToPositiveInfinity:
                    rnd = (UInt64)(1 ^ sign) & sbit;
                    break;
                default:
                    throw ThrowNotSupportedException_MidpointRounding(mode);
                }

                // Repack the significand to IEEE layout in-place.
                b0 |= b1 << 48;
                b1 >>= 16;
                v = ((UInt128)b1 << 64) | b0;

                // Add sign + biased exponent field, plus the rounding bit.
                UInt64 e3Field = ((UInt64)e3 << 48) | ((UInt64)sign << 63);
                UInt128 dv = ((UInt128)e3Field << 64) | rnd;
                v += dv;

                result_hi = (UInt64)(v >> 64);
                return (UInt64)v;
            }
        }
    }
}