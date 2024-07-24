using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Collections.Generic;
using UltimateOrb.Numerics.DataTypes;

namespace UltimateOrb.Numerics
{

    internal readonly struct NoCheck {
    }

    [Experimental("UoWIP_GenericMath")]
#pragma warning disable CA1715 // Identifiers should have correct prefix
    public readonly partial struct Modular<T, ModulusT> :
        INumber<Modular<T, ModulusT>>
#pragma warning restore CA1715 // Identifiers should have correct prefix
        where T : IBinaryInteger<T>, IUnsignedNumber<T>
        where ModulusT : IConstant<ModulusT, T> {

        readonly T m_value;

        public Modular(T value) : this(default, value % ModulusT.Value) {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Modular(NoCheck _, T value) {
            CheckModulus();
            m_value = value;
        }

        [Conditional("DEBUG")]
        static void CheckModulus() {
            _ = T.One / ModulusT.Value;
        }


        public static Modular<T, ModulusT> One {

            get => new(default, ModulusT.Value <= T.One ? T.Zero : T.One);
        }

        public static int Radix => throw new NotImplementedException();

        public static Modular<T, ModulusT> Zero {

            get => new(default, T.Zero);
        }

        static Modular<T, ModulusT> IAdditiveIdentity<Modular<T, ModulusT>, Modular<T, ModulusT>>.AdditiveIdentity {

            get => new(default, T.AdditiveIdentity);
        }

        static Modular<T, ModulusT> IMultiplicativeIdentity<Modular<T, ModulusT>, Modular<T, ModulusT>>.MultiplicativeIdentity {

            get => new(default, ModulusT.Value <= T.One ? T.AdditiveIdentity : T.MultiplicativeIdentity);
        }

        public static Modular<T, ModulusT> Abs(Modular<T, ModulusT> value) {
            return value;
        }

        public static bool IsCanonical(Modular<T, ModulusT> value) {
            return true;
        }

        public static bool IsComplexNumber(Modular<T, ModulusT> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(Modular<T, ModulusT> value) {
            /*
            ModulusT.Value.
            if (typeof(T) == typeof(ulong)) {
                T.TrailingZeroCount(value.m_Value)
                var d = Mathematics.NumberTheory.EuclideanAlgorithm.GreatestCommonDivisor((ulong)(object)ModulusT.Value, (ulong)(object)value, out var r);

            }*/
            throw new NotImplementedException();
        }

        public static bool IsFinite(Modular<T, ModulusT> value) {
            return true;
        }

        public static bool IsImaginaryNumber(Modular<T, ModulusT> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(Modular<T, ModulusT> value) {
            return false;
        }

        public static bool IsInteger(Modular<T, ModulusT> value) {
            return true;
        }

        public static bool IsNaN(Modular<T, ModulusT> value) {
            return false;
        }

        public static bool IsNegative(Modular<T, ModulusT> value) {
            throw new NotSupportedException();
        }

        public static bool IsNegativeInfinity(Modular<T, ModulusT> value) {
            return false;
        }

        public static bool IsNormal(Modular<T, ModulusT> value) {
            return T.IsZero(value.m_value);
        }

        public static bool IsOddInteger(Modular<T, ModulusT> value) {
            return !IsEvenInteger(value);
        }

        public static bool IsPositive(Modular<T, ModulusT> value) {
            throw new NotSupportedException();
        }

        public static bool IsPositiveInfinity(Modular<T, ModulusT> value) {
            return false;
        }

        public static bool IsRealNumber(Modular<T, ModulusT> value) {
            return true;
        }

        public static bool IsSubnormal(Modular<T, ModulusT> value) {
            return false;
        }

        public static bool IsZero(Modular<T, ModulusT> value) {
            return T.IsZero(value.m_value);
        }

        public static Modular<T, ModulusT> MaxMagnitude(Modular<T, ModulusT> x, Modular<T, ModulusT> y) {
            throw new NotSupportedException();
        }

        public static Modular<T, ModulusT> MaxMagnitudeNumber(Modular<T, ModulusT> x, Modular<T, ModulusT> y) {
            throw new NotSupportedException();
        }

        public static Modular<T, ModulusT> MinMagnitude(Modular<T, ModulusT> x, Modular<T, ModulusT> y) {
            throw new NotSupportedException();
        }

        public static Modular<T, ModulusT> MinMagnitudeNumber(Modular<T, ModulusT> x, Modular<T, ModulusT> y) {
            throw new NotSupportedException();
        }

        public static Modular<T, ModulusT> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            var v = T.Parse(s, style, provider);
            return new Modular<T, ModulusT>(v);
        }

        public static Modular<T, ModulusT> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            var v = T.Parse(s, style, provider);
            return new Modular<T, ModulusT>(v);
        }

        public static Modular<T, ModulusT> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            var v = T.Parse(s, provider);
            return new Modular<T, ModulusT>(v);
        }

        public static Modular<T, ModulusT> Parse(string s, IFormatProvider? provider) {
            var v = T.Parse(s, provider);
            return new Modular<T, ModulusT>(v);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Modular<T, ModulusT> result) {
            if (!T.IsZero(ModulusT.Value) && T.TryParse(s, style, provider, out var v)) {
                result = new Modular<T, ModulusT>(v);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Modular<T, ModulusT> result) {
            if (!T.IsZero(ModulusT.Value) && T.TryParse(s, style, provider, out var v)) {
                result = new Modular<T, ModulusT>(v);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Modular<T, ModulusT> result) {
            if (!T.IsZero(ModulusT.Value) && T.TryParse(s, provider, out var v)) {
                result = new Modular<T, ModulusT>(v);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Modular<T, ModulusT> result) {
            if (!T.IsZero(ModulusT.Value) && T.TryParse(s, provider, out var v)) {
                result = new Modular<T, ModulusT>(v);
                return true;
            }
            result = default;
            return false;
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            return m_value.ToString(format, formatProvider);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            return m_value.TryFormat(destination, out charsWritten, format, provider);
        }

        public static Modular<T, ModulusT> operator +(Modular<T, ModulusT> value) {
            return value;
        }

        static readonly bool IsZeroRng = ModulusT.Value <= T.One;

        static readonly bool HasSpareBits = BinaryIntegerTypeTraitHelpers.HasInfinitePrecision<T>() || ModulusT.Value <= (T.AllBitsSet ^ (T.AllBitsSet >>> 1));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator +(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            if (!IsZeroRng) {
                if (typeof(T) == typeof(uint)) {
                    var t = unchecked((uint)(object)first.m_value + (ulong)(uint)(object)second.m_value);
                    var s = (T)(object)(t >= (uint)(object)ModulusT.Value ? unchecked((uint)t - (uint)(object)ModulusT.Value) : unchecked((uint)t));
                    return new(default, s);
                }
                if (HasSpareBits) {
                    var t = unchecked(first.m_value + second.m_value);
                    var s = t >= ModulusT.Value ? unchecked(t - ModulusT.Value) : t;
                    return new(default, s);
                } else {
                    var t = unchecked(first.m_value + second.m_value);
                    var s = t < first.m_value || t >= ModulusT.Value ? unchecked(t - ModulusT.Value) : t;
                    return new(default, s);
                }
            }
            return Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator -(Modular<T, ModulusT> value) {
            if (!IsZeroRng) {
                return new(T.IsZero(value.m_value) ? T.Zero : unchecked(ModulusT.Value - value.m_value));
            }
            return Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator -(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            if (!IsZeroRng) {
                var t = unchecked(first.m_value - second.m_value);
                var s = first.m_value < second.m_value ? unchecked(t + ModulusT.Value) : t;
                return new(s);
            }
            return Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator ++(Modular<T, ModulusT> value) {
            if (!IsZeroRng) {
                var t = value.m_value;
                ++t;
                return new(default, ModulusT.Value.Equals(t) ? T.Zero : t);
            }
            return Zero;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator --(Modular<T, ModulusT> value) {
            if (!IsZeroRng) {
                var t = T.IsZero(value.m_value) ? ModulusT.Value : value.m_value;
                return new(default, --t);
            }
            return Zero;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Modular<T, ModulusT> operator *(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            if (!IsZeroRng) {
                UltimateOrb.Numerics.StandardGenericMathArithmeticProvider<T>.BigMulUnsigned(out var result_lo, out var result_hi, first.m_value, second.m_value);

                return Zero;
            }
            return Zero;
        }

        public static Modular<T, ModulusT> operator /(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            throw new NotImplementedException();
        }

        public static Modular<T, ModulusT> operator %(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            var q = first / second;
            return first - q * second;
        }

        public override bool Equals(object? obj) {
            return obj is not Modular<T, ModulusT> other ? false : Equals(other);
        }

        public override int GetHashCode() {
            return m_value.GetHashCode();
        }

        public int CompareTo(object? obj) {
            return obj == null ? 1 : obj is Modular<T, ModulusT> other ? CompareTo(other) : throw new ArgumentException("Object must be of type Modular<T, ModulusT>.");
        }

        public int CompareTo(Modular<T, ModulusT> other) {
            return m_value.CompareTo(other.m_value);
        }

        public bool Equals(Modular<T, ModulusT> other) {
            return m_value.Equals(other.m_value);
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertFromChecked<TOther>(TOther value, out Modular<T, ModulusT> result) {

            throw new NotImplementedException();
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertFromSaturating<TOther>(TOther value, out Modular<T, ModulusT> result) {

            throw new NotImplementedException();
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertFromTruncating<TOther>(TOther value, out Modular<T, ModulusT> result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertToChecked<TOther>(Modular<T, ModulusT> value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertToSaturating<TOther>(Modular<T, ModulusT> value, out TOther result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<Modular<T, ModulusT>>.TryConvertToTruncating<TOther>(Modular<T, ModulusT> value, out TOther result) {
            throw new NotImplementedException();
        }

        public static bool operator ==(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            return first.Equals(second);
        }

        public static bool operator !=(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            return !(first == second);
        }

        public static bool operator <(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            throw new NotSupportedException();
        }

        public static bool operator <=(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            throw new NotSupportedException();
        }

        public static bool operator >(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            throw new NotSupportedException();
        }

        public static bool operator >=(Modular<T, ModulusT> first, Modular<T, ModulusT> second) {
            throw new NotSupportedException();
        }
    }
}
