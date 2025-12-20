using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        static partial class AbsUnchecked_PerType<T> where T : ISignedNumber<T>, IBinaryInteger<T>, IMinMaxValue<T> {

            internal static readonly int BitSizeM1 = checked(int.CreateChecked(T.Log2(T.MaxValue)) + 1);
        }

        public static T AbsUnchecked<T>(T value) where T : ISignedNumber<T>, IBinaryInteger<T>, IMinMaxValue<T> {
            var mask = value >> AbsUnchecked_PerType<T>.BitSizeM1;
            return unchecked(value ^ mask - mask);
        }
    }
}
