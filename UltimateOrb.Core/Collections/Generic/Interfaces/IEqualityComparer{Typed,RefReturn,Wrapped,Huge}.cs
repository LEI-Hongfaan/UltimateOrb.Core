
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IEqualityComparer<in T>
        : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IEqualityComparer<in T>
        : Core.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IEqualityComparer<in T>
        : Core.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IEqualityComparer<in T>
        : Huge.IEqualityComparer<T>
        , Wrapped.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IEqualityComparer<in T>
        : Core.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IEqualityComparer<in T>
        : Huge.IEqualityComparer<T>
        , RefReturn.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IEqualityComparer<in T>
        : Wrapped.IEqualityComparer<T>
        , RefReturn.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IEqualityComparer<in T>
        : Wrapped_Huge.IEqualityComparer<T>
        , RefReturn_Huge.IEqualityComparer<T>
        , RefReturn_Wrapped.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IEqualityComparer<in T>
        : Core.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IEqualityComparer<in T>
        : Huge.IEqualityComparer<T>
        , Typed.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IEqualityComparer<in T>
        : Wrapped.IEqualityComparer<T>
        , Typed.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IEqualityComparer<in T>
        : Wrapped_Huge.IEqualityComparer<T>
        , Typed_Huge.IEqualityComparer<T>
        , Typed_Wrapped.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IEqualityComparer<in T>
        : RefReturn.IEqualityComparer<T>
        , Typed.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IEqualityComparer<in T>
        : RefReturn_Huge.IEqualityComparer<T>
        , Typed_Huge.IEqualityComparer<T>
        , Typed_RefReturn.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IEqualityComparer<in T>
        : RefReturn_Wrapped.IEqualityComparer<T>
        , Typed_Wrapped.IEqualityComparer<T>
        , Typed_RefReturn.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IEqualityComparer<in T>
        : RefReturn_Wrapped_Huge.IEqualityComparer<T>
        , Typed_Wrapped_Huge.IEqualityComparer<T>
        , Typed_RefReturn_Huge.IEqualityComparer<T>
        , Typed_RefReturn_Wrapped.IEqualityComparer<T> {
    }
}
