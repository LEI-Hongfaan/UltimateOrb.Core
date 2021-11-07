
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , System.Collections.Generic.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Core.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Core.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Huge.ICollection<T>
        , Wrapped.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Core.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Huge.ICollection<T>
        , RefReturn.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Wrapped.ICollection<T>
        , RefReturn.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface ICollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        , Wrapped_Huge.ICollection<T>
        , RefReturn_Huge.ICollection<T>
        , RefReturn_Wrapped.ICollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , Core.ICollection<T>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , Huge.ICollection<T>
        , Typed.ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , Wrapped.ICollection<T>
        , Typed.ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , Wrapped_Huge.ICollection<T>
        , Typed_Huge.ICollection<T, TEnumerator>
        , Typed_Wrapped.ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , RefReturn.ICollection<T>
        , Typed.ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , RefReturn_Huge.ICollection<T>
        , Typed_Huge.ICollection<T, TEnumerator>
        , Typed_RefReturn.ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , RefReturn_Wrapped.ICollection<T>
        , Typed_Wrapped.ICollection<T, TEnumerator>
        , Typed_RefReturn.ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface ICollection<T, out TEnumerator>
        : IEnumerable<T, TEnumerator>
        , IReadOnlyCollection<T, TEnumerator>
        , RefReturn_Wrapped_Huge.ICollection<T>
        , Typed_Wrapped_Huge.ICollection<T, TEnumerator>
        , Typed_RefReturn_Huge.ICollection<T, TEnumerator>
        , Typed_RefReturn_Wrapped.ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge.ExtraTypeParametersProvided {

    public partial interface ICollection<T, out TEnumerator, in TEqualityComparer>
        : ICollection<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}
