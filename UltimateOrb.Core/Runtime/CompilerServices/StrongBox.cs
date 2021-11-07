using System.Runtime.CompilerServices;

namespace UltimateOrb.Runtime.CompilerServices {
    using Local = Interfaces.Typed_RefReturn_Wrapped_Huge;

    public partial class StrongBox<T>
        : Local.IStrongBox<T>
        , Local.IReadOnlyStrongBox<T> {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public StrongBox() {
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public StrongBox(in T value) {
            this.Value = value;
        }

        public T Value;

        ref T Interfaces.RefReturn.IStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }
    }

    public partial class ReadOnlyStrongBox<T>
        : Local.IStrongBox<T>
        , Local.IReadOnlyStrongBox<T>
        , Local.IWriteNotSupportedStrongBox<T> {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyStrongBox() {
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyStrongBox(in T value) {
            this.Value = value;
        }

        public readonly T Value;

        ref readonly T Interfaces.RefReturn.IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this.Value;
        }
    }

    public partial class StrongBoxBase<T>
        : Local.IStrongBox<T>
        , Local.IReadOnlyStrongBox<T> {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public StrongBoxBase() {
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public StrongBoxBase(in T value) {
            this.Value = value;
        }

        protected T Value;

        ref T Interfaces.RefReturn.IStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref Value;
        }
    }

    public partial class ReadOnlyStrongBoxBase<T>
        : Local.IStrongBox<T>
        , Local.IReadOnlyStrongBox<T>
        , Local.IWriteNotSupportedStrongBox<T> {

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyStrongBoxBase() {
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ReadOnlyStrongBoxBase(in T value) {
            this.Value = value;
        }

        protected readonly T Value;

        ref readonly T Interfaces.RefReturn.IReadOnlyStrongBox<T>.Value {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this.Value;
        }
    }
}
