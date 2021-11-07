namespace UltimateOrb.Utilities.Extensions {

    public static partial class CanonicalIntegerBooleanExtensions {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToInteger(this CanonicalIntegerBoolean value) {
            return (int)value;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint ToUInteger(this CanonicalIntegerBoolean value) {
            return unchecked((uint)(int)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static CanonicalIntegerBoolean ToBoolean(this int value) {
            return 0 != value;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static CanonicalIntegerBoolean ToBoolean(this uint value) {
            return 0 != value;
        }

        /// <devdoc>
        ///     <para>
        ///         ECMA-335: (III.1.1.1 Numeric data types - Short integers)
        ///         <c>bool</c> or <see cref="System.Boolean"/> (8-bit) is zero-extended.
        ///     </para>
        /// </devdoc>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int AsIntegerUnsafe(this CanonicalIntegerBoolean canonical) {
            return (int)canonical;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static CanonicalIntegerBoolean AsBooleanUnsafe(this int canonical) {
            return new CanonicalIntegerBoolean(canonical);
        }

        /// <devdoc>
        ///     <para>
        ///         ECMA-335: (III.1.1.1 Numeric data types - Short integers)
        ///         <c>bool</c> or <see cref="System.Boolean"/> (8-bit) is zero-extended.
        ///     </para>
        /// </devdoc>
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint AsUIntegerUnsafe(this CanonicalIntegerBoolean canonical) {
            return unchecked((uint)(int)canonical);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static CanonicalIntegerBoolean AsBooleanUnsafe(this uint canonical) {
            return new CanonicalIntegerBoolean(canonical);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToIntegerFromCanonical(this CanonicalIntegerBoolean canonical) {
            return canonical.AsIntegerUnsafe();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ToIntegerFromCanonicalChecked(this CanonicalIntegerBoolean canonical) {
            _ = checked(unchecked((uint)(int)(-2)) + unchecked((uint)canonical.AsIntegerUnsafe()));
            return canonical.AsIntegerUnsafe();
        }
    }
}

namespace UltimateOrb.Utilities.Extensions {

    public static partial class CanonicalIntegerBooleanExtensions {


    }
}