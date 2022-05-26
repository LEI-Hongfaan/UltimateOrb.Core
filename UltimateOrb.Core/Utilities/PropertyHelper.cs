#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UltimateOrb.Functional.CommonDelegates;

namespace UltimateOrb.Utilities {

    public static class PropertyHelper {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty>(ref object? valueRef)
            where TProperty : new() {
            return GetOrCreateIfNull<TProperty, object?>(ref valueRef);
        }
        /* // Dangerous if misused.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty>(ref object? valueRef, Func<TProperty>? selector) {
            return GetOrCreateIfNull<TProperty, object?>(ref valueRef, selector);
        }
        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty>(ref TProperty valueRef)
            where TProperty : class?, new() {
            return GetOrCreateIfNull<TProperty, TProperty>(ref valueRef);
        }
        /* // Dangerous if misused.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty>(ref TProperty valueRef, Func<TProperty>? selector)
            where TProperty : class? {
            return GetOrCreateIfNull<TProperty, TProperty>(ref valueRef, selector);
        }
        */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty, TField>(ref TField valueRef)
            where TProperty : TField, new()
            where TField : class? {
            return GetOrCreateIfNull<TProperty, TProperty, TField>(ref valueRef);
        }

        /* // Dangerous if misused.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty, TField>(ref TField valueRef, Func<TProperty>? selector)
            where TProperty : TField
            where TField : class? {
            return GetOrCreateIfNull<TProperty, TProperty, TField>(ref valueRef, selector);
        }
        */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty, TValue>(ref object? valueRef, Func<TValue>? selector)
            where TValue : TProperty {
            return GetOrCreateIfNull<TProperty, TValue, object?>(ref valueRef, selector);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty, TValue, TField>(ref TField valueRef)
            where TProperty : TField
            where TValue : TProperty, new()
            where TField : class? {
            var value = valueRef;
            if (null == value) {
                // Force type cast.
                Volatile.Write(ref value!, CreateInstance.PerType<TValue>.Value.Invoke());
                var t = Interlocked.CompareExchange(ref valueRef!, value, null!);
                if (null != t) {
                    return (TProperty)t;
                }
            }
            return (TProperty)value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TProperty GetOrCreateIfNull<TProperty, TValue, TField>(ref TField valueRef, Func<TValue>? selector)
            where TProperty : TField
            where TValue : TProperty
            where TField : class? {
            if (selector is null) {
                throw ThrowArgumentNullException_selector();
            }
            var value = valueRef;
            if (null == value) {
                // Force type cast.
                Volatile.Write(ref value!, selector.Invoke());
                var t = Interlocked.CompareExchange(ref valueRef!, value, null!);
                if (null != t) {
                    return (TProperty)t;
                }
            }
            return (TProperty)value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static ArgumentNullException ThrowArgumentNullException_selector() {
            throw new ArgumentNullException("selector");
        }

        /*
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetIfNull<TProperty, TValue, TField>(ref TField valueRef, TValue newValue)
            where TProperty : TField
            where TValue : TProperty, new()
            where TField : class? {
            var value = valueRef;
            if (null == value) {
                // Force type cast.
                Volatile.Write(ref value!, newValue);
                if (null != value) {
                    Interlocked.CompareExchange(ref valueRef!, value, null!);
                }
            }
        }
        */
    }
}