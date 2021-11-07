using System.Runtime.CompilerServices;

namespace UltimateOrb {

    public static partial class ToNonstrict<T1, T2, TResult, TFunc>
        where TFunc : IO.IFunc<T1, T2, TResult> {

        public static readonly C0 Value = default;

        public static readonly Lazy<C0> Value_ = Value;

        public readonly partial struct C0 : IFunc<TFunc, C0.C1> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public C1 Invoke(TFunc arg) {
                return new C1(arg);
            }

            public readonly partial struct C1 : IFunc<ILazy<T1>, ILazy<T2>, Lazy<TResult>> {

                private readonly TFunc arg;

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public C1(TFunc arg) {
                    this.arg = arg;
                }

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public Lazy<TResult> Invoke(ILazy<T1> arg1, ILazy<T2> arg2) {
                    var f = this.arg;
                    var v1 = arg1;
                    var v2 = arg2;
                    if (v1.IsEvaluated) {
                        var u1 = v1.Cache;
                        if (v2.IsEvaluated) {
                            var u2 = v2.Cache;
                            return new Lazy<TResult>(f.Invoke(u1, u2));
                        } else {
                            return new Lazy<TResult>(() => f.Invoke(u1, v2.Value));
                        }
                    } else {
                        if (v2.IsEvaluated) {
                            var u2 = v2.Cache;
                            return new Lazy<TResult>(() => f.Invoke(v1.Value, u2));
                        } else {
                            return new Lazy<TResult>(() => f.Invoke(v1.Value, v2.Value));
                        }
                    }
                }
            }
        }
    }

    public static partial class ToNonstrict<T1, T2, TResult, TFunc, TFuncB1>
            where TFunc : IO.IFunc<T1, TFuncB1>
            where TFuncB1 : IO.IFunc<T2, TResult> {

        public static readonly C0 Value = default;

        public static readonly Lazy<C0> Value_ = Value;

        public readonly partial struct C0 : IFunc<TFunc, C0.C1> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public C1 Invoke(TFunc arg) {
                return new C1(arg);
            }

            public readonly partial struct C1 : IFunc<ILazy<T1>, C1.C2> {

                private readonly TFunc arg;

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public C1(TFunc arg) {
                    this.arg = arg;
                }

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public C2 Invoke(ILazy<T1> arg) {
                    return new C2(this, arg);
                }

                public readonly partial struct C2 : IFunc<ILazy<T2>, Lazy<TResult>> {

                    private readonly C1 c1;

                    private readonly ILazy<T1> arg;

                    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                    public C2(C1 c1, ILazy<T1> arg) {
                        this.c1 = c1;
                        this.arg = arg;
                    }

                    [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                    public Lazy<TResult> Invoke(ILazy<T2> arg) {
                        var f = this.c1.arg;
                        var v1 = this.arg;
                        var v2 = arg;
                        if (v1.IsEvaluated) {
                            return ToNonstrict<T2, TResult, TFuncB1>.Value.Invoke(f.Invoke(v1.Cache)).Invoke(v2);
                        }
                        if (v2.IsEvaluated) {
                            var u2 = v2.Cache;
                            return new Lazy<TResult>(() => f.Invoke(v1.Value).Invoke(u2));
                        }
                        return new Lazy<TResult>(() => f.Invoke(v1.Value).Invoke(v2.Value));
                    }
                }
            }
        }
    }

    public static partial class ToNonstrict<T, TResult, TFunc>
            where TFunc : IO.IFunc<T, TResult> {

        public static readonly C0 Value = default;

        public static readonly Lazy<C0> Value_ = Value;

        public readonly partial struct C0 : IFunc<TFunc, C0.C1> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public C1 Invoke(TFunc arg) {
                return new C1(arg);
            }

            public readonly partial struct C1 : IFunc<ILazy<T>, Lazy<TResult>> {

                private readonly TFunc arg;

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public C1(TFunc arg) {
                    this.arg = arg;
                }

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                public Lazy<TResult> Invoke(ILazy<T> arg) {
                    var f = this.arg;
                    if (arg.IsEvaluated) {
                        var value = arg.Cache;
                        return new Lazy<TResult>(f.Invoke(value));
                    }
                    return new Lazy<TResult>(() => f.Invoke(arg.Value));
                }
            }
        }
    }
}
