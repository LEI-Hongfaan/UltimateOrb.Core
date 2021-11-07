
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyList<out T>
        : IReadOnlyCollection<T>
        , System.Collections.Generic.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {
    
    public partial interface IReadOnlyList<out T>
        : IReadOnlyCollection<T>
	    , Core.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {
    
    public partial interface IReadOnlyList<out T>
        : IReadOnlyCollection<T>
	    , Core.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {
    
    public partial interface IReadOnlyList<out T>
        : IReadOnlyCollection<T>
	    , Huge.IReadOnlyList<T>
	    , Wrapped.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {
    
    public partial interface IReadOnlyList<T>
        : IReadOnlyCollection<T>
	    , Core.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {
    
    public partial interface IReadOnlyList<T>
        : IReadOnlyCollection<T>
	    , Huge.IReadOnlyList<T>
	    , RefReturn.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {
    
    public partial interface IReadOnlyList<T>
        : IReadOnlyCollection<T>
	    , Wrapped.IReadOnlyList<T>
	    , RefReturn.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {
    
    public partial interface IReadOnlyList<T>
        : IReadOnlyCollection<T>
	    , Wrapped_Huge.IReadOnlyList<T>
	    , RefReturn_Huge.IReadOnlyList<T>
	    , RefReturn_Wrapped.IReadOnlyList<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {
    
    public partial interface IReadOnlyList<out T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , Core.IReadOnlyList<T>
        where TEnumerator: System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {
    
    public partial interface IReadOnlyList<out T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , Huge.IReadOnlyList<T>
	    , Typed.IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {
    
    public partial interface IReadOnlyList<out T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , Wrapped.IReadOnlyList<T>
	    , Typed.IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {
    
    public partial interface IReadOnlyList<out T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , Wrapped_Huge.IReadOnlyList<T>
	    , Typed_Huge.IReadOnlyList<T, TEnumerator>
	    , Typed_Wrapped.IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {
    
    public partial interface IReadOnlyList<T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , RefReturn.IReadOnlyList<T>
	    , Typed.IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {
    
    public partial interface IReadOnlyList<T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , RefReturn_Huge.IReadOnlyList<T>
	    , Typed_Huge.IReadOnlyList<T, TEnumerator>
	    , Typed_RefReturn.IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {
    
    public partial interface IReadOnlyList<T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , RefReturn_Wrapped.IReadOnlyList<T>
	    , Typed_Wrapped.IReadOnlyList<T, TEnumerator>
	    , Typed_RefReturn.IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {
    
    public partial interface IReadOnlyList<T, out TEnumerator>
        : IReadOnlyCollection<T, TEnumerator>
	    , RefReturn_Wrapped_Huge.IReadOnlyList<T>
	    , Typed_Wrapped_Huge.IReadOnlyList<T, TEnumerator>
	    , Typed_RefReturn_Huge.IReadOnlyList<T, TEnumerator>
	    , Typed_RefReturn_Wrapped.IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge.ExtraTypeParametersProvided {
    
    public partial interface IReadOnlyList<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyList<T, TEnumerator>
        where TEnumerator: RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}
