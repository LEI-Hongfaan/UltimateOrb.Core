using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Functional {

    public static partial class BooleanFunctorModule {

        public struct AndFunctor<Term1, Term2>
            : IFunc<bool>
            where Term1 : IFunc<bool>
            where Term2 : IFunc<bool> {

            Term1 _Term1;

            Term2 _Term2;

            public AndFunctor(Term1 term1, Term2 term2) {
                _Term1 = term1;
                _Term2 = term2;
            }

            public bool Invoke() {
                return _Term1.Invoke() & _Term2.Invoke();
            }
        }

        public static AndFunctor<TFunc1, TFunc2> And<TFunc1, TFunc2>(TFunc1 func1, TFunc2 func2)
            where TFunc1 : IFunc<bool>
            where TFunc2 : IFunc<bool> {
            return new AndFunctor<TFunc1, TFunc2>(func1, func2);
        }

        public struct AndAlsoFunctor<Term1, Term2>
            : IFunc<bool>
            where Term1 : IFunc<bool>
            where Term2 : IFunc<bool> {

            Term1 _Term1;

            Term2 _Term2;

            public AndAlsoFunctor(Term1 term1, Term2 term2) {
                _Term1 = term1;
                _Term2 = term2;
            }

            public bool Invoke() {
                return _Term1.Invoke() ? _Term2.Invoke() : false;
            }
        }

        public static AndAlsoFunctor<TFunc1, TFunc2> AndAlso<TFunc1, TFunc2>(TFunc1 func1, TFunc2 func2)
            where TFunc1 : IFunc<bool>
            where TFunc2 : IFunc<bool> {
            return new AndAlsoFunctor<TFunc1, TFunc2>(func1, func2);
        }

        public struct OrFunctor<Term1, Term2>
            : IFunc<bool>
            where Term1 : IFunc<bool>
            where Term2 : IFunc<bool> {

            Term1 _Term1;

            Term2 _Term2;

            public OrFunctor(Term1 term1, Term2 term2) {
                _Term1 = term1;
                _Term2 = term2;
            }

            public bool Invoke() {
                return _Term1.Invoke() | _Term2.Invoke();
            }
        }

        public static OrFunctor<TFunc1, TFunc2> Or<TFunc1, TFunc2>(TFunc1 func1, TFunc2 func2)
            where TFunc1 : IFunc<bool>
            where TFunc2 : IFunc<bool> {
            return new OrFunctor<TFunc1, TFunc2>(func1, func2);
        }

        public struct OrElseFunctor<Term1, Term2>
            : IFunc<bool>
            where Term1 : IFunc<bool>
            where Term2 : IFunc<bool> {

            Term1 _Term1;

            Term2 _Term2;

            public OrElseFunctor(Term1 term1, Term2 term2) {
                _Term1 = term1;
                _Term2 = term2;
            }

            public bool Invoke() {
                return _Term1.Invoke() ? true : _Term2.Invoke();
            }
        }

        public static OrElseFunctor<TFunc1, TFunc2> OrElse<TFunc1, TFunc2>(TFunc1 func1, TFunc2 func2)
            where TFunc1 : IFunc<bool>
            where TFunc2 : IFunc<bool> {
            return new OrElseFunctor<TFunc1, TFunc2>(func1, func2);
        }
    }
}
