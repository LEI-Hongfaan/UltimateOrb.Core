using System.Runtime.CompilerServices;

namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IEnumerator<T> {

        T System.Collections.Generic.IEnumerator<T>.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Current;
        }

        new T Current {

            get;

            set;
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IEnumerator<T> {

        T Core.IEnumerator<T>.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Current;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            set => Current = value;
        }

        ref readonly T IReadOnlyEnumerator<T>.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Current;
        }

        T System.Collections.Generic.IEnumerator<T>.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Current;
        }

        new ref T Current {

            get;
        }
    }
}
