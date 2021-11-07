using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using UltimateOrb.Utilities.InterfaceReadOnlyExtensions.System;

namespace UltimateOrb.Utilities {
    using UltimateOrb.Runtime.CompilerServices;

    [Serializable]
    public readonly struct Int32CheckedNoThrow
        : IComparable
        , IFormattable
        , IConvertible
        , IComparable<Int32CheckedNoThrow>
        , IEquatable<Int32CheckedNoThrow> {

        private readonly Int32 Value;

        public static readonly Int32CheckedNoThrow MaxValue = 2147483647;

        public static readonly Int32CheckedNoThrow MinValue = -2147483648;

        #region Comparisons
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return !(first == second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value < second.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator <=(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value <= second.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value == second.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value > second.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator >=(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value >= second.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(Int32CheckedNoThrow other) {
            return Value.CompareTo(other.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(object? obj) {
            {
                if (obj is Int32CheckedNoThrow value) {
                    return CompareTo(value);
                }
            }
            {
                if (obj is Int32 value) {
                    return -value.CompareTo(this); // throw
                }
            }
            return Value.CompareTo(obj); // throw
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override bool Equals(object? obj) {
            {
                if (obj is Int32CheckedNoThrow value) {
                    return Equals(value);
                }
            }
            {
                if (obj is Int32 value) {
                    return value.Equals(this); // throw
                }
            }
            return Value.Equals(obj); // throw
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(Int32CheckedNoThrow other) {
            return Value.Equals(other.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override int GetHashCode() {
            return Value.GetHashCode();
        }
        #endregion Comparisons

        #region Constructors, Conversions
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal Int32CheckedNoThrow(int value) {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32(Int32CheckedNoThrow value) {
            return value.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32?(Int32CheckedNoThrow? value) {
            return value?.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(Int32 value) {
            return new Int32CheckedNoThrow(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(Int16 value) {
            return new Int32CheckedNoThrow(value);
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(UInt16 value) {
            return new Int32CheckedNoThrow(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(Byte value) {
            return new Int32CheckedNoThrow(value);
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(SByte value) {
            return new Int32CheckedNoThrow(value);
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow(Char value) {
            return new Int32CheckedNoThrow(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator Int32CheckedNoThrow?(Int32? value) {
            return Unsafe.As<Int32?, Int32CheckedNoThrow?>(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32CheckedNoThrow Parse(string s, IFormatProvider? provider) {
            return Int32.Parse(s, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32CheckedNoThrow Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Int32.Parse(s, style, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32CheckedNoThrow Parse(string s) {
            return Int32.Parse(s);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32CheckedNoThrow Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null) {
            return Int32.Parse(s, style, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32CheckedNoThrow Parse(string s, NumberStyles style) {
            return Int32.Parse(s, style);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Int32CheckedNoThrow result) {
            Unsafe.SkipInit(out result);
            return Int32.TryParse(s, style, provider, out Unsafe.As<Int32CheckedNoThrow, Int32>(ref result));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Int32CheckedNoThrow result) {
            Unsafe.SkipInit(out result);
            return Int32.TryParse(s, style, provider, out Unsafe.As<Int32CheckedNoThrow, Int32>(ref result));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ReadOnlySpan<char> s, out Int32CheckedNoThrow result) {
            Unsafe.SkipInit(out result);
            return Int32.TryParse(s, out Unsafe.As<Int32CheckedNoThrow, Int32>(ref result));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse([NotNullWhen(true)] string? s, out Int32CheckedNoThrow result) {
            Unsafe.SkipInit(out result);
            return Int32.TryParse(s, out Unsafe.As<Int32CheckedNoThrow, Int32>(ref result));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            return Value.TryFormat(destination, out charsWritten, format, provider);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string? ToString() {
            return Value.ToString();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            return Value.ToString(format, formatProvider);
        }

        public string ToString(IFormatProvider? provider) {
            return Value.ToString(provider);
        }

        public TypeCode GetTypeCode() {
            return Value.GetTypeCode();
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return Value.ToBoolean(provider);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            return Value.ToByte(provider);
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            return Value.ToChar(provider);
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            return Value.ToDateTime(provider);
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return Value.ToDecimal(provider);
        }

        Double IConvertible.ToDouble(IFormatProvider? provider) {
            return Value.ToDouble(provider);
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            return Value.ToInt16(provider);
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            return Value.ToInt32(provider);
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            return Value.ToInt64(provider);
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            return Value.ToSByte(provider);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return Value.ToSingle(provider);
        }

        string IConvertible.ToString(IFormatProvider? provider) {
            return Value.ToString(provider);
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) {
            return Value.ToType(conversionType, provider);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            return Value.ToUInt16(provider);
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            return Value.ToUInt32(provider);
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            return Value.ToUInt64(provider);
        }
        #endregion Constructors, Conversions

        #region Arithmetic Operations
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator -(Int32CheckedNoThrow? value) {
            if (value.HasValue) {
                if (CheckedNoThrow.TryNegate(value.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator -(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                if (CheckedNoThrow.TrySubtract(first.GetValueOrDefault(), second.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator %(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                if (CheckedNoThrow.TryRemainder(first.GetValueOrDefault(), second.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator &(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value & second.Value;
        }

        // This overload is optional. Define it here to avoid CS8620 false alerms.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator &(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                return first.GetValueOrDefault() & second.GetValueOrDefault();
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator *(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                if (CheckedNoThrow.TryMultiply(first.GetValueOrDefault(), second.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator /(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                if (CheckedNoThrow.TryDivide(first.GetValueOrDefault(), second.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator ^(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value ^ second.Value;
        }

        // This overload is optional. Define it here to avoid CS8620 false alerms.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator ^(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                return first.GetValueOrDefault() ^ second.GetValueOrDefault();
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator |(Int32CheckedNoThrow first, Int32CheckedNoThrow second) {
            return first.Value | second.Value;
        }

        // This overload is optional. Define it here to avoid CS8620 false alerms.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator |(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                return first.GetValueOrDefault() | second.GetValueOrDefault();
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator ~(Int32CheckedNoThrow value) {
            return ~value.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator +(Int32CheckedNoThrow value) {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator +(Int32CheckedNoThrow? value) {
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator +(Int32CheckedNoThrow? first, Int32CheckedNoThrow? second) {
            if (first.HasValue && second.HasValue) {
                if (CheckedNoThrow.TryAdd(first.GetValueOrDefault(), second.GetValueOrDefault(), out var result)) {
                    return result;
                }
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator <<(Int32CheckedNoThrow value, int count) {
            return value.Value << count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator <<(Int32CheckedNoThrow value, int? count) {
            return value.Value << count;
        }

        // This overload is optional. Define it here to avoid CS8620 false alerms.
        // This overload also suppresses CS8629.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator <<(Int32CheckedNoThrow? value, int? count) {
            if (value.HasValue && count.HasValue) {
                return value.GetValueOrDefault() << count.GetValueOrDefault();
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow operator >>(Int32CheckedNoThrow value, int count) {
            return value.Value >> count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator >>(Int32CheckedNoThrow value, int? count) {
            return value.Value >> count;
        }

        // This overload is optional. Define it here to avoid CS8620 false alerms.
        // This overload also suppresses CS8629.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Int32CheckedNoThrow? operator >>(Int32CheckedNoThrow? value, int? count) {
            if (value.HasValue && count.HasValue) {
                return value.GetValueOrDefault() >> count.GetValueOrDefault();
            }
            return null;
        }
        #endregion Arithmetic Operations
    }
}
