using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb {

    public readonly ref struct BigSpan<T> {

        readonly ByReference<T> _reference;

        readonly nint _length;
    }

    public readonly ref struct ReadOnlyBigSpan<T> {

        readonly ReadOnlyByReference<T> _reference;

        readonly nint _length;
    }
}
