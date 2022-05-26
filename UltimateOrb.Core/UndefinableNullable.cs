namespace UltimateOrb {
    public static class UndefinableNullable {

        public static ref readonly T GetValueRefOrDefaultRef<T>(in UndefinableNullable<T> undefinableNullable) where T : struct {
            return  ref undefinableNullable.value;
        }
    }
}