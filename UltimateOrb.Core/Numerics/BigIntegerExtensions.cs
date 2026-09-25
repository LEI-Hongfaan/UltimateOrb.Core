using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        static partial class BigIntegerExtensions {

        public static bool TestBit(this BigInteger value, int bitPosition) {
            ArgumentOutOfRangeException.ThrowIfNegative(bitPosition, nameof(bitPosition));
            return bitPosition >= int.MaxValue ? BigInteger.IsNegative(value) : !(value & (BigInteger.One << bitPosition)).IsZero;
        }

        [CLSCompliant(false)]
        public static bool TestBit(this BigInteger value, uint bitPosition) {
            return bitPosition >= int.MaxValue.ToUnsignedUnchecked() ? BigInteger.IsNegative(value) : !(value & (BigInteger.One << bitPosition.ToSignedUnchecked())).IsZero;
        }
    }

    static partial class BigIntegerExtensions {

        extension(BigInteger) {

            /// <summary>
            /// Gets a BigInteger value that represents the number two.
            /// </summary>
            public static BigInteger Two => new BigInteger(2);
        }
    }
}
