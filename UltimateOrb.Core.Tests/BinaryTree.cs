using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb.Core.Tests {
    using IntT = Int32;
    using UIntT = UInt32;

    public static partial class BinaryTreeModule {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static BinaryTree<TResult>.Tree Select<TSource, TResult>(this BinaryTree<TSource>.Tree source, Func<TSource, IntT, TResult> selector) {
            if (null != source) {
                ref var node = ref source.Value.root;
                ref var result_node = ref Create(selector.Invoke(node.value, 2)).root;
                if (null != node.left_child) {
                    Select(ref node.left_child.Value, selector, 0, ref result_node);
                }
                if (null != node.right_child) {
                    Select(ref node.right_child.Value, selector, 1, ref result_node);
                }
                return result_node.tree;
            }
            // TODO
            throw new ArgumentNullException();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<TResult>.TreeStruct Select<TSource, TResult>(in this BinaryTree<TSource>.TreeStruct source, Func<TSource, IntT, TResult> selector) {
            ref readonly var node = ref source.root;
            var result_tree = CreateReferenceTyped(selector.Invoke(node.value, 2));
            ref var result = ref result_tree.Value;
            ref var result_node = ref result.root;
            if (null != node.left_child) {
                Select(ref node.left_child.Value, selector, 0, ref result_node);
            }
            if (null != node.right_child) {
                Select(ref node.right_child.Value, selector, 1, ref result_node);
            }
            return ref result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void Select<TSource, TResult>(ref this BinaryTree<TSource>.NodeStruct source, Func<TSource, IntT, TResult> selector, IntT index, ref BinaryTree<TResult>.NodeStruct parent) {
            var result = new BinaryTree<TResult>.Node(selector.Invoke(source.value, index), parent.tree);
            if (0 == index) {
                parent.left_child = result;
            } else if (1 == index) {
                parent.right_child = result;
            }
            if (null != source.left_child) {
                Select(ref source.left_child.Value, selector, 0, ref result.Value);
            }
            if (null != source.right_child) {
                Select(ref source.right_child.Value, selector, 1, ref result.Value);
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<T>(this BinaryTree<T>.Tree tree) {
            if (null != tree) {
                var sb = new System.Text.StringBuilder();
                sb.Append('(');
                sb.Append('{');
                tree.Value.root.ToString(sb);
                sb.Append(')');
                sb.Append('}');
                return sb.ToString();
            }
            // return "null";
            return "";
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string ToString<T>(in this BinaryTree<T>.TreeStruct tree) {
            var sb = new System.Text.StringBuilder();
            sb.Append('{');
            tree.root.ToString(sb);
            sb.Append('}');
            return sb.ToString();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ToString<T>(this BinaryTree<T>.Node node, System.Text.StringBuilder sb) {
            if (null != node) {
                sb.Append('(');
                sb.Append(node.Value.value.ToString());
                sb.Append('[');
                node.Value.left_child.ToString(sb);
                sb.Append(',');
                node.Value.right_child.ToString(sb);
                sb.Append(']');
                sb.Append(')');
                return;
            }
            {
                // sb.Append("null");
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void ToString<T>(in this BinaryTree<T>.NodeStruct node, System.Text.StringBuilder sb) {
            sb.Append(node.value.ToString());
            sb.Append('[');
            node.left_child.ToString(sb);
            sb.Append(',');
            node.right_child.ToString(sb);
            sb.Append(']');
            sb.Append(')');
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<T>.TreeStruct Create<T>(in T root_item) {
            var t = new BinaryTree<T>.Tree(root_item);
            return ref t.Value;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static BinaryTree<T>.Tree CreateReferenceTyped<T>(in T root_item) {
            var t = new BinaryTree<T>.Tree(root_item);
            return t;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<T>.NodeStruct AddSorted<T, TComparer>(ref this BinaryTree<T>.TreeStruct @this, in T item, in TComparer comparer) where TComparer : IComparer<T> {
            return ref AddSorted(ref @this.root, item, comparer);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<T>.NodeStruct AddSorted<T, TComparer>(this BinaryTree<T>.Tree @this, in T item, in TComparer comparer) where TComparer : IComparer<T> {
            return ref AddSorted(ref @this.Value.root, item, comparer);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<T>.NodeStruct AddSorted<T, TComparer>(ref this BinaryTree<T>.NodeStruct @this, in T item, in TComparer comparer) where TComparer : IComparer<T> {
            var t = comparer.Compare(@this.value, item);
            BinaryTree<T>.Node node;
            if (t > 0) {
                var child = @this.left_child;
                if (null != child) {
                    node = child;
                } else {
                    var b = new BinaryTree<T>.Node(item, @this.tree);
                    @this.left_child = b;
                    return ref b.Value;
                }
            } else {
                var child = @this.right_child;
                if (null != child) {
                    node = child;
                } else {
                    var b = new BinaryTree<T>.Node(item, @this.tree);
                    @this.right_child = b;
                    return ref b.Value;
                }
            }
            return ref node.AddSorted(item, comparer);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref BinaryTree<T>.NodeStruct AddSorted<T, TComparer>(this BinaryTree<T>.Node @this, in T item, in TComparer comparer) where TComparer : IComparer<T> {
            for (var node = @this; ;) {
                var t = comparer.Compare(node.Value.value, item);
                if (t > 0) {
                    var ch = node.Value.left_child;
                    if (null != ch) {
                        node = ch;
                        continue;
                    }
                    {
                        var b = new BinaryTree<T>.Node(item, node.Value.tree);
                        node.Value.left_child = b;
                        return ref b.Value;
                    }
                } else {
                    var ch = node.Value.right_child;
                    if (null != ch) {
                        node = ch;
                        continue;
                    }
                    {
                        var b = new BinaryTree<T>.Node(item, node.Value.tree);
                        node.Value.right_child = b;
                        return ref b.Value;
                    }
                }
            }
        }
    }

    public static partial class BinaryTree<T> {

        public partial struct TreeStruct {

            internal NodeStruct root;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal TreeStruct(T value, Tree tree, Node right_child, Node left_child) {
                this.root = new NodeStruct(value, tree, right_child, left_child);
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal TreeStruct(T value, Tree tree) {
                this.root = new NodeStruct(value, tree);
            }
        }

        public partial class Tree : StrongBoxBase<TreeStruct> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public Tree(T value) {
                this.Value = new TreeStruct(value, this);
            }

            internal protected new ref TreeStruct Value {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => ref base.Value;
            }
        }

        public partial class Node : StrongBoxBase<NodeStruct> {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal protected Node(T value, Tree tree, Node right_child, Node left_child) : base(new NodeStruct(value, tree, right_child, left_child)) {
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal protected Node(T value, Tree tree) : base(new NodeStruct(value, tree)) {
            }

            internal protected new ref NodeStruct Value {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => ref base.Value;
            }
        }

        public partial struct NodeStruct {

            internal T value;

            internal Node left_child;

            internal Node right_child;

            internal readonly Tree tree;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal NodeStruct(T item, Tree tree, Node right_child, Node left_child) {
                this.value = item;
                this.tree = tree;
                this.right_child = right_child;
                this.left_child = left_child;
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            internal NodeStruct(T item, Tree tree) {
                this.value = item;
                this.tree = tree;
                this.right_child = null;
                this.left_child = null;
            }
        }
    }
}
