using System;
using System.Diagnostics.Contracts;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UltimateOrb.Numerics;
using static UltimateOrb.Utilities.SignConverter;

namespace UltimateOrb.Mathematics {

#if INDEPENDENT_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BinaryNumerals {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [PureAttribute()]
        public static int ComputeParityCheckBit(byte v) {
            return unchecked((byte)((((v * 0x0101010101010101ul) & 0x8040201008040201ul) % 0x1FF) & 1));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ComputeParityCheckBit(sbyte v) {
            return ComputeParityCheckBit(unchecked((byte)v));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ComputeParityCheckBit(UInt32 v) {
            v ^= v >> 1;
            v ^= v >> 2;
            v = unchecked((v & 0x11111111u) * 0x11111111u);
            return unchecked((byte)((v >> 28) & 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ComputeParityCheckBit(Int32 v) {
            return ComputeParityCheckBit(unchecked((UInt32)v));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ComputeParityCheckBit(UInt64 v) {
            v ^= v >> 1;
            v ^= v >> 2;
            v = unchecked((v & 0x1111111111111111ul) * 0x1111111111111111ul);
            return unchecked((byte)((v >> 60) & 1));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ComputeParityCheckBit(Int64 v) {
            return ComputeParityCheckBit(unchecked((Int64)v));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountLeadingZeros(UInt64 w) {
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.Result<int>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<int>() <= 64);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(w);
#else
            if (0u == w) {
                return 64;
            }
            var v = unchecked((UInt32)(w >> 32));
            var r = 31;
            if (0u == v) {
                v = unchecked((UInt32)w);
                r = 63;
            }
            if (v > 0xFFFFu) {
                v >>= 16;
                unchecked {
                    r -= 16;
                }
            }
            if (v > 0xFFu) {
                v >>= 8;
                unchecked {
                    r -= 8;
                }
            }
            if (v > 0xFu) {
                v >>= 4;
                unchecked {
                    r -= 4;
                }
            }
            {
                unchecked {
                    r -= 0x3 & unchecked((int)((Int32)0b11111111_11111111_10101010_01010011 >> ((int)v << 1)));
                }
            }
            return r;
#endif
        }

        [System.CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountLeadingZeros(UltimateOrb.UInt128 w) {
            return DoubleArithmetic.CountLeadingZeros(GetLowPart(w), GetHighPart(w));
        }

#if NET7_0_OR_GREATER
        [System.CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountLeadingZeros(System.UInt128 w) {
            return DoubleArithmetic.CountLeadingZeros(GetLowPart(w), GetHighPart(w));
        }
#endif

#if !INDEPENDENT_XINTN_LIBRARY || FEATURE_XINTN_INT256
        [System.CLSCompliantAttribute(false)]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int CountLeadingZeros(UltimateOrb.UInt256 w) {
            return DoubleArithmetic.CountLeadingZeros(GetLowPart(w), GetHighPart(w));
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountLeadingZeros(byte value) {
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.Result<int>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<int>() <= 32);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(value);
#else
            var v = unchecked((uint)value);
            if (0u == v) {
                return 8;
            }
            var r = 7;
            if (v > 0xFu) {
                v >>= 4;
                unchecked {
                    r -= 4;
                }
            }
            {
                unchecked {
                    r -= 0x3 & unchecked((int)((Int32)0b11111111_11111111_10101010_01010011 >> ((int)v << 1)));
                }
            }
            return r;
#endif
        }

        // 5.00 Cyc
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountLeadingZeros(UInt32 v) {
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.Result<int>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<int>() <= 32);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.LeadingZeroCount(v);
#else
            if (0u == v) {
                return 32;
            }
            var r = 31;
            if (v > 0xFFFFu) {
                v >>= 16;
                unchecked {
                    r -= 16;
                }
            }
            if (v > 0xFFu) {
                v >>= 8;
                unchecked {
                    r -= 8;
                }
            }
            if (v > 0xFu) {
                v >>= 4;
                unchecked {
                    r -= 4;
                }
            }
            {
                unchecked {
                    r -= 0x3 & unchecked((int)((Int32)0b11111111_11111111_10101010_01010011 >> ((int)v << 1)));
                }
            }
            return r;
#endif
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountStorageBits(UInt64 v) {
            if (0 == v) {
                return 0;
            }
            return 1 + Log2Floor(v.ToUnsignedUnchecked());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountStorageBits(UInt32 v) {
            if (0 == v) {
                return 0;
            }
            return 1 + Log2Floor(v.ToUnsignedUnchecked());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountStorageBits(Int64 v) {
            if (0 > v) {
                v = ~v;
            }
            if (0 == v) {
                return 1;
            }
            return 2 + Log2Floor(v.ToUnsignedUnchecked());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountStorageBits(Int32 v) {
            if (0 > v) {
                v = ~v;
            }
            if (0 == v) {
                return 1;
            }
            return 2 + Log2Floor(v.ToUnsignedUnchecked());
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long CountTrailingZeros(byte[] source) {
            if (source == null) {
                return -1;
            }
            long r = 0;
            int i;
            for (i = 0; source.Length > i; ++i) {
                if (source[i] == 0) {
                    r = unchecked(r + 8);
                } else {
                    i = source[i];
                    i &= unchecked(-(sbyte)(byte)i);
                    goto L_1;
                }
            }
            return -1;
        L_1:;
            if (0 == (i & 0x0F)) {
                r = unchecked(r + 4);
            }
            if (0 == (i & 0x33)) {
                r = unchecked(r + 2);
            }
            if (0 == (i & 0x55)) {
                r = unchecked(r + 1);
            }
            return r;
        }

        [TargetedPatchingOptOutAttribute("")]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long CountTrailingZeros(ReadOnlySpan<UInt64> source) {
            long r = 0;
            for (var i = source.Length - 1; 0 <= i; --i) {
                var t = source[i];
                if (0 == t) {
                    r = unchecked(r + 64);
                } else {
                    return unchecked(r + CountTrailingZeros(t));
                }
            }
            return r;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountTrailingZeros(UInt32 x) {
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.Result<int>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<int>() <= 64);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(x);
#else
            if (x == 0) {
                return 32;
            }
            var n = 0;
            if ((x & 0x0000FFFF) == 0) {
                x >>= 16;
                n = unchecked(n + 16);
            }
            if ((x & 0x000000FF) == 0) {
                x >>= 8;
                n = unchecked(n + 8);
            }
            if ((x & 0x0000000F) == 0) {
                x >>= 4;
                n = unchecked(n + 4);
            }
            if ((x & 0x00000003) == 0) {
                x >>= 2;
                n = unchecked(n + 2);
            }
            if ((x & 0x00000001) == 0) {
                n = unchecked(n + 1);
            }
            return n;
#endif
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountTrailingZeros(UInt64 x) {
            System.Diagnostics.Contracts.Contract.Ensures(0 <= System.Diagnostics.Contracts.Contract.Result<int>());
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<int>() <= 64);
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.TrailingZeroCount(x);
#else
            if (x == 0) {
                return 64;
            }
            var n = 0;
            if ((x & 0xFFFFFFFF) == 0) {
                x >>= 32;
                n = unchecked(n + 32);
            }
            if ((x & 0x0000FFFF) == 0) {
                x >>= 16;
                n = unchecked(n + 16);
            }
            if ((x & 0x000000FF) == 0) {
                x >>= 8;
                n = unchecked(n + 8);
            }
            if ((x & 0x0000000F) == 0) {
                x >>= 4;
                n = unchecked(n + 4);
            }
            if ((x & 0x00000003) == 0) {
                x >>= 2;
                n = unchecked(n + 2);
            }
            if ((x & 0x00000001) == 0) {
                n = unchecked(n + 1);
            }
            return n;
#endif
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountTrailingZeros(UInt128 x) {
            return DoubleArithmetic.CountTrailingZeros(x.GetLowPart(), x.GetHighPart());
        }

#if NET7_0_OR_GREATER
        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountTrailingZeros(System.UInt128 x) {
            return DoubleArithmetic.CountTrailingZeros(x.GetLowPart(), x.GetHighPart());
        }
#endif

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static byte ExtendSigned(int bits, byte value) {
            bits &= (1 << unchecked((int)(sizeof(byte) * 8))) - 1;
            return unchecked((byte)ExtendSigned(bits, value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static short ExtendSigned(int bits, short value) {
            bits &= (1 << unchecked((int)(sizeof(short) * 8))) - 1;
            return unchecked((short)ExtendSigned(bits, value));
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ExtendSigned(int bits, int value) {
            var m = unchecked((int)(sizeof(int) * 8) - bits);
            return (value << m) >> m;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long ExtendSigned(int bits, long value) {
            var m = unchecked((int)(sizeof(long) * 8) - bits);
            return (value << m) >> m;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int ExtendUnsigned(int bits, int value) {
            var m = unchecked((int)(sizeof(int) * 8) - bits);
            return unchecked((int)(((uint)(value << m)) >> m));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long ExtendUnsigned(int bits, long value) {
            var m = unchecked((int)(sizeof(long) * 8) - bits);
            return unchecked((long)(((ulong)(value << m)) >> m));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes(UInt64 v) {
            return 0 != unchecked((v - 0x0101010101010101ul) & ~v & 0x8080808080808080ul);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes(UInt32 v) {
            return 0 != unchecked((v - 0x01010101u) & ~v & 0x80808080ul);
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes(Int64 v) {
            return HasZeroBytes(unchecked((UInt64)v));
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes(Int32 v) {
            return 0 != unchecked((v - 0x01010101) & ~v & 0x80808080);
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes_A_Definition(UInt64 v) {
            for (int i = 0; i < unchecked((int)sizeof(UInt64)); ++i) {
                if ((v & 0xFF) == 0) {
                    return true;
                }
                v >>= 8;
            }
            return false;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static bool HasZeroBytes_A_Definition(UInt32 v) {
            for (int i = 0; i < unchecked((int)sizeof(UInt32)); ++i) {
                if ((v & 0xFF) == 0) {
                    return true;
                }
                v >>= 8;
            }
            return false;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt64 MostSignificantBit(UInt64 x) {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return unchecked((UInt64)Int64.MinValue) >> System.Numerics.BitOperations.LeadingZeroCount(x);
#else
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            x |= (x >> 32);
            return (x & ~(x >> 1));
#endif
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt32 MostSignificantBit(UInt32 x) {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return unchecked((UInt32)UInt32.MinValue) >> System.Numerics.BitOperations.LeadingZeroCount(x);
#else
            x |= (x >> 1);
            x |= (x >> 2);
            x |= (x >> 4);
            x |= (x >> 8);
            x |= (x >> 16);
            return (x & ~(x >> 1));
#endif
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt64 MostSignificantBit_A_Definition(UInt64 x) {
            var a = unchecked((UInt64)Int64.MinValue);
            for (int i = 0; i < unchecked((int)(sizeof(UInt64) * 8)); ++i) {
                if ((x & a) != 0) {
                    return a;
                }
                a >>= 1;
            }
            return a;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static UInt32 MostSignificantBit_A_Definition(UInt32 x) {
            var a = unchecked((UInt32)Int32.MinValue);
            for (int i = 0; i < unchecked((int)(sizeof(UInt32) * 8)); ++i) {
                if ((x & a) != 0) {
                    return a;
                }
                a >>= 1;
            }
            return a;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long NextPermutation(long v) {
            return unchecked((long)NextPermutation((ulong)v));
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int NextPermutation(int v) {
            return unchecked((int)NextPermutation((uint)v));
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        // ~30.9 Cyc
        public static ulong NextPermutation(ulong v) {
            unchecked {
                if (v == 0) {
                    return ulong.MaxValue;
                }
                ulong t = (v | (v - 1)) + 1;
                return (ulong)(t | ((((t & (ulong)(-(long)t)) / (v & (ulong)(-(long)v))) >> 1) - 1));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "v-1")]
        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint NextPermutation(uint v) {
            unchecked {
                if (v == 0) {
                    return uint.MaxValue;
                }
                uint t = (v | (v - 1)) + 1;
                return (uint)(t | ((((t & -t) / (v & -v)) >> 1) - 1));
            }
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int PopulationCount(UInt64 x) {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.PopCount(x);
#else
            unchecked {
                x -= (x >> 1) & 0x5555555555555555;
                x = (x & 0x3333333333333333) + ((x >> 2) & 0x3333333333333333);
                x = (x + (x >> 4)) & 0x0f0f0f0f0f0f0f0f;
                return (int)((x * 0x0101010101010101) >> 56);
            }
#endif
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt64 GetHighPart(this UInt128 d) {
            return unchecked((UInt64)d.HiInt64Bits);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt64 GetLowPart(this UInt128 d) {
            return unchecked((UInt64)d.LoInt64Bits);
        }

#if NET7_0_OR_GREATER
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt64 GetHighPart(this System.UInt128 d) {
            return unchecked((UInt64)(d >>> 64));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt64 GetLowPart(this System.UInt128 d) {
            return unchecked((UInt64)d);
        }

#if !INDEPENDENT_XINTN_LIBRARY || FEATURE_XINTN_INT256
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt128 GetHighPart(this UInt256 d) {
            return unchecked((UInt128)d.HiInt128Bits);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        static UInt128 GetLowPart(this UInt256 d) {
            return unchecked((UInt128)d.LoInt128Bits);
        }
#endif

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int PopulationCount(System.UInt128 x) {
            return System.Numerics.BitOperations.PopCount(x.GetHighPart()) + System.Numerics.BitOperations.PopCount(x.GetLowPart());
        }
#endif

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        // 8.38 Cyc
        public static int PopulationCount(UInt32 x) {
#if NETCOREAPP3_0 || NETCOREAPP3_1 || (NET5_0 || NET6_0 || NET5_0_OR_GREATER) || NETCOREAPP3_0_OR_GREATER || NET5_0_OR_GREATER
            return System.Numerics.BitOperations.PopCount(x);
#else
            unchecked {
                x -= (x >> 1) & 0x55555555;
                x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
                x = (x + (x >> 4)) & 0x0f0f0f0f;
                return (int)((x * 0x01010101) >> 24);
            }
#endif
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int PopulationCount_A_Definition(UInt32 x) {
            unchecked {
                var r = 0;
                for (int i = 0; i < unchecked((int)(sizeof(UInt32) * 8)); ++i) {
                    if ((x & 1) == 1) {
                        ++r;
                    }
                    x >>= 1;
                }
                return r;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int PopulationCount_A_Definition(UInt64 x) {
            unchecked {
                var r = 0;
                for (int i = 0; i < unchecked((int)(sizeof(UInt64) * 8)); ++i) {
                    if ((x & 1) == 1) {
                        ++r;
                    }
                    x >>= 1;
                }
                return r;
            }
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static byte ReverseBits(byte v) {
            return unchecked((byte)((v * 0X0202020202LU & 0X010884422010LU) % 1023));
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint ReverseBits(uint v) {
            var r = (uint)0;
            for (int i = 0; i < unchecked((int)(sizeof(uint))); ++i) {
                r = unchecked(((r << 8) | ReverseBits((byte)v)));
                v >>= 8;
            }
            return r;
        }

        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static byte ReverseBits_A_Definition(byte v) {
            byte r = 0;
            for (int i = 0; i < unchecked((int)(sizeof(byte) * 8)); ++i) {
                r = unchecked((byte)((r << 1) | (v & (byte)1)));
                v >>= 1;
            }
            return r;
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint ReverseBits_A_Definition(uint v) {
            uint r = 0;
            for (int i = 0; i < unchecked((int)(sizeof(uint) * 8)); ++i) {
                r = unchecked((uint)((r << 1) | (v & (uint)1)));
                v >>= 1;
            }
            return r;
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountSignificantBits(ulong value) {
            if (0 == value) {
                return 0;
            }
            return 64 - CountLeadingZeros(value) - CountTrailingZeros(value);
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int CountSignificantBits(long value) {
            return CountSignificantBits(Elementary.Math.AbsAsUnsigned(value));
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int Log2Floor(ulong value) {
            return System.Numerics.BitOperations.Log2(value);
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int Log2Floor(UltimateOrb.UInt128 value) {
            return DoubleArithmetic.Log2Floor(value.GetLowPart(), value.GetHighPart());
        }

        [System.CLSCompliantAttribute(false)]
        [TargetedPatchingOptOutAttribute("")]
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int Log2Floor(System.UInt128 value) {
            return DoubleArithmetic.Log2Floor(value.GetLowPart(), value.GetHighPart());
        }
    }
}
