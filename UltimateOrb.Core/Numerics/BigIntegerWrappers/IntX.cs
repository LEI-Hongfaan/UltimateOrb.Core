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
    public readonly struct Int<TBitSize> :
        IMinMaxValue<Int<TBitSize>>
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

        public static Int<TBitSize> MinValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new Int<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        public static Int<TBitSize> MaxValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new Int<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        readonly static BigInteger MinValueAsBigInteger = BigInteger.MinusOne << (BitSize - 1);

        readonly static BigInteger MaxValueAsBigInteger = ~MinValueAsBigInteger;

        Int(BigInteger value, Checked _ = default) {
            checked(0u - BooleanIntegerModule.LessThanOrEqual(BitSize, value.GetBitLength()).ToUnsignedUnchecked()).Ignore();
            Value = value;
        }

        Int(BigInteger value, Plain _) => Value = value;

        Int(BigInteger value, Unchecked _) {
            var t = MinValueAsBigInteger & value;
            if (0 > value.Sign) {
                t += MaxValueAsBigInteger;
            }
            Value = t;
        }

        static Int<TBitSize> Checked(BigInteger Value) {
            return new Int<TBitSize>(Value, default(Checked));
        }

        static Int<TBitSize> Unchecked(BigInteger Value) {
            return new Int<TBitSize>(Value, default(Unchecked));
        }

        public override bool Equals(object? obj) {
            if (obj is Int<TBitSize> value) {
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

        public static implicit operator BigInteger(Int<TBitSize> value) {
            return value.Value;
        }

        public static explicit operator Int<TBitSize>(BigInteger value) {
            return Checked(value);
        }

        public static explicit operator Int<TBitSize>(UInt<TBitSize> value) {
            return Checked(value);
        }

        public static Int<TBitSize> operator +(Int<TBitSize> value) {
            return value;
        }

        public static Int<TBitSize> operator -(Int<TBitSize> value) {
            return Checked(-value.Value);
        }

        public static Int<TBitSize> operator +(Int<TBitSize> first, Int<TBitSize> second) {
            return Checked(first.Value + second.Value);
        }

        public static Int<TBitSize> operator -(Int<TBitSize> first, Int<TBitSize> second) {
            return Checked(first.Value - second.Value);
        }

        public static Int<TBitSize> operator *(Int<TBitSize> first, Int<TBitSize> second) {
            return Checked(first.Value * second.Value);
        }

        public static Int<TBitSize> operator /(Int<TBitSize> first, Int<TBitSize> second) {
            return Checked(first.Value / second.Value);
        }

        public static Int<TBitSize> operator %(Int<TBitSize> first, Int<TBitSize> second) {
            return Checked(first.Value % second.Value);
        }

        public static explicit operator Int<TBitSize>(Int128 value) {
            return Checked(value.LoInt64Bits.ToUnsignedUnchecked() | (BigInteger)value.HiInt64Bits.ToSignedUnchecked());
        }

        [CLSCompliant(false)]
        public static explicit operator Int<TBitSize>(UInt128 value) {
            return Checked(value.LoInt64Bits.ToUnsignedUnchecked() | (BigInteger)value.HiInt64Bits.ToUnsignedUnchecked());
        }
    }
}
