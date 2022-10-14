/*
MIT License

Copyright (c) 2017 LEI Hongfaan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


Microsoft .NET Reference Source
The MIT License (MIT)

Copyright (c) Microsoft Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions: 

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software. 

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
*/

// #define DEBUG_LOGIC_LINKEDTREE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ListNamespace = System.Collections.Generic;

namespace UltimateOrb.Collections.Generic.ReferenceTypes {
    using UltimateOrb.Threading;
    using ThrowHelper = UltimateOrb.Utilities.ThrowHelper;

    public static class LinkedTreeExtensions {

        public static async ValueTask<IEnumerable<LinkedTree<TResult>>> ToTreesAsync<TSource, TNodeId, TResult>(this IEnumerable<TSource> source, Func<TSource, TNodeId> nodeIdSelector, Func<TSource, TNodeId> parentSelector, Func<TSource, TNodeId> previousSiblingSelector, Func<TSource, TResult> resultSelector, AsyncOptions asyncOptions = default) {
            var recorded = new Dictionary<TNodeId, LinkedTree<TResult>.Node>();
            var result = new ListNamespace.List<LinkedTree<TResult>>();
            var c = 0;
            
            foreach (var node in source) {
                var t = new LinkedTree<TResult>.Node();
                t.Value = resultSelector.Invoke(node);
                var id = nodeIdSelector.Invoke(node);
                recorded.Add(id, t);
                if (0 == ++c % 4096) {
                    await asyncOptions.Yield();
                }
            }

            foreach (var node in source) {
                var id = nodeIdSelector.Invoke(node);
                var t = recorded[id];

                var parentId = parentSelector.Invoke(node);
                var previousSiblingId = previousSiblingSelector.Invoke(node);
                if (null != parentId && recorded.TryGetValue(parentId, out var parent)) {
                    t.parent = parent;
                    if (null != previousSiblingId && recorded.TryGetValue(previousSiblingId, out var previousSibling)) {
                        t.previous_sibling = previousSibling;
                        previousSibling.next_sibling = t;
                    } else {
                        parent.first_child = t;
                    }
                } else {
                    if (null != previousSiblingId && recorded.TryGetValue(previousSiblingId, out var previousSibling)) {
                        t.parent = previousSibling.parent;
                        t.previous_sibling = previousSibling;
                        previousSibling.next_sibling = t;
                    } else {
                        var tree = new LinkedTree<TResult>();
                        t.tree = tree;
                        tree.root = t;
                        result.Add(tree);
                    }
                }
                if (0 == ++c % 4096) {
                    await asyncOptions.Yield();
                }
            }
            foreach (var p in recorded) {
                var node = p.Value;
                if (node.IsRootInternal) {
                    var tree = node.tree;
                    foreach (var n in tree.AsPreorderNodeEnumerable()) {
                        n.tree = tree;
                        if (null == n.next_sibling ) {
                            var parent = n.parent;
                            if (null != parent) {
                                parent.last_child = n;
                            }
                        }
                        if (0 == ++c % 4096) {
                            await asyncOptions.Yield();
                        }
                    }
                }
            }

#if DEBUG_LOGIC_LINKEDTREE
            foreach (var p in recorded) {
                if (!p.Value.Verify()) {
                    throw new InvalidOperationException();
                }
                if (0 == ++c % 4096) {
                    await System.Threading.Tasks.Task.Yield();
                }
            }
#endif
            return result;
        }
    }

    public partial class LinkedTree<T> : IEnumerable<T> {

        internal Node root;

        public Node Root {

            get {
                return this.root;
            }
        }

        public long Count {

            get {
                return this.AsDepthFirstEnumerable().LongCount();
            }
        }

        public override string ToString() {
            return $@"{{Count = {this.Count}}}";
        }

        public readonly struct PostorderNodeEnumerable : IEnumerable<Node> {

            internal readonly LinkedTree<T> tree;

            internal PostorderNodeEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PostorderNodeEnumerator GetEnumerator() {
                return new PostorderNodeEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct PreorderReversedNodeEnumerable : IEnumerable<Node> {

            internal readonly LinkedTree<T> tree;

            internal PreorderReversedNodeEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PreorderReversedNodeEnumerator GetEnumerator() {
                return new PreorderReversedNodeEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct PreorderNodeEnumerable : IEnumerable<Node> {

            internal readonly LinkedTree<T> tree;

            internal PreorderNodeEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PreorderNodeEnumerator GetEnumerator() {
                return new PreorderNodeEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct BreadthFirstNodeEnumerable : IEnumerable<Node> {

            internal readonly LinkedTree<T> tree;

            internal BreadthFirstNodeEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public BreadthFirstNodeEnumerator GetEnumerator() {
                return new BreadthFirstNodeEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct PostorderEnumerable : IEnumerable<T> {

            internal readonly LinkedTree<T> tree;

            internal PostorderEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PostorderEnumerator GetEnumerator() {
                return new PostorderEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct PreorderReversedEnumerable : IEnumerable<T> {

            internal readonly LinkedTree<T> tree;

            internal PreorderReversedEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PreorderReversedEnumerator GetEnumerator() {
                return new PreorderReversedEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }


        public readonly struct PreorderNodeModifiableEnumerable : IEnumerable<Node> {

            internal readonly LinkedTree<T> tree;

            internal PreorderNodeModifiableEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PreorderNodeModifiableEnumerator GetEnumerator() {
                return new PreorderNodeModifiableEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<Node> System.Collections.Generic.IEnumerable<Node>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct PreorderEnumerable : IEnumerable<T> {

            internal readonly LinkedTree<T> tree;

            internal PreorderEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public PreorderEnumerator GetEnumerator() {
                return new PreorderEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct DepthFirstEnumerable : IEnumerable<T> {

            internal readonly LinkedTree<T> tree;

            internal DepthFirstEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public IEnumerator<T> GetEnumerator() {
                Stack<Node.ChildrenEnumerator> s
                    = new Stack<Node.ChildrenEnumerator>(5);
                if (null != tree.root) {
                    yield return tree.root.value;
                    s.Push(tree.root.GetChildrenEnumerator());
                }
                for (; ; ) {
                    if (0 == s.Count) {
                        break;
                    }
                    var i = s.Pop();
                    while (i.MoveNext()) {
                        var t = i.Current;
                        yield return t.value;
                        if (null != t.first_child) {
                            s.Push(i);
                            i = t.GetChildrenEnumerator();
                            continue;
                        }
                    }
                    i.Dispose();
                }
                yield break;
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public readonly struct BreadthFirstEnumerable : IEnumerable<T> {

            internal readonly LinkedTree<T> tree;

            internal BreadthFirstEnumerable(LinkedTree<T> tree) {
                this.tree = tree;
            }

            public BreadthFirstEnumerator GetEnumerator() {
                return new BreadthFirstEnumerator(this.tree);
            }

            System.Collections.Generic.IEnumerator<T> System.Collections.Generic.IEnumerable<T>.GetEnumerator() {
                return this.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetEnumerator();
            }
        }

        public struct PostorderNodeEnumerator : IEnumerator<Node> {

            internal Node node;

            public PostorderNodeEnumerator(LinkedTree<T> tree) {
                this.node = new Node { next_sibling = tree.root };
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(this.node);
                }
#endif
            }

            public Node Current {

                get {
                    return node;
                }
            }

            public void Dispose() {
                node = null;
            }

            object System.Collections.IEnumerator.Current {

                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                if (null == this.node) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null == node.next_sibling) {
                    if (null == node.parent) {
                        return false;
                    } else {
                        node = node.parent;
                        return true;
                    }
                } else {
                    node = node.next_sibling;
                    for (; null != node.first_child; node = node.first_child) {
                    }
                    return true;
                }
            }

            public void Reset() {
                ThrowHelper.Throw<NotSupportedException>();
            }
        }

        public struct PreorderReversedNodeEnumerator : IEnumerator<Node> {

            internal Node node;

            public PreorderReversedNodeEnumerator(LinkedTree<T> tree) {
                this.node = new Node { previous_sibling = tree.root };
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(this.node);
                }
#endif
            }

            public Node Current {

                get {
                    return node;
                }
            }

            public void Dispose() {
                node = null;
            }

            object System.Collections.IEnumerator.Current {

                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                if (null == node) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null == node.previous_sibling) {
                    if (null == node.parent) {
                        return false;
                    } else {
                        node = node.parent;
                        return true;
                    }
                } else {
                    node = node.previous_sibling;
                    for (; null != node.last_child; node = node.last_child) {
                    }
                    return true;
                }
            }

            public void Reset() {
                ThrowHelper.Throw<NotSupportedException>();
            }
        }

        public struct PreorderNodeEnumerator : IEnumerator<Node> {

            internal Node node;

            public PreorderNodeEnumerator(LinkedTree<T> tree) {
                this.node = new Node { first_child = tree.root };
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(this.node);
                }
#endif
            }

            internal PreorderNodeEnumerator(Node node) {
                this.node = node;
            }

            public Node Current {

                get {
                    return node;
                }
            }

            public void Dispose() {
                node = null;
            }

            object System.Collections.IEnumerator.Current {

                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                if (null == this.node) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null == node.first_child) {
                    for (var temp = node; ; temp = temp.parent) {
                        if (null == temp.next_sibling) {
                            if (null == temp.parent) {
                                return false;
                            }
                        } else {
                            node = temp.next_sibling;
                            return true;
                        }
                    }
                } else {
                    node = node.first_child;
                    return true;
                }
            }

            public void Reset() {
                ThrowHelper.Throw<NotSupportedException>();
            }
        }

        public struct BreadthFirstNodeEnumerator : IEnumerator<Node> {

            internal Node node;

            public BreadthFirstNodeEnumerator(LinkedTree<T> tree) {
                this.node = new Node { next_sibling = tree.root };
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(this.node);
                }
#endif
            }

            public Node Current {

                get {
                    return node;
                }
            }

            public void Dispose() {
                node = null;
            }

            object System.Collections.IEnumerator.Current {

                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                var node = this.node;
                if (null == node) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                var temp = node.NextCousin;
                if (temp != null) {
                    this.node = temp;
                    return true;
                }
                var depth = 1L;
                // Node node = this;
                temp = node.parent;
                for (; temp != null;) {
                    node = temp;
                    checked {
                        ++depth;
                    }
                    temp = node.parent;
                }
                for (; ; node = temp) {
                    if (0 == depth) {
                        this.node = node;
                        return true;
                    }
                L_0001:;
                    temp = node.first_child;
                    if (null != temp) {
                        unchecked {
                            --depth;
                        }
                        continue;
                    }
                L_0002:;
                    temp = node.next_sibling;
                    if (null == temp) {
                        node = node.parent;
                        if (null == node) {
                            return false;
                        }
                        unchecked {
                            ++depth;
                        }
                        goto L_0002;
                    }
                    node = temp;
                    goto L_0001;
                }
            }

            public void Reset() {
                ThrowHelper.Throw<NotSupportedException>();
            }
        }

        /**
         * <summary>
         * modifiable:
         * any fields of current node and/or previous nodes - or -
         * any fields those refer to current node and/or previous nodes
         * </summary>
         */
        public struct PreorderNodeModifiableEnumerator : IEnumerator<Node> {

            private Node next_sibling;

            private Node first_child;

            internal Node node;

            private Stack<Node> s;

            private static readonly Stack<Node> s_ReadOnlyEmptyStack = new Stack<Node>(0);

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
            private static Stack<Node> GetReadOnlyEmptyStack() {
                return s_ReadOnlyEmptyStack;
            }

            public PreorderNodeModifiableEnumerator(LinkedTree<T> tree) {
                this.next_sibling = null;
                this.first_child = null;
                this.node = new Node { };
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(this.node);
                }
#endif
                var root = tree.root;
                if (root == null) {
                    this.s = GetReadOnlyEmptyStack();
                } else {
                    var s = new Stack<Node>(24);
                    s.Push(root);
                    this.s = s;
                }
            }

            public Node Current {

                get {
                    return node;
                }
            }

            public void Dispose() {
                this = default(PreorderNodeModifiableEnumerator);
            }

            object System.Collections.IEnumerator.Current {

                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                var node = this.node;
                if (null == node) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (first_child != null) {
                    if (next_sibling != null) {
                        s.Push(next_sibling);
                    }
                    this.node = first_child;
                    next_sibling = first_child.next_sibling;
                    first_child = first_child.first_child;
                } else {
                    if (next_sibling != null) {
                    } else if (s.Count == 0) {
                        return false;
                    } else {
                        next_sibling = s.Pop();
                    }
                    this.node = next_sibling;
                    first_child = next_sibling.first_child;
                    next_sibling = next_sibling.next_sibling;
                }
                return true;
            }

            public void Reset() {
                ThrowHelper.Throw<NotSupportedException>();
            }
        }

        public struct PostorderEnumerator : IEnumerator<T> {

            internal PostorderNodeEnumerator @this;

            public PostorderEnumerator(LinkedTree<T> tree) {
                @this = new PostorderNodeEnumerator(tree);
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(@this.node);
                }
#endif
            }

            public T Current {
                get {
                    return @this.Current.value;
                }
            }

            public void Dispose() {
                @this.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                return @this.MoveNext();
            }

            public void Reset() {
                @this.Reset();
            }
        }

        public struct PreorderReversedEnumerator : IEnumerator<T> {

            internal PreorderReversedNodeEnumerator @this;

            public PreorderReversedEnumerator(LinkedTree<T> tree) {
                @this = new PreorderReversedNodeEnumerator(tree);
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(@this.node);
                }
#endif
            }

            public T Current {
                get {
                    return @this.Current.value;
                }
            }

            public void Dispose() {
                @this.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                return @this.MoveNext();
            }

            public void Reset() {
                @this.Reset();
            }
        }

        public struct PreorderEnumerator : IEnumerator<T> {

            internal PreorderNodeEnumerator @this;

            public PreorderEnumerator(LinkedTree<T> tree) {
                @this = new PreorderNodeEnumerator(tree);
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(@this.node);
                }
#endif
            }

            public T Current {
                get {
                    return @this.Current.value;
                }
            }

            public void Dispose() {
                @this.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                return @this.MoveNext();
            }

            public void Reset() {
                @this.Reset();
            }
        }

        public struct BreadthFirstEnumerator : IEnumerator<T> {

            internal BreadthFirstNodeEnumerator @this;

            public BreadthFirstEnumerator(LinkedTree<T> tree) {
                @this = new BreadthFirstNodeEnumerator(tree);
#if DEBUG_LOGIC_LINKEDTREE
                lock (Node.NodesSkippingVerification) {
                    Node.NodesSkippingVerification.Add(@this.node);
                }
#endif
            }

            public T Current {
                get {
                    return @this.Current.value;
                }
            }

            public void Dispose() {
                @this.Dispose();
            }

            object System.Collections.IEnumerator.Current {
                get {
                    return this.Current;
                }
            }

            public bool MoveNext() {
                return @this.MoveNext();
            }

            public void Reset() {
                @this.Reset();
            }
        }

        public PreorderNodeModifiableEnumerable AsPreorderNodeModifiableEnumerable() {
            return new PreorderNodeModifiableEnumerable(this);
        }

        public PostorderNodeEnumerable AsPostorderNodeEnumerable() {
            return new PostorderNodeEnumerable(this);
        }

        public PreorderReversedNodeEnumerable AsPreorderReversedNodeEnumerable() {
            return new PreorderReversedNodeEnumerable(this);
        }

        public PreorderNodeEnumerable AsPreorderNodeEnumerable() {
            return new PreorderNodeEnumerable(this);
        }

        public BreadthFirstNodeEnumerable AsBreadthFirstNodeEnumerable() {
            return new BreadthFirstNodeEnumerable(this);
        }

        public PostorderEnumerable AsPostorderEnumerable() {
            return new PostorderEnumerable(this);
        }

        public PreorderReversedEnumerable AsPreorderReversedEnumerable() {
            return new PreorderReversedEnumerable(this);
        }

        public PreorderEnumerable AsPreorderEnumerable() {
            return new PreorderEnumerable(this);
        }

        public DepthFirstEnumerable AsDepthFirstEnumerable() {
            return new DepthFirstEnumerable(this);
        }

        public BreadthFirstEnumerable AsBreadthFirstEnumerable() {
            return new BreadthFirstEnumerable(this);
        }

        public PreorderNodeModifiableEnumerator GetPreorderNodeModifiableEnumerator() {
            return new PreorderNodeModifiableEnumerator(this);
        }

        public PostorderNodeEnumerator GetPostorderNodeEnumerator() {
            return new PostorderNodeEnumerator(this);
        }

        public PreorderReversedNodeEnumerator GetPreorderReversedNodeEnumerator() {
            return new PreorderReversedNodeEnumerator(this);
        }

        public PreorderNodeEnumerator GetPreorderNodeEnumerator() {
            return new PreorderNodeEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return (new DepthFirstEnumerable(this)).GetEnumerator();
        }

        public PostorderEnumerator GetPostorderEnumerator() {
            return new PostorderEnumerator(this);
        }

        public PreorderReversedEnumerator GetPreorderReversedEnumerator() {
            return new PreorderReversedEnumerator(this);
        }

        public PreorderEnumerator GetPreorderEnumerator() {
            return new PreorderEnumerator(this);
        }

        public void Clear() {
            var e = this.GetPreorderNodeModifiableEnumerator();
            while (e.MoveNext()) {
                e.Current.UncheckedResetInternal();
            }
            this.root = null;
            return;

            /*
            var r = this.root;
            if (null == r) {
                return;
            }
            this.root = null;
            for (r.tree = null; r.first_child != null; this.Remove(r.first_child)) {
            }*/
        }

        public LinkedTree<T> RemoveSubtree(Node node) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            LinkedTree<T> result;
            if (null == node.parent) {
            } else {
                if (null == node.next_sibling) {
                    node.parent.last_child = node.previous_sibling;
                }
                if (null == node.previous_sibling) {
                    node.parent.first_child = node.next_sibling;
                }
                node.previous_sibling = node.next_sibling = null;
                node.parent = null;
            }
            result = new LinkedTree<T> { root = node };
            var e = result.GetPreorderNodeModifiableEnumerator();
            while (e.MoveNext()) {
                e.Current.tree = result;
            }
            return result;
        }

        public void Remove(Node node) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (null == node.parent) {
                ThrowHelper.Throw<ArgumentOutOfRangeException>();
            }
            node.tree = null;
            var p = node.parent;
            node.parent = null;
            var t = node.first_child;
            if (null == t) {
                if (null == node.previous_sibling) {
                    p.first_child = node.next_sibling;
                    if (null == node.next_sibling) {
                        p.last_child = node.previous_sibling;
                    } else {
                        node.next_sibling.previous_sibling = node.previous_sibling;
                        node.next_sibling = null;
                    }
                } else {
                    node.previous_sibling.next_sibling = node.next_sibling;
                    if (null == node.next_sibling) {
                        p.last_child = node.previous_sibling;
                    } else {
                        node.next_sibling.previous_sibling = node.previous_sibling;
                        node.next_sibling = null;
                    }
                    node.previous_sibling = null;
                }
            } else {
                node.first_child = null;
                node.last_child = null;
                if (null == (t.previous_sibling = node.previous_sibling)) {
                    p.first_child = t;
                } else {
                    node.previous_sibling = null;
                    t.previous_sibling.next_sibling = t;
                }
                for (; ; ) {
                    t.parent = p;
                    if (null == t.next_sibling) {
                        if (null == (t.next_sibling = node.next_sibling)) {
                            p.last_child = t;
                        } else {
                            node.next_sibling = null;
                            t.next_sibling.previous_sibling = t;
                        }
                        break;
                    } else {
                        t = t.next_sibling;
                    }
                }
            }
        }

        internal Node SubtreeClone(LinkedTree<T> value) {
            Node result;
            var srcParent = value.root;
            var dstParent = new Node { tree = this, value = srcParent.value };
            result = dstParent;
            var q = new Queue<Node>();
            Node srcPreviousSibling;
            Node dstPreviousSibling;
            Node srcNode;
            Node dstNode;
        L_0001:;
            srcPreviousSibling = null;
            dstPreviousSibling = null;
        L_0002:;
            srcNode = srcPreviousSibling == null ? srcParent.first_child : srcPreviousSibling.next_sibling;
            if (srcNode == null) {
                dstParent.last_child = dstPreviousSibling;
                if (q.Count == 0) {
                    goto L_0003;
                }
                srcParent = q.Dequeue();
                dstParent = q.Dequeue();
                goto L_0001;
            } else {
                dstNode = new Node { tree = this, parent = dstParent, previous_sibling = dstPreviousSibling };
                if (dstPreviousSibling != null) {
                    dstPreviousSibling.next_sibling = dstNode;
                }
                dstPreviousSibling = dstNode;
                q.Enqueue(srcNode);
                q.Enqueue(dstNode);
                goto L_0002;
            }
        L_0003:;
            return result;
        }

        public Node Add(T value) {
            var result = new Node { tree = this, value = value, first_child = this.root, last_child = this.root };
            if (null != this.root) {
                this.root.parent = result;
            }
            return this.root = result;
        }

        public Node Add(Node value) {
            if (null != value.tree) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            value.tree = this;
            value.first_child = this.root;
            value.last_child = this.root;
            if (null != this.root) {
                this.root.parent = value;
            }
            return this.root = value;
        }

        public Node AddSynchronized(T value) {
            Node r = null;
            try {
                System.Threading.Monitor.Enter(this);
                r = this.root;
                var result = new Node { tree = this, value = value, first_child = r, last_child = r };
                if (null != r) {
                    System.Threading.Monitor.Enter(r);
                    r.parent = result;
                }
                return this.root = result;
            } finally {
                if (null != r && System.Threading.Monitor.IsEntered(r)) {
                    System.Threading.Monitor.Exit(r);
                }
                if (System.Threading.Monitor.IsEntered(this)) {
                    System.Threading.Monitor.Exit(this);
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void AddAfter_PatchNewLinks(Node node, Node value) {
            value.previous_sibling = node;
            value.next_sibling = node.next_sibling;
            value.parent = node.parent;
        }

        [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void AddAfter_PatchOldLinks(Node node, Node value) {
            node.next_sibling = value;
            if (null != value.next_sibling) {
                value.next_sibling.previous_sibling = value;
            }
            if (object.ReferenceEquals(node, node.parent.last_child)) {
                node.parent.last_child = value;
            }
        }

        public Node AddAfter(Node node, T value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (null == node.parent) {
                ThrowHelper.Throw<ArgumentOutOfRangeException>();
            }
            var result = new Node { tree = this, value = value };
            AddAfter_PatchNewLinks(node, result);
            AddAfter_PatchOldLinks(node, result);
            return result;
        }

        public Node AddAfter(Node node, Node value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (null == node.parent) {
                ThrowHelper.Throw<ArgumentOutOfRangeException>();
            }
            if (null != value.tree) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            value.tree = this;
            AddAfter_PatchNewLinks(node, value);
            AddAfter_PatchOldLinks(node, value);
            return value;
        }

        public Node AddAfter(Node node, LinkedTree<T> value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (null == node.parent) {
                ThrowHelper.Throw<ArgumentOutOfRangeException>();
            }
            if (null != value.root) {
                return null;
            }
            Node result = SubtreeClone(value);
            AddAfter_PatchNewLinks(node, result);
            AddAfter_PatchOldLinks(node, result);
            return result;
        }

        public LinkedTree<T> RemoveSubtreeAsTree(Node node) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (node.parent == null) {
                node.tree.root = null;
            } else {
                if (node.previous_sibling == null) {
                    node.parent.first_child = node.next_sibling;
                } else {
                    node.previous_sibling = node.next_sibling;
                }
                if (node.next_sibling == null) {
                    node.parent.last_child = node.previous_sibling;
                } else {
                    node.next_sibling = node.previous_sibling;
                }
            }
            var result = new LinkedTree<T> { root = node };
            var e = new PreorderNodeEnumerator(node);
            while (e.MoveNext()) {
                e.Current.tree = result;
            }
            return result;
        }

        public Node AddBefore(Node node, T value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            if (null == node.parent) {
                ThrowHelper.Throw<ArgumentOutOfRangeException>();
            }
            var result = node.previous_sibling = new Node { tree = this, next_sibling = node, value = value, previous_sibling = node.previous_sibling, parent = node.parent };
            if (null != result.previous_sibling) {
                result.previous_sibling.next_sibling = result;
            }
            if (object.ReferenceEquals(node, node.parent.first_child)) {
                node.parent.first_child = result;
            }
            return result;
        }

        public Node AddFirst(Node node, T value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            var t = node.first_child;
            if (null == t) {
                return node.first_child = node.last_child = new Node { tree = this, parent = node, value = value };
            } else {
                return node.first_child = t.previous_sibling = new Node { tree = this, parent = node, value = value, next_sibling = t };
            }
        }

        public Node AddLast(Node node, T value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            var t = node.last_child;
            if (null == t) {
                return node.last_child = node.first_child = new Node { tree = this, parent = node, value = value };
            } else {
                return node.last_child = t.next_sibling = new Node { tree = this, parent = node, value = value, previous_sibling = t };
            }
        }

        public Node AddLastSynchronized(Node node, T value) {
            var t = default(Node);
            try {
                System.Threading.Monitor.Enter(node);
                if (!object.ReferenceEquals(this, node.tree)) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                t = node.last_child;
                if (null == t) {
                    return node.last_child = node.first_child = new Node { tree = this, parent = node, value = value };
                } else {
                    System.Threading.Monitor.Enter(t);
                    return node.last_child = t.next_sibling = new Node { tree = this, parent = node, value = value, previous_sibling = t };
                }
            } finally {
                if (null != t && System.Threading.Monitor.IsEntered(t)) {
                    System.Threading.Monitor.Exit(t);
                }
                if (null != node && System.Threading.Monitor.IsEntered(node)) {
                    System.Threading.Monitor.Exit(node);
                }
            }
        }

        public Node AddParent(Node node, T value) {
            if (!object.ReferenceEquals(this, node.tree)) {
                ThrowHelper.Throw<InvalidOperationException>();
            }
            Node result = new Node { first_child = node, last_child = node, value = value, tree = this, next_sibling = node.next_sibling, previous_sibling = node.previous_sibling, parent = node.parent };
            if (null != node.parent) {
                if (null != node.next_sibling) {
                    node.next_sibling.previous_sibling = result;
                    node.next_sibling = null;
                } else {
                    node.parent.last_child = result;
                }
                if (null != node.previous_sibling) {
                    node.previous_sibling.next_sibling = result;
                    node.previous_sibling = null;
                } else {
                    node.parent.first_child = result;
                }
            } else {
                this.root = result;
            }
            return node.parent = result;
        }

        public IEnumerable<(long ChildCount, T Value)> GetPreorderNodeInfo() {
            foreach (var item in this.AsPreorderNodeEnumerable()) {
                yield return (item.GetChildCount(), item.value);
            }
        }

        public LinkedTree() {
        }

        public LinkedTree(T root) {
            this.root = new Node() { tree = this, value = root };
        }

        public LinkedTree(T root, IEnumerable<LinkedTree<T>> subtrees) {
            if (null == subtrees) {
                this.root = new Node() { tree = this, value = root };
                return;
            }
            var l = new ListNamespace.List<(long ChildCount, T Value)> {
                default
            };
            var c = 0L;
            foreach (var subtree in subtrees) {
                var a = l.Count;
                l.AddRange(subtree.GetPreorderNodeInfo());
                if (l.Count > a) {
                    checked {
                        ++c;
                    }
                }
            }
            l[0] = (c, root);
            var s = new Stack<long>(0);
            Node p = null;
            foreach (var item in l) {
                if (0 == item.ChildCount) {
                    if (null == p) {
                        this.root = new Node { tree = this, value = item.Value };
                        return;
                    } else {
                        p.AddLast(item.Value);
                        for (; ; ) {
                            var t = s.Pop();
                            if (0 == t) {
                                if (s.Count == 0) {
                                    return;
                                } else {
                                    p = p.parent;
                                }
                            } else {
                                s.Push(checked(t - 1));
                                break;
                            }
                        }
                    }
                } else {
                    if (null == p) {
                        p = this.root = new Node { tree = this, value = item.Value };
                    } else {
                        p = p.AddLast(item.Value);
                    }
                    s.Push(checked(item.ChildCount - 1));
                }
            }
        }

        public LinkedTree<T> Clone() {
            return new LinkedTree<T>(this.GetPreorderNodeInfo());
        }

        public LinkedTree(IEnumerable<(long ChildCount, T Value)> preorderNodeInfo) {
            var s = new Stack<long>();
            Node p = null;
            foreach (var item in preorderNodeInfo) {
                if (0 == item.ChildCount) {
                    if (null == p) {
                        this.root = new Node { tree = this, value = item.Value };
                        return;
                    } else {
                        p.AddLast(item.Value);
                        for (; ; ) {
                            var t = s.Pop();
                            if (0 == t) {
                                if (s.Count == 0) {
                                    return;
                                } else {
                                    p = p.parent;
                                }
                            } else {
                                s.Push(checked(t - 1));
                                break;
                            }
                        }
                    }
                } else {
                    if (null == p) {
                        p = this.root = new Node { tree = this, value = item.Value };
                    } else {
                        p = p.AddLast(item.Value);
                    }
                    s.Push(checked(item.ChildCount - 1));
                }
            }
        }

        public LinkedTree<TResult> StructuralSelect<TResult>(Func<T, TResult> selector) {
            return new LinkedTree<TResult>(this.GetPreorderNodeInfo().Select((x) => ((x.ChildCount, selector.Invoke(x.Value)))));
        }

        public bool StructuralEquals(IEqualityComparer<T> comparer, LinkedTree<T> other) {
            var node1 = other.root;
            var node = this.root;
            if (null == node) {
                return null == node1;
            }
            Node temp1;
            for (; ; ) {
                temp1 = node1.first_child;
                if (null == node.first_child) {
                    if (temp1 == null) {
                        temp1 = node1;
                    } else {
                        return false;
                    }
                    for (var temp = node; ;) {
                        temp1 = temp1.next_sibling;
                        if (null == temp.next_sibling) {
                            if (null == temp1) {
                            } else {
                                return false;
                            }
                            if (null == temp.parent) {
                                goto L_0003;
                            }
                        } else {
                            if (null == temp1) {
                                return false;
                            } else {
                            }
                            node = temp.next_sibling;
                            if (!comparer.Equals(node.value, temp1.value)) {
                                return false;
                            }
                            goto L_0002;
                        }
                        {
                            temp = temp.parent;
                            temp1 = temp1.parent;
                        }
                    }
                } else {
                    if (temp1 == null) {
                        return false;
                    } else {
                        node1 = temp1;
                    }
                    node = node.first_child;
                    if (!comparer.Equals(node.value, node1.value)) {
                        return false;
                    }
                    goto L_0002;
                }
            L_0002:;
            }
        L_0003:;
            return true;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public long RadialHeight {
            get {
                if (root == null) {
                    return 0;
                }
                var t = 0L;
                for (var e = this.AsPostorderNodeEnumerable().GetEnumerator(); e.MoveNext();) {
                    var n = e.Current;
                    if (n.IsLeafInternal) {
                        var d = n.Depth;
                        if (t > d) {
                            t = d;
                        }
                    }
                }
                return t;
            }
        }

        public long Height {
            [System.Diagnostics.Contracts.PureAttribute()]
            get {
                var result = 0L;
                var node = this.root;
                if (node == null) {
                    return result;
                }
                for (var t = 0L; ;) {
                    if (null == node.first_child) {
                        for (var temp = node; ;) {
                            if (null == temp.next_sibling) {
                                if (null == temp.parent) {
                                    goto L_0003;
                                }
                            } else {
                                node = temp.next_sibling;
                                goto L_0002;
                            }
                            {
                                temp = temp.parent;
                                checked {
                                    --t;
                                }
                            }
                        }
                    } else {
                        node = node.first_child;
                        checked {
                            ++t;
                        }
                        if (t > result) {
                            result = t;
                        }
                        goto L_0002;
                    }
                L_0002:;
                }
            L_0003:;
                return result;
            }
        }
    }

    public partial class LinkedTree<T> {

        public partial class Node
#if DEBUG_LOGIC_LINKEDTREE
            : DebugObject, IVerifiable, ITreeNode, ITreeNodeTyped<Node>
#else
            : ITreeNode, ITreeNodeTyped<Node>
#endif
            {

#if DEBUG_LOGIC_LINKEDTREE
            internal static readonly ISet<Node> NodesSkippingVerification = new HashSet<Node>();
#endif

            internal LinkedTree<T> tree;

            internal Node parent;

            internal Node previous_sibling;

            internal Node last_child;

            internal Node next_sibling;

            internal Node first_child;

            internal T value;

#if DEBUG_LOGIC_LINKEDTREE
            bool IVerifiable.Verify() {
                return this.Verify();
            }
#endif

#if DEBUG_LOGIC_LINKEDTREE
            [System.Runtime.CompilerServices.DiscardableAttribute()]
            [System.Diagnostics.Contracts.PureAttribute()]
            public bool Verify() {
                lock (NodesSkippingVerification) {
                    if (NodesSkippingVerification.Contains(this)) {
                        return true;
                    }
                }
                if (null == this.tree) {
                    return null == this.next_sibling && null == this.first_child && null == this.previous_sibling && null == this.last_child && null == this.parent;
                } else {
                    if (null == this.parent) {
                        if (this != this.tree.root || null != this.next_sibling || null != this.previous_sibling) {
                            return false;
                        }
                    }
                    if ((null != this.first_child) != (null != this.last_child)) {
                        return false;
                    }
                    if (null != this.first_child) {
                        if (this != this.first_child.parent) {
                            return false;
                        }
                        if (null != this.first_child.previous_sibling) {
                            return false;
                        }
                    }
                    if (null != this.last_child) {
                        if (this != this.last_child.parent) {
                            return false;
                        }
                        if (null != this.last_child.next_sibling) {
                            return false;
                        }
                    }
                    if (null != this.next_sibling) {
                        if (this != this.next_sibling.previous_sibling) {
                            return false;
                        }
                    } else {
                        if (null != this.parent) {
                            if (this != this.parent.last_child) {
                                return false;
                            }
                        }
                    }
                    if (null != this.previous_sibling) {
                        if (this != this.previous_sibling.next_sibling) {
                            return false;
                        }
                    } else {
                        if (null != this.parent) {
                            if (this != this.parent.first_child) {
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
#endif

            [System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal void UncheckedResetInternal() {
                this.tree = null;
                this.parent = null;
                this.previous_sibling = null;
                this.last_child = null;
                this.next_sibling = null;
                this.first_child = null;
            }

            public Node PreviousCousin {
                get {
                    var depth = default(uint);
                    Node temp = this.previous_sibling;
                    if (null != temp) {
                        return temp;
                    }
                    for (Node node = this; ;) {
                        temp = node.parent;
                        if (null != temp) {
                            node = temp;
                            checked {
                                ++depth;
                            }
                        } else {
                            return null;
                        }
                    L_0001:;
                        temp = node.previous_sibling;
                        if (null != temp) {
                        L_0002:;
                            node = temp.last_child;
                            if (null != node) {
                                if (1 == depth) {
                                    return node;
                                }
                                temp = node;
                                unchecked {
                                    --depth;
                                }
                                goto L_0002;
                            } else {
                                node = temp;
                                goto L_0001;
                            }
                        }
                    }
                }
            }

            public Node NextCousin {
                get {
                    var depth = default(uint);
                    Node temp = this.next_sibling;
                    if (null != temp) {
                        return temp;
                    }
                    for (Node node = this; ;) {
                        temp = node.parent;
                        if (null != temp) {
                            node = temp;
                            checked {
                                ++depth;
                            }
                        } else {
                            return null;
                        }
                    L_0001:;
                        temp = node.next_sibling;
                        if (null != temp) {
                        L_0002:;
                            node = temp.first_child;
                            if (null != node) {
                                if (1 == depth) {
                                    return node;
                                }
                                temp = node;
                                unchecked {
                                    --depth;
                                }
                                goto L_0002;
                            } else {
                                node = temp;
                                goto L_0001;
                            }
                        }
                    }
                }
            }

            public IEnumerable<Node> GetAncestorNodes() {
                var node = this.parent;
                while (null != node) {
                    yield return node;
                    node = node.parent;
                }
            }


            public Node FirstSibling {
                get {
                    return ((null == this.parent) ? (this) : (this.first_child));
                }
            }

            public Node LastSibling {
                get {
                    return ((null == this.parent) ? (this) : (this.last_child));
                }
            }

            public Node FirstCousin {
                get {
                    var depth = default(uint);
                    Node node = this;
                    Node temp = this.parent;
                    for (; temp != null;) {
                        node = temp;
                        checked {
                            ++depth;
                        }
                        temp = node.parent;
                    }
                    for (; ; node = temp) {
                        if (0 == depth) {
                            return node;
                        }
                    L_0001:;
                        temp = node.first_child;
                        if (null != temp) {
                            unchecked {
                                --depth;
                            }
                            continue;
                        }
                    L_0002:;
                        temp = node.next_sibling;
                        if (null == temp) {
                            node = node.parent;
                            unchecked {
                                ++depth;
                            }
                            goto L_0002;
                        }
                        node = temp;
                        goto L_0001;
                    }
                }
            }

            public Node LastCousin {
                get {
                    var depth = 0L;
                    Node node = this;
                    Node temp = this.parent;
                    for (; temp != null;) {
                        node = temp;
                        checked {
                            ++depth;
                        }
                        temp = node.parent;
                    }
                    for (; ; node = temp) {
                        if (0 == depth) {
                            return node;
                        }
                    L_0001:;
                        temp = node.last_child;
                        if (null != temp) {
                            unchecked {
                                --depth;
                            }
                            continue;
                        }
                    L_0002:;
                        temp = node.previous_sibling;
                        if (null == temp) {
                            node = node.parent;
                            unchecked {
                                --depth;
                            }
                            goto L_0002;
                        }
                        node = temp;
                        goto L_0001;
                    }
                }
            }

            public long Depth {
                get {
                    var temp = this.parent;
                    for (var depth = -1L; ;) {
                        if (null == temp) {
                            return checked(++depth);
                        }
                        checked {
                            ++depth;
                        }
                        temp = temp.parent;
                    }
                }
            }

            public Node AddParent(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                Node result = new Node { first_child = this, last_child = this, value = value, tree = this.tree, next_sibling = this.next_sibling, previous_sibling = this.previous_sibling, parent = this.parent };
                if (null != this.parent) {
                    if (null != this.next_sibling) {
                        this.next_sibling.previous_sibling = result;
                        this.next_sibling = null;
                    } else {
                        this.parent.last_child = result;
                    }
                    if (null != this.previous_sibling) {
                        this.previous_sibling.next_sibling = result;
                        this.previous_sibling = null;
                    } else {
                        this.parent.first_child = result;
                    }
                } else {
                    this.tree.root = result;
                }
                return this.parent = result;
            }

            public Node Add(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                Node result = new Node { parent = this, value = value, tree = this.tree, first_child = this.first_child, last_child = this.last_child };
                for (var node = this.first_child; null != node; node = node.next_sibling) {
                    node.parent = result;
                }
                return this.first_child = this.last_child = result;
            }

            /*
            public Node Add(Node value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null != value.tree) {
                    ThrowHelper.Throw<ArgumentOutOfRangeException>();
                }
                value.parent = this;
                value.tree = this.tree;
                value.first_child = this.first_child;
                value.last_child = this.last_child;
                var result = value;
                for (var node = this.first_child; null != node; node = node.next_sibling) {
                    node.parent = result;
                }
                return this.first_child = this.last_child = result;
            }
            */

            public Node AddAfter(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null == this.parent) {
                    ThrowHelper.Throw<ArgumentOutOfRangeException>();
                }
                var result = this.next_sibling = new Node { previous_sibling = this, value = value, tree = this.tree, next_sibling = this.next_sibling, parent = this.parent };
                if (null != result.next_sibling) {
                    result.next_sibling.previous_sibling = result;
                }
                if (this == this.parent.last_child) {
                    this.parent.last_child = result;
                }
                return result;
            }

            public Node AddBefore(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                if (null == this.parent) {
                    ThrowHelper.Throw<ArgumentOutOfRangeException>();
                }
                var result = this.previous_sibling = new Node { next_sibling = this, value = value, tree = this.tree, previous_sibling = this.previous_sibling, parent = this.parent };
                if (null != result.previous_sibling) {
                    result.previous_sibling.next_sibling = result;
                }
                if (this == this.parent.first_child) {
                    this.parent.first_child = result;
                }
                return result;
            }

            public Node AddFirst(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                var t = this.first_child;
                if (null == t) {
                    return this.first_child = (this.last_child = new Node { parent = this, value = value, tree = this.tree });
                } else {
                    return this.first_child = (t.previous_sibling = new Node { parent = this, value = value, tree = this.tree, next_sibling = t });
                }
            }

            public Node AddLast(T value) {
                if (null == this.tree) {
                    ThrowHelper.Throw<InvalidOperationException>();
                }
                var t = this.last_child;
                if (null == t) {
                    return this.last_child = (this.first_child = new Node { parent = this, value = value, tree = this.tree });
                } else {
                    return this.last_child = (t.next_sibling = new Node { parent = this, value = value, tree = this.tree, previous_sibling = t });
                }
            }

            public T Value {

                get {
                    return this.value;
                }

                set {
                    this.value = value;
                }
            }

            public LinkedTree<T> LinkedTree {

                get {
                    return this.tree;
                }
            }

            public Node Parent {

                get {
                    return this.parent;
                }
            }

            public Node NextSibling {

                get {
                    return this.next_sibling;
                }
            }

            public Node FirstChild {

                get {
                    return this.first_child;
                }
            }

            public Node PreviousSibling {

                get {
                    return this.previous_sibling;
                }
            }

            public Node LastChild {

                get {
                    return this.last_child;
                }
            }

            public bool IsLeaf {

                get {
                    if (null == this.tree) {
                        ThrowHelper.Throw<InvalidOperationException>();
                    }
                    return this.IsLeafInternal;
                }
            }

            internal bool IsRootInternal {

                get {
                    return null == this.parent;
                }
            }

            public bool IsRoot {

                get {
                    if (null == this.tree) {
                        ThrowHelper.Throw<InvalidOperationException>();
                    }
                    return this.IsRootInternal;
                }
            }

            internal bool IsLeafInternal {

                get {
                    return null == this.first_child;
                }
            }

            public struct ChildrenEnumerator : IEnumerator<Node> {

                internal Node node;

                public ChildrenEnumerator(Node parent) {
                    this.node = new Node { next_sibling = parent.first_child };
#if DEBUG_LOGIC_LINKEDTREE
                    lock (NodesSkippingVerification) {
                        Node.NodesSkippingVerification.Add(this.node);
                    }
#endif
                }

                public ChildrenEnumerator(ChildrenEnumerable enumerable)
                    : this(enumerable.node) {
                }

                public Node Current {

                    get {
                        return this.node;
                    }
                }

                public void Dispose() {
                    node = null;
                }

                object System.Collections.IEnumerator.Current {

                    get {
                        return this.Current;
                    }
                }

                public bool MoveNext() {
                    if (null == this.node) {
                        return false;
                    }
                    return null != (this.node = this.node.next_sibling);
                }

                public void Reset() {
                    ThrowHelper.Throw<NotSupportedException>();
                }
            }

            public ChildrenEnumerator GetChildrenEnumerator() {
                return new ChildrenEnumerator(this);
            }

            public readonly struct ChildrenEnumerable : IEnumerable<Node> {

                internal readonly Node node;

                public ChildrenEnumerable(Node parent) {
                    node = parent;
                }

                public ChildrenEnumerator GetEnumerator() {
                    return new ChildrenEnumerator(this);
                }

                IEnumerator<Node> IEnumerable<Node>.GetEnumerator() {
                    return GetEnumerator();
                }

                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                    return GetEnumerator();
                }
            }

            public ChildrenEnumerable AsChildrenEnumerable() {
                return new ChildrenEnumerable(this);
            }

            public long GetChildCount() {
                var result = 0L;
                for (var node = this.first_child; node != null; node = node.next_sibling) {
                    checked {
                        ++result;
                    }
                }
                return result;
            }

            public override string ToString() {
                // TODO: ...
                return $@"{{{value.ToString()}}}";
            }

            IEnumerator<ITreeNode> IEnumerable<ITreeNode>.GetEnumerator() {
                return this.GetChildrenEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
                return this.GetChildrenEnumerator();
            }

            IEnumerator<Node> IEnumerable<Node>.GetEnumerator() {
                return this.GetChildrenEnumerator();
            }
        }
    }

    public partial interface IFixedTreeNode : IList<IFixedTreeNode> {
    }

    public partial interface ITreeNode : IEnumerable<ITreeNode> {
    }

    public partial interface ITreeNodeTyped<TNode> : IEnumerable<TNode> where TNode : ITreeNodeTyped<TNode> {
    }

    public partial interface IFixedTreeNodeTyped<TNode> : IList<TNode> where TNode : IFixedTreeNodeTyped<TNode> {
    }

    public static class TreeModule {

        public static IEnumerator<ITreeNodeTyped<TNode>> GetDescendants<TNode>(this ITreeNodeTyped<TNode> node) where TNode : ITreeNodeTyped<TNode> {
            var s
                = new Stack<IEnumerator<TNode>>(10);
            for (IEnumerator<TNode> enumerator = node.GetEnumerator(); ;) {
                while (enumerator.MoveNext()) {
                    var t = enumerator.Current;
                    yield return t;
                    s.Push(enumerator);
                    enumerator = t.GetEnumerator();
                    continue;
                }
                enumerator.Dispose();
                if (0 == s.Count) {
                    yield break;
                }
                var i = s.Pop();
            }
        }

        public static IEnumerator<ITreeNode> GetDescendants(this ITreeNode node) {
            var s
                = new Stack<IEnumerator<ITreeNode>>(10);
            for (IEnumerator<ITreeNode> enumerator = node.GetEnumerator(); ;) {
                while (enumerator.MoveNext()) {
                    var t = enumerator.Current;
                    yield return t;
                    s.Push(enumerator);
                    enumerator = t.GetEnumerator();
                    continue;
                }
                enumerator.Dispose();
                if (0 == s.Count) {
                    yield break;
                }
                var i = s.Pop();
            }
        }
    }
}

namespace UltimateOrb.Collections.Generic.Testing {

#if DEBUG_LOGIC_LINKEDTREE
    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public interface IVerifiable {

        bool Verify();
    }

    [System.Runtime.CompilerServices.DiscardableAttribute()]
    public partial class DebugObject {

        private static ulong stamp = 0;

        private static readonly LinkedList<(ulong Stamp, DebugObject Object)> traced_object_collection = new LinkedList<(ulong, DebugObject)>();

        private object object_collection_syncroot {

            get {
                return traced_object_collection;
            }
        }

        public DebugObject(bool tracing = true) {
            if (tracing) {
                lock (traced_object_collection) {
                    traced_object_collection.AddFirst((unchecked(stamp++), this));
                }
            }
        }

        public static bool VerifyAll() {
            lock (traced_object_collection) {
                try {
                    return traced_object_collection.Select((x) => x.Object as IVerifiable).Where((x) => null != x).All((x) => x.Verify());
                } catch (Exception) {
                    return false;
                }
            }
        }

        public static void UntraceAll() {
            lock (traced_object_collection) {
                stamp = 0;
                traced_object_collection.Clear();
            }
        }
    }
#endif
}