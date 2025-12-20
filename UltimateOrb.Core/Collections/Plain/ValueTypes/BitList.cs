using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Collections.Specialized;
using UltimateOrb.Utilities;

namespace UltimateOrb.Collections.Plain.ValueTypes {
    using static BitList;

    public static class BitListExtensions {

        public static ref BitVector32[] GetChunks(this ref BitList list) {
            return ref Unsafe.As<Int32[], BitVector32[]>(ref list.Array);
        }

        public static bool Add<TSpinWait>(this BitListCore list, bool value, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            var sdfasd = DigitBitSize * list.Array.Value.Length;
            throw new NotImplementedException();
        }

        public static bool TryCompareExchange<TSpinWait>(this BitListCore list, int index, bool value, bool comparand, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return BitArrayExtensions.TryCompareExchange(
                ref list.Array.Value[index >> Log2DigitBitSize],
                DigitBitIndexMask & index,
                value, comparand, out original, ref spinner);
        }

        public static bool TryCompareExchange<TSpinWait>(this ref BitList list, int index, bool value, bool comparand, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return TryCompareExchange(
                list.Core,
                DigitBitIndexMask & index,
                value, comparand, out original, ref spinner);
        }

        public static bool TryExchange<TSpinWait>(this ref BitList list, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return BitArrayExtensions.TryExchange(
                ref list.Array[index >> Log2DigitBitSize],
                DigitBitIndexMask & index,
                value, out original, ref spinner);
        }
    }



    public readonly ref struct BitListCore {

        public readonly ByReference<Array<Int32>> Array;

        public readonly ByReference<long> Count;

        public BitListCore(ref int[] array, ref long count) : this() {
            Array = new ByReference<Array<Int32>>(ref Unsafe.As<Int32[], Array<Int32>>(ref array));
            Count = new ByReference<long>(ref count);
        }
    }

    public struct BitList {

        public BitListCore Core {

            get {
                unsafe {
                    fixed (void * p = &Count) {
                       return new BitListCore(ref Unsafe.AsRef<Int32[]>(Unsafe.AsPointer(ref Array)), ref Unsafe.AsRef<long>(p));
                    }
                }
            }
        }

        public const int DigitBitSize = 32;

        public const int Log2DigitBitSize = 5;

        public const Int32 One = 1;

        public const int DigitBitIndexMask = 32 - 1;

        public Int32[] Array;

        public long Count;
    }
}
