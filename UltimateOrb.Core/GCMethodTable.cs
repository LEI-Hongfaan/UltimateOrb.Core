using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb {
    internal unsafe readonly struct GCMethodTable {

        public readonly UInt32 m_flags;

        public readonly UInt32 m_baseSize;

        public readonly GCMethodTable* m_pRelatedType;

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public nuint GetComponentSize() {
            return unchecked((nuint)(nint)(((Int32)m_flags >> 31) & (UInt16.MaxValue & (Int32)m_flags)));
        }
    }
}
