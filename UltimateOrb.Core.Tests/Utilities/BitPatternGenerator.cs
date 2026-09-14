using System;
using System.Collections.Generic;
using System.Numerics;
using UltimateOrb;
using UltimateOrb.Utilities;
using UInt128 = System.UInt128;

public static class BitPatternGenerator {

    public static IEnumerable<uint> GenerateBinary32(
        int expMaxWeight = 4,
        int manMaxWeight = 6) {
        const int EXP_BITS = 8;
        const int MAN_BITS = 23;

        expMaxWeight = Math.Min(expMaxWeight, EXP_BITS / 2);
        manMaxWeight = Math.Min(manMaxWeight, MAN_BITS / 2);

        foreach (uint e in BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(EXP_BITS, expMaxWeight))
            foreach (uint m in BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(MAN_BITS, manMaxWeight)) {
                var mN = ((1u << MAN_BITS) - 1u) & ~m;
                var eN = ((1u << EXP_BITS) - 1u) & ~e;

                yield return (e << MAN_BITS) | m;
                yield return (e << MAN_BITS) | mN;
                yield return (eN << MAN_BITS) | m;
                yield return (eN << MAN_BITS) | mN;
                yield return 0x80000000u | (e << MAN_BITS) | m;
                yield return 0x80000000u | (e << MAN_BITS) | mN;
                yield return 0x80000000u | (eN << MAN_BITS) | m;
                yield return 0x80000000u | (eN << MAN_BITS) | mN;
            }
    }

    public static IEnumerable<Single> GenerateSingle(
        int expMaxWeight = 4,
        int manMaxWeight = 6) {
        foreach (var bits in GenerateBinary32(expMaxWeight, manMaxWeight)) {
            yield return BitConverter.UInt32BitsToSingle(bits);
        }
    }

    public static IEnumerable<ulong> GenerateBinary64(
        int expMaxWeight = 5,
        int manMaxWeight = 3) {
        const int EXP_BITS = 11;
        const int MAN_BITS = 52;

        expMaxWeight = Math.Min(expMaxWeight, EXP_BITS / 2);
        manMaxWeight = Math.Min(manMaxWeight, MAN_BITS / 2);

        foreach (uint e in BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(EXP_BITS, expMaxWeight))
            foreach (ulong m in BitPatternEnumerable.GetUInt64BitsWithPopCountLessThanOrEqual(MAN_BITS, manMaxWeight)) {
                var eN = ((1u << EXP_BITS) - 1u) & ~e;
                var mN = ((1ul << MAN_BITS) - 1ul) & ~m;

                yield return ((ulong)e << MAN_BITS) | m;
                yield return ((ulong)e << MAN_BITS) | mN;
                yield return ((ulong)eN << MAN_BITS) | m;
                yield return ((ulong)eN << MAN_BITS) | mN;
                yield return 0x8000000000000000UL | ((ulong)e << MAN_BITS) | m;
                yield return 0x8000000000000000UL | ((ulong)e << MAN_BITS) | mN;
                yield return 0x8000000000000000UL | ((ulong)eN << MAN_BITS) | m;
                yield return 0x8000000000000000UL | ((ulong)eN << MAN_BITS) | mN;
            }
    }

    public static IEnumerable<double> GenerateDouble(
        int expMaxWeight = 5,
        int manMaxWeight = 3) {
        foreach (var bits in GenerateBinary64(expMaxWeight, manMaxWeight)) {
            yield return BitConverter.UInt64BitsToDouble(bits);
        }
    }

    public static IEnumerable<UInt128> GenerateBinary128(
        int expMaxWeight = 4,
        int manMaxWeight = 2) {
        const int EXP_BITS = 15;
        const int MAN_BITS = 112;
        const int SIGN_SHIFT = EXP_BITS + MAN_BITS; // 127

        expMaxWeight = Math.Min(expMaxWeight, EXP_BITS / 2);
        manMaxWeight = Math.Min(manMaxWeight, MAN_BITS / 2);

        foreach (uint e in BitPatternEnumerable.GetUInt32BitsWithPopCountLessThanOrEqual(EXP_BITS, expMaxWeight))
            foreach (UInt128 m in BitPatternEnumerable.GetUInt128BitsWithPopCountLessThanOrEqual(MAN_BITS, manMaxWeight)) {
                var eN = ((1u << EXP_BITS) - 1u) & ~e;
                var mN = (((UInt128)1 << MAN_BITS) - (UInt128)1) & ~m;

                yield return ((UInt128)e << MAN_BITS) | m;
                yield return ((UInt128)e << MAN_BITS) | mN;
                yield return ((UInt128)eN << MAN_BITS) | m;
                yield return ((UInt128)eN << MAN_BITS) | mN;
                yield return ((UInt128)1 << SIGN_SHIFT) | ((UInt128)e << MAN_BITS) | m;
                yield return ((UInt128)1 << SIGN_SHIFT) | ((UInt128)e << MAN_BITS) | mN;
                yield return ((UInt128)1 << SIGN_SHIFT) | ((UInt128)eN << MAN_BITS) | m;
                yield return ((UInt128)1 << SIGN_SHIFT) | ((UInt128)eN << MAN_BITS) | mN;
            }
    }

#pragma warning disable UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
    public static IEnumerable<Quadruple> GenerateQuadruple(
        int expMaxWeight = 4,
        int manMaxWeight = 2) {
        foreach (var bits in GenerateBinary128(expMaxWeight, manMaxWeight)) {
            yield return BitConverter.UInt128BitsToQuadruple(bits);
        }
    }
#pragma warning restore UoWIP // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
}