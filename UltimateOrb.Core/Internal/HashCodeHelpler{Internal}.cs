using System;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UltimateOrb.Numerics;

namespace Internal {

    [DiscardableAttribute()]
    internal static partial class HashCodeHelper {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(Int32 value) {
            return unchecked((int)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(UInt32 value) {
            return GetHashCode(unchecked((Int32)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(Int64 value) {
            return GetHashCode(unchecked((Int32)value)) ^ GetHashCode(unchecked((Int32)(value >> 32)));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(UInt64 value) {
            return GetHashCode(unchecked((Int64)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(UltimateOrb.Int128 value) {
            return GetHashCode(unchecked((UltimateOrb.UInt128)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(UltimateOrb.UInt128 value) {
            return GetHashCode(value.GetLowPart() ^ value.GetHighPart());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(global::System.Int128 value) {
            return GetHashCode(unchecked((global::System.UInt128)value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int GetHashCode(global::System.UInt128 value) {
            return GetHashCode(value.GetLowPart() ^ value.GetHighPart());
        }
    }
}
