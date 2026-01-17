using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    internal static class Int128InternalExtensions {

        extension(UltimateOrb.Int128 @this) {

            public static UltimateOrb.Int128 FromLoHi(UInt64 lo, Int64 hi) {
                return new UltimateOrb.Int128(lo: lo, hi: hi);
            }
        }

        extension(UltimateOrb.UInt128 @this) {

            public static UltimateOrb.UInt128 FromLoHi(UInt64 lo, UInt64 hi) {
                return new UltimateOrb.UInt128(lo: lo, hi: hi);
            }
        }
    }

    internal static class SystemInt128InternalExtensions {

        extension(System.Int128 @this) {

            public static System.Int128 FromLoHi(UInt64 lo, Int64 hi) {
                return new System.Int128(upper: unchecked((UInt64)hi), lower: lo);
            }
        }

        extension(System.UInt128 @this) {

            public static System.UInt128 FromLoHi(UInt64 lo, UInt64 hi) {
                return new System.UInt128(upper: hi, lower: lo);
            }
        }
    }
}
