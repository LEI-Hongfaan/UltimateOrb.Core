using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBigMulUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBigMulUnsignedProvider<TSelf, T> {

        public abstract static void BigMulUnsigned(out T result_lo, out T result_hi, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBigMulSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBigMulSignedProvider<TSelf, T> {
        public abstract static void BigMulSigned(out T result_lo, out T result_hi, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerCopyProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerCopyProvider<TSelf, T> {
        public abstract static void Copy(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerNegateSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateSignedProvider<TSelf, T> {
        public abstract static void NegateSigned(out T result, in T value);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerNegateUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateUnsignedProvider<TSelf, T> {
        public abstract static void NegateUnsigned(out T result, in T value);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerNegateUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerNegateUncheckedProvider<TSelf, T> {
        public abstract static void NegateUnchecked(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerIncreaseProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerIncreaseProvider<TSelf, T> {
        public abstract static void Increase(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDecreaseSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseSignedProvider<TSelf, T> {
        public abstract static void DecreaseSigned(out T result, in T value);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDecreaseUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseUnsignedProvider<TSelf, T> {
        public abstract static void DecreaseUnsigned(out T result, in T value);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDecreaseUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDecreaseUncheckedProvider<TSelf, T> {
        public abstract static void DecreaseUnchecked(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerAddSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddSignedProvider<TSelf, T> {
        public abstract static void AddSigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerAddUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddUnsignedProvider<TSelf, T> {
        public abstract static void AddUnsigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerAddUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerAddUncheckedProvider<TSelf, T> {
        public abstract static void AddUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerSubtractSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractSignedProvider<TSelf, T> {
        public abstract static void SubtractSigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerSubtractUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractUnsignedProvider<TSelf, T> {
        public abstract static void SubtractUnsigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerSubtractUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerSubtractUncheckedProvider<TSelf, T> {
        public abstract static void SubtractUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerMultiplySignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplySignedProvider<TSelf, T> {
        public abstract static void MultiplySigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerMultiplyUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplyUnsignedProvider<TSelf, T> {
        public abstract static void MultiplyUnsigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerMultiplyUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerMultiplyUncheckedProvider<TSelf, T> {
        public abstract static void MultiplyUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivideSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideSignedProvider<TSelf, T> {
        public abstract static void DivideSigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivideUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideUnsignedProvider<TSelf, T> {
        public abstract static void DivideUnsigned(out T result, in T first, in T second);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivideUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivideUncheckedProvider<TSelf, T> {
        public abstract static void DivideUnchecked(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerShiftLeftProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftProvider<TSelf, T> {
        public abstract static void ShiftLeft(out T result, in T value, in int shiftCount);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerShiftLeftSignedCheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftSignedCheckedProvider<TSelf, T> {
        public abstract static void ShiftLeftSignedChecked(out T result, in T value, in int shiftCount);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerShiftLeftUnsignedCheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftLeftUnsignedCheckedProvider<TSelf, T> {
        public abstract static void ShiftLeftUnsignedChecked(out T result, in T value, in int shiftCount);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerShiftRightSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftRightSignedProvider<TSelf, T> {
        public abstract static void ShiftRightSigned(out T result, in T value, in int shiftCount);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerShiftRightUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerShiftRightUnsignedProvider<TSelf, T> {
        public abstract static void ShiftRightUnsigned(out T result, in T value, in int shiftCount);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerFusedMultiplyAddSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddSignedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddSigned(out T result, in T first, in T second, in T remainder);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerFusedMultiplyAddUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddUnsignedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddUnsigned(out T result, in T first, in T second, in T remainder);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerFusedMultiplyAddUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerFusedMultiplyAddUncheckedProvider<TSelf, T> {
        public abstract static void FusedMultiplyAddUnchecked(out T result, in T first, in T second, in T remainder);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivRemSignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemSignedProvider<TSelf, T> {
        public abstract static void DivRemSigned(out T quotient, out T remainder, in T dividend, in T divisor);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivRemUnsignedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemUnsignedProvider<TSelf, T> {
        public abstract static void DivRemUnsigned(out T quotient, out T remainder, in T dividend, in T divisor);
    }

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerDivRemUncheckedProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerDivRemUncheckedProvider<TSelf, T> {
        public abstract static void DivRemUnchecked(out T quotient, out T remainder, in T dividend, in T divisor);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBitwiseAndProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseAndProvider<TSelf, T> {
        public abstract static void BitwiseAnd(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBitwiseOrProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseOrProvider<TSelf, T> {
        public abstract static void BitwiseOr(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBitwiseXorProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseXorProvider<TSelf, T> {
        public abstract static void BitwiseXor(out T result, in T first, in T second);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBitwiseNotProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseNotProvider<TSelf, T> {
        public abstract static void BitwiseNot(out T result, in T value);
    }
}

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_GenericMath")]
    public interface IBinaryIntegerBitwiseAndNotProvider<TSelf, T>
        where TSelf :
            IBinaryIntegerBitwiseAndNotProvider<TSelf, T> {
        public abstract static void BitwiseAndNot(out T result, in T first, in T second);
    }
}

