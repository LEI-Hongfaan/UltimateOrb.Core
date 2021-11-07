using System;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace UltimateOrb.Utilities {

    public static partial class CheckedNoThrow {

        [CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static bool TryAdd(int first, uint second, out int result) {
            unchecked {
                var t = (long)first + second;
                result = (int)t;
                return int.MinValue <= t && t <= int.MaxValue;
            }
        }
    }
}
