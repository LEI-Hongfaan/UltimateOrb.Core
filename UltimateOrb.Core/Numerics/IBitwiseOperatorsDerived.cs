using System;
using System.Numerics;

namespace UltimateOrb.Numerics {

    public static partial class IBitwiseOperatorsDerivedTags {

        public readonly struct Other : ITag {
        }

        public readonly struct Result : ITag {
        }
    }

    public interface IBitwiseOperatorsDerived<TSelf, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IBitwiseOperatorsDerivedTagged<TSelf, TSelf, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>
        where TSelf : IBitwiseOperatorsDerived<TSelf, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>?
        where TBase1 : IBitwiseOperators<TBase1, TOther1, TResult1>?
        where TBase2 : IBitwiseOperators<TBase2, TOther2, TResult2>? {
    }

    public interface IBitwiseOperatorsDerivedTagged<TSelf, SelfTag, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase1, TBase2, TOther, TOther, TOther1, TOther2, TResult, TResult, TResult1, TResult2>
        where TSelf : IBitwiseOperatorsDerivedTagged<TSelf, SelfTag, TBase1, TBase2, TOther, TOther1, TOther2, TResult, TResult1, TResult2>?
        where TBase1 : IBitwiseOperators<TBase1, TOther1, TResult1>?
        where TBase2 : IBitwiseOperators<TBase2, TOther2, TResult2>? {
    }

    public interface IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase1, TBase2, TOther, OtherTag, TOther1, TOther2, TResult, ResultTag, TResult1, TResult2>
        : IBitwiseOperators<TSelf, TOther, TResult>
        , IInterfaceDerivedTaggedSelfBase<TSelf, Tag<SelfTag>, TBase1, TBase2>
        , IInterfaceDerivedBase<TSelf, Tag<OtherTag, IBitwiseOperatorsDerivedTags.Other>, TOther, TOther1, TOther2>
        , IInterfaceDerivedBase<TSelf, Tag<ResultTag, IBitwiseOperatorsDerivedTags.Result>, TResult, TResult1, TResult2>
        where TSelf : IBitwiseOperatorsDerivedFullyTagged<TSelf, SelfTag, TBase1, TBase2, TOther, OtherTag, TOther1, TOther2, TResult, ResultTag, TResult1, TResult2>?
        where TBase1 : IBitwiseOperators<TBase1, TOther1, TResult1>?
        where TBase2 : IBitwiseOperators<TBase2, TOther2, TResult2>? {

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ~(TSelf value) {
            return TSelf.FromBase(~TSelf.ToBase1(value)!, ~TSelf.ToBase2(value)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator &(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! & TSelf.ToBase1(right)!, TSelf.ToBase2(left)! & TSelf.ToBase2(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator |(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! | TSelf.ToBase1(right)!, TSelf.ToBase2(left)! | TSelf.ToBase2(right)!)!;
        }

        static TResult IBitwiseOperators<TSelf, TOther, TResult>.operator ^(TSelf left, TOther right) {
            return TSelf.FromBase(TSelf.ToBase1(left)! ^ TSelf.ToBase1(right)!, TSelf.ToBase2(left)! ^ TSelf.ToBase2(right)!)!;
        }
    }
}

namespace UltimateOrb.Numerics.Extensions {

    partial class BinaryNumberExtensions {

        extension<TSelf>(TSelf @this)
            where TSelf : IBinaryNumber<TSelf> {

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_BitwiseAnd(TSelf,TSelf)"/>
            public static TSelf operator &(TSelf left, TSelf right) => left & right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_BitwiseOr(TSelf,TSelf)"/>
            public static TSelf operator |(TSelf left, TSelf right) => left | right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_ExclusiveOr(TSelf,TSelf)"/>
            public static TSelf operator ^(TSelf left, TSelf right) => left ^ right;

            /// <inheritdoc cref="IBitwiseOperators{TSelf, TSelf, TSelf}.op_OnesComplement(TSelf)"/>
            public static TSelf operator ~(TSelf value) => ~value;
        }
    }
}