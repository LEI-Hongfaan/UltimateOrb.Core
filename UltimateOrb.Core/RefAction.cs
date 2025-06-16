using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    /// <summary>
    /// Represents a method that performs an action on a value passed by reference.
    /// </summary>
    /// <typeparam name="T">The type of the value to be passed by reference.</typeparam>
    /// <param name="byRef">The value to be passed by reference.</param>
    public delegate void RefAction<T>(ref T byRef);

    /// <summary>
    /// Represents a method that performs an action on a value passed by reference, with an additional argument.
    /// </summary>
    /// <typeparam name="T">The type of the value to be passed by reference.</typeparam>
    /// <typeparam name="TArg">The type of the additional argument.</typeparam>
    /// <param name="byRef">The value to be passed by reference.</param>
    /// <param name="arg">The additional argument.</param>
    public delegate void RefAction<T, TArg>(ref T byRef, TArg arg);
}
