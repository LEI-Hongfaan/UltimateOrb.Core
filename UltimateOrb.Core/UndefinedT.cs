using System;

namespace UltimateOrb {

    /// <summary>
    /// Represents the type of the undefined value witch can be used with <see cref="UndefinableNullable{T}"/>.
    /// </summary>
    public sealed class UndefinedT : IEquatable<UndefinedT?>, IComparable<UndefinedT?> {

        private UndefinedT() {
        }

        /// <summary>
        /// The undefined value.
        /// </summary>
        public static readonly UndefinedT Undefined = new ();

        public override bool Equals(object? obj) {
            return ReferenceEquals(Undefined, obj);
        }

        public override int GetHashCode() {
            return 0;
        }

        public int CompareTo(UndefinedT? other) {
            if (other is null) {
                return 1;
            }
            return 0;
        }

        public bool Equals(UndefinedT? other) {
            return other is not null;
        }

        public static bool operator <(UndefinedT? left, UndefinedT? right) {
            return Utilities.CilVerifiable.LessThan((object?)left, (object?)right);
        }

        public static bool operator <=(UndefinedT? left, UndefinedT? right) {
            return Utilities.CilVerifiable.LessThanOrEqual((object?)left, (object?)right);
        }

        public static bool operator >(UndefinedT? left, UndefinedT? right) {
            return Utilities.CilVerifiable.GreaterThan((object?)left, (object?)right);
        }

        public static bool operator >=(UndefinedT? left, UndefinedT? right) {
            return Utilities.CilVerifiable.GreaterThanOrEqual((object?)left, (object?)right);
        }

        public static bool operator ==(UndefinedT? left, UndefinedT? right) {
            return ReferenceEquals(left, right);
        }

        public static bool operator !=(UndefinedT? left, UndefinedT? right) {
            return !(left == right);
        }

        /// <summary>
        /// Returns a string that represents the undefined value.
        /// </summary>
        public override string ToString() {
            return "Undefined";
        }
    }
}