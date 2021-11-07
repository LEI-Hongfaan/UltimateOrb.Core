
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyEnumerator<out T>
        : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IReadOnlyEnumerator<out T>
        : Core.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IReadOnlyEnumerator<out T>
        : Core.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyEnumerator<out T>
        : Huge.IReadOnlyEnumerator<T>
        , Wrapped.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyEnumerator<T>
        : Core.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyEnumerator<T>
        : Huge.IReadOnlyEnumerator<T>
        , RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyEnumerator<T>
        : Wrapped.IReadOnlyEnumerator<T>
        , RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyEnumerator<T>
        : Wrapped_Huge.IReadOnlyEnumerator<T>
        , RefReturn_Huge.IReadOnlyEnumerator<T>
        , RefReturn_Wrapped.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IReadOnlyEnumerator<out T>
        : Core.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IReadOnlyEnumerator<out T>
        : Huge.IReadOnlyEnumerator<T>
        , Typed.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyEnumerator<out T>
        : Wrapped.IReadOnlyEnumerator<T>
        , Typed.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyEnumerator<out T>
        : Wrapped_Huge.IReadOnlyEnumerator<T>
        , Typed_Huge.IReadOnlyEnumerator<T>
        , Typed_Wrapped.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyEnumerator<T>
        : RefReturn.IReadOnlyEnumerator<T>
        , Typed.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyEnumerator<T>
        : RefReturn_Huge.IReadOnlyEnumerator<T>
        , Typed_Huge.IReadOnlyEnumerator<T>
        , Typed_RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyEnumerator<T>
        : RefReturn_Wrapped.IReadOnlyEnumerator<T>
        , Typed_Wrapped.IReadOnlyEnumerator<T>
        , Typed_RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyEnumerator<T>
        : RefReturn_Wrapped_Huge.IReadOnlyEnumerator<T>
        , Typed_Wrapped_Huge.IReadOnlyEnumerator<T>
        , Typed_RefReturn_Huge.IReadOnlyEnumerator<T>
        , Typed_RefReturn_Wrapped.IReadOnlyEnumerator<T> {
    }
}
