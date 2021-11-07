﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static UltimateOrb.Utilities.SignConverter;

namespace UltimateOrb.Buffers.Unmanaged {

    public readonly struct BufferView2D<T> where T : unmanaged {

        unsafe readonly T* _Base;

        readonly nuint _Length0;

        readonly nuint _Stride0;

        readonly nuint _Length1;

        public ref T this[nint index0, nint index1] {

            get {
                if (_Length0 > unchecked((nuint)index0) &&
                    _Length1 > unchecked((nuint)index1)) {
                    unsafe {
                        return ref _Base[unchecked(_Length1 * (nuint)index0 + (nuint)index1)];
                    }
                }
                throw new IndexOutOfRangeException();
            }
        }

        [CLSCompliant(false)]
        public unsafe BufferView2D(T* data, nint length0, nint length1) : this(data, length0, length1, length0) {
        }

        [CLSCompliant(false)]
        public unsafe BufferView2D(T* data, nint length0, nint length1, nint stride0) {
            _ = checked((nuint)length0);
            _ = checked((nuint)length1);
            _ = checked((nuint)stride0);
            _ = checked(unchecked((nuint)stride0) - unchecked((nuint)length1));
            if (0 != length0 && 0 != length1) {
                _ = *(byte*)data;
            }
            _Base = data;
            _Length0 = unchecked((nuint)length0);
            _Length1 = unchecked((nuint)length1);
            _Stride0 = unchecked((nuint)stride0);
        }

        public T[,]? ToStandardArray2D() {
            unsafe {
                if (0 != (nuint)_Base) {
                    var a = new T[_Length0, _Length1];
                    var cb = unchecked((uint)sizeof(T) * _Length0 * _Length1);
                    fixed (T* p = a) {
                        Buffer.MemoryCopy(_Base, p, cb, cb);
                    }
                    return a;
                }
            }
            return null;
        }

        public int Length {

            get => checked((int)unchecked(_Length0 * _Length1));
        }

        public long LongLength {

            get => unchecked((long)unchecked(_Length0 * _Length1));
        }

        public int Length0 {

            get => checked((int)_Length0);
        }

        public long LongLength0 {

            get => unchecked((long)_Length0);
        }

        public int Length1 {

            get => checked((int)_Length1);
        }

        public long LongLength1 {

            get => unchecked((long)_Length1);
        }
    }
}
