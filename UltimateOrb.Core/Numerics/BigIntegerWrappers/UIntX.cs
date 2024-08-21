using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Functional.DataTypes;
using UltimateOrb.Runtime.CompilerServices.TypeTokens;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics.BigIntegerWrappers {
    using static ConstructorTags;

    [Serializable]
    [CLSCompliant(false)]
    public readonly struct UInt<TBitSize> :
        IMinMaxValue<UInt<TBitSize>>
        where TBitSize :
            struct,
            IConstantDataType<int> {

        readonly BigInteger Value;

        static int BitSize {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                var value = default(TBitSize).Value;
                value.ThrowOnLessThan(1);
                return value;
            }
        }

        public static UInt<TBitSize> MinValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new UInt<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        public static UInt<TBitSize> MaxValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new UInt<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        readonly static BigInteger MinValueAsBigInteger = BigInteger.MinusOne << (BitSize - 1);

        readonly static BigInteger MaxValueAsBigInteger = ~MinValueAsBigInteger;

        UInt(BigInteger value, Checked _ = default) {
            checked(0u - BooleanIntegerModule.LessThanOrEqual(BitSize, value.GetBitLength()).ToUnsignedUnchecked()).Ignore();
            Value = value;
        }

        UInt(BigInteger value, Plain _) => Value = value;

        UInt(BigInteger value, Unchecked _) {
            var t = MinValueAsBigInteger & value;
            if (0 > value.Sign) {
                t += MaxValueAsBigInteger;
            }
            Value = t;
        }

        static UInt<TBitSize> Checked(BigInteger Value) {
            return new UInt<TBitSize>(Value, default(Checked));
        }

        static UInt<TBitSize> Unchecked(BigInteger Value) {
            return new UInt<TBitSize>(Value, default(Unchecked));
        }

        public override bool Equals(object? obj) {
            if (obj is UInt<TBitSize> value) {
                return Value.Equals(value.Value);
            }
            return false;
        }

        public override int GetHashCode() {
            return Value.GetHashCode();
        }

        public override string? ToString() {
            return Value.ToString();
        }

        public static implicit operator BigInteger(UInt<TBitSize> value) {
            return value.Value;
        }

        public static explicit operator UInt<TBitSize>(BigInteger value) {
            return Checked(value);
        }

        public static explicit operator UInt<TBitSize>(Int<TBitSize> value) {
            return Checked(value);
        }

        public static UInt<TBitSize> operator +(UInt<TBitSize> value) {
            return value;
        }

        public static UInt<TBitSize> operator -(UInt<TBitSize> value) {
            return Checked(-value.Value);
        }

        public static UInt<TBitSize> operator +(UInt<TBitSize> first, UInt<TBitSize> second) {
            return Checked(first.Value + second.Value);
        }

        public static UInt<TBitSize> operator -(UInt<TBitSize> first, UInt<TBitSize> second) {
            return Checked(first.Value - second.Value);
        }

        public static UInt<TBitSize> operator *(UInt<TBitSize> first, UInt<TBitSize> second) {
            return Checked(first.Value * second.Value);
        }

        public static UInt<TBitSize> operator /(UInt<TBitSize> first, UInt<TBitSize> second) {
            return Checked(first.Value / second.Value);
        }

        public static UInt<TBitSize> operator %(UInt<TBitSize> first, UInt<TBitSize> second) {
            return Checked(first.Value % second.Value);
        }

        public static explicit operator UInt<TBitSize>(Int128 value) {
            return Checked(value.LoInt64Bits.ToUnsignedUnchecked() | (BigInteger)value.HiInt64Bits.ToSignedUnchecked());
        }

        public static explicit operator UInt<TBitSize>(UInt128 value) {
            return Checked(value.LoInt64Bits.ToUnsignedUnchecked() | (BigInteger)value.HiInt64Bits.ToUnsignedUnchecked());
        }
    }
}
