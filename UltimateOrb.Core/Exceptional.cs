using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Runtime.CompilerServices;

namespace UltimateOrb {

    public struct Exceptional {

        public static Exceptional<T, Exception> FromValue<T>(T value) {
            return value;
        }

        public static Exceptional<T, TException> FromValue<T, TException>(T value) where TException : Exception {
            return value;
        }

        public static Exceptional<T, TException> FromException<T, TException>(TException exception) where TException : Exception {
            return Exceptional<T, TException>.FromException(exception);
        }
    }

    public readonly struct ExceptionT {
    }

    public readonly struct Exceptional<T, TException> where TException : Exception {
        readonly U<TException, T> _Base;

        public Exceptional(T value) {
            _Base = value;
        }

        public Exceptional(TException exception, ExceptionT _ = default) {
            _Base = (exception ?? throw new ArgumentNullException(nameof(exception)));
        }

        public bool HasValue {

            get => 2 == _Base.Case;
        }

        public bool HasException {

            get => 1 == _Base.Case;
        }

        public T? ValueOrDefault {

            get => _Base.Case2OrDefault;
        }

        public T Value {

            get => HasException ? throw _Base.Case1OrDefault! : _Base.Case2;
        }

        public TException? ExceptionOrDefault {

            get => _Base.Case1OrDefault;
        }

        public TException Exception {

            get => _Base.Case1;
        }

        public void ThrowIfException() {
            if (HasException) {
                throw _Base.Case1OrDefault!;
            }
        }

        public ValueTask<T> AsValueTask() {
            if (HasException) {
                return ValueTask.FromException<T>(_Base.Case1OrDefault!);
            }
            return ValueTask.FromResult(_Base.Case2);
        }

        public Task<T> AsTask() {
            if (HasException) {
                return Task.FromException<T>(_Base.Case1OrDefault!);
            }
            return Task.FromResult(_Base.Case2);
        }

        public static Exceptional<T, TException> FromException(TException exception) {
            return new Exceptional<T, TException>(exception, default);
        }

        public static implicit operator TException(Exceptional<T, TException> exceptional) {
            return exceptional.Exception;
        }

        public static implicit operator Exceptional<T, Exception>(Exceptional<T, TException> exceptional) {
            return Unsafe.As<Exceptional<T, TException>, Exceptional<T, Exception>>(ref exceptional);
        }

        public static explicit operator Exceptional<T, TException>(Exceptional<T, Exception> exceptional) {
            if (exceptional.HasException) {
                _ = (TException)exceptional.ExceptionOrDefault!;
            }
            return Unsafe.As<Exceptional<T, Exception>, Exceptional<T, TException>>(ref exceptional);
        }

        public static implicit operator Exceptional<T, TException>(TException exception) {
            return new Exceptional<T, TException>(exception);
        }

        public static implicit operator Exceptional<T, TException>(T value) {
            return new Exceptional<T, TException>(value);
        }

        public static explicit operator T(Exceptional<T, TException> value) {
            return value.Value;
        }
    }
}
