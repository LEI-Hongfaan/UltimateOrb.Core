
namespace UltimateOrb.Collections.Generic.Interfaces.Core {

    public partial interface IReadOnlyCollection<out T>
        : IReadOnlyEnumerable<T>
        , System.Collections.Generic.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Huge {

    public partial interface IReadOnlyCollection<out T>
        : IReadOnlyEnumerable<T>
        , Core.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped {

    public partial interface IReadOnlyCollection<out T>
        : IReadOnlyEnumerable<T>
        , Core.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Wrapped_Huge {

    public partial interface IReadOnlyCollection<out T>
        : IReadOnlyEnumerable<T>
        , Huge.IReadOnlyCollection<T>
        , Wrapped.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn {

    public partial interface IReadOnlyCollection<T>
        : IReadOnlyEnumerable<T>
        , Core.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge {

    public partial interface IReadOnlyCollection<T>
        : IReadOnlyEnumerable<T>
        , Huge.IReadOnlyCollection<T>
        , RefReturn.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped {

    public partial interface IReadOnlyCollection<T>
        : IReadOnlyEnumerable<T>
        , Wrapped.IReadOnlyCollection<T>
        , RefReturn.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyCollection<T>
        : IReadOnlyEnumerable<T>
        , Wrapped_Huge.IReadOnlyCollection<T>
        , RefReturn_Huge.IReadOnlyCollection<T>
        , RefReturn_Wrapped.IReadOnlyCollection<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed {

    public partial interface IReadOnlyCollection<out T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Core.IReadOnlyCollection<T>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge {

    public partial interface IReadOnlyCollection<out T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Huge.IReadOnlyCollection<T>
        , Typed.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped {

    public partial interface IReadOnlyCollection<out T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Wrapped.IReadOnlyCollection<T>
        , Typed.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge {

    public partial interface IReadOnlyCollection<out T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , Wrapped_Huge.IReadOnlyCollection<T>
        , Typed_Huge.IReadOnlyCollection<T, TEnumerator>
        , Typed_Wrapped.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn {

    public partial interface IReadOnlyCollection<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn.IReadOnlyCollection<T>
        , Typed.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge {

    public partial interface IReadOnlyCollection<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Huge.IReadOnlyCollection<T>
        , Typed_Huge.IReadOnlyCollection<T, TEnumerator>
        , Typed_RefReturn.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped {

    public partial interface IReadOnlyCollection<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Wrapped.IReadOnlyCollection<T>
        , Typed_Wrapped.IReadOnlyCollection<T, TEnumerator>
        , Typed_RefReturn.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge {

    public partial interface IReadOnlyCollection<T, out TEnumerator>
        : IReadOnlyEnumerable<T, TEnumerator>
        , RefReturn_Wrapped_Huge.IReadOnlyCollection<T>
        , Typed_Wrapped_Huge.IReadOnlyCollection<T, TEnumerator>
        , Typed_RefReturn_Huge.IReadOnlyCollection<T, TEnumerator>
        , Typed_RefReturn_Wrapped.IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<out T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Huge.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<out T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<out T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_Wrapped_Huge.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<out T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : System.Collections.Generic.IEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Huge.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : System.Collections.Generic.IEqualityComparer<T> {
    }
}

namespace UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge.ExtraTypeParametersProvided {

    public interface IReadOnlyCollection<T, out TEnumerator, in TEqualityComparer>
        : IReadOnlyCollection<T, TEnumerator>
        where TEnumerator : RefReturn.IReadOnlyEnumerator<T>
        where TEqualityComparer : Huge.IEqualityComparer<T> {
    }
}
