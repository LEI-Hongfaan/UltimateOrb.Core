// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace UltimateOrb {

    /// <summary>
    /// Represents a double-precision value. It's like <see cref="Double"></see> but has NaN as the default value.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [DebuggerDisplay($@"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public readonly struct NaNDefaultedDouble :
        IComparable,
        IConvertible,
        ISpanFormattable,
        IComparable<NaNDefaultedDouble>,
        IEquatable<NaNDefaultedDouble>,
        IBinaryFloatingPointIeee754<NaNDefaultedDouble>,
        IMinMaxValue<NaNDefaultedDouble> {
        private readonly Int64 bits;

        private const Int64 NaNBits = unchecked((Int64)0Xfff8000000000000);

        private NaNDefaultedDouble(Int64 bits) {
            this.bits = bits;
        }

        private Int64 DoubleBits {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NaNBits ^ bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NaNDefaultedDouble(Double value) {
            this.bits = NaNBits ^ BitConverter.DoubleToInt64Bits(value);
        }

        /// <summary>
        /// The <see cref="Double"></see> value that this <see cref="NaNDefaultedDouble"></see> instance represents.
        /// </summary>
        public Double Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => BitConverter.Int64BitsToDouble(NaNBits ^ bits);
        }

        /// <summary>
        /// Converts the specified <see cref="Double"></see> value to <see cref="NaNDefaultedDouble"></see> value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator NaNDefaultedDouble(Double value) {
            return new NaNDefaultedDouble(value);
        }

        /// <summary>
        /// Converts the specified <see cref="NaNDefaultedDouble"></see> value to <see cref="Double"></see> value.
        /// </summary>
        /// <param name="value">The specified value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Double(NaNDefaultedDouble value) {
            return value.Value;
        }

        //
        // Public Constants
        //

        /// <inheritdoc cref="Double.MinValue" />
        public static NaNDefaultedDouble MinValue {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0Xffefffffffffffff));
        }
        /// <inheritdoc cref="Double.MaxValue" />
        public static NaNDefaultedDouble MaxValue {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X7fefffffffffffff));
        }
        // Note Epsilon should be a double whose hex representation is 0x1
        // on little endian machines.
        /// <inheritdoc cref="Double.Epsilon" />
        public static NaNDefaultedDouble Epsilon {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ 1);
        }
        /// <inheritdoc cref="Double.NegativeInfinity" />
        public static NaNDefaultedDouble NegativeInfinity {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0Xfff0000000000000));
        }
        /// <inheritdoc cref="Double.PositiveInfinity" />
        public static NaNDefaultedDouble PositiveInfinity {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X7ff0000000000000));
        }

        /// <inheritdoc cref="Double.NaN" />
        public static NaNDefaultedDouble NaN {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => default;
        }

        /// <summary>Represents the additive identity (0).</summary>
        internal static NaNDefaultedDouble AdditiveIdentity {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits);
        }

        /// <summary>Represents the multiplicative identity (1).</summary>
        internal static NaNDefaultedDouble MultiplicativeIdentity {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X3ff0000000000000));
        }

        /// <summary>Represents the number one (1).</summary>
        internal static NaNDefaultedDouble One {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X3ff0000000000000));
        }

        /// <summary>Represents the number zero (0).</summary>
        internal static NaNDefaultedDouble Zero {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits);
        }

        /// <summary>Represents the number negative one (-1).</summary>
        internal static NaNDefaultedDouble NegativeOne {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0Xbff0000000000000));
        }

        /// <summary>Represents the number negative zero (-0).</summary>
        public static NaNDefaultedDouble NegativeZero {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X8000000000000000));
        }

        /// <summary>Represents the natural logarithmic base, specified by the constant, e.</summary>
        /// <remarks>Euler's number is approximately 2.7182818284590452354.</remarks>
        public static NaNDefaultedDouble E {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X4005bf0a8b145769));
        }

        /// <summary>Represents the ratio of the circumference of a circle to its diameter, specified by the constant, π.</summary>
        /// <remarks>Pi is approximately 3.1415926535897932385.</remarks>
        public static NaNDefaultedDouble Pi {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X400921fb54442d18));
        }

        /// <summary>Represents the number of radians in one turn, specified by the constant, τ.</summary>
        /// <remarks>Tau is approximately 6.2831853071795864769.</remarks>
        public static NaNDefaultedDouble Tau {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(NaNBits ^ unchecked((Int64)0X401921fb54442d18));
        }

        //
        // Constants for manipulating the private bit-representation
        //

        internal const ulong SignMask = 0x8000_0000_0000_0000;
        internal const int SignShift = 63;
        internal const byte ShiftedSignMask = (byte)(SignMask >> SignShift);

        internal const ulong BiasedExponentMask = 0x7FF0_0000_0000_0000;
        internal const int BiasedExponentShift = 52;
        internal const ushort ShiftedExponentMask = (ushort)(BiasedExponentMask >> BiasedExponentShift);

        internal const ulong TrailingSignificandMask = 0x000F_FFFF_FFFF_FFFF;

        internal const byte MinSign = 0;
        internal const byte MaxSign = 1;

        internal const ushort MinBiasedExponent = 0x0000;
        internal const ushort MaxBiasedExponent = 0x07FF;

        internal const ushort ExponentBias = 1023;

        internal const short MinExponent = -1022;
        internal const short MaxExponent = +1023;

        internal const ulong MinTrailingSignificand = 0x0000_0000_0000_0000;
        internal const ulong MaxTrailingSignificand = 0x000F_FFFF_FFFF_FFFF;

        internal const int TrailingSignificandLength = 52;
        internal const int SignificandLength = TrailingSignificandLength + 1;

        internal ushort BiasedExponent {
            get {
                var bits = unchecked((ulong)DoubleBits);
                return ExtractBiasedExponentFromBits(bits);
            }
        }

        internal short Exponent {
            get {
                return (short)(BiasedExponent - ExponentBias);
            }
        }

        internal ulong Significand {
            get {
                return TrailingSignificand | ((BiasedExponent != 0) ? (1UL << BiasedExponentShift) : 0UL);
            }
        }

        internal ulong TrailingSignificand {
            get {
                var bits = unchecked((ulong)DoubleBits);
                return ExtractTrailingSignificandFromBits(bits);
            }
        }

        internal static ushort ExtractBiasedExponentFromBits(ulong bits) {
            return (ushort)((bits >> BiasedExponentShift) & ShiftedExponentMask);
        }

        internal static ulong ExtractTrailingSignificandFromBits(ulong bits) {
            return bits & TrailingSignificandMask;
        }

        /// <summary>Determines whether the specified value is finite (zero, subnormal, or normal).</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsFinite(NaNDefaultedDouble d) {
            var bits = d.DoubleBits;
            return (bits & 0x7FFFFFFFFFFFFFFF) < 0x7FF0000000000000;
        }

        /// <summary>Determines whether the specified value is infinite.</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsInfinity(NaNDefaultedDouble d) {
            var bits = d.DoubleBits;
            return (bits & 0x7FFFFFFFFFFFFFFF) == 0x7FF0000000000000;
        }

        /// <summary>Determines whether the specified value is NaN.</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsNaN(NaNDefaultedDouble d) {
            // A NaN will never equal itself so this is an
            // easy and efficient way to check for NaN.
            var t = d.Value;
#pragma warning disable CS1718
            return t != t;
#pragma warning restore CS1718
        }

        /// <summary>Determines whether the specified value is negative.</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsNegative(NaNDefaultedDouble d) {
            return 0 <= d.bits;
        }

        /// <summary>Determines whether the specified value is negative infinity.</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNegativeInfinity(NaNDefaultedDouble d) {
            return d == NaNDefaultedDouble.NegativeInfinity;
        }

        /// <summary>Determines whether the specified value is normal.</summary>
        //[NonVersionable]
        // This is probably not worth inlining, it has branches and should be rarely called
        public static unsafe bool IsNormal(NaNDefaultedDouble d) {
            var bits = d.DoubleBits;
            bits &= 0x7FFFFFFFFFFFFFFF;
            return (bits < 0x7FF0000000000000) && (bits != 0) && ((bits & 0x7FF0000000000000) != 0);
        }

        /// <summary>Determines whether the specified value is positive infinity.</summary>
        //[NonVersionable]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveInfinity(NaNDefaultedDouble d) {
            return d == NaNDefaultedDouble.PositiveInfinity;
        }

        /// <summary>Determines whether the specified value is subnormal.</summary>
        //[NonVersionable]
        // This is probably not worth inlining, it has branches and should be rarely called
        public static unsafe bool IsSubnormal(NaNDefaultedDouble d) {
            var bits = d.DoubleBits;
            bits &= 0x7FFFFFFFFFFFFFFF;
            return (bits < 0x7FF0000000000000) && (bits != 0) && ((bits & 0x7FF0000000000000) == 0);
        }

        // Compares this object to another object, returning an instance of System.Relation.
        // Null is considered less than any instance.
        //
        // If object is not of type Double, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
        /// <inheritdoc cref="Double.CompareTo(object?)" />
        public int CompareTo(object? value) {
            if (value == null) {
                return 1;
            }
            var v = Value;
            if (value is NaNDefaultedDouble d) {
                var u = d.Value;
                if (v < u) {
                    return -1;
                }

                if (v > u) {
                    return 1;
                }

                if (v == u) {
                    return 0;
                }

                // At least one of the values is NaN.
                if (Double.IsNaN(v)) {
                    return Double.IsNaN(d) ? 0 : -1;
                } else {
                    return 1;
                }
            }
            return Compare(v, value);

            [MethodImpl(MethodImplOptions.NoInlining)]
            static int Compare(Double v, object? value) {
                return value is Double u ? u.CompareTo((NaNDefaultedDouble)v) : v.CompareTo(value);
            }
        }

        /// <inheritdoc cref="Double.CompareTo(Double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(NaNDefaultedDouble value) {
            return Value.CompareTo(value.Value);
        }

        // True if obj is another Double with the same value as the current instance.  This is
        // a method of object equality, that only returns true if obj is also a double.
        /// <inheritdoc cref="Double.Equals(object?)" />
        public override bool Equals([NotNullWhen(true)] object? obj) {
            if (obj is NaNDefaultedDouble v) {
                return Value.Equals(v.Value);
            }
            return false;
        }

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Equality(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator ==(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value == second.Value;

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Inequality(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator !=(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value != second.Value;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThan(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator <(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value < second.Value;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThan(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator >(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value > second.Value;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThanOrEqual(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator <=(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value <= second.Value;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThanOrEqual(TSelf, TOther)" />
        //[NonVersionable]
        public static bool operator >=(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value >= second.Value;

        /// <inheritdoc cref="Double.Equals(Double)" />
        public bool Equals(NaNDefaultedDouble obj) {
            return Value.Equals(obj);
        }

        // The hashcode for a double is the absolute value of the integer representation
        // of that double.
        /// <inheritdoc cref="Double.GetHashCode()" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)] // 64-bit constants make the IL unusually large that makes the inliner to reject the method
        public override int GetHashCode() {
            var bits = DoubleBits;

            // Optimized check for IsNan() || IsZero()
            if (((bits - 1) & 0x7FFFFFFFFFFFFFFF) >= 0x7FF0000000000000) {
                // Ensure that all NaNs and both zeros have the same hash code
                bits &= 0x7FF0000000000000;
            }

            return unchecked((int)bits) ^ ((int)(bits >> 32));
        }

        /// <inheritdoc cref="Double.ToString()" />
        public override string ToString() {
            return Value.ToString();
        }

        /// <inheritdoc cref="Double.ToString(string?)" />
        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return Value.ToString(format);
        }

        /// <inheritdoc cref="Double.ToString(IFormatProvider?)" />
        public string ToString(IFormatProvider? provider) {
            return Value.ToString(provider);
        }

        /// <inheritdoc cref="Double.ToString(string?, IFormatProvider?)" />
        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            return Value.ToString(format, provider);
        }

        /// <inheritdoc cref="Double.TryFormat(Span{char}, out int, ReadOnlySpan{char}, IFormatProvider?)" />
        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            return Value.TryFormat(destination, out charsWritten, format, provider);
        }

        /// <inheritdoc cref="Double.Parse(string)" />
        public static NaNDefaultedDouble Parse(string s) {
            return Double.Parse(s);
        }

        /// <inheritdoc cref="Double.Parse(string, NumberStyles)" />
        public static NaNDefaultedDouble Parse(string s, NumberStyles style) {
            return Double.Parse(s, style);
        }

        /// <inheritdoc cref="Double.Parse(string, IFormatProvider?)" />
        public static NaNDefaultedDouble Parse(string s, IFormatProvider? provider) {
            return Double.Parse(s, provider);
        }

        /// <inheritdoc cref="Double.Parse(string, NumberStyles, IFormatProvider?)" />
        public static NaNDefaultedDouble Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Double.Parse(s, style, provider);
        }

        // Parses a double from a String in the given style.  If
        // a NumberFormatInfo isn't specified, the current culture's
        // NumberFormatInfo is assumed.
        //
        // This method will not throw an OverflowException, but will return
        // PositiveInfinity or NegativeInfinity for a number that is too
        // large or too small.
        /// <inheritdoc cref="Double.Parse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?)" />
        public static NaNDefaultedDouble Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null) {
            return Double.Parse(s, style, provider);
        }

        /// <inheritdoc cref="Double.TryParse(string?, out Double)" />
        public static bool TryParse([NotNullWhen(true)] string? s, out NaNDefaultedDouble result) {
            if (s == null) {
                result = 0;
                return false;
            }

            return TryParse((ReadOnlySpan<char>)s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
        }

        /// <inheritdoc cref="Double.TryParse(ReadOnlySpan{char}, out Double)" />
        public static bool TryParse(ReadOnlySpan<char> s, out NaNDefaultedDouble result) {
            return TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.CurrentInfo, out result);
        }

        /// <inheritdoc cref="Double.TryParse(string?, NumberStyles, IFormatProvider?, out Double)" />
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out NaNDefaultedDouble result) {
            var r = Double.TryParse(s, style, provider, out var t);
            result = t;
            return r;
        }

        /// <inheritdoc cref="Double.TryParse(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out Double)" />
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out NaNDefaultedDouble result) {
            var r = Double.TryParse(s, style, provider, out var t);
            result = t;
            return r;
        }

        /*
        private static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, NumberFormatInfo info, out NaNDefaultedDouble result) {
            var r = Double.TryParse(s, style, provider, out var t);
            result = t;
            return r;
        }
        */

        //
        // IConvertible implementation
        //
        // TODO: Review
        /// <inheritdoc cref="Double.GetTypeCode()" />
        public TypeCode GetTypeCode() {
            return TypeCode.Double;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return Convert.ToBoolean(Value);
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            return ((IConvertible)Value).ToChar(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            return Convert.ToSByte(Value);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            return Convert.ToByte(Value);
        }

        short IConvertible.ToInt16(IFormatProvider? provider) {
            return Convert.ToInt16(Value);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider) {
            return Convert.ToUInt16(Value);
        }

        int IConvertible.ToInt32(IFormatProvider? provider) {
            return Convert.ToInt32(Value);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider) {
            return Convert.ToUInt32(Value);
        }

        long IConvertible.ToInt64(IFormatProvider? provider) {
            return Convert.ToInt64(Value);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider) {
            return Convert.ToUInt64(Value);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return Convert.ToSingle(Value);
        }

        Double IConvertible.ToDouble(IFormatProvider? provider) {
            return Value;
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return Convert.ToDecimal(Value);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            return ((IConvertible)Value).ToDateTime(provider);
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            if (type == typeof(NaNDefaultedDouble)) {
                return this;
            }
            return ((IConvertible)Value).ToType(type, provider);
        }

        //
        // IAdditionOperators
        //

        /// <inheritdoc cref="IAdditionOperators{TSelf, TOther, TResult}.op_Addition(TSelf, TOther)" />
        static NaNDefaultedDouble IAdditionOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator +(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value + second.Value;

        //
        // IAdditiveIdentity
        //

        /// <inheritdoc cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity" />
        static NaNDefaultedDouble IAdditiveIdentity<NaNDefaultedDouble, NaNDefaultedDouble>.AdditiveIdentity => AdditiveIdentity;

        //
        // IBinaryNumber
        //
        // TODO: Review
        /// <inheritdoc cref="IBinaryNumber{TSelf}.AllBitsSet" />
        static NaNDefaultedDouble IBinaryNumber<NaNDefaultedDouble>.AllBitsSet => new(unchecked((Int64)0Xffffffffffffffff));

        /// <inheritdoc cref="IBinaryNumber{TSelf}.IsPow2(TSelf)" />
        public static bool IsPow2(NaNDefaultedDouble value) {
            ulong bits = unchecked((ulong)value.DoubleBits);

            ushort biasedExponent = ExtractBiasedExponentFromBits(bits); ;
            ulong trailingSignificand = ExtractTrailingSignificandFromBits(bits);

            return (value > 0)
                && (biasedExponent != MinBiasedExponent) && (biasedExponent != MaxBiasedExponent)
                && (trailingSignificand == MinTrailingSignificand);
        }

        /// <inheritdoc cref="IBinaryNumber{TSelf}.Log2(TSelf)" />
        public static NaNDefaultedDouble Log2(NaNDefaultedDouble value) => Math.Log2(value);

        //
        // IBitwiseOperators
        //

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_BitwiseAnd(TSelf, TOther)" />
        static NaNDefaultedDouble IBitwiseOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator &(NaNDefaultedDouble first, NaNDefaultedDouble second) {
            return new NaNDefaultedDouble(first.bits & second.bits);
        }

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_BitwiseOr(TSelf, TOther)" />
        static NaNDefaultedDouble IBitwiseOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator |(NaNDefaultedDouble first, NaNDefaultedDouble second) {
            return new NaNDefaultedDouble(first.bits | second.bits);
        }

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_ExclusiveOr(TSelf, TOther)" />
        static NaNDefaultedDouble IBitwiseOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator ^(NaNDefaultedDouble first, NaNDefaultedDouble second) {
            return new NaNDefaultedDouble(first.bits ^ second.bits);
        }

        /// <inheritdoc cref="IBitwiseOperators{TSelf, TOther, TResult}.op_OnesComplement(TSelf)" />
        static NaNDefaultedDouble IBitwiseOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator ~(NaNDefaultedDouble value) {
            return new NaNDefaultedDouble(~value.bits);
        }

        //
        // IDecrementOperators
        //

        /// <inheritdoc cref="IDecrementOperators{TSelf}.op_Decrement(TSelf)" />
        static NaNDefaultedDouble IDecrementOperators<NaNDefaultedDouble>.operator --(NaNDefaultedDouble value) => value.Value - (Double)1.0;

        //
        // IDivisionOperators
        //

        /// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.op_Division(TSelf, TOther)" />
        static NaNDefaultedDouble IDivisionOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator /(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value / second.Value;

        //
        // IExponentialFunctions
        //

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.Exp" />
        public static NaNDefaultedDouble Exp(NaNDefaultedDouble x) => Math.Exp(x);

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.ExpM1(TSelf)" />
        public static NaNDefaultedDouble ExpM1(NaNDefaultedDouble x) => Math.Exp(x) - 1;

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.Exp2(TSelf)" />
        public static NaNDefaultedDouble Exp2(NaNDefaultedDouble x) => Math.Pow(2, x);

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.Exp2M1(TSelf)" />
        public static NaNDefaultedDouble Exp2M1(NaNDefaultedDouble x) => Math.Pow(2, x) - 1;

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.Exp10(TSelf)" />
        public static NaNDefaultedDouble Exp10(NaNDefaultedDouble x) => Math.Pow(10, x);

        /// <inheritdoc cref="IExponentialFunctions{TSelf}.Exp10M1(TSelf)" />
        public static NaNDefaultedDouble Exp10M1(NaNDefaultedDouble x) => Math.Pow(10, x) - 1;

        //
        // IFloatingPoint
        //

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Ceiling(TSelf)" />
        public static NaNDefaultedDouble Ceiling(NaNDefaultedDouble x) => Math.Ceiling(x);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Floor(TSelf)" />
        public static NaNDefaultedDouble Floor(NaNDefaultedDouble x) => Math.Floor(x);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf)" />
        public static NaNDefaultedDouble Round(NaNDefaultedDouble x) => Math.Round(x);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, int)" />
        public static NaNDefaultedDouble Round(NaNDefaultedDouble x, int digits) => Math.Round(x, digits);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, MidpointRounding)" />
        public static NaNDefaultedDouble Round(NaNDefaultedDouble x, MidpointRounding mode) => Math.Round(x, mode);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Round(TSelf, int, MidpointRounding)" />
        public static NaNDefaultedDouble Round(NaNDefaultedDouble x, int digits, MidpointRounding mode) => Math.Round(x, digits, mode);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.Truncate(TSelf)" />
        public static NaNDefaultedDouble Truncate(NaNDefaultedDouble x) => Math.Truncate(x);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.GetExponentByteCount()" />
        int IFloatingPoint<NaNDefaultedDouble>.GetExponentByteCount() => sizeof(short);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.GetExponentShortestBitLength()" />
        int IFloatingPoint<NaNDefaultedDouble>.GetExponentShortestBitLength() {
            short exponent = Exponent;

            if (exponent >= 0) {
                return (sizeof(short) * 8) - short.LeadingZeroCount(exponent);
            } else {
                return (sizeof(short) * 8) + 1 - short.LeadingZeroCount((short)(~exponent));
            }
        }

        /// <inheritdoc cref="IFloatingPoint{TSelf}.GetSignificandByteCount()" />
        int IFloatingPoint<NaNDefaultedDouble>.GetSignificandByteCount() => sizeof(ulong);

        /// <inheritdoc cref="IFloatingPoint{TSelf}.GetSignificandBitLength()" />
        int IFloatingPoint<NaNDefaultedDouble>.GetSignificandBitLength() => 53;

        /// <inheritdoc cref="IFloatingPoint{TSelf}.TryWriteExponentBigEndian(Span{byte}, out int)" />
        bool IFloatingPoint<NaNDefaultedDouble>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(short)) {
                short exponent = Exponent;

                if (BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(short);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        /// <inheritdoc cref="IFloatingPoint{TSelf}.TryWriteExponentLittleEndian(Span{byte}, out int)" />
        bool IFloatingPoint<NaNDefaultedDouble>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(short)) {
                short exponent = Exponent;

                if (!BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(short);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        /// <inheritdoc cref="IFloatingPoint{TSelf}.TryWriteSignificandBigEndian(Span{byte}, out int)" />
        bool IFloatingPoint<NaNDefaultedDouble>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(ulong)) {
                ulong significand = Significand;

                if (BitConverter.IsLittleEndian) {
                    significand = BinaryPrimitives.ReverseEndianness(significand);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = sizeof(ulong);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        /// <inheritdoc cref="IFloatingPoint{TSelf}.TryWriteSignificandLittleEndian(Span{byte}, out int)" />
        bool IFloatingPoint<NaNDefaultedDouble>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(ulong)) {
                ulong significand = Significand;

                if (!BitConverter.IsLittleEndian) {
                    significand = BinaryPrimitives.ReverseEndianness(significand);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = sizeof(ulong);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        //
        // IFloatingPointConstants
        //

        /// <inheritdoc cref="IFloatingPointConstants{TSelf}.E" />
        static NaNDefaultedDouble IFloatingPointConstants<NaNDefaultedDouble>.E => Math.E;

        /// <inheritdoc cref="IFloatingPointConstants{TSelf}.Pi" />
        static NaNDefaultedDouble IFloatingPointConstants<NaNDefaultedDouble>.Pi => Pi;

        /// <inheritdoc cref="IFloatingPointConstants{TSelf}.Tau" />
        static NaNDefaultedDouble IFloatingPointConstants<NaNDefaultedDouble>.Tau => Tau;

        //
        // IFloatingPointIeee754
        //

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.Epsilon" />
        static NaNDefaultedDouble IFloatingPointIeee754<NaNDefaultedDouble>.Epsilon => Epsilon;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.NaN" />
        static NaNDefaultedDouble IFloatingPointIeee754<NaNDefaultedDouble>.NaN => NaN;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.NegativeInfinity" />
        static NaNDefaultedDouble IFloatingPointIeee754<NaNDefaultedDouble>.NegativeInfinity => NegativeInfinity;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.NegativeZero" />
        static NaNDefaultedDouble IFloatingPointIeee754<NaNDefaultedDouble>.NegativeZero => NegativeZero;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.PositiveInfinity" />
        static NaNDefaultedDouble IFloatingPointIeee754<NaNDefaultedDouble>.PositiveInfinity => PositiveInfinity;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.Atan2(TSelf, TSelf)" />
        public static NaNDefaultedDouble Atan2(NaNDefaultedDouble y, NaNDefaultedDouble x) => Math.Atan2(y, x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.Atan2Pi(TSelf, TSelf)" />
        public static NaNDefaultedDouble Atan2Pi(NaNDefaultedDouble y, NaNDefaultedDouble x) => Atan2(y, x) / Pi;

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.BitDecrement(TSelf)" />
        public static NaNDefaultedDouble BitDecrement(NaNDefaultedDouble x) => Math.BitDecrement(x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.BitIncrement(TSelf)" />
        public static NaNDefaultedDouble BitIncrement(NaNDefaultedDouble x) => Math.BitIncrement(x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.FusedMultiplyAdd(TSelf, TSelf, TSelf)" />
        public static NaNDefaultedDouble FusedMultiplyAdd(NaNDefaultedDouble first, NaNDefaultedDouble second, NaNDefaultedDouble addend) => Math.FusedMultiplyAdd(first, second, addend);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.Ieee754Remainder(TSelf, TSelf)" />
        public static NaNDefaultedDouble Ieee754Remainder(NaNDefaultedDouble first, NaNDefaultedDouble second) => Math.IEEERemainder(first, second);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.ILogB(TSelf)" />
        public static int ILogB(NaNDefaultedDouble x) => Math.ILogB(x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.ReciprocalEstimate(TSelf)" />
        public static NaNDefaultedDouble ReciprocalEstimate(NaNDefaultedDouble x) => Math.ReciprocalEstimate(x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.ReciprocalSqrtEstimate(TSelf)" />
        public static NaNDefaultedDouble ReciprocalSqrtEstimate(NaNDefaultedDouble x) => Math.ReciprocalSqrtEstimate(x);

        /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.ScaleB(TSelf, int)" />
        public static NaNDefaultedDouble ScaleB(NaNDefaultedDouble x, int n) => Math.ScaleB(x, n);

        // /// <inheritdoc cref="IFloatingPointIeee754{TSelf}.Compound(TSelf, TSelf)" />
        // public static NaNDefaultedDouble Compound(NaNDefaultedDouble x, NaNDefaultedDouble n) => Math.Compound(x, n);

        //
        // IHyperbolicFunctions
        //

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Acosh(TSelf)" />
        public static NaNDefaultedDouble Acosh(NaNDefaultedDouble x) => Math.Acosh(x);

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Asinh(TSelf)" />
        public static NaNDefaultedDouble Asinh(NaNDefaultedDouble x) => Math.Asinh(x);

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Atanh(TSelf)" />
        public static NaNDefaultedDouble Atanh(NaNDefaultedDouble x) => Math.Atanh(x);

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Cosh(TSelf)" />
        public static NaNDefaultedDouble Cosh(NaNDefaultedDouble x) => Math.Cosh(x);

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Sinh(TSelf)" />
        public static NaNDefaultedDouble Sinh(NaNDefaultedDouble x) => Math.Sinh(x);

        /// <inheritdoc cref="IHyperbolicFunctions{TSelf}.Tanh(TSelf)" />
        public static NaNDefaultedDouble Tanh(NaNDefaultedDouble x) => Math.Tanh(x);

        //
        // IIncrementOperators
        //

        /// <inheritdoc cref="IIncrementOperators{TSelf}.op_Increment(TSelf)" />
        static NaNDefaultedDouble IIncrementOperators<NaNDefaultedDouble>.operator ++(NaNDefaultedDouble value) => value.Value + (Double)1.0;

        //
        // ILogarithmicFunctions
        //

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.Log(TSelf)" />
        public static NaNDefaultedDouble Log(NaNDefaultedDouble x) => Math.Log(x);

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.Log(TSelf, TSelf)" />
        public static NaNDefaultedDouble Log(NaNDefaultedDouble x, NaNDefaultedDouble newBase) => Math.Log(x, newBase);

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.LogP1(TSelf)" />
        public static NaNDefaultedDouble LogP1(NaNDefaultedDouble x) => Math.Log(x + 1);

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.Log2P1(TSelf)" />
        public static NaNDefaultedDouble Log2P1(NaNDefaultedDouble x) => Math.Log2(x + 1);

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.Log10(TSelf)" />
        public static NaNDefaultedDouble Log10(NaNDefaultedDouble x) => Math.Log10(x);

        /// <inheritdoc cref="ILogarithmicFunctions{TSelf}.Log10P1(TSelf)" />
        public static NaNDefaultedDouble Log10P1(NaNDefaultedDouble x) => Math.Log10(x + 1);

        //
        // IMinMaxValue
        //

        /// <inheritdoc cref="IMinMaxValue{TSelf}.MinValue" />
        static NaNDefaultedDouble IMinMaxValue<NaNDefaultedDouble>.MinValue => MinValue;

        /// <inheritdoc cref="IMinMaxValue{TSelf}.MaxValue" />
        static NaNDefaultedDouble IMinMaxValue<NaNDefaultedDouble>.MaxValue => MaxValue;

        //
        // IModulusOperators
        //

        /// <inheritdoc cref="IModulusOperators{TSelf, TOther, TResult}.op_Modulus(TSelf, TOther)" />
        static NaNDefaultedDouble IModulusOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator %(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value % second.Value;

        //
        // IMultiplicativeIdentity
        //

        /// <inheritdoc cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity" />
        static NaNDefaultedDouble IMultiplicativeIdentity<NaNDefaultedDouble, NaNDefaultedDouble>.MultiplicativeIdentity => MultiplicativeIdentity;

        //
        // IMultiplyOperators
        //

        /// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.op_Multiply(TSelf, TOther)" />
        static NaNDefaultedDouble IMultiplyOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator *(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value * second.Value;

        //
        // INumber
        //

        /// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)" />
        public static NaNDefaultedDouble Clamp(NaNDefaultedDouble value, NaNDefaultedDouble min, NaNDefaultedDouble max) => Math.Clamp(value, min, max);

        /// <inheritdoc cref="INumber{TSelf}.CopySign(TSelf, TSelf)" />
        public static NaNDefaultedDouble CopySign(NaNDefaultedDouble value, NaNDefaultedDouble sign) => Math.CopySign(value, sign);

        /// <inheritdoc cref="INumber{TSelf}.Max(TSelf, TSelf)" />
        public static NaNDefaultedDouble Max(NaNDefaultedDouble x, NaNDefaultedDouble y) => Math.Max(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MaxNumber(TSelf, TSelf)" />
        public static NaNDefaultedDouble MaxNumber(NaNDefaultedDouble x, NaNDefaultedDouble y) {
            // This matches the IEEE 754:2019 `maximumNumber` function
            //
            // It does not propagate NaN inputs back to the caller and
            // otherwise returns the larger of the inputs. It
            // treats +0 as larger than -0 as per the specification.

            if (x != y) {
                if (!IsNaN(y)) {
                    return y < x ? x : y;
                }

                return x;
            }

            return IsNegative(y) ? x : y;
        }

        /// <inheritdoc cref="INumber{TSelf}.Min(TSelf, TSelf)" />
        public static NaNDefaultedDouble Min(NaNDefaultedDouble x, NaNDefaultedDouble y) => Math.Min(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MinNumber(TSelf, TSelf)" />
        public static NaNDefaultedDouble MinNumber(NaNDefaultedDouble x, NaNDefaultedDouble y) {
            // This matches the IEEE 754:2019 `minimumNumber` function
            //
            // It does not propagate NaN inputs back to the caller and
            // otherwise returns the larger of the inputs. It
            // treats +0 as larger than -0 as per the specification.

            if (x != y) {
                if (!IsNaN(y)) {
                    return x < y ? x : y;
                }

                return x;
            }

            return IsNegative(x) ? x : y;
        }

        /// <inheritdoc cref="INumber{TSelf}.Sign(TSelf)" />
        public static int Sign(NaNDefaultedDouble value) => Math.Sign(value);

        //
        // INumberBase
        //

        /// <inheritdoc cref="INumberBase{TSelf}.One" />
        static NaNDefaultedDouble INumberBase<NaNDefaultedDouble>.One => One;

        /// <inheritdoc cref="INumberBase{TSelf}.Radix" />
        static int INumberBase<NaNDefaultedDouble>.Radix => 2;

        /// <inheritdoc cref="INumberBase{TSelf}.Zero" />
        static NaNDefaultedDouble INumberBase<NaNDefaultedDouble>.Zero => Zero;

        /// <inheritdoc cref="INumberBase{TSelf}.Abs(TSelf)" />
        public static NaNDefaultedDouble Abs(NaNDefaultedDouble value) => Math.Abs(value);

        /// <inheritdoc cref="INumberBase{TSelf}.CreateChecked{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NaNDefaultedDouble CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            NaNDefaultedDouble result;

            if (typeof(TOther) == typeof(NaNDefaultedDouble)) {
                result = (NaNDefaultedDouble)(object)value;
            } else {
                result = Double.CreateChecked<TOther>(value);
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateSaturating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NaNDefaultedDouble CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            NaNDefaultedDouble result;

            if (typeof(TOther) == typeof(NaNDefaultedDouble)) {
                result = (NaNDefaultedDouble)(object)value;
            } else {
                result = Double.CreateSaturating<TOther>(value);
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateTruncating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static NaNDefaultedDouble CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            NaNDefaultedDouble result;

            if (typeof(TOther) == typeof(NaNDefaultedDouble)) {
                result = (NaNDefaultedDouble)(object)value;
            } else {
                result = Double.CreateTruncating<TOther>(value);
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsCanonical(TSelf)" />
        static bool INumberBase<NaNDefaultedDouble>.IsCanonical(NaNDefaultedDouble value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsComplexNumber(TSelf)" />
        static bool INumberBase<NaNDefaultedDouble>.IsComplexNumber(NaNDefaultedDouble value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsEvenInteger(TSelf)" />
        public static bool IsEvenInteger(NaNDefaultedDouble value) => IsInteger(value) && (Abs(value % 2) == 0);

        /// <inheritdoc cref="INumberBase{TSelf}.IsImaginaryNumber(TSelf)" />
        static bool INumberBase<NaNDefaultedDouble>.IsImaginaryNumber(NaNDefaultedDouble value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInteger(TSelf)" />
        public static bool IsInteger(NaNDefaultedDouble value) => IsFinite(value) && (value == Truncate(value));

        /// <inheritdoc cref="INumberBase{TSelf}.IsOddInteger(TSelf)" />
        public static bool IsOddInteger(NaNDefaultedDouble value) => IsInteger(value) && (Abs(value % 2) == 1);

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositive(TSelf)" />
        public static bool IsPositive(NaNDefaultedDouble value) => 0 > value.bits;

        /// <inheritdoc cref="INumberBase{TSelf}.IsRealNumber(TSelf)" />
        public static bool IsRealNumber(NaNDefaultedDouble value) {
            // A NaN will never equal itself so this is an
            // easy and efficient way to check for a real number.

#pragma warning disable CS1718
            return value == value;
#pragma warning restore CS1718
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsZero(TSelf)" />
        static bool INumberBase<NaNDefaultedDouble>.IsZero(NaNDefaultedDouble value) => NaNBits == (unchecked((Int64)0X8000000000000000) | value.bits);

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitude(TSelf, TSelf)" />
        public static NaNDefaultedDouble MaxMagnitude(NaNDefaultedDouble x, NaNDefaultedDouble y) => Math.MaxMagnitude(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitudeNumber(TSelf, TSelf)" />
        public static NaNDefaultedDouble MaxMagnitudeNumber(NaNDefaultedDouble x, NaNDefaultedDouble y) => Double.MaxMagnitudeNumber(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitude(TSelf, TSelf)" />
        public static NaNDefaultedDouble MinMagnitude(NaNDefaultedDouble x, NaNDefaultedDouble y) => Math.MinMagnitude(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitudeNumber(TSelf, TSelf)" />
        public static NaNDefaultedDouble MinMagnitudeNumber(NaNDefaultedDouble x, NaNDefaultedDouble y) => Double.MinMagnitudeNumber(x, y);


        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<NaNDefaultedDouble>.TryConvertFromChecked<TOther>(TOther value, out NaNDefaultedDouble result) {
            return TryConvertFrom<TOther>(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<NaNDefaultedDouble>.TryConvertFromSaturating<TOther>(TOther value, out NaNDefaultedDouble result) {
            return TryConvertFrom<TOther>(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<NaNDefaultedDouble>.TryConvertFromTruncating<TOther>(TOther value, out NaNDefaultedDouble result) {
            return TryConvertFrom<TOther>(value, out result);
        }

        private static bool TryConvertFrom<TOther>(TOther value, out NaNDefaultedDouble result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `double` will handle the other signed types and
            // `ConvertTo` will handle the unsigned types

            if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = (Double)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualValue = (int)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                System.Int128 actualValue = (System.Int128)(object)value;
                result = (Double)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Double)) {
                Double actualValue = (Double)(object)value;
                result = actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<NaNDefaultedDouble>.TryConvertToChecked<TOther>(NaNDefaultedDouble value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `NaNDefaultedDouble` will handle the other signed types and
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
                decimal actualResult = checked((decimal)value.Value);
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
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                System.UInt128 actualResult = checked((System.UInt128)value.Value);
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
        static bool INumberBase<NaNDefaultedDouble>.TryConvertToSaturating<TOther>(NaNDefaultedDouble value, [MaybeNullWhen(false)] out TOther result) {
            return TryConvertTo<TOther>(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToTruncating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<NaNDefaultedDouble>.TryConvertToTruncating<TOther>(NaNDefaultedDouble value, [MaybeNullWhen(false)] out TOther result) {
            return TryConvertTo<TOther>(value, out result);
        }

        private static bool TryConvertTo<TOther>(NaNDefaultedDouble value, [MaybeNullWhen(false)] out TOther result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `double` will handle the other signed types and
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
                decimal actualResult = (value >= +79228162514264337593543950336.0) ? decimal.MaxValue :
                                       (value <= -79228162514264337593543950336.0) ? decimal.MinValue :
                                       IsNaN(value) ? 0.0m : (decimal)value.Value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualResult = (value >= ushort.MaxValue) ? ushort.MaxValue :
                                      (value <= ushort.MinValue) ? ushort.MinValue : (ushort)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult = (value >= uint.MaxValue) ? uint.MaxValue :
                                    (value <= uint.MinValue) ? uint.MinValue : (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult = (value >= ulong.MaxValue) ? ulong.MaxValue :
                                     (value <= ulong.MinValue) ? ulong.MinValue :
                                     IsNaN(value) ? 0 : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                System.UInt128 actualResult = (value >= 340282366920938463463374607431768211455.0) ? System.UInt128.MaxValue :
                                       (value <= 0.0) ? System.UInt128.MinValue : (System.UInt128)value.Value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
#if TARGET_64BIT
                nuint actualResult = (value >= ulong.MaxValue) ? unchecked((nuint)ulong.MaxValue) :
                                     (value <= ulong.MinValue) ? unchecked((nuint)ulong.MinValue) : (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
#else
                nuint actualResult = (value >= uint.MaxValue) ? uint.MaxValue :
                                     (value <= uint.MinValue) ? uint.MinValue : (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
#endif
            } else {
                result = default;
                return false;
            }
        }

        //
        // IParsable
        //
        /// <inheritdoc cref="Double.TryParse(string?, IFormatProvider?, out Double)" />
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out NaNDefaultedDouble result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

        //
        // IPowerFunctions
        //

        /// <inheritdoc cref="IPowerFunctions{TSelf}.Pow(TSelf, TSelf)" />
        public static NaNDefaultedDouble Pow(NaNDefaultedDouble x, NaNDefaultedDouble y) => Math.Pow(x, y);

        //
        // IRootFunctions
        //

        /// <inheritdoc cref="IRootFunctions{TSelf}.Cbrt(TSelf)" />
        public static NaNDefaultedDouble Cbrt(NaNDefaultedDouble x) => Math.Cbrt(x);

        /// <inheritdoc cref="IRootFunctions{TSelf}.Hypot(TSelf, TSelf)" />
        public static NaNDefaultedDouble Hypot(NaNDefaultedDouble x, NaNDefaultedDouble y) {
            return Double.Hypot(x, y);
        }

        /// <inheritdoc cref="IRootFunctions{TSelf}.RootN(TSelf, int)" />
        public static NaNDefaultedDouble RootN(NaNDefaultedDouble x, int n) {
            return Double.RootN(x, n);
        }

        /// <inheritdoc cref="IRootFunctions{TSelf}.Sqrt(TSelf)" />
        public static NaNDefaultedDouble Sqrt(NaNDefaultedDouble x) => Math.Sqrt(x);

        //
        // ISignedNumber
        //

        /// <inheritdoc cref="ISignedNumber{TSelf}.NegativeOne" />
        static NaNDefaultedDouble ISignedNumber<NaNDefaultedDouble>.NegativeOne => NegativeOne;

        //
        // ISpanParsable
        //

        /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
        public static NaNDefaultedDouble Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out NaNDefaultedDouble result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

        //
        // ISubtractionOperators
        //

        /// <inheritdoc cref="ISubtractionOperators{TSelf, TOther, TResult}.op_Subtraction(TSelf, TOther)" />
        static NaNDefaultedDouble ISubtractionOperators<NaNDefaultedDouble, NaNDefaultedDouble, NaNDefaultedDouble>.operator -(NaNDefaultedDouble first, NaNDefaultedDouble second) => first.Value - second.Value;

        //
        // ITrigonometricFunctions
        //

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Acos(TSelf)" />
        public static NaNDefaultedDouble Acos(NaNDefaultedDouble x) => Math.Acos(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.AcosPi(TSelf)" />
        public static NaNDefaultedDouble AcosPi(NaNDefaultedDouble x) => Double.AcosPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Asin(TSelf)" />
        public static NaNDefaultedDouble Asin(NaNDefaultedDouble x) => Math.Asin(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.AsinPi(TSelf)" />
        public static NaNDefaultedDouble AsinPi(NaNDefaultedDouble x) => Double.AsinPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Atan(TSelf)" />
        public static NaNDefaultedDouble Atan(NaNDefaultedDouble x) => Math.Atan(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.AtanPi(TSelf)" />
        public static NaNDefaultedDouble AtanPi(NaNDefaultedDouble x) => Double.AtanPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Cos(TSelf)" />
        public static NaNDefaultedDouble Cos(NaNDefaultedDouble x) => Math.Cos(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.CosPi(TSelf)" />
        public static NaNDefaultedDouble CosPi(NaNDefaultedDouble x) => Double.CosPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Sin(TSelf)" />
        public static NaNDefaultedDouble Sin(NaNDefaultedDouble x) => Math.Sin(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.SinCos(TSelf)" />
        public static (NaNDefaultedDouble Sin, NaNDefaultedDouble Cos) SinCos(NaNDefaultedDouble x) => Math.SinCos(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.SinCos(TSelf)" />
        public static (NaNDefaultedDouble SinPi, NaNDefaultedDouble CosPi) SinCosPi(NaNDefaultedDouble x) => Double.SinCosPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.SinPi(TSelf)" />
        public static NaNDefaultedDouble SinPi(NaNDefaultedDouble x) => Double.SinPi(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.Tan(TSelf)" />
        public static NaNDefaultedDouble Tan(NaNDefaultedDouble x) => Math.Tan(x);

        /// <inheritdoc cref="ITrigonometricFunctions{TSelf}.TanPi(TSelf)" />
        public static NaNDefaultedDouble TanPi(NaNDefaultedDouble x) => Double.TanPi(x);

        //
        // IUnaryNegationOperators
        //

        /// <inheritdoc cref="IUnaryNegationOperators{TSelf, TResult}.op_UnaryNegation(TSelf)" />
        static NaNDefaultedDouble IUnaryNegationOperators<NaNDefaultedDouble, NaNDefaultedDouble>.operator -(NaNDefaultedDouble value) => -value.Value;

        //
        // IUnaryPlusOperators
        //

        /// <inheritdoc cref="IUnaryPlusOperators{TSelf, TResult}.op_UnaryPlus(TSelf)" />
        static NaNDefaultedDouble IUnaryPlusOperators<NaNDefaultedDouble, NaNDefaultedDouble>.operator +(NaNDefaultedDouble value) => value;

        private string GetDebuggerDisplay() {
            return this.ToString();
        }

        /*
        //
        // Helpers
        //

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static NaNDefaultedDouble CosForIntervalPiBy4(NaNDefaultedDouble x, NaNDefaultedDouble xTail) {
            // This code is based on `cos_piby4` from amd/aocl-libm-ose
            // Copyright (C) 2008-2020 Advanced Micro Devices, Inc. All rights reserved.
            //
            // Licensed under the BSD 3-Clause "New" or "Revised" License
            // See THIRD-PARTY-NOTICES.TXT for the full license text

            // Taylor series for cos(x) is: 1 - (x^2 / 2!) + (x^4 / 4!) - (x^6 / 6!) ...
            //
            // Then define f(xx) where xx = (x * x)
            // and f(xx) = 1 - (xx / 2!) + (xx^2 / 4!) - (xx^3 / 6!) ...
            //
            // We use a minimax approximation of (f(xx) - 1 + (xx / 2)) / (xx * xx)
            // because this produces an expansion in even powers of x.
            //
            // If xTail is non-zero, we subtract a correction term g(x, xTail) = (x * xTail)
            // to the result, where g(x, xTail) is an approximation to sin(x) * sin(xTail)
            //
            // This is valid because xTail is tiny relative to x.

            const double C1 = +0.41666666666666665390037E-1;        // approx: +1 / 4!
            const double C2 = -0.13888888888887398280412E-2;        // approx: -1 / 6!
            const double C3 = +0.248015872987670414957399E-4;       // approx: +1 / 8!
            const double C4 = -0.275573172723441909470836E-6;       // approx: -1 / 10!
            const double C5 = +0.208761463822329611076335E-8;       // approx: +1 / 12!
            const double C6 = -0.113826398067944859590880E-10;      // approx: -1 / 14!

            double xx = x * x;

            double tmp1 = 0.5 * xx;
            double tmp2 = 1.0 - tmp1;

            double result = C6;

            result = (result * xx) + C5;
            result = (result * xx) + C4;
            result = (result * xx) + C3;
            result = (result * xx) + C2;
            result = (result * xx) + C1;

            result *= (xx * xx);
            result += 1.0 - tmp2 - tmp1 - (x * xTail);
            result += tmp2;

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static NaNDefaultedDouble SinForIntervalPiBy4(NaNDefaultedDouble x, NaNDefaultedDouble xTail) {
            // This code is based on `sin_piby4` from amd/aocl-libm-ose
            // Copyright (C) 2008-2020 Advanced Micro Devices, Inc. All rights reserved.
            //
            // Licensed under the BSD 3-Clause "New" or "Revised" License
            // See THIRD-PARTY-NOTICES.TXT for the full license text

            // Taylor series for sin(x) is x - (x^3 / 3!) + (x^5 / 5!) - (x^7 / 7!) ...
            // Which can be expressed as x * (1 - (x^2 / 3!) + (x^4 /5!) - (x^6 /7!) ...)
            //
            // Then define f(xx) where xx = (x * x)
            // and f(xx) = 1 - (xx / 3!) + (xx^2 / 5!) - (xx^3 / 7!) ...
            //
            // We use a minimax approximation of (f(xx) - 1) / xx
            // because this produces an expansion in even powers of x.
            //
            // If xTail is non-zero, we add a correction term g(x, xTail) = (1 - xx / 2) * xTail
            // to the result, where g(x, xTail) is an approximation to cos(x) * sin(xTail)
            //
            // This is valid because xTail is tiny relative to x.

            const double C1 = -0.166666666666666646259241729;       // approx: -1 / 3!
            const double C2 = +0.833333333333095043065222816E-2;    // approx: +1 / 5!
            const double C3 = -0.19841269836761125688538679E-3;     // approx: -1 / 7!
            const double C4 = +0.275573161037288022676895908448E-5; // approx: +1 / 9!
            const double C5 = -0.25051132068021699772257377197E-7;  // approx: -1 / 11!
            const double C6 = +0.159181443044859136852668200E-9;    // approx: +1 / 13!

            double xx = x * x;
            double xxx = xx * x;

            double result = C6;

            result = (result * xx) + C5;
            result = (result * xx) + C4;
            result = (result * xx) + C3;
            result = (result * xx) + C2;

            if (xTail == 0.0) {
                result = (xx * result) + C1;
                result = (xxx * result) + x;
            } else {
                result = x - ((xx * ((0.5 * xTail) - (xxx * result))) - xTail - (xxx * C1));
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static NaNDefaultedDouble TanForIntervalPiBy4(NaNDefaultedDouble x, NaNDefaultedDouble xTail, bool isReciprocal) {
            // This code is based on `tan_piby4` from amd/aocl-libm-ose
            // Copyright (C) 2008-2020 Advanced Micro Devices, Inc. All rights reserved.
            //
            // Licensed under the BSD 3-Clause "New" or "Revised" License
            // See THIRD-PARTY-NOTICES.TXT for the full license text

            // In order to maintain relative precision transform using the identity:
            //  tan((pi / 4) - x) = (1 - tan(x)) / (1 + tan(x)) for arguments close to (pi / 4).
            //
            // Similarly use tan(x - (pi / 4)) = (tan(x) - 1) / (tan(x) + 1) close to (-pi / 4).

            const double PiBy4Head = 7.85398163397448278999E-01;
            const double PiBy4Tail = 3.06161699786838240164E-17;

            int transform = 0;

            if (x > +0.68) {
                transform = 1;
                x = (PiBy4Head - x) + (PiBy4Tail - xTail);
                xTail = 0.0;
            } else if (x < -0.68) {
                transform = -1;
                x = (PiBy4Head + x) + (PiBy4Tail + xTail);
                xTail = 0.0;
            }

            // Core Remez [2, 3] approximation to tan(x + xTail) on the interval [0, 0.68].

            double tmp1 = (x * x) + (2.0 * x * xTail);

            double denominator = -0.232371494088563558304549252913E-3;
            denominator = +0.260656620398645407524064091208E-1 + (denominator * tmp1);
            denominator = -0.515658515729031149329237816945E+0 + (denominator * tmp1);
            denominator = +0.111713747927937668539901657944E+1 + (denominator * tmp1);

            double numerator = +0.224044448537022097264602535574E-3;
            numerator = -0.229345080057565662883358588111E-1 + (numerator * tmp1);
            numerator = +0.372379159759792203640806338901E+0 + (numerator * tmp1);

            double tmp2 = x * tmp1;
            tmp2 *= numerator / denominator;
            tmp2 += xTail;

            // Reconstruct tan(x) in the transformed case

            double result = x + tmp2;

            if (transform != 0) {
                if (isReciprocal) {
                    result = (transform * (2 * result / (result - 1))) - 1.0;
                } else {
                    result = transform * (1.0 - (2 * result / (1 + result)));
                }
            } else if (isReciprocal) {
                // Compute -1.0 / (x + tmp2) accurately

                ulong bits = BitConverter.DoubleToUInt64Bits(result);
                bits &= 0xFFFFFFFF00000000;

                double z1 = BitConverter.UInt64BitsToDouble(bits);
                double z2 = tmp2 - (z1 - x);

                double reciprocal = -1.0 / result;

                bits = BitConverter.DoubleToUInt64Bits(reciprocal);
                bits &= 0xFFFFFFFF00000000;

                double reciprocalHead = BitConverter.UInt64BitsToDouble(bits);
                result = reciprocalHead + (reciprocal * (1.0 + (reciprocalHead * z1) + (reciprocalHead * z2)));
            }

            return result;
        }
        */
    }
}