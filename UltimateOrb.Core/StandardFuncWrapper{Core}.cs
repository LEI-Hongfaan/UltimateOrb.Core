
using System;
using UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_Wrapped_Huge;

namespace UltimateOrb {

    public readonly partial struct StandardFuncWrapper<TResult>
        : IReadOnlyStrongBox<Func<TResult>>
       , IFunc<TResult> {

        public readonly Func<TResult> Value;

        Func<TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<TResult>(Func<TResult> value) => new StandardFuncWrapper<TResult>(value);

        public static implicit operator Func<TResult>(StandardFuncWrapper<TResult> value) => value.Value;

        public TResult Invoke() => Value.Invoke();

        public StandardFuncWrapperCatch<TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T, TResult>
        : IReadOnlyStrongBox<Func<T, TResult>>
       , IFunc<T, TResult> {

        public readonly Func<T, TResult> Value;

        Func<T, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T, TResult>(Func<T, TResult> value) => new StandardFuncWrapper<T, TResult>(value);

        public static implicit operator Func<T, TResult>(StandardFuncWrapper<T, TResult> value) => value.Value;

        public TResult Invoke(T arg) => Value.Invoke(arg);

        public StandardFuncWrapperCatch<T, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, TResult>>
       , IFunc<T1, T2, TResult> {

        public readonly Func<T1, T2, TResult> Value;

        Func<T1, T2, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, TResult>(Func<T1, T2, TResult> value) => new StandardFuncWrapper<T1, T2, TResult>(value);

        public static implicit operator Func<T1, T2, TResult>(StandardFuncWrapper<T1, T2, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2) => Value.Invoke(arg1, arg2);

        public StandardFuncWrapperCatch<T1, T2, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, TResult>>
       , IFunc<T1, T2, T3, TResult> {

        public readonly Func<T1, T2, T3, TResult> Value;

        Func<T1, T2, T3, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> value) => new StandardFuncWrapper<T1, T2, T3, TResult>(value);

        public static implicit operator Func<T1, T2, T3, TResult>(StandardFuncWrapper<T1, T2, T3, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3) => Value.Invoke(arg1, arg2, arg3);

        public StandardFuncWrapperCatch<T1, T2, T3, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, TResult>>
       , IFunc<T1, T2, T3, T4, TResult> {

        public readonly Func<T1, T2, T3, T4, TResult> Value;

        Func<T1, T2, T3, T4, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, TResult>(StandardFuncWrapper<T1, T2, T3, T4, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => Value.Invoke(arg1, arg2, arg3, arg4);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, TResult>>
       , IFunc<T1, T2, T3, T4, T5, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, TResult> Value;

        Func<T1, T2, T3, T4, T5, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => Value.Invoke(arg1, arg2, arg3, arg4, arg5);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TException>(this);
    }

    public readonly partial struct StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>
        : IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>
       , IFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> {

        public readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Value;

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> Runtime.CompilerServices.Interfaces.Core.IReadOnlyStrongBox<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>>.Value {

            get => Value;
        }

        public StandardFuncWrapper(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value) => Value = value;

        public static implicit operator StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value) => new StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(value);

        public static implicit operator Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(StandardFuncWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> value) => value.Value;

        public TResult Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) => Value.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);

        public StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TException> Catch<TException>()
            where TException : Exception => new StandardFuncWrapperCatch<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TException>(this);
    }
}
