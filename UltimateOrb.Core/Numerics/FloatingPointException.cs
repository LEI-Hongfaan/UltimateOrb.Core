using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace UltimateOrb.Numerics {

    interface IFloatingPointException {

        public double? OffendingNumber { get; }
    }

    partial class SR {

        internal const string Arg_FloatingPointInvalidOperationException = "Specified argument is invalid for the floating-point operation.";
    }

    [Serializable]
    public class FloatingPointInvalidOperationException : ArgumentOutOfRangeException, IFloatingPointException {

        private readonly double? _offendingNumber;

        const int _hresult = HResults.COR_E_ARGUMENTOUTOFRANGE;

        public FloatingPointInvalidOperationException()
            : base(null, SR.Arg_FloatingPointInvalidOperationException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        public FloatingPointInvalidOperationException(double offendingNumber)
            : base(null, SR.Arg_FloatingPointInvalidOperationException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointInvalidOperationException(string? message)
            : base(null, message ?? SR.Arg_FloatingPointInvalidOperationException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        public FloatingPointInvalidOperationException(string? message, double offendingNumber)
            : base(null, offendingNumber, message ?? SR.Arg_FloatingPointInvalidOperationException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointInvalidOperationException(string? message, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointInvalidOperationException, innerException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_acturalValue")]
        static extern ref object? Get_actualValue(ArgumentOutOfRangeException @this);

        public FloatingPointInvalidOperationException(string? message, double offendingNumber, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointInvalidOperationException, innerException) {
            Get_actualValue(this) = offendingNumber;
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FloatingPointInvalidOperationException(SerializationInfo info, StreamingContext context) : base(info, context) {
            _offendingNumber = (double?)info.GetValue("OffendingNumber", typeof(double));
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("OffendingNumber", _offendingNumber, typeof(double));
        }

        public double? OffendingNumber => _offendingNumber;
    }

    [Serializable]
    public class FloatingPointDivideByZeroException : DivideByZeroException, IFloatingPointException {

        private readonly double? _offendingNumber;

        public FloatingPointDivideByZeroException() : this(double.NegativeInfinity) { }
        public FloatingPointDivideByZeroException(double offendingNumber) : base() { _offendingNumber = offendingNumber; }
        public FloatingPointDivideByZeroException(string message) : this(message, double.NegativeInfinity) { }
        public FloatingPointDivideByZeroException(string message, double offendingNumber) : base(message) { _offendingNumber = offendingNumber; }
        public FloatingPointDivideByZeroException(string message, Exception inner) : this(message, double.NegativeInfinity, inner) { }
        public FloatingPointDivideByZeroException(string message, double offendingNumber, Exception inner) : base(message, inner) { _offendingNumber = offendingNumber; }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FloatingPointDivideByZeroException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) {
            _offendingNumber = (double?)info.GetValue("m_OffendingNumber", typeof(double));
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("m_OffendingNumber", _offendingNumber, typeof(double));
        }

        public new double? OffendingNumber { get => _offendingNumber; }
    }

    [Serializable]
    public class FloatingPointOverflowException : NotFiniteNumberException, IFloatingPointException {

        private readonly double? _offendingNumber;

        public FloatingPointOverflowException() : base(double.NaN) { }
        public FloatingPointOverflowException(double offendingNumber) : base(offendingNumber) { _offendingNumber = offendingNumber; }
        public FloatingPointOverflowException(string message) : base(message, double.NaN) { }
        public FloatingPointOverflowException(string message, double offendingNumber) : base(message, offendingNumber) { _offendingNumber = offendingNumber; }
        public FloatingPointOverflowException(string message, Exception inner) : base(message, double.NaN, inner) { }
        public FloatingPointOverflowException(string message, double offendingNumber, Exception inner) : base(message, offendingNumber, inner) { _offendingNumber = offendingNumber; }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FloatingPointOverflowException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) {
            _offendingNumber = (double?)info.GetValue("m_OffendingNumber", typeof(double));
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("m_OffendingNumber", _offendingNumber, typeof(double));
        }

        public new double? OffendingNumber { get => _offendingNumber; }
    }

    partial class SR {

        internal const string Arg_FloatingPointUnderflowException = "The operation resulted in an underflowed floating-point value.";
    }

    [Serializable]
    public class FloatingPointUnderflowException : ArithmeticException, IFloatingPointException {

        private readonly double? _offendingNumber;

        const int _hresult = HResults.COR_E_ARITHMETIC;

        public FloatingPointUnderflowException()
            : base(SR.Arg_FloatingPointUnderflowException) {
            _offendingNumber = 0;
            HResult = _hresult;
        }

        public FloatingPointUnderflowException(double offendingNumber)
            : base(SR.Arg_FloatingPointUnderflowException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointUnderflowException(string? message)
            : base(message ?? SR.Arg_FloatingPointUnderflowException) {
            _offendingNumber = 0;
            HResult = _hresult;
        }

        public FloatingPointUnderflowException(string? message, double offendingNumber)
            : base(message ?? SR.Arg_FloatingPointUnderflowException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointUnderflowException(string? message, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointUnderflowException, innerException) {
            HResult = _hresult;
        }

        public FloatingPointUnderflowException(string? message, double offendingNumber, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointUnderflowException, innerException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FloatingPointUnderflowException(SerializationInfo info, StreamingContext context) : base(info, context) {
            _offendingNumber = (double?)info.GetValue("OffendingNumber", typeof(double));
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("OffendingNumber", _offendingNumber, typeof(double));
        }

        public double? OffendingNumber => _offendingNumber;
    }

    partial class SR {

        internal const string Arg_FloatingPointInexactOperationException = "The operation produced an inexact floating-point result.";
    }

    [Serializable]
    public class FloatingPointInexactOperationException : ArithmeticException, IFloatingPointException {

        const int _hresult = HResults.COR_E_ARITHMETIC;

        private readonly double? _offendingNumber;

        public FloatingPointInexactOperationException()
            : base(SR.Arg_FloatingPointInexactOperationException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        public FloatingPointInexactOperationException(double offendingNumber)
            : base(SR.Arg_FloatingPointInexactOperationException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointInexactOperationException(string? message)
            : base(message ?? SR.Arg_FloatingPointInexactOperationException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        public FloatingPointInexactOperationException(string? message, double offendingNumber)
            : base(message ?? SR.Arg_FloatingPointInexactOperationException) {
            _offendingNumber = offendingNumber;
            HResult = _hresult;
        }

        public FloatingPointInexactOperationException(string? message, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointInexactOperationException, innerException) {
            _offendingNumber = null;
            HResult = _hresult;
        }

        public FloatingPointInexactOperationException(string? message, double offendingNumber, Exception? innerException)
            : base(message ?? SR.Arg_FloatingPointInexactOperationException, innerException) {
            _offendingNumber = offendingNumber;
            HResult = HResults.COR_E_ARGUMENTOUTOFRANGE;
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected FloatingPointInexactOperationException(SerializationInfo info, StreamingContext context) : base(info, context) {
            _offendingNumber = (double?)info.GetValue("OffendingNumber", typeof(double));
        }

        [Obsolete(Obsoletions.LegacyFormatterImplMessage, DiagnosticId = Obsoletions.LegacyFormatterImplDiagId, UrlFormat = Obsoletions.SharedUrlFormat)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context) {
            base.GetObjectData(info, context);
            info.AddValue("OffendingNumber", _offendingNumber, typeof(double));
        }

        public double? OffendingNumber => _offendingNumber;
    }
}