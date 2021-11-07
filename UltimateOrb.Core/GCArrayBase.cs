using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

    internal unsafe readonly struct GCArrayBase {

        public readonly GCObject m_base;

        public GCMethodTable* m_pMethTab {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_base.m_pMethTab;
        }

        public readonly UInt32 m_dwLength;

        public UInt32 GetNumComponents() {
            return m_dwLength;
        }

        static readonly nuint s_OffsetOfNumComponents = GetOffsetOfNumComponentsCore();

        static nuint GetOffsetOfNumComponentsCore() {
            GCArrayBase value;
            Unsafe.SkipInit(out value);
            return unchecked((nuint)Unsafe.AsPointer(ref value) - (nuint)Unsafe.AsPointer(ref Unsafe.AsRef(in value.m_dwLength)));
        }

        public static nuint GetOffsetOfNumComponents() {
            return s_OffsetOfNumComponents;
        }
    }
}
