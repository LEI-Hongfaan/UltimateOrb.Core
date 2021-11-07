using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IStrongBox<T> {

        object? IStrongBox.Value {

            get => (object?)Value;

#pragma warning disable CS8601 // Possible null reference assignment.
            set => Value = (T)value;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        T IReadOnlyStrongBox<T>.Value {

            get => Value;
        }

        T IValueProvider<TypeTokens.Value, T>.Value {

            get => Value;

            set => Value = value;
        }

        T IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            get => Value;
        }

        new T Value {

            get;

            set;
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IStrongBox<T> {

        ref readonly T IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }

        ref T IValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }

        T Core.IStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set => Value = value;
        }

        ref readonly T IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }

        T Core.IReadOnlyStrongBox<T>.Value {

            get => Value;
        }

        T Core.IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        T Core.IValueProvider<TypeTokens.Value, T>.Value {

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
