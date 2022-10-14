#pragma warning disable CS8909 // Do not compare function pointer values
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.CInterop {

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr : IComparable, IComparable<ActionPtr>, IEquatable<ActionPtr>, ISpanFormattable, IFormattable, ISerializable {
        
        readonly delegate* managed<void> value__;

        public unsafe ActionPtr(delegate* managed<void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* managed<void>)value);
        }

        public void Invoke() {
            value__();
        }

        public static implicit operator ActionPtr(delegate* managed<void> value) => new (value);

        public static implicit operator delegate* managed<void>(ActionPtr value) => value.value__;

        public static explicit operator ActionPtr(void* value) => new ((delegate* managed<void>)value);

        public static implicit operator void*(ActionPtr value) => value.value__;

        public int CompareTo(ActionPtr other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr value) {
                var u = this.value__;
                var v = value.value__;
                if (u < v) {
                    return -1;
                }
                if (u > v) {
                    return 1;
                }
                return 0;
            }

            // TODO:
            throw new ArgumentException("Argument should be of type ActionPtr.", nameof(obj));
        }

        public bool Equals(ActionPtr other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr other && this.value__ == other.value__;
        }

        public override int GetHashCode() {
            return unchecked((nuint)this.value__).GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((long)(nint)this.value__));
        }

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            return unchecked((UIntPtr)(nuint)this.value__).TryFormat(destination, out charsWritten, format, provider);
        }

        string IFormattable.ToString(string? format, IFormatProvider? formatProvider) {
            return unchecked((UIntPtr)(nuint)this.value__).ToString(format, formatProvider);
        }

        public override string? ToString() {
            // TODO:
            return unchecked((UIntPtr)(nuint)this.value__).ToString();
        }

        public static bool operator ==(ActionPtr first, ActionPtr second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr first, ActionPtr second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr first, ActionPtr second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr first, ActionPtr second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr first, ActionPtr second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr first, ActionPtr second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<TResult> : IComparable, IComparable<FuncPtr<TResult>>, IEquatable<FuncPtr<TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* managed<TResult> value__;

        public unsafe FuncPtr(delegate* managed<TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* managed<TResult>)value);
        }

        public TResult Invoke() {
            return value__();
        }

        public int CompareTo(FuncPtr<TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<TResult> value) {
                var u = this.value__;
                var v = value.value__;
                if (u < v) {
                    return -1;
                }
                if (u > v) {
                    return 1;
                }
                return 0;
            }

            // TODO:
            throw new ArgumentException("Argument should be of type FuncPtr<TResult>.", nameof(obj));
        }

        public bool Equals(FuncPtr<TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<TResult>(delegate* managed<TResult> value) => new (value);

        public static implicit operator delegate* managed<TResult>(FuncPtr<TResult> value) => value.value__;

        public static explicit operator FuncPtr<TResult>(void* value) => new ((delegate* managed<TResult>)value);

        public static implicit operator void*(FuncPtr<TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<TResult> other && this.value__ == other.value__;
        }

        public override int GetHashCode() {
            return unchecked((nuint)this.value__).GetHashCode();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((long)(nint)this.value__));
        }

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            return unchecked((UIntPtr)(nuint)this.value__).TryFormat(destination, out charsWritten, format, provider);
        }

        string IFormattable.ToString(string? format, IFormatProvider? formatProvider) {
            return unchecked((UIntPtr)(nuint)this.value__).ToString(format, formatProvider);
        }

        public override string? ToString() {
            // TODO:
            return unchecked((UIntPtr)(nuint)this.value__).ToString();
        }

        public static bool operator ==(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<TResult> first, FuncPtr<TResult> second) => first.value__ >= second.value__;
    }
}
