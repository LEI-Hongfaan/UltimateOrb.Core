using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Runtime.InteropServices;

namespace UltimateOrb {

    public interface IComparableNongenericDerived<TSelf, TBase1, TBase2>
        : IComparable
        , IComparableNongenericDerivedTagged<TSelf, Tag<TSelf>, TBase1, TBase2>
        where TSelf : IComparableNongenericDerived<TSelf, TBase1, TBase2>?
        where TBase1 : IComparable?
        where TBase2 : IComparable? {
    }

    public interface IComparableNongenericDerivedTagged<TSelf, Tag, TBase1, TBase2>
        : IComparable
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase1, TBase2>
        where TSelf : IComparableNongenericDerivedTagged<TSelf, Tag, TBase1, TBase2>?
        where TBase1 : IComparable?
        where TBase2 : IComparable? {

        int IComparable.CompareTo(object? obj) {
            var base1__ = TSelf.ToBase1((TSelf)(object)this);
            var base2__ = TSelf.ToBase2((TSelf)(object)this);
            if (obj is TSelf other__) {
                var r1 = base1__.CompareTo((object?)TSelf.ToBase1(other__));
                if (0 != r1) { return r1; }
                var r2 = base2__.CompareTo((object?)TSelf.ToBase2(other__));
                return r2;
            }
            {
                var r1 = base1__.CompareTo(obj);
                if (0 != r1) { return r1; }
                var r2 = base2__.CompareTo(obj);
                return r2;
            }
        }
    }
    public interface IComparableDerived<TSelf, TBase1, TBase2>
        : IComparable<TSelf>
        , IComparableDerivedTagged<TSelf, TSelf, TBase1, TBase2>
        where TSelf : IComparableDerived<TSelf, TBase1, TBase2>?
        where TBase1 : IComparable<TBase1>?
        where TBase2 : IComparable<TBase2>? {
    }

    public interface IComparableDerivedTagged<TSelf, Tag, TBase1, TBase2>
        : IComparable<TSelf>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase1, TBase2>
        where TSelf : IComparableDerivedTagged<TSelf, Tag, TBase1, TBase2>?
        where TBase1 : IComparable<TBase1>?
        where TBase2 : IComparable<TBase2>? {

        int IComparable<TSelf>.CompareTo(TSelf? obj) {
            var base1__ = TSelf.ToBase1((TSelf)(object)this);
            var base2__ = TSelf.ToBase2((TSelf)(object)this);
            var r1 = base1__.CompareTo(TSelf.ToBase1(obj));
            if (0 != r1) { return r1; }
            var r2 = base2__.CompareTo(TSelf.ToBase2(obj));
            return r2;
        }
    }

    public interface IComparableWithNongenericDerived<TSelf, TBase1, TBase2>
        : IComparable, IComparable<TSelf>
        , IComparableWithNongenericDerivedTagged<TSelf, TSelf, TBase1, TBase2>
        where TSelf : IComparableWithNongenericDerived<TSelf, TBase1, TBase2>?
        where TBase1 : IComparable<TBase1>?
        where TBase2 : IComparable<TBase2>? {
    }

    public interface IComparableWithNongenericDerivedTagged<TSelf, Tag, TBase1, TBase2>
        : IComparableDerivedTagged<TSelf, Tag, TBase1, TBase2>, IComparable
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase1, TBase2>
        where TSelf : IComparableWithNongenericDerivedTagged<TSelf, Tag, TBase1, TBase2>?
        where TBase1 : IComparable<TBase1>?
        where TBase2 : IComparable<TBase2>? {

        int IComparable.CompareTo(object? obj) {
            var base1__ = TSelf.ToBase1((TSelf)(object)this);
            var base2__ = TSelf.ToBase2((TSelf)(object)this);
            if (obj is TSelf other__) {
                var r1 = base1__.CompareTo(TSelf.ToBase1(other__));
                if (0 != r1) { return r1; }
                var r2 = base2__.CompareTo(TSelf.ToBase2(other__));
                return r2;
            }
            if (obj == null || (obj is TBase1 && obj is TBase2)) {
                var r1 = base1__.CompareTo((TBase1?)obj);
                if (0 != r1) { return r1; }
                var r2 = base2__.CompareTo((TBase2?)obj);
                return r2;
            }
            throw new ArgumentException($@"Object must be of supported type.", nameof(obj));
        }
    }
}
namespace UltimateOrb.Extensions {

    public static partial class ComparableExtensions {

        extension<T>(T @this)
            where T : IComparable {

            /// <inheritdoc cref="IComparable.CompareTo(object)"/>
            public int CompareTo(object? obj) => @this.CompareTo(obj);
        }

        extension<T>(T @this)
            where T : IComparable<T> {

            /// <inheritdoc cref="IComparable{T}.CompareTo(T)"/>
            public int CompareTo(T? obj) => @this.CompareTo(obj);
        }
    }
}

namespace UltimateOrb.Numerics {

    public interface IBitwiseOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
    : IBitwiseOperators<TSelf, TOther, TResult>
    , IBitwiseOperatorsDerivedTagged<TSelf, TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
    where TSelf : IBitwiseOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>?
    where TBase : IBitwiseOperators<TBase, TOtherBase, TResultBase>? {
    }

    public interface IBitwiseOperatorsDerivedTagged<TSelf, SelfTag, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase, TOther, TOther, TOtherBase, TResult, TResult, TResultBase>
        where TSelf : IBitwiseOperatorsDerivedTagged<TSelf, SelfTag, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IBitwiseOperators<TBase, TOtherBase, TResultBase>? {

    }

    public interface IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase, TOther, OtherTag, TOtherBase, TResult, ResultTag, TResultBase>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<SelfTag>, TBase>
        , IInterfaceDerivedBase<TSelf, Tag<OtherTag, IBitwiseOperatorsDerivedTags.Other>, TOther, TOtherBase>
        , IInterfaceDerivedBase<TSelf, Tag<ResultTag, IBitwiseOperatorsDerivedTags.Result>, TResult, TResultBase>
        where TSelf : IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase, TOther, OtherTag, TOtherBase, TResult, ResultTag, TResultBase>?
        where TBase : IBitwiseOperators<TBase, TOtherBase, TResultBase>? {

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ~(TSelf value) {
            return TSelf.FromBase(~TSelf.ToBase(value)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator &(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase(left)! & TSelf.ToBase(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator |(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase(left)! | TSelf.ToBase(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ^(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase(left)! ^ TSelf.ToBase(right)!)!;
        }
    }
    public interface IBinaryNumberDerived<TSelf, TBase>
        : IBinaryNumberDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IBinaryNumberDerived<TSelf, TBase>
        where TBase : IBinaryNumber<TBase> {
    }

    public interface IBinaryNumberDerivedTagged<TSelf, Tag, TBase>
        : IBinaryNumber<TSelf>
        , INumberDerivedTagged<TSelf, Tag, TBase>
        , IBitwiseOperatorsDerivedTagged<TSelf, Tag, TBase, TSelf, TBase, TSelf, TBase>
        where TSelf : IBinaryNumberDerivedTagged<TSelf, Tag, TBase>
        where TBase : IBinaryNumber<TBase> {

        static TSelf? INumberDerivedTagged<TSelf, Tag, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? INumberDerivedTagged<TSelf, Tag, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        static TSelf? IInterfaceDerivedBase<TSelf, Tag<TSelf, IBitwiseOperatorsDerivedTags.Other>, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag<TSelf, IBitwiseOperatorsDerivedTags.Other>, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        static TSelf? IInterfaceDerivedBase<TSelf, Tag<TSelf, IBitwiseOperatorsDerivedTags.Result>, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag<TSelf, IBitwiseOperatorsDerivedTags.Result>, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TSelf? FromBase(TBase? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.FromBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TBase? ToBase(TSelf? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.ToBase(value);

        static bool IBinaryNumber<TSelf>.IsPow2(TSelf value) {
            return TBase.IsPow2(TSelf.ToBase(value));
        }

        static TSelf IBinaryNumber<TSelf>.Log2(TSelf value) {
            return TSelf.FromBase(TBase.Log2(TSelf.ToBase(value)));
        }
    }

    public interface IBinaryIntegerDerived<TSelf, TBase>
        : IBinaryIntegerDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IBinaryIntegerDerived<TSelf, TBase>
        where TBase : IBinaryInteger<TBase> {
    }

    public interface IBinaryIntegerDerivedTagged<TSelf, Tag, TBase>
        : IBinaryInteger<TSelf>
        , IBinaryNumberDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IBinaryIntegerDerivedTagged<TSelf, Tag, TBase>
        where TBase : IBinaryInteger<TBase> {

        static TSelf IBinaryInteger<TSelf>.PopCount(TSelf value) {
            return TSelf.FromBase(TBase.PopCount(TSelf.ToBase(value)));
        }

        static TSelf IBinaryInteger<TSelf>.TrailingZeroCount(TSelf value) {
            return TSelf.FromBase(TBase.TrailingZeroCount(TSelf.ToBase(value)));
        }

        static bool IBinaryInteger<TSelf>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value) {
            var return__ = TBase.TryReadBigEndian(source, isUnsigned, out var value__);
            value = TSelf.FromBase(value__);
            return return__;
        }

        static bool IBinaryInteger<TSelf>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out TSelf value) {
            var return__ = TBase.TryReadLittleEndian(source, isUnsigned, out var value__);
            value = TSelf.FromBase(value__);
            return return__;
        }

        int IBinaryInteger<TSelf>.GetByteCount() {
            return TSelf.ToBase((TSelf)(object)this).GetByteCount();
        }

        int IBinaryInteger<TSelf>.GetShortestBitLength() {
            return TSelf.ToBase((TSelf)(object)this).GetShortestBitLength();
        }

        bool IBinaryInteger<TSelf>.TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            return TSelf.ToBase((TSelf)(object)this).TryWriteBigEndian(destination, out bytesWritten);
        }

        bool IBinaryInteger<TSelf>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            return TSelf.ToBase((TSelf)(object)this).TryWriteLittleEndian(destination, out bytesWritten);
        }

        static TSelf IShiftOperators<TSelf, int, TSelf>.operator <<(TSelf value, int shiftAmount) {
            return TSelf.FromBase(TSelf.ToBase(value) << shiftAmount);
        }

        static TSelf IShiftOperators<TSelf, int, TSelf>.operator >>(TSelf value, int shiftAmount) {
            return TSelf.FromBase(TSelf.ToBase(value) >> shiftAmount);
        }

        static TSelf IShiftOperators<TSelf, int, TSelf>.operator >>>(TSelf value, int shiftAmount) {
            return TSelf.FromBase(TSelf.ToBase(value) >>> shiftAmount);
        }
    }
}

namespace UltimateOrb.Runtime.InteropServices {

    public struct UnsafeEndiannessAwareUpperLowerPair<TUpper, TLower> {

        LittleEndian @union;

        [UnscopedRef]
        public ref TUpper Upper {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref (!BitConverter.IsLittleEndian ? ref Unsafe.As<LittleEndian, BigEndian>(ref @union).Upper : ref @union.Upper);
        }

        [UnscopedRef]
        public ref TLower Lower {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref (!BitConverter.IsLittleEndian ? ref Unsafe.As<LittleEndian, BigEndian>(ref @union).Lower : ref @union.Lower);
        }

        [StructLayout(LayoutKind.Sequential)]
        struct BigEndian {
            public TUpper Upper;
            public TLower Lower;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LittleEndian {
            public TLower Lower;
            public TUpper Upper;
        }
    }

    public readonly ref struct ReadOnlyUnsafeEndiannessAwareUpperLowerPair<TUpper, TLower> {

        readonly LittleEndian @union;

        [UnscopedRef]
        public ref readonly TUpper Upper {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref (!BitConverter.IsLittleEndian ? ref Unsafe.As<LittleEndian, BigEndian>(ref Unsafe.AsRef(in @union)).Upper : ref @union.Upper);
        }

        [UnscopedRef]
        public ref readonly TLower Lower {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref (!BitConverter.IsLittleEndian ? ref Unsafe.As<LittleEndian, BigEndian>(ref Unsafe.AsRef(in @union)).Lower : ref @union.Lower);
        }

        [StructLayout(LayoutKind.Sequential)]
        readonly struct BigEndian {
            public readonly TUpper Upper;
            public readonly TLower Lower;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LittleEndian {
            public readonly TLower Lower;
            public readonly TUpper Upper;
        }
    }
}

namespace UltimateOrb.Numerics {

    internal static class LongTags {

        internal readonly struct Hi2Lo { }

        internal readonly struct Lo2Hi { }
    }

    public readonly struct Long<TInt, TUInt>
        : IBinaryInteger<Long<TInt, TUInt>>, ISignedNumber<Long<TInt, TUInt>>
        , IBitwiseOperators<Long<TInt, TUInt>, Long<TInt, TUInt>, Long<TInt, TUInt>>, IBitwiseOperatorsDerivedTagged<Long<TInt, TUInt>, LongTags.Lo2Hi, TUInt, TInt, Long<TInt, TUInt>, TUInt, TInt, Long<TInt, TUInt>, TUInt, TInt>
        , IComparable, IComparableNongenericDerivedTagged<Long<TInt, TUInt>, LongTags.Hi2Lo, TInt, TUInt>
        , IComparable<Long<TInt, TUInt>>, IComparableDerivedTagged<Long<TInt, TUInt>, LongTags.Hi2Lo, TInt, TUInt>
        where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>
        where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt> {

        readonly UnsafeEndiannessAwareUpperLowerPair<TInt, TUInt> pair;

        [return: NotNullIfNotNull(nameof(value1)), NotNullIfNotNull(nameof(value2))]
        static Long<TInt, TUInt> IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TInt>.FromBase(TUInt? value1, TInt? value2) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.FromBase(value1, value2);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt? IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TInt>.ToBase1(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase1(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TInt? IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TInt>.ToBase2(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase2(value);
        }

        [return: NotNullIfNotNull(nameof(value1)), NotNullIfNotNull(nameof(value2))]
        static Long<TInt, TUInt> IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TInt, TUInt>.FromBase(TInt? value1, TUInt? value2) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.FromBase(value2, value1);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TInt? IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TInt, TUInt>.ToBase1(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase2(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt? IInterfaceDerivedTaggedSelfBase<Long<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TInt, TUInt>.ToBase2(Long<TInt, TUInt> value) {
            return InterfaceDerivedDefault<Long<TInt, TUInt>, TUInt, TInt>.ToBase1(value);
        }

        public static Long<TInt, TUInt> One => throw new NotImplementedException();

        public static int Radix => throw new NotImplementedException();

        public static Long<TInt, TUInt> Zero => throw new NotImplementedException();

        public static Long<TInt, TUInt> AdditiveIdentity => throw new NotImplementedException();

        public static Long<TInt, TUInt> MultiplicativeIdentity => throw new NotImplementedException();

        public static Long<TInt, TUInt> NegativeOne => throw new NotImplementedException();

        public static Long<TInt, TUInt> Abs(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsFinite(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNaN(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegative(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNormal(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositive(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPow2(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsZero(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Log2(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MaxMagnitude(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MaxMagnitudeNumber(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MinMagnitude(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> MinMagnitudeNumber(Long<TInt, TUInt> x, Long<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> PopCount(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> TrailingZeroCount(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToChecked<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(Long<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Long<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public int CompareTo(object? obj) {
            throw new NotImplementedException();
        }

        public int CompareTo(Long<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public bool Equals(Long<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public int GetByteCount() {
            throw new NotImplementedException();
        }

        public int GetShortestBitLength() {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator +(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator +(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator -(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator -(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator ++(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator --(Long<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator *(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator /(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator %(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator <<(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator >>(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static bool operator ==(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator !=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >=(Long<TInt, TUInt> left, Long<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static Long<TInt, TUInt> operator >>>(Long<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }
    }

    public readonly struct UInt128_Alt
        : IBinaryInteger<UInt128_Alt>, IUnsignedNumber<UInt128_Alt>
        , IBinaryIntegerDerived<UInt128_Alt, ULong<Int64, UInt64>> {

        readonly ULong<Int64, UInt64> value__;

        [return: NotNullIfNotNull(nameof(value))]
        public static UInt128_Alt FromBase(ULong<Int64, UInt64> value) {
            return InterfaceDerivedDefault<UInt128_Alt, ULong<Int64, UInt64>>.FromBase(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        public static ULong<Int64, UInt64> ToBase(UInt128_Alt value) {
            return InterfaceDerivedDefault<UInt128_Alt, ULong<Int64, UInt64>>.ToBase(value);
        }
    }




    public readonly struct ULong<TInt, TUInt>
        : IBinaryInteger<ULong<TInt, TUInt>>, IUnsignedNumber<ULong<TInt, TUInt>>
        , IBitwiseOperators<ULong<TInt, TUInt>, ULong<TInt, TUInt>, ULong<TInt, TUInt>>, IBitwiseOperatorsDerivedTagged<ULong<TInt, TUInt>, LongTags.Lo2Hi, TUInt, TUInt, ULong<TInt, TUInt>, TUInt, TUInt, ULong<TInt, TUInt>, TUInt, TUInt>
        , IComparable, IComparableNongenericDerivedTagged<ULong<TInt, TUInt>, LongTags.Hi2Lo, TUInt, TUInt>
        , IComparable<ULong<TInt, TUInt>>, IComparableDerivedTagged<ULong<TInt, TUInt>, LongTags.Hi2Lo, TUInt, TUInt>
        where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>
        where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt> {

        readonly UnsafeEndiannessAwareUpperLowerPair<TUInt, TUInt> pair;

        [return: NotNullIfNotNull(nameof(value1)), NotNullIfNotNull(nameof(value2))]
        static ULong<TInt, TUInt> IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TUInt>.FromBase(TUInt value1, TUInt? value2) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.FromBase(value1, value2);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TUInt>.ToBase1(ULong<TInt, TUInt> value) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.ToBase1(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt? IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Lo2Hi>, TUInt, TUInt>.ToBase2(ULong<TInt, TUInt> value) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.ToBase2(value);
        }

        [return: NotNullIfNotNull(nameof(value1)), NotNullIfNotNull(nameof(value2))]
        static ULong<TInt, TUInt> IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TUInt, TUInt>.FromBase(TUInt value1, TUInt value2) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.FromBase(value2, value1);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt? IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TUInt, TUInt>.ToBase1(ULong<TInt, TUInt> value) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.ToBase2(value);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static TUInt IInterfaceDerivedTaggedSelfBase<ULong<TInt, TUInt>, Tag<LongTags.Hi2Lo>, TUInt, TUInt>.ToBase2(ULong<TInt, TUInt> value) {
            return InterfaceDerivedDefault<ULong<TInt, TUInt>, TUInt, TUInt>.ToBase1(value);
        }
        public static ULong<TInt, TUInt> One => throw new NotImplementedException();

        public static int Radix => throw new NotImplementedException();

        public static ULong<TInt, TUInt> Zero => throw new NotImplementedException();

        public static ULong<TInt, TUInt> AdditiveIdentity => throw new NotImplementedException();

        public static ULong<TInt, TUInt> MultiplicativeIdentity => throw new NotImplementedException();

        public int GetByteCount() {
            throw new NotImplementedException();
        }

        public int GetShortestBitLength() {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> PopCount(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> TrailingZeroCount(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public bool TryWriteBigEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public bool TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            throw new NotImplementedException();
        }

        public static bool IsPow2(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Log2(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Abs(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsFinite(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInteger(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNaN(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegative(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNormal(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositive(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsZero(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> MaxMagnitude(ULong<TInt, TUInt> x, ULong<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> MaxMagnitudeNumber(ULong<TInt, TUInt> x, ULong<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> MinMagnitude(ULong<TInt, TUInt> x, ULong<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> MinMagnitudeNumber(ULong<TInt, TUInt> x, ULong<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToChecked<TOther>(ULong<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(ULong<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(ULong<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public bool Equals(ULong<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ULong<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool operator >(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >=(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <=(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator %(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator +(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator --(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator /(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator ==(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator !=(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator ++(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator *(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator -(ULong<TInt, TUInt> left, ULong<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator -(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator +(ULong<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator <<(ULong<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator >>(ULong<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }

        public static ULong<TInt, TUInt> operator >>>(ULong<TInt, TUInt> value, int shiftAmount) {
            throw new NotImplementedException();
        }
    }
}

namespace UltimateOrb.Mathematics.Exact {


    public readonly struct LongRational<TInt, TUInt> :
        IComparable<LongRational<TInt, TUInt>>,
        IEquatable<LongRational<TInt, TUInt>>,
        // ISpanFormattable,
        IMinMaxValue<LongRational<TInt, TUInt>>,
        // IUtf8SpanFormattable,
        // IParsable<LongRational<TInt, TUInt>>,
        // ISpanParsable<LongRational<TInt, TUInt>>,
        // IUtf8SpanParsable<LongRational<TInt, TUInt>>,
        IAdditionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IAdditiveIdentity<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IComparisonOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, bool>,
        IDecrementOperators<LongRational<TInt, TUInt>>,
        IDivisionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IEqualityOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, bool>,
        // IExponentialFunctions<LongRational<TInt, TUInt>>,
        // IFloatingPoint<LongRational<TInt, TUInt>>,
        IIncrementOperators<LongRational<TInt, TUInt>>,
        IModulusOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IMultiplicativeIdentity<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IMultiplyOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        // INumber<LongRational<TInt, TUInt>>,
        INumberBase<LongRational<TInt, TUInt>>,
        // IPowerFunctions<LongRational<TInt, TUInt>>,
        // IRootFunctions<LongRational<TInt, TUInt>>,
        ISignedNumber<LongRational<TInt, TUInt>>,
        ISubtractionOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IUnaryNegationOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>>,
        IUnaryPlusOperators<LongRational<TInt, TUInt>, LongRational<TInt, TUInt>> {
        public static LongRational<TInt, TUInt> MaxValue => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> MinValue => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> AdditiveIdentity => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> MultiplicativeIdentity => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> One => throw new NotImplementedException();

        public static int Radix => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> Zero => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> NegativeOne => throw new NotImplementedException();

        public static LongRational<TInt, TUInt> Abs(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsCanonical(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsComplexNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsEvenInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsFinite(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsImaginaryNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNaN(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegative(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNegativeInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsNormal(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositive(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsPositiveInfinity(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsRealNumber(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsSubnormal(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static bool IsZero(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MaxMagnitude(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MaxMagnitudeNumber(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MinMagnitude(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> MinMagnitudeNumber(LongRational<TInt, TUInt> x, LongRational<TInt, TUInt> y) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(string s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> Parse(string s, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToChecked<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(LongRational<TInt, TUInt> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LongRational<TInt, TUInt> result) {
            throw new NotImplementedException();
        }

        public int CompareTo(LongRational<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public bool Equals(LongRational<TInt, TUInt> other) {
            throw new NotImplementedException();
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            throw new NotImplementedException();
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator +(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator +(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator -(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator -(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator ++(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator --(LongRational<TInt, TUInt> value) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator *(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator /(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static LongRational<TInt, TUInt> operator %(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator ==(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator !=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator <=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }

        public static bool operator >=(LongRational<TInt, TUInt> left, LongRational<TInt, TUInt> right) {
            throw new NotImplementedException();
        }
    }
}
