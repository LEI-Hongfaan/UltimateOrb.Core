using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Runtime.CompilerServices {

    [Experimental("UoWIP")]
    public interface IReadOnlyWrapper<out T> {

        T Value { get; }
    }

    [Experimental("UoWIP")]
    public interface IWrapper<T> : IReadOnlyWrapper<T> {

        T IReadOnlyWrapper<T>.Value { get => Value; }

        new T Value { get; set; }
    }

    [Experimental("UoWIP")]
    public interface IReadOnlyReferenceWrapper<T> : IReadOnlyWrapper<T> {

        T IReadOnlyWrapper<T>.Value { get => Value; }

        new ref readonly T Value { get; }
    }

    [Experimental("UoWIP")]
    public interface IReferenceWrapper<T> : IWrapper<T>, IReadOnlyReferenceWrapper<T> {

        T IWrapper<T>.Value { get => Value; set => Value = value; }

        T IReadOnlyWrapper<T>.Value { get => Value; }

        ref readonly T IReadOnlyReferenceWrapper<T>.Value { get => ref Value; }

        new ref T Value { get; }
    }

    [Experimental("UoWIP")]
    public interface IReadOnlyWrapper<TSelf, T>
      where TSelf : IReadOnlyWrapper<TSelf, T> {

        public static abstract TSelf Wrap(T value);

        T Value { get; }
    }

    [Experimental("UoWIP")]
    public interface IWrapper<TSelf, T> : IReadOnlyWrapper<TSelf, T>
        where TSelf : IWrapper<TSelf, T> {

        T IReadOnlyWrapper<TSelf, T>.Value { get => Value; }

        new T Value { get; set; }
    }

    [Experimental("UoWIP")]
    public interface IReadOnlyReferenceWrapper<TSelf, T> : IReadOnlyWrapper<TSelf, T>
        where TSelf : IReadOnlyReferenceWrapper<TSelf, T> {

        T IReadOnlyWrapper<TSelf, T>.Value { get => Value; }

        new ref readonly T Value { get; }
    }

    [Experimental("UoWIP")]
    public interface IReferenceWrapper<TSelf, T> : IWrapper<TSelf, T>, IReadOnlyReferenceWrapper<TSelf, T>
        where TSelf : IReferenceWrapper<TSelf, T> {

        T IWrapper<TSelf, T>.Value { get => Value; set => Value = value; }

        T IReadOnlyWrapper<TSelf, T>.Value { get => Value; }

        ref readonly T IReadOnlyReferenceWrapper<TSelf, T>.Value { get => ref Value; }

        new ref T Value { get; }
    }

    [Experimental("UoWIP")]
    public struct WrapperEqualityComparer<T, TWrapper, TComparer>
        : IEqualityComparer<TWrapper>
        where T : struct
        where TWrapper : IWrapper<TWrapper, T>
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
