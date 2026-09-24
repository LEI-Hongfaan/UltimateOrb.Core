#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

/*
namespace UltimateOrb.Numerics.Specialized {

    [AttributeUsage(AttributeTargets.Struct)]
    public class GenerateFixedDecimal32Attribute : Attribute {

        public int ExponentBias { get; }

        public GenerateFixedDecimal32Attribute(int exponentBias) {
            ExponentBias = exponentBias;
        }
    }
}
*/
using UltimateOrb;
using UltimateOrb.Numerics;
using UltimateOrb.Numerics.Extensions;

[assembly: System.Runtime.CompilerServices.IgnoresAccessChecksToAttribute("UltimateOrb.Core")]

namespace UltimateOrb.Core.Tests {
    using NUnit.Framework;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Runtime.Intrinsics;
    using System.Text.RegularExpressions;
    using System.Threading;
    using UltimateOrb.Collections.Generic.Interfaces.RefReturn_Huge;
    using UltimateOrb.Mathematics.Exact;
    using UltimateOrb.Mathematics.Geometry;
    using UltimateOrb.Numerics;
    using UltimateOrb.Numerics.Specialized;
    using UltimateOrb.Numerics.Tests;
    using UltimateOrb.Plain.ValueTypes;
    using UltimateOrb.Unmanaged;

    // using UltimateOrb.Runtime.CompilerServices.Tests;

    public static partial class WhitespaceRemover {

        // This attribute tells the compiler to generate efficient regex code at compile time.
        [GeneratedRegex(@"\s+", RegexOptions.Compiled)]
        private static partial Regex WhitespaceRegex();

        /// <summary>
        /// Removes all Unicode whitespace characters from the input string.
        /// </summary>
        /// <param name="input">The string from which to remove whitespace.</param>
        /// <returns>The string without any whitespace characters.</returns>
        [return: NotNullIfNotNull(nameof(input))]
        public static string? RemoveWhitespace(this string? input) {
            if (string.IsNullOrEmpty(input)) {
                return input;
            }

            return WhitespaceRegex().Replace(input, "");
        }
    }

    internal static class Win32DecimalHelpers {

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_flags")]
        internal static extern ref readonly Int32 GetFlagsInternal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_hi32")]
        internal static extern ref readonly UInt32 GetHigh32Internal(this in decimal dec);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lo64")]
        internal static extern ref readonly UInt64 GetLow64Internal(this in decimal dec);
    }

    [UltimateOrb.Numerics.Specialized.GenerateFixedDecimal32(3)]
    public readonly partial struct MilliDecimal {

    }


    internal static class Program {


        private static int aasfd() {
            var rr = new Random();
            var a = 0;

            for (var i = 0; 100 > i; ++i) {

                var asdf = rr.Next(0x8000);
                var b = new byte[asdf];
                a ^= b.GetHashCode();
            }

            for (var i = 0; GC.MaxGeneration > i; ++i) {
                if (3 > rr.Next(100)) {
                    GC.Collect();
                }
            }
            return a;
        }

        static readonly int ff = 33;

        static int ff2 = 33;
        static CanonicalIntegerBoolean ff3;
        static double aaffaa = -3.0;

        const int dssaf = 16 * 1024 * 1024;

        [StructLayout(LayoutKind.Sequential, Size = dssaf)]
        struct Bits8388608 {
            internal unsafe fixed UInt64 b[dssaf / sizeof(UInt64)];

        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.AggressiveOptimization)]
        static T UInt128ConversionTest1<T>(decimal a) where T : INumberBase<T> {
            return T.CreateChecked(a);
        }


        struct asfa : ICloneableDerivedByNongeneric<asfa> {
            public int a;
            public int b;

            public object Clone() {
                var t = this;
                ++b;
                return t;
            }

            public override string ToString() {
                return (a, b).ToString();
            }
        }
        static T Clone1<T>(ref T @this) where T : struct, ICloneable<T> {
            return @this.Clone();
        }

        static T Clone1b<T>(ref T @this) where T : ICloneable<T> {
            return @this.Clone();
        }

        static T Clone0<T>(ref T @this) where T : struct, ICloneable {
            return (T)@this.Clone();
        }
        static T Clone0a<T>(T @this) where T : struct, ICloneable {
            return (T)@this.Clone();
        }

        struct Bits24 {
            byte a;
            byte b;
            byte c;
        }

        readonly ref struct u13x64_ref {

            readonly ref InlineArray13<UInt64> dataRef;

            public ref UInt64 this[int index] => ref dataRef[index];
        }

        readonly ref struct u13x64_ref_readonly {

            readonly ref readonly InlineArray13<UInt64> dataRef;

            public ref readonly UInt64 this[int index] => ref dataRef[index];
        }

        internal static partial class AssertAlways {

            public static void Equal<T>(T a, T b) {
                if (!EqualityComparer<T>.Default.Equals(a, b)) {
                    throw new InvalidOperationException(
                        $"AssertAlways.Equal failed: expected [{a}], actual [{b}]");
                }
            }
        }
        static Quadruple[] ComputeQ(
    int n,
    Quadruple p,
    Quadruple lambda) {
            Quadruple q = 1 - p;
            Quadruple[] Q = new Quadruple[n];

            for (int k = 0; k < n; k++) {
                Q[k] =
                    p * Quadruple.Pow(q, k)
                    / Quadruple.Pow(lambda, k + 1);
            }

            return Q;
        }


        // ------------------------------------------------------------
        // Helper: print Log(x)
        // ------------------------------------------------------------
        private static void TestLog(string name, Quadruple x) {
            Quadruple y = Quadruple.Log(x);
            Console.WriteLine($"{name}: x={x}, Log(x)={y}");
        }

        // ------------------------------------------------------------
        // Helper: compare Quadruple result against Mathematica 200-digit decimal
        // ------------------------------------------------------------
        private static void VerifyAgainstMathematica(string name, Quadruple computed, string decimal200) {
            Quadruple reference = (Quadruple)BigRational.Parse(decimal200, null);

            bool equal = computed == reference;

            Console.WriteLine($"{name} (Mathematica 200-digit): {(equal ? "OK" : "MISMATCH")}");

            if (!equal) {
                Console.WriteLine($"Computed:  {computed}");
                Console.WriteLine($"Reference: {reference}");
            }
        }

public readonly struct TotalOrderIeee754Comparer<T> :
    IComparer<T>,
    System.Collections.Generic.    IEqualityComparer<T>,
    IEquatable<TotalOrderIeee754Comparer<T>>
    where T : IFloatingPointIeee754<T>, IMinMaxValue<T> {
            private const int StackAllocLimit = 128;
            private static readonly bool UseLittleEndian = BitConverter.IsLittleEndian;

            // ─────────────────────────────────────────────────────────────────
            //  Decimal format constants
            //
            //  bias = emax + p − 2,  emax = ILogB(MaxValue),  p = Precision.
            //    decimal32 : 96  +  7 − 2 =  101
            //    decimal64 : 384 + 16 − 2 =  398
            //    decimal128: 6144+ 34 − 2 = 6176
            //
            //  For a decimal NaN the written (biased) exponent has its top
            //  bits fixed as s1111Q, where s is the NaN's sign and Q is the
            //  signaling bit.  The Q bit therefore sits at
            //      qPos = exponentBitWidth − 6
            //  regardless of the format.
            // ─────────────────────────────────────────────────────────────────

            private static readonly int DecimalBias =
                T.Radix == 10 ? T.ILogB(T.MaxValue) + FloatingPointIeee754InterchageTypeTraits< T>.Precision - 2 : default;

            private static readonly int QBitPosition =
                T.Radix == 10 ? T.Zero.GetExponentByteCount() * 8 - 6 : -1;

            // ─────────────────────────────────────────────────────────────────
            //  IComparer<T>
            // ─────────────────────────────────────────────────────────────────

            public int Compare(T? x, T? y) {
                if (x is null) return y is null ? 0 : -1;
                if (y is null) return 1;

                bool xIsNaN = T.IsNaN(x);
                bool yIsNaN = T.IsNaN(y);
                if (xIsNaN || yIsNaN) return CompareNaNs(x, y, xIsNaN, yIsNaN);

                int cmp = x.CompareTo(y);
                if (cmp != 0) return cmp;

                bool xNeg = T.IsNegative(x);
                bool yNeg = T.IsNegative(y);

                // §5.10 c.1 / c.2: −0 < +0.
                if (T.IsZero(x) && T.IsZero(y) && xNeg != yNeg)
                    return xNeg ? -1 : 1;

                // §5.10 c.3: exponent tie-break is decimal-only.
                if (T.Radix != 10) return 0;

                int expCmp = ReadExponentAsInt64(x).CompareTo(ReadExponentAsInt64(y));
                return xNeg ? -expCmp : expCmp;
            }

            // ─────────────────────────────────────────────────────────────────
            //  NaN ordering
            // ─────────────────────────────────────────────────────────────────

            private static int CompareNaNs(T x, T y, bool xIsNaN, bool yIsNaN) {
                if (xIsNaN && !yIsNaN) return T.IsNegative(x) ? -1 : 1;
                if (!xIsNaN && yIsNaN) return T.IsNegative(y) ? 1 : -1;

                bool xNeg = T.IsNegative(x);
                bool yNeg = T.IsNegative(y);
                if (xNeg != yNeg) return xNeg ? -1 : 1;

                int cmp = T.Radix == 10 ? CompareDecimalNaNs(x, y) : CompareBinaryNaNs(x, y);
                return xNeg ? -cmp : cmp;
            }

            // Binary: Q is the MSB of the significand, payload is below it.
            // Unsigned significand comparison already yields sNaN < qNaN for
            // +NaN and ascending payload order.  Do not touch.
            private static int CompareBinaryNaNs(T x, T y)
                => CompareSignificands(x, y);

            // Decimal: Q is bit QBitPosition of the written exponent; payload
            // is split between the exponent (below Q) and the significand.
            private static int CompareDecimalNaNs(T x, T y) {
                long ex = ReadExponentAsInt64(x);
                long ey = ReadExponentAsInt64(y);

                long qx = (ex >> QBitPosition) & 1L;
                long qy = (ey >> QBitPosition) & 1L;

                // §5.10 d.5.ii: signaling < quiet for +NaN.  Q = 1 → signaling.
                // Reverse the comparison so that qx=1 / qy=0 gives −1.
                int qCmp = qy.CompareTo(qx);
                if (qCmp != 0) return qCmp;

                // Same kind: compare high payload bits (below Q) naturally.
                long mask = (1L << QBitPosition) - 1L;
                int pCmp = (ex & mask).CompareTo(ey & mask);
                if (pCmp != 0) return pCmp;

                // Low payload bits live in the significand.
                return CompareSignificands(x, y);
            }

            // ─────────────────────────────────────────────────────────────────
            //  Writers and readers
            // ─────────────────────────────────────────────────────────────────

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteExponent(T value, Span<byte> dest) {
                if (UseLittleEndian) value.TryWriteExponentLittleEndian(dest, out _);
                else value.TryWriteExponentBigEndian(dest, out _);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteSignificand(T value, Span<byte> dest) {
                if (UseLittleEndian) value.TryWriteSignificandLittleEndian(dest, out _);
                else value.TryWriteSignificandBigEndian(dest, out _);
            }

            private static long ReadExponentAsInt64(T value) {
                int n = value.GetExponentByteCount();
                Span<byte> buf = n <= StackAllocLimit ? stackalloc byte[n] : new byte[n];
                WriteExponent(value, buf);
                return ReadSignedInteger(buf);
            }

            private static long ReadSignedInteger(ReadOnlySpan<byte> bytes) {
                if (bytes.Length == 0) return 0;
                ulong v = 0;
                if (UseLittleEndian)
                    for (int i = bytes.Length - 1; i >= 0; i--) v = (v << 8) | bytes[i];
                else
                    for (int i = 0; i < bytes.Length; i++) v = (v << 8) | bytes[i];

                int bits = bytes.Length * 8;
                if (bits < 64 && (v & (1UL << (bits - 1))) != 0)
                    v |= ~((1UL << bits) - 1);
                return unchecked((long)v);
            }

            private static int CompareSignificands(T x, T y) {
                int nx = x.GetSignificandByteCount();
                int ny = y.GetSignificandByteCount();
                if (nx != ny) return CompareSignificandsDifferentWidth(x, nx, y, ny);

                Span<byte> bx = nx <= StackAllocLimit ? stackalloc byte[nx] : new byte[nx];
                Span<byte> by = ny <= StackAllocLimit ? stackalloc byte[ny] : new byte[ny];
                WriteSignificand(x, bx);
                WriteSignificand(y, by);
                return CompareUnsignedBytes(bx, by);
            }

            private static int CompareUnsignedBytes(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b) {
                if (UseLittleEndian) {
                    for (int i = a.Length - 1; i >= 0; i--) {
                        int c = a[i].CompareTo(b[i]);
                        if (c != 0) return c;
                    }
                    return 0;
                }
                return a.SequenceCompareTo(b);
            }

            private static int CompareSignificandsDifferentWidth(T x, int nx, T y, int ny) {
                int width = Math.Max(nx, ny);
                byte[] xBuf = new byte[width];
                byte[] yBuf = new byte[width];
                if (UseLittleEndian) {
                    WriteSignificand(x, xBuf.AsSpan(0, nx));
                    WriteSignificand(y, yBuf.AsSpan(0, ny));
                } else {
                    WriteSignificand(x, xBuf.AsSpan(width - nx));
                    WriteSignificand(y, yBuf.AsSpan(width - ny));
                }
                return CompareUnsignedBytes(xBuf, yBuf);
            }

            // ─────────────────────────────────────────────────────────────────
            //  IEqualityComparer<T>
            // ─────────────────────────────────────────────────────────────────

            public bool Equals(T? x, T? y) => Compare(x, y) == 0;

            public int GetHashCode(T obj) {
                if (obj is null) return 0;

                var hash = new HashCode();
                bool isNaN = T.IsNaN(obj);
                hash.Add(T.IsNegative(obj));
                hash.Add(isNaN);
                hash.Add(ReadExponentAsInt64(obj));

                if (isNaN) {
                    int ns = obj.GetSignificandByteCount();
                    Span<byte> sb = ns <= StackAllocLimit ? stackalloc byte[ns] : new byte[ns];
                    WriteSignificand(obj, sb);
                    hash.AddBytes(sb);
                }
                return hash.ToHashCode();
            }

            public bool Equals(TotalOrderIeee754Comparer<T> other) => true;
            public override bool Equals(object? obj) => obj is TotalOrderIeee754Comparer<T>;
            public override int GetHashCode() => 0;
            public static bool operator ==(TotalOrderIeee754Comparer<T> a, TotalOrderIeee754Comparer<T> b) => true;
            public static bool operator !=(TotalOrderIeee754Comparer<T> a, TotalOrderIeee754Comparer<T> b) => false;
        }



        // ─────────────────────────────────────────────────────────────────────────────
        //  The comparer (same as the corrected version from the previous answer)
        // ─────────────────────────────────────────────────────────────────────────────
        public readonly struct TotalOrderIeee754Comparer2<T> :
     IComparer<T>,
     System.Collections.Generic.IEqualityComparer<T>,
     IEquatable<TotalOrderIeee754Comparer2<T>>
     where T : IFloatingPointIeee754<T> {
            private const int StackAllocLimit = 128;
            private static readonly bool UseLittleEndian = BitConverter.IsLittleEndian;

            public int Compare(T? x, T? y) {
                if (x is null) return y is null ? 0 : -1;
                if (y is null) return 1;

                bool xIsNaN = T.IsNaN(x);
                bool yIsNaN = T.IsNaN(y);
                if (xIsNaN || yIsNaN) return CompareNaNs(x, y, xIsNaN, yIsNaN);

                // Finite: numeric comparison first.  CompareTo canonicalizes
                // §3.5.2 non-canonical encodings, so a non-canonical value and
                // its canonical representative both return 0 here.
                int cmp = x.CompareTo(y);
                if (cmp != 0) return cmp;

                bool xNeg = T.IsNegative(x);
                bool yNeg = T.IsNegative(y);

                // §5.10 c.1 / c.2: −0 < +0.
                if (T.IsZero(x) && T.IsZero(y) && xNeg != yNeg)
                    return xNeg ? -1 : 1;

                // §5.10 c.3 exponent tie-break is decimal-only: binary formats
                // have exactly one canonical encoding per datum, so CompareTo
                // == 0 (after the zero rule) implies identity.
                if (T.Radix != 10) return 0;

                int expCmp = CompareExponents(x, y);
                return xNeg ? -expCmp : expCmp;
            }

            // ─────────────────────────────────────────────────────────────────
            //  NaN ordering
            //
            //  The written significand for a decimal NaN has its MSB forced
            //  to 1 by UnpackDecimalIeee754, so it cannot distinguish
            //  signaling from quiet — it only carries the payload.  The
            //  written exponent for a decimal NaN is derived from the
            //  combination field, which contains the S/Q bit.
            //
            //  For a binary NaN, the significand write is the raw trailing
            //  significand, whose MSB is the quiet bit (§3.4: 1 = quiet),
            //  and the exponent write is constant.
            //
            //  So we order by exponent first, then by significand.  For
            //  decimal NaNs the exponent comparison does the S/Q work; for
            //  binary NaNs the exponent comparison is a no-op and the
            //  significand comparison does it.
            // ─────────────────────────────────────────────────────────────────

            private static int CompareNaNs(T x, T y, bool xIsNaN, bool yIsNaN) {
                // §5.10 d.1 / d.3.
                if (xIsNaN && !yIsNaN) return T.IsNegative(x) ? -1 : 1;
                if (!xIsNaN && yIsNaN) return T.IsNegative(y) ? 1 : -1;

                // §5.10 d.5.i: negative sign orders below positive sign.
                bool xNeg = T.IsNegative(x);
                bool yNeg = T.IsNegative(y);
                if (xNeg != yNeg) return xNeg ? -1 : 1;

                int expCmp = CompareExponents(x, y);
                if (expCmp != 0) {
                    // Reached only for decimal formats.  The decimal encoding
                    // places the S/Q bit inside the combination field, and the
                    // canonical values show it is 1 for signaling (0x7e… vs
                    // 0x7c…).  The exponent write therefore evaluates larger
                    // for sNaN.  §5.10 d.5.ii wants signaling below quiet, so
                    // negate.
                    //
                    // §5.10 d.5.iii says payload ordering within the same kind
                    // and sign is implementation-defined, so the fact that the
                    // negation also reverses payload order is permitted.
                    int cmp = T.Radix == 10 ? -expCmp : expCmp;
                    return xNeg ? -cmp : cmp;
                }

                // Same exponent write.  Compare significands.
                //   Binary  — MSB = 1 for quiet (§3.4).  Unsigned order is
                //             sNaN < qNaN already, and the payload bits below
                //             the MSB order payloads normally.
                //   Decimal — MSB is forced to 1 by the decoder, so this
                //             comparison only orders payloads; the S/Q
                //             distinction has already been handled above.
                //             Do not negate: §5.10 d.5.iii leaves payload
                //             order implementation-defined, and keeping the
                //             natural order matches the binary behaviour.
                int sigCmp = CompareSignificands(x, y);
                return xNeg ? -sigCmp : sigCmp;
            }

            // ─────────────────────────────────────────────────────────────────
            //  Writers — native endianness, one group per machine
            // ─────────────────────────────────────────────────────────────────

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteExponent(T value, Span<byte> dest) {
                if (UseLittleEndian) value.TryWriteExponentLittleEndian(dest, out _);
                else value.TryWriteExponentBigEndian(dest, out _);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void WriteSignificand(T value, Span<byte> dest) {
                if (UseLittleEndian) value.TryWriteSignificandLittleEndian(dest, out _);
                else value.TryWriteSignificandBigEndian(dest, out _);
            }

            // ─────────────────────────────────────────────────────────────────
            //  Exponent / significand comparison
            // ─────────────────────────────────────────────────────────────────

            private static int CompareExponents(T x, T y) {
                int nx = x.GetExponentByteCount();
                int ny = y.GetExponentByteCount();
                if (nx == ny) {
                    Span<byte> bx = nx <= StackAllocLimit ? stackalloc byte[nx] : new byte[nx];
                    Span<byte> by = ny <= StackAllocLimit ? stackalloc byte[ny] : new byte[ny];
                    WriteExponent(x, bx);
                    WriteExponent(y, by);
                    return CompareSignedBytes(bx, by);
                }
                return CompareExponentsDifferentWidth(x, nx, y, ny);
            }

            private static int CompareSignificands(T x, T y) {
                int nx = x.GetSignificandByteCount();
                int ny = y.GetSignificandByteCount();
                if (nx == ny) {
                    Span<byte> bx = nx <= StackAllocLimit ? stackalloc byte[nx] : new byte[nx];
                    Span<byte> by = ny <= StackAllocLimit ? stackalloc byte[ny] : new byte[ny];
                    WriteSignificand(x, bx);
                    WriteSignificand(y, by);
                    return CompareUnsignedBytes(bx, by);
                }
                return CompareSignificandsDifferentWidth(x, nx, y, ny);
            }

            // ─────────────────────────────────────────────────────────────────
            //  Integer comparisons over byte spans
            // ─────────────────────────────────────────────────────────────────

            private static int CompareSignedBytes(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b) {
                int signA = UseLittleEndian ? (a[^1] >> 7) : (a[0] >> 7);
                int signB = UseLittleEndian ? (b[^1] >> 7) : (b[0] >> 7);
                if (signA != signB) return signB - signA;

                if (UseLittleEndian) {
                    for (int i = a.Length - 1; i >= 0; i--) {
                        int c = a[i].CompareTo(b[i]);
                        if (c != 0) return c;
                    }
                    return 0;
                }
                return a.SequenceCompareTo(b);
            }

            private static int CompareUnsignedBytes(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b) {
                if (UseLittleEndian) {
                    for (int i = a.Length - 1; i >= 0; i--) {
                        int c = a[i].CompareTo(b[i]);
                        if (c != 0) return c;
                    }
                    return 0;
                }
                return a.SequenceCompareTo(b);
            }

            // ─────────────────────────────────────────────────────────────────
            //  Different-width fallbacks (dead code for well-formed T)
            // ─────────────────────────────────────────────────────────────────

            private static int CompareExponentsDifferentWidth(T x, int nx, T y, int ny) {
                int width = Math.Max(nx, ny);
                byte[] xBuf = new byte[width];
                byte[] yBuf = new byte[width];

                if (UseLittleEndian) {
                    WriteExponent(x, xBuf.AsSpan(0, nx));
                    WriteExponent(y, yBuf.AsSpan(0, ny));
                    xBuf.AsSpan(nx).Fill((byte)((sbyte)xBuf[nx - 1] >> 7));
                    yBuf.AsSpan(ny).Fill((byte)((sbyte)yBuf[ny - 1] >> 7));
                } else {
                    WriteExponent(x, xBuf.AsSpan(width - nx));
                    WriteExponent(y, yBuf.AsSpan(width - ny));
                    xBuf.AsSpan(0, width - nx).Fill((byte)((sbyte)xBuf[width - nx] >> 7));
                    yBuf.AsSpan(0, width - ny).Fill((byte)((sbyte)yBuf[width - ny] >> 7));
                }
                return CompareSignedBytes(xBuf, yBuf);
            }

            private static int CompareSignificandsDifferentWidth(T x, int nx, T y, int ny) {
                int width = Math.Max(nx, ny);
                byte[] xBuf = new byte[width];
                byte[] yBuf = new byte[width];
                if (UseLittleEndian) {
                    WriteSignificand(x, xBuf.AsSpan(0, nx));
                    WriteSignificand(y, yBuf.AsSpan(0, ny));
                } else {
                    WriteSignificand(x, xBuf.AsSpan(width - nx));
                    WriteSignificand(y, yBuf.AsSpan(width - ny));
                }
                return CompareUnsignedBytes(xBuf, yBuf);
            }

            // ─────────────────────────────────────────────────────────────────
            //  IEqualityComparer<T>
            // ─────────────────────────────────────────────────────────────────

            public bool Equals(T? x, T? y) => Compare(x, y) == 0;

            public int GetHashCode(T obj) {
                if (obj is null) return 0;

                var hash = new HashCode();
                bool isNaN = T.IsNaN(obj);
                hash.Add(T.IsNegative(obj));
                hash.Add(isNaN);

                // The exponent write is canonical for every finite encoding:
                // non-canonical values (§3.5.2) share the exponent of their
                // canonical representative, and Compare already reports them
                // equal via CompareTo.
                int ne = obj.GetExponentByteCount();
                Span<byte> eb = ne <= StackAllocLimit ? stackalloc byte[ne] : new byte[ne];
                WriteExponent(obj, eb);
                hash.AddBytes(eb);

                // Include the significand only for NaN.  For a decimal NaN the
                // significand carries the payload; for a binary NaN it carries
                // the payload and the quiet bit.  For finite values, omitting
                // it is essential: a non-canonical encoding's raw coefficient
                // is zeroed by the decoder but might still differ from a
                // canonical encoding's bytes on some implementations, and
                // Compare reports the two as equal.
                if (isNaN) {
                    int ns = obj.GetSignificandByteCount();
                    Span<byte> sb = ns <= StackAllocLimit ? stackalloc byte[ns] : new byte[ns];
                    WriteSignificand(obj, sb);
                    hash.AddBytes(sb);
                }

                return hash.ToHashCode();
            }

            public bool Equals(TotalOrderIeee754Comparer2<T> other) => true;
            public override bool Equals(object? obj) => obj is TotalOrderIeee754Comparer2<T>;
            public override int GetHashCode() => 0;
            public static bool operator ==(TotalOrderIeee754Comparer2<T> a, TotalOrderIeee754Comparer2<T> b) => true;
            public static bool operator !=(TotalOrderIeee754Comparer2<T> a, TotalOrderIeee754Comparer2<T> b) => false;
        }

        private static int _passed;
        private static int _failed;

        // ─────────────────────────────────────────────────────────────────────────────
        //  Shared test harness
        // ─────────────────────────────────────────────────────────────────────────────
        public static class TestHarness {
            public static int Passed;
            public static int Failed;

            public static void Assert(bool condition, string message) {
                if (condition) { Passed++; } else {
                    Failed++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"    FAIL: {message}");
                    Console.ResetColor();
                }
            }

            public static void Reset() {
                Passed = 0;
                Failed = 0;
            }

            public static void PrintSummary(string typeName) {
                Console.WriteLine();
                Console.ForegroundColor = Failed == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"  === {typeName}: {Passed} passed, {Failed} failed ===");
                Console.ResetColor();
            }
        }

        // ─────────────────────────────────────────────────────────────────────────────
        //  Decimal-format test suite (works for any IFloatingPointIeee754 decimal type)
        // ─────────────────────────────────────────────────────────────────────────────
        public static class Decimal128TestSuite<T>
            where T : IFloatingPointIeee754<T> {
            public static void Run(string typeName) {
                Console.WriteLine($"\n═══════════════════════════════════════════════════════════════");
                Console.WriteLine($"  {typeName}");
                Console.WriteLine($"═══════════════════════════════════════════════════════════════\n");

                TestHarness.Reset();
                var cmp = new TotalOrderIeee754Comparer<T>();
                var eq = (System.Collections.Generic.IEqualityComparer<T>)cmp;

                // ─────────────────────────────────────────────────────────────────
                // Construct values through Parse.  Decimal formats support
                // cohort-distinguishing literals: "1.0" and "1.00" have the same
                // numeric value but different quantum exponents.
                // ─────────────────────────────────────────────────────────────────

                // Non-NaN canonical values
                T posInf = T.Parse("∞", null);
                T negInf = T.Parse("-∞", null);

                T posOne = T.Parse("1", null);
                T negOne = T.Parse("-1", null);

                T posTwo = T.Parse("2", null);
                T negTwo = T.Parse("-2", null);

                // Cohort pairs: same numeric value, different exponent
                T posOneC1 = T.Parse("1.0", null);     // exponent -1
                T posOneC2 = T.Parse("1.00", null);    // exponent -2
                T negOneC1 = T.Parse("-1.0", null);
                T negOneC2 = T.Parse("-1.00", null);

                // Zero cohorts
                T posZero = T.Parse("0", null);
                T negZero = T.Parse("-0", null);
                T posZeroC1 = T.Parse("0.0", null);
                T posZeroC2 = T.Parse("0.00", null);
                T negZeroC1 = T.Parse("-0.0", null);
                T negZeroC2 = T.Parse("-0.00", null);

                // Sign- and kind-specified NaNs via Fortran-style syntax.
                // Explicit '+' or '-' is required; bare "NaN" has an unspecified sign.
                T posQNaN1 = typeof(T)== typeof(System.Numerics.Decimal128) ?
                    (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("+qNaN(1)"))) :
                    T.Parse("+qNaN(1)", null);
                T posQNaN2 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("+qNaN(2)"))) : T.Parse("+qNaN(2)", null);
                T posSNaN1 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("+sNaN(1)"))) : T.Parse("+sNaN(1)", null);
                T posSNaN2 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("+sNaN(2)"))) : T.Parse("+sNaN(2)", null);
                T negQNaN1 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("-qNaN(1)"))) : T.Parse("-qNaN(1)", null);
                T negQNaN2 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("-qNaN(2)"))) : T.Parse("-qNaN(2)", null);
                T negSNaN1 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("-sNaN(1)"))) : T.Parse("-sNaN(1)", null);
                T negSNaN2 = typeof(T) == typeof(System.Numerics.Decimal128) ? (T)(object)System.Numerics.Decimal128.DecodeBinary(Unsafe.BitCast<Decimal128Bid, UInt128>(Decimal128Bid.Parse("-sNaN(2)"))) : T.Parse("-sNaN(2)", null);

                // Sanity: signs are as intended
                TestHarness.Assert(!T.IsNegative(posQNaN1), "posQNaN1 has positive sign");
                TestHarness.Assert(T.IsNegative(negQNaN1), "negQNaN1 has negative sign");
                TestHarness.Assert(!T.IsNegative(posSNaN1), "posSNaN1 has positive sign");
                TestHarness.Assert(T.IsNegative(negSNaN1), "negSNaN1 has negative sign");
                TestHarness.Assert(T.IsNaN(posQNaN1), "posQNaN1 is NaN");
                TestHarness.Assert(T.IsNaN(negSNaN1), "negSNaN1 is NaN");

                // ─────────────────────────────────────────────────────────────────
                // 1. Numeric ordering
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("1. Numeric ordering");
                TestHarness.Assert(cmp.Compare(negInf, negTwo) < 0, "-Inf < -2");
                TestHarness.Assert(cmp.Compare(negTwo, negOne) < 0, "-2 < -1");
                TestHarness.Assert(cmp.Compare(negOne, negZero) < 0, "-1 < -0");
                TestHarness.Assert(cmp.Compare(negZero, posZero) < 0, "-0 < +0");
                TestHarness.Assert(cmp.Compare(posZero, posOne) < 0, "+0 < +1");
                TestHarness.Assert(cmp.Compare(posOne, posTwo) < 0, "+1 < +2");
                TestHarness.Assert(cmp.Compare(posTwo, posInf) < 0, "+2 < +Inf");

                // ─────────────────────────────────────────────────────────────────
                // 2. Reflexivity and antisymmetry
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("2. Reflexivity & antisymmetry");
                var coreValues = new[]
                {
            negQNaN2, negQNaN1, negSNaN2, negSNaN1,
            negInf, negTwo, negOne, negZero,
            posZero, posOne, posTwo, posInf,
            posSNaN1, posSNaN2, posQNaN1, posQNaN2
        };
                foreach (var v in coreValues) {
                    TestHarness.Assert(cmp.Compare(v, v) == 0, $"Compare({v}, {v}) == 0");
                    TestHarness.Assert(eq.Equals(v, v), $"Equals({v}, {v})");
                }
                for (int i = 0; i < coreValues.Length; i++)
                    for (int j = 0; j < coreValues.Length; j++) {
                        int ij = Math.Sign(cmp.Compare(coreValues[i], coreValues[j]));
                        int ji = Math.Sign(cmp.Compare(coreValues[j], coreValues[i]));
                        TestHarness.Assert(ij == -ji, $"antisymmetry [{coreValues[i]}, {coreValues[j]}]");
                    }

                // ─────────────────────────────────────────────────────────────────
                // 3. Signed zeros (§5.10 c.1 / c.2)
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("3. Signed zeros");
                TestHarness.Assert(cmp.Compare(negZero, posZero) < 0, "totalOrder(-0, +0) is true");
                TestHarness.Assert(cmp.Compare(posZero, negZero) > 0, "totalOrder(+0, -0) is false");
                TestHarness.Assert(!eq.Equals(negZero, posZero), "-0 and +0 not equal under totalOrder");
                TestHarness.Assert(!eq.Equals(posZero, negZero), "+0 and -0 not equal under totalOrder");

                // ─────────────────────────────────────────────────────────────────
                // 4. Decimal cohorts (§5.10 c.3) — the key decimal-specific test
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("4. Decimal cohorts (exponent tie-break)");

                // Positive: smaller exponent orders first (1.00 < 1.0)
                TestHarness.Assert(cmp.Compare(posOneC2, posOneC1) < 0, "+1.00 < +1.0 (exponent -2 < -1)");
                TestHarness.Assert(cmp.Compare(posOneC1, posOneC2) > 0, "+1.0 > +1.00");
                TestHarness.Assert(!eq.Equals(posOneC2, posOneC1), "+1.00 and +1.0 are not equal under totalOrder");

                // Negative: larger exponent orders first (-1.0 < -1.00)
                TestHarness.Assert(cmp.Compare(negOneC1, negOneC2) < 0, "-1.0 < -1.00 (exponent -1 > -2)");
                TestHarness.Assert(cmp.Compare(negOneC2, negOneC1) > 0, "-1.00 > -1.0");
                TestHarness.Assert(!eq.Equals(negOneC1, negOneC2), "-1.0 and -1.00 are not equal under totalOrder");

                // Zero cohorts: same-sign zeros still distinguished by exponent
                TestHarness.Assert(cmp.Compare(posZeroC2, posZeroC1) < 0, "+0.00 < +0.0");
                TestHarness.Assert(cmp.Compare(negZeroC1, negZeroC2) < 0, "-0.0 < -0.00");
                TestHarness.Assert(!eq.Equals(posZeroC2, posZeroC1), "+0.00 and +0.0 not equal");
                TestHarness.Assert(!eq.Equals(negZeroC1, negZeroC2), "-0.0 and -0.00 not equal");

                // Cross-sign zero cohorts: sign rule takes precedence over exponent
                TestHarness.Assert(cmp.Compare(negZeroC2, posZeroC1) < 0, "-0.00 < +0.0 (sign first)");

                // ─────────────────────────────────────────────────────────────────
                // 5. NaN ordering (§5.10 d)
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("5. NaN ordering");

                // d.1: -NaN below every non-NaN
                TestHarness.Assert(cmp.Compare(negQNaN1, negInf) < 0, "-qNaN < -Inf");
                TestHarness.Assert(cmp.Compare(negSNaN1, negOne) < 0, "-sNaN < -1");
                TestHarness.Assert(cmp.Compare(negQNaN1, negZero) < 0, "-qNaN < -0");
                TestHarness.Assert(cmp.Compare(negQNaN1, posZero) < 0, "-qNaN < +0");
                TestHarness.Assert(cmp.Compare(negQNaN1, posInf) < 0, "-qNaN < +Inf");

                // d.3: +NaN above every non-NaN
                TestHarness.Assert(cmp.Compare(posQNaN1, negInf) > 0, "+qNaN > -Inf");
                TestHarness.Assert(cmp.Compare(posSNaN1, posOne) > 0, "+sNaN > +1");
                TestHarness.Assert(cmp.Compare(posQNaN1, posInf) > 0, "+qNaN > +Inf");

                // d.5.i: negative sign orders below positive sign
                TestHarness.Assert(cmp.Compare(negQNaN1, posQNaN1) < 0, "-qNaN < +qNaN");
                TestHarness.Assert(cmp.Compare(negSNaN1, posSNaN1) < 0, "-sNaN < +sNaN");
                TestHarness.Assert(cmp.Compare(negQNaN1, posSNaN1) < 0, "-qNaN < +sNaN");
                TestHarness.Assert(cmp.Compare(negSNaN1, posQNaN1) < 0, "-sNaN < +qNaN");

                // d.5.ii: for +NaN, signaling < quiet; for -NaN, quiet < signaling
                TestHarness.Assert(cmp.Compare(posSNaN1, posQNaN1) < 0, "+sNaN < +qNaN");
                TestHarness.Assert(cmp.Compare(posQNaN1, posSNaN1) > 0, "+qNaN > +sNaN");
                TestHarness.Assert(cmp.Compare(negQNaN1, negSNaN1) < 0, "-qNaN < -sNaN");
                TestHarness.Assert(cmp.Compare(negSNaN1, negQNaN1) > 0, "-sNaN > -qNaN");

                // Payload ordering within same kind and sign
                TestHarness.Assert(cmp.Compare(posSNaN1, posSNaN2) < 0, "+sNaN1 < +sNaN2 (payload)");
                TestHarness.Assert(cmp.Compare(posQNaN1, posQNaN2) < 0, "+qNaN1 < +qNaN2 (payload)");
                TestHarness.Assert(cmp.Compare(negSNaN1, negSNaN2) > 0, "-sNaN1 > -sNaN2 (payload, reversed)");
                TestHarness.Assert(cmp.Compare(negQNaN1, negQNaN2) > 0, "-qNaN1 > -qNaN2 (payload, reversed)");

                // Bitwise-identical NaNs are equal
                TestHarness.Assert(cmp.Compare(posQNaN1, posQNaN1) == 0, "+qNaN1 == +qNaN1");
                TestHarness.Assert(cmp.Compare(negSNaN1, negSNaN1) == 0, "-sNaN1 == -sNaN1");

                // Distinct NaNs are not equal
                TestHarness.Assert(!eq.Equals(posQNaN1, posQNaN2), "+qNaN1 != +qNaN2 (payload)");
                TestHarness.Assert(!eq.Equals(posSNaN1, posQNaN1), "+sNaN1 != +qNaN1 (kind)");
                TestHarness.Assert(!eq.Equals(negQNaN1, negSNaN1), "-qNaN1 != -sNaN1 (kind)");
                TestHarness.Assert(!eq.Equals(posQNaN1, negQNaN1), "+qNaN1 != -qNaN1 (sign)");

                // ─────────────────────────────────────────────────────────────────
                // 6. Transitivity and totality over the full chain
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("6. Transitivity & totality");
                for (int i = 0; i < coreValues.Length; i++)
                    for (int j = i + 1; j < coreValues.Length; j++)
                        for (int k = j + 1; k < coreValues.Length; k++) {
                            int ij = cmp.Compare(coreValues[i], coreValues[j]);
                            int jk = cmp.Compare(coreValues[j], coreValues[k]);
                            int ik = cmp.Compare(coreValues[i], coreValues[k]);
                            TestHarness.Assert(ij < 0 && jk < 0 ? ik < 0 : true,
                                $"transitivity [{coreValues[i]}, {coreValues[j]}, {coreValues[k]}]");
                        }
                for (int i = 0; i < coreValues.Length; i++)
                    for (int j = 0; j < coreValues.Length; j++) {
                        int c = Math.Sign(cmp.Compare(coreValues[i], coreValues[j]));
                        TestHarness.Assert(c is -1 or 0 or 1, $"totality [{coreValues[i]}, {coreValues[j]}]");
                    }

                // ─────────────────────────────────────────────────────────────────
                // 7. Interface consistency
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("7. Interface consistency");
                TestHarness.Assert(eq.Equals(negOne, negOne), "Equals(-1, -1)");
                TestHarness.Assert(!eq.Equals(negOne, posOne), "!Equals(-1, +1)");
                TestHarness.Assert(eq.Equals(posQNaN1, posQNaN1), "Equals(+qNaN1, +qNaN1)");
                TestHarness.Assert(eq.Equals(negSNaN1, negSNaN1), "Equals(-sNaN1, -sNaN1)");

                TestHarness.Assert(eq.GetHashCode(negOne) == eq.GetHashCode(negOne), "hash(-1) == hash(-1)");
                TestHarness.Assert(eq.GetHashCode(posQNaN1) == eq.GetHashCode(posQNaN1),
                    "hash(+qNaN1) == hash(+qNaN1)");
                TestHarness.Assert(eq.GetHashCode(posOneC1) == eq.GetHashCode(posOneC1),
                    "hash(+1.0) == hash(+1.0)");
                TestHarness.Assert(eq.GetHashCode(posOneC1) != eq.GetHashCode(posOneC2),
                    "hash(+1.0) != hash(+1.00) — cohorts hash differently");

                var set = new HashSet<T>(eq);
                foreach (var v in coreValues) set.Add(v);
                TestHarness.Assert(set.Count == coreValues.Length,
                    $"HashSet has {coreValues.Length} distinct values (got {set.Count})");

                // ─────────────────────────────────────────────────────────────────
                // 8. Sorting the full chain
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("8. Sorting");
                var shuffled = new[]
                {
            posQNaN2, negTwo, posSNaN1, posZero, negQNaN1, negInf,
            posOne, negSNaN2, negZero, posInf,
            negQNaN2, posTwo, negSNaN1, negOne, posQNaN1, posSNaN2
        };
                Array.Sort(shuffled, cmp);
                for (int i = 0; i < coreValues.Length; i++) {
                    TestHarness.Assert(cmp.Compare(shuffled[i], coreValues[i]) == 0,
                        $"sorted[{i}] == {coreValues[i]} (got {shuffled[i]})");
                }

                // ─────────────────────────────────────────────────────────────────
                // 9. Parser round-trips
                // ─────────────────────────────────────────────────────────────────
                Console.WriteLine("9. Parser round-trips");
                if (typeof(T) != typeof(System.Numerics.Decimal128)) {
                    TestHarness.Assert(cmp.Compare(T.Parse("+qNaN(1)", null), posQNaN1) == 0,
                        "Parse(\"+NaN(Q1)\") == posQNaN1");
                    TestHarness.Assert(cmp.Compare(T.Parse("-sNaN(1)", null), negSNaN1) == 0,
                        "Parse(\"-NaN(S1)\") == negSNaN1");
                }
                TestHarness.Assert(cmp.Compare(T.Parse("1.00", null), posOneC2) == 0,
                    "Parse(\"1.00\") == posOneC2");

                // CopySign preserves NaN kind and payload
                TestHarness.Assert(cmp.Compare(T.CopySign(posSNaN1, -T.One), negSNaN1) == 0,
                    "CopySign(+sNaN1, -1) == -sNaN1");
                TestHarness.Assert(cmp.Compare(T.CopySign(negQNaN2, T.One), posQNaN2) == 0,
                    "CopySign(-qNaN2, +1) == +qNaN2");

                TestHarness.PrintSummary(typeName);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        private static int Main(string[] args) {
            {
                Console.WriteLine($"Decimal128Bid.MaxValue = {Decimal128Bid.MaxValue}");
                Console.WriteLine($"Decimal128Bid.MaxValue = {Decimal128Bid.ILogB(Decimal128Bid.MaxValue)}");


            }

            {
                Console.WriteLine($"Scale10(0, -7) = {Decimal128Bid.Scale10(0, -7)}");
                Console.WriteLine($"Scale10(0, +7) = {Decimal128Bid.Scale10(0, +7)}");
                Console.WriteLine($"Scale10(0, +7000) = {Decimal128Bid.Scale10(0, +7000)}");
                Console.WriteLine($"Scale10(0, -7000) = {Decimal128Bid.Scale10(0, -7000)}");

                Console.WriteLine($"IsNegative(-0) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("-0"))}");

                Decimal128Bid[] testData1 = [
                    Decimal128Bid.Parse("+qNaN(0x4000000000000000000000000009)"),
                    Decimal128Bid.Parse("+qNaN(0X3ffffffffffffffffffffffffffF)"),
                    Decimal128Bid.Parse("sNaN(111)"),
                    Decimal128Bid.Parse("qNaN(222)"),
                    Decimal128Bid.Parse("NaN(333)"),
                    Decimal128Bid.Parse("+sNaN(444)"),
                    Decimal128Bid.Parse("+qNaN(555)"),
                    Decimal128Bid.Parse("+NaN(666)"),
                    Decimal128Bid.Parse("-sNaN(777)"),
                    Decimal128Bid.Parse("-qNaN(888)"),
                    Decimal128Bid.Parse("-NaN(999)"),

                    Decimal128Bid.Parse("+inF"),
                    Decimal128Bid.Parse("-∞"),
                    Decimal128Bid.Parse("Infinity"),
                    Decimal128Bid.Parse("1919810"),
                    Decimal128Bid.ToCoarsestCohort(Decimal128Bid.Parse("1919810")),
                    Decimal128Bid.ToFinestCohort(Decimal128Bid.Parse("1919810")),

                    Decimal128Bid.Parse("-10100"),
                    Decimal128Bid.ToCoarsestCohort(Decimal128Bid.Parse("-10100")),
                    Decimal128Bid.ToFinestCohort(Decimal128Bid.Parse("-10100")),
                    Decimal128Bid.ToCohort(Decimal128Bid.Parse("-10100"), qExponent: 1),
                    Decimal128Bid.Pi,
                    Decimal128Bid.Tau,
                    Decimal128Bid.Epsilon,
                    -Decimal128Bid.Epsilon,
                    -Decimal128Bid.AdditiveIdentity,
                    Decimal128Bid.AdditiveIdentity,
                    Decimal128Bid.BitDecrement(Decimal128Bid.Epsilon),
                    Decimal128Bid.BitIncrement(Decimal128Bid.Epsilon),
                    Decimal128Bid.BitDecrement(-Decimal128Bid.Epsilon),
                    Decimal128Bid.BitIncrement(-Decimal128Bid.Epsilon),

                    Decimal128Bid.Parse("-0"),
                    Decimal128Bid.Parse("+0"),
                    Decimal128Bid.Parse("-.0E-9000"),

                    Decimal128Bid.Parse("-∞"),
                    Decimal128Bid.Parse("Infinity"), ];

                foreach (var item in testData1.OrderBy(Decimal128Extensions.TotalOrderIeee754_192BitsKeySelector)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(x => x, new TotalOrderIeee754Comparer<Decimal128Bid>())) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(x => x)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
                foreach (var item in testData1.OrderBy(Decimal128Extensions.TotalOrderDefaultSystemInt128KeySelector)) {
                    Console.Write(item.ToStringWithSignAndNaNPayload());
                    Console.Write(' ');
                }
                Console.WriteLine();
            }
            {
                Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║  IEEE 754 totalOrder tests for Decimal128Bid and Decimal128Dpd ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝");

                Decimal128TestSuite<System.Numerics.Decimal128>.Run("System.Numerics.Decimal128");
                Decimal128TestSuite<Decimal128Bid>.Run("Decimal128Bid");
                Decimal128TestSuite<Decimal128Dpd>.Run("Decimal128Dpd");

                // Overall summary
                Console.WriteLine();
                int totalPassed = TestHarness.Passed; // only last run's counts; use counters per type if needed
                Console.WriteLine("Done. Check the per-type summaries above.");
            }
            {


                static void Assert(bool condition, string message) {
                    if (condition) { _passed++; } else {
                        _failed++;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"  FAIL: {message}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("=== UltimateOrb.Quadruple totalOrder tests ===\n");

                var cmp = new System.Numerics.TotalOrderIeee754Comparer<Quadruple>();
                var eq = (System.Collections.Generic.IEqualityComparer<Quadruple>)cmp;
                // -----------------------------------------------------------------
                // Canonical values.
                //
                // The Fortran-style NaN literal has THREE independent axes:
                //
                //     sign     kind       payload
                //     ----     ----       -------
                //     '+'/'-'  'Q'/'S'/∅  decimal digits
                //
                // If the sign is omitted, the sign bit is UNSPECIFIED — it may be
                // positive on one platform and negative on another.  So every NaN
                // we construct here is written with an explicit leading '+' or '-'.
                // The same rule applies to bare "NaN": always write "+NaN" or
                // "-NaN" when the sign matters.
                //
                // Grammar (case-insensitive):
                //     [+|-] NaN ( [Q|S] digits? )
                // Examples:
                //     +NaN           quiet, positive, default payload
                //     +NaN(Q1)       quiet, positive, payload 1
                //     -NaN(S2)       signaling, negative, payload 2
                // -----------------------------------------------------------------
                Quadruple posInf = Quadruple.PositiveInfinity;
                Quadruple negInf = Quadruple.NegativeInfinity;

                // Quiet NaNs, positive sign.
                Quadruple posQNaN1 = Quadruple.Parse("+NaN(Q1)");
                Quadruple posQNaN2 = Quadruple.Parse("+NaN(Q2)");

                // Signaling NaNs, positive sign.
                Quadruple posSNaN1 = Quadruple.Parse("+NaN(S1)");
                Quadruple posSNaN2 = Quadruple.Parse("+NaN(S2)");

                // Quiet and signaling NaNs, negative sign.
                Quadruple negQNaN1 = Quadruple.Parse("-NaN(Q1)");
                Quadruple negQNaN2 = Quadruple.Parse("-NaN(Q2)");
                Quadruple negSNaN1 = Quadruple.Parse("-NaN(S1)");
                Quadruple negSNaN2 = Quadruple.Parse("-NaN(S2)");

                Quadruple posZero = Quadruple.Zero;
                Quadruple negZero = Quadruple.NegativeZero;

                Quadruple posOne = Quadruple.One;
                Quadruple negOne = -Quadruple.One;
                Quadruple posTwo = Quadruple.One + Quadruple.One;
                Quadruple negTwo = -posTwo;

                // Sanity: every constructed NaN really has the intended sign.
                Assert(Quadruple.IsNaN(posQNaN1), "posQNaN1 is NaN");
                Assert(!Quadruple.IsNegative(posQNaN1), "posQNaN1 has positive sign");
                Assert(Quadruple.IsNaN(posSNaN1), "posSNaN1 is NaN");
                Assert(!Quadruple.IsNegative(posSNaN1), "posSNaN1 has positive sign");
                Assert(Quadruple.IsNaN(negQNaN1), "negQNaN1 is NaN");
                Assert(Quadruple.IsNegative(negQNaN1), "negQNaN1 has negative sign");
                Assert(Quadruple.IsNaN(negSNaN1), "negSNaN1 is NaN");
                Assert(Quadruple.IsNegative(negSNaN1), "negSNaN1 has negative sign");

                // -----------------------------------------------------------------
                // The full canonical chain in totalOrder:
                //
                //   -qNaN2 < -qNaN1 < -sNaN2 < -sNaN1
                //     < -Inf < -2 < -1 < -0 < +0 < +1 < +2 < +Inf
                //     < +sNaN1 < +sNaN2 < +qNaN1 < +qNaN2
                //
                // For positive NaNs, larger payload → larger significand → greater.
                // For negative NaNs, the comparison is reversed, so larger payload
                // orders first.
                // -----------------------------------------------------------------
                var canonical = new[]
                {
            negQNaN2, negQNaN1, negSNaN2, negSNaN1,
            negInf, negTwo, negOne, negZero,
            posZero, posOne, posTwo, posInf,
            posSNaN1, posSNaN2, posQNaN1, posQNaN2
        };

                // -----------------------------------------------------------------
                // 1. Numeric ordering
                // -----------------------------------------------------------------
                Console.WriteLine("1. Numeric ordering");
                Assert(cmp.Compare(negInf, negTwo) < 0, "-Inf < -2");
                Assert(cmp.Compare(negTwo, negOne) < 0, "-2 < -1");
                Assert(cmp.Compare(negOne, negZero) < 0, "-1 < -0");
                Assert(cmp.Compare(negZero, posZero) < 0, "-0 < +0");
                Assert(cmp.Compare(posZero, posOne) < 0, "+0 < +1");
                Assert(cmp.Compare(posOne, posTwo) < 0, "+1 < +2");
                Assert(cmp.Compare(posTwo, posInf) < 0, "+2 < +Inf");

                // -----------------------------------------------------------------
                // 2. Reflexivity and antisymmetry
                // -----------------------------------------------------------------
                Console.WriteLine("2. Reflexivity & antisymmetry");
                foreach (var v in canonical) {
                    Assert(cmp.Compare(v, v) == 0, $"Compare({v}, {v}) == 0");
                    Assert(eq.Equals(v, v), $"Equals({v}, {v})");
                }
                for (int i = 0; i < canonical.Length; i++)
                    for (int j = 0; j < canonical.Length; j++) {
                        int ij = Math.Sign(cmp.Compare(canonical[i], canonical[j]));
                        int ji = Math.Sign(cmp.Compare(canonical[j], canonical[i]));
                        Assert(ij == -ji, $"antisymmetry [{canonical[i]}, {canonical[j]}]");
                    }

                // -----------------------------------------------------------------
                // 3. Signed zeros (§5.10 c.1 / c.2)
                // -----------------------------------------------------------------
                Console.WriteLine("3. Signed zeros");
                Assert(cmp.Compare(negZero, posZero) < 0, "totalOrder(-0, +0) is true");
                Assert(cmp.Compare(posZero, negZero) > 0, "totalOrder(+0, -0) is false");
                Assert(!eq.Equals(negZero, posZero), "-0 and +0 not equal under totalOrder");
                Assert(!eq.Equals(posZero, negZero), "+0 and -0 not equal under totalOrder");

                // -----------------------------------------------------------------
                // 4. NaN ordering (§5.10 d)
                // -----------------------------------------------------------------
                Console.WriteLine("4. NaN ordering");

                // d.1: -NaN is below every non-NaN
                Assert(cmp.Compare(negQNaN1, negInf) < 0, "-qNaN < -Inf");
                Assert(cmp.Compare(negSNaN1, negInf) < 0, "-sNaN < -Inf");
                Assert(cmp.Compare(negQNaN1, negOne) < 0, "-qNaN < -1");
                Assert(cmp.Compare(negQNaN1, negZero) < 0, "-qNaN < -0");
                Assert(cmp.Compare(negQNaN1, posZero) < 0, "-qNaN < +0");
                Assert(cmp.Compare(negQNaN1, posInf) < 0, "-qNaN < +Inf");

                // d.2: every non-NaN is above -NaN
                Assert(cmp.Compare(negInf, negQNaN1) > 0, "-Inf > -qNaN");
                Assert(cmp.Compare(posInf, negSNaN1) > 0, "+Inf > -sNaN");

                // d.3: +NaN is above every non-NaN
                Assert(cmp.Compare(posQNaN1, negInf) > 0, "+qNaN > -Inf");
                Assert(cmp.Compare(posSNaN1, negOne) > 0, "+sNaN > -1");
                Assert(cmp.Compare(posQNaN1, posZero) > 0, "+qNaN > +0");
                Assert(cmp.Compare(posQNaN1, posInf) > 0, "+qNaN > +Inf");

                // d.4: every non-NaN is below +NaN
                Assert(cmp.Compare(negInf, posQNaN1) < 0, "-Inf < +qNaN");
                Assert(cmp.Compare(posInf, posSNaN1) < 0, "+Inf < +sNaN");

                // d.5.i: negative sign orders below positive sign
                Assert(cmp.Compare(negQNaN1, posQNaN1) < 0, "-qNaN < +qNaN");
                Assert(cmp.Compare(negSNaN1, posSNaN1) < 0, "-sNaN < +sNaN");
                Assert(cmp.Compare(negQNaN1, posSNaN1) < 0, "-qNaN < +sNaN");
                Assert(cmp.Compare(negSNaN1, posQNaN1) < 0, "-sNaN < +qNaN");

                // d.5.ii: for +NaN, signaling < quiet; for -NaN, quiet < signaling.
                Assert(cmp.Compare(posSNaN1, posQNaN1) < 0, "+sNaN < +qNaN");
                Assert(cmp.Compare(posQNaN1, posSNaN1) > 0, "+qNaN > +sNaN");
                Assert(cmp.Compare(negQNaN1, negSNaN1) < 0, "-qNaN < -sNaN");
                Assert(cmp.Compare(negSNaN1, negQNaN1) > 0, "-sNaN > -qNaN");

                // Payload ordering within the same kind and sign (d.5.iii).
                Assert(cmp.Compare(posSNaN1, posSNaN2) < 0, "+sNaN1 < +sNaN2 (payload)");
                Assert(cmp.Compare(posQNaN1, posQNaN2) < 0, "+qNaN1 < +qNaN2 (payload)");
                Assert(cmp.Compare(negSNaN1, negSNaN2) > 0, "-sNaN1 > -sNaN2 (payload, reversed)");
                Assert(cmp.Compare(negQNaN1, negQNaN2) > 0, "-qNaN1 > -qNaN2 (payload, reversed)");

                // Bitwise-identical NaNs are equal under totalOrder.
                Assert(cmp.Compare(posQNaN1, posQNaN1) == 0, "+qNaN1 == +qNaN1");
                Assert(cmp.Compare(posSNaN1, posSNaN1) == 0, "+sNaN1 == +sNaN1");
                Assert(cmp.Compare(negQNaN1, negQNaN1) == 0, "-qNaN1 == -qNaN1");
                Assert(cmp.Compare(negSNaN1, negSNaN1) == 0, "-sNaN1 == -sNaN1");

                // Distinct NaNs are not equal.
                Assert(!eq.Equals(posQNaN1, posQNaN2), "+qNaN1 != +qNaN2 (payload)");
                Assert(!eq.Equals(posSNaN1, posQNaN1), "+sNaN1 != +qNaN1 (kind)");
                Assert(!eq.Equals(negQNaN1, negSNaN1), "-qNaN1 != -sNaN1 (kind)");
                Assert(!eq.Equals(posQNaN1, negQNaN1), "+qNaN1 != -qNaN1 (sign)");

                // -----------------------------------------------------------------
                // 5. Transitivity and totality over the full chain
                // -----------------------------------------------------------------
                Console.WriteLine("5. Transitivity & totality");
                for (int i = 0; i < canonical.Length; i++)
                    for (int j = i + 1; j < canonical.Length; j++)
                        for (int k = j + 1; k < canonical.Length; k++) {
                            int ij = cmp.Compare(canonical[i], canonical[j]);
                            int jk = cmp.Compare(canonical[j], canonical[k]);
                            int ik = cmp.Compare(canonical[i], canonical[k]);
                            Assert(ij < 0 && jk < 0 ? ik < 0 : true,
                                   $"transitivity [{canonical[i]}, {canonical[j]}, {canonical[k]}]");
                        }
                for (int i = 0; i < canonical.Length; i++)
                    for (int j = 0; j < canonical.Length; j++) {
                        int c = Math.Sign(cmp.Compare(canonical[i], canonical[j]));
                        Assert(c is -1 or 0 or 1, $"totality [{canonical[i]}, {canonical[j]}]");
                    }

                // -----------------------------------------------------------------
                // 6. IComparer<T> / IEqualityComparer<T> consistency
                // -----------------------------------------------------------------
                Console.WriteLine("6. Interface consistency");
                Assert(eq.Equals(negOne, negOne), "Equals(-1, -1)");
                Assert(!eq.Equals(negOne, posOne), "!Equals(-1, +1)");
                Assert(eq.Equals(posQNaN1, posQNaN1), "Equals(+qNaN1, +qNaN1)");
                Assert(eq.Equals(negSNaN1, negSNaN1), "Equals(-sNaN1, -sNaN1)");

                Assert(eq.GetHashCode(negOne) == eq.GetHashCode(negOne), "hash(-1) == hash(-1)");
                Assert(eq.GetHashCode(posQNaN1) == eq.GetHashCode(posQNaN1),
                       "hash(+qNaN1) == hash(+qNaN1)");
                Assert(eq.GetHashCode(negSNaN1) == eq.GetHashCode(negSNaN1),
                       "hash(-sNaN1) == hash(-sNaN1)");

                var set = new HashSet<Quadruple>(eq);
                foreach (var v in canonical) set.Add(v);
                Assert(set.Count == canonical.Length,
                       $"HashSet has {canonical.Length} distinct values (got {set.Count})");

                // -----------------------------------------------------------------
                // 7. Sorting the full chain
                // -----------------------------------------------------------------
                Console.WriteLine("7. Sorting");
                var shuffled = new[]
                {
            posQNaN2, negTwo, posSNaN1, posZero, negQNaN1, negInf,
            posOne, negSNaN2, negZero, posInf,
            negQNaN2, posTwo, negSNaN1, negOne, posQNaN1, posSNaN2
        };
                Array.Sort(shuffled, cmp);
                for (int i = 0; i < canonical.Length; i++) {
                    Assert(cmp.Compare(shuffled[i], canonical[i]) == 0,
                           $"sorted[{i}] == {canonical[i]} (got {shuffled[i]})");
                }

                // -----------------------------------------------------------------
                // 8. Parser round-trips and sign-preservation
                // -----------------------------------------------------------------
                Console.WriteLine("8. Parser round-trips");

                // Re-parsing the same literal must give an equal value.
                Assert(cmp.Compare(Quadruple.Parse("+NaN(Q1)"), posQNaN1) == 0,
                       "Parse(\"+NaN(Q1)\") == posQNaN1");
                Assert(cmp.Compare(Quadruple.Parse("+NaN(S1)"), posSNaN1) == 0,
                       "Parse(\"+NaN(S1)\") == posSNaN1");
                Assert(cmp.Compare(Quadruple.Parse("-NaN(Q1)"), negQNaN1) == 0,
                       "Parse(\"-NaN(Q1)\") == negQNaN1");
                Assert(cmp.Compare(Quadruple.Parse("-NaN(S1)"), negSNaN1) == 0,
                       "Parse(\"-NaN(S1)\") == negSNaN1");

                // Explicitly-signed bare NaN literals.
                Quadruple posQNaN0 = Quadruple.Parse("+NaN");
                Quadruple negQNaN0 = Quadruple.Parse("-NaN");
                Assert(Quadruple.IsNaN(posQNaN0), "+NaN parses to a NaN");
                Assert(!Quadruple.IsNegative(posQNaN0), "+NaN has positive sign");
                Assert(Quadruple.IsNaN(negQNaN0), "-NaN parses to a NaN");
                Assert(Quadruple.IsNegative(negQNaN0), "-NaN has negative sign");
                Assert(cmp.Compare(posQNaN0, negQNaN0) > 0, "+NaN > -NaN");
                Assert(cmp.Compare(posQNaN0, posQNaN1) < 0, "+NaN (default payload) < +NaN(Q1)");

                // The parser must agree with CopySign on sign-flip semantics.
                Assert(cmp.Compare(Quadruple.CopySign(posQNaN1, -Quadruple.One), negQNaN1) == 0,
                       "CopySign(+qNaN1, -1) == -qNaN1");
                Assert(cmp.Compare(Quadruple.CopySign(posSNaN1, -Quadruple.One), negSNaN1) == 0,
                       "CopySign(+sNaN1, -1) == -sNaN1");
                Assert(cmp.Compare(Quadruple.CopySign(negQNaN2, Quadruple.One), posQNaN2) == 0,
                       "CopySign(-qNaN2, +1) == +qNaN2");
                Assert(cmp.Compare(Quadruple.CopySign(negSNaN2, Quadruple.One), posSNaN2) == 0,
                       "CopySign(-sNaN2, +1) == +sNaN2");

                // -----------------------------------------------------------------
                // Summary
                // -----------------------------------------------------------------
                Console.WriteLine();
                Console.ForegroundColor = _failed == 0 ? ConsoleColor.Green : ConsoleColor.Red;
                Console.WriteLine($"=== {_passed} passed, {_failed} failed ===");
                Console.ResetColor();
            }
            {

                VerifyAgainstMathematica(
                    "Hypot(2.0Q, 3.0Q)",
                    Quadruple.Hypot(2, 3),
                    "3.6055512754639892931192212674704959462512965738452462127104530562271669482930104452046190820184907176735141820240635403760306782646978077051630171668927097577426905642741526332338303949623469447962732"
                );
                VerifyAgainstMathematica(
                    "Hypot(2 * Quadruple.Epsilon, -3 * Quadruple.Epsilon)",
                    Quadruple.Hypot(2 * Quadruple.Epsilon, -3 * Quadruple.Epsilon),
                    "2.3346575910742460648666845727864261102928952010012980249857794005876202541694614854643944638732349531984863529962503496445201678776978842033097992570692016487485157853929308212072493348652568872694954E-4965"
                );
                return 0;
            }
            {
                var x = Quadruple.PiOverTwo;
                Console.WriteLine(Quadruple.Parse("-6.4751751194380251109244389582276465524995693380346810096898843891970395401241193710176714912766499402558781414768481196765872198863825420466851100719726179830427927107513349344167346256384717402394485E-4966"));
                Console.WriteLine(x.ToString("G36"));
                Console.WriteLine((BigRational)x);
                Console.WriteLine();
                Console.WriteLine("SinCos.Sin/Cos");
                Console.WriteLine();

                VerifyAgainstMathematica(
                 "Sin(0)",
                 Quadruple.SinCos(0).Sin,
                 "0"
             );
                VerifyAgainstMathematica(
                    "Cos(0)",
                    Quadruple.SinCos(0).Cos,
                    "1"
                );


                VerifyAgainstMathematica(
                    "Sin(Quadruple.Pi)",
                    Quadruple.SinCos(Quadruple.Pi).Sin,
                    "8.6718101301237810247970440260433519687623233462565303417759357210804305024405832251835165593321742030164854972798621711503745900722378718736731952293430765605241765460717475367383543465448593745588219E-35"
                );
                VerifyAgainstMathematica(
                    "Cos(Quadruple.Pi)",
                    Quadruple.SinCos(Quadruple.Pi).Cos,
                    "-0.999999999999999999999999999999999999999999999999999999999999999999996239985453354128600525555154985137336409707247878560844139805039770182053151090938566469356979217900488313577944439248110260264467083"
                );

                VerifyAgainstMathematica(
                    "Sin(Quadruple.PiOverTwo)",
                    Quadruple.SinCos(Quadruple.PiOverTwo).Sin,
                    "0.999999999999999999999999999999999999999999999999999999999999999999999059996363338532150131388788746284334102426811969640211034951259942545071484354266249185146878285421363443538083496550683761707115373"
                );
                VerifyAgainstMathematica(
                    "Cos(Quadruple.PiOverTwo)",
                    Quadruple.SinCos(Quadruple.PiOverTwo).Cos,
                    "4.3359050650618905123985220130216759843811616731282651708879678605402193269868209896472055890893799222107503881377875894357012094128810566783674843543614930100556156928380881393864089708669233214455179E-35"
                );


                VerifyAgainstMathematica(
                    "Sin(0.5q)",
                    Quadruple.SinCos(0.5).Sin,
                    "0.47942553860420300027328793521557138808180336794060067518861661312553500028781483220963127468434826908613209108450571741781109374860994028278015396204619192460995729393228140053354633818805522859567014"
                );
                VerifyAgainstMathematica(
                    "Cos(0.5q)",
                    Quadruple.SinCos(0.5).Cos,
                    "0.87758256189037271611628158260382965199164519710974405299761086831595076327421394740579418408468225835547840059310905399341382797683328026679975612095022401558762915687859072347693931098961673967701441"
                );



                VerifyAgainstMathematica(
                   "Sin(-2^-16494)",
                   Quadruple.SinCos(-Quadruple.Epsilon).Sin,
                   "-6.4751751194380251109244389582276465524995693380346810096898843891970395401241193710176714912766499402558781414768481196765872198863825420466851100719726179830427927107513349344167346256384717402394485E-4966"
                );
                VerifyAgainstMathematica(
                   "Cos(-2^-16494)",
                   Quadruple.SinCos(-Quadruple.Epsilon).Cos,
                   "1"
                );

                VerifyAgainstMathematica(
                   "Sin(1.0q)",
                   Quadruple.SinCos(1.0).Sin,
                   "0.84147098480789650665250232163029899962256306079837106567275170999191040439123966894863974354305269585434903790792067429325911892099189888119341032772921240948079195582676660699990776401197840878273257"
                );
                VerifyAgainstMathematica(
                   "Cos(1.0q)",
                   Quadruple.SinCos(1.0).Cos,
                   "0.54030230586813971740093660744297660373231042061792222767009725538110039477447176451795185608718308934357173116003008909786063376002166345640651226541731858471797116447447949423311792455139325433594352"
                );

                VerifyAgainstMathematica(
                   "Sin(1.18973149535723176508575932662800702E+4932q=1189731<<...>>363968)",
                   Quadruple.SinCos(Quadruple.MaxValue).Sin,
                   "0.95191485407882048113632489293757294203297420250976463875139332618137512162045793913431205526834123348503247352731909125464224200828509068032778740296872558559594417733410200876422009802861743735201273"
                );
                VerifyAgainstMathematica(
                   "Cos(1.18973149535723176508575932662800702E+4932q=1189731<<...>>363968)",
                   Quadruple.SinCos(Quadruple.MaxValue).Cos,
                   "-0.30636271082509031488660022448440017919312519259050857080649519080770756682434732290755375700667347959975438682991870671207223337586354269355536125317367190130057536352937686702071667622453570128415116"
                );
                
                
                Console.WriteLine();
                Console.WriteLine("Direct Sin/Cos");
                Console.WriteLine();



                VerifyAgainstMathematica(
                   "Sin(Quadruple.Pi)",
                   Quadruple.Sin(Quadruple.Pi),
                   "8.6718101301237810247970440260433519687623233462565303417759357210804305024405832251835165593321742030164854972798621711503745900722378718736731952293430765605241765460717475367383543465448593745588219E-35"
               );
                VerifyAgainstMathematica(
                    "Cos(Quadruple.Pi)",
                    Quadruple.Cos(Quadruple.Pi),
                    "-0.999999999999999999999999999999999999999999999999999999999999999999996239985453354128600525555154985137336409707247878560844139805039770182053151090938566469356979217900488313577944439248110260264467083"
                );

                VerifyAgainstMathematica(
                    "Sin(Quadruple.PiOverTwo)",
                    Quadruple.Sin(Quadruple.PiOverTwo),
                    "0.999999999999999999999999999999999999999999999999999999999999999999999059996363338532150131388788746284334102426811969640211034951259942545071484354266249185146878285421363443538083496550683761707115373"
                );
                VerifyAgainstMathematica(
                    "Cos(Quadruple.PiOverTwo)",
                    Quadruple.Cos(Quadruple.PiOverTwo),
                    "4.3359050650618905123985220130216759843811616731282651708879678605402193269868209896472055890893799222107503881377875894357012094128810566783674843543614930100556156928380881393864089708669233214455179E-35"
                );


                VerifyAgainstMathematica(
                    "Sin(0.5q)",
                    Quadruple.Sin(0.5),
                    "0.47942553860420300027328793521557138808180336794060067518861661312553500028781483220963127468434826908613209108450571741781109374860994028278015396204619192460995729393228140053354633818805522859567014"
                );
                VerifyAgainstMathematica(
                    "Cos(0.5q)",
                    Quadruple.Cos(0.5),
                    "0.87758256189037271611628158260382965199164519710974405299761086831595076327421394740579418408468225835547840059310905399341382797683328026679975612095022401558762915687859072347693931098961673967701441"
                );



                VerifyAgainstMathematica(
                   "Sin(-2^-16494)",
                   Quadruple.Sin(-Quadruple.Epsilon),
                   "-6.4751751194380251109244389582276465524995693380346810096898843891970395401241193710176714912766499402558781414768481196765872198863825420466851100719726179830427927107513349344167346256384717402394485E-4966"
                );
                VerifyAgainstMathematica(
                   "Cos(-2^-16494)",
                   Quadruple.Cos(-Quadruple.Epsilon),
                   "1"
                );

                VerifyAgainstMathematica(
                   "Sin(1.0q)",
                   Quadruple.Sin(1.0),
                   "0.84147098480789650665250232163029899962256306079837106567275170999191040439123966894863974354305269585434903790792067429325911892099189888119341032772921240948079195582676660699990776401197840878273257"
                );
                VerifyAgainstMathematica(
                   "Cos(1.0q)",
                   Quadruple.Cos(1.0),
                   "0.54030230586813971740093660744297660373231042061792222767009725538110039477447176451795185608718308934357173116003008909786063376002166345640651226541731858471797116447447949423311792455139325433594352"
                );

                VerifyAgainstMathematica(
                   "Sin(1.18973149535723176508575932662800702E+4932q=1189731<<...>>363968)",
                   Quadruple.Sin(Quadruple.MaxValue),
                   "0.95191485407882048113632489293757294203297420250976463875139332618137512162045793913431205526834123348503247352731909125464224200828509068032778740296872558559594417733410200876422009802861743735201273"
                );
                VerifyAgainstMathematica(
                   "Cos(1.18973149535723176508575932662800702E+4932q=1189731<<...>>363968)",
                   Quadruple.Cos(Quadruple.MaxValue),
                   "-0.30636271082509031488660022448440017919312519259050857080649519080770756682434732290755375700667347959975438682991870671207223337586354269355536125317367190130057536352937686702071667622453570128415116"
                );
                return 0;

            }
            {
                var c = new TotalOrderIeee754Comparer2<Decimal128Bid>();

                Console.WriteLine(c.Compare((Decimal128Bid)42.0m, (Decimal128Bid)42m));
                Console.WriteLine(c.Compare((Decimal128Bid)42m, (Decimal128Bid)42.0m));
                return 0;

            }
            {
                
                Console.WriteLine($"{Quadruple.Log(2):G36}");
                Console.WriteLine($"UInt128Bits: {BitConverter.QuadrupleToUInt128Bits(Quadruple.Log(2)):X32}");



            }
            {
                Console.WriteLine("==== Quadruple Log(x) Tests ====");

                // ------------------------------------------------------------
                // 1. Special values
                // ------------------------------------------------------------
                TestLog("Log(NaN)", Quadruple.NaN);
                TestLog("Log(+Infinity)", Quadruple.PositiveInfinity);
                TestLog("Log(-Infinity)", Quadruple.NegativeInfinity);
                TestLog("Log(+0)", Quadruple.PositiveZero);
                TestLog("Log(-0)", Quadruple.NegativeZero);

                // ------------------------------------------------------------
                // 2. Negative finite values → must be NaN
                // ------------------------------------------------------------
                TestLog("Log(-1)", -1.0);
                TestLog("Log(-0.5)", -0.5);

                // ------------------------------------------------------------
                // 3. Subnormal values
                // ------------------------------------------------------------
                Quadruple tiny = BitConverter.UInt128BitsToQuadruple(
                    (UInt128) 0x0000000000000001UL << 64 | 0x0000000000000000UL);
                TestLog("Log(tiny subnormal)", tiny);

                // ------------------------------------------------------------
                // 4. Normal values
                // ------------------------------------------------------------
                TestLog("Log(1)", 1.0);
                TestLog("Log(2)", 2.0);
                TestLog("Log(10)", 10.0);
                TestLog("Log(0.5)", 0.5);
                TestLog("Log(0.1)", 0.1);

                // ------------------------------------------------------------
                // 5. Random values
                // ------------------------------------------------------------
                var rng = new Random(12345);
                for (int i = 0; i < 5; i++) {
                    double d = rng.NextDouble() * 100.0;
                    Quadruple q = d; // implicit conversion
                    TestLog($"Log(random {d})", q);
                }

                // ------------------------------------------------------------
                // 6. High‑precision verification using Mathematica
                // ------------------------------------------------------------
                // Paste your Mathematica results here:
                // N[Log[2], 200]
                VerifyAgainstMathematica(
                    "Log(2)",
                    Quadruple.Log(2.0),
                    "0.69314718055994530941723212145817656807550013436025525412068000949339362196969471560586332699641868754200148102057068573368552023575813055703267075163507596193072757082837143519030703862389167347112335"
                );

                // N[Log[10], 200]
                VerifyAgainstMathematica(
                    "Log(10)",
                    Quadruple.Log(10.0),
                    "2.3025850929940456840179914546843642076011014886287729760333279009675726096773524802359972050895982983419677840422862486334095254650828067566662873690987816894829072083255546808437998948262331985283935"
                );
                // N[Log[4153837486827862102824397063376077/41538374868278621028243970633760768], 200]
                VerifyAgainstMathematica(
                    "Log(0.1q=4153837486827862102824397063376077/41538374868278621028243970633760768)",
                    Quadruple.Log(Quadruple.Parse("0.1")),
                    "-2.3025850929940456840179914546843641594528528789478766496338793363443908053517333646704753652583976428153075575198446405543537886690756540178214839463095814559471881177097639943840568347657965875825098"
                );
                // N[Log[9290479413410269547711582135067345/81129638414606681695789005144064], 200]
                VerifyAgainstMathematica(
                    "Log(114.514q=9290479413410269547711582135067345/81129638414606681695789005144064)",
                    Quadruple.Log(Quadruple.Parse("114.514")),
                    "4.7406970862621944647029536135153566062819806957416593090297720392187042266985546148933920336590056973975396067158984205459881533168500196273221266806064882167002708062930322052153615804749974654378977"
                );


                Console.WriteLine("==== Tests Completed ====");
            }
            {
                VerifyConstant("PiOverTwo", Quadruple.PiOverTwo,
                    "1.5707963267948966192313216916397514420985846996875529104874722961539082031431044993140174126710585339910740432566411533235469223047752911158626797040642405587251420513509692605527798223114744774651910");

                VerifyConstant("Log10OfE", Quadruple.Log10OfE,
                    "0.43429448190325182765112891891660508229439700580366656611445378316586464920887077472922494933843174831870610674476630373364167928715896390656922106466281226585212708656867032959337086965882668833116361");

                VerifyConstant("Log2OfE", Quadruple.Log2OfE,
                    "1.4426950408889634073599246810018921374266459541529859341354494069311092191811850798855266228935063444969975183096525442555931016871683596427206621582234793362745373698847184936307013876635320155338943");

                VerifyConstant("LogOf10", Quadruple.LogOf10,
                    "2.3025850929940456840179914546843642076011014886287729760333279009675726096773524802359972050895982983419677840422862486334095254650828067566662873690987816894829072083255546808437998948262331985283935");

                VerifyConstant("LogOf2", Quadruple.LogOf2,
                    "0.69314718055994530941723212145817656807550013436025525412068000949339362196969471560586332699641868754200148102057068573368552023575813055703267075163507596193072757082837143519030703862389167347112335");

                static void VerifyConstant(string name, Quadruple expected, string decimalString) {
                    Quadruple parsed = (Quadruple)BigRational.Parse(decimalString, null);

                    bool equal = expected == parsed;

                    Console.WriteLine($"{name}: {(equal ? "OK" : "MISMATCH")}");

                    if (!equal) {
                        Console.WriteLine($"Expected: {expected}");
                        Console.WriteLine($"Parsed:   {parsed}");
                    }
                }

            }


            {
                Console.WriteLine("=== Exp small argument tests ===");

                var halfUlp = Quadruple.ScaleB(Quadruple.One, -113);
                var ulp = Quadruple.ScaleB(Quadruple.One, -112);

                var a = Quadruple.Exp(halfUlp);
                var b = Quadruple.Exp(-halfUlp);

                Console.WriteLine("Exp(+2^-113):");
                Console.WriteLine(a.ToString("G36"));

                Console.WriteLine("Exp(-2^-113):");
                Console.WriteLine(b.ToString("G36"));

                Console.WriteLine("Exp(+2^-112)-1:");
                Console.WriteLine((Quadruple.Exp(ulp) - Quadruple.One).ToString("G36"));

                Console.WriteLine("Exp(-2^-112)-1:");
                Console.WriteLine((Quadruple.Exp(-ulp) - Quadruple.One).ToString("G36"));

                Console.WriteLine();
            }
            {
                Console.WriteLine("=== Exp small argument tests ===");

                var minSubnormal = Quadruple.Epsilon;
                var halfUlp = Quadruple.ScaleB(Quadruple.One, -113);
                var ulp = Quadruple.ScaleB(Quadruple.One, -112);

                Console.WriteLine($"minSubnormal = {minSubnormal:G36}");
                Console.WriteLine($"halfUlp      = {halfUlp:G36}");
                Console.WriteLine($"ulp          = {ulp:G36}");
                Console.WriteLine();

                // Below representable resolution around 1
                Console.WriteLine("Exp(minSubnormal):");
                Console.WriteLine(Quadruple.Exp(minSubnormal).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-minSubnormal).ToString("G36"));
                Console.WriteLine();

                // Exactly half an ulp
                Console.WriteLine("Exp(±2^-113) - 1:");
                Console.WriteLine((Quadruple.Exp(halfUlp) - Quadruple.One).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(-halfUlp) - Quadruple.One).ToString("G36"));
                Console.WriteLine();

                // One ulp
                Console.WriteLine("Exp(±2^-112) - 1:");
                Console.WriteLine((Quadruple.Exp(ulp) - Quadruple.One).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(-ulp) - Quadruple.One).ToString("G36"));
                Console.WriteLine();

                // Neighbor checks
                Console.WriteLine("Neighbor checks:");
                Console.WriteLine((Quadruple.Exp(ulp) == Quadruple.One).ToString());
                Console.WriteLine((Quadruple.Exp(-ulp) == Quadruple.One).ToString());

                Console.WriteLine();

            }

            {
                var x = Quadruple.ScaleB(Quadruple.One, -112);

                Console.WriteLine((Quadruple.Exp(-x) == Quadruple.One).ToString());
                Console.WriteLine((Quadruple.Exp(-x) - 1).ToString("G36"));
                Console.WriteLine();
            }

            {
                var x = Quadruple.ScaleB(Quadruple.One, -112);

                Console.WriteLine((Quadruple.Exp(x) == Quadruple.One).ToString());
                Console.WriteLine((Quadruple.Exp(x) - 1).ToString("G36"));
                Console.WriteLine();
            }
            {
                var u = Quadruple.ScaleB(Quadruple.One, -113);

                Console.WriteLine((Quadruple.One - u).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(-u) - 1).ToString("G36"));

                Console.WriteLine();
            }

            {
                var u = Quadruple.ScaleB(Quadruple.One, -113);

                Console.WriteLine((Quadruple.One + u).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(u) - 1).ToString("G36"));


                Console.WriteLine();
            }

            {
                var u = Quadruple.ScaleB(1, -112);

                Console.WriteLine(u.ToString("G36"));
                Console.WriteLine((1 + u).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(u) - 1).ToString("G36"));
                Console.WriteLine();
            }

            {
                Console.WriteLine(Quadruple.Epsilon.ToString("G36"));
                Console.WriteLine((Quadruple.One + Quadruple.Epsilon).ToString("G36"));
                Console.WriteLine((Quadruple.Exp(Quadruple.Epsilon) - 1).ToString("G36"));
                Console.WriteLine();

            }
            {
                Console.WriteLine(Quadruple.Exp(3).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-1).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(1).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-0.0).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(Quadruple.PositiveInfinity).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(Quadruple.NegativeInfinity).ToString("G36"));

                Console.WriteLine(Quadruple.Exp(Quadruple.Epsilon).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-Quadruple.Epsilon).ToString("G36"));


                Console.WriteLine(Quadruple.Exp(1e4).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(1e-4).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-1e4).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(-1e-4).ToString("G36"));


                Console.WriteLine(Quadruple.Exp(Quadruple.MinValue).ToString("G36"));
                Console.WriteLine(Quadruple.Exp(Quadruple.MaxValue).ToString("G36"));

                Console.WriteLine();


            }
            {
                Quadruple p = Quadruple.Parse("0.01");
                Quadruple q = 1 - p;

                const int n = 10;

                // Compute Perron right eigenvector by iteration
                Quadruple[,] T = new Quadruple[n, n];

                for (int i = 0; i < n; i++) {
                    T[i, 0] = p;
                    if (i < 9)
                        T[i, i + 1] = q;
                }

                Quadruple[] r = new Quadruple[n];
                for (int i = 0; i < n; i++)
                    r[i] = 1;

                Quadruple lambda = 0;

                for (int iter = 0; iter < 10000; iter++) {
                    Quadruple[] nr = new Quadruple[n];

                    for (int i = 0; i < n; i++) {
                        for (int j = 0; j < n; j++)
                            nr[i] += T[i, j] * r[j];
                    }

                    lambda = nr[0];

                    Quadruple scale = nr[0];
                    for (int i = 0; i < n; i++)
                        nr[i] /= scale;

                    r = nr;
                }

                Console.WriteLine($"lambda = {lambda}");

                Console.WriteLine("P_k:");
                for (int k = 0; k < n; k++) {
                    Quadruple Pk = p * r[0] / (lambda * r[k]);
                    Console.WriteLine($"P_{k} = {Pk}");
                }
                Console.WriteLine("Q_k:");
                var d = ComputeQ(10, p, lambda);
                for (int k = 0; k < n; k++) {
                    Quadruple Qk = d[k];
                    Console.WriteLine($"Q_{k} = {Qk}");
                }
            }


            {

                Console.WriteLine((-Quadruple.One).ToString("G36"));
                Console.WriteLine(Quadruple.Acos(-Quadruple.One).ToString("G36"));


                Console.WriteLine((-Quadruple.BitDecrement(Quadruple.One)).ToString("G36"));
                Console.WriteLine(Quadruple.Acos(-Quadruple.BitDecrement(Quadruple.One)).ToString("G36"));
                Console.WriteLine(Quadruple.BitDecrement(Quadruple.One).ToString("G36"));
                Console.WriteLine(Quadruple.Acos(Quadruple.BitDecrement(Quadruple.One)).ToString("G36"));
                Console.WriteLine(Quadruple.BitIncrement(Quadruple.One).ToString("G36"));
                Console.WriteLine(Quadruple.Acos(Quadruple.BitIncrement(Quadruple.One)).ToString("G36"));
                Console.WriteLine(((double)Quadruple.Acos(1)).ToString("G17"));
                Console.WriteLine(Quadruple.Acos(1).ToString("G36"));
                Console.WriteLine(((double)Quadruple.Acos(0)).ToString("G17"));
                Console.WriteLine(Quadruple.Acos(0).ToString("G36"));

                Console.WriteLine(Quadruple.Parse("-NaN(S)",
                                    NumberStyles.Float | NumberStyles.HexFloat).ToString("G36"));
            }
            {
                // G<prec> in [0, 1)
                AssertAlways.Equal("0.284444444444444440743701029025138323",
                    Quadruple.Parse("0X1.23456789abcdef0123456789p-2",
                                    NumberStyles.Float | NumberStyles.HexFloat).ToString("G36"));
                AssertAlways.Equal("0.05", Quadruple.Parse("0.05").ToString("G1"));
                AssertAlways.Equal("0.1", Quadruple.Parse("0.1").ToString("G1"));
                AssertAlways.Equal("0.013", Quadruple.Parse("0.0125").ToString("G2"));

                // N / C / P in [0, 1)
                AssertAlways.Equal("0.28", Quadruple.Parse("0.2844").ToString("N2"));
                AssertAlways.Equal("$0.28", Quadruple.Parse("0.2844").ToString("C2",
                                            CultureInfo.GetCultureInfo("en-US")));
                AssertAlways.Equal("28.44%", Quadruple.Parse("0.2844").ToString("P2",
                                            CultureInfo.GetCultureInfo("en-US")));

                // Boundary: decExp just below zero
                AssertAlways.Equal("0.1", Quadruple.Parse("0.1").ToString("F1"));
                AssertAlways.Equal("0.1", Quadruple.Parse("0.1").ToString("N1"));
                AssertAlways.Equal("0.10", Quadruple.Parse("0.1").ToString("F2"));
            }
            {
                Console.WriteLine($@"{Quadruple.Parse("0X1.23456789abcdef0123456789p-2", System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.HexFloat):G36}");
                Console.WriteLine($@"{Quadruple.Parse("0X1.23456789abcdef0123456789p-2", System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.HexFloat):G}");
                Console.WriteLine($@"{(double)Quadruple.Parse("0X1.23456789abcdef0123456789p-2", System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.HexFloat):G36}");
                Console.WriteLine($@"{(double)Quadruple.Parse("0X1.23456789abcdef0123456789p-2", System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.HexFloat):G}");
                Console.WriteLine();

                Console.WriteLine($@"{(BigRational)Quadruple.MinValue}");
                Console.WriteLine($@"{(BigRational)Quadruple.Epsilon}");
                Console.WriteLine($@"{(BigRational)Quadruple.Epsilon}");
                Console.WriteLine($@"{Quadruple.NegativeInfinity:E1}");
                Console.WriteLine($@"{Quadruple.NaN:E1}");

                Console.WriteLine($@"{Quadruple.Epsilon:E1}");
                Console.WriteLine($@"{Quadruple.Pi:E1}");
                Console.WriteLine($@"{(Quadruple)1234.5:42_70_00}");
                Console.WriteLine($@"{(Quadruple)123.45:42_70_00}");
                Console.WriteLine($@"{(Quadruple)12.345:42_70_00}");

                Console.WriteLine($@"{(Quadruple)12.345:B}");


                Console.WriteLine($@"{((double)(Quadruple)3.0).ToString()}");
                Console.WriteLine($@"{((BigRational)3.0).ToString()}");
                Console.WriteLine($@"{((BigRational)(Quadruple)3.0).ToString()}");

                {
                    var sfas0 = Quadruple.Cbrt(3.0);
                    var sfas1 = QuadrupleLib.Float128<QuadrupleLib.Accelerators.DefaultAccelerator>.Cbrt(3.0);
                    Console.WriteLine($@"{sfas0:G100}");
                    Console.WriteLine($@"{((BigRational)sfas0).ToString()}");
                    Console.WriteLine($@"{sfas1}");
                }
                {
                    var sfas0 = Quadruple.Sqrt(3.0);
                    var sfas1 = QuadrupleLib.Float128<QuadrupleLib.Accelerators.DefaultAccelerator>.Sqrt(3.0);
                    Console.WriteLine($@"{sfas0:G1000}");
                    Console.WriteLine($@"{((BigRational)sfas0).ToString()}");
                    Console.WriteLine($@"{sfas1}");
                }
                Console.WriteLine();
                {
                    Console.WriteLine(BigRational.Parse("1.7320508075688772935274463415058723221530973174112081324397189303443238272972592994847218506038188934326171875", null));
                }

                return 0;
            }
            {
                {
                    var asdfas = checked(unchecked((System.UInt128)System.Int128.MinValue) / unchecked(-System.UInt128.One));

                    Console.WriteLine($@"{asdfas}");
                }

                {
                    var asdfas = checked(unchecked((Unsigned<System.Int128>)System.Int128.MinValue) / unchecked(-Unsigned<System.Int128>.One));

                    Console.WriteLine($@"{asdfas}");
                }

                {
                    var asdfas = checked(unchecked((System.UInt128)System.Int128.MinValue) / System.UInt128.One);

                    Console.WriteLine($@"{asdfas}");
                }

                {
                    var asdfas = checked(unchecked((Unsigned<System.Int128>)System.Int128.MinValue) / Unsigned<System.Int128>.One);

                    Console.WriteLine($@"{asdfas}");
                }


                return 0;

            }
            {
                var sdfa = BitPatternGenerator.GenerateQuadruple().LongCount();
                Console.WriteLine(sdfa);
                var asd = new BinaryFloatingPointIeee754ArithmeticTests();
                asd.QuadrupleIsOddIntegerTest();
                Console.WriteLine("Done.");
                return 0;
            }
            {
                var t = BinaryFloatingPointIeee754Arithmetic.IsInteger<Single>(1);

                var sdfa = BitPatternGenerator.GenerateSingle().LongCount();
                Console.WriteLine(sdfa);
                var asd = new BinaryFloatingPointIeee754ArithmeticTests();
                asd.SingleIsIntegerTest();
                Console.WriteLine("Done.");
                return 0;
            }
            {
                var sdfasd = Math.BigMul(4UL, 2UL);

            }
            {

                var sfads = BigRational.Parse("21827907538883637012326748457700300661358717434156476363", null);

                var sdfas = (Quadruple)sfads;
                var sdfa = BFloat16.CreateChecked(sdfas);


                var sdfadsf = (BigRational)sdfas;
                Console.WriteLine(sdfadsf.ToString());
                return 0;
            }
            if (false) {


                var sdfasf = Vector128<Bits24>.IsSupported;
                Console.WriteLine(sdfasf);
                Console.WriteLine("Done.");
                return 0;
            }
            {
#pragma warning disable UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var aa = DoubleArithmetic.BigMulUnsigned<nuint>(unchecked((nuint)0x8000000000000000), 2, out var hi);
#pragma warning restore UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                Console.WriteLine(aa);
                Console.WriteLine(hi);
                return 0;
            }
            {
                Console.WriteLine($"Hypot(-3.0, -4) = {Decimal128Bid.Hypot(-3.0M, -4M)}");

                Console.WriteLine($"Hypot(1E-6176, -1E-6176) = {Decimal128Bid.Hypot(Decimal128Bid.Epsilon, -Decimal128Bid.Epsilon)}");

                Console.WriteLine($"Hypot(-3E-6176, 2E-6176) = {Decimal128Bid.Hypot(-3 * Decimal128Bid.Epsilon, 2 * Decimal128Bid.Epsilon)}");

                Console.WriteLine($"Sqrt(NaN) = {Decimal128Bid.Sqrt(Decimal128Bid.NaN)}");

                Console.WriteLine($"Sqrt(∞) = {Decimal128Bid.Sqrt(Decimal128Bid.PositiveInfinity)}");


                Console.WriteLine($"Hypot(Decimal128Bid.MaxValue, 42) = {Decimal128Bid.Hypot(Decimal128Bid.MaxValue, 42)}");

                Console.WriteLine($"Sqrt(10) = {Decimal128Bid.Sqrt(10)}");

                Console.WriteLine($"Sqrt(0.1M) = {Decimal128Bid.Sqrt(0.1M)}");
                Console.WriteLine($"Sqrt(6.25E-6174) = {Decimal128Bid.Sqrt(625 * Decimal128Bid.Epsilon)}");
                Console.WriteLine($"Sqrt(6.00E-6174) = {Decimal128Bid.Sqrt(600 * Decimal128Bid.Epsilon)}");

                Console.WriteLine($"Sqrt(2E-6176) = {Decimal128Bid.Sqrt(2 * Decimal128Bid.Epsilon)}");
                Console.WriteLine($"Sqrt(3E-6176) = {Decimal128Bid.Sqrt(3 * Decimal128Bid.Epsilon)}");
                Console.WriteLine($"Sqrt(-0) = {Decimal128Bid.Sqrt(-Decimal128Bid.Zero)}");
                Console.WriteLine($"Sqrt(-1E-6176) = {Decimal128Bid.Sqrt(-Decimal128Bid.Epsilon)}");
                Console.WriteLine($"Sqrt(10) = {Decimal128Bid.Sqrt(10)}");
                Console.WriteLine($"Round(Sqrt(2), 14) = {Decimal128Bid.Round(Decimal128Bid.Sqrt(2), 14)}");

                Console.WriteLine($"Round(123.45M, 1) = {Decimal128Bid.Round(123.45M, 1)}");

            }

            {
                Console.WriteLine($"Exp(0) = {Decimal128Bid.Exp(0)}");
                Console.WriteLine($"Exp(1) = {Decimal128Bid.Exp(1)}");
                Console.WriteLine($"Exp(7) = {Decimal128Bid.Exp(7)}");
                Console.WriteLine($"Exp(0.5) = {Decimal128Bid.Exp(0.5)}");
                Console.WriteLine($"Exp(-123.456M) = {Decimal128Bid.Exp(-123.456M)}");
                Console.WriteLine($"Exp(0.1M) = {Decimal128Bid.Exp(0.1M)}");
                Console.WriteLine($"Exp(-0.1M) = {Decimal128Bid.Exp(-0.1M)}");
                Console.WriteLine($"Exp(7) = {Decimal128Bid.Exp(7)}");
                Console.WriteLine($"Exp(3.5) = {Decimal128Bid.Exp(3.5)}");
                Console.WriteLine($"Exp(-8) = {Decimal128Bid.Exp(-8)}");
                Console.WriteLine($"Exp(-8.5) = {Decimal128Bid.Exp(-8.5)}");
                Console.WriteLine($"Exp(-8.000M) = {Decimal128Bid.Exp(-8.000M)}");
            }

            {
                Console.WriteLine($"E = {Decimal128Bid.E}");
                Console.WriteLine($"Exp(0) = {Decimal128Bid.Exp(0)}");
                Console.WriteLine($"Exp(1) = {Decimal128Bid.Exp(1)}");
                Console.WriteLine($"Exp(7) = {Decimal128Bid.Exp(7)}");
                Console.WriteLine($"Exp(0.5) = {Decimal128Bid.Exp(0.5)}");
                Console.WriteLine($"Exp(-123.456M) = {Decimal128Bid.Exp(-123.456M)}");
                Console.WriteLine($"Exp(0.1M) = {Decimal128Bid.Exp(0.1M)}");
                Console.WriteLine($"Exp(-0.1M) = {Decimal128Bid.Exp(-0.1M)}");
                Console.WriteLine($"Exp(7) = {Decimal128Bid.Exp(7)}");
                Console.WriteLine($"Exp(3.5) = {Decimal128Bid.Exp(3.5)}");
                Console.WriteLine($"Exp(-8) = {Decimal128Bid.Exp(-8)}");
                Console.WriteLine($"Exp(-8.5) = {Decimal128Bid.Exp(-8.5)}");
                Console.WriteLine($"Exp(-8.000M) = {Decimal128Bid.Exp(-8.000M)}");
            }
            {
                Console.WriteLine($"Exp2(7) = {Decimal128Bid.Exp2(7)}");
                Console.WriteLine($"Exp2(0.5) = {Decimal128Bid.Exp2(0.5)}");
                Console.WriteLine($"Exp2(-123.456M) = {Decimal128Bid.Exp2(-123.456M)}");
                Console.WriteLine($"Exp2(0.1M) = {Decimal128Bid.Exp2(0.1M)}");
                Console.WriteLine($"Exp2(-0.1M) = {Decimal128Bid.Exp2(-0.1M)}");
                Console.WriteLine($"Exp2(7) = {Decimal128Bid.Exp2(7)}");
                Console.WriteLine($"Exp2(3.5) = {Decimal128Bid.Exp2(3.5)}");
                Console.WriteLine($"Exp2(-8) = {Decimal128Bid.Exp2(-8)}");
                Console.WriteLine($"Exp2(-8.5) = {Decimal128Bid.Exp2(-8.5)}");
                Console.WriteLine($"Exp2(-8.000M) = {Decimal128Bid.Exp2(-8.000M)}");
            }
            {
                Console.WriteLine($"Exp10(0.5) = {Decimal128Bid.Exp10(0.5)}");
                Console.WriteLine($"Exp10(-123.456M) = {Decimal128Bid.Exp10(-123.456M)}");
                Console.WriteLine($"Exp10(0.1M) = {Decimal128Bid.Exp10(0.1M)}");
                Console.WriteLine($"Exp10(-0.1M) = {Decimal128Bid.Exp10(-0.1M)}");
                Console.WriteLine($"Exp10(7) = {Decimal128Bid.Exp10(7)}");
                Console.WriteLine($"Exp10(3.5) = {Decimal128Bid.Exp10(3.5)}");
                Console.WriteLine($"Exp10(-8) = {Decimal128Bid.Exp10(-8)}");
                Console.WriteLine($"Exp10(-8.5) = {Decimal128Bid.Exp10(-8.5)}");
                Console.WriteLine($"Exp10(-8.000M) = {Decimal128Bid.Exp10(-8.000M)}");
            }

            {
                Decimal128Bid.IsZero(Decimal128Bid.Parse("+sNaN"));
                Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+sNaN"));
                Console.WriteLine($"IsSignalingNaN(+sNaN) = {Decimal128Bid.IsSignalingNaN(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsQuietNaN(+sNaN) = {Decimal128Bid.IsQuietNaN(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsNegative(+sNaN) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("+sNaN"))}");
                Console.WriteLine($"IsSignalingNaN(+qNaN(0x4243)) = {Decimal128Bid.IsSignalingNaN(Decimal128Bid.Parse("+qNaN(0x4243)"))}");
                Console.WriteLine($"IsQuietNaN(+qNaN(0x4243)) = {Decimal128Bid.IsQuietNaN(Decimal128Bid.Parse("+qNaN(0x4243)"))}");
                Console.WriteLine($"IsNegative(+qNaN(0x4243)) = {Decimal128Bid.IsNegative(Decimal128Bid.Parse("+qNaN(0x4243)"))}");

                Console.WriteLine($"Decimal128Bid(1919810) = {(Decimal128Bid)1919810}");
                Console.WriteLine($"CoarsestCohort(1919810) = {Decimal128Bid.ToCoarsestCohort(1919810)}");
                Console.WriteLine($"FinestCohort(1919810) = {Decimal128Bid.ToFinestCohort(1919810)}");

            }
            {
                var comparer = new TotalOrderIeee754Comparer2<Decimal128Bid>();

                Console.WriteLine($"TotalOrderIeee754(+sNaN, +qNaN(0x4243)) = {int.Sign(comparer.Compare(
                    Decimal128Bid.Parse("+sNaN"), Decimal128Bid.Parse("+qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(+sNaN, +qNaN(0x4243)) = {int.Sign(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+sNaN")).CompareTo(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("+qNaN(0x4243)"))))}");

                Console.WriteLine($"TotalOrderIeee754(sNaN, qNaN(0x4243)) = {int.Sign(comparer.Compare(
                    Decimal128Bid.Parse("sNaN"), Decimal128Bid.Parse("qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(sNaN, qNaN(0x4243)) = {int.Sign(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("sNaN")).CompareTo(
                    Decimal128Bid.TotalOrderIeee754_192BitsKeySelector(Decimal128Bid.Parse("qNaN(0x4243)"))))}");

                Console.WriteLine($"TotalOrderIeee754(-sNaN, -qNaN(0x4243)) = {int.Sign(comparer.Compare(Decimal128Bid.Parse("-sNaN"), Decimal128Bid.Parse("-qNaN(0x4243)")))}");
                Console.WriteLine($"TotalOrderIeee754(+sNaN(10), +sNaN(0X10)) = {int.Sign(comparer.Compare(Decimal128Bid.Parse("+sNaN(10)"), Decimal128Bid.Parse("+sNaN(0X10)")))}");

            }
            {
                Console.WriteLine($"{Decimal128Bid.Parse("7.102030405E13")}");
                Console.WriteLine($"{Decimal128Bid.Parse("sNaN")}");
                Console.WriteLine($"{Decimal128Bid.Parse("qNaN(0x4243)")}");
                Console.WriteLine($"{Decimal128Bid.Parse("Inf")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-Inf")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-0E-444")}");
                Console.WriteLine($"{Decimal128Bid.Parse("-1000E-444")}");
                Console.WriteLine();

            }
            {
                Console.WriteLine($"{BigRational.Parse("7.102030405E13", null)}");

                Console.WriteLine($"{BigRational.Parse(".1", null)}");

                var sdfs = BigRational.Parse("423123.23423423567657657657567657657657657991112E-32", null);

                Console.WriteLine($"{sdfs}");
                Console.WriteLine();

            }
            {
                Console.WriteLine($"default(Decimal128) = {default(Decimal128)}");
                Console.WriteLine($"Decimal128Bid(Double.MaxValue) = {(Decimal128Bid)double.MaxValue}");




                Console.WriteLine($"Decimal128(-0E-3M) = {(Decimal128Bid)(-0E-3M)}");
                Console.WriteLine($"Quantum(Decimal128(-0E-900M)) = {Decimal128Bid.Quantum((Decimal128Bid)(-0E-900M))}");

                Console.WriteLine($"Zero - 0E-3 = {Decimal128Bid.Zero - Decimal128Bid.Scale10(0, -3)}");
                Console.WriteLine($"Zero + AdditiveIdentity = {Decimal128Bid.Zero + Decimal128Bid.AdditiveIdentity}");

                Console.WriteLine($"Zero = {Decimal128Bid.Zero}");
                Console.WriteLine($"NegativeZero = {Decimal128Bid.NegativeZero}");
                Console.WriteLine($"AdditiveIdentity = {Decimal128Bid.AdditiveIdentity}");
                Console.WriteLine($"BitIncrement({-Decimal128Bid.Epsilon}) = {Decimal128Bid.BitIncrement(-Decimal128Bid.Epsilon)}");

                Console.WriteLine($"BitDecrement({Decimal128Bid.Epsilon}) = {Decimal128Bid.BitDecrement(Decimal128Bid.Epsilon)}");
                Console.WriteLine($"BitDecrement(-1) = {Decimal128Bid.BitDecrement(-1)}");
                Console.WriteLine($"BitDecrement({Decimal128Bid.MinValue}) = {Decimal128Bid.BitDecrement(Decimal128Bid.MinValue)}");

                Console.WriteLine($"AtanPi({Decimal128Bid.MaxValue}) = {Decimal128Bid.AtanPi(Decimal128Bid.MaxValue)}");
                Console.WriteLine($"AtanPi({Decimal128Bid.NegativeInfinity}) = {Decimal128Bid.AtanPi(Decimal128Bid.NegativeInfinity)}");
                Console.WriteLine($"AtanPi(1) = {Decimal128Bid.AtanPi(1)}");
                Console.WriteLine($"Atan(-1E-10M) = {Decimal128Bid.Atan(-1E-10M)}");
                Console.WriteLine();

                Console.WriteLine($"Decimal128(0.1D) = {(Decimal128Bid)0.1D}");
                Console.WriteLine($"Decimal128(0.1M) = {(Decimal128Bid)0.1M}");
                Console.WriteLine($"Atan(0.1D) = {Decimal128Bid.Atan(0.1D)}");
                Console.WriteLine($"Atan(0.1M) = {Decimal128Bid.Atan(0.1M)}");
                Console.WriteLine();

                for (Decimal128Bid x = 0.0M; x <= 1.0M; x += 0.05M) {
                    Console.WriteLine($"Atan({x}) = {Decimal128Bid.Atan(x)}");
                }

                Console.WriteLine();
                Console.WriteLine($"4 * Atan(1) = {4 * Decimal128Bid.Atan(1)}");
                Console.WriteLine($"Pi = {Decimal128Bid.Pi}");
                return 0;
            }

            {
                Console.WriteLine(BigRational.Math.ILog10((BigRational)9.9));
                Decimal128Bid a = Decimal128Bid.Pi;
                var b = (BigRational)a;
                var dsfa = (Decimal128Bid)b + Decimal128Bid.One;
                var d = (BigRational)dsfa;
                var dd = Decimal128Bid.Tau / 2;
                var rr = (BigRational)dd;
                var dd1 = -Decimal128Bid.MaxValue % Decimal128Bid.Tau;
                var rTau = (BigRational)(-Decimal128Bid.Tau);
                Console.WriteLine(rTau);
                var rr1 = (BigRational)dd1;
                var xx1 = (double)rr1;
                Console.WriteLine(Decimal128Bid.Epsilon);
                Console.WriteLine(Decimal128Bid.One / 1000);
                Console.WriteLine(Decimal128Bid.Epsilon * 1000);
                Console.WriteLine(Decimal128Bid.Epsilon * 1001);
                Console.WriteLine(Decimal128Bid.Epsilon / 2);
                Console.WriteLine(Decimal128Bid.Epsilon / 1.99999999);

                Console.WriteLine(Decimal128Bid.Scale10(100000000, -7));


                Console.WriteLine((Decimal128Bid)801E5M); // System.Decimal does not have strictly positive qExponent
                Console.WriteLine(Decimal128Bid.Scale10(801, 5));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / 10);
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(1, 1));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(100, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801, 5) / Decimal128Bid.Scale10(100000000, -7));

                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / 10);
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(1, 1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(100, -1));
                Console.WriteLine(Decimal128Bid.Scale10(801000000, -1) / Decimal128Bid.Scale10(100000000, -7));





                Console.WriteLine((Decimal128Bid)1234D);
                Console.WriteLine((Decimal128Bid)1234E7D);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D}");
                Console.WriteLine((Decimal128Bid)1234E7D / 10);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 10}");
                Console.WriteLine((Decimal128Bid)1234E7D / 1E1M);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 1E1M}");
                Console.WriteLine((Decimal128Bid)1234E7D / 100E-1M);
                Console.WriteLine($"System.Decimal {(System.Decimal)1234E7D / 100E-1M}");

                Console.WriteLine((Decimal128Bid)(-1.25D));
                Console.WriteLine((Decimal128Bid)4.2D);
                Console.WriteLine((Decimal128Bid)(-4.20M));

                Console.WriteLine((Decimal128Bid)1 / 0.1M);
                Console.WriteLine((Decimal128Bid)1 % 0.1M);

                Console.WriteLine((Decimal128Bid)1 / 0.1D);
                Console.WriteLine((Decimal128Bid)1 % 0.1D);

                Console.WriteLine(Decimal128Bid.Scale10(-9.05M, -33));
                Console.WriteLine((1 + Decimal128Bid.Scale10(1, -32)) * (1 - Decimal128Bid.Scale10(1, -33)));

                Console.WriteLine(Decimal128Bid.FusedMultiplyAdd(1 + Decimal128Bid.Scale10(1, -32), 1 - Decimal128Bid.Scale10(1, -33), Decimal128Bid.Scale10(-9.05M, -33)));
                Console.WriteLine(Decimal128Bid.FusedMultiplyAdd(1 + Decimal128Bid.Scale10(1, -32), 1 - Decimal128Bid.Scale10(1, -33), Decimal128Bid.Scale10(-9.049999999999M, -33)));
                Console.WriteLine(Decimal128Bid.Hypot(3, 4));
                Console.WriteLine(Decimal128Bid.Hypot(30, 40));
                Console.WriteLine(Decimal128Bid.Hypot(0, 1));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.Scale10(1, 1), 1));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(9, 6127)));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(10, 6127)));
                Console.WriteLine(Decimal128Bid.Hypot(Decimal128Bid.MinValue, Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34)));
                Console.WriteLine(Decimal128Bid.Scale10(10, 6127));
                Console.WriteLine(Decimal128Bid.Scale10(1, 6128));
                Console.WriteLine(Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34));
                Console.WriteLine(Decimal128Bid.Scale10(Decimal128Bid.Scale10(1, 34) - 1, 6128 - 34) + Decimal128Bid.Scale10(1, 6127 - 33));

                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6176));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6176)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6143)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) - Decimal128Bid.Scale10(1, -6176));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6143) - Decimal128Bid.Scale10(1, -6176)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(9, -6144));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(9, -6144)):x32}");
                Console.WriteLine(Decimal128Bid.Scale10(1, -6144));
                Console.WriteLine($"0X{BitConverter.Decimal128ToUInt128Bits(Decimal128Bid.Scale10(1, -6144)):x32}");

                Console.WriteLine(Decimal128Bid.Scale10(1, -6143));
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143)));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * Decimal128Bid.Scale10(1, -100));
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * Decimal128Bid.Scale10(1, -100)));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * .95D);
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * .95D));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6143) * .95M);
                Console.WriteLine(Decimal128Bid.IsSubnormal(Decimal128Bid.Scale10(1, -6143) * .95M));


                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6177));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6144));

                Console.WriteLine(Decimal128Bid.Scale10((Decimal128Bid)(BigRational)BigInteger.Pow(10, 33), -6179));
                Console.WriteLine(Decimal128Bid.Scale10(1, -6146));


                Console.WriteLine((double)Decimal128Bid.MaxValue);


                return 0;
            }






            {
                var sdfad = Rational64.FromFraction(27, -512);
                var sdfasdad = Rational64.Cbrt(sdfad);
                Console.WriteLine(sdfasdad);
            }
            {

                var v1 = Unsafe.BitCast<Int64, Long<int, uint>>(0X0700002000000100L);
                var v2 = Unsafe.BitCast<Int64, Long<int, uint>>(0X0001001000000100L);
                var v3 = v1 | v2;
                Console.WriteLine(Unsafe.BitCast<Long<int, uint>, Int64>(v3));

            }
            if (false) {
                asfa sadfa = new asfa() { a = 1919810, b = 114514 };
                Console.WriteLine($"{nameof(sadfa)} = {sadfa}");
                var t = Clone1(ref sadfa);
                Console.WriteLine($"{nameof(t)} = {t}");
                Console.WriteLine($"{nameof(sadfa)} = {sadfa}");
                Console.WriteLine($"===");
                var t2 = Clone1b(ref sadfa);
                Console.WriteLine($"{nameof(t2)} = {t2}");



                return 0;
            }

            {
                var d = (decimal)BigRational.FromFraction(22, 7);
                Console.WriteLine(d);
                var d1 = (decimal)(double)BigRational.FromFraction(22, 7);
                Console.WriteLine(d1);

                Console.WriteLine((BigRational)d);

                return 0;
            }


            {

                Console.WriteLine(new BitMatrix16x16(Vector256<UInt16>.One) << 9.AsRowShiftCount());
                return 0;
            }
            {

                Console.WriteLine((BigRational)Math.PI);
                Console.WriteLine(BigRational.FromDoubleByContinuedFraction(Math.PI));


                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromRationalByContinuedFraction((BigRational)Math.PI, i));

                }
                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromRationalByContinuedFraction((BigRational)0.1, i));

                }
                for (int i = 1; i < 30; ++i) {
                    Console.WriteLine(BigRational.FromDoubleByContinuedFraction(0.1, i));

                }
                Console.WriteLine(BigRational.FromDoubleByContinuedFraction(float.PositiveInfinity));

                return 0;
            }
            {


                string output = $$$"""{{{3:V:<s:aaa>}}}""";
                Console.WriteLine(output);
                return 0;
            }
            {
                {
                    var a = """
                194286934960954013505219131142675643538134782692201101987417450051523093244750612407244445269834128729581999001097698837
                059113628401747382012597604790968895348450733898006365028455976752552318787766900421989105252601990430439871601888440842
                224149126597339343184859242253914800956534425991653876968765566900786049362862971570561111/11588945817616684075008826067
                085048249736465095545485270761101056576578952644709576992975689593185437220926517277187699610446278876498377104974586240
                406864862576323161803360152645281736589031288050365377966313964390654862478447629844776743834403928717405761405728126068
                257954320624252883485392554162544380618209422131790230847488
                """;
                    a = a.RemoveWhitespace();
                    var b = a.Split('/', 2, StringSplitOptions.TrimEntries);
                    BigInteger n;
                    var m = BigInteger.One;
                    if (b.Length == 1) {
                        n = BigInteger.Parse(b[0]);
                    } else {
                        n = BigInteger.Parse(b[0]);
                        m = BigInteger.Parse(b[1]);
                    }
                    var p = BigRational.FromFraction(n, m);
                    Console.WriteLine(p);
                    Console.WriteLine((double)p);

                    return 0;

                }

            }
            {
                var sdfasdf = 34325451245454352453523453453.735354345m;
                sdfasdf /= 100m;
                Console.WriteLine($@"{sdfasdf.GetFlagsInternal():R} {sdfasdf.GetHigh32Internal():R} {sdfasdf.GetLow64Internal():R}");
                Console.WriteLine($@"{sdfasdf:R}");
                var aaa = UInt128ConversionTest1<UltimateOrb.UInt128>(sdfasdf);

                Console.WriteLine($@"{aaa:R}");

                return 0;
            }
            {

                var t = new Rational64ExactTests();
                for (long i = 0; i < 330000000; i++) {
                    t.FractionPart_OfIntegersIsZero((uint)i);
                }
                return 0;

            }


            {
                var s = 0;
                for (var i = 0L; 1_000_000_000_000 > i; ++i) {
#pragma warning disable UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    var sdfs = DoubleArithmetic.BigMulUnsigned<uint>(0x40000001, 0x00030002, out var aa);
#pragma warning restore UoWIP_GenericMath // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                    s += sdfs.GetHashCode();
                    s ^= aa.GetHashCode();

                }
                Console.WriteLine(s);
            }


            {
                Console.WriteLine(RuntimeEnvironment.GetSystemVersion());

                var netCoreVer = System.Environment.Version;
                Console.WriteLine(netCoreVer);

                var runtimeVer = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
                Console.WriteLine(runtimeVer);

                Console.WriteLine(MilliDecimal.MaxValue);

                // UnsafeTests.UnboxNullableTest();
                return 0;
            }
            {
                for (uint i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MaxValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = unchecked((uint)(int.MinValue)) >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = uint.MaxValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;

            }
            {


                for (var i = 0; 10 > i; ++i) {
                    Console.WriteLine($@"{i,6}: {Mathematics.BinaryNumerals.CountStorageBits(i),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MaxValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                Console.WriteLine($@"======");
                for (var i = 3; 0 <= i; --i) {
                    var j = int.MinValue >>> i;
                    Console.WriteLine($@"{j,6:X8}: {Mathematics.BinaryNumerals.CountStorageBits(j),6}");
                }
                return 0;




            }
            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        p0 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new StrongBox<Bits8388608>();
                        unsafe {
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 1] = 11000000 + (UInt64)i;
                            affffffffs.Value.b[dssaf / sizeof(UInt64) - 2] = 22000000 + (UInt64)i;

                        }


                        if (i == 7) {
                            p1 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 77) {
                            p2 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 777) {
                            p3 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        } else if (i == 7777) {
                            p4 = ref Unsafe.As<Bits8388608, UInt64>(ref Unsafe.Add(ref affffffffs.Value, 1));
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;




            }

            {
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                ref var p1 = ref Unsafe.NullRef<UInt64>();
                ref var p2 = ref Unsafe.NullRef<UInt64>();
                ref var p3 = ref Unsafe.NullRef<UInt64>();
                ref var p4 = ref Unsafe.NullRef<UInt64>();
                {
                    ref var p0 = ref Unsafe.NullRef<UInt64>();
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        p0 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                    }
                    p0 = ref Unsafe.NullRef<UInt64>();
                }
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);

                for (var j = 0; 2 > j; ++j) {
                    for (var i = 0; 10000 > i; ++i) {
                        var affffffffs = new UInt64[1024 * 1024];
                        affffffffs[^1] = 11000000 + (UInt64)i;
                        affffffffs[^2] = 22000000 + (UInt64)i;

                        if (i == 7) {
                            p1 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 77) {
                            p2 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 777) {
                            p3 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        } else if (i == 7777) {
                            p4 = ref Unsafe.Add(ref MemoryMarshal.GetReference(affffffffs.AsSpan()), affffffffs.Length);
                        }
                    }
                    GC.Collect();
                    Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                    Console.WriteLine(Unsafe.Subtract(ref p1, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p2, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p3, 1));
                    Console.WriteLine(Unsafe.Subtract(ref p4, 1));
                }
                p1 = ref Unsafe.NullRef<UInt64>();
                p2 = ref Unsafe.NullRef<UInt64>();
                p3 = ref Unsafe.NullRef<UInt64>();
                p4 = ref Unsafe.NullRef<UInt64>();
                GC.Collect();
                Console.WriteLine(GC.GetGCMemoryInfo().HeapSizeBytes);
                return 0;
            }
            {
                {
                    Console.WriteLine(Math.ILogB(-3));
                    Console.WriteLine(Math.ScaleB(double.MaxValue, 1000000000));


                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        Console.WriteLine(Math.ILogB(Math.Abs(Volatile.Read(ref aaffaa))));
                    }
                    aaaa();
                    aaaa();

                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var afa = new UltimateOrb.Numerics.QuaternionD(101, 102, 103, (double)104);
                        Console.WriteLine(afa.X);
                    }
                    aaaa();
                    aaaa();
                }
            }
            {

                if (false) {
                    Console.WriteLine($@"{new Vector3D(4, 3, 5).GetNormalizedSafe()}");
                    Console.WriteLine($@"{new Vector3D(1, 1, 1).GetNormalizedSafe()}");
                    {
                        var aa = new Vector3D(4 * double.Epsilon, 3 * double.Epsilon, 5 * double.Epsilon);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.MaxValue);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, 1);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    {
                        var aa = new Vector3D(double.MaxValue, -double.MaxValue, double.NaN);
                        Console.WriteLine($@"{aa.GetNormalizedSafe()}");
                    }
                    return 0;

                }


                const bool Extrinsic = false;
                const bool Intrinsic = true;
                System.Numerics.Vector3 X = new System.Numerics.Vector3(1, 0, 0);
                System.Numerics.Vector3 Y = new System.Numerics.Vector3(0, 1, 0);
                System.Numerics.Vector3 Z = new System.Numerics.Vector3(0, 0, 1);

                static System.Numerics.Matrix4x4 M0(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, double angle0, double angle1, double angle2) {
                    var t0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis0, angle0.ToStandardF());
                    var t1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis1, angle1.ToStandardF());
                    var t2 = System.Numerics.Matrix4x4.CreateFromAxisAngle(axis2, angle2.ToStandardF());
                    return isIntrinsic ? t2 * t1 * t0 : t0 * t1 * t2;
                }

                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.SystemNumericsExtensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var m0 = aa.ToStandardF();
                        UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(
                            x, y, z, a,
                            out var a1, out var a2, out var a3);
                        var m1 = M0(Intrinsic, X, Y, Z, a1, a2, a3);


                        var d = m0 - m1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 3.5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 5e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;
                }
                if (false) {




                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        new System.Numerics.Vector3(0, 0, 1),
                        MathF.PI / 3.0F);
                    Console.WriteLine(m0);
                    return 0;

                }


                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToExtrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;

                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(-1, 2, 3, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }


                {
                    var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                    var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                    var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                    var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                    var rr = new Random();

                    var eMax = float.NaN;
                    var e1Max = float.NaN;
                    for (var i = 0; 10_000_000 > i; ++i) {
                        var x = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var y = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var z = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var a = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                        var aa = default(Matrix4x4D);

                        UltimateOrb.Numerics.SystemNumericsExtensions.ToMatrixFromAxisAngle(
                            x, y, z, a,
                            out aa.E00, out aa.E01, out aa.E02,
                            out aa.E10, out aa.E11, out aa.E12,
                            out aa.E20, out aa.E21, out aa.E22);
                        aa.E33 = 1.0;
                        var a0 = aa.ToStandardF();
                        var a1 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                            System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x, y, z)), a);

                        var d = a0 - a1;
                        var dx = System.Numerics.Vector4.Transform(vx, d);
                        var dy = System.Numerics.Vector4.Transform(vy, d);
                        var dz = System.Numerics.Vector4.Transform(vz, d);
                        var dw = System.Numerics.Vector4.Transform(vw, d);
                        var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                        var e1 = dw.LengthSquared();
                        if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                            eMax = e;
                        }
                        if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                            e1Max = e1;
                        }
                        if (e >= 7e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }
                        if (e1 >= 12e-13F) {
                            Console.WriteLine("!!!");
                            throw new Exception();
                        }

                    }

                    Console.WriteLine("...");
                    Console.WriteLine(eMax);
                    Console.WriteLine(e1Max);
                    Console.WriteLine();
                    return 0;





                }

                {
                    //
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, 2.0 / 3.0 * Math.PI);
                    // var (x, y, z, angle) = (1.0, 1.0, 1.0, -Math.PI);
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);
                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);
                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }
                    return 0;
                }
                {
                    var (x, y, z, angle) = (0.5, -1.5, 2.5, 2.0);

                    UltimateOrb.Numerics.SystemNumericsExtensions.ToIntrinsicXYZEulerAnglesFromAxisAngle(x, y, z, angle, out var a0, out var a1, out var a2);

                    Console.WriteLine($@"{a0:R}, {a1:R}, {a2:R}");

                    var m0 = System.Numerics.Matrix4x4.CreateFromAxisAngle(
                        System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(x.ToStandardF(), y.ToStandardF(), z.ToStandardF())),
                        angle.ToStandardF());

                    Console.WriteLine(m0);
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                        var p = new System.Numerics.Vector4(1, 1, 1, 0);
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m0));
                        Console.WriteLine(System.Numerics.Vector4.Transform(p, m1));


                    }











                    /*
                    {
                        var m1 = M0(Extrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Extrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Y, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, X, Z, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, X, Z, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Y, Z, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, X, Y, a0, a1, a2);
                        Console.WriteLine(m1);
                    }
                    {
                        var m1 = M0(Intrinsic, Z, Y, X, a0, a1, a2);
                        Console.WriteLine(m1);
                    }*/
                    return 0;
                }




                {


                    static System.Numerics.Matrix4x4 M1(Converter<Vector3D, Vector4D> converter, double angle0, double angle1, double angle2) {
                        var s = converter(new Vector3D(angle0, angle1, angle2));
                        var t = System.Numerics.Matrix4x4.CreateFromAxisAngle(s.E012.GetNormalized().ToStandardF(), s.E3.ToStandardF());
                        return t;
                    }

                    static int sss(bool isIntrinsic, System.Numerics.Vector3 axis0, System.Numerics.Vector3 axis1, System.Numerics.Vector3 axis2, Converter<Vector3D, Vector4D> converter) {
                        var m0 = (double angle0, double angle1, double angle2)
                            => M0(isIntrinsic, axis0, axis1, axis2, angle0, angle1, angle2);
                        var vx = new System.Numerics.Vector4(1, 0, 0, 0);
                        var vy = new System.Numerics.Vector4(0, 1, 0, 0);
                        var vz = new System.Numerics.Vector4(0, 0, 1, 0);
                        var vw = new System.Numerics.Vector4(-1, -1, -1, 0);

                        var rr = new Random();

                        var eMax = float.NaN;
                        var e1Max = float.NaN;
                        for (var i = 0; 10_000_000 > i; ++i) {
                            var angle0 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle1 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var angle2 = (10 * Math.PI * rr.NextDouble() - 5 * Math.PI).ToStandardF();
                            var a0 = m0(angle0, angle1, angle2);
                            var a1 = M1(converter, angle0, angle1, angle2);

                            var d = a0 - a1;
                            var dx = System.Numerics.Vector4.Transform(vx, d);
                            var dy = System.Numerics.Vector4.Transform(vy, d);
                            var dz = System.Numerics.Vector4.Transform(vz, d);
                            var dw = System.Numerics.Vector4.Transform(vw, d);
                            var e = dx.LengthSquared() + dy.LengthSquared() + dz.LengthSquared();
                            var e1 = dw.LengthSquared();
                            if (!(e <= eMax)) { // Caution with NaN! Do not invert the condition.
                                eMax = e;
                            }
                            if (!(e1 <= e1Max)) { // Caution with NaN! Do not invert the condition.
                                e1Max = e1;
                            }
                            if (e >= 3.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }
                            if (e1 >= 5.5e-13F) {
                                Console.WriteLine("!!!");
                                throw new Exception();
                            }

                        }

                        Console.WriteLine("...");
                        Console.WriteLine(eMax);
                        Console.WriteLine(e1Max);
                        Console.WriteLine();
                        return 0;
                    }




                    if (true) {

                        sss(Extrinsic, X, Y, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXYZEulerAngles);
                        sss(Intrinsic, X, Y, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicXYZEulerAngles);
                        sss(Extrinsic, X, Z, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXZYEulerAngles);
                        sss(Intrinsic, X, Z, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicXZYEulerAngles);
                        sss(Extrinsic, Y, X, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicYXZEulerAngles);
                        sss(Intrinsic, Y, X, Z, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYXZEulerAngles);
                        sss(Extrinsic, Y, Z, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicYZXEulerAngles);
                        sss(Intrinsic, Y, Z, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYZXEulerAngles);
                        sss(Extrinsic, Z, X, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZXYEulerAngles);
                        sss(Intrinsic, Z, X, Y, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicZXYEulerAngles);
                        sss(Extrinsic, Z, Y, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles);
                        sss(Intrinsic, Z, Y, X, UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicZYXEulerAngles);



                        return 0;
                    }
                }


                {
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    var q = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{rb}");
                    Console.WriteLine($@"{p}");
                    Console.WriteLine($@"{q}");
                    // p |--> p . rb


                }
                {


                    var ra = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 1, 0), 0.5F * MathF.PI);
                    var rb = System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI);
                    var p = new System.Numerics.Vector4(1, 1, 1, 0);
                    p = System.Numerics.Vector4.Transform(p, ra);
                    p = System.Numerics.Vector4.Transform(p, rb);
                    Console.WriteLine($@"{p}");
                }


                {
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(1, 0, 0), 0.5F * MathF.PI));
                    Console.WriteLine(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(2, 0, 0), 0.5F * MathF.PI)); // incorrect


                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicXYZEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }

                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles(7, 8, 9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromExtrinsicZYXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdfa = UltimateOrb.Numerics.SystemNumericsExtensions.ToAxisAngleFromIntrinsicYZXEulerAngles(0.6, -1.3, 0.9);
                        Console.WriteLine(sdfa);
                        Console.WriteLine(sdfa.E012.GetNormalized());
                    }
                    aaaa();
                }
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
                    static void aaaa() {
                        var sdaf1 = new Vector4D(3, 3, 3, 3);
                        var sdaf2 = new Vector4D(7, 17, 27, 37);
                        var sdaf = sdaf1 + sdaf2;
                        Console.WriteLine(sdaf);
                    }
                    aaaa();
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensionsD.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var sdfa = new System.Numerics.Vector3(0.6F, -1.3F, 0.9F);
                    var m = UltimateOrb.Numerics.StandardExtensions.ToRotationMatrixExtrinsicXYZ(sdfa);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());
                }
                {
                    var m = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(0.9F, -1.3F, 0.6F);
                    Console.WriteLine($"[ {m.M11},\t{m.M12},\t{m.M13},\t{m.M14},\n  {m.M21},\t{m.M22},\t{m.M23},\t{m.M24},\n  {m.M31},\t{m.M32},\t{m.M33},\t{m.M34},\n  {m.M41},\t{m.M42},\t{m.M43},\t{m.M44} ]");
                    Console.WriteLine(m.GetDeterminant());

                }




            }
            {
                Thread.MemoryBarrier();
                ff2 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff2 = unchecked((int)(byte)ff);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = Unsafe.As<int, CanonicalIntegerBoolean>(ref Unsafe.AsRef(ff));
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(999, 321);
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = true;
                Thread.MemoryBarrier();
                Thread.MemoryBarrier();
                ff3 = false;
                Thread.MemoryBarrier();
                System.Diagnostics.Debugger.Break();
                var sds12 = UltimateOrb.Utilities.BooleanIntegerModule.GreaterThan(999, 321);
                Console.WriteLine($@"{sds12}");
                var sds = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(3, 7);
                Console.WriteLine($@"{sds}");
                var sds1 = UltimateOrb.CanonicalIntegerBooleanModule.GreaterThan(1, 0);
                Console.WriteLine($@"{sds1}");
                return 0;
            }
            {

                UltimateOrb.Numerics.Generic.Long<Int64, UInt64> ggg = new(3, 6);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);
                Console.WriteLine(ggg.Lo);
                Console.WriteLine(ggg.Hi);

                return 0;
            }
            {
                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }

                for (int i = 0; i < 100000000; i++) {

                    RefStructTest.ReadAFieldA();
                }
            }

            {

                var threads = new Thread[10];
                var a = true;
                for (int i = 0; i < threads.Length; i++) {
                    threads[i] = new Thread(() => {
                        for (; Volatile.Read(ref a);) {
                            aasfd();
                        }
                    });
                }
                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Start();
                }
                Thread.Sleep(100);
                try {
                    aasfd();
                    var sdfas = new int[333];
                    aasfd();
                    var sdafsdf = sdfas.AsMemorySpan(2, 42);
                    aasfd();
                    var sdfasdf = sdafsdf.Span;
                    aasfd();

                    for (var i = 0; sdfas.Length > i; ++i) {
                        aasfd();
                        sdfasdf[i] = -i;
                        aasfd();
                    }
                    Volatile.Write(ref a, false);
                    var sadfa = sdfas[44];
                    Console.WriteLine(sadfa);
                } finally {
                    Volatile.Write(ref a, false);
                }

                for (int i = 0; i < threads.Length; i++) {
                    threads[i].Join();
                }
                return 0;
            }
            {

                var func_x = (Func<double, double>)((x) => x * x);
                var func_y = (Func<double, double>)((x) => x / 2 - 100);

                var curve_x = func_x.ToParametricCurve((-2, 3)).Reparametrization(x => 2 * x, x => x / 2);
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(1).ToString());
                return 0;

            }

            {

                var func_x = (Func<double, double>)((x) => -x);
                var func_y = (Func<double, double>)((x) => 2 * x);

                var curve_x = func_x.ToParametricCurve((-2, 3));
                var curve_y = func_y.ToParametricCurve((-2, 3));
                var curve_xy =
                    from x in curve_x
                    from y in curve_y
                    select (x, y);

                Console.WriteLine(curve_xy.Domain.ToString());


                Console.WriteLine(curve_xy.Invoke(2).ToString());
                return 0;
            }



            {
                var vdsadf = new BigUIntegerBuilder(333);
                vdsadf.MultiplyExp10(20);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Add(66666666);
                vdsadf.Multiply(1000000000);
                var sadf = vdsadf.ToString();
                Console.WriteLine($@"{sadf}");
                return 0;
            }
            {
                var a = typeof(MathF);
                Console.WriteLine($@"{(Quadruple)1.5}");
                return 0;
            }
            {
                var a = -2.8;
                var b = 1.0;
                Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{Math.IEEERemainder(a, b):R}");

                // Console.WriteLine($@"{a % b:R}");
                Console.WriteLine($@"{(Double)Quadruple.Math.IEEERemainder(a, b):R}");

                return 0;

            }
            {
                var a1 = 1;
                var a2 = 2;
                var a3 = 3;
                var a4 = 4;
                var b = (double)(1L << (52 + 2));
                for (var i = 0; i < 64; i++) {
                    var bb = Math.ScaleB(b, i);

                    for (var j = 0; j < 8; ++j) {
                        Console.WriteLine($@"{UltimateOrb.Utilities.CilVerifiable.AddThenSubtractFirst(bb, j):R}");
                    }
                }
                return 0;

            }
            {
                var sdf = (object)(-4);

                Console.WriteLine(UltimateOrb.Utilities.CilVerifiable.UnboxRef<int>(sdf));

                return 0;
            }
            {
                var aaa = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var bbb = Volatile.Read(ref UltimateOrb.Dummy<int>.Value);
                var a = 0L;
                for (var i = 0L; i < 400000000000L; i++) {
                    a ^= UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(aaa, bbb);

                }
                Console.WriteLine(a);

                return 0;
            }
            {
                var sdffa = new int[] {
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, Double.NaN),
                    UltimateOrb.Utilities.BooleanIntegerModule.GreaterThanOrEqual(0.3, 0.2),
                };

                foreach (var item in sdffa) {
                    Console.WriteLine(item);
                }


                return 0;
            }
            {
                var a = (1UL << 52) + 1;
                var b = (1UL << 52);
                var c = (1UL << 52) - 1;
                var d = (1UL << 52) - 1;

                var p = a + 0.5;
                var q = b + 0.5;
                var r = c + 0.5;
                var s = d + 0.75;
                var sp = d + 0.76;
                var sm = d + 0.74;

                Console.WriteLine($@"{double.Epsilon:R}");
                Console.WriteLine($@"{double.Epsilon * d:R}");
                Console.WriteLine($@"{p:R}");
                Console.WriteLine($@"{q:R}");
                Console.WriteLine($@"{r:R}");
                Console.WriteLine($@"{s:R}");
                Console.WriteLine($@"{sp:R}");
                Console.WriteLine($@"{sm:R}");
                return 0;
            }
            {

                var sdfada = 0u;
                sdfada ^= sdfada;
                var sdaf = System.Numerics.BitOperations.LeadingZeroCount(sdfada);
                _ = sdaf.GetHashCode();

            }
            {
                var sfassss = BitConverter.Int64BitsToDouble(0x7FFF400000000000);
                var sfas = sfassss;
                var vdsa = 0.0 - sfas;
                var sfad = BitConverter.DoubleToInt64Bits(sfas);
                var asdsd = BitConverter.DoubleToInt64Bits(vdsa);
                _ = (sfad ^ asdsd).GetHashCode();

            }
            {


                var ccc = (Quadruple)0.0 < (Quadruple)Double.NegativeInfinity;
                Console.WriteLine(ccc);
            }
            {
                var ccc = Double.NaN != Double.NaN;
                var a = Quadruple.IsNaN(Quadruple.NaN);
                Console.WriteLine(a);
                var sdfa = +Quadruple.MinValue;
                Console.WriteLine(sdfa);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            {
                var sdfa = (NodeId_A)3;
                var dsafsd = 7 * sdfa;
                Console.WriteLine(dsafsd);
            }
            return 0;
        }
    }
}


namespace UltimateOrb.Plain.ValueTypes {

    public readonly struct NodeId_A
        : IEquatable<NodeId_A>, IFormattable {

        private readonly int m_value;

        internal NodeId_A(int value) {
            this.m_value = value;
        }

        public static NodeId_A operator +(NodeId_A value) {
            return value;
        }

        public static NodeId_A operator -(NodeId_A value) {
            return new NodeId_A(unchecked(~value.m_value - 1));
        }

        public static NodeId_A operator <<(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value << shift);
        }

        public static NodeId_A operator >>(NodeId_A value, int shift) {
            return new NodeId_A(value.m_value >> shift);
        }

        public static NodeId_A operator +(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(1 + first.m_value + second.m_value));
        }

        public static NodeId_A operator -(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(first.m_value + ~second.m_value));
        }

        public static NodeId_A operator *(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value * ~second.m_value)));
        }

        public static NodeId_A operator /(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value / ~second.m_value)));
        }

        public static NodeId_A operator %(NodeId_A first, NodeId_A second) {
            return new NodeId_A(unchecked(~(~first.m_value % ~second.m_value)));
        }

        public static NodeId_A operator &(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value | second.m_value);
        }

        public static NodeId_A operator |(NodeId_A first, NodeId_A second) {
            return new NodeId_A(first.m_value & second.m_value);
        }

        public static NodeId_A operator ^(NodeId_A first, NodeId_A second) {
            return new NodeId_A(~(first.m_value ^ second.m_value));
        }

        public static NodeId_A operator ~(NodeId_A value) {
            return new NodeId_A(~value.m_value);
        }

        public static NodeId_A operator ++(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value - 1));
        }

        public static NodeId_A operator --(NodeId_A value) {
            return new NodeId_A(unchecked(value.m_value + 1));
        }

        public static implicit operator int(NodeId_A value) {
            return ~value.m_value;
        }

        public static implicit operator NodeId_A(int value) {
            return new NodeId_A(~value);
        }

        public override bool Equals(object obj) {
            if (obj is NodeId_A other) {
                return this.Equals(other);
            }
            return false;
        }

        public bool Equals(NodeId_A other) {
            return this.m_value == other.m_value;
        }

        public override int GetHashCode() {
            return this.m_value;
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            return (~this.m_value).ToString(format, formatProvider);
        }

        public override string ToString() {
            return (~this.m_value).ToString();
        }

        public static bool operator ==(NodeId_A left, NodeId_A right) {
            return left.m_value == right.m_value;
        }

        public static bool operator !=(NodeId_A left, NodeId_A right) {
            return left.m_value != right.m_value;
        }
    }
}





namespace UltimateOrb {

    static partial class Decimal128ExtensionsA {

        extension(Decimal128Bid @this) {

            public string ToStringWithSignAndNaNPayload() {
                return $"{(Decimal128Bid.IsNegative(@this) ? "-" : "+")}{(Decimal128Bid.IsSignalingNaN(@this) ? "s" : "")}{(Decimal128Bid.IsQuietNaN(@this) ? "q" : "")}{Decimal128Bid.Abs(@this)}{(Decimal128Bid.IsNaN(@this) ? $"({((UInt128.One << 110) - 1) & (UInt128)BitConverter.Decimal128ToUInt128Bits(@this)})" : "")}";
            }
        }
    }
}


#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
