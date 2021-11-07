namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IEnumerable<T> {

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }

        new IEnumerator<T> GetEnumerator();
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IEnumerable<T> {

        IReadOnlyEnumerator<T> IReadOnlyEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }

        Core.IEnumerator<T> Core.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0401 // Possible allocation of reference type enumerator
            return GetEnumerator();
#pragma warning restore HAA0401 // Possible allocation of reference type enumerator
        }

        new IEnumerator<T> GetEnumerator();
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IEnumerable<T, out TEnumerator> {

        Core.IEnumerator<T> Core.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        new TEnumerator GetEnumerator();
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IEnumerable<T, out TEnumerator> {

        RefReturn.IEnumerator<T> RefReturn.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        RefReturn.IReadOnlyEnumerator<T> RefReturn.IReadOnlyEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        Core.IEnumerator<T> Core.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
#pragma warning disable HAA0601 // Value type to reference type conversion causing boxing allocation
            return GetEnumerator();
#pragma warning restore HAA0601 // Value type to reference type conversion causing boxing allocation
        }

        new TEnumerator GetEnumerator();
    }
}
