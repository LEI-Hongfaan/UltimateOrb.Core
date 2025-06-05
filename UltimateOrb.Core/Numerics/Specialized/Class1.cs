using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Unsafe = System.Runtime.CompilerServices.Unsafe;

namespace UltimateOrb.Numerics.Specialized {

    public readonly struct BitMatrix16x16 {

        // The 256 bits are arranged in 16 lanes of UInt16,
        // where each lane represents one row (16 bits) of our 16×16 grid.
        private readonly Vector256<UInt16> _data;

        /// <summary>
        /// Initializes an instance with the provided SIMD vector.
        /// </summary>
        public BitMatrix16x16(Vector256<UInt16> data) => _data = data;

        /// <summary>
        /// Returns an instance with all bits cleared.
        /// </summary>
        public static BitMatrix16x16 Empty => new(Vector256<UInt16>.Zero);

        /// <summary>
        /// Provides read‑only two‑dimensional access to bits using row and column indices.
        /// Indices must be in the range 0..15.
        /// </summary>
        public bool this[int row, int col] {
            get {
                if (row is < 0 or >= 16 || col is < 0 or >= 16) {
                    throw new IndexOutOfRangeException("Indices must be in the range [0, 15].");
                }

                UInt16 rowValue = _data.GetElement(row);
                return ((rowValue >> col) & 1) != 0;
            }
        }

        /// <summary>
        /// Returns a new BitMatrix16x16 with the specified bit updated.
        /// </summary>
        public BitMatrix16x16 WithBit(int row, int col, bool value) {
            if (row is < 0 or >= 16 || col is < 0 or >= 16) {
                throw new IndexOutOfRangeException("Indices must be in the range [0, 15].");
            }

            UInt16 rowValue = _data.GetElement(row);
            UInt16 mask = (UInt16)(1 << col);
            UInt16 newRowValue = value ? (UInt16)(rowValue | mask) : (UInt16)(rowValue & ~mask);
            Vector256<UInt16> newData = _data.WithElement(row, newRowValue);
            return new BitMatrix16x16(newData);
        }

        // — Column shifting operators (default direction) —
        // Shifting columns means that each row's bits move left/right.
        public static BitMatrix16x16 operator <<(BitMatrix16x16 m, int shift) {
            // Shift each UInt16 lane left (columns become shifted).
            return new BitMatrix16x16(m._data << shift);
        }

        public static BitMatrix16x16 operator >>(BitMatrix16x16 m, int shift) {
            // Shift each UInt16 lane right.
            return new BitMatrix16x16(m._data >> shift);
        }

        public static BitMatrix16x16 operator >>>(BitMatrix16x16 m, int shift) {
            return m >> shift;
        }

        // — Row shifting operators —
        // The overloads below (with RowShiftCount<int>) move complete rows.
        // For operator <<, rows are shifted upward (rows at higher index move to lower index).
        public static BitMatrix16x16 operator <<(BitMatrix16x16 m, RowShiftCount<int> shift) {
            int count = shift.Value << 1;

            if (Avx2.IsSupported) {
                var t = Avx2.ShiftLeftLogical128BitLane(m._data, unchecked((byte)(0X0F & count)));
                if ((16 & count) != 0) {
                    return new BitMatrix16x16(Avx2.Permute2x128(t, t, 0X08));
                }
                var r = Avx2.ShiftRightLogical128BitLane(m._data, unchecked((byte)(0X0F & -count)));
                var r1 = Avx2.Permute2x128(r, r, 0X08);
                var result = Avx2.Or(t, r1);
                return new BitMatrix16x16(result);
            }

            Debug.Assert(BitConverter.IsLittleEndian);

            UInt256 u = Unsafe.As<BitMatrix16x16, UInt256>(ref m);
            u <<= (count << 3);
            return Unsafe.As<UInt256, BitMatrix16x16>(ref u);
        }

        // For operator >>, rows are shifted downward.
        public static BitMatrix16x16 operator >>(BitMatrix16x16 m, RowShiftCount<int> shift) {
            int count = shift.Value << 1;

            if (Avx2.IsSupported) {
                var t = Avx2.ShiftRightLogical128BitLane(m._data, unchecked((byte)(0X0F & count)));
                if ((16 & count) != 0) {
                    return new BitMatrix16x16(Avx2.Permute2x128(t, t, 0X81));
                }
                var r = Avx2.ShiftLeftLogical128BitLane(m._data, unchecked((byte)(0X0F & -count)));
                var r1 = Avx2.Permute2x128(r, r, 0X81);
                var result = Avx2.Or(t, r1);
                return new BitMatrix16x16(result);
            }

            Debug.Assert(BitConverter.IsLittleEndian);

            UInt256 u = Unsafe.As<BitMatrix16x16, UInt256>(ref m);
            u <<= (count << 3);
            return Unsafe.As<UInt256, BitMatrix16x16>(ref u);
        }

        public static BitMatrix16x16 operator >>>(BitMatrix16x16 m, RowShiftCount<int> shift) {
            return m >> shift;
        }

        public static BitMatrix16x16 operator &(BitMatrix16x16 left, BitMatrix16x16 right) {
            return new BitMatrix16x16(left._data & right._data);
        }

        public static BitMatrix16x16 operator |(BitMatrix16x16 left, BitMatrix16x16 right) {
            return new BitMatrix16x16(left._data | right._data);
        }

        public static BitMatrix16x16 operator ^(BitMatrix16x16 left, BitMatrix16x16 right) {
            return new BitMatrix16x16(left._data ^ right._data);
        }

        public static BitMatrix16x16 operator ~(BitMatrix16x16 value) {
            return new BitMatrix16x16(~value._data);
        }

        /// <summary>
        /// Returns a string representation of the 16×16 bit array.
        /// </summary>
        public override string ToString() {
            var builder = new StringBuilder(16 * 17);
            for (int row = 0; row < 16; row++) {
                UInt16 rowValue = _data.GetElement(row);
                // Render bits from most-significant (left) to least-significant (right).
                for (int col = 15; col >= 0; col--) {
                    builder.Append(((rowValue >> col) & 1) != 0 ? '1' : '0');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}
