
using System;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using NUnit.Framework;
using UltimateOrb.Numerics;

namespace UltimateOrb.Numerics.Tests {

    [TestFixture]
    public class CircularIntervalTests {

        private static CircularInterval CI(double a, double b) => new CircularInterval(Vector128.Create(a, b));

        [Test]
        public void Multiply_RegularIntervals_ReturnsExpected() {
            // (-2, 3) * (-5, 7) -> (-15, 21)
            var a = CI(-2, 3);
            var b = CI(-5, 7);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(-15, 21)));
        }

        [Test]
        public void Multiply_CircularIntervals_ReturnsExpected() {
            // (3, -10) * (7, 11) -> (21, -70)
            var a = CI(3, -10);
            var b = CI(7, 11);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(21, -70)));
        }

        [Test]
        public void Multiply_IntervalWithInfinity_ReturnsExpected() {
            // (0, inf) * (1, 5) -> (0, inf)
            var a = CI(0, double.PositiveInfinity);
            var b = CI(1, 5);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(0, double.PositiveInfinity)));
        }

        [Test]
        public void Multiply_CircularWithZero_ReturnsFullInterval() {
            // (3, -10) * (-1, 2) -> (-inf, inf)
            var a = CI(3, -10);
            var b = CI(-1, 2);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(double.NegativeInfinity, double.PositiveInfinity)));
        }

        [Test]
        public void Multiply_BothRegularIntervalsWithZero_ReturnsExpected() {
            // (-1, 2) * (3, 4) -> (-4, 8)
            var a = CI(-1, 2);
            var b = CI(3, 4);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(-4, 8)));
        }

        [Test]
        public void Multiply_BothCircularIntervals_ReturnsCircular() {
            // (5, -2) * (7, -3) -> (35, -21)
            var a = CI(5, -2);
            var b = CI(7, -3);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(35, -21)));
        }

        [Test]
        public void Multiply_ZeroIntervals_ReturnsZero() {
            // (0, 0) * (0, 0) -> (0, 0)
            var a = CI(0, 0);
            var b = CI(0, 0);
            var result = a * b;
            Assert.That(result, Is.EqualTo(CI(0, 0)));
        }
    }
}
