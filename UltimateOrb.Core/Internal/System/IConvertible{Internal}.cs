using System;
using System.Runtime.CompilerServices;

namespace Internal.System {

    [DiscardableAttribute()]
    internal static class IConvertibleModule {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetTypeCode<TConvertible>(this TConvertible @this) where TConvertible : IConvertible {
            return @this.GetTypeCode();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToBoolean(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Char ToChar<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToChar(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSByte(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToByte(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt16(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt16(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt32(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt32(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt64(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt64(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSingle(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDouble(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDecimal(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDateTime(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToString(provider);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static object ToType<TConvertible>(this TConvertible @this, Type conversionType, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToType(conversionType, provider);
        }
    }
}
