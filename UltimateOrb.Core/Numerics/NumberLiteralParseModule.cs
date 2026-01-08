using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    static partial class NumberLiteralParseModule {

        internal static NumberLiteralParseResult ParseNumberLiteral(string text) {
            var regex = NumberLiteralRegexFactory.NumberLiteralRegex();
            var m = regex.Match(text);
            if (!m.Success) {
                return new NumberLiteralParseResult(default, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorInvalid), BigInteger.Zero, BigInteger.Zero, 0, 0);
            }

            // Determine sign field (2-bit) and IsNegative boolean
            NumberLiteralFlags signFlag = NumberLiteralFlags.SignUnsigned;
            bool isNegativeBool = false;
            if (m.Groups["sign"].Success) {
                var s = m.Groups["sign"].Value;
                if (s == "+") {
                    signFlag = NumberLiteralFlags.SignPositive;
                } else if (s == "-") {
                    signFlag = NumberLiteralFlags.SignNegative;
                    isNegativeBool = true;
                }
            } else {
                signFlag = NumberLiteralFlags.SignUnsigned;
            }

            // NaN: may include optional prefix and optional payload
            if (m.Groups["nan"].Success) {
                var prefix = m.Groups["nanPrefix"].Value;
                NumberLiteralFlags specialKind = NumberLiteralFlags.SpecialUnspecifiedNaN;
                if (!string.IsNullOrEmpty(prefix)) {
                    if (prefix.Equals("q", StringComparison.OrdinalIgnoreCase)) specialKind = NumberLiteralFlags.SpecialQuietNaN;
                    else if (prefix.Equals("s", StringComparison.OrdinalIgnoreCase)) specialKind = NumberLiteralFlags.SpecialSignalingNaN;
                    else specialKind = NumberLiteralFlags.SpecialUnspecifiedNaN;
                } else {
                    specialKind = NumberLiteralFlags.SpecialUnspecifiedNaN;
                }

                var payloadGroup = m.Groups["nanPayload"];
                if (payloadGroup.Success) {
                    var payloadText = payloadGroup.Value;
                    try {
                        BigInteger payloadValue;
                        if (payloadText.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) {
                            var payloadText0 = payloadText.AsSpan()[2..];
                            payloadValue = BigInteger.Parse(payloadText0, System.Globalization.NumberStyles.AllowHexSpecifier);
                            if (BigInteger.IsNegative(payloadValue)) {
                                payloadValue += BigInteger.One << checked(4 * payloadText0.Length);
                            }
                        } else {
                            payloadValue = BigInteger.Parse(payloadText);
                        }

                        // store payload in SignificandIntegralPart; fractional part zero
                        var flags = NumberLiteralFlags.IsNotFinite | specialKind;
                        flags = flags.WithSign(signFlag);
                        return new NumberLiteralParseResult(
                            IsNegative: isNegativeBool,
                            Flags: flags,
                            SignificandIntegralPart: payloadValue,
                            SignificandFractionalPart: BigInteger.Zero,
                            SignificandFractionalPartLength: 0,
                            Exponent: 0
                        );
                    } catch (FormatException) {
                        return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorMalformed).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                    } catch (OverflowException) {
                        return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorSignificandOverflow).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                    }
                } else {
                    var flags = NumberLiteralFlags.IsNotFinite | specialKind;
                    flags = flags.WithSign(signFlag);
                    return new NumberLiteralParseResult(isNegativeBool, flags, BigInteger.Zero, BigInteger.Zero, 0, 0);
                }
            }

            // Infinity
            if (m.Groups["inf"].Success) {
                var flags = NumberLiteralFlags.IsNotFinite | NumberLiteralFlags.SpecialInfinity;
                flags = flags.WithSign(signFlag);
                return new NumberLiteralParseResult(isNegativeBool, flags, BigInteger.Zero, BigInteger.Zero, 0, 0);
            }

            // Hex float
            if (m.Groups["hex"].Success) {
                try {
                    var sig = m.Groups["hexSignificand"].Value;
                    var hexExpSign = m.Groups["hexExpSign"].Value;
                    var hexExp = long.Parse(m.Groups["hexExp"].Value);
                    if (hexExpSign == "-") hexExp = -hexExp;

                    string intPartHex, fracPartHex;
                    if (sig.Contains('.')) {
                        var parts = sig.Split('.', 2);
                        intPartHex = parts[0].Length == 0 ? "0" : parts[0];
                        fracPartHex = parts[1].Length == 0 ? "0" : parts[1];
                    } else {
                        intPartHex = sig;
                        fracPartHex = "0";
                    }

                    BigInteger intPart = BigInteger.Parse(intPartHex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    if (BigInteger.IsNegative(intPart)) {
                        intPart += BigInteger.One << checked(4 * intPartHex.Length);
                    }
                    BigInteger fracPart = fracPartHex == "0" ? BigInteger.Zero :
                        BigInteger.Parse(fracPartHex, System.Globalization.NumberStyles.AllowHexSpecifier);
                    var fracLen = fracPartHex == "0" ? 0 : fracPartHex.Length;
                    if (BigInteger.IsNegative(fracPart)) {
                        fracPart += BigInteger.One << checked(4 * fracLen);
                    }

                    var flags = NumberLiteralFlags.IsFinite | NumberLiteralFlags.Hex;
                    flags = flags.WithSign(signFlag);

                    return new NumberLiteralParseResult(isNegativeBool, flags, intPart, fracPart, fracLen, hexExp);
                } catch (FormatException) {
                    return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorMalformed).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                } catch (OverflowException) {
                    return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorSignificandOverflow).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                }
            }

            // Decimal
            if (m.Groups["dec"].Success) {
                try {
                    var sig = m.Groups["decSignificand"].Value;
                    long decExp = 0;
                    if (m.Groups["decExp"].Success) {
                        decExp = long.Parse(m.Groups["decExp"].Value);
                        if (m.Groups["decExpSign"].Value == "-") decExp = -decExp;
                    }

                    string intPartDec, fracPartDec;
                    if (sig.Contains('.')) {
                        var parts = sig.Split('.', 2);
                        intPartDec = parts[0].Length == 0 ? "0" : parts[0];
                        fracPartDec = parts[1].Length == 0 ? "0" : parts[1];
                    } else {
                        intPartDec = sig;
                        fracPartDec = "0";
                    }

                    BigInteger intPart = BigInteger.Parse(intPartDec);
                    BigInteger fracPart = fracPartDec == "0" ? BigInteger.Zero : BigInteger.Parse(fracPartDec);
                    long fracLen = fracPartDec == "0" ? 0 : fracPartDec.Length;

                    var flags = NumberLiteralFlags.IsFinite; // decimal by default
                    flags = flags.WithSign(signFlag);

                    return new NumberLiteralParseResult(isNegativeBool, flags, intPart, fracPart, fracLen, decExp);
                } catch (FormatException) {
                    return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorMalformed).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                } catch (OverflowException) {
                    return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorSignificandOverflow).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
                }
            }

            // fallback: shouldn't reach
            return new NumberLiteralParseResult(isNegativeBool, NumberLiteralFlags.Error.WithErrorCode(NumberLiteralFlags.ErrorInvalid).WithSign(signFlag), BigInteger.Zero, BigInteger.Zero, 0, 0);
        }
    }
}
