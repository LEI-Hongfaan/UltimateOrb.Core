using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;
using Misc = UltimateOrb.Miscellaneous;

namespace UltimateOrb.Numerics {
    using Unsafe = System.Runtime.CompilerServices.Unsafe;

#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    public static partial class Binary128Arithmetic {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray3<UInt64> o, in InlineArray3<UInt64> b, UInt128 a) {
            unchecked {
                UInt64 aLo = (UInt64)a;
                UInt64 aHi = (UInt64)(a >> 64);
                InlineArray3<UInt64> r = default;
                r[0] = AddWithCarry(b[0], aLo, 0, out nuint c);
                r[1] = AddWithCarry(b[1], aHi, c, out c);
                r[2] = AddWithCarry(b[2], 0, c, out c);
                o = r;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray3<UInt64> o, in InlineArray3<UInt64> a, in InlineArray3<UInt64> b) {
            unchecked {
                InlineArray3<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray3<UInt64> o, in InlineArray3<UInt64> a, in InlineArray3<UInt64> b) {
            unchecked {
                InlineArray3<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray4<UInt64> o, in InlineArray4<UInt64> a, in InlineArray4<UInt64> b) {
            unchecked {
                InlineArray4<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out c);
                o_[3] = AddWithCarry(a[3], b[3], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray4<UInt64> o, in InlineArray4<UInt64> a, in InlineArray4<UInt64> b) {
            unchecked {
                InlineArray4<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out c);
                o_[3] = SubtractWithBorrow(a[3], b[3], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray5<UInt64> o, in InlineArray5<UInt64> a, in InlineArray5<UInt64> b) {
            unchecked {
                InlineArray5<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out c);
                o_[3] = AddWithCarry(a[3], b[3], c, out c);
                o_[4] = AddWithCarry(a[4], b[4], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray5<UInt64> o, in InlineArray5<UInt64> a, in InlineArray5<UInt64> b) {
            unchecked {
                InlineArray5<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out c);
                o_[3] = SubtractWithBorrow(a[3], b[3], c, out c);
                o_[4] = SubtractWithBorrow(a[4], b[4], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray6<UInt64> o, in InlineArray6<UInt64> a, in InlineArray6<UInt64> b) {
            unchecked {
                InlineArray6<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out c);
                o_[3] = AddWithCarry(a[3], b[3], c, out c);
                o_[4] = AddWithCarry(a[4], b[4], c, out c);
                o_[5] = AddWithCarry(a[5], b[5], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray6<UInt64> o, in InlineArray6<UInt64> a, in InlineArray6<UInt64> b) {
            unchecked {
                InlineArray6<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out c);
                o_[3] = SubtractWithBorrow(a[3], b[3], c, out c);
                o_[4] = SubtractWithBorrow(a[4], b[4], c, out c);
                o_[5] = SubtractWithBorrow(a[5], b[5], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray7<UInt64> o, in InlineArray7<UInt64> a, in InlineArray7<UInt64> b) {
            unchecked {
                InlineArray7<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out c);
                o_[3] = AddWithCarry(a[3], b[3], c, out c);
                o_[4] = AddWithCarry(a[4], b[4], c, out c);
                o_[5] = AddWithCarry(a[5], b[5], c, out c);
                o_[6] = AddWithCarry(a[6], b[6], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray7<UInt64> o, in InlineArray7<UInt64> a, in InlineArray7<UInt64> b) {
            unchecked {
                InlineArray7<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out c);
                o_[3] = SubtractWithBorrow(a[3], b[3], c, out c);
                o_[4] = SubtractWithBorrow(a[4], b[4], c, out c);
                o_[5] = SubtractWithBorrow(a[5], b[5], c, out c);
                o_[6] = SubtractWithBorrow(a[6], b[6], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void AddUnchecked(out InlineArray8<UInt64> o, in InlineArray8<UInt64> a, in InlineArray8<UInt64> b) {
            unchecked {
                InlineArray8<UInt64> o_ = default;
                o_[0] = AddWithCarry(a[0], b[0], 0, out var c);
                o_[1] = AddWithCarry(a[1], b[1], c, out c);
                o_[2] = AddWithCarry(a[2], b[2], c, out c);
                o_[3] = AddWithCarry(a[3], b[3], c, out c);
                o_[4] = AddWithCarry(a[4], b[4], c, out c);
                o_[5] = AddWithCarry(a[5], b[5], c, out c);
                o_[6] = AddWithCarry(a[6], b[6], c, out c);
                o_[7] = AddWithCarry(a[7], b[7], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void SubtractUnchecked(out InlineArray8<UInt64> o, in InlineArray8<UInt64> a, in InlineArray8<UInt64> b) {
            unchecked {
                InlineArray8<UInt64> o_ = default;
                o_[0] = SubtractWithBorrow(a[0], b[0], 0, out var c);
                o_[1] = SubtractWithBorrow(a[1], b[1], c, out c);
                o_[2] = SubtractWithBorrow(a[2], b[2], c, out c);
                o_[3] = SubtractWithBorrow(a[3], b[3], c, out c);
                o_[4] = SubtractWithBorrow(a[4], b[4], c, out c);
                o_[5] = SubtractWithBorrow(a[5], b[5], c, out c);
                o_[6] = SubtractWithBorrow(a[6], b[6], c, out c);
                o_[7] = SubtractWithBorrow(a[7], b[7], c, out _);
                o = o_;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void MultiplyHighUnsignedApproximate(out InlineArray2<UInt64> o, in InlineArray2<UInt64> a, in InlineArray2<UInt64> b) {
            unchecked {
                InlineArray2<UInt64> o_ = default;
                UInt64 t;

                UInt64 a1b0_hi = MultiplyHigh(a[1], b[0]);
                UInt64 o0 = a1b0_hi;

                UInt64 a0b1_hi = MultiplyHigh(a[0], b[1]);
                UInt64 a1b1_hi = Math.BigMul(a[1], b[1], out UInt64 a1b1_lo);

                t = AddWithCarry(a1b1_lo, a0b1_hi, 0, out var c0);
                o0 = AddWithCarry(o0, t, 0, out var c1);
                UInt64 o1 = AddWithCarry(a1b1_hi, 0, c0, out _);
                o1 = AddWithCarry(o1, 0, c1, out _);

                o_[0] = o0;
                o_[1] = o1;
                o = o_;
            }
        }

        // ----------------------------------------------------------------------
        // n = 3
        // ----------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void MultiplyHighUnsignedApproximate(out InlineArray3<UInt64> o, in InlineArray3<UInt64> a, in InlineArray3<UInt64> b) {
            unchecked {
                InlineArray3<UInt64> o_ = default;

                UInt64 a1b1_hi = Math.BigMul(a[1], b[1], out UInt64 a1b1_lo);
                UInt64 a2b0_hi = Math.BigMul(a[2], b[0], out UInt64 a2b0_lo);
                UInt64 a0b2_hi = Math.BigMul(a[0], b[2], out UInt64 a0b2_lo);
                UInt64 a2b1_hi = Math.BigMul(a[2], b[1], out UInt64 a2b1_lo);
                UInt64 a1b2_hi = Math.BigMul(a[1], b[2], out UInt64 a1b2_lo);
                UInt64 a2b2_hi = Math.BigMul(a[2], b[2], out UInt64 a2b2_lo);

                // a2b1 += a2b0>>64
                UInt64 new_lo = a2b1_lo + a2b0_hi;
                UInt64 carry = new_lo < a2b1_lo ? 1UL : 0UL;
                a2b1_hi += carry;
                a2b1_lo = new_lo;

                // a1b2 += a0b2>>64
                new_lo = a1b2_lo + a0b2_hi;
                carry = new_lo < a1b2_lo ? 1UL : 0UL;
                a1b2_hi += carry;
                a1b2_lo = new_lo;

                UInt64 o0 = a1b1_hi;
                UInt64 o1 = a2b2_lo;
                UInt64 o2 = a2b2_hi;

                o0 = AddWithCarry(o0, a2b1_lo, 0, out var c);
                o1 = AddWithCarry(o1, a2b1_hi, c, out c);
                o2 = AddWithCarry(o2, 0, c, out c);

                o_[0] = AddWithCarry(o0, a1b2_lo, 0, out c);
                o_[1] = AddWithCarry(o1, a1b2_hi, c, out c);
                o_[2] = AddWithCarry(o2, 0, c, out c);

                o = o_;
            }
        }

        // ----------------------------------------------------------------------
        // n = 4
        // ----------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void MultiplyHighUnsignedApproximate(out InlineArray4<UInt64> o, in InlineArray4<UInt64> a, in InlineArray4<UInt64> b) {
            unchecked {
                InlineArray4<UInt64> o_ = default;

                UInt64 a3b0_hi = Math.BigMul(a[3], b[0], out UInt64 a3b0_lo);
                UInt64 a3b2_hi = Math.BigMul(a[3], b[2], out UInt64 a3b2_lo);
                UInt64 a2b1_hi = Math.BigMul(a[2], b[1], out UInt64 a2b1_lo);
                UInt64 a3b1_hi = Math.BigMul(a[3], b[1], out UInt64 a3b1_lo);
                UInt64 a1b2_hi = Math.BigMul(a[1], b[2], out UInt64 a1b2_lo);
                UInt64 a2b2_hi = Math.BigMul(a[2], b[2], out UInt64 a2b2_lo);
                UInt64 a0b3_hi = Math.BigMul(a[0], b[3], out UInt64 a0b3_lo);
                UInt64 a1b3_hi = Math.BigMul(a[1], b[3], out UInt64 a1b3_lo);
                UInt64 a2b3_hi = Math.BigMul(a[2], b[3], out UInt64 a2b3_lo);
                UInt64 a3b3_hi = Math.BigMul(a[3], b[3], out UInt64 a3b3_lo);

                // a3b1 += a2b1>>64
                UInt64 new_lo = a3b1_lo + a2b1_hi;
                UInt64 carry = new_lo < a3b1_lo ? 1UL : 0UL;
                a3b1_hi += carry;
                a3b1_lo = new_lo;

                // a2b2 += a1b2>>64
                new_lo = a2b2_lo + a1b2_hi;
                carry = new_lo < a2b2_lo ? 1UL : 0UL;
                a2b2_hi += carry;
                a2b2_lo = new_lo;

                UInt64 o0 = AddWithCarry(a0b3_hi, a1b3_lo, 0, out var c);
                UInt64 o1 = AddWithCarry(a1b3_hi, a2b3_lo, c, out c);
                UInt64 o2 = AddWithCarry(a2b3_hi, a3b3_lo, c, out c);
                UInt64 o3 = AddWithCarry(a3b3_hi, 0, c, out c);

                o0 = AddWithCarry(o0, a3b0_hi, 0, out c);
                o1 = AddWithCarry(o1, a3b2_lo, c, out c);
                o2 = AddWithCarry(o2, a3b2_hi, c, out c);
                o3 = AddWithCarry(o3, 0, c, out c);

                o0 = AddWithCarry(o0, a3b1_lo, 0, out c);
                o1 = AddWithCarry(o1, a3b1_hi, c, out c);
                o2 = AddWithCarry(o2, 0, c, out c);
                o3 = AddWithCarry(o3, 0, c, out c);

                o_[0] = AddWithCarry(o0, a2b2_lo, 0, out c);
                o_[1] = AddWithCarry(o1, a2b2_hi, c, out c);
                o_[2] = AddWithCarry(o2, 0, c, out c);
                o_[3] = AddWithCarry(o3, 0, c, out _);

                o = o_;
            }
        }

        // ----------------------------------------------------------------------
        // n = 5
        // ----------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void MultiplyHighUnsignedApproximate(out InlineArray5<UInt64> o, in InlineArray5<UInt64> a, in InlineArray5<UInt64> b) {
            unchecked {
                InlineArray5<UInt64> o_ = default;
                UInt64 t;

                UInt64 a4b0_hi = MultiplyHigh(a[4], b[0]);
                UInt64 o0 = a4b0_hi;

                // diagonal i+j=5
                UInt64 a3b1_hi = MultiplyHigh(a[3], b[1]);
                UInt64 a4b1_hi = Math.BigMul(a[4], b[1], out UInt64 a4b1_lo);
                t = AddWithCarry(a4b1_lo, a3b1_hi, 0, out var c0);
                o0 = AddWithCarry(o0, t, 0, out var c1);
                UInt64 o1 = AddWithCarry(a4b1_hi, 0, c0, out c0);
                o1 = AddWithCarry(o1, 0, c1, out c1);

                // diagonal i+j=6
                UInt64 a2b2_hi = Math.BigMul(a[2], b[2], out _);
                UInt64 a3b2_hi = Math.BigMul(a[3], b[2], out UInt64 a3b2_lo);
                t = AddWithCarry(a3b2_lo, a2b2_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a4b2_hi = Math.BigMul(a[4], b[2], out UInt64 a4b2_lo);
                t = AddWithCarry(a4b2_lo, a3b2_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 o2 = AddWithCarry(a4b2_hi, 0, c0, out c0);
                o2 = AddWithCarry(o2, 0, c1, out c1);

                // diagonal i+j=7
                UInt64 a1b3_hi = Math.BigMul(a[1], b[3], out _);
                UInt64 a2b3_hi = Math.BigMul(a[2], b[3], out UInt64 a2b3_lo);
                t = AddWithCarry(a2b3_lo, a1b3_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a3b3_hi = Math.BigMul(a[3], b[3], out UInt64 a3b3_lo);
                t = AddWithCarry(a3b3_lo, a2b3_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 a4b3_hi = Math.BigMul(a[4], b[3], out UInt64 a4b3_lo);
                t = AddWithCarry(a4b3_lo, a3b3_hi, c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt64 o3 = AddWithCarry(a4b3_hi, 0, c0, out c0);
                o3 = AddWithCarry(o3, 0, c1, out c1);

                // diagonal i+j=8
                UInt64 a0b4_hi = Math.BigMul(a[0], b[4], out _);
                UInt64 a1b4_hi = Math.BigMul(a[1], b[4], out UInt64 a1b4_lo);
                t = AddWithCarry(a1b4_lo, a0b4_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a2b4_hi = Math.BigMul(a[2], b[4], out UInt64 a2b4_lo);
                t = AddWithCarry(a2b4_lo, a1b4_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 a3b4_hi = Math.BigMul(a[3], b[4], out UInt64 a3b4_lo);
                t = AddWithCarry(a3b4_lo, a2b4_hi, c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt64 a4b4_hi = Math.BigMul(a[4], b[4], out UInt64 a4b4_lo);
                t = AddWithCarry(a4b4_lo, a3b4_hi, c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                UInt64 o4 = AddWithCarry(a4b4_hi, 0, c0, out _);
                o4 = AddWithCarry(o4, 0, c1, out _);

                o_[0] = o0;
                o_[1] = o1;
                o_[2] = o2;
                o_[3] = o3;
                o_[4] = o4;
                o = o_;
            }
        }

        // ----------------------------------------------------------------------
        // n = 6
        // ----------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void MultiplyHighUnsignedApproximate(out InlineArray6<UInt64> o, in InlineArray6<UInt64> a, in InlineArray6<UInt64> b) {
            unchecked {
                InlineArray6<UInt64> o_ = default;
                UInt64 t;

                UInt64 a5b0_hi = MultiplyHigh(a[5], b[0]);
                UInt64 o0 = a5b0_hi;

                // i+j=6
                UInt64 a4b1_hi = MultiplyHigh(a[4], b[1]);
                UInt64 a5b1_hi = Math.BigMul(a[5], b[1], out UInt64 a5b1_lo);
                t = AddWithCarry(a5b1_lo, a4b1_hi, 0, out var c0);
                o0 = AddWithCarry(o0, t, 0, out var c1);
                UInt64 o1 = AddWithCarry(a5b1_hi, 0, c0, out c0);
                o1 = AddWithCarry(o1, 0, c1, out c1);

                // i+j=7
                UInt64 a3b2_hi = Math.BigMul(a[3], b[2], out _);
                UInt64 a4b2_hi = Math.BigMul(a[4], b[2], out UInt64 a4b2_lo);
                t = AddWithCarry(a4b2_lo, a3b2_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a5b2_hi = Math.BigMul(a[5], b[2], out UInt64 a5b2_lo);
                t = AddWithCarry(a5b2_lo, a4b2_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 o2 = AddWithCarry(a5b2_hi, 0, c0, out c0);
                o2 = AddWithCarry(o2, 0, c1, out c1);

                // i+j=8  (note: original names a1b3, a2b3, a3b3, a4b3 correspond to a[2..5]*b[3])
                UInt64 a2b3_hi = Math.BigMul(a[2], b[3], out _);   // original a1b3
                UInt64 a3b3_hi = Math.BigMul(a[3], b[3], out UInt64 a3b3_lo); // original a2b3
                t = AddWithCarry(a3b3_lo, a2b3_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a4b3_hi = Math.BigMul(a[4], b[3], out UInt64 a4b3_lo); // original a3b3
                t = AddWithCarry(a4b3_lo, a3b3_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 a5b3_hi = Math.BigMul(a[5], b[3], out UInt64 a5b3_lo); // original a4b3
                t = AddWithCarry(a5b3_lo, a4b3_hi, c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt64 o3 = AddWithCarry(a5b3_hi, 0, c0, out c0);
                o3 = AddWithCarry(o3, 0, c1, out c1);

                // i+j=9
                UInt64 a1b4_hi = Math.BigMul(a[1], b[4], out _);
                UInt64 a2b4_hi = Math.BigMul(a[2], b[4], out UInt64 a2b4_lo);
                t = AddWithCarry(a2b4_lo, a1b4_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a3b4_hi = Math.BigMul(a[3], b[4], out UInt64 a3b4_lo);
                t = AddWithCarry(a3b4_lo, a2b4_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 a4b4_hi = Math.BigMul(a[4], b[4], out UInt64 a4b4_lo);
                t = AddWithCarry(a4b4_lo, a3b4_hi, c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt64 a5b4_hi = Math.BigMul(a[5], b[4], out UInt64 a5b4_lo);
                t = AddWithCarry(a5b4_lo, a4b4_hi, c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                UInt64 o4 = AddWithCarry(a5b4_hi, 0, c0, out c0);
                o4 = AddWithCarry(o4, 0, c1, out c1);

                // i+j=10
                UInt64 a0b5_hi = Math.BigMul(a[0], b[5], out _);
                UInt64 a1b5_hi = Math.BigMul(a[1], b[5], out UInt64 a1b5_lo);
                t = AddWithCarry(a1b5_lo, a0b5_hi, 0, out c0);
                o0 = AddWithCarry(o0, t, 0, out c1);
                UInt64 a2b5_hi = Math.BigMul(a[2], b[5], out UInt64 a2b5_lo);
                t = AddWithCarry(a2b5_lo, a1b5_hi, c0, out c0);
                o1 = AddWithCarry(o1, t, c1, out c1);
                UInt64 a3b5_hi = Math.BigMul(a[3], b[5], out UInt64 a3b5_lo);
                t = AddWithCarry(a3b5_lo, a2b5_hi, c0, out c0);
                o2 = AddWithCarry(o2, t, c1, out c1);
                UInt64 a4b5_hi = Math.BigMul(a[4], b[5], out UInt64 a4b5_lo);
                t = AddWithCarry(a4b5_lo, a3b5_hi, c0, out c0);
                o3 = AddWithCarry(o3, t, c1, out c1);
                UInt64 a5b5_hi = Math.BigMul(a[5], b[5], out UInt64 a5b5_lo);
                t = AddWithCarry(a5b5_lo, a4b5_hi, c0, out c0);
                o4 = AddWithCarry(o4, t, c1, out c1);
                UInt64 o5 = AddWithCarry(a5b5_hi, 0, c0, out _);
                o5 = AddWithCarry(o5, 0, c1, out _);

                o_[0] = o0;
                o_[1] = o1;
                o_[2] = o2;
                o_[3] = o3;
                o_[4] = o4;
                o_[5] = o5;
                o = o_;
            }
        }

        // mu5u2u3 :  5 = 2 + 3.  first = b[0..1], second = a[0..2].
        internal static void BigMulUnsigned(
            out InlineArray5<UInt64> result,
            in InlineArray2<UInt64> first,
            in InlineArray3<UInt64> second) {
            unchecked {
                UInt64 a0 = second[0], a1 = second[1], a2 = second[2];
                UInt64 b0 = first[0], b1 = first[1];

                InlineArray5<UInt64> o = default;
                UInt64 t0, t1, tt;
                nuint c, c0, c1;

                // ── row 0 : b[0] * a[] ──────────────────────────────────────
                UInt128 T = Math.BigMul(b0, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[0] = t0;
                tt = t1;

                T = Math.BigMul(b0, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(tt, t0, 0, out c);
                tt = t1;

                T = Math.BigMul(b0, a2);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[2] = AddWithCarry(tt, t0, c, out c);
                o[3] = AddWithCarry(0, t1, c, out c);

                // ── row 1 : b[1] * a[] ──────────────────────────────────────
                T = Math.BigMul(b1, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(o[1], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b1, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[2] = AddWithCarry(o[2], t0, c0, out c0);
                tt = t1;

                T = Math.BigMul(b1, a2);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, c1, out c1);
                o[3] = AddWithCarry(o[3], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[4] = AddWithCarry(0, t1, c0, out c0);

                result = o;
            }
        }

        // mu7u5u2 :  7 = 5 + 2.  first = b[0..4], second = a[0..1].
        internal static void BigMulUnsigned(
            out InlineArray7<UInt64> result,
            in InlineArray5<UInt64> first,
            in InlineArray2<UInt64> second) {
            unchecked {
                UInt64 a0 = second[0], a1 = second[1];
                UInt64 b0 = first[0], b1 = first[1], b2 = first[2], b3 = first[3], b4 = first[4];

                InlineArray7<UInt64> o = default;
                UInt64 t0, t1, tt;
                nuint c, c0, c1;

                // ── row 0 : b[0] ────────────────────────────────────────────
                UInt128 T = Math.BigMul(b0, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[0] = t0;
                tt = t1;

                T = Math.BigMul(b0, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(tt, t0, 0, out c);
                o[2] = AddWithCarry(0, t1, c, out c);

                // ── row 1 : b[1] ────────────────────────────────────────────
                T = Math.BigMul(b1, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(o[1], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b1, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[2] = AddWithCarry(o[2], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[3] = AddWithCarry(0, t1, c0, out c0);

                // ── row 2 : b[2] ────────────────────────────────────────────
                T = Math.BigMul(b2, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[2] = AddWithCarry(o[2], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b2, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[3] = AddWithCarry(o[3], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[4] = AddWithCarry(0, t1, c0, out c0);

                // ── row 3 : b[3] ────────────────────────────────────────────
                T = Math.BigMul(b3, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[3] = AddWithCarry(o[3], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b3, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[4] = AddWithCarry(o[4], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[5] = AddWithCarry(0, t1, c0, out c0);

                // ── row 4 : b[4] ────────────────────────────────────────────
                T = Math.BigMul(b4, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[4] = AddWithCarry(o[4], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b4, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[5] = AddWithCarry(o[5], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[6] = AddWithCarry(0, t1, c0, out c0);

                result = o;
            }
        }

        // mu8u6u2 :  8 = 6 + 2.  first = b[0..5], second = a[0..1].
        internal static void MultiplyUnsigned(
            out InlineArray8<UInt64> result,
            in InlineArray6<UInt64> first,
            in InlineArray2<UInt64> second) {
            unchecked {
                UInt64 a0 = second[0], a1 = second[1];
                UInt64 b0 = first[0], b1 = first[1], b2 = first[2],
                       b3 = first[3], b4 = first[4], b5 = first[5];

                InlineArray8<UInt64> o = default;
                UInt64 t0, t1, tt;
                nuint c, c0, c1;

                // ── row 0 : b[0] ────────────────────────────────────────────
                UInt128 T = Math.BigMul(b0, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[0] = t0;
                tt = t1;

                T = Math.BigMul(b0, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(tt, t0, 0, out c);
                o[2] = AddWithCarry(0, t1, c, out c);

                // ── row 1 : b[1] ────────────────────────────────────────────
                T = Math.BigMul(b1, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[1] = AddWithCarry(o[1], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b1, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[2] = AddWithCarry(o[2], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[3] = AddWithCarry(0, t1, c0, out c0);

                // ── row 2 : b[2] ────────────────────────────────────────────
                T = Math.BigMul(b2, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[2] = AddWithCarry(o[2], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b2, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[3] = AddWithCarry(o[3], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[4] = AddWithCarry(0, t1, c0, out c0);

                // ── row 3 : b[3] ────────────────────────────────────────────
                T = Math.BigMul(b3, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[3] = AddWithCarry(o[3], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b3, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[4] = AddWithCarry(o[4], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[5] = AddWithCarry(0, t1, c0, out c0);

                // ── row 4 : b[4] ────────────────────────────────────────────
                T = Math.BigMul(b4, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[4] = AddWithCarry(o[4], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b4, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[5] = AddWithCarry(o[5], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[6] = AddWithCarry(0, t1, c0, out c0);

                // ── row 5 : b[5] ────────────────────────────────────────────
                T = Math.BigMul(b5, a0);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                o[5] = AddWithCarry(o[5], t0, 0, out c0);
                tt = t1;

                T = Math.BigMul(b5, a1);
                t1 = (UInt64)(T >> 64); t0 = (UInt64)T;
                t0 = AddWithCarry(tt, t0, 0, out c1);
                o[6] = AddWithCarry(o[6], t0, c0, out c0);
                t1 = AddWithCarry(0, t1, c1, out c1);
                o[7] = AddWithCarry(0, t1, c0, out c0);

                result = o;
            }
        }
    }


    public static partial class Binary128Arithmetic {

        internal static void ShiftLeftUnsignedFull(Span<UInt64> a, int k) {
            unchecked {
                if (Misc.Unlikely(k < 0)) {           // __builtin_expect(k<0, 0)
                    ShiftRightUnsignedFull(a, -k);
                    return;
                }
                int n = a.Length;
                if (n == 0) return;

                int off = k >> 6;
                int dst = n - 1;
                int src = dst - off;

                if (Misc.Likely(src >= 0)) {          // __builtin_expect(src >= a, 1)
                                                      // q = (~k)&63 = 63 - (k&63), so  nt >> 1 >> q  ==  nt >> (64 - s).
                    int s = k & 63, q = ~k & 63;
                    UInt64 c = a[src--];
                    while (Misc.Likely(src >= 0)) {   // __builtin_expect(src >= a, 1)
                        UInt64 nt = a[src--];
                        a[dst--] = (c << s) | (nt >> 1 >> q);
                        c = nt;
                    }
                    a[dst--] = c << s;
                }
                while (Misc.Unlikely(dst >= 0))       // __builtin_expect(dst >= a, 0)
                    a[dst--] = 0;
            }
        }

        internal static void ShiftRightUnsignedFull(Span<UInt64> a, int k) {
            unchecked {
                if (Misc.Unlikely(k < 0)) {           // __builtin_expect(k<0, 0)
                    ShiftLeftUnsignedFull(a, -k);
                    return;
                }
                int n = a.Length;
                if (n == 0) return;

                int off = k >> 6;
                int src = off;
                int dst = 0;
                int aend = n;

                if (Misc.Likely(src < aend)) {        // __builtin_expect(src < aend, 1)
                    UInt64 c = a[src++];
                    int s = k & 63;
                    if (Misc.Likely(s != 0)) {        // __builtin_expect(s, 1)
                        int q = -k & 63;              // 64 - s
                        while (Misc.Likely(src < aend)) {
                            UInt64 nt = a[src++];
                            a[dst++] = (c >> s) | (nt << q);
                            c = nt;
                        }
                        a[dst++] = c >> s;
                    } else {
                        while (Misc.Likely(src < aend)) {
                            UInt64 nt = a[src++];
                            a[dst++] = c;
                            c = nt;
                        }
                        a[dst++] = c;
                    }
                }
                while (Misc.Unlikely(dst < aend))     // __builtin_expect(dst < aend, 0)
                    a[dst++] = 0;
            }
        }

        internal static void ShiftRightSignedFull(Span<UInt64> a, int k) {
            unchecked {
                if (Misc.Unlikely(k < 0)) {           // __builtin_expect(k<0, 0)
                    ShiftLeftUnsignedFull(a, -k);
                    return;
                }
                int n = a.Length;
                if (n == 0) return;

                int off = k >> 6;
                int src = off;
                int dst = 0;
                int aend = n;

                Int64 c;
                if (Misc.Likely(src < aend)) {        // __builtin_expect(src < aend, 1)
                    int s = k & 63;
                    int q = ~k & 63;                  // 63 - s, so  nt << 1 << q == nt << (64 - s)
                    c = (Int64)a[src++];
                    while (Misc.Likely(src < aend)) {
                        UInt64 nt = a[src++];
                        // Note the deliberate asymmetry in the C source:
                        //   loop body:  (u64)c >> s   — logical shift of the bit pattern
                        //   final write: c >> s       — arithmetic shift (sign-extends top s bits)
                        a[dst++] = ((UInt64)c >> s) | (nt << 1 << q);
                        c = (Int64)nt;
                    }
                    a[dst++] = (UInt64)(c >> s);      // arithmetic
                } else {
                    c = (Int64)a[n - 1];
                }

                c >>= 63;                             // 0 or -1, driven by sign of the top word
                UInt64 cf = (UInt64)c;
                while (Misc.Unlikely(dst < aend))     // __builtin_expect(dst < aend, 0)
                    a[dst++] = cf;
            }
        }
    }

    public static partial class Binary128Arithmetic {

        internal static void SquareSigned(out InlineArray6<UInt64> result, in InlineArray3<UInt64> value) {
            unchecked {
                UInt64 a0 = value[0], a1 = value[1], a2 = value[2];

                UInt128 a2a2 = Math.BigMul(a2, a2);
                UInt128 a2a1 = Math.BigMul(a2, a1);
                UInt128 a2a0 = Math.BigMul(a2, a0);
                UInt128 a1a1 = Math.BigMul(a1, a1);
                UInt128 a1a0 = Math.BigMul(a1, a0);
                UInt128 a0a0 = Math.BigMul(a0, a0);

                // 2·a, sign-masked by the top bit of a[2]
                UInt64 t2 = (a2 << 1) | (a1 >> 63);
                UInt64 t1 = (a1 << 1) | (a0 >> 63);
                UInt64 t0 = a0 << 1;
                Int64 m = (Int64)a2 >> 63;      // 0 or -1
                UInt64 mu = (UInt64)m;
                t2 &= mu;
                t1 &= mu;
                t0 &= mu;

                InlineArray6<UInt64> o = default;
                o[0] = (UInt64)a0a0;
                o[1] = (UInt64)(a0a0 >> 64);
                o[2] = (UInt64)a1a1;
                o[3] = (UInt64)(a1a1 >> 64);
                o[4] = (UInt64)a2a2;
                o[5] = (UInt64)(a2a2 >> 64);

                nuint c0;
                UInt64 b0 = (UInt64)a1a0;
                UInt64 b1 = AddWithCarry((UInt64)a2a0, (UInt64)(a1a0 >> 64), 0, out c0);
                UInt64 b2 = AddWithCarry((UInt64)a2a1, (UInt64)(a2a0 >> 64), c0, out c0);
                UInt64 b3 = AddWithCarry(0, (UInt64)(a2a1 >> 64), c0, out c0);

                o[1] = AddWithCarry(o[1], b0, 0, out c0);
                o[2] = AddWithCarry(o[2], b1, c0, out c0);
                o[3] = AddWithCarry(o[3], b2, c0, out c0);
                o[4] = AddWithCarry(o[4], b3, c0, out c0);
                o[5] = AddWithCarry(o[5], 0, c0, out c0);

                o[1] = AddWithCarry(o[1], b0, 0, out c0);
                o[2] = AddWithCarry(o[2], b1, c0, out c0);
                o[3] = AddWithCarry(o[3], b2, c0, out c0);
                o[4] = AddWithCarry(o[4], b3, c0, out c0);
                o[5] = AddWithCarry(o[5], 0, c0, out c0);

                o[3] = SubtractWithBorrow(o[3], t0, c0, out c0);
                o[4] = SubtractWithBorrow(o[4], t1, c0, out c0);
                o[5] = SubtractWithBorrow(o[5], t2, c0, out c0);

                result = o;
            }
        }

        internal static void SquareHighUnsignedApproximate(out InlineArray6<UInt64> result, in InlineArray6<UInt64> value) {
            unchecked {
                UInt64 a0 = value[0], a1 = value[1], a2 = value[2];
                UInt64 a3 = value[3], a4 = value[4], a5 = value[5];

                InlineArray6<UInt64> o = default;
                nuint c0;

                // ── accumulate off-diagonal a[i]*a[j] (i > j), high halves first ──

                UInt128 a3a2 = Math.BigMul(a3, a2);
                UInt128 a4a1 = Math.BigMul(a4, a1);
                o[0] = AddWithCarry((UInt64)(a3a2 >> 64), (UInt64)(a4a1 >> 64), 0, out c0);
                o[1] = AddWithCarry(0, 0, c0, out c0);

                UInt128 a5a0 = Math.BigMul(a5, a0);
                o[0] = AddWithCarry(o[0], (UInt64)(a5a0 >> 64), 0, out c0);
                o[1] = AddWithCarry(o[1], 0, c0, out c0);

                UInt128 a4a2 = Math.BigMul(a4, a2);
                o[0] = AddWithCarry(o[0], (UInt64)a4a2, 0, out c0);
                o[1] = AddWithCarry(o[1], (UInt64)(a4a2 >> 64), c0, out c0);
                o[2] = AddWithCarry(0, 0, c0, out c0);

                UInt128 a5a1 = Math.BigMul(a5, a1);
                o[0] = AddWithCarry(o[0], (UInt64)a5a1, 0, out c0);
                o[1] = AddWithCarry(o[1], (UInt64)(a5a1 >> 64), c0, out c0);
                o[2] = AddWithCarry(o[2], 0, c0, out c0);

                UInt128 a4a3 = Math.BigMul(a4, a3);
                o[1] = AddWithCarry(o[1], (UInt64)a4a3, 0, out c0);
                o[2] = AddWithCarry(o[2], (UInt64)(a4a3 >> 64), c0, out c0);
                o[3] = AddWithCarry(0, 0, c0, out c0);

                UInt128 a5a2 = Math.BigMul(a5, a2);
                o[1] = AddWithCarry(o[1], (UInt64)a5a2, 0, out c0);
                o[2] = AddWithCarry(o[2], (UInt64)(a5a2 >> 64), c0, out c0);
                o[3] = AddWithCarry(o[3], 0, c0, out c0);

                UInt128 a5a3 = Math.BigMul(a5, a3);
                o[2] = AddWithCarry(o[2], (UInt64)a5a3, 0, out c0);
                o[3] = AddWithCarry(o[3], (UInt64)(a5a3 >> 64), c0, out c0);
                o[4] = AddWithCarry(0, 0, c0, out c0);

                UInt128 a5a4 = Math.BigMul(a5, a4);
                o[3] = AddWithCarry(o[3], (UInt64)a5a4, 0, out c0);
                o[4] = AddWithCarry(o[4], (UInt64)(a5a4 >> 64), c0, out c0);
                o[5] = AddWithCarry(0, 0, c0, out c0);

                // ── double the accumulated sum (carry ripples low → high) ─────
                o[0] = AddWithCarry(o[0], o[0], 0, out c0);
                o[1] = AddWithCarry(o[1], o[1], c0, out c0);
                o[2] = AddWithCarry(o[2], o[2], c0, out c0);
                o[3] = AddWithCarry(o[3], o[3], c0, out c0);
                o[4] = AddWithCarry(o[4], o[4], c0, out c0);
                o[5] = AddWithCarry(o[5], o[5], c0, out c0);

                // ── add the diagonal squares ─────────────────────────────────
                UInt128 a3a3 = Math.BigMul(a3, a3);
                o[0] = AddWithCarry(o[0], (UInt64)a3a3, 0, out c0);
                o[1] = AddWithCarry(o[1], (UInt64)(a3a3 >> 64), c0, out c0);

                UInt128 a4a4 = Math.BigMul(a4, a4);
                o[2] = AddWithCarry(o[2], (UInt64)a4a4, c0, out c0);
                o[3] = AddWithCarry(o[3], (UInt64)(a4a4 >> 64), c0, out c0);

                UInt128 a5a5 = Math.BigMul(a5, a5);
                o[4] = AddWithCarry(o[4], (UInt64)a5a5, c0, out c0);
                o[5] = AddWithCarry(o[5], (UInt64)(a5a5 >> 64), c0, out c0);

                result = o;
            }
        }
    }

    partial class Binary128Arithmetic {

        internal static ReadOnlySpan<byte> Rcp => [
            0, 248, 240, 232, 224, 217, 210, 202, 195, 188, 181, 174, 168, 161,
            155, 148, 142, 135, 129, 123, 117, 111, 105, 100, 94, 88, 83, 77,
            72, 66, 61, 56, 51, 46, 41, 36, 31, 26, 21, 16, 12, 7, 3, 254, 250,
            245, 241, 236, 232, 228, 224, 220, 216, 212, 208, 204, 200, 196,
            192, 188, 185, 181, 177, 174, 170, 167, 163, 160, 156, 153, 149,
            146, 143, 140, 136, 133, 130, 127, 124, 121, 118, 115, 112, 109,
            106, 103, 100, 97, 94, 92, 89, 86, 83, 81, 78, 75, 73, 70, 67, 65,
            62, 60, 57, 55, 52, 50, 48, 45, 43, 41, 38, 36, 34, 31, 29, 27, 25,
            22, 20, 18, 16, 14, 12, 10, 8, 6, 4, 2,
        ];

        internal static ReadOnlySpan<byte> Ind => [
            0, 0, 1, 1, 2, 3, 3, 4, 5, 5, 6, 6, 7, 8, 8, 9, 10, 10, 11, 11,
            12, 13, 13, 14, 15, 15, 16, 16, 17, 18, 18, 19, 19, 20, 21, 21,
            22, 22, 23, 23, 24, 25, 25, 26, 26, 27, 27, 28, 29, 29, 30, 30,
            31, 31, 32, 32, 33, 33, 34, 34, 35, 36, 36, 37, 37, 38, 38, 39,
            39, 40, 40, 41, 41, 41, 42, 42, 43, 43, 44, 44, 45, 45, 46, 46,
            47, 47, 47, 48, 48, 49, 49, 50, 50, 50, 51, 51, 52, 52, 52, 53,
            53, 54, 54, 54, 55, 55, 56, 56, 56, 57, 57, 57, 58, 58, 58, 59,
            59, 60, 60, 60, 61, 61, 61, 62, 62, 62, 63, 63, 63,
        ];

        static ReadOnlySpan<Byte> Indh => [
    0, 0, 1, 1, 2, 2, 3, 4, 5, 5, 5, 6, 6, 7, 8, 9, 10, 10, 10, 11, 11,
  11, 12, 12, 13, 14, 14, 15, 16, 16, 17, 18, 19, 20, 20, 20, 21, 21,
  21, 22, 22, 23, 23, 23, 24, 24, 25, 25, 26, 26, 27, 27, 28, 29, 29,
  30, 31, 31, 32, 33, 34, 34, 35, 36, 37, 38, 38, 38, 38, 39, 39, 39,
  39, 40, 40, 40, 41, 41, 41, 41, 42, 42, 42, 43, 43, 43, 44, 44, 44,
  45, 45, 46, 46, 46, 47, 47, 47, 48, 48, 49, 49, 49, 50, 50, 51, 51,
  52, 52, 53, 53, 54, 54, 54, 55, 55, 56, 57, 57, 58, 58, 59, 59, 60,
  60, 61, 62, 62, 63
];

        // a 15 bit approximation of tan(i*pi/4/64)
        internal static ReadOnlySpan<UInt16> Tn => [
            0, 0x192, 0x324, 0x4b7, 0x64a, 0x7dd, 0x971, 0xb06, 0xc9b, 0xe32,
            0xfca, 0x1162, 0x12fd, 0x1498, 0x1636, 0x17d5, 0x1976, 0x1b19,
            0x1cbe, 0x1e66, 0x2010, 0x21bd, 0x236c, 0x251f, 0x26d4, 0x288d,
            0x2a49, 0x2c09, 0x2dcd, 0x2f94, 0x3160, 0x3330, 0x3505, 0x36de,
            0x38bd, 0x3aa1, 0x3c8a, 0x3e79, 0x406e, 0x4269, 0x446b, 0x4673,
            0x4883, 0x4a9a, 0x4cb8, 0x4edf, 0x510e, 0x5346, 0x5587, 0x57d1,
            0x5a26, 0x5c85, 0x5eee, 0x6163, 0x63e4, 0x6672, 0x690c, 0x6bb4,
            0x6e6a, 0x712f, 0x7403, 0x76e8, 0x79de, 0x7ce5, 0x8000,
        ];

        // phi0[i] = atan(tn[i]);
        internal static ReadOnlySpan<UInt64> Phi0_flat => [
            0, 0, 0, 0, 0, 0,
            0x02c1e09886f8fc65, 0x12befe0e684801ab, 0x5b500d3aa545cf10, 0xc29c938f5e9594c8, 0xe19fb2a3207f6303, 0x00c8fd6b34169a4d,
            0x4cb54df37491326d, 0xbcc2d6b5024de6ba, 0x0140a83479a7d09b, 0xb47181e24638fe92, 0x94e685c329b2d773, 0x0191eb5b0f2af2cb,
            0x96290951ce6369c6, 0xe6f739fc68793759, 0x23222e343d6f817c, 0xb5a31899b4d03cbb, 0xe18dc8765eb3b239, 0x025b3a2f01f4cf05,
            0x26a8b16ac189860c, 0xec201bca4b528ce4, 0xd8b160e7c9c348ed, 0xf7384a1bcde479ed, 0x3665e6a2207124bc, 0x03245a68934296f4,
            0x2b2747a2b7b246bd, 0x0eb316ea6af2c481, 0x8994c96123649fad, 0x40c4923addf07637, 0x7eb0104c3bbaeebb, 0x03ed3c99ec5f4fd6,
            0xc1aa40ed9cf81137, 0x7b9341c26981ba2c, 0xa3f94febb560fec3, 0x038b8f349e0b9941, 0x60a39999b30013e3, 0x04b650c091b47191,
            0x23fb4c9fabd5a3f5, 0x8e9c1d3e3a2a9e80, 0x4f7d972251777188, 0x7f688f435290217b, 0xa760b14ee5c8185b, 0x057f86ef8cce05d9,
            0xa345983253154b9f, 0x742e9e3f6bbf6db1, 0xbe3b949d708e4ab7, 0xb8f721b926dd7968, 0xbc9cc590f04c39b1, 0x0648506548567e1c,
            0x4270e6ec850ed3d4, 0xbd6add47dd4704ae, 0xd7f4f0121bbb10d8, 0x6420acc6d7aa717a, 0x5181cc2b7f67466d, 0x07119afb1db64d3b,
            0x3c0ddcb933c0132f, 0x29ac0726e281c9c4, 0x8af040655ae13a00, 0xe5e69a461efee36f, 0x98430538072960bc, 0x07dad79d66d396e1,
            0xc3d6eaaa86b1e257, 0x478e9831c107b273, 0x91ece4ca7c8c9a92, 0x7e4d9cd1133d108e, 0x6b2499055ef359f2, 0x08a378a00557419c,
            0xe452b1a89a5919ea, 0x3cfb3638e459c56f, 0x82a58ff9c80f179f, 0x98506bfc8617b00d, 0xeef4781de96151b3, 0x096ce71f84443705,
            0x7022ba3fd2c3a3ec, 0x1e810ce3f03023fb, 0x080ea2ce1b951a1b, 0xdc28825e93081751, 0xfa554a296e7bbd3e, 0x0a359a37ad347506,
            0x5082480b4642639f, 0xf6d143858afffc0b, 0xe83a1d095ceb9d93, 0xf1c6b6f3f95d034c, 0xbe30339b35470763, 0x0afef860ced6731a,
            0x06a1328f3ed37ce1, 0xa8fbddade1ab0e4e, 0xace3bbf391d4780d, 0xc419de3fafb1443c, 0x71d3f327dc1666e5, 0x0bc7f750a98aaf11,
            0x2cdef1893d2be331, 0x1644a02bce301472, 0xe8835bd11f8a7463, 0x7f78b6932fe4c9a9, 0x573436e5491c3c3b, 0x0c910288a5912f73,
            0x820f7fdd710ebd56, 0xba60aab851afd713, 0x4453d264039e6104, 0xd07a8bd375db11fb, 0x1a70ad3ce98dd37d, 0x0d5a08c366f7ad26,
            0x23c5850e662df2a8, 0x48eb29d7959af0d9, 0x1db3e8ca092b26ff, 0x1576eaf97ffde937, 0x2b63c532aae1baef, 0x0e22f8c260ccbeb2,
            0x5a1cbdcc77ccbebd, 0x42d39881973ab439, 0xea1ab61b1291bf62, 0x61d834c4d89794de, 0x3ea9630254b006a7, 0x0eec3a81159ad32d,
            0x354baa05f6e10d1c, 0x2ff82bd8a784b660, 0x2161dc75b94d9dad, 0xf2cebf4c84ed043f, 0xf9f105fb8919b427, 0x0fb5424b656558f6,
            0xb89dde65241d6464, 0xbb4ad1f58d638111, 0xfa29639aeb3ba102, 0x9a4c65cd7e32af7d, 0x7ed90db179691680, 0x107e76c03c9dea1c,
            0xc92938551b12cad8, 0xec7dcfe04738fefb, 0x2937dd301a0b4ca0, 0x01f63aedeb6b1188, 0x3e1132674a1a6ece, 0x11474db47224d179,
            0xb8cd0fc44f74d861, 0x2e822666fd0d636e, 0x8e1d4cec346d99be, 0xc6330a710a48d5da, 0x5f1a2b16680c1a30, 0x1210a27e5ff90c02,
            0x5e1c793da47d6569, 0xfff7d2b2957a644f, 0xeeebd0bcf6fee6c2, 0xeb7eb29db9b819fc, 0xe00180e9012b321a, 0x12d974fae8ccb2fc,
            0x8a7aa5432dddb0a8, 0xf6f9c2e4f0d2b4b7, 0x2c27002eeef60e9f, 0xe93934b45f17a029, 0x10ab1e3995f97abe, 0x13a29d51b33d21e1,
            0x8016d96ec5b3e5df, 0x0d12ea7481262996, 0x0ba014f4fcf82814, 0xa94b6c896608a877, 0x0ce9531d42e1282f, 0x146b92651a66881b,
            0xa3a7dc6a86bc6ca7, 0x6832f1af6eeb7d1b, 0x80fa69802980962a, 0x4fbf24d0d8a67b5d, 0x81c083440206ac79, 0x1534b3c5ac621e6d,
            0x10236fc7bc489479, 0xe866bbcf232064e3, 0x8decedc977f53a0c, 0x01533415a5b2003d, 0xf33cb2ee84283a29, 0x15fdebcff69fce1e,
            0x1c6a16ed73097fed, 0x04b6b552132a277c, 0x873e480ede4b49f9, 0xd2e10ecf59b39d8d, 0x3181a4ff744e39c8, 0x16c6b4791c7551cb,
            0x3762db8a2f46ae6c, 0xe605126135c4d13e, 0x95cd248f2199c031, 0x046e6925e63b921f, 0x1edbbe0a60738519, 0x178fda3cf9a1f86a,
            0x622dd7b0d6052b2b, 0x6c52c04b49a43f67, 0x88c53f408d6effd6, 0x2f9443c296486b1e, 0xb29d416f839b4c6a, 0x1858d64de0abafb9,
            0x41e74e55d8377c5c, 0x6d71bf109d52b4b4, 0x257cc4f9c5ac1896, 0xb36f178c4ec3381d, 0x48b86fb55342a616, 0x192200ca656f9b4e,
            0x9ce847bba8a998f8, 0xd11a4c5b5dd0f414, 0xe01bcb3136d1ebdb, 0xa0862f030b9c517c, 0xa825b4e5ba19aa06, 0x19ead54cc2ca2709,
            0x414da7fe923aa6b3, 0x7937289ae91c4e1e, 0x023d066c6e855b6c, 0xd806ede78bcf8603, 0x7557aecadb39e171, 0x1ab4151b72c17c74,
            0xeba76a289fd187b2, 0x806ebe30e6895092, 0xc8828c04732a6e15, 0x7160eba8b1434d38, 0x425d9aff030711ea, 0x1b7d3b004cb73b1d,
            0xe3de9fa9dfe3af41, 0x88b2384a90ac9a3c, 0x4e49b80c84414a77, 0xc31388ba77526296, 0xadf256ec30dee5f4, 0x1c462f4336d5d87f,
            0xc8016db77a6fe180, 0x74d367ad0de559f3, 0x3590c43f99974721, 0xde083347f4ae4c39, 0x83520693c00f13ae, 0x1d0f41d64261e9ad,
            0x90d94a7879b7f974, 0x7f4261801e9d8880, 0xe65deec790b85094, 0xc1610e738a4c9f6c, 0xc4c9618e94bf1b41, 0x1dd857e3ab6ae652,
            0xcf5ca650e7803992, 0xda4170d655e373a3, 0xd03830653bb48d42, 0x389df7ea5a405f86, 0x988fa1d2cc3a0516, 0x1ea156d864e87301,
            0x63ac43b79fd4a119, 0x860223d73b8c2c2c, 0xd57b1b3b47b73093, 0x19cdea35fab86a95, 0xa4546bf333ad5f5e, 0x1f6a88026bdc2f47,
            0x1732b191c4b0e778, 0x9352dbb5661dd274, 0x144fe9b20ee58259, 0x0eb5ed9afee849a5, 0xad9b43ad3c3e1fa7, 0x20336b52f6bb9ab6,
            0xa982314f96806c98, 0x0b0c5b3f526aa38c, 0x90f83830b407e311, 0xad1715d205e8ce0e, 0x196bdc15af2356b3, 0x20fca91c686855d2,
            0xfe926dfb72a70556, 0x3b5f2a42166f3183, 0xa62b33bca0f85eca, 0xed2dbb498fb85d4d, 0xf309e31fbdc9aeb7, 0x21c5c0b58968023e,
            0xcf2fe2ca74009c66, 0xa73a2906aba43e6e, 0x9871f53ec824467b, 0x1a38ceab9fce7f8f, 0xbfdeff67b18287c1, 0x228e9579f321c783,
            0x4d3b3149caa4f593, 0xa46a448c68bdefa9, 0xaa62fe638979d0b3, 0x56d7577e496786b1, 0x9f94db3d59a078d7, 0x2357c4da5035f70f,
            0xf4eabdca73374af6, 0xae0684761c9ebb0f, 0x44cdeed621e49a28, 0x7928137ddd0fa064, 0xfd755dd3bdcbab23, 0x2420cf7ee51f46e0,
            0xd104943294266a7e, 0xa603aaff8ea1b4e0, 0xf074b304a436b2e8, 0x08f6bac958fc93e0, 0x6402f3a15aabb8f2, 0x24e9f01e8e1f712c,
            0x3d3b9d056aa1ee79, 0x345a09eb2405f477, 0xe168c8e2fb055c2a, 0x49420a6b6791619d, 0x8787fb8a5a45b05b, 0x25b303d3106131a6,
            0xb805175699cb99a9, 0x4fe4be06b00aec48, 0xd121ef2ed145dbc9, 0x6d83a3084993fdc2, 0xcd1219f2665f60d6, 0x267be85a925b38d2,
            0x73b42b0d4b9f4cf9, 0xa7b9b20992b3d8a1, 0x558f26417137e904, 0x257de5e0913d9714, 0x13b1717f1dc3721f, 0x2745274cfd47c28c,
            0x2c961ad29e10d590, 0xbe94571f4b227c15, 0xc65400b7a5517ca5, 0xf8c749addac40559, 0xd2adc89b03c20921, 0x280e42f64d3f9844,
            0x279b685af05d793c, 0xddfa01d66dc021d9, 0x03e0fc67ab913ee1, 0x6c0a3ecf5778f283, 0xe8e9142622ebdbb4, 0x28d716f50d9e9584,
            0xcf4ec61ec1887d94, 0x502713a07a267307, 0x5fdbb5a9b9c8750a, 0xd5cf19043439b62c, 0x1136bf78715f5851, 0x29a021f7d8cb387a,
            0xbca0a7dd978cef70, 0x98e574273b015563, 0x67a77b57da9afb00, 0x54de998591fc535c, 0xa86e6b5b7a9bae15, 0x2a693870fc67c444,
            0x7c6cbe72316ce0d5, 0x1e0fd83317165e74, 0x71adf16aa60f3af3, 0xa9341d97e4f6adcb, 0xe717978b9134d491, 0x2b327dd7ab2ca09d,
            0xa0ae2c41858d5c87, 0x239d87b916e40920, 0xa024d4fa90f4f26b, 0xb55be89ce49ef928, 0x28b56505dd97d3dd, 0x2bfb77a035071fb6,
            0xb0030b87c9b62d18, 0x9992bf2e20ce7cd4, 0x1765547834a268e5, 0xa187fec213be1016, 0x3fbcd166548791ee, 0x2cc4936b1987b1b3,
            0xd339bea7ca0b74e0, 0xc29c5e9826b1f6e3, 0x2f5e5ab61673e3f6, 0xff998a6f2d19532f, 0x9d5322247bb149e5, 0x2d8da0fcd932a561,
            0xd20d77dc0b077313, 0xbfccaa053a043a9b, 0x12b3010be6a1eb3f, 0xf316fcb46d0ffb40, 0x4095f9630a4eb3e1, 0x2e56b94da0515984,
            0xd168195d379149a7, 0x60a53f3de0e7efd3, 0x3658cd3898020ce5, 0xa30583591a2dffbe, 0x1e375bd40e0e3933, 0x2f1faa4f3a17939d,
            0xee927d556dfb2ae8, 0xb5cd6ae1598c854e, 0x217441a97323d25e, 0xe696c043bb881630, 0xe2445ad229e46e95, 0x2fe8ccf5ebf0a133,
            0x159fdfc634d4d38a, 0x9724055712324ced, 0x7c59984442c9d80a, 0x6b5f6d06b34c1da6, 0x12b2f428ff7acb76, 0x30b1e91cf0943362,
            0xa9acfc1b07dbdc4d, 0xcc77842a2fe6f682, 0xe7f3a1e83c315272, 0x19506e1283ee2a6b, 0xbe60466777b9622d, 0x317ac874b0fba710,
        ];

        internal static ReadOnlySpan<InlineArray6<UInt64>> Phi0 => MemoryMarshal.CreateReadOnlySpan(
            ref System.Runtime.CompilerServices.Unsafe.As<UInt64, InlineArray6<UInt64>>(ref MemoryMarshal.GetReference(Phi0_flat)), 64);

        // quadrant offsets
        internal static ReadOnlySpan<UInt64> Qoff_flat => [
            0, 0, 0, 0, 0, 0,
            0xf7ca8cd9e69d218e, 0x28a5043cc71a026e, 0x105df531d89cd91, 0x948127044533e63a, 0x62633145c06e0e68, 0x6487ed5110b4611a,
            0xef9519b3cd3a431b, 0x514a08798e3404dd, 0x20bbea63b139b22, 0x29024e088a67cc74, 0xc4c6628b80dc1cd1, 0xc90fdaa22168c234,
        ];

        internal static ReadOnlySpan<InlineArray6<UInt64>> Qoff => MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<UInt64, InlineArray6<UInt64>>(ref MemoryMarshal.GetReference(Qoff_flat)), 3);
    }


    partial class Binary128Arithmetic {

        static ReadOnlySpan<UInt64> RsAtan2Special_flat => [
            0, 0,
            0x62633145c06e0e69, 0x6487ed5110b4611a,
            0xc9ca64f450528ace, 0x4b65f1fccc8748d3,
            0x9898cc51701b839a, 0x1921fb54442d1846,
            0x313198a2e0370734, 0x3243f6a8885a308d,
        ];

        static UnsafeUInt128Accessor RsAtan2Special =>
            new UnsafeUInt128Accessor(in MemoryMarshal.GetReference(RsAtan2Special_flat));

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static UInt128 AsAtan2Special(UInt128 y, UInt128 x, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = (UInt64)1 << 63;

                UInt64 ixLo = (UInt64)x, ixHi = (UInt64)(x >> 64);
                UInt64 iyLo = (UInt64)y, iyHi = (UInt64)(y >> 64);

                uint xsgn = (uint)(ixHi >> 63);
                uint ysgn = (uint)(iyHi >> 63);

                // strip sign bit for classification
                ixHi &= ~smsk;
                iyHi &= ~smsk;

                UInt128 ixAbs = ((UInt128)ixHi << 64) | ixLo;
                UInt128 iyAbs = ((UInt128)iyHi << 64) | iyLo;

                int xnan = GetClass(ixAbs);
                int ynan = GetClass(iyAbs);

                UInt128 outVal;

                if (xnan == 2 || ynan == 2) {
                    // signaling NaN → invalid; return quiet NaN
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                    return GetNaN(y, x);
                } else if (ynan == 3) {
                    return y;                       // propagate quiet NaN (y)
                } else if (xnan == 3) {
                    return x;                       // propagate quiet NaN (x)
                } else if (xnan == 1 && ynan == 1) {
                    // atan2(±Inf, -Inf) = ±3π/4   atan2(±Inf, +Inf) = ±π/4
                    outVal = RsAtan2Special[3 - xsgn];
                } else if (xnan == 1) {
                    // atan2(±y, -Inf) = ±π      atan2(±y, +Inf) = ±0
                    outVal = RsAtan2Special[xsgn];
                } else if (ynan == 1) {
                    // atan2(±Inf, x) = ±π/2
                    outVal = RsAtan2Special[4];
                } else if (iyAbs == 0 && ixAbs == 0) {
                    // atan2(±0, -0) = ±π       atan2(±0, +0) = ±0
                    outVal = RsAtan2Special[xsgn];
                } else if (iyAbs == 0) {
                    // atan2(±0, -x) = ±π       atan2(±0, +x) = ±0
                    outVal = RsAtan2Special[xsgn];
                } else {
                    // ixAbs == 0 (and iyAbs != 0):  atan2(±y, 0) = ±π/2
                    outVal = RsAtan2Special[4];
                }

                UInt64 outLo = (UInt64)outVal;
                UInt64 outHi = (UInt64)(outVal >> 64);

                if (outHi != 0) {
                    // Renormalize the selected constant so its MSB lands in bit 63 of outHi.
                    int k = (int)UInt64.LeadingZeroCount(outHi);

                    UInt64 rnd = (outLo >> (14 - k)) & 1;
                    UInt64 frac = outLo << (49 + k);
                    int xn = 0x4000 - k;

                    outLo = (outLo >> (15 - k)) | (outHi << (49 + k));
                    outHi = outHi >> (15 - k);

                    // Rounding override for directed modes:
                    //   up   → round toward +∞   → increment only when result is positive
                    //   down → round toward −∞   → increment only when result is negative
                    if (!IsNearest(rm)) {
                        bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                        bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                        UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                        UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                        rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                    }

                    UInt64 rndBit = rnd * (frac != 0 ? 1UL : 0UL);
                    UInt128 dout = ((UInt128)(((UInt64)xn << 48) | ((UInt64)ysgn << 63)) << 64)
                                  | rndBit;

                    outVal = ((UInt128)outHi << 64) | outLo;
                    outVal += dout;

                    if (frac != 0)
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                } else {
                    // Selected constant was ±0 → emit signed zero.
                    outVal = (UInt128)((UInt64)ysgn << 63) << 64;
                }

                return outVal;
            }
        }

        static ReadOnlySpan<UInt64> CAtan2Core_flat => [
            0xffffffffffffffffUL, 0xffffffffffffffffUL,
            0x555555555555554fUL, 0x0015555555555555UL,
            0x3333333333332f55UL, 0x0000033333333333UL,
            0x49249249249147dfUL, 0x0000000092492492UL,
            0x1c71c71c71a599e3UL, 0x00000000001c71c7UL,
            0x745d1745cf00b15fUL, 0x00000000000005d1UL,
            0x3b13b13af8b70dd8UL, 0x0000000000000001UL,
            0x004444439744d102UL, 0x0000000000000000UL,
            0x00000f0cb8c66f08UL, 0x0000000000000000UL,
        ];

        static ReadOnlySpan<InlineArray2<UInt64>> CAtan2Core => MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<UInt64, InlineArray2<UInt64>>(ref MemoryMarshal.GetReference(CAtan2Core_flat)), 9);

        static UInt128 Atan2Core(UInt128 y, UInt128 x, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;
                const UInt64 inf = (UInt64)0x7FFF << 48;

                UInt64 xLo = (UInt64)x, xHi = (UInt64)(x >> 64);
                UInt64 yLo = (UInt64)y, yHi = (UInt64)(y >> 64);

                Int64 xsgn = (Int64)xHi >> 63;   // 0 or -1
                UInt64 ysgn = yHi & smsk;         // 0 or 0x8000000000000000

                xHi &= ~smsk;
                yHi &= ~smsk;

                if (Misc.Unlikely(yHi >= inf || xHi >= inf))
                    return AsAtan2Special(y, x, rm);

                UInt128 xAbs = ((UInt128)xHi << 64) | xLo;
                UInt128 yAbs = ((UInt128)yHi << 64) | yLo;

                // Ensure a = max(xAbs, yAbs) via signed-128 trick
                Int128 dab = unchecked((Int128)xAbs) - unchecked((Int128)yAbs);
                Int64 g = (Int64)(dab >> 127);       // 0 or -1
                dab &= (Int128)g;
                UInt128 aVal = xAbs - unchecked((UInt128)dab);
                UInt128 bVal = yAbs + unchecked((UInt128)dab);

                UInt64 aHi = (UInt64)(aVal >> 64), aLo = (UInt64)aVal;
                UInt64 bHi = (UInt64)(bVal >> 64), bLo = (UInt64)bVal;

                int xn = (int)(aHi >> 48);
                int yn = (int)(bHi >> 48);

                if (Misc.Unlikely(yn == 0)) {
                    int ns = -15;
                    if (bHi != 0) ns += (int)BitOperations.LeadingZeroCount(bHi);
                    else if (bLo != 0) ns += (int)BitOperations.LeadingZeroCount(bLo) + 64;
                    else return AsAtan2Special(y, x, rm);

                    yn = 1 - ns;
                    bLo = DoubleArithmetic.ShiftLeft(bLo, bHi, ns, out bHi);
                    bVal = ((UInt128)bHi << 64) | bLo;

                    if (Misc.Unlikely(xn == 0)) {
                        ns = -15;
                        if (Misc.Likely(aHi != 0)) ns += (int)BitOperations.LeadingZeroCount(aHi);
                        else ns += (int)BitOperations.LeadingZeroCount(aLo) + 64;
                        xn = 1 - ns;
                        aLo = DoubleArithmetic.ShiftLeft(aLo, aHi, ns, out aHi);
                        aVal = ((UInt128)aHi << 64) | aLo;
                    }
                }

                int dn = xn - yn;
                aHi = (aHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                bHi = (bHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                aVal = ((UInt128)aHi << 64) | aLo;
                bVal = ((UInt128)bHi << 64) | bLo;

                // Approximate reciprocal step to select the tangent index
                int inda = (int)((aHi >> 41) & 127);
                long rcpx = (2L + (inda == 0 ? 1 : 0) + (inda < 43 ? 1 : 0)) << 8 | (long)Rcp[inda];
                long kL = (long)(((bHi >> 10) * (UInt64)rcpx) >> 41);
                if (Misc.Likely(dn < 64)) kL >>= dn; else kL = 0;

                int isct = Ind[(int)kL];
                if (dn < 64 && (aHi >> 15) * Tn[isct + 1] < (bHi >> dn))
                    isct++;

                UInt128 kn, kd;

                if (isct == 0) {
                    kn = bVal << 15;
                    kd = aVal << 15;
                } else {
                    if (Misc.Unlikely(isct == 64)) isct = 63;
                    kn = (bVal << 15) - ((aVal * Tn[isct]) << dn);
                    if (Misc.Unlikely((kn >> 127) != 0))
                        kn = (bVal << 15) - ((aVal * Tn[--isct]) << dn);

                    UInt64 knh = (UInt64)(kn >> 64);
                    UInt64 knl = (UInt64)kn;
                    int nzn = kn != 0
                        ? (Misc.Likely(knh != 0) ? (int)BitOperations.LeadingZeroCount(knh)
                                    : 64 + (int)BitOperations.LeadingZeroCount(knl))
                        : 0;
                    kn <<= nzn;

                    int sl = 15 + dn, sr = 49 - dn;
                    InlineArray3<UInt64> kd3 = default;
                    kd3[2] = aHi >> sr;
                    kd3[1] = (aLo >> sr) | (aHi << sl);
                    kd3[0] = aLo << sl;

                    bVal *= Tn[isct];
                    // Preserve union invariant: bHi, bLo reflect the multiplied value.
                    bHi = (UInt64)(bVal >> 64);
                    bLo = (UInt64)bVal;
                    AddUnchecked(out kd3, in kd3, bVal);

                    UInt64 kdh = kd3[2];
                    int nzd, md;
                    if (kdh != 0) {
                        nzd = (int)BitOperations.LeadingZeroCount(kdh);
                        md = 64 - nzd;
                        UInt64 hi = (kdh << nzd) | (kd3[1] >> md);
                        UInt64 lo = (kd3[1] << nzd) | (kd3[0] >> md);
                        kd = ((UInt128)hi << 64) | lo;
                    } else {
                        kdh = kd3[1];
                        nzd = (int)BitOperations.LeadingZeroCount(kdh);
                        md = (~nzd) & 63;
                        UInt64 hi = (kdh << nzd) | (kd3[0] >> 1 >> md);
                        UInt64 lo = kd3[0] << nzd;
                        kd = ((UInt128)hi << 64) | lo;
                        nzd += 64;
                    }
                    dn = nzn - nzd + 64;
                }

                UInt128 R = ReciprocalU(kd);
                UInt128 T = MultiplyHighApproximate(kn, R);
                UInt128 T2 = SquareHighApproximate(T);
                int dn2 = 2 * dn - 12;
                if (Misc.Likely(dn2 < 128)) T2 >>= dn2; else T2 = 0;

                // Polynomial: f = T - T·T2·P(T2) where P uses c[5..1]
                UInt64 t2h = (UInt64)(T2 >> 64);
                UInt64 fl = CAtan2Core_flat[8 * 2 + 0];
                fl = CAtan2Core_flat[7 * 2 + 0] - MultiplyHigh(t2h, fl);
                fl = CAtan2Core_flat[6 * 2 + 0] - MultiplyHigh(t2h, fl);

                UInt128 f = ((UInt128)CAtan2Core_flat[5 * 2 + 1] << 64)
                          | (CAtan2Core_flat[5 * 2 + 0] - t2h - MultiplyHigh(t2h, fl));
                for (int ii = 4; ii >= 1; ii--) {
                    UInt128 ci = ((UInt128)CAtan2Core_flat[ii * 2 + 1] << 64) | CAtan2Core_flat[ii * 2 + 0];
                    f = ci - MultiplyHighApproximate(T2, f);
                }
                f = MultiplyHighApproximate(T2, f);
                f = MultiplyHighApproximate(T, f);
                f = T - f;

                UInt128 v;
                UInt64 rnd;

                if (Misc.Likely((((Int64)isct | g) | xsgn) != 0)) {
                    // Path through 3-word fixup + phase offset
                    InlineArray3<UInt64> f3 = default;
                    f3[0] = 0;
                    f3[1] = (UInt64)f;
                    f3[2] = (UInt64)(f >> 64);

                    dn++;
                    if (Misc.Likely(dn < 64)) {
                        f3[0] = f3[1] << (64 - dn);
                        f3[1] = (f3[1] >> dn) | (f3[2] << (64 - dn));
                        f3[2] = f3[2] >> dn;
                    } else {
                        ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref f3[0], 3), dn);
                    }

                    AddUnchecked(out f3, in f3, in Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in Phi0[isct][3])));

                    Int64 msk = g ^ xsgn;
                    f3[0] ^= (UInt64)msk;
                    f3[1] ^= (UInt64)msk;
                    f3[2] ^= (UInt64)msk;

                    int qidx = (g != 0 ? 1 : 0) + ((g == 0 && xsgn != 0) ? 2 : 0);
                    AddUnchecked(out f3, in f3, in Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in Qoff[qidx][3])));

                    int kk = (int)BitOperations.LeadingZeroCount(f3[2]);
                    rnd = (f3[1] >> (14 - kk)) & 1;
                    UInt128 t = ((UInt128)f3[1] << 64) | f3[0];
                    const UInt64 eps = 0xCA2339C0EBEDFA4UL;
                    t += eps;
                    UInt64 th = (UInt64)(t >> 64);
                    UInt64 tl = (UInt64)t;
                    th &= (1UL << (15 - kk)) - 1;
                    th ^= (UInt64)(IsNearest(rm) ? 1UL : 0UL) << (14 - kk);
                    if (th == 0 && tl < 0x194467381D7DBF48UL)
                        return AsAtan2Accurate(y, x, rm);

                    xn = 0x3FFF - kk;
                    UInt64 vHi = f3[2] >> (15 - kk);
                    UInt64 vLo = (f3[1] >> (15 - kk)) | (f3[2] << (49 + kk));
                    v = ((UInt128)vHi << 64) | vLo;
                } else {
                    v = f;
                    UInt64 vHi = (UInt64)(v >> 64);
                    UInt64 vLo = (UInt64)v;
                    int kk = (int)BitOperations.LeadingZeroCount(vHi);
                    xn = 0x3FFE - dn - kk;
                    if (xn > 0) {
                        UInt64 tl = (vLo + 6) & (~0UL >> (49 + kk));
                        tl ^= (UInt64)(IsNearest(rm) ? 1UL : 0UL) << (14 - kk);
                        if (Misc.Unlikely(tl <= 15)) return AsAtan2Accurate(y, x, rm);
                        rnd = (vLo >> (14 - kk)) & 1;
                        vLo = (vLo >> (15 - kk)) | (vHi << (49 + kk));
                        vHi = vHi >> (15 - kk);
                        v = ((UInt128)vHi << 64) | vLo;
                    } else {
                        xn = 0;
                        kk = 15 - 0x3FFE + dn;
                        if (kk < 128) {
                            UInt128 t = (v + 6) & (((UInt128)1 << kk) - 1);
                            t ^= (UInt128)(IsNearest(rm) ? 1UL : 0UL) << (kk - 1);
                            if (Misc.Unlikely(t <= 15)) return AsAtan2Accurate(y, x, rm);
                            rnd = (UInt64)((v >> (kk - 1)) & 1);
                            v >>= kk;
                        } else {
                            rnd = (kk == 128) ? (UInt64)(v >> 127) : 0;
                            v = 0;
                        }
                    }
                }

                if (Misc.Unlikely(!IsNearest(rm))) {
                    bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                    bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                    UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                    UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                    rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                }

                UInt128 dv = ((UInt128)((UInt64)xn << 48) << 64) | rnd;
                v += dv;

                UInt64 vHiF = (UInt64)(v >> 64);
                if (vHiF < (1UL << 48))
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);

                vHiF |= ysgn;
                v = ((UInt128)vHiF << 64) | (UInt64)v;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt128 ReciprocalU(UInt128 kd) {
            unchecked {
                UInt128 kdh = kd >> 64;
                UInt128 kdl = (UInt64)kd;      // low 64 bits, kept as u128 per C's Hl=kdl*r

                UInt128 n = (UInt128)1 << 127;

                // r = n / kdh, then truncated to u64.  When kdh > 2^63 the true quotient
                // exceeds u64 range and truncates to 0; the guard turns that into ~0.
                UInt64 r = (UInt64)(n / kdh);
                if (Misc.Unlikely(r == 0)) r = ~0UL;

                // Hh = kdh * r  (fits in i128: both factors < 2^64)
                Int128 Hh = (Int128)kdh * (Int128)r;
                UInt128 Hl = kdl * r;

                Hh += (Int128)(Hl >> 64);
                Hh <<= 57;
                Hh |= (Int128)((UInt64)Hl >> 7);

                Int64 hh = (Int64)(Hh >> 92);

                // mhuU(r, Hh): high 128 bits of the 192-bit product (u64)r * (u128)Hh.
                //   r * Hh = (r * Hh_hi) << 64 | (r * Hh_lo)
                //   high128 = r * Hh_hi + ((r * Hh_lo) >> 64)
                UInt64 hhLo = (UInt64)(UInt128)Hh;
                UInt64 hhHi = (UInt64)((UInt128)Hh >> 64);
                UInt128 pHi = Math.BigMul(r, hhHi);
                UInt128 pLo = Math.BigMul(r, hhLo);
                UInt128 mhu = pHi + (pLo >> 64);

                // sub = ((hh >> 63) & r) << 64   — sign-extended mask AND r, placed high
                UInt64 mask = (UInt64)(hh >> 63);      // 0 or 0xFFFF...FFFF
                Int128 sub = (Int128)(mask & r) << 64;

                // Both operands must be reinterpreted to the same 128-bit ring for
                // the subtraction to match C's `u128 - i128` (u128 wins by equal rank).
                Int128 dR = (Int128)mhu - sub;
                dR >>= 56;

                UInt128 R = (UInt128)r << 64;
                R -= (UInt128)dR;
                return R;
            }
        }


        static ReadOnlySpan<UInt64> CpAtan2Acc => [
            0x41df126e5ec4236b, 0x0000000000005029, 0xe7141aa59e9c06bb, 0x0000000005397794, 0xa29ae03dd40fb732, 0x000000572620ace8,
            0xf6c75ac2bf3ba522, 0x0005b05b05b058b0, 0x7f991c081742d872, 0x5f417d05f417cd73, 0xdfd4fc68937baa13, 0x7063e7063e706110,
            0x000000000000063e, 0xd2afeb0c4c44b156, 0x69069069069066d6, 0x0000000000690690, 0x3041b7df0f1e81af, 0x6eb3e45306eb3ce9,
            0x00000006eb3e4530, 0xf51a7ea3f72eba0a, 0x7507507507507456, 0x0000750750750750, 0x6d164965256c0f25, 0x7c1f07c1f07c1ebe,
            0x07c1f07c1f07c1f0, 0xed1197809680b661, 0x1084210842108407, 0x2108421084210842, 0x0000000000000084, 0x288d0c52f8e6282c,
            0x8d3dcb08d3dcb086, 0xb08d3dcb08d3dcb0, 0x000000000008d3dc, 0x81137e954e3d7cb3, 0xed097b425ed097b2, 0x097b425ed097b425,
            0x0000000097b425ed, 0x209a8d788f1f9fe1, 0xd70a3d70a3d70a3d, 0x3d70a3d70a3d70a3, 0x00000a3d70a3d70a, 0x09decb83cffc8425,
            0x590b21642c8590b2, 0x642c8590b21642c8, 0x00b21642c8590b21, 0x2f3708c2cde40e8c, 0xc30c30c30c30c30c, 0x0c30c30c30c30c30,
            0x30c30c30c30c30c3, 0x000000000000000c, 0x50b0362e3e44ed43, 0xd79435e50d79435e, 0x9435e50d79435e50, 0x35e50d79435e50d7,
            0x000000000000d794, 0x0f0bfeb7bb779a21, 0x0f0f0f0f0f0f0f0f, 0x0f0f0f0f0f0f0f0f, 0x0f0f0f0f0f0f0f0f, 0x000000000f0f0f0f,
            0x1110e255d91317d2, 0x1111111111111111, 0x1111111111111111, 0x1111111111111111, 0x0000011111111111, 0xb13b119fb0d7521d,
            0x3b13b13b13b13b13, 0x13b13b13b13b13b1, 0xb13b13b13b13b13b, 0x0013b13b13b13b13, 0x1745d163a699798c, 0xd1745d1745d1745d,
            0x5d1745d1745d1745, 0x45d1745d1745d174, 0x745d1745d1745d17, 0x0000000000000001, 0x71c71c716c4c5694, 0xc71c71c71c71c71c,
            0x1c71c71c71c71c71, 0x71c71c71c71c71c7, 0xc71c71c71c71c71c, 0x0000000000001c71, 0x9249249247f4c207, 0x4924924924924924,
            0x2492492492492492, 0x9249249249249249, 0x4924924924924924, 0x0000000002492492, 0x33333333333116c0, 0x3333333333333333,
            0x3333333333333333, 0x3333333333333333, 0x3333333333333333, 0x0000003333333333, 0x55555555555553d6, 0x5555555555555555,
            0x5555555555555555, 0x5555555555555555, 0x5555555555555555, 0x0005555555555555,
        ];

        static UInt128 AsAtan2Accurate(UInt128 y, UInt128 x, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;

                UInt64 xHi = (UInt64)(x >> 64), xLo = (UInt64)x;
                UInt64 yHi = (UInt64)(y >> 64), yLo = (UInt64)y;

                UInt64 xsgn = xHi >> 63;     // 0 or 1
                UInt64 ysgn = yHi & smsk;    // 0 or 0x8000...

                xHi &= ~smsk;
                yHi &= ~smsk;

                UInt128 xAbs = ((UInt128)xHi << 64) | xLo;
                UInt128 yAbs = ((UInt128)yHi << 64) | yLo;

                Int128 dab = unchecked((Int128)xAbs) - unchecked((Int128)yAbs);
                Int64 g = (Int64)(dab >> 127);
                dab &= (Int128)g;
                UInt128 aVal = xAbs - unchecked((UInt128)dab);
                UInt128 bVal = yAbs + unchecked((UInt128)dab);

                UInt64 aHi = (UInt64)(aVal >> 64), aLo = (UInt64)aVal;
                UInt64 bHi = (UInt64)(bVal >> 64), bLo = (UInt64)bVal;

                int xn = (int)(aHi >> 48);
                int yn = (int)(bHi >> 48);

                if (Misc.Unlikely(yn == 0)) {
                    int ns = -15;
                    if (bHi != 0) ns += (int)BitOperations.LeadingZeroCount(bHi);
                    else ns += (int)BitOperations.LeadingZeroCount(bLo) + 64;
                    yn = 1 - ns;
                    bLo = DoubleArithmetic.ShiftLeft(bLo, bHi, ns, out bHi);
                    bVal = ((UInt128)bHi << 64) | bLo;

                    if (Misc.Unlikely(xn == 0)) {
                        ns = -15;
                        if (aHi != 0) ns += (int)BitOperations.LeadingZeroCount(aHi);
                        else ns += (int)BitOperations.LeadingZeroCount(aLo) + 64;
                        xn = 1 - ns;
                        aLo = DoubleArithmetic.ShiftLeft(aLo, aHi, ns, out aHi);
                        aVal = ((UInt128)aHi << 64) | aLo;
                    }
                }

                int dn = xn - yn;
                aHi = (aHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                bHi = (bHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                // Keep the 128-bit aliases in sync with the masked hi/lo halves.
                aVal = ((UInt128)aHi << 64) | aLo;
                bVal = ((UInt128)bHi << 64) | bLo;

                int inda = (int)((aHi >> 41) & 127);
                long rcpx = (2L + (inda == 0 ? 1 : 0) + (inda < 43 ? 1 : 0)) << 8 | (long)Rcp[inda];
                long kL = (long)(((bHi >> 10) * (UInt64)rcpx) >> 41);
                if (Misc.Likely(dn < 64)) kL >>= dn; else kL = 0;
                int isct = Ind[(int)kL];
                if (dn < 64 && (aHi >> 15) * Tn[isct + 1] < (bHi >> dn)) isct++;

                UInt128 kn;
                InlineArray3<UInt64> kd = default;

                if (isct == 0) {
                    kn = bVal << 15;
                    aVal <<= 15;
                    // Keep aHi/aLo consistent with aVal (mirrors C union aliasing).
                    aHi = (UInt64)(aVal >> 64);
                    aLo = (UInt64)aVal;
                    kd[2] = aHi;
                    kd[1] = aLo;
                    kd[0] = 0;
                } else {
                    if (isct == 64) isct = 63;
                    kn = (bVal << 15) - ((aVal * Tn[isct]) << dn);
                    if (Misc.Unlikely((kn >> 127) != 0)) kn = (bVal << 15) - ((aVal * Tn[--isct]) << dn);

                    UInt64 knh = (UInt64)(kn >> 64);
                    UInt64 knl = (UInt64)kn;
                    int nzn = kn != 0
                        ? (knh != 0 ? (int)BitOperations.LeadingZeroCount(knh)
                                    : 64 + (int)BitOperations.LeadingZeroCount(knl))
                        : 0;
                    kn <<= nzn;

                    int sl = 15 + dn, sr = 64 - sl;
                    kd[2] = aHi >> sr;
                    kd[1] = (aLo >> sr) | (aHi << sl);
                    kd[0] = aLo << sl;

                    bVal *= Tn[isct];
                    AddUnchecked(out kd, in kd, bVal);

                    int nzd = 0;
                    if (kd[2] == 0) { kd[2] = kd[1]; kd[1] = kd[0]; kd[0] = 0; nzd = 64; }
                    sl = (int)BitOperations.LeadingZeroCount(kd[2]);
                    sr = (~sl) & 63;
                    nzd += sl;
                    kd[2] = (kd[2] << sl) | (kd[1] >> 1 >> sr);
                    kd[1] = (kd[1] << sl) | (kd[0] >> 1 >> sr);
                    kd[0] = kd[0] << sl;
                    dn = nzn - nzd + 64;
                }

                UInt128 R = ReciprocalU(((UInt128)kd[2] << 64) | kd[1]);

                // r = (0,0,0,0, R_low, R_high) as a 6-word vector
                InlineArray6<UInt64> r = default;
                r[4] = (UInt64)R;
                r[5] = (UInt64)(R >> 64);

                // H = mu5u2u3(r[4..5], kd) — 5-word result
                InlineArray2<UInt64> rTop = default;
                rTop[0] = r[4]; rTop[1] = r[5];
                InlineArray5<UInt64> H;
                BigMulUnsigned(out H, in rTop, in kd);

                // sH = H >> 58 (top 3 words)
                InlineArray3<UInt64> sH = default;
                sH[0] = (H[0] >> 58) | (H[1] << 6);
                sH[1] = (H[1] >> 58) | (H[2] << 6);
                sH[2] = (H[2] >> 58) | (H[3] << 6);

                InlineArray6<UInt64> H2;
                SquareSigned(out H2, in sH);

                // H <<= 11+64 = 75
                ShiftLeftUnsignedFull(MemoryMarshal.CreateSpan(ref H[0], 5), 75);

                // H -= H2[2..6]
                nuint cc;
                H[0] = SubtractWithBorrow(H[0], H2[2], 0, out cc);
                H[1] = SubtractWithBorrow(H[1], H2[3], cc, out cc);
                H[2] = SubtractWithBorrow(H[2], H2[4], cc, out cc);
                H[3] = SubtractWithBorrow(H[3], 0, cc, out cc);
                H[4] = SubtractWithBorrow(H[4], 0, cc, out cc);

                // dR = mu7u5u2(H, r[4..5])
                InlineArray7<UInt64> dR = default;
                BigMulUnsigned(out dR, in H, in rTop);

                if ((H[4] >> 63) != 0) {
                    dR[5] = SubtractWithBorrow(dR[5], r[4], 0, out cc);
                    dR[6] = SubtractWithBorrow(dR[6], r[5], cc, out cc);
                }

                ShiftRightSignedFull(MemoryMarshal.CreateSpan(ref dR[0], 7), 10);

                r[0] = SubtractWithBorrow(r[0], dR[2], 0, out cc);
                r[1] = SubtractWithBorrow(r[1], dR[3], cc, out cc);
                r[2] = SubtractWithBorrow(r[2], dR[4], cc, out cc);
                r[3] = SubtractWithBorrow(r[3], dR[5], cc, out cc);
                r[4] = SubtractWithBorrow(r[4], dR[6], cc, out cc);
                r[5] = SubtractWithBorrow(r[5], (UInt64)((Int64)dR[6] >> 63), cc, out cc);

                // t = mu8u6u2(r, kn)
                InlineArray2<UInt64> nn = default;
                nn[0] = (UInt64)kn;
                nn[1] = (UInt64)(kn >> 64);
                InlineArray8<UInt64> t = default;
                MultiplyUnsigned(out t, in r, in nn);

                // t2 = sqrhu6(t[2..7])
                InlineArray6<UInt64> tLo = default;
                tLo[0] = t[2]; tLo[1] = t[3]; tLo[2] = t[4];
                tLo[3] = t[5]; tLo[4] = t[6]; tLo[5] = t[7];
                InlineArray6<UInt64> t2 = default;
                SquareHighUnsignedApproximate(out t2, in tLo);
                int dn2 = 2 * dn - 14;
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref t2[0], 6), dn2);

                // Long polynomial evaluation on cp[] (CpAtan2Acc)
                ReadOnlySpan<UInt64> cp = CpAtan2Acc;
                InlineArray6<UInt64> fp = default;

                int ck = 0;
                fp[0] = cp[0];
                fp[1] = cp[1];

                // 2-word phase (5 iters)
                for (int i = 25; i > 20; i--) {
                    ck += 2;
                    ref InlineArray2<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, fp_, Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in t2[4])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[2] = cp[ck + 2];

                // 3-word phase (5 iters)
                for (int i = 20; i > 15; i--) {
                    ck += 3;
                    ref InlineArray3<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in t2[3])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[3] = cp[ck + 3];

                // 4-word phase
                for (int i = 15; i > 10; i--) {
                    ck += 4;
                    ref InlineArray4<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in t2[2])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[4] = cp[ck + 4];

                // 5-word phase
                for (int i = 10; i > 5; i--) {
                    ck += 5;
                    ref InlineArray5<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in t2[1])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[5] = cp[ck + 5];

                // 6-word phase (4 iters)
                for (int i = 5; i > 1; i--) {
                    ck += 6;
                    MultiplyHighUnsignedApproximate(out fp, in fp, in t2);
                    SubtractUnchecked(out fp, in Unsafe.As<UInt64, InlineArray6<UInt64>>(ref Unsafe.AsRef(in cp[ck])), in fp);
                }
                MultiplyHighUnsignedApproximate(out fp, in fp, in t2);
                MultiplyHighUnsignedApproximate(out fp, in tLo, in fp);
                SubtractUnchecked(out fp, in tLo, in fp);

                if (((UInt64)((long)isct | g) | xsgn) != 0) {
                    ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref fp[0], 6), dn + 1);
                    AddUnchecked(out fp, in Phi0[isct], in fp);
                    Int64 msk = g ^ -(Int64)xsgn;
                    fp[0] ^= (UInt64)msk;
                    fp[1] ^= (UInt64)msk;
                    fp[2] ^= (UInt64)msk;
                    fp[3] ^= (UInt64)msk;
                    fp[4] ^= (UInt64)msk;
                    fp[5] ^= (UInt64)msk;
                    int qidx = (g != 0 ? 1 : 0) + ((g == 0 && xsgn != 0) ? 2 : 0);
                    AddUnchecked(out fp, in Qoff[qidx], in fp);
                    dn = -1;
                }

                UInt128 v = U128(fp[5], fp[4]);
                System.Diagnostics.Debug.Assert(BitOperations.LeadingZeroCount(fp[5]) == LeadingZeroCount(v));
                int kk = (int)BitOperations.LeadingZeroCount(fp[5]);
                v <<= kk;
                xn = 0x3FFE - dn - kk;

                UInt64 rnd;
                if (xn > 0) {
                    rnd = ((nuint)v >> 14) & 1;
                    v >>= 15;
                } else {
                    kk = 15 - xn;
                    if (kk < 128) {
                        rnd = (UInt64)((v >> (kk - 1)) & 1);
                        v >>= kk;
                    } else {
                        rnd = (kk == 128) ? (UInt64)(v >> 127) : 0;
                        v = 0;
                    }
                    xn = 0;
                }

                if (Misc.Unlikely(!IsNearest(rm))) {
                    bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                    bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                    UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                    UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                    rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                }

                UInt128 dv = ((UInt128)((UInt64)xn << 48) << 64) | rnd;
                v += dv;

                UInt64 vHiF = (UInt64)(v >> 64);
                if (vHiF < (1UL << 48))
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);

                vHiF |= ysgn;
                v = ((UInt128)vHiF << 64) | (UInt64)v;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }

    }

    partial class Binary128Arithmetic {

        public static UInt128 Atan2(UInt128 y, UInt128 x, MidpointRounding mode) {
            return Atan2Core(y, x, mode);
        }

        public static UInt128 Atan2(UInt128 y, UInt128 x) {
            return Atan2Core(y, x, MidpointRounding.ToEven);
        }

        public static UInt64 Atan2(UInt64 y_lo, UInt64 y_hi, UInt64 x_lo, UInt64 x_hi,
            MidpointRounding mode, out UInt64 result_hi) {
            unchecked {
                // Reassemble 128‑bit operands
                UInt128 y = ((UInt128)y_hi << 64) | y_lo;
                UInt128 x = ((UInt128)x_hi << 64) | x_lo;

                // Compute atan2 using your canonical core
                UInt128 result = Atan2Core(y, x, mode);

                result_hi = (UInt64)(result >> 64);
                return (UInt64)result;
            }
        }

        public static UInt64 Atan2(UInt64 y_lo, UInt64 y_hi, UInt64 x_lo, UInt64 x_hi,
            out UInt64 result_hi) {
            unchecked {
                // Default rounding = ToEven
                UInt128 y = ((UInt128)y_hi << 64) | y_lo;
                UInt128 x = ((UInt128)x_hi << 64) | x_lo;

                UInt128 result = Atan2Core(y, x, MidpointRounding.ToEven);

                result_hi = (UInt64)(result >> 64);
                return (UInt64)result;
            }
        }
    }
}




namespace UltimateOrb.Numerics {
    using Unsafe = System.Runtime.CompilerServices.Unsafe;

#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    partial class Binary128Arithmetic {

        public static UInt128 Atan(UInt128 y, MidpointRounding rounding) => AtanCore(y, rounding);
        public static UInt128 Atan(UInt128 y) => AtanCore(y, MidpointRounding.ToEven);

        public static UInt64 Atan(UInt64 y_lo, UInt64 y_hi,
            MidpointRounding rounding, out UInt64 result_hi) {
            unchecked {
                UInt128 r = AtanCore(((UInt128)y_hi << 64) | y_lo, rounding);
                result_hi = (UInt64)(r >> 64);
                return (UInt64)r;
            }
        }

        public static UInt64 Atan(UInt64 y_lo, UInt64 y_hi, out UInt64 result_hi)
            => Atan(y_lo, y_hi, MidpointRounding.ToEven, out result_hi);
    }


    partial class Binary128Arithmetic {

        internal static ReadOnlySpan<InlineArray6<UInt64>> Pio2 => MemoryMarshal.CreateReadOnlySpan(
            ref Unsafe.As<UInt64, InlineArray6<UInt64>>(ref MemoryMarshal.GetReference(PIO2_384_flat)), 1);

        // --- C: 0x1.921fb54442d18469898cc51701b8p+0q — literal π/2 as u128 ---
        const UInt64 PiOver2_hi = 0x3FFF921FB54442D1UL;
        const UInt64 PiOver2_lo = 0x849898CC51701B80UL;
    }

    partial class Binary128Arithmetic {

        // ------------------------------------------------------------------
        // Special-case handler for Atan(y) — the analogue of AsAtan2Special
        // with x = +1.0 (so `xsgn = 0` and `x` is never NaN/Inf).
        // Reachable only when y is sNaN/qNaN/±Inf (from AtanCore's early
        // test) or y == ±0 (from the b == 0 fallback in AtanCore).
        // ------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static UInt128 AsAtanSpecial(UInt128 y, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = (UInt64)1 << 63;

                UInt64 iyLo = (UInt64)y;
                UInt64 iyHi = (UInt64)(y >> 64);
                uint ysgn = (uint)(iyHi >> 63);

                iyHi &= ~smsk;
                UInt128 iyAbs = ((UInt128)iyHi << 64) | iyLo;

                int ynan = GetClass(iyAbs);

                UInt128 outVal;

                if (ynan == 2) {
                    // signaling NaN
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                    return GetNaN(y);
                } else if (ynan == 3) {
                    // quiet NaN — pass through
                    return y;
                } else if (ynan == 1) {
                    // ±Inf  →  ±π/2
                    outVal = RsAtan2Special[4];
                } else if (iyAbs == 0) {
                    // ±0  →  ±0  (sign of y)
                    outVal = RsAtan2Special[0];
                } else {
                    // Unreachable when x = +1.0; defensive fallback.
                    throw null!;
                }

                UInt64 outLo = (UInt64)outVal;
                UInt64 outHi = (UInt64)(outVal >> 64);

                if (outHi != 0) {
                    // Renormalize the selected constant so MSB lands in bit 63 of outHi.
                    int k = (int)UInt64.LeadingZeroCount(outHi);

                    UInt64 rnd = (outLo >> (14 - k)) & 1;
                    UInt64 frac = outLo << (49 + k);
                    int xn = 0x4000 - k;

                    outLo = (outLo >> (15 - k)) | (outHi << (49 + k));
                    outHi = outHi >> (15 - k);

                    // Directed rounding override (same convention as AsAtan2Special).
                    if (!IsNearest(rm)) {
                        bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                        bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                        UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                        UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                        rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                    }

                    UInt64 rndBit = rnd * (frac != 0 ? 1UL : 0UL);
                    UInt128 dout = ((UInt128)(((UInt64)xn << 48) | ((UInt64)ysgn << 63)) << 64)
                                 | rndBit;

                    outVal = ((UInt128)outHi << 64) | outLo;
                    outVal += dout;

                    if (frac != 0)
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                } else {
                    // ±0 (RsAtan2Special[0]) → signed zero.
                    outVal = (UInt128)((UInt64)ysgn << 63) << 64;
                }

                return outVal;
            }
        }

        // ------------------------------------------------------------------
        // AtanCore — Atan2Core specialized with x = +1.0.
        //
        // Derived simplifications applied (relative to Atan2Core):
        //   * `xsgn` is identically 0.
        //   * The early bail-out only tests y (x = 1.0 never triggers it).
        //   * `if (xn == 0)` inside the subnormal-normalization step is dead
        //     code:  a is either One (biased exp 0x3FFF) or |y| > 1 (biased
        //     exp ≥ 1).
        //   * `g ^ xsgn` ⇒ `g`,  `qidx` ⇒ `(g != 0) ? 1 : 0`,
        //     `(isct | g) | xsgn` ⇒ `(isct | g)`.
        //   * `AsAtan2Accurate(y, One, rm)` is used unchanged for the
        //     accurate-recompute slow path.
        // ------------------------------------------------------------------
        static UInt128 AtanCore(UInt128 y, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;
                const UInt64 inf = (UInt64)0x7FFF << 48;

                UInt64 yLo = (UInt64)y;
                UInt64 yHi = (UInt64)(y >> 64);

                UInt64 ysgn = yHi & smsk;   // 0 or 0x8000_0000_0000_0000
                yHi &= ~smsk;

                if (Misc.Unlikely(yHi >= inf))
                    return AsAtanSpecial(y, rm);

                UInt128 yAbs = ((UInt128)yHi << 64) | yLo;

                // g = -1  ⇔  |y| > 1.0   (compare |y| against One as bit patterns)
                Int128 dab = unchecked((Int128)ONE) - unchecked((Int128)yAbs);
                Int64 g = (Int64)(dab >> 127);    // 0 or -1
                dab &= (Int128)g;
                UInt128 aVal = ONE - unchecked((UInt128)dab);
                UInt128 bVal = yAbs + unchecked((UInt128)dab);

                UInt64 aHi = (UInt64)(aVal >> 64), aLo = (UInt64)aVal;
                UInt64 bHi = (UInt64)(bVal >> 64), bLo = (UInt64)bVal;

                int xn = (int)(aHi >> 48);
                int yn = (int)(bHi >> 48);

                // Only `b` can be subnormal here — see header comment.
                // If b == 0 then y == ±0, route to AsAtanSpecial.
                if (Misc.Unlikely(yn == 0)) {
                    int ns = -15;
                    if (bHi != 0) ns += (int)BitOperations.LeadingZeroCount(bHi);
                    else if (bLo != 0) ns += (int)BitOperations.LeadingZeroCount(bLo) + 64;
                    else return AsAtanSpecial(y, rm);   // y == ±0

                    yn = 1 - ns;
                    bLo = DoubleArithmetic.ShiftLeft(bLo, bHi, ns, out bHi);
                    bVal = ((UInt128)bHi << 64) | bLo;
                }

                int dn = xn - yn;

                aHi = (aHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                bHi = (bHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                aVal = ((UInt128)aHi << 64) | aLo;
                bVal = ((UInt128)bHi << 64) | bLo;

                // Approximate reciprocal step to select the tangent index
                int inda = (int)((aHi >> 41) & 127);
                long rcpx = (2L + (inda == 0 ? 1 : 0) + (inda < 43 ? 1 : 0)) << 8
                          | (long)Rcp[inda];
                long kL = (long)(((bHi >> 10) * (UInt64)rcpx) >> 41);
                if (Misc.Likely(dn < 64)) kL >>= dn; else kL = 0;

                int isct = Ind[(int)kL];
                if (dn < 64 && (aHi >> 15) * Tn[isct + 1] < (bHi >> dn)) isct++;

                UInt128 kn, kd;

                if (isct == 0) {
                    kn = bVal << 15;
                    kd = aVal << 15;
                } else {
                    if (Misc.Unlikely(isct == 64)) isct = 63;
                    kn = (bVal << 15) - ((aVal * Tn[isct]) << dn);
                    if (Misc.Unlikely((kn >> 127) != 0))
                        kn = (bVal << 15) - ((aVal * Tn[--isct]) << dn);

                    UInt64 knh = (UInt64)(kn >> 64);
                    UInt64 knl = (UInt64)kn;
                    int nzn = kn != 0
                        ? (Misc.Likely(knh != 0) ? (int)BitOperations.LeadingZeroCount(knh)
                                                 : 64 + (int)BitOperations.LeadingZeroCount(knl))
                        : 0;
                    kn <<= nzn;

                    int sl = 15 + dn, sr = 49 - dn;
                    InlineArray3<UInt64> kd3 = default;
                    kd3[2] = aHi >> sr;
                    kd3[1] = (aLo >> sr) | (aHi << sl);
                    kd3[0] = aLo << sl;

                    bVal *= Tn[isct];
                    bHi = (UInt64)(bVal >> 64);
                    bLo = (UInt64)bVal;
                    AddUnchecked(out kd3, in kd3, bVal);

                    UInt64 kdh = kd3[2];
                    int nzd, md;
                    if (kdh != 0) {
                        nzd = (int)BitOperations.LeadingZeroCount(kdh);
                        md = 64 - nzd;
                        UInt64 hi = (kdh << nzd) | (kd3[1] >> md);
                        UInt64 lo = (kd3[1] << nzd) | (kd3[0] >> md);
                        kd = ((UInt128)hi << 64) | lo;
                    } else {
                        kdh = kd3[1];
                        nzd = (int)BitOperations.LeadingZeroCount(kdh);
                        md = (~nzd) & 63;
                        UInt64 hi = (kdh << nzd) | (kd3[0] >> 1 >> md);
                        UInt64 lo = kd3[0] << nzd;
                        kd = ((UInt128)hi << 64) | lo;
                        nzd += 64;
                    }
                    dn = nzn - nzd + 64;
                }

                UInt128 R = ReciprocalU(kd);
                UInt128 T = MultiplyHighApproximate(kn, R);
                UInt128 T2 = SquareHighApproximate(T);
                int dn2 = 2 * dn - 12;
                if (Misc.Likely(dn2 < 128)) T2 >>= dn2; else T2 = 0;

                // Polynomial: f = T - T·T2·P(T2), P uses c[5..1]
                UInt64 t2h = (UInt64)(T2 >> 64);
                UInt64 fl = CAtan2Core_flat[8 * 2 + 0];
                fl = CAtan2Core_flat[7 * 2 + 0] - MultiplyHigh(t2h, fl);
                fl = CAtan2Core_flat[6 * 2 + 0] - MultiplyHigh(t2h, fl);

                UInt128 f = ((UInt128)CAtan2Core_flat[5 * 2 + 1] << 64)
                          | (CAtan2Core_flat[5 * 2 + 0] - t2h - MultiplyHigh(t2h, fl));
                for (int ii = 4; ii >= 1; ii--) {
                    UInt128 ci = ((UInt128)CAtan2Core_flat[ii * 2 + 1] << 64)
                               | CAtan2Core_flat[ii * 2 + 0];
                    f = ci - MultiplyHighApproximate(T2, f);
                }
                f = MultiplyHighApproximate(T2, f);
                f = MultiplyHighApproximate(T, f);
                f = T - f;

                UInt128 v;
                UInt64 rnd;

                if (Misc.Likely(((Int64)isct | g) != 0)) {
                    // Path through 3-word fixup + phase offset
                    InlineArray3<UInt64> f3 = default;
                    f3[0] = 0;
                    f3[1] = (UInt64)f;
                    f3[2] = (UInt64)(f >> 64);

                    dn++;
                    if (Misc.Likely(dn < 64)) {
                        f3[0] = f3[1] << (64 - dn);
                        f3[1] = (f3[1] >> dn) | (f3[2] << (64 - dn));
                        f3[2] = f3[2] >> dn;
                    } else {
                        ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref f3[0], 3), dn);
                    }

                    AddUnchecked(out f3, in f3, in Unsafe.As<UInt64, InlineArray3<UInt64>>(
                        ref Unsafe.AsRef(in Phi0[isct][3])));

                    // msk = g ^ xsgn, with xsgn = 0  ⇒  msk = g
                    Int64 msk = g;
                    f3[0] ^= (UInt64)msk;
                    f3[1] ^= (UInt64)msk;
                    f3[2] ^= (UInt64)msk;

                    // qidx = (g != 0 ? 1 : 0) + ((g == 0 && xsgn != 0) ? 2 : 0)
                    //      = (g != 0 ? 1 : 0)
                    int qidx = (g != 0) ? 1 : 0;
                    AddUnchecked(out f3, in f3, in Unsafe.As<UInt64, InlineArray3<UInt64>>(
                        ref Unsafe.AsRef(in Qoff[qidx][3])));

                    int kk = (int)BitOperations.LeadingZeroCount(f3[2]);
                    rnd = (f3[1] >> (14 - kk)) & 1;

                    UInt128 t = ((UInt128)f3[1] << 64) | f3[0];
                    const UInt64 eps = 0xCA2339C0EBEDFA4UL;
                    t += eps;
                    UInt64 th = (UInt64)(t >> 64);
                    UInt64 tl = (UInt64)t;
                    th &= (1UL << (15 - kk)) - 1;
                    th ^= (UInt64)(IsNearest(rm) ? 1UL : 0UL) << (14 - kk);
                    if (th == 0 && tl < 0x194467381D7DBF48UL)
                        return AsAtanAccurate(y, rm);

                    xn = 0x3FFF - kk;
                    UInt64 vHi = f3[2] >> (15 - kk);
                    UInt64 vLo = (f3[1] >> (15 - kk)) | (f3[2] << (49 + kk));
                    v = ((UInt128)vHi << 64) | vLo;
                } else {
                    // isct == 0 and g == 0  →  |y| ≤ 1, tiny sector
                    v = f;
                    UInt64 vHi = (UInt64)(v >> 64);
                    UInt64 vLo = (UInt64)v;
                    int kk = (int)BitOperations.LeadingZeroCount(vHi);
                    xn = 0x3FFE - dn - kk;
                    if (xn > 0) {
                        UInt64 tl = (vLo + 6) & (~0UL >> (49 + kk));
                        tl ^= (UInt64)(IsNearest(rm) ? 1UL : 0UL) << (14 - kk);
                        if (Misc.Unlikely(tl <= 15)) return AsAtanAccurate(y, rm);
                        rnd = (vLo >> (14 - kk)) & 1;
                        vLo = (vLo >> (15 - kk)) | (vHi << (49 + kk));
                        vHi = vHi >> (15 - kk);
                        v = ((UInt128)vHi << 64) | vLo;
                    } else {
                        xn = 0;
                        kk = 15 - 0x3FFE + dn;
                        if (kk < 128) {
                            UInt128 t = (v + 6) & (((UInt128)1 << kk) - 1);
                            t ^= (UInt128)(IsNearest(rm) ? 1UL : 0UL) << (kk - 1);
                            if (Misc.Unlikely(t <= 15)) return AsAtanAccurate(y, rm);
                            rnd = (UInt64)((v >> (kk - 1)) & 1);
                            v >>= kk;
                        } else {
                            rnd = (kk == 128) ? (UInt64)(v >> 127) : 0;
                            v = 0;
                        }
                    }
                }

                if (Misc.Unlikely(!IsNearest(rm))) {
                    bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                    bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                    UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                    UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                    rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                }

                UInt128 dv = ((UInt128)((UInt64)xn << 48) << 64) | rnd;
                v += dv;

                UInt64 vHiF = (UInt64)(v >> 64);
                if (vHiF < (1UL << 48))
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);

                vHiF |= ysgn;
                v = ((UInt128)vHiF << 64) | (UInt64)v;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }

    }

    partial class Binary128Arithmetic {

        // ------------------------------------------------------------------
        // AsAtanAccurate — AsAtan2Accurate specialized with x = +1.0.
        //
        // Simplifications applied:
        //   * xsgn ≡ 0, xAbs = One.  Consequently:
        //       - g = -1 ⇔ |y| > 1;   g = 0 ⇔ |y| ≤ 1.
        //       - a = max(One, |y|), b = min(One, |y|).
        //       - The inner `if (xn == 0)` renormalization never fires.
        //       - qidx  = (g != 0) ? 1 : 0.
        //       - mask  = g ^ -(Int64)xsgn = g.
        //       - The `(isct | g | xsgn) != 0` dispatch becomes (isct | g).
        //   * x = One is never NaN/Inf, so the early `yn == 0` path can
        //     never itself recurse into specials.
        // ------------------------------------------------------------------
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        static UInt128 AsAtanAccurate(UInt128 y, MidpointRounding rm) {
            unchecked {
                const UInt64 smsk = 1UL << 63;

                UInt64 yHi = (UInt64)(y >> 64), yLo = (UInt64)y;

                UInt64 ysgn = yHi & smsk;      // 0 or 0x8000_0000_0000_0000
                yHi &= ~smsk;

                UInt128 yAbs = ((UInt128)yHi << 64) | yLo;

                // g = -1 ⇔ |y| > One;   aVal = max(One, |y|);  bVal = min(One, |y|).
                Int128 dab = unchecked((Int128)ONE) - unchecked((Int128)yAbs);
                Int64 g = (Int64)(dab >> 127);       // 0 or -1
                dab &= (Int128)g;
                UInt128 aVal = ONE - unchecked((UInt128)dab);
                UInt128 bVal = yAbs + unchecked((UInt128)dab);

                UInt64 aHi = (UInt64)(aVal >> 64), aLo = (UInt64)aVal;
                UInt64 bHi = (UInt64)(bVal >> 64), bLo = (UInt64)bVal;

                int xn = (int)(aHi >> 48);
                int yn = (int)(bHi >> 48);

                // Only `b` (== |y|, only possible when g == 0) can be subnormal.
                // When g == -1, b == One, so this block is skipped entirely.
                if (Misc.Unlikely(yn == 0)) {
                    int ns = -15;
                    if (bHi != 0) ns += (int)BitOperations.LeadingZeroCount(bHi);
                    else ns += (int)BitOperations.LeadingZeroCount(bLo) + 64;
                    yn = 1 - ns;
                    bLo = DoubleArithmetic.ShiftLeft(bLo, bHi, ns, out bHi);
                    bVal = ((UInt128)bHi << 64) | bLo;
                    // a == One here, so xn == 0x3FFF ≠ 0 — no renormalization.
                }

                int dn = xn - yn;
                aHi = (aHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                bHi = (bHi & 0x0000FFFFFFFFFFFFUL) | (1UL << 48);
                aVal = ((UInt128)aHi << 64) | aLo;
                bVal = ((UInt128)bHi << 64) | bLo;

                int inda = (int)((aHi >> 41) & 127);
                long rcpx = (2L + (inda == 0 ? 1 : 0) + (inda < 43 ? 1 : 0)) << 8
                          | (long)Rcp[inda];
                long kL = (long)(((bHi >> 10) * (UInt64)rcpx) >> 41);
                if (Misc.Likely(dn < 64)) kL >>= dn; else kL = 0;

                int isct = Ind[(int)kL];
                if (dn < 64 && (aHi >> 15) * Tn[isct + 1] < (bHi >> dn)) isct++;

                UInt128 kn;
                InlineArray3<UInt64> kd = default;

                if (isct == 0) {
                    kn = bVal << 15;
                    aVal <<= 15;
                    aHi = (UInt64)(aVal >> 64);
                    aLo = (UInt64)aVal;
                    kd[2] = aHi;
                    kd[1] = aLo;
                    kd[0] = 0;
                } else {
                    if (isct == 64) isct = 63;
                    kn = (bVal << 15) - ((aVal * Tn[isct]) << dn);
                    if (Misc.Unlikely((kn >> 127) != 0))
                        kn = (bVal << 15) - ((aVal * Tn[--isct]) << dn);

                    UInt64 knh = (UInt64)(kn >> 64);
                    UInt64 knl = (UInt64)kn;
                    int nzn = kn != 0
                        ? (knh != 0 ? (int)BitOperations.LeadingZeroCount(knh)
                                    : 64 + (int)BitOperations.LeadingZeroCount(knl))
                        : 0;
                    kn <<= nzn;

                    int sl = 15 + dn, sr = 64 - sl;
                    kd[2] = aHi >> sr;
                    kd[1] = (aLo >> sr) | (aHi << sl);
                    kd[0] = aLo << sl;

                    bVal *= Tn[isct];
                    AddUnchecked(out kd, in kd, bVal);

                    int nzd = 0;
                    if (kd[2] == 0) { kd[2] = kd[1]; kd[1] = kd[0]; kd[0] = 0; nzd = 64; }
                    sl = (int)BitOperations.LeadingZeroCount(kd[2]);
                    sr = (~sl) & 63;
                    nzd += sl;
                    kd[2] = (kd[2] << sl) | (kd[1] >> 1 >> sr);
                    kd[1] = (kd[1] << sl) | (kd[0] >> 1 >> sr);
                    kd[0] = kd[0] << sl;
                    dn = nzn - nzd + 64;
                }

                UInt128 R = ReciprocalU(((UInt128)kd[2] << 64) | kd[1]);

                InlineArray6<UInt64> r = default;
                r[4] = (UInt64)R;
                r[5] = (UInt64)(R >> 64);

                InlineArray2<UInt64> rTop = default;
                rTop[0] = r[4]; rTop[1] = r[5];
                InlineArray5<UInt64> H;
                BigMulUnsigned(out H, in rTop, in kd);

                InlineArray3<UInt64> sH = default;
                sH[0] = (H[0] >> 58) | (H[1] << 6);
                sH[1] = (H[1] >> 58) | (H[2] << 6);
                sH[2] = (H[2] >> 58) | (H[3] << 6);

                InlineArray6<UInt64> H2;
                SquareSigned(out H2, in sH);

                ShiftLeftUnsignedFull(MemoryMarshal.CreateSpan(ref H[0], 5), 75);

                nuint cc;
                H[0] = SubtractWithBorrow(H[0], H2[2], 0, out cc);
                H[1] = SubtractWithBorrow(H[1], H2[3], cc, out cc);
                H[2] = SubtractWithBorrow(H[2], H2[4], cc, out cc);
                H[3] = SubtractWithBorrow(H[3], 0, cc, out cc);
                H[4] = SubtractWithBorrow(H[4], 0, cc, out cc);

                InlineArray7<UInt64> dR = default;
                BigMulUnsigned(out dR, in H, in rTop);

                if ((H[4] >> 63) != 0) {
                    dR[5] = SubtractWithBorrow(dR[5], r[4], 0, out cc);
                    dR[6] = SubtractWithBorrow(dR[6], r[5], cc, out cc);
                }

                ShiftRightSignedFull(MemoryMarshal.CreateSpan(ref dR[0], 7), 10);

                r[0] = SubtractWithBorrow(r[0], dR[2], 0, out cc);
                r[1] = SubtractWithBorrow(r[1], dR[3], cc, out cc);
                r[2] = SubtractWithBorrow(r[2], dR[4], cc, out cc);
                r[3] = SubtractWithBorrow(r[3], dR[5], cc, out cc);
                r[4] = SubtractWithBorrow(r[4], dR[6], cc, out cc);
                r[5] = SubtractWithBorrow(r[5], (UInt64)((Int64)dR[6] >> 63), cc, out cc);

                InlineArray2<UInt64> nn = default;
                nn[0] = (UInt64)kn;
                nn[1] = (UInt64)(kn >> 64);
                InlineArray8<UInt64> t = default;
                MultiplyUnsigned(out t, in r, in nn);

                InlineArray6<UInt64> tLo = default;
                tLo[0] = t[2]; tLo[1] = t[3]; tLo[2] = t[4];
                tLo[3] = t[5]; tLo[4] = t[6]; tLo[5] = t[7];
                InlineArray6<UInt64> t2 = default;
                SquareHighUnsignedApproximate(out t2, in tLo);
                int dn2 = 2 * dn - 14;
                ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref t2[0], 6), dn2);

                ReadOnlySpan<UInt64> cp = CpAtan2Acc;
                InlineArray6<UInt64> fp = default;

                int ck = 0;
                fp[0] = cp[0];
                fp[1] = cp[1];

                // 2-word phase
                for (int i = 25; i > 20; i--) {
                    ck += 2;
                    ref InlineArray2<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, fp_, Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in t2[4])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray2<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[2] = cp[ck + 2];

                // 3-word phase
                for (int i = 20; i > 15; i--) {
                    ck += 3;
                    ref InlineArray3<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in t2[3])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray3<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[3] = cp[ck + 3];

                // 4-word phase
                for (int i = 15; i > 10; i--) {
                    ck += 4;
                    ref InlineArray4<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in t2[2])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray4<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[4] = cp[ck + 4];

                // 5-word phase
                for (int i = 10; i > 5; i--) {
                    ck += 5;
                    ref InlineArray5<UInt64> fp_ = ref Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in fp[0]));
                    MultiplyHighUnsignedApproximate(out fp_, in fp_, Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in t2[1])));
                    SubtractUnchecked(out fp_, Unsafe.As<UInt64, InlineArray5<UInt64>>(ref Unsafe.AsRef(in cp[ck])), fp_);
                }
                fp[5] = cp[ck + 5];

                // 6-word phase
                for (int i = 5; i > 1; i--) {
                    ck += 6;
                    MultiplyHighUnsignedApproximate(out fp, in fp, in t2);
                    SubtractUnchecked(out fp, in Unsafe.As<UInt64, InlineArray6<UInt64>>(ref Unsafe.AsRef(in cp[ck])), in fp);
                }
                MultiplyHighUnsignedApproximate(out fp, in fp, in t2);
                MultiplyHighUnsignedApproximate(out fp, in tLo, in fp);
                SubtractUnchecked(out fp, in tLo, in fp);

                // xsgn == 0  ⇒  dispatch on (isct | g).
                if (((Int64)isct | g) != 0) {
                    ShiftRightUnsignedFull(MemoryMarshal.CreateSpan(ref fp[0], 6), dn + 1);
                    AddUnchecked(out fp, in Phi0[isct], in fp);
                    // mask = g ^ -(Int64)xsgn = g ^ 0 = g.
                    Int64 msk = g;
                    fp[0] ^= (UInt64)msk;
                    fp[1] ^= (UInt64)msk;
                    fp[2] ^= (UInt64)msk;
                    fp[3] ^= (UInt64)msk;
                    fp[4] ^= (UInt64)msk;
                    fp[5] ^= (UInt64)msk;
                    // qidx = (g != 0 ? 1 : 0) + 0.
                    int qidx = (g != 0) ? 1 : 0;
                    AddUnchecked(out fp, in Qoff[qidx], in fp);
                    dn = -1;
                }

                UInt128 v = U128(fp[5], fp[4]);
                System.Diagnostics.Debug.Assert(BitOperations.LeadingZeroCount(fp[5]) == LeadingZeroCount(v));
                int kk = (int)BitOperations.LeadingZeroCount(fp[5]);
                v <<= kk;
                xn = 0x3FFE - dn - kk;

                UInt64 rnd;
                if (xn > 0) {
                    rnd = ((nuint)v >> 14) & 1;
                    v >>= 15;
                } else {
                    kk = 15 - xn;
                    if (kk < 128) {
                        rnd = (UInt64)((v >> (kk - 1)) & 1);
                        v >>= kk;
                    } else {
                        rnd = (kk == 128) ? (UInt64)(v >> 127) : 0;
                        v = 0;
                    }
                    xn = 0;
                }

                if (Misc.Unlikely(!IsNearest(rm))) {
                    bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                    bool isDown = rm == MidpointRounding.ToNegativeInfinity;
                    UInt64 incUp = (ysgn == 0) ? 1UL : 0UL;
                    UInt64 incDown = (ysgn != 0) ? 1UL : 0UL;
                    rnd = incUp * (isUp ? 1UL : 0UL) + incDown * (isDown ? 1UL : 0UL);
                }

                UInt128 dv = ((UInt128)((UInt64)xn << 48) << 64) | rnd;
                v += dv;

                UInt64 vHiF = (UInt64)(v >> 64);
                if (vHiF < (1UL << 48))
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Underflow);

                vHiF |= ysgn;
                v = ((UInt128)vHiF << 64) | (UInt64)v;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                return v;
            }
        }
    }
}