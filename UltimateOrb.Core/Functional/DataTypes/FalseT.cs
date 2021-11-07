namespace UltimateOrb.Functional.DataTypes {

    public readonly struct FalseT : IConstantDataType<bool> {

        public bool Value {

            get => false;
        }
    }
}
