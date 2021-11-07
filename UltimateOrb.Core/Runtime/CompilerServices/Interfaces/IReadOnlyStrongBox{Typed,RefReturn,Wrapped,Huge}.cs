
namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Core.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Core.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Huge.IReadOnlyStrongBox<T>
        , Wrapped.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Core.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Huge.IReadOnlyStrongBox<T>
        , RefReturn.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Wrapped.IReadOnlyStrongBox<T>
        , RefReturn.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IReadOnlyStrongBox<T>
        , RefReturn_Huge.IReadOnlyStrongBox<T>
        , RefReturn_Wrapped.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Core.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Huge.IReadOnlyStrongBox<T>
        , Typed.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Wrapped.IReadOnlyStrongBox<T>
        , Typed.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyStrongBox<out T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IReadOnlyStrongBox<T>
        , Typed_Huge.IReadOnlyStrongBox<T>
        , Typed_Wrapped.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , RefReturn.IReadOnlyStrongBox<T>
        , Typed.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , RefReturn_Huge.IReadOnlyStrongBox<T>
        , Typed_Huge.IReadOnlyStrongBox<T>
        , Typed_RefReturn.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped.IReadOnlyStrongBox<T>
        , Typed_Wrapped.IReadOnlyStrongBox<T>
        , Typed_RefReturn.IReadOnlyStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyStrongBox<T>
        : IReadOnlyValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped_Huge.IReadOnlyStrongBox<T>
        , Typed_Wrapped_Huge.IReadOnlyStrongBox<T>
        , Typed_RefReturn_Huge.IReadOnlyStrongBox<T>
        , Typed_RefReturn_Wrapped.IReadOnlyStrongBox<T> {
    }
}
