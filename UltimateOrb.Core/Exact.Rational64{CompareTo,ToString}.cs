using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace UltimateOrb.Mathematics.Exact {

    public readonly partial struct Rational64 : IComparable, IFormattable {

        public int CompareTo(object other) {
            return other is not Rational64 q ? CompareTo__ThrowArgumentException() : CompareTo(q);
        }

        [DoesNotReturn]
        [MethodImplAttribute(MethodImplOptions.NoInlining)]
        private static int CompareTo__ThrowArgumentException() {
            const string msg = @"Object must be of type Rational64.";
            throw new ArgumentException(msg, @"other");
        }

        public override string ToString() {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.SignedNumerator);
            var t = this.Denominator;
            if (1 != t) {
                var i = sb.Length - 1;
                for (; 0 <= i; --i) {
                    var c = sb[i];
                    if (char.IsNumber(c)) {
                        break;
                    }
                }
                sb.Insert(i + 1, '/');
                sb.Insert(i + 2, t);
            }
            return sb.ToString();
        }

        public string ToString(IFormatProvider formatProvider) {
            return this.ToString(null, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            Contract.Ensures(Contract.Result<string>() != null);
            var sb = new StringBuilder(33);
            sb.Append(this.SignedNumerator.ToString(format, formatProvider));
            var t = this.Denominator;
            if (1 != t) {
                var i = sb.Length - 1;
                for (; 0 <= i; --i) {
                    var c = sb[i];
                    if (char.IsNumber(c)) {
                        break;
                    }
                }
                sb.Insert(i + 1, '/');
                sb.Insert(i + 2, t.ToString(format, formatProvider));
            }
            return sb.ToString();
        }
    }
}
