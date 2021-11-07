
namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : IKeyedValueMetadataProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : Wrapped_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , RefReturn_Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Core.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, out T>
        : Wrapped_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : RefReturn.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : RefReturn_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : RefReturn_Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyKeyedValueProvider<ValueToken, in TKey, T>
        : RefReturn_Wrapped_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_Wrapped_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Huge.IReadOnlyKeyedValueProvider<ValueToken, TKey, T>
        , Typed_RefReturn_Wrapped.IReadOnlyKeyedValueProvider<ValueToken, TKey, T> {
    }
}
