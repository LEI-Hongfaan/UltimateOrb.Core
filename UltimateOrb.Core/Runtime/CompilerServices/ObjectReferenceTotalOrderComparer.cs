using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SystemRuntimeHelpers = System.Runtime.CompilerServices.RuntimeHelpers;

namespace UltimateOrb.Runtime.CompilerServices {

    interface ISingletonPattern<TSelf>
        where TSelf : ISingletonPattern<TSelf> {

        public static abstract TSelf Instance { get; }
    }

    interface IReadOnlyValueSingletonPattern<TSelf>
        : ISingletonPattern<TSelf>
        where TSelf : struct, IReadOnlyValueSingletonPattern<TSelf> {

        static TSelf ISingletonPattern<TSelf>.Instance { get => TSelf.Instance; }

        public new static ref readonly TSelf Instance { get => ref TSelf.BoxedInstance.Unbox(); }

        public static abstract ReadOnlyBoxed<TSelf> BoxedInstance { get; }
    }

    public interface IObjectIdentityComparer : IEqualityComparer<object?>, IComparer<object?> {
    }

    public readonly struct ObjectIdentityComparer : IReadOnlyValueSingletonPattern<ObjectIdentityComparer>, IObjectIdentityComparer {

        static ReadOnlyBoxed<ObjectIdentityComparer> BoxedInstanceInternal { get; } = default(ObjectIdentityComparer).Box();

        public static IObjectIdentityComparer BoxedInstance { get => Unsafe.As<IObjectIdentityComparer>(BoxedInstanceInternal); }

        static ReadOnlyBoxed<ObjectIdentityComparer> IReadOnlyValueSingletonPattern<ObjectIdentityComparer>.BoxedInstance { get => BoxedInstanceInternal; }

        private readonly static ConditionalWeakTable<object, SyncBlock> s_SyncTable = [];

        private class SyncBlock {

            internal SyncBlock? m_Next;

            public SyncBlock Next {

                get {
                    lock (this) {
                        var next = m_Next;
                        if (next == null) {
                            next = new SyncBlock();
                            m_Next = next;
                        }
                        return next;
                    }
                }
            }

            public SyncBlock() {
            }
        }
        bool IEqualityComparer<object?>.Equals(object? x, object? y) {
            return Equals(x, y);
        }

        public new static bool Equals(object? x, object? y) {
            return ReferenceEquals(x, y);
        }

        int IEqualityComparer<object?>.GetHashCode([DisallowNull] object? obj) {
            return GetHashCode(obj);
        }

        public static int GetHashCode([DisallowNull] object? obj) {
            return SystemRuntimeHelpers.GetHashCode(obj);
        }

        int IComparer<object?>.Compare(object? x, object? y) {
            return Compare(x, y);
        }

        public static int Compare(object? x, object? y) {
            if (x == null) {
                return y == null ? 0 : -1;
            }
            if (y == null) {
                return 1;
            }
            return CompareNotNull(x, y);
        }

        private static int CompareNotNull(object x, object y) {
            Debug.Assert(null != x);
            Debug.Assert(null != y);
            if (Miscellaneous.Likely(!Equals(x, y))) {
                return CompareCore(x, y);
            }
            return 0;
        }

        internal static int CompareCore(object x, object y) {
            Debug.Assert(null != x);
            Debug.Assert(null != y);
            Debug.Assert(!Equals(x, y));
            {
                var u = GetHashCode(x);
                var v = GetHashCode(y);
                if (Miscellaneous.Likely(u != v)) {
                    return u.CompareTo(v);
                }
            }
            {
                var p = s_SyncTable.GetOrCreateValue(x);
                var q = s_SyncTable.GetOrCreateValue(y);
                for (var i = 0; ; ++i) {
                    Debug.Assert(!ReferenceEquals(p, q));
                    var u = GetHashCode(p);
                    var v = GetHashCode(q);
                    if (Miscellaneous.Likely(u != v)) {
                        return u.CompareTo(v);
                    }
                    p = p.Next;
                    q = q.Next;
                }
            }
        }
    }

    static partial class ComparerExtionaions {

        extension<T, TComparer>(TComparer @this)
            where TComparer : IComparer<T> {

            public void Sort(ref T x, ref T y) {
                if (@this.Compare(x, y) > 0) {
                    (x, y) = (y, x);
                }
            }
        }
    }
}
