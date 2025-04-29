using FsCheck;
using FsCheck.Xunit;

namespace UltimateOrb.Tests {

    public class UInt128Tests {

        [Fact]
        public void Convert_UInt128_To_Single_Should_MatchReferenceImplementation() {
            // Iterate through all 32-bit patterns.
            // Here, we use a ulong loop variable so we can cover the full [0, uint.MaxValue] range
            // and then cast to int (with unchecked, this covers negative bit-patterns correctly).
            for (ulong b = 0x7F800000; b <= uint.MaxValue; ++b) {
                // 'bits' represents the raw 32-bit bit pattern.
                int bits = unchecked((int)b);

                float f = BitConverter.Int32BitsToSingle(bits);

                // Convert the integer bits to our UInt128 implementations.
                // These conversions assume that both System.UInt128 and UltimateOrb.UInt128
                // support an implicit or explicit conversion from int.
                var sysValue = (System.UInt128)f;
                var ultValue = (UltimateOrb.UInt128)f;

                // Verify that both UInt128 implementations represent the same underlying value.
                Assert.True(!float.IsFinite(f) || f < 0 || sysValue.Equals(ultValue),
                    $"UInt128 mismatch for bits 0x{bits:X8} ({f:R}): System value {sysValue} != UltimateOrb value {ultValue}");

                if (!float.IsFinite(f) || f < 0) {
                    continue;
                }

                // Convert to float.
                float sysResult = (float)sysValue;
                float ultResult = (float)ultValue;

                // Using the instance Equals method on Single, which correctly handles -0.0/0.0 and NaN cases.
                Assert.True(sysResult.Equals(ultResult),
                    $"Float conversion mismatch for bits 0x{bits:X8}: System raw bits: 0x{BitConverter.SingleToInt32Bits(sysResult):X8}, UltimateOrb raw bits: 0x{BitConverter.SingleToInt32Bits(ultResult):X8}");

                // Test conversion when adding 1.
                float sysPlus = (float)unchecked(sysValue + 1);
                float ultPlus = (float)unchecked(ultValue + 1);
                Assert.True(sysPlus.Equals(ultPlus),
                    $"+1 conversion mismatch for bits 0x{bits:X8}: System raw bits: 0x{BitConverter.SingleToInt32Bits(sysPlus):X8}, UltimateOrb raw bits: 0x{BitConverter.SingleToInt32Bits(ultPlus):X8}");

                // Test conversion when subtracting 1.
                float sysMinus = (float)unchecked(sysValue - 1);
                float ultMinus = (float)unchecked(ultValue - 1);
                Assert.True(sysMinus.Equals(ultMinus),
                    $"-1 conversion mismatch for bits 0x{bits:X8}: System raw bits: 0x{BitConverter.SingleToInt32Bits(sysMinus):X8}, UltimateOrb raw bits: 0x{BitConverter.SingleToInt32Bits(ultMinus):X8}");
            }
        }

        [Fact]
        public void Convert_UInt128_To_Half_Should_MatchReferenceImplementation() {
            // Start at zero.
            var value = UltimateOrb.UInt128.Zero;
            // Loop until the reference conversion produces Half.PositiveInfinity.
            // Note: For each value, we convert using both the UltimateOrb implementation and the reference System.UInt128.
            while (true) {
                // Convert using the reference conversion (System.UInt128 as ref impl).
                Half expected = (Half)((System.UInt128)value);
                // Convert using the UltimateOrb conversion.
                Half actual = (Half)value;


                // Assert that both conversions produce the same Half value.
                Assert.True(expected.Equals(actual),
                       $"Conversion mismatch for bits 0X{value:x16}: System value {expected:R} != UltimateOrb value {actual:R}");

                // If the conversion reached positive infinity, we stop testing.
                if (value > 65536) {
                    break;
                }

                // Increment the UltimateOrb.UInt128 value. Assumes that operator+ is overloaded.
                value = value + 1;
            }
        }

        [Property(MaxTest = 3000)]
        public void Test2(int x, int y) {
        }
    }
}