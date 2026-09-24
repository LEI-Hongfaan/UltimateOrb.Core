using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBigMulUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBigMulUnsignedProvider<TSelf, T> {

        public abstract static void BigMulUnsigned(out T result_lo, out T result_hi, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBigMulSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBigMulSignedProvider<TSelf, T> {
        public abstract static void BigMulSigned(out T result_lo, out T result_hi, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerCopyProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerCopyProvider<TSelf, T> {
        public abstract static void Copy(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerNegateSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateSignedProvider<TSelf, T> {
        public abstract static void NegateSigned(out T result, in T value);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerNegateUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateUnsignedProvider<TSelf, T> {
        public abstract static void NegateUnsigned(out T result, in T value);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerNegateUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateUncheckedProvider<TSelf, T> {
        public abstract static void NegateUnchecked(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerIncreaseProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerIncreaseProvider<TSelf, T> {
        public abstract static void Increase(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDecreaseSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseSignedProvider<TSelf, T> {
        public abstract static void DecreaseSigned(out T result, in T value);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDecreaseUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseUnsignedProvider<TSelf, T> {
        public abstract static void DecreaseUnsigned(out T result, in T value);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDecreaseUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseUncheckedProvider<TSelf, T> {
        public abstract static void DecreaseUnchecked(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerAddSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddSignedProvider<TSelf, T> {
        public abstract static void AddSigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerAddUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddUnsignedProvider<TSelf, T> {
        public abstract static void AddUnsigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerAddUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddUncheckedProvider<TSelf, T> {
        public abstract static void AddUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerSubtractSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractSignedProvider<TSelf, T> {
        public abstract static void SubtractSigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerSubtractUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractUnsignedProvider<TSelf, T> {
        public abstract static void SubtractUnsigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerSubtractUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractUncheckedProvider<TSelf, T> {
        public abstract static void SubtractUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerMultiplySignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplySignedProvider<TSelf, T> {
        public abstract static void MultiplySigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerMultiplyUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplyUnsignedProvider<TSelf, T> {
        public abstract static void MultiplyUnsigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerMultiplyUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplyUncheckedProvider<TSelf, T> {
        public abstract static void MultiplyUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivideSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideSignedProvider<TSelf, T> {
        public abstract static void DivideSigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivideUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideUnsignedProvider<TSelf, T> {
        public abstract static void DivideUnsigned(out T result, in T first, in T second);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivideUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideUncheckedProvider<TSelf, T> {
        public abstract static void DivideUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerShiftLeftProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftProvider<TSelf, T> {
        public abstract static void ShiftLeft(out T result, in T value, in int shiftCount);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerShiftLeftSignedCheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftSignedCheckedProvider<TSelf, T> {
        public abstract static void ShiftLeftSignedChecked(out T result, in T value, in int shiftCount);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerShiftLeftUnsignedCheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftUnsignedCheckedProvider<TSelf, T> {
        public abstract static void ShiftLeftUnsignedChecked(out T result, in T value, in int shiftCount);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerShiftRightSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftRightSignedProvider<TSelf, T> {
        public abstract static void ShiftRightSigned(out T result, in T value, in int shiftCount);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerShiftRightUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftRightUnsignedProvider<TSelf, T> {
        public abstract static void ShiftRightUnsigned(out T result, in T value, in int shiftCount);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerFusedMultiplyAddSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddSignedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddSigned(out T result, in T first, in T second, in T remainder);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerFusedMultiplyAddUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddUnsignedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddUnsigned(out T result, in T first, in T second, in T remainder);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerFusedMultiplyAddUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddUncheckedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddUnchecked(out T result, in T first, in T second, in T remainder);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivRemSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemSignedProvider<TSelf, T> {
        public abstract static void DivRemSigned(out T quotient, out T remainder, in T dividend, in T divisor);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivRemUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemUnsignedProvider<TSelf, T> {
        public abstract static void DivRemUnsigned(out T quotient, out T remainder, in T dividend, in T divisor);
    }

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerDivRemUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemUncheckedProvider<TSelf, T> {
        public abstract static void DivRemUnchecked(out T quotient, out T remainder, in T dividend, in T divisor);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBitwiseAndProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseAndProvider<TSelf, T> {
        public abstract static void BitwiseAnd(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBitwiseOrProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseOrProvider<TSelf, T> {
        public abstract static void BitwiseOr(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBitwiseXorProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseXorProvider<TSelf, T> {
        public abstract static void BitwiseXor(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBitwiseNotProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseNotProvider<TSelf, T> {
        public abstract static void BitwiseNot(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

#if STANDALONE_NUMERICS_LIBRARY
    internal
#else
    [Experimental("UoWIP_GenericMath")]
    public
#endif
        interface IBinaryIntegerBitwiseAndNotProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseAndNotProvider<TSelf, T> {
        public abstract static void BitwiseAndNot(out T result, in T first, in T second);
    }
}

