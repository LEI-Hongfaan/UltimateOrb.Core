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
    public unsafe readonly struct ActionPtr<T> : IComparable, IComparable<ActionPtr<T>>, IEquatable<ActionPtr<T>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T, void>)value);
        }

        public void Invoke(T arg) {
            value__(arg);
        }

        public static implicit operator ActionPtr<T>(delegate* unmanaged[Cdecl]<T, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T, void>(ActionPtr<T> value) => value.value__;

        public static explicit operator ActionPtr<T>(void* value) => new((delegate* unmanaged[Cdecl]<T, void>)value);

        public static implicit operator void*(ActionPtr<T> value) => value.value__;

        public int CompareTo(ActionPtr<T> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<TBase>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T> first, ActionPtr<T> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T> first, ActionPtr<T> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T> first, ActionPtr<T> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T> first, ActionPtr<T> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T> first, ActionPtr<T> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T> first, ActionPtr<T> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T, TResult> : IComparable, IComparable<FuncPtr<T, TResult>>, IEquatable<FuncPtr<T, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T, TResult>)value);
        }

        public TResult Invoke(T arg) {
            return value__(arg);
        }

        public int CompareTo(FuncPtr<T, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T, TResult> value) {
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

        public bool Equals(FuncPtr<T, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T, TResult>(delegate* unmanaged[Cdecl]<T, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T, TResult>(FuncPtr<T, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T, TResult>)value);

        public static implicit operator void*(FuncPtr<T, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T, TResult> first, FuncPtr<T, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2> : IComparable, IComparable<ActionPtr<T1, T2>>, IEquatable<ActionPtr<T1, T2>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2) {
            value__(arg1, arg2);
        }

        public static implicit operator ActionPtr<T1, T2>(delegate* unmanaged[Cdecl]<T1, T2, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, void>(ActionPtr<T1, T2> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2> first, ActionPtr<T1, T2> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, TResult> : IComparable, IComparable<FuncPtr<T1, T2, TResult>>, IEquatable<FuncPtr<T1, T2, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2) {
            return value__(arg1, arg2);
        }

        public int CompareTo(FuncPtr<T1, T2, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, TResult>(delegate* unmanaged[Cdecl]<T1, T2, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, TResult>(FuncPtr<T1, T2, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, TResult> first, FuncPtr<T1, T2, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3> : IComparable, IComparable<ActionPtr<T1, T2, T3>>, IEquatable<ActionPtr<T1, T2, T3>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3) {
            value__(arg1, arg2, arg3);
        }

        public static implicit operator ActionPtr<T1, T2, T3>(delegate* unmanaged[Cdecl]<T1, T2, T3, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, void>(ActionPtr<T1, T2, T3> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3> first, ActionPtr<T1, T2, T3> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, TResult>>, IEquatable<FuncPtr<T1, T2, T3, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) {
            return value__(arg1, arg2, arg3);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, TResult>(FuncPtr<T1, T2, T3, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, TResult> first, FuncPtr<T1, T2, T3, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4>>, IEquatable<ActionPtr<T1, T2, T3, T4>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            value__(arg1, arg2, arg3, arg4);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void>(ActionPtr<T1, T2, T3, T4> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4> first, ActionPtr<T1, T2, T3, T4> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            return value__(arg1, arg2, arg3, arg4);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult>(FuncPtr<T1, T2, T3, T4, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, TResult> first, FuncPtr<T1, T2, T3, T4, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            value__(arg1, arg2, arg3, arg4, arg5);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void>(ActionPtr<T1, T2, T3, T4, T5> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5> first, ActionPtr<T1, T2, T3, T4, T5> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            return value__(arg1, arg2, arg3, arg4, arg5);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult>(FuncPtr<T1, T2, T3, T4, T5, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, TResult> first, FuncPtr<T1, T2, T3, T4, T5, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void>(ActionPtr<T1, T2, T3, T4, T5, T6> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6> first, ActionPtr<T1, T2, T3, T4, T5, T6> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IComparable, IComparable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>, IEquatable<ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>, ISpanFormattable, IFormattable, ISerializable {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void> value__;

        public unsafe ActionPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void> value) {
            value__ = value;
        }

        ActionPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void>)value);
        }

        public void Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public static implicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void>(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value) => value.value__;

        public static explicit operator ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, void>)value);

        public static implicit operator void*(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value) => value.value__;

        public int CompareTo(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> value) {
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
            throw new ArgumentException("Argument should be of type ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>.", nameof(obj));
        }

        public bool Equals(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other && this.value__ == other.value__;
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

        public static bool operator ==(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ == second.value__;

        public static bool operator !=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ != second.value__;

        public static bool operator <(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ < second.value__;

        public static bool operator <=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ <= second.value__;

        public static bool operator >(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ > second.value__;

        public static bool operator >=(ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> first, ActionPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> second) => first.value__ >= second.value__;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> : IComparable, IComparable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>, IEquatable<FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>, ISpanFormattable, IFormattable, ISerializable where TResult : unmanaged {

        readonly delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value__;

        public unsafe FuncPtr(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) {
            value__ = value;
        }

        FuncPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>)value);
        }

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            return value__(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public int CompareTo(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) {
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

        public bool Equals(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> other) {
            return this.value__ == other.value__;
        }

        public static implicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => new(value);

        public static implicit operator delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => value.value__;

        public static explicit operator FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(void* value) => new((delegate* unmanaged[Cdecl]<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>)value);

        public static implicit operator void*(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => value.value__;

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> other && this.value__ == other.value__;
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

        public static bool operator ==(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ == second.value__;

        public static bool operator !=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ != second.value__;

        public static bool operator <(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ < second.value__;

        public static bool operator <=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ <= second.value__;

        public static bool operator >(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ > second.value__;

        public static bool operator >=(FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> first, FuncPtr<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> second) => first.value__ >= second.value__;
    }
}
