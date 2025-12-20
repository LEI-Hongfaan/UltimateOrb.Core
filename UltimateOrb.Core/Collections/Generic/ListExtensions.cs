using System;
using System.Collections.Generic;

namespace UltimateOrb.Collections.Generic {

    public static class ListExtensions {

        public static ReadOnlyListView<T, TResult> AsReadOnly<T, TResult>(this IList<T> list, Func<T, TResult> selector) {
            return new ReadOnlyListView<T, TResult>(list, selector);
        }
    }
}
