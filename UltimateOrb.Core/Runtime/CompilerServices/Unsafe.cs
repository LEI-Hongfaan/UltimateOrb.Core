using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Runtime.CompilerServices {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class Unsafe {
    }

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class Unsafe {

#pragma warning disable IDE0060 // Remove unused parameter
        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldobj !!0
            ret
        ")]
        public static unsafe T Read<T>(void* source) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            unaligned. 0x1
            ldobj !!0
            ret
        ")]

        public static unsafe T ReadUnaligned<T>(void* source) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            unaligned. 0x1
            ldobj !!0
            ret
        ")]
        public static unsafe T ReadUnaligned<T>(ref byte source) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            stobj !!0
            ret
        ")]
        public static unsafe void Write<T>(void* destination, T value) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            unaligned. 0x01
            stobj !!0
            ret
        ")]
        public static unsafe void WriteUnaligned<T>(void* destination, T value) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            unaligned. 0x01
            stobj !!0
            ret
        ")]
        public static unsafe void WriteUnaligned<T>(ref byte destination, T value) {
            throw null!;
        }


        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldobj !!0
            stobj !!0
            ret
        ")]
        public static unsafe void Copy<T>(void* destination, ref T source) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldobj !!0
            stobj !!0
            ret
        ")]
        public static unsafe void Copy<T>(ref T destination, void* source) {
            throw null!;
        }


        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            conv.u
            ret
        ")]
        public static unsafe void* AsPointer<T>(ref T value) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ret
        ")]
        public static void SkipInit<T>(out T value) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            sizeof !!0
            ret
        ")]
        public static Int32 SizeOf<T>() {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            cpblk
            ret
        ")]
        public static unsafe void CopyBlock(void* destination, void* source, uint byteCount) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            cpblk
            ret
        ")]
        public static void CopyBlock(ref byte destination, ref byte source, uint byteCount) {
            throw null!;
        }


        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            unaligned. 0x1
            cpblk
            ret
        ")]
        public static unsafe void CopyBlockUnaligned(void* destination, void* source, uint byteCount) {
            throw null!;
        }


        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            unaligned. 0x1
            cpblk
            ret
        ")]
        public static void CopyBlockUnaligned(ref byte destination, ref byte source, uint byteCount) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            initblk
            ret
        ")]
        public static unsafe void InitBlock(void* startAddress, byte value, uint byteCount) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            initblk
            ret
        ")]
        public static void InitBlock(ref byte startAddress, byte value, uint byteCount) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            unaligned. 0x1
            initblk
            ret
        ")]
        public static unsafe void InitBlockUnaligned(void* startAddress, byte value, uint byteCount) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ldarg.2
            unaligned. 0x1
            initblk
            ret
        ")]
        public static void InitBlockUnaligned(ref byte startAddress, byte value, uint byteCount) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static T As<T>(object? o) where T : class {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static unsafe ref T AsRef<T>(void* source) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static ref T AsRef<T>(in T source) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ret
        ")]
        public static ref TTo As<TFrom, TTo>(ref TFrom source) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            unbox !!0
            ret
        ")]
        public static ref T Unbox<T>(object box) where T : struct {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            conv.i
            mul
            add
            ret
        ")]
        public static ref T Add<T>(ref T source, int elementOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            conv.i
            mul
            add
            ret
        ")]
        public static unsafe void* Add<T>(void* source, int elementOffset) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            mul
            add
            ret
        ")]
        public static ref T Add<T>(ref T source, nint elementOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            mul
            add
            ret
        ")]
        public static ref T Add<T>(ref T source, nuint elementOffset) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            add
            ret
        ")]
        public static ref T AddByteOffset<T>(ref T source, nint byteOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            add
            ret
        ")]
        public static ref T AddByteOffset<T>(ref T source, nuint byteOffset) {
            throw null!;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            conv.i
            mul
            sub
            ret
        ")]
        public static ref T Subtract<T>(ref T source, int elementOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            conv.i
            mul
            sub
            ret
        ")]
        public static unsafe void* Subtract<T>(void* source, int elementOffset) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            mul
            sub
            ret
        ")]
        public static ref T Subtract<T>(ref T source, nint elementOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sizeof !!0
            mul
            sub
            ret
        ")]
        public static ref T Subtract<T>(ref T source, nuint elementOffset) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sub
            ret
        ")]
        public static ref T SubtractByteOffset<T>(ref T source, nint byteOffset) {
            throw null!;
        }

        [CLSCompliant(false)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sub
            ret
        ")]
        public static ref T SubtractByteOffset<T>(ref T source, nuint byteOffset) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            sub
            ret
        ")]
        public static nint ByteOffset<T>(ref T origin, ref T target) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            ceq
            ret
        ")]
        public static bool AreSame<T>(ref T left, ref T right) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            cgt.un
            ret
        ")]
        public static bool IsAddressGreaterThan<T>(ref T left, ref T right) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldarg.1
            clt.un
            ret
        ")]
        public static bool IsAddressLessThan<T>(ref T left, ref T right) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldarg.0
            ldc.i4.0
            conv.u
            ceq
            ret
        ")]
        public static bool IsNullRef<T>(ref T source) {
            throw null!;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [ILMethodBody(@"
            ldc.i4.0
            conv.u
            ret
        ")]
        public static ref T NullRef<T>() {
            throw null!;
        }
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
