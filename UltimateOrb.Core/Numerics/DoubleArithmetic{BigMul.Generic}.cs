using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
#if !STANDALONE_XINTN_LIBRARY
using UltimateOrb.Numerics.BigIntegerWrappers;
#endif

namespace UltimateOrb.Numerics {

    partial class DoubleArithmetic {

#if STANDALONE_XINTN_LIBRARY
#else
#if NET8_0_OR_GREATER
        [Experimental("UoWIP_GenericMath")]
#endif
        public static T BigMulUnsigned<T>(T first, T second, out T highResult) where T : IBinaryInteger<T> {
            unchecked {
                // TODO:
                StandardGenericMathArithmeticProvider<T>.BigMulUnsigned(out var lo, out var hi, first, second);
                highResult = hi;
                return lo;
            }
        }
#endif
    }
}
