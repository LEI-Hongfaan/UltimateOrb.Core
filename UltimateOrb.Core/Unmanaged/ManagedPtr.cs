#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0190 // Null check can be simplified
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Unmanaged {

    using static ManagedPtrHelpers;
    using static UltimateOrb.Unmanaged.ManagedPtrHelpers;

    static partial class SR {

        public static string NotSupported_CannotCallGetHashCodeOnManagedPtr {

            get => "GetHashCode() on ManagedPtr and ReadOnlyManagedPtr is not supported.";
        }
    }

    static partial class ManagedPtrHelpers {

        public readonly struct ManagedPtrDummy {
        }
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ManagedPtr : IComparable/*, IComparable<ManagedPtr>, IEquatable<ManagedPtr>*/, ISpanFormattable, IFormattable, ISerializable {

        internal readonly ref ManagedPtrDummy ref__;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe ManagedPtr(ref ManagedPtrDummy valueRef) {
            ref__ = ref valueRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ManagedPtr(ref byte valueRef) : this(ref Unsafe.As<byte, ManagedPtrDummy>(ref Unsafe.AsRef(in valueRef))) {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ManagedPtr(ByReference<byte> valueRef) : this(ref valueRef.Value) {
        }

        [Obsolete]
        unsafe ManagedPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidManagedPtrValue");
            }
            ref__ = ref Unsafe.AsRef<ManagedPtrDummy>(unchecked((void*)value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static implicit operator ManagedPtr(void* value) => new ManagedPtr(ref Unsafe.AsRef<byte>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static implicit operator ManagedPtr(Ptr value) => new ManagedPtr(ref Unsafe.AsRef<byte>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator void*(ManagedPtr value) => Unsafe.AsPointer(ref value.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator Ptr(ManagedPtr value) => Unsafe.AsPointer(ref value.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(ManagedPtr other) {
            var d = Unsafe.ByteOffset(ref ref__, ref other.ref__);
            return Math.Sign(d);
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            // if (obj is ManagedPtr value)
            // ref struct will not be on the managed heap

            // TODO:
            throw new ArgumentException("Argument should be of type ManagedPtr.", nameof(obj));
        }

        public bool Equals(ManagedPtr other) {
            return this == other;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            // ref struct will not be on the managed heap
            return false;
        }

        [Obsolete]
        public override int GetHashCode() {
            throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnManagedPtr);
        }

        [Obsolete]
        public unsafe void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((nint)Unsafe.AsPointer(ref ref__)));
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GCBaseOffset(UnknownVersion)[".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "&GCBaseOffset(UnknownVersion)[".Length;
                if (!unchecked((nuint)(void*)this).TryFormat(destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "]".Length;
                charsWritten = cc;
                return true;
            L_Failed:;
                charsWritten = default;
                return false;
            }
        }

        /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GC(UnknownVersion)["u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "&GC(UnknownVersion)["u8.Length;
                if (!unchecked((nuint)(void*)this).TryFormat(utf8Destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]"u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "]"u8.Length;
                bytesWritten = cc;
                return true;
            L_Failed:;
                bytesWritten = default;
                return false;
            }
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return ToString(format, null);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            unsafe {
                return $@"&GC(UnknownVersion)[{unchecked((nuint)(void*)this).ToString(format, provider)}]";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ManagedPtr first, ManagedPtr second) => Unsafe.AreSame(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ManagedPtr first, ManagedPtr second) => !(first == second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ManagedPtr first, ManagedPtr second) => Unsafe.IsAddressLessThan(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ManagedPtr first, ManagedPtr second) => !(first > second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ManagedPtr first, ManagedPtr second) => Unsafe.IsAddressGreaterThan(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ManagedPtr first, ManagedPtr second) => !(first < second);
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ManagedPtr<T> : IComparable/*, IComparable<ManagedPtr<T>>, IEquatable<ManagedPtr<T>>*/, ISpanFormattable, IFormattable, ISerializable where T : unmanaged {

        internal readonly ref T ref__;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ManagedPtr(ref T valueRef) {
            ref__ = ref valueRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ManagedPtr(ByReference<T> valueRef) : this(ref valueRef.Value) {
        }

        [Obsolete]
        unsafe ManagedPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidManagedPtrValue");
            }
            ref__ = ref Unsafe.AsRef<T>(unchecked((void*)value));
        }

        public ref T Dereferenced {

            get => ref this.ref__;
        }

#pragma warning disable IDE1006 // Naming Styles
        public static ref T op_PointerDereference(ManagedPtr<T> value) {
            return ref value.ref__;
        }
#pragma warning restore IDE1006 // Naming Styles

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(ManagedPtr<T> other) {
            var d = Unsafe.ByteOffset(ref ref__, ref other.ref__);
            return Math.Sign(d);
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }

            // TODO:
            throw new ArgumentException("Argument should be of type ManagedPtr<T>.", nameof(obj));
        }

        public bool Equals(ManagedPtr<T> other) {
            return this == other;
        }

        public ref T this[int offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, offset);
        }

        public ref T this[uint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, offset);
        }

        public ref T this[long offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, checked((nint)offset));
        }

        public ref T this[ulong offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, checked((nuint)offset));
        }

        public ref T this[nint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, offset);
        }

        public ref T this[nuint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref ref__, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static implicit operator ManagedPtr<T>(T* value) => new ManagedPtr<T>(ref *value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static implicit operator ManagedPtr<T>(Ptr<T> value) => new ManagedPtr<T>(ref *(T*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator T*(ManagedPtr<T> value) => (T*)Unsafe.AsPointer(ref value.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator Ptr<T>(ManagedPtr<T> value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ManagedPtr<T>(ManagedPtr value) => new ManagedPtr<T>(ref Unsafe.As<ManagedPtrDummy, T>(ref value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ManagedPtr(ManagedPtr<T> value) => new ManagedPtr(ref Unsafe.As<T, ManagedPtrDummy>(ref value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator ManagedPtr<T>(void* value) => new ManagedPtr<T>(ref *(T*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator ManagedPtr<T>(Ptr value) => new ManagedPtr<T>(ref *(T*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator void*(ManagedPtr<T> value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator Ptr(ManagedPtr<T> value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ManagedPtr<T>(ReadOnlyManagedPtr value) => (ManagedPtr<T>)(ManagedPtr)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr(ManagedPtr<T> value) => (ManagedPtr)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ManagedPtr<T> operator ++(ManagedPtr<T> ptr) => ptr + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ManagedPtr<T> operator --(ManagedPtr<T> ptr) => ptr - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, int offset) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(int offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, uint offset) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(uint offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, long offset) => new(ref Unsafe.Add(ref ptr.ref__, checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(long offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, ulong offset) => new(ref Unsafe.Add(ref ptr.ref__, checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ulong offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, nint offset) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(nint offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(ManagedPtr<T> ptr, nuint offset) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator +(nuint offset, ManagedPtr<T> ptr) => new(ref Unsafe.Add(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, int offset) => new(ref Unsafe.Subtract(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, uint offset) => new(ref Unsafe.Subtract(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, long offset) => new(ref Unsafe.Subtract(ref ptr.ref__, checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, ulong offset) => new(ref Unsafe.Subtract(ref ptr.ref__, checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, nint offset) => new(ref Unsafe.Subtract(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ManagedPtr<T> operator -(ManagedPtr<T> ptr, nuint offset) => new(ref Unsafe.Subtract(ref ptr.ref__, offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator -(ManagedPtr<T> first, ManagedPtr<T> second) => Unsafe.ByteOffset(ref first.ref__, ref second.ref__) / Unsafe.SizeOf<T>();

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return false;
        }

        [Obsolete]
        public override int GetHashCode() {
            throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnManagedPtr);
        }

        [Obsolete]
        public unsafe void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((nint)Unsafe.AsPointer(ref ref__)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ManagedPtr<TResult> Cast<TResult>() where TResult : unmanaged {
            return (ManagedPtr<TResult>)(ManagedPtr)this;
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GCBaseOffset(UnknownVersion)[".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "&GCBaseOffset(UnknownVersion)[".Length;
                if (!unchecked((nuint)(T*)this).TryFormat(destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "]".Length;
                charsWritten = cc;
                return true;
            L_Failed:;
                charsWritten = default;
                return false;
            }
        }

        /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GC(UnknownVersion)["u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "&GC(UnknownVersion)["u8.Length;
                if (!unchecked((nuint)(T*)this).TryFormat(utf8Destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]"u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "]"u8.Length;
                bytesWritten = cc;
                return true;
            L_Failed:;
                bytesWritten = default;
                return false;
            }
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return ToString(format, null);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            unsafe {
                return $@"&GC(UnknownVersion)[{unchecked((nuint)(T*)this).ToString(format, provider)}]";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ManagedPtr<T> first, ManagedPtr<T> second) => Unsafe.AreSame(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ManagedPtr<T> first, ManagedPtr<T> second) => !(first == second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ManagedPtr<T> first, ManagedPtr<T> second) => Unsafe.IsAddressLessThan(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ManagedPtr<T> first, ManagedPtr<T> second) => !(first > second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ManagedPtr<T> first, ManagedPtr<T> second) => Unsafe.IsAddressGreaterThan(ref first.ref__, ref second.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ManagedPtr<T> first, ManagedPtr<T> second) => !(first < second);
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct ReadOnlyManagedPtr : IComparable/*, IComparable<ReadOnlyManagedPtr>, IEquatable<ReadOnlyManagedPtr>*/, ISpanFormattable, IFormattable, ISerializable {

        internal readonly ref readonly ManagedPtrDummy ref__;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal unsafe ReadOnlyManagedPtr(ref readonly ManagedPtrDummy valueRef) {
            ref__ = ref valueRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyManagedPtr(ref readonly byte valueRef) : this(ref Unsafe.As<byte, ManagedPtrDummy>(ref Unsafe.AsRef(in valueRef))) {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyManagedPtr(ReadOnlyByReference<byte> valueRef) : this(in valueRef.Value) {
        }

        [Obsolete]
        unsafe ReadOnlyManagedPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidReadOnlyManagedPtrValue");
            }
            ref__ = ref Unsafe.AsRef<ManagedPtrDummy>(unchecked((void*)value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr(ManagedPtr value) => new ReadOnlyManagedPtr(ref value.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ManagedPtr(ReadOnlyManagedPtr value) => new ManagedPtr(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static implicit operator ReadOnlyManagedPtr(ReadOnlyPtr value) => new ReadOnlyManagedPtr(ref Unsafe.AsRef<byte>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator ReadOnlyPtr(ReadOnlyManagedPtr value) => (ReadOnlyPtr)Unsafe.AsPointer(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator ReadOnlyManagedPtr(void* value) => new ReadOnlyManagedPtr(ref Unsafe.AsRef<byte>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator ReadOnlyManagedPtr(Ptr value) => new ReadOnlyManagedPtr(ref Unsafe.AsRef<byte>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator void*(ReadOnlyManagedPtr value) => Unsafe.AsPointer(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static explicit operator Ptr(ReadOnlyManagedPtr value) => Unsafe.AsPointer(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(ReadOnlyManagedPtr other) {
            var d = Unsafe.ByteOffset(ref Unsafe.AsRef(in ref__), ref Unsafe.AsRef(in other.ref__));
            return Math.Sign(d);
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            // if (obj is ReadOnlyManagedPtr value)
            // ref struct will not be on the managed heap

            // TODO:
            throw new ArgumentException("Argument should be of type ReadOnlyManagedPtr.", nameof(obj));
        }

        public bool Equals(ReadOnlyManagedPtr other) {
            return this == other;
        }

        public override bool Equals([NotNullWhen(true)] object? obj) {
            // ref struct will not be on the managed heap
            return false;
        }

        public override int GetHashCode() {
            throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnManagedPtr);
        }

        [Obsolete]
        public unsafe void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ref__))));
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GCBaseOffset(UnknownVersion)[".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "&GCBaseOffset(UnknownVersion)[".Length;
                if (!unchecked((nuint)(void*)this).TryFormat(destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "]".Length;
                charsWritten = cc;
                return true;
            L_Failed:;
                charsWritten = default;
                return false;
            }
        }

        /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GCBaseOffset(UnknownVersion)["u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "&GCBaseOffset(UnknownVersion)["u8.Length;
                if (!unchecked((nuint)(void*)this).TryFormat(utf8Destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]"u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "]"u8.Length;
                bytesWritten = cc;
                return true;
            L_Failed:;
                bytesWritten = default;
                return false;
            }
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return ToString(format, null);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            unsafe {
                return $@"&GCBaseOffset(UnknownVersion)[{unchecked((nuint)(void*)this).ToString(format, provider)}]";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => Unsafe.AreSame(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => !(first == second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => Unsafe.IsAddressLessThan(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => !(first > second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ReadOnlyManagedPtr first, ReadOnlyManagedPtr second) => !(first < second);
    }

    [CLSCompliant(false)]
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly ref struct ReadOnlyManagedPtr<T> : IComparable/*, IComparable<ReadOnlyManagedPtr<T>>, IEquatable<ReadOnlyManagedPtr<T>>*/, ISpanFormattable, IFormattable, ISerializable where T : unmanaged {

        readonly ref readonly T ref__;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyManagedPtr(ref readonly T valueRef) {
            ref__ = ref valueRef;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyManagedPtr(ReadOnlyByReference<T> valueRef) : this(ref Unsafe.AsRef(in valueRef.Value)) {
        }

        [Obsolete]
        unsafe ReadOnlyManagedPtr(SerializationInfo info, StreamingContext context) {
            long value = info.GetInt64("value");
            if (sizeof(nint) == sizeof(int) && (value > int.MaxValue || value < int.MinValue)) {
                throw new ArgumentException("Serialization_InvalidManagedPtrValue");
            }
            ref__ = ref Unsafe.AsRef<T>(unchecked((void*)value));
        }

        public ref readonly T Dereferenced {

            get => ref this.ref__;
        }

#pragma warning disable IDE1006 // Naming Styles
        public static ref readonly T op_PointerDereference(ReadOnlyManagedPtr<T> value) {
            return ref value.ref__;
        }
#pragma warning restore IDE1006 // Naming Styles

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CompareTo(ReadOnlyManagedPtr<T> other) {
            var d = Unsafe.ByteOffset(ref Unsafe.AsRef(in ref__), ref Unsafe.AsRef(in other.ref__));
            return Math.Sign(d);
        }

        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }

            // TODO:
            throw new ArgumentException("Argument should be of type ReadOnlyManagedPtr<T>.", nameof(obj));
        }

        public bool Equals(ReadOnlyManagedPtr<T> other) {
            return this == other;
        }

        public ref readonly T this[int offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), offset);
        }

        public ref readonly T this[uint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), offset);
        }

        public ref readonly T this[long offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), checked((nint)offset));
        }

        public ref readonly T this[ulong offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), checked((nuint)offset));
        }

        public ref readonly T this[nint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), offset);
        }

        public ref readonly T this[nuint offset] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.Add(ref Unsafe.AsRef(in ref__), offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr<T>(Ptr<T> value) => (ReadOnlyManagedPtr<T>)(ReadOnlyPtr<T>)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ptr<T>(ReadOnlyManagedPtr<T> value) => (Ptr<T>)(ReadOnlyPtr<T>)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr<T>(ReadOnlyPtr<T> value) => new ReadOnlyManagedPtr<T>(ref Unsafe.AsRef<T>((T*)value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlyPtr<T>(ReadOnlyManagedPtr<T> value) => (ReadOnlyPtr<T>)Unsafe.AsPointer(ref Unsafe.AsRef<T>(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr<T>(T* value) => new ReadOnlyManagedPtr<T>(ref Unsafe.AsRef<T>(value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator T*(ReadOnlyManagedPtr<T> value) => (T*)Unsafe.AsPointer(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlyManagedPtr<T>(ReadOnlyManagedPtr value) => new ReadOnlyManagedPtr<T>(ref Unsafe.As<ManagedPtrDummy, T>(ref Unsafe.AsRef(in value.ref__)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr(ReadOnlyManagedPtr<T> value) => new ReadOnlyManagedPtr(ref Unsafe.As<T, ManagedPtrDummy>(ref Unsafe.AsRef(in value.ref__)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlyManagedPtr<T>(ManagedPtr value) => (ManagedPtr<T>)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ManagedPtr(ReadOnlyManagedPtr<T> value) => (ManagedPtr<T>)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlyManagedPtr<T>(Ptr value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlyManagedPtr<T>(void* value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Ptr(ReadOnlyManagedPtr<T> value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator void*(ReadOnlyManagedPtr<T> value) => (T*)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyManagedPtr<T>(ManagedPtr<T> value) => new ReadOnlyManagedPtr<T>(ref value.ref__);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ManagedPtr<T>(ReadOnlyManagedPtr<T> value) => new ManagedPtr<T>(ref Unsafe.AsRef(in value.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyManagedPtr<T> operator ++(ReadOnlyManagedPtr<T> ptr) => ptr + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyManagedPtr<T> operator --(ReadOnlyManagedPtr<T> ptr) => ptr - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, int offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(int offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, uint offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(uint offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, long offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(long offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, ulong offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ulong offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, nint offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(nint offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(ReadOnlyManagedPtr<T> ptr, nuint offset) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator +(nuint offset, ReadOnlyManagedPtr<T> ptr) => new(ref Unsafe.Add(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, int offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, uint offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, long offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), checked((nint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, ulong offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), checked((nuint)offset)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, nint offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static ReadOnlyManagedPtr<T> operator -(ReadOnlyManagedPtr<T> ptr, nuint offset) => new(ref Unsafe.Subtract(ref Unsafe.AsRef(in ptr.ref__), offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator -(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => Unsafe.ByteOffset(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__)) / Unsafe.SizeOf<T>();

        public override bool Equals([NotNullWhen(true)] object? obj) {
            return false;
        }

        [Obsolete]
        public override int GetHashCode() {
            throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnManagedPtr);
        }

        [Obsolete]
        public unsafe void GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("value", unchecked((nint)Unsafe.AsPointer(ref Unsafe.AsRef(in ref__))));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyManagedPtr<TResult> Cast<TResult>() where TResult : unmanaged {
            return (ReadOnlyManagedPtr<TResult>)(ReadOnlyManagedPtr)this;
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GCBaseOffset(UnknownVersion)[".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "&GCBaseOffset(UnknownVersion)[".Length;
                if (!unchecked((nuint)(T*)this).TryFormat(destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]".TryCopyTo(destination)) {
                    goto L_Failed;
                }
                cc += "]".Length;
                charsWritten = cc;
                return true;
            L_Failed:;
                charsWritten = default;
                return false;
            }
        }

        /// <inheritdoc cref="IUtf8SpanFormattable.TryFormat" />
        public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.NumericFormat)] ReadOnlySpan<char> format = default, IFormatProvider? provider = null) {
            unsafe {
                var cc = 0;
                if (!"&GC(UnknownVersion)["u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "&GC(UnknownVersion)["u8.Length;
                if (!unchecked((nuint)(T*)this).TryFormat(utf8Destination, out var c1, format, provider)) {
                    goto L_Failed;
                }
                cc += c1;
                if (!"]"u8.TryCopyTo(utf8Destination)) {
                    goto L_Failed;
                }
                cc += "]"u8.Length;
                bytesWritten = cc;
                return true;
            L_Failed:;
                bytesWritten = default;
                return false;
            }
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
            return ToString(format, null);
        }

        public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
            unsafe {
                return $@"&GC(UnknownVersion)[{unchecked((nuint)(T*)this).ToString(format, provider)}]";
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => Unsafe.AreSame(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => !(first == second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => Unsafe.IsAddressLessThan(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => !(first > second);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef(in first.ref__), ref Unsafe.AsRef(in second.ref__));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ReadOnlyManagedPtr<T> first, ReadOnlyManagedPtr<T> second) => !(first < second);
    }
}
