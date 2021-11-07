using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BooleanIntegerModule = UltimateOrb.Utilities.BooleanIntegerModule;

namespace UltimateOrb {

    /// <summary>
    /// Provides helper functions related to <see cref="CanonicalIntegerBoolean"/>.
    /// </summary>
    public static partial class CanonicalIntegerBooleanModule {
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {

        /// <summary>
        /// Converts an integer to a <see cref="CanonicalIntegerBoolean"/> value. No checks will be performed in release mode.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>A <see cref="CanonicalIntegerBoolean"/> value. The internal representation of the <see cref="CanonicalIntegerBoolean"/> value is specified by the integer.</returns>
        public static CanonicalIntegerBoolean ToCanonicalIntegerBooleanUnsafe(this int value) {
            return new CanonicalIntegerBoolean(value);
        }

        /// <summary>
        /// Converts an integer to a <see cref="CanonicalIntegerBoolean"/> value. No checks will be performed in release mode.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>A <see cref="CanonicalIntegerBoolean"/> value. The internal representation of the <see cref="CanonicalIntegerBoolean"/> value is specified by the integer.</returns>
        public static CanonicalIntegerBoolean ToCanonicalIntegerBooleanUnsafe(this uint value) {
            return new CanonicalIntegerBoolean(value);
        }
        /// <summary>
        /// Converts a <see cref="bool"/> to a <see cref="CanonicalIntegerBoolean"/> value. No checks will be performed in release mode.
        /// </summary>
        /// <param name="value">The <see cref="bool"/> to convert.</param>
        /// <returns>A <see cref="CanonicalIntegerBoolean"/> value. The internal representation of the <see cref="CanonicalIntegerBoolean"/> value is specified by the <see cref="bool"/>.</returns>
        public static CanonicalIntegerBoolean ToCanonicalIntegerBooleanUnsafe(this bool value) {
            return new CanonicalIntegerBoolean(value);
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsMinusOne(Int32 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsMinusOne(UInt32 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsMinusOne(Int64 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsMinusOne(UInt64 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsMinusOne(IntPtr value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsMinusOne(UIntPtr value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsMinusOne(value));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsZero(Int32 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsZero(UInt32 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsZero(Int64 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsZero(UInt64 value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        public static CanonicalIntegerBoolean IsZero(IntPtr value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }

        /// <summary>
        /// Compares <paramref name="value"/> to -1 (bitwise complement of 0).
        /// </summary>
        /// <param name="value">The value to compare.</param>
        /// <returns><c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if the value is equal to -1 (bitwise complement of 0); otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.</returns>
        [CLSCompliant(false)]
        public static CanonicalIntegerBoolean IsZero(UIntPtr value) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.IsZero(value));
        }
    }
}
