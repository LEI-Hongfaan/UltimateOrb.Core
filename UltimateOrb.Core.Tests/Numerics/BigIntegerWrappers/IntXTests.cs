using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using NUnit.Framework;
using UltimateOrb.Numerics.BigIntegerWrappers;
using UltimateOrb.Numerics.DataTypes;

#if false
namespace UltimateOrb.Numerics.BigIntegerWrappers.Tests {
    using Int13 = Int<BitSize13>;

    [Experimental("UoWIP_GenericMath")]
    public struct BitSize13 : IConstant<BitSize13, int> {
        public static int Value => 13;
    }

    [Experimental("UoWIP_GenericMath")]
    [TestFixture]
    public partial class IntXTests {

        [TestCase(4095, 4095)]
        [TestCase(4096, -4096)]
        [TestCase(4097, -4095)]
        [TestCase(8191, -1)]
        [TestCase(8192, 0)]
        [TestCase(-1, -1)]
        [TestCase(-4096, -4096)]
        [TestCase(-4097, 4095)]
        public void UncheckedConstructor_13Bit_WrapsCorrectly(long input, int expected) {
            var value = new BigInteger(input);
            var wrapped = unchecked((Int13)value);
            Assert.That((BigInteger)wrapped, Is.EqualTo((BigInteger)expected));
        }
    }

    [TestFixture]
    public partial class IntXTests {

        [TestCase(4095, 4095)]
        [TestCase(4096, -4096)]
        [TestCase(4097, -4095)]
        [TestCase(8191, -1)]
        [TestCase(8192, 0)]
        [TestCase(-1, -1)]
        [TestCase(-4096, -4096)]
        [TestCase(-4097, 4095)]
        public void CheckedConstructor_13Bit_ThrowsOnOverflow(long input, int expected) {
            var value = new BigInteger(input);
            bool shouldThrow = value < -4096 || value > 4095;
            if (shouldThrow) {
                Assert.Throws<OverflowException>(() => { var _ = checked((Int13)value); });
            } else {
                var wrapped = checked((Int13)value);
                Assert.That((BigInteger)wrapped, Is.EqualTo((BigInteger)expected));
            }
        }
    }
}

namespace UltimateOrb.Numerics.BigIntegerWrappers.Tests {
    using Int257 = Int<BitSize257>;

    [Experimental("UoWIP_GenericMath")]
    public struct BitSize257 : IConstant<BitSize257, int> {
        public static int Value => 257;
    }

    [TestFixture]
    public partial class IntXTests {

        [TestCase("0", "0")]
        [TestCase("1", "1")]
        [TestCase("-1", "-1")]
        [TestCase("170141183460469231731687303715884105727", "170141183460469231731687303715884105727")] // 2^127-1
        [TestCase("170141183460469231731687303715884105728", "170141183460469231731687303715884105728")]
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279871", "-1")] // 2^257-1
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279872", "0")] // 2^257
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279873", "1")] // 2^257+1
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279872", "0")] // -2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279873", "-1")] // -2^257-1
        public void UncheckedConstructor_257Bit_WrapsCorrectly(string input, string expected) {
            var value = BigInteger.Parse(input);
            var wrapped = unchecked((Int257)value);
            Assert.That((BigInteger)wrapped, Is.EqualTo(BigInteger.Parse(expected)));
        }

        [TestCase("0", "0", false)]
        [TestCase("1", "1", false)]
        [TestCase("-1", "-1", false)]
        [TestCase("170141183460469231731687303715884105727", "170141183460469231731687303715884105727", false)] // 2^127-1
        [TestCase("170141183460469231731687303715884105728", "170141183460469231731687303715884105728", false)]
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279871", "-1", true)] // 2^257-1
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279872", "0", true)] // 2^257
        [TestCase("231584178474632390847141970017375815706539969331281128078915168015826259279873", "1", true)] // 2^257+1
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279872", "0", true)] // -2^257
        [TestCase("-231584178474632390847141970017375815706539969331281128078915168015826259279873", "-1", true)] // -2^257-1
        public void CheckedConstructor_257Bit_ThrowsOnOverflow(string input, string expected, bool shouldThrow) {
            var value = BigInteger.Parse(input);
            if (shouldThrow) {
                Assert.Throws<OverflowException>(() => { var _ = checked((Int257)value); });
            } else {
                var wrapped = checked((Int257)value);
                Assert.That((BigInteger)wrapped, Is.EqualTo(BigInteger.Parse(expected)));
            }
        }
    }
}
#endif