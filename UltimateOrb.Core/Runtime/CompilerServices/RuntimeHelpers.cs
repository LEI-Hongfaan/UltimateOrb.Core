using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb.Runtime.CompilerServices {

    public class RuntimeHelpers {

        public static nint GetObjectDataSize(object obj) {
            if (obj is null) {
                throw new ArgumentNullException(nameof(obj));
            }
            return GetObjectDataSizeCore(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private static nint GetObjectDataSizeCore(object obj) {
            ref var length = ref Unsafe.As<VariableLengthObjectRawView>(obj).Length;
            unsafe {
                fixed (nint* pLength = &length) {
                    GCMethodTable* pMT = ObjectReferenceUnsafe.AsPointer<GCObject>(obj)->m_pMethTab;
                    return unchecked((int)pMT->m_baseSize - 2 * CilVerifiable.SizeOf<nint>() + (nint)pMT->GetComponentSize() * length);
                }
            }
        }
    }
}
