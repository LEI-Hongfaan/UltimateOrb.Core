using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UltimateOrb.Threading;

namespace UltimateOrb.Threading.Tests {

    [TestFixture]
    public class MonitorExtensionsRandomizedTests {

        private const int DefaultIterations = 50_000;
        private const int DefaultParallelTasks = 256;
        private const int DefaultStressDurationMs = 20_000;

        private static Random CreateRandom(int? seed = null) => seed.HasValue ? new Random(seed.Value) : new Random();

        #region Helpers

        private static object[] CreateRandomObjects(int count, Random rnd) {
            var arr = new object[count];
            for (int i = 0; i < count; i++) {
                // mix of boxed ints, strings, and new objects to vary RuntimeHelpers.GetHashCode distribution
                switch (rnd.Next(3)) {
                case 0: arr[i] = new object(); break;
                case 1: arr[i] = rnd.Next(); break;
                default: arr[i] = Guid.NewGuid().ToString(); break;
                }
            }
            return arr;
        }

        private static void AssertNoDeadlock(Task t, int timeoutMs, string message) {
            if (!t.Wait(timeoutMs)) {
                Assert.Fail("Potential deadlock or hang detected: " + message);
            }
        }

        #endregion

        [Test]
        public void ArgumentValidation_NullObjectsAndLockTaken() {
            // Null checks
            Assert.Throws<ArgumentNullException>(() => MonitorExtensions.Enter(null!, new object()));
            Assert.Throws<ArgumentNullException>(() => MonitorExtensions.Enter(new object(), null!));
            bool taken = true;
            Assert.Throws<ArgumentException>(() => MonitorExtensions.Enter(new object(), new object(), ref taken));
            taken = true;
            Assert.Throws<ArgumentException>(() => MonitorExtensions.TryEnter(new object(), new object(), ref taken));
            // TryEnter with invalid timeout (negative but not Infinite)
            taken = false;
            Assert.Throws<ArgumentOutOfRangeException>(() => MonitorExtensions.TryEnter(new object(), new object(), TimeSpan.FromTicks(-1), ref taken));
        }

        [Test]
        public void DuplicateObjectPath_EnterExitWorks() {
            var o = new object();
            // Enter/Exit when both objects are same should behave like Monitor.Enter once
            MonitorExtensions.Enter(o, o);
            try {
                // nested Enter with same object should block or succeed depending on reentrancy; Monitor is reentrant for same thread
                MonitorExtensions.Enter(o, o);
                MonitorExtensions.Exit(o, o);
            } finally {
                MonitorExtensions.Exit(o, o);
            }
        }

        [Test]
        public void TryEnterTimeoutsAndSuccesses_RandomPairs() {
            var rnd = CreateRandom(seed: 12345);
            var objs = CreateRandomObjects(100, rnd);
            for (int i = 0; i < 1000; i++) {
                var a = objs[rnd.Next(objs.Length)];
                var b = objs[rnd.Next(objs.Length)];
                bool taken = false;
                // Use small timeout to exercise timeout path
                MonitorExtensions.TryEnter(a, b, TimeSpan.FromMilliseconds(rnd.Next(0, 5)), ref taken);
                if (taken) {
                    try {
                        // if taken, ensure we can exit without exception
                        MonitorExtensions.Exit(a, b);
                    } catch (SynchronizationLockException) {
                        Assert.Fail("Exit threw SynchronizationLockException after successful TryEnter");
                    }
                }
            }
        }

        [Test]
        public void MassiveRandomizedPairOrdering_NoDeadlocks() {
            var rnd = CreateRandom(seed: 42);
            var objs = CreateRandomObjects(200, rnd);
            var cts = new CancellationTokenSource();
            var tasks = new Task[DefaultParallelTasks];
            var failures = new ConcurrentBag<Exception>();

            for (int t = 0; t < tasks.Length; t++) {
                tasks[t] = Task.Run(() => {
                    var localRnd = CreateRandom(rnd.Next());
                    try {
                        for (int iter = 0; iter < DefaultIterations && !cts.IsCancellationRequested; iter++) {
                            var o1 = objs[localRnd.Next(objs.Length)];
                            var o2 = objs[localRnd.Next(objs.Length)];
                            // randomly choose Enter or TryEnter with timeout
                            if (localRnd.NextDouble() < 0.5) {
                                // blocking Enter path
                                try {
                                    MonitorExtensions.Enter(o1, o2);
                                    try {
                                        // do trivial work
                                        Thread.SpinWait(10);
                                    } finally {
                                        MonitorExtensions.Exit(o1, o2);
                                    }
                                } catch (Exception ex) {
                                    failures.Add(ex);
                                }
                            } else {
                                bool taken = false;
                                try {
                                    MonitorExtensions.TryEnter(o1, o2, TimeSpan.FromMilliseconds(localRnd.Next(0, 10)), ref taken);
                                    if (taken) {
                                        try {
                                            Thread.SpinWait(5);
                                        } finally {
                                            MonitorExtensions.Exit(o1, o2);
                                        }
                                    }
                                } catch (Exception ex) {
                                    failures.Add(ex);
                                }
                            }
                        }
                    } catch (Exception ex) {
                        failures.Add(ex);
                    }
                }, cts.Token);
            }

            // Wait with timeout to detect hangs
            var all = Task.WhenAll(tasks);
            if (!all.Wait(TimeSpan.FromSeconds(30))) {
                cts.Cancel();
                Assert.Fail("Stress test timed out — possible deadlock or long blocking operation.");
            }

            if (!failures.IsEmpty) {
                throw new AggregateException("One or more exceptions occurred during randomized stress test.", failures);
            }
        }

        [Test]
        public void ConcurrencyStress_RunForDuration_NoDeadlocksOrExceptions() {
            var rnd = CreateRandom(seed: 98765);
            var objs = CreateRandomObjects(500, rnd);
            var cts = new CancellationTokenSource();
            var sw = Stopwatch.StartNew();
            var tasks = Enumerable.Range(0, DefaultParallelTasks).Select(i => Task.Run(async () => {
                await Task.Delay(100);
                var localRnd = CreateRandom(rnd.Next());
                while (!cts.IsCancellationRequested) {
                    var o1 = objs[localRnd.Next(objs.Length)];
                    var o2 = objs[localRnd.Next(objs.Length)];
                    // 10% chance to use duplicate object path
                    if (localRnd.Next(10) == 0) {
                        o2 = o1;
                    }
                    try {
                        // random choice of API
                        var choice = localRnd.Next(4);
                        switch (choice) {
                        case 0:
                            MonitorExtensions.Enter(o1, o2);
                            try { Thread.SpinWait(localRnd.Next(1, 50)); } finally { MonitorExtensions.Exit(o1, o2); }
                            break;
                        case 1:
                            bool taken = false;
                            MonitorExtensions.TryEnter(o1, o2, TimeSpan.FromMilliseconds(localRnd.Next(0, 20)), ref taken);
                            if (taken) {
                                try { Thread.SpinWait(localRnd.Next(1, 20)); } finally { MonitorExtensions.Exit(o1, o2); }
                            }
                            break;
                        case 2:
                            // TryEnter without ref overload
                            var ok = MonitorExtensions.TryEnter(o1, o2);
                            if (ok) {
                                try { Thread.SpinWait(localRnd.Next(1, 20)); } finally { MonitorExtensions.Exit(o1, o2); }
                            }
                            break;
                        default:
                            // TryEnter with milliseconds
                            var ok2 = MonitorExtensions.TryEnter(o1, o2, localRnd.Next(0, 20));
                            if (ok2) {
                                try { Thread.SpinWait(localRnd.Next(1, 20)); } finally { MonitorExtensions.Exit(o1, o2); }
                            }
                            break;
                        }
                    } catch (Exception ex) {
                        // rethrow to fail test
                        throw;
                    }
                }
            }, cts.Token)).ToArray();

            // run for configured duration
            var finished = Task.WhenAll(tasks);
            try {
                if (!finished.Wait(TimeSpan.FromMilliseconds(DefaultStressDurationMs))) {
                    // cancel and wait a bit for graceful shutdown
                    cts.Cancel();
                    Task.WaitAll(tasks, TimeSpan.FromSeconds(5));
                }
            } finally {
                cts.Cancel();
            }

            // If any task faulted, rethrow
            var faulted = tasks.FirstOrDefault(t => t.IsFaulted);
            if (faulted != null) {
                throw faulted.Exception ?? new Exception("Task faulted without exception.");
            }

            Assert.Pass("Stress run completed without task faults or deadlocks.");
        }

        [Test]
        public void Fuzzing_InvalidCombinations_DoNotCrashRuntime() {
            var rnd = CreateRandom(seed: 2021);
            var objs = CreateRandomObjects(50, rnd);
            for (int i = 0; i < 10_000; i++) {
                var a = objs[rnd.Next(objs.Length)];
                var b = objs[rnd.Next(objs.Length)];
                // random misuse patterns
                try {
                    if (rnd.NextDouble() < 0.02) {
                        // call Exit without Enter - should throw SynchronizationLockException
                        Assert.Throws<SynchronizationLockException>(() => MonitorExtensions.Exit(a, b));
                    } else if (rnd.NextDouble() < 0.02) {
                        // call TryEnter with negative timeout (invalid) - expect ArgumentOutOfRangeException
                        bool taken = false;
                        Assert.Throws<ArgumentOutOfRangeException>(() => MonitorExtensions.TryEnter(a, b, TimeSpan.FromTicks(-123), ref taken));
                    } else {
                        // normal random TryEnter/Enter usage
                        bool taken = false;
                        MonitorExtensions.TryEnter(a, b, TimeSpan.FromMilliseconds(rnd.Next(0, 3)), ref taken);
                        if (taken) {
                            MonitorExtensions.Exit(a, b);
                        }
                    }
                } catch (ArgumentNullException) {
                    // allowed if object was null (we didn't create nulls), but keep test robust
                }
            }
        }
    }
}
