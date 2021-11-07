using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyValueProvider<ValueToken, out T> {

        T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyValueProvider<ValueToken, T> {

        T Core.IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        new ref readonly T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
