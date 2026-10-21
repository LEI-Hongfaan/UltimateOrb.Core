using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.Generic;

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
    // BitWidth in [1, 2^23 − 1]
    [Experimental("UoWIP")]
    public readonly partial struct Int<TStorage, BitWidthT>
        where TStorage : unmanaged
        where BitWidthT : struct, IConstant<BitWidthT, int> {

        readonly TStorage _storage;
    }

    // BitWidth in [1, 2^23 − 1]
    [Experimental("UoWIP")]
    public readonly partial struct UInt<TStorage, BitWidthT>
        where TStorage : unmanaged
        where BitWidthT : struct, IConstant<BitWidthT, int> {

        readonly TStorage _storage;
    }
}

namespace UltimateOrb.Numerics {

    // BitWidth % (8 * sizeof(nuint)) == 0
    [Experimental("UoWIP")]
    internal readonly partial struct IntN<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }

    // BitWidth % (8 * sizeof(nuint)) == 0
    [Experimental("UoWIP")]
    internal readonly partial struct UIntN<TBits> where TBits : unmanaged {

        readonly TBits bits;
    }
}
