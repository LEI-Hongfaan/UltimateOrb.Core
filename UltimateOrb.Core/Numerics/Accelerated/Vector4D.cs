using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System;
using System.Runtime.Intrinsics;
using System.Runtime.InteropServices;

namespace UltimateOrb.Numerics.Accelerated {

    /// <summary>Represents a vector with four single-precision doubleing-point values.</summary>
    public readonly partial struct Vector4D :
        IEquatable<Vector4D>,
        IFormattable {

        readonly Vector256<double> m_value;

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double X {

            get => m_value.GetElement(0);
        }

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double Y {

            get => m_value.GetElement(1);
        }

        /// <summary>The Z value of the vector component of the quaternion.</summary>
        public double Z {

            get => m_value.GetElement(2);
        }

        /// <summary>The rotation component of the quaternion.</summary>
        public double W {

            get => m_value.GetElement(3);
        }

        internal const int Count = 4;

        internal Vector4D(Vector256<double> xyzw) {
            m_value = xyzw;
        }

        /// <summary>Creates a new <see cref="Vector4D" /> object whose four elements have the same value.</summary>
        /// <param name="value">The value to assign to all four elements.</param>
        public Vector4D(double value) {
            this = Create(value);
        }

        /*
        /// <summary>Creates a   new <see cref="Vector4D" /> object from the specified <see cref="Vector2D" /> object and a Z and a W component.</summary>
        /// <param name="value">The vector to use for the X and Y components.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        public Vector4D(Vector2D value, double z, double w) {
            this = Create(value, z, w);
        }
        */

        /// <summary>Constructs a new <see cref="Vector4D" /> object from the specified <see cref="Numerics.Vector3D" /> object and a W component.</summary>
        /// <param name="value">The vector to use for the X, Y, and Z components.</param>
        /// <param name="w">The W component.</param>
        public Vector4D(Vector3D value, double w) {
            this = Create(value, w);
        }

        /// <summary>Creates a vector whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
        public Vector4D(double x, double y, double z, double w) {
            this = Create(x, y, z, w);
        }

        /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Single}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        public Vector4D(ReadOnlySpan<double> values) {
            this = Create(values);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.E" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.E" /> (that is, it returns the vector <c>Create(double.E)</c>).</value>
        public static Vector4D E {

            get => Create(double.E);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.Epsilon" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.Epsilon" /> (that is, it returns the vector <c>Create(double.Epsilon)</c>).</value>
        public static Vector4D Epsilon {

            get => Create(double.Epsilon);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.NaN" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.NaN" /> (that is, it returns the vector <c>Create(double.NaN)</c>).</value>
        public static Vector4D NaN {

            get => Create(double.NaN);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.NegativeInfinity" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.NegativeInfinity" /> (that is, it returns the vector <c>Create(double.NegativeInfinity)</c>).</value>
        public static Vector4D NegativeInfinity {

            get => Create(double.NegativeInfinity);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.NegativeZero" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.NegativeZero" /> (that is, it returns the vector <c>Create(double.NegativeZero)</c>).</value>
        public static Vector4D NegativeZero {

            get => Create(double.NegativeZero);
        }

        /// <summary>Gets a vector whose elements are equal to one.</summary>
        /// <value>A vector whose elements are equal to one (that is, it returns the vector <c>Create(1)</c>).</value>
        public static Vector4D One {

            get => Create(1.0);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.Pi" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.Pi" /> (that is, it returns the vector <c>Create(double.Pi)</c>).</value>
        public static Vector4D Pi {

            get => Create(double.Pi);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.PositiveInfinity" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.PositiveInfinity" /> (that is, it returns the vector <c>Create(double.PositiveInfinity)</c>).</value>
        public static Vector4D PositiveInfinity {

            get => Create(double.PositiveInfinity);
        }

        /// <summary>Gets a vector whose elements are equal to <see cref="double.Tau" />.</summary>
        /// <value>A vector whose elements are equal to <see cref="double.Tau" /> (that is, it returns the vector <c>Create(double.Tau)</c>).</value>
        public static Vector4D Tau {

            get => Create(double.Tau);
        }

        /// <summary>Gets the vector (1,0,0,0).</summary>
        /// <value>The vector <c>(1,0,0,0)</c>.</value>
        public static Vector4D UnitX {

            get => CreateScalar(1.0);
        }

        /// <summary>Gets the vector (0,1,0,0).</summary>
        /// <value>The vector <c>(0,1,0,0)</c>.</value>
        public static Vector4D UnitY {

            get => Create(0.0, 1.0, 0.0, 0.0);
        }

        /// <summary>Gets the vector (0,0,1,0).</summary>
        /// <value>The vector <c>(0,0,1,0)</c>.</value>
        public static Vector4D UnitZ {

            get => Create(0.0, 0.0, 1.0, 0.0);
        }

        /// <summary>Gets the vector (0,0,0,1).</summary>
        /// <value>The vector <c>(0,0,0,1)</c>.</value>
        public static Vector4D UnitW {

            get => Create(0.0, 0.0, 0.0, 1.0);
        }

        /// <summary>Gets a vector whose elements are equal to zero.</summary>
        /// <value>A vector whose elements are equal to zero (that is, it returns the vector <c>Create(0)</c>).</value>
        public static Vector4D Zero {

            get => default;
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>The the element at <paramref name="index" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        public double this[int index] {

            get => m_value.GetElement(index);
        }

        internal Vector256<double> ToVector256() {
            return m_value;
        }

        internal Vector256<double> ToVector256Unsafe() {
            return m_value;
        }

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        /// <remarks>The <see cref="op_Addition" /> method defines the addition operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator +(Vector4D left, Vector4D right) => (left.ToVector256() + right.ToVector256()).AsVector4();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
        /// <remarks>The <see cref="Vector4D.op_Division" /> method defines the division operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator /(Vector4D left, Vector4D right) => (left.ToVector256() / right.ToVector256()).AsVector4();

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="value1">The vector.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <remarks>The <see cref="Vector4D.op_Division" /> method defines the division operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator /(Vector4D value1, double value2) => (value1.ToVector256() / value2).AsVector4();

        /// <summary>Returns a value that indicates whether each pair of elements in two specified vectors is equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two <see cref="Vector4D" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Vector4D left, Vector4D right) => left.ToVector256() == right.ToVector256();

        /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
        /// <param name="left">The first vector to compare.</param>
        /// <param name="right">The second vector to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Vector4D left, Vector4D right) => !(left == right);

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        /// <remarks>The <see cref="Vector4D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(Vector4D left, Vector4D right) => (left.ToVector256() * right.ToVector256()).AsVector4();

        /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="Vector4D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator *(Vector4D left, double right) => (left.ToVector256() * right).AsVector4();

        /// <summary>Multiplies the scalar value by the specified vector.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        /// <remarks>The <see cref="Vector4D.op_Multiply" /> method defines the multiplication operation for <see cref="Vector4D" /> objects.</remarks>
        public static Vector4D operator *(double left, Vector4D right) => right * left;

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the subtraction operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator -(Vector4D left, Vector4D right) => (left.ToVector256() - right.ToVector256()).AsVector4();

        /// <summary>Negates the specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the unary negation operation for <see cref="Vector4D" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D operator -(Vector4D value) => (-value.ToVector256()).AsVector4();

        /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The absolute value vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Abs(Vector4D value) => Vector256.Abs(value.ToVector256()).AsVector4();

        /// <summary>Adds two vectors together.</summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The summed vector.</returns>
        public static Vector4D Add(Vector4D left, Vector4D right) => left + right;

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="ISimdVector{TSelf, T}.Clamp(TSelf, TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max) => Vector256.Clamp(value1.ToVector256(), min.ToVector256(), max.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.ClampNative(TSelf, TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D ClampNative(Vector4D value1, Vector4D min, Vector4D max) => Vector256.ClampNative(value1.ToVector256(), min.ToVector256(), max.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.CopySign(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D CopySign(Vector4D value, Vector4D sign) => Vector256.CopySign(value.ToVector256(), sign.ToVector256()).AsVector4();

        /// <inheritdoc cref="Vector256.Cos(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Cos(Vector4D vector) => Vector256.Cos(vector.ToVector256()).AsVector4();
#endif

        /// <summary>Creates a new <see cref="Vector4D" /> object whose four elements have the same value.</summary>
        /// <param name="value">The value to assign to all four elements.</param>
        /// <returns>A new <see cref="Vector4D" /> whose four elements have the same value.</returns>
        public static Vector4D Create(double value) => Vector256.Create(value).AsVector4();

        /*
        /// <summary>Creates a new <see cref="Vector4D" /> object from the specified <see cref="Vector2D" /> object and a Z and a W component.</summary>
        /// <param name="vector">The vector to use for the X and Y components.</param>
        /// <param name="z">The Z component.</param>
        /// <param name="w">The W component.</param>
        /// <returns>A new <see cref="Vector4D" /> from the specified <see cref="Vector2D" /> object and a Z and a W component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Create(Vector2D vector, double z, double w) {
            return vector.ToVector256Unsafe()
                         .WithElement(2, z)
                         .WithElement(3, w)
                         .AsVector4();
        }
        */

        /// <summary>Constructs a new <see cref="Vector4D" /> object from the specified <see cref="Numerics.Vector3D" /> object and a W component.</summary>
        /// <param name="vector">The vector to use for the X, Y, and Z components.</param>
        /// <param name="w">The W component.</param>
        /// <returns>A new <see cref="Vector4D" /> from the specified <see cref="Numerics.Vector3D" /> object and a W component.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Create(Vector3D vector, double w) {
            return Unsafe.As<Vector3D, Vector4D>(ref vector).
                ToVector256Unsafe().WithElement(3, w).AsVector4();
        }

        /// <summary>Creates a vector whose elements have the specified values.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
        /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
        /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
        /// <returns>A new <see cref="Vector4D" /> whose elements have the specified values.</returns>
        public static Vector4D Create(double x, double y, double z, double w) => Vector256.Create(x, y, z, w).AsVector4();

        /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Single}" />. The span must contain at least 4 elements.</summary>
        /// <param name="values">The span of elements to assign to the vector.</param>
        public static Vector4D Create(ReadOnlySpan<double> values) => Vector256.Create(values).AsVector4();

        /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements initialized to zero.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <returns>A new <see cref="Vector4D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements initialized to zero.</returns>
        internal static Vector4D CreateScalar(double x) => Vector256.CreateScalar(x).AsVector4();

        /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements left uninitialized.</summary>
        /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
        /// <returns>A new <see cref="Vector4D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements left uninitialized.</returns>
        internal static Vector4D CreateScalarUnsafe(double x) => Vector256.CreateScalarUnsafe(x).AsVector4();

#if NET9_0_OR_GREATER
#else
        internal const double DegreesToRadiansFactor = 57.2957795130823208767981548141; // 180 / Math.PI
#endif

        /// <inheritdoc cref="Vector256.DegreesToRadians(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D DegreesToRadians(Vector4D degrees) =>
#if NET9_0_OR_GREATER
            Vector256.DegreesToRadians(degrees.ToVector256()).AsVector4();
#else
            (Vector256.Create(DegreesToRadiansFactor) * degrees.ToVector256()).AsVector4();
#endif

        /// <summary>Computes the Euclidean distance between the two given points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance.</returns>
        public static double Distance(Vector4D value1, Vector4D value2) => double.Sqrt(DistanceSquared(value1, value2));

        /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
        /// <param name="value1">The first point.</param>
        /// <param name="value2">The second point.</param>
        /// <returns>The distance squared.</returns>
        public static double DistanceSquared(Vector4D value1, Vector4D value2) => (value1 - value2).LengthSquared();

        /// <summary>Divides the first vector by the second.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The vector resulting from the division.</returns>
        public static Vector4D Divide(Vector4D left, Vector4D right) => left / right;

        /// <summary>Divides the specified vector by a specified scalar value.</summary>
        /// <param name="left">The vector.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The vector that results from the division.</returns>
        public static Vector4D Divide(Vector4D left, double divisor) => left / divisor;

        /// <summary>Returns the dot product of two vectors.</summary>
        /// <param name="vector1">The first vector.</param>
        /// <param name="vector2">The second vector.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(Vector4D vector1, Vector4D vector2) => Vector256.Dot(vector1.ToVector256(), vector2.ToVector256());

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="Vector256.Exp(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Exp(Vector4D vector) => Vector256.Exp(vector.ToVector256()).AsVector4();

        /// <inheritdoc cref="Vector256.MultiplyAddEstimate(Vector256{double}, Vector256{double}, Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D FusedMultiplyAdd(Vector4D left, Vector4D right, Vector4D addend) => Vector256.FusedMultiplyAdd(left.ToVector256(), right.ToVector256(), addend.ToVector256()).AsVector4();

        /// <inheritdoc cref="Vector256.Hypot(Vector256{double}, Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Hypot(Vector4D x, Vector4D y) => Vector256.Hypot(x.ToVector256(), y.ToVector256()).AsVector4();
#endif

        /// <inheritdoc cref="Lerp(Vector4D, Vector4D, Vector4D)" />
        /// <remarks><format type="text/markdown"><![CDATA[
        /// The behavior of this method changed in .NET 5.0. For more information, see [Behavior change for Vector2D.Lerp and Vector4D.Lerp](/dotnet/core/compatibility/3.1-5.0#behavior-change-for-vector2lerp-and-vector4lerp).
        /// ]]></format></remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount) => Lerp(value1, value2, Create(amount));

        /// <inheritdoc cref="Vector256.Lerp(Vector256{double}, Vector256{double}, Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Lerp(Vector4D value1, Vector4D value2, Vector4D amount) {
#if NET9_0_OR_GREATER
            return Vector256.Lerp(value1.ToVector256(), value2.ToVector256(), amount.ToVector256()).AsVector4();
#else
            Span<double> xyzw = stackalloc double[Count];
            for (var i = 0; Count > i; ++i) {
                xyzw[i] = double.Lerp(value1[i], value2[i], amount[i]);
            }
            return new Vector4D(xyzw);
#endif
        }

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="Vector256.Log(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Log(Vector4D vector) => Vector256.Log(vector.ToVector256()).AsVector4();

        /// <inheritdoc cref="Vector256.Log2(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Log2(Vector4D vector) => Vector256.Log2(vector.ToVector256()).AsVector4();
#endif
        /// <inheritdoc cref="ISimdVector{TSelf, T}.Max(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Max(Vector4D value1, Vector4D value2) => Vector256.Max(value1.ToVector256(), value2.ToVector256()).AsVector4();

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="ISimdVector{TSelf, T}.MaxMagnitude(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MaxMagnitude(Vector4D value1, Vector4D value2) => Vector256.MaxMagnitude(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MaxMagnitudeNumber(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MaxMagnitudeNumber(Vector4D value1, Vector4D value2) => Vector256.MaxMagnitudeNumber(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MaxNative(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MaxNative(Vector4D value1, Vector4D value2) => Vector256.MaxNative(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MaxNumber(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MaxNumber(Vector4D value1, Vector4D value2) => Vector256.MaxNumber(value1.ToVector256(), value2.ToVector256()).AsVector4();
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="ISimdVector{TSelf, T}.Min(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Min(Vector4D value1, Vector4D value2) => Vector256.Min(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MinMagnitude(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MinMagnitude(Vector4D value1, Vector4D value2) => Vector256.MinMagnitude(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MinMagnitudeNumber(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MinMagnitudeNumber(Vector4D value1, Vector4D value2) => Vector256.MinMagnitudeNumber(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MinNative(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MinNative(Vector4D value1, Vector4D value2) => Vector256.MinNative(value1.ToVector256(), value2.ToVector256()).AsVector4();

        /// <inheritdoc cref="ISimdVector{TSelf, T}.MinNumber(TSelf, TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MinNumber(Vector4D value1, Vector4D value2) => Vector256.MinNumber(value1.ToVector256(), value2.ToVector256()).AsVector4();
#endif

        /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The element-wise product vector.</returns>
        public static Vector4D Multiply(Vector4D left, Vector4D right) => left * right;

        /// <summary>Multiplies a vector by a specified scalar.</summary>
        /// <param name="left">The vector to multiply.</param>
        /// <param name="right">The scalar value.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D Multiply(Vector4D left, double right) => left * right;

        /// <summary>Multiplies a scalar value by a specified vector.</summary>
        /// <param name="left">The scaled value.</param>
        /// <param name="right">The vector.</param>
        /// <returns>The scaled vector.</returns>
        public static Vector4D Multiply(double left, Vector4D right) => left * right;

        /// <inheritdoc cref="Vector256.MultiplyAddEstimate(Vector256{double}, Vector256{double}, Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D MultiplyAddEstimate(Vector4D left, Vector4D right, Vector4D addend) =>
#if NET9_0_OR_GREATER
            Vector256.MultiplyAddEstimate(left.ToVector256(), right.ToVector256(), addend.ToVector256()).AsVector4();
#else
            (addend.ToVector256() + left.ToVector256() * right.ToVector256()).AsVector4();
#endif

        /// <summary>Negates a specified vector.</summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>The negated vector.</returns>
        public static Vector4D Negate(Vector4D value) => -value;

        /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
        /// <param name="vector">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        public static Vector4D Normalize(Vector4D vector) => vector / vector.Length();

#if NET9_0_OR_GREATER
#else
        internal const double RadiansToDegreesFactor = 0.0174532925199432957692369; // Math.PI / 180
#endif

        /// <inheritdoc cref="Vector256.RadiansToDegrees(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D RadiansToDegrees(Vector4D radians) =>
#if NET9_0_OR_GREATER
            Vector256.RadiansToDegrees(radians.ToVector256()).AsVector4();
#else
            (Vector256.Create(RadiansToDegreesFactor) * radians.ToVector256()).AsVector4();
#endif

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="Vector256.Round(Vector256{double})" />
        public static Vector4D Round(Vector4D vector) => Vector256.Round(vector.ToVector256()).AsVector4();

        /// <inheritdoc cref="Vector256.Round(Vector256{double}, MidpointRounding)" />
        public static Vector4D Round(Vector4D vector, MidpointRounding mode) => Vector256.Round(vector.ToVector256(), mode).AsVector4();

        /// <inheritdoc cref="Vector256.Sin(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Sin(Vector4D vector) => Vector256.Sin(vector.ToVector256()).AsVector4();
#endif

        /// <inheritdoc cref="Vector256.SinCos(Vector256{double})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector4D Sin, Vector4D Cos) SinCos(Vector4D vector) {
#if NET9_0_OR_GREATER
            (Vector256<double> sin, Vector256<double> cos) = Vector256.SinCos(vector.ToVector256());
            return (sin.AsVector4(), cos.AsVector4());
#else
            // TODO:
            var (xs, xc) = Math.SinCos(vector.X);
            var (ys, yc) = Math.SinCos(vector.Y);
            var (zs, zc) = Math.SinCos(vector.Z);
            var (ws, wc) = Math.SinCos(vector.W);
            return (new Vector4D(xs, ys, zs, ws), new Vector4D(xc, yc, zc, wc));
#endif
        }

        /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
        /// <param name="value">A vector.</param>
        /// <returns>The square root vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D SquareRoot(Vector4D value) => Vector256.Sqrt(value.ToVector256()).AsVector4();

        /// <summary>Subtracts the second vector from the first.</summary>
        /// <param name="left">The first vector.</param>
        /// <param name="right">The second vector.</param>
        /// <returns>The difference vector.</returns>
        public static Vector4D Subtract(Vector4D left, Vector4D right) => left - right;

        /*
        /// <summary>Transforms a two-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector4D Transform(Vector2D position, Matrix4x4 matrix) => Transform(position, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector4D Transform(Vector2D position, in Matrix4x4.Impl matrix) {
            // This implementation is based on the DirectX Math Library XMVector2Transform method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            Vector4D result = matrix.X * position.X;
            result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
            return result + matrix.W;
        }

        /// <summary>Transforms a two-dimensional vector by the specified QuaternionD rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector2D value, QuaternionD rotation) => Transform(Create(value, 0.0, 1.0), rotation);

        /// <summary>Transforms a three-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="position">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector4D Transform(Vector3D position, Matrix4x4 matrix) => Transform(position, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector4D Transform(Vector3D position, in Matrix4x4.Impl matrix) {
            // This implementation is based on the DirectX Math Library XMVector3Transform method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            Vector4D result = matrix.X * position.X;
            result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
            result = MultiplyAddEstimate(matrix.Z, Create(position.Z), result);
            return result + matrix.W;
        }
        */

        /// <summary>Transforms a three-dimensional vector by the specified QuaternionD rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector3D value, QuaternionD rotation) => Transform(Create(value, 1.0), rotation);

        /*
        /// <summary>Transforms a four-dimensional vector by a specified 4x4 matrix.</summary>
        /// <param name="vector">The vector to transform.</param>
        /// <param name="matrix">The transformation matrix.</param>
        /// <returns>The transformed vector.</returns>
        public static Vector4D Transform(Vector4D vector, Matrix4x4 matrix) => Transform(vector, in matrix.AsImpl());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector4D Transform(Vector4D vector, in Matrix4x4 matrix) {
            // This implementation is based on the DirectX Math Library XMVector4Transform method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            Vector4D result = matrix.X * vector.X;
            result = MultiplyAddEstimate(matrix.Y, Create(vector.Y), result);
            result = MultiplyAddEstimate(matrix.Z, Create(vector.Z), result);
            result = MultiplyAddEstimate(matrix.W, Create(vector.W), result);
            return result;
        }
        */

        /// <summary>Transforms a four-dimensional vector by the specified QuaternionD rotation value.</summary>
        /// <param name="value">The vector to rotate.</param>
        /// <param name="rotation">The rotation to apply.</param>
        /// <returns>The transformed vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4D Transform(Vector4D value, QuaternionD rotation) {
            // This implementation is based on the DirectX Math Library XMVector3Rotate method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl

            QuaternionD conjuagate = QuaternionD.Conjugate(rotation);
            QuaternionD temp = QuaternionD.Concatenate(conjuagate, value.AsQuaternion());
            return QuaternionD.Concatenate(temp, rotation).AsVector4();
        }

#if NET9_0_OR_GREATER
        /// <inheritdoc cref="Vector256.Truncate(Vector256{double})" />
        public static Vector4D Truncate(Vector4D vector) => Vector256.Truncate(vector.ToVector256()).AsVector4();
#endif

        /// <summary>Copies the elements of the vector to a specified array.</summary>
        /// <param name="array">The destination array.</param>
        /// <remarks><paramref name="array" /> must have at least four elements. The method copies the vector's elements starting at index 0.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        public readonly void CopyTo(double[] array) => this.ToVector256().CopyTo(array);

        /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
        /// <param name="array">The destination array.</param>
        /// <param name="index">The index at which to copy the first element of the vector.</param>
        /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four vector elements. In other words, elements <paramref name="index" /> through <paramref name="index" /> + 3 must already exist in <paramref name="array" />.</remarks>
        /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
        /// -or-
        /// <paramref name="index" /> is greater than or equal to the array length.</exception>
        /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
        public readonly void CopyTo(double[] array, int index) => this.ToVector256().CopyTo(array, index);

        /// <summary>Copies the vector to the given <see cref="Span{T}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
        public readonly void CopyTo(Span<double> destination) => this.ToVector256().CopyTo(destination);

        /// <summary>Attempts to copy the vector to the given <see cref="Span{Single}" />. The length of the destination span must be at least 4.</summary>
        /// <param name="destination">The destination span which the values are copied into.</param>
        /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
        public readonly bool TryCopyTo(Span<double> destination) => this.ToVector256().TryCopyTo(destination);

        /// <summary>Returns a value that indicates whether this instance and another vector are equal.</summary>
        /// <param name="other">The other vector.</param>
        /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two vectors are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z" />, and <see cref="W" /> elements are equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Vector4D other) => this.ToVector256().Equals(other.ToVector256());

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Vector4D" /> object and their corresponding elements are equal.</remarks>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is Vector4D other) && Equals(other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z, W);

        /// <summary>Returns the length of this vector object.</summary>
        /// <returns>The vector's length.</returns>
        /// <altmember cref="LengthSquared"/>
        public readonly double Length() => double.Sqrt(LengthSquared());

        /// <summary>Returns the length of the vector squared.</summary>
        /// <returns>The vector's length squared.</returns>
        /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
        /// <altmember cref="Length"/>
        public readonly double LengthSquared() => Dot(this, this);

        /// <summary>Returns the string representation of the current instance using default formatting.</summary>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        public override readonly string ToString() => ToString("G", CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
        /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
        /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
        /// <returns>The string representation of the current instance.</returns>
        /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
        /// <related type="Article" href="/dotnet/standard/base-types/standard-numeric-format-strings">Standard Numeric Format Strings</related>
        /// <related type="Article" href="/dotnet/standard/base-types/custom-numeric-format-strings">Custom Numeric Format Strings</related>
        public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider) {
            string separator = NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

            return $"<{X.ToString(format, formatProvider)}{separator} {Y.ToString(format, formatProvider)}{separator} {Z.ToString(format, formatProvider)}{separator} {W.ToString(format, formatProvider)}>";
        }
    }
}
namespace UltimateOrb.Numerics.Accelerated {

    public readonly partial struct Vector4D {

        public double E0 {

            get => m_value.GetElement(0);
        }

        public double E1 {

            get => m_value.GetElement(1);
        }

        public double E2 {

            get => m_value.GetElement(2);
        }

        public double E3 {

            get => m_value.GetElement(3);
        }

        public double GetLengthSquared() {
            var v = m_value;
            return Vector256.Dot(v, v);
        }

        public ReadOnlySpan<double> AsSpan() {
            return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<Vector4D, double>(ref Unsafe.AsRef(in this)), Count);
        }

        public ref readonly Numerics.Vector3D E012 {

            get {
                return ref Unsafe.As<Vector4D, Numerics.Vector3D>(ref Unsafe.AsRef(in this));
            }
        }

        /*
        public Vector4D(double e0, double e1, double e2, double e3) {
            this.E0 = e0;
            this.E1 = e1;
            this.E2 = e2;
            this.E3 = e3;
        }
        */

        public string ToString_UO() {
            return $@"[ {E0:R}, {E1:R}, {E2:R}, {E3:R} ]";
        }

        public Vector4D GetNormalized() {
            throw new NotImplementedException();
        }

        Vector256<double> ToIntrinsic() {
            return m_value;
        }

        public static double DotProduct(Vector4D first, Vector4D second) {
            return Vector256.Dot(first.ToVector256(), second.ToVector256());
        }
    }
}