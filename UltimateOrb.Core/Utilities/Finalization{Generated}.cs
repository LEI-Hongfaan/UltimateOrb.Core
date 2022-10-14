
#pragma warning disable IDE0190 // Null check can be simplified

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UltimateOrb.Utilities {

    public struct FinalActionDisposable<T> : IDisposable {
        
        Action<T>? action;
        
        readonly T arg;
        
        public FinalActionDisposable(Action<T> action, T arg) {
            this.action = action;
            this.arg = arg;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T> Defer<T>(this Action<T> action, T arg) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T>(action, arg);
        }
    }

    public struct FinalAsyncActionDisposable<T> : IAsyncDisposable {

        Func<T, Task>? action;
        
        readonly T arg;
        
        public FinalAsyncActionDisposable(Func<T, Task> action, T arg) {
            this.action = action;
            this.arg = arg;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T> Defer<T>(this Func<T, Task> action, T arg) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T>(action, arg);
        }
    }

    public struct FinalActionDisposable<T1, T2> : IDisposable {
        
        Action<T1, T2>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        public FinalActionDisposable(Action<T1, T2> action, T1 arg1, T2 arg2) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2> Defer<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2>(action, arg1, arg2);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2> : IAsyncDisposable {

        Func<T1, T2, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        public FinalAsyncActionDisposable(Func<T1, T2, Task> action, T1 arg1, T2 arg2) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2> Defer<T1, T2>(this Func<T1, T2, Task> action, T1 arg1, T2 arg2) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2>(action, arg1, arg2);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3> : IDisposable {
        
        Action<T1, T2, T3>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        public FinalActionDisposable(Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3> Defer<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3>(action, arg1, arg2, arg3);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3> : IAsyncDisposable {

        Func<T1, T2, T3, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, Task> action, T1 arg1, T2 arg2, T3 arg3) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3> Defer<T1, T2, T3>(this Func<T1, T2, T3, Task> action, T1 arg1, T2 arg2, T3 arg3) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3>(action, arg1, arg2, arg3);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4> : IDisposable {
        
        Action<T1, T2, T3, T4>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4> Defer<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4>(action, arg1, arg2, arg3, arg4);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4> : IAsyncDisposable {

        Func<T1, T2, T3, T4, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4> Defer<T1, T2, T3, T4>(this Func<T1, T2, T3, T4, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4>(action, arg1, arg2, arg3, arg4);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5> : IDisposable {
        
        Action<T1, T2, T3, T4, T5>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5> Defer<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5>(action, arg1, arg2, arg3, arg4, arg5);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5> Defer<T1, T2, T3, T4, T5>(this Func<T1, T2, T3, T4, T5, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5>(action, arg1, arg2, arg3, arg4, arg5);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6> Defer<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6>(action, arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6> Defer<T1, T2, T3, T4, T5, T6>(this Func<T1, T2, T3, T4, T5, T6, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6>(action, arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7> Defer<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7> Defer<T1, T2, T3, T4, T5, T6, T7>(this Func<T1, T2, T3, T4, T5, T6, T7, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8> Defer<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8> Defer<T1, T2, T3, T4, T5, T6, T7, T8>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        readonly T12 arg12;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
            this.arg12 = arg12;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        readonly T12 arg12;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
            this.arg12 = arg12;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
    }

    public struct FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IDisposable {
        
        Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        readonly T12 arg12;
        
        readonly T13 arg13;
        
        public FinalActionDisposable(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
            this.arg12 = arg12;
            this.arg13 = arg13;
        }

        public void Dispose() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
                action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
            }
        }
    }

    public static partial class Finalization {

        public static FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
    }

    public struct FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : IAsyncDisposable {

        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task>? action;
        
        readonly T1 arg1;
        
        readonly T2 arg2;
        
        readonly T3 arg3;
        
        readonly T4 arg4;
        
        readonly T5 arg5;
        
        readonly T6 arg6;
        
        readonly T7 arg7;
        
        readonly T8 arg8;
        
        readonly T9 arg9;
        
        readonly T10 arg10;
        
        readonly T11 arg11;
        
        readonly T12 arg12;
        
        readonly T13 arg13;
        
        public FinalAsyncActionDisposable(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            this.action = action;
            this.arg1 = arg1;
            this.arg2 = arg2;
            this.arg3 = arg3;
            this.arg4 = arg4;
            this.arg5 = arg5;
            this.arg6 = arg6;
            this.arg7 = arg7;
            this.arg8 = arg8;
            this.arg9 = arg9;
            this.arg10 = arg10;
            this.arg11 = arg11;
            this.arg12 = arg12;
            this.arg13 = arg13;
        }

        public ValueTask DisposeAsync() {
            var action = Interlocked.Exchange(ref this.action, null);
            if (null != action) {
               return new ValueTask(action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
            }
            return ValueTask.CompletedTask;
        }
    }

    public static partial class Finalization {

        public static FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Defer<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) {
            if (action is null) {
                throw new ArgumentNullException(nameof(action));
            }
            return new FinalAsyncActionDisposable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
    }
}
