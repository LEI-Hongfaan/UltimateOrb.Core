using System;

namespace UltimateOrb.Numerics {

    using static ConstructorTags;

    public readonly struct Int24I {

        readonly Int32 _Value;

        internal Int24I(Int32 value) {
            _Value = value;
        }
        const Int32 _MinValueI = -1 << 23;
        const Int32 _MaxValueI = -1 - _MinValueI;


        public Int24I(Int32 value, Checked _) {
            if (value <= _MaxValueI && _MinValueI <= value) {
                _Value = value;
                return;
            }
            throw new OverflowException();
        }

        public static Int24I operator +(Int24I first, Int24I second) {
            return new Int24I(first._Value + second._Value, default(Checked));
        }

        public static Int24I operator -(Int24I first, Int24I second) {
            return new Int24I(first._Value - second._Value, default(Checked));
        }

        public static Int24I operator *(Int24I first, Int24I second) {
            return new Int24I(first._Value * second._Value, default(Checked));
        }

        public static Int24I operator /(Int24I first, Int24I second) {
            return new Int24I(first._Value / second._Value, default(Checked));
        }

        public static Int24I BigMul(Int24I first, Int24I second, out UInt24I highResult) {
            var a = Math.BigMul(first._Value, second._Value);
            throw new NotImplementedException();
        }
    }
}
