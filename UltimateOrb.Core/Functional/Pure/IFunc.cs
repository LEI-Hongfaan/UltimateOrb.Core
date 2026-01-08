using System;
using System.Collections.Generic;
using IO = UltimateOrb;

namespace UltimateOrb.Functional.Pure {

    public partial interface IFunc<out TResult> : IO.IFunc<TResult> {

        // TResult Invoke();
    }

    public partial interface IFunc<in T, out TResult> : IO.IFunc<T, TResult> {

        // TResult Invoke(TBase arg);
    }

    public partial interface IFunc<in T1, in T2, out TResult> : IO.IFunc<T1, T2, TResult> {

        // TResult Invoke(T1 arg1, T2 arg2);
    }

    public partial interface IPredicate<in T> : IFunc<T, bool> {

        // bool Invoke(TBase obj);
    }

    public partial struct StandardFuncAsFunc<TResult> : IFunc<TResult> {

        private readonly Func<TResult> value;

        public StandardFuncAsFunc(Func<TResult> value) {
            this.value = value;
        }

        public TResult Invoke() {
            return this.value();
        }

        /*
        TResult IFunc<TResult>.Invoke() {
            return this.Invoke();
        }
        */
    }

    public partial struct StandardFuncAsFunc<T, TResult> : IFunc<T, TResult> {

        private readonly Func<T, TResult> value;

        public StandardFuncAsFunc(Func<T, TResult> value) {
            this.value = value;
        }

        public TResult Invoke(T arg) {
            return this.value(arg);
        }

        /*
        TResult IFunc<TBase, TResult>.Invoke(TBase arg) {
            return this.Invoke(arg);
        }
        */
    }

    public partial struct StandardFuncAsFunc<T1, T2, TResult> : IFunc<T1, T2, TResult> {

        private readonly Func<T1, T2, TResult> value;

        public StandardFuncAsFunc(Func<T1, T2, TResult> value) {
            this.value = value;
        }

        public TResult Invoke(T1 arg1, T2 arg2) {
            return this.value(arg1, arg2);
        }

        /*
        TResult IFunc<T1, T2, TResult>.Invoke(T1 arg1, T2 arg2) {
            return this.Invoke(arg1, arg2);
        }
        */
    }

    public partial struct StandardPredicateAsPredicate<T> : IPredicate<T> {

        private readonly Predicate<T> value;

        public StandardPredicateAsPredicate(Predicate<T> value) {
            this.value = value;
        }

        public bool Invoke(T obj) {
            return this.value(obj);
        }

        /*
        bool IFunc<TBase, bool>.Invoke(TBase obj) {
            return this.Invoke(obj);
        }
        */
    }

    public static partial class DelegateConverter {

        public static StandardFuncAsFunc<TResult> AsFunc<TResult>(this Func<TResult> value) {
            return new StandardFuncAsFunc<TResult>(value);
        }

        public static Func<TResult> AsStandardFunc<TResult, TFunc>(this TFunc value) where TFunc : IFunc<TResult> {
            return value.Invoke;
        }

        public static StandardFuncAsFunc<T, TResult> AsFunc<T, TResult>(this Func<T, TResult> value) {
            return new StandardFuncAsFunc<T, TResult>(value);
        }

        public static Func<T, TResult> AsStandardFunc<T, TResult, TFunc>(this TFunc value) where TFunc : IFunc<T, TResult> {
            return value.Invoke;
        }

        public static StandardFuncAsFunc<T1, T2, TResult> AsFunc<T1, T2, TResult>(this Func<T1, T2, TResult> value) {
            return new StandardFuncAsFunc<T1, T2, TResult>(value);
        }

        public static Func<T1, T2, TResult> AsStandardFunc<T1, T2, TResult, TFunc>(this TFunc value) where TFunc : IFunc<T1, T2, TResult> {
            return value.Invoke;
        }

        public static StandardPredicateAsPredicate<T> AsPredicate<T>(this Predicate<T> value) {
            return new StandardPredicateAsPredicate<T>(value);
        }

        public static Predicate<T> AsStandardPredicate<T, TPredicate>(this TPredicate value) where TPredicate : IPredicate<T> {
            return value.Invoke;
        }
    }
}
