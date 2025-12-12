using System;

namespace UltimateOrb.Mathematics.NumberTheory {
    using Utilities = BinaryNumerals;

    /// <summary>
    ///     <para>Provides methods for primality tests.</para>
    /// </summary>
    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public static partial class IsPrimeModule {

        private const ulong prime_table = 0x28208A20A08A28AC;

        private const uint primes_2_3_7 = 2 * 3 * 7;

        private const ulong euler_2_3_7 = 0x00000220A28A2822;

        private const uint primes_5_11 = 5 * 11;

        private const ulong euler_5_11 = 0x007BCEF5BDAF73DE;
        
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        private static bool IsProbablePrimeTrialDivision(uint primes, ulong euler, UInt64 value) {
            var a = unchecked((int)(value % primes));
            if (0 == ((euler >> a) & 1)) {
                return false;
            }
            return true;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        private static bool IsProbablePrimeTrialDivision(uint primes, ulong euler, UInt32 value) {
            var a = unchecked((int)(value % primes));
            if (0 == ((euler >> a) & 1)) {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     <para>Checks whether an input number is prime or not.</para>
        /// </summary>
        /// <param name="value">
        ///     <para>The input number.</para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         <c lang="cs">true</c> if the input number is prime;
        ///         otherwise, <c lang="cs">false</c>.
        ///     </para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(UInt64 value) {
            unchecked {
                if (64u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, value)) {
                    return false;
                }
                var d = (UInt64)value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(3, value, d, s)) {
                    return false;
                }
                if (value < 1373653u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(5, value, d, s)) {
                    return false;
                }
                if (value < 25326001u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7, value, d, s)) {
                    return false;
                }
                if (value < 3215031751u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(11, value, d, s)) {
                    return false;
                }
                if (value < 2152302898747u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(13, value, d, s)) {
                    return false;
                }
                if (value < 3474749660383u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(17, value, d, s)) {
                    return false;
                }
                if (value < 341550071728321u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(19, value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(23, value, d, s)) {
                    return false;
                }
                if (value < 3825123056546413051u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(29, value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(31, value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(37, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     <para>Checks whether an input number is prime or not.</para>
        /// </summary>
        /// <param name="value">
        ///     <para>The input number.</para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         <c lang="cs">true</c> if the input number is prime;
        ///         otherwise, <c lang="cs">false</c>.
        ///     </para>
        /// </returns>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        // ~14.5 kCyc : rnd positive odd numbers
        // 0b 0??????? ???????? ???????? ???????? ???????? ???????? ???????? ???????1
        public static bool IsPrime(Int64 value) {
            unchecked {
                if (64 > value) {
                    if (2 > value) {
                        return false;
                    }
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, (UInt64)value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, (UInt64)value)) {
                    return false;
                }
                var d = (UInt64)value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(3, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 1373653u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(5, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 25326001u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 3215031751u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(11, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 2152302898747u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(13, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 3474749660383u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(17, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 341550071728321u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(19, (UInt64)value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(23, (UInt64)value, d, s)) {
                    return false;
                }
                if ((UInt64)value < 3825123056546413051u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(29, (UInt64)value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(31, (UInt64)value, d, s)) {
                    return false;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(37, (UInt64)value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     <para>Checks whether an input number is prime or not.</para>
        /// </summary>
        /// <param name="value">
        ///     <para>The input number.</para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         <c lang="cs">true</c> if the input number is prime;
        ///         otherwise, <c lang="cs">false</c>.
        ///     </para>
        /// </returns>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPrime(UInt32 value) {
            unchecked {
                if (64u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, value)) {
                    return false;
                }
                /*
                var d = value - 1;
                var firstCoefficient = Utilities.CountTrailingZeros(d);
                d >>= firstCoefficient;
                */
                var d = value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7, value, d, s)) {
                    return false;
                }
                if (value < 314821u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(61, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     <para>Checks whether an input number is prime or not.</para>
        /// </summary>
        /// <param name="value">
        ///     <para>The input number.</para>
        /// </param>
        /// <returns>
        ///     <para>
        ///         <c lang="cs">true</c> if the input number is prime;
        ///         otherwise, <c lang="cs">false</c>.
        ///     </para>
        /// </returns>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        // ~2.44 kCyc : rnd positive odd numbers
        // 0b 0??????? ???????? ???????? ???????1
        public static bool IsPrime(Int32 value) {
            unchecked {
                if (64 > value) {
                    if (2 > value) {
                        return false;
                    }
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, (UInt32)value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, (UInt32)value)) {
                    return false;
                }
                var d = (UInt32)value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, (UInt32)value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7, (UInt32)value, d, s)) {
                    return false;
                }
                if (value < 314821u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(61, (UInt32)value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        public static bool IsPrime(UInt16 value) {
            unchecked {
                if (64u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, value)) {
                    return false;
                }
                /*
                var d = value - 1;
                var firstCoefficient = Utilities.CountTrailingZeros(d);
                d >>= firstCoefficient;
                */
                var d = (uint)value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(3, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        public static bool IsPrime(Int16 value) {
            unchecked {
                if (64 > value) {
                    if (2 > value) {
                        return false;
                    }
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (!IsProbablePrimeTrialDivision(primes_2_3_7, euler_2_3_7, (uint)value)) {
                    return false;
                }
                if (!IsProbablePrimeTrialDivision(primes_5_11, euler_5_11, (uint)value)) {
                    return false;
                }
                var d = (uint)value >> 1;
                int s = 1;
                while (0u == (1u & d)) {
                    d >>= 1;
                    ++s;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2, (uint)value, d, s)) {
                    return false;
                }
                if (value < 2047u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(3, (uint)value, d, s)) {
                    return false;
                }
                return true;
            }
        }
    }
}
