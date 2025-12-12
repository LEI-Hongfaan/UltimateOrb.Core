using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using NUnit.Framework;
using UltimateOrb.Numerics.BigIntegerWrappers;
using UltimateOrb.Numerics.DataTypes;

namespace UltimateOrb.Numerics.BigIntegerWrappers.Tests {

    [Experimental("UoWIP_GenericMath")]
    public partial class UIntXTests {
    }
}

namespace UltimateOrb.Numerics.BigIntegerWrappers.Tests {
    using UInt13 = UInt<BitSize13>;

    [TestFixture]
    public partial class UIntXTests {

        [TestCase(0, 0)]
        [TestCase(4095, 4095)]
        [TestCase(4096, 4096)]
        [TestCase(8191, 8191)]
        [TestCase(8192, 0)]
        [TestCase(8193, 1)]
        [TestCase(-1, 8191)]
        [TestCase(-4096, 4096)]
        [TestCase(-4097, 4095)]
        public void UncheckedConstructor_13Bit_WrapsCorrectly(long input, int expected) {
            var value = new BigInteger(input);
            var wrapped = unchecked((UInt13)value);
            Assert.That((BigInteger)wrapped, Is.EqualTo((BigInteger)expected));
        }
    }

    [TestFixture]
    public partial class UIntXTests {

        [TestCase(0, 0)]
        [TestCase(4095, 4095)]
        [TestCase(4096, 4096)]
        [TestCase(8191, 8191)]
        [TestCase(8192, 0)]
        [TestCase(8193, 1)]
        [TestCase(-1, 8191)]
        [TestCase(-4096, 4096)]
        [TestCase(-4097, 4095)]
        public void CheckedConstructor_13Bit_ThrowsOnOverflow(long input, int expected) {
            var value = new BigInteger(input);
            bool shouldThrow = value < 0 || value > 8191;
            if (shouldThrow) {
                Assert.Throws<OverflowException>(() => { var _ = checked((UInt13)value); });
            } else {
                var wrapped = checked((UInt13)value);
                Assert.That((BigInteger)wrapped, Is.EqualTo((BigInteger)expected));
            }
        }
    }
}

namespace UltimateOrb.Numerics.BigIntegerWrappers.Tests {
    using UInt257 = UInt<BitSize257>;

    [TestFixture]
    public partial class UIntXTests {

        [TestCase("0", "0")]
        [TestCase("1", "1")]
        [TestCase("170141183460469231731687303715884105727", "170141183460469231731687303715884105727")]
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279871", "231584178474632390847141970017375815706539969331281128078915168015826259279871")] // 2^257-1
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279872", "0")] // 2^257
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279873", "1")] // 2^257+1
        [TestCase("-1", "231584178474632390847141970017375815706539969331281128078915168015826259279871")] // -1 mod 2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279872", "0")] // -2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279873", "231584178474632390847141970017375815706539969331281128078915168015826259279871")] // -2^257-1
        public void UncheckedConstructor_257Bit_WrapsCorrectly(string input, string expected) {
            var value = BigInteger.Parse(input);
            var wrapped = unchecked((UInt257)value);
            Assert.That((BigInteger)wrapped, Is.EqualTo(BigInteger.Parse(expected)));
        }

        [TestCase("0", "0", false)]
        [TestCase("1", "1", false)]
        [TestCase("170141183460469231731687303715884105727", "170141183460469231731687303715884105727", false)]
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279871", "231584178474632390847141970017375815706539969331281128078915168015826259279871", false)] // 2^257-1
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279872", "0", true)] // 2^257
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279873", "1", true)] // 2^257+1
        [TestCase("-1", "231584178474632390847141970017375815706539969331281128078915168015826259279871", true)] // -1 mod 2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279872", "0", true)] // -2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279873", "231584178474632390847141970017375815706539969331281128078915168015826259279871", true)] // -2^257-1
        public void CheckedConstructor_257Bit_ThrowsOnOverflow(string input, string expected, bool shouldThrow) {
            var value = BigInteger.Parse(input);
            var min = BigInteger.Zero;
            var max = BigInteger.Parse("231584178474632390847141970017375815706539969331281128078915168015826259279871");
            if (shouldThrow) {
                Assert.Throws<OverflowException>(() => { var _ = checked((UInt257)value); });
            } else {
                var wrapped = checked((UInt257)value);
                Assert.That((BigInteger)wrapped, Is.EqualTo(BigInteger.Parse(expected)));
            }
        }
    }
}
