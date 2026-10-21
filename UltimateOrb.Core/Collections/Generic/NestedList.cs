using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Collections.Generic {

    // Semantic: 
    internal struct NestedList<T> {

        readonly T[]? list;
        readonly Boxed<NestedList<T>>[]? listAlt;
    }

}
