using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    public static partial class DoubleArithmetic {

#if NET7_0_OR_GREATER
        public static int LeadingZeroCount<TInt>(TInt lo, TInt hi) where TInt: IBinaryInteger<TInt> {
            return 0 != TInt.Sign(hi)
                ? int.CreateChecked(TInt.LeadingZeroCount(hi))
                : checked(8 * hi.GetByteCount() + int.CreateChecked(TInt.LeadingZeroCount(lo)));
        }
#endif
    }
}
