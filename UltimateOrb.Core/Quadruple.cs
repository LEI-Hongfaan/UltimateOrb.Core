// Legacy macro compatibility.
#if IGNORE_NAN_PAYLOAD && !NAN_PERMISSIVE
#define NAN_PERMISSIVE
#endif
using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Serialization;
using System.Text;
using static UltimateOrb.Internal.System.Extensions;
using UltimateOrb.Mathematics;
using UltimateOrb.Numerics;
using UltimateOrb.Utilities;
using Number = UltimateOrb.Internal.System.Number;


#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using static UltimateOrb.Numerics.Binary128Arithmetic;
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using Binary128Arithmetic = UltimateOrb.Numerics.Binary128Arithmetic;
using UltimateOrb.Internal.System;

namespace UltimateOrb {

    [Experimental("UoWIP")]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public readonly partial struct Quadruple :
        IComparable,
        IConvertible,
        IComparable<Quadruple>,
        IEquatable<Quadruple>,
#if NET7_0_OR_GREATER
        IBinaryFloatingPointIeee754<Quadruple>,
        IMinMaxValue<Quadruple>,
#endif
#if NET8_0_OR_GREATER
        IUtf8SpanFormattable,
#endif
        ISpanFormattable {

#if USE_SIMD_VECTOR_BACKING_FIELD_FOR_LARGE_BASIC_NUMERIC_TYPES || !NET7_0_OR_GREATER
        readonly Vector128<UInt64> _Bits;
#else
        readonly System.UInt128 _Bits;
#endif

        readonly UInt64 _Lo64Bits {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if USE_SIMD_VECTOR_BACKING_FIELD_FOR_LARGE_BASIC_NUMERIC_TYPES || !NET7_0_OR_GREATER
            get => _Bits.GetElement(System.BitConverter.IsLittleEndian ? 0 : 1);
#else
            get => unchecked((UInt64)_Bits);
#endif
        }

        readonly UInt64 _Hi64Bits {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#if USE_SIMD_VECTOR_BACKING_FIELD_FOR_LARGE_BASIC_NUMERIC_TYPES || !NET7_0_OR_GREATER
            get => _Bits.GetElement(System.BitConverter.IsLittleEndian ? 1 : 0);
#else
            get => unchecked((UInt64)(_Bits >> 64));
#endif
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        Quadruple(UInt64 lo64Bits, UInt64 hi64Bits) {
#if USE_SIMD_VECTOR_BACKING_FIELD_FOR_LARGE_BASIC_NUMERIC_TYPES || !NET7_0_OR_GREATER
            _Bits = Vector128.Create(System.BitConverter.IsLittleEndian ? lo64Bits : hi64Bits, System.BitConverter.IsLittleEndian ? hi64Bits : lo64Bits);
#else
            _Bits = new System.UInt128(lower: lo64Bits, upper: hi64Bits);
#endif
        }

        public static Quadruple operator +(Quadruple value) {
            return value;
        }

        public static Quadruple operator -(Quadruple value) {
            return new Quadruple(value._Lo64Bits, 0x8000000000000000 ^ value._Hi64Bits);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple operator +(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Add(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Add(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Add(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Add(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
            return new Quadruple(Binary128Arithmetic.Add(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple operator -(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Subtract(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Subtract(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Subtract(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Subtract(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
            return new Quadruple(Binary128Arithmetic.Subtract(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
        }

        public static Quadruple operator *(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Multiply(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Multiply(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Multiply(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Multiply(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
            return new Quadruple(Binary128Arithmetic.Multiply(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
        }

        public static Quadruple operator /(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Divide(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Divide(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Divide(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Divide(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
            return new Quadruple(Binary128Arithmetic.Divide(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
        }

        public static Quadruple operator %(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Divide(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Remainder(Quadruple first, Quadruple second) {
            return new Quadruple(Binary128Arithmetic.Remainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static Quadruple Remainder(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
            return new Quadruple(Binary128Arithmetic.Remainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
        }
        #region Non-computational Operations
        #region IEEE Std 754
        /// <summary>
        /// The IEEE Std 754 <c>class</c>.
        /// class(x) tells which of the following ten classes x falls into... <see cref="FloatingPointClass"/>
        /// </summary>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static FloatingPointClass GetFloatingPointClass(Quadruple value) {
            return Binary128Arithmetic.GetFloatingPointClass(value._Lo64Bits, value._Hi64Bits);
        }

        /// <summary>
        ///     The IEEE Std 754 <c>isSignMinus</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if <paramref name="value"/> has negative sign.</returns>
        /// <remarks>Applies to zeros and NaNs as well.</remarks>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsNegative(Quadruple value) {
            return 0 > unchecked((Int64)value._Hi64Bits);
        }


        /// <summary>
        ///     The IEEE Std 754 <c>isNormal</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is normal (not zero, subnormal, infinite, or NaN).</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsNormal(Quadruple value) {
            var hi = 0x7FFF000000000000U & value._Hi64Bits;
            return 0x7FFF000000000000U > hi && hi > 0;
        }


        /// <summary>
        ///     The IEEE Std 754 <c>isFinite</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is zero, subnormal or normal (not infinite or NaN).</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsFinite(Quadruple value) {
            return 0x7FFF000000000000 != (0x7FFF000000000000 & value._Hi64Bits);
        }


        /// <summary>
        ///     The IEEE Std 754 <c>isZero</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is ±0.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsZero(Quadruple value) {
            return (value._Lo64Bits == 0 && unchecked((value._Hi64Bits & 0x7FFFFFFFFFFFFFFF) == 0x0000000000000000));
        }


        /// <summary>
        ///     The IEEE Std 754 <c>isSubnormal</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is subnormal.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsSubnormal(Quadruple value) {
            var hi = 0x7FFF000000000000U & value._Hi64Bits;
            return 0 == hi && (0 != value._Lo64Bits || 0 != (0x7FFFFFFFFFFFFFFFU & value._Hi64Bits));
        }

        /// <summary>
        ///     The IEEE Std 754 <c>isInfinite</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is infinite.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsInfinity(Quadruple value) {
            return (value._Lo64Bits == 0 && unchecked((value._Hi64Bits & 0x7FFFFFFFFFFFFFFF) == 0x7FFF000000000000));
        }

        /// <summary>
        ///     The IEEE Std 754 <c>isInfinite</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is a NaN.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsNaN(Quadruple value) {
            // (value._Hi64Bits & 0x7FFF000000000000) == 0x7FFF000000000000 && !IsInfinity(value);
            return DoubleArithmetic.GreaterThan(value._Lo64Bits, 0x7FFFFFFFFFFFFFFFu & value._Hi64Bits, 0u, 0x7FFF000000000000u);
        }

        //static bool IsNaN_A(Quadruple value) {
        //    return (0 == (0x7FFF000000000000U & ~value._Hi64Bits)) && (0 != value._Lo64Bits || 0 != (0x0000FFFFFFFFFFFF & value._Hi64Bits));
        //}

        /// <summary>
        ///     The IEEE Std 754 <c>isSignaling</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is a signaling NaN.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsSignalingNaN(Quadruple value) {
            UInt64 hi = value._Hi64Bits;
            UInt64 lo = value._Lo64Bits;

            return (hi & 0x7FFF800000000000UL) == 0x7FFF000000000000UL
                && ((hi & 0x00007FFFFFFFFFFFUL) != 0 || lo != 0);
        }

        /// <summary>
        ///     The IEEE Std 754 <c>isCanonical</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>True if and only if x is a finite number, infinity, or NaN that is canonical.</returns>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsCanonical(Quadruple value) {
            return true;
        }

        /// <summary>
        ///     The IEEE Std 754 <c>totalOrder</c>.
        /// </summary>
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsLessThanIEEETotalOrder(Quadruple first, Quadruple second) {
            return DoubleArithmetic.LessThan(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits);
        }

        /// <summary>
        ///     The IEEE Std 754 <c>totalOrderMag</c>.
        /// </summary>
        /// <returns>totalOrder(abs(x), abs(y))</returns>
        public static bool IsLessThanIEEETotalOrderMagnitude(Quadruple first, Quadruple second) {
            return DoubleArithmetic.LessThan(first._Lo64Bits, 0x7FFFFFFFFFFFFFFFu & first._Hi64Bits, second._Lo64Bits, 0x7FFFFFFFFFFFFFFFu & second._Hi64Bits);
        }

        #endregion

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsPositiveInfinity(Quadruple value) {
            return (value._Lo64Bits == 0 && value._Hi64Bits == 0x7FFF000000000000);
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool IsNegativeInfinity(Quadruple value) {
            return (value._Lo64Bits == 0 && value._Hi64Bits == 0xFFFF000000000000);
        }

        #endregion

        #region Equalities, Hashing and Comparisions

        public override bool Equals(object obj) {
            return obj is Quadruple quadruple && Equals(quadruple);
        }

        public bool Equals(Quadruple other) {
            if (IsNaN(this)) {
                return IsNaN(other);
            }
            if (IsNaN(other)) {
                return false;
            }
            return EqualsNonNaN(this, other);
        }

        static bool EqualsNonNaN(Quadruple first, Quadruple second) {
            return first._Lo64Bits == second._Lo64Bits && (first._Hi64Bits == second._Hi64Bits || (0 == first._Lo64Bits && 0 == (0x7FFFFFFFFFFFFFFFU & (first._Hi64Bits | second._Hi64Bits))));
        }

        public static bool operator ==(Quadruple first, Quadruple second) {
            if (IsNaN(first) || IsNaN(second)) {
                return false;
            }
            return EqualsNonNaN(first, second);
        }

        public static bool operator !=(Quadruple first, Quadruple second) {
            return !(first == second);
        }

        public static bool operator <(Quadruple first, Quadruple second) {
            if (IsNaN(first) || IsNaN(second)) {
                return false;
            }
            var first_sign = first.RawSign;
            var second_sign = second.RawSign;
            return (first_sign != second_sign) ?
                ((0 != first_sign) && (0 != ((0x7FFFFFFFFFFFFFFFU & (first._Hi64Bits | second._Hi64Bits)) | first._Lo64Bits | second._Lo64Bits))) :
                (((first._Hi64Bits != second._Hi64Bits) || (first._Lo64Bits != second._Lo64Bits)) && ((0 != first_sign) ^ DoubleArithmetic.LessThan(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits)));
        }

        public static bool operator >(Quadruple first, Quadruple second) {
            if (IsNaN(first) || IsNaN(second)) {
                return false;
            }
            var first_sign = first.RawSign;
            var second_sign = second.RawSign;
            return (first_sign != second_sign) ?
                ((0 == first_sign) && (0 != ((0x7FFFFFFFFFFFFFFFU & (first._Hi64Bits | second._Hi64Bits)) | first._Lo64Bits | second._Lo64Bits))) :
                (((first._Hi64Bits != second._Hi64Bits) || (first._Lo64Bits != second._Lo64Bits)) && ((0 == first_sign) == DoubleArithmetic.GreaterThan(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits)));
        }

        public static bool operator <=(Quadruple first, Quadruple second) {
            if (IsNaN(first) || IsNaN(second)) {
                return false;
            }
            var first_sign = first.RawSign;
            var second_sign = second.RawSign;
            return (first_sign != second_sign) ?
                ((0 != first_sign) || (0 == ((0x7FFFFFFFFFFFFFFFU & (first._Hi64Bits | second._Hi64Bits)) | first._Lo64Bits | second._Lo64Bits))) :
                (((first._Hi64Bits == second._Hi64Bits) && (first._Lo64Bits == second._Lo64Bits)) || ((0 != first_sign) ^ DoubleArithmetic.LessThan(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits)));
        }

        public static bool operator >=(Quadruple first, Quadruple second) {
            if (IsNaN(first) || IsNaN(second)) {
                return false;
            }
            var first_sign = first.RawSign;
            var second_sign = second.RawSign;
            return (first_sign != second_sign) ?
                ((0 == first_sign) || (0 == ((0x7FFFFFFFFFFFFFFFU & (first._Hi64Bits | second._Hi64Bits)) | first._Lo64Bits | second._Lo64Bits))) :
                (((first._Hi64Bits == second._Hi64Bits) && (first._Lo64Bits == second._Lo64Bits)) || ((0 == first_sign) == DoubleArithmetic.GreaterThan(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits)));
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public override int GetHashCode() {
            var lo = this._Lo64Bits;
            var hi = this._Hi64Bits;
            // x => IsNan(x) || IsZero(x)
            if (DoubleArithmetic.LessThanOrEqual(0, 0x7FFF000000000000U, DoubleArithmetic.DecreaseUnchecked(lo, hi, out var t_hi), 0x7FFFFFFFFFFFFFFFU & t_hi)) {
                return (0x7FFF000000000000U & hi).GetHashCode();
            }
            return (lo ^ hi).GetHashCode();
        }

        #endregion

        #region Constants

        public static Quadruple PositiveInfinity {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(0, 0x7FFF000000000000u);
        }

        static UInt128 PositiveInfinityBits {

            get => new UInt128(lo: 0, hi: 0x7FFF000000000000u);
        }

        public static Quadruple NaN {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(0, 0xFFFF800000000000u);
        }

        public static Quadruple NegativeInfinity {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(0, 0xFFFF000000000000u);
        }

        static UInt128 NegativeInfinityBits {

            get => new UInt128(lo: 0, hi: 0xFFFF000000000000u);
        }

        public static Quadruple MaxValue {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(0xFFFFFFFFFFFFFFFFu, 0x7FFEFFFFFFFFFFFFu);
        }

        public static Quadruple MinValue {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(0xFFFFFFFFFFFFFFFFu, 0xFFFEFFFFFFFFFFFFu);
        }

        public static Quadruple Epsilon {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => new Quadruple(1, 0);
        }

        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Quadruple PositiveZero {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new Quadruple(0, UInt64.MinValue);
            }
        }

        static UInt128 PositiveZeroBits {

            get {
                return new UInt128(0, UInt64.MinValue);
            }
        }

        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Quadruple NegativeZero {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new Quadruple(0, unchecked((UInt64)Int64.MinValue));
            }
        }

        static UInt128 NegativeZeroBits {

            get {
                return new UInt128(0, unchecked((UInt64)Int64.MinValue));
            }
        }
        #endregion

        #region Conversions

        /*
        <b>Infinity via the exponent field.</b> When rounding carries into the
        significand's hidden bit and the exponent is already at <c>emax</c>,
        the rebiased exponent field naturally becomes all ones
        (<c>0x7FFF</c>), which the format interprets as an infinity. The
        implementation relies on this and does not special-case the
        "carry at <c>emax</c>" condition. One renormalization plus the final
        <c>biased &gt;= QuadExpMax</c> test is sufficient.
         */
        /// <summary>
        /// Converts a value of an IEEE 754 binary interchange format to
        /// <see cref="Quadruple"/> (binary128).
        /// </summary>
        /// <typeparam name="TFloat">
        /// A radix-2 IEEE 754 interchange floating-point type. Supported storage
        /// widths are 16, 32, 64, 128, 160, 192, 224, and 256 bits.
        /// </typeparam>
        /// <typeparam name="TFloatUIntBits">
        /// An unsigned integer type with the same storage size as
        /// <typeparamref name="TFloat"/>. Used to inspect the raw bit pattern.
        /// </typeparam>
        /// <remarks>
        /// <para>
        /// <b>Supported source formats.</b> binary16, binary32, binary64,
        /// binary128, and the <c>k &gt;= 128, k % 32 == 0</c> family
        /// (binary160, binary192, binary224, binary256). Format parameters come
        /// from <see cref="BinaryFloatingPointIeee754TypeTraitsInternal{TFloat}"/>.
        /// </para>
        /// <para>
        /// <b>Widening (source &lt;= binary128).</b> Exact. No rounding occurs
        /// because the target has at least as much precision as the source.
        /// </para>
        /// <para>
        /// <b>Narrowing (source &gt; binary128).</b> Round-to-nearest, ties-to-even
        /// on the discarded significand bits, with a single renormalization on
        /// carry. Overflow yields <c>±∞</c>. Values rounding into the target
        /// subnormal range are handled with a second round-to-nearest, ties-to-even
        /// on the extra bits, including promotion to the smallest normal when the
        /// subnormal rounds up.
        /// </para>
        /// <para>
        /// <b>NaN payload handling — two modes.</b> The signaling/quiet bit is
        /// always at the MSB of the fraction field and is never treated as part of
        /// the payload.
        /// </para>
        /// <list type="bullet">
        ///   <item>
        ///     <description>
        ///     <b>Legacy mode</b> (<c>NAN_PAYLOAD_LEGACY</c> defined).
        ///     MSB-aligns the source fraction to the target, then folds a scatter
        ///     of the dropped bits back into the low positions via a bitwise OR,
        ///     using fold parameters that mirror the existing
        ///     <c>Quadruple → Double</c> converter (<c>foldShift = dstFracBits / 4</c>).
        ///     No class-change guard is needed: the OR is sufficient to keep the
        ///     destination nonzero whenever the source fraction was nonzero.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///     <b>Modern mode</b> (default; <c>NAN_PAYLOAD_LEGACY</c> not defined).
        ///     Treats the payload as an unsigned integer excluding the quiet bit
        ///     and LSB-aligns it across formats. Widening zero-extends. Narrowing
        ///     keeps the low bits of the source payload and jams the MSB of the
        ///     target payload to 1 iff any source payload bit at or above that
        ///     position was nonzero; this MSB is the payload-overflow indicator
        ///     for the concrete binary interchange format. The quiet bit is copied
        ///     verbatim. No class-change guard is needed: a signaling NaN source
        ///     necessarily has a nonzero payload (otherwise it would be an
        ///     infinity), so both widening and narrowing keep the destination
        ///     fraction nonzero.
        ///     </description>
        ///   </item>
        /// </list>
        /// <para>
        /// <b>Permissive NaN.</b> When <c>NAN_PERMISSIVE</c> is defined, the
        /// caller promises not to observe <em>which</em> NaN is produced, and the
        /// implementation is therefore free to return any NaN without inspecting
        /// the source. This permits a fast path that skips payload extraction,
        /// sign preservation, and the quiet/signaling classification. The
        /// particular NaN returned is not part of the contract: it may be
        /// positive or negative, quiet or signaling, with any payload, and may
        /// vary between runs, target frameworks, or generic instantiations of
        /// this method. Code that needs a specific sign, payload, or signaling
        /// class must leave this macro undefined and rely on one of the payload
        /// modes above. This option takes precedence over both modes.
        /// </para>
        /// <para>
        /// <b>Preconditions.</b> Internal method. Callers must ensure
        /// <typeparamref name="TFloat"/> is a supported radix-2 IEEE 754 binary
        /// interchange format and <typeparamref name="TFloatUIntBits"/> has the
        /// same storage size. Violations are caught by <see cref="Debug.Assert"/>
        /// in debug builds and produce undefined results in release builds.
        /// </para>
        /// </remarks>
        internal static Quadruple FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>(TFloat value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits>, IMinMaxValue<TFloatUIntBits> {
            // ---- Target (binary128) constants ----
            const int QuadFracBits = 112;
            const int QuadBias = 16383;
            const int QuadExpMax = 0x7FFF;
            const int QuadPayloadBits = QuadFracBits - 1;    // 111, excludes the quiet bit
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            const int QuadFoldShift = QuadFracBits / 4;    // 28; see XML remarks
#pragma warning restore CS0219 // Variable is assigned but its value is never used

            // ---- Guard rails (internal method: fail fast in Debug) ----
            Debug.Assert(Unsafe.SizeOf<TFloat>() == Unsafe.SizeOf<TFloatUIntBits>(),
                "T and TFloatUIntBits must have identical storage size.");
            Debug.Assert(TFloat.Radix == 2,
                "Only radix-2 (binary) interchange formats can be converted to binary128.");
            Debug.Assert(BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.IsSupported,
                $"'{typeof(TFloat).FullName}' is not a supported IEEE 754 interchange format.");
            unchecked {
                // ---- Source format parameters ----
                int srcFracBits = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.TrailingSignificandFieldBitWidth;   // t
                int srcExpBits = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.ExponentFieldBitWidth; // w
                int srcBias = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.MaxExponent;
                int srcExpMax = (1 << srcExpBits) - 1;

                Debug.Assert(srcFracBits > 0 && srcExpBits > 0);
                Debug.Assert(srcExpBits + srcFracBits + 1 == 8 * Unsafe.SizeOf<TFloat>());

                // ---- Extract raw fields ----
                TFloatUIntBits bits = UltimateOrb.Runtime.CompilerServices.Unsafe
                    .BitCast<TFloat, TFloatUIntBits>(value);

                TFloatUIntBits fracMask = (TFloatUIntBits.One << srcFracBits) - TFloatUIntBits.One;
                TFloatUIntBits expMask = (TFloatUIntBits.One << srcExpBits) - TFloatUIntBits.One;

                int srcExp = int.CreateTruncating((bits >> srcFracBits) & expMask);
                TFloatUIntBits srcFrac = bits & fracMask;

                bool negative = TFloat.IsNegative(value);
                UInt64 signHi = negative ? (1UL << 63) : 0UL;
                UInt64 infNanHi = signHi | ((UInt64)QuadExpMax << 48);

                // =================================================================
                // ±Infinity
                // =================================================================
                if (srcExp == srcExpMax && srcFrac == TFloatUIntBits.Zero) {
                    return new Quadruple(0UL, infNanHi);
                }

                // =================================================================
                // NaN
                // =================================================================
                if (srcExp == srcExpMax) {
#if NAN_PERMISSIVE
                    // ----------------------------------------------------------------
                    // Permissive NaN: the caller does not observe which NaN is
                    // produced, so we skip payload and sign extraction entirely and
                    // return a precomputed NaN. Any other NaN would also satisfy the
                    // contract; this one was chosen because it is the cheapest to
                    // materialize as a constant on every supported target.
                    // ----------------------------------------------------------------
                    return NaN;
#else
#if NAN_PAYLOAD_LEGACY

                    // ----------------------------------------------------------------
                    // Legacy convention: MSB-align + scatter-fold.
                    //
                    //   - Top QuadFracBits positions hold the MSB-aligned source
                    //     fraction (narrowing) or the zero-extended source fraction
                    //     (widening).
                    //   - Dropped bits are OR-folded back in as two disjoint chunks:
                    //       fold1: droppedBits >> foldShift   (high part of dropped)
                    //       fold2: droppedBits & foldMask     (low  part of dropped)
                    //     with foldShift = QuadFracBits / 4, matching the existing
                    //     Quadruple → Double converter's foldShift = 52 / 4 = 13.
                    //   - No class-change guard: for any NaN source, srcFrac != 0,
                    //     and the union of {MSB-align, fold1, fold2} covers every
                    //     dropped bit, so the destination fraction is guaranteed
                    //     nonzero.
                    // ----------------------------------------------------------------
                    UInt128 destFrac;
                    if (srcFracBits <= QuadFracBits) {
                        // Widening: exact, nothing dropped.
                        destFrac = UInt128.CreateTruncating(srcFrac) << (QuadFracBits - srcFracBits);
                    } else {
                        int drop = srcFracBits - QuadFracBits;

                        TFloatUIntBits dropMask = (TFloatUIntBits.One << drop) - TFloatUIntBits.One;

                        UInt128 shifted     = UInt128.CreateTruncating(srcFrac >> drop);
                        UInt128 droppedBits = UInt128.CreateTruncating(srcFrac & dropMask);

                        UInt128 fold1 = droppedBits >> QuadFoldShift;
                        UInt128 fold2 = droppedBits & (((UInt128)1 << QuadFoldShift) - 1);

                        destFrac = shifted | fold1 | fold2;
                    }

                    UInt64 legacyFracLo = (UInt64)destFrac;
                    UInt64 legacyFracHi = (UInt64)(destFrac >> 64) & 0x0000_FFFF_FFFF_FFFFUL;
                    return new Quadruple(legacyFracLo, infNanHi | legacyFracHi);
#else

                    // ----------------------------------------------------------------
                    // Modern convention: payload is an unsigned integer excluding the
                    // quiet bit; LSB-aligned across formats; narrowing jams the MSB
                    // of the target payload to 1 as the payload-overflow indicator.
                    // ----------------------------------------------------------------
                    int srcPayloadBits = srcFracBits - 1;

                    bool isQuiet = (srcFrac & (TFloatUIntBits.One << (srcFracBits - 1)))
                                   != TFloatUIntBits.Zero;

                    UInt128 srcPayload = System.UInt128.CreateTruncating(
                        srcFrac & ((TFloatUIntBits.One << (srcFracBits - 1)) - TFloatUIntBits.One));

                    UInt128 dstPayload;
                    if (srcPayloadBits <= QuadPayloadBits) {
                        // Widening / equal: integer value preserved, zero-extended.
                        dstPayload = srcPayload;
                    } else {
                        // Narrowing: low bits kept; high bits set the overflow flag.
                        UInt128 lowMask = ((UInt128)1 << (QuadPayloadBits - 1)) - 1;
                        UInt128 lowBits = srcPayload & lowMask;
                        UInt128 highBits = srcPayload >> (QuadPayloadBits - 1);

                        dstPayload = lowBits;
                        if (highBits != UInt128.Zero) {
                            dstPayload |= (UInt128)1 << (QuadPayloadBits - 1);
                        }
                    }

                    UInt128 destFrac = dstPayload;
                    if (isQuiet) {
                        destFrac |= (UInt128)1 << (QuadFracBits - 1);
                    }
                    // No class-change guard: sNaN ⇒ payload ≠ 0; qNaN ⇒ quiet bit set.

                    UInt64 modernFracLo = (UInt64)destFrac;
                    UInt64 modernFracHi = (UInt64)(destFrac >> 64) & 0x0000_FFFF_FFFF_FFFFUL;
                    return new Quadruple(modernFracLo, infNanHi | modernFracHi);

#endif // NAN_PAYLOAD_LEGACY
#endif // !NAN_PERMISSIVE
                }

                // =================================================================
                // ±0
                // =================================================================
                if (srcExp == 0 && srcFrac == TFloatUIntBits.Zero) {
                    return new Quadruple(0UL, signHi);
                }

                // =================================================================
                // Finite nonzero: build the source significand with implicit 1 at
                // bit srcFracBits, plus the unbiased exponent.
                // =================================================================
                TFloatUIntBits sig;
                int unbiased;

                if (srcExp == 0) {
                    // Source subnormal → normalize.
                    int totalBits = 8 * Unsafe.SizeOf<TFloatUIntBits>();
                    int lz = int.CreateTruncating(TFloatUIntBits.LeadingZeroCount(srcFrac)) - (totalBits - srcFracBits);
                    sig = srcFrac << (lz + 1);
                    unbiased = -srcBias - lz;
                } else {
                    sig = (TFloatUIntBits.One << srcFracBits) | srcFrac;
                    unbiased = srcExp - srcBias;
                }

                // =================================================================
                // Align MSB of sig with the target's implicit-1 position (bit 112).
                //   shift <= 0 : widening (exact).
                //   shift >  0 : narrowing (round-half-to-even on discarded bits).
                // =================================================================
                int shift = srcFracBits - QuadFracBits;
                UInt128 sig128;

                if (shift <= 0) {
                    sig128 = System.UInt128.CreateTruncating(sig) << (-shift);
                } else {
                    TFloatUIntBits mask = (TFloatUIntBits.One << shift) - TFloatUIntBits.One;
                    TFloatUIntBits dropped = sig & mask;
                    TFloatUIntBits top = sig >> shift;
                    TFloatUIntBits half = TFloatUIntBits.One << (shift - 1);

                    if (dropped > half ||
                        (dropped == half && (top & TFloatUIntBits.One) != TFloatUIntBits.Zero)) {
                        top = top + TFloatUIntBits.One;

                        // Renormalize if the carry reached bit 113. When the exponent
                        // is already at emax, the resulting biased exponent becomes
                        // 0x7FFF (all ones) and is caught by the
                        // `biased >= QuadExpMax` test below — the infinity case needs
                        // no special treatment here.
                        if (top >= (TFloatUIntBits.One << (QuadFracBits + 1))) {
                            top = top >> 1;
                            unbiased += 1;
                        }
                    }
                    sig128 = System.UInt128.CreateTruncating(top);
                }

                int biased = unbiased + QuadBias;

                // =================================================================
                // Overflow → ±Infinity.
                //
                // Reached either when the source exponent was already above binary128's
                // range, or when rounding carried the significand past emax. In both
                // cases the value's exponent field becomes all ones (0x7FFF), so this
                // single comparison produces the correct ±∞.
                // =================================================================
                if (biased >= QuadExpMax) {
                    return new Quadruple(0UL, infNanHi);
                }

                // =================================================================
                // Target subnormal: extra right shift with round-half-to-even.
                // Only reachable for source formats wider than binary128.
                // =================================================================
                if (biased <= 0) {
                    int drop = 1 - biased;
                    if (drop > 128) {
                        return new Quadruple(0UL, signHi);
                    }

                    UInt128 top, dropped, half;
                    if (drop == 128) {
                        top = UInt128.Zero;
                        dropped = sig128;
                        half = (UInt128)1 << 127;
                    } else {
                        top = sig128 >> drop;
                        dropped = sig128 & (((UInt128)1 << drop) - 1);
                        half = (UInt128)1 << (drop - 1);
                    }

                    if (dropped > half || (dropped == half && (top & 1) != 0)) {
                        top++;
                    }

                    // Subnormal rounding may promote to the smallest normal:
                    // exponent field = 1, fraction = 0.
                    if (top == ((UInt128)1 << QuadFracBits)) {
                        return new Quadruple(0UL, signHi | (1UL << 48));
                    }

                    UInt64 fracLo = (UInt64)top;
                    UInt64 fracHi = (UInt64)(top >> 64) & 0x0000_FFFF_FFFF_FFFFUL;
                    return new Quadruple(fracLo, signHi | fracHi);
                }

                // =================================================================
                // Target normal
                // =================================================================
                {
                    UInt128 frac = sig128 & (((UInt128)1 << QuadFracBits) - 1);
                    UInt64 fracLo = (UInt64)frac;
                    UInt64 fracHi = (UInt64)(frac >> 64) & 0x0000_FFFF_FFFF_FFFFUL;
                    return new Quadruple(fracLo, signHi | ((UInt64)biased << 48) | fracHi);
                }
            }

        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator Quadruple(Double value) {
            var lo = unchecked((UInt64)System.BitConverter.DoubleToInt64Bits(value));
            var e = unchecked(((int)(lo >> 52) & ((1 << 11) - 1)) + (0x3FFF - (1024 - 1)));
            UInt64 hi = 0x8000000000000000u & lo;
            lo &= ((UInt64)1 << 52) - 1;
            if ((0x3FFF - (1024 - 1)) != e) {
                if (((1 << 11) - 1) + (0x3FFF - (1024 - 1)) != e) {
                } else {
                    e = 0x7FFF;
                }
            } else {
                if (lo <= 1) {
                    return new Quadruple(0, (unchecked((UInt64)(-(Int64)lo)) & ((UInt64)(1 + (0x3FFF - (1024 - 1)) - 52) << (FractionBitCount - 64))) | hi);
                }
                var c = BinaryNumerals.CountLeadingZeros(lo);
                unchecked {
                    // e += 1 + (64 - 52) - (1 + c);
                    e += (64 - 52) - c;
                }
                lo <<= unchecked(1 + c);
                lo >>= (64 - 52);
            }
            hi |= unchecked((UInt64)e) << (FractionBitCount - 64);
            return new Quadruple(lo << (FractionBitCount - 52), (lo >> (64 - (FractionBitCount - 52))) | hi);
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Double(Quadruple value) {
            var e = unchecked(((int)(value._Hi64Bits >> (FractionBitCount - 64)) & ((1 << ExponentBitCount) - 1)) + ((1024 - 1) - 0x3FFF));
            UInt64 hi = 0x8000000000000000u & value._Hi64Bits;
            var lo = Hi64BitsFractionMask & value._Hi64Bits;
            var t = value._Lo64Bits;
            if (e > 0) {
                if (0x7FF > e) {
                    hi |= unchecked((UInt64)e) << 52;
                    lo = lo << 6 | t >> 58 | ((UInt64)(t << 6) == 0 ? 0u : 1u);
                } else {
                    goto L_NF;
                }
            } else {
                goto L_Sub;
            }
        L_0:;
            lo = unchecked((lo >> 2) + (1u & (0Xc8U >> (7 & (int)lo))));
        L_1:;
            return System.BitConverter.Int64BitsToDouble(unchecked((Int64)(hi | lo)));
        L_Sub:;
            {
                // Subnormal or Zero
                if ((1024 - 1) - 0x3FFF != e || 0 != (lo | t)) {
                    lo |= Hi64BitsImplicitBit;
                    var count = unchecked(64 - 2 + -3 - e);
                    UInt64 s;
                    if (count < 64) {
                        var minus_count = unchecked(-count);
                        lo = lo << (/*63 & */minus_count) | t >> count | ((UInt64)(t << (/*63 & */minus_count)) == 0 ? 0u : 1u);
                    } else {
                        lo = (count < 127) ?
                            lo >> (/*63 & */count) | (((lo & unchecked(((UInt64)1 << (/*63 & */count)) - 1)) | t) == 0 ? 0u : 1u) :
                            (0 == (lo | t) ? 0u : 1u);
                    }
                    goto L_0;
                } else {
                    lo = 0;
                    goto L_1;
                }
            }
        L_NF:;
            hi |= 0x7FF0000000000000u;
            if (((1 << ExponentBitCount) - 1) + ((1024 - 1) - 0x3FFF) != e) {
                lo = 0;
            } else {
                lo = (lo << 4) | (t >> 60) | (0x0FFFFFFFFFFFFFFFu & t) >> 13 | (0x1FFFu & t);
            }
            goto L_1;
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator Quadruple(Int64 value) {
            if (0 != value) {
                var a = unchecked((UInt64)(0 > value ? -value : value));
                if (1 == a) {
                    return new Quadruple(0, (0x8000000000000000u & unchecked((UInt64)value)) ^ 0x3FFF000000000000u);
                }
                var c = BinaryNumerals.CountLeadingZeros(a);
                // 1    63
                // 0x3FFF000000000000 0000000000000000
                // 2    62
                // 0x4000000000000000 0000000000000000
                a <<= unchecked(1 + c);
                return new Quadruple(a << (FractionBitCount - 64), (0x8000000000000000u & unchecked((UInt64)value)) ^ ((unchecked((UInt64)(0x3FFF + (64 - 1) - c)) << (FractionBitCount - 64)) | (a >> (128 - FractionBitCount))));
            }
            return default;
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator Quadruple(UInt64 value) {
            var a = value;
            if (2 <= a) {
                var c = BinaryNumerals.CountLeadingZeros(a);
                a <<= unchecked(1 + c);
                return new Quadruple(a << (FractionBitCount - 64), (unchecked((UInt64)(0x3FFF + (64 - 1) - c)) << (FractionBitCount - 64)) | (a >> (128 - FractionBitCount)));
            }
            if (0 == a) {
                return default;
            }
            return new Quadruple(0, 0x3FFF000000000000u);
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt64(Quadruple value) {
            if (0 <= unchecked((Int64)value._Hi64Bits)) {
                _ = checked(0xBFC1000000000000u + unchecked((UInt64)value._Hi64Bits));
                var e = 0x7FFF & unchecked((int)(value._Hi64Bits >> (FractionBitCount - 64)));
                var hi = 0x0001000000000000U | (Hi64BitsFractionMask & value._Hi64Bits);
                var count = unchecked(0x402F - e);
                if (0 > count) {
                    return (hi << unchecked(-count)) | (value._Lo64Bits >> (/*63 & */count));
                } else {
                    if (49 <= count) {
                        return 0;
                    }
                    return hi >> count;
                }
            }
            {
                _ = checked(0x4001000000000000u + unchecked((UInt64)value._Hi64Bits));
                return 0;
            }
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt32(Quadruple value) {
            if (0 <= unchecked((Int64)value._Hi64Bits)) {
                _ = checked(0xBFE1000000000000u + unchecked((UInt64)value._Hi64Bits));
                var e = 0x7FFF & unchecked((int)(value._Hi64Bits >> (FractionBitCount - 64)));
                var hi = 0x0001000000000000U | (Hi64BitsFractionMask & value._Hi64Bits);
                var count = unchecked(0x402F - e);
                if (49 <= count) {
                    return 0;
                }
                return unchecked((UInt32)(hi >> count));
            }
            {
                _ = checked(0x4001000000000000u + unchecked((UInt64)value._Hi64Bits));
                return 0;
            }
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int32(Quadruple value) {
            var e = 0x7FFF & unchecked((int)(value._Hi64Bits >> (FractionBitCount - 64)));
            var hi = (Hi64BitsFractionMask & value._Hi64Bits) | (0 == value._Lo64Bits ? 0u : 1u);
            var count = unchecked(0x402F - e);
            if (49 <= count) {
                return 0;
            }
            var sign = value._Hi64Bits >> 63;
            if (count < 18) {
                if ((0 != sign) && (count == 17)) {
                    // 0x0000000000020000u > hi
                    _ = checked(hi + unchecked((UInt64)(-(Int64)0x0000000000020000u)));
                    return unchecked((Int32)0x80000000);
                }
                {
                    UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThan(count, 18);
                    throw null!;
                }
            }
            hi |= 0x0001000000000000u;
            var fa = unchecked((Int32)(hi >> count));
            return (0 != sign) ? unchecked(-fa) : fa;
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int64(Quadruple value) {
            var e = 0x7FFF & unchecked((int)(value._Hi64Bits >> (FractionBitCount - 64)));
            var hi = (Hi64BitsFractionMask & value._Hi64Bits) | (0 == value._Lo64Bits ? 0u : 1u);
            var sign = value._Hi64Bits >> 63;
            var count = unchecked(0x402F - e);
            Int64 fa;
            if (count < 0) {
                if (count < -14) {
                    // 0xC03E000000000000 == hi
                    _ = checked(-unchecked((Int64)(hi - 0x403E000000000000)));
                    // 0x0002000000000000 > lo
                    _ = checked(hi + unchecked((UInt64)(-(Int64)0x0002000000000000)));
                    return unchecked((Int64)0x8000000000000000);
                }
                hi |= 0x0001000000000000u;
                var minus_count = unchecked(-count);
                fa = unchecked((Int64)((hi << minus_count) | (value._Lo64Bits >> (/*63 & */count))));
            } else {
                if (49 <= count) {
                    return 0;
                }
                hi |= 0x0001000000000000u;
                fa = unchecked((Int64)(hi >> count));
            }
            return (0 != sign) ? unchecked(-fa) : fa;
        }


        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator Quadruple(Int32 value) {
            if (0 != value) {
                var a = unchecked((UInt32)(0 > value ? -value : value));
                if (1 == a) {
                    return new Quadruple(0, (0x8000000000000000u & unchecked((UInt64)(Int64)value)) ^ 0x3FFF000000000000u);
                }
                var c = BinaryNumerals.CountLeadingZeros(unchecked((UInt32)a));
                // 1    63
                // 0x3FFF000000000000 0000000000000000
                // 2    62
                // 0x4000000000000000 0000000000000000
                a <<= unchecked(1 + c);
                return new Quadruple(0, (0x8000000000000000u & unchecked((UInt64)(Int64)value)) ^ ((unchecked((UInt64)(0x3FFF + (32 - 1) - c)) << (FractionBitCount - 64)) | ((UInt64)a << ((FractionBitCount - 64) - 32))));
            }
            return default;
        }

        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator Quadruple(UInt32 value) {
            var a = value;
            if (2 <= a) {
                var c = BinaryNumerals.CountLeadingZeros(a);
                a <<= unchecked(1 + c);
                return new Quadruple(0, (unchecked((UInt64)(0x3FFF + (32 - 1) - c)) << (FractionBitCount - 64)) | ((UInt64)a << ((FractionBitCount - 64) - 32)));
            }
            if (0 == a) {
                return default;
            }
            return new Quadruple(0, 0x3FFF000000000000u);
        }

        public static implicit operator Quadruple(Int16 value) {
            return (Quadruple)(int)value;
        }

        public static implicit operator Quadruple(UInt16 value) {
            return (Quadruple)(uint)value;
        }

        public static implicit operator Quadruple(byte value) {
            return (Quadruple)(uint)value;
        }

        public static implicit operator Quadruple(sbyte value) {
            return (Quadruple)(int)value;
        }

        public string ToString(string? format) => ToString(format, null);

        public string ToString(string? format, IFormatProvider? provider) {
            var @this = this;

            foreach (var i in (ReadOnlySpan<int>)[64, 256]) {
                var (flowControl, value) = TryToStringLocal(@this, format, provider, i);
                if (!flowControl) {
                    return value!;
                }
            }

            for (int i = 1024; i < Array.MaxLength; i *= 4) {
                var (flowControl, value) = TryToStringArrayPool(@this, format, provider, i);
                if (!flowControl) {
                    return value!;
                }
            }
            {
                var i = Array.MaxLength;
                var (flowControl, value) = TryToStringArrayPool(@this, format, provider, i);
                if (!flowControl) {
                    return value!;
                }
                throw new FormatException("The format produced too large.");
            }

            static (bool flowControl, string? value) TryToStringLocal(Quadruple @this, string? format, IFormatProvider? provider, int bufferSize) {
                Span<char> stack = stackalloc char[bufferSize];
                if (@this.TryFormat(stack, out int n, format.AsSpan(), provider)) return (flowControl: false, value: new string(stack[..n]));
                return (flowControl: true, value: null);
            }

            static (bool flowControl, string? value) TryToStringArrayPool(Quadruple @this, string? format, IFormatProvider? provider, int bufferSize) {
                var heap = ArrayPool<char>.Shared.Rent(bufferSize);
                try {
                    if (@this.TryFormat(heap, out int n, format.AsSpan(), provider)) {
                        return (flowControl: false, value: new string(heap, 0, n));
                    }
                    return (flowControl: true, value: null);
                } finally {
                    ArrayPool<char>.Shared.Return(heap);
                }
            }
        }



        public string ToString(IFormatProvider? provider) => ToString(null, provider);

        public override string ToString() => ToString(null, null);
        #endregion

        public static bool IsPow2(Quadruple value) {
            // Assume Quadruple stores the raw 128-bit IEEE‑754 binary128 payload
            // as two ulongs: High (bits 64..127) and Low (bits 0..63).
            // Layout:
            //  sign:    bit 127
            //  exponent: bits 112..126 (15 bits)
            //  fraction: bits 0..111 (112 bits)

            UInt64 hi = value._Hi64Bits;
            UInt64 lo = value._Lo64Bits;

            // Sign bit: 0 => positive, 1 => negative
            bool isNegative = ((hi >> 63) & 1UL) != 0UL;
            if (isNegative) {
                return false;
            }

            // Exponent (15 bits)
            uint exponent = (uint)((hi >> 48) & 0x7FFFu);

            // Special cases: zero, subnormal, Inf/NaN
            if (exponent == 0u) {
                // zero or subnormal
                return false;
            }

            if (exponent == 0x7FFFu) {
                // Inf or NaN
                return false;
            }

            // Fraction (112 bits) is composed of the low 48 bits of hi and all of lo
            ulong fracHigh = hi & 0x0000_FFFF_FFFF_FFFFUL;
            bool fractionIsZero = (fracHigh == 0UL) && (lo == 0UL);

            // A normalized IEEE floating power of two has fraction == 0
            return fractionIsZero;
        }

        public static Quadruple Exp(Quadruple x) {
            var lo = Binary128Arithmetic.Exp(x._Lo64Bits, x._Hi64Bits, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Exp(Quadruple x, MidpointRounding mode) {
            var lo = Binary128Arithmetic.Exp(x._Lo64Bits, x._Hi64Bits, mode, out var hi);
            return new Quadruple(lo, hi);
        }

        internal static Quadruple PiOverTwo {
            // 0x3FFF921FB54442D18469898CC51701B8
            get => new Quadruple(
                lo64Bits: 0x8469898CC51701B8UL,
                hi64Bits: 0x3FFF921FB54442D1UL);
        }

        internal static Quadruple Log10OfE {
            // 0x3FFDBCB7B1526E50E32A6AB7555F5A68
            get => new Quadruple(
                lo64Bits: 0xE32A6AB7555F5A68UL,
                hi64Bits: 0x3FFDBCB7B1526E50UL);
        }

        internal static Quadruple Log2OfE {
            // 0x3FFF71547652B82FE1777D0FFDA0D23A
            get => new Quadruple(
                lo64Bits: 0xE1777D0FFDA0D23AUL,
                hi64Bits: 0x3FFF71547652B82FUL);
        }

        internal static Quadruple LogOf10 {
            // 0x400026BB1BBB5551582DD4ADAC5705A6
            get => new Quadruple(
                lo64Bits: 0x582DD4ADAC5705A6UL,
                hi64Bits: 0x400026BB1BBB5551UL);
        }

        internal static Quadruple LogOf2 {
            // 0x3FFE62E42FEFA39EF35793C7673007E6
            get => new Quadruple(
                lo64Bits: 0xF35793C7673007E6UL,
                hi64Bits: 0x3FFE62E42FEFA39EUL);
        }

        public static Quadruple Exp10(Quadruple x) {
            // TODO: Provide a correct impl
            return Exp(LogOf10 * x);
        }

        public static Quadruple Exp2(Quadruple x) {
            // TODO: Provide a correct impl
            return Exp(LogOf2 * x);
        }

        // ---------------------------------------------------------------------
        // BCL generic-math bitwise operators for floating-point types: raw bit manipulation.
        //
        // They are pure functions of the 128-bit interchange encoding: no NaN
        // propagation, no sign/magnitude decomposition, just the raw bit patterns.
        // ---------------------------------------------------------------------

        static Quadruple IBitwiseOperators<Quadruple, Quadruple, Quadruple>.operator &(Quadruple left, Quadruple right) {
            UInt128 l = BitConverter.QuadrupleToUInt128Bits(left);
            UInt128 r = BitConverter.QuadrupleToUInt128Bits(right);
            return BitConverter.UInt128BitsToQuadruple(l & r);
        }

        static Quadruple IBitwiseOperators<Quadruple, Quadruple, Quadruple>.operator |(Quadruple left, Quadruple right) {
            UInt128 l = BitConverter.QuadrupleToUInt128Bits(left);
            UInt128 r = BitConverter.QuadrupleToUInt128Bits(right);
            return BitConverter.UInt128BitsToQuadruple(l | r);
        }

        static Quadruple IBitwiseOperators<Quadruple, Quadruple, Quadruple>.operator ^(Quadruple left, Quadruple right) {
            UInt128 l = BitConverter.QuadrupleToUInt128Bits(left);
            UInt128 r = BitConverter.QuadrupleToUInt128Bits(right);
            return BitConverter.UInt128BitsToQuadruple(l ^ r);
        }

        static Quadruple IBitwiseOperators<Quadruple, Quadruple, Quadruple>.operator ~(Quadruple value) {
            return BitConverter.UInt128BitsToQuadruple(~BitConverter.QuadrupleToUInt128Bits(value));
        }

        /*
        public static Quadruple operator &(Quadruple first, Quadruple second) {
            throw new NotImplementedException();
        }

        public static Quadruple operator |(Quadruple first, Quadruple second) {
            throw new NotImplementedException();
        }

        public static Quadruple operator ^(Quadruple first, Quadruple second) {
            throw new NotImplementedException();
        }

        public static Quadruple operator ~(Quadruple value) {
            throw new NotImplementedException();
        }
        */

        #region Misc.
        UInt64 RawFractionHi {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => unchecked((UInt64)(Hi64BitsFractionMask & _Hi64Bits));
        }

        UInt64 RawFractionLo {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get => _Lo64Bits;
        }

        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int RawSign {

            get => unchecked((int)(((Int64)this._Hi64Bits) >> 63));
        }

        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int RawExponent {

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get => unchecked(0x7FFF & (int)(((Int64)this._Hi64Bits) >> (FractionBitCount - 64)));
        }
        #endregion

        public static partial class Math {

            public static Quadruple ScaleB(Quadruple x, int n) {
                return ScaleB(x, n, FloatingPointRounding.ToNearestWithMidpointToEven);
            }

            public static Quadruple ScaleB(Quadruple x, int n, [ConstantExpected] FloatingPointRounding rounding) {
                return new Quadruple(Binary128Arithmetic.ScaleB(x._Lo64Bits, x._Hi64Bits, n, rounding, out var t), t);
            }

            public static Quadruple BitDecrement(Quadruple x) {
                var lo = x._Lo64Bits;
                var hi = x._Hi64Bits;

                if (0x7FFF000000000000u <= (0x7FFFFFFFFFFFFFFFu & hi)) {
                    // +Infinity |--> Quadruple.MaxValue
                    // NaN |--> NaN
                    // -Infinity |--> -Infinity
                    return (0 == lo && hi == 0x7FFF000000000000u) ? Quadruple.MaxValue : x;
                }

                if (0 == lo && 0 == hi) {
                    // +0.0 returns -Quadruple.Epsilon
                    return -Quadruple.Epsilon;
                }

                // Negative values need to be incremented
                // Positive values need to be decremented
                if (0 > unchecked((Int64)hi)) {
                    lo = DoubleArithmetic.IncreaseUnchecked(lo, hi, out hi);
                } else {
                    lo = DoubleArithmetic.DecreaseUnchecked(lo, hi, out hi);
                }
                return new Quadruple(lo, hi);
            }

            public static Quadruple BitIncrement(Quadruple x) {
                var lo = x._Lo64Bits;
                var hi = x._Hi64Bits;

                if (0x7FFF000000000000u <= (0x7FFFFFFFFFFFFFFFu & hi)) {
                    // -Infinity |--> Quadruple.MinValue
                    // NaN |--> NaN
                    // +Infinity |--> +Infinity
                    return (0 == lo && hi == 0xFFFF000000000000u) ? Quadruple.MinValue : x;
                }

                if (0 == lo && 0x8000000000000000u == hi) {
                    // -0.0 returns Quadruple.Epsilon
                    return Quadruple.Epsilon;
                }

                // Negative values need to be decremented
                // Positive values need to be incremented
                if (0 > unchecked((Int64)hi)) {
                    lo = DoubleArithmetic.DecreaseUnchecked(lo, hi, out hi);
                } else {
                    lo = DoubleArithmetic.IncreaseUnchecked(lo, hi, out hi);
                }
                return new Quadruple(lo, hi);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Quadruple Clamp(Quadruple value, Quadruple min, Quadruple max) {
                if (min > max) {
                    _ = ThrowMinMaxException(min, max);
                }

                if (value < min) {
                    return min;
                } else if (value > max) {
                    return max;
                }

                return value;
            }

            public static Quadruple Max(Quadruple val1, Quadruple val2) {
                if (Quadruple.IsNaN(val1)) {
                    return val1;
                }
                if (Quadruple.IsNaN(val2)) {
                    return val2;
                }
                var val1_lo = _ToLinearMinMax(val1._Lo64Bits, val1._Hi64Bits, out var val1_hi);
                var val2_lo = _ToLinearMinMax(val2._Lo64Bits, val2._Hi64Bits, out var val2_hi);
                if (DoubleArithmetic.GreaterThan(val1_lo, unchecked((Int64)val1_hi), val2_lo, unchecked((Int64)val2_hi))) {
                    return val1;
                } else {
                    return val2;
                }
            }

            public static Quadruple Max_A(Quadruple val1, Quadruple val2) {

                if ((val1 > val2) || Quadruple.IsNaN(val1)) {
                    return val1;
                }

                if (val1 == val2) {
                    return Quadruple.IsNegative(val1) ? val2 : val1;
                }

                return val2;
            }

            static UInt64 _ToLinearMinMax(UInt64 value_lo, UInt64 value_hi, out Int64 result_hi) {
                if (0 <= unchecked((Int64)value_hi)) {
                    result_hi = unchecked((Int64)value_hi);
                    return value_lo;
                } else {
                    return DoubleArithmetic.SubtractUnchecked(0xffffffffffffffffu, 0x7fffffffffffffff, value_lo, unchecked((Int64)value_hi), out result_hi);
                }
            }


            public static Quadruple Min(Quadruple val1, Quadruple val2) {
                if (Quadruple.IsNaN(val1)) {
                    return val1;
                }
                if (Quadruple.IsNaN(val2)) {
                    return val2;
                }
                var val1_lo = _ToLinearMinMax(val1._Lo64Bits, val1._Hi64Bits, out var val1_hi);
                var val2_lo = _ToLinearMinMax(val2._Lo64Bits, val2._Hi64Bits, out var val2_hi);
                if (DoubleArithmetic.LessThan(val1_lo, unchecked((Int64)val1_hi), val2_lo, unchecked((Int64)val2_hi))) {
                    return val1;
                } else {
                    return val2;
                }
            }

            public static Quadruple Min_A(Quadruple val1, Quadruple val2) {

                if ((val1 < val2) || Quadruple.IsNaN(val1)) {
                    return val1;
                }

                if (val1 == val2) {
                    return Quadruple.IsNegative(val1) ? val1 : val2;
                }

                return val2;
            }

            public static Quadruple CopySign(Quadruple x, Quadruple y) {
                var hi = 0x7fffffffffffffffu & x._Hi64Bits;
                var s = 0x8000000000000000u & y._Hi64Bits;
                return new Quadruple(x._Lo64Bits, hi | s);
            }

            public static Quadruple LogB(Quadruple x) {
                var x_lo = x._Lo64Bits;
                var x_hi = 0x7fffffffffffffffu & x._Hi64Bits;
                if ((x_hi | x_lo) == 0) {
                    return Quadruple.NegativeInfinity;
                }
                if (0x7fff000000000000u <= x_hi) {
                    if (0x7fff000000000000u == x_hi && 0 == x_lo) {
                        return Quadruple.PositiveInfinity;
                    }
                    return x;
                }
                var e = unchecked((int)(x_hi >> 48));
                if (0 == e) {
                    var t = x_hi;
                    if (0 == x_hi) {
                        // e -= 48;
                        e -= 48 + 31;
                    } else {
                        t = x_lo;
                        // e += 16;
                        e -= -16 + 31;
                    }
                    {
                        var v = unchecked((UInt32)(t >> 32));
                        // var r = 31;
                        if (0u == v) {
                            v = unchecked((UInt32)t);
                            // r = 63;
                            e -= 32;
                        }
                        if (v > 0xFFFFu) {
                            v >>= 16;
                            unchecked {
                                // r -= 16;
                                e += 16;
                            }
                        }
                        if (v > 0xFFu) {
                            v >>= 8;
                            unchecked {
                                // r -= 8;
                                e += 8;
                            }
                        }
                        if (v > 0xFu) {
                            v >>= 4;
                            unchecked {
                                // r -= 4;
                                e += 4;
                            }
                        }
                        {
                            unchecked {
                                // r -= 0x3 & unchecked((int)((Int32)0b11111111_11111111_10101010_01010011 >> ((int)v << 1)));
                                e += 0x3 & unchecked((int)((Int32)0b11111111_11111111_10101010_01010011 >> ((int)v << 1)));
                            }
                        }
                        unchecked {
                            // e -= r;
                        }
                    }
                }
                return (Quadruple)unchecked(e - 16383);
            }

            public static int ILogB(Quadruple x) {
                var x_hi = 0x7fffffffffffffffu & x._Hi64Bits;
                if (x_hi <= 0x0001000000000000u) {
                    var x_lo = x._Lo64Bits;
                    if (0 == (x_hi | x_lo)) {
                        return int.MinValue;
                    } else {
                        int result;
                        // Subnormal
                        if (0 == x_hi) {
                            for (result = -0x3fff - (FractionBitCount - 64); unchecked((Int64)x_lo) > 0; x_lo <<= 1) {
                                unchecked {
                                    --result;
                                }
                            }
                        } else {
                            for (x_hi <<= 16, result = -0x3fff; unchecked((Int64)x_hi) > 0; x_hi <<= 1) {
                                unchecked {
                                    --result;
                                }
                            }
                        }
                        return result;
                    }
                } else if (0x7fff000000000000u > x_hi) {
                    return unchecked((int)(x_hi >> (FractionBitCount - 64)) - 0x3fff);
                }
                {
                    // +/- Infinity, NaN
                    return int.MaxValue;
                }
            }



            public static int Sign(Quadruple value) {
                var value_e = 0x7FFF000000000000 & value._Hi64Bits;
                if (0x7FFF000000000000 != value_e) {
                    return IsZero(value) ? 0 : 0 > unchecked((Int64)value._Hi64Bits) ? -1 : 1;
                } else {
                    if (0 == value._Lo64Bits) {
                        if (0x7FFFFFFFFFFFFFFF == value._Hi64Bits) {
                            return 1;
                        } else if (0xFFFFFFFFFFFFFFFF == value._Hi64Bits) {
                            return -1;
                        }
                    }
                }
                throw ThrowArithmeticException();
            }

            public static Quadruple Abs(Quadruple value) {
                return new Quadruple(value._Lo64Bits, 0x7FFFFFFFFFFFFFFFu & value._Hi64Bits);
            }

            [DoesNotReturnAttribute()]
            private static Exception ThrowMinMaxException(Quadruple min, Quadruple max) {
                throw new ArgumentException();
            }

            [DoesNotReturnAttribute()]
            private static ArithmeticException ThrowArithmeticException() {
                _ = System.Math.Sign(Double.NaN);
                throw null!;
            }

            public static Quadruple Sqrt(Quadruple value) {
                return new Quadruple(Binary128Arithmetic.Sqrt(value._Lo64Bits, value._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var hi), hi);
            }

            public static Quadruple Sqrt(Quadruple value, [ConstantExpected] FloatingPointRounding rounding) {
                return new Quadruple(Binary128Arithmetic.Sqrt(value._Lo64Bits, value._Hi64Bits, rounding, out var hi), hi);
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            public static Quadruple IEEERemainder(Quadruple first, Quadruple second) {
                return new Quadruple(Binary128Arithmetic.IEEERemainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            public static Quadruple IEEERemainder(Quadruple first, Quadruple second, [ConstantExpected] FloatingPointRounding rounding) {
                return new Quadruple(Binary128Arithmetic.IEEERemainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
            }
        }

        public static partial class Math {

        }

        public static class Converter {

        }

        public static class BitConverter {

            public static Int128 QuadrupleToInt128Bits(Quadruple value) {
                return new Int128(value._Lo64Bits, unchecked((Int64)value._Hi64Bits));
            }

            public static Quadruple Int128BitsToQuadruple(Int128 bits) {
                return new Quadruple(unchecked((UInt64)bits.LoInt64Bits), unchecked((UInt64)bits.HiInt64Bits));
            }

            public static UInt128 QuadrupleToUInt128Bits(Quadruple value) {
                return new UInt128(value._Lo64Bits, unchecked((UInt64)value._Hi64Bits));
            }

            public static Quadruple UInt128BitsToQuadruple(UInt128 bits) {
                return new Quadruple(unchecked((UInt64)bits.LoInt64Bits), unchecked((UInt64)bits.HiInt64Bits));
            }

#if NET7_0_OR_GREATER
            public static System.Int128 QuadrupleToStandardInt128Bits(Quadruple value) {
                return new System.Int128(lower: value._Lo64Bits, upper: unchecked((UInt64)value._Hi64Bits));
            }

            public static Quadruple Int128BitsToQuadruple(System.Int128 bits) {
                return new Quadruple(unchecked((UInt64)bits.LoInt64Bits), unchecked((UInt64)bits.HiInt64Bits));
            }

            public static System.UInt128 QuadrupleToStandardUInt128Bits(Quadruple value) {
                return new System.UInt128(lower: value._Lo64Bits, upper: unchecked((UInt64)value._Hi64Bits));
            }

            public static Quadruple UInt128BitsToQuadruple(System.UInt128 bits) {
                return new Quadruple(unchecked((UInt64)bits.LoInt64Bits), unchecked((UInt64)bits.HiInt64Bits));
            }
#endif
        }
    }
    partial struct Quadruple {
        // IEEE 754 binary128:
        // sign     : bit 127
        // exponent : bits 126..112 (15 bits, bias 16383)
        // fraction : bits 111..0 (112 bits)

        public static Quadruple NegativeSignalingNaN =>
            new(lo64Bits: 0x0000000000000001UL,
                hi64Bits: 0xFFFF000000000000UL);

        public static Quadruple PositiveSignalingNaN =>
            new(lo64Bits: 0x0000000000000001UL,
                hi64Bits: 0x7FFF000000000000UL);

        public static Quadruple NegativeQuietNaN =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0xFFFF800000000000UL);

        public static Quadruple PositiveQuietNaN =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0x7FFF800000000000UL);

        public static Quadruple NegativeNaN => NegativeQuietNaN;

        public static Quadruple PositiveNaN => PositiveQuietNaN;

        public static Quadruple NegativeOne =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0xBFFF000000000000UL);

        // Correctly rounded IEEE 754 binary128 constants

        public static Quadruple E =>
            new(lo64Bits: 0x95355FB8AC404E7AUL,
                hi64Bits: 0x40005BF0A8B14576UL);

        public static Quadruple Pi =>
            new(lo64Bits: 0x8469898CC51701B8UL,
                hi64Bits: 0x4000921FB54442D1UL);

        /// <inheritdoc cref="IFloatingPointConstants{Quadruple}.Tau"/>
        public static Quadruple Tau =>
            new(lo64Bits: 0x8469898CC51701B8UL,
                hi64Bits: 0x4001921FB54442D1UL);

        public static Quadruple One =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0x3FFF000000000000UL);

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public static Quadruple Two =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0x4000000000000000UL);

        [DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
        public static Quadruple OneHalf =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0x3FFE000000000000UL);

        public static int Radix => 2;

        public static Quadruple Zero => default;

        // -0
        public static Quadruple AdditiveIdentity =>
            new(lo64Bits: 0x0000000000000000UL,
                hi64Bits: 0x8000000000000000UL);

        public static Quadruple MultiplicativeIdentity => One;

        public static Quadruple Abs(Quadruple value) {
            return new Quadruple(
                lo64Bits: value._Lo64Bits,
                hi64Bits: value._Hi64Bits & 0x7FFFFFFFFFFFFFFFUL);
        }

        public static Quadruple CopySign(Quadruple value, Quadruple sign) {
            return new Quadruple(
                lo64Bits: value._Lo64Bits,
                hi64Bits: (value._Hi64Bits & 0x7FFFFFFFFFFFFFFFUL) | (sign._Hi64Bits & 0x8000000000000000UL));
        }

        public static Quadruple Acos(Quadruple x) {
            var lo = Binary128Arithmetic.Acos(x._Lo64Bits, x._Hi64Bits, MidpointRounding.ToEven, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Acos(Quadruple x, MidpointRounding mode) {
            var lo = Binary128Arithmetic.Acos(x._Lo64Bits, x._Hi64Bits, mode, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Acosh(Quadruple x) {
            // TODO: Provide a correct impl
            // acosh(x) = ln(x + sqrt(x-1)*sqrt(x+1))
            var t = Sqrt(x - One) * Sqrt(x + One);
            return Log(x + t);
        }

        public static Quadruple AcosPi(Quadruple x) {
            // TODO: Provide a correct impl
            // acos_pi(x) = acos(x) / π
            var y = Sqrt(One - x * x);
            return Atan2(y, x) / Pi;
        }

        public static Quadruple Asin(Quadruple x) {
            // TODO: Provide a correct impl
            // asin(x) = atan(x / sqrt(1 - x^2))
            var t = x / Sqrt(One - x * x);
            return Atan(t);
        }

        public static Quadruple Asinh(Quadruple x) {
            // TODO: Provide a correct impl
            // asinh(x) = ln(x + sqrt(x^2 + 1))
            return Log(x + Sqrt(x * x + One));
        }

        public static Quadruple AsinPi(Quadruple x) {
            // TODO: Provide a correct impl
            // asin_pi(x) = asin(x) / π
            return Asin(x) / Pi;
        }

        public static Quadruple Atan(Quadruple x) {
            // TODO: Provide a correct impl
            // atan(x) = atan2(x, 1)
            return Atan2(x, One);
        }

        public static Quadruple Atan2(Quadruple y, Quadruple x) {
            // TODO: Provide a correct impl
            // Quadrant-correct atan2
            if (x > Zero)
                return Atan(y / x);

            if (x < Zero)
                return (y >= Zero ? Pi : -Pi) + Atan(y / x);

            // x == 0
            if (y > Zero) return PiOverTwo;
            if (y < Zero) return -PiOverTwo;

            return NaN;
        }

        public static Quadruple Atan2Pi(Quadruple y, Quadruple x) {
            // TODO: Provide a correct impl
            // atan2_pi = atan2 / π
            return Atan2(y, x) / Pi;
        }

        public static Quadruple Atanh(Quadruple x) {
            // TODO: Provide a correct impl
            // atanh(x) = 1/2 * ln((1+x)/(1-x))
            return OneHalf * Log((One + x) / (One - x));
        }

        public static Quadruple AtanPi(Quadruple x) {
            // TODO: Provide a correct impl
            // atan_pi(x) = atan(x) / π
            return Atan(x) / Pi;
        }

        public static Quadruple BitDecrement(Quadruple x) {
            UInt128 bits = BitConverter.QuadrupleToUInt128Bits(x);

            if (!Quadruple.IsFinite(x)) {
                // NaN returns NaN
                // -Infinity returns -Infinity
                // +Infinity returns MaxValue
                return (bits == Quadruple.PositiveInfinityBits) ? Quadruple.MaxValue : x;
            }

            if (bits == Quadruple.PositiveZeroBits) {
                // +0.0 returns -Quadruple.Epsilon
                return -Quadruple.Epsilon;
            }

            // Negative values need to be incremented
            // Positive values need to be decremented

            if (Quadruple.IsNegative(x)) {
                bits += 1;
            } else {
                bits -= 1;
            }
            return BitConverter.UInt128BitsToQuadruple(bits);
        }

        public static Quadruple BitIncrement(Quadruple x) {
            UInt128 bits = BitConverter.QuadrupleToUInt128Bits(x);

            if (!Quadruple.IsFinite(x)) {
                // NaN returns NaN
                // -Infinity returns MinValue
                // +Infinity returns +Infinity
                return (bits == Quadruple.NegativeInfinityBits) ? double.MinValue : x;
            }

            if (bits == Quadruple.NegativeZeroBits) {
                // -0.0 returns Epsilon
                return Quadruple.Epsilon;
            }

            // Negative values need to be decremented
            // Positive values need to be incremented

            if (Quadruple.IsNegative(x)) {
                bits -= 1;
            } else {
                bits += 1;
            }
            return BitConverter.UInt128BitsToQuadruple(bits);
        }

        public static Quadruple Cbrt(Quadruple x) {
            var lo = Binary128Arithmetic.Cbrt(x._Lo64Bits, x._Hi64Bits, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Cbrt(Quadruple x, MidpointRounding mode) {
            var lo = Binary128Arithmetic.Cbrt(x._Lo64Bits, x._Hi64Bits, mode, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Cos(Quadruple x) => System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Cos(System.BitConverter.QuadrupleToUInt128Bits(x)));

        public static Quadruple Cos(Quadruple x, MidpointRounding mode) => System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Cos(System.BitConverter.QuadrupleToUInt128Bits(x), mode));


        public static Quadruple Cosh(Quadruple x) {
            // TODO: Provide a correct impl.
            return Hypot(One, Sinh(x));
        }

        public static Quadruple CosPi(Quadruple x) {
            // TODO: Provide a correct impl.
            return SinCosPi(x).CosPi;
        }

        public static Quadruple FusedMultiplyAdd(Quadruple left, Quadruple right, Quadruple addend) {
            // TODO: Provide a correct impl.
            return left * right + addend;
        }

        public static Quadruple Hypot(Quadruple x, Quadruple y) =>
            System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Hypot(
                System.BitConverter.QuadrupleToUInt128Bits(x),
                System.BitConverter.QuadrupleToUInt128Bits(y)));

        public static Quadruple Hypot(Quadruple x, Quadruple y, MidpointRounding mode) =>
            System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Hypot(
                System.BitConverter.QuadrupleToUInt128Bits(x),
                System.BitConverter.QuadrupleToUInt128Bits(y), mode));

        public static Quadruple Ieee754Remainder(Quadruple left, Quadruple right) {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IFloatingPointIeee754{Quadruple}.ILogB(Quadruple)"/>
        public static int ILogB(Quadruple value) {
            return Binary128Arithmetic.ILogB(value._Lo64Bits, value._Hi64Bits);
        }

        public static bool IsComplexNumber(Quadruple value) {
            return false;
        }

        private const int EXP_BIAS = 16383;

        public static bool IsInteger(Quadruple value)
            => BinaryFloatingPointIeee754Arithmetic.IsInteger<Quadruple>(value);

        public static bool IsEvenInteger(Quadruple value)
            => BinaryFloatingPointIeee754Arithmetic.IsEvenInteger<Quadruple, System.UInt128>(value);

        public static bool IsOddInteger(Quadruple value)
            => BinaryFloatingPointIeee754Arithmetic.IsOddInteger<Quadruple, System.UInt128>(value);

        public static bool IsImaginaryNumber(Quadruple value) {
            return false;
        }

        public static bool IsQuietNaN(Quadruple value) {
            UInt64 hi = value._Hi64Bits;
            UInt64 lo = value._Lo64Bits;

            return (hi & 0x7FFF800000000000UL) == 0x7FFF800000000000UL
                && ((hi & 0x00007FFFFFFFFFFFUL) != 0 || lo != 0);
        }

        public static bool IsPositive(Quadruple value) {
            return 0 <= value._Hi64Bits.ToSignedUnchecked();
        }

        public static bool IsRealNumber(Quadruple value) {
            return !IsNaN(value);
        }

        public static Quadruple Log(Quadruple x) {
            //return System.Math.Log((double)x);
            var lo = Binary128Arithmetic.Log(x._Lo64Bits, x._Hi64Bits, MidpointRounding.ToEven, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Log(Quadruple x, MidpointRounding mode) {
            var lo = Binary128Arithmetic.Log(x._Lo64Bits, x._Hi64Bits, mode, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Log(Quadruple x, Quadruple newBase) {
            // TODO: Provide a correct impl.
            return Log(x) / Log(newBase);
        }

        public static Quadruple Log10(Quadruple x) {
            // TODO: Provide a correct impl.
            return Log(x) * Log10OfE;
        }

        public static Quadruple Log2(Quadruple x) {
            // TODO: Provide a correct impl.
            return Log(x) * Log2OfE;
        }

        public static Quadruple LogP1(Quadruple x) {
            // TODO: Provide a correct impl.
            return Log(One + x);
        }

        public static Quadruple MaxMagnitude(Quadruple x, Quadruple y) {
            if (Quadruple.IsNaN(x)) {
                return x;
            }
            if (Quadruple.IsNaN(y)) {
                return y;
            }

            Quadruple ax = Quadruple.Abs(x);
            Quadruple ay = Quadruple.Abs(y);

            int cmp = ax.CompareTo(ay);
            if (cmp > 0) return x;
            if (cmp < 0) return y;

            return x; // tie → left operand wins
        }

        public static Quadruple MaxMagnitudeNumber(Quadruple x, Quadruple y) {
            bool xn = Quadruple.IsNaN(x);
            bool yn = Quadruple.IsNaN(y);

            if (xn && yn) return x;
            if (xn) return y;
            if (yn) return x;

            Quadruple ax = Quadruple.Abs(x);
            Quadruple ay = Quadruple.Abs(y);

            int cmp = ax.CompareTo(ay);
            if (cmp > 0) return x;
            if (cmp < 0) return y;

            return x;
        }

        public static Quadruple MinMagnitude(Quadruple x, Quadruple y) {
            if (Quadruple.IsNaN(x)) {
                return x;
            }
            if (Quadruple.IsNaN(y)) {
                return y;
            }

            Quadruple ax = Quadruple.Abs(x);
            Quadruple ay = Quadruple.Abs(y);

            int cmp = ax.CompareTo(ay);
            if (cmp < 0) return x;
            if (cmp > 0) return y;

            return x; // tie → left operand wins
        }

        public static Quadruple MinMagnitudeNumber(Quadruple x, Quadruple y) {
            bool xn = Quadruple.IsNaN(x);
            bool yn = Quadruple.IsNaN(y);

            if (xn && yn) return x;
            if (xn) return y;
            if (yn) return x;

            Quadruple ax = Quadruple.Abs(x);
            Quadruple ay = Quadruple.Abs(y);

            int cmp = ax.CompareTo(ay);
            if (cmp < 0) return x;
            if (cmp > 0) return y;

            return x;
        }

        static readonly BigInteger MaxNaNPayloadAsBigInteger = (BigInteger.One << 110) - 1;

        public static Quadruple Parse(string s) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null);

        public static Quadruple Parse(string s, NumberStyles style) => Parse(s, style, provider: null);

        public static Quadruple Parse(string s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        public static Quadruple Parse(string s, NumberStyles style, IFormatProvider? provider) {
            ArgumentNullException.ThrowIfNull(s);
            return Parse(s.AsSpan(), style, provider);
        }

        public static Quadruple Parse(ReadOnlySpan<char> s, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.ParseFloat(s, style, NumberFormatInfo.GetInstance(provider));
        }

        /// <inheritdoc cref="ISpanParsable{TSelf}.Parse(ReadOnlySpan{char}, IFormatProvider?)" />
        public static Quadruple Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        /// <inheritdoc cref="INumberBase{TSelf}.Parse(ReadOnlySpan{byte}, NumberStyles, IFormatProvider?)" />
        public static Quadruple Parse(ReadOnlySpan<byte> utf8Text, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.ParseFloat(utf8Text, style, NumberFormatInfo.GetInstance(provider));
        }

        /// <inheritdoc cref="IUtf8SpanParsable{TSelf}.Parse(ReadOnlySpan{byte}, IFormatProvider?)" />
        public static Quadruple Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider) => Parse(utf8Text, NumberStyles.Float | NumberStyles.AllowThousands, provider);

        public static Quadruple Pow(Quadruple x, Quadruple y) {
            // TODO: Provide a correct impl
            return Exp(Log(x) * y);
        }

        static Quadruple CanonicalizeAndResetSignalingNaN(Quadruple nan) {
            Debug.Assert(IsNaN(nan));

            // Extract raw 128-bit encoding using your BitConverterExtensions
            UInt128 bits = BitConverterExtensions.QuadrupleToUInt128Bits(nan);

            // Quiet bit = fraction bit 111
            // That is bit position 111 in the 128-bit word.
            const int QuietBitIndex = 111;

            // Force quiet bit = 1 (turn sNaN → qNaN, preserve payload)
            bits |= ((UInt128)1 << QuietBitIndex);

            // Reconstruct Quadruple using your extension
            return BitConverterExtensions.UInt128BitsToQuadruple(bits);
        }

        public static Quadruple Compound(Quadruple x, int n) {
            // TODO: Provide a correct-rounding impl
            
            // (1 + x)^n with full IEEE‑754 semantics

            // 1. Classification
            if (IsNaN(x)) return CanonicalizeAndResetSignalingNaN(x);
            if (n < 0) return Quadruple.NaN;        // negative compounding not allowed
            if (n == 0) return Quadruple.One;
            if (n == 1) return Quadruple.One + x;

            // 2. Special cases
            if (IsPositiveInfinity(x)) return Quadruple.PositiveInfinity;
            if (IsNegativeInfinity(x)) {
                // (1 + -∞) = -∞ → (-∞)^n
                return (n % 2 == 0) ? Quadruple.PositiveInfinity : Quadruple.NegativeInfinity;
            }

            Quadruple baseValue = Quadruple.One + x;

            if (IsZero(baseValue)) return Quadruple.Zero;
            if (IsNegative(baseValue)) {
                // Negative base: only allowed for odd n
                if ((n & 1) == 0) return Quadruple.NaN;
            }
            
            // return Exp(LogP1(x) * n);

            // 3. Exponentiation by squaring (correct rounding after each op)
            Quadruple result = Quadruple.One;
            Quadruple y = baseValue;

            int k = n;
            while (k > 0) {
                if ((k & 1) != 0)
                    result *= y;   // fused multiply‑add if available

                y *= y;
                k >>= 1;
            }

            return result;
        }

        public static Quadruple RootN(Quadruple x, int n) {
            // TODO: Provide a correct-rounding impl
       
            // Classification
            if (IsNaN(x)) return CanonicalizeAndResetSignalingNaN(x);
            if (n <= 0) return Quadruple.NaN;
            if (n == 1) return x;

            // Special cases
            if (IsZero(x) || IsPositiveInfinity(x)) return x; // preserve sign for zero
            if (IsNegativeInfinity(x)) return (n % 2 == 1) ? Quadruple.NegativeInfinity : Quadruple.NaN;

            // Negative base with even root is NaN
            if (x < Quadruple.Zero && (n % 2 == 0))
                return Quadruple.NaN;

            // For negative base and odd n, compute root of absolute value and restore sign
            if (x < Quadruple.Zero) {
                Quadruple absRoot = Exp(Log(-x) / n);
                return -absRoot;
            }

            // General positive case
            return Exp(Log(x) / n);
        }


        public static Quadruple Round(Quadruple x, int digits, MidpointRounding mode = MidpointRounding.ToEven) {
            if (0 == digits) {
                var lo = Binary128Arithmetic.Round(x._Lo64Bits, x._Hi64Bits,
                    mode.ToFloatingPointRounding(), out var hi);
                return new Quadruple(lo, hi);
            }
            {
                // TODO: Provide a correct impl
                return Round(x * Exp10(digits), 0, mode);
            }
        }

        public static Quadruple Floor(Quadruple x) {
            var lo = Binary128Arithmetic.Round(x._Lo64Bits, x._Hi64Bits,
                FloatingPointRounding.Downward, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Ceiling(Quadruple x) {
            var lo = Binary128Arithmetic.Round(x._Lo64Bits, x._Hi64Bits,
                FloatingPointRounding.Upward, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Truncate(Quadruple value) {
            var lo = Binary128Arithmetic.Truncate(value._Lo64Bits, value._Hi64Bits, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple ScaleB(Quadruple x, int n) {
            var lo = Binary128Arithmetic.ScaleB(x._Lo64Bits, x._Hi64Bits, n,
                FloatingPointRounding.ToNearestWithMidpointToEven, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple ScaleB(Quadruple x, int n, MidpointRounding mode) {
            var lo = Binary128Arithmetic.ScaleB(x._Lo64Bits, x._Hi64Bits, n,
                mode.ToFloatingPointRounding(), out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Sin(Quadruple x) => System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Sin(System.BitConverter.QuadrupleToUInt128Bits(x)));

        public static Quadruple Sin(Quadruple x, MidpointRounding mode) => System.BitConverter.UInt128BitsToQuadruple(Binary128Arithmetic.Sin(System.BitConverter.QuadrupleToUInt128Bits(x), mode));

        public static (Quadruple Sin, Quadruple Cos) SinCos(Quadruple x) {
            var (s, c) = Binary128Arithmetic.SinCos(System.BitConverter.QuadrupleToUInt128Bits(x));
            return (System.BitConverter.UInt128BitsToQuadruple(s), System.BitConverter.UInt128BitsToQuadruple(c));
        }

        public static (Quadruple Sin, Quadruple Cos) SinCos(Quadruple x, MidpointRounding mode) {
            var (s, c) = Binary128Arithmetic.SinCos(System.BitConverter.QuadrupleToUInt128Bits(x), mode);
            return (System.BitConverter.UInt128BitsToQuadruple(s), System.BitConverter.UInt128BitsToQuadruple(c));
        }

        public static (Quadruple SinPi, Quadruple CosPi) SinCosPi(Quadruple x) {
            // TODO: Provide a correct impl.

            if (Quadruple.IsNaN(x))
                return (Quadruple.NaN, Quadruple.NaN);

            if (Quadruple.IsInfinity(x))
                return (Quadruple.NaN, Quadruple.NaN);

            // Reduce x = k + r, where r ∈ [-0.5, 0.5]
            Quadruple k = Quadruple.Floor(x);
            Quadruple r = x - k;

            // Map r into [-0.5, 0.5]
            if (r > Quadruple.OneHalf) { r -= Quadruple.One; k += Quadruple.One; }
            if (r < -Quadruple.OneHalf) { r += Quadruple.One; k -= Quadruple.One; }

            // Special exact points
            if (r == Quadruple.Zero) {
                Quadruple cos = ((Quadruple.IsOddInteger(k)) ? -Quadruple.One : Quadruple.One);
                return (Quadruple.Zero, cos);
            }

            if (r == Quadruple.OneHalf) {
                Quadruple sin = ((Quadruple.IsOddInteger(k)) ? -Quadruple.One : Quadruple.One);
                return (sin, Quadruple.Zero);
            }

            if (r == -Quadruple.OneHalf) {
                Quadruple sin = ((Quadruple.IsOddInteger(k)) ? Quadruple.One : -Quadruple.One);
                return (sin, Quadruple.Zero);
            }

            // Compute sin(pi*r), cos(pi*r) using your existing SinCos
            Quadruple angle = Quadruple.Pi * r;
            var (s, c) = Quadruple.SinCos(angle);

            // Apply (-1)^k
            if (Quadruple.IsOddInteger(k)) {
                s = -s;
                c = -c;
            }

            return (s, c);
        }


        public static Quadruple Sinh(Quadruple x) {

            // TODO: Provide a correct impl.

            if (Quadruple.IsNaN(x)) return x;
            if (Quadruple.IsInfinity(x)) return x;

            Quadruple ex = Quadruple.Exp(x);

            // For large |x|, exp(-x) underflows → sinh ≈ ±exp(x)/2
            if (Quadruple.Abs(x) > (Quadruple)40)
                return Quadruple.CopySign(ex / Quadruple.Two, x);

            Quadruple emx = Quadruple.Exp(-x);
            return (ex - emx) / Quadruple.Two;
        }


        public static Quadruple SinPi(Quadruple x) {

            // TODO: Provide a correct impl.
            return SinCosPi(x).SinPi;
        }

        public static Quadruple Sqrt(Quadruple x) {
            var lo = Binary128Arithmetic.Sqrt(x._Lo64Bits, x._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Sqrt(Quadruple x, MidpointRounding mode) {
            var lo = Binary128Arithmetic.Sqrt(x._Lo64Bits, x._Hi64Bits, mode.ToFloatingPointRounding(), out var hi);
            return new Quadruple(lo, hi);
        }

        public static Quadruple Tan(Quadruple x) {
            // TODO: Provide a correct impl.
            if (Quadruple.IsNaN(x)) return x;
            var (s, c) = Quadruple.SinCos(x);
            return s / c;
        }


        public static Quadruple Tanh(Quadruple x) {

            // TODO: Provide a correct impl.
            if (Quadruple.IsNaN(x)) return x;
            if (Quadruple.IsInfinity(x)) return Quadruple.CopySign(Quadruple.One, x);

            Quadruple ax = Quadruple.Abs(x);

            // Large x → tanh(x) ≈ ±1
            if (ax > (Quadruple)20)
                return Quadruple.CopySign(Quadruple.One, x);

            Quadruple e2x = Quadruple.Exp(x + x);
            return (e2x - Quadruple.One) / (e2x + Quadruple.One);
        }


        public static Quadruple TanPi(Quadruple x) {

            // TODO: Provide a correct impl.
            var (s, c) = SinCosPi(x);

            if (Quadruple.IsNaN(s) || Quadruple.IsNaN(c))
                return Quadruple.NaN;

            if (c == Quadruple.Zero)
                return Quadruple.NaN; // or ±∞ depending on your policy

            return s / c;
        }


        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out Quadruple result) where TOther : INumberBase<TOther> {
            return INumberBaseFriendInternal<Quadruple>.TryConvertFromTruncating(value, out result);
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out Quadruple result) where TOther : INumberBase<TOther> {
            return INumberBaseFriendInternal<Quadruple>.TryConvertFromTruncating(value, out result);
        }

        const UInt64 SignBitUInt64 = unchecked((UInt64)UInt64.MinValue);

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out Quadruple result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(byte)) {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualValue = (decimal)(object)value;
                result = (Quadruple)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = FromIeee754InterchangeBinary<double, UInt64>(actualValue);
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = FromIeee754InterchangeBinary<Half, UInt16>(actualValue);
                return true;
            }
#if NET11_0_OR_GREATER
            else if (typeof(TOther) == typeof(BFloat16)) {
                BFloat16 actualValue = (BFloat16)(object)value;
                result = FromIeee754InterchangeBinary<BFloat16, UInt16>(actualValue);
                return true;
            }
#endif
            else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualValue = (int)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                UltimateOrb.Int128 actualValue = (UltimateOrb.Int128)(object)value;
                result = (Quadruple)actualValue;
                return true;
#if NET7_0_OR_GREATER
            } else if (typeof(TOther) == typeof(System.Int128)) {
                System.Int128 actualValue = (System.Int128)(object)value;
                result = (Quadruple)actualValue;
                return true;
#endif
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = FromIeee754InterchangeBinary<float, UInt32>(actualValue);
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                UltimateOrb.UInt128 actualValue = (UltimateOrb.UInt128)(object)value;
                result = (Quadruple)actualValue;
                return true;
#if NET7_0_OR_GREATER
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                System.UInt128 actualValue = (System.UInt128)(object)value;
                result = (Quadruple)actualValue;
                return true;
#endif
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(BigInteger)) {
                BigInteger actualValue = (BigInteger)(object)value;
                result = (Quadruple)actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        public static bool TryConvertToChecked<TOther>(Quadruple value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(Quadruple value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(Quadruple value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }


        public static bool TryParse([NotNullWhen(true)] string? s, out Quadruple result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);

        public static bool TryParse(ReadOnlySpan<char> s, out Quadruple result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);

        /// <summary>Tries to convert a UTF-8 character span containing the string representation of a number to its double-precision floating-point number equivalent.</summary>
        /// <param name="utf8Text">A read-only UTF-8 character span that contains the number to convert.</param>
        /// <param name="result">When this method returns, contains a double-precision floating-point number equivalent of the numeric value or symbol contained in <paramref name="utf8Text" /> if the conversion succeeded or zero if the conversion failed. The conversion fails if the <paramref name="utf8Text" /> is <see cref="ReadOnlySpan{T}.Empty" /> or is not in a valid format. This parameter is passed uninitialized; any value originally supplied in result will be overwritten.</param>
        /// <returns><c>true</c> if <paramref name="utf8Text" /> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(ReadOnlySpan<byte> utf8Text, out Quadruple result) => TryParse(utf8Text, NumberStyles.Float | NumberStyles.AllowThousands, provider: null, out result);

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Quadruple result) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(s.AsSpan(), style, NumberFormatInfo.GetInstance(provider), out result, out _);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Quadruple result) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(s, style, NumberFormatInfo.GetInstance(provider), out result, out _);
        }


        /// <inheritdoc cref="INumberBase{TSelf}.TryParsePartial(string, NumberStyles, IFormatProvider?, out TSelf, out int)" />
        public static bool TryParsePartial([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Quadruple result, out int charsConsumed) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(s.AsSpan(), style | Number.AllowTrailingInvalidCharacters, NumberFormatInfo.GetInstance(provider), out result, out charsConsumed);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryParsePartial(ReadOnlySpan{char}, NumberStyles, IFormatProvider?, out TSelf, out int)" />
        public static bool TryParsePartial(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Quadruple result, out int charsConsumed) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(s, style | Number.AllowTrailingInvalidCharacters, NumberFormatInfo.GetInstance(provider), out result, out charsConsumed);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryParsePartial(ReadOnlySpan{byte}, NumberStyles, IFormatProvider?, out TSelf, out int)" />
        public static bool TryParsePartial(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out Quadruple result, out int bytesConsumed) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(utf8Text, style | Number.AllowTrailingInvalidCharacters, NumberFormatInfo.GetInstance(provider), out result, out bytesConsumed);
        }

        /// <inheritdoc cref="IParsable{TSelf}.TryParse(string?, IFormatProvider?, out TSelf)" />
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Quadruple result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

        /// <inheritdoc cref="ISpanParsable{TSelf}.TryParse(ReadOnlySpan{char}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Quadruple result) => TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);

        /// <inheritdoc cref="INumberBase{TSelf}.TryParse(ReadOnlySpan{byte}, NumberStyles, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, out Quadruple result) {
            NumberFormatInfo.ValidateParseStyleFloatingPoint(style);
            return QuadrupleNumber.TryParseFloat(utf8Text, style, NumberFormatInfo.GetInstance(provider), out result, out _);
        }

        /// <inheritdoc cref="IUtf8SpanParsable{TSelf}.TryParse(ReadOnlySpan{byte}, IFormatProvider?, out TSelf)" />
        public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, out Quadruple result) => TryParse(utf8Text, NumberStyles.Float | NumberStyles.AllowThousands, provider, out result);




        /// <summary>
        /// Extracts the raw biased exponent and trailing significand field of a
        /// binary128 value supplied as two 64-bit halves.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Binary128 (<see cref="Quadruple"/>) bit layout:
        /// <code>
        ///   bit  127       : sign
        ///   bits 126 .. 112: biased exponent           (15 bits)
        ///   bits 111 ..   0: trailing significand field (112 bits)
        /// </code>
        /// </para>
        /// <para>
        /// <paramref name="significandHi"/> receives bits 111..64 (48 bits, already
        /// masked); <paramref name="significandLo"/> receives bits 63..0 (64 bits,
        /// full). Together they are the <i>trailing significand field</i>, not the
        /// significand — the implicit integer bit is not materialized here and must be
        /// added by the caller for normal values.
        /// </para>
        /// <para>
        /// The sign bit is <b>not</b> returned; it is bit 63 of <paramref name="hi"/>.
        /// This is deliberate: callers that must preserve <c>−0</c>, <c>−subnormal</c>,
        /// <c>−∞</c> or a signed NaN need to read it from the raw <c>hi</c> word
        /// themselves, since an intermediary that "helpfully" discards it will
        /// silently lose those cases.
        /// </para>
        /// <para>
        /// This routine is <b>binary128</b>-only. It has nothing to do with
        /// decimal128, whose 128-bit encoding (sign : 1 / combination : 5 /
        /// exponent continuation : 12 / coefficient continuation : 110) does not share
        /// this layout.
        /// </para>
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificand(
            UInt64 lo, UInt64 hi,
            out UInt64 significandLo,
            out UInt64 significandHi) {
            uint exponent = (uint)((hi >> 48) & 0x7FFFu);

            significandHi = hi & 0x0000_FFFF_FFFF_FFFFUL;   // bits 111..64  (48 bits)
            significandLo = lo;                             // bits  63.. 0  (64 bits)

            return exponent;
        }

        /// <summary>
        /// <see cref="UInt128"/>-packed overload. The trailing significand is returned
        /// zero-extended in the low 112 bits of <paramref name="significand"/>; the
        /// top 16 bits are guaranteed zero.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificand(
            UInt128 value,
            out UInt128 significand) {
            unchecked {
                UInt64 lo = (UInt64)value;
                UInt64 hi = (UInt64)(value >> 64);

                uint rawExp = ExtractRawBiasedExponentAndRawSignificand(
                    lo, hi, out UInt64 sigLo, out UInt64 sigHi);

                // sigHi is 48 bits; shift into position 64..111 and OR in the low half.
                // The top 16 bits (112..127) of the result are zero by construction.
                significand = ((UInt128)sigHi << 64) | sigLo;
                return rawExp;
            }
        }

        /// <summary>
        /// Convenience overload operating on <c>this</c>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint ExtractRawBiasedExponentAndRawSignificand(out UInt128 significand) {
            return ExtractRawBiasedExponentAndRawSignificand(
                BitConverter.QuadrupleToUInt128Bits(this), out significand);
        }

        public int CompareTo(object? obj) {
            if (obj is null)
                return 1; // this > null

            if (obj is Quadruple q)
                return CompareTo(q);

            throw new ArgumentException("Object must be of type Quadruple.", nameof(obj));
        }

        public int CompareTo(Quadruple other) {
            // NaN < -inf in .NET total order
            return ToIntegerTotalOrderDotNet(this).CompareTo(ToIntegerTotalOrderDotNet(other));
        }

        private static System.Int128 ToIntegerTotalOrderDotNet(Quadruple q) {
            if (Quadruple.IsNaN(q)) {
                return unchecked(System.Int128.MinValue - 1 - (System.Int128)NegativeInfinityBits);
            }
            System.Int128 bits = BitConverter.QuadrupleToInt128Bits(q);
            return Quadruple.IsNegative(q)
                ? unchecked(System.Int128.MinValue - bits)
                : bits;
        }

        public int GetExponentByteCount() {
            return sizeof(Int16);
        }

        public int GetExponentShortestBitLength() {
            unchecked {
                var exponent = unchecked((short)(RawBiasedExponent - EXP_BIAS));
                if (exponent >= 0) {
                    return (sizeof(short) * 8) - short.LeadingZeroCount(exponent);
                } else {
                    return (sizeof(short) * 8) + 1 - short.LeadingZeroCount((short)(~exponent));
                }
            }
        }

        public int GetSignificandBitLength() {
            return 112 + 1;
        }

        public int GetSignificandByteCount() {
            return Unsafe.SizeOf<UInt128>();
        }

        TypeCode IConvertible.GetTypeCode() {
            return TypeCode.Object;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, nameof(Quadruple), nameof(DateTime)));
        }

        // value = (-1)^negative * M * 2^e,  M ∈ [2^112, 2^113) for normals.
        // For ±0 and subnormals: M = 0, e = 0, negative = false (they round to +0).
        private static bool TryExtractComponents(
            Quadruple value, out bool negative, out UInt128 M, out int e) {
            negative = false;
            M = UInt128.Zero;
            e = 0;

            ulong hi = value._Hi64Bits;
            ulong lo = value._Lo64Bits;

            int sign = (int)(hi >> 63);
            int exponent = (int)((hi >> 48) & 0x7FFF);
            ulong mantHi = hi & 0x0000_FFFF_FFFF_FFFFUL;
            ulong mantLo = lo;

            if (exponent == 0x7FFF) {
                return false;   // NaN or ±Infinity
            }

            if (exponent == 0) {
                return true;    // ±0 / subnormal: M = 0, negative = false
            }

            int unbiased = exponent - 16383;
            M = ((UInt128)1 << 112) | ((UInt128)mantHi << 64) | (UInt128)mantLo;
            e = unbiased - 112;    // finite range: [-16382 - 112, 16383 - 112]
            negative = sign != 0;
            return true;
        }
        internal static bool TryConvertToPlainIntegerSigned<TInt>(Quadruple value, out TInt result)
            where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            result = default;

            if (!TryExtractComponents(value, out bool negative, out UInt128 M, out int e)) {
                return false;
            }

            if (M == UInt128.Zero) { result = TInt.Zero; return true; }

            int bitWidth = 8 * Unsafe.SizeOf<TInt>();

            // Signed-range bounds for the magnitude.
            // bitWidth >= 129 → everything the UInt128 path can produce fits.
            UInt128 maxPos, maxNeg;
            if (bitWidth >= 129) { maxPos = UInt128.MaxValue; maxNeg = UInt128.MaxValue; } else { int sh = bitWidth - 1; maxPos = ((UInt128)1 << sh) - 1; maxNeg = (UInt128)1 << sh; }

            if (e < 0) {
                int s = -e;
                if (s >= 113) { result = TInt.Zero; return true; }
                UInt128 m = M >> s;

                UInt128 bound = negative ? maxNeg : maxPos;
                if (m > bound) {
                    return false;
                }

                if (!negative) { result = TInt.CreateTruncating(m); return true; }
                TInt mT = TInt.CreateTruncating(m);
                result = unchecked(TInt.Zero - mT);
                return true;
            }

            // e >= 0.
            if (113 + e > bitWidth) {
                return false;
            }

            if (113 + e <= 128) {
                UInt128 m = M << e;
                UInt128 bound = negative ? maxNeg : maxPos;
                if (m > bound) {
                    return false;
                }

                if (!negative) { result = TInt.CreateTruncating(m); return true; }
                TInt mT = TInt.CreateTruncating(m);
                result = unchecked(TInt.Zero - mT);
                return true;
            }

            // Wide path: 113 + e > 128, so bitWidth >= 129.
            // bitlen(value) = 113 + e.
            //   positive signed: need 113 + e <= bitWidth - 1.
            //   negative signed: need 113 + e <= bitWidth; equality only allowed when
            //                    the mantissa is exactly 2^112 (value = 2^(bitWidth-1)).
            if (!negative) {
                if (113 + e > bitWidth - 1) {
                    return false;
                }
            } else {
                if (113 + e > bitWidth) {
                    return false;
                }

                if (113 + e == bitWidth && M != ((UInt128)1 << 112)) {
                    return false;
                }
            }

            TInt tM = TInt.CreateTruncating(M);
            TInt magT = tM << e;
            if (!negative) { result = magT; return true; }
            result = unchecked(TInt.Zero - magT);
            return true;
        }
        internal static bool TryConvertToPlainIntegerUnsigned<TInt>(Quadruple value, out TInt result)
    where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            result = default;

            if (!TryExtractComponents(value, out bool negative, out UInt128 M, out int e)) {
                return false;
            }

            if (M == UInt128.Zero) { result = TInt.Zero; return true; }
            if (negative) {
                return false;
            }

            int bitWidth = 8 * Unsafe.SizeOf<TInt>();

            if (e < 0) {
                // Truncate: |v| = M >> (-e).  Result < 2^113, always fits in UInt128.
                int s = -e;
                if (s >= 113) { result = TInt.Zero; return true; }
                UInt128 m = M >> s;
                UInt128 maxVal = bitWidth >= 128 ? UInt128.MaxValue : (((UInt128)1 << bitWidth) - 1);
                if (m > maxVal) {
                    return false;
                }

                result = TInt.CreateTruncating(m);
                return true;
            }

            // e >= 0: |v| = M << e, bitlen = 113 + e.
            if (113 + e > bitWidth) {
                return false;
            }

            if (113 + e <= 128) {
                // Fits in UInt128; m < 2^(113+e) <= 2^bitWidth, so CreateTruncating is exact.
                UInt128 m = M << e;
                result = TInt.CreateTruncating(m);
                return true;
            }

            // Wide path: 113 + e > 128, so bitWidth >= 129.
            // Do the shift directly in TInt.
            TInt tM = TInt.CreateTruncating(M);   // exact: M < 2^113 <= 2^bitWidth
            result = tM << e;                     // e < bitWidth (from 113+e <= bitWidth)
            return true;
        }
        internal static bool TryRoundToPlainIntegerSigned<TInt>(Quadruple value, out TInt result)
            where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            result = default;

            if (!TryExtractComponents(value, out bool negative, out UInt128 M, out int e)) {
                return false;
            }

            if (M == UInt128.Zero) { result = TInt.Zero; return true; }

            int bitWidth = 8 * Unsafe.SizeOf<TInt>();

            // |v| = M * 2^e,  M ∈ [2^112, 2^113).

            // ---- Sub-unity case: |v| < 1, rounds to 0 or ±1 (half-to-even). ----
            // |v| < 1  ⇔  M * 2^e < 1  ⇔  e <= -113   (since M >= 2^112).
            if (e <= -113) {
                // e == -113 : |v| ∈ [0.5, 1);  exactly 0.5 iff M == 2^112.
                // e <  -113 : |v| < 0.5.
                if (e < -113 || M == ((UInt128)1 << 112)) {
                    result = TInt.Zero;
                    return true;
                }
                // |v| ∈ (0.5, 1) → rounds to ±1.
                if (!negative) { result = TInt.One; return true; }
                result = unchecked(TInt.Zero - TInt.One);
                return true;
            }

            // ---- e ∈ [-112, ∞).  Compute the rounded integer part. ----
            // Two subpaths: UInt128 (when it fits) or wide (uses TInt's own width).

            UInt128 mag128 = UInt128.Zero;
            bool widePath;

            if (e < 0) {
                // Right shift with round-half-to-even; s ∈ [1, 112].
                int s = -e;
                UInt128 intPart = M >> s;
                UInt128 mask = ((UInt128)1 << s) - 1;
                UInt128 rem = M & mask;
                UInt128 half = (UInt128)1 << (s - 1);
                if (rem > half || (rem == half && (intPart & UInt128.One) != UInt128.Zero)) {
                    intPart++;                 // ≤ 2^112, no overflow
                }

                mag128 = intPart;
                widePath = false;
            } else if (113 + e <= 128) {
                // Left shift, exact; fits in UInt128.
                mag128 = M << e;
                widePath = false;
            } else {
                // 113 + e > 128, magnitude doesn't fit UInt128. Use wide path.
                widePath = true;
            }

            if (!widePath) {
                UInt128 maxPos, maxNeg;
                if (bitWidth >= 129) {
                    // Every UInt128 magnitude fits (bitWidth > 128).
                    maxPos = UInt128.MaxValue;
                    maxNeg = UInt128.MaxValue;
                } else {
                    int sh = bitWidth - 1;                  // 0 .. 127
                    maxPos = ((UInt128)1 << sh) - 1;        // 2^(bw-1) - 1
                    maxNeg = (UInt128)1 << sh;             // 2^(bw-1)
                }

                if (!negative) {
                    if (mag128 > maxPos) {
                        return false;
                    }

                    result = TInt.CreateTruncating(mag128);
                    return true;
                }

                if (mag128 > maxNeg) {
                    return false;
                }

                if (mag128 == UInt128.Zero) { result = TInt.Zero; return true; }

                TInt mT = TInt.CreateTruncating(mag128);
                result = unchecked(TInt.Zero - mT);
                return true;
            }

            // ---- Wide path: e ≥ 0, 113 + e > 128. Requires bitWidth ≥ 129. ----
            if (bitWidth < 129) {
                return false;   // bitlen = 113 + e > 128 > bitWidth
            }

            // bitlen(M << e) = 113 + e.
            if (!negative) {
                // Positive signed range: magnitude ≤ 2^(bw-1) - 1 → bitlen ≤ bw - 1.
                if (113 + e > bitWidth - 1) {
                    return false;
                }
            } else {
                // Negative signed range: magnitude ≤ 2^(bw-1), with equality only
                // when the magnitude is exactly 2^(bw-1) (M == 2^112).
                if (113 + e > bitWidth) {
                    return false;
                }

                if (113 + e == bitWidth && M != ((UInt128)1 << 112)) {
                    return false;
                }
            }

            TInt tM = TInt.CreateTruncating(M);   // exact: M < 2^113 ≤ 2^bitWidth
            TInt magT = tM << e;                    // e < bitWidth (since 113 + e ≤ bitWidth)

            if (!negative) { result = magT; return true; }
            result = unchecked(TInt.Zero - magT);
            return true;
        }
        internal static bool TryRoundToPlainIntegerUnsigned<TInt>(Quadruple value, out TInt result)
            where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            result = default;

            if (!TryExtractComponents(value, out bool negative, out UInt128 M, out int e)) {
                return false;
            }

            if (M == UInt128.Zero) { result = TInt.Zero; return true; }

            int bitWidth = 8 * Unsafe.SizeOf<TInt>();

            // ---- Sub-unity: |v| < 1, rounds to 0 or ±1. ----
            if (e <= -113) {
                if (e < -113 || M == ((UInt128)1 << 112)) {
                    result = TInt.Zero;
                    return true;
                }
                // Rounds to ±1; negative is out of range for unsigned.
                if (negative) {
                    return false;
                }

                result = TInt.One;
                return true;
            }

            // ---- |v| ≥ 1. ----
            UInt128 mag128 = UInt128.Zero;
            bool widePath;

            if (e < 0) {
                int s = -e;                     // 1 .. 112
                UInt128 intPart = M >> s;
                UInt128 mask = ((UInt128)1 << s) - 1;
                UInt128 rem = M & mask;
                UInt128 half = (UInt128)1 << (s - 1);
                if (rem > half || (rem == half && (intPart & UInt128.One) != UInt128.Zero)) {
                    intPart++;
                }

                mag128 = intPart;
                widePath = false;
            } else if (113 + e <= 128) {
                mag128 = M << e;
                widePath = false;
            } else {
                widePath = true;
            }

            if (!widePath) {
                if (mag128 == UInt128.Zero) { result = TInt.Zero; return true; }
                if (negative) {
                    return false;
                }

                UInt128 maxVal = bitWidth >= 128
                    ? UInt128.MaxValue
                    : ((UInt128)1 << bitWidth) - 1;

                if (mag128 > maxVal) {
                    return false;
                }

                result = TInt.CreateTruncating(mag128);
                return true;
            }

            // ---- Wide path. ----
            if (negative) {
                return false;
            }

            if (bitWidth < 129) {
                return false;
            }

            // Unsigned range: magnitude ≤ 2^bitWidth - 1 → bitlen ≤ bitWidth.
            if (113 + e > bitWidth) {
                return false;
            }

            TInt tM = TInt.CreateTruncating(M);
            result = tM << e;
            return true;
        }

        internal static TInt RoundToPlainIntegerSigned<TInt>(Quadruple value)
            where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            var s = TryRoundToPlainIntegerSigned<TInt>(value, out var result);
            UltimateOrb.Utilities.ThrowHelper.ThrowOnFalse(s);
            return result;
        }

        internal static TInt RoundToPlainIntegerUnsigned<TInt>(Quadruple value)
            where TInt : unmanaged, IBinaryInteger<TInt>, IMinMaxValue<TInt> {
            var s = TryRoundToPlainIntegerUnsigned<TInt>(value, out var result);
            UltimateOrb.Utilities.ThrowHelper.ThrowOnFalse(s);
            return result;
        }

        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return !IsZero(this);
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, "Quadruple", "Char"));
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerSigned<sbyte>(this);
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerUnsigned<byte>(this);
        }

        short IConvertible.ToInt16(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerSigned<short>(this);
        }

        ushort IConvertible.ToUInt16(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerUnsigned<ushort>(this);
        }

        int IConvertible.ToInt32(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerSigned<int>(this);
        }

        uint IConvertible.ToUInt32(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerUnsigned<uint>(this);
        }

        long IConvertible.ToInt64(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerSigned<long>(this);
        }

        ulong IConvertible.ToUInt64(IFormatProvider? provider) {
            return Quadruple.RoundToPlainIntegerUnsigned<ulong>(this);
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return (Single)this;
        }

        double IConvertible.ToDouble(IFormatProvider? provider) {
            return (double)this;
        }

        decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return (decimal)this;
        }

        public object ToType(Type conversionType, IFormatProvider? provider) {
            if (typeof(Quadruple) == conversionType) {
                return this;
            }
            return ConvertInternal.DefaultToType(in this, conversionType, provider);
        }

        public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(Int16)) {
                var exponent = unchecked((Int16)(this.ExtractRawBiasedExponentAndRawSignificand(out _) - EXP_BIAS));

                if (System.BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(Int16);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(Int16)) {
                var exponent = unchecked((Int16)(this.ExtractRawBiasedExponentAndRawSignificand(out _) - EXP_BIAS));

                if (!System.BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(Int16);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        internal UInt128 GetTrailingSignificandOrPayloadEx() {
            return ((ExtractRawBiasedExponentAndRawSignificand(out var trailingSignificand) != 0) ? (UInt128.One << FractionBitCount) : 0UL) | trailingSignificand;
        }

        internal UInt128 GetSignificandOrPayloadEx() {
            return GetTrailingSignificandOrPayloadEx() | ((ExtractRawBiasedExponentAndRawSignificand(out _) != 0) ? (UInt128.One << FractionBitCount) : 0UL);
        }

        public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= Unsafe.SizeOf<UInt128>()) {
                var significand = this.GetSignificandOrPayloadEx();

                if (System.BitConverter.IsLittleEndian) {
#if NET8_0_OR_GREATER
                    significand = BinaryPrimitives.ReverseEndianness(significand);
#else
                    significand = new UInt128(
                        hi: BinaryPrimitives.ReverseEndianness(significand.GetLowPart()),
                        lo: BinaryPrimitives.ReverseEndianness(significand.GetHighPart()));
#endif
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = Unsafe.SizeOf<UInt128>();
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= Unsafe.SizeOf<UInt128>()) {
                var significand = this.GetSignificandOrPayloadEx();

                if (!System.BitConverter.IsLittleEndian) {
#if NET8_0_OR_GREATER
                    significand = BinaryPrimitives.ReverseEndianness(significand);
#else
                    significand = new UInt128(
                        hi: BinaryPrimitives.ReverseEndianness(significand.GetLowPart()),
                        lo: BinaryPrimitives.ReverseEndianness(significand.GetHighPart()));
#endif
                }
                double a;
                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = Unsafe.SizeOf<UInt128>();
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        internal int RawBiasedExponent {
            get {
                return unchecked((int)ExtractRawBiasedExponentAndRawSignificand(out _));
            }
        }

        public static Quadruple operator ++(Quadruple value) {
            return One + value;
        }

        public static Quadruple operator --(Quadruple value) {
            return value - One;
        }

    }
}
namespace UltimateOrb {

    public static partial class QuadrupleExtensions {

    }

    partial class QuadrupleExtensions {


    }
}

namespace UltimateOrb {

    partial struct Quadruple {

        public static explicit operator Quadruple(UltimateOrb.Int128 value) {
            return (Quadruple)(System.Int128)value;
        }

        public static explicit operator Quadruple(UltimateOrb.UInt128 value) {
            return (Quadruple)(System.UInt128)value;
        }

        public static explicit operator Quadruple(System.UInt128 value) {
            if (value == System.UInt128.Zero) {
                // +0.0
                return new Quadruple(0UL, 0UL);
            }

            // Bit length, 1..128.
            int n = 128 - BinaryNumerals.CountLeadingZeros(value);

            UInt64 lo = unchecked((UInt64)value);
            UInt64 hi = unchecked((UInt64)(value >> 64));

            // Unbiased exponent = n - 1; biased = n - 1 + 16383 = n + 16382.
            int exp = n + 16382;

            // 113-bit significand with the implicit 1 parked at bit 112.
            // Layout: bits 0..63 in sigLo, bits 64..112 in the low 49 bits of sigHi.
            UInt64 sigLo, sigHi;

            if (n <= 113) {
                // Exact: shift the value left so its MSB lands at bit 112.
                int sh = 113 - n;                 // 0..112
                if (sh == 0) {
                    sigHi = hi;
                    sigLo = lo;
                } else if (sh < 64) {
                    sigHi = (hi << sh) | (lo >> (64 - sh));
                    sigLo = lo << sh;
                } else {
                    sigHi = lo << (sh - 64);
                    sigLo = 0UL;
                }
            } else {
                // n > 113: drop sh = n - 113 bits (1..15) with round-half-to-even.
                int sh = n - 113;

                UInt64 discarded = lo & ((1UL << sh) - 1);
                sigLo = (lo >> sh) | (hi << (64 - sh));
                sigHi = hi >> sh;

                UInt64 half = 1UL << (sh - 1);
                if (discarded > half || (discarded == half && (sigLo & 1UL) != 0)) {
                    sigLo++;
                    if (sigLo == 0) {
                        sigHi++;
                    }
                }

                // Rounding may have carried into bit 113. Renormalize if so.
                if ((sigHi >> 49) != 0) {
                    sigLo = (sigLo >> 1) | (sigHi << 63);
                    sigHi >>= 1;
                    exp++;
                }
            }

            // Strip the implicit leading 1 (bit 112 == bit 48 of sigHi).
            UInt64 mantLo = sigLo;
            UInt64 mantHi = sigHi & 0x0000_FFFF_FFFF_FFFFUL;

            // Pack: [sign:1 | exp:15 | mantissa hi:48] : [mantissa lo:64]
            UInt64 resultHi = ((UInt64)exp << 48) | mantHi;
            UInt64 resultLo = mantLo;

            return new Quadruple(resultLo, resultHi);
        }

        public static explicit operator Quadruple(System.Int128 value) {
            return 0 > value ? -unchecked((Quadruple)(System.UInt128)(-value)) : unchecked((Quadruple)(System.UInt128)value);
        }
    }
}

namespace UltimateOrb {

    partial struct Quadruple {

        public static implicit operator Quadruple(decimal value) {
            unchecked {
                // Layout of System.Decimal (Win32 DECIMAL):
                //   _flags  bits 16..23 : scale (0..28)
                //   _flags  bit  31     : sign
                //   _hi32               : high 32 bits of the 96-bit mantissa
                //   _lo64               : low  64 bits of the 96-bit mantissa
                var d = UltimateOrb.Runtime.CompilerServices.Unsafe
                    .BitCast<System.Decimal, Win32Decimal>(value);

                int flags = d._flags;
                int sign = (flags >> 31) & 1;
                int scale = (flags >> 16) & 0xFF;

                // 96-bit unsigned mantissa as BigInteger.
                BigInteger p = ((BigInteger)d._hi32 << 64) | d._lo64;

                if (p.IsZero) {
                    // ±0.0, preserving the sign of the decimal.
                    ulong zeroHi = (ulong)sign << 63;
                    return new Quadruple(0UL, zeroHi);
                }

                BigInteger q = BigInteger.Pow(10, scale);

                // E = floor(log2(p/q)).
                int n = (int)p.GetBitLength();
                int m = (int)q.GetBitLength();
                int E = n >= m
                    ? (p >= (q << (n - m)) ? n - m : n - m - 1)
                    : ((p << (m - n)) >= q ? n - m : n - m - 1);

                // M = p * 2^(112 - E) / q,  M ∈ [2^112, 2^113).
                // For decimal inputs, E ∈ [-95, 95], so k = 112 - E ∈ [17, 207] > 0.
                int k = 112 - E;
                BigInteger N = p << k;
                BigInteger D = q;

                BigInteger Q = BigInteger.DivRem(N, D, out BigInteger R);

                // Round to nearest, ties to even.
                if (!R.IsZero) {
                    int cmp = (R << 1).CompareTo(D);
                    if (cmp > 0 || (cmp == 0 && !Q.IsEven)) {
                        Q += 1;
                    }
                }

                // Rounding may carry into bit 113: renormalize.
                if (Q >= (BigInteger.One << 113)) {
                    Q >>= 1;
                    E += 1;
                }

                // Strip the implicit leading 1 (Q ∈ [2^112, 2^113)).
                BigInteger sig = Q - (BigInteger.One << 112);   // 112 bits

                int biasedExp = E + 16383;                      // always valid for decimal

                UInt64 sigLo = (UInt64)(sig & UInt64.MaxValue);
                UInt64 sigHi = (UInt64)(sig >> 64);               // < 2^48

                UInt64 resultHi =
                    ((UInt64)sign << 63) |
                    ((UInt64)biasedExp << 48) |
                    sigHi;
                UInt64 resultLo = sigLo;

                return new Quadruple(resultLo, resultHi);
            }
        }

#if NET7_0_OR_GREATER
        public static implicit operator Quadruple(float value) {
            return FromIeee754InterchangeBinary<float, UInt32>(value);
        }

        public static implicit operator Quadruple(Half value) {
            return FromIeee754InterchangeBinary<Half, UInt16>(value);
        }
#else
        public static implicit operator Quadruple(float value) {
            return (Quadruple)(double)value;
        }

        public static implicit operator Quadruple(Half value) {
            return (Quadruple)(double)value;
        }
#endif
    }
}

namespace UltimateOrb {

    partial struct Quadruple {

        /// <summary>
        /// Converts a <see cref="BigInteger"/> to <see cref="Quadruple"/>
        /// (binary128) with round-to-nearest, ties-to-even.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>
        /// The correctly-rounded binary128 representation of
        /// <paramref name="value"/>, or <c>±∞</c> if the value's magnitude
        /// exceeds the finite binary128 range.
        /// </returns>
        /// <remarks>
        /// <para>
        /// <b>Rounding.</b> Values that fit in 113 bits are converted exactly.
        /// Otherwise the value is rounded to the nearest 113-bit significand,
        /// with ties resolved to even. Rounding may carry, in which case the
        /// significand is renormalized and the exponent incremented by one.
        /// </para>
        /// <para>
        /// <b>Overflow.</b> Any magnitude with 16385 bits or more is guaranteed
        /// to overflow and returns <see cref="Quadruple.PositiveInfinity"/> or
        /// <see cref="Quadruple.NegativeInfinity"/>. Magnitudes with 16384 bits
        /// may overflow if rounding carries at the top of the range; that case
        /// is handled by the ordinary renormalization plus the
        /// <c>biased &gt;= QuadExpMax</c> check.
        /// </para>
        /// <para>
        /// <b>Sign and zero.</b> Zero converts to <c>+0.0</c>. The sign is
        /// preserved for all other values, including values that overflow to
        /// infinity.
        /// </para>
        /// </remarks>
        public static explicit operator Quadruple(BigInteger value) {
            const int QuadFracBits = 112;
            const int QuadBias = 16383;
            const int QuadExpMax = 0x7FFF;

            if (value.IsZero) {
                return new Quadruple(0UL, 0UL);
            }

            bool negative = value.Sign < 0;
            BigInteger mag = BigInteger.Abs(value);

            UInt64 signHi = negative ? (1UL << 63) : 0UL;
            UInt64 infNanHi = signHi | ((UInt64)QuadExpMax << 48);

            long nLong = mag.GetBitLength();
            Debug.Assert(nLong >= 1);

            // magnitudes with n >= 16385 always overflow binary128.
            // n == 16384 may still overflow if rounding carries; that path is
            // handled by the biased-exponent check below.
            if (nLong > 16384) {
                return new Quadruple(0UL, infNanHi);
            }

            int n = (int)nLong;

            UInt128 sig113;    // 113-bit significand, MSB at bit 112
            int unbiased;      // value = sig113 * 2^(unbiased - 112)

            if (n <= QuadFracBits + 1) {
                // ---- Exact: 113 bits or fewer, no rounding needed. ----
                // Left-shift so the magnitude's MSB lands at bit 112.
                int shift = (QuadFracBits + 1) - n;    // 0..112
                sig113 = ToUInt128(mag) << shift;
                unbiased = n - 1;
            } else {
                // ---- Rounding: drop (n - 113) low bits, half-to-even. ----
                int drop = n - (QuadFracBits + 1);

                BigInteger topBig = mag >> drop;                       // 113 bits
                BigInteger dropped = mag & ((BigInteger.One << drop) - 1);

                UInt128 top = ToUInt128(topBig);

                BigInteger half = BigInteger.One << (drop - 1);
                if (dropped > half ||
                    (dropped == half && (top & (UInt128)1) != UInt128.Zero)) {
                    top++;
                    if (top == ((UInt128)1 << (QuadFracBits + 1))) {
                        // Carry out of the 113-bit significand: renormalize.
                        // (When the exponent is at emax, the resulting biased
                        // exponent becomes 0x7FFF and is caught below.)
                        top >>= 1;
                        unbiased = n;      // bumped from n - 1
                    } else {
                        unbiased = n - 1;
                    }
                } else {
                    unbiased = n - 1;
                }

                sig113 = top;
            }

            int biased = unbiased + QuadBias;

            // Overflow → ±Infinity.
            if (biased >= QuadExpMax) {
                return new Quadruple(0UL, infNanHi);
            }

            Debug.Assert(biased > 0);   // n >= 1 ⇒ unbiased >= 0 ⇒ biased >= 16383

            // Strip the implicit leading 1 (bit 112).
            UInt128 frac = sig113 & (((UInt128)1 << QuadFracBits) - 1);

            UInt64 fracLo = (UInt64)frac;
            UInt64 fracHi = (UInt64)(frac >> 64) & 0x0000_FFFF_FFFF_FFFFUL;

            UInt64 hi = signHi | ((UInt64)biased << 48) | fracHi;
            UInt64 lo = fracLo;

            return new Quadruple(lo, hi);

            static UInt128 ToUInt128(BigInteger v) {
                Debug.Assert(!BigInteger.IsNegative(v));
                Debug.Assert(v.GetBitLength() <= 128);
                return unchecked((UltimateOrb.UInt128)v);
            }
        }
    }
}

namespace UltimateOrb {
    public readonly partial struct Quadruple {

        public bool TryFormat(Span<char> destination, out int charsWritten,
                              ReadOnlySpan<char> format, IFormatProvider? provider)
            => QuadrupleFormatter.TryFormat(_Lo64Bits, _Hi64Bits,
                                            destination, out charsWritten, format, provider);

        // ----------------------------------------------------------------------------
        // Bit decomposition:  |v| = Significand · 2^Exp2
        //   normal:    sig ∈ [2^112, 2^113), exp2 ∈ [-16494, 16271]
        //   subnormal: sig ∈ [1, 2^112),   exp2 = -16494
        // ----------------------------------------------------------------------------
        internal readonly struct Decoded {
            public readonly bool IsNegative, IsNaN, IsPosInf, IsNegInf, IsZero;
            public readonly BigInteger Significand;
            public readonly int Exp2;

            private Decoded(bool neg, bool nan, bool posinf, bool neginf, bool zero,
                            BigInteger sig, int e2) {
                IsNegative = neg; IsNaN = nan; IsPosInf = posinf; IsNegInf = neginf;
                IsZero = zero; Significand = sig; Exp2 = e2;
            }

            public static Decoded Decode(ulong lo, ulong hi) {
                bool neg = (hi >> 63) != 0;
                int biased = (int)((hi >> 48) & 0x7FFF);
                ulong hiMant = hi & 0x0000_FFFF_FFFF_FFFFUL;
                BigInteger fr = ((BigInteger)hiMant << 64) | lo;

                if (biased == 0x7FFF)
                    return fr.IsZero
                        ? (neg ? new Decoded(true, false, false, true, false, default, 0)
                               : new Decoded(false, false, true, false, false, default, 0))
                        : new Decoded(neg, true, false, false, false, default, 0);

                if (biased == 0)
                    return fr.IsZero
                        ? new Decoded(neg, false, false, false, true, default, 0)
                        : new Decoded(neg, false, false, false, false, fr, -16494);

                return new Decoded(neg, false, false, false, false,
                                   fr | (BigInteger.One << 112), biased - 16495);
            }
        }

        // ----------------------------------------------------------------------------
        // Small helpers
        // ----------------------------------------------------------------------------
        internal static class BigIntLog {
            // floor(log2(x)) for x > 0.  Trivial because BigInteger gives us bit length.
            public static int ILog2(BigInteger x) {
                Debug.Assert(x.Sign > 0);
                return (int)(x.GetBitLength() - 1);
            }
        }

        internal ref struct Writer {
            public Span<char> Buf; public int Pos;
            public Writer(Span<char> buf) { Buf = buf; Pos = 0; }
            public bool TryAppend(char c) { if (Pos >= Buf.Length) return false; Buf[Pos++] = c; return true; }
            public bool TryAppend(scoped ReadOnlySpan<char> s) { if (s.Length > Buf.Length - Pos) return false; s.CopyTo(Buf[Pos..]); Pos += s.Length; return true; }
            public bool TryAppend(string s) => TryAppend(s.AsSpan());
            public bool TryAppendZeros(int n) { if (n > Buf.Length - Pos) return false; Buf.Slice(Pos, n).Fill('0'); Pos += n; return true; }
        }

        internal readonly struct Ratio {
            public readonly BigInteger Num, Den;
            private Ratio(BigInteger n, BigInteger d) { Num = n; Den = d; }
            public static Ratio FromSigExp2(BigInteger sig, int exp2)
                => exp2 >= 0 ? new Ratio(sig << exp2, BigInteger.One)
                             : new Ratio(sig, BigInteger.One << -exp2);
            public static Ratio FromDec(BigInteger dNum, int dExp)
                => dExp >= 0 ? new Ratio(dNum * BigInteger.Pow(10, dExp), BigInteger.One)
                             : new Ratio(dNum, BigInteger.Pow(10, -dExp));
            public static Ratio Half(in Ratio a) => new Ratio(a.Num, a.Den << 1);
            public static Ratio Midpoint(in Ratio a, in Ratio b)
                => new Ratio(a.Num * b.Den + b.Num * a.Den, (a.Den * b.Den) << 1);
            public static int Compare(in Ratio a, in Ratio b)
                => (a.Num * b.Den).CompareTo(b.Num * a.Den);
        }

        // ----------------------------------------------------------------------------
        // Exact decimal digit extraction
        // ----------------------------------------------------------------------------
        internal static class DecimalDigits {
            private const double Log10Of2 = 0.30102999566398119521;
            // Any Quadruple's exact decimal has at most ~11,600 significant digits
            // and at most 16,494 fractional positions.  Cap above both.
            public const int MaxSignificantComputed = 11_563 + 4;
            public const int MaxFractionalComputed = 16_494 + 4;

            // sign(sig · 2^exp2  −  10^E), exact.
            public static int CompareSigExp2To10Pow(BigInteger sig, int exp2, int E) {
                if (E >= 0) {
                    int d = exp2 - E;
                    BigInteger p5 = BigInteger.Pow(5, E);
                    return d >= 0 ? (sig << d).CompareTo(p5) : sig.CompareTo(p5 << -d);
                } else {
                    int ne = -E, d = exp2 + ne;
                    BigInteger p5 = BigInteger.Pow(5, ne);
                    return d >= 0 ? ((sig << d) * p5).CompareTo(BigInteger.One)
                                  : (sig * p5).CompareTo(BigInteger.One << -d);
                }
            }

            // floor(log10(|v|)),  exact.
            public static int DecimalExponent(BigInteger sig, int exp2) {
                Debug.Assert(sig.Sign > 0);
                long log2v = (long)BigIntLog.ILog2(sig) + exp2;
                int E = (int)System.Math.Floor(log2v * Log10Of2);

                // At most ±1 correction.
                while (CompareSigExp2To10Pow(sig, exp2, E) < 0) E--;
                while (CompareSigExp2To10Pow(sig, exp2, E + 1) >= 0) E++;
                return E;
            }

            // round-half-to-even(sig · 2^exp2 · 10^k), exact.
            public static BigInteger RoundToScale(BigInteger sig, int exp2, int k) {
                int e2 = exp2 + k;
                BigInteger num = sig, den = BigInteger.One;
                if (e2 >= 0) num <<= e2; else den <<= -e2;
                if (k >= 0) num *= BigInteger.Pow(5, k);
                else den *= BigInteger.Pow(5, -k);

                BigInteger q = BigInteger.DivRem(num, den, out BigInteger r);
                BigInteger twice = r << 1;
                int c = twice.CompareTo(den);
                if (c > 0 || (c == 0 && !q.IsEven)) q += BigInteger.One;
                return q;
            }
            public static void GetSignificant(BigInteger sig, int exp2, int N,
                                                out string digits, out int decExp) {
                // N is the logical digit count.  We compute at most
                // MaxSignificantComputed and pad with zeros in the writer.
                int compute = N > MaxSignificantComputed ? MaxSignificantComputed : N;
                if (sig.IsZero) { digits = "0"; decExp = 1; return; }

                int E = DecimalExponent(sig, exp2);
                BigInteger q = RoundToScale(sig, exp2, compute - 1 - E);
                string s = q.ToString();
                if (s.Length == compute + 1) { s = s[..compute]; E++; }
                digits = s; decExp = E + 1;
            }

            public static void GetFixed(BigInteger sig, int exp2, int fracDigits,
                                        out string digits, out int decExp) {
                int compute = fracDigits > MaxFractionalComputed
                            ? MaxFractionalComputed : fracDigits;
                if (sig.IsZero) {
                    digits = new string('0', System.Math.Max(1, compute + 1));
                    // NOTE: decExp uses `compute`, not `fracDigits`.
                    decExp = digits.Length - compute;
                    return;
                }
                BigInteger q = RoundToScale(sig, exp2, compute);
                digits = q.ToString();
                // decExp = digits.Length - compute, NOT digits.Length - fracDigits.
                // The value is q / 10^compute; the writer will append zeros beyond
                // `compute` fractional digits without changing decExp.
                decExp = digits.Length - compute;
            }
        }

        // ----------------------------------------------------------------------------
        // Shortest round-trip digits, via exact basin test
        // ----------------------------------------------------------------------------
        internal static class RoundTripDigits {
            private const int MaxN = 37;   // ⌈113·log10 2⌉ + 1


            static readonly BigInteger p112 = BigInteger.One << 112;
            static readonly BigInteger p113 = BigInteger.One << 113;


            // Neighbours of sig · 2^exp2.  A zero result means "no neighbour"
            // (below smallest positive / above largest finite).
            private static void Neighbours(BigInteger sig, int exp2,
                                           out BigInteger pS, out int pE,
                                           out BigInteger nS, out int nE) {
                // prev
                if (sig == BigInteger.One && exp2 == -16494) { pS = 0; pE = 0; } else if (sig == p112 && exp2 > -16494) { pS = p113 - 1; pE = exp2 - 1; } else { pS = sig - 1; pE = exp2; }

                // next
                if (sig == p113 - 1 && exp2 == 16271) { nS = 0; nE = 0; }   // don't touch pE
                else if (sig == p113 - 1) { nS = p112; nE = exp2 + 1; } else { nS = sig + 1; nE = exp2; }
            }

            // Does decimal  dNum · 10^dExp  lie in the (nearest-even) basin of sig · 2^exp2?
            public static bool IsInBasin(BigInteger sig, int exp2, BigInteger dNum, int dExp) {
                Debug.Assert(sig.Sign > 0);
                var v = Ratio.FromSigExp2(sig, exp2);
                var d = Ratio.FromDec(dNum, dExp);

                bool sigEven = sig.IsEven;
                Neighbours(sig, exp2, out var pS, out var pE, out var nS, out var nE);

                // Lower boundary
                if (pS.IsZero) {
                    // v is the smallest positive Quadruple → mid-point with 0
                    var vm = Ratio.Half(v);
                    int c = Ratio.Compare(d, vm);
                    if (c < 0) return false;
                    if (c == 0 && !sigEven) return false;
                } else {
                    var prev = Ratio.FromSigExp2(pS, pE);
                    var mid = Ratio.Midpoint(v, prev);
                    int c = Ratio.Compare(d, mid);
                    if (c < 0) return false;
                    if (c == 0 && !sigEven) return false;
                }

                // Upper boundary
                if (!nS.IsZero) {
                    var next = Ratio.FromSigExp2(nS, nE);
                    var mid = Ratio.Midpoint(v, next);
                    int c = Ratio.Compare(d, mid);
                    if (c > 0) return false;
                    if (c == 0 && !sigEven) return false;
                } else {
                    // next "would-be" Quadruple is 1.000…0 × 2^16384
                    var next = Ratio.FromSigExp2(p112, 16272);
                    var mid = Ratio.Midpoint(v, next);
                    int c = Ratio.Compare(d, mid);
                    if (c > 0) return false;
                    if (c == 0 && !sigEven) return false;
                }
                return true;
            }

            // Shortest decimal that round-trips.
            public static void Get(BigInteger sig, int exp2, out string digits, out int decExp) {
                for (int N = 1; N <= MaxN; N++) {
                    DecimalDigits.GetSignificant(sig, exp2, N, out var d, out var e);
                    var dNum = BigInteger.Parse(d);
                    int dExp = e - d.Length;

                    if (IsInBasin(sig, exp2, dNum, dExp)) {
                        digits = d; decExp = e; return;
                    }
                }
                // Unreachable for any finite, non-zero Quadruple.
                DecimalDigits.GetSignificant(sig, exp2, MaxN, out digits, out decExp);
            }
        }
        internal static class QuadrupleFormatter {
            public static bool TryFormat(ulong lo, ulong hi, Span<char> dst, out int written,
                                         ReadOnlySpan<char> format, IFormatProvider? provider) {
                var nfi = NumberFormatInfo.GetInstance(provider);
                var d = Decoded.Decode(lo, hi);

                if (d.IsNaN) return Copy(nfi.NaNSymbol.AsSpan(), dst, out written);
                if (d.IsPosInf) return Copy(nfi.PositiveInfinitySymbol.AsSpan(), dst, out written);
                if (d.IsNegInf) return Copy(nfi.NegativeInfinitySymbol.AsSpan(), dst, out written);

                if (format.IsEmpty) return General(dst, out written, d, -1, nfi);

                char c = format[0];
                int precision = -1;
                bool hasPrec = false;
                bool stdValid = char.IsLetter(c);

                if (stdValid && format.Length > 1) {
                    var rest = format[1..];
                    bool allDigits = rest.Length > 0;
                    for (int i = 0; i < rest.Length && allDigits; i++)
                        if (rest[i] < '0' || rest[i] > '9') allDigits = false;

                    if (allDigits) {
                        long prec = 0;
                        for (int i = 0; i < rest.Length; i++) {
                            prec = prec * 10 + (rest[i] - '0');
                            if (prec > 999_999_999)
                                throw new FormatException(
                                    $"Precision specifier in '{format.ToString()}' exceeds the maximum of 999,999,999.");
                        }
                        precision = (int)prec;
                        hasPrec = true;
                    } else {
                        stdValid = false;
                    }
                }

                if (!stdValid)
                    return CustomFormat(dst, out written, d, format, nfi);

                switch (char.ToUpperInvariant(c)) {
                case 'G': return General(dst, out written, d, hasPrec ? precision : -1, nfi);
                case 'F':
                    return Fixed(dst, out written, d,
                                              hasPrec ? precision : nfi.NumberDecimalDigits, nfi);
                case 'E':
                    return Exponential(dst, out written, d,
                                              hasPrec ? precision : 6, char.IsUpper(c), nfi);
                case 'N':
                    return Numeric(dst, out written, d,
                                              hasPrec ? precision : nfi.NumberDecimalDigits, nfi);
                case 'R': return RoundTrip(dst, out written, d, nfi);
                case 'C':
                    return Currency(dst, out written, d,
                                              hasPrec ? precision : nfi.CurrencyDecimalDigits, nfi);
                case 'P':
                    return Percent(dst, out written, d,
                                              hasPrec ? precision : nfi.PercentDecimalDigits, nfi);
                default:
                    throw new FormatException($"Format specifier '{c}' was invalid.");
                }
            }

            // ------------------------------------------------------------------ util
            private static bool Copy(ReadOnlySpan<char> s, Span<char> dst, out int written) {
                if (s.Length > dst.Length) { written = 0; return false; }
                s.CopyTo(dst); written = s.Length; return true;
            }
            private static bool TryParsePrecision(ReadOnlySpan<char> s, out int v) {
                v = 0;
                if (s.IsEmpty) return false;
                long acc = 0;
                foreach (char c in s) {
                    if (c < '0' || c > '9') return false;
                    acc = acc * 10 + (c - '0');
                    if (acc > 999_999_999) return false;
                }
                v = (int)acc; return true;
            }
            private static bool Sign(bool neg, ref Writer w, NumberFormatInfo nfi)
                => !neg || w.TryAppend(nfi.NegativeSign);
            private static string TrimZeros(string s) {
                int n = s.Length;
                while (n > 1 && s[n - 1] == '0') n--;
                return s[..n];
            }

            // Emit "0.digits × 10^decExp" as fixed-point, padding with 0/omitting as needed.
            // Replaces WriteFixed entirely.
            private static bool WriteFixed(string digits, int decExp, int fracDigits,
                                           NumberFormatInfo nfi, ref Writer w) {
                if (decExp <= 0) {
                    if (!w.TryAppend('0')) return false;
                    if (fracDigits <= 0) return true;
                    if (!w.TryAppend(nfi.NumberDecimalSeparator)) return false;
                    int lead = System.Math.Min(-decExp, fracDigits);
                    if (!w.TryAppendZeros(lead)) return false;
                    int fromDigits = System.Math.Min(digits.Length, fracDigits - lead);
                    if (fromDigits > 0 && !w.TryAppend(digits.AsSpan(0, fromDigits))) return false;
                    int trailing = fracDigits - lead - fromDigits;
                    if (trailing > 0 && !w.TryAppendZeros(trailing)) return false;
                    return true;
                }

                // decExp > 0: integer part.
                int intFromDigits = System.Math.Min(digits.Length, decExp);
                if (intFromDigits > 0 && !w.TryAppend(digits.AsSpan(0, intFromDigits))) return false;
                if (intFromDigits < decExp && !w.TryAppendZeros(decExp - intFromDigits)) return false;

                if (fracDigits <= 0) return true;
                if (!w.TryAppend(nfi.NumberDecimalSeparator)) return false;

                int fracStart = decExp;
                if (fracStart >= digits.Length) {
                    if (!w.TryAppendZeros(fracDigits)) return false;
                    return true;
                }
                int fromDigits2 = System.Math.Min(digits.Length - fracStart, fracDigits);
                if (!w.TryAppend(digits.AsSpan(fracStart, fromDigits2))) return false;
                int trailing2 = fracDigits - fromDigits2;
                if (trailing2 > 0 && !w.TryAppendZeros(trailing2)) return false;
                return true;
            }

            private static bool WriteGrouped(string digits, int decExp, int fracDigits,
                                 int[] sizes, string grp, string dec,
                                 ref Writer w) {
                if (decExp <= 0) {
                    if (!w.TryAppend('0')) return false;
                    if (fracDigits <= 0) return true;
                    if (!w.TryAppend(dec)) return false;
                    int lead = System.Math.Min(-decExp, fracDigits);
                    if (!w.TryAppendZeros(lead)) return false;
                    int fromDigits = System.Math.Min(digits.Length, fracDigits - lead);
                    if (fromDigits > 0 && !w.TryAppend(digits.AsSpan(0, fromDigits))) return false;
                    int trailing = fracDigits - lead - fromDigits;
                    if (trailing > 0 && !w.TryAppendZeros(trailing)) return false;
                    return true;
                }

                int intLen = decExp;
                int gs = sizes.Length > 0 ? sizes[0] : 0;

                if (gs <= 0 || intLen <= gs) {
                    int real = System.Math.Min(digits.Length, intLen);
                    if (real > 0 && !w.TryAppend(digits.AsSpan(0, real))) return false;
                    if (real < intLen && !w.TryAppendZeros(intLen - real)) return false;
                } else {
                    for (int i = 0; i < intLen; i++) {
                        if (i > 0 && (intLen - i) % gs == 0)
                            if (!w.TryAppend(grp)) return false;
                        char ch = i < digits.Length ? digits[i] : '0';
                        if (!w.TryAppend(ch)) return false;
                    }
                }

                if (fracDigits <= 0) return true;
                if (!w.TryAppend(dec)) return false;

                int fracStart = intLen;
                if (fracStart >= digits.Length) {
                    if (!w.TryAppendZeros(fracDigits)) return false;
                    return true;
                }
                int fromDigits2 = System.Math.Min(digits.Length - fracStart, fracDigits);
                if (!w.TryAppend(digits.AsSpan(fracStart, fromDigits2))) return false;
                int trailing2 = fracDigits - fromDigits2;
                if (trailing2 > 0 && !w.TryAppendZeros(trailing2)) return false;
                return true;
            }

            // WriteScientific gains a targetFracDigits parameter.
            private static bool WriteScientific(string digits, int decExp,
                                                int targetFracDigits,
                                                ref Writer w, NumberFormatInfo nfi,
                                                char ec, int minExpDigits, bool plusAlways) {
                if (!w.TryAppend(digits[0])) return false;

                // Fraction body: emit digits[1..] then pad with zeros to reach targetFracDigits.
                int fracFromDigits = System.Math.Min(digits.Length - 1, targetFracDigits);
                if (targetFracDigits > 0) {
                    if (!w.TryAppend(nfi.NumberDecimalSeparator)) return false;
                    if (fracFromDigits > 0 &&
                        !w.TryAppend(digits.AsSpan(1, fracFromDigits))) return false;
                    int trailing = targetFracDigits - fracFromDigits;
                    if (trailing > 0 && !w.TryAppendZeros(trailing)) return false;
                }

                if (!w.TryAppend(ec)) return false;
                int e = decExp - 1;
                if (e < 0) { if (!w.TryAppend(nfi.NegativeSign)) return false; e = -e; } else if (plusAlways) { if (!w.TryAppend(nfi.PositiveSign)) return false; }

                Span<char> tmp = stackalloc char[16];
                int p = tmp.Length;
                if (e == 0) tmp[--p] = '0';
                while (e > 0) { tmp[--p] = (char)('0' + e % 10); e /= 10; }
                int n = tmp.Length - p;
                if (n < minExpDigits && !w.TryAppendZeros(minExpDigits - n)) return false;
                return w.TryAppend(tmp[p..]);
            }

            // ------------------------------------------------------------------ G
            public static bool General(Span<char> dst, out int written, in Decoded d,
                           int precision, NumberFormatInfo nfi) {
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    written = wz.Pos; return true;
                }

                string digits;
                int decExp;
                int threshold;

                if (precision <= 0) {
                    // Default G: shortest round-trip digits.
                    RoundTripDigits.Get(d.Significand, d.Exp2, out digits, out decExp);

                    // Threshold for the fixed/scientific decision.
                    // Mirrors .NET's Double (=15) and Single (=7):
                    //   floor(significandBits · log10(2)).
                    // For Quadruple, 113 bits → 34.
                    threshold = 34;
                } else {
                    DecimalDigits.GetSignificant(d.Significand, d.Exp2, precision,
                                                 out digits, out decExp);
                    digits = TrimZeros(digits);
                    threshold = precision;
                }

                int sciExp = decExp - 1;
                bool useSci = sciExp < -4 || sciExp >= threshold;

                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;

                if (useSci) {
                    int targetFrac = digits.Length > 1 ? digits.Length - 1 : 0;
                    if (!WriteScientific(digits, decExp, targetFrac, ref w, nfi, 'E', 2, true)) goto fail;
                } else {
                    // Fixed-point. WriteFixed handles decExp ≤ 0 (emitting "0." and
                    // leading zeros), 0 < decExp < digits.Length, and decExp ≥ digits.Length
                    // (padding with trailing zeros). `digits` is trailing-zero-free in both
                    // the R-like and G<prec> paths, so fracDigits = digits.Length − decExp
                    // is exactly the number of fractional digits to emit.
                    if (!WriteFixed(digits, decExp, digits.Length - decExp, nfi, ref w)) goto fail;
                }
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ F
            public static bool Fixed(Span<char> dst, out int written, in Decoded d,
                                     int fracDigits, NumberFormatInfo nfi) {
                if (fracDigits < 0) fracDigits = 0;
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    if (fracDigits > 0) {
                        if (!wz.TryAppend(nfi.NumberDecimalSeparator)) goto fail;
                        if (!wz.TryAppendZeros(fracDigits)) goto fail;
                    }
                    written = wz.Pos; return true;
                }
                DecimalDigits.GetFixed(d.Significand, d.Exp2, fracDigits,
                                       out var digits, out int decExp);
                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (!WriteFixed(digits, decExp, fracDigits, nfi, ref w)) goto fail;
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ E
            // Exponential
            public static bool Exponential(Span<char> dst, out int written, in Decoded d,
                                           int precision, bool upper, NumberFormatInfo nfi) {
                char ec = upper ? 'E' : 'e';
                if (d.IsZero) { /* unchanged zero branch */ }

                DecimalDigits.GetSignificant(d.Significand, d.Exp2, precision + 1,
                                             out var digits, out int decExp);
                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (!WriteScientific(digits, decExp, precision, ref w, nfi, ec, 3, true)) goto fail;
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ N
            public static bool Numeric(Span<char> dst, out int written, in Decoded d,
                                       int fracDigits, NumberFormatInfo nfi) {
                if (fracDigits < 0) fracDigits = 0;
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    if (fracDigits > 0) {
                        if (!wz.TryAppend(nfi.NumberDecimalSeparator)) goto fail;
                        if (!wz.TryAppendZeros(fracDigits)) goto fail;
                    }
                    written = wz.Pos; return true;
                }
                DecimalDigits.GetFixed(d.Significand, d.Exp2, fracDigits,
                                       out var digits, out int decExp);
                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (!WriteGrouped(digits, decExp, fracDigits,
                                  nfi.NumberGroupSizes, nfi.NumberGroupSeparator,
                                  nfi.NumberDecimalSeparator, ref w)) goto fail;
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ R
            public static bool RoundTrip(Span<char> dst, out int written, in Decoded d,
                                         NumberFormatInfo nfi) {
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    written = wz.Pos; return true;
                }
                RoundTripDigits.Get(d.Significand, d.Exp2, out var digits, out int decExp);

                int sciExp = decExp - 1;
                bool useSci = sciExp < -4 || sciExp >= digits.Length;

                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (useSci) {
                    int targetFrac = digits.Length > 1 ? digits.Length - 1 : 0;
                    if (!WriteScientific(digits, decExp, targetFrac, ref w, nfi, 'E', 2, true)) goto fail;
                } else {
                    if (!WriteFixed(digits, decExp, digits.Length - decExp, nfi, ref w)) goto fail;
                }
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ C / P
            private static readonly string[] CurrPos = { "$n", "n$", "$ n", "n $" };
            private static readonly string[] CurrNeg =
            {
        "($n)","-$n","$-n","$n-","(n$)","-n$","n-$","n$-",
        "-n $","-$ n","n $-","$ n-","$ -n","n- $","($ n)","(n $)"
    };
            private static readonly string[] PctPos = { "n %", "n%", "%n", "% n" };
            private static readonly string[] PctNeg =
            {
        "-n %","-n%","-%n","%-n","-%n","n-%","n%-","-%n",
        "n %-","-n %","% n-","% -n"
    };

            private static bool EmitPattern(Span<char> dst, out int written, string pat,
                                            ReadOnlySpan<char> body, string sym,
                                            NumberFormatInfo nfi) {
                var w = new Writer(dst);
                foreach (char c in pat) {
                    switch (c) {
                    case 'n': if (!w.TryAppend(body)) { written = 0; return false; } break;
                    case '$':
                    case '%': if (!w.TryAppend(sym.AsSpan())) { written = 0; return false; } break;
                    case '-': if (!w.TryAppend(nfi.NegativeSign.AsSpan())) { written = 0; return false; } break;
                    default: if (!w.TryAppend(c)) { written = 0; return false; } break;
                    }
                }
                written = w.Pos; return true;
            }

            public static bool Currency(Span<char> dst, out int written, in Decoded d,
                                        int fracDigits, NumberFormatInfo nfi) {
                if (fracDigits < 0) fracDigits = 0;
                // body computed with C-specific group/decimal separators
                Span<char> bodyBuf = stackalloc char[512];
                if (!CurrencyBody(bodyBuf, out int bodyLen, d, fracDigits, nfi)) { written = 0; return false; }
                var body = bodyBuf[..bodyLen];

                int p = d.IsNegative ? nfi.CurrencyNegativePattern : nfi.CurrencyPositivePattern;
                string pat = d.IsNegative ? CurrNeg[p] : CurrPos[p];
                return EmitPattern(dst, out written, pat, body, nfi.CurrencySymbol, nfi);
            }

            private static bool CurrencyBody(Span<char> dst, out int written, in Decoded d,
                                             int fracDigits, NumberFormatInfo nfi) {
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    if (fracDigits > 0) {
                        if (!wz.TryAppend(nfi.CurrencyDecimalSeparator)) goto fail;
                        if (!wz.TryAppendZeros(fracDigits)) goto fail;
                    }
                    written = wz.Pos; return true;
                }
                DecimalDigits.GetFixed(d.Significand, d.Exp2, fracDigits,
                                       out var digits, out int decExp);
                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (!WriteGrouped(digits, decExp, fracDigits,
                                  nfi.CurrencyGroupSizes, nfi.CurrencyGroupSeparator,
                                  nfi.CurrencyDecimalSeparator, ref w)) goto fail;
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            public static bool Percent(Span<char> dst, out int written, in Decoded d,
                                       int fracDigits, NumberFormatInfo nfi) {
                if (fracDigits < 0) fracDigits = 0;

                // ×100 = add 2 to decExp — compute digits on the scaled significand.
                Span<char> bodyBuf = stackalloc char[512];
                if (!PercentBody(bodyBuf, out int bodyLen, d, fracDigits, nfi)) { written = 0; return false; }
                var body = bodyBuf[..bodyLen];

                int p = d.IsNegative ? nfi.PercentNegativePattern : nfi.PercentPositivePattern;
                string pat = d.IsNegative ? PctNeg[p] : PctPos[p];
                return EmitPattern(dst, out written, pat, body, nfi.PercentSymbol, nfi);
            }

            private static bool PercentBody(Span<char> dst, out int written, in Decoded d,
                                            int fracDigits, NumberFormatInfo nfi) {
                if (d.IsZero) {
                    var wz = new Writer(dst);
                    if (!Sign(d.IsNegative, ref wz, nfi)) goto fail;
                    if (!wz.TryAppend('0')) goto fail;
                    if (fracDigits > 0) {
                        if (!wz.TryAppend(nfi.PercentDecimalSeparator)) goto fail;
                        if (!wz.TryAppendZeros(fracDigits)) goto fail;
                    }
                    written = wz.Pos; return true;
                }
                BigInteger scaledSig = d.Significand * 100;
                DecimalDigits.GetFixed(scaledSig, d.Exp2, fracDigits,
                                       out var digits, out int decExp);
                var w = new Writer(dst);
                if (!Sign(d.IsNegative, ref w, nfi)) goto fail;
                if (!WriteGrouped(digits, decExp, fracDigits,
                                  nfi.PercentGroupSizes, nfi.PercentGroupSeparator,
                                  nfi.PercentDecimalSeparator, ref w)) goto fail;
                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            // ------------------------------------------------------------------ custom
            // Handles the standard custom specifiers: 0 # . , %  ‰  E0 E+0 E-0  ' "
            // escape with \  sections with ;  literal chars.
            public static bool CustomFormat(Span<char> dst, out int written, in Decoded d,
                                ReadOnlySpan<char> fmt, NumberFormatInfo nfi) {
                // ---- 1. Split sections -------------------------------------------------
                ReadOnlySpan<char> pos = fmt, neg = fmt, zero = fmt;
                int s1 = fmt.IndexOf(';');
                if (s1 >= 0) {
                    pos = fmt[..s1];
                    var rest = fmt[(s1 + 1)..];
                    int s2 = rest.IndexOf(';');
                    if (s2 < 0) { neg = rest; zero = pos; } else { neg = rest[..s2]; zero = rest[(s2 + 1)..]; }
                }
                var section = d.IsZero ? zero : (d.IsNegative ? neg : pos);
                if (section.IsEmpty) section = pos;

                // ---- 2. Pre-scan -------------------------------------------------------
                int intZeros = 0, fracZeros = 0, intHashes = 0, fracHashes = 0;
                bool hasPoint = false, hasExp = false, sciPlus = false;
                char sciChar = 'E'; int sciZeros = 0;
                int percentMul = 0, perMilleMul = 0, scaleCommas = 0;
                int lastIntPlaceholder = -1;

                bool beforePoint = true;
                for (int i = 0; i < section.Length; i++) {
                    char c = section[i];
                    if (c == '\\') { i++; continue; }
                    if (c == '\'' || c == '"') {
                        char q = c; i++;
                        while (i < section.Length && section[i] != q) { if (section[i] == '\\') i++; i++; }
                        continue;
                    }
                    switch (c) {
                    case '0':
                        if (beforePoint) { intZeros++; lastIntPlaceholder = i; } else fracZeros++;
                        break;
                    case '#':
                        if (beforePoint) { intHashes++; lastIntPlaceholder = i; } else fracHashes++;
                        break;
                    case '.':
                        if (beforePoint) { hasPoint = true; beforePoint = false; }
                        break;
                    case ',':
                        // Grouping = between two placeholders.  Scaling = before '.' with no
                        // intervening placeholder.  We'll count scaling below the loop.
                        break;
                    case '%': percentMul++; break;
                    case '‰': perMilleMul++; break;
                    case 'E':
                    case 'e':
                        int j = i + 1;
                        if (j < section.Length && (section[j] == '+' || section[j] == '-')) {
                            sciPlus = section[j] == '+'; j++;
                        }
                        if (j < section.Length && section[j] == '0') {
                            hasExp = true; sciChar = c;
                            while (j < section.Length && section[j] == '0') { sciZeros++; j++; }
                        }
                        break;
                    }
                }
                // Scaling commas: commas strictly to the right of the last integer placeholder
                // and left of the decimal point (or end of integer part if no point).
                int intEnd = section.Length;
                for (int i = 0; i < section.Length; i++) if (section[i] == '.') { intEnd = i; break; }
                for (int i = System.Math.Max(lastIntPlaceholder + 1, 0); i < intEnd; i++)
                    if (section[i] == ',') scaleCommas++;

                int multiplier = percentMul * 100 + perMilleMul * 1000;
                int scaleShift = 3 * scaleCommas;

                // ---- 3. Compute digits for the (possibly scaled) value -----------------
                BigInteger scaledSig = multiplier > 0 ? d.Significand * multiplier : d.Significand;
                int M_int = intZeros + intHashes;
                int M_frac = fracZeros + fracHashes;
                int totalFrac = M_frac;

                string digits; int decExp;

                if (d.IsZero) {
                    digits = "0"; decExp = 1;
                } else if (hasExp) {
                    int sigDigits = System.Math.Min(1 + totalFrac, DecimalDigits.MaxSignificantComputed);
                    DecimalDigits.GetSignificant(scaledSig, d.Exp2, sigDigits, out digits, out decExp);
                    decExp -= scaleShift;
                } else if (totalFrac > 0) {
                    // Fixed with fractional digits; scaling must affect the rounding position
                    // so we pass an adjusted k to RoundToScale.
                    int k = totalFrac - scaleShift;
                    BigInteger q = DecimalDigits.RoundToScale(scaledSig, d.Exp2, k);
                    digits = q.ToString();
                    decExp = digits.Length - totalFrac;
                } else {
                    // No fractional placeholders: round to integer position relative to the
                    // scaled value.
                    BigInteger q = DecimalDigits.RoundToScale(scaledSig, d.Exp2, -scaleShift);
                    digits = q.ToString();
                    decExp = digits.Length;
                }

                // ---- 4. Significant range for `#` suppression --------------------------
                int lastNonZeroIdx = digits.Length - 1;
                while (lastNonZeroIdx >= 0 && digits[lastNonZeroIdx] == '0') lastNonZeroIdx--;

                int sigHigh, sigLow;
                if (d.IsZero || lastNonZeroIdx < 0) {
                    sigHigh = int.MinValue;   // no integer position is significant
                    sigLow = int.MaxValue;   // no fractional position is significant
                } else {
                    sigHigh = decExp - 1;
                    sigLow = decExp - 1 - lastNonZeroIdx;
                }
                int nIntValueDigits = d.IsZero ? 0 : System.Math.Max(0, decExp);
                int nExtraIntDigits = System.Math.Max(0, nIntValueDigits - M_int);

                // ---- 5. Emit ----------------------------------------------------------
                var w = new Writer(dst);

                // Negative sign is supplied by us only for single-section formats.
                if (s1 < 0 && d.IsNegative && !d.IsZero)
                    if (!w.TryAppend(nfi.NegativeSign)) goto fail;

                int intPlaceholdersSeen = 0;
                int fracPlaceholdersSeen = 0;
                bool wrotePoint = false;
                bool extrasEmitted = false;
                bool expEmitted = false;

                for (int i = 0; i < section.Length; i++) {
                    char c = section[i];

                    if (c == '\\') {
                        if (i + 1 < section.Length && !w.TryAppend(section[i + 1])) goto fail;
                        i++; continue;
                    }
                    if (c == '\'' || c == '"') {
                        char q = c; i++;
                        while (i < section.Length && section[i] != q) {
                            if (section[i] == '\\' && i + 1 < section.Length) i++;
                            if (!w.TryAppend(section[i])) goto fail;
                            i++;
                        }
                        continue;
                    }

                    switch (c) {
                    case '0':
                    case '#': {
                            bool isInt = !wrotePoint;

                            // Emit leading "extra" integer digits once, right before the first
                            // integer placeholder.
                            if (isInt && !extrasEmitted) {
                                extrasEmitted = true;
                                for (int p = nIntValueDigits - 1; p >= M_int; p--) {
                                    int idx = decExp - 1 - p;
                                    char ch = unchecked((uint)idx) < unchecked((uint)digits.Length) ? digits[idx] : '0';
                                    if (!w.TryAppend(ch)) goto fail;
                                }
                            }

                            char emit = '\0';
                            if (isInt) {
                                int decPos = M_int - 1 - intPlaceholdersSeen;
                                if (c == '0') {
                                    int idx = decExp - 1 - decPos;
                                    emit = unchecked((uint)idx) < unchecked((uint)digits.Length) ? digits[idx] : '0';
                                } else if (decPos <= sigHigh) {
                                    int idx = decExp - 1 - decPos;
                                    emit = unchecked((uint)idx) < unchecked((uint)digits.Length) ? digits[idx] : '0';
                                }
                                intPlaceholdersSeen++;
                            } else {
                                int decPos = -(fracPlaceholdersSeen + 1);
                                if (c == '0') {
                                    int idx = decExp - 1 - decPos;
                                    emit = unchecked((uint)idx) < unchecked((uint)digits.Length) ? digits[idx] : '0';
                                } else if (decPos >= sigLow) {
                                    // Inside the fraction, `#` emits its digit if the position is
                                    // significant, and a padding '0' if the position is above the
                                    // significant range but there's significance further right.
                                    int idx = decExp - 1 - decPos;
                                    emit = unchecked((uint)idx) < unchecked((uint)digits.Length) ? digits[idx] : '0';
                                }
                                fracPlaceholdersSeen++;
                            }

                            if (emit != '\0' && !w.TryAppend(emit)) goto fail;
                            break;
                        }
                    case '.':
                        if (!hasPoint) break;
                        wrotePoint = true;
                        if (!w.TryAppend(nfi.NumberDecimalSeparator)) goto fail;
                        break;
                    case ',':
                        if (IsPlaceholder(section, i - 1) && IsPlaceholder(section, i + 1))
                            if (!w.TryAppend(nfi.NumberGroupSeparator)) goto fail;
                        // else: scaling separator — swallowed
                        break;
                    case '%': if (!w.TryAppend(nfi.PercentSymbol)) goto fail; break;
                    case '‰': if (!w.TryAppend(nfi.PerMilleSymbol)) goto fail; break;
                    case 'E':
                    case 'e': {
                            if (!hasExp || expEmitted) { if (!w.TryAppend(c)) goto fail; break; }
                            expEmitted = true;
                            if (!w.TryAppend(sciChar)) goto fail;
                            int expVal = decExp - 1;
                            if (expVal < 0) { if (!w.TryAppend(nfi.NegativeSign)) goto fail; expVal = -expVal; } else if (sciPlus) { if (!w.TryAppend(nfi.PositiveSign)) goto fail; }

                            Span<char> tmp = stackalloc char[16];
                            int p = tmp.Length;
                            if (expVal == 0) tmp[--p] = '0';
                            while (expVal > 0) { tmp[--p] = (char)('0' + expVal % 10); expVal /= 10; }
                            int nd = tmp.Length - p;
                            if (nd < sciZeros && !w.TryAppendZeros(sciZeros - nd)) goto fail;
                            if (!w.TryAppend(tmp[p..])) goto fail;

                            int k = i + 1;
                            if (k < section.Length && (section[k] == '+' || section[k] == '-')) k++;
                            while (k < section.Length && section[k] == '0') k++;
                            i = k - 1;
                            break;
                        }
                    default:
                        if (!w.TryAppend(c)) goto fail;
                        break;
                    }
                }

                written = w.Pos; return true;
            fail: written = 0; return false;
            }

            private static bool IsPlaceholder(ReadOnlySpan<char> s, int i)
                => i >= 0 && i < s.Length && (s[i] == '0' || s[i] == '#');
        }

        internal static class QuadrupleNumber {
            // ------------------------------------------------------------------
            // IEEE 754 binary128 constants
            // ------------------------------------------------------------------
            private const int MantissaBits = 112;
            private const int SignificandBits = 113;
            private const int ExponentBias = 16383;
            private const int InfinityExponent = 0x7FFF;
            private const int MinNormalBinExp = -16382;
            private const int MaxNormalBinExp = 16383;
            private const int MinSubnormalExp2 = -16494;    // sig · 2^exp2 for the smallest subnormal

            // NaN fraction layout, aligned with the numeric conversion convention
            // (FromIeee754InterchangeBinary / ToIeee754InterchangeBinaryNarrowing):
            //   bits 0..109  : payload data
            //   bit  110     : payload-overflow indicator
            //                  (set iff the parsed integer had any bit at position >= 110)
            //   bit  111     : quiet bit
            private const int NaNPayloadDataBits = 110;
            private const int NaNPayloadOverflowBit = 110;
            private const int NaNQuietBit = 111;

            // Fast reject bounds on the decimal exponent of the MSB.
            // MaxValue ≈ 1.19e4932, smallest positive ≈ 3.65e-4966.
            private const int MaxDecMSB = 4934;
            private const int MinDecMSB = -4970;

            // Guard against pathological input.
            private const int MaxInputDigits = 200_000;

            // ==================================================================
            // Public entry points
            // ==================================================================
            public static Quadruple ParseFloat<TChar>(ReadOnlySpan<TChar> value,
                                                      NumberStyles styles,
                                                      NumberFormatInfo info)
                where TChar : unmanaged {
                if (!TryParseFloat(value, styles, info, out Quadruple result, out _))
                    ThrowFormatException(value);
                return result;
            }

            public static bool TryParseFloat<TChar>(ReadOnlySpan<TChar> value,
                                                    NumberStyles styles,
                                                    NumberFormatInfo info,
                                                    out Quadruple result,
                                                    out int elementsConsumed)
                where TChar : unmanaged {
                if ((styles & NumberStyles.AllowHexSpecifier) != 0) {
                    if (TryParseHexFloat(value, styles, info, out result, out elementsConsumed))
                        return true;
                    return TryParseSpecialValue(value, styles, info, out result, out elementsConsumed);
                }

                if (TryParseDecimalNumber(value, styles, info, out result, out elementsConsumed))
                    return true;

                return TryParseSpecialValue(value, styles, info, out result, out elementsConsumed);
            }

            // ==================================================================
            // Decimal numeric parse
            // ==================================================================
            private static bool TryParseDecimalNumber<TChar>(ReadOnlySpan<TChar> value,
                                                             NumberStyles styles,
                                                             NumberFormatInfo info,
                                                             out Quadruple result,
                                                             out int elementsConsumed)
                where TChar : unmanaged {
                result = default;
                elementsConsumed = 0;

                int len = value.Length;
                if (len == 0) return false;
                int idx = 0;

                // ---- leading whitespace ----
                if ((styles & NumberStyles.AllowLeadingWhite) != 0) {
                    while (idx < len && IsWhite(UtfChar<TChar>.CastToUInt32(value[idx]))) idx++;
                    if (idx >= len) return false;
                }

                // ---- '(' for negative ----
                bool hadParens = false;
                bool isNegative = false;
                if ((styles & NumberStyles.AllowParentheses) != 0
                    && UtfChar<TChar>.CastToUInt32(value[idx]) == '(') {
                    hadParens = true;
                    isNegative = true;
                    idx++;
                }

                // ---- leading sign ----
                if (!hadParens && (styles & NumberStyles.AllowLeadingSign) != 0) {
                    if (TryConsumeSign(value, ref idx, info, out bool neg))
                        isNegative = neg;
                }

                // ---- leading currency symbol ----
                if ((styles & NumberStyles.AllowCurrencySymbol) != 0) {
                    if (MatchSymbol(value, idx, info.CurrencySymbol, out int c))
                        idx += c;
                }

                if (idx >= len) return false;

                // ---- mantissa digits ----
                BigInteger sig = BigInteger.Zero;
                bool seenPoint = false;
                bool anyDigits = false;
                int fracDigits = 0;
                int digitsSeen = 0;
                int maxDigits = MaxInputDigits;

                // Local helper — accumulates a decimal digit.
                void ProcessDigit(uint ch) {
                    anyDigits = true;
                    if (seenPoint) fracDigits++;
                    if (digitsSeen < maxDigits) {
                        sig = sig * 10 + (ch - '0');
                        digitsSeen++;
                    }
                    // else: silently drop digits — the value is out of Quadruple range
                    // and the estimate below will clamp to Infinity/Zero anyway.
                }

                // integer digits
                while (idx < len) {
                    uint ch = UtfChar<TChar>.CastToUInt32(value[idx]);
                    if (ch >= '0' && ch <= '9') { ProcessDigit(ch); idx++; continue; }

                    if ((styles & NumberStyles.AllowThousands) != 0 && !seenPoint && anyDigits
                        && MatchSymbol(value, idx, info.NumberGroupSeparator, out int gs)
                        && gs > 0 && idx + gs < len
                        && IsAsciiDigit(UtfChar<TChar>.CastToUInt32(value[idx + gs]))) {
                        idx += gs;
                        continue;
                    }
                    break;
                }

                // decimal point
                if ((styles & NumberStyles.AllowDecimalPoint) != 0 && idx < len
                    && MatchSymbol(value, idx, info.NumberDecimalSeparator, out int ds) && ds > 0) {
                    idx += ds;
                    seenPoint = true;
                    while (idx < len) {
                        uint ch = UtfChar<TChar>.CastToUInt32(value[idx]);
                        if (ch >= '0' && ch <= '9') { ProcessDigit(ch); idx++; continue; }

                        if ((styles & NumberStyles.AllowThousands) != 0 && anyDigits
                            && MatchSymbol(value, idx, info.NumberGroupSeparator, out int gs2)
                            && gs2 > 0 && idx + gs2 < len
                            && IsAsciiDigit(UtfChar<TChar>.CastToUInt32(value[idx + gs2]))) {
                            idx += gs2;
                            continue;
                        }
                        break;
                    }
                }

                if (!anyDigits) return false;

                // ---- exponent ----
                long expAdjust = 0;
                if ((styles & NumberStyles.AllowExponent) != 0 && idx < len) {
                    uint ch = UtfChar<TChar>.CastToUInt32(value[idx]);
                    if (ch == 'e' || ch == 'E') {
                        int probe = idx + 1;
                        bool expNeg = false;
                        if (probe < len) {
                            uint es = UtfChar<TChar>.CastToUInt32(value[probe]);
                            if (es == '-') { expNeg = true; probe++; } else if (es == '+') probe++;
                        }
                        int expStart = probe;
                        long ev = 0;
                        while (probe < len) {
                            uint d = UtfChar<TChar>.CastToUInt32(value[probe]);
                            if (d < '0' || d > '9') break;
                            ev = ev * 10 + (d - '0');
                            if (ev > 10_000_000) ev = 10_000_000;
                            probe++;
                        }
                        if (probe > expStart) {
                            expAdjust = expNeg ? -ev : ev;
                            idx = probe;
                        }
                    }
                }

                // ---- trailing currency symbol ----
                if ((styles & NumberStyles.AllowCurrencySymbol) != 0
                    && MatchSymbol(value, idx, info.CurrencySymbol, out int c2)) {
                    idx += c2;
                }

                // ---- trailing sign ----
                if ((styles & NumberStyles.AllowTrailingSign) != 0) {
                    if (TryConsumeSign(value, ref idx, info, out bool neg2) && neg2)
                        isNegative = true;
                }

                // ---- closing paren ----
                if (hadParens) {
                    if (idx >= len || UtfChar<TChar>.CastToUInt32(value[idx]) != ')')
                        return false;
                    idx++;
                }

                // ---- trailing whitespace ----
                if ((styles & NumberStyles.AllowTrailingWhite) != 0) {
                    while (idx < len && IsWhite(UtfChar<TChar>.CastToUInt32(value[idx]))) idx++;
                }

                // ---- trailing content check ----
                // '\0' padding is always tolerated.
                while (idx < len && UtfChar<TChar>.CastToUInt32(value[idx]) == 0) idx++;

                bool allowTrailingInvalid = (styles & Number.AllowTrailingInvalidCharacters) != 0;
                if (idx < len && !allowTrailingInvalid) return false;

                // ---- convert ----
                result = BuildQuadruple(isNegative, sig, expAdjust - fracDigits);
                elementsConsumed = idx;
                return true;
            }

            // ==================================================================
            // Hexadecimal float parse — "0x<hex>[.<hex>]p<sign><dec>"
            // ==================================================================
            private static bool TryParseHexFloat<TChar>(ReadOnlySpan<TChar> value,
                                                        NumberStyles styles,
                                                        NumberFormatInfo info,
                                                        out Quadruple result,
                                                        out int elementsConsumed)
                where TChar : unmanaged {
                result = default;
                elementsConsumed = 0;

                int len = value.Length;
                if (len == 0) return false;
                int idx = 0;

                // leading whitespace
                if ((styles & NumberStyles.AllowLeadingWhite) != 0)
                    while (idx < len && IsWhite(UtfChar<TChar>.CastToUInt32(value[idx]))) idx++;

                // sign
                bool isNegative = false;
                if ((styles & NumberStyles.AllowLeadingSign) != 0) {
                    if (idx < len && TryConsumeSign(value, ref idx, info, out bool neg))
                        isNegative = neg;
                }

                // "0x" prefix
                if (idx + 1 >= len) return false;
                if (UtfChar<TChar>.CastToUInt32(value[idx]) != '0') return false;
                uint xCh = UtfChar<TChar>.CastToUInt32(value[idx + 1]) | 0x20;
                if (xCh != 'x') return false;
                idx += 2;

                // significand
                BigInteger sig = BigInteger.Zero;
                int integerHexDigits = 0;
                int fracHexDigits = 0;
                bool anyDigits = false;

                int intStart = idx;
                while (idx < len) {
                    int d = HexValue(UtfChar<TChar>.CastToUInt32(value[idx]));
                    if (d < 0) break;
                    sig = (sig << 4) | d;
                    integerHexDigits++;
                    idx++;
                }
                anyDigits = idx > intStart;

                bool seenPoint = false;
                if ((styles & NumberStyles.AllowDecimalPoint) != 0 && idx < len
                    && UtfChar<TChar>.CastToUInt32(value[idx]) == '.') {
                    seenPoint = true;
                    idx++;
                    int fracStart = idx;
                    while (idx < len) {
                        int d = HexValue(UtfChar<TChar>.CastToUInt32(value[idx]));
                        if (d < 0) break;
                        sig = (sig << 4) | d;
                        fracHexDigits++;
                        idx++;
                    }
                    anyDigits |= idx > fracStart;
                }

                if (!anyDigits) return false;

                // 'p' exponent (required)
                if (idx >= len) return false;
                uint pCh = UtfChar<TChar>.CastToUInt32(value[idx]) | 0x20;
                if (pCh != 'p') return false;
                idx++;

                bool expNeg = false;
                if (idx < len) {
                    uint es = UtfChar<TChar>.CastToUInt32(value[idx]);
                    if (es == '-') { expNeg = true; idx++; } else if (es == '+') idx++;
                }

                int expStart = idx;
                long binExp = 0;
                while (idx < len) {
                    uint d = UtfChar<TChar>.CastToUInt32(value[idx]);
                    if (d < '0' || d > '9') break;
                    binExp = binExp * 10 + (d - '0');
                    if (binExp > int.MaxValue) binExp = int.MaxValue;
                    idx++;
                }
                if (idx == expStart) return false;
                if (expNeg) binExp = -binExp;

                // trailing whitespace
                if ((styles & NumberStyles.AllowTrailingWhite) != 0)
                    while (idx < len && IsWhite(UtfChar<TChar>.CastToUInt32(value[idx]))) idx++;

                // trailing '\0'
                while (idx < len && UtfChar<TChar>.CastToUInt32(value[idx]) == 0) idx++;

                bool allowTrailingInvalid = (styles & Number.AllowTrailingInvalidCharacters) != 0;
                if (idx < len && !allowTrailingInvalid) return false;

                elementsConsumed = idx;

                if (sig.IsZero) {
                    result = isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                        : new Quadruple(0, 0);
                    return true;
                }

                // value = sig × 2^(binExp − 4·fracHexDigits)
                long exp2 = binExp - 4L * fracHexDigits;
                if (exp2 > int.MaxValue) exp2 = int.MaxValue;
                if (exp2 < int.MinValue) exp2 = int.MinValue;

                result = BuildQuadrupleFromBinary(isNegative, sig, (int)exp2);
                return true;
            }

            // ==================================================================
            // Infinity / NaN [+payload]
            // ==================================================================
            private static bool TryParseSpecialValue<TChar>(ReadOnlySpan<TChar> value,
                                                            NumberStyles styles,
                                                            NumberFormatInfo info,
                                                            out Quadruple result,
                                                            out int elementsConsumed)
                where TChar : unmanaged {
                result = default;
                elementsConsumed = 0;

                ReadOnlySpan<TChar> trimmed = TrimStartWhitespace(value);
                bool allowTrailingInvalid = (styles & Number.AllowTrailingInvalidCharacters) != 0;

                elementsConsumed = value.Length - trimmed.Length;

                // no sign
                if (TryMatchSymbolIgnoreCase(trimmed, info.PositiveInfinitySymbol, ref elementsConsumed)
                    && TailOk(trimmed, info.PositiveInfinitySymbol.Length, allowTrailingInvalid, ref elementsConsumed)) {
                    result = new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                    return true;
                }
                if (TryMatchSymbolIgnoreCase(trimmed, info.NegativeInfinitySymbol, ref elementsConsumed)
                    && TailOk(trimmed, info.NegativeInfinitySymbol.Length, allowTrailingInvalid, ref elementsConsumed)) {
                    result = new Quadruple(0, 0xFFFF_0000_0000_0000UL);
                    return true;
                }
                if (TryParseNaN(trimmed, info.NaNSymbol, isNegative: false, allowTrailingInvalid,
                                ref elementsConsumed, out result)) {
                    return true;
                }

                // leading "+"
                if (trimmed.Length > 0 && UtfChar<TChar>.CastToUInt32(trimmed[0]) == '+') {
                    ReadOnlySpan<TChar> after = trimmed.Slice(1);
                    elementsConsumed = value.Length - after.Length;

                    if (TryMatchSymbolIgnoreCase(after, info.PositiveInfinitySymbol, ref elementsConsumed)
                        && TailOk(after, info.PositiveInfinitySymbol.Length, allowTrailingInvalid, ref elementsConsumed)) {
                        result = new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                        return true;
                    }
                    if (TryParseNaN(after, info.NaNSymbol, isNegative: false, allowTrailingInvalid,
                                    ref elementsConsumed, out result)) {
                        return true;
                    }
                    result = default;
                    elementsConsumed = 0;
                    return false;
                }

                // leading "−"
                int negLen = MatchNegativeSignLength(trimmed, info);
                if (negLen > 0) {
                    ReadOnlySpan<TChar> after = trimmed.Slice(negLen);
                    elementsConsumed = value.Length - after.Length;

                    if (TryMatchSymbolIgnoreCase(after, info.NegativeInfinitySymbol, ref elementsConsumed)
                        && TailOk(after, info.NegativeInfinitySymbol.Length, allowTrailingInvalid, ref elementsConsumed)) {
                        result = new Quadruple(0, 0xFFFF_0000_0000_0000UL);
                        return true;
                    }
                    if (TryParseNaN(after, info.NaNSymbol, isNegative: true, allowTrailingInvalid,
                                    ref elementsConsumed, out result)) {
                        return true;
                    }
                }

                result = default;
                elementsConsumed = 0;
                return false;
            }

            private static bool TryParseNaN<TChar>(ReadOnlySpan<TChar> candidate,
                string nanSym,
                bool isNegative,
                bool allowTrailingInvalid,
                ref int elementsConsumed,
                out Quadruple result)
                where TChar : unmanaged {
                result = default;

                if (nanSym.Length == 0) return false;

                // ------------------------------------------------------------------
                // Optional "s" / "q" prefix, case-insensitive.
                //   s → signaling (quiet = false), q → quiet (quiet = true).
                //
                // Recognized only when immediately followed by the locale's NaN symbol;
                // otherwise the leading character is not part of the token and we fall
                // through to the bare-symbol match. Safe for locales whose NaN symbol
                // itself begins with 's' or 'q'.
                // ------------------------------------------------------------------
                int nanStart = 0;
                bool quiet = true;   // default

                if (candidate.Length >= nanSym.Length + 1) {
                    uint c0 = UtfChar<TChar>.CastToUInt32(candidate[0]) | 0x20;
                    if ((c0 == 's' || c0 == 'q') && MatchSymbolIgnoreCaseAt(candidate, 1, nanSym)) {
                        quiet = (c0 == 'q');
                        nanStart = 1;
                    }
                }

                if (nanStart == 0) {
                    if (candidate.Length < nanSym.Length) return false;
                    if (!MatchSymbolIgnoreCaseAt(candidate, 0, nanSym)) return false;
                }

                ReadOnlySpan<TChar> after = candidate.Slice(nanStart + nanSym.Length);

                BigInteger payload = BigInteger.Zero;   // default
                bool payloadOverflow = false;

                if (after.Length > 0 && UtfChar<TChar>.CastToUInt32(after[0]) == '(') {
                    after = after.Slice(1);

                    // Optional explicit Q / S marker, case-insensitive.
                    // Overrides the prefix: "sNaN(Q1)" → quiet, "qNaN(S1)" → signaling.
                    bool sawMarker = false;
                    if (after.Length > 0) {
                        uint c = UtfChar<TChar>.CastToUInt32(after[0]) | 0x20;
                        if (c == 'q') { quiet = true; after = after.Slice(1); sawMarker = true; } else if (c == 's') { quiet = false; after = after.Slice(1); sawMarker = true; }
                    }

                    // Digit run. Fold modulo 2^110; the first time the accumulator
                    // would reach 2^110 set the overflow flag and mask back down.
                    int pos = 0;
                    while (pos < after.Length) {
                        uint ch = UtfChar<TChar>.CastToUInt32(after[pos]);
                        if (ch < '0' || ch > '9') break;

                        payload = payload * 10 + (ch - '0');
                        if (payload >= (BigInteger.One << NaNPayloadDataBits)) {
                            payloadOverflow = true;
                            payload &= (BigInteger.One << NaNPayloadDataBits) - 1;
                        }
                        pos++;
                    }

                    // Empty digit run is legal only when an explicit Q/S marker was
                    // consumed: "NaN(S)" / "NaN(Q)" collapse to a zero-payload NaN of
                    // the chosen class. "NaN()" (no marker, no digits) stays malformed.
                    if (pos == 0 && !sawMarker) return false;
                    after = after.Slice(pos);

                    if (after.Length == 0 || UtfChar<TChar>.CastToUInt32(after[0]) != ')') return false;
                    after = after.Slice(1);
                }

                ReadOnlySpan<TChar> tail = TrimStartWhitespace(after);
                if (!tail.IsEmpty && !allowTrailingInvalid) return false;

                elementsConsumed += candidate.Length - tail.Length;

                // Fraction layout: bits 109..0 data, bit 110 overflow, bit 111 quiet.
                BigInteger mantissa = payload & ((BigInteger.One << NaNPayloadDataBits) - 1);

                if (quiet)
                    mantissa |= BigInteger.One << NaNQuietBit;

                // Force the overflow indicator when:
                //   - the parsed integer had bits at position >= 110 (jam), or
                //   - the raw fraction would be all-zero after the quiet-bit decision
                //     (sNaN, sNaN(0), sNaN(S), NaN(S), NaN(S0), …), which would
                //     otherwise decode as ±Infinity.
                if (mantissa.IsZero || payloadOverflow)
                    mantissa |= BigInteger.One << NaNPayloadOverflowBit;

                ulong mantLo = (ulong)(mantissa & ulong.MaxValue);
                ulong mantHi = (ulong)((mantissa >> 64) & 0x0000_FFFF_FFFF_FFFFUL);
                ulong signBit = isNegative ? (1UL << 63) : 0UL;
                ulong hi = signBit | ((ulong)InfinityExponent << 48) | mantHi;

                result = new Quadruple(mantLo, hi);
                return true;
            }

            // ==================================================================
            // Build a Quadruple from an exact decimal  value = isNeg ? −sig·10^exp10 : sig·10^exp10
            // ==================================================================
            private static Quadruple BuildQuadruple(bool isNegative, BigInteger sig, long exp10) {
                if (sig.IsZero)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                // Estimate of the decimal position of the MSB.
                int sigDigits = (int)((sig.GetBitLength() * 30103L) / 100_000L) + 1;
                long msbDec = (long)sigDigits - 1 + exp10;

                if (msbDec > MaxDecMSB)
                    return isNegative ? new Quadruple(0, 0xFFFF_0000_0000_0000UL)
                                      : new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                if (msbDec < MinDecMSB)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                BigInteger N, D;
                if (exp10 >= 0) { N = sig * BigInteger.Pow(10, (int)exp10); D = BigInteger.One; } else { N = sig; D = BigInteger.Pow(10, (int)-exp10); }

                return BuildFromRatio(isNegative, N, D);
            }

            /// <summary>Build a Quadruple from  sig × 2^exp2  (hex path).</summary>
            private static Quadruple BuildQuadrupleFromBinary(bool isNegative, BigInteger sig, int exp2) {
                if (sig.IsZero)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                int B = (int)(sig.GetBitLength() - 1) + exp2;
                if (B > MaxNormalBinExp)
                    return isNegative ? new Quadruple(0, 0xFFFF_0000_0000_0000UL)
                                      : new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                if (B < MinSubnormalExp2 - 1)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                BigInteger N, D;
                if (exp2 >= 0) { N = sig << exp2; D = BigInteger.One; } else { N = sig; D = BigInteger.One << -exp2; }

                return BuildFromRatio(isNegative, N, D);
            }

            // ==================================================================
            // Round  N/D  to a Quadruple (nearest, ties to even)
            // ==================================================================
            private static Quadruple BuildFromRatio(bool isNegative, BigInteger N, BigInteger D) {
                Debug.Assert(N.Sign > 0);
                Debug.Assert(D.Sign > 0);

                int B = ComputeBinaryExponent(N, D);
                if (B > MaxNormalBinExp)
                    return isNegative ? new Quadruple(0, 0xFFFF_0000_0000_0000UL)
                                      : new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                if (B < MinSubnormalExp2 - 1)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                bool isSubnormal = B < MinNormalBinExp;
                int e = isSubnormal ? MinSubnormalExp2 : B - MantissaBits;

                BigInteger m = RoundHalfToEven(N, D, e, out bool carry);

                if (m.IsZero)
                    return isNegative ? new Quadruple(0, 0x8000_0000_0000_0000UL)
                                      : new Quadruple(0, 0);

                if (carry) {
                    m >>= 1;
                    e++;
                }

                return Encode(isNegative, m, e, isSubnormal);
            }

            /// <summary>Exact floor(log2(N / D)) for N &gt; 0, D &gt; 0.</summary>
            private static int ComputeBinaryExponent(BigInteger N, BigInteger D) {
                long bitN = N.GetBitLength();
                long bitD = D.GetBitLength();
                long diff = bitN - bitD;

                bool geq;
                if (diff >= 0) geq = N >= (D << (int)diff);
                else geq = (N << (int)(-diff)) >= D;

                return geq ? (int)diff : (int)diff - 1;
            }

            /// <summary>
            /// Returns m = round_half_to_even(N · 2^(−e) / D).
            /// Sets <paramref name="carry"/> if m == 2^113 (i.e. the significand
            /// overflowed and the exponent must be bumped).
            /// </summary>
            private static BigInteger RoundHalfToEven(BigInteger N, BigInteger D, int e, out bool carry) {
                BigInteger num, den;
                if (e <= 0) { num = N << -e; den = D; } else { num = N; den = D << e; }

                BigInteger m = BigInteger.DivRem(num, den, out BigInteger r);
                BigInteger twice = r << 1;
                int cmp = twice.CompareTo(den);

                bool roundUp = cmp > 0 || (cmp == 0 && !m.IsEven);

                if (roundUp) {
                    m += BigInteger.One;
                    carry = m == (BigInteger.One << SignificandBits);
                } else {
                    carry = false;
                }
                return m;
            }

            private static Quadruple Encode(bool isNegative, BigInteger m, int e, bool isSubnormal) {
                ulong signBit = isNegative ? (1UL << 63) : 0UL;

                if (isSubnormal) {
                    ulong lo = (ulong)(m & ulong.MaxValue);
                    ulong hiMant = (ulong)((m >> 64) & 0x0000_FFFF_FFFF_FFFFUL);
                    ulong hi = signBit | hiMant;
                    return new Quadruple(lo, hi);
                }

                int biasedExp = e + MantissaBits + ExponentBias;
                if (biasedExp >= InfinityExponent)
                    return isNegative ? new Quadruple(0, 0xFFFF_0000_0000_0000UL)
                                      : new Quadruple(0, 0x7FFF_0000_0000_0000UL);
                Debug.Assert(biasedExp >= 1 && biasedExp < InfinityExponent);

                BigInteger mantissa = m - (BigInteger.One << MantissaBits);
                ulong lo2 = (ulong)(mantissa & ulong.MaxValue);
                ulong hiMant2 = (ulong)((mantissa >> 64) & 0x0000_FFFF_FFFF_FFFFUL);
                ulong hi2 = signBit | ((ulong)biasedExp << 48) | hiMant2;
                return new Quadruple(lo2, hi2);
            }

            // ==================================================================
            // Low-level helpers
            // ==================================================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsAsciiDigit(uint ch) => ch >= '0' && ch <= '9';

            private static bool IsWhite(uint ch) {
                if (ch == ' ' || (ch >= 0x0009 && ch <= 0x000D)) return true;
                if (ch == 0x0085 || ch == 0x00A0) return true;
                if (ch == 0x1680) return true;
                if (ch >= 0x2000 && ch <= 0x200A) return true;
                if (ch == 0x2028 || ch == 0x2029 || ch == 0x202F || ch == 0x205F || ch == 0x3000) return true;
                return false;
            }

            private static int HexValue(uint ch) {
                if (ch >= '0' && ch <= '9') return (int)(ch - '0');
                uint c = ch | 0x20;
                if (c >= 'a' && c <= 'f') return (int)(c - 'a' + 10);
                return -1;
            }

            private static ReadOnlySpan<TChar> TrimStartWhitespace<TChar>(ReadOnlySpan<TChar> span)
                where TChar : unmanaged {
                int i = 0;
                while (i < span.Length && IsWhite(UtfChar<TChar>.CastToUInt32(span[i]))) i++;
                return span.Slice(i);
            }

            private static bool TryConsumeSign<TChar>(ReadOnlySpan<TChar> value, ref int idx,
                                                      NumberFormatInfo info, out bool isNegative)
                where TChar : unmanaged {
                isNegative = false;
                int len = value.Length;
                if (idx >= len) return false;

                if (MatchSymbol(value, idx, info.NegativeSign, out int n)) { idx += n; isNegative = true; return true; }
                if (MatchSymbol(value, idx, info.PositiveSign, out int p)) { idx += p; isNegative = false; return true; }

                uint c = UtfChar<TChar>.CastToUInt32(value[idx]);
                if (c == '-') { idx++; isNegative = true; return true; }
                if (c == '+') { idx++; isNegative = false; return true; }
                return false;
            }

            private static int MatchNegativeSignLength<TChar>(ReadOnlySpan<TChar> value, NumberFormatInfo info)
                where TChar : unmanaged {
                if (value.IsEmpty) return 0;
                if (MatchSymbol(value, 0, info.NegativeSign, out int n) && n > 0) return n;
                if (UtfChar<TChar>.CastToUInt32(value[0]) == '-') return 1;
                return 0;
            }

            /// <summary>Match a NumberFormatInfo symbol against a TChar span.</summary>
            private static bool MatchSymbol<TChar>(ReadOnlySpan<TChar> input, int offset,
                                                   string symbol, out int consumed)
                where TChar : unmanaged {
                consumed = 0;
                if (symbol.Length == 0 || offset < 0 || offset >= input.Length) return false;

                if (typeof(TChar) == typeof(char)) {
                    if (offset + symbol.Length > input.Length) return false;
                    var casted = MemoryMarshal.Cast<TChar, char>(input.Slice(offset));
                    for (int i = 0; i < symbol.Length; i++)
                        if (casted[i] != symbol[i]) return false;
                    consumed = symbol.Length;
                    return true;
                }
                if (typeof(TChar) == typeof(byte)) {
                    Span<byte> buf = stackalloc byte[64];
                    int n;
                    try { n = Encoding.UTF8.GetBytes(symbol, buf); } catch (ArgumentException) { return false; }
                    if (offset + n > input.Length) return false;
                    var casted = MemoryMarshal.Cast<TChar, byte>(input.Slice(offset));
                    for (int i = 0; i < n; i++)
                        if (casted[i] != buf[i]) return false;
                    consumed = n;
                    return true;
                }
                if (typeof(TChar) == typeof(Rune)) {
                    var casted = MemoryMarshal.Cast<TChar, Rune>(input.Slice(offset));
                    int i = 0;
                    foreach (var r in symbol.EnumerateRunes()) {
                        if (i >= casted.Length || casted[i] != r) return false;
                        i++;
                    }
                    consumed = i;
                    return true;
                }
                // Unknown TChar: fall back to code-unit comparison.
                if (offset + symbol.Length > input.Length) return false;
                for (int i = 0; i < symbol.Length; i++)
                    if (UtfChar<TChar>.CastToUInt32(input[offset + i]) != symbol[i]) return false;
                consumed = symbol.Length;
                return true;
            }

            private static bool MatchSymbolIgnoreCaseAt<TChar>(ReadOnlySpan<TChar> input, int offset, string symbol)
                where TChar : unmanaged {
                if (symbol.Length == 0 || offset + symbol.Length > input.Length) return false;
                for (int i = 0; i < symbol.Length; i++) {
                    uint a = UtfChar<TChar>.CastToUInt32(input[offset + i]);
                    uint b = symbol[i];
                    if (a >= 'A' && a <= 'Z') a |= 0x20;
                    if (b >= 'A' && b <= 'Z') b |= 0x20;
                    if (a != b) return false;
                }
                return true;
            }

            private static bool TryMatchSymbolIgnoreCase<TChar>(ReadOnlySpan<TChar> input, string symbol, ref int consumed)
                where TChar : unmanaged {
                if (!MatchSymbolIgnoreCaseAt(input, 0, symbol)) return false;
                consumed += symbol.Length;
                return true;
            }

            private static bool TailOk<TChar>(ReadOnlySpan<TChar> input, int fromIndex,
                                              bool allowTrailingInvalid, ref int consumed)
                where TChar : unmanaged {
                ReadOnlySpan<TChar> tail = TrimStartWhitespace(input.Slice(fromIndex));
                if (!tail.IsEmpty && !allowTrailingInvalid) return false;
                consumed = consumed - (input.Length - fromIndex) + (input.Length - fromIndex - tail.Length);
                // simpler: consumed was already advanced past the symbol; add whitespace we just consumed
                consumed += input.Length - fromIndex - tail.Length;
                return true;
            }

            [DoesNotReturn]
            private static void ThrowFormatException<TChar>(ReadOnlySpan<TChar> value)
                where TChar : unmanaged
                => throw new FormatException("The input string was not in a correct format.");
        }
    }
}
namespace UltimateOrb.Internal.System.Globalization {
    using static global::System.Globalization.NumberStyles;

    static partial class Extensions {

        extension(NumberStyles) {

#if !NET8_0_OR_GREATER
            public static NumberStyles AllowBinarySpecifier { get => (NumberStyles)1024; }
#endif

#if !NET11_0_OR_GREATER
            public static NumberStyles HexFloat { get => AllowLeadingWhite | AllowTrailingWhite | AllowLeadingSign | AllowDecimalPoint | AllowExponent | AllowHexSpecifier; }
#endif
        }
    }
}

namespace UltimateOrb.Internal.System {
    using Unsafe = UltimateOrb.Runtime.CompilerServices.Unsafe;

    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    internal static partial class UtfChar<TSelf>
    where TSelf : unmanaged {
        public static bool IsSupported { get; } =
            typeof(TSelf) == typeof(byte) ? true :
            typeof(TSelf) == typeof(char) ? true :
            typeof(TSelf) == typeof(Rune) ? true :
            typeof(TSelf) == typeof(sbyte) ? true :
            typeof(TSelf) == typeof(UInt16) ? true :
            typeof(TSelf) == typeof(Int16) ? true :
            typeof(TSelf) == typeof(UInt32) ? true :
            typeof(TSelf) == typeof(Int32) ? true :
            false;

        /// <summary>Gets whether this type represents UTF-8 code units.</summary>
        public static bool IsUtf8 { get; } =
            typeof(TSelf) == typeof(byte) ? true :
            typeof(TSelf) == typeof(sbyte) ? true :
            false;

        /// <summary>Gets whether this type represents UTF-16 code units.</summary>
        public static bool IsUtf16 { get; } =
            typeof(TSelf) == typeof(char) ? true :
            typeof(TSelf) == typeof(UInt16) ? true :
            typeof(TSelf) == typeof(Int16) ? true :
            false;

        /// <summary>Gets whether this type represents UTF-32 code units.</summary>
        public static bool IsUtf32 { get; } =
            typeof(TSelf) == typeof(Rune) ? true :
            typeof(TSelf) == typeof(UInt32) ? true :
            typeof(TSelf) == typeof(Int32) ? true :
            false;

        /// <summary>Casts the specified value to this type.</summary>
        public static TSelf CastFrom(byte value) {
            if (typeof(TSelf) == typeof(byte)) return (TSelf)(object)value;
            if (typeof(TSelf) == typeof(sbyte)) return (TSelf)(object)unchecked((sbyte)value);
            if (typeof(TSelf) == typeof(char)) return (TSelf)(object)(char)value;
            if (typeof(TSelf) == typeof(UInt16)) return (TSelf)(object)(UInt16)value;
            if (typeof(TSelf) == typeof(Int16)) return (TSelf)(object)(Int16)value;
            if (typeof(TSelf) == typeof(UInt32)) return (TSelf)(object)(UInt32)value;
            if (typeof(TSelf) == typeof(Int32)) return (TSelf)(object)(Int32)value;
            if (typeof(TSelf) == typeof(Rune)) return (TSelf)(object)Unsafe.BitCast<UInt32, Rune>(unchecked((UInt32)value));
            throw new NotSupportedException();
        }

        /// <summary>Casts the specified value to this type.</summary>
        public static TSelf CastFrom(char value) {
            if (typeof(TSelf) == typeof(char)) return (TSelf)(object)value;
            if (typeof(TSelf) == typeof(byte)) return (TSelf)(object)unchecked((byte)value);
            if (typeof(TSelf) == typeof(sbyte)) return (TSelf)(object)unchecked((sbyte)value);
            if (typeof(TSelf) == typeof(UInt16)) return (TSelf)(object)(UInt16)value;
            if (typeof(TSelf) == typeof(Int16)) return (TSelf)(object)unchecked((Int16)value);
            if (typeof(TSelf) == typeof(UInt32)) return (TSelf)(object)(UInt32)value;
            if (typeof(TSelf) == typeof(Int32)) return (TSelf)(object)(Int32)value;
            if (typeof(TSelf) == typeof(Rune)) return (TSelf)(object)Unsafe.BitCast<UInt32, Rune>(unchecked((UInt32)value));
            throw new NotSupportedException();
        }

        /// <summary>Casts the specified value to this type.</summary>
        public static TSelf CastFrom(int value) {
            if (typeof(TSelf) == typeof(Int32)) return (TSelf)(object)value;
            if (typeof(TSelf) == typeof(byte)) return (TSelf)(object)unchecked((byte)value);
            if (typeof(TSelf) == typeof(sbyte)) return (TSelf)(object)unchecked((sbyte)value);
            if (typeof(TSelf) == typeof(char)) return (TSelf)(object)unchecked((char)value);
            if (typeof(TSelf) == typeof(UInt16)) return (TSelf)(object)unchecked((UInt16)value);
            if (typeof(TSelf) == typeof(Int16)) return (TSelf)(object)unchecked((Int16)value);
            if (typeof(TSelf) == typeof(UInt32)) return (TSelf)(object)unchecked((UInt32)value);
            if (typeof(TSelf) == typeof(Rune)) return (TSelf)(object)Unsafe.BitCast<Int32, Rune>(unchecked((Int32)value));
            throw new NotSupportedException();
        }

        /// <summary>Casts the specified value to this type.</summary>
        public static TSelf CastFrom(uint value) {
            if (typeof(TSelf) == typeof(UInt32)) return (TSelf)(object)value;
            if (typeof(TSelf) == typeof(byte)) return (TSelf)(object)unchecked((byte)value);
            if (typeof(TSelf) == typeof(sbyte)) return (TSelf)(object)unchecked((sbyte)value);
            if (typeof(TSelf) == typeof(char)) return (TSelf)(object)unchecked((char)value);
            if (typeof(TSelf) == typeof(UInt16)) return (TSelf)(object)unchecked((UInt16)value);
            if (typeof(TSelf) == typeof(Int16)) return (TSelf)(object)unchecked((Int16)value);
            if (typeof(TSelf) == typeof(Int32)) return (TSelf)(object)unchecked((Int32)value);
            if (typeof(TSelf) == typeof(Rune)) return (TSelf)(object)Unsafe.BitCast<UInt32, Rune>(unchecked((UInt32)value));
            throw new NotSupportedException();
        }

        /// <summary>Casts the specified value to this type.</summary>
        public static TSelf CastFrom(ulong value) {
            if (typeof(TSelf) == typeof(byte)) return (TSelf)(object)unchecked((byte)value);
            if (typeof(TSelf) == typeof(sbyte)) return (TSelf)(object)unchecked((sbyte)value);
            if (typeof(TSelf) == typeof(char)) return (TSelf)(object)unchecked((char)value);
            if (typeof(TSelf) == typeof(UInt16)) return (TSelf)(object)unchecked((UInt16)value);
            if (typeof(TSelf) == typeof(Int16)) return (TSelf)(object)unchecked((Int16)value);
            if (typeof(TSelf) == typeof(UInt32)) return (TSelf)(object)unchecked((UInt32)value);
            if (typeof(TSelf) == typeof(Int32)) return (TSelf)(object)unchecked((Int32)value);
            if (typeof(TSelf) == typeof(Rune)) return (TSelf)(object)Unsafe.BitCast<UInt32, Rune>(unchecked((UInt32)value));
            throw new NotSupportedException();
        }

        /// <summary>Casts a value of this type to an UInt32.</summary>
        public static UInt32 CastToUInt32(TSelf value) {
            if (typeof(TSelf) == typeof(UInt32)) return (UInt32)(object)value;
            if (typeof(TSelf) == typeof(Int32)) return unchecked((UInt32)(Int32)(object)value);
            if (typeof(TSelf) == typeof(byte)) return (byte)(object)value;
            if (typeof(TSelf) == typeof(sbyte)) return unchecked((byte)(sbyte)(object)value);
            if (typeof(TSelf) == typeof(char)) return (char)(object)value;
            if (typeof(TSelf) == typeof(UInt16)) return (UInt16)(object)value;
            if (typeof(TSelf) == typeof(Int16)) return unchecked((UInt16)(Int16)(object)value);
            if (typeof(TSelf) == typeof(Rune)) return unchecked((UInt32)((Rune)(object)value).Value);
            throw new NotSupportedException();
        }
    }

    internal static partial class BinaryFloatParseAndFormatInfo<TSelf>
        where TSelf : unmanaged, IBinaryFloatingPointIeee754<TSelf>, IMinMaxValue<TSelf> {

        public static bool IsSupported { get; } =
            typeof(TSelf) == typeof(double) ? true :
            typeof(TSelf) == typeof(float) ? true :
            typeof(TSelf) == typeof(Half) ? true :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? true :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? true :
#pragma warning restore UoWIP
            false;

        /// <summary>
        /// Ceiling(Log10(5^(Abs(MinBinaryExponent) - 1))) + NormalMantissaBits + 1 + 1
        /// </summary>
        public static int NumberBufferLength { get; } =
            typeof(TSelf) == typeof(double) ? 769 :
            typeof(TSelf) == typeof(float) ? 114 :
            typeof(TSelf) == typeof(Half) ? 21 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 114 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 11572 :
#pragma warning restore UoWIP
            default;

        public static ulong ZeroBits { get; } = 0;

        public static TBits GetZeroBits<TBits>() where TBits : unmanaged, IBinaryInteger<TBits> {
            return TBits.Zero;
        }

        public static ulong InfinityBits { get; } =
            typeof(TSelf) == typeof(double) ? 0x7FF00000_00000000UL :
            typeof(TSelf) == typeof(float) ? 0x7F800000UL :
            typeof(TSelf) == typeof(Half) ? 0x7C00UL :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 0x7F80UL :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 0x7FFF0000_00000000UL :
#pragma warning restore UoWIP
            default;

        public static TBits GetInfinityBits<TBits>() where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                if (typeof(TSelf) == typeof(double) ||
                    typeof(TSelf) == typeof(float) ||
                    typeof(TSelf) == typeof(Half) ||
#if NET11_0_OR_GREATER
                    typeof(TSelf) == typeof(BFloat16) ||
#endif
#pragma warning disable UoWIP
                    typeof(TSelf) == typeof(Quadruple)) {
#pragma warning restore UoWIP
                    return ((TBits.One << ExponentBias) - TBits.One) << DenormalMantissaBits;
                }
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// (1UL &lt;&lt; SignificandLength) - 1
        /// </summary>
        public static ulong NormalMantissaMask { get; } =
            typeof(TSelf) == typeof(double) ? 0x001FFFFF_FFFFFFFFUL :
            typeof(TSelf) == typeof(float) ? 0x00FFFFFFUL :
            typeof(TSelf) == typeof(Half) ? 0x07FFUL :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 0x00FFUL :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 0x0001FFFF_FFFFFFFFUL :
#pragma warning restore UoWIP
            default;

        public static TBits GetNormalMantissaMask<TBits>() where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                if (typeof(TSelf) == typeof(double) ||
                    typeof(TSelf) == typeof(float) ||
                    typeof(TSelf) == typeof(Half) ||
#if NET11_0_OR_GREATER
                    typeof(TSelf) == typeof(BFloat16) ||
#endif
#pragma warning disable UoWIP
                    typeof(TSelf) == typeof(Quadruple))
#pragma warning restore UoWIP
                {
                    return (TBits.One << NormalMantissaBits) - TBits.One;
                }
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// TrailingSignificandMask == (1UL &lt;&lt; TrailingSignificandLength) - 1
        /// </summary>
        public static ulong DenormalMantissaMask { get; } =
            typeof(TSelf) == typeof(double) ? 0x000FFFFF_FFFFFFFFUL :
            typeof(TSelf) == typeof(float) ? 0x007FFFFFUL :
            typeof(TSelf) == typeof(Half) ? 0x03FFUL :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 0x007FUL :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 0x0000FFFF_FFFFFFFFUL :
#pragma warning restore UoWIP
            default;

        public static TBits GetDenormalMantissaMask<TBits>() where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                if (typeof(TSelf) == typeof(double) ||
                    typeof(TSelf) == typeof(float) ||
                    typeof(TSelf) == typeof(Half) ||
#if NET11_0_OR_GREATER
                    typeof(TSelf) == typeof(BFloat16) ||
#endif
#pragma warning disable UoWIP
                    typeof(TSelf) == typeof(Quadruple))
#pragma warning restore UoWIP
                {
                    return (TBits.One << DenormalMantissaBits) - TBits.One;
                }
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// 1 - MaxBinaryExponent
        /// </summary>
        public static int MinBinaryExponent { get; } =
            typeof(TSelf) == typeof(double) ? -1022 :
            typeof(TSelf) == typeof(float) ? -126 :
            typeof(TSelf) == typeof(Half) ? -14 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? -126 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? -16382 :
#pragma warning restore UoWIP
            default;

        public static int MaxBinaryExponent { get; } =
            typeof(TSelf) == typeof(double) ? 1023 :
            typeof(TSelf) == typeof(float) ? 127 :
            typeof(TSelf) == typeof(Half) ? 15 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 127 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 16383 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Floor(Log10(Epsilon)) where Epsilon is the smallest positive
        /// subnormal, i.e. 2^(MinBinaryExponent - TrailingSignificandLength).
        /// </summary>
        public static int MinDecimalExponent { get; } =
            typeof(TSelf) == typeof(double) ? -324 :
            typeof(TSelf) == typeof(float) ? -45 :
            typeof(TSelf) == typeof(Half) ? -8 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? -41 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? -4966 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Ceiling(Log10(MaxValue))
        /// </summary>
        public static int MaxDecimalExponent { get; } =
            typeof(TSelf) == typeof(double) ? 309 :
            typeof(TSelf) == typeof(float) ? 39 :
            typeof(TSelf) == typeof(Half) ? 5 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 39 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 4933 :
#pragma warning restore UoWIP
            default;

        public static int ExponentBias { get; } =
            typeof(TSelf) == typeof(double) ? 1023 :
            typeof(TSelf) == typeof(float) ? 127 :
            typeof(TSelf) == typeof(Half) ? 15 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 127 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 16383 :
#pragma warning restore UoWIP
            default;

        public static ushort ExponentBits { get; } =
            typeof(TSelf) == typeof(double) ? (ushort)11 :
            typeof(TSelf) == typeof(float) ? (ushort)8 :
            typeof(TSelf) == typeof(Half) ? (ushort)5 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? (ushort)8 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? (ushort)15 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// (MaxBinaryExponent + 2 * SignificandLength) / 3
        /// </summary>
        public static int OverflowDecimalExponent { get; } =
            typeof(TSelf) == typeof(double) ? 376 :   // (1023 + 2*53) / 3
            typeof(TSelf) == typeof(float) ? 58 :     // ( 127 + 2*24) / 3
            typeof(TSelf) == typeof(Half) ? 12 :      // (  15 + 2*11) / 3
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 47 :  // ( 127 + 2* 8) / 3
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 5536 : // (16383 + 2*113) / 3
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// (1 &lt;&lt; ExponentBits) - 1
        /// </summary>
        public static int InfinityExponent { get; } =
            typeof(TSelf) == typeof(double) ? 0x7FF :
            typeof(TSelf) == typeof(float) ? 0xFF :
            typeof(TSelf) == typeof(Half) ? 0x1F :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 0xFF :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 0x7FFF :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// SignificandLength (implicit leading bit included)
        /// </summary>
        public static ushort NormalMantissaBits { get; } =
            typeof(TSelf) == typeof(double) ? (ushort)53 :
            typeof(TSelf) == typeof(float) ? (ushort)24 :
            typeof(TSelf) == typeof(Half) ? (ushort)11 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? (ushort)8 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? (ushort)113 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// TrailingSignificandLength (explicit mantissa bits)
        /// </summary>
        public static ushort DenormalMantissaBits { get; } =
            typeof(TSelf) == typeof(double) ? (ushort)52 :
            typeof(TSelf) == typeof(float) ? (ushort)23 :
            typeof(TSelf) == typeof(Half) ? (ushort)10 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? (ushort)7 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? (ushort)112 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Ceiling(Log10(2^(MinBinaryExponent - 1 - TrailingSignificandLength - 64)))
        /// </summary>
        public static int MinFastFloatDecimalExponent { get; } =
            typeof(TSelf) == typeof(double) ? -342 :
            typeof(TSelf) == typeof(float) ? -64 :
            typeof(TSelf) == typeof(Half) ? -26 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? -59 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? -4984 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// MaxDecimalExponent - 1
        /// </summary>
        public static int MaxFastFloatDecimalExponent { get; } =
            typeof(TSelf) == typeof(double) ? 308 :
            typeof(TSelf) == typeof(float) ? 38 :
            typeof(TSelf) == typeof(Half) ? 4 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 38 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 4932 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// -Floor(Log5(2^(64 - SignificandLength)))
        /// </summary>
        public static int MinExponentRoundToEven { get; } =
            typeof(TSelf) == typeof(double) ? -4 :
            typeof(TSelf) == typeof(float) ? -17 :
            typeof(TSelf) == typeof(Half) ? -22 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? -24 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 22 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Floor(Log5(2^(SignificandLength + 1)))
        /// </summary>
        public static int MaxExponentRoundToEven { get; } =
            typeof(TSelf) == typeof(double) ? 23 :
            typeof(TSelf) == typeof(float) ? 10 :
            typeof(TSelf) == typeof(Half) ? 5 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 3 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 49 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Max n such that 10^n is exactly representable.
        /// </summary>
        public static int MaxExponentFastPath { get; } =
            typeof(TSelf) == typeof(double) ? 22 :
            typeof(TSelf) == typeof(float) ? 10 :
            typeof(TSelf) == typeof(Half) ? 4 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 3 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 48 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// 2UL &lt;&lt; TrailingSignificandLength
        /// </summary>
        public static ulong MaxMantissaFastPath { get; } =
            typeof(TSelf) == typeof(double) ? 0x00200000_00000000UL : // 2 << 52
            typeof(TSelf) == typeof(float) ? 0x01000000UL :            // 2 << 23
            typeof(TSelf) == typeof(Half) ? 0x0800UL :                 // 2 << 10
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 0x0100UL :             // 2 <<  7
#endif
#pragma warning disable UoWIP
            // 2 << 112 cannot fit in ulong; disable fast path.
            typeof(TSelf) == typeof(Quadruple) ? 0UL :
#pragma warning restore UoWIP
            default;

        public static TBits GetMaxMantissaFastPath<TBits>() where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                if (typeof(TSelf) == typeof(double) ||
                    typeof(TSelf) == typeof(float) ||
                    typeof(TSelf) == typeof(Half) ||
#if NET11_0_OR_GREATER
                    typeof(TSelf) == typeof(BFloat16) ||
#endif
#pragma warning disable UoWIP
                    typeof(TSelf) == typeof(Quadruple))
#pragma warning restore UoWIP
                {
                    return TBits.One << (DenormalMantissaBits + 1);
                }
            }
            throw new NotSupportedException();
        }

        /// <summary>
        /// Ceiling(Log10(2^SignificandLength)) + 1
        /// </summary>
        public static int MaxRoundTripDigits { get; } =
            typeof(TSelf) == typeof(double) ? 17 :
            typeof(TSelf) == typeof(float) ? 9 :
            typeof(TSelf) == typeof(Half) ? 5 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 4 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 36 :
#pragma warning restore UoWIP
            default;

        /// <summary>
        /// Legacy pre-.NET Core 3.0 custom-format precision ceiling.
        /// </summary>
        public static int MaxPrecisionCustomFormat { get; } =
            typeof(TSelf) == typeof(double) ? 15 :
            typeof(TSelf) == typeof(float) ? 7 :
            typeof(TSelf) == typeof(Half) ? 5 :
#if NET11_0_OR_GREATER
            typeof(TSelf) == typeof(BFloat16) ? 4 :
#endif
#pragma warning disable UoWIP
            typeof(TSelf) == typeof(Quadruple) ? 36 :
#pragma warning restore UoWIP
            default;

        // ------------------------------------------------------------------------
        // Methods
        // ------------------------------------------------------------------------

        public static TSelf BitsToFloat(ulong bits) {
            if (typeof(TSelf) == typeof(double)) {
                double v = BitConverter.UInt64BitsToDouble(bits);
                return Unsafe.As<double, TSelf>(ref v);
            }
            if (typeof(TSelf) == typeof(float)) {
                float v = BitConverter.UInt32BitsToSingle((uint)bits);
                return Unsafe.As<float, TSelf>(ref v);
            }
            if (typeof(TSelf) == typeof(Half)) {
                Half v = BitConverter.UInt16BitsToHalf((ushort)bits);
                return Unsafe.As<Half, TSelf>(ref v);
            }
#if NET11_0_OR_GREATER
            if (typeof(TSelf) == typeof(BFloat16)) {
                ushort v = (ushort)bits;
                return Unsafe.As<ushort, TSelf>(ref v);
            }
#endif
#pragma warning disable UoWIP
            if (typeof(TSelf) == typeof(Quadruple)) {
                // Only the top 64 bits (sign + exponent + high mantissa) are represented here.
                UInt128 v = (UInt128)bits << 64;
                return Unsafe.As<UInt128, TSelf>(ref v);
            }
#pragma warning restore UoWIP

            return default;
        }

        public static ulong FloatToBits(TSelf value) {
            if (typeof(TSelf) == typeof(double)) {
                return BitConverter.DoubleToUInt64Bits(Unsafe.As<TSelf, double>(ref value));
            }
            if (typeof(TSelf) == typeof(float)) {
                return BitConverter.SingleToUInt32Bits(Unsafe.As<TSelf, float>(ref value));
            }
            if (typeof(TSelf) == typeof(Half)) {
                return BitConverter.HalfToUInt16Bits(Unsafe.As<TSelf, Half>(ref value));
            }
#if NET11_0_OR_GREATER
            if (typeof(TSelf) == typeof(BFloat16)) {
                return Unsafe.As<TSelf, ushort>(ref value);
            }
#endif
#pragma warning disable UoWIP
            if (typeof(TSelf) == typeof(Quadruple)) {
                UInt128 raw = Unsafe.As<TSelf, UInt128>(ref value);
                return (ulong)(raw >> 64);
            }
#pragma warning restore UoWIP

            return default;
        }

        // TBits supports (U)Int16/32/64/128
        // zero extend/truncating
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSelf BitsToFloat<TBits>(TBits bits) where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                return Unsafe.As<TBits, TSelf>(ref bits);
            } else {
                Debug.Assert(sizeof(TBits) <= sizeof(TSelf));
                TSelf result = default;
                Unsafe.As<TSelf, TBits>(ref result) = bits;
                return result;
            }
        }

        // TBits supports (U)Int16/32/64/128
        // but not supported (throws) if sizeof(TBits) < sizeof(TSelf)
        // zero extend
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TBits FloatToBits<TBits>(TSelf value) where TBits : unmanaged, IBinaryInteger<TBits> {
            if (sizeof(TBits) >= sizeof(TSelf)) {
                return Unsafe.As<TSelf, TBits>(ref value);
            }
            throw new NotSupportedException();
        }
    }

    internal static partial class Number {

        internal const NumberStyles AllowTrailingInvalidCharacters = unchecked((NumberStyles)0x80000000);
    }
}

namespace UltimateOrb.Internal.System {
    using Globalization;

    static partial class Extensions {



        extension(NumberFormatInfo) {

            private static NumberStyles InvalidNumberStyles {
                get =>
                    ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
                    | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign
                    | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint
                    | NumberStyles.AllowThousands | NumberStyles.AllowExponent
                    | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier
                    | NumberStyles.AllowBinarySpecifier);
            }

            internal static void ValidateParseStyleFloatingPoint(NumberStyles style) {
                // Check for undefined flags, AllowBinarySpecifier (never valid for float), or AllowHexSpecifier with anything other than HexFloat flags.
                // When AllowHexSpecifier is specified, AllowExponent must also be specified; this reserves
                // AllowHexSpecifier without AllowExponent for possible future use (e.g. optional p exponent).
                if ((style & (NumberFormatInfo.InvalidNumberStyles | NumberStyles.AllowBinarySpecifier | NumberStyles.AllowHexSpecifier)) != 0 &&
                    ((style & ~NumberStyles.HexFloat) != 0 ||
                     (style & NumberStyles.AllowHexSpecifier) != 0 && (style & NumberStyles.AllowExponent) == 0)) {
                    ThrowInvalid(style);

                    static void ThrowInvalid(NumberStyles value) {
                        throw new ArgumentException(
                            (value & NumberFormatInfo.InvalidNumberStyles) != 0 ? SR.Argument_InvalidNumberStyles :
                            (value & NumberStyles.AllowBinarySpecifier) != 0 ? SR.Arg_BinaryStyleNotSupported :
                            SR.Arg_InvalidHexFloatStyle,
                            nameof(style));
                    }
                }
            }
        }
    }
}

namespace UltimateOrb {

    partial struct Quadruple {

        /// <summary>
        /// Converts <paramref name="value"/> to <see cref="decimal"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The result is the nearest representable <see cref="decimal"/> to
        /// <paramref name="value"/> under round-half-to-even, in the same sense as
        /// <see cref="Convert.ToDecimal(double)"/>: the closest <see cref="decimal"/>
        /// value, using the full 28-digit precision decimal can hold.
        /// </para>
        /// <para>
        /// NaN and ±∞ throw <see cref="OverflowException"/>.  Values exceeding
        /// <see cref="decimal.MaxValue"/> throw.  Values that round to zero
        /// (including all subnormals, and normals with |value| &lt; 0.5·10⁻²⁸) return
        /// signed zero preserving the input's sign bit.
        /// </para>
        /// <para>
        /// Note: unlike <c>(decimal)double</c>, this does <i>not</i> produce the
        /// shortest round-trippable decimal.  E.g. <c>(decimal)(Quadruple)(1.0/3.0)</c>
        /// yields twenty-eight 3s, not sixteen.
        /// </para>
        /// </remarks>
        public static explicit operator decimal(Quadruple value) {
            // NaN / ±∞ → OverflowException.  Matches (decimal)double.NaN and (decimal)double.±∞.
            if (!TryExtractComponents(value, out _, out UInt128 M, out int e)) {
                throw ThrowOverflowExceptionWin32Decimal();
            }

            // Sign is the MSB of the high 64-bit word.  Extraction reports negative=false
            // for zeros / subnormals, so we pull the raw sign bit for those paths.
            bool negative = (value._Hi64Bits >> 63) != 0;

            // ±0 and every subnormal.  Smallest subnormal is 2^-16494 ≈ 10^-4965,
            // far below decimal's smallest positive (1e-28): all round to zero.
            if (M == UInt128.Zero) {
                return negative ? new decimal(0, 0, 0, true, 0) : 0m;
            }

            // value = M · 2^e,  M ∈ [2^112, 2^113)  ⇒  value ≥ 2^(112+e).
            // value < 2^96 = decimal.MaxValue + 1 requires 112 + e < 96, i.e. e ≤ -17.
            if (e >= -16) {
                throw ThrowOverflowExceptionWin32Decimal();
            }

            // value · 10^s = M · 10^s · 2^e        (e ≥ 0, exact integer, no division)
            //              = M · 10^s / 2^(-e)     (e < 0, divide + round)
            BigInteger p = (BigInteger)M;
            BigInteger denom = e >= 0 ? BigInteger.One : (BigInteger.One << -e);
            int leftShift = e >= 0 ? e : 0;

            // Try scales most-precise first.  The first that fits 96 bits wins — it is
            // the finest-grained decimal representation of value.  The loop naturally
            // handles both underflow (div == 0) and overflow (no s fits ⇒ throw).
            for (int s = 28; s >= 0; --s) {
                BigInteger num = p * BigInteger.Pow(10, s);
                if (leftShift > 0) {
                    num <<= leftShift;
                }

                BigInteger div;
                if (e >= 0) {
                    div = num;
                } else {
                    div = BigInteger.DivRem(num, denom, out BigInteger rem);
                    if (!rem.IsZero) {
                        // Round-half-to-even.
                        int cmp = (rem << 1).CompareTo(denom);
                        if (cmp > 0 || (cmp == 0 && !div.IsEven)) {
                            div += BigInteger.One;
                        }
                    }
                }

                // 13+ bytes ⇒ div ≥ 2^96 ⇒ does not fit.  Try a coarser scale.
                if (div.GetByteCount(isUnsigned: true) > 12) {
                    continue;
                }

                // Underflow: |value| < 0.5·10^-28, so |value·10^28| < 0.5.  Coarser
                // scales only shrink |value·10^s| further, so 0m is the answer.
                if (div.IsZero) {
                    return negative ? new decimal(0, 0, 0, true, 0) : 0m;
                }

                // Canonicalise: strip trailing zeros.  Preserves the value, keeps the
                // mantissa minimal (e.g. 15·10^27 @ s=28 → 15 @ s=1).
                while (s > 0 && (div % 10).IsZero) {
                    div /= 10;
                    s -= 1;
                }

                uint lo = (uint)(div & 0xFFFF_FFFF);
                uint mid = (uint)((div >> 32) & 0xFFFF_FFFF);
                uint hi = (uint)((div >> 64) & 0xFFFF_FFFF);

                return new decimal(unchecked((int)lo), unchecked((int)mid), unchecked((int)hi),
                                   negative, (byte)s);
            }

            // No scale in [0, 28] produced a 96-bit mantissa ⇒ |value| > decimal.MaxValue.
            throw ThrowOverflowExceptionWin32Decimal();
        }

        [DoesNotReturn]
        private static OverflowException ThrowOverflowExceptionWin32Decimal() {
            var t = double.NegativeInfinity;
            _ = (decimal)t;
#pragma warning disable CS8763 // A method marked [DoesNotReturn] should not return.
            return null!;
#pragma warning restore CS8763 // A method marked [DoesNotReturn] should not return.
        }

#if NET7_0_OR_GREATER

        /// <summary>
        /// Converts a <see cref="Quadruple"/> (binary128) to an IEEE 754 binary
        /// interchange format <typeparamref name="TFloat"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <b>Widening (target ≥ binary128).</b> Exact. The significand is left-shifted
        /// and the exponent re-biased.
        /// </para>
        /// <para>
        /// <b>Narrowing (target &lt; binary128).</b> Round-to-nearest, ties-to-even on
        /// the discarded significand bits, with a single renormalization on carry.
        /// Overflow yields <c>±∞</c>; this relies on the renormalized biased exponent
        /// naturally reaching <c>expMax</c>, matching the <c>From</c> direction. Values
        /// rounding into the target subnormal range get a second round-to-nearest,
        /// ties-to-even pass, including promotion to the smallest normal.
        /// </para>
        /// <para>
        /// <b>NaN payload handling.</b> Same three modes as
        /// <c>FromIeee754InterchangeBinary</c>:
        /// <list type="bullet">
        ///   <item><description><c>NAN_PERMISSIVE</c>: caller does not observe which NaN; any NaN is returned.</description></item>
        ///   <item><description><c>NAN_PAYLOAD_LEGACY</c>: MSB-align + scatter-fold, mirroring the existing <c>Quadruple → Double</c> converter.</description></item>
        ///   <item><description>default (modern): LSB-align the payload excluding the quiet bit; widening zero-extends; narrowing keeps low bits and jams the MSB of the target payload as overflow indicator; quiet bit copied verbatim.</description></item>
        /// </list>
        /// </para>
        /// <para>
        /// <b>Preconditions.</b> Internal method. <typeparamref name="TFloat"/> must be a
        /// supported radix-2 binary interchange format and <typeparamref name="TFloatUIntBits"/>
        /// must have identical storage size. Violations are caught by
        /// <see cref="Debug.Assert"/> in Debug builds.
        /// </para>
        /// </remarks>
        internal static TFloat ToIeee754InterchangeBinaryNarrowing<TFloat, TFloatUIntBits>(Quadruple value)
            where TFloat : unmanaged, IBinaryFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits>, IMinMaxValue<TFloatUIntBits> {
            // ---- Source (binary128) constants ----
            const int QuadFracBits = 112;
            const int QuadBias = 16383;
            const int QuadExpMax = 0x7FFF;
            const int QuadPayloadBits = QuadFracBits - 1;    // 111, excludes the quiet bit
            const int QuadFoldShift = QuadFracBits / 4;     // 28; mirrors Quadruple → Double

            // ---- Guard rails (internal method: fail fast in Debug) ----
            Debug.Assert(Unsafe.SizeOf<TFloat>() == Unsafe.SizeOf<TFloatUIntBits>(),
                "T and TFloatUIntBits must have identical storage size.");
            Debug.Assert(TFloat.Radix == 2,
                "Only radix-2 (binary) interchange formats can be produced from binary128.");
            Debug.Assert(BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.IsSupported,
                $"'{typeof(TFloat).FullName}' is not a supported IEEE 754 interchange format.");

            // ---- Target format parameters ----
            int dstFracBits = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.TrailingSignificandFieldBitWidth;   // t
            int dstExpBits = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.ExponentFieldBitWidth; // w
            int dstBias = BinaryFloatingPointIeee754TypeTraitsInternal<TFloat>.MaxExponent;
            int dstExpMax = (1 << dstExpBits) - 1;

            Debug.Assert(dstFracBits > 0 && dstExpBits > 0);
            Debug.Assert(dstExpBits + dstFracBits + 1 == 8 * Unsafe.SizeOf<TFloat>());

            TFloatUIntBits dstFracMask = (TFloatUIntBits.One << dstFracBits) - TFloatUIntBits.One;
            TFloatUIntBits dstExpMask = (TFloatUIntBits.One << dstExpBits) - TFloatUIntBits.One;

            // ---- Extract raw binary128 fields (read sign directly so −0 / −subnormal survive) ----
            UInt64 lo = value._Lo64Bits;
            UInt64 hi = value._Hi64Bits;

            bool negative = (hi & (1UL << 63)) != 0;
            int quadExp = (int)((hi >> 48) & 0x7FFF);
            UInt128 quadFrac = ((UInt128)(hi & 0x0000_FFFF_FFFF_FFFFUL) << 64) | (UInt128)lo;

            TFloatUIntBits signField = negative
                ? (TFloatUIntBits.One << (dstFracBits + dstExpBits))
                : TFloatUIntBits.Zero;

            TFloatUIntBits dstExpField = TFloatUIntBits.CreateTruncating(dstExpMax) << dstFracBits;

            // =================================================================
            // ±Infinity
            // =================================================================
            if (quadExp == QuadExpMax && quadFrac == UInt128.Zero) {
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | dstExpField);
            }

            // =================================================================
            // NaN
            // =================================================================
            if (quadExp == QuadExpMax) {
#if NAN_PERMISSIVE
                // Caller does not observe which NaN is produced; any NaN satisfies the contract.
                return TFloat.NaN;
#else
#if NAN_PAYLOAD_LEGACY
                // MSB-align + scatter-fold. Symmetric with FromIeee754InterchangeBinary.
                UInt128 destFrac;
                if (QuadFracBits <= dstFracBits) {
                    // Widening: exact, nothing dropped.
                    destFrac = quadFrac << (dstFracBits - QuadFracBits);
                } else {
                    int drop = QuadFracBits - dstFracBits;

                    UInt128 shifted     = quadFrac >> drop;
                    UInt128 droppedBits = quadFrac & (((UInt128)1 << drop) - 1);

                    UInt128 fold1 = droppedBits >> QuadFoldShift;
                    UInt128 fold2 = droppedBits & (((UInt128)1 << QuadFoldShift) - 1);

                    destFrac = shifted | fold1 | fold2;
                }

                TFloatUIntBits fracField = TFloatUIntBits.CreateTruncating(destFrac) & dstFracMask;
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | dstExpField | fracField);
#else
                // Modern convention: payload is an unsigned integer excluding the quiet
                // bit, LSB-aligned across formats; narrowing jams the MSB of the target
                // payload as the overflow indicator.
                bool isQuiet = (quadFrac & ((UInt128)1 << (QuadFracBits - 1))) != UInt128.Zero;
                UInt128 srcPayload = quadFrac & (((UInt128)1 << (QuadFracBits - 1)) - 1);

                int dstPayloadBits = dstFracBits - 1;    // excludes the target quiet bit

                UInt128 dstPayload;
                if (QuadPayloadBits <= dstPayloadBits) {
                    // Widening / equal: integer value preserved, zero-extended.
                    dstPayload = srcPayload;
                } else {
                    // Narrowing: low bits kept; high bits set the overflow flag.
                    UInt128 lowMask = ((UInt128)1 << (dstPayloadBits - 1)) - 1;
                    UInt128 lowBits = srcPayload & lowMask;
                    UInt128 highBits = srcPayload >> (dstPayloadBits - 1);

                    dstPayload = lowBits;
                    if (highBits != UInt128.Zero) {
                        dstPayload |= (UInt128)1 << (dstPayloadBits - 1);
                    }
                }

                UInt128 destFrac = dstPayload;
                if (isQuiet) {
                    destFrac |= (UInt128)1 << (dstFracBits - 1);
                }
                // No class-change guard: sNaN source ⇒ payload ≠ 0; qNaN source ⇒ quiet bit set.

                TFloatUIntBits fracField = TFloatUIntBits.CreateTruncating(destFrac) & dstFracMask;
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | dstExpField | fracField);
#endif // NAN_PAYLOAD_LEGACY
#endif // !NAN_PERMISSIVE
            }

            // =================================================================
            // ±0
            // =================================================================
            if (quadExp == 0 && quadFrac == UInt128.Zero) {
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField);
            }

            // =================================================================
            // Finite nonzero: normalize source significand with implicit 1 at bit 112.
            // =================================================================
            UInt128 sig;
            int unbiased;

            if (quadExp == 0) {
                // Binary128 subnormal → normalize.
                int lz = int.CreateTruncating(System.UInt128.LeadingZeroCount(quadFrac)) - (128 - QuadFracBits);
                sig = quadFrac << (lz + 1);
                unbiased = -QuadBias - lz;
            } else {
                sig = ((UInt128)1 << QuadFracBits) | quadFrac;
                unbiased = quadExp - QuadBias;
            }

            // =================================================================
            // Align MSB of sig with the target's implicit-1 position (bit dstFracBits).
            //   shift >= 0 : widening or equal (exact).
            //   shift <  0 : narrowing  (round-half-to-even on discarded bits).
            // =================================================================
            int shift = dstFracBits - QuadFracBits;
            UInt128 sigDst;

            if (shift >= 0) {
                sigDst = sig << shift;
            } else {
                int drop = -shift;
                UInt128 dropped = sig & (((UInt128)1 << drop) - 1);
                UInt128 top = sig >> drop;
                UInt128 half = (UInt128)1 << (drop - 1);

                if (dropped > half || (dropped == half && (top & 1) != 0)) {
                    top++;
                    if (top >= ((UInt128)1 << (dstFracBits + 1))) {
                        top >>= 1;
                        unbiased++;
                    }
                }
                sigDst = top;
            }

            int biased = unbiased + dstBias;

            // =================================================================
            // Overflow → ±Infinity. Same trick as the From direction: the renormalized
            // biased exponent naturally becomes expMax, so no carry-at-emax guard.
            // =================================================================
            if (biased >= dstExpMax) {
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | dstExpField);
            }

            // =================================================================
            // Target subnormal: extra right shift with round-half-to-even.
            // =================================================================
            if (biased <= 0) {
                int drop = 1 - biased;
                if (drop > 128) {
                    return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField);
                }

                UInt128 top, dropped, half;
                if (drop == 128) {
                    top = UInt128.Zero;
                    dropped = sigDst;
                    half = (UInt128)1 << 127;
                } else {
                    top = sigDst >> drop;
                    dropped = sigDst & (((UInt128)1 << drop) - 1);
                    half = (UInt128)1 << (drop - 1);
                }

                if (dropped > half || (dropped == half && (top & 1) != 0)) {
                    top++;
                }

                // Subnormal rounding may promote to smallest normal (biased = 1, frac = 0).
                if (top == ((UInt128)1 << dstFracBits)) {
                    TFloatUIntBits promoted = TFloatUIntBits.One << dstFracBits;
                    return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | promoted);
                }

                TFloatUIntBits fracField = TFloatUIntBits.CreateTruncating(top) & dstFracMask;
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(signField | fracField);
            }

            // =================================================================
            // Target normal
            // =================================================================
            {
                UInt128 frac = sigDst & (((UInt128)1 << dstFracBits) - 1);
                TFloatUIntBits expField = TFloatUIntBits.CreateTruncating(biased) & dstExpMask;
                TFloatUIntBits fracField = TFloatUIntBits.CreateTruncating(frac) & dstFracMask;
                return UltimateOrb.Runtime.CompilerServices.Unsafe.BitCast<TFloatUIntBits, TFloat>(
                    signField | (expField << dstFracBits) | fracField);
            }
        }

        public static explicit operator Single(Quadruple value) {
            return ToIeee754InterchangeBinaryNarrowing<Single, UInt32>(value);
        }

        public static explicit operator Half(Quadruple value) {
            return ToIeee754InterchangeBinaryNarrowing<Half, UInt16>(value);
        }
#else
        public static explicit operator Single(Quadruple value) {
            return (Single)(double)value;
        }

        public static explicit operator Half(Quadruple value) {
            return (Half)(double)value;
        }
#endif

    }
}

namespace UltimateOrb {

    public static partial class BitConverterExtensions {

        extension(System.BitConverter) {

#if !NET7_0_OR_GREATER
            [Experimental("UoWIP")]
            public static Int128 QuadrupleToInt128Bits(Quadruple value) {
                 return Quadruple.BitConverter.QuadrupleToInt128Bits(value);
            }

            [Experimental("UoWIP")]
            public static Quadruple Int128BitsToQuadruple(Int128 bits) {
                return Quadruple.BitConverter.Int128BitsToQuadruple(bits);
            }

            [Experimental("UoWIP")]
            public static UInt128 QuadrupleToUInt128Bits(Quadruple value) {
                return Quadruple.BitConverter.QuadrupleToUInt128Bits(value);
            }

            [Experimental("UoWIP")]
            public static Quadruple UInt128BitsToQuadruple(UInt128 bits) {
                return Quadruple.BitConverter.UInt128BitsToQuadruple(bits);
            }
#else
            [Experimental("UoWIP")]
            public static System.Int128 QuadrupleToInt128Bits(Quadruple value) {
                return Quadruple.BitConverter.QuadrupleToInt128Bits(value);
            }

            [Experimental("UoWIP")]
            public static Quadruple Int128BitsToQuadruple(System.Int128 bits) {
                return Quadruple.BitConverter.Int128BitsToQuadruple(bits);
            }

            [Experimental("UoWIP")]
            public static System.UInt128 QuadrupleToUInt128Bits(Quadruple value) {
                return Quadruple.BitConverter.QuadrupleToUInt128Bits(value);
            }

            [Experimental("UoWIP")]
            public static Quadruple UInt128BitsToQuadruple(System.UInt128 bits) {
                return Quadruple.BitConverter.UInt128BitsToQuadruple(bits);
            }
#endif
        }
    }
}