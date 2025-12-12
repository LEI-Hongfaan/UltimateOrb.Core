using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    internal static partial class ComplexExtensions {

        public static bool TryConvertToTRealChecked(this System.Numerics.Complex value, out double result) {
            var x = value.Real;
            var y = value.Imaginary;
            if (x.Equals(x - y) && x.Equals(x + y)) {
                result = x;
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryConvertToTRealSaturating(this System.Numerics.Complex value, out double result) {
            return TryConvertToTRealTruncating(value, out result);
        }

        public static bool TryConvertToTRealTruncating(this System.Numerics.Complex value, out double result) {
            result = value.Real;
            return true;
        }
    }
}
