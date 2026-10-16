using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Runtime.CompilerServices {

    public struct InlineArray1<T> {

        InlineArrayBlock1<T> _data0;

        public int Length {

            get => 1;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray1<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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

        public int Length {

            get => 2;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray2<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray3<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray4<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray5<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray6<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray7<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray8<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray9<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray10<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray11<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray12<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray13<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray14<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray15<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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
                return ref Unsafe.Add(ref Unsafe.As<InlineArray16<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
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

    public struct InlineArray17<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock1<T> _data16;

        public int Length {

            get => 17;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray17<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray18<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock2<T> _data16;

        public int Length {

            get => 18;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray18<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray19<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock2<T> _data16;
        InlineArrayBlock1<T> _data18;

        public int Length {

            get => 19;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray19<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray20<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock4<T> _data16;

        public int Length {

            get => 20;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray20<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray21<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock4<T> _data16;
        InlineArrayBlock1<T> _data20;

        public int Length {

            get => 21;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray21<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray22<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock4<T> _data16;
        InlineArrayBlock2<T> _data20;

        public int Length {

            get => 22;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray22<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray23<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock4<T> _data16;
        InlineArrayBlock2<T> _data20;
        InlineArrayBlock1<T> _data22;

        public int Length {

            get => 23;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray23<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray24<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;

        public int Length {

            get => 24;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray24<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray25<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock1<T> _data24;

        public int Length {

            get => 25;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray25<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray26<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock2<T> _data24;

        public int Length {

            get => 26;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray26<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray27<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock2<T> _data24;
        InlineArrayBlock1<T> _data26;

        public int Length {

            get => 27;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray27<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray28<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock4<T> _data24;

        public int Length {

            get => 28;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray28<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray29<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock4<T> _data24;
        InlineArrayBlock1<T> _data28;

        public int Length {

            get => 29;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray29<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray30<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock4<T> _data24;
        InlineArrayBlock2<T> _data28;

        public int Length {

            get => 30;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray30<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray31<T> {

        InlineArrayBlock16<T> _data0;
        InlineArrayBlock8<T> _data16;
        InlineArrayBlock4<T> _data24;
        InlineArrayBlock2<T> _data28;
        InlineArrayBlock1<T> _data30;

        public int Length {

            get => 31;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray31<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray32<T> {

        InlineArrayBlock32<T> _data0;

        public int Length {

            get => 32;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray32<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray33<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock1<T> _data32;

        public int Length {

            get => 33;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray33<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray34<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock2<T> _data32;

        public int Length {

            get => 34;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray34<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray35<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock2<T> _data32;
        InlineArrayBlock1<T> _data34;

        public int Length {

            get => 35;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray35<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray36<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock4<T> _data32;

        public int Length {

            get => 36;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray36<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray37<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock4<T> _data32;
        InlineArrayBlock1<T> _data36;

        public int Length {

            get => 37;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray37<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray38<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock4<T> _data32;
        InlineArrayBlock2<T> _data36;

        public int Length {

            get => 38;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray38<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray39<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock4<T> _data32;
        InlineArrayBlock2<T> _data36;
        InlineArrayBlock1<T> _data38;

        public int Length {

            get => 39;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray39<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray40<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;

        public int Length {

            get => 40;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray40<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray41<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock1<T> _data40;

        public int Length {

            get => 41;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray41<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray42<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock2<T> _data40;

        public int Length {

            get => 42;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray42<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray43<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock2<T> _data40;
        InlineArrayBlock1<T> _data42;

        public int Length {

            get => 43;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray43<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray44<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock4<T> _data40;

        public int Length {

            get => 44;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray44<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray45<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock4<T> _data40;
        InlineArrayBlock1<T> _data44;

        public int Length {

            get => 45;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray45<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray46<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock4<T> _data40;
        InlineArrayBlock2<T> _data44;

        public int Length {

            get => 46;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray46<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray47<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock8<T> _data32;
        InlineArrayBlock4<T> _data40;
        InlineArrayBlock2<T> _data44;
        InlineArrayBlock1<T> _data46;

        public int Length {

            get => 47;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray47<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray48<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;

        public int Length {

            get => 48;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray48<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray49<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock1<T> _data48;

        public int Length {

            get => 49;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray49<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray50<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock2<T> _data48;

        public int Length {

            get => 50;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray50<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray51<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock2<T> _data48;
        InlineArrayBlock1<T> _data50;

        public int Length {

            get => 51;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray51<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray52<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock4<T> _data48;

        public int Length {

            get => 52;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray52<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray53<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock4<T> _data48;
        InlineArrayBlock1<T> _data52;

        public int Length {

            get => 53;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray53<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray54<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock4<T> _data48;
        InlineArrayBlock2<T> _data52;

        public int Length {

            get => 54;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray54<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray55<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock4<T> _data48;
        InlineArrayBlock2<T> _data52;
        InlineArrayBlock1<T> _data54;

        public int Length {

            get => 55;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray55<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray56<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;

        public int Length {

            get => 56;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray56<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray57<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock1<T> _data56;

        public int Length {

            get => 57;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray57<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray58<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock2<T> _data56;

        public int Length {

            get => 58;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray58<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray59<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock2<T> _data56;
        InlineArrayBlock1<T> _data58;

        public int Length {

            get => 59;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray59<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray60<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock4<T> _data56;

        public int Length {

            get => 60;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray60<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray61<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock4<T> _data56;
        InlineArrayBlock1<T> _data60;

        public int Length {

            get => 61;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray61<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray62<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock4<T> _data56;
        InlineArrayBlock2<T> _data60;

        public int Length {

            get => 62;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray62<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray63<T> {

        InlineArrayBlock32<T> _data0;
        InlineArrayBlock16<T> _data32;
        InlineArrayBlock8<T> _data48;
        InlineArrayBlock4<T> _data56;
        InlineArrayBlock2<T> _data60;
        InlineArrayBlock1<T> _data62;

        public int Length {

            get => 63;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray63<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

    public struct InlineArray64<T> {

        InlineArrayBlock64<T> _data0;

        public int Length {

            get => 64;
        }

        [UnscopedRef]
        public ref T this[int index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), (nint)index); // The (nint) conversion works around an ILGPU compiler bug; we can remove it once they provide a fix.
            }
        }

        [UnscopedRef]
        public ref T this[uint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), (nuint)index);
            }
        }

        [UnscopedRef]
        public ref T this[nint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[nuint index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), index);
            }
        }

        [UnscopedRef]
        public ref T this[long index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), unchecked((nint)index));
            }
        }

        [UnscopedRef]
        public ref T this[ulong index] {

            get {
                Debug.Assert(index.ToUnsignedUnchecked() < Length.ToUnsignedUnchecked());
                return ref Unsafe.Add(ref Unsafe.As<InlineArray64<T>, T>(ref this), unchecked((nuint)index));
            }
        }
    }

}