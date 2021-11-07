// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*
The xxHash32 implementation is based on the code published by Yann Collet:
https://raw.githubusercontent.com/Cyan4973/xxHash/5c174cfa4e45a42f94082dc0d4539b39696afea1/xxhash.c
  xxHash - Fast Hash algorithm
  Copyright (C) 2012-2016, Yann Collet
  BSD 2-Clause License (http://www.opensource.org/licenses/bsd-license.php)
  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are
  met:
  * Redistributions of source code must retain the above copyright
  notice, this list of conditions and the following disclaimer.
  * Redistributions in binary form must reproduce the above
  copyright notice, this list of conditions and the following disclaimer
  in the documentation and/or other materials provided with the
  distribution.
  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
  OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
  SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
  LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
  OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
  You can contact the author at :
  - xxHash homepage: http://www.xxhash.com
  - xxHash source repository : https://github.com/Cyan4973/xxHash
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
namespace UltimateOrb.Reflection.Traits {

}
    namespace UltimateOrb {

    public static class LongHashCodeExtenstions {
        
        public static long GetLongHashCode<T>(this T obj) {
            if (typeof(T).IsValueType) {
                if (typeof(T) == typeof(byte)) {
                    return (nint)typeof(byte).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(sbyte)) {
                    return (nint)typeof(sbyte).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(short)) {
                    return (nint)typeof(short).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(ushort)) {
                    return (nint)typeof(ushort).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(char)) {
                    return (nint)typeof(char).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(int)) {
                    return (nint)typeof(int).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(uint)) {
                    return (nint)typeof(uint).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(int)) {
                    return (nint)typeof(int).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(uint)) {
                    return (nint)typeof(uint).TypeHandle.Value ^ ((long)obj!.GetHashCode() << sizeof(int));
                }
                if (typeof(T) == typeof(long)) {
                    return (nint)typeof(long).TypeHandle.Value ^ ((long)(object)obj!);
                }
                if (typeof(T) == typeof(ulong)) {
                    return (nint)typeof(ulong).TypeHandle.Value ^ ((long)(object)obj!);
                }
                if (typeof(T) == typeof(Guid)) {
                    return (nint)typeof(ulong).TypeHandle.Value ^ ((long)(object)obj!);
                }
            }
            throw new NotImplementedException();
        }
    }

    // xxHash32 is used for the hash code.
    // https://github.com/Cyan4973/xxHash

    public struct LongHashCode {
        private static readonly ulong s_seed = GenerateGlobalSeed();

        private const ulong Prime1 = 0x9E3779B185EBCA87U;
        private const ulong Prime2 = 0xC2B2AE3D27D4EB4FU;
        private const ulong Prime3 = 0x165667B19E3779F9U;
        private const ulong Prime4 = 0x85EBCA77C2B2AE63U;
        private const ulong Prime5 = 0x27D4EB2F165667C5U;

        private ulong _v1, _v2, _v3, _v4;
        private ulong _queue1, _queue2, _queue3;
        private ulong _length;

        private static ulong GenerateGlobalSeed() {
            Span<ulong> result = stackalloc ulong[1];
            RandomNumberGenerator.Fill(MemoryMarshal.AsBytes(result));
            return result[0];
        }

        public static long Combine<T1>(T1 value1) {
            // Provide a way of diffusing bits from something with a limited
            // input hash space. For example, many enums only have a few
            // possible hashes, only using the bottom few bits of the code. Some
            // collections are built on the assumption that hashes are spread
            // over a larger space, so diffusing the bits may help the
            // collection work more efficiently.

            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));

            ulong hash = MixEmptyState();
            unchecked { hash += 4; }

            hash = QueueRound(hash, hc1);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2>(T1 value1, T2 value2) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));

            ulong hash = MixEmptyState();
            unchecked { hash += 8; }

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3>(T1 value1, T2 value2, T3 value3) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));

            ulong hash = MixEmptyState();
            unchecked { hash += 12; }

            hash = QueueRound(hash, hc1);
            hash = QueueRound(hash, hc2);
            hash = QueueRound(hash, hc3);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));
            ulong hc4 = unchecked((ulong)(value4?.GetLongHashCode() ?? 0));

            Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            ulong hash = MixState(v1, v2, v3, v4);
            unchecked { hash += 16; }

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3, T4, T5>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));
            ulong hc4 = unchecked((ulong)(value4?.GetLongHashCode() ?? 0));
            ulong hc5 = unchecked((ulong)(value5?.GetLongHashCode() ?? 0));

            Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            ulong hash = MixState(v1, v2, v3, v4);
            unchecked { hash += 20; }

            hash = QueueRound(hash, hc5);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3, T4, T5, T6>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));
            ulong hc4 = unchecked((ulong)(value4?.GetLongHashCode() ?? 0));
            ulong hc5 = unchecked((ulong)(value5?.GetLongHashCode() ?? 0));
            ulong hc6 = unchecked((ulong)(value6?.GetLongHashCode() ?? 0));

            Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            ulong hash = MixState(v1, v2, v3, v4);
            unchecked { hash += 24; }

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3, T4, T5, T6, T7>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));
            ulong hc4 = unchecked((ulong)(value4?.GetLongHashCode() ?? 0));
            ulong hc5 = unchecked((ulong)(value5?.GetLongHashCode() ?? 0));
            ulong hc6 = unchecked((ulong)(value6?.GetLongHashCode() ?? 0));
            ulong hc7 = unchecked((ulong)(value7?.GetLongHashCode() ?? 0));

            Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            ulong hash = MixState(v1, v2, v3, v4);
            unchecked { hash += 28; }

            hash = QueueRound(hash, hc5);
            hash = QueueRound(hash, hc6);
            hash = QueueRound(hash, hc7);

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        public static long Combine<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8) {
            ulong hc1 = unchecked((ulong)(value1?.GetLongHashCode() ?? 0));
            ulong hc2 = unchecked((ulong)(value2?.GetLongHashCode() ?? 0));
            ulong hc3 = unchecked((ulong)(value3?.GetLongHashCode() ?? 0));
            ulong hc4 = unchecked((ulong)(value4?.GetLongHashCode() ?? 0));
            ulong hc5 = unchecked((ulong)(value5?.GetLongHashCode() ?? 0));
            ulong hc6 = unchecked((ulong)(value6?.GetLongHashCode() ?? 0));
            ulong hc7 = unchecked((ulong)(value7?.GetLongHashCode() ?? 0));
            ulong hc8 = unchecked((ulong)(value8?.GetLongHashCode() ?? 0));

            Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4);

            v1 = Round(v1, hc1);
            v2 = Round(v2, hc2);
            v3 = Round(v3, hc3);
            v4 = Round(v4, hc4);

            v1 = Round(v1, hc5);
            v2 = Round(v2, hc6);
            v3 = Round(v3, hc7);
            v4 = Round(v4, hc8);

            ulong hash = MixState(v1, v2, v3, v4);
            unchecked { hash += 32; }

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Initialize(out ulong v1, out ulong v2, out ulong v3, out ulong v4) {
            v1 = s_seed + Prime1 + Prime2;
            v2 = s_seed + Prime2;
            v3 = s_seed;
            v4 = s_seed - Prime1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong Round(ulong hash, ulong input) {
            return BitOperations.RotateLeft(hash + input * Prime2, 31) * Prime1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong QueueRound(ulong hash, ulong queuedValue) {
            return (hash ^ Round(0, queuedValue)) * Prime1 + Prime4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong MixState(ulong v1, ulong v2, ulong v3, ulong v4) {
            return BitOperations.RotateLeft(v1, 1) + BitOperations.RotateLeft(v2, 7) + BitOperations.RotateLeft(v3, 12) + BitOperations.RotateLeft(v4, 18);
        }

        private static ulong MixEmptyState() {
            return s_seed + Prime5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong MixFinal(ulong hash) {
            hash ^= hash >> 33;
            hash *= Prime2;
            hash ^= hash >> 29;
            hash *= Prime3;
            hash ^= hash >> 32;
            return hash;
        }

        public void Add<T>(T value) {
            Add(value?.GetLongHashCode() ?? 0);
        }

        public void Add<T>(T value, IEqualityComparer<T>? comparer) {
            Add(value is null ? 0 : (comparer?.GetHashCode(value) ?? value.GetHashCode()));
        }

        private void Add(long value) {
            // The original xxHash works as follows:
            // 0. Initialize immediately. We can't do this in a struct (no
            //    default ctor).
            // 1. Accumulate blocks of length 16 (4 uints) into 4 accumulators.
            // 2. Accumulate remaining blocks of length 4 (1 ulong) into the
            //    hash.
            // 3. Accumulate remaining blocks of length 1 into the hash.

            // There is no need for #3 as this type only accepts ints. _queue1,
            // _queue2 and _queue3 are basically a buffer so that when
            // ToHashCode is called we can execute #2 correctly.

            // We need to initialize the xxHash32 state (_v1 to _v4) lazily (see
            // #0) nd the last place that can be done if you look at the
            // original code is just before the first block of 16 bytes is mixed
            // in. The xxHash32 state is never used for streams containing fewer
            // than 16 bytes.

            // To see what's really going on here, have a look at the Combine
            // methods.

            ulong val = (ulong)value;

            // Storing the value of _length locally shaves of quite a few bytes
            // in the resulting machine code.
            ulong previousLength = _length++;
            ulong position = previousLength % 4;

            // Switch can't be inlined.

            if (position == 0) {
                _queue1 = val;
            } else if (position == 1) {
                _queue2 = val;
            } else if (position == 2) {
                _queue3 = val;
            } else // position == 3
            {
                if (previousLength == 3) {
                    Initialize(out _v1, out _v2, out _v3, out _v4);
                }

                _v1 = Round(_v1, _queue1);
                _v2 = Round(_v2, _queue2);
                _v3 = Round(_v3, _queue3);
                _v4 = Round(_v4, val);
            }
        }

        public long ToHashCode() {
            // Storing the value of _length locally shaves of quite a few bytes
            // in the resulting machine code.
            ulong length = _length;

            // position refers to the *next* queue position in this method, so
            // position == 1 means that _queue1 is populated; _queue2 would have
            // been populated on the next call to Add.
            ulong position = length % 4;

            // If the length is less than 4, _v1 to _v4 don't contain anything
            // yet. xxHash32 treats this differently.

            ulong hash = length < 4 ? MixEmptyState() : MixState(_v1, _v2, _v3, _v4);

            // _length is incremented once per Add(Int32) and is therefore 4
            // times too small (xxHash length is in bytes, not ints).

            hash += length * 4;

            // Mix what remains in the queue

            // Switch can't be inlined right now, so use as few branches as
            // possible by manually excluding impossible scenarios (position > 1
            // is always false if position is not > 0).
            if (position > 0) {
                hash = QueueRound(hash, _queue1);
                if (position > 1) {
                    hash = QueueRound(hash, _queue2);
                    if (position > 2) {
                        hash = QueueRound(hash, _queue3);
                    }
                }
            }

            hash = MixFinal(hash);
            return unchecked((long)hash);
        }

#pragma warning disable 0809
        // Obsolete member 'memberA' overrides non-obsolete member 'memberB'.
        // Disallowing GetHashCode and Equals is by design

        // * We decided to not override GetHashCode() to produce the hash code
        //   as this would be weird, both naming-wise as well as from a
        //   behavioral standpoint (GetHashCode() should return the object's
        //   hash code, not the one being computed).

        // * Even though ToHashCode() can be called safely multiple times on
        //   this implementation, it is not part of the contract. If the
        //   implementation has to change in the future we don't want to worry
        //   about people who might have incorrectly used this type.

        [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes. Use ToHashCode to retrieve the computed hash code.", error: true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => default(HashCode).GetHashCode();

        [Obsolete("HashCode is a mutable struct and should not be compared with other HashCodes.", error: true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj) => default(HashCode).Equals(obj);
#pragma warning restore 0809
    }
}