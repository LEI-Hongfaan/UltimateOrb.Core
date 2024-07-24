using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UltimateOrb.Mathematics;
using UltimateOrb.Utilities.Extensions;
using static UltimateOrb.Utilities.BooleanIntegerModule;
using static UltimateOrb.Utilities.ThrowHelper;

namespace UltimateOrb.Numerics {

    [Experimental("UoWIP_F128")]
    public static partial class Binary128Arithmetic {

        public const int FractionBitCount = 112;

        public const int ExponentBitCount = 15;

        internal const UInt64 Hi64BitsImplicitBit = (UInt64)1 << (FractionBitCount - 64);

        internal const UInt64 Hi64BitsFractionMask = Hi64BitsImplicitBit - 1;

        internal const UInt64 Hi64BitsExponentMask = (((UInt64)1 << ExponentBitCount) - 1) << (FractionBitCount - 64);

        public static int GetRawExponentFromHi64Bits(UInt64 hi) {
            const int ExponentMask = (1 << ExponentBitCount) - 1;
            return ExponentMask & unchecked((int)((Int64)hi >> (FractionBitCount - 64)));
        }

        public static int GetRawSignFromHi64Bits(UInt64 hi) {
            return unchecked((int)((UInt64)hi >> (64 - 1)));
        }

        public static UInt64 GetRawFractionHiFromHi64Bits(UInt64 hi) {
            return Hi64BitsFractionMask & hi;
        }

        public static UInt64 GetHi64BitsFromRawParts(int sign, int exponent, UInt64 fraction_hi) {
            return unchecked(((UInt64)sign << (64 - 1)) + ((UInt64)exponent << (FractionBitCount - 64)) + fraction_hi);
        }

        public static UInt64 ShiftRightWithJamming(UInt64 value_lo, UInt64 value_hi, int count, out UInt64 result_hi) {
            Debug.Assert(0 <= count);
            if (count < 64) {
                var minus_count = unchecked(-count);
                result_hi = value_hi >> count;
                return value_hi << (/*63 & */minus_count) | value_lo >> count | ((UInt64)(value_lo << (/*63 & */minus_count)) == 0 ? 0u : 1u);
            } else {
                result_hi = 0;
                return (count < 127) ?
                    value_hi >> (/*63 & */count) | (((value_hi & unchecked(((UInt64)1 << (/*63 & */count)) - 1)) | value_lo) == 0 ? 0u : 1u) :
                    ((value_hi | value_lo) == 0 ? 0u : 1u);
            }
        }

        public static UInt64 ShiftRightWithJamming(UInt64 value_cy, UInt64 value_lo, UInt64 value_hi, int count, out UInt64 result_lo, out UInt64 result_hi) {
            UInt64 r;
            var minus_count = unchecked(-count);
            if (count < 64) {
                result_hi = value_hi >> count;
                result_lo = (value_hi << (/*63 & */minus_count)) | (value_lo >> count);
                r = value_lo << (/*63 & */minus_count);
            } else {
                result_hi = 0;
                if (count == 64) {
                    result_lo = value_hi;
                    r = value_lo;
                } else {
                    value_cy |= value_lo;
                    if (count < 128) {
                        result_lo = value_hi >> (/*63 & */count);
                        r = value_hi << (/*63 & */minus_count);
                    } else {
                        result_lo = 0;
                        r = (count == 128) ? value_hi : (value_hi != 0 ? (UInt64)1 : 0);
                    }
                }
            }
            if (0 != value_cy) {
                r |= 1;
            }
            return r;
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static UInt64 ShiftLeftPartial(UInt64 low, UInt64 high, int count, out UInt64 highResult) {
            unchecked {
                /*
                if (count == 0) {
                    highResult = high;
                    return low;
                }
                */
                highResult = (UInt64)(((UInt64)high << count) | ((UInt64)low >> (-count/* 64 - count */)));
                return (UInt64)(low << count);
            }
        }

        public static UInt64 ShiftRightWithJammingPartial(UInt64 value_cy, UInt64 value_lo, UInt64 value_hi, int count, out UInt64 result_lo, out UInt64 result_hi) {
            var minus_count = unchecked(-count);
            result_hi = value_hi >> count;
            result_lo = (value_hi << (/*63 & */minus_count)) | (value_lo >> count);
            return (value_lo << (/*63 & */minus_count)) | (0 == value_cy ? 0u : 1u);
        }

        //public static int GetShiftedSignificandFromSubnormalRawFraction(UInt64 fraction_lo, UInt64 fraction_hi, out UInt64 result_lo, out UInt64 result_hi) {
        //    int shift_count;
        //    int exponent;
        //    if (0 == fraction_hi) {
        //        shift_count = unchecked(BinaryNumerals.CountLeadingZeros(fraction_lo) - 15);
        //        exponent = unchecked(-63 - shift_count);
        //        if (shift_count < 0) {
        //            result_lo = fraction_lo << (/*63 & */shift_count);
        //            result_hi = fraction_lo >> unchecked(-shift_count);
        //        } else {
        //            result_lo = 0;
        //            result_hi = fraction_lo << shift_count;
        //        }
        //    } else {
        //        shift_count = unchecked(BinaryNumerals.CountLeadingZeros(fraction_hi) - 15);
        //        exponent = 1 - shift_count;
        //        result_lo = ShiftLeftPartial(fraction_hi, fraction_lo, shift_count, out result_hi);
        //    }
        //    return exponent;
        //}

        internal static UInt64 GetNaN(UInt64 value_lo, UInt64 value_hi, out UInt64 result_hi) {
            return (result_hi = /*0x0000800000000000u |*/ value_hi).Comma(value_lo);
        }

        internal static UInt64 GetNaN(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, out UInt64 result_hi) {
            var mask_eq = 0x7FFF800000000000u;
            var mask_fNq = 0x00007FFFFFFFFFFFu;
            var first_eq = mask_eq & first_hi;
            if ((Hi64BitsExponentMask == first_eq) && (0 != (first_lo | (mask_fNq & first_hi)))) {
                // first: sNaN
                // second: ?
                {
                    // is_quiet
                    result_hi =
                        // 0x0000800000000000u |
                        first_hi;
                    return first_lo;
                }
            }
            var second_eq = mask_eq & second_hi;
            {
                // first: qNaN or ...
                // second: ?
                if ((Hi64BitsExponentMask == second_eq) && (0 != (second_lo | (mask_fNq & second_hi)))) {
                    // first: qNaN or ...
                    // second: sNaN
                    // is_quiet
                    result_hi =
                        // 0x0000800000000000u |
                        second_hi;
                    return second_lo;
                }

                if (mask_eq == first_eq) {
                    // first: qNaN
                    // second: qNaN or ...
                    {
                        // first: qNaN
                        // second: Infinity or ...
                        result_hi = first_hi;
                        return first_lo;
                    }
                }
                {
                    // at least one NaN
                    Debug.Assert(mask_eq == second_eq);
                    {
                        // first: Infinity or ...
                        // second: qNaN 
                        result_hi = second_hi;
                        return second_lo;
                    }
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetBitsFromRawPartsWithRounding(UInt64 fraction_cy, UInt64 fraction_lo, UInt64 fraction_hi, int exponent, int sign, FloatingPointRounding rounding, out UInt64 result_hi) {
            var roundTiesToEven = (rounding == FloatingPointRounding.ToNearestWithMidpointToEven);
            var cy = (0 > unchecked((Int64)fraction_cy));
            if (!roundTiesToEven && (rounding != FloatingPointRounding.ToNearestWithMidpointAwayFromZero)) {
                // unchecked(FloatingPointRounding.Up - sign) == (((0 != sign) ? FloatingPointRounding.Down : FloatingPointRounding.Up))
                cy = (rounding == GetRoundingTowardInfinityFromSign(sign)) && (0 != fraction_cy);
            }
            if (0x7FFD <= unchecked((uint)exponent)) {
                if (exponent < 0) {
                    fraction_cy = Binary128Arithmetic.ShiftRightWithJamming(fraction_cy, fraction_lo, fraction_hi, unchecked(-exponent), out fraction_lo, out fraction_hi);
                    exponent = 0;
                    cy = (0 > unchecked((Int64)fraction_cy));
                    if (!roundTiesToEven && (rounding != FloatingPointRounding.ToNearestWithMidpointAwayFromZero)) {
                        cy = (rounding == GetRoundingTowardInfinityFromSign(sign)) && (0 != fraction_cy);
                    }
                } else if (
                    (0x7FFD < exponent) || ((exponent == 0x7FFD) &&
                    DoubleArithmetic.Equals(fraction_lo, fraction_hi, 0xFFFFFFFFFFFFFFFFU, 0x0001FFFFFFFFFFFFU) && cy)
                ) {
                    if (
                        roundTiesToEven ||
                        (rounding == FloatingPointRounding.ToNearestWithMidpointAwayFromZero) ||
                        (rounding == GetRoundingTowardInfinityFromSign(sign))
                    ) {
                        result_hi = 0x7FFF000000000000U | (unchecked((UInt64)sign) << (64 - 1));
                        return 0;
                    } else {
                        result_hi = 0x7FFEFFFFFFFFFFFFU | (unchecked((UInt64)sign) << (64 - 1));
                        return 0xFFFFFFFFFFFFFFFFU;
                    }
                }
            }
            if (0 != fraction_cy) {
                // Rounding: ToOdd
                if (rounding == FloatingPointRounding.ToOdd) {
                    fraction_lo |= 1;
                    goto L_1;
                }
            }
            if (cy) {
                fraction_lo = DoubleArithmetic.IncreaseUnchecked(fraction_lo, fraction_hi, out fraction_hi);
                fraction_lo &= ((0 == (fraction_cy & 0x7FFFFFFFFFFFFFFFU)) && roundTiesToEven) ? ~(UInt64)1 : ~(UInt64)0;
            } else {
                if (0 == (fraction_hi | fraction_lo)) {
                    exponent = 0;
                }
            }
        L_1:;
            result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts(sign, exponent, fraction_hi);
            return fraction_lo;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static FloatingPointRounding GetRoundingTowardInfinityFromSign(int sign) {
            System.Diagnostics.Debug.Assert(unchecked((uint)sign) <= 1);
            return unchecked(FloatingPointRounding.Upward + sign);
        }

        // [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 BigDivRemPartialInternal(UInt64 dividend_lo_lo, UInt64 dividend_lo_hi, UInt64 dividend_hi, UInt64 divisor, out UInt64 remainder, out UInt64 quotient_hi) {
            System.Diagnostics.Debug.Assert(0 != dividend_hi);
            System.Diagnostics.Debug.Assert(divisor > dividend_hi);
            unchecked {
                quotient_hi = DoubleArithmetic.BigDivRemPartialInternal(dividend_lo_hi, dividend_hi, divisor, out var r);
                return DoubleArithmetic.BigDivRemPartialInternal(dividend_lo_lo, r, divisor, out remainder);
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 SqrtWithRoundingToOddPartial(UInt64 radicand_lo, UInt64 radicand_hi, out UInt64 remainder) {
            Debug.Assert(0u != radicand_hi);
            Debug.Assert(radicand_lo <= ~(UInt64)0 >> 2);
            unchecked {
                var old_lo = 0;
                var old_hi = (UInt64)(67108864.0 * System.BitConverter.Int64BitsToDouble(System.BitConverter.DoubleToInt64Bits(System.Math.Sqrt(radicand_hi)) - 1));
                // var l = DoubleArithmetic.BigDivRemPartialInternal (old_lo, old_hi, ,  out h);
                throw new NotImplementedException();
                /*
                var a = (UInt64)0u;
                var lo = radicand << (Misc.ULong.BitSize - 2);
                radicand = radicand >> 2;
                ULong h;
                DoubleArithmetic.BigDivNoThrow
                var l = DoubleArithmetic.BigSquare(old_hi, out h);
                l += lo;
                h += radicand;
                if (l < lo) {
                    ++h;
                }
                var @new = MathEx.BigDivNoThrow(l, h, old) >> 1;
                l = MathEx.BigSquare(@new, out h);
                if ((h > radicand) || ((h == radicand) && (l > lo))) {
                    --@new;
                    @new <<= 1;
                    radicand = (radicand << 2) | (lo >> (Misc.ULong.BitSize - 2));
                    lo = a;
                    a = (@new << 1) + 1u;
                    var b = l;
                    l <<= 2;
                    h = (h << 2) + (ULong)(((l < a) ? -1 : 0) + ((0 > (Long)@new) ? -1 : 0) + ((int)(b >> (Misc.ULong.BitSize - 2))));
                    l -= a;
                } else {
                    @new <<= 1;
                    radicand = (radicand << 2) | (lo >> (Misc.ULong.BitSize - 2));
                    lo = a;
                    a = (@new << 1) + 1u;
                    var b = l;
                    l <<= 2;
                    l += a;
                    h = (h << 2) + (((l < a) ? 1u : 0u) + ((0 > (Long)@new) ? 1u : 0u) + ((uint)(b >> (Misc.ULong.BitSize - 2))));
                }
                if ((h > radicand) || ((h == radicand) && (l > lo))) {
                } else {
                    ++@new;
                }
                return @new;
                */
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 Round(UInt64 value_lo, UInt64 value_hi, FloatingPointRounding roundingMode, out UInt64 result_hi) {
            unchecked {
                var exponent = GetRawExponentFromHi64Bits(value_hi);
                UInt64 lo;
                UInt64 hi;
                UInt64 correction_lo;
                UInt64 correction_hi;
                UInt64 roundingBitsMask;
                if (0x402F <= exponent) {

                    if (0x406F <= exponent) {
                        result_hi = value_hi;
                        return value_lo;
                    }

                    correction_lo = (UInt64)2 << (0x406E - exponent);

                    roundingBitsMask = correction_lo - 1;
                    hi = value_hi;
                    lo = value_lo;
                    var roundNearEven = (roundingMode == FloatingPointRounding.ToNearestWithMidpointToEven);
                    if (roundNearEven || (roundingMode == FloatingPointRounding.ToNearestWithMidpointAwayFromZero)) {
                        if (exponent == 0x402F) {
                            if (0x8000000000000000u <= lo) {
                                ++hi;
                                if (
                                    roundNearEven
                                        && (lo == 0x8000000000000000u)
                                ) {
                                    hi &= ~(UInt64)1;
                                }
                            }
                        } else {

                            lo = DoubleArithmetic.AddUnchecked(lo, hi, correction_lo >> 1, 0u, out hi);
                            if (roundNearEven && (0 == (lo & roundingBitsMask))) {
                                lo &= ~correction_lo;
                            }
                        }
                    } else if (
                        roundingMode
                            == (0 > (Int64)hi ? FloatingPointRounding.Downward
                                    : FloatingPointRounding.Upward)
                    ) {
                        lo = DoubleArithmetic.AddUnchecked(lo, hi, roundingBitsMask, 0u, out hi);

                    }
                    lo &= ~roundingBitsMask;
                    correction_hi = (uint)(0 == correction_lo).AsIntegerUnsafe();
                } else {
                    if (exponent < 0x3FFF) {
                        if (0 == ((value_hi & 0x7FFFFFFFFFFFFFFFu) | value_lo)) {
                            result_hi = value_hi;
                            return value_lo;
                        }

                        hi = value_hi & 0x8000000000000000u;
                        lo = 0;
                        switch (roundingMode) {
                        case FloatingPointRounding.ToNearestWithMidpointToEven:
                            if (0 == (GetRawFractionHiFromHi64Bits(value_hi) | value_lo)) {
                                break;
                            }

                            goto case FloatingPointRounding.ToNearestWithMidpointAwayFromZero;
                        case FloatingPointRounding.ToNearestWithMidpointAwayFromZero:
                            if (exponent == 0x3FFE) {
                                hi |= 0x3FFF000000000000u;
                            }

                            break;
                        case FloatingPointRounding.Downward:
                            if (0 != hi) {
                                hi = 0xBFFF000000000000u;
                            }

                            break;
                        case FloatingPointRounding.Upward:
                            if (0 == hi) {
                                hi = 0x3FFF000000000000u;
                            }

                            break;
                        case FloatingPointRounding.ToOdd:
                            hi |= 0x3FFF000000000000u;
                            break;
                        }
                        goto L_1;
                    }

                    hi = value_hi;
                    lo = 0;
                    correction_hi = (UInt64)1 << (0x402F - exponent);
                    roundingBitsMask = correction_hi - 1;
                    if (roundingMode == FloatingPointRounding.ToNearestWithMidpointAwayFromZero) {
                        hi += correction_hi >> 1;
                    } else if (roundingMode == FloatingPointRounding.ToNearestWithMidpointToEven) {
                        hi += correction_hi >> 1;
                        if (0 == ((hi & roundingBitsMask) | value_lo)) {
                            hi &= ~correction_hi;
                        }
                    } else if (
                        roundingMode
                            == (0 > (Int64)hi ? FloatingPointRounding.Downward
                                    : FloatingPointRounding.Upward)
                    ) {
                        hi = (hi | (value_lo != 0 ? 1u : 0u)) + roundingBitsMask;
                    }
                    hi &= ~roundingBitsMask;
                    correction_lo = 0;
                }

                if ((hi != value_hi) || (lo != value_lo)) {
                    if (roundingMode == FloatingPointRounding.ToOdd) {
                        hi |= correction_hi;
                        lo |= correction_lo;
                    }
                }
            L_1:;
                result_hi = hi;
                return lo;
            }
        }

        /*
        public static UInt64 Sqrt(UInt64 value_lo, UInt64 value_hi, out UInt64 result_hi) {
            if (0 > unchecked(value_hi)) {
                goto L_Neg;
            }
            var e = GetRawExponentFromHi64Bits(value_hi);
            if (0x7fff > e) {
                UInt64 lo = value_lo;
                UInt64 hi = GetRawFractionHiFromHi64Bits(value_hi);
                if (e <= 0) {
                    if (IsPositiveZero(value_lo, value_hi)) {
                        goto L_1;
                    }
                    // Subnormal
                    e = GetShiftedSignificandFromSubnormalRawFraction(lo, hi, out lo, out hi);
                }
                hi |= Hi64BitsImplicitBit;
                //DoubleArithmetic.BigSqrtRem();

            }
        L_1:;
            result_hi = value_hi;
            return value_lo;
        L_Neg:;
            if (0 == value_lo) {
                if (0x8000000000000000u == value_hi || value_hi > 0xffff000000000000u) {
                    goto L_1;
                }
            } else {
                if (0xffff000000000000u <= value_hi) {
                    goto L_1;
                }
            }
        L_DefaultNaN:;
            return GetNaN(out result_hi);
        }
        */

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsPositiveZero(UInt64 value_lo, UInt64 value_hi) {
            return 0 == (value_lo | value_hi);
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsSignalingNaN(UInt64 value_lo, UInt64 value_hi) {
            return (0x7FFF000000000000u == (0x7FFF800000000000u & value_hi)) && (0 != value_lo || 0 != (0x00007FFFFFFFFFFF & value_hi));
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static UInt64 GetBitsFromRawPartsWithNormalizationAndRounding(UInt64 fraction_lo, UInt64 fraction_hi, int exponent, int sign, FloatingPointRounding rounding, out UInt64 result_hi) {
            if (0 == fraction_hi) {
                unchecked {
                    exponent -= 64;
                }
                fraction_hi = fraction_lo;
                fraction_lo = 0;
            }
            var shift_count = unchecked(BinaryNumerals.CountLeadingZeros(fraction_hi) - 15);
            unchecked {
                exponent -= shift_count;
            }
            UInt64 fraction_cy;
            if (0 <= shift_count) {
                if (0 != shift_count) {
                    fraction_lo = ShiftLeftPartial(fraction_lo, fraction_hi, shift_count, out fraction_hi);
                }
                if (unchecked((uint)exponent) < 0x7FFD) {
                    result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts(sign, 0 == (fraction_hi | fraction_lo) ? 0 : exponent, fraction_hi);
                    return fraction_lo;
                }
                fraction_cy = 0;
            } else {
                fraction_cy = Binary128Arithmetic.ShiftRightWithJammingPartial(0, fraction_lo, fraction_hi, unchecked(-shift_count), out fraction_lo, out fraction_hi);
            }
            return Binary128Arithmetic.GetBitsFromRawPartsWithRounding(fraction_cy, fraction_lo, fraction_hi, exponent, sign, rounding, out result_hi);

        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetNaN(out UInt64 result_hi) {
            result_hi = 0xFFFF800000000000u;
            return 0;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 AddAbsoluteValuesThenCopySign(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, int sign, FloatingPointRounding rounding, out UInt64 result_hi) {
            int first_e;
            UInt64 first_f_lo, first_f_hi;
            int second_e;
            UInt64 second_f_lo, second_f_hi;
            int de;
            UInt64 result_f_lo, result_f_hi;
            int result_e;
            UInt64 result_f_cy;

            first_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(first_hi);
            first_f_hi = Hi64BitsFractionMask & first_hi;
            first_f_lo = first_lo;
            second_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(second_hi);
            second_f_hi = Hi64BitsFractionMask & second_hi;
            second_f_lo = second_lo;
            de = unchecked(first_e - second_e);
            if (0 == de) {
                if (first_e == 0x7FFF) {
                    if (0 != (first_f_hi | first_f_lo | second_f_hi | second_f_lo)) {
                        goto L_NaN;
                    }

                    result_hi = first_hi;
                    return first_lo;
                }
                result_f_lo = DoubleArithmetic.AddUnchecked(first_f_lo, first_f_hi, second_f_lo, second_f_hi, out result_f_hi);
                if (0 == first_e) {
                    result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts(sign, 0, result_f_hi);
                    return result_f_lo;

                }
                result_e = first_e;
                result_f_hi |= 0x0002000000000000u;
                result_f_cy = 0;
                goto L_ShR;
            }
            if (de < 0) {
                if (second_e == 0x7FFF) {
                    if (0 != (second_f_hi | second_f_lo)) {
                        goto L_NaN;
                    }

                    result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts(sign, 0x7FFF, 0);
                    return 0;
                }
                result_e = second_e;
                if (0 != first_e) {
                    first_f_hi |= 0x0001000000000000u;
                } else {
                    unchecked {
                        ++de;
                    }
                    result_f_cy = 0;
                    if (0 == de) {
                        goto L_A;
                    }
                }
                result_f_cy = Binary128Arithmetic.ShiftRightWithJamming(0, first_f_lo, first_f_hi, unchecked(-de), out first_f_lo, out first_f_hi);
            } else {
                if (first_e == 0x7FFF) {
                    if (0 != (first_f_hi | first_f_lo)) {
                        goto L_NaN;
                    }

                    result_hi = first_hi;
                    return first_lo;
                }
                result_e = first_e;
                if (0 != second_e) {
                    second_f_hi |= 0x0001000000000000u;
                } else {
                    unchecked {
                        --de;
                    }
                    result_f_cy = 0;
                    if (0 == de) {
                        goto L_A;
                    }
                }
                result_f_cy = Binary128Arithmetic.ShiftRightWithJamming(0, second_f_lo, second_f_hi, de, out second_f_lo, out second_f_hi);
            }
        L_A:;
            result_f_lo = DoubleArithmetic.AddUnchecked(first_f_lo, first_f_hi | 0x0001000000000000u, second_f_lo, second_f_hi, out result_f_hi);
            unchecked {
                --result_e;
            }
            if (result_f_hi < 0x0002000000000000u) {
                goto L_1;
            }

            unchecked {
                ++result_e;
            }
        L_ShR:;
            result_f_cy = Binary128Arithmetic.ShiftRightWithJammingPartial(result_f_cy, result_f_lo, result_f_hi, 1, out result_f_lo, out result_f_hi);
        L_1:;
            return Binary128Arithmetic.GetBitsFromRawPartsWithRounding(result_f_cy, result_f_lo, result_f_hi, result_e, sign, rounding, out result_hi);
        L_NaN:;
            return Binary128Arithmetic.GetNaN(first_lo, first_hi, second_lo, second_hi, out result_hi);
        }



        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 SubtractAbsoluteValuesThenCopySign(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, int sign, FloatingPointRounding rounding, out UInt64 result_hi) {
            int first_e;
            UInt64 first_f_lo, first_f_hi;
            int second_e;
            UInt64 second_f_lo, second_f_hi;
            int de;
            UInt64 result_f_lo, result_f_hi;
            int result_e;

            first_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(first_hi);
            first_f_hi = Hi64BitsFractionMask & first_hi;
            first_f_lo = first_lo;
            second_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(second_hi);
            second_f_hi = Hi64BitsFractionMask & second_hi;
            second_f_lo = second_lo;
            first_f_lo = ShiftLeftPartial(first_f_lo, first_f_hi, 4, out first_f_hi);
            second_f_lo = ShiftLeftPartial(second_f_lo, second_f_hi, 4, out second_f_hi);
            unchecked {
                de = first_e - second_e;
            }
            if (0 < de) {
                goto L_e_GT;
            }

            if (de < 0) {
                goto L_e_LT;
            }

            if (first_e == 0x7FFF) {
                if (0 != (first_f_hi | first_f_lo | second_f_hi | second_f_lo)) {
                    goto L_NaN;
                }
                // inf - inf
                return Binary128Arithmetic.GetNaN(out result_hi);
            }
            result_e = first_e;
            if (0 == result_e) {
                result_e = 1;
            }

            if (second_f_hi < first_f_hi) {
                goto L_GT;
            }

            if (first_f_hi < second_f_hi) {
                goto L_LT;
            }

            if (second_f_lo < first_f_lo) {
                goto L_GT;
            }

            if (first_f_lo < second_f_lo) {
                goto L_LT;
            }

            result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts((rounding == FloatingPointRounding.Downward) ? 1 : 0, 0, 0);
            return 0;

        L_e_LT:;
            if (second_e == 0x7FFF) {
                if (0 != (second_f_hi | second_f_lo)) {
                    goto L_NaN;
                }

                result_hi = Binary128Arithmetic.GetHi64BitsFromRawParts(sign ^ 1, 0x7FFF, 0);
                return 0;
            }
            if (0 != first_e) {
                first_f_hi |= 0x0010000000000000u;
            } else {
                unchecked {
                    ++de;
                }
                if (0 == de) {
                    goto L_A_LT;
                }
            }
            first_f_lo = Binary128Arithmetic.ShiftRightWithJamming(first_f_lo, first_f_hi, unchecked(-de), out first_f_hi);
        L_A_LT:;
            result_e = second_e;
            second_f_hi |= 0x0010000000000000u;
        L_LT:;
            sign = unchecked(1 - sign);
            result_f_lo = DoubleArithmetic.SubtractUnchecked(second_f_lo, second_f_hi, first_f_lo, first_f_hi, out result_f_hi);
            goto L_1;
        L_e_GT:;
            if (first_e == 0x7FFF) {
                if (0 != (first_f_hi | first_f_lo)) {
                    goto L_NaN;
                }

                result_hi = first_hi;
                return first_lo;
            }
            if (0 != second_e) {
                second_f_hi |= 0x0010000000000000u;
            } else {
                unchecked {
                    --de;
                }
                if (0 == de) {
                    goto L_A_GT;
                }
            }
            second_f_lo = Binary128Arithmetic.ShiftRightWithJamming(second_f_lo, second_f_hi, de, out second_f_hi);
        L_A_GT:;
            result_e = first_e;
            first_f_hi |= 0x0010000000000000u;
        L_GT:;
            result_f_lo = DoubleArithmetic.SubtractUnchecked(first_f_lo, first_f_hi, second_f_lo, second_f_hi, out result_f_hi);
        L_1:;
            return Binary128Arithmetic.GetBitsFromRawPartsWithNormalizationAndRounding(result_f_lo, result_f_hi, unchecked(result_e - 5), sign, rounding, out result_hi);
        L_NaN:;
            return Binary128Arithmetic.GetNaN(first_lo, first_hi, second_lo, second_hi, out result_hi);
        }

        public static UInt64 Add(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
            var second_s = Binary128Arithmetic.GetRawSignFromHi64Bits(second_hi);
            if (first_s == second_s) {
                return Binary128Arithmetic.AddAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            } else {
                return Binary128Arithmetic.SubtractAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            }
        }

        public static UInt64 Subtract(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
            var second_s = Binary128Arithmetic.GetRawSignFromHi64Bits(second_hi);
            if (first_s == second_s) {
                return Binary128Arithmetic.SubtractAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            } else {
                return Binary128Arithmetic.AddAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            }
        }

        public static UInt64 Divide(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
            var second_s = Binary128Arithmetic.GetRawSignFromHi64Bits(second_hi);
            var first_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(first_hi);
            var second_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(second_hi);
            var first_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(first_hi);
            var second_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(second_hi);
            var result_s = first_s ^ second_s;

            UInt64 result_lo_;
            UInt64 result_hi_;

            if (first_e == 0x7FFF) {
                if (0 != (first_f_hi | first_lo)) {
                    goto L_NaN;
                }

                if (second_e == 0x7FFF) {
                    if (0 != (second_f_hi | second_lo)) {
                        goto L_NaN;
                    }

                    goto L_DefaultNaN;
                }
                goto L_Infinity;
            }
            if (second_e == 0x7FFF) {
                if (0 != (second_f_hi | second_lo)) {
                    goto L_NaN;
                }

                goto L_Zero;
            }

            if (0 == second_e) {
                if (0 == (second_f_hi | second_lo)) {
                    if (0 == (unchecked((uint)first_e) | (first_f_hi | first_lo))) {
                        goto L_DefaultNaN;
                    }

                    goto L_Infinity;
                }
                second_e = NormalizeSubnormal(second_lo, second_f_hi, out second_lo, out second_f_hi);
            }
            if (0 == first_e) {
                if (0 == (first_f_hi | first_lo)) {
                    goto L_Zero;
                }

                first_e = NormalizeSubnormal(first_lo, first_f_hi, out first_lo, out first_f_hi);
            }

            var result_e = unchecked(first_e - second_e + 0x3FFE);
            first_f_hi |= Hi64BitsImplicitBit;
            second_f_hi |= Hi64BitsImplicitBit;
            var r_lo = first_lo;
            var r_hi = first_f_hi;
            if (DoubleArithmetic.LessThan(first_lo, first_f_hi, second_lo, second_f_hi)) {
                unchecked {
                    --result_e;
                }
                r_lo = DoubleArithmetic.ShiftLeft(first_lo, first_f_hi, out r_hi);
            }
            var recip32 = InvertRough(unchecked((UInt32)(second_f_hi >> 17)));
            var i = 3;
            UInt32 q;
            UInt64 p_lo;
            UInt64 p_hi;

            Span<UInt32> qs = stackalloc UInt32[4];
            for (; ; ) {
                var qL = unchecked((UInt64)(UInt32)(r_hi >> 19) * recip32);
                q = unchecked((UInt32)((qL + 0x80000000) >> 32));
                unchecked {
                    --i;
                }
                if (i < 0) {
                    break;
                }

                r_lo = ShiftLeftPartial(r_lo, r_hi, 29, out r_hi);
                p_lo = DoubleArithmetic.MultiplyUnchecked(second_lo, second_f_hi, q, 0u, out p_hi);
                r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, p_lo, p_hi, out r_hi);

                if (0 > unchecked((Int64)r_hi)) {
                    unchecked {
                        --q;
                    }
                    r_lo = DoubleArithmetic.AddUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                }
                qs[i] = q;
            }

            if (2 > (7 & unchecked(1 + q))) {
                r_lo = ShiftLeftPartial(r_lo, r_hi, 29, out r_hi);
                p_lo = DoubleArithmetic.MultiplyUnchecked(second_lo, second_f_hi, q, 0u, out p_hi);
                r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, p_lo, p_hi, out r_hi);

                if (0 > unchecked((Int64)r_hi)) {
                    unchecked {
                        --q;
                    }
                    r_lo = DoubleArithmetic.AddUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                } else if (DoubleArithmetic.LessThanOrEqual(second_lo, second_f_hi, r_lo, r_hi)) {
                    unchecked {
                        ++q;
                    }
                    r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                }
                if (0 != (r_hi | r_lo)) {
                    q |= 1;
                }
            }

            var result_f_cy = (UInt64)((UInt64)q << 60);
            p_lo = ShiftLeftPartial((UInt64)qs[1], 0u, 54, out p_hi);
            {
                var result_f_lo = DoubleArithmetic.AddUnchecked(unchecked(((UInt64)qs[0] << 25) + (q >> 4)), (UInt64)qs[2] << 19, p_lo, p_hi, out var result_f_hi);

                return GetBitsFromRawPartsWithRounding(result_f_cy, result_f_lo, result_f_hi, result_e, result_s, rounding, out result_hi);
            }

        L_Zero:;
            result_hi_ = unchecked((UInt64)result_s) << (64 - 1);
        L_1:;
            result_lo_ = 0;
            result_hi = result_hi_;
            return result_lo_;

        L_Infinity:;
            result_hi_ = 0x7FFF000000000000u | (unchecked((UInt64)result_s) << (64 - 1));
            goto L_1;

        L_DefaultNaN:;
            return GetNaN(out result_hi);

        L_NaN:;
            return GetNaN(first_lo, first_hi, second_lo, second_hi, out result_hi);
        }

        public static UInt64 Multiply(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(first_hi);
            var first_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(first_hi);
            var second_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(second_hi);
            var second_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(second_hi);
            var result_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi ^ second_hi);

            UInt64 is_valid; // neither +-0 * +-inf nor +-inf * +-0
            if (first_e == 0x7FFF) {
                if (0 != (first_f_hi | first_lo) || ((second_e == 0x7FFF) && 0 != (second_f_hi | second_lo))) {
                    goto L_NaN;
                }
                is_valid = unchecked((uint)second_e) | second_f_hi | second_lo;
                goto L_Infinity;
            }
            if (second_e == 0x7FFF) {
                if (0 != (second_f_hi | second_lo)) {
                    goto L_NaN;
                }

                is_valid = unchecked((uint)first_e) | first_f_hi | first_lo;
                goto L_Infinity;
            }

            if (0 == first_e) {
                if (0 == (first_f_hi | first_lo)) {
                    goto L_Zero;
                }

                first_e = NormalizeSubnormal(first_lo, first_f_hi, out first_lo, out first_f_hi);
            }
            if (0 == second_e) {
                if (0 == (second_f_hi | second_lo)) {
                    goto L_Zero;
                }

                second_e = NormalizeSubnormal(second_lo, second_f_hi, out second_lo, out second_f_hi);
            }

            var result_e = unchecked(first_e + second_e - 0x4000);
            first_f_hi |= (UInt64)0x0001000000000000;
            second_lo = ShiftLeftPartial(second_lo, second_f_hi, 16, out second_f_hi);

            var result_f_lo_lo = DoubleArithmetic.BigMul(first_lo, first_f_hi, second_lo, second_f_hi, out var result_f_lo_hi, out var result_f_hi_lo, out var result_f_hi_hi);

            var result_f_cy = result_f_lo_hi | (result_f_lo_lo != 0).AsUIntegerUnsafe();
            var result_f_lo = DoubleArithmetic.AddUnchecked(result_f_hi_lo, result_f_hi_hi, first_lo, first_f_hi, out var result_f_hi);
            if ((UInt64)0x0002000000000000 <= result_f_hi) {
                ++result_e;
                result_f_cy = ShiftRightWithJammingPartial(result_f_cy, result_f_lo, result_f_hi, 1, out result_f_lo, out result_f_hi);
            }
            return GetBitsFromRawPartsWithRounding(result_f_cy, result_f_lo, result_f_hi, result_e, result_s, rounding, out result_hi);
        L_Zero:;
            result_hi = GetHi64BitsFromRawParts(result_s, 0, 0);
            return 0;
        L_Infinity:;
            if (0 == is_valid) {
                // invalid
                return GetNaN(out result_hi);
            }
            result_hi = GetHi64BitsFromRawParts(result_s, 0x7FFF, 0);
            return 0;
        L_NaN:;
            return GetNaN(first_lo, first_hi, second_lo, second_hi, out result_hi);
        }

        public static UInt64 Sqrt(UInt64 value_lo, UInt64 value_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            unchecked {
                var value_s = GetRawSignFromHi64Bits(value_hi);
                var value_e = GetRawExponentFromHi64Bits(value_hi);
                var value_f_hi = GetRawFractionHiFromHi64Bits(value_hi);

                if (value_e == 0x7FFF) {
                    if (0 != (value_f_hi | value_lo)) {
                        return GetNaN(value_lo, value_hi, out result_hi);
                    }
                    if (0 == value_s) {
                        return (result_hi = value_hi).Comma(value_lo);
                    }

                    goto L_NaN;
                }

                if (0 != value_s) {
                    if (0 == ((uint)value_e | value_f_hi | value_lo)) {
                        return (result_hi = value_hi).Comma(value_lo);
                    };
                    goto L_NaN;
                }

                if (0 == value_e) {
                    if (0 == (value_f_hi | value_lo)) {
                        return (result_hi = value_hi).Comma(value_lo);
                    }

                    value_e = NormalizeSubnormal(value_lo, value_f_hi, out value_lo, out value_f_hi);
                }

                var result_e = ((value_e - 0x3FFF) >> 1) + 0x3FFE;
                value_e &= 1;
                value_f_hi |= 0x0001000000000000u;

                var significand32 = (UInt32)(value_f_hi >> 17);
                var rSqrt32 = ReciprocalSqrt(value_e, significand32);
                var result_significand32 = (UInt32)(((UInt64)significand32 * rSqrt32) >> 32);
                var nz = (0 != value_e).AsIntegerUnsafe();

                result_significand32 >>= nz;
                var remainder_lo = ShiftLeftPartial(value_lo, value_f_hi, 13 - nz, out var remainder_hi);

                var r_2 = result_significand32;
                remainder_hi -= (UInt64)result_significand32 * result_significand32;

                var r = (UInt32)(((UInt32)(remainder_hi >> 2) * (UInt64)rSqrt32) >> 32);
                var result_significand32shifted = (UInt64)result_significand32 << 32;
                var result_significand64 = result_significand32shifted + ((UInt64)r << 3);

                var t_lo = ShiftLeftPartial(remainder_lo, remainder_hi, 29, out var t_hi);
                UInt64 s_lo;
                UInt64 s_hi;

                for (; ; ) {
                    s_lo = MultiplyThenShiftLeftBy32(result_significand32shifted + result_significand64, r, out s_hi);
                    remainder_lo = DoubleArithmetic.SubtractUnchecked(t_lo, t_hi, s_lo, s_hi, out remainder_hi);
                    if (/* LIKELY */0 == (remainder_hi & 0x8000000000000000u)) {
                        break;
                    }

                    --r;
                    result_significand64 -= 1 << 3;
                }
                var r_1 = r;

                r = (UInt32)(((remainder_hi >> 2) * rSqrt32) >> 32);

                t_lo = ShiftLeftPartial(remainder_lo, remainder_hi, 29, out t_hi);

                result_significand64 <<= 1;

                for (; ; ) {
                    s_lo = ShiftLeftPartial(result_significand64, 0, 32, out s_hi);
                    s_lo = DoubleArithmetic.AddUnchecked(s_lo, s_hi, (UInt64)r << 6, 0, out s_hi);
                    s_lo = MultiplyUnchecked(s_lo, s_hi, r, out s_hi);
                    remainder_lo = DoubleArithmetic.SubtractUnchecked(t_lo, t_hi, s_lo, s_hi, out remainder_hi);
                    if (/* LIKELY */0 == (remainder_hi & 0x8000000000000000u)) {
                        break;
                    }

                    --r;
                }
                var r_0 = r;

                r = (UInt32)((((remainder_hi >> 2) * rSqrt32) >> 32) + 2);
                var result_f_cy = (UInt64)r << 59;
                s_lo = ShiftLeftPartial(r_1, 0, 53, out s_hi);
                var result_f_lo = DoubleArithmetic.AddUnchecked(
                    ((UInt64)r_0 << 24) + (r >> 5), (UInt64)r_2 << 18,
                    s_lo, s_hi,
                    out var result_f_hi);

                if ((r & 0xF) <= 2) {
                    r &= ~3u;
                    result_f_cy = (UInt64)r << 59;
                    t_lo = ShiftLeftPartial(result_f_lo, result_f_hi, 6, out t_hi);
                    t_lo |= result_f_cy >> 58;
                    s_lo = DoubleArithmetic.SubtractUnchecked(t_lo, t_hi, r, 0, out s_hi);
                    t_lo = MultiplyThenShiftLeftBy32(s_lo, r, out t_hi);
                    s_lo = MultiplyThenShiftLeftBy32(s_hi, r, out s_hi);
                    s_lo = DoubleArithmetic.AddUnchecked(s_lo, s_hi, t_hi, 0, out s_hi);
                    remainder_lo = ShiftLeftPartial(remainder_lo, remainder_hi, 20, out remainder_hi);
                    s_lo = DoubleArithmetic.SubtractUnchecked(s_lo, s_hi, remainder_lo, remainder_hi, out s_hi);
                    // s_hi @ s_lo @ t_lo : negative remainder
                    if (0 > (Int64)s_hi) {
                        result_f_cy |= 1;
                    } else {
                        if (0 != (s_hi | s_lo | t_lo)) {
                            if (0 != result_f_cy) {
                                --result_f_cy;
                            } else {
                                result_f_lo = DoubleArithmetic.DecreaseUnchecked(result_f_lo, result_f_hi, out result_f_hi);
                                result_f_cy = ~(UInt64)0;
                            }
                        }
                    }
                }
                return GetBitsFromRawPartsWithRounding(result_f_cy, result_f_lo, result_f_hi, result_e, 0, rounding, out result_hi);
            L_NaN:
                return GetNaN(out result_hi);
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 Remainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, bool quotientTowardZero, out UInt64 result_hi) {
            unchecked {
                var first_f_lo = first_lo;
                var first_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(first_hi);
                var first_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(first_hi);
                var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
                var second_f_hi = Binary128Arithmetic.GetRawFractionHiFromHi64Bits(second_hi);
                var second_e = Binary128Arithmetic.GetRawExponentFromHi64Bits(second_hi);

                if (first_e == 0x7FFF) {
                    if (0 != (first_f_hi | first_f_lo) || ((second_e == 0x7FFF) && 0 != (second_f_hi | second_lo))) {
                        goto L_NaN;
                    }
                    goto L_Invalid;
                }
                if (second_e == 0x7FFF) {
                    if (0 != (second_f_hi | second_lo)) {
                        goto L_NaN;
                    }

                    result_hi = first_hi;
                    return first_lo;
                }

                if (0 == second_e) {
                    if (0 == (second_f_hi | second_lo)) {
                        goto L_Invalid;
                    }

                    second_e = NormalizeSubnormal(second_lo, second_f_hi, out second_lo, out second_f_hi);
                }
                if (0 == first_e) {
                    if (0 == (first_f_hi | first_f_lo)) {
                        result_hi = first_hi;
                        return first_lo;
                    }
                    first_e = NormalizeSubnormal(first_f_lo, first_f_hi, out first_f_lo, out first_f_hi);
                }

                UInt32 q;
                first_f_hi |= (UInt64)(0x0001000000000000);
                second_f_hi |= (UInt64)(0x0001000000000000);
                var r_lo = first_f_lo;
                var r_hi = first_f_hi;
                UInt64 r1_lo;
                UInt64 r1_hi;
                var d = first_e - second_e;
                if (d < 1) {
                    if (d < -1) {
                        result_hi = first_hi;
                        return first_lo;
                    }
                    q = 0;
                    if (0 != d) {
                        --second_e;
                        second_lo = DoubleArithmetic.AddUnchecked(second_lo, second_f_hi, second_lo, second_f_hi, out second_f_hi);
                    } else {
                        if (DoubleArithmetic.LessThanOrEqual(second_lo, second_f_hi, r_lo, r_hi)) {
                            q = 1;
                            r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                        }
                    }
                } else {
                    var recip32 = InvertRough((UInt32)(second_f_hi >> 17));
                    d -= 30;
                    UInt64 p_lo;
                    UInt64 p_hi;
                    UInt64 q64;
                    for (; ; ) {
                        q64 = (UInt64)(UInt32)(r_hi >> 19) * recip32;
                        if (d < 0) {
                            break;
                        }

                        q = (UInt32)((q64 + 0x80000000) >> 32);
                        r_lo = ShiftLeftPartial(r_lo, r_hi, 29, out r_hi);
                        p_lo = MultiplyUnchecked(second_lo, second_f_hi, q, out p_hi);
                        r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, p_lo, p_hi, out r_hi);
                        if (0 != (r_hi & (UInt64)(0x8000000000000000))) {
                            r_lo = DoubleArithmetic.AddUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                        }
                        d -= 29;
                    }
                    // -29 <= d
                    q = (UInt32)(q64 >> 32) >> (~d & 31);
                    r_lo = ShiftLeftPartial(r_lo, r_hi, 30 + d, out r_hi);
                    p_lo = MultiplyUnchecked(second_lo, second_f_hi, q, out p_hi);
                    r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, p_lo, p_hi, out r_hi);
                    if (0 != (r_hi & (UInt64)(0x8000000000000000))) {
                        r1_lo = DoubleArithmetic.AddUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r1_hi);
                        goto L_1;
                    }
                }

                do {
                    r1_lo = r_lo;
                    r1_hi = r_hi;
                    ++q;
                    r_lo = DoubleArithmetic.SubtractUnchecked(r_lo, r_hi, second_lo, second_f_hi, out r_hi);
                } while (0 == (r_hi & (UInt64)(0x8000000000000000)));
            L_1:;
                UInt64 r0_lo;
                if (quotientTowardZero || 0 != ((r0_lo = DoubleArithmetic.AddUnchecked(r_lo, r_hi, r1_lo, r1_hi, out var r0_hi)).Comma(r0_hi) & (UInt64)(0x8000000000000000)) || (0 == (r0_hi | r0_lo) && 0 != (q & 1))) {
                    r_lo = r1_lo;
                    r_hi = r1_hi;
                }
                var result_s = first_s;
                if (0 != (r_hi & (UInt64)(0x8000000000000000))) {
                    result_s = 1 - result_s;
                    r_lo = DoubleArithmetic.NegateUnchecked(r_lo, r_hi, out r_hi);
                }
                return GetBitsFromRawPartsWithNormalizationAndRounding(r_lo, r_hi, second_e - 1, result_s, rounding, out result_hi);

            L_NaN:;
                return GetNaN(first_f_lo, first_hi, second_lo, second_hi, out result_hi);
            L_Invalid:;
                return GetNaN(out result_hi);
            }
        }

        public static UInt64 IEEERemainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            return Remainder(first_lo, first_hi, second_lo, second_hi, rounding, false, out result_hi);
        }

        public static UInt64 Remainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            return Remainder(first_lo, first_hi, second_lo, second_hi, rounding, true, out result_hi);
        }

        static readonly UInt16[] Reciprocal32Table0 = new UInt16[] {
            0xFFC4, 0xF0BE, 0xE363, 0xD76F, 0xCCAD, 0xC2F0, 0xBA16, 0xB201,
            0xAA97, 0xA3C6, 0x9D7A, 0x97A6, 0x923C, 0x8D32, 0x887E, 0x8417
        };

        static readonly UInt16[] Reciprocal32Table1 = new UInt16[] {
            0xF0F1, 0xD62C, 0xBFA1, 0xAC77, 0x9C0A, 0x8DDB, 0x8185, 0x76BA,
            0x6D3B, 0x64D4, 0x5D5C, 0x56B1, 0x50B6, 0x4B55, 0x4679, 0x4211
        };

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static UInt32 Stub_Invert0001(UInt32 value) {
            unchecked {
                var index = (value >> 27) & 0xF;
                var r0 = (UInt16)(Reciprocal32Table0[index] - ((Reciprocal32Table1[index] * (UInt32)(UInt16)(value >> 11)) >> 20));
                var sigma0 = ~(UInt32)(((UInt32)r0 * (UInt64)value) >> 7);
                var r = ((UInt32)r0 << 16) + (UInt32)((r0 * (UInt64)sigma0) >> 24);
                var sigma0Squared = ((UInt64)sigma0 * sigma0) >> 32;
                r += (UInt32)(((UInt32)r * (UInt64)sigma0Squared) >> 48);
                return r;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static UInt64 MultiplyUnchecked(UInt64 first_lo, UInt64 first_hi, UInt32 second, out UInt64 result_hi) {
            unchecked {
                var lo = first_lo * second;
                var mid = (UInt64)(UInt32)(first_lo >> 32) * second;
                var carry = (UInt32)(lo >> 32) - (UInt32)mid;
                result_hi = first_hi * second + (UInt32)((mid + carry) >> 32);
                return lo;
            }
        }

        static readonly UInt16[] ReciprocalSqrt32Table0 = new UInt16[] {
            0xB4C9, 0xFFAB, 0xAA7D, 0xF11C, 0xA1C5, 0xE4C7, 0x9A43, 0xDA29,
            0x93B5, 0xD0E5, 0x8DED, 0xC8B7, 0x88C6, 0xC16D, 0x8424, 0xBAE1
        };

        static readonly UInt16[] ReciprocalSqrt32Table1 = new UInt16[] {
            0xA5A5, 0xEA42, 0x8C21, 0xC62D, 0x788F, 0xAA7F, 0x6928, 0x94B6,
            0x5CC7, 0x8335, 0x52A6, 0x74E2, 0x4A3E, 0x68FE, 0x432B, 0x5EFD
        };

        private static UInt32 ReciprocalSqrt(int oddExponent, UInt32 value) {
            unchecked {
                var index = ((value >> 27) & 0xE) + oddExponent;
                var r0 = (UInt16)(ReciprocalSqrt32Table0[index] - ((ReciprocalSqrt32Table1[index] * (UInt32)(UInt16)(value >> 12)) >> 20));
                var r0Squared = (UInt32)r0 * r0;
                if (0 == oddExponent) {
                    r0Squared <<= 1;
                }

                var sigma0 = ~(UInt32)(((UInt32)r0Squared * (UInt64)value) >> 23);
                var r = ((UInt32)r0 << 16) + (UInt32)((r0 * (UInt64)sigma0) >> 25);
                var sigma0Squared = ((UInt64)sigma0 * sigma0) >> 32;
                r += (UInt32)(((UInt32)((r >> 1) + (r >> 3) - ((UInt32)r0 << 14)) * (UInt64)sigma0Squared) >> 48);
                if (0 <= (Int32)r) {
                    r = 0x80000000;
                }

                return r;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static UInt64 MultiplyThenShiftLeftBy32(UInt64 first, UInt32 second_hi, out ulong result_hi) {
            unchecked {
                var mid = (UInt64)(UInt32)first * second_hi;
                result_hi = (UInt64)(UInt32)(first >> 32) * second_hi + (mid >> 32);
                return mid << 32;
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static int NormalizeSubnormal(UInt64 value_lo, UInt64 value_hi, out UInt64 result_lo, out UInt64 result_hi) {
            unchecked {
                if (0 == value_hi) {
                    var count = BinaryNumerals.CountLeadingZeros(value_lo) - 15;
                    if (count < 0) {
                        result_hi = value_lo >> -count;
                        result_lo = value_lo << (/*64 & */count);
                    } else {
                        result_hi = value_lo << count;
                        result_lo = 0;
                    }
                    return -63 - count;
                } else {
                    var count = BinaryNumerals.CountLeadingZeros(value_hi) - 15;
                    result_lo = ShiftLeftPartial(value_lo, value_hi, count, out result_hi);
                    return 1 - count;
                }
            }
        }

        public static UInt64 FusedMultiplyAdd(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, UInt64 addend_lo, UInt64 addend_hi, FloatingPointRounding rounding, out UInt64 result_hi) {
            throw new NotImplementedException();
        }

        const UInt64 Binary64_MaxValue_Binary128_Hi64Bits = 0X43feffffffffffffU;

        const UInt64 Binary64_MaxValue_Binary128_Lo64Bits = 0Xf000000000000000U;

        internal static UInt32 InvertRough(UInt32 value) {
            const double d = (double)0x8000000000000000u;
            // const double d = (double)0x7FFFFFFFFFFFFE00u;
            unchecked {
                var t = (UInt32)(d / value);
                --t;
                return t;
            }
        }

        internal static int NormalizeSubnormal(UInt64 fraction_lo, UInt64 fraction_hi, int exponent, out UInt64 result_fraction_lo, out UInt64 result_fraction_hi) {
            var first_lzc = BinaryNumerals.CountLeadingZeros(fraction_hi);
            if (64 == first_lzc) {
                unchecked {
                    first_lzc += BinaryNumerals.CountLeadingZeros(fraction_lo);
                }
            }
            unchecked {
                exponent += (FractionBitCount - 64) - first_lzc;
            }
            result_fraction_lo = DoubleArithmetic.ShiftLeft(fraction_lo, fraction_hi, unchecked((128 - FractionBitCount) + 1 + first_lzc), out result_fraction_hi);
            return exponent;
        }


        public static UInt64 ScaleB(UInt64 x_lo, UInt64 x_hi, int n, FloatingPointRounding rounding, out UInt64 result_hi) {
            var lx = x_lo;
            var hx = x_hi;
            var k = Binary128Arithmetic.GetRawExponentFromHi64Bits(hx);
            if (0x7fff == k) {
                // Infinity, NaN
                result_hi = hx;
                return lx;
            }
            var f_hi = GetRawFractionHiFromHi64Bits(hx);

            if (k == 0) {
                // Subnormal, Zero
                if (0 == ((0x7fffffffffffffffu & hx) | lx)) {
                    // Zero
                    result_hi = hx;
                    return lx;
                }
                k = Binary128Arithmetic.NormalizeSubnormal(lx, f_hi, k, out lx, out f_hi);
                f_hi &= 0x0001000000000000u;
            }
            var s = GetRawSignFromHi64Bits(hx);
            unchecked {
                k += n;
            }
            if (n < -900630 || k < -128) {
                k = -128;
            } else if (n > 910305 || k > 0x7fff) {
                k = 0x7fff;
            }
            return Binary128Arithmetic.GetBitsFromRawPartsWithRounding(0, lx, f_hi, k, s, rounding: rounding, out result_hi);
        }

        public static UInt64 ModF(UInt64 x_lo, UInt64 x_hi, int n, FloatingPointRounding rounding, out UInt64 result_hi) {
            var lx = x_lo;
            var hx = x_hi;
            var k = Binary128Arithmetic.GetRawExponentFromHi64Bits(hx);
            if (0x7fff == k) {
                // Infinity, NaN
                result_hi = hx;
                return lx;
            }
            var f_hi = GetRawFractionHiFromHi64Bits(hx);

            if (k == 0) {
                // Subnormal, Zero
                if (0 == ((0x7fffffffffffffffu & hx) | lx)) {
                    // Zero
                    result_hi = hx;
                    return lx;
                }
                k = Binary128Arithmetic.NormalizeSubnormal(lx, f_hi, k, out lx, out f_hi);
                f_hi &= 0x0001000000000000u;
            }
            var s = GetRawSignFromHi64Bits(hx);
            unchecked {
                k += n;
            }
            if (n < -900630 || k < -128) {
                k = -128;
            } else if (n > 910305 || k > 0x7fff) {
                k = 0x7fff;
            }
            return Binary128Arithmetic.GetBitsFromRawPartsWithRounding(0, lx, f_hi, k, s, rounding: rounding, out result_hi);
        }
    }


}
