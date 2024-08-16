using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using UltimateOrb.Utilities;
using UltimateOrb.Mathematics.NumberTheory;
using static UltimateOrb.Utilities.ThrowHelper;

namespace UltimateOrb.Mathematics.Exact {

    [ComVisibleAttribute(true)]
    [SerializableAttribute()]
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 8)]
    public readonly partial struct Rational64 : IComparable<Rational64>, IEquatable<Rational64> {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Int64 bits;

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        internal Rational64(UInt64 bits) {
            this.bits = unchecked((Int64)bits);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 FromFraction(UInt32 numerator, Int32 denominator) {
            Contract.EnsuresOnThrow<DivideByZeroException>(0 == Contract.OldValue(denominator));
            unchecked {
                if (0 == denominator) {
                    _ = numerator / denominator;
                }
                if (0 == numerator) {
                    return default(Rational64);
                }
                var d = EuclideanAlgorithm.GreatestCommonDivisorPartial(numerator, Mathematics.Elementary.Math.AbsAsUnsigned(denominator));
                numerator /= d;
                denominator /= (Int32)d;
                denominator -= BooleanIntegerModule.GreaterThanOrEqual(denominator, 0);
                return new Rational64((UInt64)denominator << 32 | numerator);
            }
        }

#if DEBUG
        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public Rational64(UInt32 numerator, Int32 denominator) {
            this = Rational64.FromFraction(numerator, denominator);
        }
#endif

        [PureAttribute()]
        public static Rational64 Zero {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                return default(Rational64);
            }
        }

        [PureAttribute()]
        public Int64 Denominator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                Contract.Ensures(Contract.Result<Int64>() > 0);
                var c = unchecked((Int32)(bits >> 32));
                if (0 <= unchecked((Int32)c)) {
                    return unchecked((UInt32)(++c));
                } else {
                    return unchecked((UInt32)(-c));
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [PureAttribute()]
        public Int64 Numerator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                Contract.Ensures(0 <= Contract.Result<Int64>());
                return unchecked((UInt32)bits);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [PureAttribute()]
        public int Sign {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var c = bits;
                if (c > 0) {
                    return 1;
                } else {
                    return unchecked((int)(c >> (64 - 1)));
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [PureAttribute()]
        public Int64 SignedDenominator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var c = bits;
                if (0 == unchecked((Int32)c)) {
                    return 0;
                }
                c = unchecked((Int32)(c >> 32));
                return 0 <= unchecked((Int32)c) ?
                    unchecked(1u + (UInt32)c) :
                    unchecked((Int32)c);
            }
        }

        [PureAttribute()]
        public Int64 SignedNumerator {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [TargetedPatchingOptOutAttribute("")]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            [PureAttribute()]
            get {
                var c = bits;
                if (0 > c) {
                    return unchecked(-(Int64)(UInt32)bits);
                } else {
                    return unchecked((UInt32)bits);
                }
            }
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 Inverse(Rational64 value) {
            var d = unchecked((UInt32)value.bits);
            var c = unchecked((Int32)(value.bits >> 32));
            if (0 != d) {
                {
                    var ignored = checked(d - unchecked((UInt32)Int32.MinValue));
                }
                if (0 <= c) {
                    unchecked {
                        ++c;
                    }
                    unchecked {
                        --d;
                    }
                } else {
                    unchecked {
                        c = -c;
                    }
                }
                return new Rational64(unchecked((UInt64)(((Int64)c << 32) | (Int64)(Int32)d)));
            }
            {
                var ignored = c / unchecked((Int32)d);
            }
            return default(Rational64);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 Negate(Rational64 value) {
            var d = unchecked((UInt32)(value.bits));
            var c = unchecked((Int32)(value.bits >> 32));
            if (0u == d) {
                return Rational64.Zero;
            } else {
                c = 0 > c ? ~c : -c;
                return new Rational64(unchecked((UInt64)((Int64)c << 32) | d));
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator -(Rational64 value) {
            return Rational64.Negate(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator !=(Rational64 first, Rational64 second) {
            return first.bits != second.bits;
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 Multiply(Rational64 first, Rational64 second) {
            unchecked {
                var first_numerator = (UInt32)first.bits;
                var first_denominator = (Int32)(first.bits >> 32);
                var second_numerator = (UInt32)second.bits;
                var second_denominator = (Int32)(second.bits >> 32);
                if (0 == first_numerator || 0 == second_numerator) {
                    return Zero;
                }
                var s = 0 > (first_denominator ^ second_denominator);
                if (0 <= first_denominator) {
                    ++first_denominator;
                } else {
                    first_denominator = -first_denominator;
                }
                if (0 <= second_denominator) {
                    ++second_denominator;
                } else {
                    second_denominator = -second_denominator;
                }
                {
                    var d = EuclideanAlgorithm.GreatestCommonDivisorPartial((UInt32)first_denominator, second_numerator);
                    first_denominator = (Int32)((UInt32)first_denominator / d);
                    second_numerator /= d;
                }
                {
                    var d = EuclideanAlgorithm.GreatestCommonDivisorPartial(first_numerator, (UInt32)second_denominator);
                    first_numerator /= d;
                    second_denominator = (Int32)((UInt32)second_denominator / d);
                }
                /*
                var q = (Int64)((UInt64)(UInt32)first_denominator * (UInt32)second_denominator);
                if (s) {
                    q = -q;
                } else {
                    --q;
                }
                checked((Int32)q).Ignore();
                return new Rational64(checked(first_numerator * second_numerator) | (UInt64)q << 32);
                */
                var p = (UInt64)first_numerator * second_numerator;
                var q = (Int64)((UInt64)(UInt32)first_denominator * (UInt32)second_denominator);
                p = checked((UInt32)p);
                checked(unchecked((UInt32)Int32.MinValue) - unchecked((UInt64)q)).Ignore();
                if (s) {
                    return new Rational64((UInt64)(-q << 32) | p);
                } else {
                    return new Rational64((UInt64)(--q << 32) | p);
                }
            }
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 MultiplyAsRational128(Rational64 first, Rational64 second, out UInt64 bits_hi) {
            unchecked {
                var first_numerator = (UInt32)first.bits;
                var first_denominator = (Int32)(first.bits >> 32);
                var second_numerator = (UInt32)second.bits;
                var second_denominator = (Int32)(second.bits >> 32);
                if (0 == first_numerator || 0 == second_numerator) {
                    bits_hi = 0;
                    return 0;
                }
                var s = 0 > (first_denominator ^ second_denominator);
                if (0 <= first_denominator) {
                    ++first_denominator;
                } else {
                    first_denominator = -first_denominator;
                }
                if (0 <= second_denominator) {
                    ++second_denominator;
                } else {
                    second_denominator = -second_denominator;
                }
                {
                    var d = EuclideanAlgorithm.GreatestCommonDivisorPartial((UInt32)first_denominator, second_numerator);
                    first_denominator = (Int32)((UInt32)first_denominator / d);
                    second_numerator /= d;
                }
                {
                    var d = EuclideanAlgorithm.GreatestCommonDivisorPartial(first_numerator, (UInt32)second_denominator);
                    first_numerator /= d;
                    second_denominator = (Int32)((UInt32)second_denominator / d);
                }
                var p = (UInt64)first_numerator * second_numerator;
                var q = (Int64)((UInt64)(UInt32)first_denominator * (UInt32)second_denominator);

                if (s) {
                    q = -q;
                } else {
                    --q;
                }
                bits_hi = (UInt64)q;
                return p;
            }
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 AddAsRational128(Rational64 first, Rational64 second, out UInt64 bits_hi) {
            unchecked {
                var first_numerator = (UInt32)first.bits;
                var first_denominator = first.bits >> 32;
                var second_numerator = (UInt32)second.bits;
                var second_denominator = second.bits >> 32;
                if (0 <= first_denominator) {
                    ++first_denominator;
                }
                if (0 <= second_denominator) {
                    ++second_denominator;
                }
                var p = first_numerator * second_denominator;
                var r = first_denominator * second_numerator;
                var q = first_denominator * second_denominator;
                p += r;
                if (p == 0) {
                    bits_hi = 0;
                    return 0;
                }
                {
                    var d = NumberTheory.EuclideanAlgorithm.GreatestCommonDivisor(p, q);
                    if (0 > p) {
                        d = -d;
                    }
                    p /= d;
                    q /= d;
                }
                if (0 <= q) {
                    --q;
                }
                bits_hi = (UInt64)q;
                return (UInt64)p;
            }
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 SubtractAsRational128(Rational64 first, Rational64 second, out UInt64 bits_hi) {
            unchecked {
                var first_numerator = (UInt32)first.bits;
                var first_denominator = first.bits >> 32;
                var second_numerator = (UInt32)second.bits;
                var second_denominator = second.bits >> 32;
                if (0 <= first_denominator) {
                    ++first_denominator;
                }
                if (0 <= second_denominator) {
                    ++second_denominator;
                }
                var p = first_numerator * second_denominator;
                var r = first_denominator * second_numerator;
                var q = first_denominator * second_denominator;
                p -= r;
                {
                    var d = NumberTheory.EuclideanAlgorithm.GreatestCommonDivisor(p, q);
                    if (0 > p) {
                        d = -d;
                    }
                    p /= d;
                    q /= d;
                }
                if (0 < q) {
                    --q;
                }
                bits_hi = (UInt64)q;
                return (UInt64)p;
            }
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator /(Rational64 first, Rational64 second) {
            var d = unchecked((UInt32)first.bits);
            var c = unchecked((Int32)(first.bits >> 32));
            var f = unchecked((UInt32)second.bits);
            var e = unchecked((Int32)(second.bits >> 32));
            if (0 == f) {
                unchecked(f / 0).Ignore();
            }
            if (0 == d) {
                return Zero;
            }
            var a = EuclideanAlgorithm.GreatestCommonDivisor(d, f);
            d /= a;
            f /= a;
            bool s;
            if (s = 0 <= c) {
                ++c;
            } else {
                c = -c;
            }
            if (0 <= e) {
                s = !s;
                ++e;
            } else {
                e = -e;
            }
            var b = EuclideanAlgorithm.GreatestCommonDivisor(c.ToUnsignedUnchecked(), e.ToUnsignedUnchecked());
            c = unchecked(c.ToUnsignedUnchecked() / b).ToSignedUnchecked();
            e = unchecked(e.ToUnsignedUnchecked() / b).ToSignedUnchecked();
            var p = checked(d * e.ToUnsignedUnchecked());
            var q = unchecked((Int64)c * f);
            if (!s) {
                unchecked {
                    --q;
                }
            } else {
                unchecked {
                    q = -q;
                }
            }
            q = ((Int64)checked((Int32)q) << 32) | (Int64)p;
            return new Rational64(unchecked((UInt64)q));
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator *(Rational64 first, Rational64 second) {
            var d = unchecked((UInt32)first.bits);
            var c = unchecked((Int32)(first.bits >> 32));
            var f = unchecked((UInt32)second.bits);
            var e = unchecked((Int32)(second.bits >> 32));
            if (0 == d || 0 == f) {
                return Zero;
            }
            d = Reduce(d, e, out e);
            f = Reduce(f, c, out c);
            var p = checked(d * f);
            bool s;
            if (s = (0 <= c)) {
                unchecked {
                    ++c;
                }
            }
            if (0 <= e) {
                unchecked {
                    ++e;
                }
                s = !s;
            }
            var q = unchecked((Int64)c * e);
            if (!s) {
                unchecked {
                    --q;
                }
            }
            q = ((Int64)checked((Int32)q) << 32) | (Int64)p;
            return new Rational64(unchecked((UInt64)q));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator +(Rational64 value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator +(Rational64 first, Rational64 second) {
            var lo = Rational64.AddAsRational128(first, second, out var hi);
            return new Rational64(((Int64)checked((Int32)hi.ToSignedUnchecked())).ToUnsignedUnchecked() << 32 | (UInt64)checked((UInt32)lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator -(Rational64 first, Rational64 second) {
            var lo = Rational64.SubtractAsRational128(first, second, out var hi);
            return new Rational64(((Int64)checked((Int32)hi.ToSignedUnchecked())).ToUnsignedUnchecked() << 32 | (UInt64)checked((UInt32)unchecked((Int32)lo)));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator ==(Rational64 first, Rational64 second) {
            return first.bits == second.bits;
        }

        /*
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool operator <=(Rational64 first, Rational64 second) {
            return first.bits == second.bits;
        }
        */

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        internal static UInt32 Reduce(UInt32 p, Int32 q, out Int32 r) {
            UInt32 s;
            if (0 > q) {
                s = unchecked((UInt32)(-q));
            } else {
                s = unchecked((UInt32)(q));
                ++s;
            }
            var t = p;
            for (; ; ) {
                var o = t % s;
                if (0 == o) {
                    r = q / unchecked((Int32)s);
                    return p / s;
                }
                t = s;
                s = o;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 FromInt64Bits(Int64 bits) {
            var numerator = unchecked((UInt32)bits);
            var denominator = unchecked((Int32)(bits >> 32));
            var s = 0 > denominator;
            if (s) {
                denominator = -denominator;
            } else {
                ++denominator;
            }
            var d = EuclideanAlgorithm.GreatestCommonDivisorPartial(numerator, unchecked((UInt32)denominator));
            if (s) {
                return new Rational64(unchecked(((UInt64)((UInt32)(-denominator) / d) << 32) | numerator / d));
            } else {
                return new Rational64(unchecked(((UInt64)((UInt32)denominator / d - 1) << 32) | numerator / d));
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int64 ToInt64Bits(Rational64 value) {
            Contract.Ensures(Contract.OldValue(value) == Rational64.FromInt64Bits(Contract.Result<Int64>()));
            return value.bits;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public int GetHashCodeSealed() {
            var code = this.bits;
            return unchecked((int)code ^ (int)(code >> 32));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override int GetHashCode() {
            return this.GetHashCodeSealed();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public bool EqualsSealed(Rational64 other) {
            return this.bits == other.bits;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public bool Equals(Rational64 other) {
            return this.EqualsSealed(other);
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public bool EqualsSealed(object other) {
            if (other is Rational64) {
                var t = (Rational64)other;
                return t.Equals(other);
            }
            return base.Equals(other);
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override bool Equals(object other) {
            return this.EqualsSealed(other);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRational64(UInt32 value) {
            return new Rational64(unchecked((UInt64)value));
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator Rational64(UInt32 value) {
            return new Rational64(unchecked((UInt64)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRational64(Int32 value) {
            if (0 <= value) {
                return new Rational64(unchecked((UInt64)value));
            } else {
                return new Rational64(unchecked((UInt64)(unchecked(-value) | ((Int64)(-1) << 32))));
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static implicit operator Rational64(Int32 value) {
            if (0 <= value) {
                return new Rational64(unchecked((UInt64)value));
            } else {
                return new Rational64(unchecked((UInt64)(unchecked(-value) | ((Int64)(-1) << 32))));
            }
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRational64(UInt64 value) {
            checked(UInt32.MaxValue - value).Ignore();
            return new Rational64(unchecked((UInt64)value));
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(UInt64 value) {
            checked(UInt32.MaxValue - value).Ignore();
            return new Rational64(unchecked((UInt64)value));
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRational64(Int64 value) {
            if (0 <= value) {
                checked(unchecked((UInt64)UInt32.MaxValue) - unchecked((UInt64)value)).Ignore();
                return new Rational64(unchecked((UInt64)value));
            } else {
                value = unchecked(-value);
                checked(unchecked((UInt64)UInt32.MaxValue) - unchecked((UInt64)value)).Ignore();
                return new Rational64(unchecked((UInt64)(value | ((Int64)(-1) << 32))));
            }
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(Int64 value) {
            if (0 <= value) {
                checked(unchecked((UInt64)UInt32.MaxValue) - unchecked((UInt64)value)).Ignore();
                return new Rational64(unchecked((UInt64)value));
            } else {
                value = unchecked(-value);
                checked(unchecked((UInt64)UInt32.MaxValue) - unchecked((UInt64)value)).Ignore();
                return new Rational64(unchecked((UInt64)(value | ((Int64)(-1) << 32))));
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int64 ToInt64Nearest(Rational64 value) {
            var c = value.bits;
            if (0 == c) {
                return 0;
            }
            if (1 == unchecked((Int32)(c >> 32)) || 1 == ~unchecked((Int32)(c >> 32))) {
                c >>= 1;
                if (1 == (1 & c)) {
                    unchecked {
                        ++c;
                    }
                }
                if (0 > c) {
                    return unchecked(-(Int64)(Int32)c);
                } else {
                    return unchecked((Int32)c);
                }
            }
            c = unchecked((Int32)(c >> 32));
            if (0 <= unchecked((Int32)c)) {
                unchecked {
                    ++c;
                }
            }
            return unchecked((Int64)((double)(UInt32)value.bits / c));
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int64 ToInt64(Rational64 value) {
            var numerator = unchecked((UInt32)value.bits);
            var denominator = unchecked((Int32)(value.bits >> 32));
            var c = denominator >> (32 - 1);
            checked(0 - unchecked((UInt32)(c ^ denominator))).Ignore();
            return (Int64)(((UInt64)c << 32) | numerator);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Int64(Rational64 value) {
            return ToInt64(value);
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 ToUInt64(Rational64 value) {
            return checked((UInt64)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator UInt64(Rational64 value) {
            return checked((UInt64)(Int64)value);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int32 ToInt32(Rational64 value) {
            return checked((Int32)(Int64)value);
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Int32(Rational64 value) {
            return checked((Int32)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 ToUInt32(Rational64 value) {
            return checked((UInt32)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator UInt32(Rational64 value) {
            return checked((UInt32)(Int64)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Double ToDoubleInexact(Rational64 value) {
            var c = value.bits;
            if (0 == c) {
                return 0;
            }
            c = unchecked((Int32)(c >> 32));
            if (0 <= unchecked((Int32)c)) {
                unchecked {
                    ++c;
                }
            }
            return unchecked((Int64)((Double)(UInt32)value.bits / c));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Double(Rational64 value) {
            var c = value.bits;
            if (0 == c) {
                return 0;
            }
            c = unchecked((Int32)(c >> 32));
            if (0 <= unchecked((Int32)c)) {
                unchecked {
                    ++c;
                }
            }
            return unchecked((Int64)((Double)(UInt32)value.bits / c));
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRatioanl64ContinuedFractionBestApproximation(Double value) {
            // TODO: Perf
            if (Double.IsInfinity(value) || Double.IsNaN(value)) {
                // TODO: Perf
                throw new NotFiniteNumberException(value);
            }
            // Interval "( 2 ^ 32 - 1 - 1 / 2, 2 ^ 32 - 1 + 1 / 2 )"
            var s = 0 > value;
            var a = (Double)(s ? -value : value);
            const Double b = 4.294967296e9;
            if (b <= a) {
                // TODO: Perf
                throw new OverflowException();
            }
            if (a <= 4.6566128709089882341637330901978e-10) {
                return default(Rational64);
            }
            var p2 = (UInt32)1;
            var q2 = (UInt32)0;
            var d = unchecked((UInt32)Math.Floor(a));
            var p1 = d;
            var q1 = (UInt32)1;
            L_0:
            a = 1.0 / (a - d);
            var c = unchecked((UInt32)Math.Floor(a));
            var p0 = unchecked((UInt64)c * p1 + p2);
            var q0 = unchecked((UInt64)c * q1 + q2);
            if (b <= a || p0 > UInt32.MaxValue || q0 > unchecked((UInt32)Int32.MinValue)) {
                if (s) {
                    return new Rational64(((UInt64)unchecked((UInt32)(-(Int32)q1)) << 32) | p1);
                } else {
                    return new Rational64(((UInt64)unchecked(q1 - 1) << 32) | p1);
                }
            }
            p2 = p1;
            q2 = q1;
            p1 = unchecked((UInt32)p0);
            q1 = unchecked((UInt32)q0);
            d = unchecked((UInt32)Math.Floor(a));
            goto L_0;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public int CompareTo(Rational64 other) {
            var this_numerator = (UInt32)this.bits;
            var this_denominator = (Int32)(this.bits >> 32);
            var other_numerator = (UInt32)other.bits;
            var other_denominator = (Int32)(other.bits >> 32);
            if (0 <= this_denominator) {
                if (0 <= other_denominator) {
                    unchecked {
                        ++this_denominator;
                        ++other_denominator;
                    }
                    var m = unchecked((UInt64)this_numerator) * unchecked((UInt32)other_denominator);
                    var n = unchecked((UInt32)this_denominator) * unchecked((UInt64)other_numerator);
                    if (m < n) {
                        return -1;
                    }
                    if (m > n) {
                        return 1;
                    }
                    return 0;
                } else {
                    return 1;
                }
            } else {
                if (0 <= other_denominator) {
                    return -1;
                } else {
                    unchecked {
                        this_denominator = -this_denominator;
                        other_denominator = -other_denominator;
                    }
                    var m = unchecked((UInt64)this_numerator) * unchecked((UInt32)other_denominator);
                    var n = unchecked((UInt32)this_denominator) * unchecked((UInt64)other_numerator);
                    if (m > n) {
                        return -1;
                    }
                    if (m < n) {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(Double value) {
            // TODO: Perf
            ThrowOnInfinite(value);
            // Interval "( 2 ^ 32 - 1 - 1 / 2, 2 ^ 32 - 1 + 1 / 2 )"
            var s = 0 > value;
            var a = (Double)(s ? -value : value);
            const Double b = 4.294967296e9;
            ThrowOnLessThanOrEqual(b, a);
            if (a <= 4.6566128709089882341637330901978e-10) {
                return default(Rational64);
            }
            var p2 = (UInt32)1;
            var q2 = (UInt32)0;
            var d = unchecked((UInt32)Math.Floor(a));
            var p1 = d;
            var q1 = (UInt32)1;
            L_0:
            a = 1.0 / (a - d);
            var c = unchecked((UInt32)Math.Floor(a));
            var p0 = unchecked((UInt64)c * p1 + p2);
            var q0 = unchecked((UInt64)c * q1 + q2);
            if (b <= a || p0 > UInt32.MaxValue || q0 > unchecked((UInt32)Int32.MinValue)) {
                if (s) {
                    return new Rational64(((UInt64)unchecked((UInt32)(-(Int32)q1)) << 32) | p1);
                } else {
                    return new Rational64(((UInt64)unchecked(q1 - 1) << 32) | p1);
                }
            }
            p2 = p1;
            q2 = q1;
            p1 = unchecked((UInt32)p0);
            q1 = unchecked((UInt32)q0);
            d = unchecked((UInt32)Math.Floor(a));
            goto L_0;
        }
    }
}


namespace UltimateOrb.Mathematics.Exact {

    public static partial class Rational64Module {

        public static Rational64 GetNextRational64<TRandomNumberGenerator>(this TRandomNumberGenerator random) where TRandomNumberGenerator : IRandomNumberGenerator {
            for (; ; ) {
                var a = random.GetNextInt64Bits();
                var b = a >> 32;
                if (0 <= b) {
                    unchecked {
                        ++b;
                    }
                } else {
                    unchecked {
                        b = -b;
                    }
                }
                if (1 == EuclideanAlgorithm.GreatestCommonDivisor(unchecked((UInt32)a), unchecked((UInt32)b))) {
                    return new Rational64(a.ToUnsignedUnchecked());
                }
            }
        }

        public static Rational64 GetNextRational64<TRandomNumberGenerator>(ref this TRandomNumberGenerator random) where TRandomNumberGenerator : struct, IRandomNumberGenerator {
            for (; ; ) {
                var a = random.GetNextInt64Bits();
                var b = a >> 32;
                if (0 <= b) {
                    unchecked {
                        ++b;
                    }
                } else {
                    unchecked {
                        b = -b;
                    }
                }
                if (1 == EuclideanAlgorithm.GreatestCommonDivisor(unchecked((UInt32)a), unchecked((UInt32)b))) {
                    return new Rational64(a.ToUnsignedUnchecked());
                }
            }
        }
    }
}
