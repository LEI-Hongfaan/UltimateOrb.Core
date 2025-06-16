#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using System;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using FsCheck;
using FsCheck.NUnit;
using UltimateOrb;
using Assert = NUnit.Framework.Assert;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;

namespace UltimateOrb.Core.Tests {

    [TestFixture]
    public class UInt256ConversionTests {

        private static double ReferenceToDouble(UInt256 value) {
            var br = (Numerics.BigRational)(BigInteger)value;
            return (double)br;
        }

        [TestCase(0UL)]
        [TestCase(1UL)]
        [TestCase(2UL)]
        [TestCase(123456789UL)]
        [TestCase(ulong.MaxValue)]
        public void UInt256_ToDouble_MatchesReference_ForUlong(ulong input) {
            var u = (UInt256)input;
            double expected = ReferenceToDouble(u);
            double actual = (double)u;
            Assert.That(actual, Is.EqualTo(expected), $"Failed for input {input}");
        }

        [Test]
        public void UInt256_ToDouble_MaxValue() {
            var u = UInt256.MaxValue;
            double expected = ReferenceToDouble(u);
            double actual = (double)u;
            Assert.That(actual, Is.EqualTo(expected), "Failed for UInt256.MaxValue");
        }

        [Test]
        public void UInt256_ToDouble_PowersOfTwo() {
            for (int i = 0; i < 256; i += 1) {
                var u = UInt256.One << i;
                double expected = ReferenceToDouble(u);
                double actual = (double)u;
                Assert.That(actual, Is.EqualTo(expected), $"Failed for 1 << {i}");
            }
        }

        [Property(MaxTest = 100000)]
        public void UInt256_ToDouble_Randomized() {
            var rng = Random.Shared;
            Span<byte> bytes = stackalloc byte[32];
            for (int i = 0; i < 20; ++i) {
                rng.NextBytes(bytes);
                // Bit-cast the bytes to UInt256
                UInt256 u = Unsafe.ReadUnaligned<UInt256>(ref MemoryMarshal.GetReference(bytes));
                double expected = ReferenceToDouble(u);
                double actual = (double)u;
                Assert.That(actual, Is.EqualTo(expected), $"Failed for random value {u}");
            }
        }

        // FsCheck property-based test
        [Property(MaxTest = 100000)]
        public void UInt256_ToDouble_Property(ulong d0, ulong d1, ulong d2, ulong d3) {
            var u = UInt256.FromBits(UInt128.FromBits(d0, d1), UInt128.FromBits(d2, d3));
            double expected = ReferenceToDouble(u);
            double actual = (double)u;
            // Allow a small tolerance for floating-point rounding
            Assert.That(actual, Is.EqualTo(expected), $"Failed for UInt256 {u}");
        }

        [Test]
        public void UInt256_ToDouble_RoundsToEven_WhenExactlyHalfway() {
            // 2^53 is exactly representable in double, so 2^53+1 is halfway between 2^53 and 2^53+2
            var u = (UInt256)9007199254740992UL; // 2^53
            var u_halfway = u + 1; // 2^53 + 1
            double d_halfway = (double)u_halfway;
            // Should round to even (2^53)
            Assert.That(d_halfway, Is.EqualTo((double)u), "Halfway value should round to even (down)");

            // 2^53+2 is also exactly representable, so halfway between 2^53+2 and 2^53+4 is 2^53+3
            var u_even = u + 2; // 2^53 + 2
            var u_halfway2 = u + 3; // 2^53 + 3
            double d_halfway2 = (double)u_halfway2;
            // Should round to even (2^53 + 4)
            Assert.That(d_halfway2, Is.EqualTo((double)(u + 4)), "Halfway value should round to even (up)");
        }

        [Test]
        public void UInt256_ToDouble_RoundsDown_JustBelowHalfway() {
            // 2^53 + 0.499... should round down to 2^53
            var u = (UInt256)9007199254740992UL; // 2^53
            var u_just_below = u + 0; // 2^53
            double d_just_below = (double)u_just_below;
            Assert.That(d_just_below, Is.EqualTo((double)u), "Just below halfway should round down");
        }

        [Test]
        public void UInt256_ToDouble_RoundsUp_JustAboveHalfway() {
            // 2^53 + 1.5 should round up to 2^53 + 2
            var u = (UInt256)9007199254740992UL; // 2^53
            var u_just_above = u + 2; // 2^53 + 2
            double d_just_above = (double)u_just_above;
            Assert.That(d_just_above, Is.EqualTo((double)(u + 2)), "Just above halfway should round up");
        }

        [Test]
        public void UInt256_ToDouble_RoundsCorrectly_LargeValues() {
            // Test a value near the double's mantissa limit (2^104)
            var u = (UInt256.One << 104);
            var u_halfway = u + (UInt256.One << 51); // Halfway between 2^104 and 2^104 + 2^52
            double d_halfway = (double)u_halfway;
            // Should round to even (down)
            Assert.That(d_halfway, Is.EqualTo((double)u), "Large halfway value should round to even (down)");

            var u_even = u + (UInt256.One << 52); // Next even double
            var u_halfway2 = u + (UInt256.One << 52) + (UInt256.One << 51); // Halfway between 2^104+2^52 and 2^104+2^53
            double d_halfway2 = (double)u_halfway2;
            // Should round to even (up)
            Assert.That(d_halfway2, Is.EqualTo((double)(u + (UInt256.One << 53))), "Large halfway value should round to even (up)");
        }

        [Test]
        public void UInt256_ToDouble_CancelsRoundingAdjustment_WhenNoExtraBits() {
            // 2^53 is exactly representable in double
            var u = UInt256.One << 53;
            double d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 53)), "2^53 should be exactly representable");

            // 2^104 is exactly representable in double
            u = UInt256.One << 104;
            d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 104)), "2^104 should be exactly representable");
        }

        [Test]
        public void UInt256_ToDouble_AppliesRoundingAdjustment_WhenExtraBitsPresent() {
            // 2^53 + 1 is not exactly representable, should round to even (down)
            var u = (UInt256.One << 53) + 1;
            double d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 53)), "2^53+1 should round down to 2^53");

            // 2^53 + 2 should round up to 2^53 + 2
            u = (UInt256.One << 53) + 2;
            d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 53) + 2), "2^53+2 should be exactly representable");

            // 2^104 + (1 << 51) is halfway, should round to even (down)
            u = (UInt256.One << 104) + (UInt256.One << 51);
            d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 104)), "2^104 + 2^51 should round down to 2^104");

            // 2^104 + (1 << 52) should round up to 2^104 + 2^52
            u = (UInt256.One << 104) + (UInt256.One << 52);
            d = (double)u;
            Assert.That(d, Is.EqualTo(Math.Pow(2, 104) + Math.Pow(2, 52)), "2^104 + 2^52 should be exactly representable");
        }
    }
}
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
