using FsCheck.NUnit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace UltimateOrb.Core.Tests {

    public class NaNDefaultedDoubleTests {

        [Property(MaxTest = 200000, QuietOnSuccess = true)]
        public void Test_0001(int value) {
            var v1 = (NaNDefaultedDouble)value;
            var v0 = (double)value;
            Assert.True(BitConverter.DoubleToInt64Bits(v0).Equals(BitConverter.DoubleToInt64Bits(v1)));
        }

        [Property(MaxTest = 200000, QuietOnSuccess = true)]
        public void Test_0002(double v0) {
            var v1 = (NaNDefaultedDouble)v0;
            Assert.True(BitConverter.DoubleToInt64Bits(v0).Equals(BitConverter.DoubleToInt64Bits(v1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public void Test_0003() {
            NaNDefaultedDouble v = default;
            Assert.True(NaNDefaultedDouble.IsNaN(v));
            Assert.True(Double.IsNaN((double)v));
        }

        [Property(MaxTest = 50000, QuietOnSuccess = true)]
        public void Test_Add_0001(double x0, double y0) {
            NaNDefaultedDouble x1 = x0;
            NaNDefaultedDouble y1 = y0;
            Assert.True(BitConverter.DoubleToInt64Bits(x0 + y0).Equals(BitConverter.DoubleToInt64Bits(x1 + y1)));
        }

    }
}
