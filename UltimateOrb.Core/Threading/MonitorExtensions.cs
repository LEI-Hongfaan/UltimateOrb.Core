using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;

namespace UltimateOrb.Threading {

    public static class MonitorExtensions {

        private readonly static ConditionalWeakTable<object, SyncBlock> s_SyncTable = new ConditionalWeakTable<object, SyncBlock>();

        private class SyncBlock {

            static long s_TotalCount = 0;

            internal readonly long m_Id;

            public SyncBlock() {
                if (0 > s_TotalCount) {
                    throw new InvalidOperationException("No more SyncBlock can be created.");
                }
                m_Id = unchecked(-Interlocked.Increment(ref s_TotalCount));
            }
        }

        public static void Enter(object obj1, object obj2, ref bool lockTaken) {
            // Probe and Check
            if (lockTaken) {
                throw new ArgumentException(nameof(lockTaken));
            }
            TryEntryInternal(obj1, obj2, Timeout.InfiniteTimeSpan, ref lockTaken);
        }

        public static bool TryEnter(object obj1, object obj2) {
            var lockTaken = false;
            TryEntryInternal(obj1, obj2, TimeSpan.Zero, ref lockTaken);
            return lockTaken;
        }

        public static void TryEnter(object obj1, object obj2, ref bool lockTaken) {
            // Probe and Check
            if (lockTaken) {
                throw new ArgumentException(nameof(lockTaken));
            }

            TryEntryInternal(obj1, obj2, TimeSpan.Zero, ref lockTaken);
        }

        public static bool TryEnter(object obj1, object obj2, int millisecondsTimeout) {
            bool lockTaken = false;
            TryEnter(obj1, obj2, millisecondsTimeout, ref lockTaken);
            return lockTaken;
        }

        public static bool TryEnter(object obj1, object obj2, TimeSpan timeout) {
            return TryEnter(obj1, obj2, timeout);
        }

        public static void TryEnter(object obj1, object obj2, int millisecondsTimeout, ref bool lockTaken) {
            // Probe and Check
            if (lockTaken) {
                throw new ArgumentException(nameof(lockTaken));
            }
            TryEntryInternal(obj1, obj2, Timeout.Infinite == millisecondsTimeout ? Timeout.InfiniteTimeSpan : TimeSpan.FromMilliseconds(millisecondsTimeout), ref lockTaken);
        }

        public static void TryEnter(object obj1, object obj2, TimeSpan timeout, ref bool lockTaken) {
            // Probe and Check
            if (lockTaken) {
                throw new ArgumentException(nameof(lockTaken));
            }
            TryEntryInternal(obj1, obj2, timeout, ref lockTaken);
        }

        static void TryEntryInternal(object obj1, object obj2, TimeSpan timeout, ref bool lockTaken) {
            if (obj1 is null) {
                throw new ArgumentNullException(nameof(obj1));
            }
            if (obj2 is null) {
                throw new ArgumentNullException(nameof(obj2));
            }
            if (0 > timeout.Ticks && timeout != Timeout.InfiniteTimeSpan) {
                throw new ArgumentOutOfRangeException(nameof(timeout));
            }
            Debug.Assert(!lockTaken);
#if DEBUG && DEBUG_ES
            ThrowException(0.04);
#endif
            if (!object.ReferenceEquals(obj1, obj2)) {
                GetOrdered(obj1, obj2, out object o1, out object o2);
                Debug.Assert(null != o1);
                Debug.Assert(null != o2);
                var t1 = false;
                var t2 = false;
                try {
                    if (timeout != Timeout.InfiniteTimeSpan) {
                        var rt = timeout;
                        if (rt.Ticks > 0) {
                            SpinWait spinner = default;
                            var sw = new Stopwatch();
                            sw.Start();
                            for (var spin_count = 0; ;) {
#if DEBUG && DEBUG_ES
                                ThrowException(0.05);
#endif
                                Monitor.TryEnter(o1, rt, ref t1);
#if DEBUG && DEBUG_ES
                                ThrowException(0.05);
#endif
                                if (t1) {
                                    Monitor.TryEnter(o2, ref t2);
                                }
#if DEBUG && DEBUG_ES
                                ThrowException(0.05);
#endif
                                if (t2) {
                                    Debug.Assert(t1);
                                    break;
                                }
                                {
                                    if (t1) {
                                        try {
                                        } finally {
                                            Monitor.Exit(o1);
                                            t1 = false;
                                        }
                                    }
#if DEBUG && DEBUG_ES
                                    ThrowException(0.06);
#endif
                                    spinner.SpinOnce();
                                    rt -= sw.Elapsed;
                                    if (rt.Ticks <= 0) {
                                        break;
                                    }
                                }
                            }
                        } else {
#if DEBUG && DEBUG_ES
                            ThrowException(0.05);
#endif
                            Monitor.TryEnter(o1, ref t1);
#if DEBUG && DEBUG_ES
                            ThrowException(0.05);
#endif
                            if (t1) {
                                Monitor.TryEnter(o2, ref t2);
#if DEBUG && DEBUG_ES
                                ThrowException(0.05);
#endif
                            }
                        }
                    } else {
                        DoEnter(o1, o2, ref t1, ref t2);
                    }
                } finally {
                    if (!t2 && t1) {
                        Monitor.Exit(o1);
                    }
                    lockTaken = t2;
                }
                Debug.Assert(!t2 || t1);
                return;
            }
            {
#if DEBUG
                LogDuplicateObject();
#endif
#if DEBUG && DEBUG_ES
                ThrowException(0.03);
#endif
                Monitor.TryEnter(obj1, timeout, ref lockTaken);
#if DEBUG && DEBUG_ES
                ThrowException(0.05);
#endif
            }
        }

        public static void Enter(object obj1, object obj2) {
#if DEBUG && DEBUG_ES
            ThrowException(0.02);
#endif
            if (obj1 is null) {
                throw new ArgumentNullException(nameof(obj1));
            }
            if (obj2 is null) {
                throw new ArgumentNullException(nameof(obj2));
            }
#if DEBUG && DEBUG_ES
            ThrowException(0.04);
#endif
            if (!object.ReferenceEquals(obj1, obj2)) {
                GetOrdered(obj1, obj2, out object o1, out object o2);
                Debug.Assert(null != o1);
                Debug.Assert(null != o2);
#if DEBUG && DEBUG_ES
                ThrowException(0.02);
#endif
                var t1 = false;
#if DEBUG && DEBUG_ES
                ThrowException(0.04);
#endif
                var t2 = false;
#if DEBUG && DEBUG_ES
                ThrowException(0.02);
#endif
                try {
#if DEBUG && DEBUG_ES
                    ThrowException(0.03);
#endif
                    DoEnter(o1, o2, ref t1, ref t2);
                } finally {
                    if (!t2 && t1) {
                        Monitor.Exit(o1);
                    }
                }
                return;
            }
            {
#if DEBUG
                LogDuplicateObject();
#endif
#if DEBUG && DEBUG_ES
                ThrowException(0.03);
#endif
                Monitor.Enter(obj1);
#if DEBUG && DEBUG_ES
                ThrowException(0.05);
#endif
            }
        }

        // [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static void DoEnter(object o1, object o2, ref bool t1, ref bool t2) {
            SpinWait spinner = default;
            for (var spin_count = 0; ;) {
#if DEBUG && DEBUG_ES
                ThrowException(0.05);
#endif
                Monitor.Enter(o1, ref t1);
#if DEBUG && DEBUG_ES
                ThrowException(0.05);
#endif
                Monitor.TryEnter(o2, ref t2);
#if DEBUG && DEBUG_ES
                ThrowException(0.05);
#endif
                if (t2) {
                    break;
                }
                {
                    try {
                    } finally {
                        Monitor.Exit(o1);
                        t1 = false;
                    }
#if DEBUG && DEBUG_ES
                    ThrowException(0.05);
#endif
                    spinner.SpinOnce();
                }
            }
        }

        private static void GetOrdered(object obj1, object obj2, out object o1, out object o2) {
            Debug.Assert(obj1 != obj2);
            var a = s_SyncTable.GetOrCreateValue(obj1);
            Debug.Assert(0 > a.m_Id);
            var b = s_SyncTable.GetOrCreateValue(obj2);
            Debug.Assert(0 > b.m_Id);
            Debug.Assert(a.m_Id != b.m_Id);
            if (a.m_Id <= b.m_Id) {
                o1 = obj2;
                o2 = obj1;
            } else {
                o1 = obj1;
                o2 = obj2;
            }
        }

#if DEBUG
        [ThreadStatic]
        private static Random _Random_DEGUB;

        private static Random Random_DEGUB {

            get {
                var random = _Random_DEGUB;
                if (random is null) {
                    random = new Random();
                    _Random_DEGUB = random;
                }
                return random;
            }
        }

        [Conditional("DEBUG")]
        private static void ThrowException(double p, [System.Runtime.CompilerServices.CallerLineNumber] int callerLineNumber = 0) {
            if (!(0 <= p && p <= 1.0)) {
                throw new ArgumentOutOfRangeException(nameof(p), p, "The specified probability must be between 0 and 1, inclusively.");
            }
            if (p > Random_DEGUB.NextDouble()) {
                throw new Exception($@"TEST: Oops! Ln: {callerLineNumber}.");
            }
        }
#endif

#if DEBUG
        [Conditional("TRACE")]
        private static void LogDuplicateObject() {
            Trace.WriteLine("Moniter: Duplicate object passed as argument.");
        }
#endif

        public static void Exit(object obj1, object obj2) {
            if (obj1 is null) {
                throw new ArgumentNullException(nameof(obj1));
            }
            if (obj2 is null) {
                throw new ArgumentNullException(nameof(obj2));
            }
            if (obj1 != obj2) {
                if (Monitor.IsEntered(obj1)) {
                    if (Monitor.IsEntered(obj2)) {
                        Monitor.Exit(obj1);
                        Monitor.Exit(obj2);
                        return;
                    }
                    {
                        // System.Threading.SynchronizationLockException
                        Monitor.Exit(obj2);
                    }
                }
                {
                    // System.Threading.SynchronizationLockException
                    Monitor.Exit(obj1);
                }
            }
            {
#if DEBUG
                LogDuplicateObject();
#endif
                Monitor.Exit(obj1);
            }
        }
    }
}
