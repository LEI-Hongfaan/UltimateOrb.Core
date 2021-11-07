namespace UltimateOrb.Functional.DataTypes {

    public readonly struct TrueT : IConstantDataType<bool> {

        public bool Value {

            get => true;
        }
    }
}
