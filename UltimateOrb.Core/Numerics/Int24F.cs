using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Numerics {

    using static ConstructorTags;

    public static partial class ConstructorTags {

        public readonly struct Checked {
        }

        public readonly struct Unchecked {
        }

        public readonly struct Plain {
        }
    }

    public readonly struct Int24F {

        readonly float _Value;

        internal Int24F(float value) {
            _Value = value;
        }

        const float _MinValueF = -1 << 23;
        const float _MaxValueF = -1 - _MinValueF;


        public Int24F(float value, Checked _) {
            if (value <= _MaxValueF && _MinValueF <= value) {
                _Value = value;
                return;
            }
            throw new OverflowException();
        }

        public Int32 Value {

            get => unchecked((Int32)_Value);
        }

        public static Int24F operator +(Int24F first, Int24F second) {
            return new Int24F(first._Value + second._Value, default(Checked));
        }


        public static Int24F operator -(Int24F first, Int24F second) {
            return new Int24F(first._Value - second._Value, default(Checked));
        }

        public static Int24F operator *(Int24F first, Int24F second) {
            return new Int24F(first._Value * second._Value, default(Checked));
        }

        public static Int24F operator /(Int24F first, Int24F second) {
            return new Int24F(first._Value / second._Value, default(Checked));
        }

        public static explicit operator Int24F(int value) {
            var v = (float)value;
            return new Int24F(v, default(Checked));
        }
    }
}
