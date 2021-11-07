using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace UltimateOrb.Core.Tests {

    public static partial class TestModule {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal static bool Test_Sqrt_Stub_0001(long s) {
            for (long i = s; i <= s + 0x3FFFFFFF; ++i) {
                var v = unchecked((UInt32)i);
                var f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                var g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_1_A() {
            return Test_Sqrt_Stub_0001(0);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_1_B() {
            return Test_Sqrt_Stub_0001(0x4000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_1_C() {
            return Test_Sqrt_Stub_0001(0x8000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_1_D() {
            return Test_Sqrt_Stub_0001(0xC000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_3() {
            for (ulong i = 0; i <= 0x1000000; ++i) {
                var v = i;
                var f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                var g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
                v = ~i;
                f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
            }
            for (ulong i = 1; i <= 0x100000000; i += 101) {
                var v = i;
                var f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                var g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
                v = ~i;
                f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
            }
            for (ulong i = 2; i <= 0x10000000000; i += 10001) {
                var v = i;
                var f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                var g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
                v = ~i;
                f = Mathematics.Elementary.Math.Sqrt_A_F(v);
                g = Mathematics.Elementary.Math.Sqrt_A_I(v);
                if (f != g) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1000000, QuietOnSuccess = true)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool Test_Sqrt_4(UInt64 v) {
            for (UInt64 i = 1; 0 != i; i <<= 1) {
                var w = v ^ i;
                var f = Mathematics.Elementary.Math.Sqrt_A_F(w);
                var g = Mathematics.Elementary.Math.Sqrt_A_I(w);
                if (f != g) {
                    return false;
                }
            }
            return true;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal static bool Test_Sqrt_Stub_0002(ulong s) {
            UInt64 q = checked(s * s);
            unchecked {
                --q;
            }
            for (var i = s; 0x40000000 + s > i; ++i) {
                unchecked {
                    ++q;
                }
                var f = Mathematics.Elementary.Math.Sqrt_A_F(q);
                if (f != i) {
                    return false;
                }
                unchecked {
                    ++q;
                }
                f = Mathematics.Elementary.Math.Sqrt_A_F(q);
                if (f != i) {
                    if (i != 0) {
                        return false;
                    }
                }
                unchecked {
                    q += i << 1;
                    --q;
                }
                f = Mathematics.Elementary.Math.Sqrt_A_F(q);
                if (f != i) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_2_A() {
            return Test_Sqrt_Stub_0002(0x0000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_2_B() {
            return Test_Sqrt_Stub_0002(0x4000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_2_C() {
            return Test_Sqrt_Stub_0002(0x8000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Sqrt_2_D() {
            return Test_Sqrt_Stub_0002(0xC000000);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        internal static bool Test_SqrtRem_Stub_0002(ulong s) {
            UInt64 q = checked(s * s);
            unchecked {
                --q;
            }
            for (var i = s; 0x40000000 + s > i; ++i) {
                unchecked {
                    ++q;
                }
                var f = Mathematics.Elementary.Math.SqrtRem_A_F(q, out var r);
                if (f != i || r != 0) {
                    return false;
                }
                unchecked {
                    ++q;
                }
                f = Mathematics.Elementary.Math.SqrtRem_A_F(q, out r);
                if (f != i || r != 1) {
                    if (i != 0) {
                        return false;
                    }
                }
                unchecked {
                    q += i << 1;
                    --q;
                }
                f = Mathematics.Elementary.Math.SqrtRem_A_F(q, out r);
                if (f != i || r != i << 1) {
                    return false;
                }
            }
            return true;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_2_A() {
            return Test_SqrtRem_Stub_0002(0x0000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_2_B() {
            return Test_SqrtRem_Stub_0002(0x4000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_2_C() {
            return Test_SqrtRem_Stub_0002(0x8000000);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_2_D() {
            return Test_SqrtRem_Stub_0002(0xC000000);
        }

        [Property(MaxTest = 1000000, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_3(UInt64 v) {
            var q = Mathematics.Elementary.Math.SqrtRem_A_F(v, out var r);
            var w = checked(r + q * q);
            if (w != v) {
                return false;
            }
            if (r > checked(q * 2)) {
                return false;
            }
            return true;
        }

        [Property(MaxTest = 1000000, QuietOnSuccess = true)]
        public static bool Test_SqrtRem_4(UInt64 v) {
            var q = Mathematics.Elementary.Math.SqrtRem_A_I(v, out var r);
            var w = checked(r + q * q);
            if (w != v) {
                return false;
            }
            if (r > checked(q * 2)) {
                return false;
            }
            return true;
        }
    }
}
