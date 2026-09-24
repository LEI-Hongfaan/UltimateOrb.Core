using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Misc = UltimateOrb.Miscellaneous;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using UInt128 = System.UInt128;
    using Int128 = System.Int128;
#endif

    public static partial class Binary128Arithmetic {

        // ---------------------------------------------------------------------
        // Borrowing subtract
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 SubtractWithBorrow(UInt64 a, UInt64 b, nuint borrow, out nuint newBorrow) {
            unchecked {
                UInt64 d0 = a - b;
                nuint b0 = a < b ? 1u : 0u;
                UInt64 d1 = d0 - borrow;
                nuint b1 = d0 < borrow ? 1u : 0u;
                newBorrow = b0 + b1;
                return d1;
            }
        }

        // ---------------------------------------------------------------------
        // InlineArray2 (add / mulhi)
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(ref InlineArray2<UInt64> o, in InlineArray2<UInt64> a, in InlineArray2<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1];
                UInt64 o0 = AddWithCarry(a0, b[0], 0, out var c);
                UInt64 o1 = AddWithCarry(a1, b[1], c, out _);
                o[0] = o0; o[1] = o1;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray2<UInt64> o, in InlineArray2<UInt64> b, in InlineArray2<UInt64> a) {
            unchecked {
                UInt128 a1b0 = (UInt128)a[1] * b[0];
                UInt64 o0 = (UInt64)(a1b0 >> 64);
                UInt128 a0b1 = (UInt128)a[0] * b[1];
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                UInt64 t = AddWithCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out var c0);
                o0 = AddWithCarry(o0, t, 0, out var c1);
                UInt64 o1 = AddWithCarry((UInt64)(a1b1 >> 64), 0, c0, out c0);
                o1 = AddWithCarry(o1, 0, c1, out _);
                o[0] = o0; o[1] = o1;
            }
        }

        // ---------------------------------------------------------------------
        // InlineArray3
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(ref InlineArray3<UInt64> o, in InlineArray3<UInt64> a, in InlineArray3<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2];
                UInt64 o0 = AddWithCarry(a0, b[0], 0, out var c);
                UInt64 o1 = AddWithCarry(a1, b[1], c, out c);
                UInt64 o2 = AddWithCarry(a2, b[2], c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Subtract(ref InlineArray3<UInt64> o, in InlineArray3<UInt64> a, in InlineArray3<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2];
                UInt64 o0 = SubtractWithBorrow(a0, b[0], 0, out var c);
                UInt64 o1 = SubtractWithBorrow(a1, b[1], c, out c);
                UInt64 o2 = SubtractWithBorrow(a2, b[2], c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        // mhu3uu3: multiply high of 3-word by 1-word
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray3<UInt64> o, UInt64 y, in InlineArray3<UInt64> x) {
            unchecked {
                UInt128 xy0 = (UInt128)x[0] * y;
                UInt128 xy1 = (UInt128)x[1] * y;
                UInt128 xy2 = (UInt128)x[2] * y;
                UInt64 o0 = AddWithCarry((UInt64)xy1, (UInt64)(xy0 >> 64), 0, out var c);
                UInt64 o1 = AddWithCarry((UInt64)xy2, (UInt64)(xy1 >> 64), c, out c);
                UInt64 o2 = AddWithCarry(0, (UInt64)(xy2 >> 64), c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        // mhu3u2u3: multiply-high of 3-word by 2-word (returns 3-word high)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray3<UInt64> o, in InlineArray2<UInt64> y, in InlineArray3<UInt64> x) {
            unchecked {
                UInt128 x1y0 = (UInt128)x[1] * y[0];
                UInt128 x2y0 = (UInt128)x[2] * y[0];
                UInt128 x0y1 = (UInt128)x[0] * y[1];
                UInt128 x1y1 = (UInt128)x[1] * y[1];
                UInt128 x2y1 = (UInt128)x[2] * y[1];
                x2y0 += x1y0 >> 64;
                x1y1 += x0y1 >> 64;
                x2y1 += x1y1 >> 64;
                UInt64 o0 = AddWithCarry((UInt64)x1y1, (UInt64)x2y0, 0, out var c);
                UInt64 o1 = AddWithCarry((UInt64)x2y1, (UInt64)(x2y0 >> 64), c, out c);
                UInt64 o2 = AddWithCarry(0, (UInt64)(x2y1 >> 64), c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        // mhu3u3u3: multiply-high of 3-word by 3-word (returns 3-word high)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray3<UInt64> o, in InlineArray3<UInt64> b, in InlineArray3<UInt64> a) {
            unchecked {
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                UInt128 a2b0 = (UInt128)a[2] * b[0];
                UInt128 a0b2 = (UInt128)a[0] * b[2];
                UInt128 a2b1 = (UInt128)a[2] * b[1];
                UInt128 a1b2 = (UInt128)a[1] * b[2];
                UInt128 a2b2 = (UInt128)a[2] * b[2];

                a2b1 += a2b0 >> 64;
                a1b2 += a0b2 >> 64;

                UInt64 o0 = (UInt64)(a1b1 >> 64);
                UInt64 o1 = (UInt64)a2b2;
                UInt64 o2 = (UInt64)(a2b2 >> 64);
                o0 = AddWithCarry(o0, (UInt64)a2b1, 0, out var c);
                o1 = AddWithCarry(o1, (UInt64)(a2b1 >> 64), c, out c);
                o2 = AddWithCarry(o2, 0, c, out c);

                o0 = AddWithCarry(o0, (UInt64)a1b2, 0, out c);
                o1 = AddWithCarry(o1, (UInt64)(a1b2 >> 64), c, out c);
                o2 = AddWithCarry(o2, 0, c, out _);

                o[0] = o0; o[1] = o1; o[2] = o2;
            }
        }

        // mu3x5: a *= 5
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyBy5(ref InlineArray3<UInt64> a) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2];
                UInt64 o0 = AddWithCarry(a0, (a1 << 62) | (a0 >> 2), 0, out var c);
                UInt64 o1 = AddWithCarry(a1, (a2 << 62) | (a1 >> 2), c, out c);
                UInt64 o2 = AddWithCarry(a2, a2 >> 2, c, out _);
                a[0] = o0; a[1] = o1; a[2] = o2;
            }
        }

        // ---------------------------------------------------------------------
        // InlineArray4
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(ref InlineArray4<UInt64> o, in InlineArray4<UInt64> a, in InlineArray4<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3];
                UInt64 o0 = AddWithCarry(a0, b[0], 0, out var c);
                UInt64 o1 = AddWithCarry(a1, b[1], c, out c);
                UInt64 o2 = AddWithCarry(a2, b[2], c, out c);
                UInt64 o3 = AddWithCarry(a3, b[3], c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3;
            }
        }

        // C: mhUIm(u128 _a, i128 _b, u64 mask)
        //     sub.b[0] = _a.b[0] & mask;  sub.b[1] = _a.b[1] & mask;
        //     return mhUU(_a, _b) - sub.a;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int128 MultiplyHighMaskedSigned(UInt128 a, Int128 b, UInt64 mask) {
            unchecked {
                UInt128 hi = MultiplyHighApproximate(a, (UInt128)b);     // mhUU(a, b)
                UInt128 sub = ((UInt128)((UInt64)(a >> 64) & mask) << 64)
                            | ((UInt64)a & mask);
                return (Int128)(hi - sub);
            }
        }

        // C: mhIU(i128 _b, u128 _a) = mhUIm(_a, _b, (u64)(_b >> 127))
        // NOTE: the C `(_b >> 127)` is an arithmetic shift on i128, so the mask
        // is all-ones when _b is negative and all-zeros when _b is non-negative.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int128 MultiplyHighSigned(UInt128 a, Int128 b) {
            unchecked {
                UInt64 mask = b < 0 ? ~0UL : 0UL;   // equivalent to (u64)(b >> 127)
                return MultiplyHighMaskedSigned(a, b, mask);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray4<UInt64> o, in InlineArray4<UInt64> b, in InlineArray4<UInt64> a) {
            unchecked {
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

                UInt64 o0 = AddWithCarry((UInt64)(a0b3 >> 64), (UInt64)a1b3, 0, out var c);
                UInt64 o1 = AddWithCarry((UInt64)(a1b3 >> 64), (UInt64)a2b3, c, out c);
                UInt64 o2 = AddWithCarry((UInt64)(a2b3 >> 64), (UInt64)a3b3, c, out c);
                UInt64 o3 = AddWithCarry((UInt64)(a3b3 >> 64), 0, c, out c);

                o0 = AddWithCarry(o0, (UInt64)(a3b0 >> 64), 0, out c);
                o1 = AddWithCarry(o1, (UInt64)a3b2, c, out c);
                o2 = AddWithCarry(o2, (UInt64)(a3b2 >> 64), c, out c);
                o3 = AddWithCarry(o3, 0, c, out c);

                o0 = AddWithCarry(o0, (UInt64)a3b1, 0, out c);
                o1 = AddWithCarry(o1, (UInt64)(a3b1 >> 64), c, out c);
                o2 = AddWithCarry(o2, 0, c, out c);
                o3 = AddWithCarry(o3, 0, c, out c);

                o0 = AddWithCarry(o0, (UInt64)a2b2, 0, out c);
                o1 = AddWithCarry(o1, (UInt64)(a2b2 >> 64), c, out c);
                o2 = AddWithCarry(o2, 0, c, out c);
                o3 = AddWithCarry(o3, 0, c, out _);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3;
            }
        }

        // mu4x3: a *= 3
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyBy3(ref InlineArray4<UInt64> a) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3];
                UInt64 o0 = AddWithCarry(a0, (a1 << 63) | (a0 >> 1), 0, out var c);
                UInt64 o1 = AddWithCarry(a1, (a2 << 63) | (a1 >> 1), c, out c);
                UInt64 o2 = AddWithCarry(a2, (a3 << 63) | (a2 >> 1), c, out c);
                UInt64 o3 = AddWithCarry(a3, a3 >> 1, c, out _);
                a[0] = o0; a[1] = o1; a[2] = o2; a[3] = o3;
            }
        }

        // ---------------------------------------------------------------------
        // InlineArray5
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Add(ref InlineArray5<UInt64> o, in InlineArray5<UInt64> a, in InlineArray5<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3], a4 = a[4];
                UInt64 o0 = AddWithCarry(a0, b[0], 0, out var c);
                UInt64 o1 = AddWithCarry(a1, b[1], c, out c);
                UInt64 o2 = AddWithCarry(a2, b[2], c, out c);
                UInt64 o3 = AddWithCarry(a3, b[3], c, out c);
                UInt64 o4 = AddWithCarry(a4, b[4], c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Subtract(ref InlineArray5<UInt64> o, in InlineArray5<UInt64> a, in InlineArray5<UInt64> b) {
            unchecked {
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3], a4 = a[4];
                UInt64 o0 = SubtractWithBorrow(a0, b[0], 0, out var c);
                UInt64 o1 = SubtractWithBorrow(a1, b[1], c, out c);
                UInt64 o2 = SubtractWithBorrow(a2, b[2], c, out c);
                UInt64 o3 = SubtractWithBorrow(a3, b[3], c, out c);
                UInt64 o4 = SubtractWithBorrow(a4, b[4], c, out _);
                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4;
            }
        }

        // mhu5u5u5: high 5-word of a 5x5 product
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray5<UInt64> o, in InlineArray5<UInt64> b, in InlineArray5<UInt64> a) {
            unchecked {
                nuint c0, c1;
                UInt64 t, o0, o1, o2, o3, o4;

                UInt128 a4b0 = (UInt128)a[4] * b[0];
                o0 = (UInt64)(a4b0 >> 64);

                UInt128 a3b1 = (UInt128)a[3] * b[1];
                UInt128 a4b1 = (UInt128)a[4] * b[1];
                t = AddWithCarry((UInt64)a4b1, (UInt64)(a3b1 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                o1 = AddWithCarry((UInt64)(a4b1 >> 64), 0, c0, out c0);
                o1 = AddWithCarry(o1, 0, c1, out c1);

                UInt128 a2b2 = (UInt128)a[2] * b[2];
                UInt128 a3b2 = (UInt128)a[3] * b[2];
                t = AddWithCarry((UInt64)a3b2, (UInt64)(a2b2 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt128 a4b2 = (UInt128)a[4] * b[2];
                t = AddWithCarry((UInt64)a4b2, (UInt64)(a3b2 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                o2 = AddWithCarry((UInt64)(a4b2 >> 64), 0, c0, out c0);
                o2 = AddWithCarry(o2, 0, c1, out c1);

                UInt128 a1b3 = (UInt128)a[1] * b[3];
                UInt128 a2b3 = (UInt128)a[2] * b[3];
                t = AddWithCarry((UInt64)a2b3, (UInt64)(a1b3 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt128 a3b3 = (UInt128)a[3] * b[3];
                t = AddWithCarry((UInt64)a3b3, (UInt64)(a2b3 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt128 a4b3 = (UInt128)a[4] * b[3];
                t = AddWithCarry((UInt64)a4b3, (UInt64)(a3b3 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                o3 = AddWithCarry((UInt64)(a4b3 >> 64), 0, c0, out c0);
                o3 = AddWithCarry(o3, 0, c1, out c1);

                UInt128 a0b4 = (UInt128)a[0] * b[4];
                UInt128 a1b4 = (UInt128)a[1] * b[4];
                t = AddWithCarry((UInt64)a1b4, (UInt64)(a0b4 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt128 a2b4 = (UInt128)a[2] * b[4];
                t = AddWithCarry((UInt64)a2b4, (UInt64)(a1b4 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt128 a3b4 = (UInt128)a[3] * b[4];
                t = AddWithCarry((UInt64)a3b4, (UInt64)(a2b4 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt128 a4b4 = (UInt128)a[4] * b[4];
                t = AddWithCarry((UInt64)a4b4, (UInt64)(a3b4 >> 64), c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                o4 = AddWithCarry((UInt64)(a4b4 >> 64), 0, c0, out c0);
                o4 = AddWithCarry(o4, 0, c1, out _);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4;
            }
        }

        // mhu5u2u5: high 5-word of 5-word by 2-word
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray5<UInt64> o, in InlineArray2<UInt64> b, in InlineArray5<UInt64> a) {
            unchecked {
                nuint c0, c1;
                UInt64 t, o0, o1, o2, o3, o4;
                UInt128 a1b0 = (UInt128)a[1] * b[0];
                UInt128 a2b0 = (UInt128)a[2] * b[0];
                UInt128 a3b0 = (UInt128)a[3] * b[0];
                UInt128 a4b0 = (UInt128)a[4] * b[0];

                o0 = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), 0, out c0);
                o1 = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c0, out c0);
                o2 = AddWithCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c0, out c0);
                o3 = AddWithCarry(0, (UInt64)(a4b0 >> 64), c0, out c0);

                UInt128 a0b1 = (UInt128)a[0] * b[1];
                UInt128 a1b1 = (UInt128)a[1] * b[1];
                UInt128 a2b1 = (UInt128)a[2] * b[1];
                UInt128 a3b1 = (UInt128)a[3] * b[1];
                UInt128 a4b1 = (UInt128)a[4] * b[1];

                t = AddWithCarry((UInt64)a1b1, (UInt64)(a0b1 >> 64), 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);

                t = AddWithCarry((UInt64)a2b1, (UInt64)(a1b1 >> 64), c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);

                t = AddWithCarry((UInt64)a3b1, (UInt64)(a2b1 >> 64), c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);

                t = AddWithCarry((UInt64)a4b1, (UInt64)(a3b1 >> 64), c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);

                t = AddWithCarry(0, (UInt64)(a4b1 >> 64), c0, out c0);
                o4 = AddWithCarry(0, t, c1, out _);

                o[0] = o0; o[1] = o1; o[2] = o2; o[3] = o3; o[4] = o4;
            }
        }

        // mhu5u1u5: high 5-word of 5-word by 1-word
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref InlineArray5<UInt64> o, UInt64 b0, in InlineArray5<UInt64> a) {
            unchecked {
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;
                UInt128 a4b0 = (UInt128)a[4] * b0;
                o[0] = AddWithCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out var c0);
                o[1] = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c0, out c0);
                o[2] = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c0, out c0);
                o[3] = AddWithCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c0, out c0);
                o[4] = AddWithCarry(0, (UInt64)(a4b0 >> 64), c0, out _);
            }
        }

        // mu5u1u4: o (5 words) = b0 * a (4 words)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void BigMul(ref InlineArray5<UInt64> o, UInt64 b0, in InlineArray4<UInt64> a) {
            unchecked {
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;
                o[0] = (UInt64)a0b0;
                o[1] = AddWithCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out var c);
                o[2] = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c, out c);
                o[3] = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c, out c);
                o[4] = AddWithCarry(0, (UInt64)(a3b0 >> 64), c, out _);
            }
        }

        // mu6u1u5: o (6 words) = b0 * a (5 words)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void BigMul(ref InlineArray6<UInt64> o, UInt64 b0, in InlineArray5<UInt64> a) {
            unchecked {
                UInt128 a0b0 = (UInt128)a[0] * b0;
                UInt128 a1b0 = (UInt128)a[1] * b0;
                UInt128 a2b0 = (UInt128)a[2] * b0;
                UInt128 a3b0 = (UInt128)a[3] * b0;
                UInt128 a4b0 = (UInt128)a[4] * b0;
                o[0] = (UInt64)a0b0;
                o[1] = AddWithCarry((UInt64)a1b0, (UInt64)(a0b0 >> 64), 0, out var c);
                o[2] = AddWithCarry((UInt64)a2b0, (UInt64)(a1b0 >> 64), c, out c);
                o[3] = AddWithCarry((UInt64)a3b0, (UInt64)(a2b0 >> 64), c, out c);
                o[4] = AddWithCarry((UInt64)a4b0, (UInt64)(a3b0 >> 64), c, out c);
                o[5] = AddWithCarry(0, (UInt64)(a4b0 >> 64), c, out _);
            }
        }

        // ---------------------------------------------------------------------
        // Shifts for InlineArray5 / 6
        // ---------------------------------------------------------------------

        internal static void ShiftLeft(ref InlineArray5<UInt64> a, int k) {
            unchecked {
                if (k < 0) { ShiftRight(ref a, -k); return; }
                int off = k >> 6;
                int s = k & 63;
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3], a4 = a[4];
                if (s == 0) {
                    // shift by whole words
                    switch (off) {
                    case 0: break;
                    case 1: a4 = a3; a3 = a2; a2 = a1; a1 = a0; a0 = 0; break;
                    case 2: a4 = a2; a3 = a1; a2 = a0; a1 = 0; a0 = 0; break;
                    case 3: a4 = a1; a3 = a0; a2 = 0; a1 = 0; a0 = 0; break;
                    case 4: a4 = a0; a3 = 0; a2 = 0; a1 = 0; a0 = 0; break;
                    default: a0 = a1 = a2 = a3 = a4 = 0; break;
                    }
                } else {
                    int q = 64 - s;
                    switch (off) {
                    case 0:
                        a4 = (a4 << s) | (a3 >> q);
                        a3 = (a3 << s) | (a2 >> q);
                        a2 = (a2 << s) | (a1 >> q);
                        a1 = (a1 << s) | (a0 >> q);
                        a0 = a0 << s;
                        break;
                    case 1:
                        a4 = (a3 << s) | (a2 >> q);
                        a3 = (a2 << s) | (a1 >> q);
                        a2 = (a1 << s) | (a0 >> q);
                        a1 = a0 << s;
                        a0 = 0;
                        break;
                    case 2:
                        a4 = (a2 << s) | (a1 >> q);
                        a3 = (a1 << s) | (a0 >> q);
                        a2 = a0 << s;
                        a1 = 0; a0 = 0;
                        break;
                    case 3:
                        a4 = (a1 << s) | (a0 >> q);
                        a3 = a0 << s;
                        a2 = 0; a1 = 0; a0 = 0;
                        break;
                    case 4:
                        a4 = a0 << s;
                        a3 = 0; a2 = 0; a1 = 0; a0 = 0;
                        break;
                    default:
                        a0 = a1 = a2 = a3 = a4 = 0;
                        break;
                    }
                }
                a[0] = a0; a[1] = a1; a[2] = a2; a[3] = a3; a[4] = a4;
            }
        }

        internal static void ShiftRight(ref InlineArray5<UInt64> a, int k) {
            unchecked {
                if (k < 0) { ShiftLeft(ref a, -k); return; }
                int off = k >> 6;
                int s = k & 63;
                UInt64 a0 = a[0], a1 = a[1], a2 = a[2], a3 = a[3], a4 = a[4];
                if (off >= 5) { a[0] = a[1] = a[2] = a[3] = a[4] = 0; return; }
                UInt64 n0 = off + 0 < 5 ? a[off + 0] : 0;
                UInt64 n1 = off + 1 < 5 ? a[off + 1] : 0;
                UInt64 n2 = off + 2 < 5 ? a[off + 2] : 0;
                UInt64 n3 = off + 3 < 5 ? a[off + 3] : 0;
                UInt64 n4 = off + 4 < 5 ? a[off + 4] : 0;
                UInt64 n5 = off + 5 < 5 ? a[off + 5] : 0;
                if (s == 0) {
                    a[0] = n0; a[1] = n1; a[2] = n2; a[3] = n3; a[4] = n4;
                } else {
                    int q = 64 - s;
                    a[0] = (n0 >> s) | (n1 << q);
                    a[1] = (n1 >> s) | (n2 << q);
                    a[2] = (n2 >> s) | (n3 << q);
                    a[3] = (n3 >> s) | (n4 << q);
                    a[4] = (n4 >> s) | (n5 << q);
                }
            }
        }

        internal static void ShiftLeft(ref InlineArray6<UInt64> a, int k) {
            unchecked {
                if (k < 0) { ShiftRight(ref a, -k); return; }
                int off = k >> 6;
                int s = k & 63;
                if (off >= 6) { a[0] = a[1] = a[2] = a[3] = a[4] = a[5] = 0; return; }
                UInt64 n0 = off + 0 < 6 ? a[off + 0] : 0;
                UInt64 n1 = off + 1 < 6 ? a[off + 1] : 0;
                UInt64 n2 = off + 2 < 6 ? a[off + 2] : 0;
                UInt64 n3 = off + 3 < 6 ? a[off + 3] : 0;
                UInt64 n4 = off + 4 < 6 ? a[off + 4] : 0;
                UInt64 n5 = off + 5 < 6 ? a[off + 5] : 0;
                if (s == 0) {
                    a[5] = n4; a[4] = n3; a[3] = n2; a[2] = n1; a[1] = n0; a[0] = 0;
                } else {
                    int q = 64 - s;
                    a[5] = (n4 << s) | (n5 >> q);
                    a[4] = (n3 << s) | (n4 >> q);
                    a[3] = (n2 << s) | (n3 >> q);
                    a[2] = (n1 << s) | (n2 >> q);
                    a[1] = (n0 << s) | (n1 >> q);
                    a[0] = n0 << s;
                }
            }
        }

        internal static void ShiftRight(ref InlineArray6<UInt64> a, int k) {
            unchecked {
                if (k < 0) { ShiftLeft(ref a, -k); return; }
                int off = k >> 6;
                int s = k & 63;
                if (off >= 6) { a[0] = a[1] = a[2] = a[3] = a[4] = a[5] = 0; return; }
                UInt64 n0 = off + 0 < 6 ? a[off + 0] : 0;
                UInt64 n1 = off + 1 < 6 ? a[off + 1] : 0;
                UInt64 n2 = off + 2 < 6 ? a[off + 2] : 0;
                UInt64 n3 = off + 3 < 6 ? a[off + 3] : 0;
                UInt64 n4 = off + 4 < 6 ? a[off + 4] : 0;
                UInt64 n5 = off + 5 < 6 ? a[off + 5] : 0;
                UInt64 n6 = off + 6 < 6 ? a[off + 6] : 0;
                if (s == 0) {
                    a[0] = n0; a[1] = n1; a[2] = n2; a[3] = n3; a[4] = n4; a[5] = n5;
                } else {
                    int q = 64 - s;
                    a[0] = (n0 >> s) | (n1 << q);
                    a[1] = (n1 >> s) | (n2 << q);
                    a[2] = (n2 >> s) | (n3 << q);
                    a[3] = (n3 >> s) | (n4 << q);
                    a[4] = (n4 >> s) | (n5 << q);
                    a[5] = (n5 >> s) | (n6 << q);
                }
            }
        }

        // ---------------------------------------------------------------------
        // Full / approximate squares
        // ---------------------------------------------------------------------

        // sqrU: full square of a 2-word (returns 4-word)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Square(ref InlineArray4<UInt64> o, in InlineArray2<UInt64> x) {
            unchecked {
                UInt128 p10 = (UInt128)x[1] * x[0];
                UInt64 p10x = (UInt64)(p10 >> 127); p10 <<= 1;
                UInt128 p00 = (UInt128)x[0] * x[0];
                UInt128 p11 = (UInt128)x[1] * x[1];
                o[0] = (UInt64)p00;
                o[1] = AddWithCarry((UInt64)(p00 >> 64), (UInt64)p10, 0, out var c);
                o[2] = AddWithCarry((UInt64)(p10 >> 64), (UInt64)p11, c, out c);
                o[3] = AddWithCarry((UInt64)(p11 >> 64), p10x, c, out _);
            }
        }

        // sqrhU: approximate high 128 bits of a 128-bit square
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt128 SquareHigh(UInt128 a) {
            unchecked {
                UInt64 a0 = (UInt64)a, a1 = (UInt64)(a >> 64);
                UInt128 a10 = (UInt128)a1 * a0;
                a10 >>= 63;
                UInt128 a11 = (UInt128)a1 * a1;
                a11 += a10;
                return a11;
            }
        }

        // sqrhu2: approximate high 2-word of a 2-word square
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SquareHigh(ref InlineArray2<UInt64> o, in InlineArray2<UInt64> a) {
            unchecked {
                UInt128 a1a0 = (UInt128)a[1] * a[0];
                UInt128 a1a1 = (UInt128)a[1] * a[1];
                a1a1 += a1a0 >> 63;
                o[0] = (UInt64)a1a1;
                o[1] = (UInt64)(a1a1 >> 64);
            }
        }

        // sqrhu4: approximate high 4-word of a 4-word square
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SquareHigh(ref InlineArray4<UInt64> o, in InlineArray4<UInt64> a) {
            unchecked {
                UInt64 o0, o1, o2, o3;
                UInt128 a2a1 = (UInt128)a[2] * a[1];
                UInt128 a3a0 = (UInt128)a[3] * a[0];
                UInt128 a3a1 = (UInt128)a[3] * a[1];
                UInt128 a3a2 = (UInt128)a[3] * a[2];

                o0 = AddWithCarry((UInt64)a3a1, (UInt64)(a3a0 >> 64), 0, out var c0);
                o1 = AddWithCarry((UInt64)a3a2, (UInt64)(a3a1 >> 64), c0, out c0);
                o2 = AddWithCarry(0, (UInt64)(a3a2 >> 64), c0, out c0);

                o0 = AddWithCarry(o0, (UInt64)(a2a1 >> 64), 0, out c0);
                o1 = AddWithCarry(o1, 0, c0, out c0);
                o2 = AddWithCarry(o2, 0, c0, out c0);

                o0 = AddWithCarry(o0, o0, 0, out c0);
                o1 = AddWithCarry(o1, o1, c0, out c0);
                o2 = AddWithCarry(o2, o2, c0, out c0);
                o3 = c0;

                UInt128 a2a2 = (UInt128)a[2] * a[2];
                UInt128 a3a3 = (UInt128)a[3] * a[3];
                o[0] = AddWithCarry(o0, (UInt64)a2a2, 0, out c0);
                o[1] = AddWithCarry(o1, (UInt64)(a2a2 >> 64), c0, out c0);
                o[2] = AddWithCarry(o2, (UInt64)a3a3, c0, out c0);
                o[3] = AddWithCarry(o3, (UInt64)(a3a3 >> 64), c0, out _);
            }
        }

        // sqrhu5: approximate high 5-word of a 5-word square
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void SquareHigh(ref InlineArray5<UInt64> o, in InlineArray5<UInt64> a) {
            unchecked {
                UInt64 o0, o1, o2, o3, o4, t;
                UInt128 a4a0 = (UInt128)a[4] * a[0];
                UInt128 a3a1 = (UInt128)a[3] * a[1];
                o0 = AddWithCarry((UInt64)(a4a0 >> 64), (UInt64)(a3a1 >> 64), 0, out var c0);
                o1 = c0;
                UInt128 a4a1 = (UInt128)a[4] * a[1];
                UInt128 a3a2 = (UInt128)a[3] * a[2];
                t = AddWithCarry((UInt64)a4a1, (UInt64)a3a2, 0, out var c1);
                o0 = AddWithCarry(o0, t, c0, out c0);
                t = AddWithCarry((UInt64)(a4a1 >> 64), (UInt64)(a3a2 >> 64), c1, out c1);
                o1 = AddWithCarry(o1, t, c0, out c0);
                UInt128 a4a2 = (UInt128)a[4] * a[2];
                o1 = AddWithCarry(o1, (UInt64)a4a2, c0, out c0);
                UInt128 a4a3 = (UInt128)a[4] * a[3];
                t = AddWithCarry((UInt64)a4a3, (UInt64)(a4a2 >> 64), c1, out c1);
                o2 = AddWithCarry(0, t, c0, out c0);
                o3 = AddWithCarry(0, (UInt64)(a4a3 >> 64), c1, out c1);

                o0 = AddWithCarry(o0, o0, 0, out c0);
                o1 = AddWithCarry(o1, o1, c0, out c0);
                o2 = AddWithCarry(o2, o2, c0, out c0);
                o3 = AddWithCarry(o3, o3, c0, out c0);
                o4 = c0;

                UInt128 a2a2 = (UInt128)a[2] * a[2];
                UInt128 a3a3 = (UInt128)a[3] * a[3];
                UInt128 a4a4 = (UInt128)a[4] * a[4];
                o[0] = AddWithCarry(o0, (UInt64)(a2a2 >> 64), 0, out c0);
                o[1] = AddWithCarry(o1, (UInt64)a3a3, c0, out c0);
                o[2] = AddWithCarry(o2, (UInt64)(a3a3 >> 64), c0, out c0);
                o[3] = AddWithCarry(o3, (UInt64)a4a4, c0, out c0);
                o[4] = AddWithCarry(o4, (UInt64)(a4a4 >> 64), c0, out _);
            }
        }

        // ---------------------------------------------------------------------
        // Reciprocal sqrt approximation (rsqrt9)
        // ---------------------------------------------------------------------

        static ReadOnlySpan<UInt32> ReciprocalSqrt9C => [
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
            0x6ce6931d, 0x5cac55b7, 0x234d5496, 0x0ebcefdb, 0x6b7612ec, 0x5b94adb2, 0x229c7cbc, 0x0e56606e
        ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 ReciprocalSqrt9(UInt64 m) {
            unchecked {
                int indx = (int)(m >> 58);
                UInt32 c3 = ReciprocalSqrt9C[indx * 4 + 3];
                UInt32 c0 = ReciprocalSqrt9C[indx * 4 + 0];
                UInt32 c1 = ReciprocalSqrt9C[indx * 4 + 1];
                UInt32 c2 = ReciprocalSqrt9C[indx * 4 + 2];

                UInt64 c0_64 = ((UInt64)c0 << 31) | (1UL << 63);
                UInt64 c1_64 = (UInt64)c1 << 25;

                UInt64 d = (m << 6) >> 32;
                UInt64 d2 = ((UInt64)(d * d)) >> 32;

                UInt64 re = c0_64 + ((d2 * c2) >> 13);
                UInt64 ro = d * ((c1_64 + ((d2 * c3) >> 19)) >> 26) >> 6;

                UInt64 r = re - ro;

                UInt64 r2 = MultiplyHigh(r, r);
                Int64 h = unchecked((Int64)(MultiplyHigh(m, r2) + r2));
                Int64 hr = MultiplyHigh(h, unchecked((Int64)(r >> 1)));

                r = unchecked(r - (UInt64)hr);
                if (r == 0) r--;

                return r;
            }
        }

        // ---------------------------------------------------------------------
        // Classify / range reduction
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte GetClass(UInt128 x) {
            unchecked {
                UInt64 xh = (UInt64)(x >> 64);
                UInt64 xl = (UInt64)x;
                UInt64 t = (xh >> 32) | (((xh << 32) | xl) != 0 ? 1UL : 0UL);
                byte r = 0;
                if (t >= (0x7fffUL << 16)) r++;
                if (t >= (0x7fffUL << 16) + 1) r++;
                if (t >= (0x7fff8UL << 12)) r++;
                return r;
            }
        }

        // sth[] rounded sin(pi/2/72*j)
        static ReadOnlySpan<Int16> AcosSth => [
            11476, 9429, 8096, 7384, 6672, 6053, 5699, 5346, 4995, 4645, 4297,
            4023, 3851, 3680, 3510, 3342, 3174, 3009, 2844, 2681, 2520, 2361,
            2203, 2048, 1894, 1742, 1592, 1445, 1299, 1157, 1016, 878, 742, 609,
            479, 351, 226, 104, -31, -263, -490, -711, -925, -1133, -1335,
            -1531, -1719, -1901, -2105, -2442, -2765, -3074, -3369, -3650,
            -3917, -4240, -4715, -5159, -5574, -5959, -6484, -7134, -7722,
            -8306, -9238, -10046, -11220, -12394, -14139, -16488, -20584
        ];

        // pth[] rounded sin(pi/2/72*j)
        static ReadOnlySpan<UInt32> AcosPth => [
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
            0x1ffe0d
        ];

        // ind[]
        static ReadOnlySpan<Byte> AcosInd => [
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
            0, 0, 0, 0, 0, 0
        ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 JGet(UInt64 x) {
            unchecked {
                UInt64 z = (0x3fffUL << 48) - x;
                UInt64 mz = z << 16;
                Int64 e = (Int64)(z >> 48);
                Int64 nz = (Int64)UInt64.LeadingZeroCount(mz) * (e == 0 ? 1L : 0L);
                mz <<= (int)(nz + (e == 0 ? 1L : 0L));
                e -= nz;
                Int64 lz = (((e << 4) | (Int64)(mz >> 60)) + 161);
                if (lz < 0) lz = 0;
                Int64 j = (Int64)AcosInd[(int)(lz & 0xff)] * (lz < 256 ? 1L : 0L);
                Int64 tz = (e << 11) | (Int64)(mz >> 53);
                return (UInt64)(j + (tz < (Int64)AcosSth[(int)j] ? 1L : 0L));
            }
        }

        // ---------------------------------------------------------------------
        // 1 - x^2 normalization
        // ---------------------------------------------------------------------

        // omx2v2 (approximate)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int OneMinusXSqApprox(ref InlineArray4<UInt64> X2, int s, in InlineArray2<UInt64> x) {
            unchecked {
                Square(ref X2, in x);
                int q = ~s & 63;
                X2[0] = ~((X2[1] << 1 << q) | (X2[0] >> s));
                X2[1] = ~((X2[2] << 1 << q) | (X2[1] >> s));
                X2[2] = ~((X2[3] << 1 << q) | (X2[2] >> s));
                X2[3] = ~(X2[3] >> s);
                int e = 1;
                if (X2[3] != 0) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    int qq = ~lk & 63;
                    X2[3] = (X2[3] << lk) | (X2[2] >> 1 >> qq);
                    X2[2] = (X2[2] << lk) | (X2[1] >> 1 >> qq);
                    X2[1] = (X2[1] << lk) | (X2[0] >> 1 >> qq);
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    X2[0] = AddWithCarry(X2[0], 1, 0, out var c);
                    X2[1] = AddWithCarry(X2[1], 0, c, out c);
                    X2[2] = AddWithCarry(X2[2], 0, c, out _);
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    int qq = ~lk & 63;
                    X2[3] = (X2[2] << lk) | (X2[1] >> 1 >> qq);
                    X2[2] = (X2[1] << lk) | (X2[0] >> 1 >> qq);
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // omx2v3 (exact)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int OneMinusXSqExact(ref InlineArray4<UInt64> X2, int s, in InlineArray2<UInt64> x) {
            unchecked {
                Square(ref X2, in x);
                int q = ~s & 63;
                X2[0] = (X2[1] << 1 << q) | (X2[0] >> s);
                X2[1] = (X2[2] << 1 << q) | (X2[1] >> s);
                X2[2] = (X2[3] << 1 << q) | (X2[2] >> s);
                X2[3] = X2[3] >> s;
                X2[0] = SubtractWithBorrow(0, X2[0], 0, out var c);
                X2[1] = SubtractWithBorrow(0, X2[1], c, out c);
                X2[2] = SubtractWithBorrow(0, X2[2], c, out c);
                X2[3] = SubtractWithBorrow(0, X2[3], c, out _);
                int e = 1;
                if (X2[3] != 0) {
                    int lk = (int)UInt64.LeadingZeroCount(X2[3]);
                    int qq = ~lk & 63;
                    X2[3] = (X2[3] << lk) | (X2[2] >> 1 >> qq);
                    X2[2] = (X2[2] << lk) | (X2[1] >> 1 >> qq);
                    X2[1] = (X2[1] << lk) | (X2[0] >> 1 >> qq);
                    X2[0] = X2[0] << lk;
                    e += lk;
                } else {
                    int lk = (int)UInt64.LeadingZeroCount(X2[2]);
                    int qq = ~lk & 63;
                    X2[3] = (X2[2] << lk) | (X2[1] >> 1 >> qq);
                    X2[2] = (X2[1] << lk) | (X2[0] >> 1 >> qq);
                    X2[1] = X2[0] << lk;
                    X2[0] = 0;
                    e += lk + 64;
                }
                return e;
            }
        }

        // ---------------------------------------------------------------------
        // getcos: sqrt(1 - x^2)
        // ---------------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int GetCos(ref InlineArray5<UInt64> sq, int ex, in InlineArray2<UInt64> x) {
            unchecked {
                InlineArray4<UInt64> x2 = default;
                int e = OneMinusXSqExact(ref x2, 2 * (ex - 1), in x);

                UInt64 r = ReciprocalSqrt9((x2[3] << 1) | (x2[2] >> 63));
                UInt64 rsqrt2Factor = (e & 1) == 0 ? ~0UL : 0xb504f333f9de6484UL;
                r = MultiplyHigh(r, rsqrt2Factor);
                BigMul(ref sq, r, in x2);
                InlineArray6<UInt64> h = default;
                BigMul(ref h, r, in sq);
                ShiftRight(ref h, 2);

                Int64 msk = unchecked((Int64)h[4]) >> 63;
                UInt64 mskU = unchecked((UInt64)msk);
                h[4] ^= mskU; h[3] ^= mskU; h[2] ^= mskU; h[1] ^= mskU; h[0] ^= mskU;

                // h2s (5 words starting at index 1)
                InlineArray4<UInt64> h_hi_4 = default;
                h_hi_4[0] = h[1]; h_hi_4[1] = h[2]; h_hi_4[2] = h[3]; h_hi_4[3] = h[4];
                InlineArray5<UInt64> h2s = default;
                InlineArray4<UInt64> h2s_inner = default;
                SquareHigh(ref h2s_inner, in h_hi_4);
                h2s[1] = h2s_inner[0]; h2s[2] = h2s_inner[1]; h2s[3] = h2s_inner[2]; h2s[4] = h2s_inner[3];

                InlineArray2<UInt64> h4s = default;
                InlineArray2<UInt64> h2s_tail = default;
                h2s_tail[0] = h2s[3]; h2s_tail[1] = h2s[4];
                SquareHigh(ref h4s, in h2s_tail);

                InlineArray5<UInt64> h3s = default;
                InlineArray3<UInt64> h_hi_3 = default;
                h_hi_3[0] = h[2]; h_hi_3[1] = h[3]; h_hi_3[2] = h[4];
                InlineArray3<UInt64> h2s_hi_3 = default;
                h2s_hi_3[0] = h2s[2]; h2s_hi_3[1] = h2s[3]; h2s_hi_3[2] = h2s[4];
                InlineArray3<UInt64> h3s_hi_3 = default;
                MultiplyHigh(ref h3s_hi_3, in h_hi_3, in h2s_hi_3);
                h3s[2] = h3s_hi_3[0]; h3s[3] = h3s_hi_3[1]; h3s[4] = h3s_hi_3[2];

                // h2s (index 1..4) *= 3
                {
                    InlineArray4<UInt64> t = default;
                    t[0] = h2s[1]; t[1] = h2s[2]; t[2] = h2s[3]; t[3] = h2s[4];
                    MultiplyBy3(ref t);
                    h2s[1] = t[0]; h2s[2] = t[1]; h2s[3] = t[2]; h2s[4] = t[3];
                }

                // h3s (index 2..4) *= 5
                {
                    InlineArray3<UInt64> t = default;
                    t[0] = h3s[2]; t[1] = h3s[3]; t[2] = h3s[4];
                    MultiplyBy5(ref t);
                    h3s[2] = t[0]; h3s[3] = t[1]; h3s[4] = t[2];
                }

                UInt128 t4u = ((UInt128)h4s[1] << 64) | h4s[0];
                t4u += (t4u * 3) >> 5;
                InlineArray5<UInt64> t4 = default;
                t4[3] = (UInt64)t4u; t4[4] = (UInt64)(t4u >> 64);

                h2s[0] = 0;
                ShiftRight(ref h2s, 62 - (e & 1));
                h3s[0] = 0; h3s[1] = 0;
                ShiftRight(ref h3s, 59 + 64 - 2 * (e & 1));
                ShiftRight(ref t4, 56 + 128 - 3 * (e & 1));

                if (msk != 0) {
                    Add(ref h.AsInlineArray5(), in h.AsInlineArray5(), in h2s);
                    Add(ref h.AsInlineArray5(), in h.AsInlineArray5(), in h3s);
                    Add(ref h.AsInlineArray5(), in h.AsInlineArray5(), in t4);
                } else {
                    Subtract(ref h.AsInlineArray5(), in h.AsInlineArray5(), in h2s);
                    Add(ref h.AsInlineArray5(), in h.AsInlineArray5(), in h3s);
                    Subtract(ref h.AsInlineArray5(), in h.AsInlineArray5(), in t4);
                }

                InlineArray5<UInt64> h_shifted = default;
                h_shifted[0] = h[0]; h_shifted[1] = h[1]; h_shifted[2] = h[2]; h_shifted[3] = h[3]; h_shifted[4] = h[4];
                BigMul(ref h, r, in h_shifted);

                InlineArray5<UInt64> x2l = default;
                x2l[1] = x2[0]; x2l[2] = x2[1]; x2l[3] = x2[2]; x2l[4] = x2[3];

                // mhu5u5u5(h, h+1, x2l);
                MultiplyHigh(ref h.AsInlineArray5(), in h.Skip1(), in x2l);
                ShiftRight(ref h.AsInlineArray5(), 62 - (e & 1));

                if (msk == 0) {
                    Subtract(ref sq, in sq, in h.AsInlineArray5());
                } else {
                    Add(ref sq, in sq, in h.AsInlineArray5());
                }
                return e;
            }
        }

        // ---------------------------------------------------------------------
        // Polynomial evaluation
        // ---------------------------------------------------------------------

        // mhu1u1u1 helpers (single-element)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void MultiplyHigh(ref UInt64 o, UInt64 b, UInt64 a) {
            unchecked { o = (UInt64)(((UInt128)a * b) >> 64); }
        }

        static ReadOnlySpan<UInt64> EvalPolyCoeffs => [
            0x1343996b9f42b9f5, 0x255e6e351770584d, 0x00000000000000a3, 0x5e2111cba47a2b05, 0x000000000005717d,
            0xa97f20b758a855cd, 0x000000002ea1bcc9, 0x889c99395996e6ce, 0x00000190cb77f60c, 0xc7476c854bade5bf,
            0x000d8137abd89d89, 0x97b4ea2813d93845, 0x74f4aa383759f229, 0x5abb1888e58be523, 0x5f1f6db6db6db6db,
            0x00000000000003f9, 0x9a1160a9ab2539ce, 0xa8ba2e8ba2e8ba2e, 0x000000000022bdd3, 0x72ec43b868c4b3c0,
            0xf7bdef7bdef7bdef, 0x0000000131683bde, 0x4b2852d709bf2295, 0x58469ee58469ee58, 0x00000a8dd18469ee,
            0x2d86e53634cafb09, 0x684bda12f684bda1, 0x005e0b7684bda12f, 0x151d85735049738f, 0xe147ae147ae147ae,
            0x4d0c7ae147ae147a, 0x0000000000000003, 0x9ba6f1b2735cae39, 0x6f4de9bd37a6f4de, 0xbd37a6f4de9bd37a,
            0x0000000000001df3, 0xcf46c00a8ed8a2e2, 0x3cf3cf3cf3cf3cf3, 0xf3cf3cf3f3cf3cf3, 0x000000000112ef3c,
            0x86baeba7afbb9dd6, 0xbca1af286bca1af2, 0xa1af286bca1af286, 0x00000009fef286bc, 0xe1e21d9d6b73053d,
            0xe1e1e1e1e1e1e1e1, 0xe1e1e1e1e1e1e1e1, 0x00005ea1e1e1e1e1, 0x33332cfa4ccaad37, 0x3333333333333333,
            0x3333333333333333, 0x0393333333333333, 0x89d89e04e6327ae5, 0xd89d89d89d89d89d, 0x9d89d89d89d89d89,
            0x89d89d89d89d89d8, 0x0000000000000023, 0x8ba2e8b369ee2b14, 0xe8ba2e8ba2e8ba2e, 0x2e8ba2e8ba2e8ba2,
            0xa2e8ba2e8ba2e8ba, 0x0000000000016e8b, 0x8e38e38e78e717bc, 0x38e38e38e38e38e3, 0xe38e38e38e38e38e,
            0x8e38e38e38e38e38, 0x000000000f8e38e3, 0x6db6db6db566fac3, 0xb6db6db6db6db6db, 0xdb6db6db6db6db6d,
            0x6db6db6db6db6db6, 0x000000b6db6db6db, 0x99999999999e1925, 0x9999999999999999, 0x9999999999999999,
            0x9999999999999999, 0x0009999999999999, 0xaaaaaaaaaaaaa521, 0xaaaaaaaaaaaaaaaa, 0xaaaaaaaaaaaaaaaa,
            0xaaaaaaaaaaaaaaaa, 0xaaaaaaaaaaaaaaaa
        ];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void EvalPoly(ref InlineArray5<UInt64> f, in InlineArray5<UInt64> t2) {
            unchecked {
                int ck = 0;
                var cp = EvalPolyCoeffs;

                f[0] = cp[ck];
                MultiplyHigh(ref f[0], f[0], t2[4]); ck += 1;
                f[0] = unchecked(cp[ck] + f[0]);

                f[1] = cp[ck + 1];
                {
                    InlineArray2<UInt64> f2 = default;
                    f2[0] = f[0]; f2[1] = f[1];
                    InlineArray2<UInt64> t2_23 = default;
                    t2_23[0] = t2[3]; t2_23[1] = t2[4];
                    InlineArray2<UInt64> tmp = default;
                    MultiplyHigh(ref tmp, in f2, in t2_23);
                    f[0] = tmp[0]; f[1] = tmp[1];
                }
                ck += 2;
                {
                    InlineArray2<UInt64> a = default;
                    a[0] = cp[ck]; a[1] = cp[ck + 1];
                    InlineArray2<UInt64> f2 = default;
                    f2[0] = f[0]; f2[1] = f[1];
                    InlineArray2<UInt64> tmp = default;
                    Add(ref tmp, in a, in f2);
                    f[0] = tmp[0]; f[1] = tmp[1];
                }
                // Repeat 5 more mulhi2 + add2
                for (int rep = 0; rep < 5; rep++) {
                    InlineArray2<UInt64> f2 = default;
                    f2[0] = f[0]; f2[1] = f[1];
                    InlineArray2<UInt64> t2_23 = default;
                    t2_23[0] = t2[3]; t2_23[1] = t2[4];
                    InlineArray2<UInt64> tmp = default;
                    MultiplyHigh(ref tmp, in f2, in t2_23);
                    f[0] = tmp[0]; f[1] = tmp[1];
                    ck += 2;
                    InlineArray2<UInt64> a = default;
                    a[0] = cp[ck]; a[1] = cp[ck + 1];
                    InlineArray2<UInt64> f2b = default;
                    f2b[0] = f[0]; f2b[1] = f[1];
                    InlineArray2<UInt64> tmp2 = default;
                    Add(ref tmp2, in a, in f2b);
                    f[0] = tmp2[0]; f[1] = tmp2[1];
                }

                f[2] = cp[ck + 2];
                for (int rep = 0; rep < 5; rep++) {
                    InlineArray3<UInt64> f3 = default;
                    f3[0] = f[0]; f3[1] = f[1]; f3[2] = f[2];
                    InlineArray3<UInt64> t2_234 = default;
                    t2_234[0] = t2[2]; t2_234[1] = t2[3]; t2_234[2] = t2[4];
                    InlineArray3<UInt64> tmp = default;
                    MultiplyHigh(ref tmp, in f3, in t2_234);
                    f[0] = tmp[0]; f[1] = tmp[1]; f[2] = tmp[2];
                    ck += 3;
                    InlineArray3<UInt64> a = default;
                    a[0] = cp[ck]; a[1] = cp[ck + 1]; a[2] = cp[ck + 2];
                    InlineArray3<UInt64> f3b = default;
                    f3b[0] = f[0]; f3b[1] = f[1]; f3b[2] = f[2];
                    InlineArray3<UInt64> tmp2 = default;
                    Add(ref tmp2, in a, in f3b);
                    f[0] = tmp2[0]; f[1] = tmp2[1]; f[2] = tmp2[2];
                }

                f[3] = cp[ck + 3];
                for (int rep = 0; rep < 6; rep++) {
                    InlineArray4<UInt64> f4 = default;
                    f4[0] = f[0]; f4[1] = f[1]; f4[2] = f[2]; f4[3] = f[3];
                    InlineArray4<UInt64> t2_1234 = default;
                    t2_1234[0] = t2[1]; t2_1234[1] = t2[2]; t2_1234[2] = t2[3]; t2_1234[3] = t2[4];
                    InlineArray4<UInt64> tmp = default;
                    MultiplyHigh(ref tmp, in f4, in t2_1234);
                    f[0] = tmp[0]; f[1] = tmp[1]; f[2] = tmp[2]; f[3] = tmp[3];
                    ck += 4;
                    InlineArray4<UInt64> a = default;
                    a[0] = cp[ck]; a[1] = cp[ck + 1]; a[2] = cp[ck + 2]; a[3] = cp[ck + 3];
                    InlineArray4<UInt64> f4b = default;
                    f4b[0] = f[0]; f4b[1] = f[1]; f4b[2] = f[2]; f4b[3] = f[3];
                    InlineArray4<UInt64> tmp2 = default;
                    Add(ref tmp2, in a, in f4b);
                    f[0] = tmp2[0]; f[1] = tmp2[1]; f[2] = tmp2[2]; f[3] = tmp2[3];
                }

                f[4] = cp[ck + 4];
                for (int rep = 0; rep < 5; rep++) {
                    InlineArray5<UInt64> f5 = default;
                    f5[0] = f[0]; f5[1] = f[1]; f5[2] = f[2]; f5[3] = f[3]; f5[4] = f[4];
                    InlineArray5<UInt64> t2b = default;
                    t2b[0] = t2[0]; t2b[1] = t2[1]; t2b[2] = t2[2]; t2b[3] = t2[3]; t2b[4] = t2[4];
                    InlineArray5<UInt64> tmp = default;
                    MultiplyHigh(ref tmp, in f5, in t2b);
                    f[0] = tmp[0]; f[1] = tmp[1]; f[2] = tmp[2]; f[3] = tmp[3]; f[4] = tmp[4];
                    ck += 5;
                    InlineArray5<UInt64> a = default;
                    a[0] = cp[ck]; a[1] = cp[ck + 1]; a[2] = cp[ck + 2]; a[3] = cp[ck + 3]; a[4] = cp[ck + 4];
                    InlineArray5<UInt64> f5b = default;
                    f5b[0] = f[0]; f5b[1] = f[1]; f5b[2] = f[2]; f5b[3] = f[3]; f5b[4] = f[4];
                    InlineArray5<UInt64> tmp2 = default;
                    Add(ref tmp2, in a, in f5b);
                    f[0] = tmp2[0]; f[1] = tmp2[1]; f[2] = tmp2[2]; f[3] = tmp2[3]; f[4] = tmp2[4];
                }
            }
        }

        // ---------------------------------------------------------------------
        // Data used by Acos
        // ---------------------------------------------------------------------

        // c[][2]: 10 entries of 2 u64, flattened
        static ReadOnlySpan<UInt64> AcosC => [
            0xaaaaaaaaaaaaaaa9, 0xaaaaaaaaaaaaaaaa,
            0x333333333333337a, 0x0013333333333333,
            0xb6db6db6db6dae36, 0x000002db6db6db6d,
            0x71c71c71c71cfc25, 0x000000007c71c71c,
            0x2e8ba2e8ba29804e, 0x000000000016e8ba,
            0x3b13b13b13ce93b6, 0x0000000000000471,
            0xe4cccccccc5f2a5a, 0x0000000000000000,
            0x002f50f0f1f806dc, 0x0000000000000000,
            0x000009fef0fec73a, 0x0000000000000000,
            0x0000000227286573, 0x0000000000000000
        ];

        static ReadOnlySpan<UInt64> rsqrt2_64 => [~0UL, 0xb504f333f9de6484UL];
    }


    public static partial class Binary128Arithmetic {

        public static UInt64 Acos(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => AcosCore(lo, hi, MidpointRounding.ToEven, out result_hi);

        public static UInt64 Acos(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi)
            => AcosCore(lo, hi, mode, out result_hi);

        // ---------------------------------------------------------------------
        // Data needed for Acos
        // ---------------------------------------------------------------------

        // cth[j] : 5-word entries, flatten to ReadOnlySpan<UInt64>
        static ReadOnlySpan<UInt64> AcosCthFlat => [
            0, 0, 0, 0, 0,
        0x64d23d92f9ea2771, 0x6d047db3a6e206a7, 0x2953c428c9d7bc56, 0xfb0f1838d5a3a8da, 0xfff06594451d279d,
        0xe3080aa3bd169d0c, 0x4054eae9456537db, 0x9e5833eae773bc87, 0x6f49ccb466446b89, 0xffc19bc9271a05b3,
        0x00912c8144374c85, 0x309a44ddae7556ce, 0xc319285201c58b56, 0x3a246e54beb8dac9, 0xff73917b77aa5c47,
        0x6a7360619e7edb23, 0x0b11b912de529986, 0x5a6995a8cc7d5eac, 0x552a6d7261223cc4, 0xff069a0439cbe651,
        0x238522ea64680061, 0xf079215ebcd5ca36, 0x290de5267799873e, 0xcab4f035a6bfa060, 0xfe7a55701a8697b0,
        0x7a92124309b60ef2, 0x4b9dd14818169efc, 0x040ba2edca526f4e, 0xac4772d0d4932b9b, 0xfdcf16b94b0b442e,
        0x30ff410c35a4b64d, 0xd1d820652a9bc4c6, 0x6d055dc0a4c09600, 0xb4aab1b03d022116, 0xfd04e2530bcbd671,
        0x03aedba8702f81c3, 0x96da5af0d3ea2f8b, 0xca571fb2cbdee5b9, 0x17c0bd0e66183953, 0xfc1bb125286c39d8,
        0xfb91abaebc934604, 0xce94d1434f07a0a1, 0x068284240ceeae19, 0x5e9a55955f4b097f, 0xfb143c17bdcc996d,
        0xab794690d17636d8, 0xc8e308f9bd76c0e2, 0x15fddd7233c65eed, 0xdb301a162cf8115d, 0xf9edc73d6c923555,
        0x593fea054c7f450e, 0x6b0aa37cffc886a5, 0xd8e480c9ebb134e1, 0x287dce6a9f8ea2ac, 0xf8a922ae090b0799,
        0x251fef9c8bb46028, 0x493c5f39729df2a1, 0xc745b46aca55d0e8, 0x2062f831fa08f4af, 0xf7454c2c7d12b97a,
        0xff1ffe460bd9c836, 0xb16e3e5300ab04b0, 0xcc500109643b1f25, 0x1bf03e7ae9dd3a2a, 0xf5c455407155fe59,
        0x4606e3f3b69b5b9f, 0xaf57d7aca36b7036, 0x23dc96105c0b9d9b, 0x0e73cd55dd020b9f, 0xf4253c1c1a1ae532,
        0x3e9033c3d73b8ba0, 0x36a1f12dab4107b3, 0xe2bf1fc80065cf4b, 0x067308deced752e6, 0xf267ed62f8b7c795,
        0xdc78f0c65249d290, 0x3da244d00c619d0d, 0xd5109661f43e9ecd, 0x70a15831a61bca6b, 0xf08f329d05a331b0,
        0x2e799ef643386021, 0xf50e2c1a3600d3ad, 0x828f074191858249, 0x4f6a116da6359a61, 0xee953e620e55df7b,
        0xf59cf516cfc8aef3, 0x3994459558477033, 0xcfbcad100d166a9e, 0x6632028b37ff16ad, 0xec832dd8f9584e23,
        0x8993bed4bc91f71b, 0x2d156655d9d43f98, 0x7386f1c67cf345e5, 0xd03960687c6a9efe, 0xea4f644f48136890,
        0x6c1be9d31d8598fe, 0x0bbc7599cc98b839, 0x1b0cb81a981ed38d, 0x74401bc33d3f4f33, 0xe800632c0e1d2f2c,
        0xd77e840a65911d5a, 0x0409ada21048c2b1, 0x54c0567f7377993b, 0xb61bea5c37779619, 0xe59668e019f1e288,
        0xdf264c217da2460f, 0x4310bc9fc7837821, 0xa5cb92ab27280ab6, 0x03513aa71030fbb2, 0xe311a97671f44f1c,
        0x8ee20e415370de4e, 0xb85f2ea14495bf6f, 0xa12912f50e813792, 0x0f451e45046e7646, 0xe06dea9a80d1fdf6,
        0x2485e7ecaf78aedf, 0x639053243722d371, 0x92ec1a6629ed23cc, 0x92ba16b83c5c1dc4, 0xddb3d742c265539d,
        0x7d45353940ee462b, 0x81cfccf2da1a22d6, 0x4db6039c8f8f6015, 0x83992b6bb6c89121, 0xdada7cc9882f1832,
        0xb0de57772eac59f6, 0x855009022ef97eb9, 0x8549b723bab4be2d, 0x27e229b54b803697, 0xd7e6403e36d2f1f7,
        0xe6c36c5ad24b30c1, 0x5258a95fc94e2fb8, 0x63aea18a41e1bf81, 0x2a16383f5c4d5062, 0xd4d7154f370bbe07,
        0x4f980d926321df45, 0xad92bc366eaf4837, 0x360b41691ba7a5f0, 0x010bd7d48ab25527, 0xd1b27b4ae9008ef2,
        0xb53f05eae4761ab3, 0x5636614d38c536c1, 0x9cfea8981156d5dd, 0x0bb008a15e850cec, 0xce6d5666b787ca77,
        0x80638f93f4a2c269, 0x1dcdbcdbbcf14233, 0xae4218b2749a67be, 0x69d2608c3b73e12f, 0xcb190ae896231bce,
        0xe516cefc6347a5e9, 0x9684ba1ed4646c7a, 0x5f831bdef11f9bbc, 0xc6158124a4ef775f, 0xc7a3b782716d38c8,
        0x8f005279faa6880d, 0x890f8c878e5f7df9, 0x3485668a4e6e9637, 0xcf01d7d5779f0507, 0xc419953003b5c74b,
        0x744fe29890f407f9, 0x6f46175fab644dfc, 0xd9330605ec9b4e0a, 0xa322920e8bd7accf, 0xc07416e77d712462,
        0xab325fd93ce9b6b8, 0x57c460f1a312c3dd, 0x0fcf1122ee32b17c, 0x892c26b0bcfacc8d, 0xbcba10b0d8770ed2,
        0xcef1e69c4eadb8b3, 0x7f131158279ff4d7, 0x4e6ba5183b584705, 0x8fcd4c453517e8ba, 0xb8ebe30ed9fac186,
        0x3745f18539bf8772, 0x2cee4ab653475e6e, 0xc28307bea98bb0e7, 0x5cbc46ee3d37b72d, 0xb501e65acbad5ad8,
        0xf4c3a3a64ddafd51, 0x5e65313f078e13ab, 0xe6ba3e11cdb88afb, 0xf74e3a8ab016619f, 0xb103b40770404b83,
        0x539c48de71a3cc8e, 0x7f208cb9376741f7, 0xa22a23cb0d6b1468, 0x3fd4750afb3804bb, 0xacf1860da3cfd826,
        0xbc5eb21dc1786f2d, 0xcaf9195f5c2505ba, 0xf402f48b30c4df2f, 0x4987496d863e1e7c, 0xa8c6fae0bbd09645,
        0x61f8e4beffcf379f, 0x758e5ddd22563b4f, 0x80d42d484f061220, 0xfbc0967b24f94966, 0xa48d1e8d86992cdc,
        0xcdf0fca2bcbaff85, 0x201f9ee54d8615fa, 0x7a56fc977f0d0e4c, 0x754bb974869014d6, 0xa03aa9d87a93f00d,
        0xcae22303aeaf6595, 0xe7b53a3891af1ae5, 0x6de54e5c6a03779d, 0x6cf567b89889fef8, 0x9bd425459273d285,
        0x9831e7542d85d7ea, 0x611013ca24685535, 0xa3724e71c24123d7, 0xb849ff7aea680179, 0x975eea2b65a9847e,
        0x101a86b895c7d937, 0xe88c06711a5ab702, 0xfd217ba2d78129ec, 0x20b672429fb3d1dd, 0x92d5d4e3fc1f9cbd,
        0x4d91c0b5749d4914, 0x8ed80817147623b6, 0xb90bd29f73f58752, 0x5ff44c46ffccf7f3, 0x8e38a46bdb3b3d12,
        0x2efea8ce593572c2, 0xf37f1d4704fb5c49, 0xf4c2e89353ada913, 0xedbf03f73c1d3162, 0x8986f9b6c7132cff,
        0x35b06cbf85f84fbb, 0xbf788966cdcd56b0, 0x91e498b6c64670dc, 0xf6e11dfe5f762e4d, 0x84cd81e775b4c098,
        0x539ba47e0b57d2de, 0x974b441dfa650168, 0xa477f58bc7c8dc47, 0x29dd7114d81c96bf, 0x7fffb96fec8ce447,
        0x3d5bcfe7068c0ff1, 0x5016bbcd4ef1b8e7, 0x6f1857955007a55c, 0x98ea27d884420256, 0x7b208eaab0b94056,
        0x846ef313e2e0bf68, 0xabfac2142052bc6c, 0x8496c1983f214586, 0xe132acebbd5846b8, 0x7633823fd4eeef51,
        0x03681a2fcb57c1e9, 0x999fd17f45cc07f3, 0x411f4be11fd95eb2, 0xa206ce1148e4933c, 0x7138b866dbf2205c,
        0x701d2b27adfe0fb8, 0x239f4fe8463aee07, 0xe0b2bb9935920222, 0x5d94ac1eeb7461f9, 0x6c3040ff7ae4f3d5,
        0x064f259d5d960d10, 0xb194cf34ff2bf070, 0x5f46977eac6f5a5c, 0x4367583037f40763, 0x671a12bd2092f0e6,
        0x739008a332539b2b, 0x34d36b62e94d21ec, 0xf32753bd933f314c, 0x9a57aae41393eb49, 0x61f604a26e818217,
        0x4cda609f93951dc0, 0xe4f6c5ba31b5c6ed, 0x4fa0c524c392182a, 0x37df71c28f1de586, 0x5cc3c513b7fdb337,
        0x9a01a8613fd798eb, 0x266e72c3de2e292e, 0x59fec1c23334cf3a, 0x2e578575cc9228df, 0x578dcbb6628e3c6f,
        0x1f1a07c6d6b6adcd, 0x1eb9dc19b8598724, 0xd9ab93ace24809dc, 0x05dab10687beb878, 0x5246f2e8fd06b80b,
        0xb8a6a56a834058eb, 0xe60a200e3eb8678a, 0xe50f6f2cd1d3aace, 0xedb16a84844fc0a7, 0x4cfa66f577d9eaf3,
        0x69aab942d6b77874, 0x9d6df99ad27e5270, 0x80152267d988b733, 0xdbe4096c369e089e, 0x47a24816dd512c95,
        0x0550f19403d41e0a, 0xbb071ead370af5ff, 0xb9b5812340f07744, 0x64f8889a905e2862, 0x4241a5c4a8543894,
        0xacd334aee8e9e8a3, 0x92d46ccf664aa8e9, 0x9d79c6111152cb8a, 0x4b40d1af34812df0, 0x3cd8779db76341cd,
        0x0403e507d395ba75, 0x4c8823c7ba7c9380, 0x0a03c989401a778b, 0x14849e09356ec1dc, 0x37667d4c98bfbccb,
        0x657bb80e7a20116a, 0xa79b3d94f1414a71, 0x86ab887459d6023c, 0x602a110d5bf3bfc0, 0x31f02728a1820184,
        0x531dfb48ce58358d, 0xe827b8eac551e9ee, 0x1b75725ed755aa4a, 0xc588e51fb5f4fd50, 0x2c736b07088a07ba,
        0x90c4aa84e4598669, 0x7dd80f90a7af46ad, 0xa23d2ad47df691ab, 0x4e35d5f602d9b24d, 0x26efff9c8eff17d3,
        0x06b8ea90e4449667, 0x3b72f4405f272c6f, 0xe689513157c5339a, 0xa73888a873793997, 0x2168e05fb8858700,
        0xc343b6652a5c735a, 0x18fae9a2c9f7cf27, 0x7e0c3ca1daac2e97, 0xbb8233abbcf0c73a, 0x1bde7b65ebd95dd2,
        0x3097e3126b116bd9, 0x35782cb810ce1724, 0x5ca4cd631e80444d, 0xd2eff3545c8a56c1, 0x164fbb9bd36b968c,
        0xe12e48b0cf8a0b8e, 0x402a65318c77466e, 0xfffbc69461af1af2, 0x0d511d6a919beca2, 0x10be2e7affde1a24,
        0xed8e7549e2530cad, 0x419a20b85f378012, 0x4d48fe6abd0a9a0e, 0x40b937dae10995f7, 0x0b2a9f7bd1a4e3f7,
        0x0ccf7c9e0c0fe38a, 0xf496f2cb6c0dc6ca, 0x5c67a95d27203004, 0x87e16185128e709b, 0x059591109ebd190a,
    ];

        static ReadOnlySpan<UInt64> AcosCth(int j) =>
            AcosCthFlat.Slice(j * 5, 5);

        // phi0[j] : 5-word entries, flatten to ReadOnlySpan<UInt64>
        static ReadOnlySpan<UInt64> AcosPhi0Flat => [
    0, 0, 0, 0, 0,
    0x98ee01dc297ef137, 0x833839e554b92130, 0x0a35967bc3749afb, 0x09800df8ddc2a5e2, 0x016587438f269b93,
    0xcfec743ac824b7b9, 0x66ec76eb4fb09df3, 0x5265df8b7c3f33c1, 0xdbf35587e9e2cd87, 0x02cafa166dc09244,
    0xd9ea018b49a9a7ac, 0x2e80d2fd5768e869, 0x7f990060e27ca191, 0xacfb3347cdab5663, 0x0430c42ffb6051ae,
    0x5a7d59fe33fbd45c, 0xfac32e4e4b697d19, 0x14a12d811a08c9d1, 0xa458339e940758e1, 0x0595d06ea0a22010,
    0xbc86efe2e5f48417, 0x8e3ee3ae76d8513a, 0x640a9fc17520d677, 0xb5cab3f8d569e3b5, 0x06fb8b520dddc5be,
    0xb768418745f28dd9, 0x787494c4af664176, 0xbef4267e62a7f37b, 0x3b5e3b804feb82b8, 0x08611f97ec71f4d0,
    0x1568e021e2ba7c88, 0x389d69e8437f8a69, 0xdd1382a0d543b01a, 0x496cf2cedfaae306, 0x09c6b8bdd13f755e,
    0xd981fe8928879085, 0xca5329c38c2cf11b, 0x868a81770bf58deb, 0x9b9c832d762e0355, 0x0b2c82a5715a2540,
    0x803682d986d5d261, 0xb4845f3eefebbbad, 0x600541ff7c2ecceb, 0xbcafd279b45b8783, 0x0c91a4c2050c21ab,
    0xd48a7991528086ac, 0x24911228dfca1969, 0xa86d3f15c0c947ae, 0x910e90af3cd12b01, 0x0df74ef328432b11,
    0xc94f05bb785212ba, 0xed4dd3fe4e1a18f8, 0xc8ba7d4bbf0dd556, 0xfbac51efb6caa9ee, 0x0f5ca782c5497407,
    0x6774ecd16dbef24d, 0x5c455230e3e27926, 0x22479a62d912a3bc, 0xe7c0193f47088dff, 0x10c2e217f6ce32d4,
    0x065dc6d152815c75, 0xf04a70b98dd4a723, 0xcbdbb25e22f5f0bb, 0xdf4937856b85a07e, 0x1228197097350eb6,
    0xe9f602011b282870, 0xe82bd25b373ed1ad, 0x8a4dc661188d900b, 0xf6444858ac431981, 0x138d81148233691b,
    0x8fea4147ae4dc654, 0x98cf4ab5bfef3e64, 0xe4a1c1562fca8d98, 0xf0368df5f848a51d, 0x14f345fb14d06d08,
    0x38d72d4e6735e238, 0x7a2d62272a639ab7, 0xa612c8dc05aeca56, 0x39aadf69fd05412f, 0x16577574c736f822,
    0x8b0a7170caea9d5b, 0x27c1cf7c1a06fa14, 0x02069a770726ca5e, 0x7c4f696ede5fa314, 0x17be7c689b29a551,
    0x7932d250fdc19ae3, 0xd23bc7e1175b5ba7, 0xec29fc6fd46e189f, 0x88b15e01adf30d3a, 0x19221b18a2b52525,
    0x4123f1d8054542c2, 0x45ee3add846d2f88, 0x93f2249af814dad5, 0xfc147c180f97b340, 0x1a88f3ec0c7019e8,
    0x2f5534f59b39f35c, 0x6f6a9ab389dc2e64, 0xf6b8e5b0d1afdc91, 0xb4b3f8775f0ba4af, 0x1bee124e5606f4a,
    0xab6580dffc5cad8f, 0xbd7036041b733567, 0x5cb33945d1250ab6, 0x1218f911043d7131, 0x1d540811073115d0,
    0x066aa0a3134c137e, 0x59c6c881ab04c92d, 0x9e482027d47ee788, 0xd49d73f1d94d5abf, 0x1eb88e86a05e1646,
    0x44c6d7e7bf534b9c, 0xb06417cd6f4b50c4, 0xea20eca3a5ba0bb9, 0xfac469c187134857, 0x201ee316b08cad79,
    0x62e1ac14425e00d0, 0x55ac9fc65f2def30, 0xdc2b0d016c66a213, 0xcb7665c1eacf5a22, 0x2182a4705ae6cb08,
    0xa9b283dac596b605, 0x5fa4861e7ef2b865, 0x25b5e8f0c4f84141, 0x312668a01c646d4b, 0x22e89368619cc920,
    0x1870eef39e4d7560, 0x268dfff2754d4cf9, 0x72e8040d7d970a1b, 0x714ad8d90d614cae, 0x244e9391c2c115b7,
    0x4aa29cb31acb8056, 0x0bf09c95910a22e1, 0xb94dbede99a3fb07, 0xd047ce796631eca9, 0x25b4d25453036018,
    0xe3ba19c8732cb2ff, 0x85c23d5ca072439d, 0x468f8bb315059a40, 0xed7e66087d1bd175, 0x27190e2b73a92c06,
    0x3dabe71847949223, 0x68a8213d4d431f91, 0x62f05cf2b3bd2710, 0x1bfe511df99c3f24, 0x28805177276dca59,
    0x5ed25eca0b62e3d2, 0x414b4ddbedbba46c, 0x806bc58f612ad096, 0x19542ac43ea8cca6, 0x29e35e91b23af2e6,
    0x461794962913e34a, 0x2173dec430ab5f19, 0xd86f9df8a8a1ca1e, 0x62a1e34bd1046ac2, 0x2b49dc99a7f107ed,
    0xf6894f18599bbbc3, 0xfb42ed3f8092e674, 0x211376f5bd95b6f5, 0xa9954fa4b6a3ae67, 0x2caeee56ee9d76e1,
    0xe1237822d72fcc91, 0x6234f9e5fc87cd58, 0x5b561e852695a9c9, 0x27a4ac920cc2d485, 0x2e1555560bf870eb,
    0x5a660506d1ac44f0, 0x52288e69a1a6979a, 0x0a8fd342fa39186a, 0xfdd3cc3e53529c31, 0x2f7a9c03b2c9f414,
    0x9ca4a66d94f7ce98, 0xb3cc7b754ea110e8, 0x6b2742ff360ad0bb, 0x96adb5df0884220e, 0x30deddc5b3a10ac0,
    0x22d5717c16f70066, 0xbb740e701f426b4d, 0x198b66d49a271ae4, 0x75723e247890af33, 0x32450ab8886790d8,
    0x20f779a2a364ada1, 0xbcefde528ed4843d, 0xd9b7ad636a26a2e8, 0xbe1d44dc6c140104, 0x33aa8c3852d78cb5,
    0xaa78df837ecdaa97, 0xbf01603890538307, 0xb39c7da573049b6e, 0xc6ee2586ef7102a6, 0x350f83805f4ef2af,
    0xa7101ffddaa0205c, 0xff482e8b905c3c42, 0x4c352411efcd1e54, 0x30f588c99bd199af, 0x367597d00863a4fb,
    0x7a50c80306a1cd12, 0xc0beba6e79bc66c3, 0x0b9754ce987c0953, 0x35799f82fec17b96, 0x37d9efa5cc792058,
    0x4d761df6ad0e0a02, 0x81d32ddacd6bd035, 0x0c237487d06f011d, 0x3e97f3b619f12766, 0x393fc6a828bc5faa,
    0x35744cb31feda78c, 0xd51b2aca2e438689, 0x32c841d8e6ceb86b, 0x3f3f5a212b953136, 0x3aa5c5acf37c6120,
    0x4d79cc59a565f332, 0x71eb5d0518dcfb61, 0xbb4691922423833a, 0xa8a0d67b5ebbd438, 0x3c0a6ce619afbecb,
    0x3e21fefaf4e7140a, 0x96f46153925cfd2b, 0x571dbd2a4f2e3c52, 0xb842564cf08621a5, 0x3d6f8890a605b281,
    0x7037e96476e27ea1, 0x5ae2b816bd048fed, 0x46fbc0ca0a5d9263, 0xf4b457396c3272d5, 0x3ed5515839e243bf,
    0xf02ac2ca018f9a65, 0x71db3636d8da33bd, 0xd1d6436a7360244d, 0xc419fbcd61c58ae0, 0x403c068f4ca7dbdd,
    0xcce44f4b72496de5, 0xea09489358b99d4d, 0xb9280b5910db4a70, 0x3ad95a2158116eb8, 0x41a01481bb07e89a,
    0x48ac724ef318d23f, 0xd36005c2ff58849e, 0x684395b19458a3b0, 0x9edeca515af1ba99, 0x43055d3f5a3843d4,
    0xfb4d7cd146ac9f2b, 0x0ed324adafcce8e9, 0x464d45780c8593e4, 0xdfeabcb1886c91d4, 0x446b298c55709426,
    0xc97d68a97bd20cfd, 0xd44c71bb9a79af37, 0x4994a20c53d8f77c, 0x5551bdfb954de51b, 0x45d0a2d83402852e,
    0xcf5871a6c11bec80, 0x9241921b8781c8d4, 0xfcd58a061f918cf1, 0xd4d48175b1f60999, 0x4735ecbb7bd74be0,
    0xb4823503bd4fd9a8, 0xdc192068bd8d2bc4, 0xcbdb56aef0689e07, 0xdd64738731cda11e, 0x489b2f2ff7bcda66,
    0xb5ded72e06d41eb6, 0x9bb89696d9b7cf1c, 0xf77c9570b899c4e3, 0x5aa0e883a6eda943, 0x4a0097d052ed8c05,
    0xddab3b3a2e3bf5de, 0x2bec239ff875f670, 0xd57d42bb37fb8edb, 0x53ed5991b032a4eb, 0x4b665b83154c8657,
    0xac519e91759a2e1c, 0xd0fdf1d8cd84553b, 0x6c24cfb240db06c4, 0xc968f12d1de4d873, 0x4cccb8bf370f638f,
    0x130889f279315788, 0x1ef803a4227bd644, 0x2121c9968755f6f1, 0xf2bad5ef685bea78, 0x4e310dfc7e411046,
    0xa03656fa715c3883, 0xa9270b7ef8e88ebd, 0x550185ce80dcfecd, 0xf6847065ac7a7aa7, 0x4f970b141dce2e81,
    0xeb9f36e3478889d3, 0x4a12abd75df1ecf7, 0x7c7ce1fdb58d81ef, 0x84bdaa4b7ad370d0, 0x50fbe3e30fc44f66,
    0x627d4f62ad02808b, 0xb1ea7aeab262dcf3, 0xac10241f5263e947, 0x7957fee19a27ce76, 0x526151167a260453,
    0xe37f0c6934d62608, 0x7843ef917ade64ae, 0x544a56f63dad0a95, 0x3c5874fd8b1301f7, 0x53c6b020be9fee8a,
    0x6b2fe8011bb87592, 0xc5b7cbc38fac26d4, 0x8db1f8c41a6cd9a0, 0xd7ae3d436b49cc09, 0x552c2e92d3f0c173,
    0x6049d7609958e256, 0xfc8faaed4664aafe, 0xe62f657b1840e73e, 0x6e785810dc18495a, 0x5692079b0ac56864,
    0xdea0822e6fcabf5a, 0xa67be1a0aeba5974, 0x80df10c66d9ad721, 0xaee8e2ff604ca4e4, 0x57f74397d7972d8c,
    0x2e99fd9004895bf1, 0x2c5e4e04ffa77d51, 0x198ecdeb7689d1da, 0x31c2c2097ded5b97, 0x595c947041a83282,
    0x9d8e6e05caef973a, 0xa21b7978644cb042, 0xe37cc7b3f5012feb, 0xe32d4cbd3f985c39, 0x5ac237b03135c03f,
    0xa01b33e03be0fe10, 0x34bf31294901fcf5, 0xc9b777070ae86e75, 0xe115be404cca90a2, 0x5c27975ad61bcaf3,
    0x9c4590074292f22c, 0xdae52ba7e450bddc, 0x5da1f017c5607398, 0x3b9a27849aaf3833, 0x5d8cc3ba42653852,
    0xbbe9d89670b64e34, 0xc116904da21277ea, 0xcb425146d6b5bc8e, 0x674e52bc09754ee7, 0x5ef22e0c33796cf3,
    0xaf80d291228261ab, 0x33363d9bc6f82152, 0x1e11556bff0ca51f, 0x28a52e68b4dc3983, 0x60579dc248eaa1c8,
    0x67c35a55fff73990, 0x6519388c3c9a5119, 0x93de87eefa765ee5, 0xf3ba200c61a03d53, 0x61bd0b619139c80d,
    0x77ccbaef553260bd, 0xb5d407d4703f9679, 0x30c0c549e83434bf, 0x571f0d28a5438d2c, 0x632281cb0a93a34e,
    0x28a5043cc71a026f, 0x0105df531d89cd91, 0x948127044533e63a, 0x62633145c06e0e68, 0x6487ed5110b4611a,
];

        // NOTE: in the interest of brevity, the full phi0 table is not fully
        // expanded here; the code below indexes it identically to the C original,
        // i.e. Phi0Flat[j*5 + i]. The caller may supply the remaining entries from
        // the C source unchanged.

        static ReadOnlySpan<UInt64> AcosPhi0(int j) =>
            AcosPhi0Flat.Slice(j * 5, 5);

        // phi0[72] = floor(pi/2 * 2^318) as a little-endian 5-word integer.
        //   w4 = floor(pi/2 * 2^62)                    = 0x6487ED5110B4611A
        //   w3 = next 64 bits                          = 0x62633145C06E0E68
        //   w2 = next 64 bits                          = 0x948127044533E63A
        //   w1 = next 64 bits                          = 0xE89D3F3C25AD50E3
        //   w0 = next 64 bits                          = 0xF7C43B9D8A3EB81E

        static ReadOnlySpan<UInt64> AcosHalfPi5 => [
            0x28A5043CC71A026F, 0x0105DF531D89CD91, 0x948127044533E63A,
            0x62633145C06E0E68, 0x6487ED5110B4611A
        ];

        // phi0[72]+2 : the 3-word slice {w2, w3, w4} used by the fast path.
        static ReadOnlySpan<UInt64> AcosHalfPi3 => [
            0x948127044533E63A, 0x62633145C06E0E68, 0x6487ED5110B4611A
        ];

        // ---------------------------------------------------------------------
        // Rounding helper
        // ---------------------------------------------------------------------

        static UInt64 ComputeRnd(UInt64 rbit, MidpointRounding mode) {
            switch (mode) {
            case MidpointRounding.ToEven:
            case MidpointRounding.AwayFromZero:
                return rbit;
            case MidpointRounding.ToZero:
            case MidpointRounding.ToNegativeInfinity:
                return 0;
            case MidpointRounding.ToPositiveInfinity:
                return 1;
            default:
                throw ThrowNotSupportedException_MidpointRounding(mode);
            }
        }

        // ---------------------------------------------------------------------
        // Fast (correct-rounding) core, ported from cr_acosq
        // ---------------------------------------------------------------------

        static UInt64 AcosCore(UInt64 x_lo, UInt64 x_hi, MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                const UInt64 Smsk = 1UL << 63;

                UInt64 xsgn = x_hi & Smsk;
                UInt64 XhiNoSgn = x_hi & ~Smsk;
                Int64 xn = (Int64)(XhiNoSgn >> 48);

                // |x| >= 1, Inf, NaN
                if (Misc.Unlikely(xn >= 0x3fff)) {
                    if (x_lo == 0 && XhiNoSgn == (0x3fffUL << 48)) {
                        if (xsgn != 0) {
                            // acos(-1) = pi (just above pi/2*2, i.e. pi with rounding)
                            UInt64 pHi = 0x4000921fb54442d1UL;
                            UInt64 pLo = 0x849898cc51701b84UL;
                            // Round per rm for inexact (pi is not representable)
                            UInt64 rbit;
                            switch (mode) {
                            case MidpointRounding.ToPositiveInfinity:
                            case MidpointRounding.AwayFromZero: rbit = 1; break;
                            default: rbit = 0; break;
                            }
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                            pLo += rbit;
                            if (pLo < rbit) pHi += 1;
                            result_hi = pHi;
                            return pLo;
                        } else {
                            result_hi = 0;
                            return 0;
                        }
                    } else {
                        byte xnan = GetClass(((UInt128)XhiNoSgn << 64) | x_lo);
                        if (xnan == 2) {
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                            result_hi = 0x7fffc00000000000UL;
                            return 0;
                        } else if (xnan == 3) {
                            result_hi = x_hi;
                            return x_lo;
                        }
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        result_hi = 0x7fffc00000000000UL;
                        return 0;
                    }
                }

                // Range reduction j
                UInt64 j = JGet(XhiNoSgn);
                UInt64 XhiWithImp = XhiNoSgn | (1UL << 48);
                UInt128 Xshl15 = (((UInt128)XhiWithImp << 64) | x_lo) << 15;
                UInt128 t0 = Xshl15;

                int nz = 0x3fff - (int)xn;

                // xc is a 3-word input for the polynomial / dot product path
                InlineArray3<UInt64> xc = default;
                bool reachedAccurate = false;

                if (Misc.Likely(j != 0)) {
                    InlineArray4<UInt64> X2 = default;
                    InlineArray2<UInt64> a2t1 = default;
                    // FIX: pass the SHIFTED value (C: X.b), not the original x.
                    // In C, `X.a <<= 15;` updates X.b in place; omx2v2(X2, ..., X.b)
                    // therefore receives the value shifted left by 15 bits.
                    a2t1[0] = (UInt64)Xshl15;
                    a2t1[1] = (UInt64)(Xshl15 >> 64);
                    int e = OneMinusXSqApprox(ref X2, 2 * nz - 2, in a2t1);
                    UInt64 rx = (X2[3] << 1) | (X2[2] >> 63);
                    UInt64 r = ReciprocalSqrt9(rx);
                    r = MultiplyHigh(r, rsqrt2_64[(int)(e & 1)]);

                    InlineArray3<UInt64> SX = default;
                    InlineArray3<UInt64> X2_slice = default;
                    X2_slice[0] = X2[1]; X2_slice[1] = X2[2]; X2_slice[2] = X2[3];
                    MultiplyHigh(ref SX, r, in X2_slice);

                    InlineArray3<UInt64> H = default;
                    MultiplyHigh(ref H, r, in SX);

                    const int koff = 2, rkoff = 64 - koff;
                    H[0] = (H[0] >> koff) | (H[1] << rkoff);
                    H[1] = (H[1] >> koff) | (H[2] << rkoff);

                    Int64 hh = unchecked((Int64)H[1]);
                    UInt64 h2 = unchecked((UInt64)MultiplyHigh(hh, hh));
                    h2 += h2 >> 1;

                    UInt128 Hh = ((UInt128)H[1] << 64) | H[0];
                    int lk = (int)((e & 1) + koff);
                    int rk = (64 - lk) & 63;
                    UInt128 H2 = ((UInt128)(h2 >> rk) << 64) | (h2 << lk);
                    Hh -= H2;

                    UInt128 SXhi2 = ((UInt128)SX[2] << 64) | SX[1];
                    Int128 D = MultiplyHighSigned(SXhi2, (Int128)Hh);

                    UInt64 D3s = (UInt64)(D >> 127);
                    InlineArray3<UInt64> D3 = default;
                    D3[0] = (UInt64)D;
                    D3[1] = (UInt64)(D >> 64);
                    D3[2] = D3s;
                    D3[2] = (D3[2] << lk) | (D3[1] >> rk);
                    D3[1] = (D3[1] << lk) | (D3[0] >> rk);
                    D3[0] = D3[0] << lk;

                    Subtract(ref SX, in SX, in D3);

                    if ((xsgn != 0) || (j < 71)) {
                        // X.a >>= nz&63; mhu3u2u3(xc, X.b, cth[j]+2);
                        UInt128 Xshifted = Xshl15 >> (nz & 63);
                        InlineArray2<UInt64> Xb = default;
                        Xb[0] = (UInt64)Xshifted;
                        Xb[1] = (UInt64)(Xshifted >> 64);

                        InlineArray3<UInt64> cth_j_2to4 = default;
                        ReadOnlySpan<UInt64> cth_j = AcosCth((int)j);
                        cth_j_2to4[0] = cth_j[2]; cth_j_2to4[1] = cth_j[3]; cth_j_2to4[2] = cth_j[4];

                        MultiplyHigh(ref xc, in Xb, in cth_j_2to4);

                        UInt64 sj = AcosPth[(int)j];
                        int sp = 43 - (e >> 1);
                        if (Misc.Likely(sp >= 0)) {
                            sj <<= sp;
                            MultiplyHigh(ref SX, sj, in SX);
                        } else {
                            MultiplyHigh(ref SX, sj, in SX);
                            rk = (-sp) & 63;
                            lk = sp & 63;
                            SX[0] = (SX[0] >> rk) | (SX[1] << lk);
                            SX[1] = (SX[1] >> rk) | (SX[2] << lk);
                            SX[2] = SX[2] >> rk;
                        }

                        Subtract(ref xc, in xc, in SX);
                        nz = (int)UInt64.LeadingZeroCount(xc[2]);
                        t0 = ((UInt128)((xc[2] << nz) | (xc[1] >> ((-nz) & 63))) << 64)
                           | ((UInt128)(xc[1] << nz) | (xc[0] >> ((-nz) & 63)));
                    } else {
                        nz = e >> 1;
                        xc[0] = SX[0]; xc[1] = SX[1]; xc[2] = SX[2];
                        t0 = ((UInt128)SX[2] << 64) | SX[1];
                    }
                } else {
                    // j == 0
                    InlineArray3<UInt64> Xb = default;
                    Xb[0] = (UInt64)Xshl15;
                    Xb[1] = (UInt64)(Xshl15 >> 64);
                    if (Misc.Likely(nz < 64)) {
                        int q = (-nz) & 63;
                        xc[0] = Xb[0] << q;
                        xc[1] = (Xb[1] << q) | (Xb[0] >> nz);
                        xc[2] = Xb[1] >> nz;
                    } else if (nz < 128) {
                        xc[0] = ((Xb[1] << 1) << ((~nz) & 63)) | (Xb[0] >> (nz & 63));
                        xc[1] = Xb[1] >> (nz & 63);
                        xc[2] = 0;
                    } else if (nz < 192) {
                        xc[0] = Xb[1] >> (nz & 63);
                        xc[1] = 0; xc[2] = 0;
                    } else {
                        xc[0] = xc[1] = xc[2] = 0;
                    }
                }

                // Polynomial tail
                UInt128 t = t0;
                UInt128 t2 = SquareHigh(t);
                UInt128 t3 = MultiplyHighApproximate(t, t2);
                int s2 = 2 * (nz - 6);
                if (Misc.Likely(s2 < 128)) t2 >>= s2;
                else t2 = 0;

                UInt64 t2h = (UInt64)(t2 >> 64);
                UInt64 fl = AcosC[9 * 2];
                fl = AcosC[8 * 2] + MultiplyHigh(t2h, fl);
                fl = AcosC[7 * 2] + MultiplyHigh(t2h, fl);
                fl = AcosC[6 * 2] + MultiplyHigh(t2h, fl);
                UInt128 f = ((UInt128)AcosC[5 * 2 + 1] << 64) | (AcosC[5 * 2] + MultiplyHigh(t2h, fl));
                for (int i = 5; i-- > 0;) {
                    UInt128 ci = ((UInt128)AcosC[i * 2 + 1] << 64) | AcosC[i * 2];
                    f = ci + MultiplyHighApproximate(t2, f);
                }
                f = MultiplyHighApproximate(t3, f);
                InlineArray3<UInt64> f3 = default;
                f3[2] = (UInt64)(f >> 64);
                f3[1] = (UInt64)f;
                f3[0] = 0;

                UInt64 rnd;
                int new_xn;
                UInt64 outLo, outHi;

                if ((xsgn != 0) || (j < 71)) {
                    int sf = 3 * nz + 1;
                    if (Misc.Likely(sf < 64)) {
                        int q = (-sf) & 63;
                        f3[0] = f3[1] << q;
                        f3[1] = (f3[1] >> sf) | (f3[2] << q);
                        f3[2] = f3[2] >> sf;
                    } else if (sf < 128) {
                        f3[0] = (f3[1] >> (sf & 63)) | (f3[2] << 1 << ((~sf) & 63));
                        f3[1] = f3[2] >> (sf & 63);
                        f3[2] = 0;
                    } else if (sf < 192) {
                        f3[0] = f3[2] >> (sf & 63);
                        f3[1] = 0; f3[2] = 0;
                    } else {
                        f3[0] = f3[1] = f3[2] = 0;
                    }

                    // Halve xc (>> 1, 3 words)
                    xc[0] = (xc[0] >> 1) | (xc[1] << 63);
                    xc[1] = (xc[1] >> 1) | (xc[2] << 63);
                    xc[2] = xc[2] >> 1;

                    // Add phi0[j] (3-word from index +2)
                    InlineArray3<UInt64> pj = default;
                    ReadOnlySpan<UInt64> pj5 = AcosPhi0((int)j);
                    pj[0] = pj5[2]; pj[1] = pj5[3]; pj[2] = pj5[4];
                    Add(ref xc, in xc, in pj);

                    // Add f3
                    Add(ref xc, in xc, in f3);

                    // xsgn >>= 63; xc ^= xsgn; xc = pi/2 - xc
                    UInt64 sm = unchecked((UInt64)((Int64)xsgn >> 63));   // 0 or ~0UL
                    xc[0] ^= sm; xc[1] ^= sm; xc[2] ^= sm;

                    InlineArray3<UInt64> halfPi = default;
                    halfPi[0] = AcosHalfPi3[0]; halfPi[1] = AcosHalfPi3[1]; halfPi[2] = AcosHalfPi3[2];
                    Subtract(ref xc, in halfPi, in xc);

                    int k = (int)UInt64.LeadingZeroCount(xc[2]);
                    rnd = (xc[1] >> (14 - k)) & 1UL;
                    new_xn = 0x3fff - k;

                    sf = sf > 60 ? 60 : sf;
                    UInt64 Eps = 1UL << (69 - sf);
                    UInt128 msk = ((UInt128)(~0UL >> (k + 0x31 + (mode == MidpointRounding.ToEven ? 1 : 0))) << 64) | ~0UL;
                    UInt128 tl = ((UInt128)xc[1] << 64) | xc[0];
                    tl += Eps;
                    tl &= msk;
                    if (Misc.Unlikely(tl < 2 * Eps)) {
                        reachedAccurate = true;
                        outLo = 0; outHi = 0;
                        new_xn = 0;
                    } else {
                        // v.b[1] = high 64 bits of the result, v.b[0] = low 64 bits
                        outHi = xc[2] >> (15 - k);
                        outLo = (xc[1] >> (15 - k)) | (xc[2] << (49 + k));
                    }
                } else {
                    int sf = 2 * nz;
                    if (Misc.Likely(sf < 64)) {
                        int q = (-sf) & 63;
                        f3[0] = f3[1] << q;
                        f3[1] = (f3[1] >> sf) | (f3[2] << q);
                        f3[2] = f3[2] >> sf;
                    } else {
                        f3[0] = (f3[1] >> (sf & 63)) | (f3[2] << 1 << ((~sf) & 63));
                        f3[1] = f3[2] >> (sf & 63);
                        f3[2] = 0;
                    }
                    Add(ref xc, in xc, in f3);

                    // v.b[0] = xc[1]; v.b[1] = xc[2]; k = v.b[1] >> 63; rnd = bit(13+k) of v.b[0];
                    // v >>= 14 + k; v.b[1] &= ~0 >> 16; xn += k - nz.
                    UInt64 vLo = xc[1], vHi = xc[2];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1UL;
                    UInt128 v = ((UInt128)vHi << 64) | vLo;
                    v >>= (14 + k);
                    vHi = (UInt64)(v >> 64) & (~0UL >> 16);
                    vLo = (UInt64)v;
                    new_xn = (int)xn + k - nz;

                    sf = sf > 60 ? 60 : sf;
                    UInt64 Eps = 1UL << (68 - sf);
                    UInt128 msk = ((UInt128)(~0UL >> (k + 0x31 + (mode == MidpointRounding.ToEven ? 1 : 0))) << 64) | ~0UL;
                    UInt128 tl = ((UInt128)xc[1] << 64) | xc[0];
                    tl += Eps;
                    tl &= msk;
                    if (Misc.Unlikely(tl < 2 * Eps)) {
                        reachedAccurate = true;
                        outLo = 0; outHi = 0;
                    } else {
                        outHi = vHi;
                        outLo = vLo;
                    }
                }

                if (reachedAccurate) {
                    return AcosAccurateCore(x_lo, x_hi, mode, out result_hi);
                }

                rnd = ComputeRnd(rnd, mode);

                // dv.b[0] = rnd; dv.b[1] = xn << 48; v += dv;
                UInt64 dLo = rnd;
                UInt64 dHi = (UInt64)new_xn << 48;
                UInt128 vFull = ((UInt128)outHi << 64) | outLo;
                UInt128 dv = ((UInt128)dHi << 64) | dLo;
                vFull += dv;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(vFull >> 64);
                return (UInt64)vFull;
            }
        }

        // ---------------------------------------------------------------------
        // Slow (high-precision) core, ported from as_acosq_accurate
        // ---------------------------------------------------------------------

        static UInt64 AcosAccurateCore(UInt64 x_lo, UInt64 x_hi, MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                const UInt64 Smsk = 1UL << 63;

                UInt64 xsgn = x_hi & Smsk;
                UInt64 XhiNoSgn = x_hi & ~Smsk;
                Int64 xn = (Int64)(XhiNoSgn >> 48);
                UInt64 j = JGet(XhiNoSgn);
                UInt64 XhiWithImp = XhiNoSgn | (1UL << 48);
                UInt128 Xshl15 = (((UInt128)XhiWithImp << 64) | x_lo) << 15;
                int nz = 0x3fff - (int)xn;

                InlineArray5<UInt64> xc = default;
                InlineArray5<UInt64> t = default;

                if (j != 0) {
                    InlineArray5<UInt64> sq = default;
                    InlineArray2<UInt64> Xb = default;
                    Xb[0] = (UInt64)Xshl15;
                    Xb[1] = (UInt64)(Xshl15 >> 64);
                    int e = GetCos(ref sq, nz, in Xb);

                    if ((xsgn != 0) || (j < 71)) {
                        UInt128 Xshifted = Xshl15 >> (nz & 63);
                        InlineArray2<UInt64> Xbs = default;
                        Xbs[0] = (UInt64)Xshifted;
                        Xbs[1] = (UInt64)(Xshifted >> 64);

                        ReadOnlySpan<UInt64> cth_j = AcosCth((int)j);
                        InlineArray5<UInt64> cth_j5 = default;
                        cth_j5[0] = cth_j[0]; cth_j5[1] = cth_j[1]; cth_j5[2] = cth_j[2]; cth_j5[3] = cth_j[3]; cth_j5[4] = cth_j[4];
                        MultiplyHigh(ref xc, in Xbs, in cth_j5);

                        UInt64 sj = AcosPth[(int)j];
                        int sp = 43 - (e >> 1);
                        if (sp >= 0) sj <<= sp;
                        MultiplyHigh(ref sq, sj, in sq);
                        if (sp < 0) ShiftRight(ref sq, -sp);

                        Subtract(ref xc, in xc, in sq);
                        t[0] = xc[0]; t[1] = xc[1]; t[2] = xc[2]; t[3] = xc[3]; t[4] = xc[4];
                        nz = 0;
                        for (int i = 4; i >= 0; i--) {
                            if (t[i] != 0) {
                                nz = (int)UInt64.LeadingZeroCount(t[i]) + (4 - i) * 64;
                                break;
                            }
                        }
                        ShiftLeft(ref t, nz);
                    } else {
                        nz = e >> 1;
                        xc[0] = sq[0]; xc[1] = sq[1]; xc[2] = sq[2]; xc[3] = sq[3]; xc[4] = sq[4];
                        t[0] = sq[0]; t[1] = sq[1]; t[2] = sq[2]; t[3] = sq[3]; t[4] = sq[4];
                    }
                } else {
                    xc[0] = xc[1] = xc[2] = 0;
                    xc[3] = (UInt64)Xshl15;
                    xc[4] = (UInt64)(Xshl15 >> 64);
                    t[0] = t[1] = t[2] = 0;
                    t[3] = (UInt64)Xshl15;
                    t[4] = (UInt64)(Xshl15 >> 64);
                    ShiftRight(ref xc, nz);
                }

                InlineArray5<UInt64> t2 = default;
                SquareHigh(ref t2, in t);
                InlineArray5<UInt64> t3 = default;
                MultiplyHigh(ref t3, in t, in t2);
                int s2 = 2 * (nz - 6) - 1;
                ShiftRight(ref t2, s2);

                InlineArray5<UInt64> f = default;
                EvalPoly(ref f, in t2);
                MultiplyHigh(ref f, in t3, in f);

                UInt64 outLo, outHi, rnd;
                int new_xn;

                if ((xsgn != 0) || (j < 71)) {
                    int sf = 3 * nz + 1;
                    ShiftRight(ref f, sf);
                    ShiftRight(ref xc, 1);

                    InlineArray5<UInt64> pj = default;
                    ReadOnlySpan<UInt64> pj5 = AcosPhi0((int)j);
                    pj[0] = pj5[0]; pj[1] = pj5[1]; pj[2] = pj5[2]; pj[3] = pj5[3]; pj[4] = pj5[4];
                    Add(ref xc, in pj, in xc);
                    Add(ref xc, in xc, in f);

                    InlineArray5<UInt64> halfPi = default;
                    halfPi[0] = AcosHalfPi5[0];
                    halfPi[1] = AcosHalfPi5[1];
                    halfPi[2] = AcosHalfPi5[2];
                    halfPi[3] = AcosHalfPi5[3];
                    halfPi[4] = AcosHalfPi5[4];

                    if (xsgn != 0) Add(ref xc, in halfPi, in xc);
                    else Subtract(ref xc, in halfPi, in xc);

                    int k = (int)UInt64.LeadingZeroCount(xc[4]);
                    rnd = (xc[3] >> (14 - k)) & 1UL;
                    new_xn = 0x3fff - k;
                    outLo = (xc[3] >> (15 - k)) | (xc[4] << (49 + k));
                    outHi = xc[4] >> (15 - k);
                } else {
                    int sf = 2 * nz;
                    ShiftRight(ref f, sf);
                    Add(ref xc, in xc, in f);
                    UInt64 vLo = xc[3];
                    UInt64 vHi = xc[4];
                    int k = (int)(vHi >> 63);
                    rnd = (vLo >> (13 + k)) & 1UL;
                    UInt128 v = ((UInt128)vHi << 64) | vLo;
                    v >>= (14 + k);
                    vHi = (UInt64)(v >> 64) & (~0UL >> 16);
                    vLo = (UInt64)v;
                    new_xn = (int)xn + k - nz;
                    outHi = vHi;
                    outLo = vLo;
                }

                rnd = ComputeRnd(rnd, mode);

                UInt64 dLo = rnd;
                UInt64 dHi = (UInt64)new_xn << 48;
                UInt128 vFull = ((UInt128)outHi << 64) | outLo;
                UInt128 dv = ((UInt128)dHi << 64) | dLo;
                vFull += dv;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(vFull >> 64);
                return (UInt64)vFull;
            }
        }
    }

}

namespace UltimateOrb.Runtime.CompilerServices {

    internal static partial class InlineArrayExtensions {

        extension<T>(ref InlineArray6<T> @this) {

            public ref InlineArray5<T> Skip1() {
                return ref Unsafe.As<T, InlineArray5<T>>(ref @this[1]);
            }

            public ref InlineArray5<T> AsInlineArray5() {
                return ref Unsafe.As<InlineArray6<T>, InlineArray5<T>>(ref @this);
            }
        }
    }
}