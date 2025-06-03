using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Functional.Pure;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb {

    [Experimental("UoWIP")]
    public readonly ref struct PtrReferenceWrapper<T> : IReferenceWrapper<T> where T : unmanaged {

        readonly ref Ptr<T> ptr;

        public PtrReferenceWrapper(Ptr<T> ptr) {
            this.ptr = ptr;
        }

        T IWrapper<T>.Value { get => Value; set => Value = value; }

        T IReadOnlyWrapper<T>.Value { get => Value; }

        ref readonly T IReadOnlyReferenceWrapper<T>.Value { get => ref Value; }

        ref T IReferenceWrapper<T>.Value => ref Value;

        ref T Value { get => ref ptr.Dereferenced; }
    }

    [Experimental("UoWIP")]
    public static partial class PtrExtensions {

        public static PtrReferenceWrapper<T> AsReferenceWrapper<T>(this Ptr<T> ptr) where T : unmanaged {
            return new PtrReferenceWrapper<T>(ptr);
        }
    }


    [Experimental("UoWIP")]
    public struct WrapperEqualityComparer<T, TWrapper, TConverter, TComparer>
        : IEqualityComparer<TWrapper>
        where T : struct
        where TWrapper : IWrapper<T>
        where TComparer : IEqualityComparer<T> {

        TComparer comparer;

        public WrapperEqualityComparer(TComparer comparer) {
            this.comparer = comparer;
        }

        public bool Equals(T x, T y) {
            return comparer.Equals(x, y);
        }

        public bool Equals(TWrapper? x, TWrapper? y) {
            return x != null ? y != null && comparer.Equals(x.Value, y.Value) : y == null;
        }

        public int GetHashCode([DisallowNull] T obj) {
            return comparer.GetHashCode(obj);
        }

        public int GetHashCode([DisallowNull] TWrapper obj) {
            return obj is null ? 0 : comparer.GetHashCode(obj.Value);
        }
    }
}
