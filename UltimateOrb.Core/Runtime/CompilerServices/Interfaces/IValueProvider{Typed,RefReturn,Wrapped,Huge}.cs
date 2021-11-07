
namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Core.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Core.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Huge.IValueProvider<ValueToken, T>
        , Wrapped.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Core.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Huge.IValueProvider<ValueToken, T>
        , RefReturn.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Wrapped.IValueProvider<ValueToken, T>
        , RefReturn.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Wrapped_Huge.IValueProvider<ValueToken, T>
        , RefReturn_Huge.IValueProvider<ValueToken, T>
        , RefReturn_Wrapped.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Core.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Huge.IValueProvider<ValueToken, T>
        , Typed.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Wrapped.IValueProvider<ValueToken, T>
        , Typed.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , Wrapped_Huge.IValueProvider<ValueToken, T>
        , Typed_Huge.IValueProvider<ValueToken, T>
        , Typed_Wrapped.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , RefReturn.IValueProvider<ValueToken, T>
        , Typed.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , RefReturn_Huge.IValueProvider<ValueToken, T>
        , Typed_Huge.IValueProvider<ValueToken, T>
        , Typed_RefReturn.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , RefReturn_Wrapped.IValueProvider<ValueToken, T>
        , Typed_Wrapped.IValueProvider<ValueToken, T>
        , Typed_RefReturn.IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IValueProvider<ValueToken, T>
        : IReadOnlyValueProvider<ValueToken, T>
        , RefReturn_Wrapped_Huge.IValueProvider<ValueToken, T>
        , Typed_Wrapped_Huge.IValueProvider<ValueToken, T>
        , Typed_RefReturn_Huge.IValueProvider<ValueToken, T>
        , Typed_RefReturn_Wrapped.IValueProvider<ValueToken, T> {
    }
}
