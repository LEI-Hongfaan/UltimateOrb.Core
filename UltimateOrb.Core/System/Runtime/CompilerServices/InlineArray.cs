using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;





// polyfill
#if !NET10_0_OR_GREATER
namespace System.Runtime.CompilerServices {

    struct InlineArrayBlock1<T> {

        public T Value;
    }

    struct InlineArrayBlock2<T> {

        public InlineArrayBlock1<T> Lower;

        public InlineArrayBlock1<T> Upper;
    }

    struct InlineArrayBlock4<T> {

        public InlineArrayBlock2<T> Lower;

        public InlineArrayBlock2<T> Upper;
    }

    struct InlineArrayBlock8<T> {

        public InlineArrayBlock4<T> Lower;

        public InlineArrayBlock4<T> Upper;
    }

    struct InlineArrayBlock16<T> {

        public InlineArrayBlock8<T> Lower;

        public InlineArrayBlock8<T> Upper;
    }

    public struct InlineArray1<T> {

        InlineArrayBlock1<T> _data0;

        public int Length {

            get => 1;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray2<T> {

        InlineArrayBlock2<T> _data0;

        /*
        Span<T> AsSpan() {
            return MemoryMarshal.CreateSpan(ref this[0], Length);
        }

        public InlineArray2(params ReadOnlySpan<T> values) : this() {
            var vs = values.Length.ToUnsignedUnchecked() > Length.ToUnsignedUnchecked() ?
                values[..Length] : values;
            vs.CopyTo(AsSpan());
        }
        */

        public int Length {

            get => 2;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray3<T> {

        InlineArrayBlock2<T> _data0;

        InlineArrayBlock1<T> _data2;

        public int Length {

            get => 3;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray4<T> {

        InlineArrayBlock4<T> _data0;

        public int Length {

            get => 4;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray5<T> {

        InlineArrayBlock4<T> _data0;

        InlineArrayBlock1<T> _data4;

        public int Length {

            get => 5;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray6<T> {

        InlineArrayBlock4<T> _data0;

        InlineArrayBlock2<T> _data4;

        public int Length {

            get => 6;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray7<T> {

        InlineArrayBlock4<T> _data0;

        InlineArrayBlock2<T> _data4;

        InlineArrayBlock1<T> _data6;

        public int Length {

            get => 7;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray8<T> {

        InlineArrayBlock8<T> _data0;

        public int Length {

            get => 8;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray9<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock1<T> _data8;

        public int Length {

            get => 9;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray10<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock2<T> _data8;

        public int Length {

            get => 10;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray11<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock2<T> _data8;

        InlineArrayBlock1<T> _data10;

        public int Length {

            get => 11;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray12<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock4<T> _data8;

        public int Length {

            get => 12;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray13<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock4<T> _data8;

        InlineArrayBlock1<T> _data12;

        public int Length {

            get => 13;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray14<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock4<T> _data8;

        InlineArrayBlock2<T> _data12;

        public int Length {

            get => 14;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray15<T> {

        InlineArrayBlock8<T> _data0;

        InlineArrayBlock4<T> _data8;

        InlineArrayBlock2<T> _data12;

        InlineArrayBlock1<T> _data14;

        public int Length {

            get => 15;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray16<T> {

        InlineArrayBlock16<T> _data0;

        public int Length {

            get => 16;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }
}
#endif