using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;

namespace UltimateOrb.Linq.Try {

    public readonly struct FuncPtr<T, TResult>
        : IReadOnlyStrongBox<IntPtr>
        , IFunc<T, TResult> {

        [CLSCompliant(false)]
        public readonly unsafe delegate*<T, TResult> Value;

        public readonly IntPtr Pointer {

            get {
                unsafe {
                    return unchecked((nint)(nuint)Value);
                }
            }
        }

        IntPtr Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<IntPtr>.Value {

            get => Pointer;
        }

        public TResult Invoke(T arg) {
            unsafe {
                return Value(arg);
            }
        }
    }

    public static class TryExtenstions {

        public static StandardFuncWrapper<TResult> Wrap<TResult>(this Func<TResult> value) => value;
    
        public static StandardFuncWrapper<T, TResult> Wrap<T, TResult>(this Func<T, TResult> value) => value;
    
        public static StandardFuncWrapper<T1, T2, TResult> Wrap<T1, T2, TResult>(this Func<T1, T2, TResult> value) => value;
    }

    public static class ExceptionalExtenstions {

        public static Exceptional<TResult, TException> Select<TSource, TResult, TException>(this Exceptional<TSource, TException> source, Func<TSource, TResult> selector)
            where TException : Exception => source.Select(selector.Wrap());

        public static Exceptional<TResult, TException> Select<TSource, TResult, TException>(this Exceptional<TSource, TException> source, StandardFuncWrapper<TSource, TResult> selector)
            where TException : Exception => source.Select(selector.Catch<TException>());

        public static Exceptional<TResult, TException> Select<TSource, TResult, TException>(this Exceptional<TSource, TException> source, StandardFuncWrapperCatch<TSource, TResult, TException> selector)
            where TException : Exception => source.HasValue ? selector.Invoke(source.ValueOrDefault!) : source.Exception;

        public static Exceptional<TResult, TException> SelectMany<TSource, TCollection, TResult, TException>(this Exceptional<TSource, TException> source, Func<TSource, TCollection> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
            where TException : Exception {
            return source.SelectMany(collectionSelector.Wrap(), resultSelector.Wrap());
        }

        public static Exceptional<TResult, TException> SelectMany<TSource, TCollection, TResult, TException>(this Exceptional<TSource, TException> source, StandardFuncWrapper<TSource, TCollection> collectionSelector, StandardFuncWrapper<TSource, TCollection, TResult> resultSelector)
            where TException : Exception {
            return source.SelectMany(collectionSelector.Catch<TException>(), resultSelector.Catch<TException>());
        }

        public static Exceptional<TResult, TException> SelectMany<TSource, TCollection, TResult, TException>(this Exceptional<TSource, TException> source, StandardFuncWrapperCatch<TSource, TCollection, TException> collectionSelector, StandardFuncWrapperCatch<TSource, TCollection, TResult, TException> resultSelector)
            where TException : Exception {
            if (source.HasException) {
                return source.ExceptionOrDefault!;
            }
            var t = source.Value; // may throw
            var collection = collectionSelector.Invoke(t);
            if (collection.HasException) {
                return collection.ExceptionOrDefault!;
            }
            var d = collection.Value; // may throw
            return resultSelector.Invoke(t, d);
        }
    }
}
