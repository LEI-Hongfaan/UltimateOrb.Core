using System.Collections.Generic;

namespace UltimateOrb.Runtime.CompilerServices {

    public struct BoxedComparer<T, TComparer> : IComparer<T>, IComparer<Boxed<T>>
        where T : struct
        where TComparer : IComparer<T> {

        TComparer comparer;

        public BoxedComparer(TComparer comparer) {
            this.comparer = comparer;
        }

        public int Compare(T x, T y) => comparer.Compare(x, y);

        public int Compare(Boxed<T>? x, Boxed<T>? y) {
            if (x != null) {
                if (y != null) {
                    return comparer.Compare(x.Value, y.Value);
                }
                return 1;
            }
            if (y != null) {
                return -1;
            }
            return 0;
        }
    }
}
