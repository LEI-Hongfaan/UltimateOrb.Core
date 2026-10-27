using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb.Numerics {
#if NET8_0_OR_GREATER
    using Int128 = System.Int128;
    using UInt128 = System.UInt128;
#endif

    [Experimental("UoWIP")]
    public static partial class BigRationalMathF {

        //TODO:
        // ================================================================
        //  sNaN detection / quieting.
        // ================================================================
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSignalingNaN<T>(T v) where T : IFloatingPointIeee754<T> {
            if (!T.IsNaN(v)) return false;
            if (typeof(T) == typeof(double)) return IsSNaN_Binary64(Unsafe.As<T, double>(ref v));
            if (typeof(T) == typeof(float)) return IsSNaN_Binary32(Unsafe.As<T, float>(ref v));
            if (typeof(T) == typeof(Half)) return IsSNaN_Binary16(Unsafe.As<T, Half>(ref v));
            if (typeof(T) == typeof(Quadruple)) return IsSNaN_Binary128(Unsafe.As<T, Quadruple>(ref v));
            return false;
        }

        private static bool IsSNaN_Binary16(Half v) {
            ushort b = Unsafe.As<Half, ushort>(ref v);
            return (b & 0x7E00) == 0x7C00 && (b & 0x01FF) != 0;
        }
        private static bool IsSNaN_Binary32(float v) {
            uint b = BitConverter.SingleToUInt32Bits(v);
            return (b & 0x7FC00000U) == 0x7F800000U && (b & 0x003FFFFFU) != 0;
        }
        private static bool IsSNaN_Binary64(double v) {
            ulong b = BitConverter.DoubleToUInt64Bits(v);
            return (b & 0x7FF8000000000000UL) == 0x7FF0000000000000UL
                && (b & 0x0007FFFFFFFFFFFFUL) != 0;
        }
        private static bool IsSNaN_Binary128(Quadruple v) {
            UInt128 b = BitConverter.QuadrupleToUInt128Bits(v);
            UInt128 e = (UInt128)0x7FFF << 112;
            UInt128 q = (UInt128)1 << 111;
            return (b & (e | q)) == e && (b & (q - 1)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T QuietNaN<T>(T v) where T : IFloatingPointIeee754<T> {
            if (typeof(T) == typeof(double)) {
                ulong b = BitConverter.DoubleToUInt64Bits(Unsafe.As<T, double>(ref v)) | (1UL << 51);
                return (T)(object)BitConverter.UInt64BitsToDouble(b);
            }
            if (typeof(T) == typeof(float)) {
                uint b = BitConverter.SingleToUInt32Bits(Unsafe.As<T, float>(ref v)) | (1U << 22);
                return (T)(object)BitConverter.UInt32BitsToSingle(b);
            }
            if (typeof(T) == typeof(Quadruple)) {
                UInt128 b = BitConverter.QuadrupleToUInt128Bits(Unsafe.As<T, Quadruple>(ref v)) | ((UInt128)1 << 111);
                return (T)(object)BitConverter.UInt128BitsToQuadruple(b);
            }
            if (typeof(T) == typeof(Half)) {
                ushort b = (ushort)(Unsafe.As<T, ushort>(ref v) | (1 << 9));
                return (T)(object)Unsafe.As<ushort, Half>(ref b);
            }
            return T.NaN;
        }

        // ================================================================
        //  Per-T invariants.
        // ================================================================
        private static class Specials<T> where T : IFloatingPointIeee754<T> {

            public static readonly int Precision =
                1 - T.ILogB(T.BitIncrement(T.One) - T.One);

            public static readonly int MaxExp = T.ILogB(T.BitDecrement(T.PositiveInfinity));
            public static readonly int MinExp = T.ILogB(T.BitIncrement(T.Zero));

            public static readonly BigRational TinyPos = MakeTinyPos();
            public static readonly BigRational TinyNeg = -TinyPos;
            public static readonly BigRational HugePos = MakeHugePos();
            public static readonly BigRational HugeNeg = -HugePos;

            public static readonly BigRational LogMax2 =
                BigRational.FromFraction(2L * MaxExp + 2, BigInteger.One);
            public static readonly BigRational LogMin2 =
                BigRational.FromFraction(2L * MinExp, BigInteger.One);

            public static readonly long InitialPrecision = 2L * Precision + 32;
            public static readonly long PrecisionStep = Precision + 32;

            private static BigRational MakeTinyPos() {
                int e = -2 * MinExp;
                return e >= 0
                    ? BigRational.FromFraction(BigInteger.One << e, BigInteger.One)
                    : BigRational.FromFraction(BigInteger.One, BigInteger.One << -e);
            }
            private static BigRational MakeHugePos() {
                BigInteger mant = (BigInteger.One << Precision) - 1;
                int shift = MaxExp - Precision + 1;
                var max = shift >= 0
                    ? BigRational.FromFraction(mant << shift, BigInteger.One)
                    : BigRational.FromFraction(mant, BigInteger.One << -shift);
                return max * max;
            }
        }

        // ================================================================
        //  BigRational helpers.
        // ================================================================
        private static BigRational B(long v) => BigRational.FromFraction(v, BigInteger.One);
        private static BigRational B(long n, long d) => BigRational.FromFraction(n, d);

        private static bool BelowPrecision(BigRational v, long p) {
            if (BigRational.IsZero(v)) return true;
            BigRational a = BigRational.Abs(v);
            BigInteger n = a.SignedNumerator, d = a.Denominator;
            if (p >= 0) {
                if (p > int.MaxValue - 8) return true;
                return n * (BigInteger.One << (int)p) < d;
            }
            if (-p > int.MaxValue - 8) return false;
            return n < d * (BigInteger.One << (int)-p);
        }

        private static bool IsPowerOfTwo(BigInteger v) => v.Sign > 0 && (v & (v - 1)).IsZero;

        private static bool TryExactSqrt(BigRational x, out BigRational r) {
            r = default;
            if (x.Sign < 0) return false;
            if (BigRational.IsZero(x)) { r = BigRational.Zero; return true; }
            BigInteger p = GenericMath.SqrtRem(x.SignedNumerator, out var pr);
            if (!pr.IsZero) return false;
            BigInteger q = GenericMath.SqrtRem(x.Denominator, out var qr);
            if (!qr.IsZero) return false;
            r = BigRational.FromFraction(p, q);
            return true;
        }

        // ================================================================
        //  Rounding driver.
        //
        //  CONTRACT on toFloat:
        //    * preimage of every T value under toFloat is convex;
        //    * toFloat(toRational(v)) == v for every v ∈ T (round-trip);
        //    * toFloat is monotone non-decreasing.
        //
        //  CONTRACT on compute(prec):
        //    * R ∈ [lo, hi] always (R is the exact real-valued result);
        //    * hi − lo ≤ C · 2^{-prec} for some constant C (linear shrink).
        //
        //  TERMINATION (rigorous):  If R is not on a boundary between two T
        //  basins, then dist(R, boundary) = m > 0.  As prec grows linearly,
        //  hi − lo shrinks to zero, so eventually hi − lo < m, both
        //  endpoints fall in the same basin, and toFloat(lo) == toFloat(hi).
        //  No artificial iteration cap is needed — the caller must have
        //  pre-classified ties (exact midpoints) BEFORE calling this routine.
        // ================================================================
        private static T RoundInterval<T>(
            Func<BigRational, T> toFloat,
            Func<long, (BigRational Lo, BigRational Hi)> compute)
            where T : IFloatingPointIeee754<T> {

            long prec = Specials<T>.InitialPrecision;
            long step = Specials<T>.PrecisionStep;
            while (true) {
                var (lo, hi) = compute(prec);
                if (lo > hi) (lo, hi) = (hi, lo);
                T tl = toFloat(lo), th = toFloat(hi);
                if (EqualityComparer<T>.Default.Equals(tl, th)) return tl;
                prec += step;
            }
        }

        // ================================================================
        //  Sqrt (IEEE 754-2019 §9.2).
        // ================================================================
        public static T Sqrt<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            if (T.IsNaN(x)) return QuietNaN(x);
            if (T.IsZero(x)) return x;                       // +0, -0 preserved
            if (T.IsNegative(x)) return T.NaN;
            if (T.IsPositiveInfinity(x)) return T.PositiveInfinity;

            BigRational xr = toRational(x);
            if (TryExactSqrt(xr, out BigRational exact))
                return toFloat(exact);                       // exact rational; toFloat rounds (tie-safe)

            T approx = T.Sqrt(x);
            BigRational seed = (T.IsFinite(approx) && !T.IsZero(approx))
                ? toRational(approx)
                : BigRational.One;
            return RoundInterval(toFloat, prec => SqrtNewton(xr, seed, prec));
        }

        private static (BigRational Lo, BigRational Hi) SqrtNewton(
            BigRational x, BigRational seed, long precision) {
            var two = B(2);
            BigRational y = seed;
            long bits = 1;                                   // seed may be arbitrarily coarse
            while (bits < precision + 1) {
                y = (y + x / y) / two;
                bits = 2 * bits + 1;
            }
            BigRational t = x / y;
            return y < t ? (y, t) : (t, y);
        }

        // ================================================================
        //  Hypot (IEEE 754-2019 §9.2).
        // ================================================================
        public static T Hypot<T>(T x, T y, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            // §6.2: sNaN quiets first.
            if (IsSignalingNaN(x)) return QuietNaN(x);
            if (IsSignalingNaN(y)) return QuietNaN(y);

            // §9.2: hypot(±∞, anything) = +∞ (even for qNaN).
            if (T.IsInfinity(x) || T.IsInfinity(y)) return T.PositiveInfinity;

            if (T.IsNaN(x)) return QuietNaN(x);
            if (T.IsNaN(y)) return QuietNaN(y);

            if (T.IsZero(x) && T.IsZero(y)) return -T.NegativeZero;
            if (T.IsZero(x)) return T.Abs(y);
            if (T.IsZero(y)) return T.Abs(x);

            BigRational xr = toRational(x), yr = toRational(y);
            BigRational r2 = xr * xr + yr * yr;

            if (TryExactSqrt(r2, out BigRational exact))
                return toFloat(exact);

            BigRational seed;
            T approx = toFloat(r2);
            if (T.IsFinite(approx) && !T.IsZero(approx)) {
                T sqrtApprox = T.Sqrt(approx);
                seed = (T.IsFinite(sqrtApprox) && !T.IsZero(sqrtApprox))
                    ? toRational(sqrtApprox)
                    : toRational(T.Max(T.Abs(x), T.Abs(y)));
            } else {
                seed = toRational(T.Max(T.Abs(x), T.Abs(y)));
            }
            return RoundInterval(toFloat, prec => SqrtNewton(r2, seed, prec));
        }

        // ================================================================
        //  Exp family.
        // ================================================================
        public static T Exp<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            if (T.IsNaN(x)) return QuietNaN(x);
            if (T.IsPositiveInfinity(x)) return T.PositiveInfinity;
            if (T.IsNegativeInfinity(x)) return -T.NegativeZero;
            if (T.IsZero(x)) return toFloat(BigRational.One);

            BigRational xr = toRational(x);
            return RoundInterval(toFloat, prec => ExpPoint<T>(xr, prec));
        }

        public static T ExpM1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            if (T.IsNaN(x)) return QuietNaN(x);
            if (T.IsPositiveInfinity(x)) return T.PositiveInfinity;
            if (T.IsNegativeInfinity(x)) return toFloat(BigRational.NegativeOne);
            if (T.IsZero(x)) return T.CopySign(T.Zero, x);   // ±0 preserved

            BigRational xr = toRational(x);
            if (BigRational.Abs(xr) <= BigRational.One)
                return RoundInterval(toFloat, prec => ExpM1Series(xr, prec));
            return RoundInterval(toFloat, prec => {
                var (lo, hi) = ExpPoint<T>(xr, prec);
                return (lo - BigRational.One, hi - BigRational.One);
            });
        }

        public static T Exp2<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T>
            => ExpBase(x, toRational, toFloat, B(2), BigRational.Zero);
        public static T Exp2M1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T>
            => ExpBase(x, toRational, toFloat, B(2), BigRational.One);
        public static T Exp10<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T>
            => ExpBase(x, toRational, toFloat, B(10), BigRational.Zero);
        public static T Exp10M1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T>
            => ExpBase(x, toRational, toFloat, B(10), BigRational.One);

        public static T ExpM<T>(T x, T @base, T subtrahend,
                                Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            if (!T.IsFinite(@base))
                throw new ArgumentOutOfRangeException(nameof(@base), "base must be finite.");
            if (!T.IsFinite(subtrahend))
                throw new ArgumentOutOfRangeException(nameof(subtrahend), "subtrahend must be finite.");
            return ExpBase(x, toRational, toFloat, toRational(@base), toRational(subtrahend));
        }

        private static T ExpBase<T>(
            T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat,
            BigRational b, BigRational sub)
            where T : IFloatingPointIeee754<T> {

            if (T.IsNaN(x)) return QuietNaN(x);
            if (b.Sign <= 0) throw new ArgumentOutOfRangeException(nameof(b), "base must be positive.");

            // b = 1: b^x − sub = 1 − sub (constant).
            if (b == BigRational.One) return toFloat(BigRational.One - sub);

            int cmp1 = b.CompareTo(BigRational.One);

            // x = ±∞: b^(±∞) is 0⁺ or +∞; subtract sub.
            if (T.IsPositiveInfinity(x)) {
                if (cmp1 > 0) return T.PositiveInfinity;
                // b^(+∞) = +0, result = +0 − sub
                return BigRational.IsZero(sub) ? -T.NegativeZero : toFloat(-sub);
            }
            if (T.IsNegativeInfinity(x)) {
                if (cmp1 < 0) return T.PositiveInfinity;
                // b^(-∞) = +0, result = +0 − sub
                return BigRational.IsZero(sub) ? -T.NegativeZero : toFloat(-sub);
            }

            // x = ±0: b^0 = 1, result = 1 − sub.
            if (T.IsZero(x)) {
                BigRational exact = BigRational.One - sub;
                if (BigRational.IsZero(exact)) return T.CopySign(T.Zero, x);   // ±0
                return toFloat(exact);
            }
            {
                BigRational xr = toRational(x);

                // Exact-dyadic fast path: every possible tie is dyadic.
                if (TryGetExactDyadic<T>(xr, b, sub, out BigRational exact))
                    return toFloat(exact);

                // Irrational result: no tie possible.
                return RoundInterval(toFloat, prec => {
                    var (lnLo, lnHi) = LnOf(b, prec + 32);

                    BigRational yLo, yHi;
                    if (xr.Sign >= 0) { yLo = xr * lnLo; yHi = xr * lnHi; } else { yLo = xr * lnHi; yHi = xr * lnLo; }
                    if (yLo > yHi) (yLo, yHi) = (yHi, yLo);

                    var (eLo, eHi) = ExpInterval<T>(yLo, yHi, prec);
                    eLo -= sub; eHi -= sub;
                    return eLo < eHi ? (eLo, eHi) : (eHi, eLo);
                });
            }
        }

        // ================================================================
        //  Exact-dyadic test for b^x − sub.
        // ================================================================
        private static bool TryGetExactDyadic<T>(
            BigRational x, BigRational b, BigRational sub, out BigRational exact)
            where T : IFloatingPointIeee754<T> {

            long limit = Specials<T>.Precision + 1;
            exact = default;

            // Case A: integer exponent.
            if (x.Denominator.IsOne) {
                BigInteger k = x.SignedNumerator;
                if (BigInteger.Abs(k) > limit) return false;
                if (!k.IsZero) {
                    bool ok = k.Sign > 0
                        ? IsPowerOfTwo(b.Denominator)
                        : IsPowerOfTwo(b.SignedNumerator);
                    if (!ok) return false;
                }
                exact = PowExact(b, k) - sub;
                return true;
            }

            // Case B: b = 2^m and mx ∈ ℤ.
            if (TryGetPowerOfTwoExponent(b, out BigInteger m)) {
                BigInteger num = m * x.SignedNumerator;
                if (num % x.Denominator != 0) return false;
                BigInteger e = num / x.Denominator;
                if (BigInteger.Abs(e) > 2 * Specials<T>.MaxExp + 2) return false;
                exact = Pow2Exact(e) - sub;
                return true;
            }

            // Case C: b = c^q with q = x.Denominator (≤ limit).
            if (x.Denominator > limit) return false;
            int q = (int)x.Denominator;
            var n = b.SignedNumerator;
            var d = b.Denominator;
            if (n.Sign <= 0) return false;

            var nRoot = IntegerNthRoot(n, q, out bool nExact);
            if (!nExact) return false;
            var dRoot = IntegerNthRoot(d, q, out bool dExact);
            if (!dExact) return false;

            BigInteger p = x.SignedNumerator;
            if (BigInteger.Abs(p) > limit) return false;

            exact = PowExact(BigRational.FromFraction(nRoot, dRoot), p) - sub;
            return true;
        }

        private static BigRational PowExact(BigRational b, BigInteger n) {
            if (n.IsZero) return BigRational.One;
            int a = checked((int)BigInteger.Abs(n));
            var p = BigRational.Pow(b, a);
            return n.Sign > 0 ? p : BigRational.Invert(p);
        }
        private static BigRational Pow2Exact(BigInteger e) {
            if (e.IsZero) return BigRational.One;
            int a = checked((int)BigInteger.Abs(e));
            return e.Sign > 0
                ? BigRational.FromFraction(BigInteger.One << a, BigInteger.One)
                : BigRational.FromFraction(BigInteger.One, BigInteger.One << a);
        }
        private static bool TryGetPowerOfTwoExponent(BigRational b, out BigInteger m) {
            m = BigInteger.Zero;
            var n = b.SignedNumerator; var d = b.Denominator;
            if (n.Sign <= 0 || !IsPowerOfTwo(n) || !IsPowerOfTwo(d)) return false;
            m = n.GetBitLength() - d.GetBitLength();
            return true;
        }
        private static BigInteger IntegerNthRoot(BigInteger n, int q, out bool exact) {
            // n > 0, q ≥ 2.
            if (n.IsOne) { exact = true; return BigInteger.One; }
            if (n.GetBitLength() < q) { exact = false; return BigInteger.One; }
            BigInteger lo = BigInteger.One;
            BigInteger hi = BigInteger.One << (int)((n.GetBitLength() + q - 1) / q + 1);
            while (hi - lo > 1) {
                var mid = (lo + hi) >> 1;
                if (BigInteger.Pow(mid, q).CompareTo(n) <= 0) lo = mid; else hi = mid;
            }
            exact = BigInteger.Pow(lo, q) == n;
            return lo;
        }

        // ================================================================
        //  Exp(x) interval (single rational point), with saturation.
        // ================================================================
        private static (BigRational Lo, BigRational Hi) ExpPoint<T>(BigRational x, long precision)
            where T : IFloatingPointIeee754<T> {
            if (BigRational.IsZero(x)) return (BigRational.One, BigRational.One);
            if (x > Specials<T>.LogMax2) return (Specials<T>.HugePos, Specials<T>.HugePos);
            if (x < Specials<T>.LogMin2) return (Specials<T>.TinyPos, Specials<T>.TinyPos);

            BigRational nR = BigRational.Ceiling(x);
            BigRational fR = x - nR;
            BigInteger n = nR.SignedNumerator;

            long nBits = n.IsZero ? 0 : (long)BigInteger.Log2(BigInteger.Abs(n) + 1);
            long a = (long)Math.Sqrt(nBits);

            var eCalc = new ECalculator();
            var fracCalc = new FractionExpCalculator();
            var (eLo, eHi) = eCalc.GetE(precision + a + 2);
            var (fLo, fHi) = fracCalc.ExpInternal(fR, precision + 2);

            BigRational nLo, nHi;
            if (n.IsZero) {
                nLo = nHi = BigRational.One;
            } else {
                int abs = checked((int)BigInteger.Abs(n));
                if (n.Sign > 0) {
                    nLo = BigRational.Pow(eLo, abs);
                    nHi = BigRational.Pow(eHi, abs);
                } else {
                    var tLo = BigRational.Pow(eHi, abs);
                    var tHi = BigRational.Pow(eLo, abs);
                    nLo = BigRational.Invert(tLo);
                    nHi = BigRational.Invert(tHi);
                }
            }
            var lo = nLo * fLo; var hi = nHi * fHi;
            return lo < hi ? (lo, hi) : (hi, lo);
        }

        private static (BigRational Lo, BigRational Hi) ExpInterval<T>(
            BigRational a, BigRational b, long precision)
            where T : IFloatingPointIeee754<T> {
            if (a > b) (a, b) = (b, a);
            if (a == b) return ExpPoint<T>(a, precision);

            BigRational mid = (a + b) / B(2), h = (b - a) / B(2);
            if (h <= B(1, 2)) {
                var (mL, mH) = ExpPoint<T>(mid, precision);
                BigRational w = h + h;
                var lo = mL * (BigRational.One - w);
                var hi = mH * (BigRational.One + w);
                return lo < hi ? (lo, hi) : (hi, lo);
            }
            var (a0, a1) = ExpPoint<T>(a, precision);
            var (b0, b1) = ExpPoint<T>(b, precision);
            return (a0 < b0 ? a0 : b0, a1 > b1 ? a1 : b1);
        }

        private static (BigRational Lo, BigRational Hi) ExpM1Series(BigRational x, long precision) {
            if (BigRational.IsZero(x)) return (BigRational.Zero, BigRational.Zero);
            BigRational sum = x, term = x;
            long k = 1;
            while (true) {
                k++;
                BigRational next = term * x / k;
                if (BelowPrecision(BigRational.Abs(next), precision)) {
                    BigRational a = BigRational.Abs(next);
                    BigRational basis = sum + next;
                    if (x.Sign > 0) return (basis, basis + a);    // all positive; |R−S_k| ≤ |t_k|
                    return sum < basis ? (sum, basis) : (basis, sum);  // alternating
                }
                sum += next;
                term = next;
            }
        }

        // ================================================================
        //  Log family.
        // ================================================================
        public static T Log<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, null, BigRational.Zero);
        public static T Log2<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, B(2), BigRational.Zero);
        public static T Log10<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, B(10), BigRational.Zero);
        public static T LogP1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, null, BigRational.One);
        public static T Log2P1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, B(2), BigRational.One);
        public static T Log10P1<T>(T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> => LogImpl(x, toRational, toFloat, B(10), BigRational.One);

        public static T LogP<T>(T x, T @base, T addend,
                                Func<T, BigRational> toRational, Func<BigRational, T> toFloat)
            where T : IFloatingPointIeee754<T> {
            if (!T.IsFinite(@base))
                throw new ArgumentOutOfRangeException(nameof(@base), "base must be finite.");
            if (!T.IsFinite(addend))
                throw new ArgumentOutOfRangeException(nameof(addend), "addend must be finite.");
            BigRational b = toRational(@base);
            if (b.Sign <= 0 || b == BigRational.One)
                throw new ArgumentOutOfRangeException(nameof(@base),
                    "base must be positive and not equal to 1.");
            return LogImpl(x, toRational, toFloat, b, toRational(addend));
        }

        private static T LogImpl<T>(
            T x, Func<T, BigRational> toRational, Func<BigRational, T> toFloat,
            BigRational? scale, BigRational addend)
            where T : IFloatingPointIeee754<T> {

            if (T.IsNaN(x)) return QuietNaN(x);

            if (T.IsPositiveInfinity(x)) {
                if (scale is null) return T.PositiveInfinity;        // ln(e) = 1 > 0
                return scale.Value.CompareTo(BigRational.One) > 0
                    ? T.PositiveInfinity : T.NegativeInfinity;
            }
            if (T.IsNegativeInfinity(x)) return T.NaN;

            // log1p(±0) = ±0 (sign preserved from input).
            if (T.IsZero(x) && addend == BigRational.One) return x;

            BigRational y = toRational(x) + addend;
            if (y.Sign < 0) return T.NaN;
            if (BigRational.IsZero(y)) return T.NegativeInfinity;

            // y = 1: log_scale(1) = 0 exactly.
            if (y == BigRational.One) return toFloat(BigRational.Zero);

            return RoundInterval(toFloat, prec => {
                var (lnLo, lnHi) = LnOf(y, prec + 32);
                if (scale is null) return (lnLo, lnHi);

                var (sLo, sHi) = LnOf(scale.Value, prec + 32);
                BigRational r1 = lnLo / sLo, r2 = lnLo / sHi, r3 = lnHi / sLo, r4 = lnHi / sHi;
                BigRational lo = r1, hi = r1;
                if (r2 < lo) lo = r2; if (r3 < lo) lo = r3; if (r4 < lo) lo = r4;
                if (r2 > hi) hi = r2; if (r3 > hi) hi = r3; if (r4 > hi) hi = r4;
                return (lo, hi);
            });
        }

        // ================================================================
        //  ln(v), v > 0.
        // ================================================================
        private static (BigRational Lo, BigRational Hi) LnOf(BigRational v, long precision) {
            if (v.Sign <= 0) throw new ArgumentOutOfRangeException(nameof(v));
            if (v == BigRational.One) return (BigRational.Zero, BigRational.Zero);

            long k = v.SignedNumerator.GetBitLength() - v.Denominator.GetBitLength();
            BigRational m = ScaleTwoPow(v, -k);

            BigRational t = (m - BigRational.One) / (m + BigRational.One);
            var (aLo, aHi) = AtanhSeries(t, precision + 2);
            BigRational lnM_Lo = aLo + aLo, lnM_Hi = aHi + aHi;
            if (k == 0) return (lnM_Lo, lnM_Hi);

            var (l2Lo, l2Hi) = Ln2(precision);
            BigRational kR = B(k);
            BigRational lnK_Lo = k > 0 ? kR * l2Lo : kR * l2Hi;
            BigRational lnK_Hi = k > 0 ? kR * l2Hi : kR * l2Lo;
            return (lnM_Lo + lnK_Lo, lnM_Hi + lnK_Hi);
        }

        private static BigRational ScaleTwoPow(BigRational x, long k) {
            if (k == 0) return x;
            if (k > 0) return BigRational.FromFraction(x.SignedNumerator, x.Denominator << (int)k);
            return BigRational.FromFraction(x.SignedNumerator << (int)(-k), x.Denominator);
        }

        private static (BigRational Lo, BigRational Hi) AtanhSeries(BigRational t, long precision) {
            if (BigRational.IsZero(t)) return (BigRational.Zero, BigRational.Zero);
            BigRational t2 = t * t, sum = t, tp = t;
            long k = 1;
            while (true) {
                k += 2; tp *= t2;
                BigRational next = tp / k;
                if (BelowPrecision(BigRational.Abs(next), precision)) {
                    BigRational a = BigRational.Abs(next), tail = a + a;
                    BigRational basis = sum + next;
                    return t.Sign > 0 ? (basis, basis + tail) : (basis - tail, basis);
                }
                sum += next;
            }
        }

        public static (BigRational Lo, BigRational Hi) Ln2(long precision) {
            var (lo, hi) = AtanhSeries(B(1, 3), precision + 2);
            return (lo + lo, hi + hi);
        }

        // ================================================================
        //  e = Σ 1/k!  with rigorous tail bound.
        // ================================================================
        private sealed class ECalculator {
            private BigRational _sum = BigRational.One, _term = BigRational.One;
            private long _k = 0;
            public (BigRational Lo, BigRational Hi) GetE(long precision) {
                while (true) {
                    BigRational rem = _term * 2 / (_k + 1);   // 0 < e − S_k < rem
                    if (BelowPrecision(rem, precision)) return (_sum, _sum + rem);
                    _k++; _term /= _k; _sum += _term;
                }
            }
        }

        // ================================================================
        //  exp(f) for f ∈ (-1, 0].
        // ================================================================
        private sealed class FractionExpCalculator {
            private bool _has; private BigRational _f;
            private BigRational _sum = BigRational.One, _term = BigRational.One;
            private long _k;
            public (BigRational Lo, BigRational Hi) ExpInternal(BigRational f, long precision) {
                if (BigRational.IsZero(f)) return (BigRational.One, BigRational.One);
                if (!_has || _f != f) {
                    _f = f; _sum = BigRational.One; _term = BigRational.One; _k = 0; _has = true;
                }
                while (true) {
                    BigRational next = _term * f / (_k + 1);
                    if (BelowPrecision(BigRational.Abs(next), precision)) {
                        BigRational basis = _sum + next;
                        return _sum < basis ? (_sum, basis) : (basis, _sum);
                    }
                    _k++; _term = next; _sum += next;
                }
            }
        }
    }
}