using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb;

namespace UltimateOrb.Utilities.InterfaceReadOnlyExtensions.System {

    public static partial class ConvertibleExtensions {

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetTypeCode<TConvertible>(this TConvertible @this) where TConvertible : IConvertible {
            return @this.GetTypeCode();
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToBoolean(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Char ToChar<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToChar(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSingle(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDouble(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDecimal(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDateTime(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToString(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static object ToType<TConvertible>(this TConvertible @this, Type conversionType, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToType(conversionType, provider);
        }
    }
}
