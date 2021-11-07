using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb;

namespace UltimateOrb.Utilities.InterfaceHelpers.System {

    public static partial class ConvertibleHelper {

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetTypeCode<TConvertible>(ref TConvertible @this) where TConvertible : IConvertible {
            return @this.GetTypeCode();
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToBoolean(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Char ToChar<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToChar(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToUInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToSingle(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDouble(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDecimal(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToDateTime(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TConvertible>(ref TConvertible @this, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToString(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static object ToType<TConvertible>(ref TConvertible @this, Type conversionType, IFormatProvider? provider) where TConvertible : IConvertible {
            return @this.ToType(conversionType, provider);
        }
    }
}
