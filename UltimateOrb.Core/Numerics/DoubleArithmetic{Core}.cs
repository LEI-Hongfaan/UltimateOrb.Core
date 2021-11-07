using System;

namespace UltimateOrb.Numerics {
	using UInt = UInt32;
	using ULong = UInt64;
	using Int = Int32;
	using Long = Int64;

	using Math = global::Internal.System.Math;

	using IntT = Int64;
	using UIntT = UInt64;
	
	using LIntT = UInt64;
	using HIntT = UInt64;

	public static partial class DoubleArithmetic {

		/// <summary>
		///     <para>
		///         Adds the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// 2.67 Cyc (special input set test6)
		public static LIntT AddUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = unchecked(first_hi + second_hi);
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ + unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) >= unchecked((UIntT)first_lo)) {
				result_hi_ = checked(unchecked((IntT)first_hi) + unchecked((IntT)second_hi));
			} else {
				if (IntT.MaxValue != unchecked((IntT)first_hi)) {
					result_hi_ = unchecked(1 + unchecked((IntT)first_hi));
					result_hi_ = checked(result_hi_ + unchecked((IntT)second_hi));
				} else {
					result_hi_ = checked(unchecked((IntT)second_hi) - IntT.MinValue);
				}
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) + unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ + 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one operand with double-precision data from the value of another operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = unchecked(first_hi - second_hi);
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ - unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one signed operand with double-precision data from the value of another signed operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi) - 1);
				if (unchecked((IntT)first_hi) <= 0) {
					if (0 <= unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)(~result_hi_));
					}
				} else {
					if (0 > unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)result_hi_);
					}
				}
			} else {
				result_hi_ = checked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi));
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one unsigned operand with double-precision data from the value of another unsigned operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) - unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ - 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)(-(IntT)value_hi)) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of a signed operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)checked(-unchecked((IntT)value_hi))) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an unsigned operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			if (0 != unchecked((IntT)value_lo)) {
				var u = checked(0 - unchecked((UIntT)value_lo));
				throw (OverflowException)null!;
			} else if (0 != unchecked((IntT)value_hi)) {
				var u = checked(0 - unchecked((UIntT)value_hi));
				throw (OverflowException)null!;
			}
			result_hi = 0;
			return 0;
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// ~17.5 cyc
		public static LIntT MultiplyUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)unchecked(result_hi_ + (UIntT)first_lo * (UIntT)second_hi + (UIntT)first_hi * (UIntT)second_lo));
			return unchecked((LIntT)result_lo_);
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplySigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var s = (0 > unchecked((IntT)(first_hi ^ second_hi)));
			if (0 > unchecked((IntT)first_hi)) {
				first_lo = NegateUnchecked(first_lo, first_hi, out first_hi);
			}
			if (0 > unchecked((IntT)second_hi)) {
				second_lo = NegateUnchecked(second_lo, second_hi, out second_hi);
			}
			{
				UIntT result_hi_;
				if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
					result_hi_ = checked(0u - unchecked((UIntT)first_hi));
					throw (OverflowException)null!;
				}
				var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
				result_hi_ = checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo));
				if (unchecked((UIntT)IntT.MinValue) <= result_hi_) {
					if ((unchecked((UIntT)IntT.MinValue) == result_hi_) && (unchecked((UIntT)UIntT.MinValue) == result_lo_) && s) {
						result_hi = unchecked((HIntT)IntT.MinValue);
						return unchecked((LIntT)UIntT.MinValue);
					}
					result_hi_ = checked(0u - unchecked((UIntT)result_hi_));
					throw (OverflowException)null!;
				}
				if (s) {
					result_lo_ = NegateUnchecked(result_lo_, result_hi_, out result_hi_);
				}
				result_hi = unchecked((HIntT)result_hi_);
				return unchecked((LIntT)result_lo_);
			}
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplyUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
				result_hi_ = checked(0u - unchecked((UIntT)first_hi));
				throw (OverflowException)null!;
			}
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo)));
			return unchecked((LIntT)result_lo_);
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt32;
	using ULong = UInt64;
	using Int = Int32;
	using Long = Int64;

	using Math = global::Internal.System.Math;

	using IntT = Int64;
	using UIntT = UInt64;
	
	using LIntT = UInt64;
	using HIntT = Int64;

	public static partial class DoubleArithmetic {

		/// <summary>
		///     <para>
		///         Adds the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// 2.67 Cyc (special input set test6)
		public static LIntT AddUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = unchecked(first_hi + second_hi);
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ + unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) >= unchecked((UIntT)first_lo)) {
				result_hi_ = checked(unchecked((IntT)first_hi) + unchecked((IntT)second_hi));
			} else {
				if (IntT.MaxValue != unchecked((IntT)first_hi)) {
					result_hi_ = unchecked(1 + unchecked((IntT)first_hi));
					result_hi_ = checked(result_hi_ + unchecked((IntT)second_hi));
				} else {
					result_hi_ = checked(unchecked((IntT)second_hi) - IntT.MinValue);
				}
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) + unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ + 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one operand with double-precision data from the value of another operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = unchecked(first_hi - second_hi);
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ - unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one signed operand with double-precision data from the value of another signed operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi) - 1);
				if (unchecked((IntT)first_hi) <= 0) {
					if (0 <= unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)(~result_hi_));
					}
				} else {
					if (0 > unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)result_hi_);
					}
				}
			} else {
				result_hi_ = checked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi));
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one unsigned operand with double-precision data from the value of another unsigned operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) - unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ - 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)(-(IntT)value_hi)) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of a signed operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)checked(-unchecked((IntT)value_hi))) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an unsigned operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			if (0 != unchecked((IntT)value_lo)) {
				var u = checked(0 - unchecked((UIntT)value_lo));
				throw (OverflowException)null!;
			} else if (0 != unchecked((IntT)value_hi)) {
				var u = checked(0 - unchecked((UIntT)value_hi));
				throw (OverflowException)null!;
			}
			result_hi = 0;
			return 0;
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// ~17.5 cyc
		public static LIntT MultiplyUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)unchecked(result_hi_ + (UIntT)first_lo * (UIntT)second_hi + (UIntT)first_hi * (UIntT)second_lo));
			return unchecked((LIntT)result_lo_);
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplySigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var s = (0 > unchecked((IntT)(first_hi ^ second_hi)));
			if (0 > unchecked((IntT)first_hi)) {
				first_lo = NegateUnchecked(first_lo, first_hi, out first_hi);
			}
			if (0 > unchecked((IntT)second_hi)) {
				second_lo = NegateUnchecked(second_lo, second_hi, out second_hi);
			}
			{
				UIntT result_hi_;
				if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
					result_hi_ = checked(0u - unchecked((UIntT)first_hi));
					throw (OverflowException)null!;
				}
				var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
				result_hi_ = checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo));
				if (unchecked((UIntT)IntT.MinValue) <= result_hi_) {
					if ((unchecked((UIntT)IntT.MinValue) == result_hi_) && (unchecked((UIntT)UIntT.MinValue) == result_lo_) && s) {
						result_hi = unchecked((HIntT)IntT.MinValue);
						return unchecked((LIntT)UIntT.MinValue);
					}
					result_hi_ = checked(0u - unchecked((UIntT)result_hi_));
					throw (OverflowException)null!;
				}
				if (s) {
					result_lo_ = NegateUnchecked(result_lo_, result_hi_, out result_hi_);
				}
				result_hi = unchecked((HIntT)result_hi_);
				return unchecked((LIntT)result_lo_);
			}
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplyUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
				result_hi_ = checked(0u - unchecked((UIntT)first_hi));
				throw (OverflowException)null!;
			}
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo)));
			return unchecked((LIntT)result_lo_);
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt32;
	using ULong = UInt64;
	using Int = Int32;
	using Long = Int64;

	using Math = global::Internal.System.Math;

	using IntT = Int64;
	using UIntT = UInt64;
	
	using LIntT = Int64;
	using HIntT = UInt64;

	public static partial class DoubleArithmetic {

		/// <summary>
		///     <para>
		///         Adds the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// 2.67 Cyc (special input set test6)
		public static LIntT AddUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = unchecked(first_hi + second_hi);
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ + unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) >= unchecked((UIntT)first_lo)) {
				result_hi_ = checked(unchecked((IntT)first_hi) + unchecked((IntT)second_hi));
			} else {
				if (IntT.MaxValue != unchecked((IntT)first_hi)) {
					result_hi_ = unchecked(1 + unchecked((IntT)first_hi));
					result_hi_ = checked(result_hi_ + unchecked((IntT)second_hi));
				} else {
					result_hi_ = checked(unchecked((IntT)second_hi) - IntT.MinValue);
				}
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) + unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ + 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one operand with double-precision data from the value of another operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = unchecked(first_hi - second_hi);
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ - unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one signed operand with double-precision data from the value of another signed operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi) - 1);
				if (unchecked((IntT)first_hi) <= 0) {
					if (0 <= unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)(~result_hi_));
					}
				} else {
					if (0 > unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)result_hi_);
					}
				}
			} else {
				result_hi_ = checked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi));
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one unsigned operand with double-precision data from the value of another unsigned operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) - unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ - 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)(-(IntT)value_hi)) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of a signed operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)checked(-unchecked((IntT)value_hi))) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an unsigned operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			if (0 != unchecked((IntT)value_lo)) {
				var u = checked(0 - unchecked((UIntT)value_lo));
				throw (OverflowException)null!;
			} else if (0 != unchecked((IntT)value_hi)) {
				var u = checked(0 - unchecked((UIntT)value_hi));
				throw (OverflowException)null!;
			}
			result_hi = 0;
			return 0;
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// ~17.5 cyc
		public static LIntT MultiplyUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)unchecked(result_hi_ + (UIntT)first_lo * (UIntT)second_hi + (UIntT)first_hi * (UIntT)second_lo));
			return unchecked((LIntT)result_lo_);
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplySigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var s = (0 > unchecked((IntT)(first_hi ^ second_hi)));
			if (0 > unchecked((IntT)first_hi)) {
				first_lo = NegateUnchecked(first_lo, first_hi, out first_hi);
			}
			if (0 > unchecked((IntT)second_hi)) {
				second_lo = NegateUnchecked(second_lo, second_hi, out second_hi);
			}
			{
				UIntT result_hi_;
				if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
					result_hi_ = checked(0u - unchecked((UIntT)first_hi));
					throw (OverflowException)null!;
				}
				var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
				result_hi_ = checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo));
				if (unchecked((UIntT)IntT.MinValue) <= result_hi_) {
					if ((unchecked((UIntT)IntT.MinValue) == result_hi_) && (unchecked((UIntT)UIntT.MinValue) == result_lo_) && s) {
						result_hi = unchecked((HIntT)IntT.MinValue);
						return unchecked((LIntT)UIntT.MinValue);
					}
					result_hi_ = checked(0u - unchecked((UIntT)result_hi_));
					throw (OverflowException)null!;
				}
				if (s) {
					result_lo_ = NegateUnchecked(result_lo_, result_hi_, out result_hi_);
				}
				result_hi = unchecked((HIntT)result_hi_);
				return unchecked((LIntT)result_lo_);
			}
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplyUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
				result_hi_ = checked(0u - unchecked((UIntT)first_hi));
				throw (OverflowException)null!;
			}
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo)));
			return unchecked((LIntT)result_lo_);
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt32;
	using ULong = UInt64;
	using Int = Int32;
	using Long = Int64;

	using Math = global::Internal.System.Math;

	using IntT = Int64;
	using UIntT = UInt64;
	
	using LIntT = Int64;
	using HIntT = Int64;

	public static partial class DoubleArithmetic {

		/// <summary>
		///     <para>
		///         Adds the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// 2.67 Cyc (special input set test6)
		public static LIntT AddUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = unchecked(first_hi + second_hi);
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ + unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) >= unchecked((UIntT)first_lo)) {
				result_hi_ = checked(unchecked((IntT)first_hi) + unchecked((IntT)second_hi));
			} else {
				if (IntT.MaxValue != unchecked((IntT)first_hi)) {
					result_hi_ = unchecked(1 + unchecked((IntT)first_hi));
					result_hi_ = checked(result_hi_ + unchecked((IntT)second_hi));
				} else {
					result_hi_ = checked(unchecked((IntT)second_hi) - IntT.MinValue);
				}
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Adds the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT AddUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo + second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) + unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) < unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ + 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one operand with double-precision data from the value of another operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = unchecked(first_hi - second_hi);
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(result_hi_ - unchecked((HIntT)(UIntT)1u));
			}
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one signed operand with double-precision data from the value of another signed operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractSigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			IntT result_hi_;
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = unchecked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi) - 1);
				if (unchecked((IntT)first_hi) <= 0) {
					if (0 <= unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)(~result_hi_));
					}
				} else {
					if (0 > unchecked((IntT)second_hi)) {
						var ignored = checked((UIntT)result_hi_);
					}
				}
			} else {
				result_hi_ = checked(unchecked((IntT)first_hi) - unchecked((IntT)second_hi));
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Subtracts the value of one unsigned operand with double-precision data from the value of another unsigned operand with double-precision data and returns the result.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT SubtractUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var result_lo_ = unchecked(first_lo - second_lo);
			var result_hi_ = checked(unchecked((UIntT)first_hi) - unchecked((UIntT)second_hi));
			if (unchecked((UIntT)result_lo_) > unchecked((UIntT)first_lo)) {
				result_hi_ = checked(result_hi_ - 1u);
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)(-(IntT)value_hi)) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of a signed operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = unchecked((LIntT)(-(IntT)value_lo));
			var result_hi_ = (((LIntT)0 == value_lo) ? unchecked((HIntT)checked(-unchecked((IntT)value_hi))) : unchecked((HIntT)(-1 - (IntT)value_hi)));
			result_hi = result_hi_;
			return result_lo_;
		}

		/// <summary>
		///     <para>
		///         Negates the specified value of an unsigned operand with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="value_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="value_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT NegateUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			if (0 != unchecked((IntT)value_lo)) {
				var u = checked(0 - unchecked((UIntT)value_lo));
				throw (OverflowException)null!;
			} else if (0 != unchecked((IntT)value_hi)) {
				var u = checked(0 - unchecked((UIntT)value_hi));
				throw (OverflowException)null!;
			}
			result_hi = 0;
			return 0;
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, no exception will be thrown and the result will be truncated, ignoring the bits beyond the precision.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         Of this operation, the sign interpretations of the operands do not matter and both signed and unsigned versions yield the same results.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		// ~17.5 cyc
		public static LIntT MultiplyUnchecked(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)unchecked(result_hi_ + (UIntT)first_lo * (UIntT)second_hi + (UIntT)first_hi * (UIntT)second_lo));
			return unchecked((LIntT)result_lo_);
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two signed operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as signed operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplySigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			var s = (0 > unchecked((IntT)(first_hi ^ second_hi)));
			if (0 > unchecked((IntT)first_hi)) {
				first_lo = NegateUnchecked(first_lo, first_hi, out first_hi);
			}
			if (0 > unchecked((IntT)second_hi)) {
				second_lo = NegateUnchecked(second_lo, second_hi, out second_hi);
			}
			{
				UIntT result_hi_;
				if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
					result_hi_ = checked(0u - unchecked((UIntT)first_hi));
					throw (OverflowException)null!;
				}
				var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
				result_hi_ = checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo));
				if (unchecked((UIntT)IntT.MinValue) <= result_hi_) {
					if ((unchecked((UIntT)IntT.MinValue) == result_hi_) && (unchecked((UIntT)UIntT.MinValue) == result_lo_) && s) {
						result_hi = unchecked((HIntT)IntT.MinValue);
						return unchecked((LIntT)UIntT.MinValue);
					}
					result_hi_ = checked(0u - unchecked((UIntT)result_hi_));
					throw (OverflowException)null!;
				}
				if (s) {
					result_lo_ = NegateUnchecked(result_lo_, result_hi_, out result_hi_);
				}
				result_hi = unchecked((HIntT)result_hi_);
				return unchecked((LIntT)result_lo_);
			}
		}

		/// <summary>
		///     <para>
		///         Multiplies the specified values of two unsigned operands with double-precision data.
		///         If the result can not be represented in regard to the precision of the destination, <see cref="OverflowException"/> will be thrown.
		///     </para>
		/// </summary>
		/// <param name="first_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="first_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the first operand.</para>
		/// </param>
		/// <param name="second_lo">
		///     <para>The <c>lo</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="second_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the second operand.</para>
		/// </param>
		/// <param name="result_hi">
		///     <para>The <c>hi</c> bits of the double-precision data of the result.</para>
		/// </param>
		/// <returns>
		///     <para>The <c>lo</c> bits of the double-precision data of the result.</para>
		/// </returns>
		/// <remarks>
		///     <para>
		///         The operands of this operation are interpreted as unsigned operands regardless of the sign interpretations implied from their signatures.
		///     </para>
		/// </remarks>
		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT MultiplyUnsigned(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi, out HIntT result_hi) {
			UIntT result_hi_;
			if (first_hi != (HIntT)0u && second_hi != (HIntT)0u) {
				result_hi_ = checked(0u - unchecked((UIntT)first_hi));
				throw (OverflowException)null!;
			}
			var result_lo_ = BigMul(unchecked((UIntT)first_lo), unchecked((UIntT)second_lo), out result_hi_);
			result_hi = unchecked((HIntT)checked(result_hi_ + unchecked((UIntT)first_lo) * unchecked((UIntT)second_hi) + unchecked((UIntT)first_hi) * unchecked((UIntT)second_lo)));
			return unchecked((LIntT)result_lo_);
		}
	}
}
