using System.Runtime.CompilerServices;
using ThrowHelper = UltimateOrb.Internal.ThrowHelper;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedStrongBox<T> {

        T IValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        T IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        object? IStrongBox.Value {

            get => (object?)Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        abstract T IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T IStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyStrongBox<T>)this).Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IWriteNotSupportedStrongBox<T> {

        T Core.IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Value;
        }

        ref readonly T IReadOnlyValueProvider<TypeTokens.Value, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }

        T Core.IValueProvider<TypeTokens.Value, T>.Value {

            get => Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        ref T IValueProvider<TypeTokens.Value, T>.Value {

            get => ref Value;
        }

        T Core.IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyStrongBox<T>)this).Value;
        }

        abstract ref readonly T IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T Core.IStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyStrongBox<T>)this).Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        ref T IStrongBox<T>.Value {

            get {
                throw ThrowHelper.ThrowNotSupportedException_readonly();
            }
        }
    }
}
