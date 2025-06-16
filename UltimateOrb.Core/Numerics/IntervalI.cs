using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UltimateOrb.Utilities;

namespace UltimateOrb.Numerics {

    public static partial class GenericMath {

        internal static partial class TypeTraits<T>
            where T :
                IBinaryInteger<T> {

            public static readonly bool IsSigned = GenericMath.IsSigned<T>();

            public static readonly T MinValue = GenericMath.GetMinValue<T>();

            public static readonly T MaxValue = ~MinValue;

            public static readonly T HighestBitSet = GenericMath.GetHighestBitSet<T>();
        }

        private static bool IsSigned<T>()
            where T :
                IBinaryInteger<T> {
            return ~T.Zero != T.Zero && T.IsNegative(~T.Zero);
        }

        private static T GetMinValueInternal<T>()
            where T :
                IBinaryInteger<T> {
            if (IsSigned<T>()) {
                return GetHighestBitSet<T>();
            } else {
                return default!;
            }
        }

        private static T GetMinValue<T>()
           where T :
               IBinaryInteger<T> {
            var minValue = GetMinValueInternal<T>();
            // Check that both unchecked(--r) and unchecked(++r) are greater than minValue
            {
                bool s = false;
                T r = minValue;
                try {
                    unchecked {
                        --r;
                    }
                } catch (ArithmeticException) {
                    s = true; // overflow occurred
                }
                if (!s && r <= minValue) {
                    throw new NotSupportedException();
                }
            }
            {
                bool s = false;
                T r = minValue;
                try {
                    unchecked {
                        ++r;
                    }
                } catch (ArithmeticException) {
                    s = true; // overflow occurred
                }
                if (!s && r <= minValue) {
                    throw new NotSupportedException();
                }
            }
            return minValue;
        }

        private static T GetHighestBitSet<T>()
            where T :
                IBinaryInteger<T> {
            if (IsSigned<T>()) {
                var a = T.AllBitsSet;
                var b = a >>> 1;
                var v = a ^ b;
                if (v >= a) {
                    throw new NotSupportedException();
                }
                return v;
            } else {
                var a = T.AllBitsSet;
                var b = a >>> 1;
                var v = a ^ b;
                return v;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Result, bool Overflow) AddUnsignedWithOverflow<T>(T first, T second)
            where T :
                IBinaryInteger<T> {
            if (TypeTraits<T>.IsSigned) {
                var r = unchecked(first + second);
                return (r, (r ^ TypeTraits<T>.MinValue) < (first ^ TypeTraits<T>.MinValue));
            } else {
                var r = unchecked(first + second);
                return (r, r < first);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Result, bool Overflow) AddSignedWithOverflow<T>(T first, T second)
            where T :
                IBinaryInteger<T> {
            if (TypeTraits<T>.IsSigned) {
                var r = unchecked(first + second);
                return (r, !T.IsZero((first ^ ~second) & (first ^ r) & TypeTraits<T>.HighestBitSet));
            } else {
                var r = unchecked(first + second);
                return (r, !T.IsZero((first ^ ~second) & (first ^ r) & TypeTraits<T>.HighestBitSet));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Result, bool Overflow) SubtractUnsignedWithOverflow<T>(T first, T second)
            where T :
                IBinaryInteger<T> {
            if (TypeTraits<T>.IsSigned) {
                var r = unchecked(first - second);
                return (r, (first ^ TypeTraits<T>.MinValue) < (second ^ TypeTraits<T>.MinValue));
            } else {
                var r = unchecked(first - second);
                return (r, first < second);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Result, bool Overflow) SubtractSignedWithOverflow<T>(T first, T second)
            where T :
                IBinaryInteger<T> {
            if (TypeTraits<T>.IsSigned) {
                var r = unchecked(first - second);
                return (r, (first ^ TypeTraits<T>.MinValue) < (r ^ TypeTraits<T>.MinValue));
            } else {
                var r = unchecked(first - second);
                return (r, first < r);
            }
        }
    }

    public readonly partial struct IntervalI<T>
        where T :
            IBinaryInteger<T> {

        readonly T _LowerBoundExclusive;

        readonly T _UpperBound;

        public readonly T LowerBound {

            get {
                var value = _LowerBoundExclusive;
                unchecked {
                    ++value;
                }
                return value;
            }
        }

        public IntervalI(T lowerBound, T upperBound) {
            // ...
        }

        internal IntervalI(T lowerBoundExclusive, T upperBound, NoCheck ignored) {
            // ...
        }

        public static IntervalI<T> operator +(IntervalI<T> value) {
            return value;
        }

        public static IntervalI<T> operator -(IntervalI<T> value) {
            return new IntervalI<T>(~value._UpperBound, ~value._LowerBoundExclusive, default(NoCheck));
        }

        public static IntervalI<T> operator +(IntervalI<T> first, IntervalI<T> second) {
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator +(IntervalI<T> first, T second) {
            if (IsEmpty(first)) {
                return default;
            }
            return new IntervalI<T>(checked(first._LowerBoundExclusive + second), checked(first._UpperBound + second), default(NoCheck));
        }

        public static IntervalI<T> operator +(T first, IntervalI<T> second) {
            if (IsEmpty(second)) {
                return default;
            }
            return new IntervalI<T>(checked(first + second._LowerBoundExclusive), checked(first + second._UpperBound), default(NoCheck));
        }

        public static IntervalI<T> operator -(IntervalI<T> first, IntervalI<T> second) {
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator -(IntervalI<T> first, T second) {
            if (IsEmpty(first)) {
                return default;
            }
            return new IntervalI<T>(checked(first._LowerBoundExclusive - second), checked(first._UpperBound - second), default(NoCheck));
        }

        public static IntervalI<T> operator -(T first, IntervalI<T> second) {
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator *(IntervalI<T> first, IntervalI<T> second) {
            // ...
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator *(IntervalI<T> first, T second) {
            // ...
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator *(T first, IntervalI<T> second) {
            // ...
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator /(IntervalI<T> first, IntervalI<T> second) {
            // throws DivisionByZeroException when second contains 0
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator /(IntervalI<T> first, T second) {
            // ...
            throw new NotImplementedException();
        }

        public static IntervalI<T> operator /(T first, IntervalI<T> second) {
            // allows 0 to be in second
            throw new NotImplementedException();
        }

        public static bool IsEmpty(IntervalI<T> value) {
            return value._LowerBoundExclusive == value._UpperBound;
        }
    }
}
