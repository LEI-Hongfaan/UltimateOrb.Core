using System;
using System.Buffers.Binary;
using System.Buffers;
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
using Environment = System.Environment;
using System.Threading;
//using System.Numerics.Tensors;
using UltimateOrb.Numerics.Generic;
using static UltimateOrb.Miscellaneous;

#if AAAA
namespace UltimateOrb.Numerics.BigIntegerWrappers {

    [Serializable]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public readonly struct BigUInteger :
        ISpanFormattable,
        IComparable,
        IComparable<BigUInteger>,
        IEquatable<BigUInteger>,
        IBinaryInteger<BigUInteger>,
        IUnsignedNumber<BigUInteger> {

        internal readonly BigInteger m_value;

        // We have to make a choice of how to represent int.MinValue. This is the one
        // value that fits in an int, but whose negation does not fit in an int.
        // We choose to use a large representation, so we're symmetric with respect to negation.
        private static readonly BigUInteger s_bnMinInt = new BigUInteger(new BigInteger(int.MinValue), default(ConstructorTags.Unchecked) );
        private static readonly BigUInteger s_bnOneInt = new BigUInteger(BigInteger.One, default(ConstructorTags.Unchecked));
        private static readonly BigUInteger s_bnZeroInt = new BigUInteger(BigInteger.Zero, default(ConstructorTags.Unchecked));
        private static readonly BigUInteger s_bnMinusOneInt = new BigUInteger(BigInteger.MinusOne, default(ConstructorTags.Unchecked));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigUInteger(BigInteger value, ConstructorTags.Unchecked ignored) {
            m_value = ignored.Comma(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigUInteger(BigInteger value, ConstructorTags.Checked ignored) : this(value) {
        }

        public BigUInteger(int value) {
            var v = checked((uint)value);
            m_value = new(v);
        }

        [CLSCompliant(false)]
        public BigUInteger(uint value) {
            m_value = new(value);
        }

        public BigUInteger(long value) {
            var v = checked((ulong)value);
            m_value = new(v);
        }

        [CLSCompliant(false)]
        public BigUInteger(ulong value) {
            m_value = new(value);

        }
        public BigUInteger(float value) : this((double)value) {
        }

        public BigUInteger(double value) : this(new BigInteger(value)) {
        }

        public BigUInteger(BigInteger value) {
            m_value = BigInteger.IsNegative(value) ? checked((uint)value.Sign).Comma(value) : value;
        }

        public BigUInteger(decimal value) : this(new BigInteger(value)) {
        }

        /// <summary>
        /// Creates a BigInteger from a little-endian twos-complement byte array.
        /// </summary>
        /// <param name="value"></param>
        [CLSCompliant(false)]
        public BigUInteger(byte[] value) : this(new BigInteger(value)) {
        }

        public BigUInteger(ReadOnlySpan<byte> value, bool isBigEndian = false) : this(new BigInteger(value, isUnsigned: true, isBigEndian), default(ConstructorTags.Unchecked)) {
        }

        public static BigUInteger Zero { get { return s_bnZeroInt; } }

        public static BigUInteger One { get { return s_bnOneInt; } }

        public static BigUInteger AllBitsSet { get { return s_bnMinusOneInt; } }

        public bool IsPowerOfTwo {
            get {
                return m_value.IsPowerOfTwo;
            }
        }

        public bool IsZero { get { return m_value.IsZero; } }

        public bool IsOne { get { return m_value.IsOne; } }

        public bool IsEven { get { return m_value.IsEven; } }

        public int Sign {
            get { return m_value.Sign; }
        }

        public static BigUInteger Parse(string value) {
            return new BigUInteger(BigInteger.Parse(value));
        }

        public static BigUInteger Parse(string value, NumberStyles style) {
            return new BigUInteger(BigInteger.Parse(value, style));
        }

        public static BigUInteger Parse(string value, IFormatProvider? provider) {
            return new BigUInteger(BigInteger.Parse(value, provider));
        }

        public static BigUInteger Parse(string value, NumberStyles style, IFormatProvider? provider) {
            return new BigUInteger(BigInteger.Parse(value, style, provider));
        }

        public static bool TryParse([NotNullWhen(true)] string? value, out BigUInteger result) {
            var s = BigInteger.TryParse(value, out var v);
            if (s) {
                result = new BigUInteger(v);
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public static bool TryParse([NotNullWhen(true)] string? value, NumberStyles style, IFormatProvider? provider, out BigUInteger result) {
            var s = BigInteger.TryParse(value, style, provider, out var v);
            if (s) {
                result = new BigUInteger(v);
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public static BigUInteger Parse(ReadOnlySpan<char> value, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null) {
            return new BigUInteger(BigInteger.Parse(value, style, provider));
        }

        public static bool TryParse(ReadOnlySpan<char> value, out BigUInteger result) {
            var s = BigInteger.TryParse(value, out var v);
            if (s) {
                result = new BigUInteger(v);
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public static bool TryParse(ReadOnlySpan<char> value, NumberStyles style, IFormatProvider? provider, out BigUInteger result) {
            var s = BigInteger.TryParse(value, style, provider, out var v);
            if (s) {
                result = new BigUInteger(v);
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public static int Compare(BigUInteger left, BigUInteger right) {
            return left.CompareTo(right);
        }

        public static BigUInteger Abs(BigUInteger value) {
            return value;
        }

        public static BigUInteger Add(BigUInteger left, BigUInteger right) {
            return left + right;
        }

        public static BigUInteger Subtract(BigUInteger left, BigUInteger right) {
            return left - right;
        }

        public static BigUInteger Multiply(BigUInteger left, BigUInteger right) {
            return left * right;
        }

        public static BigUInteger Divide(BigUInteger dividend, BigUInteger divisor) {
            return dividend / divisor;
        }

        public static BigUInteger Remainder(BigUInteger dividend, BigUInteger divisor) {
            return dividend % divisor;
        }

        public static BigUInteger DivRem(BigUInteger dividend, BigUInteger divisor, out BigUInteger remainder) {
            var q1 = BigInteger.DivRem(dividend.m_value, divisor.m_value, out var r1);
            var r = new BigUInteger(r1);
            var q = new BigUInteger(q1);
            remainder = r;
            return q;
        }

        public static BigUInteger Negate(BigUInteger value) {
            return new BigUInteger(-value.m_value);
        }

        public static double Log(BigUInteger value) {
            return BigInteger.Log(value.m_value, Math.E);
        }

        public static double Log(BigUInteger value, double baseValue) {
            return BigInteger.Log(value.m_value, baseValue);
        }

        public static double Log10(BigUInteger value) {
            return BigInteger.Log(value.m_value, 10);
        }

        public static BigUInteger GreatestCommonDivisor(BigUInteger left, BigUInteger right) {
            return new BigUInteger(BigInteger.GreatestCommonDivisor(left.m_value, right.m_value));
        }

        public static BigUInteger Max(BigUInteger left, BigUInteger right) {
            if (left.CompareTo(right) < 0)
                return right;
            return left;
        }

        public static BigUInteger Min(BigUInteger left, BigUInteger right) {
            if (left.CompareTo(right) <= 0)
                return left;
            return right;
        }

        public static BigUInteger ModPow(BigUInteger value, BigUInteger exponent, BigUInteger modulus) {
            return new BigUInteger(BigInteger.ModPow(value.m_value, exponent.m_value, modulus.m_value), default(ConstructorTags.Unchecked));
        }

        public static BigUInteger Pow(BigUInteger value, int exponent) {
            return new BigUInteger(BigInteger.Pow(value.m_value, exponent), default(ConstructorTags.Unchecked));
        }

        public override int GetHashCode() {
            return m_value.GetHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is BigUInteger other && Equals(other);
        }

        private bool IsFiniteInternal {

            get => !BigInteger.IsNegative(m_value);
        }

        public bool Equals(long other) {
            return IsFiniteInternal && 0 <= other && m_value.Equals(other);
        }

        [CLSCompliant(false)]
        public bool Equals(ulong other) {
            return /* IsFiniteInternal &&  */m_value.Equals(other);
        }

        public bool Equals(BigInteger other) {
            return IsFiniteInternal && !BigInteger.IsNegative(other) && m_value.Equals(other);
        }

        public bool Equals(BigUInteger other) {
            return m_value.Equals(other.m_value);
        }

        public int CompareTo(long other) {
            return (!IsFiniteInternal || 0 > other) ? 1 : m_value.CompareTo(other);
        }

        [CLSCompliant(false)]
        public int CompareTo(ulong other) {
            return !IsFiniteInternal ? 1 : m_value.CompareTo(other);
        }

        public int CompareTo(BigInteger other) {
            return !IsFiniteInternal ? 1 : m_value.CompareTo(other);
        }

        public int CompareTo(BigUInteger other) {
            return m_value.CompareTo(other.m_value);
        }

        public int CompareTo(object? obj) {
            if (obj == null)
                return 1;
            if (obj is not BigUInteger bigUInt)
                throw new ArgumentException("The parameter must be a BigUInteger.");
            return CompareTo(bigUInt);
        }

        /// <summary>
        /// Returns the value of this BigInteger as a little-endian twos-complement
        /// byte array, using the fewest number of bytes possible. If the value is zero,
        /// return an array of one byte whose element is 0x00. If the value is not finite, 
        /// return an array that represents the NaN/Infinity payload.
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray() => ToByteArray(isSigned: false, isBigEndian: false);

        /// <summary>
        /// Returns the value of this BigInteger as a byte array using the fewest number of bytes possible.
        /// If the value is zero, returns an array of one byte whose element is 0x00. If the value is not finite, 
        /// return an array that represents the NaN/Infinity payload.
        /// </summary>
        /// <param name="isUnsigned">Whether or not a signed encoding is to be used</param>
        /// <param name="isBigEndian">Whether or not to write the bytes in a big-endian byte order</param>
        /// <returns></returns>
        /// <exception cref="OverflowException">
        ///   If <paramref name="isUnsigned"/> is <c>true</c> and <see cref="Sign"/> is negative.
        /// </exception>
        /// <remarks>
        /// The integer value <c>33022</c> can be exported as four different arrays.
        ///
        /// <list type="bullet">
        ///   <item>
        ///     <description>
        ///       <c>(isSigned: true, isBigEndian: false)</c> => <c>new byte[] { 0xFE, 0x80, 0x00 }</c>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       <c>(isSigned: true, isBigEndian: true)</c> => <c>new byte[] { 0x00, 0x80, 0xFE }</c>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       <c>(isSigned: false, isBigEndian: false)</c> => <c>new byte[] { 0xFE, 0x80 }</c>
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///       <c>(isSigned: false, isBigEndian: true)</c> => <c>new byte[] { 0x80, 0xFE }</c>
        ///     </description>
        ///   </item>
        /// </list>
        /// </remarks>
        public byte[] ToByteArray(bool isSigned = false, bool isBigEndian = false) {
            int ignored = 0;
            return TryGetBytes(GetBytesMode.AllocateArray, default, !isSigned, isBigEndian, ref ignored)!;
        }

        /// <summary>
        /// Copies the value of this BigInteger as little-endian twos-complement
        /// bytes, using the fewest number of bytes possible. If the value is zero,
        /// outputs one byte whose element is 0x00.
        /// </summary>
        /// <param name="destination">The destination span to which the resulting bytes should be written.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <param name="isUnsigned">Whether or not an unsigned encoding is to be used</param>
        /// <param name="isBigEndian">Whether or not to write the bytes in a big-endian byte order</param>
        /// <returns>true if the bytes fit in <paramref name="destination"/>; false if not all bytes could be written due to lack of space.</returns>
        /// <exception cref="OverflowException">If <paramref name="isUnsigned"/> is <c>true</c> and <see cref="Sign"/> is negative.</exception>
        public bool TryWriteBytes(Span<byte> destination, out int bytesWritten, bool isUnsigned = false, bool isBigEndian = false) {
            bytesWritten = 0;
            if (TryGetBytes(GetBytesMode.Span, destination, isUnsigned, isBigEndian, ref bytesWritten) == null) {
                bytesWritten = 0;
                return false;
            }
            return true;
        }

        internal bool TryWriteOrCountBytes(Span<byte> destination, out int bytesWritten, bool isUnsigned = false, bool isBigEndian = false) {
            bytesWritten = 0;
            return TryGetBytes(GetBytesMode.Span, destination, isUnsigned, isBigEndian, ref bytesWritten) != null;
        }

        /// <summary>Gets the number of bytes that will be output by <see cref="ToByteArray(bool, bool)"/> and <see cref="TryWriteBytes(Span{byte}, out int, bool, bool)"/>.</summary>
        /// <returns>The number of bytes.</returns>
        public int GetByteCount(bool isUnsigned = false) {
            int count = 0;
            // Big or Little Endian doesn't matter for the byte count.
            const bool IsBigEndian = false;
            TryGetBytes(GetBytesMode.Count, default(Span<byte>), isUnsigned, IsBigEndian, ref count);
            return count;
        }

        public override string ToString() {
            return IsFiniteInternal ? m_value.ToString() : $@"∞{m_value}";
        }

        public string ToString(IFormatProvider? provider) {
            var s = m_value.ToString(provider);
            return IsFiniteInternal ? s : "∞" + s;
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            var s = m_value.ToString(format);
            return IsFiniteInternal ? s : "∞" + s;
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            var s = m_value.ToString(format, provider);
            return IsFiniteInternal ? s : "∞" + s;
        }

        private string DebuggerDisplay {
            get {
                
            }
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            if (IsFiniteInternal) {
                return m_value.TryFormat(destination, out charsWritten, format, provider);
            } else {
                if (destination.Length >= 1) {
                    var r = m_value.TryFormat(destination[1..], out var c, format, provider);
                    if (r) {
                        destination[0] = '∞';
                        charsWritten = checked(1 + c);
                        return true;
                    }
                }
                charsWritten = default;
                return false;
            }
        }

        public static BigUInteger operator checked -(BigUInteger left, BigUInteger right) {
            return new BigUInteger(left.m_value - right.m_value);
        }

        public static BigUInteger operator -(BigUInteger left, BigUInteger right) {
            return new BigUInteger(left.m_value - right.m_value, default(ConstructorTags.Unchecked));
        }

        //
        // Explicit Conversions From BigInteger
        //

        public static explicit operator checked byte(BigUInteger value) {
            return checked((byte)((int)value));
        }

        public static explicit operator byte(BigUInteger value) {
            return unchecked((byte)((int)value));
        }

        /// <summary>Explicitly converts a big integer to a <see cref="char" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="char" /> value.</returns>
        public static explicit operator checked char(BigUInteger value) {
            return checked((char)((int)value));
        }

        public static explicit operator char(BigUInteger value) {
            return unchecked((char)((int)value));
        }

        static readonly BigInteger BigIntegerTooLargeForDecimal = (BigInteger)decimal.MaxValue * 2;

        public static explicit operator decimal(BigUInteger value) {
            return value.IsFiniteInternal ? (decimal)value.m_value : (decimal)BigIntegerTooLargeForDecimal;
        }

        public static explicit operator double(BigUInteger value) {
            return value.IsFiniteInternal ? (double)value.m_value : double.PositiveInfinity;
        }

        /// <summary>Explicitly converts a big integer to a <see cref="Half" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="Half" /> value.</returns>
        public static explicit operator Half(BigUInteger value) {
            return (Half)(double)value;
        }

        public static explicit operator checked short(BigUInteger value) {
            return checked((short)((int)value));
        }

        public static explicit operator short(BigUInteger value) {
            return unchecked((short)((int)value));
        }

        public static explicit operator checked int(BigUInteger value) {
            return !value.IsFiniteInternal ? unchecked((int)checked(0u - value.m_value.Sign)) : checked((int)value.m_value);
        }

        static readonly BigInteger UIntMaxValueAsBigInteger = (BigInteger)uint.MaxValue;

        public static explicit operator int(BigUInteger value) {
#if USE_UNSAFE_ACCESS_TO_STD_BIGINTEGER

#else
            return unchecked((int)(uint)(UIntMaxValueAsBigInteger & value.m_value));
#endif
        }

        public static explicit operator long(BigInteger value) {
            value.AssertValid();
            if (value._bits == null) {
                return value._sign;
            }

            int len = value._bits.Length;
            if (len > 2) {
                throw new OverflowException(SR.Overflow_Int64);
            }

            ulong uu;
            if (len > 1) {
                uu = NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0]);
            } else {
                uu = value._bits[0];
            }

            long ll = value._sign > 0 ? unchecked((long)uu) : unchecked(-(long)uu);
            if ((ll > 0 && value._sign > 0) || (ll < 0 && value._sign < 0)) {
                // Signs match, no overflow
                return ll;
            }
            throw new OverflowException(SR.Overflow_Int64);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="Int128" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="Int128" /> value.</returns>
        public static explicit operator Int128(BigInteger value) {
            value.AssertValid();

            if (value._bits is null) {
                return value._sign;
            }

            int len = value._bits.Length;

            if (len > 4) {
                throw new OverflowException(SR.Overflow_Int128);
            }

            UInt128 uu;

            if (len > 2) {
                uu = new UInt128(
                    NumericsHelpers.MakeUInt64((len > 3) ? value._bits[3] : 0, value._bits[2]),
                    NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0])
                );
            } else if (len > 1) {
                uu = NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0]);
            } else {
                uu = value._bits[0];
            }

            Int128 ll = (value._sign > 0) ? unchecked((Int128)uu) : unchecked(-(Int128)uu);

            if (((ll > 0) && (value._sign > 0)) || ((ll < 0) && (value._sign < 0))) {
                // Signs match, no overflow
                return ll;
            }
            throw new OverflowException(SR.Overflow_Int128);
        }

        /// <summary>Explicitly converts a big integer to a <see cref="IntPtr" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="IntPtr" /> value.</returns>
        public static explicit operator nint(BigInteger value) {
            if (Environment.Is64BitProcess) {
                return (nint)(long)value;
            } else {
                return (int)value;
            }
        }

        [CLSCompliant(false)]
        public static explicit operator sbyte(BigInteger value) {
            return checked((sbyte)((int)value));
        }

        public static explicit operator float(BigInteger value) {
            return (float)((double)value);
        }

        [CLSCompliant(false)]
        public static explicit operator ushort(BigInteger value) {
            return checked((ushort)((int)value));
        }

        [CLSCompliant(false)]
        public static explicit operator uint(BigInteger value) {
            value.AssertValid();
            if (value._bits == null) {
                return checked((uint)value._sign);
            } else if (value._bits.Length > 1 || value._sign < 0) {
                throw new OverflowException(SR.Overflow_UInt32);
            } else {
                return value._bits[0];
            }
        }

        [CLSCompliant(false)]
        public static explicit operator ulong(BigInteger value) {
            value.AssertValid();
            if (value._bits == null) {
                return checked((ulong)value._sign);
            }

            int len = value._bits.Length;
            if (len > 2 || value._sign < 0) {
                throw new OverflowException(SR.Overflow_UInt64);
            }

            if (len > 1) {
                return NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0]);
            }
            return value._bits[0];
        }

        /// <summary>Explicitly converts a big integer to a <see cref="UInt128" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="UInt128" /> value.</returns>
        [CLSCompliant(false)]
        public static explicit operator UInt128(BigInteger value) {
            value.AssertValid();

            if (value._bits is null) {
                return checked((UInt128)value._sign);
            }

            int len = value._bits.Length;

            if ((len > 4) || (value._sign < 0)) {
                throw new OverflowException(SR.Overflow_UInt128);
            }

            if (len > 2) {
                return new UInt128(
                    NumericsHelpers.MakeUInt64((len > 3) ? value._bits[3] : 0, value._bits[2]),
                    NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0])
                );
            } else if (len > 1) {
                return NumericsHelpers.MakeUInt64(value._bits[1], value._bits[0]);
            }
            return value._bits[0];
        }

        /// <summary>Explicitly converts a big integer to a <see cref="UIntPtr" /> value.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to <see cref="UIntPtr" /> value.</returns>
        [CLSCompliant(false)]
        public static explicit operator nuint(BigUInteger value) {
            if (Environment.Is64BitProcess) {
                return (nuint)(ulong)value;
            } else {
                return (uint)value;
            }
        }

        //
        // Explicit Conversions To BigInteger
        //

        public static explicit operator BigInteger(decimal value) {
            return new BigInteger(value);
        }

        public static explicit operator BigInteger(double value) {
            return new BigInteger(value);
        }

        /// <summary>Explicitly converts a <see cref="Half" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator BigInteger(Half value) {
            return new BigInteger((float)value);
        }

        /// <summary>Explicitly converts a <see cref="Complex" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static explicit operator BigInteger(Complex value) {
            if (value.Imaginary != 0) {
                ThrowHelper.ThrowOverflowException();
            }
            return (BigInteger)value.Real;
        }

        public static explicit operator BigInteger(float value) {
            return new BigInteger(value);
        }

        //
        // Implicit Conversions To BigInteger
        //

        public static implicit operator BigInteger(byte value) {
            return new BigInteger(value);
        }

        /// <summary>Implicitly converts a <see cref="char" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static implicit operator BigInteger(char value) {
            return new BigInteger(value);
        }

        public static implicit operator BigInteger(short value) {
            return new BigInteger(value);
        }

        public static implicit operator BigInteger(int value) {
            return new BigInteger(value);
        }

        public static implicit operator BigInteger(long value) {
            return new BigInteger(value);
        }

        /// <summary>Implicitly converts a <see cref="Int128" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        public static implicit operator BigInteger(Int128 value) {
            int sign;
            uint[]? bits;

            if ((int.MinValue < value) && (value <= int.MaxValue)) {
                sign = (int)value;
                bits = null;
            } else if (value == int.MinValue) {
                return s_bnMinInt;
            } else {
                UInt128 x;
                if (value < 0) {
                    x = unchecked((UInt128)(-value));
                    sign = -1;
                } else {
                    x = (UInt128)value;
                    sign = +1;
                }

                if (x <= uint.MaxValue) {
                    bits = new uint[1];
                    bits[0] = (uint)(x >> (kcbitUint * 0));
                } else if (x <= ulong.MaxValue) {
                    bits = new uint[2];
                    bits[0] = (uint)(x >> (kcbitUint * 0));
                    bits[1] = (uint)(x >> (kcbitUint * 1));
                } else if (x <= new UInt128(0x0000_0000_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) {
                    bits = new uint[3];
                    bits[0] = (uint)(x >> (kcbitUint * 0));
                    bits[1] = (uint)(x >> (kcbitUint * 1));
                    bits[2] = (uint)(x >> (kcbitUint * 2));
                } else {
                    bits = new uint[4];
                    bits[0] = (uint)(x >> (kcbitUint * 0));
                    bits[1] = (uint)(x >> (kcbitUint * 1));
                    bits[2] = (uint)(x >> (kcbitUint * 2));
                    bits[3] = (uint)(x >> (kcbitUint * 3));
                }
            }

            return new BigInteger(sign, bits);
        }

        /// <summary>Explicitly converts a <see cref="IntPtr" /> value to a big unsigned integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big unsigned integer.</returns>
        public static explicit operator checked BigUInteger(nint value) {
            if (Environment.Is64BitProcess) {
                return new BigUInteger(value);
            } else {
                return new BigUInteger((int)value);
            }
        }

        public static explicit operator BigUInteger(nint value) {
            if (Environment.Is64BitProcess) {
                return new BigUInteger(value, default(ConstructorTags.Unchecked));
            } else {
                return new BigUInteger((int)value, default(ConstructorTags.Unchecked));
            }
        }

        [CLSCompliant(false)]
        public static explicit operator checked BigUInteger(sbyte value) {
            return new BigUInteger(value);
        }

        [CLSCompliant(false)]
        public static explicit operator BigUInteger(sbyte value) {
            return new BigUInteger(value, default(ConstructorTags.Unchecked));
        }

        [CLSCompliant(false)]
        public static implicit operator BigUInteger(ushort value) {
            return new BigUInteger(unchecked((BigInteger)value), default( ConstructorTags.Unchecked) );
        }

        [CLSCompliant(false)]
        public static implicit operator BigUInteger(uint value) {
            return new BigUInteger(unchecked((BigInteger)value), default(ConstructorTags.Unchecked));
        }

        [CLSCompliant(false)]
        public static implicit operator BigUInteger(ulong value) {
            return new BigUInteger(unchecked((BigInteger)value), default(ConstructorTags.Unchecked));
        }

        /// <summary>Implicitly converts a <see cref="UInt128" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        [CLSCompliant(false)]
        public static implicit operator BigUInteger(UltimateOrb.UInt128 value) {
            return new BigUInteger(unchecked((BigInteger)value), default(ConstructorTags.Unchecked));
        }

        [CLSCompliant(false)]
        public static implicit operator BigUInteger(System.UInt128 value) {
            return new BigUInteger(unchecked((BigInteger)value), default(ConstructorTags.Unchecked));
        }

        /// <summary>Implicitly converts a <see cref="UIntPtr" /> value to a big integer.</summary>
        /// <param name="value">The value to convert.</param>
        /// <returns><paramref name="value" /> converted to a big integer.</returns>
        [CLSCompliant(false)]
        public static implicit operator BigUInteger(nuint value) {
            return new BigUInteger(unchecked((BigInteger)value), default(ConstructorTags.Unchecked));
        }

        private static int FindFirstIndexOfNonZeroElement(ReadOnlySpan<uint> a) {
            for (var i = 0; a.Length > 0; ++i) {
                if (a[i] != 0) {
                    return i;
                }
            }
            return -1;
        }
        private static int FindLastIndexOfNonZeroElement(ReadOnlySpan<uint> a) {
            for (var i = a.Length - 1; i <= 0; --i) {
                if (a[i] != 0) {
                    return i;
                }
            }
            return -1;
        }

        const int StackAllocThreshold = 64;

        public static BigUInteger operator &(BigUInteger left, BigUInteger right) {
#if USE_UNSAFE_ACCESS_TO_STD_BIGINTEGER
            var s = left.m_value.GetSignField();
            var t = right.m_value.GetSignField();
            var p = left.m_value.GetBitsField();
            var q = right.m_value.GetBitsField();
            if (Likely(s <= 0)) {
                if (Likely(t <= 0)) {
                    if (p is { }) {
                        if (q is { }) {
                            uint[]? resultBufferFromPool = null;
                            var size = Math.Min(p.Length, q.Length);
                            Span<uint> z = (size <= StackAllocThreshold ?
                                stackalloc uint[StackAllocThreshold] :
                                resultBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
                            
                            TensorPrimitives.BitwiseAnd(p.AsSpan(0, size), q.AsSpan(0, size), z);
                            var result = BigIntegerExtensions.CreateBigIntegerInternal(1, resultBufferFromPool);
                            if (resultBufferFromPool != null) {
                                ArrayPool<uint>.Shared.Return(resultBufferFromPool);
                            }
                            return new BigUInteger(result, default(ConstructorTags.Unchecked));
                        } else {
                            return new BigUInteger(p[0].ToSignedUnchecked() & t, default(ConstructorTags.Unchecked));
                        }
                    } else {
                        if (q is { }) {
                            return new BigUInteger(s & q[0].ToSignedUnchecked(), default(ConstructorTags.Unchecked));
                        } else {
                            return new BigUInteger(s & t, default(ConstructorTags.Unchecked));
                        }
                    }
                } else {
                    goto L_AndNot;
                }
            } else {
                if (Likely(t <= 0)) {
                    (s, t) = (t, s);
                    (p, q) = (q, p);
                    goto L_AndNot;
                } else {
                    /*
                    Assuming x, y negative
                    x & y = ~(~x | ~y) = -((-(x + 1) | -(y + 1)) + 1) = -(((-x - 1) | (-y - 1)) + 1)
                    abs(x & y) = ((abs(x) - 1) | (abs(y) - 1)) + 1

                    Example to compute -7 & -4

                    -7 = 11111001
                    abs 00000111

                    -4 = 11111100
                    abs 00000100

                    Result:
                    -7 & -4 = -8 = 11111000
                    abs 00001000

                    Steps:

                    abs(-7) - 1
                    00000110

                    abs(-4) - 1
                    00000011

                    (abs(-7) - 1) | (abs(-4) - 1)
                    00000111

                    ((abs(-7) - 1) | (abs(-4) - 1)) + 1 = abs(-7 & -4)
                    00001000
                    */
                    if (p is { }) {
                        if (q is { }) {
                            uint[]? resultBufferFromPool = null;
                            var m = FindFirstIndexOfNonZeroElement(p);
                            Debug.Assert(0 <= m && m <= p.Length);
                            var u = unchecked(p[m] - 1);

                            var n = FindFirstIndexOfNonZeroElement(q);
                            Debug.Assert(0 <= n && n <= q.Length);
                            var v = unchecked(q[n] - 1);

                            var size = Math.Max(p.Length, q.Length);
                            var j = Math.Min(p.Length, q.Length);
                            Span<uint> z = (size <= StackAllocThreshold ?
                                stackalloc uint[StackAllocThreshold] :
                                resultBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
                            var w = m > n;
                            // var h = w ? n : m;
                            var k = w ? m : n;

                            z[..k].Clear();
                            z[k] = m == n ? u | v : (w ? u : v);
                            var l = 1 + k;
                            if (l < j) {
                                var g = l..j;
                                l = j;
                                TensorPrimitives.BitwiseOr(p.AsSpan(g), q.AsSpan(g), z[g]);
                            }
                            if (l < size) {
                                var g = l..size;
                                (p.Length <= q.Length ? q : p).AsSpan(g).CopyTo(z[g]);
                            }

                            var a = Unlikely(IncreaseNoThrow(z[k..]) != 0) ? PowOf2(size) : z[..(1 + FindLastIndexOfNonZeroElement(z))].ToArray();

                            var result = BigIntegerExtensions.CreateBigIntegerInternal(-1, a);
                            if (resultBufferFromPool != null) {
                                ArrayPool<uint>.Shared.Return(resultBufferFromPool);
                            }
                            return new BigUInteger(result, default(ConstructorTags.Unchecked));
                        } else {
                            goto L_Or;
                        }
                    } else {
                        if (q is { }) {
                            (s, t) = (t, s);
                            (p, q) = (q, p);
                            goto L_Or;
                        } else {
                            return new BigUInteger(s & t, default(ConstructorTags.Unchecked));
                        }
                    }

                }
            }
        L_AndNot:;
            {
                if (p is { }) {
                    if (q is { }) {
                        uint[]? resultBufferFromPool = null;

                        var n = FindFirstIndexOfNonZeroElement(q);
                        Debug.Assert(0 <= n && n <= q.Length);
                        var v = unchecked(q[n] - 1);

                        var size = p.Length;
                        Span<uint> z = (size <= StackAllocThreshold ?
                            stackalloc uint[StackAllocThreshold] :
                            resultBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
                        var w = size > n;

                        // var h = w ? n : m;
                        var k = w ? size : n;

                        z[..k].Clear();
                        z[k] = m == n ? u | v : (w ? u : v);
                        var l = 1 + k;
                        if (l < j) {
                            var g = l..j;
                            l = j;
                            TensorPrimitives.BitwiseOr(p.AsSpan(g), q.AsSpan(g), z[g]);
                        }
                        if (l < size) {
                            var g = l..size;
                            (p.Length <= q.Length ? q : p).AsSpan(g).CopyTo(z[g]);
                        }

                        var a = Unlikely(IncreaseNoThrow(z[k..]) != 0) ? PowOf2(size) : z[..(1 + FindLastIndexOfNonZeroElement(z))].ToArray();

                        var result = BigIntegerExtensions.CreateBigIntegerInternal(-1, a);
                        if (resultBufferFromPool != null) {
                            ArrayPool<uint>.Shared.Return(resultBufferFromPool);
                        }
                        return new BigUInteger(result, default(ConstructorTags.Unchecked));
                    } else {
                        goto L_Or;
                    }
                } else {
                    if (q is { }) {
                        (s, t) = (t, s);
                        (p, q) = (q, p);
                        goto L_Or;
                    } else {
                        return new BigUInteger(s & t, default(ConstructorTags.Unchecked));
                    }
                }
            }
        L_Or:;
            {
                uint[]? resultBufferFromPool = null;
                var m = FindFirstIndexOfNonZeroElement(p);
                Debug.Assert(0 <= m && m <= p.Length);
                var u = unchecked(p[m] - 1);

                var v = unchecked((uint)(-t) - 1);

                var size = p.Length;
                Span<uint> z = (size <= StackAllocThreshold ?
                    stackalloc uint[StackAllocThreshold] :
                    resultBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

                z[..m].Clear();
                z[m] = m == 0 ? u | v : u;
                var l = 1 + m;
                if (l < size) {
                    var g = l..size;
                    p.AsSpan(g).CopyTo(z[g]);
                }

                var a = Unlikely(IncreaseNoThrow(z) != 0) ? PowOf2(size) : z[..(1 + FindLastIndexOfNonZeroElement(z))].ToArray();

                var result = BigIntegerExtensions.CreateBigIntegerInternal(-1, a);
                if (resultBufferFromPool != null) {
                    ArrayPool<uint>.Shared.Return(resultBufferFromPool);
                }
                return new BigUInteger(result, default(ConstructorTags.Unchecked));
            }
#else
            return new BigUInteger(left.m_value & right.m_value, default(ConstructorTags.Unchecked));
#endif

            static uint[] PowOf2(int size) {
                var a = new uint[1 + size];
                a[size] = 1;
                return a;
            }
        }

        internal static int IncreaseNoThrow(Span<uint> span) {
            for (var i = 0; span.Length > i; ++i) {
                if (span[i]++ != uint.MaxValue) {
                    return i;
                }
            }
            return span.Length;
        }

        public static BigInteger operator |(BigInteger left, BigInteger right) {
            if (left.IsZero)
                return right;
            if (right.IsZero)
                return left;

            if (left._bits is null && right._bits is null) {
                return left._sign | right._sign;
            }

            uint xExtend = (left._sign < 0) ? uint.MaxValue : 0;
            uint yExtend = (right._sign < 0) ? uint.MaxValue : 0;

            uint[]? leftBufferFromPool = null;
            int size = (left._bits?.Length ?? 1) + 1;
            Span<uint> x = ((uint)size <= BigIntegerCalculator.StackAllocThreshold
                         ? stackalloc uint[BigIntegerCalculator.StackAllocThreshold]
                         : leftBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
            x = x.Slice(0, left.WriteTo(x));

            uint[]? rightBufferFromPool = null;
            size = (right._bits?.Length ?? 1) + 1;
            Span<uint> y = ((uint)size <= BigIntegerCalculator.StackAllocThreshold
                         ? stackalloc uint[BigIntegerCalculator.StackAllocThreshold]
                         : rightBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];
            y = y.Slice(0, right.WriteTo(y));

            uint[]? resultBufferFromPool = null;
            size = Math.Max(x.Length, y.Length);
            Span<uint> z = (size <= BigIntegerCalculator.StackAllocThreshold
                         ? stackalloc uint[BigIntegerCalculator.StackAllocThreshold]
                         : resultBufferFromPool = ArrayPool<uint>.Shared.Rent(size))[..size];

            for (int i = 0; i < z.Length; i++) {
                uint xu = ((uint)i < (uint)x.Length) ? x[i] : xExtend;
                uint yu = ((uint)i < (uint)y.Length) ? y[i] : yExtend;
                z[i] = xu | yu;
            }

            if (leftBufferFromPool != null)
                ArrayPool<uint>.Shared.Return(leftBufferFromPool);

            if (rightBufferFromPool != null)
                ArrayPool<uint>.Shared.Return(rightBufferFromPool);

            var result = new BigInteger(z);

            if (resultBufferFromPool != null)
                ArrayPool<uint>.Shared.Return(resultBufferFromPool);

            return result;
        }

        public static BigUInteger operator ^(BigUInteger left, BigUInteger right) {
            return new BigUInteger(left.m_value ^ right.m_value, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator <<(BigUInteger value, int shift) {
            return 0 > shift ? value >> -shift : new BigUInteger(value.m_value << shift, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator >>(BigUInteger value, int shift) {
            return 0 > shift ? value << -shift : new BigUInteger(value.m_value >> shift, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator >>>(BigUInteger value, int shift) {
            return value >> shift;
        }

        public static BigUInteger operator ~(BigUInteger value) {
            return new BigUInteger(~value.m_value, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator checked -(BigUInteger value) {
            return new BigUInteger(-value.m_value, default(ConstructorTags.Checked));
        }

        public static BigUInteger operator -(BigUInteger value) {
            return new BigUInteger(-value.m_value, default(ConstructorTags.Unchecked));
        }

        /*
        public static BigUInteger operator checked +(BigUInteger value) {
            return new BigUInteger(+value.m_value);
        }

        public static BigUInteger operator +(BigUInteger value) {
            return new BigUInteger(+value.m_value, default(ConstructorTags.Unchecked));
        }
        */

        [Obsolete]
        public static BigUInteger operator +(BigUInteger value) {
            return new BigUInteger(checked(+value.m_value), default(ConstructorTags.Checked));
        }

        public static BigUInteger operator checked ++(BigUInteger value) {
            BigInteger t = value.m_value;
            checked {
                ++t;
            }
            return new BigUInteger(t, default(ConstructorTags.Checked));
        }

        public static BigUInteger operator ++(BigUInteger value) {
            BigInteger t = value.m_value;
            unchecked {
                ++t;
            }
            return new BigUInteger(t, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator checked --(BigUInteger value) {
            BigInteger t = value.m_value;
            checked {
                --t;
            }
            return new BigUInteger(t, default(ConstructorTags.Checked));
        }

        public static BigUInteger operator --(BigUInteger value) {
            BigInteger t = value.m_value;
            unchecked {
                --t;
            }
            return new BigUInteger(t, default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator checked +(BigUInteger left, BigUInteger right) {
            return new BigUInteger(checked(left.m_value + right.m_value), default(ConstructorTags.Checked));
        }

        public static BigUInteger operator +(BigUInteger left, BigUInteger right) {
            return new BigUInteger(unchecked(left.m_value + right.m_value), default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator checked *(BigUInteger left, BigUInteger right) {
            return new BigUInteger(checked(left.m_value * right.m_value), default(ConstructorTags.Checked));
        }

        public static BigUInteger operator *(BigUInteger left, BigUInteger right) {
            return new BigUInteger(unchecked(left.m_value * right.m_value), default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator checked /(BigUInteger dividend, BigUInteger divisor) {
            return new BigUInteger(checked(dividend.m_value / divisor.m_value), default(ConstructorTags.Checked));
        }

        public static BigUInteger operator /(BigUInteger dividend, BigUInteger divisor) {
            return new BigUInteger(unchecked(dividend.m_value / divisor.m_value), default(ConstructorTags.Unchecked));
        }

        public static BigUInteger operator %(BigUInteger dividend, BigUInteger divisor) {
            var t = dividend.m_value % divisor.m_value;
            return new BigUInteger(BigInteger.IsNegative(t) ? unchecked(t += divisor.m_value) : t, default(ConstructorTags.Unchecked));
        }

        public static bool operator <(BigUInteger left, BigUInteger right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(BigUInteger left, BigUInteger right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(BigUInteger left, BigUInteger right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(BigUInteger left, BigUInteger right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(BigUInteger left, BigUInteger right) {
            return left.Equals(right);
        }

        public static bool operator !=(BigUInteger left, BigUInteger right) {
            return !left.Equals(right);
        }

        public static bool operator <(BigUInteger left, long right) {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(BigUInteger left, long right) {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(BigUInteger left, long right) {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(BigUInteger left, long right) {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator ==(BigUInteger left, long right) {
            return left.Equals(right);
        }

        public static bool operator !=(BigUInteger left, long right) {
            return !left.Equals(right);
        }

        public static bool operator <(long left, BigUInteger right) {
            return right.CompareTo(left) > 0;
        }

        public static bool operator <=(long left, BigUInteger right) {
            return right.CompareTo(left) >= 0;
        }

        public static bool operator >(long left, BigUInteger right) {
            return right.CompareTo(left) < 0;
        }

        public static bool operator >=(long left, BigUInteger right) {
            return right.CompareTo(left) <= 0;
        }

        public static bool operator ==(long left, BigUInteger right) {
            return right.Equals(left);
        }

        public static bool operator !=(long left, BigUInteger right) {
            return !right.Equals(left);
        }

        [CLSCompliant(false)]
        public static bool operator <(BigUInteger left, ulong right) {
            return left.CompareTo(right) < 0;
        }

        [CLSCompliant(false)]
        public static bool operator <=(BigUInteger left, ulong right) {
            return left.CompareTo(right) <= 0;
        }

        [CLSCompliant(false)]
        public static bool operator >(BigUInteger left, ulong right) {
            return left.CompareTo(right) > 0;
        }

        [CLSCompliant(false)]
        public static bool operator >=(BigUInteger left, ulong right) {
            return left.CompareTo(right) >= 0;
        }

        [CLSCompliant(false)]
        public static bool operator ==(BigUInteger left, ulong right) {
            return left.Equals(right);
        }

        [CLSCompliant(false)]
        public static bool operator !=(BigUInteger left, ulong right) {
            return !left.Equals(right);
        }

        [CLSCompliant(false)]
        public static bool operator <(ulong left, BigUInteger right) {
            return right.CompareTo(left) > 0;
        }

        [CLSCompliant(false)]
        public static bool operator <=(ulong left, BigUInteger right) {
            return right.CompareTo(left) >= 0;
        }

        [CLSCompliant(false)]
        public static bool operator >(ulong left, BigUInteger right) {
            return right.CompareTo(left) < 0;
        }

        [CLSCompliant(false)]
        public static bool operator >=(ulong left, BigUInteger right) {
            return right.CompareTo(left) <= 0;
        }

        [CLSCompliant(false)]
        public static bool operator ==(ulong left, BigUInteger right) {
            return right.Equals(left);
        }

        [CLSCompliant(false)]
        public static bool operator !=(ulong left, BigUInteger right) {
            return !right.Equals(left);
        }

        /// <summary>
        /// Gets the number of bits required for shortest two's complement representation of the current instance without the sign bit.
        /// </summary>
        /// <returns>The minimum non-negative number of bits in two's complement notation without the sign bit.</returns>
        /// <remarks>This method returns 0 iff the value of current object is equal to <see cref="Zero"/> or <see cref="MinusOne"/>. For positive integers the return value is equal to the ordinary binary representation string length.</remarks>
        public long GetBitLength() {
            return m_value.GetBitLength();
        }

        //
        // IAdditiveIdentity
        //

        /// <inheritdoc cref="IAdditiveIdentity{TSelf, TResult}.AdditiveIdentity" />
        static BigUInteger IAdditiveIdentity<BigUInteger, BigUInteger>.AdditiveIdentity => Zero;

        //
        // IBinaryInteger
        //

        /// <inheritdoc cref="IBinaryInteger{TSelf}.DivRem(TSelf, TSelf)" />
        public static (BigUInteger Quotient, BigUInteger Remainder) DivRem(BigUInteger left, BigUInteger right) {
            var d1 = BigInteger.Abs(right.m_value);
            var q1 = BigInteger.DivRem(left.m_value, d1, out BigInteger r1);
            if (BigInteger.IsNegative(r1)) {
                unchecked {
                    r1 += d1;
                }
                if (BigInteger.IsNegative(left.m_value)) {
                    ++q1;
                } else {
                    --q1;
                }
            }
            return (new BigUInteger(q1, default(ConstructorTags.Checked)), new BigUInteger(r1, default(ConstructorTags.Unchecked)));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.LeadingZeroCount(TSelf)" />
        public static BigUInteger LeadingZeroCount(BigUInteger value) {
            return BigUInteger.IsNegative(value.m_value) ? Zero : new BigUInteger(new BigInteger(unchecked(-value.m_value.GetBitLength())/* Can not overflow in practice */), default(ConstructorTags.Unchecked));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.PopCount(TSelf)" />
        public static BigUInteger PopCount(BigUInteger value) {
            return new BigUInteger(BigUInteger.IsNegative(value.m_value) ? -BigInteger.PopCount(~value.m_value) : BigInteger.PopCount(value.m_value), default(ConstructorTags.Unchecked));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateLeft(TSelf, int)" />
        public static BigUInteger RotateLeft(BigUInteger value, int rotateAmount) {
            if (0 <= rotateAmount) {
                var t = value.m_value << rotateAmount;
                if (Miscellaneous.Unlikely(BigInteger.IsNegative(value.m_value))) {
                    t |= (BigInteger.One << rotateAmount) - BigInteger.One;
                }
                return new BigUInteger(t, default(ConstructorTags.Unchecked));
            }
            return RotateRight(value, -rotateAmount);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.RotateRight(TSelf, int)" />
        public static BigUInteger RotateRight(BigUInteger value, int rotateAmount) {
            if (0 <= rotateAmount) {
                return value >> rotateAmount;
            }
            return RotateLeft(value, -rotateAmount);
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TrailingZeroCount(TSelf)" />
        public static BigUInteger TrailingZeroCount(BigUInteger value) {
            if (value.m_value.IsZero) {
                return Zero;
            }
            return new BigUInteger(BigInteger.TrailingZeroCount(value.m_value), default(ConstructorTags.Unchecked));
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadBigEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<BigUInteger>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out BigUInteger value) {
            var v = new BigInteger(source, isUnsigned, isBigEndian: true);
            if (!BigInteger.IsNegative(v)) {
                value = new BigUInteger(v, default(ConstructorTags.Unchecked));
                return true;
            }
            value = default;
            return true;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryReadLittleEndian(ReadOnlySpan{byte}, bool, out TSelf)" />
        static bool IBinaryInteger<BigUInteger>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out BigUInteger value) {
            var v = new BigInteger(source, isUnsigned, isBigEndian: false);
            if (!BigInteger.IsNegative(v)) {
                value = new BigUInteger(v, default(ConstructorTags.Unchecked));
                return true;
            }
            value = default;
            return true;
        }

        /// <inheritdoc cref="IBinaryInteger{TSelf}.GetShortestBitLength()" />
        int IBinaryInteger<BigUInteger>.GetShortestBitLength() {
            AssertValid();
            uint[]? bits = _bits;

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
        int IBinaryInteger<BigInteger>.GetByteCount() => GetGenericMathByteCount();

        /// <inheritdoc cref="IBinaryInteger{TSelf}.TryWriteBigEndian(Span{byte}, out int)" />
        bool IBinaryInteger<BigInteger>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            AssertValid();
            uint[]? bits = _bits;

            int byteCount = GetGenericMathByteCount();

            if (destination.Length >= byteCount) {
                if (bits is null) {
                    int value = BitConverter.IsLittleEndian ? BinaryPrimitives.ReverseEndianness(_sign) : _sign;
                    Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
                } else if (_sign >= 0) {
                    // When the value is positive, we simply need to copy all bits as big endian

                    ref byte startAddress = ref MemoryMarshal.GetReference(destination);
                    ref byte address = ref Unsafe.Add(ref startAddress, (bits.Length - 1) * sizeof(uint));

                    for (int i = 0; i < bits.Length; i++) {
                        uint part = bits[i];

                        if (BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Subtract(ref address, sizeof(uint));
                    }
                } else {
                    // When the value is negative, we need to copy the two's complement representation
                    // We'll do this "inline" to avoid needing to unnecessarily allocate.

                    ref byte startAddress = ref MemoryMarshal.GetReference(destination);
                    ref byte address = ref Unsafe.Add(ref startAddress, byteCount - sizeof(uint));

                    int i = 0;
                    uint part;

                    do {
                        // first do complement and +1 as long as carry is needed
                        part = ~bits[i] + 1;

                        if (BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Subtract(ref address, sizeof(uint));

                        i++;
                    }
                    while ((part == 0) && (i < bits.Length));

                    while (i < bits.Length) {
                        // now ones complement is sufficient
                        part = ~bits[i];

                        if (BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Subtract(ref address, sizeof(uint));

                        i++;
                    }

                    if (Unsafe.AreSame(ref address, ref startAddress)) {
                        // We need one extra part to represent the sign as the most
                        // significant bit of the two's complement value was 0.
                        Unsafe.WriteUnaligned(ref address, uint.MaxValue);
                    } else {
                        // Otherwise we should have been precisely one part behind address
                        Debug.Assert(Unsafe.AreSame(ref startAddress, ref Unsafe.Add(ref address, sizeof(uint))));
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
        bool IBinaryInteger<BigInteger>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            AssertValid();
            uint[]? bits = _bits;

            int byteCount = GetGenericMathByteCount();

            if (destination.Length >= byteCount) {
                if (bits is null) {
                    int value = BitConverter.IsLittleEndian ? _sign : BinaryPrimitives.ReverseEndianness(_sign);
                    Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), value);
                } else if (_sign >= 0) {
                    // When the value is positive, we simply need to copy all bits as little endian

                    ref byte address = ref MemoryMarshal.GetReference(destination);

                    for (int i = 0; i < bits.Length; i++) {
                        uint part = bits[i];

                        if (!BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Add(ref address, sizeof(uint));
                    }
                } else {
                    // When the value is negative, we need to copy the two's complement representation
                    // We'll do this "inline" to avoid needing to unnecessarily allocate.

                    ref byte address = ref MemoryMarshal.GetReference(destination);
                    ref byte lastAddress = ref Unsafe.Add(ref address, byteCount - sizeof(uint));

                    int i = 0;
                    uint part;

                    do {
                        // first do complement and +1 as long as carry is needed
                        part = ~bits[i] + 1;

                        if (!BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Add(ref address, sizeof(uint));

                        i++;
                    }
                    while ((part == 0) && (i < bits.Length));

                    while (i < bits.Length) {
                        // now ones complement is sufficient
                        part = ~bits[i];

                        if (!BitConverter.IsLittleEndian) {
                            part = BinaryPrimitives.ReverseEndianness(part);
                        }

                        Unsafe.WriteUnaligned(ref address, part);
                        address = ref Unsafe.Add(ref address, sizeof(uint));

                        i++;
                    }

                    if (Unsafe.AreSame(ref address, ref lastAddress)) {
                        // We need one extra part to represent the sign as the most
                        // significant bit of the two's complement value was 0.
                        Unsafe.WriteUnaligned(ref address, uint.MaxValue);
                    } else {
                        // Otherwise we should have been precisely one part ahead address
                        Debug.Assert(Unsafe.AreSame(ref lastAddress, ref Unsafe.Subtract(ref address, sizeof(uint))));
                    }
                }

                bytesWritten = byteCount;
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        private int GetGenericMathByteCount() {
            AssertValid();
            uint[]? bits = _bits;

            if (bits is null) {
                return sizeof(int);
            }

            int result = bits.Length * 4;

            if (_sign < 0) {
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

                if ((int)part >= 0) {
                    // When the most significant bit of the part is zero
                    // we need another part to represent the value.
                    result += sizeof(uint);
                }
            }

            return result;
        }

        //
        // IBinaryNumber
        //

        /// <inheritdoc cref="IBinaryNumber{TSelf}.IsPow2(TSelf)" />
        public static bool IsPow2(BigUInteger value) => value.IsPowerOfTwo;

        /// <inheritdoc cref="IBinaryNumber{TSelf}.Log2(TSelf)" />
        public static BigUInteger Log2(BigUInteger value) {
            return new BigUInteger(BigInteger.Log2(value.m_value), default(ConstructorTags.Unchecked));
        }

        //
        // IMultiplicativeIdentity
        //

        /// <inheritdoc cref="IMultiplicativeIdentity{TSelf, TResult}.MultiplicativeIdentity" />
        static BigUInteger IMultiplicativeIdentity<BigUInteger, BigUInteger>.MultiplicativeIdentity => One;

        //
        // INumber
        //

        /// <inheritdoc cref="INumber{TSelf}.Clamp(TSelf, TSelf, TSelf)" />
        public static BigUInteger Clamp(BigUInteger value, BigUInteger min, BigUInteger max) {
            if (min > max) {
                ThrowMinMaxException(min, max);
            }

            if (value < min) {
                return min;
            } else if (value > max) {
                return max;
            }
            return value;

            [DoesNotReturn]
            static void ThrowMinMaxException<T>(T min, T max) {
                throw new ArgumentException(SR.Format(SR.Argument_MinMaxValue, min, max));
            }
        }

        /// <inheritdoc cref="INumber{TSelf}.CopySign(TSelf, TSelf)" />
        public static BigUInteger CopySign(BigUInteger value, BigUInteger sign) {
            return new BigUInteger(BigInteger.CopySign(value.m_value, sign.m_value), default(ConstructorTags.Unchecked));
        }

        /// <inheritdoc cref="INumber{TSelf}.MaxNumber(TSelf, TSelf)" />
        static BigUInteger INumber<BigUInteger>.MaxNumber(BigUInteger x, BigUInteger y) => Max(x, y);

        /// <inheritdoc cref="INumber{TSelf}.MinNumber(TSelf, TSelf)" />
        static BigUInteger INumber<BigUInteger>.MinNumber(BigUInteger x, BigUInteger y) => Min(x, y);

        /// <inheritdoc cref="INumber{TSelf}.Sign(TSelf)" />
        static int INumber<BigUInteger>.Sign(BigUInteger value) {
            return value.Sign;
        }

        //
        // INumberBase
        //

        /// <inheritdoc cref="INumberBase{TSelf}.Radix" />
        static int INumberBase<BigUInteger>.Radix => 2;

        /// <inheritdoc cref="INumberBase{TSelf}.CreateChecked{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigUInteger CreateChecked<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            BigUInteger result;

            if (typeof(TOther) == typeof(BigUInteger)) {
                result = (BigUInteger)(object)value;
            } else if (typeof(TOther) == typeof(BigInteger)) {
                result = checked((BigUInteger)unchecked((BigInteger)(object)value));
            } else if (!TryConvertFromChecked(value, out result) && !TOther.TryConvertToChecked(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateSaturating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigUInteger CreateSaturating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            BigUInteger result;

            if (typeof(TOther) == typeof(BigUInteger)) {
                result = (BigUInteger)(object)value;
            } else if(typeof(TOther) == typeof(BigInteger)) {
                var v = (BigInteger)(object)value;
                result = BigInteger.IsNegative(v) ? Zero : unchecked((BigUInteger)v);
            } else if (!TryConvertFromSaturating(value, out result) && !TOther.TryConvertToSaturating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.CreateTruncating{TOther}(TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigUInteger CreateTruncating<TOther>(TOther value)
            where TOther : INumberBase<TOther> {
            BigUInteger result;

            if (typeof(TOther) == typeof(BigUInteger)) {
                result = (BigUInteger)(object)value;
            } else if (typeof(TOther) == typeof(BigInteger)) {
                result = unchecked((BigUInteger)unchecked((BigInteger)(object)value));
            } else if (!TryConvertFromTruncating(value, out result) && !TOther.TryConvertToTruncating(value, out result)) {
                ThrowHelper.ThrowNotSupportedException();
            }

            return result;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsCanonical(TSelf)" />
        static bool INumberBase<BigUInteger>.IsCanonical(BigUInteger value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsComplexNumber(TSelf)" />
        static bool INumberBase<BigUInteger>.IsComplexNumber(BigUInteger value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsEvenInteger(TSelf)" />
        public static bool IsEvenInteger(BigUInteger value) {
            return value.IsFiniteInternal && value.m_value.IsEven;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsFinite(TSelf)" />
        static bool INumberBase<BigUInteger>.IsFinite(BigUInteger value) => value.IsFiniteInternal;

        /// <inheritdoc cref="INumberBase{TSelf}.IsImaginaryNumber(TSelf)" />
        static bool INumberBase<BigUInteger>.IsImaginaryNumber(BigUInteger value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInfinity(TSelf)" />
        static bool INumberBase<BigUInteger>.IsInfinity(BigUInteger value) => !value.IsFiniteInternal;

        /// <inheritdoc cref="INumberBase{TSelf}.IsInteger(TSelf)" />
        static bool INumberBase<BigUInteger>.IsInteger(BigUInteger value) => value.IsFiniteInternal;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNaN(TSelf)" />
        static bool INumberBase<BigUInteger>.IsNaN(BigUInteger value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegative(TSelf)" />
        public static bool IsNegative(BigUInteger value) {
            return false;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsNegativeInfinity(TSelf)" />
        static bool INumberBase<BigUInteger>.IsNegativeInfinity(BigUInteger value) => !value.IsFiniteInternal;

        /// <inheritdoc cref="INumberBase{TSelf}.IsNormal(TSelf)" />
        static bool INumberBase<BigUInteger>.IsNormal(BigUInteger value) => BigInteger.IsPositive(value.m_value);

        /// <inheritdoc cref="INumberBase{TSelf}.IsOddInteger(TSelf)" />
        public static bool IsOddInteger(BigUInteger value) {
            return value.IsFiniteInternal && !value.m_value.IsEven;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositive(TSelf)" />
        public static bool IsPositive(BigUInteger value) {
            return BigInteger.IsPositive(value.m_value);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.IsPositiveInfinity(TSelf)" />
        static bool INumberBase<BigUInteger>.IsPositiveInfinity(BigUInteger value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsRealNumber(TSelf)" />
        static bool INumberBase<BigUInteger>.IsRealNumber(BigUInteger value) => true;

        /// <inheritdoc cref="INumberBase{TSelf}.IsSubnormal(TSelf)" />
        static bool INumberBase<BigUInteger>.IsSubnormal(BigUInteger value) => false;

        /// <inheritdoc cref="INumberBase{TSelf}.IsZero(TSelf)" />
        static bool INumberBase<BigUInteger>.IsZero(BigUInteger value) {
            return value.m_value.IsZero;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitude(TSelf, TSelf)" />
        public static BigUInteger MaxMagnitude(BigUInteger x, BigUInteger y) {
            return Max(x, y);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MaxMagnitudeNumber(TSelf, TSelf)" />
        static BigUInteger INumberBase<BigUInteger>.MaxMagnitudeNumber(BigUInteger x, BigUInteger y) => MaxMagnitude(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitude(TSelf, TSelf)" />
        public static BigUInteger MinMagnitude(BigUInteger x, BigUInteger y) {
            return Min(x, y);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.MinMagnitudeNumber(TSelf, TSelf)" />
        static BigUInteger INumberBase<BigUInteger>.MinMagnitudeNumber(BigUInteger x, BigUInteger y) => MinMagnitude(x, y);

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigUInteger>.TryConvertFromChecked<TOther>(TOther value, out BigUInteger result) => TryConvertFromChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out BigUInteger result)
            where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(byte)) {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualValue = (decimal)(object)value;
                result = (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = checked((BigInteger)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = checked((BigInteger)actualValue);
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
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = actualValue;
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
                result = checked((BigInteger)actualValue);
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualValue = (UInt128)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigUInteger>.TryConvertFromSaturating<TOther>(TOther value, out BigUInteger result) => TryConvertFromSaturating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out BigUInteger result)
            where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(byte)) {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualValue = (decimal)(object)value;
                result = (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = double.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = Half.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
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
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = actualValue;
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
                result = float.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualValue = (UInt128)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigUInteger>.TryConvertFromTruncating<TOther>(TOther value, out BigUInteger result) => TryConvertFromTruncating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out BigUInteger result)
            where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(byte)) {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualValue = (decimal)(object)value;
                result = (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = double.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = Half.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
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
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualValue = (Int128)(object)value;
                result = actualValue;
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
                result = float.IsNaN(actualValue) ? Zero : (BigInteger)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualValue = (UInt128)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<BigUInteger>.TryConvertToChecked<TOther>(BigUInteger value, [MaybeNullWhen(false)] out TOther result) {
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
        static bool INumberBase<BigUInteger>.TryConvertToSaturating<TOther>(BigUInteger value, [MaybeNullWhen(false)] out TOther result) {
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
        static bool INumberBase<BigUInteger>.TryConvertToTruncating<TOther>(BigUInteger value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(byte)) {
                byte actualResult;

                if (value._bits is not null) {
                    uint bits = value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (byte)bits;
                } else {
                    actualResult = (byte)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualResult;

                if (value._bits is not null) {
                    uint bits = value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (char)bits;
                } else {
                    actualResult = (char)value._sign;
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
                    actualResult = IsNegative(value) ? (short)(~value._bits[0] + 1) : (short)value._bits[0];
                } else {
                    actualResult = (short)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? (int)(~value._bits[0] + 1) : (int)value._bits[0];
                } else {
                    actualResult = value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult;

                if (value._bits is not null) {
                    ulong bits = 0;

                    if (value._bits.Length >= 2) {
                        bits = value._bits[1];
                        bits <<= 32;
                    }

                    bits |= value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (long)bits;
                } else {
                    actualResult = value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult;

                if (value._bits is not null) {
                    ulong lowerBits = 0;
                    ulong upperBits = 0;

                    if (value._bits.Length >= 4) {
                        upperBits = value._bits[3];
                        upperBits <<= 32;
                    }

                    if (value._bits.Length >= 3) {
                        upperBits |= value._bits[2];
                    }

                    if (value._bits.Length >= 2) {
                        lowerBits = value._bits[1];
                        lowerBits <<= 32;
                    }

                    lowerBits |= value._bits[0];

                    UInt128 bits = new UInt128(upperBits, lowerBits);

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (Int128)bits;
                } else {
                    actualResult = value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualResult;

                if (value._bits is not null) {
                    nuint bits = 0;

                    if (Environment.Is64BitProcess && (value._bits.Length >= 2)) {
                        bits = value._bits[1];
                        bits <<= 32;
                    }

                    bits |= value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (nint)bits;
                } else {
                    actualResult = value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Complex)) {
                Complex actualResult = (Complex)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult;

                if (value._bits is not null) {
                    actualResult = IsNegative(value) ? (sbyte)(~value._bits[0] + 1) : (sbyte)value._bits[0];
                } else {
                    actualResult = (sbyte)value._sign;
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
                    uint bits = value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = (ushort)bits;
                } else {
                    actualResult = (ushort)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualResult;

                if (value._bits is not null) {
                    uint bits = value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                } else {
                    actualResult = (uint)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualResult;

                if (value._bits is not null) {
                    ulong bits = 0;

                    if (value._bits.Length >= 2) {
                        bits = value._bits[1];
                        bits <<= 32;
                    }

                    bits |= value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                } else {
                    actualResult = (ulong)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(UInt128)) {
                UInt128 actualResult;

                if (value._bits is not null) {
                    ulong lowerBits = 0;
                    ulong upperBits = 0;

                    if (value._bits.Length >= 4) {
                        upperBits = value._bits[3];
                        upperBits <<= 32;
                    }

                    if (value._bits.Length >= 3) {
                        upperBits |= value._bits[2];
                    }

                    if (value._bits.Length >= 2) {
                        lowerBits = value._bits[1];
                        lowerBits <<= 32;
                    }

                    lowerBits |= value._bits[0];

                    UInt128 bits = new UInt128(upperBits, lowerBits);

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                } else {
                    actualResult = (UInt128)value._sign;
                }

                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualResult;

                if (value._bits is not null) {
                    nuint bits = 0;

                    if (Environment.Is64BitProcess && (value._bits.Length >= 2)) {
                        bits = value._bits[1];
                        bits <<= 32;
                    }

                    bits |= value._bits[0];

                    if (IsNegative(value)) {
                        bits = ~bits + 1;
                    }

                    actualResult = bits;
                } else {
                    actualResult = (nuint)value._sign;
                }

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
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out BigInteger result) => TryParse(s, NumberStyles.Integer, provider, out result);

        //
        // IShiftOperators
        //

        /// <inheritdoc cref="IShiftOperators{TSelf, TOther, TResult}.op_UnsignedRightShift(TSelf, TOther)" />
        public static BigUInteger operator >>>(BigUInteger value, int shiftAmount) {
            return value >> shiftAmount;
        }

        //
        // ISignedNumber
        //

        /*
        /// <inheritdoc cref="ISignedNumber{TSelf}.NegativeOne" />
        static BigInteger ISignedNumber<BigInteger>.NegativeOne => MinusOne;
        */

        //
        // ISpanParsable
        //

        /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
        public static BigUInteger Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Integer, provider);

        /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out BigUInteger result) => TryParse(s, NumberStyles.Integer, provider, out result);
    }
}
#endif