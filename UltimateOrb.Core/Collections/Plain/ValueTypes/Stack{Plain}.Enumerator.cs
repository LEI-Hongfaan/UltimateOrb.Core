using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace UltimateOrb.Collections.Plain.ValueTypes {
    using UltimateOrb.Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;

    public partial struct Stack<T> {

        public partial struct Enumerator
            : IEnumerator<T> {

            private readonly Array<T> buffer;

            private readonly int count;

            private int index;

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public Enumerator(Stack<T> collection) {
                this.buffer = collection.m_buffer;
                var count = collection.m_count;
                this.count = count;
                this.index = count;
            }

            ref T Collections.Generic.Interfaces.RefReturn.IEnumerator<T>.Current => ref this.buffer[this.index];

            ref readonly T Collections.Generic.Interfaces.RefReturn.IReadOnlyEnumerator<T>.Current => ref this.buffer[this.index];

            T Collections.Generic.Interfaces.Core.IEnumerator<T>.Current {

                get => this.buffer[this.index];

                set => this.buffer[this.index] = value;
            }

            public void Dispose() {
                throw new System.NotImplementedException();
            }

            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() {
                var c = this.index;
                if (c > 0) {
                    unchecked {
                        --c;
                    }
                    this.index = c;
                    return true;
                }
                return false;
            }

            [EditorBrowsableAttribute(EditorBrowsableState.Advanced)]
            // [ReliabilityContractAttribute(Consistency.WillNotCorruptState, Cer.Success)]
            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            public void Reset() {
                this.index = this.count;
            }
        }
    }
}
