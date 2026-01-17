using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Mathematics.Exact;
using UltimateOrb.Numerics;
using UltimateOrb.Utilities;
using Debug = System.Diagnostics.Debug;

namespace UltimateOrb {

    public interface ITag { }

    public readonly struct Tag<T> : ITag
#if !NET9_0_OR_GREATER
#else
        where T : allows ref struct
#endif
        {
    }

    public readonly struct Tag<T, BaseTag> : ITag
#if !NET9_0_OR_GREATER
#else
        where T : allows ref struct
#endif
        where BaseTag : struct, ITag {
    }

    public interface IInterfaceDerivedSelfBase<TSelf, TBase>
        : IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase>
        where TSelf : IInterfaceDerivedSelfBase<TSelf, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase : allows ref struct
#endif
        {

        static TSelf? IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TSelf? FromBase(TBase? value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase? ToBase(TSelf? value);
    }

    public interface IInterfaceDerivedSelfBaseFriend<TSelf, TBase>
        : IInterfaceDerivedSelfBase<TSelf, TBase>
        where TSelf : IInterfaceDerivedSelfBase<TSelf, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TSelf? FromBase(TBase? value) {
            return TSelf.FromBase(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase? ToBase(TSelf? value) {
            return TSelf.ToBase(value);
        }
    }

    public interface IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase>
        : IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase>
        where TSelf : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase : allows ref struct
#endif
        {

        static TSelf? IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TSelf? FromBase(TBase? value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase? ToBase(TSelf? value);
    }


    public interface IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag, TBase>
        : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase>
        where TSelf : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TSelf? FromBase(TBase? value) {
            return TSelf.FromBase(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase? ToBase(TSelf? value) {
            return TSelf.ToBase(value);
        }
    }

    public interface IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase>
        where TSelf : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TDerived : allows ref struct
        where TBase : allows ref struct
#endif
        {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual TDerived? FromBase(TBase? value) => InterfaceDerivedDefault<TDerived, TBase>.FromBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual TBase? ToBase(TDerived? value) => InterfaceDerivedDefault<TDerived, TBase>.ToBase(value);
    }

    public interface IInterfaceDerivedBaseFriend<TSelf, Tag, TDerived, TBase>
        : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase>
        where TSelf : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TDerived : allows ref struct
        where TBase : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TDerived? FromBase(TBase? value) {
            return TSelf.FromBase(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase? ToBase(TDerived? value) {
            return TSelf.ToBase(value);
        }
    }

    public static partial class InterfaceDerivedDefault<TDerived, TBase>
#if !NET9_0_OR_GREATER
#else
        where TDerived : allows ref struct
        where TBase : allows ref struct
#endif
        {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static TDerived? FromBase(TBase? value) {
            return Unsafe.As<TBase?, TDerived?>(ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static TBase? ToBase(TDerived? value) {
            return Unsafe.As<TDerived?, TBase?>(ref value);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly
#if !NET9_0_OR_GREATER
#else
       ref
#endif
       struct SequentialLayoutValueTuple<T1>
#if !NET9_0_OR_GREATER
#else
       where T1 : allows ref struct
#endif
       {
        public readonly T1 Item1;

        public SequentialLayoutValueTuple(T1 item1) {
            Item1 = item1;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal readonly
#if !NET9_0_OR_GREATER
# else
        ref
#endif
        struct SequentialLayoutValueTuple<T1, T2>
#if !NET9_0_OR_GREATER
#else
        where T1 : allows ref struct
        where T2 : allows ref struct
#endif 
        {
        public readonly T1 Item1;
        public readonly T2 Item2;

        public SequentialLayoutValueTuple(T1 item1, T2 item2) {
            Item1 = item1;
            Item2 = item2;
        }
    }
    public interface IInterfaceDerivedSelfBase<TSelf, TBase1, TBase2>
        : IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase1, TBase2>
        where TSelf : IInterfaceDerivedSelfBase<TSelf, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        static TSelf? IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase1, TBase2>.FromBase(TBase1? value1, TBase2? value2) => TSelf.FromBase(value1, value2);

        static TBase1? IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase1, TBase2>.ToBase1(TSelf? value) => TSelf.ToBase1(value);

        static TBase2? IInterfaceDerivedTaggedSelfBase<TSelf, Tag<TSelf>, TBase1, TBase2>.ToBase2(TSelf? value) => TSelf.ToBase2(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        protected static abstract new TSelf? FromBase(TBase1? value1, TBase2? value2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase1? ToBase1(TSelf? value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase2? ToBase2(TSelf? value);
    }

    public interface IInterfaceDerivedSelfBaseFriend<TSelf, TBase1, TBase2>
        : IInterfaceDerivedSelfBase<TSelf, TBase1, TBase2>
        where TSelf : IInterfaceDerivedSelfBase<TSelf, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        public static new TSelf? FromBase(TBase1? value1, TBase2? value2) {
            return TSelf.FromBase(value1, value2);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase1"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase1? ToBase1(TSelf? value) {
            return TSelf.ToBase1(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase2? ToBase2(TSelf? value) {
            return TSelf.ToBase2(value);
        }
    }

    public interface IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase1, TBase2>
        : IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase1, TBase2>
        where TSelf : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        static TSelf? IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase1, TBase2>.FromBase(TBase1? value1, TBase2? value2) => TSelf.FromBase(value1, value2);

        static TBase1? IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase1, TBase2>.ToBase1(TSelf? value) => TSelf.ToBase1(value);

        static TBase2? IInterfaceDerivedBase<TSelf, Tag, TSelf, TBase1, TBase2>.ToBase2(TSelf? value) => TSelf.ToBase2(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        protected static abstract new TSelf? FromBase(TBase1? value1, TBase2? value2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase1? ToBase1(TSelf? value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static abstract new TBase2? ToBase2(TSelf? value);
    }

    public interface IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag, TBase1, TBase2>
        : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase1, TBase2>
        where TSelf : IInterfaceDerivedTaggedSelfBase<TSelf, Tag, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        public static new TSelf? FromBase(TBase1? value1, TBase2? value2) {
            return TSelf.FromBase(value1, value2);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase1"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase1? ToBase1(TSelf? value) {
            return TSelf.ToBase1(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase2? ToBase2(TSelf? value) {
            return TSelf.ToBase2(value);
        }
    }

    public interface IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase1, TBase2>
        where TSelf : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TDerived : allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        protected static virtual TDerived? FromBase(TBase1? value1, TBase2? value2) => InterfaceDerivedDefault<TDerived, TBase1, TBase2>.FromBase(value1, value2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual TBase1? ToBase1(TDerived? value) => InterfaceDerivedDefault<TDerived, TBase1, TBase2>.ToBase1(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual TBase2? ToBase2(TDerived? value) => InterfaceDerivedDefault<TDerived, TBase1, TBase2>.ToBase2(value);
    }

    public interface IInterfaceDerivedBaseFriend<TSelf, Tag, TDerived, TBase1, TBase2>
        : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase1, TBase2>
        where TSelf : IInterfaceDerivedBase<TSelf, Tag, TDerived, TBase1, TBase2>?
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
#endif
        where Tag : struct, ITag
#if !NET9_0_OR_GREATER
#else
        , allows ref struct
        where TDerived : allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        /// <summary>
        /// Converts the specified value to <typeparamref name="TSelf"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        public static new TDerived? FromBase(TBase1? value1, TBase2? value2) {
            return TSelf.FromBase(value1, value2);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase1"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase1? ToBase1(TDerived? value) {
            return TSelf.ToBase1(value);
        }

        /// <summary>
        /// Converts the specified value to <typeparamref name="TBase2"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static new TBase2? ToBase2(TDerived? value) {
            return TSelf.ToBase2(value);
        }
    }

    public static partial class InterfaceDerivedDefault<TDerived, TBase1, TBase2>
#if !NET9_0_OR_GREATER
#else
        where TDerived : allows ref struct
        where TBase1 : allows ref struct
        where TBase2 : allows ref struct
#endif
        {

        /*
        [StructLayout(LayoutKind.Explicit)]
        readonly
#if !NET9_0_OR_GREATER
# else
            ref
#endif
            struct U {

            [FieldOffset(0)]
            public readonly SequentialLayoutValueTuple<TDerived> Item1;

            [FieldOffset(0)]
            public readonly SequentialLayoutValueTuple<TBase1, TBase2> Item2;
        }

        [StructLayout(LayoutKind.Explicit)]
        readonly
#if !NET9_0_OR_GREATER
# else
            ref
#endif
            struct U1 {

            [FieldOffset(0)]
            public readonly SequentialLayoutValueTuple<TDerived, byte>  Item1;

            [FieldOffset(0)]
            public readonly SequentialLayoutValueTuple<SequentialLayoutValueTuple<TBase1, TBase2>, byte> Item2;
        }

        static class aaaa {

            internal static readonly bool ggg = Check();

            private static bool Check() {
                U1 u1 = default;
                ref var b = ref Unsafe.As<U1, byte>(ref u1); 
                if (Unsafe.ByteOffset(ref b, ref Unsafe.AsRef(in u1.Item1.Item2)) !=
                    Unsafe.ByteOffset(ref b, ref Unsafe.AsRef(in u1.Item2.Item2))) {
                    throw new InvalidOperationException();
                }
                return true;
            }
        }
        */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value1))]
        [return: NotNullIfNotNull(nameof(value2))]
        public static TDerived? FromBase(TBase1? value1, TBase2? value2) {
            /*
            if (aaaa.ggg) {
                var t = new SequentialLayoutValueTuple<TBase1?, TBase2?>(value1, value2);
                return Unsafe.As<SequentialLayoutValueTuple<TBase1?, TBase2?>, U>(ref t).Item1.Item1;
            }
            throw new InvalidOperationException();
            */
            var t = new SequentialLayoutValueTuple<TBase1?, TBase2?>(value1, value2);
            return Unsafe.BitCast<SequentialLayoutValueTuple<TBase1?, TBase2?>, SequentialLayoutValueTuple<TDerived?>>(t).Item1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static TBase1? ToBase1(TDerived? value) {
            /*
            if (aaaa.ggg) {
                return Unsafe.As<TDerived?, U>(ref value).Item2.Item1;
            }
            throw new InvalidOperationException();
            */
            var t = new SequentialLayoutValueTuple<TDerived?>(value);
            return Unsafe.BitCast<SequentialLayoutValueTuple<TDerived?>, SequentialLayoutValueTuple<TBase1?, TBase2?>>(t).Item1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        public static TBase2? ToBase2(TDerived? value) {
            /*
            if (aaaa.ggg) {
                return Unsafe.As<TDerived?, U>(ref value).Item2.Item2;
            }
            throw new InvalidOperationException();
            */
            var t = new SequentialLayoutValueTuple<TDerived?>(value);
            return Unsafe.BitCast<SequentialLayoutValueTuple<TDerived?>, SequentialLayoutValueTuple<TBase1?, TBase2?>>(t).Item2;
        }
    }


}

namespace UltimateOrb {

    public interface IComparableNongenericDerived<TSelf, TBase>
        : IComparableNongenericDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IComparableNongenericDerived<TSelf, TBase>?
        where TBase : IComparable? {
    }

    public interface IComparableNongenericDerivedTagged<TSelf, Tag, TBase>
        : IComparable, IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        where TSelf : IComparableNongenericDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IComparable? {

        int IComparable.CompareTo(object? obj) {
            var this__ = TSelf.ToBase((TSelf)(object)this);
            if (obj is TSelf other__) {
                return this__.CompareTo((object?)TSelf.ToBase(other__));
            }
            //if (obj is TBase other) {
            //    return CompareTo((object?)TSelf.FromBase(other));
            //}
            return this__.CompareTo(obj);
        }
    }

    [CLSCompliant(false)]
    public interface IConvertibleNongenericDerived<TSelf, TBase>
       : IConvertibleNongenericDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : IConvertibleNongenericDerived<TSelf, TBase>?
       where TBase : IConvertible? {
    }

    [CLSCompliant(false)]
    public interface IConvertibleNongenericDerivedTagged<TSelf, Tag, TBase>
        : IConvertible, IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        where TSelf : IConvertibleNongenericDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IConvertible? {

        TypeCode IConvertible.GetTypeCode() => TSelf.ToBase((TSelf)(object)this).GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToBoolean(provider);

        byte IConvertible.ToByte(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToByte(provider);

        char IConvertible.ToChar(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToChar(provider);

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToDateTime(provider);

        decimal IConvertible.ToDecimal(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToDecimal(provider);

        double IConvertible.ToDouble(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToDouble(provider);

        Int16 IConvertible.ToInt16(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToInt16(provider);

        Int32 IConvertible.ToInt32(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToInt32(provider);

        Int64 IConvertible.ToInt64(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToInt64(provider);

        sbyte IConvertible.ToSByte(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToSByte(provider);

        Single IConvertible.ToSingle(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToSingle(provider);

        string IConvertible.ToString(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToString(provider);

        object IConvertible.ToType(Type conversionType, IFormatProvider? provider) {
            var this__ = TSelf.ToBase((TSelf)(object)this);
            if (typeof(TSelf) == conversionType) {
                //return (TSelf)(object)target;
                var t = this__.ToType(typeof(TBase), provider);
                if (t is TBase v) {
                    return TSelf.FromBase(v);
                }
                return t;
            }
            return this__.ToType(conversionType, provider);
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToUInt16(provider);

        UInt32 IConvertible.ToUInt32(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToUInt32(provider);

        UInt64 IConvertible.ToUInt64(IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).ToUInt64(provider);
    }

    public interface IComparableDerived<TSelf, TBase>
       : IComparableDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : IComparableDerived<TSelf, TBase>?
       where TBase : IComparable<TBase>? {
    }

    public interface IComparableDerivedTagged<TSelf, Tag, TBase>
        : IComparable<TSelf>, IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        where TSelf : IComparableDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IComparable<TBase>? {

        int IComparable<TSelf>.CompareTo(TSelf? other) => TSelf.ToBase((TSelf)(object)this).CompareTo(TSelf.ToBase(other));
    }

    public interface IEquatableDerived<TSelf, TBase>
       : IEquatableDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : IEquatableDerived<TSelf, TBase>?
       where TBase : IEquatable<TBase>? {
    }

    public interface IEquatableDerivedTagged<TSelf, Tag, TBase>
        : IEquatable<TSelf>, IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        where TSelf : IEquatableDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IEquatable<TBase>? {

        bool IEquatable<TSelf>.Equals(TSelf? other) => TSelf.ToBase((TSelf)(object)this).Equals(TSelf.ToBase(other));
    }
}

namespace UltimateOrb.Numerics {

    public interface IMinMaxValueDerived<TSelf, TBase>
        : IMinMaxValueDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IMinMaxValueDerived<TSelf, TBase>?
        where TBase : IMinMaxValue<TBase>? {
    }

    public interface IMinMaxValueDerivedTagged<TSelf, Tag, TBase>
        : IMinMaxValue<TSelf>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        where TSelf : IMinMaxValueDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IMinMaxValue<TBase>? {

        static TSelf IMinMaxValue<TSelf>.MaxValue { get => TSelf.FromBase(TBase.MaxValue)!; }

        static TSelf IMinMaxValue<TSelf>.MinValue { get => TSelf.FromBase(TBase.MinValue)!; }
    }

    public interface IMinMaxValueDerivedInitOnly<TSelf, TBase>
        : IMinMaxValueDerivedInitOnlyTagged<TSelf, TSelf, TBase>
        where TSelf : IMinMaxValueDerivedInitOnly<TSelf, TBase>?
        where TBase : IMinMaxValue<TBase>? {
    }

    public interface IMinMaxValueDerivedInitOnlyTagged<TSelf, Tag, TBase>
        : IMinMaxValueDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IMinMaxValueDerivedInitOnlyTagged<TSelf, Tag, TBase>?
        where TBase : IMinMaxValue<TBase>? {

        static TSelf IMinMaxValue<TSelf>.MaxValue { get; } = TSelf.FromBase(TBase.MaxValue)!;

        static TSelf IMinMaxValue<TSelf>.MinValue { get; } = TSelf.FromBase(TBase.MinValue)!;
    }

    public interface INumberBaseDerived<TSelf, TBase>
        : INumberBaseDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : INumberBaseDerived<TSelf, TBase>?
        where TBase : INumberBase<TBase>? {
    }

    public partial interface INumberBaseDerivedTagged<TSelf, Tag, TBase>
        : INumberBase<TSelf>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        , IEquatableDerivedTagged<TSelf, Tag, TBase>
        , IEqualityOperatorsDerivedTagged<TSelf, Tag, TBase, TSelf, TBase, bool, bool>
        where TSelf : INumberBaseDerivedTagged<TSelf, Tag, TBase>?
        where TBase : INumberBase<TBase>? {

        static TSelf? IInterfaceDerivedBase<TSelf, Tag<TSelf, IEqualityOperatorsDerivedTags.Other>, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag<TSelf, IEqualityOperatorsDerivedTags.Other>, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        // static bool IInterfaceDerivedBase<TSelf, Tag<bool, IEqualityOperatorsDerivedTags.Result>, bool, bool>.FromBase(bool value) => value;

        // static bool IInterfaceDerivedBase<TSelf, Tag<bool, IEqualityOperatorsDerivedTags.Result>, bool, bool>.ToBase(bool value) => value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TSelf? FromBase(TBase? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.FromBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TBase? ToBase(TSelf? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.ToBase(value);

        static TSelf INumberBase<TSelf>.One { get => TSelf.FromBase(TBase.One)!; }

        static int INumberBase<TSelf>.Radix => TBase.Radix;

        static TSelf INumberBase<TSelf>.Zero { get => TSelf.FromBase(TBase.Zero)!; }

        static TSelf IAdditiveIdentity<TSelf, TSelf>.AdditiveIdentity { get => TSelf.FromBase(TBase.AdditiveIdentity)!; }

        static TSelf IMultiplicativeIdentity<TSelf, TSelf>.MultiplicativeIdentity { get => TSelf.FromBase(TBase.MultiplicativeIdentity)!; }

        static TSelf INumberBase<TSelf>.Abs(TSelf value) => TSelf.FromBase(TBase.Abs(TSelf.ToBase(value)));

        static bool INumberBase<TSelf>.IsCanonical(TSelf value) => TBase.IsCanonical(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsComplexNumber(TSelf value) => TBase.IsComplexNumber(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsEvenInteger(TSelf value) => TBase.IsEvenInteger(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsFinite(TSelf value) => TBase.IsFinite(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsImaginaryNumber(TSelf value) => TBase.IsImaginaryNumber(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsInfinity(TSelf value) => TBase.IsInfinity(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsInteger(TSelf value) => TBase.IsInteger(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsNaN(TSelf value) => TBase.IsNaN(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsNegative(TSelf value) => TBase.IsNegative(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsNegativeInfinity(TSelf value) => TBase.IsNegativeInfinity(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsNormal(TSelf value) => TBase.IsNormal(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsOddInteger(TSelf value) => TBase.IsOddInteger(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsPositive(TSelf value) => TBase.IsPositive(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsPositiveInfinity(TSelf value) => TBase.IsPositiveInfinity(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsRealNumber(TSelf value) => TBase.IsRealNumber(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsSubnormal(TSelf value) => TBase.IsSubnormal(TSelf.ToBase(value));

        static bool INumberBase<TSelf>.IsZero(TSelf value) => TBase.IsZero(TSelf.ToBase(value));

        static TSelf INumberBase<TSelf>.MaxMagnitude(TSelf x, TSelf y) => TSelf.FromBase(TBase.MaxMagnitude(TSelf.ToBase(x), TSelf.ToBase(y)));

        static TSelf INumberBase<TSelf>.MaxMagnitudeNumber(TSelf x, TSelf y) => TSelf.FromBase(TBase.MaxMagnitudeNumber(TSelf.ToBase(x), TSelf.ToBase(y)));

        static TSelf INumberBase<TSelf>.MinMagnitude(TSelf x, TSelf y) => TSelf.FromBase(TBase.MinMagnitude(TSelf.ToBase(x), TSelf.ToBase(y)));

        static TSelf INumberBase<TSelf>.MinMagnitudeNumber(TSelf x, TSelf y) => TSelf.FromBase(TBase.MinMagnitudeNumber(TSelf.ToBase(x), TSelf.ToBase(y)));

        static TSelf IParsable<TSelf>.Parse(string s, IFormatProvider? provider) => TSelf.FromBase(TBase.Parse(s, provider));

        static TSelf ISpanParsable<TSelf>.Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => TSelf.FromBase(TBase.Parse(s, provider));

        static TSelf INumberBase<TSelf>.Parse(string s, NumberStyles style, IFormatProvider? provider) => TSelf.FromBase(TBase.Parse(s, style, provider));

        static TSelf INumberBase<TSelf>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) => TSelf.FromBase(TBase.Parse(s, style, provider));

        static bool INumberBase<TSelf>.TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
                return true;
            }
            var __ = TBase.TryConvertFromChecked(value, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool INumberBase<TSelf>.TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
                return true;
            }
            var __ = TBase.TryConvertFromSaturating(value, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool INumberBase<TSelf>.TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
                return true;
            }
            var __ = TBase.TryConvertFromTruncating(value, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool INumberBase<TSelf>.TryConvertToChecked<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TOther)(object)value;
                return true;
            }
            return TBase.TryConvertToChecked(TSelf.ToBase(value), out result);
        }

        static bool INumberBase<TSelf>.TryConvertToSaturating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TOther)(object)value;
                return true;
            }
            return TBase.TryConvertToSaturating(TSelf.ToBase(value), out result);
        }

        static bool INumberBase<TSelf>.TryConvertToTruncating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result) {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TOther)(object)value;
                return true;
            }
            return TBase.TryConvertToTruncating(TSelf.ToBase(value), out result);
        }

        static bool IParsable<TSelf>.TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
            var __ = TBase.TryParse(s, provider, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool ISpanParsable<TSelf>.TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
            var __ = TBase.TryParse(s, provider, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool INumberBase<TSelf>.TryParse(string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
            var __ = TBase.TryParse(s, style, provider, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        static bool INumberBase<TSelf>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out TSelf result) {
            var __ = TBase.TryParse(s, style, provider, out var result__);
            result = TSelf.FromBase(result__);
            return __;
        }

        string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => TSelf.ToBase((TSelf)(object)this).ToString(format, formatProvider);

        bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) => TSelf.ToBase((TSelf)(object)this).TryFormat(destination, out charsWritten, format, provider);

        static TSelf IUnaryPlusOperators<TSelf, TSelf>.operator +(TSelf value) => TSelf.FromBase(+TSelf.ToBase(value));

        static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator checked +(TSelf left, TSelf right) => TSelf.FromBase(checked(TSelf.ToBase(left) + TSelf.ToBase(right)));

        static TSelf IAdditionOperators<TSelf, TSelf, TSelf>.operator /*unchecked*/ +(TSelf left, TSelf right) => TSelf.FromBase(unchecked(TSelf.ToBase(left) + TSelf.ToBase(right)));

        static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator checked -(TSelf value) => TSelf.FromBase(checked(-TSelf.ToBase(value)));

        static TSelf IUnaryNegationOperators<TSelf, TSelf>.operator /*unchecked*/ -(TSelf value) => TSelf.FromBase(unchecked(-TSelf.ToBase(value)));

        static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator checked -(TSelf left, TSelf right) => TSelf.FromBase(checked(TSelf.ToBase(left) - TSelf.ToBase(right)));

        static TSelf ISubtractionOperators<TSelf, TSelf, TSelf>.operator /*unchecked*/ -(TSelf left, TSelf right) => TSelf.FromBase(unchecked(TSelf.ToBase(left) - TSelf.ToBase(right)));

        static TSelf IIncrementOperators<TSelf>.operator checked ++(TSelf value) {
            var value_ = TSelf.ToBase(value);
            checked {
                ++value_;
            }
            return TSelf.FromBase(value_);
        }

        static TSelf IIncrementOperators<TSelf>.operator /*unchecked*/ ++(TSelf value) {
            var value_ = TSelf.ToBase(value);
            unchecked {
                ++value_;
            }
            return TSelf.FromBase(value_);
        }
        static TSelf IDecrementOperators<TSelf>.operator checked --(TSelf value) {
            var value__ = TSelf.ToBase(value);
            checked {
                --value__;
            }
            return TSelf.FromBase(value__);
        }

        static TSelf IDecrementOperators<TSelf>.operator /*unchecked*/ --(TSelf value) {
            var value__ = TSelf.ToBase(value);
            unchecked {
                --value__;
            }
            return TSelf.FromBase(value__);
        }

        static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator checked *(TSelf left, TSelf right) => TSelf.FromBase(checked(TSelf.ToBase(left) * TSelf.ToBase(right)));
        static TSelf IMultiplyOperators<TSelf, TSelf, TSelf>.operator /*unchecked*/ *(TSelf left, TSelf right) => TSelf.FromBase(unchecked(TSelf.ToBase(left)! * TSelf.ToBase(right)!))!;

        static TSelf IDivisionOperators<TSelf, TSelf, TSelf>.operator /(TSelf left, TSelf right) => TSelf.FromBase(TSelf.ToBase(left)! / TSelf.ToBase(right)!)!;

        static bool IEqualityOperators<TSelf, TSelf, bool>.operator ==(TSelf? left, TSelf? right) => TSelf.ToBase(left) == TSelf.ToBase(right);
        static bool IEqualityOperators<TSelf, TSelf, bool>.operator !=(TSelf? left, TSelf? right) => TSelf.ToBase(left) != TSelf.ToBase(right);
    }

    public interface IFloatingPointConstantsDerived<TSelf, TBase>
        : IFloatingPointConstantsDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IFloatingPointConstantsDerived<TSelf, TBase>?
        where TBase : IFloatingPointConstants<TBase>? {
    }

    /// <inheritdoc cref="IFloatingPointConstants{TSelf}"/>
    /// <typeparam name="TBase">
    /// The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.
    /// </typeparam>
    public interface IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        : IFloatingPointConstants<TSelf>, INumberBaseDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IFloatingPointConstants<TBase>? {

        static TSelf IFloatingPointConstants<TSelf>.E { get => TSelf.FromBase(TBase.E)!; }

        static TSelf IFloatingPointConstants<TSelf>.Pi { get => TSelf.FromBase(TBase.Pi)!; }

        static TSelf IFloatingPointConstants<TSelf>.Tau { get => TSelf.FromBase(TBase.Tau)!; }
    }

    public static partial class IEqualityOperatorsDerivedTags {

        public readonly struct Other : ITag {
        }

        public readonly struct Result : ITag {
        }
    }

    public interface IEqualityOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IEqualityOperatorsDerivedTagged<TSelf, TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        where TSelf : IEqualityOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IEqualityOperators<TBase, TOtherBase, TResultBase>? {
    }

    /// <inheritdoc cref="IEqualityOperators{TSelf,TOther,TResult}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    /// <typeparam name="TOtherBase">The source/base type supplying data or behavior that <typeparamref name="TOther"/> adapts or forwards.</typeparam>
    /// <typeparam name="TResultBase">The source/base type supplying data or behavior that <typeparamref name="TResult"/> adapts or forwards.</typeparam>
    public interface IEqualityOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IEqualityOperators<TSelf, TOther, TResult>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        , IInterfaceDerivedBase<TSelf, Tag<TOther, IEqualityOperatorsDerivedTags.Other>, TOther, TOtherBase>
        , IInterfaceDerivedBase<TSelf, Tag<TResult, IEqualityOperatorsDerivedTags.Result>, TResult, TResultBase>
        where TSelf : IEqualityOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IEqualityOperators<TBase, TOtherBase, TResultBase>? {

        static TResult IEqualityOperators<TSelf, TOther, TResult>.operator ==(TSelf? left, TOther? right) => TSelf.FromBase(TSelf.ToBase(left) == TSelf.ToBase(right))!;

        static TResult IEqualityOperators<TSelf, TOther, TResult>.operator !=(TSelf? left, TOther? right) => TSelf.FromBase(TSelf.ToBase(left) != TSelf.ToBase(right))!;
    }

    public static partial class IComparisonOperatorsDerivedTags {

        public readonly struct Other : ITag {
        }

        public readonly struct Result : ITag {
        }
    }

    public interface IComparisonOperatorssDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IComparisonOperatorsDerivedTagged<TSelf, TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        where TSelf : IComparisonOperatorssDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IComparisonOperators<TBase, TOtherBase, TResultBase>? {
    }

    /// <inheritdoc cref="IComparisonOperators{TSelf,TOther,TResult}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    /// <typeparam name="TOtherBase">The source/base type supplying data or behavior that <typeparamref name="TOther"/> adapts or forwards.</typeparam>
    /// <typeparam name="TResultBase">The source/base type supplying data or behavior that <typeparamref name="TResult"/> adapts or forwards.</typeparam>
    public interface IComparisonOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IComparisonOperators<TSelf, TOther, TResult>
        , IEqualityOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>
        where TSelf : IComparisonOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IComparisonOperators<TBase, TOtherBase, TResultBase>? {

        static TResult IComparisonOperators<TSelf, TOther, TResult>.operator <(TSelf left, TOther right) => TSelf.FromBase(TSelf.ToBase(left)! < TSelf.ToBase(right)!)!;

        static TResult IComparisonOperators<TSelf, TOther, TResult>.operator <=(TSelf left, TOther right) => TSelf.FromBase(TSelf.ToBase(left)! <= TSelf.ToBase(right)!)!;

        static TResult IComparisonOperators<TSelf, TOther, TResult>.operator >(TSelf left, TOther right) => TSelf.FromBase(TSelf.ToBase(left)! > TSelf.ToBase(right)!)!;

        static TResult IComparisonOperators<TSelf, TOther, TResult>.operator >=(TSelf left, TOther right) => TSelf.FromBase(TSelf.ToBase(left)! >= TSelf.ToBase(right)!)!;
    }

    public static partial class IModulusOperatorsDerivedTags {

        public readonly struct Other : ITag {
        }

        public readonly struct Result : ITag {
        }
    }

    public interface IModulusOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IModulusOperatorsDerivedTagged<TSelf, TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>
        where TSelf : IModulusOperatorsDerived<TSelf, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IModulusOperators<TBase, TOtherBase, TResultBase>? {
    }

    /// <inheritdoc cref="IModulusOperators{TSelf,TOther,TResult}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    /// <typeparam name="TOtherBase">The source/base type supplying data or behavior that <typeparamref name="TOther"/> adapts or forwards.</typeparam>
    /// <typeparam name="TResultBase">The source/base type supplying data or behavior that <typeparamref name="TResult"/> adapts or forwards.</typeparam>
    public interface IModulusOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>
        : IModulusOperators<TSelf, TOther, TResult>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<Tag>, TBase>
        , IInterfaceDerivedBase<TSelf, Tag<TOther, IModulusOperatorsDerivedTags.Other>, TOther, TOtherBase>
        , IInterfaceDerivedBase<TSelf, Tag<TResult, IModulusOperatorsDerivedTags.Result>, TResult, TResultBase>
        where TSelf : IModulusOperatorsDerivedTagged<TSelf, Tag, TBase, TOther, TOtherBase, TResult, TResultBase>?
        where TBase : IModulusOperators<TBase, TOtherBase, TResultBase>? {

        static TResult IModulusOperators<TSelf, TOther, TResult>.operator %(TSelf left, TOther right) => TSelf.FromBase(TSelf.ToBase(left)! % TSelf.ToBase(right)!)!;
    }

    public interface INumberDerived<TSelf, TBase>
        : INumberDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : INumberDerived<TSelf, TBase>?
        where TBase : INumber<TBase>? {
    }

    /// <inheritdoc cref="INumber{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface INumberDerivedTagged<TSelf, Tag, TBase>
        : INumber<TSelf>
        , IComparableNongenericDerivedTagged<TSelf, Tag, TBase>
        , IComparableDerivedTagged<TSelf, Tag, TBase>
        , IComparisonOperatorsDerivedTagged<TSelf, Tag, TBase, TSelf, TBase, bool, bool>
        , IModulusOperatorsDerivedTagged<TSelf, Tag, TBase, TSelf, TBase, TSelf, TBase>
        , INumberBaseDerivedTagged<TSelf, Tag, TBase>
        where TSelf : INumberDerivedTagged<TSelf, Tag, TBase>?
        where TBase : INumber<TBase>? {

        static TSelf? IInterfaceDerivedBase<TSelf, Tag<TSelf, IModulusOperatorsDerivedTags.Other>, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag<TSelf, IModulusOperatorsDerivedTags.Other>, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        static TSelf? IInterfaceDerivedBase<TSelf, Tag<TSelf, IModulusOperatorsDerivedTags.Result>, TSelf, TBase>.FromBase(TBase? value) => TSelf.FromBase(value);

        static TBase? IInterfaceDerivedBase<TSelf, Tag<TSelf, IModulusOperatorsDerivedTags.Result>, TSelf, TBase>.ToBase(TSelf? value) => TSelf.ToBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TSelf? FromBase(TBase? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.FromBase(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNullIfNotNull(nameof(value))]
        protected static virtual new TBase? ToBase(TSelf? value) => IInterfaceDerivedTaggedSelfBaseFriend<TSelf, Tag<Tag>, TBase>.ToBase(value);
    }

    public interface ISignedNumberDerived<TSelf, TBase>
       : ISignedNumberDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : ISignedNumberDerived<TSelf, TBase>?
       where TBase : ISignedNumber<TBase>? {
    }

    /// <inheritdoc cref="ISignedNumber{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface ISignedNumberDerivedTagged<TSelf, Tag, TBase>
        : ISignedNumber<TSelf>
        , INumberBaseDerivedTagged<TSelf, Tag, TBase>
        where TSelf : ISignedNumberDerivedTagged<TSelf, Tag, TBase>?
        where TBase : ISignedNumber<TBase>? {

        static TSelf ISignedNumber<TSelf>.NegativeOne { get => TSelf.FromBase(TBase.NegativeOne)!; }
    }

    public interface ISignedNumberDerivedInitOnly<TSelf, TBase>
       : ISignedNumberDerivedInitOnlyTagged<TSelf, TSelf, TBase>
       where TSelf : ISignedNumberDerivedInitOnly<TSelf, TBase>?
       where TBase : ISignedNumber<TBase>? {
    }

    /// <inheritdoc cref="ISignedNumberDerived{TSelf,TBase}"/>
    public interface ISignedNumberDerivedInitOnlyTagged<TSelf, Tag, TBase>
        : ISignedNumberDerivedTagged<TSelf, Tag, TBase>
        where TSelf : ISignedNumberDerivedInitOnlyTagged<TSelf, Tag, TBase>?
        where TBase : ISignedNumber<TBase>? {

        static TSelf ISignedNumber<TSelf>.NegativeOne { get; } = TSelf.FromBase(TBase.NegativeOne)!;
    }

    public interface IFloatingPointDerived<TSelf, TBase>
      : IFloatingPointDerivedTagged<TSelf, TSelf, TBase>
      where TSelf : IFloatingPointDerived<TSelf, TBase>?
      where TBase : IFloatingPoint<TBase>? {
    }

    /// <inheritdoc cref="IFloatingPoint{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IFloatingPointDerivedTagged<TSelf, Tag, TBase>
        : IFloatingPoint<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        , INumberDerivedTagged<TSelf, Tag, TBase>
        , ISignedNumberDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IFloatingPointDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IFloatingPoint<TBase>? {

        static TSelf IFloatingPoint<TSelf>.Round(TSelf x, int digits, MidpointRounding mode) => TSelf.FromBase(TBase.Round(TSelf.ToBase(x)!, digits, mode))!;

        int IFloatingPoint<TSelf>.GetExponentByteCount() => TSelf.ToBase((TSelf)(object)this).GetExponentByteCount();

        int IFloatingPoint<TSelf>.GetExponentShortestBitLength() => TSelf.ToBase((TSelf)(object)this).GetExponentShortestBitLength();

        int IFloatingPoint<TSelf>.GetSignificandBitLength() => TSelf.ToBase((TSelf)(object)this).GetSignificandBitLength();

        int IFloatingPoint<TSelf>.GetSignificandByteCount() => TSelf.ToBase((TSelf)(object)this).GetSignificandByteCount();

        bool IFloatingPoint<TSelf>.TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) => TSelf.ToBase((TSelf)(object)this).TryWriteExponentBigEndian(destination, out bytesWritten);

        bool IFloatingPoint<TSelf>.TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) => TSelf.ToBase((TSelf)(object)this).TryWriteExponentLittleEndian(destination, out bytesWritten);

        bool IFloatingPoint<TSelf>.TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) => TSelf.ToBase((TSelf)(object)this).TryWriteSignificandBigEndian(destination, out bytesWritten);

        bool IFloatingPoint<TSelf>.TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) => TSelf.ToBase((TSelf)(object)this).TryWriteSignificandLittleEndian(destination, out bytesWritten);
    }

    public interface IExponentialFunctionsDerived<TSelf, TBase>
        : IExponentialFunctionsDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IExponentialFunctionsDerived<TSelf, TBase>?
        where TBase : IExponentialFunctions<TBase>? {
    }

    /// <inheritdoc cref="IExponentialFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IExponentialFunctionsDerivedTagged<TSelf, Tag, TBase>
        : IExponentialFunctions<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IExponentialFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IExponentialFunctions<TBase>? {

        static TSelf IExponentialFunctions<TSelf>.Exp(TSelf x) => TSelf.FromBase(TBase.Exp(TSelf.ToBase(x)!))!;

        static TSelf IExponentialFunctions<TSelf>.ExpM1(TSelf x) => TSelf.FromBase(TBase.ExpM1(TSelf.ToBase(x)!))!;

        static TSelf IExponentialFunctions<TSelf>.Exp2(TSelf x) => TSelf.FromBase(TBase.Exp2(TSelf.ToBase(x)!))!;

        static TSelf IExponentialFunctions<TSelf>.Exp2M1(TSelf x) => TSelf.FromBase(TBase.Exp2M1(TSelf.ToBase(x)!))!;

        static TSelf IExponentialFunctions<TSelf>.Exp10(TSelf x) => TSelf.FromBase(TBase.Exp10(TSelf.ToBase(x)!))!;

        static TSelf IExponentialFunctions<TSelf>.Exp10M1(TSelf x) => TSelf.FromBase(TBase.Exp10M1(TSelf.ToBase(x)!))!;
    }

    public interface IHyperbolicFunctionsDerived<TSelf, TBase>
       : IHyperbolicFunctionsDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : IHyperbolicFunctionsDerived<TSelf, TBase>?
       where TBase : IHyperbolicFunctions<TBase>? {
    }

    /// <inheritdoc cref="IHyperbolicFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IHyperbolicFunctionsDerivedTagged<TSelf, Tag, TBase>
        : IHyperbolicFunctions<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IHyperbolicFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IHyperbolicFunctions<TBase>? {

        static TSelf IHyperbolicFunctions<TSelf>.Acosh(TSelf x) => TSelf.FromBase(TBase.Acosh(TSelf.ToBase(x)!))!;

        static TSelf IHyperbolicFunctions<TSelf>.Asinh(TSelf x) => TSelf.FromBase(TBase.Asinh(TSelf.ToBase(x)!))!;

        static TSelf IHyperbolicFunctions<TSelf>.Atanh(TSelf x) => TSelf.FromBase(TBase.Atanh(TSelf.ToBase(x)!))!;

        static TSelf IHyperbolicFunctions<TSelf>.Cosh(TSelf x) => TSelf.FromBase(TBase.Cosh(TSelf.ToBase(x)!))!;

        static TSelf IHyperbolicFunctions<TSelf>.Sinh(TSelf x) => TSelf.FromBase(TBase.Sinh(TSelf.ToBase(x)!))!;

        static TSelf IHyperbolicFunctions<TSelf>.Tanh(TSelf x) => TSelf.FromBase(TBase.Tanh(TSelf.ToBase(x)!))!;
    }

    public interface ILogarithmicFunctionsDerived<TSelf, TBase>
       : ILogarithmicFunctionsDerivedTagged<TSelf, TSelf, TBase>
       where TSelf : ILogarithmicFunctionsDerived<TSelf, TBase>?
       where TBase : ILogarithmicFunctions<TBase>? {
    }

    /// <inheritdoc cref="ILogarithmicFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface ILogarithmicFunctionsDerivedTagged<TSelf, Tag, TBase>
        : ILogarithmicFunctions<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : ILogarithmicFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : ILogarithmicFunctions<TBase>? {

        static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x) => TSelf.FromBase(TBase.Log(TSelf.ToBase(x)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.Log(TSelf x, TSelf newBase) => TSelf.FromBase(TBase.Log(TSelf.ToBase(x)!, TSelf.ToBase(newBase)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.LogP1(TSelf x) => TSelf.FromBase(TBase.LogP1(TSelf.ToBase(x)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.Log2(TSelf x) => TSelf.FromBase(TBase.Log2(TSelf.ToBase(x)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.Log2P1(TSelf x) => TSelf.FromBase(TBase.Log2P1(TSelf.ToBase(x)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.Log10(TSelf x) => TSelf.FromBase(TBase.Log10(TSelf.ToBase(x)!))!;

        static TSelf ILogarithmicFunctions<TSelf>.Log10P1(TSelf x) => TSelf.FromBase(TBase.Log10P1(TSelf.ToBase(x)!))!;
    }

    public interface IPowerFunctionsDerived<TSelf, TBase>
      : IPowerFunctionsDerivedTagged<TSelf, TSelf, TBase>
      where TSelf : IPowerFunctionsDerived<TSelf, TBase>?
      where TBase : IPowerFunctions<TBase>? {
    }

    /// <inheritdoc cref="IPowerFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IPowerFunctionsDerivedTagged<TSelf, Tag, TBase>
        : IPowerFunctions<TSelf>
        , INumberBaseDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IPowerFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IPowerFunctions<TBase>? {

        static TSelf IPowerFunctions<TSelf>.Pow(TSelf x, TSelf y) => TSelf.FromBase(TBase.Pow(TSelf.ToBase(x)!, TSelf.ToBase(y)!))!;
    }

    public interface IRootFunctionsDerived<TSelf, TBase>
        : IRootFunctionsDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IRootFunctionsDerived<TSelf, TBase>?
        where TBase : IRootFunctions<TBase>? {
    }

    /// <inheritdoc cref="IRootFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IRootFunctionsDerivedTagged<TSelf, Tag, TBase>
        : IRootFunctions<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IRootFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : IRootFunctions<TBase>? {

        static TSelf IRootFunctions<TSelf>.Cbrt(TSelf x) => TSelf.FromBase(TBase.Cbrt(TSelf.ToBase(x)!))!;

        static TSelf IRootFunctions<TSelf>.Hypot(TSelf x, TSelf y) => TSelf.FromBase(TBase.Hypot(TSelf.ToBase(x)!, TSelf.ToBase(y)!))!;

        static TSelf IRootFunctions<TSelf>.RootN(TSelf x, int n) => TSelf.FromBase(TBase.RootN(TSelf.ToBase(x)!, n))!;

        static TSelf IRootFunctions<TSelf>.Sqrt(TSelf x) => TSelf.FromBase(TBase.Sqrt(TSelf.ToBase(x)!))!;
    }

    public interface ITrigonometricFunctionsDerived<TSelf, TBase>
        : ITrigonometricFunctionsDerivedTagged<TSelf, TSelf, TBase>
        where TSelf : ITrigonometricFunctionsDerived<TSelf, TBase>?
        where TBase : ITrigonometricFunctions<TBase>? {
    }

    /// <inheritdoc cref="ITrigonometricFunctions{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface ITrigonometricFunctionsDerivedTagged<TSelf, Tag, TBase>
        : ITrigonometricFunctions<TSelf>
        , IFloatingPointConstantsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : ITrigonometricFunctionsDerivedTagged<TSelf, Tag, TBase>?
        where TBase : ITrigonometricFunctions<TBase>? {

        static TSelf ITrigonometricFunctions<TSelf>.Acos(TSelf x) => TSelf.FromBase(TBase.Acos(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.AcosPi(TSelf x) => TSelf.FromBase(TBase.AcosPi(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.Asin(TSelf x) => TSelf.FromBase(TBase.Asin(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.AsinPi(TSelf x) => TSelf.FromBase(TBase.AsinPi(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.Atan(TSelf x) => TSelf.FromBase(TBase.Atan(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.AtanPi(TSelf x) => TSelf.FromBase(TBase.AtanPi(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.Cos(TSelf x) => TSelf.FromBase(TBase.Cos(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.CosPi(TSelf x) => TSelf.FromBase(TBase.CosPi(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.DegreesToRadians(TSelf degrees) => TSelf.FromBase(TBase.DegreesToRadians(TSelf.ToBase(degrees)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.RadiansToDegrees(TSelf radians) => TSelf.FromBase(TBase.RadiansToDegrees(TSelf.ToBase(radians)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.Sin(TSelf x) => TSelf.FromBase(TBase.Sin(TSelf.ToBase(x)!))!;

        static (TSelf Sin, TSelf Cos) ITrigonometricFunctions<TSelf>.SinCos(TSelf x) {
            var t = TBase.SinCos(TSelf.ToBase(x)!);
            return (TSelf.FromBase(t.Sin)!, TSelf.FromBase(t.Cos)!);
        }

        static (TSelf SinPi, TSelf CosPi) ITrigonometricFunctions<TSelf>.SinCosPi(TSelf x) {
            var t = TBase.SinCosPi(TSelf.ToBase(x)!);
            return (TSelf.FromBase(t.SinPi)!, TSelf.FromBase(t.CosPi)!);
        }

        static TSelf ITrigonometricFunctions<TSelf>.SinPi(TSelf x) => TSelf.FromBase(TBase.SinPi(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.Tan(TSelf x) => TSelf.FromBase(TBase.Tan(TSelf.ToBase(x)!))!;

        static TSelf ITrigonometricFunctions<TSelf>.TanPi(TSelf x) => TSelf.FromBase(TBase.TanPi(TSelf.ToBase(x)!))!;
    }

    public interface IFloatingPointIeee754Derived<TSelf, TBase>
        : IFloatingPointIeee754DerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IFloatingPointIeee754Derived<TSelf, TBase>?
        where TBase : IFloatingPointIeee754<TBase>? {
    }

    public interface IFloatingPointIeee754DerivedTagged<TSelf, Tag, TBase>
        : IFloatingPointIeee754<TSelf>
        , IExponentialFunctionsDerivedTagged<TSelf, Tag, TBase>
        , IFloatingPointDerivedTagged<TSelf, Tag, TBase>
        , IHyperbolicFunctionsDerivedTagged<TSelf, Tag, TBase>
        , ILogarithmicFunctionsDerivedTagged<TSelf, Tag, TBase>
        , IPowerFunctionsDerivedTagged<TSelf, Tag, TBase>
        , IRootFunctionsDerivedTagged<TSelf, Tag, TBase>
        , ITrigonometricFunctionsDerivedTagged<TSelf, Tag, TBase>
        where TSelf : IFloatingPointIeee754DerivedTagged<TSelf, Tag, TBase>?
        where TBase : IFloatingPointIeee754<TBase>? {

        static TSelf IFloatingPointIeee754<TSelf>.Epsilon => TSelf.FromBase(TBase.Epsilon)!;

        static TSelf IFloatingPointIeee754<TSelf>.NaN => TSelf.FromBase(TBase.NaN)!;

        static TSelf IFloatingPointIeee754<TSelf>.NegativeInfinity => TSelf.FromBase(TBase.NegativeInfinity)!;

        static TSelf IFloatingPointIeee754<TSelf>.NegativeZero => TSelf.FromBase(TBase.NegativeZero)!;

        static TSelf IFloatingPointIeee754<TSelf>.PositiveInfinity => TSelf.FromBase(TBase.PositiveInfinity)!;

        static TSelf IFloatingPointIeee754<TSelf>.Atan2(TSelf y, TSelf x) => TSelf.FromBase(TBase.Atan2(TSelf.ToBase(y)!, TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.Atan2Pi(TSelf y, TSelf x) => TSelf.FromBase(TBase.Atan2Pi(TSelf.ToBase(y)!, TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.BitDecrement(TSelf x) => TSelf.FromBase(TBase.BitDecrement(TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.BitIncrement(TSelf x) => TSelf.FromBase(TBase.BitIncrement(TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.FusedMultiplyAdd(TSelf left, TSelf right, TSelf addend) => TSelf.FromBase(TBase.FusedMultiplyAdd(TSelf.ToBase(left)!, TSelf.ToBase(right)!, TSelf.ToBase(addend)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.Ieee754Remainder(TSelf left, TSelf right) => TSelf.FromBase(TBase.Ieee754Remainder(TSelf.ToBase(left)!, TSelf.ToBase(right)!))!;

        static int IFloatingPointIeee754<TSelf>.ILogB(TSelf x) => TBase.ILogB(TSelf.ToBase(x)!);

        static TSelf IFloatingPointIeee754<TSelf>.Lerp(TSelf value1, TSelf value2, TSelf amount) => TSelf.FromBase(TBase.Lerp(TSelf.ToBase(value1)!, TSelf.ToBase(value2)!, TSelf.ToBase(amount)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.ReciprocalEstimate(TSelf x) => TSelf.FromBase(TBase.ReciprocalEstimate(TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.ReciprocalSqrtEstimate(TSelf x) => TSelf.FromBase(TBase.ReciprocalSqrtEstimate(TSelf.ToBase(x)!))!;

        static TSelf IFloatingPointIeee754<TSelf>.ScaleB(TSelf x, int n) => TSelf.FromBase(TBase.ScaleB(TSelf.ToBase(x)!, n))!;
    }

    public interface IDecimalFloatingPointIeee754Derived<TSelf, TBase>
        : IDecimalFloatingPointIeee754DerivedTagged<TSelf, TSelf, TBase>
        where TSelf : IDecimalFloatingPointIeee754Derived<TSelf, TBase>?
        where TBase : IDecimalFloatingPointIeee754<TBase>? {
    }

    /// <inheritdoc cref="IDecimalFloatingPointIeee754{TSelf}"/>
    /// <typeparam name="TBase">The source/base type supplying data or behavior that <typeparamref name="TSelf"/> adapts or forwards.</typeparam>
    public interface IDecimalFloatingPointIeee754DerivedTagged<TSelf, Tag, TBase>
        : IFloatingPointIeee754DerivedTagged<TSelf, Tag, TBase>
        , IDecimalFloatingPointIeee754<TSelf>
        where TSelf : IDecimalFloatingPointIeee754DerivedTagged<TSelf, Tag, TBase>?
        where TBase : IDecimalFloatingPointIeee754<TBase>? {

        static int INumberBase<TSelf>.Radix => TBase.Radix;
    }
}
namespace UltimateOrb {

    partial class Decimal128Extensions {


        extension(System.BitConverter) {

            public static System.Int128 Decimal128ToInt128Bits(Decimal128Bid value) {
                return unchecked((System.Int128)value.bits);
            }

            public static System.UInt128 Decimal128ToUInt128Bits(Decimal128Bid value) {
                return unchecked((System.UInt128)value.bits);
            }

            public static System.Int128 Decimal128ToInt128Bits(Decimal128Dpd value) {
                return unchecked((System.Int128)value.bits);
            }

            public static System.UInt128 Decimal128ToUInt128Bits(Decimal128Dpd value) {
                return unchecked((System.UInt128)value.bits);
            }

            public static System.Int128 Decimal128ToInt128Bits(Decimal128 value) {
                return unchecked((System.Int128)value.value.bits);
            }

            public static System.UInt128 Decimal128ToUInt128Bits(Decimal128 value) {
                return unchecked((System.UInt128)value.value.bits);
            }
        }
    }
}

namespace UltimateOrb {

    [Experimental("UoWIP")]
    public static partial class Decimal128Extensions {
    }
}

namespace UltimateOrb {
    partial class Decimal128Extensions {

        extension(Decimal128Bid @this) {

            public static (Int64 D2, UInt64 D1, UInt64 D0) TotalOrderIeee754_192BitsKeySelector(Decimal128Bid value) {
                var x = unchecked((System.Int128)(Decimal128Bid.IsZero(value) ? Decimal128Bid.CopySign(default, value) : Decimal128Bid.ToFinestCohort(value)).bits);
                Debug.Assert(System.Int128.IsNegative(x) == Decimal128Bid.IsNegative(value));
                if (Decimal128Bid.IsNaN(value)) {
                    var x1 = new Decimal128Bid(unchecked((UInt128)x), default);
                    x ^= (System.Int128)new UltimateOrb.Int128(lo: 0u, hi: unchecked((Int64)0X_02000000_00000000L));
                }
                x = System.Int128.IsNegative(x) ? unchecked(System.Int128.MaxValue - x) : x;
                var exp = unchecked((Int32)value.ExtractRawBiasedExponentAndRawSignificand(out _));
                Debug.Assert(exp >= 0);
                if (exp >= Decimal128Bid.MaxBiasedExponent) {
                    exp = Decimal128Bid.MaxBiasedExponent;
                }
                exp = System.Int128.IsNegative(x) ? unchecked(Decimal128Bid.MaxBiasedExponent - exp) : exp;
                return (x.GetHighPart(), x.GetLowPart(), unchecked((UInt64)(Int64)exp));
            }

            public static System.Int128 TotalOrderDefaultSystemInt128KeySelector(Decimal128Bid value) {
                var x = unchecked((System.Int128)(Decimal128Bid.IsZero(value) ? Decimal128Bid.CopySign(default, value) : Decimal128Bid.ToFinestCohort(value, resetNaNSignAndPayload: true)).bits);
                return System.Int128.IsNegative(x) ? unchecked(System.Int128.MinValue - x) : x;
            }
        }
    }

}
namespace UltimateOrb {

    // Wrapper. Same API shape as Decimal128Bid
    [Experimental("UoWIP")]
    public readonly struct Decimal128 :
        IInterfaceDerivedSelfBase<Decimal128, Decimal128Bid>,
        IComparable, IComparableNongenericDerived<Decimal128, Decimal128Bid>,
        IConvertible, IConvertibleNongenericDerived<Decimal128, Decimal128Bid>,
        IComparable<Decimal128>, IComparableDerived<Decimal128, Decimal128Bid>,
        IEquatable<Decimal128>, IEquatableDerived<Decimal128, Decimal128Bid>,
        IDecimalFloatingPointIeee754<Decimal128>, IDecimalFloatingPointIeee754Derived<Decimal128, Decimal128Bid>,
        IMinMaxValue<Decimal128>, IMinMaxValueDerivedInitOnly<Decimal128, Decimal128Bid>,
        IUtf8SpanFormattable {

        internal readonly Decimal128Bid value;

        static Decimal128 IInterfaceDerivedSelfBase<Decimal128, Decimal128Bid>.FromBase(Decimal128Bid value) => InterfaceDerivedDefault<Decimal128, Decimal128Bid>.FromBase(value);

        static Decimal128Bid IInterfaceDerivedSelfBase<Decimal128, Decimal128Bid>.ToBase(Decimal128 value) => InterfaceDerivedDefault<Decimal128, Decimal128Bid>.ToBase(value);
    }

    [Experimental("UoWIP")]
    static partial class Decimal128EncodingConverter {
        // Build at startup
        private static readonly UInt16[] s_decToDpd = BuildDecToDpdTable();
        private static readonly UInt16[] s_dpdToDec = BuildDpdToDecTable();

        private static UInt16[] BuildDecToDpdTable() {
            var table = new UInt16[1000];
            for (int v = 0; v < 1000; v++) {
                int d0 = v % 10;
                int d1 = (v / 10) % 10;
                int d2 = (v / 100) % 10;
                table[v] = EncodeDpdThreeDigits((byte)d2, (byte)d1, (byte)d0);
            }
            return table;
        }

        private static ushort[] BuildDpdToDecTable() {
            var inv = new ushort[1024];
            for (int i = 0; i < inv.Length; i++) inv[i] = 0xFFFF;
            for (int d = 0; d < 1000; d++) {
                ushort dpd = s_decToDpd[d];
                inv[dpd] = (ushort)d;
            }
            return inv;
        }

        // Encode three decimal digits (d2,d1,d0) into 10-bit DPD (canonical)
        // Implementation follows Cowlishaw's DPD mapping rules.
        private static ushort EncodeDpdThreeDigits(byte d2, byte d1, byte d0) {
            // BCD bits: a3..a0, b3..b0, c3..c0
            int a = d2, b = d1, c = d0;
            int a0 = a & 1, a1 = (a >> 1) & 1, a2 = (a >> 2) & 1, a3 = (a >> 3) & 1;
            int b0 = b & 1, b1 = (b >> 1) & 1, b2 = (b >> 2) & 1, b3 = (b >> 3) & 1;
            int c0 = c & 1, c1 = (c >> 1) & 1, c2 = (c >> 2) & 1, c3 = (c >> 3) & 1;

            // Compose DPD bits p9..p0 according to the 8-case Cowlishaw mapping.
            // The mapping below implements the canonical DPD encoding.
            int p9, p8, p7, p6, p5, p4, p3, p2, p1, p0;

            int ms = (a3 << 2) | (b3 << 1) | c3;
            switch (ms) {
            // case 000
            case 0:
                p9 = a2; p8 = a1; p7 = a0;
                p6 = b2; p5 = b1; p4 = b0;
                p3 = c2; p2 = c1; p1 = c0;
                p0 = 0;
                break;

            // case 001
            case 1:
                p9 = a2; p8 = a1; p7 = a0;
                p6 = b2; p5 = b1; p4 = b0;
                p3 = 1; p2 = c2; p1 = c1; p0 = c0;
                break;

            // case 010
            case 2:
                p9 = a2; p8 = a1; p7 = a0;
                p6 = 1; p5 = b2; p4 = b1; p3 = b0;
                p2 = c2; p1 = c1; p0 = c0;
                break;

            // case 011
            case 3:
                p9 = a2; p8 = a1; p7 = a0;
                p6 = 1; p5 = b2; p4 = b1; p3 = b0;
                p2 = 1; p1 = c2; p0 = c1;
                break;

            // case 100
            case 4:
                p9 = 1; p8 = a2; p7 = a1; p6 = a0;
                p5 = b2; p4 = b1; p3 = b0;
                p2 = c2; p1 = c1; p0 = c0;
                break;

            // case 101
            case 5:
                p9 = 1; p8 = a2; p7 = a1; p6 = a0;
                p5 = b2; p4 = b1; p3 = b0;
                p2 = 1; p1 = c2; p0 = c1;
                break;

            // case 110
            case 6:
                p9 = 1; p8 = a2; p7 = a1; p6 = a0;
                p5 = 1; p4 = b2; p3 = b1; p2 = b0;
                p1 = c2; p0 = c1;
                break;

            // case 111
            default:
                p9 = 1; p8 = a2; p7 = a1; p6 = a0;
                p5 = 1; p4 = b2; p3 = b1; p2 = b0;
                p1 = 1; p0 = c2;
                break;
            }

            // pack bits into 10-bit ushort (p9 is MSB)
            int dpd = (p9 << 9) | (p8 << 8) | (p7 << 7) | (p6 << 6) |
                      (p5 << 5) | (p4 << 4) | (p3 << 3) | (p2 << 2) |
                      (p1 << 1) | p0;
            return (ushort)dpd;
        }
    }

    partial struct Decimal128Dpd {
    }

    [Experimental("UoWIP")]
    public readonly partial struct Decimal128Dpd :
        IInterfaceDerivedSelfBase<Decimal128Dpd, Decimal128Bid>,
        IComparable, IComparableNongenericDerived<Decimal128Dpd, Decimal128Bid>,
        IConvertible, IConvertibleNongenericDerived<Decimal128Dpd, Decimal128Bid>,
        IComparable<Decimal128Dpd>, IComparableDerived<Decimal128Dpd, Decimal128Bid>,
        IEquatable<Decimal128Dpd>, IEquatableDerived<Decimal128Dpd, Decimal128Bid>,
        IDecimalFloatingPointIeee754<Decimal128Dpd>, IDecimalFloatingPointIeee754Derived<Decimal128Dpd, Decimal128Bid>,
        IMinMaxValue<Decimal128Dpd>, IMinMaxValueDerivedInitOnly<Decimal128Dpd, Decimal128Bid>,
        IUtf8SpanFormattable {

        internal readonly UInt128 bits;

        static Decimal128Dpd IInterfaceDerivedSelfBase<Decimal128Dpd, Decimal128Bid>.FromBase(Decimal128Bid value) {
            return value;
        }

        static Decimal128Bid IInterfaceDerivedSelfBase<Decimal128Dpd, Decimal128Bid>.ToBase(Decimal128Dpd value) {
            return value;
        }

        public static implicit operator Decimal128Bid(Decimal128Dpd value) {
            throw new NotImplementedException();
        }

        public static implicit operator Decimal128Dpd(Decimal128Bid value) {
            throw new NotImplementedException();
        }

        internal readonly struct ConstructorInternalFromPartsTag {
        }

        static readonly ConstructorInternalFromPartsTag CtorFromParts;

        internal Decimal128Dpd(UInt128 significand, uint biasedExponent, UInt64 sign, ConstructorInternalFromPartsTag _) {
            Debug.Assert(sign == 0 || sign == (1UL << 63));
            Debug.Assert(biasedExponent <= Decimal128Bid.MaxBiasedExponent);
            Debug.Assert(significand <= (((UInt128)0b1010UL << 110) - 1));
            Debug.Assert((biasedExponent <= Decimal128Bid.MaxBiasedExponent) || (significand >= (UInt128)0b1000UL << 110));

            // split into halves
            var lo = unchecked((UInt64)significand.LoInt64Bits);
            var hi = unchecked((UInt64)significand.HiInt64Bits);

            // format test (absolute bit 113 -> hi bit index 49)
            const int formatTestOffsetHi = 113 - 64; // 49
            bool isFormat1 = (hi & (1UL << formatTestOffsetHi)) != 0;

            // stored field offsets (absolute) and lengths
            const int expLowOffsetAbs = 110;
            const int expLowLen = 12;
            const int significandHighOffsetAbs = 122;
            int significandHighLen = isFormat1 ? 1 : 3; // runtime
            int expHighOffsetAbs = significandHighOffsetAbs + significandHighLen; // runtime
            const int expHighLen = 2;
            int formatIndicatorOffsetAbs = expHighOffsetAbs + expHighLen; // runtime
            const int formatIndicatorLen = 2;

            // hi offsets (const where possible)
            const int expLowOffsetHi = expLowOffsetAbs - 64;               // 46
            const int significandHighOffsetHi = significandHighOffsetAbs - 64; // 58
            int expHighOffsetHi = expHighOffsetAbs - 64;                   // runtime (59 or 61)
            int formatIndicatorOffsetHi = formatIndicatorOffsetAbs - 64;   // runtime

            // Masks: const when fully compile-time, runtime otherwise
            const ulong significandLowMask = (1UL << significandHighOffsetHi) - 1UL; // bits below top chunk (const)
            ulong significandHighMask = ((1UL << significandHighLen) - 1UL) << significandHighOffsetHi; // runtime
            const ulong expLowMask = ((1UL << expLowLen) - 1UL) << expLowOffsetHi; // const
            ulong expHighMask = ((1UL << expHighLen) - 1UL) << expHighOffsetHi;   // runtime
            ulong formatIndicatorMask = ((1UL << formatIndicatorLen) - 1UL) << formatIndicatorOffsetHi; // runtime

            // --- Significand high: extract already-positioned chunk (avoid >> then <<)
            ulong significandHighPre = hi & significandHighMask;

            // --- Exponent: minimize shifts
            // Extract the 14-bit exponent once
            const uint expTotalMaskRaw = (1u << (expLowLen + expHighLen)) - 1u; // 0x3FFF
            uint expRaw = biasedExponent & expTotalMaskRaw;

            // Raw masks for low/high parts (in raw bit coordinates)
            const uint expLowMaskRaw = (1u << expLowLen) - 1u;                 // 0x0FFF
            const uint expHighMaskRaw = ((1u << expHighLen) - 1u) << expLowLen; // 0x3000

            // Place low part: single left shift into final Hi64 position
            // (mask first to avoid moving high bits into low region)
            ulong expLowPre = ((ulong)(expRaw & expLowMaskRaw) << expLowOffsetHi) & expLowMask;

            // Place high part: mask the high raw bits then shift left by (expHighOffsetHi - expLowLen)
            // This avoids a right shift to normalize the high raw bits to 0..(2^expHighLen-1)-1
            int highShiftFromRawToHi = expHighOffsetHi - expLowLen;
            ulong expHighPre = ((ulong)(expRaw & expHighMaskRaw) << highShiftFromRawToHi) & expHighMask;

            // combine exponent parts
            ulong expPre = expLowPre | expHighPre;

            // Format indicator pre-shifted (value '11' in the 2-bit field) only for format1
            ulong formatIndicatorPre = isFormat1 ? ((0b11UL << formatIndicatorOffsetHi) & formatIndicatorMask) : 0UL;

            // Assemble high word: preserve low portion of hi, then OR in pre-shifted fields and sign
            ulong highWord = (hi & significandLowMask)
                             | significandHighPre
                             | expPre
                             | formatIndicatorPre
                             | sign;

            // low word is unchanged
            ulong lowWord = lo;

            bits = UInt128.FromBits(lo: unchecked((long)lowWord), hi: unchecked((long)highWord));
        }

        UInt64 HiUInt64Bits => (UInt64)(bits.GetHighPart());

        /// <summary>
        /// Extract the 14 stored exponent bits (raw, no bias).
        /// - Low 12 bits are overall 121..110 => hi64 bits 57..46 => (hi64 >> 46) & 0xFFF
        /// - Candidate top2 are hi64 bits 62..61. If candidate != 0b11 use them;
        ///   otherwise use alternate pair at hi64 bits 60..59.
        /// </summary>
        internal int RawBiasedExponent {
            get {
                // Masks
                const uint Low12Mask = 0xFFFu;   // bits 0..11
                const uint HighPartMask = 0x3000u; // bits 12..13 (already shifted)

                ulong hi64 = HiUInt64Bits;

                // low 12 exponent bits: hi64 bits 57..46 -> shift 46
                uint low12 = (uint)((hi64 >> 46) & Low12Mask);

                // candidate top2: hi64 bits 62..61
                uint candidateTop2 = (uint)((hi64 >> 61) & 0x3u);

                // Extract high part already shifted into bits 13..12:
                // - normal: hi64 bits 62..61 -> place at 13..12 by shifting right by (61 - 12) = 49
                // - alternate: hi64 bits 60..59 -> place at 13..12 by shifting right by (59 - 12) = 47
                uint highPartShifted = candidateTop2 != 0b11
                    ? (uint)((hi64 >> 49) & HighPartMask) // normal case
                    : (uint)((hi64 >> 47) & HighPartMask); // alternate case

                // Combine (no left shift needed)
                uint storedExp = highPartShifted | low12;

                return (int)storedExp;
            }
        }
        internal uint RawBiasedExponentForCanonical {
            get {
                // bits >> 110 -> equivalent to hi >> 46 when splitting into hi/lo 64-bit halves
                uint v = unchecked((uint)(bits >> 110));
                uint low12 = v & 0xFFFu;
                uint top2_at_13_12 = (v >> 3) & 0x3000u;
                return low12 | top2_at_13_12;
            }
        }
    }

    /// <summary>Defines an IEEE 754 floating-point type that is represented in a base-10 format.</summary>
    /// <typeparam name="TSelf">The type that implements the interface.</typeparam>
    public interface IDecimalFloatingPointIeee754<TSelf> : IFloatingPointIeee754<TSelf>
        where TSelf : IDecimalFloatingPointIeee754<TSelf>? {

        static int INumberBase<TSelf>.Radix {

            get => 10;
        }

        /// <summary>Computes the integer logarithm (base 10) of a value.</summary>
        /// <param name="x">The value whose integer logarithm is to be computed.</param>
        /// <returns>The integer logarithm (base 10) of <paramref name="x" />.</returns>
        public static virtual int ILog10(TSelf x) => TSelf.Radix != 10 ? throw new NotImplementedException() : TSelf.ILogB(x);

        /// <summary>Computes the product of a value and 10 raised to the specified power.</summary>
        /// <param name="x">The value which 10 raised to the power of <paramref name="n" /> multiplies.</param>
        /// <param name="n">The value to which 10 is raised before multipliying <paramref name="x" />.</param>
        /// <returns>The product of <paramref name="x" /> and 10 raised to the power of <paramref name="n" />.</returns>
        public static virtual TSelf Scale10(TSelf x, int n) => TSelf.Radix != 10 ? throw new NotImplementedException() : TSelf.Scale10(x, n);
    }

    [Experimental("UoWIP")]
    public readonly partial struct Decimal128Bid :
        ISpanFormattable,
        IComparable,
        IConvertible, IConvertibleNongenericDerived<Decimal128Bid, BigRational>, IInterfaceDerivedSelfBase<Decimal128Bid, BigRational>,
        IComparable<Decimal128Bid>,
        IEquatable<Decimal128Bid>,
        IFloatingPointIeee754<Decimal128Bid>,
        IDecimalFloatingPointIeee754<Decimal128Bid>,
        IMinMaxValue<Decimal128Bid>,
        IUtf8SpanFormattable {

        internal readonly UInt128 bits;

        internal readonly struct ConstructorInternalFromBitsTag {
        }

        internal static readonly ConstructorInternalFromBitsTag CtorFromBits;

        internal Decimal128Bid(UInt128 bits, ConstructorInternalFromBitsTag _) {
            this.bits = bits;
        }

        internal readonly struct ConstructorInternalFromPartsTag {
        }

        static readonly ConstructorInternalFromPartsTag CtorFromParts;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Decimal128Bid(UInt128 significand, uint biasedExponent, UInt64 sign, ConstructorInternalFromPartsTag _) {
            Debug.Assert(sign == 0 || sign == (1UL << 63) || (significand <= UInt64.MaxValue && biasedExponent == 0u));
            Debug.Assert(biasedExponent <= MaxBiasedExponent);
            Debug.Assert(significand <= (((UInt128)0b1010UL << 110) - 1));

            // split into halves
            var lo = unchecked((UInt64)significand.LoInt64Bits);
            var hi = unchecked((UInt64)significand.HiInt64Bits);

            // format test (absolute bit 113 -> hi bit index 49)
            const int formatTestOffsetHi = 113 - 64; // 49
            bool isFormat1 = (hi & (1UL << formatTestOffsetHi)) != 0;

            // stored field offsets (absolute) and lengths
            int expOffsetAbs = isFormat1 ? 111 : 113;
            // hi offsets (const where possible)
            int expOffsetHi = expOffsetAbs - 64;
            const int expLen = 14;

            int significandLenHi = expOffsetHi;

            //const int formatIndicatorOffsetAbs = 125;
            //const int formatIndicatorOffsetHi = formatIndicatorOffsetAbs - 64;
            //const int formatIndicatorLen = 2;
            //const ulong formatIndicatorMaskHi = ((1UL << formatIndicatorLen) - 1UL) << formatIndicatorOffsetHi;

            ulong significandMaskHi = (1UL << significandLenHi) - 1UL; // bits below top chunk (const)
            ulong expMask = ((1UL << expLen) - 1UL) << expOffsetHi;

            // --- Significand high: extract already-positioned chunk (avoid >> then <<)
            ulong significandPre = significandMaskHi & hi;

            // --- Exponent: minimize shifts
            // Include the format indicator (value '11' in the 2-bit field) only for format1
            ulong expRaw = (isFormat1 ? (0b11u << 12) : 0u) + biasedExponent;

            ulong expPre = (ulong)expRaw << expOffsetHi;

            // Assemble high word: preserve low portion of hi, then OR in pre-shifted fields and sign
            ulong highWord = significandPre | (expPre + sign);

            // low word is unchanged
            ulong lowWord = lo;

            bits = UInt128.FromBits(lo: unchecked((long)lowWord), hi: unchecked((long)highWord));
        }

        internal readonly struct ConstructorInternalFromPartsForCanonicalTag {
        }

        static readonly ConstructorInternalFromPartsForCanonicalTag CtorFromPartsCanonical;

        static int ILogBQuantum(Decimal128Bid value) {
            if (IsNaN(value)) {
                return ILogSpecialResults.ILogNaN;
            }
            if (IsInfinity(value)) {
                return ILogSpecialResults.ILogInfinity;
            }
            return unchecked((int)value.ExtractRawBiasedExponentAndRawSignificand(out _) - EXP_BIAS);
        }

        public static bool SameQuantum(Decimal128Bid first, Decimal128Bid second) {
            Debug.Assert(ILogSpecialResults.ILogNaN != ILogSpecialResults.ILogInfinity);
            return ILogBQuantum(first) == ILogBQuantum(second);
        }

        public static Decimal128Bid Quantum(Decimal128Bid value) {
            if (IsNaN(value)) {
                return value;
            }
            if (IsInfinity(value)) {
                return PositiveInfinity;
            }
            var exp = unchecked((uint)value.RawBiasedExponent);
            return new(1u, exp, 0u, CtorFromPartsCanonical);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Decimal128Bid(UInt128 significand, uint biasedExponent, UInt64 sign, ConstructorInternalFromPartsForCanonicalTag _) {
            Debug.Assert(sign == 0 || sign == (1UL << 63) || (significand <= UInt64.MaxValue && biasedExponent == 0u));
            Debug.Assert(biasedExponent <= MaxBiasedExponent);
            Debug.Assert(significand <= (((UInt128)0b1000UL << 110) - 1));

            // split into halves
            var lo = unchecked((UInt64)significand.LoInt64Bits);
            var hi = unchecked((UInt64)significand.HiInt64Bits);

            // stored field offsets (absolute) and lengths
            const int expOffsetAbs = 113;
            // hi offsets (const where possible)
            const int expOffsetHi = expOffsetAbs - 64;
            const int expLen = 14;

            const int significandLenHi = expOffsetHi;

            //const int formatIndicatorOffsetAbs = 125;
            //const int formatIndicatorOffsetHi = formatIndicatorOffsetAbs - 64;
            //const int formatIndicatorLen = 2;
            //const ulong formatIndicatorMaskHi = ((1UL << formatIndicatorLen) - 1UL) << formatIndicatorOffsetHi;

            const ulong significandMaskHi = (1UL << significandLenHi) - 1UL; // bits below top chunk (const)
            const ulong expMask = ((1UL << expLen) - 1UL) << expOffsetHi;

            // --- Significand high: extract already-positioned chunk (avoid >> then <<)
            ulong significandPre = significandMaskHi & hi;

            // --- Exponent: minimize shifts
            ulong expRaw = biasedExponent;

            ulong expPre = (ulong)expRaw << expOffsetHi;

            // Assemble high word: preserve low portion of hi, then OR in pre-shifted fields and sign
            ulong highWord = significandPre | (expPre + sign);

            // low word is unchanged
            ulong lowWord = lo;

            bits = UInt128.FromBits(lo: unchecked((long)lowWord), hi: unchecked((long)highWord));
        }

    }
}


namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static explicit operator BigRational(Decimal128Bid value) {
            if (!Decimal128Bid.IsFinite(value)) {
                ThrowNotFiniteNumberException(value);
            }
            var e = unchecked((int)value.ExtractRawBiasedExponentAndRawSignificand(out var m) - 6176);
            if (m.IsZero || m > MaxSignificandAsUInt128) {
                return default;
            }
            var q = BigIntegerSmallExp10Module.Exp10(unchecked((ushort)UltimateOrb.Mathematics.Elementary.Math.AbsAsUnsigned(e)));
            var p = (BigInteger)m;
            if (0 > value.bits.HiInt64Bits) {
                p = -p;
            }
            if (e >= 0) {
                if (e > 0) {
                    p *= q;
                }
                q = BigInteger.One;
            } else {
                var g = BigInteger.GreatestCommonDivisor(p, q);
                if (!g.IsOne) {
                    q /= g;
                    p /= g;
                }
            }
            return new BigRational(denominator: q, signedNumerator: p);
        }

        // Returns floor(log10(value)) for value > 0
        public static int ILog10(BigInteger value) {
            if (value <= 0) return int.MinValue;

            // Fast initial estimate from bit length: log10(value) ≈ (bitLength-1) * log10(2)
            var bitLen = (int)value.GetBitLength(); // .NET 7+ / .NET 10
            const double Log10Of2 = 0.3010299956639811952137388947244930267682; // high-precision constant
            var est = (int)((bitLen - 1) * Log10Of2);

            // Build 10^est and adjust up or down by at most a few steps
            BigInteger Exp10 = BigIntegerSmallExp10Module.Exp10(est);
            // If est was 0 we already have Exp10 = 1

            // If Exp10 <= value, try to increase until Exp10*10 > value
            for (; Exp10 <= value;) {
                BigInteger next = Exp10 * 10;
                if (next > value) break;
                Exp10 = next;
                est++;
            }

            // If Exp10 > value, decrease until Exp10 <= value
            for (; Exp10 > value;) {
                Exp10 /= 10;
                est--;
            }

            return est;
        }

        public static Decimal128Bid ToCohort(Decimal128Bid x, int qExponent) {
            return ToCohortInternal(x, unchecked((int)Math.Clamp(qExponent + (long)EXP_BIAS, 0, MaxBiasedExponent)));
        }

        public static Decimal128Bid ToCohortInternal(Decimal128Bid x, int preferredBiasedExponent, bool resetNaNSignAndPayload = false) {
            if (IsFinite(x)) {
                if (!IsZero(x)) {
                    return AdjustSignBitAndBiasedExponentPartial0(x, unchecked((UInt64)Int64.MinValue) & x.bits.GetHighPart(), preferredBiasedExponent);
                }
                preferredBiasedExponent = Math.Clamp(preferredBiasedExponent, 0, MaxBiasedExponent);
                ulong packedHi = (unchecked((UInt64)Int64.MinValue) & x.bits.GetHighPart()) | ((UInt64)(uint)preferredBiasedExponent << Hi64BiasedExponentShift);
                return new(new UInt128(lo: 0u, hi: packedHi), CtorFromBits);
            }
            return CanonicalizeNaNOrInfinity(x, resetNaNSignAndPayload);
        }

        public static Decimal128Bid ToFinestCohort(Decimal128Bid x) {
            return ToFinestCohort(x, false);
        }

        internal static Decimal128Bid ToFinestCohort(Decimal128Bid x, bool resetNaNSignAndPayload) {
            return ToCohortInternal(x, 0, resetNaNSignAndPayload);
        }

        private static Decimal128Bid CanonicalizeNaNOrInfinity(Decimal128Bid x, bool resetNaNSignAndPayload = false) {
            if (IsInfinity(x)) {
                return CopySign(PositiveInfinity, x);
            } else {
                Debug.Assert(IsNaN(x));
                if (!resetNaNSignAndPayload) {
                    var b = x.bits;
                    b &= new UInt128(lo: UInt64.MaxValue, hi: 0X_FE003FFF_FFFFFFFFUL);
                    return new(b, CtorFromBits);
                }
                return NegativeSignalingNaN;
            }
        }
        public static Decimal128Bid ToCoarsestCohort(Decimal128Bid x) {
            return ToCoarsestCohort(x, false);
        }

        internal static Decimal128Bid ToCoarsestCohort(Decimal128Bid x, bool resetNaNSignAndPayload) {
            return ToCohortInternal(x, MaxBiasedExponent, resetNaNSignAndPayload);
        }

        // Returns floor(log10(numerator/denominator)) for positive numerator and denominator
        static int ILog10OfFraction(BigInteger numerator, BigInteger denominator) {
            Debug.Assert(numerator > 0);
            Debug.Assert(denominator > 0);

            // Quick path when numerator == denominator
            if (numerator == denominator) return 0;

            // Use difference of floor logs as initial exponent
            int lp = ILog10(numerator);
            int lq = ILog10(denominator);
            int e = lp - lq;

            if (e >= 0) {
                BigInteger scaledQ = denominator * BigIntegerSmallExp10Module.Exp10(e);
                return (numerator >= scaledQ) ? e : e - 1;
            } else {
                BigInteger scaledP = numerator * BigIntegerSmallExp10Module.Exp10(-e);
                return (scaledP >= denominator) ? e : e - 1;
            }
        }

        static readonly BigInteger MaxSignificandAsBigInteger = BigInteger.Pow(10, 34) - 1;
        static readonly UInt128 MaxSignificandAsUInt128 = (UInt128)MaxSignificandAsBigInteger;
        static readonly UInt128 Exp10_34AsUInt128 = (UInt128)BigInteger.Pow(10, 34);
        static readonly UInt128 MaxExp10SignificandAsUInt128 = (UInt128)BigInteger.Pow(10, 33);
        static readonly UInt128 MaxSignificandOverTenAsUInt128 = MaxSignificandAsUInt128 / 10;
        static readonly BigRational boundExclusive = BigInteger.Pow(10, 6144) * (BigRational.FromFraction(1, 2) + MaxSignificandAsBigInteger);

        const int PREC = 34;
        const int G = 3; // guard digits
        const int PREC_G = PREC + G;
        const int EMIN = -6143;
        const int EMAX = 6144;
        static readonly BigInteger TEN = new BigInteger(10);

        // BID packing constants (BID-style layout used here)
        const int EXP_BITS = 14;
        const int COEFF_BITS = 113; // must hold up to 10^34-1 (~113 bits)
        const int SIGN_SHIFT = 127;
        const int EXP_SHIFT = COEFF_BITS;
        const int EXP_BIAS = 6176; // decimal128 bias

        public static explicit operator Decimal128Bid(BigRational value) {
            var s = value.Sign;
            if (s == 0) {
                return Zero;
            }
            var a = BigRational.Abs(value);
            if (a >= boundExclusive) {
                return BigRational.IsNegative(value) ? NegativeInfinity : PositiveInfinity;
            }
            // 1.                                           ×10^−6176
            // 0.000 000 000 000 000 000 000 000 000 000 001×10^−6143
            // 0 000 000 000 000 000 000 000 000 000 000 001×10^−6176
            // 0 000 000 000 000 000 000 000 000 000 000 001, 0
            // 9.999 999 999 999 999 999 999 999 999 999 999×10^6144
            // 9 999 999 999 999 999 999 999 999 999 999 999×10^6111
            // 9 999 999 999 999 999 999 999 999 999 999 999, 12287 == 0B_0010_1111_1111_1111
            var e = BigRational.Math.ILog10(a) - 33;
            if (e > 6111) {
                goto L_Inf;
            }
            if (e < -6176 - 33 - 1) {
                goto L_Zero;
            }
            e = System.Math.Max(-EXP_BIAS, e);
            if (0 <= e) {
                a /= BigIntegerSmallExp10Module.Exp10(e);
            } else {
                a *= BigIntegerSmallExp10Module.Exp10(-e);
            }
            bool exact = BigRational.IsInteger(a); // preferred exponent is 0 if true
            var t = unchecked((UInt128)BigRational.Math.RoundToBigInteger(a));
            if (Miscellaneous.Unlikely(t == Exp10_34AsUInt128)) {
                Debug.Assert(!exact);
                if (e == 6111) {
                    goto L_Inf;
                }
                t = MaxExp10SignificandAsUInt128;
                ++e;
            } else {
                Debug.Assert(t <= MaxSignificandAsUInt128);
            }
            var v = new Decimal128Bid(t, unchecked((uint)(EXP_BIAS + e)), 0 > s ? 0X8000000000000000UL : 0u, CtorFromPartsCanonical);
            return Miscellaneous.Unlikely(exact) ? AdjustExponent0Partial(v) : v;
        L_Inf:;
            return 0 > s ? NegativeInfinity : PositiveInfinity;
        L_Zero:;
            return new Decimal128Bid(0u, 0, 0 > s ? 0X8000000000000000UL : 0u, CtorFromPartsCanonical);
        }

        [DoesNotReturn]
        static void ThrowNotFiniteNumberException(Decimal128Bid value) {
            throw new NotFiniteNumberException($"The Decimal128 value {value} is not finite.");
        }

        public static explicit operator Decimal128Bid(Rational64 value) {
            return (Decimal128Bid)(BigRational)value;
        }
    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static Decimal128Bid Epsilon => new(1u, CtorFromBits);

        public static Decimal128Bid NegativeSignalingNaN => new(0u | ((UInt128)0XFE00000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid PositiveSignalingNaN => new(0u | ((UInt128)0X7E00000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid NegativeQuietNaN => new(0u | ((UInt128)0XFC00000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid PositiveQuietNaN => new(0u | ((UInt128)0X7C00000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid NegativeNaN => NegativeQuietNaN;

        public static Decimal128Bid PositiveNaN => PositiveQuietNaN;

        public static Decimal128Bid NaN => NegativeNaN;

        public static Decimal128Bid NegativeInfinity => new(0u | ((UInt128)0XF800000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid NegativeZero => new(UInt128.FromBits(lo: 0u, hi: 0X8000000000000000UL | ((UInt64)EXP_BIAS << (COEFF_BITS - 64))), CtorFromBits);

        public static Decimal128Bid PositiveInfinity => new(0u | ((UInt128)0X7800000000000000UL << 64), CtorFromBits);

        public static Decimal128Bid NegativeOne => new((UInt128.One << SIGN_SHIFT) | ((UInt128)EXP_BIAS << COEFF_BITS) | (UInt128)1u, CtorFromBits);

        public static Decimal128Bid E => new(0X4e906accb26abb56UL | ((UInt128)0X000086058a4bf4deUL << 64), -33 + EXP_BIAS, 0u, CtorFromPartsCanonical);

        public static Decimal128Bid Pi => new(0Xbabe5564e6f39f8fUL | ((UInt128)0X00009ae4795796a7UL << 64), -33 + EXP_BIAS, 0u, CtorFromPartsCanonical);

        /// <inheritdoc cref="IFloatingPointConstants{Decimal128Bid}.Tau"/>
        public static Decimal128Bid Tau => new(0X757caac9cde73f1eUL | ((UInt128)0X000135c8f2af2d4fUL << 64), -33 + EXP_BIAS, 0u, CtorFromPartsCanonical);

        public static Decimal128Bid One => new(1u | ((UInt128)EXP_BIAS << COEFF_BITS), CtorFromBits);

        static Decimal128Bid One_Finest => new(MaxExp10SignificandAsUInt128 | ((UInt128)(-33 + EXP_BIAS) << COEFF_BITS), CtorFromBits);

        public static Decimal128Bid OneHalf => new(5u | ((UInt128)(-1 + EXP_BIAS) << COEFF_BITS), CtorFromBits);

        public static int Radix => 10;

        public static Decimal128Bid Zero => new(0u | ((UInt128)EXP_BIAS << COEFF_BITS), CtorFromBits);

        public static Decimal128Bid AdditiveIdentity => new(UInt128.FromBits(lo: 0u, hi: 0X8000000000000000UL | ((UInt64)MaxBiasedExponent << (COEFF_BITS - 64))), CtorFromBits);

        public static Decimal128Bid MultiplicativeIdentity => One;

        public static Decimal128Bid MaxValue => new(MaxSignificandAsUInt128, MaxBiasedExponent, 0u, CtorFromPartsCanonical);

        public static Decimal128Bid MinValue => new(MaxSignificandAsUInt128, MaxBiasedExponent, 0X8000000000000000UL, CtorFromPartsCanonical);

        public static Decimal128Bid Abs(Decimal128Bid value) {
            return new Decimal128Bid((UInt128.MaxValue >>> 1) & value.bits, CtorFromBits);
        }

        public static Decimal128Bid CopySign(Decimal128Bid value, Decimal128Bid sign) {
            return new Decimal128Bid(((UInt128.MaxValue >>> 1) & value.bits) | (~(UInt128.MaxValue >>> 1) & sign.bits), CtorFromBits);
        }

        public static Decimal128Bid Acos(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Acosh(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid AcosPi(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Asin(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Asinh(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid AsinPi(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        // Create Decimal128Bid from a decimal literal string with round-ties-to-even
        // Only some formats are supported.
        static Decimal128Bid ParsePartialInternal(string literal) {
            if (literal == null) throw new ArgumentNullException(nameof(literal));
            literal = literal.Trim();

            // Handle sign
            bool negative = false;
            if (literal.StartsWith("+", StringComparison.Ordinal)) literal = literal.Substring(1);
            if (literal.StartsWith("-", StringComparison.Ordinal)) { negative = true; literal = literal.Substring(1); }

            // Split exponent if present
            int eIndex = literal.IndexOfAny(new[] { 'e', 'E' });
            int expFromLiteral = 0;
            string mantissa = eIndex >= 0 ? literal.Substring(0, eIndex) : literal;
            if (eIndex >= 0) {
                string expPart = literal.Substring(eIndex + 1);
                if (!int.TryParse(expPart, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out expFromLiteral))
                    throw new FormatException("Invalid exponent in literal.");
            }

            // Split integer and fractional parts
            string intPart = "0";
            string fracPart = "";
            int dot = mantissa.IndexOf('.');
            if (dot >= 0) {
                intPart = mantissa.Substring(0, dot);
                fracPart = mantissa.Substring(dot + 1);
            } else {
                intPart = mantissa;
            }

            // Normalize parts
            intPart = intPart.TrimStart('0');
            if (intPart.Length == 0) intPart = "0";
            // Remove trailing zeros in fractional part is optional; keep them to preserve exact value
            // Combine digits
            string digits = intPart + fracPart;
            if (digits.Length == 0) digits = "0";

            // If zero, return zero representation
            BigInteger bigDigits = BigInteger.Parse(digits, CultureInfo.InvariantCulture);
            if (bigDigits.IsZero) {
                // Construct zero: significand 0, stored exponent = EXP_BIAS (or appropriate zero encoding)
                return new Decimal128Bid(0UL, 0 + EXP_BIAS, 0, CtorFromPartsCanonical);
            }

            // Effective decimal exponent: value = bigDigits * 10^(expFromLiteral - fracPart.Length)
            int effectiveExp = expFromLiteral - fracPart.Length;

            // Compute decimal order d = (digitsLen - 1) + effectiveExp
            int digitsLen = digits.Length;
            int d = (digitsLen - 1) + effectiveExp;

            // Choose k so that n has 34 digits: k = d - 33
            int k = d - 33;

            // We need n = Round( value / 10^k ) = Round( bigDigits * 10^(effectiveExp - k) )
            int shift = effectiveExp - k; // may be negative
            BigInteger n; // candidate significand (may be rounded)
            if (shift >= 0) {
                // Multiply by 10^shift (no rounding needed)
                n = bigDigits * BigInteger.Pow(10, shift);
            } else {
                // Divide by 10^{-shift} with round-ties-to-even
                BigInteger denom = BigInteger.Pow(10, -shift);
                BigInteger quotient = BigInteger.DivRem(bigDigits, denom, out BigInteger rem);

                // Compare rem*2 with denom
                BigInteger twiceRem = rem << 1; // rem * 2
                int cmp = twiceRem.CompareTo(denom);
                if (cmp > 0) {
                    // rem*2 > denom -> round up
                    n = quotient + BigInteger.One;
                } else if (cmp < 0) {
                    // rem*2 < denom -> round down (keep quotient)
                    n = quotient;
                } else {
                    // tie: rem*2 == denom -> round to even
                    // if quotient is odd -> round up; else keep quotient
                    if ((quotient & BigInteger.One) != BigInteger.Zero)
                        n = quotient + BigInteger.One;
                    else
                        n = quotient;
                }
            }

            // After rounding, n should be close to 10^34 range; adjust if necessary
            BigInteger ten34 = BigInteger.Pow(10, 34);
            BigInteger ten33 = BigInteger.Pow(10, 33);

            if (n >= ten34) {
                // e.g., rounding produced 10^34 -> shift right and increment k
                n /= 10;
                k += 1;
            }
            while (n < ten33) {
                // If too small (shouldn't usually happen), scale up and decrement k
                n *= 10;
                k -= 1;
            }

            // Convert n to decimal string (34 digits)
            string significandStr = n.ToString(CultureInfo.InvariantCulture);
            Debug.Assert(significandStr.Length >= 33 && significandStr.Length <= 34);

            // Build UInt128 from the decimal string. If your environment lacks UInt128.Parse,
            // replace this with your own conversion from BigInteger to UInt128.
            System.UInt128 u = System.UInt128.Parse(significandStr, NumberStyles.None, CultureInfo.InvariantCulture);

            int storedExp = k + EXP_BIAS;

            // Construct Decimal128Bid from parts
            Decimal128Bid result = new Decimal128Bid(u, (uint)storedExp, 0, CtorFromPartsCanonical);

            // Apply sign if needed (assumes Decimal128Bid supports unary negation)
            if (negative) {
                result = -result;
            }

            return result;
        }
        // Numerator coefficients:
        static readonly Decimal128Bid AtanInternal_A3_RI8_P8 = new(UInt128.FromLoHi(0X6f6666f5d0275cccUL, 0X2ff8c444e744e16cUL), CtorFromBits); // 0.003980811371675454576777904129531084
        static readonly Decimal128Bid AtanInternal_A3_RI8_P7 = new(UInt128.FromLoHi(0X2ac0f261c592be41UL, 0X2ff642428239183bUL), CtorFromBits); // 1.343908394641428421372356875959873E-4
        static readonly Decimal128Bid AtanInternal_A3_RI8_P6 = new(UInt128.FromLoHi(0X0825fb7876b24adeUL, 0X2ffc4ecac49ed6c5UL), CtorFromBits); // 0.1598092888999137195075107244100318
        static readonly Decimal128Bid AtanInternal_A3_RI8_P5 = new(UInt128.FromLoHi(0X2542ea73a51d70eaUL, 0X2ff8992f3715f016UL), CtorFromBits); // 0.003106949441193634853841265284182250
        static readonly Decimal128Bid AtanInternal_A3_RI8_P4 = new(UInt128.FromLoHi(0X238056b1c37b023bUL, 0X2ffdd69aaf45d619UL), CtorFromBits); // 0.9544987895047132250116910413972027
        static readonly Decimal128Bid AtanInternal_A3_RI8_P3 = new(UInt128.FromLoHi(0Xfc901325284ec802UL, 0X2ffa317f1d0c7abaUL), CtorFromBits); // 0.01003909037370216080405289717647362
        static readonly Decimal128Bid AtanInternal_A3_RI8_P2 = new(UInt128.FromLoHi(0X33882eb0f619c328UL, 0X2ffe5721fe75176eUL), CtorFromBits); // 1.767262915628128850806407582827304
        static readonly Decimal128Bid AtanInternal_A3_RI8_P1 = new(UInt128.FromLoHi(0X17699a92e44626ceUL, 0X2ff9817325ba85ceUL), CtorFromBits); // 0.007817850612532475808681856705373902
        static readonly Decimal128Bid AtanInternal_A3_RI8_P0 = new(UInt128.FromLoHi(0X38c15b0a00000001UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000000001

        // Denominator coefficients:
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q8 = new(UInt128.FromLoHi(0Xfe662d03b8bcdd68UL, 0X2ffa78c82e6354dbUL), CtorFromBits); // 0.02449749141335898195015848435703144
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q7 = new(UInt128.FromLoHi(0X707e5c70f7a357ffUL, 0X2ff73c5fc1bf7882UL), CtorFromBits); // 6.416828072273600192723113574881279E-4
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q6 = new(UInt128.FromLoHi(0Xef6e15f129a99d80UL, 0X2ffcb52a40496240UL), CtorFromBits); // 0.3674463616842754606559000143699328
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q5 = new(UInt128.FromLoHi(0X5b8e8dee80dff48bUL, 0X2ff91be922c88e5cUL), CtorFromBits); // 0.005758392844647023907683369985504395
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q4 = new(UInt128.FromLoHi(0Xfbc57c67a5699d67UL, 0X2ffe47b8dd418541UL), CtorFromBits); // 1.454697539158533953058463925509479
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q3 = new(UInt128.FromLoHi(0X63be42e7f9b0c201UL, 0X2ffa3e5848f918caUL), CtorFromBits); // 0.01264504057787965274027954371871233
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q2 = new(UInt128.FromLoHi(0Xf11df7b44b7a764bUL, 0X2ffe6791408bf149UL), CtorFromBits); // 2.100596248961462184139740916905547
        static readonly Decimal128Bid AtanInternal_A3_RI8_Q1 = new(UInt128.FromLoHi(0X17699a92e44db57dUL, 0X2ff9817325ba85ceUL), CtorFromBits); // 0.007817850612532475808681856705869181
                                                                                                                                                // static readonly Decimal128Bid AtanInternal_A3_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        // Numerator coefficients:
        static readonly Decimal128Bid AtanInternal_A2_RI8_P8 = new(UInt128.FromLoHi(0X46f91a626d195dfdUL, 0X2ff4519cef3fb416UL), CtorFromBits); // 1.655308815178121387631403083587069E-5
        static readonly Decimal128Bid AtanInternal_A2_RI8_P7 = new(UInt128.FromLoHi(0Xe50970f37164db5bUL, 0X2ff8743176ddffb1UL), CtorFromBits); // 0.002356678481598042805950428414794587
        static readonly Decimal128Bid AtanInternal_A2_RI8_P6 = new(UInt128.FromLoHi(0X017f034a6e3a3d2cUL, 0X2ffb0c21c09a5512UL), CtorFromBits); // 0.05438359910839813615910319264775468
        static readonly Decimal128Bid AtanInternal_A2_RI8_P5 = new(UInt128.FromLoHi(0Xfea062cadd78d8ffUL, 0X2ffce970a7bc5c3dUL), CtorFromBits); // 0.4734726903562732382352957343586559
        static readonly Decimal128Bid AtanInternal_A2_RI8_P4 = new(UInt128.FromLoHi(0X049334d8bac3c2f9UL, 0X2ffe6303f22e7557UL), CtorFromBits); // 2.008271186786144707658596342809337
        static readonly Decimal128Bid AtanInternal_A2_RI8_P3 = new(UInt128.FromLoHi(0X9722050071de2499UL, 0X2ffee28fc989eb3dUL), CtorFromBits); // 4.595216570885520958786177502946457
        static readonly Decimal128Bid AtanInternal_A2_RI8_P2 = new(UInt128.FromLoHi(0Xa65116dc468c9e96UL, 0X2fff1d9632fd0828UL), CtorFromBits); // 5.792386741565117937229739934391958
        static readonly Decimal128Bid AtanInternal_A2_RI8_P1 = new(UInt128.FromLoHi(0Xacadeb9b9079b611UL, 0X2ffeba8e792a58f5UL), CtorFromBits); // 3.783816084237409187011199506167313
                                                                                                                                                // static readonly Decimal128Bid AtanInternal_A2_RI8_P0 = new (UInt128.FromLoHi(0X38c15b0a00000000UL, 0X2ffe314dc6448d93UL), CtorFromBits); // 1.000000000000000000000000000000000

        // Denominator coefficients:
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q8 = new(UInt128.FromLoHi(0X898aa42731c10886UL, 0X2ff65b2135af5345UL), CtorFromBits); // 1.848330417956050849902219543775366E-4
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q7 = new(UInt128.FromLoHi(0X82fb5ad01953d394UL, 0X2ff9cf2b378ad346UL), CtorFromBits); // 0.009394179646983860091809654426030996
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q6 = new(UInt128.FromLoHi(0X05f610e46eff214dUL, 0X2ffc44421da4a0efUL), CtorFromBits); // 0.1384442085863362109557912236925261
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q5 = new(UInt128.FromLoHi(0X6271a879eb01bcc1UL, 0X2ffdc17e1e3fc315UL), CtorFromBits); // 0.9116794022150274542384060504849601
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q4 = new(UInt128.FromLoHi(0X5f58fabf81bac380UL, 0X2ffe9c5918dc8407UL), CtorFromBits); // 3.171114898860834513145905553261440
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q3 = new(UInt128.FromLoHi(0Xdf64a53ae1dbbac8UL, 0X2fff33785626d558UL), CtorFromBits); // 6.236233790480303793197211704867528
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q2 = new(UInt128.FromLoHi(0Xbe631eb9e87c4d0eUL, 0X2fff5763c138410cUL), CtorFromBits); // 6.964769880755365444011250880892174
        static readonly Decimal128Bid AtanInternal_A2_RI8_Q1 = new(UInt128.FromLoHi(0X6a43b49ee5cf0b67UL, 0X2ffecafdbb4132d1UL), CtorFromBits); // 4.117149417570742520344532839500647
                                                                                                                                                // static readonly Decimal128Bid AtanInternal_A2_RI8_Q0 = new (UInt128.FromLoHi(0X0000000000000001UL, 0X3040000000000000UL), CtorFromBits); // 1

        static Decimal128Bid AtanInternal_A2_RI8(Decimal128Bid x) {
            Debug.Assert(0 <= x && x < 0.0875 * 0.0875);

            // Horner for numerator
            Decimal128Bid px = AtanInternal_A2_RI8_P8;
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P7);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P6);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P5);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P4);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P3);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P2);
            px = FusedMultiplyAdd(px, x, AtanInternal_A2_RI8_P1);
            px = FusedMultiplyAdd(px, x, One);

            // Horner for denominator
            Decimal128Bid qx = AtanInternal_A2_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A2_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid AtanInternal_A3_RI8(Decimal128Bid x) {
            Debug.Assert(0 <= x && x < 0.0875);

            // Horner for numerator
            Decimal128Bid px = AtanInternal_A3_RI8_P8;
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P7);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P6);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P5);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P4);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P3);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P2);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P1);
            px = FusedMultiplyAdd(px, x, AtanInternal_A3_RI8_P0);

            // Horner for denominator
            Decimal128Bid qx = AtanInternal_A3_RI8_Q8;
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q7);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q6);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q5);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q4);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q3);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q2);
            qx = FusedMultiplyAdd(qx, x, AtanInternal_A3_RI8_Q1);
            qx = FusedMultiplyAdd(qx, x, One);

            return px / qx;
        }

        static Decimal128Bid AtanInternal4(Decimal128Bid x) {
            Debug.Assert(0 <= x && x < 0.0875);
            return x * AtanInternal_A2_RI8(x * x);
            // return x * AtanInternal_A3_RI8(x);
        }

        static readonly Decimal128Bid Tan_PiOver36 = new Decimal128Bid(
            System.UInt128.Parse("8748866352592400522201866943496146"), -35 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static readonly Decimal128Bid Tan_PiOver18 = new Decimal128Bid(
            System.UInt128.Parse("1763269807084649734710903868686190"), -34 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static readonly Decimal128Bid PiOver18 = new Decimal128Bid(
            System.UInt128.Parse("1745329251994329576923690768488613"), -34 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static Decimal128Bid AtanInternal3(Decimal128Bid x) {
            if (x > Tan_PiOver36) {
                var d = x - Tan_PiOver18;
                return PiOver18 + CopySign(AtanInternal4(Abs(d) / (One + Tan_PiOver18 * x)), d);
            }
            return AtanInternal4(x);
        }

        // Tan[Pi/12]
        static readonly Decimal128Bid Plus2MinusSqrt3 = new Decimal128Bid(
            System.UInt128.Parse("2679491924311227064725536584941276"), -34 + EXP_BIAS, 0, CtorFromPartsCanonical);

        // Tan[Pi/6]
        static readonly Decimal128Bid Sqrt3Reciprocal = new Decimal128Bid(
            System.UInt128.Parse("5773502691896257645091487805019575"), -34 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static readonly Decimal128Bid PiOver6 = new Decimal128Bid(
            System.UInt128.Parse("5235987755982988730771072305465838"), -34 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static Decimal128Bid AtanInternal2(Decimal128Bid x) {
            if (x > Plus2MinusSqrt3) {
                var d = x - Sqrt3Reciprocal;
                return PiOver6 + CopySign(AtanInternal3(Abs(d) / (One + Sqrt3Reciprocal * x)), d);
            }
            return AtanInternal3(x);
        }

        static readonly Decimal128Bid PiOver2 = new Decimal128Bid(
            System.UInt128.Parse("1570796326794896619231321691639751"), -33 + EXP_BIAS, 0, CtorFromPartsCanonical);

        static Decimal128Bid AtanInternal1(Decimal128Bid x) {
            if (x > One) {
                if (IsPositiveInfinity(x)) {
                    return PiOver2;
                }
                return PiOver2 - AtanInternal2(One / x);
            }
            return AtanInternal2(x);
        }

        public static Decimal128Bid Atan(Decimal128Bid x) {
            if (IsNaN(x)) {
                return x;
            }
            return CopySign(AtanInternal1(Abs(x)), x);
        }

        public static Decimal128Bid Atan2(Decimal128Bid y, Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Atan2Pi(Decimal128Bid y, Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Atanh(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        static readonly Decimal128Bid Pi_A1 = new Decimal128Bid(
           System.UInt128.Parse("3141592653589793238462643383279502"), -33 + EXP_BIAS, 0, CtorFromPartsCanonical);

        public static Decimal128Bid AtanPi(Decimal128Bid x) {
            if (IsNaN(x)) {
                return x;
            }
            return CopySign(AtanInternal1(Abs(x)), x) / Pi_A1;
        }

        public static Decimal128Bid BitDecrement(Decimal128Bid x) {

            if (!IsFinite(x)) {
                // NaN returns NaN
                // -Infinity returns -Infinity
                // +Infinity returns MaxValue
                return IsPositiveInfinity(x) ? MaxValue : IsNegativeInfinity(x) ? NegativeInfinity : x;
            }

            if (IsZero(x)) {
                return -Epsilon;
            }
            var sign = ~(UInt64.MaxValue >>> 1) & x.bits.GetHighPart();
            x = AdjustSignBitAndBiasedExponentPartial(x, sign, 0);
            var exp = x.ExtractRawBiasedExponentAndRawSignificand(out var significand);

            // Negative values need to be incremented
            // Positive values need to be decremented
            if (IsNegative(x)) {
                if (significand == MaxSignificandAsUInt128) {
                    if (exp == 0B_0010_1111_1111_1111) {
                        return NegativeInfinity;
                    }
                    significand = 0;
                    ++exp;
                } else {
                    ++significand;
                }
            } else {
                --significand;
                if (significand.IsZero) {
                    exp = 0;
                }
            }

            return new(significand, exp, sign, CtorFromParts);
        }

        public static Decimal128Bid BitIncrement(Decimal128Bid x) {

            if (!IsFinite(x)) {
                // NaN returns NaN
                // -Infinity returns MinValue
                // +Infinity returns +Infinity
                return IsNegativeInfinity(x) ? MaxValue : IsPositiveInfinity(x) ? NegativeInfinity : x;
            }

            if (IsZero(x)) {
                return Epsilon;
            }
            var sign = ~(UInt64.MaxValue >>> 1) & x.bits.GetHighPart();
            x = AdjustSignBitAndBiasedExponentPartial(x, sign, 0);
            var exp = x.ExtractRawBiasedExponentAndRawSignificand(out var significand);

            // Negative values need to be decremented
            // Positive values need to be incremented
            if (!IsNegative(x)) {
                if (significand == MaxSignificandAsUInt128) {
                    if (exp == 0B_0010_1111_1111_1111) {
                        return PositiveInfinity;
                    }
                    significand = 0;
                    ++exp;
                } else {
                    ++significand;
                }
            } else {
                --significand;
                if (significand.IsZero) {
                    exp = 0;
                }
            }

            return new(significand, exp, sign, CtorFromParts);
        }

        public static Decimal128Bid Cbrt(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Cos(Decimal128Bid x) {
            return SinCos(x).Cos;
        }

        public static Decimal128Bid Cosh(Decimal128Bid x) {
            return Hypot(One, Sinh(x));
        }

        public static Decimal128Bid CosPi(Decimal128Bid x) {
            return SinCosPi(x).CosPi;
        }

        public static Decimal128Bid FusedMultiplyAdd(Decimal128Bid left, Decimal128Bid right, Decimal128Bid addend) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                if (Decimal128Bid.IsNaN(left)) {
                    return left + addend;
                }
                if (Decimal128Bid.IsNaN(right)) {
                    return right + addend;
                }
                if (Decimal128Bid.IsZero(left) || Decimal128Bid.IsZero(right)) {
                    Debug.Assert(Decimal128Bid.IsInfinity(left) || Decimal128Bid.IsInfinity(right));
                    return new Decimal128Bid(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits) + addend;
                }
                Debug.Assert(Decimal128Bid.IsInfinity(left) && Decimal128Bid.IsInfinity(right));
                return new Decimal128Bid(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits) + addend;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            if (!Decimal128Bid.IsFinite(addend)) {
                if (Decimal128Bid.IsNaN(addend)) {
                    return addend;
                }
                return CopySign(PositiveInfinity, addend);
            }
            Debug.Assert(Decimal128Bid.IsFinite(addend));
            var preferredBiasedExponent = Math.Clamp(left.RawBiasedExponent + right.RawBiasedExponent - EXP_BIAS, 0, MaxBiasedExponent);
            preferredBiasedExponent = Math.Min(preferredBiasedExponent, addend.RawBiasedExponent);
            var r = (BigRational)left * (BigRational)right + (BigRational)addend;
            var result = (Decimal128Bid)r;
            return AdjustSignBitAndBiasedExponent(result, BigRational.IsNegative(r) ? 0X8000000000000000UL : 0u, preferredBiasedExponent);
        }

        public static Decimal128Bid Hypot(Decimal128Bid x, Decimal128Bid y) {
            if (IsInfinity(x) || IsInfinity(y)) {
                return PositiveInfinity;
            }
            if (IsNaN(x)) {
                return x;
            }
            if (IsNaN(y)) {
                return y;
            }
            Debug.Assert(IsFinite(x));
            Debug.Assert(IsFinite(y));
            var expX = unchecked((int)x.ExtractRawBiasedExponentAndRawSignificand(out var significandX));
            var expY = unchecked((int)y.ExtractRawBiasedExponentAndRawSignificand(out var significandY));
            var q = Math.Min(expX, expY);
            var a = (BigInteger)significandX;
            a *= a;
            if (expX - q > 0) {
                a *= BigIntegerSmallExp10Module.Exp10((expX - q) << 1);
            }
            var b = (BigInteger)significandY;
            b *= b;
            if (expY - q > 0) {
                b *= BigIntegerSmallExp10Module.Exp10((expY - q) << 1);
            }
            var c = a + b;
            var s = c << 2 * 114;
            var d = GenericMath.ISqrt(s);
            var e = s - d * d;
            if (e > d) {
                d |= 1;
            }
            BigRational v = BigRational.FromFraction(d, (UInt128)1 << 114);
            if (q >= EXP_BIAS) {
                v *= BigIntegerSmallExp10Module.Exp10(q - EXP_BIAS);
            } else {
                v /= BigIntegerSmallExp10Module.Exp10(EXP_BIAS - q);
            }
            var result = (Decimal128Bid)v;
            return AdjustSignBitAndBiasedExponentPartial(result, 0u, q);
        }

        public static Decimal128Bid Ieee754Remainder(Decimal128Bid left, Decimal128Bid right) {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDecimalFloatingPointIeee754{Decimal128Bid}.ILog10(Decimal128Bid)"/>
        public static int ILog10(Decimal128Bid x) {
            if (IsNegative(x)) {
                return ILogSpecialResults.ILogNaN;
            }
            var exp = unchecked((int)x.ExtractRawBiasedExponentAndRawSignificand(out var m));
            if (m > MaxSignificandAsUInt128) {
                m = 0;
            }
            if (m.IsZero) {
                return ILogSpecialResults.ILog0;
            }
            return exp - EXP_BIAS + UInt128Exp10Module.ILog10(m);
        }

        /// <inheritdoc cref="IFloatingPointIeee754{Decimal128Bid}.ILogB(Decimal128Bid)"/>
        public static int ILogB(Decimal128Bid x) {
            return ILog10(x);
        }

        /// <inheritdoc cref="INumberBase{Decimal128Bid}.IsCanonical(Decimal128Bid)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsCanonical(Decimal128Bid value) {
            var x = Abs(value);
            if (IsFinite(x)) {
                x.ExtractRawBiasedExponentAndRawSignificand(out var significand);
                return significand <= MaxSignificandAsUInt128;
            }
            {
                var t = (uint)(x.bits >>> 110);
                if (t != (uint)(PositiveSignalingNaN.bits >>> 110) &&
                    t != (uint)(PositiveQuietNaN.bits >>> 110) &&
                    t != (uint)(PositiveInfinity.bits >>> 110)) {
                    return false;
                }
                if (t == (uint)(PositiveInfinity.bits >>> 110)) {
                    return x.bits == PositiveInfinity.bits;
                } else {
                    return true;
                }
            }
        }

        public static bool IsComplexNumber(Decimal128Bid value) {
            return false;
        }

        public static bool IsEvenInteger(Decimal128Bid value) {
            return 0 == value % 2;
        }

        public static bool IsFinite(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111100_00000000_00000000_00000000;
            hi &= 0B_01111100_00000000_00000000_00000000;
            hi &= 0B_01111000_00000000_00000000_00000000;
            return 0B_01111000_00000000_00000000_00000000 != hi;
        }

        public static bool IsImaginaryNumber(Decimal128Bid value) {
            return false;
        }

        public static bool IsInfinity(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111110_00000000_00000000_00000000;
            hi &= 0B_11111100_00000000_00000000_00000000;
            hi &= 0B_01111100_00000000_00000000_00000000;
            return 0B_01111000_00000000_00000000_00000000 == hi;
        }

        public static bool IsInteger(Decimal128Bid value) {
            return IsFinite(value) && BigRational.IsInteger((BigRational)value);
        }

        public static bool IsNaN(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111110_00000000_00000000_00000000;
            hi &= 0B_11111100_00000000_00000000_00000000;
            hi &= 0B_01111100_00000000_00000000_00000000;
            return 0B_01111100_00000000_00000000_00000000 == hi;
        }

        public static bool IsSignalingNaN(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111110_00000000_00000000_00000000;
            hi &= 0B_01111110_00000000_00000000_00000000;
            return 0B_01111110_00000000_00000000_00000000 == hi;
        }

        public static bool IsQuietNaN(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111110_00000000_00000000_00000000;
            hi &= 0B_01111110_00000000_00000000_00000000;
            return 0B_01111100_00000000_00000000_00000000 == hi;
        }

        public static bool IsNegative(Decimal128Bid value) {
            return 0 > value.bits.HiInt64Bits;
        }

        public static bool IsNegativeInfinity(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111100_00000000_00000000_00000000;
            return 0B_11111000_00000000_00000000_00000000 == hi;
        }

        public static bool IsNormal(Decimal128Bid value) {
            throw new NotImplementedException();
        }

        public static bool IsOddInteger(Decimal128Bid value) {
            return One == Abs(value % 2);
        }

        public static bool IsPositive(Decimal128Bid value) {
            return 0 <= value.bits.HiInt64Bits;
        }

        public static bool IsPositiveInfinity(Decimal128Bid value) {
            var hi = unchecked((UInt32)(value.HiUInt64Bits >> 32));
            hi &= 0B_11111100_00000000_00000000_00000000;
            return 0B_01111000_00000000_00000000_00000000 == hi;
        }

        public static bool IsRealNumber(Decimal128Bid value) {
            return !IsNaN(value);
        }

        /*
        subnormal max value
        9.99999999999999999999999999999999E-6144
        0X0000314dc6448d9338c15b09ffffffff

        subnormal with max bits (BID)
        9E-6144
        0X00400000000000000000000000000009 
        */
        static UInt128 SubnormalMaxBitsAsUInt128 => UInt128.FromBits(9, hi: 0X0040000000000000UL);

        static Decimal128Bid SubnormalMaxValue => new(UInt128.FromBits(0X38c15b09ffffffffUL, hi: 0X0000314dc6448d93UL), CtorFromBits);

        public static bool IsSubnormal(Decimal128Bid value) {
            if (IsFinite(value)) {
                if (IsZero(value)) {
                    return false;
                }
                var a = Abs(value);
                if (a.bits <= SubnormalMaxBitsAsUInt128 && a <= SubnormalMaxValue) {
                    return true;
                }
            }
            return false;
        }

        public static bool IsZero(Decimal128Bid value) {
            var e = value.ExtractRawBiasedExponentAndRawSignificand(out var m);
            return e <= MaxBiasedExponent && (m.IsZero || m > MaxSignificandAsUInt128);
        }

        public static Decimal128Bid Log(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Log(Decimal128Bid x, Decimal128Bid newBase) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Log10(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Log2(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid MaxMagnitude(Decimal128Bid x, Decimal128Bid y) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid MaxMagnitudeNumber(Decimal128Bid x, Decimal128Bid y) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid MinMagnitude(Decimal128Bid x, Decimal128Bid y) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid MinMagnitudeNumber(Decimal128Bid x, Decimal128Bid y) {
            throw new NotImplementedException();
        }
        static readonly BigInteger MaxNaNPayloadAsBigInteger = (BigInteger.One << 110) - 1;

        public static Decimal128Bid Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider) {
            var parseResult = NumberLiteralParseModule.ParseNumberLiteral(s.ToString());
            var kind = parseResult.Flags.GetKind();
            if (kind == NumberLiteralFlags.Empty || kind == NumberLiteralFlags.Error) {
                return decimal.Parse(s, provider);
                throw new FormatException("Input string was not in a recognized format.");
            }
            if (kind != NumberLiteralFlags.IsFinite) {
                var sp = parseResult.Flags.GetSpecial();
                if (sp == NumberLiteralFlags.SpecialInfinity) {
                    return parseResult.Flags.HasFlag(NumberLiteralFlags.IsNegative) ? NegativeInfinity : PositiveInfinity;
                }
                var payload = (UltimateOrb.UInt128)System.UInt128.CreateTruncating(MaxNaNPayloadAsBigInteger & parseResult.SignificandIntegralPart);
                // traat unspecified NaN as SignalingNaN
                var ccc = sp == NumberLiteralFlags.SpecialQuietNaN ? 0B_011_1110_0000_0000 : 0B_011_1111_0000_0000;
                ccc += parseResult.Flags.GetSign() == NumberLiteralFlags.SignPositive ? 0 : 0B_100_0000_0000_0000;
                return new Decimal128Bid(payload + ((UInt128)ccc << 113), CtorFromBits);
            }
            {
                BigRational r = 0;
                var preferredBiasedExponent = checked((int)(parseResult.Exponent - parseResult.SignificandFractionalPartLength + EXP_BIAS));
                var preferredSignBit = parseResult.Flags.HasFlag(NumberLiteralFlags.IsNegative) ? unchecked((UInt64)Int64.MinValue) : 0;
                if (parseResult.SignificandFractionalPart.IsZero &&
                    parseResult.SignificandIntegralPart.IsZero) {
                } else {
                    BigInteger denominator;
                    BigInteger numerator;
                    if (parseResult.Flags.HasFlag(NumberLiteralFlags.Hex)) {
                        Debug.Assert(parseResult.SignificandFractionalPartLength >= 0);
                        int fracLen = (int)parseResult.SignificandFractionalPartLength;
                        BigInteger pow16 = BigInteger.Pow(16, fracLen);
                        numerator = parseResult.SignificandIntegralPart * pow16 + parseResult.SignificandFractionalPart;
                        denominator = pow16;

                        var exp = checked((int)parseResult.Exponent);
                        if (exp >= 0) {
                            numerator <<= exp;
                        } else {
                            denominator <<= checked(-exp);
                        }


                    } else {
                        // Decimal path
                        int fracLen = checked((int)parseResult.SignificandFractionalPartLength);
                        BigInteger pow10 = BigInteger.Pow(10, fracLen);
                        numerator = parseResult.SignificandIntegralPart * pow10 + parseResult.SignificandFractionalPart;
                        denominator = pow10;

                        var exp = checked((int)parseResult.Exponent);
                        if (exp >= 0) {
                            numerator *= BigIntegerSmallExp10Module.Exp10(exp);
                        } else {
                            denominator *= BigIntegerSmallExp10Module.Exp10(-exp);
                        }
                    }
                    r = BigRational.FromFraction(numerator, denominator);
                    if (parseResult.Flags.HasFlag(NumberLiteralFlags.IsNegative)) r = -r;
                }
                var res = (Decimal128Bid)r;
                preferredBiasedExponent = Math.Clamp(preferredBiasedExponent, 0, MaxBiasedExponent);
                return AdjustSignBitAndBiasedExponent(res, preferredSignBit, preferredBiasedExponent);
            }
        }

        public static Decimal128Bid Parse(string s, NumberStyles style, IFormatProvider? provider) {
            return Parse(s.AsSpan(), style, provider);
        }

        public static Decimal128Bid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) {
            return Parse(s, NumberStyles.Float, provider);
        }

        public static Decimal128Bid Parse(string s, IFormatProvider? provider) {
            return Parse(s.AsSpan(), NumberStyles.Float, provider);
        }

        public static Decimal128Bid Parse(string s) {
            return Parse(s, null);
        }

        public static Decimal128Bid Pow(Decimal128Bid x, Decimal128Bid y) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid RootN(Decimal128Bid x, int n) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Round(Decimal128Bid x, int digits, MidpointRounding mode = MidpointRounding.ToEven) {
            if (!IsFinite(x)) {
                return x * MultiplicativeIdentity; // Conan
            }
            var preferredBiasedExponent = checked((int)Math.Clamp((long)EXP_BIAS - digits, 0, MaxBiasedExponent));
            var r = BigRational.Math.Round((BigRational)x, digits, mode);
            return AdjustSignBitAndBiasedExponentPartial((Decimal128Bid)r, IsNegative(x) ? 0X8000000000000000UL : 0u, preferredBiasedExponent);
        }

        public static Decimal128Bid Scale10(Decimal128Bid x, int n) {
            if (!IsFinite(x)) {
                return x;
            }
            if (n >= short.MaxValue) {
                if (!IsZero(x)) {
                    return new(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & x.bits), CtorFromBits);
                }
                return new(((UInt128.MaxValue >>> 1) & AdditiveIdentity.bits) | (~(UInt128.MaxValue >>> 1) & x.bits), CtorFromBits);
            }
            if (n <= short.MinValue) {
                return new(((UInt128.MaxValue >>> 1) & AdditiveIdentity.bits) | (~(UInt128.MaxValue >>> 1) & x.bits), CtorFromBits);
            }
            if (n == 0) {
                return MultiplicativeIdentity * x;
            }
            var ee = x.RawBiasedExponent + n;
            var preferredBiasedExponent = Math.Clamp(ee, 0, MaxBiasedExponent);
            var y = (BigRational)x;
            var p = BigIntegerSmallExp10Module.Exp10(Math.Abs(n));
            Debug.Assert(p >= 10);
            if (n > 0) {
                y *= p;
            } else {
                y /= p;
            }
            return AdjustSignBitAndBiasedExponentPartial((Decimal128Bid)y, IsNegative(x) ? 0X8000000000000000UL : 0u, preferredBiasedExponent);
        }

        public static Decimal128Bid ScaleB(Decimal128Bid x, int n) {
            return Scale10(x, n);
        }

        public static Decimal128Bid Sin(Decimal128Bid x) {
            return SinCos(x).Sin;
        }

        public static (Decimal128Bid Sin, Decimal128Bid Cos) SinCos(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static (Decimal128Bid SinPi, Decimal128Bid CosPi) SinCosPi(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Sinh(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid SinPi(Decimal128Bid x) {
            return SinCosPi(x).SinPi;
        }

        public static Decimal128Bid Sqrt(Decimal128Bid x) {
            if (IsNaN(x)) {
                return x;
            }
            if (IsPositiveInfinity(x)) {
                return PositiveInfinity;
            }
            var ss = IsNegative(x);
            if (!IsZero(x) && ss) {
                return NaN;
            }
            Debug.Assert(IsFinite(x));
            var exp = unchecked((int)x.ExtractRawBiasedExponentAndRawSignificand(out var significand));
            if (int.IsOddInteger(exp)) {
                significand *= 10;
            }
            var a = (BigInteger)significand;
            
            var s = a << (2 * 114);
            var d = GenericMath.ISqrt(s);
            var e = s - d * d;
            if (e > d) {
                d |= 1;
            }
            BigRational v = BigRational.FromFraction(d, (UInt128)1 << 114);
            if (exp >= EXP_BIAS) {
                v *= BigIntegerSmallExp10Module.Exp10((exp - EXP_BIAS) >> 1);
            } else {
                v /= BigIntegerSmallExp10Module.Exp10((EXP_BIAS + 1 - exp) >> 1);
            }
            var result = (Decimal128Bid)v;
            var preferredBiasedExponent = EXP_BIAS + ((exp - EXP_BIAS) >> 1);
            return AdjustSignBitAndBiasedExponentPartial(result, ss ? 0X8000000000000000UL : 0u, preferredBiasedExponent);
        }

        public static Decimal128Bid Tan(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid Tanh(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static Decimal128Bid TanPi(Decimal128Bid x) {
            throw new NotImplementedException();
        }

        public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out Decimal128Bid result) where TOther : INumberBase<TOther> {
            return INumberBaseFriendInternal<Decimal128Bid>.TryConvertFromTruncating(value, out result);
        }

        public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out Decimal128Bid result) where TOther : INumberBase<TOther> {
            return INumberBaseFriendInternal<Decimal128Bid>.TryConvertFromTruncating(value, out result);
        }

        const UInt64 SignBitUInt64 = unchecked((UInt64)UInt64.MinValue);

        internal static Decimal128Bid FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>(TFloat value)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {
            if (TFloat.IsFinite(value)) {
                var r = (Decimal128Bid)BigRational.FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>(value);
                var c = value.GetExponentByteCount();
                if (c > sizeof(UInt64)) {
                    throw new NotImplementedException();
                }
                Span<byte> exp = stackalloc byte[sizeof(Int64)];
                if (!value.TryWriteExponentLittleEndian(exp, out var cc)) {
                    throw new InvalidOperationException();
                }
                Debug.Assert(0 < cc);
                Debug.Assert(cc <= exp.Length);
                var z = 8 * (exp.Length - cc);
                var e = BinaryPrimitives.ReadInt64LittleEndian(exp);
                e <<= z;
                e >>= z;
                e += EXP_BIAS;
                var e1 = unchecked((int)Math.Clamp(e, 0, MaxBiasedExponent));
                return AdjustSignBitAndBiasedExponent(r, TFloat.IsNegative(value) ? SignBitUInt64 : 0u, e1);
            }
            if (TFloat.IsInfinity(value)) {
                return TFloat.IsNegative(value) ? NegativeInfinity : PositiveInfinity;
            }
            Debug.Assert(TFloat.IsNaN(value));
            {
                var BitSize = 8 * Unsafe.SizeOf<TFloat>();
                //  k – round(4 × log2 (k)) + 13
                var Precision = BitSize - (int)Math.Round(4 * Math.Log2(BitSize), MidpointRounding.ToEven) + 13;
                var payloadBits = Precision - 2;
                var bits = Unsafe.BitCast<TFloat, TFloatUIntBits>(value);
                var isQuiet = TFloatUIntBits.IsOddInteger(bits >>> payloadBits);
                UltimateOrb.UInt128 payload;
                if (payloadBits > 110) {
                    bits &= (TFloatUIntBits.One << 110) - TFloatUIntBits.One;
                    payload = (UltimateOrb.UInt128)System.UInt128.CreateTruncating(bits);
                } else {
                    bits &= (TFloatUIntBits.One << payloadBits) - TFloatUIntBits.One;
                    var shift = 110 - payloadBits;
                    payload = (UltimateOrb.UInt128)System.UInt128.CreateTruncating(bits);
                    payload <<= shift;
                }
                payload |= isQuiet ? PositiveQuietNaN.bits : PositiveSignalingNaN.bits;
                payload |= TFloat.IsNegative(value) ? (-default(Decimal128Bid)).bits : default;
                return new Decimal128Bid(payload, CtorFromBits);
            }
        }

        public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out Decimal128Bid result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(byte)) {
                byte actualValue = (byte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(char)) {
                char actualValue = (char)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(decimal)) {
                decimal actualValue = (decimal)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(double)) {
                double actualValue = (double)(object)value;
                result = FromIeee754InterchangeBinary<double, UInt64>(actualValue);
                return true;
            } else if (typeof(TOther) == typeof(Half)) {
                Half actualValue = (Half)(object)value;
                result = FromIeee754InterchangeBinary<Half, UInt16>(actualValue);
                return true;
            }
#if NET11_0_OR_GREATER
            else if (typeof(TOther) == typeof(BFloat16)) {
                BFloat16 actualValue = (BFloat16)(object)value;
                result = FromIeee754InterchangeBinary<BFloat16, UInt16>(actualValue);
                return true;
            }
#endif
            else if (typeof(TOther) == typeof(short)) {
                short actualValue = (short)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(int)) {
                int actualValue = (int)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(long)) {
                long actualValue = (long)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.Int128)) {
                UltimateOrb.Int128 actualValue = (UltimateOrb.Int128)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(System.Int128)) {
                System.Int128 actualValue = (System.Int128)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nint)) {
                nint actualValue = (nint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(sbyte)) {
                sbyte actualValue = (sbyte)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(float)) {
                float actualValue = (float)(object)value;
                result = FromIeee754InterchangeBinary<float, UInt32>(actualValue);
                return true;
            } else if (typeof(TOther) == typeof(ushort)) {
                ushort actualValue = (ushort)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(uint)) {
                uint actualValue = (uint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(ulong)) {
                ulong actualValue = (ulong)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(UltimateOrb.UInt128)) {
                UltimateOrb.UInt128 actualValue = (UltimateOrb.UInt128)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(System.UInt128)) {
                System.UInt128 actualValue = (System.UInt128)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(nuint)) {
                nuint actualValue = (nuint)(object)value;
                result = actualValue;
                return true;
            } else if (typeof(TOther) == typeof(BigInteger)) {
                BigInteger actualValue = (BigInteger)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else if (typeof(TOther) == typeof(Rational64)) {
                Rational64 actualValue = (Rational64)(object)value;
                result = (Decimal128Bid)actualValue;
                return true;
            } else {
                result = default;
                return false;
            }
        }






        public static bool TryConvertToChecked<TOther>(Decimal128Bid value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToSaturating<TOther>(Decimal128Bid value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryConvertToTruncating<TOther>(Decimal128Bid value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther> {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Decimal128Bid result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Decimal128Bid result) {
            throw new NotImplementedException();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Decimal128Bid result) {
            throw new NotImplementedException();
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Decimal128Bid result) {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificand(ulong lo, ulong hi, out ulong significand_lo, out ulong significand_hi) {
            // The following IS the SAME IEEE‑754 BID format, but is viewed in an equivalent convient way in this library.
            // sub-format1: (each lotter represents 1 bit) s11eeeee eeeeeeee eccccccc cccccccc ... (...=96*c) // significand >= (UInt128)0B1000UL << 110
            // sub-format0: (each lotter represents 1 bit) seeeeeee eeeeeeec cccccccc cccccccc ... (...=96*c) // significand < (UInt128)0B1000UL << 110
            // sign 's' bit: 1 bit.
            // exponent 'e' bits: 14 bits.
            // significand 'c' bits : (1:format1 or 3:format0) + 110 bits. (Format1 has implicit significand leading bits '100' not included in the bit count.)
            // Masks and constants
            const ulong COMBINATION_MASK = 0x6000000000000000UL;   // bits 126..125
            const ulong FORMAT1_COMB = 0x6000000000000000UL;       // '11' combination
            const ulong FORMAT1_COEFF_MASK = 0x00007FFFFFFFFFFFUL; // lower 47 bits of hi (bits 46..0)
            const ulong FORMAT0_COEFF_MASK = 0x0001FFFFFFFFFFFFUL; // lower 49 bits of hi (bits 48..0)

            // assign low part first
            significand_lo = lo;

            if ((hi & COMBINATION_MASK) == FORMAT1_COMB) {
                // sub-format1:
                // stored continuation length = 47 (hi) + 64 (lo) = 111 bits
                // implicit leading bits '100' sit ABOVE those 111 stored bits:
                //   implicit bit '1' is at full-bit index 113 (LSB = 0)
                //   within significand_hi (bits 127..64) that is position (113 - 64) = 49
                uint biasedExp = (uint)((hi >> 47) & 0x3FFFUL);

                // low part already assigned
                // extract stored 47-bit continuation from hi (bits 46..0)
                ulong stored_hi47 = hi & FORMAT1_COEFF_MASK;

                // place implicit '100' above the 111 stored bits:
                // set bit 49 in significand_hi (this corresponds to full-bit index 113)
                significand_hi = (1UL << 49) | stored_hi47;

                return biasedExp;
            } else {
                // sub-format0:
                uint biasedExp = (uint)((hi >> 49) & 0x3FFFUL);

                // low part already assigned
                // extract the full 49 bits stored in hi (bits 48..0)
                ulong top49 = hi & FORMAT0_COEFF_MASK;

                // place top49 into the high 64-bit word (it occupies the low 49 bits of significand_hi)
                significand_hi = top49;

                return biasedExp;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificandForCanonical(ulong lo, ulong hi, out ulong significand_lo, out ulong significand_hi) {
            const ulong COMBINATION_MASK = 0x6000000000000000UL;   // bits 126..125
            const ulong FORMAT1_COMB = 0x6000000000000000UL;       // '11' combination
            const ulong FORMAT1_COEFF_MASK = 0x00007FFFFFFFFFFFUL; // lower 47 bits of hi (bits 46..0)
            const ulong FORMAT0_COEFF_MASK = 0x0001FFFFFFFFFFFFUL; // lower 49 bits of hi (bits 48..0)

            // assign low part first
            significand_lo = lo;

            {
                // sub-format0:
                uint biasedExp = (uint)((hi >> 49) & 0x3FFFUL);

                // low part already assigned
                // extract the full 49 bits stored in hi (bits 48..0)
                ulong top49 = hi & FORMAT0_COEFF_MASK;

                // place top49 into the high 64-bit word (it occupies the low 49 bits of significand_hi)
                significand_hi = top49;

                return biasedExp;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificand(UInt128 value, out UInt128 significand) {
            // Split value into lo/hi 64-bit parts
            ulong lo = unchecked((ulong)value);
            ulong hi = unchecked((ulong)(value >> 64));

            // Reuse the ulong-based extractor once
            uint rawExp = ExtractRawBiasedExponentAndRawSignificand(lo, hi, out ulong sigLo, out ulong sigHi49);

            // Compose UInt128 significand: place the 49-bit sigHi into the high 64 bits and OR the low 64 bits
            significand = ((UInt128)sigHi49 << 64) | sigLo;
            return rawExp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ExtractRawBiasedExponentAndRawSignificandForCanonical(UInt128 value, out UInt128 significand) {
            // Split value into lo/hi 64-bit parts
            ulong lo = unchecked((ulong)value);
            ulong hi = unchecked((ulong)(value >> 64));

            // Reuse the ulong-based extractor once
            uint rawExp = ExtractRawBiasedExponentAndRawSignificandForCanonical(lo, hi, out ulong sigLo, out ulong sigHi49);

            // Compose UInt128 significand: place the 49-bit sigHi into the high 64 bits and OR the low 64 bits
            significand = ((UInt128)sigHi49 << 64) | sigLo;
            return rawExp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint ExtractRawBiasedExponentAndRawSignificand(out UInt128 significand) {
            return ExtractRawBiasedExponentAndRawSignificand(this.bits, out significand);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal uint ExtractRawBiasedExponentAndRawSignificandForCanonical(out UInt128 significand) {
            var t = this;
            Debug.Assert(IsCanonical(t));
            return ExtractRawBiasedExponentAndRawSignificandForCanonical(t.bits, out significand);
        }

        public int CompareTo(object? obj) {
            throw new NotImplementedException();
        }

        public int CompareTo(Decimal128Bid other) {
            if (!Decimal128Bid.IsFinite(this) || !Decimal128Bid.IsFinite(other)) {
                // NaN < -inf in .NET total order
                if (Decimal128Bid.IsNaN(this)) {
                    return Decimal128Bid.IsNaN(other) ? 0 : -1;
                }
                if (Decimal128Bid.IsNaN(other)) {
                    return 1;
                }
                if (Decimal128Bid.IsNegativeInfinity(this)) {
                    return Decimal128Bid.IsNegativeInfinity(other) ? 0 : -1;
                }
                if (Decimal128Bid.IsPositiveInfinity(this)) {
                    return Decimal128Bid.IsPositiveInfinity(other) ? 0 : 1;
                }
                Debug.Assert(Decimal128Bid.IsInfinity(other));
                return Decimal128Bid.IsPositiveInfinity(other) ? -1 : 1;
            }
            Debug.Assert(Decimal128Bid.IsFinite(this));
            Debug.Assert(Decimal128Bid.IsFinite(other));
            return ((BigRational)this).CompareTo((BigRational)other);
        }

        public bool Equals(Decimal128Bid other) {
            if (!Decimal128Bid.IsFinite(this) || !Decimal128Bid.IsFinite(other)) {
                return (Decimal128Bid.IsNaN(this) && Decimal128Bid.IsNaN(other)) ||
                    (Decimal128Bid.IsNegativeInfinity(this) && Decimal128Bid.IsNegativeInfinity(other)) ||
                    (Decimal128Bid.IsPositiveInfinity(this) && Decimal128Bid.IsPositiveInfinity(other));
            }
            Debug.Assert(Decimal128Bid.IsFinite(this));
            Debug.Assert(Decimal128Bid.IsFinite(other));
            return (BigRational)this == (BigRational)other;
        }

        public int GetExponentByteCount() {
            return sizeof(Int16);
        }

        public int GetExponentShortestBitLength() {
            unchecked {
                var exponent = unchecked((short)(RawBiasedExponent - EXP_BIAS));
                if (exponent >= 0) {
                    return (sizeof(short) * 8) - short.LeadingZeroCount(exponent);
                } else {
                    return (sizeof(short) * 8) + 1 - short.LeadingZeroCount((short)(~exponent));
                }
            }
        }

        public int GetSignificandBitLength() {
            return 114;
        }

        public int GetSignificandByteCount() {
            return Unsafe.SizeOf<UInt128>();
        }

        TypeCode IConvertible.GetTypeCode() {
            return TypeCode.Object;
        }

        char IConvertible.ToChar(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, nameof(Decimal128), nameof(Char)));
        }

        DateTime IConvertible.ToDateTime(IFormatProvider? provider) {
            throw new InvalidCastException(SR.Format(SR.InvalidCast_FromTo, nameof(Decimal128), nameof(DateTime)));
        }

        double IConvertible.ToDouble(IFormatProvider? provider) {
            return (double)this;
        }

        Single IConvertible.ToSingle(IFormatProvider? provider) {
            return (Single)this;
        }

        public string ToString(string? format, IFormatProvider? formatProvider) {
            var @this = this;
            if (IsNaN(@this)) {
                return "NaN";
            }
            if (IsPositiveInfinity(@this)) {
                return "∞";
            }
            if (IsNegativeInfinity(@this)) {
                return "-∞";
            }
            var sb = new StringBuilder();
            if (IsNegative(@this)) {
                sb.Append('-');
            }
            var pos = sb.Length;
            var exp = unchecked((int)ExtractRawBiasedExponentAndRawSignificand(out var significand));
            Debug.Assert(exp <= MaxBiasedExponent);
            exp -= EXP_BIAS;
            if (significand > MaxSignificandAsUInt128) {
                significand = 0;
            }
            if (exp == 0) {
                // quantum >= 10^0
                sb.Append(significand);
            } else {
                var ee = UInt128Exp10Module.ILog10(significand);
                if (ee == ILogSpecialResults.ILog0) {
                    ee = 0;
                }
                var eee = exp + ee;
                if (exp > 0 || eee < -3) {
                    // E notation
                    // quantum >= 10^1, - or - 
                    // magnitude < 0.001
                    sb.Append(significand);
                    if (significand >= 10) {
                        sb.Insert(pos + 1, '.');
                    }
                    sb.Append('E');
                    if (eee >= 0) {
                        sb.Append('+');
                        sb.Append(eee);
                    } else {
                        sb.Append('-');
                        sb.Append(-eee);
                    }
                } else {
                    // common decimal form
                    if (eee < 0) {
                        sb.Append("0.");
                        sb.Append('0', -eee - 1);
                        sb.Append(significand);
                    } else {
                        sb.Append(significand);
                        sb.Insert(pos + 1 + eee, '.');
                    }
                }
            }
            return sb.ToString();
        }

        public string ToString(IFormatProvider? provider) {
            return ToString(null, provider);
        }

        public override string ToString() {
            return ToString(null, null);
        }

        public object ToType(Type conversionType, IFormatProvider? provider) {
            if (typeof(BigRational) == conversionType) {
                return (BigRational)this;
            }
            return ConvertInternal.DefaultToType(in this, conversionType, provider);
        }

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider) {
            // TODO: implement properly
            var s = ToString(format.ToString(), provider);
            if (s.Length <= destination.Length) {
                s.AsSpan().CopyTo(destination);
                charsWritten = s.Length;
                return true;
            } else {
                charsWritten = 0;
                return false;
            }
        }

        public bool TryWriteExponentBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(Int16)) {
                var exponent = unchecked((Int16)(this.ExtractRawBiasedExponentAndRawSignificand(out _) - EXP_BIAS));

                if (BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(Int16);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        public bool TryWriteExponentLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= sizeof(Int16)) {
                var exponent = unchecked((Int16)(this.ExtractRawBiasedExponentAndRawSignificand(out _) - EXP_BIAS));

                if (!BitConverter.IsLittleEndian) {
                    exponent = BinaryPrimitives.ReverseEndianness(exponent);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), exponent);

                bytesWritten = sizeof(Int16);
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        static readonly UInt128 NaNMaxPayloadAsUInt128 = (UInt128.One << 110) - 1;

        internal UInt128 GetSignificandOrPayload() {
            var exp = this.ExtractRawBiasedExponentAndRawSignificand(out var significand);
            if (exp <= MaxBiasedExponent) {
                if (significand > MaxSignificandAsUInt128) {
                    significand = 0;
                }
            } else {
                significand &= NaNMaxPayloadAsUInt128;
            }
            return significand;
        }

        public bool TryWriteSignificandBigEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= Unsafe.SizeOf<UInt128>()) {
                var significand = this.GetSignificandOrPayload();

                if (BitConverter.IsLittleEndian) {
                    significand = BinaryPrimitives.ReverseEndianness(significand);
                }

                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = Unsafe.SizeOf<UInt128>();
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        public bool TryWriteSignificandLittleEndian(Span<byte> destination, out int bytesWritten) {
            if (destination.Length >= Unsafe.SizeOf<UInt128>()) {
                var significand = this.GetSignificandOrPayload();

                if (!BitConverter.IsLittleEndian) {
                    significand = BinaryPrimitives.ReverseEndianness(significand);
                }
                double a;
                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(destination), significand);

                bytesWritten = Unsafe.SizeOf<UInt128>();
                return true;
            } else {
                bytesWritten = 0;
                return false;
            }
        }

        UInt64 HiUInt64Bits => unchecked((UInt64)(bits.GetHighPart()));

        internal int RawBiasedExponent {
            get {
                return unchecked((int)ExtractRawBiasedExponentAndRawSignificand(out _));
            }
        }

        internal int RawBiasedExponentForCanonical {

            get {
                return unchecked((int)ExtractRawBiasedExponentAndRawSignificandForCanonical(out _));
            }
        }

        const UInt64 HiUInt64SignBitMask = ~(UInt64.MaxValue >>> 1);
        const int Hi64BiasedExponentShift = 49;


        public static Decimal128Bid operator +(Decimal128Bid value) {
            return value;
        }
        // Name conventions
        // BiasedExponent (E): according to stored bits
        // QuantumExponent (denominator): E - bias
        // Exponent (mathematical): e in s.sss... * 10^e with 1 <= s < 10
        // ~Internal: Do not returns int.MinValue/sentinel value in case of !IsFinite
        // ~ForIeeeCanonical: input/this value is assumed IEEE canonical
        // ~ForCanonical: input/this value is assumed canonical (IEEE canonical + BID canonical, 3 bits of significand in combination bits)
        internal const int MaxBiasedExponent = 0B_0010_1111_1111_1111;

        internal static Decimal128Bid AdjustSignBitAndBiasedExponent(
            Decimal128Bid value,
            ulong preferredSignBit,
            int preferredBiasedExponent) {
            Debug.Assert(preferredBiasedExponent >= 0);
            Debug.Assert(preferredBiasedExponent <= MaxBiasedExponent);

            // Handle NaN here (this method keeps NaN handling)
            if (Decimal128Bid.IsNaN(value)) {
                return value;
            }

            // Delegate all other cases to the partial helper that skips NaN handling
            return AdjustSignBitAndBiasedExponentPartial(value, preferredSignBit, preferredBiasedExponent);
        }

        internal static Decimal128Bid AdjustSignBitAndBiasedExponentPartial0(Decimal128Bid value, ulong preferredSignBit, int preferredBiasedExponent) {

            // IMPORTANT: we do NOT clamp the working biasedExp after adjustments — that would change the numeric value.

            // Extract canonical raw exponent and canonical significand for manipulation
            // ExtractRawBiasedExponentAndRawSignificandForCanonical returns the biased exponent and outputs the significand
            int biasedExp = unchecked((int)value.ExtractRawBiasedExponentAndRawSignificandForCanonical(out UInt128 significand));

            // If nothing changed (exponent and significand unchanged), return early
            if (biasedExp == preferredBiasedExponent)
                return value;

            var maxSignificandOverTen = MaxSignificandOverTenAsUInt128;
            const uint ten = 10;

            // Move biasedExp toward targetBiasedExp by exact powers of 10 only,
            // checking exponent bounds before each step to avoid producing out-of-range exponents.
            if (biasedExp > preferredBiasedExponent) {
                // Decrease stored exponent: multiply significand by 10 while it fits and while we stay >= MinBiasedExponent
                while (biasedExp > preferredBiasedExponent) {
                    // Prevent stepping below MinBiasedExponent
                    if (biasedExp - 1 < 0)
                        break;

                    // Prevent significand overflow on multiply
                    if (significand > maxSignificandOverTen)
                        break;

                    significand *= ten;
                    biasedExp--;
                }
            } else if (biasedExp < preferredBiasedExponent) {
                // Increase stored exponent: divide significand by 10 only when exact and while we stay <= MaxBiasedExponent
                while (biasedExp < preferredBiasedExponent) {
                    // Prevent stepping above MaxBiasedExponent
                    if (biasedExp + 1 > MaxBiasedExponent)
                        break;

                    // Use explicit DivRem to check for exact division
                    UInt128 remainder;
                    UInt128 quotient = UInt128.DivRem(significand, ten, out remainder);
                    if (remainder != 0)
                        break; // cannot divide exactly; stop adjusting

                    significand = quotient;
                    biasedExp++;
                }
            }

            // Repack components into bits.
            // Preserve original sign bit for non-zero finite values.
            // Clear exponent and top-significand fields in hi, then OR in new values.
            ulong hi = value.HiUInt64Bits & HiUInt64SignBitMask;

            // top-significand bits come from the high part of the canonical significand
            ulong topBits = unchecked((UInt64)significand.HiInt64Bits);

            hi |= topBits | ((ulong)biasedExp << Hi64BiasedExponentShift);

            // Low 64 bits are the low part of significand
            ulong lo = unchecked((UInt64)significand);
            var newBitsFinal = ((UInt128)hi << 64) | (UInt128)lo;

            return new(newBitsFinal, CtorFromBits);
        }


        internal static Decimal128Bid AdjustSignBitAndBiasedExponentPartial(Decimal128Bid value, ulong preferredSignBit, int preferredBiasedExponent) {
            Debug.Assert(preferredBiasedExponent >= 0);
            Debug.Assert(preferredBiasedExponent <= MaxBiasedExponent);

            // Caller handled NaN
            Debug.Assert(!Decimal128Bid.IsNaN(value));

            // Infinity: do not change sign or exponent
            if (Decimal128Bid.IsInfinity(value)) {
                return value;
            }

            // Zero: set sign and biased exponent to preferred (clamp preferred into valid range),
            // keep significand = 0
            if (Decimal128Bid.IsZero(value)) {
                ulong packedHi = (ulong)preferredSignBit | ((ulong)preferredBiasedExponent << Hi64BiasedExponentShift);
                ulong packedLo = 0UL;
                var newBits128 = ((UInt128)packedHi << 64) | (UInt128)packedLo;
                return new(newBits128, CtorFromBits);
            }

            return AdjustSignBitAndBiasedExponentPartial0(value, preferredSignBit, preferredBiasedExponent);
        }

        internal static Decimal128Bid AdjustExponent0Partial(Decimal128Bid value) {
            Debug.Assert(Decimal128Bid.IsFinite(value));
            Debug.Assert(!Decimal128Bid.IsZero(value));

            var preferredBiasedExponent = EXP_BIAS;

            // IMPORTANT: we do NOT clamp the working biasedExp after adjustments — that would change the numeric value.

            // Extract canonical raw exponent and canonical significand for manipulation
            // ExtractRawBiasedExponentAndRawSignificandForCanonical returns the biased exponent and outputs the significand
            int biasedExp = unchecked((int)value.ExtractRawBiasedExponentAndRawSignificandForCanonical(out UInt128 significand));

            // If nothing changed (exponent and significand unchanged), return early
            if (biasedExp == preferredBiasedExponent)
                return value;

            var maxSignificandOverTen = MaxSignificandOverTenAsUInt128;
            const uint ten = 10;

            // Move biasedExp toward targetBiasedExp by exact powers of 10 only,
            // checking exponent bounds before each step to avoid producing out-of-range exponents.
            if (biasedExp > preferredBiasedExponent) {
                // Decrease stored exponent: multiply significand by 10 while it fits and while we stay >= MinBiasedExponent
                while (biasedExp > preferredBiasedExponent) {
                    // Prevent stepping below MinBiasedExponent
                    if (biasedExp - 1 < 0)
                        break;

                    // Prevent significand overflow on multiply
                    if (significand > maxSignificandOverTen)
                        break;

                    significand *= ten;
                    biasedExp--;
                }
            } else if (biasedExp < preferredBiasedExponent) {
                // Increase stored exponent: divide significand by 10 only when exact and while we stay <= MaxBiasedExponent
                while (biasedExp < preferredBiasedExponent) {
                    // Prevent stepping above MaxBiasedExponent
                    if (biasedExp + 1 > MaxBiasedExponent)
                        break;

                    // Use explicit DivRem to check for exact division
                    UInt128 remainder;
                    UInt128 quotient = UInt128.DivRem(significand, ten, out remainder);
                    if (remainder != 0)
                        break; // cannot divide exactly; stop adjusting

                    significand = quotient;
                    biasedExp++;
                }
            }

            // Repack components into bits.
            // Preserve original sign bit for non-zero finite values.
            // Clear exponent and top-significand fields in hi, then OR in new values.
            ulong hi = value.HiUInt64Bits & HiUInt64SignBitMask;

            // top-significand bits come from the high part of the canonical significand
            ulong topBits = unchecked((UInt64)significand.HiInt64Bits);

            hi |= topBits | ((ulong)biasedExp << Hi64BiasedExponentShift);

            // Low 64 bits are the low part of significand
            ulong lo = unchecked((UInt64)significand);
            var newBitsFinal = ((UInt128)hi << 64) | (UInt128)lo;

            return new(newBitsFinal, CtorFromBits);
        }

        [return: NotNullIfNotNull(nameof(value))]
        static Decimal128Bid IInterfaceDerivedSelfBase<Decimal128Bid, BigRational>.FromBase(BigRational value) {
            return (Decimal128Bid)value;
        }

        [return: NotNullIfNotNull(nameof(value))]
        static BigRational IInterfaceDerivedSelfBase<Decimal128Bid, BigRational>.ToBase(Decimal128Bid value) {
            return (BigRational)value;
        }

        public static Decimal128Bid operator +(Decimal128Bid left, Decimal128Bid right) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                if (Decimal128Bid.IsNaN(left)) {
                    return left;
                }
                if (Decimal128Bid.IsNaN(right)) {
                    return right;
                }
                if (Decimal128Bid.IsFinite(left)) {
                    return right;
                }
                if (Decimal128Bid.IsFinite(right)) {
                    return left;
                }
                Debug.Assert(Decimal128Bid.IsInfinity(left));
                Debug.Assert(Decimal128Bid.IsInfinity(right));
                if (Decimal128Bid.IsNegative(left) == Decimal128Bid.IsNegative(right)) {
                    return new(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & left.bits), CtorFromBits);
                }
                return NaN;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            var preferredBiasedExponent = Math.Min(left.RawBiasedExponent, right.RawBiasedExponent);
            var preferredSignBit = HiUInt64SignBitMask & (left.HiUInt64Bits & right.HiUInt64Bits);
            var result = (Decimal128Bid)((BigRational)left + (BigRational)right);
            return AdjustSignBitAndBiasedExponent(result, preferredSignBit, preferredBiasedExponent);
        }

        public static Decimal128Bid operator -(Decimal128Bid value) {
            return new Decimal128Bid(value.bits ^ unchecked((UInt128)Int128.MinValue), CtorFromBits);
        }

        public static Decimal128Bid operator -(Decimal128Bid left, Decimal128Bid right) {
            return left + -right;
        }

        public static Decimal128Bid operator ++(Decimal128Bid value) {
            return One + value;
        }

        public static Decimal128Bid operator --(Decimal128Bid value) {
            return value - One;
        }

        public static Decimal128Bid operator *(Decimal128Bid left, Decimal128Bid right) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                if (Decimal128Bid.IsNaN(left)) {
                    return left;
                }
                if (Decimal128Bid.IsNaN(right)) {
                    return right;
                }
                if (Decimal128Bid.IsZero(left) || Decimal128Bid.IsZero(right)) {
                    Debug.Assert(Decimal128Bid.IsInfinity(left) || Decimal128Bid.IsInfinity(right));
                    return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
                }
                Debug.Assert(Decimal128Bid.IsInfinity(left) && Decimal128Bid.IsInfinity(right));
                return new(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            var preferredBiasedExponent = Math.Clamp(left.RawBiasedExponent + right.RawBiasedExponent - EXP_BIAS, 0, MaxBiasedExponent);
            var preferredSignBit = HiUInt64SignBitMask & (left.HiUInt64Bits ^ right.HiUInt64Bits);
            var result = (Decimal128Bid)((BigRational)left * (BigRational)right);
            return AdjustSignBitAndBiasedExponent(result, preferredSignBit, preferredBiasedExponent);
        }

        public static Decimal128Bid operator /(Decimal128Bid left, Decimal128Bid right) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                if (Decimal128Bid.IsNaN(left)) {
                    return left;
                }
                if (Decimal128Bid.IsNaN(right)) {
                    return right;
                }
                if (Decimal128Bid.IsFinite(left)) {
                    Debug.Assert(Decimal128Bid.IsInfinity(right));
                    return new(~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits), CtorFromBits);
                }
                Debug.Assert(Decimal128Bid.IsInfinity(left));
                if (Decimal128Bid.IsFinite(right)) {
                    return new(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
                }
                Debug.Assert(Decimal128Bid.IsInfinity(right));
                return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            if (Decimal128Bid.IsZero(right)) {
                if (Decimal128Bid.IsZero(left)) {
                    return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
                }
                return new(PositiveInfinity.bits | (~(UInt128.MaxValue >>> 1) & (left.bits ^ right.bits)), CtorFromBits);
            }
            var result = (Decimal128Bid)((BigRational)left / (BigRational)right);
            var preferredBiasedExponent = Math.Clamp(left.RawBiasedExponent - right.RawBiasedExponent + EXP_BIAS, 0, MaxBiasedExponent);
            var preferredSignBit = HiUInt64SignBitMask & (left.HiUInt64Bits ^ right.HiUInt64Bits);
            return AdjustSignBitAndBiasedExponent(result, preferredSignBit, preferredBiasedExponent);
        }

        public static Decimal128Bid operator %(Decimal128Bid left, Decimal128Bid right) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                if (Decimal128Bid.IsNaN(left)) {
                    return left;
                }
                if (Decimal128Bid.IsNaN(right)) {
                    return right;
                }
                if (Decimal128Bid.IsFinite(left)) {
                    Debug.Assert(Decimal128Bid.IsInfinity(right));
                    // TODO: Canonicalization
                    return left * MultiplicativeIdentity;
                }
                Debug.Assert(Decimal128Bid.IsInfinity(left));
                /*
                if (Decimal128Bid.IsFinite(right)) {
                    return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & left.bits), CtorFromBits);
                }
                Debug.Assert(Decimal128Bid.IsInfinity(right));
                */
                return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & left.bits), CtorFromBits);
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            if (Decimal128Bid.IsZero(right)) {
                /*
                if (Decimal128Bid.IsZero(left)) {
                    return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & left.bits), CtorFromBits);
                }
                */
                return new(PositiveNaN.bits | (~(UInt128.MaxValue >>> 1) & left.bits), CtorFromBits);
            }
            var result = (Decimal128Bid)((BigRational)left % (BigRational)right);
            var preferredBiasedExponent = Math.Min(left.RawBiasedExponent, right.RawBiasedExponent);
            var preferredSignBit = HiUInt64SignBitMask & left.HiUInt64Bits;
            return AdjustSignBitAndBiasedExponent(result, preferredSignBit, preferredBiasedExponent);
        }

        public static bool operator ==(Decimal128Bid left, Decimal128Bid right) {
            if (!Decimal128Bid.IsFinite(left) || !Decimal128Bid.IsFinite(right)) {
                return (Decimal128Bid.IsNegativeInfinity(left) && Decimal128Bid.IsNegativeInfinity(right)) ||
                    (Decimal128Bid.IsPositiveInfinity(left) && Decimal128Bid.IsPositiveInfinity(right));
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            return (BigRational)left == (BigRational)right;
        }

        public static bool operator !=(Decimal128Bid left, Decimal128Bid right) {
            return !(left == right);
        }

        public static bool operator <(Decimal128Bid left, Decimal128Bid right) {
            if (Decimal128Bid.IsNaN(left)) {
                return false;
            }
            if (Decimal128Bid.IsNaN(right)) {
                return false;
            }
            if (Decimal128Bid.IsPositiveInfinity(left) || Decimal128Bid.IsNegativeInfinity(right)) {
                return false;
            }
            if (Decimal128Bid.IsNegativeInfinity(left)) {
                return !Decimal128Bid.IsNegativeInfinity(right);
            }
            if (Decimal128Bid.IsPositiveInfinity(right)) {
                return true;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            return (BigRational)left < (BigRational)right;
        }

        public static bool operator >(Decimal128Bid left, Decimal128Bid right) {
            if (Decimal128Bid.IsNaN(left)) {
                return false;
            }
            if (Decimal128Bid.IsNaN(right)) {
                return false;
            }
            if (Decimal128Bid.IsNegativeInfinity(left) || Decimal128Bid.IsPositiveInfinity(right)) {
                return false;
            }
            if (Decimal128Bid.IsPositiveInfinity(left)) {
                return !Decimal128Bid.IsPositiveInfinity(right);
            }
            if (Decimal128Bid.IsNegativeInfinity(right)) {
                return true;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            return (BigRational)left > (BigRational)right;
        }

        public static bool operator <=(Decimal128Bid left, Decimal128Bid right) {
            if (Decimal128Bid.IsNaN(left)) {
                return false;
            }
            if (Decimal128Bid.IsNaN(right)) {
                return false;
            }
            if (Decimal128Bid.IsNegativeInfinity(left) || Decimal128Bid.IsPositiveInfinity(right)) {
                return true;
            }
            if (Decimal128Bid.IsPositiveInfinity(left)) {
                return Decimal128Bid.IsPositiveInfinity(right);
            }
            if (Decimal128Bid.IsNegativeInfinity(right)) {
                return false;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            return (BigRational)left <= (BigRational)right;
        }

        public static bool operator >=(Decimal128Bid left, Decimal128Bid right) {
            if (Decimal128Bid.IsNaN(left)) {
                return false;
            }
            if (Decimal128Bid.IsNaN(right)) {
                return false;
            }
            if (Decimal128Bid.IsPositiveInfinity(left) || Decimal128Bid.IsNegativeInfinity(right)) {
                return true;
            }
            if (Decimal128Bid.IsNegativeInfinity(left)) {
                return Decimal128Bid.IsNegativeInfinity(right);
            }
            if (Decimal128Bid.IsPositiveInfinity(right)) {
                return false;
            }
            Debug.Assert(Decimal128Bid.IsFinite(left));
            Debug.Assert(Decimal128Bid.IsFinite(right));
            return (BigRational)left >= (BigRational)right;
        }
    }
}
namespace UltimateOrb {

    public static partial class Decimal128Extensions {

    }

    partial class Decimal128Extensions {


    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static explicit operator Decimal128Bid(UltimateOrb.Int128 value) {
            return (Decimal128Bid)(BigRational)(BigInteger)(System.Int128)value;
        }

        public static explicit operator Decimal128Bid(UltimateOrb.UInt128 value) {
            return (Decimal128Bid)(BigRational)(BigInteger)value;
        }

        public static explicit operator Decimal128Bid(System.Int128 value) {
            return (Decimal128Bid)(BigRational)(BigInteger)value;
        }

        public static explicit operator Decimal128Bid(System.UInt128 value) {
            return (Decimal128Bid)(BigRational)(BigInteger)value;
        }
    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static implicit operator Decimal128Bid(decimal value) {
            var d = Unsafe.BitCast<System.Decimal, Win32Decimal>(value);
            return unchecked(new(d._lo64 | ((UInt128)d._hi32 << 64), (uint)(EXP_BIAS - (0Xff & (d._flags >> 16))), (UInt64)((Int64)((Int32)d._flags >> 31) << 63), CtorFromPartsCanonical));
        }

        public static Decimal128Bid FromIeee754InterchangeBinaryWidening<TFloat, TFloatUIntBits>(TFloat value)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {
            // Handle finite values by converting via BigRational -> exact integer numerator/denominator
            if (TFloat.IsFinite(value)) {
                var q = BigRational.FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>(value);
                var v = (Decimal128Bid)q;
                return TFloat.IsNegative(value) != Decimal128Bid.IsNegative(v) ? -v : v;
            }

            // Infinities
            if (TFloat.IsNegativeInfinity(value)) {
                return NegativeInfinity;
            }
            if (TFloat.IsPositiveInfinity(value)) {
                return PositiveInfinity;
            }

            // NaN: preserve payload and signaling/quiet bit when possible
            Debug.Assert(TFloat.IsNaN(value));
#if IGNORE_NAN_PAYLOAD
            return NaN;
#else
            {
                // Compute float layout parameters
                var FloatBitSize = 8 * Unsafe.SizeOf<TFloat>();
                var SignificandBitLength = TFloat.MinValue.GetSignificandBitLength() - 1; // includes implicit bit
                var ExponentBitLength = FloatBitSize - 1 - SignificandBitLength;
                var ExponentAllOnes = (TFloatUIntBits.CreateTruncating(1u) << ExponentBitLength) - TFloatUIntBits.CreateTruncating(1u);
                var ExponentField = ExponentAllOnes << SignificandBitLength;

                // payload0bits = (significand bits) - 1 (we keep one bit for signaling/quiet)
                var payload0bits = SignificandBitLength - 1;

                // decimal128 raw significand has 110 payload bits in this representation; shift up to fit float payload
                const int Decimal128PayloadBits = 110;
                var shift = Decimal128PayloadBits - payload0bits;

                // Extract raw bits of the floating value as the unsigned integer type
                var floatBits = Unsafe.BitCast<TFloat, TFloatUIntBits>(value);

                // Extract sign, exponent and significand fields using generic integer ops
                var signField = (floatBits >> (FloatBitSize - 1)) & TFloatUIntBits.CreateTruncating(1u);
                var exponentMask = (TFloatUIntBits.CreateTruncating(1u) << ExponentBitLength) - TFloatUIntBits.CreateTruncating(1u);
                var exponentField = (floatBits >> SignificandBitLength) & exponentMask;
                var significandMask = (SignificandBitLength == 0)
                    ? TFloatUIntBits.CreateTruncating(0u)
                    : ((TFloatUIntBits.CreateTruncating(1u) << SignificandBitLength) - TFloatUIntBits.CreateTruncating(1u));
                var significandField = floatBits & significandMask;

                // The quiet/signaling bit is the top bit of the significand field (bit index SignificandBitLength-1)
                var qBitInFloat = TFloatUIntBits.CreateTruncating(1u) << (SignificandBitLength - 1);
                bool isQuiet = (significandField & qBitInFloat) != TFloatUIntBits.CreateTruncating(0u);

                // Extract the payload bits from the float significand (exclude the quiet bit)
                TFloatUIntBits payloadBitsFloat = TFloatUIntBits.CreateTruncating(0u);
                if (payload0bits > 0) {
                    var payloadMaskFloat = (payload0bits == 0)
                        ? TFloatUIntBits.CreateTruncating(0u)
                        : ((TFloatUIntBits.CreateTruncating(1u) << payload0bits) - TFloatUIntBits.CreateTruncating(1u));
                    payloadBitsFloat = significandField & payloadMaskFloat;
                }

                // Convert payloadBitsFloat into a UInt128 container (low portion)
                // We need to extract up to payload0bits (<= SignificandBitLength <= FloatBitSize-1)
                // We'll move payloadBitsFloat into a ulong then into UInt128
                ulong payloadLow64 = 0UL;
                // If TFloatUIntBits fits into 64 bits, reinterpret directly; otherwise extract low 64 bits via ToUInt64-like conversion
                if (Unsafe.SizeOf<TFloatUIntBits>() <= 8) {
                    payloadLow64 = Unsafe.As<TFloatUIntBits, ulong>(ref payloadBitsFloat);
                } else {
                    // For larger integer representations (unlikely for standard floats), mask down to 64 bits
                    var tmp = payloadBitsFloat & TFloatUIntBits.CreateTruncating(ulong.MaxValue);
                    payloadLow64 = Unsafe.As<TFloatUIntBits, ulong>(ref tmp);
                }

                UInt128 payload128 = (UInt128)payloadLow64;

                // Shift payload into decimal128 payload position
                if (shift > 0) {
                    payload128 <<= shift;
                } else if (shift < 0) {
                    payload128 >>= -shift;
                }

                // Set/clear decimal128 quiet/signaling bit.
                // User correction: decimal128 quiet/signaling bit is bit 1 of highest payload byte (i.e., second bit within the topmost payload byte).
                // Compute the overall payload bit index (0 = LSB of payload).
                int highestByteBits = Decimal128PayloadBits % 8;
                if (highestByteBits == 0) highestByteBits = 8;
                int bitIndexInHighestByte = 1; // user-specified: bit 1 of highest payload byte
                int payloadBitIndex = Decimal128PayloadBits - highestByteBits + bitIndexInHighestByte;

                UInt128 qBitMask = (UInt128.One << payloadBitIndex);
                if (isQuiet) {
                    payload128 |= qBitMask;   // set quiet bit
                } else {
                    payload128 &= ~qBitMask;  // clear for signaling NaN
                }

                // Compose Decimal128 BID NaN encoding.
                // Preserve sign in top bit of hi word.
                ulong signHi = Unsafe.As<TFloatUIntBits, ulong>(ref signField) != 0UL ? (1UL << 63) : 0UL;

                // Extract payload high and low parts (payload occupies up to 110 bits)
                ulong payloadLo = (ulong)payload128;
                ulong payloadHi = (ulong)(payload128 >> 64);

                // Build hi word:
                // Use the decimal128 NaN/combination-field pattern. For BID decimal128 the combination field for NaN
                // typically sets the top bits to indicate NaN; here we use the canonical pattern 0x7C00_0000_0000_0000UL
                const ulong Decimal128NaNHighPrefix = 0x7C00_0000_0000_0000UL;
                // Mask payloadHi to the bits that fit into hi after prefix (remaining low bits of hi)
                // The prefix uses the top 15 bits of hi in some encodings; to be conservative allow up to 62 payload bits in hi.
                const ulong HiPayloadMask = 0x0003_FFFF_FFFF_FFFFUL; // 62 bits
                payloadHi &= HiPayloadMask;

                ulong hi = unchecked(signHi | Decimal128NaNHighPrefix | payloadHi);
                ulong lo = payloadLo;

                return new Decimal128Bid(UInt128.FromBits(lo: lo, hi: hi), CtorFromBits);
            }
#endif
        }

        public static implicit operator Decimal128Bid(double value) {
            return FromIeee754InterchangeBinaryWidening<double, UInt64>(value);
        }

        public static implicit operator Decimal128Bid(float value) {
            return FromIeee754InterchangeBinaryWidening<float, UInt32>(value);
        }

        public static implicit operator Decimal128Bid(Half value) {
            return FromIeee754InterchangeBinaryWidening<Half, UInt16>(value);
        }
    }
}


namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static explicit operator Decimal128Bid(BigInteger value) {
            return (Decimal128Bid)(BigRational)value;
        }
    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        public static explicit operator decimal(Decimal128Bid value) {
            var r = (decimal)(BigRational)value;
            return Decimal128Bid.IsNegative(value) != decimal.IsNegative(r) ? -r : r;
        }

        public static TFloat ToIeee754InterchangeBinaryNarrowing<TFloat, TFloatUIntBits>(Decimal128Bid value)
            where TFloat : unmanaged, IFloatingPointIeee754<TFloat>, IMinMaxValue<TFloat>
            where TFloatUIntBits : unmanaged, IUnsignedNumber<TFloatUIntBits>, IBinaryInteger<TFloatUIntBits> {
            // Handle finite values by converting via BigRational -> exact integer numerator/denominator
            if (IsFinite(value)) {
                var q = (BigRational)value;
                // BigRational is expected to expose Numerator and Denominator as BigInteger
                var v = BigRational.ToIeee754InterchangeBinary<TFloat, TFloatUIntBits>(q);
                return IsNegative(value) != TFloat.IsNegative(v) ? -v : v;
            }
            // Infinities
            if (IsNegativeInfinity(value)) {
                return TFloat.NegativeInfinity;
            }
            if (IsPositiveInfinity(value)) {
                return TFloat.PositiveInfinity;
            }
            // NaN: preserve payload and signaling/quiet bit when possible
            Debug.Assert(IsNaN(value));
#if IGNORE_NAN_PAYLOAD
            return TFloat.NaN;
#else
            {
                // Extract decimal128 raw payload (implementation provided by Decimal128Bid)
                value.ExtractRawBiasedExponentAndRawSignificand(out var payload);

                // Compute float layout parameters
                var FloatBitSize = 8 * Unsafe.SizeOf<TFloat>();
                var SignificandBitLength = TFloat.MinValue.GetSignificandBitLength() - 1; // GetSignificandBitLength() includes implicit bit
                var ExponentBitLength = FloatBitSize - 1 - SignificandBitLength;
                var ExponentAllOnes = (TFloatUIntBits.One << ExponentBitLength) - TFloatUIntBits.One;
                var ExponentField = ExponentAllOnes << SignificandBitLength;

                // payload0bits = (significand bits) - 1 (we keep one bit for signaling/quiet)
                var payload0bits = SignificandBitLength - 1;

                // decimal128 raw significand has 110 payload bits in this representation; shift down to fit float payload
                const int Decimal128PayloadBits = 110;
                var shift = Decimal128PayloadBits - payload0bits;
                payload >>>= shift;

                // Mask payload to fit into float payload field
                var payloadBits = TFloatUIntBits.CreateTruncating(payload);
                var payloadMask = (TFloatUIntBits.One << payload0bits) - TFloatUIntBits.One;
                var significandField = payloadBits & payloadMask;

                // Set the canonical NaN pattern: exponent all ones + payload in significand.
                // For signaling NaN we must set the MSB of the significand payload (the quiet/signaling bit) appropriately.
                // The quiet/signaling bit is the top bit of the significand field (bit index SignificandBitLength-1).
                var qBit = TFloatUIntBits.One << (SignificandBitLength - 1);
                if (IsSignalingNaN(value)) {
                    // signaling NaN: ensure quiet bit is 0 (already 0 if payload didn't set it)
                    significandField &= ~qBit;
                } else {
                    // quiet NaN: set quiet bit
                    significandField |= qBit;
                }

                // Compose final bits: sign | exponent(all ones) | significandField
                var signField = IsNegative(value)
                    ? (TFloatUIntBits.One << (FloatBitSize - 1))
                    : TFloatUIntBits.Zero;

                var resultBits = unchecked(signField + (ExponentField + significandField));

                return Unsafe.BitCast<TFloatUIntBits, TFloat>(resultBits);
            }
#endif
        }

        public static explicit operator double(Decimal128Bid value) {
            return ToIeee754InterchangeBinaryNarrowing<double, UInt64>(value);
        }

        public static explicit operator Single(Decimal128Bid value) {
            return ToIeee754InterchangeBinaryNarrowing<Single, UInt32>(value);
        }

        public static explicit operator Half(Decimal128Bid value) {
            return ToIeee754InterchangeBinaryNarrowing<Half, UInt16>(value);
        }
    }
}

namespace UltimateOrb {

    partial struct Decimal128Bid {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(int value) {
            return (Decimal128Bid)(long)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(uint value) {
            return (Decimal128Bid)(ulong)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(long value) {
            return unchecked(0 > value ? -(Decimal128Bid)((ulong)(-value)) : (Decimal128Bid)(ulong)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(ulong value) {
            return new(value, EXP_BIAS, 0u, CtorFromPartsCanonical);
        }

        public static implicit operator Decimal128Bid(nint value) {
            return (Decimal128Bid)(long)value;
        }

        public static implicit operator Decimal128Bid(nuint value) {
            return (Decimal128Bid)(ulong)value;
        }

        public static implicit operator Decimal128Bid(byte value) {
            return (Decimal128Bid)(ulong)value;
        }

        public static implicit operator Decimal128Bid(sbyte value) {
            return (Decimal128Bid)(long)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(short value) {
            return (Decimal128Bid)(long)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(ushort value) {
            return (Decimal128Bid)(ulong)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(char value) {
            return (Decimal128Bid)(ulong)value;
        }

        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Decimal128Bid(Rune value) {
            return (Decimal128Bid)(uint)value.Value;
        }
        */
    }
}

namespace UltimateOrb {

    public interface ISerializableDerived<TSelf, TBase>
        : IInterfaceDerivedSelfBase<TSelf, TBase>, ISerializable
        where TSelf : ISerializableDerived<TSelf, TBase>?
        where TBase : ISerializable? {

        [Obsolete(Obsoletions.LegacyFormatterMessage, DiagnosticId = Obsoletions.LegacyFormatterDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            var this__ = TSelf.ToBase((TSelf)(object)this);
#pragma warning disable SYSLIB0050 // Type or member is obsolete
            this__.GetObjectData(info, context);
#pragma warning restore SYSLIB0050 // Type or member is obsolete
        }
    }

    public interface IDeserializationCallbackDerived<TSelf, TBase>
        : IDeserializationCallback, IInterfaceDerivedSelfBase<TSelf, TBase>
        where TSelf : IDeserializationCallbackDerived<TSelf, TBase>?
        where TBase : IDeserializationCallback? {

        void IDeserializationCallback.OnDeserialization(object? sender) {
            var this__ = TSelf.ToBase((TSelf)(object)this);
            this__.OnDeserialization(sender);
        }
    }

}

namespace UltimateOrb {


    public static partial class ObjectHelpers {

        public static void MemberwiseSet<TSelf>(ref readonly TSelf @this, in TSelf value) {
            if (typeof(TSelf).IsValueType) {
                ref var target = ref Unsafe.AsRef(in @this);
                target = value;
                return;
            }
            {
                ArgumentNullException.ThrowIfNull(value);
                var target = @this;
                if (ReferenceEquals(target, @this)) {
                    return;
                }
                throw new NotImplementedException();
                /*
                // copy contents of value to the target

                nuint byteCount = RuntimeHelpers.GetRawObjectDataSize(target);
                ref byte src = ref value.GetRawData();
                ref byte dst = ref target.GetRawData();

                if (RuntimeHelpers.GetMethodTable(target)->ContainsGCPointers)
                    Buffer.BulkMoveWithWriteBarrier(ref dst, ref src, byteCount);
                else
                    SpanHelpers.Memmove(ref dst, ref src, byteCount);
                GC.SuppressFinalize(value);
                return;
                */
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses target file to you under the MIT license.

namespace System {
    internal static partial class Obsoletions {
        internal const string SharedUrlFormat = "https://aka.ms/dotnet-warnings/{0}";

        // Please see docs\project\list-of-diagnostics.md for instructions on the steps required
        // to introduce a new obsoletion, apply it to downlevel builds, claim a diagnostic id,
        // and ensure the "aka.ms/dotnet-warnings/{0}" URL points to documentation for the obsoletion
        // The diagnostic ids reserved for obsoletions are SYSLIB0### (SYSLIB0001 - SYSLIB0999).

        internal const string LegacyFormatterMessage = "Formatter-based serialization is obsolete and should not be used.";
        internal const string LegacyFormatterDiagId = "SYSLIB0050";

        internal const string LegacyFormatterImplMessage = "This API supports obsolete formatter-based serialization. It should not be called or extended by application code.";
        internal const string LegacyFormatterImplDiagId = "SYSLIB0051";
    }
}


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//=============================================================================
//
//
// Purpose: Define HResult constants. Every exception has one of these.
//
//
//===========================================================================*/

namespace System {
    // Note: FACILITY_URT is defined as 0x13 (0x8013xxxx).  Within that
    // range, 0x1yyy is for Runtime errors (used for Security, Metadata, etc).
    // In that subrange, 0x15zz and 0x16zz have been allocated for classlib-type
    // HResults. Also note that some of our HResults have to map to certain
    // COM HR's, etc.

    // Another arbitrary decision...  Feel free to change this, as long as you
    // renumber the HResults yourself (and update rexcep.h).
    // Reflection will use 0x1600 -> 0x161f.  IO will use 0x1620 -> 0x163f.
    // Security will use 0x1640 -> 0x165f

    internal static partial class HResults {
        internal const int S_OK = unchecked((int)0x00000000);
        internal const int S_FALSE = unchecked((int)0x1);
        internal const int COR_E_ABANDONEDMUTEX = unchecked((int)0x8013152D);
        internal const int COR_E_AMBIGUOUSIMPLEMENTATION = unchecked((int)0x8013106A);
        internal const int COR_E_AMBIGUOUSMATCH = unchecked((int)0x8000211D);
        internal const int COR_E_APPDOMAINUNLOADED = unchecked((int)0x80131014);
        internal const int COR_E_APPLICATION = unchecked((int)0x80131600);
        internal const int COR_E_ARGUMENT = unchecked((int)0x80070057);
        internal const int COR_E_ARGUMENTOUTOFRANGE = unchecked((int)0x80131502);
        internal const int COR_E_ARITHMETIC = unchecked((int)0x80070216);
        internal const int COR_E_ARRAYTYPEMISMATCH = unchecked((int)0x80131503);
        internal const int COR_E_BADEXEFORMAT = unchecked((int)0x800700C1);
        internal const int COR_E_BADIMAGEFORMAT = unchecked((int)0x8007000B);
        internal const int COR_E_CANNOTUNLOADAPPDOMAIN = unchecked((int)0x80131015);
        internal const int COR_E_CODECONTRACTFAILED = unchecked((int)0x80131542);
        internal const int COR_E_CONTEXTMARSHAL = unchecked((int)0x80131504);
        internal const int COR_E_CUSTOMATTRIBUTEFORMAT = unchecked((int)0x80131605);
        internal const int COR_E_DATAMISALIGNED = unchecked((int)0x80131541);
        internal const int COR_E_DIRECTORYNOTFOUND = unchecked((int)0x80070003);
        internal const int COR_E_DIVIDEBYZERO = unchecked((int)0x80020012); // DISP_E_DIVBYZERO
        internal const int COR_E_DLLNOTFOUND = unchecked((int)0x80131524);
        internal const int COR_E_DUPLICATEWAITOBJECT = unchecked((int)0x80131529);
        internal const int COR_E_ENDOFSTREAM = unchecked((int)0x80070026);
        internal const int COR_E_ENTRYPOINTNOTFOUND = unchecked((int)0x80131523);
        internal const int COR_E_EXCEPTION = unchecked((int)0x80131500);
        internal const int COR_E_EXECUTIONENGINE = unchecked((int)0x80131506);
        internal const int COR_E_FAILFAST = unchecked((int)0x80131623);
        internal const int COR_E_FIELDACCESS = unchecked((int)0x80131507);
        internal const int COR_E_FILELOAD = unchecked((int)0x80131621);
        internal const int COR_E_FILENOTFOUND = unchecked((int)0x80070002);
        internal const int COR_E_FORMAT = unchecked((int)0x80131537);
        internal const int COR_E_INDEXOUTOFRANGE = unchecked((int)0x80131508);
        internal const int COR_E_INSUFFICIENTEXECUTIONSTACK = unchecked((int)0x80131578);
        internal const int COR_E_INSUFFICIENTMEMORY = unchecked((int)0x8013153D);
        internal const int COR_E_INVALIDCAST = unchecked((int)0x80004002);
        internal const int COR_E_INVALIDCOMOBJECT = unchecked((int)0x80131527);
        internal const int COR_E_INVALIDFILTERCRITERIA = unchecked((int)0x80131601);
        internal const int COR_E_INVALIDOLEVARIANTTYPE = unchecked((int)0x80131531);
        internal const int COR_E_INVALIDOPERATION = unchecked((int)0x80131509);
        internal const int COR_E_INVALIDPROGRAM = unchecked((int)0x8013153A);
        internal const int COR_E_IO = unchecked((int)0x80131620);
        internal const int COR_E_KEYNOTFOUND = unchecked((int)0x80131577);
        internal const int COR_E_MARSHALDIRECTIVE = unchecked((int)0x80131535);
        internal const int COR_E_MEMBERACCESS = unchecked((int)0x8013151A);
        internal const int COR_E_METHODACCESS = unchecked((int)0x80131510);
        internal const int COR_E_MISSINGFIELD = unchecked((int)0x80131511);
        internal const int COR_E_MISSINGMANIFESTRESOURCE = unchecked((int)0x80131532);
        internal const int COR_E_MISSINGMEMBER = unchecked((int)0x80131512);
        internal const int COR_E_MISSINGMETHOD = unchecked((int)0x80131513);
        internal const int COR_E_MISSINGSATELLITEASSEMBLY = unchecked((int)0x80131536);
        internal const int COR_E_MULTICASTNOTSUPPORTED = unchecked((int)0x80131514);
        internal const int COR_E_NOTFINITENUMBER = unchecked((int)0x80131528);
        internal const int COR_E_NOTSUPPORTED = unchecked((int)0x80131515);
        internal const int COR_E_OBJECTDISPOSED = unchecked((int)0x80131622);
        internal const int COR_E_OPERATIONCANCELED = unchecked((int)0x8013153B);
        internal const int COR_E_OUTOFMEMORY = unchecked((int)0x8007000E);
        internal const int COR_E_OVERFLOW = unchecked((int)0x80131516);
        internal const int COR_E_PATHTOOLONG = unchecked((int)0x800700CE);
        internal const int COR_E_PLATFORMNOTSUPPORTED = unchecked((int)0x80131539);
        internal const int COR_E_RANK = unchecked((int)0x80131517);
        internal const int COR_E_REFLECTIONTYPELOAD = unchecked((int)0x80131602);
        internal const int COR_E_RUNTIMEWRAPPED = unchecked((int)0x8013153E);
        internal const int COR_E_SAFEARRAYRANKMISMATCH = unchecked((int)0x80131538);
        internal const int COR_E_SAFEARRAYTYPEMISMATCH = unchecked((int)0x80131533);
        internal const int COR_E_SECURITY = unchecked((int)0x8013150A);
        internal const int COR_E_SERIALIZATION = unchecked((int)0x8013150C);
        internal const int COR_E_STACKOVERFLOW = unchecked((int)0x800703E9);
        internal const int COR_E_SYNCHRONIZATIONLOCK = unchecked((int)0x80131518);
        internal const int COR_E_SYSTEM = unchecked((int)0x80131501);
        internal const int COR_E_TARGET = unchecked((int)0x80131603);
        internal const int COR_E_TARGETINVOCATION = unchecked((int)0x80131604);
        internal const int COR_E_TARGETPARAMCOUNT = unchecked((int)0x8002000E);
        internal const int COR_E_THREADABORTED = unchecked((int)0x80131530);
        internal const int COR_E_THREADINTERRUPTED = unchecked((int)0x80131519);
        internal const int COR_E_THREADSTART = unchecked((int)0x80131525);
        internal const int COR_E_THREADSTATE = unchecked((int)0x80131520);
        internal const int COR_E_TIMEOUT = unchecked((int)0x80131505);
        internal const int COR_E_TYPEACCESS = unchecked((int)0x80131543);
        internal const int COR_E_TYPEINITIALIZATION = unchecked((int)0x80131534);
        internal const int COR_E_TYPELOAD = unchecked((int)0x80131522);
        internal const int COR_E_TYPEUNLOADED = unchecked((int)0x80131013);
        internal const int COR_E_UNAUTHORIZEDACCESS = unchecked((int)0x80070005);
        internal const int COR_E_VERIFICATION = unchecked((int)0x8013150D);
        internal const int COR_E_WAITHANDLECANNOTBEOPENED = unchecked((int)0x8013152C);
        internal const int CO_E_NOTINITIALIZED = unchecked((int)0x800401F0);
        internal const int DISP_E_PARAMNOTFOUND = unchecked((int)0x80020004);
        internal const int DISP_E_TYPEMISMATCH = unchecked((int)0x80020005);
        internal const int DISP_E_BADVARTYPE = unchecked((int)0x80020008);
        internal const int DISP_E_OVERFLOW = unchecked((int)0x8002000A);
        internal const int DISP_E_DIVBYZERO = unchecked((int)0x80020012);
        internal const int E_FILENOTFOUND = unchecked((int)0x80070002);
        internal const int E_FAIL = unchecked((int)0x80004005);
        internal const int E_HANDLE = unchecked((int)0x80070006);
        internal const int E_INVALIDARG = unchecked((int)0x80070057);
        internal const int E_NOTIMPL = unchecked((int)0x80004001);
        internal const int E_OUTOFMEMORY = unchecked((int)0x8007000E);
        internal const int E_POINTER = unchecked((int)0x80004003);
        internal const int ERROR_MRM_MAP_NOT_FOUND = unchecked((int)0x80073B1F);
        internal const int ERROR_TIMEOUT = unchecked((int)0x800705B4);
        internal const int RO_E_CLOSED = unchecked((int)0x80000013);
        internal const int RPC_E_CHANGED_MODE = unchecked((int)0x80010106);
        internal const int TYPE_E_TYPEMISMATCH = unchecked((int)0x80028CA0);
        internal const int STG_E_PATHNOTFOUND = unchecked((int)0x80030003);
        internal const int CTL_E_PATHNOTFOUND = unchecked((int)0x800A004C);
        internal const int CTL_E_FILENOTFOUND = unchecked((int)0x800A0035);
        internal const int FUSION_E_INVALID_NAME = unchecked((int)0x80131047);
        internal const int FUSION_E_REF_DEF_MISMATCH = unchecked((int)0x80131040);
        internal const int ERROR_TOO_MANY_OPEN_FILES = unchecked((int)0x80070004);
        internal const int ERROR_SHARING_VIOLATION = unchecked((int)0x80070020);
        internal const int ERROR_LOCK_VIOLATION = unchecked((int)0x80070021);
        internal const int ERROR_OPEN_FAILED = unchecked((int)0x8007006E);
        internal const int ERROR_DISK_CORRUPT = unchecked((int)0x80070571);
        internal const int ERROR_UNRECOGNIZED_VOLUME = unchecked((int)0x800703ED);
        internal const int ERROR_DLL_INIT_FAILED = unchecked((int)0x8007045A);
        internal const int MSEE_E_ASSEMBLYLOADINPROGRESS = unchecked((int)0x80131016);
        internal const int ERROR_FILE_INVALID = unchecked((int)0x800703EE);
    }
}