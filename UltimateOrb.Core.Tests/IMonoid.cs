using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UltimateOrb.AAAb {
    using IObject = IReadOnlyStrongBox;

    public partial interface IReadOnlyStrongBox {

        object Value {

            get;
        }

        T GetValue<T>();
    }

    public partial interface ITagFunction {
    }

    public partial struct id : ITagFunction {

        public static Func<IObject, IObject> ForAll(Type a) {
            throw new NotImplementedException();
        }
    }

    public partial interface IType {
    }

    public partial interface IConstraint {
    }

    public partial struct TBool : ITagTypeConstructor {
    }

    public partial interface ITagKind {
    }

    public partial struct Kind : ITagKind {
    }

    public partial interface ITagConstraint {
    }

    public partial struct Constraint : ITagConstraint {
    }

    public partial interface ITagTypeClass {
    }



    public partial struct TEq : ITagTypeClass {

        public static IConstraint Invoke(IType type) {
            throw new NotImplementedException();
        }


    }



    public partial struct AddInt32 : IRank0Type {

        public TDelegete GetValue<TDelegete>() {
            if (typeof(Func<Int32, Func<Int32, Int32>>) == typeof(TDelegete)) {
                return (TDelegete)(object)(Func<Int32, Func<Int32, Int32>>)(x => y => checked(x + y));
            }
            throw new NotImplementedException();
        }
    }

    public partial struct AddInt32R1 {

        public IRank0Type GetValue<T>() {
            if (typeof(Int32) == typeof(T)) {
                return default(AddInt32);
            }
            throw new NotImplementedException();
        }
    }

    public partial interface IRank0Type {

        TValue GetValue<TValue>();
    }

    public partial interface IRank1Type {

        TConstructor GetValue<T, TConstructor>();
    }

    public partial interface IForAll<in T, out TResult> {

        TResult Typed {

            get;
        }
    }



    public partial struct dsfdf : INum<int, int> {

        public int Typed => 0;
    }

    internal partial interface INum<in T, out TResult> : IForAll<T, TResult> {

    }

    public static partial class ForAll1<T1> {

        public readonly static Func<T1, T1> Id = x => x;

        public static partial class ForAll2<T2> {

            public readonly static Func<T1, Func<T2, T1>> Const = x => y => x;
        }
    }

    public static partial class ForAll1_Num<T1> {
        
        public readonly static Func<T1, T1> Add;

        internal readonly static Func<T1, T1> Add__;

        public static partial class U2<T2> {

            public static Func<T1, Func<T2, T1>> Const = x => y => x;
        }
    }

    public static partial class ForAll {

        public static partial class Type<T1> {

            public static partial class Id {

                public readonly static Func<T1, T1> Value = x => x;
            }

            public static partial class ForAll {

                public static partial class Type<T2> {

                    public static partial class Const {

                        public readonly static Func<T1, Func<T2, T1>> Value = x => y => x;
                    }
                }
            }
        }
    }

    public static partial class ForAll {

        public static partial class Type<T1> {

            public static partial class Id {
            }

            public static partial class ForAll {

                public static partial class Type<T2> {

                    public static partial class Const {
                    }
                }
            }
        }
    }

    public static partial class Num {

        public static partial class Type<T1> {

            public readonly static Func<T1, T1> Add;


        }
    }

    internal static partial class dsfadsfdasfdfasd<T1, T2, T3> {
        


        static Func<T1, Func<T2, T1>> fsdasd;

        static IForAll<T1, IForAll<T2, Func<T1, Func<T2, T1>>>> const_1;

        static IForAll<T1, Func<T1, IForAll<T2, Func<T2, T1>>>> saf22d;

        static Func<IForAll<T1, Func<T1,T1>>, IForAll<T2, Func<T2, T2>>> saf2ddd2d;

        static IForAll<T2, Func<IForAll<T1, Func<T1, T1>>, Func<T2, T2>>> saf2ddd2sd;

        static void sdfa() {
            
        }
    }


    internal static partial class dsfadsfdasf {


        

        internal static readonly BindingFlags[] bindingFlags = new[] { BindingFlags.Instance, BindingFlags.Static, };
    }

    public partial struct eq : ITagFunction {

        private static partial class D0<T> {

            internal static Func<T, Func<T, bool>> m_candidate;

            internal static bool m_implemented;

            internal static bool m_initialized;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Register<T>(Func<T, Func<T, bool>> func) {
            if (null == func) {
                throw new ArgumentNullException(nameof(func));
            }
            if (D0<T>.m_initialized) {
                throw new InvalidOperationException();
            }
            D0<T>.m_candidate = func;
            Register_Stub1<T>();
            D0<T>.m_candidate = null;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static void Register_Stub1<T>() {
            GC.KeepAlive(R0<T>.Value);
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static Func<T, Func<T, bool>> Register_Stub0<T>() {
            return R0<T>.Value;
        }

        public static partial class R0<T> {

            private static readonly Func<T, Func<T, bool>> m_value = GetValue();

            public static Func<T, Func<T, bool>> Value {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => m_value;
            }
            
                private static Func<T, Func<T, bool>> GetValue() {
                var func = (Func<T, Func<T, bool>>)null;
                for (; ; ) {
                    {
                        func = D0<T>.m_candidate;
                        if (null != func) {
                            break;
                        }
                    }
                    {
                        var mns = new[] { nameof(eq), @"Equals", @"op_Equals", };
                        func = NewMethod<T, T, bool>(mns);
                        if (null != func) {
                            break;
                        }
                    }
                    {

                    }
                    break;
                }
                if (null == func) {
                    throw new NotImplementedException();
                }
                L_0:
                D0<T>.m_initialized = true;
                return func;
            }

            private static Func<T1, Func<T2, TResult>> NewMethod<T1, T2, TResult>(string[] mns) {
                var func = (Func<T1, Func<T2, TResult>>)null;
                var expr = (Expression<Func<T1, Func<T2, TResult>>>)null;
                var type0 = typeof(T1);
                var type1 = typeof(T2);
                var ps1 = new[] { type1 };
                var ps2 = new[] { type0, type1 };
                var pms = new[] { Expression.Parameter(type0), Expression.Parameter(type1) };
                var rtype = typeof(TResult);
                foreach (var mn in mns) {
                    foreach (var item in dsfadsfdasf.bindingFlags) {
                        var method = (MethodInfo)null;
                        var bf = BindingFlags.Public | item;
                        var ps = (Type[])null;
                        if (BindingFlags.Instance == item) {
                            ps = ps1;
                        } else if (BindingFlags.Static == item) {
                            ps = ps2;
                        }
                        method = type1.GetMethod(mn, bf, null, ps, null);
                        if (null == method) {
                            continue;
                        }
                        if (method.IsStatic) {
                            expr = Expression.Lambda<Func<T1, Func<T2, TResult>>>(Expression.Lambda(Expression.Call(null, method, pms[0], pms[1]), pms[1]), pms[0]);
                        } else {
                            if (null == default(T)) {
                                // nullable type (Nullable`1 or class)
                                // TODO
                                expr = Expression.Lambda<Func<T1, Func<T2, TResult>>>(Expression.Lambda(Expression.Call(pms[0], method, pms[1]), pms[1]), pms[0]);
                            } else {
                                expr = Expression.Lambda<Func<T1, Func<T2, TResult>>>(Expression.Lambda(Expression.Call(pms[0], method, pms[1]), pms[1]), pms[0]);
                            }
                        }
                        if (null != expr) {
                            try {
                                func = expr.Compile();
                            } catch (Exception) {
                            }
                            if (null != func) {
                                break;
                            }
                        }
                    }
                }

                return func;
            }
        }

        public static T ForAll<TTypeTag, T>() {
            throw new NotImplementedException();
        }
    }


    public partial struct neq : ITagFunction {

        private static partial class D0<T> {

            internal static Func<T, Func<T, bool>> m_candidate;

            internal static bool m_initialized;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void Register<T>(Func<T, Func<T, bool>> func) {
            if (null == func) {
                throw new ArgumentNullException(nameof(func));
            }
            if (D0<T>.m_initialized) {
                throw new InvalidOperationException();
            }
            D0<T>.m_candidate = func;
            Register_Stub1<T>();
            D0<T>.m_candidate = null;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static void Register_Stub1<T>() {
            GC.KeepAlive(R0<T>.Value);
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static Func<T, Func<T, bool>> Register_Stub0<T>() {
            return R0<T>.Value;
        }

        public static partial class R0<T> {

            private static readonly Func<T, Func<T, bool>> m_value = GetValue();

            public static Func<T, Func<T, bool>> Value {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => m_value;
            }

            private static Func<T, Func<T, bool>> GetValue() {
                var func = (Func<T, Func<T, bool>>)null;
                for (; ; ) {
                    {
                        var t = D0<T>.m_candidate;
                        if (null != t) {
                            func = t;
                            break;
                        }
                    }
                    {
                        var mns = new[] { nameof(eq), @"Equals", @"op_Equals", };
                        var expr = (Expression<Func<T, Func<T, bool>>>)null;
                        var type = typeof(T);
                        var ps1 = new[] { type };
                        var ps2 = new[] { type, type };
                        var pms = new[] { Expression.Parameter(type), Expression.Parameter(type) };
                        var rtype = typeof(bool);
                        foreach (var mn in mns) {
                            foreach (var item in dsfadsfdasf.bindingFlags) {
                                var method = (MethodInfo)null;
                                var bf = BindingFlags.Public | item;
                                var ps = (Type[])null;
                                if (BindingFlags.Instance == item) {
                                    ps = ps1;
                                } else if (BindingFlags.Static == item) {
                                    ps = ps2;
                                }
                                method = type.GetMethod(mn, bf, null, ps, null);
                                if (null == method) {
                                    continue;
                                }
                                if (method.IsStatic) {
                                    expr = Expression.Lambda<Func<T, Func<T, bool>>>(Expression.Lambda(Expression.Call(null, method, pms[0], pms[1]), pms[1]), pms[0]);
                                } else {
                                    if (null == default(T)) {
                                        // nullable type (Nullable`1 or class)
                                        // TODO
                                        expr = Expression.Lambda<Func<T, Func<T, bool>>>(Expression.Lambda(Expression.Call(pms[0], method, pms[1]), pms[1]), pms[0]);
                                    } else {
                                        expr = Expression.Lambda<Func<T, Func<T, bool>>>(Expression.Lambda(Expression.Call(pms[0], method, pms[1]), pms[1]), pms[0]);
                                    }
                                }
                                if (null != expr) {
                                    try {
                                        func = expr.Compile();
                                    } catch (Exception) {
                                    }
                                    if (null != func) {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    {
                        // TODO
                    }
                    break;
                }
                if (null == func) {
                    throw new NotImplementedException();
                }
                L_0:
                D0<T>.m_initialized = true;
                return func;
            }
        }

        public static T ForAll<TTypeTag, T>() {
            throw new NotImplementedException();
        }
    }

    public partial interface ITagConstructor {
    }

    public partial interface ITagTypeConstructor {
    }

    public partial struct True : ITagConstructor {
    }

    public partial struct False : ITagConstructor {
    }


}

namespace UltimateOrb.Nongeneric {
    using static Bool;
    using IObject = IReadOnlyStrongBox;

    public partial interface IReadOnlyStrongBox {

        object Value {

            get;
        }

        T GetValue<T>();
    }

    public enum OperatorAssociativity {
        None,
        Left,
        Right,
        Associative,
    }

    [AttributeUsageAttribute(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    public sealed partial class OperatorNotationAttribute : Attribute {

        public string Symbol {

            get => this.symbol;
        }

        public double Precedence {

            get => this.precedence;
        }

        public OperatorAssociativity Associativity {

            get => this.associativity;
        }

        public OperatorNotationAttribute(string symbol, double precedence, OperatorAssociativity associativity = OperatorAssociativity.None) {
            this.symbol = symbol;
            this.precedence = precedence;
            this.associativity = associativity;
        }

        private readonly string symbol;

        private readonly double precedence;

        private readonly OperatorAssociativity associativity;
    }

    public partial class ObjectBase : IObject {

        public object Value => this;

        T IObject.GetValue<T>() {
            return (T)(object)this;
        }
    }

    public partial class Bool : ObjectBase, IObject {

        private Bool() {
        }

        private static readonly Bool s_False = new Bool();

        private static readonly Bool s_True = new Bool();

        public static Bool False {

            get => s_False;
        }

        public static Bool True {

            get => s_True;
        }

        public T GetValue<T>() {
            if (typeof(bool) == typeof(T)) {
                return (T)(object)(this == s_True);
            }
            return (T)(object)this;
        }

        public static readonly Func<Bool, Bool> not = x => s_True == x ? s_True : s_False;
    }

    public partial struct TOrdering : IEq {

        public static readonly Func<Ordering, Func<Ordering, Bool>> equals = x => y => (Bool)(x.GetValue<int>() == y.GetValue<int>() ? True : False);

        public static readonly Func<Ordering, Func<Ordering, Bool>> not_equals = x => y => (Bool)TEq.not_equals(x)(y);

        Func<IObject, Func<IObject, Bool>> IEq.equals => (Func<IObject, Func<IObject, Bool>>)equals;

        Func<IObject, Func<IObject, Bool>> IEq.not_equals => (Func<IObject, Func<IObject, Bool>>)not_equals;
    }

    public partial class Ordering : ObjectBase, IObject {

        private readonly int value;

        private Ordering(int value) {
            this.value = value;
        }

        private static readonly Ordering s_LT = new Ordering(-1);

        private static readonly Ordering s_EQ = new Ordering(0);

        private static readonly Ordering s_GT = new Ordering(1);

        public static Ordering LT {

            get => s_LT;
        }

        public static Ordering EQ {

            get => s_EQ;
        }

        public static Ordering GT {

            get => s_GT;
        }

        public T GetValue<T>() {
            if (typeof(int) == typeof(T)) {
                return (T)(object)this.value;
            }
            return (T)(object)this;
        }
    }

    public partial interface IEq {

        [OperatorNotationAttribute("==", 4)]
        Func<IObject, Func<IObject, Bool>> equals {

            get;
        }

        [OperatorNotationAttribute("/=", 4)]
        Func<IObject, Func<IObject, Bool>> not_equals {

            get;
        }
    }

    public partial struct TEq  {


        public static Func<IObject, Func<IObject, Bool>> equals {

            get {

                return x => y => not(not_equals(x)(y));
            }
        }

        public static Func<IObject, Func<IObject, Bool>> not_equals {

            get {

                return x => y => not(equals(x)(y));
            }
        }
    }

    public partial interface IOrd {

        [OperatorNotationAttribute("==", 4)]
        Func<IObject, Func<IObject, Ordering>> compare {

            get;
        }

        [OperatorNotationAttribute("<", 4)]
        Func<IObject, Func<IObject, Bool>> lt {

            get;
        }

        [OperatorNotationAttribute("<=", 4)]
        Func<IObject, Func<IObject, Bool>> le {

            get;
        }

        [OperatorNotationAttribute(">", 4)]
        Func<IObject, Func<IObject, Bool>> gt {

            get;
        }

        [OperatorNotationAttribute(">=", 4)]
        Func<IObject, Func<IObject, Bool>> ge {

            get;
        }

        Func<IObject, Func<IObject, IObject>> max {

            get;
        }

        Func<IObject, Func<IObject, IObject>> min {

            get;
        }
    }

    public static partial class TOrd {

        public static Func<IObject, Func<IObject, Ordering>> compare {

            get {
                throw null;
            }
        }

        public static Func<IObject, Func<IObject, Bool>> le {

            get {
                throw null;
            }
        }

        public static Func<IOrd, Func<IOrd, Bool>> lt {

            get {
                throw new NotImplementedException();
                // return x => y => compare(x)(y) == True;
            }
        }

        public static Func<IOrd, Func<IOrd, Bool>> gt {

            get {
                throw new NotImplementedException();
                // x => y => not(le(x)(y));
            }
        }

        public static Func<IOrd, Func<IOrd, Bool>> ge {

            get;
        }

        public static Func<IOrd, Func<IOrd, IOrd>> max {

            get;
        }

        public static Func<IOrd, Func<IOrd, IOrd>> min {

            get;
        }
    }

    public partial interface IFunctor {

        Func<Func<IObject, IObject>, Func<IObject, IObject>> fmap {

            get;
        }

        [OperatorNotationAttribute("<$", 4, OperatorAssociativity.Left)]
        Func<Func<IObject, IObject>, Func<IObject, IObject>> fmap_compose_const {

            get;
        }
    }

    public partial interface IMonoid {

        object mempty {

            get;
        }

        Func<Bool, Func<Bool, Bool>> mappend {

            get;
        }

        Func<object[], Bool> mconcat {

            get;
        }
    }

}
