using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateOrb.Utilities {

    public struct FinalActionDisposable : IDisposable {
        
        Action? action;

        public FinalActionDisposable(Action action) {
            this.action = action;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke();
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable Defer(Action action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable(action);
        }
    }

    public struct FinalAsyncActionDisposable : IAsyncDisposable {

        Func<Task>? action;

        public FinalAsyncActionDisposable(Func<Task> action) {
            this.action = action;
        }

        public async ValueTask DisposeAsync() {
            var func = Interlocked.Exchange(ref this.action, null);
            if (null != func) {
               await func.Invoke();
            }
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable Defer(Func<Task> action) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable(action);
        }
    }

    /*
    public struct FinalFuncDisposable<TResult> : IDisposable where TResult: IDisposable {

        Func<TResult>? func;

        public FinalFuncDisposable(Func<TResult> func) {
            this.func = func;
        }

        public void Dispose() {
            var func = Interlocked.Exchange(ref this.func, null);
            if (null != func) {
                using var t = func.Invoke();
            }
        }
    }

    public static partial class Finalization {

        public static FinalFuncDisposable<TResult> Defer<TResult>(Func<TResult> func) where TResult : IDisposable {
            if (func is null) {
                throw new ArgumentNullException(nameof(func));
            }
            return new FinalFuncDisposable<TResult>(func);
        }
    }

    public struct FinalAsyncFuncDisposable<TResult> : IAsyncDisposable where TResult : IAsyncDisposable {

        Func<Task<TResult>>? func;

        public FinalAsyncFuncDisposable(Func<Task<TResult>> func) {
            this.func = func;
        }

        public async ValueTask DisposeAsync() {
            var func = Interlocked.Exchange(ref this.func, null);
            if (null != func) {
                await using var t = await func.Invoke();
            }
        }
    }

    public static partial class Finalization {

        public static FinalAsyncFuncDisposable<TResult> Defer<TResult>(Func<Task<TResult>> func) where TResult : IAsyncDisposable {
            if (func is null) {
                throw new ArgumentNullException(nameof(func));
            }
            return new FinalAsyncFuncDisposable<TResult>(func);
        }
    }
    */
}
