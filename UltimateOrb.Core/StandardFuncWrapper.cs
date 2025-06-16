using System;
using UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;

namespace UltimateOrb {

    /// <summary>
    /// Provides static factory methods to create <see cref="StandardFuncWrapper{TResult}"/> and its generic variants
    /// from standard <see cref="Func{TResult}"/> delegates.
    /// </summary>
    public readonly partial struct StandardFuncWrapper {

        /// <summary>
        /// Creates a <see cref="StandardFuncWrapper{TResult}"/> from a <see cref="Func{TResult}"/> delegate.
        /// </summary>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="value">The function delegate to wrap.</param>
        /// <returns>A <see cref="StandardFuncWrapper{TResult}"/> wrapping the specified delegate.</returns>
        public static StandardFuncWrapper<TResult> Create<TResult>(Func<TResult> value) => value;

        /// <summary>
        /// Creates a <see cref="StandardFuncWrapper{T, TResult}"/> from a <see cref="Func{T, TResult}"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the function's argument.</typeparam>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="value">The function delegate to wrap.</param>
        /// <returns>A <see cref="StandardFuncWrapper{T, TResult}"/> wrapping the specified delegate.</returns>
        public static StandardFuncWrapper<T, TResult> Create<T, TResult>(Func<T, TResult> value) => value;

        /// <summary>
        /// Creates a <see cref="StandardFuncWrapper{T1, T2, TResult}"/> from a <see cref="Func{T1, T2, TResult}"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="value">The function delegate to wrap.</param>
        /// <returns>A <see cref="StandardFuncWrapper{T1, T2, TResult}"/> wrapping the specified delegate.</returns>
        public static StandardFuncWrapper<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> value) => value;

        /// <summary>
        /// Creates a <see cref="StandardFuncWrapper{T1, T2, T3, TResult}"/> from a <see cref="Func{T1, T2, T3, TResult}"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="TResult">The return type of the function.</typeparam>
        /// <param name="value">The function delegate to wrap.</param>
        /// <returns>A <see cref="StandardFuncWrapper{T1, T2, T3, TResult}"/> wrapping the specified delegate.</returns>
        public static StandardFuncWrapper<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> value) => value;
    }
}
