using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Misc = UltimateOrb.Miscellaneous;
using UltimateOrb.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb.Numerics {

    public static partial class Binary128Arithmetic {
        // ═══════════════════════════════════════════════════════════════════
        // Public API
        // ═══════════════════════════════════════════════════════════════════

        public static UInt64 Log(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Log(lo, hi, MidpointRounding.ToEven, out result_hi);

        public static UInt64 Log(UInt64 lo, UInt64 hi, MidpointRounding rounding, out UInt64 result_hi) {
            unchecked {
                // GAP: _MM_ROUND_* → MidpointRounding.  AwayFromZero is decided at the
                // rounding step where the result's sign is known.
                bool isNearest = rounding == MidpointRounding.ToEven;
                bool isUp = rounding == MidpointRounding.ToPositiveInfinity;
                bool isDown = rounding == MidpointRounding.ToNegativeInfinity;
                bool isAway = rounding == MidpointRounding.AwayFromZero;

                // ── Specials ──────────────────────────────────────────────────
                if (Misc.Unlikely(hi >= ((UInt64)0x7FFF << 48))) {
                    UInt64 b1 = hi & 0x7FFF_FFFF_FFFF_FFFFUL;
                    if (b1 > ((UInt64)0x7FFF << 48) ||
                        (b1 == ((UInt64)0x7FFF << 48) && lo != 0)) {
                        if ((b1 & (1UL << 47)) == 0)
                            SetFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        result_hi = hi | (1UL << 47);
                        return lo;
                    }
                    if (hi == ((UInt64)0x7FFF << 48) && lo == 0) {
                        result_hi = hi;
                        return lo;
                    }
                    if (b1 == 0 && lo == 0) {
                        SetFlagsDummy(FloatingPointExceptionFlags.DivideByZero);
                        result_hi = 0xFFFFUL << 48;
                        return 0;
                    }
                    SetFlagsDummy(FloatingPointExceptionFlags.Invalid);
                    result_hi = 0xFFFF8UL << 44;
                    return 11;
                }

                // ── Near one ──────────────────────────────────────────────────
                if ((UInt64)0x3FFE_FFFF_F000_0040UL <= hi &&
                    hi <= 0x3FFF_0000_0800_0020UL) {
                    return LogNearOne(lo, hi, isNearest, isUp, isDown, isAway, out result_hi);
                }

                // ── Main path (cr_logq) ───────────────────────────────────────
                UInt64 mLo = lo, mHi = hi;
                UInt64 e = hi & ((UInt64)0xFFFF << 48);

                if (Misc.Unlikely(e == 0)) {
                    if (mLo == 0 && mHi == 0) {
                        // +0 (the −0 case is already handled by the specials guard)
                        SetFlagsDummy(FloatingPointExceptionFlags.DivideByZero);
                        result_hi = 0xFFFFUL << 48;
                        return 0;
                    }
                    int nz0 = mHi != 0
                        ? (int)UInt64.LeadingZeroCount(mHi)
                        : (int)UInt64.LeadingZeroCount(mLo) + 64;
                    UInt128 sh = (((UInt128)mHi << 64) | mLo) << (nz0 - 15);
                    mLo = (UInt64)sh; mHi = (UInt64)(sh >> 64);
                    e -= (UInt64)((UInt64)nz0 - 16UL) << 48;
                }
                e += (UInt64)112 << 48;

                UInt64 l = Log2Mantissa(mHi);
                int j0 = (int)(l >> 59);
                int j1 = (int)((l >> 54) & 31);
                int j2 = (int)((l >> 49) & 31);
                int j3 = (int)((l >> 44) & 31);

                mHi = (mHi & 0x0000_FFFF_FFFF_FFFFUL) | ((UInt64)1 << 48);

                UInt64 r0 = LogRt0[j0], r1 = LogRt1[j1];
                UInt64 r2 = LogRt2[j2], r3 = LogRt3[j3];

                // FIX L1: r0..r3 are u64 in the C, so r1*r0 is a 64-bit multiply.
                // The previous revision truncated to 32 bits, which zeroed the
                // identity case (all j = 0).
                UInt64 r01 = r0 * r1;
                UInt64 r23 = r2 * r3;

                UInt128 mVal = ((UInt128)mHi << 64) | mLo;
                // (u128)r01 * r23 : promote both to u128 before multiply.
                UInt128 rh = BigMulFull(mVal, (UInt128)r01 * (UInt128)r23, out UInt128 rl);
                UInt64 rhh = (UInt64)(rh >> 64), rhl = (UInt64)rh;
                UInt64 rlh = (UInt64)(rl >> 64);
                mHi = (rhh << 40) | (rhl >> 24);
                mLo = (rhl << 40) | (rlh >> 24);
                mVal = ((UInt128)mHi << 64) | mLo; // refresh mVal to post-reduction m

                // ── fs = e·ln2a + offa;  += lt0..lt3 ──────────────────────────
                InlineArray3<UInt64> ln2a = default, offa = default, fs = default;
                for (int j = 0; j < 3; j++) { ln2a[j] = LogLn2A[j]; offa[j] = LogOffA[j]; }


                MultiplyAddHigh(ref fs, e, in ln2a, in offa);

                Console.WriteLine($"right after MultiplyAddHigh(ref fs, e, ...): fs {fs[0]:X16}  {fs[1]:X16}  {fs[2]:X16}");



                InlineArray3<UInt64> tmp3 = default;
                for (int j = 0; j < 3; j++) tmp3[j] = LogLt0A[j0 * 3 + j];
                Add(ref fs, in fs, in tmp3);
                for (int j = 0; j < 3; j++) tmp3[j] = LogLt1A[j1 * 3 + j];
                Add(ref fs, in fs, in tmp3);
                for (int j = 0; j < 3; j++) tmp3[j] = LogLt2A[j2 * 3 + j];
                Add(ref fs, in fs, in tmp3);
                for (int j = 0; j < 3; j++) tmp3[j] = LogLt3A[j3 * 3 + j];
                Add(ref fs, in fs, in tmp3);

                // ── Corrections ───────────────────────────────────────────────
                // f = c[3] − mhuu(m.b1, c[4] − mhuu(m.b1, c[5]))   (64-bit chain)
                // then f = c[i] − mhUU(m.a, f) for i = 2,1,0  (128-bit chain)
                UInt64 t5 = MultiplyHigh(mHi, LogC[5 * 2 + 0]);
                UInt64 t4 = LogC[4 * 2 + 0] - t5;
                UInt64 t3lo = LogC[3 * 2 + 0] - MultiplyHigh(mHi, t4);
                UInt128 f = ((UInt128)LogC[3 * 2 + 1] << 64) | t3lo;

                for (int i = 2; i >= 0; i--) {
                    UInt128 ci = ((UInt128)LogC[i * 2 + 1] << 64) | LogC[i * 2 + 0];
                    f = ci - MultiplyHighApproximate(mVal, f);
                }
                UInt128 resA = MultiplyHighApproximate(mVal, f);

                InlineArray3<UInt64> d3 = default;
                d3[0] = (UInt64)resA;
                d3[1] = (UInt64)(resA >> 64);
                d3[2] = 0;
                Add(ref fs, in fs, in d3);

                // ── Normalize ─────────────────────────────────────────────────
                Int64 msk = unchecked((Int64)fs[2]) >> 63;
                UInt64 mskU = unchecked((UInt64)msk);
                fs[0] ^= mskU; fs[1] ^= mskU; fs[2] ^= mskU;


                Console.WriteLine($"right before int nz = fs[2] != 0 ? ... : fs {fs[0]:X16}  {fs[1]:X16}  {fs[2]:X16}");

                int nz = fs[2] != 0
                    ? (int)UInt64.LeadingZeroCount(fs[2])
                    : (int)UInt64.LeadingZeroCount(fs[1]) + 64;
                int ns = nz - 15;

                UInt64 t = fs[0];
                UInt64 tm = ~0UL >> ns;
                UInt64 trBit = isNearest ? 1UL : 0UL;
                UInt64 rnd = (fs[0] >> (63 - ns)) & 1;

                Int64 el = unchecked((Int64)(
                    ((UInt64)0x4029 - (UInt64)nz) | (mskU << 15)));

                UInt64 rLo, rHi;
                if (Misc.Unlikely(((t + (trBit << (63 - ns)) + 12) & tm) <= 24)) {
                    int ls = nz - 14, rs = (-ls) & 63;
                    rHi = (fs[2] << ls) | (fs[1] >> rs);
                    rLo = (fs[1] << ls) | (fs[0] >> rs);

                    // FIX L5: the C's `res.a += (fs[0]>>(63-ls))&1` is a 128-bit add.
                    // Must propagate carry from the low word into the high word.
                    UInt64 carryBit;
                    rLo = AddWithCarry(rLo, (fs[0] >> (63 - ls)) & 1, 0, out carryBit);
                    rHi = AddWithCarry(rHi, 0, carryBit, out _);

                    rnd = LogRefine(el, ref rLo, ref rHi, lo, hi);
                } else {
                    rHi = (fs[2] << ns) | (fs[1] >> ((-ns) & 63));
                    rLo = (fs[1] << ns) | (fs[0] >> ((-ns) & 63));
                }

                if (!isNearest) {
                    // C: rnd = (rm==UP)*!msk + (rm==DOWN)*!!msk;
                    if (isAway)       // away from zero: magnitude up, rnd = 1
                        rnd = 1;
                    else if (isUp)    // toward +∞
                        rnd = (msk == 0) ? 1UL : 0UL;
                    else if (isDown)  // toward −∞
                        rnd = (msk != 0) ? 1UL : 0UL;
                    else              // ToZero
                        rnd = 0;
                }

                UInt64 elShifted = unchecked((UInt64)(el << 48));
                UInt128 res = ((UInt128)rHi << 64) | rLo;
                res += ((UInt128)elShifted << 64) | rnd;

                SetFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(res >> 64);
                return (UInt64)res;
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // as_logq_nearone
        // ═══════════════════════════════════════════════════════════════════

        static UInt64 LogNearOne(
            UInt64 xLo, UInt64 xHi,
            bool isNearest, bool isUp, bool isDown, bool isAway,
            out UInt64 result_hi) {
            unchecked {
                const UInt64 OneHi = (UInt64)0x3FFF << 48;

                Int64 neg;
                UInt64 uLo, uHi;
                int e;

                if (xHi >= OneHi) {
                    // x >= 1
                    neg = 0;
                    uLo = xLo;
                    uHi = xHi - OneHi;
                    if (uHi == 0 && uLo == 0) { result_hi = 0; return 0; } // x == 1
                    int nz0 = uHi != 0
                        ? (int)UInt64.LeadingZeroCount(uHi)
                        : (int)UInt64.LeadingZeroCount(uLo) + 64;
                    UInt128 sh = (((UInt128)uHi << 64) | uLo) << nz0;
                    uLo = (UInt64)sh; uHi = (UInt64)(sh >> 64);
                    e = nz0 - 16;
                } else {
                    // x < 1 (but inside the near-one window → x > 0)
                    neg = 1;
                    UInt64 borrow;
                    UInt64 dLo = SubtractWithBorrow(0, xLo, 0, out borrow);
                    UInt64 dHi = SubtractWithBorrow(OneHi, xHi, borrow, out _);
                    uLo = dLo; uHi = dHi;
                    int nz0 = uHi != 0
                        ? (int)UInt64.LeadingZeroCount(uHi)
                        : (int)UInt64.LeadingZeroCount(uLo) + 64;
                    UInt128 sh = (((UInt128)uHi << 64) | uLo) << nz0;
                    uLo = (UInt64)sh; uHi = (UInt64)(sh >> 64);
                    e = nz0 - 16;
                }

                UInt128 uVal = ((UInt128)uHi << 64) | uLo;
                UInt128 u2 = MultiplyHighApproximate(uVal, uVal);

                // m.a >>= e-20  →  m = u · 2^(20−e)
                int mShift = e - 20;
                UInt128 mVal = mShift >= 0 ? (uVal >> mShift) : (uVal << (-mShift));
                UInt64 mB1 = (UInt64)(mVal >> 64);

                // ── polynomial f ──────────────────────────────────────────────
                UInt128 f;
                if (neg == 0) {
                    // cp chain: 64-bit mhuu inner, mhUU outer.
                    //   f = (cp[3].b1<<64) | (cp[3].b0 − mhuu(m.b1, cp[4].b0 − mhuu(m.b1, cp[5].b0)))
                    //   i=3; while(--i>=0) f = cp[i].a − mhUU(m.a, f);   → i=2,1,0
                    //   m.a = mhUU(u2, f)
                    UInt64 t5n = MultiplyHigh(mB1, LogCp[5 * 2 + 0]);
                    UInt64 t4n = LogCp[4 * 2 + 0] - t5n;
                    UInt64 t3n = LogCp[3 * 2 + 0] - MultiplyHigh(mB1, t4n);
                    f = ((UInt128)LogCp[3 * 2 + 1] << 64) | t3n;
                    for (int i = 2; i >= 0; i--) {
                        UInt128 ci = ((UInt128)LogCp[i * 2 + 1] << 64) | LogCp[i * 2 + 0];
                        f = ci - MultiplyHighApproximate(mVal, f);
                    }
                    f = MultiplyHighApproximate(u2, f);
                } else {
                    // FIX L2: the C's `i=2; while(--i>=0)` runs for i=1,0 — TWO
                    // iterations, not three.  The prior revision used i=2,1,0.
                    // Also: inner chain is a literal u64 mhuu chain against m.b1.
                    UInt64 inner = MultiplyHigh(mB1, LogCn[5 * 2 + 0]);
                    inner += LogCn[4 * 2 + 0];
                    inner = MultiplyHigh(mB1, inner);
                    inner += LogCn[3 * 2 + 0];
                    UInt128 f0 = ((UInt128)LogCn[2 * 2 + 1] << 64) | LogCn[2 * 2 + 0];
                    f0 += (UInt128)MultiplyHigh(mB1, inner);
                    for (int i = 1; i >= 0; i--)   // ← FIX L2
                    {
                        UInt128 ci = ((UInt128)LogCn[i * 2 + 1] << 64) | LogCn[i * 2 + 0];
                        f0 = ci + MultiplyHighApproximate(mVal, f0);
                    }
                    f = MultiplyHighApproximate(u2, f0);
                }

                // ── z, t ──────────────────────────────────────────────────────
                InlineArray3<UInt64> z = default, tArr = default;
                z[0] = 0; z[1] = uLo; z[2] = uHi;
                tArr[0] = 0; tArr[1] = (UInt64)mVal; tArr[2] = (UInt64)(mVal >> 64);

                if (neg == 0) {
                    Rlshft3(ref tArr, e);
                    Subtract(ref z, in z, in tArr);
                } else {
                    UInt128 uHalf = uVal >> 1;
                    z[1] = (UInt64)uHalf;
                    z[2] = (UInt64)(uHalf >> 64);
                    Rlshft3(ref tArr, e + 2);
                    Add(ref z, in z, in tArr);
                }

                // ── rounding test ─────────────────────────────────────────────
                UInt64 eps = 12;
                UInt64 signZ2 = z[2] >> 63;
                UInt128 tl = ((UInt128)z[1] << 64) | z[0];
                UInt128 mskR = UInt128.MaxValue;
                UInt128 nrnd = (UInt128)(isNearest ? 1UL : 0UL) << (int)(13 + signZ2);
                mskR >>= (int)(50 - signZ2);

                bool crnd;
                if (neg == 0) {
                    if (e < 64) {
                        tl >>= 64 - e;
                        mskR >>= 64 - e;
                        nrnd = (nrnd << e) | eps;
                    } else {
                        eps = 1 + (eps >> (e - 64));
                        nrnd = (nrnd << 64) | eps;
                    }
                    tl = mskR & (tl + nrnd);
                    crnd = tl <= 2 * eps;
                } else {
                    if (e <= 62) {
                        tl >>= 62 - e;
                        mskR >>= 62 - e;
                        nrnd = (nrnd << e) | eps;
                    } else {
                        eps = 1 + (eps >> (e - 62));
                        nrnd = (nrnd << 62) | eps;
                    }
                    tl = mskR & (tl + nrnd);
                    crnd = tl <= 2 * eps;
                }

                // ── normalize ─────────────────────────────────────────────────
                UInt64 rHi = z[2], rLo = z[1];
                UInt128 res = ((UInt128)rHi << 64) | rLo;

                if ((rHi >> 63) == 0) {
                    res <<= 1;
                    e++;
                }

                // FIX L4: C is `(res.b[0] >> 14) & 1` — bit 14 of the LOW word.
                // The prior revision masked with 0x3FFF BEFORE shifting, zeroing it.
                UInt64 rnd = ((UInt64)res >> 14) & 1;

                e = 16381 - e;

                if (crnd) {
                    res = (res + ((UInt128)1 << 13)) >> 14;
                    rLo = (UInt64)res;
                    rHi = (UInt64)(res >> 64);
                    Int64 el = unchecked((Int64)((UInt64)e | ((UInt64)neg << 15)));
                    rnd = LogRefine(el, ref rLo, ref rHi, xLo, xHi);
                    res = ((UInt128)rHi << 64) | rLo;
                } else {
                    res >>= 15;
                }

                if (!isNearest) {
                    if (isAway)       // away from zero = magnitude up
                        rnd = 1;
                    else if (isUp)
                        rnd = neg == 0 ? 1UL : 0UL;
                    else if (isDown)
                        rnd = neg != 0 ? 1UL : 0UL;
                    else
                        rnd = 0;
                }

                UInt64 expField = (UInt64)e << 48;
                UInt64 signField = (UInt64)neg << 63;
                res += ((UInt128)(expField | signField) << 64) | rnd;

                SetFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(res >> 64);
                return (UInt64)res;
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // as_logq_refine
        // ═══════════════════════════════════════════════════════════════════

        static UInt64 LogRefine(Int64 el, ref UInt64 mLo, ref UInt64 mHi,
                                UInt64 xLo, UInt64 xHi) {
            unchecked {
                Int64 sm = -((el >> 15) & 1);
                UInt64 smU = unchecked((UInt64)sm);

                UInt128 mVal = ((UInt128)mHi << 64) | mLo;

                // mhu7xu2(x, iln2, m)
                InlineArray7<UInt64> x = default;
                MultiplyHigh(ref x, in ExpIlN2_7, mVal);

                x[0] ^= smU; x[1] ^= smU; x[2] ^= smU; x[3] ^= smU;
                x[4] ^= smU; x[5] ^= smU; x[6] ^= smU;

                int shift = unchecked((int)(0x402E - (el & 0x7FFF)));
                ShiftRightArithmetic(ref x, shift);

                int jt = (int)(x[5] >> 60);
                x[5] &= 0x0FFF_FFFF_FFFF_FFFFUL;

                InlineArray6<UInt64> f = default, f1 = default, f2 = default, ft = default;

                EvalPoly6(ref f, x[5], 36, LogC36Flat);
                EvalPoly6Reduced(ref f1, x[4], LogC36Flat);
                InlineArray6<UInt64> x6 = default;
                for (int j = 0; j < 6; j++) x6[j] = x[j];
                EvalPoly6ReducedReduced(ref f2, ref x6, LogC36Flat);

                MultiplyHigh(ref ft, in f, in f1);
                MultiplyHigh(ref ft, in ft, in f2);

                InlineArray6<UInt64> tblRow = default;
                for (int j = 0; j < 6; j++) tblRow[j] = LogTbl6Flat[jt * 6 + j];
                MultiplyHigh(ref ft, in ft, in tblRow);

                UInt64 sticky = (ft[0] | ft[1] | ft[2] | ft[3]) != 0 ? 1UL : 0UL;
                ft[4] |= sticky;

                Int64 d = unchecked((Int64)(ft[4] - (xLo << 12)));
                d = d >= 0 ? 1 : -1;
                if (sm != 0) d = -d;

                // FIX L3: C promotes d (i64) to u128 by sign extension before
                // subtracting from M.  The prior revision zero-extended, which
                // turned `M − (−1)` into `M − (2^64 − 1)` instead of `M + 1`.
                UInt128 dExt = unchecked((UInt128)(Int128)d);

                UInt128 M = ((UInt128)mHi << 64) | mLo;
                M = (M << 1) - dExt;

                mLo = (UInt64)(M >> 2);
                mHi = (UInt64)(M >> 66);
                return (UInt64)((M >> 1) & 1);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // as_log2  (crude log2 approximation)
        // ═══════════════════════════════════════════════════════════════════

        static UInt64 Log2Mantissa(UInt64 x) {
            unchecked {
                int j = (int)((x >> 43) & 31);
                UInt32 z = (UInt32)(x >> 11);
                UInt32 z2 = (UInt32)(((UInt64)z * (UInt64)z) >> 32);

                UInt32 t0 = LogTbl32[j * 4 + 0]
                          - (UInt32)(((UInt64)z * LogTbl32[j * 4 + 1]) >> 38);
                UInt32 t2 = LogTbl32[j * 4 + 2]
                          - (UInt32)(((UInt64)z * LogTbl32[j * 4 + 3]) >> 32);
                t0 = (UInt32)(t0 + (UInt32)(((UInt64)z2 * t2) >> 44));

                return LogC0[j] + (((UInt64)z * t0) >> 4);
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // Local helpers
        // ═══════════════════════════════════════════════════════════════════

        // mUUp : 128×128 → (lo128, hi128)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt128 BigMulFull(UInt128 a, UInt128 b, out UInt128 lo) {
            unchecked {
                UInt64 a0 = (UInt64)a, a1 = (UInt64)(a >> 64);
                UInt64 b0 = (UInt64)b, b1 = (UInt64)(b >> 64);
                UInt128 a0b0 = (UInt128)a0 * b0;
                UInt128 a0b1 = (UInt128)a0 * b1;
                UInt128 a1b0 = (UInt128)a1 * b0;
                UInt128 a1b1 = (UInt128)a1 * b1;

                UInt64 word0 = (UInt64)a0b0;
                UInt64 word1 = (UInt64)(a0b0 >> 64);
                UInt64 word2 = (UInt64)a1b1;
                UInt64 word3 = (UInt64)(a1b1 >> 64);

                UInt64 c;
                word1 = AddWithCarry(word1, (UInt64)a1b0, 0, out c);
                word2 = AddWithCarry(word2, (UInt64)(a1b0 >> 64), c, out c);
                word3 = AddWithCarry(word3, 0, c, out c);

                word1 = AddWithCarry(word1, (UInt64)a0b1, 0, out c);
                word2 = AddWithCarry(word2, (UInt64)(a0b1 >> 64), c, out c);
                word3 = AddWithCarry(word3, 0, c, out _);

                lo = ((UInt128)word1 << 64) | word0;
                return ((UInt128)word3 << 64) | word2;
            }
        }

        // rlshft (C) : logical right shift of a 3-word value by s ≥ 0.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Rlshft3(ref InlineArray3<UInt64> t, int s) {
            unchecked {
                int rs = s & 63;
                if (rs != 0) {
                    int ls = (-s) & 63;
                    t[0] = (t[0] >> rs) | (t[1] << ls);
                    t[1] = (t[1] >> rs) | (t[2] << ls);
                    t[2] = (t[2] >> rs);
                }
                while (s >= 64) {
                    t[0] = t[1];
                    t[1] = t[2];
                    t[2] = 0;
                    s -= 64;
                }
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // Data tables — transcribed verbatim from the C reference
        // ═══════════════════════════════════════════════════════════════════
        static ReadOnlySpan<UInt64> LogLn2A => [
            0x3F2F6AF40F343267UL, 0x1CF79ABC9E3B3980UL, 0x0000000B17217F7DUL
            ];

        static ReadOnlySpan<UInt64> LogOffA => [
            0x0198BFB39C605BC5UL, 0x718289F4C0A895F2UL, 0xFFFFFFFD3568989AUL
        ];

        static ReadOnlySpan<UInt32> LogRt0 => [
            0x80000000u, 0x7D41D96Eu, 0x7A92BE8Bu, 0x77F25CCEu,
        0x75606374u, 0x72DC8374u, 0x70666F77u, 0x6DFDDBCCu,
        0x6BA27E66u, 0x69540EC9u, 0x6712460Bu, 0x64DCDEC4u,
        0x62B39509u, 0x60962666u, 0x5E8451D0u, 0x5C7DD7A4u,
        0x5A82799Au, 0x5891FAC1u, 0x56AC1F76u, 0x54D0AD5Bu,
        0x52FF6B55u, 0x51382182u, 0x4F7A9931u, 0x4DC69CDDu,
        0x4C1BF829u, 0x4A7A77D5u, 0x48E1E9BAu, 0x47521CC6u,
        0x45CAE0F2u, 0x444C0741u, 0x42D561B4u, 0x4166C34Du
        ];
        static ReadOnlySpan<UInt32> LogRt1 => [
            0x80000000u, 0x7FE9D3A9u, 0x7FD3AB2Au, 0x7FBD8680u,
        0x7FA765ADu, 0x7F9148AFu, 0x7F7B2F86u, 0x7F651A31u,
        0x7F4F08AFu, 0x7F38FAFFu, 0x7F22F122u, 0x7F0CEB16u,
        0x7EF6E8DBu, 0x7EE0EA6Fu, 0x7ECAEFD4u, 0x7EB4F906u,
        0x7E9F0607u, 0x7E8916D5u, 0x7E732B70u, 0x7E5D43D7u,
        0x7E476009u, 0x7E318006u, 0x7E1BA3CDu, 0x7E05CB5Eu,
        0x7DEFF6B7u, 0x7DDA25D8u, 0x7DC458C1u, 0x7DAE8F71u,
        0x7D98C9E7u, 0x7D830822u, 0x7D6D4A22u, 0x7D578FE6u
        ];
        static ReadOnlySpan<UInt32> LogRt2 => [
            0x80000000u, 0x7FFF4E8Fu, 0x7FFE9D1Eu, 0x7FFDEBAFu,
        0x7FFD3A40u, 0x7FFC88D2u, 0x7FFBD765u, 0x7FFB25F9u,
        0x7FFA748Eu, 0x7FF9C325u, 0x7FF911BCu, 0x7FF86054u,
        0x7FF7AEEDu, 0x7FF6FD86u, 0x7FF64C21u, 0x7FF59ABDu,
        0x7FF4E95Au, 0x7FF437F8u, 0x7FF38696u, 0x7FF2D536u,
        0x7FF223D7u, 0x7FF17278u, 0x7FF0C11Bu, 0x7FF00FBEu,
        0x7FEF5E63u, 0x7FEEAD08u, 0x7FEDFBAFu, 0x7FED4A56u,
        0x7FEC98FEu, 0x7FEBE7A8u, 0x7FEB3652u, 0x7FEA84FDu
        ];
        static ReadOnlySpan<UInt32> LogRt3 => [
            0x80000000u, 0x7FFFFA75u, 0x7FFFF4E9u, 0x7FFFEF5Eu,
        0x7FFFE9D2u, 0x7FFFE447u, 0x7FFFDEBBu, 0x7FFFD930u,
        0x7FFFD3A4u, 0x7FFFCE18u, 0x7FFFC88Du, 0x7FFFC301u,
        0x7FFFBD76u, 0x7FFFB7EAu, 0x7FFFB25Fu, 0x7FFFACD3u,
        0x7FFFA748u, 0x7FFFA1BCu, 0x7FFF9C30u, 0x7FFF96A5u,
        0x7FFF9119u, 0x7FFF8B8Eu, 0x7FFF8602u, 0x7FFF8077u,
        0x7FFF7AEBu, 0x7FFF7560u, 0x7FFF6FD4u, 0x7FFF6A49u,
        0x7FFF64BDu, 0x7FFF5F31u, 0x7FFF59A6u, 0x7FFF541Au
        ];

        static ReadOnlySpan<UInt64> LogLt0A => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0xA89586EFCF459616UL, 0x0BF2D9D6938E7957UL, 0x00000000000058B9UL,
        0x1C4217AC03D7F347UL, 0x17E97EC3BBBE7FA5UL, 0x000000000000B172UL,
        0xEFB93FF266057747UL, 0x23F162A58989F516UL, 0x0000000000010A2BUL,
        0xE785FEE03979770CUL, 0x2FED4367CAB6F34CUL, 0x00000000000162E4UL,
        0xB7178705EB425D19UL, 0x3BE26415CB5FFCA2UL, 0x000000000001BB9DUL,
        0x99D386FE1E67FE52UL, 0x47C60C5651FC6826UL, 0x0000000000021456UL,
        0xDC33CA27AE76C0E3UL, 0x53E0AC68AF0F8D45UL, 0x0000000000026D0FUL,
        0xB920987460CCAB06UL, 0x5FC92CCD6BB8F0A7UL, 0x000000000002C5C8UL,
        0x1668340EE9FE5A93UL, 0x6BDA0FBED4C7F913UL, 0x0000000000031E81UL,
        0x08EA18591547F82BUL, 0x77C5ACA6064AC9D5UL, 0x000000000003773AUL,
        0x10BE2CE034FF1E0BUL, 0x83B323296E47FD8AUL, 0x000000000003CFF3UL,
        0xE27D52AE22538CBBUL, 0x8FC10F43EA11902BUL, 0x00000000000428ACUL,
        0x6D1339A150080F6BUL, 0x9BA8E78A4A916975UL, 0x0000000000048165UL,
        0x6CB99B86DEFF0B66UL, 0xA7B88577D11699BEUL, 0x000000000004DA1EUL,
        0x88B410857D77A113UL, 0xB3B510D166B0BFCDUL, 0x00000000000532D7UL,
        0x9328234A71B9E39EUL, 0xBFBE03BFA45E53D0UL, 0x0000000000058B90UL,
        0xA9113370E6C95819UL, 0xCBB65F48E215A1BEUL, 0x000000000005E449UL,
        0xE92C7C696E7F112CUL, 0xD78D448CDA05AA76UL, 0x0000000000063D02UL,
        0xC391AA17BB2E306DUL, 0xE3981C232F9C13AFUL, 0x00000000000695BBUL,
        0x938A7D8CFFA8155EUL, 0xEFA69C9432396C18UL, 0x000000000006EE74UL,
        0xF2A451564D1C4467UL, 0xFB92199C68D6317EUL, 0x000000000007472DUL,
        0x298ED4E3413DEFD5UL, 0x07812354074DF8F7UL, 0x0000000000079FE7UL,
        0xB9020C95072A113FUL, 0x139D8899037598BFUL, 0x000000000007F8A0UL,
        0xFAC4D6A530C11DCBUL, 0x1F91D2A304280C6DUL, 0x0000000000085159UL,
        0x361EBA5F7B210F8EUL, 0x2B7E22B48DC9C3ACUL, 0x000000000008AA12UL,
        0x067B013198BFF7BBUL, 0x378C54FB8DF946F4UL, 0x00000000000902CBUL,
        0x668E61761F70153BUL, 0x437CADB3F378D692UL, 0x0000000000095B84UL,
        0xA7E6655EA0F729E3UL, 0x4F8B03CDFC0665F2UL, 0x000000000009B43DUL,
        0x67731AA1F81E8841UL, 0x5B5E9CB8EA72447EUL, 0x00000000000A0CF6UL,
        0x1A7B7C5AEF8482A9UL, 0x677F1A61810B457CUL, 0x00000000000A65AFUL,
        0xA5E22A09ADE4180EUL, 0x7357A288FC7E3DE4UL, 0x00000000000ABE68UL
        ];

        static ReadOnlySpan<UInt64> LogLt1A => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0xFCD4886923DAE6B6UL, 0xC85BEE167CAB86CEUL, 0x00000000000002C5UL,
        0x635E6B704ED2F5FAUL, 0x90A17CEECC3FE577UL, 0x000000000000058BUL,
        0x0DF932DA91371DD2UL, 0x591B7D0A4D0016A7UL, 0x0000000000000851UL,
        0x73159A852516C020UL, 0x2174877366AB0CECUL, 0x0000000000000B17UL,
        0x6DAF0FEE571D4B99UL, 0xE9D76CBBDDB12B9DUL, 0x0000000000000DDCUL,
        0xC537F3BCCF1BCDDaUL, 0xB22EDC3896D8E511UL, 0x00000000000010A2UL,
        0x7ACD5F3C275334BCUL, 0x7A85A1047F10C872UL, 0x0000000000001368UL,
        0xCF11C6053E5E8A85UL, 0x42E69173390188DCUL, 0x000000000000162EUL,
        0x5263D7113FF4EC99UL, 0x0B5C8F0CFA3BCD0DUL, 0x00000000000018F4UL,
        0xB72B8544C29A9CCAUL, 0xD3B2174EBD216DAAUL, 0x0000000000001BB9UL,
        0xC46FC691949A5AABUL, 0x9C123DF6AAF7611BUL, 0x0000000000001E7FUL,
        0xC10A731729FCD789UL, 0x6467B2B9AEA50BE5UL, 0x0000000000002145UL,
        0x893FA461CBBD9064UL, 0x2CDDAB05D780030BUL, 0x000000000000240BUL,
        0x34C38552CB1A0B96UL, 0xF51E45F22E4B220EUL, 0x00000000000026D0UL,
        0x70F1813D88D0193BUL, 0xBD95605269B91143UL, 0x0000000000002996UL,
        0x73E5B8717251D0F7UL, 0x85ED0E45F6C8616DUL, 0x0000000000002C5CUL,
        0x7F5AE5D5B9581561UL, 0x4E50A741A891EEB4UL, 0x0000000000002F22UL,
        0x56594D2D9101BAD3UL, 0x16AADBA73393C1A7UL, 0x00000000000031E8UL,
        0xEF4A8EB0C15FBF7AUL, 0xDF06BACC0921D568UL, 0x00000000000034ADUL,
        0x5D054864B92A7751UL, 0xA76F5F65463C79F7UL, 0x0000000000003773UL,
        0x7A5575CF1DF04DF6UL, 0x6FCF7A45D5191939UL, 0x0000000000003A39UL,
        0x9618AD49BB912343UL, 0x38322C12B8541A4AUL, 0x0000000000003CFFUL,
        0x2EDCE206B7063305UL, 0x00822052FAB58FD5UL, 0x0000000000003FC5UL,
        0x2E80337B06ED4E80UL, 0xC8EB03CD67C0701CUL, 0x000000000000428AUL,
        0xD46EA81C5E577187UL, 0x91578DA94328A582UL, 0x0000000000004550UL,
        0x8705F6EFE46E502BUL, 0x59B269F9F7D32471UL, 0x0000000000004816UL,
        0xFBDB66EBECF00A88UL, 0x2206D0D3E982FCD2UL, 0x0000000000004ADCUL,
        0xC819E2A66EFB2206UL, 0xEA6005C8E6DA3826UL, 0x0000000000004DA1UL,
        0xED64886CDF895904UL, 0xB2C957EE1AAC3666UL, 0x0000000000005067UL,
        0xB1F969DAE7EDC113UL, 0x7B2D79D3AFECBFBEUL, 0x000000000000532DUL,
        0x1B3B44759B966886UL, 0x4397C0ABDEA76DC5UL, 0x00000000000055F3UL
        ];

        static ReadOnlySpan<UInt64> LogLt2A => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0x9CF3BC3D208C9185UL, 0x2E2F5FBCB16FF027UL, 0x0000000000000016UL,
        0xDA087301FF6274D4UL, 0x5C7D7F2B9B75AFE0UL, 0x000000000000002CUL,
        0x134A54F71A0236C3UL, 0x8AAA5D97D2591B8FUL, 0x0000000000000042UL,
        0xD9C68A61183CE86AUL, 0xB8F5FBAF4FB0944AUL, 0x0000000000000058UL,
        0x9C822A201EB1FC42UL, 0xE74058E9835896C7UL, 0x000000000000006EUL,
        0x5441BB438A9FA9A5UL, 0x1589751695C42B17UL, 0x0000000000000085UL,
        0xBD2D2405A0DF752AUL, 0x43D15006AEA6E5E2UL, 0x000000000000009BUL,
        0xE419CB9011950B79UL, 0x7217E989F4F4E681UL, 0x00000000000000B1UL,
        0x534A67BFDF41CFA2UL, 0xA03D3FE1448D8184UL, 0x00000000000000C7UL,
        0x51F2181BF5CF5458UL, 0xCE8155CEF8C016E6UL, 0x00000000000000DDUL,
        0x2DB9191EC9FC2EB3UL, 0xFCC429C04A827E7EUL, 0x00000000000000F3UL,
        0xCD759D3F967FC6F7UL, 0x2B05BB855DCAEF6BUL, 0x000000000000010AUL,
        0x95EF9CBD1C7B13C7UL, 0x59660D2F1CC94616UL, 0x0000000000000120UL,
        0x2AF332CA6CC1B350UL, 0x87A51A387BBEE256UL, 0x0000000000000136UL,
        0x7948F090BABD87B8UL, 0xB5E2E48603DB2B0EUL, 0x000000000000014CUL,
        0xC53979D14662EF59UL, 0xE41F6BE7D6165086UL, 0x0000000000000162UL,
        0xEABBA1758308D7B9UL, 0x125AB02E12A8FC92UL, 0x0000000000000179UL,
        0xBECF1149DA50DE5EUL, 0x40B4B4478141A2F2UL, 0x000000000000018FUL,
        0x477F919CF0A27011UL, 0x6EED71F351129EA7UL, 0x00000000000001A5UL,
        0xBA7E797E9EB461B6UL, 0x9D24EBF3E7A2E2A7UL, 0x00000000000001BBUL,
        0x3A5E7941BEF2E816UL, 0xCB7B25BD2DFD7F94UL, 0x00000000000001D1UL,
        0xC9B5422A00CEABBFUL, 0xF9B018040A909DA4UL, 0x00000000000001E7UL,
        0xCADE12D9D3E36920UL, 0x2803CA0C93032D7EUL, 0x00000000000001FEUL,
        0x222F102EB784552BUL, 0x563633DA2572876CUL, 0x0000000000000214UL,
        0x75D47798F62FF1C7UL, 0x84875D625DC415F0UL, 0x000000000000022AUL,
        0x63E729D32C4F6E9EUL, 0xB2B73DF70FEC66B5UL, 0x0000000000000240UL,
        0x7B5AFEEBC54C49ABUL, 0xE105DE3F5FE748EBUL, 0x0000000000000256UL,
        0x221CA10AAC8A3B6AUL, 0x0F5339B6125EE838UL, 0x000000000000026DUL,
        0x9E9E828D80C1CF32UL, 0x3D7F4B245F1A3BFFUL, 0x0000000000000283UL,
        0x88AD9AB83FC30A52UL, 0x6BCA1C3BB9EB9390UL, 0x0000000000000299UL,
        0x4D28B6B2476EE8F8UL, 0x9A13A7F1BC8C0D71UL, 0x00000000000002AFUL
        ];

        static ReadOnlySpan<UInt64> LogLt3A => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0x347FB733D023CC27UL, 0xB16003D72F3C6259UL, 0x0000000000000000UL,
        0x8679CA1F8E0297B8UL, 0x62E00F5F83035049UL, 0x0000000000000001UL,
        0x5490AB9896A762F9UL, 0x14402294D37EE9E5UL, 0x0000000000000002UL,
        0x5E57AE0F99D9A1E6UL, 0xC5C03D7E0F9A82C2UL, 0x0000000000000002UL,
        0x78D5D5583FAD6E57UL, 0x7720601183FF07B0UL, 0x0000000000000003UL,
        0xCE2ECE6E0065F25AUL, 0x28A08A5BAB197ADEUL, 0x0000000000000004UL,
        0x17FD57B0C9F4117DUL, 0xDA00BC4D46109FE0UL, 0x0000000000000004UL,
        0x9CDC64425D00B1F6UL, 0x8B80F5F85AD41D75UL, 0x0000000000000005UL,
        0x3AE1D5BE90AD7893UL, 0x3D01375498EC7536UL, 0x0000000000000006UL,
        0x12009A12EB106FA8UL, 0xEE618054241E50C0UL, 0x0000000000000006UL,
        0x1C52307D1547E6A6UL, 0x9FE1D11153DF1B17UL, 0x0000000000000007UL,
        0xF679672EA03B5B71UL, 0x5142296F0C4BFC59UL, 0x0000000000000008UL,
        0xA28C66C32C76DF90UL, 0x02C2898D305F6B18UL, 0x0000000000000009UL,
        0x94DD6B2C7F525610UL, 0xB422F14918B10941UL, 0x0000000000000009UL,
        0x692D440A10A891F7UL, 0x65A360C833C14EEAUL, 0x000000000000000AUL,
        0xB9F6C9A02410801AUL, 0x1703D7E24EA161D7UL, 0x000000000000000BUL,
        0x8E428B3FDD4FF401UL, 0xC88456C26358B19EUL, 0x000000000000000BUL,
        0x798BB05020BCB8CAUL, 0x7A04DD53A7646701UL, 0x000000000000000CUL,
        0xB2C5D2CBF27D3F39UL, 0x2B656B7BC4797FA9UL, 0x000000000000000DUL,
        0xEE8203ED92FD41CFUL, 0xDCE6016E062BAC06UL, 0x000000000000000DUL,
        0xFB1760C891901812UL, 0x8E469EF45C77A6E2UL, 0x000000000000000EUL,
        0xB33A3B423EEAEDD0UL, 0x3FC744479E7A7F29UL, 0x000000000000000FUL,
        0x0F790771B31F0C07UL, 0xF127F12C30A71684UL, 0x000000000000000FUL,
        0xC210DFA087D58623UL, 0xA2A8A5E075A4D054UL, 0x0000000000000010UL,
        0x1C8901C916301142UL, 0x54096223465BBF29UL, 0x0000000000000011UL,
        0x99FC5710AC4E702EUL, 0x058A263890FE90D5UL, 0x0000000000000012UL,
        0xD3BCD06F88DDB2F1UL, 0xB6EAF1D9A2E992D0UL, 0x0000000000000012UL,
        0x3F07C144ADF4E354UL, 0x686BC54FF5DBB35CUL, 0x0000000000000013UL,
        0x288D50E0D51AB78AUL, 0x19ECA0777F77067FUL, 0x0000000000000014UL,
        0x3ACDD4C172D84D84UL, 0xCB4D8326A9902BFBUL, 0x0000000000000014UL,
        0x81F0878A8D99E057UL, 0x7CCE6DAF3F7A4090UL, 0x0000000000000015UL
        ];

        static ReadOnlySpan<UInt64> LogC => [
            0xFFFF_FFFF_FFFF_FFFFUL, 0xFFFF_FFFF_FFFF_FFFFUL,
        0xFFFF_FFFF_FFFF_FFFFUL, 0x0000_07FF_FFFF_FFFFUL,
        0x5555_5555_5555_5551UL, 0x0000_0000_0055_5555UL,
        0xFFFF_FFFF_FFFF_FFEAUL, 0x0000_0000_0000_0003UL,
        0x0000_3333_3333_32F7UL, 0x0000_0000_0000_0000UL,
        0x0000_0000_02AA_AA5EUL, 0x0000_0000_0000_0000UL
        ];

        static ReadOnlySpan<UInt64> LogCp => [
            0xFFFF_FFFF_FFFF_FFFFUL, 0x7FFF_FFFF_FFFF_FFFFUL,
        0x5555_5555_5555_5555UL, 0x0000_0555_5555_5555UL,
        0xFFFF_FFFF_FFFF_FFFDUL, 0x0000_0000_003F_FFFFUL,
        0x3333_3333_3333_3326UL, 0x0000_0000_0000_0003UL,
        0x0000_2AAA_AAAA_A884UL, 0x0000_0000_0000_0000UL,
        0x0000_0000_0249_245AUL, 0x0000_0000_0000_0000UL
        ];
        static ReadOnlySpan<UInt64> LogCn => [
            0x0000_0000_0000_0000UL, 0x8000_0000_0000_0000UL,
        0xAAAA_AAAA_AAAA_AAAAUL, 0x0000_02AA_AAAA_AAAAUL,
        0xFFFF_FFFF_FFFF_FFFFUL, 0x0000_0000_000F_FFFFUL,
        0x6666_6666_6666_6668UL, 0x0000_0000_0000_0000UL,
        0x0000_02AA_AAAA_AAA8UL, 0x0000_0000_0000_0000UL,
        0x0000_0000_0012_4926UL, 0x0000_0000_0000_0000UL
        ];

        static ReadOnlySpan<UInt64> LogC36Flat => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x8000000000000000UL,
        0x2ACAA97DA57CADBEUL, 0xF3DC3B1036F5D64CUL, 0xC5068BADC5D57D15UL, 0xA079A193394C5B16UL, 0xE4F1D9CC01F97B57UL, 0x58B90BFBE8E7BCD5UL,
        0xD344071B5EC47714UL, 0x524EB0376B9686E0UL, 0xC2BE93BBB1396E6EUL, 0xA3A2751C30CE69D4UL, 0x6F16B06EC9735FCAUL, 0x1EBFBDFF82C58EA8UL,
        0x80960837D68B3044UL, 0x44545F2E4DD67D06UL, 0x4753198ADE236146UL, 0xA7AE23A226D00887UL, 0xCCE9D8AECCAF4B7BUL, 0x071AC235C1282FE2UL,
        0xEEE877628D8F6274UL, 0x8F7E8887E73829D7UL, 0x699B709699E1815FUL, 0x72478EA53E63911DUL, 0x9CCBBE0B53EEAC50UL, 0x013B2AB6FBA4E772UL,
        0x318A9F63B2F3EC5CUL, 0x2F040F926E2C93BEUL, 0xC8CDC36AA406093FUL, 0x60AED94D2DCE32E1UL, 0x20E2FED34A297D86UL, 0x002BB0FFCF14CE62UL,
        0x2D3C7B9299D0D3E1UL, 0x91EC7AEC4E3F0A35UL, 0x549592FF6D6AB786UL, 0xCFB314FFCCC47BB0UL, 0xDBD2C2A261AC8D07UL, 0x00050C244BE1B1E1UL,
        0xCD46DC2CD5899528UL, 0x95CE94549C9A636AUL, 0x5D3119327FAC831DUL, 0x4ACE1152E8810FEEUL, 0x1A1AC547321F639AUL, 0x00007FF2FF1622C3UL,
        0x8531A13788143560UL, 0x5C5F54178A891B6BUL, 0x7D5B21CD05968068UL, 0x2586E1A0F7107AB9UL, 0x11FEC7FF3036D3BEUL, 0x00000B160111D2E4UL,
        0xE590C3FDFB446BE7UL, 0x2D5087B376C0214DUL, 0x738AFD2A4917377AUL, 0x84518CB8CAA9BA26UL, 0x3E1ED253872D27FCUL, 0x000000DA929E9CAFUL,
        0xCC782DE6FF640FD0UL, 0x68569686F1264EA6UL, 0x98468B24ACD303BEUL, 0xB4CE2A0608A16570UL, 0xC764FB7ED0ECA973UL, 0x0000000F267A8AC5UL,
        0xEE22A537A6D9A8B8UL, 0x08110963D1D9380FUL, 0x43B147AA9F2C11B5UL, 0x90A4EDC7B3F29C1CUL, 0x8DD92607ABCCAF23UL, 0x00000000F465639AUL,
        0x38202F8BF18F1943UL, 0xBC2FB66E0BF68E76UL, 0x511F7698B91CBA26UL, 0xD17730E76AAEDF16UL, 0x7E14C2F15AB43F0BUL, 0x000000000E1DEB28UL,
        0xEA93B5CEDF528C2FUL, 0x6EE667B44F788F66UL, 0x714AA5175CD8C9ABUL, 0x7B8D8CE6FE81F087UL, 0x8B3687CB140D6180UL, 0x0000000000C0B0C9UL,
        0x88AD1FE5BCEC581BUL, 0xA772E0B581376DF7UL, 0x1D1EDB9F5248282AUL, 0x2AE5761242913279UL, 0x26AC3C54B9F8A1B1UL, 0x0000000000098A4BUL,
        0x9D19FAF9F3A16E65UL, 0xC2FDE1C74321A361UL, 0x3787E55BAD70A464UL, 0xFC6A729280D47C69UL, 0xA10EC1008799EC55UL, 0x00000000000070DBUL,
        0xF708EE5BF7061DEEUL, 0x921CFCDBBCCF2EB0UL, 0x7CD64E11A79215FEUL, 0x93B26377E3B574BDUL, 0xA26B9E7E2CE48E3FUL, 0x00000000000004E3UL,
        0x9BB4D4CCB609D71EUL, 0x6EF9409A4CD3AC24UL, 0xC560A32B4349D4DDUL, 0xB1001EFF03E83F71UL, 0x088968384B4FAAC3UL, 0x0000000000000033UL,
        0xB5C5F81B7E235F48UL, 0x9C1837383FEF6AA4UL, 0xCF80A6C83EA7DC89UL, 0x2ED389743C9AA15CUL, 0xF7176BDB43695D73UL, 0x0000000000000001UL,
        0x4E3F1412BE77B88AUL, 0x75F070DF1338E5DBUL, 0xD970F6D684DCCA91UL, 0xE056AB257AA7F274UL, 0x125A7ECB835C64DAUL, 0x0000000000000000UL,
        0x6C8A502410B3A92BUL, 0x5AE22FF5B3B87C53UL, 0x5510CEF268F5A4F6UL, 0x27126F802BC39D75UL, 0x00A2D6625A8289ACUL, 0x0000000000000000UL,
        0xA851C04A8293FECEUL, 0x95E77DEC82B8EB58UL, 0xEF60EB192FE67EDAUL, 0xE0AADF36DDF86F4FUL, 0x00055FF15E0F8271UL, 0x0000000000000000UL,
        0x7E2F1903476D3F72UL, 0x991510E40164F6ECUL, 0xB93A9CF471520656UL, 0x81D8F91AED9D0512UL, 0x00002B59F5A6E03BUL, 0x0000000000000000UL,
        0xE661B8B0F04BD98DUL, 0xC0C7162D7C443B27UL, 0x48EB10AA6D041426UL, 0xFB5CB2465D33A354UL, 0x0000014E7515DC98UL, 0x0000000000000000UL,
        0xA2DE610B24AFFA88UL, 0x36D51A1C2CF018AEUL, 0xE823660BEE34DF2BUL, 0xD5E52102FA4F3D47UL, 0x00000009A8D57B65UL, 0x0000000000000000UL,
        0x8297D75925CF8A0EUL, 0xB19C48D3A13188F6UL, 0x08EC765F339C0710UL, 0x6A09E41E7A3814F9UL, 0x00000000448FBF65UL, 0x0000000000000000UL,
        0x11744278158DD70DUL, 0xD10B922FB4D1B574UL, 0xDD8263D932A8A20CUL, 0x99F1CC682AF884C5UL, 0x0000000001D3EBC2UL, 0x0000000000000000UL,
        0x20127EE09BB9F25CUL, 0x5ED98D7CDE9ECACEUL, 0x46DA42907892BB39UL, 0x9AF8DBD36A3B0863UL, 0x00000000000C0334UL, 0x0000000000000000UL,
        0xAFD75D37C82B15CAUL, 0xF0ACD37674CF3129UL, 0xAE8A3FB59540AB8DUL, 0xA3DFEB50D4131639UL, 0x0000000000004C20UL, 0x0000000000000000UL,
        0xDAAF1913E1CB12F0UL, 0xE352EA644FB69C62UL, 0xF3FAC0750B414CC5UL, 0xCF69A29F13DE20E8UL, 0x00000000000001D1UL, 0x0000000000000000UL,
        0x0587A7D1A7EC6C45UL, 0x05EACA3704DCD2F6UL, 0x12DCBE8116EADA8FUL, 0xC333445E3155F79BUL, 0x000000000000000AUL, 0x0000000000000000UL,
        0x183F54470927DBCDUL, 0x76BC8F5B8E8B7093UL, 0x8F099FB908474B26UL, 0x3D9AEA5B4D0EBFAEUL, 0x0000000000000000UL, 0x0000000000000000UL,
        0x261C179F4BDFFA1DUL, 0x0909A0681E2103A8UL, 0x4DBDD766D2D9A092UL, 0x01559C86508B1539UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0x3816A7CA0CA85578UL, 0x6B7FD2696989CDEEUL, 0xD34DACABB2D48A6CUL, 0x00072CE49E65A3ACUL, 0x0000000000000000UL, 0x0000000000000000UL,
        0xD4E29D35FE14E115UL, 0xC30D08011BDAFD19UL, 0xA9A6BF19955B2132UL, 0x00002572BE492F62UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0xE4DF41884BE86BB5UL, 0x0C5772EE960FD5BFUL, 0x9DBEE3B9355D1FB0UL, 0x000000BDD01D5D84UL, 0x0000000000000000UL, 0x0000000000000000UL,
        0x80446608D32C4ED3UL, 0xB4D5892FE7709C19UL, 0xD3840403FCEFE8A9UL, 0x00000003BC4FB6D4UL, 0x0000000000000000UL, 0x0000000000000000UL
        ];

        static ReadOnlySpan<UInt64> LogTbl6Flat => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x8000000000000000UL,
        0x82DC9BF3421840D1UL, 0x7835AF9AB4E6355CUL, 0x5D42B362AF1EE859UL, 0x148A0459E7585151UL, 0xC5C95B8C2154C1B2UL, 0x85AAC367CC487B14UL,
        0x9994FAC7F4EA654BUL, 0xB4F28C879A524BD4UL, 0x91E135EE84A3F733UL, 0x1AA84FFBEBAC349FUL, 0xFBE4628758A53C90UL, 0x8B95C1E3EA8BD6E6UL,
        0x23B504FB5DADBB25UL, 0x4E0D990C27C43643UL, 0xF1203CAF65BFB9B9UL, 0x1942B34816FB4F26UL, 0x0FD6D8E0AE5AC9D8UL, 0x91C3D373AB11C336UL,
        0x4E9F94EE0C90DBBDUL, 0xBE47C34D380250A8UL, 0xD78B65CBEFA7BB6FUL, 0x5E139A1B14FA8178UL, 0x46AD23182E42F6F6UL, 0x9837F0518DB8A96FUL,
        0xA264696B6F6E2B1CUL, 0xCD12E5ADA91EEF7BUL, 0x21F977FE7C7FA117UL, 0x65C15C122133E2A2UL, 0xA0911F09EBB9FDD1UL, 0x9EF5326091A111ADUL,
        0xAD35E8E58C5CE4F4UL, 0x5343B4A0330A8052UL, 0x2589C98A8290D3F0UL, 0x1DD170ACE2BCFC17UL, 0x1CBD7F621710701BUL, 0xA5FED6A9B15138EAUL,
        0x739A3D061FEA7592UL, 0x458FD5F44E26A7A7UL, 0xB165F141833A67DAUL, 0x6BE409407034FDEDUL, 0x4980A8C8F59A2EC4UL, 0xAD583EEA42A14AC6UL,
        0xA8B1FE6FDC83DB39UL, 0x4AFC83043AB8A2C3UL, 0xED17AC8583339915UL, 0x1D6F60BA893BA84CUL, 0x597D89B3754ABE9FUL, 0xB504F333F9DE6484UL,
        0x283C66DB56DF97F8UL, 0x9B985F3A0EAE1C1DUL, 0x0D9A4BE023ECE031UL, 0x15B34BBCB0298F41UL, 0xA8811FB66D0FAF7AUL, 0xBD08A39F580C36BEUL,
        0xEFBA190ED1A58A51UL, 0xBB2068BE237512DFUL, 0xC7686006E4E6C092UL, 0x6B0F939998251A36UL, 0x3E2AD0C964DD9F37UL, 0xC5672A115506DADDUL,
        0xCD8CAEBC5D894C26UL, 0x1B8343088BBDADD4UL, 0x2BBD398AF35C079FUL, 0x6F28610B8C36485AUL, 0xE235838F95F2C6EDUL, 0xCE248C151F8480E3UL,
        0x81247458FB61E576UL, 0xEFB01FDA334BCA9AUL, 0xB5C13ADA0E778299UL, 0x1D733AF522058B16UL, 0x39A68BB9902D3FDEUL, 0xD744FCCAD69D6AF4UL,
        0xF44E17262DA49B5DUL, 0x188081FE7062F61EUL, 0x1CB99D3F1FF298A2UL, 0x224B251B33092002UL, 0x065895048DD333CAUL, 0xE0CCDEEC2A94E111UL,
        0x091482854D819E44UL, 0xB338FCD2AC2FFBC8UL, 0x17D8D1E8CA31880AUL, 0xC4FAACE043B7F91CUL, 0xD02D75B3706E54FAUL, 0xEAC0C6E7DD24392EUL,
        0x29F075F23730E918UL, 0xD22DD036F1906094UL, 0xBDD80329364AA29FUL, 0x6F510308677709F5UL, 0x7B9D0C7AED980FC3UL, 0xF5257D152486CC2CUL
        ];

        static ReadOnlySpan<UInt32> LogTbl32 => [
            0xB8AA3AF4u, 0xB8A9CF24u, 0xF5EC11E6u, 0x056CDE26u,
        0xB311AD8Du, 0xADA42C83u, 0xE04081E5u, 0x04CE4AA7u,
        0xADCD64B4u, 0xA393D29Cu, 0xCD0DF3B5u, 0x0445B73Bu,
        0xA8D62754u, 0x9A5D2082u, 0xBBFCA4EDu, 0x03CF93B1u,
        0xA42589CEu, 0x91E83E37u, 0xACC3070Bu, 0x0368F7A9u,
        0x9FB5D233u, 0x8A208126u, 0x9F23232Cu, 0x030F8032u,
        0x9B81E0E3u, 0x82F3ED8Du, 0x92E88678u, 0x02C13522u,
        0x97851CC6u, 0x7C52CE34u, 0x87E698B1u, 0x027C744Du,
        0x93BB6276u, 0x762F5E29u, 0x7DF7458Bu, 0x023FE121u,
        0x9020F5EBu, 0x707D8107u, 0x74F9E730u, 0x020A57BFu,
        0x8CB2762Au, 0x6B32870Bu, 0x6CD26447u, 0x01DAE2AAu,
        0x896CD2ADu, 0x6644FAD1u, 0x656876FFu, 0x01B0B28Cu,
        0x864D4241u, 0x61AC76EFu, 0x5EA714E9u, 0x018B1793u,
        0x83513B19u, 0x5D618220u, 0x587BF145u, 0x01697C1Au,
        0x80766BE7u, 0x595D70C9u, 0x52D714A2u, 0x014B6048u,
        0x7DBAB5DEu, 0x559A4B07u, 0x4DAA85E3u, 0x01305684u,
        0x7B1C276Au, 0x5212B66Cu, 0x48EA016Fu, 0x0118008Bu,
        0x7898F797u, 0x4EC1E2F3u, 0x448ABC05u, 0x01020D05u,
        0x762F8200u, 0x4BA37A8Au, 0x40832F23u, 0x00EE3590u,
        0x73DE4338u, 0x48B392E0u, 0x3CCAED5Bu, 0x00DC3D0Fu,
        0x71A3D59Eu, 0x45EEA114u, 0x395A7D42u, 0x00CBEE53u,
        0x6F7EEE92u, 0x43516F02u, 0x362B39D1u, 0x00BD1AF0u,
        0x6D6E5BEFu, 0x40D911EFu, 0x3337376Au, 0x00AF9A47u,
        0x6B7101D3u, 0x3E82E265u, 0x30792CADu, 0x00A348B4u,
        0x6985D8A7u, 0x3C4C750Au, 0x2DEC5E91u, 0x009806E0u,
        0x67ABEB4Fu, 0x3A33945Du, 0x2B8C8F3Cu, 0x008DB92Cu,
        0x65E25598u, 0x38363B33u, 0x2955EF28u, 0x00844732u,
        0x642842CAu, 0x36528FD2u, 0x2745104Eu, 0x007B9B57u,
        0x627CEC58u, 0x3486DFA6u, 0x2556DAEFu, 0x0073A270u,
        0x60DF98BBu, 0x32D19B70u, 0x238883E0u, 0x006C4B75u,
        0x5F4F9A66u, 0x313153DEu, 0x21D78407u, 0x00658739u,
        0x5DCC4ECEu, 0x2FA4B68Cu, 0x204190E7u, 0x005F4832u
        ];

        static ReadOnlySpan<UInt64> LogC0 => [
            0x00000000111843D8UL, 0x0B5D69BAD62FA11AUL, 0x1663F6FAD5C18924UL, 0x2118B119BFF1FA3BUL,
        0x2B803474013E4469UL, 0x359EBC5B7234BAEDUL, 0x3F782D720C23D39BUL, 0x49101EAC3E8EE930UL,
        0x5269E12F3A1E46C3UL, 0x5B888736793C6255UL, 0x646EEA2480D45E3CUL, 0x6D1FAFDCE6051F08UL,
        0x759D4F80CF356A9BUL, 0x7DEA15A32F48CB06UL, 0x86082806B4AEF51CUL, 0x8DF988F4B11089FEUL,
        0x95C01A39FE25B941UL, 0x9D5D9FD503210D19UL, 0xA4D3C25E6ABF605BUL, 0xAC241134C69F89B6UL,
        0xB35004723DD423A6UL, 0xBA58FEB271A49370UL, 0xC1404EADF4CD88F9UL, 0xC80730B00143AD7BUL,
        0xCEAECFEA8199248CUL, 0xD53847AC01A300FFUL, 0xDBA4A47AAA7E5E5BUL, 0xE1F4E5170DD764E5UL,
        0xE829FB6931086C4EUL, 0xEE44CD5A005FB44DUL, 0xF446359B13F9EDEFUL, 0xFA2F045E78CC51CCUL
        ];
    }
}
