using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    internal static partial class ConvertInternal {

        internal static object DefaultToType<TConvertible>(in TConvertible value, Type targetType, IFormatProvider? provider)
            where TConvertible : IConvertible {
            ArgumentNullException.ThrowIfNull(targetType);

            Debug.Assert(value != null, "[Convert.DefaultToType]value!=null");

            if (ReferenceEquals(value.GetType(), targetType)) {
                return value;
            }

            if (ReferenceEquals(targetType, typeof(bool))) {
                return value.ToBoolean(provider);
            }

            if (ReferenceEquals(targetType, typeof(char))) {
                return value.ToChar(provider);
            }

            if (ReferenceEquals(targetType, typeof(sbyte))) {
                return value.ToSByte(provider);
            }

            if (ReferenceEquals(targetType, typeof(byte))) {
                return value.ToByte(provider);
            }

            if (ReferenceEquals(targetType, typeof(short))) {
                return value.ToInt16(provider);
            }

            if (ReferenceEquals(targetType, typeof(ushort))) {
                return value.ToUInt16(provider);
            }

            if (ReferenceEquals(targetType, typeof(int))) {
                return value.ToInt32(provider);
            }

            if (ReferenceEquals(targetType, typeof(uint))) {
                return value.ToUInt32(provider);
            }

            if (ReferenceEquals(targetType, typeof(long))) {
                return value.ToInt64(provider);
            }

            if (ReferenceEquals(targetType, typeof(ulong))) {
                return value.ToUInt64(provider);
            }

            if (ReferenceEquals(targetType, typeof(float))) {
                return value.ToSingle(provider);
            }

            if (ReferenceEquals(targetType, typeof(double))) {
                return value.ToDouble(provider);
            }

            if (ReferenceEquals(targetType, typeof(decimal))) {
                return value.ToDecimal(provider);
            }

            if (ReferenceEquals(targetType, typeof(DateTime))) {
                return value.ToDateTime(provider);
            }

            if (ReferenceEquals(targetType, typeof(string))) {
                return value.ToString(provider);
            }

            if (ReferenceEquals(targetType, typeof(object))) {
                return (object)value;
            }
            // Need to special case Enum because typecode will be underlying type, e.g. Int32
            if (ReferenceEquals(targetType, typeof(Enum))) {
                return (Enum)(IConvertible)value;
            }
            throw new InvalidCastException();
            /*
            if (ReferenceEquals(targetType, typeof(DBNull))) {
                throw new InvalidCastException(SR.InvalidCast_DBNull);
            }
            if (ReferenceEquals(targetType, typeof(Empty))) {
                throw new InvalidCastException(SR.InvalidCast_Empty);
            }
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, value.GetType().FullName, targetType.FullName));
            */
        }
    }
}
