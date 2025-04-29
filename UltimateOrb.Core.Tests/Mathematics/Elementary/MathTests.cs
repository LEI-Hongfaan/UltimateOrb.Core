#nullable enable

using UltimateOrb.Mathematics.Elementary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Runtime.InteropServices;
using System.Numerics;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
using System.Buffers;

namespace UltimateOrb.Mathematics.Elementary.Tests {
   
    public partial class MathTests {

        public static bool CheckSqrt(UInt32 radicand, UInt32 root) {
            UInt32 p;
            return root <= UInt16.MaxValue &&
                (p = root * root) <= radicand &&
                radicand - p <= 2 * root;
        }

        public static bool CheckSqrt(UInt64 radicand, UInt64 root) {
            UInt64 p;
            return root <= UInt32.MaxValue &&
                (p = root * root) <= radicand &&
                radicand - p <= 2 * root;
        }

        public static bool CheckSqrt(UInt128 radicand, UInt128 root) {
            UInt128 p;
            return root <= UInt64.MaxValue &&
                (p = root * root) <= radicand &&
                radicand - p <= 2 * root;
        }

        public static bool CheckSqrt(BigInteger radicand, BigInteger root) {
            if (0 > radicand) {
                throw new InvalidOperationException();
            }
            BigInteger p;
            return root >= 0 &&
                (p = root * root) <= radicand &&
                radicand - p <= 2 * root;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public bool Sqrt_A_F_32_Test() {
            for (ulong i = 0; i <= UInt32.MaxValue; i++) {
                var radicand = checked((UInt32)i);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public bool Sqrt_A_F_64_Test() {
            {
                var radicand = checked((UInt64)0);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            for (ulong i = 1; i <= UInt32.MaxValue; i++) {
                var radicand = checked((UInt64)((UInt64)i * i));
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
                radicand = checked((UInt64)(radicand - 1));
                root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            {
                var radicand = checked((UInt64)UInt64.MaxValue);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public bool Sqrt_A_F_128_a_Test() {
            {
                var radicand = checked((UInt128)0);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            for (ulong i = 1; i <= UInt32.MaxValue; i++) {
                var radicand = checked((UInt128)((UInt64)i * i));
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
                radicand = checked((UInt128)(radicand - 1));
                root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            {
                var radicand = checked((UInt128)UInt64.MaxValue);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public bool Sqrt_A_F_128_b_Test() {
            {
                var radicand = checked((UInt128)0);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            for (ulong i = 1; i <= UInt32.MaxValue; i++) {
                var radicand = checked((UInt128)(((UInt64)i * i) << 64));
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
                radicand = checked((UInt128)(radicand - 1));
                root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            {
                var radicand = checked((UInt128)(UInt64.MaxValue << 64));
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 10000, QuietOnSuccess = true)]
        public bool Sqrt_A_F_128_c_Test() {
            var rr = new Random();
            var count = 64 * 1024;
            var buffer = ArrayPool<UInt128>.Shared.Rent(count);
            try {
                var inputs = buffer.AsSpan(..count);
                rr.NextBytes(MemoryMarshal.AsBytes(inputs));
                foreach (var input in inputs) {
                    var radicand = checked((UInt128)input);
                    var root = Math.Sqrt(radicand);
                    if (!CheckSqrt(radicand, root)) {
                        Assert.Warn($@"Incorrect: Sqrt({(System.UInt128)radicand:X}) got {(System.UInt128)root:X}");
                        return false;
                    }
                }
                return true;
            } finally {
                ArrayPool<UInt128>.Shared.Return(buffer);
            }
        }

        [Property(MaxTest = 1000, QuietOnSuccess = true)]
        public bool Sqrt_A_F_128_d_Test() {
            {
                var radicand = checked((UInt128)0);
                var root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
                radicand = checked((UInt128)UInt128.MaxValue);
                root = Math.Sqrt(radicand);
                if (!CheckSqrt(radicand, root)) {
                    return false;
                }
            }
            var rr = new Random();
            var count = 1024 * 1024;
            var buffer = ArrayPool<UInt64>.Shared.Rent(count);
            try {
                var inputs = buffer.AsSpan(..count);
                rr.NextBytes(MemoryMarshal.AsBytes(inputs));
                foreach (var input in inputs) {
                    var radicand = checked((UInt128)((UInt128)input * input));
                    var root = Math.Sqrt(radicand);
                    if (!CheckSqrt(radicand, root)) {
                        return false;
                    }
                    radicand = checked((UInt128)(radicand - 1));
                    root = Math.Sqrt(radicand);
                    if (!CheckSqrt(radicand, root)) {
                        return false;
                    }
                }
                return true;
            } finally {
                ArrayPool<UInt64>.Shared.Return(buffer);
            }
        }
    }
}

namespace UltimateOrb.Mathematics.Elementary.Tests {
    //[TestClass()]
    public partial class MathTests {

        struct aaaaa {



            public static aaaaa<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> referenceImpl, Func<T1, T2, TResult> testImpl) {
                return new aaaaa<T1, T2, TResult>(referenceImpl, testImpl);
                }
        }
        struct aaaaa<T1, T2, TResult>  {

            readonly Func<T1, T2, TResult> ReferenceImpl;
            readonly Func<T1, T2, TResult> TestImpl;
            internal  aaaaa(Func<T1, T2, TResult> referenceImpl, Func<T1, T2, TResult> testImpl) {
                if (referenceImpl is null) {
                    throw new ArgumentNullException(nameof(referenceImpl));
                }
                if (testImpl is null) {
                    throw new ArgumentNullException(nameof(testImpl));
                }
                ReferenceImpl = referenceImpl;
                TestImpl = testImpl;
            }

            public bool Test(T1 arg1, T2 arg2) {
                Exception? rEx = null;
                TResult rRv = default!;
                try {
                    rRv = ReferenceImpl.Invoke(arg1, arg2);
                } catch (Exception ex) {
                    rEx = ex;
                }

                Exception? iEx = null;
                TResult iRv = default!;
                try {
                    iRv = TestImpl.Invoke(arg1, arg2);
                } catch (Exception ex) {
                    iEx = ex;
                }

                if (rEx is null) {
                    if (iEx is null) {
                        return EqualityComparer<TResult>.Default.Equals(rRv, iRv);
                    }
                } else if (iEx is not null) {
                    return rEx.GetType().IsAssignableFrom(iEx.GetType());
                }
                return false;
            }


        }

        //[TestMethod]
        [Property(MaxTest = 20000, QuietOnSuccess = true)]
        public bool FloorTest(int value, int divisor) {
            var a = aaaaa.Create(static (int value, int divisor) => {
                var r = Math.Floor((long)value, (long)divisor);
                return checked((int)r);
            }, Math.Floor);
            return a.Test(value, divisor);
        }
    }
}