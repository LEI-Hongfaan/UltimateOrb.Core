using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UltimateOrb.Mathematics.Exact;

namespace UltimateOrb {

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
                if (q.IsZero) {
                    return 1;
                }
                return 0 > m_SignedNumerator.Sign ? -q : q;
            }
        }

        public int Sign {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                if (m_Denominator.IsZero) {
                    return 0;
                }
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator BigRational(Rational64 value) {
            return default(Rational64) == value ? default : new BigRational(value.Denominator, value.SignedNumerator);
        }

        private static readonly BigInteger s_Rational64MaxDenominator = unchecked((UInt32)Int32.MinValue);

        private static readonly BigInteger s_Rational64MaxNumerator = UInt32.MaxValue;

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator BigRational(BigInteger value) {
            return value.IsZero ? default : new BigRational(value, s_BigIntegerOne);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator BigInteger(BigRational value) {
            if (value.m_Denominator.IsZero) {
                return default;
            }
            return value.m_SignedNumerator / value.m_Denominator;
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Double(BigRational value) {
            if (value.m_Denominator.IsZero) {
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational operator -(BigRational value) {
            return new BigRational(value.m_Denominator, -value.m_SignedNumerator);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static BigRational Invert(BigRational value) {
            var s = value.m_SignedNumerator.Sign;
            if (0 == s) {
                var ignored = Default<int>.Value / 0;
            }
            if (value.m_Denominator.IsZero) {
                return default;
            }
            return 0 > s ? new BigRational(-value.m_Denominator, -value.m_SignedNumerator) : new BigRational(value.m_Denominator, value.m_SignedNumerator);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
            return this == other;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public int CompareTo(BigRational other) {
            var a = this.m_SignedNumerator.Sign.CompareTo(other.m_SignedNumerator.Sign);
            if (0 != a) {
                return a;
            }
            return (this.m_SignedNumerator * other.m_Denominator - this.m_Denominator * other.m_SignedNumerator).Sign;
        }
    }
}

namespace UltimateOrb {
    using System.Text;

    public readonly partial struct BigRational : IFormattable {

        public override string ToString() {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.m_SignedNumerator);
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
                sb.Insert(i + 2, t);
            }
            return sb.ToString();
        }

        public string ToString(IFormatProvider formatProvider) {
            return this.ToString(null, formatProvider);
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