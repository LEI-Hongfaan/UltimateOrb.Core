using System;

namespace UltimateOrb {

    public interface IReadOnlySpanProvider<T> {

        ReadOnlySpan<T> Span {

            get;
        }
    }
}
