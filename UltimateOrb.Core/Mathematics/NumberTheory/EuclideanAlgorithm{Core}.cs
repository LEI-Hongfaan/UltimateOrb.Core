using System;

namespace UltimateOrb.Mathematics.NumberTheory {
    using Utilities = EuclideanAlgorithm;
    using Math = global::Internal.System.Math;

    public static partial class EuclideanAlgorithm {

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static uint GreatestCommonDivisorPartialStub0001(uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires((second & 1u) == 1u && second > 1u);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<uint>() != 0);
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<uint>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<uint>());
            unchecked {
                if (0u == first) {
                    return second;
                }
                while (0u == (first & 1u)) {
                    first >>= 1;
                }
                if (first == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first > second) {
                    goto L_Gt;
                }
            L_Lt:
                ;
                if (0u != ((first ^ second) & 2u)) {
                    second = (first >> 2) + (second >> 2) + 1u;
                } else {
                    second = (second - first) >> 2;
                }
                while (0u == (second & 1u)) {
                    second >>= 1;
                }
                if (second == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first < second) {
                    goto L_Lt;
                }
            L_Gt:
                ;
                if (0u != ((first ^ second) & 2u)) {
                    first = (first >> 2) + (second >> 2) + 1u;
                } else {
                    first = (first - second) >> 2;
                }
                while (0u == (first & 1u)) {
                    first >>= 1;
                }
                if (first == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first > second) {
                    goto L_Gt;
                }
                goto L_Lt;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static ulong GreatestCommonDivisorPartialStub0001(ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(((uint)second & 1u) == 1u && second > 1u);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<ulong>() != 0);
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            unchecked {
                if (0u == first) {
                    return second;
                }
                while (0u == ((uint)first & 1u)) {
                    first >>= 1;
                }
                if (first == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first > second) {
                    goto L_Gt;
                }
            L_Lt:
                ;
                if (0u != (((uint)first ^ (uint)second) & 2u)) {
                    second = (first >> 2) + (second >> 2) + 1u;
                } else {
                    second = (second - first) >> 2;
                }
                while (0u == ((uint)second & 1u)) {
                    second >>= 1;
                }
                if (second == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first < second) {
                    goto L_Lt;
                }
            L_Gt:
                ;
                if (0u != (((uint)first ^ (uint)second) & 2u)) {
                    first = (first >> 2) + (second >> 2) + 1u;
                } else {
                    first = (first - second) >> 2;
                }
                while (0u == ((uint)first & 1u)) {
                    first >>= 1;
                }
                if (first == 1u) {
                    return 1u;
                }
                if (first == second) {
                    return second;
                } else if (first > second) {
                    goto L_Gt;
                }
                goto L_Lt;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static uint GreatestCommonDivisorPartialStub0002(uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires((first & 1u) == 1u || (second & 1u) == 1u);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<uint>() != 0);
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<uint>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<uint>());
            unchecked {
                uint c;
                if (0u != (second & 1u)) {
                    if (first == 1u || second == 1u) {
                        c = 1u;
                    } else {
                        c = GreatestCommonDivisorPartialStub0001(first, second);
                    }
                } else {
                    if (first == 1u) {
                        c = 1u;
                    } else {
                        c = GreatestCommonDivisorPartialStub0001(second, first);
                    }
                }
                return c;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static ulong GreatestCommonDivisorPartialStub0002(ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(((uint)first & 1u) == 1u || ((uint)second & 1u) == 1u);
            System.Diagnostics.Contracts.Contract.Ensures(System.Diagnostics.Contracts.Contract.Result<ulong>() != 0);
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            unchecked {
                ulong c;
                if (0u != ((uint)second & 1u)) {
                    if (first == 1u || second == 1u) {
                        c = 1u;
                    } else {
                        c = GreatestCommonDivisorPartialStub0001(first, second);
                    }
                } else {
                    if (first == 1u) {
                        c = 1u;
                    } else {
                        c = GreatestCommonDivisorPartialStub0001(second, first);
                    }
                }
                return c;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static uint Abs(int value) {
            unchecked {
                if (0 > value) {
                    return (uint)(-value);
                }
                return (uint)value;
                // return 0 <= value ? (ulong)value : (ulong)(-value);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static ulong Abs(long value) {
            unchecked {
                if (0 > value) {
                    return (ulong)(-value);
                }
                return (ulong)value;
                // return 0 <= value ? (ulong)value : (ulong)(-value);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static int CountTrailingZeros(UInt32 value) {
            System.Diagnostics.Contracts.Contract.Requires(0 != value);
            var c = 0;
            for (var i = value; 0 == i % 2; i >>= 1) {
                unchecked {
                    ++c;
                }
            }
            return c;
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        internal static int CountTrailingZeros(UInt64 value) {
            System.Diagnostics.Contracts.Contract.Requires(0 != value);
            var c = 0;
            for (var i = value; 0 == i % 2; i >>= 1) {
                unchecked {
                    ++c;
                }
            }
            return c;
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint GreatestCommonDivisor(uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Ensures((System.Diagnostics.Contracts.Contract.Result<uint>() == 0u) == (System.Diagnostics.Contracts.Contract.OldValue(first) == 0u && System.Diagnostics.Contracts.Contract.OldValue(second) == 0u));
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<uint>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<uint>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<uint>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<uint>());
            unchecked {
                if (0u == second) {
                    return first;
                }
                if (0u == first) {
                    return second;
                }
                if (first > second) {
                    first %= second;
                    if (0u == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0u == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros(first | second);
                return GreatestCommonDivisorPartialStub0002(first >> v, second >> v) << v;
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisor(ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Ensures((System.Diagnostics.Contracts.Contract.Result<ulong>() == 0u) == (System.Diagnostics.Contracts.Contract.OldValue(first) == 0u && System.Diagnostics.Contracts.Contract.OldValue(second) == 0u));
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<ulong>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<ulong>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<ulong>());
            unchecked {
                if (0u == second) {
                    return first;
                }
                if (0u == first) {
                    return second;
                }
                if (first > second) {
                    first %= second;
                    if (0u == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0u == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros(first | second);
                return GreatestCommonDivisorPartialStub0002(first >> v, second >> v) << v;
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static int GreatestCommonDivisor(int first, int second) {
            System.Diagnostics.Contracts.Contract.Ensures((System.Diagnostics.Contracts.Contract.Result<long>() == 0u) == (System.Diagnostics.Contracts.Contract.OldValue(first) == 0u && System.Diagnostics.Contracts.Contract.OldValue(second) == 0u));
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<long>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<long>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<long>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<long>());
            unchecked {
                first = (int)Abs(first);
                if (0 == second) {
                    return first;
                }
                second = (int)Abs(second);
                if (0 == first) {
                    return second;
                }
                if (first > second) {
                    first %= second;
                    if (0 == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0 == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros((uint)(first | second));
                return (int)(GreatestCommonDivisorPartialStub0002((uint)(first >> v), (uint)(second >> v)) << v);
            }
        }

        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static long GreatestCommonDivisor(long first, long second) {
            System.Diagnostics.Contracts.Contract.Ensures((System.Diagnostics.Contracts.Contract.Result<long>() == 0u) == (System.Diagnostics.Contracts.Contract.OldValue(first) == 0u && System.Diagnostics.Contracts.Contract.OldValue(second) == 0u));
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<long>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(first) % System.Diagnostics.Contracts.Contract.Result<long>());
            System.Diagnostics.Contracts.Contract.Ensures(0u == System.Diagnostics.Contracts.Contract.Result<long>() || 0u == System.Diagnostics.Contracts.Contract.OldValue(second) % System.Diagnostics.Contracts.Contract.Result<long>());
            unchecked {
                first = (long)Abs(first);
                if (0 == second) {
                    return first;
                }
                second = (long)Abs(second);
                if (0 == first) {
                    return second;
                }
                if (first > second) {
                    first %= second;
                    if (0 == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0 == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros((ulong)(first | second));
                return (long)(GreatestCommonDivisorPartialStub0002((ulong)(first >> v), (ulong)(second >> v)) << v);
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint GreatestCommonDivisorPartial(uint first, uint second) {
            System.Diagnostics.Contracts.Contract.Requires(first > 0);
            System.Diagnostics.Contracts.Contract.Requires(second > 0);
            unchecked {
                if (first > second) {
                    first %= second;
                    if (0u == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0u == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros(first | second);
                return GreatestCommonDivisorPartialStub0002(first >> v, second >> v) << v;
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisorPartial(ulong first, ulong second) {
            System.Diagnostics.Contracts.Contract.Requires(first > 0);
            System.Diagnostics.Contracts.Contract.Requires(second > 0);
            unchecked {
                if (first > second) {
                    first %= second;
                    if (0u == first) {
                        return second;
                    }
                } else {
                    second %= first;
                    if (0u == second) {
                        return first;
                    }
                }
                var v = Utilities.CountTrailingZeros(first | second);
                return GreatestCommonDivisorPartialStub0002(first >> v, second >> v) << v;
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static uint GreatestCommonDivisorPartial(uint first, uint second, out int firstCoefficient, out int secondCoefficient) {
            System.Diagnostics.Contracts.Contract.Requires(first > 0);
            System.Diagnostics.Contracts.Contract.Requires(second > 0);
            unchecked {
                int u0 = 1;
                int v0 = 0;
                int u1 = 0;
                int v1 = 1;
                if (second > first) {
                    goto L_02;
                }
            L_01:
                uint q;
                q = Math.DivRem(first, second, out first);
                if (0u == first) {
                    firstCoefficient = u1;
                    secondCoefficient = v1;
                    return second;
                }
                u0 -= (int)q * u1;
                v0 -= (int)q * v1;
            L_02:
                q = Math.DivRem(second, first, out second);
                if (0u == second) {
                    firstCoefficient = u0;
                    secondCoefficient = v0;
                    return first;
                }
                u1 -= (int)q * u0;
                v1 -= (int)q * v0;
                goto L_01;
            }
        }


        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisorPartial(ulong first, ulong second, out long firstCoefficient, out long secondCoefficient) {
            System.Diagnostics.Contracts.Contract.Requires(first > 0);
            System.Diagnostics.Contracts.Contract.Requires(second > 0);
            unchecked {
                long u0 = 1;
                long v0 = 0;
                long u1 = 0;
                long v1 = 1;
                if (second > first) {
                    goto L_02;
                }
            L_01:
                ulong q;
                q = Math.DivRem(first, second, out first);
                if (0u == first) {
                    firstCoefficient = u1;
                    secondCoefficient = v1;
                    return second;
                }
                u0 -= (long)q * u1;
                v0 -= (long)q * v1;
            L_02:
                q = Math.DivRem(second, first, out second);
                if (0u == second) {
                    firstCoefficient = u0;
                    secondCoefficient = v0;
                    return first;
                }
                u1 -= (long)q * u0;
                v1 -= (long)q * v0;
                goto L_01;
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisorPartial(ulong module, ulong value, out long pseudoinverse) {
            System.Diagnostics.Contracts.Contract.Requires(module > 0);
            System.Diagnostics.Contracts.Contract.Requires(value > 0);
            unchecked {
                long v0 = 0;
                long v1 = 1;
                if (value > module) {
                    goto L_02;
                }
            L_01:
                ulong q;
                q = Math.DivRem(module, value, out module);
                if (0u == module) {
                    pseudoinverse = v1;
                    return value;
                }
                v0 -= (long)q * v1;
            L_02:
                q = Math.DivRem(value, module, out value);
                if (0u == value) {
                    pseudoinverse = v0;
                    return module;
                }
                v1 -= (long)q * v0;
                goto L_01;
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisorPartial(ulong module, ulong value, out ulong pseudoinverse) {
            System.Diagnostics.Contracts.Contract.Requires(module > 0);
            System.Diagnostics.Contracts.Contract.Requires(module > value);
            System.Diagnostics.Contracts.Contract.Requires(value > 0);
            unchecked {
                long v0 = 0;
                long v1 = 1;
                for (var n = module; ;) {
                    ulong q;
                    q = Math.DivRem(n, value, out n);
                    if (0u == n) {
                        pseudoinverse = ((0 > v1) ? (module + (ulong)v1) : (ulong)v1);
                        return value;
                    }
                    v0 -= (long)q * v1;
                    q = Math.DivRem(value, n, out value);
                    if (0u == value) {
                        pseudoinverse = ((0 > v0) ? (module + (ulong)v0) : (ulong)v0);
                        return n;
                    }
                    v1 -= (long)q * v0;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
        [System.Runtime.TargetedPatchingOptOutAttribute("")]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.Contracts.PureAttribute()]
        public static ulong GreatestCommonDivisor(ulong module, ulong value, out ulong pseudoinverse) {
            System.Diagnostics.Contracts.Contract.Requires(module > 0u);
            System.Diagnostics.Contracts.Contract.EnsuresOnThrow<DivideByZeroException>(0u == System.Diagnostics.Contracts.Contract.OldValue(module));
            unchecked {
                long v0 = 0;
                long v1 = 1;
                var n = module;
                if (value > module) {
                    goto L_02;
                }
                if (0u == value && 0u != module) {
                    goto L_03;
                }
            L_01:
                ulong q;
                q = Math.DivRem(n, value, out n);
                if (0u == n) {
                    if (0 == v1) {
                        goto L_03;
                    }
                    pseudoinverse = ((0 > v1) ? (module + (ulong)v1) : (ulong)v1);
                    return value;
                }
                v0 -= (long)q * v1;
            L_02:
                q = Math.DivRem(value, n, out value);
                if (0u == value) {
                    if (0 == v0) {
                        goto L_03;
                    }
                    pseudoinverse = ((0 > v0) ? (module + (ulong)v0) : (ulong)v0);
                    return n;
                }
                v1 -= (long)q * v0;
                goto L_01;
            L_03:
                pseudoinverse = 0u;
                return 0u;
            }
        }
    }
}
