using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.Extensions.CanonicalIntegerBooleanExtensions;

namespace UltimateOrb {
    using TIntT = UInt64;

    public static partial class MathWithCarrying {

        public static void IncreaseUnchecked(TIntT value, out TIntT result) {
            result = unchecked(value + (TIntT)1);
        }

        public static void ConditionalIncreaseUnchecked(CanonicalIntegerBoolean carry, TIntT value, out TIntT result) {
            result = unchecked(value + (TIntT)carry.ToUInteger());
        }

        public static CanonicalIntegerBoolean ConditionalIncreaseUnsigned(CanonicalIntegerBoolean carry, TIntT value, out TIntT result) {
            ConditionalIncreaseUnchecked(carry, value, out result);
            return carry & CanonicalIntegerBooleanModule.Equals(-1, value.ToSignedUnchecked());
        }

        public static void DecreaseUnchecked(TIntT value, out TIntT result) {
            result = unchecked(value - (TIntT)1);
        }

        public static void ConditionalDecreaseUnchecked(CanonicalIntegerBoolean borrow, TIntT value, out TIntT result) {
            result = unchecked(value - (TIntT)borrow.ToUInteger());
        }

        public static CanonicalIntegerBoolean ConditionalDecreaseUnsigned(CanonicalIntegerBoolean borrow, TIntT value, out TIntT result) {
            ConditionalDecreaseUnchecked(borrow, value, out result);
            return borrow & CanonicalIntegerBooleanModule.Equals(0, value);
        }

        public static CanonicalIntegerBoolean IncreaseUnsigned(TIntT value, out TIntT result) {
            result = 1 + value;
            return CanonicalIntegerBooleanModule.Equals(-1, value.ToSignedUnchecked());
        }

        public static CanonicalIntegerBoolean IncreaseUnsigned(CanonicalIntegerBoolean carry, TIntT value, out TIntT result) {
            var t = 1 + value;
            return CanonicalIntegerBooleanModule.Equals(-1, value.ToSignedUnchecked()) | ConditionalIncreaseUnsigned(carry, t, out result);
        }

        public static CanonicalIntegerBoolean DecreaseUnsigned(TIntT value, out TIntT result) {
            result = value - 1;
            return CanonicalIntegerBooleanModule.Equals(0, value);
        }

        public static CanonicalIntegerBoolean DecreaseUnsigned(CanonicalIntegerBoolean borrow, TIntT value, out TIntT result) {
            var t = value - 1;
            return CanonicalIntegerBooleanModule.Equals(0, value) | ConditionalDecreaseUnsigned(borrow, t, out result);
        }

        public static void AddUnchecked(TIntT first, TIntT second, out TIntT result) {
            result = unchecked(first + second);
        }

        public static void AddUnchecked(CanonicalIntegerBoolean carry, TIntT first, TIntT second, out TIntT result) {
            result = unchecked((TIntT)carry + first + second);
        }

        public static void SubtractUnchecked(TIntT first, TIntT second, out TIntT result) {
            result = unchecked(first - second);
        }

        public static void SubtractUnchecked(CanonicalIntegerBoolean borrow, TIntT first, TIntT second, out TIntT result) {
            result = unchecked(first - (TIntT)borrow - second);
        }

        public static CanonicalIntegerBoolean AddUnsigned(TIntT first, TIntT second, out TIntT result) {
            var result_ = unchecked(first + second);
            result = result_;
            return CanonicalIntegerBooleanModule.LessThanOrEqual(first.ToUnsignedUnchecked(), result_.ToUnsignedUnchecked());
        }

        public static CanonicalIntegerBoolean AddUnsigned(CanonicalIntegerBoolean carry, TIntT first, TIntT second, out TIntT result) {
            return ConditionalIncreaseUnsigned(carry, first, out var result_) | AddUnsigned(result_, second, out result);
        }

        public static CanonicalIntegerBoolean SubtractUnsigned(TIntT first, TIntT second, out TIntT result) {
            result = unchecked(first - second);
            return CanonicalIntegerBooleanModule.GreaterThanOrEqual(first.ToUnsignedUnchecked(), second.ToUnsignedUnchecked());
        }

        public static CanonicalIntegerBoolean SubtractUnsigned(CanonicalIntegerBoolean borrow, TIntT first, TIntT second, out TIntT result) {
            return ConditionalDecreaseUnsigned(borrow, first, out var result_) | SubtractUnsigned(result_, second, out result);
        }
    }
}
