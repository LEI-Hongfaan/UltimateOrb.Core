// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UltimateOrb.Functional.Pure;
using UltimateOrb.Runtime.CompilerServices;
using UltimateOrb.Utilities;
using static UltimateOrb.Functional.BooleanFunctorModule;
using static UltimateOrb.Utilities.Extensions.BooleanIntegerExtensions;

namespace UltimateOrb.Collections.Specialized {

    public static partial class BitArrayExtenstions {

        public readonly struct CancellationTokenAsSpinWait : IFunc<bool> {

            readonly CancellationToken _CancellationToken;

            public CancellationTokenAsSpinWait(CancellationToken cancellationToken) {
                this._CancellationToken = cancellationToken;
            }

            public bool Invoke() {
                _CancellationToken.ThrowIfCancellationRequested();
                return true;
            }
        }

        public static CancellationTokenAsSpinWait AsSpinWait(this CancellationToken cancellationToken) {
            return new CancellationTokenAsSpinWait(cancellationToken);
        }

        public struct WrappedSpinWaitWithSleep1Threshold : IFunc<bool> {

            SpinWait _SpinWait;

            readonly int sleep1Threshold;

            public WrappedSpinWaitWithSleep1Threshold(int sleep1Threshold) : this() {
                this.sleep1Threshold = sleep1Threshold;
            }

            public bool Invoke() {
                _SpinWait.SpinOnce(sleep1Threshold);
                return true;
            }
        }

        public static WrappedSpinWaitWithSleep1Threshold CreateSpinWait(int sleep1Threshold) {
            return new WrappedSpinWaitWithSleep1Threshold(sleep1Threshold);
        }

        public struct TimeoutSpinWait : IFunc<bool> {

            readonly Stopwatch _Stopwatch;

            readonly TimeSpan _Timeout;

            public TimeoutSpinWait(TimeSpan timeout) {
                _Timeout = timeout;
                _Stopwatch = new Stopwatch();
            }

            public void Start() {
                _Stopwatch.Start();
            }

            public bool Invoke() {
                return _Stopwatch.Elapsed >= _Timeout;
            }
        }

        public static TimeoutSpinWait CreateTimeoutSpinWait(TimeSpan timeout, bool startNow) {
            var r = new TimeoutSpinWait(timeout); if (startNow) {
                r.Start();
            }
            return r;
        }

        public static AndAlsoFunctor<TSpinWait1, TSpinWait2> Combine<TSpinWait1, TSpinWait2>(TSpinWait1 spinWait1, TSpinWait2 spinWait2)
            where TSpinWait1 : IFunc<bool>
            where TSpinWait2 : IFunc<bool> {
            return AndAlso(spinWait1, spinWait2);
        }

        public struct WrappedSpinWait : IFunc<bool> {

            SpinWait _SpinWait;

            public bool Invoke() {
                _SpinWait.SpinOnce();
                return true;
            }
        }

        public static WrappedSpinWait CreateSpinWait() {
            return default;
        }

        public static bool TryExchange<TSpinWait>(this ref BitArray64 bitArray, int index, bool value, out bool original, ref TSpinWait spinner)
             where TSpinWait : IFunc<bool> {
            return TryExchange<TSpinWait>(ref bitArray._Data, index, value, out original, ref spinner);
        }

        [CLSCompliantAttribute(false)]
        public static bool TryExchange<TSpinWait>(this ref UInt64 chunk, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            UInt64 actual_;
            if (value) {
                var bit = (UInt64)1 << unchecked((int)index);
                do {
                    var expected = Volatile.Read(ref chunk);
                    actual_ = Interlocked.CompareExchange(ref Unsafe.As<ulong, long>(ref chunk), (bit | expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                    if (expected == actual_) {
                        original = 0 != (bit & actual_);
                        return true;
                    }
                } while (spinner.Invoke());
                original = 0 != (bit & actual_);
                return false;
            } else {
                var nbit = ~((UInt64)1 << unchecked((int)index));
                do {
                    var expected = Volatile.Read(ref chunk);
                    actual_ = Interlocked.CompareExchange(ref Unsafe.As<ulong, long>(ref chunk), (nbit & expected).ToSignedUnchecked(), expected.ToSignedUnchecked()).ToUnsignedUnchecked();
                    if (expected == actual_) {
                        original = 0 != (~nbit & actual_);
                        return true;
                    }
                } while (spinner.Invoke());
                original = 0 != (~nbit & actual_);
                return false;
            }
        }

        public static bool TryExchange<TSpinWait>(this ref Int64 chunk, int index, bool value, out bool original, ref TSpinWait spinner)
            where TSpinWait : IFunc<bool> {
            return TryExchange(ref CilVerifiable.AsUnsigned(ref chunk), index, value, out original, ref spinner);
        }

        public static bool TryExchange(this ref Int64 chunk, int index, bool value, out bool original, CancellationToken cancellationToken = default) {
            var spinner = cancellationToken.AsSpinWait();
            return TryExchange(ref chunk, index, value, out original, ref spinner);
        }
    }


    /// <devdoc>
    ///    <para>Provides a simple light bit vector with easy integer or Boolean access to
    ///       a 64 bit storage.</para>
    /// </devdoc>
    public struct BitArray64 {

        internal UInt64 _Data;

        /// <devdoc>
        /// <para>Initializes a new instance of the BitArray64 structure with the specified internal data.</para>
        /// </devdoc>
        public BitArray64(Int64 data) {
            _Data = unchecked((UInt64)data);
        }

        /// <devdoc>
        /// <para>Initializes a new instance of the BitArray64 structure with the information in the specified
        ///    value.</para>
        /// </devdoc>
        public BitArray64(BitArray64 value) {
            _Data = value._Data;
        }

        /// <devdoc>
        ///    <para>Gets or sets a value indicating whether all the specified bits are set.</para>
        /// </devdoc>
        public bool this[int index] {

            get {
                if (64 <= unchecked((uint)index)) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return (1 & unchecked((int)(_Data >> index))).AsBooleanUnsafe();
            }

            set {
                if (64 <= unchecked((uint)index)) {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                if (value) {
                    _Data |= (UInt64)1 << index;
                } else {
                    _Data &= ~((UInt64)1 << index);
                }
            }
        }

        /// <devdoc>
        ///    returns the raw data stored in this bit vector...
        /// </devdoc>
        public Int64 Data {

            get {
                return unchecked((Int64)_Data);
            }
        }

        public override bool Equals(object? o) {
            if (!(o is BitArray64)) {
                return false;
            }

            return _Data == ((BitArray64)o)._Data;
        }

        public override int GetHashCode() {
            return _Data.GetHashCode();
        }

        public static string ToString(BitArray64 value) {
            return string.Create(/*"BitArray64{".Length*/12 + /*64 bits*/64 + /*"}".Length"*/1, value, (dst, v) => {
                ReadOnlySpan<char> prefix = "BitArray64{";
                prefix.CopyTo(dst);
                dst[^1] = '}';

                var locdata = unchecked((Int64)v._Data);
                dst = dst.Slice(prefix.Length, 64);
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