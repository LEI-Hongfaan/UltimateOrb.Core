using System.Collections.Generic;

namespace UltimateOrb {

    public partial struct StringOrdinalEqualityComparer : SequenceSearchModule.ISequenceEqualityComparerWithRollingHash<char, StringOrdinalEqualityComparer.HashCodeBuilder> {

        public HashCodeBuilder CreateHashCodeBuilder<TList>(TList source, int start, int count) where TList : IReadOnlyList<char> {
            return HashCodeBuilder.Create(source, start, count);
        }

        public bool Equals<TListFirst, TListSecond>(TListFirst first, int startFirst, int countFirst, TListSecond second, int startSecond, int countSecond)
            where TListFirst : IReadOnlyList<char>
            where TListSecond : IReadOnlyList<char> {
            if (countFirst == countSecond) {
                var i = startFirst;
                var j = startSecond;
                for (var c = countFirst; c > 0; --c) {
                    if (first[i++] != second[j++]) {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool Equals(char x, char y) {
            return x == y;
        }

        public int GetHashCode<TList>(TList source, int start, int count) where TList : IReadOnlyList<char> {
            var i = start;
            var c = i + count;
            var v = (uint)0;
            for (; c > i; ++i) {
                v = unchecked((uint)(((ulong)m * v + source[i]) % p));
            }
            return unchecked((int)v);
        }

        public int GetHashCode(char obj) {
            return unchecked((ushort)obj);
        }

        private const uint m = 256;

        private const uint p = 2147483647;

        public partial struct HashCodeBuilder : SequenceSearchModule.IRollingHashCodeBuilder<char> {

            private readonly uint shiftMultiplier;

            private uint currentHashCode;

            public HashCodeBuilder(uint a, uint v) {
                this.shiftMultiplier = a;
                this.currentHashCode = v;
            }

            internal static HashCodeBuilder Create<TList>(TList source, int start, int count) where TList : IReadOnlyList<char> {
                var i = start;
                var c = i + count;
                var a = (uint)1;
                var v = (uint)0;
                for (; c > i; ++i) {
                    v = unchecked((uint)(((ulong)m * v + source[i]) % p));
                    a = unchecked((uint)((ulong)m * a % p));
                }
                return new HashCodeBuilder(a, v);
            }

            public int GetCurrentHashCode() {
                return unchecked((int)this.currentHashCode);
            }

            public void Shift(char @out, char @in) {
                var o = (uint)unchecked((ushort)@out);
                var i = (uint)unchecked((ushort)@in);
                var v = this.currentHashCode;
                this.currentHashCode = unchecked((uint)((((ulong)shiftMultiplier * (p - o)) + ((ulong)m * v + i)) % p));
            }
        }
    }
}
