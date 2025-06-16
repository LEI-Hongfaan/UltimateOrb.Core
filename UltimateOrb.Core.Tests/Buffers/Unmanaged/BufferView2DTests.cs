using System;
using System.Linq;
using System.Runtime.InteropServices;
using NUnit.Framework;
using FsCheck;
using FsCheck.NUnit;
using UltimateOrb.Buffers.Unmanaged;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;

namespace UltimateOrb.Buffers.Unmanaged.Tests {

    [TestFixture]
    public class BufferView2DTests {

        [Test]
        public unsafe void Constructor_And_Properties_Work() {
            int[] data = { 1, 2, 3, 4, 5, 6 };
            fixed (int* p = data) {
                var view = new BufferView2D<int>(p, 2, 3);

                Assert.That(view.Length0, Is.EqualTo(2));
                Assert.That(view.Length0, Is.EqualTo(2));
                Assert.That(view.Length1, Is.EqualTo(3));
                Assert.That(view.Length, Is.EqualTo(6));
                Assert.That(view.LongLength0, Is.EqualTo(2L));
                Assert.That(view.LongLength1, Is.EqualTo(3L));
                Assert.That(view.LongLength, Is.EqualTo(6L));
            }
        }

        [Test]
        public unsafe void Constructor_With_Stride_Works() {
            int[] data = { 1, 2, 3, 4, 5, 6, 7, 8 };
            fixed (int* p = data) {
                var view = new BufferView2D<int>(p, 2, 3, 4);
                Assert.That(view.Length0, Is.EqualTo(2));
                Assert.That(view.Length1, Is.EqualTo(3));
            }
        }

        [Test]
        public unsafe void Indexer_Returns_Correct_Reference() {
            int[] data = { 10, 20, 30, 40, 50, 60 };
            fixed (int* p = data) {
                var view = new BufferView2D<int>(p, 2, 3);
                ref int val = ref view[1, 2];
                Assert.That(val, Is.EqualTo(60));
                val = 99;
                Assert.That(data[5], Is.EqualTo(99));
            }
        }

        [Test]
        public unsafe void Indexer_Throws_On_OutOfRange() {
            int[] data = new int[6];
            fixed (int* p = data) {
                var view = new BufferView2D<int>(p, 2, 3);
                Assert.Throws<IndexOutOfRangeException>(() => { var _ = view[2, 0]; });
                Assert.Throws<IndexOutOfRangeException>(() => { var _ = view[0, 3]; });
            }
        }

        [Test]
        public unsafe void ToStandardArray2D_Copies_Data() {
            int[] data = { 1, 2, 3, 4, 5, 6 };
            fixed (int* p = data) {
                var view = new BufferView2D<int>(p, 2, 3);
                var arr = view.ToStandardArray2D();
                Assert.That(arr, Is.Not.Null);
                Assert.That(arr.GetLength(0), Is.EqualTo(2));
                Assert.That(arr.GetLength(1), Is.EqualTo(3));
                Assert.That(arr[1, 0], Is.EqualTo(4));
                Assert.That(arr[1, 2], Is.EqualTo(6));
            }
        }

        [Test]
        public void ToStandardArray2D_Returns_Null_If_Base_Is_Zero() {
            var view = default(BufferView2D<int>);
            var arr = view.ToStandardArray2D();
            Assert.That(arr, Is.Null);
        }

        [Property]
        public void Length_And_LongLength_Are_Consistent(uint l0, uint l1) {
            // Avoid overflow and zero-length
            l0 = Math.Max(1, Math.Min(l0, 100));
            l1 = Math.Max(1, Math.Min(l1, 100));
            unsafe {
                int[] data = new int[l0 * l1];
                fixed (int* p = data) {
                    var view = new BufferView2D<int>(p, (nint)l0, (nint)l1);
                    Assert.That(view.Length, Is.EqualTo((int)(l0 * l1)));
                    Assert.That(view.LongLength, Is.EqualTo((long)l0 * l1));
                }
            }
        }

        [Property]
        public void Indexer_And_ToStandardArray2D_Agree(uint l0, uint l1) {
            l0 = Math.Max(1, Math.Min(l0, 20));
            l1 = Math.Max(1, Math.Min(l1, 20));
            unsafe {
                int[] data = Enumerable.Range(0, (int)(l0 * l1)).ToArray();
                fixed (int* p = data) {
                    var view = new BufferView2D<int>(p, (nint)l0, (nint)l1);
                    var arr = view.ToStandardArray2D();
                    for (int i = 0; i < l0; ++i)
                        for (int j = 0; j < l1; ++j)
                            Assert.That(arr[i, j], Is.EqualTo(view[i, j]));
                }
            }
        }
    }
}
