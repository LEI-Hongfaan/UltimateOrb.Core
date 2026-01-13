using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics;

namespace UltimateOrb.Numerics {

    static class CheckSignedUnsignedPair<TInt, TUInt>
        where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>?
        where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt>? {

        internal static bool IsCompatible = Get_IsCompatible();

        private static bool Get_IsCompatible() {
            if (TInt.AllBitsSet != TInt.CreateTruncating(TUInt.AllBitsSet)) {
                return false;
            }
            if (TUInt.PopCount(TUInt.AllBitsSet) != TUInt.CreateChecked(TInt.PopCount(TInt.AllBitsSet))) {
                return false;
            }
            return true;
        }
    }
}

namespace UltimateOrb.Mathematics.Elementary {

    partial class Math {


#if NET7_0_OR_GREATER
        extension(System.Math) {

            public static TUInt AbsAsUnsigned<TInt, TUInt>(TInt value)
                where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>?
                where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt>? {
                if (!CheckSignedUnsignedPair<TInt, TUInt>.IsCompatible) {
                    ThrowHelper.ThrowNotSupportedException();
                }
                return TUInt.CreateTruncating(TInt.IsNegative(value) ? -value : value);
            }

            public static TUInt AbsAsUnsigned<TInt, TUInt>(TUInt value)
                where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>?
                where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt>? {
                if (!CheckSignedUnsignedPair<TInt, TUInt>.IsCompatible) {
                    ThrowHelper.ThrowNotSupportedException();
                }
                return value;
            }
        }
#endif
    }
}
