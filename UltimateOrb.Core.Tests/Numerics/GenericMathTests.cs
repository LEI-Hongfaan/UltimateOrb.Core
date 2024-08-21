using NUnit.Framework;
using UltimateOrb.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics.Tests {

    [TestFixture()]
    public partial class GenericMathTests {

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void AddWithCarryTest_Unsigned(UInt32 a, UInt32 b) {
            var cy0 = !CheckedNoThrow.TryAdd(a, b, out var sum0);

            var (sum, cy) = GenericMath.AddUnsignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void AddWithCarryTest_Signed(Int32 a, Int32 b) {
            var cy0 = !CheckedNoThrow.TryAdd(a.ToUnsignedUnchecked(), b.ToUnsignedUnchecked(), out var sum0);

            var (sum, cy) = GenericMath.AddUnsignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0.ToSignedUnchecked()));
            });
        }


        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void AddWithOverflowTest_Unsigned(UInt32 a, UInt32 b) {
            var ov0 = !CheckedNoThrow.TryAdd(a.ToSignedUnchecked(), b.ToSignedUnchecked(), out var sum0);

            var (sum, ov) = GenericMath.AddSignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(ov, Is.EqualTo(ov0));
                Assert.That(sum, Is.EqualTo(sum0.ToUnsignedUnchecked()));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void AddWithOverflowTest_Signed(Int32 a, Int32 b) {
            var ov0 = !CheckedNoThrow.TryAdd(a, b, out var sum0);

            var (sum, ov) = GenericMath.AddSignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(ov, Is.EqualTo(ov0));
                Assert.That(sum, Is.EqualTo(sum0));
            });
        }

        [Test]
        public void AddWithOverflowTest_Signed_1() {
            AddWithOverflowTest_Signed(int.MinValue, -2);
        }

        [Test]
        public void AddWithOverflowTest_Signed_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    AddWithOverflowTest_Signed(x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    AddWithOverflowTest_Signed(x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                    AddWithOverflowTest_Signed(~x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    AddWithOverflowTest_Signed(~x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                }
            }
        }

        [Test]
        public void AddWithOverflowTest_Unsigned_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    AddWithOverflowTest_Unsigned(x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    AddWithOverflowTest_Unsigned(x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                    AddWithOverflowTest_Unsigned(~x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    AddWithOverflowTest_Unsigned(~x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                }
            }
        }

        [Test]
        public void AddWithCarryTest_Signed_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    AddWithCarryTest_Signed(x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    AddWithCarryTest_Signed(x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                    AddWithCarryTest_Signed(~x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    AddWithCarryTest_Signed(~x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                }
            }
        }

        [Test]
        public void AddWithCarryTest_Unsigned_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    AddWithCarryTest_Unsigned(x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    AddWithCarryTest_Unsigned(x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                    AddWithCarryTest_Unsigned(~x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    AddWithCarryTest_Unsigned(~x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                }
            }
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractWithBorrowTest_Unsigned(UInt32 a, UInt32 b) {
            var cy0 = !CheckedNoThrow.TrySubtract(a, b, out var sum0);

            var (sum, cy) = GenericMath.SubtractUnsignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractWithBorrowTest_Signed(Int32 a, Int32 b) {
            var cy0 = !CheckedNoThrow.TrySubtract(a.ToUnsignedUnchecked(), b.ToUnsignedUnchecked(), out var sum0);

            var (sum, cy) = GenericMath.SubtractUnsignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0.ToSignedUnchecked()));
            });
        }

        [Test]
        public void SubtractWithBorrowTest_Signed_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    SubtractWithBorrowTest_Signed(x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    SubtractWithBorrowTest_Signed(x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                    SubtractWithBorrowTest_Signed(~x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    SubtractWithBorrowTest_Signed(~x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                }
            }
        }

        [Test]
        public void SubtractWithBorrowTest_Unsigned_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    SubtractWithBorrowTest_Unsigned(x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    SubtractWithBorrowTest_Unsigned(x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                    SubtractWithBorrowTest_Unsigned(~x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    SubtractWithBorrowTest_Unsigned(~x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                }
            }
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractWithOverflowTest_Unsigned(UInt32 a, UInt32 b) {
            var cy0 = !CheckedNoThrow.TrySubtract(a, b, out var sum0);

            var (sum, cy) = GenericMath.SubtractSignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0));
            });
        }

        [Property(MaxTest = 2000000, QuietOnSuccess = true, MaxFail = 1)]
        public void SubtractWithOverflowTest_Signed(Int32 a, Int32 b) {
            var cy0 = !CheckedNoThrow.TrySubtract(a.ToUnsignedUnchecked(), b.ToUnsignedUnchecked(), out var sum0);

            var (sum, cy) = GenericMath.SubtractSignedWithOverflow(a, b);

            Assert.Multiple(() => {
                Assert.That(cy, Is.EqualTo(cy0));
                Assert.That(sum, Is.EqualTo(sum0.ToSignedUnchecked()));
            });
        }

        [Test]
        public void SubtractWithOverflowTest_Signed_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    SubtractWithOverflowTest_Signed(x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    SubtractWithOverflowTest_Signed(x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                    SubtractWithOverflowTest_Signed(~x.ToSignedUnchecked(), y.ToSignedUnchecked());
                    SubtractWithOverflowTest_Signed(~x.ToSignedUnchecked(), ~y.ToSignedUnchecked());
                }
            }
        }

        [Test]
        public void SubtractWithOverflowTest_Unsigned_2() {
            var a = UltimateOrb.Utilities.Extensions.BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(32, 3);
            foreach (var x in a) {
                foreach (var y in a) {
                    SubtractWithOverflowTest_Unsigned(x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    SubtractWithOverflowTest_Unsigned(x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                    SubtractWithOverflowTest_Unsigned(~x.ToUnsignedUnchecked(), y.ToUnsignedUnchecked());
                    SubtractWithOverflowTest_Unsigned(~x.ToUnsignedUnchecked(), ~y.ToUnsignedUnchecked());
                }
            }
        }
    }
}