using System;
using System.Diagnostics.CodeAnalysis;

namespace UltimateOrb.Numerics {
    internal class ThrowHelper {

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }
    }
}