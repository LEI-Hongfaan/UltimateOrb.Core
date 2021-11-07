using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    internal struct EnumeratorUpgraded<T, TBase>
        : Typed_Wrapped_Huge.IReadOnlyEnumerator<T>
        where TBase : System.Collections.Generic.IEnumerator<T> {

        public TBase _Base;

        public EnumeratorUpgraded(TBase @base) {
            _Base = @base;
        }

        public T Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => _Base.Current;
        }

        object System.Collections.IEnumerator.Current {

            get => ((System.Collections.IEnumerator)_Base).Current;
        }

        public void Dispose() {
            _Base.Dispose();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() {
            return _Base.MoveNext();
        }

        public void Reset() {
            _Base.Reset();
        }
    }

    public partial interface IReadOnlyEnumerator<out T> {

        object? System.Collections.IEnumerator.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => (object?)Current;
        }

        void System.Collections.IEnumerator.Reset() {
            throw new NotSupportedException();
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyEnumerator<T> {

        T System.Collections.Generic.IEnumerator<T>.Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => Current;
        }

        new ref readonly T Current {

            get;
        }
    }
}
