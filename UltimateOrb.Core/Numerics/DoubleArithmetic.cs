using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    /// <summary>
    ///     <para>
    ///         Provides double precision operations of <see cref="Long"/> and <see cref="ULong"/> and other operations to support longer numeric data types. 
    ///     </para>
    /// </summary>
    public static partial class DoubleArithmetic {

#if NET7_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static  UInt64 GetHighPart(this System.UInt128 value) {
            return unchecked((UInt64)(value >>> 64));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetLowPart(this System.UInt128 value) {
            return unchecked((UInt64)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int64 GetHighPart(this System.Int128 value) {
            return unchecked((Int64)(value >>> 64));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetLowPart(this System.Int128 value) {
            return unchecked((UInt64)value);
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetHighPart(this UltimateOrb.UInt128 value) {
            return unchecked((UInt64)value.HiInt64Bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetLowPart(this UltimateOrb.UInt128 value) {
            return unchecked((UInt64)value.LoInt64Bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Int64 GetHighPart(this UltimateOrb.Int128 value) {
            return unchecked((Int64)value.HiInt64Bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt64 GetLowPart(this UltimateOrb.Int128 value) {
            return unchecked((UInt64)value.LoInt64Bits);
        }

        private static partial class Misc {

            public static partial class UInt {

                public const int Size = 4;

                public const int BitSize = 32;
            }

            public static partial class ULong {

                public const int Size = 8;

                public const int BitSize = 64;
            }

            public static partial class Int {

                public const int Size = 4;

                public const int BitSize = 32;
            }

            public static partial class Long {

                public const int Size = 8;

                public const int BitSize = 64;
            }

            public static partial class UIntPtr {

                public static readonly int Size = System.UIntPtr.Size;

                public static readonly int BitSize = 8 * Size;
            }

            public static partial class IntPtr {

                public static readonly int Size = System.IntPtr.Size;

                public static readonly int BitSize = 8 * Size;
            }
        }
    }
}
