using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T> {

        T IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => this[key];
        }

        new T this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T> {

        T Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => this[key];
        }

        ref readonly T IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this[key];
        }

        T Core.IKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => this[key];

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set => this[key] = value;
        }

        new ref T this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
