#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0190 // Null check can be simplified
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct Ptr : IComparable, IComparable<Ptr>, IEquatable<Ptr>, ISpanFormattable, IFormattable, ISerializable {

        readonly void* value__;

        [CLSCompliant(false)]
        public unsafe Ptr(void* value) {
            value__ = value;
        }

        Ptr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((void*)value);
        }

        public static implicit operator Ptr(void* value) => new Ptr(value);

        public static implicit operator void*(Ptr value) => value.value__;

        public int CompareTo(Ptr other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is Ptr value) {
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
            throw new ArgumentException("Argument should be of type Ptr.", nameof(obj));
        }

        public bool Equals(Ptr other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is Ptr other && this.value__ == other.value__;
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

        public static bool operator ==(Ptr first, Ptr second) => first.value__ == second.value__;

        public static bool operator !=(Ptr first, Ptr second) => first.value__ != second.value__;

        public static bool operator <(Ptr first, Ptr second) => first.value__ < second.value__;

        public static bool operator <=(Ptr first, Ptr second) => first.value__ <= second.value__;

        public static bool operator >(Ptr first, Ptr second) => first.value__ > second.value__;

        public static bool operator >=(Ptr first, Ptr second) => first.value__ >= second.value__;
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct Ptr<T> : IComparable, IComparable<Ptr<T>>, IEquatable<Ptr<T>>, ISpanFormattable, IFormattable, ISerializable where T : unmanaged {

        readonly T* value__;

        public unsafe Ptr(T* value) {
            value__ = value;
        }

        Ptr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((T*)value);
        }

        public ref T Dereferenced {

            get => ref *this.value__;
        }

#pragma warning disable IDE1006 // Naming Styles
        public static ref T op_PointerDereference(Ptr<T> value) {
            return ref *value.value__;
        }
#pragma warning restore IDE1006 // Naming Styles

        public int CompareTo(Ptr<T> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is Ptr<T> value) {
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
            throw new ArgumentException("Argument should be of type Ptr<T>.", nameof(obj));
        }

        public bool Equals(Ptr<T> other) {
            return this.value__ == other.value__;
        }

        public ref T this[int offset] {

            get => ref this.value__[offset];
        }

        public ref T this[uint offset] {

            get => ref this.value__[offset];
        }

        public ref T this[long offset] {

            get => ref this.value__[offset];
        }

        public ref T this[ulong offset] {

            get => ref this.value__[offset];
        }

        public ref T this[nint offset] {

            get => ref this.value__[offset];
        }

        public ref T this[nuint offset] {

            get => ref this.value__[offset];
        }

        public static implicit operator Ptr<T>(T* value) => new Ptr<T>(value);

        public static implicit operator T*(Ptr<T> value) => value.value__;

        public static explicit operator Ptr<T>(Ptr value) => new Ptr<T>((T*)(void*)value);

        public static implicit operator Ptr(Ptr<T> value) => new Ptr(value.value__);

        public static explicit operator Ptr<T>(void* value) => new Ptr<T>((T*)value);

        public static implicit operator void*(Ptr<T> value) => value.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, int offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(int offset, Ptr<T> ptr) => offset + ptr.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, uint offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(uint offset, Ptr<T> ptr) => offset + ptr.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, long offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(long offset, Ptr<T> ptr) => offset + ptr.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, ulong offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(ulong offset, Ptr<T> ptr) => offset + ptr.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, nint offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(nint offset, Ptr<T> ptr) => offset + ptr.value__;

        public static Ptr<T> operator +(Ptr<T> ptr, nuint offset) => ptr.value__ + offset;

        public static Ptr<T> operator +(nuint offset, Ptr<T> ptr) => offset + ptr.value__;

        public static nint operator -(Ptr<T> first, Ptr<T> second) => unchecked((nint)(first.value__ - second.value__));

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is Ptr<T> other && this.value__ == other.value__;
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

        public static bool operator ==(Ptr<T> first, Ptr<T> second) => first.value__ == second.value__;

        public static bool operator !=(Ptr<T> first, Ptr<T> second) => first.value__ != second.value__;

        public static bool operator <(Ptr<T> first, Ptr<T> second) => first.value__ < second.value__;

        public static bool operator <=(Ptr<T> first, Ptr<T> second) => first.value__ <= second.value__;

        public static bool operator >(Ptr<T> first, Ptr<T> second) => first.value__ > second.value__;

        public static bool operator >=(Ptr<T> first, Ptr<T> second) => first.value__ >= second.value__;
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ReadOnlyPtr : IComparable, IComparable<ReadOnlyPtr>, IEquatable<ReadOnlyPtr>, ISpanFormattable, IFormattable, ISerializable {

        readonly void* value__;

        [CLSCompliant(false)]
        public unsafe ReadOnlyPtr(void* value) {
            value__ = value;
        }

        ReadOnlyPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((void*)value);
        }

        public static implicit operator ReadOnlyPtr(Ptr value) => new ReadOnlyPtr((void*)value);

        public static explicit operator Ptr(ReadOnlyPtr value) => new Ptr(value.value__);

        public static explicit operator ReadOnlyPtr(void* value) => new ReadOnlyPtr(value);

        public static implicit operator void*(ReadOnlyPtr value) => value.value__;

        public int CompareTo(ReadOnlyPtr other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ReadOnlyPtr value) {
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
            throw new ArgumentException("Argument should be of type ReadOnlyPtr.", nameof(obj));
        }

        public bool Equals(ReadOnlyPtr other) {
            return this.value__ == other.value__;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ReadOnlyPtr other && this.value__ == other.value__;
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

        public static bool operator ==(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ == second.value__;

        public static bool operator !=(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ != second.value__;

        public static bool operator <(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ < second.value__;

        public static bool operator <=(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ <= second.value__;

        public static bool operator >(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ > second.value__;

        public static bool operator >=(ReadOnlyPtr first, ReadOnlyPtr second) => first.value__ >= second.value__;
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct ReadOnlyPtr<T> : IComparable, IComparable<ReadOnlyPtr<T>>, IEquatable<ReadOnlyPtr<T>>, ISpanFormattable, IFormattable, ISerializable where T : unmanaged {

        readonly T* value__;

        public unsafe ReadOnlyPtr(T* value) {
            value__ = value;
        }

        ReadOnlyPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidPtrValue");
            }
            value__ = unchecked((T*)value);
        }

        public ref readonly T Dereferenced {

            get => ref *this.value__;
        }

#pragma warning disable IDE1006 // Naming Styles
        public static ref readonly T op_PointerDereference(ReadOnlyPtr<T> value) {
            return ref *value.value__;
        }
#pragma warning restore IDE1006 // Naming Styles

        public int CompareTo(ReadOnlyPtr<T> other) {
            return unchecked((nuint)this.value__).CompareTo(unchecked((nuint)this.value__));
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is ReadOnlyPtr<T> value) {
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
            throw new ArgumentException("Argument should be of type ReadOnlyPtr<T>.", nameof(obj));
        }

        public bool Equals(ReadOnlyPtr<T> other) {
            return this.value__ == other.value__;
        }

        public ref readonly T this[int offset] {

            get => ref this.value__[offset];
        }

        public ref readonly T this[uint offset] {

            get => ref this.value__[offset];
        }

        public ref readonly T this[long offset] {

            get => ref this.value__[offset];
        }

        public ref readonly T this[ulong offset] {

            get => ref this.value__[offset];
        }

        public ref readonly T this[nint offset] {

            get => ref this.value__[offset];
        }

        public ref readonly T this[nuint offset] {

            get => ref this.value__[offset];
        }

        public static implicit operator ReadOnlyPtr<T>(Ptr<T> value) => new ReadOnlyPtr<T>((T*)value);

        public static explicit operator Ptr<T>(ReadOnlyPtr<T> value) => new Ptr<T>(value.value__);

        public static implicit operator ReadOnlyPtr<T>(T* value) => new ReadOnlyPtr<T>(value);

        public static explicit operator T*(ReadOnlyPtr<T> value) => value.value__;

        public static explicit operator ReadOnlyPtr<T>(ReadOnlyPtr value) => new ReadOnlyPtr<T>((T*)(void*)value);

        public static implicit operator ReadOnlyPtr(ReadOnlyPtr<T> value) => new ReadOnlyPtr(value.value__);

        public static explicit operator ReadOnlyPtr<T>(Ptr value) => new ReadOnlyPtr<T>((T*)value);

        public static explicit operator Ptr(ReadOnlyPtr<T> value) => new Ptr(value.value__);

        public static explicit operator ReadOnlyPtr<T>(void* value) => new ReadOnlyPtr<T>((T*)value);

        public static explicit operator void*(ReadOnlyPtr<T> value) => value.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, int offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(int offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, uint offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(uint offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, long offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(long offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, ulong offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(ulong offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, nint offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(nint offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static ReadOnlyPtr<T> operator +(ReadOnlyPtr<T> ReadOnlyPtr, nuint offset) => ReadOnlyPtr.value__ + offset;

        public static ReadOnlyPtr<T> operator +(nuint offset, ReadOnlyPtr<T> ReadOnlyPtr) => offset + ReadOnlyPtr.value__;

        public static nint operator -(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => unchecked((nint)(first.value__ - second.value__));

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return obj is ReadOnlyPtr<T> other && this.value__ == other.value__;
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

        public static bool operator ==(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ == second.value__;

        public static bool operator !=(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ != second.value__;

        public static bool operator <(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ < second.value__;

        public static bool operator <=(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ <= second.value__;

        public static bool operator >(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ > second.value__;

        public static bool operator >=(ReadOnlyPtr<T> first, ReadOnlyPtr<T> second) => first.value__ >= second.value__;
    }
}
