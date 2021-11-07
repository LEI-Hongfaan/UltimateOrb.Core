using System;
using System.Collections.Generic;

namespace Internal.System.Collections.Generic {
    using UltimateOrb;
    using System = global::System;

    public interface IFixedSizeList<T> : IFixedSizeCollection<T>, System.Collections.Generic.IList<T> {

        void System.Collections.Generic.IList<T>.Insert(int index, T value) {
            ThrowNotSupportedException_FixedSizeCollection();
        }

        void System.Collections.Generic.IList<T>.RemoveAt(int index) {
            ThrowNotSupportedException_FixedSizeCollection();
        }
    }
}
