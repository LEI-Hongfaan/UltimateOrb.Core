using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb {
    using Helper = System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides miscellaneous functions.
    /// </summary>
#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class Miscellaneous {

        /// <summary>
        /// Hints that a boolean value is probably true.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool Likely(bool value) {
            return value;
        }

        /// <summary>
        /// Hints that a boolean value is probably false.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool Unlikely(bool value) {
            return value;
        }

        /// <summary>
        /// Specifies a parameter that is intentionally ignored.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter to be ignored.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            ret
        ")]
        public static void IgnoreOutParameter<T>(out T value) {
            Unsafe.SkipInit(out value!);
        }

        /// <summary>
        /// Specifies a parameter that is intentionally ignored.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter to be ignored.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void IgnoreInParameter<T>(in T value) {
#pragma warning restore IDE0060 // Remove unused parameter
        }

        /// <summary>
        /// Specifies a parameter that is intentionally ignored.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The parameter to be ignored.</param>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void IgnoreParameter<T>(T value) {
#pragma warning restore IDE0060 // Remove unused parameter
        }

#if NET7_0_OR_GREATER
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal static T HighestBitSetInternal<T>() where T : IBinaryInteger<T> {
            return ~(T.AllBitsSet >>> 1);
        }
#endif

#if !STANDALONE_XINTN_LIBRARY
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MulSaturating<T>(T first, [ConstantExpected] T second)
            where T : unmanaged, IBinaryInteger<T>, IUnsignedNumber<T>, IMinMaxValue<T> {
            var bound = T.One << (4 * Unsafe.SizeOf<T>());
            return ((second >= bound) || (first >= bound))
                && (second > T.Zero)
                && ((T.MaxValue / second) < first)
                ? T.MaxValue
                : (first * second);
        }
#endif
    }
}
