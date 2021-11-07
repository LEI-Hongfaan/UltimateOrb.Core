using System.Runtime.CompilerServices;

namespace UltimateOrb {

    [Discardable]
    static partial class XInt128Helpers {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Try_HandleOutParameterIfFalse<T>(out T? result) {
            result = default;
        }
    }
}
