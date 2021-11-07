namespace UltimateOrb {
    /// <summary>
    ///     <para>Provides an empty array of the specified type.</para>
    /// </summary>
    /// <typeparam name="T">
    ///     <para>The element type of the array.</para>
    /// </typeparam>
    public static partial class Array_Empty<T> {

        /// <summary>
        ///     <para>
        ///         Represents the empty array.
        ///         This field is read-only.
        ///     </para>
        /// </summary>
        public static readonly T[] Value = new T[0];
    }
}
