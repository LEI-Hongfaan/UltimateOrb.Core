using System;

namespace UltimateOrb.Mathematics.NumberTheory {
    using Math = Numerics.DoubleArithmetic;

    /// <summary>
    ///     <para>Provides methods for modular arithmetic.</para>
    /// </summary>
    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public static partial class ZZOverNZZModule {

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