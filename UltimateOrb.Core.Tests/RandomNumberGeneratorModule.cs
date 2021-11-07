using System;

namespace UltimateOrb.Core.Tests {
    using System.Runtime.CompilerServices;

    public static partial class RandomNumberGeneratorModule {

        [ThreadStaticAttribute()]
        private static byte[] buffer_1;

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static byte[] Get_buffer_1() {
            var a = buffer_1;
            if (null == buffer_1) {
                a = new byte[16];
                buffer_1 = a;
            }
            return a;
        }
        /*
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 GetNext<TRandom>(ref this TRandom random, Int64 maxExclusive) where TRandom : struct, IRandomNumberGenerator {
            if (maxExclusive > 0) {

                var a = random.GetNextInt32(1 << 22);
                var b = random.GetNextInt32(1 << 22);
                var c = random.GetNextInt32(unchecked((Int32)(maxExclusive >> (64 - (22 + 22)))));
                return (Int64)a | ((Int64)b << 22) | ((Int64)c << (22 + 22));
            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int64 GetNext<TRandom>(this TRandom random, Int64 maxExclusive) where TRandom : IRandomNumberGenerator {
            if (maxExclusive > 0) {
                var a = random.GetNextInt32(1 << 22);
                var b = random.GetNextInt32(1 << 22);
                var c = random.GetNextInt32(unchecked((Int32)(maxExclusive >> (64 - (22 + 22)))));
                return (Int64)a | ((Int64)b << 22) | ((Int64)c << (22 + 22));
            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetNext<TRandom>(ref this TRandom random, UInt64 maxExclusive) where TRandom : struct, IRandomNumberGenerator {
            if (maxExclusive > 0) {
                var a = random.GetNextInt32(1 << 22);
                var b = random.GetNextInt32(1 << 22);
                var c = random.GetNextInt32(unchecked((Int32)(maxExclusive >> (64 - (22 + 22)))));
                return (UInt64)(UInt32)a | ((UInt64)(UInt32)b << 22) | ((UInt64)(UInt32)c << (22 + 22));
            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetNext<TRandom>(this TRandom random, UInt64 maxExclusive) where TRandom : IRandomNumberGenerator {
            if (maxExclusive > 0) {
                var a = random.GetNextInt32(1 << 22);
                var b = random.GetNextInt32(1 << 22);
                var c = random.GetNextInt32(unchecked((Int32)(maxExclusive >> (64 - (22 + 22)))));
                
                return (UInt64)(UInt32)a | ((UInt64)(UInt32)b << 22) | ((UInt64)(UInt32)c << (22 + 22));
            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int128 GetNext<TRandom>(ref this TRandom random, Int128 maxExclusive) where TRandom : struct, IRandomNumberGenerator {
            if (maxExclusive > 0) {
                var a = random.GetNextInt32(1 << 26);
                var b = random.GetNextInt32(1 << 26);
                var c = random.GetNextInt32(1 << 26);
                var d = random.GetNextInt32(1 << 26);
                var e = random.GetNextInt32(unchecked((Int32)(maxExclusive >> (128 - (22 + 22)))));

                return (UInt64)(UInt32)a | ((UInt64)(UInt32)b << 22) | ((UInt64)(UInt32)c << (22 + 22));

            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Int128 GetNext<TRandom>(this TRandom random, Int128 maxExclusive) where TRandom : IRandomNumberGenerator {
            if (maxExclusive > 0) {
               
            }
            throw new ArgumentOutOfRangeException(nameof(maxExclusive));
        }
        */
    }
}
