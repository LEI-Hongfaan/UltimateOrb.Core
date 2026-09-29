using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UltimateOrb.Mathematics;
using UltimateOrb.Utilities.Extensions;
using static UltimateOrb.Utilities.BooleanIntegerModule;
using static UltimateOrb.Utilities.ThrowHelper;

namespace UltimateOrb.Numerics {

#if NET8_0_OR_GREATER
    [Experimental("UoWIP")]
#endif
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

        internal static UInt64 GetNaN(UInt64 lo, UInt64 hi, out UInt64 result_hi) {
            // Quiet bit (bit 111)
            const UInt64 QuietBit = 0x0000800000000000u;

            // Preserve sign + payload, force quiet
            result_hi = hi | QuietBit;
            return lo;
        }

        internal static UInt64 GetNaN(
            UInt64 first_lo, UInt64 first_hi,
            UInt64 second_lo, UInt64 second_hi,
            out UInt64 result_hi) {

            const UInt64 ExpMask = 0x7FFF000000000000u;
            const UInt64 QuietBit = 0x0000800000000000u;
            const UInt64 FracMask = 0x0000FFFFFFFFFFFFu;

            bool first_is_nan =
                ((first_hi & ExpMask) == ExpMask) &&
                ((first_hi & FracMask) != 0 || first_lo != 0);

            bool second_is_nan =
                ((second_hi & ExpMask) == ExpMask) &&
                ((second_hi & FracMask) != 0 || second_lo != 0);

            Debug.Assert(first_is_nan || second_is_nan);

            bool first_is_snan = first_is_nan && ((first_hi & QuietBit) == 0);
            bool second_is_snan = second_is_nan && ((second_hi & QuietBit) == 0);

            // 1. signaling NaN has priority
            if (first_is_snan) {
                result_hi = first_hi | QuietBit;
                return first_lo;
            }
            if (second_is_snan) {
                result_hi = second_hi | QuietBit;
                return second_lo;
            }

            // 2. quiet NaN: prefer first
            if (first_is_nan) {
                result_hi = first_hi | QuietBit;
                return first_lo;
            }

            // 3. otherwise second must be quiet NaN
            result_hi = second_hi | QuietBit;
            return second_lo;
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt128 GetNaN() {
            // negative canonical quiet NaN
            UInt128 nan = ((UInt128)0xFFFF800000000000u << 64);

            Debug.Assert(
                ((nan >> 112) & 0x7FFFu) == 0x7FFFu &&   // exponent = all ones
                ((nan >> 111) & 1) == 1               // quiet bit = 1
            );

            return nan;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt128 GetNaN(UInt128 bits) {
            UInt128 ExpMask = (UInt128)0x7FFFu << 112;
            UInt128 QuietBit = (UInt128)1 << 111;
            UInt128 FracMask = ((UInt128)1 << 112) - 1;

            // Must be NaN: exponent all ones AND fraction != 0
            Debug.Assert(((bits & ExpMask) == ExpMask) && ((bits & FracMask) != 0));

            // Preserve sign + payload, only force quiet bit
            UInt128 result = bits | QuietBit;

            Debug.Assert(((result & QuietBit) != 0));   // quiet

            return result;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt128 GetNaN(UInt128 first, UInt128 second) {
            UInt128 ExpMask = (UInt128)0x7FFFu << 112;
            UInt128 QuietBit = (UInt128)1 << 111;
            UInt128 FracMask = ((UInt128)1 << 112) - 1;

            bool first_is_nan =
                ((first & ExpMask) == ExpMask) &&
                ((first & FracMask) != 0);

            bool second_is_nan =
                ((second & ExpMask) == ExpMask) &&
                ((second & FracMask) != 0);

            // At least one must be NaN
            Debug.Assert(first_is_nan || second_is_nan);

            bool first_is_snan = first_is_nan && ((first & QuietBit) == 0);
            bool second_is_snan = second_is_nan && ((second & QuietBit) == 0);

            UInt128 chosen;

            // 1. signaling NaN has priority
            if (first_is_snan)
                chosen = first;
            else if (second_is_snan)
                chosen = second;

            // 2. quiet NaN: prefer first
            else if (first_is_nan)
                chosen = first;

            // 3. otherwise second must be quiet NaN
            else
                chosen = second;

            // Quiet the chosen NaN (sign + payload preserved)
            UInt128 result = chosen | QuietBit;

            Debug.Assert(((result & QuietBit) != 0));   // quiet

            return result;
        }


        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetBitsFromRawPartsWithRounding(UInt64 fraction_cy, UInt64 fraction_lo, UInt64 fraction_hi, int exponent, int sign, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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
                DoubleArithmetic.BigDivNoThrowWhenOverflow
                var l = DoubleArithmetic.BigSquare(old_hi, out h);
                l += lo;
                h += radicand;
                if (l < lo) {
                    ++h;
                }
                var @new = MathEx.BigDivNoThrowWhenOverflow(l, h, old) >> 1;
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
        public static UInt64 Round(UInt64 value_lo, UInt64 value_hi, [ConstantExpected] FloatingPointRounding roundingMode, out UInt64 result_hi) {
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

        public static UInt64 Truncate(UInt64 lo, UInt64 hi, out UInt64 result_hi) {
            unchecked {
                var s = GetRawSignFromHi64Bits(hi);
                var e = GetRawExponentFromHi64Bits(hi);
                var f_hi = GetRawFractionHiFromHi64Bits(hi);

                if (e == 0x7FFF) {
                    // NaN / Inf
                    if (0 != (f_hi | lo)) {
                        return GetNaN(lo, hi, out result_hi);   // quiet NaN
                    }
                    result_hi = hi;                             // ±Inf unchanged
                    return lo;
                }

                if (e < 0x3FFF) {
                    // |value| < 1 — includes ±0, subnormals, and normalized values in (-1, 1).
                    // Truncate toward zero => ±0.0 with the original sign.
                    result_hi = (UInt64)s << (64 - 1);
                    return 0;
                }

                if (e >= 0x406F) {
                    // |value| >= 2^112 — every bit of the 113-bit significand sits at or
                    // above the 1's place; nothing to clear.
                    result_hi = hi;
                    return lo;
                }

                // 0x3FFF <= e <= 0x406E.  Clear the low (0x406F - e) bits of the
                // significand, where bit 112 is the implicit bit and bits 0..111
                // are the fraction (lo = 0..63, f_hi = 64..111).
                int n = 0x406F - e;                              // 1 .. 112
                if (n < 64) {
                    lo &= ~((1UL << n) - 1);
                } else {
                    lo = 0;
                    int nh = n - 64;                             // 0 .. 48
                    f_hi &= ~((1UL << nh) - 1);
                }

                result_hi = GetHi64BitsFromRawParts(s, e, f_hi);
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
        static UInt64 GetBitsFromRawPartsWithNormalizationAndRounding(UInt64 fraction_lo, UInt64 fraction_hi, int exponent, int sign, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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

        // Returns the low 64 bits; sets result_hi to the high 64 bits.
        // Positive infinity for binary128: sign = 0, exponent = all 1s (15 bits), fraction = 0.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetPositiveInfinity(out UInt64 result_hi) {
            // Exponent all ones (15 bits) placed at high-word bits 48..62
            // result_hi = exponent << 48
            result_hi = 0x7FFF000000000000UL;
            return 0u; // low 64 bits (fraction low) = 0
        }

        // Negative infinity: same as positive infinity but with sign bit set in the high word.
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetNegativeInfinity(out UInt64 result_hi) {
            result_hi = 0xFFFF000000000000UL;
            return 0u;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UInt64 GetNaN(out UInt64 result_hi) {
            result_hi = 0xFFFF800000000000u;   // negative canonical quiet NaN
            return 0;
        }

        public static int Compare(
            UInt64 first_lo, UInt64 first_hi,
            UInt64 second_lo, UInt64 second_hi) {
            // NaN: exponent all ones (15 bits) AND mantissa nonzero
            bool isNaN1 = ((first_hi >> 48) & 0x7FFFu) == 0x7FFFu
                       && ((first_hi & 0x0000_FFFF_FFFF_FFFFUL) != 0UL || first_lo != 0UL);
            bool isNaN2 = ((second_hi >> 48) & 0x7FFFu) == 0x7FFFu
                       && ((second_hi & 0x0000_FFFF_FFFF_FFFFUL) != 0UL || second_lo != 0UL);

            if (isNaN1) return isNaN2 ? 0 : -1;
            if (isNaN2) return 1;

            // Sign bits (bit 127 = bit 63 of hi)
            bool neg1 = (first_hi & 0x8000_0000_0000_0000UL) != 0;
            bool neg2 = (second_hi & 0x8000_0000_0000_0000UL) != 0;

            // Magnitudes: clear the sign bit
            UInt64 mag1_hi = first_hi & 0x7FFF_FFFF_FFFF_FFFFUL;
            UInt64 mag2_hi = second_hi & 0x7FFF_FFFF_FFFF_FFFFUL;

            // Compare magnitudes as unsigned 128-bit integers
            int cmp = mag1_hi.CompareTo(mag2_hi);
            if (cmp == 0) cmp = first_lo.CompareTo(second_lo);

            if (cmp == 0) {
                // Equal magnitude
                if (mag1_hi == 0 && first_lo == 0) return 0; // both are zero -> -0 == +0
                if (neg1 == neg2) return 0;                  // same sign, equal non-zero
                return neg1 ? -1 : 1;                        // different signs, non-zero: negative < positive
            } else {
                // Different magnitudes
                if (neg1 == neg2) {
                    // Same sign: larger magnitude means more negative if negative, more positive if positive
                    return neg1 ? -cmp : cmp;
                } else {
                    // Different signs: negative is always less than positive
                    return neg1 ? -1 : 1;
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        internal static UInt64 AddAbsoluteValuesThenCopySign(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, int sign, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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
        internal static UInt64 SubtractAbsoluteValuesThenCopySign(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, int sign, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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

        public static UInt64 Add(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
            var second_s = Binary128Arithmetic.GetRawSignFromHi64Bits(second_hi);
            if (first_s == second_s) {
                return Binary128Arithmetic.AddAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            } else {
                return Binary128Arithmetic.SubtractAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            }
        }

        public static UInt64 Subtract(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
            var first_s = Binary128Arithmetic.GetRawSignFromHi64Bits(first_hi);
            var second_s = Binary128Arithmetic.GetRawSignFromHi64Bits(second_hi);
            if (first_s == second_s) {
                return Binary128Arithmetic.SubtractAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            } else {
                return Binary128Arithmetic.AddAbsoluteValuesThenCopySign(first_lo, first_hi, second_lo, second_hi, first_s, rounding, out result_hi);
            }
        }

        public static UInt64 Divide(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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

        public static UInt64 Multiply(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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

        public static UInt64 Sqrt(UInt64 value_lo, UInt64 value_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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
        internal static UInt64 Remainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, bool quotientTowardZero, out UInt64 result_hi) {
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

        public static UInt64 IEEERemainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
            return Remainder(first_lo, first_hi, second_lo, second_hi, rounding, false, out result_hi);
        }

        public static UInt64 Remainder(UInt64 first_lo, UInt64 first_hi, UInt64 second_lo, UInt64 second_hi, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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


        // ====================================================================
        // 256-bit helpers used by FusedMultiplyAddOrSubtract.
        // Word order: z0 = MSW, z3 = LSW.
        // ====================================================================

        /// <summary>
        /// 128x128 -> 256 bit unsigned multiplication.
        /// (<paramref name="a_hi"/>:<paramref name="a_lo"/>) * (<paramref name="b_hi"/>:<paramref name="b_lo"/>).
        /// </summary>
        private static void BigMul(
            out UInt64 z0, out UInt64 z1, out UInt64 z2, out UInt64 z3,
            UInt64 a_hi, UInt64 a_lo, UInt64 b_hi, UInt64 b_lo) {
            unchecked {
                UInt128 p0 = (UInt128)a_lo * b_lo;
                UInt128 p1 = (UInt128)a_lo * b_hi;
                UInt128 p2 = (UInt128)a_hi * b_lo;
                UInt128 p3 = (UInt128)a_hi * b_hi;

                UInt64 p0l = (UInt64)p0, p0h = (UInt64)(p0 >> 64);
                UInt64 p1l = (UInt64)p1, p1h = (UInt64)(p1 >> 64);
                UInt64 p2l = (UInt64)p2, p2h = (UInt64)(p2 >> 64);
                UInt64 p3l = (UInt64)p3, p3h = (UInt64)(p3 >> 64);

                z3 = p0l;

                UInt64 s = p0h + p1l;
                UInt64 c1 = s < p0h ? 1UL : 0UL;
                UInt64 s2 = s + p2l;
                UInt64 c2 = s2 < s ? 1UL : 0UL;
                z2 = s2;
                UInt64 carryA = c1 + c2;

                s = p3l + p1h;
                c1 = s < p3l ? 1UL : 0UL;
                s2 = s + p2h;
                c2 = s2 < s ? 1UL : 0UL;
                UInt64 s3 = s2 + carryA;
                UInt64 c3 = s3 < s2 ? 1UL : 0UL;
                z1 = s3;
                UInt64 carryB = c1 + c2 + c3;

                z0 = p3h + carryB;
            }
        }

        /// <summary>256-bit addition, result truncated to 256 bits.</summary>
        private static void AddUnchecked(
            out UInt64 z0, out UInt64 z1, out UInt64 z2, out UInt64 z3,
            UInt64 a0, UInt64 a1, UInt64 a2, UInt64 a3,
            UInt64 b0, UInt64 b1, UInt64 b2, UInt64 b3) {
            unchecked {
                UInt64 s = a3 + b3;
                UInt64 c = s < a3 ? 1UL : 0UL;
                z3 = s;

                UInt64 t = a2 + b2;
                UInt64 ca = t < a2 ? 1UL : 0UL;
                UInt64 t2 = t + c;
                UInt64 cb = t2 < t ? 1UL : 0UL;
                z2 = t2;
                c = ca + cb;

                t = a1 + b1;
                ca = t < a1 ? 1UL : 0UL;
                t2 = t + c;
                cb = t2 < t ? 1UL : 0UL;
                z1 = t2;
                c = ca + cb;

                z0 = a0 + b0 + c;
            }
        }

        /// <summary>256-bit subtraction, result truncated to 256 bits.</summary>
        private static void SubtractUnchecked(
            out UInt64 z0, out UInt64 z1, out UInt64 z2, out UInt64 z3,
            UInt64 a0, UInt64 a1, UInt64 a2, UInt64 a3,
            UInt64 b0, UInt64 b1, UInt64 b2, UInt64 b3) {
            unchecked {
                UInt64 t1 = a3 - b3;
                UInt64 borrow = a3 < b3 ? 1UL : 0UL;
                z3 = t1;

                t1 = a2 - b2;
                UInt64 bb = a2 < b2 ? 1UL : 0UL;
                UInt64 t2 = t1 - borrow;
                UInt64 bc = t1 < borrow ? 1UL : 0UL;
                z2 = t2;
                borrow = bb | bc;

                t1 = a1 - b1;
                bb = a1 < b1 ? 1UL : 0UL;
                t2 = t1 - borrow;
                bc = t1 < borrow ? 1UL : 0UL;
                z1 = t2;
                borrow = bb | bc;

                t1 = a0 - b0;
                z0 = t1 - borrow;
            }
        }

        /// <summary>
        /// 128-bit logical right shift. <paramref name="dist"/> must be in [0, 64).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ShiftRightUnsignedPartial(
            out UInt64 r_lo, out UInt64 r_hi, UInt64 a_lo,
            UInt64 a_hi, int dist) {
            unchecked {
                if (dist == 0) { r_hi = a_hi; r_lo = a_lo; return; }
                r_hi = a_hi >> dist;
                r_lo = (a_hi << (64 - dist)) | (a_lo >> dist);
            }
        }

        /// <summary>
        /// 256-bit right shift with "jam": the bit OR of all discarded low bits
        /// is jammed into bit 0 of the result. Word order matches (<c>a0</c> MSW … <c>a3</c> LSW).
        /// </summary>
        private static void ShiftRightUnsignedJamming(
            UInt64 a0, UInt64 a1, UInt64 a2, UInt64 a3,
            int dist,
            out UInt64 z0, out UInt64 z1, out UInt64 z2, out UInt64 z3) {
            unchecked {
                if (dist == 0) {
                    z0 = a0; z1 = a1; z2 = a2; z3 = a3;
                    return;
                }
                if (dist >= 256) {
                    z0 = z1 = z2 = z3 = 0;
                    if ((a0 | a1 | a2 | a3) != 0) z3 = 1;
                    return;
                }

                int wordDist = dist >> 6;
                int bitDist = dist & 63;

                // Initial word-level alignment.
                UInt64 r0, r1, r2, r3;
                switch (wordDist) {
                case 0: r0 = a0; r1 = a1; r2 = a2; r3 = a3; break;
                case 1: r0 = a1; r1 = a2; r2 = a3; r3 = 0; break;
                case 2: r0 = a2; r1 = a3; r2 = 0; r3 = 0; break;
                case 3: r0 = a3; r1 = 0; r2 = 0; r3 = 0; break;
                default: r0 = r1 = r2 = r3 = 0; break;
                }

                UInt64 jam = 0;

                // Any bit in the discarded low words?
                if (wordDist >= 1 && a3 != 0) jam = 1;
                if (wordDist >= 2 && a2 != 0) jam = 1;
                if (wordDist >= 3 && a1 != 0) jam = 1;
                if (wordDist >= 4 && a0 != 0) jam = 1;

                // Bit-level shift.
                if (bitDist != 0) {
                    UInt64 n0 = r0 >> bitDist;
                    UInt64 n1 = (r1 >> bitDist) | (r0 << (64 - bitDist));
                    UInt64 n2 = (r2 >> bitDist) | (r1 << (64 - bitDist));
                    UInt64 n3 = (r3 >> bitDist) | (r2 << (64 - bitDist));
                    r0 = n0; r1 = n1; r2 = n2; r3 = n3;

                    // Low bits of the "partial" word a[3 - wordDist] are discarded.
                    int partialIdx = 3 - wordDist;
                    if (partialIdx >= 0) {
                        UInt64 av = partialIdx switch {
                            0 => a0,
                            1 => a1,
                            2 => a2,
                            3 => a3,
                            _ => 0UL,
                        };
                        if ((av & ((1UL << bitDist) - 1)) != 0) jam = 1;
                    }
                }

                if (jam != 0) r3 |= 1;

                z0 = r0; z1 = r1; z2 = r2; z3 = r3;
            }
        }


        /// <summary>
        /// Selects the sign of the product and of the addend for
        /// <see cref="FusedMultiplyAddOrSubtract(UInt64, UInt64, UInt64, UInt64, UInt64, UInt64, FusedMultiplyAddOrSubtractOperationKind, FloatingPointRounding, out UInt64)"/>.
        /// </summary>
        public enum FusedMultiplyAddOrSubtractOperationKind : byte {
            /// <summary>Compute <c>A * B + C</c>.</summary>
            FusedMultiplyAdd = 0,
            /// <summary>Compute <c>A * B - C</c>.</summary>
            FusedMultiplySubtract = 1,
            /// <summary>Compute <c>-A * B + C</c>.</summary>
            FusedMultiplySubtractAndNegate = 2,
        }

        public static UInt64 FusedMultiplyAddOrSubtract(
            UInt64 first_lo, UInt64 first_hi,
            UInt64 second_lo, UInt64 second_hi,
            UInt64 addOrSubtractOperand_lo, UInt64 addOrSubtractOperand_hi,
            [ConstantExpected] FusedMultiplyAddOrSubtractOperationKind operation, [ConstantExpected] FloatingPointRounding rounding,
            out UInt64 result_hi) {
            unchecked {
                // -------- Operand decomposition --------
                int signA = GetRawSignFromHi64Bits(first_hi);
                int expA = GetRawExponentFromHi64Bits(first_hi);
                UInt64 sigA_hi = GetRawFractionHiFromHi64Bits(first_hi);
                UInt64 sigA_lo = first_lo;

                int signB = GetRawSignFromHi64Bits(second_hi);
                int expB = GetRawExponentFromHi64Bits(second_hi);
                UInt64 sigB_hi = GetRawFractionHiFromHi64Bits(second_hi);
                UInt64 sigB_lo = second_lo;

                int signC = GetRawSignFromHi64Bits(addOrSubtractOperand_hi)
                            ^ (operation == FusedMultiplyAddOrSubtractOperationKind.FusedMultiplySubtract ? 1 : 0);
                int expC = GetRawExponentFromHi64Bits(addOrSubtractOperand_hi);
                UInt64 sigC_hi = GetRawFractionHiFromHi64Bits(addOrSubtractOperand_hi);
                UInt64 sigC_lo = addOrSubtractOperand_lo;

                int signZ = (signA ^ signB)
                            ^ (operation == FusedMultiplyAddOrSubtractOperationKind.FusedMultiplySubtractAndNegate ? 1 : 0);

                // -------- Locals declared up front so goto labels can share them --------
                UInt64 z0 = 0, z1 = 0, z2 = 0, z3 = 0;   // product 256-bit, z0 = MSW
                UInt64 c0 = 0, c1 = 0, c2 = 0, c3 = 0;   // C       256-bit, c0 = MSW

                UInt64 sigZ_hi = 0, sigZ_lo = 0;
                int expZ = 0;
                UInt64 sigZExtra = 0;
                int shiftDist = 0;
                int expDiff = 0;
                UInt64 magBits = 0;

                // -------- Step 1: NaN / Inf in A or B --------
                if (expA == 0x7FFF) {
                    if ((sigA_hi | sigA_lo) != 0
                            || (expB == 0x7FFF && (sigB_hi | sigB_lo) != 0)) {
                        goto propagateNaN_ABC;
                    }
                    magBits = (UInt64)(uint)expB | sigB_hi | sigB_lo;
                    goto infProdArg;
                }
                if (expB == 0x7FFF) {
                    if ((sigB_hi | sigB_lo) != 0) goto propagateNaN_ABC;
                    magBits = (UInt64)(uint)expA | sigA_hi | sigA_lo;
                    goto infProdArg;
                }

                // -------- Step 2: NaN / Inf in C --------
                if (expC == 0x7FFF) {
                    if ((sigC_hi | sigC_lo) != 0) {
                        // C is NaN: let propagateNaN_ZC pick it (A*B is finite here).
                        sigZ_hi = 0;
                        sigZ_lo = 0;
                        goto propagateNaN_ZC;
                    }
                    // C is Inf: A*B is finite, so result = C.
                    result_hi = addOrSubtractOperand_hi;
                    return addOrSubtractOperand_lo;
                }

                // -------- Step 3: normalize A, B --------
                if (expA == 0) {
                    if ((sigA_hi | sigA_lo) == 0) goto zeroProd;
                    expA = NormalizeSubnormal(sigA_lo, sigA_hi, out sigA_lo, out sigA_hi);
                }
                if (expB == 0) {
                    if ((sigB_hi | sigB_lo) == 0) goto zeroProd;
                    expB = NormalizeSubnormal(sigB_lo, sigB_hi, out sigB_lo, out sigB_hi);
                }

                // -------- Step 4: compute the full 256-bit product --------
                expZ = expA + expB - 0x3FFE;
                sigA_hi |= Hi64BitsImplicitBit;
                sigB_hi |= Hi64BitsImplicitBit;
                sigA_lo = ShiftLeftPartial(sigA_lo, sigA_hi, 8, out sigA_hi);
                sigB_lo = ShiftLeftPartial(sigB_lo, sigB_hi, 15, out sigB_hi);

                BigMul(out z0, out z1, out z2, out z3, sigA_hi, sigA_lo, sigB_hi, sigB_lo);
                sigZ_hi = z0;
                sigZ_lo = z1;

                shiftDist = 0;
                if ((sigZ_hi & 0x0100000000000000UL) == 0) {
                    --expZ;
                    shiftDist = -1;
                }

                // -------- Step 5: special case C == 0 --------
                if (expC == 0) {
                    if ((sigC_hi | sigC_lo) == 0) {
                        shiftDist += 8;
                        goto sigZ_label;
                    }
                    expC = NormalizeSubnormal(sigC_lo, sigC_hi, out sigC_lo, out sigC_hi);
                }
                sigC_hi |= Hi64BitsImplicitBit;
                sigC_lo = ShiftLeftPartial(sigC_lo, sigC_hi, 8, out sigC_hi);

                // -------- Step 6: align exponents --------
                expDiff = expZ - expC;
                if (expDiff < 0) {
                    expZ = expC;
                    if (signZ == signC || expDiff < -1) {
                        shiftDist -= expDiff;
                        if (shiftDist != 0) {
                            sigZ_lo = ShiftRightWithJamming(sigZ_lo, sigZ_hi, shiftDist, out sigZ_hi);
                        }
                    } else {
                        // signZ != signC && expDiff == -1
                        if (shiftDist == 0) {
                            // Shift the full 256-bit product right by 1 to keep sticky bits.
                            ShiftRightUnsignedPartial(out UInt64 x_lo, out UInt64 x_hi, z3, z2, 1);
                            z2 = (sigZ_lo << 63) | x_hi;
                            z3 = x_lo;
                            ShiftRightUnsignedPartial(out sigZ_lo, out sigZ_hi, sigZ_lo, sigZ_hi, 1);
                            z0 = sigZ_hi;
                            z1 = sigZ_lo;
                        }
                    }
                } else {
                    if (shiftDist != 0) {
                        // Doubling the 256-bit product (logical << 1).
                        AddUnchecked(out z0, out z1, out z2, out z3, z0, z1, z2, z3, z0, z1, z2, z3);
                    }
                    if (expDiff == 0) {
                        sigZ_hi = z0;
                        sigZ_lo = z1;
                    } else {
                        // Shift C right into the 256-bit frame.
                        c0 = sigC_hi;
                        c1 = sigC_lo;
                        c2 = 0;
                        c3 = 0;
                        ShiftRightUnsignedJamming(c0, c1, c2, c3, expDiff, out c0, out c1, out c2, out c3);
                    }
                }

                // -------- Step 7: combine --------
                shiftDist = 8;
                if (signZ == signC) {
                    // ----- same sign: ADD -----
                    if (expDiff <= 0) {
                        sigZ_lo = DoubleArithmetic.AddUnchecked(
                            sigC_lo, sigC_hi, sigZ_lo, sigZ_hi, out sigZ_hi);
                    } else {
                        AddUnchecked(out z0, out z1, out z2, out z3, z0, z1, z2, z3, c0, c1, c2, c3);
                        sigZ_hi = z0;
                        sigZ_lo = z1;
                    }
                    if ((sigZ_hi & 0x0200000000000000UL) != 0) {
                        ++expZ;
                        shiftDist = 9;
                    }
                    // fall through to sigZ_label
                } else {
                    // ----- opposite signs: SUBTRACT -----
                    if (expDiff < 0) {
                        signZ = signC;
                        if (expDiff < -1) {
                            sigZ_lo = DoubleArithmetic.SubtractUnchecked(
                                sigC_lo, sigC_hi, sigZ_lo, sigZ_hi, out sigZ_hi);
                            sigZExtra = z2 | z3;
                            if (sigZExtra != 0) {
                                sigZ_lo = DoubleArithmetic.SubtractUnchecked(
                                    sigZ_lo, sigZ_hi, 0, 1, out sigZ_hi);
                            }
                            if ((sigZ_hi & 0x0100000000000000UL) == 0) {
                                --expZ;
                                shiftDist = 7;
                            }
                            goto shiftRightRoundPack;
                        } else {
                            // expDiff == -1
                            c0 = sigC_hi;
                            c1 = sigC_lo;
                            c2 = 0;
                            c3 = 0;
                            SubtractUnchecked(out z0, out z1, out z2, out z3, c0, c1, c2, c3,
                                        z0, z1, z2, z3);
                        }
                    } else if (expDiff == 0) {
                        sigZ_lo = DoubleArithmetic.SubtractUnchecked(
                            sigZ_lo, sigZ_hi, sigC_lo, sigC_hi, out sigZ_hi);
                        if ((sigZ_hi | sigZ_lo) == 0 && z2 == 0 && z3 == 0) {
                            goto completeCancellation;
                        }
                        z0 = sigZ_hi;
                        z1 = sigZ_lo;
                        if ((sigZ_hi & 0x8000000000000000UL) != 0) {
                            signZ = 1 - signZ;
                            SubtractUnchecked(out z0, out z1, out z2, out z3, 0, 0, 0, 0,
                                        z0, z1, z2, z3);
                        }
                    } else {
                        // expDiff > 0
                        SubtractUnchecked(out z0, out z1, out z2, out z3, z0, z1, z2, z3,
                                    c0, c1, c2, c3);
                        if (expDiff > 1) {
                            sigZ_hi = z0;
                            sigZ_lo = z1;
                            if ((sigZ_hi & 0x0100000000000000UL) == 0) {
                                --expZ;
                                shiftDist = 7;
                            }
                            goto sigZ_label;
                        }
                    }

                    // ----- renormalize the small difference -----
                    sigZ_hi = z0;
                    sigZ_lo = z1;
                    sigZExtra = z2;
                    UInt64 sig256Z0 = z3;

                    if (sigZ_hi != 0) {
                        if (sig256Z0 != 0) sigZExtra |= 1;
                    } else {
                        expZ -= 64;
                        sigZ_hi = sigZ_lo;
                        sigZ_lo = sigZExtra;
                        sigZExtra = sig256Z0;
                        if (sigZ_hi == 0) {
                            expZ -= 64;
                            sigZ_hi = sigZ_lo;
                            sigZ_lo = sigZExtra;
                            sigZExtra = 0;
                            if (sigZ_hi == 0) {
                                expZ -= 64;
                                sigZ_hi = sigZ_lo;
                                sigZ_lo = 0;
                            }
                        }
                    }

                    shiftDist = (int)UInt64.LeadingZeroCount(sigZ_hi);
                    expZ += 7 - shiftDist;
                    shiftDist = 15 - shiftDist;
                    if (shiftDist > 0) goto shiftRightRoundPack;
                    if (shiftDist != 0) {
                        shiftDist = -shiftDist;
                        sigZ_lo = ShiftLeftPartial(sigZ_lo, sigZ_hi, shiftDist, out sigZ_hi);
                        UInt64 x_lo = ShiftLeftPartial(sigZExtra, 0, shiftDist, out UInt64 x_hi);
                        sigZ_lo |= x_hi;
                        sigZExtra = x_lo;
                    }
                    goto roundPack;
                }

            sigZ_label:
                sigZExtra = z2 | z3;

            shiftRightRoundPack:
                sigZExtra = (sigZ_lo << (64 - shiftDist)) | (sigZExtra != 0 ? 1UL : 0UL);
                ShiftRightUnsignedPartial(out sigZ_lo, out sigZ_hi, sigZ_lo, sigZ_hi, shiftDist);

            roundPack:
                return GetBitsFromRawPartsWithRounding(
                    sigZExtra, sigZ_lo, sigZ_hi, expZ - 1, signZ, rounding, out result_hi);

            propagateNaN_ABC:
                {
                    UInt64 nan_lo = GetNaN(first_lo, first_hi, second_lo, second_hi,
                                           out UInt64 nan_hi);
                    sigZ_lo = nan_lo;
                    sigZ_hi = nan_hi;
                }
                goto propagateNaN_ZC;

            infProdArg:
                if (magBits != 0) {
                    sigZ_hi = GetHi64BitsFromRawParts(signZ, 0x7FFF, 0);
                    sigZ_lo = 0;
                    if (expC != 0x7FFF) goto uiZ;
                    if ((sigC_hi | sigC_lo) != 0) goto propagateNaN_ZC;
                    if (signZ == signC) goto uiZ;
                }
                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                sigZ_hi = 0xFFFF800000000000UL;
                sigZ_lo = 0;

            propagateNaN_ZC:
                {
                    UInt64 nan_lo = GetNaN(sigZ_lo, sigZ_hi,
                                           addOrSubtractOperand_lo, addOrSubtractOperand_hi,
                                           out UInt64 nan_hi);
                    result_hi = nan_hi;
                    return nan_lo;
                }

            zeroProd:
                if (((uint)expC | (sigC_hi | sigC_lo)) == 0 && signZ != signC) {

                } else {
                    goto completeCancellationEnd;
                }
            completeCancellation:
                {
              
                    result_hi = GetHi64BitsFromRawParts(
                        rounding == FloatingPointRounding.Downward ? 1 : 0, 0, 0);
                    return 0;
                }
            completeCancellationEnd:
                result_hi = addOrSubtractOperand_hi;
                return addOrSubtractOperand_lo;

            uiZ:
                result_hi = sigZ_hi;
                return sigZ_lo;
            }
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

        internal static int NormalizeSubnormal(UInt64 fraction_lo, UInt64 fraction_hi, int exponent,
            out UInt64 result_fraction_lo, out UInt64 result_fraction_hi) {
            // fraction_hi holds only 48 bits; the upper 16 bits are always zero.
            int first_lzc;
            if (fraction_hi != 0) {
                first_lzc = BinaryNumerals.CountLeadingZeros(fraction_hi) - 16;
            } else {
                // 48 zero bits from fraction_hi + leading zeros in fraction_lo
                first_lzc = 48 + BinaryNumerals.CountLeadingZeros(fraction_lo);
            }

            // The biased exponent of the normalized form is -first_lzc.
            // (The incoming exponent is 0 for a subnormal.)
            exponent -= first_lzc;

            // Shift left so the highest set bit moves to position 112 (implicit bit).
            // Correct shift amount is first_lzc + 1.
            result_fraction_lo = DoubleArithmetic.ShiftLeft(fraction_lo, fraction_hi,
                first_lzc + 1, out result_fraction_hi);

            return exponent;
        }

        internal static int NormalizeSubnormal_A(UInt64 fraction_lo, UInt64 fraction_hi, int exponent, out UInt64 result_fraction_lo, out UInt64 result_fraction_hi) {
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

        public static UInt64 ScaleB(UInt64 x_lo, UInt64 x_hi, int n,
            [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
            var lx = x_lo;
            var hx = x_hi;
            var k = Binary128Arithmetic.GetRawExponentFromHi64Bits(hx);

            if (0x7fff == k) {
                // Infinity, NaN
                result_hi = hx;
                return lx;
            }
            var s = GetRawSignFromHi64Bits(hx);
           

            var f_hi = GetRawFractionHiFromHi64Bits(hx);

            if (k == 0) {
                // Subnormal, Zero
                if (0 == ((0x7fffffffffffffffu & hx) | lx)) {
                    result_hi = hx;
                    return lx;
                }

                k = Binary128Arithmetic.NormalizeSubnormal(lx, f_hi, k,
                    out lx, out f_hi);
            }
            if (n > 910305) {
                result_hi = 0x8000000000000000 & hx;
                return 0;
            }
            if (n < -900630) {
                result_hi = 0x7fff000000000000UL | (0x8000000000000000 & hx);
                return 0;
            }
            

            // GetBitsFromRawPartsWithRounding expects the implicit bit to be set.
            f_hi |= Binary128Arithmetic.Hi64BitsImplicitBit;


            unchecked {
                k += n;
            }

            // GetBitsFromRawPartsWithRounding expects exponent = true_biased - 1
            // for ALL values (normal and subnormal). Not just subnormals.
            k--;

            // Clamp to avoid overflow/underflow in k += n and in GetBits shifts.
            if (k < -128) {
                k = -128;
            } else if (k > 0x7fff) {
                k = 0x7fff;
            }

            return Binary128Arithmetic.GetBitsFromRawPartsWithRounding(0, lx, f_hi, k, s,
                rounding, out result_hi);
        }

        [Obsolete]
        static UInt64 ScaleB_L(UInt64 x_lo, UInt64 x_hi, int n, [ConstantExpected] FloatingPointRounding rounding, out UInt64 result_hi) {
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


        public static FloatingPointClass GetFloatingPointClass(UInt64 lo, UInt64 hi) {
            UInt64 sign = hi >> 63;
            UInt64 exp = (hi >> 48) & 0x7FFF;
            UInt64 fracHi = hi & 0x0000FFFFFFFFFFFFUL;
            bool fracIsZero = (fracHi | lo) == 0;

            // --- NaN / Infinity ---
            if (exp == 0x7FFF) {
                if (!fracIsZero) {
                    // NaN
                    UInt64 quietBit = (hi >> 47) & 1;
                    return quietBit == 1
                        ? FloatingPointClass.QuietNaN
                        : FloatingPointClass.SignalingNaN;
                }

                // Infinity
                return sign == 1
                    ? FloatingPointClass.NegativeInfinity
                    : FloatingPointClass.PositiveInfinity;
            }

            // --- Zero / Subnormal ---
            if (exp == 0) {
                if (fracIsZero) {
                    return sign == 1
                        ? FloatingPointClass.NegativeZero
                        : FloatingPointClass.PositiveZero;
                }

                return sign == 1
                    ? FloatingPointClass.NegativeSubnormal
                    : FloatingPointClass.PositiveSubnormal;
            }

            // --- Normal ---
            return sign == 1
                ? FloatingPointClass.NegativeNormal
                : FloatingPointClass.PositiveNormal;
        }

        public static int ILogB(UInt64 lo, UInt64 hi) {
            const int Bias = 16383;
            const int FractionBits = 112;

            UInt64 exp = (hi >> 48) & 0x7FFF;
            UInt64 fracHi = hi & 0x0000FFFFFFFFFFFFUL;
            bool fracIsZero = (fracHi == 0) && (lo == 0);

            // --- NaN ---
            if (exp == 0x7FFF && !fracIsZero)
                return ILogSpecialResults.ILogNaN; // FP_ILOGBNAN

            // --- Infinity ---
            if (exp == 0x7FFF && fracIsZero)
                return ILogSpecialResults.ILogInfinity; // FP_ILOGBINF

            // --- Zero ---
            if (exp == 0 && fracIsZero)
                return ILogSpecialResults.ILog0; // FP_ILOGB0

            // --- Subnormal ---
            if (exp == 0 && !fracIsZero)
                return 1 - Bias - FractionBits; // -17494

            // --- Normal ---
            return (int)exp - Bias;
        }

    }


}
