using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP")]
    public partial class FloatingPointIeee754InterchageTypeTraits<T>
        where T : unmanaged, IFloatingPointIeee754<T> {

        public static int StorageBitWidth => checked(8 * Unsafe.SizeOf<T>());

        static InvalidOperationException ThrowInvalidOperationException_Radix() {
            throw new InvalidOperationException($"The radix of '{typeof(T).FullName}' must be 2 or 10.");
        }

        public static int Precision { get; } = T.Radix == 2 ?
            StorageBitWidth - (int)Math.Round(4 * Math.Log2(StorageBitWidth), MidpointRounding.ToEven) + 13 : T.Radix == 10 ?
            9 * StorageBitWidth / 32 - 2 :
            throw ThrowInvalidOperationException_Radix();

        public static int MaxExponent { get; } = T.Radix == 2 ?
            NumberBaseExtensions.Pow(2, uint.CreateChecked(StorageBitWidth - Precision - 1)) - 1 : T.Radix == 10 ?
            3 * NumberBaseExtensions.Pow(2, uint.CreateChecked(StorageBitWidth / 16 + 3)) :
            throw ThrowInvalidOperationException_Radix();

        public static int ExponentOrCombinationFieldBitWidth { get; } = T.Radix == 2 ?
            (int)Math.Round(4 * Math.Log2(StorageBitWidth), MidpointRounding.ToEven) - 13 : T.Radix == 10 ?
            StorageBitWidth / 16  + 9:
            throw ThrowInvalidOperationException_Radix();

        public static int TrailingSignificandFieldBitWidth  { get; } = T.Radix == 2 ?
            StorageBitWidth - ExponentOrCombinationFieldBitWidth - 1 : T.Radix == 10 ?
            15 * StorageBitWidth / 16 - 10 :
            throw ThrowInvalidOperationException_Radix();
    }

    //public interface IDecimalFloatingPointIeee754DerivedIeee754Interchage<TSelf,
    //    TBitsInt, TBitsUInt, TExponentInt, TExponentUInt, TBitsShort, TBitsUShort>
    //    : IDecimalFloatingPointIeee754<TSelf>
    //    where TSelf : unmanaged, IDecimalFloatingPointIeee754DerivedIeee754Interchage<TSelf,
    //        TBitsInt, TBitsUInt, TExponentInt, TExponentUInt, TBitsShort, TBitsUShort>
    //    where TBitsInt : unmanaged, IBinaryInteger<TBitsInt>, ISignedNumber<TBitsInt>
    //    where TBitsUInt : unmanaged, IBinaryInteger<TBitsUInt>, IUnsignedNumber<TBitsUInt>
    //    where TExponentInt : unmanaged, IBinaryInteger<TExponentInt>, ISignedNumber<TExponentInt>
    //    where TExponentUInt : unmanaged, IBinaryInteger<TExponentUInt>, IUnsignedNumber<TExponentUInt>
    //    where TBitsShort : unmanaged, IBinaryInteger<TBitsShort>, ISignedNumber<TBitsShort>
    //    where TBitsUShort : unmanaged, IBinaryInteger<TBitsUShort>, IUnsignedNumber<TBitsUShort> {

    //    protected static virtual TSelf FromBits(TBitsInt bits) => Unsafe.BitCast<TBitsInt, TSelf>(bits);

    //    protected static virtual int StorageBitWidth => FloatingPointIeee754InterchageTypeTraits<TSelf>.StorageBitWidth;

    //    protected static virtual int Precision => FloatingPointIeee754InterchageTypeTraits<TSelf>.Precision;

    //    static TSelf IFloatingPointIeee754<TSelf>.Epsilon => TSelf.FromBits(TBitsInt.One);

    //    static TSelf IFloatingPointIeee754<TSelf>.NaN => throw new NotImplementedException();

    //    static TSelf IFloatingPointIeee754<TSelf>.NegativeInfinity => throw new NotImplementedException();

    //    static TSelf IFloatingPointIeee754<TSelf>.NegativeZero => -Zero;

    //    static TSelf IFloatingPointIeee754<TSelf>.PositiveInfinity => throw new NotImplementedException();

    //    static TSelf ISignedNumber<TSelf>.NegativeOne => -INumberBaseFriend < TSelf >.One;

    //    static TSelf IFloatingPointConstants<TSelf>.E => throw new NotImplementedException();

    //    static TSelf IFloatingPointConstants<TSelf>.Pi => throw new NotImplementedException();

    //    static TSelf IFloatingPointConstants<TSelf>.Tau => throw new NotImplementedException();

    //    static TSelf INumberBase<TSelf>.One => throw new NotImplementedException();

    //    static TSelf INumberBase<TSelf>.Zero => throw new NotImplementedException();

    //    static TSelf IAdditiveIdentity<TSelf, TSelf>.AdditiveIdentity => throw new NotImplementedException();

    //    static TSelf IMultiplicativeIdentity<TSelf, TSelf>.MultiplicativeIdentity => INumberBaseFriend<TSelf>.One;

    //    static TSelf INumberBase<TSelf>.Abs(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Acos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Acosh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AcosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Asin(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Asinh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AsinPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Atan(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Atan2(TSelf y, TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Atan2Pi(TSelf y, TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Atanh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AtanPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.BitDecrement(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.BitIncrement(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Cbrt(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Cos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Cosh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.CosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp10(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp2(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Hypot(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Ieee754Remainder(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static int IFloatingPointIeee754<TSelf>.ILogB(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsCanonical(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsComplexNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsEvenInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsFinite(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsImaginaryNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNaN(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNegative(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNegativeInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNormal(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsOddInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsPositive(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsPositiveInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsRealNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsSubnormal(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsZero(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x, TSelf newBase) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log10(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log2(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MaxMagnitude(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MaxMagnitudeNumber(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MinMagnitude(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MinMagnitudeNumber(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.Parse(string s, NumberStyles style, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ISpanParsable<TSelf>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IParsable<TSelf>.Parse(string s, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IPowerFunctions<TSelf>.Pow(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.RootN(TSelf x, int n) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPoint<TSelf>.Round(TSelf x, int digits, MidpointRounding mode) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.ScaleB(TSelf x, int n) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Sin(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static (TSelf Sin, TSelf Cos) ITrigonometricFunctions<TSelf>.SinCos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static (TSelf SinPi, TSelf CosPi) ITrigonometricFunctions<TSelf>.SinCosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Sinh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.SinPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Sqrt(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Tan(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Tanh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.TanPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromChecked<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromSaturating<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromTruncating<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToChecked<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToSaturating<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToTruncating<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool ISpanParsable<TSelf>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IParsable<TSelf>.TryParse(string? s, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    int IComparable.CompareTo(object? obj) {
    //        throw new NotImplementedException();
    //    }

    //    int IComparable<TSelf>.CompareTo(TSelf? other) {
    //        throw new NotImplementedException();
    //    }

    //    bool IEquatable<TSelf>.Equals(TSelf? other) {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetExponentByteCount() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetExponentShortestBitLength() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetSignificandBitLength() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetSignificandByteCount() {
    //        throw new NotImplementedException();
    //    }

    //    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) {
    //        throw new NotImplementedException();
    //    }

    //    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IUnaryPlusOperators<TSelf, TSelf>.operator +(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator +(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator -(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator -(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IIncrementOperators<TSelf>.operator ++(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IDecrementOperators<TSelf>.operator --(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator *(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IDivisionOperators<TSelf, TSelf, TSelf>.operator /(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IModulusOperators<TSelf, TSelf, TSelf>.operator %(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IEqualityOperators<TSelf, TSelf, bool>.operator ==(TSelf? left, TSelf? right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IEqualityOperators<TSelf, TSelf, bool>.operator !=(TSelf? left, TSelf? right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator <(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator >(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator <=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator >=(TSelf left, TSelf right) {
    //        return left >= right;
    //    }

    //    public static virtual bool operator <=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    public static virtual bool operator >=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }
    //}

    /*
    [Experimental("UoWIP")]
    internal readonly struct Decimal192Bid {

    }
    */
}
