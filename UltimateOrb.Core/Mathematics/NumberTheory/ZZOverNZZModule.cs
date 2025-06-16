using System;

namespace UltimateOrb.Mathematics.NumberTheory {
    using Math = Numerics.DoubleArithmetic;

    /// <summary>
    ///     <para>Provides methods for modular arithmetic over the ring Z/nZ.</para>
    ///     <para>
    ///         This static class implements common modular arithmetic operations such as addition, subtraction,
    ///         multiplication, squaring, and exponentiation for unsigned 32-bit and 64-bit integers.
    ///         All operations assume that the modulus <paramref name="n"/> is greater than the operands.
    ///     </para>
    /// </summary>
    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public static partial class ZZOverNZZModule {

        /// <summary>
        /// Computes the sum of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first + second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Sum(uint n, uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return unchecked(n <= (second = (first + second)) || first > second ? second - n : second);
        }

        /// <summary>
        /// Computes the sum of two unsigned 64-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first + second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Sum(ulong n, ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            return unchecked(n <= (second = (first + second)) || first > second ? second - n : second);
        }

        /// <summary>
        /// Computes the modular doubling of an unsigned 32-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to double.</param>
        /// <returns>The result of (2 * value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Double(uint n, uint value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return Sum(n, value, value);
        }

        /// <summary>
        /// Computes the modular doubling of an unsigned 64-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to double.</param>
        /// <returns>The result of (2 * value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Double(ulong n, ulong value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            return Sum(n, value, value);
        }

        /// <summary>
        /// Computes the difference of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The minuend.</param>
        /// <param name="second">The subtrahend.</param>
        /// <returns>The result of (first - second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Difference(uint n, uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return unchecked(second > first ? first - second + n : first - second);
        }

        /// <summary>
        /// Computes the difference of two unsigned 64-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The minuend.</param>
        /// <param name="second">The subtrahend.</param>
        /// <returns>The result of (first - second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Difference(ulong n, ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            return unchecked(second > first ? first - second + n : first - second);
        }

        /// <summary>
        /// Computes the modular additive inverse (opposite) of an unsigned 32-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of (-value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Opposite(uint n, uint value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return Difference(n, 0u, value);
        }

        /// <summary>
        /// Computes the modular additive inverse (opposite) of an unsigned 64-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of (-value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Opposite(ulong n, ulong value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            return Difference(n, 0u, value);
        }

        /// <summary>
        /// Computes the product of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first * second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Product(uint n, uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return unchecked((uint)(((ulong)first * second) % n));
        }

        /// <summary>
        /// Computes the product of two unsigned 64-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first * second) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Product(ulong n, ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            ulong q;
            var p = Math.BigMul(first, second, out q);
            return Math.BigRemNoThrowWhenOverflow(p, q, n);
        }

        /// <summary>
        /// Computes the square of an unsigned 32-bit integer modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to square.</param>
        /// <returns>The result of (value * value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Square(uint n, uint value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            return unchecked((uint)(((ulong)value * value) % n));
        }

        /// <summary>
        /// Computes the square of an unsigned 64-bit integer modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to square.</param>
        /// <returns>The result of (value * value) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Square(ulong n, ulong value) {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            ulong q;
            var p = Math.BigSquare(value, out q);
            return Math.BigRemNoThrowWhenOverflow(p, q, n);
        }

        /// <summary>
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Power(uint n, uint @base, uint exponent) {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            uint j = 0u;
            if (n != 1u) {
                j = 1u;
            }
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    j = Product(n, @base, j);
                }
                if (0u == (exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }

        /// <summary>
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 64-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint Power(uint n, uint @base, ulong exponent) {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<uint>());
            uint j = 0u;
            if (n != 1u) {
                j = 1u;
            }
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    j = Product(n, @base, j);
                }
                if (0u == (exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }

        /// <summary>
        /// Computes the modular exponentiation of an unsigned 64-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Power(ulong n, ulong @base, uint exponent) {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            ulong j = 0u;
            if (n != 1u) {
                j = 1u;
            }
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    j = Product(n, @base, j);
                }
                if (0u == (exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }

        /// <summary>
        /// Computes the modular exponentiation of an unsigned 64-bit integer base raised to a 64-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong Power(ulong n, ulong @base, ulong exponent) {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<ulong>());
            ulong j = 0u;
            if (n != 1u) {
                j = 1u;
            }
            for (; ; ) {
                if (0u != (exponent & 1u)) {
                    j = Product(n, @base, j);
                }
                if (0u == (exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }
    }
}