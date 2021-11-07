using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Linq;



namespace UltimateOrb.Mathematics.Geometry.Extensions {

    public static class TupleVectorExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (TResult, TResult) Select<TSource, TResult>(this (TSource, TSource) source, Func<TSource, TResult> selector) {
            return (selector.Invoke(source.Item1), selector.Invoke(source.Item2));
        } 
    }
}


namespace UltimateOrb.Mathematics.Geometry {
    using static Extensions.TupleVectorExtensions;

    internal class ParametricCurveReparametrizationImpl<TSource> : IParametricCurve<TSource> {
        private IParametricCurve<TSource> source;
        private Func<double, double> parameterSelector;
        private (double LowerBound, double UpperBound) domain;

        public ParametricCurveReparametrizationImpl(IParametricCurve<TSource> source, Func<double, double> parameterSelector, Func<double, double> parameterInverseSelector) {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.parameterSelector = parameterSelector ?? throw new ArgumentNullException(nameof(parameterSelector));
            this.domain = from x in source.Domain select parameterInverseSelector(x);
        }

        public ParametricCurveReparametrizationImpl(IParametricCurve<TSource> source, Func<double, double> parameterSelector, (double LowerBound, double UpperBound) domain) {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.parameterSelector = parameterSelector ?? throw new ArgumentNullException(nameof(parameterSelector));
            this.domain = domain;
        }

        public (double LowerBound, double UpperBound) Domain {

            get {
                return domain;
            }
        }



        public TSource Invoke(double arg) {
            return source.Invoke(parameterSelector.Invoke(arg));
        }
    }
    public static partial class ParametricCurveExtensions {

        public static IParametricCurve<TSource> Reparametrization<TSource>(this IParametricCurve<TSource> source, Func<double, double> parameterSelector, Func<double, double> parameterInverseSelector) {
            return new ParametricCurveReparametrizationImpl<TSource>(source, parameterSelector, parameterInverseSelector);
        }

        public static IParametricCurve<TSource> Reparametrization<TSource>(this IParametricCurve<TSource> source, Func<double, double> parameterSelector, (double LowerBound, double UpperBound) domain) {
            return new ParametricCurveReparametrizationImpl<TSource>(source, parameterSelector, domain);
        }
    }

}

namespace UltimateOrb.Mathematics.Geometry {

    public class ParametricCurveToParematricCurveImpl<T> : IParametricCurve<T> {

        readonly Func<double, T> func;

        readonly (double LowerBound, double UpperBound) domain;

        public ParametricCurveToParematricCurveImpl(Func<double, T> func, (double LowerBound, double UpperBound) domain) {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
            this.domain = domain;
        }

        public (double LowerBound, double UpperBound) Domain {

            get => domain;
        }

        public T Invoke(double arg) {
            return func.Invoke(arg);
        }
    }


    public static partial class ParametricCurveExtensions {

        public static IParametricCurve<T> ToParametricCurve<T>(this Func<double, T> func, (double LowerBound, double UpperBound) domain) {

            return new ParametricCurveToParematricCurveImpl<T>(func, domain);
        }
    }


}



namespace UltimateOrb.Mathematics.Geometry {

    public interface IParametricCurve {

        (double LowerBound, double UpperBound) Domain {

            get;
        }
    }

    public interface IParametricCurve<T> : IParametricCurve, IFunc<double, T> {

    }


    public static partial class ParametricCurveExtensions {


        public static (double LowerBound, double UpperBound) IntersectWith(this (double LowerBound, double UpperBound) first, (double LowerBound, double UpperBound) second) {
            var upper_bound = Math.Min(first.UpperBound, second.UpperBound);
            var lower_bound = Math.Max(first.LowerBound, second.LowerBound);
            if (lower_bound <= upper_bound) {
                return (lower_bound, upper_bound);
            } else {
                return (double.NaN, double.NaN);
            }
        }

    }
}


namespace UltimateOrb.Mathematics.Geometry {

    public class ParametricCurveSelectImpl<TSource, TResult> : IParametricCurve<TResult> {

        readonly IParametricCurve<TSource> source;
        readonly Func<TSource, TResult> selector;

        public ParametricCurveSelectImpl(IParametricCurve<TSource> source, Func<TSource, TResult> selector) {
            this.source = source ?? throw new ArgumentNullException(nameof(source));
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public (double LowerBound, double UpperBound) Domain {

            get => source.Domain;
        }

        public TResult Invoke(double arg) {
            return selector.Invoke(source.Invoke(arg));
        }
    }


    public static partial class ParametricCurveExtensions {

        public static IParametricCurve<TResult> Select<TSource, TResult>(IParametricCurve<TSource> source, Func<TSource, TResult> selector) {
            return new ParametricCurveSelectImpl<TSource, TResult>(source, selector);
        }
    }
}

namespace UltimateOrb.Mathematics.Geometry {

    public class ParametricCurveSelectManyImpl<TSource, TCollection, TResult> : IParametricCurve<TResult> {
        readonly IParametricCurve<TSource> source;
        readonly Func<TSource, IParametricCurve<TCollection>> collectionSelector;
        readonly Func<TSource, TCollection, TResult> resultSelector;

        public ParametricCurveSelectManyImpl(IParametricCurve<TSource> source, Func<TSource, IParametricCurve<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public (double LowerBound, double UpperBound) Domain {

            get {

                return (double.NaN, double.NaN);
            }
        }

        public TResult Invoke(double arg) {
            var a = source.Invoke(arg);
            var collection = collectionSelector.Invoke(a);
            return resultSelector.Invoke(a, collection.Invoke(arg));
        }
    }


    public static partial class ParametricCurveExtensions {






        public static IParametricCurve<TResult> SelectMany<TSource, TCollection, TResult>(this IParametricCurve<TSource> source, Func<TSource, IParametricCurve<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) {
            return new ParametricCurveSelectManyImpl<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

    }
}


namespace UltimateOrb.Mathematics.Geometry {

    public class ParametricCurveSelectManyImpl2<TSource, TCollection, TResult> : IParametricCurve<TResult> {
        readonly IParametricCurve<TSource> source;
        readonly Func<TSource, IParametricCurve<TCollection>> collectionSelector;
        readonly Func<TSource, TCollection, TResult> resultSelector;

        public ParametricCurveSelectManyImpl2(IParametricCurve<TSource> source, Func<TSource, IParametricCurve<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector) {
            this.source = source;
            this.collectionSelector = collectionSelector;
            this.resultSelector = resultSelector;
        }

        public (double LowerBound, double UpperBound) Domain {

            get {
                var domain = source.Domain;
                var collection = collectionSelector.Invoke(source.Invoke(domain.LowerBound));
                return domain.IntersectWith(collection.Domain);
            }
        }

        public TResult Invoke(double arg) {
            var a = source.Invoke(arg);
            var collection = collectionSelector.Invoke(a);
            return resultSelector.Invoke(a, collection.Invoke(arg));
        }
    }


    public static partial class ParametricCurveExtensions {




    }
}