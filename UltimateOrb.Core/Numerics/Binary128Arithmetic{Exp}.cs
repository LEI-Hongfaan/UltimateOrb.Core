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
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    partial class Binary128Arithmetic {

        // ─── addu6u6 : o += b ───────────────────────────────────────────────────────
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(ref InlineArray6<UInt64> o, in InlineArray6<UInt64> b) {
            unchecked {
                nuint c;
                o[0] = AddWithCarry(o[0], b[0], 0, out c);
                o[1] = AddWithCarry(o[1], b[1], c, out c);
                o[2] = AddWithCarry(o[2], b[2], c, out c);
                o[3] = AddWithCarry(o[3], b[3], c, out c);
                o[4] = AddWithCarry(o[4], b[4], c, out c);
                o[5] = AddWithCarry(o[5], b[5], c, out _);
            }
        }

        // ─── mhu7xu2 : o = high 7 words of (a (7w) * b (2w)) , i.e. product >> 128 ──
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray7<UInt64> o, in InlineArray7<UInt64> a, UInt128 b) {
            unchecked {
                UInt64 b0 = (UInt64)b, b1 = (UInt64)(b >> 64);
                UInt128 a0b1 = (UInt128)a[0] * b1, a1b1 = (UInt128)a[1] * b1,
                        a2b1 = (UInt128)a[2] * b1, a3b1 = (UInt128)a[3] * b1,
                        a4b1 = (UInt128)a[4] * b1, a5b1 = (UInt128)a[5] * b1,
                        a6b1 = (UInt128)a[6] * b1;
                nuint c0;
                UInt64 o0 = AddWithCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                UInt64 o1 = AddWithCarry((UInt64)a2b1, (UInt64)(a1b1 >> 64), c0, out c0);
                UInt64 o2 = AddWithCarry((UInt64)a3b1, (UInt64)(a2b1 >> 64), c0, out c0);
                UInt64 o3 = AddWithCarry((UInt64)a4b1, (UInt64)(a3b1 >> 64), c0, out c0);
                UInt64 o4 = AddWithCarry((UInt64)a5b1, (UInt64)(a4b1 >> 64), c0, out c0);
                UInt64 o5 = AddWithCarry((UInt64)a6b1, (UInt64)(a5b1 >> 64), c0, out c0);
                UInt64 o6 = AddWithCarry(0, (UInt64)(a6b1 >> 64), c0, out _);

                UInt128 a1b0 = (UInt128)a[1] * b0, a2b0 = (UInt128)a[2] * b0,
                        a3b0 = (UInt128)a[3] * b0, a4b0 = (UInt128)a[4] * b0,
                        a5b0 = (UInt128)a[5] * b0, a6b0 = (UInt128)a[6] * b0;
                nuint c1;
                UInt64 t;
                t = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), 0, out c1);
                o0 = AddWithCarry(o0, t, 0, out c0);
                t = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c1, out c1);
                o1 = AddWithCarry(o1, t, c0, out c0);
                t = AddWithCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c1, out c1);
                o2 = AddWithCarry(o2, t, c0, out c0);
                t = AddWithCarry((UInt64)a5b0, (UInt64)(a4b0 >> 64), c1, out c1);
                o3 = AddWithCarry(o3, t, c0, out c0);
                t = AddWithCarry((UInt64)a6b0, (UInt64)(a5b0 >> 64), c1, out c1);
                o4 = AddWithCarry(o4, t, c0, out c0);
                t = AddWithCarry(0, (UInt64)(a6b0 >> 64), c1, out c1);
                o5 = AddWithCarry(o5, t, c0, out c0);
                o6 = AddWithCarry(o6, 0, c0, out _);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4; o[5] = o5; o[6] = o6;
            }
        }

        // ─── mhu4xu2 : o = high 4 words of (a (4w) * b (2w)) ────────────────────────
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray4<UInt64> o, in InlineArray4<UInt64> a, UInt128 b) {
            unchecked {
                UInt64 b0 = (UInt64)b, b1 = (UInt64)(b >> 64);
                UInt128 a0b1 = (UInt128)a[0] * b1, a1b1 = (UInt128)a[1] * b1,
                        a2b1 = (UInt128)a[2] * b1, a3b1 = (UInt128)a[3] * b1;
                nuint c0;
                UInt64 o0 = AddWithCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                UInt64 o1 = AddWithCarry((UInt64)a2b1, (UInt64)(a1b1 >> 64), c0, out c0);
                UInt64 o2 = AddWithCarry((UInt64)a3b1, (UInt64)(a2b1 >> 64), c0, out c0);
                UInt64 o3 = AddWithCarry(0, (UInt64)(a3b1 >> 64), c0, out c0);

                UInt128 a1b0 = (UInt128)a[1] * b0, a2b0 = (UInt128)a[2] * b0, a3b0 = (UInt128)a[3] * b0;
                nuint c1;
                UInt64 t;
                t = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), 0, out c1);
                o0 = AddWithCarry(o0, t, 0, out c0);
                t = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c1, out c1);
                o1 = AddWithCarry(o1, t, c0, out c0);
                t = AddWithCarry(0, (UInt64)(a3b0 >> 64), c1, out c1);
                o2 = AddWithCarry(o2, t, c0, out c0);
                o3 = AddWithCarry(o3, 0, c0, out _);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3;
            }
        }

        // ─── mhu3xu2 : o = high 3 words of (a (3w) * b (2w)) ────────────────────────
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray3<UInt64> o, in InlineArray3<UInt64> a, UInt128 b) {
            unchecked {
                UInt64 b0 = (UInt64)b, b1 = (UInt64)(b >> 64);
                UInt128 a1b0 = (UInt128)a[1] * b0, a2b0 = (UInt128)a[2] * b0;
                UInt128 a0b1 = (UInt128)a[0] * b1, a1b1 = (UInt128)a[1] * b1, a2b1 = (UInt128)a[2] * b1;
                nuint c0, c1;
                UInt64 t;
                UInt64 o0 = AddWithCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                UInt64 o1 = AddWithCarry((UInt64)a2b1, (UInt64)(a1b1 >> 64), c0, out c0);
                UInt64 o2 = AddWithCarry(0, (UInt64)(a2b1 >> 64), c0, out c0);
                t = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), 0, out c1);
                o0 = AddWithCarry(o0, t, 0, out c0);
                t = AddWithCarry(0, (UInt64)(a2b0 >> 64), c1, out c1);
                o1 = AddWithCarry(o1, t, c0, out c0);
                o2 = AddWithCarry(o2, 0, c0, out _);
                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        // ─── mhu6u6u6 : o = high 6 words of (a (6w) * b (6w)) ───────────────────────
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray6<UInt64> o, in InlineArray6<UInt64> b, in InlineArray6<UInt64> a) {
            unchecked {
                nuint c0, c1;
                UInt64 t, o0, o1, o2, o3, o4, o5;
                UInt128 a5b5 = (UInt128)a[5] * b[5], a5b4 = (UInt128)a[5] * b[4],
                        a5b3 = (UInt128)a[5] * b[3], a5b2 = (UInt128)a[5] * b[2],
                        a5b1 = (UInt128)a[5] * b[1], a5b0 = (UInt128)a[5] * b[0];
                o0 = AddWithCarry((UInt64)a5b1, (UInt64)(a5b0 >> 64), 0, out c0);
                o1 = AddWithCarry((UInt64)a5b2, (UInt64)(a5b1 >> 64), c0, out c0);
                o2 = AddWithCarry((UInt64)a5b3, (UInt64)(a5b2 >> 64), c0, out c0);
                o3 = AddWithCarry((UInt64)a5b4, (UInt64)(a5b3 >> 64), c0, out c0);
                o4 = AddWithCarry((UInt64)a5b5, (UInt64)(a5b4 >> 64), c0, out c0);
                o5 = AddWithCarry(0, (UInt64)(a5b5 >> 64), c0, out c0);

                UInt128 a4b5 = (UInt128)a[4] * b[5], a4b4 = (UInt128)a[4] * b[4],
                        a4b3 = (UInt128)a[4] * b[3], a4b2 = (UInt128)a[4] * b[2],
                        a4b1 = (UInt128)a[4] * b[1];
                t = AddWithCarry((UInt64)a4b2, (UInt64)(a4b1 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                t = AddWithCarry((UInt64)a4b3, (UInt64)(a4b2 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                t = AddWithCarry((UInt64)a4b4, (UInt64)(a4b3 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                t = AddWithCarry((UInt64)a4b5, (UInt64)(a4b4 >> 64), c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                t = AddWithCarry(0, (UInt64)(a4b5 >> 64), c0, out c0);
                o4 = AddWithCarry(o4, t, c1, out c1);
                o5 = AddWithCarry(o5, 0, c1, out _);

                UInt128 a3b5 = (UInt128)a[3] * b[5], a3b4 = (UInt128)a[3] * b[4],
                        a3b3 = (UInt128)a[3] * b[3], a3b2 = (UInt128)a[3] * b[2];
                t = AddWithCarry((UInt64)a3b3, (UInt64)(a3b2 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                t = AddWithCarry((UInt64)a3b4, (UInt64)(a3b3 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                t = AddWithCarry((UInt64)a3b5, (UInt64)(a3b4 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                t = AddWithCarry(0, (UInt64)(a3b5 >> 64), c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                o4 = AddWithCarry(o4, 0, c1, out c1);
                o5 = AddWithCarry(o5, 0, c1, out _);

                UInt128 a2b5 = (UInt128)a[2] * b[5], a2b4 = (UInt128)a[2] * b[4], a2b3 = (UInt128)a[2] * b[3];
                t = AddWithCarry((UInt64)a2b4, (UInt64)(a2b3 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                t = AddWithCarry((UInt64)a2b5, (UInt64)(a2b4 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                t = AddWithCarry(0, (UInt64)(a2b5 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                o3 = AddWithCarry(o3, 0, c1, out c1);
                o4 = AddWithCarry(o4, 0, c1, out c1);
                o5 = AddWithCarry(o5, 0, c1, out _);

                UInt128 a1b5 = (UInt128)a[1] * b[5], a1b4 = (UInt128)a[1] * b[4];
                t = AddWithCarry((UInt64)a1b5, (UInt64)(a1b4 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                t = AddWithCarry(0, (UInt64)(a1b5 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                o2 = AddWithCarry(o2, 0, c1, out c1);
                o3 = AddWithCarry(o3, 0, c1, out c1);
                o4 = AddWithCarry(o4, 0, c1, out c1);
                o5 = AddWithCarry(o5, 0, c1, out _);

                UInt128 a0b5 = (UInt128)a[0] * b[5];
                o[0] = AddWithCarry(o0, (UInt64)(a0b5 >> 64), 0, out c1);
                o[1] = AddWithCarry(o1, 0, c1, out c1);
                o[2] = AddWithCarry(o2, 0, c1, out c1);
                o[3] = AddWithCarry(o3, 0, c1, out c1);
                o[4] = AddWithCarry(o4, 0, c1, out c1);
                o[5] = AddWithCarry(o5, 0, c1, out _);
            }
        }

        // ─── Arithmetic right shift (sign-extends the top bit) ─────────────────────
        internal static void ShiftRightArithmetic_A_1(ref InlineArray3<UInt64> a, int n) {
            unchecked {
                if (n <= 0) {
                    if (n < 0) ShiftLeftArithmetic(ref a, -n);
                    return;
                }
                UInt64 s0 = a[0], s1 = a[1], s2 = a[2];
                UInt64 sign = unchecked((UInt64)((Int64)s2 >> 63));
                if (n >= 192) { a[0] = a[1] = a[2] = sign; return; }

                Span<UInt64> src = stackalloc UInt64[6] { s0, s1, s2, sign, sign, sign };
                int qk = n >> 6, rk = n & 63;
                if (rk == 0) {
                    a[0] = src[qk];
                    a[1] = src[qk + 1];
                    a[2] = src[qk + 2];
                } else {
                    int lk = 64 - rk;
                    a[0] = (src[qk] >> rk) | (src[qk + 1] << lk);
                    a[1] = (src[qk + 1] >> rk) | (src[qk + 2] << lk);
                    a[2] = (src[qk + 2] >> rk) | (src[qk + 3] << lk);
                }
            }
        }

        internal static void ShiftRightArithmetic(ref InlineArray3<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftLeftArithmetic(ref a, -n); return; }
                if (n >= 192) {
                    UInt64 s = unchecked((UInt64)((Int64)a[2] >> 63));
                    a[0] = a[1] = a[2] = s; return;
                }
                UInt64[] old = [a[0], a[1], a[2]];
                UInt64 sign = unchecked((UInt64)((Int64)old[2] >> 63));
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 0; i < 3; i++) {
                    int src = i + qk;
                    UInt64 lo = src < 3 ? old[src] : sign;
                    if (rk == 0) { a[i] = lo; continue; }
                    UInt64 hi = src + 1 < 3 ? old[src + 1] : sign;
                    a[i] = (lo >> rk) | (hi << lk);
                }
            }
        }

        internal static void ShiftRightArithmetic(ref InlineArray4<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftLeftArithmetic(ref a, -n); return; }
                if (n >= 256) {
                    UInt64 s = unchecked((UInt64)((Int64)a[3] >> 63));
                    a[0] = a[1] = a[2] = a[3] = s; return;
                }
                UInt64[] old = [a[0], a[1], a[2], a[3]];
                UInt64 sign = unchecked((UInt64)((Int64)old[3] >> 63));
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 0; i < 4; i++) {
                    int src = i + qk;
                    UInt64 lo = src < 4 ? old[src] : sign;
                    if (rk == 0) { a[i] = lo; continue; }
                    UInt64 hi = src + 1 < 4 ? old[src + 1] : sign;
                    a[i] = (lo >> rk) | (hi << lk);
                }
            }
        }

        internal static void ShiftRightArithmetic(ref InlineArray7<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftLeftArithmetic(ref a, -n); return; }
                if (n >= 448) {
                    UInt64 s = unchecked((UInt64)((Int64)a[6] >> 63));
                    a[0] = a[1] = a[2] = a[3] = a[4] = a[5] = a[6] = s; return;
                }
                UInt64[] old = [a[0], a[1], a[2], a[3], a[4], a[5], a[6]];
                UInt64 sign = unchecked((UInt64)((Int64)old[6] >> 63));
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 0; i < 7; i++) {
                    int src = i + qk;
                    UInt64 lo = src < 7 ? old[src] : sign;
                    if (rk == 0) { a[i] = lo; continue; }
                    UInt64 hi = src + 1 < 7 ? old[src + 1] : sign;
                    a[i] = (lo >> rk) | (hi << lk);
                }
            }
        }

        // Helper used by the arithmetic shifts above (mirrors C shln-like semantics
        // but only used when n < 0; not present in cr_expq, kept minimal here).
        internal static void ShiftLeftArithmetic(ref InlineArray3<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftRightArithmetic(ref a, -n); return; }
                if (n >= 192) { a[0] = a[1] = a[2] = 0; return; }
                UInt64[] old = [a[0], a[1], a[2]];
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 2; i >= 0; i--) {
                    int src = i - qk;
                    UInt64 hi = src >= 0 ? old[src] : 0;
                    if (rk == 0) { a[i] = hi; continue; }
                    UInt64 lo = src - 1 >= 0 ? old[src - 1] : 0;
                    a[i] = (hi << rk) | (lo >> lk);
                }
            }
        }
        internal static void ShiftLeftArithmetic(ref InlineArray4<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftRightArithmetic(ref a, -n); return; }
                if (n >= 256) { a[0] = a[1] = a[2] = a[3] = 0; return; }
                UInt64[] old = [a[0], a[1], a[2], a[3]];
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 3; i >= 0; i--) {
                    int src = i - qk;
                    UInt64 hi = src >= 0 ? old[src] : 0;
                    if (rk == 0) { a[i] = hi; continue; }
                    UInt64 lo = src - 1 >= 0 ? old[src - 1] : 0;
                    a[i] = (hi << rk) | (lo >> lk);
                }
            }
        }
        internal static void ShiftLeftArithmetic(ref InlineArray7<UInt64> a, int n) {
            unchecked {
                if (n <= 0) { if (n < 0) ShiftRightArithmetic(ref a, -n); return; }
                if (n >= 448) { a[0] = a[1] = a[2] = a[3] = a[4] = a[5] = a[6] = 0; return; }
                UInt64[] old = [a[0], a[1], a[2], a[3], a[4], a[5], a[6]];
                int qk = n >> 6, rk = n & 63, lk = 64 - rk;
                for (int i = 6; i >= 0; i--) {
                    int src = i - qk;
                    UInt64 hi = src >= 0 ? old[src] : 0;
                    if (rk == 0) { a[i] = hi; continue; }
                    UInt64 lo = src - 1 >= 0 ? old[src - 1] : 0;
                    a[i] = (hi << rk) | (lo >> lk);
                }
            }
        }

        // ─── mahu1u6u6 / mahu1u3u3 : o = a*b + c ────────────────────────────────────
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyAddHigh(ref InlineArray6<UInt64> o, UInt64 a,
                                             in InlineArray6<UInt64> b, in InlineArray6<UInt64> c) {
            unchecked {
                nuint c0, c1;
                UInt64 t;
                UInt128 ab0 = (UInt128)a * b[0], ab1 = (UInt128)a * b[1];
                t = AddWithCarry((UInt64)ab1, (UInt64)(ab0 >> 64), 0, out c0);
                o[0] = AddWithCarry(c[0], t, 0, out c1);
                UInt128 ab2 = (UInt128)a * b[2];
                t = AddWithCarry((UInt64)ab2, (UInt64)(ab1 >> 64), c0, out c0);
                o[1] = AddWithCarry(c[1], t, c1, out c1);
                UInt128 ab3 = (UInt128)a * b[3];
                t = AddWithCarry((UInt64)ab3, (UInt64)(ab2 >> 64), c0, out c0);
                o[2] = AddWithCarry(c[2], t, c1, out c1);
                UInt128 ab4 = (UInt128)a * b[4];
                t = AddWithCarry((UInt64)ab4, (UInt64)(ab3 >> 64), c0, out c0);
                o[3] = AddWithCarry(c[3], t, c1, out c1);
                UInt128 ab5 = (UInt128)a * b[5];
                t = AddWithCarry((UInt64)ab5, (UInt64)(ab4 >> 64), c0, out c0);
                o[4] = AddWithCarry(c[4], t, c1, out c1);
                t = AddWithCarry(0, (UInt64)(ab5 >> 64), c0, out c0);
                o[5] = AddWithCarry(c[5], t, c1, out _);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyAddHigh(ref InlineArray3<UInt64> o, UInt64 a,
                                             in InlineArray3<UInt64> b, in InlineArray3<UInt64> c) {
            unchecked {
                nuint c0, c1;
                UInt64 t;
                UInt128 ab0 = (UInt128)a * b[0], ab1 = (UInt128)a * b[1];
                t = AddWithCarry((UInt64)ab1, (UInt64)(ab0 >> 64), 0, out c0);
                o[0] = AddWithCarry(c[0], t, 0, out c1);
                UInt128 ab2 = (UInt128)a * b[2];
                t = AddWithCarry((UInt64)ab2, (UInt64)(ab1 >> 64), c0, out c0);
                o[1] = AddWithCarry(c[1], t, c1, out c1);
                t = AddWithCarry(0, (UInt64)(ab2 >> 64), c0, out c0);
                o[2] = AddWithCarry(c[2], t, c1, out _);
            }
        }

        // mahuUU: c + (a * b_lo >> 64) + a * b_hi
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt128 MultiplyAddHigh(UInt64 a, UInt128 b, UInt128 c) {
            unchecked {
                c += ((UInt128)a * (UInt64)b) >> 64;
                return (UInt128)a * (b >> 64) + c;
            }
        }

        // ─── Polynomial evaluation ─────────────────────────────────────────────────
        // pol6 : f = Horner evaluation of a degree-(n-1) poly with 6-word coeffs, x is u64.
        static void EvalPoly6(ref InlineArray6<UInt64> f, UInt64 x, int n, ReadOnlySpan<UInt64> c) {
            unchecked {
                InlineArray6<UInt64> a = default, b = default, t = default;
                for (int j = 0; j < 6; j++) { b[j] = c[(n - 1) * 6 + j]; t[j] = c[(n - 2) * 6 + j]; }
                MultiplyAddHigh(ref a, x, in b, in t);
                for (int i = n - 3; i > 1; i--) {
                    for (int j = 0; j < 6; j++) t[j] = c[i * 6 + j];
                    for (int j = 0; j < 6; j++) b[j] = a[j];
                    MultiplyAddHigh(ref a, x, in b, in t);
                }
                for (int j = 0; j < 6; j++) t[j] = c[j];
                for (int j = 0; j < 6; j++) b[j] = a[j];
                MultiplyAddHigh(ref f, x, in b, in t);
            }
        }

        // pol6red : reduced fixed-shape form used in as_expq_superaccurate.
        static void EvalPoly6Reduced(ref InlineArray6<UInt64> f, UInt64 x, ReadOnlySpan<UInt64> c) {
            unchecked {
                InlineArray6<UInt64> a = default, b = default, t = default;
                // c[5][5]
                b[0] = c[5 * 6 + 5]; b[1] = b[2] = b[3] = b[4] = b[5] = 0;
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref a, x, in t, in b);
                // c[4][4], c[4][5]
                b[0] = c[4 * 6 + 4]; b[1] = c[4 * 6 + 5]; b[2] = b[3] = b[4] = b[5] = 0;
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref a, x, in t, in b);
                // c[3][3..5]
                b[0] = c[3 * 6 + 3]; b[1] = c[3 * 6 + 4]; b[2] = c[3 * 6 + 5]; b[3] = b[4] = b[5] = 0;
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref a, x, in t, in b);
                // c[2][2..5]
                b[0] = c[2 * 6 + 2]; b[1] = c[2 * 6 + 3]; b[2] = c[2 * 6 + 4]; b[3] = c[2 * 6 + 5]; b[4] = b[5] = 0;
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref a, x, in t, in b);
                // c[1][1..5]
                b[0] = c[1 * 6 + 1]; b[1] = c[1 * 6 + 2]; b[2] = c[1 * 6 + 3]; b[3] = c[1 * 6 + 4]; b[4] = c[1 * 6 + 5]; b[5] = 0;
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref a, x, in t, in b);
                // final
                for (int j = 0; j < 6; j++) b[j] = c[j];
                for (int j = 0; j < 6; j++) t[j] = a[j];
                MultiplyAddHigh(ref f, x, in t, in b);
            }
        }

        // pol6redred : f = ... using full 6-word x (mutated in place: x[5..2] = x[3..0], x[1..0] = 0).
        static void EvalPoly6ReducedReduced(ref InlineArray6<UInt64> f, ref InlineArray6<UInt64> x, ReadOnlySpan<UInt64> c) {
            unchecked {
                x[5] = x[3]; x[4] = x[2]; x[3] = x[1]; x[2] = x[0]; x[1] = x[0] = 0;
                InlineArray6<UInt64> c2 = default;
                for (int j = 0; j < 6; j++) c2[j] = c[2 * 6 + j];
                MultiplyHigh(ref f, in x, in c2);
                f[0] = f[2]; f[1] = f[3]; f[2] = f[4]; f[3] = f[5]; f[4] = f[5] = 0;
                InlineArray6<UInt64> c1 = default;
                for (int j = 0; j < 6; j++) c1[j] = c[1 * 6 + j];
                Add(ref f, in c1);
                InlineArray6<UInt64> tmp = default;
                for (int j = 0; j < 6; j++) tmp[j] = f[j];
                MultiplyHigh(ref f, in x, in tmp);
                f[0] = f[2]; f[1] = f[3]; f[2] = f[4]; f[3] = f[5];
                f[4] = c[0 * 6 + 4]; f[5] = c[0 * 6 + 5];
            }
        }

        // pol3 : 3-word Horner evaluation.
        static void EvalPoly3(ref InlineArray3<UInt64> f, UInt64 x, int n, ReadOnlySpan<UInt64> c) {
            unchecked {
                InlineArray3<UInt64> a = default, b = default, t = default;
                for (int j = 0; j < 3; j++) { b[j] = c[(n - 1) * 3 + j]; t[j] = c[(n - 2) * 3 + j]; }
                MultiplyAddHigh(ref a, x, in b, in t);
                for (int i = n - 3; i > 1; i--) {
                    for (int j = 0; j < 3; j++) t[j] = c[i * 3 + j];
                    for (int j = 0; j < 3; j++) b[j] = a[j];
                    MultiplyAddHigh(ref a, x, in b, in t);
                }
                for (int j = 0; j < 3; j++) t[j] = c[j];
                for (int j = 0; j < 3; j++) b[j] = a[j];
                MultiplyAddHigh(ref f, x, in b, in t);
            }
        }

        // pol : f = c[0] + c[1]*x + ... + c[n-1]*x^(n-1) + f0*x^n, 128-bit coeffs.
        static UInt128 EvalPoly2(UInt64 x, UInt128 f0, int n, ReadOnlySpan<UInt128> c) {
            unchecked {
                UInt128 f = c[n - 1] + f0;
                for (int i = n - 1; i > 0; i--)
                    f = MultiplyAddHigh(x, f, c[i - 1]);
                return f;
            }
        }
    }
}

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    partial class Binary128Arithmetic {

        public static UInt64 Exp(UInt64 lo, UInt64 hi, out UInt64 result_hi)
    => Exp(lo, hi, MidpointRounding.ToEven, out result_hi);

        public static UInt64 Exp(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                // exp(x) > 0 always, so "away from zero" == "toward +inf".
                if (mode == MidpointRounding.AwayFromZero)
                    mode = MidpointRounding.ToPositiveInfinity;

                bool isNearest = mode == MidpointRounding.ToEven;
                bool isUp = mode == MidpointRounding.ToPositiveInfinity;

                UInt64 b1 = hi & 0x7FFFFFFFFFFFFFFFUL;   // sign-stripped

                // ── Tiny |x|:  |x| < 2^(-114)  (covers ±0 and every binary128 subnormal) ──
                if (Misc.Unlikely(b1 < ((UInt64)(16383 - 114) << 48))) {
                    if (((hi << 1) | lo) == 0) {
                        // exp(±0) = 1
                        result_hi = 0x3FFF_0000_0000_0000UL;
                        return 0;
                    }
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                    // sa[] = { 1 - 2^-113, 1.0, 1 + 2^-112 }
                    int idx = 1 + (isUp ? 1 : 0) - (((hi >> 63) != 0 && !isNearest) ? 1 : 0);
                    switch (idx) {
                    case 0:  // largest value < 1
                        result_hi = 0x3FFE_FFFF_FFFF_FFFFUL;
                        return 0xFFFF_FFFF_FFFF_FFFFUL;
                    case 2:  // smallest value > 1
                        result_hi = 0x3FFF_0000_0000_0000UL;
                        return 1;
                    default:
                        result_hi = 0x3FFF_0000_0000_0000UL;
                        return 0;
                    }
                }

                // ── |x| >= xmax  (~ 1.135e4):  Inf, NaN, overflow, underflow ──────────
                if (Misc.Unlikely(b1 >= 0x400C_62E4_2FEF_A39EUL)) {
                    // ±Inf
                    if (b1 == ((UInt64)0x7FFF << 48) && lo == 0) {
                        if ((hi >> 63) == 0) { result_hi = hi; return lo; }
                        result_hi = 0; return 0;
                    }
                    // NaN (quietise sNaN)
                    if (b1 > ((UInt64)0x7FFF << 48) ||
                        (b1 == ((UInt64)0x7FFF << 48) && lo != 0)) {
                        if ((b1 & (1UL << 47)) == 0)
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        result_hi = hi | (1UL << 47);
                        return lo;
                    }

                    // Bounds:  xmax = +1.62e42fefa39ef35793c7673007e6 * 2^13
                    //          xmin = -1.654bb3b2c73ebb059fabb506ff34 * 2^13
                    const UInt64 XMaxHi = 0x400C_62E4_2FEF_A39EUL, XMaxLo = 0xF357_93C7_6730_07E6UL;
                    const UInt64 XMinHi = 0xC00C_654B_B3B2_C73EUL, XMinLo = 0xBB05_9FAB_B506_FF34UL;
                    UInt128 ua = ((UInt128)hi << 64) | lo;
                    UInt128 xmaxA = ((UInt128)XMaxHi << 64) | XMaxLo;
                    UInt128 xminA = ((UInt128)XMinHi << 64) | XMinLo;

                    if ((hi >> 63) == 0 && ua >= xmaxA) {
                        // positive overflow → +∞ (or MaxValue for downward rounding)
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Overflow
                                    | FloatingPointExceptionFlags.Inexact);
                        if (isNearest || isUp) { result_hi = 0x7FFF_0000_0000_0000UL; return 0; }
                        result_hi = 0x7FFE_FFFF_FFFF_FFFFUL;
                        return 0xFFFF_FFFF_FFFF_FFFFUL;
                    }
                    if (ua >= xminA) {
                        // negative underflow → 0 (or smallest subnormal for rounding up)
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow
                                    | FloatingPointExceptionFlags.Inexact);
                        if (isUp) { result_hi = 0; return 1; }
                        result_hi = 0; return 0;
                    }
                }

                // ── Main path ─────────────────────────────────────────────────────────
                // Range-reduce:  fs ≈ x * log2(e)  in a 192-bit fixed-point form.
                Int64 sm = unchecked((Int64)hi >> 63);       // 0 or -1
                UInt64 mHi = (hi & 0x0000_FFFF_FFFF_FFFFUL) | (1UL << 48);
                UInt128 mVal = ((UInt128)mHi << 64) | lo;

                InlineArray3<UInt64> iln2Top3 = default;
                for (int j = 0; j < 3; j++) iln2Top3[j] = ExpIlN2[j + 4];
                InlineArray3<UInt64> fs = default;
                MultiplyHigh(ref fs, in iln2Top3, mVal);
                fs[0] ^= unchecked((UInt64)sm);
                fs[1] ^= unchecked((UInt64)sm);
                fs[2] ^= unchecked((UInt64)sm);
                ShiftRightArithmetic(ref fs, 0x401A - (int)((hi >> 48) & 0x7FFF));

                Int64 fs2 = unchecked((Int64)fs[2]);
                int el = (int)(fs2 >> 20);
                int i0 = (int)((fs2 >> 15) & 31);
                int i1 = (int)((fs2 >> 10) & 31);
                int i2 = (int)((fs2 >> 5) & 31);
                int i3 = (int)(fs2 & 31);

                // ── Polynomial + 2^k table fusion ────────────────────────────────────
                UInt64 z = fs[1];

                // f0 = mhuu(z, c[3][0] + mhuu(z, c[4][0] + mhuu(z, c[5][0])))
                UInt64 f0 = MultiplyHigh(z,
                                ExpC[3 * 2 + 0] + MultiplyHigh(z,
                                    ExpC[4 * 2 + 0] + MultiplyHigh(z, ExpC[5 * 2 + 0])));

                // res = pol(z, f0, 3, c)  ==  c[0] + c[1]*z + c[2]*z^2 + f0*z^2
                UInt128 res = EvalPoly2(z, f0, 3, ExpCAsUInt128);

                UInt128 t0 = ((UInt128)ExpR0[i0 * 2 + 1] << 64) | ExpR0[i0 * 2 + 0];
                UInt128 t1 = ((UInt128)ExpR1[i1 * 2 + 1] << 64) | ExpR1[i1 * 2 + 0];
                UInt128 t2 = ((UInt128)ExpR2[i2 * 2 + 1] << 64) | ExpR2[i2 * 2 + 0];
                UInt128 t3 = ((UInt128)ExpR3[i3 * 2 + 1] << 64) | ExpR3[i3 * 2 + 0];

                UInt128 mBig = MultiplyHighApproximate(
                                   MultiplyHighApproximate(t0, t1),
                                   MultiplyHighApproximate(t2, t3));

                UInt64 k = (UInt64)(((UInt128)0xB17217F7D1CF79ACUL * fs[0]) >> 64);
                // NOTE: uses the high word of mBig, not the mantissa of x.
                mBig += ((UInt128)(UInt64)(mBig >> 64) * (UInt128)k) >> 84;

                res = MultiplyHighApproximate(res, mBig);

                UInt64 resLo = (UInt64)res, resHi = (UInt64)(res >> 64);
                UInt64 rnd;

                if (Misc.Likely(el >= -16382)) {
                    UInt64 s = (UInt64)(isNearest ? 1 : 0) << 10;
                    if (Misc.Unlikely(((resLo + s + 6) & 0x7FF) <= 6)) {
                        AsExpqAccurate(out Int64 nel, ref resLo, ref resHi,
                                       ((UInt128)hi << 64) | lo);
                        el = (int)nel;
                    }
                    rnd = (resLo >> 10) & 1UL;
                    resLo = (resLo >> 11) | (resHi << 53);
                    resHi >>= 11;
                    el += 16382;
                } else {
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);
                    if (el > -16499) {
                        int sh = -16372 - el;
                        UInt128 s = (UInt128)(isNearest ? 1 : 0) << sh;
                        UInt128 mask = ((UInt128)2 << sh) - 1;
                        UInt128 ra = ((UInt128)resHi << 64) | resLo;
                        if (Misc.Unlikely(((ra + s + 6) & mask) <= 6)) {
                            AsExpqAccurate(out Int64 nel, ref resLo, ref resHi,
                                           ((UInt128)hi << 64) | lo);
                            el = (int)nel;
                            ra = ((UInt128)resHi << 64) | resLo;
                        }
                        rnd = (UInt64)((ra >> sh) & 1);
                        ra >>= (-16371 - el);
                        resLo = (UInt64)ra;
                        resHi = (UInt64)(ra >> 64);
                    } else {
                        rnd = 0; resLo = 0; resHi = 0;
                    }
                    el = 0;
                }

                if (!isNearest) rnd = isUp ? 1UL : 0UL;

                UInt128 v = ((UInt128)resHi << 64) | resLo;
                v += ((UInt128)((UInt64)el << 48) << 64) | rnd;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(v >> 64);
                return (UInt64)v;
            }
        }
    }
}

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    partial class Binary128Arithmetic {


        // as_expq_accurate — 3-word fast path used when the main result is near a tie.
        static void AsExpqAccurate(out Int64 el, ref UInt64 mLo, ref UInt64 mHi, UInt128 x0) {
            // Tables ExpTbl0, ExpTbl1, ExpCAcc (all int[][3] → flat spans, row-major).
            // (Full literal tables omitted here for brevity — copy verbatim from the C source;
            //  3-word rows, 16 / 16 / 15 entries respectively. See §4 for the flat layout.)
            unchecked {
                UInt64 sm = unchecked((UInt64)((Int64)(x0 >> 64) >> 63));
                UInt128 t = (x0 & (((UInt128)1 << 112) - 1)) | ((UInt128)1 << 112);
                InlineArray4<UInt64> x = default;
                InlineArray4<UInt64> iln2Top4 = default;
                for (int j = 0; j < 4; j++) iln2Top4[j] = ExpIlN2[j + 3];
                MultiplyHigh(ref x, in iln2Top4, t);
                x[0] ^= sm; x[1] ^= sm; x[2] ^= sm; x[3] ^= sm;
                ShiftRightArithmetic(ref x, 0x402E - (int)((x0 >> 112) & 0x7FFF));
                el = unchecked((Int64)x[3]);
                int jt0 = (int)(x[2] >> 60), jt1 = (int)((x[2] >> 56) & 15);
                x[2] &= 0x00FF_FFFF_FFFF_FFFFUL;

                InlineArray3<UInt64> ft = default;
                InlineArray3<UInt64> t0 = default, t1 = default;
                for (int j = 0; j < 3; j++) { t0[j] = ExpTbl0Flat[jt0 * 3 + j]; t1[j] = ExpTbl1Flat[jt1 * 3 + j]; }
                MultiplyHigh(ref ft, in t0, in t1);

                InlineArray3<UInt64> f = default;
                EvalPoly3(ref f, x[2], ExpCAccRows, ExpCAccFlat);
                MultiplyHigh(ref ft, in f, in ft);

                UInt128 c1 = ((UInt128)ExpCAccFlat[1 * 3 + 2] << 64) | ExpCAccFlat[1 * 3 + 1];
                UInt128 f2 = MultiplyHighApproximate(((UInt128)x[1] << 64) | x[0],
                                c1 + MultiplyHigh(ExpCAccFlat[2 * 3 + 2], x[1]));
                InlineArray3<UInt64> f1 = default;
                f1[0] = (UInt64)f2; f1[1] = (UInt64)(f2 >> 64); f1[2] = ExpCAccFlat[0 * 3 + 2];
                MultiplyHigh(ref f, in ft, in f1);

                mLo = (f[1] >> 1) | (f[2] << 63);
                mHi = f[2] >> 1;

                bool rndfail;
                if (Misc.Likely(el >= -16382)) {
                    f2 = ((UInt128)f[1] << 64) | f[0];
                    UInt128 d = (f2 + 8) & ~((UInt128)0 >> 53);
                    rndfail = d <= 16;
                } else {
                    int s = -16371 - (int)el;
                    nuint k;
                    f[0] = AddWithCarry(f[0], 8, 0, out k);
                    f[1] = AddWithCarry(f[1], 0, k, out k);
                    f[2] = AddWithCarry(f[2], 0, k, out k);
                    if (s < 64) { f[1] &= (1UL << s) - 1; f[2] = 0; } else if (s < 128) { f[2] &= (1UL << (s - 64)) - 1; }
                    rndfail = f[0] <= 16 && (f[1] | f[2]) == 0;
                }
                if (rndfail)
                    AsExpqSuperaccurate(out el, ref mLo, ref mHi, x0);
            }
        }

        // as_expq_superaccurate — same shape, uses the 7-word iln2 and the 6-word tables.
        static void AsExpqSuperaccurate(out Int64 el, ref UInt64 mLo, ref UInt64 mHi, UInt128 x0) {
            // Uses ExpTbl6Flat (16×6), ExpC6Flat (28×6), ExpIlN2 (7 words).
            unchecked {
                Int64 sm = unchecked((Int64)(x0 >> 64) >> 63);
                UInt128 t = (x0 & (((UInt128)1 << 112) - 1)) | ((UInt128)1 << 112);
                InlineArray7<UInt64> x = default;
                MultiplyHigh(ref x, in ExpIlN2_7, t);
                for (int j = 0; j < 7; j++) x[j] ^= unchecked((UInt64)sm);
                ShiftRightArithmetic(ref x, 0x402E - (int)((x0 >> 112) & 0x7FFF));
                el = unchecked((Int64)x[6]);
                int jt = (int)(x[5] >> 60);
                x[5] &= 0x0FFF_FFFF_FFFF_FFFFUL;

                InlineArray6<UInt64> f = default, f1 = default, f2 = default, ft = default;
                EvalPoly6(ref f, x[5], 28, ExpC6Flat);
                EvalPoly6Reduced(ref f1, x[4], ExpC6Flat);
                InlineArray6<UInt64> xCopy = default;
                for (int j = 0; j < 6; j++) xCopy[j] = x[j];
                EvalPoly6ReducedReduced(ref f2, ref xCopy, ExpC6Flat);

                MultiplyHigh(ref ft, in f, in f1);
                MultiplyHigh(ref ft, in ft, in f2);
                InlineArray6<UInt64> tbl = default;
                for (int j = 0; j < 6; j++) tbl[j] = ExpTbl6Flat[jt * 6 + j];
                MultiplyHigh(ref ft, in ft, in tbl);

                mLo = (ft[4] >> 1) | (ft[5] << 63);
                mHi = ft[5] >> 1;
            }
        }
    }
}

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    partial class Binary128Arithmetic {


        // iln2 (7 words, from the C source):
        static ReadOnlySpan<UInt64> ExpIlN2 => [
            0xea90b9e60c4a90a0, 0x24d92f75c16be0b3, 0xde1c43f755176cd6, 0x8b25166cd1a13247,
        0xeb577aa8dd695a58, 0xbe87fed0691d3e88, 0xb8aa3b295c17f0bb
        ];

        static ref readonly InlineArray7<UInt64> ExpIlN2_7 => ref UltimateOrb.Runtime.CompilerServices.Unsafe.As<UInt64, InlineArray7<UInt64>>(ref MemoryMarshal.GetReference(ExpIlN2));

        // r0..r3 (32 rows × 2 words each). Copy verbatim from the C source.
        static ReadOnlySpan<UInt64> ExpR0 => [
                               0, 0x8000000000000000, 0x3e2a475b46520bff, 0x82cd8698ac2ba1d7,
 0xc5c95b8c2154c1b2, 0x85aac367cc487b14,  0x5df8d76c98c67563, 0x88980e8092da8527,
 0xfbe4628758a53c90, 0x8b95c1e3ea8bd6e6,  0x2dc0144c8783d4c6, 0x8ea4398b45cd53c0,
 0x0fd6d8e0ae5ac9d8, 0x91c3d373ab11c336,  0x2e8afad12551de54, 0x94f4efa8fef70961,
 0x46ad23182e42f6f6, 0x9837f0518db8a96f,  0xa2a817a2a3cc3f1f, 0x9b8d39b9d54e5538,
 0xa0911f09ebb9fdd1, 0x9ef5326091a111ad,  0x9b7a04ef80cfdea8, 0xa27043030c496818,
 0x1cbd7f621710701b, 0xa5fed6a9b15138ea,  0x541e24ec3531fa73, 0xa9a15ab4ea7c0ef8,
 0x4980a8c8f59a2ec4, 0xad583eea42a14ac6,  0x87d037e96d215d8e, 0xb123f581d2ac258f,
 0x597d89b3754abe9f, 0xb504f333f9de6484,  0x1b879778566b65a2, 0xb8fbaf4762fb9ee9,
 0xa8811fb66d0faf7a, 0xbd08a39f580c36be,  0x7c457d59a50087b5, 0xc12c4cca66709456,
 0x3e2ad0c964dd9f37, 0xc5672a115506dadd,  0x80e1f92a0511697e, 0xc9b9bd866e2f27a2,
 0xe235838f95f2c6ed, 0xce248c151f8480e3,  0x12248e57c3de4028, 0xd2a81d91f12ae45a,
 0x39a68bb9902d3fde, 0xd744fccad69d6af4,  0x3d840d5a9e29aa64, 0xdbfbb797daf23755,
 0x065895048dd333ca, 0xe0ccdeec2a94e111,  0x1e5e8f4a4edbb0ed, 0xe5b906e77c8348a8,
 0xd02d75b3706e54fb, 0xeac0c6e7dd24392e,  0x46561cf6948db913, 0xefe4b99bdcdaf5cb,
 0x7b9d0c7aed980fc3, 0xf5257d152486cc2c,  0x7c25bb14315d7fcd, 0xfa83b2db722a033a,


        ];
        static ReadOnlySpan<UInt64> ExpR1 => [
                             0, 0x8000000000000000,  0x3690dfe44d11d008, 0x8016302f17467628,
 0xff8ce94a6797b3ce, 0x802c6436d0e04f50,  0x49fc841afba9c3c6, 0x80429c17d77c18ed,
 0x94d589f608ee4aa2, 0x8058d7d2d5e5f6b0,  0xe54ec5f966eb1872, 0x806f17687707a7af,
 0xa0cc0a49c10ea66b, 0x80855ad965e88b83,  0x4a8a4f44bb703db6, 0x809ba2264dada76a,
 0x25335719b6e6fd20, 0x80b1ed4fd999ab6c,  0xb880575ea03548c1, 0x80c83c56b50cf77f,
 0x3b13310f5ad57fb1, 0x80de8f3b8b85a0af,  0xe0adc640acaa6b0b, 0x80f4e5ff089f763e,
 0x0cef03ab14a66550, 0x810b40a1d81406d4,  0x6abd3b0eab9c7048, 0x81219f24a5baa59d,
 0xe885724f14131287, 0x813801881d886f7b,  0x99775205944eadc4, 0x814e67cceb90502c,
 0x7be56527bd14def5, 0x8164d1f3bc030773,  0x24f1624278193c37, 0x817b3ffd3b2f2e47,
 0x51ac3dac02ca5008, 0x8191b1ea15813bfd,  0x5dd1caf33588f2d3, 0x81a827baf7838b78,
 0xa047bab784691314, 0x81bea1708dde6055,  0xad87c8fb65a6993c, 0x81d51f0b8557ec1c,
 0x801cf6ea3b3068f3, 0x81eba08c8ad4536f,  0x875bb1f380439fee, 0x820225f44b55b33b,
 0x9c7cd106d23f3768, 0x8218af4373fc25eb,  0xde4357a774d13d5c, 0x822f3c7ab205c89a,
 0x7354f57a2d982491, 0x8245cd9ab2cec048,  0x336d3fddc28165ad, 0x825c62a423d13f0c,
 0x3793aa0d08c818fb, 0x8272fb97b2a5894c,  0x517c473948af9a0c, 0x828998760d01faf3,
 0x6a3b68fcc424ff9f, 0x82a0393fe0bb0ca8,  0xc87433776c8b975c, 0x82b6ddf5dbc35906,

        ];
        static ReadOnlySpan<UInt64> ExpR2 => [
                                0, 0x8000000000000000,  0xaa22beacca949013, 0x8000b17292f702a3,
 0xe84c2e1a463473da, 0x800162e61bed4a48,  0xe9b3d4c106428682, 0x8002145a9ae42bf6,
 0xb6566a58c048be1f, 0x8002c5d00fdcfcb6,  0x2ef8674028829792, 0x800377467ad91193,
 0x0d2893e85affca64, 0x800428bddbd9bf99,  0xe3429843d1643041, 0x8004da3632e05bd6,
 0x1c718b38e549cb93, 0x80058baf7fee3b5d,  0xfcb28217df49d908, 0x80063d29c304b33d,
 0xa0d7201492b1d78a, 0x8006eea4fc25188d,  0xfe8825c385e97278, 0x8007a0212b50c061,
 0xe448009aa78e39cd, 0x8008519e5088ffd2,  0xf9755a75904a13f7, 0x8009031c6bcf2bf9,
 0xbe4da91d51695528, 0x8009b49b7d2499f2,  0x8befbdd3d03567f0, 0x800a661b848a9eda,
 0x945e54e2ae18f2f0, 0x800b179c82028fd0,  0xe282a52dbd92678a, 0x800bc91e758dc1f5,
 0x5a2eefc903f9e56b, 0x800c7aa15f2d8a6d,  0xb8210f92481f5ed7, 0x800d2c253ee33e5b,
 0x920508ce2dc5e9b3, 0x800dddaa14b032e7,  0x567798c8de012934, 0x800e8f2fe095bd39,
 0x4d08c57a3c79bc3e, 0x800f40b6a295327b,  0x963e6d2da99d9c76, 0x800ff23e5aafe7d9,
 0x2b96d62d51c15a07, 0x8010a3c708e73282,  0xdf8b3e7109372044, 0x80115550ad3c67a4,
 0x5d926b50b5606f22, 0x801206db47b0dc73,  0x2a23393a42bf75ce, 0x8012b866d845e621,
 0xa2b72b6b280cfa61, 0x801369f35efcd9e3,  0xfdccfbad7657bafd, 0x80141b80dbd70cf1,
 0x4aeb2a187632347d, 0x8014cd0f4ed5d485,  0x72a28cd4d1f3bae9, 0x80157e9eb7fa85d8,

        ];
        static ReadOnlySpan<UInt64> ExpR3 => [

                               0, 0x8000000000000000,  0xecfc487503488bb2, 0x8000058b90de7e4c,
 0x8307016c1cd4e8b7, 0x80000b1721fa7c18,  0x6c292b5fbf0c0ab5, 0x800010a2b353f965,
 0x526be456600bdbe5, 0x8000162e44eaf636,  0xdfd867e27af0ecb1, 0x80001bb9d6bf728d,
 0xbe780f22911e236a, 0x8000214568d16e6e,  0x985450c12b846c01, 0x800026d0fb20e9db,
 0x1776c0f4dbea67d6, 0x80002c5c8dade4d7,  0xe5e911803e341d94, 0x800031e820785f63,
 0xadb511b1f9aaa919, 0x80003773b3805984,  0x18e4ae64c243eb6d, 0x80003cff46c5d33c,
 0xd181f1ff59ea3ac5, 0x8000428ada48cc8c,  0x8197047491c4129c, 0x800048166e094579,
 0xd32e2b434b7bc3d3, 0x80004da202073e04,  0x7051c9767a8724db, 0x8000532d9642b631,
 0x030c5fa5256f41fe, 0x800058b92abbae02,  0x35688bf267180da2, 0x80005e44bf722579,
 0xb1710a0d700810a8, 0x800063d054661c99,  0x2130b33187b01ad5, 0x8000695be9979366,
 0x2eb27e260db2f346, 0x80006ee77f0689e1,  0x84017f3e7b2d08fa, 0x8000747314b3000d,
 0xcb28e85a63fc235f, 0x800079feaa9cf5ed,  0xae3408e5780712fb, 0x80007f8a40c46b84,
 0xd72e4dd784856215, 0x80008515d72960d4,  0xf02341b475470578, 0x80008aa16dcbd5e0,
 0xa31e8c8c55fc0d3b, 0x8000902d04abcaab,  0x9a2bf3fb537c55a0, 0x800095b89bc93f37,
 0x7f575b29bd0f37f7, 0x80009b4433243387,  0xfcacc2cc05b33b9c, 0x8000a0cfcabca79d,
 0xbc384922c565c6f3, 0x8000a65b62929b7d,  0x680629faba6ad083, 0x8000abe6faa60f29,

        ];

        // c (6 rows × 2 words each).
        static ReadOnlySpan<UInt64> ExpC => [
            0xFFFF_FFFF_FFFF_FFFF, 0x7FFF_FFFF_FFFF_FFFF,
        0x7BCD_5E4F_1D9C_C01F, 0x0000_058B_90BF_BE8E,
        0xFF82_C58E_A86F_16B0, 0x0000_0000_001E_BFBD,
        0x71AC_235C_1282_FE2C, 0x0000_0000_0000_0000,
        0x0000_013B_2AB6_FBA4, 0x0000_0000_0000_0000,
        0x0000_0000_0002_BB10, 0x0000_0000_0000_0000
        ];

        static ReadOnlySpan<UInt64> ExpC_BE => [
            0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF,
        0x0000_058B_90BF_BE8E, 0x7BCD_5E4F_1D9C_C01F,
        0x0000_0000_001E_BFBD, 0xFF82_C58E_A86F_16B0,
        0x0000_0000_0000_0000, 0x71AC_235C_1282_FE2C,
        0x0000_0000_0000_0000, 0x0000_013B_2AB6_FBA4,
        0x0000_0000_0000_0000, 0x0000_0000_0002_BB10,
    ];

        static ReadOnlySpan<UInt128> ExpCAsUInt128 => MemoryMarshal.Cast<UInt64, UInt128>(BitConverter.IsLittleEndian ? ExpC : ExpC_BE);


        // 6-word tables used by AsExpqSuperaccurate (16×6 and 28×6).
        static ReadOnlySpan<UInt64> ExpTbl6Flat => [

             0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
0x82dc9bf3421840d1, 0x7835af9ab4e6355c, 0x5d42b362af1ee859, 0x148a0459e7585151, 0xc5c95b8c2154c1b2, 0x85aac367cc487b14,
 0x9994fac7f4ea654b, 0xb4f28c879a524bd4, 0x91e135ee84a3f733, 0x1aa84ffbebac349f, 0xfbe4628758a53c90, 0x8b95c1e3ea8bd6e6,
 0x23b504fb5dadbb25, 0x4e0d990c27c43643, 0xf1203caf65bfb9b9, 0x1942b34816fb4f26, 0x0fd6d8e0ae5ac9d8, 0x91c3d373ab11c336,
 0x4e9f94ee0c90dbbd, 0xbe47c34d380250a8, 0xd78b65cbefa7bb6f, 0x5e139a1b14fa8178, 0x46ad23182e42f6f6, 0x9837f0518db8a96f,
 0xa264696b6f6e2b1c, 0xcd12e5ada91eef7b, 0x21f977fe7c7fa117, 0x65c15c122133e2a2, 0xa0911f09ebb9fdd1, 0x9ef5326091a111ad,
 0xad35e8e58c5ce4f4, 0x5343b4a0330a8052, 0x2589c98a8290d3f0, 0x1dd170ace2bcfc17, 0x1cbd7f621710701b, 0xa5fed6a9b15138ea,
 0x739a3d061fea7592, 0x458fd5f44e26a7a7, 0xb165f141833a67da, 0x6be409407034fded, 0x4980a8c8f59a2ec4, 0xad583eea42a14ac6,
 0xa8b1fe6fdc83db39, 0x4afc83043ab8a2c3, 0xed17ac8583339915, 0x1d6f60ba893ba84c, 0x597d89b3754abe9f, 0xb504f333f9de6484,
 0x283c66db56df97f8, 0x9b985f3a0eae1c1d, 0x0d9a4be023ece031, 0x15b34bbcb0298f41, 0xa8811fb66d0faf7a, 0xbd08a39f580c36be,
 0xefba190ed1a58a51, 0xbb2068be237512df, 0xc7686006e4e6c092, 0x6b0f939998251a36, 0x3e2ad0c964dd9f37, 0xc5672a115506dadd,
 0xcd8caebc5d894c26, 0x1b8343088bbdadd4, 0x2bbd398af35c079f, 0x6f28610b8c36485a, 0xe235838f95f2c6ed, 0xce248c151f8480e3,
 0x81247458fb61e576, 0xefb01fda334bca9a, 0xb5c13ada0e778299, 0x1d733af522058b16, 0x39a68bb9902d3fde, 0xd744fccad69d6af4,
 0xf44e17262da49b5d, 0x188081fe7062f61e, 0x1cb99d3f1ff298a2, 0x224b251b33092002, 0x065895048dd333ca, 0xe0ccdeec2a94e111,
 0x091482854d819e44, 0xb338fcd2ac2ffbc8, 0x17d8d1e8ca31880a, 0xc4faace043b7f91c, 0xd02d75b3706e54fa, 0xeac0c6e7dd24392e,
 0x29f075f23730e918, 0xd22dd036f1906094, 0xbdd80329364aa29f, 0x6f510308677709f5, 0x7b9d0c7aed980fc3, 0xf5257d152486cc2c
            ];


        static ReadOnlySpan<UInt64> ExpC6Flat => [
              0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
 0x2acaa97da57cadbe, 0xf3dc3b1036f5d64c, 0xc5068badc5d57d15, 0xa079a193394c5b16, 0xe4f1d9cc01f97b57, 0x58b90bfbe8e7bcd5,
 0xd344071b5ec47714, 0x524eb0376b9686e0, 0xc2be93bbb1396e6e, 0xa3a2751c30ce69d4, 0x6f16b06ec9735fca, 0x1ebfbdff82c58ea8,
 0x80960837d68b3044, 0x44545f2e4dd67d06, 0x4753198ade236146, 0xa7ae23a226d00887, 0xcce9d8aeccaf4b7b, 0x071ac235c1282fe2,
 0xeee877628d8f6274, 0x8f7e8887e73829d7, 0x699b709699e1815f, 0x72478ea53e63911d, 0x9ccbbe0b53eeac50, 0x013b2ab6fba4e772,
 0x318a9f63b2f3ec5c, 0x2f040f926e2c93be, 0xc8cdc36aa406093f, 0x60aed94d2dce32e1, 0x20e2fed34a297d86, 0x002bb0ffcf14ce62,
 0x2d3c7b9299d0d3e1, 0x91ec7aec4e3f0a35, 0x549592ff6d6ab786, 0xcfb314ffccc47bb0, 0xdbd2c2a261ac8d07, 0x00050c244be1b1e1,
 0xcd46dc2cd5899528, 0x95ce94549c9a636a, 0x5d3119327fac831d, 0x4ace1152e8810fee, 0x1a1ac547321f639a, 0x00007ff2ff1622c3,
 0x8531a13788143560, 0x5c5f54178a891b6b, 0x7d5b21cd05968068, 0x2586e1a0f7107ab9, 0x11fec7ff3036d3be, 0x00000b160111d2e4,
 0xe590c3fdfb446be7, 0x2d5087b376c0214d, 0x738afd2a4917377a, 0x84518cb8caa9ba26, 0x3e1ed253872d27fc, 0x000000da929e9caf,
 0xcc782de6ff640fd0, 0x68569686f1264ea6, 0x98468b24acd303be, 0xb4ce2a0608a16570, 0xc764fb7ed0eca973, 0x0000000f267a8ac5,
 0xee22a537a6d9a8b8, 0x08110963d1d9380f, 0x43b147aa9f2c11b5, 0x90a4edc7b3f29c1c, 0x8dd92607abccaf23, 0x00000000f465639a,
 0x38202f8bf18f1943, 0xbc2fb66e0bf68e76, 0x511f7698b91cba26, 0xd17730e76aaedf16, 0x7e14c2f15ab43f0b, 0x000000000e1deb28,
 0xea93b5cedf528c2f, 0x6ee667b44f788f66, 0x714aa5175cd8c9ab, 0x7b8d8ce6fe81f087, 0x8b3687cb140d6180, 0x0000000000c0b0c9,
 0x88ad1fe5bcec581b, 0xa772e0b581376df7, 0x1d1edb9f5248282a, 0x2ae5761242913279, 0x26ac3c54b9f8a1b1, 0x0000000000098a4b,
 0x9d19faf9f3a16e65, 0xc2fde1c74321a361, 0x3787e55bad70a464, 0xfc6a729280d47c69, 0xa10ec1008799ec55, 0x00000000000070db,
 0xf708ee5bf7061dee, 0x921cfcdbbccf2eb0, 0x7cd64e11a79215fe, 0x93b26377e3b574bd, 0xa26b9e7e2ce48e3f, 0x00000000000004e3,
 0x9bb4d4ccb609d71e, 0x6ef9409a4cd3ac24, 0xc560a32b4349d4dd, 0xb1001eff03e83f71, 0x088968384b4faac3, 0x0000000000000033,
 0xb5c5f81b7e235f48, 0x9c1837383fef6aa4, 0xcf80a6c83ea7dc89, 0x2ed389743c9aa15c, 0xf7176bdb43695d73, 0x0000000000000001,
 0x4e3f1412be77b88a, 0x75f070df1338e5db, 0xd970f6d684dcca91, 0xe056ab257aa7f274, 0x125a7ecb835c64da, 0x0000000000000000,
 0x6c8a502410b3a92b, 0x5ae22ff5b3b87c53, 0x5510cef268f5a4f6, 0x27126f802bc39d75, 0x00a2d6625a8289ac, 0x0000000000000000,
 0xa851c04a8293fece, 0x95e77dec82b8eb58, 0xef60eb192fe67eda, 0xe0aadf36ddf86f4f, 0x00055ff15e0f8271, 0x0000000000000000,
 0x7e2f1903476d3f72, 0x991510e40164f6ec, 0xb93a9cf471520656, 0x81d8f91aed9d0512, 0x00002b59f5a6e03b, 0x0000000000000000,
 0xe661b8b0f04bd98d, 0xc0c7162d7c443b27, 0x48eb10aa6d041426, 0xfb5cb2465d33a354, 0x0000014e7515dc98, 0x0000000000000000,
 0xa2de610b24affa88, 0x36d51a1c2cf018ae, 0xe823660bee34df2b, 0xd5e52102fa4f3d47, 0x00000009a8d57b65, 0x0000000000000000,
 0x8297d75925cf8a0e, 0xb19c48d3a13188f6, 0x08ec765f339c0710, 0x6a09e41e7a3814f9, 0x00000000448fbf65, 0x0000000000000000,
 0x11744278158dd70d, 0xd10b922fb4d1b574, 0xdd8263d932a8a20c, 0x99f1cc682af884c5, 0x0000000001d3ebc2, 0x0000000000000000,
 0x20127ee09bb9f25c, 0x5ed98d7cde9ecace, 0x46da42907892bb39, 0x9af8dbd36a3b0863, 0x00000000000c0334, 0x0000000000000000,
 0xafd75d37c82b15ca, 0xf0acd37674cf3129, 0xae8a3fb59540ab8d, 0xa3dfeb50d4131639, 0x0000000000004c20, 0x0000000000000000,
 0xdaaf1913e1cb12f0, 0xe352ea644fb69c62, 0xf3fac0750b414cc5, 0xcf69a29f13de20e8, 0x00000000000001d1, 0x0000000000000000,
 0x0587a7d1a7ec6c45, 0x05eaca3704dcd2f6, 0x12dcbe8116eada8f, 0xc333445e3155f79b, 0x000000000000000a, 0x0000000000000000,
 0x183f54470927dbcd, 0x76bc8f5b8e8b7093, 0x8f099fb908474b26, 0x3d9aea5b4d0ebfae, 0x0000000000000000, 0x0000000000000000,
 0x261c179f4bdffa1d, 0x0909a0681e2103a8, 0x4dbdd766d2d9a092, 0x01559c86508b1539, 0x0000000000000000, 0x0000000000000000,
 0x3816a7ca0ca85578, 0x6b7fd2696989cdee, 0xd34dacabb2d48a6c, 0x00072ce49e65a3ac, 0x0000000000000000, 0x0000000000000000,
 0xd4e29d35fe14e115, 0xc30d08011bdafd19, 0xa9a6bf19955b2132, 0x00002572be492f62, 0x0000000000000000, 0x0000000000000000,
 0xe4df41884be86bb5, 0x0c5772ee960fd5bf, 0x9dbee3b9355d1fb0, 0x000000bdd01d5d84, 0x0000000000000000, 0x0000000000000000,
 0x80446608d32c4ed3, 0xb4d5892fe7709c19, 0xd3840403fcefe8a9, 0x00000003bc4fb6d4, 0x0000000000000000, 0x0000000000000000



            ];
        static int ExpC6Rows => 28;

        // 3-word tables used by AsExpqAccurate (16×3, 16×3, 15×3).
        static ReadOnlySpan<UInt64> ExpTbl0Flat => [

             0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
 0x148a0459e7585151, 0xc5c95b8c2154c1b2, 0x85aac367cc487b14,
 0x1aa84ffbebac349f, 0xfbe4628758a53c90, 0x8b95c1e3ea8bd6e6,
 0x1942b34816fb4f26, 0x0fd6d8e0ae5ac9d8, 0x91c3d373ab11c336,
 0x5e139a1b14fa8178, 0x46ad23182e42f6f6, 0x9837f0518db8a96f,
 0x65c15c122133e2a2, 0xa0911f09ebb9fdd1, 0x9ef5326091a111ad,
 0x1dd170ace2bcfc17, 0x1cbd7f621710701b, 0xa5fed6a9b15138ea,
 0x6be409407034fded, 0x4980a8c8f59a2ec4, 0xad583eea42a14ac6,
 0x1d6f60ba893ba84c, 0x597d89b3754abe9f, 0xb504f333f9de6484,
 0x15b34bbcb0298f41, 0xa8811fb66d0faf7a, 0xbd08a39f580c36be,
 0x6b0f939998251a36, 0x3e2ad0c964dd9f37, 0xc5672a115506dadd,
 0x6f28610b8c36485a, 0xe235838f95f2c6ed, 0xce248c151f8480e3,
 0x1d733af522058b16, 0x39a68bb9902d3fde, 0xd744fccad69d6af4,
 0x224b251b33092002, 0x065895048dd333ca, 0xe0ccdeec2a94e111,
 0xc4faace043b7f91c, 0xd02d75b3706e54fa, 0xeac0c6e7dd24392e,
 0x6f510308677709f5, 0x7b9d0c7aed980fc3, 0xf5257d152486cc2c

            ];
        static ReadOnlySpan<UInt64> ExpTbl1Flat => [

             0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
 0x2adc0c3f864ba0f5, 0x94d589f608ee4aa2, 0x8058d7d2d5e5f6b0,
 0x01f60261b05f1202, 0x25335719b6e6fd20, 0x80b1ed4fd999ab6c,
 0xa9c9ffc2ca67ffde, 0x0cef03ab14a6654f, 0x810b40a1d81406d4,
 0x9eb851655e2e5c4d, 0x7be56527bd14def4, 0x8164d1f3bc030773,
 0xd5abd77e8e1d3a02, 0xa047bab784691313, 0x81bea1708dde6055,
 0x205da5fe02d7b22a, 0x9c7cd106d23f3768, 0x8218af4373fc25eb,
 0x352354079f8705c1, 0x3793aa0d08c818fb, 0x8272fb97b2a5894c,
 0x29f1a4afbefa5d7c, 0x3e2a475b46520bff, 0x82cd8698ac2ba1d7,
 0xff36264c3caa8a43, 0x90950cc78d29f056, 0x83285071e0fc4546,
 0xe5c5849563af188b, 0xe201d4ec3d93f683, 0x8383594eefb6ee36,
 0x2af66c991b2a1028, 0x334544586ffe6d47, 0x83dea15b9541b132,
 0x0d96b414ec4c9d06, 0x1af92eca13fd1582, 0x843a28c3acde4046,
 0xff8d63ef80157021, 0xf38ffeb805e14189, 0x8495efb3303efd2f,
 0x17011ed39873fe65, 0x0f03062c26b5ba5d, 0x84f1f656379c1a29,
 0xa23f7708cf218f12, 0x16c873d1d378c1c9, 0x854e3cd8f9c8c95d


            ];
        static ReadOnlySpan<UInt64> ExpCAccFlat => [

            0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
 0xa079a193394c54e1, 0xe4f1d9cc01f97b57, 0x58b90bfbe8e7bcd5,
 0xa3a2751c329dec63, 0x6f16b06ec9735fca, 0x1ebfbdff82c58ea8,
 0xa7ae236cccddaf6f, 0xcce9d8aeccaf4b7b, 0x071ac235c1282fe2,
 0x724ac5c53418fab2, 0x9ccbbe0b53eeac50, 0x013b2ab6fba4e772,
 0x42d10a63971b725e, 0x20e2fed34a297d86, 0x002bb0ffcf14ce62,
 0xd132fe878c97b1ac, 0xdbd2c2a261ac8dbc, 0x00050c244be1b1e1,
 0xf9ce89ab39b2c8cf, 0x1a1ac547321c73be, 0x00007ff2ff1622c3,
 0xd0096a8f975ae04e, 0x11fec7ff38d4399b, 0x00000b160111d2e4,
 0x2965d6ecab7d25ba, 0x3e1ed24165e0982b, 0x000000da929e9caf,
 0x96be361027c23482, 0xc76516f9cc9face5, 0x0000000f267a8ac5,
 0xa8358f9d94f711b3, 0x8dbb68c80b739ee8, 0x00000000f465639a,
 0x4b1c3c1e37a9e553, 0x947ddee7032a7546, 0x000000000e1deb28,
 0xfb45ff03de0da11f, 0x5fd29dc12646b116, 0x0000000000c0b0be,
 0xe66d2a5194015941, 0x2dd5a6458bb23f71, 0x0000000000098d9a

            ];
        static int ExpCAccRows => 15;
    }
}