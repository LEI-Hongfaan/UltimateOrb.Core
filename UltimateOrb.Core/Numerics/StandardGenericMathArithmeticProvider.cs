using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {
    using static UnsafeParameterHelpers;

    [Experimental("UoWIP_GenericMath")]
    public readonly partial struct StandardGenericMathArithmeticProvider<T> :
        IBinaryIntegerBigMulUnsignedProvider<StandardGenericMathArithmeticProvider<T>, T>
        where T :
            IBinaryInteger<T>,
            IAdditionOperators<T, T, T>,
            ISubtractionOperators<T, T, T>,
            IBitwiseOperators<T, T, T>,
            IShiftOperators<T, int, T>,
            IDecrementOperators<T>,
            IMultiplyOperators<T, T, T>,
            IMultiplicativeIdentity<T, T>,
            IAdditiveIdentity<T, T>,
            IComparisonOperators<T, T, bool> {


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void BigMulUnsigned(out T result_lo, out T result_hi, in T first, in T second) {
            if (typeof(T) == typeof(uint)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, uint>(out result_lo), out UnsafeAsForOut<T, uint>(out result_hi),
                    in UnsafeAsForIn<T, uint>(in first), in UnsafeAsForIn<T, uint>(in second));
                return;
            } else if (typeof(T) == typeof(int)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, int>(out result_lo), out UnsafeAsForOut<T, int>(out result_hi),
                    in UnsafeAsForIn<T, int>(in first), in UnsafeAsForIn<T, int>(in second));
                return;
            } else if (typeof(T) == typeof(ulong)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, ulong>(out result_lo), out UnsafeAsForOut<T, ulong>(out result_hi),
                    in UnsafeAsForIn<T, ulong>(in first), in UnsafeAsForIn<T, ulong>(in second));
                return;
            } else if (typeof(T) == typeof(long)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, long>(out result_lo), out UnsafeAsForOut<T, long>(out result_hi),
                    in UnsafeAsForIn<T, long>(in first), in UnsafeAsForIn<T, long>(in second));
                return;
            } else if (typeof(T) == typeof(UInt128)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, UInt128>(out result_lo), out UnsafeAsForOut<T, UInt128>(out result_hi),
                    in UnsafeAsForIn<T, UInt128>(in first), in UnsafeAsForIn<T, UInt128>(in second));
                return;
            } else if (typeof(T) == typeof(Int128)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, Int128>(out result_lo), out UnsafeAsForOut<T, Int128>(out result_hi),
                    in UnsafeAsForIn<T, Int128>(in first), in UnsafeAsForIn<T, Int128>(in second));
                return;
            } else if (typeof(T) == typeof(System.UInt128)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, System.UInt128>(out result_lo), out UnsafeAsForOut<T, System.UInt128>(out result_hi),
                    in UnsafeAsForIn<T, System.UInt128>(in first), in UnsafeAsForIn<T, System.UInt128>(in second));
                return;
            } else if (typeof(T) == typeof(System.Int128)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, System.Int128>(out result_lo), out UnsafeAsForOut<T, System.Int128>(out result_hi),
                    in UnsafeAsForIn<T, System.Int128>(in first), in UnsafeAsForIn<T, System.Int128>(in second));
                return;
            } else if (typeof(T) == typeof(BigInteger)) {
                BasicArithmeticForWellKnownTypesProvider.BigMulUnsigned(
                    out UnsafeAsForOut<T, BigInteger>(out result_lo), out UnsafeAsForOut<T, BigInteger>(out result_hi),
                    in UnsafeAsForIn<T, BigInteger>(in first), in UnsafeAsForIn<T, BigInteger>(in second));
                return;
            }

            if (BinaryIntegerTypeTraitHelpers.HasInfinitePrecision<T>()) {
                result_lo = first * second;
                result_hi = result_lo < T.AdditiveIdentity ? -T.MultiplicativeIdentity : T.Zero;
                return;
            }
            var bits = checked((int)BinaryIntegerTypeTraitHelpers.GetBitSize<T>());

            // Otherwise, split the operands into two halves
            var half = bits / 2;
            var mask = ~T.AdditiveIdentity >>> half;

            var x1 = first >>> half;
            var x0 = first & mask;
            var y1 = second >>> half;
            var y0 = second & mask;

            // Compute the subproducts
            var z2 = x1 * y1;
            var z0 = x0 * y0;
            var xSum = x1 + x0;
            var ySum = y1 + y0;
            var z1 = xSum * ySum - z2 - z0;

            // Split z1 into two halves
            var t1 = z1 >>> half;
            var t0 = z1 << half;

            // Combine the subproducts to get the final product
            var q0 = z0 + t0;
            var q1 = z2 + t1 + (q0 < t0 ? T.MultiplicativeIdentity : T.AdditiveIdentity) + (((xSum + ySum) >>> 1) - t1 & mask << half);
            result_lo = q0;
            result_hi = q1;
        }
    }
}
