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
        //  Carry/borrow helpers (analogues of __builtin_addcl / __builtin_subcl)
        // ============================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt64 AddCL(UInt64 a, UInt64 b, UInt64 carryIn, out UInt64 carryOut) {
            unchecked {
                UInt64 s = a + b;
                UInt64 c1 = s < a ? 1UL : 0UL;
                UInt64 s2 = s + carryIn;
                UInt64 c2 = s2 < s ? 1UL : 0UL;
                carryOut = c1 + c2;
                return s2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt64 SubCL(UInt64 a, UInt64 b, UInt64 bin, out UInt64 borrow) {
            unchecked {
                UInt64 d = a - b;
                UInt64 b1 = a < b ? 1UL : 0UL;
                UInt64 d2 = d - bin;
                UInt64 b2 = d < bin ? 1UL : 0UL;
                borrow = b1 + b2;
                return d2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 ReinterpretF128AsU128(UInt128 z) => z;   // raw bits

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 ReinterpretU128AsF128(UInt128 t) => t;   // raw bits
        // ============================================================================
        //  Multi-word arithmetic (little-endian word order).
        //  C sources pass fixed-size arrays (uNx64); we use Span<UInt64> so subarray
        //  views (e.g. cth[j]+2) port naturally.
        // ============================================================================

        // o = a + b
        static void AddU1(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked { o[0] = a[0] + b[0]; }
        }

        // o[0] = high64(a[0] * b[0])
        static void MulHiU1(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked { o[0] = MultiplyHigh(a[0], b[0]); }
        }

        // o = a + b   (n = 2)
        static void AddU2(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = AddCarry(a[0], b[0], 0, out c);
                o[1] = AddCarry(a[1], b[1], c, out c);
            }
        }

        // o = high64(b * a)  (both 2-word)
        static void MulHiU2(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0, c1, t, o0, o1;
                UInt128 a1b0 = (UInt128)a[1] * b[0];
                o0 = (UInt64)(a1b0 >> 64);

                UInt128 a0b1 = (UInt128)a[0] * b[1];
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                t = AddCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);
                o1 = AddCarry((UInt64)(a1b1 >> 64), 0, c0, out c0);
                o1 = AddCarry(o1, 0, c1, out c1);
                o[0] = o0;
                o[1] = o1;
            }
        }

        // o = a + b   (n = 3)
        static void AddU3(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = AddCarry(a[0], b[0], 0, out c);
                o[1] = AddCarry(a[1], b[1], c, out c);
                o[2] = AddCarry(a[2], b[2], c, out c);
            }
        }

        // o = a - b   (n = 3)
        static void SubU3(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = SubBorrow(a[0], b[0], 0, out c);
                o[1] = SubBorrow(a[1], b[1], c, out c);
                o[2] = SubBorrow(a[2], b[2], c, out c);
            }
        }

        // o = high64(b * a)  (both 3-word)
        static void MulHiU3(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c, o0, o1, o2;
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                UInt128 a2b0 = (UInt128)a[2] * b[0];
                UInt128 a0b2 = (UInt128)a[0] * b[2];
                UInt128 a2b1 = (UInt128)a[2] * b[1];
                UInt128 a1b2 = (UInt128)a[1] * b[2];
                UInt128 a2b2 = (UInt128)a[2] * b[2];

                a2b1 += a2b0 >> 64;
                a1b2 += a0b2 >> 64;

                o0 = (UInt64)(a1b1 >> 64);
                o1 = (UInt64)a2b2;
                o2 = (UInt64)(a2b2 >> 64);
                o0 = AddCarry(o0, (UInt64)a2b1, 0, out c);
                o1 = AddCarry(o1, (UInt64)(a2b1 >> 64), c, out c);
                o2 = AddCarry(o2, 0, c, out c);

                o[0] = AddCarry(o0, (UInt64)a1b2, 0, out c);
                o[1] = AddCarry(o1, (UInt64)(a1b2 >> 64), c, out c);
                o[2] = AddCarry(o2, 0, c, out c);
            }
        }

        // o = a + b   (n = 4)
        static void AddU4(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = AddCarry(a[0], b[0], 0, out c);
                o[1] = AddCarry(a[1], b[1], c, out c);
                o[2] = AddCarry(a[2], b[2], c, out c);
                o[3] = AddCarry(a[3], b[3], c, out c);
            }
        }

        // o = high64(b * a)  (both 4-word)
        static void MulHiU4(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c, o0, o1, o2, o3;
                UInt128 a3b0 = (UInt128)a[3] * b[0];
                UInt128 a3b2 = (UInt128)a[3] * b[2];
                UInt128 a2b1 = (UInt128)a[2] * b[1];
                UInt128 a3b1 = (UInt128)a[3] * b[1];
                UInt128 a1b2 = (UInt128)a[1] * b[2];
                UInt128 a2b2 = (UInt128)a[2] * b[2];
                UInt128 a0b3 = (UInt128)a[0] * b[3];
                UInt128 a1b3 = (UInt128)a[1] * b[3];
                UInt128 a2b3 = (UInt128)a[2] * b[3];
                UInt128 a3b3 = (UInt128)a[3] * b[3];

                a3b1 += a2b1 >> 64;
                a2b2 += a1b2 >> 64;

                o0 = AddCarry((UInt64)(a0b3 >> 64), (UInt64)a1b3, 0, out c);
                o1 = AddCarry((UInt64)(a1b3 >> 64), (UInt64)a2b3, c, out c);
                o2 = AddCarry((UInt64)(a2b3 >> 64), (UInt64)a3b3, c, out c);
                o3 = AddCarry((UInt64)(a3b3 >> 64), 0, c, out c);

                o0 = AddCarry(o0, (UInt64)(a3b0 >> 64), 0, out c);
                o1 = AddCarry(o1, (UInt64)a3b2, c, out c);
                o2 = AddCarry(o2, (UInt64)(a3b2 >> 64), c, out c);
                o3 = AddCarry(o3, 0, c, out c);

                o0 = AddCarry(o0, (UInt64)a3b1, 0, out c);
                o1 = AddCarry(o1, (UInt64)(a3b1 >> 64), c, out c);
                o2 = AddCarry(o2, 0, c, out c);
                o3 = AddCarry(o3, 0, c, out c);

                o[0] = AddCarry(o0, (UInt64)a2b2, 0, out c);
                o[1] = AddCarry(o1, (UInt64)(a2b2 >> 64), c, out c);
                o[2] = AddCarry(o2, 0, c, out c);
                o[3] = AddCarry(o3, 0, c, out c);
                
            }
        }

        // o = a - b   (n = 5)
        static void SubU5(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = SubBorrow(a[0], b[0], 0, out c);
                o[1] = SubBorrow(a[1], b[1], c, out c);
                o[2] = SubBorrow(a[2], b[2], c, out c);
                o[3] = SubBorrow(a[3], b[3], c, out c);
                o[4] = SubBorrow(a[4], b[4], c, out c);
            }
        }

        // o = a + b   (n = 5)
        static void AddU5(Span<UInt64> o, ReadOnlySpan<UInt64> a, ReadOnlySpan<UInt64> b) {
            unchecked {
                UInt64 c;
                o[0] = AddCarry(a[0], b[0], 0, out c);
                o[1] = AddCarry(a[1], b[1], c, out c);
                o[2] = AddCarry(a[2], b[2], c, out c);
                o[3] = AddCarry(a[3], b[3], c, out c);
                o[4] = AddCarry(a[4], b[4], c, out c);
            }
        }

        // o = high64(b * a)  (both 5-word)
        static void MulHiU5(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0, c1, t, o0, o1, o2, o3, o4;

                UInt128 a4b0 = (UInt128)a[4] * b[0];
                o0 = (UInt64)(a4b0 >> 64);

                UInt128 a3b1 = (UInt128)a[3] * b[1];
                UInt128 a4b1 = (UInt128)a[4] * b[1];
                t = AddCarry((UInt64)a4b1, (UInt64)(a3b1 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);
                o1 = AddCarry((UInt64)(a4b1 >> 64), 0, c0, out c0);
                o1 = AddCarry(o1, 0, c1, out c1);

                UInt128 a2b2 = (UInt128)a[2] * b[2];
                UInt128 a3b2 = (UInt128)a[3] * b[2];
                t = AddCarry((UInt64)a3b2, (UInt64)(a2b2 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);
                UInt128 a4b2 = (UInt128)a[4] * b[2];
                t = AddCarry((UInt64)a4b2, (UInt64)(a3b2 >> 64), c0, out c0);
                o1 = AddCarry(o1, t, c1, out c1);
                o2 = AddCarry((UInt64)(a4b2 >> 64), 0, c0, out c0);
                o2 = AddCarry(o2, 0, c1, out c1);

                UInt128 a1b3 = (UInt128)a[1] * b[3];
                UInt128 a2b3 = (UInt128)a[2] * b[3];
                t = AddCarry((UInt64)a2b3, (UInt64)(a1b3 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);
                UInt128 a3b3 = (UInt128)a[3] * b[3];
                t = AddCarry((UInt64)a3b3, (UInt64)(a2b3 >> 64), c0, out c0);
                o1 = AddCarry(o1, t, c1, out c1);
                UInt128 a4b3 = (UInt128)a[4] * b[3];
                t = AddCarry((UInt64)a4b3, (UInt64)(a3b3 >> 64), c0, out c0);
                o2 = AddCarry(o2, t, c1, out c1);
                o3 = AddCarry((UInt64)(a4b3 >> 64), 0, c0, out c0);
                o3 = AddCarry(o3, 0, c1, out c1);

                UInt128 a0b4 = (UInt128)a[0] * b[4];
                UInt128 a1b4 = (UInt128)a[1] * b[4];
                t = AddCarry((UInt64)a1b4, (UInt64)(a0b4 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);
                UInt128 a2b4 = (UInt128)a[2] * b[4];
                t = AddCarry((UInt64)a2b4, (UInt64)(a1b4 >> 64), c0, out c0);
                o1 = AddCarry(o1, t, c1, out c1);
                UInt128 a3b4 = (UInt128)a[3] * b[4];
                t = AddCarry((UInt64)a3b4, (UInt64)(a2b4 >> 64), c0, out c0);
                o2 = AddCarry(o2, t, c1, out c1);
                UInt128 a4b4 = (UInt128)a[4] * b[4];
                t = AddCarry((UInt64)a4b4, (UInt64)(a3b4 >> 64), c0, out c0);
                o3 = AddCarry(o3, t, c1, out c1);
                o4 = AddCarry((UInt64)(a4b4 >> 64), 0, c0, out c0);
                o4 = AddCarry(o4, 0, c1, out c1);

                o[0] = o0;
                o[1] = o1;
                o[2] = o2;
                o[3] = o3;
                o[4] = o4;
            }
        }

        // o (5 words) = high64(a (5 words) * b (2 words))
        static void MulHiU5U2(Span<UInt64> o, ReadOnlySpan<UInt64> b, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0, c1, t, o0, o1, o2, o3, o4;

                UInt128 a1b0 = (UInt128)a[1] * b[0];
                UInt128 a2b0 = (UInt128)a[2] * b[0];
                UInt128 a3b0 = (UInt128)a[3] * b[0];
                UInt128 a4b0 = (UInt128)a[4] * b[0];

                o0 = AddCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), 0, out c0);
                o1 = AddCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c0, out c0);
                o2 = AddCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c0, out c0);
                o3 = AddCarry(0, (UInt64)(a4b0 >> 64), c0, out c0);

                UInt128 a0b1 = (UInt128)a[0] * b[1];
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                UInt128 a2b1 = (UInt128)a[2] * b[1];
                UInt128 a3b1 = (UInt128)a[3] * b[1];
                UInt128 a4b1 = (UInt128)a[4] * b[1];

                t = AddCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                o0 = AddCarry(o0, t, 0, out c1);

                t = AddCarry((UInt64)a2b1, (UInt64)(a1b1 >> 64), c0, out c0);
                o1 = AddCarry(o1, t, c1, out c1);

                t = AddCarry((UInt64)a3b1, (UInt64)(a2b1 >> 64), c0, out c0);
                o2 = AddCarry(o2, t, c1, out c1);

                t = AddCarry((UInt64)a4b1, (UInt64)(a3b1 >> 64), c0, out c0);
                o3 = AddCarry(o3, t, c1, out c1);

                t = AddCarry(0, (UInt64)(a4b1 >> 64), c0, out c0);
                o4 = AddCarry(0, t, c1, out c1);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4;
            }
        }

        // o (5 words) = high64(a (5 words) * b0 (1 word))
        static void MulHiU5U1(Span<UInt64> o, UInt64 b0, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0;
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;
                UInt128 a4b0 = (UInt128)a[4] * b0;

                o[0] = AddCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out c0);
                o[1] = AddCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c0, out c0);
                o[2] = AddCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c0, out c0);
                o[3] = AddCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c0, out c0);
                o[4] = AddCarry(0, (UInt64)(a4b0 >> 64), c0, out c0);
            }
        }

        // o (5 words) = full a (4 words) * b0 (1 word)
        static void MulFullU4U1(Span<UInt64> o, UInt64 b0, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c;
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;

                o[0] = (UInt64)a0b0;
                o[1] = AddCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out c);
                o[2] = AddCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c, out c);
                o[3] = AddCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c, out c);
                o[4] = AddCarry(0, (UInt64)(a3b0 >> 64), c, out c);
            }
        }

        // o (6 words) = full a (5 words) * b0 (1 word)
        static void MulFullU5U1(Span<UInt64> o, UInt64 b0, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c;
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;
                UInt128 a4b0 = (UInt128)a[4] * b0;

                o[0] = (UInt64)a0b0;
                o[1] = AddCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out c);
                o[2] = AddCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c, out c);
                o[3] = AddCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c, out c);
                o[4] = AddCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c, out c);
                o[5] = AddCarry(0, (UInt64)(a4b0 >> 64), c, out c);
            }
        }

        // a *= 3  (4 words, in place)
        static void MulU4x3(Span<UInt64> a) {
            unchecked {
                UInt64 c;
                a[0] = AddCarry(a[0], a[1] << 63 | a[0] >> 1, 0, out c);
                a[1] = AddCarry(a[1], a[2] << 63 | a[1] >> 1, c, out c);
                a[2] = AddCarry(a[2], a[3] << 63 | a[2] >> 1, c, out c);
                a[3] = AddCarry(a[3], a[3] >> 1, c, out c);
            }
        }

        // a *= 5  (3 words, in place)
        static void MulU3x5(Span<UInt64> a) {
            unchecked {
                UInt64 c;
                a[0] = AddCarry(a[0], a[1] << 62 | a[0] >> 2, 0, out c);
                a[1] = AddCarry(a[1], a[2] << 62 | a[1] >> 2, c, out c);
                a[2] = AddCarry(a[2], a[2] >> 2, c, out c);
            }
        }

        // ============================================================================
        //  Shifts: in-place, n words, shift count k (can be negative).
        //  Pointer semantics from the C source are expressed with Span<UInt64>.
        // ============================================================================

        static void ShrN(int n, Span<UInt64> a, int k) {
            unchecked {
                if (Misc.Unlikely(k < 0)) {
                    ShlN(n, a, -k);
                } else {
                    int off = k >> 6;
                    int srcIdx = off, dstIdx = 0, aend = n;
                    if (Misc.Likely(srcIdx < aend)) {
                        UInt64 c = a[srcIdx++];
                        int s = k & 63;
                        if (Misc.Likely(s != 0)) {
                            int q = -k & 63;
                            while (Misc.Likely(srcIdx < aend)) {
                                UInt64 nt = a[srcIdx++];
                                a[dstIdx++] = c >> s | nt << q;
                                c = nt;
                            }
                            a[dstIdx++] = c >> s;
                        } else {
                            while (Misc.Likely(srcIdx < aend)) {
                                UInt64 nt = a[srcIdx++];
                                a[dstIdx++] = c;
                                c = nt;
                            }
                            a[dstIdx++] = c;
                        }
                    }
                    while (Misc.Unlikely(dstIdx < aend)) a[dstIdx++] = 0;
                }
            }
        }

        static void ShlN(int n, Span<UInt64> a, int k) {
            unchecked {
                if (Misc.Unlikely(k < 0)) {
                    ShrN(n, a, -k);
                } else {
                    int off = k >> 6;
                    int dstIdx = n - 1, srcIdx = dstIdx - off;
                    if (Misc.Likely(srcIdx >= 0)) {
                        int s = k & 63, q = ~k & 63;
                        UInt64 c = a[srcIdx--];
                        while (Misc.Likely(srcIdx >= 0)) {
                            UInt64 nt = a[srcIdx--];
                            a[dstIdx--] = c << s | nt >> 1 >> q;
                            c = nt;
                        }
                        a[dstIdx--] = c << s;
                    }
                    while (Misc.Unlikely(dstIdx >= 0)) a[dstIdx--] = 0;
                }
            }
        }

        // ============================================================================
        //  Misc 128-bit helpers
        // ============================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 Uq(ReadOnlySpan<UInt64> c) => ((UInt128)c[1] << 64) | c[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 MulHi128Approx(UInt128 a, UInt128 b) {
            UInt64 a_hi = (UInt64)(a >> 64), a_lo = (UInt64)a;
            UInt64 b_hi = (UInt64)(b >> 64), b_lo = (UInt64)b;
            UInt128 a1b0 = (UInt128)a_hi * b_lo;
            UInt128 a0b1 = (UInt128)a_lo * b_hi;
            UInt128 a1b1 = (UInt128)a_hi * b_hi;
            a1b1 += a1b0 >> 64;
            a1b1 += a0b1 >> 64;
            return a1b1;
        }

        static UInt128 SqrhU(UInt128 a) {
            unchecked {
                UInt64 a_hi = (UInt64)(a >> 64), a_lo = (UInt64)a;
                UInt128 a10 = (UInt128)a_hi * a_lo;
                a10 >>= 63;
                UInt128 a11 = (UInt128)a_hi * a_hi;
                a11 += a10;
                return a11;
            }
        }

        // o (4 words) = full square of x (2 words)
        static void SqrU(Span<UInt64> o, ReadOnlySpan<UInt64> x) {
            unchecked {
                UInt128 p10 = (UInt128)x[1] * x[0];
                UInt64 c, p10x = (UInt64)(p10 >> 127);
                p10 <<= 1;
                UInt128 p00 = (UInt128)x[0] * x[0];
                UInt128 p11 = (UInt128)x[1] * x[1];
                o[0] = (UInt64)p00;
                o[1] = AddCarry((UInt64)(p00 >> 64), (UInt64)p10, 0, out c);
                o[2] = AddCarry((UInt64)(p10 >> 64), (UInt64)p11, c, out c);
                o[3] = AddCarry((UInt64)(p11 >> 64), p10x, c, out c);
            }
        }

        // o (5 words) = approximate square of a (5 words)
        static void SqrhU5(Span<UInt64> o, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0, c1, o0, o1, o2, o3, o4, t;
                UInt128 a4a0 = (UInt128)a[4] * a[0];
                UInt128 a3a1 = (UInt128)a[3] * a[1];
                o0 = AddCarry((UInt64)(a4a0 >> 64), (UInt64)(a3a1 >> 64), 0, out c0);
                o1 = c0;
                UInt128 a4a1 = (UInt128)a[4] * a[1];
                UInt128 a3a2 = (UInt128)a[3] * a[2];
                t = AddCarry((UInt64)a4a1, (UInt64)a3a2, 0, out c1);
                o0 = AddCarry(o0, t, c0, out c0);
                t = AddCarry((UInt64)(a4a1 >> 64), (UInt64)(a3a2 >> 64), c1, out c1);
                o1 = AddCarry(o1, t, c0, out c0);
                UInt128 a4a2 = (UInt128)a[4] * a[2];
                o1 = AddCarry(o1, (UInt64)a4a2, c0, out c0);
                UInt128 a4a3 = (UInt128)a[4] * a[3];
                t = AddCarry((UInt64)a4a3, (UInt64)(a4a2 >> 64), c1, out c1);
                o2 = AddCarry(0, t, c0, out c0);
                o3 = AddCarry(0, (UInt64)(a4a3 >> 64), c1, out c1);

                o0 = AddCarry(o0, o0, 0, out c0);
                o1 = AddCarry(o1, o1, c0, out c0);
                o2 = AddCarry(o2, o2, c0, out c0);
                o3 = AddCarry(o3, o3, c0, out c0);
                o4 = c0;

                UInt128 a2a2 = (UInt128)a[2] * a[2];
                UInt128 a3a3 = (UInt128)a[3] * a[3];
                UInt128 a4a4 = (UInt128)a[4] * a[4];
                o[0] = AddCarry(o0, (UInt64)(a2a2 >> 64), 0, out c0);
                o[1] = AddCarry(o1, (UInt64)a3a3, c0, out c0);
                o[2] = AddCarry(o2, (UInt64)(a3a3 >> 64), c0, out c0);
                o[3] = AddCarry(o3, (UInt64)a4a4, c0, out c0);
                o[4] = AddCarry(o4, (UInt64)(a4a4 >> 64), c0, out c0);
            }
        }

        // o (4 words) = approximate square of a (4 words)
        static void SqrhU4(Span<UInt64> o, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt64 c0, o0, o1, o2, o3;
                UInt128 a2a1 = (UInt128)a[2] * a[1];
                UInt128 a3a0 = (UInt128)a[3] * a[0];
                UInt128 a3a1 = (UInt128)a[3] * a[1];
                UInt128 a3a2 = (UInt128)a[3] * a[2];

                o0 = AddCarry((UInt64)a3a1, (UInt64)(a3a0 >> 64), 0, out c0);
                o1 = AddCarry((UInt64)a3a2, (UInt64)(a3a1 >> 64), c0, out c0);
                o2 = AddCarry(0, (UInt64)(a3a2 >> 64), c0, out c0);

                o0 = AddCarry(o0, (UInt64)(a2a1 >> 64), 0, out c0);
                o1 = AddCarry(o1, 0, c0, out c0);
                o2 = AddCarry(o2, 0, c0, out c0);

                o0 = AddCarry(o0, o0, 0, out c0);
                o1 = AddCarry(o1, o1, c0, out c0);
                o2 = AddCarry(o2, o2, c0, out c0);
                o3 = c0;

                UInt128 a2a2 = (UInt128)a[2] * a[2];
                UInt128 a3a3 = (UInt128)a[3] * a[3];
                o[0] = AddCarry(o0, (UInt64)a2a2, 0, out c0);
                o[1] = AddCarry(o1, (UInt64)(a2a2 >> 64), c0, out c0);
                o[2] = AddCarry(o2, (UInt64)a3a3, c0, out c0);
                o[3] = AddCarry(o3, (UInt64)(a3a3 >> 64), c0, out c0);
            }
        }

        // o (2 words) = approximate square of a (2 words)
        static void SqrhU2(Span<UInt64> o, ReadOnlySpan<UInt64> a) {
            unchecked {
                UInt128 a1a0 = (UInt128)a[1] * a[0];
                UInt128 a1a1 = (UInt128)a[1] * a[1];
                a1a1 += a1a0 >> 63;
                o[0] = (UInt64)a1a1;
                o[1] = (UInt64)(a1a1 >> 64);
            }
        }

        // ============================================================================
        //  Signed 128-bit helpers (analogues of mhUIm / mhIU)
        // ============================================================================

        static Int128 MhUIm(UInt128 a, Int128 b, UInt64 mask) {
            unchecked {
                UInt128 sub = a & ((UInt128)mask | ((UInt128)mask << 64));
                return (Int128)MulHi128Approx(a, (UInt128)b) - (Int128)sub;
            }
        }

        static Int128 MhIU(Int128 b, UInt128 a) {
            unchecked {
                return MhUIm(a, b, (UInt64)(b >> 127));
            }
        }

        // ============================================================================
        //  Classification
        //  0 = ordinary, 1 = infinity, 2 = sNaN, 3 = qNaN
        // ============================================================================

        static SByte GetClass(UInt128 x) {
            unchecked {
                UInt64 xh = (UInt64)(x >> 64), xl = (UInt64)x;
                int t = unchecked((int)(xh >> 32)) | ((xh << 32 | xl) != 0 ? 1 : 0);
                int r = 0;
                if (t >= (0x7FFF << 16)) r++;
                if (t >= (0x7FFF << 16) + 1) r++;
                if (t >= (0x7FFF8 << 12)) r++;
                return (SByte)r;
            }
        }

        // ============================================================================
        //  rsqrt9 tables and function
        // ============================================================================

        static ReadOnlySpan<UInt32> Rsqrt9Table => [
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

        // Note: The value of `0xffffffff` is used as an "unsigned" 32-bit constant.
        // In C#, `static const unsigned c[][4]` -- 4 columns, 64 rows. We store
        // them flattened: c[row][col] = Rsqrt9Table[row*4 + col].
        // NOTE: 0xffffffff as UInt32 literal is 0xffffffffUL, but the original table
        // shows "0xffffffff" which is UInt32 4294967295. In the code below,
        // `c0 <<= 31; c0 |= 1ull<<63;` — c0 is UInt64, so the top bit becomes
        // explicit anyway. Fine.

        static UInt64 Rsqrt9(UInt64 m) {
            unchecked {
                int indx = (int)(m >> 58);
                UInt32 c0 = Rsqrt9Table[indx * 4 + 0];
                UInt32 c1 = Rsqrt9Table[indx * 4 + 1];
                UInt32 c2 = Rsqrt9Table[indx * 4 + 2];
                UInt32 c3 = Rsqrt9Table[indx * 4 + 3];

                UInt64 c0_64 = (UInt64)c0 << 31;
                c0_64 |= 1UL << 63;
                UInt64 c1_64 = (UInt64)c1 << 25;
                UInt64 d = (m << 6) >> 32;
                UInt64 d2 = (UInt64)(d * d) >> 32;
                UInt64 re = c0_64 + ((d2 * c2) >> 13);
                UInt64 ro = (d * ((c1_64 + ((d2 * c3) >> 19)) >> 26)) >> 6;
                UInt64 r = re - ro;

                UInt64 r2 = MultiplyHigh(r, r);
                Int64 h = unchecked((Int64)(MultiplyHigh(m, r2) + r2));
                Int64 hr = MultiplyHigh(h, (Int64)(r >> 1));
                r -= unchecked((UInt64)hr);
                if (Misc.Unlikely(r == 0)) r--;
                return r;
            }
        }

        // ============================================================================
        //  jget table (short) and index table (signed char)
        // ============================================================================

        static ReadOnlySpan<Int16> Sth => [
            11476, 9429, 8096, 7384, 6672, 6053, 5699, 5346, 4995, 4645, 4297,
            4023, 3851, 3680, 3510, 3342, 3174, 3009, 2844, 2681, 2520, 2361,
            2203, 2048, 1894, 1742, 1592, 1445, 1299, 1157, 1016, 878, 742, 609,
            479, 351, 226, 104, -31, -263, -490, -711, -925, -1133, -1335,
            -1531, -1719, -1901, -2105, -2442, -2765, -3074, -3369, -3650,
            -3917, -4240, -4715, -5159, -5574, -5959, -6484, -7134, -7722,
            -8306, -9238, -10046, -11220, -12394, -14139, -16488, -20584,
        ];

        static ReadOnlySpan<UInt32> Pth => [
            0, 0xb2c0, 0x16560, 0x21800, 0x2ca00, 0x37c00, 0x42d80, 0x4de80,
            0x58f00, 0x63e80, 0x6ed80, 0x79b80, 0x84900, 0x8f500, 0x9a000,
            0xa4a00, 0xaf200, 0xb9a00, 0xc3f00, 0xce400, 0xd8700, 0xe2800,
            0xec700, 0xf6500, 0x100000, 0x109a00, 0x113200, 0x11c800, 0x125b00,
            0x12ed00, 0x137b00, 0x140800, 0x149200, 0x151a00, 0x159f00,
            0x162100, 0x16a100, 0x171e00, 0x179800, 0x180f80, 0x188380,
            0x18f500, 0x196380, 0x19ce80, 0x1a3680, 0x1a9b80, 0x1afd80,
            0x1b5b80, 0x1bb680, 0x1c0e40, 0x1c6280, 0x1cb340, 0x1d0080,
            0x1d4a40, 0x1d9080, 0x1dd340, 0x1e1200, 0x1e4d60, 0x1e84e0,
            0x1eb8c0, 0x1ee8e0, 0x1f1540, 0x1f3de0, 0x1f62a0, 0x1f8390,
            0x1fa0b0, 0x1fb9f0, 0x1fcf50, 0x1fe0d4, 0x1fee76, 0x1ff834,
            0x1ffe0d,
        ];

        // cth[j] is 5 words; store flattened (row stride = 5).
        static ReadOnlySpan<UInt64> Cth => [
            // j = 0
            0, 0, 0, 0, 0,
            // j = 1
            0x64d23d92f9ea2771UL, 0x6d047db3a6e206a7UL, 0x2953c428c9d7bc56UL, 0xfb0f1838d5a3a8daUL, 0xfff06594451d279dUL,
            // j = 2
            0xe3080aa3bd169d0cUL, 0x4054eae9456537dbUL, 0x9e5833eae773bc87UL, 0x6f49ccb466446b89UL, 0xffc19bc9271a05b3UL,
            // j = 3
            0x00912c8144374c85UL, 0x309a44ddae7556ceUL, 0xc319285201c58b56UL, 0x3a246e54beb8dac9UL, 0xff73917b77aa5c47UL,
            // j = 4
            0x6a7360619e7edb23UL, 0x0b11b912de529986UL, 0x5a6995a8cc7d5eacUL, 0x552a6d7261223cc4UL, 0xff069a0439cbe651UL,
            // j = 5
            0x238522ea64680061UL, 0xf079215ebcd5ca36UL, 0x290de5267799873eUL, 0xcab4f035a6bfa060UL, 0xfe7a55701a8697b0UL,
            // j = 6
            0x7a92124309b60ef2UL, 0x4b9dd14818169efcUL, 0x040ba2edca526f4eUL, 0xac4772d0d4932b9bUL, 0xfdcf16b94b0b442eUL,
            // j = 7
            0x30ff410c35a4b64dUL, 0xd1d820652a9bc4c6UL, 0x6d055dc0a4c09600UL, 0xb4aab1b03d022116UL, 0xfd04e2530bcbd671UL,
            // j = 8
            0x03aedba8702f81c3UL, 0x96da5af0d3ea2f8bUL, 0xca571fb2cbdee5b9UL, 0x17c0bd0e66183953UL, 0xfc1bb125286c39d8UL,
            // j = 9
            0xfb91abaebc934604UL, 0xce94d1434f07a0a1UL, 0x068284240ceeae19UL, 0x5e9a55955f4b097fUL, 0xfb143c17bdcc996dUL,
            // j = 10
            0xab794690d17636d8UL, 0xc8e308f9bd76c0e2UL, 0x15fddd7233c65eedUL, 0xdb301a162cf8115dUL, 0xf9edc73d6c923555UL,
            // j = 11
            0x593fea054c7f450eUL, 0x6b0aa37cffc886a5UL, 0xd8e480c9ebb134e1UL, 0x287dce6a9f8ea2acUL, 0xf8a922ae090b0799UL,
            // j = 12
            0x251fef9c8bb46028UL, 0x493c5f39729df2a1UL, 0xc745b46aca55d0e8UL, 0x2062f831fa08f4afUL, 0xf7454c2c7d12b97aUL,
            // j = 13
            0xff1ffe460bd9c836UL, 0xb16e3e5300ab04b0UL, 0xcc500109643b1f25UL, 0x1bf03e7ae9dd3a2aUL, 0xf5c455407155fe59UL,
            // j = 14
            0x4606e3f3b69b5b9fUL, 0xaf57d7aca36b7036UL, 0x23dc96105c0b9d9bUL, 0x0e73cd55dd020b9fUL, 0xf4253c1c1a1ae532UL,
            // j = 15
            0x3e9033c3d73b8ba0UL, 0x36a1f12dab4107b3UL, 0xe2bf1fc80065cf4bUL, 0x067308deced752e6UL, 0xf267ed62f8b7c795UL,
            // j = 16
            0xdc78f0c65249d290UL, 0x3da244d00c619d0dUL, 0xd5109661f43e9ecdUL, 0x70a15831a61bca6bUL, 0xf08f329d05a331b0UL,
            // j = 17
            0x2e799ef643386021UL, 0xf50e2c1a3600d3adUL, 0x828f074191858249UL, 0x4f6a116da6359a61UL, 0xee953e620e55df7bUL,
            // j = 18
            0xf59cf516cfc8aef3UL, 0x3994459558477033UL, 0xcfbcad100d166a9eUL, 0x6632028b37ff16adUL, 0xec832dd8f9584e23UL,
            // j = 19
            0x8993bed4bc91f71bUL, 0x2d156655d9d43f98UL, 0x7386f1c67cf345e5UL, 0xd03960687c6a9efeUL, 0xea4f644f48136890UL,
            // j = 20
            0x6c1be9d31d8598feUL, 0x0bbc7599cc98b839UL, 0x1b0cb81a981ed38dUL, 0x74401bc33d3f4f33UL, 0xe800632c0e1d2f2cUL,
            // j = 21
            0xd77e840a65911d5aUL, 0x0409ada21048c2b1UL, 0x54c0567f7377993bUL, 0xb61bea5c37779619UL, 0xe59668e019f1e288UL,
            // j = 22
            0xdf264c217da2460fUL, 0x4310bc9fc7837821UL, 0xa5cb92ab27280ab6UL, 0x03513aa71030fbb2UL, 0xe311a97671f44f1cUL,
            // j = 23
            0x8ee20e415370de4eUL, 0xb85f2ea14495bf6fUL, 0xa12912f50e813792UL, 0x0f451e45046e7646UL, 0xe06dea9a80d1fdf6UL,
            // j = 24
            0x2485e7ecaf78aedfUL, 0x639053243722d371UL, 0x92ec1a6629ed23ccUL, 0x92ba16b83c5c1dc4UL, 0xddb3d742c265539dUL,
            // j = 25
            0x7d45353940ee462bUL, 0x81cfccf2da1a22d6UL, 0x4db6039c8f8f6015UL, 0x83992b6bb6c89121UL, 0xdada7cc9882f1832UL,
            // j = 26
            0xb0de57772eac59f6UL, 0x855009022ef97eb9UL, 0x8549b723bab4be2dUL, 0x27e229b54b803697UL, 0xd7e6403e36d2f1f7UL,
            // j = 27
            0xe6c36c5ad24b30c1UL, 0x5258a95fc94e2fb8UL, 0x63aea18a41e1bf81UL, 0x2a16383f5c4d5062UL, 0xd4d7154f370bbe07UL,
            // j = 28
            0x4f980d926321df45UL, 0xad92bc366eaf4837UL, 0x360b41691ba7a5f0UL, 0x010bd7d48ab25527UL, 0xd1b27b4ae9008ef2UL,
            // j = 29
            0xb53f05eae4761ab3UL, 0x5636614d38c536c1UL, 0x9cfea8981156d5ddUL, 0x0bb008a15e850cecUL, 0xce6d5666b787ca77UL,
            // j = 30
            0x80638f93f4a2c269UL, 0x1dcdbcdbbcf14233UL, 0xae4218b2749a67beUL, 0x69d2608c3b73e12fUL, 0xcb190ae896231bceUL,
            // j = 31
            0xe516cefc6347a5e9UL, 0x9684ba1ed4646c7aUL, 0x5f831bdef11f9bbcUL, 0xc6158124a4ef775fUL, 0xc7a3b782716d38c8UL,
            // j = 32
            0x8f005279faa6880dUL, 0x890f8c878e5f7df9UL, 0x3485668a4e6e9637UL, 0xcf01d7d5779f0507UL, 0xc419953003b5c74bUL,
            // j = 33
            0x744fe29890f407f9UL, 0x6f46175fab644dfcUL, 0xd9330605ec9b4e0aUL, 0xa322920e8bd7accfUL, 0xc07416e77d712462UL,
            // j = 34
            0xab325fd93ce9b6b8UL, 0x57c460f1a312c3ddUL, 0x0fcf1122ee32b17cUL, 0x892c26b0bcfacc8dUL, 0xbcba10b0d8770ed2UL,
            // j = 35
            0xcef1e69c4eadb8b3UL, 0x7f131158279ff4d7UL, 0x4e6ba5183b584705UL, 0x8fcd4c453517e8baUL, 0xb8ebe30ed9fac186UL,
            // j = 36
            0x3745f18539bf8772UL, 0x2cee4ab653475e6eUL, 0xc28307bea98bb0e7UL, 0x5cbc46ee3d37b72dUL, 0xb501e65acbad5ad8UL,
            // j = 37
            0xf4c3a3a64ddafd51UL, 0x5e65313f078e13abUL, 0xe6ba3e11cdb88afbUL, 0xf74e3a8ab016619fUL, 0xb103b40770404b83UL,
            // j = 38
            0x539c48de71a3cc8eUL, 0x7f208cb9376741f7UL, 0xa22a23cb0d6b1468UL, 0x3fd4750afb3804bbUL, 0xacf1860da3cfd826UL,
            // j = 39
            0xbc5eb21dc1786f2dUL, 0xcaf9195f5c2505baUL, 0xf402f48b30c4df2fUL, 0x4987496d863e1e7cUL, 0xa8c6fae0bbd09645UL,
            // j = 40
            0x61f8e4beffcf379fUL, 0x758e5ddd22563b4fUL, 0x80d42d484f061220UL, 0xfbc0967b24f94966UL, 0xa48d1e8d86992cdcUL,
            // j = 41
            0xcdf0fca2bcbaff85UL, 0x201f9ee54d8615faUL, 0x7a56fc977f0d0e4cUL, 0x754bb974869014d6UL, 0xa03aa9d87a93f00dUL,
            // j = 42
            0xcae22303aeaf6595UL, 0xe7b53a3891af1ae5UL, 0x6de54e5c6a03779dUL, 0x6cf567b89889fef8UL, 0x9bd425459273d285UL,
            // j = 43
            0x9831e7542d85d7eaUL, 0x611013ca24685535UL, 0xa3724e71c24123d7UL, 0xb849ff7aea680179UL, 0x975eea2b65a9847eUL,
            // j = 44
            0x101a86b895c7d937UL, 0xe88c06711a5ab702UL, 0xfd217ba2d78129ecUL, 0x20b672429fb3d1ddUL, 0x92d5d4e3fc1f9cbdUL,
            // j = 45
            0x4d91c0b5749d4914UL, 0x8ed80817147623b6UL, 0xb90bd29f73f58752UL, 0x5ff44c46ffccf7f3UL, 0x8e38a46bdb3b3d12UL,
            // j = 46
            0x2efea8ce593572c2UL, 0xf37f1d4704fb5c49UL, 0xf4c2e89353ada913UL, 0xedbf03f73c1d3162UL, 0x8986f9b6c7132cffUL,
            // j = 47
            0x35b06cbf85f84fbbUL, 0xbf788966cdcd56b0UL, 0x91e498b6c64670dcUL, 0xf6e11dfe5f762e4dUL, 0x84cd81e775b4c098UL,
            // j = 48
            0x539ba47e0b57d2deUL, 0x974b441dfa650168UL, 0xa477f58bc7c8dc47UL, 0x29dd7114d81c96bfUL, 0x7fffb96fec8ce447UL,
            // j = 49
            0x3d5bcfe7068c0ff1UL, 0x5016bbcd4ef1b8e7UL, 0x6f1857955007a55cUL, 0x98ea27d884420256UL, 0x7b208eaab0b94056UL,
            // j = 50
            0x846ef313e2e0bf68UL, 0xabfac2142052bc6cUL, 0x8496c1983f214586UL, 0xe132acebbd5846b8UL, 0x7633823fd4eeef51UL,
            // j = 51
            0x03681a2fcb57c1e9UL, 0x999fd17f45cc07f3UL, 0x411f4be11fd95eb2UL, 0xa206ce1148e4933cUL, 0x7138b866dbf2205cUL,
            // j = 52
            0x701d2b27adfe0fb8UL, 0x239f4fe8463aee07UL, 0xe0b2bb9935920222UL, 0x5d94ac1eeb7461f9UL, 0x6c3040ff7ae4f3d5UL,
            // j = 53
            0x064f259d5d960d10UL, 0xb194cf34ff2bf070UL, 0x5f46977eac6f5a5cUL, 0x4367583037f40763UL, 0x671a12bd2092f0e6UL,
            // j = 54
            0x739008a332539b2bUL, 0x34d36b62e94d21ecUL, 0xf32753bd933f314cUL, 0x9a57aae41393eb49UL, 0x61f604a26e818217UL,
            // j = 55
            0x4cda609f93951dc0UL, 0xe4f6c5ba31b5c6edUL, 0x4fa0c524c392182aUL, 0x37df71c28f1de586UL, 0x5cc3c513b7fdb337UL,
            // j = 56
            0x9a01a8613fd798ebUL, 0x266e72c3de2e292eUL, 0x59fec1c23334cf3aUL, 0x2e578575cc9228dfUL, 0x578dcbb6628e3c6fUL,
            // j = 57
            0x1f1a07c6d6b6adcdUL, 0x1eb9dc19b8598724UL, 0xd9ab93ace24809dcUL, 0x05dab10687beb878UL, 0x5246f2e8fd06b80bUL,
            // j = 58
            0xb8a6a56a834058ebUL, 0xe60a200e3eb8678aUL, 0xe50f6f2cd1d3aaceUL, 0xedb16a84844fc0a7UL, 0x4cfa66f577d9eaf3UL,
            // j = 59
            0x69aab942d6b77874UL, 0x9d6df99ad27e5270UL, 0x80152267d988b733UL, 0xdbe4096c369e089eUL, 0x47a24816dd512c95UL,
            // j = 60
            0x0550f19403d41e0aUL, 0xbb071ead370af5ffUL, 0xb9b5812340f07744UL, 0x64f8889a905e2862UL, 0x4241a5c4a8543894UL,
            // j = 61
            0xacd334aee8e9e8a3UL, 0x92d46ccf664aa8e9UL, 0x9d79c6111152cb8aUL, 0x4b40d1af34812df0UL, 0x3cd8779db76341cdUL,
            // j = 62
            0x0403e507d395ba75UL, 0x4c8823c7ba7c9380UL, 0x0a03c989401a778bUL, 0x14849e09356ec1dcUL, 0x37667d4c98bfbccbUL,
            // j = 63
            0x657bb80e7a20116aUL, 0xa79b3d94f1414a71UL, 0x86ab887459d6023cUL, 0x602a110d5bf3bfc0UL, 0x31f02728a1820184UL,
            // j = 64
            0x531dfb48ce58358dUL, 0xe827b8eac551e9eeUL, 0x1b75725ed755aa4aUL, 0xc588e51fb5f4fd50UL, 0x2c736b07088a07baUL,
            // j = 65
            0x90c4aa84e4598669UL, 0x7dd80f90a7af46adUL, 0xa23d2ad47df691abUL, 0x4e35d5f602d9b24dUL, 0x26efff9c8eff17d3UL,
            // j = 66
            0x06b8ea90e4449667UL, 0x3b72f4405f272c6fUL, 0xe689513157c5339aUL, 0xa73888a873793997UL, 0x2168e05fb8858700UL,
            // j = 67
            0xc343b6652a5c735aUL, 0x18fae9a2c9f7cf27UL, 0x7e0c3ca1daac2e97UL, 0xbb8233abbcf0c73aUL, 0x1bde7b65ebd95dd2UL,
            // j = 68
            0x3097e3126b116bd9UL, 0x35782cb810ce1724UL, 0x5ca4cd631e80444dUL, 0xd2eff3545c8a56c1UL, 0x164fbb9bd36b968cUL,
            // j = 69
            0xe12e48b0cf8a0b8eUL, 0x402a65318c77466eUL, 0xfffbc69461af1af2UL, 0x0d511d6a919beca2UL, 0x10be2e7affde1a24UL,
            // j = 70
            0xed8e7549e2530cadUL, 0x419a20b85f378012UL, 0x4d48fe6abd0a9a0eUL, 0x40b937dae10995f7UL, 0x0b2a9f7bd1a4e3f7UL,
            // j = 71
            0x0ccf7c9e0c0fe38aUL, 0xf496f2cb6c0dc6caUL, 0x5c67a95d27203004UL, 0x87e16185128e709bUL, 0x059591109ebd190aUL,
        ];

        static ReadOnlySpan<SByte> Ind => [
            70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70,
            70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 69, 69,
            69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 69, 68,
            68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 67, 67, 67, 67,
            67, 67, 67, 67, 67, 66, 66, 66, 66, 66, 66, 66, 66, 66, 65, 65, 65,
            65, 65, 65, 64, 64, 64, 64, 64, 64, 64, 64, 63, 63, 63, 63, 62, 62,
            62, 62, 62, 61, 61, 61, 61, 61, 60, 60, 60, 60, 59, 59, 59, 58, 58,
            58, 57, 57, 57, 57, 56, 56, 56, 55, 55, 55, 54, 54, 53, 53, 52, 52,
            51, 51, 51, 50, 50, 49, 49, 49, 48, 48, 47, 46, 46, 45, 44, 44, 43,
            42, 42, 41, 41, 40, 39, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30, 30,
            29, 28, 27, 26, 25, 24, 24, 23, 22, 21, 20, 19, 19, 18, 17, 16, 16,
            15, 14, 13, 13, 12, 11, 11, 10, 10, 10, 9, 9, 9, 8, 8, 7, 7, 7, 6,
            6, 6, 5, 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2,
            2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            0, 0, 0, 0, 0, 0,
        ];

        // phi0[j] is 6 words; store flattened (row stride = 6).
        static ReadOnlySpan<UInt64> Phi0 => [
            // j = 0
            0, 0, 0, 0, 0, 0,
            // j = 1
            0x98ee01dc297ef137UL, 0x833839e554b92130UL, 0x0a35967bc3749afbUL, 0x09800df8ddc2a5e2UL, 0x016587438f269b93UL, 0,
            // j = 2
            0xcfec743ac824b7b9UL, 0x66ec76eb4fb09df3UL, 0x5265df8b7c3f33c1UL, 0xdbf35587e9e2cd87UL, 0x02cafa166dc09244UL, 0,
            // j = 3
            0xd9ea018b49a9a7acUL, 0x2e80d2fd5768e869UL, 0x7f990060e27ca191UL, 0xacfb3347cdab5663UL, 0x0430c42ffb6051aeUL, 0,
            // j = 4
            0x5a7d59fe33fbd45cUL, 0xfac32e4e4b697d19UL, 0x14a12d811a08c9d1UL, 0xa458339e940758e1UL, 0x0595d06ea0a22010UL, 0,
            // j = 5
            0xbc86efe2e5f48417UL, 0x8e3ee3ae76d8513aUL, 0x640a9fc17520d677UL, 0xb5cab3f8d569e3b5UL, 0x06fb8b520dddc5beUL, 0,
            // j = 6
            0xb768418745f28dd9UL, 0x787494c4af664176UL, 0xbef4267e62a7f37bUL, 0x3b5e3b804feb82b8UL, 0x08611f97ec71f4d0UL, 0,
            // j = 7
            0x1568e021e2ba7c88UL, 0x389d69e8437f8a69UL, 0xdd1382a0d543b01aUL, 0x496cf2cedfaae306UL, 0x09c6b8bdd13f755eUL, 0,
            // j = 8
            0xd981fe8928879085UL, 0xca5329c38c2cf11bUL, 0x868a81770bf58debUL, 0x9b9c832d762e0355UL, 0x0b2c82a5715a2540UL, 0,
            // j = 9
            0x803682d986d5d261UL, 0xb4845f3eefebbbadUL, 0x600541ff7c2eccebUL, 0xbcafd279b45b8783UL, 0x0c91a4c2050c21abUL, 0,
            // j = 10
            0xd48a7991528086acUL, 0x24911228dfca1969UL, 0xa86d3f15c0c947aeUL, 0x910e90af3cd12b01UL, 0x0df74ef328432b11UL, 0,
            // j = 11
            0xc94f05bb785212baUL, 0xed4dd3fe4e1a18f8UL, 0xc8ba7d4bbf0dd556UL, 0xfbac51efb6caa9eeUL, 0x0f5ca782c5497407UL, 0,
            // j = 12
            0x6774ecd16dbef24dUL, 0x5c455230e3e27926UL, 0x22479a62d912a3bcUL, 0xe7c0193f47088dffUL, 0x10c2e217f6ce32d4UL, 0,
            // j = 13
            0x065dc6d152815c75UL, 0xf04a70b98dd4a723UL, 0xcbdbb25e22f5f0bbUL, 0xdf4937856b85a07eUL, 0x1228197097350eb6UL, 0,
            // j = 14
            0xe9f602011b282870UL, 0xe82bd25b373ed1adUL, 0x8a4dc661188d900bUL, 0xf6444858ac431981UL, 0x138d81148233691bUL, 0,
            // j = 15
            0x8fea4147ae4dc654UL, 0x98cf4ab5bfef3e64UL, 0xe4a1c1562fca8d98UL, 0xf0368df5f848a51dUL, 0x14f345fb14d06d08UL, 0,
            // j = 16
            0x38d72d4e6735e238UL, 0x7a2d62272a639ab7UL, 0xa612c8dc05aeca56UL, 0x39aadf69fd05412fUL, 0x16577574c736f822UL, 0,
            // j = 17
            0x8b0a7170caea9d5bUL, 0x27c1cf7c1a06fa14UL, 0x02069a770726ca5eUL, 0x7c4f696ede5fa314UL, 0x17be7c689b29a551UL, 0,
            // j = 18
            0x7932d250fdc19ae3UL, 0xd23bc7e1175b5ba7UL, 0xec29fc6fd46e189fUL, 0x88b15e01adf30d3aUL, 0x19221b18a2b52525UL, 0,
            // j = 19
            0x4123f1d8054542c2UL, 0x45ee3add846d2f88UL, 0x93f2249af814dad5UL, 0xfc147c180f97b340UL, 0x1a88f3ec0c7019e8UL, 0,
            // j = 20
            0x2f5534f59b39f35cUL, 0x6f6a9ab389dc2e64UL, 0xf6b8e5b0d1afdc91UL, 0xb4b3f8775f0ba4afUL, 0x1bee1224e5606f4aUL, 0,
            // j = 21
            0xab6580dffc5cad8fUL, 0xbd7036041b733567UL, 0x5cb33945d1250ab6UL, 0x1218f911043d7131UL, 0x1d540811073115d0UL, 0,
            // j = 22
            0x066aa0a3134c137eUL, 0x59c6c881ab04c92dUL, 0x9e482027d47ee788UL, 0xd49d73f1d94d5abfUL, 0x1eb88e86a05e1646UL, 0,
            // j = 23
            0x44c6d7e7bf534b9cUL, 0xb06417cd6f4b50c4UL, 0xea20eca3a5ba0bb9UL, 0xfac469c187134857UL, 0x201ee316b08cad79UL, 0,
            // j = 24
            0x62e1ac14425e00d0UL, 0x55ac9fc65f2def30UL, 0xdc2b0d016c66a213UL, 0xcb7665c1eacf5a22UL, 0x2182a4705ae6cb08UL, 0,
            // j = 25
            0xa9b283dac596b605UL, 0x5fa4861e7ef2b865UL, 0x25b5e8f0c4f84141UL, 0x312668a01c646d4bUL, 0x22e89368619cc920UL, 0,
            // j = 26
            0x1870eef39e4d7560UL, 0x268dfff2754d4cf9UL, 0x72e8040d7d970a1bUL, 0x714ad8d90d614caeUL, 0x244e9391c2c115b7UL, 0,
            // j = 27
            0x4aa29cb31acb8056UL, 0x0bf09c95910a22e1UL, 0xb94dbede99a3fb07UL, 0xd047ce796631eca9UL, 0x25b4d25453036018UL, 0,
            // j = 28
            0xe3ba19c8732cb2ffUL, 0x85c23d5ca072439dUL, 0x468f8bb315059a40UL, 0xed7e66087d1bd175UL, 0x27190e2b73a92c06UL, 0,
            // j = 29
            0x3dabe71847949223UL, 0x68a8213d4d431f91UL, 0x62f05cf2b3bd2710UL, 0x1bfe511df99c3f24UL, 0x28805177276dca59UL, 0,
            // j = 30
            0x5ed25eca0b62e3d2UL, 0x414b4ddbedbba46cUL, 0x806bc58f612ad096UL, 0x19542ac43ea8cca6UL, 0x29e35e91b23af2e6UL, 0,
            // j = 31
            0x461794962913e34aUL, 0x2173dec430ab5f19UL, 0xd86f9df8a8a1ca1eUL, 0x62a1e34bd1046ac2UL, 0x2b49dc99a7f107edUL, 0,
            // j = 32
            0xf6894f18599bbbc3UL, 0xfb42ed3f8092e674UL, 0x211376f5bd95b6f5UL, 0xa9954fa4b6a3ae67UL, 0x2caeee56ee9d76e1UL, 0,
            // j = 33
            0xe1237822d72fcc91UL, 0x6234f9e5fc87cd58UL, 0x5b561e852695a9c9UL, 0x27a4ac920cc2d485UL, 0x2e1555560bf870ebUL, 0,
            // j = 34
            0x5a660506d1ac44f0UL, 0x52288e69a1a6979aUL, 0x0a8fd342fa39186aUL, 0xfdd3cc3e53529c31UL, 0x2f7a9c03b2c9f414UL, 0,
            // j = 35
            0x9ca4a66d94f7ce98UL, 0xb3cc7b754ea110e8UL, 0x6b2742ff360ad0bbUL, 0x96adb5df0884220eUL, 0x30deddc5b3a10ac0UL, 0,
            // j = 36
            0x22d5717c16f70066UL, 0xbb740e701f426b4dUL, 0x198b66d49a271ae4UL, 0x75723e247890af33UL, 0x32450ab8886790d8UL, 0,
            // j = 37
            0x20f779a2a364ada1UL, 0xbcefde528ed4843dUL, 0xd9b7ad636a26a2e8UL, 0xbe1d44dc6c140104UL, 0x33aa8c3852d78cb5UL, 0,
            // j = 38
            0xaa78df837ecdaa97UL, 0xbf01603890538307UL, 0xb39c7da573049b6eUL, 0xc6ee2586ef7102a6UL, 0x350f83805f4ef2afUL, 0,
            // j = 39
            0xa7101ffddaa0205cUL, 0xff482e8b905c3c42UL, 0x4c352411efcd1e54UL, 0x30f588c99bd199afUL, 0x367597d00863a4fbUL, 0,
            // j = 40
            0x7a50c80306a1cd12UL, 0xc0beba6e79bc66c3UL, 0x0b9754ce987c0953UL, 0x35799f82fec17b96UL, 0x37d9efa5cc792058UL, 0,
            // j = 41
            0x4d761df6ad0e0a02UL, 0x81d32ddacd6bd035UL, 0x0c237487d06f011dUL, 0x3e97f3b619f12766UL, 0x393fc6a828bc5faaUL, 0,
            // j = 42
            0x35744cb31feda78cUL, 0xd51b2aca2e438689UL, 0x32c841d8e6ceb86bUL, 0x3f3f5a212b953136UL, 0x3aa5c5acf37c6120UL, 0,
            // j = 43
            0x4d79cc59a565f332UL, 0x71eb5d0518dcfb61UL, 0xbb4691922423833aUL, 0xa8a0d67b5ebbd438UL, 0x3c0a6ce619afbecbUL, 0,
            // j = 44
            0x3e21fefaf4e7140aUL, 0x96f46153925cfd2bUL, 0x571dbd2a4f2e3c52UL, 0xb842564cf08621a5UL, 0x3d6f8890a605b281UL, 0,
            // j = 45
            0x7037e96476e27ea1UL, 0x5ae2b816bd048fedUL, 0x46fbc0ca0a5d9263UL, 0xf4b457396c3272d5UL, 0x3ed5515839e243bfUL, 0,
            // j = 46
            0xf02ac2ca018f9a65UL, 0x71db3636d8da33bdUL, 0xd1d6436a7360244dUL, 0xc419fbcd61c58ae0UL, 0x403c068f4ca7dbddUL, 0,
            // j = 47
            0xcce44f4b72496de5UL, 0xea09489358b99d4dUL, 0xb9280b5910db4a70UL, 0x3ad95a2158116eb8UL, 0x41a01481bb07e89aUL, 0,
            // j = 48
            0x48ac724ef318d23fUL, 0xd36005c2ff58849eUL, 0x684395b19458a3b0UL, 0x9edeca515af1ba99UL, 0x43055d3f5a3843d4UL, 0,
            // j = 49
            0xfb4d7cd146ac9f2bUL, 0x0ed324adafcce8e9UL, 0x464d45780c8593e4UL, 0xdfeabcb1886c91d4UL, 0x446b298c55709426UL, 0,
            // j = 50
            0xc97d68a97bd20cfdUL, 0xd44c71bb9a79af37UL, 0x4994a20c53d8f77cUL, 0x5551bdfb954de51bUL, 0x45d0a2d83402852eUL, 0,
            // j = 51
            0xcf5871a6c11bec80UL, 0x9241921b8781c8d4UL, 0xfcd58a061f918cf1UL, 0xd4d48175b1f60999UL, 0x4735ecbb7bd74be0UL, 0,
            // j = 52
            0xb4823503bd4fd9a8UL, 0xdc192068bd8d2bc4UL, 0xcbdb56aef0689e07UL, 0xdd64738731cda11eUL, 0x489b2f2ff7bcda66UL, 0,
            // j = 53
            0xb5ded72e06d41eb6UL, 0x9bb89696d9b7cf1cUL, 0xf77c9570b899c4e3UL, 0x5aa0e883a6eda943UL, 0x4a0097d052ed8c05UL, 0,
            // j = 54
            0xddab3b3a2e3bf5deUL, 0x2bec239ff875f670UL, 0xd57d42bb37fb8edbUL, 0x53ed5991b032a4ebUL, 0x4b665b83154c8657UL, 0,
            // j = 55
            0xac519e91759a2e1cUL, 0xd0fdf1d8cd84553bUL, 0x6c24cfb240db06c4UL, 0xc968f12d1de4d873UL, 0x4cccb8bf370f638fUL, 0,
            // j = 56
            0x130889f279315788UL, 0x1ef803a4227bd644UL, 0x2121c9968755f6f1UL, 0xf2bad5ef685bea78UL, 0x4e310dfc7e411046UL, 0,
            // j = 57
            0xa03656fa715c3883UL, 0xa9270b7ef8e88ebdUL, 0x550185ce80dcfecdUL, 0xf6847065ac7a7aa7UL, 0x4f970b141dce2e81UL, 0,
            // j = 58
            0xeb9f36e3478889d3UL, 0x4a12abd75df1ecf7UL, 0x7c7ce1fdb58d81efUL, 0x84bdaa4b7ad370d0UL, 0x50fbe3e30fc44f66UL, 0,
            // j = 59
            0x627d4f62ad02808bUL, 0xb1ea7aeab262dcf3UL, 0xac10241f5263e947UL, 0x7957fee19a27ce76UL, 0x526151167a260453UL, 0,
            // j = 60
            0xe37f0c6934d62608UL, 0x7843ef917ade64aeUL, 0x544a56f63dad0a95UL, 0x3c5874fd8b1301f7UL, 0x53c6b020be9fee8aUL, 0,
            // j = 61
            0x6b2fe8011bb87592UL, 0xc5b7cbc38fac26d4UL, 0x8db1f8c41a6cd9a0UL, 0xd7ae3d436b49cc09UL, 0x552c2e92d3f0c173UL, 0,
            // j = 62
            0x6049d7609958e256UL, 0xfc8faaed4664aafeUL, 0xe62f657b1840e73eUL, 0x6e785810dc18495aUL, 0x5692079b0ac56864UL, 0,
            // j = 63
            0xdea0822e6fcabf5aUL, 0xa67be1a0aeba5974UL, 0x80df10c66d9ad721UL, 0xaee8e2ff604ca4e4UL, 0x57f74397d7972d8cUL, 0,
            // j = 64
            0x2e99fd9004895bf1UL, 0x2c5e4e04ffa77d51UL, 0x198ecdeb7689d1daUL, 0x31c2c2097ded5b97UL, 0x595c947041a83282UL, 0,
            // j = 65
            0x9d8e6e05caef973aUL, 0xa21b7978644cb042UL, 0xe37cc7b3f5012febUL, 0xe32d4cbd3f985c39UL, 0x5ac237b03135c03fUL, 0,
            // j = 66
            0xa01b33e03be0fe10UL, 0x34bf31294901fcf5UL, 0xc9b777070ae86e75UL, 0xe115be404cca90a2UL, 0x5c27975ad61bcaf3UL, 0,
            // j = 67
            0x9c4590074292f22cUL, 0xdae52ba7e450bddcUL, 0x5da1f017c5607398UL, 0x3b9a27849aaf3833UL, 0x5d8cc3ba42653852UL, 0,
            // j = 68
            0xbbe9d89670b64e34UL, 0xc116904da21277eaUL, 0xcb425146d6b5bc8eUL, 0x674e52bc09754ee7UL, 0x5ef22e0c33796cf3UL, 0,
            // j = 69
            0xaf80d291228261abUL, 0x33363d9bc6f82152UL, 0x1e11556bff0ca51fUL, 0x28a52e68b4dc3983UL, 0x60579dc248eaa1c8UL, 0,
            // j = 70
            0x67c35a55fff73990UL, 0x6519388c3c9a5119UL, 0x93de87eefa765ee5UL, 0xf3ba200c61a03d53UL, 0x61bd0b619139c80dUL, 0,
            // j = 71
            0x77ccbaef553260bdUL, 0xb5d407d4703f9679UL, 0x30c0c549e83434bfUL, 0x571f0d28a5438d2cUL, 0x632281cb0a93a34eUL, 0,
            // j = 72  (last row for phi0[72])
            0x28a5043cc71a026fUL, 0x0105df531d89cd91UL, 0x948127044533e63aUL, 0x62633145c06e0e68UL, 0x6487ed5110b4611aUL, 0,
        ];

        // ============================================================================
        //  jget : range reduction
        // ============================================================================

        static UInt64 JGet(UInt64 x) {
            unchecked {
                UInt64 z = (0x3FFFUL << 48) - x;
                UInt64 mz = z << 16;
                int e = (int)(z >> 48);
                int nz = (int)UInt64.LeadingZeroCount(mz) * (e == 0 ? 1 : 0);
                mz <<= nz + (e == 0 ? 1 : 0);
                e -= nz;
                long lz = (((long)e << 4) | (long)(mz >> 60)) + 161;
                lz *= (lz >= 0 ? 1 : 0);
                long j = Ind[(int)(lz & 0xFF)] * (lz < 256 ? 1 : 0);
                long tz = ((long)e << 11) | (long)(mz >> 53);
                return (UInt64)(j + (tz < Sth[(int)j] ? 1 : 0));
            }
        }

        // ============================================================================
        //  omx2v2 / omx2v3 : 1 - x^2 helpers
        // ============================================================================

        // Approximated normalize 1-x^2
        static int Omx2v2(Span<UInt64> X2, int s, ReadOnlySpan<UInt64> x) {
            unchecked {
                SqrU(X2, x);
                X2[0] = X2[1] << 1 << (~s & 63) | X2[0] >> s;
                X2[0] = ~X2[0];
                X2[1] = X2[2] << 1 << (~s & 63) | X2[1] >> s;
                X2[1] = ~X2[1];
                X2[2] = X2[3] << 1 << (~s & 63) | X2[2] >> s;
                X2[2] = ~X2[2];
                X2[3] = X2[3] >> s;
                X2[3] = ~X2[3];
                int e = 1;
                if (Misc.Likely(X2[3] != 0)) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    X2[3] = X2[3] << lk | X2[2] >> 1 >> (~lk & 63);
                    X2[2] = X2[2] << lk | X2[1] >> 1 >> (~lk & 63);
                    X2[1] = X2[1] << lk | X2[0] >> 1 >> (~lk & 63);
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    UInt64 c;
                    X2[0] = AddCarry(X2[0], 1, 0, out c);
                    X2[1] = AddCarry(X2[1], 0, c, out c);
                    X2[2] = AddCarry(X2[2], 0, c, out c);
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    X2[3] = X2[2] << lk | X2[1] >> 1 >> (~lk & 63);
                    X2[2] = X2[1] << lk | X2[0] >> 1 >> (~lk & 63);
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // Exact normalized 1 - x^2
        static int Omx2v3(Span<UInt64> X2, int s, ReadOnlySpan<UInt64> x) {
            unchecked {
                SqrU(X2, x);
                X2[0] = X2[1] << 1 << (~s & 63) | X2[0] >> s;
                X2[1] = X2[2] << 1 << (~s & 63) | X2[1] >> s;
                X2[2] = X2[3] << 1 << (~s & 63) | X2[2] >> s;
                X2[3] = X2[3] >> s;

                UInt64 c;
                X2[0] = SubBorrow(0, X2[0], 0, out c);
                X2[1] = SubBorrow(0, X2[1], c, out c);
                X2[2] = SubBorrow(0, X2[2], c, out c);
                X2[3] = SubBorrow(0, X2[3], c, out c);

                int e = 1;
                if (Misc.Likely(X2[3] != 0)) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    X2[3] = X2[3] << lk | X2[2] >> 1 >> (~lk & 63);
                    X2[2] = X2[2] << lk | X2[1] >> 1 >> (~lk & 63);
                    X2[1] = X2[1] << lk | X2[0] >> 1 >> (~lk & 63);
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    X2[3] = X2[2] << lk | X2[1] >> 1 >> (~lk & 63);
                    X2[2] = X2[1] << lk | X2[0] >> 1 >> (~lk & 63);
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // ============================================================================
        //  getcos : sqrt(1 - x^2)
        //  sq : 5 words (out),  x : 2 words
        // ============================================================================

        static readonly UInt64[] Rsqrt2 = [~0UL, 0xb504f333f9de6484UL];

        static int GetCos(Span<UInt64> sq, int ex, ReadOnlySpan<UInt64> x) {
            unchecked {
                Span<UInt64> x2 = stackalloc UInt64[4];
                int e = Omx2v3(x2, 2 * (ex - 1), x);
                UInt64 rx = x2[3] << 1 | x2[2] >> 63;
                UInt64 r = Rsqrt9(rx);
                r = (UInt64)((UInt128)r * Rsqrt2[e & 1] >> 64);
                MulFullU4U1(sq, r, x2);
                Span<UInt64> h = stackalloc UInt64[6];
                MulFullU5U1(h, r, sq);
                ShrN(6, h, 2);
                Int64 msk = (Int64)h[4];
                msk >>= 63;
                UInt64 mskU = (UInt64)msk;
                h[4] ^= mskU; h[3] ^= mskU; h[2] ^= mskU; h[1] ^= mskU; h[0] ^= mskU;

                Span<UInt64> h2s = stackalloc UInt64[5];
                SqrhU4(h2s.Slice(1), h.Slice(1));
                Span<UInt64> h4s = stackalloc UInt64[2];
                SqrhU2(h4s, h2s.Slice(3));
                Span<UInt64> h3s = stackalloc UInt64[5];
                MulHiU3(h3s.Slice(2), h.Slice(2, 3), h2s.Slice(2, 3));
                MulU4x3(h2s.Slice(1));
                MulU3x5(h3s.Slice(2));
                UInt128 t4u = ((UInt128)h4s[1] << 64) | h4s[0];
                t4u += t4u * 3 >> 5;
                Span<UInt64> t4 = stackalloc UInt64[5] { 0, 0, 0, (UInt64)t4u, (UInt64)(t4u >> 64) };

                h2s[0] = 0;
                ShrN(5, h2s, 62 - (e & 1));
                h3s[0] = h3s[1] = 0;
                ShrN(5, h3s, 59 + 64 - 2 * (e & 1));
                ShrN(5, t4, 56 + 128 - 3 * (e & 1));

                if (msk != 0) {
                    AddU5(h, h, h2s);
                    AddU5(h, h, h3s);
                    AddU5(h, h, t4);
                } else {
                    SubU5(h, h, h2s);
                    AddU5(h, h, h3s);
                    SubU5(h, h, t4);
                }

                MulFullU5U1(h, r, h);

                Span<UInt64> x2l = stackalloc UInt64[5] { 0, x2[0], x2[1], x2[2], x2[3] };
                MulHiU5(h, h.Slice(1), x2l);
                ShrN(5, h, 62 - (e & 1));

                if (msk == 0) {
                    SubU5(sq, sq, h);
                } else {
                    AddU5(sq, sq, h);
                }
                return e;
            }
        }

        // ============================================================================
        //  evalpoly / cp table
        // ============================================================================

        static ReadOnlySpan<UInt64> Cp => [
            0x1343996b9f42b9f5UL, 0x255e6e351770584dUL, 0x00000000000000a3UL, 0x5e2111cba47a2b05UL, 0x000000000005717dUL,
            0xa97f20b758a855cdUL, 0x000000002ea1bcc9UL, 0x889c99395996e6ceUL, 0x00000190cb77f60cUL, 0xc7476c854bade5bfUL,
            0x000d8137abd89d89UL, 0x97b4ea2813d93845UL, 0x74f4aa383759f229UL, 0x5abb1888e58be523UL, 0x5f1f6db6db6db6dbUL,
            0x00000000000003f9UL, 0x9a1160a9ab2539ceUL, 0xa8ba2e8ba2e8ba2eUL, 0x000000000022bdd3UL, 0x72ec43b868c4b3c0UL,
            0xf7bdef7bdef7bdefUL, 0x0000000131683bdeUL, 0x4b2852d709bf2295UL, 0x58469ee58469ee58UL, 0x00000a8dd18469eeUL,
            0x2d86e53634cafb09UL, 0x684bda12f684bda1UL, 0x005e0b7684bda12fUL, 0x151d85735049738fUL, 0xe147ae147ae147aeUL,
            0x4d0c7ae147ae147aUL, 0x0000000000000003UL, 0x9ba6f1b2735cae39UL, 0x6f4de9bd37a6f4deUL, 0xbd37a6f4de9bd37aUL,
            0x0000000000001df3UL, 0xcf46c00a8ed8a2e2UL, 0x3cf3cf3cf3cf3cf3UL, 0xf3cf3cf3cf3cf3cfUL, 0x000000000112ef3cUL,
            0x86baeba7afbb9dd6UL, 0xbca1af286bca1af2UL, 0xa1af286bca1af286UL, 0x00000009fef286bcUL, 0xe1e21d9d6b73053dUL,
            0xe1e1e1e1e1e1e1e1UL, 0xe1e1e1e1e1e1e1e1UL, 0x00005ea1e1e1e1e1UL, 0x33332cfa4ccaad37UL, 0x3333333333333333UL,
            0x3333333333333333UL, 0x0393333333333333UL, 0x89d89e04e6327ae5UL, 0xd89d89d89d89d89dUL, 0x9d89d89d89d89d89UL,
            0x89d89d89d89d89d8UL, 0x0000000000000023UL, 0x8ba2e8b369ee2b14UL, 0xe8ba2e8ba2e8ba2eUL, 0x2e8ba2e8ba2e8ba2UL,
            0xa2e8ba2e8ba2e8baUL, 0x0000000000016e8bUL, 0x8e38e38e78e717bcUL, 0x38e38e38e38e38e3UL, 0xe38e38e38e38e38eUL,
            0x8e38e38e38e38e38UL, 0x000000000f8e38e3UL, 0x6db6db6db566fac3UL, 0xb6db6db6db6db6dbUL, 0xdb6db6db6db6db6dUL,
            0x6db6db6db6db6db6UL, 0x000000b6db6db6dbUL, 0x99999999999e1925UL, 0x9999999999999999UL, 0x9999999999999999UL,
            0x9999999999999999UL, 0x0009999999999999UL, 0xaaaaaaaaaaaaa521UL, 0xaaaaaaaaaaaaaaaaUL, 0xaaaaaaaaaaaaaaaaUL,
            0xaaaaaaaaaaaaaaaaUL, 0xaaaaaaaaaaaaaaaaUL,
        ];

        static void EvalPoly(Span<UInt64> f, ReadOnlySpan<UInt64> t2) {
            unchecked {
                ReadOnlySpan<UInt64> ck = Cp;
                int ci = 0;
                f[0] = ck[ci];
                MulHiU1(f, f, t2.Slice(4, 1)); AddU1(f, ck.Slice(++ci, 1), f);
                f[1] = ck[ci + 1];
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                MulHiU2(f, f, t2.Slice(3, 2)); AddU2(f, ck.Slice(ci += 2, 2), f);
                f[2] = ck[ci + 2];
                MulHiU3(f, f, t2.Slice(2, 3)); AddU3(f, ck.Slice(ci += 3, 3), f);
                MulHiU3(f, f, t2.Slice(2, 3)); AddU3(f, ck.Slice(ci += 3, 3), f);
                MulHiU3(f, f, t2.Slice(2, 3)); AddU3(f, ck.Slice(ci += 3, 3), f);
                MulHiU3(f, f, t2.Slice(2, 3)); AddU3(f, ck.Slice(ci += 3, 3), f);
                MulHiU3(f, f, t2.Slice(2, 3)); AddU3(f, ck.Slice(ci += 3, 3), f);
                f[3] = ck[ci + 3];
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                MulHiU4(f, f, t2.Slice(1, 4)); AddU4(f, ck.Slice(ci += 4, 4), f);
                f[4] = ck[ci + 4];
                MulHiU5(f, f, t2.Slice(0, 5)); AddU5(f, ck.Slice(ci += 5, 5), f);
                MulHiU5(f, f, t2.Slice(0, 5)); AddU5(f, ck.Slice(ci += 5, 5), f);
                MulHiU5(f, f, t2.Slice(0, 5)); AddU5(f, ck.Slice(ci += 5, 5), f);
                MulHiU5(f, f, t2.Slice(0, 5)); AddU5(f, ck.Slice(ci += 5, 5), f);
                MulHiU5(f, f, t2.Slice(0, 5)); AddU5(f, ck.Slice(ci += 5, 5), f);
            }
        }

        static void Cpu5(Span<UInt64> o, ReadOnlySpan<UInt64> a) {
            for (int i = 0; i < 5; i++) o[i] = a[i];
        }

        // ============================================================================
        //  cr_acosq : main entry point
        //  Ports __float128 → (UInt64 lo, UInt64 hi) pair representation.
        // ============================================================================

        static ReadOnlySpan<UInt64> CrAcosqCoef => [
            0xaaaaaaaaaaaaaaa9UL, 0xaaaaaaaaaaaaaaaaUL,
            0x333333333333337aUL, 0x0013333333333333UL,
            0xb6db6db6db6dae36UL, 0x000002db6db6db6dUL,
            0x71c71c71c71cfc25UL, 0x000000007c71c71cUL,
            0x2e8ba2e8ba29804eUL, 0x000000000016e8baUL,
            0x3b13b13b13ce93b6UL, 0x0000000000000471UL,
            0xe4cccccccc5f2a5aUL, 0x0000000000000000UL,
            0x002f50f0f1f806dcUL, 0x0000000000000000UL,
            0x000009fef0fec73aUL, 0x0000000000000000UL,
            0x0000000227286573UL, 0x0000000000000000UL,
        ];

        public static UInt64 Acos(UInt64 lo, UInt64 hi, MidpointRounding rounding, out UInt64 result_hi) {
            unchecked {
                // Note: The C source uses MXCSR. We map `MidpointRounding` onto the
                // rounding-mode branch used at the end (rnd = (rm==_MM_ROUND_UP)).
                // `_MM_ROUND_NEAREST` ↔ MidpointRounding.ToEven etc.
                bool roundUp = rounding == MidpointRounding.ToPositiveInfinity;
                bool roundNearest = rounding == MidpointRounding.ToEven;
                // For other modes we fall through to the C code's default branch
                // (rnd from sign-based logic in the caller).
                return AcosInternal(lo, hi, roundUp, roundNearest, out result_hi);
            }
        }

        public static UInt64 Acos(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Acos(lo, hi, MidpointRounding.ToEven, out result_hi);

        static UInt64 AcosInternal(UInt64 lo, UInt64 hi, bool roundUp, bool roundNearest, out UInt64 result_hi) {
            unchecked {
                UInt64 sign = hi >> 63;
                UInt64 xn = hi & 0x7FFFFFFFFFFFFFFFUL;
                long xnExp = (long)(xn >> 48);

                UInt128 X = ((UInt128)xn << 64) | lo;

                // |x| >= 1, Inf, NaN
                if (Misc.Unlikely(xnExp >= 0x3FFF)) {
                    if (lo == 0 && xn == (0x3FFFUL << 48)) {
                        // |x| == 1
                        if (sign != 0) {
                            // acos(-1) = pi
                            // TODO(port): 0x1.921fb54442d18469898cc51701b8p+1q
                            // High 128 bits: 0x4000921FB54442D18469898CC51701B8UL (approx).
                            // Fill in from the C source's binary representation.
                            UInt128 pi = ((UInt128)0x4000921FB54442D1UL << 64) | 0x8469898CC51701B8UL;
                            UInt64 rnd1 = (roundNearest ? 0UL : (roundUp ? 1UL : 0UL));
                            pi += rnd1;
                            result_hi = (UInt64)(pi >> 64);
                            return (UInt64)pi;
                        } else {
                            result_hi = 0;
                            return 0;
                        }
                    } else {
                        SByte xnan = GetClass(X);
                        if (xnan == 2) {
                            // sNaN -> qNaN
                            result_hi = hi | (1UL << 47);
                            return lo;
                        } else if (xnan == 3) {
                            // qNaN propagate
                            result_hi = hi;
                            return lo;
                        }
                        // |x| > 1 finite: domain error -> NaN
                        // (would set FE_INVALID + EDOM in C)
                        result_hi = hi | (1UL << 47);
                        return lo;
                    }
                }

                UInt64 j = JGet(xn);
                X |= (UInt128)1 << 112;  // set implicit bit (bit 48 of hi = bit 112 of X)
                X <<= 15;
                UInt128 t = X;
                int nz = 0x3FFF - (int)xnExp;

                Span<UInt64> xc = stackalloc UInt64[3];

                if (Misc.Likely(j != 0)) {
                    Span<UInt64> X2 = stackalloc UInt64[4];
                    int e = Omx2v2(X2, 2 * nz - 2, XcHiLo(X));
                    UInt64 rx = X2[3] << 1 | X2[2] >> 63;
                    UInt64 r = Rsqrt9(rx);
                    r = (UInt64)((UInt128)r * Rsqrt2[e & 1] >> 64);
                    Span<UInt64> SX = stackalloc UInt64[3];
                    MulHiU3(SX, X2.Slice(1, 3), FromU64(r));
                    Span<UInt64> H = stackalloc UInt64[3];
                    MulHiU3(H, SX, FromU64(r));
                    const int koff = 2, rkoff = 64 - koff;
                    H[0] = H[0] >> koff | H[1] << rkoff;
                    H[1] = H[1] >> koff | H[2] << rkoff;
                    long hh = (long)H[1];
                    UInt64 h2 = (UInt64)MultiplyHigh(hh, hh);
                    h2 += h2 >> 1;
                    UInt128 Hh = ((UInt128)H[1] << 64) | H[0];
                    int lk = (e & 1) + koff, rk = (64 - lk) & 63;
                    UInt128 H2 = (UInt128)(h2 >> rk) << 64 | (UInt128)(h2 << lk);
                    Hh -= H2;
                    Int128 D = MhIU((Int128)(((UInt128)SX[2] << 64) | SX[1]), Hh);
                    UInt64 D3s = (UInt64)(D >> 127);
                    Span<UInt64> D3 = stackalloc UInt64[3] { (UInt64)D, (UInt64)(D >> 64), D3s };
                    D3[2] = D3[2] << lk | D3[1] >> rk;
                    D3[1] = D3[1] << lk | D3[0] >> rk;
                    D3[0] = D3[0] << lk;

                    SubU3(SX, SX, D3);

                    if (sign != 0 || j < 71) {
                        X >>= nz & 63;
                        MulHiU3U2(xc, XcHiLo(ref X), Cth.Slice((int)j * 5 + 2, 3));
                        UInt64 sj = Pth[(int)j];
                        int sp = 43 - (e >> 1);
                        if (Misc.Likely(sp >= 0)) {
                            sj <<= sp;
                            MulHiU5U1(SX, sj, SX);
                        } else {
                            MulHiU5U1(SX, sj, SX);
                            rk = -sp & 63;
                            lk = sp & 63;
                            SX[0] = SX[0] >> rk | SX[1] << lk;
                            SX[1] = SX[1] >> rk | SX[2] << lk;
                            SX[2] = SX[2] >> rk;
                        }
                        SubU3(xc, xc, SX);
                        nz = (int)UInt64.LeadingZeroCount(xc[2]);
                        t = ((UInt128)(xc[2] << nz | xc[1] >> (-nz & 63)) << 64)
                          | (UInt128)(xc[1] << nz | xc[0] >> (-nz & 63));
                    } else {
                        nz = e >> 1;
                        xc[0] = SX[0];
                        xc[1] = SX[1];
                        xc[2] = SX[2];
                        t = ((UInt128)SX[2] << 64) | SX[1];
                    }
                } else {
                    if (Misc.Likely(nz < 64)) {
                        xc[0] = (UInt64)X << (-nz & 63);
                        xc[1] = (UInt64)(X >> 64) << (-nz & 63) | (UInt64)X >> nz;
                        xc[2] = (UInt64)(X >> 64) >> nz;
                    } else if (nz < 128) {
                        xc[0] = (UInt64)(X >> 64) << 1 << (~nz & 63) | (UInt64)X >> (nz & 63);
                        xc[1] = (UInt64)(X >> 64) >> (nz & 63);
                        xc[2] = 0;
                    } else if (nz < 192) {
                        xc[0] = (UInt64)(X >> 64) >> (nz & 63);
                        xc[2] = xc[1] = 0;
                    } else {
                        xc[2] = xc[1] = xc[0] = 0;
                    }
                }
                UInt128 t2 = SqrhU(t);
                UInt128 t3 = MulHi128Approx(t, t2);
                int s2 = 2 * (nz - 6);
                if (Misc.Likely(s2 < 128))
                    t2 >>= s2;
                else
                    t2 = 0;

                UInt64 t2h = (UInt64)(t2 >> 64);
                UInt64 fl = CrAcosqCoef[9 * 2 + 0];
                fl = CrAcosqCoef[8 * 2 + 0] + MultiplyHigh(t2h, fl);
                fl = CrAcosqCoef[7 * 2 + 0] + MultiplyHigh(t2h, fl);
                fl = CrAcosqCoef[6 * 2 + 0] + MultiplyHigh(t2h, fl);
                UInt128 f = ((UInt128)CrAcosqCoef[5 * 2 + 1] << 64) | (CrAcosqCoef[5 * 2 + 0] + MultiplyHigh(t2h, fl));
                for (int i = 5; i > 0;) {
                    i--;
                    f = Uq(CrAcosqCoef.Slice(i * 2, 2)) + MulHi128Approx(t2, f);
                }
                f = MulHi128Approx(t3, f);
                Span<UInt64> f3 = stackalloc UInt64[3];
                f3[2] = (UInt64)(f >> 64);
                f3[1] = (UInt64)f;
                f3[0] = 0;

                UInt128 v;
                UInt64 rnd;
                long newXn;

                if (sign != 0 || j < 71) {
                    int sf = 3 * nz + 1;
                    if (Misc.Likely(sf < 64)) {
                        f3[0] = f3[1] << (-sf & 63);
                        f3[1] = f3[1] >> sf | f3[2] << (-sf & 63);
                        f3[2] = f3[2] >> sf;
                    } else if (sf < 128) {
                        f3[0] = f3[1] >> (sf & 63) | f3[2] << 1 << (~sf & 63);
                        f3[1] = f3[2] >> (sf & 63);
                        f3[2] = 0;
                    } else if (sf < 192) {
                        f3[0] = f3[2] >> (sf & 63);
                        f3[2] = f3[1] = 0;
                    } else {
                        f3[2] = f3[1] = f3[0] = 0;
                    }

                    xc[0] = xc[0] >> 1 | xc[1] << 63;
                    xc[1] = xc[1] >> 1 | xc[2] << 63;
                    xc[2] = xc[2] >> 1;
                    AddU3(xc, xc, Phi0.Slice((int)j * 6 + 2, 3));
                    AddU3(xc, xc, f3);

                    UInt64 xsgnBit = sign;
                    xc[0] ^= xsgnBit; xc[1] ^= xsgnBit; xc[2] ^= xsgnBit;
                    SubU3(xc, Phi0.Slice(72 * 6 + 2, 3), xc);

                    int k = (int)UInt64.LeadingZeroCount(xc[2]);
                    rnd = (xc[1] >> (14 - k)) & 1;
                    newXn = 0x3FFF - k;

                    sf = Math.Min(sf, 60);
                    UInt64 Eps = 1UL << (69 - sf);
                    UInt128 msk = ((UInt128)(~0UL >> (k + 0x31 + (roundNearest ? 1 : 0))) << 64) | ~0UL;
                    UInt128 tl = ((UInt128)xc[1] << 64) | xc[0];
                    tl += Eps;
                    tl &= msk;
                    if (tl < 2 * Eps) return AsAcosqAccurate(lo, hi, roundUp, roundNearest, out result_hi);

                    UInt64 vHi = xc[2] >> (15 - k);
                    UInt64 vLo = xc[1] >> (15 - k) | xc[2] << (49 + k);
                    v = ((UInt128)vHi << 64) | vLo;
                } else {
                    int sf = 2 * nz;
                    if (Misc.Likely(sf < 64)) {
                        f3[0] = f3[1] << (-sf & 63);
                        f3[1] = f3[1] >> sf | f3[2] << (-sf & 63);
                        f3[2] = f3[2] >> sf;
                    } else {
                        f3[0] = f3[1] >> (sf & 63) | f3[2] << 1 << (~sf & 63);
                        f3[1] = f3[2] >> (sf & 63);
                        f3[2] = 0;
                    }
                    AddU3(xc, xc, f3);

                    UInt64 vLo = xc[1];
                    UInt64 vHi = xc[2];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1;
                    UInt128 vv = ((UInt128)vHi << 64) | vLo;
                    vv >>= 14 + k;
                    vv &= ~(UInt128)0 >> 16 | (UInt128)0 << 64;  // mask low 48 bits of hi half
                    v = vv;
                    newXn = xnExp + k - nz;

                    sf = Math.Min(sf, 60);
                    UInt64 Eps = 1UL << (68 - sf);
                    UInt128 msk = ((UInt128)(~0UL >> (k + 0x31 + (roundNearest ? 1 : 0))) << 64) | ~0UL;
                    UInt128 tl = ((UInt128)xc[1] << 64) | xc[0];
                    tl += Eps;
                    tl &= msk;
                    if (tl < 2 * Eps) return AsAcosqAccurate(lo, hi, roundUp, roundNearest, out result_hi);
                }

                if (Misc.Unlikely(!roundNearest))
                    rnd = roundUp ? 1UL : 0UL;

                UInt128 dv = ((UInt128)(UInt64)newXn << 48) | rnd;
                v += dv;

                result_hi = (UInt64)(v >> 64);
                return (UInt64)v;
            }
        }

        // Helper to slice X into a 2-word Span view (lo, hi).
        static ReadOnlySpan<UInt64> XcHiLo(ref UInt128 x) {
            // Allocates each call; only used in cold-ish paths, so acceptable.
            return [(UInt64)x, (UInt64)(x >> 64)];
        }

        static ReadOnlySpan<UInt64> FromU64(in UInt64 v) => new (in v);

        // ============================================================================
        //  as_acosq_accurate : high-precision fallback
        // ============================================================================

        static UInt64 AsAcosqAccurate(UInt64 lo, UInt64 hi, bool roundUp, bool roundNearest, out UInt64 result_hi) {
            unchecked {
                UInt64 sign = hi >> 63;
                UInt64 xn = hi & 0x7FFFFFFFFFFFFFFFUL;
                long xnExp = (long)(xn >> 48);

                UInt128 X = ((UInt128)xn << 64) | lo;

                UInt64 j = JGet(xn);
                X |= (UInt128)1 << 112;
                X <<= 15;
                Span<UInt64> t = stackalloc UInt64[5];
                int nz = 0x3FFF - (int)xnExp;
                Span<UInt64> xc = stackalloc UInt64[5];

                if (j != 0) {
                    Span<UInt64> sq = stackalloc UInt64[5];
                    int e = GetCos(sq, nz, XcHiLo(X));
                    if (sign != 0 || j < 71) {
                        X >>= nz & 63;
                        MulHiU5U2(xc, XcHiLo(X), Cth.Slice((int)j * 5, 5));
                        UInt64 sj = Pth[(int)j];
                        int sp = 43 - (e >> 1);
                        if (Misc.Likely(sp >= 0)) sj <<= sp;
                        MulHiU5U1(sq, sj, sq);
                        if (Misc.Unlikely(sp < 0)) ShrN(5, sq, -sp);
                        SubU5(xc, xc, sq);
                        Cpu5(t, xc);
                        for (int i = 4; i >= 0; i--) if (t[i] != 0) { nz = (int)UInt64.LeadingZeroCount(t[i]) + (4 - i) * 64; break; }
                        ShlN(5, t, nz);
                    } else {
                        nz = e >> 1;
                        Cpu5(xc, sq);
                        Cpu5(t, sq);
                    }
                } else {
                    t[0] = t[1] = t[2] = xc[0] = xc[1] = xc[2] = 0;
                    xc[3] = t[3] = (UInt64)X;
                    xc[4] = t[4] = (UInt64)(X >> 64);
                    ShrN(5, xc, nz);
                }

                Span<UInt64> t2 = stackalloc UInt64[5];
                SqrhU5(t2, t);
                Span<UInt64> t3 = stackalloc UInt64[5];
                MulHiU5(t3, t, t2);
                int s2 = 2 * (nz - 6) - 1;
                ShrN(5, t2, s2);
                Span<UInt64> f = stackalloc UInt64[5];
                EvalPoly(f, t2);
                MulHiU5(f, t3, f);

                UInt128 v;
                UInt64 rnd;
                long newXn;

                if (sign != 0 || j < 71) {
                    int sf = 3 * nz + 1;
                    ShrN(5, f, sf);
                    ShrN(5, xc, 1);
                    AddU5(xc, Phi0.Slice((int)j * 6, 5), xc);
                    AddU5(xc, xc, f);
                    if (sign != 0) {
                        AddU5(xc, Phi0.Slice(72 * 6, 5), xc);
                    } else {
                        SubU5(xc, Phi0.Slice(72 * 6, 5), xc);
                    }
                    int k = (int)UInt64.LeadingZeroCount(xc[4]);
                    rnd = (xc[3] >> (14 - k)) & 1;
                    newXn = 0x3FFF - k;
                    UInt64 vHi = xc[4] >> (15 - k);
                    UInt64 vLo = xc[3] >> (15 - k) | xc[4] << (49 + k);
                    v = ((UInt128)vHi << 64) | vLo;
                } else {
                    int sf = 2 * nz;
                    ShrN(5, f, sf);
                    AddU5(xc, xc, f);
                    UInt64 vLo = xc[3];
                    UInt64 vHi = xc[4];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1;
                    UInt128 vv = ((UInt128)vHi << 64) | vLo;
                    vv >>= 14 + k;
                    vv &= ((UInt128)~0UL << 64) >> 16;
                    v = vv;
                    newXn = xnExp + k - nz;
                }

                if (Misc.Unlikely(!roundNearest))
                    rnd = roundUp ? 1UL : 0UL;

                UInt128 dv = ((UInt128)(UInt64)newXn << 48) | rnd;
                v += dv;

                result_hi = (UInt64)(v >> 64);
                return (UInt64)v;
            }
        }
    }
}