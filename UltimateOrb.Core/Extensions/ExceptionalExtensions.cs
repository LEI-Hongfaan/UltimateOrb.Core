using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace UltimateOrb {

    [Serializable]
    [DebuggerDisplay("{InnerException}")]
    public class UException<TException1, TException2>
        : AggregateException
        where TException1 : Exception
        where TException2 : Exception {

        public UException(TException1 exception) : base(exception.Message, exception) {
        }

        public UException(TException2 exception) : base(exception.Message, exception) {
        }

        protected UException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }

    public static partial class ExceptionalExtensions {

        public static Exceptional<T, Exception> ToExceptional<T>(this T value) {
            return value;
        }

        public static Exceptional<T, TException> ToExceptional<T, TException>(this T value) where TException : Exception {
            return value;
        }

        public static Exceptional<T, TException> ToExceptional<T, TException>(TException exception) where TException : Exception {
            return Exceptional<T, TException>.FromException(exception);
        }
    }
}
