using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;
using Misc = UltimateOrb.Miscellaneous;
using System.Diagnostics;

#if ILGPU_V1_X_COMPATIBILITY
using Unsafe = UltimateOrb.Runtime.CompilerServices.Unsafe;
#else
using Unsafe = System.Runtime.CompilerServices.Unsafe;
#endif
using InlineArrayNS = UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {
    using static UltimateOrb.Runtime.CompilerServices.Unsafe;

    public static partial class Binary128Arithmetic {


        extension(UltimateOrb.Runtime.CompilerServices.Unsafe) {

#if ILGPU_V1_X_COMPATIBILITY
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static TTo BitCastUnchecked<TFrom, TTo>(TFrom source)
#if NET9_0_OR_GREATER
            where TFrom : allows ref struct
            where TTo : allows ref struct
#endif
            {

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
                Debug.Assert(sizeof(TFrom) != sizeof(TTo));
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
                Debug.Assert(!typeof(TFrom).IsValueType);
                Debug.Assert(!typeof(TTo).IsValueType);
                unsafe {
#pragma warning disable CS9087 // This returns a parameter by reference but it is not a ref parameter
                    return ReadUnaligned<TTo>(ref As<TFrom, byte>(ref source));
#pragma warning restore CS9087 // This returns a parameter by reference but it is not a ref parameter
                }
            }
#endif
        }
    }
}

namespace System.Runtime.CompilerServices {
    using static System.Runtime.CompilerServices.Unsafe;

#if !NET9_0_OR_GREATER
    internal static partial class UnsafeExtensions {

        extension(System.Runtime.CompilerServices.Unsafe) {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static TTo BitCast<TFrom, TTo>(TFrom source) {

#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
                if (sizeof(TFrom) != sizeof(TTo) || !typeof(TFrom).IsValueType || !typeof(TTo).IsValueType) {
                    UltimateOrb.Utilities.ThrowHelper.Throw<NotSupportedException>();
                }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
                unsafe {
#pragma warning disable CS9087 // This returns a parameter by reference but it is not a ref parameter
                    return ReadUnaligned<TTo>(ref As<TFrom, byte>(ref source));
#pragma warning restore CS9087 // This returns a parameter by reference but it is not a ref parameter
                }
            }
        }
    }
#endif
}

namespace UltimateOrb.Numerics {

    public static partial class Binary128Arithmetic {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo BitCast<TFrom, TTo>(TFrom source)
#if NET9_0_OR_GREATER
            where TFrom : allows ref struct
            where TTo : allows ref struct
#endif
            {
#if ILGPU_V1_X_COMPATIBILITY
            return Unsafe.BitCastUnchecked<TFrom, TTo>(source);
#else
            return Unsafe.BitCast<TFrom, TTo>(source);
#endif
        }
    }
}

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    public static partial class Binary128Arithmetic {

        // ═════════════════════════════════════════════════════════════════════
        //  Constant data tables
        //
        //  Convention:
        //    Xxx_flat  — single-source flat raw data (ReadOnlySpan<primitive>)
        //    Xxx       — typed view:
        //                  standard : static ref readonly InlineArrayN<...>
        //                  ILGPU    : static readonly  InlineArrayN<...>
        //
        //  Nested tables mirror the C/C++ dimensions.  Where a span view is
        //  required (EvalPoly6 / EvalPoly6Reduced / EvalPoly3), a
        //  MemoryMarshal.CreateReadOnlySpan bridge is used at the call site.
        // ═════════════════════════════════════════════════════════════════════
#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray6<UInt64> Log2E192 =
            LoadUnaligned<InlineArrayNS.InlineArray6<UInt64>>(Log2E192_flat);
        static readonly InlineArrayNS.InlineArray6<UInt64> Log10E192 =
            LoadUnaligned<InlineArrayNS.InlineArray6<UInt64>>(Log10E192_flat);
        static readonly InlineArrayNS.InlineArray7<UInt64> Iln2Log2Of10Half_7 =
            LoadUnaligned<InlineArrayNS.InlineArray7<UInt64>>(Iln2Log2Of10Half_flat);
#else
        static ref readonly InlineArrayNS.InlineArray6<UInt64> Log2E192 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<UInt64>>(
                ref MemoryMarshal.GetReference(Log2E192_flat));
        static ref readonly InlineArrayNS.InlineArray6<UInt64> Log10E192 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<UInt64>>(
                ref MemoryMarshal.GetReference(Log10E192_flat));
        static ref readonly InlineArrayNS.InlineArray7<UInt64> Iln2Log2Of10Half_7 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray7<UInt64>>(
                ref MemoryMarshal.GetReference(Iln2Log2Of10Half_flat));
#endif

        // ── LogLn2A / LogOffA : 3-word ln2 and offset ──────────────────────
#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray3<UInt64> LogLn2A =
            LoadUnaligned<InlineArrayNS.InlineArray3<UInt64>>(LogLn2A_flat);
        static readonly InlineArrayNS.InlineArray3<UInt64> LogOffA =
            LoadUnaligned<InlineArrayNS.InlineArray3<UInt64>>(LogOffA_flat);
#else
        static ref readonly InlineArrayNS.InlineArray3<UInt64> LogLn2A =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray3<UInt64>>(
                ref MemoryMarshal.GetReference(LogLn2A_flat));
        static ref readonly InlineArrayNS.InlineArray3<UInt64> LogOffA =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray3<UInt64>>(
                ref MemoryMarshal.GetReference(LogOffA_flat));
#endif

        // ── LogLt0A..3 : [32][3] UInt64 ────────────────────────────────────

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt0A =
            LoadUnaligned<InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(LogLt0A_flat);
        static readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt1A =
            LoadUnaligned<InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(LogLt1A_flat);
        static readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt2A =
            LoadUnaligned<InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(LogLt2A_flat);
        static readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt3A =
            LoadUnaligned<InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(LogLt3A_flat);
#else
        static ref readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt0A =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(
                ref MemoryMarshal.GetReference(LogLt0A_flat));
        static ref readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt1A =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(
                ref MemoryMarshal.GetReference(LogLt1A_flat));
        static ref readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt2A =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(
                ref MemoryMarshal.GetReference(LogLt2A_flat));
        static ref readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>> LogLt3A =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray3<UInt64>>>(
                ref MemoryMarshal.GetReference(LogLt3A_flat));
#endif

        // ── LogC / LogCp / LogCn : [6][2] UInt64 ───────────────────────────

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogC =
            LoadUnaligned<InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(LogC_flat);
        static readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogCp =
            LoadUnaligned<InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(LogCp_flat);
        static readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogCn =
            LoadUnaligned<InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(LogCn_flat);
#else
        static ref readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogC =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(
                ref MemoryMarshal.GetReference(LogC_flat));
        static ref readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogCp =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(
                ref MemoryMarshal.GetReference(LogCp_flat));
        static ref readonly InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>> LogCn =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<InlineArrayNS.InlineArray2<UInt64>>>(
                ref MemoryMarshal.GetReference(LogCn_flat));
#endif

        // ── LogC37 / LogTbl6 : [37][6] / [16][6] UInt64 ────────────────────
        //  These are consumed by EvalPoly6 which takes a span, so in both
        //  modes we expose a span-compatible view.  The nested InlineArray
        //  view is available for callers that index directly (LogTbl6[jt]).

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>> LogC37 =
            LoadUnaligned<InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>>>(LogC37_flat);
        static readonly InlineArrayNS.InlineArray16<InlineArrayNS.InlineArray6<UInt64>> LogTbl6 =
            LoadUnaligned<InlineArrayNS.InlineArray16<InlineArrayNS.InlineArray6<UInt64>>>(LogTbl6_flat);

        // Span bridge (ILGPU: spans may still be OK for stack-local views)
        static ReadOnlySpan<InlineArrayNS.InlineArray6<UInt64>> LogC37_Span =>
            MemoryMarshal.CreateReadOnlySpan(
                ref Unsafe.As<InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>>,
                               InlineArrayNS.InlineArray6<UInt64>>(ref Unsafe.AsRef(in LogC37)),
                37);
#else
        static ref readonly InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>> LogC37 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>>>(
                ref MemoryMarshal.GetReference(LogC37_flat));
        static ref readonly InlineArrayNS.InlineArray16<InlineArrayNS.InlineArray6<UInt64>> LogTbl6 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray16<InlineArrayNS.InlineArray6<UInt64>>>(
                ref MemoryMarshal.GetReference(LogTbl6_flat));

        static ReadOnlySpan<InlineArrayNS.InlineArray6<UInt64>> LogC37_Span =>
            MemoryMarshal.CreateReadOnlySpan(
                ref Unsafe.As<InlineArrayNS.InlineArray37<InlineArrayNS.InlineArray6<UInt64>>,
                               InlineArrayNS.InlineArray6<UInt64>>(ref Unsafe.AsRef(in LogC37)),
                37);
#endif

        // ── LogTbl32 : [32][4] UInt32 ──────────────────────────────────────

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray4<UInt32>> LogTbl32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray4<UInt32>>>(LogTbl32_flat);
#else
        static ref readonly InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray4<UInt32>> LogTbl32 =>
            ref Unsafe.As<UInt32, InlineArrayNS.InlineArray32<InlineArrayNS.InlineArray4<UInt32>>>(
                ref MemoryMarshal.GetReference(LogTbl32_flat));
#endif

        // ── 1D 32-entry tables ─────────────────────────────────────────────

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray32<UInt32> LogRt0 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt32>>(LogRt0_flat);
        static readonly InlineArrayNS.InlineArray32<UInt32> LogRt1 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt32>>(LogRt1_flat);
        static readonly InlineArrayNS.InlineArray32<UInt32> LogRt2 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt32>>(LogRt2_flat);
        static readonly InlineArrayNS.InlineArray32<UInt32> LogRt3 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt32>>(LogRt3_flat);
        static readonly InlineArrayNS.InlineArray32<UInt64> LogC0 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt64>>(LogC0_flat);
#else
        static ref readonly InlineArrayNS.InlineArray32<UInt32> LogRt0 =>
            ref Unsafe.As<UInt32, InlineArrayNS.InlineArray32<UInt32>>(
                ref MemoryMarshal.GetReference(LogRt0_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt32> LogRt1 =>
            ref Unsafe.As<UInt32, InlineArrayNS.InlineArray32<UInt32>>(
                ref MemoryMarshal.GetReference(LogRt1_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt32> LogRt2 =>
            ref Unsafe.As<UInt32, InlineArrayNS.InlineArray32<UInt32>>(
                ref MemoryMarshal.GetReference(LogRt2_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt32> LogRt3 =>
            ref Unsafe.As<UInt32, InlineArrayNS.InlineArray32<UInt32>>(
                ref MemoryMarshal.GetReference(LogRt3_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt64> LogC0 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<UInt64>>(
                ref MemoryMarshal.GetReference(LogC0_flat));
#endif

        // ── Log10 exact-hash tables ────────────────────────────────────────

#if ILGPU_V1_X_COMPATIBILITY
        static readonly InlineArrayNS.InlineArray32<UInt64> Log10_K_Shift48_n32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt64>>(Log10_K_Shift48_n32_flat);
        static readonly InlineArrayNS.InlineArray32<Byte> Log10_B_Shift48_n32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<Byte>>(Log10_B_Shift48_n32_flat);
        static readonly InlineArrayNS.InlineArray32<UInt64> Log10_K1_S48_4_N32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt64>>(Log10_K1_S48_4_N32_flat);
        static readonly InlineArrayNS.InlineArray32<UInt64> Log10_K2_S48_4_N32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<UInt64>>(Log10_K2_S48_4_N32_flat);
        static readonly InlineArrayNS.InlineArray32<Byte> Log10_B_S48_4_N32 =
            LoadUnaligned<InlineArrayNS.InlineArray32<Byte>>(Log10_B_S48_4_N32_flat);
#else
        static ref readonly InlineArrayNS.InlineArray32<UInt64> Log10_K_Shift48_n32 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<UInt64>>(
                ref MemoryMarshal.GetReference(Log10_K_Shift48_n32_flat));
        static ref readonly InlineArrayNS.InlineArray32<Byte> Log10_B_Shift48_n32 =>
            ref Unsafe.As<Byte, InlineArrayNS.InlineArray32<Byte>>(
                ref MemoryMarshal.GetReference(Log10_B_Shift48_n32_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt64> Log10_K1_S48_4_N32 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<UInt64>>(
                ref MemoryMarshal.GetReference(Log10_K1_S48_4_N32_flat));
        static ref readonly InlineArrayNS.InlineArray32<UInt64> Log10_K2_S48_4_N32 =>
            ref Unsafe.As<UInt64, InlineArrayNS.InlineArray32<UInt64>>(
                ref MemoryMarshal.GetReference(Log10_K2_S48_4_N32_flat));
        static ref readonly InlineArrayNS.InlineArray32<Byte> Log10_B_S48_4_N32 =>
            ref Unsafe.As<Byte, InlineArrayNS.InlineArray32<Byte>>(
                ref MemoryMarshal.GetReference(Log10_B_S48_4_N32_flat));
#endif
    }
}


namespace UltimateOrb.Numerics {

#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    public static partial class Binary128Arithmetic {
        // ═══════════════════════════════════════════════════════════════════
        // Public API
        // ═══════════════════════════════════════════════════════════════════
        // ═════════════════════════════════════════════════════════════════════
        //  Public API
        // ═════════════════════════════════════════════════════════════════════
        public static UInt64 Log(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Log(lo, hi, MidpointRounding.ToEven, out result_hi);
        public static UInt64 Log2(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Log2(lo, hi, MidpointRounding.ToEven, out result_hi);
        public static UInt64 Log10(UInt64 lo, UInt64 hi, out UInt64 result_hi)
            => Log10(lo, hi, MidpointRounding.ToEven, out result_hi);

        public static UInt64 Log(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi)
            => LogCore(lo, hi, mode, LogScale.Ln, out result_hi);
        public static UInt64 Log2(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi)
            => LogCore(lo, hi, mode, LogScale.Log2, out result_hi);
        public static UInt64 Log10(UInt64 lo, UInt64 hi, MidpointRounding mode, out UInt64 result_hi)
            => LogCore(lo, hi, mode, LogScale.Log10, out result_hi);

        enum LogScale { Ln, Log2, Log10 }

        // ═════════════════════════════════════════════════════════════════════
        //  Base-conversion multipliers, encoded as log_base(e) · 2^192.
        //
        //  Represented as 6 UInt64 words.  MultiplyHigh(a, c) then computes
        //      (a · c) >> 384
        //  for a 6-word a; if a is placed in the HIGH 3 words (i.e. a = v·2^192
        //  for a 3-word v), this reduces to v · c / 2^192 = v · log_base(e).
        // ═════════════════════════════════════════════════════════════════════

        // ─────────────────────────────────────────────────────────────────────
        //  PackB128Int64 — encode any integer |n| < 2^15 as binary128.  Exact.
        // ─────────────────────────────────────────────────────────────────────
        static UInt64 PackB128Int64(int n, out UInt64 result_hi) {
            unchecked {
                if (n == 0) { result_hi = 0; return 0; }
                UInt64 sign = n < 0 ? 1UL : 0UL;
                UInt64 absN = (UInt64)(n < 0 ? -n : n);
                int msb = 63 - (int)UInt64.LeadingZeroCount(absN);
                UInt128 frac112 = ((UInt128)(absN - (1UL << msb))) << (112 - msb);
                UInt64 expField = (UInt64)(msb + 16383);
                result_hi = (sign << 63) | (expField << 48) | (UInt64)(frac112 >> 64);
                return (UInt64)frac112;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  Main entry
        // ═════════════════════════════════════════════════════════════════════
        static UInt64 LogCore(UInt64 lo, UInt64 hi, MidpointRounding mode,
            [ConstantExpected] LogScale scale, out UInt64 result_hi) {
            unchecked {
                bool isNearest = IsNearest(mode);
                bool isUp = mode == MidpointRounding.ToPositiveInfinity;
                bool isDown = mode == MidpointRounding.ToNegativeInfinity;

                // ── Specials ────────────────────────────────────────────────
                if (Misc.Unlikely(hi >= ((UInt64)0x7FFF << 48))) {
                    UInt64 b1 = hi & 0x7FFF_FFFF_FFFF_FFFFUL;
                    if (b1 > ((UInt64)0x7FFF << 48) ||
                        (b1 == ((UInt64)0x7FFF << 48) && lo != 0)) {
                        if ((b1 & (1UL << 47)) == 0)
                            RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                        result_hi = hi | (1UL << 47);
                        return lo;
                    }
                    if (hi == ((UInt64)0x7FFF << 48) && lo == 0) {
                        result_hi = hi;
                        return lo;
                    }
                    if (b1 == 0 && lo == 0) {
                        RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.DivideByZero);
                        result_hi = 0xFFFFUL << 48;
                        return 0;
                    }
                    RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Invalid);
                    result_hi = 0xFFFF8UL << 44;
                    return 11;
                }

                // ── Near one ────────────────────────────────────────────────
                if ((UInt64)0X3ffe_ffff_f000_0040UL <= hi &&
                       hi <= 0X3fff_0000_0800_0020UL) {
                    return LogNearOne(lo, hi, mode, scale, out result_hi);
                }

                // Log2 exact shortcut
                if (scale == LogScale.Log2) {
                    UInt64 expField = (hi >> 48) & 0x7FFF;
                    UInt64 fracField = hi & 0x0000_FFFF_FFFF_FFFFUL;

                    if (expField != 0 && expField != 0x7FFF) {
                        // Normal:  x = 2^n  iff the entire 112-bit fraction is zero.
                        if (Misc.Unlikely(fracField == 0 && lo == 0)) {
                            int n = (int)expField - 16383;
                            return PackB128Int64(n, out result_hi);
                        }
                    } else if (Misc.Unlikely(expField == 0)) {
                        // Subnormal:  x = 2^n  iff the 112-bit fraction is a single power of 2.
                        UInt128 frac112 = ((UInt128)fracField << 64) | lo;
                        if (frac112 != 0 && (frac112 & (frac112 - 1)) == 0) {
                            int p = 127 - (int)System.UInt128.LeadingZeroCount(frac112); // ∈ [0, 111]
                            int n = p - 16494;                                     // ∈ [-16494, -16383]
                            return PackB128Int64(n, out result_hi);
                        }
                    }
                } else if (scale == LogScale.Log10) {
                    var n = Log10_GetValue(hi, lo);
                    if (Misc.Unlikely(n != 0)) {
                        return PackB128Int64(n, out result_hi);
                    }
                }

                // ── Main path ───────────────────────────────────────────────
                UInt64 mLo = lo, mHi = hi;
                UInt64 e = hi & ((UInt64)0xFFFF << 48);
                if (Misc.Unlikely(e == 0)) { /* … */ }
                e += (UInt64)112 << 48;

                UInt64 l = Log2Mantissa(mHi);
                int j0 = (int)(l >> 59);
                int j1 = (int)((l >> 54) & 31);
                int j2 = (int)((l >> 49) & 31);
                int j3 = (int)((l >> 44) & 31);

                mHi = (mHi & 0x0000_FFFF_FFFF_FFFFUL) | ((UInt64)1 << 48);

                UInt64 r0 = LogRt0[j0], r1 = LogRt1[j1];
                UInt64 r2 = LogRt2[j2], r3 = LogRt3[j3];

                UInt64 r01 = r0 * r1, r23 = r2 * r3;
                UInt128 mVal = ((UInt128)mHi << 64) | mLo;
                UInt128 rh = Math.BigMul(mVal, (UInt128)r01 * (UInt128)r23, out UInt128 rl);
                UInt64 rhh = (UInt64)(rh >> 64), rhl = (UInt64)rh, rlh = (UInt64)(rl >> 64);
                mHi = (rhh << 40) | (rhl >> 24);
                mLo = (rhl << 40) | (rlh >> 24);
                mVal = ((UInt128)mHi << 64) | mLo;

                // fs = e·ln2 + off + Σ chunk·ln2   (no more Load3)
                InlineArrayNS.InlineArray3<UInt64> fs;
                MultiplyAddHigh(out fs, e, in LogLn2A, in LogOffA);
                Add(ref fs, in fs, in LogLt0A[j0]);
                Add(ref fs, in fs, in LogLt1A[j1]);
                Add(ref fs, in fs, in LogLt2A[j2]);
                Add(ref fs, in fs, in LogLt3A[j3]);

                // ln(1 + u) series — LogC is [6][2]
                UInt64 t5 = MultiplyHigh(mHi, LogC[5][0]);
                UInt64 t4 = LogC[4][0] - t5;
                UInt64 t3lo = LogC[3][0] - MultiplyHigh(mHi, t4);
                UInt128 f = ((UInt128)LogC[3][1] << 64) | t3lo;
                for (int i = 2; i >= 0; i--) {
                    UInt128 ci = ((UInt128)LogC[i][1] << 64) | LogC[i][0];
                    f = ci - MultiplyHighApproximate(mVal, f);
                }
                UInt128 resA = MultiplyHighApproximate(mVal, f);

                InlineArrayNS.InlineArray3<UInt64> d3 = default;
                d3[0] = (UInt64)resA; d3[1] = (UInt64)(resA >> 64); d3[2] = 0;
                Add(ref fs, in fs, in d3);

                // ═════════════════════════════════════════════════════════════
                //  BASE CONVERSION:  multiply fs by log_base(e).
                //  fs stays in the same implicit scale, so the normalization
                //  and rounding below are unaffected — only the numerical
                //  value of fs is rescaled.
                // ═════════════════════════════════════════════════════════════
                switch (scale) {
                case LogScale.Log2: ScaleByConstant(ref fs, in Log2E192); break;
                case LogScale.Log10: ScaleByConstant(ref fs, in Log10E192); break;
                }

                // ── Normalize ────────────────────────────────────────────────
                Int64 msk = unchecked((Int64)fs[2]) >> 63;
                UInt64 mskU = unchecked((UInt64)msk);
                fs[0] ^= mskU; fs[1] ^= mskU; fs[2] ^= mskU;

                int nz = Misc.Likely(fs[2] != 0)
                    ? (int)UInt64.LeadingZeroCount(fs[2])
                    : (int)UInt64.LeadingZeroCount(fs[1]) + 64;
                int ns = nz - 15;

                UInt64 t = fs[0];
                UInt64 tm = ~0UL >> ns;
                UInt64 rnd = (fs[0] >> (63 - ns)) & 1;

                Int64 el = unchecked((Int64)(
                    ((UInt64)0x4029 - (UInt64)nz) | (mskU << 15)));

                UInt64 rLo, rHi;
                if (Misc.Unlikely(((t + ((isNearest ? 1UL : 0UL) << (63 - ns)) + 12) & tm) <= 24)) {
                    int ls = nz - 14, rs = (-ls) & 63;
                    rHi = (fs[2] << ls) | (fs[1] >> rs);
                    rLo = (fs[1] << ls) | (fs[0] >> rs);

                    rLo = AddWithCarry(rLo, (fs[0] >> (63 - ls)) & 1, 0, out var carryBit);
                    rHi = AddWithCarry(rHi, 0, carryBit, out _);

                    rnd = LogRefine(el, ref rLo, ref rHi, lo, hi, scale);
                } else {
                    rHi = (fs[2] << ns) | (fs[1] >> ((-ns) & 63));
                    rLo = (fs[1] << ns) | (fs[0] >> ((-ns) & 63));
                }

                if (Misc.Unlikely(!isNearest)) {
                    if (isUp) rnd = (msk == 0) ? 1UL : 0UL;
                    else if (isDown) rnd = (msk != 0) ? 1UL : 0UL;
                    else rnd = 0;
                }

                UInt64 elShifted = unchecked((UInt64)(el << 48));
                UInt128 res = ((UInt128)rHi << 64) | rLo;
                res += ((UInt128)elShifted << 64) | rnd;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(res >> 64);
                return (UInt64)res;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  LogNearOne — scale-aware
        // ═════════════════════════════════════════════════════════════════════
        static UInt64 LogNearOne(UInt64 xLo, UInt64 xHi, MidpointRounding rm,
            [ConstantExpected] LogScale scale, out UInt64 result_hi) {
            unchecked {
                const UInt64 OneHi = (UInt64)0x3FFF << 48;

                bool isNearest = IsNearest(rm);
                bool isUp = rm == MidpointRounding.ToPositiveInfinity;
                bool isDown = rm == MidpointRounding.ToNegativeInfinity;

                Int64 neg;
                UInt64 uLo, uHi;
                int e;

                if (xHi >= OneHi) {
                    neg = 0;
                    uHi = xHi - OneHi;
                    uLo = xLo;
                    if (uHi == 0 && uLo == 0) { result_hi = 0; return 0; }

                    int nz0 = uHi != 0
                        ? (int)UInt64.LeadingZeroCount(uHi)
                        : (int)UInt64.LeadingZeroCount(uLo) + 64;
                    UInt128 sh = (((UInt128)uHi << 64) | uLo) << nz0;
                    uLo = (UInt64)sh; uHi = (UInt64)(sh >> 64);
                    e = nz0 - 16;
                } else {
                    neg = 1;
                    uLo = SubtractWithBorrow(0, xLo, 0, out var borrow);
                    uHi = SubtractWithBorrow(OneHi, xHi, borrow, out _);

                    int nz0 = uHi != 0
                        ? (int)UInt64.LeadingZeroCount(uHi)
                        : (int)UInt64.LeadingZeroCount(uLo) + 64;
                    UInt128 sh = (((UInt128)uHi << 64) | uLo) << nz0;
                    uLo = (UInt64)sh; uHi = (UInt64)(sh >> 64);
                    e = nz0 - 16;
                }

                UInt128 uVal = ((UInt128)uHi << 64) | uLo;
                UInt128 u2 = MultiplyHighApproximate(uVal, uVal);

                int mShift = e - 20;
                UInt128 mVal = mShift >= 0 ? (uVal >> mShift) : (uVal << (-mShift));
                UInt64 mB1 = (UInt64)(mVal >> 64);

                if (neg == 0) {
                    UInt64 t5 = MultiplyHigh(mB1, LogCp[5][0]);
                    UInt64 t4 = LogCp[4][0] - t5;
                    UInt64 t3 = LogCp[3][0] - MultiplyHigh(mB1, t4);
                    UInt128 f = ((UInt128)LogCp[3][1] << 64) | t3;
                    for (int i = 2; i >= 0; i--) {
                        UInt128 ci = ((UInt128)LogCp[i][1] << 64) | LogCp[i][0];
                        f = ci - MultiplyHighApproximate(mVal, f);
                    }
                    mVal = MultiplyHighApproximate(u2, f);
                } else {
                    UInt64 inner = MultiplyHigh(mB1, LogCn[5][0]);
                    inner += LogCn[4][0];
                    inner = MultiplyHigh(mB1, inner);
                    inner += LogCn[3][0];
                    UInt128 f0 = ((UInt128)LogCn[2][1] << 64) | LogCn[2][0];
                    f0 += (UInt128)MultiplyHigh(mB1, inner);
                    for (int i = 1; i >= 0; i--) {
                        UInt128 ci = ((UInt128)LogCn[i][1] << 64) | LogCn[i][0];
                        f0 = ci + MultiplyHighApproximate(mVal, f0);
                    }
                    mVal = MultiplyHighApproximate(u2, f0);
                }

                InlineArrayNS.InlineArray3<UInt64> z = default, tArr = default;
                tArr[0] = 0;
                tArr[1] = (UInt64)mVal;
                tArr[2] = (UInt64)(mVal >> 64);

                if (neg == 0) {
                    z[0] = 0; z[1] = uLo; z[2] = uHi;
                    Rlshft3(ref tArr, e);
                    Subtract(ref z, in z, in tArr);
                } else {
                    UInt128 uHalf = uVal >> 1;
                    z[0] = 0;
                    z[1] = (UInt64)uHalf;
                    z[2] = (UInt64)(uHalf >> 64);
                    Rlshft3(ref tArr, e + 2);
                    Add(ref z, in z, in tArr);
                }

                // ═════════════════════════════════════════════════════════════
                //  BASE CONVERSION:  z is ln(1 ± u) in implicit scale; multiply
                //  by log_base(e) before the rounding test.
                // ═════════════════════════════════════════════════════════════
                switch (scale) {
                case LogScale.Log2: e -=ScaleByConstantUnsignedWithCarry(ref z, in Log2E192); break;
                case LogScale.Log10: ScaleByConstantUnsigned(ref z, in Log10E192); break;
                }

                // ── Rounding test ────────────────────────────────────────────
                UInt64 eps = 12;
                UInt64 signZ2 = z[2] >> 63;
                UInt128 tl = ((UInt128)z[1] << 64) | z[0];
                UInt128 mskR = UInt128.MaxValue;
                UInt128 nrnd = (UInt128)(isNearest ? 1UL : 0UL) << (int)(13 + signZ2);
                mskR >>= (int)(50 - signZ2);

                bool crnd;
                if (neg == 0) {
                    if (e < 64) {
                        tl >>= 64 - e;
                        mskR >>= 64 - e;
                        nrnd = (nrnd << e) | eps;
                    } else {
                        eps = 1 + (eps >> (e - 64));
                        nrnd = (nrnd << 64) | eps;
                    }
                    tl = mskR & (tl + nrnd);
                    crnd = tl <= 2 * eps;
                } else {
                    if (e <= 62) {
                        tl >>= 62 - e;
                        mskR >>= 62 - e;
                        nrnd = (nrnd << e) | eps;
                    } else {
                        eps = 1 + (eps >> (e - 62));
                        nrnd = (nrnd << 62) | eps;
                    }
                    tl = mskR & (tl + nrnd);
                    crnd = tl <= 2 * eps;
                }

                // ── Normalize ────────────────────────────────────────────────
                UInt128 res = ((UInt128)z[2] << 64) | z[1];

                if (scale == LogScale.Log10) {
                    if (Misc.Unlikely(z[2] >> 61 == 0)) {
                        res <<= 3;
                        e += 3;
                    } else if (Misc.Unlikely((z[2] >> 62) == 0)) {
                        res <<= 2;
                        e += 2;
                    } else if ((z[2] >> 63) == 0) {
                        res <<= 1;
                        e += 1;
                    }
                } else {
                    if ((z[2] >> 63) == 0) {
                        res <<= 1;
                        e++;
                    }
                }

                UInt64 rnd = ((UInt64)res >> 14) & 1;
                e = 16381 - e;

                if (crnd) {
                    res = (res + ((UInt128)1 << 13)) >> 14;
                    UInt64 rLo = (UInt64)res;
                    UInt64 rHi = (UInt64)(res >> 64);
                    Int64 el = unchecked((Int64)((UInt64)e | ((UInt64)neg << 15)));
                    rnd = LogRefine(el, ref rLo, ref rHi, xLo, xHi, scale);
                    res = ((UInt128)rHi << 64) | rLo;
                } else {
                    res >>= 15;
                }

                if (!isNearest) {
                    if (isUp) rnd = neg == 0 ? 1UL : 0UL;
                    else if (isDown) rnd = neg != 0 ? 1UL : 0UL;
                    else rnd = 0;
                }

                UInt64 expField = (UInt64)e << 48;
                UInt64 signField = (UInt64)neg << 63;
                res += ((UInt128)(expField | signField) << 64) | rnd;

                RaiseExceptionFlagsDummy(FloatingPointExceptionFlags.Inexact);
                result_hi = (UInt64)(res >> 64);
                return (UInt64)res;
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  LogRefine  (LogC37_Span / LogTbl6 / Iln2Log2Of10Half_7)
        // ═════════════════════════════════════════════════════════════════════
        static UInt64 LogRefine(Int64 el, ref UInt64 mLo, ref UInt64 mHi,
                                UInt64 xLo, UInt64 xHi,
                                [ConstantExpected] LogScale scale) {
            unchecked {
                Int64 sm = -((el >> 15) & 1);
                UInt64 smU = unchecked((UInt64)sm);
                UInt128 mVal = ((UInt128)mHi << 64) | mLo;

                InlineArrayNS.InlineArray7<UInt64> x = default;
                int baseShiftAdjust;
                switch (scale) {
                case LogScale.Log2:
                    x[4] = (UInt64)(mVal & 1) << 63;
                    x[5] = (UInt64)(mVal >> 1);
                    x[6] = (UInt64)(mVal >> 65);
                    baseShiftAdjust = 0;
                    break;
                case LogScale.Log10:
                    MultiplyHigh(out x, in Iln2Log2Of10Half_7, mVal);
                    baseShiftAdjust = -1;
                    break;
                default: // Ln
                    MultiplyHigh(out x, in ExpIlN2_7, mVal);
                    baseShiftAdjust = 0;
                    break;
                }

                x[0] ^= smU; x[1] ^= smU; x[2] ^= smU; x[3] ^= smU;
                x[4] ^= smU; x[5] ^= smU; x[6] ^= smU;

                int shift = unchecked((int)((0x402E + baseShiftAdjust) - (el & 0x7FFF)));
                ShiftRightArithmetic(ref x, shift);

                int jt = (int)(x[5] >> 60);
                x[5] &= 0x0FFF_FFFF_FFFF_FFFFUL;

                InlineArrayNS.InlineArray6<UInt64> f, f1 = default, f2 = default, ft = default;

                // EvalPoly6 / EvalPoly6Reduced need a span; bridge via LogC37_Span.
                EvalPoly6(out f, x[5], LogC37_Span);
                EvalPoly6Reduced(out f1, x[4], LogC37_Span);

                // x6 = first 6 words of x (reinterpret)
                InlineArrayNS.InlineArray6<UInt64> x6 =
                    Unsafe.As<InlineArrayNS.InlineArray7<UInt64>, InlineArrayNS.InlineArray6<UInt64>>(ref x);
                EvalPoly6ReducedReduced(out f2, ref x6, LogC37_Span);

                MultiplyHigh(out ft, in f, in f1);
                MultiplyHigh(out ft, in ft, in f2);
                MultiplyHigh(out ft, in ft, in LogTbl6[jt]);   // nested InlineArray indexing

                UInt64 sticky = (ft[0] | ft[1] | ft[2] | ft[3]) != 0 ? 1UL : 0UL;
                ft[4] |= sticky;

                Int64 d = unchecked((Int64)(ft[4] - (xLo << 12)));
                d = d >= 0 ? 1 : -1;
                if (sm != 0) d = -d;

                UInt128 dExt = unchecked((UInt128)(Int128)d);
                UInt128 M = ((UInt128)mHi << 64) | mLo;
                M = (M << 1) - dExt;

                mLo = (UInt64)(M >> 2);
                mHi = (UInt64)(M >> 66);
                return (UInt64)((M >> 1) & 1);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  Log2Mantissa  (LogTbl32 = [32][4]; LogC0 = [32])
        // ═════════════════════════════════════════════════════════════════════
        static UInt64 Log2Mantissa(UInt64 x) {
            unchecked {
                int j = (int)((x >> 43) & 31);
                UInt32 z = (UInt32)(x >> 11);
                UInt32 z2 = (UInt32)(((UInt64)z * (UInt64)z) >> 32);

                UInt32 t0 = LogTbl32[j][0] - (UInt32)(((UInt64)z * LogTbl32[j][1]) >> 38);
                UInt32 t2 = LogTbl32[j][2] - (UInt32)(((UInt64)z * LogTbl32[j][3]) >> 32);
                t0 = (UInt32)(t0 + (UInt32)(((UInt64)z2 * t2) >> 44));

                return LogC0[j] + (((UInt64)z * t0) >> 4);
            }
        }

        // ═════════════════════════════════════════════════════════════════════
        //  ScaleByConstant
        //
        //  v := |v| · c / 2^192, then restore sign.
        //
        //  Implementation: place |v| in the HIGH 3 words of a 6-word value
        //  (i.e. as |v|·2^192), then MultiplyHigh against the 6-word constant
        //  c whose value is `constant · 2^192`.  The product's top 6 words are
        //
        //      (|v|·2^192 · c) >> 384 = |v| · c / 2^192 = |v| · constant
        //
        //  which is what we want.  No manual word-by-word multiply needed.
        // ═════════════════════════════════════════════════════════════════════
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ScaleByConstant(ref InlineArrayNS.InlineArray3<UInt64> v, in InlineArrayNS.InlineArray6<UInt64> c) {
            unchecked {
                // Sign-extract |v|.
                Int64 sign = unchecked((Int64)v[2]) >> 63;
                UInt64 sMask = unchecked((UInt64)sign);

                UInt64 a0 = v[0] ^ sMask;
                UInt64 a1 = v[1] ^ sMask;
                UInt64 a2 = v[2] ^ sMask;
                UInt64 carry = unchecked((UInt64)(sign & 1));
                a0 = AddWithCarry(a0, carry, 0, out nuint c0);
                a1 = AddWithCarry(a1, 0, c0, out c0);
                a2 = AddWithCarry(a2, 0, c0, out _);

                // Place |v| in HIGH 3 words.
                InlineArrayNS.InlineArray6<UInt64> a6 = default;
                a6[3] = a0; a6[4] = a1; a6[5] = a2;

                InlineArrayNS.InlineArray6<UInt64> o6 = default;
                MultiplyHigh(out o6, in a6, in c);

                UInt64 r0 = o6[0], r1 = o6[1], r2 = o6[2];

                // Re-apply sign (two's-complement negate).
                if (sMask != 0) {
                    r0 = ~r0; r1 = ~r1; r2 = ~r2;
                    r0 = AddWithCarry(r0, 1, 0, out c0);
                    r1 = AddWithCarry(r1, 0, c0, out c0);
                    r2 = AddWithCarry(r2, 0, c0, out _);
                }

                v[0] = r0; v[1] = r1; v[2] = r2;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void ScaleByConstantUnsigned(ref InlineArrayNS.InlineArray3<UInt64> v, in InlineArrayNS.InlineArray6<UInt64> c) {
            unchecked {
                // Place v in HIGH 3 words.
                InlineArrayNS.InlineArray6<UInt64> a6 = default;
                a6[3] = v[0]; a6[4] = v[1]; a6[5] = v[2];

                InlineArrayNS.InlineArray6<UInt64> o6 = default;
                MultiplyHigh(out o6, in a6, in c);

                UInt64 r0 = o6[0], r1 = o6[1], r2 = o6[2];

                v[0] = r0; v[1] = r1; v[2] = r2;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int ScaleByConstantUnsignedWithCarry(ref InlineArrayNS.InlineArray3<UInt64> v, in InlineArrayNS.InlineArray6<UInt64> c) {
            unchecked {
                // Place v in HIGH 3 words → a6 = v·2^192.
                InlineArrayNS.InlineArray6<UInt64> a6 = default;
                a6[3] = v[0]; a6[4] = v[1]; a6[5] = v[2];

                InlineArrayNS.InlineArray6<UInt64> o6 = default;
                MultiplyHigh(out o6, in a6, in c);

                if (Misc.Unlikely(o6[3] != 0)) {
                    // Overflow into bit 192: v·log_base(e) ≥ 2^192.
                    // Halve it (arithmetic-free logical shift right by 1) and
                    // compensate by bumping the exponent so that the final
                    // biased exponent (16381 - e) increases by 1.
                    v[0] = (o6[0] >> 1) | (o6[1] << 63);
                    v[1] = (o6[1] >> 1) | (o6[2] << 63);
                    v[2] = (o6[2] >> 1) | (o6[3] << 63);
                    return 1;
                } else {
                    v[0] = o6[0];
                    v[1] = o6[1];
                    v[2] = o6[2];
                    return 0;
                }
            }
        }
        // ═════════════════════════════════════════════════════════════════════
        //  Load3 / Load6 — reinterpret a ReadOnlySpan slice as an InlineArrayNS.InlineArray.
        //
        //  InlineArrayNS.InlineArrayN<UInt64> has exactly the layout of UInt64[N], so
        //  Unsafe.As on the span's first-element ref gives us a value copy
        //  without a manual element-by-element loop.
        // ═════════════════════════════════════════════════════════════════════
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ref readonly InlineArrayNS.InlineArray3<UInt64> Load3(ReadOnlySpan<UInt64> s, int wordOffset) {
            return ref Unsafe.As<UInt64, InlineArrayNS.InlineArray3<UInt64>>(
                ref Unsafe.Add(ref MemoryMarshal.GetReference(s), wordOffset * 3));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ref readonly InlineArrayNS.InlineArray6<UInt64> Load6(ReadOnlySpan<UInt64> s, int wordOffset) {
            return ref Unsafe.As<UInt64, InlineArrayNS.InlineArray6<UInt64>>(
                ref Unsafe.Add(ref MemoryMarshal.GetReference(s), wordOffset * 6));
        }

        // ═══════════════════════════════════════════════════════════════════
        // as_log2  (crude log2 approximation)
        // ═══════════════════════════════════════════════════════════════════

        // ═══════════════════════════════════════════════════════════════════
        // Local helpers
        // ═══════════════════════════════════════════════════════════════════
        /*
        // mUUp : 128×128 → (lo128, hi128)
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static UInt128 BigMulUnsigned(UInt128 a, UInt128 b, out UInt128 lo) {
            unchecked {
                UInt64 a0 = (UInt64)a, a1 = (UInt64)(a >> 64);
                UInt64 b0 = (UInt64)b, b1 = (UInt64)(b >> 64);
                UInt128 a0b0 = (UInt128)a0 * b0;
                UInt128 a0b1 = (UInt128)a0 * b1;
                UInt128 a1b0 = (UInt128)a1 * b0;
                UInt128 a1b1 = (UInt128)a1 * b1;

                UInt64 word0 = (UInt64)a0b0;
                UInt64 word1 = (UInt64)(a0b0 >> 64);
                UInt64 word2 = (UInt64)a1b1;
                UInt64 word3 = (UInt64)(a1b1 >> 64);

                word1 = AddWithCarry(word1, (UInt64)a1b0, 0, out var c);
                word2 = AddWithCarry(word2, (UInt64)(a1b0 >> 64), c, out c);
                word3 = AddWithCarry(word3, 0, c, out c);

                word1 = AddWithCarry(word1, (UInt64)a0b1, 0, out c);
                word2 = AddWithCarry(word2, (UInt64)(a0b1 >> 64), c, out c);
                word3 = AddWithCarry(word3, 0, c, out _);

                lo = ((UInt128)word1 << 64) | word0;
                return ((UInt128)word3 << 64) | word2;
            }
        }
        */

        // rlshft (C) : logical right shift of a 3-word value by s ≥ 0.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void Rlshft3(ref InlineArrayNS.InlineArray3<UInt64> t, int s) {
            unchecked {
                int rs = s & 63;
                if (rs != 0) {
                    int ls = (-s) & 63;
                    t[0] = (t[0] >> rs) | (t[1] << ls);
                    t[1] = (t[1] >> rs) | (t[2] << ls);
                    t[2] = (t[2] >> rs);
                }
                while (s >= 64) {
                    t[0] = t[1];
                    t[1] = t[2];
                    t[2] = 0;
                    s -= 64;
                }
            }
        }

        public static int Log10_GetValue(UInt64 hi, UInt64 lo) {
            if (Misc.Unlikely(lo == 0)) {
                return Log10_GetValue_Shift48_n32(hi);
            } else {
                return Log10_GetValue_S48_4_N32(hi, lo);
            }
        }

        // h(x) = x >> 48,  n = 32  (byte table)
        public static int Log10_GetValue_Shift48_n32(UInt64 x) {
            int r = (int)((x >> 48) % 32);
            if (Log10_K_Shift48_n32[r] != x) return 0;
            return Log10_B_Shift48_n32[r];
        }

        static ReadOnlySpan<UInt64> Log10_K_Shift48_n32_flat => [
            0x40202A05F2000000UL,
            0x40415AF1D78B58C4UL,
            0x4002400000000000UL,
            0x402374876E800000UL,
            0x4044B1AE4D6E2EF5UL,
            0x4005900000000000UL,
            0x4026D1A94A200000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x4008F40000000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x402A2309CE540000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x400C388000000000UL,
            0x402D6BCC41E90000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x400F86A000000000UL,
            0x4030C6BF52634000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x4012E84800000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x40341C37937E0800UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x4016312D00000000UL,
            0x40376345785D8A00UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x40197D7840000000UL,
            0x403ABC16D674EC80UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x401CDCD650000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x403E158E460913D0UL,
            0xFFFFFFFFFFFFFFFFUL,
        ];

        static ReadOnlySpan<byte> Log10_B_Shift48_n32_flat => [
            10,
            20,
            1,
            11,
            21,
            2,
            12,
            0,
            3,
            0,
            13,
            0,
            4,
            14,
            0,
            5,
            15,
            0,
            6,
            0,
            16,
            0,
            7,
            17,
            0,
            8,
            18,
            0,
            9,
            0,
            19,
            0,
        ];

        // Log10 exact hash tables
        // h(k1, k2) = ((k1 >> 48) + (k2 >> 4)) % 32
        private static int Log10_GetValue_S48_4_N32(UInt64 key1, UInt64 key2) {
            int r = (int)(unchecked((key1 >> 48) + (key2 >> 4)) % 32UL);
            if (Log10_K1_S48_4_N32[r] != key1 || Log10_K2_S48_4_N32[r] != key2) return 0;
            return Log10_B_S48_4_N32[r];
        }

        static ReadOnlySpan<UInt64> Log10_K1_S48_4_N32_flat => [
            0x408078287F49C4A1UL,
            0x409B18427B3B4A05UL,
            0x406293E5939A08CEUL,
            0x4083D6329F1C35CAUL,
            0x409466BB7F0435C9UL,
            0x4065F8DEF8808B02UL,
            0x409E5E531A0A1C87UL,
            0x408725DFA371A19EUL,
            0x40480F0CF064DD59UL,
            0x40693B8B5B5056E1UL,
            0x408A6F578C4E0A06UL,
            0x404B52D02C7E14AFUL,
            0x406C8A6E32246C99UL,
            0x408DCB2D6F618C87UL,
            0x404EA784379D99DBUL,
            0x406FED09BEAD87C0UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x40911EFC659CF7D4UL,
            0x405208B2A2C28029UL,
            0x40733426172C74D8UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x40554ADF4B732033UL,
            0x4076812F9CF7920EUL,
            0xFFFFFFFFFFFFFFFFUL,
            0x40589D971E4FE840UL,
            0x4079E17B84357691UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x4097C06A5EC5433CUL,
            0x405C027E72F1F128UL,
            0x407D2CED32A16A1BUL,
            0xFFFFFFFFFFFFFFFFUL,
            0x405F431E0FAE6D72UL,
        ];

        static ReadOnlySpan<UInt64> Log10_K2_S48_4_N32_flat => [
            0xD6622FB2AB400000UL,
            0xBC8A8A4DE8459868UL,
            0x9DBD480000000000UL,
            0x4BFABB9F56100000UL,
            0xE717BB4500591500UL,
            0x452C9A0000000000UL,
            0x2BAD2CE16256FE82UL,
            0x6F7CB54395CA0000UL,
            0x2000000000000000UL,
            0x6B3BE04000000000UL,
            0x0B5BE2947B3C8000UL,
            0x6800000000000000UL,
            0xC60AD85000000000UL,
            0x8E32DB399A0BA000UL,
            0x4200000000000000UL,
            0x378D8E6400000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0xB8DFC90400474400UL,
            0x0940000000000000UL,
            0x22B878FE80000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x4B90000000000000UL,
            0x2B66973E20000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x1E74000000000000UL,
            0xB6403D0DA8000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x60DDAA16406F5A40UL,
            0x1308800000000000UL,
            0x11E8262889000000UL,
            0xFFFFFFFFFFFFFFFFUL,
            0x17CAA00000000000UL,
        ];

        static ReadOnlySpan<byte> Log10_B_S48_4_N32_flat => [
            39,
            47,
            30,
            40,
            45,
            31,
            48,
            41,
            22,
            32,
            42,
            23,
            33,
            43,
            24,
            34,
            0,
            44,
            25,
            35,
            0,
            26,
            36,
            0,
            27,
            37,
            0,
            46,
            28,
            38,
            0,
            29,
        ];
        // ── Log2E192 : log2(e)·2^192, 6 words ──────────────────────────────
        static ReadOnlySpan<UInt64> Log2E192_flat => [
            0xd6aef551bad2b4b1UL,
            0x7d0ffda0d23a7d11UL,
            0x71547652b82fe177UL,
            0x0000000000000001UL,
            0x0000000000000000UL,
            0x0000000000000000UL,
        ];

        // ── Log10E192 : log10(e)·2^192, 6 words ────────────────────────────
        static ReadOnlySpan<UInt64> Log10E192_flat => [
            0x1f71a30122e4d101UL,
            0x9aadd557d699ee19UL,
            0x6f2dec549b9438caUL,
            0x0000000000000000UL,
            0x0000000000000000UL,
            0x0000000000000000UL,
        ];

        // ── Iln2Log2Of10Half_7 : (log2(10)/2)·2^447, 7 words ───────────────
        static ReadOnlySpan<UInt64> Iln2Log2Of10Half_flat => [
            0x1435ee480565ca55UL,
            0x3e24beab63ad0bd6UL,
            0x06650fde9dd51f3aUL,
            0x91b6ac8082e7859dUL,
            0xd96c55fe37b3ad4eUL,
            0x492bf6ff4dafdb4cUL,
            0xd49a784bcd1b8afeUL,
        ];

        // ═══════════════════════════════════════════════════════════════════
        // Data tables — transcribed verbatim from the C reference
        // ═══════════════════════════════════════════════════════════════════
        static ReadOnlySpan<UInt64> LogLn2A_flat => [
            0x3F2F6AF40F343267UL, 0x1CF79ABC9E3B3980UL, 0x0000000B17217F7DUL
            ];

        static ReadOnlySpan<UInt64> LogOffA_flat => [
            0x0198BFB39C605BC5UL, 0x718289F4C0A895F2UL, 0xFFFFFFFD3568989AUL
        ];

        static ReadOnlySpan<UInt32> LogRt0_flat => [
            0x80000000, 0x7d41d96e, 0x7a92be8b, 0x77f25cce, 0x75606374, 0x72dc8374, 0x70666f77, 0x6dfddbcc,
      0x6ba27e66, 0x69540ec9, 0x6712460b, 0x64dcdec4, 0x62b39509, 0x60962666, 0x5e8451d0, 0x5c7dd7a4,
      0x5a82799a, 0x5891fac1, 0x56ac1f76, 0x54d0ad5b, 0x52ff6b55, 0x51382182, 0x4f7a9931, 0x4dc69cdd,
      0x4c1bf829, 0x4a7a77d5, 0x48e1e9ba, 0x47521cc6, 0x45cae0f2, 0x444c0741, 0x42d561b4, 0x4166c34d,
        ];
        static ReadOnlySpan<UInt32> LogRt1_flat => [
            0x80000000, 0x7fe9d3a9, 0x7fd3ab2a, 0x7fbd8680, 0x7fa765ad, 0x7f9148af, 0x7f7b2f86, 0x7f651a31,
      0x7f4f08af, 0x7f38faff, 0x7f22f122, 0x7f0ceb16, 0x7ef6e8db, 0x7ee0ea6f, 0x7ecaefd4, 0x7eb4f906,
      0x7e9f0607, 0x7e8916d5, 0x7e732b70, 0x7e5d43d7, 0x7e476009, 0x7e318006, 0x7e1ba3cd, 0x7e05cb5e,
      0x7deff6b7, 0x7dda25d8, 0x7dc458c1, 0x7dae8f71, 0x7d98c9e7, 0x7d830822, 0x7d6d4a22, 0x7d578fe6,
        ];
        static ReadOnlySpan<UInt32> LogRt2_flat => [
            0x80000000, 0x7fff4e8f, 0x7ffe9d1e, 0x7ffdebaf, 0x7ffd3a40, 0x7ffc88d2, 0x7ffbd765, 0x7ffb25f9,
      0x7ffa748e, 0x7ff9c325, 0x7ff911bc, 0x7ff86054, 0x7ff7aeed, 0x7ff6fd86, 0x7ff64c21, 0x7ff59abd,
      0x7ff4e95a, 0x7ff437f8, 0x7ff38696, 0x7ff2d536, 0x7ff223d7, 0x7ff17278, 0x7ff0c11b, 0x7ff00fbe,
      0x7fef5e63, 0x7feead08, 0x7fedfbaf, 0x7fed4a56, 0x7fec98fe, 0x7febe7a8, 0x7feb3652, 0x7fea84fd,
        ];
        static ReadOnlySpan<UInt32> LogRt3_flat => [
            0x80000000, 0x7ffffa75, 0x7ffff4e9, 0x7fffef5e, 0x7fffe9d2, 0x7fffe447, 0x7fffdebb, 0x7fffd930,
      0x7fffd3a4, 0x7fffce18, 0x7fffc88d, 0x7fffc301, 0x7fffbd76, 0x7fffb7ea, 0x7fffb25f, 0x7fffacd3,
      0x7fffa748, 0x7fffa1bc, 0x7fff9c30, 0x7fff96a5, 0x7fff9119, 0x7fff8b8e, 0x7fff8602, 0x7fff8077,
      0x7fff7aeb, 0x7fff7560, 0x7fff6fd4, 0x7fff6a49, 0x7fff64bd, 0x7fff5f31, 0x7fff59a6, 0x7fff541a,
        ];

        static ReadOnlySpan<UInt64> LogLt0A_flat => [
            0,0,0,0xa89586efcf459616,0xbf2d9d6938e7957,0x58b9,
  0x1c4217ac03d7f347,0x17e97ec3bbbe7fa5,0xb172,0xefb93ff266057747,0x23f162a58989f516,0x10a2b,
  0xe785fee03979770c,0x2fed4367cab6f34c,0x162e4,0xb7178705eb425d19,0x3be26415cb5ffca2,0x1bb9d,
  0x99d386fe1e67fe52,0x47c60c5651fc6826,0x21456,0xdc33ca27ae76c0e3,0x53e0ac68af0f8d45,0x26d0f,
  0xb920987460ccab06,0x5fc92ccd6bb8f0a7,0x2c5c8,0x1668340ee9fe5a93,0x6bda0fbed4c7f913,0x31e81,
  0x8ea18591547f82b,0x77c5aca6064ac9d5,0x3773a,0x10be2ce034ff1e0b,0x83b323296e47fd8a,0x3cff3,
  0xe27d52ae22538cbb,0x8fc10f43ea11902b,0x428ac,0x6d1339a150080f6b,0x9ba8e78a4a916975,0x48165,
  0x6cb99b86deff0b66,0xa7b88577d11699be,0x4da1e,0x88b410857d77a113,0xb3b510d166b0bfcd,0x532d7,
  0x9328234a71b9e39e,0xbfbe03bfa45e53d0,0x58b90,0xa9113370e6c95819,0xcbb65f48e215a1be,0x5e449,
  0xe92c7c696e7f112c,0xd78d448cda05aa76,0x63d02,0xc391aa17bb2e306d,0xe3981c232f9c13af,0x695bb,
  0x938a7d8cffa8155e,0xefa69c9432396c18,0x6ee74,0xf2a451564d1c4467,0xfb92199c68d6317e,0x7472d,
  0x298ed4e3413defd5,0x7812354074df8f7,0x79fe7,0xb9020c95072a113f,0x139d8899037598bf,0x7f8a0,
  0xfac4d6a530c11dcb,0x1f91d2a304280c6d,0x85159,0x361eba5f7b210f8e,0x2b7e22b48dc9c3ac,0x8aa12,
  0x67b013198bff7bb,0x378c54fb8df946f4,0x902cb,0x668e61761f70153b,0x437cadb3f378d692,0x95b84,
  0xa7e6655ea0f729e3,0x4f8b03cdfc0665f2,0x9b43d,0x67731aa1f81e8841,0x5b5e9cb8ea72447e,0xa0cf6,
  0x1a7b7c5aef8482a9,0x677f1a61810b457c,0xa65af,0xa5e22a09ade4180e,0x7357a288fc7e3de4,0xabe68,
        ];

        static ReadOnlySpan<UInt64> LogLt1A_flat => [
              0,0,0,0xfcd4886923dae6b6,0xc85bee167cab86ce,0x2c5,
  0x635e6b704ed2f5fa,0x90a17ceecc3fe577,0x58b,0xdf932da91371dd2,0x591b7d0a4d0016a7,0x851,
  0x73159a852516c020,0x2174877366ab0cec,0xb17,0x6daf0fee571d4b99,0xe9d76cbddb12b19d,0xddc,
  0xc537f3bccf1bcdda,0xb22edc3896d8e511,0x10a2,0x7acd5f3c275334bc,0x7a85a1047f10c872,0x1368,
  0xcf11c6053e5e8a85,0x42e69173390188dc,0x162e,0x5263d7113ff4ec99,0xb5c8f0cfa3bcd0d,0x18f4,
  0xb72b8544c29a9cca,0xd3b2174ebd216daa,0x1bb9,0xc46fc691949a5aab,0x9c123df6aaf7611b,0x1e7f,
  0xc10a731729fcd789,0x6467b2b9aea50be5,0x2145,0x893fa461cbbd9064,0x2cddab05d780030b,0x240b,
  0x34c38552cb1a0b96,0xf51e45f22e4b220e,0x26d0,0x70f1813d88d0193b,0xbd95605269b91143,0x2996,
  0x73e5b8717251d0f7,0x85ed0e45f6c8616d,0x2c5c,0x7f5ae5d5b9581561,0x4e50a741a891eeb4,0x2f22,
  0x56594d2d9101bad3,0x16aadba73393c1a7,0x31e8,0xef4a8eb0c15fbf7a,0xdf06bacc0921d568,0x34ad,
  0x5d054864b92a7751,0xa76f5f65463c79f7,0x3773,0x7a5575cf1df04df6,0x6fcf7a45d5191939,0x3a39,
  0x9618ad49bb912343,0x38322c12b8541a4a,0x3cff,0x2edce206b7063305,0x822052fab58fd5,0x3fc5,
  0x2e80337b06ed4e80,0xc8eb03cd67c0701c,0x428a,0xd46ea81c5e577187,0x91578da94328a582,0x4550,
  0x8705f6efe46e502b,0x59b269f9f7d32471,0x4816,0xfbdb66ebecf00a88,0x2206d0d3e982fcd2,0x4adc,
  0xc819e2a66efb2206,0xea6005c8e6da3826,0x4da1,0xed64886cdf895904,0xb2c957ee1aac3666,0x5067,
  0xb1f969dae7edc113,0x7b2d79d3afecbfbe,0x532d,0x1b3b44759b966886,0x4397c0abdea76dc5,0x55f3,
        ];

        static ReadOnlySpan<UInt64> LogLt2A_flat => [
            0,0,0,0x9cf3bc3d208c9185,0x2e2f5fbcb16ff027,0x16,
  0xda087301ff6274d4,0x5c7d7f2b9b75afe0,0x2c,0x134a54f71a0236c3,0x8aaa5d97d2591b8f,0x42,
  0xd9c68a61183ce86a,0xb8f5fbaf4fb0944a,0x58,0x9c822a201eb1fc42,0xe74058e9835896c7,0x6e,
  0x5441bb438a9fa9a5,0x1589751695c42b17,0x85,0xbd2d2405a0df752a,0x43d15006aea6e5e2,0x9b,
  0xe419cb9011950b79,0x7217e989f4f4e681,0xb1,0x534a67bfdf41cfa2,0xa03d3fe1448d8184,0xc7,
  0x51f2181bf5cf5458,0xce8155cef8c016e6,0xdd,0x2db9191ec9fc2eb3,0xfcc429c04a827e7e,0xf3,
  0xcd759d3f967fc6f7,0x2b05bb855dcaef6b,0x10a,0x95ef9cbd1c7b13c7,0x59660d2f1cc94616,0x120,
  0x2af332ca6cc1b350,0x87a51a387bbee256,0x136,0x7948f090babd87b8,0xb5e2e48603db2b0e,0x14c,
  0xc53979d14662ef59,0xe41f6be7d6165086,0x162,0xeabba1758308d7b9,0x125ab02e12a8fc92,0x179,
  0xbecf1149da50de5e,0x40b4b4478141a2f2,0x18f,0x477f919cf0a27011,0x6eed71f351129ea7,0x1a5,
  0xba7e797e9eb461b6,0x9d24ebf3e7a2e2a7,0x1bb,0x3a5e7941bef2e816,0xcb7b25bd2dfd7f94,0x1d1,
  0xc9b5422a00ceabbf,0xf9b018040a909da4,0x1e7,0xcade12d9d3e36920,0x2803ca0c93032d7e,0x1fe,
  0x222f102eb784552b,0x563633da2572876c,0x214,0x75d47798f62ff1c7,0x84875d625dc415f0,0x22a,
  0x63e729d32c4f6e9e,0xb2b73df70fec66b5,0x240,0x7b5afeebc54c49ab,0xe105de3f5fe748eb,0x256,
  0x221ca10aac8a3b6a,0xf5339b6125ee838,0x26d,0x9e9e828d80c1cf32,0x3d7f4b245f1a3bff,0x283,
  0x88ad9ab83fc30a52,0x6bca1c3bb9eb9390,0x299,0x4d28b6b2476ee8f8,0x9a13a7f1bc8c0d71,0x2af,
        ];

        static ReadOnlySpan<UInt64> LogLt3A_flat => [
            0,0,0,0x347fb733d023cc27,0xb16003d72f3c6259,0,
  0x8679ca1f8e0297b8,0x62e00f5f83035049,1,0x5490ab9896a762f9,0x14402294d37ee9e5,2,
  0x5e57ae0f99d9a1e6,0xc5c03d7e0f9a82c2,2,0x78d5d5583fad6e57,0x7720601183ff07b0,3,
  0xce2ece6e0065f25a,0x28a08a5bab197ade,4,0x17fd57b0c9f4117d,0xda00bc4d46109fe0,4,
  0x9cdc64425d00b1f6,0x8b80f5f85ad41d75,5,0x3ae1d5be90ad7893,0x3d01375498ec7536,6,
  0x12009a12eb106fa8,0xee618054241e50c0,6,0x1c52307d1547e6a6,0x9fe1d11153df1b17,7,
  0xf679672ea03b5b71,0x5142296f0c4bfc59,8,0xa28c66c32c76df90,0x2c2898d305f6b18,9,
  0x94dd6b2c7f525610,0xb422f14918b10941,9,0x692d440a10a891f7,0x65a360c833c14eea,0xa,
  0xb9f6c9a02410801a,0x1703d7e24ea161d7,0xb,0x8e428b3fdd4ff401,0xc88456c26358b19e,0xb,
  0x798bb05020bcb8ca,0x7a04dd53a7646701,0xc,0xb2c5d2cbf27d3f39,0x2b656b7bc4797fa9,0xd,
  0xee8203ed92fd41cf,0xdce6016e062bac06,0xd,0xfb1760c891901812,0x8e469ef45c77a6e2,0xe,
  0xb33a3b423eeaedd0,0x3fc744479e7a7f29,0xf,0xf790771b31f0c07,0xf127f12c30a71684,0xf,
  0xc210dfa087d58623,0xa2a8a5e075a4d054,0x10,0x1c8901c916301142,0x54096223465bbf29,0x11,
  0x99fc5710ac4e702e,0x58a263890fe90d5,0x12,0xd3bcd06f88ddb2f1,0xb6eaf1d9a2e992d0,0x12,
  0x3f07c144adf4e354,0x686bc54ff5dbb35c,0x13,0x288d50e0d51ab78a,0x19eca0777f77067f,0x14,
  0x3acdd4c172d84d84,0xcb4d8326a9902bfb,0x14,0x81f0878a8d99e057,0x7cce6daf3f7a4090,0x15,
        ];

        static ReadOnlySpan<UInt64> LogC_flat => [
            0xFFFF_FFFF_FFFF_FFFFUL, 0xFFFF_FFFF_FFFF_FFFFUL,
        0xFFFF_FFFF_FFFF_FFFFUL, 0x0000_07FF_FFFF_FFFFUL,
        0x5555_5555_5555_5551UL, 0x0000_0000_0055_5555UL,
        0xFFFF_FFFF_FFFF_FFEAUL, 0x0000_0000_0000_0003UL,
        0x0000_3333_3333_32F7UL, 0x0000_0000_0000_0000UL,
        0x0000_0000_02AA_AA5EUL, 0x0000_0000_0000_0000UL
        ];

        static ReadOnlySpan<UInt64> LogCp_flat => [
          0xffffffffffffffffUL, 0x7fffffffffffffffUL,
      0x5555555555555555UL, 0x0000055555555555UL,
      0xfffffffffffffffdUL, 0x00000000003fffffUL,
      0x3333333333333326UL, 0x0000000000000003UL,
      0x00002aaaaaaaaa84UL, 0x0000000000000000UL,
      0x000000000249245aUL, 0x0000000000000000UL
        ];
        static ReadOnlySpan<UInt64> LogCn_flat => [
           0x0000000000000000UL, 0x8000000000000000UL,
      0xaaaaaaaaaaaaaaaaUL, 0x000002aaaaaaaaaaUL,
      0xffffffffffffffffUL, 0x00000000000fffffUL,
      0x6666666666666668UL, 0x0000000000000000UL,
      0x000002aaaaaaaaa8UL, 0x0000000000000000UL,
      0x0000000000124926UL, 0x0000000000000000UL
        ];

        static ReadOnlySpan<UInt64> LogC37_flat => [
            0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x0000000000000000, 0x8000000000000000,
            0x2acaa97da57cadbe, 0xf3dc3b1036f5d64c, 0xc5068badc5d57d15, 0xa079a193394c5b16, 0xe4f1d9cc01f97b57, 0x58b90bfbe8e7bcd5,
            0xd344071b5ec47714, 0x524eb0376b9686e0, 0xc2be93bbb1396e6e, 0xa3a2751c30ce69d4, 0x6f16b06ec9735fca, 0x1ebfbdff82c58ea8,
            0x80960837d68b3044, 0x44545f2e4dd67d06, 0x4753198ade236146, 0xa7ae23a226d00887, 0xcce9d8aeccaf4b7b, 0x071ac235c1282fe2,
            0xeee877628d8f6274, 0x8f7e8887e73829d7, 0x699b709699e1815f, 0x72478ea53e63911d, 0x9ccbbe0b53eeac50, 0x013b2ab6fba4e772,
            0x318a9f63b2f3ec5c, 0x2f040f926e2c93be, 0xc8cdc36aa406093f, 0x60aed94d2dce32e1, 0x20e2fed34a297d86, 0x002bb0ffcf14ce62,
            0x2d3c7b9299d0d3e1, 0x91ec7aec4e3f0a35, 0x549592ff6d6ab786, 0xcfb314ffccc47bb0, 0xdbd2c2a261ac8d07, 0x00050c244be1b1e1,
            0xcd46dc2cd5899528, 0x95ce94549c9a636a, 0x5d3119327fac831d, 0x4ace1152e8810fee, 0x1a1ac547321f639a, 0x00007ff2ff1622c3,
            0x8531a13788143560, 0x5c5f54178a891b6b, 0x7d5b21cd05968068, 0x2586e1a0f7107ab9, 0x11fec7ff3036d3be, 0x00000b160111d2e4,
            0xe590c3fdfb446be7, 0x2d5087b376c0214d, 0x738afd2a4917377a, 0x84518cb8caa9ba26, 0x3e1ed253872d27fc, 0x000000da929e9caf,
            0xcc782de6ff640fd0, 0x68569686f1264ea6, 0x98468b24acd303be, 0xb4ce2a0608a16570, 0xc764fb7ed0eca973, 0x0000000f267a8ac5,
            0xee22a537a6d9a8b8, 0x08110963d1d9380f, 0x43b147aa9f2c11b5, 0x90a4edc7b3f29c1c, 0x8dd92607abccaf23, 0x00000000f465639a,
            0x38202f8bf18f1943, 0xbc2fb66e0bf68e76, 0x511f7698b91cba26, 0xd17730e76aaedf16, 0x7e14c2f15ab43f0b, 0x000000000e1deb28,
            0xea93b5cedf528c2f, 0x6ee667b44f788f66, 0x714aa5175cd8c9ab, 0x7b8d8ce6fe81f087, 0x8b3687cb140d6180, 0x0000000000c0b0c9,
            0x88ad1fe5bcec581b, 0xa772e0b581376df7, 0x1d1edb9f5248282a, 0x2ae5761242913279, 0x26ac3c54b9f8a1b1, 0x0000000000098a4b,
            0x9d19faf9f3a16e65, 0xc2fde1c74321a361, 0x3787e55bad70a464, 0xfc6a729280d47c69, 0xa10ec1008799ec55, 0x00000000000070db,
            0xf708ee5bf7061dee, 0x921cfcdbbccf2eb0, 0x7cd64e11a79215fe, 0x93b26377e3b574bd, 0xa26b9e7e2ce48e3f, 0x00000000000004e3,
            0x9bb4d4ccb609d71e, 0x6ef9409a4cd3ac24, 0xc560a32b4349d4dd, 0xb1001eff03e83f71, 0x088968384b4faac3, 0x0000000000000033,
            0xb5c5f81b7e235f48, 0x9c1837383fef6aa4, 0xcf80a6c83ea7dc89, 0x2ed389743c9aa15c, 0xf7176bdb43695d73, 0x0000000000000001,
            0x4e3f1412be77b88a, 0x75f070df1338e5db, 0xd970f6d684dcca91, 0xe056ab257aa7f274, 0x125a7ecb835c64da, 0x0000000000000000,
            0x6c8a502410b3a92b, 0x5ae22ff5b3b87c53, 0x5510cef268f5a4f6, 0x27126f802bc39d75, 0x00a2d6625a8289ac, 0x0000000000000000,
            0xa851c04a8293fece, 0x95e77dec82b8eb58, 0xef60eb192fe67eda, 0xe0aadf36ddf86f4f, 0x00055ff15e0f8271, 0x0000000000000000,
            0x7e2f1903476d3f72, 0x991510e40164f6ec, 0xb93a9cf471520656, 0x81d8f91aed9d0512, 0x00002b59f5a6e03b, 0x0000000000000000,
            0xe661b8b0f04bd98d, 0xc0c7162d7c443b27, 0x48eb10aa6d041426, 0xfb5cb2465d33a354, 0x0000014e7515dc98, 0x0000000000000000,
            0xa2de610b24affa88, 0x36d51a1c2cf018ae, 0xe823660bee34df2b, 0xd5e52102fa4f3d47, 0x00000009a8d57b65, 0x0000000000000000,
            0x8297d75925cf8a0e, 0xb19c48d3a13188f6, 0x08ec765f339c0710, 0x6a09e41e7a3814f9, 0x00000000448fbf65, 0x0000000000000000,
            0x11744278158dd70d, 0xd10b922fb4d1b574, 0xdd8263d932a8a20c, 0x99f1cc682af884c5, 0x0000000001d3ebc2, 0x0000000000000000,
            0x20127ee09bb9f25c, 0x5ed98d7cde9ecace, 0x46da42907892bb39, 0x9af8dbd36a3b0863, 0x00000000000c0334, 0x0000000000000000,
            0xafd75d37c82b15ca, 0xf0acd37674cf3129, 0xae8a3fb59540ab8d, 0xa3dfeb50d4131639, 0x0000000000004c20, 0x0000000000000000,
            0xdaaf1913e1cb12f0, 0xe352ea644fb69c62, 0xf3fac0750b414cc5, 0xcf69a29f13de20e8, 0x00000000000001d1, 0x0000000000000000,
            0x0587a7d1a7ec6c45, 0x05eaca3704dcd2f6, 0x12dcbe8116eada8f, 0xc333445e3155f79b, 0x000000000000000a, 0x0000000000000000,
            0x183f54470927dbcd, 0x76bc8f5b8e8b7093, 0x8f099fb908474b26, 0x3d9aea5b4d0ebfae, 0x0000000000000000, 0x0000000000000000,
            0x261c179f4bdffa1d, 0x0909a0681e2103a8, 0x4dbdd766d2d9a092, 0x01559c86508b1539, 0x0000000000000000, 0x0000000000000000,
            0x3816a7ca0ca85578, 0x6b7fd2696989cdee, 0xd34dacabb2d48a6c, 0x00072ce49e65a3ac, 0x0000000000000000, 0x0000000000000000,
            0xd4e29d35fe14e115, 0xc30d08011bdafd19, 0xa9a6bf19955b2132, 0x00002572be492f62, 0x0000000000000000, 0x0000000000000000,
            0xe4df41884be86bb5, 0x0c5772ee960fd5bf, 0x9dbee3b9355d1fb0, 0x000000bdd01d5d84, 0x0000000000000000, 0x0000000000000000,
            0x80446608d32c4ed3, 0xb4d5892fe7709c19, 0xd3840403fcefe8a9, 0x00000003bc4fb6d4, 0x0000000000000000, 0x0000000000000000];

        private static T LoadUnalignedInternal<T, TElement>(ReadOnlySpan<TElement> data) {
            if (Unsafe.SizeOf<T>() > checked(Unsafe.SizeOf<T>() * data.Length)) {
                throw new InvalidOperationException();
            }
            return Unsafe.ReadUnaligned<T>(ref Unsafe.As<TElement, byte>(ref MemoryMarshal.GetReference(data)));
        }
        internal static T LoadUnaligned<T>(ReadOnlySpan<sbyte> data) => LoadUnalignedInternal<T, sbyte>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<byte> data) => LoadUnalignedInternal<T, byte>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<short> data) => LoadUnalignedInternal<T, short>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<ushort> data) => LoadUnalignedInternal<T, ushort>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<char> data) => LoadUnalignedInternal<T, char>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<int> data) => LoadUnalignedInternal<T, int>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<uint> data) => LoadUnalignedInternal<T, uint>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<float> data) => LoadUnalignedInternal<T, float>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<ulong> data) => LoadUnalignedInternal<T, ulong>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<long> data) => LoadUnalignedInternal<T, long>(data);
        internal static T LoadUnaligned<T>(ReadOnlySpan<double> data) => LoadUnalignedInternal<T, double>(data);

        static ReadOnlySpan<UInt64> LogTbl6_flat => [
            0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x0000000000000000UL, 0x8000000000000000UL,
            0x82DC9BF3421840D1UL, 0x7835AF9AB4E6355CUL, 0x5D42B362AF1EE859UL, 0x148A0459E7585151UL, 0xC5C95B8C2154C1B2UL, 0x85AAC367CC487B14UL,
            0x9994FAC7F4EA654BUL, 0xB4F28C879A524BD4UL, 0x91E135EE84A3F733UL, 0x1AA84FFBEBAC349FUL, 0xFBE4628758A53C90UL, 0x8B95C1E3EA8BD6E6UL,
            0x23B504FB5DADBB25UL, 0x4E0D990C27C43643UL, 0xF1203CAF65BFB9B9UL, 0x1942B34816FB4F26UL, 0x0FD6D8E0AE5AC9D8UL, 0x91C3D373AB11C336UL,
            0x4E9F94EE0C90DBBDUL, 0xBE47C34D380250A8UL, 0xD78B65CBEFA7BB6FUL, 0x5E139A1B14FA8178UL, 0x46AD23182E42F6F6UL, 0x9837F0518DB8A96FUL,
            0xA264696B6F6E2B1CUL, 0xCD12E5ADA91EEF7BUL, 0x21F977FE7C7FA117UL, 0x65C15C122133E2A2UL, 0xA0911F09EBB9FDD1UL, 0x9EF5326091A111ADUL,
            0xAD35E8E58C5CE4F4UL, 0x5343B4A0330A8052UL, 0x2589C98A8290D3F0UL, 0x1DD170ACE2BCFC17UL, 0x1CBD7F621710701BUL, 0xA5FED6A9B15138EAUL,
            0x739A3D061FEA7592UL, 0x458FD5F44E26A7A7UL, 0xB165F141833A67DAUL, 0x6BE409407034FDEDUL, 0x4980A8C8F59A2EC4UL, 0xAD583EEA42A14AC6UL,
            0xA8B1FE6FDC83DB39UL, 0x4AFC83043AB8A2C3UL, 0xED17AC8583339915UL, 0x1D6F60BA893BA84CUL, 0x597D89B3754ABE9FUL, 0xB504F333F9DE6484UL,
            0x283C66DB56DF97F8UL, 0x9B985F3A0EAE1C1DUL, 0x0D9A4BE023ECE031UL, 0x15B34BBCB0298F41UL, 0xA8811FB66D0FAF7AUL, 0xBD08A39F580C36BEUL,
            0xEFBA190ED1A58A51UL, 0xBB2068BE237512DFUL, 0xC7686006E4E6C092UL, 0x6B0F939998251A36UL, 0x3E2AD0C964DD9F37UL, 0xC5672A115506DADDUL,
            0xCD8CAEBC5D894C26UL, 0x1B8343088BBDADD4UL, 0x2BBD398AF35C079FUL, 0x6F28610B8C36485AUL, 0xE235838F95F2C6EDUL, 0xCE248C151F8480E3UL,
            0x81247458FB61E576UL, 0xEFB01FDA334BCA9AUL, 0xB5C13ADA0E778299UL, 0x1D733AF522058B16UL, 0x39A68BB9902D3FDEUL, 0xD744FCCAD69D6AF4UL,
            0xF44E17262DA49B5DUL, 0x188081FE7062F61EUL, 0x1CB99D3F1FF298A2UL, 0x224B251B33092002UL, 0x065895048DD333CAUL, 0xE0CCDEEC2A94E111UL,
            0x091482854D819E44UL, 0xB338FCD2AC2FFBC8UL, 0x17D8D1E8CA31880AUL, 0xC4FAACE043B7F91CUL, 0xD02D75B3706E54FAUL, 0xEAC0C6E7DD24392EUL,
            0x29F075F23730E918UL, 0xD22DD036F1906094UL, 0xBDD80329364AA29FUL, 0x6F510308677709F5UL, 0x7B9D0C7AED980FC3UL, 0xF5257D152486CC2CUL
        ];

        static ReadOnlySpan<UInt32> LogTbl32_flat => [
            0xB8AA3AF4u, 0xB8A9CF24u, 0xF5EC11E6u, 0x056CDE26u,
        0xB311AD8Du, 0xADA42C83u, 0xE04081E5u, 0x04CE4AA7u,
        0xADCD64B4u, 0xA393D29Cu, 0xCD0DF3B5u, 0x0445B73Bu,
        0xA8D62754u, 0x9A5D2082u, 0xBBFCA4EDu, 0x03CF93B1u,
        0xA42589CEu, 0x91E83E37u, 0xACC3070Bu, 0x0368F7A9u,
        0x9FB5D233u, 0x8A208126u, 0x9F23232Cu, 0x030F8032u,
        0x9B81E0E3u, 0x82F3ED8Du, 0x92E88678u, 0x02C13522u,
        0x97851CC6u, 0x7C52CE34u, 0x87E698B1u, 0x027C744Du,
        0x93BB6276u, 0x762F5E29u, 0x7DF7458Bu, 0x023FE121u,
        0x9020F5EBu, 0x707D8107u, 0x74F9E730u, 0x020A57BFu,
        0x8CB2762Au, 0x6B32870Bu, 0x6CD26447u, 0x01DAE2AAu,
        0x896CD2ADu, 0x6644FAD1u, 0x656876FFu, 0x01B0B28Cu,
        0x864D4241u, 0x61AC76EFu, 0x5EA714E9u, 0x018B1793u,
        0x83513B19u, 0x5D618220u, 0x587BF145u, 0x01697C1Au,
        0x80766BE7u, 0x595D70C9u, 0x52D714A2u, 0x014B6048u,
        0x7DBAB5DEu, 0x559A4B07u, 0x4DAA85E3u, 0x01305684u,
        0x7B1C276Au, 0x5212B66Cu, 0x48EA016Fu, 0x0118008Bu,
        0x7898F797u, 0x4EC1E2F3u, 0x448ABC05u, 0x01020D05u,
        0x762F8200u, 0x4BA37A8Au, 0x40832F23u, 0x00EE3590u,
        0x73DE4338u, 0x48B392E0u, 0x3CCAED5Bu, 0x00DC3D0Fu,
        0x71A3D59Eu, 0x45EEA114u, 0x395A7D42u, 0x00CBEE53u,
        0x6F7EEE92u, 0x43516F02u, 0x362B39D1u, 0x00BD1AF0u,
        0x6D6E5BEFu, 0x40D911EFu, 0x3337376Au, 0x00AF9A47u,
        0x6B7101D3u, 0x3E82E265u, 0x30792CADu, 0x00A348B4u,
        0x6985D8A7u, 0x3C4C750Au, 0x2DEC5E91u, 0x009806E0u,
        0x67ABEB4Fu, 0x3A33945Du, 0x2B8C8F3Cu, 0x008DB92Cu,
        0x65E25598u, 0x38363B33u, 0x2955EF28u, 0x00844732u,
        0x642842CAu, 0x36528FD2u, 0x2745104Eu, 0x007B9B57u,
        0x627CEC58u, 0x3486DFA6u, 0x2556DAEFu, 0x0073A270u,
        0x60DF98BBu, 0x32D19B70u, 0x238883E0u, 0x006C4B75u,
        0x5F4F9A66u, 0x313153DEu, 0x21D78407u, 0x00658739u,
        0x5DCC4ECEu, 0x2FA4B68Cu, 0x204190E7u, 0x005F4832u
        ];

        static ReadOnlySpan<UInt64> LogC0_flat => [
            0x00000000111843D8UL, 0x0B5D69BAD62FA11AUL, 0x1663F6FAD5C18924UL, 0x2118B119BFF1FA3BUL,
        0x2B803474013E4469UL, 0x359EBC5B7234BAEDUL, 0x3F782D720C23D39BUL, 0x49101EAC3E8EE930UL,
        0x5269E12F3A1E46C3UL, 0x5B888736793C6255UL, 0x646EEA2480D45E3CUL, 0x6D1FAFDCE6051F08UL,
        0x759D4F80CF356A9BUL, 0x7DEA15A32F48CB06UL, 0x86082806B4AEF51CUL, 0x8DF988F4B11089FEUL,
        0x95C01A39FE25B941UL, 0x9D5D9FD503210D19UL, 0xA4D3C25E6ABF605BUL, 0xAC241134C69F89B6UL,
        0xB35004723DD423A6UL, 0xBA58FEB271A49370UL, 0xC1404EADF4CD88F9UL, 0xC80730B00143AD7BUL,
        0xCEAECFEA8199248CUL, 0xD53847AC01A300FFUL, 0xDBA4A47AAA7E5E5BUL, 0xE1F4E5170DD764E5UL,
        0xE829FB6931086C4EUL, 0xEE44CD5A005FB44DUL, 0xF446359B13F9EDEFUL, 0xFA2F045E78CC51CCUL
        ];
    }
}
