using System.Numerics;

namespace UltimateOrb.Numerics {
    record struct NumberLiteralParseResult(
        bool IsNegative,
        NumberLiteralFlags Flags,
        BigInteger SignificandIntegralPart,
        BigInteger SignificandFractionalPart,
        long SignificandFractionalPartLength,
        long Exponent
    );
}
