

using System;

namespace UltimateOrb.Core.Tests {
    using System.Runtime.InteropServices;
    using System.Threading;
    using UltimateOrb.Plain.ValueTypes;
    using UltimateOrb.Mathematics.Geometry;
    using System.Runtime.CompilerServices;

    internal static class Program {


        private static int aasfd() {
            var rr = new Random();
            var a = 0;

            for (var i = 0; 100 > i; ++i) {

                var asdf = rr.Next(0x8000);
                var b = new byte[asdf];
                a ^= b.GetHashCode();
            }

            for (var i = 0; GC.MaxGeneration > i; ++i) {
                if (3 > rr.Next(100)) {
                    GC.Collect();
                }
            }
            return a;
        }

        static readonly int ff = 33;

        static int ff2 = 33;
        static CanonicalIntegerBoolean ff3;

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static int Main(string[] args) {
            {
                Thread.MemoryBarrier();
                ff2 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff2 = unchecked((int)(byte)ff);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = Unsafe.As<int, CanonicalIntegerBoolean>(ref Unsafe.AsRef(ff));
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = true;
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = false;
                Thread.MemoryBarrier();
                System.Diagnostics.Debugger.Break();
                var sds12 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Console.WriteLine($@"{sds12}");
                var sds = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(3, 7);
                Console.WriteLine($@"{sds}");
                var sds1 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(1, 0);
                Console.WriteLine($@"{sds1}");
                return 0;
            }
            {

                UltimateOrb.Numerics.Generic.Long<Int64, UInt64> ggg = new(3, 6);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);

                return 0;
            }
            {
                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }

                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }
            }

            {

                var threads = new Thread[10];
                var a = true;
                for (int i = 0; i < threads.Length; i++) {
                    threads[i] = new Thread(() => {
                        for (; Volatile.Read(ref a);) {
                            aasfd();
                        }
                    });
                }
                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Start();
                }
                Thread.Sleep(100);
                try {
                    aasfd();
                    var sdfas = new int[333];
                    aasfd();
                    var sdafsdf = sdfas.AsMemorySpan(2, 42);
                    aasfd();
                    var sdfasdf = sdafsdf.Span;
                    aasfd();

                    for (var i = 0; sdfas.Length > i; ++i) {
                        aasfd();
                        sdfasdf[i] = -i;
                        aasfd();
                    }
                    Volatile.Write(ref a, false);
                    var sadfa = sdfas[44];
                    Console.WriteLine(sadfa);
                } finally {
                    Volatile.Write(ref a, false);
                }

                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Join();
                }
                return 0;
            }
            {

                var func_x = (Func<double, double>)((x) => x * x);
                var func_y = (Func<double, double>)((x) => x / 2 - 100);

                var curve_x = func_x.ToParametricCurve((-2, 3)).Reparametrization(x => 2 * x, x => x / 2);
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(1).ToString());
                return 0;

            }

            {

                var func_x = (Func<double, double>)((x) => -x);
                var func_y = (Func<double, double>)((x) => 2 * x);

                var curve_x = func_x.ToParametricCurve((-2, 3));
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(2).ToString());
                return 0;
            }



            {
                var vdsadf = new BigUIntegerBuilder(333);
                vdsadf.MultiplyPow10(20);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Multiply(1000000000);
                var sadf = vdsadf.ToString();
                Console.WriteLine($@"{sadf}");
                return 0;
            }
            {
                var a = typeof(MathF);
                Console.WriteLine($@"{(Quadruple)1.5}");
                return 0;
            }
            {
                var a = -2.8;
                var b = 1.0;
                Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{Math.IEEERemainder(a, b):R}");

                // Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{(Double)Quadruple.Math.IEEERemainder(a, b):R}");

                return 0;

            }
            {
                var a1 = 1;
                var a2 = 2;
                var a3 = 3;
                var a4 = 4;
                var b = (double)(1L << (52 + 2));
                for (var i = 0; i < 64; i++) {
                    var bb = Math.ScaleB(b, i);

                    for (var j = 0; j < 8; ++j) {
                        Console.WriteLine($@"{UltimateOrb.Utilities.CilVerifiable.AddThenSubtractFirst(bb, j):R}");
                    }
                }
                return 0;

            }
            {
                var sdf = (object)(-4);

                Console.WriteLine(UltimateOrb.Utilities.CilVerifiable.UnboxRef<int>(sdf));

                return 0;
            }
            {
                var aaa = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var bbb = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var a = 0L;
                for (var i = 0L; i < 400000000000L; i++) {
                    a ^= UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(aaa, bbb);

                }
                Console.WriteLine(a);

                return 0;
            }
            {
                var sdffa = new int[] {
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, Double.NaN),
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, 0.2),
                };

                foreach (var item in sdffa) {
                    Console.WriteLine(item);
                }


                return 0;
            }
            {
                var a = (1UL << 52) + 1;
                var b = (1UL << 52);
                var c = (1UL << 52) - 1;
                var d = (1UL << 52) - 1;

                var p = a + 0.5;
                var q = b + 0.5;
                var r = c + 0.5;
                var s = d + 0.75;
                var sp = d + 0.76;
                var sm = d + 0.74;

                Console.WriteLine($@"{double.Epsilon:R}");
                Console.WriteLine($@"{double.Epsilon * d:R}");
                Console.WriteLine($@"{p:R}");
                Console.WriteLine($@"{q:R}");
                Console.WriteLine($@"{r:R}");
                Console.WriteLine($@"{s:R}");
                Console.WriteLine($@"{sp:R}");
                Console.WriteLine($@"{sm:R}");
                return 0;
            }
            {

                var sdfada = 0u;
                sdfada ^= sdfada;
                var sdaf = System.Numerics.BitOperations.LeadingZeroCount(sdfada);
                _ = sdaf.GetHashCode();

            }
            {
                var sfassss = BitConverter.Int64BitsToDouble(0x7FFF400000000000);
                var sfas = sfassss;
                var vdsa = 0.0 - sfas;
                var sfad = BitConverter.DoubleToInt64Bits(sfas);
                var asdsd = BitConverter.DoubleToInt64Bits(vdsa);
                _ = (sfad ^ asdsd).GetHashCode();

            }
            {


                var ccc = (Quadruple)0.0 < (Quadruple)Double.NegativeInfinity;
                Console.WriteLine(ccc);
            }
            {
                var ccc = Double.NaN != Double.NaN;
                var a = Quadruple.IsNaN(Quadruple.NaN);
                Console.WriteLine(a);
                var sdfa = +Quadruple.MinValue;
                Console.WriteLine(sdfa);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            return 0;
        }
    }
}


namespace UltimateOrb.Plain.ValueTypes {

    public readonly struct NodeId_A
        : IEquatable<NodeId_A>, IFormattable {

        private readonly int m_value;

        internal NodeId_A(int value) {
            this.m_value = value;
        }

        public static NodeId_A operator +(NodeId_A value) {
            return value;
        }

        public static NodeId_A operator -(NodeId_A value) {
            return new NodeId_A(unchecked(~value.m_value - 1));
        }

        public static NodeId_A operator <<(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value << shift);
        }

        public static NodeId_A operator >>(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value >> shift);
        }

        public static NodeId_A operator +(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(1 + first.m_value + second.m_value));
        }

        public static NodeId_A operator -(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(first.m_value + ~second.m_value));
        }

        public static NodeId_A operator *(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value * ~second.m_value)));
        }

        public static NodeId_A operator /(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value / ~second.m_value)));
        }

        public static NodeId_A operator %(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value % ~second.m_value)));
        }

        public static NodeId_A operator &(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value | second.m_value);
        }

        public static NodeId_A operator |(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value & second.m_value);
        }

        public static NodeId_A operator ^(NodeId_A first, NodeId_A second) {
            return new NodeId_A(~(first.m_value ^ second.m_value));
        }

        public static NodeId_A operator ~(NodeId_A value) {
            return new NodeId_A(~value.m_value);
        }

        public static NodeId_A operator ++(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value - 1));
        }

        public static NodeId_A operator --(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value + 1));
        }

        public static implicit operator int(NodeId_A value) {
            return ~value.m_value;
        }

        public static implicit operator NodeId_A(int value) {
            return new NodeId_A(~value);
        }

        public override bool Equals(object obj) {
            if (obj is NodeId_A other) {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(NodeId_A other) {
            return this.m_value == other.m_value;
        }

        public override int GetHashCode() {
            return this.m_value;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            return (~this.m_value).ToString(format, formatProvider);
        }

        public override string ToString() {
            return (~this.m_value).ToString();
        }

        public static bool operator ==(NodeId_A left, NodeId_A right) {
            return left.m_value == right.m_value;
        }

        public static bool operator !=(NodeId_A left, NodeId_A right) {
            return left.m_value != right.m_value;
        }
    }
}
