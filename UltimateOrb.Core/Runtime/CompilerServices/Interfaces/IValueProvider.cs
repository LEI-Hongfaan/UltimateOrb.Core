using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IValueProvider<ValueToken, T> {

        T IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        new T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IValueProvider<ValueToken, T> {

        T Core.IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        ref readonly T IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }

        T Core.IValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set => Value = value;
        }

        new ref T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
