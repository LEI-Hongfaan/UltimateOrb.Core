﻿using System;
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
    public readonly partial struct BigDecimal
        : IEquatable<BigDecimal>
        , IComparable<BigDecimal> {

        private readonly BigInteger m_SignedSignificand;

        private readonly nint m_shiftCount;


        // result's default precision is determined by these fields of the operands.
        private static nuint s_defaultPrecision; // 
        private static FloatingPointRounding s_defaultRounding = FloatingPointRounding.ToNearestWithMidpointToEven; // IEEE rounding, default: roundTiesToEven



        /*
        public BigInteger Denominator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var q = m_SignedShiftCount;
                if (q.IsZero) {
                    return 1;
                }
                return q;
            }
        }
        
        public BigInteger SignedSignificand {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return m_SignedSignificand;
            }
        }

        public BigInteger Numerator {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return BigInteger.Abs(m_SignedSignificand);
            }
        }

        public BigInteger SignedDenominator {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var q = m_SignedShiftCount;
                var p = m_SignedSignificand.Sign;
                return p == 0 ? 1 : (0 > m_SignedSignificand.Sign ? -q : q);
            }
        }
        */

        public int Sign {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                double a;
                return m_SignedSignificand.Sign;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        private BigDecimal(BigInteger signedSignificand, nint signedShiftCount) {
            this.m_SignedSignificand = signedSignificand;
            this.m_SignedShiftCount = signedShiftCount;
        }
        /*
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator BigDecimal(Rational64 value) {
            return default(Rational64) == value ? default : new BigDecimal(value.Denominator, value.SignedSignificand);
        }
        
        private static readonly BigInteger s_Rational64MaxDenominator = unchecked((UInt32)Int32.MinValue);

        private static readonly BigInteger s_Rational64MaxNumerator = UInt32.MaxValue;
        */

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(BigDecimal value) {
            var d = value.m_SignedShiftCount;
            if (!d.IsZero) {
                if (d <= s_Rational64MaxDenominator) {
                    var n = value.m_SignedSignificand;
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
        public static implicit operator BigDecimal(BigInteger value) {
            return value.IsZero ? default : new BigDecimal(value, 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator BigInteger(BigDecimal value) {
            if (value.m_SignedSignificand.IsZero) {
                return default;
            }
            return value.m_SignedSignificand / value.m_SignedShiftCount;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Double(BigDecimal value) {
            if (value.m_SignedSignificand.IsZero) {
                return (Double)0;
            }
            return (Double)value.m_SignedSignificand / (Double)value.m_SignedShiftCount;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator +(BigDecimal value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator -(BigDecimal value) {
            return new BigDecimal(-value.m_SignedSignificand, value.m_SignedShiftCount );
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator +(BigDecimal first, BigDecimal second) {
            if (first.m_SignedSignificand.IsZero) {
                return second;
            }
            if (second.m_SignedSignificand.IsZero) {
                return first;
            }
            var d = first.m_SignedShiftCount * second.m_SignedShiftCount;
            var n = first.m_SignedSignificand * second.m_SignedShiftCount + first.m_SignedShiftCount * second.m_SignedSignificand;
            if (n.IsZero) {
                return default;
            }
            var g = BigInteger.GreatestCommonDivisor(d, n);
            return new BigDecimal(d / g, n / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator -(BigDecimal first, BigDecimal second) {
            if (first.m_SignedSignificand.IsZero) {
                return -second;
            }
            if (second.m_SignedSignificand.IsZero) {
                return first;
            }
            var d = first.m_SignedShiftCount * second.m_SignedShiftCount;
            var n = first.m_SignedSignificand * second.m_SignedShiftCount - first.m_SignedShiftCount * second.m_SignedSignificand;
            if (n.IsZero) {
                return default;
            }
            var g = BigInteger.GreatestCommonDivisor(d, n);
            return new BigDecimal(d / g, n / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator *(BigDecimal first, BigDecimal second) {
            if (first.m_SignedSignificand.IsZero || second.m_SignedSignificand.IsZero) {
                return default;
            }
            var g0 = BigInteger.GreatestCommonDivisor(first.m_SignedShiftCount, second.m_SignedSignificand);
            var g1 = BigInteger.GreatestCommonDivisor(first.m_SignedSignificand, second.m_SignedShiftCount);
            var d = (first.m_SignedShiftCount / g0) * (second.m_SignedShiftCount / g1);
            var n = (first.m_SignedSignificand / g1) * (second.m_SignedSignificand / g0);
            return new BigDecimal(d, n);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal operator /(BigDecimal first, BigDecimal second) {
            if (second.m_SignedSignificand.IsZero) {
                var ignored = Default<int>.Value / 0;
            }
            if (first.m_SignedSignificand.IsZero) {
                return default;
            }
            var g0 = BigInteger.GreatestCommonDivisor(first.m_SignedShiftCount, second.m_SignedShiftCount);
            var g1 = BigInteger.GreatestCommonDivisor(first.m_SignedSignificand, second.m_SignedSignificand);
            var d = (first.m_SignedShiftCount / g0) * (second.m_SignedSignificand / g1);
            var n = (first.m_SignedSignificand / g1) * (second.m_SignedShiftCount / g0);
            if (0 > d.Sign) {
                d = -d;
                n = -n;
            }
            return new BigDecimal(d, n);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal Invert(BigDecimal value) {
            var s = value.m_SignedSignificand.Sign;
            if (0 == s) {
                var ignored = Default<int>.Value / 0;
            }
            if (value.m_SignedSignificand.IsZero) {
                return default;
            }
            return 0 > s ? new BigDecimal(-value.m_SignedShiftCount, -value.m_SignedSignificand) : new BigDecimal(value.m_SignedShiftCount, value.m_SignedSignificand);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal FromFraction(long numerator, long denominator) {
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
            return new BigDecimal(denominator / g, numerator / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigDecimal FromFraction(BigInteger numerator, BigInteger denominator) {
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
            return new BigDecimal(denominator / g, numerator / g);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator ==(BigDecimal first, BigDecimal second) {
            return first.m_SignedSignificand == second.m_SignedSignificand && first.m_SignedShiftCount == second.m_SignedShiftCount;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator !=(BigDecimal first, BigDecimal second) {
            return first.m_SignedSignificand != second.m_SignedSignificand || first.m_SignedShiftCount != second.m_SignedShiftCount;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override bool Equals(object obj) {
            if (obj is BigDecimal value) {
                return this == value;
            }
            return base.Equals(obj);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override int GetHashCode() {
            return this.m_SignedShiftCount.GetHashCode() ^ this.m_SignedSignificand.GetHashCode();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public bool Equals(BigDecimal other) {
            return this.m_SignedShiftCount.Equals(other.m_SignedShiftCount) && this.m_SignedSignificand.Equals(other.m_SignedSignificand);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public int CompareTo(BigDecimal other) {
            var a = this.m_SignedSignificand.CompareTo(other.m_SignedSignificand);
            return 0 != a ? a : (this.m_SignedSignificand * other.m_SignedShiftCount).CompareTo(this.m_SignedShiftCount * other.m_SignedSignificand);
        }
    }
}

namespace UltimateOrb.Numerics {
    using System.Text;

    public readonly partial struct BigDecimal : IFormattable {

        public override string ToString() {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.m_SignedSignificand);
            var t = this.m_SignedShiftCount;
            if (1 != t) {
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
            sb.Append(this.m_SignedSignificand.ToString(format, formatProvider));
            var t = this.m_SignedShiftCount;
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

    public readonly partial struct BigDecimal {

        public static implicit operator BigDecimal(Single value) => FromSingle(value);

        public static implicit operator BigDecimal(double value) => FromDouble(value);

        public static BigDecimal FromSingle(Single value) {
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
                    return default(BigDecimal);
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

            return new BigDecimal(denominator, numerator);
        }

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(float value) {
            throw new NotFiniteNumberException($"The float value {value} is not finite.");
        }

        public static BigDecimal FromDouble(double value) {
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
                    return default(BigDecimal);
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

            return new BigDecimal(denominator, numerator);
        }

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(double value) {
            throw new NotFiniteNumberException($"The double value {value} is not finite.");
        }

        static BigDecimal ContinuedFraction(double value, int maxIterations) {
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
            return new BigDecimal(denominator, numerator);
        }
        public static BigDecimal FromFloatContinuedFraction(float value, int maxIterations = 13) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            if (float.IsNaN(value) || float.IsInfinity(value)) {
                ThrowNotFiniteNumberException(value);
            }
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigDecimal result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = new BigDecimal(result.Denominator, - result.Numerator);
            }
            return result;
        }

        public static BigDecimal FromDoubleContinuedFraction(double value, int maxIterations = 26) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            if (double.IsNaN(value) || double.IsInfinity(value)) {
                ThrowNotFiniteNumberException(value);
            }
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigDecimal result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = new BigDecimal(result.Denominator , - result.Numerator);
            }
            return result;
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly partial struct BigDecimal
        : INumber<BigDecimal> {

        public static BigDecimal One => new BigDecimal(BigInteger.One, BigInteger.One);

        public static BigDecimal MinusOne => new BigDecimal(BigInteger.One, BigInteger.MinusOne);

        public static int Radix => throw new NotSupportedException();

        public static BigDecimal Zero => default;

        static BigDecimal IAdditiveIdentity<BigDecimal, BigDecimal>.AdditiveIdentity => One;

        static BigDecimal IMultiplicativeIdentity<BigDecimal, BigDecimal>.MultiplicativeIdentity => default;

        public static BigDecimal Abs(BigDecimal value) {
            return new BigDecimal(value.m_SignedShiftCount, BigInteger.Abs(value.m_SignedSignificand));
        }

        public static bool IsCanonical(BigDecimal value) {
            return true;
        }

        public static bool IsComplexNumber(BigDecimal value) {
            return false;
        }

        public static bool IsEvenInteger(BigDecimal value) {
            return (value.m_SignedShiftCount.IsOne && value.m_SignedSignificand.IsEven) || value.m_SignedSignificand.IsZero;
        }

        public static bool IsFinite(BigDecimal value) {
            return true;
        }

        public static bool IsImaginaryNumber(BigDecimal value) {
            return false;
        }

        public static bool IsInfinity(BigDecimal value) {
            return false;
        }

        public static bool IsInteger(BigDecimal value) {
            return value.m_SignedShiftCount.IsOne || value.m_SignedShiftCount.IsZero;
        }

        public static bool IsNaN(BigDecimal value) {
            return false;
        }

        public static bool IsNegative(BigDecimal value) {
            return BigInteger.IsNegative(value.m_SignedSignificand);
        }

        public static bool IsNegativeInfinity(BigDecimal value) {
            return false;
        }

        public static bool IsNormal(BigDecimal value) {
            return !value.m_SignedSignificand.IsZero;
        }

        public static bool IsOddInteger(BigDecimal value) {
            return (value.m_SignedShiftCount.IsOne && !value.m_SignedSignificand.IsEven) || value.m_SignedSignificand.IsZero;
        }

        public static bool IsPositive(BigDecimal value) {
            return BigInteger.IsPositive(value.m_SignedSignificand);
        }

        public static bool IsPositiveInfinity(BigDecimal value) {
            return false;
        }

        public static bool IsRealNumber(BigDecimal value) {
            return true;
        }

        public static bool IsSubnormal(BigDecimal value) {
            return false;
        }

        public static bool IsZero(BigDecimal value) {
            return value.m_SignedSignificand.IsZero;
        }

        public static BigDecimal MaxMagnitude(BigDecimal x, BigDecimal y) {
            var ax = Abs(x);
            var ay = Abs(y);
            return ax > ay ? x : y;
        }

        public static BigDecimal MaxMagnitudeNumber(BigDecimal x, BigDecimal y) {
            return MaxMagnitude(x, y);
        }

        public static BigDecimal MinMagnitude(BigDecimal x, BigDecimal y) {
            var ax = Abs(x);
            var ay = Abs(y);
            return ax < ay ? x : y;
        }

        public static BigDecimal MinMagnitudeNumber(BigDecimal x, BigDecimal y) {
            return MinMagnitude(x, y);
        }

        public static BigDecimal Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigDecimal Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigDecimal result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigDecimal result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigDecimal result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigDecimal result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromChecked<TOther>(TOther value, out BigDecimal result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromSaturating<TOther>(TOther value, out BigDecimal result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertFromTruncating<TOther>(TOther value, out BigDecimal result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertToChecked<TOther>(BigDecimal value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<BigDecimal>.TryConvertToSaturating<TOther>(BigDecimal value, out TOther result) {
            throw new NotImplementedException();
        }

        static readonly BigInteger s_UInt32MaxValueAsBigInteger = UInt32.MaxValue;

        static readonly BigInteger s_UInt64MaxValueAsBigInteger = UInt64.MaxValue;

        static readonly BigInteger s_UInt128MaxValueAsBigInteger = UInt128.MaxValue;

        static readonly BigInteger s_UInt16MaxValueAsBigInteger = UInt16.MaxValue;

        static readonly BigInteger s_ByteMaxValueAsBigInteger = byte.MaxValue;

        static readonly BigInteger s_UIntPtrMaxValueAsBigInteger = UIntPtr.MaxValue;



        static bool INumberBase<BigDecimal>.TryConvertToTruncating<TOther>(BigDecimal value, out TOther result) {
            static BigInteger F(BigDecimal value) {
                if (value.m_SignedSignificand.IsZero) {
                    return value.m_SignedSignificand;
                }
                return value.m_SignedSignificand / value.m_SignedShiftCount;
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
                if (!value.m_SignedSignificand.IsZero) {
                    var p = BigInteger.Abs(value.m_SignedSignificand);
                    var a = p.GetBitLength();
                    Debug.Assert(a > 0);
                    var b = value.m_SignedShiftCount.GetBitLength();
                    Debug.Assert(b > 0);
                    var c = a - b;
                    if (c > 1024) {
                        result = (TOther)(object)(BigInteger.IsNegative(value.m_SignedSignificand) ? double.NegativeInfinity : double.PositiveInfinity);
                        return true;
                    }
                    if (c < -1075) {
                        result = (TOther)(object)(BigInteger.IsNegative(value.m_SignedSignificand) ? -0.0: 0.0);
                        return true;
                    }
                    var c0 = (int)c;
                    var f = ((0 > c0) ?
                        ((p << -c0) < b) :
                        ((p >> c0) < b));
                    c0 -= f ? 1 : 0;
                    var c1 = c0 - 52;
                    var (q, r) = 0 > c1 ?
                        BigInteger.DivRem(p << -c1, value.m_SignedShiftCount) :
                        BigInteger.DivRem(p, value.m_SignedShiftCount << c1);
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

                if (obj is not BigDecimal bigRational) {
                    throw new ArgumentException("The parameter must be a BigDecimal.", nameof(obj));
                }

                return CompareTo(bigRational);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static BigDecimal operator ++(BigDecimal value) {
            return value.m_SignedSignificand.IsZero ? BigDecimal.One : new BigDecimal(value.m_SignedShiftCount, value.m_SignedSignificand + value.m_SignedShiftCount);
        }

        public static BigDecimal operator --(BigDecimal value) {
            return value.m_SignedSignificand.IsZero ? BigDecimal.MinusOne : new BigDecimal(value.m_SignedShiftCount, value.m_SignedSignificand - value.m_SignedShiftCount);
        }

        public static BigDecimal DivideEuclidean(BigDecimal first, BigDecimal second) {
            if (second.m_SignedSignificand.IsZero) {
                _ = Default<int>.Value / 0;
            }
            if (first.m_SignedSignificand.IsZero) {
                return Zero;
            }
            return (first.m_SignedSignificand * second.Denominator) / (first.Denominator * second.m_SignedSignificand);
        }

        public static BigDecimal operator %(BigDecimal first, BigDecimal second) {
            if (second.m_SignedSignificand.IsZero) {
                _ = Default<int>.Value / 0;
            }
            if (first.m_SignedSignificand.IsZero) {
                return Zero;
            }
            var p = (first.m_SignedSignificand * second.Denominator) % (first.Denominator * second.m_SignedSignificand);
            var q = first.Denominator * second.Denominator;
            var d = BigInteger.GreatestCommonDivisor(q, p);
            return new BigDecimal(q / d, p / d);
        }

        public static bool operator <(BigDecimal first, BigDecimal second) {
            return first.CompareTo(second) < 0;
        }

        public static bool operator >(BigDecimal first, BigDecimal second) {
            return first.CompareTo(second) > 0;
        }

        public static bool operator <=(BigDecimal first, BigDecimal second) {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >=(BigDecimal first, BigDecimal second) {
            return first.CompareTo(second) >= 0;
        }
    }
}