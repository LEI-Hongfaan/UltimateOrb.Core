using System.Threading;

namespace UltimateOrb.Utilities {

    public class GCCollectionApproximateCounter : GCCollectionRoughListener<int> {

        public int GCCollectionCount {

            get => Volatile.Read(ref Data);
        }

        public GCCollectionApproximateCounter() {
            Action = (ref int count) => {
                Interlocked.Increment(ref count);
            };
        }
    }
}
