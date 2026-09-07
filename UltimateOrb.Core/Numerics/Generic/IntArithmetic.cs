using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics.Generic {
    /// <summary>
    /// requires:<br/>
    ///   default(T) == T.Zero<br/>
    ///   default(TInt) == TInt.Zero<br/>
    ///   default(TUInt) == TUInt.Zero<br/>
    ///   default(TInt) == TInt.Zero<br/>
    ///   default(TUInt) == TUInt.Zero<br/>
    ///   T.MinValue ^ T.MaxValue == T.AllBitsSet<br/>
    ///   TInt.MinValue ^ TInt.MaxValue == TInt.AllBitsSet<br/>
    ///   TUInt.MinValue ^ TUInt.MaxValue == TUInt.AllBitsSet<br/>
    ///   bitWidth(T) == T.PopCount(T.AllBitsSet)<br/>
    ///   bitWidth(TInt) == TInt.PopCount(TInt.AllBitsSet)<br/>
    ///   bitWidth(TUInt) == TInt.PopCount(TUInt.AllBitsSet)<br/>
    ///   bitWidth(TInt) == bitSize(TInt)<br/>
    ///   bitWidth(TUInt) == bitSize(TUInt)<br/>
    ///   bitWidth(TUInt) == bitWidth(TInt)<br/>
    ///   bitWidth(TInt) >= bitWidth(T)<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInt"></typeparam>
    /// <typeparam name="TUInt"></typeparam>
    internal static partial class IntArithmetic<T, TInt, TUInt>
        where T : IBinaryInteger<T>
        where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>, IMinMaxValue<TInt>
        where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt>, IMinMaxValue<TUInt> {

        public static T AddSaturatingSigned(T first, T second) {
            unchecked {
                TUInt x = TUInt.CreateTruncating(first);
                TUInt y = TUInt.CreateTruncating(second);

                TUInt sum = x + y;

                TUInt t = (x >> (8 * Unsafe.SizeOf<TUInt>() - 1)) + TUInt.CreateTruncating(TInt.MaxValue);

                if (!TInt.IsNegative(TInt.CreateTruncating((t ^ y) | ~(y ^ sum)))) {
                    sum = t;
                }

                return T.CreateTruncating(sum);
            }
        }

        public static T AddSaturatingUnsigned(T first, T second) {
            unchecked {
                var x = TUInt.CreateTruncating(first);
                var y = TUInt.CreateTruncating(second);
                var sum = x + y;
                sum |= sum < x ? TUInt.AllBitsSet : TUInt.Zero;
                return T.CreateTruncating(sum);
            }
        }

        public static T SubtractSaturatingSigned(T first, T second) {
            unchecked {
                TUInt x = TUInt.CreateTruncating(first);
                TUInt y = TUInt.CreateTruncating(second);

                TUInt sum = x + y;

                TUInt t = (x >> (8 * Unsafe.SizeOf<TUInt>() - 1)) + TUInt.CreateTruncating(TInt.MaxValue);

                if (TInt.IsNegative(TInt.CreateTruncating((t ^ y) & (y ^ sum)))) {
                    sum = t;
                }

                return T.CreateTruncating(sum);
            }
        }

        public static T SubtractSaturatingUnsigned(T first, T second) {
            unchecked {
                var x = TUInt.CreateTruncating(first);
                var y = TUInt.CreateTruncating(second);
                var diff = x - y;
                diff &= diff <= x ? TUInt.AllBitsSet : TUInt.Zero;
                return T.CreateTruncating(diff);
            }
        }

        public static T MultiplySaturatingSigned(T first, T second) {
            unchecked {
                var x = TInt.CreateTruncating(first);
                var y = TInt.CreateTruncating(second);

                // The sign of the mathematical result
                bool negative = TInt.IsNegative(x) != TInt.IsNegative(y);

                TUInt ux = TUInt.CreateTruncating(x);
                TUInt uy = TUInt.CreateTruncating(y);

                // Convert to unsigned magnitudes without overflowing on MinValue.
                if (TInt.IsNegative(x)) {
                    ux = TUInt.Zero - ux;
                }

                if (TInt.IsNegative(y)) {
                    uy = TUInt.Zero - uy;
                }

                TUInt prod = ux * uy;

                // Detect unsigned multiplication overflow.
                bool overflow = ux != TUInt.Zero && prod / ux != uy;

                // Maximum representable magnitude depends on the sign:
                //
                // positive:  MaxValue
                // negative:  MaxValue + 1 (MinValue's magnitude)
                TUInt limit = negative
                    ? TUInt.CreateTruncating(TInt.MaxValue) + TUInt.One
                    : TUInt.CreateTruncating(TInt.MaxValue);

                if (overflow || prod > limit) {
                    return T.CreateTruncating(
                        negative ? TInt.MinValue : TInt.MaxValue);
                }

                TUInt result = negative
                    ? TUInt.Zero - prod
                    : prod;

                return T.CreateTruncating(result);
            }
        }
        
        public static T MultiplySaturatingUnsigned(T first, T second) {
            unchecked {
                var x = TUInt.CreateTruncating(first);
                var y = TUInt.CreateTruncating(second);

                return T.CreateTruncating(
                    y != TUInt.Zero && x > TUInt.MaxValue / y
                        ? TUInt.MaxValue
                        : x * y);
            }
        }

        public static T MultiplySaturatingUnsigned_A_1(T first, T second) {
            unchecked {
                var x = TUInt.CreateTruncating(first);
                var y = TUInt.CreateTruncating(second);
                var bound = TUInt.One << (4 * Unsafe.SizeOf<TUInt>());
                return T.CreateTruncating(
                    ((y >= bound) || (x >= bound)) && (y != TUInt.Zero) && ((TUInt.MaxValue / y) < x) ?
                        TUInt.MaxValue : (x * y));
            }
        }

        public static T DivideSaturatingSigned(T dividend, T divisor) {
            unchecked {
                var x = TInt.CreateTruncating(dividend);
                var y = TInt.CreateTruncating(divisor);
                TInt quot;
                if (Miscellaneous.Unlikely(y == TInt.NegativeOne && x == TInt.MinValue)) {
                    quot = TInt.MaxValue;
                } else {
                    quot = TInt.CreateTruncating(dividend) / TInt.CreateTruncating(divisor);
                }
                return T.CreateTruncating(quot);
            }
        }

        public static T DivideSaturatingUnsigned(T dividend, T divisor) {
            unchecked {
                return T.CreateTruncating(TUInt.CreateTruncating(dividend) / TUInt.CreateTruncating(divisor));
            }
        }

        public static T CreateSaturatingSignedFromInterger<TOther>(TOther value)
            where TOther : IBinaryInteger<TOther> {
            return T.CreateTruncating(TInt.CreateSaturating(value));
        }

        public static T CreateSaturatingUnsignedFromInterger<TOther>(TOther value)
            where TOther : IBinaryInteger<TOther> {
            return T.CreateTruncating(TUInt.CreateSaturating(value));
        }
    }
}
