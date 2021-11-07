
namespace UltimateOrb.Linq {

    public readonly ref partial struct SingletonRefEnumerable<T> {

        private readonly ByReference<T> Single;

        public SingletonRefEnumerable(ref T single) {
            this.Single = new ByReference<T>(ref single);
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        public ref partial struct Enumerator {

            private readonly SingletonRefEnumerable<T> _Container;

            private sbyte _Index;

            public Enumerator(SingletonRefEnumerable<T> container) {
                this._Container = container;
                this._Index = -1;
            }

            public ref T Current {

                get => ref _Container.Single.Value;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                var i = this._Index;
                if (1 > i) {
                    unchecked {
                        ++i;
                    }
                    this._Index = i;
                }
                return 0 == i;
            }

            public void Reset() {
                this._Index = -1;
            }
        }
    }
}

namespace UltimateOrb.Linq {
    using Local = Collections.Generic.Interfaces.Typed_Wrapped_Huge;

    public readonly partial struct SingletonValueEnumerable<T>
        : Local.IReadOnlyEnumerable<T, SingletonValueEnumerable<T>.Enumerator> {

        private readonly T Single;

        public SingletonValueEnumerable(T single) {
            this.Single = single;
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        public partial struct Enumerator
            : Local.IReadOnlyEnumerator<T> {

            private readonly SingletonValueEnumerable<T> _Container;

            private sbyte _Index;

            public Enumerator(SingletonValueEnumerable<T> container) {
                this._Container = container;
                this._Index = -1;
            }

            public T Current {

                get => _Container.Single;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                var i = this._Index;
                if (1 > i) {
                    unchecked {
                        ++i;
                    }
                    this._Index = i;
                }
                return 0 == i;
            }

            public void Reset() {
                this._Index = -1;
            }
        }
    }
}

namespace UltimateOrb.Linq {
    using Local = Collections.Generic.Interfaces.Typed_RefReturn_Wrapped_Huge;

    public partial class SingletonEnumerable<T>
        : Local.IEnumerable<T, SingletonEnumerable<T>.Enumerator> {

        private T Single;

        public SingletonEnumerable(T single) {
            this.Single = single;
        }

        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        public partial struct Enumerator
            : Local.IEnumerator<T> {

            private readonly SingletonEnumerable<T> _Container;

            private sbyte _Index;

            public Enumerator(SingletonEnumerable<T> container) {
                this._Container = container;
                this._Index = -1;
            }

            public ref T Current {

                get => ref _Container.Single;
            }

            public void Dispose() {
            }

            public bool MoveNext() {
                var i = this._Index;
                if (1 > i) {
                    unchecked {
                        ++i;
                    }
                    this._Index = i;
                }
                return 0 == i;
            }

            public void Reset() {
                this._Index = -1;
            }
        }
    }
}
