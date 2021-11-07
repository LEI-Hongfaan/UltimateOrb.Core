using System;

namespace UltimateOrb.Runtime.CompilerServices {

    public partial interface IKeyedValueMetadataProvider<ValueToken, in TKey, out T>
        : IValueMetadataProvider<ValueToken, T> {

        Type KeyType {

            get => typeof(TKey);
        }
    }
}
