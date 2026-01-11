using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace UltimateOrb {
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
