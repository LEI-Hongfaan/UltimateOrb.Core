using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics.Accelerated {

    /// <summary>Represents a vector that is used to encode three-dimensional physical rotations.</summary>
    /// <remarks>The <see cref="QuaternionD" /> structure is used to efficiently rotate an object about the (x,y,z) vector by the angle theta, where:
    /// <c>w = cos(theta/2)</c></remarks>
    public readonly partial struct QuaternionD :
        IEquatable<QuaternionD> {

        /// <summary>The vector representation of the quaternion.</summary>
        readonly Vector4D m_value;

        public readonly Vector4D XYZW {

            get => m_value;
        }

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double X {

            get => XYZW.X;
        }

        /// <summary>The Y value of the vector component of the quaternion.</summary>
        public double Y {

            get => XYZW.Y;
        }

        /// <summary>The Z value of the vector component of the quaternion.</summary>
        public double Z {

            get => XYZW.Z;
        }

        /// <summary>The rotation component of the quaternion.</summary>
        public double W {

            get => XYZW.W;
        }

        internal const int Count = 4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal QuaternionD(Vector256<double> xyzw) {
            m_value = new Vector4D(xyzw);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public QuaternionD(Vector4D xyzw) {
            m_value = xyzw;
        }

        /// <summary>Constructs a quaternion from the specified components.</summary>
        /// <param name="x">The value to assign to the X component of the quaternion.</param>
        /// <param name="y">The value to assign to the Y component of the quaternion.</param>
        /// <param name="z">The value to assign to the Z component of the quaternion.</param>
        /// <param name="w">The value to assign to the W component of the quaternion.</param>
        public QuaternionD(double x, double y, double z, double w) {
            this = Create(x, y, z, w);
        }

        /// <summary>Creates a quaternion from the specified vector and rotation parts.</summary>
        /// <param name="vectorPart">The vector part of the quaternion.</param>
        /// <param name="scalarPart">The rotation part of the quaternion.</param>
        public QuaternionD(Vector3D vectorPart, double scalarPart) {
            this = Create(vectorPart, scalarPart);
        }

        /// <summary>Gets a quaternion that represents a zero.</summary>
        /// <value>A quaternion whose values are <c>(0, 0, 0, 0)</c>.</value>
        public static QuaternionD Zero {

            get => default;
        }

        /// <summary>Gets a quaternion that represents no rotation.</summary>
        /// <value>A quaternion whose values are <c>(0, 0, 0, 1)</c>.</value>
        public static QuaternionD Identity {

            get => Create(0.0, 0.0, 0.0, 1.0);
        }

        /// <summary>Gets or sets the element at the specified index.</summary>
        /// <param name="index">The index of the element to get or set.</param>
        /// <returns>The element at <paramref name="index" />.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
        public double this[int index] {

            get => m_value[index];
        }

        /// <summary>Gets a value that indicates whether the current instance is the identity quaternion.</summary>
        /// <value><see langword="true" /> if the current instance is the identity quaternion; otherwise, <see langword="false" />.</value>
        /// <altmember cref="Identity"/>
        public readonly bool IsIdentity => Identity.Equals(this);

        internal Vector256<double> ToVector256() {
            return m_value.ToVector256();
        }

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        /// <remarks>The <see cref="op_Addition" /> method defines the operation of the addition operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator +(QuaternionD value1, QuaternionD value2) => (value1.ToVector256() + value2.ToVector256()).AsQuaternion();

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="value1">The dividend.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="value1" /> by <paramref name="value2" />.</returns>
        /// <remarks>The <see cref="op_Division" /> method defines the division operation for <see cref="QuaternionD" /> objects.</remarks>
        public static QuaternionD operator /(QuaternionD value1, QuaternionD value2) => value1 * Inverse(value2);

        /// <summary>Returns a value that indicates whether two quaternions are equal.</summary>
        /// <param name="value1">The first quaternion to compare.</param>
        /// <param name="value2">The second quaternion to compare.</param>
        /// <returns><see langword="true" /> if the two quaternions are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two quaternions are equal if each of their corresponding components is equal.
        /// The <see cref="op_Equality" /> method defines the operation of the equality operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(QuaternionD value1, QuaternionD value2) => value1.ToVector256() == value2.ToVector256();

        /// <summary>Returns a value that indicates whether two quaternions are not equal.</summary>
        /// <param name="value1">The first quaternion to compare.</param>
        /// <param name="value2">The second quaternion to compare.</param>
        /// <returns><see langword="true" /> if <paramref name="value1" /> and <paramref name="value2" /> are not equal; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(QuaternionD value1, QuaternionD value2) => !(value1 == value2);

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        /// <remarks>The <see cref="QuaternionD.op_Multiply" /> method defines the operation of the multiplication operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(QuaternionD value1, QuaternionD value2) {
            // This implementation is based on the DirectX Math Library XMQuaternionMultiply method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            Vector256<double> left = value1.ToVector256();
            Vector256<double> right = value2.ToVector256();

            Vector256<double> result = right * left.GetElement(3);
#if NET9_0_OR_GREATER
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(3, 2, 1, 0)) * left.GetElement(0), Vector256.Create(+1.0, -1.0, +1.0, -1.0), result);
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(2, 3, 0, 1)) * left.GetElement(1), Vector256.Create(+1.0, +1.0, -1.0, -1.0), result);
            result = Vector256.MultiplyAddEstimate(Vector256.Shuffle(right, Vector256.Create(1, 0, 3, 2)) * left.GetElement(2), Vector256.Create(-1.0, +1.0, +1.0, -1.0), result);
#else
            result += Vector256.Multiply(Vector256.Shuffle(right, Vector256.Create(3, 2, 1, 0)) * left.GetElement(0), Vector256.Create(+1.0, -1.0, +1.0, -1.0));
            result += Vector256.Multiply(Vector256.Shuffle(right, Vector256.Create(2, 3, 0, 1)) * left.GetElement(1), Vector256.Create(+1.0, +1.0, -1.0, -1.0));
            result += Vector256.Multiply(Vector256.Shuffle(right, Vector256.Create(1, 0, 3, 2)) * left.GetElement(2), Vector256.Create(-1.0, +1.0, +1.0, -1.0));
#endif
            return result.AsQuaternion();
        }

        /// <summary>Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.</summary>
        /// <param name="value1">The source quaternion.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The scaled quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator *(QuaternionD value1, double value2) => (value1.ToVector256() * value2).AsQuaternion();

        /// <summary>Subtracts each element in a second quaternion from its corresponding element in a first quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion containing the values that result from subtracting each element in <paramref name="value2" /> from its corresponding element in <paramref name="value1" />.</returns>
        /// <remarks>The <see cref="op_Subtraction" /> method defines the operation of the subtraction operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(QuaternionD value1, QuaternionD value2) => (value1.ToVector256() - value2.ToVector256()).AsQuaternion();

        /// <summary>Reverses the sign of each component of the quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negated quaternion.</returns>
        /// <remarks>The <see cref="op_UnaryNegation" /> method defines the operation of the unary negation operator for <see cref="QuaternionD" /> objects.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD operator -(QuaternionD value) => (-value.ToVector256()).AsQuaternion();

        /// <summary>Adds each element in one quaternion with its corresponding element in a second quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion that contains the summed values of <paramref name="value1" /> and <paramref name="value2" />.</returns>
        public static QuaternionD Add(QuaternionD value1, QuaternionD value2) => value1 + value2;

        /// <summary>Concatenates two quaternions.</summary>
        /// <param name="value1">The first quaternion rotation in the series.</param>
        /// <param name="value2">The second quaternion rotation in the series.</param>
        /// <returns>A new quaternion representing the concatenation of the <paramref name="value1" /> rotation followed by the <paramref name="value2" /> rotation.</returns>
        public static QuaternionD Concatenate(QuaternionD value1, QuaternionD value2) => value2 * value1;

        /// <summary>Returns the conjugate of a specified quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns>A new quaternion that is the conjugate of <see langword="value" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD Conjugate(QuaternionD value) {
            // This implementation is based on the DirectX Math Library XMQuaternionConjugate method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            return (value.ToVector256() * Vector256.Create(-1.0, -1.0, -1.0, 1.0)).AsQuaternion();
        }

        /// <summary>Creates a quaternion from the specified components.</summary>
        /// <param name="x">The value to assign to the X component of the quaternion.</param>
        /// <param name="y">The value to assign to the Y component of the quaternion.</param>
        /// <param name="z">The value to assign to the Z component of the quaternion.</param>
        /// <param name="w">The value to assign to the W component of the quaternion.</param>
        /// <returns>A new quaternion created from the specified components.</returns>>
        internal static QuaternionD Create(double x, double y, double z, double w) => Vector256.Create(x, y, z, w).AsQuaternion();

        /// <summary>Creates a quaternion from the specified vector and rotation parts.</summary>
        /// <param name="vectorPart">The vector part of the quaternion.</param>
        /// <param name="scalarPart">The rotation part of the quaternion.</param>
        /// <returns>A new quaternion created from the specified vector and rotation parts.</returns>
        internal static QuaternionD Create(Vector3D vectorPart, double scalarPart) => Vector4D.Create(vectorPart, scalarPart).AsQuaternion();

        /// <summary>Creates a quaternion from a unit vector and an angle to rotate around the vector.</summary>
        /// <param name="axis">The unit vector to rotate around.</param>
        /// <param name="angle">The angle, in radians, to rotate around the vector.</param>
        /// <returns>The newly created quaternion.</returns>
        /// <remarks><paramref name="axis" /> vector must be normalized before calling this method or the resulting <see cref="QuaternionD" /> will be incorrect.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD CreateFromAxisAngle(Vector3D axis, double angle) {
            // This implementation is based on the DirectX Math Library XMQuaternionRotationNormal method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            (double s, double c) = double.SinCos(angle * 0.5);
            return (Vector4D.Create(axis, 1.0).ToVector256() * Vector256.Create(s, s, s, c)).AsQuaternion();
        }

        /// <summary>Creates a quaternion from the specified rotation matrix.</summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The newly created quaternion.</returns>
        public static QuaternionD CreateFromRotationMatrix(Matrix4x4 matrix) {
            double trace = matrix.M11 + matrix.M22 + matrix.M33;

            Span<double> xyzw = stackalloc double[4];

            if (trace > 0.0) {
                double s = double.Sqrt(trace + 1.0);
                xyzw[3] = s * 0.5;
                s = 0.5 / s;
                xyzw[0] = (matrix.M23 - matrix.M32) * s;
                xyzw[1] = (matrix.M31 - matrix.M13) * s;
                xyzw[2] = (matrix.M12 - matrix.M21) * s;
            } else {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33) {
                    double s = double.Sqrt(1.0 + matrix.M11 - matrix.M22 - matrix.M33);
                    double invS = 0.5 / s;
                    xyzw[0] = 0.5 * s;
                    xyzw[1] = (matrix.M12 + matrix.M21) * invS;
                    xyzw[2] = (matrix.M13 + matrix.M31) * invS;
                    xyzw[3] = (matrix.M23 - matrix.M32) * invS;
                } else if (matrix.M22 > matrix.M33) {
                    double s = double.Sqrt(1.0 + matrix.M22 - matrix.M11 - matrix.M33);
                    double invS = 0.5 / s;
                    xyzw[0] = (matrix.M21 + matrix.M12) * invS;
                    xyzw[1] = 0.5 * s;
                    xyzw[2] = (matrix.M32 + matrix.M23) * invS;
                    xyzw[3] = (matrix.M31 - matrix.M13) * invS;
                } else {
                    double s = double.Sqrt(1.0 + matrix.M33 - matrix.M11 - matrix.M22);
                    double invS = 0.5 / s;
                    xyzw[0] = (matrix.M31 + matrix.M13) * invS;
                    xyzw[1] = (matrix.M32 + matrix.M23) * invS;
                    xyzw[2] = 0.5 * s;
                    xyzw[3] = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return Unsafe.As<double, QuaternionD>(ref xyzw[0]);
        }

        /// <summary>Creates a new quaternion from the given yaw, pitch, and roll.</summary>
        /// <param name="yaw">The yaw angle, in radians, around the Y axis.</param>
        /// <param name="pitch">The pitch angle, in radians, around the X axis.</param>
        /// <param name="roll">The roll angle, in radians, around the Z axis.</param>
        /// <returns>The resulting quaternion.</returns>
        public static QuaternionD CreateFromYawPitchRoll_DirectXMath(double yaw, double pitch, double roll) {
            (Vector3D sin, Vector3D cos) = Vector3D.SinCos(Vector3D.Create(roll, pitch, yaw) * 0.5);

            (double sr, double cr) = (sin.X, cos.X);
            (double sp, double cp) = (sin.Y, cos.Y);
            (double sy, double cy) = (sin.Z, cos.Z);

            Span<double> xyzw = [
                cy * sp * cr + sy * cp * sr,
                sy * cp * cr - cy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                cy * cp * cr + sy * sp * sr,
            ];
            return Unsafe.As<double, QuaternionD>(ref xyzw[0]);
        }

        /// <summary>Divides one quaternion by a second quaternion.</summary>
        /// <param name="value1">The dividend.</param>
        /// <param name="value2">The divisor.</param>
        /// <returns>The quaternion that results from dividing <paramref name="value1" /> by <paramref name="value2" />.</returns>
        public static QuaternionD Divide(QuaternionD value1, QuaternionD value2) => value1 / value2;

        /// <summary>Calculates the dot product of two quaternions.</summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <returns>The dot product.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot(QuaternionD quaternion1, QuaternionD quaternion2) => Vector256.Dot(quaternion1.ToVector256(), quaternion2.ToVector256());

        /// <summary>Returns the inverse of a quaternion.</summary>
        /// <param name="value">The quaternion.</param>
        /// <returns>The inverted quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD Inverse(QuaternionD value) {
            // This implementation is based on the DirectX Math Library XMQuaternionInverse method
            // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathMisc.inl

            const double Epsilon = 1.192092896e-7f;

            //  -1   (       a              -v       )
            // q   = ( -------------   ------------- )
            //       (  a^2 + |v|^2  ,  a^2 + |v|^2  )

            Vector256<double> lengthSquared = Vector256.Create(value.LengthSquared());
            return Vector256.AndNot(
                (Conjugate(value).ToVector256() / lengthSquared),
                Vector256.LessThanOrEqual(lengthSquared, Vector256.Create(Epsilon))
            ).AsQuaternion();
        }

        /// <summary>Performs a linear interpolation between two quaternions based on a value that specifies the weighting of the second quaternion.</summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="amount">The relative weight of <paramref name="quaternion2" /> in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static QuaternionD Lerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount) {
            Vector256<double> q2 = quaternion2.ToVector256();

            q2 = Vector256.ConditionalSelect(
                Vector256.GreaterThanOrEqual(Vector256.Create(Dot(quaternion1, quaternion2)), Vector256<double>.Zero),
                 q2,
                -q2
            );
#if NET9_0_OR_GREATER
            Vector256<double> result = Vector256.MultiplyAddEstimate(quaternion1.ToVector256(), Vector256.Create(1.0 - amount), q2 * amount);
#else
            Vector256<double> result = q2 * amount + Vector256.Multiply(quaternion1.ToVector256(), Vector256.Create(1.0 - amount));
#endif
            return Normalize(result.AsQuaternion());
        }

        /// <summary>Returns the quaternion that results from multiplying two quaternions together.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The product quaternion.</returns>
        public static QuaternionD Multiply(QuaternionD value1, QuaternionD value2) => value1 * value2;

        /// <summary>Returns the quaternion that results from scaling all the components of a specified quaternion by a scalar factor.</summary>
        /// <param name="value1">The source quaternion.</param>
        /// <param name="value2">The scalar value.</param>
        /// <returns>The scaled quaternion.</returns>
        public static QuaternionD Multiply(QuaternionD value1, double value2) => value1 * value2;

        /// <summary>Reverses the sign of each component of the quaternion.</summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>The negated quaternion.</returns>
        public static QuaternionD Negate(QuaternionD value) => -value;

        /// <summary>Divides each component of a specified <see cref="QuaternionD" /> by its length.</summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <returns>The normalized quaternion.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static QuaternionD Normalize(QuaternionD value) => (value.ToVector256() / value.Length()).AsQuaternion();

        /// <summary>Interpolates between two quaternions, using spherical linear interpolation.</summary>
        /// <param name="quaternion1">The first quaternion.</param>
        /// <param name="quaternion2">The second quaternion.</param>
        /// <param name="amount">The relative weight of the second quaternion in the interpolation.</param>
        /// <returns>The interpolated quaternion.</returns>
        public static QuaternionD Slerp(QuaternionD quaternion1, QuaternionD quaternion2, double amount) {
            const double SlerpEpsilon = 1e-6f;

            double cosOmega = Dot(quaternion1, quaternion2);
            double sign = 1.0;

            if (cosOmega < 0.0) {
                cosOmega = -cosOmega;
                sign = -1.0;
            }

            double s1, s2;

            if (cosOmega > (1.0 - SlerpEpsilon)) {
                // Too close, do straight linear interpolation.
                s1 = 1.0 - amount;
                s2 = amount * sign;
            } else {
                double omega = double.Acos(cosOmega);
                double invSinOmega = 1 / double.Sin(omega);

                s1 = double.Sin((1.0 - amount) * omega) * invSinOmega;
                s2 = double.Sin(amount * omega) * invSinOmega * sign;
            }

            return (quaternion1 * s1) + (quaternion2 * s2);
        }

        /// <summary>Subtracts each element in a second quaternion from its corresponding element in a first quaternion.</summary>
        /// <param name="value1">The first quaternion.</param>
        /// <param name="value2">The second quaternion.</param>
        /// <returns>The quaternion containing the values that result from subtracting each element in <paramref name="value2" /> from its corresponding element in <paramref name="value1" />.</returns>
        public static QuaternionD Subtract(QuaternionD value1, QuaternionD value2) => value1 - value2;

        /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
        /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="QuaternionD" /> object and the corresponding components of each matrix are equal.</remarks>
        public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is QuaternionD other) && Equals(other);

        /// <summary>Returns a value that indicates whether this instance and another quaternion are equal.</summary>
        /// <param name="other">The other quaternion.</param>
        /// <returns><see langword="true" /> if the two quaternions are equal; otherwise, <see langword="false" />.</returns>
        /// <remarks>Two quaternions are equal if each of their corresponding components is equal.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(QuaternionD other) => this.ToVector256().Equals(other.ToVector256());

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>The hash code.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override readonly int GetHashCode() => Unsafe.BitCast<QuaternionD, UInt256>(this).GetHashCode();

        /// <summary>Calculates the length of the quaternion.</summary>
        /// <returns>The computed length of the quaternion.</returns>
        public readonly double Length() => double.Sqrt(LengthSquared());

        /// <summary>Calculates the squared length of the quaternion.</summary>
        /// <returns>The length squared of the quaternion.</returns>
        public readonly double LengthSquared() => Dot(this, this);

        /// <summary>Returns a string that represents this quaternion.</summary>
        /// <returns>The string representation of this quaternion.</returns>
        /// <remarks>The numeric values in the returned string are formatted by using the conventions of the current culture. For example, for the en-US culture, the returned string might appear as <c>{X:1.1 Y:2.2 Z:3.3 W:4.4}</c>.</remarks>
        public override readonly string ToString() => $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";
    }
}

namespace UltimateOrb.Numerics.Accelerated {

    public readonly partial struct QuaternionD : IEquatable<QuaternionD> {

        public QuaternionD GetNormalized() {
            return new QuaternionD(this.XYZW.GetNormalized());
        }
    }
}
