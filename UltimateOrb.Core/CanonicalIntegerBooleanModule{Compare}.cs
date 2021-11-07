using System;
using BooleanIntegerModule = UltimateOrb.Utilities.BooleanIntegerModule;

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(uint first, uint second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(uint first, uint second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(uint first, uint second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(uint first, uint second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(uint first, uint second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(int first, int second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(int first, int second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(int first, int second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(int first, int second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(int first, int second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(ulong first, ulong second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(ulong first, ulong second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(ulong first, ulong second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(ulong first, ulong second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(ulong first, ulong second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(long first, long second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(long first, long second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(long first, long second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(long first, long second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(long first, long second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(float first, float second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(float first, float second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(float first, float second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(float first, float second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(float first, float second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(double first, double second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(double first, double second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(double first, double second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(double first, double second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(double first, double second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(UIntPtr first, UIntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(UIntPtr first, UIntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(UIntPtr first, UIntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(UIntPtr first, UIntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(UIntPtr first, UIntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}

namespace UltimateOrb {

    public static partial class CanonicalIntegerBooleanModule {
        
        /// <summary>
        ///     <para>
        ///         Returns <c>(<see cref="CanonicalIntegerBoolean"/>)true</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>(<see cref="CanonicalIntegerBoolean"/>)false</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean Equals(IntPtr first, IntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.Equals(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThan(IntPtr first, IntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean LessThanOrEqual(IntPtr first, IntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.LessThanOrEqual(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThan(IntPtr first, IntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThan(first, second));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static CanonicalIntegerBoolean GreaterThanOrEqual(IntPtr first, IntPtr second) {
            return new CanonicalIntegerBoolean(BooleanIntegerModule.GreaterThanOrEqual(first, second));
        }
    }
}
