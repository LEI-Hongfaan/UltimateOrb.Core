using System;
using System.Threading.Tasks;

namespace UltimateOrb {

    sealed class EmptyDisposable : IDisposable, IAsyncDisposable {

        internal static EmptyDisposable Instance = new EmptyDisposable();

        internal EmptyDisposable() {
        }

        public void Dispose() {
        }

        public ValueTask DisposeAsync() {
            return ValueTask.CompletedTask;
        }
    }

    /// <summary>
    /// Supports <see cref="IDisposable"/> interface.
    /// </summary>
    public static class Disposable {

        /// <summary>
        /// An empty IDisposable will do nothing when disposing or finalizing.
        /// </summary>
        public static readonly IDisposable Empty = EmptyDisposable.Instance;
    }

    /// <summary>
    /// Supports <see cref="IDisposable"/> interface.
    /// </summary>
    public static class AsyncDisposable {

        /// <summary>
        /// An empty IDisposable will do nothing when disposing or finalizing.
        /// </summary>
        public static readonly IAsyncDisposable Empty = EmptyDisposable.Instance;
    }
}
