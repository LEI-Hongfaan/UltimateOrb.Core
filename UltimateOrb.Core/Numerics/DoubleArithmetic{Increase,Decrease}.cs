using System;
using static UltimateOrb.Utilities.BooleanIntegerModule;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

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

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
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

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
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

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
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

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
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

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return IncreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return IncreaseSigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return DecreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return DecreaseSigned(value_lo, value_hi, out result_hi);
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

	using Math = global::Internal.System.Math;

	using IntT = System.Int128;
	using UIntT = System.UInt128;
	
	using LIntT = System.UInt128;
	using HIntT = System.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

	using Math = global::Internal.System.Math;

	using IntT = System.Int128;
	using UIntT = System.UInt128;
	
	using LIntT = System.UInt128;
	using HIntT = System.Int128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

	using Math = global::Internal.System.Math;

	using IntT = System.Int128;
	using UIntT = System.UInt128;
	
	using LIntT = System.Int128;
	using HIntT = System.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

	using Math = global::Internal.System.Math;

	using IntT = System.Int128;
	using UIntT = System.UInt128;
	
	using LIntT = System.Int128;
	using HIntT = System.Int128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = System.UInt128;
	using Int = Int64;
	using Long = System.Int128;

	using Math = global::Internal.System.Math;

	using IntT = System.Int128;
	using UIntT = System.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return IncreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return IncreaseSigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return DecreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return DecreaseSigned(value_lo, value_hi, out result_hi);
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

	using Math = global::Internal.System.Math;

	using IntT = UltimateOrb.Int128;
	using UIntT = UltimateOrb.UInt128;
	
	using LIntT = UltimateOrb.UInt128;
	using HIntT = UltimateOrb.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

	using Math = global::Internal.System.Math;

	using IntT = UltimateOrb.Int128;
	using UIntT = UltimateOrb.UInt128;
	
	using LIntT = UltimateOrb.UInt128;
	using HIntT = UltimateOrb.Int128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

	using Math = global::Internal.System.Math;

	using IntT = UltimateOrb.Int128;
	using UIntT = UltimateOrb.UInt128;
	
	using LIntT = UltimateOrb.Int128;
	using HIntT = UltimateOrb.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

	using Math = global::Internal.System.Math;

	using IntT = UltimateOrb.Int128;
	using UIntT = UltimateOrb.UInt128;
	
	using LIntT = UltimateOrb.Int128;
	using HIntT = UltimateOrb.Int128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ += unchecked((HIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ += unchecked((IntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT IncreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				++result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ += unchecked((UIntT)(~(LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnchecked(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((HIntT)value_hi);
			unchecked {
				result_hi_ -= unchecked((HIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseSigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((IntT)value_hi);
			checked {
				result_hi_ -= unchecked((IntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}

		[System.CLSCompliantAttribute(true)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static LIntT DecreaseUnsigned(LIntT value_lo, HIntT value_hi, out HIntT result_hi) {
			var result_lo_ = value_lo;
			unchecked {
				--result_lo_;
			}
			var result_hi_ = unchecked((UIntT)value_hi);
			checked {
				result_hi_ -= unchecked((UIntT)((LIntT)0 == value_lo).AsIntegerUnsafe());
			}
			result_hi = unchecked((HIntT)result_hi_);
			return result_lo_;
		}
	}
}

namespace UltimateOrb.Numerics {
	using UInt = UInt64;
	using ULong = UltimateOrb.UInt128;
	using Int = Int64;
	using Long = UltimateOrb.Int128;

	using Math = global::Internal.System.Math;

	using IntT = UltimateOrb.Int128;
	using UIntT = UltimateOrb.UInt128;

	public static partial class DoubleArithmetic {

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return IncreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Increase(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return IncreaseSigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
			return DecreaseUnsigned(value_lo, value_hi, out result_hi);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static UIntT Decrease(UIntT value_lo, IntT value_hi, out IntT result_hi) {
			return DecreaseSigned(value_lo, value_hi, out result_hi);
		}
	}
}

namespace UltimateOrb.Numerics {

    public static partial class DoubleArithmetic<IntT, UIntT>
        where IntT : struct, System.Numerics.IBinaryInteger<IntT>, System.Numerics.ISignedNumber<IntT>
        where UIntT : struct, System.Numerics.IBinaryInteger<UIntT>, System.Numerics.IUnsignedNumber<UIntT> {

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT IncreaseUnchecked(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                ++result_lo_;
            }
            var result_hi_ = value_hi;
            unchecked {
                result_hi_ += UIntT.AllBitsSet == value_lo ? UIntT.One : UIntT.Zero;
            }
            result_hi = result_hi_;
            return result_lo_;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT IncreaseSigned(UIntT value_lo, IntT value_hi, out IntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                ++result_lo_;
            }
            var result_hi_ = value_hi;
            if (UIntT.AllBitsSet == value_lo) {
                checked {
                    ++result_hi_;
                }
            }
            result_hi = result_hi_;
            return result_lo_;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT IncreaseUnsigned(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                ++result_lo_;
            }
            var result_hi_ = value_hi;
            if (UIntT.AllBitsSet == value_lo) {
                checked {
                    ++result_hi_;
                }
            }
            result_hi = result_hi_;
            return result_lo_;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT DecreaseUnchecked(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                --result_lo_;
            }
            var result_hi_ = value_hi;
            unchecked {
                result_hi_ -= UIntT.Zero == value_lo ? UIntT.One : UIntT.Zero;
            }
            result_hi = result_hi_;
            return result_lo_;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT DecreaseSigned(UIntT value_lo, IntT value_hi, out IntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                --result_lo_;
            }
            var result_hi_ = value_hi;
            if (UIntT.Zero == value_lo) {
                checked {
                    --result_hi_;
                }
            }
            result_hi = result_hi_;
            return result_lo_;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT DecreaseUnsigned(UIntT value_lo, UIntT value_hi, out UIntT result_hi) {
            var result_lo_ = value_lo;
            unchecked {
                --result_lo_;
            }
            var result_hi_ = value_hi;
            if (UIntT.Zero == value_lo) {
                checked {
                    --result_hi_;
                }
            }
            result_hi = result_hi_;
            return result_lo_;
        }
    }
}
