using System;

namespace UltimateOrb.Mathematics.NumberTheory {
    using Utilities = BinaryNumerals;

    /// <summary>
    ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
    ///     <seealso cref="IsPrimeModule"/>
    /// </summary>
    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public static partial class IsPrimeMillerRabinModule {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static bool IsMillerRabinPseudoprimeInternal(uint a, UInt64 n, UInt64 d, int s) {
            unchecked {
                var t = ZZOverNZZModule.Power(n, a, d);
                if (t == 1) {
                    return true;
                }
                if (t == n - 1u) {
                    return true;
                }
                for (int i = s; i != 0; --i) {
                    t = ZZOverNZZModule.Square(n, t);
                    if (t == n - 1) {
                        return true;
                    }
                }
                return false;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static bool IsMillerRabinPseudoprimeInternal(uint a, UInt32 n, UInt32 d, int s) {
            unchecked {
                var t = ZZOverNZZModule.Power(n, a, d);
                if (t == 1) {
                    return true;
                }
                if (t == n - 1u) {
                    return true;
                }
                for (int i = s; i != 0; --i) {
                    t = ZZOverNZZModule.Square(n, t);
                    if (t == n - 1) {
                        return true;
                    }
                }
                return false;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static bool IsPrimeMillerRabin(UInt64 value) {
            unchecked {
                if (value <= 2u) {
                    return value == 2u;
                }
                if ((value & 1u) == 0u) {
                    return false;
                }
                var d = value - 1u;
                var s = Utilities.CountTrailingZeros(d);
                d >>= s;
                if (!IsMillerRabinPseudoprimeInternal(2, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(3, value, d, s)) {
                    return false;
                }
                if (value < 1373653u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(5, value, d, s)) {
                    return false;
                }
                if (value < 25326001u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(7, value, d, s)) {
                    return false;
                }
                if (value < 3215031751u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(11, value, d, s)) {
                    return false;
                }
                if (value < 2152302898747u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(13, value, d, s)) {
                    return false;
                }
                if (value < 3474749660383u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(17, value, d, s)) {
                    return false;
                }
                if (value < 341550071728321u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(19, value, d, s)) {
                    return false;
                }
                if (!IsMillerRabinPseudoprimeInternal(23, value, d, s)) {
                    return false;
                }
                if (value < 3825123056546413051u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(29, value, d, s)) {
                    return false;
                }
                if (!IsMillerRabinPseudoprimeInternal(31, value, d, s)) {
                    return false;
                }
                if (!IsMillerRabinPseudoprimeInternal(37, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static bool IsPrimeMillerRabin(UInt32 value) {
            unchecked {
                if (value <= 2u) {
                    return value == 2u;
                }
                if ((value & 1u) == 0u) {
                    return false;
                }
                var d = value - 1u;
                var s = Utilities.CountTrailingZeros(d);
                d >>= s;
                if (!IsMillerRabinPseudoprimeInternal(2u, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                /* // 2017Oct06
                if (value < 2047u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(3, value, d, s)) {
                    return false;
                }
                if (value < 1373653u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(5, value, d, s)) {
                    return false;
                }
                if (value < 25326001u) {
                    return true;
                }
                */
                if (!IsMillerRabinPseudoprimeInternal(7u, value, d, s)) {
                    return false;
                }
                if (value < 314821u) {
                    return true;
                }
                /* // 2017Oct06
                if (value < 3215031751u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(11, value, d, s)) {
                    return false;
                }
                */
                if (!IsMillerRabinPseudoprimeInternal(61u, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static bool IsPrimeMillerRabin(UInt16 value) {
            unchecked {
                if (value <= 2u) {
                    return value == 2u;
                }
                if ((value & 1u) == 0u) {
                    return false;
                }
                var d = value - 1u;
                var s = Utilities.CountTrailingZeros(d);
                d >>= s;
                if (!IsMillerRabinPseudoprimeInternal(2u, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsMillerRabinPseudoprimeInternal(3, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(UInt64)"/>
        /// </summary>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(UInt64 value) {
            return IsPrimeMillerRabin(value);
        }

        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(UInt32)"/>
        /// </summary>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(UInt32 value) {
            return IsPrimeMillerRabin(value);
        }

        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(UInt16)"/>
        /// </summary>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(UInt16 value) {
            return IsPrimeMillerRabin(value);
        }

        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(Int64)"/>
        /// </summary>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(Int64 value) {
            if (0 > value) {
                return false;
            }
            return IsPrimeMillerRabin(unchecked((UInt64)value));
        }

        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(Int32)"/>
        /// </summary>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(Int32 value) {
            if (0 > value) {
                return false;
            }
            return IsPrimeMillerRabin(unchecked((UInt32)value));
        }
        
        /// <summary>
        ///     <para>This API supports the product infrastructure and is not intended to be used directly from your code.</para>
        ///     <seealso cref="IsPrimeModule.IsPrime(Int16)"/>
        /// </summary>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(Int16 value) {
            if (0 > value) {
                return false;
            }
            return IsPrimeMillerRabin(unchecked((UInt16)value));
        }
    }
}
