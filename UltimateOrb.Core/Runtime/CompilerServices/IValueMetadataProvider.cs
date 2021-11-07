using System;

namespace UltimateOrb.Runtime.CompilerServices {

    public partial interface IValueMetadataProvider<ValueToken, out T> {

        Type ValueType {

            get => typeof(T);
        }

        Type ValueId {

            get => typeof(ValueToken);
        }

        string ValueName {

            get => typeof(ValueToken).Name;
        }
    }
}
