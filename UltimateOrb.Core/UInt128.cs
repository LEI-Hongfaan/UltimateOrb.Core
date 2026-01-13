// WIP: Checked operators
// #define LEGACY_OPERATOR_CHECKNESS
using Internal;
using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using UltimateOrb.Numerics;
using UltimateOrb.Utilities;
using static UltimateOrb.Utilities.SignConverter;

namespace UltimateOrb {
    using static global::Internal.System.Converter;
    using static global::Internal.System.IConvertibleModule;
    using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
    using static UltimateOrb.Utilities.ThrowHelper;
    using static UltimateOrb.XInt128Helpers;
#if NET7_0_OR_GREATER
    using SystemXInt128 = System.UInt128;
#endif
    using HInt64 = UInt64;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;
    using OInt128 = Int128;
    using XInt128 = UInt128;

    [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Interoperability", "CA1413:AvoidNonpublicFieldsInComVisibleValueTypes")]
    [System.CLSCompliantAttribute(false)]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    [System.SerializableAttribute()]
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 16)]
    public readonly partial struct UInt128
        : IEquatable<XInt128>, IComparable<XInt128>, IComparable
#if NET7_0_OR_GREATER
#if !LEGACY_OPERATOR_CHECKNESS
        , INumberBase<XInt128>, IBinaryInteger<XInt128>, IUnsignedNumber<XInt128>
#endif
        , IMinMaxValue<XInt128>
#endif
#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        , IConvertible, IFormattable
#if NET8_0_OR_GREATER
        , IUtf8SpanFormattable
#endif
#endif
    {

#if NET7_0_OR_GREATER
        private readonly SystemXInt128 bits;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [UnscopedRef]
        private readonly ref readonly UInt64 lo {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref bits.lo;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [UnscopedRef]
        private readonly ref readonly HInt64 hi {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref bits.hi;
        }
#else
#if BIGENDIAN
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HInt64 hi;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64 lo;
#else
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UInt64 lo;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HInt64 hi;
#endif
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
        internal UInt128(UInt64 lo, HInt64 hi) {
#if NET7_0_OR_GREATER
            this.bits = new (lower: lo, upper: unchecked((UInt64)hi));
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt128 IAdditiveIdentity<XInt128, XInt128>.AdditiveIdentity {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return default(XInt128);
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt128 IMultiplicativeIdentity<XInt128, XInt128>.MultiplicativeIdentity {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(1, 0);
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
        public static XInt128 Two {

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                return new XInt128(2, 0);
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
                return false;
            }
        }

#if NET7_0_OR_GREATER
        static int INumberBase<XInt128>.Radix {

            get => 2;
        }

        /*
        [DiscardableAfterILLink]
        [Obsolete]
        internal static partial class StubILLinkHintDelegates<T> where T : INumberBase<T> {
            public delegate bool TryParseSpanFunc(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
            public delegate T ParseStringFunc(string s, NumberStyles style, IFormatProvider? provider);
            public delegate T ParseSpanFunc(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider);
            public delegate T ParseUtf8SpanFunc(ReadOnlySpan<byte> s, NumberStyles style, IFormatProvider? provider);
            public delegate bool TryParseStringFunc([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
            public delegate bool TryParseUtf8SpanFunc(ReadOnlySpan<byte> utf8Text, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out T result);
            public delegate T AbsFunc(T value);
            public delegate bool IsCanonicalFunc(T value);
            public delegate bool IsComplexNumberFunc(T value);
            public delegate bool IsEvenIntegerFunc(T value);
            public delegate bool IsFiniteFunc(T value);
            public delegate bool IsImaginaryNumberFunc(T value);
            public delegate bool IsInfinityFunc(T value);
            public delegate bool IsIntegerFunc(T value);
            public delegate bool IsNaNFunc(T value);
            public delegate bool IsNegativeFunc(T value);
            public delegate bool IsNegativeInfinityFunc(T value);
            public delegate bool IsNormalFunc(T value);
            public delegate bool IsOddIntegerFunc(T value);
            public delegate bool IsPositiveFunc(T value);
            public delegate bool IsPositiveInfinityFunc(T value);
            public delegate bool IsRealNumberFunc(T value);
            public delegate bool IsSubnormalFunc(T value);
            public delegate bool IsZeroFunc(T value);
            public delegate T MaxMagnitudeFunc(T x, T y);
            public delegate T MaxMagnitudeNumberFunc(T x, T y);
            public delegate T MinMagnitudeFunc(T x, T y);
            public delegate T MinMagnitudeNumberFunc(T x, T y);
            public delegate T CreateCheckedFunc(T value);
            public delegate T CreateSaturatingFunc(T value);
            public delegate T CreateTruncatingFunc(T value);
            public delegate bool TryConvertFromCheckedFunc(T value, [MaybeNullWhen(false)] out T result);
            public delegate bool TryConvertFromSaturatingFunc(T value, [MaybeNullWhen(false)] out T result);
            public delegate bool TryConvertFromTruncatingFunc(T value, [MaybeNullWhen(false)] out T result);
            public delegate bool TryConvertToCheckedFunc(T value, [MaybeNullWhen(false)] out T result);
            public delegate bool TryConvertToSaturatingFunc(T value, [MaybeNullWhen(false)] out T result);
            public delegate bool TryConvertToTruncatingFunc(T value, [MaybeNullWhen(false)] out T result);
        }

        [DiscardableAfterILLink]
        [Obsolete]
        internal static void StubILLinkHint<TBase>() where TBase : INumberBase<TBase> {
            // Properties
            GC.KeepAlive(TBase.One);
            GC.KeepAlive(TBase.Radix);
            GC.KeepAlive(TBase.Zero);
            // Methods
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.AbsFunc)TBase.Abs);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsCanonicalFunc)TBase.IsCanonical);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsComplexNumberFunc)TBase.IsComplexNumber);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsEvenIntegerFunc)TBase.IsEvenInteger);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsFiniteFunc)TBase.IsFinite);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsImaginaryNumberFunc)TBase.IsImaginaryNumber);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsInfinityFunc)TBase.IsInfinity);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsIntegerFunc)TBase.IsInteger);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsNaNFunc)TBase.IsNaN);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsNegativeFunc)TBase.IsNegative);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsNegativeInfinityFunc)TBase.IsNegativeInfinity);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsNormalFunc)TBase.IsNormal);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsOddIntegerFunc)TBase.IsOddInteger);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsPositiveFunc)TBase.IsPositive);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsPositiveInfinityFunc)TBase.IsPositiveInfinity);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsRealNumberFunc)TBase.IsRealNumber);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsSubnormalFunc)TBase.IsSubnormal);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.IsZeroFunc)TBase.IsZero);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.MaxMagnitudeFunc)TBase.MaxMagnitude);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.MaxMagnitudeNumberFunc)TBase.MaxMagnitudeNumber);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.MinMagnitudeFunc)TBase.MinMagnitude);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.MinMagnitudeNumberFunc)TBase.MinMagnitudeNumber);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.CreateCheckedFunc)TBase.CreateChecked<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.CreateSaturatingFunc)TBase.CreateSaturating<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.CreateTruncatingFunc)TBase.CreateTruncating<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.ParseStringFunc)TBase.Parse);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.ParseSpanFunc)TBase.Parse);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryParseStringFunc)TBase.TryParse);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryParseSpanFunc)TBase.TryParse);

            {
                
                // _ = TBase.TryParse(default(ReadOnlySpan<byte>), default(NumberStyles), default(IFormatProvider)!, out _);
                // TBase.Parse(default(ReadOnlySpan<byte>), default(NumberStyles), default(IFormatProvider)!);

            }



            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertFromCheckedFunc)TBase.TryConvertFromChecked<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertFromSaturatingFunc)TBase.TryConvertFromSaturating<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertFromTruncatingFunc)TBase.TryConvertFromTruncating<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertToCheckedFunc)TBase.TryConvertToChecked<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertToSaturatingFunc)TBase.TryConvertToSaturating<TBase>);
            GC.KeepAlive((StubILLinkHintDelegates<TBase>.TryConvertToTruncatingFunc)TBase.TryConvertToTruncating<TBase>);

            GC.KeepAlive(TBase.AdditiveIdentity);
            GC.KeepAlive(TBase.MultiplicativeIdentity);
            GC.KeepAlive(() => {
                var t = default(TBase)!;
                unchecked {
                    --t;
                    _ = t / t;
                    ++t;
                    _ = t * t;
                }
                checked {
                    --t;
                    _ = t / t;
                    ++t;
                    _ = t * t;
                }
            });

        }

        [DiscardableAfterILLink]
        [Obsolete]
        internal static void StubILLinkHint() {
            StubILLinkHint<XInt128>();


            // Add other necessary types here
        }*/
#endif

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
        static int INumber<XInt128>.Sign(XInt128 value) {
            return value.Sign;
        }
#endif

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
            if (n < 128) {
                if (n > 0) {
                    if (n <= 64) {
                        if (64 < n) {
                            return 0 == this.hi && 0 == this.lo >> (64 - n);
                        }
                        return this.CanConvertToUInt64;
                    }
                    return 0 == this.hi >> (128 - n);
                }
                return this.IsZero;
            }
            return true;
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt128 IShiftOperators<XInt128, int, XInt128>.operator >>(XInt128 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRightSigned(value.lo, value.hi, count, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 operator >>>(XInt128 value, int count) {
            var lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(value.lo, value.hi, count, out HInt64 hi);
            return new XInt128(lo, hi);
        }
#endif

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
                        return ex.Message.Replace(nameof(Int64), nameof(UInt128));
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

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 AsSigned() {
            return new OInt128(unchecked((UInt64)this.lo), unchecked((Int64)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToSignedUnchecked() {
            return new OInt128(unchecked((UInt64)this.lo), unchecked((Int64)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToSignedChecked() {
            return new OInt128(unchecked((UInt64)this.lo), checked((Int64)this.hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public OInt128 ToSigned() {
            return new OInt128(unchecked((UInt64)this.lo), checked((Int64)this.hi));
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
            return value.ToUnsignedChecked();
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(OInt128 value) {
            return value.ToUnsignedUnchecked();
        }
#endif

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [Obsolete]
#endif
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 op_ExplicitUnchecked(OInt128 value) {
            return value.ToUnsignedUnchecked();
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(byte value) {
            return new XInt128(unchecked((UInt64)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt16 value) {
            return new XInt128(unchecked((UInt64)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt32 value) {
            return new XInt128(unchecked((UInt64)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(UInt64 value) {
            return new XInt128(unchecked((UInt64)value), 0);
        }

#if NET7_0_OR_GREATER
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(System.UInt128 value) {
            return new XInt128(lo: value.GetLowPart(), hi: value.GetHighPart());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator System.UInt128(XInt128 value) {
            return new System.UInt128(upper: value.GetHighPart(), lower: value.GetLowPart());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(System.Int128 value) {
            return new XInt128(lo: value.GetLowPart(), hi: checked((HInt64)value.GetHighPart()));
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator XInt128(System.Int128 value) {
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
            System.Int128(XInt128 value) {
            return new System.Int128(upper: unchecked((UInt64)checked((Int64)value.GetHighPart())), lower: value.GetLowPart());
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator System.Int128(XInt128 value) {
            return new System.Int128(upper: unchecked((UInt64)unchecked((Int64)value.GetHighPart())), lower: value.GetLowPart());
        }
#endif
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static implicit operator XInt128(char value) {
            return new XInt128(lo: unchecked((UInt64)(uint)value), 0);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(sbyte value) {

            return new XInt128(unchecked((UInt64)checked((uint)(int)value)), 0);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(sbyte value) {
            return unchecked((XInt128)(Int128)(int)value);
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
            XInt128(Int16 value) {
            return new XInt128(unchecked((UInt64)checked((uint)(int)value)), 0);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(Int16 value) {
            return unchecked((XInt128)(Int128)(int)value);
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
            XInt128(Int32 value) {
            return new XInt128(unchecked((UInt64)checked((uint)(int)value)), 0);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(Int32 value) {
            return unchecked((XInt128)(Int128)(int)value);
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
            XInt128(Int64 value) {
            return new XInt128(checked((UInt64)value), 0);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(Int64 value) {
            return unchecked((XInt128)(Int128)value);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            byte(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((byte)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator byte(XInt128 value) {
            return unchecked((byte)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            UInt16(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((UInt16)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator UInt16(XInt128 value) {
            return unchecked((UInt16)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            UInt32(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((UInt32)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator UInt32(XInt128 value) {
            return unchecked((UInt32)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            UInt64(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return value.lo;
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator UInt64(XInt128 value) {
            return value.lo;
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            char(XInt128 value) {
            (checked(0 - unchecked((UInt64)value.hi))).Ignore(); // check overflow
            return checked((char)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator char(XInt128 value) {
            return unchecked((char)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            sbyte(XInt128 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((sbyte)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator sbyte(XInt128 value) {
            return unchecked((sbyte)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            Int16(XInt128 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((Int16)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator Int16(XInt128 value) {
            return unchecked((Int16)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            Int32(XInt128 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((Int32)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator Int32(XInt128 value) {
            return unchecked((Int32)value.lo);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            Int64(XInt128 value) {
            (checked(0 - value.hi)).Ignore(); // check overflow
            return checked((Int64)value.lo);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator Int64(XInt128 value) {
            return unchecked((Int64)value.lo);
        }
#endif

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(Half value) {
            return checked((XInt128)unchecked((double)value));
        }
#endif

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(Half value) {
            return unchecked((XInt128)unchecked((double)value));
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(Single value) {
            return checked((XInt128)unchecked((double)value));
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(Single value) {
            return unchecked((XInt128)unchecked((double)value));
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
            checked
#endif
            XInt128(double value) {
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
                {
                    e = unchecked(e - (FractionBitSize + ExponentBias));
                    var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                    var hi = (UInt64)0;
                    if (e < 0) {
                        var ol = (UInt64)0;
                        ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                        // Do NOT uncomment. (int)3.5 == 3
                        /*
                        // IEEE Std 754-2008 roundTiesToEven
                        if (unchecked((UInt64)Int64.MinValue) < ol || (unchecked((UInt64)Int64.MinValue) == ol && 0 != (1 & lo))) {
                            lo = unchecked(1 + lo);
                        }
                        */
                    } else {
                        lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                    }
                    if (0 > b) {
                        lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                    }
                    return new XInt128(lo, hi);
                }
            }
            return Zero;
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
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
                e = unchecked(e - (FractionBitSize + ExponentBias));
                var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                var hi = (UInt64)0;
                if (e < 0) {
                    var ol = (UInt64)0;
                    ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                    // Do NOT uncomment. (int)3.5 == 3
                    /*
                    // IEEE Std 754-2008 roundTiesToEven
                    if (unchecked((UInt64)Int64.MinValue) < ol || (unchecked((UInt64)Int64.MinValue) == ol && 0 != (1 & lo))) {
                        lo = unchecked(1 + lo);
                    }
                    */
                } else {
                    lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                }
                if (0 > b) {
                    lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                }
                return new XInt128(lo, hi);
            }
            return Zero;
        }
#endif

#if (NET5_0 || NET6_0 || NET5_0_OR_GREATER)
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Half(XInt128 value) {
            var lo = value.lo;
            var hi = value.hi;

            // We cannot write "return (Half)lo;" because of the double rounding issue.
            // See https://www.exploringbinary.com/double-rounding-errors-in-floating-point-conversions/ .
            return 0 != hi || 65520 <= lo ? Half.PositiveInfinity : BitConverter.Int16BitsToHalf(unchecked((Int16)ToHalfPartial((uint)lo)));
        }
#endif

        internal static int ToHalfPartial(uint value) {
            Debug.Assert(value <= 65519);
            unchecked {
                const int exponentBias = 15;
                const int mantissaBits = 10;

                if (value == 0) {
                    return 0;
                }

                // Count leading zeros to find the position of the highest set bit
                var leadingZeros = Mathematics.BinaryNumerals.CountLeadingZeros(value);

                int exponent = 32 - 1 + exponentBias - leadingZeros;

                uint shifted = value << (1 + leadingZeros);

                uint mantissa = shifted >>> (32 - mantissaBits);

                // Remaining bits for rounding (bits 21-0)
                uint remaining = shifted & 0x3FFFFF;

                // Apply rounding (round to nearest, ties to even)
                if (remaining > 0x200000 || (remaining == 0x200000 && 0 != (1 & mantissa))) {
                    ++mantissa;
                }

                // Combine into half-precision format (sign bit is 0 for unsigned input)
                return (exponent << mantissaBits) + (int)mantissa;
            }
        }

#if NET7_0_OR_GREATER
        private static Single ToSingle(System.UInt128 value) {
            return ToSingle(unchecked((UInt64)(value)), unchecked((UInt64)(value >>> 64)));
        }
#endif

        internal static Single ToSingle(UInt64 lo, UInt64 hi) {
            unchecked {
                if (0 == (lo | hi)) {
                    return default; // 0.0F
                }
                var lz = UltimateOrb.Numerics.DoubleArithmetic.CountLeadingZeros(lo, hi);
                UInt32 resultBits;
                if (lz < 104) {
                    var shift = 103 - lz;

                    // Logical shift right
                    var shrdVal = UltimateOrb.Numerics.DoubleArithmetic.ShiftRight(lo, hi, shift);
                    var shrxVal = hi >> shift;
                    var shrVal = (shift & 64) == 0 ? shrdVal : shrxVal;

                    // To prepare for rounding the code extracts bits from the lower 24 bits.
                    var mantissaCandidate = (UInt32)shrVal & 0xFFFFFF;
                    ++mantissaCandidate; // rounding adjustment
                    mantissaCandidate >>= 1;

                    // Count trailing zeros
                    var tzc = UltimateOrb.Numerics.DoubleArithmetic.CountTrailingZeros(lo, hi); ;

                    resultBits = mantissaCandidate & ~(shift == tzc ? 1u : 0u); // cancel the rounding adjustment if no extra bits
                } else {
                    // The number is small enough that a simple left-shift of lo is enough.
                    var shift = lz - 104;
                    var shifted = (UInt32)(lo << shift);
                    resultBits = shifted & 0x7FFFFF;
                }
                return BitConverter.UInt32BitsToSingle(0X7f000000 + resultBits - ((UInt32)(uint)lz << 23));
            }
        }

        [Obsolete]
        internal static Int32 ToSinglePartial(UInt64 value_lo, UInt64 value_hi) {
            Debug.Assert(value_hi != 0);
            unchecked {
                const int exponentBias = 127;
                const int mantissaBitCount = 23;
                const int nonMantissaBitCount = 32 - mantissaBitCount;

                // Count leading zeros to find the position of the highest set bit
                int shiftCount = 1 + BitOperations.LeadingZeroCount(value_hi);

                int exponent = 128 + exponentBias - shiftCount;

                UInt64 shifted = 1 == value_hi ? value_lo : (value_lo >>> (64 - shiftCount)) | (value_hi << shiftCount);

                Int32 mantissa = (int)(shifted >>> (32 + nonMantissaBitCount));

                // Remaining bits for rounding (bits 40-0)
                UInt64 remaining = shifted & 0X1ffffffffff;

                if (!(1 == value_hi) && 0 != value_lo << shiftCount) {
                    remaining |= 1;
                }

                // Apply rounding (round to nearest, ties to even)
                if (remaining > 0X10000000000 || (remaining == 0X10000000000 && 0 != (1 & mantissa))) {
                    ++mantissa;
                }

                // Combine into half-precision format (sign bit is 0 for unsigned input)
                return (exponent << mantissaBitCount) + mantissa;
            }
        }

        // Make the conversion explicit due to precision loss may be significant.
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator Single(XInt128 value) {
            return ToSingle(value.lo, value.hi);
        }

        [Obsolete]
        internal static double ToDouble(UInt64 lo, UInt64 hi) {
            const int BitSize = 64;
            const int ExponentBitSize = 11;
            const int ExponentBias = unchecked(checked((1 << (ExponentBitSize - 1))) - 1);
            const int SignBitSize = 1;
            const int FractionBitSize = checked(BitSize - SignBitSize - ExponentBitSize);

            if (0 != hi) {
                var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)hi));
                /*
                var c = 0;
                for (var tmp = hi; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                */
                var s = unchecked((UInt64)(checked(128 - 1 + ExponentBias) - c)) << FractionBitSize;
                lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, unchecked(1 + c), out hi);
                var lo0 = lo & unchecked(((UInt64)1 << checked(BitSize - FractionBitSize)) - 1);
                lo = Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, checked(BitSize - FractionBitSize), out hi);
                s |= hi;
                // IEEE Std 754-2008 roundTiesToEven
                if (unchecked((UInt64)Int64.MinValue) < lo || (unchecked((UInt64)Int64.MinValue) == lo && (lo0 > 0 || (lo0 == 0 && 0 != (1 & hi))))) {
                    s = unchecked(1 + s);
                }
                return BitConverter.Int64BitsToDouble(unchecked((Int64)s));
            }
            return unchecked((double)lo);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator double(XInt128 value) {
            unchecked {
                // This code is based on `u128_to_f64_round` from m-ou-se/floatconv
                // Copyright (c) 2020 Mara Bos <m-ou.se@m-ou.se>. All rights reserved.
                //
                // Licensed under the BSD 2 - Clause "Simplified" License
                // See THIRD-PARTY-NOTICES.TXT for the full license text

                const double TwoPow52 = 4503599627370496.0;
                const double TwoPow76 = 75557863725914323419136.0;
                const double TwoExp104 = 20282409603651670423947251286016.0;
                const double TwoPow128 = 340282366920938463463374607431768211456.0;

                const ulong TwoPow52Bits = 0x4330000000000000;
                const ulong TwoPow76Bits = 0x44B0000000000000;
                const ulong TwoExp104Bits = 0x4670000000000000;
                const ulong TwoPow128Bits = 0x47F0000000000000;

                var lo = value.lo;
                var hi = value.hi;
                if ((hi >> 40) == 0) {
                    // value < (2^88)

                    // For values greater than ulong.MaxValue but less than 2^104 this takes advantage
                    // that we can represent both "halves" of the uint128 within the 52-bit mantissa of
                    // a pair of doubles.

                    double lower = BitConverter.UInt64BitsToDouble(TwoPow52Bits | ((lo << 12) >> 12)) - TwoPow52;
                    double upper = BitConverter.UInt64BitsToDouble(TwoExp104Bits | UltimateOrb.Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, 52, out _)) - TwoExp104;

                    return lower + upper;
                } else {
                    // For values greater than 2^88 we basically do the same as before but we need to account
                    // for the precision loss that double will have. As such, the lower value effectively drops the
                    // lowest 24 bits and then or's them back to ensure rounding stays correct.

                    double lower = BitConverter.UInt64BitsToDouble(TwoPow76Bits | (UltimateOrb.Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, 12, out _) >> 12) | 0xFFFFFF & lo) - TwoPow76;
                    double upper = BitConverter.UInt64BitsToDouble(TwoPow128Bits | UltimateOrb.Numerics.DoubleArithmetic.ShiftRightUnsigned(lo, hi, 76, out _)) - TwoPow128;

                    return lower + upper;
                }
            }
        }

#if FEATURE_STANDARD_LIBRARY_INTEROPERABILITY_FORMATTING_AND_CONVERSION
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static explicit operator global::System.Decimal(XInt128 value) {
            checked((UInt32)value.hi).Ignore(); // check overflow
            return new decimal(unchecked((Int32)(value.lo >> (32 * 0))), unchecked((Int32)(value.lo >> (32 * 1))), unchecked((Int32)(value.hi >> (32 * 0))), false, 0);
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
            lo = Numerics.DoubleArithmetic.DivRemUnchecked(lo, hi, d.lo, d.hi, out HInt64 r_lo, out HInt64 r_hi, out hi);
            // Note: r <= (a[0] | ((UInt64)a[1] << 32), (Int64)a[2]) .
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
            return new XInt128(lo, hi);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public byte[] ToBigIntegerByteArray() {
            var a = new byte[128 / 8 + 1];
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
            var lo = Numerics.DoubleArithmetic.NegateUnsigned(value.lo, value.hi, out HInt64 hi);
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
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 IncreaseSigned(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.IncreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator ++(XInt128 value) {
#else
        public static XInt128 operator checked ++(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.IncreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_IncrementUnchecked(XInt128 value) {
#else
        public static XInt128 operator ++(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.IncreaseUnchecked(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static XInt128 Decrease(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 DecreaseSigned(XInt128 value) {
            var lo = Numerics.DoubleArithmetic.DecreaseSigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator --(XInt128 value) {
#else
        public static XInt128 operator checked --(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.DecreaseUnsigned(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_DecrementUnchecked(XInt128 value) {
#else
        public static XInt128 operator --(XInt128 value) {
#endif
            var lo = Numerics.DoubleArithmetic.DecreaseUnchecked(value.lo, value.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Add(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.AddUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }


        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator +(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator checked +(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.AddUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_AdditionUnchecked(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator +(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.AddUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Subtract(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.SubtractUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator -(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator checked -(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.SubtractUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_SubtractionUnchecked(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator -(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.SubtractUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Multiply(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.MultiplyUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator *(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator checked *(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.MultiplyUnsigned(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_MultiplyUnchecked(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator *(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.MultiplyUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 DivRem(XInt128 first, XInt128 second, out XInt128 remainder) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out HInt64 remainder_lo, out HInt64 remainder_hi, out HInt64 hi);
            remainder = new XInt128(remainder_lo, remainder_hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static (XInt128 Quotient, XInt128 Remainder) DivRem(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.DivRem(first.lo, first.hi, second.lo, second.hi, out UInt64 remainder_lo, out HInt64 remainder_hi, out HInt64 hi);
            return (new XInt128(lo, hi), new XInt128(remainder_lo, remainder_hi));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static XInt128 Divide(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        [System.Runtime.CompilerServices.SpecialNameAttribute()]
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

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt128 IDivisionOperators<XInt128, XInt128, XInt128>.operator checked /(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        static XInt128 IDivisionOperators<XInt128, XInt128, XInt128>.operator /(XInt128 first, XInt128 second) {
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        public static XInt128 operator /(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator checked /(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.Divide(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
#if !NET7_0_OR_GREATER || LEGACY_OPERATOR_CHECKNESS
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "op")]
        public static XInt128 op_DivideUnchecked(XInt128 first, XInt128 second) {
#else
        public static XInt128 operator /(XInt128 first, XInt128 second) {
#endif
            var lo = Numerics.DoubleArithmetic.DivideUnchecked(first.lo, first.hi, second.lo, second.hi, out HInt64 hi);
            return new XInt128(lo, hi);
        }
        #endregion

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static Double FractionalDivideAsDouble(XInt128 first, XInt128 second) {
            // TODO:
            return (Double)first / (Double)second;
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        #region IBinaryNumber
        int IBinaryInteger<XInt128>.GetByteCount() {
            return 8;
        }

        static XInt128 IBinaryInteger<XInt128>.LeadingZeroCount(XInt128 value) {
            return unchecked((XInt128)Numerics.DoubleArithmetic.CountLeadingZeros(value.lo, value.hi));
        }

        static XInt128 IBinaryInteger<XInt128>.PopCount(XInt128 value) {
            return unchecked((XInt128)(Mathematics.BinaryNumerals.PopulationCount(value.lo) + Mathematics.BinaryNumerals.PopulationCount(value.hi)));
        }

        static XInt128 IBinaryInteger<XInt128>.RotateLeft(XInt128 value, int rotateAmount) {
            return new XInt128(Numerics.DoubleArithmetic.RotateLeft(value.lo, value.hi, rotateAmount, out var hi), hi);
        }

        static XInt128 IBinaryInteger<XInt128>.RotateRight(XInt128 value, int rotateAmount) {
            return new XInt128(Numerics.DoubleArithmetic.RotateRight(value.lo, value.hi, rotateAmount, out var hi), hi);
        }

        static XInt128 IBinaryInteger<XInt128>.TrailingZeroCount(XInt128 value) {
            return unchecked((XInt128)Numerics.DoubleArithmetic.CountTrailingZeros(value.lo, value.hi));
        }

        bool IBinaryInteger<XInt128>.TryWriteLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (16 <= destination.Length) {
                BinaryPrimitives.TryWriteUInt64LittleEndian(destination, lo);
                BinaryPrimitives.TryWriteUInt64LittleEndian(destination[8..], hi);
                bytesWritten = 16;
                return true;
            }
            bytesWritten = default;
            return false;
        }

        bool IBinaryInteger<XInt128>.TryWriteBigEndian(System.Span<byte> destination, out int bytesWritten) {
            if (16 <= destination.Length) {
                BinaryPrimitives.TryWriteUInt64BigEndian(destination, hi);
                BinaryPrimitives.TryWriteUInt64BigEndian(destination[8..], lo);
                bytesWritten = 16;
                return true;
            }
            bytesWritten = default;
            return false;
        }

        static bool IBinaryInteger<XInt128>.TryReadLittleEndian(ReadOnlySpan<byte> source, bool isUnsigned, out XInt128 value) {
            if (16 <= source.Length) {
                var v = new BigInteger(source, isUnsigned, false);
                if (XInt128.MinValue <= v && v <= XInt128.MaxValue) {
                    value = unchecked((XInt128)v);
                    return true;
                }

            }
            value = default;
            return false;
        }

        static bool IBinaryInteger<XInt128>.TryReadBigEndian(ReadOnlySpan<byte> source, bool isUnsigned, out XInt128 value) {
            if (16 <= source.Length) {
                var v = new BigInteger(source, isUnsigned, false);
                value = unchecked((XInt128)v);
                return true;
            }
            value = default;
            return false;
        }

        static bool IBinaryNumber<XInt128>.IsPow2(XInt128 value) {
            return Numerics.DoubleArithmetic.IsPowerOfTwo(value.lo, value.hi);
        }

        static XInt128 IBinaryNumber<XInt128>.Log2(XInt128 value) {
            return unchecked((XInt128)(Numerics.DoubleArithmetic.Log2Floor(value.lo, value.hi)));
        }

        static XInt128 INumber<XInt128>.Clamp(XInt128 value, XInt128 min, XInt128 max) {
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

        static XInt128 INumber<XInt128>.CopySign(XInt128 value, XInt128 sign) {
            return value;
        }

        public static XInt128 Max(XInt128 x, XInt128 y) {
            return (x > y) ? x : y;
        }

        public static XInt128 Min(XInt128 x, XInt128 y) {
            return (x <= y) ? x : y;
        }

        static XInt128 INumber<XInt128>.MaxNumber(XInt128 x, XInt128 y) {
            return Max(x, y);
        }

        static XInt128 INumber<XInt128>.MinNumber(XInt128 x, XInt128 y) {
            return Min(x, y);
        }

        static XInt128 INumberBase<XInt128>.MaxMagnitude(XInt128 x, XInt128 y) {
            return Max(x, y);
        }

        static XInt128 INumberBase<XInt128>.MaxMagnitudeNumber(XInt128 x, XInt128 y) {
            return Max(x, y);
        }

        static XInt128 INumberBase<XInt128>.MinMagnitude(XInt128 x, XInt128 y) {
            return Min(x, y);
        }

        static XInt128 INumberBase<XInt128>.MinMagnitudeNumber(XInt128 x, XInt128 y) {
            return Min(x, y);
        }

        int IBinaryInteger<XInt128>.GetShortestBitLength() {
            return unchecked(128 - Numerics.DoubleArithmetic.CountLeadingZeros(lo, hi));
        }

        static XInt128 IBinaryNumber<XInt128>.AllBitsSet {

            get => new(~(UInt64)0, ~(HInt64)0);
        }

        static XInt128 INumberBase<XInt128>.Abs(XInt128 value) {
            return value;
        }

        static bool INumberBase<XInt128>.IsCanonical(XInt128 value) {
            return true;
        }

        static bool INumberBase<XInt128>.IsComplexNumber(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsEvenInteger(XInt128 value) {
            return 0 == (1 & unchecked((int)value.lo));
        }

        static bool INumberBase<XInt128>.IsFinite(XInt128 value) {
            return true;
        }

        static bool INumberBase<XInt128>.IsImaginaryNumber(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsInfinity(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsInteger(XInt128 value) {
            return true;
        }

        static bool INumberBase<XInt128>.IsNaN(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsNegative(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsNegativeInfinity(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsNormal(XInt128 value) {
            return true;
        }

        static bool INumberBase<XInt128>.IsOddInteger(XInt128 value) {
            return (1 & unchecked((int)value.lo)).AsBooleanUnsafe();
        }

        static bool INumberBase<XInt128>.IsPositive(XInt128 value) {
            return !value.IsZero;
        }

        static bool INumberBase<XInt128>.IsPositiveInfinity(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsRealNumber(XInt128 value) {
            return true;
        }

        static bool INumberBase<XInt128>.IsSubnormal(XInt128 value) {
            return false;
        }

        static bool INumberBase<XInt128>.IsZero(XInt128 value) {
            return value.IsZero;
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertFromChecked<TOther>(TOther value, out UInt128 result) => TryConvertFromChecked(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromChecked<TOther>(TOther value, out UInt128 result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UltimateOrb.UInt128` will handle all types in corelib (System) and the other unsigned types in UltimateOrb. And
            // `ConvertTo` will handle all types in corelib (System) and the signed types in UltimateOrb.

            // Handle UltimateOrb.Int128 (signed 128-bit)
            /*if (value is UltimateOrb.Int128 s128) {
                if (s128.IsNegative) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)s128);
                return true;
            }*/
            if (false) {
            }
#if NET7_0_OR_GREATER
            // Handle System.Int128 (signed 128-bit)
            else if (value is System.Int128 sysS128) {
                if (System.Int128.IsNegative(sysS128)) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)sysS128);
                return true;
            }
#endif
            // Handle other integer types
            else if (value is ulong ul) {
                result = ul;
                return true;
            } else if (value is char c) {
                result = c;
                return true;
            } else if (value is long il) {
                if (il < 0) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)il);
                return true;
            } else if (value is uint ui) {
                result = ui;
                return true;
            } else if (value is int i) {
                if (i < 0) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)i);
                return true;
            } else if (value is ushort us) {
                result = us;
                return true;
            } else if (value is short ss) {
                if (ss < 0) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)ss);
                return true;
            } else if (value is byte b) {
                result = b;
                return true;
            } else if (value is sbyte sb) {
                if (sb < 0) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)sb);
                return true;
            }

            // Handle platform-dependent size types
            else if (value is nuint uPtr) {
                result = unchecked((UInt128)uPtr);
                return true;
            } else if (value is nint ptr) {
                if (ptr < 0) {
                    result = default;
                    return false;
                }
                result = unchecked((UInt128)ptr);
                return true;
            }
            // Handle floating-point types
            else if (value is double d) {
                return TryConvertFromChecked(d, out result);
            } else if (value is float f) {
                return TryConvertFromChecked(unchecked((double)f), out result);
            } else if (value is Half h) {
                return TryConvertFromChecked(unchecked((double)h), out result);
            }
            // Handle decimal
            else if (value is decimal dec) {
                dec = decimal.Truncate(dec);
                if (dec < 0.0m) {
                    result = default;
                    return false;
                }
                Span<int> bits = stackalloc int[4];
                decimal.GetBits(dec, bits);
                var _lo64 = unchecked((uint)bits[0] + ((ulong)(uint)bits[1] << 32));
                var _hi32 = unchecked((uint)bits[2]);
                result = new UInt128(lo: _lo64, hi: _hi32);
                return true;
            }
            // Handle UltimateOrb.UInt128 (unsigned 128-bit)
            else if (value is UltimateOrb.UInt128 u128) {
                result = u128;
                return true;
            }
#if NET7_0_OR_GREATER
            // Handle System.UInt128 (unsigned 128-bit)
            else if (value is System.UInt128 sysU128) {
                result = sysU128;
                return true;
            }
#endif
            // Unsupported type
            else {
                result = default;
                return false;
            }

            static bool TryConvertFromChecked(double value, out UInt128 result) {
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
                    if (0 > b) {
                        result = default;
                        return false;
                    }
                    checked(checked(ExponentBias - 1 + 128) - e).Ignore();
                    {
                        e = unchecked(e - (FractionBitSize + ExponentBias));
                        var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                        var hi = (UInt64)0;
                        if (e > 0) {
                            lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, e, out hi);
                        } // else if (e < 0) {
                          // var ol = (UInt64)0;
                          // ol = MathEx.ShiftRight(ol, lo, unchecked(-e), out lo);
                          // // Do NOT uncomment. (int)3.5 == 3
                        /*
                        // IEEE Std 754-2008 roundTiesToEven
                        if (unchecked((UInt64)Int64.MinValue) < ol || (unchecked((UInt64)Int64.MinValue) == ol && 0 != (1 & lo))) {
                            lo = unchecked(1 + lo);
                        }
                        */
                        // }
                        if (0 > b) {
                            lo = Numerics.DoubleArithmetic.NegateUnchecked(lo, hi, out hi);
                        }
                        result = new XInt128(lo, hi);
                        return true;
                    }
                }
                result = Zero;
                return true;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertFromSaturating<TOther>(TOther value, out UInt128 result) => TryConvertFromSaturating(value, out result);

        static partial class INumberBaseExtensions<T>
            where T : INumberBase<T> {

            public static bool TryConvertFromSaturating<TOther>(TOther value, out T result)
                where TOther : INumberBase<TOther> {
                return T.TryConvertFromSaturating(value, out result!);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromSaturating<TOther>(TOther value, out UInt128 result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UltimateOrb.UInt128` will handle all types in corelib (System) and the other unsigned types in UltimateOrb. And
            // `ConvertTo` will handle all types in corelib (System) and the signed types in UltimateOrb.

            // Handle UltimateOrb.Int128 (signed 128-bit)
            if (value is UltimateOrb.Int128 s128) {
                result = s128.IsNegative ? UInt128.MinValue : (UInt128)s128;
                return true;
            }
#if NET7_0_OR_GREATER
            // Handle System.Int128 (signed 128-bit)
            else if (value is System.Int128 sysS128) {
                result = sysS128 < 0 ? UInt128.MinValue : (UInt128)sysS128;
                return true;
            }
#endif
            // Handle unsigned integer types
            else if (value is long il) {
                result = long.IsNegative(il) ? UInt128.MinValue : (UInt128)unchecked((ulong)il);
                return true;
            } else if (value is ulong ul) {
                result = ul;
                return true;
            } else if (value is int i) {
                result = int.IsNegative(i) ? UInt128.MinValue : (UInt128)unchecked((uint)i);
                return true;
            } else if (value is uint ui) {
                result = ui;
                return true;
            } else if (value is short ss) {
                result = short.IsNegative(ss) ? UInt128.MinValue : (UInt128)unchecked((ushort)ss);
                return true;
            } else if (value is ushort us) {
                result = us;
                return true;
            } else if (value is char c) {
                result = c;
                return true;
            } else if (value is byte b) {
                result = b;
                return true;
            } else if (value is sbyte sb) {
                result = sbyte.IsNegative(sb) ? UInt128.MinValue : (UInt128)unchecked((byte)sb);
                return true;
            }
            // Handle platform-dependent unsigned size (nuint)
            else if (value is nuint uPtr) {
                result = (UInt128)uPtr.ToUInt64();
                return true;
            } else if (value is nint ptr) {
                result = nint.IsNegative(ptr) ? UInt128.MinValue : (UInt128)unchecked((UInt64)ptr.ToInt64());
                return true;
            }
            // Handle floating-point types with saturation logic
            else if (value is double d) {
                return TryConvertFromSaturating(d, out result);
            } else if (value is float f) {
                return TryConvertFromSaturating(unchecked((double)f), out result);
            } else if (value is Half h) {
                return TryConvertFromSaturating(unchecked((double)h), out result);
            }
            // Handle decimal (clamp negative values to 0)
            else if (value is decimal dec) {
                result = decimal.IsNegative(dec) ? UInt128.MinValue : unchecked((UInt128)dec);
                return true;
            }
            // Handle UltimateOrb.UInt128 (unsigned 128-bit)
            else if (value is UltimateOrb.UInt128 u128) {
                result = u128;
                return true;
            }
#if NET7_0_OR_GREATER
            // Handle System.UInt128 (unsigned 128-bit)
            else if (value is System.UInt128 sysU128) {
                result = sysU128;
                return true;
            }
#endif
            // Unsupported type
            else {
                result = default;
                return false;
            }

            static bool TryConvertFromSaturating(double d, out XInt128 result) {
                var x = BitConverter.DoubleToInt64Bits(d).ToUnsignedUnchecked();
                if (x >= 0X3ff0000000000000) {
                    if (0 <= x.ToSignedUnchecked()) {
                        if (x <= 0X7ff0000000000000) {
                            result = UInt128.MaxValue;
                        } else {
                            // NaN
                            result = default;
                            return false;
                        }
                    } else {
                        if (x <= 0Xfff0000000000000) {
                            result = UInt128.MinValue;
                        } else {
                            // NaN
                            result = default;
                            return false;
                        }
                    }
                } else {
                    result = unchecked((UInt128)d);
                }
                return true;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertFromTruncating<TOther>(TOther value, out UInt128 result) => TryConvertFromTruncating(value, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryConvertFromTruncating<TOther>(TOther value, out UInt128 result)
            where TOther : INumberBase<TOther> {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UltimateOrb.UInt128` will handle all types in corelib (System) and the other unsigned types in UltimateOrb. And
            // `ConvertTo` will handle all types in corelib (System) and the signed types in UltimateOrb.

            // Handle UltimateOrb.Int128 (signed 128-bit)
            if (value is UltimateOrb.Int128 s128) {
                result = unchecked((UInt128)s128);
                return true;
            }
#if NET7_0_OR_GREATER
            // Handle System.Int128 (signed 128-bit)
            else if (value is System.Int128 sysS128) {
                result = unchecked((UInt128)sysS128);
                return true;
            }
#endif
            // Handle integer types with truncating conversion
            else if (value is long il) {
                result = unchecked((UInt128)(Int128)il);
                return true;
            } else if (value is ulong ul) {
                result = ul;
                return true;
            } else if (value is int i) {
                result = unchecked((UInt128)(Int128)i);
                return true;
            } else if (value is uint ui) {
                result = ui;
                return true;
            } else if (value is short ss) {
                result = unchecked((UInt128)(Int128)ss);
                return true;
            } else if (value is ushort us) {
                result = us;
                return true;
            } else if (value is char c) {
                result = c;
                return true;
            } else if (value is byte b) {
                result = b;
                return true;
            } else if (value is sbyte sb) {
                result = unchecked((UInt128)(Int128)sb);
                return true;
            }
            // Handle platform-dependent size
            else if (value is nuint uPtr) {
                result = (UInt128)uPtr.ToUInt64();
                return true;
            } else if (value is nint ptr) {
                result = unchecked((UInt128)(Int128)ptr.ToInt64());
                return true;
            }
            // Handle floating-point types with truncating logic
            else if (value is double d) {
                return TryConvertFromTruncating(d, out result);
            } else if (value is float f) {
                return TryConvertFromTruncating((double)f, out result);
            } else if (value is Half h) {
                return TryConvertFromTruncating((double)h, out result);
            }
            // Handle decimal with truncating conversion
            else if (value is decimal dec) {
                result = unchecked((UInt128)(Int128)dec);
                return true;
            }
            // Handle UltimateOrb.UInt128 (unsigned 128-bit)
            else if (value is UltimateOrb.UInt128 u128) {
                result = u128;
                return true;
            }
#if NET7_0_OR_GREATER
            // Handle System.UInt128 (unsigned 128-bit)
            else if (value is System.UInt128 sysU128) {
                result = sysU128;
                return true;
            }
#endif
            // Unsupported type
            else {
                result = default;
                return false;
            }

            static bool TryConvertFromTruncating(double d, out UInt128 result) {
                if (!double.IsFinite(d)) {
                    result = default;
                    return false;
                }

                result = unchecked((UInt128)d);
                return true;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertToChecked<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UInt128` will handle the other unsigned types and
            // `ConvertTo` will handle the signed types

            if (typeof(TOther) == typeof(double)) {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualResult = checked((short)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult = checked((int)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult = checked((long)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult = checked((Int128)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualResult = checked((nint)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult = checked((sbyte)value);
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToSaturating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertToSaturating<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UInt128` will handle the other unsigned types and
            // `ConvertTo` will handle the signed types

            if (typeof(TOther) == typeof(double)) {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_7FFF)) ? short.MaxValue : (short)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? int.MaxValue : (int)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? long.MaxValue : (long)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult = (value >= new UInt128(0x7FFF_FFFF_FFFF_FFFF, 0xFFFF_FFFF_FFFF_FFFF)) ? Int128.MaxValue : (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
#if TARGET_32BIT
                nint actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_7FFF_FFFF)) ? nint.MaxValue : (nint)value;
                result = (TOther)(object)actualResult;
                return true;
#else
                nint actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x7FFF_FFFF_FFFF_FFFF)) ? nint.MaxValue : (nint)value;
                result = (TOther)(object)actualResult;
                return true;
#endif
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult = (value >= new UInt128(0x0000_0000_0000_0000, 0x0000_0000_0000_007F)) ? sbyte.MaxValue : (sbyte)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToTruncating{TOther}(TSelf, out TOther)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static bool INumberBase<UInt128>.TryConvertToTruncating<TOther>(UInt128 value, [MaybeNullWhen(false)] out TOther result) {
            // In order to reduce overall code duplication and improve the inlinabilty of these
            // methods for the corelib types we have `ConvertFrom` handle the same sign and
            // `ConvertTo` handle the opposite sign. However, since there is an uneven split
            // between signed and unsigned types, the one that handles unsigned will also
            // handle `Decimal`.
            //
            // That is, `ConvertFrom` for `UInt128` will handle the other unsigned types and
            // `ConvertTo` will handle the signed types

            if (typeof(TOther) == typeof(double)) {
                double actualResult = (double)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualResult = (Half)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(short)) {
                short actualResult = (short)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualResult = (int)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualResult = (long)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(Int128)) {
                Int128 actualResult = (Int128)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualResult = (nint)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualResult = (sbyte)value;
                result = (TOther)(object)actualResult;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualResult = (float)value;
                result = (TOther)(object)actualResult;
                return true;
            } else {
                result = default;
                return false;
            }
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
            if (this <= Char.MaxValue) {
                return unchecked((Char)this.lo).ToChar(provider);
            }
            return ((long)Char.MaxValue + 1).ToChar(provider); // Let the underlying standard libraries raise the exception.
        }

        sbyte IConvertible.ToSByte(IFormatProvider? provider) {
            if (this <= (byte)sbyte.MaxValue) {
                return unchecked((sbyte)this.lo).ToSByte(provider);
            }
            return ((long)sbyte.MaxValue + 1).ToSByte(provider); // Let the underlying standard libraries raise the exception.
        }

        byte IConvertible.ToByte(IFormatProvider? provider) {
            if (this <= byte.MaxValue) {
                return unchecked((byte)this.lo).ToByte(provider);
            }
            return ((long)byte.MaxValue + 1).ToByte(provider); // Let the underlying standard libraries raise the exception.
        }

        Int16 IConvertible.ToInt16(IFormatProvider? provider) {
            if (this <= (UInt16)Int16.MaxValue) {
                return unchecked((Int16)this.lo).ToInt16(provider);
            }
            return ((long)Int16.MaxValue + 1).ToInt16(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) {
            if (this <= UInt16.MaxValue) {
                return unchecked((UInt16)this.lo).ToUInt16(provider);
            }
            return ((long)UInt16.MaxValue + 1).ToUInt16(provider); // Let the underlying standard libraries raise the exception.
        }

        Int32 IConvertible.ToInt32(IFormatProvider? provider) {
            if (this <= (UInt32)Int32.MaxValue) {
                return unchecked((Int32)this.lo).ToInt32(provider);
            }
            return ((long)Int32.MaxValue + 1).ToInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) {
            if (this <= UInt32.MaxValue) {
                return unchecked((UInt32)this.lo).ToUInt32(provider);
            }
            return ((long)UInt32.MaxValue + 1).ToUInt32(provider); // Let the underlying standard libraries raise the exception.
        }

        Int64 IConvertible.ToInt64(IFormatProvider? provider) {
            if (this <= (UInt64)Int64.MaxValue) {
                return unchecked((Int64)this.lo).ToInt64(provider);
            }
            return (UInt64.MaxValue).ToInt64(provider); // Let the underlying standard libraries raise the exception.
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) {
            if (this <= UInt64.MaxValue) {
                return unchecked((UInt64)this.lo).ToUInt64(provider);
            }
            return (Int64.MinValue).ToUInt64(provider); // Let the underlying standard libraries raise the exception.
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
                    return ex.Message.Replace(nameof(Int64), nameof(UInt128));
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
        public static bool TryParseCStyleNormalizedU128(string s, out XInt128 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                UInt64 lo;
                UInt64 hi;
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
                        result = new XInt128(lo, hi);
                        return true;
                    }
                }
            }
        L_f:;
            Try_HandleOutParameterIfFalse(out result);
            return false;
        }

        public static bool TryParseCStyleNormalizedX128(string s, out XInt128 result) {
            if (null != s && s.Length > 0) {
                var i = 0;
                var c = s[i++];
                UInt64 lo;
                UInt64 hi;
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
                                var ignored = checked(0 - unchecked(hi >> (64 - 4)));
                            }
                            lo = Numerics.DoubleArithmetic.ShiftLeft(lo, hi, 4, out hi);
                            lo = Numerics.DoubleArithmetic.AddUnsigned(lo, hi, unchecked((uint)d), 0, out hi);
                        } catch (ArithmeticException) {
                            goto L_f;
                        }
                    }
                    if (s.Length == i) {
                        // TODO: ...
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
        //private static char[] buffer_ToStringCStyleU128 {

        //    [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //    get {
        //        return Int128.buffer_ToStringCStyleI128;
        //    }
        //}

        public string ToStringCStyleU128() {
            return new string(ToStringCStyleU128(stackalloc char[40]));
        }

        public Span<char> ToStringCStyleU128(Span<char> buffer) {
            var lo = this.lo;
            var hi = unchecked((UInt64)this.hi);
            var i = buffer.Length;
            {
                UInt64 r_lo;
                UInt64 r_hi;
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
                return this.ToStringCStyleU128();
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
                return this.ToStringCStyleU128();
            }
        }

        public static implicit operator BigInteger(XInt128 value) {
            // TODO: Perf
            Span<byte> buffer = stackalloc byte[16];
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, value.lo);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(8), value.hi);
            return new BigInteger(buffer, isUnsigned: true, isBigEndian: false);
        }

#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static explicit operator XInt128(BigInteger value) {
            var lo = unchecked((UInt64)(value & UInt64.MaxValue));
            var hi = unchecked((UInt64)((value >> 64) & UInt64.MaxValue));
            return new XInt128(lo, hi);
        }

        public static explicit operator checked XInt128(BigInteger value) {
            var lo = unchecked((UInt64)(value & UInt64.MaxValue));
            var hi = checked((UInt64)(value >> 64));
            return new XInt128(lo, hi);
        }
#else
        public static explicit operator XInt128(BigInteger value) {
            var lo = unchecked((UInt64)(value & UInt64.MaxValue));
            var hi = unchecked((UInt64)((value >> 64) & UInt64.MaxValue));
            if (value != hi) {
                throw new OverflowException(); 
            }
            return new XInt128(lo, hi);
        }
#endif
#endif
#if NET7_0_OR_GREATER && !LEGACY_OPERATOR_CHECKNESS
        public static XInt128 Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            return System.UInt128.Parse(s, style, provider);
        }

        public static XInt128 Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Parse(s.AsSpan(), style, provider);
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out XInt128 result) {
            Unsafe.SkipInit(out result);
            return System.UInt128.TryParse(s, style, provider, out Unsafe.As<UltimateOrb.UInt128, System.UInt128>(ref result));
        }

        public static bool TryParse(string? s, NumberStyles style, IFormatProvider? provider, out XInt128 result) {
            return TryParse(s.AsSpan(), style, provider, out result);
        }

        static XInt128 ISpanParsable<XInt128>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Integer, provider);
        }

        static bool ISpanParsable<XInt128>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out XInt128 result) {
            return TryParse(s, NumberStyles.Integer, provider, out result);
        }

        static XInt128 IParsable<XInt128>.Parse(string s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Integer, provider);
        }

        static bool IParsable<XInt128>.TryParse(string? s, IFormatProvider? provider, out XInt128 result) {
            return TryParse(s.AsSpan(), NumberStyles.Integer, provider, out result);
        }

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            return ((System.UInt128)this).TryFormat(destination, out charsWritten, format, provider);
        }
#endif
    }
}

namespace UltimateOrb {

#if NET8_0_OR_GREATER
    [Discardable]
    static class SystemInt128UnsafeAccessors {

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
        internal extern static ref readonly UInt64 GetLowerUInt64Ref(in System.Int128 value);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
        internal extern static ref readonly UInt64 GetUpperUInt64Ref(in System.Int128 value);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_lower")]
        internal extern static ref readonly UInt64 GetLowerUInt64Ref(in System.UInt128 value);

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_upper")]
        internal extern static ref readonly UInt64 GetUpperUInt64Ref(in System.UInt128 value);
    }
#endif

#if NET7_0_OR_GREATER
    [Discardable]
    static class SystemInt128Extensions {

        extension(System.Int128 @this) {

            internal UInt64 LoUInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this);
#else
                get => unchecked((UInt64)@this);
#endif
            }

            internal UInt64 HiUInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this);
#else
                get => unchecked((UInt64)(@this >>> 64));
#endif
            }

            internal Int64 LoInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => unchecked((Int64)SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this));
#else
                get => unchecked((Int64)@this);
#endif
            }

            internal Int64 HiInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => unchecked((Int64)SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this));
#else
                get => unchecked((Int64)(@this >>> 64));
#endif
            }
        }

        extension(System.UInt128 @this) {

            internal UInt64 LoUInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this);
#else
                get => unchecked((UInt64)@this);
#endif
            }

            internal UInt64 HiUInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this);
#else
                get => unchecked((UInt64)(@this >>> 64));
#endif
            }

            internal Int64 LoInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => unchecked((Int64)SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this));
#else
                get => unchecked((Int64)@this);
#endif
            }

            internal Int64 HiInt64Bits {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => unchecked((Int64)SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this));
#else
                get => unchecked((Int64)(@this >>> 64));
#endif
            }
        }

#if NET8_0_OR_GREATER
#else
        readonly struct SystemInt128Layout {
            internal readonly UInt64 d0;
            internal readonly UInt64 d1;
        }
#endif

        extension(in System.Int128 @this) {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal ref readonly UInt64 lo {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => ref SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this);
#else
                get {
                    ref readonly var t = ref Unsafe.As<System.Int128, SystemInt128Layout>(ref Unsafe.AsRef(in @this));
                    return ref (BitConverter.IsLittleEndian ? ref t.d0 : ref t.d1);
                }
#endif
            }

            internal ref readonly Int64 hi {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => ref Unsafe.As<UInt64, Int64>(ref Unsafe.AsRef(in SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this)));
#else
                get {
                    ref readonly var t = ref Unsafe.As<System.Int128, SystemInt128Layout>(ref Unsafe.AsRef(in @this));
                    return ref Unsafe.As<UInt64, Int64>(ref Unsafe.AsRef(in (BitConverter.IsLittleEndian ? ref t.d0 : ref t.d1)));
                }
#endif
            }
        }

        extension(in System.UInt128 @this) {

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            internal ref readonly UInt64 lo {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => ref SystemInt128UnsafeAccessors.GetLowerUInt64Ref(in @this);
#else
                get {
                    ref readonly var t = ref Unsafe.As<System.UInt128, SystemInt128Layout>(ref Unsafe.AsRef(in @this));
                    return ref (BitConverter.IsLittleEndian ? ref t.d0 : ref t.d1);
                }
#endif
            }

            internal ref readonly UInt64 hi {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NET8_0_OR_GREATER
                get => ref SystemInt128UnsafeAccessors.GetUpperUInt64Ref(in @this);
#else
                get {
                    ref readonly var t = ref Unsafe.As<System.UInt128, SystemInt128Layout>(ref Unsafe.AsRef(in @this));
                    return ref (BitConverter.IsLittleEndian ? ref t.d1 : ref t.d0);
                }
#endif
            }
        }
    }
#endif
    }

    namespace UltimateOrb {
    using Internal;
    using UltimateOrb.Runtime.CompilerServices;
    using static UltimateOrb.Utilities.ThrowHelper;
    using HInt64 = UInt64;
    using MathEx = UltimateOrb.Numerics.DoubleArithmetic;
    using OInt128 = Int128;
    using XInt128 = UInt128;

    public partial struct UInt128 {

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
                            result_lo = Numerics.DoubleArithmetic.MultiplyUnsigned(result_lo, result_hi, lo, hi, out result_hi);
                        }
                        if (0 != (exponent >>= 1)) {
                            lo = Numerics.DoubleArithmetic.MultiplyUnsigned(lo, hi, lo, hi, out hi);
                            continue;
                        }
                        break;
                    }
                    return new XInt128(result_lo, result_hi);
                }
                if (0 == exponent || (1 == lo && 0 == hi)) {
                    return XInt128.One;
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
                var result_lo_lo = Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out HInt64 result_lo_hi, out HInt64 result_hi_lo, out HInt64 result_hi_hi);
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

        public static partial class EuclideanAlgorithm {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 GreatestCommonDivisor(XInt128 first, XInt128 second) {
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
                    return new XInt128(first_lo, first_hi);
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
        }

        public static partial class ZZOverNZZModule {

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Sum(XInt128 n, XInt128 first, XInt128 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                return unchecked(n <= (second = (first + second)) || first > second ? second - n : second);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Double(XInt128 n, XInt128 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                return Sum(n, value, value);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Difference(XInt128 n, XInt128 first, XInt128 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                return unchecked(second > first ? first - second + n : first - second);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Opposite(XInt128 n, XInt128 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                return Difference(n, 0u, value);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Product(XInt128 n, XInt128 first, XInt128 second) {
                System.Diagnostics.Contracts.Contract.Requires(n > first);
                System.Diagnostics.Contracts.Contract.Requires(n > second);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                var p_lo_lo = UltimateOrb.Numerics.DoubleArithmetic.BigMul(first.lo, first.hi, second.lo, second.hi, out var p_lo_hi, out var p_hi_lo, out var p_hi_hi);
                var lo = UltimateOrb.Numerics.DoubleArithmetic.BigRemNoThrowWhenOverflow(p_lo_lo, p_lo_hi, p_hi_lo, p_hi_hi, n.lo, n.hi, out var hi);
                return new XInt128(lo, hi);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Square(XInt128 n, XInt128 value) {
                System.Diagnostics.Contracts.Contract.Requires(n > value);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                var p_lo_lo = UltimateOrb.Numerics.DoubleArithmetic.BigSquare(value.lo, value.hi, out var p_lo_hi, out var p_hi_lo, out var p_hi_hi);
                var lo = UltimateOrb.Numerics.DoubleArithmetic.BigRemNoThrowWhenOverflow(p_lo_lo, p_lo_hi, p_hi_lo, p_hi_hi, n.lo, n.hi, out var hi);
                return new XInt128(lo, hi);
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 Power(XInt128 n, XInt128 @base, uint exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                XInt128 j = 0u;
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
            public static XInt128 Power(XInt128 n, XInt128 @base, ulong exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                XInt128 j = 0u;
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
            public static XInt128 Power(XInt128 n, XInt128 @base, XInt128 exponent) {
                System.Diagnostics.Contracts.Contract.Requires(n > @base);
                System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.OldValue(n) > System.Diagnostics.Contracts.Contract.Result<XInt128>());
                XInt128 j = 0u;
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
            public static XInt128 ToUInt128(double value) {
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
                    {
                        e = unchecked(e - (FractionBitSize + ExponentBias));
                        var lo = unchecked((UInt64)(((Int64)1 << FractionBitSize) | (FractionMask & b)));
                        var hi = (UInt64)0;
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
                }
                return Zero;
            }

            [System.CLSCompliantAttribute(false)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.MayFail)]
            [System.Runtime.TargetedPatchingOptOutAttribute("")]
            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            [System.Diagnostics.Contracts.PureAttribute()]
            public static XInt128 ToUInt128(Single value) {
                return ToUInt128(unchecked((double)value));
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
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Half ToHalf(UInt64 value) {
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
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)value_));
                int c;
                if (Lzcnt.X64.IsSupported || ArmBase.Arm64.IsSupported || X86Base.X64.IsSupported) {
                    c = BitOperations.LeadingZeroCount(unchecked((UInt64)value_));
                } else {
                    c = 0;
                    for (var tmp = value_; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                        unchecked {
                            ++c;
                        }
                    }
                }
                var s = unchecked((UInt64)(checked(64 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = value_ & unchecked(((UInt64)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((UInt64)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                return unchecked((Half)BitConverter.Int64BitsToDouble(unchecked((Int64)s)));
            }
            return default;
        }
#endif

        [global::System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [global::System.Runtime.TargetedPatchingOptOutAttribute("")]
        [global::System.Runtime.CompilerServices.MethodImplAttribute(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [global::System.Diagnostics.Contracts.PureAttribute()]
        public static Single ToSingle(UInt64 value) {
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
                // var c = Mathematics.BinaryNumerals.CountLeadingZeros(unchecked((UInt64)value_));
                var c = 0;
                for (var tmp = value_; 0 <= unchecked((Int64)tmp); tmp <<= 1) {
                    unchecked {
                        ++c;
                    }
                }
                var s = unchecked((UInt64)(checked(64 - 1 + ExponentBias) - c)) << FractionBitSize;
                value_ <<= unchecked(1 + c);
                var lo = value_ & unchecked(((UInt64)1 << checked(BitSize - FractionBitSizeTo)) - 1);
                value_ >>= checked(BitSize - FractionBitSizeTo);
                // IEEE Std 754-2008 roundTiesToEven
                if ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) < lo || ((UInt64)1 << checked(BitSize - FractionBitSizeTo - 1) == unchecked((UInt64)lo) && 0 != (1 & value_))) {
                    unchecked {
                        ++value_;
                    }
                }
                s = unchecked(s + (value_ << checked(FractionBitSize - FractionBitSizeTo)));
                return unchecked((Single)BitConverter.Int64BitsToDouble(unchecked((Int64)s)));
            }
            return default;
        }
    }
}