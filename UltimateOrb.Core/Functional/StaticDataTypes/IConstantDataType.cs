namespace UltimateOrb.Functional.StaticDataTypes {

    public interface IConstantDataType<T, Tag> {

        public static virtual T Value {

            get;
        }
    }
}
