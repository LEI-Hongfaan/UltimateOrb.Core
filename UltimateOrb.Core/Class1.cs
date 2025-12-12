using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb;
using UltimateOrb.Collections;

namespace UltimateOrb {

    public static partial class ShiftCountExtensions {

        public static RowShiftCount<T> AsRowShiftCount<T>(this T value) {
            return new RowShiftCount<T>(value);
        }
    }

    /// <summary>
    /// A strongly typed shift count for row shifts. This generic struct leverages
    /// generic math (via INumber{T}) and allows for implicit conversion from the numeric type.
    /// </summary>
    public readonly struct RowShiftCount<T> {

        public readonly T Value;

        public RowShiftCount(T value) => Value = value;

        /// <inheritdoc/>
        public override string ToString() => Value.ToString()!;

        public static explicit operator RowShiftCount<T>(T value) => new RowShiftCount<T>(value);

        public static explicit operator T(RowShiftCount<T> value) => value.Value;
    }
}
