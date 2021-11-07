namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Core.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Core.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Core.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Core.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IWriteNotSupportedKeyedValueProvider<ValueToken, in TKey, T>
        : IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Huge.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Wrapped.IWriteNotSupportedKeyedValueProvider<ValueToken, TKey, T> {
    }
}
