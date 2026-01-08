using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {
    /*
    readonly struct NullableOptional_SqlNullable<TBase> {

        readonly IntPtr IsNull;
    }

    public readonly struct NullableOptional<TBase> {

        public readonly TBase ValueOrDefault;

        public readonly bool HasValue;

        public NullableOptional(TBase value) {
            

            if (typeof(TBase).IsValueType) {
                if (typeof(System.Data.SqlTypes.INullable).IsAssignableFrom(typeof(TBase))) {
                    System.Data.SqlTypes.SqlChars
                }
            }

            if (IsNullAssignable<TBase>.Value) {
                if (IsStandardNullAssignable<TBase>.Value) {
                    return null == value;
                }

                if (IsINullableReference<TBase>.Value) {
                    return ((INullableReference)value).IsNull();
                }

                return !((INullable)value).HasValue;
            }
            return false;


            if (typeof(TBase).IsValueType) {

            }
            if (value is null) {
                ValueOrDefault = default!;
                HasValue = false;
            } else {

                
                ValueOrDefault = value;
                HasValue = true;
            }
        }

        public TBase GetValueOrDefault() => ValueOrDefault;
    }
    */
}
