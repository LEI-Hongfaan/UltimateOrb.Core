namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Core {

    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Core.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Core.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Wrapped_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Wrapped.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Core.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , RefReturn.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Wrapped.IWriteNotSupportedValueProvider<ValueToken, T>
        , RefReturn.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn_Wrapped_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Wrapped_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , RefReturn_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , RefReturn_Wrapped.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Core.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Wrapped.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , Wrapped_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_Wrapped.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , RefReturn.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , RefReturn_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_RefReturn.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , RefReturn_Wrapped.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_Wrapped.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_RefReturn.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}

namespace UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge {
    
    public partial interface IWriteNotSupportedValueProvider<ValueToken, T>
        : IValueProvider<ValueToken, T>
        , RefReturn_Wrapped_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_Wrapped_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_RefReturn_Huge.IWriteNotSupportedValueProvider<ValueToken, T>
        , Typed_RefReturn_Wrapped.IWriteNotSupportedValueProvider<ValueToken, T> {
    }
}
