using System;
using System.Collections.Generic;

namespace UltimateOrb {
    using Runtime.CompilerServices;
    using System.Diagnostics.CodeAnalysis;
    using static U;

    /// <summary>
    /// Provides static methods to support discriminated unions. 
    /// </summary>
    public readonly partial struct U {

        [DoesNotReturn]
        internal static InvalidOperationException ThrowInvalidOperationException() {
            throw new InvalidOperationException();
        }

        [DoesNotReturn]
        internal static InvalidCastException ThrowInvalidCastException() {
            throw new InvalidCastException();
        }

        [DoesNotReturn]
        internal static ArgumentOutOfRangeException ThrowArgumentOutOfRange_case() {
            throw new ArgumentOutOfRangeException("case");
        }
    }

    namespace Runtime.CompilerServices {

        /// <summary>
        /// Defines a common interface for discriminated unions.
        /// </summary>
        public interface IU {

            /// <summary>
            /// Returns the value in a discriminated union.
            /// </summary>
            object? Value {

                get;
            }

            /// <summary>
            /// Returns the number that presents of which case the value is in a discriminated union.
            /// </summary>
            int Case {

                get;
            }

            /// <summary>
            /// Returns the case count of a discriminated union.
            /// </summary>
            int CaseCount {

                get;
            }
        }
    }

    /// <summary>
    /// Defines a value-type discriminated union with two cases.
    /// </summary>
    public readonly struct U<T1, T2> : IComparable<U<T1, T2>>, IEquatable<U<T1, T2>>, IU {

        /// <summary>
        /// Determines of which case the value is in a discriminated union.
        /// </summary>
        public readonly int Case;

        /// <summary>
        /// Value when <see cref="Case"/> is 1 or default.
        /// </summary>
        public readonly T1? Case1OrDefault;

        /// <summary>
        /// Value when <see cref="Case"/> is 2 or default.
        /// </summary>
        public readonly T2? Case2OrDefault;

        /// <summary>
        /// Constructs a discriminated union from the value of case 1. 
        /// </summary>
        public U(T1 value) : this() {
            Case1OrDefault = value;
            Case = 1;
        }

        /// <summary>
        /// Constructs a discriminated union from the value of case 2. 
        /// </summary>
        public U(T2 value) : this() {
            Case2OrDefault = value;
            Case = 2;
        }

        /// <summary>
        /// Constructs a discriminated union from the value. 
        /// </summary>
        public static U<T1, T2> Create<T>(T value, int @case) {
            if (1 == @case) {
                return new U<T1, T2>((T1)(object)value!);
            }
            if (2 == @case) {
                return new U<T1, T2>((T2)(object)value!);
            }
            throw ThrowArgumentOutOfRange_case();
        }

        /// <summary>
        /// Value of case 1.
        /// </summary>
        public T1 Case1 {

            get => 1 != Case ? throw ThrowInvalidOperationException() : Case1OrDefault!;
        }

        /// <summary>
        /// Value of case 2.
        /// </summary>
        public T2 Case2 {

            get => 2 != Case ? throw ThrowInvalidOperationException() : Case2OrDefault!;
        }

        object? IU.Value =>
             1 == Case ? (object?)Case1OrDefault :
             2 == Case ? (object?)Case2OrDefault : null;

        int IU.Case => Case;

        int IU.CaseCount => 2;

        /// <summary>
        /// Value of case 2.
        /// </summary>
        public T GetValue<T>() {
            if (1 == Case) {
                return (T)(object?)Case1OrDefault!;
            }
            if (2 == Case) {
                return (T)(object?)Case2OrDefault!;
            }
            throw ThrowInvalidOperationException();
        }

        /// <summary>
        /// Compares discriminated unions. Two discriminated unions are not equal if their case numbers are different.
        /// </summary>
        public int CompareTo(U<T1, T2> other) {
            var result = unchecked((uint)this.Case).CompareTo(unchecked((uint)other.Case));
            if (0 != result) {
                return result;
            }
            result = Comparer<T1>.Default.Compare(this.Case1OrDefault, other.Case1OrDefault);
            if (0 != result) {
                return result;
            }
            result = Comparer<T2>.Default.Compare(this.Case2OrDefault, other.Case2OrDefault);
            return result;
        }

        /// <summary>
        /// Compares discriminated unions. Two discriminated unions are not equal if their case numbers are different.
        /// </summary>
        public bool Equals(U<T1, T2> other) {
            return
                this.Case == other.Case &&
                (1 != Case || EqualityComparer<T1>.Default.Equals(this.Case1OrDefault, other.Case1OrDefault)) &&
                (2 != Case || EqualityComparer<T2>.Default.Equals(this.Case2OrDefault, other.Case2OrDefault));
        }

        /// <summary>
        /// Extracts the value in a discriminated union.
        /// </summary>
        public static explicit operator T1(U<T1, T2> value) {
            return 1 != value.Case ? throw ThrowInvalidCastException() : value.Case1OrDefault!;
        }

        /// <summary>
        /// Extracts the value in a discriminated union.
        /// </summary>
        public static explicit operator T2(U<T1, T2> value) {
            return 2 != value.Case ? throw ThrowInvalidCastException() : value.Case2OrDefault!;
        }

        /// <summary>
        /// Warps a value into a discriminated union.
        /// </summary>
        public static implicit operator U<T1, T2>(T1 value) {
            return new U<T1, T2>(value);
        }

        /// <summary>
        /// Warps a value into a discriminated union.
        /// </summary>
        public static implicit operator U<T1, T2>(T2 value) {
            return new U<T1, T2>(value);
        }

        /// <summary>
        /// Compares discriminated unions.<br />
        /// Two discriminated unions are not equal if their case tags are different.<br />
        /// </summary>
        public override bool Equals(object? obj) {
            if (obj is U<T1, T2> value) {
                return Equals(value);
            }
            return false;
        }

        /// <summary>
        /// Calculates the hash code for the current <see cref="U{T1, T2}"/> instance.
        /// </summary>
        /// <returns>The hash code for the current <see cref="U{T1, T2}"/> instance.</returns>
        public override int GetHashCode() {
            return Case ^ (
                1 == Case ? (Case1OrDefault?.GetHashCode()).GetValueOrDefault() :
                2 == Case ? (Case2OrDefault?.GetHashCode()).GetValueOrDefault() : 0);
        }

        /// <summary>
        /// Returns a string that represents the value of this <see cref="U{T1, T2}"/> instance.
        /// </summary>
        /// <returns>The string representation of this <see cref="U{T1, T2}"/> instance.</returns>
        public override string? ToString() {
            if (1 == Case) {
                return $@"{{{Case1OrDefault?.ToString()}:1}}";
            }
            if (2 == Case) {
                return $@"{{{Case2OrDefault?.ToString()}:2}}";
            }
            return "";
        }

        /// <summary>
        /// Compares discriminated unions.<br />
        /// Two discriminated unions are not equal if their case tags are different.<br />
        /// </summary>
        public static bool operator ==(U<T1, T2> first, U<T1, T2> second) {
            return first.Equals(second);
        }

        /// <summary>
        /// Compares discriminated unions.<br />
        /// Two discriminated unions are not equal if their case tags are different.<br />
        /// </summary>
        public static bool operator !=(U<T1, T2> first, U<T1, T2> second) {
            return !(first == second);
        }
    }
}
