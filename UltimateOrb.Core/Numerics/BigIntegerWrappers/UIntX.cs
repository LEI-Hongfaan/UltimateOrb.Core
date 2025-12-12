using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UltimateOrb.Functional.DataTypes;
using UltimateOrb.Numerics.DataTypes;
using UltimateOrb.Numerics.Generic;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {

    interface INumberBaseEx<TSelf> : INumberBase<TSelf>
        where TSelf : INumberBase<TSelf> {

        public static bool TryConvertFromChecked<TOther>(TOther value, out TSelf result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertFromChecked(value, out result);

        public static bool TryConvertFromSaturating<TOther>(TOther value, out TSelf result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertFromSaturating(value, out result);

        public static bool TryConvertFromTruncating<TOther>(TOther value, out TSelf result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertFromTruncating(value, out result);

        public static bool TryConvertToChecked<TOther>(TSelf value, out TOther result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertToChecked(value, out result);

        public static bool TryConvertToSaturating<TOther>(TSelf value, out TOther result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertToSaturating(value, out result);

        public static bool TryConvertToTruncating<TOther>(TSelf value, out TOther result) where TOther : System.Numerics.INumberBase<TOther> => TryConvertToTruncating(value, out result);
    }
}

namespace UltimateOrb.Numerics.BigIntegerWrappers {
    using static ConstructorTags;


    [Experimental("UoWIP_GenericMath")]
    public interface IXIntX {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IIntX : IXIntX {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IUIntX : IXIntX {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IXIntX<TSelf, TBitSize> :
        IXIntX,
        IBinaryInteger<TSelf>,
        IMinMaxValue<TSelf>
        where TSelf : IXIntX<TSelf, TBitSize>
        where TBitSize :
            IConstant<TBitSize, int> {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IIntX<TSelf, TBitSize> :
        IIntX,
        IXIntX<TSelf, TBitSize>,
        ISignedNumber<TSelf>
        where TSelf : IIntX<TSelf, TBitSize>
        where TBitSize :
            IConstant<TBitSize, int> {
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IUIntX<TSelf, TBitSize> :
        IUIntX,
        IXIntX<TSelf, TBitSize>,
        IUnsignedNumber<TSelf>
        where TSelf : IUIntX<TSelf, TBitSize>
        where TBitSize :
            IConstant<TBitSize, int> {
    }

    [Experimental("UoWIP_GenericMath")]
    [Serializable]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [CLSCompliant(false)]
    public readonly struct UInt<TBitSize> :
        IUIntX<UInt<TBitSize>, TBitSize>,
        IBinaryInteger<UInt<TBitSize>>,
        IUnsignedNumber<UInt<TBitSize>>,
        IMinMaxValue<UInt<TBitSize>>
        where TBitSize :
            IConstant<TBitSize, int> {

        readonly BigInteger Value;

        static int BitSize {

            get => TXIntInternal<TBitSize>.BitSize;
        }

        public static UInt<TBitSize> MinValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new UInt<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        public static UInt<TBitSize> MaxValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new UInt<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        static BigInteger MinValueAsBigInteger {

            get => TXIntInternal<TBitSize>.UnsignedMinValueAsBigInteger;
        }

        static BigInteger MaxValueAsBigInteger {

            get => TXIntInternal<TBitSize>.UnsignedMaxValueAsBigInteger;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        UInt(BigInteger value, Checked _ = default) {
            checked(1u - value.Sign.ToUnsignedUnchecked()).Ignore();
            UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(BitSize, value.GetBitLength().ToUnsignedUnchecked());
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        UInt(BigInteger value, Plain _) => Value = value;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        UInt(BigInteger value, Unchecked _) {
            var t = MaxValueAsBigInteger & value;
            Value = t;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt<TBitSize> CreateNewChecked(BigInteger Value) {
            return new UInt<TBitSize>(Value, default(Checked));
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool TryCreateNewChecked(BigInteger Value, out UInt<TBitSize> result) {
            if (Value > MaxValueAsBigInteger || Value < MinValueAsBigInteger) {
                result = default;
                return false;
            }
            result = new UInt<TBitSize>(Value, default(Plain));
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static UInt<TBitSize> CreateNewSaturated(BigInteger Value) {
            if (Value > MaxValueAsBigInteger) {
                Value = MaxValueAsBigInteger;
            } else if (Value < MinValueAsBigInteger) {
                Value = MinValueAsBigInteger;
            }
            return new UInt<TBitSize>(Value, default(Plain));
        }

        static UInt<TBitSize> CreateNewUnchecked(BigInteger Value) {
            return new UInt<TBitSize>(Value, default(Unchecked));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public UInt(int value) {
            UltimateOrb.Utilities.ThrowHelper.ThrowOnNegative(value);
            if (BitSize < sizeof(int) * 8 - 1) {
                UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(unchecked(((uint)1 << BitSize) - 1), value.ToUnsignedUnchecked());
            }
            Value = new BigInteger(value);
            AssertValid();
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public UInt(uint value) {
            if (BitSize < sizeof(uint) * 8) {
                UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(unchecked(((uint)1 << BitSize) - 1), value);
            }
            Value = new BigInteger(value);
            AssertValid();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public UInt(long value) {
            UltimateOrb.Utilities.ThrowHelper.ThrowOnNegative(value);
            if (BitSize < sizeof(long) * 8 - 1) {
                UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(unchecked(((ulong)1 << BitSize) - 1), value.ToUnsignedUnchecked());
            }
            Value = new BigInteger(value);
            AssertValid();
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public UInt(ulong value) {
            if (BitSize < sizeof(ulong) * 8) {
                UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(unchecked(((ulong)1 << BitSize) - 1), value);
            }
            Value = new BigInteger(value);
            AssertValid();
        }

        public UInt(float value) : this((double)value) {
        }

        public UInt(double value) {
            this = CreateNewChecked(new BigInteger(value));
        }

        public UInt(decimal value) {
            this = CreateNewChecked(new BigInteger(value));
        }

        /// <summary>
        /// Creates a BigInteger from a little-endian twos-complement byte array.
        /// </summary>
        /// <param name="value"></param>
        [CLSCompliant(false)]
        public UInt(byte[] value) {
            this = CreateNewChecked(new BigInteger(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public UInt(ReadOnlySpan<byte> value, bool isUnsigned = false, bool isBigEndian = false) {
            this = CreateNewChecked(new BigInteger(value, isUnsigned, isBigEndian));
        }

        public static UInt<TBitSize> Zero {

            get => default;
        }

        public static UInt<TBitSize> One {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new UInt<TBitSize>(BigInteger.One, default(Plain));
        }

        public bool IsPowerOfTwo {

            get => BigInteger.IsPow2(Value);
        }

        public bool IsZero {

            get => Value.IsZero;
        }

        public bool IsOne {

            get => Value.IsOne;
        }

        public bool IsEven {

            get => Value.IsEven;
        }

        public int Sign {

            get => Value.Sign;
        }

        public static UInt<TBitSize> Parse(string value) {
            return CreateNewChecked(Parse(value, NumberStyles.Integer));
        }

        public static UInt<TBitSize> Parse(string value, NumberStyles style) {
            return CreateNewChecked(Parse(value, style, NumberFormatInfo.CurrentInfo));
        }

        public static UInt<TBitSize> Parse(string value, IFormatProvider? provider) {
            return CreateNewChecked(Parse(value, NumberStyles.Integer, NumberFormatInfo.GetInstance(provider)));
        }

        public static UInt<TBitSize> Parse(string value, NumberStyles style, IFormatProvider? provider) {
            ArgumentNullException.ThrowIfNull(value);
            return CreateNewChecked(Parse(value.AsSpan(), style, NumberFormatInfo.GetInstance(provider)));
        }

        public static bool TryParse([NotNullWhen(true)] string? value, out UInt<TBitSize> result) {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse([NotNullWhen(true)] string? value, NumberStyles style, IFormatProvider? provider, out UInt<TBitSize> result) {
            return TryParse(value.AsSpan(), style, NumberFormatInfo.GetInstance(provider), out result);
        }

        public static UInt<TBitSize> Parse(ReadOnlySpan<char> value, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null) {
            return CreateNewChecked(BigInteger.Parse(value, style, NumberFormatInfo.GetInstance(provider)));
        }

        public static bool TryParse(ReadOnlySpan<char> value, out UInt<TBitSize> result) {
            return TryParse(value, NumberStyles.Integer, NumberFormatInfo.CurrentInfo, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> value, NumberStyles style, IFormatProvider? provider, out UInt<TBitSize> result) {
            if (BigInteger.TryParse(value, style, NumberFormatInfo.GetInstance(provider), out var t)) {
                if (t.Sign >= 0 && t.GetBitLength() < BitSize) {
                    result = CreateNewUnchecked(t);
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static int Compare(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.Value.CompareTo(right.Value);
        }

        static UInt<TBitSize> INumberBase<UInt<TBitSize>>.Abs(UInt<TBitSize> value) => Abs(value);

        private static UInt<TBitSize> Abs(UInt<TBitSize> value) {
            return value;
        }

        public static UInt<TBitSize> Multiply(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left * right;
        }

        public static UInt<TBitSize> Divide(UInt<TBitSize> dividend, UInt<TBitSize> divisor) {
            return dividend / divisor;
        }

        public static UInt<TBitSize> Remainder(UInt<TBitSize> dividend, UInt<TBitSize> divisor) {
            return dividend % divisor;
        }

        public static UInt<TBitSize> DivRem(UInt<TBitSize> dividend, UInt<TBitSize> divisor, out UInt<TBitSize> remainder) {
            var q = BigInteger.DivRem(dividend, divisor, out var r);
            var r1 = CreateNewChecked(r);
            var q1 = CreateNewChecked(q);
            remainder = r1;
            return q1;
        }

        public static UInt<TBitSize> Negate(UInt<TBitSize> value) {
            return -value;
        }

        public static double Log(UInt<TBitSize> value) {
            return BigInteger.Log(value, Math.E);
        }

        public static double Log(UInt<TBitSize> value, double baseValue) {
            return BigInteger.Log(value, Math.E);
        }

        public static double Log10(UInt<TBitSize> value) {
            return Log(value, 10.0);
        }

        public static UInt<TBitSize> GreatestCommonDivisor(UInt<TBitSize> left, UInt<TBitSize> right) {
            return new UInt<TBitSize>(BigInteger.GreatestCommonDivisor(left, right), default(Plain));
        }

        public static UInt<TBitSize> Max(UInt<TBitSize> left, UInt<TBitSize> right) {
            if (left.CompareTo(right) < 0)
                return right;
            return left;
        }

        public static UInt<TBitSize> Min(UInt<TBitSize> left, UInt<TBitSize> right) {
            if (left.CompareTo(right) <= 0)
                return left;
            return right;
        }

        public static UInt<TBitSize> ModPow(UInt<TBitSize> value, UInt<TBitSize> exponent, UInt<TBitSize> modulus) {
            return new UInt<TBitSize>(BigInteger.ModPow(value, exponent, modulus), default(Plain));
        }

        public static UInt<TBitSize> Pow(UInt<TBitSize> value, int exponent) {
            ArgumentOutOfRangeException.ThrowIfNegative(exponent);
            if (exponent == 0)
                return One;
            if (exponent == 1)
                return value;
            if (value.IsZero || value.IsOne) {
                return value;
            }
            // Early exit if overflow detected by log2 of result.
            var bitLength = value.Value.GetBitLength(); // bitLength >= 2
            UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThanOrEqual((bitLength - 1) * exponent, BitSize);
            return CreateNewChecked(BigInteger.Pow(value, exponent));
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is UInt<TBitSize> other && Equals(other);
        }

        public bool Equals(long other) {
            return Value.Equals(other);
        }

        [CLSCompliant(false)]
        public bool Equals(ulong other) {
            return Value.Equals(other);
        }

        public bool Equals(BigInteger other) {
            return Value.Equals(other);
        }

        public bool Equals(Int<TBitSize> other) {
            return Value.Equals(other);
        }

        public bool Equals(UInt<TBitSize> other) {
            return Value.Equals(other);
        }

        public int CompareTo(long other) {
            return Value.CompareTo(other);
        }

        [CLSCompliant(false)]
        public int CompareTo(ulong other) {
            return Value.CompareTo(other);
        }

        public int CompareTo(BigInteger other) {
            return Value.CompareTo(other);
        }

        public int CompareTo(Int<TBitSize> other) {
            return Value.CompareTo(other);
        }

        public int CompareTo(UInt<TBitSize> other) {
            return Value.CompareTo(other);
        }

        public int CompareTo(object? obj) {
            if (obj == null)
                return 1;
            if (obj is not UInt<TBitSize> bigInt)
                throw new ArgumentException("Argument_MustBeUInt", nameof(obj));
            return Value.CompareTo(bigInt);
        }

        public override string ToString() {
            return this.ToString(null, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(IFormatProvider? provider) {
            return this.ToString(null, NumberFormatInfo.GetInstance(provider));
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return this.ToString(format, NumberFormatInfo.CurrentInfo);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            return this.Value.ToString(format, NumberFormatInfo.GetInstance(provider));
        }

        static partial class BigIntegerHelpers {

            [UnsafeAccessor(UnsafeAccessorKind.Method)]
            public extern static string get_DebuggerDisplay(BigInteger value);
        }

        private string DebuggerDisplay {

            get => BigIntegerHelpers.get_DebuggerDisplay(Value);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            return Value.TryFormat(destination, out charsWritten, format, provider);
        }

        public static UInt<TBitSize> Add(UInt<TBitSize> left, UInt<TBitSize> right) {
            return checked(left + right);
        }

        public static UInt<TBitSize> operator checked -(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewChecked(first.Value - second.Value);
        }

        public static UInt<TBitSize> operator -(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewUnchecked(first.Value - second.Value);
        }

        public static UInt<TBitSize> Subtract(UInt<TBitSize> left, UInt<TBitSize> right) {
            return checked(left - right);
        }

        //
        // Explicit Conversions From BigInteger
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static T ConvertUnsignedCheckedTo<T>(UInt<TBitSize> value) where T : unmanaged, IUnsignedNumber<T> {
            if (BitSize <= Unsafe.SizeOf<T>() * 8) {
                return T.CreateTruncating(value.Value);
            }
            return T.CreateChecked(value.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static T ConvertSignedCheckedTo<T>(UInt<TBitSize> value) where T : unmanaged, ISignedNumber<T> {
            if (BitSize < Unsafe.SizeOf<T>() * 8) {
                return T.CreateTruncating(value.Value);
            }
            return T.CreateChecked(value.Value);
        }

        public static explicit operator checked byte(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<byte>(value);
        }

        public static explicit operator/* unchecked*/ byte(UInt<TBitSize> value) {
            return byte.CreateTruncating(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="char" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="char" /> value.</returns>
        public static explicit operator checked char(UInt<TBitSize> value) {
            return (char)checked((ushort)value);
        }

        public static explicit operator/* unchecked*/ char(UInt<TBitSize> value) {
            return (char)unchecked((ushort)value);
        }

        public static explicit operator/* checked*/ decimal(UInt<TBitSize> value) {
            return checked((decimal)value.Value);
        }

        public static explicit operator double(UInt<TBitSize> value) {
            return double.CreateChecked(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="Half" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="Half" /> value.</returns>
        public static explicit operator Half(UInt<TBitSize> value) {
            return Half.CreateChecked(value.Value);
        }

        public static explicit operator checked short(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<short>(value);
        }

        public static explicit operator/* unchecked*/ short(UInt<TBitSize> value) {
            return short.CreateTruncating(value.Value);
        }

        public static explicit operator checked int(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<int>(value);
        }

        public static explicit operator/* unchecked*/ int(UInt<TBitSize> value) {
            return int.CreateTruncating(value.Value);
            /*
            value.AssertValid();
            var sign = value.Value.GetSignField();
            var bits = value.Value.GetBitsField();
            return bits == null ? sign : unchecked(sign * (int)bits[0]);
            */
        }

        public static explicit operator checked long(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<long>(value);
        }

        public static explicit operator/* unchecked*/ long(UInt<TBitSize> value) {
            return long.CreateTruncating(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="Int128" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="Int128" /> value.</returns>
        public static explicit operator checked UltimateOrb.Int128(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<System.Int128>(value);
        }

        public static explicit operator/* unchecked*/ UltimateOrb.Int128(UInt<TBitSize> value) {
            return System.Int128.CreateTruncating(value.Value);
        }

        public static explicit operator checked System.Int128(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<System.Int128>(value);
        }

        public static explicit operator/* unchecked*/ System.Int128(UInt<TBitSize> value) {
            return System.Int128.CreateTruncating(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="IntPtr" /> value.</returns>
        public static explicit operator checked nint(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<nint>(value);
        }

        public static explicit operator/* unchecked*/ nint(UInt<TBitSize> value) {
            return nint.CreateTruncating(value.Value);
        }

        // code below is WIP and need rewriting for correctness and performance
        [CLSCompliant(false)]
        public static explicit operator checked sbyte(UInt<TBitSize> value) {
            return ConvertSignedCheckedTo<sbyte>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ sbyte(UInt<TBitSize> value) {
            return sbyte.CreateTruncating(value.Value);
        }

        public static explicit operator float(UInt<TBitSize> value) {
            return float.CreateChecked(value.Value);
        }

        [CLSCompliant(false)]
        public static explicit operator checked ushort(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<ushort>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ ushort(UInt<TBitSize> value) {
            return ushort.CreateTruncating(value.Value);
        }

        [CLSCompliant(false)]
        public static explicit operator checked uint(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<uint>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ uint(UInt<TBitSize> value) {
            return uint.CreateTruncating(value.Value);
        }

        [CLSCompliant(false)]
        public static explicit operator checked ulong(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<ulong>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ ulong(UInt<TBitSize> value) {
            return ulong.CreateTruncating(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="UInt128" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="UInt128" /> value.</returns>
        [CLSCompliant(false)]
        public static explicit operator checked UltimateOrb.UInt128(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<System.UInt128>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UltimateOrb.UInt128(UInt<TBitSize> value) {
            return System.UInt128.CreateTruncating(value.Value);
        }
        /// <summary>Explicitly converts a big integer to a <see cref="UInt128" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="UInt128" /> value.</returns>
        [CLSCompliant(false)]
        public static explicit operator checked System.UInt128(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<System.UInt128>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ System.UInt128(UInt<TBitSize> value) {
            return System.UInt128.CreateTruncating(value.Value);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="UIntPtr" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="UIntPtr" /> value.</returns>
        [CLSCompliant(false)]
        public static explicit operator checked nuint(UInt<TBitSize> value) {
            return ConvertUnsignedCheckedTo<nuint>(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ nuint(UInt<TBitSize> value) {
            return nuint.CreateTruncating(value.Value);
        }

        public static implicit operator BigInteger(UInt<TBitSize> value) {
            return value.Value;
        }

        //
        // Explicit Conversions To UInt<TBitSize>
        //

        public static explicit operator checked UInt<TBitSize>(BigInteger value) {
            return CreateNewChecked(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(BigInteger value) {
            return CreateNewUnchecked(value);
        }

        public static explicit operator checked UInt<TBitSize>(Int<TBitSize> value) {
            return CreateNewChecked(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(Int<TBitSize> value) {
            return CreateNewUnchecked(value);
        }

        public static explicit operator UInt<TBitSize>(decimal value) {
            return CreateNewChecked(checked((BigInteger)value));
        }

        public static explicit operator UInt<TBitSize>(double value) {
            return CreateNewChecked(checked((BigInteger)value));
        }

        /// <summary>Explicitly converts a <see cref="Half" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator UInt<TBitSize>(Half value) {
            return CreateNewChecked(checked((BigInteger)(float)value));
        }

        /// <summary>Explicitly converts a <see cref="Complex" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator UInt<TBitSize>(Complex value) {
            var i = Math.Abs(value.Imaginary);
            var r = Math.Abs(value.Real);
            UltimateOrb.Utilities.ThrowHelper.ThrowOnNotEqualNumeric(i, r - i);
            return CreateNewChecked((BigInteger)value.Real);
        }

        public static explicit operator UInt<TBitSize>(float value) {
            return CreateNewChecked(checked((BigInteger)value));
        }

        //
        // Implicit Conversions To UInt<TBitSize>
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static UInt<TBitSize> ConvertUnsignedCheckedFrom<T>(T value) where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T> {
            if (BitSize < Unsafe.SizeOf<T>() * 8) {
                _ = checked(unchecked((T.One << BitSize) - T.One) - value);
            }
            return new UInt<TBitSize>(BigInteger.CreateTruncating(value), default(Plain));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        static UInt<TBitSize> ConvertSignedCheckedFrom<T>(T value) where T : unmanaged, IBinaryInteger<T>, ISignedNumber<T> {
            UltimateOrb.Utilities.ThrowHelper.ThrowOnTrue(T.IsNegative(value));
            if (BitSize < Unsafe.SizeOf<T>() * 8 - 1) {
                UltimateOrb.Utilities.ThrowHelper.ThrowOnTrue(unchecked((T.One << BitSize) - T.One) >= value);
            }
            return new UInt<TBitSize>(BigInteger.CreateTruncating(value), default(Plain));
        }

        public static explicit operator checked UInt<TBitSize>(byte value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(byte value) {
            var t = new BigInteger(value);
            if (BitSize >= Unsafe.SizeOf<byte>() * 8) {
                return new UInt<TBitSize>(t, default(Plain));
            }
            return CreateNewUnchecked(t);
        }

        /// <summary>Implicitly converts a <see cref="char" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator checked UInt<TBitSize>(char value) {
            return checked((UInt<TBitSize>)unchecked((ushort)value));
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(char value) {
            return unchecked((UInt<TBitSize>)unchecked((ushort)value));
        }

        public static explicit operator checked UInt<TBitSize>(short value) {
            return ConvertSignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(short value) {
            return CreateNewUnchecked(BigInteger.CreateTruncating(value));
        }

        public static explicit operator checked UInt<TBitSize>(int value) {
            return ConvertSignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(int value) {
            return CreateNewUnchecked(new BigInteger(value));
        }

        public static explicit operator checked UInt<TBitSize>(long value) {
            return ConvertSignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(long value) {
            return CreateNewUnchecked(new BigInteger(value));
        }

        /// <summary>Implicitly converts a <see cref="Int128" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator checked UInt<TBitSize>(UltimateOrb.Int128 value) {
            return checked((UInt<TBitSize>)(System.Int128)value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(UltimateOrb.Int128 value) {
            return unchecked((UInt<TBitSize>)(System.Int128)value);
        }

        public static explicit operator checked UInt<TBitSize>(System.Int128 value) {
            return ConvertSignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(System.Int128 value) {
            return CreateNewUnchecked(unchecked((BigInteger)value));
        }

        /// <summary>Implicitly converts a <see cref="IntPtr" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator checked UInt<TBitSize>(nint value) {
            return ConvertSignedCheckedFrom(value);
        }

        public static explicit operator/* unchecked*/ UInt<TBitSize>(nint value) {
            return CreateNewUnchecked(unchecked((BigInteger)value));
        }

        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(sbyte value) {
            return ConvertSignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(sbyte value) {
            return CreateNewUnchecked(BigInteger.CreateTruncating(value));
        }

        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(ushort value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(ushort value) {
            var t = BigInteger.CreateTruncating(value);
            if (BitSize >= Unsafe.SizeOf<ushort>() * 8) {
                return new UInt<TBitSize>(t, default(Plain));
            }
            return CreateNewUnchecked(t);
        }

        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(uint value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(uint value) {
            var t = new BigInteger(value);
            if (BitSize >= Unsafe.SizeOf<uint>() * 8) {
                return new UInt<TBitSize>(t, default(Plain));
            }
            return CreateNewUnchecked(t);
        }

        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(ulong value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(ulong value) {
            var t = new BigInteger(value);
            if (BitSize >= Unsafe.SizeOf<uint>() * 8) {
                return new UInt<TBitSize>(t, default(Plain));
            }
            return CreateNewUnchecked(t);
        }

        /// <summary>Implicitly converts a <see cref="UInt128" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(UltimateOrb.UInt128 value) {
            return checked((UInt<TBitSize>)(System.UInt128)value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(UltimateOrb.UInt128 value) {
            return unchecked((UInt<TBitSize>)(System.UInt128)value);
        }

        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(System.UInt128 value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(System.UInt128 value) {
            return CreateNewUnchecked(unchecked((BigInteger)value));
        }

        /// <summary>Implicitly converts a <see cref="UIntPtr" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        [CLSCompliant(false)]
        public static explicit operator checked UInt<TBitSize>(nuint value) {
            return ConvertUnsignedCheckedFrom(value);
        }

        [CLSCompliant(false)]
        public static explicit operator/* unchecked*/ UInt<TBitSize>(nuint value) {
            var t = new BigInteger(value);
            if (BitSize >= Unsafe.SizeOf<nuint>() * 8) {
                return new UInt<TBitSize>(t, default(Plain));
            }
            return CreateNewUnchecked(t);
        }

        public static UInt<TBitSize> operator &(UInt<TBitSize> left, UInt<TBitSize> right) {
            return new UInt<TBitSize>(left.Value & right.Value, default(Plain));
        }

        public static UInt<TBitSize> operator |(UInt<TBitSize> left, UInt<TBitSize> right) {
            return new UInt<TBitSize>(left.Value | right.Value, default(Plain));
        }

        public static UInt<TBitSize> operator ^(UInt<TBitSize> left, UInt<TBitSize> right) {
            return CreateNewUnchecked(left.Value ^ right.Value);
        }

        public static UInt<TBitSize> operator <<(UInt<TBitSize> value, int shiftCount) {
            var c = TXIntInternal<TBitSize>.NormalizeShiftCount(shiftCount);
            return CreateNewUnchecked(value.Value << c);
        }

        public static UInt<TBitSize> operator >>(UInt<TBitSize> value, int shiftCount) {
            var c = TXIntInternal<TBitSize>.NormalizeShiftCount(shiftCount);
            return new UInt<TBitSize>(value.Value >> c, default(Plain));
        }

        public static UInt<TBitSize> operator ~(UInt<TBitSize> value) {
            return CreateNewUnchecked(~value.Value);
        }

        public static UInt<TBitSize> operator checked -(UInt<TBitSize> value) {
            return CreateNewChecked(-value.Value);
        }

        public static UInt<TBitSize> operator -(UInt<TBitSize> value) {
            return CreateNewUnchecked(-value.Value);
        }

        public static UInt<TBitSize> operator +(UInt<TBitSize> value) {
            return value;
        }

        public static UInt<TBitSize> operator checked ++(UInt<TBitSize> value) {
            return CreateNewChecked(value.Value + BigInteger.One);
        }

        public static UInt<TBitSize> operator ++(UInt<TBitSize> value) {
            return CreateNewUnchecked(value.Value + BigInteger.One);
        }

        public static UInt<TBitSize> operator checked --(UInt<TBitSize> value) {
            return CreateNewChecked(value.Value - BigInteger.One);
        }

        public static UInt<TBitSize> operator --(UInt<TBitSize> value) {
            return CreateNewUnchecked(value.Value - BigInteger.One);
        }


        public static UInt<TBitSize> operator checked +(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewChecked(first.Value + second.Value);
        }

        public static UInt<TBitSize> operator +(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewUnchecked(first.Value + second.Value);
        }

        public static UInt<TBitSize> operator *(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewUnchecked(first.Value * second.Value);
        }

        public static UInt<TBitSize> operator /(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewChecked(first.Value / second.Value);
        }

        public static UInt<TBitSize> operator %(UInt<TBitSize> first, UInt<TBitSize> second) {
            return CreateNewChecked(first.Value % second.Value);
        }

        public static bool operator <(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.CompareTo(right) > 0;
        }
        public static bool operator >=(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(UInt<TBitSize> left, UInt<TBitSize> right) {
            return left.Equals(right);
        }

        public static bool operator !=(UInt<TBitSize> left, UInt<TBitSize> right) {
            return !left.Equals(right);
        }

        public static bool operator <(UInt<TBitSize> left, long right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UInt<TBitSize> left, long right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(UInt<TBitSize> left, long right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UInt<TBitSize> left, long right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(UInt<TBitSize> left, long right) {
            return left.Equals(right);
        }

        public static bool operator !=(UInt<TBitSize> left, long right) {
            return !left.Equals(right);
        }

        public static bool operator <(long left, UInt<TBitSize> right) {
            return right.CompareTo(left) > 0;
        }

        public static bool operator <=(long left, UInt<TBitSize> right) {
            return right.CompareTo(left) >= 0;
        }

        public static bool operator >(long left, UInt<TBitSize> right) {
            return right.CompareTo(left) < 0;
        }

        public static bool operator >=(long left, UInt<TBitSize> right) {
            return right.CompareTo(left) <= 0;
        }

        public static bool operator ==(long left, UInt<TBitSize> right) {
            return right.Equals(left);
        }

        public static bool operator !=(long left, UInt<TBitSize> right) {
            return !right.Equals(left);
        }

        [CLSCompliant(false)]
        public static bool operator <(UInt<TBitSize> left, ulong right) {
            return left.CompareTo(right) < 0;
        }

        [CLSCompliant(false)]
        public static bool operator <=(UInt<TBitSize> left, ulong right) {
            return left.CompareTo(right) <= 0;
        }

        [CLSCompliant(false)]
        public static bool operator >(UInt<TBitSize> left, ulong right) {
            return left.CompareTo(right) > 0;
        }

        [CLSCompliant(false)]
        public static bool operator >=(UInt<TBitSize> left, ulong right) {
            return left.CompareTo(right) >= 0;
        }

        [CLSCompliant(false)]
        public static bool operator ==(UInt<TBitSize> left, ulong right) {
            return left.Equals(right);
        }

        [CLSCompliant(false)]
        public static bool operator !=(UInt<TBitSize> left, ulong right) {
            return !left.Equals(right);
        }

        [CLSCompliant(false)]
        public static bool operator <(ulong left, UInt<TBitSize> right) {
            return right.CompareTo(left) > 0;
        }

        [CLSCompliant(false)]
        public static bool operator <=(ulong left, UInt<TBitSize> right) {
            return right.CompareTo(left) >= 0;
        }

        [CLSCompliant(false)]
        public static bool operator >(ulong left, UInt<TBitSize> right) {
            return right.CompareTo(left) < 0;
        }

        [CLSCompliant(false)]
        public static bool operator >=(ulong left, UInt<TBitSize> right) {
            return right.CompareTo(left) <= 0;
        }

        [CLSCompliant(false)]
        public static bool operator ==(ulong left, UInt<TBitSize> right) {
            return right.Equals(left);
        }

        [CLSCompliant(false)]
        public static bool operator !=(ulong left, UInt<TBitSize> right) {
            return !right.Equals(left);
        }

        /// <summary>
        /// Gets the number of bits required for shortest two's complement representation of the current instance without the sign bit.
        /// </summary>
        /// <returns>The minimum non-negative number of bits in two's complement notation without the sign bit.</returns>
        /// <remarks>This method returns 0 iff the value of current object is equal to <see cref="Zero"/> or <see cref="MinusOne"/>. For positive integers the return value is equal to the ordinary binary representation string length.</remarks>
        public int GetBitLength() {
            AssertValid();
            return int.CreateTruncating(Value.GetBitLength());
        }

        [Conditional("DEBUG")]
        private void AssertValid() {
            Debug.Assert(MinValueAsBigInteger <= Value && Value <= MaxValueAsBigInteger);
        }

        //
        // IAdditiveIdentity
        //

        /// <inheritdoc cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity" />
        static UInt<TBitSize> IAdditiveIdentity<UInt<TBitSize>, UInt<TBitSize>>.AdditiveIdentity => Zero;

        //
        // IBinaryInteger
        //

        /// <inheritdoc cref="IBinaryInteger{TSelf}.DivRem(TSelf, TSelf)" />
        public static (UInt<TBitSize> Quotient, UInt<TBitSize> Remainder) DivRem(UInt<TBitSize> left, UInt<TBitSize> right) {
            UInt<TBitSize> quotient = DivRem(left, right, out UInt<TBitSize> remainder);
            return (quotient, remainder);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.LeadingZeroCount(TSelf)" />
        public static int LeadingZeroCount(UInt<TBitSize> value) {
            value.AssertValid();
            var sign = value.Value.GetSignField();
            var bits = value.Value.GetBitsField();
            if (bits is null) {
                return unchecked(BitSize - 32 + Int32.LeadingZeroCount(sign));
            }
            return unchecked(BitSize - 32 * bits.Length + Int32.LeadingZeroCount(unchecked((Int32)bits[^1])));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.LeadingZeroCount(TSelf)" />
        static UInt<TBitSize> IBinaryInteger<UInt<TBitSize>>.LeadingZeroCount(UInt<TBitSize> value) {
            return new UInt<TBitSize>(LeadingZeroCount(value), default(Checked));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.PopCount(TSelf)" />
        public static int PopCount(UInt<TBitSize> value) {
            value.AssertValid();
            var sign = value.Value.GetSignField();
            UInt32[] bits = value.Value.GetBitsField();
            if (bits is null) {
                return int.PopCount(sign);
            }
            int result = 0;
            for (int i = 0; i < bits.Length; i++) {
                unchecked {
                    result += int.PopCount(unchecked((Int32)bits[i]));
                }
            }
            return result;
        }

        static UInt<TBitSize> IBinaryInteger<UInt<TBitSize>>.PopCount(UInt<TBitSize> value) {
            value.AssertValid();
            return new UInt<TBitSize>(unchecked((uint)PopCount(value)), default(Plain));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateLeft(TSelf, int)" />
        public static UInt<TBitSize> RotateLeft(UInt<TBitSize> value, int rotateAmount) {
            value.AssertValid();
            var c = TXIntInternal<TBitSize>.NormalizeShiftCount(rotateAmount);
            return CreateNewUnchecked((((value.Value << BitSize) | value.Value) << c) >> BitSize);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateRight(TSelf, int)" />
        public static UInt<TBitSize> RotateRight(UInt<TBitSize> value, int rotateAmount) {
            value.AssertValid();
            var c = TXIntInternal<TBitSize>.NormalizeShiftCount(rotateAmount);
            return CreateNewUnchecked((((value.Value << BitSize) | value.Value) >>> c));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TrailingZeroCount(TSelf)" />
        public static int TrailingZeroCount(UInt<TBitSize> value) {
            value.AssertValid();
            var sign = value.Value.GetSignField();
            UInt32[] bits = value.Value.GetBitsField();
            if (sign == 0) {
                return BitSize;
            }
            if (bits is null) {
                return int.TrailingZeroCount(sign);
            }
            int result = 0;

            // Both positive values and their two's-complement negative representation will share the same TrailingZeroCount,
            // so the sign of value does not matter and both cases can be handled in the same way

            UInt32 part = bits[0];

            for (int i = 1; (part == 0) && (i < bits.Length); i++) {
                part = bits[i];
                unchecked {
                    result += 32;
                }
            }

            unchecked {
                result += Int32.TrailingZeroCount(part.ToSignedUnchecked());
            }

            return result;
        }

        static UInt<TBitSize> IBinaryInteger<UInt<TBitSize>>.TrailingZeroCount(UInt<TBitSize> value) {
            return new UInt<TBitSize>(unchecked((uint)TrailingZeroCount(value)), default(Plain));
        }

        private static bool TryCreate(BigInteger t, out UInt<TBitSize> value) {
            if (!BigInteger.IsNegative(t) && t.GetBitLength() < BitSize) {
                value = new UInt<TBitSize>(t, default(Plain));
                return true;
            } else {
                value = default;
                return false;
            }
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadBigEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<UInt<TBitSize>>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt<TBitSize> value) {
            // TODO: This type is considered a fixed-size integer type, so we should use a fixed-size read here.
            var t = new BigInteger(source, isUnsigned, isBigEndian: true);
            return TryCreate(t, out value);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadLittleEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<UInt<TBitSize>>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out UInt<TBitSize> value) {
            // TODO: This type is considered a fixed-size integer type, so we should use a fixed-size read here.
            var t = new BigInteger(source, isUnsigned, isBigEndian: false);
            return TryCreate(t, out value);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetShortestBitLength()" />
        int IBinaryInteger<UInt<TBitSize>>.GetShortestBitLength() {
            AssertValid();
            uint[]? bits = Value.GetBitsField();
            var _sign = Value.GetSignField();
            if (bits is null) {
                int value = _sign;

                if (value >= 0) {
                    return (sizeof(int) * 8) - BitOperations.LeadingZeroCount((uint)(value));
                } else {
                    return (sizeof(int) * 8) + 1 - BitOperations.LeadingZeroCount((uint)(~value));
                }
            }

            int result = (bits.Length - 1) * 32;

            if (_sign >= 0) {
                result += (sizeof(uint) * 8) - BitOperations.LeadingZeroCount(bits[^1]);
            } else {
                uint part = ~bits[^1] + 1;

                // We need to remove the "carry" (the +1) if any of the initial
                // bytes are not zero. This ensures we get the correct two's complement
                // part for the computation.

                for (int index = 0; index < bits.Length - 1; index++) {
                    if (bits[index] != 0) {
                        part -= 1;
                        break;
                    }
                }

                result += (sizeof(uint) * 8) + 1 - BitOperations.LeadingZeroCount(~part);
            }

            return result;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetByteCount()" />
        int IBinaryInteger<UInt<TBitSize>>.GetByteCount() => GetGenericMathByteCount();

        private static int GetGenericMathByteCount() {
            return (BitSize >> 3) + ((0 != (7 & BitSize)) ? 1 : 0);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteBigEndian(Span{byte}, out int)" />
        bool IBinaryInteger<UInt<TBitSize>>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            AssertValid();
            UInt32[]? bits = Value.GetBitsField();
            var _sign = Value.GetSignField();

            int byteCount = GetGenericMathByteCount();

            if (destination.Length >= byteCount) {
                if (bits is null) {
                    for (int i = 0; i < byteCount; i++) {
                        destination[i] = (byte)(_sign >> (8 * (byteCount - 1 - i)));
                    }
                } else {
                    Debug.Assert(_sign >= 0);
                    ref byte destRef = ref MemoryMarshal.GetReference(destination);
                    int totalWords = bits.Length;
                    int fullWords = byteCount / 4;
                    int extra = byteCount % 4;

                    int index = totalWords - 1;
                    if (extra > 0) {
                        uint word = bits[index];
                        for (int i = 0; i < extra; i++) {
                            destRef = (byte)(word >> (24 - 8 * i));
                            destRef = ref Unsafe.Add(ref destRef, 1);
                        }
                        index--;
                    }

                    for (; index >= 0; index--) {
                        BinaryPrimitives.WriteUInt32BigEndian(MemoryMarshal.CreateSpan(ref destRef, 4), bits[index]);
                        destRef = ref Unsafe.Add(ref destRef, 4);
                    }
                }
                bytesWritten = byteCount;
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteLittleEndian(Span{byte}, out int)" />
        bool IBinaryInteger<UInt<TBitSize>>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            AssertValid();
            UInt32[]? bits = Value.GetBitsField();
            var _sign = Value.GetSignField();

            int byteCount = (BitSize >> 3) + ((0 != (7 & BitSize)) ? 1 : 0);

            if (destination.Length >= byteCount) {
                if (bits is null) {
                    for (int i = 0; i < byteCount; i++) {
                        destination[i] = (byte)(_sign >> (i * 8));
                    }
                } else {
                    Debug.Assert(_sign >= 0);
                    ref byte destRef = ref MemoryMarshal.GetReference(destination);
                    int fullWords = byteCount / 4;
                    Debug.Assert(fullWords <= bits.Length);
                    int extra = byteCount % 4;

                    for (int i = 0; i < fullWords; i++) {
                        uint part = bits[i];
                        if (!BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }
                        Unsafe.WriteUnaligned(ref destRef, part);
                        destRef = ref Unsafe.Add(ref destRef, 4);
                    }

                    if (extra > 0) {
                        uint last = bits[fullWords];
                        for (int j = 0; j < extra; j++) {
                            destRef = (byte)(last >> (j * 8));
                            destRef = ref Unsafe.Add(ref destRef, 1);
                        }
                    }
                }
                bytesWritten = byteCount;
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        //
        // IBinaryNumber
        //

        /// <inheritdoc cref="IBinaryNumber{TSelf}.AllBitsSet" />
        static UInt<TBitSize> IBinaryNumber<UInt<TBitSize>>.AllBitsSet => MaxValue;

        /// <inheritdoc cref="IBinaryNumber{TSelf}.IsPow2(TSelf)" />
        public static bool IsPow2(UInt<TBitSize> value) => value.Value.IsPowerOfTwo;

        /// <inheritdoc cref="IBinaryNumber{TSelf}.Log2(TSelf)" />
        public static int Log2(UInt<TBitSize> value) {
            value.AssertValid();
            UInt32[]? bits = value.Value.GetBitsField();
            Int32 sign = value.Value.GetSignField();

            if (bits is null) {
                return 31 ^ Int32.LeadingZeroCount(sign | 1);
            }

            return ((bits.Length * 32) - 1) ^ Int32.LeadingZeroCount(bits[^1].ToSignedUnchecked());
        }

        static UInt<TBitSize> IBinaryNumber<UInt<TBitSize>>.Log2(UInt<TBitSize> value) {
            return new UInt<TBitSize>(unchecked((uint)Log2(value)), default(Plain));
        }

        //
        // IMultiplicativeIdentity
        //

        /// <inheritdoc cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity" />
        static UInt<TBitSize> IMultiplicativeIdentity<UInt<TBitSize>, UInt<TBitSize>>.MultiplicativeIdentity => One;

        //
        // INumber
        //

        /// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)" />
        public static UInt<TBitSize> Clamp(UInt<TBitSize> value, UInt<TBitSize> min, UInt<TBitSize> max) {
            return new UInt<TBitSize>(BigInteger.Clamp(value.Value, min.Value, max.Value), default(Plain));
        }

        /// <inheritdoc cref="INumber{TSelf}.CopySign(TSelf, TSelf)" />
        public static UInt<TBitSize> CopySign(UInt<TBitSize> value, UInt<TBitSize> sign) {
            value.AssertValid();
            sign.AssertValid();
            return value;
        }

        /// <inheritdoc cref="INumber{TSelf}.MaxNumber(TSelf, TSelf)" />
        static UInt<TBitSize> INumber<UInt<TBitSize>>.MaxNumber(UInt<TBitSize> x, UInt<TBitSize> y) => Max(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MinNumber(TSelf, TSelf)" />
        static UInt<TBitSize> INumber<UInt<TBitSize>>.MinNumber(UInt<TBitSize> x, UInt<TBitSize> y) => Min(x, y);

        /// <inheritdoc cref="INumber{TSelf}.Sign(TSelf)" />
        static int INumber<UInt<TBitSize>>.Sign(UInt<TBitSize> value) {
            value.AssertValid();
            return value.IsZero ? 0 : 1;
        }

        //
        // INumberBase
        //

        /// <inheritdoc cref="INumberBase{TSelf}.Radix" />
        static int INumberBase<UInt<TBitSize>>.Radix => 2;

        /// <inheritdoc cref="INumberBase{TSelf}.CreateChecked{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            UInt<TBitSize> result;

            if (typeof(TOther) == typeof(UInt<TBitSize>)) {
                result = (UInt<TBitSize>)(object)value;
            } else if (!TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateSaturating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TBitSize> CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            UInt<TBitSize> result;

            if (typeof(TOther) == typeof(UInt<TBitSize>)) {
                result = (UInt<TBitSize>)(object)value;
            } else if (!TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateTruncating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt<TBitSize> CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            UInt<TBitSize> result;

            if (typeof(TOther) == typeof(UInt<TBitSize>)) {
                result = (UInt<TBitSize>)(object)value;
            } else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsCanonical(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsCanonical(UInt<TBitSize> value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsComplexNumber(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsComplexNumber(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsEvenInteger(TSelf)" />
        public static bool IsEvenInteger(UInt<TBitSize> value) {
            return value.Value.IsEven;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsFinite(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsFinite(UInt<TBitSize> value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsImaginaryNumber(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsImaginaryNumber(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInfinity(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsInfinity(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInteger(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsInteger(UInt<TBitSize> value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNaN(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsNaN(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegative(TSelf)" />
        public static bool IsNegative(UInt<TBitSize> value) {
            return BigInteger.IsNegative(value.Value);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegativeInfinity(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsNegativeInfinity(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNormal(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsNormal(UInt<TBitSize> value) => !value.IsZero;

        /// <inheritdoc cref="INumberBase{TSelf}.IsOddInteger(TSelf)" />
        public static bool IsOddInteger(UInt<TBitSize> value) {
            return !value.Value.IsEven;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositive(TSelf)" />
        public static bool IsPositive(UInt<TBitSize> value) {
            return BigInteger.IsPositive(value.Value);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositiveInfinity(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsPositiveInfinity(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsRealNumber(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsRealNumber(UInt<TBitSize> value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsSubnormal(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsSubnormal(UInt<TBitSize> value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsZero(TSelf)" />
        static bool INumberBase<UInt<TBitSize>>.IsZero(UInt<TBitSize> value) {
            return value.IsZero;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitude(TSelf, TSelf)" />
        public static UInt<TBitSize> MaxMagnitude(UInt<TBitSize> x, UInt<TBitSize> y) {
            x.AssertValid();
            y.AssertValid();

            BigInteger ax = Abs(x);
            BigInteger ay = Abs(y);

            if (ax > ay) {
                return x;
            }

            if (ax == ay) {
                return IsNegative(x) ? y : x;
            }

            return y;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitudeNumber(TSelf, TSelf)" />
        static UInt<TBitSize> INumberBase<UInt<TBitSize>>.MaxMagnitudeNumber(UInt<TBitSize> x, UInt<TBitSize> y) => MaxMagnitude(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitude(TSelf, TSelf)" />
        public static UInt<TBitSize> MinMagnitude(UInt<TBitSize> x, UInt<TBitSize> y) {
            x.AssertValid();
            y.AssertValid();

            BigInteger ax = Abs(x);
            BigInteger ay = Abs(y);

            if (ax < ay) {
                return x;
            }

            if (ax == ay) {
                return IsNegative(x) ? x : y;
            }

            return y;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitudeNumber(TSelf, TSelf)" />
        static UInt<TBitSize> INumberBase<UInt<TBitSize>>.MinMagnitudeNumber(UInt<TBitSize> x, UInt<TBitSize> y) => MinMagnitude(x, y);

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="INumberBase{TSelf}.MultiplyAddEstimate(TSelf, TSelf, TSelf)" />
        static UInt<TBitSize> INumberBase<UInt<TBitSize>>.MultiplyAddEstimate(UInt<TBitSize> left, UInt<TBitSize> right, UInt<TBitSize> addend) => CreateNewChecked((left.Value * right.Value) + addend.Value);
#endif

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt<TBitSize>>.TryConvertFromChecked<TOther>(TOther value, out UInt<TBitSize> result) => TryConvertFromChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out UInt<TBitSize> result)
            where TOther : INumberBase<TOther> {
            BigInteger i;
            if (typeof(TOther) == typeof(BigInteger)) {
                i = (BigInteger)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther) == typeof(Int<TBitSize>)) {
                i = (Int<TBitSize>)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther).IsAssignableTo(typeof(IUIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther).IsAssignableTo(typeof(IIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(byte)) {
                result = unchecked((UInt<TBitSize>)(byte)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                result = unchecked((UInt<TBitSize>)(char)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                result = unchecked((UInt<TBitSize>)(decimal)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(Half)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(short)) {
                result = unchecked((UInt<TBitSize>)(short)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                result = unchecked((UInt<TBitSize>)(int)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                result = unchecked((UInt<TBitSize>)(long)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                result = unchecked((UInt<TBitSize>)(UltimateOrb.Int128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                result = unchecked((UInt<TBitSize>)(System.Int128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                result = unchecked((UInt<TBitSize>)(nint)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(Complex)) {
                if (INumberBaseEx<double>.TryConvertToTruncating(((Complex)(object)value).Real, out i)) {
                    goto L_CreateNew;
                }
                result = default;
                return false;
            } else if (typeof(TOther) == typeof(sbyte)) {
                result = unchecked((UInt<TBitSize>)(sbyte)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(ushort)) {
                result = unchecked((UInt<TBitSize>)(ushort)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                result = unchecked((UInt<TBitSize>)(uint)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                result = unchecked((UInt<TBitSize>)(ulong)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                result = unchecked((UInt<TBitSize>)(UltimateOrb.UInt128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                result = unchecked((UInt<TBitSize>)(System.UInt128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                result = unchecked((UInt<TBitSize>)(nuint)(object)value);
                return true;
            } else {
                result = default;
                return false;
            }
        L_CreateNew:;
            result = CreateNewUnchecked(i);
            return true;
        L_TryBigInt:;
            if (INumberBaseEx<TOther>.TryConvertToTruncating(value, out i)) {
                goto L_CreateNew;
            }
            result = default;
            return false;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt<TBitSize>>.TryConvertFromSaturating<TOther>(TOther value, out UInt<TBitSize> result) => TryConvertFromSaturating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt<TBitSize> result)
            where TOther : INumberBase<TOther> {
            BigInteger i;
            if (typeof(TOther) == typeof(BigInteger)) {
                i = (BigInteger)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther) == typeof(Int<TBitSize>)) {
                i = (Int<TBitSize>)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther).IsAssignableTo(typeof(IUIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther).IsAssignableTo(typeof(IIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(byte)) {
                if (BitSize >= Unsafe.SizeOf<byte>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((byte)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(char)) {
                if (BitSize >= Unsafe.SizeOf<char>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((char)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(decimal)) {
                if (BitSize >= 96) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((decimal)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(double)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(Half)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(short)) {
                if (BitSize >= Unsafe.SizeOf<short>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((short)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(int)) {
                if (BitSize >= Unsafe.SizeOf<int>() * 8 - 1) {
                    if (!TOther.IsNegative(value)) {
                        var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((int)(object)value, out i);
                        Debug.Assert(s);
                        goto L_CreateNewRaw;
                    }
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(long)) {
                if (BitSize >= Unsafe.SizeOf<long>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((long)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                if (BitSize >= Unsafe.SizeOf<UltimateOrb.Int128>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((System.Int128)(UltimateOrb.Int128)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                if (BitSize >= Unsafe.SizeOf<System.Int128>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((System.Int128)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(nint)) {
                if (BitSize >= Unsafe.SizeOf<nint>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((nint)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(Complex)) {
                if (INumberBaseEx<double>.TryConvertToSaturating(((Complex)(object)value).Real, out i)) {
                    goto L_CreateNew;
                }
                result = default;
                return false;
            } else if (typeof(TOther) == typeof(sbyte)) {
                if (BitSize >= Unsafe.SizeOf<sbyte>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((sbyte)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(float)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(ushort)) {
                if (BitSize >= Unsafe.SizeOf<ushort>() * 8 - 1) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((ushort)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(uint)) {
                if (BitSize >= Unsafe.SizeOf<uint>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((uint)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(ulong)) {
                if (BitSize >= Unsafe.SizeOf<ulong>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((ulong)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                if (BitSize >= Unsafe.SizeOf<UltimateOrb.UInt128>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((System.UInt128)(UltimateOrb.UInt128)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                if (BitSize >= Unsafe.SizeOf<System.UInt128>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((System.UInt128)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(nuint)) {
                if (BitSize >= Unsafe.SizeOf<nuint>() * 8) {
                    var s = INumberBaseEx<BigInteger>.TryConvertFromTruncating((nuint)(object)value, out i);
                    Debug.Assert(s);
                    goto L_CreateNewRaw;
                }
                goto L_TryBigInt;
            } else {
                result = default;
                return false;
            }
        L_CreateNew:;
            result = CreateNewSaturated(i);
            return true;
        L_CreateNewRaw:;
            result = new UInt<TBitSize>(i, default(Plain));
            return true;
        L_TryBigInt:;
            if (INumberBaseEx<TOther>.TryConvertToSaturating(value, out i)) {
                goto L_CreateNew;
            }
            result = default;
            return false;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt<TBitSize>>.TryConvertFromTruncating<TOther>(TOther value, out UInt<TBitSize> result) => TryConvertFromTruncating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt<TBitSize> result)
            where TOther : INumberBase<TOther> {
            BigInteger i;
            if (typeof(TOther) == typeof(BigInteger)) {
                i = (BigInteger)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther) == typeof(Int<TBitSize>)) {
                i = (Int<TBitSize>)(object)value;
                goto L_CreateNew;
            } else if (typeof(TOther).IsAssignableTo(typeof(IUIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther).IsAssignableTo(typeof(IIntX))) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(byte)) {
                result = unchecked((UInt<TBitSize>)(byte)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                result = unchecked((UInt<TBitSize>)(char)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                result = unchecked((UInt<TBitSize>)(decimal)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(Half)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(short)) {
                result = unchecked((UInt<TBitSize>)(short)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                result = unchecked((UInt<TBitSize>)(int)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                result = unchecked((UInt<TBitSize>)(long)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                result = unchecked((UInt<TBitSize>)(UltimateOrb.Int128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                result = unchecked((UInt<TBitSize>)(System.Int128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                result = unchecked((UInt<TBitSize>)(nint)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(Complex)) {
                if (INumberBaseEx<double>.TryConvertToTruncating(((Complex)(object)value).Real, out i)) {
                    goto L_CreateNew;
                }
                result = default;
                return false;
            } else if (typeof(TOther) == typeof(sbyte)) {
                result = unchecked((UInt<TBitSize>)(sbyte)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                goto L_TryBigInt;
            } else if (typeof(TOther) == typeof(ushort)) {
                result = unchecked((UInt<TBitSize>)(ushort)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                result = unchecked((UInt<TBitSize>)(uint)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                result = unchecked((UInt<TBitSize>)(ulong)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                result = unchecked((UInt<TBitSize>)(UltimateOrb.UInt128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                result = unchecked((UInt<TBitSize>)(System.UInt128)(object)value);
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                result = unchecked((UInt<TBitSize>)(nuint)(object)value);
                return true;
            } else {
                result = default;
                return false;
            }
        L_CreateNew:;
            result = CreateNewUnchecked(i);
            return true;
        L_TryBigInt:;
            if (INumberBaseEx<TOther>.TryConvertToTruncating(value, out i)) {
                goto L_CreateNew;
            }
            result = default;
            return false;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt<TBitSize>>.TryConvertToChecked<TOther>(UInt<TBitSize> value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(byte)) {
                byte actualResult = checked((byte)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult = checked((char)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualResult = checked((decimal)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult = checked((long)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult = checked((Int128)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualResult = checked((nint)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Complex)) {
                Complex actualResult = (Complex)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualResult = checked((float)value);
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
        static bool INumberBase<UInt<TBitSize>>.TryConvertToSaturating<TOther>(UInt<TBitSize> value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(byte)) {
                byte actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? byte.MinValue : byte.MaxValue;
                } else {
                    actualResult = (value._sign >= byte.MaxValue) ? byte.MaxValue :
                                   (value._sign <= byte.MinValue) ? byte.MinValue : (byte)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? char.MinValue : char.MaxValue;
                } else {
                    actualResult = (value._sign >= char.MaxValue) ? char.MaxValue :
                                   (value._sign <= char.MinValue) ? char.MinValue : (char)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualResult = (value >= new Int128(0x0000_0000_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? decimal.MaxValue :
                                       (value <= new Int128(0xFFFF_FFFF_0000_0000, 0x0000_0000_0000_0001)) ? decimal.MinValue : (decimal)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? short.MinValue : short.MaxValue;
                } else {
                    actualResult = (value._sign >= short.MaxValue) ? short.MaxValue :
                                   (value._sign <= short.MinValue) ? short.MinValue : (short)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? int.MinValue : int.MaxValue;
                } else {
                    actualResult = (value._sign >= int.MaxValue) ? int.MaxValue :
                                   (value._sign <= int.MinValue) ? int.MinValue : (int)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult = (value >= long.MaxValue) ? long.MaxValue :
                                    (value <= long.MinValue) ? long.MinValue : (long)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult = (value >= Int128.MaxValue) ? Int128.MaxValue :
                                      (value <= Int128.MinValue) ? Int128.MinValue : (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualResult = (value >= nint.MaxValue) ? nint.MaxValue :
                                    (value <= nint.MinValue) ? nint.MinValue : (nint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Complex)) {
                Complex actualResult = (Complex)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? sbyte.MinValue : sbyte.MaxValue;
                } else {
                    actualResult = (value._sign >= sbyte.MaxValue) ? sbyte.MaxValue :
                                   (value._sign <= sbyte.MinValue) ? sbyte.MinValue : (sbyte)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? ushort.MinValue : ushort.MaxValue;
                } else {
                    actualResult = (value._sign >= ushort.MaxValue) ? ushort.MaxValue :
                                   (value._sign <= ushort.MinValue) ? ushort.MinValue : (ushort)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult = (value >= uint.MaxValue) ? uint.MaxValue :
                                    IsNegative(value) ? uint.MinValue : (uint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult = (value >= ulong.MaxValue) ? ulong.MaxValue :
                                     IsNegative(value) ? ulong.MinValue : (ulong)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualResult = (value >= UInt128.MaxValue) ? UInt128.MaxValue :
                                       IsNegative(value) ? UInt128.MinValue : (UInt128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualResult = (value >= nuint.MaxValue) ? nuint.MaxValue :
                                     IsNegative(value) ? nuint.MinValue : (nuint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToTruncating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt<TBitSize>>.TryConvertToTruncating<TOther>(UInt<TBitSize> value, [MaybeNullWhen(false)] out TOther result) {
            if (INumberBase<BigInteger>.TryConvertToTruncating<TOther>(value.Value, out var t)) {
                result = t;
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int256)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt256)) {
                goto L_D;
            }


            /*
            if (typeof(TOther) == typeof(byte)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(char)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(decimal)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(double)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(Half)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(short)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(int)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(long)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(Int128)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(nint)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(Complex)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(sbyte)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(float)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(ushort)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(uint)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(ulong)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(UInt128)) {
                goto L_D;
            } else if (typeof(TOther) == typeof(nuint)) {
                goto L_D;
            } else {
                result = default;
                return false;
            }
            */
        }

        //
        // IParsable
        //

        /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out UInt<TBitSize> result) => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        /// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_UnsignedRightShift(TSelf, TOther)" />
        public static UInt<TBitSize> operator >>>(UInt<TBitSize> value, int shiftCount) {
            return value >> shiftCount;
        }

        //
        // ISpanParsable
        //

        /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
        public static UInt<TBitSize> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Integer, provider);

        /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out UInt<TBitSize> result) {
            if (TryParse(s, NumberStyles.Integer, provider, out var t)) {
                if (!BigInteger.IsNegative(t) && t.GetBitLength() < BitSize) {
                    result = new UInt<TBitSize>(t, default(Plain));
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}
