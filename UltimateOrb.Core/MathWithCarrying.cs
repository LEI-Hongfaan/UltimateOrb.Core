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

        public static void DecreaseUnchecked(TIntT value, out TIntT result) {
            result = unchecked(value - (TIntT)1);
        }

        public static void ConditionalDecreaseUnchecked(CanonicalIntegerBoolean borrow, TIntT value, out TIntT result) {
            result = unchecked(value - (TIntT)borrow.ToUInteger());
        }

        private static CanonicalIntegerBoolean IncreaseUnsigned(TIntT value, out TIntT result) {
            CanonicalIntegerBooleanModule.
        }

        private static CanonicalIntegerBoolean IncreaseUnsigned(CanonicalIntegerBoolean carry, TIntT value, out TIntT result) {
        }

        private static CanonicalIntegerBoolean DecreaseUnsigned(TIntT value, out TIntT result) {
        }

        private static CanonicalIntegerBoolean DecreaseUnsigned(CanonicalIntegerBoolean borrow, TIntT value, out TIntT result) {
        }

        private static void AddUnchecked(TIntT first, TIntT second, out TIntT result) {
        }

        private static void AddUnchecked(CanonicalIntegerBoolean carry, TIntT first, TIntT second, out TIntT result) {
        }

        private static void SubtractUnchecked(TIntT first, TIntT second, out TIntT result) {
        }

        private static void SubtractUnchecked(CanonicalIntegerBoolean borrow, TIntT first, TIntT second, out TIntT result) {
        }

        private static CanonicalIntegerBoolean AddUnsigned(TIntT first, TIntT second, out TIntT result) {
        }

        private static CanonicalIntegerBoolean AddUnsigned(CanonicalIntegerBoolean carry, TIntT first, TIntT second, out TIntT result) {
        }

        private static CanonicalIntegerBoolean SubtractUnsigned(TIntT first, TIntT second, out TIntT result) {
        }

        private static CanonicalIntegerBoolean SubtractUnsigned(CanonicalIntegerBoolean borrow, TIntT first, TIntT second, out TIntT result) {
        }
    }
}
