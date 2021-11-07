using System;

using System.Diagnostics;
using System.Runtime.CompilerServices;
using UltimateOrb.Mathematics;

using size_t = System.Int32;

using mp_limb_t = System.UInt64;
using mp_limb_signed_t = System.Int64;
using mp_size_t = System.Int32;

using mp_bitcnt_t = System.Int64;

using static UltimateOrb.Utilities.BooleanIntegerModule;
using System.Buffers;

namespace UltimateOrb.Core.Tests.A {
    using mp_ptr = ArrayIterator<mp_limb_t>;
    using mp_srcptr = ArrayReadOnlyIterator<mp_limb_t>;

    public readonly struct ArrayIterator<T> : IEquatable<ArrayIterator<T>> {

        internal readonly T[] _Array;

        internal readonly uint _Index;

        public ref T Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._Array[this._Index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T op_PointerDereference(ArrayIterator<T> value) {
            return ref value._Array[value._Index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ArrayIterator(T[] array, uint index) {
            this._Array = array;
            this._Index = index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(ArrayIterator<T> @base, int offset) {
            return new ArrayIterator<T>(@base._Array, unchecked(@base._Index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(int offset, ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._Array, unchecked(@base._Index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator ++(ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._Array, unchecked(1 + @base._Index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator --(ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._Array, unchecked(@base._Index) - 1);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(ArrayIterator<T> @base) {
            return @base;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator -(ArrayIterator<T> @base, int offset) {
            return new ArrayIterator<T>(@base._Array, unchecked(@base._Index - (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int operator -(ArrayIterator<T> first, ArrayIterator<T> second) {
            Debug.Assert(first._Array == second._Array);
            return unchecked((int)(first._Index - second._Index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Array == second._Array && first._Index < second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Array == second._Array && first._Index <= second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Array == second._Array && first._Index > second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Array == second._Array && first._Index >= second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Array == second._Array && first._Index == second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, long second) {
            if (0 == second) {
                return null == first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, long second) {
            if (0 == second) {
                return null != first._Array;
            }
            return false;
        }


        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long first, ArrayIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long first, ArrayIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, ulong second) {
            if (0 == second) {
                return null == first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, ulong second) {
            if (0 == second) {
                return null != first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, ArrayIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, ArrayIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._Index != second._Index || first._Array != second._Array;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ArrayIterator<T> other) {
            return this._Array == other._Array && this._Index == other._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            return unchecked((int)this._Index ^ (null == this._Array ? 0 : this._Array.GetHashCode()));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) {
            if (obj is ArrayIterator<T> ptr) {
                return this.Equals(ptr);
            }
            return base.Equals(obj);
        }

        public ref T this[int offset] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._Array[unchecked(this._Index + (uint)offset)];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayIterator<T>(T[] array) {
            return new ArrayIterator<T>(array, 0);
        }
    }

    public readonly struct ArrayReadOnlyIterator<T> : IEquatable<ArrayReadOnlyIterator<T>> {

        internal readonly T[] _Array;

        internal readonly uint _Index;

        public ref T Current {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._Array[this._Index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T op_PointerDereference(ArrayReadOnlyIterator<T> value) {
            return ref value._Array[value._Index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ArrayReadOnlyIterator(T[] array, uint index) {
            this._Array = array;
            this._Index = index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator +(ArrayReadOnlyIterator<T> @base, int offset) {
            return new ArrayReadOnlyIterator<T>(@base._Array, unchecked(@base._Index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator +(int offset, ArrayReadOnlyIterator<T> @base) {
            return new ArrayReadOnlyIterator<T>(@base._Array, unchecked(@base._Index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator ++(ArrayReadOnlyIterator<T> @base) {
            return new ArrayReadOnlyIterator<T>(@base._Array, unchecked(1 + @base._Index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator --(ArrayReadOnlyIterator<T> @base) {
            return new ArrayReadOnlyIterator<T>(@base._Array, unchecked(@base._Index) - 1);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator +(ArrayReadOnlyIterator<T> @base) {
            return @base;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayReadOnlyIterator<T> operator -(ArrayReadOnlyIterator<T> @base, int offset) {
            return new ArrayReadOnlyIterator<T>(@base._Array, unchecked(@base._Index - (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int operator -(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            Debug.Assert(first._Array == second._Array);
            return unchecked((int)(first._Index - second._Index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Array == second._Array && first._Index < second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Array == second._Array && first._Index <= second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Array == second._Array && first._Index > second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Array == second._Array && first._Index >= second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Array == second._Array && first._Index == second._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayReadOnlyIterator<T> first, long second) {
            if (0 == second) {
                return null == first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayReadOnlyIterator<T> first, long second) {
            if (0 == second) {
                return null != first._Array;
            }
            return false;
        }


        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long first, ArrayReadOnlyIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long first, ArrayReadOnlyIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayReadOnlyIterator<T> first, ulong second) {
            if (0 == second) {
                return null == first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayReadOnlyIterator<T> first, ulong second) {
            if (0 == second) {
                return null != first._Array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, ArrayReadOnlyIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, ArrayReadOnlyIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayReadOnlyIterator<T> first, ArrayReadOnlyIterator<T> second) {
            return first._Index != second._Index || first._Array != second._Array;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ArrayReadOnlyIterator<T> other) {
            return this._Array == other._Array && this._Index == other._Index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            return unchecked((int)this._Index ^ (null == this._Array ? 0 : this._Array.GetHashCode()));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) {
            if (obj is ArrayReadOnlyIterator<T> ptr) {
                return this.Equals(ptr);
            }
            return base.Equals(obj);
        }

        public ref readonly T this[int offset] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._Array[unchecked(this._Index + (uint)offset)];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayReadOnlyIterator<T>(T[] array) {
            return new ArrayReadOnlyIterator<T>(array, 0);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayReadOnlyIterator<T>(ArrayIterator<T> value) {
            return new ArrayReadOnlyIterator<T>(value._Array, value._Index);
        }
    }



    public static partial class sdfadsfas {

        static bool LIKELY(bool cond) {
            return cond;
        }
        static bool UNLIKELY(bool cond) {
            return cond;
        }

        static uint CBool(bool value) {
            unchecked {
                return (uint)value.AsIntegerUnsafe();
            }
        }

        static T2 Comma<T1, T2>(T1 arg1, T2 arg2) {
            return arg2;
        }

        static ref T2 Comma<T1, T2>(T1 arg1, ref T2 arg2) {
            return ref arg2;
        }

        static uint NEG(uint value) {
            return unchecked((uint)-(int)value);
        }

        static ulong NEG(ulong value) {
            return unchecked((ulong)-(long)value);
        }

        #region Double Precision Arithmetic
        public static void umul_ppmm(out mp_limb_t w1, out mp_limb_t w0, mp_limb_t u, mp_limb_t v) {
            w0 = DoubleArithmetic.BigMul(u, v, out w1);
        }
        #endregion

        #region Assertions
        public static void ASSERT(bool condition) {
            Debug.Assert(condition);
        }

        public static void ASSERT_CARRY(mp_limb_t expr) {
            Debug.Assert(0 != expr);
        }

        public static void ASSERT_NOCARRY(mp_limb_t expr) {
            Debug.Assert(0 == expr);
        }

        public static bool MPN_OVERLAP_P(mp_srcptr xp, mp_size_t xsize, mp_srcptr yp, mp_size_t ysize) {
            return ((xp) + (xsize) > (yp) && (yp) + (ysize) > (xp));
        }

        public static bool MPN_SAME_OR_INCR2_P(mp_srcptr dst, mp_size_t dsize, mp_srcptr src, mp_size_t ssize) {
            return ((dst) <= (src) || !MPN_OVERLAP_P(dst, dsize, src, ssize));
        }

        public static bool MPN_SAME_OR_INCR_P(mp_srcptr dst, mp_srcptr src, mp_size_t size) {
            return MPN_SAME_OR_INCR2_P(dst, size, src, size);
        }

        public static bool MPN_SAME_OR_DECR2_P(mp_srcptr dst, mp_size_t dsize, mp_srcptr src, mp_size_t ssize) {
            return ((dst) >= (src) || !MPN_OVERLAP_P(dst, dsize, src, ssize));
        }

        public static bool MPN_SAME_OR_DECR_P(mp_srcptr dst, mp_srcptr src, mp_size_t size) {
            return MPN_SAME_OR_DECR2_P(dst, size, src, size);
        }

        public static bool MPN_SAME_OR_SEPARATE_P(mp_srcptr xp, mp_srcptr yp, mp_size_t size) {
            return MPN_SAME_OR_SEPARATE2_P(xp, size, yp, size);
        }

        public static bool MPN_SAME_OR_SEPARATE2_P(mp_srcptr xp, mp_size_t xsize, mp_srcptr yp, mp_size_t ysize) {
            return ((xp) == (yp) || !MPN_OVERLAP_P(xp, xsize, yp, ysize));
        }
        #endregion

        public static mp_limb_t mpn_mul_1(mp_ptr rp, mp_srcptr up, mp_size_t n, mp_limb_t vl) {
            mp_limb_t ul, cl, hpl, lpl;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_INCR_P(rp, up, n));

            cl = 0;
            do {
                ul = up++.Current;
                umul_ppmm(out hpl, out lpl, ul, vl);

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }
        #region Compiler Services
        static (string, string)[] sdsa = new[] {
            ("TMP_SDECL;","sss"),
            ("MAX", "Math.Max" ),
        };
        #endregion
        const mp_size_t MP_SIZE_T_MAX = mp_size_t.MaxValue;


        #region Allocations



        static T[] salloc<T>(mp_size_t size) {
            return new T[size];
        }


        ref struct mp_tmp_marker_t {
            internal mp_limb_t[] Current;

            public void Dispose() {

            }
        }


        static void TMP_SMARK() {
        }
        static void TMP_MARK(out mp_tmp_marker_t __tmp_marker) {
            __tmp_marker = default;
        }


        static mp_limb_t[] TMP_SALLOC_LIMBS(mp_size_t n) {
            return new mp_limb_t[n];
        }

        static mp_limb_t[] TMP_BALLOC_LIMBS(ref mp_tmp_marker_t __tmp_marker, mp_size_t n) {
            return __tmp_marker.Current = ArrayPool<mp_limb_t>.Shared.Rent(n);
        }

        static mp_limb_t[] TMP_ALLOC_LIMBS(ref mp_tmp_marker_t __tmp_marker, mp_size_t n) {
            return (LIKELY((n) <= 0x7f00) ? TMP_SALLOC_LIMBS(n) : TMP_BALLOC_LIMBS(ref __tmp_marker, n));
        }
        static void TMP_SFREE() {

        }
        static void TMP_FREE(ref mp_tmp_marker_t __tmp_marker) {
            var t = __tmp_marker.Current;
            if (null != t) {
                ArrayPool<mp_limb_t>.Shared.Return(t);
            }
        }
        #endregion
        #region Thresholds
        static bool ABOVE_THRESHOLD(mp_size_t size, mp_size_t thresh) {
            return ((thresh) == 0 || ((thresh) != MP_SIZE_T_MAX && (size) >= (thresh)));
        }
        static bool BELOW_THRESHOLD(mp_size_t size, mp_size_t thresh) {
            return !ABOVE_THRESHOLD(size, thresh);
        }


        const mp_size_t MUL_TOOM22_THRESHOLD = 30;
        const mp_size_t MUL_TOOM33_THRESHOLD = 100;
        const mp_size_t MUL_TOOM44_THRESHOLD = 300;
        const mp_size_t MUL_TOOM6H_THRESHOLD = 350;
        const mp_size_t SQR_TOOM6_THRESHOLD = MUL_TOOM6H_THRESHOLD;
        const mp_size_t MUL_TOOM8H_THRESHOLD = 450;
        const mp_size_t SQR_TOOM8_THRESHOLD = MUL_TOOM8H_THRESHOLD;
        const mp_size_t MUL_TOOM32_TO_TOOM43_THRESHOLD = 100;
        const mp_size_t MUL_TOOM32_TO_TOOM53_THRESHOLD = 110;
        const mp_size_t MUL_TOOM42_TO_TOOM53_THRESHOLD = 100;
        const mp_size_t MUL_TOOM42_TO_TOOM63_THRESHOLD = 110;
        const mp_size_t MUL_TOOM43_TO_TOOM54_THRESHOLD = 150;

        /* MUL_TOOM22_THRESHOLD_LIMIT is the maximum for MUL_TOOM22_THRESHOLD.  In a
           normal build MUL_TOOM22_THRESHOLD is a constant and we use that.  In a fat
           binary or tune program build MUL_TOOM22_THRESHOLD is a variable and a
           separate hard limit will have been defined.  Similarly for TOOM3.  */
        const mp_size_t MUL_TOOM22_THRESHOLD_LIMIT = MUL_TOOM22_THRESHOLD;
        const mp_size_t MUL_TOOM33_THRESHOLD_LIMIT = MUL_TOOM33_THRESHOLD;
        const mp_size_t MULLO_BASECASE_THRESHOLD_LIMIT = MULLO_BASECASE_THRESHOLD;
        const mp_size_t SQRLO_BASECASE_THRESHOLD_LIMIT = SQRLO_BASECASE_THRESHOLD;
        const mp_size_t SQRLO_DC_THRESHOLD_LIMIT = SQRLO_DC_THRESHOLD;


        /* SQR_BASECASE_THRESHOLD is where mpn_sqr_basecase should take over from
           mpn_mul_basecase.  Default is to use mpn_sqr_basecase from 0.  (Note that we
           certainly always want it if there's a native assembler mpn_sqr_basecase.)

           If it turns out that mpn_toom2_sqr becomes faster than mpn_mul_basecase
           before mpn_sqr_basecase does, then SQR_BASECASE_THRESHOLD is the toom2
           threshold and SQR_TOOM2_THRESHOLD is 0.  This oddity arises more or less
           because SQR_TOOM2_THRESHOLD represents the size up to which mpn_sqr_basecase
           should be used, and that may be never.  */

        const mp_size_t SQR_BASECASE_THRESHOLD = 0  /* never use mpn_mul_basecase */;
        const mp_size_t SQR_TOOM2_THRESHOLD = 50;
        const mp_size_t SQR_TOOM3_THRESHOLD = 120;
        const mp_size_t SQR_TOOM4_THRESHOLD = 400;

        /* See comments above about MUL_TOOM33_THRESHOLD_LIMIT.  */
        const mp_size_t SQR_TOOM3_THRESHOLD_LIMIT = SQR_TOOM3_THRESHOLD;
        const mp_size_t MULMID_TOOM42_THRESHOLD = MUL_TOOM22_THRESHOLD;
        const mp_size_t MULLO_BASECASE_THRESHOLD = 0  /* never use mpn_mul_basecase */;
        const mp_size_t MULLO_DC_THRESHOLD = (2 * MUL_TOOM22_THRESHOLD);
        const mp_size_t MULLO_MUL_N_THRESHOLD = (2 * MUL_FFT_THRESHOLD);
        const mp_size_t SQRLO_BASECASE_THRESHOLD = 0  /* never use mpn_sqr_basecase */;
        const mp_size_t SQRLO_DC_THRESHOLD = (MULLO_DC_THRESHOLD);
        const mp_size_t SQRLO_SQR_THRESHOLD = (MULLO_MUL_N_THRESHOLD);
        const mp_size_t DC_DIV_QR_THRESHOLD = (2 * MUL_TOOM22_THRESHOLD);
        const mp_size_t DC_DIVAPPR_Q_THRESHOLD = 200;
        const mp_size_t DC_BDIV_QR_THRESHOLD = (2 * MUL_TOOM22_THRESHOLD);
        const mp_size_t DC_BDIV_Q_THRESHOLD = 180;
        const mp_size_t DIVEXACT_JEB_THRESHOLD = 25;
        const mp_size_t INV_MULMOD_BNM1_THRESHOLD = (4 * MULMOD_BNM1_THRESHOLD);
        const mp_size_t INV_APPR_THRESHOLD = INV_NEWTON_THRESHOLD;
        const mp_size_t INV_NEWTON_THRESHOLD = 200;
        const mp_size_t BINV_NEWTON_THRESHOLD = 300;
        const mp_size_t MU_DIVAPPR_Q_THRESHOLD = 2000;
        const mp_size_t MU_DIV_QR_THRESHOLD = 2000;
        const mp_size_t MUPI_DIV_QR_THRESHOLD = 200;
        const mp_size_t MU_BDIV_Q_THRESHOLD = 2000;
        const mp_size_t MU_BDIV_QR_THRESHOLD = 2000;
        const mp_size_t MULMOD_BNM1_THRESHOLD = 16;
        const mp_size_t SQRMOD_BNM1_THRESHOLD = 16;
        const mp_size_t MUL_TO_MULMOD_BNM1_FOR_2NXN_THRESHOLD = (INV_MULMOD_BNM1_THRESHOLD / 2);

        const mp_size_t REDC_1_TO_REDC_N_THRESHOLD = 100;




        /* First k to use for an FFT modF multiply.  A modF FFT is an order
           log(2^k)/log(2^(k-1)) algorithm, so k=3 is merely 1.5 like karatsuba,
           whereas k=4 is 1.33 which is faster than toom3 at 1.485.    */
        const mp_size_t FFT_FIRST_K = 4;

        /* Threshold at which FFT should be used to do a modF NxN -> N multiply. */
        const mp_size_t MUL_FFT_MODF_THRESHOLD = (MUL_TOOM33_THRESHOLD * 3);
        const mp_size_t SQR_FFT_MODF_THRESHOLD = (SQR_TOOM3_THRESHOLD * 3);


        /* Threshold at which FFT should be used to do an NxN -> 2N multiply.  This
           will be a size where FFT is using k=7 or k=8, since an FFT-k used for an
           NxN->2N multiply and not recursing into itself is an order
           log(2^k)/log(2^(k-2)) algorithm, so it'll be at least k=7 at 1.39 which
           is the first better than toom3.  */
        const mp_size_t MUL_FFT_THRESHOLD = (MUL_FFT_MODF_THRESHOLD * 10);
        const mp_size_t SQR_FFT_THRESHOLD = (SQR_FFT_MODF_THRESHOLD * 10);

        /* Table of thresholds for successive modF FFT "k"s.  The first entry is
           where FFT_FIRST_K+1 should be used, the second FFT_FIRST_K+2,
           etc.  See mpn_fft_best_k(). */
        static readonly mp_size_t[] MUL_FFT_TABLE = new mp_size_t[]
          { MUL_TOOM33_THRESHOLD* 4,   /* k=5 */				
    MUL_TOOM33_THRESHOLD* 8,   /* k=6 */				
    MUL_TOOM33_THRESHOLD* 16,  /* k=7 */				
    MUL_TOOM33_THRESHOLD* 32,  /* k=8 */				
    MUL_TOOM33_THRESHOLD* 96,  /* k=9 */				
    MUL_TOOM33_THRESHOLD* 288, /* k=10 */				
    0 };
        static readonly mp_size_t[] SQR_FFT_TABLE = new mp_size_t[]
  { SQR_TOOM3_THRESHOLD* 4,   /* k=5 */				
    SQR_TOOM3_THRESHOLD* 8,   /* k=6 */				
    SQR_TOOM3_THRESHOLD* 16,  /* k=7 */				
    SQR_TOOM3_THRESHOLD* 32,  /* k=8 */				
    SQR_TOOM3_THRESHOLD* 96,  /* k=9 */				
    SQR_TOOM3_THRESHOLD* 288, /* k=10 */				
    0 };
        /*
        struct fft_table_nk {
            gmp_uint_least32_t n:27;
            gmp_uint_least32_t k:5;
        };
        */

        const mp_size_t MPN_FFT_TABLE_SIZE = 16;


        // const mp_size_t DC_DIV_QR_THRESHOLD = (3 * MUL_TOOM22_THRESHOLD);
        const mp_size_t GET_STR_DC_THRESHOLD = 18;
        const mp_size_t GET_STR_PRECOMPUTE_THRESHOLD = 35;
        const mp_size_t SET_STR_DC_THRESHOLD = 750;
        const mp_size_t SET_STR_PRECOMPUTE_THRESHOLD = 2000;
        const mp_size_t FAC_ODD_THRESHOLD = 35;
        const mp_size_t FAC_DSC_THRESHOLD = 400;
        #endregion
        #region Digit Constants
        const int GMP_LIMB_BITS = 64;

        const int __GMP_BITS_PER_MP_LIMB = GMP_LIMB_BITS;
        const mp_size_t SIZEOF_MP_LIMB_T = (GMP_LIMB_BITS >> 3);
        const int GMP_NAIL_BITS = 0;

        const int GMP_NUMB_BITS = (GMP_LIMB_BITS - GMP_NAIL_BITS);
        const mp_limb_t GMP_NUMB_MASK = ((~((mp_limb_t)0)) >> GMP_NAIL_BITS);
        const mp_limb_t GMP_NUMB_MAX = GMP_NUMB_MASK;
        const mp_limb_t GMP_NAIL_MASK = (~GMP_NUMB_MASK);

        const mp_size_t GMP_LIMB_BYTES = SIZEOF_MP_LIMB_T;
        #endregion

        static mp_size_t MIN(mp_size_t l, mp_size_t o) {
            return ((l) < (o) ? (l) : (o));
        }

        static mp_size_t MAX(mp_size_t h, mp_size_t i) {
            return ((h) > (i) ? (h) : (i));
        }

        #region Scatch Size Helpers



        /* FIXME: Make these itch functions less conservative.  Also consider making
           them dependent on just 'an', and compute the allocation directly from 'an'
           instead of via n.  */

        /* toom22/toom2: Scratch need is 2*(an + k), k is the recursion depth.
           k is ths smallest k such that
             ceil(an/2^k) < MUL_TOOM22_THRESHOLD.
           which implies that
             k = bitsize of floor ((an-1)/(MUL_TOOM22_THRESHOLD-1))
               = 1 + floor (log_2 (floor ((an-1)/(MUL_TOOM22_THRESHOLD-1))))
        */
        static mp_size_t mpn_toom22_mul_itch(mp_size_t an, mp_size_t bn) {
            return (2 * ((an) + GMP_NUMB_BITS));
        }
        static mp_size_t mpn_toom2_sqr_itch(mp_size_t an) {
            return (2 * ((an) + GMP_NUMB_BITS));
        }


        /* toom33/toom3: Scratch need is 5an/2 + 10k, k is the recursion depth.
           We use 3an + C, so that we can use a smaller constant.
         */
        static mp_size_t mpn_toom33_mul_itch(mp_size_t an, mp_size_t bn) {
            return (3 * (an) + GMP_NUMB_BITS);
        }
        static mp_size_t mpn_toom3_sqr_itch(mp_size_t an) {
            return (3 * (an) + GMP_NUMB_BITS);
        }

        /* toom33/toom3: Scratch need is 8an/3 + 13k, k is the recursion depth.
           We use 3an + C, so that we can use a smaller constant.
         */
        static mp_size_t mpn_toom44_mul_itch(mp_size_t an, mp_size_t bn) {
            return (3 * (an) + GMP_NUMB_BITS);
        }
        static mp_size_t mpn_toom4_sqr_itch(mp_size_t an) {
            return (3 * (an) + GMP_NUMB_BITS);
        }

        static mp_size_t mpn_toom6_sqr_itch(mp_size_t n) {
            return (((n) - SQR_TOOM6_THRESHOLD) * 2 +
             MAX(SQR_TOOM6_THRESHOLD * 2 + GMP_NUMB_BITS * 6,
                 mpn_toom4_sqr_itch(SQR_TOOM6_THRESHOLD)));
        }

        const mp_size_t MUL_TOOM6H_MIN =
          ((MUL_TOOM6H_THRESHOLD > MUL_TOOM44_THRESHOLD) ?
            MUL_TOOM6H_THRESHOLD : MUL_TOOM44_THRESHOLD);
        static mp_size_t mpn_toom6_mul_n_itch(mp_size_t n) {
            return (((n) - MUL_TOOM6H_MIN) * 2 +
             MAX(MUL_TOOM6H_MIN * 2 + GMP_NUMB_BITS * 6,
                 mpn_toom44_mul_itch(MUL_TOOM6H_MIN, MUL_TOOM6H_MIN)));
        }

        static mp_size_t
        mpn_toom6h_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t estimatedN;
            estimatedN = (an + bn) / (size_t)10 + 1;
            return mpn_toom6_mul_n_itch(estimatedN * 6);
        }

        static mp_size_t mpn_toom8_sqr_itch(mp_size_t n) {
            return ((((n) * 15) >> 3) - ((SQR_TOOM8_THRESHOLD * 15) >> 3) +
       MAX(((SQR_TOOM8_THRESHOLD * 15) >> 3) + GMP_NUMB_BITS * 6,
           mpn_toom6_sqr_itch(SQR_TOOM8_THRESHOLD)));
        }

        const mp_size_t MUL_TOOM8H_MIN =
          ((MUL_TOOM8H_THRESHOLD > MUL_TOOM6H_MIN) ?
            MUL_TOOM8H_THRESHOLD : MUL_TOOM6H_MIN);
        static mp_size_t mpn_toom8_mul_n_itch(mp_size_t n) {
            return ((((n) * 15) >> 3) - ((MUL_TOOM8H_MIN * 15) >> 3) +
     MAX(((MUL_TOOM8H_MIN * 15) >> 3) + GMP_NUMB_BITS * 6,
         mpn_toom6_mul_n_itch(MUL_TOOM8H_MIN)));
        }

        static mp_size_t
        mpn_toom8h_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t estimatedN;
            estimatedN = (an + bn) / (size_t)14 + 1;
            return mpn_toom8_mul_n_itch(estimatedN * 8);
        }

        static mp_size_t
mpn_toom32_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (2 * an >= 3 * bn ? (an - 1) / (size_t)3 : (bn - 1) >> 1);
            mp_size_t itch = 2 * n + 1;

            return itch;
        }

        static mp_size_t
mpn_toom42_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = an >= 2 * bn ? (an + 3) >> 2 : (bn + 1) >> 1;
            return 6 * n + 3;
        }

        static mp_size_t
mpn_toom43_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (3 * an >= 4 * bn ? (an - 1) >> 2 : (bn - 1) / (size_t)3);

            return 6 * n + 4;
        }

        static mp_size_t
mpn_toom52_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (2 * an >= 5 * bn ? (an - 1) / (size_t)5 : (bn - 1) >> 1);
            return 6 * n + 4;
        }

        static mp_size_t
mpn_toom53_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (3 * an >= 5 * bn ? (an - 1) / (size_t)5 : (bn - 1) / (size_t)3);
            return 10 * n + 10;
        }

        static mp_size_t
mpn_toom62_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (an >= 3 * bn ? (an - 1) / (size_t)6 : (bn - 1) >> 1);
            return 10 * n + 10;
        }

        static mp_size_t
mpn_toom63_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (an >= 2 * bn ? (an - 1) / (size_t)6 : (bn - 1) / (size_t)3);
            return 9 * n + 3;
        }

        static mp_size_t
mpn_toom54_mul_itch(mp_size_t an, mp_size_t bn) {
            mp_size_t n = 1 + (4 * an >= 5 * bn ? (an - 1) / (size_t)5 : (bn - 1) / (size_t)4);
            return 9 * n + 3;
        }

        /* let S(n) = space required for input size n,
           then S(n) = 3 floor(n/2) + 1 + S(floor(n/2)).   */
        static mp_size_t mpn_toom42_mulmid_itch(mp_size_t n) {
            return (3 * (n) + GMP_NUMB_BITS);
        }
        #endregion




        /* Copy N limbs from SRC to DST incrementing, N==0 allowed.  */
        static void MPN_COPY_INCR(mp_ptr dst, mp_srcptr src, mp_size_t n) {
            ASSERT((n) >= 0);
            ASSERT(MPN_SAME_OR_INCR_P(dst, src, n));
            if ((n) != 0) {
                mp_size_t __n = (n) - 1;
                mp_ptr __dst = (dst);
                mp_srcptr __src = (src);
                mp_limb_t __x;
                __x = __src++.Current;
                if (__n != 0) {
                    do {

                        __dst++.Current = __x;
                        __x = __src++.Current;
                    }
                    while (0 != --__n);
                }

                __dst++.Current = __x;
            }
        }


        /* Copy N limbs from SRC to DST decrementing, N==0 allowed.  */
        static void MPN_COPY_DECR(mp_ptr dst, mp_srcptr src, mp_size_t n) {
            ASSERT((n) >= 0);
            ASSERT(MPN_SAME_OR_DECR_P(dst, src, n));
            if ((n) != 0) {
                mp_size_t __n = (n) - 1;
                mp_ptr __dst = (dst) + __n;
                mp_srcptr __src = (src) + __n;
                mp_limb_t __x;
                __x = __src--.Current;
                if (__n != 0) {
                    do {

                        __dst--.Current = __x;
                        __x = __src--.Current;
                    }
                    while (0 != --__n);
                }

                __dst--.Current = __x;
            }
        }

        static void MPN_COPY(mp_ptr d, mp_srcptr s, mp_size_t n) {
            ASSERT(MPN_SAME_OR_SEPARATE_P(d, s, n));
            MPN_COPY_INCR(d, s, n);
        }


        /* Set {dst,size} to the limbs of {src,size} in reverse order. */
        static void MPN_REVERSE(mp_ptr dst, mp_srcptr src, mp_size_t size) {
            mp_ptr __dst = (dst);
            mp_size_t __size = (size);
            mp_srcptr __src = (src) + __size - 1;
            mp_size_t __i;
            ASSERT((size) >= 0);
            ASSERT(!MPN_OVERLAP_P(dst, size, src, size));

            for (__i = 0; __i < __size; __i++) {

                __dst.Current = __src.Current;
                __dst++;
                __src--;
            }
        }


        /* Zero n limbs at dst.
         */

        static void MPN_FILL(mp_ptr dst, mp_size_t n, mp_limb_t f) {
            mp_ptr __dst = (dst);
            mp_size_t __n = (n);
            ASSERT(__n > 0);
            do
                __dst++.Current = (f);
            while (0 != --__n);
        }

        static void MPN_ZERO(mp_ptr dst, mp_size_t n) {
            ASSERT((n) >= 0);
            if ((n) != 0)
                MPN_FILL(dst, n, (mp_limb_t)0);
        }

        /* On the x86s repe/scasl doesn't seem useful, since it takes many cycles to
           start up and would need to strip a lot of zeros before it'd be faster
           than a simple cmpl loop.  Here are some times in cycles for
           std/repe/scasl/cld and cld/repe/scasl (the latter would be for stripping
           low zeros).

                std   cld
               P5    18    16
               P6    46    38
               K6    36    13
               K7    21    20
        */
        static void MPN_NORMALIZE(mp_ptr DST, mp_size_t NLIMBS) {
            while ((NLIMBS) > 0) {
                if ((DST)[(NLIMBS) - 1] != 0)
                    break;
                (NLIMBS)--;
            }
        }

        static void MPN_NORMALIZE_NOT_ZERO(mp_ptr DST, mp_size_t NLIMBS) {
            while (true) {
                ASSERT((NLIMBS) >= 1);
                if ((DST)[(NLIMBS) - 1] != 0)
                    break;
                (NLIMBS)--;
            }
        }

        /* Strip least significant zero limbs from {ptr,size} by incrementing ptr
           and decrementing size.  low should be ptr[0], and will be the new ptr[0]
           on returning.  The number in {ptr,size} must be non-zero, ie. size!=0 and
           somewhere a non-zero limb.  */
        static void MPN_STRIP_LOW_ZEROS_NOT_ZERO(mp_ptr ptr, mp_size_t size, mp_limb_t low) {
            ASSERT((size) >= 1);
            ASSERT((low) == (ptr)[0]);

            while ((low) == 0) {
                (size)--;
                ASSERT((size) >= 1);
                (ptr)++;
                (low) = (ptr).Current;
            }
        }



        public static
        void
mpn_com(mp_ptr rp, mp_srcptr up, mp_size_t n) {
            mp_limb_t ul;
            do {
                ul = up++.Current;
                rp++.Current = ~ul & GMP_NUMB_MASK;
            } while (--n != 0);
        }



        public static
        void
        mpn_zero(mp_ptr rp, mp_size_t n) {
            mp_size_t i;

            rp += n;
            for (i = -n; i != 0; i++)
                rp[i] = 0;
        }



        /* Copy {src,size} to {dst,size}, starting at "start".  This is designed to
   keep the indexing dst[j] and src[j] nice and simple for __GMPN_ADD_1,
   __GMPN_ADD, etc.  */

        static void __GMPN_COPY_REST(mp_ptr dst, mp_srcptr src, mp_size_t size, mp_size_t start) {
            mp_size_t __gmp_j;
            /* ASSERT ((size) >= 0); */
            /* ASSERT ((start) >= 0); */
            /* ASSERT ((start) <= (size)); */
            /* ASSERT (MPN_SAME_OR_SEPARATE_P (dst, src, size)); */
            for (__gmp_j = (start); __gmp_j < (size); __gmp_j++)
                (dst)[__gmp_j] = (src)[__gmp_j];
        }
        /* Enhancement: Use some of the smarter code from gmp-impl.h.  Maybe use
           mpn_copyi if there's a native version, and if we don't mind demanding
           binary compatibility for it (on targets which use it).  */
        static void __GMPN_COPY(mp_ptr dst, mp_srcptr src, mp_size_t size) {
            __GMPN_COPY_REST(dst, src, size, 0);
        }

        static bool __GMPN_ADDCB(mp_limb_t r, mp_limb_t x, mp_limb_t y) {
            return ((r) < (y));
        }

        static bool __GMPN_SUBCB(mp_limb_t r, mp_limb_t x, mp_limb_t y) {
            return ((x) < (y));
        }

        static void __GMPN_ADD_1(out mp_limb_t cout, mp_ptr dst, mp_srcptr src, mp_size_t n, mp_limb_t v) {
            mp_size_t __gmp_i;
            mp_limb_t __gmp_x, __gmp_r;

            /* ASSERT ((n) >= 1); */
            /* ASSERT (MPN_SAME_OR_SEPARATE_P (dst, src, n)); */

            __gmp_x = (src)[0];
            __gmp_r = __gmp_x + (v);
            (dst)[0] = __gmp_r;
            if (__GMPN_ADDCB(__gmp_r, __gmp_x, (v))) {
                (cout) = 1;
                for (__gmp_i = 1; __gmp_i < (n);) {
                    __gmp_x = (src)[__gmp_i];
                    __gmp_r = __gmp_x + 1;
                    (dst)[__gmp_i] = __gmp_r;
                    ++__gmp_i;
                    if (!__GMPN_ADDCB(__gmp_r, __gmp_x, 1)) {
                        if ((src) != (dst))
                            __GMPN_COPY_REST(dst, src, n, __gmp_i);
                        (cout) = 0;
                        break;
                    }
                }
            } else {
                if ((src) != (dst))
                    __GMPN_COPY_REST(dst, src, n, 1);
                (cout) = 0;
            }
        }


        static void __GMPN_SUB_1(out mp_limb_t cout, mp_ptr dst, mp_srcptr src, mp_size_t n, mp_limb_t v) {
            mp_size_t __gmp_i;
            mp_limb_t __gmp_x, __gmp_r;

            /* ASSERT ((n) >= 1); */
            /* ASSERT (MPN_SAME_OR_SEPARATE_P (dst, src, n)); */

            __gmp_x = (src)[0];
            __gmp_r = __gmp_x - (v);
            (dst)[0] = __gmp_r;
            if (__GMPN_SUBCB(__gmp_r, __gmp_x, (v))) {
                (cout) = 1;
                for (__gmp_i = 1; __gmp_i < (n);) {
                    __gmp_x = (src)[__gmp_i];
                    __gmp_r = __gmp_x - 1;
                    (dst)[__gmp_i] = __gmp_r;
                    ++__gmp_i;
                    if (!__GMPN_SUBCB(__gmp_r, __gmp_x, 1)) {
                        if ((src) != (dst))
                            __GMPN_COPY_REST(dst, src, n, __gmp_i);
                        (cout) = 0;
                        break;
                    }
                }
            } else {
                if ((src) != (dst))
                    __GMPN_COPY_REST(dst, src, n, 1);
                (cout) = 0;
            }
        }

        static void __GMPN_ADD(out mp_limb_t cout, mp_ptr wp, mp_srcptr xp, mp_size_t xsize, mp_srcptr yp, mp_size_t ysize) {
            mp_size_t __gmp_i;
            mp_limb_t __gmp_x;

            /* ASSERT ((ysize) >= 0); */
            /* ASSERT ((xsize) >= (ysize)); */
            /* ASSERT (MPN_SAME_OR_SEPARATE2_P (wp, xsize, xp, xsize)); */
            /* ASSERT (MPN_SAME_OR_SEPARATE2_P (wp, xsize, yp, ysize)); */

            __gmp_i = (ysize);
            if (__gmp_i != 0) {
                if (0 != mpn_add_n(wp, xp, yp, __gmp_i)) {
                    do {
                        if (__gmp_i >= (xsize)) {
                            (cout) = 1;
                            goto __gmp_done;
                        }
                        __gmp_x = (xp)[__gmp_i];
                    }
                    while ((((wp)[__gmp_i++] = (__gmp_x + 1) & GMP_NUMB_MASK) == 0));
                }
            }
            if ((wp) != (xp))
                __GMPN_COPY_REST(wp, xp, xsize, __gmp_i);
            (cout) = 0;
        __gmp_done:
            ;
        }

        static void __GMPN_SUB(out mp_limb_t cout, mp_ptr wp, mp_srcptr xp, mp_size_t xsize, mp_srcptr yp, mp_size_t ysize) {
            mp_size_t __gmp_i;
            mp_limb_t __gmp_x;

            /* ASSERT ((ysize) >= 0); */
            /* ASSERT ((xsize) >= (ysize)); */
            /* ASSERT (MPN_SAME_OR_SEPARATE2_P (wp, xsize, xp, xsize)); */
            /* ASSERT (MPN_SAME_OR_SEPARATE2_P (wp, xsize, yp, ysize)); */

            __gmp_i = (ysize);
            if (__gmp_i != 0) {
                if (0 != mpn_sub_n(wp, xp, yp, __gmp_i)) {
                    do {
                        if (__gmp_i >= (xsize)) {
                            (cout) = 1;
                            goto __gmp_done;
                        }
                        __gmp_x = (xp)[__gmp_i];
                    }
                    while (Comma(((wp)[__gmp_i++] = (__gmp_x - 1) & GMP_NUMB_MASK), __gmp_x == 0));
                }
            }
            if ((wp) != (xp))
                __GMPN_COPY_REST(wp, xp, xsize, __gmp_i);
            (cout) = 0;
        __gmp_done:
            ;
        }

        /* Compare {xp,size} and {yp,size}, setting "result" to positive, zero or
           negative.  size==0 is allowed.  On random data usually only one limb will
           need to be examined to get a result, so it's worth having it inline.  */
        static void __GMPN_CMP(out int result, mp_srcptr xp, mp_srcptr yp, mp_size_t size) {
            mp_size_t __gmp_i;
            mp_limb_t __gmp_x, __gmp_y;

            /* ASSERT ((size) >= 0); */

            (result) = 0;
            __gmp_i = (size);
            while (--__gmp_i >= 0) {
                __gmp_x = (xp)[__gmp_i];
                __gmp_y = (yp)[__gmp_i];
                if (__gmp_x != __gmp_y) {
                    /* Cannot use __gmp_x - __gmp_y, may overflow an "int" */
                    (result) = (__gmp_x > __gmp_y ? 1 : -1);
                    break;
                }
            }
        }


        public static mp_limb_t mpn_add_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t ul, vl, sl, rl, cy, cy1, cy2;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_INCR_P(rp, up, n));
            ASSERT(MPN_SAME_OR_INCR_P(rp, vp, n));

            cy = 0;
            do {
                ul = up++.Current;
                vl = vp++.Current;
                sl = ul + vl;
                cy1 = CBool(sl < ul);
                rl = sl + cy;
                cy2 = CBool(rl < sl);
                cy = cy1 | cy2;
                rp++.Current = rl;
            }
            while (--n != 0);

            return cy;
        }


        public static mp_limb_t
        mpn_sub_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t ul, vl, sl, rl, cy, cy1, cy2;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_INCR_P(rp, up, n));
            ASSERT(MPN_SAME_OR_INCR_P(rp, vp, n));

            cy = 0;
            do {
                ul = up++.Current;
                vl = vp++.Current;
                sl = ul - vl;
                cy1 = CBool(sl > ul);
                rl = sl - cy;
                cy2 = CBool(rl > sl);
                cy = cy1 | cy2;
                rp++.Current = rl;
            }
            while (--n != 0);

            return cy;
        }

        const mp_size_t L1_CACHE_SIZE = 8192    /* only 68040 has less than this */;


        const mp_size_t PART_SIZE = (L1_CACHE_SIZE / GMP_LIMB_BYTES / 6);


        /* mpn_add_n_sub_n.
           r1[] = s1[] + s2[]
           r2[] = s1[] - s2[]
           All operands have n limbs.
           In-place operations allowed.  */
        public static mp_limb_t
        mpn_add_n_sub_n(mp_ptr r1p, mp_ptr r2p, mp_srcptr s1p, mp_srcptr s2p, mp_size_t n) {
            mp_limb_t acyn, acyo;       /* carry for add */
            mp_limb_t scyn, scyo;       /* carry for subtract */
            mp_size_t off;      /* offset in operands */
            mp_size_t this_n;       /* size of current chunk */

            /* We alternatingly add and subtract in chunks that fit into the (L1)
               cache.  Since the chunks are several hundred limbs, the function call
               overhead is insignificant, but we get much better locality.  */

            /* We have three variant of the inner loop, the proper loop is chosen
               depending on whether r1 or r2 are the same operand as s1 or s2.  */

            if (r1p != s1p && r1p != s2p) {
                /* r1 is not identical to either input operand.  We can therefore write
               to r1 directly, without using temporary storage.  */
                acyo = 0;
                scyo = 0;
                for (off = 0; off < n; off += PART_SIZE) {
                    this_n = MIN(n - off, PART_SIZE);

                    acyn = mpn_add_n(r1p + off, s1p + off, s2p + off, this_n);
                    acyo = acyn + mpn_add_1(r1p + off, r1p + off, this_n, acyo);

                    scyn = mpn_sub_n(r2p + off, s1p + off, s2p + off, this_n);
                    scyo = scyn + mpn_sub_1(r2p + off, r2p + off, this_n, scyo);
                }
            } else if (r2p != s1p && r2p != s2p) {
                /* r2 is not identical to either input operand.  We can therefore write
               to r2 directly, without using temporary storage.  */
                acyo = 0;
                scyo = 0;
                for (off = 0; off < n; off += PART_SIZE) {
                    this_n = MIN(n - off, PART_SIZE);

                    scyn = mpn_sub_n(r2p + off, s1p + off, s2p + off, this_n);
                    scyo = scyn + mpn_sub_1(r2p + off, r2p + off, this_n, scyo);

                    acyn = mpn_add_n(r1p + off, s1p + off, s2p + off, this_n);
                    acyo = acyn + mpn_add_1(r1p + off, r1p + off, this_n, acyo);

                }
            } else {
                /* r1 and r2 are identical to s1 and s2 (r1==s1 and r2==s2 or vice versa)
               Need temporary storage.  */
                var tp = salloc<mp_limb_t>(PART_SIZE);
                acyo = 0;
                scyo = 0;
                for (off = 0; off < n; off += PART_SIZE) {
                    this_n = MIN(n - off, PART_SIZE);

                    acyn = mpn_add_n(tp, s1p + off, s2p + off, this_n);
                    acyo = acyn + mpn_add_1(tp, tp, this_n, acyo);

                    scyn = mpn_sub_n(r2p + off, s1p + off, s2p + off, this_n);
                    scyo = scyn + mpn_sub_1(r2p + off, r2p + off, this_n, scyo);
                    MPN_COPY(r1p + off, tp, this_n);
                }
            }

            return 2 * acyo + scyo;
        }

        public static
mp_limb_t
mpn_add_nc(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n, mp_limb_t ci) {
            mp_limb_t co;
            co = mpn_add_n(rp, up, vp, n);
            co += mpn_add_1(rp, rp, n, ci);
            return co;
        }

        public static mp_limb_t
mpn_sub_nc(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n, mp_limb_t ci) {
            mp_limb_t co;
            co = mpn_sub_n(rp, up, vp, n);
            co += mpn_sub_1(rp, rp, n, ci);
            return co;
        }

        public static
        mp_limb_t mpn_add(mp_ptr __gmp_wp, mp_srcptr __gmp_xp, mp_size_t __gmp_xsize, mp_srcptr __gmp_yp, mp_size_t __gmp_ysize) {
            mp_limb_t __gmp_c;
            __GMPN_ADD(out __gmp_c, __gmp_wp, __gmp_xp, __gmp_xsize, __gmp_yp, __gmp_ysize);
            return __gmp_c;
        }

        public static
        mp_limb_t mpn_add_1(mp_ptr __gmp_dst, mp_srcptr __gmp_src, mp_size_t __gmp_size, mp_limb_t __gmp_n) {
            mp_limb_t __gmp_c;
            __GMPN_ADD_1(out __gmp_c, __gmp_dst, __gmp_src, __gmp_size, __gmp_n);
            return __gmp_c;
        }
        public static
    int
    mpn_cmp(mp_srcptr __gmp_xp, mp_srcptr __gmp_yp, mp_size_t __gmp_size) {
            int __gmp_result;
            __GMPN_CMP(out __gmp_result, __gmp_xp, __gmp_yp, __gmp_size);
            return __gmp_result;
        }

        public static bool mpn_zero_p(mp_srcptr __gmp_p, mp_size_t __gmp_n) {
            /* if (__GMP_LIKELY (__gmp_n > 0)) */
            do {
                if (__gmp_p[--__gmp_n] != 0)
                    return false;
            } while (__gmp_n != 0);
            return true;
        }

        public static
                mp_limb_t
                mpn_sub(mp_ptr __gmp_wp, mp_srcptr __gmp_xp, mp_size_t __gmp_xsize, mp_srcptr __gmp_yp, mp_size_t __gmp_ysize) {
            mp_limb_t __gmp_c;
            __GMPN_SUB(out __gmp_c, __gmp_wp, __gmp_xp, __gmp_xsize, __gmp_yp, __gmp_ysize);
            return __gmp_c;
        }

        public static
        mp_limb_t
        mpn_sub_1(mp_ptr __gmp_dst, mp_srcptr __gmp_src, mp_size_t __gmp_size, mp_limb_t __gmp_n) {
            mp_limb_t __gmp_c;
            __GMPN_SUB_1(out __gmp_c, __gmp_dst, __gmp_src, __gmp_size, __gmp_n);
            return __gmp_c;
        }

        public static
          mp_limb_t
 mpn_neg(mp_ptr __gmp_rp, mp_srcptr __gmp_up, mp_size_t __gmp_n) {
            while (__gmp_up.Current == 0) /* Low zero limbs are unchanged by negation. */
              {
                __gmp_rp.Current = 0;
                if (0 == --__gmp_n) /* All zero */
                    return 0;
                ++__gmp_up; ++__gmp_rp;
            }

            __gmp_rp.Current = (NEG(__gmp_up.Current)) & GMP_NUMB_MASK;

            if (0 != --__gmp_n) /* Higher limbs get complemented. */
                mpn_com(++__gmp_rp, ++__gmp_up, __gmp_n);

            return 1;
        }

        /* Multiply {up,usize} by {vp,vsize} and write the result to
        {prodp,usize+vsize}.  Must have usize>=vsize.

        Note that prodp gets usize+vsize limbs stored, even if the actual result
        only needs usize+vsize-1.

        There's no good reason to call here with vsize>=MUL_TOOM22_THRESHOLD.
        Currently this is allowed, but it might not be in the future.

        This is the most critical code for multiplication.  All multiplies rely
        on this, both small and huge.  Small ones arrive here immediately, huge
        ones arrive here as this is the base case for Karatsuba's recursive
        algorithm.  */
        public static
             void
             mpn_mul_basecase(mp_ptr rp,
                       mp_srcptr up, mp_size_t un,
                       mp_srcptr vp, mp_size_t vn) {
            ASSERT(un >= vn);
            ASSERT(vn >= 1);
            ASSERT(!MPN_OVERLAP_P(rp, un + vn, up, un));
            ASSERT(!MPN_OVERLAP_P(rp, un + vn, vp, vn));

            /* We first multiply by the low order limb (or depending on optional function
               availability, limbs).  This result can be stored, not added, to rp.  We
               also avoid a loop for zeroing this way.  */

            rp[un] = mpn_mul_1(rp, up, un, vp[0]);
            rp += 1; vp += 1; vn -= 1;

            /* Now accumulate the product of up[] and the next higher limb (or depending
               on optional function availability, limbs) from vp[].  */


            const mp_size_t MAX_LEFT = (2 - 1); /* Used to simplify loops into if statements */

            while (vn >= 1) {
                rp[un] = mpn_addmul_1(rp, up, un, vp[0]);
                if (MAX_LEFT == 1)
                    return;
                rp += 1; vp += 1; vn -= 1;
            }
        }


        public static void MPN_SQR_DIAGONAL(mp_ptr rp, mp_srcptr up, mp_size_t n) {
            mp_size_t _i;
            for (_i = 0; _i < (n); _i++) {
                mp_limb_t ul, lpl;
                ul = (up)[_i];
                umul_ppmm(out (rp)[2 * _i + 1], out lpl, ul, ul << GMP_NAIL_BITS);
                (rp)[2 * _i] = lpl >> GMP_NAIL_BITS;
            }
        }

        public static void MPN_SQR_DIAG_ADDLSH1(mp_ptr rp, mp_srcptr tp, mp_srcptr up, mp_size_t n) {
            mp_limb_t cy;
            MPN_SQR_DIAGONAL(rp, up, n);
            cy = mpn_addlsh1_n(rp + 1, rp + 1, tp, 2 * n - 2);
            rp[2 * n - 1] += cy;
        }

        public static
void
mpn_sqr_basecase(mp_ptr rp, mp_srcptr up, mp_size_t n) {
            mp_size_t i;

            ASSERT(n >= 1);
            ASSERT(!MPN_OVERLAP_P(rp, 2 * n, up, n));

            {
                mp_limb_t ul, lpl;
                ul = up[0];
                umul_ppmm(out rp[1], out lpl, ul, ul << GMP_NAIL_BITS);
                rp[0] = lpl >> GMP_NAIL_BITS;
            }
            if (n > 1) {
                var tarr = salloc<mp_limb_t>(2 * SQR_TOOM2_THRESHOLD);
                mp_ptr tp = tarr;
                mp_limb_t cy;

                /* must fit 2*n limbs in tarr */
                ASSERT(n <= SQR_TOOM2_THRESHOLD);

                cy = mpn_mul_1(tp, up + 1, n - 1, up[0]);
                tp[n - 1] = cy;
                for (i = 2; i < n; i++) {
                    mp_limb_t cy1;
                    cy1 = mpn_addmul_1(tp + 2 * i - 2, up + i, n - i, up[i - 1]);
                    tp[n + i - 2] = cy1;
                }

                MPN_SQR_DIAG_ADDLSH1(rp, tp, up, n);
            }
        }




        public static void mpn_incr_u(mp_ptr p) {
            mp_limb_t __x;
            mp_ptr __p = (p);
            {
                while (++((__p++).Current) == 0)
                    ;
            }
        }

        public static void mpn_incr_u(mp_ptr p, mp_limb_t incr) {
            mp_limb_t __x;
            mp_ptr __p = (p);
            {
                __x = __p.Current + (incr);

                __p.Current = __x;
                if (__x < (incr))
                    while (++((++__p).Current) == 0)
                        ;
            }
        }
        public static void mpn_decr_u(mp_ptr p) {
            mp_limb_t __x;
            mp_ptr __p = (p);
            {
                while (((__p++).Current)-- == 0)
                    ;
            }
        }
        public static void mpn_decr_u(mp_ptr p, mp_limb_t incr) {
            mp_limb_t __x;
            mp_ptr __p = (p);
            {
                __x = __p.Current;

                __p.Current = __x - (incr);
                if (__x < (incr))
                    while (((++__p).Current)-- == 0)
                        ;
            }
        }

        public static void MPN_INCR_U(mp_ptr ptr, mp_size_t size) {
            mpn_incr_u(ptr);
        }

        public static void MPN_DECR_U(mp_ptr ptr, mp_size_t size) {
            mpn_decr_u(ptr);
        }

        public static void MPN_INCR_U(mp_ptr ptr, mp_size_t size, mp_limb_t n) {
            mpn_incr_u(ptr, n);
        }

        public static void MPN_DECR_U(mp_ptr ptr, mp_size_t size, mp_limb_t n) {
            mpn_decr_u(ptr, n);
        }


        /* Shift U (pointed to by up and n limbs long) cnt bits to the left
           and store the n least significant limbs of the result at rp.
           Return the bits shifted out from the most significant limb.

           Argument constraints:
           1. 0 < cnt < GMP_NUMB_BITS.
           2. If the result is to be written over the input, rp must be >= up.
        */

        public static mp_limb_t
        mpn_lshift(mp_ptr rp, mp_srcptr up, mp_size_t n, int cnt) {
            mp_limb_t high_limb, low_limb;
            int tnc;
            mp_size_t i;
            mp_limb_t retval;

            ASSERT(n >= 1);
            ASSERT(cnt >= 1);
            ASSERT(cnt < GMP_NUMB_BITS);
            ASSERT(MPN_SAME_OR_DECR_P(rp, up, n));

            up += n;
            rp += n;

            tnc = GMP_NUMB_BITS - cnt;
            low_limb = (--up).Current;
            retval = low_limb >> tnc;
            high_limb = (low_limb << cnt) & GMP_NUMB_MASK;

            for (i = n - 1; i != 0; i--) {
                low_limb = (--up).Current;
                (--rp).Current = high_limb | (low_limb >> tnc);
                high_limb = (low_limb << cnt) & GMP_NUMB_MASK;
            }
            (--rp).Current = high_limb;

            return retval;
        }


        public static mp_limb_t
       mpn_rshift(mp_ptr rp, mp_srcptr up, mp_size_t n, int cnt) {
            mp_limb_t high_limb, low_limb;
            int tnc;
            mp_size_t i;
            mp_limb_t retval;

            ASSERT(n >= 1);
            ASSERT(cnt >= 1);
            ASSERT(cnt < GMP_NUMB_BITS);
            ASSERT(MPN_SAME_OR_INCR_P(rp, up, n));

            tnc = GMP_NUMB_BITS - cnt;
            high_limb = up++.Current;
            retval = (high_limb << tnc) & GMP_NUMB_MASK;
            low_limb = high_limb >> cnt;

            for (i = n - 1; i != 0; i--) {
                high_limb = up++.Current;
                rp++.Current = low_limb | ((high_limb << tnc) & GMP_NUMB_MASK);
                low_limb = high_limb >> cnt;
            }
            rp.Current = low_limb;

            return retval;
        }


        /* Evaluate in: -1, 0, +1, +inf

  <-s-><--n--><--n-->
   ___ ______ ______
  |a2_|___a1_|___a0_|
	|_b1_|___b0_|
	<-t--><--n-->

  v0  =  a0         * b0      #   A(0)*B(0)
  v1  = (a0+ a1+ a2)*(b0+ b1) #   A(1)*B(1)      ah  <= 2  bh <= 1
  vm1 = (a0- a1+ a2)*(b0- b1) #  A(-1)*B(-1)    |ah| <= 1  bh = 0
  vinf=          a2 *     b1  # A(inf)*B(inf)
*/
        public static
                void TOOM32_MUL_N_REC(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n, mp_ptr ws) {
            mpn_mul_n(p, a, b, n);
        }

        public static
        void
mpn_toom32_mul(mp_ptr pp,
        mp_srcptr ap, mp_size_t an,
        mp_srcptr bp, mp_size_t bn,
        mp_ptr scratch) {
            mp_size_t n, s, t;
            int vm1_neg;
            mp_limb_t cy;
            mp_limb_t hi; // mp_limb_signed_t hi;
            mp_limb_t ap1_hi, bp1_hi;



            /* Required, to ensure that s + t >= n. */
            ASSERT(bn + 2 <= an && an + 6 <= 3 * bn);

            n = 1 + (2 * an >= 3 * bn ? (an - 1) / (size_t)3 : (bn - 1) >> 1);

            var a0 = ap;
            var a1 = (ap + n);
            var a2 = (ap + 2 * n);
            var b0 = bp;
            var b1 = (bp + n);

            s = an - 2 * n;
            t = bn - n;

            ASSERT(0 < s && s <= n);
            ASSERT(0 < t && t <= n);
            ASSERT(s + t >= n);

            /* Product area of size an + bn = 3*n + s + t >= 4*n + 2. */
            var ap1 = (pp)      /* n, most significant limb in ap1_hi */;
            var bp1 = (pp + n)      /* n, most significant bit in bp1_hi */;
            var am1 = (pp + 2 * n)      /* n, most significant bit in hi */;
            var bm1 = (pp + 3 * n)      /* n */;
            var v1 = (scratch)      /* 2n + 1 */;
            var vm1 = (pp)      /* 2n + 1 */;
            var scratch_out = (scratch + 2 * n + 1) /* Currently unused. */;

            /* Scratch need: 2*n + 1 + scratch for the recursive multiplications. */

            /* FIXME: Keep v1[2*n] and vm1[2*n] in scalar variables? */

            /* Compute ap1 = a0 + a1 + a3, am1 = a0 - a1 + a3 */
            ap1_hi = mpn_add(ap1, a0, n, a2, s);

            if (ap1_hi == 0 && mpn_cmp(ap1, a1, n) < 0) {
                ap1_hi = mpn_add_n_sub_n(ap1, am1, a1, ap1, n) >> 1;
                hi = 0;
                vm1_neg = 1;
            } else {
                cy = mpn_add_n_sub_n(ap1, am1, ap1, a1, n);
                hi = ap1_hi - (cy & 1);
                ap1_hi += (cy >> 1);
                vm1_neg = 0;
            }

            /* Compute bp1 = b0 + b1 and bm1 = b0 - b1. */
            if (t == n) {

                if (mpn_cmp(b0, b1, n) < 0) {
                    cy = mpn_add_n_sub_n(bp1, bm1, b1, b0, n);
                    vm1_neg ^= 1;
                } else {
                    cy = mpn_add_n_sub_n(bp1, bm1, b0, b1, n);
                }
                bp1_hi = cy >> 1;

            } else {
                /* FIXME: Should still use mpn_add_n_sub_n for the main part. */
                bp1_hi = mpn_add(bp1, b0, n, b1, t);

                if (mpn_zero_p(b0 + t, n - t) && mpn_cmp(b0, b1, t) < 0) {
                    ASSERT_NOCARRY(mpn_sub_n(bm1, b1, b0, t));
                    mpn_zero(bm1 + t, n - t);
                    vm1_neg ^= 1;
                } else {
                    ASSERT_NOCARRY(mpn_sub(bm1, b0, n, b1, t));
                }
            }

            TOOM32_MUL_N_REC(v1, ap1, bp1, n, scratch_out);
            if (ap1_hi == 1) {
                cy = bp1_hi + mpn_add_n(v1 + n, v1 + n, bp1, n);
            } else if (ap1_hi == 2) {
                cy = 2 * bp1_hi + mpn_addlsh1_n(v1 + n, v1 + n, bp1, n);
            } else
                cy = 0;
            if (bp1_hi != 0)
                cy += mpn_add_n(v1 + n, v1 + n, ap1, n);
            v1[2 * n] = cy;

            TOOM32_MUL_N_REC(vm1, am1, bm1, n, scratch_out);
            if (0 != hi)
                hi = mpn_add_n(vm1 + n, vm1 + n, bm1, n);

            vm1[2 * n] = hi;

            /* v1 <-- (v1 + vm1) / 2 = x0 + x2 */
            if (0 != vm1_neg) {
                mpn_rsh1sub_n(v1, v1, vm1, 2 * n + 1);
            } else {
                mpn_rsh1add_n(v1, v1, vm1, 2 * n + 1);
            }

            /* We get x1 + x3 = (x0 + x2) - (x0 - x1 + x2 - x3), and hence

               y = x1 + x3 + (x0 + x2) * B
                 = (x0 + x2) * B + (x0 + x2) - vm1.

               y is 3*n + 1 limbs, y = y0 + y1 B + y2 B^2. We store them as
               follows: y0 at scratch, y1 at pp + 2*n, and y2 at scratch + n
               (already in place, except for carry propagation).

               We thus add

             B^3  B^2   B    1
              |    |    |    |
             +-----+----+
           + |  x0 + x2 |
             +----+-----+----+
           +      |  x0 + x2 |
              +----------+
           -      |  vm1     |
           --+----++----+----+-
             | y2  | y1 | y0 |
             +-----+----+----+

            Since we store y0 at the same location as the low half of x0 + x2, we
            need to do the middle sum first. */

            hi = vm1[2 * n];
            cy = mpn_add_n(pp + 2 * n, v1, v1 + n, n);
            MPN_INCR_U(v1 + n, n + 1, cy + v1[2 * n]);

            /* FIXME: Can we get rid of this second vm1_neg conditional by
               swapping the location of +1 and -1 values? */
            if (0 != vm1_neg) {
                cy = mpn_add_n(v1, v1, vm1, n);
                hi += mpn_add_nc(pp + 2 * n, pp + 2 * n, vm1 + n, n, cy);
                MPN_INCR_U(v1 + n, n + 1, hi);
            } else {
                cy = mpn_sub_n(v1, v1, vm1, n);
                hi += mpn_sub_nc(pp + 2 * n, pp + 2 * n, vm1 + n, n, cy);
                MPN_DECR_U(v1 + n, n + 1, hi);
            }

            TOOM32_MUL_N_REC(pp, a0, b0, n, scratch_out);
            /* vinf, s+t limbs.  Use mpn_mul for now, to handle unbalanced operands */
            if (s > t) mpn_mul(pp + 3 * n, a2, s, b1, t);
            else mpn_mul(pp + 3 * n, b1, t, a2, s);

            /* Remaining interpolation.

               y * B + x0 + x3 B^3 - x0 B^2 - x3 B
               = (x1 + x3) B + (x0 + x2) B^2 + x0 + x3 B^3 - x0 B^2 - x3 B
               = y0 B + y1 B^2 + y3 B^3 + Lx0 + H x0 B
                 + L x3 B^3 + H x3 B^4 - Lx0 B^2 - H x0 B^3 - L x3 B - H x3 B^2
               = L x0 + (y0 + H x0 - L x3) B + (y1 - L x0 - H x3) B^2
                 + (y2 - (H x0 - L x3)) B^3 + H x3 B^4

                B^4       B^3       B^2        B         1
           |         |         |         |         |         |
             +-------+                   +---------+---------+
             |  Hx3  |                   | Hx0-Lx3 |    Lx0  |
             +------+----------+---------+---------+---------+
                |    y2    |  y1     |   y0    |
                ++---------+---------+---------+
                -| Hx0-Lx3 | - Lx0   |
                 +---------+---------+
                        | - Hx3  |
                        +--------+

              We must take into account the carry from Hx0 - Lx3.
            */

            cy = mpn_sub_n(pp + n, pp + n, pp + 3 * n, n);
            hi = scratch[2 * n] + cy;

            cy = mpn_sub_nc(pp + 2 * n, pp + 2 * n, pp, n, cy);
            hi -= mpn_sub_nc(pp + 3 * n, scratch + n, pp + n, n, cy);

            hi += mpn_add(pp + n, pp + n, 3 * n, scratch, n);

            /* FIXME: Is support for s + t == n needed? */
            if (LIKELY(s + t > n)) {
                hi -= mpn_sub(pp + 2 * n, pp + 2 * n, 2 * n, pp + 4 * n, s + t - n);

                if (hi < 0)
                    MPN_DECR_U(pp + 4 * n, s + t - n, NEG(hi));
                else
                    MPN_INCR_U(pp + 4 * n, s + t - n, hi);
            } else
                ASSERT(hi == 0);
        }



        /* Evaluate in: -1, 0, +inf

          <-s--><--n-->
           ____ ______
          |_a1_|___a0_|
           |b1_|___b0_|
           <-t-><--n-->

          v0  =  a0     * b0       #   A(0)*B(0)
          vm1 = (a0- a1)*(b0- b1)  #  A(-1)*B(-1)
          vinf=      a1 *     b1   # A(inf)*B(inf)
        */


        static void TOOM22_MUL_N_REC(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n, mp_ptr ws) {
            if (BELOW_THRESHOLD(n, MUL_TOOM22_THRESHOLD))
                mpn_mul_basecase(p, a, n, b, n);
            else
                mpn_toom22_mul(p, a, n, b, n, ws);
        }

        /* Normally, this calls mul_basecase or toom22_mul.  But when when the fraction
           MUL_TOOM33_THRESHOLD / MUL_TOOM22_THRESHOLD is large, an initially small
           relative unbalance will become a larger and larger relative unbalance with
           each recursion (the difference s-t will be invariant over recursive calls).
           Therefore, we need to call toom32_mul.  FIXME: Suppress depending on
           MUL_TOOM33_THRESHOLD / MUL_TOOM22_THRESHOLD and on MUL_TOOM22_THRESHOLD.  */
        static void TOOM22_MUL_REC(mp_ptr p, mp_srcptr a, mp_size_t an, mp_srcptr b, mp_size_t bn, mp_ptr ws) {
            if (BELOW_THRESHOLD(bn, MUL_TOOM22_THRESHOLD))
                mpn_mul_basecase(p, a, an, b, bn);
            else if (4 * an < 5 * bn)
                mpn_toom22_mul(p, a, an, b, bn, ws);
            else
                mpn_toom32_mul(p, a, an, b, bn, ws);
        }

        public static void
        mpn_toom22_mul(mp_ptr pp,
        mp_srcptr ap, mp_size_t an,
        mp_srcptr bp, mp_size_t bn,
        mp_ptr scratch) {
            const int __gmpn_cpuvec_initialized = 1;
            mp_size_t n, s, t;
            int vm1_neg;
            mp_limb_t cy, cy2;
            mp_ptr asm1;
            mp_ptr bsm1;

            s = an >> 1;
            n = an - s;
            t = bn - n;

            var a0 = ap;
            var a1 = (ap + n);
            var b0 = bp;
            var b1 = (bp + n);

            ASSERT(an >= bn);

            ASSERT(0 < s && s <= n && s >= n - 1);
            ASSERT(0 < t && t <= s);

            asm1 = pp;
            bsm1 = pp + n;

            vm1_neg = 0;

            /* Compute asm1.  */
            if (s == n) {
                if (mpn_cmp(a0, a1, n) < 0) {
                    mpn_sub_n(asm1, a1, a0, n);
                    vm1_neg = 1;
                } else {
                    mpn_sub_n(asm1, a0, a1, n);
                }
            } else /* n - s == 1 */
                {
                if (a0[s] == 0 && mpn_cmp(a0, a1, s) < 0) {
                    mpn_sub_n(asm1, a1, a0, s);
                    asm1[s] = 0;
                    vm1_neg = 1;
                } else {
                    asm1[s] = a0[s] - mpn_sub_n(asm1, a0, a1, s);
                }
            }

            /* Compute bsm1.  */
            if (t == n) {
                if (mpn_cmp(b0, b1, n) < 0) {
                    mpn_sub_n(bsm1, b1, b0, n);
                    vm1_neg ^= 1;
                } else {
                    mpn_sub_n(bsm1, b0, b1, n);
                }
            } else {
                if (mpn_zero_p(b0 + t, n - t) && mpn_cmp(b0, b1, t) < 0) {
                    mpn_sub_n(bsm1, b1, b0, t);
                    mpn_zero(bsm1 + t, n - t);
                    vm1_neg ^= 1;
                } else {
                    mpn_sub(bsm1, b0, n, b1, t);
                }
            }

            var v0 = pp             /* 2n */;
            var vinf = (pp + 2 * n)         /* s+t */;
            var vm1 = scratch               /* 2n */;
            var scratch_out = scratch + 2 * n;

            /* vm1, 2n limbs */
            TOOM22_MUL_N_REC(vm1, asm1, bsm1, n, scratch_out);

            if (s > t) TOOM22_MUL_REC(vinf, a1, s, b1, t, scratch_out);
            else TOOM22_MUL_N_REC(vinf, a1, b1, s, scratch_out);

            /* v0, 2n limbs */
            TOOM22_MUL_N_REC(v0, ap, bp, n, scratch_out);

            /* H(v0) + L(vinf) */
            cy = mpn_add_n(pp + 2 * n, v0 + n, vinf, n);

            /* L(v0) + H(v0) */
            cy2 = cy + mpn_add_n(pp + n, pp + 2 * n, v0, n);

            /* L(vinf) + H(vinf) */
            cy += mpn_add(pp + 2 * n, pp + 2 * n, n, vinf + n, s + t - n);

            if (0 != vm1_neg)
                cy += mpn_add_n(pp + n, pp + n, vm1, 2 * n);
            else
                cy -= mpn_sub_n(pp + n, pp + n, vm1, 2 * n);

            ASSERT(cy + 1 <= 3);
            ASSERT(cy2 <= 2);

            MPN_INCR_U(pp + 2 * n, s + t, cy2);
            if (LIKELY(cy <= 2))
                /* if s+t==n, cy is zero, but we should not acces pp[3*n] at all. */
                MPN_INCR_U(pp + 3 * n, s + t - n, cy);
            else
                MPN_DECR_U(pp + 3 * n, s + t - n, 1);
        }


        static
        mp_limb_t
        mpn_bdiv_dbm1(mp_ptr qp, mp_srcptr ap, mp_size_t n, mp_limb_t bd) {
            mp_limb_t h = 0;
            mp_limb_t a, p0, p1, cy;
            mp_size_t i;

            for (i = 0; i < n; i++) {
                a = ap[i];
                umul_ppmm(out p1, out p0, a, bd << GMP_NAIL_BITS);
                p0 >>= GMP_NAIL_BITS;
                cy = CBool(h < p0);
                h = (h - p0) & GMP_NUMB_MASK;
                qp[i] = h;
                h = h - p1 - cy;
            }

            return h;
        }


        static
        mp_limb_t
        mpn_bdiv_dbm1c(mp_ptr qp, mp_srcptr ap, mp_size_t n, mp_limb_t bd, mp_limb_t h) {
            mp_limb_t a, p0, p1, cy;
            mp_size_t i;

            for (i = 0; i < n; i++) {
                a = ap[i];
                umul_ppmm(out p1, out p0, a, bd << GMP_NAIL_BITS);
                p0 >>= GMP_NAIL_BITS;
                cy = CBool(h < p0);
                h = (h - p0) & GMP_NUMB_MASK;
                qp[i] = h;
                h = h - p1 - cy;
            }

            return h;
        }

        static void SUBC_LIMB(out mp_limb_t cout, out mp_limb_t w, mp_limb_t x, mp_limb_t y) {
            mp_limb_t __x = (x);
            mp_limb_t __y = (y);
            mp_limb_t __w = __x - __y;
            (w) = __w;
            (cout) = CBool(__w > __x);
        }

        /* Multiplicative inverse of 3, modulo 2^GMP_NUMB_BITS.
           Eg. 0xAAAAAAAB for 32 bits, 0xAAAAAAAAAAAAAAAB for 64 bits.
           GMP_NUMB_MAX/3*2+1 is right when GMP_NUMB_BITS is even, but when it's odd
           we need to start from GMP_NUMB_MAX>>1. */
        const mp_limb_t MODLIMB_INVERSE_3 = (((GMP_NUMB_MAX >> (GMP_NUMB_BITS % 2)) / 3) * 2 + 1);
        const mp_limb_t GMP_NUMB_HIGHBIT = ((mp_limb_t)(1) << (GMP_NUMB_BITS - 1));
        /* ceil(GMP_NUMB_MAX/3) and ceil(2*GMP_NUMB_MAX/3).
           These expressions work because GMP_NUMB_MAX%3 != 0 for all GMP_NUMB_BITS. */
        const mp_limb_t GMP_NUMB_CEIL_MAX_DIV3 = (GMP_NUMB_MAX / 3 + 1);
        const mp_limb_t GMP_NUMB_CEIL_2MAX_DIV3 = ((GMP_NUMB_MAX >> 1) / 3 + 1 + GMP_NUMB_HIGHBIT);

        static
       mp_limb_t mpn_divexact_by3(mp_ptr rp, mp_srcptr up, mp_size_t un) {
            // return (3 & mpn_bdiv_dbm1(dst, src, size, (mp_limb_t) (GMP_NUMB_MASK / 3)));

            mp_limb_t c = 0;
            mp_limb_t l, q, s;
            mp_size_t i;

            ASSERT(un >= 1);
            ASSERT(c == 0 || c == 1 || c == 2);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, un));

            i = 0;
            do {
                s = up[i];
                SUBC_LIMB(out c, out l, s, c);

                q = (l * MODLIMB_INVERSE_3) & GMP_NUMB_MASK;
                rp[i] = q;

                c += CBool(q >= GMP_NUMB_CEIL_MAX_DIV3);
                c += CBool(q >= GMP_NUMB_CEIL_2MAX_DIV3);
            }
            while (++i < un);

            ASSERT(c == 0 || c == 1 || c == 2);
            return c;
        }
        static
        mp_limb_t
      mpn_divexact_by3c(mp_ptr rp, mp_srcptr up, mp_size_t un, mp_limb_t c) {
            //mp_limb_t r;
            //r = mpn_bdiv_dbm1c(rp, up, un, GMP_NUMB_MASK / 3, GMP_NUMB_MASK / 3 * c);

            ///* Possible bdiv_dbm1 return values are C * (GMP_NUMB_MASK / 3), 0 <= C < 3.
            //   We want to return C.  We compute the remainder mod 4 and notice that the
            //   inverse of (2^(2k)-1)/3 mod 4 is 1.  */
            //return r & 3;


            mp_limb_t l, q, s;
            mp_size_t i;

            ASSERT(un >= 1);
            ASSERT(c == 0 || c == 1 || c == 2);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, un));

            i = 0;
            do {
                s = up[i];
                SUBC_LIMB(out c, out l, s, c);

                q = (l * MODLIMB_INVERSE_3) & GMP_NUMB_MASK;
                rp[i] = q;

                c += CBool(q >= GMP_NUMB_CEIL_MAX_DIV3);
                c += CBool(q >= GMP_NUMB_CEIL_2MAX_DIV3);
            }
            while (++i < un);

            ASSERT(c == 0 || c == 1 || c == 2);
            return c;
        }

        static
        void
        mpn_toom_interpolate_5pts(mp_ptr c, mp_ptr v2, mp_ptr vm1,
                       mp_size_t k, mp_size_t twor, int sa,
                       mp_limb_t vinf0) {
            mp_limb_t cy, saved;
            mp_size_t twok;
            mp_size_t kk1;
            mp_ptr c1, v1, c3, vinf;

            twok = k + k;
            kk1 = twok + 1;

            c1 = c + k;
            v1 = c1 + k;
            c3 = v1 + k;
            vinf = c3 + k;

            var v0 = (c);
            /* (1) v2 <- v2-vm1 < v2+|vm1|,       (16 8 4 2 1) - (1 -1 1 -1  1) =
               thus 0 <= v2 < 50*B^(2k) < 2^6*B^(2k)             (15 9 3  3  0)
            */
            if (0 != sa)
                ASSERT_NOCARRY(mpn_add_n(v2, v2, vm1, kk1));
            else
                ASSERT_NOCARRY(mpn_sub_n(v2, v2, vm1, kk1));

            /* {c,2k} {c+2k,2k+1} {c+4k+1,2r-1} {t,2k+1} {t+2k+1,2k+1} {t+4k+2,2r}
                 v0       v1       hi(vinf)       |vm1|     v2-vm1      EMPTY */

            ASSERT_NOCARRY(mpn_divexact_by3(v2, v2, kk1));    /* v2 <- v2 / 3 */
            /* (5 3 1 1 0)*/

            /* {c,2k} {c+2k,2k+1} {c+4k+1,2r-1} {t,2k+1} {t+2k+1,2k+1} {t+4k+2,2r}
                 v0       v1      hi(vinf)       |vm1|     (v2-vm1)/3    EMPTY */

            /* (2) vm1 <- tm1 := (v1 - vm1) / 2  [(1 1 1 1 1) - (1 -1 1 -1 1)] / 2 =
               tm1 >= 0                                         (0  1 0  1 0)
               No carry comes out from {v1, kk1} +/- {vm1, kk1},
               and the division by two is exact.
               If (sa!=0) the sign of vm1 is negative */
            if (0 != sa) {

                mpn_rsh1add_n(vm1, v1, vm1, kk1);

            } else {

                mpn_rsh1sub_n(vm1, v1, vm1, kk1);

            }

            /* {c,2k} {c+2k,2k+1} {c+4k+1,2r-1} {t,2k+1} {t+2k+1,2k+1} {t+4k+2,2r}
                 v0       v1        hi(vinf)       tm1     (v2-vm1)/3    EMPTY */

            /* (3) v1 <- t1 := v1 - v0    (1 1 1 1 1) - (0 0 0 0 1) = (1 1 1 1 0)
               t1 >= 0
            */
            vinf[0] -= mpn_sub_n(v1, v1, c, twok);

            /* {c,2k} {c+2k,2k+1} {c+4k+1,2r-1} {t,2k+1} {t+2k+1,2k+1} {t+4k+2,2r}
                 v0     v1-v0        hi(vinf)       tm1     (v2-vm1)/3    EMPTY */

            /* (4) v2 <- t2 := ((v2-vm1)/3-t1)/2 = (v2-vm1-3*t1)/6
               t2 >= 0                  [(5 3 1 1 0) - (1 1 1 1 0)]/2 = (2 1 0 0 0)
            */

            mpn_rsh1sub_n(v2, v2, v1, kk1);


            /* {c,2k} {c+2k,2k+1} {c+4k+1,2r-1} {t,2k+1} {t+2k+1,2k+1} {t+4k+2,2r}
                 v0     v1-v0        hi(vinf)     tm1    (v2-vm1-3t1)/6    EMPTY */

            /* (5) v1 <- t1-tm1           (1 1 1 1 0) - (0 1 0 1 0) = (1 0 1 0 0)
               result is v1 >= 0
            */
            ASSERT_NOCARRY(mpn_sub_n(v1, v1, vm1, kk1));

            /* We do not need to read the value in vm1, so we add it in {c+k, ...} */
            cy = mpn_add_n(c1, c1, vm1, kk1);
            MPN_INCR_U(c3 + 1, twor + k - 1, cy); /* 2n-(3k+1) = 2r+k-1 */
            /* Memory allocated for vm1 is now free, it can be recycled ...*/

            /* (6) v2 <- v2 - 2*vinf,     (2 1 0 0 0) - 2*(1 0 0 0 0) = (0 1 0 0 0)
               result is v2 >= 0 */
            saved = vinf[0];       /* Remember v1's highest byte (will be overwritten). */
            vinf[0] = vinf0;       /* Set the right value for vinf0                     */
#if __defined__HAVE_NATIVE_mpn_sublsh1_n_ip1
            cy = mpn_sublsh1_n_ip1(v2, vinf, twor);
#else
            /* Overwrite unused vm1 */
            cy = mpn_lshift(vm1, vinf, twor, 1);
            cy += mpn_sub_n(v2, v2, vm1, twor);
#endif
            MPN_DECR_U(v2 + twor, kk1 - twor, cy);

            /* Current matrix is
               [1 0 0 0 0; vinf
                0 1 0 0 0; v2
                1 0 1 0 0; v1
                0 1 0 1 0; vm1
                0 0 0 0 1] v0
               Some values already are in-place (we added vm1 in the correct position)
               | vinf|  v1 |  v0 |
                    | vm1 |
               One still is in a separated area
              | +v2 |
               We have to compute v1-=vinf; vm1 -= v2,
                 |-vinf|
                    | -v2 |
               Carefully reordering operations we can avoid to compute twice the sum
               of the high half of v2 plus the low half of vinf.
            */

            /* Add the high half of t2 in {vinf} */
            if (LIKELY(twor > k + 1)) { /* This is the expected flow  */
                cy = mpn_add_n(vinf, vinf, v2 + k, k + 1);
                MPN_INCR_U(c3 + kk1, twor - k - 1, cy); /* 2n-(5k+1) = 2r-k-1 */
            } else { /* triggered only by very unbalanced cases like
	      (k+k+(k-2))x(k+k+1) , should be handled by toom32 */
                ASSERT_NOCARRY(mpn_add_n(vinf, vinf, v2 + k, twor));
            }
            /* (7) v1 <- v1 - vinf,       (1 0 1 0 0) - (1 0 0 0 0) = (0 0 1 0 0)
               result is >= 0 */
            /* Side effect: we also subtracted (high half) vm1 -= v2 */
            cy = mpn_sub_n(v1, v1, vinf, twor);          /* vinf is at most twor long.  */
            vinf0 = vinf[0];                     /* Save again the right value for vinf0 */
            vinf[0] = saved;
            MPN_DECR_U(v1 + twor, kk1 - twor, cy);       /* Treat the last bytes.       */

            /* (8) vm1 <- vm1-v2          (0 1 0 1 0) - (0 1 0 0 0) = (0 0 0 1 0)
               Operate only on the low half.
            */
            cy = mpn_sub_n(c1, c1, v2, k);
            MPN_DECR_U(v1, kk1, cy);

            /********************* Beginning the final phase **********************/

            /* Most of the recomposition was done */

            /* add t2 in {c+3k, ...}, but only the low half */
            cy = mpn_add_n(c3, c3, v2, k);
            vinf[0] += cy;
            ASSERT(vinf[0] >= cy); /* No carry */
            MPN_INCR_U(vinf, twor, vinf0); /* Add vinf0, propagate carry. */


        }

        /* Check that the nail parts are zero. */
        static
         void ASSERT_LIMB(mp_limb_t limb) {
            mp_limb_t __nail = (limb) & GMP_NAIL_MASK;
            ASSERT(__nail == 0);
        }
        static void ASSERT_MPN(mp_srcptr ptr, mp_size_t size) {
            /* let whole loop go dead when no nails */
            if (GMP_NAIL_BITS != 0) {
                mp_size_t __i;
                for (__i = 0; __i < (size); __i++)
                    ASSERT_LIMB((ptr)[__i]);
            }
        }


        public static
        mp_limb_t
        mpn_pi1_bdiv_q_1(mp_ptr rp, mp_srcptr up, mp_size_t n, mp_limb_t d,
                  mp_limb_t di, int shift) {
            mp_size_t i;
            mp_limb_t c, h, l, u, u_next, dummy;

            ASSERT(n >= 1);
            ASSERT(d != 0);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT_MPN(up, n);
            ASSERT_LIMB(d);

            d <<= GMP_NAIL_BITS;

            if (shift != 0) {
                c = 0;

                u = up[0];
                rp--;
                for (i = 1; i < n; i++) {
                    u_next = up[i];
                    u = ((u >> shift) | (u_next << (GMP_NUMB_BITS - shift))) & GMP_NUMB_MASK;

                    SUBC_LIMB(out c, out l, u, c);

                    l = (l * di) & GMP_NUMB_MASK;
                    rp[i] = l;

                    umul_ppmm(out h, out dummy, l, d);
                    c += h;
                    u = u_next;
                }

                u = u >> shift;
                l = u - c;
                l = (l * di) & GMP_NUMB_MASK;
                rp[i] = l;
            } else {
                u = up[0];
                l = (u * di) & GMP_NUMB_MASK;
                rp[0] = l;
                c = 0;

                for (i = 1; i < n; i++) {
                    umul_ppmm(out h, out dummy, l, d);
                    c += h;

                    u = up[i];
                    SUBC_LIMB(out c, out l, u, c);

                    l = (l * di) & GMP_NUMB_MASK;
                    rp[i] = l;
                }
            }

            return c;
        }

        /* binvert_limb_table[i] is the multiplicative inverse of 2*i+1 mod 256,
        ie. (binvert_limb_table[i] * (2*i+1)) % 256 == 1 */

        static readonly byte[] binvert_limb_table = new byte[128] {

  0x01, 0xAB, 0xCD, 0xB7, 0x39, 0xA3, 0xC5, 0xEF,
  0xF1, 0x1B, 0x3D, 0xA7, 0x29, 0x13, 0x35, 0xDF,
  0xE1, 0x8B, 0xAD, 0x97, 0x19, 0x83, 0xA5, 0xCF,
  0xD1, 0xFB, 0x1D, 0x87, 0x09, 0xF3, 0x15, 0xBF,
  0xC1, 0x6B, 0x8D, 0x77, 0xF9, 0x63, 0x85, 0xAF,
  0xB1, 0xDB, 0xFD, 0x67, 0xE9, 0xD3, 0xF5, 0x9F,
  0xA1, 0x4B, 0x6D, 0x57, 0xD9, 0x43, 0x65, 0x8F,
  0x91, 0xBB, 0xDD, 0x47, 0xC9, 0xB3, 0xD5, 0x7F,
  0x81, 0x2B, 0x4D, 0x37, 0xB9, 0x23, 0x45, 0x6F,
  0x71, 0x9B, 0xBD, 0x27, 0xA9, 0x93, 0xB5, 0x5F,
  0x61, 0x0B, 0x2D, 0x17, 0x99, 0x03, 0x25, 0x4F,
  0x51, 0x7B, 0x9D, 0x07, 0x89, 0x73, 0x95, 0x3F,
  0x41, 0xEB, 0x0D, 0xF7, 0x79, 0xE3, 0x05, 0x2F,
  0x31, 0x5B, 0x7D, 0xE7, 0x69, 0x53, 0x75, 0x1F,
  0x21, 0xCB, 0xED, 0xD7, 0x59, 0xC3, 0xE5, 0x0F,
  0x11, 0x3B, 0x5D, 0xC7, 0x49, 0x33, 0x55, 0xFF
        };
        /* binvert_limb() sets inv to the multiplicative inverse of n modulo
           2^GMP_NUMB_BITS, ie. satisfying inv*n == 1 mod 2^GMP_NUMB_BITS.
           n must be odd (otherwise such an inverse doesn't exist).

           This is not to be confused with invert_limb(), which is completely
           different.

           The table lookup gives an inverse with the low 8 bits valid, and each
           multiply step doubles the number of bits.  See Jebelean "An algorithm for
           exact division" end of section 4 (reference in gmp.texi).

           Possible enhancement: Could use UHWtype until the last step, if half-size
           multiplies are faster (might help under _LONG_LONG_LIMB).

           Alternative: As noted in Granlund and Montgomery "Division by Invariant
           Integers using Multiplication" (reference in gmp.texi), n itself gives a
           3-bit inverse immediately, and could be used instead of a table lookup.
           A 4-bit inverse can be obtained effectively from xoring bits 1 and 2 into
           bit 3, for instance with (((n + 2) & 4) << 1) ^ n.  */








        static void binvert_limb(out mp_limb_t inv, mp_limb_t n) {
            mp_limb_t __n = (n);
            mp_limb_t __inv;
            ASSERT((__n & 1) == 1);

            __inv = binvert_limb_table[(__n / 2) & 0x7F]; /*  8 */
            if (GMP_NUMB_BITS > 8) __inv = 2 * __inv - __inv * __inv * __n;
            if (GMP_NUMB_BITS > 16) __inv = 2 * __inv - __inv * __inv * __n;
            if (GMP_NUMB_BITS > 32) __inv = 2 * __inv - __inv * __inv * __n;

            if (GMP_NUMB_BITS > 64) {
                int __invbits = 64;
                do {
                    __inv = 2 * __inv - __inv * __inv * __n;
                    __invbits *= 2;
                } while (__invbits < GMP_NUMB_BITS);
            }

            ASSERT((__inv * __n & GMP_NUMB_MASK) == 1);
            (inv) = __inv & GMP_NUMB_MASK;
        }

        public static
        mp_limb_t
        mpn_bdiv_q_1(mp_ptr rp, mp_srcptr up, mp_size_t n, mp_limb_t d) {
            mp_limb_t di;
            int shift;

            ASSERT(n >= 1);
            ASSERT(d != 0);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT_MPN(up, n);
            ASSERT_LIMB(d);

            if ((d & 1) == 0) {
                count_trailing_zeros(out shift, d);
                d >>= shift;
            } else
                shift = 0;

            binvert_limb(out di, d);
            return mpn_pi1_bdiv_q_1(rp, up, n, d, di, shift);
        }


        static void count_trailing_zeros(out int count, mp_limb_t x) {
#if NETCOREAPP3_1 || NETCOREAPP3_0
            count = System.Numerics.BitOperations.TrailingZeroCount(x);
#else
            count = BinaryNumerals.CountTrailingZeros(x);
#endif
        }

        const mp_limb_t BINVERT_3 = MODLIMB_INVERSE_3;

        const mp_limb_t BINVERT_9 =
        ((unchecked(((GMP_NUMB_MAX / 9) << (6 - GMP_NUMB_BITS % 6)) * 8) & GMP_NUMB_MAX) | 0x39);

        const mp_limb_t BINVERT_15 =
  ((unchecked(((GMP_NUMB_MAX >> (GMP_NUMB_BITS % 4)) / 15) * 14 * 16) & GMP_NUMB_MAX) + 15);

        /* For the various mpn_divexact_byN here, fall back to using either
           mpn_pi1_bdiv_q_1 or mpn_divexact_1.  The former has less overhead and is
           many faster if it is native.  For now, since mpn_divexact_1 is native on
           several platforms where mpn_pi1_bdiv_q_1 does not yet exist, do not use
           mpn_pi1_bdiv_q_1 unconditionally.  FIXME.  */


        static
void mpn_divexact_by9(mp_ptr dst, mp_srcptr src, mp_size_t size) {
            mpn_pi1_bdiv_q_1(dst, src, size, 9, BINVERT_9, 0);
            //mpn_divexact_1(dst,src,size,9);
        }


        static
void mpn_divexact_by15(mp_ptr dst, mp_srcptr src, mp_size_t size) {
            mpn_pi1_bdiv_q_1(dst, src, size, 15, BINVERT_15, 0);
            //mpn_divexact_1(dst,src,size,15);
        }



        /* Interpolation for toom4, using the evaluation points 0, infinity,
           1, -1, 2, -2, 1/2. More precisely, we want to compute
           f(2^(GMP_NUMB_BITS * n)) for a polynomial f of degree 6, given the
           seven values

             w0 = f(0),
             w1 = f(-2),
             w2 = f(1),
             w3 = f(-1),
             w4 = f(2)
             w5 = 64 * f(1/2)
             w6 = limit at infinity of f(x) / x^6,

           The result is 6*n + w6n limbs. At entry, w0 is stored at {rp, 2n },
           w2 is stored at { rp + 2n, 2n+1 }, and w6 is stored at { rp + 6n,
           w6n }. The other values are 2n + 1 limbs each (with most
           significant limbs small). f(-1) and f(-1/2) may be negative, signs
           determined by the flag bits. Inputs are destroyed.

           Needs (2*n + 1) limbs of temporary storage.
        */
        static
        void
    mpn_toom_interpolate_7pts(mp_ptr rp, mp_size_t n, toom7_flags flags,
               mp_ptr w1, mp_ptr w3, mp_ptr w4, mp_ptr w5,
               mp_size_t w6n, mp_ptr tp) {
            mp_size_t m;
            mp_limb_t cy;

            m = 2 * n + 1;
            var w0 = rp;
            var w2 = (rp + 2 * n);
            var w6 = (rp + 6 * n);

            ASSERT(w6n > 0);
            ASSERT(w6n <= 2 * n);

            /* Using formulas similar to Marco Bodrato's

               W5 = W5 + W4
               W1 =(W4 - W1)/2
               W4 = W4 - W0
               W4 =(W4 - W1)/4 - W6*16
               W3 =(W2 - W3)/2
               W2 = W2 - W3

               W5 = W5 - W2*65      May be negative.
               W2 = W2 - W6 - W0
               W5 =(W5 + W2*45)/2   Now >= 0 again.
               W4 =(W4 - W2)/3
               W2 = W2 - W4

               W1 = W5 - W1         May be negative.
               W5 =(W5 - W3*8)/9
               W3 = W3 - W5
               W1 =(W1/15 + W5)/2   Now >= 0 again.
               W5 = W5 - W1

               where W0 = f(0), W1 = f(-2), W2 = f(1), W3 = f(-1),
                 W4 = f(2), W5 = f(1/2), W6 = f(oo),

               Note that most intermediate results are positive; the ones that
               may be negative are represented in two's complement. We must
               never shift right a value that may be negative, since that would
               invalidate the sign bit. On the other hand, divexact by odd
               numbers work fine with two's complement.
            */

            mpn_add_n(w5, w5, w4, m);
            if (0 != (flags & toom7_flags.toom7_w1_neg)) {
                mpn_rsh1add_n(w1, w1, w4, m);
            } else {
                mpn_rsh1sub_n(w1, w4, w1, m);
            }
            mpn_sub(w4, w4, m, w0, 2 * n);
            mpn_sub_n(w4, w4, w1, m); ASSERT(0 == (w4[0] & 3));
            mpn_rshift(w4, w4, m, 2); /* w4>=0 */

            tp[w6n] = mpn_lshift(tp, w6, w6n, 4);
            mpn_sub(w4, w4, m, tp, w6n + 1);

            if (0 != (flags & toom7_flags.toom7_w3_neg)) {

                mpn_rsh1add_n(w3, w3, w2, m);
            } else {

                mpn_rsh1sub_n(w3, w2, w3, m);

            }

            mpn_sub_n(w2, w2, w3, m);

            mpn_submul_1(w5, w2, m, 65);
            mpn_sub(w2, w2, m, w6, w6n);
            mpn_sub(w2, w2, m, w0, 2 * n);

            mpn_addmul_1(w5, w2, m, 45); ASSERT(0 == (w5[0] & 1));
            mpn_rshift(w5, w5, m, 1);
            mpn_sub_n(w4, w4, w2, m);

            mpn_divexact_by3(w4, w4, m);
            mpn_sub_n(w2, w2, w4, m);

            mpn_sub_n(w1, w5, w1, m);
            mpn_lshift(tp, w3, m, 3);
            mpn_sub_n(w5, w5, tp, m);
            mpn_divexact_by9(w5, w5, m);
            mpn_sub_n(w3, w3, w5, m);

            mpn_divexact_by15(w1, w1, m);
            mpn_add_n(w1, w1, w5, m); ASSERT(0 == (w1[0] & 1));
            mpn_rshift(w1, w1, m, 1); /* w1>=0 now */
            mpn_sub_n(w5, w5, w1, m);

            /* These bounds are valid for the 4x4 polynomial product of toom44,
             * and they are conservative for toom53 and toom62. */
            ASSERT(w1[2 * n] < 2);
            ASSERT(w2[2 * n] < 3);
            ASSERT(w3[2 * n] < 4);
            ASSERT(w4[2 * n] < 3);
            ASSERT(w5[2 * n] < 2);

            /* Addition chain. Note carries and the 2n'th limbs that need to be
             * added in.
             *
             * Special care is needed for w2[2n] and the corresponding carry,
             * since the "simple" way of adding it all together would overwrite
             * the limb at wp[2*n] and rp[4*n] (same location) with the sum of
             * the high half of w3 and the low half of w4.
             *
             *         7    6    5    4    3    2    1    0
             *    |    |    |    |    |    |    |    |    |
             *                  ||w3 (2n+1)|
             *             ||w4 (2n+1)|
             *        ||w5 (2n+1)|        ||w1 (2n+1)|
             *  + | w6 (w6n)|        ||w2 (2n+1)| w0 (2n) |  (share storage with r)
             *  -----------------------------------------------
             *  r |    |    |    |    |    |    |    |    |
             *        c7   c6   c5   c4   c3                 Carries to propagate
             */

            cy = mpn_add_n(rp + n, rp + n, w1, m);
            MPN_INCR_U(w2 + n + 1, n, cy);
            cy = mpn_add_n(rp + 3 * n, rp + 3 * n, w3, n);
            MPN_INCR_U(w3 + n, n + 1, w2[2 * n] + cy);
            cy = mpn_add_n(rp + 4 * n, w3 + n, w4, n);
            MPN_INCR_U(w4 + n, n + 1, w3[2 * n] + cy);
            cy = mpn_add_n(rp + 5 * n, w4 + n, w5, n);
            MPN_INCR_U(w5 + n, n + 1, w4[2 * n] + cy);
            if (w6n > n + 1) {
                cy = mpn_add_n(rp + 6 * n, rp + 6 * n, w5 + n, n + 1);
                MPN_INCR_U(rp + 7 * n + 1, w6n - n - 1, cy);
            } else {
                ASSERT_NOCARRY(mpn_add_n(rp + 6 * n, rp + 6 * n, w5 + n, w6n));
#if WANT_ASSERT
      {
	mp_size_t i;
	for (i = w6n; i <= n; i++)
	  ASSERT (w5[n + i] == 0);
      }
#endif
            }
        }




        /* Evaluate in: -1, 0, +1, +2, +inf

          <-s--><--n--><--n-->
           ____ ______ ______
          |_a2_|___a1_|___a0_|
           |b2_|___b1_|___b0_|
           <-t-><--n--><--n-->

          v0  =  a0         * b0          #   A(0)*B(0)
          v1  = (a0+ a1+ a2)*(b0+ b1+ b2) #   A(1)*B(1)      ah  <= 2  bh <= 2
          vm1 = (a0- a1+ a2)*(b0- b1+ b2) #  A(-1)*B(-1)    |ah| <= 1  bh <= 1
          v2  = (a0+2a1+4a2)*(b0+2b1+4b2) #   A(2)*B(2)      ah  <= 6  bh <= 6
          vinf=          a2 *         b2  # A(inf)*B(inf)
        */

        /* FIXME: TOOM33_MUL_N_REC is not quite right for a balanced
           multiplication at the infinity point. We may have
           MAYBE_mul_basecase == 0, and still get s just below
           MUL_TOOM22_THRESHOLD. If MUL_TOOM33_THRESHOLD == 7, we can even get
           s == 1 and mpn_toom22_mul will crash.
        */

        private static void TOOM33_MUL_N_REC(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n, mp_ptr ws) {
            if (BELOW_THRESHOLD(n, MUL_TOOM22_THRESHOLD))
                mpn_mul_basecase(p, a, n, b, n);
            else if (BELOW_THRESHOLD(n, MUL_TOOM33_THRESHOLD))
                mpn_toom22_mul(p, a, n, b, n, ws);
            else
                mpn_toom33_mul(p, a, n, b, n, ws);
        }
        static
        void
        mpn_toom33_mul(mp_ptr pp,
        mp_srcptr ap, mp_size_t an,
        mp_srcptr bp, mp_size_t bn,
        mp_ptr scratch) {

            mp_size_t n, s, t;
            int vm1_neg;
            mp_limb_t cy, vinf0;
            mp_ptr gp;
            mp_ptr as1, asm1, as2;
            mp_ptr bs1, bsm1, bs2;


            n = (an + 2) / (size_t)3;


            var a0 = ap;
            var a1 = (ap + n);
            var a2 = (ap + 2 * n);
            var b0 = bp;
            var b1 = (bp + n);
            var b2 = (bp + 2 * n);

            s = an - 2 * n;
            t = bn - 2 * n;

            ASSERT(an >= bn);

            ASSERT(0 < s && s <= n);
            ASSERT(0 < t && t <= n);

            as1 = scratch + 4 * n + 4;
            asm1 = scratch + 2 * n + 2;
            as2 = pp + n + 1;

            bs1 = pp;
            bsm1 = scratch + 3 * n + 3; /* we need 4n+4 <= 4n+s+t */
            bs2 = pp + 2 * n + 2;

            gp = scratch;

            vm1_neg = 0;

            /* Compute as1 and asm1.  */
            cy = mpn_add(gp, a0, n, a2, s);

            if (cy == 0 && mpn_cmp(gp, a1, n) < 0) {
                cy = mpn_add_n_sub_n(as1, asm1, a1, gp, n);
                as1[n] = cy >> 1;
                asm1[n] = 0;
                vm1_neg = 1;
            } else {
                mp_limb_t cy2;
                cy2 = mpn_add_n_sub_n(as1, asm1, gp, a1, n);
                as1[n] = cy + (cy2 >> 1);
                asm1[n] = cy - (cy2 & 1);
            }

            /* Compute as2.  */

            cy = mpn_add_n(as2, a2, as1, s);
            if (s != n)
                cy = mpn_add_1(as2 + s, as1 + s, n - s, cy);
            cy += as1[n];
            cy = 2 * cy + mpn_rsblsh1_n(as2, a0, as2, n);

            as2[n] = cy;

            /* Compute bs1 and bsm1.  */
            cy = mpn_add(gp, b0, n, b2, t);

            if (cy == 0 && mpn_cmp(gp, b1, n) < 0) {
                cy = mpn_add_n_sub_n(bs1, bsm1, b1, gp, n);
                bs1[n] = cy >> 1;
                bsm1[n] = 0;
                vm1_neg ^= 1;
            } else {
                mp_limb_t cy2;
                cy2 = mpn_add_n_sub_n(bs1, bsm1, gp, b1, n);
                bs1[n] = cy + (cy2 >> 1);
                bsm1[n] = cy - (cy2 & 1);
            }

            /* Compute bs2.  */

            cy = mpn_add_n(bs2, b2, bs1, t);
            if (t != n)
                cy = mpn_add_1(bs2 + t, bs1 + t, n - t, cy);
            cy += bs1[n];
            cy = 2 * cy + mpn_rsblsh1_n(bs2, b0, bs2, n);

            bs2[n] = cy;

            ASSERT(as1[n] <= 2);
            ASSERT(bs1[n] <= 2);
            ASSERT(asm1[n] <= 1);
            ASSERT(bsm1[n] <= 1);
            ASSERT(as2[n] <= 6);
            ASSERT(bs2[n] <= 6);

            var v0 = pp             /* 2n */;
            var v1 = (pp + 2 * n)           /* 2n+1 */;
            var vinf = (pp + 4 * n)         /* s+t */;
            var vm1 = scratch               /* 2n+1 */;
            var v2 = (scratch + 2 * n + 1)      /* 2n+2 */;
            var scratch_out = (scratch + 5 * n + 5);

            /* vm1, 2n+1 limbs */
            TOOM33_MUL_N_REC(vm1, asm1, bsm1, n, scratch_out);
            cy = 0;
            if (asm1[n] != 0)
                cy = bsm1[n] + mpn_add_n(vm1 + n, vm1 + n, bsm1, n);
            if (bsm1[n] != 0)
                cy += mpn_add_n(vm1 + n, vm1 + n, asm1, n);
            vm1[2 * n] = cy;


            TOOM33_MUL_N_REC(v2, as2, bs2, n + 1, scratch_out); /* v2, 2n+1 limbs */

            /* vinf, s+t limbs */
            if (s > t) mpn_mul(vinf, a2, s, b2, t);
            else TOOM33_MUL_N_REC(vinf, a2, b2, s, scratch_out);

            vinf0 = vinf[0];                /* v1 overlaps with this */

            /* v1, 2n+1 limbs */
            TOOM33_MUL_N_REC(v1, as1, bs1, n, scratch_out);
            if (as1[n] == 1) {
                cy = bs1[n] + mpn_add_n(v1 + n, v1 + n, bs1, n);
            } else if (as1[n] != 0) {
                cy = 2 * bs1[n] + mpn_addlsh1_n(v1 + n, v1 + n, bs1, n);
            } else
                cy = 0;
            if (bs1[n] == 1) {
                cy += mpn_add_n(v1 + n, v1 + n, as1, n);
            } else if (bs1[n] != 0) {
                cy += mpn_addlsh1_n(v1 + n, v1 + n, as1, n);
            }
            v1[2 * n] = cy;


            TOOM33_MUL_N_REC(v0, ap, bp, n, scratch_out);   /* v0, 2n limbs */

            mpn_toom_interpolate_5pts(pp, v2, vm1, n, s + t, vm1_neg, vinf0);
        }


        /* Evaluate in: -1, 0, +1, +2, +inf

          <-s-><--n--><--n--><--n-->
           ___ ______ ______ ______
          |a3_|___a2_|___a1_|___a0_|
                   |_b1_|___b0_|
                   <-t--><--n-->

          v0  =  a0             * b0      #   A(0)*B(0)
          v1  = (a0+ a1+ a2+ a3)*(b0+ b1) #   A(1)*B(1)      ah  <= 3  bh <= 1
          vm1 = (a0- a1+ a2- a3)*(b0- b1) #  A(-1)*B(-1)    |ah| <= 1  bh  = 0
          v2  = (a0+2a1+4a2+8a3)*(b0+2b1) #   A(2)*B(2)      ah  <= 14 bh <= 2
          vinf=              a3 *     b1  # A(inf)*B(inf)
        */
        static
        void TOOM42_MUL_N_REC(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n, mp_ptr ws) {
            mpn_mul_n(p, a, b, n);
        }
        static
        void
        mpn_toom42_mul(mp_ptr pp,
        mp_srcptr ap, mp_size_t an,
        mp_srcptr bp, mp_size_t bn,
        mp_ptr scratch) {
            mp_size_t n, s, t;
            int vm1_neg;
            mp_limb_t cy, vinf0;
            mp_ptr a0_a2;
            mp_ptr as1, asm1, as2;
            mp_ptr bs1, bsm1, bs2;
            mp_ptr tmp;
            TMP_DECL;



            n = an >= 2 * bn ? (an + 3) >> 2 : (bn + 1) >> 1;

            var a0 = ap;
            var a1 = (ap + n);
            var a2 = (ap + 2 * n);
            var a3 = (ap + 3 * n);
            var b0 = bp;
            var b1 = (bp + n);

            s = an - 3 * n;
            t = bn - n;

            ASSERT(0 < s && s <= n);
            ASSERT(0 < t && t <= n);

            TMP_MARK;

            tmp = TMP_ALLOC_LIMBS(6 * n + 5);
            as1 = tmp; tmp += n + 1;
            asm1 = tmp; tmp += n + 1;
            as2 = tmp; tmp += n + 1;
            bs1 = tmp; tmp += n + 1;
            bsm1 = tmp; tmp += n;
            bs2 = tmp; tmp += n + 1;

            a0_a2 = pp;

            /* Compute as1 and asm1.  */
            vm1_neg = mpn_toom_eval_dgr3_pm1(as1, asm1, ap, n, s, a0_a2) & 1;

            /* Compute as2.  */

            cy = mpn_addlsh1_n(as2, a2, a3, s);
            if (s != n)
                cy = mpn_add_1(as2 + s, a2 + s, n - s, cy);
            cy = 2 * cy + mpn_addlsh1_n(as2, a1, as2, n);
            cy = 2 * cy + mpn_addlsh1_n(as2, a0, as2, n);

            as2[n] = cy;

            /* Compute bs1 and bsm1.  */
            if (t == n) {

                if (mpn_cmp(b0, b1, n) < 0) {
                    cy = mpn_add_n_sub_n(bs1, bsm1, b1, b0, n);
                    vm1_neg ^= 1;
                } else {
                    cy = mpn_add_n_sub_n(bs1, bsm1, b0, b1, n);
                }
                bs1[n] = cy >> 1;

            } else {
                bs1[n] = mpn_add(bs1, b0, n, b1, t);

                if (mpn_zero_p(b0 + t, n - t) && mpn_cmp(b0, b1, t) < 0) {
                    mpn_sub_n(bsm1, b1, b0, t);
                    MPN_ZERO(bsm1 + t, n - t);
                    vm1_neg ^= 1;
                } else {
                    mpn_sub(bsm1, b0, n, b1, t);
                }
            }

            /* Compute bs2, recycling bs1. bs2=bs1+b1  */
            mpn_add(bs2, bs1, n + 1, b1, t);

            ASSERT(as1[n] <= 3);
            ASSERT(bs1[n] <= 1);
            ASSERT(asm1[n] <= 1);
            /*ASSERT (bsm1[n] == 0);*/
            ASSERT(as2[n] <= 14);
            ASSERT(bs2[n] <= 2);

            var v0 = pp             /* 2n */;
            var v1 = (pp + 2 * n)           /* 2n+1 */;
            var vinf = (pp + 4 * n)         /* s+t */;
            var vm1 = scratch               /* 2n+1 */;
            var v2 = (scratch + 2 * n + 1)      /* 2n+2 */;
            var scratch_out = scratch + 4 * n + 4   /* Currently unused. */;

            /* vm1, 2n+1 limbs */
            TOOM42_MUL_N_REC(vm1, asm1, bsm1, n, scratch_out);
            cy = 0;
            if (asm1[n] != 0)
                cy = mpn_add_n(vm1 + n, vm1 + n, bsm1, n);
            vm1[2 * n] = cy;

            TOOM42_MUL_N_REC(v2, as2, bs2, n + 1, scratch_out); /* v2, 2n+1 limbs */

            /* vinf, s+t limbs */
            if (s > t) mpn_mul(vinf, a3, s, b1, t);
            else mpn_mul(vinf, b1, t, a3, s);

            vinf0 = vinf[0];                /* v1 overlaps with this */

            /* v1, 2n+1 limbs */
            TOOM42_MUL_N_REC(v1, as1, bs1, n, scratch_out);
            if (as1[n] == 1) {
                cy = bs1[n] + mpn_add_n(v1 + n, v1 + n, bs1, n);
            } else if (as1[n] == 2) {

                cy = 2 * bs1[n] + mpn_addlsh1_n(v1 + n, v1 + n, bs1, n);

            } else if (as1[n] == 3) {
                cy = 3 * bs1[n] + mpn_addmul_1(v1 + n, bs1, n, (mp_limb_t)(3));
            } else
                cy = 0;
            if (bs1[n] != 0)
                cy += mpn_add_n(v1 + n, v1 + n, as1, n);
            v1[2 * n] = cy;

            TOOM42_MUL_N_REC(v0, ap, bp, n, scratch_out);   /* v0, 2n limbs */

            mpn_toom_interpolate_5pts(pp, v2, vm1, n, s + t, vm1_neg, vinf0);

            TMP_FREE;
        }

        static
        int
        mpn_toom_eval_dgr3_pm1(mp_ptr xp1, mp_ptr xm1,
                    mp_srcptr xp, mp_size_t n, mp_size_t x3n, mp_ptr tp) {
            int neg;

            ASSERT(x3n > 0);
            ASSERT(x3n <= n);

            xp1[n] = mpn_add_n(xp1, xp, xp + 2 * n, n);
            tp[n] = mpn_add(tp, xp + n, n, xp + 3 * n, x3n);

            neg = (mpn_cmp(xp1, tp, n + 1) < 0) ? ~0 : 0;


            if (0 != neg)
                mpn_add_n_sub_n(xp1, xm1, tp, xp1, n + 1);
            else
                mpn_add_n_sub_n(xp1, xm1, xp1, tp, n + 1);


            ASSERT(xp1[n] <= 3);
            ASSERT(xm1[n] <= 1);

            return neg;
        }

        /* Needs n+1 limbs of temporary storage. */
        static int
        mpn_toom_eval_dgr3_pm2(mp_ptr xp2, mp_ptr xm2,
                    mp_srcptr xp, mp_size_t n, mp_size_t x3n, mp_ptr tp) {
            mp_limb_t cy;
            int neg;

            ASSERT(x3n > 0);
            ASSERT(x3n <= n);

            /* (x0 + 4 * x2) +/- (2 x1 + 8 x_3) */


            xp2[n] = mpn_addlsh_n(xp2, xp, xp + 2 * n, n, 2);

            cy = mpn_addlsh_n(tp, xp + n, xp + 3 * n, x3n, 2);

            if (x3n < n)
                cy = mpn_add_1(tp + x3n, xp + n + x3n, n - x3n, cy);
            tp[n] = cy;

            mpn_lshift(tp, tp, n + 1, 1);

            neg = (mpn_cmp(xp2, tp, n + 1) < 0) ? ~0 : 0;

            if (0 != neg)
                mpn_add_n_sub_n(xp2, xm2, tp, xp2, n + 1);
            else
                mpn_add_n_sub_n(xp2, xm2, xp2, tp, n + 1);


            ASSERT(xp2[n] < 15);
            ASSERT(xm2[n] < 10);

            return neg;
        }


        /* Evaluate in: 0, +1, -1, +2, -2, 1/2, +inf

          <-s--><--n--><--n--><--n-->
           ____ ______ ______ ______
          |_a3_|___a2_|___a1_|___a0_|
           |b3_|___b2_|___b1_|___b0_|
           <-t-><--n--><--n--><--n-->

          v0  =   a0             *  b0              #    A(0)*B(0)
          v1  = ( a0+ a1+ a2+ a3)*( b0+ b1+ b2+ b3) #    A(1)*B(1)      ah  <= 3   bh  <= 3
          vm1 = ( a0- a1+ a2- a3)*( b0- b1+ b2- b3) #   A(-1)*B(-1)    |ah| <= 1  |bh| <= 1
          v2  = ( a0+2a1+4a2+8a3)*( b0+2b1+4b2+8b3) #    A(2)*B(2)      ah  <= 14  bh  <= 14
          vm2 = ( a0-2a1+4a2-8a3)*( b0-2b1+4b2-8b3) #    A(2)*B(2)      ah  <= 9  |bh| <= 9
          vh  = (8a0+4a1+2a2+ a3)*(8b0+4b1+2b2+ b3) #  A(1/2)*B(1/2)    ah  <= 14  bh  <= 14
          vinf=               a3 *          b2      #  A(inf)*B(inf)
        */


        const bool MAYBE_mul_basecase = (MUL_TOOM44_THRESHOLD < 4 * MUL_TOOM22_THRESHOLD);
        const bool MAYBE_mul_toom22 =
        (MUL_TOOM44_THRESHOLD < 4 * MUL_TOOM33_THRESHOLD);
        const bool MAYBE_mul_toom44 =
        (MUL_TOOM6H_THRESHOLD >= 4 * MUL_TOOM44_THRESHOLD);

        static void TOOM44_MUL_N_REC(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n, mp_ptr ws) {
            if (MAYBE_mul_basecase
            && BELOW_THRESHOLD(n, MUL_TOOM22_THRESHOLD))
                mpn_mul_basecase(p, a, n, b, n);
            else if (MAYBE_mul_toom22
                 && BELOW_THRESHOLD(n, MUL_TOOM33_THRESHOLD))
                mpn_toom22_mul(p, a, n, b, n, ws);
            else if (!MAYBE_mul_toom44
                 || BELOW_THRESHOLD(n, MUL_TOOM44_THRESHOLD))
                mpn_toom33_mul(p, a, n, b, n, ws);
            else
                mpn_toom44_mul(p, a, n, b, n, ws);
        }
        [FlagsAttribute()]
        enum toom7_flags { toom7_w1_neg = 1, toom7_w3_neg = 2 };

        /* Use of scratch space. In the product area, we store

              ___________________
             |vinf|____|_v1_|_v0_|
              s+t  2n-1 2n+1  2n

           The other recursive products, vm1, v2, vm2, vh are stored in the
           scratch area. When computing them, we use the product area for
           intermediate values.

           Next, we compute v1. We can store the intermediate factors at v0
           and at vh + 2n + 2.

           Finally, for v0 and vinf, factors are parts of the input operands,
           and we need scratch space only for the recursive multiplication.

           In all, if S(an) is the scratch need, the needed space is bounded by

             S(an) <= 4 (2*ceil(an/4) + 1) + 1 + S(ceil(an/4) + 1)

           which should give S(n) = 8 n/3 + c log(n) for some constant c.
        */
        static
        void
        mpn_toom44_mul(mp_ptr pp,
        mp_srcptr ap, mp_size_t an,
        mp_srcptr bp, mp_size_t bn,
        mp_ptr scratch) {
            mp_size_t n, s, t;
            mp_limb_t cy;
            toom7_flags flags;



            ASSERT(an >= bn);

            n = (an + 3) >> 2;


            var a0 = ap;
            var a1 = (ap + n);
            var a2 = (ap + 2 * n);
            var a3 = (ap + 3 * n);
            var b0 = bp;
            var b1 = (bp + n);
            var b2 = (bp + 2 * n);
            var b3 = (bp + 3 * n);


            s = an - 3 * n;
            t = bn - 3 * n;

            ASSERT(0 < s && s <= n);
            ASSERT(0 < t && t <= n);
            ASSERT(s >= t);

            /* NOTE: The multiplications to v2, vm2, vh and vm1 overwrites the
             * following limb, so these must be computed in order, and we need a
             * one limb gap to tp. */
            var v0 = pp             /* 2n */;
            var v1 = (pp + 2 * n)           /* 2n+1 */;
            var vinf = (pp + 6 * n)         /* s+t */;
            var v2 = scratch                /* 2n+1 */;
            var vm2 = (scratch + 2 * n + 1)     /* 2n+1 */;
            var vh = (scratch + 4 * n + 2)      /* 2n+1 */;
            var vm1 = (scratch + 6 * n + 3)     /* 2n+1 */;
            var tp = (scratch + 8 * n + 5);

            /* apx and bpx must not overlap with v1 */
            var apx = pp                /* n+1 */;
            var amx = (pp + n + 1)          /* n+1 */;
            var bmx = (pp + 2 * n + 2)          /* n+1 */;
            var bpx = (pp + 4 * n + 2)          /* n+1 */;

            /* Total scratch need: 8*n + 5 + scratch for recursive calls. This
               gives roughly 32 n/3 + log term. */

            /* Compute apx = a0 + 2 a1 + 4 a2 + 8 a3 and amx = a0 - 2 a1 + 4 a2 - 8 a3.  */
            flags = (toom7_flags.toom7_w1_neg & (toom7_flags)mpn_toom_eval_dgr3_pm2(apx, amx, ap, n, s, tp));

            /* Compute bpx = b0 + 2 b1 + 4 b2 + 8 b3 and bmx = b0 - 2 b1 + 4 b2 - 8 b3.  */
            flags = (flags ^ (toom7_flags.toom7_w1_neg & (toom7_flags)mpn_toom_eval_dgr3_pm2(bpx, bmx, bp, n, t, tp)));

            TOOM44_MUL_N_REC(v2, apx, bpx, n + 1, tp);    /* v2,  2n+1 limbs */
            TOOM44_MUL_N_REC(vm2, amx, bmx, n + 1, tp); /* vm2,  2n+1 limbs */

            /* Compute apx = 8 a0 + 4 a1 + 2 a2 + a3 = (((2*a0 + a1) * 2 + a2) * 2 + a3 */

            cy = mpn_addlsh1_n(apx, a1, a0, n);
            cy = 2 * cy + mpn_addlsh1_n(apx, a2, apx, n);
            if (s < n) {
                mp_limb_t cy2;
                cy2 = mpn_addlsh1_n(apx, a3, apx, s);
                apx[n] = 2 * cy + mpn_lshift(apx + s, apx + s, n - s, 1);
                MPN_INCR_U(apx + s, n + 1 - s, cy2);
            } else
                apx[n] = 2 * cy + mpn_addlsh1_n(apx, a3, apx, n);

            /* Compute bpx = 8 b0 + 4 b1 + 2 b2 + b3 = (((2*b0 + b1) * 2 + b2) * 2 + b3 */


            cy = mpn_addlsh1_n(bpx, b1, b0, n);
            cy = 2 * cy + mpn_addlsh1_n(bpx, b2, bpx, n);
            if (t < n) {
                mp_limb_t cy2;
                cy2 = mpn_addlsh1_n(bpx, b3, bpx, t);
                bpx[n] = 2 * cy + mpn_lshift(bpx + t, bpx + t, n - t, 1);
                MPN_INCR_U(bpx + t, n + 1 - t, cy2);
            } else
                bpx[n] = 2 * cy + mpn_addlsh1_n(bpx, b3, bpx, n);


            ASSERT(apx[n] < 15);
            ASSERT(bpx[n] < 15);

            TOOM44_MUL_N_REC(vh, apx, bpx, n + 1, tp);  /* vh,  2n+1 limbs */

            /* Compute apx = a0 + a1 + a2 + a3 and amx = a0 - a1 + a2 - a3.  */
            flags = (flags | (toom7_flags.toom7_w3_neg & (toom7_flags)mpn_toom_eval_dgr3_pm1(apx, amx, ap, n, s, tp)));

            /* Compute bpx = b0 + b1 + b2 + b3 and bmx = b0 - b1 + b2 - b3.  */
            flags = (flags ^ (toom7_flags.toom7_w3_neg & (toom7_flags)mpn_toom_eval_dgr3_pm1(bpx, bmx, bp, n, t, tp)));

            TOOM44_MUL_N_REC(vm1, amx, bmx, n + 1, tp);   /* vm1,  2n+1 limbs */
            /* Clobbers amx, bmx. */
            TOOM44_MUL_N_REC(v1, apx, bpx, n + 1, tp);  /* v1,  2n+1 limbs */

            TOOM44_MUL_N_REC(v0, a0, b0, n, tp);
            if (s > t)
                mpn_mul(vinf, a3, s, b3, t);
            else
                TOOM44_MUL_N_REC(vinf, a3, b3, s, tp);  /* vinf, s+t limbs */

            mpn_toom_interpolate_7pts(pp, n, flags, vm2, vm1, v2, vh, s + t, tp);
        }




        public static void mpn_mul_n(mp_ptr p, mp_srcptr a, mp_srcptr b, mp_size_t n) {
            ASSERT(n >= 1);
            ASSERT(!MPN_OVERLAP_P(p, 2 * n, a, n));
            ASSERT(!MPN_OVERLAP_P(p, 2 * n, b, n));

            if (BELOW_THRESHOLD(n, MUL_TOOM22_THRESHOLD)) {
                mpn_mul_basecase(p, a, n, b, n);
            } else if (BELOW_THRESHOLD(n, MUL_TOOM33_THRESHOLD)) {
                /* Allocate workspace of fixed size on stack: fast! */

                var ws = salloc<mp_limb_t>(mpn_toom22_mul_itch(MUL_TOOM33_THRESHOLD_LIMIT - 1,
                              MUL_TOOM33_THRESHOLD_LIMIT - 1));
                ASSERT(MUL_TOOM33_THRESHOLD <= MUL_TOOM33_THRESHOLD_LIMIT);
                mpn_toom22_mul(p, a, n, b, n, ws);
            } else if (BELOW_THRESHOLD(n, MUL_TOOM44_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom33_mul_itch(n, n));
                mpn_toom33_mul(p, a, n, b, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, MUL_TOOM6H_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom44_mul_itch(n, n));
                mpn_toom44_mul(p, a, n, b, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, MUL_TOOM8H_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom6_mul_n_itch(n));
                mpn_toom6h_mul(p, a, n, b, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, MUL_FFT_THRESHOLD)) {
                mp_ptr ws;
                TMP_DECL;
                TMP_MARK;
                ws = TMP_ALLOC_LIMBS(mpn_toom8_mul_n_itch(n));
                mpn_toom8h_mul(p, a, n, b, n, ws);
                TMP_FREE;
            } else {
                /* The current FFT code allocates its own space.  That should probably
               change.  */
                mpn_fft_mul(p, a, n, b, n);
            }
        }



        const mp_size_t MUL_BASECASE_MAX_UN = 500;

        /* Areas where the different toom algorithms can be called (extracted
           from the t-toom*.c files, and ignoring small constant offsets):

           1/6  1/5 1/4 4/13 1/3 3/8 2/5 5/11 1/2 3/5 2/3 3/4 4/5   1 vn/un
                                                4/7              6/7
                               6/11
                                               |--------------------| toom22 (small)
                                                                   || toom22 (large)
                                                               |xxxx| toom22 called
                              |-------------------------------------| toom32
                                                 |xxxxxxxxxxxxxxxx| | toom32 called
                                                       |------------| toom33
                                                                  |x| toom33 called
                     |---------------------------------|            | toom42
                          |xxxxxxxxxxxxxxxxxxxxxxxx|            | toom42 called
                                               |--------------------| toom43
                                                       |xxxxxxxxxx|   toom43 called
                 |-----------------------------|                      toom52 (unused)
                                                           |--------| toom44
                                   |xxxxxxxx| toom44 called
                                      |--------------------|        | toom53
                                                |xxxxxx|              toom53 called
            |-------------------------|                               toom62 (unused)
                                                   |----------------| toom54 (unused)
                              |--------------------|                  toom63
                                  |xxxxxxxxx|                   | toom63 called
                                  |---------------------------------| toom6h
                                   |xxxxxxxx| toom6h called
                                          |-------------------------| toom8h (32 bit)
                         |------------------------------------------| toom8h (64 bit)
                                   |xxxxxxxx| toom8h called
        */

        static bool TOOM33_OK(mp_size_t an, mp_size_t bn) {
            return (6 + 2 * an < 3 * bn);
        }

        static bool TOOM44_OK(mp_size_t an, mp_size_t bn) {
            return (12 + 3 * an < 4 * bn);
        }

        /* Multiply the natural numbers u (pointed to by UP, with UN limbs) and v
           (pointed to by VP, with VN limbs), and store the result at PRODP.  The
           result is UN + VN limbs.  Return the most significant limb of the result.

           NOTE: The space pointed to by PRODP is overwritten before finished with U
           and V, so overlap is an error.

           Argument constraints:
           1. UN >= VN.
           2. PRODP != UP and PRODP != VP, i.e. the destination must be distinct from
              the multiplier and the multiplicand.  */

        /*
          * The cutoff lines in the toomX2 and toomX3 code are now exactly between the
            ideal lines of the surrounding algorithms.  Is that optimal?

          * The toomX3 code now uses a structure similar to the one of toomX2, except
            that it loops longer in the unbalanced case.  The result is that the
            remaining area might have un < vn.  Should we fix the toomX2 code in a
            similar way?

          * The toomX3 code is used for the largest non-FFT unbalanced operands.  It
            therefore calls mpn_mul recursively for certain cases.

          * Allocate static temp space using THRESHOLD variables (except for toom44
            when !WANT_FFT).  That way, we can typically have no TMP_ALLOC at all.

          * We sort ToomX2 algorithms together, assuming the toom22, toom32, toom42
            have the same vn threshold.  This is not true, we should actually use
            mul_basecase for slightly larger operands for toom32 than for toom22, and
            even larger for toom42.

          * That problem is even more prevalent for toomX3.  We therefore use special
            THRESHOLD variables there.

          * Is our ITCH allocation correct?
        */


        public static
        mp_limb_t
        mpn_mul(mp_ptr prodp,
         mp_srcptr up, mp_size_t un,
         mp_srcptr vp, mp_size_t vn) {

            var ITCH = (16 * vn + 100);

            ASSERT(un >= vn);
            ASSERT(vn >= 1);
            ASSERT(!MPN_OVERLAP_P(prodp, un + vn, up, un));
            ASSERT(!MPN_OVERLAP_P(prodp, un + vn, vp, vn));

            if (un == vn) {
                if (up == vp)
                    mpn_sqr(prodp, up, un);
                else
                    mpn_mul_n(prodp, up, vp, un);
            } else if (vn < MUL_TOOM22_THRESHOLD) { /* plain schoolbook multiplication */

                /* Unless un is very large, or else if have an applicable mpn_mul_N,
               perform basecase multiply directly.  */
                if (un <= MUL_BASECASE_MAX_UN
        || vn == 1
        )
                    mpn_mul_basecase(prodp, up, un, vp, vn);
                else {
                    /* We have un >> MUL_BASECASE_MAX_UN > vn.  For better memory
                       locality, split up[] into MUL_BASECASE_MAX_UN pieces and multiply
                       these pieces with the vp[] operand.  After each such partial
                       multiplication (but the last) we copy the most significant vn
                       limbs into a temporary buffer since that part would otherwise be
                       overwritten by the next multiplication.  After the next
                       multiplication, we add it back.  This illustrates the situation:

                                                                  -->vn<--
                                                                    |  |<------- un ------->|
                                                                       _____________________|
                                                                      X                    /|
                                                                    /XX__________________/  |
                                                  _____________________                     |
                                                 X                    /                     |
                                               /XX__________________/                       |
                             _____________________                                          |
                            /                    /                                          |
                          /____________________/                                            |
                      ==================================================================

                      The parts marked with X are the parts whose sums are copied into
                      the temporary buffer.  */

                    var tp = salloc<mp_limb_t>(MUL_TOOM22_THRESHOLD_LIMIT);
                    mp_limb_t cy;
                    ASSERT(MUL_TOOM22_THRESHOLD <= MUL_TOOM22_THRESHOLD_LIMIT);

                    mpn_mul_basecase(prodp, up, MUL_BASECASE_MAX_UN, vp, vn);
                    prodp += MUL_BASECASE_MAX_UN;
                    MPN_COPY(tp, prodp, vn);        /* preserve high triangle */
                    up += MUL_BASECASE_MAX_UN;
                    un -= MUL_BASECASE_MAX_UN;
                    while (un > MUL_BASECASE_MAX_UN) {
                        mpn_mul_basecase(prodp, up, MUL_BASECASE_MAX_UN, vp, vn);
                        cy = mpn_add_n(prodp, prodp, tp, vn); /* add back preserved triangle */
                        mpn_incr_u(prodp + vn, cy);
                        prodp += MUL_BASECASE_MAX_UN;
                        MPN_COPY(tp, prodp, vn);        /* preserve high triangle */
                        up += MUL_BASECASE_MAX_UN;
                        un -= MUL_BASECASE_MAX_UN;
                    }
                    if (un > vn) {
                        mpn_mul_basecase(prodp, up, un, vp, vn);
                    } else {
                        ASSERT(un > 0);
                        mpn_mul_basecase(prodp, vp, vn, up, un);
                    }
                    cy = mpn_add_n(prodp, prodp, tp, vn); /* add back preserved triangle */
                    mpn_incr_u(prodp + vn, cy);
                }
            } else if (BELOW_THRESHOLD(vn, MUL_TOOM33_THRESHOLD)) {
                /* Use ToomX2 variants */
                mp_ptr scratch;
                TMP_SDECL;
                TMP_SMARK;

                {
                    var ITCH_TOOMX2 = (9 * vn / 2 + GMP_NUMB_BITS * 2);
                    scratch = TMP_SALLOC_LIMBS(ITCH_TOOMX2);
                    ASSERT(mpn_toom22_mul_itch((5 * vn - 1) / 4, vn) <= ITCH_TOOMX2); /* 5vn/2+ */
                    ASSERT(mpn_toom32_mul_itch((7 * vn - 1) / 4, vn) <= ITCH_TOOMX2); /* 7vn/6+ */
                    ASSERT(mpn_toom42_mul_itch(3 * vn - 1, vn) <= ITCH_TOOMX2); /* 9vn/2+ */
                }

                /* FIXME: This condition (repeated in the loop below) leaves from a vn*vn
               square to a (3vn-1)*vn rectangle.  Leaving such a rectangle is hardly
               wise; we would get better balance by slightly moving the bound.  We
               will sometimes end up with un < vn, like in the X3 arm below.  */
                if (un >= 3 * vn) {
                    mp_limb_t cy;
                    mp_ptr ws;

                    /* The maximum ws usage is for the mpn_mul result.  */
                    ws = TMP_SALLOC_LIMBS(4 * vn);

                    mpn_toom42_mul(prodp, up, 2 * vn, vp, vn, scratch);
                    un -= 2 * vn;
                    up += 2 * vn;
                    prodp += 2 * vn;

                    while (un >= 3 * vn) {
                        mpn_toom42_mul(ws, up, 2 * vn, vp, vn, scratch);
                        un -= 2 * vn;
                        up += 2 * vn;
                        cy = mpn_add_n(prodp, prodp, ws, vn);
                        MPN_COPY(prodp + vn, ws + vn, 2 * vn);
                        mpn_incr_u(prodp + vn, cy);
                        prodp += 2 * vn;
                    }

                    /* vn <= un < 3vn */

                    if (4 * un < 5 * vn)
                        mpn_toom22_mul(ws, up, un, vp, vn, scratch);
                    else if (4 * un < 7 * vn)
                        mpn_toom32_mul(ws, up, un, vp, vn, scratch);
                    else
                        mpn_toom42_mul(ws, up, un, vp, vn, scratch);

                    cy = mpn_add_n(prodp, prodp, ws, vn);
                    MPN_COPY(prodp + vn, ws + vn, un);
                    mpn_incr_u(prodp + vn, cy);
                } else {
                    if (4 * un < 5 * vn)
                        mpn_toom22_mul(prodp, up, un, vp, vn, scratch);
                    else if (4 * un < 7 * vn)
                        mpn_toom32_mul(prodp, up, un, vp, vn, scratch);
                    else
                        mpn_toom42_mul(prodp, up, un, vp, vn, scratch);
                }
                TMP_SFREE;
            } else if (BELOW_THRESHOLD((un + vn) >> 1, MUL_FFT_THRESHOLD) ||
                   BELOW_THRESHOLD(3 * vn, MUL_FFT_THRESHOLD)) {
                /* Handle the largest operands that are not in the FFT range.  The 2nd
               condition makes very unbalanced operands avoid the FFT code (except
               perhaps as coefficient products of the Toom code.  */

                if (BELOW_THRESHOLD(vn, MUL_TOOM44_THRESHOLD) || !TOOM44_OK(un, vn)) {
                    /* Use ToomX3 variants */
                    mp_ptr scratch;
                    TMP_DECL;
                    TMP_MARK;

                    {
                        var ITCH_TOOMX3 = (4 * vn + GMP_NUMB_BITS);
                        scratch = TMP_ALLOC_LIMBS(ITCH_TOOMX3);
                        ASSERT(mpn_toom33_mul_itch((7 * vn - 1) / 6, vn) <= ITCH_TOOMX3); /* 7vn/2+ */
                        ASSERT(mpn_toom43_mul_itch((3 * vn - 1) / 2, vn) <= ITCH_TOOMX3); /* 9vn/4+ */
                        ASSERT(mpn_toom32_mul_itch((7 * vn - 1) / 4, vn) <= ITCH_TOOMX3); /* 7vn/6+ */
                        ASSERT(mpn_toom53_mul_itch((11 * vn - 1) / 6, vn) <= ITCH_TOOMX3); /* 11vn/3+ */
                        ASSERT(mpn_toom42_mul_itch((5 * vn - 1) / 2, vn) <= ITCH_TOOMX3); /* 15vn/4+ */
                        ASSERT(mpn_toom63_mul_itch((5 * vn - 1) / 2, vn) <= ITCH_TOOMX3); /* 15vn/4+ */
                    }

                    if (2 * un >= 5 * vn) {
                        mp_limb_t cy;
                        mp_ptr ws;

                        /* The maximum ws usage is for the mpn_mul result.  */
                        ws = TMP_ALLOC_LIMBS(7 * vn >> 1);

                        if (BELOW_THRESHOLD(vn, MUL_TOOM42_TO_TOOM63_THRESHOLD))
                            mpn_toom42_mul(prodp, up, 2 * vn, vp, vn, scratch);
                        else
                            mpn_toom63_mul(prodp, up, 2 * vn, vp, vn, scratch);
                        un -= 2 * vn;
                        up += 2 * vn;
                        prodp += 2 * vn;

                        while (2 * un >= 5 * vn)    /* un >= 2.5vn */
                      {
                            if (BELOW_THRESHOLD(vn, MUL_TOOM42_TO_TOOM63_THRESHOLD))
                                mpn_toom42_mul(ws, up, 2 * vn, vp, vn, scratch);
                            else
                                mpn_toom63_mul(ws, up, 2 * vn, vp, vn, scratch);
                            un -= 2 * vn;
                            up += 2 * vn;
                            cy = mpn_add_n(prodp, prodp, ws, vn);
                            MPN_COPY(prodp + vn, ws + vn, 2 * vn);
                            mpn_incr_u(prodp + vn, cy);
                            prodp += 2 * vn;
                        }

                        /* vn / 2 <= un < 2.5vn */

                        if (un < vn)
                            mpn_mul(ws, vp, vn, up, un);
                        else
                            mpn_mul(ws, up, un, vp, vn);

                        cy = mpn_add_n(prodp, prodp, ws, vn);
                        MPN_COPY(prodp + vn, ws + vn, un);
                        mpn_incr_u(prodp + vn, cy);
                    } else {
                        if (6 * un < 7 * vn)
                            mpn_toom33_mul(prodp, up, un, vp, vn, scratch);
                        else if (2 * un < 3 * vn) {
                            if (BELOW_THRESHOLD(vn, MUL_TOOM32_TO_TOOM43_THRESHOLD))
                                mpn_toom32_mul(prodp, up, un, vp, vn, scratch);
                            else
                                mpn_toom43_mul(prodp, up, un, vp, vn, scratch);
                        } else if (6 * un < 11 * vn) {
                            if (4 * un < 7 * vn) {
                                if (BELOW_THRESHOLD(vn, MUL_TOOM32_TO_TOOM53_THRESHOLD))
                                    mpn_toom32_mul(prodp, up, un, vp, vn, scratch);
                                else
                                    mpn_toom53_mul(prodp, up, un, vp, vn, scratch);
                            } else {
                                if (BELOW_THRESHOLD(vn, MUL_TOOM42_TO_TOOM53_THRESHOLD))
                                    mpn_toom42_mul(prodp, up, un, vp, vn, scratch);
                                else
                                    mpn_toom53_mul(prodp, up, un, vp, vn, scratch);
                            }
                        } else {
                            if (BELOW_THRESHOLD(vn, MUL_TOOM42_TO_TOOM63_THRESHOLD))
                                mpn_toom42_mul(prodp, up, un, vp, vn, scratch);
                            else
                                mpn_toom63_mul(prodp, up, un, vp, vn, scratch);
                        }
                    }
                    TMP_FREE;
                } else {
                    mp_ptr scratch;
                    TMP_DECL;
                    TMP_MARK;

                    if (BELOW_THRESHOLD(vn, MUL_TOOM6H_THRESHOLD)) {
                        scratch = TMP_SALLOC_LIMBS(mpn_toom44_mul_itch(un, vn));
                        mpn_toom44_mul(prodp, up, un, vp, vn, scratch);
                    } else if (BELOW_THRESHOLD(vn, MUL_TOOM8H_THRESHOLD)) {
                        scratch = TMP_SALLOC_LIMBS(mpn_toom6h_mul_itch(un, vn));
                        mpn_toom6h_mul(prodp, up, un, vp, vn, scratch);
                    } else {
                        scratch = TMP_ALLOC_LIMBS(mpn_toom8h_mul_itch(un, vn));
                        mpn_toom8h_mul(prodp, up, un, vp, vn, scratch);
                    }
                    TMP_FREE;
                }
            } else {
                if (un >= 8 * vn) {
                    mp_limb_t cy;
                    mp_ptr ws;
                    TMP_DECL;
                    TMP_MARK;

                    /* The maximum ws usage is for the mpn_mul result.  */
                    ws = TMP_BALLOC_LIMBS(9 * vn >> 1);

                    mpn_fft_mul(prodp, up, 3 * vn, vp, vn);
                    un -= 3 * vn;
                    up += 3 * vn;
                    prodp += 3 * vn;

                    while (2 * un >= 7 * vn)    /* un >= 3.5vn  */
                      {
                        mpn_fft_mul(ws, up, 3 * vn, vp, vn);
                        un -= 3 * vn;
                        up += 3 * vn;
                        cy = mpn_add_n(prodp, prodp, ws, vn);
                        MPN_COPY(prodp + vn, ws + vn, 3 * vn);
                        mpn_incr_u(prodp + vn, cy);
                        prodp += 3 * vn;
                    }

                    /* vn / 2 <= un < 3.5vn */

                    if (un < vn)
                        mpn_mul(ws, vp, vn, up, un);
                    else
                        mpn_mul(ws, up, un, vp, vn);

                    cy = mpn_add_n(prodp, prodp, ws, vn);
                    MPN_COPY(prodp + vn, ws + vn, un);
                    mpn_incr_u(prodp + vn, cy);

                    TMP_FREE;
                } else
                    mpn_fft_mul(prodp, up, un, vp, vn);
            }

            return prodp[un + vn - 1];  /* historic */
        }




        static
        void
        mpn_sqr(mp_ptr p, mp_srcptr a, mp_size_t n) {
            ASSERT(n >= 1);
            ASSERT(!MPN_OVERLAP_P(p, 2 * n, a, n));

            if (BELOW_THRESHOLD(n, SQR_BASECASE_THRESHOLD)) { /* mul_basecase is faster than sqr_basecase on small sizes sometimes */
                mpn_mul_basecase(p, a, n, a, n);
            } else if (BELOW_THRESHOLD(n, SQR_TOOM2_THRESHOLD)) {
                mpn_sqr_basecase(p, a, n);
            } else if (BELOW_THRESHOLD(n, SQR_TOOM3_THRESHOLD)) {
                /* Allocate workspace of fixed size on stack: fast! */
                var ws = salloc<mp_limb_t>(mpn_toom2_sqr_itch(SQR_TOOM3_THRESHOLD_LIMIT - 1));
                ASSERT(SQR_TOOM3_THRESHOLD <= SQR_TOOM3_THRESHOLD_LIMIT);
                mpn_toom2_sqr(p, a, n, ws);
            } else if (BELOW_THRESHOLD(n, SQR_TOOM4_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom3_sqr_itch(n));
                mpn_toom3_sqr(p, a, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, SQR_TOOM6_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom4_sqr_itch(n));
                mpn_toom4_sqr(p, a, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, SQR_TOOM8_THRESHOLD)) {
                mp_ptr ws;
                TMP_SDECL;
                TMP_SMARK;
                ws = TMP_SALLOC_LIMBS(mpn_toom6_sqr_itch(n));
                mpn_toom6_sqr(p, a, n, ws);
                TMP_SFREE;
            } else if (BELOW_THRESHOLD(n, SQR_FFT_THRESHOLD)) {
                mp_ptr ws;
                TMP_DECL;
                TMP_MARK;
                ws = TMP_ALLOC_LIMBS(mpn_toom8_sqr_itch(n));
                mpn_toom8_sqr(p, a, n, ws);
                TMP_FREE;
            } else {
                /* The current FFT code allocates its own space.  That should probably
               change.  */
                mpn_fft_mul(p, a, n, a, n);
            }
        }

        static void
        mpn_fft_mul(mp_ptr pp,
                    mp_srcptr ap, mp_size_t an,
                    mp_srcptr bp, mp_size_t bn) {
            mpn_nussbaumer_mul(pp, ap, an, bp, bn);
        }

        /* Multiply {ap,an} by {bp,bn}, and put the result in {pp, an+bn} */
        static void
        mpn_nussbaumer_mul(mp_ptr pp,
                    mp_srcptr ap, mp_size_t an,
                    mp_srcptr bp, mp_size_t bn) {
            mp_size_t rn;
            mp_ptr tp;
            TMP_DECL;

            ASSERT(an >= bn);
            ASSERT(bn > 0);

            TMP_MARK;

            if ((ap == bp) && (an == bn)) {
                rn = mpn_sqrmod_bnm1_next_size(2 * an);
                tp = TMP_ALLOC_LIMBS(mpn_sqrmod_bnm1_itch(rn, an));
                mpn_sqrmod_bnm1(pp, rn, ap, an, tp);
            } else {
                rn = mpn_mulmod_bnm1_next_size(an + bn);
                tp = TMP_ALLOC_LIMBS(mpn_mulmod_bnm1_itch(rn, an, bn));
                mpn_mulmod_bnm1(pp, rn, ap, an, bp, bn, tp);
            }

            TMP_FREE;
        }



        static mp_size_t
        mpn_mulmod_bnm1_itch(mp_size_t rn, mp_size_t an, mp_size_t bn) {
            mp_size_t n, itch;
            n = rn >> 1;
            itch = rn + 4 +
              (an > n ? (bn > n ? rn : n) : 0);
            return itch;
        }


        static mp_size_t
        mpn_sqrmod_bnm1_itch(mp_size_t rn, mp_size_t an) {
            mp_size_t n, itch;
            n = rn >> 1;
            itch = rn + 3 +
              (an > n ? an : 0);
            return itch;
        }



        /* Input is {ap,rn}; output is {rp,rn}, computation is
           mod B^rn - 1, and values are semi-normalised; zero is represented
           as either 0 or B^n - 1.  Needs a scratch of 2rn limbs at tp.
           tp==rp is allowed. */
        static void
        mpn_bc_sqrmod_bnm1(mp_ptr rp, mp_srcptr ap, mp_size_t rn, mp_ptr tp) {
            mp_limb_t cy;

            ASSERT(0 < rn);

            mpn_sqr(tp, ap, rn);
            cy = mpn_add_n(rp, tp, tp + rn, rn);
            /* If cy == 1, then the value of rp is at most B^rn - 2, so there can
             * be no overflow when adding in the carry. */
            MPN_INCR_U(rp, rn, cy);
        }


        /* Input is {ap,rn+1}; output is {rp,rn+1}, in
           semi-normalised representation, computation is mod B^rn + 1. Needs
           a scratch area of 2rn + 2 limbs at tp; tp == rp is allowed.
           Output is normalised. */
        static void
        mpn_bc_sqrmod_bnp1(mp_ptr rp, mp_srcptr ap, mp_size_t rn, mp_ptr tp) {
            mp_limb_t cy;

            ASSERT(0 < rn);

            mpn_sqr(tp, ap, rn + 1);
            ASSERT(tp[2 * rn + 1] == 0);
            ASSERT(tp[2 * rn] < GMP_NUMB_MAX);
            cy = tp[2 * rn] + mpn_sub_n(rp, tp, tp + rn, rn);
            rp[rn] = 0;
            MPN_INCR_U(rp, rn + 1, cy);
        }


        /* Computes {rp,MIN(rn,2an)} <- {ap,an}^2 Mod(B^rn-1)
         *
         * The result is expected to be ZERO if and only if the operand
         * already is. Otherwise the class [0] Mod(B^rn-1) is represented by
         * B^rn-1.
         * It should not be a problem if sqrmod_bnm1 is used to
         * compute the full square with an <= 2*rn, because this condition
         * implies (B^an-1)^2 < (B^rn-1) .
         *
         * Requires rn/4 < an <= rn
         * Scratch need: rn/2 + (need for recursive call OR rn + 3). This gives
         *
         * S(n) <= rn/2 + MAX (rn + 4, S(n/2)) <= 3/2 rn + 4
         */
        static void
        mpn_sqrmod_bnm1(mp_ptr rp, mp_size_t rn, mp_srcptr ap, mp_size_t an, mp_ptr tp) {
            ASSERT(0 < an);
            ASSERT(an <= rn);

            if ((rn & 1) != 0 || BELOW_THRESHOLD(rn, SQRMOD_BNM1_THRESHOLD)) {
                if (UNLIKELY(an < rn)) {
                    if (UNLIKELY(2 * an <= rn)) {
                        mpn_sqr(rp, ap, an);
                    } else {
                        mp_limb_t cy;
                        mpn_sqr(tp, ap, an);
                        cy = mpn_add(rp, tp, rn, tp + rn, 2 * an - rn);
                        MPN_INCR_U(rp, rn, cy);
                    }
                } else
                    mpn_bc_sqrmod_bnm1(rp, ap, rn, tp);
            } else {
                mp_size_t n;
                mp_limb_t cy;
                mp_limb_t hi;

                n = rn >> 1;

                ASSERT(2 * an > n);

                /* Compute xm = a^2 mod (B^n - 1), xp = a^2 mod (B^n + 1)
               and crt together as

               x = -xp * B^n + (B^n + 1) * [ (xp + xm)/2 mod (B^n-1)]
                */

                var a0 = ap;
                var a1 = (ap + n);

                var xp = tp /* 2n + 2 */
                                /* am1  maybe in {xp, n} */;
                var sp1 = (tp + 2 * n + 2)
                                /* ap1  maybe in {sp1, n + 1} */;

                {
                    mp_srcptr am1;
                    mp_size_t anm;
                    mp_ptr so;

                    if (LIKELY(an > n)) {
                        so = xp + n;
                        am1 = xp;
                        cy = mpn_add(xp, a0, n, a1, an - n);
                        MPN_INCR_U(xp, n, cy);
                        anm = n;
                    } else {
                        so = xp;
                        am1 = a0;
                        anm = an;
                    }

                    mpn_sqrmod_bnm1(rp, n, am1, anm, so);
                }

                {
                    int k;
                    mp_srcptr ap1;
                    mp_size_t anp;

                    if (LIKELY(an > n)) {
                        ap1 = sp1;
                        cy = mpn_sub(sp1, a0, n, a1, an - n);
                        sp1[n] = 0;
                        MPN_INCR_U(sp1, n + 1, cy);
                        anp = (int)((uint)n + ap1[n]);
                    } else {
                        ap1 = a0;
                        anp = an;
                    }

                    if (BELOW_THRESHOLD(n, MUL_FFT_MODF_THRESHOLD))
                        k = 0;
                    else {
                        int mask;
                        k = mpn_fft_best_k(n, 1);
                        mask = (1 << k) - 1;
                        while (0 != (n & mask)) { k--; mask >>= 1; };
                    }
                    if (k >= FFT_FIRST_K)
                        xp[n] = mpn_mul_fft(xp, n, ap1, anp, ap1, anp, k);
                    else if (UNLIKELY(ap1 == a0)) {
                        ASSERT(anp <= n);
                        ASSERT(2 * anp > n);
                        mpn_sqr(xp, a0, an);
                        anp = 2 * an - n;
                        cy = mpn_sub(xp, xp, n, xp + n, anp);
                        xp[n] = 0;
                        MPN_INCR_U(xp, n + 1, cy);
                    } else
                        mpn_bc_sqrmod_bnp1(xp, ap1, n, xp);
                }

                /* Here the CRT recomposition begins.

               xm <- (xp + xm)/2 = (xp + xm)B^n/2 mod (B^n-1)
               Division by 2 is a bitwise rotation.

               Assumes xp normalised mod (B^n+1).

               The residue class [0] is represented by [B^n-1]; except when
               both input are ZERO.
                */


                //#if HAVE_NATIVE_mpn_rsh1add_nc
                //      cy = mpn_rsh1add_nc(rp, rp, xp, n, xp[n]); /* B^n = 1 */
                //      hi = cy << (GMP_NUMB_BITS - 1);
                //      cy = 0;
                //      /* next update of rp[n-1] will set cy = 1 only if rp[n-1]+=hi
                //	 overflows, i.e. a further increment will not overflow again. */
                //#else /* ! _nc */
                cy = xp[n] + mpn_rsh1add_n(rp, rp, xp, n); /* B^n = 1 */
                hi = (cy << (GMP_NUMB_BITS - 1)) & GMP_NUMB_MASK; /* (cy&1) << ... */
                cy >>= 1;
                /* cy = 1 only if xp[n] = 1 i.e. {xp,n} = ZERO, this implies that
               the rsh1add was a simple rshift: the top bit is 0. cy=1 => hi=0. */
                // #endif

                add_ssaaaa(out cy, out rp[n - 1], cy, rp[n - 1], (mp_limb_t)(0), hi);



                ASSERT(cy <= 1);
                /* Next increment can not overflow, read the previous comments about cy. */
                ASSERT((cy == 0) || ((rp[n - 1] & GMP_NUMB_HIGHBIT) == 0));
                MPN_INCR_U(rp, n, cy);

                /* Compute the highest half:
               ([(xp + xm)/2 mod (B^n-1)] - xp ) * B^n
                 */
                if (UNLIKELY(2 * an < rn)) {
                    /* Note that in this case, the only way the result can equal
                       zero mod B^{rn} - 1 is if the input is zero, and
                       then the output of both the recursive calls and this CRT
                       reconstruction is zero, not B^{rn} - 1. */
                    cy = mpn_sub_n(rp + n, rp, xp, 2 * an - n);

                    /* FIXME: This subtraction of the high parts is not really
                       necessary, we do it to get the carry out, and for sanity
                       checking. */
                    cy = xp[n] + mpn_sub_nc(xp + 2 * an - n, rp + 2 * an - n,
                                 xp + 2 * an - n, rn - 2 * an, cy);
                    ASSERT(mpn_zero_p(xp + 2 * an - n + 1, rn - 1 - 2 * an));
                    cy = mpn_sub_1(rp, rp, 2 * an, cy);
                    ASSERT(cy == (xp + 2 * an - n)[0]);
                } else {
                    cy = xp[n] + mpn_sub_n(rp + n, rp, xp, n);
                    /* cy = 1 only if {xp,n+1} is not ZERO, i.e. {rp,n} is not ZERO.
                       DECR will affect _at most_ the lowest n limbs. */
                    MPN_DECR_U(rp, 2 * n, cy);
                }

            }
        }



        /* Inputs are {ap,rn} and {bp,rn}; output is {rp,rn}, computation is
           mod B^rn - 1, and values are semi-normalised; zero is represented
           as either 0 or B^n - 1.  Needs a scratch of 2rn limbs at tp.
           tp==rp is allowed. */
        static void
        mpn_bc_mulmod_bnm1(mp_ptr rp, mp_srcptr ap, mp_srcptr bp, mp_size_t rn,
                    mp_ptr tp) {
            mp_limb_t cy;

            ASSERT(0 < rn);

            mpn_mul_n(tp, ap, bp, rn);
            cy = mpn_add_n(rp, tp, tp + rn, rn);
            /* If cy == 1, then the value of rp is at most B^rn - 2, so there can
             * be no overflow when adding in the carry. */
            MPN_INCR_U(rp, rn, cy);
        }


        /* Inputs are {ap,rn+1} and {bp,rn+1}; output is {rp,rn+1}, in
           semi-normalised representation, computation is mod B^rn + 1. Needs
           a scratch area of 2rn + 2 limbs at tp; tp == rp is allowed.
           Output is normalised. */
        static void
        mpn_bc_mulmod_bnp1(mp_ptr rp, mp_srcptr ap, mp_srcptr bp, mp_size_t rn,
                    mp_ptr tp) {
            mp_limb_t cy;

            ASSERT(0 < rn);

            mpn_mul_n(tp, ap, bp, rn + 1);
            ASSERT(tp[2 * rn + 1] == 0);
            ASSERT(tp[2 * rn] < GMP_NUMB_MAX);
            cy = tp[2 * rn] + mpn_sub_n(rp, tp, tp + rn, rn);
            rp[rn] = 0;
            MPN_INCR_U(rp, rn + 1, cy);
        }


        /* Computes {rp,MIN(rn,an+bn)} <- {ap,an}*{bp,bn} Mod(B^rn-1)
         *
         * The result is expected to be ZERO if and only if one of the operand
         * already is. Otherwise the class [0] Mod(B^rn-1) is represented by
         * B^rn-1. This should not be a problem if mulmod_bnm1 is used to
         * combine results and obtain a natural number when one knows in
         * advance that the final value is less than (B^rn-1).
         * Moreover it should not be a problem if mulmod_bnm1 is used to
         * compute the full product with an+bn <= rn, because this condition
         * implies (B^an-1)(B^bn-1) < (B^rn-1) .
         *
         * Requires 0 < bn <= an <= rn and an + bn > rn/2
         * Scratch need: rn + (need for recursive call OR rn + 4). This gives
         *
         * S(n) <= rn + MAX (rn + 4, S(n/2)) <= 2rn + 4
         */
        static void
        mpn_mulmod_bnm1(mp_ptr rp, mp_size_t rn, mp_srcptr ap, mp_size_t an, mp_srcptr bp, mp_size_t bn, mp_ptr tp) {
            ASSERT(0 < bn);
            ASSERT(bn <= an);
            ASSERT(an <= rn);

            if ((rn & 1) != 0 || BELOW_THRESHOLD(rn, MULMOD_BNM1_THRESHOLD)) {
                if (UNLIKELY(bn < rn)) {
                    if (UNLIKELY(an + bn <= rn)) {
                        mpn_mul(rp, ap, an, bp, bn);
                    } else {
                        mp_limb_t cy;
                        mpn_mul(tp, ap, an, bp, bn);
                        cy = mpn_add(rp, tp, rn, tp + rn, an + bn - rn);
                        MPN_INCR_U(rp, rn, cy);
                    }
                } else
                    mpn_bc_mulmod_bnm1(rp, ap, bp, rn, tp);
            } else {
                mp_size_t n;
                mp_limb_t cy;
                mp_limb_t hi;

                n = rn >> 1;

                /* We need at least an + bn >= n, to be able to fit one of the
               recursive products at rp. Requiring strict inequality makes
               the code slightly simpler. If desired, we could avoid this
               restriction by initially halving rn as long as rn is even and
               an + bn <= rn/2. */

                ASSERT(an + bn > n);

                /* Compute xm = a*b mod (B^n - 1), xp = a*b mod (B^n + 1)
               and crt together as

               x = -xp * B^n + (B^n + 1) * [ (xp + xm)/2 mod (B^n-1)]
                */

                var a0 = ap;
                var a1 = (ap + n);
                var b0 = bp;
                var b1 = (bp + n);

                var xp = tp /* 2n + 2 */
                                /* am1  maybe in {xp, n} */
                                /* bm1  maybe in {xp + n, n} */;
                var sp1 = (tp + 2 * n + 2)
                                /* ap1  maybe in {sp1, n + 1} */
                                /* bp1  maybe in {sp1 + n + 1, n + 1} */;

                {
                    mp_srcptr am1, bm1;
                    mp_size_t anm, bnm;
                    mp_ptr so;

                    bm1 = b0;
                    bnm = bn;
                    if (LIKELY(an > n)) {
                        am1 = xp;
                        cy = mpn_add(xp, a0, n, a1, an - n);
                        MPN_INCR_U(xp, n, cy);
                        anm = n;
                        so = xp + n;
                        if (LIKELY(bn > n)) {
                            bm1 = so;
                            cy = mpn_add(so, b0, n, b1, bn - n);
                            MPN_INCR_U(so, n, cy);
                            bnm = n;
                            so += n;
                        }
                    } else {
                        so = xp;
                        am1 = a0;
                        anm = an;
                    }

                    mpn_mulmod_bnm1(rp, n, am1, anm, bm1, bnm, so);
                }

                {
                    int k;
                    mp_srcptr ap1, bp1;
                    mp_size_t anp, bnp;

                    bp1 = b0;
                    bnp = bn;
                    if (LIKELY(an > n)) {
                        ap1 = sp1;
                        cy = mpn_sub(sp1, a0, n, a1, an - n);
                        sp1[n] = 0;
                        MPN_INCR_U(sp1, n + 1, cy);
                        anp = (int)((uint)n + ap1[n]);
                        if (LIKELY(bn > n)) {
                            bp1 = sp1 + n + 1;
                            cy = mpn_sub(sp1 + n + 1, b0, n, b1, bn - n);
                            sp1[2 * n + 1] = 0;
                            MPN_INCR_U(sp1 + n + 1, n + 1, cy);
                            bnp = (int)((uint)n + bp1[n]);
                        }
                    } else {
                        ap1 = a0;
                        anp = an;
                    }

                    if (BELOW_THRESHOLD(n, MUL_FFT_MODF_THRESHOLD))
                        k = 0;
                    else {
                        int mask;
                        k = mpn_fft_best_k(n, 0);
                        mask = (1 << k) - 1;
                        while (0 != (n & mask)) { k--; mask >>= 1; };
                    }
                    if (k >= FFT_FIRST_K)
                        xp[n] = mpn_mul_fft(xp, n, ap1, anp, bp1, bnp, k);
                    else if (UNLIKELY(bp1 == b0)) {
                        ASSERT(anp + bnp <= 2 * n + 1);
                        ASSERT(anp + bnp > n);
                        ASSERT(anp >= bnp);
                        mpn_mul(xp, ap1, anp, bp1, bnp);
                        anp = anp + bnp - n;
                        ASSERT(anp <= n || xp[2 * n] == 0);
                        anp -= (int)CBool(anp > n);
                        cy = mpn_sub(xp, xp, n, xp + n, anp);
                        xp[n] = 0;
                        MPN_INCR_U(xp, n + 1, cy);
                    } else
                        mpn_bc_mulmod_bnp1(xp, ap1, bp1, n, xp);
                }

                /* Here the CRT recomposition begins.

               xm <- (xp + xm)/2 = (xp + xm)B^n/2 mod (B^n-1)
               Division by 2 is a bitwise rotation.

               Assumes xp normalised mod (B^n+1).

               The residue class [0] is represented by [B^n-1]; except when
               both input are ZERO.
                */


                //#if HAVE_NATIVE_mpn_rsh1add_nc
                //      cy = mpn_rsh1add_nc(rp, rp, xp, n, xp[n]); /* B^n = 1 */
                //      hi = cy << (GMP_NUMB_BITS - 1);
                //      cy = 0;
                //      /* next update of rp[n-1] will set cy = 1 only if rp[n-1]+=hi
                //	 overflows, i.e. a further increment will not overflow again. */
                //#else /* ! _nc */
                cy = xp[n] + mpn_rsh1add_n(rp, rp, xp, n); /* B^n = 1 */
                hi = (cy << (GMP_NUMB_BITS - 1)) & GMP_NUMB_MASK; /* (cy&1) << ... */
                cy >>= 1;
                /* cy = 1 only if xp[n] = 1 i.e. {xp,n} = ZERO, this implies that
               the rsh1add was a simple rshift: the top bit is 0. cy=1 => hi=0. */
                //#endif

                add_ssaaaa(out cy, out rp[n - 1], cy, rp[n - 1], 0, hi);


                ASSERT(cy <= 1);
                /* Next increment can not overflow, read the previous comments about cy. */
                ASSERT((cy == 0) || ((rp[n - 1] & GMP_NUMB_HIGHBIT) == 0));
                MPN_INCR_U(rp, n, cy);

                /* Compute the highest half:
               ([(xp + xm)/2 mod (B^n-1)] - xp ) * B^n
                 */
                if (UNLIKELY(an + bn < rn)) {
                    /* Note that in this case, the only way the result can equal
                       zero mod B^{rn} - 1 is if one of the inputs is zero, and
                       then the output of both the recursive calls and this CRT
                       reconstruction is zero, not B^{rn} - 1. Which is good,
                       since the latter representation doesn't fit in the output
                       area.*/
                    cy = mpn_sub_n(rp + n, rp, xp, an + bn - n);

                    /* FIXME: This subtraction of the high parts is not really
                       necessary, we do it to get the carry out, and for sanity
                       checking. */
                    cy = xp[n] + mpn_sub_nc(xp + an + bn - n, rp + an + bn - n,
                                 xp + an + bn - n, rn - (an + bn), cy);
                    ASSERT(an + bn == rn - 1 ||
                        mpn_zero_p(xp + an + bn - n + 1, rn - 1 - (an + bn)));
                    cy = mpn_sub_1(rp, rp, an + bn, cy);
                    ASSERT(cy == (xp + an + bn - n)[0]);
                } else {
                    cy = xp[n] + mpn_sub_n(rp + n, rp, xp, n);
                    /* cy = 1 only if {xp,n+1} is not ZERO, i.e. {rp,n} is not ZERO.
                       DECR will affect _at most_ the lowest n limbs. */
                    MPN_DECR_U(rp, 2 * n, cy);
                }

            }
        }

        static mp_size_t
        mpn_mulmod_bnm1_next_size(mp_size_t n) {
            mp_size_t nh;

            if (BELOW_THRESHOLD(n, MULMOD_BNM1_THRESHOLD))
                return n;
            if (BELOW_THRESHOLD(n, 4 * (MULMOD_BNM1_THRESHOLD - 1) + 1))
                return (n + (2 - 1)) & (-2);
            if (BELOW_THRESHOLD(n, 8 * (MULMOD_BNM1_THRESHOLD - 1) + 1))
                return (n + (4 - 1)) & (-4);

            nh = (n + 1) >> 1;

            if (BELOW_THRESHOLD(nh, MUL_FFT_MODF_THRESHOLD))
                return (n + (8 - 1)) & (-8);

            return 2 * mpn_fft_next_size(nh, mpn_fft_best_k(nh, 0));
        }




        /* If this machine has no inline assembler, use C macros.  */

        static void add_ssaaaa(out mp_limb_t sh, out mp_limb_t sl, mp_limb_t ah, mp_limb_t al, mp_limb_t bh, mp_limb_t bl) {
            mp_limb_t __x;
            __x = (al) + (bl);
            (sh) = (ah) + (bh) + CBool(__x < (al));
            (sl) = __x;
        }

        static void sub_ddmmss(out mp_limb_t sh, out mp_limb_t sl, mp_limb_t ah, mp_limb_t al, mp_limb_t bh, mp_limb_t bl) {
            mp_limb_t __x;
            __x = (al) - (bl);
            (sh) = (ah) - (bh) - CBool((al) < (bl));
            (sl) = __x;
        }


        static
        mp_size_t
        mpn_sqrmod_bnm1_next_size(mp_size_t n) {
            mp_size_t nh;

            if (BELOW_THRESHOLD(n, SQRMOD_BNM1_THRESHOLD))
                return n;
            if (BELOW_THRESHOLD(n, 4 * (SQRMOD_BNM1_THRESHOLD - 1) + 1))
                return (n + (2 - 1)) & (-2);
            if (BELOW_THRESHOLD(n, 8 * (SQRMOD_BNM1_THRESHOLD - 1) + 1))
                return (n + (4 - 1)) & (-4);

            nh = (n + 1) >> 1;

            if (BELOW_THRESHOLD(nh, SQR_FFT_MODF_THRESHOLD))
                return (n + (8 - 1)) & (-8);

            return 2 * mpn_fft_next_size(nh, mpn_fft_best_k(nh, 1));
        }


        struct fft_table_nk {

            internal UInt32 value_;

            internal mp_size_t n {

                get {
                    return unchecked((mp_size_t)((((UInt32)1 << 27) - 1) & value_));
                }

                set {
                    value_ = (~(((UInt32)1 << 27) - 1) & value_) | unchecked((UInt32)value);
                }
            }

            internal mp_size_t k {

                get {
                    return unchecked((mp_size_t)(value_ >> 27));
                }

                set {
                    value_ = ((((UInt32)1 << 27) - 1) & value_) | (unchecked((UInt32)value) << 27);
                }
            }
        };


        static mp_limb_t mpn_mul_fft_internal(mp_ptr, mp_size_t, int, mp_ptr*,
                               mp_ptr*, mp_ptr, mp_ptr, mp_size_t,
                               mp_size_t, mp_size_t, int**, mp_ptr, int);
        static void mpn_mul_fft_decompose(mp_ptr, mp_ptr*, mp_size_t, mp_size_t, mp_srcptr,
                           mp_size_t, mp_size_t, mp_size_t, mp_ptr);


        /* Find the best k to use for a mod 2^(m*GMP_NUMB_BITS)+1 FFT for m >= n.
           We have sqr=0 if for a multiply, sqr=1 for a square.
           There are three generations of this code; we keep the old ones as long as
           some gmp-mparam.h is not updated.  */


        /*****************************************************************************/

        //#if TUNE_PROGRAM_BUILD || (defined (MUL_FFT_TABLE3) && defined (SQR_FFT_TABLE3))

        //# ifndef FFT_TABLE3_SIZE		/* When tuning this is defined in gmp-impl.h */
        //#if defined (MUL_FFT_TABLE3_SIZE) && defined (SQR_FFT_TABLE3_SIZE)
        //#if MUL_FFT_TABLE3_SIZE > SQR_FFT_TABLE3_SIZE
        //#define FFT_TABLE3_SIZE MUL_FFT_TABLE3_SIZE
        //#else
        //#define FFT_TABLE3_SIZE SQR_FFT_TABLE3_SIZE
        //#endif
        //#endif
        //#endif

        //# ifndef FFT_TABLE3_SIZE
        //#define FFT_TABLE3_SIZE 200
        //#endif

        static readonly fft_table_nk[] MUL_FFT_TABLE3 = new fft_table_nk[MUL_FFT_TABLE3_SIZE] { };

        static readonly fft_table_nk[] SQR_FFT_TABLE3 = new fft_table_nk[SQR_FFT_TABLE3_SIZE] { };


        static
    int
    mpn_fft_best_k_A(mp_size_t n, int sqr) {
            ArrayReadOnlyIterator<fft_table_nk> fft_tab, tab;
            mp_size_t tab_n, thres;
            int last_k;

            fft_tab = (0 != sqr ? SQR_FFT_TABLE3 : MUL_FFT_TABLE3);
            last_k = fft_tab.Current.k;
            for (tab = fft_tab + 1; ; tab++) {
                tab_n = tab.Current.n;
                thres = tab_n << last_k;
                if (n <= thres)
                    break;
                last_k = tab.Current.k;
            }
            return last_k;
        }

        static readonly mp_size_t[] MUL_FFT_TABLE = new mp_size_t[MUL_FFT_TABLE3_SIZE] { };

        static readonly mp_size_t[] SQR_FFT_TABLE = new mp_size_t[SQR_FFT_TABLE3_SIZE] { };

        static
    int
    mpn_fft_best_k(mp_size_t n, int sqr) {
            int i;
            var fft_tab = (0 != sqr ? SQR_FFT_TABLE : MUL_FFT_TABLE);

            for (i = 0; fft_tab[i] != 0; i++)
                if (n < fft_tab[i])
                    return i + FFT_FIRST_K;

            /* treat 4*last as one further entry */
            if (i == 0 || n < 4 * fft_tab[i - 1])
                return i + FFT_FIRST_K;
            else
                return i + FFT_FIRST_K + 1;
        }


        /*****************************************************************************/


        /* Returns smallest possible number of limbs >= pl for a fft of size 2^k,
           i.e. smallest multiple of 2^k >= pl.

           Don't declare static: needed by tuneup.
        */
        static
        mp_size_t
        mpn_fft_next_size(mp_size_t pl, int k) {
            pl = 1 + ((pl - 1) >> k); /* ceil (pl/2^k) */
            return pl << k;
        }


        /* Initialize l[i][j] with bitrev(j) */
        static void
        mpn_fft_initl(int[][] l, int k) {
            int i, j, K;
            int[] li;

            l[0][0] = 0;
            for (i = 1, K = 1; i <= k; i++, K *= 2) {
                li = l[i];
                for (j = 0; j < K; j++) {
                    li[j] = 2 * l[i - 1][j];
                    li[K + j] = 1 + li[j];
                }
            }
        }


        /* r <- a*2^d mod 2^(n*GMP_NUMB_BITS)+1 with a = {a, n+1}
           Assumes a is semi-normalized, i.e. a[n] <= 1.
           r and a must have n+1 limbs, and not overlap.
        */
        static void
        mpn_fft_mul_2exp_modF(mp_ptr r, mp_srcptr a, mp_bitcnt_t d, mp_size_t n) {
            int sh;
            mp_size_t m;
            mp_limb_t cc, rd;

            sh = (int)((ulong)d % GMP_NUMB_BITS);
            m = (mp_size_t)((ulong)d / GMP_NUMB_BITS);

            if (m >= n)         /* negate */
              {
                /* r[0..m-1]  <-- lshift(a[n-m]..a[n-1], sh)
               r[m..n-1]  <-- -lshift(a[0]..a[n-m-1],  sh) */

                m -= n;
                if (sh != 0) {
                    /* no out shift below since a[n] <= 1 */
                    mpn_lshift(r, a + n - m, m + 1, sh);
                    rd = r[m];
                    cc = mpn_lshiftc(r + m, a, n - m, sh);
                } else {
                    MPN_COPY(r, a + n - m, m);
                    rd = a[n];
                    mpn_com(r + m, a, n - m);
                    cc = 0;
                }

                /* add cc to r[0], and add rd to r[m] */

                /* now add 1 in r[m], subtract 1 in r[n], i.e. add 1 in r[0] */

                r[n] = 0;
                /* cc < 2^sh <= 2^(GMP_NUMB_BITS-1) thus no overflow here */
                cc++;
                mpn_incr_u(r, cc);

                rd++;
                /* rd might overflow when sh=GMP_NUMB_BITS-1 */
                cc = (rd == 0) ? 1 : rd;
                r = r + m + (int)CBool(rd == 0);
                mpn_incr_u(r, cc);
            } else {
                /* r[0..m-1]  <-- -lshift(a[n-m]..a[n-1], sh)
               r[m..n-1]  <-- lshift(a[0]..a[n-m-1],  sh)  */
                if (sh != 0) {
                    /* no out bits below since a[n] <= 1 */
                    mpn_lshiftc(r, a + n - m, m + 1, sh);
                    rd = ~r[m];
                    /* {r, m+1} = {a+n-m, m+1} << sh */
                    cc = mpn_lshift(r + m, a, n - m, sh); /* {r+m, n-m} = {a, n-m}<<sh */
                } else {
                    /* r[m] is not used below, but we save a test for m=0 */
                    mpn_com(r, a + n - m, m + 1);
                    rd = a[n];
                    MPN_COPY(r + m, a, n - m);
                    cc = 0;
                }

                /* now complement {r, m}, subtract cc from r[0], subtract rd from r[m] */

                /* if m=0 we just have r[0]=a[n] << sh */
                if (m != 0) {
                    /* now add 1 in r[0], subtract 1 in r[m] */
                    if (cc-- == 0) /* then add 1 to r[0] */
                        cc = mpn_add_1(r, r, n, (mp_limb_t)(1));
                    cc = mpn_sub_1(r, r, m, cc) + 1;
                    /* add 1 to cc instead of rd since rd might overflow */
                }

                /* now subtract cc and rd from r[m..n] */

                r[n] = NEG(mpn_sub_1(r + m, r + m, n - m, cc));
                r[n] -= mpn_sub_1(r + m, r + m, n - m, rd);
                if (r[n] & GMP_LIMB_HIGHBIT)
                    r[n] = mpn_add_1(r, r, n, (mp_limb_t)(1));
            }
        }


        /* r <- a+b mod 2^(n*GMP_NUMB_BITS)+1.
           Assumes a and b are semi-normalized.
        */
        static void
        mpn_fft_add_modF(mp_ptr r, mp_srcptr a, mp_srcptr b, mp_size_t n) {
            mp_limb_t c, x;

            c = a[n] + b[n] + mpn_add_n(r, a, b, n);
            /* 0 <= c <= 3 */

            /* GCC 4.1 outsmarts most expressions here, and generates a 50% branch.  The
               result is slower code, of course.  But the following outsmarts GCC.  */
            x = (c - 1) & (mp_limb_t)(long)-(int)CBool(c != 0);
            r[n] = c - x;
            MPN_DECR_U(r, n + 1, x);

        }

        /* r <- a-b mod 2^(n*GMP_NUMB_BITS)+1.
           Assumes a and b are semi-normalized.
        */
        static void
        mpn_fft_sub_modF(mp_ptr r, mp_srcptr a, mp_srcptr b, mp_size_t n) {
            mp_limb_t c, x;

            c = a[n] - b[n] - mpn_sub_n(r, a, b, n);
            /* -2 <= c <= 1 */

            /* GCC 4.1 outsmarts most expressions here, and generates a 50% branch.  The
               result is slower code, of course.  But the following outsmarts GCC.  */
            x = (NEG(c)) & (mp_limb_t)(long)-(int)CBool((c & GMP_LIMB_HIGHBIT) != 0);
            r[n] = x + c;
            MPN_INCR_U(r, n + 1, x);

        }

        /* input: A[0] ... A[inc*(K-1)] are residues mod 2^N+1 where
              N=n*GMP_NUMB_BITS, and 2^omega is a primitive root mod 2^N+1
           output: A[inc*l[k][i]] <- \sum (2^omega)^(ij) A[inc*j] mod 2^N+1 */

        static void
        mpn_fft_fft(ArrayReadOnlyIterator< mp_ptr> Ap, mp_size_t K, ArrayReadOnlyIterator<ArrayReadOnlyIterator<int>> ll,
                 mp_size_t omega, mp_size_t n, mp_size_t inc, mp_ptr tp) {
            if (K == 2) {
                mp_limb_t cy;

      cy = mpn_add_n_sub_n (Ap[0], Ap[inc], Ap[0], Ap[inc], n + 1) & 1;

                if (Ap[0][n] > 1) /* can be 2 or 3 */
                    Ap[0][n] = 1 - mpn_sub_1(Ap[0], Ap[0], n, Ap[0][n] - 1);
                if (0 != cy) /* Ap[inc][n] can be -1 or -2 */
                    Ap[inc][n] = mpn_add_1(Ap[inc], Ap[inc], n, ~Ap[inc][n] + 1);
            } else {
                mp_size_t j, K2 = K >> 1;
                var lk = ll.Current;

                mpn_fft_fft(Ap, K2, ll - 1, 2 * omega, n, inc * 2, tp);
                mpn_fft_fft(Ap + inc, K2, ll - 1, 2 * omega, n, inc * 2, tp);
                /* A[2*j*inc]   <- A[2*j*inc] + omega^l[k][2*j*inc] A[(2j+1)inc]
               A[(2j+1)inc] <- A[2*j*inc] + omega^l[k][(2j+1)inc] A[(2j+1)inc] */
                for (j = 0; j < K2; j++, lk += 2, Ap += 2 * inc) {
                    /* Ap[inc] <- Ap[0] + Ap[inc] * 2^(lk[1] * omega)
                       Ap[0]   <- Ap[0] + Ap[inc] * 2^(lk[0] * omega) */
                    mpn_fft_mul_2exp_modF(tp, Ap[inc], lk[0] * omega, n);
                    mpn_fft_sub_modF(Ap[inc], Ap[0], tp, n);
                    mpn_fft_add_modF(Ap[0], Ap[0], tp, n);
                }
            }
        }

        /* input: A[0] ... A[inc*(K-1)] are residues mod 2^N+1 where
              N=n*GMP_NUMB_BITS, and 2^omega is a primitive root mod 2^N+1
           output: A[inc*l[k][i]] <- \sum (2^omega)^(ij) A[inc*j] mod 2^N+1
           tp must have space for 2*(n+1) limbs.
        */


        /* Given ap[0..n] with ap[n]<=1, reduce it modulo 2^(n*GMP_NUMB_BITS)+1,
           by subtracting that modulus if necessary.

           If ap[0..n] is exactly 2^(n*GMP_NUMB_BITS) then mpn_sub_1 produces a
           borrow and the limbs must be zeroed out again.  This will occur very
           infrequently.  */

        static void
        mpn_fft_normalize(mp_ptr ap, mp_size_t n) {
            if (ap[n] != 0) {
                MPN_DECR_U(ap, n + 1, (mp_limb_t)(1));
                if (ap[n] == 0) {
                    /* This happens with very low probability; we have yet to trigger it,
                       and thereby make sure this code is correct.  */
                    MPN_ZERO(ap, n);
                    ap[n] = 1;
                } else
                    ap[n] = 0;
            }
        }

        /* a[i] <- a[i]*b[i] mod 2^(n*GMP_NUMB_BITS)+1 for 0 <= i < K */
        static void
        mpn_fft_mul_modF_K(mp_ptr* ap, mp_ptr* bp, mp_size_t n, mp_size_t K) {
            int i;
            int sqr = (int)CBool (ap == bp);
            TMP_DECL;

            TMP_MARK;

            if (n >= (sqr ? SQR_FFT_MODF_THRESHOLD : MUL_FFT_MODF_THRESHOLD)) {
                mp_size_t K2, nprime2, Nprime2, M2, maxLK, l, Mp2;
                int k;
                int** fft_l, *tmp;
                mp_ptr* Ap, *Bp, A, B, T;

                k = mpn_fft_best_k(n, sqr);
                K2 = (mp_size_t)1 << k;
                ASSERT_ALWAYS((n & (K2 - 1)) == 0);
                maxLK = (K2 > GMP_NUMB_BITS) ? K2 : GMP_NUMB_BITS;
                M2 = n * GMP_NUMB_BITS >> k;
                l = n >> k;
                Nprime2 = ((2 * M2 + k + 2 + maxLK) / maxLK) * maxLK;
                /* Nprime2 = ceil((2*M2+k+3)/maxLK)*maxLK*/
                nprime2 = Nprime2 / GMP_NUMB_BITS;

                /* we should ensure that nprime2 is a multiple of the next K */
                if (nprime2 >= (sqr ? SQR_FFT_MODF_THRESHOLD : MUL_FFT_MODF_THRESHOLD)) {
                    mp_size_t K3;
                    for (; ; )
                      {
                        K3 = (mp_size_t)1 << mpn_fft_best_k(nprime2, sqr);
                        if ((nprime2 & (K3 - 1)) == 0)
                            break;
                        nprime2 = (nprime2 + K3 - 1) & -K3;
                        Nprime2 = nprime2 * GMP_LIMB_BITS;
                        /* warning: since nprime2 changed, K3 may change too! */
                    }
                }
                ASSERT_ALWAYS(nprime2 < n); /* otherwise we'll loop */

                Mp2 = Nprime2 >> k;

                Ap = TMP_BALLOC_MP_PTRS(K2);
                Bp = TMP_BALLOC_MP_PTRS(K2);
                A = TMP_BALLOC_LIMBS(2 * (nprime2 + 1) << k);
                T = TMP_BALLOC_LIMBS(2 * (nprime2 + 1));
                B = A + ((nprime2 + 1) << k);
                fft_l = TMP_BALLOC_TYPE(k + 1, int *);
                tmp = TMP_BALLOC_TYPE((size_t)2 << k, int);
                for (i = 0; i <= k; i++) {
                    fft_l[i] = tmp;
                    tmp += (mp_size_t)1 << i;
                }

                mpn_fft_initl(fft_l, k);

                TRACE(printf("recurse: %ldx%ld limbs -> %ld times %ldx%ld (%1.2f)\n", n,
                      n, K2, nprime2, nprime2, 2.0 * (double)n / nprime2 / K2));
                for (i = 0; i < K; i++, ap++, bp++) {
                    mp_limb_t cy;
                    mpn_fft_normalize(*ap, n);
                    if (!sqr)
                        mpn_fft_normalize(*bp, n);

                    mpn_mul_fft_decompose(A, Ap, K2, nprime2, *ap, (l << k) + 1, l, Mp2, T);
                    if (!sqr)
                        mpn_mul_fft_decompose(B, Bp, K2, nprime2, *bp, (l << k) + 1, l, Mp2, T);

                    cy = mpn_mul_fft_internal(*ap, n, k, Ap, Bp, A, B, nprime2,
                                   l, Mp2, fft_l, T, sqr);
                    (*ap)[n] = cy;
                }
            } else {
                mp_ptr a, b, tp, tpn;
                mp_limb_t cc;
                mp_size_t n2 = 2 * n;
                tp = TMP_BALLOC_LIMBS(n2);
                tpn = tp + n;
                TRACE(printf("  mpn_mul_n %ld of %ld limbs\n", K, n));
                for (i = 0; i < K; i++) {
                    a = *ap++;
                    b = *bp++;
                    if (sqr)
                        mpn_sqr(tp, a, n);
                    else
                        mpn_mul_n(tp, b, a, n);
                    if (a[n] != 0)
                        cc = mpn_add_n(tpn, tpn, b, n);
                    else
                        cc = 0;
                    if (b[n] != 0)
                        cc += mpn_add_n(tpn, tpn, a, n) + a[n];
                    if (cc != 0) {
                        /* FIXME: use MPN_INCR_U here, since carry is not expected.  */
                        cc = mpn_add_1(tp, tp, n2, cc);
                        ASSERT(cc == 0);
                    }
                    a[n] = mpn_sub_n(a, tp, tpn, n) && mpn_add_1(a, a, n, CNST_LIMB(1));
                }
            }
            TMP_FREE;
        }


        /* input: A^[l[k][0]] A^[l[k][1]] ... A^[l[k][K-1]]
           output: K*A[0] K*A[K-1] ... K*A[1].
           Assumes the Ap[] are pseudo-normalized, i.e. 0 <= Ap[][n] <= 1.
           This condition is also fulfilled at exit.
        */
        static void
        mpn_fft_fftinv(ArrayReadOnlyIterator<  mp_ptr> Ap, mp_size_t K, mp_size_t omega, mp_size_t n, mp_ptr tp) {
            if (K == 2) {
                mp_limb_t cy;

      cy = mpn_add_n_sub_n (Ap[0], Ap[1], Ap[0], Ap[1], n + 1) & 1;

                if (Ap[0][n] > 1) /* can be 2 or 3 */
                    Ap[0][n] = 1 - mpn_sub_1(Ap[0], Ap[0], n, Ap[0][n] - 1);
                if (0 != cy) /* Ap[1][n] can be -1 or -2 */
                    Ap[1][n] = mpn_add_1(Ap[1], Ap[1], n, ~Ap[1][n] + 1);
            } else {
                mp_size_t j, K2 = K >> 1;

                mpn_fft_fftinv(Ap, K2, 2 * omega, n, tp);
                mpn_fft_fftinv(Ap + K2, K2, 2 * omega, n, tp);
                /* A[j]     <- A[j] + omega^j A[j+K/2]
               A[j+K/2] <- A[j] + omega^(j+K/2) A[j+K/2] */
                for (j = 0; j < K2; j++, Ap++) {
                    /* Ap[K2] <- Ap[0] + Ap[K2] * 2^((j + K2) * omega)
                       Ap[0]  <- Ap[0] + Ap[K2] * 2^(j * omega) */
                    mpn_fft_mul_2exp_modF(tp, Ap[K2], j * omega, n);
                    mpn_fft_sub_modF(Ap[K2], Ap[0], tp, n);
                    mpn_fft_add_modF(Ap[0], Ap[0], tp, n);
                }
            }
        }


        /* R <- A/2^k mod 2^(n*GMP_NUMB_BITS)+1 */
        static void
        mpn_fft_div_2exp_modF(mp_ptr r, mp_srcptr a, mp_bitcnt_t k, mp_size_t n) {
            mp_bitcnt_t i;

            ASSERT(r != a);
            i = (mp_bitcnt_t)2 * n * GMP_NUMB_BITS - k;
            mpn_fft_mul_2exp_modF(r, a, i, n);
            /* 1/2^k = 2^(2nL-k) mod 2^(n*GMP_NUMB_BITS)+1 */
            /* normalize so that R < 2^(n*GMP_NUMB_BITS)+1 */
            mpn_fft_normalize(r, n);
        }


        /* {rp,n} <- {ap,an} mod 2^(n*GMP_NUMB_BITS)+1, n <= an <= 3*n.
           Returns carry out, i.e. 1 iff {ap,an} = -1 mod 2^(n*GMP_NUMB_BITS)+1,
           then {rp,n}=0.
        */
        static mp_size_t
        mpn_fft_norm_modF(mp_ptr rp, mp_size_t n, mp_ptr ap, mp_size_t an) {
            mp_size_t l, m, rpn;
            mp_limb_t cc;

            ASSERT((n <= an) && (an <= 3 * n));
            m = an - 2 * n;
            if (m > 0) {
                l = n;
                /* add {ap, m} and {ap+2n, m} in {rp, m} */
                cc = mpn_add_n(rp, ap, ap + 2 * n, m);
                /* copy {ap+m, n-m} to {rp+m, n-m} */
                rpn = (mp_size_t)mpn_add_1(rp + m, ap + m, n - m, cc);
            } else {
                l = an - n; /* l <= n */
                MPN_COPY(rp, ap, n);
                rpn = 0;
            }

            /* remains to subtract {ap+n, l} from {rp, n+1} */
            cc = mpn_sub_n(rp, rp, ap + n, l);
            rpn -= (mp_size_t)mpn_sub_1(rp + l, rp + l, n - l, cc);
            if (rpn < 0) /* necessarily rpn = -1 */
                rpn = mpn_add_1(rp, rp, n, (mp_limb_t)(1));
            return rpn;
        }

        /* store in A[0..nprime] the first M bits from {n, nl},
           in A[nprime+1..] the following M bits, ...
           Assumes M is a multiple of GMP_NUMB_BITS (M = l * GMP_NUMB_BITS).
           T must have space for at least (nprime + 1) limbs.
           We must have nl <= 2*K*l.
        */
        static void
        mpn_mul_fft_decompose(mp_ptr A, mp_ptr* Ap, mp_size_t K, mp_size_t nprime,
                       mp_srcptr n, mp_size_t nl, mp_size_t l, mp_size_t Mp,
                       mp_ptr T) {
            mp_size_t i, j;
            mp_ptr tmp;
            mp_size_t Kl = K * l;
            TMP_DECL;
            TMP_MARK;

            if (nl > Kl) /* normalize {n, nl} mod 2^(Kl*GMP_NUMB_BITS)+1 */
              {
                mp_size_t dif = nl - Kl;
                mp_limb_t cy;

                tmp = TMP_BALLOC_LIMBS(Kl + 1);

                if (dif > Kl) {
                    int subp = 0;

                    cy = mpn_sub_n(tmp, n, n + Kl, Kl);
                    n += 2 * Kl;
                    dif -= Kl;

                    /* now dif > 0 */
                    while (dif > Kl) {
                        if (0 != subp)
                            cy += mpn_sub_n(tmp, tmp, n, Kl);
                        else
                            cy -= mpn_add_n(tmp, tmp, n, Kl);
                        subp ^= 1;
                        n += Kl;
                        dif -= Kl;
                    }
                    /* now dif <= Kl */
                    if (0 != subp)
                        cy += mpn_sub(tmp, tmp, Kl, n, dif);
                    else
                        cy -= mpn_add(tmp, tmp, Kl, n, dif);
                    if (cy >= 0)
                        cy = mpn_add_1(tmp, tmp, Kl, cy);
                    else
                        cy = mpn_sub_1(tmp, tmp, Kl, NEG(cy));
                } else /* dif <= Kl, i.e. nl <= 2 * Kl */
                {
                    cy = mpn_sub(tmp, n, Kl, n + Kl, dif);
                    cy = mpn_add_1(tmp, tmp, Kl, cy);
                }
                tmp[Kl] = cy;
                nl = Kl + 1;
                n = tmp;
            }
            for (i = 0; i < K; i++) {
                Ap[i] = A;
                /* store the next M bits of n into A[0..nprime] */
                if (nl > 0) /* nl is the number of remaining limbs */
              {
                    j = (l <= nl && i < K - 1) ? l : nl; /* store j next limbs */
                    nl -= j;
                    MPN_COPY(T, n, j);
                    MPN_ZERO(T + j, nprime + 1 - j);
                    n += l;
                    mpn_fft_mul_2exp_modF(A, T, i * Mp, nprime);
                } else
                    MPN_ZERO(A, nprime + 1);
                A += nprime + 1;
            }
            ASSERT_ALWAYS(nl == 0);
            TMP_FREE;
        }

        /* op <- n*m mod 2^N+1 with fft of size 2^k where N=pl*GMP_NUMB_BITS
           op is pl limbs, its high bit is returned.
           One must have pl = mpn_fft_next_size (pl, k).
           T must have space for 2 * (nprime + 1) limbs.
        */

        static mp_limb_t
        mpn_mul_fft_internal(mp_ptr op, mp_size_t pl, int k,
                      mp_ptr* Ap, mp_ptr* Bp, mp_ptr A, mp_ptr B,
                      mp_size_t nprime, mp_size_t l, mp_size_t Mp,
                      int** fft_l, mp_ptr T, int sqr) {
            mp_size_t K, i, pla, lo, sh, j;
            mp_ptr p;
            mp_limb_t cc;

            K = (mp_size_t)1 << k;

            /* direct fft's */
            mpn_fft_fft(Ap, K, fft_l + k, 2 * Mp, nprime, 1, T);
            if (!sqr)
                mpn_fft_fft(Bp, K, fft_l + k, 2 * Mp, nprime, 1, T);

            /* term to term multiplications */
            mpn_fft_mul_modF_K(Ap, sqr ? Ap : Bp, nprime, K);

            /* inverse fft's */
            mpn_fft_fftinv(Ap, K, 2 * Mp, nprime, T);

            /* division of terms after inverse fft */
            Bp[0] = T + nprime + 1;
            mpn_fft_div_2exp_modF(Bp[0], Ap[0], k, nprime);
            for (i = 1; i < K; i++) {
                Bp[i] = Ap[i - 1];
                mpn_fft_div_2exp_modF(Bp[i], Ap[i], k + (K - i) * Mp, nprime);
            }

            /* addition of terms in result p */
            MPN_ZERO(T, nprime + 1);
            pla = l * (K - 1) + nprime + 1; /* number of required limbs for p */
            p = B; /* B has K*(n' + 1) limbs, which is >= pla, i.e. enough */
            MPN_ZERO(p, pla);
            cc = 0; /* will accumulate the (signed) carry at p[pla] */
            for (i = K - 1, lo = l * i + nprime, sh = l * i; i >= 0; i--, lo -= l, sh -= l) {
                mp_ptr n = p + sh;

                j = (K - i) & (K - 1);

                if (mpn_add_n(n, n, Bp[j], nprime + 1))
                    cc += mpn_add_1(n + nprime + 1, n + nprime + 1,
                              pla - sh - nprime - 1, CNST_LIMB(1));
                T[2 * l] = i + 1; /* T = (i + 1)*2^(2*M) */
                if (mpn_cmp(Bp[j], T, nprime + 1) > 0) { /* subtract 2^N'+1 */
                    cc -= mpn_sub_1(n, n, pla - sh, CNST_LIMB(1));
                    cc -= mpn_sub_1(p + lo, p + lo, pla - lo, CNST_LIMB(1));
                }
            }
            if (cc == -CNST_LIMB(1)) {
                if ((cc = mpn_add_1(p + pla - pl, p + pla - pl, pl, CNST_LIMB(1)))) {
                    /* p[pla-pl]...p[pla-1] are all zero */
                    mpn_sub_1(p + pla - pl - 1, p + pla - pl - 1, pl + 1, CNST_LIMB(1));
                    mpn_sub_1(p + pla - 1, p + pla - 1, 1, CNST_LIMB(1));
                }
            } else if (cc == 1) {
                if (pla >= 2 * pl) {
                    while ((cc = mpn_add_1(p + pla - 2 * pl, p + pla - 2 * pl, 2 * pl, cc)))
                        ;
                } else {
                    cc = mpn_sub_1(p + pla - pl, p + pla - pl, pl, cc);
                    ASSERT(cc == 0);
                }
            } else
                ASSERT(cc == 0);

            /* here p < 2^(2M) [K 2^(M(K-1)) + (K-1) 2^(M(K-2)) + ... ]
               < K 2^(2M) [2^(M(K-1)) + 2^(M(K-2)) + ... ]
               < K 2^(2M) 2^(M(K-1))*2 = 2^(M*K+M+k+1) */
            return mpn_fft_norm_modF(op, pl, p, pla);
        }

        /* return the lcm of a and 2^k */
        static mp_bitcnt_t
        mpn_mul_fft_lcm(mp_bitcnt_t a, int k) {
            mp_bitcnt_t l = k;

            while (a % 2 == 0 && k > 0) {
                a >>= 1;
                k--;
            }
            return a << (int)l;
        }


        mp_limb_t
        mpn_mul_fft(mp_ptr op, mp_size_t pl,
                 mp_srcptr n, mp_size_t nl,
                 mp_srcptr m, mp_size_t ml,
                 int k) {
            int i;
            mp_size_t K, maxLK;
            mp_size_t N, Nprime, nprime, M, Mp, l;
            mp_ptr* Ap, *Bp, A, T, B;
            int** fft_l;
            int *tmp;
            int sqr = (n == m && nl == ml);
            mp_limb_t h;
            TMP_DECL;

            TRACE(printf("\nmpn_mul_fft pl=%ld nl=%ld ml=%ld k=%d\n", pl, nl, ml, k));
            ASSERT_ALWAYS(mpn_fft_next_size(pl, k) == pl);

            TMP_MARK;
            N = pl * GMP_NUMB_BITS;
            fft_l = TMP_BALLOC_TYPE(k + 1, int *);
            tmp = TMP_BALLOC_TYPE((size_t)2 << k, int);
            for (i = 0; i <= k; i++) {
                fft_l[i] = tmp;
                tmp += (mp_size_t)1 << i;
            }

            mpn_fft_initl(fft_l, k);
            K = (mp_size_t)1 << k;
            M = N >> k; /* N = 2^k M */
            l = 1 + (M - 1) / GMP_NUMB_BITS;
            maxLK = mpn_mul_fft_lcm(GMP_NUMB_BITS, k); /* lcm (GMP_NUMB_BITS, 2^k) */

            Nprime = (1 + (2 * M + k + 2) / maxLK) * maxLK;
            /* Nprime = ceil((2*M+k+3)/maxLK)*maxLK; */
            nprime = Nprime / GMP_NUMB_BITS;
            TRACE(printf("N=%ld K=%ld, M=%ld, l=%ld, maxLK=%ld, Np=%ld, np=%ld\n",
                   N, K, M, l, maxLK, Nprime, nprime));
            /* we should ensure that recursively, nprime is a multiple of the next K */
            if (nprime >= (sqr ? SQR_FFT_MODF_THRESHOLD : MUL_FFT_MODF_THRESHOLD)) {
                mp_size_t K2;
                for (; ; )
              {
                    K2 = (mp_size_t)1 << mpn_fft_best_k(nprime, sqr);
                    if ((nprime & (K2 - 1)) == 0)
                        break;
                    nprime = (nprime + K2 - 1) & -K2;
                    Nprime = nprime * GMP_LIMB_BITS;
                    /* warning: since nprime changed, K2 may change too! */
                }
                TRACE(printf("new maxLK=%ld, Np=%ld, np=%ld\n", maxLK, Nprime, nprime));
            }
            ASSERT_ALWAYS(nprime < pl); /* otherwise we'll loop */

            T = TMP_BALLOC_LIMBS(2 * (nprime + 1));
            Mp = Nprime >> k;

            TRACE(printf("%ldx%ld limbs -> %ld times %ldx%ld limbs (%1.2f)\n",
                  pl, pl, K, nprime, nprime, 2.0 * (double)N / Nprime / K);
            printf("   temp space %ld\n", 2 * K * (nprime + 1)));

            A = TMP_BALLOC_LIMBS(K * (nprime + 1));
            Ap = TMP_BALLOC_MP_PTRS(K);
            mpn_mul_fft_decompose(A, Ap, K, nprime, n, nl, l, Mp, T);
            if (sqr) {
                mp_size_t pla;
                pla = l * (K - 1) + nprime + 1; /* number of required limbs for p */
                B = TMP_BALLOC_LIMBS(pla);
                Bp = TMP_BALLOC_MP_PTRS(K);
            } else {
                B = TMP_BALLOC_LIMBS(K * (nprime + 1));
                Bp = TMP_BALLOC_MP_PTRS(K);
                mpn_mul_fft_decompose(B, Bp, K, nprime, m, ml, l, Mp, T);
            }
            h = mpn_mul_fft_internal(op, pl, k, Ap, Bp, A, B, nprime, l, Mp, fft_l, T, sqr);

            TMP_FREE;
            return h;
        }





        static mp_limb_t
                mpn_addmul_1(mp_ptr rp, mp_srcptr up, mp_size_t n, mp_limb_t vl) {
            mp_limb_t ul, cl, hpl, lpl, rl;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));

            cl = 0;
            do {
                ul = up++.Current;
                umul_ppmm(out hpl, out lpl, ul, vl);

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                rl = rp.Current;
                lpl = rl + lpl;
                cl += CBool(lpl < rl);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }
        static
        mp_limb_t
        mpn_submul_1(mp_ptr rp, mp_srcptr up, mp_size_t n, mp_limb_t vl) {
            mp_limb_t ul, cl, hpl, lpl, rl;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));

            cl = 0;
            do {
                ul = up++.Current;
                umul_ppmm(out hpl, out lpl, ul, vl);

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                rl = rp.Current;
                lpl = rl - lpl;
                cl += CBool(lpl > rl);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }
        static mp_limb_t mpn_addlsh1_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t ul, vl, cl, hpl, lpl;
            int cnt = 1;
            int tnc;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT(MPN_SAME_OR_DECR_P(rp, vp, n));

            tnc = GMP_NUMB_BITS - cnt;
            cl = 0;
            do {
                vl = vp++.Current;
                lpl = vl >> tnc;
                hpl = (vl << cnt) & GMP_NUMB_MASK;

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                ul = up++.Current;
                lpl = ul + lpl;
                cl += CBool(lpl < ul);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }

        static mp_limb_t mpn_sublsh1_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t ul, vl, cl, hpl, lpl;
            int cnt = 1;
            int tnc;

            ASSERT(n >= 1);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT(MPN_SAME_OR_DECR_P(rp, vp, n));

            tnc = GMP_NUMB_BITS - cnt;
            cl = 0;
            do {
                vl = vp++.Current;
                lpl = vl >> tnc;
                hpl = (vl << cnt) & GMP_NUMB_MASK;

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                ul = up++.Current;
                lpl = ul - lpl;
                cl += CBool(lpl > ul);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }

        static mp_limb_t mpn_addlsh_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n, int cnt) {
            mp_limb_t ul, vl, cl, hpl, lpl;
            int tnc;

            ASSERT(n >= 1);
            ASSERT(cnt >= 1);
            ASSERT(cnt < GMP_NUMB_BITS);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT(MPN_SAME_OR_DECR_P(rp, vp, n));

            tnc = GMP_NUMB_BITS - cnt;
            cl = 0;
            do {
                vl = vp++.Current;
                lpl = vl >> tnc;
                hpl = (vl << cnt) & GMP_NUMB_MASK;

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                ul = up++.Current;
                lpl = ul + lpl;
                cl += CBool(lpl < ul);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }

        static mp_limb_t mpn_sublsh_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n, int cnt) {
            mp_limb_t ul, vl, cl, hpl, lpl;
            int tnc;

            ASSERT(n >= 1);
            ASSERT(cnt >= 1);
            ASSERT(cnt < GMP_NUMB_BITS);
            ASSERT(MPN_SAME_OR_SEPARATE_P(rp, up, n));
            ASSERT(MPN_SAME_OR_DECR_P(rp, vp, n));

            tnc = GMP_NUMB_BITS - cnt;
            cl = 0;
            do {
                vl = vp++.Current;
                lpl = vl >> tnc;
                hpl = (vl << cnt) & GMP_NUMB_MASK;

                lpl += cl;
                cl = CBool(lpl < cl) + hpl;

                ul = up++.Current;
                lpl = ul - lpl;
                cl += CBool(lpl > ul);
                rp++.Current = lpl;
            }
            while (--n != 0);

            return cl;
        }



        static mp_limb_t mpn_rsblsh1_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            throw new NotImplementedException();
        }

        static mp_limb_t mpn_rsblsh_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n, int cnt) {
            throw new NotImplementedException();
        }



        static mp_limb_t
        mpn_rsh1sub_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t cya, cys;

            throw new NotImplementedException();
            ASSERT(n >= 1);

            cya = mpn_sub_n(rp, up, vp, n);
            cys = mpn_rshift(rp, rp, n, 1) >> (GMP_NUMB_BITS - 1);
            rp[n - 1] |= cya << (GMP_NUMB_BITS - 1);
            return cys;
        }

        static mp_limb_t
        mpn_rsh1add_n(mp_ptr rp, mp_srcptr up, mp_srcptr vp, mp_size_t n) {
            mp_limb_t cya, cys;

            throw new NotImplementedException();
            ASSERT(n >= 1);

            cya = mpn_add_n(rp, up, vp, n);
            cys = mpn_rshift(rp, rp, n, 1) >> (GMP_NUMB_BITS - 1);
            rp[n - 1] |= cya << (GMP_NUMB_BITS - 1);
            return cys;
        }
    }

}

namespace UltimateOrb.Core.Tests {
    /*
    public readonly struct PtrSimulated<T> : IEquatable<PtrSimulated<T>> {

        internal readonly T[] _array;

        internal readonly uint _index;

        public ref T Data {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._array[this._index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public PtrSimulated(T[] array, uint index) {
            this._array = array;
            this._index = index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator +(PtrSimulated<T> @base, int offset) {
            return new PtrSimulated<T>(@base._array, checked((uint)(@base._index + offset)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator +(int offset, PtrSimulated<T> @base) {
            return @base + offset;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator ++(PtrSimulated<T> @base) {
            return @base + 1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator --(PtrSimulated<T> @base) {
            return @base - 1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator +(PtrSimulated<T> @base) {
            return @base;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static PtrSimulated<T> operator -(PtrSimulated<T> @base, int offset) {
            return new PtrSimulated<T>(@base._array, checked((uint)(@base._index - offset)));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int operator -(PtrSimulated<T> first, PtrSimulated<T> second) {
            if (first._array == second._array) {
                return checked((int)((long)first._index - second._index));
            }
            // TODO: Perf
            throw new InvalidOperationException();
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._array == second._array && first._index < second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._array == second._array && first._index <= second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._array == second._array && first._index > second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._array == second._array && first._index >= second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._array == second._array && first._index == second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PtrSimulated<T> first, long second) {
            if (0 == second) {
                return null == first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PtrSimulated<T> first, long second) {
            if (0 == second) {
                return null != first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long first, PtrSimulated<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long first, PtrSimulated<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(PtrSimulated<T> first, ulong second) {
            if (0 == second) {
                return null == first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PtrSimulated<T> first, ulong second) {
            if (0 == second) {
                return null != first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, PtrSimulated<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, PtrSimulated<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(PtrSimulated<T> first, PtrSimulated<T> second) {
            return first._index != second._index || first._array != second._array;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(PtrSimulated<T> other) {
            return this._array == other._array && this._index == other._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            return unchecked((int)this._index);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) {
            if (obj is PtrSimulated<T> ptr) {
                return this.Equals(ptr);
            }
            return base.Equals(obj);
        }

        public ref T this[int offset] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._array[checked(this._index + offset)];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator PtrSimulated<T>(T[] array) {
            return new PtrSimulated<T>(array, 0);
        }
    }

    */


    public readonly struct ArrayIterator<T> : IEquatable<ArrayIterator<T>> {

        internal readonly T[] _array;

        internal readonly uint _index;

        public ref T Data {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._array[this._index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ref T op_PointerDereference(ArrayIterator<T> value) {
            return ref value._array[value._index];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public ArrayIterator(T[] array, uint index) {
            this._array = array;
            this._index = index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(ArrayIterator<T> @base, int offset) {
            return new ArrayIterator<T>(@base._array, unchecked(@base._index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(int offset, ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._array, unchecked(@base._index + (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator ++(ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._array, unchecked(1 + @base._index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator --(ArrayIterator<T> @base) {
            return new ArrayIterator<T>(@base._array, unchecked(@base._index) - 1);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator +(ArrayIterator<T> @base) {
            return @base;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static ArrayIterator<T> operator -(ArrayIterator<T> @base, int offset) {
            return new ArrayIterator<T>(@base._array, unchecked(@base._index - (uint)offset));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int operator -(ArrayIterator<T> first, ArrayIterator<T> second) {
            Debug.Assert(first._array == second._array);
            return unchecked((int)(first._index - second._index));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._array == second._array && first._index < second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._array == second._array && first._index <= second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._array == second._array && first._index > second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._array == second._array && first._index >= second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._array == second._array && first._index == second._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, long second) {
            if (0 == second) {
                return null == first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, long second) {
            if (0 == second) {
                return null != first._array;
            }
            return false;
        }


        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(long first, ArrayIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(long first, ArrayIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ArrayIterator<T> first, ulong second) {
            if (0 == second) {
                return null == first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, ulong second) {
            if (0 == second) {
                return null != first._array;
            }
            return false;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(ulong first, ArrayIterator<T> second) {
            return second == first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ulong first, ArrayIterator<T> second) {
            return second != first;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(ArrayIterator<T> first, ArrayIterator<T> second) {
            return first._index != second._index || first._array != second._array;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ArrayIterator<T> other) {
            return this._array == other._array && this._index == other._index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() {
            return unchecked((int)this._index ^ (null == this._array ? 0 : this._array.GetHashCode()));
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj) {
            if (obj is ArrayIterator<T> ptr) {
                return this.Equals(ptr);
            }
            return base.Equals(obj);
        }

        public ref T this[int offset] {

            [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
            get => ref this._array[unchecked(this._index + (uint)offset)];
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ArrayIterator<T>(T[] array) {
            return new ArrayIterator<T>(array, 0);
        }
    }



}
