using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    public static partial class IBitwiseOperatorsDerivedTags {

        public readonly struct Other : ITag {
        }

        public readonly struct Result : ITag {
        }
    }
    
    public interface IBitwiseOperatorsDerived<TSelf, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IInterfaceDerivedSelfBase<TSelf, TBase1, TBase2>
        , IInterfaceDerivedBase<TSelf, IBitwiseOperatorsDerivedTags.Other, TOther,  TOther1, TOther2>
        , IInterfaceDerivedBase<TSelf, IBitwiseOperatorsDerivedTags.Result, TResult, TResult1, TResult2>
        where TSelf : IBitwiseOperatorsDerived<TSelf, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>?
        where TBase1 : IBitwiseOperators<TBase1, TOther1, TResult1>?
        where TBase2 : IBitwiseOperators<TBase2, TOther2, TResult2>? {
        
        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ~(TSelf value) {
            return TSelf.FromBase(~TSelf.ToBase1(value)!, ~TSelf.ToBase2(value)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator &(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! & TSelf.ToBase1(right)!, TSelf.ToBase2(left)! & TSelf.ToBase2(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator |(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! | TSelf.ToBase1(right)!, TSelf.ToBase2(left)! | TSelf.ToBase2(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ^(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! ^ TSelf.ToBase1(right)!, TSelf.ToBase2(left)! ^ TSelf.ToBase2(right)!)!;
        }
    }
}

namespace UltimateOrb.Numerics.Extensions {

    public static partial class BinaryNumberExtensions {
    }
}

namespace UltimateOrb.Numerics.Extensions {

    partial class BinaryNumberExtensions {

        extension<TSelf>(TSelf @this)
            where TSelf : IBinaryNumber<TSelf> {

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_BitwiseAnd(TSelf,TSelf)"/>
            public static TSelf operator &(TSelf left, TSelf right) => left & right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_BitwiseOr(TSelf,TSelf)"/>
            public static TSelf operator |(TSelf left, TSelf right) => left | right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_ExclusiveOr(TSelf,TSelf)"/>
            public static TSelf operator ^(TSelf left, TSelf right) => left ^ right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_OnesComplement(TSelf)"/>
            public static TSelf operator ~(TSelf value) => ~value;
        }
    }
}

namespace UltimateOrb.Numerics {

    public readonly struct Long<TInt, TUInt>
        : IBinaryInteger<Long<TInt, TUInt>>
        , IBitwiseOperators<Long<TInt, TUInt>, Long<TInt, TUInt>, Long<TInt, TUInt>>, IBitwiseOperatorsDerived<Long<TInt, TUInt>, TUInt, TInt, Long<TInt, TUInt>, TUInt, TInt, Long<TInt, TUInt>, TUInt, TInt>
        where TInt: IBitwiseOperators<TInt, TInt, TInt>
        where TUInt : IBitwiseOperators<TUInt, TUInt, TUInt> {
        readonly TUInt lo;
        readonly TInt hi;

        [return: NotNullIfNotNull(nameof(value1)), NotNullIfNotNull(nameof(value2))]
        public static Long<TInt, TUInt> FromBase(TUInt? value1, TInt? value2) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.FromBase(value1, value2);
        }

        [return: NotNullIfNotNull(nameof(value))]
        public static TUInt? ToBase1(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase1(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        public static TInt? ToBase2(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase2(value);
        }

        public static Long<TInt, TUInt> One => throw new NotImplementedException();

        public static int Radix => throw new NotImplementedException();

        public static Long<TInt, TUInt> Zero => throw new NotImplementedException();

        public static Long<TInt, TUInt> AdditiveIdentity => throw new NotImplementedException();

        public static Long<TInt, TUInt> MultiplicativeIdentity => throw new NotImplementedException();

        public static Long<TInt, TUInt> Abs(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsFinite(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNaN(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegative(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNormal(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositive(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPow2(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsZero(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Log2(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MaxMagnitude(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MaxMagnitudeNumber(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MinMagnitude(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MinMagnitudeNumber(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> PopCount(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> TrailingZeroCount(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToChecked<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public int CompareTo(object? obj) {
            throw new NotImplementedException();
        }

        public int CompareTo(Long<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public bool Equals(Long<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public int GetByteCount() {
            throw new NotImplementedException();
        }

        public int GetShortestBitLength() {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator +(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator +(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator -(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator -(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator ++(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator --(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator *(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator /(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator %(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator <<(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator >>(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static bool operator ==(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator !=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator >>>(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }
    }

    public readonly struct ULong<TInt, TUInt> {

    }
}

namespace UltimateOrb.Mathematics.Exact {


    public readonly struct LongRational<TInt, TUInt> :
        IComparable<LongRational<TInt, TUInt>>,
        IEquatable<LongRational<TInt, TUInt>>,
        // ISpanFormattable,
        IMinMaxValue<LongRational<TInt, TUInt>>,
        // IUtf8SpanFormattable,
        // IParsable<LongRational<TInt, TUInt>>,
        // ISpanParsable<LongRational<TInt, TUInt>>,
        // IUtf8SpanParsable<LongRational<TInt, TUInt>>,
        IAdditionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IAdditiveIdentity<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IComparisonOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, bool>,
        IDecrementOperators<LongRational<TInt, TUInt>>,
        IDivisionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IEqualityOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, bool>,
        // IExponentialFunctions<LongRational<TInt, TUInt>>,
        // IFloatingPoint<LongRational<TInt, TUInt>>,
        IIncrementOperators<LongRational<TInt, TUInt>>,
        IModulusOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IMultiplicativeIdentity<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IMultiplyOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        // INumber<LongRational<TInt, TUInt>>,
        INumberBase<LongRational<TInt, TUInt>>,
        // IPowerFunctions<LongRational<TInt, TUInt>>,
        // IRootFunctions<LongRational<TInt, TUInt>>,
        ISignedNumber<LongRational<TInt, TUInt>>,
        ISubtractionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IUnaryNegationOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IUnaryPlusOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>> {
        public static LongRational<TInt, TUInt> MaxValue => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> MinValue => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> AdditiveIdentity => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> MultiplicativeIdentity => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> One => throw new NotImplementedException();

        public static int Radix => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> Zero => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> NegativeOne => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> Abs(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsFinite(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNaN(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegative(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNormal(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositive(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsZero(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MaxMagnitude(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MaxMagnitudeNumber(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MinMagnitude(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MinMagnitudeNumber(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToChecked<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public int CompareTo(LongRational<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public bool Equals(LongRational<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator +(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator +(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator -(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator -(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator ++(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator --(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator *(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator /(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator %(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator ==(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator !=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }
    }
}
