using System;
using UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;

namespace UltimateOrb {

    public readonly partial struct StandardFuncWrapper {

        public static StandardFuncWrapper<TResult> Create<TResult>(Func<TResult> value) => value;
    
        public static StandardFuncWrapper<T, TResult> Create<T, TResult>(Func<T, TResult> value) => value;

        public static StandardFuncWrapper<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> value) => value;
   
        public static StandardFuncWrapper<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> value) => value;
    }
}
