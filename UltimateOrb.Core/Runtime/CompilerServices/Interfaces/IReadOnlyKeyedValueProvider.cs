using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T> {

        T this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T> {

        T Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => this[key];
        }

        new ref readonly T this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
