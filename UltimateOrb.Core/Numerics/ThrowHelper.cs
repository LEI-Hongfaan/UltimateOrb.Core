using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {

    internal static partial class ThrowHelper {

        [DoesNotReturn]
        internal static void ThrowArgumentException_DestinationTooShort() {
            throw new ArgumentException();
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_values() {
            throw new ArgumentOutOfRangeException("values");
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }

        [DoesNotReturn]
        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess() {
            throw new ArgumentOutOfRangeException();
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_InvalidEnumValue<TEnum>(TEnum value, [CallerArgumentExpression(nameof(value))] string argumentName = "") {
            throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, value, typeof(TEnum).Name), argumentName);
        }
    }
}