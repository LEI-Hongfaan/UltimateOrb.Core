using System;
using System.Collections.Generic;

namespace UltimateOrb {

    using static UndefinedT;

    public readonly struct UndefinableNullable<T> : IEquatable<T?>, IEquatable<UndefinedT>, IEquatable<UndefinableNullable<T>> where T : struct {

        enum UndefinableNullableFlags : byte {
            HasValue = 0,
            IsNull = 1,
            IsUndefined = byte.MaxValue,
        }

        internal readonly T value;

        readonly UndefinableNullableFlags flags;

        public UndefinableNullable(T value) {
            this.value = value;
            this.flags = UndefinableNullableFlags.HasValue;
        }

        public UndefinableNullable(T? value) {
            if (value.HasValue) {
                this.value = value.GetValueOrDefault();
                this.flags = UndefinableNullableFlags.HasValue;
            }
            this.flags = UndefinableNullableFlags.IsNull;
        }

        public UndefinableNullable(UndefinedT? value) {
            if (ReferenceEquals(value, null)) {
                this.flags = UndefinableNullableFlags.IsNull;
                return;
            }
            this.flags = UndefinableNullableFlags.IsUndefined;
        }
        public bool HasValue {

            get => flags == UndefinableNullableFlags.HasValue;
        }

        public T Value {

            get => HasValue ? value : throw new InvalidOperationException();
        }

        public bool Equals(object? other) {
            if (other is UndefinableNullable<T> value) {
                return Equals(value);
            }
            return flags switch {
                UndefinableNullableFlags.HasValue => other is T v && Equals(v),
                UndefinableNullableFlags.IsNull => ReferenceEquals(null, other),
                _ => ReferenceEquals(Undefined, other),
            };
        }

        public bool Equals(T other) {
            return HasValue && EqualityComparer<T>.Default.Equals(value, other);
        }

        public bool Equals(T? other) {
            return flags switch {
                UndefinableNullableFlags.HasValue => other.HasValue && EqualityComparer<T>.Default.Equals(value, other.GetValueOrDefault()),
                UndefinableNullableFlags.IsNull => !other.HasValue,
                _ => false,
            };
        }

        public bool Equals(UndefinedT? other) {
            if (ReferenceEquals(other, null)) {
                return flags == UndefinableNullableFlags.IsNull;
            }
            return flags == UndefinableNullableFlags.IsUndefined;
        }

        public bool Equals(UndefinableNullable<T> other) {
            return flags == other.flags && EqualityComparer<T>.Default.Equals(value, other.value);
        }

        public static implicit operator UndefinableNullable<T>(T value) {
            return new UndefinableNullable<T>(value);
        }

        public static implicit operator UndefinableNullable<T>(T? value) {
            return new UndefinableNullable<T>(value);
        }

        public static implicit operator UndefinableNullable<T>(UndefinedT? value) {
            return new UndefinableNullable<T>(value);
        }

        public static explicit operator T(UndefinableNullable<T> value) {
            if (value.HasValue) {
                return value.value;
            }
            throw new InvalidCastException();
        }

        public static explicit operator T?(UndefinableNullable<T> value) {
            if (value.HasValue) {
                return value.value;
            }
            throw new InvalidCastException();
        }

        public static explicit operator UndefinedT?(UndefinableNullable<T> value) {
            return value.flags switch {
                UndefinableNullableFlags.HasValue => throw new InvalidCastException(),
                UndefinableNullableFlags.IsNull => null,
                _ => Undefined,
            };
        }

        public static bool operator ==(UndefinableNullable<T> left, UndefinableNullable<T> right) {
            return left.Equals(right);
        }

        public static bool operator !=(UndefinableNullable<T> left, UndefinableNullable<T> right) {
            return !(left == right);
        }

        public T GetValueOrDefault() {
            return value;
        }

        public object? GetValueAsObject() {
            return flags switch {
                UndefinableNullableFlags.HasValue => value,
                UndefinableNullableFlags.IsNull => null,
                _ => Undefined,
            };
        }

        public override int GetHashCode() {
            return HasValue ? value.GetHashCode() : 0;
        }
    }
}