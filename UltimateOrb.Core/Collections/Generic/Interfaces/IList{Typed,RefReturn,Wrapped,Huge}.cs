namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , System.Collections.Generic.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Core.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Core.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Huge.IList<T>
        , Wrapped.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Core.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Huge.IList<T>
        , RefReturn.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Wrapped.IList<T>
        , RefReturn.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IList<T>
        : ICollection<T>
        , IReadOnlyList<T>
        , Wrapped_Huge.IList<T>
        , RefReturn_Huge.IList<T>
        , RefReturn_Wrapped.IList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , Core.IList<T>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , Huge.IList<T>
        , Typed.IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , Wrapped.IList<T>
        , Typed.IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , Wrapped_Huge.IList<T>
        , Typed_Huge.IList<T, TEnumerator>
        , Typed_Wrapped.IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , RefReturn.IList<T>
        , Typed.IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , RefReturn_Huge.IList<T>
        , Typed_Huge.IList<T, TEnumerator>
        , Typed_RefReturn.IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , RefReturn_Wrapped.IList<T>
        , Typed_Wrapped.IList<T, TEnumerator>
        , Typed_RefReturn.IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IList<T, out TEnumerator>
        : ICollection<T, TEnumerator>
        , IReadOnlyList<T, TEnumerator>
        , RefReturn_Wrapped_Huge.IList<T>
        , Typed_Wrapped_Huge.IList<T, TEnumerator>
        , Typed_RefReturn_Huge.IList<T, TEnumerator>
        , Typed_RefReturn_Wrapped.IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge.ExtraTypeParametersProvided {

    public partial interface IList<T, out TEnumerator, in TEqualityComparer>
        : IList<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}
