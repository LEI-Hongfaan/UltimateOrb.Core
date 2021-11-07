using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.InteropServices;

namespace UltimateOrb.Extensions {
    using UltimateOrb.Runtime.CompilerServices;
    using static ThrowHelper;
    using static MemorySpanRuntimeHelpers;

    public static class FieldOffsetMemoryReferenceExtensions {

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static FieldOffsetMemoryReference<T> GetMemoryReference<T>(this T[] array, nint index) {
            if (array is null) {
                throw ThrowArgumentNullException_array();
            }
            unsafe {
                fixed (byte* pData = &Unsafe.As<ObjectRawView>(array).Data) {
                    return new FieldOffsetMemoryReference<T>(array, unchecked((nint)Unsafe.AsPointer(ref array[index]) - (nint)pData));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static FieldOffsetMemoryReference<T> GetMemoryReference<T>(this object? obj, FieldInfo field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            if (field is null) {
                throw ThrowArgumentNullException_field();
            }
            CheckType<T>(field);
            CheckInstance(field);
            CheckMutable(field);
            return new FieldOffsetMemoryReference<T>(obj, field.GetFieldOffset());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static FieldOffsetMemoryReference<T> GetMemoryReference<T>(this object? obj, ref T field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            if (Unsafe.IsNullRef(ref field)) {
                throw ThrowArgumentNullException_field();
            }
            unsafe {
                fixed (byte* pData = &Unsafe.As<ObjectRawView>(obj).Data) {
                    var offset = DangerousGetPinnedObjectFieldOffset(obj, ref Unsafe.AsRef(in field));
                    if (DangerousGetPinnedObjectDataSizeCore(obj) <= offset) {
                        throw new ArgumentException("Referenced field does not belong to the object.");
                    }
                    return new FieldOffsetMemoryReference<T>(obj, offset);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static FieldOffsetMemoryReference<T> GetMemoryReference<T>(FieldInfo field) {
            if (field is null) {
                throw new ArgumentNullException(nameof(field));
            }
            CheckType<T>(field);
            CheckStatic(field);
            CheckMutable(field);
            return new FieldOffsetMemoryReference<T>(null, field.GetStaticFieldAddress());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static ReadOnlyFieldOffsetMemoryReference<T> GetReadOnlyMemoryReference<T>(this T[] array, nint index) {
            if (array is null) {
                throw ThrowArgumentNullException_array();
            }
            unsafe {
                fixed (byte* pData = &Unsafe.As<ObjectRawView>(array).Data) {
                    return new ReadOnlyFieldOffsetMemoryReference<T>(array, unchecked((nint)Unsafe.AsPointer(ref array[index]) - (nint)pData));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ReadOnlyFieldOffsetMemoryReference<T> GetReadOnlyMemoryReference<T>(this object? obj, FieldInfo field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            if (field is null) {
                throw ThrowArgumentNullException_field();
            }
            CheckType<T>(field);
            CheckInstance(field);
            return new ReadOnlyFieldOffsetMemoryReference<T>(obj, field.GetFieldOffset());
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ReadOnlyFieldOffsetMemoryReference<T> GetReadOnlyMemoryReference<T>(this object? obj, in T field) {
            if (obj is null) {
                throw ThrowArgumentNullException_obj();
            }
            if (Unsafe.IsNullRef(ref Unsafe.AsRef(in field))) {
                throw ThrowArgumentNullException_field();
            }
            unsafe {
                fixed (byte* pData = &Unsafe.As<ObjectRawView>(obj).Data) {
                    var offset = DangerousGetPinnedObjectFieldOffset(obj, ref Unsafe.AsRef(in field));
                    if (DangerousGetPinnedObjectDataSizeCore(obj) <= offset) {
                        throw new ArgumentException("Referenced field does not belong to the object.");
                    }
                    return new ReadOnlyFieldOffsetMemoryReference<T>(obj, offset);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static ReadOnlyFieldOffsetMemoryReference<T> GetReadOnlyMemoryReference<T>(FieldInfo field) {
            if (field is null) {
                throw new ArgumentNullException(nameof(field));
            }
            CheckType<T>(field);
            CheckStatic(field);
            return new ReadOnlyFieldOffsetMemoryReference<T>(null, field.GetStaticFieldAddress());
        }
    }
}
