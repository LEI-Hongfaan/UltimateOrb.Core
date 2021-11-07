
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , System.Collections.Generic.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Core.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Core.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Huge.IEnumerable<T>
        , Wrapped.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Core.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Huge.IEnumerable<T>
        , RefReturn.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Wrapped.IEnumerable<T>
        , RefReturn.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IEnumerable<T>
        : IReadOnlyEnumerable<T>
        , Wrapped_Huge.IEnumerable<T>
        , RefReturn_Huge.IEnumerable<T>
        , RefReturn_Wrapped.IEnumerable<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Core.IEnumerable<T>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Huge.IEnumerable<T>
        , Typed.IEnumerable<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Wrapped.IEnumerable<T>
        , Typed.IEnumerable<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Wrapped_Huge.IEnumerable<T>
        , Typed_Huge.IEnumerable<T, TEnumerator>
        , Typed_Wrapped.IEnumerable<T, TEnumerator>
        where TEnumerator : Core.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn.IEnumerable<T>
        , Typed.IEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Huge.IEnumerable<T>
        , Typed_Huge.IEnumerable<T, TEnumerator>
        , Typed_RefReturn.IEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Wrapped.IEnumerable<T>
        , Typed_Wrapped.IEnumerable<T, TEnumerator>
        , Typed_RefReturn.IEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IEnumerable<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Wrapped_Huge.IEnumerable<T>
        , Typed_Wrapped_Huge.IEnumerable<T, TEnumerator>
        , Typed_RefReturn_Huge.IEnumerable<T, TEnumerator>
        , Typed_RefReturn_Wrapped.IEnumerable<T, TEnumerator>
        where TEnumerator : RefReturn.IEnumerator<T> {
    }
}
