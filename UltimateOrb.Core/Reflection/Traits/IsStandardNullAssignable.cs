namespace UltimateOrb.Reflection.Traits {

    public static partial class IsStandardNullAssignable<T> {

        public static readonly bool Value = GetValue();

        private static bool GetValue() {
            return null == default(T);
        }
    }
}
