using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using UltimateOrb.Runtime.Intrinsics;

namespace UltimateOrb.Runtime.Intrinsics {

    public static partial class IntrinsicExtensions {

        public static Vector256<double> Sum(Vector256<double> value) {
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                var t0 = Vector256.Create(value.GetElement(0));
                var t1 = Vector256.Create(value.GetElement(1));
                var t2 = Vector256.Create(value.GetElement(2));
                var t3 = Vector256.Create(value.GetElement(3));
                return System.Runtime.Intrinsics.X86.Avx.Add(
                    System.Runtime.Intrinsics.X86.Avx.Add(t0, t1),
                    System.Runtime.Intrinsics.X86.Avx.Add(t2, t3));
            } else {
                return Vector256.Create(value.GetElement(0) + value.GetElement(1) + value.GetElement(2) + value.GetElement(3));
            }
        }

        public static double DotProductScalar(Vector256<double> first, Vector256<double> second) {
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                return DotProduct(first, second).ToScalar();
            } else {
                return
                    first.GetElement(0) * second.GetElement(0) +
                    first.GetElement(1) * second.GetElement(1) +
                    first.GetElement(2) * second.GetElement(2) +
                    first.GetElement(3) * second.GetElement(3);
            }
        }

        public static Vector256<double> DotProduct(Vector256<double> first, Vector256<double> second) {
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                return Sum(System.Runtime.Intrinsics.X86.Avx.Multiply(first, second));
            } else {
                return Vector256.Create(DotProductScalar(first, second));
            }
        }

        public static Vector256<double> Add(Vector256<double> first, Vector256<double> second) {
            return System.Runtime.Intrinsics.X86.Avx.IsSupported ?
                System.Runtime.Intrinsics.X86.Avx.Add(first, second) : System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported ?
                Vector256.Create(
                    System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Add(
                        first.GetLower(),
                        second.GetLower()),
                    System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Add(
                        first.GetUpper(),
                        second.GetUpper())) :
                Vector256.Create(
                    first.GetElement(0) + second.GetElement(0),
                    first.GetElement(1) + second.GetElement(1),
                    first.GetElement(2) + second.GetElement(2),
                    first.GetElement(3) + second.GetElement(3));
        }

        public static Vector256<double> Scale(this Vector256<double> value, double scale) {
            return System.Runtime.Intrinsics.X86.Avx.IsSupported ?
                System.Runtime.Intrinsics.X86.Avx.Multiply(value, Vector256.Create(scale)) : System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported ?
                Vector256.Create(
                    System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Multiply(
                        value.GetLower(),
                        Vector128.Create(scale)),
                    System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Multiply(
                        value.GetUpper(),
                        Vector128.Create(scale))) :
                Vector256.Create(
                    value.GetElement(0) * scale,
                    value.GetElement(1) * scale,
                    value.GetElement(2) * scale,
                    value.GetElement(3) * scale);
        }
    }

    public readonly struct Vector4D {

        readonly Vector256<double> v;

        public ReadOnlySpan<double> AsSpan() {
            unsafe {
                return new ReadOnlySpan<double>(Unsafe.AsPointer(ref Unsafe.AsRef(in this)), Unsafe.SizeOf<Vector4D>() / Unsafe.SizeOf<double>());
            }
        }

        public Vector4D(Vector256<double> v) {
            this.v = v;
        }

        public Vector4D(double e0, double e1, double e2, double e3) {
            this.v = Vector256.Create(e0, e1, e2, e3);
        }

        public Vector4D(ReadOnlySpan<double> v) {
            if (4 > v.Length) {
                throw new ArgumentException("The span must have 4 elements at least.", nameof(v));
            }
            unsafe {
                this.v = System.Runtime.Intrinsics.X86.Avx.IsSupported ?
                    System.Runtime.Intrinsics.X86.Avx.LoadVector256((double*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(v))) :
                    Vector256.Create(v[0], v[1], v[2], v[3]);
            }
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D operator +(Vector4D first, Vector4D second) {
            return new Vector4D(IntrinsicExtensions.Add(first.v, second.v));
        }

        double E0 {

            get => v.GetElement(0);
        }

        double E1 {

            get => v.GetElement(1);
        }

        double E2 {

            get => v.GetElement(2);
        }

        double E3 {

            get => v.GetElement(3);
        }

        public override string ToString() {
            return $@"[ {E0:R}, {E1:R}, {E2:R}, {E3:R} ]";
        }
    }
}


namespace UltimateOrb.Numerics {

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4D {

        public double E0;

        public double E1;

        public double E2;

        public double E3;

        public double GetLengthSquared() {
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                unsafe {
                    fixed (double* p = &E0) {
                        var t = System.Runtime.Intrinsics.X86.Avx.LoadVector256(p);
                        t = System.Runtime.Intrinsics.X86.Avx.Multiply(t, t);
                        t = System.Runtime.Intrinsics.X86.Avx.HorizontalAdd(t, t);
                        return t.GetElement(0) + t.GetElement(2);
                    }
                }
            } else {
                return Math.FusedMultiplyAdd(E3, E3, Math.FusedMultiplyAdd(E2, E2, Math.FusedMultiplyAdd(E1, E1, E0 * E0)));
            }
        }

        public Span<double> AsSpan() {
            unsafe {
                return new Span<double>(Unsafe.AsPointer(ref this), Unsafe.SizeOf<Vector4D>() / Unsafe.SizeOf<double>());
            }
        }


        public ref Vector3D E012 {

            get {
                unsafe {
                    return ref Unsafe.AsRef<Vector3D>(Unsafe.AsPointer(ref this));
                }
            }
        }

        public Vector4D(double e0, double e1, double e2, double e3) {
            this.E0 = e0;
            this.E1 = e1;
            this.E2 = e2;
            this.E3 = e3;
        }

        public Vector4D(ReadOnlySpan<double> v) {
            if (4 > v.Length) {
                throw new ArgumentException("", nameof(v));
            }
            Miscellaneous.IgnoreOutParameter(out this);
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                unsafe {
                    fixed (double* dst = &E0)
                    fixed (double* src = &v[0]) {
                        System.Runtime.Intrinsics.X86.Avx.Store(dst, System.Runtime.Intrinsics.X86.Avx.LoadVector256(src));
                    }
                }
            } else {
                this = Unsafe.As<double, Vector4D>(ref MemoryMarshal.GetReference(v));
            }
        }

        public Vector4D(Vector256<double> v) {
            Miscellaneous.IgnoreOutParameter(out this);
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                unsafe {
                    fixed (double* dst = &E0) {
                        System.Runtime.Intrinsics.X86.Avx.Store(dst, v);
                    }
                }
            } else {
                this = Unsafe.As<Vector256<double>, Vector4D>(ref v);
            }
        }

        public Vector4D(Vector3D e012, double e3) {
            Miscellaneous.IgnoreOutParameter(out this);
            this.E012 = e012;
            this.E3 = e3;
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D operator +(Vector4D first, Vector4D second) {
            Miscellaneous.IgnoreOutParameter(out Vector4D result);
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                unsafe {
                    System.Runtime.Intrinsics.X86.Avx.Store(&result.E0,
                        System.Runtime.Intrinsics.X86.Avx.Add(
                            System.Runtime.Intrinsics.X86.Avx.LoadVector256(&first.E0),
                            System.Runtime.Intrinsics.X86.Avx.LoadVector256(&second.E0)));
                }
            } else if (System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.IsSupported) {
                unsafe {
                    System.Runtime.Intrinsics.Arm.AdvSimd.Store(&result.E0,
                        System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Add(
                            System.Runtime.Intrinsics.Arm.AdvSimd.LoadVector128(&first.E0),
                            System.Runtime.Intrinsics.Arm.AdvSimd.LoadVector128(&second.E0)));
                    System.Runtime.Intrinsics.Arm.AdvSimd.Store(&result.E2,
                        System.Runtime.Intrinsics.Arm.AdvSimd.Arm64.Add(
                            System.Runtime.Intrinsics.Arm.AdvSimd.LoadVector128(&first.E2),
                            System.Runtime.Intrinsics.Arm.AdvSimd.LoadVector128(&second.E2)));
                }
            } else {
                result.E0 = first.E0 + second.E0;
                result.E1 = first.E1 + second.E1;
                result.E2 = first.E2 + second.E2;
                result.E3 = first.E3 + second.E3;
            }
            return result;
        }

        public override string ToString() {
            return $@"[ {E0:R}, {E1:R}, {E2:R}, {E3:R} ]";
        }

        public Vector4D GetNormalized() {
            throw new NotImplementedException();
        }

        Vector256<double> ToIntrinsic() {
            if (System.Runtime.Intrinsics.X86.Avx.IsSupported) {
                unsafe {
                    fixed (double* p = &E0) {
                        return System.Runtime.Intrinsics.X86.Avx.LoadVector256(p);
                    }
                }
            }
            return Vector256.Create(E0, E1, E2, E3);
        }

        public static double DotProduct(Vector4D first, Vector4D second) {
            return IntrinsicExtensions.DotProductScalar(first.ToIntrinsic(), second.ToIntrinsic());
        }


        public static Vector4D operator *(Vector4D first, double second) {
            return new Vector4D(IntrinsicExtensions.Scale(first.ToIntrinsic(), second));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3D {

        public double E0;

        public double E1;

        public double E2;

        public Span<double> AsSpan() {
            unsafe {
                return new Span<double>(Unsafe.AsPointer(ref this), Unsafe.SizeOf<Vector4D>() / Unsafe.SizeOf<double>());
            }
        }

        public Vector3D(double e0, double e1, double e2) {
            this.E0 = e0;
            this.E1 = e1;
            this.E2 = e2;
        }

        public Vector3D(ReadOnlySpan<double> v) {
            if (3 > v.Length) {
                throw new ArgumentException("", nameof(v));
            }
            Miscellaneous.IgnoreOutParameter(out this);
            this = Unsafe.As<double, Vector3D>(ref MemoryMarshal.GetReference(v));
        }

        public double GetLengthSquared() {
            if (System.Runtime.Intrinsics.X86.Sse3.IsSupported) {
                unsafe {
                    fixed (double* p = &E0) {
                        var t = System.Runtime.Intrinsics.X86.Sse2.LoadVector128(p);
                        t = System.Runtime.Intrinsics.X86.Sse2.Multiply(t, t);
                        return Math.FusedMultiplyAdd(E2, E2, System.Runtime.Intrinsics.X86.Sse3.HorizontalAdd(t, t).ToScalar());
                    }
                }
            } else {
                return Math.FusedMultiplyAdd(E2, E2, Math.FusedMultiplyAdd(E1, E1, E0 * E0));
            }
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector3D operator +(Vector3D first, Vector3D second) {
            Unsafe.SkipInit(out Vector3D result);
            result.E0 = first.E0 + second.E0;
            result.E1 = first.E1 + second.E1;
            result.E2 = first.E2 + second.E2;
            return result;
        }

        public override string ToString() {
            return $@"[ {E0:R}, {E1:R}, {E2:R} ]";
        }

        public Vector3D GetNormalized() {
            var e0 = this.E0;
            var e1 = this.E1;
            var e2 = this.E2;
            return this / Math.Sqrt(e0 * e0 + e1 * e1 + e2 * e2);
        }

        public Vector3D GetNormalizedSafe() {
            var e0 = this.E0;
            var e1 = this.E1;
            var e2 = this.E2;
            NormalizeSafe(e0, e1, e2, out e0, out e1, out e2);
            return new Vector3D(e0, e1, e2);
        }

        internal static void NormalizeSafe(
            double e0, double e1, double e2,
            out double r0, out double r1, out double r2) {
            System.Diagnostics.Debug.Assert(double.IsFinite(e0));
            System.Diagnostics.Debug.Assert(double.IsFinite(e1));
            System.Diagnostics.Debug.Assert(double.IsFinite(e2));
            var b0 = Math.ILogB(e0);
            var b1 = Math.ILogB(e1);
            var b2 = Math.ILogB(e2);
            var b = 510 - Math.Max(b0, Math.Max(b1, b2));
            e0 = Math.ScaleB(e0, b);
            e1 = Math.ScaleB(e1, b);
            e2 = Math.ScaleB(e2, b);
            var s = Math.Sqrt(e0 * e0 + e1 * e1 + e2 * e2);
            r0 = e0 / s;
            r1 = e1 / s;
            r2 = e2 / s;
        }

        public static Vector3D operator *(Vector3D first, double scalar) {
            return new Vector3D(first.E0 * scalar, first.E1 * scalar, first.E2 * scalar);
        }

        public static Vector3D operator /(Vector3D first, double second) {
            return new Vector3D(first.E0 / second, first.E1 / second, first.E2 / second);
        }
    }


    public struct QuaternionD {

        public Vector4D XYZW;

        public static QuaternionD Identity {

            get => new QuaternionD(0, 0, 0, 1);
        }

        ref QuaternionD @this {

            get {
                unsafe {
                    return ref Unsafe.AsRef<QuaternionD>(Unsafe.AsPointer(ref this));
                }
            }
        }

        public ref double X {

            get => ref @this.XYZW.E0;
        }

        public ref double Y {

            get => ref @this.XYZW.E1;
        }

        public ref double Z {

            get => ref @this.XYZW.E2;
        }

        public ref double W {

            get => ref @this.XYZW.E3;
        }

        QuaternionD(Vector4D xyzw) {
            this.XYZW = xyzw;
        }

        public QuaternionD(double x, double y, double z, double w) : this(new Vector4D(x, y, z, w)) {
        }

        public QuaternionD(Vector3D xyz, double w) : this(new Vector4D(xyz, w)) {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(QuaternionD first, QuaternionD second) {
            return new QuaternionD(first.XYZW + second.XYZW);
        }

        public QuaternionD GetNormalized() {
            return new QuaternionD(this.XYZW.GetNormalized());
        }
    }
    /*
    public struct AxisAngleD {

        public Vector4D XYZW;

        AxisAngleD(Vector4D v) {
            this.XYZW = v;
        }

        public AxisAngleD(Vector3D axis, double angle) {
            Unsafe.SkipInit(out this.XYZW);
            Unsafe.As<Vector4D, Vector3D>(ref this.XYZW) = axis;
            this.XYZW.E3 = angle;
        }
    }
    */

    public struct Matrix4x4D {

        public Vector4D Row0;

        public Vector4D Row1;

        public Vector4D Row2;

        public Vector4D Row3;

        public Matrix4x4D(Vector4D r0, Vector4D r1, Vector4D r2, Vector4D r3) {
            this.Row0 = r0;
            this.Row1 = r1;
            this.Row2 = r2;
            this.Row3 = r3;
        }

        ref Matrix4x4D @this {

            get {
                unsafe {
                    fixed (Matrix4x4D* ptr = &this) {
                        return ref Unsafe.AsRef<Matrix4x4D>(ptr);
                    }
                }
            }
        }

        public ref double E00 {

            get => ref @this.Row0.E0;
        }

        public ref double E01 {

            get => ref @this.Row0.E1;
        }

        public ref double E02 {

            get => ref @this.Row0.E2;
        }

        public ref double E03 {

            get => ref @this.Row0.E3;
        }

        public ref double E10 {

            get => ref @this.Row1.E0;
        }

        public ref double E11 {

            get => ref @this.Row1.E1;
        }

        public ref double E12 {

            get => ref @this.Row1.E2;
        }

        public ref double E13 {

            get => ref @this.Row1.E3;
        }

        public ref double E20 {

            get => ref @this.Row2.E0;
        }

        public ref double E21 {

            get => ref @this.Row2.E1;
        }

        public ref double E22 {

            get => ref @this.Row2.E2;
        }

        public ref double E23 {

            get => ref @this.Row2.E3;
        }

        public ref double E30 {

            get => ref @this.Row3.E0;
        }

        public ref double E31 {

            get => ref @this.Row3.E1;
        }

        public ref double E32 {

            get => ref @this.Row3.E2;
        }

        public ref double E33 {

            get => ref @this.Row3.E3;
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix4x4D operator +(Matrix4x4D first, Matrix4x4D second) {
            Miscellaneous.IgnoreOutParameter(out Matrix4x4D result);
            result.Row0 = first.Row0 + second.Row0;
            result.Row1 = first.Row1 + second.Row1;
            result.Row2 = first.Row2 + second.Row2;
            result.Row3 = first.Row3 + second.Row3;
            return result;
        }
    }

    public static partial class Extensions {

        public static void ToMatrixFromAxisAngle(
            double x, double y, double z, double angle,
            out double e00, out double e01, out double e02,
            out double e10, out double e11, out double e12,
            out double e20, out double e21, out double e22) {
            Vector3D.NormalizeSafe(x, y, z,
                out var xn, out var yn, out var zn);
            var (sa, ca) = Math.SinCos(angle);
            var xx = xn * xn;
            var yy = yn * yn;
            var zz = zn * zn;
            var xy = xn * yn;
            var xz = xn * zn;
            var yz = yn * zn;
            e00 = xx + ca * (1.0D - xx);
            e01 = xy - ca * xy + sa * zn;
            e02 = xz - ca * xz - sa * yn;
            e10 = xy - ca * xy - sa * zn;
            e11 = yy + ca * (1.0D - yy);
            e12 = yz - ca * yz + sa * xn;
            e20 = xz - ca * xz + sa * yn;
            e21 = yz - ca * yz - sa * xn;
            e22 = zz + ca * (1.0D - zz);
        }

        [SkipLocalsInit]
        public static void ToExtrinsicXYZEulerAnglesFromAxisAngle(
            double x, double y, double z, double angle,
            out double angle0, out double angle1, out double angle2) {
            Unsafe.SkipInit(out double xn);
            Unsafe.SkipInit(out double yn);
            Unsafe.SkipInit(out double zn);
            if (0.0D != angle) {
                Vector3D.NormalizeSafe(x, y, z,
                    out xn, out yn, out zn);
            }
            var (sa, ca) = Math.SinCos(angle);
            var xx = xn * xn;
            var yy = yn * yn;
            var zz = zn * zn;
            var xy = xn * yn;
            var xz = xn * zn;
            var yz = yn * zn;
            var e00 = xx + ca * (1.0D - xx);
            var e01 = xy - ca * xy + sa * zn;
            var e02 = xz - ca * xz - sa * yn;
            var e10 = xy - ca * xy - sa * zn;
            var e11 = yy + ca * (1.0D - yy);
            var e12 = yz - ca * yz + sa * xn;
            var e20 = xz - ca * xz + sa * yn;
            var e21 = yz - ca * yz - sa * xn;
            var e22 = zz + ca * (1.0D - zz);
            angle0 = Math.Atan2(e12, e22);
            angle1 = Math.Asin(-e02);
            angle2 = Math.Atan2(e01, e00);
        }

        public static void ToIntrinsicXYZEulerAnglesFromAxisAngle(
            double x, double y, double z, double angle,
            out double angle0, out double angle1, out double angle2) {
            Unsafe.SkipInit(out double xn);
            Unsafe.SkipInit(out double yn);
            Unsafe.SkipInit(out double zn);
            if (0.0D != angle) {
                Vector3D.NormalizeSafe(x, y, z,
                    out xn, out yn, out zn);
            }
            var (sa, ca) = Math.SinCos(angle);
            var xx = xn * xn;
            var yy = yn * yn;
            var zz = zn * zn;
            var xy = xn * yn;
            var xz = xn * zn;
            var yz = yn * zn;
            var e00 = xx + ca * (1.0D - xx);
            var e01 = xy - ca * xy + sa * zn;
            var e02 = xz - ca * xz - sa * yn;
            var e10 = xy - ca * xy - sa * zn;
            var e11 = yy + ca * (1.0D - yy);
            var e12 = yz - ca * yz + sa * xn;
            var e20 = xz - ca * xz + sa * yn;
            var e21 = yz - ca * yz - sa * xn;
            var e22 = zz + ca * (1.0D - zz);
            angle0 = Math.Atan2(-e21, e22);
            angle1 = Math.Asin(e20);
            angle2 = Math.Atan2(-e10, e00);
        }

        /* // incorrect
        public static void ToIntrinsicYZXEulerAnglesFromAxisAngle(
            double x, double y, double z, double angle,
            out double angle0, out double angle1, out double angle2) {
            Vector3D.NormalizeSafe(x, y, z,
                out var xn, out var yn, out var zn);
            var (s, c) = Math.SinCos(angle);
            var t = 1.0 - c;
            var xx = xn * xn;
            var yy = yn * yn;
            var u = xn * yn * t + zn * s;
            var ua = Math.Abs(u);
            {
                var zz = zn * zn;
                var zt = zn * t;
                angle0 = Math.Atan2(yn * s - xn * zt, xx + (yy + zz) * c);
                angle1 = Math.Asin(Math.Clamp(u, -1.0, 1.0));
                angle2 = Math.Atan2(xn * s - yn * zt, yy + (xx + zz) * c);
            }
        }
        
        public static void ToIntrinsicYZXEulerAnglesFromAxisAngle1(
            double x, double y, double z, double angle,
            out double angle0, out double angle1, out double angle2) {
            var (s, c) = Math.SinCos(angle);
            var t = 1.0 - c;
            var xx = x * x;
            var yy = y * y;
            var n = xx + yy + z * z;
            s *= Math.Sqrt(n);
            var u = (x * y * t + z * s) / n;
            var ua = Math.Abs(u);
            if (ua > 0.999) {
                var (s2, c2) = Math.SinCos(angle * 0.5);
                angle0 = Math.CopySign(2.0, u) * Math.Atan2(x * s2, c2);
                angle1 = Math.Asin(Math.Clamp(u, -1.0, 1.0));
                angle2 = 0;
            } else {
                var zz = z * z;
                var zt = z * t;
                angle0 = Math.Atan2(y * s - x * zt, xx + (yy + zz) * c);
                angle1 = Math.Asin(u);
                angle2 = Math.Atan2(x * s - y * zt, yy + (xx + zz) * c);
            }
        }
        */


        [SkipLocalsInit]
        public static Vector4D ToAxisAngleFromRotationMatrix(in this Matrix4x4D rotation, bool normalize = false) {
            var trace = rotation.E00 + rotation.E11 + rotation.E22;
            Unsafe.SkipInit(out Vector4D result);
            if (trace > 0.0D) {
                var s = Math.Sqrt(1.0D + trace);
                result.E0 = rotation.E12 - rotation.E21;
                result.E1 = rotation.E20 - rotation.E02;
                result.E2 = rotation.E01 - rotation.E10;
                result.E3 = 2.0D * Math.Acos(s * 0.5D);
            } else {
                if (rotation.E00 >= rotation.E11 && rotation.E00 >= rotation.E22) {
                    var s = Math.Sqrt(rotation.E00 - rotation.E11 - rotation.E22 + 1.0D);
                    var invS = 0.5D / s;
                    result.E0 = 0.5D * s;
                    result.E1 = (rotation.E01 + rotation.E10) * invS;
                    result.E2 = (rotation.E02 + rotation.E20) * invS;
                    result.E3 = 2.0D * Math.Acos((rotation.E12 - rotation.E21) * invS);
                } else if (rotation.E11 > rotation.E22) {
                    var s = Math.Sqrt(rotation.E11 - rotation.E00 - rotation.E22 + 1.0D);
                    var invS = 0.5D / s;
                    result.E0 = (rotation.E10 + rotation.E01) * invS;
                    result.E1 = 0.5D * s;
                    result.E2 = (rotation.E21 + rotation.E12) * invS;
                    result.E3 = 2.0D * Math.Acos((rotation.E20 - rotation.E02) * invS);
                } else {
                    var s = Math.Sqrt(rotation.E22 - rotation.E00 - rotation.E11 + 1.0D);
                    var invS = 0.5D / s;
                    result.E0 = (rotation.E20 + rotation.E02) * invS;
                    result.E1 = (rotation.E21 + rotation.E12) * invS;
                    result.E2 = 0.5D * s;
                    result.E3 = 2.0D * Math.Acos((rotation.E01 - rotation.E10) * invS);
                }
            }
            return result;
        }

        [SkipLocalsInit]
        public static QuaternionD ToQuaternionFromRotationMatrix(in this Matrix4x4D rotation, bool normalize = false) {
            var trace = rotation.E00 + rotation.E11 + rotation.E22;
            Unsafe.SkipInit(out QuaternionD result);
            if (trace > 0.0D) {
                var s = Math.Sqrt(1.0D + trace);
                result.W = s * 0.5D;
                s = 0.5f / s;
                result.X = (rotation.E12 - rotation.E21) * s;
                result.Y = (rotation.E20 - rotation.E02) * s;
                result.Z = (rotation.E01 - rotation.E10) * s;
            } else {
                if (rotation.E00 >= rotation.E11 && rotation.E00 >= rotation.E22) {
                    var s = Math.Sqrt(rotation.E00 - rotation.E11 - rotation.E22 + 1.0D);
                    var invS = 0.5D / s;
                    result.X = 0.5D * s;
                    result.Y = (rotation.E01 + rotation.E10) * invS;
                    result.Z = (rotation.E02 + rotation.E20) * invS;
                    result.W = (rotation.E12 - rotation.E21) * invS;
                } else if (rotation.E11 > rotation.E22) {
                    var s = Math.Sqrt(rotation.E11 - rotation.E00 - rotation.E22 + 1.0D);
                    var invS = 0.5D / s;
                    result.X = (rotation.E10 + rotation.E01) * invS;
                    result.Y = 0.5D * s;
                    result.Z = (rotation.E21 + rotation.E12) * invS;
                    result.W = (rotation.E20 - rotation.E02) * invS;
                } else {
                    var s = Math.Sqrt(rotation.E22 - rotation.E00 - rotation.E11 + 1.0D);
                    var invS = 0.5D / s;
                    result.X = (rotation.E20 + rotation.E02) * invS;
                    result.Y = (rotation.E21 + rotation.E12) * invS;
                    result.Z = 0.5D * s;
                    result.W = (rotation.E01 - rotation.E10) * invS;
                }
            }
            return result;
        }

        public static Vector4D ToAxisAngleFromQuaternion(in this QuaternionD rotation, bool normalize = false) {
            var result = rotation.XYZW;
            result.E3 = 2.0 * Math.Acos(result.E3);
            if (normalize) {
                result.E012 = result.E012.GetNormalizedSafe();
            }
            return result;
        }

        public static QuaternionD ToQuaternionFromAxisAngle(in this Vector4D axisAngle, bool axisNormalized = false) {
            var angle = axisAngle.E3;
            var (s, c) = Math.SinCos(0.5 * angle);
            return new QuaternionD((axisNormalized ? axisAngle.E012 : axisAngle.E012.GetNormalizedSafe()) * s, c);
        }

        public static QuaternionD ToQuaternionFromAxisAngle(in this Vector3D axisAngle, double angle, bool axisNormalized = false) {
            var (s, c) = Math.SinCos(0.5 * angle);
            return new QuaternionD((axisNormalized ? axisAngle : axisAngle.GetNormalizedSafe()) * s, c);
        }

        public static Vector3D ToExtrinsicZXYEulerAnglesFromXYZ(in this Vector3D angles) {
            var (s0, c0) = Math.SinCos(angles.E0);
            var (s1, c1) = Math.SinCos(angles.E1);
            var (s2, c2) = Math.SinCos(angles.E2);
            var s2s0 = s2 * s0;
            var c2c0 = c2 * c0;
            return new Vector3D(
                Math.Atan2(c2c0 + s2s0 * s1, c1 * s2),
                Math.Asin(c2 * s0 - c0 * s1 * s2),
                Math.Atan2(c0 * c1, c2c0 * s1 + s2s0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
            double angle0, double angle1, double angle2,
            out double x, out double y, out double z, out double angle) {
            var (s0, c0) = Math.SinCos(angle0 * 0.5);
            var (s1, c1) = Math.SinCos(angle1 * 0.5);
            var (s2, c2) = Math.SinCos(angle2 * 0.5);
            var c0s1 = c0 * s1;
            var s0c1 = s0 * c1;
            x = s0c1 * c2 - c0s1 * s2;
            y = c0s1 * c2 + s0c1 * s2;
            var c0c1 = c0 * c1;
            var s0s1 = s0 * s1;
            z = c0c1 * s2 - s0s1 * c2;
            var w = c0c1 * c2 + s0s1 * s2;
            angle = 2.0 * Math.Acos(w);
        }

        /*
        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicXYZEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, -angle2,
                out result.E0, out result.E1, out result.E2, out result.E3);
            result.E2 = -result.E2;
            return result;
        }
        */
        // Manually inlined to gain performance.
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
            double angle0, double angle1, double angle2,
            out double x, out double y, out double z, out double angle) {
            var (s0, c0) = Math.SinCos(angle0 * 0.5);
            var (s1, c1) = Math.SinCos(angle1 * 0.5);
            var (s2, c2) = Math.SinCos(angle2 * 0.5);
            var c0s1 = c0 * s1;
            var s0c1 = s0 * c1;
            x = s0c1 * c2 + c0s1 * s2;
            y = c0s1 * c2 - s0c1 * s2;
            var c0c1 = c0 * c1;
            var s0s1 = s0 * s1;
            z = c0c1 * s2 + s0s1 * c2;
            var w = c0c1 * c2 - s0s1 * s2;
            angle = 2.0 * Math.Acos(w);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicXYZEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E0, out result.E1, out result.E2, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicXYZEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicXYZEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicXYZEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E0, out result.E1, out result.E2, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicXYZEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicXYZEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicXZYEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E0, out result.E2, out result.E1, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicXZYEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicXZYEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicXZYEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E0, out result.E2, out result.E1, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicXZYEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicXZYEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicYXZEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E1, out result.E0, out result.E2, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicYXZEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicYXZEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicYXZEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E1, out result.E0, out result.E2, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicYXZEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicYXZEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicYZXEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E1, out result.E2, out result.E0, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicYZXEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicYZXEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicYZXEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E1, out result.E2, out result.E0, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicYZXEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicYZXEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicZXYEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E2, out result.E0, out result.E1, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicZXYEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicZXYEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicZXYEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E2, out result.E0, out result.E1, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicZXYEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicZXYEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicZYXEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromIntrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E2, out result.E1, out result.E0, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromExtrinsicZYXEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromExtrinsicZYXEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicZYXEulerAngles(double angle0, double angle1, double angle2) {
            Unsafe.SkipInit(out Vector4D result);
            ToAxisAngleFromExtrinsicXYZEulerAnglesInternal(
                angle0, angle1, angle2,
                out result.E2, out result.E1, out result.E0, out result.E3);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Vector4D ToAxisAngleFromIntrinsicZYXEulerAngles(this Vector3D angles) {
            return ToAxisAngleFromIntrinsicZYXEulerAngles(angles.E0, angles.E1, angles.E2);
        }

        [SkipLocalsInit]
        public static QuaternionD ToQuaternionFromAxisAngle(this Vector4D axisAngle) {
            if (axisAngle.E3 == 0.0D) {
                return QuaternionD.Identity;
            }
            Unsafe.SkipInit(out QuaternionD result);

            var t = axisAngle.E3 * 0.5D;
            var (s, c) = Math.SinCos(t);

            result.XYZW.E012 = axisAngle.E012.GetNormalized() * s;
            result.XYZW.E3 = c;

            return result.GetNormalized();
        }

        public static System.Numerics.Matrix4x4 ToStandardF(this Matrix4x4D v) {
            return new System.Numerics.Matrix4x4(
                v.E00.ToStandardF(), v.E01.ToStandardF(), v.E02.ToStandardF(), v.E03.ToStandardF(),
                v.E10.ToStandardF(), v.E11.ToStandardF(), v.E12.ToStandardF(), v.E13.ToStandardF(),
                v.E20.ToStandardF(), v.E21.ToStandardF(), v.E22.ToStandardF(), v.E23.ToStandardF(),
                v.E30.ToStandardF(), v.E31.ToStandardF(), v.E32.ToStandardF(), v.E33.ToStandardF());
        }


        public static System.Numerics.Vector3 ToStandardF(this Vector3D v) {
            return new System.Numerics.Vector3(v.E0.ToStandardF(), v.E1.ToStandardF(), v.E2.ToStandardF());
        }

        public static System.Numerics.Vector4 ToStandardF(this Vector4D v) {
            return new System.Numerics.Vector4(v.E0.ToStandardF(), v.E1.ToStandardF(), v.E2.ToStandardF(), v.E3.ToStandardF());
        }

        public static System.Numerics.Quaternion ToStandardF(this QuaternionD q) {
            return new System.Numerics.Quaternion(q.XYZW.E0.ToStandardF(), q.XYZW.E1.ToStandardF(), q.XYZW.E2.ToStandardF(), q.XYZW.E3.ToStandardF());
        }

        public static float ToStandardF(this double v) {
            return (float)v;
        }

        public static float ToStandardF(this float v) {
            return v;
        }
    }

    [Discardable()]
    public static partial class StandardExtensionsD {





        public static void ExtractRotationIntrinsicYXZ(this Quaternion r, out float yaw, out float pitch, out float roll) {
            double x = r.X;
            double y = r.Y;
            double z = r.Z;
            double w = r.W;
            yaw = (float)Math.Atan2(2.0 * (y * w + x * z), 1.0 - 2.0 * (x * x + y * y));
            pitch = (float)Math.Asin(2.0 * (x * w - y * z));
            roll = (float)Math.Atan2(2.0 * (x * y + z * w), 1.0 - 2.0 * (x * x + z * z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Matrix4x4 ToRotationMatrixExtrinsicXYZ(this Vector3 angles) {
            double r = angles.X;
            double p = angles.Y;
            double y = angles.Z;
            var (rs, rc) = Math.SinCos(r);
            var (ps, pc) = Math.SinCos(p);
            var (ys, yc) = Math.SinCos(y);
            return new Matrix4x4((float)(yc * pc), (float)(yc * ps * rs - ys * rc), (float)(yc * ps * rc + ys * rs), 0.0F,
                (float)(ys * pc), (float)(ys * ps * rs + yc * rc), (float)(ys * ps * rc - yc * rs), 0.0F,
                (float)(-ps), (float)(pc * rs), (float)(pc * rc), 0.0F,
                0.0F, 0.0F, 0.0F, 1.0F);
        }

        [Obsolete]
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void ToRotationMatrixExtrinsicXYZ(this ReadOnlySpan<double> angles, Span<double> matrix4x4RowMajor) {
            if (3 > angles.Length) {
                throw new ArgumentException(nameof(angles));
            }
            if (16 > matrix4x4RowMajor.Length) {
                throw new ArgumentException(nameof(matrix4x4RowMajor));
            }
            var (rs, rc) = Math.SinCos(angles[0]);
            var (ps, pc) = Math.SinCos(angles[1]);
            var (ys, yc) = Math.SinCos(angles[2]);
            matrix4x4RowMajor[0] = yc * pc;
            matrix4x4RowMajor[1] = yc * ps * rs - ys * rc;
            matrix4x4RowMajor[2] = yc * ps * rc + ys * rs;
            matrix4x4RowMajor[3] = 0.0;
            matrix4x4RowMajor[4] = ys * pc;
            matrix4x4RowMajor[5] = ys * ps * rs + yc * rc;
            matrix4x4RowMajor[6] = ys * ps * rc - yc * rs;
            matrix4x4RowMajor[7] = 0.0;
            matrix4x4RowMajor[8] = -ps;
            matrix4x4RowMajor[9] = pc * rs;
            matrix4x4RowMajor[10] = pc * rc;
            matrix4x4RowMajor[11] = 0.0;
            matrix4x4RowMajor[12] = 0.0;
            matrix4x4RowMajor[13] = 0.0;
            matrix4x4RowMajor[14] = 0.0;
            matrix4x4RowMajor[15] = 1.0;
        }
    }

    [Discardable()]
    public static partial class StandardExtensions {

        public static void ExtractRotationIntrinsicYXZ(this Quaternion r, out float yaw, out float pitch, out float roll) {
            yaw = MathF.Atan2(2.0f * (r.Y * r.W + r.X * r.Z), 1.0f - 2.0f * (r.X * r.X + r.Y * r.Y));
            pitch = MathF.Asin(2.0f * (r.X * r.W - r.Y * r.Z));
            roll = MathF.Atan2(2.0f * (r.X * r.Y + r.Z * r.W), 1.0f - 2.0f * (r.X * r.X + r.Z * r.Z));
        }

        public static Vector3 ExtractRotationIntrinsicYXZ(this Quaternion r) {
            Vector3 result;
            StandardExtensionsD.ExtractRotationIntrinsicYXZ(r, out result.X, out result.Y, out result.Z);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static Matrix4x4 ToRotationMatrixExtrinsicXYZ(this Vector3 angles) {
            var r = angles.X;
            var p = angles.Y;
            var y = angles.Z;
            var (rs, rc) = MathF.SinCos(r);
            var (ps, pc) = MathF.SinCos(p);
            var (ys, yc) = MathF.SinCos(y);
            return new Matrix4x4(yc * pc, yc * ps * rs - ys * rc, yc * ps * rc + ys * rs, 0.0F,
                ys * pc, ys * ps * rs + yc * rc, ys * ps * rc - yc * rs, 0.0F,
                -ps, pc * rs, pc * rc, 0.0F,
                0.0F, 0.0F, 0.0F, 1.0F);
        }
    }
}
