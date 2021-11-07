using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internal.System {
    using System = global::System;
    using SizeOfModule = UltimateOrb.Utilities.SizeOfModule;

    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public static partial class Math {

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt64 DivRem(UInt64 dividend, UInt64 divisor, out UInt64 remainder) {
            System.Diagnostics.Contracts.Contract.Requires(0u != divisor);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(divisor) > System.Diagnostics.Contracts.Contract.ValueAtReturn(out remainder));
            System.Diagnostics.Contracts.Contract.EnsuresOnThrow<DivideByZeroException>(!(0u != System.Diagnostics.Contracts.Contract.OldValue(divisor)));
            unchecked {
                // 2019Dec27
                if (true || SizeOfModule.SizeOf<UInt64>() > SizeOfModule.SizeOf<UIntPtr>()) {
                    var q = dividend / divisor;
                    remainder = dividend - q * divisor;
                    return q;
                }
                {
                    remainder = dividend % divisor;
                    return dividend / divisor;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt32 DivRem(UInt32 dividend, UInt32 divisor, out UInt32 remainder) {
            System.Diagnostics.Contracts.Contract.Requires(0u != divisor);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(divisor) > System.Diagnostics.Contracts.Contract.ValueAtReturn(out remainder));
            System.Diagnostics.Contracts.Contract.EnsuresOnThrow<DivideByZeroException>(!(0u != System.Diagnostics.Contracts.Contract.OldValue(divisor)));
            // 2019Dec27
            var q = dividend / divisor;
            remainder = dividend - q * divisor;
            return q;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static Int64 Remainder(Int64 dividend, Int64 divisor) {
            System.Diagnostics.Contracts.Contract.Requires(0u != divisor);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(dividend) <= 0 || 0 <= System.Diagnostics.Contracts.Contract.Result<Int64>());
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.OldValue(dividend) || System.Diagnostics.Contracts.Contract.Result<Int64>() <= 0);
            System.Diagnostics.Contracts.Contract.Ensures(0 > System.Diagnostics.Contracts.Contract.OldValue(divisor) || System.Diagnostics.Contracts.Contract.OldValue(divisor) > System.Diagnostics.Contracts.Contract.Result<Int64>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(divisor) > 0 || System.Diagnostics.Contracts.Contract.Result<Int64>() > System.Diagnostics.Contracts.Contract.OldValue(divisor));
            System.Diagnostics.Contracts.Contract.EnsuresOnThrow<DivideByZeroException>(!(0 != System.Diagnostics.Contracts.Contract.OldValue(divisor)));
            return -1 == divisor ? 0 : dividend % divisor;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static Int32 Remainder(Int32 dividend, Int32 divisor) {
            System.Diagnostics.Contracts.Contract.Requires(0u != divisor);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(dividend) <= 0 || 0 <= System.Diagnostics.Contracts.Contract.Result<Int64>());
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.OldValue(dividend) || System.Diagnostics.Contracts.Contract.Result<Int64>() <= 0);
            System.Diagnostics.Contracts.Contract.Ensures(0 > System.Diagnostics.Contracts.Contract.OldValue(divisor) || System.Diagnostics.Contracts.Contract.OldValue(divisor) > System.Diagnostics.Contracts.Contract.Result<Int64>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(divisor) > 0 || System.Diagnostics.Contracts.Contract.Result<Int64>() > System.Diagnostics.Contracts.Contract.OldValue(divisor));
            System.Diagnostics.Contracts.Contract.EnsuresOnThrow<DivideByZeroException>(!(0 != System.Diagnostics.Contracts.Contract.OldValue(divisor)));
            return -1 == divisor ? 0 : dividend % divisor;
        }
    }
}
