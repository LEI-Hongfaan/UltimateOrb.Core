using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    [Obsolete]
    public readonly struct CircularInterval {

        readonly Vector128<double> b;

        public CircularInterval(Vector128<double> b) {
            this.b = b;
        }

        public static CircularInterval operator +(CircularInterval value) {
            return value;
        }

        public static CircularInterval operator -(CircularInterval value) {
            var t = -value.b;
            t = Avx.Permute(t, 0b0000_0000_0000_0000_0000_0000_0000_0001); // Swap the elements to maintain circularity
            return new CircularInterval(t);
        }

        public static CircularInterval operator +(CircularInterval first, CircularInterval second) {
            return new CircularInterval(first.b + second.b);
        }

        public static CircularInterval operator -(CircularInterval first, CircularInterval second) {
            return new CircularInterval(first.b - second.b);
        }

        public static CircularInterval operator *(CircularInterval first, CircularInterval second) {
            // (a,b) == { x \in R \cup {inf,-inf} | a <= x AND x <= b } if a <= b else { x \in R \cup {inf,-inf} | b <= x OR x <= a }
            // (-2,3) * (-5,7) -> (-15,21)

            // (3,-10) * (7,11) -> (21,-70)

            // (0,inf) * (1,5) -> (0,inf)

            // (3,-10) * (-1,2) -> (-inf,inf)

            // Unpack interval endpoints
            double a0 = first.b.GetElement(0);
            double a1 = first.b.GetElement(1);
            double b0 = second.b.GetElement(0);
            double b1 = second.b.GetElement(1);

            // Compute all products
            double p0 = a0 * b0;
            double p1 = a0 * b1;
            double p2 = a1 * b0;
            double p3 = a1 * b1;

            // Find min and max
            double min = Math.Min(Math.Min(p0, p1), Math.Min(p2, p3));
            double max = Math.Max(Math.Max(p0, p1), Math.Max(p2, p3));

            // Determine if the intervals are "circular" (a0 > a1 or b0 > b1)
            bool firstCircular = a0 > a1;
            bool secondCircular = b0 > b1;

            // If either interval is circular and contains zero, result is (-inf, inf)
            bool firstContainsZero = (a0 <= 0 && 0 <= a1) || firstCircular;
            bool secondContainsZero = (b0 <= 0 && 0 <= b1) || secondCircular;
            if ((firstCircular && secondContainsZero) || (secondCircular && firstContainsZero)) {
                return new CircularInterval(Vector128.Create(double.NegativeInfinity, double.PositiveInfinity));
            }

            // If both intervals are not circular, return [min, max]
            if (!firstCircular && !secondCircular) {
                return new CircularInterval(Vector128.Create(min, max));
            }

            // If one or both are circular, result is circular: [max, min]
            return new CircularInterval(Vector128.Create(max, min));

        }

        // INumber<CircularInterval> implementation

        public static CircularInterval Clamp(CircularInterval value, CircularInterval min, CircularInterval max)
            => throw new NotImplementedException();

        public static CircularInterval CopySign(CircularInterval value, CircularInterval sign)
            => throw new NotImplementedException();

        public static CircularInterval Max(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        public static CircularInterval MaxNumber(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        public static CircularInterval Min(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        public static CircularInterval MinNumber(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        public static int Sign(CircularInterval value)
            => throw new NotImplementedException();

        // INumberBase<CircularInterval> members (required by INumber)
        public static CircularInterval One => new CircularInterval(Vector128<double>.One);

        // static int INumberBase<CircularInterval>.Radix => 2;

        public static CircularInterval Zero => new CircularInterval(Vector128<double>.Zero);

        /*
        static bool INumberBase<CircularInterval>.IsCanonical(CircularInterval value) => true;
        static bool INumberBase<CircularInterval>.IsComplexNumber(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsEvenInteger(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsFinite(CircularInterval value) => true;
        static bool INumberBase<CircularInterval>.IsImaginaryNumber(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsInfinity(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsInteger(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsNaN(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsNegative(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsNegativeInfinity(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsNormal(CircularInterval value) => true;
        static bool INumberBase<CircularInterval>.IsOddInteger(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsPositive(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsPositiveInfinity(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsRealNumber(CircularInterval value) => true;
        static bool INumberBase<CircularInterval>.IsSubnormal(CircularInterval value) => false;
        static bool INumberBase<CircularInterval>.IsZero(CircularInterval value) => Vector128<double>.Zero == value.b;

        static CircularInterval INumberBase<CircularInterval>.Abs(CircularInterval value)
            => throw new NotImplementedException();

        static CircularInterval INumberBase<CircularInterval>.MaxMagnitude(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        static CircularInterval INumberBase<CircularInterval>.MaxMagnitudeNumber(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        static CircularInterval INumberBase<CircularInterval>.MinMagnitude(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        static CircularInterval INumberBase<CircularInterval>.MinMagnitudeNumber(CircularInterval x, CircularInterval y)
            => throw new NotImplementedException();

        static CircularInterval INumberBase<CircularInterval>.MultiplyAddEstimate(CircularInterval first, CircularInterval second, CircularInterval addend)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertFromChecked<TOther>(TOther value, out CircularInterval result)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertFromSaturating<TOther>(TOther value, out CircularInterval result)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertFromTruncating<TOther>(TOther value, out CircularInterval result)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertToChecked<TOther>(CircularInterval value, out TOther result)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertToSaturating<TOther>(CircularInterval value, out TOther result)
            => throw new NotImplementedException();

        static bool INumberBase<CircularInterval>.TryConvertToTruncating<TOther>(CircularInterval value, out TOther result)
            => throw new NotImplementedException();
        */

    }

    [Obsolete]
    public readonly struct CircularIntervalF<T> where T : IFloatingPoint<T> {

        readonly T b0;

        readonly T b1;
    }
}
