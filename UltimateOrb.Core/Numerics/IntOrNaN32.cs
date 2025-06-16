using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;
using UltimateOrb.Utilities.InterfaceReadOnlyExtensions.System;

namespace UltimateOrb.Numerics {

#if false
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct IntOrNaN32 :
        IComparable,
        IConvertible,
        ISpanFormattable,
        IComparable<IntOrNaN32>,
        IEquatable<IntOrNaN32>,
        IBinaryInteger<IntOrNaN32>,
        IMinMaxValue<IntOrNaN32>,
        ISignedNumber<IntOrNaN32>,
        IUtf8SpanFormattable {

        readonly int m_value;

        const int NaNBits = int.MinValue;

        internal IntOrNaN32(int value, NoCheck ignored) {
            ignored.Ignore();
            m_value = value;
        }

        public static IntOrNaN32 BigMul(IntOrNaN32 first, IntOrNaN32 second, out IntOrNaN32 highResult) {
            return IsNaN(first) || IsNaN(second) ? GetNaN(out highResult) : Math.BigMul(first.m_value, second.m_value, out highResult);
        }


        public static IntOrNaN32 NaN {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new IntOrNaN32(NaNBits, default(NoCheck));
        }

        static IntOrNaN32 GetNaN(out IntOrNaN32 highResult) {
            throw new NotImplementedException();
        }

        public int CompareTo(object? value) {
            if (value == null) {
                return 1;
            }

            return value is IntOrNaN32 i ? CompareTo(i) : throw new ArgumentException(SR.Arg_MustBeInt32OrNaN);
        }

        public int CompareTo(IntOrNaN32 value) {
            // NaN.CompareTo(n) < 0
            return m_value.CompareTo(value.m_value);
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is not IntOrNaN32 i ? false : m_value == i.m_value;
        }

        public bool Equals(IntOrNaN32 obj) {
            return m_value == obj.m_value;
        }

        // The absolute value of the int contained.
        public override int GetHashCode() {
            return m_value;
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return ToString(format, null);
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? double.NaN.ToString(format, provider) : v.ToString(format, provider);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            var v = m_value;
            return NaNBits == v ? double.NaN.TryFormat(destination, out charsWritten, format, provider) : v.TryFormat(destination, out charsWritten, format, provider);
        }

        /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            var v = m_value;
            return NaNBits == v ? double.NaN.TryFormat(utf8Destination, out bytesWritten, format, provider) : v.TryFormat(utf8Destination, out bytesWritten, format, provider);
        }

        public static IntOrNaN32 Parse(string s) => Parse(s, NumberStyles.Integer, provider: null);

        public static IntOrNaN32 Parse(string s, NumberStyles style) => Parse(s, style, provider: null);

        public static IntOrNaN32 Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Integer, provider);

        public static IntOrNaN32 Parse(string s, NumberStyles style, IFormatProvider? provider) {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s.AsSpan(), style, provider);
        }

        public static IntOrNaN32 Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null) {
            if (int.TryParse(s, provider, out var v)) {
                if (v != NaNBits) {
                    return new IntOrNaN32(v, default(NoCheck));
                }
                throw new OverflowException(OverflowMessage);
            } else {
                var n = double.Parse(s, provider);
                if (double.IsNaN(n)) {
                    return NaN;
                }
            }
            throw new FormatException(SR.Format("The input string '{0}' was not in a correct format.", s.ToString()));
        }

        static string OverflowMessage {

            get => "Value was either too large or too small for an IntOrNaN32.";
        }

        public static IntOrNaN32 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider = null) {
            if (int.TryParse(s, MaskNumberStylesForIntegerFormatting(style), provider, out var v)) {
                if (v != NaNBits) {
                    return new IntOrNaN32(v, default(NoCheck));
                }
                throw new OverflowException(OverflowMessage);
            } else {
                var n = double.Parse(s, MaskNumberStylesForFloatingPointFormatting(style), provider);
                if (double.IsNaN(n)) {
                    return NaN;
                }
            }
            throw new FormatException(SR.Format("The input string '{0}' was not in a correct format.", s.ToString()));
        }

        public static bool TryParse([NotNullWhen(true)] string? s, out IntOrNaN32 result) {
            if (s is null) {
                result = default;
                return false;
            }
            return TryParse(s.AsSpan(), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, out IntOrNaN32 result) {
            if (int.TryParse(s, out var v)) {
                if (v != NaNBits) {
                    result = new IntOrNaN32(v, default(NoCheck));
                    return true;
                }
            } else if (double.TryParse(s, out var n)) {
                if (double.IsNaN(n)) {
                    result = NaN;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryParse(ReadOnlySpan<byte> utf8Text, out IntOrNaN32 result) {
            if (int.TryParse(utf8Text, NumberStyles.Integer, null, out var v)) {
                if (v != NaNBits) {
                    result = new IntOrNaN32(v, default(NoCheck));
                    return true;
                }
            } else if (double.TryParse(utf8Text, out var n)) {
                if (double.IsNaN(n)) {
                    result = NaN;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out IntOrNaN32 result) {
            if (s is null) {
                result = default;
                return false;
            }
            return TryParse(s.AsSpan(), style, provider, out result);
        }

        const NumberStyles InvalidNumberStyles = ~(
            NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite |
            NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign |
            NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowThousands | NumberStyles.AllowExponent |
            NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier |
            NumberStyles.AllowBinarySpecifier);

        private const NumberStyles NumberStylesMask1 = ~(
            NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign |
            NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint |
            NumberStyles.AllowThousands | NumberStyles.AllowExponent |
            NumberStyles.AllowCurrencySymbol);

        private const NumberStyles NumberStylesMask2 = ~(
            NumberStyles.AllowHexSpecifier | NumberStyles.AllowBinarySpecifier);

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out IntOrNaN32 result) {
            if (int.TryParse(s, MaskNumberStylesForIntegerFormatting(style), provider, out var v)) {
                if (v != NaNBits) {
                    result = new IntOrNaN32(v, default(NoCheck));
                    return true;
                }
            } else if (double.TryParse(s, MaskNumberStylesForFloatingPointFormatting(style), provider, out var n)) {
                if (double.IsNaN(n)) {
                    result = NaN;
                    return true;
                }
            }
            result = default;
            return false;
        }

        private static NumberStyles MaskNumberStylesForFloatingPointFormatting(NumberStyles style) {
            return NumberStylesMask2 & style;
        }

        private static NumberStyles MaskNumberStylesForIntegerFormatting(NumberStyles style) {
            if (0 != ((NumberStyles.AllowHexSpecifier | NumberStyles.AllowBinarySpecifier) & style)) {
                return NumberStylesMask1 & style;
            }
            return style;
        }

        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToBoolean(double.NaN, provider) : ConvertibleExtensions.ToBoolean(v, provider);
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToChar(double.NaN, provider) : ConvertibleExtensions.ToChar(v, provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToSByte(double.NaN, provider) : ConvertibleExtensions.ToSByte(v, provider);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToByte(double.NaN, provider) : ConvertibleExtensions.ToByte(v, provider);
        }

        short IConvertible.ToInt16(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToInt16(double.NaN, provider) : ConvertibleExtensions.ToInt16(v, provider);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToUInt16(double.NaN, provider) : ConvertibleExtensions.ToUInt16(v, provider);
        }

        int IConvertible.ToInt32(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToInt32(double.NaN, provider) : ConvertibleExtensions.ToInt32(v, provider);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToUInt32(double.NaN, provider) : ConvertibleExtensions.ToUInt32(v, provider);
        }

        long IConvertible.ToInt64(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToInt64(double.NaN, provider) : ConvertibleExtensions.ToInt64(v, provider);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToUInt64(double.NaN, provider) : ConvertibleExtensions.ToUInt64(v, provider);
        }

        float IConvertible.ToSingle(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToSingle(double.NaN, provider) : ConvertibleExtensions.ToSingle(v, provider);
        }

        double IConvertible.ToDouble(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToDouble(double.NaN, provider) : ConvertibleExtensions.ToDouble(v, provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToDecimal(double.NaN, provider) : ConvertibleExtensions.ToDecimal(v, provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToDateTime(double.NaN, provider) : ConvertibleExtensions.ToDateTime(v, provider);
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            if (type == typeof(IntOrNaN32)) {
                return this;
            }
            var v = m_value;
            return NaNBits == v ? ConvertibleExtensions.ToType(double.NaN, type, provider) : ConvertibleExtensions.ToType(v, type, provider);
        }

        public static bool IsNaN(IntOrNaN32 value) {
            return NaNBits == value.m_value;
        }


        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        static IntOrNaN32 IAdditionOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator +(IntOrNaN32 first, IntOrNaN32 second) => first + second;

        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        static IntOrNaN32 IAdditionOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator checked +(IntOrNaN32 first, IntOrNaN32 second) => checked(first + second);
        
        /// <inheritdoc cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity" />
        static IntOrNaN32 IAdditiveIdentity<IntOrNaN32, IntOrNaN32>.AdditiveIdentity => default;

        /// <inheritdoc cref="IBinaryInteger{TSelf}.DivRem(TSelf, TSelf)" />
        public static (IntOrNaN32 Quotient, IntOrNaN32 Remainder) DivRem(IntOrNaN32 first, IntOrNaN32 second) {
            return IsNaN(first) || IsNaN(second) ? GetNaNPair() : Math.DivRem(first.m_value, second.m_value);
        }

        static (IntOrNaN32, IntOrNaN32) GetNaNPair() {
            return (NaN, NaN);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.LeadingZeroCount(TSelf)" />
        public static IntOrNaN32 LeadingZeroCount(IntOrNaN32 value) => new IntOrNaN32(NaNBits == value.m_value ? NaNBits : BitOperations.LeadingZeroCount(value.m_value.ToUnsignedUnchecked()), default(NoCheck));

        /// <inheritdoc cref="IBinaryInteger{TSelf}.PopCount(TSelf)" />
        public static IntOrNaN32 PopCount(IntOrNaN32 value) => new IntOrNaN32(NaNBits == value.m_value ? NaNBits : BitOperations.PopCount(value.m_value.ToUnsignedUnchecked()), default(NoCheck));

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateLeft(TSelf, int)" />
        public static IntOrNaN32 RotateLeft(IntOrNaN32 value, int rotateAmount) => new IntOrNaN32(NaNBits == value.m_value ? NaNBits : BitOperations.RotateLeft(value.m_value.ToUnsignedUnchecked(), rotateAmount).ToSignedUnchecked(), default(NoCheck));

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateRight(TSelf, int)" />
        public static IntOrNaN32 RotateRight(IntOrNaN32 value, int rotateAmount) => new IntOrNaN32(NaNBits == value.m_value ? NaNBits : BitOperations.RotateRight(value.m_value.ToUnsignedUnchecked(), rotateAmount).ToSignedUnchecked(), default(NoCheck));

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TrailingZeroCount(TSelf)" />
        public static IntOrNaN32 TrailingZeroCount(IntOrNaN32 value) => new IntOrNaN32(NaNBits == value.m_value ? NaNBits : BitOperations.TrailingZeroCount(value.m_value.ToUnsignedUnchecked()), default(NoCheck));

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadBigEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<IntOrNaN32>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntOrNaN32 value) {
            if (source.Length < sizeof(int)) {
                value = default;
                return false;
            }
            int v = BinaryPrimitives.ReadInt32BigEndian(source);
            value = new IntOrNaN32(v, default(NoCheck));
            return true;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadLittleEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<IntOrNaN32>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out IntOrNaN32 value) {
            if (source.Length < sizeof(int)) {
                value = default;
                return false;
            }
            int v = BinaryPrimitives.ReadInt32LittleEndian(source);
            value = new IntOrNaN32(v, default(NoCheck));
            return true;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetShortestBitLength()" />
        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetShortestBitLength()" />
        int IBinaryInteger<IntOrNaN32>.GetShortestBitLength() {
            var v = m_value;
            return NaNBits == v ? NaNBits : GetShortestBitLength(unchecked((uint)v));

            static int GetShortestBitLength(uint value) {
                // This method calculates the shortest bit length of a given unsigned integer.
                // It counts the number of bits required to represent the value in binary.
                return 32 - BitOperations.LeadingZeroCount(value);
            }
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetByteCount()" />
        int IBinaryInteger<IntOrNaN32>.GetByteCount() => sizeof(int);

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteBigEndian(Span{byte}, out int)" />
        bool IBinaryInteger<IntOrNaN32>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length < sizeof(int)) {
                bytesWritten = 0;
                return false;
            }
            BinaryPrimitives.WriteInt32BigEndian(destination, m_value);
            bytesWritten = sizeof(int);
            return true;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteLittleEndian(Span{byte}, out int)" />
        bool IBinaryInteger<IntOrNaN32>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {

        }

        //
        // IBinaryNumber
        //

        /// <inheritdoc cref="IBinaryNumber{TSelf}.AllBitsSet" />
        static IntOrNaN32 IBinaryNumber<IntOrNaN32>.AllBitsSet => new IntOrNaN32(-1, default(NoCheck));

        /// <inheritdoc cref="IBinaryNumber{TSelf}.IsPow2(TSelf)" />
        public static bool IsPow2(IntOrNaN32 value) => IsNaN(value) ? false : BitOperations.IsPow2(value.m_value);

        /// <inheritdoc cref="IBinaryNumber{TSelf}.Log2(TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntOrNaN32 Log2(IntOrNaN32 value) {
            Debug.Assert(NaNBits < 0);
            return new IntOrNaN32(value.m_value < 0 ? NaNBits : BitOperations.Log2(value.m_value.ToUnsignedUnchecked()), default(NoCheck));
        }

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_BitwiseAnd(TSelf, TOther)" />
        static IntOrNaN32 IBitwiseOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator &(IntOrNaN32 first, IntOrNaN32 second) => first & second;

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_BitwiseOr(TSelf, TOther)" />
        static IntOrNaN32 IBitwiseOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator |(IntOrNaN32 first, IntOrNaN32 second) => first | second;

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_ExclusiveOr(TSelf, TOther)" />
        static IntOrNaN32 IBitwiseOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator ^(IntOrNaN32 first, IntOrNaN32 second) => first ^ second;

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_OnesComplement(TSelf)" />
        static IntOrNaN32 IBitwiseOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator ~(IntOrNaN32 value) => ~value;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThan(TSelf, TOther)" />
        static bool IComparisonOperators<IntOrNaN32, IntOrNaN32, bool>.operator <(IntOrNaN32 first, IntOrNaN32 second) => first < second;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThanOrEqual(TSelf, TOther)" />
        static bool IComparisonOperators<IntOrNaN32, IntOrNaN32, bool>.operator <=(IntOrNaN32 first, IntOrNaN32 second) => first <= second;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThan(TSelf, TOther)" />
        static bool IComparisonOperators<IntOrNaN32, IntOrNaN32, bool>.operator >(IntOrNaN32 first, IntOrNaN32 second) => first > second;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThanOrEqual(TSelf, TOther)" />
        static bool IComparisonOperators<IntOrNaN32, IntOrNaN32, bool>.operator >=(IntOrNaN32 first, IntOrNaN32 second) => first >= second;

        /// <inheritdoc cref="IDecrementOperators{TSelf}.op_Decrement(TSelf)" />
        static IntOrNaN32 IDecrementOperators<IntOrNaN32>.operator --(IntOrNaN32 value) => --value;

        /// <inheritdoc cref="IDecrementOperators{TSelf}.op_Decrement(TSelf)" />
        static IntOrNaN32 IDecrementOperators<IntOrNaN32>.operator checked --(IntOrNaN32 value) => checked(--value);

        //
        // IDivisionOperators
        //

        /// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.op_Division(TSelf, TOther)" />
        static IntOrNaN32 IDivisionOperators<IntOrNaN32, IntOrNaN32, IntOrNaN32>.operator /(int first, int second) => first / second;

        //
        // IEqualityOperators
        //

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Equality(TSelf, TOther)" />
        static bool IEqualityOperators<int, int, bool>.operator ==(int first, int second) => first == second;

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Inequality(TSelf, TOther)" />
        static bool IEqualityOperators<int, int, bool>.operator !=(int first, int second) => first != second;

        //
        // IIncrementOperators
        //

        /// <inheritdoc cref="IIncrementOperators{TSelf}.op_Increment(TSelf)" />
        static int IIncrementOperators<int>.operator ++(int value) => ++value;

        /// <inheritdoc cref="IIncrementOperators{TSelf}.op_CheckedIncrement(TSelf)" />
        static int IIncrementOperators<int>.operator checked ++(int value) => checked(++value);

        //
        // IMinMaxValue
        //

        /// <inheritdoc cref="IMinMaxValue{TSelf}.MinValue" />
        static int IMinMaxValue<int>.MinValue => MinValue;

        /// <inheritdoc cref="IMinMaxValue{TSelf}.MaxValue" />
        static int IMinMaxValue<int>.MaxValue => MaxValue;

        //
        // IModulusOperators
        //

        /// <inheritdoc cref="IModulusOperators{TSelf, TOther, TResult}.op_Modulus(TSelf, TOther)" />
        static int IModulusOperators<int, int, int>.operator %(int first, int second) => first % second;

        //
        // IMultiplicativeIdentity
        //

        /// <inheritdoc cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity" />
        static int IMultiplicativeIdentity<int, int>.MultiplicativeIdentity => MultiplicativeIdentity;

        //
        // IMultiplyOperators
        //

        /// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.op_Multiply(TSelf, TOther)" />
        static int IMultiplyOperators<int, int, int>.operator *(int first, int second) => first * second;

        /// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.op_CheckedMultiply(TSelf, TOther)" />
        static int IMultiplyOperators<int, int, int>.operator checked *(int first, int second) => checked(first * second);

        //
        // INumber
        //

        /// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)" />
        public static IntOrNaN32 Clamp(IntOrNaN32 value, IntOrNaN32 min, IntOrNaN32 max) => Math.Clamp(value, min, max);

        /// <inheritdoc cref="INumber{TSelf}.CopySign(TSelf, TSelf)" />
        public static IntOrNaN32 CopySign(IntOrNaN32 value, IntOrNaN32 sign) {
            int absValue = value;

            if (absValue < 0) {
                absValue = -absValue;
            }

            if (sign >= 0) {
                if (absValue < 0) {
                    Math.ThrowNegateTwosCompOverflow();
                }

                return absValue;
            }

            return -absValue;
        }

        /// <inheritdoc cref="INumber{TSelf}.Max(TSelf, TSelf)" />
        public static IntOrNaN32 Max(IntOrNaN32 x, IntOrNaN32 y) => Math.Max(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MaxNumber(TSelf, TSelf)" />
        static IntOrNaN32 INumber<IntOrNaN32>.MaxNumber(IntOrNaN32 x, IntOrNaN32 y) => Max(x, y);

        /// <inheritdoc cref="INumber{TSelf}.Min(TSelf, TSelf)" />
        public static IntOrNaN32 Min(IntOrNaN32 x, IntOrNaN32 y) => Math.Min(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MinNumber(TSelf, TSelf)" />
        static IntOrNaN32 INumber<IntOrNaN32>.MinNumber(IntOrNaN32 x, IntOrNaN32 y) => Min(x, y);

        /// <inheritdoc cref="INumber{TSelf}.Sign(TSelf)" />
        public static int Sign(IntOrNaN32 value) => IsNaN(value) ? Math.Sign(double.NaN) : Math.Sign(value.m_value);

        //
        // INumberBase
        //

        /// <inheritdoc cref="INumberBase{TSelf}.One" />
        static IntOrNaN32 INumberBase<IntOrNaN32>.One => One;

        /// <inheritdoc cref="INumberBase{TSelf}.Radix" />
        static int INumberBase<IntOrNaN32>.Radix => 2;

        /// <inheritdoc cref="INumberBase{TSelf}.Zero" />
        static IntOrNaN32 INumberBase<IntOrNaN32>.Zero => Zero;

        /// <inheritdoc cref="INumberBase{TSelf}.Abs(TSelf)" />
        public static IntOrNaN32 Abs(IntOrNaN32 value) => Math.Abs(value);

        /// <inheritdoc cref="INumberBase{TSelf}.CreateChecked{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            int result;

            if (typeof(TOther) == typeof(int)) {
                result = (int)(object)value;
            } else if (!TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateSaturating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            int result;

            if (typeof(TOther) == typeof(int)) {
                result = (int)(object)value;
            } else if (!TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateTruncating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            int result;

            if (typeof(TOther) == typeof(int)) {
                result = (int)(object)value;
            } else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsCanonical(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsCanonical(IntOrNaN32 value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsComplexNumber(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsComplexNumber(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsEvenInteger(TSelf)" />
        public static bool IsEvenInteger(IntOrNaN32 value) => NaNBits != value.m_value && 0 == (1 & value.m_value);

        /// <inheritdoc cref="INumberBase{TSelf}.IsFinite(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsFinite(IntOrNaN32 value) => NaNBits != value.m_value;

        /// <inheritdoc cref="INumberBase{TSelf}.IsImaginaryNumber(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsImaginaryNumber(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInfinity(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsInfinity(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInteger(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsInteger(IntOrNaN32 value) => NaNBits != value.m_value;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNaN(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsNaN(IntOrNaN32 value) => NaNBits == value.m_value;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegative(TSelf)" />
        public static bool IsNegative(IntOrNaN32 value) => value.m_value < 0;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegativeInfinity(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsNegativeInfinity(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNormal(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsNormal(IntOrNaN32 value) => 0 != (~NaNBits & value.m_value);

        /// <inheritdoc cref="INumberBase{TSelf}.IsOddInteger(TSelf)" />
        public static bool IsOddInteger(IntOrNaN32 value) => NaNBits != value.m_value && 0 != (1 & value.m_value);

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositive(TSelf)" />
        public static bool IsPositive(IntOrNaN32 value) => value.m_value >= 0;

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositiveInfinity(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsPositiveInfinity(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsRealNumber(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsRealNumber(IntOrNaN32 value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsSubnormal(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsSubnormal(IntOrNaN32 value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsZero(TSelf)" />
        static bool INumberBase<IntOrNaN32>.IsZero(IntOrNaN32 value) => 0 == value.m_value;

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitude(TSelf, TSelf)" />
        public static IntOrNaN32 MaxMagnitude(IntOrNaN32 x, IntOrNaN32 y) {
            int absX = x;

            if (absX < 0) {
                absX = -absX;

                if (absX < 0) {
                    return x;
                }
            }

            int absY = y;

            if (absY < 0) {
                absY = -absY;

                if (absY < 0) {
                    return y;
                }
            }

            if (absX > absY) {
                return x;
            }

            if (absX == absY) {
                return IsNegative(x) ? y : x;
            }

            return y;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitudeNumber(TSelf, TSelf)" />
        static IntOrNaN32 INumberBase<IntOrNaN32>.MaxMagnitudeNumber(IntOrNaN32 x, IntOrNaN32 y) =>;

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitude(TSelf, TSelf)" />
        public static int MinMagnitude(int x, int y) {
            int absX = x;

            if (absX < 0) {
                absX = -absX;

                if (absX < 0) {
                    return y;
                }
            }

            int absY = y;

            if (absY < 0) {
                absY = -absY;

                if (absY < 0) {
                    return x;
                }
            }

            if (absX < absY) {
                return x;
            }

            if (absX == absY) {
                return IsNegative(x) ? x : y;
            }

            return y;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitudeNumber(TSelf, TSelf)" />
        static IntOrNaN32 INumberBase<IntOrNaN32>.MinMagnitudeNumber(IntOrNaN32 x, IntOrNaN32 y) => ;

        /// <inheritdoc cref="INumberBase{TSelf}.MultiplyAddEstimate(TSelf, TSelf, TSelf)" />
        static IntOrNaN32 INumberBase<IntOrNaN32>.MultiplyAddEstimate(IntOrNaN32 first, IntOrNaN32 second, IntOrNaN32 addend) => (first.m_value * second.m_value) + addend.m_value;

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertFromChecked<TOther>(TOther value, out IntOrNaN32 result) => TryConvertFromChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out IntOrNaN32 result)
            where TOther : INumberBase<TOther> {


            if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = checked((int)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = checked((int)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = checked((int)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = checked((int)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = checked((int)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = checked((int)actualValue);
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertFromSaturating<TOther>(TOther value, out IntOrNaN32 result) => TryConvertFromSaturating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out IntOrNaN32 result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `int` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = (actualValue == Half.PositiveInfinity) ? MaxValue :
                         (actualValue == Half.NegativeInfinity) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertFromTruncating<TOther>(TOther value, out IntOrNaN32 result) => TryConvertFromTruncating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out IntOrNaN32 result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `int` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = (actualValue == Half.PositiveInfinity) ? MaxValue :
                         (actualValue == Half.NegativeInfinity) ? MinValue : (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = (int)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = (actualValue >= MaxValue) ? MaxValue :
                         (actualValue <= MinValue) ? MinValue : (int)actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertToChecked<TOther>(IntOrNaN32 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `int` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(byte)) {
                byte actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult = checked((char)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualResult = value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualResult = checked((ushort)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult = checked((uint)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult = checked((ulong)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualResult = checked((UInt128)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualResult = checked((nuint)value);
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToSaturating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertToSaturating<TOther>(IntOrNaN32 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `int` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(byte)) {
                byte actualResult = (value >= byte.MaxValue) ? byte.MaxValue :
                                    (value <= byte.MinValue) ? byte.MinValue : (byte)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult = (value >= char.MaxValue) ? char.MaxValue :
                                    (value <= char.MinValue) ? char.MinValue : (char)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualResult = value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualResult = (value >= ushort.MaxValue) ? ushort.MaxValue :
                                      (value <= ushort.MinValue) ? ushort.MinValue : (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult = (value <= 0) ? uint.MinValue : (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult = (value <= 0) ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualResult = (value <= 0) ? UInt128.MinValue : (UInt128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualResult = (value <= 0) ? 0 : (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToTruncating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<IntOrNaN32>.TryConvertToTruncating<TOther>(IntOrNaN32 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `int` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(byte)) {
                byte actualResult = (byte)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult = (char)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualResult = value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualResult = (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult = (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult = (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualResult = (UInt128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualResult = (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        //
        // IParsable
        //

        /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out int result) => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        /// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_LeftShift(TSelf, TOther)" />
        static int IShiftOperators<int, int, int>.operator <<(int value, int shiftAmount) => value << shiftAmount;

        /// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_RightShift(TSelf, TOther)" />
        static int IShiftOperators<int, int, int>.operator >>(int value, int shiftAmount) => value >> shiftAmount;

        /// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_UnsignedRightShift(TSelf, TOther)" />
        static int IShiftOperators<int, int, int>.operator >>>(int value, int shiftAmount) => value >>> shiftAmount;

        static IntOrNaN32 ISignedNumber<IntOrNaN32>.NegativeOne => NegativeOne;

        /// <inheritdoc cref="ISignedNumber{TSelf}.NegativeOne" />
        public static IntOrNaN32 NegativeOne {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new IntOrNaN32(-1, default(NoCheck));
        }

        /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
        public static int Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Integer, provider);

        /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out IntOrNaN32 result) => TryParse(s, NumberStyles.Integer, provider, out result);

        /// <inheritdoc cref="ISubtractionOperators{TSelf, TOther, TResult}.op_Subtraction(TSelf, TOther)" />
        public static IntOrNaN32 operator /*unchecked */-(IntOrNaN32 first, IntOrNaN32 second) {
            if (IsNaN(first) || IsNaN(second)) {
                return NaN;
            }
            return new IntOrNaN32(!CheckedNoThrow.TrySubtract(first.m_value, second.m_value, out var v) ? NaNBits : v, default(NoCheck));
        }

        /// <inheritdoc cref="ISubtractionOperators{TSelf, TOther, TResult}.op_CheckedSubtraction(TSelf, TOther)" />
        public static IntOrNaN32 operator checked -(IntOrNaN32 first, IntOrNaN32 second) => unchecked(first - second);

        /// <inheritdoc cref="IUnaryNegationOperators{TSelf, TResult}.op_UnaryNegation(TSelf)" />
        public static IntOrNaN32 operator /*unchecked */-(IntOrNaN32 value) => new IntOrNaN32(unchecked(-value.m_value), default(NoCheck));

        /// <inheritdoc cref="IUnaryNegationOperators{TSelf, TResult}.op_CheckedUnaryNegation(TSelf)" />
        public static IntOrNaN32 operator checked -(IntOrNaN32 value) => unchecked(-value);

        /// <inheritdoc cref="IUnaryPlusOperators{TSelf, TResult}.op_UnaryPlus(TSelf)" />
        public static IntOrNaN32 operator +(IntOrNaN32 value) => value;

        /// <summary>
        /// Throws ArithmeticException on a NaN value.
        /// </summary>
        /// <param name="value">The specified value</param>
        public static void CheckFinite(IntOrNaN32 value) {
            _ = checked(-value.m_value);
        }

        /// <summary>
        /// Throws ArithmeticException on a NaN value.
        /// </summary>
        /// <param name="value">The specified value</param>
        public void CheckFinite() {
            CheckFinite(this);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.Parse(ReadOnlySpan{byte}, NumberStyles, IFormatProvider?)" />
        public static int Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null) {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.ParseBinaryInteger<byte, int>(utf8Text, style, NumberFormatInfo.GetInstance(provider));
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryParse(ReadOnlySpan{byte}, NumberStyles, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out IntOrNaN32 result) {
            NumberFormatInfo.ValidateParseStyleInteger(style);
            return Number.TryParseBinaryInteger(utf8Text, style, NumberFormatInfo.GetInstance(provider), out result) == Number.ParsingStatus.OK;
        }

        /// <inheritdoc cref="IUtf8SpanParsable{TSelf}.Parse(ReadOnlySpan{byte}, IFormatProvider?)" />
        public static int Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider) => Parse(utf8Text, NumberStyles.Integer, provider);

        /// <inheritdoc cref="IUtf8SpanParsable{TSelf}.TryParse(ReadOnlySpan{byte}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out IntOrNaN32 result) => TryParse(utf8Text, NumberStyles.Integer, provider, out result);
    }
#endif

}
