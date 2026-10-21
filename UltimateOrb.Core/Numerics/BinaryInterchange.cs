using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP")]
    public readonly partial struct BinaryInterchange<TStorage> where TStorage : unmanaged {

        readonly TStorage _storage;
    }

    [Experimental("UoWIP")]
    public readonly partial struct DecimalInterchange<TStorage> where TStorage : unmanaged {

        readonly TStorage _storage;
    }
}

namespace UltimateOrb {

    [Experimental("UoWIP")]
    public readonly partial struct Octuple {

        readonly Vector256<UInt32> _storage;
    }
}