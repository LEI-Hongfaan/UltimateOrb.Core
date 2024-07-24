using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    public static partial class RandomExtensions {

        public static T NextFromBits<T>(this Random random) where T : unmanaged {
            Span<byte> bytes = stackalloc byte[Unsafe.SizeOf<T>()];
            random.NextBytes(bytes);
            return MemoryMarshal.AsRef<T>(bytes);
        }
    }
}
