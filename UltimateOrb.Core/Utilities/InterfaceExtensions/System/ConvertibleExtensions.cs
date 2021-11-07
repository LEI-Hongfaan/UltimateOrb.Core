﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb;

namespace UltimateOrb.Utilities.InterfaceExtensions.System {

    public static partial class ConvertibleExtensions {

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetTypeCode<TConvertible>(this ref TConvertible @this) where TConvertible : struct, IConvertible {
            return @this.GetTypeCode();
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToBoolean(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Char ToChar<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToChar(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToSByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToUInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToUInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToUInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToSingle(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToDouble(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToDecimal(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToDateTime(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TConvertible>(this ref TConvertible @this, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToString(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static object ToType<TConvertible>(this ref TConvertible @this, Type conversionType, IFormatProvider? provider) where TConvertible : struct, IConvertible {
            return @this.ToType(conversionType, provider);
        }
    }

    public static partial class ConvertibleExtensions {

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static TypeCode GetTypeCode<TConvertible>(this TConvertible @this) where TConvertible : class, IConvertible {
            return @this.GetTypeCode();
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToBoolean(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Char ToChar<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToChar(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static sbyte ToSByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToSByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToByte(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int16 ToInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt16 ToUInt16<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToUInt16(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int32 ToInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt32 ToUInt32<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToUInt32(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 ToInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 ToUInt64<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToUInt64(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Single ToSingle<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToSingle(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Double ToDouble<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToDouble(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Decimal ToDecimal<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToDecimal(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static DateTime ToDateTime<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToDateTime(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<TConvertible>(this TConvertible @this, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToString(provider);
        }

        [CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static object ToType<TConvertible>(this TConvertible @this, Type conversionType, IFormatProvider? provider) where TConvertible : class, IConvertible {
            return @this.ToType(conversionType, provider);
        }
    }
}
