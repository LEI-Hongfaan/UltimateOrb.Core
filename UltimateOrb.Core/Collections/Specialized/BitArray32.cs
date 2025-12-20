// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UltimateOrb.Utilities;
using static UltimateOrb.Functional.BooleanFunctorModule;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;
using static UltimateOrb.Miscellaneous;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Collections.Specialized {

    public static partial class BitArrayExtensions {

        [CLSCompliantAttribute(false)]
        public static bool TryCompareExchange<TSpinWait>(this ref UInt32 chunk, int index, bool value, bool comparand, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            UInt32 actual_;
            if (Unlikely(value == comparand)) {
                if (value) {
                    var bit = (UInt32)1 << unchecked((int)index);
                    do {
                        if (0 != (bit & Volatile.Read(ref chunk))) {
                            original = true;
                            return true;
                        }
                    } while (spinner.Invoke());
                    original = false;
                    return false;
                } else {
                    var bit = (UInt32)1 << unchecked((int)index);
                    do {
                        if (0 == (bit & Volatile.Read(ref chunk))) {
                            original = false;
                            return true;
                        }
                    } while (spinner.Invoke());
                    original = true;
                    return false;
                }
            }
            {
                var bit = (UInt32)1 << unchecked((int)index);
                var nbit = ~bit;
                if (value) {
                    do {
                        var expected = nbit & Volatile.Read(ref chunk);
                        actual_ = Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref chunk), (bit | expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                        if (expected == actual_) {
                            original = 0 != (bit & actual_);
                            return true;
                        }
                    } while (spinner.Invoke());
                    original = 0 != (bit & actual_);
                    return false;
                } else {
                    do {
                        var expected = bit | Volatile.Read(ref chunk);
                        actual_ = Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref chunk), (nbit & expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                        if (expected == actual_) {
                            original = 0 != (~nbit & actual_);
                            return true;
                        }
                    } while (spinner.Invoke());
                    original = 0 != (~nbit & actual_);
                    return false;
                }
            }
        }

        public static bool TryCompareExchange<TSpinWait>(this ref Int32 chunk, int index, bool value, bool comparand, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return TryCompareExchange(ref CilVerifiable.AsUnsigned(ref chunk), index, value, comparand, out original, ref spinner);
        }

        public static bool TryExchange<TSpinWait>(this ref BitArray32 bitArray, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return TryExchange<TSpinWait>(ref bitArray._Data, index, value, out original, ref spinner);
        }

        [CLSCompliantAttribute(false)]
        public static bool TryExchange<TSpinWait>(this ref UInt32 chunk, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            UInt32 actual_;
            if (value) {
                var bit = (UInt32)1 << unchecked((int)index);
                do {
                    var expected = Volatile.Read(ref chunk);
                    actual_ = Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref chunk), (bit | expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                    if (expected == actual_) {
                        original = 0 != (bit & actual_);
                        return true;
                    }
                } while (spinner.Invoke());
                original = 0 != (bit & actual_);
                return false;
            } else {
                var nbit = ~((UInt32)1 << unchecked((int)index));
                do {
                    var expected = Volatile.Read(ref chunk);
                    actual_ = Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref chunk), (nbit & expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                    if (expected == actual_) {
                        original = 0 != (~nbit & actual_);
                        return true;
                    }
                } while (spinner.Invoke());
                original = 0 != (~nbit & actual_);
                return false;
            }
        }

        [CLSCompliantAttribute(false)]
        public static bool TryExchange(this ref UInt32 chunk, int index, bool value, out bool original) {
            var spinner = CreateSpinWait();
            return TryExchange(ref chunk, index, value, out original, ref spinner);
        }

        [CLSCompliantAttribute(false)]
        public static bool TryExchange(this ref UInt32 chunk, int index, bool value, out bool original, CancellationToken cancellationToken) {
            if (cancellationToken.CanBeCanceled) {
                var spinner = Combine(CreateSpinWait(), cancellationToken.AsSpinWait());
                return TryExchange(ref chunk, index, value, out original, ref spinner);
            } else {
                var spinner = CreateSpinWait();
                return TryExchange(ref chunk, index, value, out original, ref spinner);
            }
        }

        public static bool TryExchange<TSpinWait>(this ref Int32 chunk, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return TryExchange(ref CilVerifiable.AsUnsigned(ref chunk), index, value, out original, ref spinner);
        }

        public static bool TryExchange(this ref Int32 chunk, int index, bool value, out bool original, CancellationToken cancellationToken) {
            if (cancellationToken.CanBeCanceled) {
                var spinner = Combine(CreateSpinWait(), cancellationToken.AsSpinWait());
                return TryExchange(ref chunk, index, value, out original, ref spinner);
            } else {
                var spinner = CreateSpinWait();
                return TryExchange(ref chunk, index, value, out original, ref spinner);
            }
        }
    }

    /// <devdoc>
    ///    <para>Provides a simple light bit vector with easy integer or Boolean access to
    ///       a 64 bit storage.</para>
    /// </devdoc>
    public struct BitArray32 {

        internal UInt32 _Data;

        /// <devdoc>
        /// <para>Initializes a new instance of the BitArray64 structure with the specified internal data.</para>
        /// </devdoc>
        public BitArray32(Int32 data) {
            _Data = unchecked((UInt32)data);
        }

        /// <devdoc>
        /// <para>Initializes a new instance of the BitArray64 structure with the information in the specified
        ///    value.</para>
        /// </devdoc>
        public BitArray32(BitArray32 value) {
            _Data = value._Data;
        }

        /// <devdoc>
        ///    <para>Gets or sets a value indicating whether all the specified bits are set.</para>
        /// </devdoc>
        public bool this[int index] {

            get {
                if (32 <= unchecked((uint)index)) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return (1 & unchecked((int)(_Data >> index))).AsBooleanUnsafe();
            }

            set {
                if (32 <= unchecked((uint)index)) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (value) {
                    _Data |= (UInt32)1 << index;
                } else {
                    _Data &= ~((UInt32)1 << index);
                }
            }
        }

        /// <devdoc>
        ///    returns the raw data stored in this bit vector...
        /// </devdoc>
        public Int64 Data {

            get {
                return unchecked((Int32)_Data);
            }
        }

        public override bool Equals(object? o) {
            if (!(o is BitArray32)) {
                return false;
            }

            return _Data == ((BitArray32)o)._Data;
        }

        public override int GetHashCode() {
            return _Data.GetHashCode();
        }

        public static string ToString(BitArray32 value) {
            return string.Create(/*"BitArray32{".Length*/12 + /*32 bits*/32 + /*"}".Length"*/1, value, (dst, v) => {
                ReadOnlySpan<char> prefix = "BitArray32{";
                prefix.CopyTo(dst);
                dst[^1] = '}';

                var locdata = unchecked((Int64)v._Data);
                dst = dst.Slice(prefix.Length, 32);
                for (int i = 0; i < dst.Length; i++) {
                    dst[i] = (0 > locdata) ? '1' : '0';
                    locdata <<= 1;
                }
            });
        }

        public override string ToString() {
            return ToString(this);
        }
    }
}
