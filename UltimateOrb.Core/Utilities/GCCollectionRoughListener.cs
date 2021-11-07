using System;
using System.Threading;

namespace UltimateOrb.Utilities {

    public class GCCollectionRoughListener<T> : IDisposable {

        [CLSCompliant(false)]
        protected T? _Data;

        public ref T? Data {

            get => ref _Data;
        }

        [CLSCompliant(false)]
        protected RefAction<T?>? _Action = null;

        public RefAction<T?>? Action {

            get => _Action;

            protected set => _Action = value;
        }

        private object? _DetectorLifetimeService;

        private int _Disposed;

        public GCCollectionRoughListener() {
            _DetectorLifetimeService = new GCCollectionRoughDetector() {
                Action = detector => {
                    if (0 != Volatile.Read(ref _Disposed)) {
                        return;
                    }
                    Action?.Invoke(ref Data);
                    GC.ReRegisterForFinalize(detector);
                },
            };
        }

        public void Start() {
            if (null == Interlocked.Exchange(ref _DetectorLifetimeService, null)) {
                throw new InvalidOperationException();
            }
        }

        public void Stop() {
            Dispose();
        }

        protected virtual void Dispose(bool disposing) {
            if (0 == Interlocked.Exchange(ref _Disposed, 1)) {
                if (disposing) {
                    // TODO: dispose managed state (managed objects)
                }
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~GCCollectionRoughListener() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose() {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            GC.SuppressFinalize(this);
            Dispose(disposing: true);
        }
    }
}
