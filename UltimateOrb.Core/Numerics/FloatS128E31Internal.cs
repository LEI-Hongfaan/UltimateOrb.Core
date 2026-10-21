using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    // TODO (AI): impl the methods other than ToString/Parse/Format

    // Notes (Do not remove):
    // * _significand > Int128.MaxValue || Zero
    // * abs(_exp) < 2 ** 31
    // * _sgn == 0 or 1
    // * undefined if _exp out of range after normalization, but it is ok to use Min/MaxValue or Zero (same sign) to replace inf or subnormal
    // * undefined if output is inf or nan, but it is ok to use Min/MaxValue or Zero (same sign) to replace inf or nan
    // * default context is checked, use check/unchecked block/expr correctly
    internal readonly struct FloatS128E31Internal
    : IComparable, IComparable<FloatS128E31Internal>, IFloatingPoint<FloatS128E31Internal> {

        public readonly UInt128 _significand;
        public readonly Int32 _exp;
        public readonly UInt32 _sgn;

        internal FloatS128E31Internal(UInt64 hi, UInt64 lo, Int32 exp, UInt32 sgn)
            : this(((UInt128)hi << 64) | lo, exp, sgn) { }

        public FloatS128E31Internal(UInt128 significand, Int32 exp, UInt32 sgn) {
            _significand = significand;
            _exp = exp;
            _sgn = sgn;
        }

        // ─── Comparison ────────────────────────────────────────────────────────

        public int CompareTo(FloatS128E31Internal other) {
            bool az = _significand == 0, bz = other._significand == 0;
            if (az && bz) return 0;
            if (az) return other._sgn == 0 ? -1 : 1;
            if (bz) return _sgn == 0 ? 1 : -1;
            if (_sgn != other._sgn) return _sgn == 0 ? 1 : -1;
            int mag = CmpDint(this, other);
            return _sgn == 0 ? mag : -mag;
        }

        public int CompareTo(object? obj) {
            if (obj is null) return 1;
            if (obj is FloatS128E31Internal other) return CompareTo(other);
            throw new ArgumentException("Object must be of type FloatS128E31Internal.", nameof(obj));
        }

        public bool Equals(FloatS128E31Internal other) {
            if (_significand == 0 && other._significand == 0) return true;
            return _significand == other._significand && _exp == other._exp && _sgn == other._sgn;
        }

        public override bool Equals(object? obj) => obj is FloatS128E31Internal o && Equals(o);
        public override int GetHashCode() => HashCode.Combine(_significand, _exp, _sgn);

        // ─── Constants ─────────────────────────────────────────────────────────

        public static readonly FloatS128E31Internal One = new(hi: 0x8000000000000000UL, lo: 0x0UL, exp: 0, sgn: 0u);
        public static readonly FloatS128E31Internal NegativeOne = new(hi: 0x8000000000000000UL, lo: 0x0UL, exp: 0, sgn: 1u);
        public static readonly FloatS128E31Internal LogOf2 = new(hi: 0xb17217f7d1cf79abUL, lo: 0xc9e3b39803f2f6afUL, exp: -1, sgn: 0u);
        public static readonly FloatS128E31Internal Log2OfEShifted = new(hi: 0xb8aa3b295c17f0bbUL, lo: 0xbe87fed0691d3e89UL, exp: 12, sgn: 0u);
        public static readonly FloatS128E31Internal Zero = new(hi: 0x0UL, lo: 0x0UL, exp: 0, sgn: 0u);

        private static readonly FloatS128E31Internal _E_val = CalculateE();
        private static readonly FloatS128E31Internal _Pi_val = CalculatePi();
        private static readonly FloatS128E31Internal _Tau_val = new(_Pi_val._significand, unchecked(_Pi_val._exp + 1), 0u);

        public static FloatS128E31Internal E => _E_val;
        public static FloatS128E31Internal Pi => _Pi_val;
        public static FloatS128E31Internal Tau => _Tau_val;

        static FloatS128E31Internal ISignedNumber<FloatS128E31Internal>.NegativeOne => NegativeOne;
        static FloatS128E31Internal INumberBase<FloatS128E31Internal>.One => One;
        static FloatS128E31Internal INumberBase<FloatS128E31Internal>.Zero => Zero;
        public static int Radix => 2;
        public static FloatS128E31Internal AdditiveIdentity => Zero;
        public static FloatS128E31Internal MultiplicativeIdentity => One;

        // ─── Helpers ───────────────────────────────────────────────────────────

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt64 Hi(UInt128 v) => unchecked((UInt64)(v >> 64));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UInt64 Lo(UInt128 v) => unchecked((UInt64)v);

        public static bool IsZero(FloatS128E31Internal x) => x._significand == 0;

        internal static int CmpDint(in FloatS128E31Internal a, in FloatS128E31Internal b) {
            var c = a._exp.CompareTo(b._exp);
            if (c != 0) return c;
            c = Hi(a._significand).CompareTo(Hi(b._significand));
            if (c != 0) return c;
            return Lo(a._significand).CompareTo(Lo(b._significand));
        }

        // ─── Exact-from-BigInteger with RNE, no double rounding ───────────────

        // value = sig * 2^exp  (signed by `sgn`), rounded once to 128-bit significand.
        private static FloatS128E31Internal FromBigIntegerScaled(BigInteger sig, int exp, uint sgn) {
            unchecked {
                if (sig.IsZero) return new FloatS128E31Internal(0, 0, sgn);
                if (sig.Sign < 0) { sgn ^= 1; sig = -sig; }

                int bits = (int)sig.GetBitLength();
                BigInteger significand;
                int newExp;

                if (bits <= 128) {
                    significand = sig << (128 - bits);
                    newExp = exp + bits - 1;
                } else {
                    int shift = bits - 128;
                    BigInteger head = sig >> shift;
                    BigInteger tail = sig - (head << shift);
                    BigInteger half = BigInteger.One << (shift - 1);

                    if (tail > half || (tail == half && !head.IsEven)) head += 1;
                    if (head == (BigInteger.One << 128)) { head >>= 1; shift += 1; }
                    significand = head;
                    newExp = exp + shift + 127;
                }
                return new FloatS128E31Internal((UInt128)significand, newExp, sgn);
            }
        }

        // ─── Add (exact, single RNE) ──────────────────────────────────────────

        internal static void AddDint(out FloatS128E31Internal r,
                                     in FloatS128E31Internal a, in FloatS128E31Internal b) {
            unchecked {
                bool az = a._significand == 0, bz = b._significand == 0;
                if (az && bz) {
                    r = new FloatS128E31Internal(0, 0, (a._sgn == 1 && b._sgn == 1) ? 1u : 0u);
                    return;
                }
                if (az) { r = b; return; }
                if (bz) { r = a; return; }

                // Fast path: |sh| ≥ 129 ⇒ smaller operand < half-ULP of larger.
                //   sh = a._exp - b._exp.  Both significands ∈ [2^127, 2^128).
                //   |b| < 2^(b._exp+1); half-ULP(a) = 2^(a._exp - 128).
                //   If b._exp + 1 ≤ a._exp - 128 (i.e. sh ≥ 129) then |b| < half-ULP(a).
                //   Symmetric for sh ≤ -129.
                int sh = a._exp - b._exp;
                if (sh > 128) { r = a; return; }
                if (sh < -128) { r = b; return; }

                int commonExp = Math.Min(a._exp, b._exp);
                BigInteger aMag = ((BigInteger)a._significand) << (a._exp - commonExp);
                BigInteger bMag = ((BigInteger)b._significand) << (b._exp - commonExp);
                if (a._sgn == 1) aMag = -aMag;
                if (b._sgn == 1) bMag = -bMag;
                BigInteger sum = aMag + bMag;

                if (sum.IsZero) { r = Zero; return; }

                r = FromBigIntegerScaled(sum, commonExp - 127, 0);
            }
        }

        // ─── Multiply (exact 256-bit product, single RNE) ─────────────────────

        private static void MulUInt128(UInt128 a, UInt128 b, out UInt128 hi, out UInt128 lo) {
            unchecked {
                UInt64 aHi = (UInt64)(a >> 64), aLo = (UInt64)a;
                UInt64 bHi = (UInt64)(b >> 64), bLo = (UInt64)b;
                UInt128 hh = (UInt128)aHi * bHi;
                UInt128 hl = (UInt128)aHi * bLo;
                UInt128 lh = (UInt128)aLo * bHi;
                UInt128 ll = (UInt128)aLo * bLo;
                UInt128 mid = hl + lh;
                UInt64 midCarry = mid < hl ? 1UL : 0UL;
                UInt128 shiftedMid = mid << 64;
                lo = shiftedMid + ll;
                UInt64 loCarry = lo < shiftedMid ? 1UL : 0UL;
                hi = hh + ((UInt128)midCarry << 64) + (mid >> 64) + loCarry;
            }
        }

        internal static void MulDint(out FloatS128E31Internal r,
                                     in FloatS128E31Internal a, in FloatS128E31Internal b) {
            unchecked {
                if (a._significand == 0 || b._significand == 0) { r = Zero; return; }

                MulUInt128(a._significand, b._significand, out UInt128 hi, out UInt128 lo);

                int exp = a._exp + b._exp;
                UInt128 sig;
                UInt128 half = (UInt128)1 << 127;

                if (hi < half) {
                    sig = (hi << 1) | (lo >> 127);
                    UInt128 tail = lo << 1;
                    if (tail > half || (tail == half && (sig & 1) != 0)) sig += 1;
                    if (sig == 0) { sig = half; exp += 1; }
                } else {
                    sig = hi;
                    if (lo > half || (lo == half && (sig & 1) != 0)) sig += 1;
                    if (sig == 0) { sig = half; exp += 1; }
                    exp += 1;
                }

                r = new FloatS128E31Internal(sig, exp, a._sgn ^ b._sgn);
            }
        }

        // ─── Multiply by Int64 (exact 128×64→192 product, single RNE) ─────────

        internal static void MulDint2(out FloatS128E31Internal r,
                                      Int64 b, in FloatS128E31Internal a) {
            unchecked {
                if (b == 0 || a._significand == 0) { r = Zero; return; }

                UInt64 absB = b < 0 ? (UInt64)(-(b + 1)) + 1UL : (UInt64)b;
                uint sgn = b < 0 ? 1u - a._sgn : a._sgn;

                UInt64 aHi = (UInt64)(a._significand >> 64), aLo = (UInt64)a._significand;
                UInt128 prodHi = (UInt128)aHi * absB;
                UInt128 prodLo = (UInt128)aLo * absB;

                UInt64 top64 = (UInt64)(prodHi >> 64);
                UInt128 mid = ((UInt64)prodHi) << 64;
                UInt128 bottom128 = mid + prodLo;
                if (bottom128 < mid) top64 += 1;

                UInt128 sig;
                int exp;

                if (top64 == 0) {
                    sig = bottom128;
                    exp = a._exp;
                } else {
                    int shift = 64 - BitOperations.LeadingZeroCount(top64);
                    UInt128 sigPart = ((UInt128)top64 << (128 - shift)) | (bottom128 >> shift);
                    UInt128 tail = bottom128 & (((UInt128)1 << shift) - 1);
                    UInt128 h = (UInt128)1 << (shift - 1);

                    if (tail > h || (tail == h && (sigPart & 1) != 0)) sigPart += 1;
                    if (sigPart == 0) { sigPart = (UInt128)1 << 127; shift += 1; }
                    sig = sigPart;
                    exp = a._exp + shift;
                }

                r = new FloatS128E31Internal(sig, exp, sgn);
            }
        }

        // ─── Divide (exact rational, single RNE) ──────────────────────────────

        internal static void DivDint(out FloatS128E31Internal r,
                                     in FloatS128E31Internal a, in FloatS128E31Internal b) {
            unchecked {
                if (b._significand == 0) throw new DivideByZeroException();
                if (a._significand == 0) {
                    r = new FloatS128E31Internal(0, 0, a._sgn ^ b._sgn);
                    return;
                }

                uint sgn = a._sgn ^ b._sgn;
                // q = (a_sig << 128) / b_sig  ∈ (2^127, 2^129).
                // If 128 bits, exp_final = a._exp - b._exp - 1;
                // if 129 bits, shift-right by 1 and exp_final = a._exp - b._exp.
                int exp = a._exp - b._exp - 1;

                BigInteger num = ((BigInteger)a._significand) << 128;
                BigInteger den = b._significand;
                BigInteger q = BigInteger.DivRem(num, den, out BigInteger rem);

                if (q >= (BigInteger.One << 128)) {
                    BigInteger qDiv2 = q >> 1;
                    BigInteger qMod2 = q & 1;
                    BigInteger fracNum = qMod2 * den + rem;
                    int cmp = fracNum.CompareTo(den);
                    BigInteger rounded;
                    if (cmp > 0) rounded = qDiv2 + 1;
                    else if (cmp < 0) rounded = qDiv2;
                    else rounded = qDiv2.IsEven ? qDiv2 : qDiv2 + 1;

                    if (rounded >= (BigInteger.One << 128)) { rounded >>= 1; exp += 1; }
                    exp += 1;
                    q = rounded;
                } else {
                    int cmp = (rem * 2).CompareTo(den);
                    if (cmp > 0) q += 1;
                    else if (cmp == 0 && !q.IsEven) q += 1;
                    if (q >= (BigInteger.One << 128)) { q >>= 1; exp += 1; }
                }

                r = new FloatS128E31Internal((UInt128)q, exp, sgn);
            }
        }

        // ─── Bit-length / write helpers ────────────────────────────────────────

        public int GetExponentByteCount() => sizeof(int);

        public int GetExponentShortestBitLength() {
            int e = _exp;
            if (e >= 0) return 32 - int.LeadingZeroCount(e);
            return 32 + 1 - int.LeadingZeroCount(~e);
        }

        public int GetSignificandBitLength() => 128;
        public int GetSignificandByteCount() => 16;

        public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
            if (BinaryPrimitives.TryWriteInt32BigEndian(destination, _exp)) {
                bytesWritten = 4; return true;
            }
            bytesWritten = 0; return false;
        }

        public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (BinaryPrimitives.TryWriteInt32LittleEndian(destination, _exp)) {
                bytesWritten = 4; return true;
            }
            bytesWritten = 0; return false;
        }

        public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
            if (BinaryPrimitives.TryWriteUInt64BigEndian(destination, Hi(_significand))
                && BinaryPrimitives.TryWriteUInt64BigEndian(destination.Slice(8), Lo(_significand))) {
                bytesWritten = 16; return true;
            }
            bytesWritten = 0; return false;
        }

        public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (BinaryPrimitives.TryWriteUInt64LittleEndian(destination, Lo(_significand))
                && BinaryPrimitives.TryWriteUInt64LittleEndian(destination.Slice(8), Hi(_significand))) {
                bytesWritten = 16; return true;
            }
            bytesWritten = 0; return false;
        }

        // ─── Classification ────────────────────────────────────────────────────

        public static FloatS128E31Internal Abs(FloatS128E31Internal value)
            => new(value._significand, value._exp, 0);
        public static bool IsCanonical(FloatS128E31Internal value) => true;
        public static bool IsComplexNumber(FloatS128E31Internal value) => false;
        public static bool IsFinite(FloatS128E31Internal value) => true;
        public static bool IsImaginaryNumber(FloatS128E31Internal value) => false;
        public static bool IsInfinity(FloatS128E31Internal value) => false;
        public static bool IsNaN(FloatS128E31Internal value) => false;
        public static bool IsNegative(FloatS128E31Internal value) => value._sgn == 1;
        public static bool IsNegativeInfinity(FloatS128E31Internal value) => false;
        public static bool IsNormal(FloatS128E31Internal value) => value._significand != 0;
        public static bool IsPositive(FloatS128E31Internal value) => value._sgn == 0;
        public static bool IsPositiveInfinity(FloatS128E31Internal value) => false;
        public static bool IsRealNumber(FloatS128E31Internal value) => true;
        public static bool IsSubnormal(FloatS128E31Internal value) => false;

        public static bool IsInteger(FloatS128E31Internal value) {
            unchecked {
                if (value._significand == 0) return true;
                if (value._exp >= 127) return true;   // value = sig * 2^0 or larger integer multiple
                if (value._exp < 0) return false;     // 0 < |value| < 1
                int shift = 127 - value._exp;         // ∈ [1, 127]
                UInt128 mask = (((UInt128)1) << shift) - 1;
                return (value._significand & mask) == 0;
            }
        }

        public static bool IsEvenInteger(FloatS128E31Internal value) {
            unchecked {
                if (value._significand == 0) return true;
                if (value._exp >= 128) return true;   // |value| ≥ 2 ⇒ even
                if (value._exp < 0) return false;     // 0 < |value| < 1 ⇒ not an integer
                int s = 127 - value._exp;             // ∈ [0, 127]
                                                      // even ⟺ sig mod 2^(s+1) == 0
                UInt128 mask = s == 127
                    ? UInt128.MaxValue
                    : (((UInt128)1) << (s + 1)) - 1;
                return (value._significand & mask) == 0;
            }
        }

        public static bool IsOddInteger(FloatS128E31Internal value)
            => IsInteger(value) && !IsEvenInteger(value);

        // ─── Min / Max ─────────────────────────────────────────────────────────

        public static FloatS128E31Internal MaxMagnitude(FloatS128E31Internal x, FloatS128E31Internal y) {
            int c = Abs(x).CompareTo(Abs(y));
            if (c > 0) return x;
            if (c < 0) return y;
            return x._sgn == 0 ? x : y;           // +0 > -0
        }
        public static FloatS128E31Internal MaxMagnitudeNumber(FloatS128E31Internal x, FloatS128E31Internal y)
            => MaxMagnitude(x, y);

        public static FloatS128E31Internal MinMagnitude(FloatS128E31Internal x, FloatS128E31Internal y) {
            int c = Abs(x).CompareTo(Abs(y));
            if (c < 0) return x;
            if (c > 0) return y;
            return x._sgn == 1 ? x : y;           // -0 < +0
        }
        public static FloatS128E31Internal MinMagnitudeNumber(FloatS128E31Internal x, FloatS128E31Internal y)
            => MinMagnitude(x, y);

        // ─── Conversions ───────────────────────────────────────────────────────

        private static FloatS128E31Internal FromUInt64(ulong v) {
            if (v == 0) return Zero;
            int bits = 64 - BitOperations.LeadingZeroCount(v);
            return new FloatS128E31Internal(((UInt128)v) << (128 - bits), bits - 1, 0);
        }

        private static FloatS128E31Internal FromInt64(long v) {
            if (v == 0) return Zero;
            if (v < 0) {
                ulong a = v == long.MinValue ? 9223372036854775808UL : (ulong)(-v);
                var p = FromUInt64(a);
                return new FloatS128E31Internal(p._significand, p._exp, 1);
            }
            return FromUInt64((ulong)v);
        }

        private static FloatS128E31Internal FromUInt128(UInt128 v) {
            if (v == 0) return Zero;
            UInt64 hi = (UInt64)(v >> 64);
            if (hi != 0) {
                int bits = 128 - BitOperations.LeadingZeroCount(hi);
                return new FloatS128E31Internal(v, bits - 1, 0);
            }
            return FromUInt64((UInt64)v);
        }

        private static FloatS128E31Internal FromInt128(Int128 v) {
            if (v == 0) return Zero;
            if (v < 0) {
                UInt128 a = v == Int128.MinValue ? (UInt128)Int128.MaxValue + 1 : (UInt128)(-v);
                var r = FromUInt128(a);
                return new FloatS128E31Internal(r._significand, r._exp, 1);
            }
            return FromUInt128((UInt128)v);
        }

        private static FloatS128E31Internal FromDouble(double v) {
            unchecked {
                if (double.IsNaN(v) || double.IsInfinity(v)) return Zero;
                if (v == 0.0)
                    return new FloatS128E31Internal(0, 0, BitConverter.DoubleToInt64Bits(v) < 0 ? 1u : 0u);

                ulong bits = BitConverter.DoubleToUInt64Bits(v);
                uint sign = (uint)(bits >> 63);
                int exp = (int)((bits >> 52) & 0x7FF);
                ulong mant = bits & 0x000FFFFFFFFFFFFFUL;

                if (exp == 0) {
                    int b = BitOperations.Log2(mant);         // 0..51
                    UInt128 sig = ((UInt128)mant) << (127 - b);
                    return new FloatS128E31Internal(sig, b - 1074, sign);
                }
                UInt128 sig128 = ((UInt128)(1UL << 52) + mant) << 75;
                return new FloatS128E31Internal(sig128, exp - 1023, sign);
            }
        }

        // ─── ToDouble — SINGLE rounding, subnormal double-rounding fixed ─────

        private static double ToDouble(FloatS128E31Internal v) {
            unchecked {
                if (v._significand == 0) return v._sgn == 1 ? -0.0 : 0.0;

                UInt128 sig = v._significand;
                int exp = v._exp;
                UInt64 signBits = ((UInt64)v._sgn) << 63;

                // biased = exp + 1023.  Normal case: exp ≥ -1022 ⇒ biased ≥ 1.
                // Overflow guard: biased ≥ 2047 ⇒ |value| would overflow; RNE would
                // have already saturated to infinity, but we guard against _exp at
                // the extreme ends of Int32.
                int biased = exp + 1023;
                if (biased >= 2047) return v._sgn == 1 ? double.NegativeInfinity : double.PositiveInfinity;

                int shift = biased >= 1
                    ? 75                                // normal: quantum 2^(exp−52)
                    : -947 - exp;                       // subnormal: quantum 2^-1074
                                                        // = 76 - biased
                                                        // Note: shift ≥ 75 in both cases; shift ≤ 128 always (bounded by exp ≥ −1075).

                UInt128 Q, rem;
                if (shift >= 128) { Q = 0; rem = sig; } else { Q = sig >> shift; rem = sig & ((((UInt128)1) << shift) - 1); }

                // RNE at target width.
                if (shift > 0) {
                    UInt128 half = shift == 128
                        ? (UInt128)1 << 127
                        : (UInt128)1 << (shift - 1);
                    if (rem > half || (rem == half && (Q & 1) != 0)) Q += 1;
                }

                UInt64 expField;
                UInt64 fracBits;
                if (shift == 75) {
                    // Normal rounding.  Q ∈ [0, 2^53].  If Q == 2^53 the significand
                    // overflowed into the next binade.
                    if (Q == ((UInt128)1 << 53)) { Q >>= 1; biased += 1; }
                    if (biased >= 2047) return v._sgn == 1 ? double.NegativeInfinity : double.PositiveInfinity;
                    if (Q == 0) { expField = 0; fracBits = 0; }   // can't happen from shift=75 with sig≠0, but safe
                    else { expField = (UInt64)biased; fracBits = (UInt64)Q & 0xFFFFFFFFFFFFFUL; }
                } else {
                    // Subnormal (or value rounded down to zero).
                    if (Q >= ((UInt128)1 << 52)) {
                        // Rounded up into the smallest normal.
                        expField = 1;
                        fracBits = 0;
                    } else {
                        expField = 0;
                        fracBits = (UInt64)Q;
                    }
                }

                UInt64 dbits = signBits | (expField << 52) | fracBits;
                return BitConverter.UInt64BitsToDouble(dbits);
            }
        }

        // ─── ToBigInteger (truncated toward zero) ─────────────────────────────

        private static BigInteger ToBigIntegerTruncated(FloatS128E31Internal v) {
            unchecked {
                if (v._significand == 0) return BigInteger.Zero;
                BigInteger sig = v._significand;
                int exp = v._exp;
                BigInteger mag;
                if (exp >= 127) mag = sig << (exp - 127);
                else if (exp >= 0) mag = sig >> (127 - exp);
                else {
                    int totalShift = 127 - exp;
                    if (totalShift >= 128) return BigInteger.Zero;
                    mag = sig >> totalShift;
                }
                return v._sgn == 1 ? -mag : mag;
            }
        }

        // ─── TryConvertFrom / TryConvertTo ────────────────────────────────────

        private static bool TryConvertFromHelper<TOther>(TOther value, out FloatS128E31Internal result)
            where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(sbyte)) { result = FromInt64((sbyte)(object)value); return true; }
            if (typeof(TOther) == typeof(byte)) { result = FromUInt64((byte)(object)value); return true; }
            if (typeof(TOther) == typeof(short)) { result = FromInt64((short)(object)value); return true; }
            if (typeof(TOther) == typeof(ushort)) { result = FromUInt64((ushort)(object)value); return true; }
            if (typeof(TOther) == typeof(int)) { result = FromInt64((int)(object)value); return true; }
            if (typeof(TOther) == typeof(uint)) { result = FromUInt64((uint)(object)value); return true; }
            if (typeof(TOther) == typeof(long)) { result = FromInt64((long)(object)value); return true; }
            if (typeof(TOther) == typeof(ulong)) { result = FromUInt64((ulong)(object)value); return true; }
            if (typeof(TOther) == typeof(float)) { result = FromDouble((float)(object)value); return true; }
            if (typeof(TOther) == typeof(double)) { result = FromDouble((double)(object)value); return true; }
            if (typeof(TOther) == typeof(System.Int128)) { result = FromInt128((System.Int128)(object)value); return true; }
            if (typeof(TOther) == typeof(System.UInt128)) { result = FromUInt128((System.UInt128)(object)value); return true; }
            if (typeof(TOther) == typeof(UltimateOrb.Int128)) { result = FromInt128((UltimateOrb.Int128)(object)value); return true; }
            if (typeof(TOther) == typeof(UltimateOrb.UInt128)) { result = FromUInt128((UltimateOrb.UInt128)(object)value); return true; }
            result = default;
            return false;
        }

        private static BigInteger ClampTo(BigInteger v, BigInteger lo, BigInteger hi) {
            if (v < lo) return lo;
            if (v > hi) return hi;
            return v;
        }

        // Saturating conversion for integer targets (matches .NET's TryConvertTo* pattern).
        private static bool TryConvertToSaturatingHelper<TOther>(FloatS128E31Internal value, out TOther result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(double)) { result = (TOther)(object)ToDouble(value); return true; }
            if (typeof(TOther) == typeof(float)) { result = (TOther)(object)(float)ToDouble(value); return true; }
            BigInteger big = ToBigIntegerTruncated(value);
            if (typeof(TOther) == typeof(sbyte)) { result = (TOther)(object)(sbyte)ClampTo(big, sbyte.MinValue, sbyte.MaxValue); return true; }
            if (typeof(TOther) == typeof(byte)) { result = (TOther)(object)(byte)ClampTo(big, byte.MinValue, byte.MaxValue); return true; }
            if (typeof(TOther) == typeof(short)) { result = (TOther)(object)(short)ClampTo(big, short.MinValue, short.MaxValue); return true; }
            if (typeof(TOther) == typeof(ushort)) { result = (TOther)(object)(ushort)ClampTo(big, ushort.MinValue, ushort.MaxValue); return true; }
            if (typeof(TOther) == typeof(int)) { result = (TOther)(object)(int)ClampTo(big, int.MinValue, int.MaxValue); return true; }
            if (typeof(TOther) == typeof(uint)) { result = (TOther)(object)(uint)ClampTo(big, uint.MinValue, uint.MaxValue); return true; }
            if (typeof(TOther) == typeof(long)) { result = (TOther)(object)(long)ClampTo(big, long.MinValue, long.MaxValue); return true; }
            if (typeof(TOther) == typeof(ulong)) { result = (TOther)(object)(ulong)ClampTo(big, ulong.MinValue, ulong.MaxValue); return true; }
            if (typeof(TOther) == typeof(System.Int128)) { result = (TOther)(object)(System.Int128)ClampTo(big, System.Int128.MinValue, System.Int128.MaxValue); return true; }
            if (typeof(TOther) == typeof(System.UInt128)) { result = (TOther)(object)(System.UInt128)ClampTo(big, System.UInt128.MinValue, System.UInt128.MaxValue); return true; }
            if (typeof(TOther) == typeof(UltimateOrb.Int128)) { result = (TOther)(object)(UltimateOrb.Int128)(System.Int128)ClampTo(big, System.Int128.MinValue, System.Int128.MaxValue); return true; }
            if (typeof(TOther) == typeof(UltimateOrb.UInt128)) { result = (TOther)(object)(UltimateOrb.UInt128)(System.Int128)ClampTo(big, System.UInt128.MinValue, System.UInt128.MaxValue); return true; }
            result = default!;
            return false;
        }

        // Checked: returns false when out of range.
        private static bool TryConvertToCheckedHelper<TOther>(FloatS128E31Internal value, out TOther result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(double)) { result = (TOther)(object)ToDouble(value); return true; }
            if (typeof(TOther) == typeof(float)) { result = (TOther)(object)(float)ToDouble(value); return true; }
            BigInteger big = ToBigIntegerTruncated(value);
            if (typeof(TOther) == typeof(sbyte)) { if (big < sbyte.MinValue || big > sbyte.MaxValue) { result = default!; return false; } result = (TOther)(object)(sbyte)big; return true; }
            if (typeof(TOther) == typeof(byte)) { if (big < byte.MinValue || big > byte.MaxValue) { result = default!; return false; } result = (TOther)(object)(byte)big; return true; }
            if (typeof(TOther) == typeof(short)) { if (big < short.MinValue || big > short.MaxValue) { result = default!; return false; } result = (TOther)(object)(short)big; return true; }
            if (typeof(TOther) == typeof(ushort)) { if (big < ushort.MinValue || big > ushort.MaxValue) { result = default!; return false; } result = (TOther)(object)(ushort)big; return true; }
            if (typeof(TOther) == typeof(int)) { if (big < int.MinValue || big > int.MaxValue) { result = default!; return false; } result = (TOther)(object)(int)big; return true; }
            if (typeof(TOther) == typeof(uint)) { if (big < uint.MinValue || big > uint.MaxValue) { result = default!; return false; } result = (TOther)(object)(uint)big; return true; }
            if (typeof(TOther) == typeof(long)) { if (big < long.MinValue || big > long.MaxValue) { result = default!; return false; } result = (TOther)(object)(long)big; return true; }
            if (typeof(TOther) == typeof(ulong)) { if (big < ulong.MinValue || big > ulong.MaxValue) { result = default!; return false; } result = (TOther)(object)(ulong)big; return true; }
            if (typeof(TOther) == typeof(System.Int128)) { if (big < (BigInteger)System.Int128.MinValue || big > (BigInteger)System.Int128.MaxValue) { result = default!; return false; } result = (TOther)(object)(System.Int128)big; return true; }
            if (typeof(TOther) == typeof(System.UInt128)) { if (big < (BigInteger)System.UInt128.MinValue || big > (BigInteger)System.UInt128.MaxValue) { result = default!; return false; } result = (TOther)(object)(System.UInt128)big; return true; }
            if (typeof(TOther) == typeof(UltimateOrb.Int128)) { if (big < (BigInteger)System.Int128.MinValue || big > (BigInteger)System.Int128.MaxValue) { result = default!; return false; } result = (TOther)(object)(UltimateOrb.Int128)(System.Int128)big; return true; }
            if (typeof(TOther) == typeof(UltimateOrb.UInt128)) { if (big < (BigInteger)System.UInt128.MinValue || big > (BigInteger)System.UInt128.MaxValue) { result = default!; return false; } result = (TOther)(object)(UltimateOrb.UInt128)(System.UInt128)big; return true; }
            result = default!;
            return false;
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out FloatS128E31Internal result)
            where TOther : INumberBase<TOther> => TryConvertFromHelper(value, out result);
        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out FloatS128E31Internal result)
            where TOther : INumberBase<TOther> => TryConvertFromHelper(value, out result);
        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out FloatS128E31Internal result)
            where TOther : INumberBase<TOther> => TryConvertFromHelper(value, out result);

        public static bool TryConvertToChecked<TOther>(FloatS128E31Internal value, [MaybeNullWhen(false)] out TOther result)
            where TOther : INumberBase<TOther> => TryConvertToCheckedHelper(value, out result);
        public static bool TryConvertToSaturating<TOther>(FloatS128E31Internal value, [MaybeNullWhen(false)] out TOther result)
            where TOther : INumberBase<TOther> => TryConvertToSaturatingHelper(value, out result);
        public static bool TryConvertToTruncating<TOther>(FloatS128E31Internal value, [MaybeNullWhen(false)] out TOther result)
            where TOther : INumberBase<TOther> => TryConvertToSaturatingHelper(value, out result);

        // ─── Operators ────────────────────────────────────────────────────────

        public static bool operator ==(FloatS128E31Internal left, FloatS128E31Internal right) => left.Equals(right);
        public static bool operator !=(FloatS128E31Internal left, FloatS128E31Internal right) => !left.Equals(right);
        public static bool operator <(FloatS128E31Internal left, FloatS128E31Internal right) => left.CompareTo(right) < 0;
        public static bool operator >(FloatS128E31Internal left, FloatS128E31Internal right) => left.CompareTo(right) > 0;
        public static bool operator <=(FloatS128E31Internal left, FloatS128E31Internal right) => left.CompareTo(right) <= 0;
        public static bool operator >=(FloatS128E31Internal left, FloatS128E31Internal right) => left.CompareTo(right) >= 0;

        public static FloatS128E31Internal operator +(FloatS128E31Internal left, FloatS128E31Internal right) {
            AddDint(out var r, left, right);
            return r;
        }

        public static FloatS128E31Internal operator -(FloatS128E31Internal left, FloatS128E31Internal right) {
            var negRight = new FloatS128E31Internal(right._significand, right._exp, 1 - right._sgn);
            AddDint(out var r, left, negRight);
            return r;
        }

        public static FloatS128E31Internal operator *(FloatS128E31Internal left, FloatS128E31Internal right) {
            MulDint(out var r, left, right);
            return r;
        }

        public static FloatS128E31Internal operator /(FloatS128E31Internal left, FloatS128E31Internal right) {
            DivDint(out var r, left, right);
            return r;
        }

        public static FloatS128E31Internal operator %(FloatS128E31Internal left, FloatS128E31Internal right) {
            unchecked {
                if (right._significand == 0) throw new DivideByZeroException();
                if (left._significand == 0) return left;

                int commonExp = Math.Min(left._exp, right._exp) - 127;
                BigInteger aVal = ((BigInteger)left._significand) << (left._exp - 127 - commonExp);
                BigInteger bVal = ((BigInteger)right._significand) << (right._exp - 127 - commonExp);
                if (left._sgn == 1) aVal = -aVal;
                if (right._sgn == 1) bVal = -bVal;

                BigInteger rem = BigInteger.Remainder(aVal, bVal);
                if (rem.IsZero) return new FloatS128E31Internal(0, 0, left._sgn);

                return FromBigIntegerScaled(rem, commonExp, 0);
            }
        }

        public static FloatS128E31Internal operator -(FloatS128E31Internal value)
            => new(value._significand, value._exp, 1 - value._sgn);

        public static FloatS128E31Internal operator +(FloatS128E31Internal value) => value;

        public static FloatS128E31Internal operator ++(FloatS128E31Internal value) {
            AddDint(out var r, value, One);
            return r;
        }

        public static FloatS128E31Internal operator --(FloatS128E31Internal value) {
            AddDint(out var r, value, NegativeOne);
            return r;
        }

        // ─── Rounding ─────────────────────────────────────────────────────────

        // Exact q / den to 128-bit significand with RNE, single rounding.
        private static FloatS128E31Internal DivBigIntegerWithRNE(BigInteger num, BigInteger den, uint sgn) {
            unchecked {
                if (num.IsZero) return new FloatS128E31Internal(0, 0, sgn);

                int numBits = (int)num.GetBitLength();
                int denBits = (int)den.GetBitLength();
                int shift = 130 - (numBits - denBits);
                if (shift < 0) shift = 0;

                BigInteger numShifted = num << shift;
                BigInteger q = BigInteger.DivRem(numShifted, den, out BigInteger rem);

                int qBits = (int)q.GetBitLength();
                if (qBits < 128) {
                    int extra = 128 - qBits;
                    numShifted = num << (shift + extra);
                    q = BigInteger.DivRem(numShifted, den, out rem);
                    shift += extra;
                    qBits = (int)q.GetBitLength();
                }

                BigInteger head;
                bool roundUp;
                int qShift = qBits - 128;

                if (qShift == 0) {
                    head = q;
                    int cmp = (2 * rem).CompareTo(den);
                    roundUp = cmp > 0 || (cmp == 0 && !head.IsEven);
                } else {
                    head = q >> qShift;
                    BigInteger tail = q & ((BigInteger.One << qShift) - 1);
                    BigInteger half = BigInteger.One << (qShift - 1);
                    int cmp = tail.CompareTo(half);
                    if (cmp > 0) roundUp = true;
                    else if (cmp < 0) roundUp = false;
                    else roundUp = !rem.IsZero || !head.IsEven;
                }

                if (roundUp) {
                    head += 1;
                    if (head == (BigInteger.One << 128)) { head >>= 1; qShift += 1; }
                }

                int exp = qShift - shift + 127;
                return new FloatS128E31Internal((UInt128)head, exp, sgn);
            }
        }

        public static FloatS128E31Internal Round(FloatS128E31Internal x, int digits, MidpointRounding mode) {
            unchecked {
                if (x._significand == 0) return x;

                BigInteger sig = x._significand;
                int e2 = x._exp - 127;

                // y = |x| * 10^digits as exact rational num/den.
                BigInteger num, den, pow10;
                if (digits >= 0) {
                    pow10 = BigInteger.Pow(10, digits);
                    num = sig * pow10;
                    den = BigInteger.One;
                } else {
                    pow10 = BigInteger.Pow(10, -digits);
                    num = sig;
                    den = pow10;
                }
                if (e2 >= 0) num <<= e2;
                else den <<= -e2;

                // Truncate y to integer q, remainder rem (0 ≤ rem < den).
                BigInteger q = BigInteger.DivRem(num, den, out BigInteger rem);

                int cmp = (2 * rem).CompareTo(den);
                bool roundUp = mode switch {
                    MidpointRounding.ToEven => cmp > 0 || (cmp == 0 && !q.IsEven),
                    MidpointRounding.AwayFromZero => cmp >= 0,
                    MidpointRounding.ToZero => false,
                    MidpointRounding.ToNegativeInfinity => (x._sgn == 1) && !rem.IsZero,
                    MidpointRounding.ToPositiveInfinity => (x._sgn == 0) && !rem.IsZero,
                    _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null),
                };
                if (roundUp) q += 1;

                if (q.IsZero) return new FloatS128E31Internal(0, 0, x._sgn);

                if (digits == 0)
                    return FromBigIntegerScaled(q, 0, x._sgn);
                if (digits < 0)
                    return FromBigIntegerScaled(q * pow10, 0, x._sgn);
                return DivBigIntegerWithRNE(q, pow10, x._sgn);
            }
        }

        public static FloatS128E31Internal Floor(FloatS128E31Internal x) => Round(x, 0, MidpointRounding.ToNegativeInfinity);
        public static FloatS128E31Internal Ceiling(FloatS128E31Internal x) => Round(x, 0, MidpointRounding.ToPositiveInfinity);
        public static FloatS128E31Internal Truncate(FloatS128E31Internal x) => Round(x, 0, MidpointRounding.ToZero);
        public static FloatS128E31Internal Round(FloatS128E31Internal x) => Round(x, 0, MidpointRounding.ToEven);
        public static FloatS128E31Internal Round(FloatS128E31Internal x, int digits)
            => Round(x, digits, MidpointRounding.ToEven);
        public static FloatS128E31Internal Round(FloatS128E31Internal x, MidpointRounding mode)
            => Round(x, 0, mode);

        // ─── Constant computation ─────────────────────────────────────────────

        private static FloatS128E31Internal CalculateE() {
            const int prec = 320;
            BigInteger scale = BigInteger.One << prec;
            BigInteger sum = scale;
            BigInteger term = scale;
            for (int k = 1; k < 250; k++) {
                term /= k;
                if (term.IsZero) break;
                sum += term;
            }
            return FromBigIntegerScaled(sum, -prec, 0);
        }

        private static FloatS128E31Internal CalculatePi() {
            const int prec = 420;
            BigInteger arctan5 = ArctanReciprocal(5, prec);
            BigInteger arctan239 = ArctanReciprocal(239, prec);
            BigInteger pi = 16 * arctan5 - 4 * arctan239;
            return FromBigIntegerScaled(pi, -prec, 0);
        }

        private static BigInteger ArctanReciprocal(int n, int prec) {
            BigInteger scale = BigInteger.One << prec;
            BigInteger sum = BigInteger.Zero;
            BigInteger nPow = n;
            BigInteger nSq = (BigInteger)n * n;
            int k = 0;
            bool positive = true;
            while (true) {
                BigInteger term = scale / (nPow * (2 * k + 1));
                if (term.IsZero) break;
                sum = positive ? sum + term : sum - term;
                nPow *= nSq;
                positive = !positive;
                k++;
            }
            return sum;
        }

        // ─── ToString / Parse / TryFormat — out of scope per instructions ─────

        public string ToString(string? format, IFormatProvider? formatProvider) => throw new NotImplementedException();
        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => throw new NotImplementedException();
        public static FloatS128E31Internal Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
        public static FloatS128E31Internal Parse(string s, NumberStyles style, IFormatProvider? provider) => throw new NotImplementedException();
        public static FloatS128E31Internal Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => throw new NotImplementedException();
        public static FloatS128E31Internal Parse(string s, IFormatProvider? provider) => throw new NotImplementedException();
        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatS128E31Internal result) => throw new NotImplementedException();
        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatS128E31Internal result) => throw new NotImplementedException();
        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatS128E31Internal result) => throw new NotImplementedException();
        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out FloatS128E31Internal result) => throw new NotImplementedException();

        /*
          Approximation tables
        */

        /* _INVERSE_2[128+i] is an approximation of 1/(1+i/2^8), where an entry
           (hi,lo,ex,sgn) represents (-1)^sgn*(hi+lo/2^64)*2^(ex-63) */
        static readonly FloatS128E31Internal[] _INVERSE_2_array = [
            new(hi: 0x8000000000000000, lo: 0x0,  exp: 1, sgn:  0x0),
            new(hi: 0xfe03f80fe03f80ff, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xfc0fc0fc0fc0fc10, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xfa232cf252138ac0, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xf83e0f83e0f83e10, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xf6603d980f6603da, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xf4898d5f85bb3951, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xf2b9d6480f2b9d65, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xf0f0f0f0f0f0f0f1, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xef2eb71fc4345239, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xed7303b5cc0ed731, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xebbdb2a5c1619c8c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xea0ea0ea0ea0ea0f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe865ac7b7603a197, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe6c2b4481cd8568a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe525982af70c880f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe38e38e38e38e38f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe1fc780e1fc780e2, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xe070381c0e070382, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xdee95c4ca037ba58, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xdd67c8a60dd67c8b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xdbeb61eed19c5958, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xda740da740da740e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd901b2036406c80e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd79435e50d79435f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd62b80d62b80d62c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd4c77b03531dec0e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd3680d3680d3680e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd20d20d20d20d20e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xd0b69fcbd2580d0c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xcf6474a8819ec8ea, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xce168a7725080ce2, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xcccccccccccccccd, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xcb8727c065c393e1, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xca4587e6b74f032a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc907da4e871146ad, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc7ce0c7ce0c7ce0d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc6980c6980c6980d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc565c87b5f9d4d1c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc4372f855d824ca6, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc30c30c30c30c30d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc1e4bbd595f6e948, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xc0c0c0c0c0c0c0c1, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xbfa02fe80bfa02ff, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xbe82fa0be82fa0bf, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xbd69104707661aa3, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xbc52640bc52640bd, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xbb3ee721a54d880c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xba2e8ba2e8ba2e8c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb92143fa36f5e02f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb81702e05c0b8171, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb70fbb5a19be3659, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb60b60b60b60b60c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb509e68a9b948220, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb40b40b40b40b40c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb30f63528917c80c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb21642c8590b2165, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb11fd3b80b11fd3c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xb02c0b02c0b02c0c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xaf3addc680af3ade, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xae4c415c9882b932, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xad602b580ad602b6, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xac7691840ac76919, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xab8f69e28359cd12, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xaaaaaaaaaaaaaaab, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa9c84a47a07f5638, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa8e83f5717c0a8e9, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa80a80a80a80a80b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa72f05397829cbc2, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa655c4392d7b73a8, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa57eb50295fad40b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa4a9cf1d96833752, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa3d70a3d70a3d70b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa3065e3fae7cd0e1, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa237c32b16cfd773, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa16b312ea8fc377d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xa0a0a0a0a0a0a0a1, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9fd809fd809fd80a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9f1165e7254813e3, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9e4cad23dd5f3a21, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9d89d89d89d89d8a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9cc8e160c3fb19b9, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9c09c09c09c09c0a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9b4c6f9ef03a3caa, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9a90e7d95bc609aa, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x99d722dabde58f07, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x991f1a515885fb38, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9868c809868c8099, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x97b425ed097b425f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x97012e025c04b80a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x964fda6c0964fda7, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x95a02568095a0257, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x94f2094f2094f20a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x9445809445809446, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x939a85c40939a85d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x92f113840497889d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x924924924924924a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x91a2b3c4d5e6f80a, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x90fdbc090fdbc091, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x905a38633e06c43b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8fb823ee08fb823f, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8f1779d9fdc3a219, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8e78356d1408e784, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8dda520237694809, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8d3dcb08d3dcb08e, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8ca29c046514e024, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8c08c08c08c08c09, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8b70344a139bc75b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8ad8f2fba9386823, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8a42f8705669db47, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x89ae4089ae4089af, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x891ac73ae9819b51, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8888888888888889, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x87f78087f78087f8, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8767ab5f34e47ef2, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x86d905447a34acc7, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x864b8a7de6d1d609, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x85bf37612cee3c9b, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8534085340853409, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x84a9f9c8084a9f9d, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8421084210842109, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x839930523fbe3368, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x83126e978d4fdf3c, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x828cbfbeb9a020a4, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8208208208208209, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x81848da8faf0d278, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8102040810204082, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8000000000000000, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0x8000000000000000, lo: 0x0,  exp: 0, sgn:  0x0),
            new(hi: 0xff00ff00ff00ff02, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xfe03f80fe03f80ff, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xfd08e5500fd08e56, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xfc0fc0fc0fc0fc11, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xfb18856506ddaba7, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xfa232cf252138ac1, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf92fb2211855a866, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf83e0f83e0f83e11, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf74e3fc22c700f76, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf6603d980f6603db, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf57403d5d00f5741, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf4898d5f85bb3951, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf3a0d52cba872337, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf2b9d6480f2b9d66, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf1d48bcee0d399fb, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf0f0f0f0f0f0f0f2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xf00f00f00f00f010, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xef2eb71fc4345239, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xee500ee500ee5010, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xed7303b5cc0ed731, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xec979118f3fc4da3, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xebbdb2a5c1619c8d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xeae56403ab959010, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xea0ea0ea0ea0ea10, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe939651fe2d8d35d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe865ac7b7603a198, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe79372e225fe30da, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe6c2b4481cd8568a, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe5f36cb00e5f36cc, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe525982af70c880f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe45932d7dc52100f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe38e38e38e38e38f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe2c4a6886a4c2e11, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe1fc780e1fc780e3, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe135a9c97500e137, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xe070381c0e070383, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdfac1f74346c5760, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdee95c4ca037ba58, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xde27eb2c41f3d9d2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdd67c8a60dd67c8b, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdca8f158c7f91ab9, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdbeb61eed19c5959, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xdb2f171df770291a, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xda740da740da740f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd9ba4256c0366e92, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd901b2036406c80f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd84a598ec9151f44, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd79435e50d79435f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd6df43fca482f00e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd62b80d62b80d62d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd578e97c3f5fe552, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd4c77b03531dec0e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd4173289870ac52f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd3680d3680d3680e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd2ba083b445250ac, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd20d20d20d20d20e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd161543e28e50275, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd0b69fcbd2580d0c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xd00d00d00d00d00e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcf6474a8819ec8ea, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcebcf8bb5b4169cc, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xce168a7725080ce2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcd712752a886d243, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xccccccccccccccce, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcc29786c7607f9a0, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcb8727c065c393e1, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xcae5d85f1bbd6c96, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xca4587e6b74f032a, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc9a633fcd967300e, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc907da4e871146ae, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc86a78900c86a78a, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc7ce0c7ce0c7ce0d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc73293d789b9f839, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc6980c6980c6980d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc5fe740317f9d00d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc565c87b5f9d4d1d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc4ce07b00c4ce07c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc4372f855d824ca7, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc3a13de60495c774, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc30c30c30c30c30d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc2780613c0309e03, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc1e4bbd595f6e948, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc152500c152500c2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc0c0c0c0c0c0c0c2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xc0300c0300c0300d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbfa02fe80bfa0300, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbf112a8ad278e8de, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbe82fa0be82fa0c0, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbdf59c91700bdf5b, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbd69104707661aa4, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbcdd535db1cc5b7c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbc52640bc52640bd, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbbc8408cd63069a2, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbb3ee721a54d880d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xbab656100bab6562, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xba2e8ba2e8ba2e8d, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb9a7862a0ff46589, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb92143fa36f5e02f, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb89bc36ce3e0453b, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb81702e05c0b8171, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb79300b79300b794, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb70fbb5a19be365a, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb68d31340e4307d9, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb60b60b60b60b60c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb58a485518d1e7e5, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb509e68a9b948220, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb48a39d44685fe98, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb40b40b40b40b40c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb38cf9b00b38cf9c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb30f63528917c80c, lo: 0x0, exp: -1, sgn:  0x0),
            new(hi: 0xb2927c29da5519d0, lo: 0x0, exp: -1, sgn:  0x0),
        ];

        /* _LOG_INV_2[128+i] is an approximation of -log(1/(1+i/2^8)), where an entry
           (hi,lo,ex,sgn) represents (-1)^sgn*(hi+lo/2^64)*2^(ex-63) */
        private static readonly FloatS128E31Internal[] _LOG_INV_2_array = [
            new(hi: 0xb17217f7d1cf79ab, lo: 0xc9e3b39803f2f6af,  exp: -1, sgn:  0x1),
            new(hi: 0xaf74155120c9011d,  lo: 0x46d235ee63073dc,  exp: -1, sgn:  0x1),
            new(hi: 0xad7a02e1b24efd32, lo: 0x160864fd949b4bd3,  exp: -1, sgn:  0x1),
            new(hi: 0xab83d135dc633301, lo: 0xffe6607ba902ef3b,  exp: -1, sgn:  0x1),
            new(hi: 0xa991713433c2b999,  lo: 0xba4aea614d05700,  exp: -1, sgn:  0x1),
            new(hi: 0xa7a2d41ad270c9d7, lo: 0xcd362382a7688479,  exp: -1, sgn:  0x1),
            new(hi: 0xa5b7eb7cb860fb89, lo: 0x7b6a62a0dec6e072,  exp: -1, sgn:  0x1),
            new(hi: 0xa3d0a93f45169a4b,  lo: 0x9594fab088c0d64,  exp: -1, sgn:  0x1),
            new(hi: 0xa1ecff97c91e267b, lo: 0x1b7efae08e597e16,  exp: -1, sgn:  0x1),
            new(hi: 0xa00ce1092e5498c4, lo: 0x69879c5a30cd1241,  exp: -1, sgn:  0x1),
            new(hi: 0x9e304061b5fda91a,  lo: 0x4603d87b6df81ac,  exp: -1, sgn:  0x1),
            new(hi: 0x9c5710b8cbb73a42, lo: 0xaa554b2dd4619e63,  exp: -1, sgn:  0x1),
            new(hi: 0x9a81456cec642e10, lo: 0x4d49f9aaea3cb5e0,  exp: -1, sgn:  0x1),
            new(hi: 0x98aed221a03458b6, lo: 0x732f89321647b358,  exp: -1, sgn:  0x1),
            new(hi: 0x96dfaabd86fa1647, lo: 0xd61188fbc94e2f14,  exp: -1, sgn:  0x1),
            new(hi: 0x9513c36876083696, lo: 0xb5cbc416a2418011,  exp: -1, sgn:  0x1),
            new(hi: 0x934b1089a6dc93c2, lo: 0xbf5bb3b60554e151,  exp: -1, sgn:  0x1),
            new(hi: 0x918586c5f5e4bf01, lo: 0x9f92199ed1a4bab0,  exp: -1, sgn:  0x1),
            new(hi: 0x8fc31afe30b2c6de, lo: 0xe300bf167e95da66,  exp: -1, sgn:  0x1),
            new(hi: 0x8e03c24d7300395a, lo: 0xcddae1ccce247837,  exp: -1, sgn:  0x1),
            new(hi: 0x8c47720791e53314, lo: 0x762ad19415fe25a5,  exp: -1, sgn:  0x1),
            new(hi: 0x8a8e1fb794b09134, lo: 0x9eb628dba173c82d,  exp: -1, sgn:  0x1),
            new(hi: 0x88d7c11e3ad53cdc, lo: 0x8a3111a707b6de2c,  exp: -1, sgn:  0x1),
            new(hi: 0x87244c308e670a66, lo: 0x85e005d06dbfa8f7,  exp: -1, sgn:  0x1),
            new(hi: 0x8573b71682a7d21b, lo: 0xb21f9f89c1ab80b2,  exp: -1, sgn:  0x1),
            new(hi: 0x83c5f8299e2b4091, lo: 0xb8f6fafe8fbb68b8,  exp: -1, sgn:  0x1),
            new(hi: 0x821b05f3b01d6774, lo: 0xdb0d58c3f7e2ea1e,  exp: -1, sgn:  0x1),
            new(hi: 0x8072d72d903d588c, lo: 0x7dd1b09c70c40109,  exp: -1, sgn:  0x1),
            new(hi: 0xfd9ac57bd2442180, lo: 0xaf05924d258c14c4,  exp: -2, sgn:  0x1),
            new(hi: 0xfa553f7018c966f4, lo: 0x2780a545a1b54dce,  exp: -2, sgn:  0x1),
            new(hi: 0xf7150ab5a09f27f6,  lo: 0xa470250d40ebe8e,  exp: -2, sgn:  0x1),
            new(hi: 0xf3da161eed6b9ab1, lo: 0x248d42f78d3e65d2,  exp: -2, sgn:  0x1),
            new(hi: 0xf0a450d139366ca7, lo: 0x7c66eb6408ff6432,  exp: -2, sgn:  0x1),
            new(hi: 0xed73aa4264b0adeb, lo: 0x5391cf4b33e42996,  exp: -2, sgn:  0x1),
            new(hi: 0xea481236f7d35bb2, lo: 0x39a767a80d6d97e6,  exp: -2, sgn:  0x1),
            new(hi: 0xe72178c0323a1a0f, lo: 0xcc4e1653e71d9973,  exp: -2, sgn:  0x1),
            new(hi: 0xe3ffce3a2aa64923, lo: 0x8eadb651b49ac539,  exp: -2, sgn:  0x1),
            new(hi: 0xe0e30349fd1cec82,  lo: 0x3e8e1802aba24d5,  exp: -2, sgn:  0x1),
            new(hi: 0xddcb08dc0717d85c, lo: 0x940a666c87842842,  exp: -2, sgn:  0x1),
            new(hi: 0xdab7d02231484a93, lo: 0xbec20cca6efe2ac4,  exp: -2, sgn:  0x1),
            new(hi: 0xd7a94a92466e833c, lo: 0xcd88bba7d0cee8df,  exp: -2, sgn:  0x1),
            new(hi: 0xd49f69e456cf1b7b, lo: 0x7f53bd2e406e66e6,  exp: -2, sgn:  0x1),
            new(hi: 0xd19a201127d3c646, lo: 0x279d79f51dcc7301,  exp: -2, sgn:  0x1),
            new(hi: 0xce995f50af69d863, lo: 0x432f3f4f861ad6a8,  exp: -2, sgn:  0x1),
            new(hi: 0xcb9d1a189ab56e77, lo: 0x7d7e9307c70c0667,  exp: -2, sgn:  0x1),
            new(hi: 0xc8a5431adfb44ca6,  lo: 0x48ce7c1a75e341a,  exp: -2, sgn:  0x1),
            new(hi: 0xc5b1cd44596fa51f, lo: 0xf218fb8f9f9ef27f,  exp: -2, sgn:  0x1),
            new(hi: 0xc2c2abbb6e5fd570,  lo: 0x3337789d592e296,  exp: -2, sgn:  0x1),
            new(hi: 0xbfd7d1dec0a8df70, lo: 0x37eda996244bccaf,  exp: -2, sgn:  0x1),
            new(hi: 0xbcf13343e7d9ec7f, lo: 0x2afd17781bb3afea,  exp: -2, sgn:  0x1),
            new(hi: 0xba0ec3b633dd8b0b, lo: 0x91dc60b2b059a609,  exp: -2, sgn:  0x1),
            new(hi: 0xb730773578cb90b3, lo: 0xaa1116c3466beb6c,  exp: -2, sgn:  0x1),
            new(hi: 0xb45641f4e350a0d4, lo: 0xe756eba00bc33976,  exp: -2, sgn:  0x1),
            new(hi: 0xb1801859d56249de, lo: 0x98ce51fff99479cb,  exp: -2, sgn:  0x1),
            new(hi: 0xaeadeefacaf97d37, lo: 0x9dd6e688ebb13b01,  exp: -2, sgn:  0x1),
            new(hi: 0xabdfba9e468fd6f9, lo: 0x472ea07749ce6bd1,  exp: -2, sgn:  0x1),
            new(hi: 0xa9157039c51ebe72, lo: 0xe164c759686a2207,  exp: -2, sgn:  0x1),
            new(hi: 0xa64f04f0b961df78, lo: 0x54f5275c2d15c21e,  exp: -2, sgn:  0x1),
            new(hi: 0xa38c6e138e20d834, lo: 0xd698298adddd7f30,  exp: -2, sgn:  0x1),
            new(hi: 0xa0cda11eaf46390e, lo: 0x632438273918db7d,  exp: -2, sgn:  0x1),
            new(hi: 0x9e1293b9998c1dad, lo: 0x3b035eae273a855c,  exp: -2, sgn:  0x1),
            new(hi: 0x9b5b3bb5f088b768, lo: 0x5078bbe3d392be24,  exp: -2, sgn:  0x1),
            new(hi: 0x98a78f0e9ae71d87, lo: 0x64dec34784707838,  exp: -2, sgn:  0x1),
            new(hi: 0x95f783e6e49a9cfc,  lo: 0x25004f3ef063312,  exp: -2, sgn:  0x1),
            new(hi: 0x934b1089a6dc93c2, lo: 0xdf5bb3b60554e151,  exp: -2, sgn:  0x1),
            new(hi: 0x90a22b6875c6a1f8, lo: 0x8e91aeba609c8876,  exp: -2, sgn:  0x1),
            new(hi: 0x8dfccb1ad35ca6ef, lo: 0x9947bdb6ddcaf59a,  exp: -2, sgn:  0x1),
            new(hi: 0x8b5ae65d67db9acf, lo: 0x7ba5168126a58b99,  exp: -2, sgn:  0x1),
            new(hi: 0x88bc74113f23def3, lo: 0xbc5a0fe396f40f1c,  exp: -2, sgn:  0x1),
            new(hi: 0x86216b3b0b17188c, lo: 0x363ceae88f720f1d,  exp: -2, sgn:  0x1),
            new(hi: 0x8389c3026ac3139d, lo: 0x6adda9d2270fa1f3,  exp: -2, sgn:  0x1),
            new(hi: 0x80f572b1363487bc, lo: 0xedbd0b5b3479d5f2,  exp: -2, sgn:  0x1),
            new(hi: 0xfcc8e3659d9bcbf1, lo: 0x8a0cdf301431b60b,  exp: -3, sgn:  0x1),
            new(hi: 0xf7ad6f26e7ff2efc, lo: 0x9cd2238f75f969ad,  exp: -3, sgn:  0x1),
            new(hi: 0xf29877ff38809097, lo: 0x2b020fa1820c948d,  exp: -3, sgn:  0x1),
            new(hi: 0xed89ed86a44a01ab,  lo: 0x9d49f96cb88317a,  exp: -3, sgn:  0x1),
            new(hi: 0xe881bf932af3dac3, lo: 0x2524848e3443e03f,  exp: -3, sgn:  0x1),
            new(hi: 0xe37fde37807b84e3, lo: 0x5e9a750b6b68781c,  exp: -3, sgn:  0x1),
            new(hi: 0xde8439c1dec5687c, lo: 0x9d57da945b5d0aa6,  exp: -3, sgn:  0x1),
            new(hi: 0xd98ec2bade71e53e, lo: 0xd0a98f2ad65bee96,  exp: -3, sgn:  0x1),
            new(hi: 0xd49f69e456cf1b7a, lo: 0x5f53bd2e406e66e7,  exp: -3, sgn:  0x1),
            new(hi: 0xcfb6203844b3209b, lo: 0x18cb02f33f79c16b,  exp: -3, sgn:  0x1),
            new(hi: 0xcad2d6e7b80bf915, lo: 0xcc507fb7a3d0bf69,  exp: -3, sgn:  0x1),
            new(hi: 0xc5f57f59c7f46156, lo: 0x9a8b6997a402bf30,  exp: -3, sgn:  0x1),
            new(hi: 0xc11e0b2a8d1e0de1, lo: 0xda631e830fd308fe,  exp: -3, sgn:  0x1),
            new(hi: 0xbc4c6c2a226399f6, lo: 0x276ebcfb2016a433,  exp: -3, sgn:  0x1),
            new(hi: 0xb780945bab55dcea, lo: 0xb4c7bc3d32750fd9,  exp: -3, sgn:  0x1),
            new(hi: 0xb2ba75f46099cf8f, lo: 0x243c2e77904afa76,  exp: -3, sgn:  0x1),
            new(hi: 0xadfa035aa1ed8fdd, lo: 0x549767e410316d2b,  exp: -3, sgn:  0x1),
            new(hi: 0xa93f2f250dac67d5, lo: 0x9ad2fb8d48054add,  exp: -3, sgn:  0x1),
            new(hi: 0xa489ec199dab06f4, lo: 0x59fb6cf0ecb411b7,  exp: -3, sgn:  0x1),
            new(hi: 0x9fda2d2cc9465c52, lo: 0x6b2b9565f5355180,  exp: -3, sgn:  0x1),
            new(hi: 0x9b2fe580ac80b182,  lo: 0x11a5b944aca8705,  exp: -3, sgn:  0x1),
            new(hi: 0x968b08643409ceb9, lo: 0xd5c0da506a088482,  exp: -3, sgn:  0x1),
            new(hi: 0x91eb89524e100d28, lo: 0xbfd3df5c52d67e77,  exp: -3, sgn:  0x1),
            new(hi: 0x8d515bf11fb94f22, lo: 0xa0713268840cbcbb,  exp: -3, sgn:  0x1),
            new(hi: 0x88bc74113f23def7, lo: 0x9c5a0fe396f40f19,  exp: -3, sgn:  0x1),
            new(hi: 0x842cc5acf1d0344b, lo: 0x6fecdfa819b96092,  exp: -3, sgn:  0x1),
            new(hi: 0xff4489cedeab2ca6, lo: 0xe17bd40d8d9291ec,  exp: -4, sgn:  0x1),
            new(hi: 0xf639cc185088fe62, lo: 0x5066e87f2c0f733d,  exp: -4, sgn:  0x1),
            new(hi: 0xed393b1c22351281, lo: 0xff4e2e660317d55f,  exp: -4, sgn:  0x1),
            new(hi: 0xe442c00de2591b4c, lo: 0xe96ab34ce0bccd10,  exp: -4, sgn:  0x1),
            new(hi: 0xdb56446d6ad8df09, lo: 0x28112e35a60e636f,  exp: -4, sgn:  0x1),
            new(hi: 0xd273b2058de1bd4b, lo: 0x36bbf837b4d320c6,  exp: -4, sgn:  0x1),
            new(hi: 0xc99af2eaca4c457b, lo: 0xeaf51f66692844b2,  exp: -4, sgn:  0x1),
            new(hi: 0xc0cbf17a071f80e9, lo: 0x396ffdf76a147cc2,  exp: -4, sgn:  0x1),
            new(hi: 0xb8069857560707a7,  lo: 0xa677b4c8bec22e0,  exp: -4, sgn:  0x1),
            new(hi: 0xaf4ad26cbc8e5bef, lo: 0x9e8b8b88a14ff0c9,  exp: -4, sgn:  0x1),
            new(hi: 0xa6988ae903f562f1, lo: 0x7e858f08597b3a68,  exp: -4, sgn:  0x1),
            new(hi: 0x9defad3e8f732186, lo: 0x476d3b5b45f6ca02,  exp: -4, sgn:  0x1),
            new(hi: 0x9550252238bd2468, lo: 0x658e5a0b811c596d,  exp: -4, sgn:  0x1),
            new(hi: 0x8cb9de8a32ab3694, lo: 0x97c9859530a4514c,  exp: -4, sgn:  0x1),
            new(hi: 0x842cc5acf1d0344c, lo: 0x1fecdfa819b96094,  exp: -4, sgn:  0x1),
            new(hi: 0xf7518e0035c3dd92, lo: 0x606d89093278a931,  exp: -5, sgn:  0x1),
            new(hi: 0xe65b9e6eed965c4f, lo: 0x609f5fe2058d5ff2,  exp: -5, sgn:  0x1),
            new(hi: 0xd5779687d887e0ee, lo: 0x49dda17056e45ebb,  exp: -5, sgn:  0x1),
            new(hi: 0xc4a550a4fd9a19bb, lo: 0x3e97660a23cc5402,  exp: -5, sgn:  0x1),
            new(hi: 0xb3e4a796a5dac213,  lo: 0x7cca0bcc06c2f8e,  exp: -5, sgn:  0x1),
            new(hi: 0xa33576a16f1f4c79, lo: 0x121016bd904dc95a,  exp: -5, sgn:  0x1),
            new(hi: 0x9297997c68c1f4e6, lo: 0x610db3d4dd423bc9,  exp: -5, sgn:  0x1),
            new(hi: 0x820aec4f3a222397, lo: 0xb9e3aea6c444eef6,  exp: -5, sgn:  0x1),
            new(hi: 0xe31e9760a5578c6d, lo: 0xf9eb2f284f31c35a,  exp: -6, sgn:  0x1),
            new(hi: 0xc24929464655f482, lo: 0xda5f3cc0b3251da6,  exp: -6, sgn:  0x1),
            new(hi: 0xa195492cc0660519, lo: 0x4a18dff7cdb4ae33,  exp: -6, sgn:  0x1),
            new(hi: 0x8102b2c49ac23a86, lo: 0x91d082dce3ddcd08,  exp: -6, sgn:  0x1),
            new(hi: 0xc122451c45155150, lo: 0xb16137f09a002b0e,  exp: -7, sgn:  0x1),
            new(hi: 0x8080abac46f389c4, lo: 0x662d417ced0079c9,  exp: -7, sgn:  0x1),
            new(hi:  0x0,                lo: 0x0, exp: 127, sgn:  0x0),
            new(hi:  0x0,                lo: 0x0, exp: 127, sgn:  0x0),
            new(hi: 0xff805515885e014e, lo: 0x435ab4da6a5bb50f,  exp: -9, sgn:  0x0),
            new(hi: 0xff015358833c4762, lo: 0xbb481c8ee1416999,  exp: -8, sgn:  0x0),
            new(hi: 0xbee23afc0853b6a8, lo: 0xa89782c20df350c2,  exp: -7, sgn:  0x0),
            new(hi: 0xfe054587e01f1e2b, lo: 0xf6d3a69bd5eab72f,  exp: -7, sgn:  0x0),
            new(hi: 0x9e75221a352ba751, lo: 0x452b7ea62f2198ea,  exp: -6, sgn:  0x0),
            new(hi: 0xbdc8d83ead88d518, lo: 0x7faa638b5e00ee90,  exp: -6, sgn:  0x0),
            new(hi: 0xdcfe013d7c8cbfc5, lo: 0x632dbac46f30d009,  exp: -6, sgn:  0x0),
            new(hi: 0xfc14d873c1980236, lo: 0xc7e09e3de453f5fc,  exp: -6, sgn:  0x0),
            new(hi: 0x8d86cc491ecbfe03, lo: 0xf1776453b7e82558,  exp: -5, sgn:  0x0),
            new(hi: 0x9cf43dcff5eafd2f, lo: 0x2ad90155c8a7236a,  exp: -5, sgn:  0x0),
            new(hi: 0xac52dd7e4726a456, lo: 0xa47a963a91bb3018,  exp: -5, sgn:  0x0),
            new(hi: 0xbba2c7b196e7e224, lo: 0xe7950f7252c163cf,  exp: -5, sgn:  0x0),
            new(hi: 0xcae41876471f5bde, lo: 0x91d00a417e330f8e,  exp: -5, sgn:  0x0),
            new(hi: 0xda16eb88cb8df5fb, lo: 0x28a63ecfb66e94c0,  exp: -5, sgn:  0x0),
            new(hi: 0xe93b5c56d85a9083, lo: 0xce2992bfea38e76b,  exp: -5, sgn:  0x0),
            new(hi: 0xf85186008b1532f9, lo: 0xe64b8b7759978998,  exp: -5, sgn:  0x0),
            new(hi: 0x83acc1acc7238978, lo: 0x5a5333c45b7f442e,  exp: -4, sgn:  0x0),
            new(hi: 0x8b29b7751bd7073b,  lo: 0x2e0b9ee992f2372,  exp: -4, sgn:  0x0),
            new(hi: 0x929fb17850a0b7be, lo: 0x5b4d3807660516a4,  exp: -4, sgn:  0x0),
            new(hi: 0x9a0ebcb0de8e848e, lo: 0x2c1bb082689ba814,  exp: -4, sgn:  0x0),
            new(hi: 0xa176e5f5323781d2, lo: 0xdcf935996c92e8d4,  exp: -4, sgn:  0x0),
            new(hi: 0xa8d839f830c1fb40, lo: 0x4c7343517c8ac264,  exp: -4, sgn:  0x0),
            new(hi: 0xb032c549ba861d83, lo: 0x774e27bc92ce3373,  exp: -4, sgn:  0x0),
            new(hi: 0xb78694572b5a5cd3, lo: 0x24cdcf68cdb2067c,  exp: -4, sgn:  0x0),
            new(hi: 0xbed3b36bd8966419, lo: 0x7c0644d7d9ed08b4,  exp: -4, sgn:  0x0),
            new(hi: 0xc61a2eb18cd907a1, lo: 0xe5a1532f6d5a1ac1,  exp: -4, sgn:  0x0),
            new(hi: 0xcd5a1231019d66d7, lo: 0x761e3e7b171e44b2,  exp: -4, sgn:  0x0),
            new(hi: 0xd49369d256ab1b1f, lo: 0x9e9154e1d5263cda,  exp: -4, sgn:  0x0),
            new(hi: 0xdbc6415d876d0839, lo: 0x3e33c0c9f8824f54,  exp: -4, sgn:  0x0),
            new(hi: 0xe2f2a47ade3a18a8, lo: 0xa0bf7c0b0d8bb4ef,  exp: -4, sgn:  0x0),
            new(hi: 0xea189eb3659aeaeb, lo: 0x93b2a3b21f448259,  exp: -4, sgn:  0x0),
            new(hi: 0xf1383b7157972f48, lo: 0x543fff0ff4f0aaf1,  exp: -4, sgn:  0x0),
            new(hi: 0xf85186008b153302, lo: 0x5e4b8b7759978993,  exp: -4, sgn:  0x0),
            new(hi: 0xff64898edf55d548, lo: 0x428ccfc99271dffa,  exp: -4, sgn:  0x0),
            new(hi: 0x8338a89652cb714a, lo: 0xb247eb86498c2ce7,  exp: -3, sgn:  0x0),
            new(hi: 0x86bbf3e68472cb2f,  lo: 0xb8bd20615747126,  exp: -3, sgn:  0x0),
            new(hi: 0x8a3c2c233a156341, lo: 0x9027c74fe0e6f64f,  exp: -3, sgn:  0x0),
            new(hi: 0x8db956a97b3d0143, lo: 0xf023472cd739f9e1,  exp: -3, sgn:  0x0),
            new(hi: 0x913378c852d65be6, lo: 0x977e3013d10f7525,  exp: -3, sgn:  0x0),
            new(hi: 0x94aa97c0ffa91a5d, lo: 0x4ee3880fb7d34429,  exp: -3, sgn:  0x0),
            new(hi: 0x981eb8c723fe97f2, lo: 0x1f1c134fb702d433,  exp: -3, sgn:  0x0),
            new(hi: 0x9b8fe100f47ba1d8,  lo: 0x4b62af189fcba0d,  exp: -3, sgn:  0x0),
            new(hi: 0x9efe158766314e4f, lo: 0x4d71827efe892fc8,  exp: -3, sgn:  0x0),
            new(hi: 0xa2695b665be8f338, lo: 0x4eca87c3f0f06211,  exp: -3, sgn:  0x0),
            new(hi: 0xa5d1b79cd2af2aca, lo: 0x8837986ceabfbed6,  exp: -3, sgn:  0x0),
            new(hi: 0xa9372f1d0da1bd10, lo: 0x580eb71e58cd36e5,  exp: -3, sgn:  0x0),
            new(hi: 0xac99c6ccc1042e94, lo: 0x3dd557528315838d,  exp: -3, sgn:  0x0),
            new(hi: 0xaff983853c9e9e40, lo: 0x5f105039091dd7f5,  exp: -3, sgn:  0x0),
            new(hi: 0xb3566a13956a86f4, lo: 0x471b1e1574d9fd55,  exp: -3, sgn:  0x0),
            new(hi: 0xb6b07f38ce90e463, lo: 0x7bb2e265d0de37e1,  exp: -3, sgn:  0x0),
            new(hi: 0xba07c7aa01bd2648, lo: 0x43f9d57b324bd05f,  exp: -3, sgn:  0x0),
            new(hi: 0xbd5c481086c848db, lo: 0xbb596b5030403242,  exp: -3, sgn:  0x0),
            new(hi: 0xc0ae050a1abf56ad, lo: 0x2f7f8c5fa9c50d76,  exp: -3, sgn:  0x0),
            new(hi: 0xc3fd03290648847d, lo: 0x30480bee4cbbd698,  exp: -3, sgn:  0x0),
            new(hi: 0xc74946f4436a054e, lo: 0xf4f5cb531201c0d3,  exp: -3, sgn:  0x0),
            new(hi: 0xca92d4e7a2b5a3ad, lo: 0xc983a9c5c4b3b135,  exp: -3, sgn:  0x0),
            new(hi: 0xcdd9b173efdc1aaa, lo: 0x8863e007c184a1e7,  exp: -3, sgn:  0x0),
            new(hi: 0xd11de0ff15ab18c6, lo: 0xd88d83d4cc613f21,  exp: -3, sgn:  0x0),
            new(hi: 0xd45f67e44178c612, lo: 0x5486e73c615158b4,  exp: -3, sgn:  0x0),
            new(hi: 0xd79e4a7405ff96c3, lo: 0x1300c9be67ae5da0,  exp: -3, sgn:  0x0),
            new(hi: 0xdada8cf47dad236d, lo: 0xdffb833c3409ee7e,  exp: -3, sgn:  0x0),
            new(hi: 0xde1433a16c66b14c, lo: 0xde744870f54f0f18,  exp: -3, sgn:  0x0),
            new(hi: 0xe14b42ac60c60512, lo: 0x4e38eb8092a01f06,  exp: -3, sgn:  0x0),
            new(hi: 0xe47fbe3cd4d10d5b, lo: 0x2ec0f797fdcd125c,  exp: -3, sgn:  0x0),
            new(hi: 0xe7b1aa704e2ee240, lo: 0xb40faab6d2ad0841,  exp: -3, sgn:  0x0),
            new(hi: 0xeae10b5a7ddc8ad8, lo: 0x806b2fc9a8038790,  exp: -3, sgn:  0x0),
            new(hi: 0xee0de5055f63eb01, lo: 0x90a33316df83ba5a,  exp: -3, sgn:  0x0),
            new(hi: 0xf1383b7157972f4a, lo: 0xb43fff0ff4f0aaf1,  exp: -3, sgn:  0x0),
            new(hi: 0xf460129552d2ff41, lo: 0xe62e3201bb2bbdce,  exp: -3, sgn:  0x0),
            new(hi: 0xf7856e5ee2c9b28a, lo: 0x76f2a1b84190a7dc,  exp: -3, sgn:  0x0),
            new(hi: 0xfaa852b25bd9b833, lo: 0xa6dbfa03186e0666,  exp: -3, sgn:  0x0),
            new(hi: 0xfdc8c36af1f15468,  lo: 0xa3361bca696504a,  exp: -3, sgn:  0x0),
            new(hi: 0x8073622d6a80e631, lo: 0xe897009015316073,  exp: -2, sgn:  0x0),
            new(hi: 0x82012ca5a68206d5, lo: 0x8fde85afdd2bc88a,  exp: -2, sgn:  0x0),
            new(hi: 0x838dc2fe6ac868e7, lo: 0x1a3fcbdef40100cb,  exp: -2, sgn:  0x0),
            new(hi: 0x851927139c871af8, lo: 0x67bd00c38061c51f,  exp: -2, sgn:  0x0),
            new(hi: 0x86a35abcd5ba5901, lo: 0x5481c3cbd925ccd2,  exp: -2, sgn:  0x0),
            new(hi: 0x882c5fcd7256a8c1, lo: 0x39055a6598e7c29e,  exp: -2, sgn:  0x0),
            new(hi: 0x89b438149d4582f5, lo: 0x34531dba493eb5a6,  exp: -2, sgn:  0x0),
            new(hi: 0x8b3ae55d5d30701a, lo: 0xc63eab8837170480,  exp: -2, sgn:  0x0),
            new(hi: 0x8cc0696ea11b7b36, lo: 0x94361c9a28d38a6a,  exp: -2, sgn:  0x0),
            new(hi: 0x8e44c60b4ccfd7dc, lo: 0x1473aa01c7778679,  exp: -2, sgn:  0x0),
            new(hi: 0x8fc7fcf24517946a, lo: 0x380cbe769f2c6793,  exp: -2, sgn:  0x0),
            new(hi: 0x914a0fde7bcb2d0e, lo: 0xc429ed3aea197a60,  exp: -2, sgn:  0x0),
            new(hi: 0x92cb0086fbb1cf75, lo: 0xa29d47c50b1182d0,  exp: -2, sgn:  0x0),
            new(hi: 0x944ad09ef4351af1, lo: 0xa49827e081cb16ba,  exp: -2, sgn:  0x0),
            new(hi: 0x95c981d5c4e924ea, lo: 0x45404f5aa577d6b4,  exp: -2, sgn:  0x0),
            new(hi: 0x974715d708e984dd, lo: 0x6648d42840d9e6fb,  exp: -2, sgn:  0x0),
            new(hi: 0x98c38e4aa20c27d2, lo: 0x846767ec990d7333,  exp: -2, sgn:  0x0),
            new(hi: 0x9a3eecd4c3eaa6ae, lo: 0xdb3a7f6e6087b947,  exp: -2, sgn:  0x0),
            new(hi: 0x9bb93315fec2d790, lo: 0x7f589fba0865790f,  exp: -2, sgn:  0x0),
            new(hi: 0x9d3262ab4a2f4e37, lo: 0xa1ae6ba06846fae0,  exp: -2, sgn:  0x0),
            new(hi: 0x9eaa7d2e0fb87c35, lo: 0xff472bc6ce648a7d,  exp: -2, sgn:  0x0),
            new(hi: 0xa0218434353f1de4, lo: 0xd493efa632530acc,  exp: -2, sgn:  0x0),
            new(hi: 0xa197795027409daa, lo: 0x1dd1d4a6df960357,  exp: -2, sgn:  0x0),
            new(hi: 0xa30c5e10e2f613e4, lo: 0x9bd9bd99e39a20b3,  exp: -2, sgn:  0x0),
            new(hi: 0xa4803402004e865c, lo: 0x31cbe0e8824116cd,  exp: -2, sgn:  0x0),
            new(hi: 0xa5f2fcabbbc506d8, lo: 0x68ca4fb7ec323d74,  exp: -2, sgn:  0x0),
            new(hi: 0xa764b99300134d79,  lo: 0xd04d10474301862,  exp: -2, sgn:  0x0),
            new(hi: 0xa8d56c396fc1684c,  lo: 0x1eb067d578c4756,  exp: -2, sgn:  0x0),
            new(hi: 0xaa45161d6e93167b, lo: 0x9b081cf72249f5b2,  exp: -2, sgn:  0x0),
            new(hi: 0xabb3b8ba2ad362a1, lo: 0x1db6506cc17a01f5,  exp: -2, sgn:  0x0),
            new(hi: 0xad215587a67f0cdf, lo: 0xe890422cb86b7cb1,  exp: -2, sgn:  0x0),
            new(hi: 0xae8dedfac04e5282, lo: 0xac707b8ffc22b3e8,  exp: -2, sgn:  0x0),
            new(hi: 0xaff983853c9e9e3f, lo: 0xc5105039091dd7f8,  exp: -2, sgn:  0x0),
            new(hi: 0xb1641795ce3ca978, lo: 0xfaf915300e517393,  exp: -2, sgn:  0x0),
            new(hi: 0xb2cdab981f0f940b, lo: 0xc857c77dc1df600f,  exp: -2, sgn:  0x0),
            new(hi: 0xb43640f4d8a5761f, lo: 0xf5f080a71c34b25d,  exp: -2, sgn:  0x0),
            new(hi: 0xb59dd911aca1ec48, lo: 0x1d2664cf09a0c1bf,  exp: -2, sgn:  0x0),
            new(hi: 0xb70475515d0f1c5e, lo: 0x4c98c6b8be17818d,  exp: -2, sgn:  0x0),
            new(hi: 0xb86a1713c491aeaa, lo: 0xd37ee2872a6f1cd6,  exp: -2, sgn:  0x0),
        ];

        private static readonly FloatS128E31Internal[] P_2_array = [
            new(hi: 0x99df88a0430813ca, lo: 0xa1cffb6e966a70f6, exp: -4, sgn: 0x0),
            new(hi: 0xaaa02d43f696c3e4, lo: 0x4dbe754667b6bc48, exp: -4, sgn:  0x1),
            new(hi: 0xba2e7a1eaf856174, lo: 0x70e5c5a5ebbe0226, exp: -4, sgn:  0x0),
            new(hi: 0xccccccb9ec017492, lo: 0xf934e28d924e76d4, exp: -4, sgn:  0x1),
            new(hi: 0xe38e38e3807cfa4b, lo: 0xc976e6cbd22e203f, exp: -4, sgn:  0x0),
            new(hi: 0xfffffffffff924cc,  lo: 0x5b308e39fa7dfb5, exp: -4, sgn:  0x1),
            new(hi: 0x924924924924911d, lo: 0x862bc3d33abb3649, exp: -3, sgn:  0x0),
            new(hi: 0xaaaaaaaaaaaaaaaa, lo: 0x6637fd4b19743eec, exp: -3, sgn:  0x1),
            new(hi: 0xcccccccccccccccc, lo: 0xccc2ca18b08fe343, exp: -3, sgn:  0x0),
            new(hi: 0xffffffffffffffff, lo: 0xffffff2245823ae0, exp: -3, sgn:  0x1),
            new(hi: 0xaaaaaaaaaaaaaaaa, lo: 0xaaaaaaaaa5c48b54, exp: -2, sgn:  0x0),
            new(hi: 0xffffffffffffffff, lo: 0xffffffffffffebd8, exp: -2, sgn:  0x1),
            new(hi: 0x8000000000000000,                lo: 0x0,  exp: 0, sgn:  0x0),
        ];

        internal static ReadOnlySpan<FloatS128E31Internal> P_2 => P_2_array;
    }

}

