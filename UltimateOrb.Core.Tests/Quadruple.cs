using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Mathematics;
using UltimateOrb.Numerics;
#pragma warning disable UoWIP_F128 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using static UltimateOrb.Numerics.Binary128Arithmetic;
#pragma warning restore UoWIP_F128 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
using Binary128Arithmetic = UltimateOrb.Numerics.Binary128Arithmetic;

namespace UltimateOrb {

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 16)]
    public readonly struct Quadruple : IEquatable<Quadruple> {

        readonly UInt64 _Lo64Bits;

        readonly UInt64 _Hi64Bits;

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        Quadruple(UInt64 lo64Bits, UInt64 hi64Bits) {
            _Lo64Bits = lo64Bits;
            _Hi64Bits = hi64Bits;
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
        public static Quadruple Add(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
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
        public static Quadruple Subtract(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
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
        public static Quadruple Multiply(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
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
        public static Quadruple Divide(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
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
        public static Quadruple Remainder(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
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
            throw new NotImplementedException();
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
            return Binary128Arithmetic.IsSignalingNaN(value._Lo64Bits, value._Hi64Bits);
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
        ///     The IEEE Std 754 <c>radix</c>.
        /// </summary>
        /// <param name="value">The specified value.</param>
        /// <returns>The radix b of the format of x, that is, two or ten.</returns>
        [Obsolete]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static FloatingPointRadix GetEncodingRadix(Quadruple value) {
            return FloatingPointRadix.Binary;
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

        [System.Diagnostics.DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public static Quadruple NegativeZero {

            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new Quadruple(0, unchecked((UInt64)Int64.MinValue));
            }
        }
        #endregion

        #region Conversions
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
        public static implicit operator Quadruple(UInt128 value) {
            // TODO: 
            var lo = unchecked((UInt64)value.LoInt64Bits);
            var hi = unchecked((UInt64)value.HiInt64Bits);
            return (Quadruple)lo + (Quadruple)hi + (Quadruple)UInt64.MaxValue * (Quadruple)hi;
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


        public override string ToString() {
            var f_lo = this.RawFractionLo;
            var f_hi = this.RawFractionHi;
            var n = 0 != this.RawSign;
            var e = this.RawExponent;
            if (0X7fff == e) {
                if (0 == (f_lo | f_hi)) {
                    return n ? "-Infinity" : "Infinity";
                }
                return "NaN";
            }
            if (0 == e) {
                e += 1;
            } else {
                f_hi += 0x0001000000000000;
            }
            e -= FractionBitCount + 0x3fff;
            var p = f_lo | ((BigInteger)f_hi << 64);
            var q = (BigInteger)(n ? -1 : 1);
            if (0 > e) {
                q <<= -e;
            } else {
                p <<= e;
            }
#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return BigRational.FromFraction(p, q).ToString();
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
            return base.ToString();
        }
        #endregion

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

            public static Quadruple ScaleB(Quadruple x, int n, FloatingPointRounding rounding) {
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

            public static Quadruple Sqrt(Quadruple value, FloatingPointRounding rounding) {
                return new Quadruple(Binary128Arithmetic.Sqrt(value._Lo64Bits, value._Hi64Bits, rounding, out var hi), hi);
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            public static Quadruple IEEERemainder(Quadruple first, Quadruple second) {
                return new Quadruple(Binary128Arithmetic.IEEERemainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, FloatingPointRounding.ToNearestWithMidpointToEven, out var t), t);
            }

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            public static Quadruple IEEERemainder(Quadruple first, Quadruple second, FloatingPointRounding rounding) {
                return new Quadruple(Binary128Arithmetic.IEEERemainder(first._Lo64Bits, first._Hi64Bits, second._Lo64Bits, second._Hi64Bits, rounding, out var t), t);
            }


            
        }

        public static partial class Math {



        }

        public static class Converter {

        }

        public static class BitConverter {

            //public static Int128 QuadrupleToInt128Bits(Quadruple value) {
            //    return new Int128(value._Lo64Bits, unchecked((Int64)value._Hi64Bits));
            //}

            public static Quadruple Int128BitsToQuadruple(Int128 bits) {
                return new Quadruple(unchecked((UInt64)bits.LoInt64Bits), unchecked((UInt64)bits.HiInt64Bits));
            }
        }
    }
}
