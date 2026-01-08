using System;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace UltimateOrb.Numerics {

    [StructLayout(LayoutKind.Sequential)]
    readonly struct Win32Decimal {
        public readonly int _flags;
        public readonly uint _hi32;
        public readonly ulong _lo64;
    }
}
