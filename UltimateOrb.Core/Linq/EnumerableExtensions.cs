using System;
using System.Collections.Generic;
using System.Linq;

namespace UltimateOrb.Linq {

    /// <inheritdoc cref="Enumerable"/>
    public static partial class EnumerableExtensions {

        /// <inheritdoc cref="Enumerable.Skip"/>
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, long count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }
            if (count <= int.MaxValue) {
                return Enumerable.Skip(source, unchecked((int)count));
            }
            return source.Skip_A_Stub0001(count); 
        }

        /// <inheritdoc cref="Enumerable.Skip"/>
        public static IEnumerable<T> Skip_A<T>(this IEnumerable<T> source, long count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }
            var n = false ? checked((int)(1u << 30)) : int.MaxValue;
            var c = count;
            if (c <= int.MaxValue) {
                return Enumerable.Skip(source, unchecked((int)c));
            }
            var s = source;
            do {
                s = Enumerable.Skip(s, n);
                c -= n;
            } while (c > n);
            return Enumerable.Skip(s, n);
        }

        static IEnumerable<T> Skip_A_Stub0001<T>(this IEnumerable<T> source, long count) {
            using var en = source.GetEnumerator();
            for (var c = count; c > 0; --c) {
                if (!en.MoveNext()) {
                    yield break;
                }
            }
            for (; en.MoveNext();) {
                yield return en.Current;
            }
        }
    }
}
