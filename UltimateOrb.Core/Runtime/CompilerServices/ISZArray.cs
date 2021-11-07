using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Runtime.CompilerServices {
    using Local = Interfaces.Typed_RefReturn_Wrapped_Huge;

    interface ISZArray {

        nint NativeLength {

            get;
        }

        int Length {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => checked((int)NativeLength);
        }

        long LongLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => NativeLength;
        }
    }

    interface ISZArray<T>
        : ISZArray
        , Local.IKeyedValueProvider<TypeTokens.Element, int, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, uint, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, long, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, ulong, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, nint, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, nuint, T> {

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, int, T>.this[int key] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nint, T>)this)[key];
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, uint, T>.this[uint key] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nuint, T>)this)[key];
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, long, T>.this[long key] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nint, T>)this)[checked((nint)key)];
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, ulong, T>.this[ulong key] {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nuint, T>)this)[checked((nuint)key)];
        }
    }

    interface ISZHugeArray : ISZArray {

        nint ISZArray.NativeLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => checked((nint)LongLength);
        }

        int ISZArray.Length {

            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get => checked((int)LongLength);
        }

        abstract long ISZArray.LongLength {

            get;
        }
    }

    interface ISZHugeArray<T>
        : ISZHugeArray
        , ISZArray<T>
        , Local.IKeyedValueProvider<TypeTokens.Element, int, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, uint, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, long, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, ulong, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, nint, T>
        , Local.IKeyedValueProvider<TypeTokens.Element, nuint, T> {

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, int, T>.this[int key] {

            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, long, T>)this)[key];
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, uint, T>.this[uint key] {

            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, ulong, T>)this)[key];
        }

        abstract ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, long, T>.this[long key] {

            get;
        }

        abstract ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, ulong, T>.this[ulong key] {

            get;
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nint, T>.this[nint key] {

            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, long, T>)this)[key];
        }

        ref T Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, nuint, T>.this[nuint key] {

            get => ref ((Interfaces.RefReturn.IKeyedValueProvider<TypeTokens.Element, ulong, T>)this)[key];
        }
    }
}
