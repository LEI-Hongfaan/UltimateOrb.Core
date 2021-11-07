using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyStrongBox<out T> {

        T IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        new T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyStrongBox<T> {

        T Core.IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        T Core.IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        new ref readonly T Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }
    }
}
