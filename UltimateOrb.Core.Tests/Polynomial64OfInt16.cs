using System;
using System.Collections;
using System.Collections.Generic;

namespace UltimateOrb.Core.Tests {

    //using UltimateOrb.Typed_Huge.Collections.Generic;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;
    using System.Text;
    using UltimateOrb.Utilities;






    /// <summary>
    /// 向量化支持的层级，供 <see cref="Polynomial128{TCoefficient}"/> 内部检测使用。
    /// </summary>
    public enum VectorizedApiLevel {
        /// <summary>不使用任何 SIMD 加速，全部走标量回退。</summary>
        None = 0,
        /// <summary>支持 <see cref="Vector128{T}"/> 的算术、比较、条件选择与 <c>Abs</c>。</summary>
        Arithmetic = 1,
    }

    /// <summary>
    /// <typeparamref name="TCoefficient"/> 的类型特征与辅助方法。
    /// </summary>
    internal static partial class Coefficient<TCoefficient>
        where TCoefficient : unmanaged, INumberBase<TCoefficient> {
        /// <summary>单个系数的字节大小。</summary>
        public static int ByteSize => sizeof(TCoefficient);

        /// <summary>
        /// <see langword="true"/> 表示硬件 SIMD 支持 <typeparamref name="TCoefficient"/>，
        /// 可安全调用 <see cref="Vector128"/> 上的泛型静态方法（算术、比较、<c>Abs</c>、<c>ConditionalSelect</c>）。
        /// </summary>
        public static bool VectorizationIsSupported => Vector128<TCoefficient>.IsSupported;

        public static bool SupportsNaN =>
            typeof(TCoefficient) == typeof(float) ||
            typeof(TCoefficient) == typeof(double) ||
            typeof(TCoefficient) == typeof(Half);

        public static bool FloatingPointVectorizationIsSupported =>
            VectorizationIsSupported && SupportsNaN;

        /// <summary>
        /// 当前 <typeparamref name="TCoefficient"/> 的向量化支持层级。
        /// </summary>
        public static VectorizedApiLevel VectorizedApiLevel =>
            VectorizationIsSupported ? VectorizedApiLevel.Arithmetic
            : VectorizedApiLevel.None;

        internal static bool IsOne<TCoefficient>(TCoefficient value)
            where TCoefficient : unmanaged, INumberBase<TCoefficient>
            => TCoefficient.One == value;

        internal static int Compare<TCoefficient>(TCoefficient x, TCoefficient y)
            where TCoefficient : unmanaged, INumberBase<TCoefficient>
            => Comparer<TCoefficient>.Default.Compare(x, y);

        // ---------------------------------------------------------------------
        //  向量化辅助——调用方必须先检查 VectorizationIsSupported
        // ---------------------------------------------------------------------

        /// <summary>逐元素取绝对值。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<TCoefficient> VectorAbs(Vector128<TCoefficient> vector)
            => Vector128.Abs(vector);

        /// <summary>逐元素比较绝对值，返回掩码；掩码为全 1 表示 <paramref name="left"/> 的绝对值更大。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<TCoefficient> VectorAbsGreaterThan(
            Vector128<TCoefficient> left, Vector128<TCoefficient> right)
            => Vector128.GreaterThan(Vector128.Abs(left), Vector128.Abs(right));

        /// <summary>逐元素比较绝对值，返回掩码；掩码为全 1 表示 <paramref name="left"/> 的绝对值更小。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<TCoefficient> VectorAbsLessThan(
            Vector128<TCoefficient> left, Vector128<TCoefficient> right)
            => Vector128.LessThan(Vector128.Abs(left), Vector128.Abs(right));

        /// <summary>该向量是否所有元素都与 <paramref name="other"/> 的对应元素相等（全 1 掩码）。</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool VectorAllOnes(Vector128<TCoefficient> mask)
            => Vector128.EqualsAll(mask, Vector128<TCoefficient>.AllBitsSet);
    }

    public readonly partial struct Polynomial128<TCoefficient> :
        INumber<Polynomial128<TCoefficient>>
        where TCoefficient : unmanaged, INumber<TCoefficient> {
        readonly Vector128<UInt64> _wbits;

        // =====================================================================
        //  构造
        // =====================================================================

        public Polynomial128(TCoefficient value) : this() {
            Unsafe.As<Polynomial128<TCoefficient>, TCoefficient>(ref this) = value;
        }

        internal Polynomial128(Vector128<UInt64> wbits) : this() {
            _wbits = wbits;
        }

        readonly ref readonly System.UInt128 _bits
            => ref Unsafe.As<Vector128<UInt64>, System.UInt128>(ref Unsafe.AsRef(in _wbits));

        readonly Vector128<TCoefficient> _vbits
            => Vector128.As<UInt64, TCoefficient>(_wbits);

        static int MaxOrder { get; }
            = sizeof(Polynomial128<TCoefficient>) / Coefficient<TCoefficient>.ByteSize - 1;

        // =====================================================================
        //  辅助——Span 访问 / 阶数工具
        // =====================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Span<TCoefficient> DangerousGetSpan(ref Polynomial128<TCoefficient> value)
            => MemoryMarshal.CreateSpan(
                ref Unsafe.As<Polynomial128<TCoefficient>, TCoefficient>(ref value),
                sizeof(Polynomial128<TCoefficient>) / sizeof(TCoefficient));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ReadOnlySpan<TCoefficient> GetReadOnlySpan(in Polynomial128<TCoefficient> value)
            => MemoryMarshal.CreateReadOnlySpan(
                ref Unsafe.As<Polynomial128<TCoefficient>, TCoefficient>(ref Unsafe.AsRef(in value)),
                sizeof(Polynomial128<TCoefficient>) / sizeof(TCoefficient));

        // -1 表示零多项式
        private static int GetDegree(ReadOnlySpan<TCoefficient> coeffs) {
            for (int i = coeffs.Length - 1; i >= 0; i--)
                if (!TCoefficient.IsZero(coeffs[i])) return i;
            return -1;
        }

        private static bool IsConstant(ReadOnlySpan<TCoefficient> coeffs) {
            for (int i = 1; i < coeffs.Length; i++)
                if (!TCoefficient.IsZero(coeffs[i])) return false;
            return true;
        }

        private static string FormatPolynomial(ReadOnlySpan<TCoefficient> coeffs, string? format, IFormatProvider? provider) {
            var sb = new StringBuilder();
            bool first = true;
            for (int i = coeffs.Length - 1; i >= 0; i--) {
                var c = coeffs[i];
                if (TCoefficient.IsZero(c)) continue;

                bool negative = TCoefficient.IsNegative(c);
                var mag = negative ? TCoefficient.Abs(c) : c;

                if (first) {
                    if (negative) sb.Append('-');
                    first = false;
                } else {
                    sb.Append(negative ? " - " : " + ");
                }

                bool showCoef = i == 0 || !Coefficient<TCoefficient>.IsOne(mag);
                if (showCoef) {
                    sb.Append(mag.ToString(format, provider));
                    if (i > 0) sb.Append('*');
                }

                if (i == 1) sb.Append('x');
                else if (i > 1) { sb.Append("x^"); sb.Append(i); }
            }

            if (first) sb.Append(TCoefficient.Zero.ToString(format, provider));
            return sb.ToString();
        }

        // =====================================================================
        //  INumberBase / 单位元 / 基数
        // =====================================================================

        static Polynomial128<TCoefficient> INumberBase<Polynomial128<TCoefficient>>.One { get; } =
            Coefficient<TCoefficient>.VectorizationIsSupported
                ? new Polynomial128<TCoefficient>(Vector128.CreateScalar(TCoefficient.One).AsUInt64())
                : new Polynomial128<TCoefficient>(TCoefficient.One);

        static int INumberBase<Polynomial128<TCoefficient>>.Radix => TCoefficient.Radix;

        static Polynomial128<TCoefficient> INumberBase<Polynomial128<TCoefficient>>.Zero { get; } =
            Coefficient<TCoefficient>.VectorizationIsSupported
                ? new Polynomial128<TCoefficient>(Vector128<UInt64>.Zero)
                : new Polynomial128<TCoefficient>(TCoefficient.Zero);

        // it could be -0
        static Polynomial128<TCoefficient>
            IAdditiveIdentity<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>>
            .AdditiveIdentity { get; } =
            Coefficient<TCoefficient>.VectorizationIsSupported
                ? new Polynomial128<TCoefficient>(-Vector128<UInt64>.Zero)
                : new Polynomial128<TCoefficient>(TCoefficient.AdditiveIdentity);

        static Polynomial128<TCoefficient>
            IMultiplicativeIdentity<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>>
            .MultiplicativeIdentity { get; } =
            Coefficient<TCoefficient>.VectorizationIsSupported
                ? new Polynomial128<TCoefficient>(Vector128.CreateScalar(TCoefficient.One).AsUInt64())
                : new Polynomial128<TCoefficient>(TCoefficient.One);

        // =====================================================================
        //  谓词
        // =====================================================================

        static Polynomial128<TCoefficient>
            INumberBase<Polynomial128<TCoefficient>>.Abs(Polynomial128<TCoefficient> value) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported)
                return new Polynomial128<TCoefficient>(
                    Coefficient<TCoefficient>.VectorAbs(value._vbits).AsUInt64());

            var result = value;
            var span = DangerousGetSpan(ref result);
            for (int i = 0; i < span.Length; i++)
                span[i] = TCoefficient.Abs(span[i]);
            return result;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsCanonical(Polynomial128<TCoefficient> value) => true;

        static bool INumberBase<Polynomial128<TCoefficient>>.IsComplexNumber(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsComplexNumber(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsImaginaryNumber(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsImaginaryNumber(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsRealNumber(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsRealNumber(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsInteger(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsInteger(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsEvenInteger(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsEvenInteger(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsOddInteger(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            return IsConstant(span) && TCoefficient.IsOddInteger(span[0]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsFinite(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (!TCoefficient.IsFinite(span[i])) return false;
            return true;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsInfinity(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (TCoefficient.IsInfinity(span[i])) return true;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsPositiveInfinity(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (TCoefficient.IsPositiveInfinity(span[i])) return true;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsNegativeInfinity(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (TCoefficient.IsNegativeInfinity(span[i])) return true;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsNaN(Polynomial128<TCoefficient> value) {
            // 向量化路径：x != x 检测 NaN。所有元素均非 NaN ⇔ Equals(x,x) 全 1。
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                var notNan = Vector128.Equals(value._vbits, value._vbits);
                return !Coefficient<TCoefficient>.VectorAllOnes(notNan);
            }

            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (TCoefficient.IsNaN(span[i])) return true;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsSubnormal(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (TCoefficient.IsSubnormal(span[i])) return true;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsNormal(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            bool anyNonZero = false;
            for (int i = 0; i < span.Length; i++) {
                var c = span[i];
                if (TCoefficient.IsZero(c)) continue;
                if (!TCoefficient.IsNormal(c)) return false;
                anyNonZero = true;
            }
            return anyNonZero;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsZero(Polynomial128<TCoefficient> value) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                // 所有分量等于零 ⇔ Equals(v, 0) 全 1。
                var eqZero = Vector128.Equals(value._vbits, Vector128<TCoefficient>.Zero);
                return Coefficient<TCoefficient>.VectorAllOnes(eqZero);
            }

            var span = GetReadOnlySpan(in value);
            for (int i = 0; i < span.Length; i++)
                if (!TCoefficient.IsZero(span[i])) return false;
            return true;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsNegative(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            int deg = GetDegree(span);
            if (deg < 0) return false;
            return TCoefficient.IsNegative(span[deg]);
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.IsPositive(Polynomial128<TCoefficient> value) {
            var span = GetReadOnlySpan(in value);
            int deg = GetDegree(span);
            if (deg < 0) return false;
            return TCoefficient.IsPositive(span[deg]);
        }

        // =====================================================================
        //  幅值 Min/Max——优先向量化
        // =====================================================================

        static Polynomial128<TCoefficient> PickMagnitude(
            Polynomial128<TCoefficient> x, Polynomial128<TCoefficient> y, bool wantMax) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                var mask = wantMax
                    ? Coefficient<TCoefficient>.VectorAbsGreaterThan(x._vbits, y._vbits)
                    : Coefficient<TCoefficient>.VectorAbsLessThan(x._vbits, y._vbits);
                return new Polynomial128<TCoefficient>(
                    Vector128.ConditionalSelect(mask, x._vbits, y._vbits).AsUInt64());
            }

            var xs = GetReadOnlySpan(in x);
            var ys = GetReadOnlySpan(in y);
            var result = default(Polynomial128<TCoefficient>);
            var rs = DangerousGetSpan(ref result);
            for (int i = 0; i < rs.Length; i++) {
                var cmp = Coefficient<TCoefficient>.Compare(
                    TCoefficient.Abs(xs[i]), TCoefficient.Abs(ys[i]));
                bool takeX = wantMax ? cmp >= 0 : cmp <= 0;
                rs[i] = takeX ? xs[i] : ys[i];
            }
            return result;
        }

        static Polynomial128<TCoefficient>
            INumberBase<Polynomial128<TCoefficient>>.MaxMagnitude(
                Polynomial128<TCoefficient> x, Polynomial128<TCoefficient> y)
            => PickMagnitude(x, y, true);

        static Polynomial128<TCoefficient>
            INumberBase<Polynomial128<TCoefficient>>.MinMagnitude(
                Polynomial128<TCoefficient> x, Polynomial128<TCoefficient> y)
            => PickMagnitude(x, y, false);

        static Polynomial128<TCoefficient>
            INumberBase<Polynomial128<TCoefficient>>.MaxMagnitudeNumber(
                Polynomial128<TCoefficient> x, Polynomial128<TCoefficient> y) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                var xIsNan = Vector128.OnesComplement(Vector128.Equals(x._vbits, x._vbits));
                var yIsNan = Vector128.OnesComplement(Vector128.Equals(y._vbits, y._vbits));
                var magnitudeMask = Coefficient<TCoefficient>.VectorAbsGreaterThan(x._vbits, y._vbits);

                // 优先级：若 x 为 NaN 取 y；否则若 y 为 NaN 取 x；否则按幅值取。
                var pickY = xIsNan;
                var pickX = Vector128.AndNot(yIsNan, xIsNan); // y 是 NaN 且 x 不是
                var normal = Vector128.AndNot(Vector128<TCoefficient>.AllBitsSet, (xIsNan | yIsNan));

                var takeX = pickX | (normal & magnitudeMask);
                var result = Vector128.ConditionalSelect(takeX, x._vbits, y._vbits);
                return new Polynomial128<TCoefficient>(result.AsUInt64());
            }

            var xs = GetReadOnlySpan(in x);
            var ys = GetReadOnlySpan(in y);
            var result2 = default(Polynomial128<TCoefficient>);
            var rs = DangerousGetSpan(ref result2);
            for (int i = 0; i < rs.Length; i++) {
                if (TCoefficient.IsNaN(xs[i])) { rs[i] = ys[i]; continue; }
                if (TCoefficient.IsNaN(ys[i])) { rs[i] = xs[i]; continue; }
                rs[i] = Coefficient<TCoefficient>.Compare(
                    TCoefficient.Abs(xs[i]), TCoefficient.Abs(ys[i])) >= 0 ? xs[i] : ys[i];
            }
            return result2;
        }

        static Polynomial128<TCoefficient>
            INumberBase<Polynomial128<TCoefficient>>.MinMagnitudeNumber(
                Polynomial128<TCoefficient> x, Polynomial128<TCoefficient> y) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                var xIsNan = Vector128.OnesComplement(Vector128.Equals(x._vbits, x._vbits));
                var yIsNan = Vector128.OnesComplement(Vector128.Equals(y._vbits, y._vbits));
                var magnitudeMask = Coefficient<TCoefficient>.VectorAbsLessThan(x._vbits, y._vbits);

                var pickX = Vector128.AndNot(yIsNan, xIsNan); // y 是 NaN 且 x 不是
                var normal = Vector128.AndNot(Vector128<TCoefficient>.AllBitsSet, (xIsNan | yIsNan));

                var takeX = pickX | (normal & magnitudeMask);
                var result = Vector128.ConditionalSelect(takeX, x._vbits, y._vbits);
                return new Polynomial128<TCoefficient>(result.AsUInt64());
            }

            var xs = GetReadOnlySpan(in x);
            var ys = GetReadOnlySpan(in y);
            var result2 = default(Polynomial128<TCoefficient>);
            var rs = DangerousGetSpan(ref result2);
            for (int i = 0; i < rs.Length; i++) {
                if (TCoefficient.IsNaN(xs[i])) { rs[i] = ys[i]; continue; }
                if (TCoefficient.IsNaN(ys[i])) { rs[i] = xs[i]; continue; }
                rs[i] = Coefficient<TCoefficient>.Compare(
                    TCoefficient.Abs(xs[i]), TCoefficient.Abs(ys[i])) <= 0 ? xs[i] : ys[i];
            }
            return result2;
        }

        // =====================================================================
        //  解析 / 格式化
        // =====================================================================

        // 接受以逗号分隔的系数，低阶在前："[1,2,3]" == 1 + 2x + 3x^2。
        // 超出 MaxOrder 的输入视为阶数溢出，直接失败（与整数溢出在 TryParse 中返回 false 一致）。
        static bool TryParseCore(
            ReadOnlySpan<char> s,
            NumberStyles style,
            IFormatProvider? provider,
            out Polynomial128<TCoefficient> result) {
            result = default;
            s = s.Trim();
            if (s.Length < 2 || s[0] != '[' || s[^1] != ']') return false;
            s = s[1..^1].Trim();
            if (s.IsEmpty) return true; // 空 == 零多项式

            var resultSpan = DangerousGetSpan(ref result);
            int index = 0;
            int start = 0;
            for (int i = 0; i <= s.Length; i++) {
                if (i == s.Length || s[i] == ',') {
                    var piece = s[start..i].Trim();
                    if (piece.IsEmpty) { result = default; return false; }
                    if (!TCoefficient.TryParse(piece, style, provider, out var coeff)) {
                        result = default;
                        return false;
                    }
                    if (index >= resultSpan.Length) {
                        // 阶数溢出。
                        result = default;
                        return false;
                    }
                    resultSpan[index++] = coeff;
                    start = i + 1;
                }
            }
            return true;
        }

        static Polynomial128<TCoefficient> ParseCore(
            ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            if (!TryParseCore(s, style, provider, out var result))
                throw new FormatException($"Cannot parse '{s.ToString()}' as {nameof(Polynomial128<TCoefficient>)}.");
            return result;
        }

        static Polynomial128<TCoefficient> INumberBase<Polynomial128<TCoefficient>>.Parse(
            ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
            => ParseCore(s, style, provider);

        static Polynomial128<TCoefficient> INumberBase<Polynomial128<TCoefficient>>.Parse(
            string s, NumberStyles style, IFormatProvider? provider)
            => ParseCore(s.AsSpan(), style, provider);

        static Polynomial128<TCoefficient> ISpanParsable<Polynomial128<TCoefficient>>.Parse(
            ReadOnlySpan<char> s, IFormatProvider? provider)
            => ParseCore(s, NumberStyles.Any, provider);

        static Polynomial128<TCoefficient> IParsable<Polynomial128<TCoefficient>>.Parse(
            string s, IFormatProvider? provider)
            => ParseCore(s.AsSpan(), NumberStyles.Any, provider);

        static bool INumberBase<Polynomial128<TCoefficient>>.TryParse(
            ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider,
            out Polynomial128<TCoefficient> result)
            => TryParseCore(s, style, provider, out result);

        static bool INumberBase<Polynomial128<TCoefficient>>.TryParse(
            string? s, NumberStyles style, IFormatProvider? provider,
            out Polynomial128<TCoefficient> result) {
            if (s is null) { result = default; return false; }
            return TryParseCore(s.AsSpan(), style, provider, out result);
        }

        static bool ISpanParsable<Polynomial128<TCoefficient>>.TryParse(
            ReadOnlySpan<char> s, IFormatProvider? provider,
            out Polynomial128<TCoefficient> result)
            => TryParseCore(s, NumberStyles.Any, provider, out result);

        static bool IParsable<Polynomial128<TCoefficient>>.TryParse(
            string? s, IFormatProvider? provider,
            out Polynomial128<TCoefficient> result) {
            if (s is null) { result = default; return false; }
            return TryParseCore(s.AsSpan(), NumberStyles.Any, provider, out result);
        }

        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
            => FormatPolynomial(GetReadOnlySpan(in this), format, formatProvider);

        bool ISpanFormattable.TryFormat(
            Span<char> destination, out int charsWritten,
            ReadOnlySpan<char> format, IFormatProvider? provider) {
            var s = FormatPolynomial(GetReadOnlySpan(in this),
                format.IsEmpty ? null : format.ToString(), provider);
            if (s.AsSpan().TryCopyTo(destination)) {
                charsWritten = s.Length;
                return true;
            }
            charsWritten = 0;
            return false;
        }

        // =====================================================================
        //  转换
        // =====================================================================

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertFromChecked<TOther>(
            TOther value, out Polynomial128<TCoefficient> result) {
            if (typeof(TOther) == typeof(TCoefficient)) {
                result = new Polynomial128<TCoefficient>(Unsafe.As<TOther, TCoefficient>(ref value));
                return true;
            }
            if (TCoefficient.TryConvertFromChecked(value, out var c)) {
                result = new Polynomial128<TCoefficient>(c);
                return true;
            }
            result = default;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertFromSaturating<TOther>(
            TOther value, out Polynomial128<TCoefficient> result) {
            if (typeof(TOther) == typeof(TCoefficient)) {
                result = new Polynomial128<TCoefficient>(Unsafe.As<TOther, TCoefficient>(ref value));
                return true;
            }
            if (TCoefficient.TryConvertFromSaturating(value, out var c)) {
                result = new Polynomial128<TCoefficient>(c);
                return true;
            }
            result = default;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertFromTruncating<TOther>(
            TOther value, out Polynomial128<TCoefficient> result) {
            if (typeof(TOther) == typeof(TCoefficient)) {
                result = new Polynomial128<TCoefficient>(Unsafe.As<TOther, TCoefficient>(ref value));
                return true;
            }
            if (TCoefficient.TryConvertFromTruncating(value, out var c)) {
                result = new Polynomial128<TCoefficient>(c);
                return true;
            }
            result = default;
            return false;
        }

        private static bool TryExtractConstant(
            in Polynomial128<TCoefficient> value, out TCoefficient constant) {
            var span = GetReadOnlySpan(in value);
            for (int i = 1; i < span.Length; i++) {
                if (!TCoefficient.IsZero(span[i])) {
                    constant = default;
                    return false;
                }
            }
            constant = span[0];
            return true;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertToChecked<TOther>(
            Polynomial128<TCoefficient> value, out TOther result) {
            if (typeof(TOther) == typeof(Polynomial128<TCoefficient>)) {
                result = (TOther)(object)value;
                return true;
            }
            if (TryExtractConstant(in value, out var constant)
                && TCoefficient.TryConvertToChecked(constant, out TOther? converted)) {
                result = converted!;
                return true;
            }
            result = default!;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertToSaturating<TOther>(
            Polynomial128<TCoefficient> value, out TOther result) {
            if (typeof(TOther) == typeof(Polynomial128<TCoefficient>)) {
                result = (TOther)(object)value;
                return true;
            }
            if (TryExtractConstant(in value, out var constant)
                && TCoefficient.TryConvertToSaturating(constant, out TOther? converted)) {
                result = converted!;
                return true;
            }
            result = default!;
            return false;
        }

        static bool INumberBase<Polynomial128<TCoefficient>>.TryConvertToTruncating<TOther>(
            Polynomial128<TCoefficient> value, out TOther result) {
            if (typeof(TOther) == typeof(Polynomial128<TCoefficient>)) {
                result = (TOther)(object)value;
                return true;
            }
            if (TryExtractConstant(in value, out var constant)
                && TCoefficient.TryConvertToTruncating(constant, out TOther? converted)) {
                result = converted!;
                return true;
            }
            result = default!;
            return false;
        }

        // =====================================================================
        //  相等性
        // =====================================================================

        bool IEquatable<Polynomial128<TCoefficient>>.Equals(Polynomial128<TCoefficient> other) {
            // 快路径：按位相等（对 IEEE 类型的规范零也正确）。
            if (_wbits == other._wbits) return true;

            // 慢路径：逐元素比较，让 -0.0 == +0.0。
            if (Coefficient<TCoefficient>.VectorizationIsSupported) {
                var eq = Vector128.Equals(_vbits, other._vbits);
                return Coefficient<TCoefficient>.VectorAllOnes(eq);
            }

            var a = GetReadOnlySpan(in this);
            var b = GetReadOnlySpan(in other);
            for (int i = 0; i < a.Length; i++)
                if (!Coefficient<TCoefficient>.Equals(a[i], b[i])) return false;
            return true;
        }

        public override bool Equals(object? obj)
            => obj is Polynomial128<TCoefficient> p
               && ((IEquatable<Polynomial128<TCoefficient>>)this).Equals(p);

        public override int GetHashCode() => _wbits.GetHashCode();

        public override string ToString() => FormatPolynomial(GetReadOnlySpan(in this), null, null);

        // =====================================================================
        //  运算符
        // =====================================================================

        static Polynomial128<TCoefficient>
            IUnaryPlusOperators<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>>
            .operator +(Polynomial128<TCoefficient> value) => value;

        // ---- 一元取负 ----

        public static Polynomial128<TCoefficient> operator -(Polynomial128<TCoefficient> value) {
            if (Coefficient<TCoefficient>.VectorizationIsSupported)
                return new Polynomial128<TCoefficient>((-value._vbits).AsUInt64());

            var result = value;
            var span = DangerousGetSpan(ref result);
            for (int i = 0; i < span.Length; i++)
                span[i] = -span[i];
            return result;
        }

        public static Polynomial128<TCoefficient> operator checked -(
            Polynomial128<TCoefficient> value) {
            var result = value;
            var span = DangerousGetSpan(ref result);
            for (int i = 0; i < span.Length; i++)
                span[i] = checked(-span[i]);
            return result;
        }

        // ---- 二元加 ----

        static Polynomial128<TCoefficient> AddCore(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right, bool isChecked) {
            if (!isChecked && Coefficient<TCoefficient>.VectorizationIsSupported)
                return new Polynomial128<TCoefficient>((left._vbits + right._vbits).AsUInt64());

            var result = default(Polynomial128<TCoefficient>);
            var ls = GetReadOnlySpan(in left);
            var rs = GetReadOnlySpan(in right);
            var resultSpan = DangerousGetSpan(ref result);
            if (isChecked) {
                for (int i = 0; i < resultSpan.Length; i++)
                    resultSpan[i] = checked(ls[i] + rs[i]);
            } else {
                for (int i = 0; i < resultSpan.Length; i++)
                    resultSpan[i] = unchecked(ls[i] + rs[i]);
            }
            return result;
        }

        public static Polynomial128<TCoefficient> operator +(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => AddCore(left, right, isChecked: false);

        public static Polynomial128<TCoefficient> operator checked +(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => AddCore(left, right, isChecked: true);

        // ---- 二元减 ----

        static Polynomial128<TCoefficient> SubtractCore(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right, bool isChecked) {
            if (!isChecked && Coefficient<TCoefficient>.VectorizationIsSupported)
                return new Polynomial128<TCoefficient>((left._vbits - right._vbits).AsUInt64());

            var result = default(Polynomial128<TCoefficient>);
            var ls = GetReadOnlySpan(in left);
            var rs = GetReadOnlySpan(in right);
            var resultSpan = DangerousGetSpan(ref result);
            if (isChecked) {
                for (int i = 0; i < resultSpan.Length; i++)
                    resultSpan[i] = checked(ls[i] - rs[i]);
            } else {
                for (int i = 0; i < resultSpan.Length; i++)
                    resultSpan[i] = unchecked( ls[i] - rs[i]);
            }
            return result;
        }

        public static Polynomial128<TCoefficient> operator -(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => SubtractCore(left, right, isChecked: false);

        public static Polynomial128<TCoefficient> operator checked -(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => SubtractCore(left, right, isChecked: true);

        // ---- 自增 / 自减（只影响常数项 = span[0]） ----

        public static Polynomial128<TCoefficient> operator ++(
            Polynomial128<TCoefficient> value) {
            var result = value;
            var span = DangerousGetSpan(ref result);
            span[0] = span[0] + TCoefficient.One;
            return result;
        }

        public static Polynomial128<TCoefficient> operator checked ++(
            Polynomial128<TCoefficient> value) {
            var result = value;
            var span = DangerousGetSpan(ref result);
            span[0] = checked(span[0] + TCoefficient.One);
            return result;
        }

        public static Polynomial128<TCoefficient> operator --(
            Polynomial128<TCoefficient> value) {
            var result = value;
            var span = DangerousGetSpan(ref result);
            span[0] = span[0] - TCoefficient.One;
            return result;
        }

        public static Polynomial128<TCoefficient> operator checked --(
            Polynomial128<TCoefficient> value) {
            var result = value;
            var span = DangerousGetSpan(ref result);
            span[0] = checked(span[0] - TCoefficient.One);
            return result;
        }

        // ---- 乘法（多项式卷积，截断于 MaxOrder；阶数溢出按整数溢出处理） ----

        static Polynomial128<TCoefficient> MultiplyCore(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right, bool isChecked) {
            var ls = GetReadOnlySpan(in left);
            var rs = GetReadOnlySpan(in right);
            var result = default(Polynomial128<TCoefficient>);
            var resultSpan = DangerousGetSpan(ref result);
            int maxOrder = MaxOrder;

            for (int i = 0; i < ls.Length; i++) {
                if (TCoefficient.IsZero(ls[i])) continue;
                for (int j = 0; j < rs.Length; j++) {
                    if (TCoefficient.IsZero(rs[j])) continue;
                    int order = i + j;
                    if (order > maxOrder) {
                        if (isChecked)
                            throw new OverflowException(
                                $"Polynomial degree {order} exceeds MaxOrder {maxOrder}.");
                        continue;
                    }
                    if (isChecked)
                        resultSpan[order] = checked(resultSpan[order] + ls[i] * rs[j]);
                    else
                        resultSpan[order] = unchecked(resultSpan[order] + ls[i] * rs[j]);
                }
            }
            return result;
        }

        public static Polynomial128<TCoefficient> operator *(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => MultiplyCore(left, right, isChecked: false);

        public static Polynomial128<TCoefficient> operator checked *(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => MultiplyCore(left, right, isChecked: true);

        // ---- 除法：欧几里得带余除法 ----

        /// <summary>
        /// 对被除式与除式做欧几里得带余除法。
        /// 返回商；余数通过 <paramref name="remainder"/> 输出，且其次数严格小于除式次数。
        /// </summary>
        /// <exception cref="DivideByZeroException">除式为零多项式。</exception>
        public static Polynomial128<TCoefficient> DivRem(
            Polynomial128<TCoefficient> dividend,
            Polynomial128<TCoefficient> divisor,
            out Polynomial128<TCoefficient> remainder)
            => DivRemCore(dividend, divisor, out remainder, isChecked: false);

        /// <summary>
        /// <see cref="DivRem"/> 的 checked 版本，中间算术使用 <see langword="checked"/> 上下文。
        /// </summary>
        public static Polynomial128<TCoefficient> DivRemChecked(
            Polynomial128<TCoefficient> dividend,
            Polynomial128<TCoefficient> divisor,
            out Polynomial128<TCoefficient> remainder)
            => DivRemCore(dividend, divisor, out remainder, isChecked: true);

        static Polynomial128<TCoefficient> DivRemCore(
            Polynomial128<TCoefficient> dividend,
            Polynomial128<TCoefficient> divisor,
            out Polynomial128<TCoefficient> remainder,
            bool isChecked) {
            var divisorSpan = GetReadOnlySpan(in divisor);
            int divisorDeg = GetDegree(divisorSpan);
            if (divisorDeg < 0)
                throw new DivideByZeroException("Polynomial division by zero.");

            remainder = dividend;
            int remainderDeg = GetDegree(GetReadOnlySpan(in remainder));
            if (remainderDeg < divisorDeg) {
                return default;
            }

            var quotient = default(Polynomial128<TCoefficient>);

            // 提前获取两个本地存储的 Span，后续不再重新赋值，因此 Span 一直有效。
            var remainderSpan = DangerousGetSpan(ref remainder);
            var quotientSpan = DangerousGetSpan(ref quotient);

            var divisorLead = divisorSpan[divisorDeg];

            while (remainderDeg >= divisorDeg) {
                int shift = remainderDeg - divisorDeg;

                if (isChecked) {
                    var factor = checked(remainderSpan[remainderDeg] / divisorLead);
                    quotientSpan[shift] = factor;
                    for (int i = 0; i <= divisorDeg; i++)
                        remainderSpan[shift + i] = checked(
                            remainderSpan[shift + i] - factor * divisorSpan[i]);
                } else {
                    var factor = remainderSpan[remainderDeg] / divisorLead;
                    quotientSpan[shift] = factor;
                    for (int i = 0; i <= divisorDeg; i++)
                        remainderSpan[shift + i] = remainderSpan[shift + i] - factor * divisorSpan[i];
                }

                remainderDeg = GetDegree(remainderSpan);
                if (remainderDeg < 0) break;
            }

            return quotient;
        }

        public static Polynomial128<TCoefficient> operator /(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => DivRemCore(left, right, out _, isChecked: false);

        public static Polynomial128<TCoefficient> operator checked /(
            Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => DivRemCore(left, right, out _, isChecked: true);

        static Polynomial128<TCoefficient>
            IModulusOperators<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>, Polynomial128<TCoefficient>>
            .operator %(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right) {
            DivRemCore(left, right, out var rem, isChecked: false);
            return rem;
        }

        // ---- 相等 / 不等 ----

        static bool
            IEqualityOperators<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>, bool>
            .operator ==(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => ((IEquatable<Polynomial128<TCoefficient>>)left).Equals(right);

        static bool
            IEqualityOperators<Polynomial128<TCoefficient>, Polynomial128<TCoefficient>, bool>
            .operator !=(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => !((IEquatable<Polynomial128<TCoefficient>>)left).Equals(right);

        // =====================================================================
        //  比较
        // =====================================================================

        /// <summary>
        /// 单项式序：先比次数，次数相同时从高次到低次逐项比较系数。
        /// 零多项式的次数为 -1，故它小于任何非零多项式。
        /// </summary>
        public int CompareTo(Polynomial128<TCoefficient> other) {
            var a = GetReadOnlySpan(in this);
            var b = GetReadOnlySpan(in other);

            int da = GetDegree(a);
            int db = GetDegree(b);

            if (da != db) return da.CompareTo(db);

            // da == db；若均为 -1（都为零多项式）则直接相等。
            for (int i = da; i >= 0; i--) {
                int cmp = Coefficient<TCoefficient>.Compare(a[i], b[i]);
                if (cmp != 0) return cmp;
            }
            return 0;
        }

        public int CompareTo(object? obj) {
            if (obj is null) return 1;
            if (obj is Polynomial128<TCoefficient> other) return CompareTo(other);
            throw new ArgumentException(
                $"Object must be of type {typeof(Polynomial128<TCoefficient>)}.",
                nameof(obj));
        }

        public static bool operator >(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => left.CompareTo(right) >= 0;

        public static bool operator <(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(Polynomial128<TCoefficient> left, Polynomial128<TCoefficient> right)
            => left.CompareTo(right) <= 0;
    }


    //public readonly partial struct Polynomial64OfInt16
    //    : IList<int, Polynomial64OfInt16.Enumerator>
    //    , IList<Int16, Polynomial64OfInt16.Enumerator>
    //    , IReadOnlyList<int, Polynomial64OfInt16.Enumerator>
    //    , IReadOnlyList<Int16, Polynomial64OfInt16.Enumerator> {

    //    private readonly UInt64 bits;

    //    public Int64 Bits {

    //        get => this.bits.ToSignedUnchecked();
    //    }

    //    internal Polynomial64OfInt16(UInt64 bits) {
    //        this.bits = bits;
    //    }

    //    public static Polynomial64OfInt16 Indeterminate {

    //        get => new Polynomial64OfInt16((UInt64)1 << 16);
    //    }

    //    public long LongCount {

    //        get {
    //            return this.Count;
    //        }
    //    }

    //    public int Count {

    //        get {
    //            var i = this.bits;
    //            if (0 == i) {
    //                return 0;
    //            }
    //            i >>= 16;
    //            if (0 == i) {
    //                return 1;
    //            }
    //            i >>= 16;
    //            if (0 == i) {
    //                return 2;
    //            }
    //            i >>= 16;
    //            if (0 == i) {
    //                return 3;
    //            }
    //            return 4;
    //        }
    //    }

    //    public bool IsReadOnly {

    //        get => true;
    //    }

    //    Int16 UltimateOrb.Collections.IReadOnlyList<Int16>.this[long index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * (int)index))));
    //            }
    //            return 0;
    //        }
    //    }

    //    Int16 UltimateOrb.Collections.IList<Int16>.this[long index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * (int)index))));
    //            }
    //            return 0;
    //        }

    //        set => throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public int this[long index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * (int)index))));
    //            }
    //            return 0;
    //        }

    //        set => throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    Int16 IReadOnlyList<Int16>.this[int index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * index))));
    //            }
    //            return 0;
    //        }
    //    }

    //    Int16 IList<Int16>.this[int index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * index))));
    //            }
    //            return 0;
    //        }

    //        set => throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public int this[int index] {

    //        get {
    //            if (0 <= index && 64 / 16 > index) {
    //                return unchecked((Int16)(checked(this.bits >> unchecked(16 * index))));
    //            }
    //            return 0;
    //        }

    //        set => throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public static Polynomial64OfInt16 FromInteger(int value) {
    //        return new Polynomial64OfInt16((UInt64)unchecked((UInt16)checked((Int16)value)));
    //    }

    //    public static Polynomial64OfInt16 operator +(Polynomial64OfInt16 first, Polynomial64OfInt16 second) {
    //        var first_d0 = first.bits;
    //        var second_d0 = second.bits;
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 0)) + unchecked((Int16)checked(second_d0 >> 16 * 0)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 1)) + unchecked((Int16)checked(second_d0 >> 16 * 1)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 2)) + unchecked((Int16)checked(second_d0 >> 16 * 2)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 3)) + unchecked((Int16)checked(second_d0 >> 16 * 3)))).Ignore();
    //        return new Polynomial64OfInt16(unchecked(first_d0 + second_d0));
    //    }

    //    public static Polynomial64OfInt16 operator -(Polynomial64OfInt16 first, Polynomial64OfInt16 second) {
    //        var first_d0 = first.bits;
    //        var second_d0 = second.bits;
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 0)) - unchecked((Int16)checked(second_d0 >> 16 * 0)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 1)) - unchecked((Int16)checked(second_d0 >> 16 * 1)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 2)) - unchecked((Int16)checked(second_d0 >> 16 * 2)))).Ignore();
    //        checked((Int16)(unchecked((Int16)checked(first_d0 >> 16 * 3)) - unchecked((Int16)checked(second_d0 >> 16 * 3)))).Ignore();
    //        return new Polynomial64OfInt16(unchecked(first_d0 - second_d0));
    //    }

    //    public Enumerator GetEnumerator() {
    //        return new Enumerator(this.bits);
    //    }

    //    IEnumerator<int> IEnumerable<int>.GetEnumerator() {
    //        return new Enumerator(this.bits);
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return new Enumerator(this.bits);
    //    }

    //    IEnumerator<Int16> IEnumerable<Int16>.GetEnumerator() {
    //        return new Enumerator(this.bits);
    //    }

    //    public int IndexOf(int item) {
    //        throw new NotImplementedException();
    //    }

    //    public void Insert(int index, int item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void RemoveAt(int index) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains<TEqualityComparer>(TEqualityComparer comparer, int item) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove<TEqualityComparer>(TEqualityComparer comparer, int item) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void Add(int item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void Clear() {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains(int item) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(int[] array, int arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove(int item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public int IndexOf<TEqualityComparer>(TEqualityComparer comparer, Int16 item) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotImplementedException();
    //    }

    //    public int IndexOf(Int16 item) {
    //        throw new NotImplementedException();
    //    }

    //    public void Insert(int index, Int16 item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains<TEqualityComparer>(TEqualityComparer comparer, Int16 item) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove<TEqualityComparer>(TEqualityComparer comparer, Int16 item) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void Add(Int16 item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains(Int16 item) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(Int16[] array, int arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove(Int16 item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public int IndexOf<TEqualityComparer>(int item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotImplementedException();
    //    }

    //    public long LongIndexOf<TEqualityComparer>(int item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotImplementedException();
    //    }

    //    public long LongIndexOf(int item) {
    //        throw new NotImplementedException();
    //    }

    //    public void Insert(long index, int item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void RemoveAt(long index) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains<TEqualityComparer>(int item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove<TEqualityComparer>(int item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<int> {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void CopyTo(int[] array, long arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(Array<int> array, int arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(Array<int> array, long arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public int IndexOf<TEqualityComparer>(Int16 item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotImplementedException();
    //    }

    //    public long LongIndexOf<TEqualityComparer>(Int16 item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotImplementedException();
    //    }

    //    public long LongIndexOf(Int16 item) {
    //        throw new NotImplementedException();
    //    }

    //    public void Insert(long index, Int16 item) {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public bool Contains<TEqualityComparer>(Int16 item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove<TEqualityComparer>(Int16 item, TEqualityComparer comparer) where TEqualityComparer : IEqualityComparer<Int16> {
    //        throw new NotSupportedException(/* NotSupported_ReadOnlyCollection */);
    //    }

    //    public void CopyTo(Int16[] array, long arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(Array<Int16> array, int arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(Array<Int16> array, long arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    public struct Enumerator : IEnumerator<int>, IEnumerator<Int16> {

    //        private UInt64 bits;

    //        private int current;

    //        public Enumerator(UInt64 bits) {
    //            this.bits = bits;
    //            this.current = 0;
    //        }

    //        public int Current {

    //            get => this.current;
    //        }

    //        object IEnumerator.Current {

    //            get => this.current;
    //        }

    //        Int16 IEnumerator<Int16>.Current {

    //            get => unchecked((Int16)this.current);
    //        }

    //        public void Dispose() {
    //        }

    //        public bool MoveNext() {
    //            var bits = this.bits;
    //            if (0 != bits) {
    //                this.current = unchecked((Int16)bits);
    //                this.bits = bits >> 16;
    //                return true;
    //            }
    //            return false;
    //        }

    //        public void Reset() {
    //            throw new NotSupportedException();
    //        }
    //    }
    //}
}
