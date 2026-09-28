using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Numerics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using UltimateOrb.Mathematics.NumberTheory;
using UltimateOrb.Numerics;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.ThrowHelper;

namespace UltimateOrb.Mathematics.Exact {

    [ComVisibleAttribute(true)]
    [SerializableAttribute()]
    [StructLayoutAttribute(LayoutKind.Sequential, Pack = 8)]
    public readonly partial struct Rational64 :
        IComparable<Rational64>,
        IEquatable<Rational64>,
        ISpanFormattable,
        IMinMaxValue<Rational64>,
        IUtf8SpanFormattable,
        IParsable<Rational64>,
        ISpanParsable<Rational64>,
        IUtf8SpanParsable<Rational64>,
        IAdditionOperators<Rational64, Rational64, Rational64>,
        IAdditiveIdentity<Rational64, Rational64>,
        IComparisonOperators<Rational64, Rational64, bool>,
        IDecrementOperators<Rational64>,
        IDivisionOperators<Rational64, Rational64, Rational64>,
        IEqualityOperators<Rational64, Rational64, bool>,
        IExponentialFunctions<Rational64>,
        // IFloatingPoint<Rational64>,
        IIncrementOperators<Rational64>,
        IModulusOperators<Rational64, Rational64, Rational64>,
        IMultiplicativeIdentity<Rational64, Rational64>,
        IMultiplyOperators<Rational64, Rational64, Rational64>,
        INumber<Rational64>,
        INumberBase<Rational64>,
        IPowerFunctions<Rational64>,
        IRootFunctions<Rational64>,
        ISignedNumber<Rational64>,
        ISubtractionOperators<Rational64, Rational64, Rational64>,
        IUnaryNegationOperators<Rational64, Rational64>,
        IUnaryPlusOperators<Rational64, Rational64> {

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

        static Rational64 IAdditiveIdentity<Rational64, Rational64>.AdditiveIdentity {

            get => Zero;
        }

        public static Rational64 One {

            get => new Rational64((UInt64)1);
        }

        static Rational64 IMultiplicativeIdentity<Rational64, Rational64>.MultiplicativeIdentity {

            get => One;
        }

        public static Rational64 MinusOne {

            get => new Rational64(unchecked((UInt64)(1 | ((Int64)(Int32)UInt32.MaxValue << 32))));
        }

        static Rational64 ISignedNumber<Rational64>.NegativeOne {

            get => MinusOne;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rational64 PositiveEpsilon {

            get => new Rational64(unchecked((UInt64)(1 | ((Int64)Int32.MaxValue << 32))));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Rational64 NegativeEpsilon {

            get => new Rational64(unchecked((UInt64)(1 | ((Int64)Int32.MinValue << 32))));
        }

        public static Rational64 MaxValue {

            get => new Rational64((UInt64)UInt32.MaxValue);
        }

        public static Rational64 MinValue {

            get => new Rational64(UInt64.MaxValue);
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

        static int INumberBase<Rational64>.Radix => throw new NotSupportedException();

        static Rational64 IFloatingPointConstants<Rational64>.E => throw new OverflowException();

        static Rational64 IFloatingPointConstants<Rational64>.Pi => throw new OverflowException();

        static Rational64 IFloatingPointConstants<Rational64>.Tau => throw new OverflowException();

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOut("")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Pure]
        public static Rational64 Inverse(Rational64 value) {
            var n = unchecked((UInt32)value.bits);
            var e = unchecked((Int32)(value.bits >> 32));

            if (n == 0u) {
                // Force DivideByZeroException via the JIT's own div-by-zero path.
                UInt32 zero = 0;
                _ = n / zero;
                return default;
            }

            // n >= 1.  Need n <= 2^31 so the new encoded denominator
            // (either n - 1 or -n) fits in Int32.  checked cast throws when
            // n - 1 > Int32.MaxValue, i.e. n > 2^31.
            _ = checked((Int32)(n - 1u));

            UInt32 newN;
            Int32 newE;
            if (e >= 0) {
                // D_old = e + 1 > 0.  Inverse = (e + 1) / n.  Both positive.
                newN = unchecked((UInt32)e + 1u);        // = D_old
                newE = unchecked((Int32)(n - 1u));       // = n - 1  (fits: guarded above)
            } else {
                // D_old = e < 0.  Inverse = e / n = |e| / (-n).  Sign lives on denom.
                newN = unchecked((UInt32)(-(Int64)e));   // = |D_old| <= 2^31, fits UInt32
                newE = checked((Int32)(-(Int64)n));      // = -n, fits because n <= 2^31
            }
            return new Rational64(unchecked(((UInt64)(UInt32)newE << 32) | newN));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 Negate(Rational64 value) {
            var n = unchecked((UInt32)value.bits);
            if (0u == n) {
                return Zero;
            }
            var e = unchecked((Int32)(value.bits >> 32));
            e = ~e;
            return new Rational64(unchecked(((UInt64)(UInt32)e << 32) | n));
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
                var q = (Int64)((UInt64)(UInt32)denominator * (UInt32)second_denominator);
                if (s) {
                    q = -q;
                } else {
                    --q;
                }
                checked((Int32)q).Ignore();
                return new Rational64(checked(numerator * second_numerator) | (UInt64)q << 32);
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 Multiply(Rational64 first, Int64 second) {
            unchecked {
                try {
                    var first_numerator = (UInt32)first.bits;
                    var first_denominator = (Int32)(first.bits >> 32);
                    var second_numerator = UltimateOrb.Mathematics.Elementary.Math.AbsAsUnsigned(second);
                    var second_denominator = (Int32)(second >> 63);
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
                    var q = (Int64)((UInt64)(UInt32)denominator * (UInt32)second_denominator);
                    if (s) {
                        q = -q;
                    } else {
                        --q;
                    }
                    checked((Int32)q).Ignore();
                    return new Rational64(checked(numerator * second_numerator) | (UInt64)q << 32);
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
                } catch (Exception) {
                }
                return default;
            }
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveOptimization)]
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

        static Int64 DecodeDenominator(Int32 encodedDenominator) {
            var r = (Int64)encodedDenominator;
            return r >= 0 ? r + 1 : r;
        }

        static Int32 EncodeDenominator(Int32 denominator) {
            Debug.Assert(denominator != 0);
            return denominator > 0 ? denominator - 1 : denominator;
        }

        static Int32 EncodeDenominator(Int64 denominator) {
            Debug.Assert(denominator != 0);
            return checked((Int32)(denominator > 0 ? unchecked(denominator - 1) : denominator));
        }

        public static (Int64 quotient, Rational64 remainder) DivRemIntegral(Rational64 dividend, Rational64 divisor) {
            // Introduce local variables for Denominator and Numerator fields
            var dividendDenominator = DecodeDenominator(unchecked((Int32)(dividend.bits >> 32)));
            var dividendNumerator = unchecked((UInt32)(dividend.bits));
            var divisorDenominator = DecodeDenominator(unchecked((Int32)(divisor.bits >> 32)));
            var divisorNumerator = unchecked((UInt32)(divisor.bits));

            if (dividend.bits == 0) {
                return default;
            }

            // Calculate the remainder denominator
            Int64 remainderDenominator = unchecked((Int64)dividendDenominator * (Int64)divisorDenominator);

            // Calculate the quotient and remainder using Math.DivRem
            Int64 quotient = Math.DivRem(dividendNumerator * divisorDenominator, dividendDenominator * divisorNumerator, out Int64 remainderNumerator);

            if (0 > remainderNumerator) {
                remainderNumerator = -remainderNumerator;
                remainderDenominator = -remainderDenominator;
            }
            remainderNumerator = unchecked((Int64)Reduce1((UInt64)remainderNumerator, remainderDenominator, out remainderDenominator));
            // Create the remainder Rational64 using the internal constructor and correct denominator
            Rational64 remainder = new Rational64(unchecked((UInt64)(checked((UInt32)remainderNumerator) | ((Int64)EncodeDenominator(remainderDenominator) << 32))));

            return (quotient, remainder);

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            static UInt64 Reduce1(UInt64 p, Int64 q, out Int64 r) {
                UInt64 s = Mathematics.Elementary.Math.AbsAsUnsigned(q);
                var t = p;
                for (; ; ) {
                    var o = t % s;
                    if (0 == o) {
                        r = q / unchecked((Int64)s);
                        return p / s;
                    }
                    t = s;
                    s = o;
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Rational64 operator %(Rational64 dividend, Rational64 divisor) {
            // Introduce local variables for Denominator and Numerator fields
            var dividendDenominator = DecodeDenominator(unchecked((Int32)(dividend.bits >> 32)));
            var dividendNumerator = unchecked((UInt32)(dividend.bits));
            var divisorDenominator = DecodeDenominator(unchecked((Int32)(dividend.bits >> 32)));
            var divisorNumerator = unchecked((UInt32)(dividend.bits));

            // Calculate the remainder denominator
            Int64 remainderDenominator = divisorNumerator * dividendDenominator;

            // Calculate the quotient and remainder
            var remainderNumerator = dividendNumerator * divisorDenominator % remainderDenominator;

            if (0 > remainderNumerator) {
                remainderNumerator = -remainderNumerator;
                remainderDenominator = -remainderDenominator;
            }

            // Create the remainder Rational64 using the internal constructor and correct denominator
            Rational64 remainder = new Rational64(unchecked((UInt64)(checked((UInt32)remainderNumerator) | ((Int64)EncodeDenominator(remainderDenominator) << 32))));

            return remainder;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
            return ToRational64Checked(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 operator -(Rational64 first, Rational64 second) {
            var lo = Rational64.SubtractAsRational128(first, second, out var hi);
            return ToRational64Checked(lo, hi);
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
        internal static Rational64 FromInt64Bits(Int64 bits) {
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
        internal bool EqualsSealed(object? other) {
            return other is Rational64 t && EqualsSealed(t);
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public override bool Equals(object? other) {
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
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRational64(UInt64 value) {
            checked(UInt32.MaxValue - value).Ignore();
            return new Rational64(unchecked((UInt64)value));
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(UInt64 value) {
            checked(UInt32.MaxValue - value).Ignore();
            return new Rational64(unchecked((UInt64)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int64 ToInt64(Rational64 value) {
            var numerator = unchecked((UInt32)value.bits);
            var denominator = unchecked((Int32)(value.bits >> 32));
            var c = denominator >> (32 - 1);
            checked(0u - unchecked((UInt32)(c ^ denominator))).Ignore();
            return unchecked(0 != c ? -(Int64)numerator : (Int64)numerator);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Int64(Rational64 value) {
            return ToInt64(value);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt64 ToUInt64(Rational64 value) {
            return checked((UInt64)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator UInt64(Rational64 value) {
            return checked((UInt64)(Int64)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Int32 ToInt32(Rational64 value) {
            return checked((Int32)(Int64)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Int32(Rational64 value) {
            return checked((Int32)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UInt32 ToUInt32(Rational64 value) {
            return checked((UInt32)(Int64)value);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
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
            return (Double)unchecked((UInt32)value.bits) / unchecked((Int32)c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool IsPow2Partial(UInt32 value) {
            Debug.Assert(0 != value);
            return (value & (value - 1)) == 0;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Double ToDoubleExact(Rational64 value) {
            var c = value.bits;
            if (0 == c) {
                return 0.0;
            }
            // c: Int64, currently encoded denominator in [-2^31, 2^31 - 1]
            c = unchecked((Int32)(c >> 32));
            // d: UInt32, will become |actual denominator|
            UInt32 d = unchecked((UInt32)(Int32)c);
            if (0 <= unchecked((Int32)d)) {
                // e >= 0: denominator d = e + 1 > 0
                unchecked {
                    ++c;     // c = e + 1, in [1, 2^31]
                    ++d;     // d = e + 1, in [1, 2^31]
                }
            } else {
                // e < 0: denominator d = e < 0
                unchecked {
                    d = (UInt32)(-((Int32)d));   // d = -e, in [1, 2^31]
                }
                // c is left as e (negative) — correct sign for the division below
            }
            // Exact iff the reduced denominator is a power of two.
            UltimateOrb.Utilities.ThrowHelper.ThrowOnFalse(IsPow2Partial(d));
            // c is Int64 here, so c = ±2^31 is not truncated.
            return (Double)unchecked((UInt32)value.bits) / (Double)c;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Double(Rational64 value) {
            return ToDoubleInexact(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator checked Double(Rational64 value) {
            return ToDoubleExact(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Single ToSingleInexact(Rational64 value) {
            return (Single)ToDoubleInexact(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Single ToSingleExact(Rational64 value) {
            var c = value.bits;
            if (0 == c) {
                return 0;
            }
            c = unchecked((Int32)(c >> 32));
            UInt32 d = unchecked((UInt32)(Int32)c);
            if (0 <= unchecked((Int32)d)) {
                unchecked {
                    ++c;
                    ++d;
                }
            } else {
                unchecked {
                    d = (-d.ToSignedUnchecked()).ToUnsignedUnchecked();
                }
            }
            var e = unchecked(BinaryNumerals.CountLeadingZeros(unchecked((UInt32)value.bits)) + BinaryNumerals.CountTrailingZeros(unchecked((UInt32)value.bits)));
            UltimateOrb.Utilities.ThrowHelper.ThrowOnGreaterThan(e, 32 - (1 + 23));
            UltimateOrb.Utilities.ThrowHelper.ThrowOnTrue(IsPow2Partial(d));
            return (Single)(Double)unchecked((UInt32)value.bits) / unchecked((Int32)c);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Single(Rational64 value) {
            return ToSingleInexact(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator checked Single(Rational64 value) {
            return ToSingleExact(value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static Rational64 ToRatioanl64ContinuedFractionBestApproximation(Double value) {
            // TODO: Perf
            if (Double.IsInfinity(value) || Double.IsNaN(value)) {
                // TODO: Perf
                throw new NotFiniteNumberException(value);
            }
            // IntervalI "( 2 ^ 32 - 1 - 1 / 2, 2 ^ 32 - 1 + 1 / 2 )"
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
            var this_numerator = unchecked((UInt32)this.bits);
            var this_denominator = unchecked((Int32)(this.bits >> 32));
            var other_numerator = unchecked((UInt32)other.bits);
            var other_denominator = unchecked((Int32)(other.bits >> 32));
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

        public static Rational64 Abs(Rational64 value) {
            return IsNegative(value) ? -value : value;
        }

        static bool INumberBase<Rational64>.IsCanonical(Rational64 value) {
            var n = unchecked((UInt32)value.bits);
            if (0u == n) {
                // Only the canonical zero encoding has bits == 0.
                // Anything else with n == 0 (e.g. 0xFFFFFFFF00000000) is 0 with
                // a denominator other than 1 — not canonical.
                return value.bits == 0;
            }
            var e = unchecked((Int32)(value.bits >> 32));
            UInt32 absD = e >= 0
                ? unchecked((UInt32)((Int64)e + 1))
                : unchecked((UInt32)(-(Int64)e));
            // n > 0 here, so gcd(n, |d|) >= 1; canonical iff it equals 1.
            return 1u == EuclideanAlgorithm.GreatestCommonDivisorPartial(n, absD);
        }

        static bool INumberBase<Rational64>.IsComplexNumber(Rational64 value) {
            return false;
        }

        static bool INumberBase<Rational64>.IsEvenInteger(Rational64 value) {
            return IsInteger(value) && (0 == (1 & unchecked((int)value.bits)));
        }

        static bool INumberBase<Rational64>.IsFinite(Rational64 value) {
            return true;
        }

        static bool INumberBase<Rational64>.IsImaginaryNumber(Rational64 value) {
            return false;
        }

        static bool INumberBase<Rational64>.IsInfinity(Rational64 value) {
            return false;
        }

        public static bool IsInteger(Rational64 value) {
            return unchecked(1 + (UInt32)(value.bits >> 32)) <= 1;
        }

        static bool INumberBase<Rational64>.IsNaN(Rational64 value) {
            return false;
        }

        public static bool IsNegative(Rational64 value) {
            return 0 > value.bits;
        }

        static bool INumberBase<Rational64>.IsNegativeInfinity(Rational64 value) {
            return false;
        }

        static bool INumberBase<Rational64>.IsNormal(Rational64 value) {
            return 0 != value.bits;
        }

        static bool INumberBase<Rational64>.IsOddInteger(Rational64 value) {
            return IsInteger(value) && (0 != (1 & unchecked((int)value.bits)));
        }

        public static bool IsPositive(Rational64 value) {
            return value.bits > 0;
        }

        static bool INumberBase<Rational64>.IsPositiveInfinity(Rational64 value) {
            return false;
        }

        static bool INumberBase<Rational64>.IsRealNumber(Rational64 value) {
            return true;
        }

        static bool INumberBase<Rational64>.IsSubnormal(Rational64 value) {
            return false;
        }

        public static bool IsZero(Rational64 value) {
            return 0 == value.bits;
        }

        static Rational64 INumberBase<Rational64>.MaxMagnitude(Rational64 x, Rational64 y) {
            return IntegerGenericMathImpl.MaxMagnitude(x, y);
        }

        static Rational64 INumberBase<Rational64>.MaxMagnitudeNumber(Rational64 x, Rational64 y) {
            return IntegerGenericMathImpl.MaxMagnitude(x, y);
        }

        static Rational64 INumberBase<Rational64>.MinMagnitude(Rational64 x, Rational64 y) {
            return IntegerGenericMathImpl.MinMagnitude(x, y);
        }

        static Rational64 INumberBase<Rational64>.MinMagnitudeNumber(Rational64 x, Rational64 y) {
            return IntegerGenericMathImpl.MinMagnitude(x, y);
        }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        /// <summary>
        /// Converts a <see cref="BigRational"/> to <see cref="Rational64"/> if the value
        /// fits the compact encoding (numerator in [0, 2^32 - 1], denominator magnitude
        /// in [1, 2^31]).  Returns false otherwise.
        /// </summary>
        private static bool TryFromBigRational(BigRational big, out Rational64 result) {
            BigInteger num = big.Numerator;
            BigInteger den = big.SignedDenominator;

            // Canonical BigRational has den > 0; normalise defensively.
            if (den.Sign < 0) {
                num = -num;
                den = -den;
            }

            // Encoding limits.
            BigInteger absNum = BigInteger.Abs(num);
            if (absNum > UInt32.MaxValue) {                 // 2^32 - 1
                result = default;
                return false;
            }
            if (den > (BigInteger.One << 31)) {             // 2^31
                result = default;
                return false;
            }

            // Canonical zero: bits == 0.
            if (absNum.IsZero) {
                result = default;
                return true;
            }

            // num.Sign != 0 and |num| <= UInt32.MaxValue, den in [1, 2^31].
            UInt32 n = (UInt32)absNum;
            Int32 e = num.Sign < 0
                ? -(Int32)(UInt32)den                       // d = -den, encoded as d
                : (Int32)((UInt32)den - 1u);                // d = +den, encoded as d - 1
            result = new Rational64(((UInt64)(UInt32)e << 32) | n);
            return true;
        }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        public static Rational64 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var big = BigRational.Parse(s, style, provider);
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return (Rational64)big;
        }

        public static Rational64 Parse(string s, NumberStyles style, IFormatProvider? provider) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var big = BigRational.Parse(s, style, provider);
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return (Rational64)big;
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Rational64 result) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            if (!BigRational.TryParse(s, style, provider, out var big)) {
                result = default;
                return false;
            }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return TryFromBigRational(big, out result);
        }

        public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out Rational64 result) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            if (!BigRational.TryParse(s, style, provider, out var big)) {
                result = default;
                return false;
            }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return TryFromBigRational(big, out result);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            BigRational big = this;         // implicit Rational64 -> BigRational
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return big.TryFormat(destination, out charsWritten, format, provider);
        }

        public static Rational64 Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var big = BigRational.Parse(s, provider);
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return (Rational64)big;
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Rational64 result) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            if (!BigRational.TryParse(s, provider, out var big)) {
                result = default;
                return false;
            }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return TryFromBigRational(big, out result);
        }

        public static Rational64 Parse(string s, IFormatProvider? provider) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            var big = BigRational.Parse(s, provider);
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return (Rational64)big;
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out Rational64 result) {
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            if (!BigRational.TryParse(s, provider, out var big)) {
                result = default;
                return false;
            }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return TryFromBigRational(big, out result);
        }

        static bool INumberBase<Rational64>.TryConvertFromChecked<TOther>(TOther value, out Rational64 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Rational64>.TryConvertFromSaturating<TOther>(TOther value, out Rational64 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Rational64>.TryConvertFromTruncating<TOther>(TOther value, out Rational64 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Rational64>.TryConvertToChecked<TOther>(Rational64 value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Rational64>.TryConvertToSaturating<TOther>(Rational64 value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Rational64>.TryConvertToTruncating<TOther>(Rational64 value, out TOther result) {
            throw new NotImplementedException();
        }


        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static explicit operator Rational64(Double value) {
            // TODO: Perf
            ThrowOnInfinite(value);
            // IntervalI "( 2 ^ 32 - 1 - 1 / 2, 2 ^ 32 - 1 + 1 / 2 )"
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

        public static UInt64 DecreaseAsRational128(Rational64 value, out UInt64 bits_hi) {
            unchecked {
                var numerator = (UInt32)value.bits;
                var denominator = value.bits >> 32;
                if (0 <= denominator) {
                    ++denominator;
                }
                var p = (Int64)numerator;
                var q = denominator;
                p -= denominator;
                {
                    if (0 > p) {
                        p = -p;
                        q = -q;
                    }
                }
                if (0 <= q) {
                    --q;
                }
                bits_hi = (UInt64)q;
                return (UInt64)p;
            }
        }

        public static Rational64 operator --(Rational64 value) {
            var lo = DecreaseAsRational128(value, out var hi);
            return ToRational64Checked(lo, hi);
        }

        public static UInt64 IncreaseAsRational128(Rational64 value, out UInt64 bits_hi) {
            unchecked {
                var numerator = (UInt32)value.bits;
                var denominator = value.bits >> 32;
                if (0 <= denominator) {
                    ++denominator;
                }
                var p = (Int64)numerator;
                var q = denominator;
                p += denominator;
                if (p == 0) {
                    bits_hi = 0;
                    return 0;
                }
                {
                    if (0 > p) {
                        p = -p;
                        q = -q;
                    }
                }
                if (0 <= q) {
                    --q;
                }
                bits_hi = (UInt64)q;
                return (UInt64)p;
            }
        }

        public static Rational64 operator ++(Rational64 value) {
            var lo = IncreaseAsRational128(value, out var hi);
            return ToRational64Checked(lo, hi);
        }

        public static bool operator <(Rational64 first, Rational64 second) {
            return first.CompareTo(second) < 0;
        }

        public static bool operator <=(Rational64 first, Rational64 second) {
            return first.CompareTo(second) <= 0;
        }

        public static bool operator >(Rational64 first, Rational64 second) {
            return first.CompareTo(second) > 0;
        }

        public static bool operator >=(Rational64 first, Rational64 second) {
            return first.CompareTo(second) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static Rational64 ToRational64Checked(ulong lo, ulong hi) {
            return new Rational64(((Int64)checked((Int32)hi.ToSignedUnchecked())).ToUnsignedUnchecked() << 32 | (UInt64)checked((UInt32)lo));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Rational64 GetFractionPart(Rational64 value) {
            var m = unchecked((Int32)(value.bits >> 32));
            var m1 = unchecked((UInt32)(0 > value.bits ? -m : 1 + m));
            var n = unchecked((UInt32)value.bits);
            var r = n % m1;
            return 0 == r ? default : new Rational64(unchecked(r | ((UInt64)m << 32)));
        }

        public static Rational64 Cbrt(Rational64 x) {
            if (IsZero(x)) {
                return x;
            }
            var n = UltimateOrb.Mathematics.Elementary.Math.CbrtRem(unchecked((UInt32)x.Numerator), out var r);
            _ = 0u - r.ToUnsignedUnchecked();
            var sd = unchecked((Int32)x.SignedDenominator);
            if (Int32.MinValue == sd) {
                _ = -sd;
            }
            var d = UltimateOrb.Mathematics.Elementary.Math.CbrtRem(sd, out var s);
            _ = 0u - r.ToUnsignedUnchecked();
            unchecked {
                d -= d > 0 ? 1 : 0;
            }
            return new Rational64(unchecked(n | ((UInt64)d << 32)));
        }

        /// <summary>
        /// Returns <paramref name="x"/> raised to the integer power <paramref name="n"/>,
        /// reduced and canonical.  Throws <see cref="OverflowException"/> if the exact
        /// result is not representable in <see cref="Rational64"/>.
        /// </summary>
        public static Rational64 PowN(Rational64 x, long n) {
            if (n == 0) return One;

            UInt32 num = (UInt32)x.bits;
            if (num == 0u) {
                if (n > 0) return default;                 // 0^n = 0, n > 0
                UInt32 zero = 0;
                _ = zero / zero;                           // 0^negative -> DivideByZeroException
                return default;                            // unreachable
            }

            if (n < 0) {
                // -n would overflow long only for long.MinValue; combined with
                // |x| != 1 this forces overflow, so guard with ThrowOnTrue.
                Utilities.ThrowHelper.ThrowOnTrue(n == long.MinValue);
                // Invert the base first.  Then the bounds enforced by PowNPositive
                // (numerator <= 2^32 - 1, denominator magnitude <= 2^31) are exactly
                // the bounds the final result must satisfy.
                return PowNPositive(Inverse(x), -n);
            }

            return PowNPositive(x, n);
        }

        // Preconditions: n > 0, x != 0.
        private static Rational64 PowNPositive(Rational64 x, long n) {
            Debug.Assert(n > 0);

            UInt32 num = (UInt32)x.bits;
            Debug.Assert(num != 0u);

            Int32 e = (Int32)(x.bits >> 32);
            Int64 d = e >= 0 ? (Int64)e + 1 : (Int64)e;
            bool neg = d < 0;
            UInt32 absD = neg ? unchecked((UInt32)(-d)) : unchecked((UInt32)d);

            // |x| = 1  =>  ±1
            if (num == 1u && absD == 1u) {
                return (neg && (n & 1L) != 0L) ? MinusOne : One;
            }

            // |x| != 1 and num != 0  =>  num >= 2 or absD >= 2.  Then num^n must
            // stay <= 2^32 - 1 and absD^n must stay <= 2^31, so n >= 32 always
            // overflows the encoding.
            Utilities.ThrowHelper.ThrowOnTrue(n > 31);

            UInt64 p = 1;
            UInt64 q = 1;
            Int32 nn = (Int32)n;
            for (Int32 i = 0; i < nn; i++) {
                p *= num;
                Utilities.ThrowHelper.ThrowOnTrue(p > UInt32.MaxValue);      // numerator bound: 2^32 - 1
                q *= absD;
                Utilities.ThrowHelper.ThrowOnTrue(q > (1UL << 31));          // denominator magnitude bound: 2^31
            }

            UInt32 newN = (UInt32)p;
            bool resultNeg = neg && (n & 1L) != 0L;
            Int64 newD = resultNeg ? -(Int64)q : (Int64)q;
            // newD == +2^31 encodes to Int32.MaxValue; newD == -2^31 encodes to
            // Int32.MinValue.  Both fit, so these casts never throw for the ranges
            // guaranteed by the loop above.
            Int32 newE = newD > 0
                ? checked((Int32)(newD - 1))
                : checked((Int32)newD);
            return new Rational64(((UInt64)(UInt32)newE << 32) | newN);
        }

        public static Rational64 Pow(Rational64 x, Rational64 y) {
            // x^0 = 1 for any x, including 0^0 = 1 (matches Math.Pow).
            if (y.bits == 0) return One;

            // 0^y for y != 0
            if (x.bits == 0) {
                if (y.bits > 0) return Zero;            // 0^positive = 0
                                                        // 0^negative = 1/0 -> DivideByZeroException.
                UInt32 zero = unchecked((UInt32)x.bits);   // x.bits == 0 -> zero == 0
                _ = zero / zero;
                return default;                         // unreachable
            }

            // Decode the exponent y = p / qAbs, with p signed, qAbs > 0, gcd(|p|, qAbs) = 1.
            Int64 p = y.SignedNumerator;
            Int64 q = y.SignedDenominator;              // nonzero, same sign as y
            Int64 qAbs = q < 0 ? -q : q;

            // |x| = 1 iff numerator == 1 and |denominator| == 1; encoded denom is 0 or -1.
            // Handle separately because RootN cannot represent ±1^(1/q) for q > 32.
            UInt32 xNum = unchecked((UInt32)x.bits);
            Int32 xE = unchecked((Int32)(x.bits >> 32));
            if (xNum == 1u && (xE == 0 || xE == -1)) {
                if (x.bits > 0) {
                    return One;                         // 1^y = 1
                }
                // x = -1
                if ((qAbs & 1) == 0) {
                    // (-1)^(p/qAbs) with qAbs even is complex; no real rational result.
                    Utilities.ThrowHelper.ThrowOnTrue(true);
                }
                // qAbs odd: (-1)^(p/qAbs) = (-1)^p.
                return (p & 1) == 0 ? One : MinusOne;
            }

            // |x| != 1 and x != 0.
            if (qAbs == 1) {
                // Integer exponent.
                return PowN(x, p);
            }

            // Non-integer exponent p/qAbs with qAbs >= 2.
            // No perfect qAbs-th power in [2, 2^32 - 1] exists when qAbs > 32, so for
            // |x| != 1 the qAbs-th root is irrational (or overflows).
            Utilities.ThrowHelper.ThrowOnTrue(qAbs > 32);

            // Even root of a negative base: complex.
            Utilities.ThrowHelper.ThrowOnTrue(x.bits < 0 && (qAbs & 1) == 0);

            // x^(p/qAbs) = (x^(1/qAbs))^p.  Root first keeps the intermediate small;
            // PowN's bounds then check the true final result, not a proxy.
            Rational64 rootBase = RootN(x, (Int32)qAbs);
            return PowN(rootBase, p);
        }

        public static Rational64 Hypot(Rational64 x, Rational64 y) {
            unchecked {
                UInt32 n1 = (UInt32)x.bits;
                UInt32 n2 = (UInt32)y.bits;
                if (n1 == 0u && n2 == 0u) return default;

                Int32 e1 = (Int32)(x.bits >> 32);
                Int32 e2 = (Int32)(y.bits >> 32);
                // hypot only depends on the magnitudes, so drop the signs of d1, d2.
                UInt32 d1 = e1 >= 0 ? (UInt32)e1 + 1 : (UInt32)(-e1);
                UInt32 d2 = e2 >= 0 ? (UInt32)e2 + 1 : (UInt32)(-e2);

                // x² + y² = (n1²·A2² + n2²·A1²) / (A1²·A2²)
                UInt64 n1s = (UInt64)n1 * n1;
                UInt64 n2s = (UInt64)n2 * n2;
                UInt64 d1s = (UInt64)d1 * d1;
                UInt64 d2s = (UInt64)d2 * d2;

                UInt128 num = Math.BigMul(n1s, d2s) + Math.BigMul(n2s, d1s);
                UInt128 den = Math.BigMul(d1s, d2s);

                var s = DoubleArithmetic.BigSqrtRem(num.GetLowPart(), num.GetHighPart(), out var rem_lo, out var rem_hi);
                Utilities.ThrowHelper.ThrowOnNonZero(rem_lo | rem_hi);

                // den == (A1·A2)², so its integer sqrt is exact.
                UInt128 t = Math.BigMul(d1, d2);

                // Reduce s/t; result is canonical because inputs are canonical.
                var t1 = DoubleArithmetic.BigRemInternal((UInt64)t, (UInt64)(t >> 64), s);

                var g = Mathematics.NumberTheory.EuclideanAlgorithm.GreatestCommonDivisor(s, t1);
                s /= g;
                t /= g;

                // Encoding limits: numerator < 2^32, positive denominator ≤ Int32.MaxValue.
                Utilities.ThrowHelper.ThrowOnTrue(t > (UInt32)Int32.MinValue);
                Utilities.ThrowHelper.ThrowOnGreaterThan(s, UInt32.MaxValue);

                UInt32 newN = (UInt32)s;
                Int32 newE = (Int32)((Int64)t - 1);               // t > 0, encoded as t - 1
                return new Rational64(((UInt64)(UInt32)newE << 32) | newN);
            }
        }


        public static Rational64 RootN(Rational64 x, int n) {
            if (n == 0) {
                throw new ArgumentOutOfRangeException(nameof(n));
            }
            if (n == 1) {
                return x;
            }
            if (n == -1) {
                return Inverse(x);
            }
            if (n < 0) {
                // x^(1/n) = (1/x)^(1/|n|).  -n fits in Int64 even for Int32.MinValue.
                // Inverse(0) throws DivideByZeroException, which is the correct
                // answer for 0^(negative).
                return RootNPositive(Inverse(x), -(Int64)n);
            }
            return RootNPositive(x, n);
        }

        // n >= 2.  Returns x^(1/n) exactly, or throws OverflowException if the
        // exact result is not representable in Rational64.
        private static Rational64 RootNPositive(Rational64 x, Int64 n) {
            Debug.Assert(n >= 2);
            unchecked {
                UInt32 num = (UInt32)x.bits;
                if (0u == num) {
                    // 0^(1/n) = 0 for any n >= 1
                    return default;
                }

                Int32 e = (Int32)(x.bits >> 32);
                Int64 d = e >= 0 ? (Int64)e + 1 : (Int64)e;
                bool neg = d < 0;

                // even root of a negative rational: not real
                Utilities.ThrowHelper.ThrowOnTrue(neg && (n & 1L) == 0L);   // OverflowException

                UInt32 absD = neg ? unchecked((UInt32)(-d)) : (UInt32)d;

                UInt32 rn;
                if (num <= 1u) {
                    rn = num;                                  // 1
                } else if (n > 32) {
                    // 2^33 > UInt32.MaxValue, so 1 is the only positive perfect n-th
                    // power in [0, 2^32 - 1].  Since num >= 2 we cannot be exact.
                    Utilities.ThrowHelper.ThrowOnTrue(true);   // OverflowException
                    throw null!;
                } else {
                    var exactN = TryIntegerRootNExact(num, (Int32)n, out rn);
                    Utilities.ThrowHelper.ThrowOnFalse(exactN);
                }

                UInt32 rd;
                if (absD <= 1u) {
                    rd = absD;                                 // 1
                } else if (n > 32) {
                    Utilities.ThrowHelper.ThrowOnTrue(true);
                    throw null!;
                } else {
                    var exactD = TryIntegerRootNExact(absD, (Int32)n, out rd);
                    Utilities.ThrowHelper.ThrowOnFalse(exactD);
                }

                // rd >= 1, so the encoding is well defined.
                Int32 newE = neg
                    ? checked((Int32)(-(Int64)rd))             // d' = -rd -> e = d'
                    : checked((Int32)((Int64)rd - 1));         // d' = +rd -> e = d' - 1
                return new Rational64(((UInt64)(UInt32)newE << 32) | rn);
            }
        }

        // floor(x^(1/n)) with exactness flag.  Preconditions: x >= 2, 2 <= n <= 32.
        private static bool TryIntegerRootNExact(UInt32 x, Int32 n, out UInt32 root) {
            Debug.Assert(x >= 2);
            Debug.Assert(n >= 2 && n <= 32);
            unchecked {
                // 65535 is the largest possible integer root of any UInt32 for n >= 2
                // (65535^2 = 4294836225 <= 2^32 - 1 < 65536^2).
                UInt32 lo = 1;
                UInt32 hi = 65535u;
                while (lo < hi) {
                    UInt32 mid = (UInt32)(((UInt64)lo + hi + 1) / 2);
                    if (PowLE(mid, n, x)) lo = mid;
                    else hi = mid - 1;
                }
                // lo == floor(x^(1/n))
                UInt64 p = 1;
                for (Int32 i = 0; i < n; i++) {
                    p *= lo;
                    if (p > x) {
                        // cannot happen because lo is a lower bound, but keep it defensive
                        root = lo;
                        return false;
                    }
                }
                root = lo;
                return p == x;
            }
        }

        // Returns true iff b^n <= limit.  Short circuits as soon as the partial
        // product exceeds limit, so n may be arbitrarily large.
        private static bool PowLE(UInt32 b, Int32 n, UInt32 limit) {
            unchecked {
                UInt64 p = 1;
                for (Int32 i = 0; i < n; i++) {
                    p *= b;
                    if (p > limit) return false;
                }
                return true;
            }
        }

        public static Rational64 Sqrt(Rational64 x) {
            unchecked {
                UInt32 n = (UInt32)x.bits;
                if (0u == n) {
                    // sqrt(0) = 0
                    return default;
                }

                Int32 e = (Int32)(x.bits >> 32);
                Utilities.ThrowHelper.ThrowOnNegative(e);
                Int64 d = e >= 0 ? (Int64)e + 1 : (Int64)e;

                UInt32 absD = (UInt32)d;

                // numerator must be a perfect square
                UInt32 sn = UltimateOrb.Mathematics.Elementary.Math.SqrtRem(n, out UInt32 rn);
                Utilities.ThrowHelper.ThrowOnNonZero(rn);


                // denominator must be a perfect square
                UInt32 sd = UltimateOrb.Mathematics.Elementary.Math.SqrtRem(absD, out UInt32 rd);
                Utilities.ThrowHelper.ThrowOnNonZero(rd);

                // sqrt(n/absD) = sn/sd, sd >= 1 so encoded denominator = sd - 1
                Int32 newE = checked((Int32)((Int64)sd - 1));
                return new Rational64(((UInt64)(UInt32)newE << 32) | sn);
            }
        }

        public static Rational64 Exp(Rational64 x) {
            // e^0 = 1
            if (x.bits == 0) return One;
            // For every nonzero algebraic x, e^x is transcendental (Lindemann-Weierstrass),
            // hence not representable as a rational.
            Utilities.ThrowHelper.ThrowOnTrue(true);
            return default;                              // unreachable
        }

        public static Rational64 Exp2(Rational64 x) {
            if (x.bits == 0) return One;

            // 2^x is rational iff x is an integer.
            Utilities.ThrowHelper.ThrowOnFalse(IsInteger(x));

            Int64 n = ToInt64(x);                        // safe: x is integer (absD = 1)
                                                         // n ∈ [-31, 31] \{0} encodes exactly; outside that range overflow.
            Utilities.ThrowHelper.ThrowOnTrue(n > 31 || n < -31);

            if (n > 0) {
                // 2^n, n ∈ [1, 31], d = 1 => encoded denominator = 0
                UInt32 p = unchecked(1u << (Int32)n);
                return new Rational64((UInt64)p);
            } else {
                // 2^-k = 1/2^k, k ∈ [1, 31]; encoded denominator = 2^k - 1
                Int32 k = (Int32)(-n);
                UInt32 absD = unchecked(1u << k);
                // 2^31 encodes to Int32.MaxValue (d = 2^31), so no signed overflow here
                Int32 newE = unchecked((Int32)absD - 1);
                return new Rational64(((UInt64)(UInt32)newE << 32) | 1UL);
            }
        }

        public static Rational64 Exp10(Rational64 x) {
            if (x.bits == 0) return One;

            Utilities.ThrowHelper.ThrowOnFalse(IsInteger(x));

            Int64 n = ToInt64(x);
            // n ∈ [-9, 9] \{0}: 10^9 ≤ UInt32.MaxValue and 10^9 ≤ 2^31
            Utilities.ThrowHelper.ThrowOnTrue(n > 9 || n < -9);

            if (n > 0) {
                // 10^n, n ∈ [1, 9], d = 1
                return new Rational64((UInt64)SmallExp10((Int32)n));
            } else {
                // 10^-k = 1/10^k, k ∈ [1, 9]; encoded denominator = 10^k - 1
                UInt32 absD = SmallExp10((Int32)(-n));
                Int32 newE = unchecked((Int32)absD - 1);
                return new Rational64(((UInt64)(UInt32)newE << 32) | 1UL);
            }

            // 10^n for n in [0, 9]; caller guarantees the range.
            static UInt32 SmallExp10(Int32 n) {
                unchecked {
                    UInt32 r = 1u;
                    for (Int32 i = 0; i < n; i++) {
                        r *= 10u;
                    }
                    return r;
                }

            }
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
