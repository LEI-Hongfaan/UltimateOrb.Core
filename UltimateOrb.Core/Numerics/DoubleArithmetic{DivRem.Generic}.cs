using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {
    
    public static partial class DoubleArithmetic {

#if STANDALONE_XINTN_LIBRARY
#else
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static T DivideUnsigned<T>(T lowDividend, T highDividend, T lowDivisor, T highDivisor, out T highResult) {
            if (typeof(T) == typeof(UInt64)) {
                var result_lo = Divide((UInt64)(object)lowDividend!, (UInt64)(object)highDividend!, (UInt64)(object)lowDivisor!, (UInt64)(object)highDivisor!, out var result_hi);
                highResult = (T)(object)result_hi;
                return (T)(object)result_lo;
            } else if (typeof(T) == typeof(Int64)) {
                unchecked {
                    var result_lo = Divide((UInt64)(Int64)(object)lowDividend!, (UInt64)(Int64)(object)highDividend!, (UInt64)(Int64)(object)lowDivisor!, (UInt64)(Int64)(object)highDivisor!, out var result_hi);
                    highResult = (T)(object)(Int64)result_hi;
                    return (T)(object)result_lo;
                }
            } else if (typeof(T) == typeof(UInt32)) {
                unchecked {
                    var dividend = (UInt32)(object)lowDividend! + (UInt64)(UInt32)(object)highDividend! << 32;
                    var divisor = (UInt32)(object)lowDivisor! + (UInt64)(UInt32)(object)highDivisor! << 32;
                    var result = dividend / divisor;
                    var result_lo = (UInt32)result;
                    var result_hi = (UInt32)(result >> 32);
                    highResult = (T)(object)result_hi;
                    return (T)(object)result_lo;
                }
            } else if (typeof(T) == typeof(Int32)) {
                unchecked {
                    var dividend = (UInt32)(Int32)(object)lowDividend! + (UInt64)(UInt32)(Int32)(object)highDividend! << 32;
                    var divisor = (UInt32)(Int32)(object)lowDivisor! + (UInt64)(UInt32)(Int32)(object)highDivisor! << 32;
                    var result = dividend / divisor;
                    var result_lo = (UInt32)result;
                    var result_hi = (UInt32)(result >> 32);
                    highResult = (T)(object)(Int32)result_hi;
                    return (T)(object)(Int32)result_lo;
                }
            } else if (typeof(T) == typeof(UltimateOrb.UInt128)) {
                var result_lo = Divide((UltimateOrb.UInt128)(object)lowDividend!, (UltimateOrb.UInt128)(object)highDividend!, (UltimateOrb.UInt128)(object)lowDivisor!, (UltimateOrb.UInt128)(object)highDivisor!, out var result_hi);
                highResult = (T)(object)result_hi;
                return (T)(object)result_lo;
            } else if (typeof(T) == typeof(UltimateOrb.Int128)) {
                unchecked {
                    var result_lo = Divide((UltimateOrb.UInt128)(UltimateOrb.Int128)(object)lowDividend!, (UltimateOrb.UInt128)(UltimateOrb.Int128)(object)highDividend!, (UltimateOrb.UInt128)(UltimateOrb.Int128)(object)lowDivisor!, (UltimateOrb.UInt128)(UltimateOrb.Int128)(object)highDivisor!, out var result_hi);
                    highResult = (T)(object)(UltimateOrb.Int128)result_hi;
                    return (T)(object)result_lo;
                }
            } else if (typeof(T) == typeof(System.UInt128)) {
                var result_lo = Divide((System.UInt128)(object)lowDividend!, (System.UInt128)(object)highDividend!, (System.UInt128)(object)lowDivisor!, (System.UInt128)(object)highDivisor!, out var result_hi);
                highResult = (T)(object)result_hi;
                return (T)(object)result_lo;
            } else if (typeof(T) == typeof(System.Int128)) {
                unchecked {
                    var result_lo = Divide((System.UInt128)(System.Int128)(object)lowDividend!, (System.UInt128)(System.Int128)(object)highDividend!, (System.UInt128)(System.Int128)(object)lowDivisor!, (System.UInt128)(System.Int128)(object)highDivisor!, out var result_hi);
                    highResult = (T)(object)(System.Int128)result_hi;
                    return (T)(object)result_lo;
                }
            } else {
                // TODO:
                throw new NotImplementedException();
            }
        }
#endif
    }
}
