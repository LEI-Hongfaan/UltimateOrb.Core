using System;

#pragma warning disable IDE0004 // Remove Unnecessary Cast
#pragma warning disable IDE0005 // Using directive is unnecessary.
#pragma warning disable IDE0065 // Misplaced using directive

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
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

#if NET7_0_OR_GREATER
namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.Int128;

    using Math = global::Internal.System.Math;

    using IntT = System.Int128;
    using UIntT = System.UInt128;

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
    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.Int128;

    using Math = global::Internal.System.Math;

    using IntT = System.Int128;
    using UIntT = System.UInt128;
    
    using LIntT = System.UInt128;
    using HIntT = System.UInt128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.Int128;

    using Math = global::Internal.System.Math;

    using IntT = System.Int128;
    using UIntT = System.UInt128;
    
    using LIntT = System.UInt128;
    using HIntT = System.Int128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.Int128;

    using Math = global::Internal.System.Math;

    using IntT = System.Int128;
    using UIntT = System.UInt128;
    
    using LIntT = System.Int128;
    using HIntT = System.UInt128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = System.UInt128;
    using Int = Int64;
    using Long = System.Int128;

    using Math = global::Internal.System.Math;

    using IntT = System.Int128;
    using UIntT = System.UInt128;
    
    using LIntT = System.Int128;
    using HIntT = System.Int128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}
#endif

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    using IntT = UltimateOrb.Int128;
    using UIntT = UltimateOrb.UInt128;

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
    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    using IntT = UltimateOrb.Int128;
    using UIntT = UltimateOrb.UInt128;
    
    using LIntT = UltimateOrb.UInt128;
    using HIntT = UltimateOrb.UInt128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    using IntT = UltimateOrb.Int128;
    using UIntT = UltimateOrb.UInt128;
    
    using LIntT = UltimateOrb.UInt128;
    using HIntT = UltimateOrb.Int128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    using IntT = UltimateOrb.Int128;
    using UIntT = UltimateOrb.UInt128;
    
    using LIntT = UltimateOrb.Int128;
    using HIntT = UltimateOrb.UInt128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(false)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}

namespace UltimateOrb.Numerics {
    using UInt = UInt64;
    using ULong = UltimateOrb.UInt128;
    using Int = Int64;
    using Long = UltimateOrb.Int128;

    using Math = global::Internal.System.Math;

    using IntT = UltimateOrb.Int128;
    using UIntT = UltimateOrb.UInt128;
    
    using LIntT = UltimateOrb.Int128;
    using HIntT = UltimateOrb.Int128;

    public static partial class DoubleArithmetic {

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static HIntT ShiftLeft(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */))));
                }
                return high;
            }
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRight(LIntT low, HIntT high, int count) {
            unsafe {
                if (0 != count) {
                    return unchecked((LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */))));
                }
                return low;
            }
        }

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, int count, out HIntT highResult) {
            unsafe {
                unchecked {
                    count &= 2 * (8 * sizeof(IntT)) - 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162

#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> (-count/* 8 * sizeof(IntT) - count */)));
                            return (LIntT)(low << count);
                        } 
                    } else {
                        // if (count > 8 * sizeof(IntT)) {
                        //     highResult = (HIntT)(low << (count/* - 8 * sizeof(IntT)*/));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightSigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((IntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        var mask = (UIntT)((IntT)high >> (8 * sizeof(IntT) - 1));
                        highResult = (HIntT)mask;
                        if (count > 8 * sizeof(IntT)) {
                            // if (0 > (IntT)high) {
                            //     highResult = (HIntT)(IntT)(-1);
                            //     return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (UIntT.MaxValue << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
                            // } else {
                            //     highResult = (HIntT)0;
                            //     return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
                            // }
                            return (LIntT)(((UIntT)high >> (count/* - 8 * sizeof(IntT)*/)) | (mask << (-count/* 8 * sizeof(IntT) + 8 * sizeof(IntT) - count */)));
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT ShiftRightUnsigned(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)((UIntT)high >> count);
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << (-count/* 8 * sizeof(IntT) - count */)));
                        }
                    } else {
                        highResult = (HIntT)0;
                        // if (count > 8 * sizeof(IntT)) {
                        //    return (LIntT)((UIntT)high >> (count/* - 8 * sizeof(IntT)*/));
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
        }
        
#pragma warning disable 162
        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateLeft(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high << count) | ((UIntT)low >> -count));
                            return (LIntT)(((UIntT)low << count) | ((UIntT)high >> -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }

        [System.CLSCompliantAttribute(true)]
        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static LIntT RotateRight(LIntT low, HIntT high, out HIntT highResult) {
            unsafe {
                unchecked {
                    const int count = 1;
                    if (count < 8 * sizeof(IntT)) {
                        if (0 != count) {
                            highResult = (HIntT)(((UIntT)high >> count) | ((UIntT)low << -count));
                            return (LIntT)(((UIntT)low >> count) | ((UIntT)high << -count));
                        }
                    } else {
                        if (count > 8 * sizeof(IntT)) {
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
        }
#pragma warning restore 162
    }
}
#pragma warning restore IDE0065 // Misplaced using directive
#pragma warning restore IDE0005 // Using directive is unnecessary.
#pragma warning restore IDE0004 // Remove Unnecessary Cast
