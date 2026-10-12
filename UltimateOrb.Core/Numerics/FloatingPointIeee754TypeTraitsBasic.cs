using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Provides basic IEEE-754 floating-point traits and derived values for a floating-point type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">A floating-point type implementing <see cref="IFloatingPointIeee754{T}"/>.</typeparam>
    [Experimental("UoWIP")]
    public static class FloatingPointIeee754TypeTraitsBasic<T> where T : IFloatingPointIeee754<T>? {

        internal static T PositiveZero { get; } = T.CopySign(T.Zero, T.One);

        /// <summary>
        /// Gets the smallest positive value greater than zero.
        /// </summary>
        /// <remarks>
        /// Computed as the bit increment of a signed zero with the sign of one:
        /// <c>T.BitIncrement(T.CopySign(T.Zero, T.One))</c>.
        /// </remarks>
        public static T Epsilon { get; } = T.BitIncrement(PositiveZero);

        internal static T PrecisionEpsilon { get; } = T.One - T.BitDecrement(T.One);

        internal static T RadixAsT { get; } = T.CreateChecked(T.Radix);

        private static T LogBApproximate(T x) => T.Radix != 2 ? (T.Radix != 10 ?
            T.Log(x, RadixAsT) : T.Log10(x)) : T.Log2(x);

        /// <summary>
        /// Gets the precision of the floating-point type expressed in units of the type.
        /// </summary>
        /// <remarks>
        /// For binary radix this is computed using base-2 logarithm; for decimal radix base-10 is used;
        /// otherwise a generic logarithm with the type radix is used.
        /// </remarks>
        public static T Precision { get; } = T.Round(-LogBApproximate(PrecisionEpsilon));

        /// <summary>
        /// Gets the smallest representable finite value (the minimum finite value).
        /// </summary>
        /// <remarks>
        /// Computed as the bit increment of negative infinity to obtain the most negative finite value.
        /// </remarks>
        public static T MinValue { get; } = T.BitIncrement(T.NegativeInfinity);

        /// <summary>
        /// Gets the largest representable finite value (the maximum finite value).
        /// </summary>
        /// <remarks>
        /// Computed as the bit decrement of positive infinity to obtain the largest finite value.
        /// </remarks>
        public static T MaxValue { get; } = T.BitDecrement(T.PositiveInfinity);

        /// <summary>
        /// Gets the minimum exponent (as a value of type <typeparamref name="T"/>) for normalized numbers.
        /// </summary>
        /// <remarks>
        /// This value is determined by finding the smallest normal value and taking its logarithm
        /// in the appropriate radix, rounded to the nearest representable value.
        /// </remarks>
        public static T MinExponent { get; } = GetMinExponent();

        private static T GetMinExponent() {
            var minNormal = Epsilon;
            for (; ; ) {
                if (T.IsNormal(minNormal)) {
                    return T.Round(LogBApproximate(minNormal));
                }
                minNormal = T.ScaleB(minNormal, 1);
            }
        }

        /// <summary>
        /// Gets the maximum exponent (as a value of type <typeparamref name="T"/>) for normalized numbers.
        /// </summary>
        /// <remarks>
        /// Computed from <see cref="MaxValue"/> by taking the rounded logarithm in the appropriate radix and subtracting one.
        /// </remarks>
        public static T MaxExponent { get; } = T.Round(LogBApproximate(MaxValue)) - T.One;

        internal static T MaxSignificantF { get; } = T.BitDecrement(RadixAsT);

        internal static T Two { get; } = T.One + T.One;

        internal static BigInteger GetMaxSignificandAsBigInteger() {
            return (BigInteger.Pow(T.Radix, int.CreateChecked(Precision - T.One)) - 1) * T.Radix + (T.Radix - 1);
        }

        /// <summary>
        /// Provides conversions of the floating-point traits into an unsigned integer type <typeparamref name="TInt"/>.
        /// </summary>
        /// <typeparam name="TInt">An unsigned integer type implementing <see cref="INumberBase{TInt}"/>.</typeparam>
        public static class As<TInt> where TInt : INumberBase<TInt>? {

            /// <summary>
            /// Gets the precision converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="Precision"/> value.
            /// </remarks>
            public static TInt Precision { get; } = TInt.CreateChecked(FloatingPointIeee754TypeTraitsBasic<T>.Precision);

            /// <summary>
            /// Gets the minimum exponent converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="MinExponent"/> value.
            /// </remarks>
            public static TInt MinExponent { get; } = TInt.CreateChecked(FloatingPointIeee754TypeTraitsBasic<T>.MinExponent);

            /// <summary>
            /// Gets the maximum exponent converted to <typeparamref name="TInt"/>.
            /// </summary>
            /// <remarks>
            /// Uses <see cref="INumberBase{T}.CreateChecked"/> to convert the <see cref="MaxExponent"/> value.
            /// </remarks>
            public static TInt MaxExponent { get; } = TInt.CreateChecked(FloatingPointIeee754TypeTraitsBasic<T>.MaxExponent);
        }
    }
}
