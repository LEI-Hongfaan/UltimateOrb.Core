using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    public readonly partial struct Boolean8 : IComparable, IComparable<Boolean8>, IConvertible, IEquatable<Boolean8>, ISerializable {

        //
        // Member Variables
        //
        private readonly byte m_value; // Do not rename (binary serialization)

        // The true value.
        //
        internal const byte TrueValue = 1;

        // The false value.
        //
        internal const byte FalseValue = 0;

        //
        // Internal Constants are real consts for performance.
        //

        // The internal string representation of true.
        //
        internal const string TrueLiteral = "True";

        // The internal string representation of false.
        //
        internal const string FalseLiteral = "False";

        private Boolean8(byte value) {
            m_value = value;
        }

        Boolean8(SerializationInfo info, StreamingContext context) {
            m_value = (bool)info.GetValue(nameof(m_value), typeof(bool))! ? TrueValue : FalseValue;
        }

        internal static readonly Boolean8 True = new Boolean8(TrueValue);

        internal static readonly Boolean8 False = new Boolean8(FalseValue);

        public static implicit operator Boolean8(bool value) {
            return new Boolean8(value ? TrueValue : FalseValue);
        }

        public static implicit operator bool(Boolean8 value) {
            return value.StandardValue;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool StandardValue {

            get => 0 != m_value;
        }

        //
        // Public Constants
        //

        // The public string representation of true.
        //
        public static readonly string TrueString = TrueLiteral;

        // The public string representation of false.
        //
        public static readonly string FalseString = FalseLiteral;

        //
        // Overriden Instance Methods
        //
        /*=================================GetHashCode==================================
        **Args:  None
        **Returns: 1 or 0 depending on whether this instance represents true or false.
        **Exceptions: None
        **Overriden From: Value
        ==============================================================================*/
        // Provides a hash code for this instance.
        public override int GetHashCode() {
            return StandardValue ? unchecked((int)TrueValue) : unchecked((int)FalseValue);
        }

        /*===================================ToString===================================
        **Args: None
        **Returns:  "True" or "False" depending on the state of the boolean.
        **Exceptions: None.
        ==============================================================================*/
        // Converts the boolean value of this instance to a String.
        public override string ToString() {
            if (false == StandardValue) {
                return FalseLiteral;
            }
            return TrueLiteral;
        }

        public string ToString(IFormatProvider? provider) {
            return ToString();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten) {
            if (StandardValue) {
                if (unchecked((uint)destination.Length) > 3) { // uint cast, per https://github.com/dotnet/runtime/issues/10596
                    destination[0] = 'T';
                    destination[1] = 'r';
                    destination[2] = 'u';
                    destination[3] = 'e';
                    charsWritten = 4;
                    return true;
                }
            } else {
                if (unchecked((uint)destination.Length) > 4) {
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

        // Determines whether two Boolean objects are equal.
        public override bool Equals([NotNullWhen(true)] object? obj) {
            // If it's not a boolean, we're definitely not equal
            if (!(obj is Boolean8 value)) {
                return false;
            }

            return StandardValue == value.StandardValue;
        }

        // [NonVersionable]
        public bool Equals(Boolean8 obj) {
            return (0 == m_value) == (0 == obj.m_value);
        }

        // Compares this object to another object, returning an integer that
        // indicates the relationship. For booleans, false sorts before true.
        // null is considered to be less than any instance.
        // If object is not of type boolean, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
        public int CompareTo(object? obj) {
            if (obj == null) {
                return 1;
            }
            if (!(obj is Boolean8)) {
                throw new ArgumentException("Object must be of type Boolean8.");
            }

            if (StandardValue == ((Boolean8)obj).StandardValue) {
                return 0;
            } else if (StandardValue == false) {
                return -1;
            }
            return 1;
        }

        public int CompareTo(Boolean8 value) {
            if (StandardValue == value.StandardValue) {
                return 0;
            } else if (StandardValue == false) {
                return -1;
            }
            return 1;
        }

        //
        // Static Methods
        //

        // Custom string compares for early application use by config switches, etc
        //
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

        // Determines whether a String represents true or false.
        //
        public static Boolean8 Parse(string value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return Parse(value.AsSpan());
        }

        public static Boolean8 Parse(ReadOnlySpan<char> value) =>
            TryParse(value, out var result) ? result : throw new FormatException(string.Format("String '{0}' was not recognized as a valid Boolean8.", new string(value)));

        // Determines whether a String represents true or false.
        //
        public static bool TryParse([NotNullWhen(true)] string? value, out Boolean8 result) {
            if (value == null) {
                result = new Boolean8(FalseValue);
                return false;
            }

            return TryParse(value.AsSpan(), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> value, out Boolean8 result) {
            if (IsTrueStringIgnoreCase(value)) {
                result = new Boolean8(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new Boolean8(FalseValue);
                return true;
            }

            // Special case: Trim whitespace as well as null characters.
            value = TrimWhiteSpaceAndNull(value);

            if (IsTrueStringIgnoreCase(value)) {
                result = new Boolean8(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new Boolean8(FalseValue);
                return true;
            }

            result = new Boolean8(FalseValue);
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

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode() {
            return TypeCode.Boolean;
        }

        Boolean IConvertible.ToBoolean(IFormatProvider? provider) {
            return 0 != m_value;
        }

        Char IConvertible.ToChar(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Boolean8", "Char"));
            throw new InvalidCastException("Conversion from type 'Boolean8' to type 'Char' is not valid.");
        }

        SByte IConvertible.ToSByte(IFormatProvider? provider) {
            return Convert.ToSByte(StandardValue);
        }

        Byte IConvertible.ToByte(IFormatProvider? provider) {
            return Convert.ToByte(StandardValue);
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            return Convert.ToInt16(StandardValue);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            return Convert.ToUInt16(StandardValue);
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            return Convert.ToInt32(StandardValue);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            return Convert.ToUInt32(StandardValue);
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            return Convert.ToInt64(StandardValue);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            return Convert.ToUInt64(StandardValue);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return Convert.ToSingle(StandardValue);
        }

        Double IConvertible.ToDouble(IFormatProvider? provider) {
            return Convert.ToDouble(StandardValue);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return Convert.ToDecimal(StandardValue);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Boolean8", "DateTime"));
            throw new InvalidCastException("Conversion from type 'Boolean8' to type 'DateTime' is not valid.");
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            // Convert.DefaultToType((IConvertible)this, type, provider);
            return ((IConvertible)StandardValue).ToType(type, provider);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(m_value), StandardValue);
        }

        public static bool operator ==(Boolean8 first, Boolean8 second) {
            return first.Equals(second);
        }

        public static bool operator !=(Boolean8 first, Boolean8 second) {
            return !(first == second);
        }
    }

    public readonly partial struct Boolean32 : IComparable, IComparable<Boolean32>, IConvertible, IEquatable<Boolean32>, ISerializable {

        //
        // Member Variables
        //
        private readonly uint m_value; // Do not rename (binary serialization)

        // The true value.
        //
        internal const uint TrueValue = 1;

        // The false value.
        //
        internal const uint FalseValue = 0;

        //
        // Internal Constants are real consts for performance.
        //

        // The internal string representation of true.
        //
        internal const string TrueLiteral = "True";

        // The internal string representation of false.
        //
        internal const string FalseLiteral = "False";

        private Boolean32(uint value) {
            m_value = value;
        }

        Boolean32(SerializationInfo info, StreamingContext context) {
            m_value = (bool)info.GetValue(nameof(m_value), typeof(bool))! ? TrueValue : FalseValue;
        }

        internal static readonly Boolean32 True = new Boolean32(TrueValue);

        internal static readonly Boolean32 False = new Boolean32(FalseValue);

        public static implicit operator Boolean32(bool value) {
            return new Boolean32(value ? TrueValue : FalseValue);
        }

        public static implicit operator bool(Boolean32 value) {
            return value.StandardValue;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool StandardValue {

            get => 0 != m_value;
        }

        //
        // Public Constants
        //

        // The public string representation of true.
        //
        public static readonly string TrueString = TrueLiteral;

        // The public string representation of false.
        //
        public static readonly string FalseString = FalseLiteral;

        //
        // Overriden Instance Methods
        //
        /*=================================GetHashCode==================================
        **Args:  None
        **Returns: 1 or 0 depending on whether this instance represents true or false.
        **Exceptions: None
        **Overriden From: Value
        ==============================================================================*/
        // Provides a hash code for this instance.
        public override int GetHashCode() {
            return StandardValue ? unchecked((int)TrueValue) : unchecked((int)FalseValue);
        }

        /*===================================ToString===================================
        **Args: None
        **Returns:  "True" or "False" depending on the state of the boolean.
        **Exceptions: None.
        ==============================================================================*/
        // Converts the boolean value of this instance to a String.
        public override string ToString() {
            if (false == StandardValue) {
                return FalseLiteral;
            }
            return TrueLiteral;
        }

        public string ToString(IFormatProvider? provider) {
            return ToString();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten) {
            if (StandardValue) {
                if (unchecked((uint)destination.Length) > 3) { // uint cast, per https://github.com/dotnet/runtime/issues/10596
                    destination[0] = 'T';
                    destination[1] = 'r';
                    destination[2] = 'u';
                    destination[3] = 'e';
                    charsWritten = 4;
                    return true;
                }
            } else {
                if (unchecked((uint)destination.Length) > 4) {
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

        // Determines whether two Boolean objects are equal.
        public override bool Equals([NotNullWhen(true)] object? obj) {
            // If it's not a boolean, we're definitely not equal
            if (!(obj is Boolean32 value)) {
                return false;
            }

            return StandardValue == value.StandardValue;
        }

        // [NonVersionable]
        public bool Equals(Boolean32 obj) {
            return (0 == m_value) == (0 == obj.m_value);
        }

        // Compares this object to another object, returning an integer that
        // indicates the relationship. For booleans, false sorts before true.
        // null is considered to be less than any instance.
        // If object is not of type boolean, this method throws an ArgumentException.
        //
        // Returns a value less than zero if this  object
        //
        public int CompareTo(object? obj) {
            if (obj == null) {
                return 1;
            }
            if (!(obj is Boolean32)) {
                throw new ArgumentException("Object must be of type Boolean32.");
            }

            if (StandardValue == ((Boolean32)obj).StandardValue) {
                return 0;
            } else if (StandardValue == false) {
                return -1;
            }
            return 1;
        }

        public int CompareTo(Boolean32 value) {
            if (StandardValue == value.StandardValue) {
                return 0;
            } else if (StandardValue == false) {
                return -1;
            }
            return 1;
        }

        //
        // Static Methods
        //

        // Custom string compares for early application use by config switches, etc
        //
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

        // Determines whether a String represents true or false.
        //
        public static Boolean32 Parse(string value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            return Parse(value.AsSpan());
        }

        public static Boolean32 Parse(ReadOnlySpan<char> value) =>
            TryParse(value, out var result) ? result : throw new FormatException(string.Format("String '{0}' was not recognized as a valid Boolean32.", new string(value)));

        // Determines whether a String represents true or false.
        //
        public static bool TryParse([NotNullWhen(true)] string? value, out Boolean32 result) {
            if (value == null) {
                result = new Boolean32(FalseValue);
                return false;
            }

            return TryParse(value.AsSpan(), out result);
        }

        public static bool TryParse(ReadOnlySpan<char> value, out Boolean32 result) {
            if (IsTrueStringIgnoreCase(value)) {
                result = new Boolean32(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new Boolean32(FalseValue);
                return true;
            }

            // Special case: Trim whitespace as well as null characters.
            value = TrimWhiteSpaceAndNull(value);

            if (IsTrueStringIgnoreCase(value)) {
                result = new Boolean32(TrueValue);
                return true;
            }

            if (IsFalseStringIgnoreCase(value)) {
                result = new Boolean32(FalseValue);
                return true;
            }

            result = new Boolean32(FalseValue);
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

        //
        // IConvertible implementation
        //

        public TypeCode GetTypeCode() {
            return TypeCode.Boolean;
        }

        Boolean IConvertible.ToBoolean(IFormatProvider? provider) {
            return 0 != m_value;
        }

        Char IConvertible.ToChar(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Boolean32", "Char"));
            throw new InvalidCastException("Conversion from type 'Boolean32' to type 'Char' is not valid.");
        }

        SByte IConvertible.ToSByte(IFormatProvider? provider) {
            return Convert.ToSByte(StandardValue);
        }

        Byte IConvertible.ToByte(IFormatProvider? provider) {
            return Convert.ToByte(StandardValue);
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            return Convert.ToInt16(StandardValue);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            return Convert.ToUInt16(StandardValue);
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            return Convert.ToInt32(StandardValue);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            return Convert.ToUInt32(StandardValue);
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            return Convert.ToInt64(StandardValue);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            return Convert.ToUInt64(StandardValue);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return Convert.ToSingle(StandardValue);
        }

        Double IConvertible.ToDouble(IFormatProvider? provider) {
            return Convert.ToDouble(StandardValue);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return Convert.ToDecimal(StandardValue);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            // throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Boolean32", "DateTime"));
            throw new InvalidCastException("Conversion from type 'Boolean32' to type 'DateTime' is not valid.");
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            // Convert.DefaultToType((IConvertible)this, type, provider);
            return ((IConvertible)StandardValue).ToType(type, provider);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            info.AddValue(nameof(m_value), StandardValue);
        }

        public static bool operator ==(Boolean32 first, Boolean32 second) {
            return first.Equals(second);
        }

        public static bool operator !=(Boolean32 first, Boolean32 second) {
            return !(first == second);
        }
    }
}