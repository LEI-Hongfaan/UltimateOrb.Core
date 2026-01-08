using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb {

    public static partial class BinaryIntegerMath {

        [ModuleInitializer()]
        internal static void InitializeTypeTraitsForCommonTypes() {
            _ = BinaryIntegerTypeTraits<byte>.IsInitialized;
            _ = BinaryIntegerTypeTraits<sbyte>.IsInitialized;
            _ = BinaryIntegerTypeTraits<short>.IsInitialized;
            _ = BinaryIntegerTypeTraits<ushort>.IsInitialized;
            _ = BinaryIntegerTypeTraits<char>.IsInitialized;
            _ = BinaryIntegerTypeTraits<int>.IsInitialized;
            _ = BinaryIntegerTypeTraits<uint>.IsInitialized;
            _ = BinaryIntegerTypeTraits<nint>.IsInitialized;
            _ = BinaryIntegerTypeTraits<nuint>.IsInitialized;
            _ = BinaryIntegerTypeTraits<long>.IsInitialized;
            _ = BinaryIntegerTypeTraits<ulong>.IsInitialized;
            _ = BinaryIntegerTypeTraits<System.Int128>.IsInitialized;
            _ = BinaryIntegerTypeTraits<System.UInt128>.IsInitialized;
            //_ = BinaryIntegerTypeTraits<UltimateOrb.Int128>.IsInitialized;
            _ = BinaryIntegerTypeTraits<UltimateOrb.UInt128>.IsInitialized;
        }

        static T AbsUncheckedInternal<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(T value) where T : notnull, IBinaryInteger<T>? {
            if (BinaryIntegerTypeTraits<T>.IsBounded && BinaryIntegerTypeTraits<T>.IsSigned) {
                if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne < 1) {
                    throw new NotImplementedException();
                }
                // TODO:
                var mask = value >> unchecked(BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne - 1);
                return unchecked(value ^ mask - mask);
            }
            if (BinaryIntegerTypeTraits<T>.IsUnsigned) {
                return value;
            }
            return T.Abs(value);
        }

        public static T? AbsUnchecked<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(T? value) where T : IBinaryInteger<T>? {
            if (null == value) {
                return value;
            }
            return AbsUncheckedInternal((T)value);
        }

        public static T? AbsUnchecked<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)] T>(T? value) where T : struct, IBinaryInteger<T>? {
            if (null == value) {
                return value;
            }
            return AbsUncheckedInternal((T)value);
        }
    }
}
