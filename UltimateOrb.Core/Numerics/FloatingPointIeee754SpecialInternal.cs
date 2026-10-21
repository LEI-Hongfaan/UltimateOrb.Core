using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    internal static partial class FloatingPointIeee754SpecialInternal {

        extension(MidpointRounding @this) {

            internal void ThrowIfNotSupported() {
                _ = @this switch {
                    System.MidpointRounding.ToEven => true,
                    System.MidpointRounding.AwayFromZero => true,
                    System.MidpointRounding.ToZero => true,
                    System.MidpointRounding.ToNegativeInfinity => true,
                    System.MidpointRounding.ToPositiveInfinity => true,
                    _ => throw new NotSupportedException($"The specified {nameof(System.MidpointRounding)} value '{@this}' is not supported."),
                };
            }
        }

        internal static bool TryFusedMultiplyAddSpecial<T>(
            T first, T second, T addend, MidpointRounding mode,
            [MaybeNullWhen(false)] out T result)
            where T : IFloatingPointIeee754<T> {
            // 'mode' only matters when the result is rounded. For NaN and infinite
            // results the value is uniquely determined and mode is irrelevant.
            _ = mode;

            // Any NaN operand → NaN.  Adding the operand to itself pushes it through
            // arithmetic so that signaling NaNs are quieted (IEEE 754 §7.2).  The
            // original priority (first, then second, then addend) is preserved.
            if (T.IsNaN(first)) {
                result = first + first;
                return true;
            }
            if (T.IsNaN(second)) {
                result = second + second;
                return true;
            }
            if (T.IsNaN(addend)) {
                result = addend + addend;
                return true;
            }

            // 0 × ±∞ → NaN (invalid operation).
            if ((T.IsInfinity(first) && T.IsZero(second)) ||
                (T.IsZero(first) && T.IsInfinity(second))) {
                result = first * second;
                return true;
            }

            // At least one operand is infinite.  mode is irrelevant here too.
            if (T.IsInfinity(first) || T.IsInfinity(second) || T.IsInfinity(addend)) {
                bool productInfinite = T.IsInfinity(first) || T.IsInfinity(second);

                if (productInfinite && T.IsInfinity(addend)) {
                    // Product is ±∞ and addend is ±∞.  Let arithmetic decide:
                    // same sign → ±∞, opposite sign → NaN (via ∞ + (−∞)).
                    result = first * second + addend;
                    return true;
                }
                if (T.IsInfinity(addend)) {
                    // Finite product + infinite addend → the addend.
                    result = addend;
                    return true;
                }
                // Infinite product + finite addend → the product (±∞).
                result = first * second;
                return true;
            }

            Debug.Assert(T.IsFinite(first));
            Debug.Assert(T.IsFinite(second));
            Debug.Assert(T.IsFinite(addend));
            result = default;
            return false;
        }
    }
}
