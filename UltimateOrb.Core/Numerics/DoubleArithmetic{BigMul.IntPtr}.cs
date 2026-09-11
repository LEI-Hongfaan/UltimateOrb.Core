using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

#if !STANDALONE_XINTN_LIBRARY
using static UltimateOrb.Utilities.UnsafeParameterHelpers;

namespace UltimateOrb.Numerics {

    partial class DoubleArithmetic {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint BigMul(nuint first, nuint second, out nuint highResult) {
            unchecked {
                if (Unsafe.SizeOf<nuint>() == Unsafe.SizeOf<uint>()) {
                    var prod = (ulong)first * (ulong)second;
                    UnsafeAsForOut<nuint, uint>(out highResult) = (uint)(prod >> (8 * Unsafe.SizeOf<uint>()));
                    return (uint)prod;
                } else if (Unsafe.SizeOf<nuint>() == Unsafe.SizeOf<ulong>()) {
                    return (nuint)BigMul(first, second, out UnsafeAsForOut<nuint, ulong>(out highResult));
                } else {
                    return BigMulUnsignedNaive(first, second, out highResult);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint BigMul(nint first, nint second, out nint highResult) {
            unchecked {
                if (Unsafe.SizeOf<nint>() == Unsafe.SizeOf<int>()) {
                    var prod = (long)first * (long)second;
                    UnsafeAsForOut<nint, int>(out highResult) = (int)(prod >> (8 * Unsafe.SizeOf<int>()));
                    return (int)prod;
                } else if (Unsafe.SizeOf<nuint>() == Unsafe.SizeOf<ulong>()) {
                    return (nint)BigMul(first, second, out UnsafeAsForOut<nint, long>(out highResult));
                } else {
                    return (nint)BigMulUnsignedNaive((nuint)first, (nuint)second, out UnsafeAsForOut<nint, nuint>(out highResult));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T BigMulUnsignedNaive<T>(T left, T right, out T highResult)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            int w = Cached<T>.BitWidthOfT;
            T zero = Cached<T>.Zero;
            T one = Cached<T>.One;

            if ((w & 1) == 0) {
                // ---------- even bit width ----------
                int h = w / 2;
                T mask = (one << h) - one;

                T a_lo = left & mask;
                T a_hi = left >> h;
                T b_lo = right & mask;
                T b_hi = right >> h;

                T p0 = a_lo * b_lo;
                T p1 = a_lo * b_hi;
                T p2 = a_hi * b_lo;
                T p3 = a_hi * b_hi;

                T cross_lo = p1 + p2;
                T cross_hi = cross_lo < p1 ? one : zero;

                T cross_lo_lo = cross_lo & mask;
                T cross_lo_hi = cross_lo >> h;

                T low_sum = p0 + (cross_lo_lo << h);
                T carry_from_low = low_sum < p0 ? one : zero;

                highResult = Sum4(p3, cross_lo_hi, cross_hi << h, carry_from_low, out _);
                return low_sum;
            } else {
                // ---------- odd bit width ----------
                int h = w / 2; // floor
                T low_mask = (one << h) - one;

                T a_lo = left & low_mask;
                T a_hi = left >> h;          // h + 1 bits
                T b_lo = right & low_mask;
                T b_hi = right >> h;         // h + 1 bits

                T p0 = a_lo * b_lo;
                T p1 = a_lo * b_hi;
                T p2 = a_hi * b_lo;
                T p3 = a_hi * b_hi;

                T cross_lo = p1 + p2;        // low w bits of cross
                T cross_hi = cross_lo < p1 ? one : zero; // 0 or 1

                T C_lo = cross_lo;
                T C_hi = cross_hi;

                T C_lo_low = C_lo & ((one << (h + 1)) - one);
                T cl = C_lo_low & low_mask;
                T ch = C_lo_low >> h;        // 1 bit

                T extra = ch + (p3 & one);   // 0, 1, or 2

                T base_val = p0 + (cl << h);

                T low;
                T carry_out;

                if (extra == zero) {
                    low = base_val;
                    carry_out = zero;
                } else if (extra == one) {
                    T bit = one << (w - 1);
                    T t = base_val + bit;
                    low = t;
                    carry_out = t < base_val ? one : zero;
                } else // extra == 2
                  {
                    low = base_val;
                    carry_out = one;
                }

                T C_lo_high = C_lo >> (h + 1);
                highResult = Sum4(carry_out, C_lo_high, C_hi << h, p3 >> 1, out T _);
                return low;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint BigMulUnsigned(nuint first, nuint second, out nuint highResult) {
            return BigMul(first, second, out highResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint BigMulUnsigned(nint first, nint second, out nint highResult) {
            return unchecked((nint)BigMul((nuint)first, (nuint)second, out UnsafeAsForOut<nint, nuint>(out highResult)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint BigMulSigned(nuint first, nuint second, out nuint highResult) {
            return unchecked((nuint)BigMul((nint)first, (nint)second, out UnsafeAsForOut<nuint, nint>(out highResult)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint BigMulSigned(nint first, nint second, out nint highResult) {
            return BigMul(first, second, out highResult);
        }
    }
}
#endif