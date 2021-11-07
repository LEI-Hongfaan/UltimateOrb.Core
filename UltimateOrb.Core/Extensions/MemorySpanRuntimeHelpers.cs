using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.InteropServices;
using UltimateOrb.Utilities;

namespace UltimateOrb.Extensions {
    using UltimateOrb.Runtime.CompilerServices;

    using static ThrowHelper;

    public static class MemorySpanRuntimeHelpers {

        /// <summary>
        /// Gets the total instance field size (including padding) in bytes.
        /// </summary>
        /// <param name="obj">The pinned object.</param>
        /// <returns></returns>
        public static nint DangerousGetPinnedObjectDataSize(object obj) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            return DangerousGetPinnedObjectDataSizeCore(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static nint DangerousGetPinnedObjectDataSizeCore(object obj) {
            unsafe {
                var p = ObjectReferenceUnsafe.AsPointer<GCArrayBase>(obj);
                GCMethodTable* pMT = p->m_base.m_pMethTab;
                return unchecked((int)pMT->m_baseSize - 2 * CilVerifiable.SizeOf<nint>() + (nint)pMT->GetComponentSize() * (nint)(nuint)p->m_dwLength);
            }
        }

        public static nint DangerousGetObjectDataSize(object obj) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            return DangerousGetObjectDataSizeCore(obj);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static nint DangerousGetObjectDataSizeCore(object obj) {
            unsafe {
                fixed (byte* pData = &Unsafe.As<ObjectRawView>(obj).Data) {
                    return DangerousGetPinnedObjectDataSizeCore(obj);
                }
            }
        }

        public static nint DangerousGetObjectFieldOffset<T>(object obj, ref T field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            return DangerousGetObjectFieldOffsetCore(obj, ref field);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static nint DangerousGetObjectFieldOffsetCore<T>(object? obj, ref T field) {
            unsafe {
                fixed (byte* pOffsetBase = &Unsafe.As<ObjectRawView>(obj).Data)
                fixed (byte* pField = &Unsafe.As<T, byte>(ref field)) {
                    return unchecked((nint)pField - (nint)pOffsetBase);
                }
            }
        }

        public static nint DangerousGetPinnedObjectFieldOffset<T>(object obj, ref T field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            return DangerousGetPinnedObjectFieldOffsetCore(obj, ref field);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static nint DangerousGetPinnedObjectFieldOffsetCore<T>(object obj, ref T field) {
            unsafe {
                fixed (byte* pField = &Unsafe.As<T, byte>(ref field)) {
                    return unchecked((nint)pField - (nint)Unsafe.AsPointer(ref Unsafe.As<ObjectRawView>(obj).Data));
                }
            }
        }

        public static int GetFieldOffset(this FieldInfo field) {
            // TODO: Caching
            return GetFieldOffsetCore(field);
        }

        private static readonly FieldInfo _FieldOffsetBase = typeof(ObjectRawView).GetField(nameof(ObjectRawView.Data))!;

        internal static int GetFieldOffsetCore(this FieldInfo field) {
            if (field is null) {
                throw new ArgumentNullException(nameof(field));
            }
            var m = new DynamicMethod("", typeof(int), Type.EmptyTypes);
            var t = field.DeclaringType!;
            var ilg = m.GetILGenerator();
            var v = ilg.DeclareLocal(t);
            ilg.Emit(OpCodes.Ldloca_S, v);
            ilg.Emit(OpCodes.Ldflda, field);
            ilg.Emit(OpCodes.Ldloca_S, v);
            if (!t.IsValueType) {
                ilg.Emit(OpCodes.Ldflda, _FieldOffsetBase);
            }
            ilg.Emit(OpCodes.Sub);
            ilg.Emit(OpCodes.Ret);
            var w = (m.CreateDelegate(typeof(Func<int>)) as Func<int>)!;
            return w.Invoke();
        }

        internal static nint GetStaticFieldAddress(this FieldInfo field) {
            if (field is null) {
                throw new ArgumentNullException(nameof(field));
            }
            if (!field.IsStatic) {
                throw new ArgumentException("Specified field must be static.", nameof(field));
            }
            // TODO: Caching
            return GetStaticFieldAddressCore(field);
        }

        private static nint GetStaticFieldAddressCore(this FieldInfo field) {
            var m = new DynamicMethod("", typeof(nint), Type.EmptyTypes);
            var ilg = m.GetILGenerator();
            ilg.Emit(OpCodes.Ldsflda, field);
            ilg.Emit(OpCodes.Ret);
            var w = (m.CreateDelegate(typeof(Func<nint>)) as Func<nint>)!;
            return w.Invoke();
        }
    }
}
