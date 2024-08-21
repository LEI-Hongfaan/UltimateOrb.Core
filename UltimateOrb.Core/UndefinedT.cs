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

        public static bool operator <(UndefinedT? first, UndefinedT? second) {
            return Utilities.CilVerifiable.LessThan((object?)first, (object?)second);
        }

        public static bool operator <=(UndefinedT? first, UndefinedT? second) {
            return Utilities.CilVerifiable.LessThanOrEqual((object?)first, (object?)second);
        }

        public static bool operator >(UndefinedT? first, UndefinedT? second) {
            return Utilities.CilVerifiable.GreaterThan((object?)first, (object?)second);
        }

        public static bool operator >=(UndefinedT? first, UndefinedT? second) {
            return Utilities.CilVerifiable.GreaterThanOrEqual((object?)first, (object?)second);
        }

        public static bool operator ==(UndefinedT? first, UndefinedT? second) {
            return ReferenceEquals(first, second);
        }

        public static bool operator !=(UndefinedT? first, UndefinedT? second) {
            return !(first == second);
        }

        /// <summary>
        /// Returns a string that represents the undefined value.
        /// </summary>
        public override string ToString() {
            return "Undefined";
        }
    }
}