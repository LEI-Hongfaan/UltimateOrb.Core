namespace UltimateOrb {

    /// <summary>
    ///     <para>Provides the CLI default value of the specified type.</para>
    /// </summary>
    /// <typeparam name="T">
    ///     <para>Specifies the type of the CLI default value.</para>
    /// </typeparam>
    public static partial class Default<T> {

        /// <summary>
        ///     <para>Represents the CLI default value of type <typeparamref name="T"/>.</para>
        /// </summary>
        public static readonly T Value;
    }
}
