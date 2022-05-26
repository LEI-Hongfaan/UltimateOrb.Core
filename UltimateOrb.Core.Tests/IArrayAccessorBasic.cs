
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Buffers;
using UltimateOrb.Functional.CommonDelegates;
using UltimateOrb.Utilities;
using NUnit.Sdk;

namespace UltimateOrb {



    public readonly struct StructuredBufferAccessor<T> :
        IMemorySpan,
        IStructuredMemorySpan
        where T : unmanaged {

        internal readonly MemorySpan<byte> _Buffer;

        internal readonly int _ByteStride;

        public object? Manager {

            get => _Buffer.Manager;
        }

        public IntPtr ByteOffset {

            get => _Buffer.ByteOffset;
        }

        public int Count {

            get => _Buffer.Count;
        }

        public int ByteStride {

            get => _ByteStride;
        }

        public ref T this[int index] {

            get {
                var count = _Buffer.Count;
                if (unchecked((uint)count) > unchecked((uint)index)) {
                    var offset = _ByteStride * index;
                    return ref Unsafe.As<byte, T>(ref Unsafe.AsRef(in _Buffer.Span[offset]));
                }
                _ = checked(unchecked((uint)count) - unchecked((uint)index));
                throw null;
            }
        }



        public StructuredBufferAccessor(T[] array) {
            _ByteStride = CilVerifiable.SizeOf<T>();
            // Always write Count field lastly.
            _Buffer = array.AsMemorySpan().AsBytes();
        }

    }

    /// <summary>
    /// Piece-wise Linear Interpolation.
    /// </summary>
    /// <remarks>Supports both differentiation and integration.</remarks>
    public class LinearSpline {


        readonly StructuredBufferAccessor<double> _x;
        readonly StructuredBufferAccessor<double> _c0;
        readonly StructuredBufferAccessor<double> _c1;
        readonly Lazy<double[]> _indefiniteIntegral;

        /// <param name="x">Sample points (N+1), sorted ascending</param>
        /// <param name="c0">Sample values (N or N+1) at the corresponding points; intercept, zero order coefficients</param>
        /// <param name="c1">Slopes (N) at the sample points (first order coefficients): N</param>
        public LinearSpline(StructuredBufferAccessor<double> x, StructuredBufferAccessor<double> c0, StructuredBufferAccessor<double> c1) {
            if ((x.Count != c0.Count + 1 && x.Count != c0.Count) || x.Count != c1.Count + 1) {
                throw new ArgumentException(@"ArgumentVectorsSameLength");
            }

            if (x.Count < 2) {
                throw new ArgumentException(string.Format(@"ArrayTooSmall", 2), nameof(x));
            }

            _x = x;
            _c0 = c0;
            _c1 = c1;
            _indefiniteIntegral = new Lazy<double[]>(ComputeIndefiniteIntegral);
        }

        /// <summary>
        /// Create a linear spline interpolation from a set of (x,y) value pairs, sorted ascendingly by x.
        /// </summary>
        public static LinearSpline InterpolateSorted(double[] x, double[] y) {
            if (x.Length != y.Length) {
                throw new ArgumentException(@"ArgumentVectorsSameLength");
            }

            if (x.Length < 2) {
                throw new ArgumentException(string.Format(@"ArrayTooSmall", 2), nameof(x));
            }

            var c1 = new double[x.Length - 1];
            for (int i = 0; i < c1.Length; i++) {
                c1[i] = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
            }

            return new LinearSpline(new StructuredBufferAccessor<double>(x), new StructuredBufferAccessor<double>(y), new StructuredBufferAccessor<double>(c1));
        }

        /// <summary>
        /// Create a linear spline interpolation from an unsorted set of (x,y) value pairs.
        /// WARNING: Works in-place and can thus causes the data array to be reordered.
        /// </summary>
        public static LinearSpline InterpolateInplace(double[] x, double[] y) {
            if (x.Length != y.Length) {
                throw new ArgumentException(@"ArgumentVectorsSameLength");
            }

            Array.Sort(x, y);
            return InterpolateSorted(x, y);
        }

        /// <summary>
        /// Create a linear spline interpolation from an unsorted set of (x,y) value pairs.
        /// </summary>
        public static LinearSpline Interpolate(IEnumerable<double> x, IEnumerable<double> y) {
            // note: we must make a copy, even if the input was arrays already
            return InterpolateInplace(x.ToArray(), y.ToArray());
        }


        /// <summary>
        /// Interpolate at point t.
        /// </summary>
        /// <param name="t">Point t to interpolate at.</param>
        /// <returns>Interpolated value x(t).</returns>
        public double Interpolate(double t) {
            int k = LeftSegmentIndex(t);
            return _c0[k] + (t - _x[k]) * _c1[k];
        }

        /// <summary>
        /// Differentiate at point t.
        /// </summary>
        /// <param name="t">Point t to interpolate at.</param>
        /// <returns>Interpolated first derivative at point t.</returns>
        public double Differentiate(double t) {
            int k = LeftSegmentIndex(t);
            return _c1[k];
        }

        /// <summary>
        /// Differentiate twice at point t.
        /// </summary>
        /// <param name="t">Point t to interpolate at.</param>
        /// <returns>Interpolated second derivative at point t.</returns>
        public double Differentiate2(double t) {
            return 0d;
        }

        /// <summary>
        /// Indefinite integral at point t.
        /// </summary>
        /// <param name="t">Point t to integrate at.</param>
        public double Integrate(double t) {
            int k = LeftSegmentIndex(t);
            var x = t - _x[k];
            return _indefiniteIntegral.Value[k] + x * (_c0[k] + x * _c1[k] / 2);
        }

        /// <summary>
        /// Definite integral between points a and b.
        /// </summary>
        /// <param name="a">Left bound of the integration interval [a,b].</param>
        /// <param name="b">Right bound of the integration interval [a,b].</param>
        public double Integrate(double a, double b) {
            return Integrate(b) - Integrate(a);
        }

        double[] ComputeIndefiniteIntegral() {
            var integral = new double[_c1.Count];
            for (int i = 0; i < integral.Length - 1; i++) {
                double w = _x[i + 1] - _x[i];
                integral[i + 1] = integral[i] + w * (_c0[i] + w * _c1[i] / 2);
            }

            return integral;
        }

        /// <summary>
        /// Find the index of the greatest sample point smaller than t,
        /// or the left index of the closest segment for extrapolation.
        /// </summary>
        int LeftSegmentIndex(double t) {
            int index = sdafasdf.BinarySearch<double, sdfsdasdad<double>, sdafasdf.ComparerComparable<double, sdfsda>>(new sdfsdasdad<double>(_x), _x.Count, new sdafasdf.ComparerComparable<double, sdfsda>(t, default(sdfsda)));
            if (index < 0) {
                index = ~index - 1;
            }

            return Math.Min(Math.Max(index, 0), _x.Count - 2);
        }
    }
    public readonly struct sdfsdasdad<T>
        : IReadOnlyRandomAccessor<T>
        where T : unmanaged {
        readonly StructuredBufferAccessor<T> x;

        public sdfsdasdad(StructuredBufferAccessor<T> x) {
            this.x = x;
        }

        public T this[int index] {

            get => x[index];
        }
    }

    public readonly struct sdfsda : IComparer<double> {
        public int Compare([AllowNull] double x, [AllowNull] double y) {
            return x.CompareTo(y);
        }
    }
    public interface IReadOnlyRandomAccessor<T> {

        T this[int index] {

            get;
        }
    }

    internal static class sdafasdf {

        public static int BinarySearch<T, TList, TComparable>(
            TList spanStart, int length, TComparable comparable)
            where TList : IReadOnlyRandomAccessor<T>
            where TComparable : IComparable<T> {
            int lo = 0;
            int hi = length - 1;
            // If length == 0, hi == -1, and loop will not be entered
            while (lo <= hi) {
                // PERF: `lo` or `hi` will never be negative inside the loop,
                //       so computing median using uints is safe since we know
                //       `length <= int.MaxValue`, and indices are >= 0
                //       and thus cannot overflow an uint.
                //       Saves one subtraction per loop compared to
                //       `int i = lo + ((hi - lo) >> 1);`
                int i = (int)(((uint)hi + (uint)lo) >> 1);

                int c = comparable.CompareTo(spanStart[i]);
                if (c == 0) {
                    return i;
                } else if (c > 0) {
                    lo = i + 1;
                } else {
                    hi = i - 1;
                }
            }
            // If none found, then a negative number that is the bitwise complement
            // of the index of the next element that is larger than or, if there is
            // no larger element, the bitwise complement of `length`, which
            // is `lo` at this point.
            return ~lo;
        }

        // Helper to allow sharing all code via IComparable<T> inlineable
        internal readonly struct ComparerComparable<T, TComparer> : IComparable<T>
            where TComparer : IComparer<T> {
            private readonly T _value;
            private readonly TComparer _comparer;

            public ComparerComparable(T value, TComparer comparer) {
                _value = value;
                _comparer = comparer;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int CompareTo([AllowNull] T other) => _comparer.Compare(_value, other);
        }
    }

    public readonly struct Accessor<TMemory, T>

    where TMemory : IReadOnlySpanProvider<byte>
    where T : unmanaged {

        readonly TMemory _BufferView;

        readonly int _ByteStride;
        readonly int _Count;

        public readonly ref T this[int index] {

            get {
                if (unchecked((uint)_Count) > unchecked((uint)index)) {
                    var offset = _ByteStride * index;
                    return ref Unsafe.As<byte, T>(ref Unsafe.AsRef(in _BufferView.Span[offset]));
                }
                _ = checked(unchecked((uint)_Count) - unchecked((uint)index));
                throw null;
            }
        }




        /*
        public unsafe static Accessor<T> CreateFrom<TStructure>(ArraySegment<T> buffer, Expression<Func<TStructure, T>> fieldSelector) where TStructure : unmanaged {

            var field = GetField(fieldSelector);
            var byte_offset = GetFieldOffset(typeof(TStructure), field);
            var bytesize_TStructure = Unsafe.SizeOf<TStructure>();
            var bytesize_T = Unsafe.SizeOf<T>();
            var a = buffer.Span[0];


        }
        */
        public static int GetFieldOffset(Type tStructure, FieldInfo field) {
            if (tStructure is null) {
                throw new ArgumentNullException(nameof(tStructure));
            }
            if (field is null) {
                throw new ArgumentNullException(nameof(field));
            }
            if (tStructure != field.DeclaringType) {
                throw new InvalidOperationException();
            }


            var m = new DynamicMethod("", typeof(int), Type.EmptyTypes);
            var ilg = m.GetILGenerator();

            var v = ilg.DeclareLocal(tStructure);
            ilg.Emit(OpCodes.Ldloca_S, v);
            ilg.Emit(OpCodes.Ldflda, field);
            ilg.Emit(OpCodes.Ldloca_S, v);
            ilg.Emit(OpCodes.Sub);
            ilg.Emit(OpCodes.Ret);

            var w = m.CreateDelegate(typeof(Func<int>)) as Func<int>;
            return w.Invoke();
        }

        private static FieldInfo GetField<TStructure>(Expression<Func<TStructure, T>> fieldSelector) {
            if (typeof(TStructure).IsValueType) {
                if (fieldSelector.Body is MemberExpression memberExpression) {
                    if (memberExpression.Member is FieldInfo fieldInfo) {
                        return fieldInfo;
                    }
                }
                // TODO: Some cases not implemented yet.
                throw new NotSupportedException();
            }
            throw new NotSupportedException();
        }


    }
}

namespace UltimateOrb {
    using System;

    public interface IArrayAccessor {

        bool IsHuge {

            get;
        }
    }

    public interface IArrayAccessor<T>
        : IArrayAccessor {

        ref T this[int index] {

            get;
        }
    }

    public interface IArrayAccessorLong<T>
        : IArrayAccessor<T> {

        ref T this[long index] {

            get;
        }
    }

    public interface IArrayAccessorNative<T>
        : IArrayAccessor<T> {

        ref T this[IntPtr index] {

            get;
        }
    }

    public interface IArrayAccessorBasic<T> {

        bool IsHuge {

            get;
        }

        ref T this[int index] {

            get;
        }

        ref T this[uint index] {

            get;
        }

        ref T this[long index] {

            get;
        }

        ref T this[ulong index] {

            get;
        }

        ref T this[IntPtr index] {

            get;
        }

        ref T this[UIntPtr index] {

            get;
        }

        int Length {

            get;
        }

        long LongLength {

            get;
        }
    }
}
