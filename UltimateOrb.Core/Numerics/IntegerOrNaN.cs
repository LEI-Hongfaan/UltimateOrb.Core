using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb.Numerics {

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct IntegerOrNaN<T>
        : IEquatable<IntegerOrNaN<T>>, IComparable<IntegerOrNaN<T>>
        where T : IBinaryInteger<T>, ISignedNumber<T> {

        private readonly T _value;

        private static T NaNBits {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => GenericMath.TypeTraits<T>.HighestBitSet;
        } 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaNBits(T value) {
            // MSB set means NaN
            return NaNBits.Equals(value);
        }

        public IntegerOrNaN(T value) {
            _value = value;
        }

        public bool IsNaN => IsNaNBits(_value);

        public T Value {
            get {
                if (IsNaN) throw new NotFiniteNumberException("Value is NaN.");
                return _value;
            }
        }

        double ValueAsDoubleInternal {
            get {
                return IsNaN ? double.NaN : double.CreateSaturating(_value);
            }
        }

        public static IntegerOrNaN<T> NaN => new IntegerOrNaN<T>(NaNBits);

        // Equality
        public bool Equals(IntegerOrNaN<T> other) => _value.Equals(other._value);

        public override bool Equals(object? obj) => obj is IntegerOrNaN<T> other && Equals(other);

        public override int GetHashCode() => _value.GetHashCode();

        // Comparison: NaN is always greater than any number, NaN == NaN
        public int CompareTo(IntegerOrNaN<T> other) {
            bool nanA = IsNaN, nanB = other.IsNaN;
            if (nanA && nanB) return 0;
            if (nanA) return 1;
            if (nanB) return -1;
            return _value.CompareTo(other._value);
        }

        // Operators
        public static bool operator ==(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // NaN is not equal to anything, including itself
            if (left.IsNaN || right.IsNaN) return false;
            return left._value.Equals(right._value);
        }

        public static bool operator !=(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // NaN is not equal to anything, including itself
            if (left.IsNaN || right.IsNaN) return true;
            return !left._value.Equals(right._value);
        }

        public static bool operator <(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // If either is NaN, return false
            if (left.IsNaN || right.IsNaN) return false;
            return left._value.CompareTo(right._value) < 0;
        }

        public static bool operator >(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // If either is NaN, return false
            if (left.IsNaN || right.IsNaN) return false;
            return left._value.CompareTo(right._value) > 0;
        }

        public static bool operator <=(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // If either is NaN, return false
            if (left.IsNaN || right.IsNaN) return false;
            return left._value.CompareTo(right._value) <= 0;
        }

        public static bool operator >=(IntegerOrNaN<T> left, IntegerOrNaN<T> right) {
            // If either is NaN, return false
            if (left.IsNaN || right.IsNaN) return false;
            return left._value.CompareTo(right._value) >= 0;
        }

        // Arithmetic: propagate NaN
        public static IntegerOrNaN<T> operator +(IntegerOrNaN<T> a, IntegerOrNaN<T> b) =>
            a.IsNaN || b.IsNaN ? NaN : new IntegerOrNaN<T>(a._value + b._value);

        public static IntegerOrNaN<T> operator -(IntegerOrNaN<T> a, IntegerOrNaN<T> b) =>
            a.IsNaN || b.IsNaN ? NaN : new IntegerOrNaN<T>(a._value - b._value);

        public static IntegerOrNaN<T> operator *(IntegerOrNaN<T> a, IntegerOrNaN<T> b) =>
            a.IsNaN || b.IsNaN ? NaN : new IntegerOrNaN<T>(a._value * b._value);

        public static IntegerOrNaN<T> operator /(IntegerOrNaN<T> a, IntegerOrNaN<T> b) {
            if (a.IsNaN || b.IsNaN || b._value == T.Zero) return NaN;
            return new IntegerOrNaN<T>(a._value / b._value);
        }

        public static IntegerOrNaN<T> operator %(IntegerOrNaN<T> a, IntegerOrNaN<T> b) {
            if (a.IsNaN || b.IsNaN || b._value == T.Zero) return NaN;
            return new IntegerOrNaN<T>(a._value % b._value);
        }

        public static IntegerOrNaN<T> operator -(IntegerOrNaN<T> a) =>
            a.IsNaN ? NaN : new IntegerOrNaN<T>(-a._value);

        public static IntegerOrNaN<T> operator ++(IntegerOrNaN<T> a) =>
            a.IsNaN ? NaN : new IntegerOrNaN<T>(a._value + T.One);

        public static IntegerOrNaN<T> operator --(IntegerOrNaN<T> a) =>
            a.IsNaN ? NaN : new IntegerOrNaN<T>(a._value - T.One);

        // Formatting
        public override string ToString() => IsNaN ? "NaN" : _value.ToString();

        // Static helpers
        public static bool IsNaNValue(IntegerOrNaN<T> value) => value.IsNaN;

        public static implicit operator IntegerOrNaN<T>(T value) => new IntegerOrNaN<T>(value);
        public static explicit operator T(IntegerOrNaN<T> value) => value.Value;
    }
}
