using System;

namespace UltimateOrb {

    /// <summary>
    /// Represents a placeholder type used to express a null pointer value in APIs that require an argument type.
    /// </summary>
    /// <remarks>
    /// This type is not instantiable and exists only as a sentinel for APIs that need a typed null placeholder.
    /// </remarks>
    public class NullPtrPlaceholderType {

        /// <summary>
        /// A typed null. Use this field when you pass null/nullptr.
        /// </summary>
        public const NullPtrPlaceholderType? nullptr = null;

        /// <summary>
        /// A typed null. Use this field when you pass null/nullptr.
        /// </summary>
        public const NullPtrPlaceholderType? @null = null;

        private NullPtrPlaceholderType() {
            throw new InvalidOperationException("NullPtrPlaceholderType cannot be instantiated");
        }
    }
}
