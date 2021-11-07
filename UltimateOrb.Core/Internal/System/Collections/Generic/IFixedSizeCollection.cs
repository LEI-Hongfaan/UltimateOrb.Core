using System;
using System.Collections.Generic;

namespace Internal.System.Collections.Generic {
    using UltimateOrb;
    using System = global::System;

    public interface IFixedSizeCollection<T> : System.Collections.Generic.ICollection<T> {

        protected internal static NotSupportedException ThrowNotSupportedException_FixedSizeCollection() {
            ((IList<byte>)Array_Empty<byte>.Value).Add(default); // throw
            throw null!;
        }

        void System.Collections.Generic.ICollection<T>.Add(T value) {
            throw ThrowNotSupportedException_FixedSizeCollection();
        }

        void System.Collections.Generic.ICollection<T>.Clear() {
            throw ThrowNotSupportedException_FixedSizeCollection();
        }

        bool System.Collections.Generic.ICollection<T>.Remove(T value) {
            throw ThrowNotSupportedException_FixedSizeCollection();
        }
    }
}
