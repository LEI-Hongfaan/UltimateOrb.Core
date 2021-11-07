using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Nonstrict {

    public static partial class Maybe {

        public static readonly NothingT Nothing = default;

        public readonly partial struct NothingT {
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Lazy<Maybe<T>> Just<T>(ILazy<T> value) => Maybe<T>.JustT.Value.Invoke(value);
        
        public readonly partial struct NothingT {
        }

        /*
        public static Lazy<Maybe<TResult>> Select<TSource, TResult>(this ILazy<Maybe<TSource>> source, Expression<Func<TSource, TResult>> selector) {
            throw new NotImplementedException();
        }
        */

        public static Lazy<Maybe<TResult>> Select<TSource, TResult>(this ILazy<Maybe<TSource>> source, Func<ILazy<TSource>, ILazy<TResult>> selector) {
            if (null != source) {
                if (source.IsEvaluated) {
                    var source_value = source.Cache;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        return new Maybe<TResult>(selector.Invoke(source_value_Value));
                    }
                    return Maybe<TResult>.Nothing;
                }
                return new Lazy<Maybe<TResult>>(() => {
                    var source_value = source.Value;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        return new Maybe<TResult>(selector.Invoke(source_value_Value));
                    }
                    return default;
                });
            }
            return null;
        }

        /*
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector) {
            if (source.IsJust) {
                var source_Value = source.Value;
                if (source_Value.IsEvaluated) {
                    return new Maybe<TResult>(selector.Invoke(source_Value.Cache).ToLazy());
                }
                return new Maybe<TResult>(new Lazy<TResult>(() => selector.Invoke(source_Value.Value)));
            }
            return default;
        }
        */

        public static Lazy<Maybe<TResult>> Select<TSource, TResult>(this ILazy<Maybe<TSource>> source, Expression<Func<TSource, TResult>> selector) {
            throw new NotImplementedException();
            /*
            if (null != source) {
                var f = ToNonstrict.FromUncurried(selector).Compile();
                return source.Select(f);
            }
            return null;
            */
        }

        public static ILazy<Maybe<TSource>> Where<TSource>(this ILazy<Maybe<TSource>> source, Func<ILazy<TSource>, ILazy<bool>> predicate) {
            if (null != source) {
                if (source.IsEvaluated) {
                    var source_value = source.Cache;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        var s = predicate.Invoke(source_value_Value);
                        if (s.IsEvaluated) {
                            if (s.Cache) {
                                return source;
                            }
                            return Maybe<TSource>.Nothing;
                        }
                        return new Lazy<Maybe<TSource>>(() => s.Value ? new Maybe<TSource>(source_value_Value) : default);
                    }
                    return Maybe<TSource>.Nothing;
                }
                return new Lazy<Maybe<TSource>>(() => {
                    var source_value = source.Value;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        var s = predicate.Invoke(source_value_Value);
                        return s.Value ? new Maybe<TSource>(source_value_Value) : default;
                    }
                    return default;
                });
            }
            return null;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ILazy<Maybe<TSource>> Where<TSource>(this ILazy<Maybe<TSource>> source, Expression<Func<TSource, bool>> predicate) {
            throw new NotImplementedException();
            /*
            if (null != source) {
                var f = ToNonstrict.FromUncurried(predicate).Compile();
                return source.Where(f);
            }
            return null;
            */
        }
        
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ILazy<Maybe<TResult>> SelectMany<TSource, TCollection, TResult>(this ILazy<Maybe<TSource>> source, Expression<Func<TSource, ILazy<Maybe<TCollection>>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector) {
            if (null != source) {
                if (source.IsEvaluated) {
                    var source_value = source.Cache;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        if (source_value_Value.IsEvaluated) {
                            var source_value_Value_value = source_value_Value.Cache;
                            var cS = collectionSelector.Compile();
                            var collection = cS.Invoke(source_value_Value_value);
                            if (collection.IsEvaluated) {
                                var collection_value = collection.Value;
                                if (collection_value.IsJust) {
                                    var collection_value_Value = collection_value.Value;
                                    if (collection_value_Value.IsEvaluated) {
                                        var collection_value_Value_value = collection_value_Value.Cache;
                                        var selector = resultSelector.Compile();
                                        return new Lazy<Maybe<TResult>>(new Maybe<TResult>(new Lazy<TResult>(selector.Invoke(source_value_Value_value, collection_value_Value_value))));
                                    }
                                    //
                                }
                                return Maybe<TResult>.Nothing;
                            }
                        }
                        //
                    }
                    return Maybe<TResult>.Nothing;
                }
                //
            }
            return null;
        }
        
        /*
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Lazy<Maybe<TResult>> Select<TSource, TResult>(this ILazy<Maybe<TSource>> source, Func<TSource, TResult> selector) {
            if (null != source) {
                if (source.IsEvaluated) {
                    var source_value = source.Cache;
                    if (source_value.IsJust) {
                        var a = source_value.Value;
                        if (a.IsEvaluated) {
                            return new Maybe<TResult>(selector.Invoke(a.Cache).ToLazy());
                        }
                        return new Maybe<TResult>(new Lazy<TResult>(() => selector.Invoke(a.Value)));
                    }
                    return Maybe<TResult>.Nothing;
                }
                return new Lazy<Maybe<TResult>>(() => {
                    var source_value = source.Value;
                    if (source_value.IsJust) {
                        var a = source_value.Value;
                        if (a.IsEvaluated) {
                            return new Maybe<TResult>(selector.Invoke(a.Cache).ToLazy());
                        }
                        return new Maybe<TResult>(new Lazy<TResult>(() => selector.Invoke(a.Value)));
                    }
                    return default;
                });
            }
            return null;
        }
        */
        /*
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Maybe<TSource> Where<TSource>(this Maybe<TSource> source, Func<TSource, bool> predicate) {
            if (source.IsJust) {
                return AsSingletonWhere(source.Value, predicate);
            }
            return default;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static Maybe<TSource> AsSingletonWhere<TSource>(ILazy<TSource> source, Func<TSource, bool> predicate) {
            return predicate.Invoke(source.Value) ? new Maybe<TSource>(source) : default;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ILazy<Maybe<TSource>> Where<TSource>(this ILazy<Maybe<TSource>> source, Func<TSource, bool> predicate) {
            if (null != source) {
                if (source.IsEvaluated) {
                    var source_value = source.Cache;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        if (source_value_Value.IsEvaluated) {
                            if (predicate.Invoke(source_value_Value.Cache)) {
                                return source;
                            }
                            return Maybe<TSource>.Nothing;
                        }
                        return new Lazy<Maybe<TSource>>(() => AsSingletonWhere(source_value_Value, predicate));
                    }
                    return Maybe<TSource>.Nothing;
                }
                return new Lazy<Maybe<TSource>>(() => {
                    var source_value = source.Value;
                    if (source_value.IsJust) {
                        var source_value_Value = source_value.Value;
                        return AsSingletonWhere(source_value_Value, predicate);
                    }
                    return default;
                });
            }
            return null;
        }
        */
    }

    public readonly partial struct Maybe<T> {

        private readonly ILazy<T> m_Value;

        public ILazy<T> Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => this.m_Value;
        }

        public static readonly Lazy<Maybe<T>> Nothing = default(Maybe<T>);

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Maybe<T>(Maybe.NothingT value) {
            return default;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Maybe.NothingT(Maybe<T> value) {
            if (value.IsNothing) {
                return default;
            }
            throw new InvalidOperationException();
        }

        public static readonly Lazy<JustT.C0> Just = JustT.Value;

        public bool IsNothing {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => null == this.m_Value;
        }

        public bool IsJust {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => null != this.m_Value;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal Maybe(ILazy<T> value) {
            this.m_Value = value;
        }

        public static partial class JustT {

            public static readonly C0 Value = default;

            public readonly partial struct C0 : IFunc<ILazy<T>, Lazy<Maybe<T>>> {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public Lazy<Maybe<T>> Invoke(ILazy<T> arg) {
                    return new Lazy<Maybe<T>>(new Maybe<T>(arg));
                }
            }
        }

        public override string ToString() {
            if (this.IsJust) {
                return $@"{nameof(Just)} {this.Value.ToString()}";
            }
            return nameof(Nothing);
        }
    }
}
