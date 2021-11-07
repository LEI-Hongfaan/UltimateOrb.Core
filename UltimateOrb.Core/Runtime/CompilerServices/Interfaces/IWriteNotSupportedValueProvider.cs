using System.Runtime.CompilerServices;
using ThrowHelper = UltimateOrb.Internal.ThrowHelper;

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedValueProvider<ValueToken, T> {

        abstract T IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T IValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyValueProvider<ValueToken, T>)this).Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IWriteNotSupportedValueProvider<ValueToken, T> {

        T Core.IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyValueProvider<ValueToken, T>)this).Value;
        }

        abstract ref readonly T IReadOnlyValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T Core.IValueProvider<ValueToken, T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ((IReadOnlyValueProvider<ValueToken, T>)this).Value;

            set => ThrowHelper.ThrowNotSupportedException_readonly();
        }

        ref T IValueProvider<ValueToken, T>.Value {

            get {
                throw ThrowHelper.ThrowNotSupportedException_readonly();
            }
        }
    }
}
