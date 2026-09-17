using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {
#pragma warning disable UoWIP

    internal static partial class BinaryFloatingPointIeee754Arithmetic {

        static partial class Constants<T> where T : IBinaryFloatingPointIeee754<T> {
            public static T Two { get; } = T.One + T.One;
            public static T Half { get; } = T.One / Two;
        }

        internal static bool IsInteger<TFloat>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat> {
            return TFloat.IsZero(value - TFloat.Truncate(value));
        }


        internal static bool IsEvenInteger<TFloat>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat> {
            return TFloat.IsZero(value - (TFloat.Truncate(value * Constants<TFloat>.Half) * Constants<TFloat>.Two));
        }

        internal static bool IsOddInteger<TFloat>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat> {
            TFloat half = value * Constants<TFloat>.Half;
            return TFloat.Abs(half - TFloat.Truncate(half)) == Constants<TFloat>.Half;
        }

        private enum IntegerKind {

            NotInteger = 0,

            Odd = 1,

            Even = 2,
        }

        internal static bool IsInteger<TFloat, TUIntBits>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            return GetIntegerKind<TFloat, TUIntBits>(value) != IntegerKind.NotInteger;
        }

        internal static bool IsEvenInteger<TFloat, TUIntBits>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            return GetIntegerKind<TFloat, TUIntBits>(value) == IntegerKind.Even;
        }

        internal static bool IsOddInteger<TFloat, TUIntBits>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            return GetIntegerKind<TFloat, TUIntBits>(value) == IntegerKind.Odd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IntegerKind GetIntegerKind<TFloat, TUIntBits>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            TUIntBits bits = UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloat, TUIntBits>(value);

            int rawExponent = GetRawExponent<TFloat, TUIntBits>(bits);

            int maxRawExponent =
                BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.MaxRawExponent;

            int exponentBias =
                BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.MaxExponent;

            int fractionBits =
                BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>
                    .TrailingSignificandFieldBitWidth;


            // NaN / infinity
            if (rawExponent == maxRawExponent) {
                return IntegerKind.NotInteger;
            }


            TUIntBits mantissa =
                bits & ((TUIntBits.One << fractionBits) - TUIntBits.One);


            // Zero
            if (rawExponent == 0) {
                return mantissa == TUIntBits.Zero
                    ? IntegerKind.Even
                    : IntegerKind.NotInteger;
            }


            int e = rawExponent - exponentBias;


            // |x| < 1
            if (e < 0) {
                return IntegerKind.NotInteger;
            }


            // Spacing >= 2. Every representable integer is even.
            if (e > fractionBits) {
                return IntegerKind.Even;
            }


            int fractionalBits = fractionBits - e;


            // Has fractional part.
            if (!LowBitsAreZero(mantissa, fractionalBits)) {
                return IntegerKind.NotInteger;
            }


            // Units bit is the hidden leading 1.
            if (fractionalBits == fractionBits) {
                return IntegerKind.Odd;
            }


            return ((mantissa >> fractionalBits) & TUIntBits.One) != TUIntBits.Zero
                ? IntegerKind.Odd
                : IntegerKind.Even;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRawExponent<TFloat, TUIntBits>(TUIntBits bits)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            int shift =
                BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>
                    .TrailingSignificandFieldBitWidth;

            int exponentBits =
                BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>
                    .ExponentFieldBitWidth;


            return int.CreateTruncating(bits >> shift)
                & ((1 << exponentBits) - 1);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool LowBitsAreZero<TUIntBits>(
            TUIntBits value,
            int count)
            where TUIntBits : unmanaged, IBinaryInteger<TUIntBits>, IUnsignedNumber<TUIntBits> {
            if (count == 0) {
                return true;
            }

            if (count >= Unsafe.SizeOf<TUIntBits>() * 8) {
                return value == TUIntBits.Zero;
            }

            return (value & ((TUIntBits.One << count) - TUIntBits.One))
                == TUIntBits.Zero;
        }
    }

#pragma warning restore UoWIP
}