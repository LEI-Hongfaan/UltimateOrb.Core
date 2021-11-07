using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UltimateOrb {
    using Helper = System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Provides miscellaneous functions.
    /// </summary>
    public static partial class Miscellaneous {

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
            pop
            ret
        ")]
        public static void IgnoreOutParameter<T>(out T value) {
            Helper.ValueAtReturn(out value);
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
    }
}
