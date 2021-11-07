using System;
using System.Runtime.InteropServices;

namespace UltimateOrb {

    /// <summary>
    ///     <para>
    ///         Provides a placeholder type.
    ///     </para>
    /// </summary>
    [ComVisibleAttribute(true)]
    [SerializableAttribute()]
    public readonly partial struct Void {
    }

    /// <summary>
    ///     <para>
    ///         Provides a type that is different from the specified type.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     <para>
    ///         The specified type.
    ///     </para>
    /// </typeparam>
    [ComVisibleAttribute(true)]
    [SerializableAttribute()]
    public readonly partial struct Void<T> {
    }

    /// <summary>
    ///     <para>
    ///         Provides a type that is different from the specified types.
    ///     </para>
    /// </summary>
    /// <typeparam name="T1">
    ///     <para>
    ///         The first specified type.
    ///     </para>
    /// </typeparam>
    /// <typeparam name="T2">
    ///     <para>
    ///         The second specified type.
    ///     </para>
    /// </typeparam>
    [ComVisibleAttribute(true)]
    [SerializableAttribute()]
    public readonly partial struct Void<T1, T2> {
    }
}
