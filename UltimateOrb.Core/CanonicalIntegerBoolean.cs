using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UltimateOrb.Utilities.InterfaceReadOnlyExtensions.System;

namespace UltimateOrb {

    /// <summary>
    /// <para>
    /// Represents a Boolean (true or false) value. Internally, this type uses an integer (<c><see cref="int" /></c>/<c><see cref="uint" /></c>).
    /// </para>
    /// <para>
    /// The internal representation of this boolean type is assumed to be canonical (0 for false and 1 for true) and remains so. Breaking this invariant may cause undefined behavior.<br />
    /// See <see cref="Bool" /> for a boolean type that handles non-canonical representations.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This type is blittable.<br />
    /// The size of a value of this type is the same as sizeof(int).
    /// </remarks>
    [DebuggerDisplay("{ToDebugDisplayString(),nq}")]
    [Serializable]
    public readonly struct CanonicalIntegerBoolean : IComparable, IComparable<CanonicalIntegerBoolean>, IConvertible, IEquatable<CanonicalIntegerBoolean>, ISerializable {

        readonly int Value;

        /// <summary>
        /// A standard <see cref="System.Boolean"/> value to represent the current <see cref="CanonicalIntegerBoolean"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool StandardValue {

            get => this;
        }

        /// <summary>
        /// The internal representation of the current <see cref="CanonicalIntegerBoolean"/>.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public int IntegerValue {

            get => Value;
        }

        internal CanonicalIntegerBoolean(int value) {
            Debug.Assert(unchecked((uint)value) <= 1);
            Value = value;
        }

        internal CanonicalIntegerBoolean(uint value) {
            Debug.Assert(value <= 1);
            Value = unchecked((int)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        internal static int ToBooleanIntegerSafe(bool value) {
            return Unsafe.As<bool, byte>(ref Unsafe.AsRef(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal CanonicalIntegerBoolean(bool value) : this(ToBooleanIntegerSafe(value)) {
        }

        CanonicalIntegerBoolean(SerializationInfo info, StreamingContext context) {
            Value = (bool)info.GetValue("m_value", typeof(bool))! ? TrueValue : FalseValue;
        }

        internal const string TrueLiteral = "True";

        internal const string FalseLiteral = "False";

        /// <summary>
        /// Represents the <see cref="CanonicalIntegerBoolean"/> value true as a string. This field is read-only.
        /// </summary>
        public static readonly string TrueString = TrueLiteral;

        /// <summary>
        /// Represents the <see cref="CanonicalIntegerBoolean"/> value false as a string. This field is read-only.
        /// </summary>
        public static readonly string FalseString = FalseLiteral;

        /// <summary>
        /// Retrieves the internal representation of the <see cref="CanonicalIntegerBoolean"/> value.
        /// </summary>
        /// <param name="value">The value to retrieve internal representation.</param>
        public static explicit operator int(CanonicalIntegerBoolean value) {
            return value.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            CGT.Un
            Ret
        ")]
        internal static int ToBooleanIntegerSafe(int value) {
            return unchecked((uint)value > 0) ? 1 : 0;
        }

        /// <summary>
        /// <para>
        /// Converts the value of the specified integer to a <see cref="CanonicalIntegerBoolean"/> value.
        /// </para>
        /// <para>
        /// <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the integer is 0; otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        /// </para>
        /// </summary>
        /// <param name="value">The specified integer.</param>
        public static explicit operator CanonicalIntegerBoolean(int value) {
            return new CanonicalIntegerBoolean(ToBooleanIntegerSafe(value));
        }

        /// <summary>
        /// Retrive the internal representation of the <see cref="CanonicalIntegerBoolean"/> value.
        /// </summary>
        /// <param name="value">The value.</param>
        [CLSCompliant(false)]
        public static explicit operator uint(CanonicalIntegerBoolean value) {
            return unchecked((uint)value.Value);
        }

        /// <summary>
        /// <para>
        /// Converts the value of the specified integer to a <see cref="CanonicalIntegerBoolean"/> value.
        /// </para>
        /// <para>
        /// <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the integer is 0; otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        /// </para>
        /// </summary>
        /// <param name="value">The specified integer.</param>
        [CLSCompliant(false)]
        public static explicit operator CanonicalIntegerBoolean(uint value) {
            return new CanonicalIntegerBoolean(ToBooleanIntegerSafe(unchecked((int)value)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        internal static bool ToStandardBooleanUnsafe(int value) {
            return Unsafe.As<byte, bool>(ref Unsafe.AsRef((byte)value));
        }

        /// <summary>
        /// <para>
        /// Converts the <see cref="CanonicalIntegerBoolean"/> value to a standard <see cref="bool"/> value.
        /// </para>
        /// <para>
        /// No checks will be performed in release mode.
        /// </para>
        /// </summary>
        /// <param name="value">The specified integer.</param>
        public static implicit operator bool(CanonicalIntegerBoolean value) {
            return ToStandardBooleanUnsafe(value.Value);
        }

        /// <summary>
        /// <para>
        /// Converts the standard <see cref="bool"/> value to a <see cref="CanonicalIntegerBoolean"/> value .
        /// </para>
        /// <para>
        /// This operation is safe on non-canonical inputs.
        /// </para>
        /// </summary>
        /// <param name="value">The specified integer.</param>
        public static implicit operator CanonicalIntegerBoolean(bool value) {
            return new CanonicalIntegerBoolean(value ? 1 : 0);
        }

        /// <summary>
        /// Computes the logical negation (NOT) of the <paramref name="value"/>.
        /// </summary>
        public static CanonicalIntegerBoolean operator !(CanonicalIntegerBoolean value) {
            return new CanonicalIntegerBoolean(value.Value ^ 1);
        }

        /// <summary>
        /// Computes the logical conjunction (AND) of the <paramref name="first"/> and <paramref name="second"/> values.
        /// </summary>
        public static CanonicalIntegerBoolean operator &(CanonicalIntegerBoolean first, CanonicalIntegerBoolean second) {
            return new CanonicalIntegerBoolean(first.Value & second.Value);
        }

        /// <summary>
        /// Computes the logical disjunction (OR) of the <paramref name="first"/> and <paramref name="second"/> values.
        /// </summary>
        public static CanonicalIntegerBoolean operator |(CanonicalIntegerBoolean first, CanonicalIntegerBoolean second) {
            return new CanonicalIntegerBoolean(first.Value | second.Value);
        }

        /// <summary>
        /// Computes the logical exclusive disjunction (XOR) of the <paramref name="first"/> and <paramref name="second"/> values.
        /// </summary>
        public static CanonicalIntegerBoolean operator ^(CanonicalIntegerBoolean first, CanonicalIntegerBoolean second) {
            return new CanonicalIntegerBoolean(first.Value ^ second.Value);
        }

        /// <summary>
        /// Determines whether the <see cref="CanonicalIntegerBoolean"/> value is true.
        /// </summary>
        public static bool operator true(CanonicalIntegerBoolean value) {
            return ToStandardBooleanUnsafe(value.Value);
        }

        /// <summary>
        /// Determines whether the <see cref="CanonicalIntegerBoolean"/> value is false.
        /// </summary>
        public static bool operator false(CanonicalIntegerBoolean value) {
            return unchecked((uint)value.Value) > 0;
        }

        internal string ToDebugDisplayString() {
            return $@"{{ {StandardValue}: 0X{Value:x8} }}";
        }

        /// <summary>
        /// Converts the current <see cref="CanonicalIntegerBoolean"/> value to a <see cref="string"/> representation.
        /// </summary>
        public override string ToString() {
            return this ? TrueLiteral : FalseLiteral;
        }

        #region IConvertible implementations
        /// <summary>
        /// Gets the <see cref="TypeCode"/> of <see cref="CanonicalIntegerBoolean"/>.
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode() {
            return TypeCode.Boolean;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return 0 != Value;
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Bool", "Char"));
            throw new InvalidCastException("Conversion from type 'Bool' to type 'Char' is not valid.");
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            return Convert.ToSByte(StandardValue);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            return Convert.ToByte(StandardValue);
        }

        short IConvertible.ToInt16(IFormatProvider? provider) {
            return Convert.ToInt16(StandardValue);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider) {
            return Convert.ToUInt16(StandardValue);
        }

        int IConvertible.ToInt32(IFormatProvider? provider) {
            return Convert.ToInt32(StandardValue);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider) {
            return Convert.ToUInt32(StandardValue);
        }

        long IConvertible.ToInt64(IFormatProvider? provider) {
            return Convert.ToInt64(StandardValue);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider) {
            return Convert.ToUInt64(StandardValue);
        }

        float IConvertible.ToSingle(IFormatProvider? provider) {
            return Convert.ToSingle(StandardValue);
        }

        double IConvertible.ToDouble(IFormatProvider? provider) {
            return Convert.ToDouble(StandardValue);
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return Convert.ToDecimal(StandardValue);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Bool", "DateTime"));
            throw new InvalidCastException("Conversion from type 'Bool' to type 'DateTime' is not valid.");
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            // Convert.DefaultToType((IConvertible)this, type, provider);
            return StandardValue.ToType(type, provider);
        }

        /// <inheritdoc/>
        public string ToString(IFormatProvider? provider) {
            return ToString();
        }
        #endregion

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue("m_value", StandardValue);
        }

        /// <summary>
        /// Formats the current <see cref="CanonicalIntegerBoolean"/> value.
        /// </summary>
        public bool TryFormat(Span<char> destination, out int charsWritten) {
            if (StandardValue) {
                if ((uint)destination.Length > 3) { // uint cast, per https://github.com/dotnet/runtime/issues/10596
                    destination[0] = 'T';
                    destination[1] = 'r';
                    destination[2] = 'u';
                    destination[3] = 'e';
                    charsWritten = 4;
                    return true;
                }
            } else {
                if ((uint)destination.Length > 4) {
                    destination[0] = 'F';
                    destination[1] = 'a';
                    destination[2] = 'l';
                    destination[3] = 's';
                    destination[4] = 'e';
                    charsWritten = 5;
                    return true;
                }
            }
            charsWritten = 0;
            return false;
        }

        /// <inheritdoc/>
        public override bool Equals([NotNullWhen(true)] object? obj) {
            // If it's not a boolean, we're definitely not equal
            if (obj is CanonicalIntegerBoolean value) {
                return Equals(value);
            }
            return false;
        }

        /// <inheritdoc/>
        public bool Equals(CanonicalIntegerBoolean obj) {
            return Value == obj.Value;
        }

        /// <inheritdoc/>
        public int CompareTo(object? obj) {
            if (obj == null) {
                return 1;
            }
            if (obj is CanonicalIntegerBoolean value) {
                return CompareTo(value);
            }
            throw new ArgumentException("Object must be of type Bool.");
        }

        /// <inheritdoc/>
        public int CompareTo(CanonicalIntegerBoolean value) {
            return Value - value.Value;
        }

        internal static bool IsTrueStringIgnoreCase(ReadOnlySpan<char> value) {
            return value.Length == 4 &&
                (value[0] == 't' || value[0] == 'T') &&
                (value[1] == 'r' || value[1] == 'R') &&
                (value[2] == 'u' || value[2] == 'U') &&
                (value[3] == 'e' || value[3] == 'E');
        }

        internal static bool IsFalseStringIgnoreCase(ReadOnlySpan<char> value) {
            return value.Length == 5 &&
                (value[0] == 'f' || value[0] == 'F') &&
                (value[1] == 'a' || value[1] == 'A') &&
                (value[2] == 'l' || value[2] == 'L') &&
                (value[3] == 's' || value[3] == 'S') &&
                (value[4] == 'e' || value[4] == 'E');
        }

        /// <summary>
        /// Determines whether a String represents true or false.
        /// </summary>
        public static CanonicalIntegerBoolean Parse(string value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return Parse(value.AsSpan());
        }

        /// <summary>
        /// Determines whether a String represents true or false.
        /// </summary>
        public static CanonicalIntegerBoolean Parse(ReadOnlySpan<char> value) =>
            TryParse(value, out var result) ? result : throw new FormatException(string.Format("String '{0}' was not recognized as a valid CanonicalIntegerBoolean.", new string(value)));

        /// <summary>
        /// Determines whether a String represents true or false.
        /// </summary>
        public static bool TryParse([NotNullWhen(true)] string? value, out CanonicalIntegerBoolean result) {
            if (value == null) {
                result = default;
                return false;
            }

            return TryParse(value.AsSpan(), out result);
        }

        /// <summary>
        /// Represents internal integer value of <see cref="True"/>.
        /// </summary>
        public const int TrueValue = 1;

        /// <summary>
        /// Represents <c>(<see cref="CanonicalIntegerBoolean"/>)true</c>.
        /// </summary>
        public static CanonicalIntegerBoolean True {

            get => new(TrueValue);
        }

        /// <summary>
        /// Represents internal integer value of <see cref="False"/>.
        /// </summary>
        public const int FalseValue = 0;

        /// <summary>
        /// Represents <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        /// </summary>
        public static CanonicalIntegerBoolean False {

            get => new(FalseValue);
        }

        /// <summary>
        /// Determines whether a String represents true or false.
        /// </summary>
        public static bool TryParse(ReadOnlySpan<char> value, out CanonicalIntegerBoolean result) {
            if (IsTrueStringIgnoreCase(value)) {
                result = new CanonicalIntegerBoolean(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new CanonicalIntegerBoolean(FalseValue);
                return true;
            }

            // Special case: Trim whitespace as well as null characters.
            value = TrimWhiteSpaceAndNull(value);

            if (IsTrueStringIgnoreCase(value)) {
                result = new CanonicalIntegerBoolean(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new CanonicalIntegerBoolean(FalseValue);
                return true;
            }

            result = new CanonicalIntegerBoolean(FalseValue);
            return false;
        }

        private static ReadOnlySpan<char> TrimWhiteSpaceAndNull(ReadOnlySpan<char> value) {
            const char nullChar = (char)0x0000;

            int start = 0;
            while (start < value.Length) {
                if (!char.IsWhiteSpace(value[start]) && value[start] != nullChar) {
                    break;
                }
                start++;
            }

            int end = value.Length - 1;
            while (end >= start) {
                if (!char.IsWhiteSpace(value[end]) && value[end] != nullChar) {
                    break;
                }
                end--;
            }

            return value.Slice(start, end - start + 1);
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return Value;
        }

        /// <summary>
        /// Compares two <see cref="CanonicalIntegerBoolean"/> values.
        /// </summary>
        public static bool operator ==(CanonicalIntegerBoolean left, CanonicalIntegerBoolean right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="CanonicalIntegerBoolean"/> values.
        /// </summary>
        public static bool operator !=(CanonicalIntegerBoolean left, CanonicalIntegerBoolean right) {
            return !(left == right);
        }
    }
}
