namespace UltimateOrb.Numerics {

    static partial class SR {

        internal static readonly string Argument_InvalidEnumValue = "The value '{0}' is not valid for this usage of the type {1}.";
        internal static readonly string Arg_MustBeInt32OrNaN = "";
        internal static readonly string InvalidCast_FromTo = "Conversion from type '{0}' to type '{1}' is not valid.";
        
        internal static string? Format(string format, object? p1) {
            return string.Format(format, p1);
        }

        internal static string? Format(string format, object? p1, object? p2) {
            return string.Format(format, p1, p2);
        }

        internal static string? Format(string format, object? p1, object? p2, object? p3) {
            return string.Format(format, p1, p2, p3);
        }

        public static string Arg_BinaryStyleNotSupported { get; } = "The number style AllowBinarySpecifier is not supported on floating point data types.";

        public static string Argument_InvalidNumberStyles { get; } = "An undefined NumberStyles value is being used.";
     
        public static string Arg_InvalidHexFloatStyle { get; } = "With the AllowHexSpecifier bit set in the enum bit field, the only other valid bits that can be combined into the enum value must be a subset of HexFloat (AllowLeadingWhite, AllowTrailingWhite, AllowLeadingSign, AllowDecimalPoint, and AllowExponent). AllowExponent is required when AllowHexSpecifier is specified.";

        public static string ArgumentOutOfRange_Generic_MustBeGreater { get; } = "{0} ('{1}') must be greater than '{2}'.";
        public static string ArgumentOutOfRange_Generic_MustBeNonNegative { get; } = "{0} ('{1}') must be a non-negative value.";
    }
}
