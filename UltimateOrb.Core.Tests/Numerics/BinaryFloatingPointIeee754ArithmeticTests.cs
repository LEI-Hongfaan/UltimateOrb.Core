using NUnit.Framework;
using QuadrupleLib;
using System;
using System.Runtime.CompilerServices;
using UltimateOrb;

namespace UltimateOrb.Numerics.Tests {

    [TestFixture()]
    public partial class BinaryFloatingPointIeee754ArithmeticTests {

        [Test]
        public void SingleIsIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateSingle()) {
                var s = Single.IsInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsInteger<Single, UInt32>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.SingleToInt32Bits(item):X8})");
                }
            }
        }

        [Test]
        public void DoubleIsIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateDouble()) {
                var s = Double.IsInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsInteger<Double, UInt64>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.DoubleToInt64Bits(item):X16})");
                }
            }
        }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        [Test]
        public void QuadrupleIsIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateQuadruple()) {
                var s = QuadrupleLib.Float128<QuadrupleLib.Accelerators.DefaultAccelerator>.IsInteger(
                    Unsafe.BitCast<Quadruple, Float128<QuadrupleLib.Accelerators.DefaultAccelerator>>(item));
                var t = BinaryFloatingPointIeee754Arithmetic.IsInteger<Quadruple, System.UInt128>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {(double)item} (0x{BitConverter.QuadrupleToInt128Bits(item):X32})");
                }
            }
        }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        [Test]
        public void SingleIsEvenIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateSingle()) {
                var s = Single.IsEvenInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsEvenInteger<Single, UInt32>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.SingleToInt32Bits(item):X8})");
                }
            }
        }

        [Test]
        public void DoubleIsEvenIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateDouble()) {
                var s = Double.IsEvenInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsEvenInteger<Double, UInt64>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.DoubleToInt64Bits(item):X16})");
                }
            }
        }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        [Test]
        public void QuadrupleIsEvenIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateQuadruple()) {
                var s = QuadrupleLib.Float128<QuadrupleLib.Accelerators.DefaultAccelerator>.IsEvenInteger(
                    Unsafe.BitCast<Quadruple, Float128<QuadrupleLib.Accelerators.DefaultAccelerator>>(item));
                var t = BinaryFloatingPointIeee754Arithmetic.IsEvenInteger<Quadruple, System.UInt128>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {(double)item} (0x{BitConverter.QuadrupleToInt128Bits(item):X32})");
                }
            }
        }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        [Test]
        public void SingleIsOddIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateSingle()) {
                var s = Single.IsOddInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsOddInteger<Single, UInt32>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.SingleToInt32Bits(item):X8})");
                }
            }
        }

        [Test]
        public void DoubleIsOddIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateDouble()) {
                var s = Double.IsOddInteger(item);
                var t = BinaryFloatingPointIeee754Arithmetic.IsOddInteger<Double, UInt64>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {item} (0x{BitConverter.DoubleToInt64Bits(item):X16})");
                }
            }
        }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        [Test]
        public void QuadrupleIsOddIntegerTest() {
            foreach (var item in BitPatternGenerator.GenerateQuadruple()) {
                var s = QuadrupleLib.Float128<QuadrupleLib.Accelerators.DefaultAccelerator>.IsOddInteger(
                    Unsafe.BitCast<Quadruple, Float128<QuadrupleLib.Accelerators.DefaultAccelerator>>(item));
                var t = BinaryFloatingPointIeee754Arithmetic.IsOddInteger<Quadruple, System.UInt128>(item);
                if (s != t) {
                    Assert.That(t, Is.EqualTo(s), $"Mismatch for value {(double)item} (0x{BitConverter.QuadrupleToInt128Bits(item):X32})");
                }
            }
        }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    }
}