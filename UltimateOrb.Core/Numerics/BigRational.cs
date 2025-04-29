using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UltimateOrb.Mathematics.Exact;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP")]
    [SerializableAttribute()]
    public readonly partial struct BigRational
        : IEquatable<BigRational>
        , IComparable<BigRational> {

        private readonly BigInteger m_Denominator;

        private readonly BigInteger m_SignedNumerator;

        public BigInteger Denominator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var q = m_Denominator;
                if (q.IsZero) {
                    return 1;
                }
                return q;
            }
        }

        public BigInteger SignedNumerator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return m_SignedNumerator;
            }
        }

        public BigInteger Numerator {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return BigInteger.Abs(m_SignedNumerator);
            }
        }

        public BigInteger SignedDenominator {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var q = m_Denominator;
                var p = m_SignedNumerator.Sign;
                return p == 0 ? 1 : (0 > m_SignedNumerator.Sign ? -q : q);
            }
        }

        public int Sign {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return m_SignedNumerator.Sign;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        private BigRational(BigInteger denominator, BigInteger signedNumerator) {
            Debug.Assert(!signedNumerator.IsZero || denominator.IsZero);
            Debug.Assert(signedNumerator.IsZero || denominator.Sign > 0);
            Debug.Assert(signedNumerator.IsZero || BigInteger.GreatestCommonDivisor(denominator, signedNumerator).IsOne);
            this.m_Denominator = denominator;
            this.m_SignedNumerator = signedNumerator;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator BigRational(Rational64 value) {
            return default(Rational64) == value ? default : new BigRational(value.Denominator, value.SignedNumerator);
        }

        private static readonly BigInteger s_Rational64MaxDenominator = unchecked((UInt32)Int32.MinValue);

        private static readonly BigInteger s_Rational64MaxNumerator = UInt32.MaxValue;

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(BigRational value) {
            var d = value.m_Denominator;
            if (!d.IsZero) {
                if (d <= s_Rational64MaxDenominator) {
                    var n = value.m_SignedNumerator;
                    if (0 < n.Sign) {
                        if (n <= s_Rational64MaxNumerator) {
                            return Rational64.FromInt64Bits(((Int64)(unchecked((Int32)checked((UInt32)d) - (Int32)1)) << 32 | (Int64)unchecked((Int32)checked((UInt32)n))));
                        }
                    }
                    {
                        n = -n;
                        if (n <= s_Rational64MaxNumerator) {
                            return Rational64.FromInt64Bits(((Int64)(-unchecked((Int32)checked((UInt32)d))) << 32 | (Int64)unchecked((Int32)checked((UInt32)n))));
                        }
                    }
                }
                {
                    var ignored = checked(0u - unchecked((uint)d.Sign));
                }
            }
            return default;
        }

        private static readonly BigInteger s_BigIntegerOne = BigInteger.One;

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator BigRational(BigInteger value) {
            return value.IsZero ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator BigInteger(BigRational value) {
            if (value.m_SignedNumerator.IsZero) {
                return default;
            }
            return value.m_SignedNumerator / value.m_Denominator;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Double(BigRational value) {
            if (value.m_SignedNumerator.IsZero) {
                return (Double)0;
            }
            return (Double)value.m_SignedNumerator / (Double)value.m_Denominator;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator +(BigRational value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator -(BigRational value) {
            return new BigRational(value.m_Denominator, -value.m_SignedNumerator);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator +(BigRational first, BigRational second) {
            if (first.m_SignedNumerator.IsZero) {
                return second;
            }
            if (second.m_SignedNumerator.IsZero) {
                return first;
            }
            var d = first.m_Denominator * second.m_Denominator;
            var n = first.m_SignedNumerator * second.m_Denominator + first.m_Denominator * second.m_SignedNumerator;
            if (n.IsZero) {
                return default;
            }
            var g = BigInteger.GreatestCommonDivisor(d, n);
            return new BigRational(d / g, n / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator -(BigRational first, BigRational second) {
            if (first.m_SignedNumerator.IsZero) {
                return -second;
            }
            if (second.m_SignedNumerator.IsZero) {
                return first;
            }
            var d = first.m_Denominator * second.m_Denominator;
            var n = first.m_SignedNumerator * second.m_Denominator - first.m_Denominator * second.m_SignedNumerator;
            if (n.IsZero) {
                return default;
            }
            var g = BigInteger.GreatestCommonDivisor(d, n);
            return new BigRational(d / g, n / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator *(BigRational first, BigRational second) {
            if (first.m_SignedNumerator.IsZero || second.m_SignedNumerator.IsZero) {
                return default;
            }
            var g0 = BigInteger.GreatestCommonDivisor(first.m_Denominator, second.m_SignedNumerator);
            var g1 = BigInteger.GreatestCommonDivisor(first.m_SignedNumerator, second.m_Denominator);
            var d = (first.m_Denominator / g0) * (second.m_Denominator / g1);
            var n = (first.m_SignedNumerator / g1) * (second.m_SignedNumerator / g0);
            return new BigRational(d, n);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator /(BigRational first, BigRational second) {
            if (second.m_SignedNumerator.IsZero) {
                var ignored = Default<int>.Value / 0;
            }
            if (first.m_SignedNumerator.IsZero) {
                return default;
            }
            var g0 = BigInteger.GreatestCommonDivisor(first.m_Denominator, second.m_Denominator);
            var g1 = BigInteger.GreatestCommonDivisor(first.m_SignedNumerator, second.m_SignedNumerator);
            var d = (first.m_Denominator / g0) * (second.m_SignedNumerator / g1);
            var n = (first.m_SignedNumerator / g1) * (second.m_Denominator / g0);
            if (0 > d.Sign) {
                d = -d;
                n = -n;
            }
            return new BigRational(d, n);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational Invert(BigRational value) {
            var s = value.m_SignedNumerator.Sign;
            if (0 == s) {
                var ignored = Default<int>.Value / 0;
            }
            if (value.m_SignedNumerator.IsZero) {
                return default;
            }
            return 0 > s ? new BigRational(-value.m_Denominator, -value.m_SignedNumerator) : new BigRational(value.m_Denominator, value.m_SignedNumerator);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational FromFraction(long numerator, long denominator) {
            if (0 == denominator) {
                var ignored = Default<int>.Value / 0;
            }
            if (0 == numerator) {
                return default;
            }
            var g = Mathematics.NumberTheory.EuclideanAlgorithm.GreatestCommonDivisor(numerator, denominator);
            if (0 > denominator) {
                g = -g;
            }
            return new BigRational(denominator / g, numerator / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational FromFraction(BigInteger numerator, BigInteger denominator) {
            var s = denominator.Sign;
            if (0 == s) {
                var ignored = Default<int>.Value / 0;
            }
            if (numerator.IsZero) {
                return default;
            }
            var g = BigInteger.GreatestCommonDivisor(numerator, denominator);
            if (0 > s) {
                g = -g;
            }
            return new BigRational(denominator / g, numerator / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator ==(BigRational first, BigRational second) {
            return first.m_SignedNumerator == second.m_SignedNumerator && first.m_Denominator == second.m_Denominator;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator !=(BigRational first, BigRational second) {
            return first.m_SignedNumerator != second.m_SignedNumerator || first.m_Denominator != second.m_Denominator;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override bool Equals(object obj) {
            if (obj is BigRational value) {
                return this == value;
            }
            return base.Equals(obj);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override int GetHashCode() {
            return this.m_Denominator.GetHashCode() ^ this.m_SignedNumerator.GetHashCode();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public bool Equals(BigRational other) {
            return this.m_Denominator.Equals(other.m_Denominator) && this.m_SignedNumerator.Equals(other.m_SignedNumerator);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public int CompareTo(BigRational other) {
            var a = this.m_SignedNumerator.CompareTo(other.m_SignedNumerator);
            return 0 != a ? a : (this.m_SignedNumerator * other.m_Denominator).CompareTo(this.m_Denominator * other.m_SignedNumerator);
        }
    }
}

namespace UltimateOrb.Numerics {
    using System.Text;

    public readonly partial struct BigRational : IFormattable {

        public override string ToString() {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.m_SignedNumerator);
            var t = this.Denominator;
            if (!t.IsOne) {
                var i = sb.Length - 1;
                for (; 0 <= i; --i) {
                    var c = sb[i];
                    if (char.IsNumber(c)) {
                        break;
                    }
                }
                sb.Insert(i + 1, '/');
                sb.Insert(i + 2, t);
            }
            return sb.ToString();
        }

        public string ToString(IFormatProvider formatProvider) {
            return this.ToString(null!, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.m_SignedNumerator.ToString(format, formatProvider));
            var t = this.m_Denominator;
            if (1 != t) {
                var i = sb.Length - 1;
                for (; 0 <= i; --i) {
                    var c = sb[i];
                    if (char.IsNumber(c)) {
                        break;
                    }
                }
                sb.Insert(i + 1, '/');
                sb.Insert(i + 2, t.ToString(format, formatProvider));
            }
            return sb.ToString();
        }
    }
}



namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational {

        public static implicit operator BigRational(Single value) => FromSingle(value);

        public static implicit operator BigRational(double value) => FromDouble(value);

        public static BigRational FromSingle(Single value) {
            const int SingleExponentBias = 127;
            const int SingleMantissaBits = 23;

            var bits = BitConverter.SingleToInt32Bits(value);
            int exponent = (bits >> SingleMantissaBits) & 0xFF;

            if (exponent == 0xFF) {
                ThrowNotFiniteNumberException(value);
            }

            bool isNegative = (unchecked((UInt32)bits) & 0x80000000) != 0;
            var mantissa = bits & 0x7FFFFF;

            if (exponent == 0) {
                // Subnormal number or zero
                if (mantissa == 0) {
                    return default(BigRational);
                }
                exponent = 1 - SingleExponentBias - SingleMantissaBits;
            } else {
                // Normalized number
                mantissa |= 0x800000;
                exponent -= SingleExponentBias + SingleMantissaBits;
            }

            // Remove trailing zeros from mantissa
            int trailingZeros = BitOperations.TrailingZeroCount(mantissa);
            mantissa >>= trailingZeros;
            exponent += trailingZeros;

            BigInteger numerator = new BigInteger(mantissa);
            BigInteger denominator;

            if (exponent > 0) {
                numerator <<= exponent; // Shift left to multiply by 2^exponent
                denominator = BigInteger.One;
            } else {
                denominator = BigInteger.One << -exponent; // Shift left to multiply by 2^-exponent
            }

            if (isNegative) {
                numerator = -numerator;
            }

            return new BigRational(denominator, numerator);
        }

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(float value) {
            throw new NotFiniteNumberException($"The float value {value} is not finite.");
        }

        public static BigRational FromDouble(double value) {
            const int DoubleExponentBias = 1023;
            const int DoubleMantissaBits = 52;

            Int64 bits = BitConverter.DoubleToInt64Bits(value);
            int exponent = (int)((bits >> DoubleMantissaBits) & 0x7FF);

            if (exponent == 0x7FF) {
                ThrowNotFiniteNumberException(value);
            }

            bool isNegative = (unchecked((UInt64)bits) & 0x8000000000000000) != 0;
            var mantissa = bits & 0xFFFFFFFFFFFFF;

            if (exponent == 0) {
                // Subnormal number or zero
                if (mantissa == 0) {
                    return default(BigRational);
                }
                exponent = 1 - DoubleExponentBias - DoubleMantissaBits;
            } else {
                // Normalized number
                mantissa |= 0x10000000000000;
                exponent -= DoubleExponentBias + DoubleMantissaBits;
            }

            // Remove trailing zeros from mantissa
            int trailingZeros = BitOperations.TrailingZeroCount(mantissa);
            mantissa >>= trailingZeros;
            exponent += trailingZeros;

            BigInteger numerator = new BigInteger(mantissa);
            BigInteger denominator;

            if (exponent > 0) {
                numerator <<= exponent; // Shift left to multiply by 2^exponent
                denominator = BigInteger.One;
            } else {
                denominator = BigInteger.One << -exponent; // Shift left to multiply by 2^-exponent
            }

            if (isNegative) {
                numerator = -numerator;
            }

            return new BigRational(denominator, numerator);
        }

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(double value) {
            throw new NotFiniteNumberException($"The double value {value} is not finite.");
        }

        static BigRational ContinuedFraction(double value, int maxIterations) {
            Debug.Assert(value > 0);
            BigInteger numerator = 0;
            BigInteger denominator = 1;
            BigInteger prevNumerator = 1;
            BigInteger prevDenominator = 0;

            for (int i = 0; i < maxIterations; i++) {
                if (value == 0) {
                    break;
                }

                BigInteger integerPart = (BigInteger)value;
                value -= (double)integerPart;
                value = 1.0 / value;

                BigInteger tempNumerator = numerator;
                numerator = integerPart * numerator + prevNumerator;
                prevNumerator = tempNumerator;

                BigInteger tempDenominator = denominator;
                denominator = integerPart * denominator + prevDenominator;
                prevDenominator = tempDenominator;
            }
            if (numerator.IsZero) {
                return Zero;
            }
            return new BigRational(denominator, numerator);
        }
        public static BigRational FromFloatContinuedFraction(float value, int maxIterations = 13) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            if (float.IsNaN(value) || float.IsInfinity(value)) {
                ThrowNotFiniteNumberException(value);
            }
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigRational result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = new BigRational(result.Denominator, - result.Numerator);
            }
            return result;
        }

        public static BigRational FromDoubleContinuedFraction(double value, int maxIterations = 26) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            if (double.IsNaN(value) || double.IsInfinity(value)) {
                ThrowNotFiniteNumberException(value);
            }
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigRational result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = new BigRational(result.Denominator , - result.Numerator);
            }
            return result;
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational {

        public static partial class Math {

            public static BigRational Floor(BigRational value) {
                var quotient = BigInteger.DivRem(value.SignedNumerator, value.Denominator, out var remainder);
                if (BigInteger.IsNegative(remainder)) {
                    --quotient;
                }
                return quotient;
            }
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational
        : INumber<BigRational> {

        public static BigRational One => new BigRational(BigInteger.One, BigInteger.One);

        public static BigRational MinusOne => new BigRational(BigInteger.One, BigInteger.MinusOne);

        public static int Radix => throw new NotSupportedException();

        public static BigRational Zero => default;

        static BigRational IAdditiveIdentity<BigRational, BigRational>.AdditiveIdentity => One;

        static BigRational IMultiplicativeIdentity<BigRational, BigRational>.MultiplicativeIdentity => default;

        public static BigRational Abs(BigRational value) {
            return new BigRational(value.m_Denominator, BigInteger.Abs(value.m_SignedNumerator));
        }

        public static bool IsCanonical(BigRational value) {
            return true;
        }

        public static bool IsComplexNumber(BigRational value) {
            return false;
        }

        public static bool IsEvenInteger(BigRational value) {
            return (value.m_Denominator.IsOne && value.m_SignedNumerator.IsEven) || value.m_SignedNumerator.IsZero;
        }

        public static bool IsFinite(BigRational value) {
            return true;
        }

        public static bool IsImaginaryNumber(BigRational value) {
            return false;
        }

        public static bool IsInfinity(BigRational value) {
            return false;
        }

        public static bool IsInteger(BigRational value) {
            return value.m_Denominator.IsOne || value.m_Denominator.IsZero;
        }

        public static bool IsNaN(BigRational value) {
            return false;
        }

        public static bool IsNegative(BigRational value) {
            return BigInteger.IsNegative(value.m_SignedNumerator);
        }

        public static bool IsNegativeInfinity(BigRational value) {
            return false;
        }

        public static bool IsNormal(BigRational value) {
            return !value.m_SignedNumerator.IsZero;
        }

        public static bool IsOddInteger(BigRational value) {
            return (value.m_Denominator.IsOne && !value.m_SignedNumerator.IsEven) || value.m_SignedNumerator.IsZero;
        }

        public static bool IsPositive(BigRational value) {
            return BigInteger.IsPositive(value.m_SignedNumerator);
        }

        public static bool IsPositiveInfinity(BigRational value) {
            return false;
        }

        public static bool IsRealNumber(BigRational value) {
            return true;
        }

        public static bool IsSubnormal(BigRational value) {
            return false;
        }

        public static bool IsZero(BigRational value) {
            return value.m_SignedNumerator.IsZero;
        }

        public static BigRational MaxMagnitude(BigRational x, BigRational y) {
            var ax = Abs(x);
            var ay = Abs(y);
            return ax > ay ? x : y;
        }

        public static BigRational MaxMagnitudeNumber(BigRational x, BigRational y) {
            return MaxMagnitude(x, y);
        }

        public static BigRational MinMagnitude(BigRational x, BigRational y) {
            var ax = Abs(x);
            var ay = Abs(y);
            return ax < ay ? x : y;
        }

        public static BigRational MinMagnitudeNumber(BigRational x, BigRational y) {
            return MinMagnitude(x, y);
        }

        public static BigRational Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigRational Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigRational Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigRational>.TryConvertFromChecked<TOther>(TOther value, out BigRational result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigRational>.TryConvertFromSaturating<TOther>(TOther value, out BigRational result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigRational>.TryConvertFromTruncating<TOther>(TOther value, out BigRational result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigRational>.TryConvertToChecked<TOther>(BigRational value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigRational>.TryConvertToSaturating<TOther>(BigRational value, out TOther result) {
            throw new NotImplementedException();
        }

        static readonly BigInteger s_UInt32MaxValueAsBigInteger = UInt32.MaxValue;

        static readonly BigInteger s_UInt64MaxValueAsBigInteger = UInt64.MaxValue;

        static readonly BigInteger s_UInt128MaxValueAsBigInteger = UInt128.MaxValue;

        static readonly BigInteger s_UInt16MaxValueAsBigInteger = UInt16.MaxValue;

        static readonly BigInteger s_ByteMaxValueAsBigInteger = byte.MaxValue;

        static readonly BigInteger s_UIntPtrMaxValueAsBigInteger = UIntPtr.MaxValue;

        static bool INumberBase<BigRational>.TryConvertToTruncating<TOther>(BigRational value, out TOther result) {
            static BigInteger F(BigRational value) {
                if (value.m_SignedNumerator.IsZero) {
                    return value.m_SignedNumerator;
                }
                return value.m_SignedNumerator / value.m_Denominator;
            }
            if (typeof(TOther) == typeof(BigInteger)) {
                result = (TOther)(object)F(value);
                return true;
            } else if (typeof(TOther) == typeof(Int32)) {
                result = (TOther)(object)unchecked((Int32)(UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(UInt32)) {
                result = (TOther)(object)unchecked((UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(Int64)) {
                result = (TOther)(object)unchecked((Int64)(UInt64)(s_UInt64MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(UInt64)) {
                result = (TOther)(object)unchecked((UInt64)(s_UInt64MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                result = (TOther)(object)unchecked((Int32)(UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                result = (TOther)(object)unchecked((UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                result = (TOther)(object)unchecked((Int32)(UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                result = (TOther)(object)unchecked((UInt32)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                result = (TOther)(object)unchecked((IntPtr)(UIntPtr)(s_UInt32MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                result = (TOther)(object)unchecked((UIntPtr)(s_UIntPtrMaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(Int16)) {
                result = (TOther)(object)unchecked((Int16)(UInt16)(s_UInt16MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(UInt16)) {
                result = (TOther)(object)unchecked((UInt16)(s_UInt16MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                // sizeof(char) == 2
                result = (TOther)(object)unchecked((char)(s_UInt16MaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                result = (TOther)(object)unchecked((sbyte)(byte)(s_ByteMaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(byte)) {
                result = (TOther)(object)unchecked((byte)(s_ByteMaxValueAsBigInteger & F(value)));
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                // TODO:
                result = (TOther)(object)(float)((double)value.Numerator / (double)value.Denominator);
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                // TODO:
                if (!value.m_SignedNumerator.IsZero) {
                    var p = BigInteger.Abs(value.m_SignedNumerator);
                    var a = p.GetBitLength();
                    Debug.Assert(a > 0);
                    var b = value.m_Denominator.GetBitLength();
                    Debug.Assert(b > 0);
                    var c = a - b;
                    if (c > 1024) {
                        result = (TOther)(object)(BigInteger.IsNegative(value.m_SignedNumerator) ? double.NegativeInfinity : double.PositiveInfinity);
                        return true;
                    }
                    if (c < -1075) {
                        result = (TOther)(object)(BigInteger.IsNegative(value.m_SignedNumerator) ? -0.0: 0.0);
                        return true;
                    }
                    var c0 = (int)c;
                    var f = ((0 > c0) ?
                        ((p << -c0) < b) :
                        ((p >> c0) < b));
                    c0 -= f ? 1 : 0;
                    var c1 = c0 - 52;
                    var (q, r) = 0 > c1 ?
                        BigInteger.DivRem(p << -c1, value.m_Denominator) :
                        BigInteger.DivRem(p, value.m_Denominator << c1);
                    throw new NotImplementedException();
                    if (c > 1022) {

                    }
                    result = (TOther)(object)((double)value.Numerator / (double)value.Denominator);
                    return true;
                }
                result = (TOther)(object)0.0D;
                return true;
            } else {
                result = default!;
                return false;
            }
        }

        public int CompareTo(object? obj) {
            if (obj == null) {
                    return 1;
                }

                if (obj is not BigRational bigRational) {
                    throw new ArgumentException("The parameter must be a BigRational.", nameof(obj));
                }

                return CompareTo(bigRational);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigRational operator ++(BigRational value) {
            return value.m_SignedNumerator.IsZero ? BigRational.One : new BigRational(value.m_Denominator, value.m_SignedNumerator + value.m_Denominator);
        }

        public static BigRational operator --(BigRational value) {
            return value.m_SignedNumerator.IsZero ? BigRational.MinusOne : new BigRational(value.m_Denominator, value.m_SignedNumerator - value.m_Denominator);
        }

        public static BigRational DivideEuclidean(BigRational first, BigRational second) {
            if (second.m_SignedNumerator.IsZero) {
                _ = Default<int>.Value / 0;
            }
            if (first.m_SignedNumerator.IsZero) {
                return Zero;
            }
            return (first.m_SignedNumerator * second.Denominator) / (first.Denominator * second.m_SignedNumerator);
        }

        public static BigRational operator %(BigRational first, BigRational second) {
            if (second.m_SignedNumerator.IsZero) {
                _ = Default<int>.Value / 0;
            }
            if (first.m_SignedNumerator.IsZero) {
                return Zero;
            }
            var p = (first.m_SignedNumerator * second.Denominator) % (first.Denominator * second.m_SignedNumerator);
            var q = first.Denominator * second.Denominator;
            var d = BigInteger.GreatestCommonDivisor(q, p);
            return new BigRational(q / d, p / d);
        }

        public static bool operator <(BigRational first, BigRational second) {
            return first.CompareTo(second) < 0;
        }

        public static bool operator >(BigRational first, BigRational second) {
            return first.CompareTo(second) > 0;
        }

        public static bool operator <=(BigRational first, BigRational second) {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >=(BigRational first, BigRational second) {
            return first.CompareTo(second) >= 0;
        }
    }
}