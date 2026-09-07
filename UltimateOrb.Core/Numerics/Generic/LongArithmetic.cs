using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics.Generic {

    /// <summary>
    /// requires:<br/>
    ///   default(T) == T.Zero<br/>
    ///   default(TInt) == TInt.Zero<br/>
    ///   default(TUInt) == TUInt.Zero<br/>
    ///   default(TInt) == TInt.Zero<br/>
    ///   default(TUInt) == TUInt.Zero<br/>
    ///   T.MinValue ^ T.MaxValue == T.AllBitsSet<br/>
    ///   TInt.MinValue ^ TInt.MaxValue == TInt.AllBitsSet<br/>
    ///   TUInt.MinValue ^ TUInt.MaxValue == TUInt.AllBitsSet<br/>
    ///   bitWidth(T) == T.PopCount(T.AllBitsSet)<br/>
    ///   bitWidth(TInt) == TInt.PopCount(TInt.AllBitsSet)<br/>
    ///   bitWidth(TUInt) == TInt.PopCount(TUInt.AllBitsSet)<br/>
    ///   bitWidth(TInt) == bitSize(TInt)<br/>
    ///   bitWidth(TUInt) == bitSize(TUInt)<br/>
    ///   bitWidth(TUInt) == bitWidth(TInt)<br/>
    ///   bitWidth(TULong) == bitWidth(TLong)<br/>
    ///   bitWidth(TInt) >= bitWidth(T)<br/>
    ///   bitWidth(TLong) >= 2 * bitWidth(TInt)<br/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TInt"></typeparam>
    /// <typeparam name="TUInt"></typeparam>
    /// <typeparam name="TLong"></typeparam>
    /// <typeparam name="TULong"></typeparam>
    internal static partial class LongArithmetic<T, TInt, TUInt, TLong, TULong>
        where T : IBinaryInteger<T>
        where TInt : IBinaryInteger<TInt>, ISignedNumber<TInt>, IMinMaxValue<TInt>
        where TUInt : IBinaryInteger<TUInt>, IUnsignedNumber<TUInt>, IMinMaxValue<TUInt>
        where TLong : IBinaryInteger<TLong>, ISignedNumber<TLong>, IMinMaxValue<TLong>
        where TULong : IBinaryInteger<TULong>, IUnsignedNumber<TULong>, IMinMaxValue<TULong> {

        static readonly TLong IntMaxValueAsLong = TLong.CreateTruncating(TInt.MaxValue);

        static readonly TLong IntMinValueAsLong = TLong.CreateTruncating(TInt.MinValue);
    }
}
