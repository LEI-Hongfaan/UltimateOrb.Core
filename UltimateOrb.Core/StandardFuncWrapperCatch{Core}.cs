
using System;

namespace UltimateOrb {

    public readonly partial struct StandardFuncWrapperCatch<TResult, TException>
        : IFunc<Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke() {
            try {
                return funcWrapper.Invoke();
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<Exceptional<TResult, TException>>(StandardFuncWrapperCatch<TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke() => funcWrapper.Invoke();

        public static explicit operator Func<TResult>(StandardFuncWrapperCatch<TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T, TResult, TException>
        : IFunc<T, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T arg) {
            try {
                return funcWrapper.Invoke(arg);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T arg) => funcWrapper.Invoke(arg);

        public static explicit operator Func<T, TResult>(StandardFuncWrapperCatch<T, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, TResult, TException>
        : IFunc<T1, T2, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2) {
            try {
                return funcWrapper.Invoke(arg1, arg2);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2) => funcWrapper.Invoke(arg1, arg2);

        public static explicit operator Func<T1, T2, TResult>(StandardFuncWrapperCatch<T1, T2, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, TResult, TException>
        : IFunc<T1, T2, T3, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3) => funcWrapper.Invoke(arg1, arg2, arg3);

        public static explicit operator Func<T1, T2, T3, TResult>(StandardFuncWrapperCatch<T1, T2, T3, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, TResult, TException>
        : IFunc<T1, T2, T3, T4, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => funcWrapper.Invoke(arg1, arg2, arg3, arg4);

        public static explicit operator Func<T1, T2, T3, T4, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5);

        public static explicit operator Func<T1, T2, T3, T4, T5, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TException> value) => value.UncatchedInvoke;
    }

    public readonly partial struct StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TException>
        : IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Exceptional<TResult, TException>>
        where TException : Exception {

        readonly StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> funcWrapper;

        public StandardFuncWrapperCatch(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> funcWrapper) => this.funcWrapper = funcWrapper;

        public Exceptional<TResult, TException> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            try {
                return funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            } catch (TException ex) {
                return ex;
            }
        }

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Exceptional<TResult, TException>>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TException> value) => value.Invoke;

        public TResult UncatchedInvoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => funcWrapper.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);

        public static explicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TException> value) => value.UncatchedInvoke;
    }
}
