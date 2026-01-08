using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {




    [Experimental("UoWIP")]
    public readonly partial struct BigFloatingPointContext {
        // Public total precision as long (implicit leading 1 included)
        public long Precision { get; }

        // Private logical stored fractional bits as nuint (Precision - 1)
        private readonly nuint _logicalStoredSignificandBitLength;

        public FloatingPointRounding Rounding { get; }

        public static readonly BigFloatingPointContext Default =
            new BigFloatingPointContext(237L, FloatingPointRounding.ToNearestWithMidpointToEven);

        public BigFloatingPointContext(long precision, FloatingPointRounding rounding = FloatingPointRounding.ToNearestWithMidpointToEven) {
            if (precision < 1L) throw new ArgumentOutOfRangeException(nameof(precision), "Precision must be >= 1.");
            long stored = checked(precision - 1L);
            if ((ulong)stored > (ulong)nuint.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision too large for this platform.");
            Precision = precision;
            _logicalStoredSignificandBitLength = (nuint)stored;
            Rounding = rounding;
        }

        // Internal accessor returning nuint
        internal nuint GetLogicalStoredSignificandBitLength() => _logicalStoredSignificandBitLength;
    }

    [Experimental("UoWIP")]
    public readonly partial struct BigFloat : IComparable<BigFloat>, IEquatable<BigFloat> {
        // value = sign * (1 + fraction / 2^storedBits) * 2^exponent
        private readonly BigInteger _fraction; // stored fractional bits (masked to _logicalStoredSignificandBitLength)
        private readonly long _exponent;
        private readonly int _sign; // bitfield: sign + special prefix + flags
        private readonly nuint _logicalStoredSignificandBitLength; // private stored bits

        // Masks and constants (unchecked for literal overflow safety)
        private const int SignBitMask = unchecked((int)0x8000_0000);        // bit 31
        private const int SpecialPrefixMask = unchecked((int)0x7800_0000);  // bits 30..27 mask
        private const int SpecialPrefixValue = unchecked((int)0x7800_0000); // bits 30..27 == 1111
        private const int InfNaNBitMask = 0x0400_0000;                      // bit 26: 0 -> Inf, 1 -> NaN
        private const int QuietSignalingBitMask = 0x0200_0000;              // bit 25: 0 -> qNaN, 1 -> sNaN

        // Public total precision as long (implicit leading 1 included)
        public long Precision => (long)_logicalStoredSignificandBitLength + 1L;

        // Default stored bits derived from default context
        private static readonly nuint DefaultStoredBits = (nuint)(BigFloatingPointContext.Default.Precision - 1L);

        // Private constructor expects logicalStoredSignificandBitLength as nuint
        private BigFloat(BigInteger fraction, long exponent, int signField, nuint logicalStoredSignificandBitLength) {
            if ((ulong)logicalStoredSignificandBitLength > (ulong)int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(logicalStoredSignificandBitLength), "Stored bit length too large for runtime shifts.");

            _logicalStoredSignificandBitLength = logicalStoredSignificandBitLength;
            _sign = signField;

            if ((_sign & SignBitMask) == 0 && fraction.IsZero && signField == 0) {
                // canonical zero: sign==0 and fraction==0 and exponent==0
                _fraction = BigInteger.Zero;
                _exponent = 0L;
                // keep _sign as provided (0 for +0, SignBitMask for -0 if you want to preserve -0)
            } else if (IsSpecialSignField(_sign)) {
                // For special values, fraction may hold NaN payload; exponent ignored
                _fraction = MaskFraction(fraction);
                _exponent = 0L;
            } else {
                // Normal number: mask fraction to stored bits and store exponent
                _fraction = MaskFraction(fraction);
                _exponent = exponent;
            }
        }

        // Public canonical zeros, one
        public static BigFloat Zero => new BigFloat(BigInteger.Zero, 0L, 0, DefaultStoredBits);
        public static BigFloat NegativeZero => new BigFloat(BigInteger.Zero, 0L, SignBitMask, DefaultStoredBits);
        public static BigFloat One => new BigFloat(BigInteger.Zero, 0L, 0, DefaultStoredBits);

        // Helpers for masks and conversions
        private int StoredBitsAsInt() => checked((int)_logicalStoredSignificandBitLength);

        private BigInteger ImplicitOne => BigInteger.One << StoredBitsAsInt();

        private BigInteger MaskFraction(BigInteger frac) {
            int sb = StoredBitsAsInt();
            return sb == 0 ? BigInteger.Zero : (frac & ((BigInteger.One << sb) - 1));
        }

        // Special sign-field helpers
        private static bool IsSpecialSignField(int signField) => (signField & SpecialPrefixMask) == SpecialPrefixValue;
        private static int MakeNormalSignField(bool negative) => negative ? SignBitMask : 0;
        private static int MakeSpecialField(bool negative, bool isNaN, bool signalingNaN) {
            int f = SpecialPrefixValue;
            if (isNaN) f |= InfNaNBitMask;
            if (signalingNaN) f |= QuietSignalingBitMask;
            if (negative) f |= SignBitMask;
            return f;
        }

        private bool IsSpecial() => (_sign & SpecialPrefixMask) == SpecialPrefixValue;
        private bool IsNaN() => IsSpecial() && (_sign & InfNaNBitMask) != 0;
        private bool IsInfinity() => IsSpecial() && (_sign & InfNaNBitMask) == 0;
        private bool IsSignalingNaN() => IsNaN() && (_sign & QuietSignalingBitMask) != 0;
        private bool IsQuietNaN() => IsNaN() && (_sign & QuietSignalingBitMask) == 0;
        private bool IsNegative() => (_sign & SignBitMask) != 0;
        private bool IsZero() => _sign == 0 && _fraction.IsZero;

        // Factories for special values
        public static BigFloat Infinity(bool negative = false)
            => new BigFloat(BigInteger.Zero, 0L, MakeSpecialField(negative, isNaN: false, signalingNaN: false), DefaultStoredBits);

        public static BigFloat NaN(bool signaling = false, bool negative = false, BigInteger? payload = null)
            => new BigFloat(payload ?? BigInteger.Zero, 0L, MakeSpecialField(negative, isNaN: true, signalingNaN: signaling), DefaultStoredBits);

        // Create from integer with specified total precision (public API uses totalPrecision)
        public static BigFloat FromInteger(long value, long totalPrecision) {
            if (totalPrecision < 1L) throw new ArgumentOutOfRangeException(nameof(totalPrecision));
            long stored = checked(totalPrecision - 1L);
            if ((ulong)stored > (ulong)nuint.MaxValue) throw new ArgumentOutOfRangeException(nameof(totalPrecision));
            nuint storedBits = (nuint)stored;
            if (value == 0) return new BigFloat(BigInteger.Zero, 0L, 0, storedBits);
            int sign = value < 0 ? MakeNormalSignField(true) : MakeNormalSignField(false);
            BigInteger abs = BigInteger.Abs(new BigInteger(value));
            // Represent as 1.fraction * 2^exponent by shifting until top bit fits implicit one
            int bitlen = GetBitLength(abs);
            long exponent = bitlen - 1;
            BigInteger full = abs;
            // remove implicit one
            BigInteger frac = full - (BigInteger.One << checked((int)exponent));
            // align fraction to stored bits
            int sb = checked((int)storedBits);
            if (sb == 0) frac = BigInteger.Zero;
            else if (exponent >= sb) {
                // shift right with rounding toward zero
                frac >>= (int)exponent - sb;
                frac &= (BigInteger.One << sb) - 1;
            } else {
                frac <<= sb - (int)exponent;
                frac &= (BigInteger.One << sb) - 1;
            }
            return new BigFloat(frac, exponent, sign, storedBits);
        }

        // Bit-length helper
        private static int GetBitLength(BigInteger v) {
            if (v.IsZero) return 0;
            v = BigInteger.Abs(v);
            int bits = 0;
            while (v != 0) { v >>= 1; bits++; }
            return bits;
        }

        // ToString with special handling
        public override string ToString() {
            if (IsNaN()) return "NaN";
            if (IsInfinity()) return IsNegative() ? "-Infinity" : "Infinity";
            if (IsZero()) return "0";
            string frac = _fraction == 0 ? "0" : ConvertToBinaryString(_fraction).PadLeft(StoredBitsAsInt(), '0');
            return $"{(IsNegative() ? "-" : "")}1.{frac}b * 2^{_exponent} Precision={Precision}";
        }

        private static string ConvertToBinaryString(BigInteger v) {
            if (v.IsZero) return "0";
            var sb = new StringBuilder();
            BigInteger t = v;
            while (t > 0) {
                sb.Insert(0, (t & 1) == 1 ? '1' : '0');
                t >>= 1;
            }
            return sb.ToString();
        }

        // Equality and hashing
        public override bool Equals(object? obj) {
            if (!(obj is BigFloat other)) return false;
            if (this.IsNaN() || other.IsNaN()) return false; // NaN != anything
            if (this.IsInfinity() || other.IsInfinity())
                return this.IsInfinity() && other.IsInfinity() && this.IsNegative() == other.IsNegative();
            // treat +0 and -0 as equal
            if (this._sign == 0 && this._fraction.IsZero && other._sign == 0 && other._fraction.IsZero) return true;
            return _sign == other._sign && _exponent == other._exponent
                   && _fraction == other._fraction && _logicalStoredSignificandBitLength == other._logicalStoredSignificandBitLength;
        }

        public bool Equals(BigFloat other) => Equals((object)other);

        public override int GetHashCode()
            => HashCode.Combine(_sign, _fraction, _exponent, (long)_logicalStoredSignificandBitLength);

        // Comparison (partial; respects sign and exponent; special values handled)
        public int CompareTo(BigFloat other) {
            // Handle NaN: by convention, throw or treat as unordered; here throw
            if (this.IsNaN() || other.IsNaN()) throw new InvalidOperationException("Comparison with NaN is unordered.");

            // Handle infinities
            if (this.IsInfinity() || other.IsInfinity()) {
                if (this.IsInfinity() && other.IsInfinity()) {
                    // compare sign
                    return this.IsNegative().CompareTo(other.IsNegative());
                }
                if (this.IsInfinity()) return this.IsNegative() ? -1 : 1;
                return other.IsNegative() ? 1 : -1;
            }

            // Handle zeros
            if (this.IsZero() && other.IsZero()) return 0;

            // Compare signs
            if (this.IsNegative() != other.IsNegative()) return this.IsNegative() ? -1 : 1;

            // Align stored bits for comparison
            int p = Math.Max(StoredBitsAsInt(), other.StoredBitsAsInt());
            BigInteger aFull = ((BigInteger.One << StoredBitsAsInt()) + _fraction) << (p - StoredBitsAsInt());
            BigInteger bFull = ((BigInteger.One << other.StoredBitsAsInt()) + other._fraction) << (p - other.StoredBitsAsInt());

            if (_exponent != other._exponent) return _exponent.CompareTo(other._exponent) * (this.IsNegative() ? -1 : 1);

            int cmp = aFull.CompareTo(bFull);
            return this.IsNegative() ? -cmp : cmp;
        }

        // Basic arithmetic entry points with special-value handling
        public BigFloat Add(BigFloat other, BigFloatingPointContext? ctx = null) {
            // Special propagation
            if (this.IsNaN()) return this;
            if (other.IsNaN()) return other;

            if (this.IsInfinity() || other.IsInfinity()) {
                if (this.IsInfinity() && other.IsInfinity()) {
                    // same sign -> infinity; opposite sign -> NaN
                    if (this.IsNegative() == other.IsNegative()) return Infinity(this.IsNegative());
                    return NaN(signaling: false);
                }
                return this.IsInfinity() ? this : other;
            }

            // Normal addition: align exponents and add significands
            int p = ResolveStoredBitsAsInt(ctx, _logicalStoredSignificandBitLength, other._logicalStoredSignificandBitLength);
            // Build full significands
            BigInteger aFull = ((BigInteger.One << StoredBitsAsInt()) + _fraction) << (p - StoredBitsAsInt());
            BigInteger bFull = ((BigInteger.One << other.StoredBitsAsInt()) + other._fraction) << (p - other.StoredBitsAsInt());

            long aExp = _exponent;
            long bExp = other._exponent;

            // Align by exponent difference
            long diff = aExp - bExp;
            BigInteger sticky = BigInteger.Zero;
            if (diff > 0) {
                if (diff > int.MaxValue) // treat as effectively zero
                {
                    bFull = BigInteger.Zero;
                } else {
                    int d = checked((int)diff);
                    if (d >= p + 2) // shifted out entirely
                    {
                        sticky = bFull != 0 ? BigInteger.One : BigInteger.Zero;
                        bFull = BigInteger.Zero;
                    } else {
                        BigInteger remMask = (BigInteger.One << d) - 1;
                        sticky = (bFull & remMask) != 0 ? BigInteger.One : BigInteger.Zero;
                        bFull >>= d;
                    }
                }
                long resultExp = aExp;
                BigInteger sum = (this.IsNegative() ? -aFull : aFull) + (other.IsNegative() ? -bFull : bFull);
                return NormalizeAndRound(sum, resultExp, p, ctx);
            } else if (diff < 0) {
                diff = -diff;
                if (diff > int.MaxValue) {
                    aFull = BigInteger.Zero;
                } else {
                    int d = checked((int)diff);
                    if (d >= p + 2) {
                        sticky = aFull != 0 ? BigInteger.One : BigInteger.Zero;
                        aFull = BigInteger.Zero;
                    } else {
                        BigInteger remMask = (BigInteger.One << d) - 1;
                        sticky = (aFull & remMask) != 0 ? BigInteger.One : BigInteger.Zero;
                        aFull >>= d;
                    }
                }
                long resultExp = bExp;
                BigInteger sum = (this.IsNegative() ? -aFull : aFull) + (other.IsNegative() ? -bFull : bFull);
                return NormalizeAndRound(sum, resultExp, p, ctx);
            } else {
                // same exponent
                long resultExp = aExp;
                BigInteger sum = (this.IsNegative() ? -aFull : aFull) + (other.IsNegative() ? -bFull : bFull);
                return NormalizeAndRound(sum, resultExp, p, ctx);
            }
        }

        public BigFloat Multiply(BigFloat other, BigFloatingPointContext? ctx = null) {
            if (this.IsNaN()) return this;
            if (other.IsNaN()) return other;

            if (this.IsInfinity() || other.IsInfinity()) {
                // Inf * 0 -> NaN
                bool aZero = this.IsZero();
                bool bZero = other.IsZero();
                if (aZero || bZero) return NaN(signaling: false);
                // result is infinity with sign = xor
                bool neg = this.IsNegative() ^ other.IsNegative();
                return Infinity(neg);
            }

            if (this.IsZero() || other.IsZero()) return Zero;

            int p = ResolveStoredBitsAsInt(ctx, _logicalStoredSignificandBitLength, other._logicalStoredSignificandBitLength);

            BigInteger aFull = ((BigInteger.One << StoredBitsAsInt()) + _fraction) << (p - StoredBitsAsInt());
            BigInteger bFull = ((BigInteger.One << other.StoredBitsAsInt()) + other._fraction) << (p - other.StoredBitsAsInt());

            BigInteger prod = aFull * bFull;
            int prodBits = GetBitLength(prod);
            int targetBits = p + 1;
            int shift = prodBits - targetBits;
            long newExp = checked(_exponent + other._exponent);

            if (shift > 0) {
                // right shift with simple rounding to nearest
                BigInteger divisor = BigInteger.One << shift;
                BigInteger rem = prod & (divisor - 1);
                prod >>= shift;
                // round to nearest (tie -> round up)
                if (rem * 2 >= divisor) prod += 1;
                newExp = checked(newExp + shift);
            } else if (shift < 0) {
                prod <<= -shift;
            }

            // normalize so top bit equals implicit one
            BigInteger implicitOne = BigInteger.One << p;
            if (prod < implicitOne) {
                prod <<= 1;
                newExp = checked(newExp - 1);
            }

            BigInteger newFrac = (prod - implicitOne) & ((BigInteger.One << p) - 1);
            int signField = MakeNormalSignField(this.IsNegative() ^ other.IsNegative());
            return new BigFloat(newFrac, newExp, signField, (nuint)p);
        }

        public BigFloat Divide(BigFloat other, BigFloatingPointContext? ctx = null) {
            if (this.IsNaN()) return this;
            if (other.IsNaN()) return other;

            if (other.IsZero()) {
                if (this.IsZero()) return NaN(signaling: false);
                // finite / 0 -> Inf with sign
                return Infinity(this.IsNegative() ^ other.IsNegative());
            }

            if (this.IsInfinity() && other.IsInfinity()) return NaN(signaling: false);
            if (this.IsInfinity()) return Infinity(this.IsNegative() ^ other.IsNegative());
            if (this.IsZero()) return Zero;

            int p = ResolveStoredBitsAsInt(ctx, _logicalStoredSignificandBitLength, other._logicalStoredSignificandBitLength);

            BigInteger aFull = ((BigInteger.One << StoredBitsAsInt()) + _fraction) << (p - StoredBitsAsInt());
            BigInteger bFull = ((BigInteger.One << other.StoredBitsAsInt()) + other._fraction) << (p - other.StoredBitsAsInt());

            // compute numerator shifted to preserve precision
            int k = 2; // extra guard bits
            BigInteger numerator = aFull << (p + k);
            BigInteger quotient = BigInteger.DivRem(numerator, bFull, out BigInteger rem);
            long newExp = checked(_exponent - other._exponent - (p + k));

            int bits = GetBitLength(quotient);
            int shift = bits - (p + 1);
            if (shift > 0) {
                BigInteger divisor = BigInteger.One << shift;
                BigInteger r = quotient & (divisor - 1);
                quotient >>= shift;
                if (r * 2 >= divisor) quotient += 1;
                newExp = checked(newExp + shift);
            } else if (shift < 0) {
                quotient <<= -shift;
            }

            BigInteger implicitOne = BigInteger.One << p;
            if (quotient < implicitOne) {
                quotient <<= 1;
                newExp = checked(newExp - 1);
            }

            BigInteger newFrac = (quotient - implicitOne) & ((BigInteger.One << p) - 1);
            int signField = MakeNormalSignField(this.IsNegative() ^ other.IsNegative());
            return new BigFloat(newFrac, newExp, signField, (nuint)p);
        }

        // Normalize and round a signed full significand value into BigFloat
        private BigFloat NormalizeAndRound(BigInteger signedFull, long resultExp, int p, BigFloatingPointContext? ctx) {
            // signedFull may be negative; handle sign and absolute
            bool negative = signedFull < 0;
            BigInteger abs = signedFull < 0 ? -signedFull : signedFull;

            if (abs.IsZero) return new BigFloat(BigInteger.Zero, 0L, 0, (nuint)p);

            // Determine bit length and normalize to p+1 bits (implicit + stored)
            int bits = GetBitLength(abs);
            int shift = bits - (p + 1);
            if (shift > 0) {
                BigInteger divisor = BigInteger.One << shift;
                BigInteger rem = abs & (divisor - 1);
                abs >>= shift;
                // rounding based on context
                bool inc = false;
                var rounding = ctx?.Rounding ?? BigFloatingPointContext.Default.Rounding;
                switch (rounding) {
                case FloatingPointRounding.ToNearest:
                    inc = rem * 2 >= divisor;
                    break;
                case FloatingPointRounding.TowardZero:
                    inc = false;
                    break;
                case FloatingPointRounding.TowardPositiveInfinity:
                    inc = !negative && rem != 0;
                    break;
                case FloatingPointRounding.TowardNegativeInfinity:
                    inc = negative && rem != 0;
                    break;
                }
                if (inc) abs += 1;
                resultExp = checked(resultExp + shift);
            } else if (shift < 0) {
                abs <<= -shift;
            }

            BigInteger implicitOne = BigInteger.One << p;
            if (abs < implicitOne) {
                abs <<= 1;
                resultExp = checked(resultExp - 1);
            }

            BigInteger frac = (abs - implicitOne) & ((BigInteger.One << p) - 1);
            int signField = MakeNormalSignField(negative);
            return new BigFloat(frac, resultExp, signField, (nuint)p);
        }

        // Resolve stored bits as int for operations
        private static int ResolveStoredBitsAsInt(BigFloatingPointContext? ctx, nuint aStored, nuint bStored) {
            if (ctx.HasValue) {
                long total = ctx.Value.Precision;
                if (total < 1L) throw new ArgumentOutOfRangeException(nameof(ctx));
                long stored = checked(total - 1L);
                if ((ulong)stored > (ulong)int.MaxValue) throw new ArgumentOutOfRangeException(nameof(ctx), "Precision too large for runtime shifts.");
                return checked((int)stored);
            }
            nuint max = aStored > bStored ? aStored : bStored;
            if ((ulong)max > (ulong)int.MaxValue) throw new ArgumentOutOfRangeException("Resolved stored bits too large for runtime shifts.");
            return checked((int)max);
        }
    }
}
