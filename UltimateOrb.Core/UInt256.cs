// WIP: Checked operators
// #define LEGACY_OPERATOR_CHECKNESS
using Internal;
using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using static UltimateOrb.Utilities.SignConverter;

namespace UltimateOrb {
    using static global::Internal.System.IConvertibleModule;
    using static global::Internal.System.Converter;
    using static UltimateOrb.Utilities.ThrowHelper;
    using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
    using static UltimateOrb.XInt256Helpers;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using XInt256 = UInt256;
    using OInt256 = Int256;
    using HInt128 = System.UInt128;
    using SUInt128 = System.UInt128;
    using SInt128 = System.Int128;

    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Interoperability", "CA1413:AvoidNonpublicFieldsInComVisibleValueTypes")]
    [System.CLSCompliantAttribute(false)]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    [System.SerializableAttribute()]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 16)]
    public readonly partial struct UInt256
        : IEquatable<XInt256>, IComparable<XInt256>, IComparable
#if NET7_0_OR_GREATER
#if !LEGACY_OPERATOR_CHECKNESS
        , INumberBase<XInt256>, IBinaryInteger<XInt256>, IUnsignedNumber<XInt256>
#endif
        , IMinMaxValue<XInt256>
#endif
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
        internal UInt256(SUInt128 lo, HInt128 hi) {
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IAdditiveIdentity<XInt256, XInt256>.AdditiveIdentity {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return default(XInt256);
            }
        }
#endif

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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IMultiplicativeIdentity<XInt256, XInt256>.MultiplicativeIdentity {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(1, 0);
            }
        }
#endif

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
        public static XInt256 Two {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt256(2, 0);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool IsTwo {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 2 == this.lo && 0 == this.hi;
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
                return false;
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
                return (this.lo == 0 && this.hi == 0) ? 0 : 1;
            }
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static int INumber<XInt256>.Sign(XInt256 value) {
            return value.Sign;
        }
#endif

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
        public bool CanConvertToUInt64 {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return 0 == this.hi && 0 == unchecked((UInt64)(this.lo >>> 64) >> 32);
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

        static int INumberBase<XInt256>.Radix => throw new NotImplementedException();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "n")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public bool CanConvertToUIntN(int n) {
            if (n < 256) {
                if (n > 0) {
                    if (n <= 128) {
                        if (128 < n) {
                            return 0 == this.hi && 0 == this.lo >> (128 - n);
                        }
                        return this.CanConvertToUInt128;
                    }
                    return 0 == this.hi >> (256 - n);
                }
                return this.IsZero;
            }
            return true;
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IShiftOperators<XInt256, int, XInt256>.operator >>(XInt256 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRightSigned(value.lo, value.hi, count, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IShiftOperators<XInt256, int, XInt256>.operator >>>(XInt256 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(value.lo, value.hi, count, out HInt128 hi);
            return new XInt256(lo, hi);
        }
#endif

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
            return this.lo.GetHashCode() ^ this.hi.GetHashCode();
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
                        return ex.Message.Replace(nameof(SInt128), nameof(UInt256));
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 AsSigned() {
            return new OInt256(unchecked((SUInt128)this.lo), unchecked((SInt128)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToSignedUnchecked() {
            return new OInt256(unchecked((SUInt128)this.lo), unchecked((SInt128)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToSignedChecked() {
            return new OInt256(unchecked((SUInt128)this.lo), checked((SInt128)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt256 ToSigned() {
            return new OInt256(unchecked((SUInt128)this.lo), checked((SInt128)this.hi));
        }

        #region Numeric Conversions
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(OInt256 value) {
            return value.ToUnsignedChecked();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 op_ExplicitUnchecked(OInt256 value) {
            return value.ToUnsignedUnchecked();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(byte value) {
            return new XInt256(unchecked((SUInt128)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(UInt16 value) {
            return new XInt256(unchecked((SUInt128)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(UInt32 value) {
            return new XInt256(unchecked((SUInt128)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(SUInt128 value) {
            return new XInt256(unchecked((SUInt128)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt256(char value) {
            return new XInt256(unchecked((SUInt128)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(sbyte value) {
            return new XInt256(unchecked((SUInt128)checked((byte)value)), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(Int16 value) {
            return new XInt256(unchecked((SUInt128)checked((UInt16)value)), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(Int32 value) {
            return new XInt256(unchecked((SUInt128)checked((UInt32)value)), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt256(SInt128 value) {
            return new XInt256(unchecked((SUInt128)checked((SUInt128)value)), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator byte(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((byte)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt16(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((UInt16)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator UInt32(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return checked((UInt32)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator SUInt128(XInt256 value) {
            (checked(0 - unchecked((SUInt128)value.hi))).Ignore(); // check overflow
            return value.lo;
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
        public static explicit operator sbyte(XInt256 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((sbyte)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int16(XInt256 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((Int16)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Int32(XInt256 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((Int32)value.lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator SInt128(XInt256 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((SInt128)value.lo);
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
            const int BitSize = 128;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            // var SInt128 SignMask = unchecked((SInt128)checked((SInt128)1u << (ExponentBitSize + FractionBitSize)));
            var ExponentMask = unchecked((SInt128)checked((((SInt128)1u << ExponentBitSize) - 1u) << FractionBitSize));
            var FractionMask = unchecked((SInt128)checked(((SInt128)1u << FractionBitSize) - 1u));

            var b = BitConverter.DoubleToInt64Bits(value);
            var e = checked((int)(ExponentMask >> FractionBitSize)) & unchecked((int)(b >> FractionBitSize));
            if (e >= checked(ExponentBias - 1)) {
                b.ThrowOnNegative();
                checked(checked(ExponentBias - 1 + 256) - e).Ignore();
                {
                    e = unchecked(e - (FractionBitSize + ExponentBias));
                    var lo = unchecked((SUInt128)(((SInt128)1 << FractionBitSize) | (FractionMask & b)));
                    var hi = (SUInt128)0;
                    if (e > 0) {
                        lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                    } // else if (e < 0) {
                      // var ol = (SUInt128)0;
                      // ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                      // // Do NOT uncomment. (int)3.5 == 3
                    /*
                    // IEEE Std 754-2008 roundTiesToEven
                    if (unchecked((SUInt128)SInt128.MinValue) < ol || (unchecked((SUInt128)SInt128.MinValue) == ol && 0 != (1 & lo))) {
                        lo = unchecked(1 + lo);
                    }
                    */
                    // }
                    if (0 > b) {
                        lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                    }
                    return new XInt256(lo, hi);
                }
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
                if (hi < (((SUInt128)1 << checked(1 + 1 + FractionBitSizeTo)) - 1) << checked(BitSize - (1 + 1 + FractionBitSizeTo))) {
                    // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                    var c = 0;
                    for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                        unchecked {
                            ++c;
                        }
                    }
                    var s = unchecked((SUInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
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
                return Half.PositiveInfinity;
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
            throw new System.NotImplementedException();
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
                if (hi < (((SUInt128)1 << checked(1 + 1 + FractionBitSizeTo)) - 1) << checked(BitSize - (1 + 1 + FractionBitSizeTo))) {
                    // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                    var c = 0;
                    for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                        unchecked {
                            ++c;
                        }
                    }
                    var s = unchecked((SUInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
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
                return Single.PositiveInfinity;
            }
            // We cannot write "return (Single)lo;" because of the double rounding issue.
            // See https://www.exploringbinary.com/double-rounding-errors-in-floating-point-conversions/ .
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
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)hi));
                var c = 0;
                for (var tmp = hi; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                var s = unchecked((SUInt128)(checked(256 - 1 + ExponentBias) - c)) << FractionBitSize;
                lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                var lo0 = lo & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSize)) - 1);
                lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSize), out hi);
                s |= hi;
                // IEEE Std 754-2008 roundTiesToEven
                if (unchecked((SUInt128)SInt128.MinValue) < lo || (unchecked((SUInt128)SInt128.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                    s = unchecked(1 + s);
                }
                // TODO:
                // return BitConverter.Int128BitsToQuadruple(unchecked((SInt128)s));
            }
            return unchecked((double)lo);
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator global::System.Decimal(XInt256 value) {
            checked((UInt32)value.hi).Ignore(); // check overflow
            return new decimal(unchecked((Int32)(value.lo >> (32 * 0))), unchecked((Int32)(value.lo >> (32 * 1))), unchecked((Int32)(value.hi >> (32 * 0))), false, 0);
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
            lo = Numerics.DoubleArithmetic.DivRemUnchecked(lo, hi, d.lo, d.hi, out HInt128 r_lo, out HInt128 r_hi, out hi);
            // Note: r <= (a[0] | ((SUInt128)a[1] << 32), (SInt128)a[2]) .
            //   ==> r: Left shift (as a multiplication) will not lead to an arithmetic overflow.
            r_lo = Numerics.DoubleArithmetic.ShiftLeft(r_lo, r_hi, out r_hi);
            var c = Numerics.DoubleArithmetic.Compare(d.lo, d.hi, r_lo, r_hi);
            // 'Banker's rounding' (same as IEEE Std 754-2008 roundTiesToEven)
            if (c > 0 || (0 == c && (0 != (1 & lo)))) {
                lo = Numerics.DoubleArithmetic.IncreaseUnsigned(lo, hi, out hi);
            }
            if (0 > f) {
                lo = Numerics.DoubleArithmetic.NegateUnsigned(lo, hi, out hi);
            }
            return new XInt256(lo, hi);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public byte[] ToBigIntegerByteArray() {
            var a = new byte[256 / 8 + 1];
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
            var lo = Numerics.DoubleArithmetic.NegateUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator -(XInt256 value) {
#else
        public static XInt256 operator checked -(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.NegateUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_UnaryNegationUnchecked(XInt256 value) {
#else
        public static XInt256 operator -(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.NegateUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Increase(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 IncreaseSigned(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator ++(XInt256 value) {
#else
        public static XInt256 operator checked ++(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_IncrementUnchecked(XInt256 value) {
#else
        public static XInt256 operator ++(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.IncreaseUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static XInt256 Decrease(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 DecreaseSigned(XInt256 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator --(XInt256 value) {
#else
        public static XInt256 operator checked --(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_DecrementUnchecked(XInt256 value) {
#else
        public static XInt256 operator --(XInt256 value) {
#endif
            var lo = Numerics.DoubleArithmetic.DecreaseUnchecked(value.lo, value.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Add(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.AddUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }


        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator +(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator checked +(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.AddUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_AdditionUnchecked(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator +(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.AddUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Subtract(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.SubtractUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator -(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator checked -(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.SubtractUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_SubtractionUnchecked(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator -(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.SubtractUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 Multiply(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.MultiplyUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt256 operator *(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator checked *(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.MultiplyUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt256 op_MultiplyUnchecked(XInt256 first, XInt256 second) {
#else
        public static XInt256 operator *(XInt256 first, XInt256 second) {
#endif
            var lo = Numerics.DoubleArithmetic.MultiplyUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt256 DivRem(XInt256 first, XInt256 second, out XInt256 remainder) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out HInt128 remainder_lo, out HInt128 remainder_hi, out HInt128 hi);
            remainder = new XInt256(remainder_lo, remainder_hi);
            return new XInt256(lo, hi);
        }
        
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static (XInt256 Quotient, XInt256 Remainder) DivRem(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out SUInt128 remainder_lo, out HInt128 remainder_hi, out HInt128 hi);
            return (new XInt256(lo, hi), new XInt256(remainder_lo, remainder_hi));
        }

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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IDivisionOperators<XInt256, XInt256, XInt256>.operator checked /(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt256 IDivisionOperators<XInt256, XInt256, XInt256>.operator /(XInt256 first, XInt256 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt128 hi);
            return new XInt256(lo, hi);
        }
#endif
        #endregion

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        #region IBinaryNumber
        int IBinaryInteger<XInt256>.GetByteCount() {
            return 8;
        }

        static XInt256 IBinaryInteger<XInt256>.LeadingZeroCount(XInt256 value) {
            return unchecked((XInt256)Numerics.DoubleArithmetic.CountLeadingZeros(value.lo, value.hi));
        }

        static XInt256 IBinaryInteger<XInt256>.PopCount(XInt256 value) {
            return unchecked((XInt256)(Mathematics.BinaryNumerals.PopulationCount(value.lo) + Mathematics.BinaryNumerals.PopulationCount(value.hi)));
        }

        static XInt256 IBinaryInteger<XInt256>.RotateLeft(XInt256 value, int rotateAmount) {
            return new XInt256(Numerics.DoubleArithmetic.RotateLeft(value.lo, value.hi, rotateAmount, out var hi), hi);
        }

        static XInt256 IBinaryInteger<XInt256>.RotateRight(XInt256 value, int rotateAmount) {
            return new XInt256(Numerics.DoubleArithmetic.RotateRight(value.lo, value.hi, rotateAmount, out var hi), hi);
        }

        static XInt256 IBinaryInteger<XInt256>.TrailingZeroCount(XInt256 value) {
            return unchecked((XInt256)Numerics.DoubleArithmetic.CountTrailingZeros(value.lo, value.hi));
        }

        bool IBinaryInteger<XInt256>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (16 <= destination.Length) {
                BinaryPrimitives.TryWriteUInt128LittleEndian(destination, lo);
                BinaryPrimitives.TryWriteUInt128LittleEndian(destination[8..], hi);
                bytesWritten = 16;
                return true;
            }
            bytesWritten = default;
            return false;
        }

        bool IBinaryInteger<XInt256>.TryWriteBigEndian(System.Span<byte> destination, out int bytesWritten) {
            if (16 <= destination.Length) {
                BinaryPrimitives.TryWriteUInt128BigEndian(destination, hi);
                BinaryPrimitives.TryWriteUInt128BigEndian(destination[8..], lo);
                bytesWritten = 16;
                return true;
            }
            bytesWritten = default;
            return false;
        }

        static bool IBinaryInteger<XInt256>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out XInt256 value) {
            if (16 <= source.Length) {
                var v = new BigInteger(source, isUnsigned, false);
                if (XInt256.MinValue <= v && v <= XInt256.MaxValue) {
                    value = unchecked((XInt256)v);
                    return true;
                }
                
            }
            value = default;
            return false;
        }

        static bool IBinaryInteger<XInt256>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out XInt256 value) {
            if (16 <= source.Length) {
                var v = new BigInteger(source, isUnsigned, false);
                value = unchecked((XInt256)v);
                return true;
            }
            value = default;
            return false;
        }

        static bool IBinaryNumber<XInt256>.IsPow2(XInt256 value) {
            return Numerics.DoubleArithmetic.IsPowerOfTwo(value.lo, value.hi);
        }

        static XInt256 IBinaryNumber<XInt256>.Log2(XInt256 value) {
            return unchecked((XInt256)(Numerics.DoubleArithmetic.Log2Floor(value.lo, value.hi)));
        }

        static XInt256 INumber<XInt256>.Clamp(XInt256 value, XInt256 min, XInt256 max) {
            if (!(min <= max)) {
                // TODO:
                throw new ArgumentException();
            }
            if (value > max) {
                return max;
            } else if (min > value) {
                return min;
            }
            return value;
        }

        static XInt256 INumber<XInt256>.CopySign(XInt256 value, XInt256 sign) {
            return value;
        }

       public static XInt256 Max(XInt256 x, XInt256 y) {
            return (x > y) ? x : y;
        }

        public static XInt256 Min(XInt256 x, XInt256 y) {
            return (x <= y) ? x : y;
        }

        static XInt256 INumber<XInt256>.MaxNumber(XInt256 x, XInt256 y) {
            return Max(x, y);
        }

        static XInt256 INumber<XInt256>.MinNumber(XInt256 x, XInt256 y) {
            return Min(x, y);
        }

        static XInt256 INumberBase<XInt256>.MaxMagnitude(XInt256 x, XInt256 y) {
            return Max(x, y);
        }

        static XInt256 INumberBase<XInt256>.MaxMagnitudeNumber(XInt256 x, XInt256 y) {
            return Max(x, y);
        }

        static XInt256 INumberBase<XInt256>.MinMagnitude(XInt256 x, XInt256 y) {
            return Min(x, y);
        }

        static XInt256 INumberBase<XInt256>.MinMagnitudeNumber(XInt256 x, XInt256 y) {
            return Min(x, y);
        }

        int IBinaryInteger<XInt256>.GetShortestBitLength() {
            return unchecked( 256 - Numerics.DoubleArithmetic.CountLeadingZeros(lo, hi));
        }

        static XInt256 IBinaryNumber<XInt256>.AllBitsSet {

            get => new(~(SUInt128)0, ~(HInt128)0);
        }

        static XInt256 INumberBase<XInt256>.Abs(XInt256 value) {
            return value;
        }

        static bool INumberBase<XInt256>.IsCanonical(XInt256 value) {
            return true;
        }

        static bool INumberBase<XInt256>.IsComplexNumber(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsEvenInteger(XInt256 value) {
            return 0 == (1 & unchecked((int)value.lo));
        }

        static bool INumberBase<XInt256>.IsFinite(XInt256 value) {
            return true;
        }

        static bool INumberBase<XInt256>.IsImaginaryNumber(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsInfinity(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsInteger(XInt256 value) {
            return true;
        }

        static bool INumberBase<XInt256>.IsNaN(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsNegative(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsNegativeInfinity(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsNormal(XInt256 value) {
            return true;
        }

        static bool INumberBase<XInt256>.IsOddInteger(XInt256 value) {
            return (1 & unchecked((int)value.lo)).AsBooleanUnsafe();
        }

        static bool INumberBase<XInt256>.IsPositive(XInt256 value) {
            return !value.IsZero;
        }

        static bool INumberBase<XInt256>.IsPositiveInfinity(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsRealNumber(XInt256 value) {
            return true;
        }

        static bool INumberBase<XInt256>.IsSubnormal(XInt256 value) {
            return false;
        }

        static bool INumberBase<XInt256>.IsZero(XInt256 value) {
            return value.IsZero;
        }

        static bool INumberBase<XInt256>.TryConvertFromChecked<TOther>(TOther value, [NotNullWhen(true)] out XInt256 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<XInt256>.TryConvertFromSaturating<TOther>(TOther value, [NotNullWhen(true)] out XInt256 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<XInt256>.TryConvertFromTruncating<TOther>(TOther value, [NotNullWhen(true)] out XInt256 result) {
            throw new NotImplementedException();
        }

        static bool INumberBase<XInt256>.TryConvertToChecked<TOther>(XInt256 value, [NotNullWhen(true)] out TOther? result) where TOther : default {
            throw new NotImplementedException();
        }

        static bool INumberBase<XInt256>.TryConvertToSaturating<TOther>(XInt256 value, [NotNullWhen(true)] out TOther? result) where TOther : default {
            throw new NotImplementedException();
        }

        static bool INumberBase<XInt256>.TryConvertToTruncating<TOther>(XInt256 value, [NotNullWhen(true)] out TOther? result) where TOther : default {
            throw new NotImplementedException();
        }
        #endregion
#endif

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

        /*
        SInt128 IConvertible.ToInt128(IFormatProvider? provider) {
            if (SInt128.MinValue <= this && this <= SInt128.MaxValue) {
                return unchecked((SInt128)this.lo).ToInt128(provider);
            }
            return ((long)Int32.MinValue - 1).ToInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        SUInt128 IConvertible.ToUInt128(IFormatProvider? provider) {
            if (SUInt128.MinValue <= this && this <= SUInt128.MaxValue) {
                return unchecked((SUInt128)this.lo).ToUInt128(provider);
            }
            return ((long)UInt32.MinValue - 1).ToUInt32(provider); // Let the underlying standard libraries raise the exception.
        }
        */

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
                    return ex.Message.Replace(nameof(Int64), nameof(UInt256));
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
        ///     <para>Parses an unsigned integer.</para>
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     <para>The Number must be in format <c>[1-9][0-9]*</c>.</para>
        /// </remarks>
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.None)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool TryParseCStyleNormalizedU256(string s, out XInt256 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                SUInt128 lo;
                SUInt128 hi;
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
                            goto L_f;
                        }
                    }
                    if (s.Length == i) {
                        result = new XInt256(lo, hi);
                        return true;
                    }
                }
            }
        L_f:;
            Try_HandleOutParameterIfFalse(out result);
            return false;
        }

        public static bool TryParseCStyleNormalizedX256(string s, out XInt256 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                SUInt128 lo;
                SUInt128 hi;
                var d = c - '0';
                if ((0 < d && d < 10) || (17 <= d && d < 23)) {
                    if (10 <= d) {
                        d -= 7;
                    }
                    lo = unchecked((uint)d);
                    hi = 0;
                    while (s.Length > i) {
                        c = s[i++];
                        d = c - '0';
                        if ((0 > d || d >= 10) && (17 > d || d >= 23)) {
                            goto L_f;
                        }
                        if (10 <= d) {
                            d -= 7;
                        }
                        // TODO: ...
                        try {
                            {
                                var ignored = checked(0 - unchecked(hi >> (128 - 4)));
                            }
                            lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, 4, out hi);
                            lo = Numerics.DoubleArithmetic.AddUnsigned(lo, hi, unchecked((uint)d), 0, out hi);
                        } catch (ArithmeticException) {
                            goto L_f;
                        }
                    }
                    if (s.Length == i) {
                        // TODO: ...
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
        //private static char[] buffer_ToStringCStyleU256 {

        //    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //    get {
        //        return Int256.buffer_ToStringCStyleI256;
        //    }
        //}

        public string ToStringCStyleU256() {
            return new string(ToStringCStyleU256(stackalloc char[40]));
        }

        public Span<char> ToStringCStyleU256(Span<char> buffer) {
            var lo = this.lo;
            var hi = unchecked((SUInt128)this.hi);
            var i = buffer.Length;
            {
                SUInt128 r_lo;
                SUInt128 r_hi;
                do {
                    lo = Numerics.DoubleArithmetic.DivRem(lo, hi, 10, 0, out r_lo, out r_hi, out hi);
                    buffer[--i] = unchecked((char)('0' + r_lo));
                } while (0 != lo || 0 != hi);
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
                return this.ToStringCStyleU256();
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        public string ToString(IFormatProvider? formatProvider) {
            return this.ToString(null, formatProvider);
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            // TODO: ...
            {
                return this.ToStringCStyleU256();
            }
        }

        public static implicit operator BigInteger(XInt256 value) {
            // TODO: Perf
            Span<byte> buffer = stackalloc byte[32];
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, Numerics. DoubleArithmetic.GetLowPart(value.lo));
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(8), Numerics.DoubleArithmetic.GetHighPart(value.lo));
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(16), Numerics.DoubleArithmetic.GetLowPart(value.hi));
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(24), Numerics.DoubleArithmetic.GetHighPart(value.hi));
            return new BigInteger(buffer, isUnsigned: true, isBigEndian: false);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt256(BigInteger value) {
            var lo = unchecked((SUInt128)(value & SUInt128.MaxValue));
            var hi = unchecked((SUInt128)((value >> 128) & SUInt128.MaxValue));
            return new XInt256(lo, hi);
        }

        public static explicit operator checked XInt256(BigInteger value) {
            var lo = unchecked((SUInt128)(value & SUInt128.MaxValue));
            var hi = checked((SUInt128)(value >> 128));
            return new XInt256(lo, hi);
        }
#else
        public static explicit operator XInt256(BigInteger value) {
            var lo = unchecked((SUInt128)(value & SUInt128.MaxValue));
            var hi = unchecked((SUInt128)((value >> 128) & SUInt128.MaxValue));
            if (value != hi) {
                throw new OverflowException(); 
            }
            return new XInt256(lo, hi);
        }
#endif
#endif
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static XInt256 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            throw new NotImplementedException();
        }

        public static XInt256 Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Parse(s.AsSpan(), style, provider);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out XInt256 result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out XInt256 result) {
            return TryParse(s.AsSpan(), style, provider, out result);
        }

        static XInt256 ISpanParsable<XInt256>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Integer, provider);
        }

        static bool ISpanParsable<XInt256>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out XInt256 result) {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }

        static XInt256 IParsable<XInt256>.Parse(string s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Integer, provider);
        }

        static bool IParsable<XInt256>.TryParse(string? s, IFormatProvider? provider, out XInt256 result) {
            return TryParse(s.AsSpan(), NumberStyles.Integer, provider, out result);
        }

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            throw new NotImplementedException();
        }
#endif
    }
}

namespace UltimateOrb {
    using Internal;
    using UltimateOrb.Runtime.CompilerServices;

    using static UltimateOrb.Utilities.ThrowHelper;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using XInt256 = UInt256;
    using OInt256 = Int256;
    using HInt128 = UInt128;
    using SUInt128 = System.UInt128;
    using SInt128 = System.Int128;

    public partial struct UInt256 {

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
                var lo = Numerics.DoubleArithmetic.DivRem(dividend.lo, dividend.hi, divisor.lo, divisor.hi, out Unsafe.AsRef(in remainder.lo), out Unsafe.AsRef(in remainder.hi), out SUInt128 hi);
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
                            result_lo = Numerics.DoubleArithmetic.MultiplyUnsigned(result_lo, result_hi, lo, hi, out result_hi);
                        }
                        if (0 != (exponent >>= 1)) {
                            lo = Numerics.DoubleArithmetic.MultiplyUnsigned(lo, hi, lo, hi, out hi);
                            continue;
                        }
                        break;
                    }
                    return new XInt256(result_lo, result_hi);
                }
                if (0 == exponent || (1 == lo && 0 == hi)) {
                    return XInt256.One;
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
                var result_lo_lo = Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out HInt128 result_lo_hi, out HInt128 result_hi_lo, out HInt128 result_hi_hi);
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

        public static partial class EuclideanAlgorithm {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 GreatestCommonDivisor(XInt256 first, XInt256 second) {
                System.Diagnostics.Contracts.Contract.Ensures((System.Diagnostics.Contracts.Contract.Result<ulong>() == 0u) == (System.Diagnostics.Contracts.Contract.OldValue(first) == 0u && System.Diagnostics.Contracts.Contract.OldValue(second) == 0u));
                System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<ulong>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<ulong>());
                System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<ulong>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<ulong>());
                unchecked {
                    var first_lo = first.lo;
                    var first_hi = first.hi;
                    var second_lo = second.lo;
                    var second_hi = second.hi;
                    if (0 == second_lo && 0 == second_hi) {
                        return first;
                    }
                    if (0 == first_lo && 0 == first_hi) {
                        return second;
                    }
                    if (Numerics.DoubleArithmetic.GreaterThan(first_lo, first_hi, second_lo, second_hi)) {
                        first_lo = Numerics.DoubleArithmetic.Remainder(first_lo, first_hi, second_lo, second_hi, out first_hi);
                        if (0 == first_lo && 0 == first_hi) {
                            return second;
                        }
                    } else {
                        second_lo = Numerics.DoubleArithmetic.Remainder(second_lo, second_hi, first_lo, first_hi, out second_hi);
                        if (0 == second_lo && 0 == second_hi) {
                            return first;
                        }
                    }
                    var c = 0;
                    for (; 0 == (1 & unchecked((uint)first_lo | (uint)second_lo));) {
                        unchecked {
                            ++c;
                        }
                        first_lo = Numerics.DoubleArithmetic.ShiftRight(first_lo, first_hi, out first_hi);
                        second_lo = Numerics.DoubleArithmetic.ShiftRight(second_lo, second_hi, out second_hi);
                    }
                    first_lo = GreatestCommonDivisorPartialStub0002(first_lo, first_hi, second_lo, second_hi, out first_hi);
                    first_lo = Numerics.DoubleArithmetic.ShiftLeft(first_lo, first_hi, c, out first_hi);
                    return new XInt256(first_lo, first_hi);
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static ulong GreatestCommonDivisorPartialStub0001(ulong first_lo, ulong first_hi, ulong second_lo, ulong second_hi, out ulong result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & second_lo) && (second_hi > 0 || second_lo > 1u));
                unchecked {
                    var first_lo_ = first_lo;
                    var first_hi_ = first_hi;
                    var second_lo_ = second_lo;
                    var second_hi_ = second_hi;
                    if (0 == first_lo_ && 0 == first_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                L_Lt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = first_lo_;
                        var t_hi = first_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.AddUnchecked(second_lo_, second_hi_, t_lo, t_hi, out second_hi_);
                    } else {
                        second_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(second_lo_, second_hi_, first_lo_, first_hi_, out second_hi_);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                    }
                    while (0 == (1 & second_lo_)) {
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, out second_hi_);
                    }
                    if (1 == second_lo_ && 0 == second_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(second_lo_, second_hi_, first_lo_, first_hi_)) {
                        goto L_Lt;
                    }
                L_Gt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = second_lo_;
                        var t_hi = second_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.AddUnchecked(first_lo_, first_hi_, t_lo, t_hi, out first_hi_);
                    } else {
                        first_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(first_lo_, first_hi_, second_lo_, second_hi_, out first_hi_);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                    goto L_Lt;
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static ulong GreatestCommonDivisorPartialStub0002(ulong first_lo, ulong first_hi, ulong second_lo, ulong second_hi, out ulong result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & first_lo) || 0 != (1 & second_lo));
                unchecked {
                    if (0 != (1 & second_lo)) {
                        if ((1 == first_lo && 0 == first_hi) || (1 == second_lo && 0 == second_hi)) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(first_lo, first_hi, second_lo, second_hi, out result_hi);
                        }
                    } else {
                        if (1 == first_lo && 0 == first_hi) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(second_lo, second_hi, first_lo, first_hi, out result_hi);
                        }
                    }
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static UInt128 GreatestCommonDivisorPartialStub0001(UInt128 first_lo, UInt128 first_hi, UInt128 second_lo, UInt128 second_hi, out UInt128 result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & second_lo) && (second_hi > 0 || second_lo > 1u));
                unchecked {
                    var first_lo_ = first_lo;
                    var first_hi_ = first_hi;
                    var second_lo_ = second_lo;
                    var second_hi_ = second_hi;
                    if (0 == first_lo_ && 0 == first_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                L_Lt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = first_lo_;
                        var t_hi = first_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.AddUnchecked(second_lo_, second_hi_, t_lo, t_hi, out second_hi_);
                    } else {
                        second_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(second_lo_, second_hi_, first_lo_, first_hi_, out second_hi_);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                    }
                    while (0 == (1 & second_lo_)) {
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, out second_hi_);
                    }
                    if (1 == second_lo_ && 0 == second_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(second_lo_, second_hi_, first_lo_, first_hi_)) {
                        goto L_Lt;
                    }
                L_Gt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = second_lo_;
                        var t_hi = second_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.AddUnchecked(first_lo_, first_hi_, t_lo, t_hi, out first_hi_);
                    } else {
                        first_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(first_lo_, first_hi_, second_lo_, second_hi_, out first_hi_);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                    goto L_Lt;
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static UInt128 GreatestCommonDivisorPartialStub0002(UInt128 first_lo, UInt128 first_hi, UInt128 second_lo, UInt128 second_hi, out UInt128 result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & first_lo) || 0 != (1 & second_lo));
                unchecked {
                    if (0 != (1 & second_lo)) {
                        if ((1 == first_lo && 0 == first_hi) || (1 == second_lo && 0 == second_hi)) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(first_lo, first_hi, second_lo, second_hi, out result_hi);
                        }
                    } else {
                        if (1 == first_lo && 0 == first_hi) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(second_lo, second_hi, first_lo, first_hi, out result_hi);
                        }
                    }
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static System.UInt128 GreatestCommonDivisorPartialStub0001(System.UInt128 first_lo, System.UInt128 first_hi, System.UInt128 second_lo, System.UInt128 second_hi, out System.UInt128 result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & second_lo) && (second_hi > 0 || second_lo > 1u));
                unchecked {
                    var first_lo_ = first_lo;
                    var first_hi_ = first_hi;
                    var second_lo_ = second_lo;
                    var second_hi_ = second_hi;
                    if (0 == first_lo_ && 0 == first_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                L_Lt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = first_lo_;
                        var t_hi = first_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        second_lo_ = Numerics.DoubleArithmetic.AddUnchecked(second_lo_, second_hi_, t_lo, t_hi, out second_hi_);
                    } else {
                        second_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(second_lo_, second_hi_, first_lo_, first_hi_, out second_hi_);
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, 2, out second_hi_);
                    }
                    while (0 == (1 & second_lo_)) {
                        second_lo_ = Numerics.DoubleArithmetic.ShiftRight(second_lo_, second_hi_, out second_hi_);
                    }
                    if (1 == second_lo_ && 0 == second_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(second_lo_, second_hi_, first_lo_, first_hi_)) {
                        goto L_Lt;
                    }
                L_Gt:;
                    if (0 != (2 & ((uint)first_lo_ ^ (uint)second_lo_))) {
                        var t_lo = second_lo_;
                        var t_hi = second_hi_;
                        t_lo = Numerics.DoubleArithmetic.ShiftRight(t_lo, t_hi, 2, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                        t_lo = Numerics.DoubleArithmetic.IncreaseUnchecked(t_lo, t_hi, out t_hi);
                        first_lo_ = Numerics.DoubleArithmetic.AddUnchecked(first_lo_, first_hi_, t_lo, t_hi, out first_hi_);
                    } else {
                        first_lo_ = Numerics.DoubleArithmetic.SubtractUnchecked(first_lo_, first_hi_, second_lo_, second_hi_, out first_hi_);
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, 2, out first_hi_);
                    }
                    while (0 == (1 & first_lo_)) {
                        first_lo_ = Numerics.DoubleArithmetic.ShiftRight(first_lo_, first_hi_, out first_hi_);
                    }
                    if (1 == first_lo_ && 0 == first_hi_) {
                        result_hi = 0;
                        return 1;
                    }
                    if (first_lo_ == second_lo_ && first_hi_ == second_hi_) {
                        result_hi = second_hi_;
                        return second_lo_;
                    } else if (Numerics.DoubleArithmetic.GreaterThan(first_lo_, first_hi_, second_lo_, second_hi_)) {
                        goto L_Gt;
                    }
                    goto L_Lt;
                }
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            internal static System.UInt128 GreatestCommonDivisorPartialStub0002(System.UInt128 first_lo, System.UInt128 first_hi, System.UInt128 second_lo, System.UInt128 second_hi, out System.UInt128 result_hi) {
                System.Diagnostics.Contracts.Contract.Requires(0 != (1 & first_lo) || 0 != (1 & second_lo));
                unchecked {
                    if (0 != (1 & second_lo)) {
                        if ((1 == first_lo && 0 == first_hi) || (1 == second_lo && 0 == second_hi)) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(first_lo, first_hi, second_lo, second_hi, out result_hi);
                        }
                    } else {
                        if (1 == first_lo && 0 == first_hi) {
                            result_hi = 0;
                            return 1;
                        } else {
                            return GreatestCommonDivisorPartialStub0001(second_lo, second_hi, first_lo, first_hi, out result_hi);
                        }
                    }
                }
            }
        }

        public static partial class ZZOverNZZModule {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Sum(XInt256 n, XInt256 first, XInt256 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                return unchecked(n <= (second = (first + second)) || first > second ? second - n : second);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Double(XInt256 n, XInt256 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                return Sum(n, value, value);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Difference(XInt256 n, XInt256 first, XInt256 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                return unchecked(second > first ? first - second + n : first - second);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Opposite(XInt256 n, XInt256 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                return Difference(n, 0u, value);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Product(XInt256 n, XInt256 first, XInt256 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                var p_lo_lo = UltimateOrb.Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out var p_lo_hi, out var p_hi_lo, out var p_hi_hi);
                var lo = UltimateOrb.Numerics.DoubleArithmetic.BigRemNoThrowWhenOverflow(p_lo_lo, p_lo_hi, p_hi_lo, p_hi_hi, n.lo, n.hi, out var hi);
                return new XInt256(lo, hi);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Square(XInt256 n, XInt256 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                var p_lo_lo = UltimateOrb.Numerics.DoubleArithmetic.BigSquare(value.lo, value.hi, out var p_lo_hi, out var p_hi_lo, out var p_hi_hi);
                var lo = UltimateOrb.Numerics.DoubleArithmetic.BigRemNoThrowWhenOverflow(p_lo_lo, p_lo_hi, p_hi_lo, p_hi_hi, n.lo, n.hi, out var hi);
                return new XInt256(lo, hi);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Power(XInt256 n, XInt256 @base, uint exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                XInt256 j = 0u;
                if (n != 1u) {
                    j = 1u;
                }
                for (; ; ) {
                    if (0u != (exponent & 1u)) {
                        j = Product(n, @base, j);
                    }
                    if (0u == (exponent >>= 1)) {
                        break;
                    }
                    @base = Square(n, @base);
                }
                return j;
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Power(XInt256 n, XInt256 @base, ulong exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                XInt256 j = 0u;
                if (n != 1u) {
                    j = 1u;
                }
                for (; ; ) {
                    if (0u != (exponent & 1u)) {
                        j = Product(n, @base, j);
                    }
                    if (0u == (exponent >>= 1)) {
                        break;
                    }
                    @base = Square(n, @base);
                }
                return j;
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 Power(XInt256 n, XInt256 @base, XInt256 exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt256>());
                XInt256 j = 0u;
                if (n != 1u) {
                    j = 1u;
                }
                for (; ; ) {
                    if (0u != (exponent & 1u)) {
                        j = Product(n, @base, j);
                    }
                    if (0u == (exponent >>= 1)) {
                        break;
                    }
                    @base = Square(n, @base);
                }
                return j;
            }
        }

        public static partial class Converter {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveOptimization)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 ToUInt256(double value) {
                const int BitSize = 128;
                const int ExponentBitSize = 11;
                const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
                const int SignBitSize = 1;
                const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

                // SInt128 SignMask = unchecked((SInt128)checked((SInt128)1u << (ExponentBitSize + FractionBitSize)));
                SInt128 ExponentMask = unchecked((SInt128)checked((((SInt128)1u << ExponentBitSize) - 1u) << FractionBitSize));
                SInt128 FractionMask = unchecked((SInt128)checked(((SInt128)1u << FractionBitSize) - 1u));

                var b = BitConverter.DoubleToInt64Bits(value);
                var e = checked((int)(ExponentMask >> FractionBitSize)) & unchecked((int)(b >> FractionBitSize));
                if (e >= checked(ExponentBias - 1)) {
                    b.ThrowOnNegative();
                    checked(checked(ExponentBias - 1 + 256) - e).Ignore();
                    {
                        e = unchecked(e - (FractionBitSize + ExponentBias));
                        var lo = unchecked((SUInt128)(((SInt128)1 << FractionBitSize) | (FractionMask & b)));
                        var hi = (SUInt128)0;
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
                }
                return Zero;
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt256 ToUInt256(Single value) {
                return ToUInt256(unchecked((double)(Single)value));
            }
        }
    }
}

namespace Internal.System {

    using static UltimateOrb.Utilities.ThrowHelper;

    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;

    using SUInt128 = global::System.UInt128;
    using SInt128 = global::System.Int128;

    public static partial class Converter {

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        [Obsolete("Not implemented")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Half ToHalf(SUInt128 value) {
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

            var value_ = value;
            if (0 != value_) {
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                var s = unchecked((SUInt128)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = value_ & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((SUInt128)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                // TODO:
                // return unchecked((Half)BitConverter.Int128BitsToQuadruple(unchecked((SInt128)s)));
            }
            return default;
        }
#endif

        [Obsolete("Not implemented")]
        [global::System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Single ToSingle(SUInt128 value) {
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

            var value_ = value;
            if (0 != value_) {
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((SUInt128)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((SInt128)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                var s = unchecked((SUInt128)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = value_ & unchecked(((SUInt128)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((SUInt128)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((SUInt128)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                // TODO:
                // return unchecked((Single)BitConverter.Int128BitsToQuadruple(unchecked((SInt128)s)));
            }
            return default;
        }
    }
}