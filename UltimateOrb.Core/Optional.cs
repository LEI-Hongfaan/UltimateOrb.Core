using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {
    /*
    readonly struct NullableOptional_SqlNullable<T> {

        readonly IntPtr IsNull;
    }

    public readonly struct NullableOptional<T> {

        public readonly T ValueOrDefault;

        public readonly bool HasValue;

        public NullableOptional(T value) {
            

            if (typeof(T).IsValueType) {
                if (typeof(System.Data.SqlTypes.INullable).IsAssignableFrom(typeof(T))) {
                    System.Data.SqlTypes.SqlChars
                }
            }

            if (IsNullAssignable<T>.Value) {
                if (IsStandardNullAssignable<T>.Value) {
                    return null == value;
                }

                if (IsINullableReference<T>.Value) {
                    return ((INullableReference)value).IsNull();
                }

                return !((INullable)value).HasValue;
            }
            return false;


            if (typeof(T).IsValueType) {

            }
            if (value is null) {
                ValueOrDefault = default!;
                HasValue = false;
            } else {

                
                ValueOrDefault = value;
                HasValue = true;
            }
        }

        public T GetValueOrDefault() => ValueOrDefault;
    }
    */
}
