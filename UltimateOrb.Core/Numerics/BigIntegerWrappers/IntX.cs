using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UltimateOrb.Functional.DataTypes;
using UltimateOrb.Numerics.DataTypes;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics.BigIntegerWrappers {
    using static ConstructorTags;

    [Experimental("UoWIP_GenericMath")]
    internal readonly struct TXIntInternal<TBitSize>
        where TBitSize : IConstant<TBitSize, int> {
       
        private static readonly int s_BitSize = GetCheckedBitSize();

        private static int GetCheckedBitSize() {
            var value = TBitSize.Value;
            value.ThrowOnLessThan(1);
            return value;
        }

        public static int BitSize {

            get => s_BitSize;
        }

        public readonly static BigInteger SignedMinValueAsBigInteger = BigInteger.MinusOne << (BitSize - 1);

        public readonly static BigInteger SignedMaxValueAsBigInteger = ~SignedMinValueAsBigInteger;

        public static BigInteger UnsignedMinValueAsBigInteger => BigInteger.Zero;

        public readonly static BigInteger ModulusAsBigInteger = BigInteger.One << BitSize;

        public readonly static BigInteger ModulusNegatedAsBigInteger = -ModulusAsBigInteger;

        public readonly static BigInteger UnsignedMaxValueAsBigInteger = ModulusAsBigInteger - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int NormalizeShiftCount(int shiftCount) {
            if (int.IsPow2(BitSize)) {
                return shiftCount & (int.Log2(BitSize) - 1);
            }
            {
                var s = shiftCount % BitSize;
                return s < 0 ? s + BitSize : s;
            }
        }
    }

    [Experimental("UoWIP_GenericMath")]
    [Serializable]
    public readonly struct Int<TBitSize> :
        //IBinaryInteger<Int<TBitSize>>,
        //ISignedNumber<Int<TBitSize>>,
        IMinMaxValue<Int<TBitSize>>
        where TBitSize :
            IConstant<TBitSize, int> {

        readonly BigInteger Value;

        static int BitSize {

            get => TXIntInternal<TBitSize>.BitSize;
        }


        public static BigInteger MinValueAsBigInteger {

            get => TXIntInternal<TBitSize>.SignedMinValueAsBigInteger;
        }

        public static BigInteger MaxValueAsBigInteger {

            get => TXIntInternal<TBitSize>.SignedMaxValueAsBigInteger;
        }

        public static Int<TBitSize> MinValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new Int<TBitSize>(MinValueAsBigInteger, default(Plain));
        }

        public static Int<TBitSize> MaxValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => new Int<TBitSize>(MinValueAsBigInteger, default(Plain));
        }


        Int(BigInteger value, Checked _ = default) {
            UltimateOrb.Utilities.ThrowHelper.ThrowOnLessThanOrEqual(BitSize, value.GetBitLength().ToUnsignedUnchecked());
            Value = value;
        }

        Int(BigInteger value, Plain _) => Value = value;

        Int(BigInteger value, Unchecked _) {
            var t = TXIntInternal<TBitSize>.UnsignedMaxValueAsBigInteger & value;
            if (BigInteger.IsNegative(value)) {
                t |= TXIntInternal<TBitSize>.ModulusNegatedAsBigInteger;
            }
            Value = t;
        }

        static Int<TBitSize> Checked(BigInteger Value) {
            return new Int<TBitSize>(Value, default(Checked));
        }

        static Int<TBitSize> Unchecked(BigInteger Value) {
            return new Int<TBitSize>(Value, default(Unchecked));
        }

        public Int(int value) {
            if (BitSize >= sizeof(int) * 8) {
                this = new Int<TBitSize>(new BigInteger(value), default(Plain));
            } else {
                this = new Int<TBitSize>(new BigInteger(value), default(Checked));
            }
        }

        [CLSCompliant(false)]
        public Int(uint value) {
            if (BitSize > sizeof(uint) * 8) {
                this = new Int<TBitSize>(new BigInteger(value), default(Plain));
            } else {
                this = new Int<TBitSize>(new BigInteger(value), default(Checked));
            }
        }

        public Int(long value) {
            if (BitSize >= sizeof(long) * 8) {
                this = new Int<TBitSize>(new BigInteger(value), default(Plain));
            } else {
                this = new Int<TBitSize>(new BigInteger(value), default(Checked));
            }
        }

        [CLSCompliant(false)]
        public Int(ulong value) {
            if (BitSize > sizeof(ulong) * 8) {
                this = new Int<TBitSize>(new BigInteger(value), default(Plain));
            } else {
                this = new Int<TBitSize>(new BigInteger(value), default(Checked));
            }
        }

        public Int(float value) {
            this = new Int<TBitSize>(new BigInteger(value), default(Checked));
        }

        public Int(double value) : this(new BigInteger(value), default(Checked)) {
        }

        public Int(decimal value) : this(new BigInteger(value), default(Checked)) {
        }

        /// <summary>
        /// Creates a BigInteger from a little-endian twos-complement byte array.
        /// </summary>
        /// <param name="value"></param>
        [CLSCompliant(false)]
        public Int(byte[] value) {
            this = new Int<TBitSize>(new BigInteger(value), default(Checked));
        }

        public Int(ReadOnlySpan<byte> value, bool isUnsigned = false, bool isBigEndian = false) {
            this = new Int<TBitSize>(new BigInteger(value, isUnsigned, isBigEndian), default(Checked));
        }

        public override bool Equals(object? obj) {
            return obj is Int<TBitSize> value ? Value.Equals(value.Value) : false;
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
            return Unchecked(value);
        }

        public static explicit operator checked Int<TBitSize>(BigInteger value) {
            return Checked(value);
        }

        public static explicit operator Int<TBitSize>(UInt<TBitSize> value) {
            return Checked((BigInteger)value);
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
            return Checked(value.GetLowPart().ToUnsignedUnchecked() | (BigInteger)value.GetHighPart().ToSignedUnchecked());
        }

        [CLSCompliant(false)]
        public static explicit operator Int<TBitSize>(UInt128 value) {
            return Checked(value.GetLowPart().ToUnsignedUnchecked() | (BigInteger)value.GetHighPart().ToUnsignedUnchecked());
        }

#if NET7_0_OR_GREATER
        public static explicit operator Int<TBitSize>(System.Int128 value) {
            return Checked(value.GetLowPart().ToUnsignedUnchecked() | (BigInteger)value.GetHighPart().ToSignedUnchecked());
        }

        [CLSCompliant(false)]
        public static explicit operator Int<TBitSize>(System.UInt128 value) {
            return Checked(value.GetLowPart().ToUnsignedUnchecked() | (BigInteger)value.GetHighPart().ToUnsignedUnchecked());
        }
#endif
    }
}
