using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UltimateOrb;
using UltimateOrb.Runtime.InteropServices.Marshalling;
using ExceptionArgument = UltimateOrb.Internal.ExceptionArgument;
using SR = UltimateOrb.Internal.SR;
using StructLayoutHelpers = UltimateOrb.Runtime.CompilerServices.StructLayoutHelpers;
using ThrowHelper = UltimateOrb.Internal.ThrowHelper;


namespace UltimateOrb.Internal {
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    internal static partial class Vector256Extensions {
        extension<T>(in Vector256<T> vector) {

            ref readonly Vector128<T> _lower {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.As<Vector256<T>, Vector128<T>>(ref Unsafe.AsRef(in vector));
            }

            ref readonly Vector128<T> _upper {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref Unsafe.As<Vector256<T>, Vector128<T>>(ref Unsafe.AsRef(in vector)), 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void SetLowerUnsafe(Vector128<T> value) => Unsafe.AsRef(in vector._lower) = value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void SetUpperUnsafe(Vector128<T> value) => Unsafe.AsRef(in vector._upper) = value;
        }

        extension(Vector256) {

            /// <summary>Creates a new <see cref="Vector256{T}" /> instance with all 64-bit parts initialized to a specified value.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="value">The value that the 64-bit parts will be initialized to.</param>
            /// <returns>A new <see cref="Vector128{T}" /> with the 64-bit parts initialized to <paramref name="value" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector256<T> Create<T>(Vector64<T> value) => Create(Vector128.Create(value, value));

            /// <summary>Creates a new <see cref="Vector256{T}" /> instance with the lower and upper 128-bits initialized to a specified value.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="value">The value that the lower and upper 128-bits will be initialized to.</param>
            /// <returns>A new <see cref="Vector128{T}" /> with the lower and upper 128-bits initialized to <paramref name="value" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector256<T> Create<T>(Vector128<T> value) => Create(value, value);

            /// <summary>Creates a new <see cref="Vector256{T}" /> instance from two <see cref="Vector128{T}" /> instances.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="lower">The value that the lower 128-bits will be initialized to.</param>
            /// <param name="upper">The value that the upper 128-bits will be initialized to.</param>
            /// <returns>A new <see cref="Vector256{T}" /> initialized from <paramref name="lower" /> and <paramref name="upper" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="lower" /> and <paramref name="upper" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector256<T> Create<T>(Vector128<T> lower, Vector128<T> upper) {
                if (Avx.IsSupported) {
                    Vector256<T> result = lower.ToVector256Unsafe();
                    return result.WithUpper(upper);
                } else {
                    ThrowHelper.ThrowForUnsupportedIntrinsicsVector256BaseType<T>();
                    Unsafe.SkipInit(out Vector256<T> result);

                    result.SetLowerUnsafe(lower);
                    result.SetUpperUnsafe(upper);

                    return result;
                }
            }
        }
    }

    internal static partial class Vector512Extensions {

        extension<T>(in Vector512<T> vector) {

            ref readonly Vector256<T> _lower {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.As<Vector512<T>, Vector256<T>>(ref Unsafe.AsRef(in vector));
            }

            ref readonly Vector256<T> _upper {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref Unsafe.Add(ref Unsafe.As<Vector512<T>, Vector256<T>>(ref Unsafe.AsRef(in vector)), 1);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void SetLowerUnsafe(Vector256<T> value) => Unsafe.AsRef(in vector._lower) = value;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void SetUpperUnsafe(Vector256<T> value) => Unsafe.AsRef(in vector._upper) = value;
        }

        extension(Vector512) {

            /// <summary>Creates a new <see cref="Vector512{T}" /> instance with all 64-bit parts initialized to a specified value.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="value">The value that the 64-bit parts will be initialized to.</param>
            /// <returns>A new <see cref="Vector128{T}" /> with the 64-bit parts initialized to <paramref name="value" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector512<T> Create<T>(Vector64<T> value) => Create(Vector128.Create(value, value));

            /// <summary>Creates a new <see cref="Vector512{T}" /> instance with all 128-bit parts initialized to a specified value.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="value">The value that the 128-bit parts will be initialized to.</param>
            /// <returns>A new <see cref="Vector128{T}" /> with the 128-bit parts initialized to <paramref name="value" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector512<T> Create<T>(Vector128<T> value) => Create(Vector256.Create(value, value));

            /// <summary>Creates a new <see cref="Vector512{T}" /> instance with the lower and upper 256-bits initialized to a specified value.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="value">The value that the lower and upper 256-bits will be initialized to.</param>
            /// <returns>A new <see cref="Vector128{T}" /> with the lower and upper 256-bits initialized to <paramref name="value" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="value" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector512<T> Create<T>(Vector256<T> value) => Create(value, value);

            /// <summary>Creates a new <see cref="Vector512{T}" /> instance from two <see cref="Vector256{T}" /> instances.</summary>
            /// <typeparam name="T">The type of the elements in the vector.</typeparam>
            /// <param name="lower">The value that the lower 256-bits will be initialized to.</param>
            /// <param name="upper">The value that the upper 256-bits will be initialized to.</param>
            /// <returns>A new <see cref="Vector512{T}" /> initialized from <paramref name="lower" /> and <paramref name="upper" />.</returns>
            /// <exception cref="NotSupportedException">The type of <paramref name="lower" /> and <paramref name="upper" /> (<typeparamref name="T" />) is not supported.</exception>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Vector512<T> Create<T>(Vector256<T> lower, Vector256<T> upper) {
                ThrowHelper.ThrowForUnsupportedIntrinsicsVector512BaseType<T>();
                Unsafe.SkipInit(out Vector512<T> result);

                result.SetLowerUnsafe(lower);
                result.SetUpperUnsafe(upper);

                return result;
            }
        }
    }
}

namespace UltimateOrb {
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.




    internal static partial class BigSpanHelpers {

        extension(Unsafe) {
            // Internal helper methods:

            // Determines if the address is aligned at least to `alignment` bytes.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static bool IsOpportunisticallyAligned<T>(ref readonly T address, nuint alignment) {
                // `alignment` is expected to be a power of 2 in bytes.
                // We use Unsafe.AsPointer to convert to a pointer,
                // GC will keep alignment when moving objects (up to sizeof(void*)),
                // otherwise alignment should be considered a hint if not pinned.
                Debug.Assert(nuint.IsPow2(alignment));
                unsafe {
                    return ((nuint)Unsafe.AsPointer(ref Unsafe.AsRef(in address)) & unchecked(alignment - 1)) == 0;
                }
            }

            // Determines the misalignment of the address with respect to the specified `alignment`.
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static nuint OpportunisticMisalignment<T>(ref readonly T address, nuint alignment) {
                // `alignment` is expected to be a power of 2 in bytes.
                // We use Unsafe.AsPointer to convert to a pointer,
                // GC will keep alignment when moving objects (up to sizeof(void*)),
                // otherwise alignment should be considered a hint if not pinned.
                Debug.Assert(nuint.IsPow2(alignment));
                unsafe {
                    return (nuint)Unsafe.AsPointer(ref Unsafe.AsRef(in address)) & unchecked(alignment - 1);
                }
            }
        }


        private static nuint MemmoveNativeThreshold {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                RuntimeInformation.ProcessArchitecture switch {
                    Architecture.Arm64 or Architecture.LoongArch64 => nuint.MaxValue,
                    Architecture.Arm => 512,
                    _ => 2048
                };
        }

        private const nuint ZeroMemoryNativeThreshold = 1024;


        private static bool HAS_CUSTOM_BLOCKS__defined {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get =>
                RuntimeInformation.ProcessArchitecture == Architecture.X64 ||
                RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ||
                (nint.Size == 4 && RuntimeInformation.ProcessArchitecture == Architecture.Arm) ||
                RuntimeInformation.ProcessArchitecture == Architecture.LoongArch64;
        }

        [StructLayout(LayoutKind.Sequential, Size = 16)]
        private struct Block16 { }

        [StructLayout(LayoutKind.Sequential, Size = 64)]
        private struct Block64 { }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Memmove(ref byte dest, ref byte src, nuint len) {
            // P/Invoke into the native version when the buffers are overlapping.
            if ((nuint)Unsafe.ByteOffset(ref src, ref dest) < len ||
                (nuint)Unsafe.ByteOffset(ref dest, ref src) < len) {
                goto BuffersOverlap;
            }

            ref byte srcEnd = ref Unsafe.Add(ref src, len);
            ref byte destEnd = ref Unsafe.Add(ref dest, len);

            if (len <= 16)
                goto MCPY02;
            if (len > 64)
                goto MCPY05;

        MCPY00:
            // Copy bytes which are multiples of 16 and leave the remainder for MCPY01 to handle.
            Debug.Assert(len > 16 && len <= 64);
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<Block16>(ref src));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<long>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 8), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 8)));
            } else {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<int>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 4)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 8), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 12), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 12)));
            }
            if (len <= 32)
                goto MCPY01;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 16), Unsafe.ReadUnaligned<Block16>(ref Unsafe.Add(ref src, 16)));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 16), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 24), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 24)));
            } else {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 16), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 20), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 20)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 24), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 24)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 28), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 28)));
            }
            if (len <= 48)
                goto MCPY01;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 32), Unsafe.ReadUnaligned<Block16>(ref Unsafe.Add(ref src, 32)));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 32), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 32)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 40), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 40)));
            } else {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 32), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 32)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 36), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 36)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 40), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 40)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 44), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 44)));
            }

        MCPY01:
            // Unconditionally copy the last 16 bytes using destEnd and srcEnd and return.
            Debug.Assert(len > 16 && len <= 64);
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<Block16>(ref Unsafe.Add(ref srcEnd, -16)));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref srcEnd, -16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref srcEnd, -8)));
            } else {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -12), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -12)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -4)));
            }
            return;

        MCPY02:
            // Copy the first 8 bytes and then unconditionally copy the last 8 bytes and return.
            if ((len & 24) == 0)
                goto MCPY03;
            Debug.Assert(len >= 8 && len <= 16);
            if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<long>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref srcEnd, -8)));
            } else {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<int>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 4)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -4)));
            }
            return;

        MCPY03:
            // Copy the first 4 bytes and then unconditionally copy the last 4 bytes and return.
            if ((len & 4) == 0)
                goto MCPY04;
            Debug.Assert(len >= 4 && len < 8);
            Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<int>(ref src));
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -4)));
            return;

        MCPY04:
            // Copy the first byte. For pending bytes, do an unconditionally copy of the last 2 bytes and return.
            Debug.Assert(len < 4);
            if (len == 0)
                return;
            dest = src;
            if ((len & 2) == 0)
                return;
            Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -2), Unsafe.ReadUnaligned<short>(ref Unsafe.Add(ref srcEnd, -2)));
            return;

        MCPY05:
            // PInvoke to the native version when the copy length exceeds the threshold.
            if (len > MemmoveNativeThreshold) {
                goto PInvoke;
            }

            if (HAS_CUSTOM_BLOCKS__defined) {
                if (len >= 256) {
                    // Try to opportunistically align the destination below. The input isn't pinned, so the GC
                    // is free to move the references. We're therefore assuming that reads may still be unaligned.
                    //
                    // dest is more important to align than src because an unaligned store is more expensive
                    // than an unaligned load.
                    nuint misalignedElements = 64 - Unsafe.OpportunisticMisalignment(ref dest, 64);
                    Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<Block64>(ref src));
                    src = ref Unsafe.Add(ref src, misalignedElements);
                    dest = ref Unsafe.Add(ref dest, misalignedElements);
                    len -= misalignedElements;
                }
            }

            // Copy 64-bytes at a time until the remainder is less than 64.
            // If remainder is greater than 16 bytes, then jump to MCPY00. Otherwise, unconditionally copy the last 16 bytes and return.
            Debug.Assert(len > 64 && len <= MemmoveNativeThreshold);
            nuint n = len >> 6;

        MCPY06:
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<Block64>(ref src));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<long>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 8), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 16), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 24), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 24)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 32), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 32)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 40), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 40)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 48), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 48)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 56), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref src, 56)));
            } else {
                Unsafe.WriteUnaligned(ref dest, Unsafe.ReadUnaligned<int>(ref src));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 4)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 8), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 12), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 12)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 16), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 20), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 20)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 24), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 24)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 28), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 28)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 32), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 32)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 36), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 36)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 40), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 40)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 44), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 44)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 48), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 48)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 52), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 52)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 56), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 56)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref dest, 60), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref src, 60)));
            }
            dest = ref Unsafe.Add(ref dest, 64);
            src = ref Unsafe.Add(ref src, 64);
            n--;
            if (n != 0)
                goto MCPY06;

            len %= 64;
            if (len > 16)
                goto MCPY00;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<Block16>(ref Unsafe.Add(ref srcEnd, -16)));
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref srcEnd, -16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<long>(ref Unsafe.Add(ref srcEnd, -8)));
            } else {
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -16), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -16)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -12), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -12)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -8), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -8)));
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destEnd, -4), Unsafe.ReadUnaligned<int>(ref Unsafe.Add(ref srcEnd, -4)));
            }
            return;

        BuffersOverlap:
            Debug.Assert(len > 0);
            // If the buffers overlap perfectly, there's no point to copying the data.
            if (Unsafe.AreSame(ref dest, ref src)) {
                // Both could be null with a non-zero length, perform an implicit null check.
                _ = Unsafe.ReadUnaligned<byte>(ref dest);
                return;
            }

        PInvoke:
            // Implicit nullchecks
            Debug.Assert(len > 0);
            _ = Unsafe.ReadUnaligned<byte>(ref dest);
            _ = Unsafe.ReadUnaligned<byte>(ref src);
            MemmoveNative(ref dest, ref src, len);
        }

        // Non-inlinable wrapper around the QCall that avoids polluting the fast path
        // with P/Invoke prolog/epilog.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void MemmoveNative(ref byte dest, ref byte src, nuint len) {
            fixed (byte* pDest = &dest)
            fixed (byte* pSrc = &src) {
                NativeMemory.Copy(pDest, pSrc, len);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static void ClearWithoutReferences(ref byte dest, nuint len) {
            if (len == 0)
                return;

            ref byte destEnd = ref Unsafe.Add(ref dest, len);

            if (len <= 16)
                goto MZER02;
            if (len > 64)
                goto MZER05;

        MZER00:
            // Clear bytes which are multiples of 16 and leave the remainder for MZER01 to handle.
            Debug.Assert(len > 16 && len <= 64);
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block16>(ref dest, default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref dest, 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 8), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref dest, 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 4), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 8), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 12), 0);
            }
            if (len <= 32)
                goto MZER01;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block16>(ref Unsafe.Add(ref dest, 16), default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 16), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 24), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 16), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 20), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 24), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 28), 0);
            }
            if (len <= 48)
                goto MZER01;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block16>(ref Unsafe.Add(ref dest, 32), default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 32), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 40), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 32), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 36), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 40), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 44), 0);
            }

        MZER01:
            // Unconditionally clear the last 16 bytes using destEnd and return.
            Debug.Assert(len > 16 && len <= 64);
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block16>(ref Unsafe.Add(ref destEnd, -16), default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref destEnd, -16), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref destEnd, -8), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -16), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -12), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -8), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -4), 0);
            }
            return;

        MZER02:
            // Clear the first 8 bytes and then unconditionally clear the last 8 bytes and return.
            if ((len & 24) == 0)
                goto MZER03;
            Debug.Assert(len >= 8 && len <= 16);
            if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref dest, 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref destEnd, -8), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref dest, 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 4), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -8), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -4), 0);
            }
            return;

        MZER03:
            // Clear the first 4 bytes and then unconditionally clear the last 4 bytes and return.
            if ((len & 4) == 0)
                goto MZER04;
            Debug.Assert(len >= 4 && len < 8);
            Unsafe.WriteUnaligned<int>(ref dest, 0);
            Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -4), 0);
            return;

        MZER04:
            // Clear the first byte. For pending bytes, do an unconditionally clear of the last 2 bytes and return.
            Debug.Assert(len < 4);
            if (len == 0)
                return;
            dest = 0;
            if ((len & 2) == 0)
                return;
            Unsafe.WriteUnaligned<short>(ref Unsafe.Add(ref destEnd, -2), 0);
            return;

        MZER05:
            // PInvoke to the native version when the clear length exceeds the threshold.
            if (len > ZeroMemoryNativeThreshold) {
                goto PInvoke;
            }

            if (HAS_CUSTOM_BLOCKS__defined) {
                if (len >= 256) {
                    // Try to opportunistically align the destination below. The input isn't pinned, so the GC
                    // is free to move the references. We're therefore assuming that reads may still be unaligned.
                    nuint misalignedElements = 64 - Unsafe.OpportunisticMisalignment(ref dest, 64);
                    Unsafe.WriteUnaligned<Block64>(ref dest, default);
                    dest = ref Unsafe.Add(ref dest, misalignedElements);
                    len -= misalignedElements;
                }
            }
            // Clear 64-bytes at a time until the remainder is less than 64.
            // If remainder is greater than 16 bytes, then jump to MZER00. Otherwise, unconditionally clear the last 16 bytes and return.
            Debug.Assert(len > 64 && len <= ZeroMemoryNativeThreshold);
            nuint n = len >> 6;

        MZER06:
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block64>(ref dest, default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref dest, 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 8), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 16), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 24), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 32), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 40), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 48), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref dest, 56), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref dest, 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 4), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 8), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 12), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 16), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 20), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 24), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 28), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 32), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 36), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 40), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 44), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 48), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 52), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 56), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref dest, 60), 0);
            }
            dest = ref Unsafe.Add(ref dest, 64);
            n--;
            if (n != 0)
                goto MZER06;

            len %= 64;
            if (len > 16)
                goto MZER00;
            if (HAS_CUSTOM_BLOCKS__defined) {
                Unsafe.WriteUnaligned<Block16>(ref Unsafe.Add(ref destEnd, -16), default);
            } else if (nint.Size == 8) {
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref destEnd, -16), 0);
                Unsafe.WriteUnaligned<long>(ref Unsafe.Add(ref destEnd, -8), 0);
            } else {
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -16), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -12), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -8), 0);
                Unsafe.WriteUnaligned<int>(ref Unsafe.Add(ref destEnd, -4), 0);
            }
            return;

        PInvoke:
            // Implicit nullchecks
            _ = Unsafe.ReadUnaligned<byte>(ref dest);
            ZeroMemoryNative(ref dest, len);
        }

        // Non-inlinable wrapper around the QCall that avoids polluting the fast path
        // with P/Invoke prolog/epilog.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void ZeroMemoryNative(ref byte b, nuint byteLength) {
            unchecked {
                fixed (byte* ptr = &b) {
                    byte* adjustedPtr = ptr;
                    if (RuntimeInformation.ProcessArchitecture is Architecture.X86 or Architecture.X64) {
                        if (byteLength > 0x100) {
                            // memset ends up calling rep stosb if the hardware claims to support it efficiently. rep stosb is up to 2x slower
                            // on misaligned blocks. Workaround this issue by aligning the blocks passed to memset upfront.
                            Unsafe.WriteUnaligned<Block16>(ptr, default);
                            Unsafe.WriteUnaligned<Block16>(ptr + byteLength - 16, default);

                            byte* alignedEnd = (byte*)((nuint)(ptr + byteLength - 1) & ~(nuint)(16 - 1));

                            adjustedPtr = (byte*)(((nuint)ptr + 16) & ~(nuint)(16 - 1));
                            byteLength = (nuint)(alignedEnd - adjustedPtr);
                        }
                    }
                    memset(adjustedPtr, 0, byteLength);

                }
            }
        }

        private static unsafe void* memset(void* dest, int value, nuint len) {
            var p = unchecked((byte)value);
            Utilities.ThrowHelper.ThrowOnNotEqual(unchecked((int)((uint)p * 0x01010101u)), value);
            NativeMemory.Fill(dest, len, p);
            return dest;
        }
    }
}

namespace UltimateOrb {

    internal static partial class BigSpanHelpers {


#if NET10_0_OR_GREATER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ClearWithReferences")]
        static extern void ClearWithReferences_([UnsafeAccessorType("PrivateLib.Class1")] object? _, ref IntPtr ip, nuint pointerSizeLength);
#endif

        public static void ClearWithReferences(ref IntPtr ip, nuint pointerSizeLength) {
#if false && NET10_0_OR_GREATER
            ClearWithReferences_(null, ref ip, pointerSizeLength);
#else
            unchecked {
                Debug.Assert(pointerSizeLength < (nuint)nint.MaxValue);
                // Debug.Assert(Unsafe.IsOpportunisticallyAligned(ref ip, (uint)sizeof(IntPtr)), "Should've been aligned on natural word boundary.");

                // First write backward 8 natural words at a time.
                // Writing backward allows us to get away with only simple modifications to the
                // mov instruction's base and index registers between loop iterations.

                for (; pointerSizeLength >= 8; pointerSizeLength -= 8) {
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -1) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -2) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -3) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -4) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -5) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -6) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -7) = default;
                    Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -8) = default;
                }

                Debug.Assert(pointerSizeLength <= 7);

                // The logic below works by trying to minimize the number of branches taken for any
                // given range of lengths. For example, the lengths [ 4 .. 7 ] are handled by a single
                // branch, [ 2 .. 3 ] are handled by a single branch, and [ 1 ] is handled by a single
                // branch.
                //
                // We can write both forward and backward as a perf improvement. For example,
                // the lengths [ 4 .. 7 ] can be handled by zeroing out the first four natural
                // words and the last 3 natural words. In the best case (length = 7), there are
                // no overlapping writes. In the worst case (length = 4), there are three
                // overlapping writes near the middle of the buffer. In perf testing, the
                // penalty for performing duplicate writes is less expensive than the penalty
                // for complex branching.

                if (pointerSizeLength >= 4) {
                    goto Write4To7;
                } else if (pointerSizeLength >= 2) {
                    goto Write2To3;
                } else if (pointerSizeLength > 0) {
                    goto Write1;
                } else {
                    return; // nothing to write
                }

            Write4To7:
                Debug.Assert(pointerSizeLength >= 4);

                // Write first four and last three.
                Unsafe.Add(ref ip, 2) = default;
                Unsafe.Add(ref ip, 3) = default;
                Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -3) = default;
                Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -2) = default;

            Write2To3:
                Debug.Assert(pointerSizeLength >= 2);

                // Write first two and last one.
                Unsafe.Add(ref ip, 1) = default;
                Unsafe.Add(ref Unsafe.Add(ref ip, (nint)pointerSizeLength), -1) = default;

            Write1:
                Debug.Assert(pointerSizeLength >= 1);

                // Write only element.
                ip = default;
            }
#endif
        }
    }
}

namespace UltimateOrb {
    using UltimateOrb.Internal;
    using static UltimateOrb.Internal.Vector256Extensions;
    using static UltimateOrb.Internal.Vector512Extensions;

    using Unsafe = UltimateOrb.Runtime.CompilerServices.Unsafe;
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    internal static partial class BigSpanHelpers {

        // [Intrinsic] // Unrolled for small sizes
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static unsafe void Fill<T>(ref T refData, nuint numElements, T value) {
            // Early checks to see if it's even possible to vectorize - JIT will turn these checks into consts.
            // - T cannot contain references (GC can't track references in vectors)
            // - Vectorization must be hardware-accelerated
            // - T's size must not exceed the vector's size
            // - T's size must be a whole power of 2
            unchecked {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    goto CannotVectorize;
                }

                if (!Vector.IsHardwareAccelerated) {
                    goto CannotVectorize;
                }

                if (sizeof(T) > Vector<byte>.Count) {
                    goto CannotVectorize;
                }

                if (!BitOperations.IsPow2(sizeof(T))) {
                    goto CannotVectorize;
                }
                if (numElements >= (uint)(Vector<byte>.Count / sizeof(T))) {
                    // We have enough data for at least one vectorized write.
                    Vector<byte> vector;

                    if (sizeof(T) == 1) {
                        vector = new Vector<byte>(Unsafe.BitCast<T, byte>(value));
                    } else if (sizeof(T) == 2) {
                        vector = (Vector<byte>)new Vector<ushort>(Unsafe.BitCast<T, ushort>(value));
                    } else if (sizeof(T) == 4) {
                        // special-case float since it's already passed in a SIMD reg
                        vector = (typeof(T) == typeof(float))
                            ? (Vector<byte>)new Vector<float>(Unsafe.BitCast<T, float>(value))
                            : (Vector<byte>)new Vector<uint>(Unsafe.BitCast<T, uint>(value));
                    } else if (sizeof(T) == 8) {
                        // special-case double since it's already passed in a SIMD reg
                        vector = (typeof(T) == typeof(double))
                            ? (Vector<byte>)new Vector<double>(Unsafe.BitCast<T, double>(value))
                            : (Vector<byte>)new Vector<ulong>(Unsafe.BitCast<T, ulong>(value));
                    } else if (sizeof(T) == Vector<byte>.Count) {
                        vector = Unsafe.BitCast<T, Vector<byte>>(value);
                    } else if (sizeof(T) == 16) {
                        if (Vector<byte>.Count == 32) {
                            vector = Vector256.Create<byte>(Unsafe.BitCast<T, Vector128<byte>>(value)).AsVector();
                        } else if (Vector<byte>.Count == 64) {
                            vector = Vector512.Create<byte>(Unsafe.BitCast<T, Vector128<byte>>(value)).AsVector();
                        } else {
                            Debug.Fail("Vector<T> is unexpected size.");
                            goto CannotVectorize;
                        }
                    } else if (sizeof(T) == 32) {
                        if (Vector<byte>.Count == 64) {
                            vector = Vector512.Create<byte>(Unsafe.BitCast<T, Vector256<byte>>(value)).AsVector();
                        } else {
                            Debug.Fail("Vector<T> is unexpected size.");
                            goto CannotVectorize;
                        }
                    } else {
                        Debug.Fail("Vector<T> is greater than 512 bits in size?");
                        goto CannotVectorize;
                    }

                    ref byte refDataAsBytes = ref Unsafe.As<T, byte>(ref refData);
                    nuint totalByteLength = numElements * (nuint)sizeof(T); // get this calculation ready ahead of time
                    nuint stopLoopAtOffset = totalByteLength & (nuint)(nint)(2 * (int)-Vector<byte>.Count); // intentional sign extension carries the negative bit
                    nuint offset = 0;

                    // Loop, writing 2 vectors at a time.
                    // Compare 'numElements' rather than 'stopLoopAtOffset' because we don't want a dependency
                    // on the very recently calculated 'stopLoopAtOffset' value.

                    if (numElements >= (uint)(2 * Vector<byte>.Count / sizeof(T))) {
                        do {
                            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref refDataAsBytes, offset), vector);
                            Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref refDataAsBytes, offset + (nuint)Vector<byte>.Count), vector);
                            offset += (uint)(2 * Vector<byte>.Count);
                        } while (offset < stopLoopAtOffset);
                    }

                    // At this point, if any data remains to be written, it's strictly less than
                    // 2 * sizeof(Vector) bytes. The loop above had us write an even number of vectors.
                    // If the total byte length instead involves us writing an odd number of vectors, write
                    // one additional vector now. The bit check below tells us if we're in an "odd vector
                    // count" situation.

                    if ((totalByteLength & (nuint)Vector<byte>.Count) != 0) {
                        Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref refDataAsBytes, offset), vector);
                    }

                    // It's possible that some small buffer remains to be populated - something that won't
                    // fit an entire vector's worth of data. Instead of falling back to a loop, we'll write
                    // a vector at the very end of the buffer. This may involve overwriting previously
                    // populated data, which is fine since we're splatting the same value for all entries.
                    // There's no need to perform a length check here because we already performed this
                    // check before entering the vectorized code path.

                    Unsafe.WriteUnaligned(ref Unsafe.AddByteOffset(ref refDataAsBytes, totalByteLength - (nuint)Vector<byte>.Count), vector);

                    // And we're done!

                    return;
                }

            CannotVectorize:

                // If we reached this point, we cannot vectorize this T, or there are too few
                // elements for us to vectorize. Fall back to an unrolled loop.

                nuint i = 0;

                // Write 8 elements at a time

                if (numElements >= 8) {
                    nuint stopLoopAtOffset = numElements & ~(nuint)7;
                    do {
                        Unsafe.Add(ref refData, (nint)i + 0) = value;
                        Unsafe.Add(ref refData, (nint)i + 1) = value;
                        Unsafe.Add(ref refData, (nint)i + 2) = value;
                        Unsafe.Add(ref refData, (nint)i + 3) = value;
                        Unsafe.Add(ref refData, (nint)i + 4) = value;
                        Unsafe.Add(ref refData, (nint)i + 5) = value;
                        Unsafe.Add(ref refData, (nint)i + 6) = value;
                        Unsafe.Add(ref refData, (nint)i + 7) = value;
                    } while ((i += 8) < stopLoopAtOffset);
                }

                // Write next 4 elements if needed

                if ((numElements & 4) != 0) {
                    Unsafe.Add(ref refData, (nint)i + 0) = value;
                    Unsafe.Add(ref refData, (nint)i + 1) = value;
                    Unsafe.Add(ref refData, (nint)i + 2) = value;
                    Unsafe.Add(ref refData, (nint)i + 3) = value;
                    i += 4;
                }

                // Write next 2 elements if needed

                if ((numElements & 2) != 0) {
                    Unsafe.Add(ref refData, (nint)i + 0) = value;
                    Unsafe.Add(ref refData, (nint)i + 1) = value;
                    i += 2;
                }

                // Write final element if needed

                if ((numElements & 1) != 0) {
                    Unsafe.Add(ref refData, (nint)i) = value;
                }
            }
        }
    }
}
namespace UltimateOrb {

    public static partial class BigSpanExtensions {

        extension<T>(T[] @this) {

            public BigSpan<T> AsBigSpan() {
                return @this.AsSpan();
            }
        }

        extension(MemoryMarshal) {

            /// <inheritdoc cref="MemoryMarshal.AsBytes{T}(Span{T})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [OverloadResolutionPriority(1)] // Prioritize this overload over the ReadOnlySpan overload so types convertible to both resolve to this mutable version.
            public static unsafe BigSpan<byte> AsBytes<T>(BigSpan<T> span)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));

                return new BigSpan<byte>(
                    ref Unsafe.As<T, byte>(ref GetReference(span)),
                    checked(span.Length * sizeof(T)));
            }

            /// <inheritdoc cref="MemoryMarshal.AsBytes{T}(ReadOnlySpan{T})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe ReadOnlyBigSpan<byte> AsBytes<T>(ReadOnlyBigSpan<T> span)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));

                return new ReadOnlyBigSpan<byte>(
                    ref Unsafe.As<T, byte>(ref GetReference(span)),
                    checked(span.Length * sizeof(T)));
            }

            /// <inheritdoc cref="MemoryMarshal.GetReference{T}(Span{T})"/>>
            public static ref T GetReference<T>(BigSpan<T> span) => ref span._reference;

            /// <inheritdoc cref="MemoryMarshal.GetReference{T}(ReadOnlySpan{T})"/>>
            public static ref T GetReference<T>(ReadOnlyBigSpan<T> span) => ref span._reference;

            /// <inheritdoc cref="MemoryMarshal.Cast{TFrom, TTo}(Span{TFrom})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [OverloadResolutionPriority(1)] // Prioritize this overload over the ReadOnlySpan overload so types convertible to both resolve to this mutable version.
            public static unsafe BigSpan<TTo> Cast<TFrom, TTo>(BigSpan<TFrom> span)
                where TFrom : struct
                where TTo : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(TFrom));
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(TTo));
                unchecked {
                    // Use unsigned integers - unsigned division by constant (especially by power of 2)
                    // and checked casts are faster and smaller.
                    uint fromSize = (uint)sizeof(TFrom);
                    uint toSize = (uint)sizeof(TTo);
                    nuint fromLength = (nuint)span.Length;
                    nint toLength;
                    if (fromSize == toSize) {
                        // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                        // should be optimized to just `length` but the JIT doesn't do that today.
                        toLength = (nint)fromLength;
                    } else if (fromSize == 1) {
                        // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                        // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
                        // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
                        // the JIT can't eliminate long multiply by 1.
                        toLength = (nint)(fromLength / toSize);
                    } else {
                        if (nint.Size == 4) {
                            // Ensure that casts are done in such a way that the JIT is able to "see"
                            // the uint->ulong casts and the multiply together so that on 32 bit targets
                            // 32x32to64 multiplication is used.
                            ulong toLengthUInt64 = (ulong)(uint)fromLength * (ulong)(uint)fromSize / (ulong)toSize;
                            toLength = checked((int)toLengthUInt64);
                        } else {
                            /*
                            ulong high = Math.BigMulUnsigned((ulong)fromLength, (ulong)fromSize, out ulong low);
                            ulong toLengthUInt64;
                            Utilities.ThrowHelper.ThrowOnLessThanOrEqual((ulong)toSize, high);
                            toLengthUInt64 = DoubleArithmetic.BigDivInternal(low, high, (ulong)toSize);
                            */
                            nuint toLengthUIntPtr = fromLength * (nuint)fromSize / (nuint)toSize;
                            toLength = checked((nint)toLengthUIntPtr);
                        }
                    }

                    return new BigSpan<TTo>(
                        ref Unsafe.As<TFrom, TTo>(ref span._reference),
                        toLength);
                }
            }

            /// <inheritdoc cref="MemoryMarshal.Cast{TFrom, TTo}(ReadOnlySpan{TFrom})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe ReadOnlyBigSpan<TTo> Cast<TFrom, TTo>(ReadOnlyBigSpan<TFrom> span)
                where TFrom : struct
                where TTo : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TFrom>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(TFrom));
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TTo>())
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(TTo));
                unchecked {
                    // Use unsigned integers - unsigned division by constant (especially by power of 2)
                    // and checked casts are faster and smaller.
                    uint fromSize = (uint)sizeof(TFrom);
                    uint toSize = (uint)sizeof(TTo);
                    nuint fromLength = (nuint)span.Length;
                    nint toLength;
                    if (fromSize == toSize) {
                        // Special case for same size types - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                        // should be optimized to just `length` but the JIT doesn't do that today.
                        toLength = (nint)fromLength;
                    } else if (fromSize == 1) {
                        // Special case for byte sized TFrom - `(ulong)fromLength * (ulong)fromSize / (ulong)toSize`
                        // becomes `(ulong)fromLength / (ulong)toSize` but the JIT can't narrow it down to `int`
                        // and can't eliminate the checked cast. This also avoids a 32 bit specific issue,
                        // the JIT can't eliminate long multiply by 1.
                        toLength = (nint)(fromLength / toSize);
                    } else {
                        if (nint.Size == 4) {
                            // Ensure that casts are done in such a way that the JIT is able to "see"
                            // the uint->ulong casts and the multiply together so that on 32 bit targets
                            // 32x32to64 multiplication is used.
                            ulong toLengthUInt64 = (ulong)(uint)fromLength * (ulong)(uint)fromSize / (ulong)toSize;
                            toLength = checked((int)toLengthUInt64);
                        } else {
                            /*
                            ulong high = Math.BigMulUnsigned((ulong)fromLength, (ulong)fromSize, out ulong low);
                            ulong toLengthUInt64;
                            Utilities.ThrowHelper.ThrowOnLessThanOrEqual((ulong)toSize, high);
                            toLengthUInt64 = DoubleArithmetic.BigDivInternal(low, high, (ulong)toSize);
                            */
                            nuint toLengthUIntPtr = fromLength * (nuint)fromSize / (nuint)toSize;
                            toLength = checked((nint)toLengthUIntPtr);
                        }
                    }

                    return new ReadOnlyBigSpan<TTo>(
                        ref Unsafe.As<TFrom, TTo>(ref span._reference),
                        toLength);
                }
            }

            /// <inheritdoc cref="MemoryMarshal.CreateSpan{T}(ref T, int)"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static BigSpan<T> CreateBigSpan<T>(scoped ref T reference, nint length) =>
                new BigSpan<T>(ref Unsafe.AsRef(in reference), length);

            /// <inheritdoc cref="MemoryMarshal.CreateReadOnlySpan{T}(ref readonly T, int)"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static ReadOnlyBigSpan<T> CreateReadOnlySpan<T>(scoped ref readonly T reference, nint length) =>
                new ReadOnlyBigSpan<T>(ref Unsafe.AsRef(in reference), length);

            /* // We are not providing these methods for now since it is unlikely to read such long string.
            /// <inheritdoc cref="MemoryMarshal.CreateReadOnlySpanFromNullTerminated(char*)"/>>
            [CLSCompliant(false)]
            public static unsafe ReadOnlyBigSpan<char> CreateReadOnlyBigSpanFromNullTerminated(char* value) =>
                value != null ? new ReadOnlyBigSpan<char>(value, string.wcslen(value)) :
                default;

            /// <inheritdoc cref="MemoryMarshal.CreateReadOnlySpanFromNullTerminated(byte*)"/>>
            [CLSCompliant(false)]
            public static unsafe ReadOnlyBigSpan<byte> CreateReadOnlyBigSpanFromNullTerminated(byte* value) =>
                value != null ? new ReadOnlyBigSpan<byte>(value, string.strlen(value)) :
                default;
            */

            /// <inheritdoc cref="MemoryMarshal.Read{T}(ReadOnlySpan{byte})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe T Read<T>(ReadOnlyBigSpan<byte> source)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (source.Length < sizeof(T)) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
                }
                return Unsafe.ReadUnaligned<T>(ref GetReference(source));
            }

            /// <inheritdoc cref="MemoryMarshal.TryRead{T}(ReadOnlySpan{byte}, out T)"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe bool TryRead<T>(ReadOnlyBigSpan<byte> source, out T value)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (source.Length < sizeof(T)) {
                    value = default;
                    return false;
                }
                value = Unsafe.ReadUnaligned<T>(ref GetReference(source));
                return true;
            }

            /// <inheritdoc cref="MemoryMarshal.Write{T}(Span{byte}, in T)"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe void Write<T>(BigSpan<byte> destination, in T value)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (destination.Length < sizeof(T)) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
                }
                Unsafe.WriteUnaligned(ref GetReference(destination), value);
            }

            /// <inheritdoc cref="MemoryMarshal.TryWrite{T}(Span{byte}, in T)"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe bool TryWrite<T>(BigSpan<byte> destination, in T value)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (destination.Length < sizeof(T)) {
                    return false;
                }
                Unsafe.WriteUnaligned(ref GetReference(destination), value);
                return true;
            }

            /// <inheritdoc cref="MemoryMarshal.AsRef{T}(Span{byte})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            [OverloadResolutionPriority(1)] // Prioritize this overload over the ReadOnlySpan overload so types convertible to both resolve to this mutable version.
            public static unsafe ref T AsRef<T>(BigSpan<byte> span)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (span.Length < sizeof(T)) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
                }
                return ref Unsafe.As<byte, T>(ref GetReference(span));
            }

            /// <inheritdoc cref="MemoryMarshal.AsRef{T}(ReadOnlySpan{byte})"/>>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static unsafe ref readonly T AsRef<T>(ReadOnlyBigSpan<byte> span)
                where T : struct {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
                }
                if (span.Length < sizeof(T)) {
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.length);
                }
                return ref Unsafe.As<byte, T>(ref GetReference(span));
            }
        }

        private const uint BulkMoveWithWriteBarrierChunk = 0x4000;

        extension(Buffer) {
            // Licensed to the .NET Foundation under one or more agreements.
            // The .NET Foundation licenses this file to you under the MIT license.

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal static unsafe void Memmove<T>(ref T destination, ref T source, nuint elementCount) {
                if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    // Blittable memmove
                    BigSpanHelpers.Memmove(
                        ref Unsafe.As<T, byte>(ref destination),
                        ref Unsafe.As<T, byte>(ref source),
                        elementCount * (nuint)sizeof(T));
                } else {
                    // Non-blittable memmove
                    BulkMoveWithWriteBarrier<T>(
                        ref Unsafe.As<T, byte>(ref destination),
                        ref Unsafe.As<T, byte>(ref source),
                        elementCount * (nuint)sizeof(T)); // TODO:
                }
            }

            internal static void BulkMoveWithWriteBarrier<T>(ref byte destination, ref byte source, nuint byteCount) {
                if (byteCount <= BulkMoveWithWriteBarrierChunk) {
                    BulkMoveWithWriteBarrierInternal<T>(ref destination, ref source, byteCount);
                    //Thread.FastPollGC();
                } else {
                    BulkMoveWithWriteBarrierBatch<T>(ref destination, ref source, byteCount);
                }
            }

            // [MethodImpl(MethodImplOptions.NoInlining)]
            private static void BulkMoveWithWriteBarrierBatch<T>(ref byte destination, ref byte source, nuint byteCount) {
                Debug.Assert(byteCount > BulkMoveWithWriteBarrierChunk);

                if (Unsafe.AreSame(ref source, ref destination))
                    return;

                // This is equivalent to: (destination - source) >= byteCount || (destination - source) < 0
                if ((nuint)(nint)Unsafe.ByteOffset(ref source, ref destination) >= byteCount) {
                    // Copy forwards
                    do {
                        byteCount -= BulkMoveWithWriteBarrierChunk;
                        BulkMoveWithWriteBarrierInternal<T>(ref destination, ref source, BulkMoveWithWriteBarrierChunk);
                        //Thread.FastPollGC();
                        destination = ref Unsafe.AddByteOffset(ref destination, BulkMoveWithWriteBarrierChunk);
                        source = ref Unsafe.AddByteOffset(ref source, BulkMoveWithWriteBarrierChunk);
                    }
                    while (byteCount > BulkMoveWithWriteBarrierChunk);
                } else {
                    // Copy backwards
                    do {
                        byteCount -= BulkMoveWithWriteBarrierChunk;
                        BulkMoveWithWriteBarrierInternal<T>(ref Unsafe.AddByteOffset(ref destination, byteCount), ref Unsafe.AddByteOffset(ref source, byteCount), BulkMoveWithWriteBarrierChunk);
                        //Thread.FastPollGC();
                    }
                    while (byteCount > BulkMoveWithWriteBarrierChunk);
                }
                BulkMoveWithWriteBarrierInternal<T>(ref destination, ref source, byteCount);
                //Thread.FastPollGC();
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void BulkMoveWithWriteBarrierInternal<T>(ref byte destination, ref byte source, nuint byteCount) {
                Debug.Assert(byteCount <= BulkMoveWithWriteBarrierChunk);
                var elementCount = unchecked((int)((uint)byteCount / (uint)sizeof(T)));
                MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<byte, T>(ref source), elementCount).CopyTo(
                    MemoryMarshal.CreateSpan(ref Unsafe.As<byte, T>(ref destination), elementCount));
            }
        }
    }

    /// <inheritdoc cref="Span{T}"/>
    [DebuggerTypeProxy(typeof(BigSpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    [NativeMarshalling(typeof(BigSpanMarshaller<,>))]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref partial struct BigSpan<T> {

        internal readonly ByReference<T> _reference1;

        internal ref T _reference {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _reference1.Value;
        }

        internal readonly nint _length;

        /// <inheritdoc cref="Span{T}.Span(T[])"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(BigArray<T>? array) {
            if (array == null) {
                this = default;
                return; // returns default
            }

            _reference1 = new(ref MemoryMarshal.GetArrayDataReference(array));
            _length = array.Length;
        }

        /// <inheritdoc cref="Span{T}.Span(T[])"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(T[]? array) {
            if (array == null) {
                this = default;
                return; // returns default
            }

            _reference1 = new(ref MemoryMarshal.GetArrayDataReference(array));
            _length = array.Length;
        }

        /// <inheritdoc cref="Span{T}.Span(T[], int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(BigArray<T>? array, nint start, nint length) {
            if (array == null) {
                if (start != 0 || length != 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException();
                this = default;
                return; // returns default
            }
            unchecked {
                if ((nuint)start > (nuint)array.Length || (nuint)length > (nuint)(array.Length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                _reference1 = new(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), start));
                _length = length;
            }
        }

        /// <inheritdoc cref="Span{T}.Span(T[], int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(T[]? array, int start, int length) {
            if (array == null) {
                if (start != 0 || length != 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException();
                this = default;
                return; // returns default
            }
            unchecked {
                if (nint.Size == 8) {
                    // See comment in Span<T>.Slice for how this works.
                    if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)array.Length)
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                } else {
                    if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                }

                _reference1 = new(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), (nint)(uint)start /* force zero-extension */));
                _length = length;
            }
        }

        /// <inheritdoc cref="Span{T}.Span(void*, int)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe BigSpan(void* pointer, nint length) {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException();
            unsafe {
                _reference1 = new(ref *(T*)pointer);
            }
            _length = length;
        }

        /// <inheritdoc cref="Span{T}.Span(void*, int)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe BigSpan(void* pointer, int length) {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException();
            unsafe {
                _reference1 = new(ref *(T*)pointer);
            }
            _length = length;
        }

        /// <inheritdoc cref="Span{T}.Span(ref T)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan(ref T reference) {
            _reference1 = new(ref reference);
            _length = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpan(ref T reference, nint length) {
            Debug.Assert(length >= 0);

            _reference1 = new(ref reference);
            _length = length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal BigSpan(ref T reference, int length) {
            Debug.Assert(length >= 0);

            _reference1 = new(ref reference);
            _length = length;
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[int index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((uint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)(uint)index /* force zero-extension */);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((uint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)(uint)index /* force zero-extension */);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((nuint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, index);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((nuint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, index);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((ulong)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)index);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.this[int]"/>
        public ref T this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((ulong)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nuint)index);
                }
            }
        }

        /// <inheritdoc cref="Span{T}.Length"/>
        public nint Length {

            get => _length;
        }

        /// <inheritdoc cref="Span{T}.IsEmpty"/>
        public bool IsEmpty {

            get => _length == 0;
        }

        /// <inheritdoc cref="Span{T}.operator!=(Span{T},Span{T})"/>
        public static bool operator !=(BigSpan<T> left, BigSpan<T> right) => !(left == right);

        /// <inheritdoc cref="Span{T}.Equals(object)"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) =>
            throw new NotSupportedException(SR.NotSupported_CannotCallEqualsOnSpan);

        /// <inheritdoc cref="Span{T}.GetHashCode()"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
           throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnSpan);

        /// <inheritdoc cref="Span{T}.op_Implicit(T[])"/>
        public static implicit operator BigSpan<T>(BigArray<T>? array) => new BigSpan<T>(array);

        /// <inheritdoc cref="Span{T}.op_Implicit(T[])"/>
        public static implicit operator BigSpan<T>(T[]? array) => new BigSpan<T>(array);

        /// <inheritdoc cref="Span{T}.op_Implicit(ArraySegment{T})"/>
        public static implicit operator BigSpan<T>(ArraySegment<T> segment) =>
            new BigSpan<T>(segment.Array, segment.Offset, segment.Count);

        /// <inheritdoc cref="BigSpan{T}.Empty"/>
        public static BigSpan<T> Empty => default;

        /// <inheritdoc cref="Span{T}.GetEnumerator()"/>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <inheritdoc cref="Span{T}.Enumerator"/>
        public ref struct Enumerator : IEnumerator<T> {

            private readonly BigSpan<T> _span;

            private nint _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(BigSpan<T> span) {
                _span = span;
                _index = -1;
            }

            /// <inheritdoc cref="Span{T}.Enumerator.MoveNext()"/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                nint index = unchecked(_index + 1);
                if (index < _span.Length) {
                    _index = index;
                    return true;
                }

                return false;
            }

            /// <inheritdoc cref="Span{T}.Enumerator.Current"/>
            public ref T Current {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _span[_index];
            }

            T IEnumerator<T>.Current => Current;

            object System.Collections.IEnumerator.Current => Current!;

            void System.Collections.IEnumerator.Reset() => _index = -1;

            void IDisposable.Dispose() { }
        }

        /// <inheritdoc cref="Span{T}.GetPinnableReference()"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref T GetPinnableReference() {
            ref T ret = ref Unsafe.NullRef<T>();
            if (_length != 0) {
                ret = ref _reference;
            }
            return ref ret;
        }

        /// <inheritdoc cref="Span{T}.Clear()"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Clear() {
            unchecked {
                if (RuntimeHelpers.IsReferenceOrContainsReferences<T>()) {
                    BigSpanHelpers.ClearWithReferences(ref Unsafe.As<T, IntPtr>(ref _reference), (nuint)_length * (nuint)(Unsafe.SizeOf<T>() / nuint.Size));
                } else {
                    BigSpanHelpers.ClearWithoutReferences(ref Unsafe.As<T, byte>(ref _reference), (nuint)_length * (nuint)Unsafe.SizeOf<T>());
                }
            }
        }

        /// <inheritdoc cref="Span{T}.Fill(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fill(T value) {
            BigSpanHelpers.Fill(ref _reference, unchecked((nuint)_length), value);
        }

        /// <inheritdoc cref="Span{T}.CopyTo(Span{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(BigSpan<T> destination) {
            // Using "if (!TryCopyTo(...))" results in two branches: one for the length
            // check, and one for the result of TryCopyTo. Since these checks are equivalent,
            // we can optimize by performing the check once ourselves then calling Memmove directly.
            unchecked {
                if ((nuint)_length <= (nuint)destination.Length) {
                    Buffer.Memmove(ref destination._reference, ref _reference, (nuint)_length);
                } else {
                    ThrowHelper.ThrowArgumentException_DestinationTooShort();
                }
            }
        }

        /// <inheritdoc cref="Span{T}.TryCopyTo(Span{T})"/>
        public bool TryCopyTo(BigSpan<T> destination) {
            unchecked {
                bool retVal = false;
                if ((nuint)_length <= (nuint)destination.Length) {
                    Buffer.Memmove(ref destination._reference, ref _reference, (nuint)_length);
                    retVal = true;
                }
                return retVal;
            }
        }

        /// <inheritdoc cref="Span{T}.operator==(Span{T},Span{T})"/>
        public static bool operator ==(BigSpan<T> left, BigSpan<T> right) =>
            left._length == right._length &&
            Unsafe.AreSame(ref left._reference, ref right._reference);

        /// <inheritdoc cref="Span{T}.op_Implicit(Span{T})"/>
        public static implicit operator ReadOnlyBigSpan<T>(BigSpan<T> span) =>
            new ReadOnlyBigSpan<T>(ref span._reference, span._length);

        /// <inheritdoc cref="Span{T}.ToString()"/>
        public override string ToString() {
            if (typeof(T) == typeof(char)) {
                return new string(MemoryMarshal.CreateReadOnlySpan<char>(ref Unsafe.As<T, char>(ref _reference), unchecked((int)Math.Min(_length, int.MaxValue))));
            }
            return $"System.Span<{typeof(T).Name}>[{_length}]";
        }

        /// <inheritdoc cref="Span{T}.Slice(int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T> Slice(int start) {
            unchecked {
                if ((uint)start > (nuint)_length)
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new BigSpan<T>(ref Unsafe.Add(ref _reference, (nint)(uint)start /* force zero-extension */), _length - start);
            }
        }

        /// <inheritdoc cref="Span{T}.Slice(int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T> Slice(nint start) {
            unchecked {
                if ((nuint)start > (nuint)_length)
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new BigSpan<T>(ref Unsafe.Add(ref _reference, (nuint)start), _length - start);
            }
        }

        /// <inheritdoc cref="Span{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length) {
            unchecked {
                if (nint.Size == 8) {
                    if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)_length)
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                } else {
                    if ((uint)start > (nuint)_length || (uint)length > (nuint)(_length - start))
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                }

                return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref _reference, (nint)(uint)start /* force zero-extension */), length);
            }
        }

        /// <inheritdoc cref="Span{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(nint start, int length) {
            unchecked {
                if ((nuint)start > (nuint)_length || (uint)length > (nuint)(_length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return MemoryMarshal.CreateSpan(ref Unsafe.Add(ref _reference, start), length);
            }
        }

        /// <inheritdoc cref="Span{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BigSpan<T> Slice(nint start, nint length) {
            unchecked {
                if ((nuint)start > (nuint)_length || (nuint)length > (nuint)(_length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new BigSpan<T>(ref Unsafe.Add(ref _reference, start), length);
            }
        }

        /// <inheritdoc cref="Span{T}.ToArray()"/>
        public T[] ToArray() {
            if (IsEmpty) {
                return [];
            }

            var destination = new T[Length]; // may throw
            CopyTo(destination);
            return destination;
        }

        /// <inheritdoc cref="Span{T}.ToArray()"/>
        public BigArray<T> ToBigArray() {
            if (IsEmpty) {
                return BigArray<T>.Empty;
            }

            var destination = GC.AllocateUninitializedBigArray<T>(Length);
            CopyTo(destination);
            return destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator BigSpan<T>(Span<T> value) {
            return new(ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Span<T>(BigSpan<T> value) {
            return MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(value), checked((int)value.Length));
        }
    }

    /// <inheritdoc cref="ReadOnlySpan{T}"/>
    [DebuggerTypeProxy(typeof(BigSpanDebugView<>))]
    [DebuggerDisplay("{ToString(),raw}")]
    [NativeMarshalling(typeof(ReadOnlyBigSpanMarshaller<,>))]
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref partial struct ReadOnlyBigSpan<T> {

        internal readonly ByReference<T> _reference1;

        internal ref T _reference {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref _reference1.Value;
        }

        internal readonly nint _length;

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(T[])"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan(BigArray<T>? array) {
            if (array == null) {
                this = default;
                return; // returns default
            }

            _reference1 = new(ref MemoryMarshal.GetArrayDataReference(array));
            _length = array.Length;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(T[])"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan(T[]? array) {
            if (array == null) {
                this = default;
                return; // returns default
            }

            _reference1 = new(ref MemoryMarshal.GetArrayDataReference(array));
            _length = array.Length;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(T[], int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan(BigArray<T>? array, nint start, nint length) {
            if (array == null) {
                if (start != 0 || length != 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException();
                this = default;
                return; // returns default
            }
            unchecked {
                if ((nuint)start > (nuint)array.Length || (nuint)length > (nuint)(array.Length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                _reference1 = new(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), start));
                _length = length;
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(T[], int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan(T[]? array, int start, int length) {
            if (array == null) {
                if (start != 0 || length != 0)
                    ThrowHelper.ThrowArgumentOutOfRangeException();
                this = default;
                return; // returns default
            }
            unchecked {
                if (nint.Size == 8) {
                    // See comment in Span<T>.Slice for how this works.
                    if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)array.Length)
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                } else {
                    if ((uint)start > (uint)array.Length || (uint)length > (uint)(array.Length - start))
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                }

                _reference1 = new(ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(array), (nint)(uint)length /* force zero-extension */));
                _length = length;
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(void*, int)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyBigSpan(void* pointer, nint length) {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException();
            unsafe {
                _reference1 = new(ref *(T*)pointer);
            }
            _length = length;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ReadOnlySpan(void*, int)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe ReadOnlyBigSpan(void* pointer, int length) {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                ThrowHelper.ThrowArgument_TypeContainsReferences(typeof(T));
            if (length < 0)
                ThrowHelper.ThrowArgumentOutOfRangeException();
            unsafe {
                _reference1 = new(ref *(T*)pointer);
            }
            _length = length;
        }

        /// <inheritdoc cref=ReadOnlySpan{T}.ReadOnlySpan(ref T)"/>
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan(ref readonly T reference) {
            _reference1 = new(ref Unsafe.AsRef(in reference));
            _length = 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyBigSpan(ref T reference, nint length) {
            Debug.Assert(length >= 0);

            _reference1 = new(ref reference);
            _length = length;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[int index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((uint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)(uint)index /* force zero-extension */);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((uint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)(uint)index /* force zero-extension */);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((nuint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, index);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((nuint)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, index);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((ulong)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nint)index);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.this[int]"/>
        public ref T this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                unchecked {
                    if ((ulong)index >= (nuint)_length)
                        ThrowHelper.ThrowIndexOutOfRangeException();
                    return ref Unsafe.Add(ref _reference, (nuint)index);
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Length"/>
        public nint Length {

            get => _length;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.IsEmpty"/>
        public bool IsEmpty {

            get => _length == 0;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.operator!=(ReadOnlySpan{T},ReadOnlySpan{T})"/>
        public static bool operator !=(ReadOnlyBigSpan<T> left, ReadOnlyBigSpan<T> right) => !(left == right);

        /// <inheritdoc cref="ReadOnlySpan{T}.Equals(object)"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) =>
            throw new NotSupportedException(SR.NotSupported_CannotCallEqualsOnSpan);

        /// <inheritdoc cref="ReadOnlySpan{T}.GetHashCode()"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() =>
           throw new NotSupportedException(SR.NotSupported_CannotCallGetHashCodeOnSpan);

        /// <inheritdoc cref="ReadOnlySpan{T}.op_Implicit(T[])"/>
        public static implicit operator ReadOnlyBigSpan<T>(BigArray<T>? array) => new ReadOnlyBigSpan<T>(array);

        /// <inheritdoc cref="ReadOnlySpan{T}.op_Implicit(T[])"/>
        public static implicit operator ReadOnlyBigSpan<T>(T[]? array) => new ReadOnlyBigSpan<T>(array);

        /// <inheritdoc cref="ReadOnlySpan{T}.op_Implicit(ArraySegment{T})"/>
        public static implicit operator ReadOnlyBigSpan<T>(ArraySegment<T> segment) =>
            new ReadOnlyBigSpan<T>(segment.Array, segment.Offset, segment.Count);

        /// <inheritdoc cref="ReadOnlyBigSpan{T}.Empty"/>
        public static ReadOnlyBigSpan<T> Empty => default;

        /// <inheritdoc cref="ReadOnlySpan{T}.GetEnumerator()"/>
        public Enumerator GetEnumerator() => new Enumerator(this);

        /// <inheritdoc cref="ReadOnlySpan{T}.Enumerator"/>
        public ref struct Enumerator : IEnumerator<T> {

            private readonly ReadOnlyBigSpan<T> _span;

            private nint _index;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(ReadOnlyBigSpan<T> span) {
                _span = span;
                _index = -1;
            }

            /// <inheritdoc cref="ReadOnlySpan{T}.Enumerator.MoveNext()"/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                nint index = unchecked(_index + 1);
                if (index < _span.Length) {
                    _index = index;
                    return true;
                }

                return false;
            }

            /// <inheritdoc cref="ReadOnlySpan{T}.Enumerator.Current"/>
            public ref readonly T Current {

                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref _span[_index];
            }

            T IEnumerator<T>.Current => Current;

            object System.Collections.IEnumerator.Current => Current!;

            void System.Collections.IEnumerator.Reset() => _index = -1;

            void IDisposable.Dispose() { }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.GetPinnableReference()"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref readonly T GetPinnableReference() {
            ref T ret = ref Unsafe.NullRef<T>();
            if (_length != 0) {
                ret = ref _reference;
            }
            return ref ret;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.CopyTo(Span{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(BigSpan<T> destination) {
            // Using "if (!TryCopyTo(...))" results in two branches: one for the length
            // check, and one for the result of TryCopyTo. Since these checks are equivalent,
            // we can optimize by performing the check once ourselves then calling Memmove directly.
            unchecked {
                if ((nuint)_length <= (nuint)destination.Length) {
                    Buffer.Memmove(ref destination._reference, ref _reference, (nuint)_length);
                } else {
                    ThrowHelper.ThrowArgumentException_DestinationTooShort();
                }
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.TryCopyTo(Span{T})"/>
        public bool TryCopyTo(BigSpan<T> destination) {
            unchecked {
                bool retVal = false;
                if ((nuint)_length <= (nuint)destination.Length) {
                    Buffer.Memmove(ref destination._reference, ref _reference, (nuint)_length);
                    retVal = true;
                }
                return retVal;
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.operator==(ReadOnlySpan{T},ReadOnlySpan{T})"/>
        public static bool operator ==(ReadOnlyBigSpan<T> left, ReadOnlyBigSpan<T> right) =>
            left._length == right._length &&
            Unsafe.AreSame(ref left._reference, ref right._reference);

        /// <inheritdoc cref="ReadOnlySpan{T}.ToString()"/>
        public override string ToString() {
            if (typeof(T) == typeof(char)) {
                return new string(MemoryMarshal.CreateReadOnlySpan<char>(ref Unsafe.As<T, char>(ref _reference), unchecked((int)Math.Min(_length, int.MaxValue))));
            }
            return $"System.Span<{typeof(T).Name}>[{_length}]";
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Slice(int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan<T> Slice(int start) {
            unchecked {
                if ((uint)start > (nuint)_length)
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new ReadOnlyBigSpan<T>(ref Unsafe.Add(ref _reference, (nint)(uint)start /* force zero-extension */), _length - start);
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Slice(int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan<T> Slice(nint start) {
            unchecked {
                if ((nuint)start > (nuint)_length)
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new ReadOnlyBigSpan<T>(ref Unsafe.Add(ref _reference, (nuint)start), _length - start);
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(int start, int length) {
            unchecked {
                if (nint.Size == 8) {
                    if ((ulong)(uint)start + (ulong)(uint)length > (ulong)(uint)_length)
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                } else {
                    if ((uint)start > (nuint)_length || (uint)length > (nuint)(_length - start))
                        ThrowHelper.ThrowArgumentOutOfRangeException();
                }

                return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref _reference, (nint)(uint)start /* force zero-extension */), length);
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<T> Slice(nint start, int length) {
            unchecked {
                if ((nuint)start > (nuint)_length || (uint)length > (nuint)(_length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref _reference, start), length);
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.Slice(int, int)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyBigSpan<T> Slice(nint start, nint length) {
            unchecked {
                if ((nuint)start > (nuint)_length || (nuint)length > (nuint)(_length - start))
                    ThrowHelper.ThrowArgumentOutOfRangeException();

                return new ReadOnlyBigSpan<T>(ref Unsafe.Add(ref _reference, start), length);
            }
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ToArray()"/>
        public T[] ToArray() {
            if (IsEmpty) {
                return [];
            }

            var destination = new T[Length]; // may throw
            CopyTo(destination);
            return destination;
        }

        /// <inheritdoc cref="ReadOnlySpan{T}.ToArray()"/>
        public BigArray<T> ToBigArray() {
            if (IsEmpty) {
                return BigArray<T>.Empty;
            }

            var destination = GC.AllocateUninitializedBigArray<T>(Length);
            CopyTo(destination);
            return destination;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBigSpan<T>(ReadOnlySpan<T> value) {
            return new(ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyBigSpan<T>(Span<T> value) {
            return new(ref MemoryMarshal.GetReference(value), value.Length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator ReadOnlySpan<T>(ReadOnlyBigSpan<T> value) {
            return MemoryMarshal.CreateReadOnlySpan(ref MemoryMarshal.GetReference(value), checked((int)value.Length));
        }
    }
}

namespace UltimateOrb {

    /// <summary>
    /// Represents a sentinel null-like type used for scenarios where
    /// <c>null</c> must be expressed as a distinct type rather than a literal.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="TNull"/> cannot be instantiated. Its static constructor throws
    /// <see cref="NotSupportedException"/> unconditionally, and its instance
    /// constructor is private.
    /// </para>
    /// <para>
    /// The type exposes a single constant, <see cref="Null"/>, which is always
    /// <c>null</c>. This allows <see cref="TNull"/> to participate in generic
    /// type systems that require a type-level representation of null.
    /// </para>
    /// </remarks>
    public class TNull {

        /// <summary>
        /// Gets the canonical null value for <see cref="TNull"/>.  
        /// This constant is always <c>null</c>.
        /// </summary>
        public const TNull? Null = null;

        /// <summary>
        /// A private static field used only to force a reference to
        /// <see cref="object"/> during type initialization.
        /// </summary>
        private static object o = typeof(object);

        /// <summary>
        /// Static constructor.  
        /// Always throws <see cref="NotSupportedException"/> to prevent
        /// type initialization and ensure <see cref="TNull"/> cannot be used
        /// as a normal runtime type.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Always thrown when the type is initialized.
        /// </exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static TNull() {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Private instance constructor.  
        /// Invokes <see cref="object.GetHashCode"/> on a cached <see cref="object"/>
        /// instance solely to prevent the constructor from being empty.
        /// </summary>
        private TNull() { _ = o.GetHashCode(); }
    }
}

namespace UltimateOrb {

    ref partial struct BigSpan<T> {

        public static implicit operator BigSpan<T>(TNull? value) {
            return new(ref Unsafe.NullRef<T>(), 0);
        }
    }
}

namespace UltimateOrb {

    ref partial struct ReadOnlyBigSpan<T> {

        public static implicit operator ReadOnlyBigSpan<T>(TNull? value) {
            return new (ref Unsafe.NullRef<T>(), 0);
        }
    }
}

namespace UltimateOrb {
    internal sealed class BigSpanDebugView<T> {

        private readonly T[] _array;

        public BigSpanDebugView(BigSpan<T> span) {
            if (span.Length <= Array.MaxLength) {
                _array = span.ToArray();
            }
            _array = GC.AllocateUninitializedArray<T>(Array.MaxLength);
            span.Slice(0, c).CopyTo(_array);
            span.Slice(span.Length - (Array.MaxLength - c), Array.MaxLength - c).CopyTo(_array.AsSpan(c, _array.Length - c));
        }

        private int c { get; } = 1 << (63 - (int)Int64.LeadingZeroCount(Array.MaxLength));

        public BigSpanDebugView(ReadOnlyBigSpan<T> span) {
            if (span.Length <= Array.MaxLength) {
                _array = span.ToArray();
            }
            _array = GC.AllocateUninitializedArray<T>(Array.MaxLength);
            span.Slice(0, c).CopyTo(_array);
            span.Slice(span.Length - (Array.MaxLength - c), Array.MaxLength - c).CopyTo(_array.AsSpan(c, _array.Length - c));
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items => _array;
    }
}





namespace UltimateOrb.Internal {

    static partial class ThrowHelper {
        [DoesNotReturn]
        internal static void ThrowUnreachableException() {
            throw new UnreachableException();
        }

        [DoesNotReturn]
        internal static void ThrowArithmeticException(string message) {
            throw new ArithmeticException(message);
        }

        [DoesNotReturn]
        internal static void ThrowAccessViolationException() {
            throw new AccessViolationException();
        }

        [DoesNotReturn]
        internal static void ThrowArrayTypeMismatchException() {
            throw new ArrayTypeMismatchException();
        }

        [DoesNotReturn]
        internal static void ThrowArgument_TypeContainsReferences(Type targetType) {
            throw new ArgumentException(SR.Format(SR.Argument_TypeContainsReferences, targetType));
        }

        [DoesNotReturn]
        internal static void ThrowIndexOutOfRangeException() {
            throw new IndexOutOfRangeException();
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException() {
            throw new ArgumentOutOfRangeException();
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_DestinationTooShort() {
            throw new ArgumentException(SR.Argument_DestinationTooShort, "destination");
        }

        [DoesNotReturn]
        internal static void ThrowSpanTooShortForColor(string? paramName = null) {
            throw new ArgumentException(SR.Arg_SpanMustHaveElementsForColor, paramName);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_InvalidTimeSpanStyles() {
            throw new ArgumentException(SR.Argument_InvalidTimeSpanStyles, "styles");
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_InvalidEnumValue<TEnum>(TEnum value, [CallerArgumentExpression(nameof(value))] string argumentName = "") {
            throw new ArgumentException(SR.Format(SR.Argument_InvalidEnumValue, value, typeof(TEnum).Name), argumentName);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_OverlapAlignmentMismatch() {
            throw new ArgumentException(SR.Argument_OverlapAlignmentMismatch);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_ArgumentNull_TypedRefType() {
            throw new ArgumentNullException("value", SR.ArgumentNull_TypedRefType);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_CannotExtractScalar(ExceptionArgument argument) {
            throw GetArgumentException(ExceptionResource.Argument_CannotExtractScalar, argument);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_TupleIncorrectType(object obj) {
            throw new ArgumentException(SR.Format(SR.ArgumentException_ValueTupleIncorrectType, obj.GetType()), "other");
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_IndexMustBeLessException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLess);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_BadComparer(object? comparer) {
            throw new ArgumentException(SR.Format(SR.Arg_BogusIComparer, comparer));
        }

        [DoesNotReturn]
        internal static void ThrowIndexArgumentOutOfRange_NeedNonNegNumException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.index,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        [DoesNotReturn]
        internal static void ThrowValueArgumentOutOfRange_NeedNonNegNumException() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.value,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        [DoesNotReturn]
        internal static void ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.length,
                                                    ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
        }

        [DoesNotReturn]
        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLessOrEqual() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.startIndex,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual);
        }

        [DoesNotReturn]
        internal static void ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.startIndex,
                                                    ExceptionResource.ArgumentOutOfRange_IndexMustBeLess);
        }

        [DoesNotReturn]
        internal static void ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.count,
                                                    ExceptionResource.ArgumentOutOfRange_Count);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Year() {
            throw GetArgumentOutOfRangeException(ExceptionArgument.year,
                                                    ExceptionResource.ArgumentOutOfRange_Year);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Month(int month) {
            throw new ArgumentOutOfRangeException(nameof(month), month, SR.ArgumentOutOfRange_Month);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_DayNumber(int dayNumber) {
            throw new ArgumentOutOfRangeException(nameof(dayNumber), dayNumber, SR.ArgumentOutOfRange_DayNumber);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_BadYearMonthDay() {
            throw new ArgumentOutOfRangeException(null, SR.ArgumentOutOfRange_BadYearMonthDay);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_BadHourMinuteSecond() {
            throw new ArgumentOutOfRangeException(null, SR.ArgumentOutOfRange_BadHourMinuteSecond);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_TimeSpanTooLong() {
            throw new ArgumentOutOfRangeException(null, SR.Overflow_TimeSpanTooLong);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_RoundingDigits(string name) {
            throw new ArgumentOutOfRangeException(name, SR.ArgumentOutOfRange_RoundingDigits);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_Range<T>(string parameterName, T value, T minInclusive, T maxInclusive) {
            throw new ArgumentOutOfRangeException(parameterName, value, SR.Format(SR.ArgumentOutOfRange_Range, minInclusive, maxInclusive));
        }

        [DoesNotReturn]
        internal static void ThrowOverflowException() {
            throw new OverflowException();
        }

        [DoesNotReturn]
        internal static void ThrowOverflowException_NegateTwosCompNum() {
            throw new OverflowException(SR.Overflow_NegateTwosCompNum);
        }

        [DoesNotReturn]
        internal static void ThrowOverflowException_TimeSpanTooLong() {
            throw new OverflowException(SR.Overflow_TimeSpanTooLong);
        }

        [DoesNotReturn]
        internal static void ThrowOverflowException_TimeSpanDuration() {
            throw new OverflowException(SR.Overflow_Duration);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_Arg_CannotBeNaN() {
            throw new ArgumentException(SR.Arg_CannotBeNaN);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_Arg_CannotBeNaN(ExceptionArgument argument) {
            throw new ArgumentException(SR.Arg_CannotBeNaN, GetArgumentName(argument));
        }

        [DoesNotReturn]
        internal static void ThrowWrongKeyTypeArgumentException<T>(T key, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongKeyTypeArgumentException((object?)key, targetType);
        }

        [DoesNotReturn]
        internal static void ThrowWrongValueTypeArgumentException<T>(T value, Type targetType) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetWrongValueTypeArgumentException((object?)value, targetType);
        }

        private static ArgumentException GetAddingDuplicateWithKeyArgumentException(object? key) {
            return new ArgumentException(SR.Format(SR.Argument_AddingDuplicateWithKey, key));
        }

        [DoesNotReturn]
        internal static void ThrowAddingDuplicateWithKeyArgumentException<T>(T key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetAddingDuplicateWithKeyArgumentException((object?)key);
        }

        [DoesNotReturn]
        internal static void ThrowKeyNotFoundException<T>(T key) {
            // Generic key to move the boxing to the right hand side of throw
            throw GetKeyNotFoundException((object?)key);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException(ExceptionResource resource) {
            throw GetArgumentException(resource);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument) {
            throw GetArgumentException(resource, argument);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument) {
            throw new ArgumentNullException(GetArgumentName(argument));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionResource resource) {
            throw new ArgumentNullException(GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentNullException(ExceptionArgument argument, ExceptionResource resource) {
            throw new ArgumentNullException(GetArgumentName(argument), GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument) {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            throw GetArgumentOutOfRangeException(argument, resource);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource) {
            throw GetArgumentOutOfRangeException(argument, paramNumber, resource);
        }

        [DoesNotReturn]
        internal static void ThrowEndOfFileException() {
            throw CreateEndOfFileException();
        }

        internal static Exception CreateEndOfFileException() =>
            new EndOfStreamException(SR.IO_EOF_ReadBeyondEOF);

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException() {
            throw new InvalidOperationException();
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException(ExceptionResource resource) {
            throw GetInvalidOperationException(resource);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException(ExceptionResource resource, Exception e) {
            throw new InvalidOperationException(GetResourceString(resource), e);
        }

        [DoesNotReturn]
        internal static void ThrowNullReferenceException() {
            throw new NullReferenceException(SR.Arg_NullArgumentNullRef);
        }

        [DoesNotReturn]
        internal static void ThrowSerializationException(ExceptionResource resource) {
            throw new SerializationException(GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowRankException(ExceptionResource resource) {
            throw new RankException(GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException(ExceptionResource resource) {
            throw new NotSupportedException(GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException_UnseekableStream() {
            throw new NotSupportedException(SR.NotSupported_UnseekableStream);
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException_UnreadableStream() {
            throw new NotSupportedException(SR.NotSupported_UnreadableStream);
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException_UnwritableStream() {
            throw new NotSupportedException(SR.NotSupported_UnwritableStream);
        }

        [DoesNotReturn]
        internal static void ThrowObjectDisposedException(object? instance) {
            throw new ObjectDisposedException(instance?.GetType().FullName);
        }

        [DoesNotReturn]
        internal static void ThrowObjectDisposedException(Type? type) {
            throw new ObjectDisposedException(type?.FullName);
        }

        [DoesNotReturn]
        internal static void ThrowObjectDisposedException_StreamClosed(string? objectName) {
            throw new ObjectDisposedException(objectName, SR.ObjectDisposed_StreamClosed);
        }

        [DoesNotReturn]
        internal static void ThrowObjectDisposedException_FileClosed() {
            throw new ObjectDisposedException(null, SR.ObjectDisposed_FileClosed);
        }

        [DoesNotReturn]
        internal static void ThrowObjectDisposedException(ExceptionResource resource) {
            throw new ObjectDisposedException(null, GetResourceString(resource));
        }

        [DoesNotReturn]
        internal static void ThrowNotSupportedException() {
            throw new NotSupportedException();
        }

        [DoesNotReturn]
        internal static void ThrowAggregateException(List<Exception> exceptions) {
            throw new AggregateException(exceptions);
        }

        [DoesNotReturn]
        internal static void ThrowOutOfMemoryException() {
            throw new OutOfMemoryException();
        }

        [DoesNotReturn]
        internal static void ThrowDivideByZeroException() {
            throw new DivideByZeroException();
        }

        [DoesNotReturn]
        internal static void ThrowOutOfMemoryException_StringTooLong() {
            throw new OutOfMemoryException(SR.OutOfMemory_StringTooLong);
        }

        [DoesNotReturn]
        internal static void ThrowOutOfMemoryException_LockEnter_WaiterCountOverflow() {
            throw new OutOfMemoryException(SR.Lock_Enter_WaiterCountOverflow_OutOfMemoryException);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_Argument_IncompatibleArrayType() {
            throw new ArgumentException(SR.Argument_IncompatibleArrayType);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_InvalidHandle(string? paramName) {
            throw new ArgumentException(SR.Arg_InvalidHandle, paramName);
        }

        [DoesNotReturn]
        internal static void ThrowUnexpectedStateForKnownCallback(object? state) {
            throw new ArgumentOutOfRangeException(nameof(state), state, SR.Argument_UnexpectedStateForKnownCallback);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumNotStarted() {
            throw new InvalidOperationException(SR.InvalidOperation_EnumNotStarted);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumEnded() {
            throw new InvalidOperationException(SR.InvalidOperation_EnumEnded);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_EnumCurrent(int index) {
            throw GetInvalidOperationException_EnumCurrent(index);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion() {
            throw new InvalidOperationException(SR.InvalidOperation_EnumFailedVersion);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen() {
            throw new InvalidOperationException(SR.InvalidOperation_EnumOpCantHappen);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidOperation_NoValue() {
            throw new InvalidOperationException(SR.InvalidOperation_NoValue);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_ConcurrentOperationsNotSupported() {
            throw new InvalidOperationException(SR.InvalidOperation_ConcurrentOperationsNotSupported);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_HandleIsNotInitialized() {
            throw new InvalidOperationException(SR.InvalidOperation_HandleIsNotInitialized);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_HandleIsNotPinned() {
            throw new InvalidOperationException(SR.InvalidOperation_HandleIsNotPinned);
        }

        [DoesNotReturn]
        internal static void ThrowArraySegmentCtorValidationFailedExceptions(Array? array, int offset, int count) {
            throw GetArraySegmentCtorValidationFailedException(array, offset, count);
        }

        [DoesNotReturn]
        internal static void ThrowInvalidOperationException_InvalidUtf8() {
            throw new InvalidOperationException(SR.InvalidOperation_InvalidUtf8);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException_BadFormatSpecifier() {
            throw new FormatException(SR.Argument_BadFormatSpecifier);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException_BadHexChar() {
            throw new FormatException(SR.Format_BadHexChar);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException_BadHexLength() {
            throw new FormatException(SR.Format_BadHexLength);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException_NeedSingleChar() {
            throw new FormatException(SR.Format_NeedSingleChar);
        }

        [DoesNotReturn]
        internal static void ThrowFormatException_BadBoolean(ReadOnlySpan<char> value) {
            throw new FormatException(SR.Format(SR.Format_BadBoolean, new string(value)));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_PrecisionTooLarge() {
            throw new ArgumentOutOfRangeException("precision", SR.Format(SR.Argument_PrecisionTooLarge, StandardFormat.MaxPrecision));
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_SymbolDoesNotFit() {
            throw new ArgumentOutOfRangeException("symbol", SR.Argument_BadFormatSpecifier);
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException_NeedNonNegNum(string paramName) {
            throw new ArgumentOutOfRangeException(paramName, SR.ArgumentOutOfRange_NeedNonNegNum);
        }

        [DoesNotReturn]
        internal static void ArgumentOutOfRangeException_Enum_Value() {
            throw new ArgumentOutOfRangeException("value", SR.ArgumentOutOfRange_Enum);
        }

        [DoesNotReturn]
        internal static void ThrowApplicationException(int hr) {
            // Get a message for this HR
            Exception? ex = Marshal.GetExceptionForHR(hr);
            if (ex != null && !string.IsNullOrEmpty(ex.Message)) {
                ex = new ApplicationException(ex.Message);
            } else {
                ex = new ApplicationException();
            }

            ex.HResult = hr;
            throw ex;
        }

        [DoesNotReturn]
        internal static void ThrowFormatInvalidString() {
            throw new FormatException(SR.Format_InvalidString);
        }

        [DoesNotReturn]
        internal static void ThrowFormatInvalidString(int offset, ExceptionResource resource) {
            throw new FormatException(SR.Format(SR.Format_InvalidStringWithOffsetAndReason, offset, GetResourceString(resource)));
        }

        [DoesNotReturn]
        internal static void ThrowFormatIndexOutOfRange() {
            throw new FormatException(SR.Format_IndexOutOfRange);
        }

        [DoesNotReturn]
        internal static void ThrowSynchronizationLockException_LockExit() {
            throw new SynchronizationLockException(SR.Lock_Exit_SynchronizationLockException);
        }

        internal static AmbiguousMatchException GetAmbiguousMatchException(MemberInfo memberInfo) {
            Type? declaringType = memberInfo.DeclaringType;
            return new AmbiguousMatchException(SR.Format(SR.Arg_AmbiguousMatchException_MemberInfo, declaringType, memberInfo));
        }

        internal static AmbiguousMatchException GetAmbiguousMatchException(Attribute attribute) {
            return new AmbiguousMatchException(SR.Format(SR.Arg_AmbiguousMatchException_Attribute, attribute));
        }

        /*internal static AmbiguousMatchException GetAmbiguousMatchException(CustomAttributeData customAttributeData) {
            return new AmbiguousMatchException(SR.Format(SR.Arg_AmbiguousMatchException_CustomAttributeData, customAttributeData));
        }*/

        private static Exception GetArraySegmentCtorValidationFailedException(Array? array, int offset, int count) {
            if (array == null)
                return new ArgumentNullException(nameof(array));
            if (offset < 0)
                return new ArgumentOutOfRangeException(nameof(offset), SR.ArgumentOutOfRange_NeedNonNegNum);
            if (count < 0)
                return new ArgumentOutOfRangeException(nameof(count), SR.ArgumentOutOfRange_NeedNonNegNum);

            Debug.Assert(array.Length - offset < count);
            return new ArgumentException(SR.Argument_InvalidOffLen);
        }

        private static ArgumentException GetArgumentException(ExceptionResource resource) {
            return new ArgumentException(GetResourceString(resource));
        }

        private static InvalidOperationException GetInvalidOperationException(ExceptionResource resource) {
            return new InvalidOperationException(GetResourceString(resource));
        }

        private static ArgumentException GetWrongKeyTypeArgumentException(object? key, Type targetType) {
            return new ArgumentException(SR.Format(SR.Arg_WrongType, key, targetType), nameof(key));
        }

        private static ArgumentException GetWrongValueTypeArgumentException(object? value, Type targetType) {
            return new ArgumentException(SR.Format(SR.Arg_WrongType, value, targetType), nameof(value));
        }

        private static KeyNotFoundException GetKeyNotFoundException(object? key) {
            return new KeyNotFoundException(SR.Format(SR.Arg_KeyNotFoundWithKey, key));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
            return new ArgumentOutOfRangeException(GetArgumentName(argument), GetResourceString(resource));
        }

        private static ArgumentException GetArgumentException(ExceptionResource resource, ExceptionArgument argument) {
            return new ArgumentException(GetResourceString(resource), GetArgumentName(argument));
        }

        private static ArgumentOutOfRangeException GetArgumentOutOfRangeException(ExceptionArgument argument, int paramNumber, ExceptionResource resource) {
            return new ArgumentOutOfRangeException(GetArgumentName(argument) + "[" + paramNumber.ToString() + "]", GetResourceString(resource));
        }

        private static InvalidOperationException GetInvalidOperationException_EnumCurrent(int index) {
            return new InvalidOperationException(
                index < 0 ?
                SR.InvalidOperation_EnumNotStarted :
                SR.InvalidOperation_EnumEnded);
        }

        // Allow nulls for reference types and Nullable<U>, but not for value types.
        // Aggressively inline so the jit evaluates the if in place and either drops the call altogether
        // Or just leaves null test and call to the Non-returning ThrowHelper.ThrowArgumentNullException
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IfNullAndNullsAreIllegalThenThrow<T>(object? value, ExceptionArgument argName) {
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>.
            if (!(default(T) == null) && value == null)
                ThrowArgumentNullException(argName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedSimdVectorBaseType<TVector, T>()
            /*where TVector : ISimdVector<TVector, T>*/ {
            bool isSupported = false;
            if (typeof(TVector) == typeof(Vector<T>)) {
                isSupported = Vector<T>.IsSupported;
            } else if (typeof(TVector) == typeof(Vector64<T>)) {
                isSupported = Vector64<T>.IsSupported;
            } else if (typeof(TVector) == typeof(Vector128<T>)) {
                isSupported = Vector128<T>.IsSupported;
            } else if (typeof(TVector) == typeof(Vector256<T>)) {
                isSupported = Vector256<T>.IsSupported;
            } else if (typeof(TVector) == typeof(Vector512<T>)) {
                isSupported = Vector512<T>.IsSupported;
            }
            if (!isSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector<T> in the Numerics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedNumericsVectorBaseType<T>() {
            if (!Vector<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector64<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector64BaseType<T>() {
            if (!Vector64<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector128<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector128BaseType<T>() {
            if (!Vector128<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector256<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector256BaseType<T>() {
            if (!Vector256<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

        // Throws if 'T' is disallowed in Vector512<T> in the Intrinsics namespace.
        // If 'T' is allowed, no-ops. JIT will elide the method entirely if 'T'
        // is supported and we're on an optimized release build.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ThrowForUnsupportedIntrinsicsVector512BaseType<T>() {
            if (!Vector512<T>.IsSupported) {
                ThrowNotSupportedException(ExceptionResource.Arg_TypeNotSupported);
            }
        }

#if false // Reflection-based implementation does not work for NativeAOT
        // This function will convert an ExceptionArgument enum value to the argument name string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetArgumentName(ExceptionArgument argument)
        {
            Debug.Assert(Enum.IsDefined(argument),
                "The enum value is not defined, please check the ExceptionArgument Enum.");

            return argument.ToString();
        }
#endif

        private static string GetArgumentName(ExceptionArgument argument) {
            switch (argument) {
            case ExceptionArgument.obj:
                return "obj";
            case ExceptionArgument.dictionary:
                return "dictionary";
            case ExceptionArgument.array:
                return "array";
            case ExceptionArgument.info:
                return "info";
            case ExceptionArgument.key:
                return "key";
            case ExceptionArgument.text:
                return "text";
            case ExceptionArgument.values:
                return "values";
            case ExceptionArgument.value:
                return "value";
            case ExceptionArgument.startIndex:
                return "startIndex";
            case ExceptionArgument.task:
                return "task";
            case ExceptionArgument.bytes:
                return "bytes";
            case ExceptionArgument.byteIndex:
                return "byteIndex";
            case ExceptionArgument.byteCount:
                return "byteCount";
            case ExceptionArgument.ch:
                return "ch";
            case ExceptionArgument.chars:
                return "chars";
            case ExceptionArgument.charIndex:
                return "charIndex";
            case ExceptionArgument.charCount:
                return "charCount";
            case ExceptionArgument.s:
                return "s";
            case ExceptionArgument.input:
                return "input";
            case ExceptionArgument.ownedMemory:
                return "ownedMemory";
            case ExceptionArgument.list:
                return "list";
            case ExceptionArgument.index:
                return "index";
            case ExceptionArgument.capacity:
                return "capacity";
            case ExceptionArgument.collection:
                return "collection";
            case ExceptionArgument.item:
                return "item";
            case ExceptionArgument.converter:
                return "converter";
            case ExceptionArgument.match:
                return "match";
            case ExceptionArgument.count:
                return "count";
            case ExceptionArgument.action:
                return "action";
            case ExceptionArgument.comparison:
                return "comparison";
            case ExceptionArgument.exceptions:
                return "exceptions";
            case ExceptionArgument.exception:
                return "exception";
            case ExceptionArgument.pointer:
                return "pointer";
            case ExceptionArgument.start:
                return "start";
            case ExceptionArgument.format:
                return "format";
            case ExceptionArgument.formats:
                return "formats";
            case ExceptionArgument.culture:
                return "culture";
            case ExceptionArgument.comparer:
                return "comparer";
            case ExceptionArgument.comparable:
                return "comparable";
            case ExceptionArgument.source:
                return "source";
            case ExceptionArgument.length:
                return "length";
            case ExceptionArgument.comparisonType:
                return "comparisonType";
            case ExceptionArgument.manager:
                return "manager";
            case ExceptionArgument.sourceBytesToCopy:
                return "sourceBytesToCopy";
            case ExceptionArgument.callBack:
                return "callBack";
            case ExceptionArgument.creationOptions:
                return "creationOptions";
            case ExceptionArgument.function:
                return "function";
            case ExceptionArgument.scheduler:
                return "scheduler";
            case ExceptionArgument.continuation:
                return "continuation";
            case ExceptionArgument.continuationAction:
                return "continuationAction";
            case ExceptionArgument.continuationFunction:
                return "continuationFunction";
            case ExceptionArgument.tasks:
                return "tasks";
            case ExceptionArgument.asyncResult:
                return "asyncResult";
            case ExceptionArgument.beginMethod:
                return "beginMethod";
            case ExceptionArgument.endMethod:
                return "endMethod";
            case ExceptionArgument.endFunction:
                return "endFunction";
            case ExceptionArgument.cancellationToken:
                return "cancellationToken";
            case ExceptionArgument.continuationOptions:
                return "continuationOptions";
            case ExceptionArgument.delay:
                return "delay";
            case ExceptionArgument.millisecondsDelay:
                return "millisecondsDelay";
            case ExceptionArgument.millisecondsTimeout:
                return "millisecondsTimeout";
            case ExceptionArgument.stateMachine:
                return "stateMachine";
            case ExceptionArgument.timeout:
                return "timeout";
            case ExceptionArgument.type:
                return "type";
            case ExceptionArgument.sourceIndex:
                return "sourceIndex";
            case ExceptionArgument.destinationIndex:
                return "destinationIndex";
            case ExceptionArgument.pHandle:
                return "pHandle";
            case ExceptionArgument.handle:
                return "handle";
            case ExceptionArgument.other:
                return "other";
            case ExceptionArgument.newSize:
                return "newSize";
            case ExceptionArgument.lengths:
                return "lengths";
            case ExceptionArgument.len:
                return "len";
            case ExceptionArgument.keys:
                return "keys";
            case ExceptionArgument.indices:
                return "indices";
            case ExceptionArgument.index1:
                return "index1";
            case ExceptionArgument.index2:
                return "index2";
            case ExceptionArgument.index3:
                return "index3";
            case ExceptionArgument.endIndex:
                return "endIndex";
            case ExceptionArgument.elementType:
                return "elementType";
            case ExceptionArgument.arrayIndex:
                return "arrayIndex";
            case ExceptionArgument.year:
                return "year";
            case ExceptionArgument.codePoint:
                return "codePoint";
            case ExceptionArgument.str:
                return "str";
            case ExceptionArgument.options:
                return "options";
            case ExceptionArgument.prefix:
                return "prefix";
            case ExceptionArgument.suffix:
                return "suffix";
            case ExceptionArgument.buffer:
                return "buffer";
            case ExceptionArgument.buffers:
                return "buffers";
            case ExceptionArgument.offset:
                return "offset";
            case ExceptionArgument.stream:
                return "stream";
            case ExceptionArgument.anyOf:
                return "anyOf";
            case ExceptionArgument.overlapped:
                return "overlapped";
            case ExceptionArgument.minimumBytes:
                return "minimumBytes";
            case ExceptionArgument.arrayType:
                return "arrayType";
            case ExceptionArgument.divisor:
                return "divisor";
            case ExceptionArgument.factor:
                return "factor";
            case ExceptionArgument.set:
                return "set";
            case ExceptionArgument.valueFactory:
                return "valueFactory";
            case ExceptionArgument.addValueFactory:
                return "addValueFactory";
            case ExceptionArgument.updateValueFactory:
                return "updateValueFactory";
            default:
                Debug.Fail("The enum value is not defined, please check the ExceptionArgument Enum.");
                return "";
            }
        }

#if false // Reflection-based implementation does not work for NativeAOT
        // This function will convert an ExceptionResource enum value to the resource string.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static string GetResourceString(ExceptionResource resource)
        {
            Debug.Assert(Enum.IsDefined(resource),
                "The enum value is not defined, please check the ExceptionResource Enum.");

            return SR.GetResourceString(resource.ToString());
        }
#endif

        private static string GetResourceString(ExceptionResource resource) {
            switch (resource) {
            case ExceptionResource.ArgumentOutOfRange_IndexMustBeLessOrEqual:
                return SR.ArgumentOutOfRange_IndexMustBeLessOrEqual;
            case ExceptionResource.ArgumentOutOfRange_IndexMustBeLess:
                return SR.ArgumentOutOfRange_IndexMustBeLess;
            case ExceptionResource.ArgumentOutOfRange_IndexCount:
                return SR.ArgumentOutOfRange_IndexCount;
            case ExceptionResource.ArgumentOutOfRange_IndexCountBuffer:
                return SR.ArgumentOutOfRange_IndexCountBuffer;
            case ExceptionResource.ArgumentOutOfRange_Count:
                return SR.ArgumentOutOfRange_Count;
            case ExceptionResource.ArgumentOutOfRange_Year:
                return SR.ArgumentOutOfRange_Year;
            case ExceptionResource.Arg_ArrayPlusOffTooSmall:
                return SR.Arg_ArrayPlusOffTooSmall;
            case ExceptionResource.Arg_ByteArrayTooSmallForValue:
                return SR.Arg_ByteArrayTooSmallForValue;
            case ExceptionResource.NotSupported_ReadOnlyCollection:
                return SR.NotSupported_ReadOnlyCollection;
            case ExceptionResource.Arg_RankMultiDimNotSupported:
                return SR.Arg_RankMultiDimNotSupported;
            case ExceptionResource.Arg_NonZeroLowerBound:
                return SR.Arg_NonZeroLowerBound;
            case ExceptionResource.ArgumentOutOfRange_GetCharCountOverflow:
                return SR.ArgumentOutOfRange_GetCharCountOverflow;
            case ExceptionResource.ArgumentOutOfRange_ListInsert:
                return SR.ArgumentOutOfRange_ListInsert;
            case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
                return SR.ArgumentOutOfRange_NeedNonNegNum;
            case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
                return SR.ArgumentOutOfRange_SmallCapacity;
            case ExceptionResource.Argument_InvalidOffLen:
                return SR.Argument_InvalidOffLen;
            case ExceptionResource.Argument_CannotExtractScalar:
                return SR.Argument_CannotExtractScalar;
            case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
                return SR.ArgumentOutOfRange_BiggerThanCollection;
            case ExceptionResource.Serialization_MissingKeys:
                return SR.Serialization_MissingKeys;
            case ExceptionResource.Serialization_NullKey:
                return SR.Serialization_NullKey;
            case ExceptionResource.NotSupported_KeyCollectionSet:
                return SR.NotSupported_KeyCollectionSet;
            case ExceptionResource.NotSupported_ValueCollectionSet:
                return SR.NotSupported_ValueCollectionSet;
            case ExceptionResource.InvalidOperation_NullArray:
                return SR.InvalidOperation_NullArray;
            case ExceptionResource.TaskT_TransitionToFinal_AlreadyCompleted:
                return SR.TaskT_TransitionToFinal_AlreadyCompleted;
            case ExceptionResource.TaskCompletionSourceT_TrySetException_NullException:
                return SR.TaskCompletionSourceT_TrySetException_NullException;
            case ExceptionResource.TaskCompletionSourceT_TrySetException_NoExceptions:
                return SR.TaskCompletionSourceT_TrySetException_NoExceptions;
            case ExceptionResource.NotSupported_StringComparison:
                return SR.NotSupported_StringComparison;
            case ExceptionResource.ConcurrentCollection_SyncRoot_NotSupported:
                return SR.ConcurrentCollection_SyncRoot_NotSupported;
            case ExceptionResource.Task_MultiTaskContinuation_NullTask:
                return SR.Task_MultiTaskContinuation_NullTask;
            case ExceptionResource.InvalidOperation_WrongAsyncResultOrEndCalledMultiple:
                return SR.InvalidOperation_WrongAsyncResultOrEndCalledMultiple;
            case ExceptionResource.Task_MultiTaskContinuation_EmptyTaskList:
                return SR.Task_MultiTaskContinuation_EmptyTaskList;
            case ExceptionResource.Task_Start_TaskCompleted:
                return SR.Task_Start_TaskCompleted;
            case ExceptionResource.Task_Start_Promise:
                return SR.Task_Start_Promise;
            case ExceptionResource.Task_Start_ContinuationTask:
                return SR.Task_Start_ContinuationTask;
            case ExceptionResource.Task_Start_AlreadyStarted:
                return SR.Task_Start_AlreadyStarted;
            case ExceptionResource.Task_RunSynchronously_Continuation:
                return SR.Task_RunSynchronously_Continuation;
            case ExceptionResource.Task_RunSynchronously_Promise:
                return SR.Task_RunSynchronously_Promise;
            case ExceptionResource.Task_RunSynchronously_TaskCompleted:
                return SR.Task_RunSynchronously_TaskCompleted;
            case ExceptionResource.Task_RunSynchronously_AlreadyStarted:
                return SR.Task_RunSynchronously_AlreadyStarted;
            case ExceptionResource.AsyncMethodBuilder_InstanceNotInitialized:
                return SR.AsyncMethodBuilder_InstanceNotInitialized;
            case ExceptionResource.Task_ContinueWith_ESandLR:
                return SR.Task_ContinueWith_ESandLR;
            case ExceptionResource.Task_ContinueWith_NotOnAnything:
                return SR.Task_ContinueWith_NotOnAnything;
            case ExceptionResource.Task_InvalidTimerTimeSpan:
                return SR.Task_InvalidTimerTimeSpan;
            case ExceptionResource.Task_Delay_InvalidMillisecondsDelay:
                return SR.Task_Delay_InvalidMillisecondsDelay;
            case ExceptionResource.Task_Dispose_NotCompleted:
                return SR.Task_Dispose_NotCompleted;
            case ExceptionResource.Task_ThrowIfDisposed:
                return SR.Task_ThrowIfDisposed;
            case ExceptionResource.Task_WaitMulti_NullTask:
                return SR.Task_WaitMulti_NullTask;
            case ExceptionResource.ArgumentException_OtherNotArrayOfCorrectLength:
                return SR.ArgumentException_OtherNotArrayOfCorrectLength;
            case ExceptionResource.ArgumentNull_Array:
                return SR.ArgumentNull_Array;
            case ExceptionResource.ArgumentNull_SafeHandle:
                return SR.ArgumentNull_SafeHandle;
            case ExceptionResource.ArgumentOutOfRange_EndIndexStartIndex:
                return SR.ArgumentOutOfRange_EndIndexStartIndex;
            case ExceptionResource.ArgumentOutOfRange_Enum:
                return SR.ArgumentOutOfRange_Enum;
            case ExceptionResource.ArgumentOutOfRange_HugeArrayNotSupported:
                return SR.ArgumentOutOfRange_HugeArrayNotSupported;
            case ExceptionResource.Argument_AddingDuplicate:
                return SR.Argument_AddingDuplicate;
            case ExceptionResource.Argument_InvalidArgumentForComparison:
                return SR.Argument_InvalidArgumentForComparison;
            case ExceptionResource.Arg_LowerBoundsMustMatch:
                return SR.Arg_LowerBoundsMustMatch;
            case ExceptionResource.Arg_MustBeType:
                return SR.Arg_MustBeType;
            case ExceptionResource.Arg_Need1DArray:
                return SR.Arg_Need1DArray;
            case ExceptionResource.Arg_Need2DArray:
                return SR.Arg_Need2DArray;
            case ExceptionResource.Arg_Need3DArray:
                return SR.Arg_Need3DArray;
            case ExceptionResource.Arg_NeedAtLeast1Rank:
                return SR.Arg_NeedAtLeast1Rank;
            case ExceptionResource.Arg_RankIndices:
                return SR.Arg_RankIndices;
            case ExceptionResource.Arg_RanksAndBounds:
                return SR.Arg_RanksAndBounds;
            case ExceptionResource.InvalidOperation_IComparerFailed:
                return SR.InvalidOperation_IComparerFailed;
            case ExceptionResource.NotSupported_FixedSizeCollection:
                return SR.NotSupported_FixedSizeCollection;
            case ExceptionResource.Rank_MultiDimNotSupported:
                return SR.Rank_MultiDimNotSupported;
            case ExceptionResource.Arg_TypeNotSupported:
                return SR.Arg_TypeNotSupported;
            case ExceptionResource.Argument_SpansMustHaveSameLength:
                return SR.Argument_SpansMustHaveSameLength;
            case ExceptionResource.Argument_InvalidFlag:
                return SR.Argument_InvalidFlag;
            case ExceptionResource.CancellationTokenSource_Disposed:
                return SR.CancellationTokenSource_Disposed;
            case ExceptionResource.Argument_AlignmentMustBePow2:
                return SR.Argument_AlignmentMustBePow2;
            case ExceptionResource.ArgumentOutOfRange_NotGreaterThanBufferLength:
                return SR.ArgumentOutOfRange_NotGreaterThanBufferLength;
            case ExceptionResource.InvalidOperation_SpanOverlappedOperation:
                return SR.InvalidOperation_SpanOverlappedOperation;
            case ExceptionResource.InvalidOperation_TimeProviderNullLocalTimeZone:
                return SR.InvalidOperation_TimeProviderNullLocalTimeZone;
            case ExceptionResource.InvalidOperation_TimeProviderInvalidTimestampFrequency:
                return SR.InvalidOperation_TimeProviderInvalidTimestampFrequency;
            case ExceptionResource.Format_UnexpectedClosingBrace:
                return SR.Format_UnexpectedClosingBrace;
            case ExceptionResource.Format_UnclosedFormatItem:
                return SR.Format_UnclosedFormatItem;
            case ExceptionResource.Format_ExpectedAsciiDigit:
                return SR.Format_ExpectedAsciiDigit;
            case ExceptionResource.Argument_HasToBeArrayClass:
                return SR.Argument_HasToBeArrayClass;
            case ExceptionResource.InvalidOperation_IncompatibleComparer:
                return SR.InvalidOperation_IncompatibleComparer;
            case ExceptionResource.ConcurrentDictionary_ItemKeyIsNull:
                return SR.ConcurrentDictionary_ItemKeyIsNull;
            case ExceptionResource.ConcurrentDictionary_TypeOfValueIncorrect:
                return SR.ConcurrentDictionary_TypeOfValueIncorrect;
            case ExceptionResource.InvalidOperation_NoElements:
                return SR.InvalidOperation_NoElements;
            default:
                Debug.Fail("The enum value is not defined, please check the ExceptionResource Enum.");
                return "";
            }
        }
    }

    //
    // The convention for this enum is using the argument name as the enum name
    //
    internal enum ExceptionArgument {
        obj,
        dictionary,
        array,
        info,
        key,
        text,
        values,
        value,
        startIndex,
        task,
        bytes,
        byteIndex,
        byteCount,
        ch,
        chars,
        charIndex,
        charCount,
        s,
        input,
        ownedMemory,
        list,
        index,
        capacity,
        collection,
        item,
        converter,
        match,
        count,
        action,
        comparison,
        exceptions,
        exception,
        pointer,
        start,
        format,
        formats,
        culture,
        comparer,
        comparable,
        source,
        length,
        comparisonType,
        manager,
        sourceBytesToCopy,
        callBack,
        creationOptions,
        function,
        scheduler,
        continuation,
        continuationAction,
        continuationFunction,
        tasks,
        asyncResult,
        beginMethod,
        endMethod,
        endFunction,
        cancellationToken,
        continuationOptions,
        delay,
        millisecondsDelay,
        millisecondsTimeout,
        stateMachine,
        timeout,
        type,
        sourceIndex,
        destinationIndex,
        pHandle,
        handle,
        other,
        newSize,
        lengths,
        len,
        keys,
        indices,
        index1,
        index2,
        index3,
        endIndex,
        elementType,
        arrayIndex,
        year,
        codePoint,
        str,
        options,
        prefix,
        suffix,
        buffer,
        buffers,
        offset,
        stream,
        anyOf,
        overlapped,
        minimumBytes,
        arrayType,
        divisor,
        factor,
        set,
        valueFactory,
        addValueFactory,
        updateValueFactory
    }
    //
    // The convention for this enum is using the resource name as the enum name
    //
    internal enum ExceptionResource {
        ArgumentOutOfRange_IndexMustBeLessOrEqual,
        ArgumentOutOfRange_IndexMustBeLess,
        ArgumentOutOfRange_IndexCount,
        ArgumentOutOfRange_IndexCountBuffer,
        ArgumentOutOfRange_Count,
        ArgumentOutOfRange_Year,
        Arg_ArrayPlusOffTooSmall,
        Arg_ByteArrayTooSmallForValue,
        NotSupported_ReadOnlyCollection,
        Arg_RankMultiDimNotSupported,
        Arg_NonZeroLowerBound,
        ArgumentOutOfRange_GetCharCountOverflow,
        ArgumentOutOfRange_ListInsert,
        ArgumentOutOfRange_NeedNonNegNum,
        ArgumentOutOfRange_NotGreaterThanBufferLength,
        ArgumentOutOfRange_SmallCapacity,
        Argument_InvalidOffLen,
        Argument_CannotExtractScalar,
        ArgumentOutOfRange_BiggerThanCollection,
        Serialization_MissingKeys,
        Serialization_NullKey,
        NotSupported_KeyCollectionSet,
        NotSupported_ValueCollectionSet,
        InvalidOperation_NullArray,
        TaskT_TransitionToFinal_AlreadyCompleted,
        TaskCompletionSourceT_TrySetException_NullException,
        TaskCompletionSourceT_TrySetException_NoExceptions,
        NotSupported_StringComparison,
        ConcurrentCollection_SyncRoot_NotSupported,
        Task_MultiTaskContinuation_NullTask,
        InvalidOperation_WrongAsyncResultOrEndCalledMultiple,
        Task_MultiTaskContinuation_EmptyTaskList,
        Task_Start_TaskCompleted,
        Task_Start_Promise,
        Task_Start_ContinuationTask,
        Task_Start_AlreadyStarted,
        Task_RunSynchronously_Continuation,
        Task_RunSynchronously_Promise,
        Task_RunSynchronously_TaskCompleted,
        Task_RunSynchronously_AlreadyStarted,
        AsyncMethodBuilder_InstanceNotInitialized,
        Task_ContinueWith_ESandLR,
        Task_ContinueWith_NotOnAnything,
        Task_InvalidTimerTimeSpan,
        Task_Delay_InvalidMillisecondsDelay,
        Task_Dispose_NotCompleted,
        Task_ThrowIfDisposed,
        Task_WaitMulti_NullTask,
        ArgumentException_OtherNotArrayOfCorrectLength,
        ArgumentNull_Array,
        ArgumentNull_SafeHandle,
        ArgumentOutOfRange_EndIndexStartIndex,
        ArgumentOutOfRange_Enum,
        ArgumentOutOfRange_HugeArrayNotSupported,
        Argument_AddingDuplicate,
        Argument_InvalidArgumentForComparison,
        Arg_LowerBoundsMustMatch,
        Arg_MustBeType,
        Arg_Need1DArray,
        Arg_Need2DArray,
        Arg_Need3DArray,
        Arg_NeedAtLeast1Rank,
        Arg_RankIndices,
        Arg_RanksAndBounds,
        InvalidOperation_IComparerFailed,
        NotSupported_FixedSizeCollection,
        Rank_MultiDimNotSupported,
        Arg_TypeNotSupported,
        Argument_SpansMustHaveSameLength,
        Argument_InvalidFlag,
        CancellationTokenSource_Disposed,
        Argument_AlignmentMustBePow2,
        InvalidOperation_SpanOverlappedOperation,
        InvalidOperation_TimeProviderNullLocalTimeZone,
        InvalidOperation_TimeProviderInvalidTimestampFrequency,
        Format_UnexpectedClosingBrace,
        Format_UnclosedFormatItem,
        Format_ExpectedAsciiDigit,
        Argument_HasToBeArrayClass,
        InvalidOperation_IncompatibleComparer,
        ConcurrentDictionary_ItemKeyIsNull,
        ConcurrentDictionary_TypeOfValueIncorrect,
        InvalidOperation_NoElements,
    }

    static partial class SR {

        private static ResourceManager s_resourceManager = new(Type.GetType("System.SR, System.")!);
        private static ResourceManager ResourceManager {
            get => s_resourceManager;
        }
    }

    static partial class SR {
        private static readonly bool s_usingResourceKeys = GetUsingResourceKeysSwitchValue();

        // This method is a target of ILLink substitution.
        private static bool GetUsingResourceKeysSwitchValue() => AppContext.TryGetSwitch("System.Resources.UseSystemResourceKeys", out bool usingResourceKeys) ? usingResourceKeys : false;

        // This method is used to decide if we need to append the exception message parameters to the message when calling SR.Format.
        // by default it returns the value of System.Resources.UseSystemResourceKeys AppContext switch or false if not specified.
        // Native code generators can replace the value this returns based on user input at the time of native code generation.
        // The trimming tools are also capable of replacing the value of this method when the application is being trimmed.
        internal static bool UsingResourceKeys() => s_usingResourceKeys;


        // We can optimize out the resource string blob if we can see all accesses to it happening
        // through the generated SR.XXX properties.
        // If a call to GetResourceString is left, the optimization gets defeated and we need to keep
        // the whole resource blob. It's important to keep this private. CoreCLR's CoreLib gets a free
        // pass because the VM needs to be able to call into this, but that's a known set of constants.
#if CORECLR || LEGACY_GETRESOURCESTRING_USER
        internal
#else
        private
#endif
        static string GetResourceString(string resourceKey) {
            if (UsingResourceKeys()) {
                return resourceKey;
            }

            string? resourceString = null;
            try {
                resourceString =
#if true || SYSTEM_PRIVATE_CORELIB || NATIVEAOT
                    InternalGetResourceString(resourceKey);
#else
                    ResourceManager.GetString(resourceKey);
#endif
            } catch (MissingManifestResourceException) { }

            return resourceString!; // only null if missing resources
        }

#if LEGACY_GETRESOURCESTRING_USER
        internal
#else
        private
#endif
        static string GetResourceString(string resourceKey, string defaultString) {
            string resourceString = GetResourceString(resourceKey);

            return resourceKey == resourceString || resourceString == null ? defaultString : resourceString;
        }

        internal static string Format(string resourceFormat, object? p1) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(resourceFormat, p1);
        }

        internal static string Format(string resourceFormat, object? p1, object? p2) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(resourceFormat, p1, p2);
        }

        internal static string Format(string resourceFormat, object? p1, object? p2, object? p3) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1, p2, p3);
            }

            return string.Format(resourceFormat, p1, p2, p3);
        }

        internal static string Format(string resourceFormat, params object?[]? args) {
            if (args != null) {
                if (UsingResourceKeys()) {
                    return resourceFormat + ", " + string.Join(", ", args);
                }

                return string.Format(resourceFormat, args);
            }

            return resourceFormat;
        }

        internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1);
            }

            return string.Format(provider, resourceFormat, p1);
        }

        internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1, p2);
            }

            return string.Format(provider, resourceFormat, p1, p2);
        }

        internal static string Format(IFormatProvider? provider, string resourceFormat, object? p1, object? p2, object? p3) {
            if (UsingResourceKeys()) {
                return string.Join(", ", resourceFormat, p1, p2, p3);
            }

            return string.Format(provider, resourceFormat, p1, p2, p3);
        }

        internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args) {
            if (args != null) {
                if (UsingResourceKeys()) {
                    return resourceFormat + ", " + string.Join(", ", args);
                }

                return string.Format(provider, resourceFormat, args);
            }

            return resourceFormat;
        }
    }

    static partial class SR {
        private static readonly object _lock = new object();
        private static List<string>? _currentlyLoading;
        private static int _infinitelyRecursingCount;
#if SYSTEM_PRIVATE_CORELIB
        private static bool _resourceManagerInited;
#endif

        private static string InternalGetResourceString(string key) {
            if (key.Length == 0) {
                Debug.Fail("SR::GetResourceString with empty resourceKey.  Bug in caller, or weird recursive loading problem?");
                return key;
            }

            // We have a somewhat common potential for infinite
            // loops with mscorlib's ResourceManager.  If "potentially dangerous"
            // code throws an exception, we will get into an infinite loop
            // inside the ResourceManager and this "potentially dangerous" code.
            // Potentially dangerous code includes the IO package, CultureInfo,
            // parts of the loader, some parts of Reflection, Security (including
            // custom user-written permissions that may parse an XML file at
            // class load time), assembly load event handlers, etc.  Essentially,
            // this is not a bounded set of code, and we need to fix the problem.
            // Fortunately, this is limited to mscorlib's error lookups and is NOT
            // a general problem for all user code using the ResourceManager.

            // The solution is to make sure only one thread at a time can call
            // GetResourceString.  Also, since resource lookups can be
            // reentrant, if the same thread comes into GetResourceString
            // twice looking for the exact same resource name before
            // returning, we're going into an infinite loop and we should
            // return a bogus string.

            bool lockTaken = false;
            try {
                Monitor.Enter(_lock, ref lockTaken);

                // Are we recursively looking up the same resource?  Note - our backout code will set
                // the ResourceHelper's currentlyLoading stack to null if an exception occurs.
                if (_currentlyLoading != null && _currentlyLoading.Count > 0 && _currentlyLoading.LastIndexOf(key) >= 0) {
                    // We can start infinitely recursing for one resource lookup,
                    // then during our failure reporting, start infinitely recursing again.
                    // avoid that.
                    if (_infinitelyRecursingCount > 0) {
                        return key;
                    }
                    _infinitelyRecursingCount++;

#if SYSTEM_PRIVATE_CORELIB
                    // Note: our infrastructure for reporting this exception will again cause resource lookup.
                    // This is the most direct way of dealing with that problem.
                    string message = $@"Encountered infinite recursion while looking up resource '{key}' in {CoreLib.Name}. Verify the installation of .NET is complete and does not need repairing, and that the state of the process has not become corrupted.";
                    Environment.FailFast(message);
#endif
                }

                _currentlyLoading ??= new List<string>();

#if SYSTEM_PRIVATE_CORELIB
                // Call class constructors preemptively, so that we cannot get into an infinite
                // loop constructing a TypeInitializationException.  If this were omitted,
                // we could get the Infinite recursion assert above by failing type initialization
                // between the Push and Pop calls below.
                if (!_resourceManagerInited)
                {
                    RuntimeHelpers.RunClassConstructor(typeof(ResourceManager).TypeHandle);
                    RuntimeHelpers.RunClassConstructor(typeof(ResourceReader).TypeHandle);
                    RuntimeHelpers.RunClassConstructor(typeof(RuntimeResourceSet).TypeHandle);
                    RuntimeHelpers.RunClassConstructor(typeof(BinaryReader).TypeHandle);
                    _resourceManagerInited = true;
                }
#endif

                _currentlyLoading.Add(key); // Push

                string? s = ResourceManager.GetString(key, null);
                _currentlyLoading.RemoveAt(_currentlyLoading.Count - 1); // Pop

                Debug.Assert(s != null, $"Looking up resource '{key}' failed. Was your resource name misspelled? Did you rebuild after adding a resource?");
                return s ?? key;
            } catch {
                if (lockTaken) {
                    // Backout code - throw away potentially corrupt state
                    s_resourceManager = null;
                    _currentlyLoading = null;
                }
                throw;
            } finally {
                if (lockTaken) {
                    Monitor.Exit(_lock);
                }
            }
        }
    }

    static partial class SR {


        // Token: 0x17000365 RID: 869
        // (get) Token: 0x0600214F RID: 8527 RVA: 0x009E962C File Offset: 0x009E962C
        internal static string Acc_CreateAbstEx {
            get {
                return SR.GetResourceString("Acc_CreateAbstEx");
            }
        }

        // Token: 0x17000366 RID: 870
        // (get) Token: 0x06002150 RID: 8528 RVA: 0x009E9638 File Offset: 0x009E9638
        internal static string Acc_CreateArgIterator {
            get {
                return SR.GetResourceString("Acc_CreateArgIterator");
            }
        }

        // Token: 0x17000367 RID: 871
        // (get) Token: 0x06002151 RID: 8529 RVA: 0x009E9644 File Offset: 0x009E9644
        internal static string Acc_CreateGenericEx {
            get {
                return SR.GetResourceString("Acc_CreateGenericEx");
            }
        }

        // Token: 0x17000368 RID: 872
        // (get) Token: 0x06002152 RID: 8530 RVA: 0x009E9650 File Offset: 0x009E9650
        internal static string Acc_CreateInterfaceEx {
            get {
                return SR.GetResourceString("Acc_CreateInterfaceEx");
            }
        }

        // Token: 0x17000369 RID: 873
        // (get) Token: 0x06002153 RID: 8531 RVA: 0x009E965C File Offset: 0x009E965C
        internal static string Acc_CreateVoid {
            get {
                return SR.GetResourceString("Acc_CreateVoid");
            }
        }

        // Token: 0x1700036A RID: 874
        // (get) Token: 0x06002154 RID: 8532 RVA: 0x009E9668 File Offset: 0x009E9668
        internal static string Acc_NotClassInit {
            get {
                return SR.GetResourceString("Acc_NotClassInit");
            }
        }

        // Token: 0x1700036B RID: 875
        // (get) Token: 0x06002155 RID: 8533 RVA: 0x009E9674 File Offset: 0x009E9674
        internal static string Acc_ReadOnly {
            get {
                return SR.GetResourceString("Acc_ReadOnly");
            }
        }

        // Token: 0x1700036C RID: 876
        // (get) Token: 0x06002156 RID: 8534 RVA: 0x009E9680 File Offset: 0x009E9680
        internal static string Access_Void {
            get {
                return SR.GetResourceString("Access_Void");
            }
        }

        // Token: 0x1700036D RID: 877
        // (get) Token: 0x06002157 RID: 8535 RVA: 0x009E968C File Offset: 0x009E968C
        internal static string AggregateException_ctor_DefaultMessage {
            get {
                return SR.GetResourceString("AggregateException_ctor_DefaultMessage");
            }
        }

        // Token: 0x1700036E RID: 878
        // (get) Token: 0x06002158 RID: 8536 RVA: 0x009E9698 File Offset: 0x009E9698
        internal static string AggregateException_ctor_InnerExceptionNull {
            get {
                return SR.GetResourceString("AggregateException_ctor_InnerExceptionNull");
            }
        }

        // Token: 0x1700036F RID: 879
        // (get) Token: 0x06002159 RID: 8537 RVA: 0x009E96A4 File Offset: 0x009E96A4
        internal static string AggregateException_DeserializationFailure {
            get {
                return SR.GetResourceString("AggregateException_DeserializationFailure");
            }
        }

        // Token: 0x17000370 RID: 880
        // (get) Token: 0x0600215A RID: 8538 RVA: 0x009E96B0 File Offset: 0x009E96B0
        internal static string AggregateException_InnerException {
            get {
                return SR.GetResourceString("AggregateException_InnerException");
            }
        }

        // Token: 0x17000371 RID: 881
        // (get) Token: 0x0600215B RID: 8539 RVA: 0x009E96BC File Offset: 0x009E96BC
        internal static string AppDomain_Name {
            get {
                return SR.GetResourceString("AppDomain_Name");
            }
        }

        // Token: 0x17000372 RID: 882
        // (get) Token: 0x0600215C RID: 8540 RVA: 0x009E96C8 File Offset: 0x009E96C8
        internal static string AppDomain_NoContextPolicies {
            get {
                return SR.GetResourceString("AppDomain_NoContextPolicies");
            }
        }

        // Token: 0x17000373 RID: 883
        // (get) Token: 0x0600215D RID: 8541 RVA: 0x009E96D4 File Offset: 0x009E96D4
        internal static string AppDomain_Policy_PrincipalTwice {
            get {
                return SR.GetResourceString("AppDomain_Policy_PrincipalTwice");
            }
        }

        // Token: 0x17000374 RID: 884
        // (get) Token: 0x0600215E RID: 8542 RVA: 0x009E96E0 File Offset: 0x009E96E0
        internal static string Arg_AccessException {
            get {
                return SR.GetResourceString("Arg_AccessException");
            }
        }

        // Token: 0x17000375 RID: 885
        // (get) Token: 0x0600215F RID: 8543 RVA: 0x009E96EC File Offset: 0x009E96EC
        internal static string Arg_AccessViolationException {
            get {
                return SR.GetResourceString("Arg_AccessViolationException");
            }
        }

        // Token: 0x17000376 RID: 886
        // (get) Token: 0x06002160 RID: 8544 RVA: 0x009E96F8 File Offset: 0x009E96F8
        internal static string Arg_AmbiguousImplementationException_NoMessage {
            get {
                return SR.GetResourceString("Arg_AmbiguousImplementationException_NoMessage");
            }
        }

        // Token: 0x17000377 RID: 887
        // (get) Token: 0x06002161 RID: 8545 RVA: 0x009E9704 File Offset: 0x009E9704
        internal static string Arg_AmbiguousMatchException_Attribute {
            get {
                return SR.GetResourceString("Arg_AmbiguousMatchException_Attribute");
            }
        }

        // Token: 0x17000378 RID: 888
        // (get) Token: 0x06002162 RID: 8546 RVA: 0x009E9710 File Offset: 0x009E9710
        internal static string Arg_AmbiguousMatchException_NoMessage {
            get {
                return SR.GetResourceString("Arg_AmbiguousMatchException_NoMessage");
            }
        }

        // Token: 0x17000379 RID: 889
        // (get) Token: 0x06002163 RID: 8547 RVA: 0x009E971C File Offset: 0x009E971C
        internal static string Arg_AmbiguousMatchException_MemberInfo {
            get {
                return SR.GetResourceString("Arg_AmbiguousMatchException_MemberInfo");
            }
        }

        // Token: 0x1700037A RID: 890
        // (get) Token: 0x06002164 RID: 8548 RVA: 0x009E9728 File Offset: 0x009E9728
        internal static string Arg_ApplicationException {
            get {
                return SR.GetResourceString("Arg_ApplicationException");
            }
        }

        // Token: 0x1700037B RID: 891
        // (get) Token: 0x06002165 RID: 8549 RVA: 0x009E9734 File Offset: 0x009E9734
        internal static string Arg_ArgumentException {
            get {
                return SR.GetResourceString("Arg_ArgumentException");
            }
        }

        // Token: 0x1700037C RID: 892
        // (get) Token: 0x06002166 RID: 8550 RVA: 0x009E9740 File Offset: 0x009E9740
        internal static string Arg_ArgumentOutOfRangeException {
            get {
                return SR.GetResourceString("Arg_ArgumentOutOfRangeException");
            }
        }

        // Token: 0x1700037D RID: 893
        // (get) Token: 0x06002167 RID: 8551 RVA: 0x009E974C File Offset: 0x009E974C
        internal static string Arg_ArithmeticException {
            get {
                return SR.GetResourceString("Arg_ArithmeticException");
            }
        }

        // Token: 0x1700037E RID: 894
        // (get) Token: 0x06002168 RID: 8552 RVA: 0x009E9758 File Offset: 0x009E9758
        internal static string Arg_ArrayLengthsDiffer {
            get {
                return SR.GetResourceString("Arg_ArrayLengthsDiffer");
            }
        }

        // Token: 0x1700037F RID: 895
        // (get) Token: 0x06002169 RID: 8553 RVA: 0x009E9764 File Offset: 0x009E9764
        internal static string Arg_ArrayPlusOffTooSmall {
            get {
                return SR.GetResourceString("Arg_ArrayPlusOffTooSmall");
            }
        }

        // Token: 0x17000380 RID: 896
        // (get) Token: 0x0600216A RID: 8554 RVA: 0x009E9770 File Offset: 0x009E9770
        internal static string Arg_BitArrayTypeUnsupported {
            get {
                return SR.GetResourceString("Arg_BitArrayTypeUnsupported");
            }
        }

        // Token: 0x17000381 RID: 897
        // (get) Token: 0x0600216B RID: 8555 RVA: 0x009E977C File Offset: 0x009E977C
        internal static string Arg_ByteArrayTooSmallForValue {
            get {
                return SR.GetResourceString("Arg_ByteArrayTooSmallForValue");
            }
        }

        // Token: 0x17000382 RID: 898
        // (get) Token: 0x0600216C RID: 8556 RVA: 0x009E9788 File Offset: 0x009E9788
        internal static string Arg_ArrayTypeMismatchException {
            get {
                return SR.GetResourceString("Arg_ArrayTypeMismatchException");
            }
        }

        // Token: 0x17000383 RID: 899
        // (get) Token: 0x0600216D RID: 8557 RVA: 0x009E9794 File Offset: 0x009E9794
        internal static string Arg_ArrayZeroError {
            get {
                return SR.GetResourceString("Arg_ArrayZeroError");
            }
        }

        // Token: 0x17000384 RID: 900
        // (get) Token: 0x0600216E RID: 8558 RVA: 0x009E97A0 File Offset: 0x009E97A0
        internal static string Arg_BadDecimal {
            get {
                return SR.GetResourceString("Arg_BadDecimal");
            }
        }

        // Token: 0x17000385 RID: 901
        // (get) Token: 0x0600216F RID: 8559 RVA: 0x009E97AC File Offset: 0x009E97AC
        internal static string Arg_BadImageFormatException {
            get {
                return SR.GetResourceString("Arg_BadImageFormatException");
            }
        }

        // Token: 0x17000386 RID: 902
        // (get) Token: 0x06002170 RID: 8560 RVA: 0x009E97B8 File Offset: 0x009E97B8
        internal static string Arg_BadLiteralFormat {
            get {
                return SR.GetResourceString("Arg_BadLiteralFormat");
            }
        }

        // Token: 0x17000387 RID: 903
        // (get) Token: 0x06002171 RID: 8561 RVA: 0x009E97C4 File Offset: 0x009E97C4
        internal static string Arg_BogusIComparer {
            get {
                return SR.GetResourceString("Arg_BogusIComparer");
            }
        }

        // Token: 0x17000388 RID: 904
        // (get) Token: 0x06002172 RID: 8562 RVA: 0x009E97D0 File Offset: 0x009E97D0
        internal static string InvalidOperation_IncompatibleComparer {
            get {
                return SR.GetResourceString("InvalidOperation_IncompatibleComparer");
            }
        }

        // Token: 0x17000389 RID: 905
        // (get) Token: 0x06002173 RID: 8563 RVA: 0x009E97DC File Offset: 0x009E97DC
        internal static string InvalidOperation_TypeMapMissingEntryAssembly {
            get {
                return SR.GetResourceString("InvalidOperation_TypeMapMissingEntryAssembly");
            }
        }

        // Token: 0x1700038A RID: 906
        // (get) Token: 0x06002174 RID: 8564 RVA: 0x009E97E8 File Offset: 0x009E97E8
        internal static string Arg_BufferTooSmall {
            get {
                return SR.GetResourceString("Arg_BufferTooSmall");
            }
        }

        // Token: 0x1700038B RID: 907
        // (get) Token: 0x06002175 RID: 8565 RVA: 0x009E97F4 File Offset: 0x009E97F4
        internal static string Arg_CannotBeNaN {
            get {
                return SR.GetResourceString("Arg_CannotBeNaN");
            }
        }

        // Token: 0x1700038C RID: 908
        // (get) Token: 0x06002176 RID: 8566 RVA: 0x009E9800 File Offset: 0x009E9800
        internal static string Arg_CannotHaveNegativeValue {
            get {
                return SR.GetResourceString("Arg_CannotHaveNegativeValue");
            }
        }

        // Token: 0x1700038D RID: 909
        // (get) Token: 0x06002177 RID: 8567 RVA: 0x009E980C File Offset: 0x009E980C
        internal static string Arg_CannotMixComparisonInfrastructure {
            get {
                return SR.GetResourceString("Arg_CannotMixComparisonInfrastructure");
            }
        }

        // Token: 0x1700038E RID: 910
        // (get) Token: 0x06002178 RID: 8568 RVA: 0x009E9818 File Offset: 0x009E9818
        internal static string Arg_CannotUnloadAppDomainException {
            get {
                return SR.GetResourceString("Arg_CannotUnloadAppDomainException");
            }
        }

        // Token: 0x1700038F RID: 911
        // (get) Token: 0x06002179 RID: 8569 RVA: 0x009E9824 File Offset: 0x009E9824
        internal static string Arg_COMAccess {
            get {
                return SR.GetResourceString("Arg_COMAccess");
            }
        }

        // Token: 0x17000390 RID: 912
        // (get) Token: 0x0600217A RID: 8570 RVA: 0x009E9830 File Offset: 0x009E9830
        internal static string Arg_COMException {
            get {
                return SR.GetResourceString("Arg_COMException");
            }
        }

        // Token: 0x17000391 RID: 913
        // (get) Token: 0x0600217B RID: 8571 RVA: 0x009E983C File Offset: 0x009E983C
        internal static string Arg_COMPropSetPut {
            get {
                return SR.GetResourceString("Arg_COMPropSetPut");
            }
        }

        // Token: 0x17000392 RID: 914
        // (get) Token: 0x0600217C RID: 8572 RVA: 0x009E9848 File Offset: 0x009E9848
        internal static string Arg_CreatInstAccess {
            get {
                return SR.GetResourceString("Arg_CreatInstAccess");
            }
        }

        // Token: 0x17000393 RID: 915
        // (get) Token: 0x0600217D RID: 8573 RVA: 0x009E9854 File Offset: 0x009E9854
        internal static string Arg_CryptographyException {
            get {
                return SR.GetResourceString("Arg_CryptographyException");
            }
        }

        // Token: 0x17000394 RID: 916
        // (get) Token: 0x0600217E RID: 8574 RVA: 0x009E9860 File Offset: 0x009E9860
        internal static string Arg_CustomAttributeFormatException {
            get {
                return SR.GetResourceString("Arg_CustomAttributeFormatException");
            }
        }

        // Token: 0x17000395 RID: 917
        // (get) Token: 0x0600217F RID: 8575 RVA: 0x009E986C File Offset: 0x009E986C
        internal static string Arg_CustomAttributeUnknownNamedArgument {
            get {
                return SR.GetResourceString("Arg_CustomAttributeUnknownNamedArgument");
            }
        }

        // Token: 0x17000396 RID: 918
        // (get) Token: 0x06002180 RID: 8576 RVA: 0x009E9878 File Offset: 0x009E9878
        internal static string Arg_CustomAttributeDuplicateNamedArgument {
            get {
                return SR.GetResourceString("Arg_CustomAttributeDuplicateNamedArgument");
            }
        }

        // Token: 0x17000397 RID: 919
        // (get) Token: 0x06002181 RID: 8577 RVA: 0x009E9884 File Offset: 0x009E9884
        internal static string Arg_DataMisalignedException {
            get {
                return SR.GetResourceString("Arg_DataMisalignedException");
            }
        }

        // Token: 0x17000398 RID: 920
        // (get) Token: 0x06002182 RID: 8578 RVA: 0x009E9890 File Offset: 0x009E9890
        internal static string Arg_DecBitCtor {
            get {
                return SR.GetResourceString("Arg_DecBitCtor");
            }
        }

        // Token: 0x17000399 RID: 921
        // (get) Token: 0x06002183 RID: 8579 RVA: 0x009E989C File Offset: 0x009E989C
        internal static string Arg_DirectoryNotFoundException {
            get {
                return SR.GetResourceString("Arg_DirectoryNotFoundException");
            }
        }

        // Token: 0x1700039A RID: 922
        // (get) Token: 0x06002184 RID: 8580 RVA: 0x009E98A8 File Offset: 0x009E98A8
        internal static string Arg_DivideByZero {
            get {
                return SR.GetResourceString("Arg_DivideByZero");
            }
        }

        // Token: 0x1700039B RID: 923
        // (get) Token: 0x06002185 RID: 8581 RVA: 0x009E98B4 File Offset: 0x009E98B4
        internal static string Arg_DlgtNullInst {
            get {
                return SR.GetResourceString("Arg_DlgtNullInst");
            }
        }

        // Token: 0x1700039C RID: 924
        // (get) Token: 0x06002186 RID: 8582 RVA: 0x009E98C0 File Offset: 0x009E98C0
        internal static string Arg_DlgtTargMeth {
            get {
                return SR.GetResourceString("Arg_DlgtTargMeth");
            }
        }

        // Token: 0x1700039D RID: 925
        // (get) Token: 0x06002187 RID: 8583 RVA: 0x009E98CC File Offset: 0x009E98CC
        internal static string Arg_DlgtTypeMis {
            get {
                return SR.GetResourceString("Arg_DlgtTypeMis");
            }
        }

        // Token: 0x1700039E RID: 926
        // (get) Token: 0x06002188 RID: 8584 RVA: 0x009E98D8 File Offset: 0x009E98D8
        internal static string Arg_DllNotFoundException {
            get {
                return SR.GetResourceString("Arg_DllNotFoundException");
            }
        }

        // Token: 0x1700039F RID: 927
        // (get) Token: 0x06002189 RID: 8585 RVA: 0x009E98E4 File Offset: 0x009E98E4
        internal static string Arg_DuplicateWaitObjectException {
            get {
                return SR.GetResourceString("Arg_DuplicateWaitObjectException");
            }
        }

        // Token: 0x170003A0 RID: 928
        // (get) Token: 0x0600218A RID: 8586 RVA: 0x009E98F0 File Offset: 0x009E98F0
        internal static string Arg_EHClauseNotClause {
            get {
                return SR.GetResourceString("Arg_EHClauseNotClause");
            }
        }

        // Token: 0x170003A1 RID: 929
        // (get) Token: 0x0600218B RID: 8587 RVA: 0x009E98FC File Offset: 0x009E98FC
        internal static string Arg_EHClauseNotFilter {
            get {
                return SR.GetResourceString("Arg_EHClauseNotFilter");
            }
        }

        // Token: 0x170003A2 RID: 930
        // (get) Token: 0x0600218C RID: 8588 RVA: 0x009E9908 File Offset: 0x009E9908
        internal static string Arg_EmptyArray {
            get {
                return SR.GetResourceString("Arg_EmptyArray");
            }
        }

        // Token: 0x170003A3 RID: 931
        // (get) Token: 0x0600218D RID: 8589 RVA: 0x009E9914 File Offset: 0x009E9914
        internal static string Arg_EmptySpan {
            get {
                return SR.GetResourceString("Arg_EmptySpan");
            }
        }

        // Token: 0x170003A4 RID: 932
        // (get) Token: 0x0600218E RID: 8590 RVA: 0x009E9920 File Offset: 0x009E9920
        internal static string Arg_EndOfStreamException {
            get {
                return SR.GetResourceString("Arg_EndOfStreamException");
            }
        }

        // Token: 0x170003A5 RID: 933
        // (get) Token: 0x0600218F RID: 8591 RVA: 0x009E992C File Offset: 0x009E992C
        internal static string Arg_EntryPointNotFoundException {
            get {
                return SR.GetResourceString("Arg_EntryPointNotFoundException");
            }
        }

        // Token: 0x170003A6 RID: 934
        // (get) Token: 0x06002190 RID: 8592 RVA: 0x009E9938 File Offset: 0x009E9938
        internal static string Arg_EnumAndObjectMustBeSameType {
            get {
                return SR.GetResourceString("Arg_EnumAndObjectMustBeSameType");
            }
        }

        // Token: 0x170003A7 RID: 935
        // (get) Token: 0x06002191 RID: 8593 RVA: 0x009E9944 File Offset: 0x009E9944
        internal static string Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType {
            get {
                return SR.GetResourceString("Arg_EnumFormatUnderlyingTypeAndObjectMustBeSameType");
            }
        }

        // Token: 0x170003A8 RID: 936
        // (get) Token: 0x06002192 RID: 8594 RVA: 0x009E9950 File Offset: 0x009E9950
        internal static string Arg_EnumIllegalVal {
            get {
                return SR.GetResourceString("Arg_EnumIllegalVal");
            }
        }

        // Token: 0x170003A9 RID: 937
        // (get) Token: 0x06002193 RID: 8595 RVA: 0x009E995C File Offset: 0x009E995C
        internal static string Arg_EnumLitValueNotFound {
            get {
                return SR.GetResourceString("Arg_EnumLitValueNotFound");
            }
        }

        // Token: 0x170003AA RID: 938
        // (get) Token: 0x06002194 RID: 8596 RVA: 0x009E9968 File Offset: 0x009E9968
        internal static string Arg_EnumUnderlyingTypeAndObjectMustBeSameType {
            get {
                return SR.GetResourceString("Arg_EnumUnderlyingTypeAndObjectMustBeSameType");
            }
        }

        // Token: 0x170003AB RID: 939
        // (get) Token: 0x06002195 RID: 8597 RVA: 0x009E9974 File Offset: 0x009E9974
        internal static string Arg_EnumValueNotFound {
            get {
                return SR.GetResourceString("Arg_EnumValueNotFound");
            }
        }

        // Token: 0x170003AC RID: 940
        // (get) Token: 0x06002196 RID: 8598 RVA: 0x009E9980 File Offset: 0x009E9980
        internal static string Arg_ExecutionEngineException {
            get {
                return SR.GetResourceString("Arg_ExecutionEngineException");
            }
        }

        // Token: 0x170003AD RID: 941
        // (get) Token: 0x06002197 RID: 8599 RVA: 0x009E998C File Offset: 0x009E998C
        internal static string Arg_ExternalException {
            get {
                return SR.GetResourceString("Arg_ExternalException");
            }
        }

        // Token: 0x170003AE RID: 942
        // (get) Token: 0x06002198 RID: 8600 RVA: 0x009E9998 File Offset: 0x009E9998
        internal static string Arg_FieldAccessException {
            get {
                return SR.GetResourceString("Arg_FieldAccessException");
            }
        }

        // Token: 0x170003AF RID: 943
        // (get) Token: 0x06002199 RID: 8601 RVA: 0x009E99A4 File Offset: 0x009E99A4
        internal static string Arg_FieldDeclTarget {
            get {
                return SR.GetResourceString("Arg_FieldDeclTarget");
            }
        }

        // Token: 0x170003B0 RID: 944
        // (get) Token: 0x0600219A RID: 8602 RVA: 0x009E99B0 File Offset: 0x009E99B0
        internal static string Arg_FldGetArgErr {
            get {
                return SR.GetResourceString("Arg_FldGetArgErr");
            }
        }

        // Token: 0x170003B1 RID: 945
        // (get) Token: 0x0600219B RID: 8603 RVA: 0x009E99BC File Offset: 0x009E99BC
        internal static string Arg_FldGetPropSet {
            get {
                return SR.GetResourceString("Arg_FldGetPropSet");
            }
        }

        // Token: 0x170003B2 RID: 946
        // (get) Token: 0x0600219C RID: 8604 RVA: 0x009E99C8 File Offset: 0x009E99C8
        internal static string Arg_FldSetArgErr {
            get {
                return SR.GetResourceString("Arg_FldSetArgErr");
            }
        }

        // Token: 0x170003B3 RID: 947
        // (get) Token: 0x0600219D RID: 8605 RVA: 0x009E99D4 File Offset: 0x009E99D4
        internal static string Arg_FldSetGet {
            get {
                return SR.GetResourceString("Arg_FldSetGet");
            }
        }

        // Token: 0x170003B4 RID: 948
        // (get) Token: 0x0600219E RID: 8606 RVA: 0x009E99E0 File Offset: 0x009E99E0
        internal static string Arg_FldSetInvoke {
            get {
                return SR.GetResourceString("Arg_FldSetInvoke");
            }
        }

        // Token: 0x170003B5 RID: 949
        // (get) Token: 0x0600219F RID: 8607 RVA: 0x009E99EC File Offset: 0x009E99EC
        internal static string Arg_FldSetPropGet {
            get {
                return SR.GetResourceString("Arg_FldSetPropGet");
            }
        }

        // Token: 0x170003B6 RID: 950
        // (get) Token: 0x060021A0 RID: 8608 RVA: 0x009E99F8 File Offset: 0x009E99F8
        internal static string Arg_FormatException {
            get {
                return SR.GetResourceString("Arg_FormatException");
            }
        }

        // Token: 0x170003B7 RID: 951
        // (get) Token: 0x060021A1 RID: 8609 RVA: 0x009E9A04 File Offset: 0x009E9A04
        internal static string Arg_GenericParameter {
            get {
                return SR.GetResourceString("Arg_GenericParameter");
            }
        }

        // Token: 0x170003B8 RID: 952
        // (get) Token: 0x060021A2 RID: 8610 RVA: 0x009E9A10 File Offset: 0x009E9A10
        internal static string Arg_GetMethNotFnd {
            get {
                return SR.GetResourceString("Arg_GetMethNotFnd");
            }
        }

        // Token: 0x170003B9 RID: 953
        // (get) Token: 0x060021A3 RID: 8611 RVA: 0x009E9A1C File Offset: 0x009E9A1C
        internal static string Arg_GuidArrayCtor {
            get {
                return SR.GetResourceString("Arg_GuidArrayCtor");
            }
        }

        // Token: 0x170003BA RID: 954
        // (get) Token: 0x060021A4 RID: 8612 RVA: 0x009E9A28 File Offset: 0x009E9A28
        internal static string Arg_BinaryStyleNotSupported {
            get {
                return SR.GetResourceString("Arg_BinaryStyleNotSupported");
            }
        }

        // Token: 0x170003BB RID: 955
        // (get) Token: 0x060021A5 RID: 8613 RVA: 0x009E9A34 File Offset: 0x009E9A34
        internal static string Arg_InvalidHexFloatStyle {
            get {
                return SR.GetResourceString("Arg_InvalidHexFloatStyle");
            }
        }

        // Token: 0x170003BC RID: 956
        // (get) Token: 0x060021A6 RID: 8614 RVA: 0x009E9A40 File Offset: 0x009E9A40
        internal static string Arg_HTCapacityOverflow {
            get {
                return SR.GetResourceString("Arg_HTCapacityOverflow");
            }
        }

        // Token: 0x170003BD RID: 957
        // (get) Token: 0x060021A7 RID: 8615 RVA: 0x009E9A4C File Offset: 0x009E9A4C
        internal static string Arg_IndexMustBeInt {
            get {
                return SR.GetResourceString("Arg_IndexMustBeInt");
            }
        }

        // Token: 0x170003BE RID: 958
        // (get) Token: 0x060021A8 RID: 8616 RVA: 0x009E9A58 File Offset: 0x009E9A58
        internal static string Arg_IndexOutOfRangeException {
            get {
                return SR.GetResourceString("Arg_IndexOutOfRangeException");
            }
        }

        // Token: 0x170003BF RID: 959
        // (get) Token: 0x060021A9 RID: 8617 RVA: 0x009E9A64 File Offset: 0x009E9A64
        internal static string Arg_InsufficientExecutionStackException {
            get {
                return SR.GetResourceString("Arg_InsufficientExecutionStackException");
            }
        }

        // Token: 0x170003C0 RID: 960
        // (get) Token: 0x060021AA RID: 8618 RVA: 0x009E9A70 File Offset: 0x009E9A70
        internal static string Arg_InvalidANSIString {
            get {
                return SR.GetResourceString("Arg_InvalidANSIString");
            }
        }

        // Token: 0x170003C1 RID: 961
        // (get) Token: 0x060021AB RID: 8619 RVA: 0x009E9A7C File Offset: 0x009E9A7C
        internal static string Arg_InvalidBase {
            get {
                return SR.GetResourceString("Arg_InvalidBase");
            }
        }

        // Token: 0x170003C2 RID: 962
        // (get) Token: 0x060021AC RID: 8620 RVA: 0x009E9A88 File Offset: 0x009E9A88
        internal static string Arg_InvalidCastException {
            get {
                return SR.GetResourceString("Arg_InvalidCastException");
            }
        }

        // Token: 0x170003C3 RID: 963
        // (get) Token: 0x060021AD RID: 8621 RVA: 0x009E9A94 File Offset: 0x009E9A94
        internal static string Arg_InvalidComObjectException {
            get {
                return SR.GetResourceString("Arg_InvalidComObjectException");
            }
        }

        // Token: 0x170003C4 RID: 964
        // (get) Token: 0x060021AE RID: 8622 RVA: 0x009E9AA0 File Offset: 0x009E9AA0
        internal static string Arg_InvalidFilterCriteriaException {
            get {
                return SR.GetResourceString("Arg_InvalidFilterCriteriaException");
            }
        }

        // Token: 0x170003C5 RID: 965
        // (get) Token: 0x060021AF RID: 8623 RVA: 0x009E9AAC File Offset: 0x009E9AAC
        internal static string Arg_InvalidHandle {
            get {
                return SR.GetResourceString("Arg_InvalidHandle");
            }
        }

        // Token: 0x170003C6 RID: 966
        // (get) Token: 0x060021B0 RID: 8624 RVA: 0x009E9AB8 File Offset: 0x009E9AB8
        internal static string Arg_InvalidHexBinaryStyle {
            get {
                return SR.GetResourceString("Arg_InvalidHexBinaryStyle");
            }
        }

        // Token: 0x170003C7 RID: 967
        // (get) Token: 0x060021B1 RID: 8625 RVA: 0x009E9AC4 File Offset: 0x009E9AC4
        internal static string Arg_InvalidNeutralResourcesLanguage_Asm_Culture {
            get {
                return SR.GetResourceString("Arg_InvalidNeutralResourcesLanguage_Asm_Culture");
            }
        }

        // Token: 0x170003C8 RID: 968
        // (get) Token: 0x060021B2 RID: 8626 RVA: 0x009E9AD0 File Offset: 0x009E9AD0
        internal static string Arg_InvalidNeutralResourcesLanguage_FallbackLoc {
            get {
                return SR.GetResourceString("Arg_InvalidNeutralResourcesLanguage_FallbackLoc");
            }
        }

        // Token: 0x170003C9 RID: 969
        // (get) Token: 0x060021B3 RID: 8627 RVA: 0x009E9ADC File Offset: 0x009E9ADC
        internal static string Arg_InvalidSatelliteContract_Asm_Ver {
            get {
                return SR.GetResourceString("Arg_InvalidSatelliteContract_Asm_Ver");
            }
        }

        // Token: 0x170003CA RID: 970
        // (get) Token: 0x060021B4 RID: 8628 RVA: 0x009E9AE8 File Offset: 0x009E9AE8
        internal static string Arg_InvalidOleVariantTypeException {
            get {
                return SR.GetResourceString("Arg_InvalidOleVariantTypeException");
            }
        }

        // Token: 0x170003CB RID: 971
        // (get) Token: 0x060021B5 RID: 8629 RVA: 0x009E9AF4 File Offset: 0x009E9AF4
        internal static string Arg_InvalidOperationException {
            get {
                return SR.GetResourceString("Arg_InvalidOperationException");
            }
        }

        // Token: 0x170003CC RID: 972
        // (get) Token: 0x060021B6 RID: 8630 RVA: 0x009E9B00 File Offset: 0x009E9B00
        internal static string Arg_InvalidTypeInRetType {
            get {
                return SR.GetResourceString("Arg_InvalidTypeInRetType");
            }
        }

        // Token: 0x170003CD RID: 973
        // (get) Token: 0x060021B7 RID: 8631 RVA: 0x009E9B0C File Offset: 0x009E9B0C
        internal static string Arg_InvalidTypeInSignature {
            get {
                return SR.GetResourceString("Arg_InvalidTypeInSignature");
            }
        }

        // Token: 0x170003CE RID: 974
        // (get) Token: 0x060021B8 RID: 8632 RVA: 0x009E9B18 File Offset: 0x009E9B18
        internal static string Arg_IOException {
            get {
                return SR.GetResourceString("Arg_IOException");
            }
        }

        // Token: 0x170003CF RID: 975
        // (get) Token: 0x060021B9 RID: 8633 RVA: 0x009E9B24 File Offset: 0x009E9B24
        internal static string Arg_KeyNotFound {
            get {
                return SR.GetResourceString("Arg_KeyNotFound");
            }
        }

        // Token: 0x170003D0 RID: 976
        // (get) Token: 0x060021BA RID: 8634 RVA: 0x009E9B30 File Offset: 0x009E9B30
        internal static string Arg_KeyNotFoundWithKey {
            get {
                return SR.GetResourceString("Arg_KeyNotFoundWithKey");
            }
        }

        // Token: 0x170003D1 RID: 977
        // (get) Token: 0x060021BB RID: 8635 RVA: 0x009E9B3C File Offset: 0x009E9B3C
        internal static string Arg_LongerThanDestArray {
            get {
                return SR.GetResourceString("Arg_LongerThanDestArray");
            }
        }

        // Token: 0x170003D2 RID: 978
        // (get) Token: 0x060021BC RID: 8636 RVA: 0x009E9B48 File Offset: 0x009E9B48
        internal static string Arg_LongerThanSrcArray {
            get {
                return SR.GetResourceString("Arg_LongerThanSrcArray");
            }
        }

        // Token: 0x170003D3 RID: 979
        // (get) Token: 0x060021BD RID: 8637 RVA: 0x009E9B54 File Offset: 0x009E9B54
        internal static string Arg_LongerThanSrcString {
            get {
                return SR.GetResourceString("Arg_LongerThanSrcString");
            }
        }

        // Token: 0x170003D4 RID: 980
        // (get) Token: 0x060021BE RID: 8638 RVA: 0x009E9B60 File Offset: 0x009E9B60
        internal static string Arg_LowerBoundsMustMatch {
            get {
                return SR.GetResourceString("Arg_LowerBoundsMustMatch");
            }
        }

        // Token: 0x170003D5 RID: 981
        // (get) Token: 0x060021BF RID: 8639 RVA: 0x009E9B6C File Offset: 0x009E9B6C
        internal static string Arg_MarshalAsAnyRestriction {
            get {
                return SR.GetResourceString("Arg_MarshalAsAnyRestriction");
            }
        }

        // Token: 0x170003D6 RID: 982
        // (get) Token: 0x060021C0 RID: 8640 RVA: 0x009E9B78 File Offset: 0x009E9B78
        internal static string Arg_MarshalDirectiveException {
            get {
                return SR.GetResourceString("Arg_MarshalDirectiveException");
            }
        }

        // Token: 0x170003D7 RID: 983
        // (get) Token: 0x060021C1 RID: 8641 RVA: 0x009E9B84 File Offset: 0x009E9B84
        internal static string Arg_MethodAccessException {
            get {
                return SR.GetResourceString("Arg_MethodAccessException");
            }
        }

        // Token: 0x170003D8 RID: 984
        // (get) Token: 0x060021C2 RID: 8642 RVA: 0x009E9B90 File Offset: 0x009E9B90
        internal static string Arg_MissingFieldException {
            get {
                return SR.GetResourceString("Arg_MissingFieldException");
            }
        }

        // Token: 0x170003D9 RID: 985
        // (get) Token: 0x060021C3 RID: 8643 RVA: 0x009E9B9C File Offset: 0x009E9B9C
        internal static string Arg_MissingManifestResourceException {
            get {
                return SR.GetResourceString("Arg_MissingManifestResourceException");
            }
        }

        // Token: 0x170003DA RID: 986
        // (get) Token: 0x060021C4 RID: 8644 RVA: 0x009E9BA8 File Offset: 0x009E9BA8
        internal static string Arg_MissingMemberException {
            get {
                return SR.GetResourceString("Arg_MissingMemberException");
            }
        }

        // Token: 0x170003DB RID: 987
        // (get) Token: 0x060021C5 RID: 8645 RVA: 0x009E9BB4 File Offset: 0x009E9BB4
        internal static string Arg_MissingMethodException {
            get {
                return SR.GetResourceString("Arg_MissingMethodException");
            }
        }

        // Token: 0x170003DC RID: 988
        // (get) Token: 0x060021C6 RID: 8646 RVA: 0x009E9BC0 File Offset: 0x009E9BC0
        internal static string Arg_MulticastNotSupportedException {
            get {
                return SR.GetResourceString("Arg_MulticastNotSupportedException");
            }
        }

        // Token: 0x170003DD RID: 989
        // (get) Token: 0x060021C7 RID: 8647 RVA: 0x009E9BCC File Offset: 0x009E9BCC
        internal static string Arg_MustBeBoolean {
            get {
                return SR.GetResourceString("Arg_MustBeBoolean");
            }
        }

        // Token: 0x170003DE RID: 990
        // (get) Token: 0x060021C8 RID: 8648 RVA: 0x009E9BD8 File Offset: 0x009E9BD8
        internal static string Arg_MustBeByte {
            get {
                return SR.GetResourceString("Arg_MustBeByte");
            }
        }

        // Token: 0x170003DF RID: 991
        // (get) Token: 0x060021C9 RID: 8649 RVA: 0x009E9BE4 File Offset: 0x009E9BE4
        internal static string Arg_MustBeChar {
            get {
                return SR.GetResourceString("Arg_MustBeChar");
            }
        }

        // Token: 0x170003E0 RID: 992
        // (get) Token: 0x060021CA RID: 8650 RVA: 0x009E9BF0 File Offset: 0x009E9BF0
        internal static string Arg_MustBeDateOnly {
            get {
                return SR.GetResourceString("Arg_MustBeDateOnly");
            }
        }

        // Token: 0x170003E1 RID: 993
        // (get) Token: 0x060021CB RID: 8651 RVA: 0x009E9BFC File Offset: 0x009E9BFC
        internal static string Arg_MustBeTimeOnly {
            get {
                return SR.GetResourceString("Arg_MustBeTimeOnly");
            }
        }

        // Token: 0x170003E2 RID: 994
        // (get) Token: 0x060021CC RID: 8652 RVA: 0x009E9C08 File Offset: 0x009E9C08
        internal static string Arg_MustBeDateTime {
            get {
                return SR.GetResourceString("Arg_MustBeDateTime");
            }
        }

        // Token: 0x170003E3 RID: 995
        // (get) Token: 0x060021CD RID: 8653 RVA: 0x009E9C14 File Offset: 0x009E9C14
        internal static string Arg_MustBeDateTimeOffset {
            get {
                return SR.GetResourceString("Arg_MustBeDateTimeOffset");
            }
        }

        // Token: 0x170003E4 RID: 996
        // (get) Token: 0x060021CE RID: 8654 RVA: 0x009E9C20 File Offset: 0x009E9C20
        internal static string Arg_MustBeDecimal {
            get {
                return SR.GetResourceString("Arg_MustBeDecimal");
            }
        }

        // Token: 0x170003E5 RID: 997
        // (get) Token: 0x060021CF RID: 8655 RVA: 0x009E9C2C File Offset: 0x009E9C2C
        internal static string Arg_MustBeDecimal32 {
            get {
                return SR.GetResourceString("Arg_MustBeDecimal32");
            }
        }

        // Token: 0x170003E6 RID: 998
        // (get) Token: 0x060021D0 RID: 8656 RVA: 0x009E9C38 File Offset: 0x009E9C38
        internal static string Arg_MustBeDecimal64 {
            get {
                return SR.GetResourceString("Arg_MustBeDecimal64");
            }
        }

        // Token: 0x170003E7 RID: 999
        // (get) Token: 0x060021D1 RID: 8657 RVA: 0x009E9C44 File Offset: 0x009E9C44
        internal static string Arg_MustBeDecimal128 {
            get {
                return SR.GetResourceString("Arg_MustBeDecimal128");
            }
        }

        // Token: 0x170003E8 RID: 1000
        // (get) Token: 0x060021D2 RID: 8658 RVA: 0x009E9C50 File Offset: 0x009E9C50
        internal static string Arg_MustBeDelegate {
            get {
                return SR.GetResourceString("Arg_MustBeDelegate");
            }
        }

        // Token: 0x170003E9 RID: 1001
        // (get) Token: 0x060021D3 RID: 8659 RVA: 0x009E9C5C File Offset: 0x009E9C5C
        internal static string Arg_MustBeDouble {
            get {
                return SR.GetResourceString("Arg_MustBeDouble");
            }
        }

        // Token: 0x170003EA RID: 1002
        // (get) Token: 0x060021D4 RID: 8660 RVA: 0x009E9C68 File Offset: 0x009E9C68
        internal static string Arg_MustBeDriveLetterOrRootDir {
            get {
                return SR.GetResourceString("Arg_MustBeDriveLetterOrRootDir");
            }
        }

        // Token: 0x170003EB RID: 1003
        // (get) Token: 0x060021D5 RID: 8661 RVA: 0x009E9C74 File Offset: 0x009E9C74
        internal static string Arg_MustBeEnum {
            get {
                return SR.GetResourceString("Arg_MustBeEnum");
            }
        }

        // Token: 0x170003EC RID: 1004
        // (get) Token: 0x060021D6 RID: 8662 RVA: 0x009E9C80 File Offset: 0x009E9C80
        internal static string Arg_MustBeEnumBaseTypeOrEnum {
            get {
                return SR.GetResourceString("Arg_MustBeEnumBaseTypeOrEnum");
            }
        }

        // Token: 0x170003ED RID: 1005
        // (get) Token: 0x060021D7 RID: 8663 RVA: 0x009E9C8C File Offset: 0x009E9C8C
        internal static string Arg_MustBeGuid {
            get {
                return SR.GetResourceString("Arg_MustBeGuid");
            }
        }

        // Token: 0x170003EE RID: 1006
        // (get) Token: 0x060021D8 RID: 8664 RVA: 0x009E9C98 File Offset: 0x009E9C98
        internal static string Arg_MustBeInt16 {
            get {
                return SR.GetResourceString("Arg_MustBeInt16");
            }
        }

        // Token: 0x170003EF RID: 1007
        // (get) Token: 0x060021D9 RID: 8665 RVA: 0x009E9CA4 File Offset: 0x009E9CA4
        internal static string Arg_MustBeInt32 {
            get {
                return SR.GetResourceString("Arg_MustBeInt32");
            }
        }

        // Token: 0x170003F0 RID: 1008
        // (get) Token: 0x060021DA RID: 8666 RVA: 0x009E9CB0 File Offset: 0x009E9CB0
        internal static string Arg_MustBeInt64 {
            get {
                return SR.GetResourceString("Arg_MustBeInt64");
            }
        }

        // Token: 0x170003F1 RID: 1009
        // (get) Token: 0x060021DB RID: 8667 RVA: 0x009E9CBC File Offset: 0x009E9CBC
        internal static string Arg_MustBeInt128 {
            get {
                return SR.GetResourceString("Arg_MustBeInt128");
            }
        }

        // Token: 0x170003F2 RID: 1010
        // (get) Token: 0x060021DC RID: 8668 RVA: 0x009E9CC8 File Offset: 0x009E9CC8
        internal static string Arg_MustBeIntPtr {
            get {
                return SR.GetResourceString("Arg_MustBeIntPtr");
            }
        }

        // Token: 0x170003F3 RID: 1011
        // (get) Token: 0x060021DD RID: 8669 RVA: 0x009E9CD4 File Offset: 0x009E9CD4
        internal static string Arg_MustBeNFloat {
            get {
                return SR.GetResourceString("Arg_MustBeNFloat");
            }
        }

        // Token: 0x170003F4 RID: 1012
        // (get) Token: 0x060021DE RID: 8670 RVA: 0x009E9CE0 File Offset: 0x009E9CE0
        internal static string Arg_MustBePointer {
            get {
                return SR.GetResourceString("Arg_MustBePointer");
            }
        }

        // Token: 0x170003F5 RID: 1013
        // (get) Token: 0x060021DF RID: 8671 RVA: 0x009E9CEC File Offset: 0x009E9CEC
        internal static string Arg_MustBePrimArray {
            get {
                return SR.GetResourceString("Arg_MustBePrimArray");
            }
        }

        // Token: 0x170003F6 RID: 1014
        // (get) Token: 0x060021E0 RID: 8672 RVA: 0x009E9CF8 File Offset: 0x009E9CF8
        internal static string Arg_MustBeRuntimeAssembly {
            get {
                return SR.GetResourceString("Arg_MustBeRuntimeAssembly");
            }
        }

        // Token: 0x170003F7 RID: 1015
        // (get) Token: 0x060021E1 RID: 8673 RVA: 0x009E9D04 File Offset: 0x009E9D04
        internal static string Arg_MustBeSByte {
            get {
                return SR.GetResourceString("Arg_MustBeSByte");
            }
        }

        // Token: 0x170003F8 RID: 1016
        // (get) Token: 0x060021E2 RID: 8674 RVA: 0x009E9D10 File Offset: 0x009E9D10
        internal static string Arg_MustBeSingle {
            get {
                return SR.GetResourceString("Arg_MustBeSingle");
            }
        }

        // Token: 0x170003F9 RID: 1017
        // (get) Token: 0x060021E3 RID: 8675 RVA: 0x009E9D1C File Offset: 0x009E9D1C
        internal static string Arg_MustBeString {
            get {
                return SR.GetResourceString("Arg_MustBeString");
            }
        }

        // Token: 0x170003FA RID: 1018
        // (get) Token: 0x060021E4 RID: 8676 RVA: 0x009E9D28 File Offset: 0x009E9D28
        internal static string Arg_MustBeTimeSpan {
            get {
                return SR.GetResourceString("Arg_MustBeTimeSpan");
            }
        }

        // Token: 0x170003FB RID: 1019
        // (get) Token: 0x060021E5 RID: 8677 RVA: 0x009E9D34 File Offset: 0x009E9D34
        internal static string Arg_MustBeType {
            get {
                return SR.GetResourceString("Arg_MustBeType");
            }
        }

        // Token: 0x170003FC RID: 1020
        // (get) Token: 0x060021E6 RID: 8678 RVA: 0x009E9D40 File Offset: 0x009E9D40
        internal static string Arg_MustBeTrue {
            get {
                return SR.GetResourceString("Arg_MustBeTrue");
            }
        }

        // Token: 0x170003FD RID: 1021
        // (get) Token: 0x060021E7 RID: 8679 RVA: 0x009E9D4C File Offset: 0x009E9D4C
        internal static string Arg_MustBeUInt16 {
            get {
                return SR.GetResourceString("Arg_MustBeUInt16");
            }
        }

        // Token: 0x170003FE RID: 1022
        // (get) Token: 0x060021E8 RID: 8680 RVA: 0x009E9D58 File Offset: 0x009E9D58
        internal static string Arg_MustBeUInt32 {
            get {
                return SR.GetResourceString("Arg_MustBeUInt32");
            }
        }

        // Token: 0x170003FF RID: 1023
        // (get) Token: 0x060021E9 RID: 8681 RVA: 0x009E9D64 File Offset: 0x009E9D64
        internal static string Arg_MustBeUInt64 {
            get {
                return SR.GetResourceString("Arg_MustBeUInt64");
            }
        }

        // Token: 0x17000400 RID: 1024
        // (get) Token: 0x060021EA RID: 8682 RVA: 0x009E9D70 File Offset: 0x009E9D70
        internal static string Arg_MustBeUInt128 {
            get {
                return SR.GetResourceString("Arg_MustBeUInt128");
            }
        }

        // Token: 0x17000401 RID: 1025
        // (get) Token: 0x060021EB RID: 8683 RVA: 0x009E9D7C File Offset: 0x009E9D7C
        internal static string Arg_MustBeUIntPtr {
            get {
                return SR.GetResourceString("Arg_MustBeUIntPtr");
            }
        }

        // Token: 0x17000402 RID: 1026
        // (get) Token: 0x060021EC RID: 8684 RVA: 0x009E9D88 File Offset: 0x009E9D88
        internal static string Arg_MustBeVersion {
            get {
                return SR.GetResourceString("Arg_MustBeVersion");
            }
        }

        // Token: 0x17000403 RID: 1027
        // (get) Token: 0x060021ED RID: 8685 RVA: 0x009E9D94 File Offset: 0x009E9D94
        internal static string Arg_MustContainEnumInfo {
            get {
                return SR.GetResourceString("Arg_MustContainEnumInfo");
            }
        }

        // Token: 0x17000404 RID: 1028
        // (get) Token: 0x060021EE RID: 8686 RVA: 0x009E9DA0 File Offset: 0x009E9DA0
        internal static string Arg_NamedParamNull {
            get {
                return SR.GetResourceString("Arg_NamedParamNull");
            }
        }

        // Token: 0x17000405 RID: 1029
        // (get) Token: 0x060021EF RID: 8687 RVA: 0x009E9DAC File Offset: 0x009E9DAC
        internal static string Arg_NamedParamTooBig {
            get {
                return SR.GetResourceString("Arg_NamedParamTooBig");
            }
        }

        // Token: 0x17000406 RID: 1030
        // (get) Token: 0x060021F0 RID: 8688 RVA: 0x009E9DB8 File Offset: 0x009E9DB8
        internal static string Arg_PInvokeBadObject {
            get {
                return SR.GetResourceString("Arg_PInvokeBadObject");
            }
        }

        // Token: 0x17000407 RID: 1031
        // (get) Token: 0x060021F1 RID: 8689 RVA: 0x009E9DC4 File Offset: 0x009E9DC4
        internal static string Arg_Need1DArray {
            get {
                return SR.GetResourceString("Arg_Need1DArray");
            }
        }

        // Token: 0x17000408 RID: 1032
        // (get) Token: 0x060021F2 RID: 8690 RVA: 0x009E9DD0 File Offset: 0x009E9DD0
        internal static string Arg_Need2DArray {
            get {
                return SR.GetResourceString("Arg_Need2DArray");
            }
        }

        // Token: 0x17000409 RID: 1033
        // (get) Token: 0x060021F3 RID: 8691 RVA: 0x009E9DDC File Offset: 0x009E9DDC
        internal static string Arg_Need3DArray {
            get {
                return SR.GetResourceString("Arg_Need3DArray");
            }
        }

        // Token: 0x1700040A RID: 1034
        // (get) Token: 0x060021F4 RID: 8692 RVA: 0x009E9DE8 File Offset: 0x009E9DE8
        internal static string Arg_NeedAtLeast1Rank {
            get {
                return SR.GetResourceString("Arg_NeedAtLeast1Rank");
            }
        }

        // Token: 0x1700040B RID: 1035
        // (get) Token: 0x060021F5 RID: 8693 RVA: 0x009E9DF4 File Offset: 0x009E9DF4
        internal static string Arg_NoAccessSpec {
            get {
                return SR.GetResourceString("Arg_NoAccessSpec");
            }
        }

        // Token: 0x1700040C RID: 1036
        // (get) Token: 0x060021F6 RID: 8694 RVA: 0x009E9E00 File Offset: 0x009E9E00
        internal static string Arg_NoDefCTorWithoutTypeName {
            get {
                return SR.GetResourceString("Arg_NoDefCTorWithoutTypeName");
            }
        }

        // Token: 0x1700040D RID: 1037
        // (get) Token: 0x060021F7 RID: 8695 RVA: 0x009E9E0C File Offset: 0x009E9E0C
        internal static string Arg_NoDefCTor {
            get {
                return SR.GetResourceString("Arg_NoDefCTor");
            }
        }

        // Token: 0x1700040E RID: 1038
        // (get) Token: 0x060021F8 RID: 8696 RVA: 0x009E9E18 File Offset: 0x009E9E18
        internal static string Arg_NonZeroLowerBound {
            get {
                return SR.GetResourceString("Arg_NonZeroLowerBound");
            }
        }

        // Token: 0x1700040F RID: 1039
        // (get) Token: 0x060021F9 RID: 8697 RVA: 0x009E9E24 File Offset: 0x009E9E24
        internal static string Arg_NoStaticVirtual {
            get {
                return SR.GetResourceString("Arg_NoStaticVirtual");
            }
        }

        // Token: 0x17000410 RID: 1040
        // (get) Token: 0x060021FA RID: 8698 RVA: 0x009E9E30 File Offset: 0x009E9E30
        internal static string Arg_NotFiniteNumberException {
            get {
                return SR.GetResourceString("Arg_NotFiniteNumberException");
            }
        }

        // Token: 0x17000411 RID: 1041
        // (get) Token: 0x060021FB RID: 8699 RVA: 0x009E9E3C File Offset: 0x009E9E3C
        internal static string Arg_NotGenericMethodDefinition {
            get {
                return SR.GetResourceString("Arg_NotGenericMethodDefinition");
            }
        }

        // Token: 0x17000412 RID: 1042
        // (get) Token: 0x060021FC RID: 8700 RVA: 0x009E9E48 File Offset: 0x009E9E48
        internal static string Arg_NotGenericParameter {
            get {
                return SR.GetResourceString("Arg_NotGenericParameter");
            }
        }

        // Token: 0x17000413 RID: 1043
        // (get) Token: 0x060021FD RID: 8701 RVA: 0x009E9E54 File Offset: 0x009E9E54
        internal static string Arg_NotGenericTypeDefinition {
            get {
                return SR.GetResourceString("Arg_NotGenericTypeDefinition");
            }
        }

        // Token: 0x17000414 RID: 1044
        // (get) Token: 0x060021FE RID: 8702 RVA: 0x009E9E60 File Offset: 0x009E9E60
        internal static string Arg_NotImplementedException {
            get {
                return SR.GetResourceString("Arg_NotImplementedException");
            }
        }

        // Token: 0x17000415 RID: 1045
        // (get) Token: 0x060021FF RID: 8703 RVA: 0x009E9E6C File Offset: 0x009E9E6C
        internal static string Arg_NotSupportedException {
            get {
                return SR.GetResourceString("Arg_NotSupportedException");
            }
        }

        // Token: 0x17000416 RID: 1046
        // (get) Token: 0x06002200 RID: 8704 RVA: 0x009E9E78 File Offset: 0x009E9E78
        internal static string Arg_NullReferenceException {
            get {
                return SR.GetResourceString("Arg_NullReferenceException");
            }
        }

        // Token: 0x17000417 RID: 1047
        // (get) Token: 0x06002201 RID: 8705 RVA: 0x009E9E84 File Offset: 0x009E9E84
        internal static string Arg_ObjObjEx {
            get {
                return SR.GetResourceString("Arg_ObjObjEx");
            }
        }

        // Token: 0x17000418 RID: 1048
        // (get) Token: 0x06002202 RID: 8706 RVA: 0x009E9E90 File Offset: 0x009E9E90
        internal static string Arg_OleAutDateInvalid {
            get {
                return SR.GetResourceString("Arg_OleAutDateInvalid");
            }
        }

        // Token: 0x17000419 RID: 1049
        // (get) Token: 0x06002203 RID: 8707 RVA: 0x009E9E9C File Offset: 0x009E9E9C
        internal static string Arg_OleAutDateScale {
            get {
                return SR.GetResourceString("Arg_OleAutDateScale");
            }
        }

        // Token: 0x1700041A RID: 1050
        // (get) Token: 0x06002204 RID: 8708 RVA: 0x009E9EA8 File Offset: 0x009E9EA8
        internal static string Arg_OverflowException {
            get {
                return SR.GetResourceString("Arg_OverflowException");
            }
        }

        // Token: 0x1700041B RID: 1051
        // (get) Token: 0x06002205 RID: 8709 RVA: 0x009E9EB4 File Offset: 0x009E9EB4
        internal static string Arg_ParamName_Name {
            get {
                return SR.GetResourceString("Arg_ParamName_Name");
            }
        }

        // Token: 0x1700041C RID: 1052
        // (get) Token: 0x06002206 RID: 8710 RVA: 0x009E9EC0 File Offset: 0x009E9EC0
        internal static string Arg_ParmArraySize {
            get {
                return SR.GetResourceString("Arg_ParmArraySize");
            }
        }

        // Token: 0x1700041D RID: 1053
        // (get) Token: 0x06002207 RID: 8711 RVA: 0x009E9ECC File Offset: 0x009E9ECC
        internal static string Arg_ParmCnt {
            get {
                return SR.GetResourceString("Arg_ParmCnt");
            }
        }

        // Token: 0x1700041E RID: 1054
        // (get) Token: 0x06002208 RID: 8712 RVA: 0x009E9ED8 File Offset: 0x009E9ED8
        internal static string Arg_PathEmpty {
            get {
                return SR.GetResourceString("Arg_PathEmpty");
            }
        }

        // Token: 0x1700041F RID: 1055
        // (get) Token: 0x06002209 RID: 8713 RVA: 0x009E9EE4 File Offset: 0x009E9EE4
        internal static string Arg_PlatformNotSupported {
            get {
                return SR.GetResourceString("Arg_PlatformNotSupported");
            }
        }

        // Token: 0x17000420 RID: 1056
        // (get) Token: 0x0600220A RID: 8714 RVA: 0x009E9EF0 File Offset: 0x009E9EF0
        internal static string Arg_PrimWiden {
            get {
                return SR.GetResourceString("Arg_PrimWiden");
            }
        }

        // Token: 0x17000421 RID: 1057
        // (get) Token: 0x0600220B RID: 8715 RVA: 0x009E9EFC File Offset: 0x009E9EFC
        internal static string Arg_PropSetGet {
            get {
                return SR.GetResourceString("Arg_PropSetGet");
            }
        }

        // Token: 0x17000422 RID: 1058
        // (get) Token: 0x0600220C RID: 8716 RVA: 0x009E9F08 File Offset: 0x009E9F08
        internal static string Arg_PropSetInvoke {
            get {
                return SR.GetResourceString("Arg_PropSetInvoke");
            }
        }

        // Token: 0x17000423 RID: 1059
        // (get) Token: 0x0600220D RID: 8717 RVA: 0x009E9F14 File Offset: 0x009E9F14
        internal static string Arg_RankException {
            get {
                return SR.GetResourceString("Arg_RankException");
            }
        }

        // Token: 0x17000424 RID: 1060
        // (get) Token: 0x0600220E RID: 8718 RVA: 0x009E9F20 File Offset: 0x009E9F20
        internal static string Arg_RankIndices {
            get {
                return SR.GetResourceString("Arg_RankIndices");
            }
        }

        // Token: 0x17000425 RID: 1061
        // (get) Token: 0x0600220F RID: 8719 RVA: 0x009E9F2C File Offset: 0x009E9F2C
        internal static string Arg_RankMultiDimNotSupported {
            get {
                return SR.GetResourceString("Arg_RankMultiDimNotSupported");
            }
        }

        // Token: 0x17000426 RID: 1062
        // (get) Token: 0x06002210 RID: 8720 RVA: 0x009E9F38 File Offset: 0x009E9F38
        internal static string Arg_RanksAndBounds {
            get {
                return SR.GetResourceString("Arg_RanksAndBounds");
            }
        }

        // Token: 0x17000427 RID: 1063
        // (get) Token: 0x06002211 RID: 8721 RVA: 0x009E9F44 File Offset: 0x009E9F44
        internal static string Arg_RegValueTooLarge {
            get {
                return SR.GetResourceString("Arg_RegValueTooLarge");
            }
        }

        // Token: 0x17000428 RID: 1064
        // (get) Token: 0x06002212 RID: 8722 RVA: 0x009E9F50 File Offset: 0x009E9F50
        internal static string Arg_RegKeyNotFound {
            get {
                return SR.GetResourceString("Arg_RegKeyNotFound");
            }
        }

        // Token: 0x17000429 RID: 1065
        // (get) Token: 0x06002213 RID: 8723 RVA: 0x009E9F5C File Offset: 0x009E9F5C
        internal static string Arg_RegSubKeyValueAbsent {
            get {
                return SR.GetResourceString("Arg_RegSubKeyValueAbsent");
            }
        }

        // Token: 0x1700042A RID: 1066
        // (get) Token: 0x06002214 RID: 8724 RVA: 0x009E9F68 File Offset: 0x009E9F68
        internal static string Arg_RegValStrLenBug {
            get {
                return SR.GetResourceString("Arg_RegValStrLenBug");
            }
        }

        // Token: 0x1700042B RID: 1067
        // (get) Token: 0x06002215 RID: 8725 RVA: 0x009E9F74 File Offset: 0x009E9F74
        internal static string Arg_ResMgrNotResSet {
            get {
                return SR.GetResourceString("Arg_ResMgrNotResSet");
            }
        }

        // Token: 0x1700042C RID: 1068
        // (get) Token: 0x06002216 RID: 8726 RVA: 0x009E9F80 File Offset: 0x009E9F80
        internal static string Arg_ResourceFileUnsupportedVersion {
            get {
                return SR.GetResourceString("Arg_ResourceFileUnsupportedVersion");
            }
        }

        // Token: 0x1700042D RID: 1069
        // (get) Token: 0x06002217 RID: 8727 RVA: 0x009E9F8C File Offset: 0x009E9F8C
        internal static string Arg_ResourceNameNotExist {
            get {
                return SR.GetResourceString("Arg_ResourceNameNotExist");
            }
        }

        // Token: 0x1700042E RID: 1070
        // (get) Token: 0x06002218 RID: 8728 RVA: 0x009E9F98 File Offset: 0x009E9F98
        internal static string Arg_SafeArrayRankMismatchException {
            get {
                return SR.GetResourceString("Arg_SafeArrayRankMismatchException");
            }
        }

        // Token: 0x1700042F RID: 1071
        // (get) Token: 0x06002219 RID: 8729 RVA: 0x009E9FA4 File Offset: 0x009E9FA4
        internal static string Arg_SafeArrayTypeMismatchException {
            get {
                return SR.GetResourceString("Arg_SafeArrayTypeMismatchException");
            }
        }

        // Token: 0x17000430 RID: 1072
        // (get) Token: 0x0600221A RID: 8730 RVA: 0x009E9FB0 File Offset: 0x009E9FB0
        internal static string Arg_SecurityException {
            get {
                return SR.GetResourceString("Arg_SecurityException");
            }
        }

        // Token: 0x17000431 RID: 1073
        // (get) Token: 0x0600221B RID: 8731 RVA: 0x009E9FBC File Offset: 0x009E9FBC
        internal static string SerializationException {
            get {
                return SR.GetResourceString("SerializationException");
            }
        }

        // Token: 0x17000432 RID: 1074
        // (get) Token: 0x0600221C RID: 8732 RVA: 0x009E9FC8 File Offset: 0x009E9FC8
        internal static string Arg_SetMethNotFnd {
            get {
                return SR.GetResourceString("Arg_SetMethNotFnd");
            }
        }

        // Token: 0x17000433 RID: 1075
        // (get) Token: 0x0600221D RID: 8733 RVA: 0x009E9FD4 File Offset: 0x009E9FD4
        internal static string Arg_SpanMustHaveElementsForColor {
            get {
                return SR.GetResourceString("Arg_SpanMustHaveElementsForColor");
            }
        }

        // Token: 0x17000434 RID: 1076
        // (get) Token: 0x0600221E RID: 8734 RVA: 0x009E9FE0 File Offset: 0x009E9FE0
        internal static string Arg_StackOverflowException {
            get {
                return SR.GetResourceString("Arg_StackOverflowException");
            }
        }

        // Token: 0x17000435 RID: 1077
        // (get) Token: 0x0600221F RID: 8735 RVA: 0x009E9FEC File Offset: 0x009E9FEC
        internal static string Arg_SurrogatesNotAllowedAsSingleChar {
            get {
                return SR.GetResourceString("Arg_SurrogatesNotAllowedAsSingleChar");
            }
        }

        // Token: 0x17000436 RID: 1078
        // (get) Token: 0x06002220 RID: 8736 RVA: 0x009E9FF8 File Offset: 0x009E9FF8
        internal static string Arg_SynchronizationLockException {
            get {
                return SR.GetResourceString("Arg_SynchronizationLockException");
            }
        }

        // Token: 0x17000437 RID: 1079
        // (get) Token: 0x06002221 RID: 8737 RVA: 0x009EA004 File Offset: 0x009EA004
        internal static string Arg_SystemException {
            get {
                return SR.GetResourceString("Arg_SystemException");
            }
        }

        // Token: 0x17000438 RID: 1080
        // (get) Token: 0x06002222 RID: 8738 RVA: 0x009EA010 File Offset: 0x009EA010
        internal static string Arg_TargetInvocationException {
            get {
                return SR.GetResourceString("Arg_TargetInvocationException");
            }
        }

        // Token: 0x17000439 RID: 1081
        // (get) Token: 0x06002223 RID: 8739 RVA: 0x009EA01C File Offset: 0x009EA01C
        internal static string Arg_TargetParameterCountException {
            get {
                return SR.GetResourceString("Arg_TargetParameterCountException");
            }
        }

        // Token: 0x1700043A RID: 1082
        // (get) Token: 0x06002224 RID: 8740 RVA: 0x009EA028 File Offset: 0x009EA028
        internal static string Arg_ThreadStartException {
            get {
                return SR.GetResourceString("Arg_ThreadStartException");
            }
        }

        // Token: 0x1700043B RID: 1083
        // (get) Token: 0x06002225 RID: 8741 RVA: 0x009EA034 File Offset: 0x009EA034
        internal static string Arg_ThreadStateException {
            get {
                return SR.GetResourceString("Arg_ThreadStateException");
            }
        }

        // Token: 0x1700043C RID: 1084
        // (get) Token: 0x06002226 RID: 8742 RVA: 0x009EA040 File Offset: 0x009EA040
        internal static string Arg_TimeoutException {
            get {
                return SR.GetResourceString("Arg_TimeoutException");
            }
        }

        // Token: 0x1700043D RID: 1085
        // (get) Token: 0x06002227 RID: 8743 RVA: 0x009EA04C File Offset: 0x009EA04C
        internal static string Arg_TypeAccessException {
            get {
                return SR.GetResourceString("Arg_TypeAccessException");
            }
        }

        // Token: 0x1700043E RID: 1086
        // (get) Token: 0x06002228 RID: 8744 RVA: 0x009EA058 File Offset: 0x009EA058
        internal static string Arg_TypedReference_Null {
            get {
                return SR.GetResourceString("Arg_TypedReference_Null");
            }
        }

        // Token: 0x1700043F RID: 1087
        // (get) Token: 0x06002229 RID: 8745 RVA: 0x009EA064 File Offset: 0x009EA064
        internal static string Arg_TypeLoadException {
            get {
                return SR.GetResourceString("Arg_TypeLoadException");
            }
        }

        // Token: 0x17000440 RID: 1088
        // (get) Token: 0x0600222A RID: 8746 RVA: 0x009EA070 File Offset: 0x009EA070
        internal static string Arg_TypeLoadNullStr {
            get {
                return SR.GetResourceString("Arg_TypeLoadNullStr");
            }
        }

        // Token: 0x17000441 RID: 1089
        // (get) Token: 0x0600222B RID: 8747 RVA: 0x009EA07C File Offset: 0x009EA07C
        internal static string Arg_TypeUnloadedException {
            get {
                return SR.GetResourceString("Arg_TypeUnloadedException");
            }
        }

        // Token: 0x17000442 RID: 1090
        // (get) Token: 0x0600222C RID: 8748 RVA: 0x009EA088 File Offset: 0x009EA088
        internal static string Arg_UnauthorizedAccessException {
            get {
                return SR.GetResourceString("Arg_UnauthorizedAccessException");
            }
        }

        // Token: 0x17000443 RID: 1091
        // (get) Token: 0x0600222D RID: 8749 RVA: 0x009EA094 File Offset: 0x009EA094
        internal static string Arg_UnboundGenField {
            get {
                return SR.GetResourceString("Arg_UnboundGenField");
            }
        }

        // Token: 0x17000444 RID: 1092
        // (get) Token: 0x0600222E RID: 8750 RVA: 0x009EA0A0 File Offset: 0x009EA0A0
        internal static string Arg_UnboundGenParam {
            get {
                return SR.GetResourceString("Arg_UnboundGenParam");
            }
        }

        // Token: 0x17000445 RID: 1093
        // (get) Token: 0x0600222F RID: 8751 RVA: 0x009EA0AC File Offset: 0x009EA0AC
        internal static string Arg_UnknownTypeCode {
            get {
                return SR.GetResourceString("Arg_UnknownTypeCode");
            }
        }

        // Token: 0x17000446 RID: 1094
        // (get) Token: 0x06002230 RID: 8752 RVA: 0x009EA0B8 File Offset: 0x009EA0B8
        internal static string Arg_UnreachableException {
            get {
                return SR.GetResourceString("Arg_UnreachableException");
            }
        }

        // Token: 0x17000447 RID: 1095
        // (get) Token: 0x06002231 RID: 8753 RVA: 0x009EA0C4 File Offset: 0x009EA0C4
        internal static string Arg_VarMissNull {
            get {
                return SR.GetResourceString("Arg_VarMissNull");
            }
        }

        // Token: 0x17000448 RID: 1096
        // (get) Token: 0x06002232 RID: 8754 RVA: 0x009EA0D0 File Offset: 0x009EA0D0
        internal static string Arg_VersionString {
            get {
                return SR.GetResourceString("Arg_VersionString");
            }
        }

        // Token: 0x17000449 RID: 1097
        // (get) Token: 0x06002233 RID: 8755 RVA: 0x009EA0DC File Offset: 0x009EA0DC
        internal static string Arg_WrongType {
            get {
                return SR.GetResourceString("Arg_WrongType");
            }
        }

        // Token: 0x1700044A RID: 1098
        // (get) Token: 0x06002234 RID: 8756 RVA: 0x009EA0E8 File Offset: 0x009EA0E8
        internal static string Argument_AbsolutePathRequired {
            get {
                return SR.GetResourceString("Argument_AbsolutePathRequired");
            }
        }

        // Token: 0x1700044B RID: 1099
        // (get) Token: 0x06002235 RID: 8757 RVA: 0x009EA0F4 File Offset: 0x009EA0F4
        internal static string Argument_AddingDuplicate {
            get {
                return SR.GetResourceString("Argument_AddingDuplicate");
            }
        }

        // Token: 0x1700044C RID: 1100
        // (get) Token: 0x06002236 RID: 8758 RVA: 0x009EA100 File Offset: 0x009EA100
        internal static string Argument_AddingDuplicate__ {
            get {
                return SR.GetResourceString("Argument_AddingDuplicate__");
            }
        }

        // Token: 0x1700044D RID: 1101
        // (get) Token: 0x06002237 RID: 8759 RVA: 0x009EA10C File Offset: 0x009EA10C
        internal static string Argument_AddingDuplicateWithKey {
            get {
                return SR.GetResourceString("Argument_AddingDuplicateWithKey");
            }
        }

        // Token: 0x1700044E RID: 1102
        // (get) Token: 0x06002238 RID: 8760 RVA: 0x009EA118 File Offset: 0x009EA118
        internal static string Argument_AdjustmentRulesNoNulls {
            get {
                return SR.GetResourceString("Argument_AdjustmentRulesNoNulls");
            }
        }

        // Token: 0x1700044F RID: 1103
        // (get) Token: 0x06002239 RID: 8761 RVA: 0x009EA124 File Offset: 0x009EA124
        internal static string Argument_AdjustmentRulesOutOfOrder {
            get {
                return SR.GetResourceString("Argument_AdjustmentRulesOutOfOrder");
            }
        }

        // Token: 0x17000450 RID: 1104
        // (get) Token: 0x0600223A RID: 8762 RVA: 0x009EA130 File Offset: 0x009EA130
        internal static string Argument_AlignmentMustBePow2 {
            get {
                return SR.GetResourceString("Argument_AlignmentMustBePow2");
            }
        }

        // Token: 0x17000451 RID: 1105
        // (get) Token: 0x0600223B RID: 8763 RVA: 0x009EA13C File Offset: 0x009EA13C
        internal static string Argument_AlreadyBoundOrSyncHandle {
            get {
                return SR.GetResourceString("Argument_AlreadyBoundOrSyncHandle");
            }
        }

        // Token: 0x17000452 RID: 1106
        // (get) Token: 0x0600223C RID: 8764 RVA: 0x009EA148 File Offset: 0x009EA148
        internal static string Argument_ArrayGetInterfaceMap {
            get {
                return SR.GetResourceString("Argument_ArrayGetInterfaceMap");
            }
        }

        // Token: 0x17000453 RID: 1107
        // (get) Token: 0x0600223D RID: 8765 RVA: 0x009EA154 File Offset: 0x009EA154
        internal static string Argument_ArraysInvalid {
            get {
                return SR.GetResourceString("Argument_ArraysInvalid");
            }
        }

        // Token: 0x17000454 RID: 1108
        // (get) Token: 0x0600223E RID: 8766 RVA: 0x009EA160 File Offset: 0x009EA160
        internal static string Argument_ArrayTooLarge {
            get {
                return SR.GetResourceString("Argument_ArrayTooLarge");
            }
        }

        // Token: 0x17000455 RID: 1109
        // (get) Token: 0x0600223F RID: 8767 RVA: 0x009EA16C File Offset: 0x009EA16C
        internal static string Argument_AttributeNamesMustBeUnique {
            get {
                return SR.GetResourceString("Argument_AttributeNamesMustBeUnique");
            }
        }

        // Token: 0x17000456 RID: 1110
        // (get) Token: 0x06002240 RID: 8768 RVA: 0x009EA178 File Offset: 0x009EA178
        internal static string Argument_BadConstructor {
            get {
                return SR.GetResourceString("Argument_BadConstructor");
            }
        }

        // Token: 0x17000457 RID: 1111
        // (get) Token: 0x06002241 RID: 8769 RVA: 0x009EA184 File Offset: 0x009EA184
        internal static string Argument_BadConstructorCallConv {
            get {
                return SR.GetResourceString("Argument_BadConstructorCallConv");
            }
        }

        // Token: 0x17000458 RID: 1112
        // (get) Token: 0x06002242 RID: 8770 RVA: 0x009EA190 File Offset: 0x009EA190
        internal static string Argument_BadExceptionCodeGen {
            get {
                return SR.GetResourceString("Argument_BadExceptionCodeGen");
            }
        }

        // Token: 0x17000459 RID: 1113
        // (get) Token: 0x06002243 RID: 8771 RVA: 0x009EA19C File Offset: 0x009EA19C
        internal static string Argument_BadFieldForConstructorBuilder {
            get {
                return SR.GetResourceString("Argument_BadFieldForConstructorBuilder");
            }
        }

        // Token: 0x1700045A RID: 1114
        // (get) Token: 0x06002244 RID: 8772 RVA: 0x009EA1A8 File Offset: 0x009EA1A8
        internal static string Argument_BadFieldSig {
            get {
                return SR.GetResourceString("Argument_BadFieldSig");
            }
        }

        // Token: 0x1700045B RID: 1115
        // (get) Token: 0x06002245 RID: 8773 RVA: 0x009EA1B4 File Offset: 0x009EA1B4
        internal static string Argument_BadFieldType {
            get {
                return SR.GetResourceString("Argument_BadFieldType");
            }
        }

        // Token: 0x1700045C RID: 1116
        // (get) Token: 0x06002246 RID: 8774 RVA: 0x009EA1C0 File Offset: 0x009EA1C0
        internal static string Argument_BadFormatSpecifier {
            get {
                return SR.GetResourceString("Argument_BadFormatSpecifier");
            }
        }

        // Token: 0x1700045D RID: 1117
        // (get) Token: 0x06002247 RID: 8775 RVA: 0x009EA1CC File Offset: 0x009EA1CC
        internal static string Argument_BadImageFormatExceptionResolve {
            get {
                return SR.GetResourceString("Argument_BadImageFormatExceptionResolve");
            }
        }

        // Token: 0x1700045E RID: 1118
        // (get) Token: 0x06002248 RID: 8776 RVA: 0x009EA1D8 File Offset: 0x009EA1D8
        internal static string Argument_BadLabel {
            get {
                return SR.GetResourceString("Argument_BadLabel");
            }
        }

        // Token: 0x1700045F RID: 1119
        // (get) Token: 0x06002249 RID: 8777 RVA: 0x009EA1E4 File Offset: 0x009EA1E4
        internal static string Argument_BadLabelContent {
            get {
                return SR.GetResourceString("Argument_BadLabelContent");
            }
        }

        // Token: 0x17000460 RID: 1120
        // (get) Token: 0x0600224A RID: 8778 RVA: 0x009EA1F0 File Offset: 0x009EA1F0
        internal static string Argument_BadNestedTypeFlags {
            get {
                return SR.GetResourceString("Argument_BadNestedTypeFlags");
            }
        }

        // Token: 0x17000461 RID: 1121
        // (get) Token: 0x0600224B RID: 8779 RVA: 0x009EA1FC File Offset: 0x009EA1FC
        internal static string Argument_BadParameterCountsForConstructor {
            get {
                return SR.GetResourceString("Argument_BadParameterCountsForConstructor");
            }
        }

        // Token: 0x17000462 RID: 1122
        // (get) Token: 0x0600224C RID: 8780 RVA: 0x009EA208 File Offset: 0x009EA208
        internal static string Argument_BadParameterTypeForCAB {
            get {
                return SR.GetResourceString("Argument_BadParameterTypeForCAB");
            }
        }

        // Token: 0x17000463 RID: 1123
        // (get) Token: 0x0600224D RID: 8781 RVA: 0x009EA214 File Offset: 0x009EA214
        internal static string Argument_BadPropertyForConstructorBuilder {
            get {
                return SR.GetResourceString("Argument_BadPropertyForConstructorBuilder");
            }
        }

        // Token: 0x17000464 RID: 1124
        // (get) Token: 0x0600224E RID: 8782 RVA: 0x009EA220 File Offset: 0x009EA220
        internal static string Argument_BadSigFormat {
            get {
                return SR.GetResourceString("Argument_BadSigFormat");
            }
        }

        // Token: 0x17000465 RID: 1125
        // (get) Token: 0x0600224F RID: 8783 RVA: 0x009EA22C File Offset: 0x009EA22C
        internal static string Argument_BadSizeForData {
            get {
                return SR.GetResourceString("Argument_BadSizeForData");
            }
        }

        // Token: 0x17000466 RID: 1126
        // (get) Token: 0x06002250 RID: 8784 RVA: 0x009EA238 File Offset: 0x009EA238
        internal static string Argument_BadTypeAttrNestedVisibilityOnNonNestedType {
            get {
                return SR.GetResourceString("Argument_BadTypeAttrNestedVisibilityOnNonNestedType");
            }
        }

        // Token: 0x17000467 RID: 1127
        // (get) Token: 0x06002251 RID: 8785 RVA: 0x009EA244 File Offset: 0x009EA244
        internal static string Argument_BadTypeAttrNonNestedVisibilityNestedType {
            get {
                return SR.GetResourceString("Argument_BadTypeAttrNonNestedVisibilityNestedType");
            }
        }

        // Token: 0x17000468 RID: 1128
        // (get) Token: 0x06002252 RID: 8786 RVA: 0x009EA250 File Offset: 0x009EA250
        internal static string Argument_BadTypeAttrReservedBitsSet {
            get {
                return SR.GetResourceString("Argument_BadTypeAttrReservedBitsSet");
            }
        }

        // Token: 0x17000469 RID: 1129
        // (get) Token: 0x06002253 RID: 8787 RVA: 0x009EA25C File Offset: 0x009EA25C
        internal static string Argument_BadTypeInCustomAttribute {
            get {
                return SR.GetResourceString("Argument_BadTypeInCustomAttribute");
            }
        }

        // Token: 0x1700046A RID: 1130
        // (get) Token: 0x06002254 RID: 8788 RVA: 0x009EA268 File Offset: 0x009EA268
        internal static string Argument_CannotSetParentToInterface {
            get {
                return SR.GetResourceString("Argument_CannotSetParentToInterface");
            }
        }

        // Token: 0x1700046B RID: 1131
        // (get) Token: 0x06002255 RID: 8789 RVA: 0x009EA274 File Offset: 0x009EA274
        internal static string Argument_CodepageNotSupported {
            get {
                return SR.GetResourceString("Argument_CodepageNotSupported");
            }
        }

        // Token: 0x1700046C RID: 1132
        // (get) Token: 0x06002256 RID: 8790 RVA: 0x009EA280 File Offset: 0x009EA280
        internal static string Argument_CompareOptionOrdinal {
            get {
                return SR.GetResourceString("Argument_CompareOptionOrdinal");
            }
        }

        // Token: 0x1700046D RID: 1133
        // (get) Token: 0x06002257 RID: 8791 RVA: 0x009EA28C File Offset: 0x009EA28C
        internal static string Argument_ConflictingDateTimeRoundtripStyles {
            get {
                return SR.GetResourceString("Argument_ConflictingDateTimeRoundtripStyles");
            }
        }

        // Token: 0x1700046E RID: 1134
        // (get) Token: 0x06002258 RID: 8792 RVA: 0x009EA298 File Offset: 0x009EA298
        internal static string Argument_ConflictingDateTimeStyles {
            get {
                return SR.GetResourceString("Argument_ConflictingDateTimeStyles");
            }
        }

        // Token: 0x1700046F RID: 1135
        // (get) Token: 0x06002259 RID: 8793 RVA: 0x009EA2A4 File Offset: 0x009EA2A4
        internal static string Argument_ConstantDoesntMatch {
            get {
                return SR.GetResourceString("Argument_ConstantDoesntMatch");
            }
        }

        // Token: 0x17000470 RID: 1136
        // (get) Token: 0x0600225A RID: 8794 RVA: 0x009EA2B0 File Offset: 0x009EA2B0
        internal static string Argument_ConstantNotSupported {
            get {
                return SR.GetResourceString("Argument_ConstantNotSupported");
            }
        }

        // Token: 0x17000471 RID: 1137
        // (get) Token: 0x0600225B RID: 8795 RVA: 0x009EA2BC File Offset: 0x009EA2BC
        internal static string Argument_ConstantNull {
            get {
                return SR.GetResourceString("Argument_ConstantNull");
            }
        }

        // Token: 0x17000472 RID: 1138
        // (get) Token: 0x0600225C RID: 8796 RVA: 0x009EA2C8 File Offset: 0x009EA2C8
        internal static string Argument_ConstructorNeedGenericDeclaringType {
            get {
                return SR.GetResourceString("Argument_ConstructorNeedGenericDeclaringType");
            }
        }

        // Token: 0x17000473 RID: 1139
        // (get) Token: 0x0600225D RID: 8797 RVA: 0x009EA2D4 File Offset: 0x009EA2D4
        internal static string Argument_ConversionOverflow {
            get {
                return SR.GetResourceString("Argument_ConversionOverflow");
            }
        }

        // Token: 0x17000474 RID: 1140
        // (get) Token: 0x0600225E RID: 8798 RVA: 0x009EA2E0 File Offset: 0x009EA2E0
        internal static string Argument_ConvertMismatch {
            get {
                return SR.GetResourceString("Argument_ConvertMismatch");
            }
        }

        // Token: 0x17000475 RID: 1141
        // (get) Token: 0x0600225F RID: 8799 RVA: 0x009EA2EC File Offset: 0x009EA2EC
        internal static string Argument_CultureIetfNotSupported {
            get {
                return SR.GetResourceString("Argument_CultureIetfNotSupported");
            }
        }

        // Token: 0x17000476 RID: 1142
        // (get) Token: 0x06002260 RID: 8800 RVA: 0x009EA2F8 File Offset: 0x009EA2F8
        internal static string Argument_CultureInvalidIdentifier {
            get {
                return SR.GetResourceString("Argument_CultureInvalidIdentifier");
            }
        }

        // Token: 0x17000477 RID: 1143
        // (get) Token: 0x06002261 RID: 8801 RVA: 0x009EA304 File Offset: 0x009EA304
        internal static string Argument_CultureIsNeutral {
            get {
                return SR.GetResourceString("Argument_CultureIsNeutral");
            }
        }

        // Token: 0x17000478 RID: 1144
        // (get) Token: 0x06002262 RID: 8802 RVA: 0x009EA310 File Offset: 0x009EA310
        internal static string Argument_CultureNotSupported {
            get {
                return SR.GetResourceString("Argument_CultureNotSupported");
            }
        }

        // Token: 0x17000479 RID: 1145
        // (get) Token: 0x06002263 RID: 8803 RVA: 0x009EA31C File Offset: 0x009EA31C
        internal static string Argument_CultureNotSupportedInInvariantMode {
            get {
                return SR.GetResourceString("Argument_CultureNotSupportedInInvariantMode");
            }
        }

        // Token: 0x1700047A RID: 1146
        // (get) Token: 0x06002264 RID: 8804 RVA: 0x009EA328 File Offset: 0x009EA328
        internal static string InvalidOperation_ResolvedAssemblyMustBeRuntimeAssembly {
            get {
                return SR.GetResourceString("InvalidOperation_ResolvedAssemblyMustBeRuntimeAssembly");
            }
        }

        // Token: 0x1700047B RID: 1147
        // (get) Token: 0x06002265 RID: 8805 RVA: 0x009EA334 File Offset: 0x009EA334
        internal static string InvalidOperation_ResolvedAssemblyRequestedNameMismatch {
            get {
                return SR.GetResourceString("InvalidOperation_ResolvedAssemblyRequestedNameMismatch");
            }
        }

        // Token: 0x1700047C RID: 1148
        // (get) Token: 0x06002266 RID: 8806 RVA: 0x009EA340 File Offset: 0x009EA340
        internal static string Argument_CustomCultureCannotBePassedByNumber {
            get {
                return SR.GetResourceString("Argument_CustomCultureCannotBePassedByNumber");
            }
        }

        // Token: 0x1700047D RID: 1149
        // (get) Token: 0x06002267 RID: 8807 RVA: 0x009EA34C File Offset: 0x009EA34C
        internal static string Argument_DateTimeBadBinaryData {
            get {
                return SR.GetResourceString("Argument_DateTimeBadBinaryData");
            }
        }

        // Token: 0x1700047E RID: 1150
        // (get) Token: 0x06002268 RID: 8808 RVA: 0x009EA358 File Offset: 0x009EA358
        internal static string Argument_DateTimeHasTicks {
            get {
                return SR.GetResourceString("Argument_DateTimeHasTicks");
            }
        }

        // Token: 0x1700047F RID: 1151
        // (get) Token: 0x06002269 RID: 8809 RVA: 0x009EA364 File Offset: 0x009EA364
        internal static string Argument_DateTimeHasTimeOfDay {
            get {
                return SR.GetResourceString("Argument_DateTimeHasTimeOfDay");
            }
        }

        // Token: 0x17000480 RID: 1152
        // (get) Token: 0x0600226A RID: 8810 RVA: 0x009EA370 File Offset: 0x009EA370
        internal static string Argument_DateTimeIsInvalid {
            get {
                return SR.GetResourceString("Argument_DateTimeIsInvalid");
            }
        }

        // Token: 0x17000481 RID: 1153
        // (get) Token: 0x0600226B RID: 8811 RVA: 0x009EA37C File Offset: 0x009EA37C
        internal static string Argument_DateTimeIsNotAmbiguous {
            get {
                return SR.GetResourceString("Argument_DateTimeIsNotAmbiguous");
            }
        }

        // Token: 0x17000482 RID: 1154
        // (get) Token: 0x0600226C RID: 8812 RVA: 0x009EA388 File Offset: 0x009EA388
        internal static string Argument_DateTimeKindMustBeUnspecified {
            get {
                return SR.GetResourceString("Argument_DateTimeKindMustBeUnspecified");
            }
        }

        // Token: 0x17000483 RID: 1155
        // (get) Token: 0x0600226D RID: 8813 RVA: 0x009EA394 File Offset: 0x009EA394
        internal static string Argument_DateTimeKindMustBeUnspecifiedOrUtc {
            get {
                return SR.GetResourceString("Argument_DateTimeKindMustBeUnspecifiedOrUtc");
            }
        }

        // Token: 0x17000484 RID: 1156
        // (get) Token: 0x0600226E RID: 8814 RVA: 0x009EA3A0 File Offset: 0x009EA3A0
        internal static string Argument_DateTimeOffsetInvalidDateTimeStyles {
            get {
                return SR.GetResourceString("Argument_DateTimeOffsetInvalidDateTimeStyles");
            }
        }

        // Token: 0x17000485 RID: 1157
        // (get) Token: 0x0600226F RID: 8815 RVA: 0x009EA3AC File Offset: 0x009EA3AC
        internal static string Argument_DateTimeOffsetIsNotAmbiguous {
            get {
                return SR.GetResourceString("Argument_DateTimeOffsetIsNotAmbiguous");
            }
        }

        // Token: 0x17000486 RID: 1158
        // (get) Token: 0x06002270 RID: 8816 RVA: 0x009EA3B8 File Offset: 0x009EA3B8
        internal static string Argument_DestinationTooShort {
            get {
                return SR.GetResourceString("Argument_DestinationTooShort");
            }
        }

        // Token: 0x17000487 RID: 1159
        // (get) Token: 0x06002271 RID: 8817 RVA: 0x009EA3C4 File Offset: 0x009EA3C4
        internal static string Argument_DuplicateTypeName {
            get {
                return SR.GetResourceString("Argument_DuplicateTypeName");
            }
        }

        // Token: 0x17000488 RID: 1160
        // (get) Token: 0x06002272 RID: 8818 RVA: 0x009EA3D0 File Offset: 0x009EA3D0
        internal static string Argument_EmitWriteLineType {
            get {
                return SR.GetResourceString("Argument_EmitWriteLineType");
            }
        }

        // Token: 0x17000489 RID: 1161
        // (get) Token: 0x06002273 RID: 8819 RVA: 0x009EA3DC File Offset: 0x009EA3DC
        internal static string Argument_EmptyWaithandleArray {
            get {
                return SR.GetResourceString("Argument_EmptyWaithandleArray");
            }
        }

        // Token: 0x1700048A RID: 1162
        // (get) Token: 0x06002274 RID: 8820 RVA: 0x009EA3E8 File Offset: 0x009EA3E8
        internal static string Argument_EncoderFallbackNotEmpty {
            get {
                return SR.GetResourceString("Argument_EncoderFallbackNotEmpty");
            }
        }

        // Token: 0x1700048B RID: 1163
        // (get) Token: 0x06002275 RID: 8821 RVA: 0x009EA3F4 File Offset: 0x009EA3F4
        internal static string Argument_EncodingConversionOverflowBytes {
            get {
                return SR.GetResourceString("Argument_EncodingConversionOverflowBytes");
            }
        }

        // Token: 0x1700048C RID: 1164
        // (get) Token: 0x06002276 RID: 8822 RVA: 0x009EA400 File Offset: 0x009EA400
        internal static string Argument_EncodingConversionOverflowChars {
            get {
                return SR.GetResourceString("Argument_EncodingConversionOverflowChars");
            }
        }

        // Token: 0x1700048D RID: 1165
        // (get) Token: 0x06002277 RID: 8823 RVA: 0x009EA40C File Offset: 0x009EA40C
        internal static string Argument_EncodingNotSupported {
            get {
                return SR.GetResourceString("Argument_EncodingNotSupported");
            }
        }

        // Token: 0x1700048E RID: 1166
        // (get) Token: 0x06002278 RID: 8824 RVA: 0x009EA418 File Offset: 0x009EA418
        internal static string Argument_EnumTypeDoesNotMatch {
            get {
                return SR.GetResourceString("Argument_EnumTypeDoesNotMatch");
            }
        }

        // Token: 0x1700048F RID: 1167
        // (get) Token: 0x06002279 RID: 8825 RVA: 0x009EA424 File Offset: 0x009EA424
        internal static string Argument_FallbackBufferNotEmpty {
            get {
                return SR.GetResourceString("Argument_FallbackBufferNotEmpty");
            }
        }

        // Token: 0x17000490 RID: 1168
        // (get) Token: 0x0600227A RID: 8826 RVA: 0x009EA430 File Offset: 0x009EA430
        internal static string Argument_FieldDeclaringTypeGeneric {
            get {
                return SR.GetResourceString("Argument_FieldDeclaringTypeGeneric");
            }
        }

        // Token: 0x17000491 RID: 1169
        // (get) Token: 0x0600227B RID: 8827 RVA: 0x009EA43C File Offset: 0x009EA43C
        internal static string Argument_FieldNeedGenericDeclaringType {
            get {
                return SR.GetResourceString("Argument_FieldNeedGenericDeclaringType");
            }
        }

        // Token: 0x17000492 RID: 1170
        // (get) Token: 0x0600227C RID: 8828 RVA: 0x009EA448 File Offset: 0x009EA448
        internal static string Argument_GenConstraintViolation {
            get {
                return SR.GetResourceString("Argument_GenConstraintViolation");
            }
        }

        // Token: 0x17000493 RID: 1171
        // (get) Token: 0x0600227D RID: 8829 RVA: 0x009EA454 File Offset: 0x009EA454
        internal static string Argument_GenericArgsCount {
            get {
                return SR.GetResourceString("Argument_GenericArgsCount");
            }
        }

        // Token: 0x17000494 RID: 1172
        // (get) Token: 0x0600227E RID: 8830 RVA: 0x009EA460 File Offset: 0x009EA460
        internal static string Argument_GenericsInvalid {
            get {
                return SR.GetResourceString("Argument_GenericsInvalid");
            }
        }

        // Token: 0x17000495 RID: 1173
        // (get) Token: 0x0600227F RID: 8831 RVA: 0x009EA46C File Offset: 0x009EA46C
        internal static string Argument_GlobalMembersMustBeStatic {
            get {
                return SR.GetResourceString("Argument_GlobalMembersMustBeStatic");
            }
        }

        // Token: 0x17000496 RID: 1174
        // (get) Token: 0x06002280 RID: 8832 RVA: 0x009EA478 File Offset: 0x009EA478
        internal static string Argument_HasToBeArrayClass {
            get {
                return SR.GetResourceString("Argument_HasToBeArrayClass");
            }
        }

        // Token: 0x17000497 RID: 1175
        // (get) Token: 0x06002281 RID: 8833 RVA: 0x009EA484 File Offset: 0x009EA484
        internal static string Argument_IdnBadBidi {
            get {
                return SR.GetResourceString("Argument_IdnBadBidi");
            }
        }

        // Token: 0x17000498 RID: 1176
        // (get) Token: 0x06002282 RID: 8834 RVA: 0x009EA490 File Offset: 0x009EA490
        internal static string Argument_IdnBadLabelSize {
            get {
                return SR.GetResourceString("Argument_IdnBadLabelSize");
            }
        }

        // Token: 0x17000499 RID: 1177
        // (get) Token: 0x06002283 RID: 8835 RVA: 0x009EA49C File Offset: 0x009EA49C
        internal static string Argument_IdnBadNameSize {
            get {
                return SR.GetResourceString("Argument_IdnBadNameSize");
            }
        }

        // Token: 0x1700049A RID: 1178
        // (get) Token: 0x06002284 RID: 8836 RVA: 0x009EA4A8 File Offset: 0x009EA4A8
        internal static string Argument_IdnBadPunycode {
            get {
                return SR.GetResourceString("Argument_IdnBadPunycode");
            }
        }

        // Token: 0x1700049B RID: 1179
        // (get) Token: 0x06002285 RID: 8837 RVA: 0x009EA4B4 File Offset: 0x009EA4B4
        internal static string Argument_IdnBadStd3 {
            get {
                return SR.GetResourceString("Argument_IdnBadStd3");
            }
        }

        // Token: 0x1700049C RID: 1180
        // (get) Token: 0x06002286 RID: 8838 RVA: 0x009EA4C0 File Offset: 0x009EA4C0
        internal static string Argument_IdnIllegalName {
            get {
                return SR.GetResourceString("Argument_IdnIllegalName");
            }
        }

        // Token: 0x1700049D RID: 1181
        // (get) Token: 0x06002287 RID: 8839 RVA: 0x009EA4CC File Offset: 0x009EA4CC
        internal static string Argument_IllegalEnvVarName {
            get {
                return SR.GetResourceString("Argument_IllegalEnvVarName");
            }
        }

        // Token: 0x1700049E RID: 1182
        // (get) Token: 0x06002288 RID: 8840 RVA: 0x009EA4D8 File Offset: 0x009EA4D8
        internal static string Argument_IllegalName {
            get {
                return SR.GetResourceString("Argument_IllegalName");
            }
        }

        // Token: 0x1700049F RID: 1183
        // (get) Token: 0x06002289 RID: 8841 RVA: 0x009EA4E4 File Offset: 0x009EA4E4
        internal static string Argument_ImplementIComparable {
            get {
                return SR.GetResourceString("Argument_ImplementIComparable");
            }
        }

        // Token: 0x170004A0 RID: 1184
        // (get) Token: 0x0600228A RID: 8842 RVA: 0x009EA4F0 File Offset: 0x009EA4F0
        internal static string Argument_InvalidAppendMode {
            get {
                return SR.GetResourceString("Argument_InvalidAppendMode");
            }
        }

        // Token: 0x170004A1 RID: 1185
        // (get) Token: 0x0600228B RID: 8843 RVA: 0x009EA4FC File Offset: 0x009EA4FC
        internal static string Argument_InvalidPreallocateAccess {
            get {
                return SR.GetResourceString("Argument_InvalidPreallocateAccess");
            }
        }

        // Token: 0x170004A2 RID: 1186
        // (get) Token: 0x0600228C RID: 8844 RVA: 0x009EA508 File Offset: 0x009EA508
        internal static string Argument_InvalidPreallocateMode {
            get {
                return SR.GetResourceString("Argument_InvalidPreallocateMode");
            }
        }

        // Token: 0x170004A3 RID: 1187
        // (get) Token: 0x0600228D RID: 8845 RVA: 0x009EA514 File Offset: 0x009EA514
        internal static string Argument_InvalidUnixCreateMode {
            get {
                return SR.GetResourceString("Argument_InvalidUnixCreateMode");
            }
        }

        // Token: 0x170004A4 RID: 1188
        // (get) Token: 0x0600228E RID: 8846 RVA: 0x009EA520 File Offset: 0x009EA520
        internal static string Argument_InvalidArgumentForComparison {
            get {
                return SR.GetResourceString("Argument_InvalidArgumentForComparison");
            }
        }

        // Token: 0x170004A5 RID: 1189
        // (get) Token: 0x0600228F RID: 8847 RVA: 0x009EA52C File Offset: 0x009EA52C
        internal static string Argument_InvalidArrayLength {
            get {
                return SR.GetResourceString("Argument_InvalidArrayLength");
            }
        }

        // Token: 0x170004A6 RID: 1190
        // (get) Token: 0x06002290 RID: 8848 RVA: 0x009EA538 File Offset: 0x009EA538
        internal static string Argument_IncompatibleArrayType {
            get {
                return SR.GetResourceString("Argument_IncompatibleArrayType");
            }
        }

        // Token: 0x170004A7 RID: 1191
        // (get) Token: 0x06002291 RID: 8849 RVA: 0x009EA544 File Offset: 0x009EA544
        internal static string InvalidAssemblyName {
            get {
                return SR.GetResourceString("InvalidAssemblyName");
            }
        }

        // Token: 0x170004A8 RID: 1192
        // (get) Token: 0x06002292 RID: 8850 RVA: 0x009EA550 File Offset: 0x009EA550
        internal static string Argument_InvalidCalendar {
            get {
                return SR.GetResourceString("Argument_InvalidCalendar");
            }
        }

        // Token: 0x170004A9 RID: 1193
        // (get) Token: 0x06002293 RID: 8851 RVA: 0x009EA55C File Offset: 0x009EA55C
        internal static string Argument_InvalidCharSequence {
            get {
                return SR.GetResourceString("Argument_InvalidCharSequence");
            }
        }

        // Token: 0x170004AA RID: 1194
        // (get) Token: 0x06002294 RID: 8852 RVA: 0x009EA568 File Offset: 0x009EA568
        internal static string Argument_InvalidCharSequenceNoIndex {
            get {
                return SR.GetResourceString("Argument_InvalidCharSequenceNoIndex");
            }
        }

        // Token: 0x170004AB RID: 1195
        // (get) Token: 0x06002295 RID: 8853 RVA: 0x009EA574 File Offset: 0x009EA574
        internal static string Argument_InvalidCodePageBytesIndex {
            get {
                return SR.GetResourceString("Argument_InvalidCodePageBytesIndex");
            }
        }

        // Token: 0x170004AC RID: 1196
        // (get) Token: 0x06002296 RID: 8854 RVA: 0x009EA580 File Offset: 0x009EA580
        internal static string Argument_InvalidCodePageConversionIndex {
            get {
                return SR.GetResourceString("Argument_InvalidCodePageConversionIndex");
            }
        }

        // Token: 0x170004AD RID: 1197
        // (get) Token: 0x06002297 RID: 8855 RVA: 0x009EA58C File Offset: 0x009EA58C
        internal static string Argument_InvalidConstructorDeclaringType {
            get {
                return SR.GetResourceString("Argument_InvalidConstructorDeclaringType");
            }
        }

        // Token: 0x170004AE RID: 1198
        // (get) Token: 0x06002298 RID: 8856 RVA: 0x009EA598 File Offset: 0x009EA598
        internal static string Argument_InvalidConstructorInfo {
            get {
                return SR.GetResourceString("Argument_InvalidConstructorInfo");
            }
        }

        // Token: 0x170004AF RID: 1199
        // (get) Token: 0x06002299 RID: 8857 RVA: 0x009EA5A4 File Offset: 0x009EA5A4
        internal static string Argument_InvalidCultureName {
            get {
                return SR.GetResourceString("Argument_InvalidCultureName");
            }
        }

        // Token: 0x170004B0 RID: 1200
        // (get) Token: 0x0600229A RID: 8858 RVA: 0x009EA5B0 File Offset: 0x009EA5B0
        internal static string Argument_InvalidPredefinedCultureName {
            get {
                return SR.GetResourceString("Argument_InvalidPredefinedCultureName");
            }
        }

        // Token: 0x170004B1 RID: 1201
        // (get) Token: 0x0600229B RID: 8859 RVA: 0x009EA5BC File Offset: 0x009EA5BC
        internal static string Argument_InvalidDateTimeKind {
            get {
                return SR.GetResourceString("Argument_InvalidDateTimeKind");
            }
        }

        // Token: 0x170004B2 RID: 1202
        // (get) Token: 0x0600229C RID: 8860 RVA: 0x009EA5C8 File Offset: 0x009EA5C8
        internal static string Argument_InvalidDateTimeStyles {
            get {
                return SR.GetResourceString("Argument_InvalidDateTimeStyles");
            }
        }

        // Token: 0x170004B3 RID: 1203
        // (get) Token: 0x0600229D RID: 8861 RVA: 0x009EA5D4 File Offset: 0x009EA5D4
        internal static string Argument_InvalidDateStyles {
            get {
                return SR.GetResourceString("Argument_InvalidDateStyles");
            }
        }

        // Token: 0x170004B4 RID: 1204
        // (get) Token: 0x0600229E RID: 8862 RVA: 0x009EA5E0 File Offset: 0x009EA5E0
        internal static string Argument_InvalidDigitSubstitution {
            get {
                return SR.GetResourceString("Argument_InvalidDigitSubstitution");
            }
        }

        // Token: 0x170004B5 RID: 1205
        // (get) Token: 0x0600229F RID: 8863 RVA: 0x009EA5EC File Offset: 0x009EA5EC
        internal static string Argument_InvalidElementName {
            get {
                return SR.GetResourceString("Argument_InvalidElementName");
            }
        }

        // Token: 0x170004B6 RID: 1206
        // (get) Token: 0x060022A0 RID: 8864 RVA: 0x009EA5F8 File Offset: 0x009EA5F8
        internal static string Argument_InvalidElementTag {
            get {
                return SR.GetResourceString("Argument_InvalidElementTag");
            }
        }

        // Token: 0x170004B7 RID: 1207
        // (get) Token: 0x060022A1 RID: 8865 RVA: 0x009EA604 File Offset: 0x009EA604
        internal static string Argument_InvalidElementText {
            get {
                return SR.GetResourceString("Argument_InvalidElementText");
            }
        }

        // Token: 0x170004B8 RID: 1208
        // (get) Token: 0x060022A2 RID: 8866 RVA: 0x009EA610 File Offset: 0x009EA610
        internal static string Argument_InvalidElementValue {
            get {
                return SR.GetResourceString("Argument_InvalidElementValue");
            }
        }

        // Token: 0x170004B9 RID: 1209
        // (get) Token: 0x060022A3 RID: 8867 RVA: 0x009EA61C File Offset: 0x009EA61C
        internal static string Argument_InvalidEnum {
            get {
                return SR.GetResourceString("Argument_InvalidEnum");
            }
        }

        // Token: 0x170004BA RID: 1210
        // (get) Token: 0x060022A4 RID: 8868 RVA: 0x009EA628 File Offset: 0x009EA628
        internal static string Argument_InvalidEnumValue {
            get {
                return SR.GetResourceString("Argument_InvalidEnumValue");
            }
        }

        // Token: 0x170004BB RID: 1211
        // (get) Token: 0x060022A5 RID: 8869 RVA: 0x009EA634 File Offset: 0x009EA634
        internal static string Argument_InvalidFieldDeclaringType {
            get {
                return SR.GetResourceString("Argument_InvalidFieldDeclaringType");
            }
        }

        // Token: 0x170004BC RID: 1212
        // (get) Token: 0x060022A6 RID: 8870 RVA: 0x009EA640 File Offset: 0x009EA640
        internal static string Argument_InvalidFileModeAndAccessCombo {
            get {
                return SR.GetResourceString("Argument_InvalidFileModeAndAccessCombo");
            }
        }

        // Token: 0x170004BD RID: 1213
        // (get) Token: 0x060022A7 RID: 8871 RVA: 0x009EA64C File Offset: 0x009EA64C
        internal static string Argument_InvalidFlag {
            get {
                return SR.GetResourceString("Argument_InvalidFlag");
            }
        }

        // Token: 0x170004BE RID: 1214
        // (get) Token: 0x060022A8 RID: 8872 RVA: 0x009EA658 File Offset: 0x009EA658
        internal static string Argument_InvalidGenericInstArray {
            get {
                return SR.GetResourceString("Argument_InvalidGenericInstArray");
            }
        }

        // Token: 0x170004BF RID: 1215
        // (get) Token: 0x060022A9 RID: 8873 RVA: 0x009EA664 File Offset: 0x009EA664
        internal static string Argument_InvalidGroupSize {
            get {
                return SR.GetResourceString("Argument_InvalidGroupSize");
            }
        }

        // Token: 0x170004C0 RID: 1216
        // (get) Token: 0x060022AA RID: 8874 RVA: 0x009EA670 File Offset: 0x009EA670
        internal static string Argument_InvalidHandle {
            get {
                return SR.GetResourceString("Argument_InvalidHandle");
            }
        }

        // Token: 0x170004C1 RID: 1217
        // (get) Token: 0x060022AB RID: 8875 RVA: 0x009EA67C File Offset: 0x009EA67C
        internal static string Argument_InvalidHighSurrogate {
            get {
                return SR.GetResourceString("Argument_InvalidHighSurrogate");
            }
        }

        // Token: 0x170004C2 RID: 1218
        // (get) Token: 0x060022AC RID: 8876 RVA: 0x009EA688 File Offset: 0x009EA688
        internal static string Argument_InvalidId {
            get {
                return SR.GetResourceString("Argument_InvalidId");
            }
        }

        // Token: 0x170004C3 RID: 1219
        // (get) Token: 0x060022AD RID: 8877 RVA: 0x009EA694 File Offset: 0x009EA694
        internal static string Argument_InvalidKindOfTypeForCA {
            get {
                return SR.GetResourceString("Argument_InvalidKindOfTypeForCA");
            }
        }

        // Token: 0x170004C4 RID: 1220
        // (get) Token: 0x060022AE RID: 8878 RVA: 0x009EA6A0 File Offset: 0x009EA6A0
        internal static string Argument_InvalidLabel {
            get {
                return SR.GetResourceString("Argument_InvalidLabel");
            }
        }

        // Token: 0x170004C5 RID: 1221
        // (get) Token: 0x060022AF RID: 8879 RVA: 0x009EA6AC File Offset: 0x009EA6AC
        internal static string Argument_InvalidLowSurrogate {
            get {
                return SR.GetResourceString("Argument_InvalidLowSurrogate");
            }
        }

        // Token: 0x170004C6 RID: 1222
        // (get) Token: 0x060022B0 RID: 8880 RVA: 0x009EA6B8 File Offset: 0x009EA6B8
        internal static string Argument_InvalidMemberForNamedArgument {
            get {
                return SR.GetResourceString("Argument_InvalidMemberForNamedArgument");
            }
        }

        // Token: 0x170004C7 RID: 1223
        // (get) Token: 0x060022B1 RID: 8881 RVA: 0x009EA6C4 File Offset: 0x009EA6C4
        internal static string Argument_InvalidMethodDeclaringType {
            get {
                return SR.GetResourceString("Argument_InvalidMethodDeclaringType");
            }
        }

        // Token: 0x170004C8 RID: 1224
        // (get) Token: 0x060022B2 RID: 8882 RVA: 0x009EA6D0 File Offset: 0x009EA6D0
        internal static string Argument_InvalidName {
            get {
                return SR.GetResourceString("Argument_InvalidName");
            }
        }

        // Token: 0x170004C9 RID: 1225
        // (get) Token: 0x060022B3 RID: 8883 RVA: 0x009EA6DC File Offset: 0x009EA6DC
        internal static string Argument_InvalidNativeDigitCount {
            get {
                return SR.GetResourceString("Argument_InvalidNativeDigitCount");
            }
        }

        // Token: 0x170004CA RID: 1226
        // (get) Token: 0x060022B4 RID: 8884 RVA: 0x009EA6E8 File Offset: 0x009EA6E8
        internal static string Argument_InvalidNativeDigitValue {
            get {
                return SR.GetResourceString("Argument_InvalidNativeDigitValue");
            }
        }

        // Token: 0x170004CB RID: 1227
        // (get) Token: 0x060022B5 RID: 8885 RVA: 0x009EA6F4 File Offset: 0x009EA6F4
        internal static string Argument_InvalidNeutralRegionName {
            get {
                return SR.GetResourceString("Argument_InvalidNeutralRegionName");
            }
        }

        // Token: 0x170004CC RID: 1228
        // (get) Token: 0x060022B6 RID: 8886 RVA: 0x009EA700 File Offset: 0x009EA700
        internal static string Argument_InvalidNormalizationForm {
            get {
                return SR.GetResourceString("Argument_InvalidNormalizationForm");
            }
        }

        // Token: 0x170004CD RID: 1229
        // (get) Token: 0x060022B7 RID: 8887 RVA: 0x009EA70C File Offset: 0x009EA70C
        internal static string Argument_InvalidNumberStyles {
            get {
                return SR.GetResourceString("Argument_InvalidNumberStyles");
            }
        }

        // Token: 0x170004CE RID: 1230
        // (get) Token: 0x060022B8 RID: 8888 RVA: 0x009EA718 File Offset: 0x009EA718
        internal static string Argument_InvalidOffLen {
            get {
                return SR.GetResourceString("Argument_InvalidOffLen");
            }
        }

        // Token: 0x170004CF RID: 1231
        // (get) Token: 0x060022B9 RID: 8889 RVA: 0x009EA724 File Offset: 0x009EA724
        internal static string Argument_InvalidOpCodeOnDynamicMethod {
            get {
                return SR.GetResourceString("Argument_InvalidOpCodeOnDynamicMethod");
            }
        }

        // Token: 0x170004D0 RID: 1232
        // (get) Token: 0x060022BA RID: 8890 RVA: 0x009EA730 File Offset: 0x009EA730
        internal static string Argument_InvalidParameterInfo {
            get {
                return SR.GetResourceString("Argument_InvalidParameterInfo");
            }
        }

        // Token: 0x170004D1 RID: 1233
        // (get) Token: 0x060022BB RID: 8891 RVA: 0x009EA73C File Offset: 0x009EA73C
        internal static string Argument_InvalidParamInfo {
            get {
                return SR.GetResourceString("Argument_InvalidParamInfo");
            }
        }

        // Token: 0x170004D2 RID: 1234
        // (get) Token: 0x060022BC RID: 8892 RVA: 0x009EA748 File Offset: 0x009EA748
        internal static string Argument_NullCharInPath {
            get {
                return SR.GetResourceString("Argument_NullCharInPath");
            }
        }

        // Token: 0x170004D3 RID: 1235
        // (get) Token: 0x060022BD RID: 8893 RVA: 0x009EA754 File Offset: 0x009EA754
        internal static string Argument_InvalidResourceCultureName {
            get {
                return SR.GetResourceString("Argument_InvalidResourceCultureName");
            }
        }

        // Token: 0x170004D4 RID: 1236
        // (get) Token: 0x060022BE RID: 8894 RVA: 0x009EA760 File Offset: 0x009EA760
        internal static string Argument_InvalidSafeBufferOffLen {
            get {
                return SR.GetResourceString("Argument_InvalidSafeBufferOffLen");
            }
        }

        // Token: 0x170004D5 RID: 1237
        // (get) Token: 0x060022BF RID: 8895 RVA: 0x009EA76C File Offset: 0x009EA76C
        internal static string Argument_InvalidSeekOrigin {
            get {
                return SR.GetResourceString("Argument_InvalidSeekOrigin");
            }
        }

        // Token: 0x170004D6 RID: 1238
        // (get) Token: 0x060022C0 RID: 8896 RVA: 0x009EA778 File Offset: 0x009EA778
        internal static string Argument_InvalidSerializedString {
            get {
                return SR.GetResourceString("Argument_InvalidSerializedString");
            }
        }

        // Token: 0x170004D7 RID: 1239
        // (get) Token: 0x060022C1 RID: 8897 RVA: 0x009EA784 File Offset: 0x009EA784
        internal static string Argument_InvalidStartupHookSignature {
            get {
                return SR.GetResourceString("Argument_InvalidStartupHookSignature");
            }
        }

        // Token: 0x170004D8 RID: 1240
        // (get) Token: 0x060022C2 RID: 8898 RVA: 0x009EA790 File Offset: 0x009EA790
        internal static string Argument_InvalidTimeSpanStyles {
            get {
                return SR.GetResourceString("Argument_InvalidTimeSpanStyles");
            }
        }

        // Token: 0x170004D9 RID: 1241
        // (get) Token: 0x060022C3 RID: 8899 RVA: 0x009EA79C File Offset: 0x009EA79C
        internal static string Argument_InvalidToken {
            get {
                return SR.GetResourceString("Argument_InvalidToken");
            }
        }

        // Token: 0x170004DA RID: 1242
        // (get) Token: 0x060022C4 RID: 8900 RVA: 0x009EA7A8 File Offset: 0x009EA7A8
        internal static string Argument_InvalidTypeForCA {
            get {
                return SR.GetResourceString("Argument_InvalidTypeForCA");
            }
        }

        // Token: 0x170004DB RID: 1243
        // (get) Token: 0x060022C5 RID: 8901 RVA: 0x009EA7B4 File Offset: 0x009EA7B4
        internal static string Argument_InvalidTypeForDynamicMethod {
            get {
                return SR.GetResourceString("Argument_InvalidTypeForDynamicMethod");
            }
        }

        // Token: 0x170004DC RID: 1244
        // (get) Token: 0x060022C6 RID: 8902 RVA: 0x009EA7C0 File Offset: 0x009EA7C0
        internal static string Argument_InvalidTypeName {
            get {
                return SR.GetResourceString("Argument_InvalidTypeName");
            }
        }

        // Token: 0x170004DD RID: 1245
        // (get) Token: 0x060022C7 RID: 8903 RVA: 0x009EA7CC File Offset: 0x009EA7CC
        internal static string Argument_TypeContainsReferences {
            get {
                return SR.GetResourceString("Argument_TypeContainsReferences");
            }
        }

        // Token: 0x170004DE RID: 1246
        // (get) Token: 0x060022C8 RID: 8904 RVA: 0x009EA7D8 File Offset: 0x009EA7D8
        internal static string Argument_InvalidUnity {
            get {
                return SR.GetResourceString("Argument_InvalidUnity");
            }
        }

        // Token: 0x170004DF RID: 1247
        // (get) Token: 0x060022C9 RID: 8905 RVA: 0x009EA7E4 File Offset: 0x009EA7E4
        internal static string Argument_LargeInteger {
            get {
                return SR.GetResourceString("Argument_LargeInteger");
            }
        }

        // Token: 0x170004E0 RID: 1248
        // (get) Token: 0x060022CA RID: 8906 RVA: 0x009EA7F0 File Offset: 0x009EA7F0
        internal static string Argument_LongEnvVarValue {
            get {
                return SR.GetResourceString("Argument_LongEnvVarValue");
            }
        }

        // Token: 0x170004E1 RID: 1249
        // (get) Token: 0x060022CB RID: 8907 RVA: 0x009EA7FC File Offset: 0x009EA7FC
        internal static string Argument_MethodDeclaringTypeGeneric {
            get {
                return SR.GetResourceString("Argument_MethodDeclaringTypeGeneric");
            }
        }

        // Token: 0x170004E2 RID: 1250
        // (get) Token: 0x060022CC RID: 8908 RVA: 0x009EA808 File Offset: 0x009EA808
        internal static string Argument_MethodDeclaringTypeGenericLcg {
            get {
                return SR.GetResourceString("Argument_MethodDeclaringTypeGenericLcg");
            }
        }

        // Token: 0x170004E3 RID: 1251
        // (get) Token: 0x060022CD RID: 8909 RVA: 0x009EA814 File Offset: 0x009EA814
        internal static string Argument_MethodNeedGenericDeclaringType {
            get {
                return SR.GetResourceString("Argument_MethodNeedGenericDeclaringType");
            }
        }

        // Token: 0x170004E4 RID: 1252
        // (get) Token: 0x060022CE RID: 8910 RVA: 0x009EA820 File Offset: 0x009EA820
        internal static string Argument_MinMaxValue {
            get {
                return SR.GetResourceString("Argument_MinMaxValue");
            }
        }

        // Token: 0x170004E5 RID: 1253
        // (get) Token: 0x060022CF RID: 8911 RVA: 0x009EA82C File Offset: 0x009EA82C
        internal static string Argument_MismatchedArrays {
            get {
                return SR.GetResourceString("Argument_MismatchedArrays");
            }
        }

        // Token: 0x170004E6 RID: 1254
        // (get) Token: 0x060022D0 RID: 8912 RVA: 0x009EA838 File Offset: 0x009EA838
        internal static string Argument_MustBeFalse {
            get {
                return SR.GetResourceString("Argument_MustBeFalse");
            }
        }

        // Token: 0x170004E7 RID: 1255
        // (get) Token: 0x060022D1 RID: 8913 RVA: 0x009EA844 File Offset: 0x009EA844
        internal static string Argument_MustBeRuntimeAssembly {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeAssembly");
            }
        }

        // Token: 0x170004E8 RID: 1256
        // (get) Token: 0x060022D2 RID: 8914 RVA: 0x009EA850 File Offset: 0x009EA850
        internal static string Argument_MustBeRuntimeFieldInfo {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeFieldInfo");
            }
        }

        // Token: 0x170004E9 RID: 1257
        // (get) Token: 0x060022D3 RID: 8915 RVA: 0x009EA85C File Offset: 0x009EA85C
        internal static string Argument_MustBeRuntimeConstructorInfo {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeConstructorInfo");
            }
        }

        // Token: 0x170004EA RID: 1258
        // (get) Token: 0x060022D4 RID: 8916 RVA: 0x009EA868 File Offset: 0x009EA868
        internal static string Argument_MustBeRuntimeMethodInfo {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeMethodInfo");
            }
        }

        // Token: 0x170004EB RID: 1259
        // (get) Token: 0x060022D5 RID: 8917 RVA: 0x009EA874 File Offset: 0x009EA874
        internal static string Argument_MustBeRuntimeMethod {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeMethod");
            }
        }

        // Token: 0x170004EC RID: 1260
        // (get) Token: 0x060022D6 RID: 8918 RVA: 0x009EA880 File Offset: 0x009EA880
        internal static string Argument_MustBeRuntimeType {
            get {
                return SR.GetResourceString("Argument_MustBeRuntimeType");
            }
        }

        // Token: 0x170004ED RID: 1261
        // (get) Token: 0x060022D7 RID: 8919 RVA: 0x009EA88C File Offset: 0x009EA88C
        internal static string Argument_MustBeTypeBuilder {
            get {
                return SR.GetResourceString("Argument_MustBeTypeBuilder");
            }
        }

        // Token: 0x170004EE RID: 1262
        // (get) Token: 0x060022D8 RID: 8920 RVA: 0x009EA898 File Offset: 0x009EA898
        internal static string Argument_MustHaveAttributeBaseClass {
            get {
                return SR.GetResourceString("Argument_MustHaveAttributeBaseClass");
            }
        }

        // Token: 0x170004EF RID: 1263
        // (get) Token: 0x060022D9 RID: 8921 RVA: 0x009EA8A4 File Offset: 0x009EA8A4
        internal static string Argument_MustHaveLayoutOrBeBlittable {
            get {
                return SR.GetResourceString("Argument_MustHaveLayoutOrBeBlittable");
            }
        }

        // Token: 0x170004F0 RID: 1264
        // (get) Token: 0x060022DA RID: 8922 RVA: 0x009EA8B0 File Offset: 0x009EA8B0
        internal static string Argument_NativeOverlappedAlreadyFree {
            get {
                return SR.GetResourceString("Argument_NativeOverlappedAlreadyFree");
            }
        }

        // Token: 0x170004F1 RID: 1265
        // (get) Token: 0x060022DB RID: 8923 RVA: 0x009EA8BC File Offset: 0x009EA8BC
        internal static string Argument_NativeOverlappedWrongBoundHandle {
            get {
                return SR.GetResourceString("Argument_NativeOverlappedWrongBoundHandle");
            }
        }

        // Token: 0x170004F2 RID: 1266
        // (get) Token: 0x060022DC RID: 8924 RVA: 0x009EA8C8 File Offset: 0x009EA8C8
        internal static string Argument_NeedGenericMethodDefinition {
            get {
                return SR.GetResourceString("Argument_NeedGenericMethodDefinition");
            }
        }

        // Token: 0x170004F3 RID: 1267
        // (get) Token: 0x060022DD RID: 8925 RVA: 0x009EA8D4 File Offset: 0x009EA8D4
        internal static string Argument_NeedNonGenericObject {
            get {
                return SR.GetResourceString("Argument_NeedNonGenericObject");
            }
        }

        // Token: 0x170004F4 RID: 1268
        // (get) Token: 0x060022DE RID: 8926 RVA: 0x009EA8E0 File Offset: 0x009EA8E0
        internal static string Argument_NeedNonGenericType {
            get {
                return SR.GetResourceString("Argument_NeedNonGenericType");
            }
        }

        // Token: 0x170004F5 RID: 1269
        // (get) Token: 0x060022DF RID: 8927 RVA: 0x009EA8EC File Offset: 0x009EA8EC
        internal static string Argument_NeverValidGenericArgument {
            get {
                return SR.GetResourceString("Argument_NeverValidGenericArgument");
            }
        }

        // Token: 0x170004F6 RID: 1270
        // (get) Token: 0x060022E0 RID: 8928 RVA: 0x009EA8F8 File Offset: 0x009EA8F8
        internal static string Argument_NoEra {
            get {
                return SR.GetResourceString("Argument_NoEra");
            }
        }

        // Token: 0x170004F7 RID: 1271
        // (get) Token: 0x060022E1 RID: 8929 RVA: 0x009EA904 File Offset: 0x009EA904
        internal static string Argument_NoRegionInvariantCulture {
            get {
                return SR.GetResourceString("Argument_NoRegionInvariantCulture");
            }
        }

        // Token: 0x170004F8 RID: 1272
        // (get) Token: 0x060022E2 RID: 8930 RVA: 0x009EA910 File Offset: 0x009EA910
        internal static string Argument_NotAWritableProperty {
            get {
                return SR.GetResourceString("Argument_NotAWritableProperty");
            }
        }

        // Token: 0x170004F9 RID: 1273
        // (get) Token: 0x060022E3 RID: 8931 RVA: 0x009EA91C File Offset: 0x009EA91C
        internal static string Argument_NotEnoughBytesToRead {
            get {
                return SR.GetResourceString("Argument_NotEnoughBytesToRead");
            }
        }

        // Token: 0x170004FA RID: 1274
        // (get) Token: 0x060022E4 RID: 8932 RVA: 0x009EA928 File Offset: 0x009EA928
        internal static string Argument_NotEnoughBytesToWrite {
            get {
                return SR.GetResourceString("Argument_NotEnoughBytesToWrite");
            }
        }

        // Token: 0x170004FB RID: 1275
        // (get) Token: 0x060022E5 RID: 8933 RVA: 0x009EA934 File Offset: 0x009EA934
        internal static string Argument_NotEnoughGenArguments {
            get {
                return SR.GetResourceString("Argument_NotEnoughGenArguments");
            }
        }

        // Token: 0x170004FC RID: 1276
        // (get) Token: 0x060022E6 RID: 8934 RVA: 0x009EA940 File Offset: 0x009EA940
        internal static string Argument_NotExceptionType {
            get {
                return SR.GetResourceString("Argument_NotExceptionType");
            }
        }

        // Token: 0x170004FD RID: 1277
        // (get) Token: 0x060022E7 RID: 8935 RVA: 0x009EA94C File Offset: 0x009EA94C
        internal static string Argument_NotInExceptionBlock {
            get {
                return SR.GetResourceString("Argument_NotInExceptionBlock");
            }
        }

        // Token: 0x170004FE RID: 1278
        // (get) Token: 0x060022E8 RID: 8936 RVA: 0x009EA958 File Offset: 0x009EA958
        internal static string Argument_NotMethodCallOpcode {
            get {
                return SR.GetResourceString("Argument_NotMethodCallOpcode");
            }
        }

        // Token: 0x170004FF RID: 1279
        // (get) Token: 0x060022E9 RID: 8937 RVA: 0x009EA964 File Offset: 0x009EA964
        internal static string Argument_ObjNotComObject {
            get {
                return SR.GetResourceString("Argument_ObjNotComObject");
            }
        }

        // Token: 0x17000500 RID: 1280
        // (get) Token: 0x060022EA RID: 8938 RVA: 0x009EA970 File Offset: 0x009EA970
        internal static string Argument_OffsetAndCapacityOutOfBounds {
            get {
                return SR.GetResourceString("Argument_OffsetAndCapacityOutOfBounds");
            }
        }

        // Token: 0x17000501 RID: 1281
        // (get) Token: 0x060022EB RID: 8939 RVA: 0x009EA97C File Offset: 0x009EA97C
        internal static string Argument_OffsetLocalMismatch {
            get {
                return SR.GetResourceString("Argument_OffsetLocalMismatch");
            }
        }

        // Token: 0x17000502 RID: 1282
        // (get) Token: 0x060022EC RID: 8940 RVA: 0x009EA988 File Offset: 0x009EA988
        internal static string Argument_OffsetOfFieldNotFound {
            get {
                return SR.GetResourceString("Argument_OffsetOfFieldNotFound");
            }
        }

        // Token: 0x17000503 RID: 1283
        // (get) Token: 0x060022ED RID: 8941 RVA: 0x009EA994 File Offset: 0x009EA994
        internal static string Argument_OffsetOutOfRange {
            get {
                return SR.GetResourceString("Argument_OffsetOutOfRange");
            }
        }

        // Token: 0x17000504 RID: 1284
        // (get) Token: 0x060022EE RID: 8942 RVA: 0x009EA9A0 File Offset: 0x009EA9A0
        internal static string Argument_OffsetPrecision {
            get {
                return SR.GetResourceString("Argument_OffsetPrecision");
            }
        }

        // Token: 0x17000505 RID: 1285
        // (get) Token: 0x060022EF RID: 8943 RVA: 0x009EA9AC File Offset: 0x009EA9AC
        internal static string Argument_OffsetUtcMismatch {
            get {
                return SR.GetResourceString("Argument_OffsetUtcMismatch");
            }
        }

        // Token: 0x17000506 RID: 1286
        // (get) Token: 0x060022F0 RID: 8944 RVA: 0x009EA9B8 File Offset: 0x009EA9B8
        internal static string Argument_OneOfCulturesNotSupported {
            get {
                return SR.GetResourceString("Argument_OneOfCulturesNotSupported");
            }
        }

        // Token: 0x17000507 RID: 1287
        // (get) Token: 0x060022F1 RID: 8945 RVA: 0x009EA9C4 File Offset: 0x009EA9C4
        internal static string Argument_OnlyMscorlib {
            get {
                return SR.GetResourceString("Argument_OnlyMscorlib");
            }
        }

        // Token: 0x17000508 RID: 1288
        // (get) Token: 0x060022F2 RID: 8946 RVA: 0x009EA9D0 File Offset: 0x009EA9D0
        internal static string Argument_OutOfOrderDateTimes {
            get {
                return SR.GetResourceString("Argument_OutOfOrderDateTimes");
            }
        }

        // Token: 0x17000509 RID: 1289
        // (get) Token: 0x060022F3 RID: 8947 RVA: 0x009EA9DC File Offset: 0x009EA9DC
        internal static string Argument_PathEmpty {
            get {
                return SR.GetResourceString("Argument_PathEmpty");
            }
        }

        // Token: 0x1700050A RID: 1290
        // (get) Token: 0x060022F4 RID: 8948 RVA: 0x009EA9E8 File Offset: 0x009EA9E8
        internal static string Argument_PreAllocatedAlreadyAllocated {
            get {
                return SR.GetResourceString("Argument_PreAllocatedAlreadyAllocated");
            }
        }

        // Token: 0x1700050B RID: 1291
        // (get) Token: 0x060022F5 RID: 8949 RVA: 0x009EA9F4 File Offset: 0x009EA9F4
        internal static string Argument_RecursiveFallback {
            get {
                return SR.GetResourceString("Argument_RecursiveFallback");
            }
        }

        // Token: 0x1700050C RID: 1292
        // (get) Token: 0x060022F6 RID: 8950 RVA: 0x009EAA00 File Offset: 0x009EAA00
        internal static string Argument_RecursiveFallbackBytes {
            get {
                return SR.GetResourceString("Argument_RecursiveFallbackBytes");
            }
        }

        // Token: 0x1700050D RID: 1293
        // (get) Token: 0x060022F7 RID: 8951 RVA: 0x009EAA0C File Offset: 0x009EAA0C
        internal static string Argument_RedefinedLabel {
            get {
                return SR.GetResourceString("Argument_RedefinedLabel");
            }
        }

        // Token: 0x1700050E RID: 1294
        // (get) Token: 0x060022F8 RID: 8952 RVA: 0x009EAA18 File Offset: 0x009EAA18
        internal static string Argument_ResolveField {
            get {
                return SR.GetResourceString("Argument_ResolveField");
            }
        }

        // Token: 0x1700050F RID: 1295
        // (get) Token: 0x060022F9 RID: 8953 RVA: 0x009EAA24 File Offset: 0x009EAA24
        internal static string Argument_ResolveFieldHandle {
            get {
                return SR.GetResourceString("Argument_ResolveFieldHandle");
            }
        }

        // Token: 0x17000510 RID: 1296
        // (get) Token: 0x060022FA RID: 8954 RVA: 0x009EAA30 File Offset: 0x009EAA30
        internal static string Argument_ResolveMember {
            get {
                return SR.GetResourceString("Argument_ResolveMember");
            }
        }

        // Token: 0x17000511 RID: 1297
        // (get) Token: 0x060022FB RID: 8955 RVA: 0x009EAA3C File Offset: 0x009EAA3C
        internal static string Argument_ResolveMethod {
            get {
                return SR.GetResourceString("Argument_ResolveMethod");
            }
        }

        // Token: 0x17000512 RID: 1298
        // (get) Token: 0x060022FC RID: 8956 RVA: 0x009EAA48 File Offset: 0x009EAA48
        internal static string Argument_ResolveMethodHandle {
            get {
                return SR.GetResourceString("Argument_ResolveMethodHandle");
            }
        }

        // Token: 0x17000513 RID: 1299
        // (get) Token: 0x060022FD RID: 8957 RVA: 0x009EAA54 File Offset: 0x009EAA54
        internal static string Argument_ResolveModuleType {
            get {
                return SR.GetResourceString("Argument_ResolveModuleType");
            }
        }

        // Token: 0x17000514 RID: 1300
        // (get) Token: 0x060022FE RID: 8958 RVA: 0x009EAA60 File Offset: 0x009EAA60
        internal static string Argument_ResolveString {
            get {
                return SR.GetResourceString("Argument_ResolveString");
            }
        }

        // Token: 0x17000515 RID: 1301
        // (get) Token: 0x060022FF RID: 8959 RVA: 0x009EAA6C File Offset: 0x009EAA6C
        internal static string Argument_ResolveType {
            get {
                return SR.GetResourceString("Argument_ResolveType");
            }
        }

        // Token: 0x17000516 RID: 1302
        // (get) Token: 0x06002300 RID: 8960 RVA: 0x009EAA78 File Offset: 0x009EAA78
        internal static string Argument_ResultCalendarRange {
            get {
                return SR.GetResourceString("Argument_ResultCalendarRange");
            }
        }

        // Token: 0x17000517 RID: 1303
        // (get) Token: 0x06002301 RID: 8961 RVA: 0x009EAA84 File Offset: 0x009EAA84
        internal static string Argument_SemaphoreInitialMaximum {
            get {
                return SR.GetResourceString("Argument_SemaphoreInitialMaximum");
            }
        }

        // Token: 0x17000518 RID: 1304
        // (get) Token: 0x06002302 RID: 8962 RVA: 0x009EAA90 File Offset: 0x009EAA90
        internal static string Argument_ShouldNotSpecifyExceptionType {
            get {
                return SR.GetResourceString("Argument_ShouldNotSpecifyExceptionType");
            }
        }

        // Token: 0x17000519 RID: 1305
        // (get) Token: 0x06002303 RID: 8963 RVA: 0x009EAA9C File Offset: 0x009EAA9C
        internal static string Argument_ShouldOnlySetVisibilityFlags {
            get {
                return SR.GetResourceString("Argument_ShouldOnlySetVisibilityFlags");
            }
        }

        // Token: 0x1700051A RID: 1306
        // (get) Token: 0x06002304 RID: 8964 RVA: 0x009EAAA8 File Offset: 0x009EAAA8
        internal static string Argument_SigIsFinalized {
            get {
                return SR.GetResourceString("Argument_SigIsFinalized");
            }
        }

        // Token: 0x1700051B RID: 1307
        // (get) Token: 0x06002305 RID: 8965 RVA: 0x009EAAB4 File Offset: 0x009EAAB4
        internal static string Argument_StreamNotReadable {
            get {
                return SR.GetResourceString("Argument_StreamNotReadable");
            }
        }

        // Token: 0x1700051C RID: 1308
        // (get) Token: 0x06002306 RID: 8966 RVA: 0x009EAAC0 File Offset: 0x009EAAC0
        internal static string Argument_StreamNotWritable {
            get {
                return SR.GetResourceString("Argument_StreamNotWritable");
            }
        }

        // Token: 0x1700051D RID: 1309
        // (get) Token: 0x06002307 RID: 8967 RVA: 0x009EAACC File Offset: 0x009EAACC
        internal static string Argument_StringFirstCharIsZero {
            get {
                return SR.GetResourceString("Argument_StringFirstCharIsZero");
            }
        }

        // Token: 0x1700051E RID: 1310
        // (get) Token: 0x06002308 RID: 8968 RVA: 0x009EAAD8 File Offset: 0x009EAAD8
        internal static string Argument_StructMustNotBeValueClass {
            get {
                return SR.GetResourceString("Argument_StructMustNotBeValueClass");
            }
        }

        // Token: 0x1700051F RID: 1311
        // (get) Token: 0x06002309 RID: 8969 RVA: 0x009EAAE4 File Offset: 0x009EAAE4
        internal static string Argument_TimeSpanHasSeconds {
            get {
                return SR.GetResourceString("Argument_TimeSpanHasSeconds");
            }
        }

        // Token: 0x17000520 RID: 1312
        // (get) Token: 0x0600230A RID: 8970 RVA: 0x009EAAF0 File Offset: 0x009EAAF0
        internal static string Argument_ToExclusiveLessThanFromExclusive {
            get {
                return SR.GetResourceString("Argument_ToExclusiveLessThanFromExclusive");
            }
        }

        // Token: 0x17000521 RID: 1313
        // (get) Token: 0x0600230B RID: 8971 RVA: 0x009EAAFC File Offset: 0x009EAAFC
        internal static string Argument_TooManyFinallyClause {
            get {
                return SR.GetResourceString("Argument_TooManyFinallyClause");
            }
        }

        // Token: 0x17000522 RID: 1314
        // (get) Token: 0x0600230C RID: 8972 RVA: 0x009EAB08 File Offset: 0x009EAB08
        internal static string Argument_TransitionTimesAreIdentical {
            get {
                return SR.GetResourceString("Argument_TransitionTimesAreIdentical");
            }
        }

        // Token: 0x17000523 RID: 1315
        // (get) Token: 0x0600230D RID: 8973 RVA: 0x009EAB14 File Offset: 0x009EAB14
        internal static string Argument_TypedReferenceInvalidField {
            get {
                return SR.GetResourceString("Argument_TypedReferenceInvalidField");
            }
        }

        // Token: 0x17000524 RID: 1316
        // (get) Token: 0x0600230E RID: 8974 RVA: 0x009EAB20 File Offset: 0x009EAB20
        internal static string Argument_TypeMustNotBeComImport {
            get {
                return SR.GetResourceString("Argument_TypeMustNotBeComImport");
            }
        }

        // Token: 0x17000525 RID: 1317
        // (get) Token: 0x0600230F RID: 8975 RVA: 0x009EAB2C File Offset: 0x009EAB2C
        internal static string Argument_TypeNameTooLong {
            get {
                return SR.GetResourceString("Argument_TypeNameTooLong");
            }
        }

        // Token: 0x17000526 RID: 1318
        // (get) Token: 0x06002310 RID: 8976 RVA: 0x009EAB38 File Offset: 0x009EAB38
        internal static string Argument_TypeNotComObject {
            get {
                return SR.GetResourceString("Argument_TypeNotComObject");
            }
        }

        // Token: 0x17000527 RID: 1319
        // (get) Token: 0x06002311 RID: 8977 RVA: 0x009EAB44 File Offset: 0x009EAB44
        internal static string Argument_UnclosedExceptionBlock {
            get {
                return SR.GetResourceString("Argument_UnclosedExceptionBlock");
            }
        }

        // Token: 0x17000528 RID: 1320
        // (get) Token: 0x06002312 RID: 8978 RVA: 0x009EAB50 File Offset: 0x009EAB50
        internal static string Argument_UnknownUnmanagedCallConv {
            get {
                return SR.GetResourceString("Argument_UnknownUnmanagedCallConv");
            }
        }

        // Token: 0x17000529 RID: 1321
        // (get) Token: 0x06002313 RID: 8979 RVA: 0x009EAB5C File Offset: 0x009EAB5C
        internal static string Argument_UnmanagedMemAccessorWrapAround {
            get {
                return SR.GetResourceString("Argument_UnmanagedMemAccessorWrapAround");
            }
        }

        // Token: 0x1700052A RID: 1322
        // (get) Token: 0x06002314 RID: 8980 RVA: 0x009EAB68 File Offset: 0x009EAB68
        internal static string Argument_UnmatchedMethodForLocal {
            get {
                return SR.GetResourceString("Argument_UnmatchedMethodForLocal");
            }
        }

        // Token: 0x1700052B RID: 1323
        // (get) Token: 0x06002315 RID: 8981 RVA: 0x009EAB74 File Offset: 0x009EAB74
        internal static string Argument_UnmatchingSymScope {
            get {
                return SR.GetResourceString("Argument_UnmatchingSymScope");
            }
        }

        // Token: 0x1700052C RID: 1324
        // (get) Token: 0x06002316 RID: 8982 RVA: 0x009EAB80 File Offset: 0x009EAB80
        internal static string Argument_UTCOutOfRange {
            get {
                return SR.GetResourceString("Argument_UTCOutOfRange");
            }
        }

        // Token: 0x1700052D RID: 1325
        // (get) Token: 0x06002317 RID: 8983 RVA: 0x009EAB8C File Offset: 0x009EAB8C
        internal static string Argument_WrongSizeArrayInNativeStruct {
            get {
                return SR.GetResourceString("Argument_WrongSizeArrayInNativeStruct");
            }
        }

        // Token: 0x1700052E RID: 1326
        // (get) Token: 0x06002318 RID: 8984 RVA: 0x009EAB98 File Offset: 0x009EAB98
        internal static string ArgumentException_BadMethodImplBody {
            get {
                return SR.GetResourceString("ArgumentException_BadMethodImplBody");
            }
        }

        // Token: 0x1700052F RID: 1327
        // (get) Token: 0x06002319 RID: 8985 RVA: 0x009EABA4 File Offset: 0x009EABA4
        internal static string ArgumentException_BufferNotFromPool {
            get {
                return SR.GetResourceString("ArgumentException_BufferNotFromPool");
            }
        }

        // Token: 0x17000530 RID: 1328
        // (get) Token: 0x0600231A RID: 8986 RVA: 0x009EABB0 File Offset: 0x009EABB0
        internal static string ArgumentException_OtherNotArrayOfCorrectLength {
            get {
                return SR.GetResourceString("ArgumentException_OtherNotArrayOfCorrectLength");
            }
        }

        // Token: 0x17000531 RID: 1329
        // (get) Token: 0x0600231B RID: 8987 RVA: 0x009EABBC File Offset: 0x009EABBC
        internal static string ArgumentException_NotIsomorphic {
            get {
                return SR.GetResourceString("ArgumentException_NotIsomorphic");
            }
        }

        // Token: 0x17000532 RID: 1330
        // (get) Token: 0x0600231C RID: 8988 RVA: 0x009EABC8 File Offset: 0x009EABC8
        internal static string ArgumentException_TupleIncorrectType {
            get {
                return SR.GetResourceString("ArgumentException_TupleIncorrectType");
            }
        }

        // Token: 0x17000533 RID: 1331
        // (get) Token: 0x0600231D RID: 8989 RVA: 0x009EABD4 File Offset: 0x009EABD4
        internal static string ArgumentException_TupleLastArgumentNotATuple {
            get {
                return SR.GetResourceString("ArgumentException_TupleLastArgumentNotATuple");
            }
        }

        // Token: 0x17000534 RID: 1332
        // (get) Token: 0x0600231E RID: 8990 RVA: 0x009EABE0 File Offset: 0x009EABE0
        internal static string ArgumentException_ValueTupleIncorrectType {
            get {
                return SR.GetResourceString("ArgumentException_ValueTupleIncorrectType");
            }
        }

        // Token: 0x17000535 RID: 1333
        // (get) Token: 0x0600231F RID: 8991 RVA: 0x009EABEC File Offset: 0x009EABEC
        internal static string ArgumentException_ValueTupleLastArgumentNotAValueTuple {
            get {
                return SR.GetResourceString("ArgumentException_ValueTupleLastArgumentNotAValueTuple");
            }
        }

        // Token: 0x17000536 RID: 1334
        // (get) Token: 0x06002320 RID: 8992 RVA: 0x009EABF8 File Offset: 0x009EABF8
        internal static string ArgumentNull_Array {
            get {
                return SR.GetResourceString("ArgumentNull_Array");
            }
        }

        // Token: 0x17000537 RID: 1335
        // (get) Token: 0x06002321 RID: 8993 RVA: 0x009EAC04 File Offset: 0x009EAC04
        internal static string ArgumentNull_ArrayElement {
            get {
                return SR.GetResourceString("ArgumentNull_ArrayElement");
            }
        }

        // Token: 0x17000538 RID: 1336
        // (get) Token: 0x06002322 RID: 8994 RVA: 0x009EAC10 File Offset: 0x009EAC10
        internal static string ArgumentNull_ArrayValue {
            get {
                return SR.GetResourceString("ArgumentNull_ArrayValue");
            }
        }

        // Token: 0x17000539 RID: 1337
        // (get) Token: 0x06002323 RID: 8995 RVA: 0x009EAC1C File Offset: 0x009EAC1C
        internal static string ArgumentNull_Child {
            get {
                return SR.GetResourceString("ArgumentNull_Child");
            }
        }

        // Token: 0x1700053A RID: 1338
        // (get) Token: 0x06002324 RID: 8996 RVA: 0x009EAC28 File Offset: 0x009EAC28
        internal static string ArgumentNull_Generic {
            get {
                return SR.GetResourceString("ArgumentNull_Generic");
            }
        }

        // Token: 0x1700053B RID: 1339
        // (get) Token: 0x06002325 RID: 8997 RVA: 0x009EAC34 File Offset: 0x009EAC34
        internal static string ArgumentNull_SafeHandle {
            get {
                return SR.GetResourceString("ArgumentNull_SafeHandle");
            }
        }

        // Token: 0x1700053C RID: 1340
        // (get) Token: 0x06002326 RID: 8998 RVA: 0x009EAC40 File Offset: 0x009EAC40
        internal static string ArgumentNull_String {
            get {
                return SR.GetResourceString("ArgumentNull_String");
            }
        }

        // Token: 0x1700053D RID: 1341
        // (get) Token: 0x06002327 RID: 8999 RVA: 0x009EAC4C File Offset: 0x009EAC4C
        internal static string ArgumentNull_TypedRefType {
            get {
                return SR.GetResourceString("ArgumentNull_TypedRefType");
            }
        }

        // Token: 0x1700053E RID: 1342
        // (get) Token: 0x06002328 RID: 9000 RVA: 0x009EAC58 File Offset: 0x009EAC58
        internal static string ArgumentOutOfRange_ActualValue {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_ActualValue");
            }
        }

        // Token: 0x1700053F RID: 1343
        // (get) Token: 0x06002329 RID: 9001 RVA: 0x009EAC64 File Offset: 0x009EAC64
        internal static string ArgumentOutOfRange_AddValue {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_AddValue");
            }
        }

        // Token: 0x17000540 RID: 1344
        // (get) Token: 0x0600232A RID: 9002 RVA: 0x009EAC70 File Offset: 0x009EAC70
        internal static string ArgumentOutOfRange_BadHourMinuteSecond {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_BadHourMinuteSecond");
            }
        }

        // Token: 0x17000541 RID: 1345
        // (get) Token: 0x0600232B RID: 9003 RVA: 0x009EAC7C File Offset: 0x009EAC7C
        internal static string ArgumentOutOfRange_BadYearMonthDay {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_BadYearMonthDay");
            }
        }

        // Token: 0x17000542 RID: 1346
        // (get) Token: 0x0600232C RID: 9004 RVA: 0x009EAC88 File Offset: 0x009EAC88
        internal static string ArgumentOutOfRange_BiggerThanCollection {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_BiggerThanCollection");
            }
        }

        // Token: 0x17000543 RID: 1347
        // (get) Token: 0x0600232D RID: 9005 RVA: 0x009EAC94 File Offset: 0x009EAC94
        internal static string ArgumentOutOfRange_Bounds_Lower_Upper {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper");
            }
        }

        // Token: 0x17000544 RID: 1348
        // (get) Token: 0x0600232E RID: 9006 RVA: 0x009EACA0 File Offset: 0x009EACA0
        internal static string ArgumentOutOfRange_CalendarRange {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_CalendarRange");
            }
        }

        // Token: 0x17000545 RID: 1349
        // (get) Token: 0x0600232F RID: 9007 RVA: 0x009EACAC File Offset: 0x009EACAC
        internal static string ArgumentOutOfRange_Capacity {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Capacity");
            }
        }

        // Token: 0x17000546 RID: 1350
        // (get) Token: 0x06002330 RID: 9008 RVA: 0x009EACB8 File Offset: 0x009EACB8
        internal static string ArgumentOutOfRange_Count {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Count");
            }
        }

        // Token: 0x17000547 RID: 1351
        // (get) Token: 0x06002331 RID: 9009 RVA: 0x009EACC4 File Offset: 0x009EACC4
        internal static string ArgumentOutOfRange_DateArithmetic {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DateArithmetic");
            }
        }

        // Token: 0x17000548 RID: 1352
        // (get) Token: 0x06002332 RID: 9010 RVA: 0x009EACD0 File Offset: 0x009EACD0
        internal static string ArgumentOutOfRange_DateTimeBadMonths {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DateTimeBadMonths");
            }
        }

        // Token: 0x17000549 RID: 1353
        // (get) Token: 0x06002333 RID: 9011 RVA: 0x009EACDC File Offset: 0x009EACDC
        internal static string ArgumentOutOfRange_DateTimeBadTicks {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DateTimeBadTicks");
            }
        }

        // Token: 0x1700054A RID: 1354
        // (get) Token: 0x06002334 RID: 9012 RVA: 0x009EACE8 File Offset: 0x009EACE8
        internal static string ArgumentOutOfRange_TimeOnlyBadTicks {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_TimeOnlyBadTicks");
            }
        }

        // Token: 0x1700054B RID: 1355
        // (get) Token: 0x06002335 RID: 9013 RVA: 0x009EACF4 File Offset: 0x009EACF4
        internal static string ArgumentOutOfRange_DateTimeBadYears {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DateTimeBadYears");
            }
        }

        // Token: 0x1700054C RID: 1356
        // (get) Token: 0x06002336 RID: 9014 RVA: 0x009EAD00 File Offset: 0x009EAD00
        internal static string ArgumentOutOfRange_Day {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Day");
            }
        }

        // Token: 0x1700054D RID: 1357
        // (get) Token: 0x06002337 RID: 9015 RVA: 0x009EAD0C File Offset: 0x009EAD0C
        internal static string ArgumentOutOfRange_DayOfWeek {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DayOfWeek");
            }
        }

        // Token: 0x1700054E RID: 1358
        // (get) Token: 0x06002338 RID: 9016 RVA: 0x009EAD18 File Offset: 0x009EAD18
        internal static string ArgumentOutOfRange_DayParam {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DayParam");
            }
        }

        // Token: 0x1700054F RID: 1359
        // (get) Token: 0x06002339 RID: 9017 RVA: 0x009EAD24 File Offset: 0x009EAD24
        internal static string ArgumentOutOfRange_DecimalRound {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DecimalRound");
            }
        }

        // Token: 0x17000550 RID: 1360
        // (get) Token: 0x0600233A RID: 9018 RVA: 0x009EAD30 File Offset: 0x009EAD30
        internal static string ArgumentOutOfRange_EndIndexStartIndex {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_EndIndexStartIndex");
            }
        }

        // Token: 0x17000551 RID: 1361
        // (get) Token: 0x0600233B RID: 9019 RVA: 0x009EAD3C File Offset: 0x009EAD3C
        internal static string ArgumentOutOfRange_Enum {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Enum");
            }
        }

        // Token: 0x17000552 RID: 1362
        // (get) Token: 0x0600233C RID: 9020 RVA: 0x009EAD48 File Offset: 0x009EAD48
        internal static string ArgumentOutOfRange_Era {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Era");
            }
        }

        // Token: 0x17000553 RID: 1363
        // (get) Token: 0x0600233D RID: 9021 RVA: 0x009EAD54 File Offset: 0x009EAD54
        internal static string ArgumentOutOfRange_FileLengthTooBig {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_FileLengthTooBig");
            }
        }

        // Token: 0x17000554 RID: 1364
        // (get) Token: 0x0600233E RID: 9022 RVA: 0x009EAD60 File Offset: 0x009EAD60
        internal static string ArgumentOutOfRange_FileTimeInvalid {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_FileTimeInvalid");
            }
        }

        // Token: 0x17000555 RID: 1365
        // (get) Token: 0x0600233F RID: 9023 RVA: 0x009EAD6C File Offset: 0x009EAD6C
        internal static string ArgumentOutOfRange_GetByteCountOverflow {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_GetByteCountOverflow");
            }
        }

        // Token: 0x17000556 RID: 1366
        // (get) Token: 0x06002340 RID: 9024 RVA: 0x009EAD78 File Offset: 0x009EAD78
        internal static string ArgumentOutOfRange_GetCharCountOverflow {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_GetCharCountOverflow");
            }
        }

        // Token: 0x17000557 RID: 1367
        // (get) Token: 0x06002341 RID: 9025 RVA: 0x009EAD84 File Offset: 0x009EAD84
        internal static string ArgumentOutOfRange_HashtableLoadFactor {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_HashtableLoadFactor");
            }
        }

        // Token: 0x17000558 RID: 1368
        // (get) Token: 0x06002342 RID: 9026 RVA: 0x009EAD90 File Offset: 0x009EAD90
        internal static string ArgumentOutOfRange_HugeArrayNotSupported {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported");
            }
        }

        // Token: 0x17000559 RID: 1369
        // (get) Token: 0x06002343 RID: 9027 RVA: 0x009EAD9C File Offset: 0x009EAD9C
        internal static string ArgumentOutOfRange_IndexMustBeLess {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexMustBeLess");
            }
        }

        // Token: 0x1700055A RID: 1370
        // (get) Token: 0x06002344 RID: 9028 RVA: 0x009EADA8 File Offset: 0x009EADA8
        internal static string ArgumentOutOfRange_IndexMustBeLessOrEqual {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexMustBeLessOrEqual");
            }
        }

        // Token: 0x1700055B RID: 1371
        // (get) Token: 0x06002345 RID: 9029 RVA: 0x009EADB4 File Offset: 0x009EADB4
        internal static string ArgumentOutOfRange_IndexCount {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexCount");
            }
        }

        // Token: 0x1700055C RID: 1372
        // (get) Token: 0x06002346 RID: 9030 RVA: 0x009EADC0 File Offset: 0x009EADC0
        internal static string ArgumentOutOfRange_IndexCountBuffer {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexCountBuffer");
            }
        }

        // Token: 0x1700055D RID: 1373
        // (get) Token: 0x06002347 RID: 9031 RVA: 0x009EADCC File Offset: 0x009EADCC
        internal static string ArgumentOutOfRange_IndexLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexLength");
            }
        }

        // Token: 0x1700055E RID: 1374
        // (get) Token: 0x06002348 RID: 9032 RVA: 0x009EADD8 File Offset: 0x009EADD8
        internal static string ArgumentOutOfRange_IndexString {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_IndexString");
            }
        }

        // Token: 0x1700055F RID: 1375
        // (get) Token: 0x06002349 RID: 9033 RVA: 0x009EADE4 File Offset: 0x009EADE4
        internal static string ArgumentOutOfRange_InvalidEraValue {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_InvalidEraValue");
            }
        }

        // Token: 0x17000560 RID: 1376
        // (get) Token: 0x0600234A RID: 9034 RVA: 0x009EADF0 File Offset: 0x009EADF0
        internal static string ArgumentOutOfRange_InvalidHighSurrogate {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_InvalidHighSurrogate");
            }
        }

        // Token: 0x17000561 RID: 1377
        // (get) Token: 0x0600234B RID: 9035 RVA: 0x009EADFC File Offset: 0x009EADFC
        internal static string ArgumentOutOfRange_InvalidLowSurrogate {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_InvalidLowSurrogate");
            }
        }

        // Token: 0x17000562 RID: 1378
        // (get) Token: 0x0600234C RID: 9036 RVA: 0x009EAE08 File Offset: 0x009EAE08
        internal static string ArgumentOutOfRange_InvalidUTF32 {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_InvalidUTF32");
            }
        }

        // Token: 0x17000563 RID: 1379
        // (get) Token: 0x0600234D RID: 9037 RVA: 0x009EAE14 File Offset: 0x009EAE14
        internal static string ArgumentOutOfRange_LengthGreaterThanCapacity {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_LengthGreaterThanCapacity");
            }
        }

        // Token: 0x17000564 RID: 1380
        // (get) Token: 0x0600234E RID: 9038 RVA: 0x009EAE20 File Offset: 0x009EAE20
        internal static string ArgumentOutOfRange_LessEqualToIntegerMaxVal {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_LessEqualToIntegerMaxVal");
            }
        }

        // Token: 0x17000565 RID: 1381
        // (get) Token: 0x0600234F RID: 9039 RVA: 0x009EAE2C File Offset: 0x009EAE2C
        internal static string ArgumentOutOfRange_ListInsert {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_ListInsert");
            }
        }

        // Token: 0x17000566 RID: 1382
        // (get) Token: 0x06002350 RID: 9040 RVA: 0x009EAE38 File Offset: 0x009EAE38
        internal static string ArgumentOutOfRange_Month {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Month");
            }
        }

        // Token: 0x17000567 RID: 1383
        // (get) Token: 0x06002351 RID: 9041 RVA: 0x009EAE44 File Offset: 0x009EAE44
        internal static string ArgumentOutOfRange_DayNumber {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_DayNumber");
            }
        }

        // Token: 0x17000568 RID: 1384
        // (get) Token: 0x06002352 RID: 9042 RVA: 0x009EAE50 File Offset: 0x009EAE50
        internal static string ArgumentOutOfRange_MonthParam {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_MonthParam");
            }
        }

        // Token: 0x17000569 RID: 1385
        // (get) Token: 0x06002353 RID: 9043 RVA: 0x009EAE5C File Offset: 0x009EAE5C
        internal static string ArgumentOutOfRange_MustBeNonNegNum {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_MustBeNonNegNum");
            }
        }

        // Token: 0x1700056A RID: 1386
        // (get) Token: 0x06002354 RID: 9044 RVA: 0x009EAE68 File Offset: 0x009EAE68
        internal static string ArgumentOutOfRange_MustBePositive {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_MustBePositive");
            }
        }

        // Token: 0x1700056B RID: 1387
        // (get) Token: 0x06002355 RID: 9045 RVA: 0x009EAE74 File Offset: 0x009EAE74
        internal static string ArgumentOutOfRange_NeedNonNegNum {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_NeedNonNegNum");
            }
        }

        // Token: 0x1700056C RID: 1388
        // (get) Token: 0x06002356 RID: 9046 RVA: 0x009EAE80 File Offset: 0x009EAE80
        internal static string ArgumentOutOfRange_NeedNonNegOrNegative1 {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1");
            }
        }

        // Token: 0x1700056D RID: 1389
        // (get) Token: 0x06002357 RID: 9047 RVA: 0x009EAE8C File Offset: 0x009EAE8C
        internal static string ArgumentOutOfRange_NeedValidId {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_NeedValidId");
            }
        }

        // Token: 0x1700056E RID: 1390
        // (get) Token: 0x06002358 RID: 9048 RVA: 0x009EAE98 File Offset: 0x009EAE98
        internal static string ArgumentOutOfRange_OffsetLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_OffsetLength");
            }
        }

        // Token: 0x1700056F RID: 1391
        // (get) Token: 0x06002359 RID: 9049 RVA: 0x009EAEA4 File Offset: 0x009EAEA4
        internal static string ArgumentOutOfRange_OffsetOut {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_OffsetOut");
            }
        }

        // Token: 0x17000570 RID: 1392
        // (get) Token: 0x0600235A RID: 9050 RVA: 0x009EAEB0 File Offset: 0x009EAEB0
        internal static string ArgumentOutOfRange_ParamSequence {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_ParamSequence");
            }
        }

        // Token: 0x17000571 RID: 1393
        // (get) Token: 0x0600235B RID: 9051 RVA: 0x009EAEBC File Offset: 0x009EAEBC
        internal static string ArgumentOutOfRange_PartialWCHAR {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_PartialWCHAR");
            }
        }

        // Token: 0x17000572 RID: 1394
        // (get) Token: 0x0600235C RID: 9052 RVA: 0x009EAEC8 File Offset: 0x009EAEC8
        internal static string ArgumentOutOfRange_PositionLessThanCapacityRequired {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_PositionLessThanCapacityRequired");
            }
        }

        // Token: 0x17000573 RID: 1395
        // (get) Token: 0x0600235D RID: 9053 RVA: 0x009EAED4 File Offset: 0x009EAED4
        internal static string ArgumentOutOfRange_Range {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Range");
            }
        }

        // Token: 0x17000574 RID: 1396
        // (get) Token: 0x0600235E RID: 9054 RVA: 0x009EAEE0 File Offset: 0x009EAEE0
        internal static string ArgumentOutOfRange_RoundingDigits {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_RoundingDigits");
            }
        }

        // Token: 0x17000575 RID: 1397
        // (get) Token: 0x0600235F RID: 9055 RVA: 0x009EAEEC File Offset: 0x009EAEEC
        internal static string ArgumentOutOfRange_SmallCapacity {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_SmallCapacity");
            }
        }

        // Token: 0x17000576 RID: 1398
        // (get) Token: 0x06002360 RID: 9056 RVA: 0x009EAEF8 File Offset: 0x009EAEF8
        internal static string ArgumentOutOfRange_StartIndex {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_StartIndex");
            }
        }

        // Token: 0x17000577 RID: 1399
        // (get) Token: 0x06002361 RID: 9057 RVA: 0x009EAF04 File Offset: 0x009EAF04
        internal static string ArgumentOutOfRange_StartIndexLargerThanLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_StartIndexLargerThanLength");
            }
        }

        // Token: 0x17000578 RID: 1400
        // (get) Token: 0x06002362 RID: 9058 RVA: 0x009EAF10 File Offset: 0x009EAF10
        internal static string ArgumentOutOfRange_StreamLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_StreamLength");
            }
        }

        // Token: 0x17000579 RID: 1401
        // (get) Token: 0x06002363 RID: 9059 RVA: 0x009EAF1C File Offset: 0x009EAF1C
        internal static string ArgumentOutOfRange_StreamPosition {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_StreamPosition");
            }
        }

        // Token: 0x1700057A RID: 1402
        // (get) Token: 0x06002364 RID: 9060 RVA: 0x009EAF28 File Offset: 0x009EAF28
        internal static string ArgumentOutOfRange_UIntPtrMax {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_UIntPtrMax");
            }
        }

        // Token: 0x1700057B RID: 1403
        // (get) Token: 0x06002365 RID: 9061 RVA: 0x009EAF34 File Offset: 0x009EAF34
        internal static string ArgumentOutOfRange_UnmanagedMemStreamLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamLength");
            }
        }

        // Token: 0x1700057C RID: 1404
        // (get) Token: 0x06002366 RID: 9062 RVA: 0x009EAF40 File Offset: 0x009EAF40
        internal static string ArgumentOutOfRange_UnmanagedMemStreamWrapAround {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_UnmanagedMemStreamWrapAround");
            }
        }

        // Token: 0x1700057D RID: 1405
        // (get) Token: 0x06002367 RID: 9063 RVA: 0x009EAF4C File Offset: 0x009EAF4C
        internal static string ArgumentOutOfRange_UtcOffset {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_UtcOffset");
            }
        }

        // Token: 0x1700057E RID: 1406
        // (get) Token: 0x06002368 RID: 9064 RVA: 0x009EAF58 File Offset: 0x009EAF58
        internal static string ArgumentOutOfRange_UtcOffsetAndDaylightDelta {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_UtcOffsetAndDaylightDelta");
            }
        }

        // Token: 0x1700057F RID: 1407
        // (get) Token: 0x06002369 RID: 9065 RVA: 0x009EAF64 File Offset: 0x009EAF64
        internal static string ArgumentOutOfRange_Week {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Week");
            }
        }

        // Token: 0x17000580 RID: 1408
        // (get) Token: 0x0600236A RID: 9066 RVA: 0x009EAF70 File Offset: 0x009EAF70
        internal static string ArgumentOutOfRange_Year {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Year");
            }
        }

        // Token: 0x17000581 RID: 1409
        // (get) Token: 0x0600236B RID: 9067 RVA: 0x009EAF7C File Offset: 0x009EAF7C
        internal static string ArgumentOutOfRange_Generic_MustBeNonZero {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeNonZero");
            }
        }

        // Token: 0x17000582 RID: 1410
        // (get) Token: 0x0600236C RID: 9068 RVA: 0x009EAF88 File Offset: 0x009EAF88
        internal static string ArgumentOutOfRange_Generic_MustBeNonNegative {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeNonNegative");
            }
        }

        // Token: 0x17000583 RID: 1411
        // (get) Token: 0x0600236D RID: 9069 RVA: 0x009EAF94 File Offset: 0x009EAF94
        internal static string ArgumentOutOfRange_Generic_MustBeNonNegativeNonZero {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeNonNegativeNonZero");
            }
        }

        // Token: 0x17000584 RID: 1412
        // (get) Token: 0x0600236E RID: 9070 RVA: 0x009EAFA0 File Offset: 0x009EAFA0
        internal static string ArgumentOutOfRange_Generic_MustBeLessOrEqual {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeLessOrEqual");
            }
        }

        // Token: 0x17000585 RID: 1413
        // (get) Token: 0x0600236F RID: 9071 RVA: 0x009EAFAC File Offset: 0x009EAFAC
        internal static string ArgumentOutOfRange_Generic_MustBeLess {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeLess");
            }
        }

        // Token: 0x17000586 RID: 1414
        // (get) Token: 0x06002370 RID: 9072 RVA: 0x009EAFB8 File Offset: 0x009EAFB8
        internal static string ArgumentOutOfRange_Generic_MustBeGreaterOrEqual {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeGreaterOrEqual");
            }
        }

        // Token: 0x17000587 RID: 1415
        // (get) Token: 0x06002371 RID: 9073 RVA: 0x009EAFC4 File Offset: 0x009EAFC4
        internal static string ArgumentOutOfRange_Generic_MustBeGreater {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeGreater");
            }
        }

        // Token: 0x17000588 RID: 1416
        // (get) Token: 0x06002372 RID: 9074 RVA: 0x009EAFD0 File Offset: 0x009EAFD0
        internal static string ArgumentOutOfRange_Generic_MustBeEqual {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeEqual");
            }
        }

        // Token: 0x17000589 RID: 1417
        // (get) Token: 0x06002373 RID: 9075 RVA: 0x009EAFDC File Offset: 0x009EAFDC
        internal static string ArgumentOutOfRange_Generic_MustBeNotEqual {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Generic_MustBeNotEqual");
            }
        }

        // Token: 0x1700058A RID: 1418
        // (get) Token: 0x06002374 RID: 9076 RVA: 0x009EAFE8 File Offset: 0x009EAFE8
        internal static string Arithmetic_NaN {
            get {
                return SR.GetResourceString("Arithmetic_NaN");
            }
        }

        // Token: 0x1700058B RID: 1419
        // (get) Token: 0x06002375 RID: 9077 RVA: 0x009EAFF4 File Offset: 0x009EAFF4
        internal static string ArrayTypeMismatch_CantAssignType {
            get {
                return SR.GetResourceString("ArrayTypeMismatch_CantAssignType");
            }
        }

        // Token: 0x1700058C RID: 1420
        // (get) Token: 0x06002376 RID: 9078 RVA: 0x009EB000 File Offset: 0x009EB000
        internal static string ArrayTypeMismatch_ConstrainedCopy {
            get {
                return SR.GetResourceString("ArrayTypeMismatch_ConstrainedCopy");
            }
        }

        // Token: 0x1700058D RID: 1421
        // (get) Token: 0x06002377 RID: 9079 RVA: 0x009EB00C File Offset: 0x009EB00C
        internal static string AssemblyLoadContext_Unload_CannotUnloadIfNotCollectible {
            get {
                return SR.GetResourceString("AssemblyLoadContext_Unload_CannotUnloadIfNotCollectible");
            }
        }

        // Token: 0x1700058E RID: 1422
        // (get) Token: 0x06002378 RID: 9080 RVA: 0x009EB018 File Offset: 0x009EB018
        internal static string AssemblyLoadContext_Verify_NotUnloading {
            get {
                return SR.GetResourceString("AssemblyLoadContext_Verify_NotUnloading");
            }
        }

        // Token: 0x1700058F RID: 1423
        // (get) Token: 0x06002379 RID: 9081 RVA: 0x009EB024 File Offset: 0x009EB024
        internal static string AssertionFailed {
            get {
                return SR.GetResourceString("AssertionFailed");
            }
        }

        // Token: 0x17000590 RID: 1424
        // (get) Token: 0x0600237A RID: 9082 RVA: 0x009EB030 File Offset: 0x009EB030
        internal static string AssertionFailed_Cnd {
            get {
                return SR.GetResourceString("AssertionFailed_Cnd");
            }
        }

        // Token: 0x17000591 RID: 1425
        // (get) Token: 0x0600237B RID: 9083 RVA: 0x009EB03C File Offset: 0x009EB03C
        internal static string AssumptionFailed {
            get {
                return SR.GetResourceString("AssumptionFailed");
            }
        }

        // Token: 0x17000592 RID: 1426
        // (get) Token: 0x0600237C RID: 9084 RVA: 0x009EB048 File Offset: 0x009EB048
        internal static string AssumptionFailed_Cnd {
            get {
                return SR.GetResourceString("AssumptionFailed_Cnd");
            }
        }

        // Token: 0x17000593 RID: 1427
        // (get) Token: 0x0600237D RID: 9085 RVA: 0x009EB054 File Offset: 0x009EB054
        internal static string AsyncMethodBuilder_InstanceNotInitialized {
            get {
                return SR.GetResourceString("AsyncMethodBuilder_InstanceNotInitialized");
            }
        }

        // Token: 0x17000594 RID: 1428
        // (get) Token: 0x0600237E RID: 9086 RVA: 0x009EB060 File Offset: 0x009EB060
        internal static string BadImageFormat_BadILFormat {
            get {
                return SR.GetResourceString("BadImageFormat_BadILFormat");
            }
        }

        // Token: 0x17000595 RID: 1429
        // (get) Token: 0x0600237F RID: 9087 RVA: 0x009EB06C File Offset: 0x009EB06C
        internal static string BadImageFormat_EmptyAssembly {
            get {
                return SR.GetResourceString("BadImageFormat_EmptyAssembly");
            }
        }

        // Token: 0x17000596 RID: 1430
        // (get) Token: 0x06002380 RID: 9088 RVA: 0x009EB078 File Offset: 0x009EB078
        internal static string BadImageFormat_InvalidType {
            get {
                return SR.GetResourceString("BadImageFormat_InvalidType");
            }
        }

        // Token: 0x17000597 RID: 1431
        // (get) Token: 0x06002381 RID: 9089 RVA: 0x009EB084 File Offset: 0x009EB084
        internal static string BadImageFormat_NegativeStringLength {
            get {
                return SR.GetResourceString("BadImageFormat_NegativeStringLength");
            }
        }

        // Token: 0x17000598 RID: 1432
        // (get) Token: 0x06002382 RID: 9090 RVA: 0x009EB090 File Offset: 0x009EB090
        internal static string BadImageFormat_ParameterSignatureMismatch {
            get {
                return SR.GetResourceString("BadImageFormat_ParameterSignatureMismatch");
            }
        }

        // Token: 0x17000599 RID: 1433
        // (get) Token: 0x06002383 RID: 9091 RVA: 0x009EB09C File Offset: 0x009EB09C
        internal static string BadImageFormat_ResType_SerBlobMismatch {
            get {
                return SR.GetResourceString("BadImageFormat_ResType_SerBlobMismatch");
            }
        }

        // Token: 0x1700059A RID: 1434
        // (get) Token: 0x06002384 RID: 9092 RVA: 0x009EB0A8 File Offset: 0x009EB0A8
        internal static string BadImageFormat_ResourceDataLengthInvalid {
            get {
                return SR.GetResourceString("BadImageFormat_ResourceDataLengthInvalid");
            }
        }

        // Token: 0x1700059B RID: 1435
        // (get) Token: 0x06002385 RID: 9093 RVA: 0x009EB0B4 File Offset: 0x009EB0B4
        internal static string BadImageFormat_ResourceNameCorrupted {
            get {
                return SR.GetResourceString("BadImageFormat_ResourceNameCorrupted");
            }
        }

        // Token: 0x1700059C RID: 1436
        // (get) Token: 0x06002386 RID: 9094 RVA: 0x009EB0C0 File Offset: 0x009EB0C0
        internal static string BadImageFormat_ResourceNameCorrupted_NameIndex {
            get {
                return SR.GetResourceString("BadImageFormat_ResourceNameCorrupted_NameIndex");
            }
        }

        // Token: 0x1700059D RID: 1437
        // (get) Token: 0x06002387 RID: 9095 RVA: 0x009EB0CC File Offset: 0x009EB0CC
        internal static string BadImageFormat_ResourcesDataInvalidOffset {
            get {
                return SR.GetResourceString("BadImageFormat_ResourcesDataInvalidOffset");
            }
        }

        // Token: 0x1700059E RID: 1438
        // (get) Token: 0x06002388 RID: 9096 RVA: 0x009EB0D8 File Offset: 0x009EB0D8
        internal static string BadImageFormat_ResourcesHeaderCorrupted {
            get {
                return SR.GetResourceString("BadImageFormat_ResourcesHeaderCorrupted");
            }
        }

        // Token: 0x1700059F RID: 1439
        // (get) Token: 0x06002389 RID: 9097 RVA: 0x009EB0E4 File Offset: 0x009EB0E4
        internal static string BadImageFormat_ResourcesIndexTooLong {
            get {
                return SR.GetResourceString("BadImageFormat_ResourcesIndexTooLong");
            }
        }

        // Token: 0x170005A0 RID: 1440
        // (get) Token: 0x0600238A RID: 9098 RVA: 0x009EB0F0 File Offset: 0x009EB0F0
        internal static string BadImageFormat_ResourcesNameInvalidOffset {
            get {
                return SR.GetResourceString("BadImageFormat_ResourcesNameInvalidOffset");
            }
        }

        // Token: 0x170005A1 RID: 1441
        // (get) Token: 0x0600238B RID: 9099 RVA: 0x009EB0FC File Offset: 0x009EB0FC
        internal static string BadImageFormat_ResourcesNameTooLong {
            get {
                return SR.GetResourceString("BadImageFormat_ResourcesNameTooLong");
            }
        }

        // Token: 0x170005A2 RID: 1442
        // (get) Token: 0x0600238C RID: 9100 RVA: 0x009EB108 File Offset: 0x009EB108
        internal static string BadImageFormat_TypeMismatch {
            get {
                return SR.GetResourceString("BadImageFormat_TypeMismatch");
            }
        }

        // Token: 0x170005A3 RID: 1443
        // (get) Token: 0x0600238D RID: 9101 RVA: 0x009EB114 File Offset: 0x009EB114
        internal static string CancellationToken_CreateLinkedToken_TokensIsEmpty {
            get {
                return SR.GetResourceString("CancellationToken_CreateLinkedToken_TokensIsEmpty");
            }
        }

        // Token: 0x170005A4 RID: 1444
        // (get) Token: 0x0600238E RID: 9102 RVA: 0x009EB120 File Offset: 0x009EB120
        internal static string CancellationTokenSource_Disposed {
            get {
                return SR.GetResourceString("CancellationTokenSource_Disposed");
            }
        }

        // Token: 0x170005A5 RID: 1445
        // (get) Token: 0x0600238F RID: 9103 RVA: 0x009EB12C File Offset: 0x009EB12C
        internal static string ConcurrentCollection_SyncRoot_NotSupported {
            get {
                return SR.GetResourceString("ConcurrentCollection_SyncRoot_NotSupported");
            }
        }

        // Token: 0x170005A6 RID: 1446
        // (get) Token: 0x06002390 RID: 9104 RVA: 0x009EB138 File Offset: 0x009EB138
        internal static string ConcurrentDictionary_ConcurrencyLevelMustBePositiveOrNegativeOne {
            get {
                return SR.GetResourceString("ConcurrentDictionary_ConcurrencyLevelMustBePositiveOrNegativeOne");
            }
        }

        // Token: 0x170005A7 RID: 1447
        // (get) Token: 0x06002391 RID: 9105 RVA: 0x009EB144 File Offset: 0x009EB144
        internal static string ConcurrentDictionary_ArrayIncorrectType {
            get {
                return SR.GetResourceString("ConcurrentDictionary_ArrayIncorrectType");
            }
        }

        // Token: 0x170005A8 RID: 1448
        // (get) Token: 0x06002392 RID: 9106 RVA: 0x009EB150 File Offset: 0x009EB150
        internal static string ConcurrentDictionary_SourceContainsDuplicateKeys {
            get {
                return SR.GetResourceString("ConcurrentDictionary_SourceContainsDuplicateKeys");
            }
        }

        // Token: 0x170005A9 RID: 1449
        // (get) Token: 0x06002393 RID: 9107 RVA: 0x009EB15C File Offset: 0x009EB15C
        internal static string ConcurrentDictionary_ArrayNotLargeEnough {
            get {
                return SR.GetResourceString("ConcurrentDictionary_ArrayNotLargeEnough");
            }
        }

        // Token: 0x170005AA RID: 1450
        // (get) Token: 0x06002394 RID: 9108 RVA: 0x009EB168 File Offset: 0x009EB168
        internal static string ConcurrentDictionary_KeyAlreadyExisted {
            get {
                return SR.GetResourceString("ConcurrentDictionary_KeyAlreadyExisted");
            }
        }

        // Token: 0x170005AB RID: 1451
        // (get) Token: 0x06002395 RID: 9109 RVA: 0x009EB174 File Offset: 0x009EB174
        internal static string ConcurrentDictionary_ItemKeyIsNull {
            get {
                return SR.GetResourceString("ConcurrentDictionary_ItemKeyIsNull");
            }
        }

        // Token: 0x170005AC RID: 1452
        // (get) Token: 0x06002396 RID: 9110 RVA: 0x009EB180 File Offset: 0x009EB180
        internal static string ConcurrentDictionary_TypeOfKeyIncorrect {
            get {
                return SR.GetResourceString("ConcurrentDictionary_TypeOfKeyIncorrect");
            }
        }

        // Token: 0x170005AD RID: 1453
        // (get) Token: 0x06002397 RID: 9111 RVA: 0x009EB18C File Offset: 0x009EB18C
        internal static string ConcurrentDictionary_TypeOfValueIncorrect {
            get {
                return SR.GetResourceString("ConcurrentDictionary_TypeOfValueIncorrect");
            }
        }

        // Token: 0x170005AE RID: 1454
        // (get) Token: 0x06002398 RID: 9112 RVA: 0x009EB198 File Offset: 0x009EB198
        internal static string CustomMarshaler_NoGetInstanceMethod {
            get {
                return SR.GetResourceString("CustomMarshaler_NoGetInstanceMethod");
            }
        }

        // Token: 0x170005AF RID: 1455
        // (get) Token: 0x06002399 RID: 9113 RVA: 0x009EB1A4 File Offset: 0x009EB1A4
        internal static string CustomMarshaler_NullReturnForGetInstance {
            get {
                return SR.GetResourceString("CustomMarshaler_NullReturnForGetInstance");
            }
        }

        // Token: 0x170005B0 RID: 1456
        // (get) Token: 0x0600239A RID: 9114 RVA: 0x009EB1B0 File Offset: 0x009EB1B0
        internal static string EventSource_AbstractMustNotDeclareEventMethods {
            get {
                return SR.GetResourceString("EventSource_AbstractMustNotDeclareEventMethods");
            }
        }

        // Token: 0x170005B1 RID: 1457
        // (get) Token: 0x0600239B RID: 9115 RVA: 0x009EB1BC File Offset: 0x009EB1BC
        internal static string EventSource_AbstractMustNotDeclareKTOC {
            get {
                return SR.GetResourceString("EventSource_AbstractMustNotDeclareKTOC");
            }
        }

        // Token: 0x170005B2 RID: 1458
        // (get) Token: 0x0600239C RID: 9116 RVA: 0x009EB1C8 File Offset: 0x009EB1C8
        internal static string EventSource_AddScalarOutOfRange {
            get {
                return SR.GetResourceString("EventSource_AddScalarOutOfRange");
            }
        }

        // Token: 0x170005B3 RID: 1459
        // (get) Token: 0x0600239D RID: 9117 RVA: 0x009EB1D4 File Offset: 0x009EB1D4
        internal static string EventSource_BadHexDigit {
            get {
                return SR.GetResourceString("EventSource_BadHexDigit");
            }
        }

        // Token: 0x170005B4 RID: 1460
        // (get) Token: 0x0600239E RID: 9118 RVA: 0x009EB1E0 File Offset: 0x009EB1E0
        internal static string EventSource_ChannelTypeDoesNotMatchEventChannelValue {
            get {
                return SR.GetResourceString("EventSource_ChannelTypeDoesNotMatchEventChannelValue");
            }
        }

        // Token: 0x170005B5 RID: 1461
        // (get) Token: 0x0600239F RID: 9119 RVA: 0x009EB1EC File Offset: 0x009EB1EC
        internal static string EventSource_DataDescriptorsOutOfRange {
            get {
                return SR.GetResourceString("EventSource_DataDescriptorsOutOfRange");
            }
        }

        // Token: 0x170005B6 RID: 1462
        // (get) Token: 0x060023A0 RID: 9120 RVA: 0x009EB1F8 File Offset: 0x009EB1F8
        internal static string EventSource_DuplicateStringKey {
            get {
                return SR.GetResourceString("EventSource_DuplicateStringKey");
            }
        }

        // Token: 0x170005B7 RID: 1463
        // (get) Token: 0x060023A1 RID: 9121 RVA: 0x009EB204 File Offset: 0x009EB204
        internal static string EventSource_EnumKindMismatch {
            get {
                return SR.GetResourceString("EventSource_EnumKindMismatch");
            }
        }

        // Token: 0x170005B8 RID: 1464
        // (get) Token: 0x060023A2 RID: 9122 RVA: 0x009EB210 File Offset: 0x009EB210
        internal static string EventSource_EvenHexDigits {
            get {
                return SR.GetResourceString("EventSource_EvenHexDigits");
            }
        }

        // Token: 0x170005B9 RID: 1465
        // (get) Token: 0x060023A3 RID: 9123 RVA: 0x009EB21C File Offset: 0x009EB21C
        internal static string EventSource_EventChannelOutOfRange {
            get {
                return SR.GetResourceString("EventSource_EventChannelOutOfRange");
            }
        }

        // Token: 0x170005BA RID: 1466
        // (get) Token: 0x060023A4 RID: 9124 RVA: 0x009EB228 File Offset: 0x009EB228
        internal static string EventSource_EventIdReused {
            get {
                return SR.GetResourceString("EventSource_EventIdReused");
            }
        }

        // Token: 0x170005BB RID: 1467
        // (get) Token: 0x060023A5 RID: 9125 RVA: 0x009EB234 File Offset: 0x009EB234
        internal static string EventSource_EventMustHaveTaskIfNonDefaultOpcode {
            get {
                return SR.GetResourceString("EventSource_EventMustHaveTaskIfNonDefaultOpcode");
            }
        }

        // Token: 0x170005BC RID: 1468
        // (get) Token: 0x060023A6 RID: 9126 RVA: 0x009EB240 File Offset: 0x009EB240
        internal static string EventSource_EventMustNotBeExplicitImplementation {
            get {
                return SR.GetResourceString("EventSource_EventMustNotBeExplicitImplementation");
            }
        }

        // Token: 0x170005BD RID: 1469
        // (get) Token: 0x060023A7 RID: 9127 RVA: 0x009EB24C File Offset: 0x009EB24C
        internal static string EventSource_EventNameReused {
            get {
                return SR.GetResourceString("EventSource_EventNameReused");
            }
        }

        // Token: 0x170005BE RID: 1470
        // (get) Token: 0x060023A8 RID: 9128 RVA: 0x009EB258 File Offset: 0x009EB258
        internal static string EventSource_EventParametersMismatch {
            get {
                return SR.GetResourceString("EventSource_EventParametersMismatch");
            }
        }

        // Token: 0x170005BF RID: 1471
        // (get) Token: 0x060023A9 RID: 9129 RVA: 0x009EB264 File Offset: 0x009EB264
        internal static string EventSource_EventSourceGuidInUse {
            get {
                return SR.GetResourceString("EventSource_EventSourceGuidInUse");
            }
        }

        // Token: 0x170005C0 RID: 1472
        // (get) Token: 0x060023AA RID: 9130 RVA: 0x009EB270 File Offset: 0x009EB270
        internal static string EventSource_EventTooBig {
            get {
                return SR.GetResourceString("EventSource_EventTooBig");
            }
        }

        // Token: 0x170005C1 RID: 1473
        // (get) Token: 0x060023AB RID: 9131 RVA: 0x009EB27C File Offset: 0x009EB27C
        internal static string EventSource_EventWithAdminChannelMustHaveMessage {
            get {
                return SR.GetResourceString("EventSource_EventWithAdminChannelMustHaveMessage");
            }
        }

        // Token: 0x170005C2 RID: 1474
        // (get) Token: 0x060023AC RID: 9132 RVA: 0x009EB288 File Offset: 0x009EB288
        internal static string EventSource_IllegalKeywordsValue {
            get {
                return SR.GetResourceString("EventSource_IllegalKeywordsValue");
            }
        }

        // Token: 0x170005C3 RID: 1475
        // (get) Token: 0x060023AD RID: 9133 RVA: 0x009EB294 File Offset: 0x009EB294
        internal static string EventSource_IllegalOpcodeValue {
            get {
                return SR.GetResourceString("EventSource_IllegalOpcodeValue");
            }
        }

        // Token: 0x170005C4 RID: 1476
        // (get) Token: 0x060023AE RID: 9134 RVA: 0x009EB2A0 File Offset: 0x009EB2A0
        internal static string EventSource_IllegalTaskValue {
            get {
                return SR.GetResourceString("EventSource_IllegalTaskValue");
            }
        }

        // Token: 0x170005C5 RID: 1477
        // (get) Token: 0x060023AF RID: 9135 RVA: 0x009EB2AC File Offset: 0x009EB2AC
        internal static string EventSource_IllegalValue {
            get {
                return SR.GetResourceString("EventSource_IllegalValue");
            }
        }

        // Token: 0x170005C6 RID: 1478
        // (get) Token: 0x060023B0 RID: 9136 RVA: 0x009EB2B8 File Offset: 0x009EB2B8
        internal static string EventSource_IncorrentlyAuthoredTypeInfo {
            get {
                return SR.GetResourceString("EventSource_IncorrentlyAuthoredTypeInfo");
            }
        }

        // Token: 0x170005C7 RID: 1479
        // (get) Token: 0x060023B1 RID: 9137 RVA: 0x009EB2C4 File Offset: 0x009EB2C4
        internal static string EventSource_InvalidCommand {
            get {
                return SR.GetResourceString("EventSource_InvalidCommand");
            }
        }

        // Token: 0x170005C8 RID: 1480
        // (get) Token: 0x060023B2 RID: 9138 RVA: 0x009EB2D0 File Offset: 0x009EB2D0
        internal static string EventSource_InvalidEventFormat {
            get {
                return SR.GetResourceString("EventSource_InvalidEventFormat");
            }
        }

        // Token: 0x170005C9 RID: 1481
        // (get) Token: 0x060023B3 RID: 9139 RVA: 0x009EB2DC File Offset: 0x009EB2DC
        internal static string EventSource_KeywordCollision {
            get {
                return SR.GetResourceString("EventSource_KeywordCollision");
            }
        }

        // Token: 0x170005CA RID: 1482
        // (get) Token: 0x060023B4 RID: 9140 RVA: 0x009EB2E8 File Offset: 0x009EB2E8
        internal static string EventSource_KeywordNeedPowerOfTwo {
            get {
                return SR.GetResourceString("EventSource_KeywordNeedPowerOfTwo");
            }
        }

        // Token: 0x170005CB RID: 1483
        // (get) Token: 0x060023B5 RID: 9141 RVA: 0x009EB2F4 File Offset: 0x009EB2F4
        internal static string EventSource_ListenerCreatedInsideCallback {
            get {
                return SR.GetResourceString("EventSource_ListenerCreatedInsideCallback");
            }
        }

        // Token: 0x170005CC RID: 1484
        // (get) Token: 0x060023B6 RID: 9142 RVA: 0x009EB300 File Offset: 0x009EB300
        internal static string EventSource_ListenerNotFound {
            get {
                return SR.GetResourceString("EventSource_ListenerNotFound");
            }
        }

        // Token: 0x170005CD RID: 1485
        // (get) Token: 0x060023B7 RID: 9143 RVA: 0x009EB30C File Offset: 0x009EB30C
        internal static string EventSource_ListenerWriteFailure {
            get {
                return SR.GetResourceString("EventSource_ListenerWriteFailure");
            }
        }

        // Token: 0x170005CE RID: 1486
        // (get) Token: 0x060023B8 RID: 9144 RVA: 0x009EB318 File Offset: 0x009EB318
        internal static string EventSource_MaxChannelExceeded {
            get {
                return SR.GetResourceString("EventSource_MaxChannelExceeded");
            }
        }

        // Token: 0x170005CF RID: 1487
        // (get) Token: 0x060023B9 RID: 9145 RVA: 0x009EB324 File Offset: 0x009EB324
        internal static string EventSource_MismatchIdToWriteEvent {
            get {
                return SR.GetResourceString("EventSource_MismatchIdToWriteEvent");
            }
        }

        // Token: 0x170005D0 RID: 1488
        // (get) Token: 0x060023BA RID: 9146 RVA: 0x009EB330 File Offset: 0x009EB330
        internal static string EventSource_NeedGuid {
            get {
                return SR.GetResourceString("EventSource_NeedGuid");
            }
        }

        // Token: 0x170005D1 RID: 1489
        // (get) Token: 0x060023BB RID: 9147 RVA: 0x009EB33C File Offset: 0x009EB33C
        internal static string EventSource_NeedName {
            get {
                return SR.GetResourceString("EventSource_NeedName");
            }
        }

        // Token: 0x170005D2 RID: 1490
        // (get) Token: 0x060023BC RID: 9148 RVA: 0x009EB348 File Offset: 0x009EB348
        internal static string EventSource_NeedPositiveId {
            get {
                return SR.GetResourceString("EventSource_NeedPositiveId");
            }
        }

        // Token: 0x170005D3 RID: 1491
        // (get) Token: 0x060023BD RID: 9149 RVA: 0x009EB354 File Offset: 0x009EB354
        internal static string EventSource_NoFreeBuffers {
            get {
                return SR.GetResourceString("EventSource_NoFreeBuffers");
            }
        }

        // Token: 0x170005D4 RID: 1492
        // (get) Token: 0x060023BE RID: 9150 RVA: 0x009EB360 File Offset: 0x009EB360
        internal static string EventSource_NonCompliantTypeError {
            get {
                return SR.GetResourceString("EventSource_NonCompliantTypeError");
            }
        }

        // Token: 0x170005D5 RID: 1493
        // (get) Token: 0x060023BF RID: 9151 RVA: 0x009EB36C File Offset: 0x009EB36C
        internal static string EventSource_NoRelatedActivityId {
            get {
                return SR.GetResourceString("EventSource_NoRelatedActivityId");
            }
        }

        // Token: 0x170005D6 RID: 1494
        // (get) Token: 0x060023C0 RID: 9152 RVA: 0x009EB378 File Offset: 0x009EB378
        internal static string EventSource_NotSupportedArrayOfBinary {
            get {
                return SR.GetResourceString("EventSource_NotSupportedArrayOfBinary");
            }
        }

        // Token: 0x170005D7 RID: 1495
        // (get) Token: 0x060023C1 RID: 9153 RVA: 0x009EB384 File Offset: 0x009EB384
        internal static string EventSource_NotSupportedArrayOfNil {
            get {
                return SR.GetResourceString("EventSource_NotSupportedArrayOfNil");
            }
        }

        // Token: 0x170005D8 RID: 1496
        // (get) Token: 0x060023C2 RID: 9154 RVA: 0x009EB390 File Offset: 0x009EB390
        internal static string EventSource_NotSupportedArrayOfNullTerminatedString {
            get {
                return SR.GetResourceString("EventSource_NotSupportedArrayOfNullTerminatedString");
            }
        }

        // Token: 0x170005D9 RID: 1497
        // (get) Token: 0x060023C3 RID: 9155 RVA: 0x009EB39C File Offset: 0x009EB39C
        internal static string EventSource_NotSupportedNestedArraysEnums {
            get {
                return SR.GetResourceString("EventSource_NotSupportedNestedArraysEnums");
            }
        }

        // Token: 0x170005DA RID: 1498
        // (get) Token: 0x060023C4 RID: 9156 RVA: 0x009EB3A8 File Offset: 0x009EB3A8
        internal static string EventSource_NullInput {
            get {
                return SR.GetResourceString("EventSource_NullInput");
            }
        }

        // Token: 0x170005DB RID: 1499
        // (get) Token: 0x060023C5 RID: 9157 RVA: 0x009EB3B4 File Offset: 0x009EB3B4
        internal static string EventSource_OpcodeCollision {
            get {
                return SR.GetResourceString("EventSource_OpcodeCollision");
            }
        }

        // Token: 0x170005DC RID: 1500
        // (get) Token: 0x060023C6 RID: 9158 RVA: 0x009EB3C0 File Offset: 0x009EB3C0
        internal static string EventSource_PinArrayOutOfRange {
            get {
                return SR.GetResourceString("EventSource_PinArrayOutOfRange");
            }
        }

        // Token: 0x170005DD RID: 1501
        // (get) Token: 0x060023C7 RID: 9159 RVA: 0x009EB3CC File Offset: 0x009EB3CC
        internal static string EventSource_RecursiveTypeDefinition {
            get {
                return SR.GetResourceString("EventSource_RecursiveTypeDefinition");
            }
        }

        // Token: 0x170005DE RID: 1502
        // (get) Token: 0x060023C8 RID: 9160 RVA: 0x009EB3D8 File Offset: 0x009EB3D8
        internal static string EventSource_StopsFollowStarts {
            get {
                return SR.GetResourceString("EventSource_StopsFollowStarts");
            }
        }

        // Token: 0x170005DF RID: 1503
        // (get) Token: 0x060023C9 RID: 9161 RVA: 0x009EB3E4 File Offset: 0x009EB3E4
        internal static string EventSource_TaskCollision {
            get {
                return SR.GetResourceString("EventSource_TaskCollision");
            }
        }

        // Token: 0x170005E0 RID: 1504
        // (get) Token: 0x060023CA RID: 9162 RVA: 0x009EB3F0 File Offset: 0x009EB3F0
        internal static string EventSource_TaskOpcodePairReused {
            get {
                return SR.GetResourceString("EventSource_TaskOpcodePairReused");
            }
        }

        // Token: 0x170005E1 RID: 1505
        // (get) Token: 0x060023CB RID: 9163 RVA: 0x009EB3FC File Offset: 0x009EB3FC
        internal static string EventSource_TooManyArgs {
            get {
                return SR.GetResourceString("EventSource_TooManyArgs");
            }
        }

        // Token: 0x170005E2 RID: 1506
        // (get) Token: 0x060023CC RID: 9164 RVA: 0x009EB408 File Offset: 0x009EB408
        internal static string EventSource_TooManyFields {
            get {
                return SR.GetResourceString("EventSource_TooManyFields");
            }
        }

        // Token: 0x170005E3 RID: 1507
        // (get) Token: 0x060023CD RID: 9165 RVA: 0x009EB414 File Offset: 0x009EB414
        internal static string EventSource_ToString {
            get {
                return SR.GetResourceString("EventSource_ToString");
            }
        }

        // Token: 0x170005E4 RID: 1508
        // (get) Token: 0x060023CE RID: 9166 RVA: 0x009EB420 File Offset: 0x009EB420
        internal static string EventSource_TraitEven {
            get {
                return SR.GetResourceString("EventSource_TraitEven");
            }
        }

        // Token: 0x170005E5 RID: 1509
        // (get) Token: 0x060023CF RID: 9167 RVA: 0x009EB42C File Offset: 0x009EB42C
        internal static string EventSource_TypeMustBeSealedOrAbstract {
            get {
                return SR.GetResourceString("EventSource_TypeMustBeSealedOrAbstract");
            }
        }

        // Token: 0x170005E6 RID: 1510
        // (get) Token: 0x060023D0 RID: 9168 RVA: 0x009EB438 File Offset: 0x009EB438
        internal static string EventSource_TypeMustDeriveFromEventSource {
            get {
                return SR.GetResourceString("EventSource_TypeMustDeriveFromEventSource");
            }
        }

        // Token: 0x170005E7 RID: 1511
        // (get) Token: 0x060023D1 RID: 9169 RVA: 0x009EB444 File Offset: 0x009EB444
        internal static string EventSource_UndefinedChannel {
            get {
                return SR.GetResourceString("EventSource_UndefinedChannel");
            }
        }

        // Token: 0x170005E8 RID: 1512
        // (get) Token: 0x060023D2 RID: 9170 RVA: 0x009EB450 File Offset: 0x009EB450
        internal static string EventSource_UndefinedKeyword {
            get {
                return SR.GetResourceString("EventSource_UndefinedKeyword");
            }
        }

        // Token: 0x170005E9 RID: 1513
        // (get) Token: 0x060023D3 RID: 9171 RVA: 0x009EB45C File Offset: 0x009EB45C
        internal static string EventSource_UndefinedOpcode {
            get {
                return SR.GetResourceString("EventSource_UndefinedOpcode");
            }
        }

        // Token: 0x170005EA RID: 1514
        // (get) Token: 0x060023D4 RID: 9172 RVA: 0x009EB468 File Offset: 0x009EB468
        internal static string EventSource_UnknownEtwTrait {
            get {
                return SR.GetResourceString("EventSource_UnknownEtwTrait");
            }
        }

        // Token: 0x170005EB RID: 1515
        // (get) Token: 0x060023D5 RID: 9173 RVA: 0x009EB474 File Offset: 0x009EB474
        internal static string EventSource_UnsupportedEventTypeInManifest {
            get {
                return SR.GetResourceString("EventSource_UnsupportedEventTypeInManifest");
            }
        }

        // Token: 0x170005EC RID: 1516
        // (get) Token: 0x060023D6 RID: 9174 RVA: 0x009EB480 File Offset: 0x009EB480
        internal static string EventSource_UnsupportedMessageProperty {
            get {
                return SR.GetResourceString("EventSource_UnsupportedMessageProperty");
            }
        }

        // Token: 0x170005ED RID: 1517
        // (get) Token: 0x060023D7 RID: 9175 RVA: 0x009EB48C File Offset: 0x009EB48C
        internal static string EventSource_VarArgsParameterMismatch {
            get {
                return SR.GetResourceString("EventSource_VarArgsParameterMismatch");
            }
        }

        // Token: 0x170005EE RID: 1518
        // (get) Token: 0x060023D8 RID: 9176 RVA: 0x009EB498 File Offset: 0x009EB498
        internal static string Exception_EndOfInnerExceptionStack {
            get {
                return SR.GetResourceString("Exception_EndOfInnerExceptionStack");
            }
        }

        // Token: 0x170005EF RID: 1519
        // (get) Token: 0x060023D9 RID: 9177 RVA: 0x009EB4A4 File Offset: 0x009EB4A4
        internal static string Exception_EndStackTraceFromPreviousThrow {
            get {
                return SR.GetResourceString("Exception_EndStackTraceFromPreviousThrow");
            }
        }

        // Token: 0x170005F0 RID: 1520
        // (get) Token: 0x060023DA RID: 9178 RVA: 0x009EB4B0 File Offset: 0x009EB4B0
        internal static string Exception_WasThrown {
            get {
                return SR.GetResourceString("Exception_WasThrown");
            }
        }

        // Token: 0x170005F1 RID: 1521
        // (get) Token: 0x060023DB RID: 9179 RVA: 0x009EB4BC File Offset: 0x009EB4BC
        internal static string ExecutionContext_ExceptionInAsyncLocalNotification {
            get {
                return SR.GetResourceString("ExecutionContext_ExceptionInAsyncLocalNotification");
            }
        }

        // Token: 0x170005F2 RID: 1522
        // (get) Token: 0x060023DC RID: 9180 RVA: 0x009EB4C8 File Offset: 0x009EB4C8
        internal static string FileNotFound_ResolveAssembly {
            get {
                return SR.GetResourceString("FileNotFound_ResolveAssembly");
            }
        }

        // Token: 0x170005F3 RID: 1523
        // (get) Token: 0x060023DD RID: 9181 RVA: 0x009EB4D4 File Offset: 0x009EB4D4
        internal static string FileNotFound_LoadFile {
            get {
                return SR.GetResourceString("FileNotFound_LoadFile");
            }
        }

        // Token: 0x170005F4 RID: 1524
        // (get) Token: 0x060023DE RID: 9182 RVA: 0x009EB4E0 File Offset: 0x009EB4E0
        internal static string Format_AttributeUsage {
            get {
                return SR.GetResourceString("Format_AttributeUsage");
            }
        }

        // Token: 0x170005F5 RID: 1525
        // (get) Token: 0x060023DF RID: 9183 RVA: 0x009EB4EC File Offset: 0x009EB4EC
        internal static string Format_Bad7BitInt {
            get {
                return SR.GetResourceString("Format_Bad7BitInt");
            }
        }

        // Token: 0x170005F6 RID: 1526
        // (get) Token: 0x060023E0 RID: 9184 RVA: 0x009EB4F8 File Offset: 0x009EB4F8
        internal static string Format_BadBase64Char {
            get {
                return SR.GetResourceString("Format_BadBase64Char");
            }
        }

        // Token: 0x170005F7 RID: 1527
        // (get) Token: 0x060023E1 RID: 9185 RVA: 0x009EB504 File Offset: 0x009EB504
        internal static string Format_BadBoolean {
            get {
                return SR.GetResourceString("Format_BadBoolean");
            }
        }

        // Token: 0x170005F8 RID: 1528
        // (get) Token: 0x060023E2 RID: 9186 RVA: 0x009EB510 File Offset: 0x009EB510
        internal static string Format_BadDatePattern {
            get {
                return SR.GetResourceString("Format_BadDatePattern");
            }
        }

        // Token: 0x170005F9 RID: 1529
        // (get) Token: 0x060023E3 RID: 9187 RVA: 0x009EB51C File Offset: 0x009EB51C
        internal static string Format_BadDateTime {
            get {
                return SR.GetResourceString("Format_BadDateTime");
            }
        }

        // Token: 0x170005FA RID: 1530
        // (get) Token: 0x060023E4 RID: 9188 RVA: 0x009EB528 File Offset: 0x009EB528
        internal static string Format_BadDateOnly {
            get {
                return SR.GetResourceString("Format_BadDateOnly");
            }
        }

        // Token: 0x170005FB RID: 1531
        // (get) Token: 0x060023E5 RID: 9189 RVA: 0x009EB534 File Offset: 0x009EB534
        internal static string Format_BadTimeOnly {
            get {
                return SR.GetResourceString("Format_BadTimeOnly");
            }
        }

        // Token: 0x170005FC RID: 1532
        // (get) Token: 0x060023E6 RID: 9190 RVA: 0x009EB540 File Offset: 0x009EB540
        internal static string Format_DateTimeOnlyContainsNoneDateParts {
            get {
                return SR.GetResourceString("Format_DateTimeOnlyContainsNoneDateParts");
            }
        }

        // Token: 0x170005FD RID: 1533
        // (get) Token: 0x060023E7 RID: 9191 RVA: 0x009EB54C File Offset: 0x009EB54C
        internal static string Format_BadDateTimeCalendar {
            get {
                return SR.GetResourceString("Format_BadDateTimeCalendar");
            }
        }

        // Token: 0x170005FE RID: 1534
        // (get) Token: 0x060023E8 RID: 9192 RVA: 0x009EB558 File Offset: 0x009EB558
        internal static string Format_BadDayOfWeek {
            get {
                return SR.GetResourceString("Format_BadDayOfWeek");
            }
        }

        // Token: 0x170005FF RID: 1535
        // (get) Token: 0x060023E9 RID: 9193 RVA: 0x009EB564 File Offset: 0x009EB564
        internal static string Format_BadFormatSpecifier {
            get {
                return SR.GetResourceString("Format_BadFormatSpecifier");
            }
        }

        // Token: 0x17000600 RID: 1536
        // (get) Token: 0x060023EA RID: 9194 RVA: 0x009EB570 File Offset: 0x009EB570
        internal static string Format_NoFormatSpecifier {
            get {
                return SR.GetResourceString("Format_NoFormatSpecifier");
            }
        }

        // Token: 0x17000601 RID: 1537
        // (get) Token: 0x060023EB RID: 9195 RVA: 0x009EB57C File Offset: 0x009EB57C
        internal static string Format_BadHexChar {
            get {
                return SR.GetResourceString("Format_BadHexChar");
            }
        }

        // Token: 0x17000602 RID: 1538
        // (get) Token: 0x060023EC RID: 9196 RVA: 0x009EB588 File Offset: 0x009EB588
        internal static string Format_BadHexLength {
            get {
                return SR.GetResourceString("Format_BadHexLength");
            }
        }

        // Token: 0x17000603 RID: 1539
        // (get) Token: 0x060023ED RID: 9197 RVA: 0x009EB594 File Offset: 0x009EB594
        internal static string Format_BadQuote {
            get {
                return SR.GetResourceString("Format_BadQuote");
            }
        }

        // Token: 0x17000604 RID: 1540
        // (get) Token: 0x060023EE RID: 9198 RVA: 0x009EB5A0 File Offset: 0x009EB5A0
        internal static string Format_BadTimeSpan {
            get {
                return SR.GetResourceString("Format_BadTimeSpan");
            }
        }

        // Token: 0x17000605 RID: 1541
        // (get) Token: 0x060023EF RID: 9199 RVA: 0x009EB5AC File Offset: 0x009EB5AC
        internal static string Format_DateOutOfRange {
            get {
                return SR.GetResourceString("Format_DateOutOfRange");
            }
        }

        // Token: 0x17000606 RID: 1542
        // (get) Token: 0x060023F0 RID: 9200 RVA: 0x009EB5B8 File Offset: 0x009EB5B8
        internal static string Format_EmptyInputString {
            get {
                return SR.GetResourceString("Format_EmptyInputString");
            }
        }

        // Token: 0x17000607 RID: 1543
        // (get) Token: 0x060023F1 RID: 9201 RVA: 0x009EB5C4 File Offset: 0x009EB5C4
        internal static string Format_ExtraJunkAtEnd {
            get {
                return SR.GetResourceString("Format_ExtraJunkAtEnd");
            }
        }

        // Token: 0x17000608 RID: 1544
        // (get) Token: 0x060023F2 RID: 9202 RVA: 0x009EB5D0 File Offset: 0x009EB5D0
        internal static string Format_GuidBrace {
            get {
                return SR.GetResourceString("Format_GuidBrace");
            }
        }

        // Token: 0x17000609 RID: 1545
        // (get) Token: 0x060023F3 RID: 9203 RVA: 0x009EB5DC File Offset: 0x009EB5DC
        internal static string Format_GuidBraceAfterLastNumber {
            get {
                return SR.GetResourceString("Format_GuidBraceAfterLastNumber");
            }
        }

        // Token: 0x1700060A RID: 1546
        // (get) Token: 0x060023F4 RID: 9204 RVA: 0x009EB5E8 File Offset: 0x009EB5E8
        internal static string Format_GuidComma {
            get {
                return SR.GetResourceString("Format_GuidComma");
            }
        }

        // Token: 0x1700060B RID: 1547
        // (get) Token: 0x060023F5 RID: 9205 RVA: 0x009EB5F4 File Offset: 0x009EB5F4
        internal static string Format_GuidDashes {
            get {
                return SR.GetResourceString("Format_GuidDashes");
            }
        }

        // Token: 0x1700060C RID: 1548
        // (get) Token: 0x060023F6 RID: 9206 RVA: 0x009EB600 File Offset: 0x009EB600
        internal static string Format_GuidEndBrace {
            get {
                return SR.GetResourceString("Format_GuidEndBrace");
            }
        }

        // Token: 0x1700060D RID: 1549
        // (get) Token: 0x060023F7 RID: 9207 RVA: 0x009EB60C File Offset: 0x009EB60C
        internal static string Format_GuidHexPrefix {
            get {
                return SR.GetResourceString("Format_GuidHexPrefix");
            }
        }

        // Token: 0x1700060E RID: 1550
        // (get) Token: 0x060023F8 RID: 9208 RVA: 0x009EB618 File Offset: 0x009EB618
        internal static string Format_GuidInvalidChar {
            get {
                return SR.GetResourceString("Format_GuidInvalidChar");
            }
        }

        // Token: 0x1700060F RID: 1551
        // (get) Token: 0x060023F9 RID: 9209 RVA: 0x009EB624 File Offset: 0x009EB624
        internal static string Format_GuidInvLen {
            get {
                return SR.GetResourceString("Format_GuidInvLen");
            }
        }

        // Token: 0x17000610 RID: 1552
        // (get) Token: 0x060023FA RID: 9210 RVA: 0x009EB630 File Offset: 0x009EB630
        internal static string Format_GuidUnrecognized {
            get {
                return SR.GetResourceString("Format_GuidUnrecognized");
            }
        }

        // Token: 0x17000611 RID: 1553
        // (get) Token: 0x060023FB RID: 9211 RVA: 0x009EB63C File Offset: 0x009EB63C
        internal static string Format_IndexOutOfRange {
            get {
                return SR.GetResourceString("Format_IndexOutOfRange");
            }
        }

        // Token: 0x17000612 RID: 1554
        // (get) Token: 0x060023FC RID: 9212 RVA: 0x009EB648 File Offset: 0x009EB648
        internal static string Format_InvalidEnumFormatSpecification {
            get {
                return SR.GetResourceString("Format_InvalidEnumFormatSpecification");
            }
        }

        // Token: 0x17000613 RID: 1555
        // (get) Token: 0x060023FD RID: 9213 RVA: 0x009EB654 File Offset: 0x009EB654
        internal static string Format_InvalidGuidFormatSpecification {
            get {
                return SR.GetResourceString("Format_InvalidGuidFormatSpecification");
            }
        }

        // Token: 0x17000614 RID: 1556
        // (get) Token: 0x060023FE RID: 9214 RVA: 0x009EB660 File Offset: 0x009EB660
        internal static string Format_InvalidString {
            get {
                return SR.GetResourceString("Format_InvalidString");
            }
        }

        // Token: 0x17000615 RID: 1557
        // (get) Token: 0x060023FF RID: 9215 RVA: 0x009EB66C File Offset: 0x009EB66C
        internal static string Format_InvalidStringWithValue {
            get {
                return SR.GetResourceString("Format_InvalidStringWithValue");
            }
        }

        // Token: 0x17000616 RID: 1558
        // (get) Token: 0x06002400 RID: 9216 RVA: 0x009EB678 File Offset: 0x009EB678
        internal static string Format_InvalidStringWithOffsetAndReason {
            get {
                return SR.GetResourceString("Format_InvalidStringWithOffsetAndReason");
            }
        }

        // Token: 0x17000617 RID: 1559
        // (get) Token: 0x06002401 RID: 9217 RVA: 0x009EB684 File Offset: 0x009EB684
        internal static string Format_UnexpectedClosingBrace {
            get {
                return SR.GetResourceString("Format_UnexpectedClosingBrace");
            }
        }

        // Token: 0x17000618 RID: 1560
        // (get) Token: 0x06002402 RID: 9218 RVA: 0x009EB690 File Offset: 0x009EB690
        internal static string Format_UnclosedFormatItem {
            get {
                return SR.GetResourceString("Format_UnclosedFormatItem");
            }
        }

        // Token: 0x17000619 RID: 1561
        // (get) Token: 0x06002403 RID: 9219 RVA: 0x009EB69C File Offset: 0x009EB69C
        internal static string Format_ExpectedAsciiDigit {
            get {
                return SR.GetResourceString("Format_ExpectedAsciiDigit");
            }
        }

        // Token: 0x1700061A RID: 1562
        // (get) Token: 0x06002404 RID: 9220 RVA: 0x009EB6A8 File Offset: 0x009EB6A8
        internal static string Format_MissingIncompleteDate {
            get {
                return SR.GetResourceString("Format_MissingIncompleteDate");
            }
        }

        // Token: 0x1700061B RID: 1563
        // (get) Token: 0x06002405 RID: 9221 RVA: 0x009EB6B4 File Offset: 0x009EB6B4
        internal static string Format_NeedSingleChar {
            get {
                return SR.GetResourceString("Format_NeedSingleChar");
            }
        }

        // Token: 0x1700061C RID: 1564
        // (get) Token: 0x06002406 RID: 9222 RVA: 0x009EB6C0 File Offset: 0x009EB6C0
        internal static string Format_NoParsibleDigits {
            get {
                return SR.GetResourceString("Format_NoParsibleDigits");
            }
        }

        // Token: 0x1700061D RID: 1565
        // (get) Token: 0x06002407 RID: 9223 RVA: 0x009EB6CC File Offset: 0x009EB6CC
        internal static string Format_OffsetOutOfRange {
            get {
                return SR.GetResourceString("Format_OffsetOutOfRange");
            }
        }

        // Token: 0x1700061E RID: 1566
        // (get) Token: 0x06002408 RID: 9224 RVA: 0x009EB6D8 File Offset: 0x009EB6D8
        internal static string Format_RepeatDateTimePattern {
            get {
                return SR.GetResourceString("Format_RepeatDateTimePattern");
            }
        }

        // Token: 0x1700061F RID: 1567
        // (get) Token: 0x06002409 RID: 9225 RVA: 0x009EB6E4 File Offset: 0x009EB6E4
        internal static string Format_StringZeroLength {
            get {
                return SR.GetResourceString("Format_StringZeroLength");
            }
        }

        // Token: 0x17000620 RID: 1568
        // (get) Token: 0x0600240A RID: 9226 RVA: 0x009EB6F0 File Offset: 0x009EB6F0
        internal static string Format_UnknownDateTimeWord {
            get {
                return SR.GetResourceString("Format_UnknownDateTimeWord");
            }
        }

        // Token: 0x17000621 RID: 1569
        // (get) Token: 0x0600240B RID: 9227 RVA: 0x009EB6FC File Offset: 0x009EB6FC
        internal static string Format_UTCOutOfRange {
            get {
                return SR.GetResourceString("Format_UTCOutOfRange");
            }
        }

        // Token: 0x17000622 RID: 1570
        // (get) Token: 0x0600240C RID: 9228 RVA: 0x009EB708 File Offset: 0x009EB708
        internal static string Globalization_cp_1200 {
            get {
                return SR.GetResourceString("Globalization_cp_1200");
            }
        }

        // Token: 0x17000623 RID: 1571
        // (get) Token: 0x0600240D RID: 9229 RVA: 0x009EB714 File Offset: 0x009EB714
        internal static string Globalization_cp_12000 {
            get {
                return SR.GetResourceString("Globalization_cp_12000");
            }
        }

        // Token: 0x17000624 RID: 1572
        // (get) Token: 0x0600240E RID: 9230 RVA: 0x009EB720 File Offset: 0x009EB720
        internal static string Globalization_cp_12001 {
            get {
                return SR.GetResourceString("Globalization_cp_12001");
            }
        }

        // Token: 0x17000625 RID: 1573
        // (get) Token: 0x0600240F RID: 9231 RVA: 0x009EB72C File Offset: 0x009EB72C
        internal static string Globalization_cp_1201 {
            get {
                return SR.GetResourceString("Globalization_cp_1201");
            }
        }

        // Token: 0x17000626 RID: 1574
        // (get) Token: 0x06002410 RID: 9232 RVA: 0x009EB738 File Offset: 0x009EB738
        internal static string Globalization_cp_20127 {
            get {
                return SR.GetResourceString("Globalization_cp_20127");
            }
        }

        // Token: 0x17000627 RID: 1575
        // (get) Token: 0x06002411 RID: 9233 RVA: 0x009EB744 File Offset: 0x009EB744
        internal static string Globalization_cp_28591 {
            get {
                return SR.GetResourceString("Globalization_cp_28591");
            }
        }

        // Token: 0x17000628 RID: 1576
        // (get) Token: 0x06002412 RID: 9234 RVA: 0x009EB750 File Offset: 0x009EB750
        internal static string Globalization_cp_65000 {
            get {
                return SR.GetResourceString("Globalization_cp_65000");
            }
        }

        // Token: 0x17000629 RID: 1577
        // (get) Token: 0x06002413 RID: 9235 RVA: 0x009EB75C File Offset: 0x009EB75C
        internal static string Globalization_cp_65001 {
            get {
                return SR.GetResourceString("Globalization_cp_65001");
            }
        }

        // Token: 0x1700062A RID: 1578
        // (get) Token: 0x06002414 RID: 9236 RVA: 0x009EB768 File Offset: 0x009EB768
        internal static string IndexOutOfRange_ArrayRankIndex {
            get {
                return SR.GetResourceString("IndexOutOfRange_ArrayRankIndex");
            }
        }

        // Token: 0x1700062B RID: 1579
        // (get) Token: 0x06002415 RID: 9237 RVA: 0x009EB774 File Offset: 0x009EB774
        internal static string IndexOutOfRange_UMSPosition {
            get {
                return SR.GetResourceString("IndexOutOfRange_UMSPosition");
            }
        }

        // Token: 0x1700062C RID: 1580
        // (get) Token: 0x06002416 RID: 9238 RVA: 0x009EB780 File Offset: 0x009EB780
        internal static string InsufficientMemory_MemFailPoint {
            get {
                return SR.GetResourceString("InsufficientMemory_MemFailPoint");
            }
        }

        // Token: 0x1700062D RID: 1581
        // (get) Token: 0x06002417 RID: 9239 RVA: 0x009EB78C File Offset: 0x009EB78C
        internal static string InsufficientMemory_MemFailPoint_TooBig {
            get {
                return SR.GetResourceString("InsufficientMemory_MemFailPoint_TooBig");
            }
        }

        // Token: 0x1700062E RID: 1582
        // (get) Token: 0x06002418 RID: 9240 RVA: 0x009EB798 File Offset: 0x009EB798
        internal static string InsufficientMemory_MemFailPoint_VAFrag {
            get {
                return SR.GetResourceString("InsufficientMemory_MemFailPoint_VAFrag");
            }
        }

        // Token: 0x1700062F RID: 1583
        // (get) Token: 0x06002419 RID: 9241 RVA: 0x009EB7A4 File Offset: 0x009EB7A4
        internal static string Interop_COM_TypeMismatch {
            get {
                return SR.GetResourceString("Interop_COM_TypeMismatch");
            }
        }

        // Token: 0x17000630 RID: 1584
        // (get) Token: 0x0600241A RID: 9242 RVA: 0x009EB7B0 File Offset: 0x009EB7B0
        internal static string Interop_Marshal_Unmappable_Char {
            get {
                return SR.GetResourceString("Interop_Marshal_Unmappable_Char");
            }
        }

        // Token: 0x17000631 RID: 1585
        // (get) Token: 0x0600241B RID: 9243 RVA: 0x009EB7BC File Offset: 0x009EB7BC
        internal static string Interop_Marshal_SafeHandle_InvalidOperation {
            get {
                return SR.GetResourceString("Interop_Marshal_SafeHandle_InvalidOperation");
            }
        }

        // Token: 0x17000632 RID: 1586
        // (get) Token: 0x0600241C RID: 9244 RVA: 0x009EB7C8 File Offset: 0x009EB7C8
        internal static string Interop_Marshal_CannotCreateSafeHandleField {
            get {
                return SR.GetResourceString("Interop_Marshal_CannotCreateSafeHandleField");
            }
        }

        // Token: 0x17000633 RID: 1587
        // (get) Token: 0x0600241D RID: 9245 RVA: 0x009EB7D4 File Offset: 0x009EB7D4
        internal static string Interop_Marshal_CannotCreateCriticalHandleField {
            get {
                return SR.GetResourceString("Interop_Marshal_CannotCreateCriticalHandleField");
            }
        }

        // Token: 0x17000634 RID: 1588
        // (get) Token: 0x0600241E RID: 9246 RVA: 0x009EB7E0 File Offset: 0x009EB7E0
        internal static string InvalidCast_CannotCastNullToValueType {
            get {
                return SR.GetResourceString("InvalidCast_CannotCastNullToValueType");
            }
        }

        // Token: 0x17000635 RID: 1589
        // (get) Token: 0x0600241F RID: 9247 RVA: 0x009EB7EC File Offset: 0x009EB7EC
        internal static string InvalidCast_CannotCoerceByRefVariant {
            get {
                return SR.GetResourceString("InvalidCast_CannotCoerceByRefVariant");
            }
        }

        // Token: 0x17000636 RID: 1590
        // (get) Token: 0x06002420 RID: 9248 RVA: 0x009EB7F8 File Offset: 0x009EB7F8
        internal static string InvalidCast_DBNull {
            get {
                return SR.GetResourceString("InvalidCast_DBNull");
            }
        }

        // Token: 0x17000637 RID: 1591
        // (get) Token: 0x06002421 RID: 9249 RVA: 0x009EB804 File Offset: 0x009EB804
        internal static string InvalidCast_DownCastArrayElement {
            get {
                return SR.GetResourceString("InvalidCast_DownCastArrayElement");
            }
        }

        // Token: 0x17000638 RID: 1592
        // (get) Token: 0x06002422 RID: 9250 RVA: 0x009EB810 File Offset: 0x009EB810
        internal static string InvalidCast_Empty {
            get {
                return SR.GetResourceString("InvalidCast_Empty");
            }
        }

        // Token: 0x17000639 RID: 1593
        // (get) Token: 0x06002423 RID: 9251 RVA: 0x009EB81C File Offset: 0x009EB81C
        internal static string InvalidCast_FromDBNull {
            get {
                return SR.GetResourceString("InvalidCast_FromDBNull");
            }
        }

        // Token: 0x1700063A RID: 1594
        // (get) Token: 0x06002424 RID: 9252 RVA: 0x009EB828 File Offset: 0x009EB828
        internal static string InvalidCast_FromTo {
            get {
                return SR.GetResourceString("InvalidCast_FromTo");
            }
        }

        // Token: 0x1700063B RID: 1595
        // (get) Token: 0x06002425 RID: 9253 RVA: 0x009EB834 File Offset: 0x009EB834
        internal static string InvalidCast_IConvertible {
            get {
                return SR.GetResourceString("InvalidCast_IConvertible");
            }
        }

        // Token: 0x1700063C RID: 1596
        // (get) Token: 0x06002426 RID: 9254 RVA: 0x009EB840 File Offset: 0x009EB840
        internal static string InvalidCast_OATypeMismatch {
            get {
                return SR.GetResourceString("InvalidCast_OATypeMismatch");
            }
        }

        // Token: 0x1700063D RID: 1597
        // (get) Token: 0x06002427 RID: 9255 RVA: 0x009EB84C File Offset: 0x009EB84C
        internal static string InvalidCast_StoreArrayElement {
            get {
                return SR.GetResourceString("InvalidCast_StoreArrayElement");
            }
        }

        // Token: 0x1700063E RID: 1598
        // (get) Token: 0x06002428 RID: 9256 RVA: 0x009EB858 File Offset: 0x009EB858
        internal static string InvalidOperation_AsyncFlowCtrlCtxMismatch {
            get {
                return SR.GetResourceString("InvalidOperation_AsyncFlowCtrlCtxMismatch");
            }
        }

        // Token: 0x1700063F RID: 1599
        // (get) Token: 0x06002429 RID: 9257 RVA: 0x009EB864 File Offset: 0x009EB864
        internal static string InvalidOperation_AsyncIOInProgress {
            get {
                return SR.GetResourceString("InvalidOperation_AsyncIOInProgress");
            }
        }

        // Token: 0x17000640 RID: 1600
        // (get) Token: 0x0600242A RID: 9258 RVA: 0x009EB870 File Offset: 0x009EB870
        internal static string InvalidOperation_BadEmptyMethodBody {
            get {
                return SR.GetResourceString("InvalidOperation_BadEmptyMethodBody");
            }
        }

        // Token: 0x17000641 RID: 1601
        // (get) Token: 0x0600242B RID: 9259 RVA: 0x009EB87C File Offset: 0x009EB87C
        internal static string InvalidOperation_BadILGeneratorUsage {
            get {
                return SR.GetResourceString("InvalidOperation_BadILGeneratorUsage");
            }
        }

        // Token: 0x17000642 RID: 1602
        // (get) Token: 0x0600242C RID: 9260 RVA: 0x009EB888 File Offset: 0x009EB888
        internal static string InvalidOperation_BadInstructionOrIndexOutOfBound {
            get {
                return SR.GetResourceString("InvalidOperation_BadInstructionOrIndexOutOfBound");
            }
        }

        // Token: 0x17000643 RID: 1603
        // (get) Token: 0x0600242D RID: 9261 RVA: 0x009EB894 File Offset: 0x009EB894
        internal static string InvalidOperation_BadInterfaceNotAbstract {
            get {
                return SR.GetResourceString("InvalidOperation_BadInterfaceNotAbstract");
            }
        }

        // Token: 0x17000644 RID: 1604
        // (get) Token: 0x0600242E RID: 9262 RVA: 0x009EB8A0 File Offset: 0x009EB8A0
        internal static string InvalidOperation_BadMethodBody {
            get {
                return SR.GetResourceString("InvalidOperation_BadMethodBody");
            }
        }

        // Token: 0x17000645 RID: 1605
        // (get) Token: 0x0600242F RID: 9263 RVA: 0x009EB8AC File Offset: 0x009EB8AC
        internal static string InvalidOperation_BadTypeAttributesNotAbstract {
            get {
                return SR.GetResourceString("InvalidOperation_BadTypeAttributesNotAbstract");
            }
        }

        // Token: 0x17000646 RID: 1606
        // (get) Token: 0x06002430 RID: 9264 RVA: 0x009EB8B8 File Offset: 0x009EB8B8
        internal static string InvalidOperation_CalledTwice {
            get {
                return SR.GetResourceString("InvalidOperation_CalledTwice");
            }
        }

        // Token: 0x17000647 RID: 1607
        // (get) Token: 0x06002431 RID: 9265 RVA: 0x009EB8C4 File Offset: 0x009EB8C4
        internal static string InvalidOperation_CannotImportGlobalFromDifferentModule {
            get {
                return SR.GetResourceString("InvalidOperation_CannotImportGlobalFromDifferentModule");
            }
        }

        // Token: 0x17000648 RID: 1608
        // (get) Token: 0x06002432 RID: 9266 RVA: 0x009EB8D0 File Offset: 0x009EB8D0
        internal static string InvalidOperation_CannotRegisterSecondResolver {
            get {
                return SR.GetResourceString("InvalidOperation_CannotRegisterSecondResolver");
            }
        }

        // Token: 0x17000649 RID: 1609
        // (get) Token: 0x06002433 RID: 9267 RVA: 0x009EB8DC File Offset: 0x009EB8DC
        internal static string InvalidOperation_CannotRegisterSecondHandler {
            get {
                return SR.GetResourceString("InvalidOperation_CannotRegisterSecondHandler");
            }
        }

        // Token: 0x1700064A RID: 1610
        // (get) Token: 0x06002434 RID: 9268 RVA: 0x009EB8E8 File Offset: 0x009EB8E8
        internal static string InvalidOperation_CannotRestoreUnsuppressedFlow {
            get {
                return SR.GetResourceString("InvalidOperation_CannotRestoreUnsuppressedFlow");
            }
        }

        // Token: 0x1700064B RID: 1611
        // (get) Token: 0x06002435 RID: 9269 RVA: 0x009EB8F4 File Offset: 0x009EB8F4
        internal static string InvalidOperation_CannotUseAFCOtherThread {
            get {
                return SR.GetResourceString("InvalidOperation_CannotUseAFCOtherThread");
            }
        }

        // Token: 0x1700064C RID: 1612
        // (get) Token: 0x06002436 RID: 9270 RVA: 0x009EB900 File Offset: 0x009EB900
        internal static string InvalidOperation_CollectionCorrupted {
            get {
                return SR.GetResourceString("InvalidOperation_CollectionCorrupted");
            }
        }

        // Token: 0x1700064D RID: 1613
        // (get) Token: 0x06002437 RID: 9271 RVA: 0x009EB90C File Offset: 0x009EB90C
        internal static string InvalidOperation_ComputerName {
            get {
                return SR.GetResourceString("InvalidOperation_ComputerName");
            }
        }

        // Token: 0x1700064E RID: 1614
        // (get) Token: 0x06002438 RID: 9272 RVA: 0x009EB918 File Offset: 0x009EB918
        internal static string InvalidOperation_ConcurrentOperationsNotSupported {
            get {
                return SR.GetResourceString("InvalidOperation_ConcurrentOperationsNotSupported");
            }
        }

        // Token: 0x1700064F RID: 1615
        // (get) Token: 0x06002439 RID: 9273 RVA: 0x009EB924 File Offset: 0x009EB924
        internal static string InvalidOperation_ConstructorNotAllowedOnInterface {
            get {
                return SR.GetResourceString("InvalidOperation_ConstructorNotAllowedOnInterface");
            }
        }

        // Token: 0x17000650 RID: 1616
        // (get) Token: 0x0600243A RID: 9274 RVA: 0x009EB930 File Offset: 0x009EB930
        internal static string InvalidOperation_DateTimeParsing {
            get {
                return SR.GetResourceString("InvalidOperation_DateTimeParsing");
            }
        }

        // Token: 0x17000651 RID: 1617
        // (get) Token: 0x0600243B RID: 9275 RVA: 0x009EB93C File Offset: 0x009EB93C
        internal static string InvalidOperation_DefaultConstructorILGen {
            get {
                return SR.GetResourceString("InvalidOperation_DefaultConstructorILGen");
            }
        }

        // Token: 0x17000652 RID: 1618
        // (get) Token: 0x0600243C RID: 9276 RVA: 0x009EB948 File Offset: 0x009EB948
        internal static string InvalidOperation_EnumEnded {
            get {
                return SR.GetResourceString("InvalidOperation_EnumEnded");
            }
        }

        // Token: 0x17000653 RID: 1619
        // (get) Token: 0x0600243D RID: 9277 RVA: 0x009EB954 File Offset: 0x009EB954
        internal static string InvalidOperation_EnumFailedVersion {
            get {
                return SR.GetResourceString("InvalidOperation_EnumFailedVersion");
            }
        }

        // Token: 0x17000654 RID: 1620
        // (get) Token: 0x0600243E RID: 9278 RVA: 0x009EB960 File Offset: 0x009EB960
        internal static string InvalidOperation_EnumNotStarted {
            get {
                return SR.GetResourceString("InvalidOperation_EnumNotStarted");
            }
        }

        // Token: 0x17000655 RID: 1621
        // (get) Token: 0x0600243F RID: 9279 RVA: 0x009EB96C File Offset: 0x009EB96C
        internal static string InvalidOperation_EnumOpCantHappen {
            get {
                return SR.GetResourceString("InvalidOperation_EnumOpCantHappen");
            }
        }

        // Token: 0x17000656 RID: 1622
        // (get) Token: 0x06002440 RID: 9280 RVA: 0x009EB978 File Offset: 0x009EB978
        internal static string InvalidOperation_EventInfoNotAvailable {
            get {
                return SR.GetResourceString("InvalidOperation_EventInfoNotAvailable");
            }
        }

        // Token: 0x17000657 RID: 1623
        // (get) Token: 0x06002441 RID: 9281 RVA: 0x009EB984 File Offset: 0x009EB984
        internal static string InvalidOperation_GenericParametersAlreadySet {
            get {
                return SR.GetResourceString("InvalidOperation_GenericParametersAlreadySet");
            }
        }

        // Token: 0x17000658 RID: 1624
        // (get) Token: 0x06002442 RID: 9282 RVA: 0x009EB990 File Offset: 0x009EB990
        internal static string InvalidOperation_GetVersion {
            get {
                return SR.GetResourceString("InvalidOperation_GetVersion");
            }
        }

        // Token: 0x17000659 RID: 1625
        // (get) Token: 0x06002443 RID: 9283 RVA: 0x009EB99C File Offset: 0x009EB99C
        internal static string InvalidOperation_GlobalsHaveBeenCreated {
            get {
                return SR.GetResourceString("InvalidOperation_GlobalsHaveBeenCreated");
            }
        }

        // Token: 0x1700065A RID: 1626
        // (get) Token: 0x06002444 RID: 9284 RVA: 0x009EB9A8 File Offset: 0x009EB9A8
        internal static string InvalidOperation_HandleIsNotInitialized {
            get {
                return SR.GetResourceString("InvalidOperation_HandleIsNotInitialized");
            }
        }

        // Token: 0x1700065B RID: 1627
        // (get) Token: 0x06002445 RID: 9285 RVA: 0x009EB9B4 File Offset: 0x009EB9B4
        internal static string InvalidOperation_HandleIsNotPinned {
            get {
                return SR.GetResourceString("InvalidOperation_HandleIsNotPinned");
            }
        }

        // Token: 0x1700065C RID: 1628
        // (get) Token: 0x06002446 RID: 9286 RVA: 0x009EB9C0 File Offset: 0x009EB9C0
        internal static string InvalidOperation_InvalidUtf8 {
            get {
                return SR.GetResourceString("InvalidOperation_InvalidUtf8");
            }
        }

        // Token: 0x1700065D RID: 1629
        // (get) Token: 0x06002447 RID: 9287 RVA: 0x009EB9CC File Offset: 0x009EB9CC
        internal static string InvalidOperation_HashInsertFailed {
            get {
                return SR.GetResourceString("InvalidOperation_HashInsertFailed");
            }
        }

        // Token: 0x1700065E RID: 1630
        // (get) Token: 0x06002448 RID: 9288 RVA: 0x009EB9D8 File Offset: 0x009EB9D8
        internal static string InvalidOperation_IComparerFailed {
            get {
                return SR.GetResourceString("InvalidOperation_IComparerFailed");
            }
        }

        // Token: 0x1700065F RID: 1631
        // (get) Token: 0x06002449 RID: 9289 RVA: 0x009EB9E4 File Offset: 0x009EB9E4
        internal static string InvalidOperation_MethodBaked {
            get {
                return SR.GetResourceString("InvalidOperation_MethodBaked");
            }
        }

        // Token: 0x17000660 RID: 1632
        // (get) Token: 0x0600244A RID: 9290 RVA: 0x009EB9F0 File Offset: 0x009EB9F0
        internal static string InvalidOperation_MethodBuilderBaked {
            get {
                return SR.GetResourceString("InvalidOperation_MethodBuilderBaked");
            }
        }

        // Token: 0x17000661 RID: 1633
        // (get) Token: 0x0600244B RID: 9291 RVA: 0x009EB9FC File Offset: 0x009EB9FC
        internal static string InvalidOperation_MethodHasBody {
            get {
                return SR.GetResourceString("InvalidOperation_MethodHasBody");
            }
        }

        // Token: 0x17000662 RID: 1634
        // (get) Token: 0x0600244C RID: 9292 RVA: 0x009EBA08 File Offset: 0x009EBA08
        internal static string InvalidOperation_MustCallInitialize {
            get {
                return SR.GetResourceString("InvalidOperation_MustCallInitialize");
            }
        }

        // Token: 0x17000663 RID: 1635
        // (get) Token: 0x0600244D RID: 9293 RVA: 0x009EBA14 File Offset: 0x009EBA14
        internal static string InvalidOperation_NativeOverlappedReused {
            get {
                return SR.GetResourceString("InvalidOperation_NativeOverlappedReused");
            }
        }

        // Token: 0x17000664 RID: 1636
        // (get) Token: 0x0600244E RID: 9294 RVA: 0x009EBA20 File Offset: 0x009EBA20
        internal static string InvalidOperation_NestedControlledExecutionRun {
            get {
                return SR.GetResourceString("InvalidOperation_NestedControlledExecutionRun");
            }
        }

        // Token: 0x17000665 RID: 1637
        // (get) Token: 0x0600244F RID: 9295 RVA: 0x009EBA2C File Offset: 0x009EBA2C
        internal static string InvalidOperation_NoMultiModuleAssembly {
            get {
                return SR.GetResourceString("InvalidOperation_NoMultiModuleAssembly");
            }
        }

        // Token: 0x17000666 RID: 1638
        // (get) Token: 0x06002450 RID: 9296 RVA: 0x009EBA38 File Offset: 0x009EBA38
        internal static string InvalidOperation_NoPublicAddMethod {
            get {
                return SR.GetResourceString("InvalidOperation_NoPublicAddMethod");
            }
        }

        // Token: 0x17000667 RID: 1639
        // (get) Token: 0x06002451 RID: 9297 RVA: 0x009EBA44 File Offset: 0x009EBA44
        internal static string InvalidOperation_NoPublicRemoveMethod {
            get {
                return SR.GetResourceString("InvalidOperation_NoPublicRemoveMethod");
            }
        }

        // Token: 0x17000668 RID: 1640
        // (get) Token: 0x06002452 RID: 9298 RVA: 0x009EBA50 File Offset: 0x009EBA50
        internal static string InvalidOperation_NotADebugModule {
            get {
                return SR.GetResourceString("InvalidOperation_NotADebugModule");
            }
        }

        // Token: 0x17000669 RID: 1641
        // (get) Token: 0x06002453 RID: 9299 RVA: 0x009EBA5C File Offset: 0x009EBA5C
        internal static string InvalidOperation_NotAllowedInDynamicMethod {
            get {
                return SR.GetResourceString("InvalidOperation_NotAllowedInDynamicMethod");
            }
        }

        // Token: 0x1700066A RID: 1642
        // (get) Token: 0x06002454 RID: 9300 RVA: 0x009EBA68 File Offset: 0x009EBA68
        internal static string InvalidOperation_NotAVarArgCallingConvention {
            get {
                return SR.GetResourceString("InvalidOperation_NotAVarArgCallingConvention");
            }
        }

        // Token: 0x1700066B RID: 1643
        // (get) Token: 0x06002455 RID: 9301 RVA: 0x009EBA74 File Offset: 0x009EBA74
        internal static string InvalidOperation_NotGenericType {
            get {
                return SR.GetResourceString("InvalidOperation_NotGenericType");
            }
        }

        // Token: 0x1700066C RID: 1644
        // (get) Token: 0x06002456 RID: 9302 RVA: 0x009EBA80 File Offset: 0x009EBA80
        internal static string InvalidOperation_NotWithConcurrentGC {
            get {
                return SR.GetResourceString("InvalidOperation_NotWithConcurrentGC");
            }
        }

        // Token: 0x1700066D RID: 1645
        // (get) Token: 0x06002457 RID: 9303 RVA: 0x009EBA8C File Offset: 0x009EBA8C
        internal static string InvalidOperation_NoUnderlyingTypeOnEnum {
            get {
                return SR.GetResourceString("InvalidOperation_NoUnderlyingTypeOnEnum");
            }
        }

        // Token: 0x1700066E RID: 1646
        // (get) Token: 0x06002458 RID: 9304 RVA: 0x009EBA98 File Offset: 0x009EBA98
        internal static string InvalidOperation_NoValue {
            get {
                return SR.GetResourceString("InvalidOperation_NoValue");
            }
        }

        // Token: 0x1700066F RID: 1647
        // (get) Token: 0x06002459 RID: 9305 RVA: 0x009EBAA4 File Offset: 0x009EBAA4
        internal static string InvalidOperation_NullArray {
            get {
                return SR.GetResourceString("InvalidOperation_NullArray");
            }
        }

        // Token: 0x17000670 RID: 1648
        // (get) Token: 0x0600245A RID: 9306 RVA: 0x009EBAB0 File Offset: 0x009EBAB0
        internal static string InvalidOperation_NullContext {
            get {
                return SR.GetResourceString("InvalidOperation_NullContext");
            }
        }

        // Token: 0x17000671 RID: 1649
        // (get) Token: 0x0600245B RID: 9307 RVA: 0x009EBABC File Offset: 0x009EBABC
        internal static string InvalidOperation_NullModuleHandle {
            get {
                return SR.GetResourceString("InvalidOperation_NullModuleHandle");
            }
        }

        // Token: 0x17000672 RID: 1650
        // (get) Token: 0x0600245C RID: 9308 RVA: 0x009EBAC8 File Offset: 0x009EBAC8
        internal static string InvalidOperation_OpenLocalVariableScope {
            get {
                return SR.GetResourceString("InvalidOperation_OpenLocalVariableScope");
            }
        }

        // Token: 0x17000673 RID: 1651
        // (get) Token: 0x0600245D RID: 9309 RVA: 0x009EBAD4 File Offset: 0x009EBAD4
        internal static string InvalidOperation_Overlapped_Pack {
            get {
                return SR.GetResourceString("InvalidOperation_Overlapped_Pack");
            }
        }

        // Token: 0x17000674 RID: 1652
        // (get) Token: 0x0600245E RID: 9310 RVA: 0x009EBAE0 File Offset: 0x009EBAE0
        internal static string InvalidOperation_PropertyInfoNotAvailable {
            get {
                return SR.GetResourceString("InvalidOperation_PropertyInfoNotAvailable");
            }
        }

        // Token: 0x17000675 RID: 1653
        // (get) Token: 0x0600245F RID: 9311 RVA: 0x009EBAEC File Offset: 0x009EBAEC
        internal static string InvalidOperation_ReadOnly {
            get {
                return SR.GetResourceString("InvalidOperation_ReadOnly");
            }
        }

        // Token: 0x17000676 RID: 1654
        // (get) Token: 0x06002460 RID: 9312 RVA: 0x009EBAF8 File Offset: 0x009EBAF8
        internal static string InvalidOperation_ResMgrBadResSet_Type {
            get {
                return SR.GetResourceString("InvalidOperation_ResMgrBadResSet_Type");
            }
        }

        // Token: 0x17000677 RID: 1655
        // (get) Token: 0x06002461 RID: 9313 RVA: 0x009EBB04 File Offset: 0x009EBB04
        internal static string InvalidOperation_ResourceNotStream_Name {
            get {
                return SR.GetResourceString("InvalidOperation_ResourceNotStream_Name");
            }
        }

        // Token: 0x17000678 RID: 1656
        // (get) Token: 0x06002462 RID: 9314 RVA: 0x009EBB10 File Offset: 0x009EBB10
        internal static string InvalidOperation_ResourceNotString_Name {
            get {
                return SR.GetResourceString("InvalidOperation_ResourceNotString_Name");
            }
        }

        // Token: 0x17000679 RID: 1657
        // (get) Token: 0x06002463 RID: 9315 RVA: 0x009EBB1C File Offset: 0x009EBB1C
        internal static string InvalidOperation_ResourceNotString_Type {
            get {
                return SR.GetResourceString("InvalidOperation_ResourceNotString_Type");
            }
        }

        // Token: 0x1700067A RID: 1658
        // (get) Token: 0x06002464 RID: 9316 RVA: 0x009EBB28 File Offset: 0x009EBB28
        internal static string InvalidOperation_SetLatencyModeNoGC {
            get {
                return SR.GetResourceString("InvalidOperation_SetLatencyModeNoGC");
            }
        }

        // Token: 0x1700067B RID: 1659
        // (get) Token: 0x06002465 RID: 9317 RVA: 0x009EBB34 File Offset: 0x009EBB34
        internal static string InvalidOperation_ShouldNotHaveMethodBody {
            get {
                return SR.GetResourceString("InvalidOperation_ShouldNotHaveMethodBody");
            }
        }

        // Token: 0x1700067C RID: 1660
        // (get) Token: 0x06002466 RID: 9318 RVA: 0x009EBB40 File Offset: 0x009EBB40
        internal static string InvalidOperation_ThreadWrongThreadStart {
            get {
                return SR.GetResourceString("InvalidOperation_ThreadWrongThreadStart");
            }
        }

        // Token: 0x1700067D RID: 1661
        // (get) Token: 0x06002467 RID: 9319 RVA: 0x009EBB4C File Offset: 0x009EBB4C
        internal static string InvalidOperation_TimeoutsNotSupported {
            get {
                return SR.GetResourceString("InvalidOperation_TimeoutsNotSupported");
            }
        }

        // Token: 0x1700067E RID: 1662
        // (get) Token: 0x06002468 RID: 9320 RVA: 0x009EBB58 File Offset: 0x009EBB58
        internal static string InvalidOperation_TimerAlreadyClosed {
            get {
                return SR.GetResourceString("InvalidOperation_TimerAlreadyClosed");
            }
        }

        // Token: 0x1700067F RID: 1663
        // (get) Token: 0x06002469 RID: 9321 RVA: 0x009EBB64 File Offset: 0x009EBB64
        internal static string InvalidOperation_TypeHasBeenCreated {
            get {
                return SR.GetResourceString("InvalidOperation_TypeHasBeenCreated");
            }
        }

        // Token: 0x17000680 RID: 1664
        // (get) Token: 0x0600246A RID: 9322 RVA: 0x009EBB70 File Offset: 0x009EBB70
        internal static string InvalidOperation_TypeNotCreated {
            get {
                return SR.GetResourceString("InvalidOperation_TypeNotCreated");
            }
        }

        // Token: 0x17000681 RID: 1665
        // (get) Token: 0x0600246B RID: 9323 RVA: 0x009EBB7C File Offset: 0x009EBB7C
        internal static string InvalidOperation_UnderlyingArrayListChanged {
            get {
                return SR.GetResourceString("InvalidOperation_UnderlyingArrayListChanged");
            }
        }

        // Token: 0x17000682 RID: 1666
        // (get) Token: 0x0600246C RID: 9324 RVA: 0x009EBB88 File Offset: 0x009EBB88
        internal static string InvalidOperation_UnknownEnumType {
            get {
                return SR.GetResourceString("InvalidOperation_UnknownEnumType");
            }
        }

        // Token: 0x17000683 RID: 1667
        // (get) Token: 0x0600246D RID: 9325 RVA: 0x009EBB94 File Offset: 0x009EBB94
        internal static string InvalidOperation_WrongAsyncResultOrEndCalledMultiple {
            get {
                return SR.GetResourceString("InvalidOperation_WrongAsyncResultOrEndCalledMultiple");
            }
        }

        // Token: 0x17000684 RID: 1668
        // (get) Token: 0x0600246E RID: 9326 RVA: 0x009EBBA0 File Offset: 0x009EBBA0
        internal static string InvalidProgram_Default {
            get {
                return SR.GetResourceString("InvalidProgram_Default");
            }
        }

        // Token: 0x17000685 RID: 1669
        // (get) Token: 0x0600246F RID: 9327 RVA: 0x009EBBAC File Offset: 0x009EBBAC
        internal static string InvariantFailed {
            get {
                return SR.GetResourceString("InvariantFailed");
            }
        }

        // Token: 0x17000686 RID: 1670
        // (get) Token: 0x06002470 RID: 9328 RVA: 0x009EBBB8 File Offset: 0x009EBBB8
        internal static string InvariantFailed_Cnd {
            get {
                return SR.GetResourceString("InvariantFailed_Cnd");
            }
        }

        // Token: 0x17000687 RID: 1671
        // (get) Token: 0x06002471 RID: 9329 RVA: 0x009EBBC4 File Offset: 0x009EBBC4
        internal static string IO_NoFileTableInInMemoryAssemblies {
            get {
                return SR.GetResourceString("IO_NoFileTableInInMemoryAssemblies");
            }
        }

        // Token: 0x17000688 RID: 1672
        // (get) Token: 0x06002472 RID: 9330 RVA: 0x009EBBD0 File Offset: 0x009EBBD0
        internal static string IO_EOF_ReadBeyondEOF {
            get {
                return SR.GetResourceString("IO_EOF_ReadBeyondEOF");
            }
        }

        // Token: 0x17000689 RID: 1673
        // (get) Token: 0x06002473 RID: 9331 RVA: 0x009EBBDC File Offset: 0x009EBBDC
        internal static string IO_FileLoad {
            get {
                return SR.GetResourceString("IO_FileLoad");
            }
        }

        // Token: 0x1700068A RID: 1674
        // (get) Token: 0x06002474 RID: 9332 RVA: 0x009EBBE8 File Offset: 0x009EBBE8
        internal static string IO_FileLoad_RequestedBy {
            get {
                return SR.GetResourceString("IO_FileLoad_RequestedBy");
            }
        }

        // Token: 0x1700068B RID: 1675
        // (get) Token: 0x06002475 RID: 9333 RVA: 0x009EBBF4 File Offset: 0x009EBBF4
        internal static string IO_DirectoryName_Name {
            get {
                return SR.GetResourceString("IO_DirectoryName_Name");
            }
        }

        // Token: 0x1700068C RID: 1676
        // (get) Token: 0x06002476 RID: 9334 RVA: 0x009EBC00 File Offset: 0x009EBC00
        internal static string IO_DirectoryNotFound_Path {
            get {
                return SR.GetResourceString("IO_DirectoryNotFound_Path");
            }
        }

        // Token: 0x1700068D RID: 1677
        // (get) Token: 0x06002477 RID: 9335 RVA: 0x009EBC0C File Offset: 0x009EBC0C
        internal static string IO_FileName_Name {
            get {
                return SR.GetResourceString("IO_FileName_Name");
            }
        }

        // Token: 0x1700068E RID: 1678
        // (get) Token: 0x06002478 RID: 9336 RVA: 0x009EBC18 File Offset: 0x009EBC18
        internal static string IO_FileNotFound {
            get {
                return SR.GetResourceString("IO_FileNotFound");
            }
        }

        // Token: 0x1700068F RID: 1679
        // (get) Token: 0x06002479 RID: 9337 RVA: 0x009EBC24 File Offset: 0x009EBC24
        internal static string IO_FileNotFound_FileName {
            get {
                return SR.GetResourceString("IO_FileNotFound_FileName");
            }
        }

        // Token: 0x17000690 RID: 1680
        // (get) Token: 0x0600247A RID: 9338 RVA: 0x009EBC30 File Offset: 0x009EBC30
        internal static string IO_AlreadyExists_Name {
            get {
                return SR.GetResourceString("IO_AlreadyExists_Name");
            }
        }

        // Token: 0x17000691 RID: 1681
        // (get) Token: 0x0600247B RID: 9339 RVA: 0x009EBC3C File Offset: 0x009EBC3C
        internal static string IO_DiskFull_Path_AllocationSize {
            get {
                return SR.GetResourceString("IO_DiskFull_Path_AllocationSize");
            }
        }

        // Token: 0x17000692 RID: 1682
        // (get) Token: 0x0600247C RID: 9340 RVA: 0x009EBC48 File Offset: 0x009EBC48
        internal static string IO_FileTooLarge_Path_AllocationSize {
            get {
                return SR.GetResourceString("IO_FileTooLarge_Path_AllocationSize");
            }
        }

        // Token: 0x17000693 RID: 1683
        // (get) Token: 0x0600247D RID: 9341 RVA: 0x009EBC54 File Offset: 0x009EBC54
        internal static string IO_BindHandleFailed {
            get {
                return SR.GetResourceString("IO_BindHandleFailed");
            }
        }

        // Token: 0x17000694 RID: 1684
        // (get) Token: 0x0600247E RID: 9342 RVA: 0x009EBC60 File Offset: 0x009EBC60
        internal static string IO_FileExists_Name {
            get {
                return SR.GetResourceString("IO_FileExists_Name");
            }
        }

        // Token: 0x17000695 RID: 1685
        // (get) Token: 0x0600247F RID: 9343 RVA: 0x009EBC6C File Offset: 0x009EBC6C
        internal static string IO_FileTooLong2GB {
            get {
                return SR.GetResourceString("IO_FileTooLong2GB");
            }
        }

        // Token: 0x17000696 RID: 1686
        // (get) Token: 0x06002480 RID: 9344 RVA: 0x009EBC78 File Offset: 0x009EBC78
        internal static string IO_FileTooLong {
            get {
                return SR.GetResourceString("IO_FileTooLong");
            }
        }

        // Token: 0x17000697 RID: 1687
        // (get) Token: 0x06002481 RID: 9345 RVA: 0x009EBC84 File Offset: 0x009EBC84
        internal static string IO_FixedCapacity {
            get {
                return SR.GetResourceString("IO_FixedCapacity");
            }
        }

        // Token: 0x17000698 RID: 1688
        // (get) Token: 0x06002482 RID: 9346 RVA: 0x009EBC90 File Offset: 0x009EBC90
        internal static string IO_InvalidStringLen_Len {
            get {
                return SR.GetResourceString("IO_InvalidStringLen_Len");
            }
        }

        // Token: 0x17000699 RID: 1689
        // (get) Token: 0x06002483 RID: 9347 RVA: 0x009EBC9C File Offset: 0x009EBC9C
        internal static string IO_SeekAppendOverwrite {
            get {
                return SR.GetResourceString("IO_SeekAppendOverwrite");
            }
        }

        // Token: 0x1700069A RID: 1690
        // (get) Token: 0x06002484 RID: 9348 RVA: 0x009EBCA8 File Offset: 0x009EBCA8
        internal static string IO_SeekBeforeBegin {
            get {
                return SR.GetResourceString("IO_SeekBeforeBegin");
            }
        }

        // Token: 0x1700069B RID: 1691
        // (get) Token: 0x06002485 RID: 9349 RVA: 0x009EBCB4 File Offset: 0x009EBCB4
        internal static string IO_SetLengthAppendTruncate {
            get {
                return SR.GetResourceString("IO_SetLengthAppendTruncate");
            }
        }

        // Token: 0x1700069C RID: 1692
        // (get) Token: 0x06002486 RID: 9350 RVA: 0x009EBCC0 File Offset: 0x009EBCC0
        internal static string IO_SharingViolation_File {
            get {
                return SR.GetResourceString("IO_SharingViolation_File");
            }
        }

        // Token: 0x1700069D RID: 1693
        // (get) Token: 0x06002487 RID: 9351 RVA: 0x009EBCCC File Offset: 0x009EBCCC
        internal static string IO_SharingViolation_NoFileName {
            get {
                return SR.GetResourceString("IO_SharingViolation_NoFileName");
            }
        }

        // Token: 0x1700069E RID: 1694
        // (get) Token: 0x06002488 RID: 9352 RVA: 0x009EBCD8 File Offset: 0x009EBCD8
        internal static string IO_StreamTooLong {
            get {
                return SR.GetResourceString("IO_StreamTooLong");
            }
        }

        // Token: 0x1700069F RID: 1695
        // (get) Token: 0x06002489 RID: 9353 RVA: 0x009EBCE4 File Offset: 0x009EBCE4
        internal static string IO_PathNotFound_NoPathName {
            get {
                return SR.GetResourceString("IO_PathNotFound_NoPathName");
            }
        }

        // Token: 0x170006A0 RID: 1696
        // (get) Token: 0x0600248A RID: 9354 RVA: 0x009EBCF0 File Offset: 0x009EBCF0
        internal static string IO_PathNotFound_Path {
            get {
                return SR.GetResourceString("IO_PathNotFound_Path");
            }
        }

        // Token: 0x170006A1 RID: 1697
        // (get) Token: 0x0600248B RID: 9355 RVA: 0x009EBCFC File Offset: 0x009EBCFC
        internal static string IO_PathTooLong {
            get {
                return SR.GetResourceString("IO_PathTooLong");
            }
        }

        // Token: 0x170006A2 RID: 1698
        // (get) Token: 0x0600248C RID: 9356 RVA: 0x009EBD08 File Offset: 0x009EBD08
        internal static string IO_PathTooLong_Path {
            get {
                return SR.GetResourceString("IO_PathTooLong_Path");
            }
        }

        // Token: 0x170006A3 RID: 1699
        // (get) Token: 0x0600248D RID: 9357 RVA: 0x009EBD14 File Offset: 0x009EBD14
        internal static string IO_UnknownFileName {
            get {
                return SR.GetResourceString("IO_UnknownFileName");
            }
        }

        // Token: 0x170006A4 RID: 1700
        // (get) Token: 0x0600248E RID: 9358 RVA: 0x009EBD20 File Offset: 0x009EBD20
        internal static string IO_MaxAttemptsReached {
            get {
                return SR.GetResourceString("IO_MaxAttemptsReached");
            }
        }

        // Token: 0x170006A5 RID: 1701
        // (get) Token: 0x0600248F RID: 9359 RVA: 0x009EBD2C File Offset: 0x009EBD2C
        internal static string Lazy_CreateValue_NoParameterlessCtorForT {
            get {
                return SR.GetResourceString("Lazy_CreateValue_NoParameterlessCtorForT");
            }
        }

        // Token: 0x170006A6 RID: 1702
        // (get) Token: 0x06002490 RID: 9360 RVA: 0x009EBD38 File Offset: 0x009EBD38
        internal static string Lazy_ctor_ModeInvalid {
            get {
                return SR.GetResourceString("Lazy_ctor_ModeInvalid");
            }
        }

        // Token: 0x170006A7 RID: 1703
        // (get) Token: 0x06002491 RID: 9361 RVA: 0x009EBD44 File Offset: 0x009EBD44
        internal static string Lazy_StaticInit_InvalidOperation {
            get {
                return SR.GetResourceString("Lazy_StaticInit_InvalidOperation");
            }
        }

        // Token: 0x170006A8 RID: 1704
        // (get) Token: 0x06002492 RID: 9362 RVA: 0x009EBD50 File Offset: 0x009EBD50
        internal static string Lazy_ToString_ValueNotCreated {
            get {
                return SR.GetResourceString("Lazy_ToString_ValueNotCreated");
            }
        }

        // Token: 0x170006A9 RID: 1705
        // (get) Token: 0x06002493 RID: 9363 RVA: 0x009EBD5C File Offset: 0x009EBD5C
        internal static string Lazy_Value_RecursiveCallsToValue {
            get {
                return SR.GetResourceString("Lazy_Value_RecursiveCallsToValue");
            }
        }

        // Token: 0x170006AA RID: 1706
        // (get) Token: 0x06002494 RID: 9364 RVA: 0x009EBD68 File Offset: 0x009EBD68
        internal static string ManualResetEventSlim_ctor_TooManyWaiters {
            get {
                return SR.GetResourceString("ManualResetEventSlim_ctor_TooManyWaiters");
            }
        }

        // Token: 0x170006AB RID: 1707
        // (get) Token: 0x06002495 RID: 9365 RVA: 0x009EBD74 File Offset: 0x009EBD74
        internal static string Marshaler_StringTooLong {
            get {
                return SR.GetResourceString("Marshaler_StringTooLong");
            }
        }

        // Token: 0x170006AC RID: 1708
        // (get) Token: 0x06002496 RID: 9366 RVA: 0x009EBD80 File Offset: 0x009EBD80
        internal static string MissingConstructor_Name {
            get {
                return SR.GetResourceString("MissingConstructor_Name");
            }
        }

        // Token: 0x170006AD RID: 1709
        // (get) Token: 0x06002497 RID: 9367 RVA: 0x009EBD8C File Offset: 0x009EBD8C
        internal static string MissingField {
            get {
                return SR.GetResourceString("MissingField");
            }
        }

        // Token: 0x170006AE RID: 1710
        // (get) Token: 0x06002498 RID: 9368 RVA: 0x009EBD98 File Offset: 0x009EBD98
        internal static string MissingField_Name {
            get {
                return SR.GetResourceString("MissingField_Name");
            }
        }

        // Token: 0x170006AF RID: 1711
        // (get) Token: 0x06002499 RID: 9369 RVA: 0x009EBDA4 File Offset: 0x009EBDA4
        internal static string MissingManifestResource_MultipleBlobs {
            get {
                return SR.GetResourceString("MissingManifestResource_MultipleBlobs");
            }
        }

        // Token: 0x170006B0 RID: 1712
        // (get) Token: 0x0600249A RID: 9370 RVA: 0x009EBDB0 File Offset: 0x009EBDB0
        internal static string MissingManifestResource_NoNeutralAsm {
            get {
                return SR.GetResourceString("MissingManifestResource_NoNeutralAsm");
            }
        }

        // Token: 0x170006B1 RID: 1713
        // (get) Token: 0x0600249B RID: 9371 RVA: 0x009EBDBC File Offset: 0x009EBDBC
        internal static string MissingManifestResource_NoNeutralDisk {
            get {
                return SR.GetResourceString("MissingManifestResource_NoNeutralDisk");
            }
        }

        // Token: 0x170006B2 RID: 1714
        // (get) Token: 0x0600249C RID: 9372 RVA: 0x009EBDC8 File Offset: 0x009EBDC8
        internal static string MissingMember {
            get {
                return SR.GetResourceString("MissingMember");
            }
        }

        // Token: 0x170006B3 RID: 1715
        // (get) Token: 0x0600249D RID: 9373 RVA: 0x009EBDD4 File Offset: 0x009EBDD4
        internal static string MissingMember_Name {
            get {
                return SR.GetResourceString("MissingMember_Name");
            }
        }

        // Token: 0x170006B4 RID: 1716
        // (get) Token: 0x0600249E RID: 9374 RVA: 0x009EBDE0 File Offset: 0x009EBDE0
        internal static string MissingMemberNestErr {
            get {
                return SR.GetResourceString("MissingMemberNestErr");
            }
        }

        // Token: 0x170006B5 RID: 1717
        // (get) Token: 0x0600249F RID: 9375 RVA: 0x009EBDEC File Offset: 0x009EBDEC
        internal static string MissingMemberTypeRef {
            get {
                return SR.GetResourceString("MissingMemberTypeRef");
            }
        }

        // Token: 0x170006B6 RID: 1718
        // (get) Token: 0x060024A0 RID: 9376 RVA: 0x009EBDF8 File Offset: 0x009EBDF8
        internal static string MissingMethod_Name {
            get {
                return SR.GetResourceString("MissingMethod_Name");
            }
        }

        // Token: 0x170006B7 RID: 1719
        // (get) Token: 0x060024A1 RID: 9377 RVA: 0x009EBE04 File Offset: 0x009EBE04
        internal static string MissingSatelliteAssembly_Culture_Name {
            get {
                return SR.GetResourceString("MissingSatelliteAssembly_Culture_Name");
            }
        }

        // Token: 0x170006B8 RID: 1720
        // (get) Token: 0x060024A2 RID: 9378 RVA: 0x009EBE10 File Offset: 0x009EBE10
        internal static string MissingSatelliteAssembly_Default {
            get {
                return SR.GetResourceString("MissingSatelliteAssembly_Default");
            }
        }

        // Token: 0x170006B9 RID: 1721
        // (get) Token: 0x060024A3 RID: 9379 RVA: 0x009EBE1C File Offset: 0x009EBE1C
        internal static string MustUseCCRewrite {
            get {
                return SR.GetResourceString("MustUseCCRewrite");
            }
        }

        // Token: 0x170006BA RID: 1722
        // (get) Token: 0x060024A4 RID: 9380 RVA: 0x009EBE28 File Offset: 0x009EBE28
        internal static string NotSupported_AbstractNonCLS {
            get {
                return SR.GetResourceString("NotSupported_AbstractNonCLS");
            }
        }

        // Token: 0x170006BB RID: 1723
        // (get) Token: 0x060024A5 RID: 9381 RVA: 0x009EBE34 File Offset: 0x009EBE34
        internal static string NotSupported_ActivAttr {
            get {
                return SR.GetResourceString("NotSupported_ActivAttr");
            }
        }

        // Token: 0x170006BC RID: 1724
        // (get) Token: 0x060024A6 RID: 9382 RVA: 0x009EBE40 File Offset: 0x009EBE40
        internal static string NotSupported_AssemblyLoadFromHash {
            get {
                return SR.GetResourceString("NotSupported_AssemblyLoadFromHash");
            }
        }

        // Token: 0x170006BD RID: 1725
        // (get) Token: 0x060024A7 RID: 9383 RVA: 0x009EBE4C File Offset: 0x009EBE4C
        internal static string NotSupported_ByRefLike {
            get {
                return SR.GetResourceString("NotSupported_ByRefLike");
            }
        }

        // Token: 0x170006BE RID: 1726
        // (get) Token: 0x060024A8 RID: 9384 RVA: 0x009EBE58 File Offset: 0x009EBE58
        internal static string NotSupported_ByRefToByRefLikeReturn {
            get {
                return SR.GetResourceString("NotSupported_ByRefToByRefLikeReturn");
            }
        }

        // Token: 0x170006BF RID: 1727
        // (get) Token: 0x060024A9 RID: 9385 RVA: 0x009EBE64 File Offset: 0x009EBE64
        internal static string NotSupported_ByRefToVoidReturn {
            get {
                return SR.GetResourceString("NotSupported_ByRefToVoidReturn");
            }
        }

        // Token: 0x170006C0 RID: 1728
        // (get) Token: 0x060024AA RID: 9386 RVA: 0x009EBE70 File Offset: 0x009EBE70
        internal static string NotSupported_Async {
            get {
                return SR.GetResourceString("NotSupported_Async");
            }
        }

        // Token: 0x170006C1 RID: 1729
        // (get) Token: 0x060024AB RID: 9387 RVA: 0x009EBE7C File Offset: 0x009EBE7C
        internal static string NotSupported_CallToVarArg {
            get {
                return SR.GetResourceString("NotSupported_CallToVarArg");
            }
        }

        // Token: 0x170006C2 RID: 1730
        // (get) Token: 0x060024AC RID: 9388 RVA: 0x009EBE88 File Offset: 0x009EBE88
        internal static string NotSupported_CannotCallEqualsOnSpan {
            get {
                return SR.GetResourceString("NotSupported_CannotCallEqualsOnSpan");
            }
        }

        // Token: 0x170006C3 RID: 1731
        // (get) Token: 0x060024AD RID: 9389 RVA: 0x009EBE94 File Offset: 0x009EBE94
        internal static string NotSupported_CannotCallGetHashCodeOnSpan {
            get {
                return SR.GetResourceString("NotSupported_CannotCallGetHashCodeOnSpan");
            }
        }

        // Token: 0x170006C4 RID: 1732
        // (get) Token: 0x060024AE RID: 9390 RVA: 0x009EBEA0 File Offset: 0x009EBEA0
        internal static string NotSupported_ChangeType {
            get {
                return SR.GetResourceString("NotSupported_ChangeType");
            }
        }

        // Token: 0x170006C5 RID: 1733
        // (get) Token: 0x060024AF RID: 9391 RVA: 0x009EBEAC File Offset: 0x009EBEAC
        internal static string NotSupported_CreateInstanceWithTypeBuilder {
            get {
                return SR.GetResourceString("NotSupported_CreateInstanceWithTypeBuilder");
            }
        }

        // Token: 0x170006C6 RID: 1734
        // (get) Token: 0x060024B0 RID: 9392 RVA: 0x009EBEB8 File Offset: 0x009EBEB8
        internal static string NotSupported_TypeBuilderInstantiation_ResolvingMembers {
            get {
                return SR.GetResourceString("NotSupported_TypeBuilderInstantiation_ResolvingMembers");
            }
        }

        // Token: 0x170006C7 RID: 1735
        // (get) Token: 0x060024B1 RID: 9393 RVA: 0x009EBEC4 File Offset: 0x009EBEC4
        internal static string NotSupported_DynamicAssembly {
            get {
                return SR.GetResourceString("NotSupported_DynamicAssembly");
            }
        }

        // Token: 0x170006C8 RID: 1736
        // (get) Token: 0x060024B2 RID: 9394 RVA: 0x009EBED0 File Offset: 0x009EBED0
        internal static string NotSupported_DynamicMethodFlags {
            get {
                return SR.GetResourceString("NotSupported_DynamicMethodFlags");
            }
        }

        // Token: 0x170006C9 RID: 1737
        // (get) Token: 0x060024B3 RID: 9395 RVA: 0x009EBEDC File Offset: 0x009EBEDC
        internal static string NotSupported_DynamicModule {
            get {
                return SR.GetResourceString("NotSupported_DynamicModule");
            }
        }

        // Token: 0x170006CA RID: 1738
        // (get) Token: 0x060024B4 RID: 9396 RVA: 0x009EBEE8 File Offset: 0x009EBEE8
        internal static string NotSupported_FixedSizeCollection {
            get {
                return SR.GetResourceString("NotSupported_FixedSizeCollection");
            }
        }

        // Token: 0x170006CB RID: 1739
        // (get) Token: 0x060024B5 RID: 9397 RVA: 0x009EBEF4 File Offset: 0x009EBEF4
        internal static string InvalidOperation_SpanOverlappedOperation {
            get {
                return SR.GetResourceString("InvalidOperation_SpanOverlappedOperation");
            }
        }

        // Token: 0x170006CC RID: 1740
        // (get) Token: 0x060024B6 RID: 9398 RVA: 0x009EBF00 File Offset: 0x009EBF00
        internal static string NotSupported_StackTraceSupportDisabled {
            get {
                return SR.GetResourceString("NotSupported_StackTraceSupportDisabled");
            }
        }

        // Token: 0x170006CD RID: 1741
        // (get) Token: 0x060024B7 RID: 9399 RVA: 0x009EBF0C File Offset: 0x009EBF0C
        internal static string InvalidOperation_TimeProviderNullLocalTimeZone {
            get {
                return SR.GetResourceString("InvalidOperation_TimeProviderNullLocalTimeZone");
            }
        }

        // Token: 0x170006CE RID: 1742
        // (get) Token: 0x060024B8 RID: 9400 RVA: 0x009EBF18 File Offset: 0x009EBF18
        internal static string InvalidOperation_TimeProviderInvalidTimestampFrequency {
            get {
                return SR.GetResourceString("InvalidOperation_TimeProviderInvalidTimestampFrequency");
            }
        }

        // Token: 0x170006CF RID: 1743
        // (get) Token: 0x060024B9 RID: 9401 RVA: 0x009EBF24 File Offset: 0x009EBF24
        internal static string NotSupported_IllegalOneByteBranch {
            get {
                return SR.GetResourceString("NotSupported_IllegalOneByteBranch");
            }
        }

        // Token: 0x170006D0 RID: 1744
        // (get) Token: 0x060024BA RID: 9402 RVA: 0x009EBF30 File Offset: 0x009EBF30
        internal static string NotSupported_KeyCollectionSet {
            get {
                return SR.GetResourceString("NotSupported_KeyCollectionSet");
            }
        }

        // Token: 0x170006D1 RID: 1745
        // (get) Token: 0x060024BB RID: 9403 RVA: 0x009EBF3C File Offset: 0x009EBF3C
        internal static string NotSupported_MaxWaitHandles {
            get {
                return SR.GetResourceString("NotSupported_MaxWaitHandles");
            }
        }

        // Token: 0x170006D2 RID: 1746
        // (get) Token: 0x060024BC RID: 9404 RVA: 0x009EBF48 File Offset: 0x009EBF48
        internal static string NotSupported_MaxWaitHandles_STA {
            get {
                return SR.GetResourceString("NotSupported_MaxWaitHandles_STA");
            }
        }

        // Token: 0x170006D3 RID: 1747
        // (get) Token: 0x060024BD RID: 9405 RVA: 0x009EBF54 File Offset: 0x009EBF54
        internal static string NotSupported_MemStreamNotExpandable {
            get {
                return SR.GetResourceString("NotSupported_MemStreamNotExpandable");
            }
        }

        // Token: 0x170006D4 RID: 1748
        // (get) Token: 0x060024BE RID: 9406 RVA: 0x009EBF60 File Offset: 0x009EBF60
        internal static string NotSupported_MustBeModuleBuilder {
            get {
                return SR.GetResourceString("NotSupported_MustBeModuleBuilder");
            }
        }

        // Token: 0x170006D5 RID: 1749
        // (get) Token: 0x060024BF RID: 9407 RVA: 0x009EBF6C File Offset: 0x009EBF6C
        internal static string NotSupported_NoCodepageData {
            get {
                return SR.GetResourceString("NotSupported_NoCodepageData");
            }
        }

        // Token: 0x170006D6 RID: 1750
        // (get) Token: 0x060024C0 RID: 9408 RVA: 0x009EBF78 File Offset: 0x009EBF78
        internal static string InvalidOperation_FunctionMissingUnmanagedCallersOnly {
            get {
                return SR.GetResourceString("InvalidOperation_FunctionMissingUnmanagedCallersOnly");
            }
        }

        // Token: 0x170006D7 RID: 1751
        // (get) Token: 0x060024C1 RID: 9409 RVA: 0x009EBF84 File Offset: 0x009EBF84
        internal static string NotSupported_NonReflectedType {
            get {
                return SR.GetResourceString("NotSupported_NonReflectedType");
            }
        }

        // Token: 0x170006D8 RID: 1752
        // (get) Token: 0x060024C2 RID: 9410 RVA: 0x009EBF90 File Offset: 0x009EBF90
        internal static string NotSupported_NoParentDefaultConstructor {
            get {
                return SR.GetResourceString("NotSupported_NoParentDefaultConstructor");
            }
        }

        // Token: 0x170006D9 RID: 1753
        // (get) Token: 0x060024C3 RID: 9411 RVA: 0x009EBF9C File Offset: 0x009EBF9C
        internal static string NotSupported_NoTypeInfo {
            get {
                return SR.GetResourceString("NotSupported_NoTypeInfo");
            }
        }

        // Token: 0x170006DA RID: 1754
        // (get) Token: 0x060024C4 RID: 9412 RVA: 0x009EBFA8 File Offset: 0x009EBFA8
        internal static string NotSupported_NYI {
            get {
                return SR.GetResourceString("NotSupported_NYI");
            }
        }

        // Token: 0x170006DB RID: 1755
        // (get) Token: 0x060024C5 RID: 9413 RVA: 0x009EBFB4 File Offset: 0x009EBFB4
        internal static string NotSupported_ObsoleteResourcesFile {
            get {
                return SR.GetResourceString("NotSupported_ObsoleteResourcesFile");
            }
        }

        // Token: 0x170006DC RID: 1756
        // (get) Token: 0x060024C6 RID: 9414 RVA: 0x009EBFC0 File Offset: 0x009EBFC0
        internal static string NotSupported_OleAutBadVarType {
            get {
                return SR.GetResourceString("NotSupported_OleAutBadVarType");
            }
        }

        // Token: 0x170006DD RID: 1757
        // (get) Token: 0x060024C7 RID: 9415 RVA: 0x009EBFCC File Offset: 0x009EBFCC
        internal static string NotSupported_OutputStreamUsingTypeBuilder {
            get {
                return SR.GetResourceString("NotSupported_OutputStreamUsingTypeBuilder");
            }
        }

        // Token: 0x170006DE RID: 1758
        // (get) Token: 0x060024C8 RID: 9416 RVA: 0x009EBFD8 File Offset: 0x009EBFD8
        internal static string NotSupported_RangeCollection {
            get {
                return SR.GetResourceString("NotSupported_RangeCollection");
            }
        }

        // Token: 0x170006DF RID: 1759
        // (get) Token: 0x060024C9 RID: 9417 RVA: 0x009EBFE4 File Offset: 0x009EBFE4
        internal static string NotSupported_Reading {
            get {
                return SR.GetResourceString("NotSupported_Reading");
            }
        }

        // Token: 0x170006E0 RID: 1760
        // (get) Token: 0x060024CA RID: 9418 RVA: 0x009EBFF0 File Offset: 0x009EBFF0
        internal static string NotSupported_ReadOnlyCollection {
            get {
                return SR.GetResourceString("NotSupported_ReadOnlyCollection");
            }
        }

        // Token: 0x170006E1 RID: 1761
        // (get) Token: 0x060024CB RID: 9419 RVA: 0x009EBFFC File Offset: 0x009EBFFC
        internal static string NotSupported_ResourceObjectSerialization {
            get {
                return SR.GetResourceString("NotSupported_ResourceObjectSerialization");
            }
        }

        // Token: 0x170006E2 RID: 1762
        // (get) Token: 0x060024CC RID: 9420 RVA: 0x009EC008 File Offset: 0x009EC008
        internal static string NotSupported_StringComparison {
            get {
                return SR.GetResourceString("NotSupported_StringComparison");
            }
        }

        // Token: 0x170006E3 RID: 1763
        // (get) Token: 0x060024CD RID: 9421 RVA: 0x009EC014 File Offset: 0x009EC014
        internal static string NotSupported_SubclassOverride {
            get {
                return SR.GetResourceString("NotSupported_SubclassOverride");
            }
        }

        // Token: 0x170006E4 RID: 1764
        // (get) Token: 0x060024CE RID: 9422 RVA: 0x009EC020 File Offset: 0x009EC020
        internal static string NotSupported_SymbolMethod {
            get {
                return SR.GetResourceString("NotSupported_SymbolMethod");
            }
        }

        // Token: 0x170006E5 RID: 1765
        // (get) Token: 0x060024CF RID: 9423 RVA: 0x009EC02C File Offset: 0x009EC02C
        internal static string NotSupported_Type {
            get {
                return SR.GetResourceString("NotSupported_Type");
            }
        }

        // Token: 0x170006E6 RID: 1766
        // (get) Token: 0x060024D0 RID: 9424 RVA: 0x009EC038 File Offset: 0x009EC038
        internal static string NotSupported_TypeNotYetCreated {
            get {
                return SR.GetResourceString("NotSupported_TypeNotYetCreated");
            }
        }

        // Token: 0x170006E7 RID: 1767
        // (get) Token: 0x060024D1 RID: 9425 RVA: 0x009EC044 File Offset: 0x009EC044
        internal static string NotSupported_UmsSafeBuffer {
            get {
                return SR.GetResourceString("NotSupported_UmsSafeBuffer");
            }
        }

        // Token: 0x170006E8 RID: 1768
        // (get) Token: 0x060024D2 RID: 9426 RVA: 0x009EC050 File Offset: 0x009EC050
        internal static string NotSupported_UnitySerHolder {
            get {
                return SR.GetResourceString("NotSupported_UnitySerHolder");
            }
        }

        // Token: 0x170006E9 RID: 1769
        // (get) Token: 0x060024D3 RID: 9427 RVA: 0x009EC05C File Offset: 0x009EC05C
        internal static string NotSupported_UnknownTypeCode {
            get {
                return SR.GetResourceString("NotSupported_UnknownTypeCode");
            }
        }

        // Token: 0x170006EA RID: 1770
        // (get) Token: 0x060024D4 RID: 9428 RVA: 0x009EC068 File Offset: 0x009EC068
        internal static string NotSupported_WaitAllSTAThread {
            get {
                return SR.GetResourceString("NotSupported_WaitAllSTAThread");
            }
        }

        // Token: 0x170006EB RID: 1771
        // (get) Token: 0x060024D5 RID: 9429 RVA: 0x009EC074 File Offset: 0x009EC074
        internal static string NotSupported_UnreadableStream {
            get {
                return SR.GetResourceString("NotSupported_UnreadableStream");
            }
        }

        // Token: 0x170006EC RID: 1772
        // (get) Token: 0x060024D6 RID: 9430 RVA: 0x009EC080 File Offset: 0x009EC080
        internal static string NotSupported_UnseekableStream {
            get {
                return SR.GetResourceString("NotSupported_UnseekableStream");
            }
        }

        // Token: 0x170006ED RID: 1773
        // (get) Token: 0x060024D7 RID: 9431 RVA: 0x009EC08C File Offset: 0x009EC08C
        internal static string NotSupported_UnwritableStream {
            get {
                return SR.GetResourceString("NotSupported_UnwritableStream");
            }
        }

        // Token: 0x170006EE RID: 1774
        // (get) Token: 0x060024D8 RID: 9432 RVA: 0x009EC098 File Offset: 0x009EC098
        internal static string NotSupported_ValueCollectionSet {
            get {
                return SR.GetResourceString("NotSupported_ValueCollectionSet");
            }
        }

        // Token: 0x170006EF RID: 1775
        // (get) Token: 0x060024D9 RID: 9433 RVA: 0x009EC0A4 File Offset: 0x009EC0A4
        internal static string NotSupported_Writing {
            get {
                return SR.GetResourceString("NotSupported_Writing");
            }
        }

        // Token: 0x170006F0 RID: 1776
        // (get) Token: 0x060024DA RID: 9434 RVA: 0x009EC0B0 File Offset: 0x009EC0B0
        internal static string NotSupported_WrongResourceReader_Type {
            get {
                return SR.GetResourceString("NotSupported_WrongResourceReader_Type");
            }
        }

        // Token: 0x170006F1 RID: 1777
        // (get) Token: 0x060024DB RID: 9435 RVA: 0x009EC0BC File Offset: 0x009EC0BC
        internal static string ObjectDisposed_FileClosed {
            get {
                return SR.GetResourceString("ObjectDisposed_FileClosed");
            }
        }

        // Token: 0x170006F2 RID: 1778
        // (get) Token: 0x060024DC RID: 9436 RVA: 0x009EC0C8 File Offset: 0x009EC0C8
        internal static string ObjectDisposed_Generic {
            get {
                return SR.GetResourceString("ObjectDisposed_Generic");
            }
        }

        // Token: 0x170006F3 RID: 1779
        // (get) Token: 0x060024DD RID: 9437 RVA: 0x009EC0D4 File Offset: 0x009EC0D4
        internal static string ObjectDisposed_ObjectName_Name {
            get {
                return SR.GetResourceString("ObjectDisposed_ObjectName_Name");
            }
        }

        // Token: 0x170006F4 RID: 1780
        // (get) Token: 0x060024DE RID: 9438 RVA: 0x009EC0E0 File Offset: 0x009EC0E0
        internal static string ObjectDisposed_WriterClosed {
            get {
                return SR.GetResourceString("ObjectDisposed_WriterClosed");
            }
        }

        // Token: 0x170006F5 RID: 1781
        // (get) Token: 0x060024DF RID: 9439 RVA: 0x009EC0EC File Offset: 0x009EC0EC
        internal static string ObjectDisposed_ReaderClosed {
            get {
                return SR.GetResourceString("ObjectDisposed_ReaderClosed");
            }
        }

        // Token: 0x170006F6 RID: 1782
        // (get) Token: 0x060024E0 RID: 9440 RVA: 0x009EC0F8 File Offset: 0x009EC0F8
        internal static string ObjectDisposed_ResourceSet {
            get {
                return SR.GetResourceString("ObjectDisposed_ResourceSet");
            }
        }

        // Token: 0x170006F7 RID: 1783
        // (get) Token: 0x060024E1 RID: 9441 RVA: 0x009EC104 File Offset: 0x009EC104
        internal static string ObjectDisposed_StreamClosed {
            get {
                return SR.GetResourceString("ObjectDisposed_StreamClosed");
            }
        }

        // Token: 0x170006F8 RID: 1784
        // (get) Token: 0x060024E2 RID: 9442 RVA: 0x009EC110 File Offset: 0x009EC110
        internal static string ObjectDisposed_ViewAccessorClosed {
            get {
                return SR.GetResourceString("ObjectDisposed_ViewAccessorClosed");
            }
        }

        // Token: 0x170006F9 RID: 1785
        // (get) Token: 0x060024E3 RID: 9443 RVA: 0x009EC11C File Offset: 0x009EC11C
        internal static string OperationCanceled {
            get {
                return SR.GetResourceString("OperationCanceled");
            }
        }

        // Token: 0x170006FA RID: 1786
        // (get) Token: 0x060024E4 RID: 9444 RVA: 0x009EC128 File Offset: 0x009EC128
        internal static string Overflow_Byte {
            get {
                return SR.GetResourceString("Overflow_Byte");
            }
        }

        // Token: 0x170006FB RID: 1787
        // (get) Token: 0x060024E5 RID: 9445 RVA: 0x009EC134 File Offset: 0x009EC134
        internal static string Overflow_Char {
            get {
                return SR.GetResourceString("Overflow_Char");
            }
        }

        // Token: 0x170006FC RID: 1788
        // (get) Token: 0x060024E6 RID: 9446 RVA: 0x009EC140 File Offset: 0x009EC140
        internal static string Overflow_Currency {
            get {
                return SR.GetResourceString("Overflow_Currency");
            }
        }

        // Token: 0x170006FD RID: 1789
        // (get) Token: 0x060024E7 RID: 9447 RVA: 0x009EC14C File Offset: 0x009EC14C
        internal static string Overflow_Decimal {
            get {
                return SR.GetResourceString("Overflow_Decimal");
            }
        }

        // Token: 0x170006FE RID: 1790
        // (get) Token: 0x060024E8 RID: 9448 RVA: 0x009EC158 File Offset: 0x009EC158
        internal static string Overflow_Duration {
            get {
                return SR.GetResourceString("Overflow_Duration");
            }
        }

        // Token: 0x170006FF RID: 1791
        // (get) Token: 0x060024E9 RID: 9449 RVA: 0x009EC164 File Offset: 0x009EC164
        internal static string Overflow_Int16 {
            get {
                return SR.GetResourceString("Overflow_Int16");
            }
        }

        // Token: 0x17000700 RID: 1792
        // (get) Token: 0x060024EA RID: 9450 RVA: 0x009EC170 File Offset: 0x009EC170
        internal static string Overflow_Int32 {
            get {
                return SR.GetResourceString("Overflow_Int32");
            }
        }

        // Token: 0x17000701 RID: 1793
        // (get) Token: 0x060024EB RID: 9451 RVA: 0x009EC17C File Offset: 0x009EC17C
        internal static string Overflow_Int64 {
            get {
                return SR.GetResourceString("Overflow_Int64");
            }
        }

        // Token: 0x17000702 RID: 1794
        // (get) Token: 0x060024EC RID: 9452 RVA: 0x009EC188 File Offset: 0x009EC188
        internal static string Overflow_Int128 {
            get {
                return SR.GetResourceString("Overflow_Int128");
            }
        }

        // Token: 0x17000703 RID: 1795
        // (get) Token: 0x060024ED RID: 9453 RVA: 0x009EC194 File Offset: 0x009EC194
        internal static string Overflow_MutexReacquireCount {
            get {
                return SR.GetResourceString("Overflow_MutexReacquireCount");
            }
        }

        // Token: 0x17000704 RID: 1796
        // (get) Token: 0x060024EE RID: 9454 RVA: 0x009EC1A0 File Offset: 0x009EC1A0
        internal static string Overflow_NegateTwosCompNum {
            get {
                return SR.GetResourceString("Overflow_NegateTwosCompNum");
            }
        }

        // Token: 0x17000705 RID: 1797
        // (get) Token: 0x060024EF RID: 9455 RVA: 0x009EC1AC File Offset: 0x009EC1AC
        internal static string Overflow_NegativeUnsigned {
            get {
                return SR.GetResourceString("Overflow_NegativeUnsigned");
            }
        }

        // Token: 0x17000706 RID: 1798
        // (get) Token: 0x060024F0 RID: 9456 RVA: 0x009EC1B8 File Offset: 0x009EC1B8
        internal static string Overflow_SByte {
            get {
                return SR.GetResourceString("Overflow_SByte");
            }
        }

        // Token: 0x17000707 RID: 1799
        // (get) Token: 0x060024F1 RID: 9457 RVA: 0x009EC1C4 File Offset: 0x009EC1C4
        internal static string Overflow_TimeSpanElementTooLarge {
            get {
                return SR.GetResourceString("Overflow_TimeSpanElementTooLarge");
            }
        }

        // Token: 0x17000708 RID: 1800
        // (get) Token: 0x060024F2 RID: 9458 RVA: 0x009EC1D0 File Offset: 0x009EC1D0
        internal static string Overflow_TimeSpanTooLong {
            get {
                return SR.GetResourceString("Overflow_TimeSpanTooLong");
            }
        }

        // Token: 0x17000709 RID: 1801
        // (get) Token: 0x060024F3 RID: 9459 RVA: 0x009EC1DC File Offset: 0x009EC1DC
        internal static string Overflow_UInt16 {
            get {
                return SR.GetResourceString("Overflow_UInt16");
            }
        }

        // Token: 0x1700070A RID: 1802
        // (get) Token: 0x060024F4 RID: 9460 RVA: 0x009EC1E8 File Offset: 0x009EC1E8
        internal static string Overflow_UInt32 {
            get {
                return SR.GetResourceString("Overflow_UInt32");
            }
        }

        // Token: 0x1700070B RID: 1803
        // (get) Token: 0x060024F5 RID: 9461 RVA: 0x009EC1F4 File Offset: 0x009EC1F4
        internal static string Overflow_UInt64 {
            get {
                return SR.GetResourceString("Overflow_UInt64");
            }
        }

        // Token: 0x1700070C RID: 1804
        // (get) Token: 0x060024F6 RID: 9462 RVA: 0x009EC200 File Offset: 0x009EC200
        internal static string Overflow_UInt128 {
            get {
                return SR.GetResourceString("Overflow_UInt128");
            }
        }

        // Token: 0x1700070D RID: 1805
        // (get) Token: 0x060024F7 RID: 9463 RVA: 0x009EC20C File Offset: 0x009EC20C
        internal static string PlatformNotSupported_ReflectionOnly {
            get {
                return SR.GetResourceString("PlatformNotSupported_ReflectionOnly");
            }
        }

        // Token: 0x1700070E RID: 1806
        // (get) Token: 0x060024F8 RID: 9464 RVA: 0x009EC218 File Offset: 0x009EC218
        internal static string PlatformNotSupported_Remoting {
            get {
                return SR.GetResourceString("PlatformNotSupported_Remoting");
            }
        }

        // Token: 0x1700070F RID: 1807
        // (get) Token: 0x060024F9 RID: 9465 RVA: 0x009EC224 File Offset: 0x009EC224
        internal static string PlatformNotSupported_SecureBinarySerialization {
            get {
                return SR.GetResourceString("PlatformNotSupported_SecureBinarySerialization");
            }
        }

        // Token: 0x17000710 RID: 1808
        // (get) Token: 0x060024FA RID: 9466 RVA: 0x009EC230 File Offset: 0x009EC230
        internal static string PlatformNotSupported_StrongNameSigning {
            get {
                return SR.GetResourceString("PlatformNotSupported_StrongNameSigning");
            }
        }

        // Token: 0x17000711 RID: 1809
        // (get) Token: 0x060024FB RID: 9467 RVA: 0x009EC23C File Offset: 0x009EC23C
        internal static string PlatformNotSupported_UnixFileMode {
            get {
                return SR.GetResourceString("PlatformNotSupported_UnixFileMode");
            }
        }

        // Token: 0x17000712 RID: 1810
        // (get) Token: 0x060024FC RID: 9468 RVA: 0x009EC248 File Offset: 0x009EC248
        internal static string PlatformNotSupported_ITypeInfo {
            get {
                return SR.GetResourceString("PlatformNotSupported_ITypeInfo");
            }
        }

        // Token: 0x17000713 RID: 1811
        // (get) Token: 0x060024FD RID: 9469 RVA: 0x009EC254 File Offset: 0x009EC254
        internal static string PlatformNotSupported_IExpando {
            get {
                return SR.GetResourceString("PlatformNotSupported_IExpando");
            }
        }

        // Token: 0x17000714 RID: 1812
        // (get) Token: 0x060024FE RID: 9470 RVA: 0x009EC260 File Offset: 0x009EC260
        internal static string PlatformNotSupported_AppDomains {
            get {
                return SR.GetResourceString("PlatformNotSupported_AppDomains");
            }
        }

        // Token: 0x17000715 RID: 1813
        // (get) Token: 0x060024FF RID: 9471 RVA: 0x009EC26C File Offset: 0x009EC26C
        internal static string PlatformNotSupported_CAS {
            get {
                return SR.GetResourceString("PlatformNotSupported_CAS");
            }
        }

        // Token: 0x17000716 RID: 1814
        // (get) Token: 0x06002500 RID: 9472 RVA: 0x009EC278 File Offset: 0x009EC278
        internal static string PlatformNotSupported_ThreadAbort {
            get {
                return SR.GetResourceString("PlatformNotSupported_ThreadAbort");
            }
        }

        // Token: 0x17000717 RID: 1815
        // (get) Token: 0x06002501 RID: 9473 RVA: 0x009EC284 File Offset: 0x009EC284
        internal static string PlatformNotSupported_ThreadSuspend {
            get {
                return SR.GetResourceString("PlatformNotSupported_ThreadSuspend");
            }
        }

        // Token: 0x17000718 RID: 1816
        // (get) Token: 0x06002502 RID: 9474 RVA: 0x009EC290 File Offset: 0x009EC290
        internal static string PostconditionFailed {
            get {
                return SR.GetResourceString("PostconditionFailed");
            }
        }

        // Token: 0x17000719 RID: 1817
        // (get) Token: 0x06002503 RID: 9475 RVA: 0x009EC29C File Offset: 0x009EC29C
        internal static string PostconditionFailed_Cnd {
            get {
                return SR.GetResourceString("PostconditionFailed_Cnd");
            }
        }

        // Token: 0x1700071A RID: 1818
        // (get) Token: 0x06002504 RID: 9476 RVA: 0x009EC2A8 File Offset: 0x009EC2A8
        internal static string PostconditionOnExceptionFailed {
            get {
                return SR.GetResourceString("PostconditionOnExceptionFailed");
            }
        }

        // Token: 0x1700071B RID: 1819
        // (get) Token: 0x06002505 RID: 9477 RVA: 0x009EC2B4 File Offset: 0x009EC2B4
        internal static string PostconditionOnExceptionFailed_Cnd {
            get {
                return SR.GetResourceString("PostconditionOnExceptionFailed_Cnd");
            }
        }

        // Token: 0x1700071C RID: 1820
        // (get) Token: 0x06002506 RID: 9478 RVA: 0x009EC2C0 File Offset: 0x009EC2C0
        internal static string PreconditionFailed {
            get {
                return SR.GetResourceString("PreconditionFailed");
            }
        }

        // Token: 0x1700071D RID: 1821
        // (get) Token: 0x06002507 RID: 9479 RVA: 0x009EC2CC File Offset: 0x009EC2CC
        internal static string PreconditionFailed_Cnd {
            get {
                return SR.GetResourceString("PreconditionFailed_Cnd");
            }
        }

        // Token: 0x1700071E RID: 1822
        // (get) Token: 0x06002508 RID: 9480 RVA: 0x009EC2D8 File Offset: 0x009EC2D8
        internal static string Rank_MultiDimNotSupported {
            get {
                return SR.GetResourceString("Rank_MultiDimNotSupported");
            }
        }

        // Token: 0x1700071F RID: 1823
        // (get) Token: 0x06002509 RID: 9481 RVA: 0x009EC2E4 File Offset: 0x009EC2E4
        internal static string Rank_MustMatch {
            get {
                return SR.GetResourceString("Rank_MustMatch");
            }
        }

        // Token: 0x17000720 RID: 1824
        // (get) Token: 0x0600250A RID: 9482 RVA: 0x009EC2F0 File Offset: 0x009EC2F0
        internal static string ReflectionTypeLoad_LoadFailed {
            get {
                return SR.GetResourceString("ReflectionTypeLoad_LoadFailed");
            }
        }

        // Token: 0x17000721 RID: 1825
        // (get) Token: 0x0600250B RID: 9483 RVA: 0x009EC2FC File Offset: 0x009EC2FC
        internal static string ResourceReaderIsClosed {
            get {
                return SR.GetResourceString("ResourceReaderIsClosed");
            }
        }

        // Token: 0x17000722 RID: 1826
        // (get) Token: 0x0600250C RID: 9484 RVA: 0x009EC308 File Offset: 0x009EC308
        internal static string Resources_StreamNotValid {
            get {
                return SR.GetResourceString("Resources_StreamNotValid");
            }
        }

        // Token: 0x17000723 RID: 1827
        // (get) Token: 0x0600250D RID: 9485 RVA: 0x009EC314 File Offset: 0x009EC314
        internal static string InvalidFilterCriteriaException_CritInt {
            get {
                return SR.GetResourceString("InvalidFilterCriteriaException_CritInt");
            }
        }

        // Token: 0x17000724 RID: 1828
        // (get) Token: 0x0600250E RID: 9486 RVA: 0x009EC320 File Offset: 0x009EC320
        internal static string InvalidFilterCriteriaException_CritString {
            get {
                return SR.GetResourceString("InvalidFilterCriteriaException_CritString");
            }
        }

        // Token: 0x17000725 RID: 1829
        // (get) Token: 0x0600250F RID: 9487 RVA: 0x009EC32C File Offset: 0x009EC32C
        internal static string RFLCT_InvalidFieldFail {
            get {
                return SR.GetResourceString("RFLCT_InvalidFieldFail");
            }
        }

        // Token: 0x17000726 RID: 1830
        // (get) Token: 0x06002510 RID: 9488 RVA: 0x009EC338 File Offset: 0x009EC338
        internal static string RFLCT_InvalidPropFail {
            get {
                return SR.GetResourceString("RFLCT_InvalidPropFail");
            }
        }

        // Token: 0x17000727 RID: 1831
        // (get) Token: 0x06002511 RID: 9489 RVA: 0x009EC344 File Offset: 0x009EC344
        internal static string RFLCT_Targ_ITargMismatch_WithType {
            get {
                return SR.GetResourceString("RFLCT_Targ_ITargMismatch_WithType");
            }
        }

        // Token: 0x17000728 RID: 1832
        // (get) Token: 0x06002512 RID: 9490 RVA: 0x009EC350 File Offset: 0x009EC350
        internal static string RFLCT_Targ_StatFldReqTarg {
            get {
                return SR.GetResourceString("RFLCT_Targ_StatFldReqTarg");
            }
        }

        // Token: 0x17000729 RID: 1833
        // (get) Token: 0x06002513 RID: 9491 RVA: 0x009EC35C File Offset: 0x009EC35C
        internal static string RFLCT_Targ_StatMethReqTarg {
            get {
                return SR.GetResourceString("RFLCT_Targ_StatMethReqTarg");
            }
        }

        // Token: 0x1700072A RID: 1834
        // (get) Token: 0x06002514 RID: 9492 RVA: 0x009EC368 File Offset: 0x009EC368
        internal static string RuntimeInstanceNotAllowed {
            get {
                return SR.GetResourceString("RuntimeInstanceNotAllowed");
            }
        }

        // Token: 0x1700072B RID: 1835
        // (get) Token: 0x06002515 RID: 9493 RVA: 0x009EC374 File Offset: 0x009EC374
        internal static string RuntimeWrappedException {
            get {
                return SR.GetResourceString("RuntimeWrappedException");
            }
        }

        // Token: 0x1700072C RID: 1836
        // (get) Token: 0x06002516 RID: 9494 RVA: 0x009EC380 File Offset: 0x009EC380
        internal static string StandardOleMarshalObjectGetMarshalerFailed {
            get {
                return SR.GetResourceString("StandardOleMarshalObjectGetMarshalerFailed");
            }
        }

        // Token: 0x1700072D RID: 1837
        // (get) Token: 0x06002517 RID: 9495 RVA: 0x009EC38C File Offset: 0x009EC38C
        internal static string Security_CannotReadFileData {
            get {
                return SR.GetResourceString("Security_CannotReadFileData");
            }
        }

        // Token: 0x1700072E RID: 1838
        // (get) Token: 0x06002518 RID: 9496 RVA: 0x009EC398 File Offset: 0x009EC398
        internal static string Security_RegistryPermission {
            get {
                return SR.GetResourceString("Security_RegistryPermission");
            }
        }

        // Token: 0x1700072F RID: 1839
        // (get) Token: 0x06002519 RID: 9497 RVA: 0x009EC3A4 File Offset: 0x009EC3A4
        internal static string SemaphoreSlim_ctor_InitialCountWrong {
            get {
                return SR.GetResourceString("SemaphoreSlim_ctor_InitialCountWrong");
            }
        }

        // Token: 0x17000730 RID: 1840
        // (get) Token: 0x0600251A RID: 9498 RVA: 0x009EC3B0 File Offset: 0x009EC3B0
        internal static string SemaphoreSlim_ctor_MaxCountWrong {
            get {
                return SR.GetResourceString("SemaphoreSlim_ctor_MaxCountWrong");
            }
        }

        // Token: 0x17000731 RID: 1841
        // (get) Token: 0x0600251B RID: 9499 RVA: 0x009EC3BC File Offset: 0x009EC3BC
        internal static string SemaphoreSlim_Release_CountWrong {
            get {
                return SR.GetResourceString("SemaphoreSlim_Release_CountWrong");
            }
        }

        // Token: 0x17000732 RID: 1842
        // (get) Token: 0x0600251C RID: 9500 RVA: 0x009EC3C8 File Offset: 0x009EC3C8
        internal static string SemaphoreSlim_Wait_TimeoutWrong {
            get {
                return SR.GetResourceString("SemaphoreSlim_Wait_TimeoutWrong");
            }
        }

        // Token: 0x17000733 RID: 1843
        // (get) Token: 0x0600251D RID: 9501 RVA: 0x009EC3D4 File Offset: 0x009EC3D4
        internal static string SemaphoreSlim_Wait_TimeSpanTimeoutWrong {
            get {
                return SR.GetResourceString("SemaphoreSlim_Wait_TimeSpanTimeoutWrong");
            }
        }

        // Token: 0x17000734 RID: 1844
        // (get) Token: 0x0600251E RID: 9502 RVA: 0x009EC3E0 File Offset: 0x009EC3E0
        internal static string Serialization_BadParameterInfo {
            get {
                return SR.GetResourceString("Serialization_BadParameterInfo");
            }
        }

        // Token: 0x17000735 RID: 1845
        // (get) Token: 0x0600251F RID: 9503 RVA: 0x009EC3EC File Offset: 0x009EC3EC
        internal static string Serialization_CorruptField {
            get {
                return SR.GetResourceString("Serialization_CorruptField");
            }
        }

        // Token: 0x17000736 RID: 1846
        // (get) Token: 0x06002520 RID: 9504 RVA: 0x009EC3F8 File Offset: 0x009EC3F8
        internal static string Serialization_DateTimeTicksOutOfRange {
            get {
                return SR.GetResourceString("Serialization_DateTimeTicksOutOfRange");
            }
        }

        // Token: 0x17000737 RID: 1847
        // (get) Token: 0x06002521 RID: 9505 RVA: 0x009EC404 File Offset: 0x009EC404
        internal static string Serialization_DelegatesNotSupported {
            get {
                return SR.GetResourceString("Serialization_DelegatesNotSupported");
            }
        }

        // Token: 0x17000738 RID: 1848
        // (get) Token: 0x06002522 RID: 9506 RVA: 0x009EC410 File Offset: 0x009EC410
        internal static string Serialization_InsufficientState {
            get {
                return SR.GetResourceString("Serialization_InsufficientState");
            }
        }

        // Token: 0x17000739 RID: 1849
        // (get) Token: 0x06002523 RID: 9507 RVA: 0x009EC41C File Offset: 0x009EC41C
        internal static string Serialization_InvalidData {
            get {
                return SR.GetResourceString("Serialization_InvalidData");
            }
        }

        // Token: 0x1700073A RID: 1850
        // (get) Token: 0x06002524 RID: 9508 RVA: 0x009EC428 File Offset: 0x009EC428
        internal static string Serialization_InvalidEscapeSequence {
            get {
                return SR.GetResourceString("Serialization_InvalidEscapeSequence");
            }
        }

        // Token: 0x1700073B RID: 1851
        // (get) Token: 0x06002525 RID: 9509 RVA: 0x009EC434 File Offset: 0x009EC434
        internal static string Serialization_InvalidOnDeser {
            get {
                return SR.GetResourceString("Serialization_InvalidOnDeser");
            }
        }

        // Token: 0x1700073C RID: 1852
        // (get) Token: 0x06002526 RID: 9510 RVA: 0x009EC440 File Offset: 0x009EC440
        internal static string Serialization_InvalidType {
            get {
                return SR.GetResourceString("Serialization_InvalidType");
            }
        }

        // Token: 0x1700073D RID: 1853
        // (get) Token: 0x06002527 RID: 9511 RVA: 0x009EC44C File Offset: 0x009EC44C
        internal static string Serialization_KeyValueDifferentSizes {
            get {
                return SR.GetResourceString("Serialization_KeyValueDifferentSizes");
            }
        }

        // Token: 0x1700073E RID: 1854
        // (get) Token: 0x06002528 RID: 9512 RVA: 0x009EC458 File Offset: 0x009EC458
        internal static string Serialization_MissingDateTimeData {
            get {
                return SR.GetResourceString("Serialization_MissingDateTimeData");
            }
        }

        // Token: 0x1700073F RID: 1855
        // (get) Token: 0x06002529 RID: 9513 RVA: 0x009EC464 File Offset: 0x009EC464
        internal static string Serialization_MissingKeys {
            get {
                return SR.GetResourceString("Serialization_MissingKeys");
            }
        }

        // Token: 0x17000740 RID: 1856
        // (get) Token: 0x0600252A RID: 9514 RVA: 0x009EC470 File Offset: 0x009EC470
        internal static string Serialization_MissingValues {
            get {
                return SR.GetResourceString("Serialization_MissingValues");
            }
        }

        // Token: 0x17000741 RID: 1857
        // (get) Token: 0x0600252B RID: 9515 RVA: 0x009EC47C File Offset: 0x009EC47C
        internal static string Serialization_NoParameterInfo {
            get {
                return SR.GetResourceString("Serialization_NoParameterInfo");
            }
        }

        // Token: 0x17000742 RID: 1858
        // (get) Token: 0x0600252C RID: 9516 RVA: 0x009EC488 File Offset: 0x009EC488
        internal static string Serialization_NotFound {
            get {
                return SR.GetResourceString("Serialization_NotFound");
            }
        }

        // Token: 0x17000743 RID: 1859
        // (get) Token: 0x0600252D RID: 9517 RVA: 0x009EC494 File Offset: 0x009EC494
        internal static string Serialization_NullKey {
            get {
                return SR.GetResourceString("Serialization_NullKey");
            }
        }

        // Token: 0x17000744 RID: 1860
        // (get) Token: 0x0600252E RID: 9518 RVA: 0x009EC4A0 File Offset: 0x009EC4A0
        internal static string Serialization_OptionalFieldVersionValue {
            get {
                return SR.GetResourceString("Serialization_OptionalFieldVersionValue");
            }
        }

        // Token: 0x17000745 RID: 1861
        // (get) Token: 0x0600252F RID: 9519 RVA: 0x009EC4AC File Offset: 0x009EC4AC
        internal static string Serialization_SameNameTwice {
            get {
                return SR.GetResourceString("Serialization_SameNameTwice");
            }
        }

        // Token: 0x17000746 RID: 1862
        // (get) Token: 0x06002530 RID: 9520 RVA: 0x009EC4B8 File Offset: 0x009EC4B8
        internal static string Serialization_StringBuilderCapacity {
            get {
                return SR.GetResourceString("Serialization_StringBuilderCapacity");
            }
        }

        // Token: 0x17000747 RID: 1863
        // (get) Token: 0x06002531 RID: 9521 RVA: 0x009EC4C4 File Offset: 0x009EC4C4
        internal static string Serialization_StringBuilderMaxCapacity {
            get {
                return SR.GetResourceString("Serialization_StringBuilderMaxCapacity");
            }
        }

        // Token: 0x17000748 RID: 1864
        // (get) Token: 0x06002532 RID: 9522 RVA: 0x009EC4D0 File Offset: 0x009EC4D0
        internal static string Lock_Enter_LockRecursionException {
            get {
                return SR.GetResourceString("Lock_Enter_LockRecursionException");
            }
        }

        // Token: 0x17000749 RID: 1865
        // (get) Token: 0x06002533 RID: 9523 RVA: 0x009EC4DC File Offset: 0x009EC4DC
        internal static string Lock_Enter_WaiterCountOverflow_OutOfMemoryException {
            get {
                return SR.GetResourceString("Lock_Enter_WaiterCountOverflow_OutOfMemoryException");
            }
        }

        // Token: 0x1700074A RID: 1866
        // (get) Token: 0x06002534 RID: 9524 RVA: 0x009EC4E8 File Offset: 0x009EC4E8
        internal static string Lock_Exit_SynchronizationLockException {
            get {
                return SR.GetResourceString("Lock_Exit_SynchronizationLockException");
            }
        }

        // Token: 0x1700074B RID: 1867
        // (get) Token: 0x06002535 RID: 9525 RVA: 0x009EC4F4 File Offset: 0x009EC4F4
        internal static string NamedWaitHandles_IncompatibleNamePrefix {
            get {
                return SR.GetResourceString("NamedWaitHandles_IncompatibleNamePrefix");
            }
        }

        // Token: 0x1700074C RID: 1868
        // (get) Token: 0x06002536 RID: 9526 RVA: 0x009EC500 File Offset: 0x009EC500
        internal static string NamedWaitHandles_ExistingObjectIncompatibleWithCurrentUserOnly {
            get {
                return SR.GetResourceString("NamedWaitHandles_ExistingObjectIncompatibleWithCurrentUserOnly");
            }
        }

        // Token: 0x1700074D RID: 1869
        // (get) Token: 0x06002537 RID: 9527 RVA: 0x009EC50C File Offset: 0x009EC50C
        internal static string SpinLock_IsHeldByCurrentThread {
            get {
                return SR.GetResourceString("SpinLock_IsHeldByCurrentThread");
            }
        }

        // Token: 0x1700074E RID: 1870
        // (get) Token: 0x06002538 RID: 9528 RVA: 0x009EC518 File Offset: 0x009EC518
        internal static string SpinLock_TryEnter_ArgumentOutOfRange {
            get {
                return SR.GetResourceString("SpinLock_TryEnter_ArgumentOutOfRange");
            }
        }

        // Token: 0x1700074F RID: 1871
        // (get) Token: 0x06002539 RID: 9529 RVA: 0x009EC524 File Offset: 0x009EC524
        internal static string SpinLock_TryEnter_LockRecursionException {
            get {
                return SR.GetResourceString("SpinLock_TryEnter_LockRecursionException");
            }
        }

        // Token: 0x17000750 RID: 1872
        // (get) Token: 0x0600253A RID: 9530 RVA: 0x009EC530 File Offset: 0x009EC530
        internal static string SpinLock_TryReliableEnter_ArgumentException {
            get {
                return SR.GetResourceString("SpinLock_TryReliableEnter_ArgumentException");
            }
        }

        // Token: 0x17000751 RID: 1873
        // (get) Token: 0x0600253B RID: 9531 RVA: 0x009EC53C File Offset: 0x009EC53C
        internal static string SpinWait_SpinUntil_TimeoutWrong {
            get {
                return SR.GetResourceString("SpinWait_SpinUntil_TimeoutWrong");
            }
        }

        // Token: 0x17000752 RID: 1874
        // (get) Token: 0x0600253C RID: 9532 RVA: 0x009EC548 File Offset: 0x009EC548
        internal static string StackTrace_InFileILOffset {
            get {
                return SR.GetResourceString("StackTrace_InFileILOffset");
            }
        }

        // Token: 0x17000753 RID: 1875
        // (get) Token: 0x0600253D RID: 9533 RVA: 0x009EC554 File Offset: 0x009EC554
        internal static string StackTrace_InFileLineNumber {
            get {
                return SR.GetResourceString("StackTrace_InFileLineNumber");
            }
        }

        // Token: 0x17000754 RID: 1876
        // (get) Token: 0x0600253E RID: 9534 RVA: 0x009EC560 File Offset: 0x009EC560
        internal static string Task_ContinueWith_ESandLR {
            get {
                return SR.GetResourceString("Task_ContinueWith_ESandLR");
            }
        }

        // Token: 0x17000755 RID: 1877
        // (get) Token: 0x0600253F RID: 9535 RVA: 0x009EC56C File Offset: 0x009EC56C
        internal static string Task_ContinueWith_NotOnAnything {
            get {
                return SR.GetResourceString("Task_ContinueWith_NotOnAnything");
            }
        }

        // Token: 0x17000756 RID: 1878
        // (get) Token: 0x06002540 RID: 9536 RVA: 0x009EC578 File Offset: 0x009EC578
        internal static string Task_InvalidTimerTimeSpan {
            get {
                return SR.GetResourceString("Task_InvalidTimerTimeSpan");
            }
        }

        // Token: 0x17000757 RID: 1879
        // (get) Token: 0x06002541 RID: 9537 RVA: 0x009EC584 File Offset: 0x009EC584
        internal static string Task_Delay_InvalidMillisecondsDelay {
            get {
                return SR.GetResourceString("Task_Delay_InvalidMillisecondsDelay");
            }
        }

        // Token: 0x17000758 RID: 1880
        // (get) Token: 0x06002542 RID: 9538 RVA: 0x009EC590 File Offset: 0x009EC590
        internal static string Task_Dispose_NotCompleted {
            get {
                return SR.GetResourceString("Task_Dispose_NotCompleted");
            }
        }

        // Token: 0x17000759 RID: 1881
        // (get) Token: 0x06002543 RID: 9539 RVA: 0x009EC59C File Offset: 0x009EC59C
        internal static string Task_FromAsync_LongRunning {
            get {
                return SR.GetResourceString("Task_FromAsync_LongRunning");
            }
        }

        // Token: 0x1700075A RID: 1882
        // (get) Token: 0x06002544 RID: 9540 RVA: 0x009EC5A8 File Offset: 0x009EC5A8
        internal static string Task_FromAsync_PreferFairness {
            get {
                return SR.GetResourceString("Task_FromAsync_PreferFairness");
            }
        }

        // Token: 0x1700075B RID: 1883
        // (get) Token: 0x06002545 RID: 9541 RVA: 0x009EC5B4 File Offset: 0x009EC5B4
        internal static string Task_MultiTaskContinuation_EmptyTaskList {
            get {
                return SR.GetResourceString("Task_MultiTaskContinuation_EmptyTaskList");
            }
        }

        // Token: 0x1700075C RID: 1884
        // (get) Token: 0x06002546 RID: 9542 RVA: 0x009EC5C0 File Offset: 0x009EC5C0
        internal static string Task_MultiTaskContinuation_FireOptions {
            get {
                return SR.GetResourceString("Task_MultiTaskContinuation_FireOptions");
            }
        }

        // Token: 0x1700075D RID: 1885
        // (get) Token: 0x06002547 RID: 9543 RVA: 0x009EC5CC File Offset: 0x009EC5CC
        internal static string Task_MultiTaskContinuation_NullTask {
            get {
                return SR.GetResourceString("Task_MultiTaskContinuation_NullTask");
            }
        }

        // Token: 0x1700075E RID: 1886
        // (get) Token: 0x06002548 RID: 9544 RVA: 0x009EC5D8 File Offset: 0x009EC5D8
        internal static string Task_RunSynchronously_AlreadyStarted {
            get {
                return SR.GetResourceString("Task_RunSynchronously_AlreadyStarted");
            }
        }

        // Token: 0x1700075F RID: 1887
        // (get) Token: 0x06002549 RID: 9545 RVA: 0x009EC5E4 File Offset: 0x009EC5E4
        internal static string Task_RunSynchronously_Continuation {
            get {
                return SR.GetResourceString("Task_RunSynchronously_Continuation");
            }
        }

        // Token: 0x17000760 RID: 1888
        // (get) Token: 0x0600254A RID: 9546 RVA: 0x009EC5F0 File Offset: 0x009EC5F0
        internal static string Task_RunSynchronously_Promise {
            get {
                return SR.GetResourceString("Task_RunSynchronously_Promise");
            }
        }

        // Token: 0x17000761 RID: 1889
        // (get) Token: 0x0600254B RID: 9547 RVA: 0x009EC5FC File Offset: 0x009EC5FC
        internal static string Task_RunSynchronously_TaskCompleted {
            get {
                return SR.GetResourceString("Task_RunSynchronously_TaskCompleted");
            }
        }

        // Token: 0x17000762 RID: 1890
        // (get) Token: 0x0600254C RID: 9548 RVA: 0x009EC608 File Offset: 0x009EC608
        internal static string Task_Start_AlreadyStarted {
            get {
                return SR.GetResourceString("Task_Start_AlreadyStarted");
            }
        }

        // Token: 0x17000763 RID: 1891
        // (get) Token: 0x0600254D RID: 9549 RVA: 0x009EC614 File Offset: 0x009EC614
        internal static string Task_Start_ContinuationTask {
            get {
                return SR.GetResourceString("Task_Start_ContinuationTask");
            }
        }

        // Token: 0x17000764 RID: 1892
        // (get) Token: 0x0600254E RID: 9550 RVA: 0x009EC620 File Offset: 0x009EC620
        internal static string Task_Start_Promise {
            get {
                return SR.GetResourceString("Task_Start_Promise");
            }
        }

        // Token: 0x17000765 RID: 1893
        // (get) Token: 0x0600254F RID: 9551 RVA: 0x009EC62C File Offset: 0x009EC62C
        internal static string Task_Start_TaskCompleted {
            get {
                return SR.GetResourceString("Task_Start_TaskCompleted");
            }
        }

        // Token: 0x17000766 RID: 1894
        // (get) Token: 0x06002550 RID: 9552 RVA: 0x009EC638 File Offset: 0x009EC638
        internal static string Task_ThrowIfDisposed {
            get {
                return SR.GetResourceString("Task_ThrowIfDisposed");
            }
        }

        // Token: 0x17000767 RID: 1895
        // (get) Token: 0x06002551 RID: 9553 RVA: 0x009EC644 File Offset: 0x009EC644
        internal static string Task_WaitMulti_NullTask {
            get {
                return SR.GetResourceString("Task_WaitMulti_NullTask");
            }
        }

        // Token: 0x17000768 RID: 1896
        // (get) Token: 0x06002552 RID: 9554 RVA: 0x009EC650 File Offset: 0x009EC650
        internal static string Task_MustBeCompleted {
            get {
                return SR.GetResourceString("Task_MustBeCompleted");
            }
        }

        // Token: 0x17000769 RID: 1897
        // (get) Token: 0x06002553 RID: 9555 RVA: 0x009EC65C File Offset: 0x009EC65C
        internal static string TaskT_ConfigureAwait_InvalidOptions {
            get {
                return SR.GetResourceString("TaskT_ConfigureAwait_InvalidOptions");
            }
        }

        // Token: 0x1700076A RID: 1898
        // (get) Token: 0x06002554 RID: 9556 RVA: 0x009EC668 File Offset: 0x009EC668
        internal static string TaskCanceledException_ctor_DefaultMessage {
            get {
                return SR.GetResourceString("TaskCanceledException_ctor_DefaultMessage");
            }
        }

        // Token: 0x1700076B RID: 1899
        // (get) Token: 0x06002555 RID: 9557 RVA: 0x009EC674 File Offset: 0x009EC674
        internal static string TaskCompletionSourceT_TrySetException_NoExceptions {
            get {
                return SR.GetResourceString("TaskCompletionSourceT_TrySetException_NoExceptions");
            }
        }

        // Token: 0x1700076C RID: 1900
        // (get) Token: 0x06002556 RID: 9558 RVA: 0x009EC680 File Offset: 0x009EC680
        internal static string TaskCompletionSourceT_TrySetException_NullException {
            get {
                return SR.GetResourceString("TaskCompletionSourceT_TrySetException_NullException");
            }
        }

        // Token: 0x1700076D RID: 1901
        // (get) Token: 0x06002557 RID: 9559 RVA: 0x009EC68C File Offset: 0x009EC68C
        internal static string TaskExceptionHolder_UnhandledException {
            get {
                return SR.GetResourceString("TaskExceptionHolder_UnhandledException");
            }
        }

        // Token: 0x1700076E RID: 1902
        // (get) Token: 0x06002558 RID: 9560 RVA: 0x009EC698 File Offset: 0x009EC698
        internal static string TaskExceptionHolder_UnknownExceptionType {
            get {
                return SR.GetResourceString("TaskExceptionHolder_UnknownExceptionType");
            }
        }

        // Token: 0x1700076F RID: 1903
        // (get) Token: 0x06002559 RID: 9561 RVA: 0x009EC6A4 File Offset: 0x009EC6A4
        internal static string TaskScheduler_ExecuteTask_WrongTaskScheduler {
            get {
                return SR.GetResourceString("TaskScheduler_ExecuteTask_WrongTaskScheduler");
            }
        }

        // Token: 0x17000770 RID: 1904
        // (get) Token: 0x0600255A RID: 9562 RVA: 0x009EC6B0 File Offset: 0x009EC6B0
        internal static string TaskScheduler_FromCurrentSynchronizationContext_NoCurrent {
            get {
                return SR.GetResourceString("TaskScheduler_FromCurrentSynchronizationContext_NoCurrent");
            }
        }

        // Token: 0x17000771 RID: 1905
        // (get) Token: 0x0600255B RID: 9563 RVA: 0x009EC6BC File Offset: 0x009EC6BC
        internal static string TaskScheduler_InconsistentStateAfterTryExecuteTaskInline {
            get {
                return SR.GetResourceString("TaskScheduler_InconsistentStateAfterTryExecuteTaskInline");
            }
        }

        // Token: 0x17000772 RID: 1906
        // (get) Token: 0x0600255C RID: 9564 RVA: 0x009EC6C8 File Offset: 0x009EC6C8
        internal static string TaskSchedulerException_ctor_DefaultMessage {
            get {
                return SR.GetResourceString("TaskSchedulerException_ctor_DefaultMessage");
            }
        }

        // Token: 0x17000773 RID: 1907
        // (get) Token: 0x0600255D RID: 9565 RVA: 0x009EC6D4 File Offset: 0x009EC6D4
        internal static string TaskT_DebuggerNoResult {
            get {
                return SR.GetResourceString("TaskT_DebuggerNoResult");
            }
        }

        // Token: 0x17000774 RID: 1908
        // (get) Token: 0x0600255E RID: 9566 RVA: 0x009EC6E0 File Offset: 0x009EC6E0
        internal static string TaskT_TransitionToFinal_AlreadyCompleted {
            get {
                return SR.GetResourceString("TaskT_TransitionToFinal_AlreadyCompleted");
            }
        }

        // Token: 0x17000775 RID: 1909
        // (get) Token: 0x0600255F RID: 9567 RVA: 0x009EC6EC File Offset: 0x009EC6EC
        internal static string Thread_ApartmentState_ChangeFailed {
            get {
                return SR.GetResourceString("Thread_ApartmentState_ChangeFailed");
            }
        }

        // Token: 0x17000776 RID: 1910
        // (get) Token: 0x06002560 RID: 9568 RVA: 0x009EC6F8 File Offset: 0x009EC6F8
        internal static string Thread_GetSetCompressedStack_NotSupported {
            get {
                return SR.GetResourceString("Thread_GetSetCompressedStack_NotSupported");
            }
        }

        // Token: 0x17000777 RID: 1911
        // (get) Token: 0x06002561 RID: 9569 RVA: 0x009EC704 File Offset: 0x009EC704
        internal static string Thread_Operation_RequiresCurrentThread {
            get {
                return SR.GetResourceString("Thread_Operation_RequiresCurrentThread");
            }
        }

        // Token: 0x17000778 RID: 1912
        // (get) Token: 0x06002562 RID: 9570 RVA: 0x009EC710 File Offset: 0x009EC710
        internal static string Threading_AbandonedMutexException {
            get {
                return SR.GetResourceString("Threading_AbandonedMutexException");
            }
        }

        // Token: 0x17000779 RID: 1913
        // (get) Token: 0x06002563 RID: 9571 RVA: 0x009EC71C File Offset: 0x009EC71C
        internal static string Threading_WaitHandleCannotBeOpenedException {
            get {
                return SR.GetResourceString("Threading_WaitHandleCannotBeOpenedException");
            }
        }

        // Token: 0x1700077A RID: 1914
        // (get) Token: 0x06002564 RID: 9572 RVA: 0x009EC728 File Offset: 0x009EC728
        internal static string Threading_WaitHandleCannotBeOpenedException_InvalidHandle {
            get {
                return SR.GetResourceString("Threading_WaitHandleCannotBeOpenedException_InvalidHandle");
            }
        }

        // Token: 0x1700077B RID: 1915
        // (get) Token: 0x06002565 RID: 9573 RVA: 0x009EC734 File Offset: 0x009EC734
        internal static string Threading_WaitHandleTooManyPosts {
            get {
                return SR.GetResourceString("Threading_WaitHandleTooManyPosts");
            }
        }

        // Token: 0x1700077C RID: 1916
        // (get) Token: 0x06002566 RID: 9574 RVA: 0x009EC740 File Offset: 0x009EC740
        internal static string Threading_SemaphoreFullException {
            get {
                return SR.GetResourceString("Threading_SemaphoreFullException");
            }
        }

        // Token: 0x1700077D RID: 1917
        // (get) Token: 0x06002567 RID: 9575 RVA: 0x009EC74C File Offset: 0x009EC74C
        internal static string ThreadLocal_Value_RecursiveCallsToValue {
            get {
                return SR.GetResourceString("ThreadLocal_Value_RecursiveCallsToValue");
            }
        }

        // Token: 0x1700077E RID: 1918
        // (get) Token: 0x06002568 RID: 9576 RVA: 0x009EC758 File Offset: 0x009EC758
        internal static string ThreadLocal_ValuesNotAvailable {
            get {
                return SR.GetResourceString("ThreadLocal_ValuesNotAvailable");
            }
        }

        // Token: 0x1700077F RID: 1919
        // (get) Token: 0x06002569 RID: 9577 RVA: 0x009EC764 File Offset: 0x009EC764
        internal static string TimeZoneNotFound_MissingData {
            get {
                return SR.GetResourceString("TimeZoneNotFound_MissingData");
            }
        }

        // Token: 0x17000780 RID: 1920
        // (get) Token: 0x0600256A RID: 9578 RVA: 0x009EC770 File Offset: 0x009EC770
        internal static string TypeInitialization_Default {
            get {
                return SR.GetResourceString("TypeInitialization_Default");
            }
        }

        // Token: 0x17000781 RID: 1921
        // (get) Token: 0x0600256B RID: 9579 RVA: 0x009EC77C File Offset: 0x009EC77C
        internal static string TypeInitialization_Type {
            get {
                return SR.GetResourceString("TypeInitialization_Type");
            }
        }

        // Token: 0x17000782 RID: 1922
        // (get) Token: 0x0600256C RID: 9580 RVA: 0x009EC788 File Offset: 0x009EC788
        internal static string TypeLoad_ResolveNestedType {
            get {
                return SR.GetResourceString("TypeLoad_ResolveNestedType");
            }
        }

        // Token: 0x17000783 RID: 1923
        // (get) Token: 0x0600256D RID: 9581 RVA: 0x009EC794 File Offset: 0x009EC794
        internal static string TypeLoad_ResolveType {
            get {
                return SR.GetResourceString("TypeLoad_ResolveType");
            }
        }

        // Token: 0x17000784 RID: 1924
        // (get) Token: 0x0600256E RID: 9582 RVA: 0x009EC7A0 File Offset: 0x009EC7A0
        internal static string TypeLoad_ResolveTypeFromAssembly {
            get {
                return SR.GetResourceString("TypeLoad_ResolveTypeFromAssembly");
            }
        }

        // Token: 0x17000785 RID: 1925
        // (get) Token: 0x0600256F RID: 9583 RVA: 0x009EC7AC File Offset: 0x009EC7AC
        internal static string UnauthorizedAccess_IODenied_NoPathName {
            get {
                return SR.GetResourceString("UnauthorizedAccess_IODenied_NoPathName");
            }
        }

        // Token: 0x17000786 RID: 1926
        // (get) Token: 0x06002570 RID: 9584 RVA: 0x009EC7B8 File Offset: 0x009EC7B8
        internal static string UnauthorizedAccess_IODenied_Path {
            get {
                return SR.GetResourceString("UnauthorizedAccess_IODenied_Path");
            }
        }

        // Token: 0x17000787 RID: 1927
        // (get) Token: 0x06002571 RID: 9585 RVA: 0x009EC7C4 File Offset: 0x009EC7C4
        internal static string UnauthorizedAccess_MemStreamBuffer {
            get {
                return SR.GetResourceString("UnauthorizedAccess_MemStreamBuffer");
            }
        }

        // Token: 0x17000788 RID: 1928
        // (get) Token: 0x06002572 RID: 9586 RVA: 0x009EC7D0 File Offset: 0x009EC7D0
        internal static string UnauthorizedAccess_RegistryKeyGeneric_Key {
            get {
                return SR.GetResourceString("UnauthorizedAccess_RegistryKeyGeneric_Key");
            }
        }

        // Token: 0x17000789 RID: 1929
        // (get) Token: 0x06002573 RID: 9587 RVA: 0x009EC7DC File Offset: 0x009EC7DC
        internal static string UnknownError_Num {
            get {
                return SR.GetResourceString("UnknownError_Num");
            }
        }

        // Token: 0x1700078A RID: 1930
        // (get) Token: 0x06002574 RID: 9588 RVA: 0x009EC7E8 File Offset: 0x009EC7E8
        internal static string Verification_Exception {
            get {
                return SR.GetResourceString("Verification_Exception");
            }
        }

        // Token: 0x1700078B RID: 1931
        // (get) Token: 0x06002575 RID: 9589 RVA: 0x009EC7F4 File Offset: 0x009EC7F4
        internal static string Word_At {
            get {
                return SR.GetResourceString("Word_At");
            }
        }

        // Token: 0x1700078C RID: 1932
        // (get) Token: 0x06002576 RID: 9590 RVA: 0x009EC800 File Offset: 0x009EC800
        internal static string DebugAssertBanner {
            get {
                return SR.GetResourceString("DebugAssertBanner");
            }
        }

        // Token: 0x1700078D RID: 1933
        // (get) Token: 0x06002577 RID: 9591 RVA: 0x009EC80C File Offset: 0x009EC80C
        internal static string DebugAssertLongMessage {
            get {
                return SR.GetResourceString("DebugAssertLongMessage");
            }
        }

        // Token: 0x1700078E RID: 1934
        // (get) Token: 0x06002578 RID: 9592 RVA: 0x009EC818 File Offset: 0x009EC818
        internal static string DebugAssertShortMessage {
            get {
                return SR.GetResourceString("DebugAssertShortMessage");
            }
        }

        // Token: 0x1700078F RID: 1935
        // (get) Token: 0x06002579 RID: 9593 RVA: 0x009EC824 File Offset: 0x009EC824
        internal static string LockRecursionException_ReadAfterWriteNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_ReadAfterWriteNotAllowed");
            }
        }

        // Token: 0x17000790 RID: 1936
        // (get) Token: 0x0600257A RID: 9594 RVA: 0x009EC830 File Offset: 0x009EC830
        internal static string LockRecursionException_RecursiveReadNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_RecursiveReadNotAllowed");
            }
        }

        // Token: 0x17000791 RID: 1937
        // (get) Token: 0x0600257B RID: 9595 RVA: 0x009EC83C File Offset: 0x009EC83C
        internal static string LockRecursionException_RecursiveWriteNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_RecursiveWriteNotAllowed");
            }
        }

        // Token: 0x17000792 RID: 1938
        // (get) Token: 0x0600257C RID: 9596 RVA: 0x009EC848 File Offset: 0x009EC848
        internal static string LockRecursionException_RecursiveUpgradeNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_RecursiveUpgradeNotAllowed");
            }
        }

        // Token: 0x17000793 RID: 1939
        // (get) Token: 0x0600257D RID: 9597 RVA: 0x009EC854 File Offset: 0x009EC854
        internal static string LockRecursionException_WriteAfterReadNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_WriteAfterReadNotAllowed");
            }
        }

        // Token: 0x17000794 RID: 1940
        // (get) Token: 0x0600257E RID: 9598 RVA: 0x009EC860 File Offset: 0x009EC860
        internal static string SynchronizationLockException_MisMatchedUpgrade {
            get {
                return SR.GetResourceString("SynchronizationLockException_MisMatchedUpgrade");
            }
        }

        // Token: 0x17000795 RID: 1941
        // (get) Token: 0x0600257F RID: 9599 RVA: 0x009EC86C File Offset: 0x009EC86C
        internal static string SynchronizationLockException_MisMatchedRead {
            get {
                return SR.GetResourceString("SynchronizationLockException_MisMatchedRead");
            }
        }

        // Token: 0x17000796 RID: 1942
        // (get) Token: 0x06002580 RID: 9600 RVA: 0x009EC878 File Offset: 0x009EC878
        internal static string SynchronizationLockException_IncorrectDispose {
            get {
                return SR.GetResourceString("SynchronizationLockException_IncorrectDispose");
            }
        }

        // Token: 0x17000797 RID: 1943
        // (get) Token: 0x06002581 RID: 9601 RVA: 0x009EC884 File Offset: 0x009EC884
        internal static string LockRecursionException_UpgradeAfterReadNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_UpgradeAfterReadNotAllowed");
            }
        }

        // Token: 0x17000798 RID: 1944
        // (get) Token: 0x06002582 RID: 9602 RVA: 0x009EC890 File Offset: 0x009EC890
        internal static string LockRecursionException_UpgradeAfterWriteNotAllowed {
            get {
                return SR.GetResourceString("LockRecursionException_UpgradeAfterWriteNotAllowed");
            }
        }

        // Token: 0x17000799 RID: 1945
        // (get) Token: 0x06002583 RID: 9603 RVA: 0x009EC89C File Offset: 0x009EC89C
        internal static string SynchronizationLockException_MisMatchedWrite {
            get {
                return SR.GetResourceString("SynchronizationLockException_MisMatchedWrite");
            }
        }

        // Token: 0x1700079A RID: 1946
        // (get) Token: 0x06002584 RID: 9604 RVA: 0x009EC8A8 File Offset: 0x009EC8A8
        internal static string NotSupported_SignatureType {
            get {
                return SR.GetResourceString("NotSupported_SignatureType");
            }
        }

        // Token: 0x1700079B RID: 1947
        // (get) Token: 0x06002585 RID: 9605 RVA: 0x009EC8B4 File Offset: 0x009EC8B4
        internal static string HashCode_HashCodeNotSupported {
            get {
                return SR.GetResourceString("HashCode_HashCodeNotSupported");
            }
        }

        // Token: 0x1700079C RID: 1948
        // (get) Token: 0x06002586 RID: 9606 RVA: 0x009EC8C0 File Offset: 0x009EC8C0
        internal static string HashCode_EqualityNotSupported {
            get {
                return SR.GetResourceString("HashCode_EqualityNotSupported");
            }
        }

        // Token: 0x1700079D RID: 1949
        // (get) Token: 0x06002587 RID: 9607 RVA: 0x009EC8CC File Offset: 0x009EC8CC
        internal static string Arg_TypeNotSupported {
            get {
                return SR.GetResourceString("Arg_TypeNotSupported");
            }
        }

        // Token: 0x1700079E RID: 1950
        // (get) Token: 0x06002588 RID: 9608 RVA: 0x009EC8D8 File Offset: 0x009EC8D8
        internal static string IO_InvalidReadLength {
            get {
                return SR.GetResourceString("IO_InvalidReadLength");
            }
        }

        // Token: 0x1700079F RID: 1951
        // (get) Token: 0x06002589 RID: 9609 RVA: 0x009EC8E4 File Offset: 0x009EC8E4
        internal static string Arg_BasePathNotFullyQualified {
            get {
                return SR.GetResourceString("Arg_BasePathNotFullyQualified");
            }
        }

        // Token: 0x170007A0 RID: 1952
        // (get) Token: 0x0600258A RID: 9610 RVA: 0x009EC8F0 File Offset: 0x009EC8F0
        internal static string Arg_NullArgumentNullRef {
            get {
                return SR.GetResourceString("Arg_NullArgumentNullRef");
            }
        }

        // Token: 0x170007A1 RID: 1953
        // (get) Token: 0x0600258B RID: 9611 RVA: 0x009EC8FC File Offset: 0x009EC8FC
        internal static string Argument_AggressiveGCRequiresMaxGeneration {
            get {
                return SR.GetResourceString("Argument_AggressiveGCRequiresMaxGeneration");
            }
        }

        // Token: 0x170007A2 RID: 1954
        // (get) Token: 0x0600258C RID: 9612 RVA: 0x009EC908 File Offset: 0x009EC908
        internal static string Argument_AggressiveGCRequiresBlocking {
            get {
                return SR.GetResourceString("Argument_AggressiveGCRequiresBlocking");
            }
        }

        // Token: 0x170007A3 RID: 1955
        // (get) Token: 0x0600258D RID: 9613 RVA: 0x009EC914 File Offset: 0x009EC914
        internal static string Argument_AggressiveGCRequiresCompacting {
            get {
                return SR.GetResourceString("Argument_AggressiveGCRequiresCompacting");
            }
        }

        // Token: 0x170007A4 RID: 1956
        // (get) Token: 0x0600258E RID: 9614 RVA: 0x009EC920 File Offset: 0x009EC920
        internal static string Argument_OverlapAlignmentMismatch {
            get {
                return SR.GetResourceString("Argument_OverlapAlignmentMismatch");
            }
        }

        // Token: 0x170007A5 RID: 1957
        // (get) Token: 0x0600258F RID: 9615 RVA: 0x009EC92C File Offset: 0x009EC92C
        internal static string Arg_MustBeNullTerminatedString {
            get {
                return SR.GetResourceString("Arg_MustBeNullTerminatedString");
            }
        }

        // Token: 0x170007A6 RID: 1958
        // (get) Token: 0x06002590 RID: 9616 RVA: 0x009EC938 File Offset: 0x009EC938
        internal static string ArgumentOutOfRange_Week_ISO {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_Week_ISO");
            }
        }

        // Token: 0x170007A7 RID: 1959
        // (get) Token: 0x06002591 RID: 9617 RVA: 0x009EC944 File Offset: 0x009EC944
        internal static string Argument_BadPInvokeMethod {
            get {
                return SR.GetResourceString("Argument_BadPInvokeMethod");
            }
        }

        // Token: 0x170007A8 RID: 1960
        // (get) Token: 0x06002592 RID: 9618 RVA: 0x009EC950 File Offset: 0x009EC950
        internal static string Argument_BadPInvokeOnInterface {
            get {
                return SR.GetResourceString("Argument_BadPInvokeOnInterface");
            }
        }

        // Token: 0x170007A9 RID: 1961
        // (get) Token: 0x06002593 RID: 9619 RVA: 0x009EC95C File Offset: 0x009EC95C
        internal static string Argument_MethodRedefined {
            get {
                return SR.GetResourceString("Argument_MethodRedefined");
            }
        }

        // Token: 0x170007AA RID: 1962
        // (get) Token: 0x06002594 RID: 9620 RVA: 0x009EC968 File Offset: 0x009EC968
        internal static string Argument_CannotExtractScalar {
            get {
                return SR.GetResourceString("Argument_CannotExtractScalar");
            }
        }

        // Token: 0x170007AB RID: 1963
        // (get) Token: 0x06002595 RID: 9621 RVA: 0x009EC974 File Offset: 0x009EC974
        internal static string Argument_CannotParsePrecision {
            get {
                return SR.GetResourceString("Argument_CannotParsePrecision");
            }
        }

        // Token: 0x170007AC RID: 1964
        // (get) Token: 0x06002596 RID: 9622 RVA: 0x009EC980 File Offset: 0x009EC980
        internal static string Argument_GWithPrecisionNotSupported {
            get {
                return SR.GetResourceString("Argument_GWithPrecisionNotSupported");
            }
        }

        // Token: 0x170007AD RID: 1965
        // (get) Token: 0x06002597 RID: 9623 RVA: 0x009EC98C File Offset: 0x009EC98C
        internal static string Argument_PrecisionTooLarge {
            get {
                return SR.GetResourceString("Argument_PrecisionTooLarge");
            }
        }

        // Token: 0x170007AE RID: 1966
        // (get) Token: 0x06002598 RID: 9624 RVA: 0x009EC998 File Offset: 0x009EC998
        internal static string AssemblyDependencyResolver_FailedToLoadHostpolicy {
            get {
                return SR.GetResourceString("AssemblyDependencyResolver_FailedToLoadHostpolicy");
            }
        }

        // Token: 0x170007AF RID: 1967
        // (get) Token: 0x06002599 RID: 9625 RVA: 0x009EC9A4 File Offset: 0x009EC9A4
        internal static string AssemblyDependencyResolver_FailedToResolveDependencies {
            get {
                return SR.GetResourceString("AssemblyDependencyResolver_FailedToResolveDependencies");
            }
        }

        // Token: 0x170007B0 RID: 1968
        // (get) Token: 0x0600259A RID: 9626 RVA: 0x009EC9B0 File Offset: 0x009EC9B0
        internal static string Arg_EnumNotCloneable {
            get {
                return SR.GetResourceString("Arg_EnumNotCloneable");
            }
        }

        // Token: 0x170007B1 RID: 1969
        // (get) Token: 0x0600259B RID: 9627 RVA: 0x009EC9BC File Offset: 0x009EC9BC
        internal static string InvalidOp_InvalidNewEnumVariant {
            get {
                return SR.GetResourceString("InvalidOp_InvalidNewEnumVariant");
            }
        }

        // Token: 0x170007B2 RID: 1970
        // (get) Token: 0x0600259C RID: 9628 RVA: 0x009EC9C8 File Offset: 0x009EC9C8
        internal static string Argument_StructArrayTooLarge {
            get {
                return SR.GetResourceString("Argument_StructArrayTooLarge");
            }
        }

        // Token: 0x170007B3 RID: 1971
        // (get) Token: 0x0600259D RID: 9629 RVA: 0x009EC9D4 File Offset: 0x009EC9D4
        internal static string IndexOutOfRange_ArrayWithOffset {
            get {
                return SR.GetResourceString("IndexOutOfRange_ArrayWithOffset");
            }
        }

        // Token: 0x170007B4 RID: 1972
        // (get) Token: 0x0600259E RID: 9630 RVA: 0x009EC9E0 File Offset: 0x009EC9E0
        internal static string Serialization_DangerousDeserialization_Switch {
            get {
                return SR.GetResourceString("Serialization_DangerousDeserialization_Switch");
            }
        }

        // Token: 0x170007B5 RID: 1973
        // (get) Token: 0x0600259F RID: 9631 RVA: 0x009EC9EC File Offset: 0x009EC9EC
        internal static string Argument_InvalidStartupHookSimpleAssemblyName {
            get {
                return SR.GetResourceString("Argument_InvalidStartupHookSimpleAssemblyName");
            }
        }

        // Token: 0x170007B6 RID: 1974
        // (get) Token: 0x060025A0 RID: 9632 RVA: 0x009EC9F8 File Offset: 0x009EC9F8
        internal static string Argument_StartupHookAssemblyLoadFailed {
            get {
                return SR.GetResourceString("Argument_StartupHookAssemblyLoadFailed");
            }
        }

        // Token: 0x170007B7 RID: 1975
        // (get) Token: 0x060025A1 RID: 9633 RVA: 0x009ECA04 File Offset: 0x009ECA04
        internal static string InvalidOperation_NonStaticComRegFunction {
            get {
                return SR.GetResourceString("InvalidOperation_NonStaticComRegFunction");
            }
        }

        // Token: 0x170007B8 RID: 1976
        // (get) Token: 0x060025A2 RID: 9634 RVA: 0x009ECA10 File Offset: 0x009ECA10
        internal static string InvalidOperation_NonStaticComUnRegFunction {
            get {
                return SR.GetResourceString("InvalidOperation_NonStaticComUnRegFunction");
            }
        }

        // Token: 0x170007B9 RID: 1977
        // (get) Token: 0x060025A3 RID: 9635 RVA: 0x009ECA1C File Offset: 0x009ECA1C
        internal static string InvalidOperation_InvalidComRegFunctionSig {
            get {
                return SR.GetResourceString("InvalidOperation_InvalidComRegFunctionSig");
            }
        }

        // Token: 0x170007BA RID: 1978
        // (get) Token: 0x060025A4 RID: 9636 RVA: 0x009ECA28 File Offset: 0x009ECA28
        internal static string InvalidOperation_InvalidComUnRegFunctionSig {
            get {
                return SR.GetResourceString("InvalidOperation_InvalidComUnRegFunctionSig");
            }
        }

        // Token: 0x170007BB RID: 1979
        // (get) Token: 0x060025A5 RID: 9637 RVA: 0x009ECA34 File Offset: 0x009ECA34
        internal static string InvalidOperation_InvalidHandle {
            get {
                return SR.GetResourceString("InvalidOperation_InvalidHandle");
            }
        }

        // Token: 0x170007BC RID: 1980
        // (get) Token: 0x060025A6 RID: 9638 RVA: 0x009ECA40 File Offset: 0x009ECA40
        internal static string InvalidOperation_MultipleComRegFunctions {
            get {
                return SR.GetResourceString("InvalidOperation_MultipleComRegFunctions");
            }
        }

        // Token: 0x170007BD RID: 1981
        // (get) Token: 0x060025A7 RID: 9639 RVA: 0x009ECA4C File Offset: 0x009ECA4C
        internal static string InvalidOperation_MultipleComUnRegFunctions {
            get {
                return SR.GetResourceString("InvalidOperation_MultipleComUnRegFunctions");
            }
        }

        // Token: 0x170007BE RID: 1982
        // (get) Token: 0x060025A8 RID: 9640 RVA: 0x009ECA58 File Offset: 0x009ECA58
        internal static string InvalidOperation_ResetGlobalComWrappersInstance {
            get {
                return SR.GetResourceString("InvalidOperation_ResetGlobalComWrappersInstance");
            }
        }

        // Token: 0x170007BF RID: 1983
        // (get) Token: 0x060025A9 RID: 9641 RVA: 0x009ECA64 File Offset: 0x009ECA64
        internal static string InvalidOperation_SuppliedInnerMustBeMarkedAggregation {
            get {
                return SR.GetResourceString("InvalidOperation_SuppliedInnerMustBeMarkedAggregation");
            }
        }

        // Token: 0x170007C0 RID: 1984
        // (get) Token: 0x060025AA RID: 9642 RVA: 0x009ECA70 File Offset: 0x009ECA70
        internal static string InvalidOperationException_NoGCRegionCallbackAlreadyRegistered {
            get {
                return SR.GetResourceString("InvalidOperationException_NoGCRegionCallbackAlreadyRegistered");
            }
        }

        // Token: 0x170007C1 RID: 1985
        // (get) Token: 0x060025AB RID: 9643 RVA: 0x009ECA7C File Offset: 0x009ECA7C
        internal static string Argument_SpansMustHaveSameLength {
            get {
                return SR.GetResourceString("Argument_SpansMustHaveSameLength");
            }
        }

        // Token: 0x170007C2 RID: 1986
        // (get) Token: 0x060025AC RID: 9644 RVA: 0x009ECA88 File Offset: 0x009ECA88
        internal static string NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed {
            get {
                return SR.GetResourceString("NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed");
            }
        }

        // Token: 0x170007C3 RID: 1987
        // (get) Token: 0x060025AD RID: 9645 RVA: 0x009ECA94 File Offset: 0x009ECA94
        internal static string GenericInvalidData {
            get {
                return SR.GetResourceString("GenericInvalidData");
            }
        }

        // Token: 0x170007C4 RID: 1988
        // (get) Token: 0x060025AE RID: 9646 RVA: 0x009ECAA0 File Offset: 0x009ECAA0
        internal static string Argument_ResourceScopeWrongDirection {
            get {
                return SR.GetResourceString("Argument_ResourceScopeWrongDirection");
            }
        }

        // Token: 0x170007C5 RID: 1989
        // (get) Token: 0x060025AF RID: 9647 RVA: 0x009ECAAC File Offset: 0x009ECAAC
        internal static string ArgumentNull_TypeRequiredByResourceScope {
            get {
                return SR.GetResourceString("ArgumentNull_TypeRequiredByResourceScope");
            }
        }

        // Token: 0x170007C6 RID: 1990
        // (get) Token: 0x060025B0 RID: 9648 RVA: 0x009ECAB8 File Offset: 0x009ECAB8
        internal static string Argument_BadResourceScopeTypeBits {
            get {
                return SR.GetResourceString("Argument_BadResourceScopeTypeBits");
            }
        }

        // Token: 0x170007C7 RID: 1991
        // (get) Token: 0x060025B1 RID: 9649 RVA: 0x009ECAC4 File Offset: 0x009ECAC4
        internal static string Argument_BadResourceScopeVisibilityBits {
            get {
                return SR.GetResourceString("Argument_BadResourceScopeVisibilityBits");
            }
        }

        // Token: 0x170007C8 RID: 1992
        // (get) Token: 0x060025B2 RID: 9650 RVA: 0x009ECAD0 File Offset: 0x009ECAD0
        internal static string Argument_EmptyString {
            get {
                return SR.GetResourceString("Argument_EmptyString");
            }
        }

        // Token: 0x170007C9 RID: 1993
        // (get) Token: 0x060025B3 RID: 9651 RVA: 0x009ECADC File Offset: 0x009ECADC
        internal static string Argument_EmptyOrWhiteSpaceString {
            get {
                return SR.GetResourceString("Argument_EmptyOrWhiteSpaceString");
            }
        }

        // Token: 0x170007CA RID: 1994
        // (get) Token: 0x060025B4 RID: 9652 RVA: 0x009ECAE8 File Offset: 0x009ECAE8
        internal static string Argument_FrameworkNameInvalid {
            get {
                return SR.GetResourceString("Argument_FrameworkNameInvalid");
            }
        }

        // Token: 0x170007CB RID: 1995
        // (get) Token: 0x060025B5 RID: 9653 RVA: 0x009ECAF4 File Offset: 0x009ECAF4
        internal static string Argument_FrameworkNameInvalidVersion {
            get {
                return SR.GetResourceString("Argument_FrameworkNameInvalidVersion");
            }
        }

        // Token: 0x170007CC RID: 1996
        // (get) Token: 0x060025B6 RID: 9654 RVA: 0x009ECB00 File Offset: 0x009ECB00
        internal static string Argument_FrameworkNameMissingVersion {
            get {
                return SR.GetResourceString("Argument_FrameworkNameMissingVersion");
            }
        }

        // Token: 0x170007CD RID: 1997
        // (get) Token: 0x060025B7 RID: 9655 RVA: 0x009ECB0C File Offset: 0x009ECB0C
        internal static string Argument_FrameworkNameTooShort {
            get {
                return SR.GetResourceString("Argument_FrameworkNameTooShort");
            }
        }

        // Token: 0x170007CE RID: 1998
        // (get) Token: 0x060025B8 RID: 9656 RVA: 0x009ECB18 File Offset: 0x009ECB18
        internal static string Arg_SwitchExpressionException {
            get {
                return SR.GetResourceString("Arg_SwitchExpressionException");
            }
        }

        // Token: 0x170007CF RID: 1999
        // (get) Token: 0x060025B9 RID: 9657 RVA: 0x009ECB24 File Offset: 0x009ECB24
        internal static string Arg_ContextMarshalException {
            get {
                return SR.GetResourceString("Arg_ContextMarshalException");
            }
        }

        // Token: 0x170007D0 RID: 2000
        // (get) Token: 0x060025BA RID: 9658 RVA: 0x009ECB30 File Offset: 0x009ECB30
        internal static string Arg_AppDomainUnloadedException {
            get {
                return SR.GetResourceString("Arg_AppDomainUnloadedException");
            }
        }

        // Token: 0x170007D1 RID: 2001
        // (get) Token: 0x060025BB RID: 9659 RVA: 0x009ECB3C File Offset: 0x009ECB3C
        internal static string SwitchExpressionException_UnmatchedValue {
            get {
                return SR.GetResourceString("SwitchExpressionException_UnmatchedValue");
            }
        }

        // Token: 0x170007D2 RID: 2002
        // (get) Token: 0x060025BC RID: 9660 RVA: 0x009ECB48 File Offset: 0x009ECB48
        internal static string Encoding_UTF7_Disabled {
            get {
                return SR.GetResourceString("Encoding_UTF7_Disabled");
            }
        }

        // Token: 0x170007D3 RID: 2003
        // (get) Token: 0x060025BD RID: 9661 RVA: 0x009ECB54 File Offset: 0x009ECB54
        internal static string IDynamicInterfaceCastable_DoesNotImplementRequested {
            get {
                return SR.GetResourceString("IDynamicInterfaceCastable_DoesNotImplementRequested");
            }
        }

        // Token: 0x170007D4 RID: 2004
        // (get) Token: 0x060025BE RID: 9662 RVA: 0x009ECB60 File Offset: 0x009ECB60
        internal static string IDynamicInterfaceCastable_MissingImplementationAttribute {
            get {
                return SR.GetResourceString("IDynamicInterfaceCastable_MissingImplementationAttribute");
            }
        }

        // Token: 0x170007D5 RID: 2005
        // (get) Token: 0x060025BF RID: 9663 RVA: 0x009ECB6C File Offset: 0x009ECB6C
        internal static string IDynamicInterfaceCastable_NotInterface {
            get {
                return SR.GetResourceString("IDynamicInterfaceCastable_NotInterface");
            }
        }

        // Token: 0x170007D6 RID: 2006
        // (get) Token: 0x060025C0 RID: 9664 RVA: 0x009ECB78 File Offset: 0x009ECB78
        internal static string Arg_MustBeHalf {
            get {
                return SR.GetResourceString("Arg_MustBeHalf");
            }
        }

        // Token: 0x170007D7 RID: 2007
        // (get) Token: 0x060025C1 RID: 9665 RVA: 0x009ECB84 File Offset: 0x009ECB84
        internal static string Arg_MustBeRune {
            get {
                return SR.GetResourceString("Arg_MustBeRune");
            }
        }

        // Token: 0x170007D8 RID: 2008
        // (get) Token: 0x060025C2 RID: 9666 RVA: 0x009ECB90 File Offset: 0x009ECB90
        internal static string BinaryFormatter_SerializationDisallowed {
            get {
                return SR.GetResourceString("BinaryFormatter_SerializationDisallowed");
            }
        }

        // Token: 0x170007D9 RID: 2009
        // (get) Token: 0x060025C3 RID: 9667 RVA: 0x009ECB9C File Offset: 0x009ECB9C
        internal static string NotSupported_CodeBase {
            get {
                return SR.GetResourceString("NotSupported_CodeBase");
            }
        }

        // Token: 0x170007DA RID: 2010
        // (get) Token: 0x060025C4 RID: 9668 RVA: 0x009ECBA8 File Offset: 0x009ECBA8
        internal static string Activator_CannotCreateInstance {
            get {
                return SR.GetResourceString("Activator_CannotCreateInstance");
            }
        }

        // Token: 0x170007DB RID: 2011
        // (get) Token: 0x060025C5 RID: 9669 RVA: 0x009ECBB4 File Offset: 0x009ECBB4
        internal static string Argv_IncludeDoubleQuote {
            get {
                return SR.GetResourceString("Argv_IncludeDoubleQuote");
            }
        }

        // Token: 0x170007DC RID: 2012
        // (get) Token: 0x060025C6 RID: 9670 RVA: 0x009ECBC0 File Offset: 0x009ECBC0
        internal static string ResourceManager_ReflectionNotAllowed {
            get {
                return SR.GetResourceString("ResourceManager_ReflectionNotAllowed");
            }
        }

        // Token: 0x170007DD RID: 2013
        // (get) Token: 0x060025C7 RID: 9671 RVA: 0x009ECBCC File Offset: 0x009ECBCC
        internal static string InvalidOperation_AssemblyLocationOverrideAlreadySet {
            get {
                return SR.GetResourceString("InvalidOperation_AssemblyLocationOverrideAlreadySet");
            }
        }

        // Token: 0x170007DE RID: 2014
        // (get) Token: 0x060025C8 RID: 9672 RVA: 0x009ECBD8 File Offset: 0x009ECBD8
        internal static string NotSupported_COM {
            get {
                return SR.GetResourceString("NotSupported_COM");
            }
        }

        // Token: 0x170007DF RID: 2015
        // (get) Token: 0x060025C9 RID: 9673 RVA: 0x009ECBE4 File Offset: 0x009ECBE4
        internal static string NotSupported_CppCli {
            get {
                return SR.GetResourceString("NotSupported_CppCli");
            }
        }

        // Token: 0x170007E0 RID: 2016
        // (get) Token: 0x060025CA RID: 9674 RVA: 0x009ECBF0 File Offset: 0x009ECBF0
        internal static string InvalidOperation_EmptyQueue {
            get {
                return SR.GetResourceString("InvalidOperation_EmptyQueue");
            }
        }

        // Token: 0x170007E1 RID: 2017
        // (get) Token: 0x060025CB RID: 9675 RVA: 0x009ECBFC File Offset: 0x009ECBFC
        internal static string Arg_FileIsDirectory_Name {
            get {
                return SR.GetResourceString("Arg_FileIsDirectory_Name");
            }
        }

        // Token: 0x170007E2 RID: 2018
        // (get) Token: 0x060025CC RID: 9676 RVA: 0x009ECC08 File Offset: 0x009ECC08
        internal static string Arg_InvalidFileAttrs {
            get {
                return SR.GetResourceString("Arg_InvalidFileAttrs");
            }
        }

        // Token: 0x170007E3 RID: 2019
        // (get) Token: 0x060025CD RID: 9677 RVA: 0x009ECC14 File Offset: 0x009ECC14
        internal static string Arg_Path2IsRooted {
            get {
                return SR.GetResourceString("Arg_Path2IsRooted");
            }
        }

        // Token: 0x170007E4 RID: 2020
        // (get) Token: 0x060025CE RID: 9678 RVA: 0x009ECC20 File Offset: 0x009ECC20
        internal static string Arg_PathIsVolume {
            get {
                return SR.GetResourceString("Arg_PathIsVolume");
            }
        }

        // Token: 0x170007E5 RID: 2021
        // (get) Token: 0x060025CF RID: 9679 RVA: 0x009ECC2C File Offset: 0x009ECC2C
        internal static string Argument_InvalidSubPath {
            get {
                return SR.GetResourceString("Argument_InvalidSubPath");
            }
        }

        // Token: 0x170007E6 RID: 2022
        // (get) Token: 0x060025D0 RID: 9680 RVA: 0x009ECC38 File Offset: 0x009ECC38
        internal static string IO_SourceDestMustBeDifferent {
            get {
                return SR.GetResourceString("IO_SourceDestMustBeDifferent");
            }
        }

        // Token: 0x170007E7 RID: 2023
        // (get) Token: 0x060025D1 RID: 9681 RVA: 0x009ECC44 File Offset: 0x009ECC44
        internal static string IO_SourceDestMustHaveSameRoot {
            get {
                return SR.GetResourceString("IO_SourceDestMustHaveSameRoot");
            }
        }

        // Token: 0x170007E8 RID: 2024
        // (get) Token: 0x060025D2 RID: 9682 RVA: 0x009ECC50 File Offset: 0x009ECC50
        internal static string PlatformNotSupported_FileEncryption {
            get {
                return SR.GetResourceString("PlatformNotSupported_FileEncryption");
            }
        }

        // Token: 0x170007E9 RID: 2025
        // (get) Token: 0x060025D3 RID: 9683 RVA: 0x009ECC5C File Offset: 0x009ECC5C
        internal static string Arg_MemberInfoNotFound {
            get {
                return SR.GetResourceString("Arg_MemberInfoNotFound");
            }
        }

        // Token: 0x170007EA RID: 2026
        // (get) Token: 0x060025D4 RID: 9684 RVA: 0x009ECC68 File Offset: 0x009ECC68
        internal static string ThreadState_NotStarted {
            get {
                return SR.GetResourceString("ThreadState_NotStarted");
            }
        }

        // Token: 0x170007EB RID: 2027
        // (get) Token: 0x060025D5 RID: 9685 RVA: 0x009ECC74 File Offset: 0x009ECC74
        internal static string ThreadState_Dead_Priority {
            get {
                return SR.GetResourceString("ThreadState_Dead_Priority");
            }
        }

        // Token: 0x170007EC RID: 2028
        // (get) Token: 0x060025D6 RID: 9686 RVA: 0x009ECC80 File Offset: 0x009ECC80
        internal static string ThreadState_Dead_State {
            get {
                return SR.GetResourceString("ThreadState_Dead_State");
            }
        }

        // Token: 0x170007ED RID: 2029
        // (get) Token: 0x060025D7 RID: 9687 RVA: 0x009ECC8C File Offset: 0x009ECC8C
        internal static string NullReference_InvokeNullRefReturned {
            get {
                return SR.GetResourceString("NullReference_InvokeNullRefReturned");
            }
        }

        // Token: 0x170007EE RID: 2030
        // (get) Token: 0x060025D8 RID: 9688 RVA: 0x009ECC98 File Offset: 0x009ECC98
        internal static string ArgumentOutOfRangeException_NoGCRegionSizeTooLarge {
            get {
                return SR.GetResourceString("ArgumentOutOfRangeException_NoGCRegionSizeTooLarge");
            }
        }

        // Token: 0x170007EF RID: 2031
        // (get) Token: 0x060025D9 RID: 9689 RVA: 0x009ECCA4 File Offset: 0x009ECCA4
        internal static string InvalidOperationException_AlreadyInNoGCRegion {
            get {
                return SR.GetResourceString("InvalidOperationException_AlreadyInNoGCRegion");
            }
        }

        // Token: 0x170007F0 RID: 2032
        // (get) Token: 0x060025DA RID: 9690 RVA: 0x009ECCB0 File Offset: 0x009ECCB0
        internal static string InvalidOperationException_NoGCRegionAllocationExceeded {
            get {
                return SR.GetResourceString("InvalidOperationException_NoGCRegionAllocationExceeded");
            }
        }

        // Token: 0x170007F1 RID: 2033
        // (get) Token: 0x060025DB RID: 9691 RVA: 0x009ECCBC File Offset: 0x009ECCBC
        internal static string InvalidOperationException_NoGCRegionInduced {
            get {
                return SR.GetResourceString("InvalidOperationException_NoGCRegionInduced");
            }
        }

        // Token: 0x170007F2 RID: 2034
        // (get) Token: 0x060025DC RID: 9692 RVA: 0x009ECCC8 File Offset: 0x009ECCC8
        internal static string InvalidOperationException_NoGCRegionNotInProgress {
            get {
                return SR.GetResourceString("InvalidOperationException_NoGCRegionNotInProgress");
            }
        }

        // Token: 0x170007F3 RID: 2035
        // (get) Token: 0x060025DD RID: 9693 RVA: 0x009ECCD4 File Offset: 0x009ECCD4
        internal static string InvalidOperationException_HardLimitTooLow {
            get {
                return SR.GetResourceString("InvalidOperationException_HardLimitTooLow");
            }
        }

        // Token: 0x170007F4 RID: 2036
        // (get) Token: 0x060025DE RID: 9694 RVA: 0x009ECCE0 File Offset: 0x009ECCE0
        internal static string InvalidOperationException_HardLimitInvalid {
            get {
                return SR.GetResourceString("InvalidOperationException_HardLimitInvalid");
            }
        }

        // Token: 0x170007F5 RID: 2037
        // (get) Token: 0x060025DF RID: 9695 RVA: 0x009ECCEC File Offset: 0x009ECCEC
        internal static string PlatformNotSupported_ReflectionEmit {
            get {
                return SR.GetResourceString("PlatformNotSupported_ReflectionEmit");
            }
        }

        // Token: 0x170007F6 RID: 2038
        // (get) Token: 0x060025E0 RID: 9696 RVA: 0x009ECCF8 File Offset: 0x009ECCF8
        internal static string Security_InvalidAssemblyPublicKey {
            get {
                return SR.GetResourceString("Security_InvalidAssemblyPublicKey");
            }
        }

        // Token: 0x170007F7 RID: 2039
        // (get) Token: 0x060025E1 RID: 9697 RVA: 0x009ECD04 File Offset: 0x009ECD04
        internal static string Arg_EntryPointNotFoundExceptionParameterizedNoLibrary {
            get {
                return SR.GetResourceString("Arg_EntryPointNotFoundExceptionParameterizedNoLibrary");
            }
        }

        // Token: 0x170007F8 RID: 2040
        // (get) Token: 0x060025E2 RID: 9698 RVA: 0x009ECD10 File Offset: 0x009ECD10
        internal static string DllNotFound_Windows {
            get {
                return SR.GetResourceString("DllNotFound_Windows");
            }
        }

        // Token: 0x170007F9 RID: 2041
        // (get) Token: 0x060025E3 RID: 9699 RVA: 0x009ECD1C File Offset: 0x009ECD1C
        internal static string InvalidOperation_ComInteropRequireComWrapperInstance {
            get {
                return SR.GetResourceString("InvalidOperation_ComInteropRequireComWrapperInstance");
            }
        }

        // Token: 0x170007FA RID: 2042
        // (get) Token: 0x060025E4 RID: 9700 RVA: 0x009ECD28 File Offset: 0x009ECD28
        internal static string InvalidOperation_ComInteropRequireComWrapperTrackerInstance {
            get {
                return SR.GetResourceString("InvalidOperation_ComInteropRequireComWrapperTrackerInstance");
            }
        }

        // Token: 0x170007FB RID: 2043
        // (get) Token: 0x060025E5 RID: 9701 RVA: 0x009ECD34 File Offset: 0x009ECD34
        internal static string ArgumentOutOfRange_NotGreaterThanBufferLength {
            get {
                return SR.GetResourceString("ArgumentOutOfRange_NotGreaterThanBufferLength");
            }
        }

        // Token: 0x170007FC RID: 2044
        // (get) Token: 0x060025E6 RID: 9702 RVA: 0x009ECD40 File Offset: 0x009ECD40
        internal static string Argument_AssemblyGetTypeCannotSpecifyAssembly {
            get {
                return SR.GetResourceString("Argument_AssemblyGetTypeCannotSpecifyAssembly");
            }
        }

        // Token: 0x170007FD RID: 2045
        // (get) Token: 0x060025E7 RID: 9703 RVA: 0x009ECD4C File Offset: 0x009ECD4C
        internal static string Argument_DirectorySeparatorInvalid {
            get {
                return SR.GetResourceString("Argument_DirectorySeparatorInvalid");
            }
        }

        // Token: 0x170007FE RID: 2046
        // (get) Token: 0x060025E8 RID: 9704 RVA: 0x009ECD58 File Offset: 0x009ECD58
        internal static string InvalidOperation_NotFunctionPointer {
            get {
                return SR.GetResourceString("InvalidOperation_NotFunctionPointer");
            }
        }

        // Token: 0x170007FF RID: 2047
        // (get) Token: 0x060025E9 RID: 9705 RVA: 0x009ECD64 File Offset: 0x009ECD64
        internal static string NotSupported_ModifiedType {
            get {
                return SR.GetResourceString("NotSupported_ModifiedType");
            }
        }

        // Token: 0x17000800 RID: 2048
        // (get) Token: 0x060025EA RID: 9706 RVA: 0x009ECD70 File Offset: 0x009ECD70
        internal static string Argument_UnexpectedStateForKnownCallback {
            get {
                return SR.GetResourceString("Argument_UnexpectedStateForKnownCallback");
            }
        }

        // Token: 0x17000801 RID: 2049
        // (get) Token: 0x060025EB RID: 9707 RVA: 0x009ECD7C File Offset: 0x009ECD7C
        internal static string OutOfMemory_StringTooLong {
            get {
                return SR.GetResourceString("OutOfMemory_StringTooLong");
            }
        }

        // Token: 0x17000802 RID: 2050
        // (get) Token: 0x060025EC RID: 9708 RVA: 0x009ECD88 File Offset: 0x009ECD88
        internal static string Argument_SearchValues_UnsupportedStringComparison {
            get {
                return SR.GetResourceString("Argument_SearchValues_UnsupportedStringComparison");
            }
        }

        // Token: 0x17000803 RID: 2051
        // (get) Token: 0x060025ED RID: 9709 RVA: 0x009ECD94 File Offset: 0x009ECD94
        internal static string ComVariant_SizeMustMatchVariantSize {
            get {
                return SR.GetResourceString("ComVariant_SizeMustMatchVariantSize");
            }
        }

        // Token: 0x17000804 RID: 2052
        // (get) Token: 0x060025EE RID: 9710 RVA: 0x009ECDA0 File Offset: 0x009ECDA0
        internal static string ComVariant_TypeIsNotSupportedType {
            get {
                return SR.GetResourceString("ComVariant_TypeIsNotSupportedType");
            }
        }

        // Token: 0x17000805 RID: 2053
        // (get) Token: 0x060025EF RID: 9711 RVA: 0x009ECDAC File Offset: 0x009ECDAC
        internal static string ComVariant_VT_DECIMAL_NotSupported_CreateRaw {
            get {
                return SR.GetResourceString("ComVariant_VT_DECIMAL_NotSupported_CreateRaw");
            }
        }

        // Token: 0x17000806 RID: 2054
        // (get) Token: 0x060025F0 RID: 9712 RVA: 0x009ECDB8 File Offset: 0x009ECDB8
        internal static string ComVariant_VT_DECIMAL_NotSupported_RawDataRef {
            get {
                return SR.GetResourceString("ComVariant_VT_DECIMAL_NotSupported_RawDataRef");
            }
        }

        // Token: 0x17000807 RID: 2055
        // (get) Token: 0x060025F1 RID: 9713 RVA: 0x009ECDC4 File Offset: 0x009ECDC4
        internal static string ComVariant_VT_VARIANT_In_Variant {
            get {
                return SR.GetResourceString("ComVariant_VT_VARIANT_In_Variant");
            }
        }

        // Token: 0x17000808 RID: 2056
        // (get) Token: 0x060025F2 RID: 9714 RVA: 0x009ECDD0 File Offset: 0x009ECDD0
        internal static string UnsupportedType {
            get {
                return SR.GetResourceString("UnsupportedType");
            }
        }

        // Token: 0x17000809 RID: 2057
        // (get) Token: 0x060025F3 RID: 9715 RVA: 0x009ECDDC File Offset: 0x009ECDDC
        internal static string RFLCT_CannotSetInitonlyStaticField {
            get {
                return SR.GetResourceString("RFLCT_CannotSetInitonlyStaticField");
            }
        }

        // Token: 0x1700080A RID: 2058
        // (get) Token: 0x060025F4 RID: 9716 RVA: 0x009ECDE8 File Offset: 0x009ECDE8
        internal static string NotSupported_EmitDebugInfo {
            get {
                return SR.GetResourceString("NotSupported_EmitDebugInfo");
            }
        }

        // Token: 0x1700080B RID: 2059
        // (get) Token: 0x060025F5 RID: 9717 RVA: 0x009ECDF4 File Offset: 0x009ECDF4
        internal static string Arg_MustBeBFloat16 {
            get {
                return SR.GetResourceString("Arg_MustBeBFloat16");
            }
        }

        // Token: 0x1700080C RID: 2060
        // (get) Token: 0x060025F6 RID: 9718 RVA: 0x009ECE00 File Offset: 0x009ECE00
        internal static string NotSupported_ReferenceEnumOrPrimitiveTypeRequired {
            get {
                return SR.GetResourceString("NotSupported_ReferenceEnumOrPrimitiveTypeRequired");
            }
        }

        // Token: 0x1700080D RID: 2061
        // (get) Token: 0x060025F7 RID: 9719 RVA: 0x009ECE0C File Offset: 0x009ECE0C
        internal static string NotSupported_IntegerEnumOrPrimitiveTypeRequired {
            get {
                return SR.GetResourceString("NotSupported_IntegerEnumOrPrimitiveTypeRequired");
            }
        }

        // Token: 0x1700080E RID: 2062
        // (get) Token: 0x060025F8 RID: 9720 RVA: 0x009ECE18 File Offset: 0x009ECE18
        internal static string Argument_BadFieldForInitializeArray {
            get {
                return SR.GetResourceString("Argument_BadFieldForInitializeArray");
            }
        }

        // Token: 0x1700080F RID: 2063
        // (get) Token: 0x060025F9 RID: 9721 RVA: 0x009ECE24 File Offset: 0x009ECE24
        internal static string Argument_BadArrayForInitializeArray {
            get {
                return SR.GetResourceString("Argument_BadArrayForInitializeArray");
            }
        }

        // Token: 0x17000810 RID: 2064
        // (get) Token: 0x060025FA RID: 9722 RVA: 0x009ECE30 File Offset: 0x009ECE30
        internal static string ComVariant_CriticalHandle_In_Variant {
            get {
                return SR.GetResourceString("ComVariant_CriticalHandle_In_Variant");
            }
        }

        // Token: 0x17000811 RID: 2065
        // (get) Token: 0x060025FB RID: 9723 RVA: 0x009ECE3C File Offset: 0x009ECE3C
        internal static string ComVariant_SafeHandle_In_Variant {
            get {
                return SR.GetResourceString("ComVariant_SafeHandle_In_Variant");
            }
        }

        // Token: 0x17000812 RID: 2066
        // (get) Token: 0x060025FC RID: 9724 RVA: 0x009ECE48 File Offset: 0x009ECE48
        internal static string ComVariant_UnsupportedSignature {
            get {
                return SR.GetResourceString("ComVariant_UnsupportedSignature");
            }
        }

        // Token: 0x17000813 RID: 2067
        // (get) Token: 0x060025FD RID: 9725 RVA: 0x009ECE54 File Offset: 0x009ECE54
        internal static string ComVariant_UnsupportedType {
            get {
                return SR.GetResourceString("ComVariant_UnsupportedType");
            }
        }

        // Token: 0x17000814 RID: 2068
        // (get) Token: 0x060025FE RID: 9726 RVA: 0x009ECE60 File Offset: 0x009ECE60
        internal static string ComVariant_VariantWrapper_In_Variant {
            get {
                return SR.GetResourceString("ComVariant_VariantWrapper_In_Variant");
            }
        }

        // Token: 0x17000815 RID: 2069
        // (get) Token: 0x060025FF RID: 9727 RVA: 0x009ECE6C File Offset: 0x009ECE6C
        internal static string NotImplemented_CreateObjectWithUserState {
            get {
                return SR.GetResourceString("NotImplemented_CreateObjectWithUserState");
            }
        }

        // Token: 0x17000816 RID: 2070
        // (get) Token: 0x06002600 RID: 9728 RVA: 0x009ECE78 File Offset: 0x009ECE78
        internal static string Argument_MustBeFunctionPointer {
            get {
                return SR.GetResourceString("Argument_MustBeFunctionPointer");
            }
        }

        // Token: 0x17000817 RID: 2071
        // (get) Token: 0x06002601 RID: 9729 RVA: 0x009ECE84 File Offset: 0x009ECE84
        internal static string ManagedFunctionPointer_CallingConventionsNotAllowed {
            get {
                return SR.GetResourceString("ManagedFunctionPointer_CallingConventionsNotAllowed");
            }
        }

        // Token: 0x17000818 RID: 2072
        // (get) Token: 0x06002602 RID: 9730 RVA: 0x009ECE90 File Offset: 0x009ECE90
        internal static string FunctionPointer_ParameterInvalid {
            get {
                return SR.GetResourceString("FunctionPointer_ParameterInvalid");
            }
        }

        // Token: 0x17000819 RID: 2073
        // (get) Token: 0x06002603 RID: 9731 RVA: 0x009ECE9C File Offset: 0x009ECE9C
        internal static string FunctionPointer_ReturnTypeInvalid {
            get {
                return SR.GetResourceString("FunctionPointer_ReturnTypeInvalid");
            }
        }

        // Token: 0x1700081A RID: 2074
        // (get) Token: 0x06002604 RID: 9732 RVA: 0x009ECEA8 File Offset: 0x009ECEA8
        internal static string FunctionPointer_CallingConventionsUnbalanced {
            get {
                return SR.GetResourceString("FunctionPointer_CallingConventionsUnbalanced");
            }
        }

        // Token: 0x1700081B RID: 2075
        // (get) Token: 0x06002605 RID: 9733 RVA: 0x009ECEB4 File Offset: 0x009ECEB4
        internal static string FunctionPointer_InvalidCallingConvention {
            get {
                return SR.GetResourceString("FunctionPointer_InvalidCallingConvention");
            }
        }

        // Token: 0x1700081C RID: 2076
        // (get) Token: 0x06002606 RID: 9734 RVA: 0x009ECEC0 File Offset: 0x009ECEC0
        internal static string NotSupported_FunctionPointerSignature {
            get {
                return SR.GetResourceString("NotSupported_FunctionPointerSignature");
            }
        }

        // Token: 0x1700081D RID: 2077
        // (get) Token: 0x06002607 RID: 9735 RVA: 0x009ECECC File Offset: 0x009ECECC
        internal static string Argument_FunctionPointersInvalid {
            get {
                return SR.GetResourceString("Argument_FunctionPointersInvalid");
            }
        }

        // Token: 0x1700081E RID: 2078
        // (get) Token: 0x06002608 RID: 9736 RVA: 0x009ECED8 File Offset: 0x009ECED8
        internal static string Arg_HexBinaryStylesNotSupported {
            get {
                return SR.GetResourceString("Arg_HexBinaryStylesNotSupported");
            }
        }

        // Token: 0x1700081F RID: 2079
        // (get) Token: 0x06002609 RID: 9737 RVA: 0x009ECEE4 File Offset: 0x009ECEE4
        internal static string InvalidOperation_NoElements {
            get {
                return SR.GetResourceString("InvalidOperation_NoElements");
            }
        }
    }
}

namespace UltimateOrb.Runtime.InteropServices.Marshalling {
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    /// <summary>
    /// Supports marshalling a <see cref="Span{T}"/> from managed value
    /// to a contiguous native array of the unmanaged values of the elements.
    /// </summary>
    /// <typeparam name="T">The managed element type of the span.</typeparam>
    /// <typeparam name="TUnmanagedElement">The unmanaged type for the elements of the span.</typeparam>
    /// <remarks>
    /// A <see cref="Span{T}"/> marshalled with this marshaller will match the semantics of <see cref="MemoryMarshal.GetReference{T}(Span{T})"/>.
    /// In particular, this marshaller will pass a non-null value for a zero-length span if the span was constructed with a non-null value.
    /// </remarks>
    [CLSCompliant(false)]
    [CustomMarshaller(typeof(BigSpan<>), MarshalMode.Default, typeof(BigSpanMarshaller<,>))]
    [CustomMarshaller(typeof(BigSpan<>), MarshalMode.ManagedToUnmanagedIn, typeof(BigSpanMarshaller<,>.ManagedToUnmanagedIn))]
    [ContiguousCollectionMarshaller]
    public static unsafe class BigSpanMarshaller<T, TUnmanagedElement>
        where TUnmanagedElement : unmanaged {
        /// <summary>
        /// Allocates the space to store the unmanaged elements.
        /// </summary>
        /// <param name="managed">The managed span.</param>
        /// <param name="numElements">The number of elements in the span.</param>
        /// <returns>A pointer to the block of memory for the unmanaged elements.</returns>
        public static TUnmanagedElement* AllocateContainerForUnmanagedElements(BigSpan<T> managed, out int numElements) {
            // Emulate the pinning behavior:
            // If the span is over a null reference, then pass a null pointer.
            if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(managed))) {
                numElements = 0;
                return null;
            }

            numElements = checked((int)managed.Length);

            // Always allocate at least one byte when the array is zero-length.
            int spaceToAllocate = Math.Max(checked(sizeof(TUnmanagedElement) * numElements), 1);
            return (TUnmanagedElement*)Marshal.AllocCoTaskMem(spaceToAllocate);
        }

        /// <summary>
        /// Gets a span of the managed collection elements.
        /// </summary>
        /// <param name="managed">The managed collection.</param>
        /// <returns>A span of the managed collection elements.</returns>
        public static ReadOnlySpan<T> GetManagedValuesSource(BigSpan<T> managed)
            => (Span<T>)managed;

        /// <summary>
        /// Gets a span of the space where the unmanaged collection elements should be stored.
        /// </summary>
        /// <param name="unmanaged">The pointer to the block of memory for the unmanaged elements.</param>
        /// <param name="numElements">The number of elements that will be copied into the memory block.</param>
        /// <returns>A span over the unmanaged memory that can contain the specified number of elements.</returns>
        public static Span<TUnmanagedElement> GetUnmanagedValuesDestination(TUnmanagedElement* unmanaged, int numElements) {
            if (unmanaged == null)
                return [];

            return new Span<TUnmanagedElement>(unmanaged, numElements);
        }

        /// <summary>
        /// Allocates space to store the managed elements.
        /// </summary>
        /// <param name="unmanaged">The unmanaged value.</param>
        /// <param name="numElements">The number of elements in the unmanaged collection.</param>
        /// <returns>A span over enough memory to contain <paramref name="numElements"/> elements.</returns>
        public static BigSpan<T> AllocateContainerForManagedElements(TUnmanagedElement* unmanaged, int numElements) {
            if (unmanaged is null)
                return (TNull?)null;

            return new T[numElements];
        }

        /// <summary>
        /// Gets a span of the space where the managed collection elements should be stored.
        /// </summary>
        /// <param name="managed">A span over the space to store the managed elements.</param>
        /// <returns>A span over the managed memory that can contain the specified number of elements.</returns>
        public static Span<T> GetManagedValuesDestination(Span<T> managed)
            => managed;

        /// <summary>
        /// Gets a span of the native collection elements.
        /// </summary>
        /// <param name="unmanaged">The unmanaged value.</param>
        /// <param name="numElements">The number of elements in the unmanaged collection.</param>
        /// <returns>A span over the native collection elements.</returns>
        public static ReadOnlySpan<TUnmanagedElement> GetUnmanagedValuesSource(TUnmanagedElement* unmanaged, int numElements) {
            if (unmanaged == null)
                return [];

            return new ReadOnlySpan<TUnmanagedElement>(unmanaged, numElements);
        }

        /// <summary>
        /// Frees the allocated unmanaged memory.
        /// </summary>
        /// <param name="unmanaged">A pointer to the allocated unmanaged memory.</param>
        public static void Free(TUnmanagedElement* unmanaged)
            => Marshal.FreeCoTaskMem((IntPtr)unmanaged);

        /// <summary>
        /// Supports marshalling from managed into unmanaged in a call from managed code to unmanaged code.
        /// </summary>
        public ref struct ManagedToUnmanagedIn {
            // We'll keep the buffer size at a maximum of 512 bytes to avoid overflowing the stack.
            public static int BufferSize => 0x200 / sizeof(TUnmanagedElement);

            private BigSpan<T> _managedArray;
            private TUnmanagedElement* _allocatedMemory;
            private BigSpan<TUnmanagedElement> _span;

            /// <summary>
            /// Initializes the <see cref="SpanMarshaller{T, TUnmanagedElement}.ManagedToUnmanagedIn"/> marshaller.
            /// </summary>
            /// <param name="managed">The span to be marshalled.</param>
            /// <param name="buffer">The buffer that may be used for marshalling.</param>
            /// <remarks>
            /// The <paramref name="buffer"/> must not be movable - that is, it should not be
            /// on the managed heap or it should be pinned.
            /// </remarks>
            public void FromManaged(BigSpan<T> managed, Span<TUnmanagedElement> buffer) {
                _allocatedMemory = null;
                // Emulate the pinning behavior:
                // If the span is over a null reference, then pass a null pointer.
                if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(managed))) {
                    _managedArray = (TNull?)null;
                    _span = default;
                    return;
                }
                unchecked {
                    _managedArray = managed;

                    if (managed.Length <= buffer.Length) {
                        _span = buffer[0..(int)managed.Length];
                    } else {
                        nint bufferSize = checked(managed.Length * sizeof(TUnmanagedElement));
                        _allocatedMemory = (TUnmanagedElement*)NativeMemory.AlignedAlloc((nuint)bufferSize, (nuint)StructLayoutHelpers.AlignOf<TUnmanagedElement>());
                        _span = new BigSpan<TUnmanagedElement>(_allocatedMemory, managed.Length);
                    }
                }
            }

            /// <summary>
            /// Gets a span that points to the memory where the managed values of the array are stored.
            /// </summary>
            /// <returns>A span over the managed values of the array.</returns>
            public ReadOnlySpan<T> GetManagedValuesSource() => (ReadOnlySpan<T>)_managedArray;

            /// <summary>
            /// Returns a span that points to the memory where the unmanaged values of the array should be stored.
            /// </summary>
            /// <returns>A span where unmanaged values of the array should be stored.</returns>
            public Span<TUnmanagedElement> GetUnmanagedValuesDestination() => (Span<TUnmanagedElement>)_span;

            /// <summary>
            /// Returns a reference to the marshalled array.
            /// </summary>
            public ref TUnmanagedElement GetPinnableReference() => ref MemoryMarshal.GetReference(_span);

            /// <summary>
            /// Returns the unmanaged value representing the array.
            /// </summary>
            public TUnmanagedElement* ToUnmanaged() {
                // Unsafe.AsPointer is safe since buffer must be pinned
                return (TUnmanagedElement*)Unsafe.AsPointer(ref GetPinnableReference());
            }

            /// <summary>
            /// Frees resources.
            /// </summary>
            public void Free() {
                NativeMemory.Free(_allocatedMemory);
            }

            /// <summary>
            /// Gets a pinnable reference to the managed span.
            /// </summary>
            /// <param name="managed">The managed span.</param>
            /// <returns>A reference that can be pinned and directly passed to unmanaged code.</returns>
            public static ref T GetPinnableReference(BigSpan<T> managed) {
                return ref MemoryMarshal.GetReference(managed);
            }
        }
    }
}

namespace System.Runtime.InteropServices.Marshalling {
    // Licensed to the .NET Foundation under one or more agreements.
    // The .NET Foundation licenses this file to you under the MIT license.

    /// <summary>
    /// Supports marshalling a <see cref="ReadOnlySpan{T}"/> from managed value
    /// to a contiguous native array of the unmanaged values of the elements.
    /// </summary>
    /// <typeparam name="T">The managed element type of the span.</typeparam>
    /// <typeparam name="TUnmanagedElement">The unmanaged type for the elements of the span.</typeparam>
    /// <remarks>
    /// A <see cref="ReadOnlySpan{T}"/> marshalled with this marshaller will match the semantics of <see cref="MemoryMarshal.GetReference{T}(ReadOnlySpan{T})"/>.
    /// In particular, this marshaller will pass a non-null value for a zero-length span if the span was constructed with a non-null value.
    /// </remarks>
    [CLSCompliant(false)]
    [CustomMarshaller(typeof(ReadOnlyBigSpan<>), MarshalMode.ManagedToUnmanagedIn, typeof(ReadOnlyBigSpanMarshaller<,>.ManagedToUnmanagedIn))]
    [CustomMarshaller(typeof(ReadOnlyBigSpan<>), MarshalMode.ManagedToUnmanagedOut, typeof(ReadOnlyBigSpanMarshaller<,>.ManagedToUnmanagedOut))]
    [CustomMarshaller(typeof(ReadOnlyBigSpan<>), MarshalMode.UnmanagedToManagedOut, typeof(ReadOnlyBigSpanMarshaller<,>.UnmanagedToManagedOut))]
    [ContiguousCollectionMarshaller]
    public static unsafe class ReadOnlyBigSpanMarshaller<T, TUnmanagedElement>
        where TUnmanagedElement : unmanaged {
        /// <summary>
        /// Supports marshalling from managed into unmanaged in a call from unmanaged code to managed code.
        /// </summary>
        public static class UnmanagedToManagedOut {
            /// <summary>
            /// Allocates the space to store the unmanaged elements.
            /// </summary>
            /// <param name="managed">The managed span.</param>
            /// <param name="numElements">The number of elements in the span.</param>
            /// <returns>A pointer to the block of memory for the unmanaged elements.</returns>
            public static TUnmanagedElement* AllocateContainerForUnmanagedElements(ReadOnlyBigSpan<T> managed, out int numElements) {
                // Emulate the pinning behavior:
                // If the span is over a null reference, then pass a null pointer.
                if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(managed))) {
                    numElements = 0;
                    return null;
                }

                numElements = checked((int)managed.Length);

                // Always allocate at least one byte when the array is zero-length.
                int spaceToAllocate = Math.Max(checked(sizeof(TUnmanagedElement) * numElements), 1);
                return (TUnmanagedElement*)Marshal.AllocCoTaskMem(spaceToAllocate);
            }

            /// <summary>
            /// Gets a span of the managed collection elements.
            /// </summary>
            /// <param name="managed">The managed collection.</param>
            /// <returns>A span of the managed collection elements.</returns>
            public static ReadOnlySpan<T> GetManagedValuesSource(ReadOnlyBigSpan<T> managed)
                => (ReadOnlySpan<T>)managed;

            /// <summary>
            /// Gets a span of the space where the unmanaged collection elements should be stored.
            /// </summary>
            /// <param name="unmanaged">The pointer to the block of memory for the unmanaged elements.</param>
            /// <param name="numElements">The number of elements that will be copied into the memory block.</param>
            /// <returns>A span over the unmanaged memory that can contain the specified number of elements.</returns>
            public static Span<TUnmanagedElement> GetUnmanagedValuesDestination(TUnmanagedElement* unmanaged, int numElements) {
                if (unmanaged == null)
                    return [];

                return new Span<TUnmanagedElement>(unmanaged, numElements);
            }
        }

        /// <summary>
        /// Supports marshalling from managed into unmanaged in a call from managed code to unmanaged code.
        /// </summary>
        public ref struct ManagedToUnmanagedIn {
            /// <summary>
            /// Gets the size of the caller-allocated buffer to allocate.
            /// </summary>
            // We'll keep the buffer size at a maximum of 512 bytes to avoid overflowing the stack.
            public static int BufferSize => 0x200 / sizeof(TUnmanagedElement);

            private ReadOnlyBigSpan<T> _managedArray;
            private TUnmanagedElement* _allocatedMemory;
            private BigSpan<TUnmanagedElement> _span;

            /// <summary>
            /// Initializes the <see cref="ReadOnlySpanMarshaller{T, TUnmanagedElement}.ManagedToUnmanagedIn"/> marshaller.
            /// </summary>
            /// <param name="managed">The span to be marshalled.</param>
            /// <param name="buffer">The buffer that may be used for marshalling.</param>
            /// <remarks>
            /// The <paramref name="buffer"/> must not be movable - that is, it should not be
            /// on the managed heap or it should be pinned.
            /// </remarks>
            public void FromManaged(ReadOnlyBigSpan<T> managed, Span<TUnmanagedElement> buffer) {
                _allocatedMemory = null;
                // Emulate the pinning behavior:
                // If the span is over a null reference, then pass a null pointer.
                if (Unsafe.IsNullRef(ref MemoryMarshal.GetReference(managed))) {
                    _managedArray = (TNull?)null;
                    _span = default;
                    return;
                }
                unchecked {
                    _managedArray = managed;

                    // Always allocate at least one byte when the span is zero-length.
                    if (managed.Length <= buffer.Length) {
                        _span = buffer[0..(int)managed.Length];
                    } else {
                        nint bufferSize = checked(managed.Length * sizeof(TUnmanagedElement));
                        _allocatedMemory = (TUnmanagedElement*)NativeMemory.AlignedAlloc((nuint)bufferSize, (nuint)StructLayoutHelpers.AlignOf<TUnmanagedElement>());
                        _span = new BigSpan<TUnmanagedElement>(_allocatedMemory, managed.Length);
                    }
                }
            }

            /// <summary>
            /// Returns a span that points to the memory where the managed values of the array are stored.
            /// </summary>
            /// <returns>A span over managed values of the array.</returns>
            public ReadOnlySpan<T> GetManagedValuesSource() => (ReadOnlySpan<T>)_managedArray;

            /// <summary>
            /// Returns a span that points to the memory where the unmanaged values of the array should be stored.
            /// </summary>
            /// <returns>A span where unmanaged values of the array should be stored.</returns>
            public Span<TUnmanagedElement> GetUnmanagedValuesDestination() => (Span<TUnmanagedElement>)_span;

            /// <summary>
            /// Returns a reference to the marshalled array.
            /// </summary>
            public ref TUnmanagedElement GetPinnableReference() => ref MemoryMarshal.GetReference(_span);

            /// <summary>
            /// Returns the unmanaged value representing the array.
            /// </summary>
            public TUnmanagedElement* ToUnmanaged() {
                // Unsafe.AsPointer is safe since buffer must be pinned
                return (TUnmanagedElement*)Unsafe.AsPointer(ref GetPinnableReference());
            }

            /// <summary>
            /// Frees resources.
            /// </summary>
            public void Free() {
                NativeMemory.AlignedFree(_allocatedMemory);
            }

            /// <summary>
            /// Pins the managed span to a pointer to pass directly to unmanaged code.
            /// </summary>
            /// <param name="managed">The managed span.</param>
            /// <returns>A reference that can be pinned and directly passed to unmanaged code.</returns>
            public static ref T GetPinnableReference(ReadOnlyBigSpan<T> managed) {
                return ref MemoryMarshal.GetReference(managed);
            }
        }

        /// <summary>
        /// Supports marshalling from unmanaged to managed in a call from managed code to unmanaged code. For example, return values and `out` parameters in P/Invoke methods.
        /// </summary>
        public struct ManagedToUnmanagedOut {
            private TUnmanagedElement* _unmanagedArray;
            private BigArray<T>? _managedValues;

            /// <summary>
            /// Initializes the <see cref="ReadOnlySpanMarshaller{T, TUnmanagedElement}.ManagedToUnmanagedOut"/> marshaller.
            /// </summary>
            /// <param name="unmanaged">A pointer to the array to be unmarshalled from native to managed.</param>
            public void FromUnmanaged(TUnmanagedElement* unmanaged) {
                _unmanagedArray = unmanaged;
            }

            /// <summary>
            /// Returns the managed value representing the native array.
            /// </summary>
            /// <returns>A span over managed values of the array.</returns>
            public ReadOnlyBigSpan<T> ToManaged() {
                return new ReadOnlyBigSpan<T>(_managedValues!);
            }

            /// <summary>
            /// Returns a span that points to the memory where the unmanaged elements of the array are stored.
            /// </summary>
            /// <param name="numElements">The number of elements in the array.</param>
            /// <returns>A span over unmanaged values of the array.</returns>
            public ReadOnlySpan<TUnmanagedElement> GetUnmanagedValuesSource(int numElements) {
                if (_unmanagedArray is null)
                    return [];

                return new ReadOnlySpan<TUnmanagedElement>(_unmanagedArray, numElements);
            }

            /// <summary>
            /// Returns a span that points to the memory where the managed elements of the array should be stored.
            /// </summary>
            /// <param name="numElements">The number of elements in the array.</param>
            /// <returns>A span where managed values of the array should be stored.</returns>
            public Span<T> GetManagedValuesDestination(int numElements) {
                _managedValues = BigArray.CreateInstance<T>(numElements);
                return (Span<T>)new BigSpan<T>( _managedValues);
            }

            /// <summary>
            /// Frees resources.
            /// </summary>
            public void Free() {
                Marshal.FreeCoTaskMem((IntPtr)_unmanagedArray);
            }
        }
    }
}