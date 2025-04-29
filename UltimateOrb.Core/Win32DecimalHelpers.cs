using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

#if FEATURE_WIN32_DECIMAL_INTEROPERABILITY
    internal static class Win32DecimalHelpers {

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_flags")]
        internal static extern ref readonly Int32 GetFlagsInternal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_hi32")]
        internal static extern ref readonly UInt32 GetHigh32Internal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lo64")]
        internal static extern ref readonly UInt64 GetLow64Internal(this in decimal dec);
    }
#endif
}
