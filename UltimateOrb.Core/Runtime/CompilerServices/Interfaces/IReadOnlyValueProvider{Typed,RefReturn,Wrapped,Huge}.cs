
namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyValueProvider<ValueToken, out T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Core.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Core.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Huge.IReadOnlyValueProvider<ValueToken, T>
        , Wrapped.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : Core.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : Huge.IReadOnlyValueProvider<ValueToken, T>
        , RefReturn.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : Wrapped.IReadOnlyValueProvider<ValueToken, T>
        , RefReturn.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : Wrapped_Huge.IReadOnlyValueProvider<ValueToken, T>
        , RefReturn_Huge.IReadOnlyValueProvider<ValueToken, T>
        , RefReturn_Wrapped.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Core.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Wrapped.IReadOnlyValueProvider<ValueToken, T>
        , Typed.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, out T>
        : Wrapped_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_Wrapped.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : RefReturn.IReadOnlyValueProvider<ValueToken, T>
        , Typed.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : RefReturn_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_RefReturn.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : RefReturn_Wrapped.IReadOnlyValueProvider<ValueToken, T>
        , Typed_Wrapped.IReadOnlyValueProvider<ValueToken, T>
        , Typed_RefReturn.IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyValueProvider<ValueToken, T>
        : RefReturn_Wrapped_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_Wrapped_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_RefReturn_Huge.IReadOnlyValueProvider<ValueToken, T>
        , Typed_RefReturn_Wrapped.IReadOnlyValueProvider<ValueToken, T> {
    }
}
