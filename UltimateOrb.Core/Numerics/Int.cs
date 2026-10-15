using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics.DataTypes;

namespace UltimateOrb.Runtime.CompilerServices.Generic {

    public interface IConstant<TSelf, TValue>
#if NET9_0_OR_GREATER
        where TSelf : allows ref struct
        where TValue : allows ref struct
#endif
        {

        public static abstract TValue Value { get; }
    }

    internal interface IBooleanConstant<TSelf> :
        IConstant<TSelf, bool>
#if NET9_0_OR_GREATER
        where TSelf : allows ref struct
#endif
        {
    }

    public readonly struct TrueT : IBooleanConstant<TrueT> {

        public static bool Value => true;
    }

    public readonly struct FalseT : IBooleanConstant<TrueT> {

        public static bool Value => false;
    }
}

namespace UltimateOrb.Numerics {

    // BitWidth % (8 * sizeof(nuint)) == 0
    [Experimental("UoWIP")]
    internal readonly partial struct IntB<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }

    // BitWidth % (8 * sizeof(nuint)) == 0
    [Experimental("UoWIP")]
    internal readonly partial struct UIntB<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }

    // BitWidth % 8 == 0
    [Experimental("UoWIP")]
    public readonly partial struct Int<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }

    // BitWidth % 8 == 0
    [Experimental("UoWIP")]
    public readonly partial struct UInt<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }
}
