using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Utilities {

    public static class GraphModule {

        sealed class Shadow<T> {
            T item;
            ICollection<T> collection;

            public List<Wrapper<Shadow<T>>> Nexts = new List<Wrapper<Shadow<T>>>();

            public Shadow(T item, ICollection<T> collection) {
                this.collection = collection;
                this.item = item;
            }

            ~Shadow() {
                collection?.Remove(item);
            }
        }

        static class PerType_GetConnectedNodes_CreateShadow<T> {

            public static readonly Func<T, ICollection<T>, Shadow<T>> CreateShadow = (item, collection) => {
                return new Shadow<T>(item, collection);
            };
        }

        public static IEnumerable<TNode> GetConnectedNodes<TNode>(IEnumerable<(TNode Source, TNode Target)> edges, IEnumerable<TNode> roots) {
            if (edges is null) {
                throw new ArgumentNullException(nameof(edges));
            }
            if (roots is null) {
                throw new ArgumentNullException(nameof(roots));
            }
            var nodes = new HashSet<TNode>();
            var keepalive = new List<Shadow<TNode>>();
            GetConnectedNodes_Prepare(edges, roots, nodes, keepalive, PerType_GetConnectedNodes_CreateShadow<TNode>.CreateShadow);
            GC.Collect();
            return nodes;
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static void GetConnectedNodes_Prepare<TNode>(IEnumerable<(TNode Source, TNode Target)> edges, IEnumerable<TNode> roots, HashSet<TNode> nodes, List<Shadow<TNode>> keepalive, Func<TNode, ICollection<TNode>, Shadow<TNode>> creator) {
            var dict = new ConcurrentDictionary<TNode, Shadow<TNode>>();
            foreach (var (source, target) in edges) {
                nodes.Add(source);
                var s = dict.GetOrAdd(source, creator, nodes);
                nodes.Add(target);
                var t = dict.GetOrAdd(target, creator, nodes);
                s.Nexts.Add(t);
            }
            foreach (var root in roots) {
                nodes.Add(root);
                if (dict.TryGetValue(root, out var t)) {
                    keepalive.Add(t);
                }
            }
            dict.Clear();
        }
    }

    public class GCCollectionRoughDetector {

        private Action<GCCollectionRoughDetector> _Action;

        public ref Action<GCCollectionRoughDetector> Action {

            get => ref _Action;
        }

        ~GCCollectionRoughDetector() {
            Volatile.Read(ref _Action)?.Invoke(this);
        }
    }
}
