using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Collections.Generic {

    public static partial class EnumerableExtensions {

        public static IEnumerable<ReadOnlyMemory<T>> EnumerateSequence<T>(this IEnumerable<T> elementSource, int minLength, int maxLengthExclusive) {
            return EnumerateSequence(elementSource.ToArray(), minLength, maxLengthExclusive);
        }

        public static IEnumerable<ReadOnlyMemory<T>> EnumerateSequence<T>(this T[] elementSource, int minLength, int maxLengthExclusive) {
            for (var i = minLength; maxLengthExclusive > i; ++i) {
                foreach (var item in EnumerateSequence(elementSource, i)) {
                    yield return item;
                }
            }
        }

        public static IEnumerable<ReadOnlyMemory<T>> EnumerateSequence<T>(this IEnumerable<T> elementSource, int length) {
            return EnumerateSequence(elementSource.ToArray(), length);
        }

        public static IEnumerable<ReadOnlyMemory<T>> EnumerateSequence<T>(this T[] elementSource, int length) {
            var a = new T[length];
            var r = new ReadOnlyMemory<T>(a);
            foreach (var _ in EnumerateSequenceYieldsCurrentCount(elementSource, a)) {
                yield return r;
            }
        }

        internal static IEnumerable<long> EnumerateSequenceYieldsCurrentCount<T>(this T[] elementSource, T[] buffer) {
            var c = 0L;
            var b = elementSource.Length;
            if (b != 0) {
                var stack = new int[buffer.Length];
                for (var i = 0; i < buffer.Length; ++i) {
                    buffer[i] = elementSource[0];
                }
                {
                L:;
                    yield return c++;
                    for (var i = 0; stack.Length != i; ++i) {
                        var en = ++stack[i];
                        if (b == en) {
                        } else {
                            buffer[i] = elementSource[en];
                            for (; 0 <= --i;) {
                                buffer[i] = elementSource[0];
                                stack[i] = 0;
                            }
                            goto L;
                        }
                    }
                }
            }
        }

        public static IEnumerable<ReadOnlyMemory<int>> EnumerateSequence(int maxElementExclusive, int length) {
            var a = new int[length];
            var r = new ReadOnlyMemory<int>(a);
            foreach (var _ in EnumerateSequenceYieldsCurrentCount(maxElementExclusive, a)) {
                yield return r;
            }
        }

        internal static IEnumerable<long> EnumerateSequenceYieldsCurrentCount(int maxElementExclusive, int[] buffer) {
            var c = 0L;
            var b = maxElementExclusive;
            if (b != 0) {
                for (var i = 0; i < buffer.Length; ++i) {
                    buffer[i] = 0;
                }
                {
                L:;
                    yield return c++;
                    for (var i = 0; buffer.Length != i; ++i) {
                        var en = ++buffer[i];
                        if (b == en) {
                        } else {
                            buffer[i] = en;
                            for (; 0 <= --i;) {
                                buffer[i] = 0;
                            }
                            goto L;
                        }
                    }
                }
            }
        }
    }
}
