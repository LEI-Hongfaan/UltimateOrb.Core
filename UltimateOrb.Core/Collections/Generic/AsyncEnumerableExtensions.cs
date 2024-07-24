using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Collections.Generic {

    public static partial class AsyncEnumerableExtensions {

        public static async ValueTask<System.Collections.Generic.List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }
            var list = new System.Collections.Generic.List<T>();
            await foreach (var element in source) {
                list.Add(element);
            }
            return list;
        }
    }
}
