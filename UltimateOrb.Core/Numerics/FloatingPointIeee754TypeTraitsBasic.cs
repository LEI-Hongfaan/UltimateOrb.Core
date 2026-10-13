using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Provides basic IEEE-754 floating-point traits and derived values for a floating-point type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">A floating-point type implementing <see cref="IFloatingPointIeee754{T}"/>.</typeparam>
    [Experimental("UoWIP")]
    public static class FloatingPointIeee754TypeTraitsBasic<T> where T : IFloatingPointIeee754<T>? {

        internal static T PositiveZero { get; } = -T.NegativeZero;

        /// <summary>
        /// Gets the smallest positive value greater than zero.
        /// </summary>
        /// <remarks>
        /// Computed as the bit increment of a signed zero with the sign of one:
        /// <c>T.BitIncrement(T.CopySign(-T.NegativeZero, T.One))</c>.
        /// </remarks>
        public static T Epsilon { get; } = T.BitIncrement(PositiveZero);

        internal static T PrecisionEpsilon { get; } = T.One - T.BitDecrement(T.One);

        internal static T RadixAsT { get; } = T.CreateChecked(T.Radix);

        private static T LogBApproximate(T x) => T.Radix != 2 ? (T.Radix != 10 ?
            T.Log(x, RadixAsT) : T.Log10(x)) : T.Log2(x);

        /// <summary>
        /// Gets the precision of the floating-point type expressed in units of the type.
        /// </summary>
        /// <remarks>
        /// For binary radix this is computed using base-2 logarithm; for decimal radix base-10 is used;
        /// otherwise a generic logarithm with the type radix is used.
        /// </remarks>
        public static T Precision { get; } = CheckPrecision(T.Round(-LogBApproximate(PrecisionEpsilon)));

        private static T CheckPrecision(T t) {
            if (t >= (T.One + T.One)) {
                // ok
                return t;
            }
            throw new NotSupportedException("Precision should be greater then 2");
        }

        /// <summary>
        /// Gets the smallest representable finite value (the minimum finite value).
        /// </summary>
        /// <remarks>
        /// Computed as the bit increment of negative infinity to obtain the most negative finite value.
        /// </remarks>
        public static T MinValue { get; } = T.BitIncrement(T.NegativeInfinity);

        /// <summary>
        /// Gets the largest representable finite value (the maximum finite value).
        /// </summary>
        /// <remarks>
        /// Computed as the bit decrement of positive infinity to obtain the largest finite value.
        /// </remarks>
        public static T MaxValue { get; } = T.BitDecrement(T.PositiveInfinity);

        /// <summary>
        /// Gets the minimum exponent (as a value of type <typeparamref name="T"/>) for normalized numbers.
        /// </summary>
        /// <remarks>
        /// This value is determined by finding the smallest normal value and taking its logarithm
        /// in the appropriate radix, rounded to the nearest representable value.
        /// </remarks>
        public static T MinExponent { get; } = GetMinExponent();

        private static T GetMinExponent() {
            var minNormal = Epsilon;
            for (; ; ) {
                if (T.IsNormal(minNormal)) {
                    return T.Round(LogBApproximate(minNormal));
                }
                minNormal = T.ScaleB(minNormal, 1);
            }
        }

        /// <summary>
        /// Gets the maximum exponent (as a value of type <typeparamref name="T"/>) for normalized numbers.
        /// </summary>
        /// <remarks>
        /// Computed from <see cref="MaxValue"/> by taking the rounded logarithm in the appropriate radix and subtracting one.
        /// </remarks>
        public static T MaxExponent { get; } = T.Round(LogBApproximate(MaxValue)) - T.One;

        internal static T MaxSignificandF { get; } = T.BitDecrement(RadixAsT);

        internal static T Two { get; } = T.One + T.One;

        internal static T MaxNaNPayloadF { get; } = MaxSignificandF - (T.Radix != 2 ? (RadixAsT - T.One) : T.ScaleB((RadixAsT + T.One), -1));

        internal static Int32 PrecisionM1OrMinValueAsInt32 { get; } = GetPrecisionM1OrMinValueAsInt32();

        private static Int32 GetPrecisionM1OrMinValueAsInt32() {
            try {
                return Int32.CreateChecked(unchecked(Precision - T.One));
            } catch (ArithmeticException) {
                return Int32.MinValue;
            }
        }

        internal static BigInteger GetMaxSignificandAsBigInteger() {
            return T.Radix != 2 ?
                (BigInteger.Pow(T.Radix, PrecisionM1OrMinValueAsInt32) - 1) * T.Radix + (T.Radix - 1)
                : (((BigInteger.One << PrecisionM1OrMinValueAsInt32) - 1) << 1) | 1;
        }

        internal static BigInteger GetMaxNaNPayloadAsBigInteger() {
            return GetMaxSignificandAsBigInteger() - (T.Radix != 2 ?
                (T.Radix - 1) * BigInteger.Pow(T.Radix, PrecisionM1OrMinValueAsInt32) :
                (BigInteger)3 << checked(PrecisionM1OrMinValueAsInt32 - 1));
        }

        /// <summary>
        /// Provides conversions of the floating-point traits into an unsigned integer type <typeparamref name="TInt"/>.
        /// </summary>
        /// <typeparam name="TInt">An unsigned integer type implementing <see cref="INumberBase{TInt}"/>.</typeparam>
        public static class As<TInt> where TInt : INumberBase<TInt>? {

            /// <summary>
            /// Gets the precision converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="Precision"/> value.
            /// </remarks>
            public static TInt Precision { get; } = TInt.CreateChecked(CheckPrecision(FloatingPointIeee754TypeTraitsBasic<T>.Precision));

            /// <summary>
            /// Gets the minimum exponent converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="MinExponent"/> value.
            /// </remarks>
            public static TInt MinExponent { get; } = TInt.CreateChecked(FloatingPointIeee754TypeTraitsBasic<T>.MinExponent);

            /// <summary>
            /// Gets the maximum exponent converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="MaxExponent"/> value.
            /// </remarks>
            public static TInt MaxExponent { get; } = TInt.CreateChecked(FloatingPointIeee754TypeTraitsBasic<T>.MaxExponent);
        }

        public static class BitsAs<TInt> where TInt : INumberBase<TInt>? {

            public static TInt MaxSignificand { get; } = TInt.CreateChecked(T.ScaleB(MaxSignificandF, unchecked(-checked(-PrecisionM1OrMinValueAsInt32))));

            public static TInt MaxNaNPayload { get; } = TInt.CreateChecked(T.ScaleB(MaxNaNPayloadF, unchecked(-checked(-PrecisionM1OrMinValueAsInt32))));
        }
    }

    [Experimental("UoWIP")]
    internal static partial class FloatingPointIeee754Internal<T> where T : IFloatingPointIeee754<T>? {

        public static TIntBits GetPayload<TIntBits>(T nan)
            where TIntBits : IBinaryInteger<TIntBits>? {
            if (T.IsNaN(nan)) {
                if (T.Radix == 2) {
                    return FloatingPointIeee754TypeTraitsBasic<T>.BitsAs<TIntBits>.MaxNaNPayload & GetRawSignificand<TIntBits>(nan);
                } else if (T.Radix == 10) {
                    var raw = GetRawSignificand<TIntBits>(nan);
                    return raw <= FloatingPointIeee754TypeTraitsBasic<T>.BitsAs<TIntBits>.MaxNaNPayload ? raw : TIntBits.Zero;
                } else {
                    throw new NotSupportedException();
                }
            }
            return unchecked(-TIntBits.One);
        }

        private static TIntBits GetRawSignificand<TIntBits>(T x) where TIntBits : IBinaryInteger<TIntBits>? {
            if (x is null) {
                return default!;
            }
            var byteCount = x.GetSignificandByteCount();
            byte[]? array = null;
            var buffer = byteCount > 64 ? stackalloc byte[byteCount] : (array = ArrayPool<byte>.Shared.Rent(byteCount)).AsSpan(0, byteCount);
            int bytesWritten;
            if (BitConverter.IsLittleEndian) {
                bytesWritten = x.WriteSignificandLittleEndian(buffer);
            } else {
                bytesWritten = x.WriteSignificandBigEndian(buffer);
            }
            var bytes = buffer[..bytesWritten];
            TIntBits r;
            if (BitConverter.IsLittleEndian) {
                r = TIntBits.ReadLittleEndian(bytes, isUnsigned: true);
            } else {
                r = TIntBits.ReadBigEndian(buffer, isUnsigned: true);
            }
            if (array != null) {
                ArrayPool<byte>.Shared.Return(array);
            }
            return r;
        }

        internal static T MaxSignificandI { get; } = T.ScaleB(FloatingPointIeee754TypeTraitsBasic<T>.MaxSignificandF, FloatingPointIeee754TypeTraitsBasic<T>.As<int>.Precision);

        internal static T MaxNaNPayloadI { get; } = T.ScaleB(FloatingPointIeee754TypeTraitsBasic<T>.MaxNaNPayloadF, FloatingPointIeee754TypeTraitsBasic<T>.As<int>.Precision);

        public static T GetNaNPayload(T nan) {
            if (T.IsNaN(nan)) {
                if (MaxSignificandI < T.CreateChecked(UInt32.MaxValue)) {
                    return T.CreateTruncating(GetPayload<UInt32>(nan));
                }
                if (MaxSignificandI < T.CreateChecked(UInt64.MaxValue)) {
                    return T.CreateTruncating(GetPayload<UInt64>(nan));
                }
                if (MaxSignificandI < T.CreateChecked(System.UInt128.MaxValue)) {
                    return T.CreateTruncating(GetPayload<System.UInt128>(nan));
                }
                {
                    return T.CreateTruncating(GetPayload<BigInteger>(nan));
                }
            }
            return T.NegativeOne;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T GetQuietNaNWithPayloadBinaryPartial<TUIntBits>(T payload)
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>? {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                throw new NotSupportedException();
            }
            var qNaNBits = UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<T, TUIntBits>(T.PositiveInfinity);
            qNaNBits |= qNaNBits >>> 1;
            var payloadBits = TUIntBits.CreateTruncating(payload);
            return T.CopySign(UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TUIntBits, T>(
                qNaNBits | payloadBits), T.NaN);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static T GetSignalingNaNWithPayloadBinaryPartial<TUIntBits>(T payload)
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>? {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                throw new NotSupportedException();
            }
            var sNaNBits = UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<T, TUIntBits>(T.PositiveInfinity);
            var payloadBits = TUIntBits.CreateTruncating(payload);
            return T.CopySign(UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TUIntBits, T>(
                sNaNBits | payloadBits), T.NaN);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TUIntBits GetQuietNaNWithPayloadDecimalPartial<TUIntBits>(T payload)
          where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>? {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                throw new NotSupportedException();
            }
            var qNaNBits = UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<T, TUIntBits>(T.PositiveInfinity);
            qNaNBits |= qNaNBits >>> 1;
            var payloadBits = TUIntBits.CreateTruncating(payload);
            return qNaNBits | payloadBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static TUIntBits GetSignalingNaNWithPayloadDecimalPartial<TUIntBits>(T payload)
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>? {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                throw new NotSupportedException();
            }
            var sNaNBits = UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<T, TUIntBits>(T.PositiveInfinity);
            sNaNBits |= sNaNBits >>> 2;
            var payloadBits = TUIntBits.CreateTruncating(payload);
            return sNaNBits | payloadBits;
        }

        public static T GetQuietNaNWithPayload(T payload) {
            // Spec 9.7: "non-negative floating-point integer" — by value.
            if (!T.IsFinite(payload) || !T.IsInteger(payload)) {
                return -T.NegativeZero; // +0
            }
            if (T.IsNegative(payload) && !T.IsZero(payload)) {
                return -T.NegativeZero; // +0
            }
            // Note: -0.0 passes the guards; its integer value is 0.
            if (!(payload <= MaxNaNPayloadI)) {
                return -T.NegativeZero; // +0
            }
            if (typeof(T) == typeof(double)) {
                return GetQuietNaNWithPayloadBinaryPartial<UInt64>(payload);
            } else if (typeof(T) == typeof(float)) {
                return GetQuietNaNWithPayloadBinaryPartial<UInt32>(payload);
            } else if (typeof(T) == typeof(Half)) {
                return GetQuietNaNWithPayloadBinaryPartial<UInt16>(payload);
            } else if (typeof(T) == typeof(Quadruple)) {
                return GetQuietNaNWithPayloadBinaryPartial<System.UInt128>(payload);
            }
#if NET11_0_OR_GREATER
            else if (typeof(T) == typeof(BFloat16)) {
                return GetQuietNaNWithPayloadBinaryPartial<UInt16>(payload);
            } else if (typeof(T) == typeof(System.Numerics.Decimal32)) {
                return (T)(object)System.Numerics.Decimal32.CopySign(System.Numerics.Decimal32.DecodeBinary(
                    GetQuietNaNWithPayloadDecimalPartial<UInt32>(payload)), System.Numerics.Decimal32.NaN);
            } else if (typeof(T) == typeof(System.Numerics.Decimal64)) {
                return (T)(object)System.Numerics.Decimal64.CopySign(System.Numerics.Decimal64.DecodeBinary(
                    GetQuietNaNWithPayloadDecimalPartial<UInt64>(payload)), System.Numerics.Decimal64.NaN);
            } else if (typeof(T) == typeof(System.Numerics.Decimal128)) {
                return (T)(object)System.Numerics.Decimal128.CopySign(System.Numerics.Decimal128.DecodeBinary(
                    GetQuietNaNWithPayloadDecimalPartial<System.UInt128>(payload)), System.Numerics.Decimal128.NaN);
            }
#endif
            else {
                throw new NotImplementedException();
            }
        }

        public static T GetSignalingNaNWithPayload(T payload) {
            // Spec 9.7: "non-negative floating-point integer" — by value.
            if (!T.IsFinite(payload) || !T.IsInteger(payload)) {
                return -T.NegativeZero; // +0
            }
            if (T.IsNegative(payload) && !T.IsZero(payload)) {
                return -T.NegativeZero; // +0
            }
            if (!(payload <= MaxNaNPayloadI)) {
                return -T.NegativeZero; // +0
            }
            if (typeof(T) == typeof(double)) {
                return GetSignalingNaNWithPayloadBinaryPartial<UInt64>(payload);
            } else if (typeof(T) == typeof(float)) {
                return GetSignalingNaNWithPayloadBinaryPartial<UInt32>(payload);
            } else if (typeof(T) == typeof(Half)) {
                return GetSignalingNaNWithPayloadBinaryPartial<UInt16>(payload);
            } else if (typeof(T) == typeof(Quadruple)) {
                return GetSignalingNaNWithPayloadBinaryPartial<System.UInt128>(payload);
            }
#if NET11_0_OR_GREATER
            else if (typeof(T) == typeof(BFloat16)) {
                return GetSignalingNaNWithPayloadBinaryPartial<UInt16>(payload);
            } else if (typeof(T) == typeof(System.Numerics.Decimal32)) {
                return (T)(object)System.Numerics.Decimal32.CopySign(System.Numerics.Decimal32.DecodeBinary(
                    GetSignalingNaNWithPayloadDecimalPartial<UInt32>(payload)), System.Numerics.Decimal32.NaN);
            } else if (typeof(T) == typeof(System.Numerics.Decimal64)) {
                return (T)(object)System.Numerics.Decimal64.CopySign(System.Numerics.Decimal64.DecodeBinary(
                    GetSignalingNaNWithPayloadDecimalPartial<UInt64>(payload)), System.Numerics.Decimal64.NaN);
            } else if (typeof(T) == typeof(System.Numerics.Decimal128)) {
                return (T)(object)System.Numerics.Decimal128.CopySign(System.Numerics.Decimal128.DecodeBinary(
                    GetSignalingNaNWithPayloadDecimalPartial<System.UInt128>(payload)), System.Numerics.Decimal128.NaN);
            }
#endif
            else {
                throw new NotImplementedException();
            }
        }
    }
}
