using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;
using static UltimateOrb.Numerics.FloatingPointIeee754ArithmeticInternal;

namespace UltimateOrb.Numerics {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

    internal static partial class FloatingPointIeee754ArithmeticInternal {
        internal static TDecimal ConvertIeee754BinaryToDecimal<TBinary, TBinaryUIntBits, TDecimal>(TBinary value)
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>?
            where TBinaryUIntBits : IBinaryInteger<TBinaryUIntBits>?, IMinMaxValue<TBinaryUIntBits>?
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {

            // ---- 1. Special values ---------------------------------------------------------
            if (TBinary.IsNaN(value)) return TDecimal.NaN;
            if (TBinary.IsPositiveInfinity(value)) return TDecimal.PositiveInfinity;
            if (TBinary.IsNegativeInfinity(value)) return TDecimal.NegativeInfinity;
            if (TBinary.IsZero(value)) return TBinary.IsNegative(value) ? TDecimal.NegativeZero : -TDecimal.NegativeZero;

            var negative = TBinary.IsNegative(value);

            // ---- 2. Decompose |value| = m · 2^exponent -------------------------------------------
            var m = GetSignificand<TBinary, TBinaryUIntBits>(value);

            var e2 = GetExponent<TBinary, int>(value) - (FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.Precision - 1);

            // ---- 3a. Fast path -------------------------------------------------------------
            if (TryToDecimalFastPath<TBinary, TBinaryUIntBits, TDecimal>(m, e2, out var result)) {
                return negative ? -result : result;
            }

            // ---- 3b. Slow path -------------------------------------------------------------
            return ToDecimalSlowPath<TBinary, TDecimal>(value);
        }

        private static bool TryToDecimalFastPath<TBinary, TBinaryUIntBits, TDecimal>(TBinaryUIntBits significand, int exponent, out TDecimal result)
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>?
            where TBinaryUIntBits : IBinaryInteger<TBinaryUIntBits>?, IMinMaxValue<TBinaryUIntBits>?
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {
            // the fast path try its best to express both significand and (2**exponent or 2**-exponent) in T exactly,
            // then compute the correct-rounding result by significand * scaling or significand / scaling
            // the method may adjust significand and/or exponent (but keep the value the same)


            // The denominator/numerator 2^|e| must itself be exactly
            // representable in T.
            var absExponent = unchecked((uint)Math.Abs((long)exponent));

            if (absExponent > ToDecimalTypeTraits<TBinary, TDecimal>.MaxExactPowerOfTwoExponent.ToUnsignedUnchecked() ||
                (!ToDecimalTypeTraits<TBinary, TDecimal>.SkipSignificandBoundCheck && !(significand <= ToDecimalTypeTraits<TBinary, TBinaryUIntBits, TDecimal>.MaxSignificandAsUIntBits))) {

                // try adjust significand and/or exponent before giving up.

                // For,example, canonicalize m * 2^e by removing all factors of two from m if significand too big

                //var ss = int.CreateChecked(TUInt.TrailingZeroCount(significand));
                //significand >>= ss;
                //exponent = int.CreateChecked((long)exponent + ss);

                // there are other adjustments can be done before giving up.

                // Note: be careful if you do something like significand *= 5 ** k. also consider do it with ToDecimalFromIntegerUnsigned(significand)

                result = default!;
                return false;
            }

            TDecimal decimalSignificand = ToDecimalFromIntegerUnsigned<TDecimal, TBinaryUIntBits>(significand);

            var scale = Exp2<TDecimal>(absExponent);

            // Exactly represented operands; only the final operation rounds.
            result = exponent >= 0
                ? decimalSignificand * scale
                : decimalSignificand / scale;

            return true;
        }
        internal static partial class ToDecimalFromIntegerUnsignedTypeTraits<TDecimal, TUInt>
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            where TUInt : IBinaryInteger<TUInt>?, IMinMaxValue<TUInt>? {

            public static bool IsDirectConversionSupported { get; } = GetIsDirectConversionSupported();

            private static bool GetIsDirectConversionSupported() {
                try {
                    _ = TDecimal.CreateChecked(TUInt.MaxValue);
                    return true;
                } catch (NotSupportedException) {
                    return false;
                }
            }

            internal static TDecimal Exp5Of64 { get; } = Exp5<TDecimal>(64);
        }

        internal static TDecimal ToDecimalFromIntegerUnsigned<TDecimal, TUInt>(TUInt significand)
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            where TUInt : IBinaryInteger<TUInt>?, IMinMaxValue<TUInt>? {
            // TUInt may be a larger type that does not have direct conversion from/to TDecimal at current stage.
            // For example UInt256 and Decimal128
            if (ToDecimalFromIntegerUnsignedTypeTraits<TDecimal, TUInt>.IsDirectConversionSupported) {
                return TDecimal.CreateChecked(significand);
            }
            {
                TDecimal result = TDecimal.Zero;
                var c = 0;
                for (; !TUInt.IsZero(significand);) {
                    result = TDecimal.FusedMultiplyAdd(ToDecimalFromIntegerUnsignedTypeTraits<TDecimal, TUInt>.Exp5Of64, TDecimal.ScaleB(result, -64),
                        TDecimal.CreateChecked(UInt64.CreateTruncating(significand)));
                    significand >>= 64;
                    c += 64;
                }
                return TDecimal.ScaleB(result, c);
            }
        }

        internal readonly struct ToDecimalTypeTraits<TBinary, TBinaryUIntBits, TDecimal>
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>?
            where TBinaryUIntBits : IBinaryInteger<TBinaryUIntBits>?, IMinMaxValue<TBinaryUIntBits>?
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {

            public static TBinaryUIntBits MaxSignificandAsUIntBits { get; } =
                ToIntegerUnsignedUnchecked<TBinaryUIntBits>(FloatingPointIeee754TypeTraitsBasic<TDecimal>.GetMaxSignificandAsBigInteger());
        }



        internal static T Exp2<T>(uint exponent)
            where T : INumberBase<T>? {
            var result = T.One;
            var factor = T.One + T.One;
            while (exponent != 0) {
                if ((exponent & 1) != 0) {
                    result *= factor;
                }

                exponent >>= 1;

                if (exponent != 0) {
                    factor *= factor;
                }
            }

            return result;
        }

        internal readonly struct ToDecimalTypeTraits<TDecimal>
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {

            public static string DefaultFormat { get; } = $"E{FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1}";
          


        }
        internal readonly struct ToDecimalTypeTraits<TBinary, TDecimal>
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>?
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {
                       
            private static int GetExactFormatPrecision() {
                int precision =
                    FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.Precision;

                int minExponent =
                    FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.MinExponent;

                int minSubnormalExponent = minExponent - precision + 1;

                int significantDigits = (int)Math.Ceiling(
                    precision * Math.Log10(2d)
                    - minSubnormalExponent * Math.Log10(5d));

                var result = significantDigits;
                return result + 1; // +1 for safe margin
            }

            public static string ExactFormat { get; } = $"G{GetExactFormatPrecision()}";

            private static int GetExponentDigitCount(TBinary value) {
                var formatted = value.ToString("E0", CultureInfo.InvariantCulture);
                var exponentIndex = formatted.LastIndexOf('E');

                // Skip E and the exponent sign.
                return formatted.Length - exponentIndex - 2;
            }

            private static int GetMaxExponentDigits() {
                var epsilon = FloatingPointIeee754TypeTraitsBasic<TBinary>.Epsilon;

                return Math.Max(3, Math.Max(
                    GetExponentDigitCount(TBinary.MinValue),
                    GetExponentDigitCount(epsilon)));
            }

            private static int MaxExponentDigits { get; } = GetMaxExponentDigits();
            private static int GetMaxFormattedLength(int precision) {
                // Reserve one character for a negative sign, even for positive values.
                var length =
                    1 + // optional sign
                    1 + // leading significand digit
                    (precision > 0 ? 1 + precision : 0) + // decimal point and fraction
                    1 + // E
                    1 + // exponent sign
                    MaxExponentDigits;

                // Also accommodate special values such as "-Infinity".
                return Math.Max(length, "-Infinity".Length);
            }

            public static int MaxDefaultFormattedLength { get; } =
                GetMaxFormattedLength(
                    FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1);

            public static int MaxExactFormattedLength { get; } =
                GetMaxFormattedLength(GetExactFormatPrecision());

            public static int MaxExactPowerOfTwoExponent { get; } =
                (int)Math.Floor(FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision
                    / Math.Log10(2));


            public static bool SkipSignificandBoundCheck { get; } =
                FloatingPointIeee754TypeTraitsBasic<TBinary>.GetMaxSignificandAsBigInteger() <= FloatingPointIeee754TypeTraitsBasic<TDecimal>.GetMaxSignificandAsBigInteger();
        }

        private static TDecimal ToDecimalSlowPath<TBinary, TDecimal>(TBinary value)
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>?
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            {

            var capacity = ToDecimalTypeTraits<TBinary, TDecimal>.MaxDefaultFormattedLength;
#if NET11_0_OR_GREATER
            byte[]? rented = null;
            var buffer = capacity > StackAllocLimit
                ? (rented = ArrayPool<byte>.Shared.Rent(capacity)).AsSpan(0, capacity)
                : stackalloc byte[capacity];
#else
            char[]? rented = null;
            var buffer = capacity > StackAllocLimit / sizeof(char)
                ? (rented = ArrayPool<char>.Shared.Rent(capacity)).AsSpan(0, capacity)
                : stackalloc char[capacity];
#endif
            
            var s = value.TryFormat(buffer, out var written, ToDecimalTypeTraits<TDecimal>.DefaultFormat, CultureInfo.InvariantCulture);
            Debug.Assert(s);
            var E = ReadDecimalExponentFromUtf8(buffer.Slice(0, written));
            if (E < FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.MinExponent) {
                var p = FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1 - (FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.MinExponent - E);
                Span<char> aa = stackalloc char[16];
                aa[0] = 'E';
                p.TryFormat(aa[1..], out var charsWritten, "G", CultureInfo.InvariantCulture);
                s = value.TryFormat(buffer, out written, aa[..(1 + charsWritten)], CultureInfo.InvariantCulture);
                Debug.Assert(s);
                if (E != ReadDecimalExponentFromUtf8(buffer.Slice(0, written))) {
                    // extreme rare
                    if (rented is not null) {
#if NET11_0_OR_GREATER
                        ArrayPool<byte>.Shared.Return(rented);
#else
                        ArrayPool<char>.Shared.Return(rented);
#endif
                    }
                    capacity = ToDecimalTypeTraits<TBinary, TDecimal>.MaxExactFormattedLength;
#if NET11_0_OR_GREATER
                    buffer = capacity > StackAllocLimit
                        ? (rented = ArrayPool<byte>.Shared.Rent(capacity)).AsSpan(0, capacity)
                        : stackalloc byte[capacity];
#else
                    buffer = capacity > StackAllocLimit / sizeof(char)
                        ? (rented = ArrayPool<char>.Shared.Rent(capacity)).AsSpan(0, capacity)
                        : stackalloc char[capacity];
#endif
                    s = value.TryFormat(buffer, out written, ToDecimalTypeTraits<TBinary, TDecimal>.ExactFormat, CultureInfo.InvariantCulture);
                    Debug.Assert(s);
                }
            }

            var result = TDecimal.Parse(buffer.Slice(0, written), CultureInfo.InvariantCulture);
            if (rented is not null) {
#if NET11_0_OR_GREATER
                ArrayPool<byte>.Shared.Return(rented);
#else
                ArrayPool<char>.Shared.Return(rented);
#endif
            }
            return result;
        }

#if NET11_0_OR_GREATER
        private static int ReadDecimalExponentFromUtf8(ReadOnlySpan<byte> s) {
            var i = s.LastIndexOf((byte)'E');
            var t = s[(i + 1)..];
            return int.Parse(t, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }
#else
        private static int ReadDecimalExponentFromUtf8(ReadOnlySpan<char> s) {
            var i = s.LastIndexOf((char)'E');
            var t = s[(i + 1)..];
            return int.Parse(t, NumberStyles.Integer, CultureInfo.InvariantCulture);
        }
#endif

        private const int StackAllocLimit = 64;

        internal static TBinary ConvertIeee754DecimalToBinary<TDecimal, TDecimalUIntBits, TBinary>(TDecimal value)
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>?, IMinMaxValue<TDecimal>?
#endif
            where TDecimalUIntBits : IBinaryInteger<TDecimalUIntBits>?, IMinMaxValue<TDecimalUIntBits>?
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>? {

            // ---- 1. Special values -------------------------------------------------------------
            if (TDecimal.IsNaN(value)) return TBinary.NaN;
            if (TDecimal.IsPositiveInfinity(value)) return TBinary.PositiveInfinity;
            if (TDecimal.IsNegativeInfinity(value)) return TBinary.NegativeInfinity;
            if (TDecimal.IsZero(value))
                return TDecimal.IsNegative(value) ? TBinary.NegativeZero : -TBinary.NegativeZero;

            bool negative = TDecimal.IsNegative(value);
            var traits = ToBinaryTypeTraits<TDecimal, TDecimalUIntBits, TBinary>.Instance;

            // ---- 2. Decompose |value| = value * 10^exponent -------------------------------
            var absValue = TDecimal.Abs(value);
            TDecimalUIntBits significand = GetSignificand<TDecimal, TDecimalUIntBits>(absValue);

            int exponent = TDecimal.ILogB(absValue) - (FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1);

            // ---- 3a. Fast path ----------------------------------------------------------------
            //     If both operands are exactly representable in T, a single IEEE mul/div
            //     is single-rounding, hence correctly rounded.
            if (traits.SignificandFitsExactly(significand) &&
                traits.PowerOfTenFitsExactly(exponent)) {

                // T result = T.CreateChecked(significand);
                TBinary result = ToBinaryFromUInt<TDecimalUIntBits, TBinary>(significand);


                if (exponent > 0) result *= Exp10<TBinary>(unchecked((uint)exponent));
                else if (exponent < 0) result /= Exp10<TBinary>(unchecked((uint)-exponent));
                return negative ? -result : result;
            }

            // ---- 3b. Slow path: result decimal string, then correctly-rounded Parse ------------
            return ToBinarySlowPath(significand, exponent, negative, traits);
        }

        private static TUIntBits GetSignificand<TFloat, TUIntBits>(TFloat value)
            where TFloat : IFloatingPointIeee754<TFloat>?, IMinMaxValue<TFloat>?
            where TUIntBits : IBinaryInteger<TUIntBits>?, IMinMaxValue<TUIntBits>? {
            var sigByteCount = value.GetSignificandByteCount();
            TUIntBits significand;
            byte[]? sigBytesArray = null;
            Span<byte> sigBytes = sigByteCount > 64 ? (sigBytesArray = ArrayPool<byte>.Shared.Rent(sigByteCount)).AsSpan(0, sigByteCount) : stackalloc byte[sigByteCount];
            var ccc = !System.BitConverter.IsLittleEndian ? value.WriteSignificandBigEndian(sigBytes) : value.WriteSignificandLittleEndian(sigBytes);
            var sigBytes1 = sigBytes[..sigByteCount];
            significand = !System.BitConverter.IsLittleEndian ? TUIntBits.ReadBigEndian(sigBytes1, isUnsigned: true) : TUIntBits.ReadLittleEndian(sigBytes1, isUnsigned: true);
            if (sigBytesArray != null) {
                ArrayPool<byte>.Shared.Return(sigBytesArray);
            }
            return significand;
        }

        private static TUIntBits GetExponent<TFloat, TUIntBits>(TFloat value)
           where TFloat : IFloatingPointIeee754<TFloat>?, IMinMaxValue<TFloat>?
           where TUIntBits : IBinaryInteger<TUIntBits>?, IMinMaxValue<TUIntBits>? {
            var sigByteCount = value.GetExponentByteCount();
            TUIntBits significand;
            byte[]? sigBytesArray = null;
            Span<byte> sigBytes = sigByteCount > 64 ? (sigBytesArray = ArrayPool<byte>.Shared.Rent(sigByteCount)).AsSpan(0, sigByteCount) : stackalloc byte[sigByteCount];
            var ccc = !System.BitConverter.IsLittleEndian ? value.WriteExponentBigEndian(sigBytes) : value.WriteExponentLittleEndian(sigBytes);
            significand = !System.BitConverter.IsLittleEndian ? TUIntBits.ReadBigEndian(sigBytes, isUnsigned: false) : TUIntBits.ReadLittleEndian(sigBytes, isUnsigned: false);
            if (sigBytesArray != null) {
                ArrayPool<byte>.Shared.Return(sigBytesArray);
            }
            return significand;
        }

        private static TBinary ToBinaryFromUInt<TUInt, TBinary>(TUInt value)
            where TUInt : IBinaryInteger<TUInt>?, IMinMaxValue<TUInt>?
            where TBinary : IBinaryFloatingPointIeee754<TBinary>?, IMinMaxValue<TBinary>? {
            if (ulong.CreateChecked(TUInt.PopCount(TUInt.AllBitsSet)) > UInt32.PopCount(UInt32.MaxValue)) {
                TBinary f = -TBinary.NegativeZero;
                int c = 0;
                for (; !TUInt.IsZero(value);) {
                    f += TBinary.CreateTruncating(UInt32.CreateTruncating(value));
                    f = TBinary.ScaleB(f, -32);
                    value >>>= 32;
                    c += 32;
                }
                return TBinary.ScaleB(f, c);
            } else {
                return TBinary.CreateTruncating(UInt32.CreateTruncating(value));
            }
        }

        private static TBinary ToBinarySlowPath<TDecimal, TDecimalUIntBits, TBinary>(
            TDecimalUIntBits significand, int exponent, bool negative,
            ToBinaryTypeTraits<TDecimal, TDecimalUIntBits, TBinary> traits)
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>, IMinMaxValue<TDecimal>
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>, IMinMaxValue<TDecimal>
#endif
            where TDecimalUIntBits : IBinaryInteger<TDecimalUIntBits>, IMinMaxValue<TDecimalUIntBits>
            where TBinary : IBinaryFloatingPointIeee754<TBinary>, IMinMaxValue<TBinary> {

            int cap = traits.MaxFormattedLength;
            Span<char> buffer = cap <= 256
                ? stackalloc char[cap]
                : new char[cap];

            if (!TryFormatExact(buffer, significand, exponent, negative, out int written))
                throw new InvalidOperationException("Buffer sizing is wrong.");

            return TBinary.Parse(buffer.Slice(0, written),
                NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        // Writes <sign><value-digits>E<signed-exponent> using invariant formatting.
        private static bool TryFormatExact<TDecimalUIntBits>(
            Span<char> dest, TDecimalUIntBits significand, int exponent, bool negative, out int written)
            where TDecimalUIntBits : IBinaryInteger<TDecimalUIntBits>, IMinMaxValue<TDecimalUIntBits> {

            int pos = 0;
            if (negative) dest[pos++] = '-';

            if (!significand.TryFormat(dest.Slice(pos), out int sigWritten,
                    default, CultureInfo.InvariantCulture)) { written = 0; return false; }
            pos += sigWritten;

            dest[pos++] = 'E';

            if (!exponent.TryFormat(dest.Slice(pos), out int expWritten,
                    default, CultureInfo.InvariantCulture)) { written = 0; return false; }
            pos += expWritten;

            written = pos;
            return true;
        }

        private static TUInt ToIntegerUnsignedUnchecked<TUInt>(BigInteger value)
            where TUInt : IBinaryInteger<TUInt>?, IMinMaxValue<TUInt>? {
            return TUInt.CreateTruncating(value);
        }

        public static TInt BinarySearchMaxValueFromBelow<TInt>(Predicate<TInt> predicate)
            where TInt : IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            return BinarySearchMaxValueFromBelow(predicate, TInt.MinValue);
        }

        public static TInt BinarySearchMaxValueFromBelow<TInt>(Predicate<TInt> predicate, TInt start)
            where TInt : IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            Debug.Assert(predicate(start));

            TInt lo = start;
            TInt step = TInt.One;
            TInt two = TInt.One + TInt.One;

            // Exponential (galloping) search: probe MinValue + 1, 2, 4, 8, ...
            // until the predicate first returns false, so we have a bracket.
            while (true) {
                // Guard against overflow when computing lo + step.
                TInt next = lo > TInt.MaxValue - step
                    ? TInt.MaxValue
                    : lo + step;

                if (predicate(next)) {
                    // Predicate is still true all the way up to MaxValue.
                    if (next == TInt.MaxValue)
                        return TInt.MaxValue;

                    lo = next;

                    // Double step, but never let it exceed MaxValue.
                    step = step > TInt.MaxValue / two
                        ? TInt.MaxValue
                        : step + step;
                } else {
                    // Invariant now: predicate(lo) == true, predicate(next) == false.
                    // Binary search the boundary in (lo, next).
                    TInt hi = next;
                    while (hi - lo > TInt.One) {
                        TInt mid = lo + (hi - lo) / two;
                        if (predicate(mid))
                            lo = mid;
                        else
                            hi = mid;
                    }

                    // lo is the largest value for which predicate returned true.
                    return lo;
                }
            }
        }
        // ---- ToBinaryTypeTraits (cached per closed generic instantiation) --------------------------------

        private readonly struct ToBinaryTypeTraits<TDecimal, TDecimalUIntBits, TBinary>
#if NET11_0_OR_GREATER
            where TDecimal : System.Numerics.IDecimalFloatingPointIeee754<TDecimal>, IMinMaxValue<TDecimal>
#else
            where TDecimal : IDecimalFloatingPointIeee754<TDecimal>, IMinMaxValue<TDecimal>
#endif
            where TDecimalUIntBits : IBinaryInteger<TDecimalUIntBits>, IMinMaxValue<TDecimalUIntBits>
            where TBinary : IBinaryFloatingPointIeee754<TBinary>, IMinMaxValue<TBinary> {

            public static readonly ToBinaryTypeTraits<TDecimal, TDecimalUIntBits, TBinary> Instance
                = new ToBinaryTypeTraits<TDecimal, TDecimalUIntBits, TBinary>();

            /// <summary>Largest exponent such that 10^exponent is exactly representable in <typeparamref name="TBinary"/>.</summary>
            public readonly int MaxExactDecimalExponent;


            /// <summary>Upper bound on the length of "-<value>E<exponent>".</summary>
            public readonly int MaxFormattedLength;
            public readonly TDecimalUIntBits SignificandBound;
            public readonly bool SkipSignificandBoundCheck;

            public ToBinaryTypeTraits() {
                // 10^exponent = 2^exponent * 5^exponent, and 5^exponent is odd, so 10^exponent is result iff 5^exponent < 2^Precision.

                // find max exponent.
                MaxExactDecimalExponent = BinarySearchMaxValueFromBelow((int n) => BigInteger.Log2(BigInteger.Pow(5, n)) < FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.Precision, 0);
                // Significand bound
                var significandBoundBigInt = ((BigInteger.One << (FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.Precision - 1)) << 1) | 1;

                var maxSignificand = (BigInteger.Pow(10, FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1) - 1) * 10 + 9;
                SkipSignificandBoundCheck = maxSignificand <= significandBoundBigInt;
                // TUInt creating from BigInteger may not be impl'ed.
                SignificandBound = ToIntegerUnsignedUnchecked<TDecimalUIntBits>(significandBoundBigInt);




                int maxSigChars = FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision + 1; // +1 for the decimal point

                // Range of the value-exponent e.  A value |v| = s * 10^e has
                // 10^MinExponent <= |v| < 10^(MaxExponent+1) for normalized v, and
                // even subnormals stay within [MinExponent - maxSigChars, MaxExponent].

                long lowBound = (long)FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.MinExponent - (FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.Precision - 1);
                long highBound = (long)FloatingPointIeee754TypeTraitsBasic<TDecimal>.As<int>.MaxExponent + 1; // add one margin
                long absBound = Math.Max(1, Math.Max(Math.Abs(lowBound), Math.Abs(highBound)));

                var expDigits = checked((int)long.Log10(absBound));

                // 'E' + optional '-' + digits.
                int maxExpChars = 1 + 1 + expDigits;

                MaxFormattedLength = 1 /* '-' */ + maxSigChars + maxExpChars;
            }

            public bool SignificandFitsExactly(TDecimalUIntBits significand) {
                if (SkipSignificandBoundCheck) return true;
                return TDecimalUIntBits.IsZero(significand >>> (FloatingPointIeee754TypeTraitsBasic<TBinary>.As<int>.Precision));
            }

            public bool PowerOfTenFitsExactly(int exponent)
                => exponent >= -MaxExactDecimalExponent && exponent <= MaxExactDecimalExponent;

        }

        internal static T Exp10<T>(uint exponent)
            where T : INumberBase<T>? {

            T b = T.CreateChecked(10); // base = 10
            T pow = T.One;

            uint e = exponent;
            while (e != 0) {
                if ((e & 1u) != 0)
                    pow *= b;

                e >>= 1;
                if (e != 0)          // skip the final, unused square
                    b *= b;
            }

            return pow;
        }

        internal static T Exp5<T>(uint exponent)
            where T : INumberBase<T>? {

            T b = T.CreateChecked(5); // base = 10
            T pow = T.One;

            uint e = exponent;
            while (e != 0) {
                if ((e & 1u) != 0)
                    pow *= b;

                e >>= 1;
                if (e != 0)          // skip the final, unused square
                    b *= b;
            }

            return pow;
        }
    }

#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

}

#region Polyfill
namespace UltimateOrb {

    public static partial class BinaryIntegerExtensions {

#if !NET11_0_OR_GREATER

        extension(long) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static long Log10(long value) {
                if (value < 0) {
#if STANDALONE_NUMERICS_LIBRARY
                    ArgumentOutOfRangeException.ThrowIfNegative(value);
#else
                    UltimateOrb.Internal.ThrowHelper.ThrowValueArgumentOutOfRange_NeedNonNegNumException();
#endif
                }
                return (long)ulong.Log10((ulong)value);
            }
        }
#endif
                }
}

namespace UltimateOrb {

    public static partial class BinaryIntegerExtensions {

#if !NET11_0_OR_GREATER

        extension(ulong) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ulong Log10(ulong value) {
                // Approximate log10 via log2, then correct with a powers of 10 lookup table.
                // http://graphics.stanford.edu/~seander/bithacks.html#IntegerLog10
                value |= 1;
                uint log2 = (uint)BitOperations.Log2(value) + 1;
                uint approx = (log2 * 1233) >> 12;
                return value < PowersOf10[(int)approx] ? approx - 1 : approx;
            }
        }

        // Lookup table for the powers of ten representable in a UInt64 (10^0 through 10^19).
        internal static ReadOnlySpan<ulong> PowersOf10 => [
            1,
            10,
            100,
            1_000,
            10_000,
            100_000,
            1_000_000,
            10_000_000,
            100_000_000,
            1_000_000_000,
            10_000_000_000,
            100_000_000_000,
            1_000_000_000_000,
            10_000_000_000_000,
            100_000_000_000_000,
            1_000_000_000_000_000,
            10_000_000_000_000_000,
            100_000_000_000_000_000,
            1_000_000_000_000_000_000,
            10_000_000_000_000_000_000,
        ];
#endif
    }
}
#endregion