using System;

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(uint first, uint second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            Ret
        ")]
        public static int LessThan(uint first, uint second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(uint first, uint second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            Ret
        ")]
        public static int GreaterThan(uint first, uint second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(uint first, uint second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(int first, int second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            Ret
        ")]
        public static int LessThan(int first, int second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(int first, int second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            Ret
        ")]
        public static int GreaterThan(int first, int second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(int first, int second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(ulong first, ulong second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            Ret
        ")]
        public static int LessThan(ulong first, ulong second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(ulong first, ulong second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            Ret
        ")]
        public static int GreaterThan(ulong first, ulong second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(ulong first, ulong second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(long first, long second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            Ret
        ")]
        public static int LessThan(long first, long second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(long first, long second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            Ret
        ")]
        public static int GreaterThan(long first, long second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(long first, long second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(float first, float second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            Ret
        ")]
        public static int LessThan(float first, float second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(float first, float second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            Ret
        ")]
        public static int GreaterThan(float first, float second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(float first, float second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(double first, double second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            Ret
        ")]
        public static int LessThan(double first, double second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(double first, double second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            Ret
        ")]
        public static int GreaterThan(double first, double second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(double first, double second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(UIntPtr first, UIntPtr second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            Ret
        ")]
        public static int LessThan(UIntPtr first, UIntPtr second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(UIntPtr first, UIntPtr second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT.Un
            Ret
        ")]
        public static int GreaterThan(UIntPtr first, UIntPtr second) {
            throw null!;
        }

		[System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT.Un
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(UIntPtr first, UIntPtr second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {
        
#pragma warning disable IDE0060 // Remove unused parameter
        /// <summary>
        ///     <para>
        ///         Returns <c>1</c> if <paramref name="first"/> is equal to <paramref name="second"/>. Otherwise, <c>0</c>.
        ///     </para>
        ///     <para>
        ///         For floating-point numbers, this function will return 0 if the numbers are unordered (either or both are NaN). The infinite values are equal to themselves.
        ///     </para>
        /// </summary>
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CEq
            Ret
        ")]
        public static int Equals(IntPtr first, IntPtr second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            Ret
        ")]
        public static int LessThan(IntPtr first, IntPtr second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int LessThanOrEqual(IntPtr first, IntPtr second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CGT
            Ret
        ")]
        public static int GreaterThan(IntPtr first, IntPtr second) {
            throw null!;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [ILMethodBodyAttribute(@"
            LdArg.0
            LdArg.1
            CLT
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int GreaterThanOrEqual(IntPtr first, IntPtr second) {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
