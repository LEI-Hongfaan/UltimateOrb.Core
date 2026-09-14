using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Compile-time metadata for an IEEE-style binary floating-point format
    /// represented by <typeparamref name="T"/>, derived exclusively from the
    /// elementary arithmetic behaviour of
    /// <see cref="IBinaryFloatingPointIeee754{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// Any type implementing <see cref="IBinaryFloatingPointIeee754{T}"/> — a
    /// value type or a heap-allocated class. The physical storage width is
    /// never observed: only the arithmetic is.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// Only <c>T.Zero</c>, <c>T.One</c> and the operators <c>+ - * /</c> are
    /// used. No <c>sizeof</c>, no <c>ILogB</c>, no <c>BitIncrement</c>, no
    /// <c>MaxMagnitude</c>, no <c>Precision</c>. This makes the class safe to
    /// use as a building block while authoring <typeparamref name="T"/> itself.
    /// </para>
    /// <list type="number">
    ///   <item><description>
    ///   <b>precision <c>p</c></b> — halve <c>u</c> from <c>1</c> until
    ///   <c>1 + u == 1</c>. The ULP at <c>1</c> is <c>2^(1-p)</c>, so the loop
    ///   count equals <c>p</c>.
    ///   </description></item>
    ///   <item><description>
    ///   <b>maximum exponent <c>emax</c></b> — exponential search followed by
    ///   binary search on the exponent, using the finiteness test
    ///   <c>x + x != x</c>. Runs in <c>O(log emax)</c> evaluations of
    ///   <c>2^e</c>, each of which is <c>O(log e)</c> multiplications by
    ///   squaring.
    ///   </description></item>
    ///   <item><description>
    ///   <b>underflow depth</b> — verified by checking
    ///   <c>2^-(emax+p-2) > 0</c> and <c>2^-(emax+p-1) == 0</c> with an
    ///   exponentiation-by-squaring helper. <c>O(log(emax+p))</c>.
    ///   </description></item>
    /// </list>
    /// <para>
    /// The standard IEEE bias <c>emax = 2^(w-1) - 1</c> yields
    /// <c>w = log2(emax + 1) + 1</c>. The implicit-leading-bit convention gives
    /// <c>t = p - 1</c> and the logical width <c>k = w + p</c>. No constraint is
    /// placed on <c>k</c>: widths of 8, 19, 24, 80, 128, etc. are all
    /// supported.
    /// </para>
    /// <para>
    /// A type is accepted iff <c>p ≥ 2</c>, <c>w ≥ 1</c>, <c>t ≥ 1</c>, the
    /// standard-bias check succeeds, and the underflow check confirms
    /// <c>2^(2-emax-p)</c> is the smallest positive value.
    /// </para>
    /// <para>
    /// All derived values are computed once per closed generic type and cached in
    /// a single <see langword="static"/> <see langword="readonly"/> tuple. There
    /// is no explicit static constructor, so the type keeps the
    /// <c>beforefieldinit</c> flag and the runtime may run the (non-throwing)
    /// initializer eagerly. Validation lives on the public getters; use
    /// <see cref="IsSupported"/> to probe a type without triggering an exception.
    /// </para>
    /// </remarks>
    [Experimental("UoWIP")]
    internal static partial class BinaryFloatingPointIeee754TypeTraitsInternal<T>
        where T : IBinaryFloatingPointIeee754<T> {

        // =====================================================================
        // No explicit static constructor: keeps 'beforefieldinit'. All work is
        // done in ComputeData(), which is non-throwing; validation happens on
        // the public getters so that IsSupported stays a safe probe.
        // =====================================================================

        private static readonly (
            int Precision,
            int MaxExponent,
            int ExponentFieldBitWidth,
            int TrailingSignificandFieldBitWidth,
            int StorageBitWidth,
            bool IsSupported) s_data = ComputeData();

        private static (
            int Precision,
            int MaxExponent,
            int ExponentFieldBitWidth,
            int TrailingSignificandFieldBitWidth,
            int StorageBitWidth,
            bool IsSupported) ComputeData() {
            T zero = T.Zero;
            T one = T.One;
            T two = one + one;
            T four = two + two;

            // ---- 1. Precision p --------------------------------------------
            // ULP at 1 is 2^(1-p), so 1 + 2^-p rounds to 1 and 1 + 2^(1-p)
            // does not.  Loop count == p.
            int p = 0;
            {
                T u = one;
                while (one + u != one) {
                    u = u / two;
                    p++;
                }
            }

            // ---- 2. Max exponent emax (exponential + binary search) --------
            // Finite test: x + x != x  (true for finite non-zero x, false for ±∞).
            int emax = 0;
            {
                if (two + two == two)              // 2 is ∞  => emax = 0
                {
                    emax = 0;
                } else if (four + four == four)      // 4 is ∞  => emax = 1
                  {
                    emax = 1;
                } else {
                    // 4 is finite => emax >= 2.  Exponential search for an
                    // upper bound hi with 2^hi = ∞, then binary-search [lo, hi).
                    long lo = 2;
                    long hi = 4;
                    T hiVal = Pow2(4);              // 2^4 = 16
                    while (hiVal + hiVal != hiVal)  // 2^hi is finite
                    {
                        lo = hi;
                        hi = hi << 1;
                        hiVal = hiVal * hiVal;      // 2^(2*old_hi)
                    }
                    while (lo + 1 < hi) {
                        long mid = lo + ((hi - lo) >> 1);
                        if (mid > int.MaxValue) { hi = mid; continue; }
                        T midVal = Pow2((int)mid);
                        if (midVal + midVal != midVal) lo = mid;
                        else hi = mid;
                    }
                    emax = (lo >= 0 && lo <= int.MaxValue) ? (int)lo : 0;
                }
            }

            // ---- 3. Exponent field width w from the standard IEEE bias -----
            // emax = 2^(w-1) - 1  ⇔  emax + 1 is a power of two.
            int w = 0;
            bool biasOk = false;
            if (emax > 0) {
                long ep1 = (long)emax + 1L;
                if ((ep1 & (ep1 - 1)) == 0L) {
                    int log2 = 0;
                    for (long t2 = ep1; t2 > 1; t2 >>= 1) log2++;
                    w = log2 + 1;
                    biasOk = true;
                }
            }

            // ---- 4. Derived fields (implicit-leading-bit convention) -------
            int t = p - 1;
            int k = w + p;

            // ---- 5. Underflow consistency ----------------------------------
            // In an IEEE-style layout the smallest positive value is
            // 2^(2 - emax - p).  Verify by checking that 2^-(emax+p-2) > 0 and
            // that halving it underflows to 0.  This pins the denormal range
            // and rejects look-alike layouts.
            bool underflowOk = false;
            if (p >= 2 && emax >= 1) {
                int U = emax + p - 1;           // candidate underflow count
                T smallest = Pow2Neg(U - 1);    // 2^-(emax+p-2)
                if (smallest != zero) {
                    T belowSmallest = smallest / two;   // 2^-(emax+p-1)
                    underflowOk = belowSmallest == zero;
                }
            }

            // ---- 6. Validation ---------------------------------------------
            bool isSupported =
                p >= 2
                && w >= 1
                && t >= 1
                && biasOk
                && underflowOk;

            return (p, emax, w, t, k, isSupported);
        }

        // ---- Helpers --------------------------------------------------------

        /// <summary>
        /// Computes <c>2^e</c> by exponentiation by squaring. Overflow to
        /// <c>+∞</c> is handled naturally by <typeparamref name="T"/>'s own
        /// arithmetic.
        /// </summary>
        private static T Pow2(int e) {
            T result = T.One;
            T b = T.One + T.One;
            while (e > 0) {
                if ((e & 1) != 0) result = result * b;
                e >>= 1;
                if (e > 0) b = b * b;
            }
            return result;
        }

        /// <summary>
        /// Computes <c>2^-e</c> by exponentiation by squaring, starting from
        /// <c>0.5</c>. Underflow to <c>0</c> is handled naturally by
        /// <typeparamref name="T"/>'s own arithmetic (including in the denormal
        /// range).
        /// </summary>
        private static T Pow2Neg(int e) {
            T result = T.One;
            T b = T.One / (T.One + T.One);
            while (e > 0) {
                if ((e & 1) != 0) result = result * b;
                e >>= 1;
                if (e > 0) b = b * b;
            }
            return result;
        }

        // ---- Public API -----------------------------------------------------

        /// <summary>
        /// Gets a value indicating whether <typeparamref name="T"/> is a supported
        /// IEEE-style binary floating-point format.
        /// </summary>
        /// <remarks>This member never throws.</remarks>
        public static bool IsSupported {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported;
        }

        /// <summary>
        /// Gets the precision <c>p</c> of <typeparamref name="T"/> — the number of
        /// significant bits, including the implicit leading 1.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE-style binary format.
        /// </exception>
        public static int Precision {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported ? s_data.Precision : ThrowUnsupported();
        }

        /// <summary>
        /// Gets the maximum exponent <c>emax</c> of <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE-style binary format.
        /// </exception>
        public static int MaxExponent {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported ? s_data.MaxExponent : ThrowUnsupported();
        }

        /// <summary>
        /// Gets the width, in bits, of the exponent field <c>w</c> of
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE-style binary format.
        /// </exception>
        public static int ExponentFieldBitWidth {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported ? s_data.ExponentFieldBitWidth : ThrowUnsupported();
        }

        /// <summary>
        /// Gets the width, in bits, of the trailing significand field <c>t</c> of
        /// <typeparamref name="T"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE-style binary format.
        /// </exception>
        public static int TrailingSignificandFieldBitWidth {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported ? s_data.TrailingSignificandFieldBitWidth : ThrowUnsupported();
        }

        /// <summary>
        /// Gets the logical storage width <c>k = w + p</c> of the format
        /// represented by <typeparamref name="T"/> under the IEEE
        /// implicit-leading-bit convention. For a value-type storage this equals
        /// <c>8 * sizeof(T)</c>; for a heap-allocated wrapper it is the width of
        /// the format being emulated.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE-style binary format.
        /// </exception>
        public static int StorageBitWidth {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_data.IsSupported ? s_data.StorageBitWidth : ThrowUnsupported();
        }

        // ---- Cold path ------------------------------------------------------

        [DoesNotReturn]
        private static int ThrowUnsupported() {
            throw new NotSupportedException(
                $"'{typeof(T).FullName}' is not a supported IEEE-style binary floating-point format. " +
                $"Detected p = {s_data.Precision}, emax = {s_data.MaxExponent}, " +
                $"w = {s_data.ExponentFieldBitWidth}, t = {s_data.TrailingSignificandFieldBitWidth}, " +
                $"derived k = {s_data.StorageBitWidth}.");
        }

        // ---- Layout probe ---------------------------------------------------

        private enum TrailingExpectation {
            AllZeros,
            TopBitOnly,
        }

        /// <summary>
        /// Computes <c>true</c> iff <typeparamref name="T"/> has a physical bit
        /// layout that exactly matches the logical IEEE-style format reported by
        /// this class, so bit-level tricks are safe.
        /// </summary>
        private static bool ComputeHasSupportedStructLayout() {
            // (1) T must be unmanaged (no GC references anywhere in its fields).
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                return false;
            }

            // The logical format itself must be supported.
            if (!s_data.IsSupported) {
                return false;
            }

            int k = s_data.StorageBitWidth;
            int w = s_data.ExponentFieldBitWidth;
            int t = s_data.TrailingSignificandFieldBitWidth;
            long bias = s_data.MaxExponent;

            // (2) Logical storage width must equal physical storage width.
            //     This is the only place Unsafe.SizeOf<T>() appears, and it is
            //     guarded by the unmanaged check above.
            if (Unsafe.SizeOf<T>() * 8 != k) {
                return false;
            }

            // (3)-(6) Bit-pattern round-trip.  Every probe reads a specific
            // pattern of bits out of the physical representation and compares
            // it against the expected sign / exponent / trailing split.
            //
            // The implicit invariants being verified:
            //   * sign bit at position k - 1
            //   * exponent field bits [t, t + w), MSB at t + w - 1
            //   * trailing significand bits [0, t), MSB at t - 1
            //   * biased exponent == unbiased + emax
            //   * implicit leading significand bit 1

            // +1.0 : sign=0, biased exponent = bias, trailing = 0
            if (!BitPatternMatches(T.One, 0, bias, TrailingExpectation.AllZeros, k, w, t)) {
                return false;
            }

            // -1.0 : sign=1, biased exponent = bias, trailing = 0
            if (!BitPatternMatches(-T.One, 1, bias, TrailingExpectation.AllZeros, k, w, t)) {
                return false;
            }

            // +2.0 : sign=0, biased exponent = bias + 1, trailing = 0
            T two = T.One + T.One;
            if (!BitPatternMatches(two, 0, bias + 1, TrailingExpectation.AllZeros, k, w, t)) {
                return false;
            }

            // +0.5 : only well-defined as a normal number when bias >= 2.
            if (bias >= 2) {
                T half = T.One / two;
                if (!BitPatternMatches(half, 0, bias - 1, TrailingExpectation.AllZeros, k, w, t)) {
                    return false;
                }
            }

            // +1.5 : sign=0, biased exponent = bias, trailing = 100...0
            {
                T half = T.One / two;
                T oneAndHalf = T.One + half;
                if (!BitPatternMatches(oneAndHalf, 0, bias, TrailingExpectation.TopBitOnly, k, w, t)) {
                    return false;
                }
            }

            // +0.0 : sign=0, biased exponent = 0, trailing = 0
            if (!BitPatternMatches(T.Zero, 0, 0, TrailingExpectation.AllZeros, k, w, t)) {
                return false;
            }

            // -0.0 : sign=1, biased exponent = 0, trailing = 0
            if (!BitPatternMatches(-T.Zero, 1, 0, TrailingExpectation.AllZeros, k, w, t)) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads the bit at <paramref name="bitIndex"/> of the <paramref name="k"/>-bit
        /// integer representation of <paramref name="value"/>, where bit <c>0</c>
        /// is the LSB of the trailing significand field. Memory endianness is
        /// taken into account.
        /// </summary>
        private static bool GetBit(T value, int bitIndex, int k) {
            ref byte baseRef = ref Unsafe.As<T, byte>(ref value);
            if (BitConverter.IsLittleEndian) {
                byte b = Unsafe.Add(ref baseRef, bitIndex >> 3);
                return (b & (1 << (bitIndex & 7))) != 0;
            } else {
                int rev = k - 1 - bitIndex;
                byte b = Unsafe.Add(ref baseRef, rev >> 3);
                return (b & (1 << (rev & 7))) != 0;
            }
        }

        private static bool BitPatternMatches(
            T value,
            int expectedSign,
            long expectedBiasedExp,
            TrailingExpectation trailing,
            int k,
            int w,
            int t) {
            // Sign bit at position k - 1.
            if (GetBit(value, k - 1, k) != (expectedSign != 0)) {
                return false;
            }

            // Exponent field: w bits, MSB at t + w - 1, LSB at t.
            for (int i = 0; i < w; i++) {
                bool actual = GetBit(value, t + i, k);
                bool expected = ((expectedBiasedExp >> i) & 1L) != 0L;
                if (actual != expected) {
                    return false;
                }
            }

            // Trailing significand field: t bits, MSB at t - 1, LSB at 0.
            switch (trailing) {
            case TrailingExpectation.AllZeros:
                for (int i = 0; i < t; i++) {
                    if (GetBit(value, i, k)) {
                        return false;
                    }
                }
                break;

            case TrailingExpectation.TopBitOnly:
                if (!GetBit(value, t - 1, k)) {
                    return false;
                }
                for (int i = 0; i < t - 1; i++) {
                    if (GetBit(value, i, k)) {
                        return false;
                    }
                }
                break;
            }

            return true;
        }

        // ---- Public API -----------------------------------------------------

        // IsSupported, Precision, MaxExponent, ExponentFieldBitWidth,
        // TrailingSignificandFieldBitWidth, StorageBitWidth unchanged.

        /// <summary>
        /// Gets a value indicating whether <typeparamref name="T"/> has a
        /// supported basic struct layout — that is, whether its physical bit
        /// representation is a direct image of the logical IEEE-style format
        /// reported by this class, so that bit-level optimizations are safe.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see langword="true"/> iff all of the following hold:
        /// </para>
        /// <list type="number">
        ///   <item><description>
        ///   <typeparamref name="T"/> is unmanaged (contains no references).
        ///   </description></item>
        ///   <item><description>
        ///   <c>8 * sizeof(T)</c> equals the detected logical storage width
        ///   <see cref="StorageBitWidth"/>.
        ///   </description></item>
        ///   <item><description>
        ///   The sign bit is the most significant bit.
        ///   </description></item>
        ///   <item><description>
        ///   There are no padding bits: the exponent field occupies exactly
        ///   <see cref="ExponentFieldBitWidth"/> bits immediately below the sign
        ///   bit, and the trailing significand field occupies exactly
        ///   <see cref="TrailingSignificandFieldBitWidth"/> bits below that.
        ///   </description></item>
        ///   <item><description>
        ///   The values <c>+1</c>, <c>-1</c>, <c>+2</c>, <c>+0.5</c>,
        ///   <c>+1.5</c>, <c>+0</c>, and <c>-0</c> round-trip through their
        ///   physical bit patterns exactly as the standard IEEE bias and
        ///   implicit-leading-bit convention predict.
        ///   </description></item>
        /// </list>
        /// <para>
        /// When <see langword="true"/>, consumers can safely reinterpret values
        /// of <typeparamref name="T"/> as raw bytes of width <c>sizeof(T)</c> and
        /// apply bit-level tricks (sign extraction by shifting, exponent
        /// adjustment by integer addition, classification by masking, and so
        /// on). When <see langword="false"/>, bit-level access is not portable
        /// and the logical parameters should be used through ordinary
        /// arithmetic.
        /// </para>
        /// <para>This member never throws.</para>
        /// </remarks>
        internal static bool HasSupportedStructLayout {

            get;
        } = ComputeHasSupportedStructLayout();

        public static int MaxRawExponent { get; } = (MaxExponent * 2) + 1;
    }


    //public interface IDecimalFloatingPointIeee754DerivedIeee754Interchage<TSelf,
    //    TBitsInt, TBitsUInt, TExponentInt, TExponentUInt, TBitsShort, TBitsUShort>
    //    : IDecimalFloatingPointIeee754<TSelf>
    //    where TSelf : unmanaged, IDecimalFloatingPointIeee754DerivedIeee754Interchage<TSelf,
    //        TBitsInt, TBitsUInt, TExponentInt, TExponentUInt, TBitsShort, TBitsUShort>
    //    where TBitsInt : unmanaged, IBinaryInteger<TBitsInt>, ISignedNumber<TBitsInt>
    //    where TBitsUInt : unmanaged, IBinaryInteger<TBitsUInt>, IUnsignedNumber<TBitsUInt>
    //    where TExponentInt : unmanaged, IBinaryInteger<TExponentInt>, ISignedNumber<TExponentInt>
    //    where TExponentUInt : unmanaged, IBinaryInteger<TExponentUInt>, IUnsignedNumber<TExponentUInt>
    //    where TBitsShort : unmanaged, IBinaryInteger<TBitsShort>, ISignedNumber<TBitsShort>
    //    where TBitsUShort : unmanaged, IBinaryInteger<TBitsUShort>, IUnsignedNumber<TBitsUShort> {

    //    protected static virtual TSelf FromBits(TBitsInt bits) => Unsafe.BitCast<TBitsInt, TSelf>(bits);

    //    protected static virtual int StorageBitWidth => FloatingPointIeee754InterchageTypeTraits<TSelf>.StorageBitWidth;

    //    protected static virtual int Precision => FloatingPointIeee754InterchageTypeTraits<TSelf>.Precision;

    //    static TSelf IFloatingPointIeee754<TSelf>.Epsilon => TSelf.FromBits(TBitsInt.One);

    //    static TSelf IFloatingPointIeee754<TSelf>.NaN => throw new NotImplementedException();

    //    static TSelf IFloatingPointIeee754<TSelf>.NegativeInfinity => throw new NotImplementedException();

    //    static TSelf IFloatingPointIeee754<TSelf>.NegativeZero => -Zero;

    //    static TSelf IFloatingPointIeee754<TSelf>.PositiveInfinity => throw new NotImplementedException();

    //    static TSelf ISignedNumber<TSelf>.NegativeOne => -INumberBaseFriend < TSelf >.One;

    //    static TSelf IFloatingPointConstants<TSelf>.E => throw new NotImplementedException();

    //    static TSelf IFloatingPointConstants<TSelf>.Pi => throw new NotImplementedException();

    //    static TSelf IFloatingPointConstants<TSelf>.Tau => throw new NotImplementedException();

    //    static TSelf INumberBase<TSelf>.One => throw new NotImplementedException();

    //    static TSelf INumberBase<TSelf>.Zero => throw new NotImplementedException();

    //    static TSelf IAdditiveIdentity<TSelf, TSelf>.AdditiveIdentity => throw new NotImplementedException();

    //    static TSelf IMultiplicativeIdentity<TSelf, TSelf>.MultiplicativeIdentity => INumberBaseFriend<TSelf>.One;

    //    static TSelf INumberBase<TSelf>.Abs(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Acos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Acosh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AcosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Asin(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Asinh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AsinPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Atan(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Atan2(TSelf y, TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Atan2Pi(TSelf y, TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Atanh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.AtanPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.BitDecrement(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.BitIncrement(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Cbrt(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Cos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Cosh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.CosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp10(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IExponentialFunctions<TSelf>.Exp2(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Hypot(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.Ieee754Remainder(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static int IFloatingPointIeee754<TSelf>.ILogB(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsCanonical(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsComplexNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsEvenInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsFinite(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsImaginaryNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNaN(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNegative(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNegativeInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsNormal(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsOddInteger(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsPositive(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsPositiveInfinity(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsRealNumber(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsSubnormal(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.IsZero(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x, TSelf newBase) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log10(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ILogarithmicFunctions<TSelf>.Log2(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MaxMagnitude(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MaxMagnitudeNumber(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MinMagnitude(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.MinMagnitudeNumber(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf INumberBase<TSelf>.Parse(string s, NumberStyles style, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ISpanParsable<TSelf>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IParsable<TSelf>.Parse(string s, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IPowerFunctions<TSelf>.Pow(TSelf x, TSelf y) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.RootN(TSelf x, int n) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPoint<TSelf>.Round(TSelf x, int digits, MidpointRounding mode) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IFloatingPointIeee754<TSelf>.ScaleB(TSelf x, int n) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Sin(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static (TSelf Sin, TSelf Cos) ITrigonometricFunctions<TSelf>.SinCos(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static (TSelf SinPi, TSelf CosPi) ITrigonometricFunctions<TSelf>.SinCosPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Sinh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.SinPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IRootFunctions<TSelf>.Sqrt(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.Tan(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IHyperbolicFunctions<TSelf>.Tanh(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ITrigonometricFunctions<TSelf>.TanPi(TSelf x) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromChecked<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromSaturating<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertFromTruncating<TOther>(TOther value, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToChecked<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToSaturating<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryConvertToTruncating<TOther>(TSelf value, out TOther result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool INumberBase<TSelf>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool ISpanParsable<TSelf>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IParsable<TSelf>.TryParse(string? s, IFormatProvider? provider, out TSelf result) {
    //        throw new NotImplementedException();
    //    }

    //    int IComparable.CompareTo(object? obj) {
    //        throw new NotImplementedException();
    //    }

    //    int IComparable<TSelf>.CompareTo(TSelf? other) {
    //        throw new NotImplementedException();
    //    }

    //    bool IEquatable<TSelf>.Equals(TSelf? other) {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetExponentByteCount() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetExponentShortestBitLength() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetSignificandBitLength() {
    //        throw new NotImplementedException();
    //    }

    //    int IFloatingPoint<TSelf>.GetSignificandByteCount() {
    //        throw new NotImplementedException();
    //    }

    //    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) {
    //        throw new NotImplementedException();
    //    }

    //    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    bool IFloatingPoint<TSelf>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IUnaryPlusOperators<TSelf, TSelf>.operator +(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator +(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator -(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator -(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IIncrementOperators<TSelf>.operator ++(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IDecrementOperators<TSelf>.operator --(TSelf value) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator *(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IDivisionOperators<TSelf, TSelf, TSelf>.operator /(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static TSelf IModulusOperators<TSelf, TSelf, TSelf>.operator %(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IEqualityOperators<TSelf, TSelf, bool>.operator ==(TSelf? left, TSelf? right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IEqualityOperators<TSelf, TSelf, bool>.operator !=(TSelf? left, TSelf? right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator <(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator >(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator <=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    static bool IComparisonOperators<TSelf, TSelf, bool>.operator >=(TSelf left, TSelf right) {
    //        return left >= right;
    //    }

    //    public static virtual bool operator <=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }

    //    public static virtual bool operator >=(TSelf left, TSelf right) {
    //        throw new NotImplementedException();
    //    }
    //}

    /*
    [Experimental("UoWIP")]
    internal readonly struct Decimal192Bid {

    }
    */
}
