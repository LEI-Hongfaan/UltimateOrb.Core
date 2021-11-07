using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UltimateOrb.Linq;

namespace UltimateOrb {

    public static partial class TypeArithmetic {

        private static partial class A {

            public static readonly Plain.CacheDictionary<Type, IEnumerable<Type>, TypeComparer> GetCurryingFamily__Cache = new Plain.CacheDictionary<Type, IEnumerable<Type>, TypeComparer>();

        }

        public readonly partial struct TypeComparer : IEqualityComparer<Type> {

            public bool Equals(Type x, Type y) {
                return x == y;
            }

            public int GetHashCode(Type obj) {
                return obj.GetHashCode();
            }
        }

        internal static void GetUncurried0(Type func, ref Typed_RefReturn_Wrapped_Huge.Collections.Generic.List<Type> list) {
            var t = func.GetGenericTypeDefinition();
            var gtas = func.GenericTypeArguments;
            for (var i = 0; gtas.Length - 1 > i; ++i) {
                list.Add(gtas[i]);
            }
            var r = gtas[gtas.Length - 1];
            if (r.IsGenericType) {
                GetUncurried0(gtas[gtas.Length - 1], ref list);
            } else {
                list.Add(r);
            }
            return;
        }

        public static Type GetUncurried(Type func) {
            if (func.IsGenericType) {
                var t = func.GetGenericTypeDefinition();
                var gtas = func.GenericTypeArguments;
                var list = new Typed_RefReturn_Wrapped_Huge.Collections.Generic.List<Type>(17);
                for (var i = 0; gtas.Length - 1 > i; ++i) {
                    list.Add(gtas[i]);
                }
                var r = gtas[gtas.Length - 1];
                if (r.IsGenericType) {
                    GetUncurried0(r, ref list);
                } else {
                    list.Add(r);
                }
                var ts = list.ToArray();
                switch (ts.Length) {
                case 0:
                    throw new NotSupportedException();
                case 1:
                    return ts[0];
                case 2:
                    return typeof(Func<,>).MakeGenericType(ts);
                case 3:
                    return typeof(Func<,,>).MakeGenericType(ts);
                case 4:
                    return typeof(Func<,,,>).MakeGenericType(ts);
                case 5:
                    return typeof(Func<,,,,>).MakeGenericType(ts);
                case 6:
                    return typeof(Func<,,,,,>).MakeGenericType(ts);
                case 7:
                    return typeof(Func<,,,,,,>).MakeGenericType(ts);
                case 8:
                    return typeof(Func<,,,,,,,>).MakeGenericType(ts);
                case 9:
                    return typeof(Func<,,,,,,,,>).MakeGenericType(ts);
                case 10:
                    return typeof(Func<,,,,,,,,,>).MakeGenericType(ts);
                case 11:
                    return typeof(Func<,,,,,,,,,,>).MakeGenericType(ts);
                case 12:
                    return typeof(Func<,,,,,,,,,,,>).MakeGenericType(ts);
                case 13:
                    return typeof(Func<,,,,,,,,,,,,>).MakeGenericType(ts);
                case 14:
                    return typeof(Func<,,,,,,,,,,,,,>).MakeGenericType(ts);
                case 15:
                    return typeof(Func<,,,,,,,,,,,,,,>).MakeGenericType(ts);
                case 16:
                    return typeof(Func<,,,,,,,,,,,,,,,>).MakeGenericType(ts);
                case 17:
                    return typeof(Func<,,,,,,,,,,,,,,,,>).MakeGenericType(ts);
                default:
                    throw new PlatformNotSupportedException();
                }
            }
            return func;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Type GetFunc(Type t, Type tResult) {
            return typeof(Func<,>).MakeGenericType(new[] { t, tResult, });
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Type GetFunc(Type t1, Type t2, Type tResult) {
            return typeof(Func<,,>).MakeGenericType(new[] { t1, t2, tResult, });
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Type GetFunc(Type t1, Type t2, Type t3, Type tResult) {
            return typeof(Func<,,,>).MakeGenericType(new[] { t1, t2, t3, tResult, });
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Type GetFunc(Type t1, Type t2, Type t3, Type t4, Type tResult) {
            return typeof(Func<,,,,>).MakeGenericType(new[] { t1, t2, t3, t4, tResult, });
        }

        public static IEnumerable<Type> GetCurryingFamily(Type func) {
            if (func.IsGenericType) {
                var t = func.GetGenericTypeDefinition();
                var gtas = func.GenericTypeArguments;
                var list = new Typed_RefReturn_Wrapped_Huge.Collections.Generic.List<Type>(17);
                for (var i = 0; gtas.Length - 1 > i; ++i) {
                    list.Add(gtas[i]);
                }
                var r = gtas[gtas.Length - 1];
                if (r.IsGenericType) {
                    GetUncurried0(r, ref list);
                } else {
                    list.Add(r);
                }
                var ts = list.ToArray();
                var res = (IEnumerable<Type>)null;
                switch (ts.Length) {
                case 0:
                    throw new NotSupportedException();
                case 1:
                    return new SingletonEnumerable<Type>(ts[0]);
                case 2:
                    return new SingletonEnumerable<Type>(typeof(Func<,>).MakeGenericType(ts));
                case 3:
                    res = new Type[] {
                        GetFunc(ts[0], GetFunc(ts[1], ts[2])),
                        typeof(Func<,,>).MakeGenericType(ts),
                    };
                    return res;
                case 4:
                    res = new Type[] {
                        GetFunc(ts[0], GetFunc(ts[1], GetFunc(ts[2], ts[3]))),
                        GetFunc(ts[0], ts[1], GetFunc(ts[2], ts[3])),
                        GetFunc(ts[0], GetFunc(ts[1], ts[2], ts[3])),
                        typeof(Func<,,,>).MakeGenericType(ts),
                    };
                    return res;
                case 5:
                    res = new Type[] {
                        GetFunc(ts[0], GetFunc(ts[1], GetFunc(ts[2], GetFunc(ts[3], ts[4])))),
                        GetFunc(ts[0], ts[1], GetFunc(ts[2], GetFunc(ts[3], ts[4]))),
                        GetFunc(ts[0], GetFunc(ts[1], ts[2], GetFunc(ts[3], ts[4]))),
                        GetFunc(ts[0], ts[1], ts[2], GetFunc(ts[3], ts[4])),
                        GetFunc(ts[0], GetFunc(ts[1], GetFunc(ts[2], ts[3], ts[4]))),
                        GetFunc(ts[0], ts[1], GetFunc(ts[2], ts[3], ts[4])),
                        GetFunc(ts[0], GetFunc(ts[1], ts[2], ts[3], ts[4])),
                        typeof(Func<,,,,>).MakeGenericType(ts),
                    };
                    return res;
                case 6:
                    throw new NotImplementedException();
                case 7:
                    throw new NotImplementedException();
                case 8:
                    throw new NotImplementedException();
                case 9:
                    throw new NotImplementedException();
                case 10:
                    throw new NotImplementedException();
                case 11:
                    throw new NotImplementedException();
                case 12:
                    throw new NotImplementedException();
                case 13:
                    throw new NotImplementedException();
                case 14:
                    throw new NotImplementedException();
                case 15:
                    throw new NotImplementedException();
                case 16:
                    throw new NotImplementedException();
                case 17:
                    throw new NotImplementedException();
                default:
                    throw new PlatformNotSupportedException();
                }
            }
            return new SingletonEnumerable<Type>(func);
        }
    }
}

namespace UltimateOrb.Plain {
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using UltimateOrb.Linq;
    using UltimateOrb.Utilities;

    public readonly partial struct CacheDictionary<TKey, TValue, TComparer>
        : IDictionary<TKey, TValue>
        where TComparer : struct, IEqualityComparer<TKey> {

        public partial class Entry {

            public readonly int m_HashCode;

            public readonly TKey m_Key;

            public readonly TValue m_Value;

            public Entry(int hashCode, TKey key, TValue value) {
                this.m_HashCode = hashCode;
                this.m_Key = key;
                this.m_Value = value;
            }
        }

        public CacheDictionary(Int32 capacity) {
            if (capacity > 0) {
                var mask = capacity;
                mask |= mask >> 1;
                mask |= mask >> 2;
                mask |= mask >> 4;
                mask |= mask >> 8;
                mask |= mask >> 16;
                checked {
                    ++mask;
                }
                var data = new Entry[mask];
                unchecked {
                    --mask;
                }
                this.m_Data = data;
                this.m_Mask = mask;
                return;
            }
            throw new ArgumentOutOfRangeException(nameof(capacity));
        }

        public readonly Entry[] m_Data;

        public readonly int m_Mask;

        public ICollection<TKey> Keys {

            get => Enumerable_Empty<TKey>.Boxed;
        }

        public ICollection<TValue> Values {

            get => Enumerable_Empty<TValue>.Boxed;
        }

        public int Count {

            get => 0;
        }

        public bool IsReadOnly {

            get => false;
        }

        public TValue this[TKey key] {

            get => this.TryGetValue(key, out var value) ? value : ThrowHelper.Throw<KeyNotFoundException, TValue>();

            set => this.Add(key, value);
        }

        public bool TryGetValue(TKey key, out TValue value) {
            var hashCode = default(TComparer).GetHashCode(key);
            var data = this.m_Data;
            var index = this.m_Mask & hashCode;
            var entry = Volatile.Read(ref data[index]);
            if (null != entry && hashCode == entry.m_HashCode && default(TComparer).Equals(key, entry.m_Key)) {
                value = entry.m_Value;
                return true;
            }
            Utilities.ThrowHelper.IgnoreOutParameter(out value);
            return false;
        }

        public ref Entry GetEntry(TKey key, out bool hasValue) {
            var hashCode = default(TComparer).GetHashCode(key);
            var data = this.m_Data;
            var index = this.m_Mask & hashCode;
            ref var entry_ref = ref data[index];
            var entry = Volatile.Read(ref entry_ref);
            if (null != entry && hashCode == entry.m_HashCode && default(TComparer).Equals(key, entry.m_Key)) {
                hasValue = true;
                return ref entry_ref;
            }
            hasValue = false;
            return ref entry_ref;
        }

        public void Add(TKey key, TValue value) {
            var hashCode = default(TComparer).GetHashCode(key);
            var data = this.m_Data;
            var index = this.m_Mask & hashCode;
            ref var entry_ref = ref data[index];
            var entry = Volatile.Read(ref entry_ref);
            if (null == entry || entry.m_HashCode != hashCode || !default(TComparer).Equals(key, entry.m_Key)) {
                Volatile.Write(ref entry_ref, new Entry(hashCode, key, value));
            }
        }

        public bool ContainsKey(TKey key) {
            return false;
        }

        public bool Remove(TKey key) {
            /*
            var hashCode = key.GetHashCode();
            var data = this.m_Data;
            var index  = this.m_Mask & hashCode;
            ref var entry_ref = ref data[index];
            var entry = Volatile.Read(ref entry_ref);
            if (null != entry && hashCode == entry.m_HashCode && default(TComparer).Equals(key, entry.m_Key)) {
                return entry == Interlocked.CompareExchange(ref entry_ref, null, entry);
            }
            */
            return false;
        }

        public void Add(KeyValuePair<TKey, TValue> item) {
            Add(item.Key, item.Value);
        }

        public void Clear() {
            var data = this.m_Data;
            for (var i = 0; data.Length > i; ++i) {
                data[i] = default;
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) {
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
        }

        public bool Remove(KeyValuePair<TKey, TValue> item) {
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            return Enumerable_Empty<KeyValuePair<TKey, TValue>>.EnumeratorBoxed;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Enumerable_Empty.EnumeratorBoxed;
        }
    }
}
