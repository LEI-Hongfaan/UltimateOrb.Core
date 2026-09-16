
namespace UltimateOrb.Numerics {

    internal enum FloatingPointExceptionFlags {

        DivideByZero = 0X1,

        Inexact = 0X2,

        Invalid = 0X4,

        Overflow = 0X8,

        Underflow = 0X10,
    }
}