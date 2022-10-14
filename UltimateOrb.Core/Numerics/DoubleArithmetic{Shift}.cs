using System;

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;

    using IntT = Int64;
    using UIntT = UInt64;

    public static partial class DoubleArithmetic {
        
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT ShiftRight(UIntT low, IntT high, int count, out IntT highResult) {
            return ShiftRightSigned(low, high, count, out highResult);
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT ShiftRight(UIntT low, UIntT high, int count, out UIntT highResult) {
            return ShiftRightUnsigned(low, high, count, out highResult);
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT ShiftRight(UIntT low, IntT high, out IntT highResult) {
            return ShiftRightSigned(low, high, out highResult);
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static UIntT ShiftRight(UIntT low, UIntT high, out UIntT highResult) {
            return ShiftRightUnsigned(low, high, out highResult);
        }
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;

    using IntT = Int64;
    using UIntT = UInt64;
    
    using LIntT = UInt64;
    using HIntT = UInt64;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */))));
            }
            return high;
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */))));
            }
            return low;
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;

    using IntT = Int64;
    using UIntT = UInt64;
    
    using LIntT = UInt64;
    using HIntT = Int64;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */))));
            }
            return high;
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */))));
            }
            return low;
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;

    using IntT = Int64;
    using UIntT = UInt64;
    
    using LIntT = Int64;
    using HIntT = UInt64;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */))));
            }
            return high;
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */))));
            }
            return low;
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt32;
    using ULong = UInt64;
    using Int = Int32;
    using Long = Int64;

    using Math = global::Internal.System.Math;

    using IntT = Int64;
    using UIntT = UInt64;
    
    using LIntT = Int64;
    using HIntT = Int64;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */))));
            }
            return high;
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            if (0 != count) {
                return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */))));
            }
            return low;
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unchecked {
                count &= 2 * sizeof(IntT) - 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* sizeof(IntT) - count */)));
                        return (LIntT)(low << count);
                    } 
                } else {
                    // if (count > sizeof(IntT)) {
                    //     highResult = (HIntT)(low << (count/* - sizeof(IntT)*/));
                    // } else {
                    //     highResult = (HIntT)low;
                    // }
                    highResult = (HIntT)(low << count);
                    return (LIntT)0;
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((IntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    var mask = (UIntT)((IntT)high >> (sizeof(IntT) - 1));
                    highResult = (HIntT)mask;
                    if (count > sizeof(IntT)) {
                        // if (0 > (IntT)high) {
                        //     highResult = (HIntT)(IntT)(-1);
                        //     return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                        // } else {
                        //     highResult = (HIntT)0;
                        //     return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                        // }
                        return (LIntT)(((UIntT)high >> (count/* - sizeof(IntT)*/)) | (mask << (-count/* sizeof(IntT) + sizeof(IntT) - count */)));
                    } else {
                        // highResult = (0 > (IntT)high) ? (HIntT)(IntT)(-1) : (HIntT)0;
                        return (LIntT)high;
                    }
                }
                {
                     highResult = high;
                     return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)((UIntT)high >> count);
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* sizeof(IntT) - count */)));
                    }
                } else {
                    highResult = (HIntT)0;
                    // if (count > sizeof(IntT)) {
                    //    return (LIntT)((UIntT)high >> (count/* - sizeof(IntT)*/));
                    // } else {
                    //     return (LIntT)high;
                    // }
                    return (LIntT)((UIntT)high >> count);
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                        return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        return (LIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unchecked {
                const int count = 1;
                if (count < sizeof(IntT)) {
                    if (0 != count) {
                        highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                        return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                    }
                } else {
                    if (count > sizeof(IntT)) {
                        highResult = (HIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        return (LIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                    } else {
                        highResult = (HIntT)low;
                        return (LIntT)high;
                    }
                }
                {
                    highResult = high;
                    return low;
                }
            }
        }
#pragma warning restore 162
    }
}
