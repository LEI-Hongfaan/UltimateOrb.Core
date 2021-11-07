
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Huge.IEnumerator<T>
        , Wrapped.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Huge.IEnumerator<T>
        , RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Wrapped.IEnumerator<T>
        , RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Wrapped_Huge.IEnumerator<T>
        , RefReturn_Huge.IEnumerator<T>
        , RefReturn_Wrapped.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Huge.IEnumerator<T>
        , Typed.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Wrapped.IEnumerator<T>
        , Typed.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , Wrapped_Huge.IEnumerator<T>
        , Typed_Huge.IEnumerator<T>
        , Typed_Wrapped.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , RefReturn.IEnumerator<T>
        , Typed.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , RefReturn_Huge.IEnumerator<T>
        , Typed_Huge.IEnumerator<T>
        , Typed_RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , RefReturn_Wrapped.IEnumerator<T>
        , Typed_Wrapped.IEnumerator<T>
        , Typed_RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IEnumerator<T>
        : IReadOnlyEnumerator<T>
        , RefReturn_Wrapped_Huge.IEnumerator<T>
        , Typed_Wrapped_Huge.IEnumerator<T>
        , Typed_RefReturn_Huge.IEnumerator<T>
        , Typed_RefReturn_Wrapped.IEnumerator<T> {
    }
}
