using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.CompilerServices.Interfaces.Typed_RefReturn_Wrapped_Huge;
using UltimateOrb.Runtime.InteropServices;

namespace UltimateOrb {
    using UltimateOrb.Runtime.CompilerServices;

    public interface IReadOnlyMemoryReference<T> : IMemoryReference, IReadOnlyStrongBox<T> {

        ref readonly T UltimateOrb.Runtime.CompilerServices.Interfaces.RefReturn.IReadOnlyStrongBox<T>.Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get {
                unsafe {
                    var obj = PinnableReference;
                    fixed (void* pData = &Unsafe.As<ObjectRawView>(obj).Data) {
                        return ref Unsafe.AsRef<T>(unchecked((void*)(nuint)(ObjectReferenceUnsafe.AsIntPtr(obj) + ByteOffset)));
                    }
                }
            }
        }
    }
}
