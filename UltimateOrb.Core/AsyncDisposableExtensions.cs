using System;
using System.Threading.Tasks;

namespace UltimateOrb {

    public static partial class AsyncDisposableExtensions {

        public static ValueTask DisposeIfNotNullAsync<T>(this T? disposable) where T : IAsyncDisposable {
            if (disposable is not null) {
                return disposable.DisposeAsync();
            }
            return ValueTask.CompletedTask;
        }
    }
}
