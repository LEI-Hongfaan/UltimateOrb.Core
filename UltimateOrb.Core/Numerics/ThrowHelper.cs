using System;
using System.Diagnostics.CodeAnalysis;

namespace UltimateOrb.Numerics {

    internal static partial class ThrowHelper {

        internal static void ThrowArgumentException_DestinationTooShort() {
            throw new ArgumentException();
        }

        internal static void ThrowArgumentOutOfRangeException_values() {
            throw new ArgumentOutOfRangeException("values");
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }

        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess() {
            throw new ArgumentOutOfRangeException();
        }
    }
}