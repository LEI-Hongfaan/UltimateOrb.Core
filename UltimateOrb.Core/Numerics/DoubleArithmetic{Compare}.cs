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

#if NET7_0_OR_GREATER
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
#endif

#if NET7_0_OR_GREATER
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
#endif

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
