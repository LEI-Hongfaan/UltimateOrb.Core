using System;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Utilities;
using Unsafe1 = System.Runtime.CompilerServices.Unsafe;

namespace UltimateOrb.Runtime.CompilerServices {

    public static partial class BoxedExtensions {

        public static Boxed<T> Box<T>(this T value) where T : struct => Unsafe1.As<Boxed<T>>(value);

        [return: NotNullIfNotNull(nameof(value))]
        public static Boxed<T>? Box<T>(this in T? value) where T : struct => Unsafe1.As<Boxed<T>>(value);

        public static ReadOnlyBoxed<T> BoxAsReadOnly<T>(this T value) where T : struct => Unsafe1.As<ReadOnlyBoxed<T>>(value);

        [return: NotNullIfNotNull(nameof(value))]
        public static ReadOnlyBoxed<T>? BoxAsReadOnly<T>(this in T? value) where T : struct => Unsafe1.As<ReadOnlyBoxed<T>>(value);

        public static ref T Unbox<T>(this Boxed<T> value) where T : struct => ref value.Value;

        public static ref readonly T Unbox<T>(this ReadOnlyBoxed<T> value) where T : struct => ref value.Value;
    }

    public abstract class Boxed<T>
        where T : struct {

        private Boxed() => throw new NotImplementedException();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref T Value => ref Unsafe1.As<StrongBox<T>>(this).Value;

        [return: NotNullIfNotNull(nameof(boxed))]
        public static Boxed<T>? GetTypedObjectReference(object? boxed) => boxed switch {
            null => null,
            T => Unsafe1.As<Boxed<T>>(boxed),
            _ => throw new ArgumentException("Type mismatched.", nameof(boxed)),
        };

        public static implicit operator ByReference<T>(Boxed<T> boxed) => new(ref boxed.Value);

        public static explicit operator T(Boxed<T> boxed) => boxed.Value;

        public static explicit operator Boxed<T>(T value) => value.Box();

        /// <returns>Untyped reference to a boxed value.</returns>
        [return: NotNullIfNotNull(nameof(boxedValue))]
        public static implicit operator ValueType?(Boxed<T>? boxedValue)
            => Unsafe1.As<ValueType>(boxedValue);

        public Boxed<T> Copy() => Unsafe1.As<Boxed<T>>(MemberwiseClone());

        [return: NotNullIfNotNull(nameof(value))]
        public static explicit operator Boxed<T>?(in T? value) => value.Box();

        /// <inheritdoc />
        public override abstract bool Equals(object? obj);

        /// <inheritdoc />
        public override abstract int GetHashCode();

        /// <inheritdoc />
        public override abstract string ToString();
    }

    public abstract class ReadOnlyBoxed<T>
        where T : struct {

        private ReadOnlyBoxed() => throw new NotImplementedException();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref readonly T Value => ref Unsafe1.As<StrongBox<T>>(this).Value;

        [return: NotNullIfNotNull(nameof(boxed))]
        public static Boxed<T>? GetTypedObjectReference(object? boxed) => boxed switch {
            null => null,
            T => Unsafe1.As<Boxed<T>>(boxed),
            _ => throw new ArgumentException("Type mismatched.", nameof(boxed)),
        };

        public static implicit operator ReadOnlyByReference<T>(ReadOnlyBoxed<T> boxed) => new(in boxed.Value);

        public static explicit operator T(ReadOnlyBoxed<T> boxed) => boxed.Value;

        public static explicit operator ReadOnlyBoxed<T>(T value) => value.BoxAsReadOnly();

        public static explicit operator Boxed<T>(ReadOnlyBoxed<T> value) => Unsafe1.As<Boxed<T>>(value);

        public static implicit operator ReadOnlyBoxed<T>(Boxed<T> value) => Unsafe1.As<ReadOnlyBoxed<T>>(value);

        /// <returns>Untyped reference to a boxed value.</returns>
        [return: NotNullIfNotNull(nameof(boxedValue))]
        public static explicit operator ValueType?(ReadOnlyBoxed<T>? boxedValue)
            => Unsafe1.As<ValueType>(boxedValue);

        public ReadOnlyBoxed<T> Copy() => Unsafe1.As<ReadOnlyBoxed<T>>(MemberwiseClone());

        [return: NotNullIfNotNull(nameof(value))]
        public static explicit operator ReadOnlyBoxed<T>?(in T? value) => value.BoxAsReadOnly();

        /// <inheritdoc />
        public override abstract bool Equals(object? obj);

        /// <inheritdoc />
        public override abstract int GetHashCode();

        /// <inheritdoc />
        public override abstract string ToString();
    }

    public abstract class BoxedAny<T> {

        private BoxedAny() => throw new NotImplementedException();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public T Value => !typeof(T).IsValueType ? Unsafe1.BitCast<Wrapper<BoxedAny<T>>, Wrapper<T>>(this).Value : Unsafe1.As<StrongBox<T>>(this).Value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref T ValueRef => ref (!typeof(T).IsValueType ? ref ThrowNotSupportedExceptionRef() : ref Unsafe1.As<StrongBox<T>>(this).Value);

        [DoesNotReturn]
        static ref T ThrowNotSupportedExceptionRef() {
            throw new NotSupportedException();
        }

        [return: NotNullIfNotNull(nameof(boxed))]
        public static BoxedAny<T>? GetTypedObjectReference(object? boxed) => boxed switch {
            null => null,
            T => Unsafe1.As<BoxedAny<T>>(boxed),
            _ => throw new ArgumentException("Type mismatched.", nameof(boxed)),
        };

        // public static implicit operator DualPtr<TSelf>(BoxedAny<TSelf> boxed) => new(ref boxed.Value);

        [return: NotNullIfNotNull(nameof(boxed))]
        public static explicit operator T(BoxedAny<T> boxed) => !typeof(T).IsValueType ? Unsafe1.BitCast<Wrapper<BoxedAny<T>?>, Wrapper<T?>>(boxed).Value! : Unsafe1.As<StrongBox<T>>(boxed)!.Value!;

        [return: NotNullIfNotNull(nameof(value))]
        public static implicit operator BoxedAny<T>?(T? value) => Unsafe1.As<BoxedAny<T>>(value);

        /// <inheritdoc cref="System.Object.MemberwiseClone()"/>
        public BoxedAny<T> UnsafeCopy() => Unsafe1.As<BoxedAny<T>>(MemberwiseClone());

        /// <inheritdoc />
        public override abstract bool Equals(object? obj);

        /// <inheritdoc />
        public override abstract int GetHashCode();

        /// <inheritdoc />
        public override abstract string ToString();
    }
}
