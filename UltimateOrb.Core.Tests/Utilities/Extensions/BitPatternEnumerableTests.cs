using NUnit.Framework;
using UltimateOrb.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Utilities.Extensions.Tests {
    [TestFixture()]
    public class BitPatternEnumerableTests {

        [Test()]
        public void GetUInt32BitsWithPopCountLessThanOrEqualTest() {
            Assert.That(BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 32).LongCount(), Is.EqualTo(1L << 32));
        }

        [Test()]
        public void GetUInt32BitsWithPopCountLessThanOrEqualTest_1() {
            Assert.That(BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 31).LongCount(), Is.EqualTo((1L << 32) - 1));
        }

        [Test()]
        public void GetUInt32BitsWithPopCountLessThanOrEqualTest_2() {
            Assert.That(BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 30).LongCount(), Is.EqualTo((1L << 32) - 1 - 32));
        }
    }
}