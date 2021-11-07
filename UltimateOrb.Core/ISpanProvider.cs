using System;

namespace UltimateOrb {

    public interface ISpanProvider<T> {

        Span<T> Span {

            get;
        }
    }
}
