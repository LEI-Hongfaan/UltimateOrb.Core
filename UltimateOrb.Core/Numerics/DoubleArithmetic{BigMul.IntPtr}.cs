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
                }
                {
                    return (nuint)BigMul(first, second, out UnsafeAsForOut<nuint, ulong>(out highResult));
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
                }
                {
                    return (nint)BigMul(first, second, out UnsafeAsForOut<nint, long>(out highResult));
                }
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