using UltimateOrb.Runtime.CompilerServices.TypeTokens;

namespace UltimateOrb.Utilities.Extensions {

    public static partial class BooleanIntegerExtensions {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToInteger(this bool value) {
            var result = 0;
            if (value) {
                result = 1;
            }
            return result;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint ToUInteger(this bool value) {
            var result = 0u;
            if (value) {
                result = 1u;
            }
            return result;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool ToBoolean(this int value) {
            return 0 != value;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool ToBoolean(this uint value) {
            return 0 != value;
        }

        /// <devdoc>
        ///     <para>
        ///         ECMA-335: (III.1.1.1 Numeric data types - Short integers)
        ///         <c>bool</c> or <see cref="System.Boolean"/> (8-bit) is zero-extended.
        ///     </para>
        ///     <para>
        ///         The body of this method will be modified by the build tools.
        ///     </para>
        /// </devdoc>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        public static int AsIntegerUnsafe(this bool canonical) {
            return canonical.ToInteger();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        public static bool AsBooleanUnsafe(this int canonical) {
            return ToBoolean(canonical);
        }

        /// <devdoc>
        ///     <para>
        ///         ECMA-335: (III.1.1.1 Numeric data types - Short integers)
        ///         <c>bool</c> or <see cref="System.Boolean"/> (8-bit) is zero-extended.
        ///     </para>
        ///     <para>
        ///         The body of this method will be modified by the build tools.
        ///     </para>
        /// </devdoc>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        public static uint AsUIntegerUnsafe(this bool canonical) {
            return unchecked((uint)canonical.ToInteger());
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        [ILMethodBodyAttribute(@"
            LdArg.0
            Ret
        ")]
        public static bool AsBooleanUnsafe(this uint canonical) {
            return ToBoolean(canonical);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToIntegerFromCanonical(this bool canonical) {
            System.Diagnostics.Debug.Assert(unchecked((uint)canonical.AsIntegerUnsafe()) <= 1);
            return canonical.AsIntegerUnsafe();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToIntegerFromCanonicalChecked(this bool canonical) {
            _ = checked(unchecked((uint)(int)(-1)) + unchecked((uint)canonical.AsIntegerUnsafe()));
            return canonical.AsIntegerUnsafe();
        }
    }
}
