using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using UltimateOrb.Utilities;

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

namespace UltimateOrb.Mathematics.NumberTheory {
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    public static partial class ZZOverNZZModule {

        /// <summary>
        /// Computes the sum of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first + second) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Sum<T>(T n, T first, T second)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            return unchecked(n <= (second = (first + second)) || first > second ? second - n : second);
        }

        /// <summary>
        /// Computes the modular doubling of an unsigned 32-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to double.</param>
        /// <returns>The result of (2 * value) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Double<T>(T n, T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T s;
            return unchecked(n <= (s = value << 1) || value > s ? s - n : s);
        }

        /// <summary>
        /// Computes the difference of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The minuend.</param>
        /// <param name="second">The subtrahend.</param>
        /// <returns>The result of (first - second) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Difference<T>(T n, T first, T second)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            return unchecked(second > first ? first - second + n : first - second);
        }

        /// <summary>
        /// Computes the modular additive inverse (opposite) of an unsigned 32-bit integer.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to negate.</param>
        /// <returns>The result of (-value) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Opposite<T>(T n, T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            return unchecked(!T.IsZero(value) ? n - value : T.Zero);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Product(byte n, byte first, byte second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            return unchecked((byte)(((uint)first * (uint)second) % (uint)n));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Product(ushort n, ushort first, ushort second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            return unchecked((ushort)(((uint)first * (uint)second) % (uint)n));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UltimateOrb.UInt128 Product(UltimateOrb.UInt128 n, UltimateOrb.UInt128 first, UltimateOrb.UInt128 second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            UltimateOrb.UInt128 q;
            var p = MathEx.BigMul(first, second, out q);
            return MathEx.BigRemNoThrowWhenOverflow(p, q, n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.UInt128 Product(System.UInt128 n, System.UInt128 first, System.UInt128 second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.UInt128 q;
            var p = MathEx.BigMul(first, second, out q);
            return MathEx.BigRemNoThrowWhenOverflow(p, q, n);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BigInteger Product(BigInteger n, BigInteger first, BigInteger second) {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            return first * second % n;
        }

        /// <summary>
        /// Computes the product of two unsigned 32-bit integers modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="first"/> and <paramref name="second"/>.</param>
        /// <param name="first">The first operand.</param>
        /// <param name="second">The second operand.</param>
        /// <returns>The result of (first * second) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Product<T>(T n, T first, T second)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > first);
            System.Diagnostics.Contracts.Contract.Requires(n > second);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            if (typeof(T) == typeof(byte)) {
                goto L_1;
            }
            if (typeof(T) == typeof(UInt16)) {
                goto L_1;
            }
            if (typeof(T) == typeof(char)) {
                goto L_1;
            }
            if (typeof(T) == typeof(UInt32)) {
                return (T)(object)Product((UInt32)(object)n, (UInt32)(object)first, (UInt32)(object)second);
            }
            if (typeof(T) == typeof(UInt64)) {
                return (T)(object)Product((UInt64)(object)n, (UInt64)(object)first, (UInt64)(object)second);
            }
            if (typeof(T) == typeof(UltimateOrb.UInt128)) {
                return (T)(object)Product((UltimateOrb.UInt128)(object)n, (UltimateOrb.UInt128)(object)first, (UltimateOrb.UInt128)(object)second);
            }
            if (typeof(T) == typeof(System.UInt128)) {
                return (T)(object)Product((System.UInt128)(object)n, (System.UInt128)(object)first, (System.UInt128)(object)second);
            }
            return T.CreateTruncating(Product(BigInteger.CreateChecked(n), BigInteger.CreateChecked(first), BigInteger.CreateChecked(second)));
        L_1:
            return T.CreateTruncating(uint.CreateTruncating(first) * uint.CreateTruncating(second) % uint.CreateTruncating(n));
        }

        /// <summary>
        /// Computes the square of an unsigned 32-bit integer modulo <paramref name="n"/>.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="value"/>.</param>
        /// <param name="value">The value to square.</param>
        /// <returns>The result of (value * value) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Square<T>(T n, T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > value);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            // TODO:
            return Product(n, value, value);
        }

        /// <summary>
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Power<T>(T n, T @base, uint exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.Zero;
            if (n != T.One) {
                j = T.One;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PowerWithPositiveBase<T>(T n, T @base, uint exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.One;
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
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Power<T>(T n, T @base, ulong exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.Zero;
            if (n != T.One) {
                j = T.One;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PowerWithPositiveBase<T>(T n, T @base, ulong exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.One;
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
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Power<T>(T n, T @base, System.UInt128 exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.Zero;
            if (n != T.One) {
                j = T.One;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PowerWithPositiveBase<T>(T n, T @base, System.UInt128 exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.One;
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
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Power<T>(T n, T @base, UltimateOrb.UInt128 exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.Zero;
            if (n != T.One) {
                j = T.One;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PowerWithPositiveBase<T>(T n, T @base, UltimateOrb.UInt128 exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            T j = T.One;
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
        /// Computes the modular exponentiation of an unsigned 32-bit integer base raised to a 32-bit exponent.
        /// </summary>
        /// <param name="n">The modulus. Must be greater than <paramref name="base"/>.</param>
        /// <param name="base">The base value.</param>
        /// <param name="exponent">The exponent value.</param>
        /// <returns>The result of (base ^ exponent) mod n.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Power<T, TExponent>(T n, T @base, TExponent exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T>
            where TExponent : notnull, IUnsignedNumber<TExponent>, IBinaryInteger<TExponent> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            if (typeof(TExponent) == typeof(UInt32)) {
                return Power(n, @base, (UInt32)(object)exponent);
            }
            if (typeof(TExponent) == typeof(UInt64)) {
                return Power(n, @base, (UInt64)(object)exponent);
            }
            if (typeof(TExponent) == typeof(UltimateOrb.UInt128)) {
                return Power(n, @base, (UltimateOrb.UInt128)(object)exponent);
            }
            if (typeof(TExponent) == typeof(System.UInt128)) {
                return Power(n, @base, (System.UInt128)(object)exponent);
            }
            // TODO: Perf
            T j = T.Zero;
            if (n != T.One) {
                j = T.One;
            }
            for (; ; ) {
                if (TExponent.IsOddInteger(exponent)) {
                    j = Product(n, @base, j);
                }
                if (TExponent.IsZero(exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PowerWithPositiveBase<T, TExponent>(T n, T @base, TExponent exponent)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T>
            where TExponent : notnull, IUnsignedNumber<TExponent>, IBinaryInteger<TExponent> {
            System.Diagnostics.Contracts.Contract.Requires(n > @base);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<T>());
            if (typeof(TExponent) == typeof(UInt32)) {
                return PowerWithPositiveBase(n, @base, (UInt32)(object)exponent);
            }
            if (typeof(TExponent) == typeof(UInt64)) {
                return PowerWithPositiveBase(n, @base, (UInt64)(object)exponent);
            }
            if (typeof(TExponent) == typeof(UltimateOrb.UInt128)) {
                return PowerWithPositiveBase(n, @base, (UltimateOrb.UInt128)(object)exponent);
            }
            if (typeof(TExponent) == typeof(System.UInt128)) {
                return PowerWithPositiveBase(n, @base, (System.UInt128)(object)exponent);
            }
            // TODO: Perf
            T j = T.One;
            for (; ; ) {
                if (TExponent.IsOddInteger(exponent)) {
                    j = Product(n, @base, j);
                }
                if (TExponent.IsZero(exponent >>= 1)) {
                    break;
                }
                @base = Square(n, @base);
            }
            return j;
        }
    }
}