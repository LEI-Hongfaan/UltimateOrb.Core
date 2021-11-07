namespace UltimateOrb.Reflection.Traits {

    public static partial class IsStandardNullableValueType<T> {

        public static readonly bool Value = GetValue();

        private static bool GetValue() {
            return null != System.Nullable.GetUnderlyingType(typeof(T));
        }
    }
}
