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
    }
}
