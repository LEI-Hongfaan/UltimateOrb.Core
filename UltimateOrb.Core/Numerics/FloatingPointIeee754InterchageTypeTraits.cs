using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    /// <summary>
    /// Provides compile-time (static) metadata describing the IEEE 754 interchange
    /// format represented by <typeparamref name="T"/>, such as storage width,
    /// precision, maximum exponent, and field widths.
    /// </summary>
    /// <typeparam name="T">
    /// An unmanaged IEEE 754 floating-point type implementing
    /// <see cref="IFloatingPointIeee754{T}"/> (for example
    /// <see cref="Half"/>, <see cref="float"/>, <see cref="double"/>,
    /// or a decimal floating-point type).
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// Only the interchange formats defined by IEEE 754 are supported:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>
    ///     Binary (radix 2): <c>k = 16</c>, <c>32</c>, <c>64</c>, or
    ///     <c>k &gt;= 128</c> with <c>k % 32 == 0</c>.
    ///     </description>
    ///   </item>
    ///   <item>
    ///     <description>
    ///     Decimal (radix 10): <c>k &gt;= 32</c> with <c>k % 32 == 0</c>.
    ///     </description>
    ///   </item>
    /// </list>
    /// <para>
    /// All derived values are computed exactly once per closed generic type and
    /// cached in <see langword="static"/> <see langword="readonly"/> fields. The
    /// type intentionally has <em>no</em> explicit static constructor, so the
    /// compiler keeps the <c>beforefieldinit</c> flag on it; the runtime is then
    /// free to run the initializers eagerly (off the hot path), and the JIT can
    /// fold accesses to the cached values.
    /// </para>
    /// <para>
    /// Use <see cref="IsSupported"/> to test whether <typeparamref name="T"/> is a
    /// supported interchange format without triggering an exception. Every other
    /// public member throws <see cref="NotSupportedException"/> when the format is
    /// not supported.
    /// </para>
    /// </remarks>
    [Experimental("UoWIP")]
    internal static partial class FloatingPointIeee754InterchageTypeTraits<T>
        where T : unmanaged, IFloatingPointIeee754<T> {
        // =====================================================================
        // No explicit static constructor. That is deliberate: it preserves the
        // 'beforefieldinit' flag, letting the runtime run these initializers
        // eagerly and letting the JIT fold/hoist reads of them.
        //
        // All initializers below are non-throwing. Validation happens on the
        // public getters via a cached bool + [DoesNotReturn] cold path, so an
        // unsupported T never poisons the type initializer and IsSupported
        // remains a safe probe.
        // =====================================================================

        /// <summary>
        /// Gets the storage size, in bits, of <typeparamref name="T"/>
        /// (that is, <c>8 * sizeof(T)</c>).
        /// </summary>
        /// <value>
        /// The bit width of the underlying storage, for example 16, 32, 64, or 128.
        /// </value>
        /// <remarks>
        /// This member never throws, even when <typeparamref name="T"/> is not a
        /// supported interchange format, so it can be used to inspect arbitrary
        /// types.
        /// </remarks>
        public static int StorageBitWidth {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => checked(8 * Unsafe.SizeOf<T>());
        }

        // ---- Validity (private, non-throwing) -------------------------------

        // Radix 2 or 10 only.
        private static readonly bool s_isRadixSupported = T.Radix is 2 or 10;

        // Binary: k in {16, 32, 64}, or k >= 128 and k % 32 == 0.
        // Decimal: k >= 32 and k % 32 == 0.
        private static readonly bool s_isStorageBitWidthSupported = T.Radix switch {
            2 => StorageBitWidth is 16 or 32 or 64
                  || (StorageBitWidth >= 128 && StorageBitWidth % 32 == 0),
            10 => StorageBitWidth >= 32 && StorageBitWidth % 32 == 0,
            _ => false,
        };

        private static readonly bool s_isSupported =
            s_isRadixSupported && s_isStorageBitWidthSupported;

        /// <summary>
        /// Gets a value indicating whether <typeparamref name="T"/> is a supported
        /// IEEE 754 interchange format.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if <typeparamref name="T"/> has radix 2 or 10 and
        /// a supported storage bit width; otherwise <see langword="false"/>.
        /// </value>
        /// <remarks>
        /// <para>Supported formats are:</para>
        /// <list type="bullet">
        ///   <item>
        ///     <description>
        ///     Binary (radix 2): <c>k = 16</c>, <c>32</c>, <c>64</c>, or
        ///     <c>k &gt;= 128</c> with <c>k % 32 == 0</c>.
        ///     </description>
        ///   </item>
        ///   <item>
        ///     <description>
        ///     Decimal (radix 10): <c>k &gt;= 32</c> with <c>k % 32 == 0</c>.
        ///     </description>
        ///   </item>
        /// </list>
        /// <para>This member never throws.</para>
        /// </remarks>
        public static bool IsSupported {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_isSupported;
        }

        // ---- Derived values (private, non-throwing, computed once) ----------

        // All values are computed in one shot so that no ordering dependency
        // exists between separate static field initializers. This is robust
        // against future partial-file splits, where cross-file initializer
        // order is unspecified.
        //
        //   Precision                          = p
        //   MaxExponent                        = emax
        //   ExponentOrCombinationFieldBitWidth = w (binary) / wc (decimal)
        //   TrailingSignificandFieldBitWidth   = t
        private static readonly (
            int Precision,
            int MaxExponent,
            int ExponentOrCombinationFieldBitWidth,
            int TrailingSignificandFieldBitWidth) s_data = ComputeData();

        private static (
            int Precision,
            int MaxExponent,
            int ExponentOrCombinationFieldBitWidth,
            int TrailingSignificandFieldBitWidth) ComputeData() {
            // Binary exponent field width (w).
            //
            // The general formula w = round(4*log2(k)) - 13 is only valid for
            // k >= 64. IEEE 754 explicitly specifies w = 5 for binary16 and
            // w = 8 for binary32.
            int w = StorageBitWidth switch {
                16 => 5,
                32 => 8,
                _ => (int)Math.Round(4 * Math.Log2(StorageBitWidth), MidpointRounding.ToEven) - 13,
            };

            int p = T.Radix == 2
                ? StorageBitWidth - w
                : 9 * StorageBitWidth / 32 - 2;

            int wc = T.Radix == 2
                ? w
                : StorageBitWidth / 16 + 9;

            int emax = T.Radix == 2
                ? NumberBaseExtensions.Pow(2, uint.CreateChecked(StorageBitWidth - p - 1)) - 1
                : 3 * NumberBaseExtensions.Pow(2, uint.CreateChecked(StorageBitWidth / 16 + 3));

            int t = T.Radix == 2
                ? StorageBitWidth - wc - 1
                : 15 * StorageBitWidth / 16 - 10;

            return (p, emax, wc, t);
        }

        // ---- Public getters (ldsfld + one predicted branch) -----------------
        //
        // For a supported T the branch is always taken and perfectly predicted,
        // so the steady-state cost is one cached load plus the branch. The else
        // arm is a [DoesNotReturn] call, which the JIT treats as cold and keeps
        // out of the inlined body.
        //
        // The [MethodImpl(MethodImplOptions.AggressiveInlining)] attribute is
        // attached to the explicit 'get' accessor rather than to the property,
        // so the inlining hint lands directly on the accessor method that the
        // caller will emit a call to.

        /// <summary>
        /// Gets the precision <c>p</c> of the interchange format represented by
        /// <typeparamref name="T"/> (the number of significant digits in the
        /// significand).
        /// </summary>
        /// <value>
        /// For radix 2, the number of significant bits (for example 11 for
        /// binary16, 24 for binary32, 53 for binary64). For radix 10, the number
        /// of significant decimal digits (for example 7 for decimal32, 16 for
        /// decimal64, 34 for decimal128).
        /// </value>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE 754 interchange format.
        /// See <see cref="IsSupported"/>.
        /// </exception>
        public static int Precision { get; } =
            s_isSupported ? s_data.Precision : ThrowUnsupported();

        /// <summary>
        /// Gets the maximum exponent <c>emax</c> of the interchange format
        /// represented by <typeparamref name="T"/>.
        /// </summary>
        /// <value>
        /// The largest finite exponent representable by the format, for example
        /// 15 for binary16, 127 for binary32, 1023 for binary64, and 96 for
        /// decimal32.
        /// </value>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE 754 interchange format.
        /// See <see cref="IsSupported"/>.
        /// </exception>
        public static int MaxExponent { get; } =
            s_isSupported ? s_data.MaxExponent : ThrowUnsupported();

        /// <summary>
        /// Gets the width, in bits, of the exponent field for binary formats, or
        /// of the combination field for decimal formats, of the interchange format
        /// represented by <typeparamref name="T"/>.
        /// </summary>
        /// <value>
        /// For binary formats, the exponent field width <c>w</c> (for example 5
        /// for binary16, 8 for binary32, 11 for binary64). For decimal formats,
        /// the combination field width (for example 11 for decimal32, 13 for
        /// decimal64, 17 for decimal128).
        /// </value>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE 754 interchange format.
        /// See <see cref="IsSupported"/>.
        /// </exception>
        public static int ExponentOrCombinationFieldBitWidth { get; } =
            s_isSupported ? s_data.ExponentOrCombinationFieldBitWidth : ThrowUnsupported();

        /// <summary>
        /// Gets the width, in bits, of the trailing significand field of the
        /// interchange format represented by <typeparamref name="T"/>.
        /// </summary>
        /// <value>
        /// For binary formats, the trailing significand field width
        /// <c>t = k - w - 1</c> (for example 10 for binary16, 23 for binary32,
        /// 52 for binary64). For decimal formats, the trailing significand field
        /// width (for example 20 for decimal32, 50 for decimal64, 110 for
        /// decimal128).
        /// </value>
        /// <exception cref="NotSupportedException">
        /// <typeparamref name="T"/> is not a supported IEEE 754 interchange format.
        /// See <see cref="IsSupported"/>.
        /// </exception>
        public static int TrailingSignificandFieldBitWidth { get; }
            = s_isSupported ? s_data.TrailingSignificandFieldBitWidth : ThrowUnsupported();

        // ---- Cold path ------------------------------------------------------

        // Called only when s_isSupported is false. Marked [DoesNotReturn] so the
        // JIT lays it out as an out-of-line cold block and does not inline it
        // into the property getters above.
        [DoesNotReturn]
        private static int ThrowUnsupported() {
            if (!s_isRadixSupported) {
                throw new NotSupportedException(
                    $"The radix of '{typeof(T).FullName}' must be 2 or 10, but was {T.Radix}.");
            }

            throw new NotSupportedException(
                $"The storage bit width {StorageBitWidth} of '{typeof(T).FullName}' is not a " +
                $"valid IEEE 754 interchange format for radix {T.Radix}. " +
                (T.Radix == 2
                    ? "Supported binary widths: 16, 32, 64, or k >= 128 with k % 32 == 0."
                    : "Supported decimal widths: k >= 32 with k % 32 == 0."));
        }
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
