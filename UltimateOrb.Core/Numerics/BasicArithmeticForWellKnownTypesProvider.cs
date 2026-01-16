using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.UnsafeParameterHelpers;

namespace UltimateOrb.Numerics {
    [Experimental("UoWIP_GenericMath")]
    public readonly partial struct BasicArithmeticForWellKnownTypesProvider :
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, uint>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, int>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, ulong>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, long>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt128>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int128>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>,
        IBinaryIntegerBigMulUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger> {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out uint result_lo, out uint result_hi, in uint first, in uint second) {
            var result = unchecked(first * (ulong)second);
            (result_lo, result_hi) = (unchecked((uint)result), unchecked((uint)(result >> 32)));
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out int result_lo, out int result_hi, in int first, in int second) {
            BigMulUnsigned(out UnsafeAsForOut<int, uint>(out result_lo), out UnsafeAsForOut<int, uint>(out result_hi),
                in UnsafeAsForIn<int, uint>(in first), in UnsafeAsForIn<int, uint>(in first));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out ulong result_lo, out ulong result_hi, in ulong first, in ulong second) {
            var lo = DoubleArithmetic.BigMulUnsigned(first, second, out var hi);
            (result_lo, result_hi) = (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out long result_lo, out long result_hi, in long first, in long second) {
            BigMulUnsigned(out UnsafeAsForOut<long, ulong>(out result_lo), out UnsafeAsForOut<long, ulong>(out result_hi),
                in UnsafeAsForIn<long, ulong>(in first), in UnsafeAsForIn<long, ulong>(in first));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out System.UInt128 result_lo, out System.UInt128 result_hi, in System.UInt128 first, in System.UInt128 second) {
            var lo = DoubleArithmetic.BigMulUnsigned(first, second, out var hi);
            (result_lo, result_hi) = (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out System.Int128 result_lo, out System.Int128 result_hi, in System.Int128 first, in System.Int128 second) {
            BigMulUnsigned(out UnsafeAsForOut<System.Int128, System.UInt128>(out result_lo), out UnsafeAsForOut<System.Int128, System.UInt128>(out result_hi),
                in UnsafeAsForIn<System.Int128, System.UInt128>(in first), in UnsafeAsForIn<System.Int128, System.UInt128>(in first));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out UInt128 result_lo, out UInt128 result_hi, in UInt128 first, in UInt128 second) {
            var lo = DoubleArithmetic.BigMulUnsigned(first, second, out var hi);
            (result_lo, result_hi) = (lo, hi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BigMulUnsigned(out Int128 result_lo, out Int128 result_hi, in Int128 first, in Int128 second) {
            BigMulUnsigned(out UnsafeAsForOut<Int128, UInt128>(out result_lo), out UnsafeAsForOut<Int128, UInt128>(out result_hi),
                in UnsafeAsForIn<Int128, UInt128>(in first), in UnsafeAsForIn<Int128, UInt128>(in first));
        }

        public static void BigMulUnsigned(out BigInteger result_lo, out BigInteger result_hi, in BigInteger first, in BigInteger second) {
            result_lo = first * second;
            result_hi = 0 > result_lo.Sign ? BigInteger.MinusOne : BigInteger.Zero;
        }
    }
}

namespace UltimateOrb.Numerics.Generic {
    using static UnsafeParameterHelpers;

    [Experimental("UoWIP_GenericMath")]
    public readonly partial struct DoubleLongArithmeticProvider<TInt, TIntArithmeticProvider, TLong, TDoubleLongDataProvider> :
        IBinaryIntegerBigMulUnsignedDoubleLongProvider<DoubleLongArithmeticProvider<TInt, TIntArithmeticProvider, TLong, TDoubleLongDataProvider>, TInt, TIntArithmeticProvider, TLong, TDoubleLongDataProvider>
        where TIntArithmeticProvider :
            IBinaryIntegerBigMulUnsignedProvider<TIntArithmeticProvider, TInt> {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBigMulUnsignedDoubleLongProvider<TSelf, TInt, TIntArithmeticProvider, TLong, TDoubleLongDataProvider> :
        IBinaryIntegerBigMulUnsignedProvider<TSelf, TLong>
        where TSelf :
            IBinaryIntegerBigMulUnsignedDoubleLongProvider<TSelf, TInt, TIntArithmeticProvider, TLong, TDoubleLongDataProvider>
        where TIntArithmeticProvider :
            IBinaryIntegerBigMulUnsignedProvider<TIntArithmeticProvider, TInt> {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static virtual void BigMulUnsigned(out TInt result_lo, out TInt result_hi, in TInt first, in TInt second) {
            TIntArithmeticProvider.BigMulUnsigned(out result_lo, out result_hi, first, second);
        }
        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static virtual void BigMulUnsigned_A_Naive(out TInt result_lo_lo, out TInt result_lo_hi, out TInt result_hi_lo, out TInt result_hi_hi, TInt first_lo, TInt first_hi, TInt second_lo, TInt second_hi) {
            TSelf.BigMulUnsigned(out var lll, out var llh, first_lo, second_lo);
            TSelf.BigMulUnsigned(out var hll, out var hlh, first_hi, second_lo);
            TSelf.AddUnchecked(out hll, out hlh, hll, hlh, llh, default(ZeroT));
            TSelf.BigMulUnsigned(out var lhl, out var lhh, first_lo, second_hi);
            TSelf.AddUnchecked(out lhl, out lhh, lhl, lhh, hll, default(ZeroT));
            TSelf.BigMulUnsigned(out var hhl, out var hhh, first_hi, second_hi);
            var c = TSelf.AddUnsignedNoThrow(out var th, hlh, lhh);
            Debug.Assert(0 == c || 1 == c);
            TSelf.AddUnchecked(out hhl, out hhh, hhl, hhh, th, default(ZeroT));
            TSelf.ConditionalIncreaseUnchecked(out hhh, hhh, c);
            result_lo_lo = lll;
            result_lo_hi = lhl;
            result_hi_lo = hhl;
            result_hi_hi = hhh;
        }
        */
        static void IBinaryIntegerBigMulUnsignedProvider<TSelf, TLong>.BigMulUnsigned(out TLong result_lo, out TLong result_hi, in TLong first, in TLong second) {
            throw new NotImplementedException();
        }
    }
}