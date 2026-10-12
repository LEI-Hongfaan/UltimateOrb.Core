using System;
using System.Numerics;
namespace UltimateOrb.Numerics {

    /// <summary>Defines an IEEE 754 floating-point type that is represented in a base-10 format.</summary>
    /// <typeparam name="TSelf">The type that implements the interface.</typeparam>
#if NET11_0_OR_GREATER
    [Obsolete("Use System.Numerics.IDecimalFloatingPointIeee754<TSelf> instead.")]
#endif
    public interface IDecimalFloatingPointIeee754<TSelf> : IFloatingPointIeee754<TSelf>
        where TSelf : IDecimalFloatingPointIeee754<TSelf>? {

        static int INumberBase<TSelf>.Radix {

            get => 10;
        }

        /// <summary>Computes the integer logarithm (base 10) of a value.</summary>
        /// <param name="x">The value whose integer logarithm is to be computed.</param>
        /// <returns>The integer logarithm (base 10) of <paramref name="x" />.</returns>
        public static virtual int ILog10(TSelf x) => TSelf.Radix != 10 ? throw new NotImplementedException() : TSelf.ILogB(x);

        /// <summary>Computes the product of a value and 10 raised to the specified power.</summary>
        /// <param name="x">The value which 10 raised to the power of <paramref name="n" /> multiplies.</param>
        /// <param name="n">The value to which 10 is raised before multipliying <paramref name="x" />.</param>
        /// <returns>The product of <paramref name="x" /> and 10 raised to the power of <paramref name="n" />.</returns>
        public static virtual TSelf Scale10(TSelf x, int n) => TSelf.Radix != 10 ? throw new NotImplementedException() : TSelf.Scale10(x, n);
    }
}
