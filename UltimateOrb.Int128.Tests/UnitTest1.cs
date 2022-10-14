using FsCheck;
using FsCheck.Xunit;

namespace UltimateOrb.Tests {

    public class UnitTest1 {

        [Fact]
        public void Test1() {
        }

        [Property(MaxTest = 3000)]
        public void Test2(int x, int y) {
        }
    }
}