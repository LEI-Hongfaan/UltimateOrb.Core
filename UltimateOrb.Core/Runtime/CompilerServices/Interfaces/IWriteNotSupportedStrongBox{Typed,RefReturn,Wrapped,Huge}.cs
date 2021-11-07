namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Core.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Core.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Huge.IWriteNotSupportedStrongBox<T>
        , Wrapped.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Core.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Huge.IWriteNotSupportedStrongBox<T>
        , RefReturn.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Wrapped.IWriteNotSupportedStrongBox<T>
        , RefReturn.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IWriteNotSupportedStrongBox<T>
        , RefReturn_Huge.IWriteNotSupportedStrongBox<T>
        , RefReturn_Wrapped.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Core.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Huge.IWriteNotSupportedStrongBox<T>
        , Typed.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Wrapped.IWriteNotSupportedStrongBox<T>
        , Typed.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , Wrapped_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_Wrapped.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , RefReturn.IWriteNotSupportedStrongBox<T>
        , Typed.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , RefReturn_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_RefReturn.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped.IWriteNotSupportedStrongBox<T>
        , Typed_Wrapped.IWriteNotSupportedStrongBox<T>
        , Typed_RefReturn.IWriteNotSupportedStrongBox<T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IWriteNotSupportedStrongBox<T>
        : IStrongBox<T>
        , IWriteNotSupportedValueProvider<TypeTokens.Value, T>
        , RefReturn_Wrapped_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_Wrapped_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_RefReturn_Huge.IWriteNotSupportedStrongBox<T>
        , Typed_RefReturn_Wrapped.IWriteNotSupportedStrongBox<T> {
    }
}
