using System.Text.RegularExpressions;

namespace UltimateOrb.Numerics {
    // Source-generated regex: pattern must be a compile-time constant
    static partial class NumberLiteralRegexFactory {
        // pattern constant (must be literal for GeneratedRegex attribute)
        private const string NumberLiteralPattern =
            @"^(?<sign>[+\-])?" +
            @"(?:" +
                // Hex float: 0x<significand>[pP][+\-]?<digits>
                @"(?<hex>0[Xx](?<hexSignificand>(?:\.[0-9A-Fa-f]+|[0-9A-Fa-f]+(?:\.(?:[0-9A-Fa-f]+|))?))(?:P|p)(?<hexExpSign>[+\-]?)(?<hexExp>[0-9]+))" +
            @"|" +
                // Decimal integer/float with optional exponent
                @"(?<dec>(?<decSignificand>(?:\.[0-9]+|[0-9]+(?:\.(?:[0-9]+|))?))(?:[Ee](?<decExpSign>[+\-]?)(?<decExp>[0-9]+))?)" +
            @"|" +
                // NaN with optional single-letter prefix s|S|q|Q and optional payload: NaN(123) or NaN(0x1A)
                @"(?<nan>(?<nanPrefix>[SsQq]?)(?<nanWord>(?:[Nn][Aa][Nn]))(?:\((?<nanPayload>(?:0[Xx][0-9A-Fa-f]+|[0-9]+))\))?)" +
            @"|" +
                // Inf or Infinity (Inf or Infinity, mixed case allowed)
                @"(?<inf>∞|(?:[Ii][Nn][Ff](?:[Ii][Nn][Ii][Tt][Yy])?))" +
            @")$";

        // The GeneratedRegex attribute instructs the compiler to produce a source-generated Regex implementation.
        [GeneratedRegex(NumberLiteralPattern)]
        public static partial Regex NumberLiteralRegex();
    }
}
