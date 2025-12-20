using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    }
}


namespace UltimateOrb.Mathematics.NumberTheory {
    using static IsPrimeModule;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    public static partial class IsPrimeModule {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision(uint primesModulus, ReadOnlySpan<char> eulerMask, uint value) {
            unchecked {
                // Wheel sieve check
                uint residue = (uint)(value % primesModulus);
                int wordIndex = (int)(residue >> 4); // /16
                int bitIndex = (int)(residue & 15);  // 0..15
                var mask = 1u << bitIndex;
                var word = Unsafe.Add(ref MemoryMarshal.GetReference(eulerMask), wordIndex);
                return (word & mask) != 0u;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision(uint primesModulus, ReadOnlySpan<char> eulerMask, ulong value) {
            unchecked {
                // Wheel sieve check
                uint residue = (uint)(value % primesModulus);
                int wordIndex = (int)(residue >> 4); // /16
                int bitIndex = (int)(residue & 15);  // 0..15
                var mask = 1u << bitIndex;
                var word = Unsafe.Add(ref MemoryMarshal.GetReference(eulerMask), wordIndex);
                return (word & mask) != 0u;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision(uint primesModulus, ReadOnlySpan<char> eulerMask, UInt128 value) {
            unchecked {
                // Wheel sieve check
                uint residue = (uint)(value % primesModulus);
                int wordIndex = (int)(residue >> 4); // /16
                int bitIndex = (int)(residue & 15);  // 0..15
                var mask = 1u << bitIndex;
                var word = Unsafe.Add(ref MemoryMarshal.GetReference(eulerMask), wordIndex);
                return (word & mask) != 0u;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision<T>(uint primesModulus, ReadOnlySpan<char> eulerMask, T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            if (typeof(T) == typeof(UInt32)) {
                return IsProbablePrimeTrialDivision(primesModulus, eulerMask, (UInt32)(object)value);
            }
            if (typeof(T) == typeof(UInt64)) {
                return IsProbablePrimeTrialDivision(primesModulus, eulerMask, (UInt64)(object)value);
            }
            if (typeof(T) == typeof(UInt128)) {
                return IsProbablePrimeTrialDivision(primesModulus, eulerMask, (UInt128)(object)value);
            }
            unchecked {
                // Wheel sieve check
                uint residue = uint.CreateTruncating(value % T.CreateTruncating(primesModulus));
                Debug.Assert(residue < primesModulus);
                int wordIndex = (int)(residue >> 4); // /16
                int bitIndex = (int)(residue & 15);  // 0..15
                var mask = 1u << bitIndex;
                var word = Unsafe.Add(ref MemoryMarshal.GetReference(eulerMask), wordIndex);
                return (word & mask) != 0u;
            }
        }
    }


    public static partial class IsPrimeModule {

        internal const uint PrimesModulus_3_5_17 = checked(3u * 5u * 17u);

        internal const string EulerMask_3_5_17 =
            "\u6996\uB4C9\uDA61\u6D32\uB689\u5B4C\u2DA6\u9653" +
            "\uCA69\u65B4\u32DA\u916D\u4CB6\u865B\u932D\u6996";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision_3_5_17<T>(T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            return IsProbablePrimeTrialDivision(PrimesModulus_3_5_17, EulerMask_3_5_17, value);
        }
    }

    public static partial class IsPrimeModule {

        internal const uint PrimesModulus_11_23 = checked(11u * 23u);

        internal const string EulerMask_11_23 =
            "\uF7FE\uFF3F\uAFFD\uFF7F\uDFDB\uEEFF\uBFF7\uFDF7" +
            "\u7BEF\uFBFF\uFFDD\uF6FE\u7FBF\uEFFD\uFF3F\uDFFB";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision_11_23<T>(T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            return IsProbablePrimeTrialDivision(PrimesModulus_11_23, EulerMask_11_23, value);
        }
    }

    public static partial class IsPrimeModule {

        internal const uint PrimesModulus_13_19 = checked(13u * 19u);

        internal const string EulerMask_13_19 =
            "\uDFFE\uFBF7\uFF3F\uFDEF\uAFFD\u77FF\uFEFF\uFFDB" +
            "\u7FDB\uEEFF\uF5FF\uBFBF\uFFF7\uDFFC\uFBEF\uFF7F";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision_13_19<T>(T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            return IsProbablePrimeTrialDivision(PrimesModulus_13_19, EulerMask_13_19, value);
        }
    }

    public static partial class IsPrimeModule {

        internal const uint PrimesModulus_7_29 = checked(7u * 29u);

        internal const string EulerMask_7_29 =
            "\uBF7E\uCFDF\uFBF7\u7AFD\uDFBF\uF76F\uFDFB\uBF6E" +
            "\uEFDF\uFBF5\u3EFD\uDFBF\uF7EF\uFDFB\uBE7E\uEFDF";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision_7_29<T>(T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            return IsProbablePrimeTrialDivision(PrimesModulus_7_29, EulerMask_7_29, value);
        }
    }

    public static partial class IsPrimeModule {


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsProbablePrimeTrialDivision_3To29<T>(T value)
            where T : notnull, IUnsignedNumber<T>, IBinaryInteger<T> {
            if (!IsProbablePrimeTrialDivision_3_5_17(value)) {
                return false;
            }
            if (!IsProbablePrimeTrialDivision_11_23(value)) {
                return false;
            }
            if (!IsProbablePrimeTrialDivision_13_19(value)) {
                return false;
            }
            if (!IsProbablePrimeTrialDivision_7_29(value)) {
                return false;
            }
            return true;
        }
    }

    public static partial class IsPrimeModule {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrimeInternal(UInt16 value) {
            Debug.Assert(0u != (1u & value));
            Debug.Assert(64u <= value);
            unchecked {
                if (!IsProbablePrimeTrialDivision_3To29(value)) {
                    return false;
                }
                var d = (uint)value - 1;
                var s = BitOperations.TrailingZeroCount(d);
                d >>>= s;
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2u, value, d, s)) {
                    return false;
                }
                if (value < 2047u + 1986u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7u, value, d, s)) {
                    return false;
                }
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrimePartialInternal(UInt16 value) {
            Debug.Assert(0u != (1u & value));
            Debug.Assert(64u <= value);
            Debug.Assert(0 <= (Int16)value);
            unchecked {
                if (!IsProbablePrimeTrialDivision_3To29(value)) {
                    return false;
                }
                var d = (uint)value - 1;
                var s = BitOperations.TrailingZeroCount(d);
                d >>>= s;
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(921211727u, value, d, s)) {
                    return value == 331u;
                }
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(UInt16 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64u + 2u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & value)) {
                    return false;
                }
                return IsOddPrimeInternal(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(UInt16 value) {
            unchecked {
                if (0u == (1u & value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64u + 5u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimeInternal(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(Int16 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64 + 2 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & value)) {
                    return false;
                }
                return IsOddPrimePartialInternal((UInt16)value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(Int16 value) {
            unchecked {
                if (0u == (1u & (UInt16)value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64 + 5 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimePartialInternal((UInt16)value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsOddPrimeInternal(UInt32 value) {
            unchecked {
                if (!IsProbablePrimeTrialDivision_3To29(value)) {
                    return false;
                }
                var d = value - 1;
                var s = BitOperations.TrailingZeroCount(d);
                d >>>= s;
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2u, value, d, s)) {
                    return false;
                }
                if (value < 2047u + 1986u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(7u, value, d, s)) {
                    return false;
                }
                if (value < 314821u + 1954272u) {
                    return true;
                }
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(61u, value, d, s)) {
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(UInt32 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64u + 2u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & value)) {
                    return false;
                }
                return IsOddPrimeInternal(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(UInt32 value) {
            unchecked {
                if (0u == (1u & value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64u + 5u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimeInternal(value);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(Int32 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64 + 2 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & (UInt32)value)) {
                    return false;
                }
                return IsOddPrimeInternal((UInt32)value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(Int32 value) {
            unchecked {
                if (0u == (1u & value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64 + 5 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimeInternal((UInt32)value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool IsOddPrimeInternal(UInt64 value) {
            unchecked {
                if (value <= UInt32.MaxValue) {
                    return IsOddPrimeInternal((UInt32)value);
                }
                if (!IsProbablePrimeTrialDivision_3To29(value)) {
                    return false;
                }
                var d = value - 1;
                var s = BitOperations.TrailingZeroCount(d);
                d >>>= s;
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(2u, value, d, s)) {
                    return false;
                }
                //if (value < 2047u + 1986u) {
                //    return true;
                //}
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(450775u, value, d, s)) {
                    return false;
                }
                //if (value < 1373653u) {
                //    return true;
                //}
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(325u, value, d, s)) {
                    return false;
                }
                //if (value < 95452781u) {
                //    return true;
                //}
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(9375u, value, d, s)) {
                    return false;
                }
                //if (value < 3874471147u) {
                //    return true;
                //}
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(28178u, value, d, s)) {
                    return false;
                } // 3874471147
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(9780504u, value, d, s)) {
                    return false;
                } // ?
                if (!IsPrimeMillerRabinModule.IsMillerRabinPseudoprimeInternal(1795265022u, value, d, s)) {
                    return false;
                } // ?
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(UInt64 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64u + 2u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & value)) {
                    return false;
                }
                return IsOddPrimeInternal(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(UInt64 value) {
            unchecked {
                if (0u == (1u & value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64u + 5u > value) {
                    return 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimeInternal(value);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsPrime(Int64 value) {
            unchecked {
                // 64 ~ 0 in prime_table
                // 65 ~ 1 in prime_table
                // 66 !~ 2 in prime_table
                if (64 + 2 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                if (0u == (1u & value)) {
                    return false;
                }
                return IsOddPrimeInternal((UInt64)value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsOddPrime(Int64 value) {
            unchecked {
                if (0u == (1u & value)) {
                    return false;
                }
                // 65 ~ 1 in prime_table
                // 67 ~ 3 in prime_table
                // 69 !~ 5 in prime_table
                if (64 + 5 > value) {
                    return value >= 0 && 0 != (1 & (int)(prime_table >> (int)value));
                }
                return IsOddPrimeInternal((UInt64)value);
            }
        }
    }
}