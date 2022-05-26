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
		
		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool Equals(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_lo == second_lo && first_hi == second_hi;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool NotEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_lo != second_lo || first_hi != second_hi;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool LessThan(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi < second_hi || (first_hi == second_hi && first_lo < second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi <= second_hi && (first_hi != second_hi || first_lo <= second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThan(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi > second_hi || (first_hi == second_hi && first_lo > second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi >= second_hi && (first_hi != second_hi || first_lo >= second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static int Compare(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			if (first_hi < second_hi) return -1;
			if (first_hi > second_hi) return 1;
			if (first_lo < second_lo) return -1;
			if (first_lo > second_lo) return 1;
			return 0;
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
		public static bool Equals(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_lo == second_lo && first_hi == second_hi;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool NotEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_lo != second_lo || first_hi != second_hi;
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool LessThan(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi < second_hi || (first_hi == second_hi && first_lo < second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool LessThanOrEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi <= second_hi && (first_hi != second_hi || first_lo <= second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThan(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi > second_hi || (first_hi == second_hi && first_lo > second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static bool GreaterThanOrEqual(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			return first_hi >= second_hi && (first_hi != second_hi || first_lo >= second_lo);
		}

		[System.CLSCompliantAttribute(false)]
		[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
		public static int Compare(LIntT first_lo, HIntT first_hi, LIntT second_lo, HIntT second_hi) {
			if (first_hi < second_hi) return -1;
			if (first_hi > second_hi) return 1;
			if (first_lo < second_lo) return -1;
			if (first_lo > second_lo) return 1;
			return 0;
		}
	}
}
