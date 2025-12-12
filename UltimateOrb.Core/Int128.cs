using Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Numerics;
using UltimateOrb.Utilities;

namespace UltimateOrb {
    using static global::Internal.System.Converter;
    using static global::Internal.System.IConvertibleModule;
    using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
    using static UltimateOrb.Utilities.ThrowHelper;
    using static UltimateOrb.XInt128Helpers;
    using HInt64 = Int64;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;
    using OInt128 = UInt128;
    using XInt128 = Int128;

    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Interoperability", "CA1413:AvoidNonpublicFieldsInComVisibleValueTypes")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    [System.SerializableAttribute()]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 16)]
    public readonly partial struct Int128
        : IEquatable<XInt128>, IComparable<XInt128>, IComparable
#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        , IConvertible, IFormattable
#endif
    {

#if NET10_0_OR_GREATER
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64 d0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64 d1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [UnscopedRef]
        private readonly ref readonly UInt64 lo {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref BitConverter.IsLittleEndian ? ref d0 : ref d1;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [UnscopedRef]
        private readonly ref readonly HInt64 hi {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref Unsafe.As<UInt64, HInt64>(ref Unsafe.AsRef(in BitConverter.IsLittleEndian ? ref d1 : ref d0));
        }
#else
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64 lo;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HInt64 hi;
#endif

        [System.Diagnostics.Contracts.PureAttribute()]
        public Int64 LoInt64Bits {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return unchecked((Int64)this.lo);
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public Int64 HiInt64Bits {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return unchecked((Int64)this.hi);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal Int128(UInt64 lo, HInt64 hi) {
#if NET10_0_OR_GREATER
            this.d0 = BitConverter.IsLittleEndian ? lo : unchecked((UInt64)hi);
            this.d1 = BitConverter.IsLittleEndian ? unchecked((UInt64)hi) : lo;
#else
            this.lo = lo;
            this.hi = hi;
#endif
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 FromBits(Int64 lo, Int64 hi) {
            return new XInt128(unchecked((UInt64)lo), unchecked((HInt64)hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 FromBits(UInt64 lo, HInt64 hi) {
            return new XInt128(unchecked((UInt64)lo), unchecked((HInt64)hi));
        }

        #region Standard values
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Zero {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return default(XInt128);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsZero {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 0 == (this.lo | unchecked((UInt64)this.hi));
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 One {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(1, 0);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsOne {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 1 == this.lo && 0 == this.hi;
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 MinusOne {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(UInt64.MaxValue, unchecked((Int64)UInt64.MaxValue));
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsMinusOne {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return UInt64.MaxValue == this.lo && unchecked((Int64)UInt64.MaxValue) == this.hi;
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 MaxValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(UInt64.MaxValue, HInt64.MaxValue);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsMaxValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return UInt64.MaxValue == this.lo && HInt64.MaxValue == this.hi;
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 MinValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(UInt64.MinValue, HInt64.MinValue);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsMinValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return UInt64.MinValue == this.lo && HInt64.MinValue == this.hi;
            }
        }
        #endregion

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsEven {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return (unchecked((int)this.lo) & 1).AsBooleanUnsafe();
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsNegative {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 0 > this.hi;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public int Sign {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                var hi = this.hi;
                return 0 > hi ? -1 : ((hi == 0 && this.lo == 0) ? 0 : 1);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool CanConvertToUInt64 {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 0 == this.hi;
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool CanConvertToUInt32 {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return CanConvertToUIntN(32);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool CanConvertToUInt16 {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return CanConvertToUIntN(16);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool CanConvertToUIntN(int n) {
            //TODO
            if ((128 - 1) <= n) {
                return 0 > this.hi;
            }
            if (n <= 0) {
                return this.IsZero;
            }
            if (n <= 64) {
                if (64 == n) {
                    return this.CanConvertToUInt64;
                }
                return 0 == this.hi && 0 == this.lo >> (64 - n);
            } else {
                return 0 == this.hi >> (64 - n);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int TestBit(XInt128 value, int index) {
            if (0 <= index && 128 > index) {
                if (index < 64) {
                    return 1 & unchecked((int)(value.lo >> index));
                }
                return 1 & unchecked((int)(value.hi >> index));
            }
            return 0;
        }

        #region Bitwise operations
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 OnesComplement(XInt128 value) {
            return new XInt128(~value.lo, ~value.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator ~(XInt128 value) {
            return new XInt128(~value.lo, ~value.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 BitwiseOr(XInt128 first, XInt128 second) {
            return new XInt128(first.lo | second.lo, first.hi | second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator |(XInt128 first, XInt128 second) {
            return new XInt128(first.lo | second.lo, first.hi | second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Xor(XInt128 first, XInt128 second) {
            return new XInt128(first.lo ^ second.lo, first.hi ^ second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator ^(XInt128 first, XInt128 second) {
            return new XInt128(first.lo ^ second.lo, first.hi ^ second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 BitwiseAnd(XInt128 first, XInt128 second) {
            return new XInt128(first.lo & second.lo, first.hi & second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator &(XInt128 first, XInt128 second) {
            return new XInt128(first.lo & second.lo, first.hi & second.hi);
        }
        #endregion

        #region Shift operations        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator <<(XInt128 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftLeft(value.lo, value.hi, count, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_LeftShift(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.ShiftLeft(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator >>(XInt128 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRight(value.lo, value.hi, count, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_RightShift(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.ShiftRight(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }
        #endregion

        #region Comparisons
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool Equals(XInt128 other) {
            return this.lo == other.lo && this.hi == other.hi;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public override bool Equals(object? obj) {
            if (obj is XInt128 value) {
                return this.Equals(value);
            }
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        bool IEquatable<XInt128>.Equals(XInt128 other) {
            return this.lo == other.lo && this.hi == other.hi;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public override int GetHashCode() {
            return HashCodeHelper.GetHashCode(this.lo ^ unchecked((UInt64)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public int CompareTo(XInt128 other) {
            return Numerics.DoubleArithmetic.Compare(this.lo, this.hi, other.lo, other.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public int CompareTo(object? obj) {
            if (obj is null) {
                return 1;
            }
            if (obj is XInt128 value) {
                return CompareTo(value);
            }

            static string? getMessageForCompareToArgumentException() {
                try {
                    ((Int64)(-1)).CompareTo(Type.EmptyTypes).Ignore(); // Generate an InvalidCastException.
                } catch (InvalidCastException ex) {
                    // Catch the InvalidCastException and extract the value of Message property.
                    try {
                        // So everyone can use their language.
                        return ex.Message.Replace(nameof(Int64), nameof(Int128));
                    } catch (Exception) {
                    }
                } catch (Exception) {
                }
                return null;
            }

            [DoesNotReturn]
            static ArgumentException ThrowArgumentException() {
                throw new InvalidCastException(getMessageForCompareToArgumentException());
            }

            throw ThrowArgumentException();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        int IComparable<XInt128>.CompareTo(XInt128 other) {
            return Numerics.DoubleArithmetic.Compare(this.lo, this.hi, other.lo, other.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator ==(XInt128 first, XInt128 second) {
            return (first.lo == second.lo) && (first.hi == second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator !=(XInt128 first, XInt128 second) {
            return !(first == second);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int Compare(XInt128 first, XInt128 second) {
            return Numerics.DoubleArithmetic.Compare(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator >(XInt128 first, XInt128 second) {
            return Numerics.DoubleArithmetic.GreaterThan(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator >=(XInt128 first, XInt128 second) {
            return Numerics.DoubleArithmetic.GreaterThanOrEqual(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator <(XInt128 first, XInt128 second) {
            return Numerics.DoubleArithmetic.LessThan(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator <=(XInt128 first, XInt128 second) {
            return Numerics.DoubleArithmetic.LessThanOrEqual(first.lo, first.hi, second.lo, second.hi);
        }
        #endregion

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 AsUnsigned() {
            return new OInt128(unchecked((UInt64)this.lo), unchecked((UInt64)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToUnsignedUnchecked() {
            return new OInt128(unchecked((UInt64)this.lo), unchecked((UInt64)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToUnsignedChecked() {
            return new OInt128(unchecked((UInt64)this.lo), checked((UInt64)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToUnsigned() {
            return new OInt128(unchecked((UInt64)this.lo), checked((UInt64)this.hi));
        }

        #region Numeric Conversions
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(OInt128 value) {
            return value.ToSignedChecked();
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(OInt128 value) {
            return value.ToSignedUnchecked();
        }
#endif

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(sbyte value) {
            return new XInt128(unchecked((UInt64)(Int64)value), value >> (8 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(Int16 value) {
            return new XInt128(unchecked((UInt64)(Int64)value), value >> (16 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(Int32 value) {
            return new XInt128(unchecked((UInt64)(Int64)value), value >> (32 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(Int64 value) {
            return new XInt128(unchecked((UInt64)(Int64)value), value >> (64 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(char value) {
            return new XInt128(value, 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(byte value) {
            return new XInt128(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt16 value) {
            return new XInt128(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt32 value) {
            return new XInt128(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt64 value) {
            return new XInt128(value, 0);
        }

#if NET7_0_OR_GREATER
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(System.Int128 value) {
            return new XInt128(lo: value.GetLowPart(), hi: value.GetHighPart());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator System.Int128(XInt128 value) {
            return new System.Int128(upper: unchecked((UInt64)value.GetHighPart()), lower: value.GetLowPart());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(System.UInt128 value) {
            return new XInt128(lo: value.GetLowPart(), hi: checked((HInt64)value.GetHighPart()));
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(System.UInt128 value) {
            return new XInt128(lo: value.GetLowPart(), hi: unchecked((HInt64)value.GetHighPart()));
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            System.UInt128(XInt128 value) {
            return new System.UInt128(upper: checked((UInt64)value.GetHighPart()), lower: value.GetLowPart());
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator System.UInt128(XInt128 value) {
            return new System.UInt128(upper: unchecked((UInt64)value.GetHighPart()), lower: value.GetLowPart());
        }
#endif
#endif

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator sbyte(XInt128 value) {
            (checked(0 - unchecked(value.hi - (unchecked((Int64)value.lo) >> (64 - 1))))).Ignore(); // check overflow
            return checked((sbyte)unchecked((Int64)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int16(XInt128 value) {
            (checked(0 - unchecked(value.hi - (unchecked((Int64)value.lo) >> (64 - 1))))).Ignore(); // check overflow
            return checked((Int16)unchecked((Int64)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int32(XInt128 value) {
            (checked(0 - unchecked(value.hi - (unchecked((Int64)value.lo) >> (64 - 1))))).Ignore(); // check overflow
            return checked((Int32)unchecked((Int64)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int64(XInt128 value) {
            (checked(0 - unchecked(value.hi - (unchecked((Int64)value.lo) >> (64 - 1))))).Ignore(); // check overflow
            return unchecked((Int64)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator char(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((char)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator byte(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((byte)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt16(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((UInt16)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt32(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((UInt32)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt64(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return value.lo;
        }

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(Half value) {
            return (XInt128)unchecked((double)value);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(Single value) {
            return (XInt128)unchecked((double)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(double value) {
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            // const Int64 SignMask = unchecked((Int64)checked((Int64)1u << (ExponentBitSize + FractionBitSize)));
            const Int64 ExponentMask = unchecked((Int64)checked((((Int64)1u << ExponentBitSize) - 1u) << FractionBitSize));
            const Int64 FractionMask = unchecked((Int64)checked(((Int64)1u << FractionBitSize) - 1u));

            var b = BitConverter.DoubleToInt64Bits(value);
            var e = checked((int)(ExponentMask >> FractionBitSize)) & unchecked((int)(b >> FractionBitSize));
            if (e >= checked(ExponentBias - 1)) {
                b.ThrowOnNegative();
                checked(checked(ExponentBias - 1 + 128) - e).Ignore();
                if (e < checked(ExponentBias - 1 + 128)) {
                    e = unchecked(e - (FractionBitSize + ExponentBias));
                    var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                    var hi = (Int64)0;
                    if (e > 0) {
                        lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                    } else if (e < 0) {
                        // var ol = (UInt64)0;
                        // ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                        // Do NOT uncomment. (int)3.5 == 3
                        /*
                        // IEEE Std 754-2008 roundTiesToEven
                        if (unchecked((UInt64)Int64.MinValue) < ol || (unchecked((UInt64)Int64.MinValue) == ol && 0 != (1 & lo))) {
                            lo = unchecked(1 + lo);
                        }
                        */
                    }
                    if (0 > b) {
                        lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                    }
                    return new XInt128(lo, hi);
                }
                // check overflow
                checked(0 - unchecked((UInt64)((Int64)0xC7E0000000000000 - b))).Ignore();
                return MinValue;
            }
            return Zero;
        }

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Half(XInt128 value) {
            unchecked {
                var lo = (int)value.lo;
                var mask = lo >> (SizeOfModule.BitSizeOf<int>() - 1);
                var v = (lo ^ mask - mask).ToUnsignedUnchecked();
                var sign = mask & (int)Int16.MinValue;
                var t = value + 65519;
                return BitConverter.Int16BitsToHalf((Int16)(sign + ((t.ToUnsignedUnchecked() > 65519 * 2 || value.hi.ToSignedUnchecked() != mask) ? HalfHelpers.HalfPositiveInfinityBits : HalfHelpers.ToHalfPartial(v))));
            }
        }
#endif

        // Make the conversion explicit due to precision loss may be significant.
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Single(XInt128 value) {
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            const int BitSizeTo = 32;
            const int ExponentBitSizeTo = 8;
            // const int ExponentBiasTo = unchecked(checked((1 << (ExponentBitSizeTo - 1))) - 1);
            const int SignBitSizeTo = 1;
            const int FractionBitSizeTo = checked(BitSizeTo - SignBitSizeTo - ExponentBitSizeTo);

            var lo = value.lo;
            var hi = value.hi;
            if (0 != hi) {
                var s = (0 > hi ? Int64.MinValue : 0);
                if (0 > hi) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)hi));
                var c = 0;
                for (var tmp = hi; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((Int64)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                var lo0 = lo & unchecked(((UInt64)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSizeTo), out hi);
                // IEEE Std 754-2008 roundTiesToEven
                if (unchecked((UInt64)Int64.MinValue) < lo || (unchecked((UInt64)Int64.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                    unchecked {
                        ++hi;
                    }
                }
                s = unchecked(s + (hi << checked(FractionBitSize - FractionBitSizeTo)));
                return unchecked((Single)BitConverter.Int64BitsToDouble(unchecked((Int64)s)));
            }
            return ToSingle(lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator double(XInt128 value) {
            return unchecked(0 > value.hi ? -(double)(OInt128)(-value) : (double)(OInt128)value);
            /*
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            var lo = value.lo;
            var hi = value.hi;
            if (0 != hi) {
                var s = (0 > hi ? Int64.MinValue : 0);
                if (0 > hi) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)hi));
                var c = 0;
                if (0 != hi) {
                    for (var tmp = hi; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                        unchecked {
                            ++c;
                        }
                    }
                    s |= unchecked((Int64)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                    lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                    var lo0 = lo & unchecked(((UInt64)1 << checked(BitSize - FractionBitSize)) - 1);
                    lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSize), out hi);
                    // IEEE Std 754-2008 roundTiesToEven
                    if (unchecked((UInt64)Int64.MinValue) < lo || (unchecked((UInt64)Int64.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                        unchecked {
                            ++hi;
                        }
                    }
                    s = unchecked(s + hi);
                    return BitConverter.Int64BitsToDouble(s);
                }
                return -unchecked((double)lo);
            }
            return unchecked((double)lo);
            */
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator global::System.Decimal(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.AbsSignedAsUnsigned(value.lo, value.hi, out UInt64 hi);
            checked((UInt32)hi).Ignore(); // check overflow
            return new decimal(unchecked((Int32)(lo >> (32 * 0))), unchecked((Int32)(lo >> (32 * 1))), unchecked((Int32)(hi >> (32 * 0))), 0 > value.hi, 0);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        // TODO
        private static readonly XInt128[] PowersOf10 = GetPowersOf10();

        private static XInt128[] GetPowersOf10() {
            var lo = (UInt64)1;
            var hi = (HInt64)0;
            var r = new XInt128[39];
            for (var i = 1; r.Length > i; ++i) {
                lo = Numerics.DoubleArithmetic.MultiplyUnchecked(10, 0, lo, hi, out hi);
                r[i] = new XInt128(lo, hi);
            }
            return r;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(global::System.Decimal value) {
            // TODO: Struct layout of global::System.Decimal may vary...
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            var a = (stackalloc UInt32[4]);
            global::System.Decimal.GetBits(value, MemoryMarshal.Cast<UInt32, Int32>(a));
#else
            var a = (((object)global::System.Decimal.GetBits(value)) as UInt32[])!; // CLI actually allows such sign casts.
#endif
            var lo = a[0] | ((UInt64)a[1] << 32);
            var hi = (HInt64)a[2];
            var f = a[3];
            var scale = (f >> 16) & 0x1F;
            var d = PowersOf10[scale];
            lo = Numerics.DoubleArithmetic.DivRemUnchecked(lo, hi, d.lo, d.hi, out UInt64 r_lo, out HInt64 r_hi, out hi);
            // Note: r <= (a[0] | ((UInt64)a[1] << 32), (Int64)a[2]) .
            //   ==> r: Left shift (as a multiplication) will not lead to an arithmetic overflow.
            r_lo = Numerics.DoubleArithmetic.ShiftLeft(r_lo, r_hi, out r_hi);
            var c = Numerics.DoubleArithmetic.Compare(d.lo, d.hi, r_lo, r_hi);
            // 'Banker's rounding' (same as IEEE Std 754-2008 roundTiesToEven)
            if (c > 0 || (0 == c && (0 != (1 & lo)))) {
                lo = Numerics.DoubleArithmetic.IncreaseUnsigned(lo, hi, out hi);
            }
            if (0 > f) {
                // Will not overflow
                // lo = MathEx.NegateSigned(lo, hi, out hi);
                lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
            }
            return new XInt128(lo, hi);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public byte[] ToBigIntegerByteArray() {
            var a = new byte[128 / 8];
            {
                var t = this.lo;
                var j = 0;
                for (var i = 0; (64 / 8) > i; ++i) {
                    a[(64 / 8) * j + i] = unchecked((byte)(t >> (8 * i)));
                }
            }
            {
                var t = this.hi;
                var j = 1;
                for (var i = 0; (64 / 8) > i; ++i) {
                    a[(64 / 8) * j + i] = unchecked((byte)(t >> (8 * i)));
                }
            }
            return a;
        }
        #endregion

        #region Basic arithmetic operations
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Plus(XInt128 value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator +(XInt128 value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Negate(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.NegateSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator -(XInt128 value) {
#else
        public static XInt128 operator checked -(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.NegateUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_UnaryNegationUnchecked(XInt128 value) {
#else
        public static XInt128 operator -(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.NegateUnchecked(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Increase(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 IncreaseUnsigned(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator ++(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_IncrementUnchecked(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseUnchecked(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static XInt128 Decrease(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 DecreaseUnsigned(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator --(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_DecrementUnchecked(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnchecked(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Add(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.AddSigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator +(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.AddSigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_AdditionUnchecked(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.AddUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Subtract(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.SubtractSigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator -(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.SubtractSigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_SubtractionUnchecked(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.SubtractUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Multiply(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.MultiplySigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator *(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.MultiplySigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_MultiplyUnchecked(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.MultiplyUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 DivRem(XInt128 first, XInt128 second, out XInt128 remainder) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out UInt64 remainder_lo, out HInt64 remainder_hi, out HInt64 hi);
            remainder = new XInt128(remainder_lo, remainder_hi);
            return new XInt128(lo, hi);
        }

        /*
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static (XInt128 Quotient, XInt128 Remainder) DivRem(XInt128 first, XInt128 second, out XInt128 remainder) {
            var lo = MathEx.DivRem(first.lo, first.hi, second.lo, second.hi, out UInt64 remainder_lo, out HInt64 remainder_hi, out HInt64 hi);
            return (new XInt128(lo, hi), new XInt128(remainder_lo, remainder_hi));
        }
        */

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Division(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_IntegerDivision(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_IntegerDivisionUnchecked(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.DivideUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Remainder(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Remainder(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator %(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Remainder(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        /*
        public static double op_FloatingPointDivisionDouble(XInt128 first, XInt128 second) {            
        }
        */
        #endregion

        #region IConvertible
        public TypeCode GetTypeCode() {
            return TypeCode.Object;
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        bool IConvertible.ToBoolean(IFormatProvider? provider) {
            return (0 != this).ToBoolean(provider);
        }

        Char IConvertible.ToChar(IFormatProvider? provider) {
            if ((long)Char.MinValue <= this && this <= (long)Char.MaxValue) {
                return unchecked((Char)this.lo).ToChar(provider);
            }
            return ((long)Char.MinValue - 1).ToChar(provider); // Let the underlying standard libraries raise the exception.
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            if ((long)sbyte.MinValue <= this && this <= (long)sbyte.MaxValue) {
                return unchecked((sbyte)this.lo).ToSByte(provider);
            }
            return ((long)sbyte.MinValue - 1).ToSByte(provider); // Let the underlying standard libraries raise the exception.
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            if ((long)byte.MinValue <= this && this <= (long)byte.MaxValue) {
                return unchecked((byte)this.lo).ToByte(provider);
            }
            return ((long)byte.MinValue - 1).ToByte(provider); // Let the underlying standard libraries raise the exception.
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            if ((long)Int16.MinValue <= this && this <= (long)Int16.MaxValue) {
                return unchecked((Int16)this.lo).ToInt16(provider);
            }
            return ((long)Int16.MinValue - 1).ToInt16(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            if ((long)UInt16.MinValue <= this && this <= (long)UInt16.MaxValue) {
                return unchecked((UInt16)this.lo).ToUInt16(provider);
            }
            return ((long)UInt16.MinValue - 1).ToUInt16(provider); // Let the underlying standard libraries raise the exception.
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            if ((long)Int32.MinValue <= this && this <= (long)Int32.MaxValue) {
                return unchecked((Int32)this.lo).ToInt32(provider);
            }
            return ((long)Int32.MinValue - 1).ToInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            if ((long)UInt32.MinValue <= this && this <= (long)UInt32.MaxValue) {
                return unchecked((UInt32)this.lo).ToUInt32(provider);
            }
            return ((long)UInt32.MinValue - 1).ToUInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            if (Int64.MinValue <= this && this <= Int64.MaxValue) {
                return unchecked((Int64)this.lo).ToInt64(provider);
            }
            return ((long)Int32.MinValue - 1).ToInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            if (UInt64.MinValue <= this && this <= UInt64.MaxValue) {
                return unchecked((UInt64)this.lo).ToUInt64(provider);
            }
            return ((long)UInt32.MinValue - 1).ToUInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return ((Single)this).ToSingle(provider);
        }

        Double IConvertible.ToDouble(IFormatProvider? provider) {
            return ((Double)this).ToDouble(provider);
        }

        Decimal IConvertible.ToDecimal(IFormatProvider? provider) {
            return ((Decimal)this).ToDecimal(provider);
        }

        private static string? getMessageForInvalidCastException(IFormatProvider? provider) {
            try {
                ((Int64)(-1)).ToDateTime(provider).Ignore(); // Generate an InvalidCastException.
            } catch (InvalidCastException ex) {
                // Catch the InvalidCastException and extract the value of Message property.
                try {
                    // So everyone can use their language.
                    return ex.Message.Replace(nameof(Int64), nameof(Int128));
                } catch (Exception) {
                }
            } catch (Exception) {
            }
            return null;
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            throw new InvalidCastException(getMessageForInvalidCastException(provider));
        }

        string IConvertible.ToString(IFormatProvider? provider) {
            return this.ToString(null, provider);
        }

        object IConvertible.ToType(Type type, IFormatProvider? provider) {
            ArgumentNullException.ThrowIfNull(type);
            return ConvertInternal.DefaultToType(this, type, provider);
        }
#endif
        #endregion

        /// <summary>
        ///     <para>Parses a signed integer.</para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     <para>The Number must be in format <c>[1-9][0-9]*</c>.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.None)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool TryParseCStyleNormalizedI128(string s, out XInt128 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                var sign = ('-' == c);
                UInt64 lo;
                UInt64 hi;
                if (sign) {
                    if (s.Length <= i) {
                        goto L_f;
                    }
                    c = s[i++];
                }
                var d = c - '0';
                if (0 < d && d < 10) {
                    lo = unchecked((uint)d);
                    hi = 0;
                    while (s.Length > i) {
                        c = s[i++];
                        d = c - '0';
                        if (0 > d || d >= 10) {
                            goto L_f;
                        }
                        // TODO: ...
                        try {
                            lo = Numerics.DoubleArithmetic.MultiplyUnsigned(lo, hi, 10, 0, out hi);
                            lo = Numerics.DoubleArithmetic.AddUnsigned(lo, hi, unchecked((uint)d), 0, out hi);
                        } catch (ArithmeticException) {
                            // overflow
                            goto L_f;
                        }
                    }
                    if (s.Length == i) {
                        if (sign) {
                            // check overflow
                            // 0x80000000_00000000_00000000_00000000
                            if (Numerics.DoubleArithmetic.GreaterThan(lo, hi, (UInt64)0, (UInt64)1 << (64 - 1))) {
                                goto L_f;
                            }
                            lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                        } else {
                            // check overflow
                            // 0x7FFFFFFF_FFFFFFFF_FFFFFFFF_FFFFFFFF
                            if (Numerics.DoubleArithmetic.GreaterThan(lo, hi, ~(UInt64)0, (~(UInt64)0) ^ ((UInt64)1 << (64 - 1)))) {
                                goto L_f;
                            }
                        }
                        result = new XInt128(lo, unchecked((HInt64)hi));
                        return true;
                    }
                }
            }
        L_f:;
            Try_HandleOutParameterIfFalse(out result);
            return false;
        }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //[ThreadStaticAttribute()] // <- Important!
        //private static char[] buffer_ToString_40;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //internal static char[] buffer_ToStringCStyleI128 {

        //    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //    get {
        //        var buffer = buffer_ToString_40;
        //        if (null == buffer) {
        //            goto L_a;
        //        }
        //    L_d:;
        //        return buffer;
        //    L_a:;
        //        buffer = new char[40];
        //        buffer_ToString_40 = buffer;
        //        goto L_d;
        //    }
        //}

        public string ToStringCStyleI128() {
            return new string(ToStringCStyleI128(stackalloc char[40]));
        }

        public Span<char> ToStringCStyleI128(Span<char> buffer) {
            var lo = this.lo;
            var hi = unchecked((UInt64)this.hi);
            var isNegative = 0 > unchecked((Int64)hi);
            if (isNegative) {
                lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
            }
            var i = buffer.Length;
            {
                UInt64 r_lo;
                UInt64 r_hi;
                do {
                    lo = Numerics.DoubleArithmetic.DivRem(lo, hi, 10, 0, out r_lo, out r_hi, out hi);
                    buffer[--i] = unchecked((char)('0' + r_lo));
                } while (0 != lo || 0 != hi);
            }
            if (isNegative) {
                buffer[--i] = '-';
            }
            return buffer[i..];
        }

        public override string ToString() {
#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
            {
                return this.ToString(null, CultureInfo.CurrentCulture);
            }
#endif
            {
#pragma warning disable CS0162 // Unreachable code detected
                return this.ToStringCStyleI128();
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        public string ToString(IFormatProvider formatProvider) {
            return this.ToString(null, formatProvider);
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            // TODO: ...
            {
                return this.ToStringCStyleI128();
            }
        }
#endif
    }
}

namespace UltimateOrb {
    using Internal;
    using UltimateOrb.Runtime.CompilerServices;
    using static UltimateOrb.Utilities.ThrowHelper;
    using HInt64 = Int64;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;
    using OInt128 = UInt128;
    using XInt128 = Int128;

    public partial struct Int128 {

        public static partial class Math {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 BigMul(HInt64 first, HInt64 second) {
                var lo = Numerics.DoubleArithmetic.BigMul(first, second, out HInt64 hi);
                return new XInt128(lo, hi);
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 DivRem(XInt128 dividend, XInt128 divisor, out XInt128 remainder) {
                Unsafe.SkipInit(out remainder);
                var lo = Numerics.DoubleArithmetic.DivRem(dividend.lo, dividend.hi, divisor.lo, divisor.hi, out Unsafe.AsRef(in remainder.lo), out Unsafe.AsRef(in remainder.hi), out HInt64 hi);
                return new XInt128(lo, hi);
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Pow(XInt128 @base, int exponent) {
                var lo = @base.lo;
                var hi = @base.hi;
                if (exponent > 0) {
                    var result_lo = (UInt64)1;
                    var result_hi = (HInt64)0;
                    for (; ; ) {
                        if (0 != (1 & exponent)) {
                            result_lo = Numerics.DoubleArithmetic.MultiplySigned(result_lo, result_hi, lo, hi, out result_hi);
                        }
                        if (0 != (exponent >>= 1)) {
                            lo = Numerics.DoubleArithmetic.MultiplySigned(lo, hi, lo, hi, out hi);
                            continue;
                        }
                        break;
                    }
                    return new XInt128(result_lo, result_hi);
                }
                if (0 == exponent || (1 == lo && 0 == hi)) {
                    return XInt128.One;
                }
                if (UInt64.MaxValue == lo && -1 == hi) {
                    return (0 == (1 & exponent)) ? XInt128.MinusOne : XInt128.One;
                }
                if (0 != lo || 0 != hi) {
                    // return 0;
                    return default;
                }
                throw ThrowDivideByZeroException();
            }
        }

        public static partial class DoubleArithmetic {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining | System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static UInt128 BigMul(XInt128 first, XInt128 second, out XInt128 result_hi) {
                var result_lo_lo = Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out UInt64 result_lo_hi, out UInt64 result_hi_lo, out HInt64 result_hi_hi);
                result_hi = new XInt128(result_hi_lo, result_hi_hi);
                return new UInt128(result_lo_lo, result_lo_hi);
            }
        }

        public static partial class BinaryNumerals {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 NextPermutation(XInt128 value) {
                var lo = value.lo;
                var hi = unchecked((UInt64)value.hi);
                lo = Numerics.DoubleArithmetic.NextPermutation(lo, hi, out hi);
                return new XInt128(lo, unchecked((HInt64)hi));
            }
        }

        public static partial class Converter {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 ToInt128(double value) {
                const int BitSize = 64;
                const int ExponentBitSize = 11;
                const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
                const int SignBitSize = 1;
                const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

                // const Int64 SignMask = unchecked((Int64)checked((Int64)1u << (ExponentBitSize + FractionBitSize)));
                const Int64 ExponentMask = unchecked((Int64)checked((((Int64)1u << ExponentBitSize) - 1u) << FractionBitSize));
                const Int64 FractionMask = unchecked((Int64)checked(((Int64)1u << FractionBitSize) - 1u));

                var b = BitConverter.DoubleToInt64Bits(value);
                var e = checked((int)(ExponentMask >> FractionBitSize)) & unchecked((int)(b >> FractionBitSize));
                if (e >= checked(ExponentBias - 1)) {
                    if (e < checked(ExponentBias - 1 + 128)) {
                        e = unchecked(e - (FractionBitSize + ExponentBias));
                        var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                        var hi = (Int64)0;
                        if (e > 0) {
                            lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                        } else if (e < 0) {
                            var ol = (UInt64)0;
                            ol = Numerics.DoubleArithmetic.ShiftRight(ol, lo, unchecked(-e), out lo);
                            // IEEE Std 754-2008 roundTiesToEven
                            if (unchecked((UInt64)Int64.MinValue) < ol || (unchecked((UInt64)Int64.MinValue) == ol && 0 != (1 & lo))) {
                                lo = unchecked(1 + lo);
                            }
                        }
                        if (0 > b) {
                            lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                        }
                        return new XInt128(lo, hi);
                    }
                    // check overflow
                    checked(0 - unchecked((UInt64)((Int64)0xC7E0000000000000 - b))).Ignore();
                    return MinValue;
                }
                return Zero;
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 ToInt128(Single value) {
                return ToInt128(unchecked((double)value));
            }
        }
    }
}

namespace Internal.System {

    using static UltimateOrb.Utilities.ThrowHelper;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    public static partial class Converter {

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Half ToHalf(Int64 value) {
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            const int BitSizeTo = 16;
            const int ExponentBitSizeTo = 5;
            // const int ExponentBiasTo = unchecked(checked((1 << (ExponentBitSizeTo - 1))) - 1);
            const int SignBitSizeTo = 1;
            const int FractionBitSizeTo = checked(BitSizeTo - SignBitSizeTo - ExponentBitSizeTo);

            var value_ = value;
            if (0 != value_) {
                var s = (0 > value_ ? Int64.MinValue : 0);
                value_ = 0 > value_ ? unchecked(-value_) : value_;
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((Int64)(checked(64 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = unchecked((UInt64)value_) & unchecked(((UInt64)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((UInt64)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                return unchecked((Half)BitConverter.Int64BitsToDouble(s));
            }
            return default;
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Single ToSingle(Int64 value) {
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            const int BitSizeTo = 32;
            const int ExponentBitSizeTo = 8;
            // const int ExponentBiasTo = unchecked(checked((1 << (ExponentBitSizeTo - 1))) - 1);
            const int SignBitSizeTo = 1;
            const int FractionBitSizeTo = checked(BitSizeTo - SignBitSizeTo - ExponentBitSizeTo);

            var value_ = value;
            if (0 != value_) {
                var s = (0 > value_ ? Int64.MinValue : 0);
                value_ = 0 > value_ ? unchecked(-value_) : value_;
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((Int64)(checked(64 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = unchecked((UInt64)value_) & unchecked(((UInt64)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((UInt64)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                return unchecked((Single)BitConverter.Int64BitsToDouble(s));
            }
            return default;
        }
    }
}