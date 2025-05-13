using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UltimateOrb.Collections.Specialized {

    [InlineArray(4)]
    struct Bits256 {
        UInt64 _value;
    }

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public struct BitSet256<T> : ISet<T>, IEquatable<BitSet256<T>>
        where T : IBinaryInteger<T>, IMinMaxValue<T> {

        Bits256 _sections;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int section, int bit) GetLocation(T value) {
            unchecked {
                var i = (uint)int.CreateTruncating(value);
                int section = (int)(i / 64);
                int bit = (int)(i % 64);
                return (section, bit);
            }
        }

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static BitSet256<T> Create<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                return otherSet;
            } else {
                var t = new BitSet256<T>();
                foreach (T item in other) {
                    t.Add(item);
                }
                return t;
            }
        }
#pragma warning restore CA1000 // Do not declare static members on generic types

        public bool Add(T item) {
            var (section, bit) = GetLocation(item);
            if (section < 0 || section >= 4) {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            ulong mask = 1UL << bit;
            ref ulong currentSection = ref _sections[section];
            if ((currentSection & mask) != 0) {
                return false;
            }

            currentSection |= mask;
            return true;
        }

        void ICollection<T>.Add(T item) => Add(item);

        public void ExceptWith<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    _sections[i] &= ~otherSet._sections[i];
                }
            } else {
                foreach (T item in other) {
                    Remove(item);
                }
            }
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other) => ExceptWith(other);

        public void IntersectWith<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    _sections[i] &= otherSet._sections[i];
                }
            } else {
                BitSet256<T> intersection = new();
                foreach (var item in this) {
                    if (other.Contains(item)) {
                        intersection.Add(item);
                    }
                }
                this = intersection;
            }
        }

        void ISet<T>.IntersectWith(IEnumerable<T> other) => IntersectWith(other);

        public readonly bool IsProperSubsetOf<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            return IsSubsetOf(other) && Count < other.Count();
        }

        readonly bool ISet<T>.IsProperSubsetOf(IEnumerable<T> other) => IsProperSubsetOf(other);

        public readonly bool IsProperSupersetOf<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                if (Equals(otherSet)) {
                    return false;
                }

                for (int i = 0; i < 4; i++) {
                    if ((_sections[i] & otherSet._sections[i]) != otherSet._sections[i]) {
                        return false;
                    }
                }

                ulong combined = 0;
                for (int i = 0; i < 4; i++) {
                    combined |= _sections[i];
                }
                return combined != 0;
            }

            var otherCollection = other as ICollection<T> ?? other.ToArray();
            return Count > otherCollection.Count && IsSupersetOf(otherCollection);
        }

        readonly bool ISet<T>.IsProperSupersetOf(IEnumerable<T> other) => IsProperSupersetOf(other);

        public readonly bool IsSubsetOf<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    if ((otherSet._sections[i] & _sections[i]) != _sections[i]) {
                        return false;
                    }
                }
                return true;
            }

            foreach (var item in this) {
                if (!other.Contains(item)) {
                    return false;
                }
            }

            return true;
        }

        readonly bool ISet<T>.IsSubsetOf(IEnumerable<T> other) => IsSubsetOf(other);

        public readonly bool IsSupersetOf<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    if ((_sections[i] & otherSet._sections[i]) != otherSet._sections[i]) {
                        return false;
                    }
                }
                return true;
            }

            foreach (var item in other) {
                if (!Contains(item)) {
                    return false;
                }
            }

            return true;
        }

        readonly bool ISet<T>.IsSupersetOf(IEnumerable<T> other) => IsSupersetOf(other);

        public readonly bool Overlaps<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    if ((_sections[i] & otherSet._sections[i]) != 0) {
                        return true;
                    }
                }
                return false;
            }

            foreach (var item in other) {
                if (Contains(item)) {
                    return true;
                }
            }

            return false;
        }

        readonly bool ISet<T>.Overlaps(IEnumerable<T> other) => Overlaps(other);

        public readonly bool SetEquals<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
            } else {
                otherSet = BitSet256<T>.Create(other);
            }
            for (int i = 0; i < 4; i++) {
                if (_sections[i] != otherSet._sections[i]) {
                    return false;
                }
            }
            return true;
        }

        readonly bool ISet<T>.SetEquals(IEnumerable<T> other) => SetEquals(other);

        public void SymmetricExceptWith<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    _sections[i] ^= otherSet._sections[i];
                }
            } else {
                foreach (var item in other) {
                    if (Contains(item)) {
                        Remove(item);
                    } else {
                        Add(item);
                    }
                }
            }
        }

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => SymmetricExceptWith(other);

        public void UnionWith<TEnumerable>(TEnumerable other) where TEnumerable : IEnumerable<T> {
            if (other is BitSet256<T> otherSet) {
                for (int i = 0; i < 4; i++) {
                    _sections[i] |= otherSet._sections[i];
                }
            } else {
                foreach (var item in other) {
                    Add(item);
                }
            }
        }

        void ISet<T>.UnionWith(IEnumerable<T> other) => UnionWith(other);

        public readonly bool Contains(T item) {
            var (section, bit) = GetLocation(item);
            if (section < 0 || section >= 4) {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            ulong mask = 1UL << bit;
            return (_sections[section] & mask) != 0;
        }

        public readonly void CopyTo(T[] array, int arrayIndex) {
            ArgumentNullException.ThrowIfNull(array);

            if (arrayIndex < 0 || arrayIndex >= array.Length) {
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            }

            if (array.Length - arrayIndex < Count) {
                throw new ArgumentException("Not enough space in the array.");
            }

            foreach (var item in this) {
                array[arrayIndex++] = item;
            }
        }

        public bool Remove(T item) {
            var (section, bit) = GetLocation(item);
            if (section < 0 || section >= 4) {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            ulong mask = 1UL << bit;
            ref ulong currentSection = ref _sections[section];
            if ((currentSection & mask) == 0) {
                return false;
            }

            currentSection &= ~mask;
            return true;
        }

        public readonly int Count {
            get {
                int count = 0;
                for (int i = 0; i < 4; i++) {
                    count += BitOperations.PopCount(_sections[i]);
                }
                return count;
            }
        }

        readonly bool ICollection<T>.IsReadOnly => false;

        public readonly IEnumerator<T> GetEnumerator() {
            var i = T.MinValue;
            for (var c = 0; c < 256; ++c) {
                if (Contains(i)) {
                    yield return i;
                }
                unchecked {
                    ++i;
                }
            }
        }

        readonly System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        public void Clear() => this = default;

        private readonly string GetDebuggerDisplay() => ToString() ?? string.Empty;

        public override readonly bool Equals(object? obj) => obj is BitSet256<T> set && Equals(set);

        public readonly bool Equals(BitSet256<T> other) {
            for (int i = 0; i < 4; i++) {
                if (_sections[i] != other._sections[i]) {
                    return false;
                }
            }
            return true;
        }

        public override readonly int GetHashCode() {
            HashCode hash = new();
            for (int i = 0; i < 4; i++) {
                hash.Add(_sections[i]);
            }
            return hash.ToHashCode();
        }

        public static bool operator ==(BitSet256<T> left, BitSet256<T> right) => left.Equals(right);

        public static bool operator !=(BitSet256<T> left, BitSet256<T> right) => !left.Equals(right);
    }
}
