using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace UltimateOrb {

    public static partial class BinaryIntegerExtensions {

        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(T @this) where T : IBinaryInteger<T>? {

            public static bool IsSigned => BinaryIntegerTypeTraits<T>.IsSigned;
            public static bool IsUnsigned => BinaryIntegerTypeTraits<T>.IsUnsigned;

            public static T? AbsUnchecked(T? value) {
                return BinaryIntegerMath.AbsUnchecked(value);
            }
        }

        extension<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(T? @this) where T : struct, IBinaryInteger<T>? {

            public static T? AbsUnchecked(T? value) {
                return BinaryIntegerMath.AbsUnchecked(value);
            }
        }
    }
}
