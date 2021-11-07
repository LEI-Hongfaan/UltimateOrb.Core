using System.Runtime.CompilerServices;
using ThrowHelper = UltimateOrb.Internal.ThrowHelper;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T> {

        abstract T IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T IKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyKeyedValueProvider<ValueToken, TKey, T>)this)[key];

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T> {

        T Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyKeyedValueProvider<ValueToken, TKey, T>)this)[key];
        }

        abstract ref readonly T IReadOnlyKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T Core.IKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyKeyedValueProvider<ValueToken, TKey, T>)this)[key];

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        ref T IKeyedValueProvider<ValueToken, TKey, T>.this[TKey key] {

            get {
                throw ThrowHelper.ThrowNotSupportedException_readonly();
            }
        }
    }
}
