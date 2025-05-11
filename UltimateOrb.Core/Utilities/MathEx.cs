using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Utilities {

    public static partial class MathEx {

        public static UInt32 AbsSignedAsUnsigned(Int32 value) {
            var mask = value >> (32 - 1);
            return unchecked((UInt32)(value ^ mask - mask));
        }

        public static UInt64 AbsSignedAsUnsigned(Int64 value) {
            var mask = value >> (64 - 1);
            return unchecked((UInt64)(value ^ mask - mask));
        }
    }
}
