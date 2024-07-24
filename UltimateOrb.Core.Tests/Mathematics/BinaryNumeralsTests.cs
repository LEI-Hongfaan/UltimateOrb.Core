using UltimateOrb.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck.NUnit;
using Assert = NUnit.Framework.Assert;

namespace UltimateOrb.Mathematics.Tests {

    public class BinaryNumeralsTests {

        [Property(MaxTest = 20, QuietOnSuccess = true)]
        public void CountTrailingZerosTest1(ReadOnlySpan<UInt64> aa) {
            Assert.That(BinaryNumerals.CountTrailingZeros(aa) >= 0);
            Assert.That(BinaryNumerals.CountTrailingZeros(aa) <= 64 * aa.Length);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public void CountTrailingZerosTest2() {
            ReadOnlySpan<UInt64> s = default;
            Assert.That(BinaryNumerals.CountTrailingZeros(s) == 0);
        }

        [Property(MaxTest = 100, QuietOnSuccess = true)]
        public void CountTrailingZerosTest2(UInt64 a) {
            ReadOnlySpan<UInt64> s = stackalloc UInt64[] { a };
            Assert.That(BinaryNumerals.CountTrailingZeros(s) == BinaryNumerals.CountTrailingZeros(a));
        }

        [Property(MaxTest = 100, QuietOnSuccess = true)]
        public void CountTrailingZerosTest2(UInt64 a, UInt64 b) {
            ReadOnlySpan<UInt64> s = stackalloc UInt64[] { a, b };
            Assert.That(BinaryNumerals.CountTrailingZeros(s) >= BinaryNumerals.CountTrailingZeros(s[1..]));
        }

        [Property(MaxTest = 100, QuietOnSuccess = true)]
        public void CountTrailingZerosTest2(UInt64 a, UInt64 b, UInt64 c) {
            ReadOnlySpan<UInt64> s = stackalloc UInt64[] { a, b, c };
            Assert.That(BinaryNumerals.CountTrailingZeros(s) >= BinaryNumerals.CountTrailingZeros(s[1..]));
        }
    }
}