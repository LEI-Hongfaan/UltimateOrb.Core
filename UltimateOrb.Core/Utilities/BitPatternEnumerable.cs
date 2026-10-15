using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Mathematics;

namespace UltimateOrb.Utilities {
    public static partial class BitPatternEnumerable {

        #region Obsolete

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt32Bits"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt32Bits)} instead.")]
        public static IEnumerable<UInt32> GetUInt32Bits(int bitLength, int popCount)
            => EnumerateUInt32Bits(bitLength, popCount);

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt32BitsUpTo"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt32BitsUpTo)} instead.")]
        public static IEnumerable<UInt32> GetUInt32BitsWithPopCountLessThanOrEqual(int bitLength, int popCount)
            => EnumerateUInt32BitsUpTo(bitLength, popCount);

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt64Bits"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt64Bits)} instead.")]
        public static IEnumerable<UInt64> GetUInt64Bits(int bitLength, int popCount)
            => EnumerateUInt64Bits(bitLength, popCount);

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt64BitsUpTo"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt64BitsUpTo)} instead.")]
        public static IEnumerable<UInt64> GetUInt64BitsWithPopCountLessThanOrEqual(int bitLength, int popCount)
            => EnumerateUInt64BitsUpTo(bitLength, popCount);

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt128Bits"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt128Bits)} instead.")]
        public static IEnumerable<UInt128> GetUInt128Bits(int bitLength, int popCount)
            => EnumerateUInt128Bits(bitLength, popCount);

        /// <summary>
        /// Legacy alias of <see cref="EnumerateUInt128BitsUpTo"/>.
        /// </summary>
        [Obsolete($"Use {nameof(EnumerateUInt128BitsUpTo)} instead.")]
        public static IEnumerable<UInt128> GetUInt128BitsWithPopCountLessThanOrEqual(int bitLength, int popCount)
            => EnumerateUInt128BitsUpTo(bitLength, popCount);

        #endregion

        /// <summary>
        /// Enumerates all 32-bit patterns with exactly <paramref name="popCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt32"/>.
        /// </summary>
        /// <param name="maxBitLength">
        /// Number of low bits used as the field width. Must be in [0, 32]. A value of 0 is
        /// only valid together with <paramref name="popCount"/> = 0 and yields a single 0.
        /// </param>
        /// <param name="popCount">Number of set bits. Must be in [0, <paramref name="maxBitLength"/>].</param>
        /// <returns>
        /// The patterns in ascending lexicographical order, from the low-bit-packed form
        /// up to the high-bit-packed form. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt32> EnumerateUInt32Bits(int maxBitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 32);
            if (1 <= maxBitLength && 0 <= popCount && popCount <= maxBitLength) {
                foreach (var v in EnumerateUInt32BitsCore(maxBitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (maxBitLength | popCount)) {
                yield return default;
            }
        }

        /// <summary>
        /// Enumerates all 32-bit patterns with at most <paramref name="maxPopCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt32"/>.
        /// </summary>
        /// <param name="maxBitLength">Number of low bits used as the field width. Must be in [0, 32].</param>
        /// <param name="maxPopCount">Maximum number of set bits. Values ≥ <paramref name="maxBitLength"/> are clamped.</param>
        /// <returns>
        /// The zero pattern followed by the groups for popcount 1..min(<paramref name="maxBitLength"/>, <paramref name="maxPopCount"/>),
        /// each group in ascending lexicographical order. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt32> EnumerateUInt32BitsUpTo(int maxBitLength, int maxPopCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 32);
            if (1 <= maxBitLength && 0 <= maxPopCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(maxBitLength, maxPopCount); ++i) {
                    foreach (var item in EnumerateUInt32BitsCore(maxBitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (maxBitLength | maxPopCount)) {
                yield return default;
            }
        }

        static IEnumerable<UInt32> EnumerateUInt32BitsCore(int maxBitLength, int popCount) {
            Debug.Assert(maxBitLength > 0);
            Debug.Assert(maxBitLength <= 32);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= maxBitLength);
            var a = unchecked(((UInt32)1 << popCount) - 1);
            var b = a << unchecked(maxBitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = BinaryNumerals.NextPermutation(a);
            }
        }

        /// <summary>
        /// Enumerates all 64-bit patterns with exactly <paramref name="popCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt64"/>.
        /// </summary>
        /// <param name="maxBitLength">
        /// Number of low bits used as the field width. Must be in [0, 64]. A value of 0 is
        /// only valid together with <paramref name="popCount"/> = 0 and yields a single 0.
        /// </param>
        /// <param name="popCount">Number of set bits. Must be in [0, <paramref name="maxBitLength"/>].</param>
        /// <returns>
        /// The patterns in ascending lexicographical order, from the low-bit-packed form
        /// up to the high-bit-packed form. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt64> EnumerateUInt64Bits(int maxBitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 64);
            if (1 <= maxBitLength && 0 <= popCount && popCount <= maxBitLength) {
                foreach (var v in EnumerateUInt64BitsCore(maxBitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (maxBitLength | popCount)) {
                yield return default;
            }
        }

        /// <summary>
        /// Enumerates all 64-bit patterns with at most <paramref name="maxPopCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt64"/>.
        /// </summary>
        /// <param name="maxBitLength">Number of low bits used as the field width. Must be in [0, 64].</param>
        /// <param name="maxPopCount">Maximum number of set bits. Values ≥ <paramref name="maxBitLength"/> are clamped.</param>
        /// <returns>
        /// The zero pattern followed by the groups for popcount 1..min(<paramref name="maxBitLength"/>, <paramref name="maxPopCount"/>),
        /// each group in ascending lexicographical order. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt64> EnumerateUInt64BitsUpTo(int maxBitLength, int maxPopCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 64);
            if (1 <= maxBitLength && 0 <= maxPopCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(maxBitLength, maxPopCount); ++i) {
                    foreach (var item in EnumerateUInt64BitsCore(maxBitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (maxBitLength | maxPopCount)) {
                yield return default;
            }
        }

        static IEnumerable<UInt64> EnumerateUInt64BitsCore(int maxBitLength, int popCount) {
            Debug.Assert(maxBitLength > 0);
            Debug.Assert(maxBitLength <= 64);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= maxBitLength);
            var a = unchecked(((UInt64)1 << popCount) - 1);
            var b = a << unchecked(maxBitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = BinaryNumerals.NextPermutation(a);
            }
        }

        /// <summary>
        /// Enumerates all 128-bit patterns with exactly <paramref name="popCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt128"/>.
        /// </summary>
        /// <param name="maxBitLength">
        /// Number of low bits used as the field width. Must be in [0, 128]. A value of 0 is
        /// only valid together with <paramref name="popCount"/> = 0 and yields a single 0.
        /// </param>
        /// <param name="popCount">Number of set bits. Must be in [0, <paramref name="maxBitLength"/>].</param>
        /// <returns>
        /// The patterns in ascending lexicographical order, from the low-bit-packed form
        /// up to the high-bit-packed form. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt128> EnumerateUInt128Bits(int maxBitLength, int popCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 128);
            if (1 <= maxBitLength && 0 <= popCount && popCount <= maxBitLength) {
                foreach (var v in EnumerateUInt128BitsCore(maxBitLength, popCount)) {
                    yield return v;
                }
            } else if (0 == (maxBitLength | popCount)) {
                yield return default;
            }
        }

        /// <summary>
        /// Enumerates all 128-bit patterns with at most <paramref name="maxPopCount"/> bits set,
        /// using the lowest <paramref name="maxBitLength"/> bits of a <see cref="UInt128"/>.
        /// </summary>
        /// <param name="maxBitLength">Number of low bits used as the field width. Must be in [0, 128].</param>
        /// <param name="maxPopCount">Maximum number of set bits. Values ≥ <paramref name="maxBitLength"/> are clamped.</param>
        /// <returns>
        /// The zero pattern followed by the groups for popcount 1..min(<paramref name="maxBitLength"/>, <paramref name="maxPopCount"/>),
        /// each group in ascending lexicographical order. Empty if the parameter combination is invalid.
        /// </returns>
        public static IEnumerable<UInt128> EnumerateUInt128BitsUpTo(int maxBitLength, int maxPopCount) {
            ArgumentOutOfRangeException.ThrowIfNegative(maxBitLength);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(maxBitLength, 128);
            if (1 <= maxBitLength && 0 <= maxPopCount) {
                yield return 0;
                for (var i = 1; i <= Math.Min(maxBitLength, maxPopCount); ++i) {
                    foreach (var item in EnumerateUInt128BitsCore(maxBitLength, i)) {
                        yield return item;
                    }
                }
            } else if (0 == (maxBitLength | maxPopCount)) {
                yield return default;
            }
        }

        static IEnumerable<UInt128> EnumerateUInt128BitsCore(int maxBitLength, int popCount) {
            Debug.Assert(maxBitLength > 0);
            Debug.Assert(maxBitLength <= 128);
            Debug.Assert(0 <= popCount);
            Debug.Assert(popCount <= maxBitLength);
            var a = unchecked(((UInt128)1 << popCount) - 1);
            var b = a << unchecked(maxBitLength - popCount);
            for (; ; ) {
                yield return a;
                if (a == b) {
                    break;
                }
                a = UInt128.BinaryNumerals.NextPermutation(a);
            }
        }
    }
}
