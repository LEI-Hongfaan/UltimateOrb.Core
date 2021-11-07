namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyEnumerable<out T>
        : System.Collections.Generic.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IReadOnlyEnumerable<out T>
        : Core.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IReadOnlyEnumerable<out T>
        : Core.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyEnumerable<out T>
        : Huge.IReadOnlyEnumerable<T>
        , Wrapped.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyEnumerable<T>
        : Core.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyEnumerable<T>
        : Huge.IReadOnlyEnumerable<T>
        , RefReturn.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyEnumerable<T>
        : Wrapped.IReadOnlyEnumerable<T>
        , RefReturn.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyEnumerable<T>
        : Wrapped_Huge.IReadOnlyEnumerable<T>
        , RefReturn_Huge.IReadOnlyEnumerable<T>
        , RefReturn_Wrapped.IReadOnlyEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IReadOnlyEnumerable<out T, out TEnumerator>
        : Core.IReadOnlyEnumerable<T>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IReadOnlyEnumerable<out T, out TEnumerator>
        : Huge.IReadOnlyEnumerable<T>
        , Typed.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyEnumerable<out T, out TEnumerator>
        : Wrapped.IReadOnlyEnumerable<T>
        , Typed.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyEnumerable<out T, out TEnumerator>
        : Wrapped_Huge.IReadOnlyEnumerable<T>
        , Typed_Huge.IReadOnlyEnumerable<T, TEnumerator>
        , Typed_Wrapped.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyEnumerable<T, out TEnumerator>
        : RefReturn.IReadOnlyEnumerable<T>
        , Typed.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyEnumerable<T, out TEnumerator>
        : RefReturn_Huge.IReadOnlyEnumerable<T>
        , Typed_Huge.IReadOnlyEnumerable<T, TEnumerator>
        , Typed_RefReturn.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyEnumerable<T, out TEnumerator>
        : RefReturn_Wrapped.IReadOnlyEnumerable<T>
        , Typed_Wrapped.IReadOnlyEnumerable<T, TEnumerator>
        , Typed_RefReturn.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyEnumerable<T, out TEnumerator>
        : RefReturn_Wrapped_Huge.IReadOnlyEnumerable<T>
        , Typed_Wrapped_Huge.IReadOnlyEnumerable<T, TEnumerator>
        , Typed_RefReturn_Huge.IReadOnlyEnumerable<T, TEnumerator>
        , Typed_RefReturn_Wrapped.IReadOnlyEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}
