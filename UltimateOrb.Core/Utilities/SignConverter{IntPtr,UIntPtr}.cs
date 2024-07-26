using System;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class SignConverter {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static IntPtr ToSignedChecked(this IntPtr value) {
            return value;
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.u
            ret
        ")]
        public static UIntPtr ToUnsignedChecked(this IntPtr value) {
            return unchecked((UIntPtr)checked((ulong)unchecked((long)value)));
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        [ILMethodBodyAttribute(@"
            ldarg.0
            conv.ovf.i.un
            ret
        ")]
        public static IntPtr ToSignedChecked(this UIntPtr value) {
            return unchecked((IntPtr)checked((long)unchecked((ulong)value)));
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UIntPtr ToUnsignedChecked(this UIntPtr value) {
            return value;
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static IntPtr ToSignedUnchecked(this UIntPtr value) {
            return unchecked((IntPtr)(long)(ulong)value);
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static UIntPtr ToUnsignedUnchecked(this UIntPtr value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static IntPtr ToSignedUnchecked(this IntPtr value) {
            return value;
        }

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        [ILMethodBodyAttribute(@"
            ldarg.0
            ret
        ")]
        public static UIntPtr ToUnsignedUnchecked(this IntPtr value) {
            return unchecked((UIntPtr)(ulong)(long)value);
        }
    }
}
