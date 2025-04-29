
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb;
namespace UltimateOrb.Core.Tests {
    public static class RefStructTest {

        public readonly ref struct RefStructA<T> {

            public readonly ByReference<T[]?> a;
            public readonly ByReference<long> b;
            public RefStructA(ref T[]? a, ref long b) {
                this.a = new ByReference<T[]?>(ref a);
                this.b = new ByReference<long>(ref b);
            }
        }

        public static long ReadAField<T>(this RefStructA<T> a) {
            return a.b.Value;
        }

        public static long ReadAFieldA() {
            ulong[] a = default!;
            long b = default;
            var s = new RefStructA<ulong>(ref a, ref b);
            return s.ReadAField();
        }
    }

}