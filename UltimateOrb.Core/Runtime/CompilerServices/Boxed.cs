using System;
using System.Buffers;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb.Runtime.CompilerServices {

    public static partial class BoxedExtensions {

        public static Boxed<T> Box<T>(this T value) where T : struct => Unsafe.As<Boxed<T>>(value);

        [return: NotNullIfNotNull(nameof(value))]
        public static Boxed<T>? Box<T>(this in T? value) where T : struct => Unsafe.As<Boxed<T>?>(value);

        public static ref T Unbox<T>(this Boxed<T> value) where T : struct => ref value.Value;
    }

    public abstract class Boxed<T>
        where T : struct {

        private Boxed() => throw new NotImplementedException();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ref T Value => ref Unsafe.As<StrongBox<T>>(this).Value;

        [return: NotNullIfNotNull(nameof(boxed))]
        public static Boxed<T>? GetTypedObjectReference(object? boxed) => boxed switch {
            null => null,
            T => Unsafe.As<Boxed<T>>(boxed),
            _ => throw new ArgumentException("Type mismatched.", nameof(boxed)),
        };

        public static implicit operator ByReference<T>(Boxed<T> boxed) => new(ref boxed.Value);

        public static explicit operator T(Boxed<T> boxed) => boxed.Value;

        public static explicit operator Boxed<T>(T value) => value.Box();

        /// <returns>Untyped reference to a boxed value.</returns>
        [return: NotNullIfNotNull(nameof(boxedValue))]
        public static implicit operator ValueType?(Boxed<T>? boxedValue)
            => Unsafe.As<ValueType>(boxedValue);

        public Boxed<T> Copy() => Unsafe.As<Boxed<T>>(MemberwiseClone());

        [return: NotNullIfNotNull(nameof(value))]
        public static explicit operator Boxed<T>?(in T? value) => value.Box();

        /// <inheritdoc />
        public override abstract bool Equals(object? obj);

        /// <inheritdoc />
        public override abstract int GetHashCode();

        /// <inheritdoc />
        public override abstract string ToString();
    }
}
