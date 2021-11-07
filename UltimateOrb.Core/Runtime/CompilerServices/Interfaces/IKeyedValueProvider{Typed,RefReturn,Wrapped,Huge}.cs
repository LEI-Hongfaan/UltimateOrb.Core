
namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Core.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Core.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Core.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Core.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IKeyedValueProvider<ValueToken, in TKey, T>
        : IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Huge.IKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Wrapped.IKeyedValueProvider<ValueToken, TKey, T> {
    }
}
