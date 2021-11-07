using System;

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
