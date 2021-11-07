using FsCheck.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UltimateOrb.Core.Tests {

    public static partial class TestModule {

        [ThreadStaticAttribute()]
        private static Random s_random;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static Random GetRandom() {
            var rr = TestModule.s_random;
            if (null == rr) {
                rr = new Random();
                TestModule.s_random = rr;
            }
            return rr;
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Reverse_1(long[] a) {
            if (null == a) {
                return true;
            }
            var rr = GetRandom();
            var c = rr.Next(checked(1 + a.Length));
            var s = rr.Next(checked(1 + a.Length - c));
            // var t = unchecked(rr.Next() - rr.Next());
            var r0 = a.Clone() as long[];
            Array.Reverse<long>(r0, s, c);
            var r1 = a.Clone() as long[];
            ArrayModule.Reverse(r1, s, c);
            return r0.SequenceEqual(r1);
        }

        [Property(MaxTest = 100000, QuietOnSuccess = true)]
        public static bool Test_Reverse_2(long[] a) {
            if (null == a) {
                return true;
            }
            var rr = GetRandom();
            // var c = rr.Next(checked(1 + a.Length));
            // var s = rr.Next(checked(1 + a.Length - c));
            // var t = unchecked(rr.Next() - rr.Next());
            var r0 = a.Clone() as long[];
            Array.Reverse<long>(r0);
            var r1 = a.Clone() as long[];
            ArrayModule.Reverse(r1);
            return r0.SequenceEqual(r1);
        }

        public static bool Test_Rotate_Stub_Shift_1(Action<List<(int ProblemSize, int Shift, bool Success)>, int, long[], int> test) {
            var rr = GetRandom();
            var rs = new List<(int ProblemSize, int Shift, bool Success)>();
            for (var aL = 0; 121 > aL; ++aL) {
                var a = Enumerable.Range(0, aL).Select((x) => 10000 + 107L * x).ToArray();
                for (var c = 0; c <= aL; ++c) {
                    for (var s = 0; s <= aL - c; ++s) {
                        for (var i = -2 - c; i <= 2 + c; ++i) {
                            test(rs, aL, a, i);
                        }
                        for (var i = 3 + c; i > 0; --i) {
                            var t = unchecked(rr.Next() - rr.Next());
                            test(rs, aL, a, t);
                        }
                    }
                }
            }
            return rs.All((x) => x.Success);
        }

        public static bool Test_Rotate_Stub_Count_Start_Shift_1(Action<List<(int ProblemSize, int Count, int Start, int Shift, bool Success)>, int, long[], int, int, int> test) {
            var rr = GetRandom();
            var rs = new List<(int ProblemSize, int Count, int Start, int Shift, bool Success)>();
            for (var aL = 0; 85 > aL; ++aL) {
                var a = Enumerable.Range(0, aL).Select((x) => 10000 + 107L * x).ToArray();
                for (var c = 0; c <= aL; ++c) {
                    for (var s = 0; s <= aL - c; ++s) {
                        for (var i = -2 - c; i <= 2 + c; ++i) {
                            test(rs, aL, a, c, s, i);
                        }
                        for (var i = 3 + c; i > 0; --i) {
                            var t = unchecked(rr.Next() - rr.Next());
                            test(rs, aL, a, c, s, t);
                        }
                    }
                }
            }
            return rs.All((x) => x.Success);
        }

        public static bool Test_Rotate_Stub_Start_Mid_EndEx_1(Action<List<(int ProblemSize, int Start, int Mid, int EndEx, bool Success)>, int, long[], int, int, int> test) {
            var rr = GetRandom();
            var rs = new List<(int ProblemSize, int Start, int Mid, int EndEx, bool Success)>();
            for (var aL = 0; 100 > aL; ++aL) {
                var a = Enumerable.Range(0, aL).Select((x) => 10000 + 107L * x).ToArray();
                for (var c = 0; c <= aL; ++c) {
                    for (var s = c; s <= aL; ++s) {
                        for (var t = s; t <= aL; ++t) {
                            test(rs, aL, a, c, s, t);
                        }
                    }
                }
            }
            return rs.All((x) => x.Success);
        }

        private static void Test_Rotate_RotateInPlace_RotateLeftInPlace_1_1(List<(int ProblemSize, int Count, int Start, int Shift, bool Success)> rs, int aL, long[] a, int c, int s, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateInPlace(r0, s, s + (c == 0 ? 0 : (0 > t ? t % c + c : t % c)), s + c);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r1, s, c, t);
            rs.Add((aL, c, s, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateInPlace_RotateLeftInPlace_1() {
            return Test_Rotate_Stub_Count_Start_Shift_1(Test_Rotate_RotateInPlace_RotateLeftInPlace_1_1);
        }

        private static void Test_Rotate_RotateInPlace_RotateLeftInPlace_3_1(List<(int ProblemSize, int Shift, bool Success)> rs, int aL, long[] a, int t) {
            var r0 = a.Clone() as long[];
            var c = r0.Length;
            ArrayModule.RotateInPlace(r0, 0, (c == 0 ? 0 : (0 > t ? t % c + c : t % c)), c);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r1, t);
            rs.Add((aL, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateInPlace_RotateLeftInPlace_3() {
            return Test_Rotate_Stub_Shift_1(Test_Rotate_RotateInPlace_RotateLeftInPlace_3_1);
        }

        private static void Test_Rotate_RotateInPlace_RotateLeftInPlace_2_1(List<(int ProblemSize, int Start, int Mid, int EndEx, bool Success)> rs, int aL, long[] a, int s, int m, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateInPlace(r0, s, m, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r1, s, checked(t - s), checked(m - s));
            rs.Add((aL, s, t, m, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateInPlace_RotateLeftInPlace_2() {
            return Test_Rotate_Stub_Start_Mid_EndEx_1(Test_Rotate_RotateInPlace_RotateLeftInPlace_2_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_AA_1_1(List<(int ProblemSize, int Count, int Start, int Shift, bool Success)> rs, int aL, long[] a, int c, int s, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r0, s, c, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace_A(r1, s, c, t);
            rs.Add((aL, c, s, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_AA_1() {
            return Test_Rotate_Stub_Count_Start_Shift_1(Test_Rotate_RotateLeftInPlace_AA_1_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_AA_2_1(List<(int ProblemSize, int Shift, bool Success)> rs, int aL, long[] a, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r0, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace_A(r1, t);
            rs.Add((aL, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_AA_2() {
            return Test_Rotate_Stub_Shift_1(Test_Rotate_RotateLeftInPlace_AA_2_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_1_1(List<(int ProblemSize, int Count, int Start, int Shift, bool Success)> rs, int aL, long[] a, int c, int s, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace_A(r0, s, c, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateRightInPlace_A(r1, s, c, checked(-t));
            rs.Add((aL, c, s, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_1() {
            return Test_Rotate_Stub_Count_Start_Shift_1(Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_1_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_2_1(List<(int ProblemSize, int Shift, bool Success)> rs, int aL, long[] a, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace_A(r0, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateRightInPlace_A(r1, checked(-t));
            rs.Add((aL, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_2() {
            return Test_Rotate_Stub_Shift_1(Test_Rotate_RotateLeftInPlace_RotateRightInPlace_AA_2_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_RotateRightInPlace_1_1(List<(int ProblemSize, int Count, int Start, int Shift, bool Success)> rs, int aL, long[] a, int c, int s, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r0, s, c, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateRightInPlace(r1, s, c, checked(-t));
            rs.Add((aL, c, s, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_RotateRightInPlace_1() {
            return Test_Rotate_Stub_Count_Start_Shift_1(Test_Rotate_RotateLeftInPlace_RotateRightInPlace_1_1);
        }

        private static void Test_Rotate_RotateLeftInPlace_RotateRightInPlace_2_1(List<(int ProblemSize, int Shift, bool Success)> rs, int aL, long[] a, int t) {
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r0, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateRightInPlace(r1, checked(-t));
            rs.Add((aL, t, r0.SequenceEqual(r1)));
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_Rotate_RotateLeftInPlace_RotateRightInPlace_2() {
            return Test_Rotate_Stub_Shift_1(Test_Rotate_RotateLeftInPlace_RotateRightInPlace_2_1);
        }

        [Property(MaxTest = 10000, QuietOnSuccess = true, Verbose = true)]
        public static bool Test_Rotate_5(long[] a) {
            if (null == a) {
                return true;
            }
            var rr = GetRandom();
            // var c = rr.Next(checked(1 + a.Length));
            // var s = rr.Next(checked(1 + a.Length - c));
            var t = unchecked(rr.Next() - rr.Next());
            var r0 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace(r0, t);
            var r1 = a.Clone() as long[];
            ArrayModule.RotateLeftInPlace_A(r1, t);
            var rrr = r0.SequenceEqual(r1);
            if (!rrr) {
                while (!rrr) {
                }
            }
            return rrr;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_IsPermutation_1() {
            var a = "Banaan🌍nas".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a.Clone() as char[], b.Clone() as char[], c);
                s.Add((t++, z));
                if (ArrayModule.NextPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_IsPermutation_2() {
            var a = "Banaan🌍nas".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.NextPermutation(b, d)) {
                    continue;
                }
                break;
            }
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.PreviousPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value);
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_NextPermutation_3() {
            var a = "abcdefgh".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.NextPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value) && s.Count == 40320;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_NextPermutation_4() {
            var a = "abcdefgg".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.NextPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value) && s.Count == 20160;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_PreviousPermutation_3() {
            var a = "hgfedcba".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.PreviousPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value) && s.Count == 40320;
        }

        [Property(MaxTest = 1, QuietOnSuccess = true)]
        public static bool Test_PreviousPermutation_4() {
            var a = "ggfedcba".ToCharArray();
            var c = ((Func<char, char, bool>)((x, y) => x == y)).AsFunc();
            var d = ((Func<char, char, bool>)((x, y) => x < y)).AsFunc();
            var t = 0UL;
            var s = new List<(ulong Id, bool Value)>(0);
            for (var b = a.Clone() as char[]; ;) {
                var z = ArrayModule.IsPermutation(a as char[], b as char[], c);
                s.Add((t++, z));
                if (ArrayModule.PreviousPermutation(b, d)) {
                    continue;
                }
                break;
            }
            return s.All((x) => x.Value) && s.Count == 20160;
        }
    }
}
