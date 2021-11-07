using System;
using System.Collections.Generic;
using System.Text;

namespace UltimateOrb.Ex0003 {

    public partial interface IKindFuncOrVal {

        IKindFuncOrVal Invoke(IKindFuncOrVal value);

        bool CanInvoke();

        Type GetValue(Type type);
    }

    public partial interface ITypeFuncOrVal {

        ITypeFuncOrVal Invoke(ITypeFuncOrVal value);

        bool CanInvoke();

        Type GetValue(Type type);
    }

    public partial interface IFuncOrVal {

        IFuncOrVal Invoke(IFuncOrVal value);

        bool CanInvoke();

        object GetValue(Type type);
    }

    public partial interface ITypeable {

        ITypeFuncOrVal GetType();
    }

    public partial interface IKindable {

        IKindFuncOrVal GetKind();
    }

    // type constuctor
    public readonly partial struct List : ITypeFuncOrVal {

        private abstract partial class _ {

            internal _() {
            }
        }

        private sealed partial class _Nil : _, IFuncOrVal {

            internal _Nil() {
            }

            public bool CanInvoke() {
                return false;
            }

            public object GetValue(Type type) {
                if (typeof(List) == type) {
                    return this;
                }
                if (typeof(Nil) == type) {
                    return this;
                }
                throw new NotSupportedException();
            }

            public IFuncOrVal Invoke(IFuncOrVal value) {
                throw new NotSupportedException();
            }

            public override string ToString() {
                return @"[]";
            }
        }

        private sealed partial class _Cons : _, IFuncOrVal {

            private IFuncOrVal head;

            private _ tail;

            internal _Cons(IFuncOrVal head, _ tail) {
                this.head = head;
                this.tail = tail;
            }

            public bool CanInvoke() {
                return false;
            }

            public object GetValue(Type type) {
                if (typeof(List) == type) {
                    return this;
                }
                if (typeof(Cons) == type) {
                    return this;
                }
                throw new NotSupportedException();
            }

            public IFuncOrVal Invoke(IFuncOrVal value) {
                throw new NotSupportedException();
            }

            public override string ToString() {
                var sb = new StringBuilder();
                sb.Append('[');
                for (var l = this; ;) {
                    sb.Append(l.head.ToString());
                    l = l.tail as _Cons;
                    if (null != l) {
                        sb.Append(',');
                        continue;
                    }
                    break;
                }
                sb.Append(']');
                return sb.ToString();
            }
        }

        internal static readonly IFuncOrVal Nil = new _Nil();

        internal static IFuncOrVal Cons(IFuncOrVal head, IFuncOrVal tail) {
            return new _Cons(head, (_)tail);
        }

        public ITypeFuncOrVal Invoke(ITypeFuncOrVal value) {
            throw new NotImplementedException();
        }

        public bool CanInvoke() {
            return true;
        }

        public Type GetValue(Type type) {
            throw new NotImplementedException();
        }
    }

    // data constructor
    public readonly partial struct Nil : IFuncOrVal {

        public bool CanInvoke() {
            return false;
        }

        public object GetValue(Type type) {
            return List.Nil;
        }

        public IFuncOrVal Invoke(IFuncOrVal value) {
            throw new NotSupportedException();
        }
    }

    // data constructor
    public readonly partial struct Cons : IFuncOrVal {

        public bool CanInvoke() {
            return true;
        }

        public object GetValue(Type type) {
            return this;
        }

        public IFuncOrVal Invoke(IFuncOrVal value) {
            return new C1(value);
        }

        public readonly partial struct C1 : IFuncOrVal {

            private readonly IFuncOrVal value;

            public C1(IFuncOrVal value) {
                this.value = value;
            }

            public bool CanInvoke() {
                return true;
            }

            public object GetValue(Type type) {
                return this;
            }

            public IFuncOrVal Invoke(IFuncOrVal value) {
                return List.Cons(this.value, value);
            }
        }
    }

    // class
    public readonly partial struct Foldable {
    }

    // function
    public readonly partial struct foldr {
    }

    // function
    public readonly partial struct foldmap {
    }

    // class
    public readonly partial struct Monoid {
    }

    public static partial class a {

        public static readonly IFuncOrVal Integer;


    }



    // type constructor
    public readonly partial struct Integer {

        public bool CanInvoke() {
            return false;
        }

        public object GetValue(Type type) {
            if (typeof(Integer) == type) {
                return typeof(Integer);
            }
            throw new NotSupportedException();
        }

        public IFuncOrVal Invoke(IFuncOrVal value) {
            throw new NotSupportedException();
        }
    }

    // type constructor of higher kind
    public readonly partial struct Product {

    }

    // function
    public readonly partial struct mempty {

        bool CanInvoke() {
            return true;
        }

        object GetValue(Type type) {
            if (typeof(List) == type) {
                return List.Nil;
            }
            if (typeof(Product) == type) {
                return List.Nil;
            }
            throw new NotImplementedException();
        }

        IFuncOrVal Invoke(IFuncOrVal value) {
            throw new NotImplementedException();
        }
    }

    // function
    public readonly partial struct mappend {
    }

    // function
    public readonly partial struct mconcat {
    }
}
