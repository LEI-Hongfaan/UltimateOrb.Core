using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics.BigIntegerWrappers;

namespace UltimateOrb.Numerics {

    partial class DoubleArithmetic {

        [Experimental("UoWIP_GenericMath")]
        public static T BigMulUnsigned<T>(T first, T second, out T highResult) where T : IBinaryInteger<T> {
            unchecked {
                // TODO:
                StandardGenericMathArithmeticProvider<T>.BigMulUnsigned(out var lo, out var hi, first, second);
                highResult = hi;
                return lo;
            }
        }
    }
}
