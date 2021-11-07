namespace UltimateOrb.Functional.DataTypes {

    public interface IConstantDataType<T> : IDataType<T> {

        T Value {

            get;
        }
    }
}
