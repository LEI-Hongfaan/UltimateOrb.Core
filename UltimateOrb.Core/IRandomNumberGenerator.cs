using System;

namespace UltimateOrb {

    public partial interface IRandomNumberGenerator {

        Int32 GetNextInt32(Int32 maxExclusive);

        Int32 GetNextInt32Bits();

        Int64 GetNextInt64Bits();

        void GetBytes(byte[] array, int offset, int count);
    }
}
