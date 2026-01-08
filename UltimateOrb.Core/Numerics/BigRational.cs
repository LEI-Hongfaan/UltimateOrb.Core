using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using UltimateOrb.Mathematics.Exact;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP")]
    [DebuggerDisplay("{ToDebugDisplayString(),nq}")]
    [SerializableAttribute()]
    public readonly partial struct BigRational
        : IEquatable<BigRational>
        , IComparable<BigRational> {

        internal readonly BigInteger m_Denominator;

        internal readonly BigInteger m_SignedNumerator;

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
        internal BigRational(BigInteger denominator, BigInteger signedNumerator) {
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

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(int value) {
            return 0 == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(uint value) {
            return 0u == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(nint value) {
            return 0 == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(nuint value) {
            return 0u == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(long value) {
            return 0 == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigRational(ulong value) {
            return 0u == value ? default : new BigRational(denominator: s_BigIntegerOne, signedNumerator: value);
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
            return ToIeee754InterchangeableBinary<Double, UInt64>(value);
        }

        internal static TFloat ToIeee754InterchangeableBinary<TFloat, TFloatUIntBits>(BigRational value)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {
            if (value.m_SignedNumerator.IsZero) {
                return TFloat.Zero;
            }
            return DivideToIeee754InterchangeableBinaryInternal<TFloat, TFloatUIntBits>(value.m_SignedNumerator, value.m_Denominator);
        }

        static TFloat DivideToIeee754InterchangeableBinaryInternal<TFloat, TFloatUIntBits>(BigInteger signedNumerator, BigInteger denominator)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {

            Debug.Assert(denominator.Sign > 0);
            bool negative = BigInteger.IsNegative(signedNumerator);
            BigInteger absDividend = BigInteger.Abs(signedNumerator);
            BigInteger absDivisor = denominator;

            // Check for overflow
            var dividendBits = checked((int)absDividend.GetBitLength());
            var divisorBits = checked((int)absDivisor.GetBitLength());
            var o = BigInteger.TrailingZeroCount(absDividend);
            var p = BigInteger.TrailingZeroCount(absDivisor);

            var exponent = unchecked(dividendBits - divisorBits);
            if (exponent >= 0) {
                var t = absDividend >> exponent;
                if (t < absDivisor) {
                    unchecked {
                        --exponent;
                    }
                }
            } else {
                var t = absDivisor >> unchecked(-exponent);
                var a = absDividend.CompareTo(t);
                if (a < 0 || (a == 0 && p < unchecked(-exponent))) {
                    unchecked {
                        --exponent;
                    }
                }
            }
            var FloatBitSize = 8 * Unsafe.SizeOf<TFloat>();
            var SignificandBitLength = TFloat.MinValue.GetSignificandBitLength() - 1;
            var ExponentBitLength = FloatBitSize - 1 - SignificandBitLength;
            var ExponentBias = (1 << (ExponentBitLength - 1)) - 1;
            var ExponentMinValue = -ExponentBias + 1;
            var ExponentUnderflowZeroBoundExclusive = -ExponentBias - SignificandBitLength;


            if (exponent > ExponentBias) {
                return negative ? TFloat.NegativeInfinity : TFloat.PositiveInfinity;
            }

            // Check for underflow
            if (exponent < ExponentUnderflowZeroBoundExclusive) {
                return negative ? TFloat.NegativeZero : TFloat.Zero;
            }

            exponent = exponent < ExponentMinValue ? ExponentMinValue : exponent;

            // Compute the scaled exponent
            var e = unchecked((int)(exponent - (long)SignificandBitLength));
            bool h = default!;
            var f = unchecked(e - 1);
            if (e > 0) {
                h = !((BigInteger.One << f) & absDividend).IsZero;
                absDividend >>= e;
            } else {
                absDividend <<= unchecked(-e);
            }

            var (q, r) = BigInteger.DivRem(absDividend, absDivisor);

            int w = 0;
            r <<= 1;
            if (e > 0) {
                if (h) {
                    ++r;
                }
                w = (o == f) ? 0 : 1;
            }
            w |= r.CompareTo(absDivisor);
            if (w > 0 || (w == 0 && !q.IsEven)) {
                ++q;
            }

            // Construct the double
            var biasedExponent = TFloatUIntBits.CreateTruncating(exponent + (ExponentBias - 1)) << SignificandBitLength;
            var s = TFloatUIntBits.CreateTruncating(q);

            var result = unchecked((TFloatUIntBits.CreateTruncating(negative ? 1u : 0u) << (FloatBitSize - 1)) + (biasedExponent + s));

            return Unsafe.BitCast<TFloatUIntBits, TFloat>(result);
        }

        static double DivideToDoubleInternal(BigInteger signedNumerator, BigInteger denominator) {
            Debug.Assert(denominator.Sign > 0);
            bool negative = BigInteger.IsNegative(signedNumerator);
            BigInteger absDividend = BigInteger.Abs(signedNumerator);
            BigInteger absDivisor = denominator;

            // Check for overflow
            var dividendBits = checked((int)absDividend.GetBitLength());
            var divisorBits = checked((int)absDivisor.GetBitLength());
            var o = BigInteger.TrailingZeroCount(absDividend);
            var p = BigInteger.TrailingZeroCount(absDivisor);

            var exponent = unchecked(dividendBits - divisorBits);
            if (exponent >= 0) {
                var t = absDividend >> exponent;
                if (t < absDivisor) {
                    unchecked {
                        --exponent;
                    }
                }
            } else {
                var t = absDivisor >> unchecked(-exponent);
                var a = absDividend.CompareTo(t);
                if (a < 0 || (a == 0 && p < unchecked(-exponent))) {
                    unchecked {
                        --exponent;
                    }
                }
            }

            if (exponent > 1023) {
                return negative ? double.NegativeInfinity : double.PositiveInfinity;
            }

            // Check for underflow
            if (exponent < -1075) {
                return negative ? -0.0 : 0.0;
            }

            exponent = exponent < -1022 ? -1022 : exponent;

            // Compute the scaled exponent
            var e = unchecked((int)(exponent - (long)52));
            bool h = default!;
            var f = unchecked(e - 1);
            if (e > 0) {
                h = !((BigInteger.One << f) & absDividend).IsZero;
                absDividend >>= e;
            } else {
                absDividend <<= unchecked(-e);
            }

            var (q, r) = BigInteger.DivRem(absDividend, absDivisor);

            int w = 0;
            r <<= 1;
            if (e > 0) {
                if (h) {
                    ++r;
                }
                w = (o == f) ? 0 : 1;
            }
            w |= r.CompareTo(absDivisor);
            if (w > 0 || (w == 0 && !q.IsEven)) {
                ++q;
            }

            // Construct the double
            var biasedExponent = (UInt64)(exponent + (1023 - 1)) << 52;
            var s = (UInt64)q;
            var result = unchecked(((negative ? (UInt64)1u : 0u) << 63) + (biasedExponent + s));

            return BitConverter.Int64BitsToDouble(unchecked((Int64)result));
        }

        static double ToDoubleInternal(BigInteger value) {
            if (value.IsZero) {
                return 0.0;
            }
            bool negative = BigInteger.IsNegative(value);
            BigInteger absDividend = negative ? -value : value;

            var dividendBits = checked((int)absDividend.GetBitLength());
            var o = BigInteger.TrailingZeroCount(absDividend);

            var exponent = unchecked(dividendBits - 1);

            if (exponent > 1023) {
                return negative ? double.NegativeInfinity : double.PositiveInfinity;
            }

            // Compute the scaled exponent
            var e = unchecked((int)(exponent - (long)52));
            bool h = false;
            var f = unchecked(e - 1);
            UInt64 q;
            if (e > 0) {
                // 
                /*
                h = !((BigInteger.One << f) & absDividend).IsZero;
                absDividend >>= e;
                den = UInt64.CreateTruncating(absDividend);
                */
                // TODO: Cleanup this after BigInteger.TestBit is available.
                absDividend >>= f;
                q = UInt64.CreateTruncating(absDividend);
                h = (0 != (1 & q));
                q >>= 1;
            } else {
                q = UInt64.CreateTruncating(absDividend) << unchecked(-e);
            }

            //if (!h && ((e > 0 && o != f) || (((e > 0 && o == f) || e <= 0) && !den.IsEven))) {
            if (!h && (!(0 == (1 & q)) || (e > 0 && o != f))) {
                ++q;
            }

            // Construct the double
            var biasedExponent = (UInt64)(exponent + (1023 - 1)) << 52;
            var result = unchecked(((negative ? (UInt64)1u : 0u) << 63) + (biasedExponent + q));

            return BitConverter.Int64BitsToDouble(unchecked((Int64)result));
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
                _ = Default<int>.Value / 0;
            }
            if (value.m_SignedNumerator.IsZero) {
                return default;
            }
            return 0 > s ? new BigRational(-value.m_SignedNumerator, -value.m_Denominator) : new BigRational(value.m_SignedNumerator, value.m_Denominator);
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
            var a = this.m_SignedNumerator.Sign.CompareTo(other.m_SignedNumerator.Sign);
            return 0 != a ? a : (this.m_SignedNumerator * other.m_Denominator).CompareTo(this.m_Denominator * other.m_SignedNumerator);
        }
    }
}

namespace UltimateOrb.Numerics {
    using System.Text;

    public readonly partial struct BigRational : IFormattable {

        public override string ToString() {
            return ToString(null, null);
        }

        [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "get_DebuggerDisplay")]
        static extern string GetDebuggerDisplay(in BigInteger value);

        string ToDebugDisplayString() {
            return $@"({GetDebuggerDisplay(SignedNumerator)})/({GetDebuggerDisplay(Denominator)})";
        }

        public string ToString(IFormatProvider? formatProvider) {
            return this.ToString(null, formatProvider);
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(38);
            sb.Append(this.m_SignedNumerator.ToString(format, formatProvider));
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
                sb.Insert(i + 2, t.ToString(format, formatProvider));
            }
            return sb.ToString();
        }
    }
}



namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational {

        public static explicit operator BigRational(Single value) => FromIeee754InterchangeableBinary<Single, UInt32>(value);

        public static explicit operator BigRational(double value) => FromIeee754InterchangeableBinary<double, UInt64>(value);

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

        internal static BigRational FromIeee754InterchangeableBinary<TFloat, TFloatUIntBits>(TFloat value)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {
            int FloatBitSize = 8 * Unsafe.SizeOf<TFloat>();
            int SignificandBitLength = TFloat.MinValue.GetSignificandBitLength() - 1; // includes implicit bit
            int ExponentBitLength = FloatBitSize - 1 - SignificandBitLength;
            int ExponentBias = (1 << (ExponentBitLength - 1)) - 1;

            // Raw bits as unsigned integer type
            TFloatUIntBits bits = Unsafe.BitCast<TFloat, TFloatUIntBits>(value);

            // Extract exponent field
            var exponentAllOnes = (TFloatUIntBits.One << ExponentBitLength) - TFloatUIntBits.One;
            var exponentMask = exponentAllOnes << SignificandBitLength;
            var exponentFieldUInt = (bits >> SignificandBitLength) & exponentAllOnes;

            // Check NaN/Infinity (all-ones exponent)
            if (exponentFieldUInt == exponentAllOnes) {
                ThrowNotFiniteNumberException(value);
            }

            // Sign
            bool isNegative = !TFloatUIntBits.IsZero(bits & (TFloatUIntBits.One << (FloatBitSize - 1)));

            // Mantissa (fraction) field
            var mantissaMask = (TFloatUIntBits.One << SignificandBitLength) - TFloatUIntBits.One;
            var mantissaUInt = bits & mantissaMask;

            // Convert exponent field to int using generic CreateTruncating
            int exponent = int.CreateTruncating(exponentFieldUInt);

            if (exponent == 0) {
                // Subnormal number or zero
                if (TFloatUIntBits.IsZero(mantissaUInt)) {
                    return default(BigRational);
                }
                // exponent = 1 - bias - mantissaBits
                exponent = 1 - ExponentBias - SignificandBitLength;
            } else {
                // Normalized number
                // Set implicit leading 1
                mantissaUInt |= TFloatUIntBits.One << SignificandBitLength;

                exponent -= ExponentBias + SignificandBitLength;
            }

            int trailingZeros = int.CreateTruncating(TFloatUIntBits.TrailingZeroCount(mantissaUInt));
            mantissaUInt >>= trailingZeros;
            exponent += trailingZeros;

            BigInteger numerator = BigInteger.CreateTruncating(mantissaUInt);
            BigInteger denominator;

            if (exponent > 0) {
                numerator <<= exponent;
                denominator = BigInteger.One;
            } else {
                denominator = BigInteger.One << -exponent;
            }

            if (isNegative) {
                numerator = BigInteger.Negate(numerator);
            }

            return new BigRational(denominator: denominator, signedNumerator: numerator);

        }

        static void ThrowNotFiniteNumberException<T>(T value) {
            throw new NotFiniteNumberException($"The {typeof(T).FullName} value {value} is not finite.");
        }

        /*
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
        */

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(double value) {
            throw new NotFiniteNumberException($"The double value {value} is not finite.");
        }

        static BigRational ContinuedFraction(double value, int maxIterations) {
            Debug.Assert(value > 0);
            BigInteger d = 0;
            BigInteger n = 1;
            BigInteger prevD = 1;
            BigInteger prevN = 0;
            int i = 0;
            for (; i < maxIterations; i++) {
                if (!double.IsFinite(value)) {
                    break;
                }

                BigInteger integerPart = (BigInteger)value;
                value -= (double)integerPart;
                value = 1.0 / value;

                BigInteger tempD = d;
                d = integerPart * d + prevD;
                prevD = tempD;

                BigInteger tempN = n;
                n = integerPart * n + prevN;
                prevN = tempN;
            }
            return BigRational.FromFraction(n, d);
        }

        /// <summary>
        /// Finds a rational value close to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxIterations"></param>
        /// <exception cref="ArithmeticException"><paramref name="value"/> is not a finite number.</exception>
        /// <exception cref="OutOfMemoryException">The denominator or numerator of the result goes too large.</exception>
        /// <returns></returns>
        public static BigRational FromSingleByContinuedFraction(Single value, int maxIterations = 12) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            CilVerifiable.CheckFinite(value);
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigRational result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = -result;
            }
            return result;
        }

        /// <summary>
        /// Finds a rational value close to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxIterations"></param>
        /// <exception cref="ArithmeticException"><paramref name="value"/> is not a finite number.</exception>
        /// <exception cref="OutOfMemoryException">The denominator or numerator of the result goes too large.</exception>
        /// <returns></returns>
        public static BigRational FromDoubleByContinuedFraction(double value, int maxIterations = 17) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            CilVerifiable.CheckFinite(value);
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigRational result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = -result;
            }
            return result;
        }

        static BigRational ContinuedFraction(BigRational value, int maxIterations) {
            Debug.Assert(value > 0);
            BigInteger d = 0;
            BigInteger n = 1;
            BigInteger prevD = 1;
            BigInteger prevN = 0;
            int i = 0;
            for (; i < maxIterations; i++) {

                BigInteger integerPart = (BigInteger)value;

                BigInteger tempNumerator = d;
                d = integerPart * d + prevD;
                prevD = tempNumerator;

                BigInteger tempDenominator = n;
                n = integerPart * n + prevN;
                prevN = tempDenominator;

                if (value.m_Denominator.IsOne) {
                    break;
                }

                value -= integerPart;
                value = BigRational.Invert(value);
            }
            return BigRational.FromFraction(n, d);
        }

        /// <summary>
        /// Finds a rational value close to the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxIterations"></param>
        /// <exception cref="ArithmeticException"><paramref name="value"/> is not a finite number.</exception>
        /// <exception cref="OutOfMemoryException">The denominator or numerator of the result goes too large.</exception>
        /// <returns></returns>
        public static BigRational FromRationalByContinuedFraction(BigRational value, int maxIterations = 17) {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxIterations);
            bool isNegative = value < 0;
            if (isNegative) {
                value = -value;
            }
            BigRational result = ContinuedFraction(value, maxIterations);
            if (isNegative) {
                result = -result;
            }
            return result;
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational {

        public static implicit operator BigRational(decimal value) {
            ref Win32Decimal v = ref Unsafe.As<decimal, Win32Decimal>(ref value);
            var e = unchecked((byte)(v._flags >> 16));
            var m = new System.UInt128(upper: v._hi32, lower: v._lo64);
            var q = BigIntegerSmallExp10Module.Exp10(e);
            var p = (BigInteger)m;
            var g = BigInteger.GreatestCommonDivisor(p, q);
            if (!g.IsOne) {
                q /= g;
                p /= g;
            }
            if (0 > v._flags) {
                p = -p;
            }
            return new BigRational(denominator: q, signedNumerator: p);
        }

        public static explicit operator decimal(BigRational value) {
            if (TryConvertToDecimal(value, out var r)) {
                return r;
            }

            throw new OverflowException("Value does not fit into a System.Decimal.");
        }

        private static bool TryConvertToDecimal(BigRational value, out decimal result) {
            if (BigRational.IsZero(value)) {
                result = 0m;
                return true;
            }

            // Extract numerator and denominator
            var p = value.SignedNumerator;
            var q = value.Denominator;

            // Normalize sign and absolute numerator
            bool negative = p.Sign < 0;
            if (negative) p = -p;

            // Try scales 0..28 (decimal supports up to 28 decimal places)
            for (int scale = 28; scale >= 0; --scale) {
                var Exp10 = BigIntegerSmallExp10Module.Exp10(scale);
                var scaled = p * Exp10;

                // Division with remainder
                BigInteger div = BigInteger.DivRem(scaled, q, out BigInteger rem);

                // If remainder != 0, perform round-to-nearest, ties to even (banker's rounding)
                if (rem != BigInteger.Zero) {
                    var twiceRem = rem << 1; // rem * 2
                    int cmp = BigInteger.Compare(twiceRem, q);
                    if (cmp > 0) {
                        // round up
                        div += BigInteger.One;
                    } else if (cmp == 0) {
                        // tie: round to even
                        if (!div.IsEven) div += BigInteger.One;
                    }
                }

                // Check fit into 96 bits
                if (div.GetByteCount(isUnsigned: true) <= 12) {
                    // Build decimal from 96-bit integer parts
                    var lo = (UInt32)(div & 0xFFFFFFFF);
                    var mid = (UInt32)((div >> 32) & 0xFFFFFFFF);
                    var hi = (UInt32)((div >> 64) & 0xFFFFFFFF);

                    // decimal ctor: (int lo, int mid, int hi, bool isNegative, byte scale)
                    result = unchecked(new decimal((Int32)lo, (Int32)mid, (Int32)hi, negative, (byte)scale));
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly partial struct BigRational {

        public static partial class Math {

            public static int ILog10(BigRational value) {
                var s = value.Sign;
                if (s < 0) {
                    return ILogSpecialResults.ILogNaN;
                }
                if (s == 0) {
                    return ILogSpecialResults.ILog0;
                }
                var den = value.m_Denominator;
                var num = value.m_SignedNumerator;
                Debug.Assert(den > 0);
                Debug.Assert(num > 0);
                if (num >= den) {
                    return BigIntegerMath.ILog10(num / den);
                } else {
                    var (q, r) = BigInteger.DivRem(den, num);
                    // q = floor(den/num), r = den % num
                    // q >= 1 because num < den
                    var d = BigIntegerMath.ILog10(q); // floor(log10(q))
                    if (d == ILogSpecialResults.ILogNaN) {
                        // overflow
                        return d;
                    }
                    Debug.Assert(d >= 0);
                    // r != 0 ->den/num is strictly greater than q (non-integer), so ceil(log10(den/num)) == d+1
                    // r == 0 -> den is divisible by num, t == q is an integer
                    // If q is exactly 10^d then log10(t) == d and floor(log10(value)) == -d
                    // otherwise log10(t) in (d, d+1) so floor(log10(value)) == -(d+1)
                    return r.IsZero && BigIntegerSmallExp10Module.Exp10(d) == q ? unchecked(-d) : unchecked(-(d + 1));
                }
            }

            public static BigRational Floor(BigRational value) {
                var quotient = BigInteger.DivRem(value.SignedNumerator, value.Denominator, out var remainder);
                if (BigInteger.IsNegative(remainder)) {
                    --quotient;
                }
                return quotient;
            }

            public static BigRational Round(BigRational value, int decimals, MidpointRounding mode = MidpointRounding.ToEven) {
                BigInteger num = value.SignedNumerator;
                BigInteger den = value.Denominator;

                // Scale numerator or denominator depending on decimals sign
                BigInteger scaledNum = num;
                BigInteger scaledDen = den;
                if (decimals == 0) {
                } else if (decimals >= 0) {
                    var factor = BigIntegerSmallExp10Module.Exp10(decimals);
                    scaledNum = num * factor;
                } else {
                    var factor = BigIntegerSmallExp10Module.Exp10(-decimals);
                    scaledDen = den * factor;
                }

                // integer division with remainder
                BigInteger q = BigInteger.DivRem(scaledNum, scaledDen, out BigInteger r);

                BigInteger absR = BigInteger.Abs(r);
                BigInteger absDen = BigInteger.Abs(scaledDen);
                int cmp = (absR * 2).CompareTo(absDen);

                bool increment = false;
                if (cmp > 0) {
                    increment = true; // > 0.5
                } else if (cmp < 0) {
                    increment = false; // < 0.5
                } else // tie: exactly .5
                  {
                    switch (mode) {
                    case MidpointRounding.ToEven:
                        // increment if den is odd
                        increment = (q & 1) != 0;
                        break;
                    case MidpointRounding.AwayFromZero:
                        increment = true;
                        break;
                    case MidpointRounding.ToZero:
                        increment = false;
                        break;
                    case MidpointRounding.ToNegativeInfinity:
                        increment = (scaledNum.Sign < 0);
                        break;
                    case MidpointRounding.ToPositiveInfinity:
                        increment = (scaledNum.Sign > 0);
                        break;
                    default:
                        ThrowHelper.ThrowArgumentException_InvalidEnumValue(mode);
                        return default;
                    }
                }

                if (increment) {
                    q += scaledNum.Sign >= 0 ? BigInteger.One : BigInteger.Negate(BigInteger.One);
                }

                // Build result and reduce
                BigInteger resultNum, resultDen;
                if (decimals >= 0) {
                    resultNum = q;
                    resultDen = BigIntegerSmallExp10Module.Exp10(decimals);
                } else {
                    var factor = BigIntegerSmallExp10Module.Exp10(-decimals);
                    resultNum = q * factor;
                    resultDen = BigInteger.One;
                }

                return BigRational.FromFraction(resultNum, resultDen);
            }

            public static BigRational Round(BigRational value, MidpointRounding mode = MidpointRounding.ToEven) {
                return RoundToBigInteger(value, mode);
            }

            public static BigInteger RoundToBigInteger(BigRational value, MidpointRounding mode = MidpointRounding.ToEven) {
                BigInteger num = value.SignedNumerator;
                if (IsInteger(value)) {
                    return num;
                }
                BigInteger den = value.Denominator;

                // integer division with remainder
                BigInteger q = BigInteger.DivRem(num, den, out BigInteger r);

                BigInteger absR = BigInteger.Abs(r);
                BigInteger absDen = BigInteger.Abs(den);
                int cmp = (absR * 2).CompareTo(absDen);

                bool increment = false;
                if (cmp > 0) {
                    increment = true; // > 0.5
                } else if (cmp < 0) {
                    increment = false; // < 0.5
                } else // tie: exactly .5
                  {
                    switch (mode) {
                    case MidpointRounding.ToEven:
                        // increment if den is odd
                        increment = (q & 1) != 0;
                        break;
                    case MidpointRounding.AwayFromZero:
                        increment = true;
                        break;
                    case MidpointRounding.ToZero:
                        increment = false;
                        break;
                    case MidpointRounding.ToNegativeInfinity:
                        increment = (num.Sign < 0);
                        break;
                    case MidpointRounding.ToPositiveInfinity:
                        increment = (num.Sign > 0);
                        break;
                    default:
                        ThrowHelper.ThrowArgumentException_InvalidEnumValue(mode);
                        return default;
                    }
                }

                if (increment) {
                    q += num.Sign >= 0 ? BigInteger.One : BigInteger.MinusOne;
                }

                return q;
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
            var di = s.IndexOf('/');
            var errDivByZero = false;
            try {
                if (di > 0) {
                    var a = s.Slice(0, di);
                    var b = s.Slice(di + 1);
                    var parseResultA = BigInteger.Parse(a, provider);
                    var parseResultB = BigInteger.Parse(b, provider);
                    errDivByZero = parseResultB.IsZero;
                    return BigRational.FromFraction(parseResultA, parseResultB);
                } else {
                    return BigInteger.Parse(s, provider);
                }
            } catch (FormatException) {
            } catch (ArithmeticException) {
                if (errDivByZero) {
                    throw;
                }
            }
            {
                var parseResult = NumberLiteralParseModule.ParseNumberLiteral(s.ToString());
                var kind = parseResult.Flags.GetKind();
                if (kind == NumberLiteralFlags.Empty || kind == NumberLiteralFlags.Error) {
                    throw new FormatException("Input string was not in a recognized format.");
                }
                if (kind != NumberLiteralFlags.IsFinite) {
                    var sp = parseResult.Flags.GetSpecial();
                    ThrowNotFiniteNumberException(sp == NumberLiteralFlags.SpecialInfinity ?
                        (parseResult.Flags.HasFlag(NumberLiteralFlags.IsNegative) ? double.NegativeInfinity : double.PositiveInfinity) :
                        double.NaN);
                }
                {
                    if (parseResult.SignificandFractionalPart.IsZero ||
                        parseResult.SignificandIntegralPart.IsZero) {
                        return Zero;
                    }
                    BigInteger denominator;
                    BigInteger numerator;
                    if (parseResult.Flags.HasFlag(NumberLiteralFlags.Hex)) {
                        Debug.Assert(parseResult.SignificandFractionalPartLength >= 0);
                        int fracLen = (int)parseResult.SignificandFractionalPartLength;
                        BigInteger pow16 = BigInteger.Pow(16, fracLen);
                        numerator = parseResult.SignificandIntegralPart * pow16 + parseResult.SignificandFractionalPart;
                        denominator = pow16;

                        var exp = checked((int)parseResult.Exponent);
                        if (exp >= 0) {
                            numerator <<= exp;
                        } else {
                            denominator <<= checked(-exp);
                        }

                       
                    } else {
                        // Decimal path
                        int fracLen = checked((int)parseResult.SignificandFractionalPartLength);
                        BigInteger pow10 = BigInteger.Pow(10, fracLen);
                        numerator = parseResult.SignificandIntegralPart * pow10 + parseResult.SignificandFractionalPart;
                        denominator = pow10;

                        var exp = checked((int)parseResult.Exponent);
                        if (exp >= 0) {
                            numerator *= BigIntegerSmallExp10Module.Exp10(exp);
                        } else {
                            denominator *= BigIntegerSmallExp10Module.Exp10(-exp);
                        }
                    }
                    var br = BigRational.FromFraction(numerator, denominator);
                    if (parseResult.Flags.HasFlag(NumberLiteralFlags.IsNegative)) br = -br;
                    return br;
                }
            }
        }

        public static BigRational Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Parse(s.AsSpan(), style, provider);
        }

        public static BigRational Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Any, provider);
        }

        public static BigRational Parse(string s, IFormatProvider? provider) {
            return Parse(s.AsSpan(), provider);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            // TODO: Implement proper TryParse logic
            try {
                result = Parse(s, style, provider);
                return true;
            } catch (FormatException) {
                result = default;
                return false;
            } catch (ArithmeticException) {
                result = default;
                return false;
            }
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            return TryParse(s, style, provider, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            return TryParse(s, NumberStyles.Any, provider, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out BigRational result) {
            return TryParse(s.AsSpan(), provider, out result);
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
                Debug.Assert(value.m_Denominator >= BigInteger.Zero);
                if (value.m_Denominator <= BigInteger.One) {
                    Debug.Assert(value.m_Denominator.IsOne || value.m_SignedNumerator.IsZero);
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
                        result = (TOther)(object)(BigInteger.IsNegative(value.m_SignedNumerator) ? -0.0 : 0.0);
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
            // TODO:
            var s = ToString(format.ToString(), provider) ?? "";
            if (s.Length <= destination.Length) {
                s.AsSpan().CopyTo(destination);
                charsWritten = s.Length;
                return true;
            } else {
                charsWritten = 0;
                return false;
            }
        }

        public static BigRational operator ++(BigRational value) {
            return value.m_SignedNumerator.IsZero ? BigRational.One : new BigRational(value.m_Denominator, value.m_SignedNumerator + value.m_Denominator);
        }

        public static BigRational operator --(BigRational value) {
            return value.m_SignedNumerator.IsZero ? BigRational.MinusOne : new BigRational(value.m_Denominator, value.m_SignedNumerator - value.m_Denominator);
        }

        public static BigRational CopySign(BigRational value, BigRational sign) {
            return new BigRational(value.m_Denominator, BigInteger.CopySign(value.m_SignedNumerator, sign.m_SignedNumerator));
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
            if (p.IsZero) {
                return Zero;
            }
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

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP")]
    public static partial class BigRationalExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble(this BigRational value) {
            return (Double)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Double ToDoubleEstimate(this BigRational value) {
            if (value.m_SignedNumerator.IsZero) {
                return (Double)0;
            }
            return (double)value.m_SignedNumerator / (double)value.m_Denominator;
        }
    }
}

namespace UltimateOrb.Numerics {

    partial struct BigRational : IConvertible {

        TypeCode IConvertible.GetTypeCode() {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return !IsZero(this);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            return checked((byte)Math.Round(this).Numerator);
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, nameof(BigInteger), nameof(Char)));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, nameof(BigInteger), nameof(DateTime)));
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return checked((decimal)this);
        }

        double IConvertible.ToDouble(IFormatProvider? provider) {
            return (double)this;
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            return checked((Int16)Math.Round(this).Numerator);
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            return checked((Int32)Math.Round(this).Numerator);
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            return checked((Int64)Math.Round(this).Numerator);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            return checked((sbyte)Math.Round(this).Numerator);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return (Single)this;
        }

        string IConvertible.ToString(IFormatProvider? provider) {
            // TODO:
            return ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) {
            if (typeof(BigInteger) == conversionType) {
                return Math.Round(this).Numerator;
            }
            return ConvertInternal.DefaultToType<BigRational>(in this, conversionType, provider);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            return checked((UInt16)Math.Round(this).Numerator);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            return checked((UInt32)Math.Round(this).Numerator);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            return checked((UInt64)Math.Round(this).Numerator);
        }
    }
}
