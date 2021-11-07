using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn;

namespace UltimateOrb.Collections.Generic {
    using Fields = UltimateOrb.Runtime.CompilerServices.TypeTokens;
    using Local = UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;


    struct List<T>
        : Local.IValueProvider<Fields.Array, Array<T>>
        , Local.IValueProvider<Fields.Count, nint> {
        Array<T> Array;
        nint Count;

        Array<T> Runtime.CompilerServices.Interfaces.Core.IValueProvider<Fields.Array, Array<T>>.Value {

            get => Array;

            set => Array = value;
        }

        nint Runtime.CompilerServices.Interfaces.Core.IValueProvider<Fields.Count, nint>.Value {

            get => Count;

            set => Count = value;
        }
    }
}
