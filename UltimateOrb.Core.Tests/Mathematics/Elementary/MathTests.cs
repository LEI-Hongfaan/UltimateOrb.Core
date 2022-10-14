#nullable enable

using UltimateOrb.Mathematics.Elementary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck.NUnit;

namespace UltimateOrb.Mathematics.Elementary.Tests {
    //[TestClass()]
    public class MathTests {

        struct aaaaa {



            public static aaaaa<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> referenceImpl, Func<T1, T2, TResult> testImpl) {
                return new aaaaa<T1, T2, TResult>(referenceImpl, testImpl);
                }
        }
        struct aaaaa<T1, T2, TResult>  {

            readonly Func<T1, T2, TResult> ReferenceImpl;
            readonly Func<T1, T2, TResult> TestImpl;
            internal  aaaaa(Func<T1, T2, TResult> referenceImpl, Func<T1, T2, TResult> testImpl) {
                if (referenceImpl is null) {
                    throw new ArgumentNullException(nameof(referenceImpl));
                }
                if (testImpl is null) {
                    throw new ArgumentNullException(nameof(testImpl));
                }
                ReferenceImpl = referenceImpl;
                TestImpl = testImpl;
            }

            public bool Test(T1 arg1, T2 arg2) {
                Exception? rEx = null;
                TResult rRv = default!;
                try {
                    rRv = ReferenceImpl.Invoke(arg1, arg2);
                } catch (Exception ex) {
                    rEx = ex;
                }

                Exception? iEx = null;
                TResult iRv = default!;
                try {
                    iRv = TestImpl.Invoke(arg1, arg2);
                } catch (Exception ex) {
                    iEx = ex;
                }

                if (rEx is null) {
                    if (iEx is null) {
                        return EqualityComparer<TResult>.Default.Equals(rRv, iRv);
                    }
                } else if (iEx is not null) {
                    return rEx.GetType().IsAssignableFrom(iEx.GetType());
                }
                return false;
            }


        }

        //[TestMethod]
        [Property(MaxTest = 20000, QuietOnSuccess = true)]
        public bool FloorTest(int value, int divisor) {
            var a = aaaaa.Create(static (int value, int divisor) => {
                var r = Math.Floor((long)value, (long)divisor);
                return checked((int)r);
            }, Math.Floor);
            return a.Test(value, divisor);
        }
    }
}