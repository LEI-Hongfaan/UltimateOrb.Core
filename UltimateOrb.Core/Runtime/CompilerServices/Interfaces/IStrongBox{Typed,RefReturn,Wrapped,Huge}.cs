namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , System.Runtime.CompilerServices.IStrongBox {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Core.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Core.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Huge.IStrongBox<T>
        , Wrapped.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Core.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Huge.IStrongBox<T>
        , RefReturn.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Wrapped.IStrongBox<T>
        , RefReturn.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IStrongBox<T>
        , RefReturn_Huge.IStrongBox<T>
        , RefReturn_Wrapped.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Core.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Huge.IStrongBox<T>
        , Typed.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Wrapped.IStrongBox<T>
        , Typed.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IStrongBox<T>
        , Typed_Huge.IStrongBox<T>
        , Typed_Wrapped.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , RefReturn.IStrongBox<T>
        , Typed.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , RefReturn_Huge.IStrongBox<T>
        , Typed_Huge.IStrongBox<T>
        , Typed_RefReturn.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped.IStrongBox<T>
        , Typed_Wrapped.IStrongBox<T>
        , Typed_RefReturn.IStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IStrongBox<T>
        : IReadOnlyStrongBox<T>
        , IValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped_Huge.IStrongBox<T>
        , Typed_Wrapped_Huge.IStrongBox<T>
        , Typed_RefReturn_Huge.IStrongBox<T>
        , Typed_RefReturn_Wrapped.IStrongBox<T> {
    }
}
