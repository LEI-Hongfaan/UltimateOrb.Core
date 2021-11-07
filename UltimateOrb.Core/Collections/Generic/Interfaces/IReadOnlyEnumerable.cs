
using CoreLocal = System.Collections.Generic;

namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyEnumerable<out T> {

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyEnumerable<T> {

        CoreLocal.IEnumerator<T> CoreLocal.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }

        new IReadOnlyEnumerator<T> GetEnumerator();
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IReadOnlyEnumerable<out T, out TEnumerator> {

        CoreLocal.IEnumerator<T> CoreLocal.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return new Core.EnumeratorUpgraded<T, TEnumerator>(GetEnumerator());
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        new TEnumerator GetEnumerator();
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyEnumerable<T, out TEnumerator> {

        RefReturn.IReadOnlyEnumerator<T> RefReturn.IReadOnlyEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        CoreLocal.IEnumerator<T> CoreLocal.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        new TEnumerator GetEnumerator();
    }
}
