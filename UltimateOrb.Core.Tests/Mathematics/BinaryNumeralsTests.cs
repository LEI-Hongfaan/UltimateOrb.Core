using UltimateOrb.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FsCheck.NUnit;
using Assert = NUnit.Framework.Assert;
using System.Runtime.InteropServices;
using System.Numerics;

namespace UltimateOrb.Mathematics.Tests {

    public class BinaryNumeralsTests {

        [Property(MaxTest = 20000, QuietOnSuccess = true)]
        public void CountTrailingZerosTest1(UInt64[] aa) {
            long v = BinaryNumerals.CountTrailingZeros(aa);
            Assert.Multiple(() => {
                Assert.That(v >= 0);
                Assert.That(v <= 64 * aa.Length);
            });

            var s = MemoryMarshal.AsBytes(aa.AsSpan());
            var i = new BigInteger(s);
            var r0 = BigInteger.TrailingZeroCount(i);

            Assert.Warn($@"{r0}, {v}");
            if (i != 0) {
                Assert.That(v == r0);
            } else {
                Assert.That(v == 64 * aa.Length);
            }
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public void CountTrailingZerosTest2() {
            ReadOnlySpan<UInt64> s = default;
            Assert.That(BinaryNumerals.CountTrailingZeros(s) == 0);
        }
    }
}