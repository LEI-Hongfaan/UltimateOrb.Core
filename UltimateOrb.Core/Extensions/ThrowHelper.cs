using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Extensions {

    internal static class ThrowHelper {

        [DoesNotReturn]
        public static ArgumentNullException ThrowArgumentNullException_array() {
            throw new ArgumentNullException("array");
        }

        [DoesNotReturn]
        public static ArgumentNullException ThrowArgumentNullException_field() {
            throw new ArgumentNullException("field");
        }

        [DoesNotReturn]
        public static ArgumentNullException ThrowArgumentNullException_obj() {
            throw new ArgumentNullException("obj");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckInstance(FieldInfo field) {
            if (field.IsStatic) {
                throw new ArgumentException("Specified field is not an instance field.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckMutable(FieldInfo field) {
            if (field.IsInitOnly) {
                throw new InvalidOperationException("Specified field is read-only.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckStatic(FieldInfo field) {
            if (!field.IsStatic) {
                throw new ArgumentException("Cannot create a reference to instance field without the instance provided.");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CheckType<T>(FieldInfo field) {
            if (field.FieldType != typeof(T)) {
                throw new ArgumentException("Field type mismatch.", nameof(field));
            }
        }
    }
}
