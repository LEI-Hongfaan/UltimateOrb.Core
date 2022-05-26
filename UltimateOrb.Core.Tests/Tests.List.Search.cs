using FsCheck.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace UltimateOrb.Core.Tests {

    public static partial class TestModule {
        
        internal readonly struct CharComparer : IFunc<char, char, bool> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public bool Invoke(char arg1, char arg2) {
                return InvokeSealed(arg1, arg2);
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            private static bool InvokeSealed(char arg1, char arg2) {
                return arg1 == arg2;
            }
        }
        
        [Property(MaxTest = 50000, QuietOnSuccess = true)]
        public static bool Test_SearchKnuthMorrisPratt_1(string a, string b) {
            if (null == a || null == b) {
                return true;
            }
            return Test_SearchKnuthMorrisPratt_Stub0001(a, b);
        }

        [Property(MaxTest = 50000, QuietOnSuccess = true)]
        public static bool Test_SearchKnuthMorrisPratt_2(string a, string b, string c) {
            if (null == a || null == b || null == c) {
                return true;
            }
            a = a + b + c;
            return Test_SearchKnuthMorrisPratt_Stub0001(a, b);
        }

        private static bool Test_SearchKnuthMorrisPratt_Stub0001(string a, string b) {
            var r0 = a.IndexOf(b);
            var r1 = (int?)null;
            var a1 = a.AsReadOnlyList();
            var b1 = b.AsReadOnlyList();
            try {
                r1 = SequenceSearchModule.IndexOf_A_KnuthMorrisPratt<char, StandardStringAsReadOnlyList, StandardStringAsReadOnlyList, CharComparer>(a1, b1, DefaultConstructor.Invoke<CharComparer>());
            } catch (Exception) {
            }
            return r0 == r1;
        }

        [Property(MaxTest = 50000, QuietOnSuccess = true)]
        public static bool Test_SearchKnuthMorrisPratt_3(string a, string b, string c) {
            if (null == a || null == b || null == c) {
                return true;
            }
            a = a + b + c + b;
            return Test_SearchKnuthMorrisPratt_Stub0001(a, b);
        }
    }
}
