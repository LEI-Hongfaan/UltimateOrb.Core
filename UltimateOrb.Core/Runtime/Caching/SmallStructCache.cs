using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UltimateOrb.Runtime.Caching {

    /// <summary>
    /// Represents a small, fixed‑size, value‑type cache with <c>M</c> hash‑indexed slots,
    /// each containing up to <c>N</c> key/value entries.
    /// <para>
    /// This cache is designed for extremely low‑overhead scenarios: it is a pure struct,
    /// stores entries inline, performs no allocations, and uses a simple modulo‑hash
    /// dispatch to select a slot.
    /// </para>
    /// <para>
    /// Each slot maintains up to <c>N</c> entries and uses a deterministic overwrite
    /// policy (typically round‑robin) when all entries in the slot are occupied.
    /// </para>
    /// <para>
    /// Lookups, insertions, and removals operate only within the slot determined by
    /// <see cref="IEqualityComparer{T}.GetHashCode(T)"/> modulo the number of slots.
    /// No probing, chaining, or resizing occurs.
    /// </para>
    /// </summary>
    /// <typeparam name="TKey">
    /// The key type. Must be non‑null. Keys are compared using the configured
    /// <see cref="IEqualityComparer{TKey}"/>.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The value type stored in the cache.
    /// </typeparam>
    /// <remarks>
    /// <para>
    /// This cache is intended for high‑performance micro‑caching, memoization,
    /// or lookup tables where a small number of entries per hash slot is sufficient.
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Auto)]
    public struct SmallStructCache7x2<TKey, TValue> where TKey : notnull {

        /// <summary>
        /// Gets a value indicating whether this cache instance was default‑constructed
        /// (i.e., whether the comparer field is uninitialized).
        /// </summary>
        public readonly bool IsDefault => _comparer == default;

        /// <summary>
        /// Represents a single hash slot containing up to <c>N</c> key/value entries.
        /// The slot supports lookup, insertion with overwrite, and removal.
        /// <para>
        /// When all entries are occupied and a new key is inserted, the slot overwrites
        /// one entry according to a deterministic policy (e.g., round‑robin).
        /// </para>
        /// </summary>
        [StructLayout(LayoutKind.Auto)]
        private struct Entry {

            public TKey Key0;
            public TValue Value0;
            public bool Occ0;

            public TKey Key1;
            public TValue Value1;
            public bool Occ1;

            // 0 or 1: next index to overwrite when both occupied
            private byte _nextToOverwrite;

            /// <summary>
            /// Attempts to retrieve the value associated with the specified key.
            /// Lookup is restricted to this slot only.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryGet(TKey key, IEqualityComparer<TKey> comparer, out TValue value) {
                if (Occ0 && comparer.Equals(Key0, key)) {
                    value = Value0;
                    return true;
                }

                if (Occ1 && comparer.Equals(Key1, key)) {
                    value = Value1;
                    return true;
                }

                value = default!;
                return false;
            }

            /// <summary>
            /// Adds a key/value pair to the slot or replaces an existing entry.
            /// <para>
            /// If the slot has free capacity, the new entry is placed in the first available position.
            /// If the slot is full, an existing entry is overwritten according to the slot's
            /// deterministic overwrite policy.
            /// </para>
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddOrReplaceWithOverwrite(TKey key, TValue value, IEqualityComparer<TKey> comparer) {
                // Replace if present
                if (Occ0 && comparer.Equals(Key0, key)) {
                    Value0 = value;
                    return;
                }

                if (Occ1 && comparer.Equals(Key1, key)) {
                    Value1 = value;
                    return;
                }

                // Insert into first free position
                if (!Occ0) {
                    Key0 = key;
                    Value0 = value;
                    Occ0 = true;
                    return;
                }

                if (!Occ1) {
                    Key1 = key;
                    Value1 = value;
                    Occ1 = true;
                    return;
                }

                // Both occupied -> overwrite according to round-robin pointer
                if (_nextToOverwrite == 0) {
                    Key0 = key;
                    Value0 = value;
                    _nextToOverwrite = 1;
                } else {
                    Key1 = key;
                    Value1 = value;
                    _nextToOverwrite = 0;
                }
            }

            /// <summary>
            /// Attempts to remove the entry associated with the specified key.
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryRemove(TKey key, IEqualityComparer<TKey> comparer) {
                if (Occ0 && comparer.Equals(Key0, key)) {
                    Occ0 = false;
                    Key0 = default!;
                    Value0 = default!;
                    return true;
                }

                if (Occ1 && comparer.Equals(Key1, key)) {
                    Occ1 = false;
                    Key1 = default!;
                    Value1 = default!;
                    return true;
                }

                return false;
            }
        }

        // Seven inline entries
        [InlineArray(7)]
        private struct InlineArrayA {
            private Entry _e0;
        }

        private InlineArrayA _entries;

        // Optional comparer stored; may be null if default is desired
        private readonly IEqualityComparer<TKey> _comparer;

        /// <summary>
        /// Initializes a new cache instance using <see cref="EqualityComparer{TKey}.Default"/>.
        /// All entries start empty.
        /// </summary>
        public SmallStructCache7x2() {
            _entries = default;
            _comparer = EqualityComparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new cache instance using the specified key comparer.
        /// All entries start empty.
        /// </summary>
        /// <param name="comparer">
        /// The comparer used for key equality. If <c>null</c>, the default comparer is used.
        /// </param>
        public SmallStructCache7x2(IEqualityComparer<TKey>? comparer = null) {
            _entries = default;
            _comparer = comparer ?? EqualityComparer<TKey>.Default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [UnscopedRef]
        private ref Entry GetEntryRefByIndex(int index) {
            return ref _entries[index];
        }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{TKey}"/> used for key comparison.
        /// </summary>
        public readonly IEqualityComparer<TKey> Comparer => _comparer;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [UnscopedRef]
        private ref Entry GetEntry(TKey key) {
            int hash = _comparer.GetHashCode(key);
            int slotIndex = unchecked((int)((uint)hash % 7u));
            return ref GetEntryRefByIndex(slotIndex);
        }

        /// <summary>
        /// Attempts to retrieve the value associated with the specified key.
        /// Lookup is restricted to the slot determined by the key's hash modulo the number of slots.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value) {
            ref Entry slot = ref GetEntry(key);
            return slot.TryGet(key, _comparer, out value);
        }

        /// <summary>
        /// Adds a key/value pair to the cache or replaces an existing entry in the same slot.
        /// <para>
        /// If the slot has free capacity, the new entry is placed in the first available position.
        /// If the slot is full, an existing entry is overwritten according to the slot's
        /// deterministic overwrite policy.
        /// </para>
        /// </summary>
        public void TryAdd(TKey key, TValue value) {
            ref Entry slot = ref GetEntry(key);
            slot.AddOrReplaceWithOverwrite(key, value, _comparer);
        }

        /// <summary>
        /// Attempts to remove the entry associated with the specified key.
        /// Removal is restricted to the slot determined by the key's hash modulo the number of slots.
        /// </summary>
        public bool TryRemove(TKey key) {
            ref Entry slot = ref GetEntry(key);
            return slot.TryRemove(key, _comparer);
        }
    }
}
