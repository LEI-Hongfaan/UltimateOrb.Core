
using System;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Core.Tests {

    using NodeId = Int32;

    using SourceNodeLocalLinkId = Int32;

    using LinkId = Int64;
    using System.Collections;

    public interface ITupleBase {

        int Length {

            get;
        }
    }

    public interface ITuple : ITupleBase {
    }

    public interface ITuple<T> : ITupleBase {

        ref T Item {

            get;
        }
    }

    public interface ITuple<T1, T2> : ITupleBase {

        ref T1 Item1 {

            get;
        }

        ref T2 Item2 {

            get;
        }
    }

    public interface ITuple<T1, T2, T3> : ITupleBase {

        ref T1 Item1 {

            get;
        }

        ref T2 Item2 {

            get;
        }

        ref T3 Item3 {

            get;
        }
    }

    public interface ITuple<T1, T2, T3, T4> : ITupleBase {

        ref T1 Item1 {

            get;
        }

        ref T2 Item2 {

            get;
        }

        ref T3 Item3 {

            get;
        }

        ref T4 Item4 {

            get;
        }
    }

    public interface ITuple<T1, T2, T3, T4, T5> : ITupleBase {

        ref T1 Item1 {

            get;
        }

        ref T2 Item2 {

            get;
        }

        ref T3 Item3 {

            get;
        }

        ref T4 Item4 {

            get;
        }

        ref T5 Item5 {

            get;
        }
    }

    public interface ITuple<T1, T2, T3, T4, T5, T6> : ITupleBase {

        T1 Item1 {

            get;

            set;
        }

        T2 Item2 {

            get;

            set;
        }

        T3 Item3 {

            get;

            set;
        }

        T4 Item4 {

            get;

            set;
        }

        T5 Item5 {

            get;

            set;
        }

        T6 Item6 {

            get;

            set;
        }
    }

    public interface IReadOnlyTupleBase : ITupleBase {
    }

    public interface IReadOnlyTuple : IReadOnlyTupleBase {
    }

    public interface IReadOnlyTuple<T> : IReadOnlyTuple {

        ref readonly T Item {

            get;
        }
    }

    public interface IReadOnlyTuple<T1, T2> : IReadOnlyTuple {

        ref readonly T1 Item1 {

            get;
        }

        ref readonly T2 Item2 {

            get;
        }
    }

    public static partial class GraphModule {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static bool StackMainContains<TNodeValue, TLinkValue>(in (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            return 0 != (1 & node.NodeValue.Item6);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void StackMainSetContains<TNodeValue, TLinkValue>(ref (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item6 |= 1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void StackMainResetContains<TNodeValue, TLinkValue>(ref (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item6 &= ~(int)1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static bool StackEvalAState1<TNodeValue, TLinkValue>(in (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            return 0 != (2 & node.NodeValue.Item6);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void StackEvalASetState1<TNodeValue, TLinkValue>(ref (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item6 |= 2;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void StackEvalAResetState1<TNodeValue, TLinkValue>(ref (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item6 &= ~(int)2;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static bool IsVisited<TNodeValue, TLinkValue>(in (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            return node.NodeValue.Item1 > 0;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static NodeId GetDfnValue<TNodeValue, TLinkValue>(ref this (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            return node.NodeValue.Item1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static NodeId GetLLkValue<TNodeValue, TLinkValue>(ref this (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            return node.NodeValue.Item2;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void SetDfnValue<TNodeValue, TLinkValue>(ref this (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node, NodeId value)
           where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item1 = value;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void SetLLkValue<TNodeValue, TLinkValue>(ref this (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) node, NodeId value)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            node.NodeValue.Item2 = value;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static void aaaa<TNodeValue, TLinkValue>(ref this Graph<TNodeValue, TLinkValue> @this)
            where TNodeValue : ITuple<NodeId, NodeId, NodeId, NodeId, Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator, int> {
            var g = @this;
            var c = g.Data.m_count;
            var j0 = 0; // count of stack 0 (main)
            var j1 = 0; // count of stack 1 (eval a)
            var d = 0;
            {
                var en0 = g.GetNodeIdEnumerator();
                for (; en0.MoveNext();) {
                    var i0 = en0.Current;
                    ref var n0 = ref g.GetNodeInternalData(i0);
                    if (IsVisited(n0)) {
                        continue;
                    }
                    var i1 = i0;
                    L_1:
                    {
                        ref var n1 = ref g.GetNodeInternalData(i1);
                        Graph<TNodeValue, TLinkValue>.NodeNextNodeIdEnumerator en2;
                        if (StackEvalAState1(g.Data.m_buffer[j1])) {
                            en2 = g.Data.m_buffer[j1].NodeValue.Item5;
                            goto L_5;
                        }
                        ++d; // d > 0;
                        SetDfnValue(ref n1,d); // dfn value
                        SetLLkValue(ref n1,d); // llk value
                        g.Data.m_buffer[j0++].NodeValue.Item3 = i1; // stack 0 push NodeId of current node n1
                        StackMainSetContains(ref n1);
                        en2 = g.GetNodeNextNodeIdEnumerator(i1);
                        L_4:
                        if (!en2.MoveNext()) {
                            goto L_3;
                        }
                        L_5:
                        var i2 = en2.Current;
                        ref var n2 = ref g.GetNodeInternalData(i2);
                        if (StackEvalAState1(g.Data.m_buffer[j1])) {
                            SetLLkValue(ref n1, Math.Min(GetLLkValue(ref n1), GetLLkValue(ref n2)));
                        }
                        if (IsVisited(n2)) {
                            if (StackMainContains(n2)) {
                                SetLLkValue(ref n1, Math.Min(GetLLkValue(ref n1), GetDfnValue(ref n2)));
                            }
                        } else {
                            g.Data.m_buffer[j1].NodeValue.Item4 = i1;
                            g.Data.m_buffer[j1].NodeValue.Item5 = en2;
                            StackEvalASetState1(ref g.Data.m_buffer[j1]);
                            {
                                ++j1;
                            }
                            i1 = i2;
                            goto L_1;
                        }
                        goto L_4;
                        L_3:
                        en2.Dispose();
                        if (GetDfnValue(ref n1) == GetLLkValue(ref n1)) { // dfn value == llk value
                            for (; ; ) {
                                {
                                    --j0; // pop NodeId from stack 0
                                }
                                // operating the stack 0 popped Node object
                                StackMainResetContains(ref n1);
                                var h = g.Data.m_buffer[j0].NodeValue.Item3; // h <- NodeId of the Node
                                if (i1 == h) {
                                    break;
                                }
                            }
                        }
                    }
                    {
                        --j1; // pop NodeId from stack 1
                    }
                    if (0 <= j1) {
                        i1 = g.Data.m_buffer[j1].NodeValue.Item4;
                        goto L_1;
                    }
                }
                en0.Dispose();
            }
        }
    }

    public partial struct Graph<TNodeValue, TLinkValue> {

        public partial struct NodeIdEnumerator : System.Collections.Generic.IEnumerator<NodeId> {

            public NodeId m_current;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public NodeIdEnumerator(NodeId current) {
                this.m_current = current;
            }

            public NodeId Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_current;
            }

            object IEnumerator.Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_current;
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Dispose() {
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                var i = this.m_current;
                unchecked {
                    --i;
                }
                if (0 <= i) {
                    this.m_current = i;
                    return true;
                }
                this.m_current = -1;
                return false;
            }

            public void Reset() {
                throw new NotSupportedException();
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NodeIdEnumerator GetNodeIdEnumerator() {
            return new NodeIdEnumerator(checked((NodeId)this.Data.Count));
        }

        // TODO: API Rev
        public partial struct SourceNodeLocalLinkIdEnumerator : System.Collections.Generic.IEnumerator<SourceNodeLocalLinkId> {

            public SourceNodeLocalLinkId m_current;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public SourceNodeLocalLinkIdEnumerator(SourceNodeLocalLinkId current) {
                this.m_current = current;
            }

            public SourceNodeLocalLinkId Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_current;
            }

            object IEnumerator.Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_current;
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Dispose() {
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                var i = this.m_current;
                unchecked {
                    --i;
                }
                if (0 <= i) {
                    this.m_current = i;
                    return true;
                }
                this.m_current = -1;
                return false;
            }

            public void Reset() {
                throw new NotSupportedException();
            }
        }

        public partial struct NodeNextNodeIdEnumerator
            : Typed_RefReturn_Wrapped_Huge.Collections.Generic.IEnumerator<NodeId>
            , Typed_RefReturn_Wrapped_Huge.Collections.Generic.IReadOnlyEnumerator<NodeId> {

            public readonly (TLinkValue LinkValue, NodeId Target)[] m_buffer;
            public SourceNodeLocalLinkId m_current;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public NodeNextNodeIdEnumerator((TLinkValue LinkValue, NodeId Target)[] buffer, SourceNodeLocalLinkId current) {
                this.m_buffer = buffer;
                this.m_current = current;
            }

            public ref NodeId Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => ref this.m_buffer[this.m_current].Target;
            }

            NodeId System.Collections.Generic.IEnumerator<NodeId>.Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_buffer[this.m_current].Target;
            }

            ref readonly NodeId RefReturn.Collections.Generic.IReadOnlyEnumerator<NodeId>.Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => ref this.m_buffer[this.m_current].Target;
            }

            object IEnumerator.Current {

                [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
                get => this.m_buffer[this.m_current].Target;
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Dispose() {
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                var i = this.m_current;
                unchecked {
                    --i;
                }
                if (0 <= i) {
                    this.m_current = i;
                    return true;
                }
                this.m_current = -1;
                return false;
            }

            public void Reset() {
                throw new NotSupportedException();
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NodeNextNodeIdEnumerator GetNodeNextNodeIdEnumerator(NodeId node) {
            return new NodeNextNodeIdEnumerator(this.Data.m_buffer[node].LinkData.m_buffer, this.Data.m_buffer[node].LinkData.m_count);
        }

        public Plain.ValueTypes.Stack<(Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue)> Data;

        public partial struct DataSelector<TNodeValueResult, TLinkValueResult, TNodeValueSelector, TLinkValueSelector>
            : IO.IFunc<(Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue), (Plain.ValueTypes.Stack<(TLinkValueResult LinkValue, NodeId Target)> LinkData, TNodeValueResult NodeValue)>
            , IO.IFunc<(TLinkValue LinkValue, NodeId Target), (TLinkValueResult LinkValue, NodeId Target)>
            where TNodeValueSelector : IO.IFunc<TNodeValue, TNodeValueResult>
            where TLinkValueSelector : IO.IFunc<TLinkValue, TLinkValueResult> {

            public TNodeValueSelector NodeValueSelector;

            public TLinkValueSelector LinkValueSelector;

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public DataSelector(TNodeValueSelector nodeValueSelector, TLinkValueSelector linkValueSelector) {
                this.NodeValueSelector = nodeValueSelector;
                this.LinkValueSelector = linkValueSelector;
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public (Plain.ValueTypes.Stack<(TLinkValueResult LinkValue, NodeId Target)> LinkData, TNodeValueResult NodeValue) Invoke((Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) arg) {
                return (arg.LinkData.Select<(TLinkValueResult LinkValue, NodeId Target), DataSelector<TNodeValueResult, TLinkValueResult, TNodeValueSelector, TLinkValueSelector>>(this), this.NodeValueSelector.Invoke(arg.NodeValue));
            }

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public (TLinkValueResult LinkValue, NodeId Target) Invoke((TLinkValue LinkValue, NodeId Target) arg) {
                return (this.LinkValueSelector.Invoke(arg.LinkValue), arg.Target);
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Graph<TNodeValueResult, TLinkValueResult> Select<TNodeValueResult, TLinkValueResult, TNodeValueSelector, TLinkValueSelector>(TNodeValueSelector nodeValueSelector, TLinkValueSelector linkValueSelector)
            where TNodeValueSelector : IO.IFunc<TNodeValue, TNodeValueResult>
            where TLinkValueSelector : IO.IFunc<TLinkValue, TLinkValueResult> {
            return new Graph<TNodeValueResult, TLinkValueResult>(Data.Select<(Plain.ValueTypes.Stack<(TLinkValueResult LinkValue, NodeId Target)> LinkData, TNodeValueResult NodeValue), DataSelector<TNodeValueResult, TLinkValueResult, TNodeValueSelector, TLinkValueSelector>>(new DataSelector<TNodeValueResult, TLinkValueResult, TNodeValueSelector, TLinkValueSelector>(nodeValueSelector, linkValueSelector)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Graph(int capacity) {
            this.Data = new Plain.ValueTypes.Stack<(Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue)>(capacity);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public Graph(Plain.ValueTypes.Stack<(Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue)> data) {
            this.Data = data;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref (Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)> LinkData, TNodeValue NodeValue) GetNodeInternalData(NodeId node) {
            return ref this.Data.m_buffer[node];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NodeId AddNode(TNodeValue value) {
            var item = (new Plain.ValueTypes.Stack<(TLinkValue LinkValue, NodeId Target)>(0), value);
            var @this = this.Data;
            var c = checked(@this.m_count + 1);
            if (null == @this.m_buffer || c > @this.m_buffer.Length) {
                var t = @this.m_count;
                @this.IncreaseCapacity();
                @this.m_buffer[unchecked(c - 1)] = item;
                this.Data.m_buffer = @this.m_buffer;
                this.Data.m_count = c;
                return t;
            }
            {
                @this.m_buffer[unchecked(c - 1)] = item;
                this.Data.m_count = c;
                return @this.m_count;
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public LinkId AddLink(NodeId source, NodeId target, TLinkValue value) {
            var item = (value, target);
            ref var this_ref = ref this.Data.m_buffer[source].LinkData;
            var @this = this_ref;
            var c = checked(@this.m_count + 1);
            if (null == @this.m_buffer || c > @this.m_buffer.Length) {
                var t = @this.m_count;
                @this.IncreaseCapacity();
                @this.m_buffer[unchecked(c - 1)] = item;
                this_ref.m_buffer = @this.m_buffer;
                this_ref.m_count = c;
                return GetLinkIdFromSourceNodeLocal(source, t);
            }
            {
                @this.m_buffer[unchecked(c - 1)] = item;
                this_ref.m_count = c;
                return GetLinkIdFromSourceNodeLocal(source, @this.m_count);
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref TNodeValue GetNodeValue(NodeId node) {
            return ref this.Data.m_buffer[node].NodeValue;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ref TLinkValue GetLinkValue(LinkId link) {
            return ref this.Data.m_buffer[GetSourceNode(link)].LinkData.m_buffer[GetLinkIdSourceNodeLocal(link)].LinkValue;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NodeId GetTargetNode(LinkId link) {
            return this.Data.m_buffer[GetSourceNode(link)].LinkData.m_buffer[GetLinkIdSourceNodeLocal(link)].Target;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public LinkId GetLinkIdFromSourceNodeLocal(NodeId source, SourceNodeLocalLinkId linkLocal) {
            return Combine(linkLocal, source);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public SourceNodeLocalLinkId GetLinkIdSourceNodeLocal(LinkId link) {
            return GetFirst(link);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public NodeId GetSourceNode(LinkId link) {
            return GetSecond(link);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static Int64 Combine(Int32 first, Int32 second) {
            return unchecked((Int64)Combine(unchecked((UInt32)first), unchecked((UInt32)second)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static UInt64 Combine(UInt32 first, UInt32 second) {
            return first | ((UInt64)second << 32);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static Int32 GetFirst(Int64 pair) {
            return unchecked((Int32)pair);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static UInt32 GetFirst(UInt64 pair) {
            return unchecked((UInt32)pair);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static Int32 GetSecond(Int64 pair) {
            return unchecked((Int32)GetSecond(unchecked((UInt64)pair)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static UInt32 GetSecond(UInt64 pair) {
            return unchecked((UInt32)(pair >> 32));
        }
    }
}
