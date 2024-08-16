#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using System;
using UltimateOrb.Mathematics.Exact;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;

using NUnit.Framework;
using UltimateOrb.Numerics;
using System.Numerics;
using UltimateOrb.Utilities;

namespace UltimateOrb.Core.Tests {

    [TestFixture]
    public class Rational64ExactTests {
#if false
        [Test]
        public void FromFraction_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            UInt32 numerator = 0;
            Int32 denominator = 0;

            // Act
            var result = Rational64.FromFraction(
                numerator,
                denominator);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Inverse_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.Inverse(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Negate_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.Negate(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Multiply_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 first = default(global::UltimateOrb.Mathematics.Exact.Rational64);
            Rational64 second = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.Multiply(
                first,
                second);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void MultiplyAsRational128_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 first = default(global::UltimateOrb.Mathematics.Exact.Rational64);
            Rational64 second = default(global::UltimateOrb.Mathematics.Exact.Rational64);
            UInt64 bits_hi = 0;

            // Act
            var result = Rational64.MultiplyAsRational128(
                first,
                second,
                out bits_hi);

            // Assert
            Assert.Fail();
        }
#endif

        [Test]
        public void AddAsRational128_StateUnderTest_ExpectedBehavior() {
            // Arrange
            Rational64 first = 1;
            Rational64 second = -10000001;
            UInt64 bits_hi = 1919810;

            // Act
            var result = Rational64.AddAsRational128(
                first,
                second,
                out bits_hi);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result, Is.EqualTo(10000000));
                Assert.That(bits_hi, Is.EqualTo(unchecked((UInt64)(-1L))));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void AddAsRational128_1(UInt32 a, Int32 b, UInt32 c, Int32 d) {
            if (b == 0 || d == 0) {
                return;
            }
            // Arrange
            Rational64 first = Rational64.FromFraction(a, b);
            Rational64 second = Rational64.FromFraction(c, d);
            BigRational first0 = BigRational.FromFraction(a, b);
            BigRational second0 = BigRational.FromFraction(c, d);
            BigRational result0 = first0 + second0;

            UInt64 bits_hi = 1919810;
            // Act
            var result = Rational64.AddAsRational128(
                first,
                second,
                out bits_hi);

            var q = result0.Denominator;

            if (0 > result0.Sign) {
                q = -q;
            } else {
                --q;
            }

            Assert.Multiple(() => {
                // Assert
                Assert.That((BigInteger)result, Is.EqualTo(result0.Numerator));
                Assert.That((BigInteger)bits_hi.ToSignedUnchecked(), Is.EqualTo(q));
            });
        }

        [Test]
        public void SubtractAsRational128_StateUnderTest_ExpectedBehavior() {
            // Arrange
            Rational64 first = 1;
            Rational64 second = 10000001;
            UInt64 bits_hi = 1919810;

            // Act
            var result = Rational64.SubtractAsRational128(
                first,
                second,
                out bits_hi);

            Assert.Multiple(() => {
                // Assert
                Assert.That(result, Is.EqualTo(10000000));
                Assert.That(bits_hi, Is.EqualTo(unchecked((UInt64)(-1L))));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractAsRational128_1(UInt32 a, Int32 b, UInt32 c, Int32 d) {
            if (b == 0 || d == 0) {
                return;
            }
            // Arrange
            Rational64 first = Rational64.FromFraction(a, b);
            Rational64 second = Rational64.FromFraction(c, d);
            BigRational first0 = BigRational.FromFraction(a, b);
            BigRational second0 = BigRational.FromFraction(c, d);
            BigRational result0 = first0 - second0;

            UInt64 bits_hi = 1919810;
            // Act
            var result = Rational64.SubtractAsRational128(
                first,
                second,
                out bits_hi);

            var q = result0.Denominator;

            if (0 > result0.Sign) {
                q = -q;
            } else {
                --q;
            }

            if ((BigInteger)result != result0.Numerator ||
                (BigInteger)bits_hi.ToSignedUnchecked() != q) {
                Assert.Warn($@"a: {a}");
                Assert.Warn($@"b: {b}");
                Assert.Warn($@"c: {c}");
                Assert.Warn($@"d: {d}");
            }
            Assert.Multiple(() => {
                // Assert
                Assert.That((BigInteger)result, Is.EqualTo(result0.Numerator));
                Assert.That((BigInteger)bits_hi.ToSignedUnchecked(), Is.EqualTo(q));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractAsRational128_1M1(UInt32 a, UInt32 c, Int32 d) {
            if (d == 0) {
                return;
            }
            // Arrange
            Rational64 first = -Rational64.FromFraction(a, Int32.MinValue);
            Rational64 second = Rational64.FromFraction(c, d);
            BigRational first0 = -BigRational.FromFraction(a, Int32.MinValue);
            BigRational second0 = BigRational.FromFraction(c, d);
            BigRational result0 = first0 - second0;

            UInt64 bits_hi = 1919810;
            // Act
            var result = Rational64.SubtractAsRational128(
                first,
                second,
                out bits_hi);

            var q = result0.Denominator;

            if (0 > result0.Sign) {
                q = -q;
            } else {
                --q;
            }
            if ((BigInteger)result != result0.Numerator ||
                (BigInteger)bits_hi.ToSignedUnchecked() != q) {
                Assert.Warn($@"a: {a}");
                Assert.Warn($@"c: {c}");
                Assert.Warn($@"d: {d}");
            }
            Assert.Multiple(() => {
                // Assert
                Assert.That((BigInteger)result, Is.EqualTo(result0.Numerator));
                Assert.That((BigInteger)bits_hi.ToSignedUnchecked(), Is.EqualTo(q));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractAsRational128_1M2(UInt32 a, Int32 b, UInt32 c) {
            if (b == 0) {
                return;
            }
            // Arrange
            Rational64 first = Rational64.FromFraction(a, b);
            Rational64 second = -Rational64.FromFraction(c, Int32.MinValue);
            BigRational first0 = BigRational.FromFraction(a, b);
            BigRational second0 = -BigRational.FromFraction(c, Int32.MinValue);
            BigRational result0 = first0 - second0;

            UInt64 bits_hi = 1919810;
            // Act
            var result = Rational64.SubtractAsRational128(
                first,
                second,
                out bits_hi);

            var q = result0.Denominator;

            if (0 > result0.Sign) {
                q = -q;
            } else {
                --q;
            }

            Assert.Multiple(() => {
                // Assert
                Assert.That((BigInteger)result, Is.EqualTo(result0.Numerator));
                Assert.That((BigInteger)bits_hi.ToSignedUnchecked(), Is.EqualTo(q));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractAsRational128_1M3(UInt32 a, UInt32 c) {
            // Arrange
            Rational64 first = -Rational64.FromFraction(a, Int32.MinValue);
            Rational64 second = -Rational64.FromFraction(c, Int32.MinValue);
            BigRational first0 = -BigRational.FromFraction(a, Int32.MinValue);
            BigRational second0 = -BigRational.FromFraction(c, Int32.MinValue);
            BigRational result0 = first0 - second0;

            UInt64 bits_hi = 1919810;
            // Act
            var result = Rational64.SubtractAsRational128(
                first,
                second,
                out bits_hi);

            var q = result0.Denominator;

            if (0 > result0.Sign) {
                q = -q;
            } else {
                --q;
            }

            Assert.Multiple(() => {
                // Assert
                Assert.That((BigInteger)result, Is.EqualTo(result0.Numerator));
                Assert.That((BigInteger)bits_hi.ToSignedUnchecked(), Is.EqualTo(q));
            });
        }

        [Test]
        public void SubtractAsRational128_2() {
            SubtractAsRational128_1(1u, -1, 3u, -1);
            SubtractAsRational128_1(0u, 1, 3u, -1);
            SubtractAsRational128_1(0, 1, 0, 1);
            SubtractAsRational128_1(1u, -1, 0u, -1);
            SubtractAsRational128_1(0u, 1, 0u, 1);
            SubtractAsRational128_1(1, 1, 1, 1);
            SubtractAsRational128_1(2, -2, 1, -1);
            SubtractAsRational128_1M1(2, 0, 2);
        }

#if false
        [Test]
        public void FromInt64Bits_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Int64 bits = 0;

            // Act
            var result = Rational64.FromInt64Bits(
                bits);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToInt64Bits_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToInt64Bits(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void GetHashCodeSealed_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);

            // Act
            var result = Rational64.GetHashCodeSealed();

            // Assert
            Assert.Fail();
        }

        [Test]
        public void GetHashCode_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);

            // Act
            var result = Rational64.GetHashCode();

            // Assert
            Assert.Fail();
        }

        [Test]
        public void EqualsSealed_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 other = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.EqualsSealed(
                other);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Equals_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 other = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.Equals(
                other);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void EqualsSealed_StateUnderTest_ExpectedBehavior1() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            object other = null;

            // Act
            var result = Rational64.EqualsSealed(
                other);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void Equals_StateUnderTest_ExpectedBehavior1() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            object other = null;

            // Act
            var result = Rational64.Equals(
                other);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToRational64_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            UInt32 value = 0;

            // Act
            var result = Rational64.ToRational64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToRational64_StateUnderTest_ExpectedBehavior1() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Int32 value = 0;

            // Act
            var result = Rational64.ToRational64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToRational64_StateUnderTest_ExpectedBehavior2() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            UInt64 value = 0;

            // Act
            var result = Rational64.ToRational64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToRational64_StateUnderTest_ExpectedBehavior3() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Int64 value = 0;

            // Act
            var result = Rational64.ToRational64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToInt64Nearest_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToInt64Nearest(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToInt64_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToInt64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToUInt64_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToUInt64(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToInt32_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToInt32(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToUInt32_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToUInt32(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToDoubleInexact_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 value = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.ToDoubleInexact(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void ToRatioanl64ContinuedFractionBestApproximation_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Double value = 0;

            // Act
            var result = Rational64.ToRatioanl64ContinuedFractionBestApproximation(
                value);

            // Assert
            Assert.Fail();
        }

        [Test]
        public void CompareTo_StateUnderTest_ExpectedBehavior() {
            // Arrange
            var Rational64 = new Rational64(TODO);
            Rational64 other = default(global::UltimateOrb.Mathematics.Exact.Rational64);

            // Act
            var result = Rational64.CompareTo(
                other);

            // Assert
            Assert.Fail();
        }
#endif
    }
}
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
