using NUnit.Framework;
using UltimateOrb.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace UltimateOrb.Utilities.Tests {

    [TestFixture()]
    public class ThrowHelperTests {

        [Test()]
        [TestCase(double.NaN)]
        [TestCase(double.NegativeInfinity)]
        [TestCase(double.PositiveInfinity)]
        public void ThrowOnNonFiniteTest1(double v) {
            Assert.Throws(new InstanceOfTypeConstraint(typeof(ArithmeticException)), () => {
                ThrowHelper.ThrowOnNonFinite(v);
            });
        }

        [Test()]
        [TestCase(double.NegativeZero)]
        [TestCase(double.Pi)]
        [TestCase(double.Epsilon)]
        [TestCase(double.MinValue)]
        [TestCase(double.MaxValue)]
        public void ThrowOnNonFiniteTest2(double v) {
            ThrowHelper.ThrowOnNonFinite(v);
        }
    }
}
