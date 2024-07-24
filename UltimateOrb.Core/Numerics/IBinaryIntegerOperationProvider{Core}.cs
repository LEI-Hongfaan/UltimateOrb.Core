#pragma warning disable IDE0049 // Simplify Names
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using static UltimateOrb.Utilities.UnsafeParameterHelpers;

namespace UltimateOrb.Numerics {

    public readonly partial struct BasicArithmeticForWellKnownTypesProvider
        : IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt32>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, Int32>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt64>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, Int64>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.UInt128>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, UltimateOrb.Int128>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, System.UInt128>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, System.Int128>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, UInt256>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, Int256>
        , IBinaryIntegerBitwiseAndProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerBitwiseOrProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerBitwiseXorProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerBitwiseAndNotProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerAddUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerAddUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerAddSignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerSubtractUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerSubtractUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerSubtractSignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerMultiplyUncheckedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerMultiplyUnsignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        , IBinaryIntegerMultiplySignedProvider<BasicArithmeticForWellKnownTypesProvider, BigInteger>
        {

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            UnsafeAsForOut<UInt32, Int32>(out result) = checked(UnsafeAsForIn<UInt32, Int32>(in first) + UnsafeAsForIn<UInt32, Int32>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            UnsafeAsForOut<UInt32, Int32>(out result) = checked(UnsafeAsForIn<UInt32, Int32>(in first) - UnsafeAsForIn<UInt32, Int32>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out UInt32 result, in UInt32 first, in UInt32 second) {
            UnsafeAsForOut<UInt32, Int32>(out result) = checked(UnsafeAsForIn<UInt32, Int32>(in first) * UnsafeAsForIn<UInt32, Int32>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out Int32 result, in Int32 first, in Int32 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out Int32 result, in Int32 first, in Int32 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out Int32 result, in Int32 first, in Int32 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out Int32 result, in Int32 first, in Int32 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out Int32 result, in Int32 first, in Int32 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out Int32 result, in Int32 first, in Int32 second) {
            UnsafeAsForOut<Int32, UInt32>(out result) = checked(UnsafeAsForIn<Int32, UInt32>(in first) + UnsafeAsForIn<Int32, UInt32>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out Int32 result, in Int32 first, in Int32 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out Int32 result, in Int32 first, in Int32 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out Int32 result, in Int32 first, in Int32 second) {
            UnsafeAsForOut<Int32, UInt32>(out result) = checked(UnsafeAsForIn<Int32, UInt32>(in first) - UnsafeAsForIn<Int32, UInt32>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out Int32 result, in Int32 first, in Int32 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out Int32 result, in Int32 first, in Int32 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out Int32 result, in Int32 first, in Int32 second) {
            UnsafeAsForOut<Int32, UInt32>(out result) = checked(UnsafeAsForIn<Int32, UInt32>(in first) * UnsafeAsForIn<Int32, UInt32>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out Int32 result, in Int32 first, in Int32 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            UnsafeAsForOut<UInt64, Int64>(out result) = checked(UnsafeAsForIn<UInt64, Int64>(in first) + UnsafeAsForIn<UInt64, Int64>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            UnsafeAsForOut<UInt64, Int64>(out result) = checked(UnsafeAsForIn<UInt64, Int64>(in first) - UnsafeAsForIn<UInt64, Int64>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out UInt64 result, in UInt64 first, in UInt64 second) {
            UnsafeAsForOut<UInt64, Int64>(out result) = checked(UnsafeAsForIn<UInt64, Int64>(in first) * UnsafeAsForIn<UInt64, Int64>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out Int64 result, in Int64 first, in Int64 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out Int64 result, in Int64 first, in Int64 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out Int64 result, in Int64 first, in Int64 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out Int64 result, in Int64 first, in Int64 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out Int64 result, in Int64 first, in Int64 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out Int64 result, in Int64 first, in Int64 second) {
            UnsafeAsForOut<Int64, UInt64>(out result) = checked(UnsafeAsForIn<Int64, UInt64>(in first) + UnsafeAsForIn<Int64, UInt64>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out Int64 result, in Int64 first, in Int64 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out Int64 result, in Int64 first, in Int64 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out Int64 result, in Int64 first, in Int64 second) {
            UnsafeAsForOut<Int64, UInt64>(out result) = checked(UnsafeAsForIn<Int64, UInt64>(in first) - UnsafeAsForIn<Int64, UInt64>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out Int64 result, in Int64 first, in Int64 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out Int64 result, in Int64 first, in Int64 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out Int64 result, in Int64 first, in Int64 second) {
            UnsafeAsForOut<Int64, UInt64>(out result) = checked(UnsafeAsForIn<Int64, UInt64>(in first) * UnsafeAsForIn<Int64, UInt64>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out Int64 result, in Int64 first, in Int64 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            UnsafeAsForOut<UltimateOrb.UInt128, UltimateOrb.Int128>(out result) = checked(UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in first) + UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            UnsafeAsForOut<UltimateOrb.UInt128, UltimateOrb.Int128>(out result) = checked(UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in first) - UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out UltimateOrb.UInt128 result, in UltimateOrb.UInt128 first, in UltimateOrb.UInt128 second) {
            UnsafeAsForOut<UltimateOrb.UInt128, UltimateOrb.Int128>(out result) = checked(UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in first) * UnsafeAsForIn<UltimateOrb.UInt128, UltimateOrb.Int128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            UnsafeAsForOut<UltimateOrb.Int128, UltimateOrb.UInt128>(out result) = checked(UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in first) + UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            UnsafeAsForOut<UltimateOrb.Int128, UltimateOrb.UInt128>(out result) = checked(UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in first) - UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            UnsafeAsForOut<UltimateOrb.Int128, UltimateOrb.UInt128>(out result) = checked(UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in first) * UnsafeAsForIn<UltimateOrb.Int128, UltimateOrb.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out UltimateOrb.Int128 result, in UltimateOrb.Int128 first, in UltimateOrb.Int128 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            UnsafeAsForOut<System.UInt128, System.Int128>(out result) = checked(UnsafeAsForIn<System.UInt128, System.Int128>(in first) + UnsafeAsForIn<System.UInt128, System.Int128>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            UnsafeAsForOut<System.UInt128, System.Int128>(out result) = checked(UnsafeAsForIn<System.UInt128, System.Int128>(in first) - UnsafeAsForIn<System.UInt128, System.Int128>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out System.UInt128 result, in System.UInt128 first, in System.UInt128 second) {
            UnsafeAsForOut<System.UInt128, System.Int128>(out result) = checked(UnsafeAsForIn<System.UInt128, System.Int128>(in first) * UnsafeAsForIn<System.UInt128, System.Int128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            UnsafeAsForOut<System.Int128, System.UInt128>(out result) = checked(UnsafeAsForIn<System.Int128, System.UInt128>(in first) + UnsafeAsForIn<System.Int128, System.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            UnsafeAsForOut<System.Int128, System.UInt128>(out result) = checked(UnsafeAsForIn<System.Int128, System.UInt128>(in first) - UnsafeAsForIn<System.Int128, System.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            UnsafeAsForOut<System.Int128, System.UInt128>(out result) = checked(UnsafeAsForIn<System.Int128, System.UInt128>(in first) * UnsafeAsForIn<System.Int128, System.UInt128>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out System.Int128 result, in System.Int128 first, in System.Int128 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            UnsafeAsForOut<UInt256, Int256>(out result) = checked(UnsafeAsForIn<UInt256, Int256>(in first) + UnsafeAsForIn<UInt256, Int256>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            UnsafeAsForOut<UInt256, Int256>(out result) = checked(UnsafeAsForIn<UInt256, Int256>(in first) - UnsafeAsForIn<UInt256, Int256>(in second));
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out UInt256 result, in UInt256 first, in UInt256 second) {
            UnsafeAsForOut<UInt256, Int256>(out result) = checked(UnsafeAsForIn<UInt256, Int256>(in first) * UnsafeAsForIn<UInt256, Int256>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out Int256 result, in Int256 first, in Int256 second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out Int256 result, in Int256 first, in Int256 second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out Int256 result, in Int256 first, in Int256 second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out Int256 result, in Int256 first, in Int256 second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out Int256 result, in Int256 first, in Int256 second) {
            result = unchecked(first + second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out Int256 result, in Int256 first, in Int256 second) {
            UnsafeAsForOut<Int256, UInt256>(out result) = checked(UnsafeAsForIn<Int256, UInt256>(in first) + UnsafeAsForIn<Int256, UInt256>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out Int256 result, in Int256 first, in Int256 second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out Int256 result, in Int256 first, in Int256 second) {
            result = unchecked(first - second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out Int256 result, in Int256 first, in Int256 second) {
            UnsafeAsForOut<Int256, UInt256>(out result) = checked(UnsafeAsForIn<Int256, UInt256>(in first) - UnsafeAsForIn<Int256, UInt256>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out Int256 result, in Int256 first, in Int256 second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out Int256 result, in Int256 first, in Int256 second) {
            result = unchecked(first * second);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out Int256 result, in Int256 first, in Int256 second) {
            UnsafeAsForOut<Int256, UInt256>(out result) = checked(UnsafeAsForIn<Int256, UInt256>(in first) * UnsafeAsForIn<Int256, UInt256>(in second));
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out Int256 result, in Int256 first, in Int256 second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAnd(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first & second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseOr(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first | second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseXor(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first ^ second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BitwiseAndNot(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first & ~second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnchecked(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddUnsigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddSigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first + second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnchecked(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractUnsigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubtractSigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first - second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnchecked(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplyUnsigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first * second;
        }

        /// <inheritdoc/>
        [CLSCompliant(false)]
        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MultiplySigned(out BigInteger result, in BigInteger first, in BigInteger second) {
            result = first * second;
        }
    }
}
#pragma warning restore IDE0049 // Simplify Names