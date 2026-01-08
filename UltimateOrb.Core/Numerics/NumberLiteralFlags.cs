using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    [Flags]
    enum NumberLiteralFlags : int {
        // Kind (2 bits: mask 0b11 at bits 0..1)
        KindMask = 0b11 << 0, // bits 0-1
        Empty = 0b00 << 0, // 00
        IsFinite = 0b01 << 0, // 01
        IsNotFinite = 0b10 << 0, // 10 (NaN/Infinity)
        Error = 0b11 << 0, // 11 (critical errors only)

        // SpecialKind (2 bits) - bits 2..3
        SpecialMask = 0b11 << 2, // bits 2-3
        SpecialInfinity = 0b00 << 2, // 00
        SpecialQuietNaN = 0b01 << 2, // 01
        SpecialSignalingNaN = 0b10 << 2, // 10
        SpecialUnspecifiedNaN = 0b11 << 2, // 11

        // Numeric kind: single bit for Hex - bit 4
        Hex = 1 << 4,    // set if hexadecimal float or hex NaN payload

        // Sign field (2 bits) occupies bits 5..6
        SignMask = 0b11 << 5, // bits 5 and 6
        SignUnsigned = 0 << 5,    // 00
        SignPositive = 1 << 5,    // 01  (explicit '+')
        IsNegative = 2 << 5,
        SignNegative = 3 << 5,    // 11  (explicit '-') and negative

        // Error code field occupies bits 7..9 (3 bits) - higher than Sign
        ErrorCodeMask = 0b111 << 7, // bits 7,8,9
        ErrorNone = 0 << 7,
        ErrorInvalid = 1 << 7,

        ErrorSignificandOverflow = 2 << 7,
        ErrorExponentOverflow = 3 << 7, // not critical if overflow towards -inf or significand is exactly zero
        ErrorMalformed = 4 << 7
    }

    static class LiteralFlagsExtensions {
        // Kind helpers (bits 0..1)
        public static NumberLiteralFlags WithKind(this NumberLiteralFlags flags, NumberLiteralFlags kind) {
            var cleared = flags & ~NumberLiteralFlags.KindMask;
            return cleared | (kind & NumberLiteralFlags.KindMask);
        }
        public static NumberLiteralFlags GetKind(this NumberLiteralFlags flags) {
            return flags & NumberLiteralFlags.KindMask;
        }
        /*
        public static string KindToString(this NumberLiteralFlags flags) {
            var k = flags.GetKind();
            return k switch {
                NumberLiteralFlags.Empty => "Empty",
                NumberLiteralFlags.IsFinite => "IsFinite",
                NumberLiteralFlags.IsNotFinite => "IsNotFinite",
                NumberLiteralFlags.Error => "Error",
                _ => "Reserved"
            };
        }
        */
        // SpecialKind helpers (bits 2..3)
        public static NumberLiteralFlags WithSpecial(this NumberLiteralFlags flags, NumberLiteralFlags special) {
            var cleared = flags & ~NumberLiteralFlags.SpecialMask;
            return cleared | (special & NumberLiteralFlags.SpecialMask);
        }
        public static NumberLiteralFlags GetSpecial(this NumberLiteralFlags flags) {
            return flags & NumberLiteralFlags.SpecialMask;
        }
        /*
        public static string SpecialToString(this NumberLiteralFlags flags) {
            var s = flags.GetSpecial();
            return s switch {
                NumberLiteralFlags.SpecialInfinity => "Infinity",
                NumberLiteralFlags.SpecialQuietNaN => "QuietNaN",
                NumberLiteralFlags.SpecialSignalingNaN => "SignalingNaN",
                NumberLiteralFlags.SpecialUnspecifiedNaN => "UnspecifiedNaN",
                _ => "Reserved"
            };
        }
        */
        // Sign helpers (bits 5..6)
        public static NumberLiteralFlags WithSign(this NumberLiteralFlags flags, NumberLiteralFlags sign) {
            var cleared = flags & ~NumberLiteralFlags.SignMask;
            return cleared | (sign & NumberLiteralFlags.SignMask);
        }
        public static NumberLiteralFlags GetSign(this NumberLiteralFlags flags) {
            return flags & NumberLiteralFlags.SignMask;
        }
        /*
        public static string SignToString(this NumberLiteralFlags flags) {
            var s = flags.GetSign();
            return s switch {
                NumberLiteralFlags.SignUnsigned => "Unsigned",
                NumberLiteralFlags.SignPositive => "Positive",
                NumberLiteralFlags.SignNegative => "Negative",
                _ => "Reserved"
            };
        }
        */
        // Error code helpers (bits 7..9)
        public static NumberLiteralFlags WithErrorCode(this NumberLiteralFlags flags, NumberLiteralFlags errorCode) {
            var cleared = flags & ~NumberLiteralFlags.ErrorCodeMask;
            return cleared | (errorCode & NumberLiteralFlags.ErrorCodeMask);
        }
        public static NumberLiteralFlags GetErrorCode(this NumberLiteralFlags flags) {
            return flags & NumberLiteralFlags.ErrorCodeMask;
        }
        public static bool HasError(this NumberLiteralFlags flags) {
            return (flags & NumberLiteralFlags.ErrorCodeMask) != NumberLiteralFlags.ErrorNone;
        }

        /*
        public static string ErrorCodeToString(this NumberLiteralFlags flags) {
            var code = flags.GetErrorCode();
            return code switch {
                NumberLiteralFlags.ErrorNone => "None",
                NumberLiteralFlags.ErrorInvalid => "Invalid",
                NumberLiteralFlags.ErrorSignificandOverflow => "SignificandOverflow",
                NumberLiteralFlags.ErrorExponentOverflow => "ExponentOverflow",
                NumberLiteralFlags.ErrorMalformed => "Malformed",
                _ => "Reserved"
            };
        }
        */

        // Determine whether the encoded error should be treated as critical,
        // given the parsed significand. Per request: ErrorExponentOverflow is NOT critical
        // when the significand is exactly zero (both integral and fractional parts zero).
        public static bool HasCriticalError(this NumberLiteralFlags flags, BigInteger significandIntegral, BigInteger significandFractional) {
            var code = flags.GetErrorCode();
            if (code == NumberLiteralFlags.ErrorNone) return false;
            if (code == NumberLiteralFlags.ErrorExponentOverflow) {
                // Not critical if significand is exactly zero
                if (significandIntegral.IsZero && significandFractional.IsZero) return false;
                return true;
            }
            // All other error codes are considered critical
            return true;
        }
    }
}
