using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UltimateOrb.Runtime.Caching;

namespace UltimateOrb.Runtime.CompilerServices;

using Unsafe1 = System.Runtime.CompilerServices.Unsafe;

public static partial class StructLayoutHelpers {

    [StructLayout(LayoutKind.Sequential)]
    readonly
#if NET9_0_OR_GREATER
        ref
#endif
        struct BytePaddingWrapper<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        {
        public readonly byte Padding;
        public readonly T Value;
    }

    /// <summary>
    /// Returns the alignment (in bytes) that the runtime uses for type <typeparamref name="T"/>.
    /// This is the number of bytes that the runtime will skip over when allocating an array of <typeparamref name="T"/> or when aligning a field of type <typeparamref name="T"/> in a struct.
    /// </summary>
    /// <typeparam name="T">
    /// The type to get the alignment for.
    /// </typeparam>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int AlignOf<T>()
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        {
        return unchecked(
            Unsafe1.SizeOf<BytePaddingWrapper<BytePaddingWrapper<T>>>() -
            Unsafe1.SizeOf<BytePaddingWrapper<T>>());
    }

    static partial class Typed<T>
#if NET9_0_OR_GREATER
        where T : allows ref struct
#endif
        {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AlignOf() {
            return AlignOf<T>();
        }
    }

    static int AlignOfInternal(Type type) {
        return (int)typeof(Typed<>).MakeGenericType(type).GetMethod(nameof(AlignOf), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)!.Invoke(null, null)!;
    }

#if !COMPILER_SERVICES_USE_SHARED_CACHE
    [ThreadStatic]
#endif
    static SmallStructCache7x2<Type, int> AlignOfCache
#if COMPILER_SERVICES_USE_SHARED_CACHE
        = new()
#endif
        ; // non-readonly

#if COMPILER_SERVICES_USE_SHARED_CACHE
    static readonly ReaderWriterLockSlim AlignOfCacheLock = new(LockRecursionPolicy.NoRecursion);
#endif

    public static int AlignOf(Type type) {
#if COMPILER_SERVICES_USE_SHARED_CACHE
        AlignOfCacheLock.EnterUpgradeableReadLock();
        try {
            if (AlignOfCache.TryGetValue(type, out int result)) {
                return result;
            } else {
                result = AlignOfInternal(type);
                AlignOfCacheLock.EnterWriteLock();
                try {
                    AlignOfCache.TryAdd(type, result);
                } finally {
                    AlignOfCacheLock.ExitWriteLock();
                }
                return result;
            }
        } finally {
            AlignOfCacheLock.ExitUpgradeableReadLock();
        }
#else
        if (AlignOfCache.IsDefault) {
            AlignOfCache = new();
        }
        if (AlignOfCache.TryGetValue(type, out int result)) {
            return result;
        } else {
            result = AlignOfInternal(type);
            AlignOfCache.TryAdd(type, result);
            return result;
        }
#endif
    }
}
