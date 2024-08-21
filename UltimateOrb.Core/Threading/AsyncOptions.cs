using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateOrb.Threading {

    public readonly struct AsyncOptions {

        public AsyncOptions(bool canceled) {
            CancellationToken = new(canceled);
        }

        public static AsyncOptions None { 
            
            get => default;
        }

        /// <inheritdoc cref="CancellationToken.IsCancellationRequested"/>
        public bool IsCancellationRequested {

            get => CancellationToken.IsCancellationRequested;
        }

        /// <inheritdoc cref="CancellationToken.CanBeCanceled"/>
        public bool CanBeCanceled { 
            
            get => CancellationToken.CanBeCanceled;
        }

        /// <inheritdoc cref="CancellationToken.WaitHandle"/>
        public WaitHandle WaitHandle { 
            
            get => CancellationToken.WaitHandle;
        }

        public bool Equals(AsyncOptions other) {
            return this.CancellationToken.Equals(other.CancellationToken) && this.YieldFunctor == other.YieldFunctor;
        }


        public override bool Equals([NotNullWhen(true)] object? other) {
            if (other is AsyncOptions options) {
                return this.Equals(options);
            }
            return false;
        }

        public override int GetHashCode() {
            var y = YieldFunctor;
            return CancellationToken.GetHashCode() ^ (null == y ? 0 : y.GetHashCode());
        }

        /// <inheritdoc cref="CancellationToken.Register(Action)"/>
        public CancellationTokenRegistration Register(Action callback) => CancellationToken.Register(callback);

        /// <inheritdoc cref="CancellationToken.Register(Action, bool)"/>
        public CancellationTokenRegistration Register(Action callback, bool useSynchronizationContext) => CancellationToken.Register(callback, useSynchronizationContext);

        /// <inheritdoc cref="CancellationToken.Register(Action{object?}, object?)"/>
        public CancellationTokenRegistration Register(Action<object?> callback, object? state) => CancellationToken.Register(callback, state);

        /// <inheritdoc cref="CancellationToken.Register(Action{object?}, object?, bool)"/>
        public CancellationTokenRegistration Register(Action<object?> callback, object? state, bool useSynchronizationContext)=> CancellationToken.Register(callback, state, useSynchronizationContext);

        /// <inheritdoc cref="CancellationToken.Register(Action{object?, CancellationToken}, object?)"/>
        public CancellationTokenRegistration Register(Action<object?, CancellationToken> callback, object? state) => CancellationToken.Register(callback, state);

        /// <inheritdoc cref="CancellationToken.ThrowIfCancellationRequested()"/>
        public void ThrowIfCancellationRequested()=> CancellationToken.ThrowIfCancellationRequested();

        /// <inheritdoc cref="CancellationToken.UnsafeRegister(Action{object?, CancellationToken}, object?)"/>
        public CancellationTokenRegistration UnsafeRegister(Action<object?, CancellationToken> callback, object? state) => CancellationToken.UnsafeRegister(callback, state);
        
        /// <inheritdoc cref="CancellationToken.UnsafeRegister(Action{object?}, object?)"/>
        public CancellationTokenRegistration UnsafeRegister(Action<object?> callback, object? state) => CancellationToken.UnsafeRegister(callback, state);

        public static bool operator ==(AsyncOptions first, AsyncOptions second) => first.Equals(second);

        public static bool operator !=(AsyncOptions first, AsyncOptions second) => !(first == second);
       
        public readonly CancellationToken CancellationToken {

            get;

            init;
        }

        public readonly Func<ValueTask>? YieldFunctor {

            get;

            init;
        }

        public static readonly Func<ValueTask> TaskYieldFunctor = TaskYield;

        static async ValueTask TaskYield() {
            await Task.Yield();
        }

        public ValueTask Yield() {
            var y = YieldFunctor;
            return y is null ? ValueTask.CompletedTask : y.Invoke();
        }

        public static implicit operator AsyncOptions(CancellationToken value) {
            return new AsyncOptions() { CancellationToken = value, };
        }

        public static explicit operator CancellationToken(AsyncOptions value) {
            return value.CancellationToken;
        }
    }
}
