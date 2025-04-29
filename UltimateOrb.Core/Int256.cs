

using Internal;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using UltimateOrb;

namespace UltimateOrb {
    using static global::Internal.System.IConvertibleModule;
    using static global::Internal.System.Converter;
    using static UltimateOrb.Utilities.ThrowHelper;
    using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
    using static UltimateOrb.XInt256Helpers;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using XInt256 = Int256;
    using OInt256 = UInt256;
    using HInt128 = System.Int128;
    using SUInt128 = System.UInt128;
    using SInt128 = System.Int128;

    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Interoperability", "CA1413:AvoidNonpublicFieldsInComVisibleValueTypes")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    [System.SerializableAttribute()]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 16)]
    public readonly partial struct Int256
        : IEquatable<XInt256>, IComparable<XInt256>, IComparable
#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        , IConvertible, IFormattable
#endif
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SUInt128 lo;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HInt128 hi;

        [System.Diagnostics.Contracts.PureAttribute()]
        public SInt128 LoInt128Bits {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return unchecked((SInt128)this.lo);
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public SInt128 HiInt128Bits {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return unchecked((SInt128)this.hi);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal Int256(SUInt128 lo, HInt128 hi) {
            this.lo = lo;
            this.hi = hi;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 FromBits(SInt128 lo, SInt128 hi) {
            return new XInt256(unchecked((SUInt128)lo), unchecked((HInt128)hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 FromBits(SUInt128 lo, HInt128 hi) {
            return new XInt256(unchecked((SUInt128)lo), unchecked((HInt128)hi));
        }

        #region Standard values
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Zero {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return default(XInt256);
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
                return 0 == (this.lo | unchecked((SUInt128)this.hi));
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 One {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(1, 0);
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
        public static XInt256 MinusOne {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(SUInt128.MaxValue, unchecked((SInt128)SUInt128.MaxValue));
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
                return SUInt128.MaxValue == this.lo && unchecked((SInt128)SUInt128.MaxValue) == this.hi;
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 MaxValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(SUInt128.MaxValue, HInt128.MaxValue);
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
                return SUInt128.MaxValue == this.lo && HInt128.MaxValue == this.hi;
            }
        }

        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 MinValue {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(SUInt128.MinValue, HInt128.MinValue);
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
                return SUInt128.MinValue == this.lo && HInt128.MinValue == this.hi;
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
        public bool CanConvertToUInt128 {

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
            if ((256 - 1) <= n) {
                return 0 > this.hi;
            }
            if (n <= 0) {
                return this.IsZero;
            }
            if (n <= 128) {
                if (128 == n) {
                    return this.CanConvertToUInt128;
                }
                return 0 == this.hi && 0 == this.lo >> (128 - n);
            } else {
                return 0 == this.hi >> (128 - n);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int TestBit(XInt256 value, int index) {
            if (0 <= index && 256 > index) {
                if (index < 128) {
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
        public static XInt256 OnesComplement(XInt256 value) {
            return new XInt256(~value.lo, ~value.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator ~(XInt256 value) {
            return new XInt256(~value.lo, ~value.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 BitwiseOr(XInt256 first, XInt256 second) {
            return new XInt256(first.lo | second.lo, first.hi | second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator |(XInt256 first, XInt256 second) {
            return new XInt256(first.lo | second.lo, first.hi | second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Xor(XInt256 first, XInt256 second) {
            return new XInt256(first.lo ^ second.lo, first.hi ^ second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator ^(XInt256 first, XInt256 second) {
            return new XInt256(first.lo ^ second.lo, first.hi ^ second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 BitwiseAnd(XInt256 first, XInt256 second) {
            return new XInt256(first.lo & second.lo, first.hi & second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator &(XInt256 first, XInt256 second) {
            return new XInt256(first.lo & second.lo, first.hi & second.hi);
        }
        #endregion

        #region Shift operations        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator <<(XInt256 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftLeft(value.lo, value.hi, count, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_LeftShift(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.ShiftLeft(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator >>(XInt256 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRight(value.lo, value.hi, count, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_RightShift(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.ShiftRight(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }
        #endregion

        #region Comparisons
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool Equals(XInt256 other) {
            return this.lo == other.lo && this.hi == other.hi;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public override bool Equals(object? obj) {
            if (obj is XInt256 value) {
                return this.Equals(value);
            }
            return false;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        bool IEquatable<XInt256>.Equals(XInt256 other) {
            return this.lo == other.lo && this.hi == other.hi;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public override int GetHashCode() {
            return HashCodeHelper.GetHashCode(this.lo ^ unchecked((SUInt128)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public int CompareTo(XInt256 other) {
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
            if (obj is XInt256 value) {
                return CompareTo(value);
            }

            static string? getMessageForCompareToArgumentException() {
                try {
                    ((SInt128)(-1)).CompareTo(Type.EmptyTypes).Ignore(); // Generate an InvalidCastException.
                } catch (InvalidCastException ex) {
                    // Catch the InvalidCastException and extract the value of Message property.
                    try {
                        // So everyone can use their language.
                        return ex.Message.Replace(nameof(SInt128), nameof(Int256));
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
        int IComparable<XInt256>.CompareTo(XInt256 other) {
            return Numerics.DoubleArithmetic.Compare(this.lo, this.hi, other.lo, other.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator ==(XInt256 first, XInt256 second) {
            return (first.lo == second.lo) && (first.hi == second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator !=(XInt256 first, XInt256 second) {
            return !(first == second);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int Compare(XInt256 first, XInt256 second) {
            return Numerics.DoubleArithmetic.Compare(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator >(XInt256 first, XInt256 second) {
            return Numerics.DoubleArithmetic.GreaterThan(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator >=(XInt256 first, XInt256 second) {
            return Numerics.DoubleArithmetic.GreaterThanOrEqual(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator <(XInt256 first, XInt256 second) {
            return Numerics.DoubleArithmetic.LessThan(first.lo, first.hi, second.lo, second.hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool operator <=(XInt256 first, XInt256 second) {
            return Numerics.DoubleArithmetic.LessThanOrEqual(first.lo, first.hi, second.lo, second.hi);
        }
        #endregion

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 AsUnsigned() {
            return new OInt256(unchecked((SUInt128)this.lo), unchecked((SUInt128)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToUnsignedUnchecked() {
            return new OInt256(unchecked((SUInt128)this.lo), unchecked((SUInt128)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToUnsignedChecked() {
            return new OInt256(unchecked((SUInt128)this.lo), checked((SUInt128)this.hi));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToUnsigned() {
            return new OInt256(unchecked((SUInt128)this.lo), checked((SUInt128)this.hi));
        }

        #region Numeric Conversions
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(OInt256 value) {
            return value.ToSignedChecked();
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_ExplicitUnchecked(OInt256 value) {
            return value.ToSignedUnchecked();
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(sbyte value) {
            return new XInt256(unchecked((SUInt128)(SInt128)value), value >> (8 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(Int16 value) {
            return new XInt256(unchecked((SUInt128)(SInt128)value), value >> (16 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(Int32 value) {
            return new XInt256(unchecked((SUInt128)(SInt128)value), value >> (32 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(SInt128 value) {
            return new XInt256(unchecked((SUInt128)(SInt128)value), value >> (128 - 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(char value) {
            return new XInt256(value, 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(byte value) {
            return new XInt256(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(UInt16 value) {
            return new XInt256(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(UInt32 value) {
            return new XInt256(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(SUInt128 value) {
            return new XInt256(value, 0);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator sbyte(XInt256 value) {
            (checked(0 - unchecked(value.hi - (unchecked((SInt128)value.lo) >> (128 - 1))))).Ignore(); // check overflow
            return checked((sbyte)unchecked((SInt128)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int16(XInt256 value) {
            (checked(0 - unchecked(value.hi - (unchecked((SInt128)value.lo) >> (128 - 1))))).Ignore(); // check overflow
            return checked((Int16)unchecked((SInt128)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int32(XInt256 value) {
            (checked(0 - unchecked(value.hi - (unchecked((SInt128)value.lo) >> (128 - 1))))).Ignore(); // check overflow
            return checked((Int32)unchecked((SInt128)value.lo));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator SInt128(XInt256 value) {
            (checked(0 - unchecked(value.hi - (unchecked((SInt128)value.lo) >> (128 - 1))))).Ignore(); // check overflow
            return unchecked((SInt128)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator char(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((char)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator byte(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((byte)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt16(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((UInt16)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt32(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((UInt32)value.lo);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator SUInt128(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return value.lo;
        }

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(Half value) {
            return (XInt256)unchecked((double)value);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(Single value) {
            return (XInt256)unchecked((double)value);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(double value) {
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
                if (e < checked(ExponentBias - 1 + 256)) {
                    e = unchecked(e - (FractionBitSize + ExponentBias));
                    var lo = unchecked((SUInt128)(((SInt128)1 << FractionBitSize) | (FractionMask & b)));
                    var hi = (SInt128)0;
                    if (e > 0) {
                        lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                    } else if (e < 0) {
                        // var ol = (SUInt128)0;
                        // ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                        // Do NOT uncomment. (int)3.5 == 3
                        /*
                        // IEEE Std 754-2008 roundTiesToEven
                        if (unchecked((SUInt128)SInt128.MinValue) < ol || (unchecked((SUInt128)SInt128.MinValue) == ol && 0 != (1 & lo))) {
                            lo = unchecked(1 + lo);
                        }
                        */
                    }
                    if (0 > b) {
                        lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                    }
                    return new XInt256(lo, hi);
                }
                // check overflow
                checked(0 - unchecked((SUInt128)((SInt128)0xC7E0000000000000 - b))).Ignore();
                return MinValue;
            }
            return Zero;
        }

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Half(XInt256 value) {
            throw new NotImplementedException();
            const int BitSize = 128;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            const int BitSizeTo = 16;
            const int ExponentBitSizeTo = 5;
            // const int ExponentBiasTo = unchecked(checked((1 << (ExponentBitSizeTo - 1))) - 1);
            const int SignBitSizeTo = 1;
            const int FractionBitSizeTo = checked(BitSizeTo - SignBitSizeTo - ExponentBitSizeTo);

            var lo = value.lo;
            var hi = value.hi;
            if (0 != hi) {
                var s = (0 > hi ? SInt128.MinValue : 0);
                if (0 > hi) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                var c = 0;
                for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((SInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
                lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                var lo0 = lo & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSizeTo), out hi);
                // IEEE Std 754-2008 roundTiesToEven
                if (unchecked((SUInt128)SInt128.MinValue) < lo || (unchecked((SUInt128)SInt128.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                    unchecked {
                        ++hi;
                    }
                }
                s = unchecked(s + (hi << checked(FractionBitSize - FractionBitSizeTo)));
                // TODO:
                // return unchecked((Half)BitConverter.Int128BitsToQuadruple(unchecked((SInt128)s)));
            }
            // We cannot write "return (Half)lo;" because of the double rounding issue.
            // See https://www.exploringbinary.com/double-rounding-errors-in-floating-point-conversions/ .
            return ToHalf(lo);
        }
#endif

        // Make the conversion explicit due to precision loss may be significant.
        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Single(XInt256 value) {
            throw new NotImplementedException();
            const int BitSize = 128;
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
                var s = (0 > hi ? SInt128.MinValue : 0);
                if (0 > hi) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                var c = 0;
                for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((SInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
                lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                var lo0 = lo & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSizeTo), out hi);
                // IEEE Std 754-2008 roundTiesToEven
                if (unchecked((SUInt128)SInt128.MinValue) < lo || (unchecked((SUInt128)SInt128.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                    unchecked {
                        ++hi;
                    }
                }
                s = unchecked(s + (hi << checked(FractionBitSize - FractionBitSizeTo)));
                // TODO:
                // return unchecked((Single)BitConverter.Int128BitsToQuadruple(unchecked((SInt128)s)));
            }
            return ToSingle(lo);
        }

        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator double(XInt256 value) {
            throw new NotImplementedException();
            const int BitSize = 128;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            var lo = value.lo;
            var hi = value.hi;
            if (0 != hi) {
                var s = (0 > hi ? SInt128.MinValue : 0);
                if (0 > hi) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                var c = 0;
                if (0 != hi) {
                    for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                        unchecked {
                            ++c;
                        }
                    }
                    s |= unchecked((SInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
                    lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                    var lo0 = lo & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSize)) - 1);
                    lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSize), out hi);
                    // IEEE Std 754-2008 roundTiesToEven
                    if (unchecked((SUInt128)SInt128.MinValue) < lo || (unchecked((SUInt128)SInt128.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                        unchecked {
                            ++hi;
                        }
                    }
                    s = unchecked(s + hi);
                    // TODO:
                    // return BitConverter.Int128BitsToQuadruple(s);
                }
                return -unchecked((double)lo);
            }
            return unchecked((double)lo);
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator global::System.Decimal(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.AbsSignedAsUnsigned(value.lo, unchecked((SUInt128)value.hi), out SUInt128 hi);
            checked((UInt32)hi).Ignore(); // check overflow
            return new decimal(unchecked((Int32)(lo >> (32 * 0))), unchecked((Int32)(lo >> (32 * 1))), unchecked((Int32)(hi >> (32 * 0))), 0 > value.hi, 0);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        // TODO
        private static readonly XInt256[] PowersOf10 = GetPowersOf10();

        private static XInt256[] GetPowersOf10() {
            var lo = (SUInt128)1;
            var hi = (HInt128)0;
            var r = new XInt256[39];
            for (var i = 1; r.Length > i; ++i) {
                lo = Numerics.DoubleArithmetic.MultiplyUnchecked(10, 0, lo, hi, out hi);
                r[i] = new XInt256(lo, hi);
            }
            return r;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(global::System.Decimal value) {
            // TODO: Struct layout of global::System.Decimal may vary...
#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
            var a = (stackalloc UInt32[4]);
            global::System.Decimal.GetBits(value, MemoryMarshal.Cast<UInt32, Int32>(a));
#else
            var a = (((object)global::System.Decimal.GetBits(value)) as UInt32[])!; // CLI actually allows such sign casts.
#endif
            var lo = a[0] | ((SUInt128)a[1] << 32);
            var hi = (HInt128)a[2];
            var f = a[3];
            var scale = (f >> 16) & 0x1F;
            var d = PowersOf10[scale];
            lo = Numerics.DoubleArithmetic.DivRemUnchecked(lo, hi, d.lo, d.hi, out SUInt128 r_lo, out HInt128 r_hi, out hi);
            // Note: r <= (a[0] | ((SUInt128)a[1] << 32), (SInt128)a[2]) .
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
            return new XInt256(lo, hi);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public byte[] ToBigIntegerByteArray() {
            var a = new byte[256 / 8];
            {
                var t = this.lo;
                var j = 0;
                for (var i = 0; (128 / 8) > i; ++i) {
                    a[(128 / 8) * j + i] = unchecked((byte)(t >> (8 * i)));
                }
            }
            {
                var t = this.hi;
                var j = 1;
                for (var i = 0; (128 / 8) > i; ++i) {
                    a[(128 / 8) * j + i] = unchecked((byte)(t >> (8 * i)));
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
        public static XInt256 Plus(XInt256 value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator +(XInt256 value) {
            return value;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Negate(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.NegateSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator -(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.NegateSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_UnaryNegationUnchecked(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.NegateUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Increase(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 IncreaseUnsigned(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator ++(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_IncrementUnchecked(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static XInt256 Decrease(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 DecreaseUnsigned(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator --(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_DecrementUnchecked(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Add(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.AddSigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator +(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.AddSigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_AdditionUnchecked(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.AddUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Subtract(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.SubtractSigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator -(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.SubtractSigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_SubtractionUnchecked(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.SubtractUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Multiply(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.MultiplySigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator *(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.MultiplySigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_MultiplyUnchecked(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.MultiplyUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 DivRem(XInt256 first, XInt256 second, out XInt256 remainder) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out SUInt128 remainder_lo, out HInt128 remainder_hi, out HInt128 hi);
            remainder = new XInt256(remainder_lo, remainder_hi);
            return new XInt256(lo, hi);
        }

        /*
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static (XInt256 Quotient, XInt256 Remainder) DivRem(XInt256 first, XInt256 second, out XInt256 remainder) {
            var lo = MathEx.DivRem(first.lo, first.hi, second.lo, second.hi, out SUInt128 remainder_lo, out HInt128 remainder_hi, out HInt128 hi);
            return (new XInt256(lo, hi), new XInt256(remainder_lo, remainder_hi));
        }
        */

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Division(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_IntegerDivision(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_IntegerDivisionUnchecked(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.DivideUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Remainder(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Remainder(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 operator %(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Remainder(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        /*
        public static double op_FloatingPointDivisionDouble(XInt256 first, XInt256 second) {            
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
            if (SInt128.MinValue <= this && this <= SInt128.MaxValue) {
                return ((Int128)unchecked((SInt128)this.lo)).ToInt64(provider);
            }
            return ((long)Int32.MinValue - 1).ToInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            if (SUInt128.MinValue <= this && this <= SUInt128.MaxValue) {
                return ((UInt128)unchecked((SUInt128)this.lo)).ToUInt64(provider);
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
                    return ex.Message.Replace(nameof(Int64), nameof(Int256));
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

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) {
            if (null == conversionType) {
                throw new ArgumentNullException(nameof(conversionType));
            }
            // return Convert.DefaultToType((IConvertible)this, type, provider);
            throw new NotImplementedException();
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
        public static bool TryParseCStyleNormalizedI256(string s, out XInt256 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                var sign = ('-' == c);
                SUInt128 lo;
                SUInt128 hi;
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
                            if (Numerics.DoubleArithmetic.GreaterThan(lo, hi, (SUInt128)0, (SUInt128)1 << (128 - 1))) {
                                goto L_f;
                            }
                            lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                        } else {
                            // check overflow
                            // 0x7FFFFFFF_FFFFFFFF_FFFFFFFF_FFFFFFFF
                            if (Numerics.DoubleArithmetic.GreaterThan(lo, hi, ~(SUInt128)0, (~(SUInt128)0) ^ ((SUInt128)1 << (128 - 1)))) {
                                goto L_f;
                            }
                        }
                        result = new XInt256(lo, unchecked((HInt128)hi));
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
        //internal static char[] buffer_ToStringCStyleI256 {

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

        public string ToStringCStyleI256() {
            return new string(ToStringCStyleI256(stackalloc char[40]));
        }

        public Span<char> ToStringCStyleI256(Span<char> buffer) {
            var lo = this.lo;
            var hi = unchecked((SUInt128)this.hi);
            var isNegative = 0 > unchecked((SInt128)hi);
            if (isNegative) {
                lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
            }
            var i = buffer.Length;
            {
                SUInt128 r_lo;
                SUInt128 r_hi;
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
                return this.ToStringCStyleI256();
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
                return this.ToStringCStyleI256();
            }
        }
#endif
    }
}

namespace UltimateOrb {
    using Internal;
    using UltimateOrb.Runtime.CompilerServices;
    using static UltimateOrb.Utilities.ThrowHelper;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using XInt256 = Int256;
    using OInt256 = UInt256;
    using HInt128 = System.Int128;
    using SUInt128 = System.UInt128;
    using SInt128 = System.Int128;

    public partial struct Int256 {

        public static partial class Math {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 BigMul(HInt128 first, HInt128 second) {
                var lo = Numerics.DoubleArithmetic.BigMul(first, second, out HInt128 hi);
                return new XInt256(lo, hi);
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 DivRem(XInt256 dividend, XInt256 divisor, out XInt256 remainder) {
                Unsafe.SkipInit(out remainder);
                var lo = Numerics.DoubleArithmetic.DivRem(dividend.lo, dividend.hi, divisor.lo, divisor.hi, out Unsafe.AsRef(in remainder.lo), out Unsafe.AsRef(in remainder.hi), out HInt128 hi);
                return new XInt256(lo, hi);
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Pow(XInt256 @base, int exponent) {
                var lo = @base.lo;
                var hi = @base.hi;
                if (exponent > 0) {
                    var result_lo = (SUInt128)1;
                    var result_hi = (HInt128)0;
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
                    return new XInt256(result_lo, result_hi);
                }
                if (0 == exponent || (1 == lo && 0 == hi)) {
                    return XInt256.One;
                }
                if (SUInt128.MaxValue == lo && -1 == hi) {
                    return (0 == (1 & exponent)) ? XInt256.MinusOne : XInt256.One;
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
            public static UInt256 BigMul(XInt256 first, XInt256 second, out XInt256 result_hi) {
                var result_lo_lo = Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out SUInt128 result_lo_hi, out SUInt128 result_hi_lo, out HInt128 result_hi_hi);
                result_hi = new XInt256(result_hi_lo, result_hi_hi);
                return new UInt256(result_lo_lo, result_lo_hi);
            }
        }

        public static partial class BinaryNumerals {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 NextPermutation(XInt256 value) {
                var lo = value.lo;
                var hi = unchecked((SUInt128)value.hi);
                lo = Numerics.DoubleArithmetic.NextPermutation(lo, hi, out hi);
                return new XInt256(lo, unchecked((HInt128)hi));
            }
        }

        public static partial class Converter {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 ToInt256(double value) {
                const int BitSize = 128;
                const int ExponentBitSize = 11;
                const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
                const int SignBitSize = 1;
                const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

                // const SInt128 SignMask = unchecked((SInt128)checked((SInt128)1u << (ExponentBitSize + FractionBitSize)));
                var ExponentMask = unchecked((SInt128)checked((((SInt128)1u << ExponentBitSize) - 1u) << FractionBitSize));
                var FractionMask = unchecked((SInt128)checked(((SInt128)1u << FractionBitSize) - 1u));

                var b = BitConverter.DoubleToInt64Bits(value);
                var e = checked((int)(ExponentMask >> FractionBitSize)) & unchecked((int)(b >> FractionBitSize));
                if (e >= checked(ExponentBias - 1)) {
                    if (e < checked(ExponentBias - 1 + 256)) {
                        e = unchecked(e - (FractionBitSize + ExponentBias));
                        var lo = unchecked((SUInt128)(((SInt128)1 << FractionBitSize) | (FractionMask & b)));
                        var hi = (SInt128)0;
                        if (e > 0) {
                            lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                        } else if (e < 0) {
                            var ol = (SUInt128)0;
                            ol = Numerics.DoubleArithmetic.ShiftRight(ol, lo, unchecked(-e), out lo);
                            // IEEE Std 754-2008 roundTiesToEven
                            if (unchecked((SUInt128)SInt128.MinValue) < ol || (unchecked((SUInt128)SInt128.MinValue) == ol && 0 != (1 & lo))) {
                                lo = unchecked(1 + lo);
                            }
                        }
                        if (0 > b) {
                            lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                        }
                        return new XInt256(lo, hi);
                    }
                    // check overflow
                    checked(0 - unchecked((SUInt128)((SInt128)0xC7E0000000000000 - b))).Ignore();
                    return MinValue;
                }
                return Zero;
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 ToInt256(Single value) {
                return ToInt256(unchecked((double)value));
            }
        }
    }
}

namespace Internal.System {

    using static UltimateOrb.Utilities.ThrowHelper;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using XInt256 = Int256;
    using OInt256 = UInt256;
    using HInt128 = global::System.Int128;
    using SUInt128 = global::System.UInt128;
    using SInt128 = global::System.Int128;

    public static partial class Converter {

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Half ToHalf(Int256 value) {
            throw new NotImplementedException();
            const int BitSize = 256;
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
                var s = 0 > value_ ? Int256.MinValue : 0;
                value_ = 0 > value_ ? unchecked(-value_) : value_;
                var c = UltimateOrb.Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt256)value_));
                /*
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((Int256)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                */
                s |= unchecked((Int256)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = unchecked((UInt256)value_) & unchecked(((UInt256)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((UInt256)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((UInt256)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((UInt256)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                // TODO:
                // return unchecked((Half)BitConverter.Int256BitsToDouble(s));
            }
            return default;
        }
#endif

        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Single ToSingle(Int256 value) {
            throw new NotImplementedException();
            const int BitSize = 256;
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
                var s = (0 > value_ ? SInt128.MinValue : 0);
                value_ = 0 > value_ ? unchecked(-value_) : value_;
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                s |= unchecked((SInt128)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = unchecked((SUInt128)value_) & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((SUInt128)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                // TODO:
                // s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                // return unchecked((Single)BitConverter.Int128BitsToQuadruple(s));
            }
            return default;
        }
    }
}