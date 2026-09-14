using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Text;

using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using UltimateOrb.Runtime.CompilerServices.TypeTokens;
    using static FsCheck.ResultContainer;

    static partial class BitsTypeTraits<T> where T : IBinaryInteger<T>, IMinMaxValue<T> {

        public static bool IsSigned { get; } = T.IsNegative(T.MinValue);

        public static T SignBit => BitsTypeTraits<T>.IsSigned ? T.MinValue : ~(T.AllBitsSet >>> 1);

        public static long BitWidth { get; } = IsSigned ?
            (T.IsZero(T.MaxValue) ? 1 : checked(2 + long.CreateChecked(T.Log2(T.MaxValue ^ (T.MaxValue >> 1))))) :
            (T.IsZero(T.MaxValue) ? 0 : checked(1 + long.CreateChecked(T.Log2(T.MaxValue ^ (T.MaxValue >> 1)))));
    }

    public static partial class UnsignedViaSigned {


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static (TInt Quotient, TInt Remainder) DivRemUnsigned<TInt>(TInt dividend, TInt divisor)
            where TInt : IBinaryInteger<TInt>/*, ISignedNumber<TInt>*/, IMinMaxValue<TInt> {
            if (divisor == TInt.Zero) {
                _ = checked(dividend / divisor);
            }
            unchecked {
                bool dividendNeg = dividend < TInt.Zero;
                bool divisorNeg = divisor < TInt.Zero;
                if (!dividendNeg && divisorNeg) {
                    return (TInt.Zero, dividend);
                }
                if (dividendNeg && divisorNeg) {
                    if (dividend < divisor) {
                        return (TInt.Zero, dividend);
                    }
                    return (TInt.One, dividend - divisor);
                }
                if (!dividendNeg) {
                    return TInt.DivRem(dividend, divisor);
                }
                TInt low = dividend & TInt.MaxValue;
                var (negQ, negR) = TInt.DivRem(TInt.MinValue, divisor);
                var (qLow, rLow) = TInt.DivRem(low, divisor);
                TInt q = -negQ + qLow;
                TInt r = -negR + rLow;
                if (r >= divisor) { r -= divisor; q += TInt.One; }
                return (q, r);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static TInt ILog2<TInt>(TInt value)
            where TInt : IBinaryInteger<TInt>/*, ISignedNumber<TInt>*/, IMinMaxValue<TInt> {
            if (value >= TInt.Zero) {
                return TInt.Log2(value);
            } else {
                var r = TInt.Log2(value >>> 1);
                ++r;
                return r;
            }
        }

        [Conditional("DEBUG")]
        static void CheckISignedNumber<TInt>() where TInt : IBinaryInteger<TInt>/*, ISignedNumber<TInt>*/, IMinMaxValue<TInt> {

        }

        internal static int GetShortestBitLength<T>(T value) where T : IBinaryInteger<T>, IMinMaxValue<T> {
            if (value >= T.Zero) {
                return value.GetShortestBitLength();
            } else {
                var r = (value >>> 1).GetShortestBitLength();
                ++r;
                return r;
            }
        }
    }

    public readonly struct Unsigned<T> :
        IBinaryInteger<Unsigned<T>>,
        IUnsignedNumber<Unsigned<T>>,
        IMinMaxValue<Unsigned<T>>
        where T : IBinaryInteger<T>, IMinMaxValue<T> {
        readonly T _bits;

        internal Unsigned(T bits) => _bits = bits;

        // ------------------------------------------------------------------
        // helpers
        // ------------------------------------------------------------------
        static Unsigned<T> Wrap(T v) => new(v);



        static int Compare(Unsigned<T> a, Unsigned<T> b)
            => BitsTypeTraits<T>.IsSigned ? (a._bits ^ T.MinValue).CompareTo(b._bits ^ T.MinValue) : a._bits.CompareTo(b._bits);

        // Binary long division.  O(bits²) but no BigInteger.
        static (Unsigned<T> Quotient, Unsigned<T> Remainder) DivRem(Unsigned<T> a, Unsigned<T> b) {
            T q, r;
            if (BitsTypeTraits<T>.IsSigned) {
                (q, r) = UnsignedViaSigned.DivRemUnsigned(a._bits, b._bits);
            } else {
                (q, r) = T.DivRem(a._bits, b._bits);
            }
            return (Wrap(q), Wrap(r));
        }

        // Only used for parsing / formatting / conversions.
        static BigInteger ToBig(Unsigned<T> v) {
            int n = v._bits.GetByteCount();          // full size for built-ins
            var bytes = new byte[n];
            bool s;
            if (BitConverter.IsLittleEndian) {
                s = v._bits.TryWriteLittleEndian(bytes.AsSpan(0, n), out _);
            } else {
                s = v._bits.TryWriteBigEndian(bytes.AsSpan(0, n), out _);
            }
            return new BigInteger(bytes, isUnsigned: true, isBigEndian: !BitConverter.IsLittleEndian);
        }

        static bool TryFromBig(BigInteger v, out Unsigned<T> result) {
            if (v.Sign < 0) {
                result = default;
                return false;
            }

            int n = BitsTypeTraits<T>.SignBit.GetByteCount();
            Span<byte> buf = stackalloc byte[n];
            buf.Clear();

            bool isLittle = BitConverter.IsLittleEndian;

            // Write the BigInteger using the machine's native endianness and unsigned form.
            // This avoids the allocation from ToByteArray() and skips the sign-byte trimming.
            if (!v.TryWriteBytes(buf, out int len, isUnsigned: true, isBigEndian: !isLittle)) {
                result = default;
                return false;
            }

            // BigInteger.TryWriteBytes writes to the start of the span.
            // For big-endian, we need leading zeros, so right-align the written bytes.
            if (!isLittle && len < n) {
                buf.Slice(0, len).CopyTo(buf.Slice(n - len));
                buf.Slice(0, n - len).Clear();
            }

            T bits;
            bool ok = isLittle
                ? T.TryReadLittleEndian(buf, isUnsigned: true, out bits)
                : T.TryReadBigEndian(buf, isUnsigned: true, out bits);

            if (!ok) {
                result = default;
                return false;
            }

            result = Wrap(bits);
            return true;
        }

        static Unsigned<T> FromBig(BigInteger v)
            => TryFromBig(v, out var r) ? r : throw new OverflowException();

        // ------------------------------------------------------------------
        // static properties & INumberBase
        // ------------------------------------------------------------------
        public static Unsigned<T> One => Wrap(T.One);
        public static int Radix => T.Radix;
        public static Unsigned<T> Zero => Wrap(T.Zero);
        public static Unsigned<T> AdditiveIdentity => Wrap(unchecked(-T.Zero));
        public static Unsigned<T> MultiplicativeIdentity => One;
        public static Unsigned<T> MaxValue => Wrap(~T.Zero);
        public static Unsigned<T> MinValue => Zero;

        public static Unsigned<T> Abs(Unsigned<T> value) => value;

        public static bool IsCanonical(Unsigned<T> value) => T.IsCanonical(value._bits);
        public static bool IsComplexNumber(Unsigned<T> value) => T.IsComplexNumber(value._bits);
        public static bool IsEvenInteger(Unsigned<T> value) => T.IsEvenInteger(value._bits);
        public static bool IsFinite(Unsigned<T> value) => T.IsFinite(value._bits);
        public static bool IsImaginaryNumber(Unsigned<T> value) => T.IsImaginaryNumber(value._bits);
        public static bool IsInfinity(Unsigned<T> value) => T.IsInfinity(value._bits);
        public static bool IsInteger(Unsigned<T> value) => T.IsInteger(value._bits);
        public static bool IsNaN(Unsigned<T> value) => T.IsNaN(value._bits);
        public static bool IsNegative(Unsigned<T> value) => false;
        public static bool IsNegativeInfinity(Unsigned<T> value) => false;
        public static bool IsNormal(Unsigned<T> value) => T.IsNormal(value._bits);
        public static bool IsOddInteger(Unsigned<T> value) => T.IsOddInteger(value._bits);
        public static bool IsPositive(Unsigned<T> value) => true;
        public static bool IsPositiveInfinity(Unsigned<T> value) => T.IsInfinity(value._bits);
        public static bool IsPow2(Unsigned<T> value) => T.IsPow2(value._bits) || (BitsTypeTraits<T>.IsSigned && value._bits == T.MinValue);
        public static bool IsRealNumber(Unsigned<T> value) => T.IsRealNumber(value._bits);
        public static bool IsSubnormal(Unsigned<T> value) => T.IsSubnormal(value._bits);
        public static bool IsZero(Unsigned<T> value) => T.IsZero(value._bits);

        public static Unsigned<T> Log2(Unsigned<T> value) {
            return Wrap(BitsTypeTraits<T>.IsSigned ? UnsignedViaSigned.ILog2(value._bits) : T.Log2(value._bits));
        }

        public static Unsigned<T> MaxMagnitude(Unsigned<T> x, Unsigned<T> y) => x >= y ? x : y;
        public static Unsigned<T> MaxMagnitudeNumber(Unsigned<T> x, Unsigned<T> y) => MaxMagnitude(x, y);
        public static Unsigned<T> MinMagnitude(Unsigned<T> x, Unsigned<T> y) => x <= y ? x : y;
        public static Unsigned<T> MinMagnitudeNumber(Unsigned<T> x, Unsigned<T> y) => MinMagnitude(x, y);

        public static Unsigned<T> PopCount(Unsigned<T> value) => Wrap(T.PopCount(value._bits));
        public static Unsigned<T> TrailingZeroCount(Unsigned<T> value) => Wrap(T.TrailingZeroCount(value._bits));

        // ---- parsing / formatting (BigInteger allowed) -------------------
        public static Unsigned<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? p)
            => FromBig(BigInteger.Parse(s, style, p));
        public static Unsigned<T> Parse(string s, NumberStyles style, IFormatProvider? p)
            => Parse(s.AsSpan(), style, p);
        public static Unsigned<T> Parse(ReadOnlySpan<char> s, IFormatProvider? p)
            => Parse(s, NumberStyles.Integer, p);
        public static Unsigned<T> Parse(string s, IFormatProvider? p)
            => Parse(s.AsSpan(), NumberStyles.Integer, p);

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? p,
                                    [MaybeNullWhen(false)] out Unsigned<T> result) {
            if (BigInteger.TryParse(s, style, p, out var big)) {
                return TryFromBig(big, out result);
            }

            result = default; return false;
        }
        public static bool TryParse(
            [NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? p, [MaybeNullWhen(false)] out Unsigned<T> result)
            => TryParse(s.AsSpan(), style, p, out result);

        public static bool TryParse(
            ReadOnlySpan<char> s, IFormatProvider? p, [MaybeNullWhen(false)] out Unsigned<T> result)
            => TryParse(s, NumberStyles.Integer, p, out result);

        public static bool TryParse(
            [NotNullWhen(true)] string? s, IFormatProvider? p, [MaybeNullWhen(false)] out Unsigned<T> result)
            => TryParse(s.AsSpan(), NumberStyles.Integer, p, out result);

        // ---- conversions -------


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Unsigned<T> CreateChecked<TOther>(TOther value)
          where TOther : INumberBase<TOther> {
            Unsigned<T> result;

            if (typeof(TOther) == typeof(Unsigned<T>)) {
                result = (Unsigned<T>)(object)value;
            } else if (!TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateSaturating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Unsigned<T> CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            Unsigned<T> result;

            if (typeof(TOther) == typeof(Unsigned<T>)) {
                result = (Unsigned<T>)(object)value;
            } else if (!TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateTruncating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Unsigned<T> CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            Unsigned<T> result;

            if (typeof(TOther) == typeof(Unsigned<T>)) {
                result = (Unsigned<T>)(object)value;
            } else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }


        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Unsigned<T>>.TryConvertFromChecked<TOther>(TOther value, out Unsigned<T> result) => TryConvertFromChecked(value, out result);

        internal static bool TryConvertFromChecked<TOther>(TOther v, [MaybeNullWhen(false)] out Unsigned<T> r)
            where TOther : INumberBase<TOther> {
            // TODO: Avoid BigInteger
            if (!INumberBaseFriendInternal<BigInteger>.TryConvertFromChecked(v, out var big) && !INumberBaseFriendInternal<TOther>.TryConvertToChecked(v, out big)) { r = default; return false; }
            return TryFromBig(big, out r);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Unsigned<T>>.TryConvertFromSaturating<TOther>(TOther value, out Unsigned<T> result) => TryConvertFromSaturating(value, out result);


        internal static bool TryConvertFromSaturating<TOther>(TOther v, [MaybeNullWhen(false)] out Unsigned<T> r)
            where TOther : INumberBase<TOther> {
            // TODO: Avoid BigInteger
            if (!INumberBaseFriendInternal<BigInteger>.TryConvertFromSaturating(v, out var big) && !INumberBaseFriendInternal<TOther>.TryConvertToSaturating(v, out big)) { r = default; return false; }
            if (TryFromBig(big, out r)) {
                return true;
            }
            r = TOther.IsNegative(v) ? Zero : MaxValue;
            return true;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<Unsigned<T>>.TryConvertFromTruncating<TOther>(TOther value, out Unsigned<T> result) => TryConvertFromTruncating(value, out result);

        internal static bool TryConvertFromTruncating<TOther>(TOther v, [MaybeNullWhen(false)] out Unsigned<T> r)
            where TOther : INumberBase<TOther> {
            if (INumberBaseFriendInternal<T>.TryConvertFromTruncating(v, out var t) || INumberBaseFriendInternal<TOther>.TryConvertToTruncating(v, out t)) {
                r = Wrap(t);
                return true;
            }
            r = default;
            return false;
        }

        static bool INumberBase<Unsigned<T>>.TryConvertToChecked<TOther>(Unsigned<T> v, [MaybeNullWhen(false)] out TOther r)
            => TOther.TryConvertFromChecked(ToBig(v), out r);

        static bool INumberBase<Unsigned<T>>.TryConvertToSaturating<TOther>(Unsigned<T> v, [MaybeNullWhen(false)] out TOther r)
            => TOther.TryConvertFromSaturating(ToBig(v), out r);

        static bool INumberBase<Unsigned<T>>.TryConvertToTruncating<TOther>(Unsigned<T> v, [MaybeNullWhen(false)] out TOther r) {
            if (INumberBaseFriendInternal<T>.TryConvertToTruncating(v._bits, out TOther t) || INumberBaseFriendInternal<TOther>.TryConvertFromTruncating(v._bits, out t)) {
                r = t;
                return true;
            }
            r = default;
            return false;
        }

        // ---- endianness --------------------------------------------------
        public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Unsigned<T> value) {
            if (source.Length == 0) {
                value = default; // zero
                return true;
            }

            // Unsigned<T> is unsigned, so if the caller says the source is signed
            // and the sign bit is set, the value is negative and out of range.
            if (!isUnsigned && (source[0] & 0x80) != 0) {
                value = default;
                return false;
            }

            int size = checked((int)((BitsTypeTraits<T>.BitWidth + 7) / 8));

            // For an unsigned target, any bytes beyond the target width must be zero.
            if (source.Length > size && source[..^size].ContainsAnyExcept((byte)0x00)) {
                value = default;
                return false;
            }

            T bits;

            if (BitsTypeTraits<T>.IsSigned) {
                // Signed T cannot represent the full unsigned range as a positive value.
                // But we only need the raw two's-complement bit pattern.
                if (source.Length < size) {
                    // Shorter than T: the unsigned value always fits as a positive signed value,
                    // so ask T to zero-extend it as unsigned.
                    if (!T.TryReadBigEndian(source, isUnsigned: true, out bits)) {
                        value = default;
                        return false;
                    }
                } else {
                    // Exactly T's width, or more with zero padding already checked.
                    // Read the last `size` bytes as a two's-complement bit pattern.
                    // This accepts high-bit-set values and yields the correct raw bits.
                    if (!T.TryReadBigEndian(source[^size..], isUnsigned: false, out bits)) {
                        value = default;
                        return false;
                    }
                }
            } else {
                // Unsigned T can already read the full unsigned range.
                if (!T.TryReadBigEndian(source, isUnsigned: true, out bits)) {
                    value = default;
                    return false;
                }
            }

            value = new Unsigned<T>(bits); // replace with your actual raw-bit constructor/factory
            return true;
        }

        public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Unsigned<T> value) {
            if (source.Length == 0) {
                value = default; // zero
                return true;
            }

            // For little-endian, the sign bit is in the most significant byte,
            // which is the last byte of the span.
            if (!isUnsigned && (source[^1] & 0x80) != 0) {
                value = default;
                return false;
            }

            // BitWidth is in bits; convert to bytes.
            int size = checked((int)((BitsTypeTraits<T>.BitWidth + 7) / 8));

            // For an unsigned target, any extra high-order bytes (at the end)
            // must be zero.
            if (source.Length > size && source[size..].ContainsAnyExcept((byte)0x00)) {
                value = default;
                return false;
            }

            T bits;

            if (BitsTypeTraits<T>.IsSigned) {
                if (source.Length < size) {
                    // Shorter than T: the value is small and positive, so zero-extend.
                    if (!T.TryReadLittleEndian(source, isUnsigned: true, out bits)) {
                        value = default;
                        return false;
                    }
                } else {
                    // At least T's width. Take the first `size` bytes (least significant)
                    // and read them as a signed value to obtain the raw two's-complement bits.
                    if (!T.TryReadLittleEndian(source[..size], isUnsigned: false, out bits)) {
                        value = default;
                        return false;
                    }
                }
            } else {
                // Unsigned T can already read the full unsigned range.
                if (!T.TryReadLittleEndian(source, isUnsigned: true, out bits)) {
                    value = default;
                    return false;
                }
            }

            value = new Unsigned<T>(bits); // replace with your actual raw-bit constructor/factory
            return true;
        }

        // ---- instance members --------------------------------------------
        public int CompareTo(Unsigned<T> other) => Compare(this, other);
        public int CompareTo(object? obj) => obj switch {
            null => 1,
            Unsigned<T> o => CompareTo(o),
            _ => throw new ArgumentException(null, nameof(obj)),
        };
        public bool Equals(Unsigned<T> other) => _bits.Equals(other._bits);
        public override bool Equals(object? obj) => obj is Unsigned<T> u && Equals(u);
        public override int GetHashCode() => _bits.GetHashCode();
        public int GetByteCount() => _bits.GetByteCount();

        public int GetShortestBitLength() => BitsTypeTraits<T>.IsSigned ?
            UnsignedViaSigned.GetShortestBitLength(_bits) :
            _bits.GetShortestBitLength();

        public string ToString(string? format, IFormatProvider? p) => ToBig(this).ToString(format, p);
        public override string ToString() => ToString(null, null);
        public bool TryFormat(Span<char> d, out int w, ReadOnlySpan<char> f, IFormatProvider? p)
            => ToBig(this).TryFormat(d, out w, f, p);

        public bool TryWriteBigEndian(Span<byte> d, out int w) => _bits.TryWriteBigEndian(d, out w);

        public bool TryWriteLittleEndian(Span<byte> d, out int w) => _bits.TryWriteLittleEndian(d, out w);

        // ==================================================================
        // OPERATORS
        // ==================================================================

        private static OverflowException ThrowOverflowException() {
            throw new OverflowException();
        }

        // ---- unary + -----------------------------------------------------
        public static Unsigned<T> operator +(Unsigned<T> value) => value;
        //public static Unsigned<T> operator checked +(Unsigned<T> value) => value;

        // ---- unary - -----------------------------------------------------
        public static Unsigned<T> operator -(Unsigned<T> value)
            => Wrap(unchecked(-value._bits));
        public static Unsigned<T> operator checked -(Unsigned<T> value)
            => T.IsZero(value._bits) ? Zero : throw ThrowOverflowException();

        // ---- ++ / -- -----------------------------------------------------
        public static Unsigned<T> operator ++(Unsigned<T> value) => value + One;
        public static Unsigned<T> operator checked ++(Unsigned<T> value) => checked(value + One);

        public static Unsigned<T> operator --(Unsigned<T> value) => value - One;
        public static Unsigned<T> operator checked --(Unsigned<T> value) => checked(value - One);

        // ---- binary + ----------------------------------------------------
        public static Unsigned<T> operator +(Unsigned<T> left, Unsigned<T> right)
            => Wrap(unchecked(left._bits + right._bits));
        public static Unsigned<T> operator checked +(Unsigned<T> left, Unsigned<T> right) {
            var result = Wrap(unchecked(left._bits + right._bits));
            if (result < left) {
                throw ThrowOverflowException();
            }

            return result;
        }

        // ---- binary - ----------------------------------------------------
        public static Unsigned<T> operator -(Unsigned<T> left, Unsigned<T> right)
            => Wrap(unchecked(left._bits - right._bits));
        public static Unsigned<T> operator checked -(Unsigned<T> left, Unsigned<T> right) {
            if (left < right) {
                throw ThrowOverflowException();
            }

            return Wrap(unchecked(left._bits - right._bits));
        }

        // ---- binary * ----------------------------------------------------
        public static Unsigned<T> operator *(Unsigned<T> left, Unsigned<T> right)
            => Wrap(unchecked(left._bits * right._bits));

        public static Unsigned<T> operator checked *(Unsigned<T> left, Unsigned<T> right) {
            if (T.IsZero(left._bits) || T.IsZero(right._bits)) {
                return Zero;
            }

            var result = Wrap(unchecked(left._bits * right._bits));
            var q = DivRem(result, left).Quotient;
            if (q != right) {
                throw ThrowOverflowException();
            }
            return result;
        }

        // ---- binary / ----------------------------------------------------
        public static Unsigned<T> operator /(Unsigned<T> left, Unsigned<T> right) {
            return DivRem(left, right).Quotient;
        }
        /*
        public static Unsigned<T> operator checked /(Unsigned<T> left, Unsigned<T> right) {
            return DivRem(left, right).Quotient;
        }
        */

        // ---- binary % ----------------------------------------------------
        public static Unsigned<T> operator %(Unsigned<T> left, Unsigned<T> right) {
            return DivRem(left, right).Remainder;
        }
        /*
        public static Unsigned<T> operator checked %(Unsigned<T> left, Unsigned<T> right) {
            return DivRem(left, right).Remainder;
        }*/

        // ---- bitwise -----------------------------------------------------
        public static Unsigned<T> operator ~(Unsigned<T> value) => Wrap(~value._bits);
        public static Unsigned<T> operator &(Unsigned<T> left, Unsigned<T> right) => Wrap(left._bits & right._bits);
        public static Unsigned<T> operator |(Unsigned<T> left, Unsigned<T> right) => Wrap(left._bits | right._bits);
        public static Unsigned<T> operator ^(Unsigned<T> left, Unsigned<T> right) => Wrap(left._bits ^ right._bits);

        // ---- shifts ------------------------------------------------------
        public static Unsigned<T> operator <<(Unsigned<T> value, int shiftAmount)
            => Wrap(value._bits << shiftAmount);
        public static Unsigned<T> operator >>(Unsigned<T> value, int shiftAmount)
            => Wrap(value._bits >>> shiftAmount);
        public static Unsigned<T> operator >>>(Unsigned<T> value, int shiftAmount)
            => Wrap(value._bits >>> shiftAmount);

        // ---- comparisons -------------------------------------------------
        public static bool operator ==(Unsigned<T> l, Unsigned<T> r) => l._bits == r._bits;
        public static bool operator !=(Unsigned<T> l, Unsigned<T> r) => l._bits != r._bits;
        public static bool operator <(Unsigned<T> l, Unsigned<T> r) =>
            BitsTypeTraits<T>.IsSigned ? (l._bits ^ T.AllBitsSet) < (r._bits ^ T.AllBitsSet) : l._bits < r._bits;
        public static bool operator >(Unsigned<T> l, Unsigned<T> r) =>

            BitsTypeTraits<T>.IsSigned ? (l._bits ^ T.AllBitsSet) > (r._bits ^ T.AllBitsSet) : l._bits > r._bits;
        public static bool operator <=(Unsigned<T> l, Unsigned<T> r) =>

            BitsTypeTraits<T>.IsSigned ? (l._bits ^ T.AllBitsSet) <= (r._bits ^ T.AllBitsSet) : l._bits <= r._bits;
        public static bool operator >=(Unsigned<T> l, Unsigned<T> r) =>

            BitsTypeTraits<T>.IsSigned ? (l._bits ^ T.AllBitsSet) >= (r._bits ^ T.AllBitsSet) : l._bits >= r._bits;


        public static explicit operator /*unchecked*/ Unsigned<T>(T value) {
            return Wrap(value);
        }
        public static explicit operator checked Unsigned<T>(T value) {
            if (BitsTypeTraits<T>.IsSigned) {
                if (value < T.Zero) {
                    throw ThrowOverflowException();
                }
            }
            return Wrap(value);
        }

        public static explicit operator /*unchecked*/ T(Unsigned<T> value) {
            return value._bits;
        }

        public static explicit operator checked T(Unsigned<T> value) {
            if (BitsTypeTraits<T>.IsSigned) {
                if (value._bits < T.Zero) {
                    throw ThrowOverflowException();
                }
            }
            return value._bits;
        }
    }
}