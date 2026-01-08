using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Numerics;

namespace UltimateOrb.Numerics {

    partial class DoubleArithmetic {

#if NET7_0_OR_GREATER

        partial class PerType<T> where T : IBinaryInteger<T>? {

            internal static readonly T a = T.One << (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >> 1);
        }

        public static T BigSqrt<T>(T lo, T hi) where T : notnull, IBinaryInteger<T>? {
            if (T.IsZero(hi)) {
                return BinaryIntegerMath.ISqrt(lo);
            }
            if (!BinaryIntegerTypeTraits<T>.IsSigned && !BinaryIntegerTypeTraits<T>.IsUnsigned) {
                throw new NotSupportedException();
            }
            if (!BinaryIntegerTypeTraits<T>.IsUnsigned) {
                if (!(T.Zero <= hi)) {
                    return hi / T.Zero;
                }
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne < 0) {
                throw new NotSupportedException();
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne == 0) {
                return T.Zero;
            }

            T root = PerType<T>.a;
            if (!BinaryIntegerTypeTraits < T >.IsBounded || BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne > 512) {
                var c = hi.GetShortestBitLength();
                if (c < 57) {
                    var h = UInt64.CreateTruncating(hi);
                    var s = (57 - c) & ~1;
                    h <<= s;
                    h |= UInt64.CreateTruncating(lo >>> (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne - s));
                    root = T.CreateTruncating(Math.Sqrt((double)h)) >> (s >> 1);
                    root |= T.One;
                } else {
                    var s = (c - 57) & ~1;
                    var h = UInt64.CreateTruncating(hi >> s);
                    root = T.CreateTruncating(Math.Sqrt((double)h)) << (s >> 1);
                }
            } else {
                var a = double.CreateSaturating(hi);
                root = T.CreateTruncating(Math.Sqrt(a));
            }

            throw new NotImplementedException();
        }

#if NET8_0_OR_GREATER
        [Experimental("UoWIP_GenericMath")]
#endif
        public static T BigSqrtRem<T>(T lo, T hi, out T remainder_lo, out uint remainder_hi) where T : notnull, IBinaryInteger<T>? {
            if (T.IsZero(hi)) {
                var root = BinaryIntegerMath.SqrtRem(lo, out var rem);
                remainder_lo = rem;
                remainder_hi = 0u;
                return root;
            }
            {
                var root = BigSqrt(lo, hi);
                var s_lo = BigSquare(root, out var s_hi);
                throw new NotImplementedException();

            }
           
        }
#endif
    }
}
namespace UltimateOrb {

#if NET7_0_OR_GREATER
    public static partial class BinaryIntegerMath {

        public static T ISqrt<T>(T value) where T : notnull, IBinaryInteger<T> {
            if (!BinaryIntegerTypeTraits<T>.IsSigned && !BinaryIntegerTypeTraits<T>.IsUnsigned) {
                throw new NotSupportedException();
            }
            if (!BinaryIntegerTypeTraits<T>.IsUnsigned) {
                if (!(T.Zero <= value)) {
                    return value / T.Zero;
                }
            }
            if (typeof(T) == typeof(uint)) {
                return (T)(object)(uint)Mathematics.Elementary.Math.ISqrt((uint)(object)value);
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<uint>.BitSizeOrNegativeOne) {
                return T.CreateTruncating(ISqrt(uint.CreateTruncating(value)));
            }
            if (typeof(T) == typeof(ulong)) {
                return (T)(object)(ulong)Mathematics.Elementary.Math.ISqrt((ulong)(object)value);
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<ulong>.BitSizeOrNegativeOne) {
                return T.CreateTruncating(ISqrt(ulong.CreateTruncating(value)));
            }
            if (typeof(T) == typeof(UltimateOrb.UInt128)) {
                return (T)(object)(UltimateOrb.UInt128)Mathematics.Elementary.Math.ISqrt((UltimateOrb.UInt128)(object)value);
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<System.UInt128>.BitSizeOrNegativeOne) {
                return T.CreateTruncating((UltimateOrb.UInt128)ISqrt((UltimateOrb.UInt128)System.UInt128.CreateTruncating(value)));
            }
            return GenericMath.ISqrt(value);
        }

        public static T SqrtRem<T>(T value, out T remainder) where T : notnull, IBinaryInteger<T> {
            if (!BinaryIntegerTypeTraits<T>.IsSigned && !BinaryIntegerTypeTraits<T>.IsUnsigned) {
                throw new NotSupportedException();
            }
            if (!BinaryIntegerTypeTraits<T>.IsUnsigned) {
                if (!(T.Zero <= value)) {
                    remainder = value;
                    return value / T.Zero;
                }
            }
            if (typeof(T) == typeof(uint)) {
                uint root = Mathematics.Elementary.Math.SqrtRem((uint)(object)value, out uint rem);
                remainder = (T)(object)rem;
                return (T)(object)root;
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<uint>.BitSizeOrNegativeOne) {
                var root = SqrtRem(uint.CreateTruncating(value), out var rem);
                remainder = T.CreateTruncating(rem);
                return T.CreateTruncating(root);
            }
            if (typeof(T) == typeof(ulong)) {
                ulong root = Mathematics.Elementary.Math.SqrtRem((ulong)(object)value, out ulong rem);
                remainder = (T)(object)rem;
                return (T)(object)root;
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<ulong>.BitSizeOrNegativeOne) {
                var root = SqrtRem(ulong.CreateTruncating(value), out var rem);
                remainder = T.CreateTruncating(rem);
                return T.CreateTruncating(root);
            }
            if (typeof(T) == typeof(UltimateOrb.UInt128)) {
                var v = (UltimateOrb.UInt128)(object)value;
                UltimateOrb.UInt128 root = DoubleArithmetic.BigSqrtRem(v.GetLowPart(), v.GetHighPart(), out var rem_lo, out var rem_hi);
                UltimateOrb.UInt128 rem = new UltimateOrb.UInt128(lo: rem_lo, hi: rem_hi);
                remainder = (T)(object)rem;
                return (T)(object)root;
            }
            if (BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne >= 0 && BinaryIntegerTypeTraits<T>.BitSizeOrNegativeOne <= BinaryIntegerTypeTraits<System.UInt128>.BitSizeOrNegativeOne) {
                var root = (UltimateOrb.UInt128)SqrtRem((UltimateOrb.UInt128)System.UInt128.CreateTruncating(value), out var rem);
                remainder = T.CreateTruncating(rem);
                return T.CreateTruncating(root);
            }
            {
                return GenericMath.SqrtRem(value, out remainder);
            }
        }
    }
#endif
}
