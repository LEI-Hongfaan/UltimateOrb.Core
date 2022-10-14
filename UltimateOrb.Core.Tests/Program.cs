

using System;

namespace UltimateOrb.Core.Tests {
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using UltimateOrb.Mathematics.Geometry;
    using UltimateOrb.Numerics;
    using UltimateOrb.Plain.ValueTypes;

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
        static double aaffaa = -3.0;

        const int dssaf = 16 * 1024 * 1024;

        [StructLayout(LayoutKind.Sequential, Size = dssaf)]
        struct Bits8388608 {
            internal unsafe fixed UInt64 b[dssaf / sizeof(UInt64)];

        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static int Main(string[] args) {
            {
                for (uint i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MaxValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = uint.MaxValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;

            }
            {


                for (var i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MaxValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;




            }
            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        p0 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        unsafe {
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 1] = 11000000 + (UInt64)i;
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 2] = 22000000 + (UInt64)i;

                        }


                        if (i == 7) {
                            p1 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 77) {
                            p2 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 777) {
                            p3 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 7777) {
                            p4 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;




            }

            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        p0 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        affffffffs[^1] = 11000000 + (UInt64)i;
                        affffffffs[^2] = 22000000 + (UInt64)i;

                        if (i == 7) {
                            p1 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 77) {
                            p2 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 777) {
                            p3 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 7777) {
                            p4 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;
            }
            {
                {
                    Console.WriteLine(Math.ILogB(-3));
                    Console.WriteLine(Math.ScaleB(double.MaxValue, 1000000000));


                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        Console.WriteLine(Math.ILogB(Math.Abs(Volatile.Read(ref aaffaa))));
                    }
                    aaaa();
                    aaaa();

                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var afa = new UltimateOrb.Numerics.QuaternionD(101, 102, 103, 104);
                        Console.WriteLine(afa.X);
                    }
                    aaaa();
                    aaaa();
                }
            }
            {

                if (false) {
                    Console.WriteLine($@"{new Vector3D(4, 3, 5).GetNormalizedSafe()}");
                    Console.WriteLine($@"{new Vector3D(1, 1, 1).GetNormalizedSafe()}");
                    {
                        var aa = new Vector3D(4 * double.Epsilon, 3 * double.Epsilon, 5 * double.Epsilon);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.MaxValue);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, 1);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.NaN);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    return 0;

                }


                const bool Extrinsic = false;
                const bool Intrinsic = true;
                System.Numerics.Vector3 X = new System.Numerics.Vector3(1, 0, 0);
                System.Numerics.Vector3 Y = new System.Numerics.Vector3(0, 1, 0);
                System.Numerics.Vector3 Z = new System.Numerics.Vector3(0, 0, 1);

                static System.Numerics.Matrix4x4 M0(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, double angle0, double angle1, double angle2) {
                    var t0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis0, angle0.ToStandardF());
                    var t1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis1, angle1.ToStandardF());
                    var t2 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis2, angle2.ToStandardF());
                    return isIntrinsic ? t2 * t1 * t0 : t0 * t1 * t2;
                }

                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.Extensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var m0 = aa.ToStandardF();
                        UltimateOrb.Numerics.Extensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(
                            x, y, z, a,
                            out var a1, out var a2, out var a3);
                        var m1 = M0(Intrinsic, X, Y, Z, a1, a2, a3);


                        var d = m0 - m1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 3.5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;
                }
                if (false) {




                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        new System.Numerics.Vector3(0, 0, 1),
                        MathF.PI / 3.0F);
                    Console.WriteLine(m0);
                    return 0;

                }


                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.Extensions.ToExtrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;

                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.Extensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }


                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.Extensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var a0 = aa.ToStandardF();
                        var a1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                            System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x, y, z)), a);

                        var d = a0 - a1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 7e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 12e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;





                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.Extensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }
                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);

                    UltimateOrb.Numerics.Extensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);

                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }











                    /*
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }*/
                    return 0;
                }




                {


                    static System.Numerics.Matrix4x4 M1(Converter<Vector3D, Vector4D> converter, double angle0, double angle1, double angle2) {
                        var s = converter(new Vector3D(angle0, angle1, angle2));
                        var t = System.Numerics.Matrix4x4.CreateFromAxisAngle(s.E012.GetNormalized().ToStandardF(), s.E3.ToStandardF());
                        return t;
                    }

                    static int sss(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, Converter<Vector3D, Vector4D> converter) {
                        var m0 = (double angle0, double angle1, double angle2)
                            => M0(isIntrinsic, axis0, axis1, axis2, angle0, angle1, angle2);
                        var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                        var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                        var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                        var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                        var rr = new Random();

                        var eMax = float.NaN;
                        var e1Max = float.NaN;
                        for (var i = 0; 10_000_000 > i; ++i) {
                            var angle0 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle1 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle2 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var a0 = m0(angle0, angle1, angle2);
                            var a1 = M1(converter, angle0, angle1, angle2);

                            var d = a0 - a1;
                            var dx = System.Numerics.Vector4.Transform(vx, d);
                            var dy = System.Numerics.Vector4.Transform(vy, d);
                            var dz = System.Numerics.Vector4.Transform(vz, d);
                            var dw = System.Numerics.Vector4.Transform(vw, d);
                            var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                            var e1 = dw.LengthSquared();
                            if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                                eMax = e;
                            }
                            if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                                e1Max = e1;
                            }
                            if (e >= 3.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }
                            if (e1 >= 5.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }

                        }

                        Console.WriteLine("...");
                        Console.WriteLine(eMax);
                        Console.WriteLine(e1Max);
                        Console.WriteLine();
                        return 0;
                    }




                    if (true) {

                        sss(Extrinsic, X, Y, Z, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicXYZEulerAngles);
                        sss(Intrinsic, X, Y, Z, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicXYZEulerAngles);
                        sss(Extrinsic, X, Z, Y, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicXZYEulerAngles);
                        sss(Intrinsic, X, Z, Y, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicXZYEulerAngles);
                        sss(Extrinsic, Y, X, Z, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicYXZEulerAngles);
                        sss(Intrinsic, Y, X, Z, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicYXZEulerAngles);
                        sss(Extrinsic, Y, Z, X, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicYZXEulerAngles);
                        sss(Intrinsic, Y, Z, X, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicYZXEulerAngles);
                        sss(Extrinsic, Z, X, Y, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicZXYEulerAngles);
                        sss(Intrinsic, Z, X, Y, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicZXYEulerAngles);
                        sss(Extrinsic, Z, Y, X, UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicZYXEulerAngles);
                        sss(Intrinsic, Z, Y, X, UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicZYXEulerAngles);



                        return 0;
                    }
                }


                {
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    var q = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{rb}");
                    Console.WriteLine($@"{p}");
                    Console.WriteLine($@"{q}");
                    // p |--> p . rb


                }
                {


                    var ra = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 1, 0), 0.5F * MathF.PI);
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    p = System.Numerics.Vector4.Transform(p, ra);
                    p = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{p}");
                }


                {
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI));
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(2, 0, 0), 0.5F * MathF.PI)); // incorrect


                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicXYZEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicZYXEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.Extensions.ToAxisAngleFromExtrinsicZYXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.Extensions.ToAxisAngleFromIntrinsicYZXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdaf1 = new Vector4D(3, 3, 3, 3);
                        var sdaf2 = new Vector4D(7, 17, 27, 37);
                        var sdaf = sdaf1 + sdaf2;
                        Console.WriteLine(sdaf);
                    }
                    aaaa();
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensionsD.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensions.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var m = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(0.9F, -1.3F, 0.6F);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());

                }




            }
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
