using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    partial class HalfHelpers {

#if NET7_0_OR_GREATER
        public static Half FromBinaryInteger<T>(T value)
            where T : IBinaryInteger<T>, IUnsignedNumber<T> {
            unchecked {
                var BitSize = BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne;
                if (BinaryIntegerTypeTraits<T>.IsSigned) {
                    if (BitSize >= 0 && BitSize <= 32) {
                        var x = uint.CreateTruncating(value);
                        x += 65519u;
                        if (x >= (65520u << 1) - 1u) {
                            return T.IsNegative(value) ? Half.NegativeInfinity : Half.PositiveInfinity;
                        }
                    } else if (BitSize >= 0 && BitSize <= 128) {
                        var x = value;
                        x += T.CreateTruncating(65519);
                        if (x >= T.CreateTruncating((65520 << 1) - 1)) {
                            return T.IsNegative(value) ? Half.NegativeInfinity : Half.PositiveInfinity;
                        }
                    } else {
                        if (value >= T.CreateTruncating(65520) || value <= T.CreateTruncating(-65520)) {
                            return T.IsNegative(value) ? Half.NegativeInfinity : Half.PositiveInfinity;
                        }
                    }
                    var v = int.CreateTruncating(value);
                    return int.IsNegative(v) ?
                        -BitConverter.Int16BitsToHalf(unchecked((Int16)HalfHelpers.ToHalfPartial(unchecked((uint)(-v))))) :
                        BitConverter.Int16BitsToHalf(unchecked((Int16)HalfHelpers.ToHalfPartial(unchecked((uint)v))));
                } else {
                    if (BitSize >= 0 && BitSize <= 32) {
                        if (uint.CreateTruncating(value) >= 65520u) {
                            return Half.PositiveInfinity;
                        }
                    } else {
                        if (value >= T.CreateTruncating((UInt16)65520u)) {
                            return Half.PositiveInfinity;
                        }
                    }
                    return BitConverter.Int16BitsToHalf(unchecked((Int16)HalfHelpers.ToHalfPartial(uint.CreateTruncating(value))));
                }
            }
        }
#endif
    }
}
