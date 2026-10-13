using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace UltimateOrb.Numerics {

    internal static partial class NumberBaseInternal<TSelf> where TSelf : INumberBase<TSelf>? {

        public static bool TryCreateChecked<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
            } else if (!INumberBaseFriendInternal<TSelf>.TryConvertFromChecked(value, out result) && !INumberBaseFriendInternal<TOther>.TryConvertToChecked(value, out result)) {
                return false;
            }
            return true;
        }

        public static bool TryCreateSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
            } else if (!INumberBaseFriendInternal<TSelf>.TryConvertFromSaturating(value, out result) && !INumberBaseFriendInternal<TOther>.TryConvertToSaturating(value, out result)) {
                return false;
            }
            return true;
        }

        public static bool TryCreateTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther> {
            if (typeof(TOther) == typeof(TSelf)) {
                result = (TSelf)(object)value;
            } else if (!INumberBaseFriendInternal<TSelf>.TryConvertFromTruncating(value, out result) && !INumberBaseFriendInternal<TOther>.TryConvertToTruncating(value, out result)) {
                return false;
            }
            return true;
        }
    }

    internal static partial class NumberBaseExtensions {

        extension<TSelf>(TSelf) where TSelf : INumberBase<TSelf>? {

            public static bool TryCreateChecked<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>
                => NumberBaseInternal<TSelf>.TryCreateChecked(value, out result);

            public static bool TryCreateSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>
                => NumberBaseInternal<TSelf>.TryCreateSaturating(value, out result);

            public static bool TryCreateTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result) where TOther : INumberBase<TOther>
                => NumberBaseInternal<TSelf>.TryCreateTruncating(value, out result);
        }
    }

    interface INumberBaseFriendInternal<TSelf> : INumberBase<TSelf>
        where TSelf : INumberBase<TSelf> {

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromChecked{TOther}(TOther, out TSelf)"/>
        public new static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertFromChecked(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromSaturating{TOther}(TOther, out TSelf)"/>
        public new static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertFromSaturating(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertFromTruncating{TOther}(TOther, out TSelf)"/>
        public new static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out TSelf result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertFromTruncating(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToChecked{TOther}(TSelf, out TOther)"/>
        public new static bool TryConvertToChecked<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertToChecked(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToSaturating{TOther}(TSelf, out TOther)"/>
        public new static bool TryConvertToSaturating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertToSaturating(value, out result);
        }

        /// <inheritdoc cref="INumberBase{TSelf}.TryConvertToTruncating{TOther}(TSelf, out TOther)"/>
        public new static bool TryConvertToTruncating<TOther>(TSelf value, [MaybeNullWhen(false)] out TOther result)
#nullable disable
            where TOther : INumberBase<TOther>
#nullable restore
            {
            return TSelf.TryConvertToTruncating(value, out result);
        }
    }
}
