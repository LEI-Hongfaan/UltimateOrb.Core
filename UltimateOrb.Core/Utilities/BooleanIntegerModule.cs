using System;

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            CEq
            Ret
        ")]
        public static int IsMinusOne(Int32 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            CEq
            Ret
        ")]
        public static int IsMinusOne(UInt32 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            Conv.I8
            CEq
            Ret
        ")]
        public static int IsMinusOne(Int64 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            Conv.I8
            CEq
            Ret
        ")]
        public static int IsMinusOne(UInt64 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            Conv.I
            CEq
            Ret
        ")]
        public static int IsMinusOne(IntPtr value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.M1
            Conv.I
            CEq
            Ret
        ")]
        public static int IsMinusOne(UIntPtr value) {
            throw (NotImplementedException)null!;
        }
    }
}

namespace UltimateOrb.Utilities {

#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        static partial class BooleanIntegerModule {

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int IsZero(Int32 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            CEq
            Ret
        ")]
        public static int IsZero(UInt32 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            Conv.I8
            CEq
            Ret
        ")]
        public static int IsZero(Int64 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            Conv.I8
            CEq
            Ret
        ")]
        public static int IsZero(UInt64 value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            Conv.I
            CEq
            Ret
        ")]
        public static int IsZero(IntPtr value) {
            throw (NotImplementedException)null!;
        }

        [ILMethodBodyAttribute(@"
            LdArg.0
            LdC.I4.0
            Conv.I
            CEq
            Ret
        ")]
        public static int IsZero(UIntPtr value) {
            throw (NotImplementedException)null!;
        }
    }
}
