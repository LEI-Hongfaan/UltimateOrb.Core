using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using UltimateOrb.Mathematics;
using InternalMath = Internal.System.Math;

namespace UltimateOrb.Core.Tests {


    struct BigUIntegerBuilder {

        // The longest binary mantissa requires: explicit mantissa bits + abs(min exponent)
        // * Half:     10 +    14 =    24
        // * Single:   23 +   126 =   149
        // * Double:   52 +  1022 =  1074
        // * Quad:    112 + 16382 = 16494
        private const int BitsForLongestBinaryMantissa = 16494;

        // The longest digit sequence requires: ceil(log2(pow(10, max significant digits + 1 rounding digit)))
        // * Half:    ceil(log2(pow(10,    21 + 1))) =    74
        // * Single:  ceil(log2(pow(10,   112 + 1))) =   376
        // * Double:  ceil(log2(pow(10,   767 + 1))) =  2552
        // * Quad:    ceil(log2(pow(10, 11563 + 1))) = 38415
        private const int BitsForLongestDigitSequence = 38415;

        // We require BitsPerBlock additional bits for shift space used during the pre-division preparation
        private const int MaxBits = BitsForLongestBinaryMantissa + BitsForLongestDigitSequence + BitsPerBlock;

        private const int BitsPerBlock = sizeof(int) * 8;
        private const int MaxBlockCount = (MaxBits + (BitsPerBlock - 1)) / BitsPerBlock;

        private static readonly uint[] s_Exp10UInt32Table = new uint[] {
                1,          // 10^0
                10,         // 10^1
                100,        // 10^2
                1000,       // 10^3
                10000,      // 10^4
                100000,     // 10^5
                1000000,    // 10^6
                10000000,   // 10^7
        };

        private static BigUIntegerBuilder[] gggg() {
            var a = new BigUIntegerBuilder[11];
            var u = new BigUIntegerBuilder(100000000u);
            {
                Array.Resize(ref u._blocks, u._length);
                a[0] = u;
            }
            for (int i = 1; a.Length > i; ++i) {
                BigUIntegerBuilder t = default;
                Multiply(ref u, ref u, ref t);
                Array.Resize(ref t._blocks, t._length);
                a[i] = u = t;
            }
            return a;
        }
        
        private static readonly BigUIntegerBuilder[] s_Exp10BigNumTable = gggg();


        // MaxBlockCount
        private uint[] _blocks;

        private int _length;

        public BigUIntegerBuilder(int capacity, int length) {
            _blocks = new uint[capacity];
            _length = length;
        }

        public BigUIntegerBuilder(uint value) {
            var blocks = new uint[4];
            blocks[0] = value;
            _blocks = blocks;
            _length = (value == 0) ? 0 : 1;
        }

        public BigUIntegerBuilder(ulong value) {
            uint lower = (uint)(value);
            uint upper = (uint)(value >> 32);

            var blocks = new uint[4];
            blocks[0] = lower;
            blocks[1] = upper;
            _blocks = blocks;

            _length = (upper == 0) ? 1 : 2;
        }

        public static void Add(ref BigUIntegerBuilder lhs, ref BigUIntegerBuilder rhs, out BigUIntegerBuilder result) {
            // determine which operand has the smaller length
            ref BigUIntegerBuilder large = ref (lhs._length < rhs._length) ? ref rhs : ref lhs;
            ref BigUIntegerBuilder small = ref (lhs._length < rhs._length) ? ref lhs : ref rhs;

            int largeLength = large._length;
            int smallLength = small._length;

            // The output will be at least as long as the largest input
            result = new BigUIntegerBuilder(0);
            result._length = largeLength;

            // Add each block and add carry the overflow to the next block
            ulong carry = 0;

            int largeIndex = 0;
            int smallIndex = 0;
            int resultIndex = 0;

            while (smallIndex < smallLength) {
                ulong sum = carry + large._blocks[largeIndex] + small._blocks[smallIndex];
                carry = sum >> 32;
                result._blocks[resultIndex] = (uint)(sum);

                largeIndex++;
                smallIndex++;
                resultIndex++;
            }

            // Add the carry to any blocks that only exist in the large operand
            while (largeIndex < largeLength) {
                ulong sum = carry + large._blocks[largeIndex];
                carry = sum >> 32;
                result._blocks[resultIndex] = (uint)(sum);

                largeIndex++;
                resultIndex++;
            }

            // If there's still a carry, append a new block
            if (carry != 0) {
                Debug.Assert(carry == 1);
                Debug.Assert((resultIndex == largeLength) && (largeLength < MaxBlockCount));

                result._blocks[resultIndex] = 1;
                result._length++;
            }
        }

        public static int Compare(ref BigUIntegerBuilder lhs, ref BigUIntegerBuilder rhs) {
            Debug.Assert(unchecked((uint)(lhs._length)) <= MaxBlockCount);
            Debug.Assert(unchecked((uint)(rhs._length)) <= MaxBlockCount);

            int lhsLength = lhs._length;
            int rhsLength = rhs._length;

            int lengthDelta = (lhsLength - rhsLength);

            if (lengthDelta != 0) {
                return lengthDelta;
            }

            if (lhsLength == 0) {
                Debug.Assert(rhsLength == 0);
                return 0;
            }

            for (int index = (lhsLength - 1); index >= 0; index--) {
                long delta = (long)(lhs._blocks[index]) - rhs._blocks[index];

                if (delta != 0) {
                    return delta > 0 ? 1 : -1;
                }
            }

            return 0;
        }

        public static uint CountSignificantBits(uint value) {
            return 32 - (uint)BinaryNumerals.CountLeadingZeros(value);
        }

        public static uint CountSignificantBits(ulong value) {
            return 64 - (uint)BinaryNumerals.CountLeadingZeros(value);
        }

        public static uint CountSignificantBits(ref BigUIntegerBuilder value) {
            if (value.IsZero()) {
                return 0;
            }

            // We don't track any unused blocks, so we only need to do a BSR on the
            // last index and add that to the number of bits we skipped.

            uint lastIndex = (uint)(value._length - 1);
            return (lastIndex * BitsPerBlock) + CountSignificantBits(value._blocks[lastIndex]);
        }

        public static void DivRem(ref BigUIntegerBuilder lhs, ref BigUIntegerBuilder rhs, out BigUIntegerBuilder quo, out BigUIntegerBuilder rem) {
            // This is modified from the CoreFX BigUIntegerBuilderCalculator.DivRemIntegral.cs implementation:
            // https://github.com/dotnet/corefx/blob/0bb106232745aedfc0d0c5a84ff2b244bf190317/src/System.Runtime.Numerics/src/System/Numerics/BigUIntegerBuilderCalculator.DivRemIntegral.cs

            Debug.Assert(!rhs.IsZero());

            quo = new BigUIntegerBuilder(0);
            rem = new BigUIntegerBuilder(0);

            if (lhs.IsZero()) {
                return;
            }

            int lhsLength = lhs._length;
            int rhsLength = rhs._length;

            if ((lhsLength == 1) && (rhsLength == 1)) {
                uint quotient = InternalMath.DivRem(lhs._blocks[0], rhs._blocks[0], out uint remainder);
                quo = new BigUIntegerBuilder(quotient);
                rem = new BigUIntegerBuilder(remainder);
                return;
            }

            if (rhsLength == 1) {
                // We can make the computation much simpler if the rhs is only one block

                int quoLength = lhsLength;

                ulong rhsValue = rhs._blocks[0];
                ulong carry = 0;

                for (int i = quoLength - 1; i >= 0; i--) {
                    ulong value = (carry << 32) | lhs._blocks[i];
                    ulong digit = InternalMath.DivRem(value, rhsValue, out carry);

                    if ((digit == 0) && (i == (quoLength - 1))) {
                        quoLength--;
                    } else {
                        quo._blocks[i] = (uint)(digit);
                    }
                }

                quo._length = quoLength;
                rem.SetUInt32((uint)(carry));

                return;
            } else if (rhsLength > lhsLength) {
                // Handle the case where we have no quotient
                rem.SetValue(ref lhs);
                return;
            } else {
                int quoLength = lhsLength - rhsLength + 1;
                rem.SetValue(ref lhs);
                int remLength = lhsLength;

                // Executes the "grammar-school" algorithm for computing q = a / b.
                // Before calculating q_i, we get more bits into the highest bit
                // block of the divisor. Thus, guessing digits of the quotient
                // will be more precise. Additionally we'll get r = a % b.

                uint divHi = rhs._blocks[rhsLength - 1];
                uint divLo = rhs._blocks[rhsLength - 2];

                // We measure the leading zeros of the divisor
                int shiftLeft = BinaryNumerals.CountLeadingZeros(divHi);
                int shiftRight = 32 - shiftLeft;

                // And, we make sure the most significant bit is set
                if (shiftLeft > 0) {
                    divHi = (divHi << shiftLeft) | (divLo >> shiftRight);
                    divLo <<= shiftLeft;

                    if (rhsLength > 2) {
                        divLo |= (rhs._blocks[rhsLength - 3] >> shiftRight);
                    }
                }

                // Then, we divide all of the bits as we would do it using
                // pen and paper: guessing the next digit, subtracting, ...
                for (int i = lhsLength; i >= rhsLength; i--) {
                    int n = i - rhsLength;
                    uint t = i < lhsLength ? rem._blocks[i] : 0;

                    ulong valHi = ((ulong)(t) << 32) | rem._blocks[i - 1];
                    uint valLo = i > 1 ? rem._blocks[i - 2] : 0;

                    // We shifted the divisor, we shift the dividend too
                    if (shiftLeft > 0) {
                        valHi = (valHi << shiftLeft) | (valLo >> shiftRight);
                        valLo <<= shiftLeft;

                        if (i > 2) {
                            valLo |= (rem._blocks[i - 3] >> shiftRight);
                        }
                    }

                    // First guess for the current digit of the quotient,
                    // which naturally must have only 32 bits...
                    ulong digit = valHi / divHi;

                    if (digit > uint.MaxValue) {
                        digit = uint.MaxValue;
                    }

                    // Our first guess may be a little bit to big
                    while (DivideGuessTooBig(digit, valHi, valLo, divHi, divLo)) {
                        digit--;
                    }

                    if (digit > 0) {
                        // Now it's time to subtract our current quotient
                        uint carry = SubtractDivisor(ref rem, n, ref rhs, digit);

                        if (carry != t) {
                            Debug.Assert(carry == t + 1);

                            // Our guess was still exactly one too high
                            carry = AddDivisor(ref rem, n, ref rhs);
                            digit--;

                            Debug.Assert(carry == 1);
                        }
                    }

                    // We have the digit!
                    if (quoLength != 0) {
                        if ((digit == 0) && (n == (quoLength - 1))) {
                            quoLength--;
                        } else {
                            quo._blocks[n] = (uint)(digit);
                        }
                    }

                    if (i < remLength) {
                        remLength--;
                    }
                }

                quo._length = quoLength;

                // We need to check for the case where remainder is zero

                for (int i = remLength - 1; i >= 0; i--) {
                    if (rem._blocks[i] == 0) {
                        remLength--;
                    }
                }

                rem._length = remLength;
            }
        }

        public static uint HeuristicDivide(ref BigUIntegerBuilder dividend, ref BigUIntegerBuilder divisor) {
            int divisorLength = divisor._length;

            if (dividend._length < divisorLength) {
                return 0;
            }

            // This is an estimated quotient. Its error should be less than 2.
            // Reference inequality:
            // a/b - floor(floor(a)/(floor(b) + 1)) < 2
            int lastIndex = (divisorLength - 1);
            uint quotient = dividend._blocks[lastIndex] / (divisor._blocks[lastIndex] + 1);

            if (quotient != 0) {
                // Now we use our estimated quotient to update each block of dividend.
                // dividend = dividend - divisor * quotient
                int index = 0;

                ulong borrow = 0;
                ulong carry = 0;

                do {
                    ulong product = ((ulong)(divisor._blocks[index]) * quotient) + carry;
                    carry = product >> 32;

                    ulong difference = (ulong)(dividend._blocks[index]) - (uint)(product) - borrow;
                    borrow = (difference >> 32) & 1;

                    dividend._blocks[index] = (uint)(difference);

                    index++;
                }
                while (index < divisorLength);

                // Remove all leading zero blocks from dividend
                while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0)) {
                    divisorLength--;
                }

                dividend._length = divisorLength;
            }

            // If the dividend is still larger than the divisor, we overshot our estimate quotient. To correct,
            // we increment the quotient and subtract one more divisor from the dividend (Because we guaranteed the error range).
            if (Compare(ref dividend, ref divisor) >= 0) {
                quotient++;

                // dividend = dividend - divisor
                int index = 0;
                ulong borrow = 0;

                do {
                    ulong difference = (ulong)(dividend._blocks[index]) - divisor._blocks[index] - borrow;
                    borrow = (difference >> 32) & 1;

                    dividend._blocks[index] = (uint)(difference);

                    index++;
                }
                while (index < divisorLength);

                // Remove all leading zero blocks from dividend
                while ((divisorLength > 0) && (dividend._blocks[divisorLength - 1] == 0)) {
                    divisorLength--;
                }

                dividend._length = divisorLength;
            }

            return quotient;
        }

        public static void Multiply(ref BigUIntegerBuilder lhs, uint value, ref BigUIntegerBuilder result) {
            if (lhs.IsZero() || (value == 1)) {
                result.SetValue(ref lhs);
                return;
            }

            if (value == 0) {
                result.SetZero();
                return;
            }

            int lhsLength = lhs._length;
            int index = 0;
            uint carry = 0;

            while (index < lhsLength) {
                ulong product = ((ulong)(lhs._blocks[index]) * value) + carry;
                result._blocks[index] = unchecked((uint)(product));
                carry = (uint)(product >> 32);

                index++;
            }

            if (carry != 0) {
                Debug.Assert(unchecked((uint)(lhsLength)) + 1 <= MaxBlockCount);
                result._blocks[index] = carry;
                result._length = (lhsLength + 1);
            } else {
                result._length = lhsLength;
            }
        }

        public static void Multiply(ref BigUIntegerBuilder lhs, ref BigUIntegerBuilder rhs, ref BigUIntegerBuilder result) {
            if (lhs.IsZero() || rhs.IsOne()) {
                result.SetValue(ref lhs);
                return;
            }

            if (rhs.IsZero()) {
                result.SetZero();
                return;
            }

            ref readonly BigUIntegerBuilder large = ref lhs;
            int largeLength = lhs._length;

            ref readonly BigUIntegerBuilder small = ref rhs;
            int smallLength = rhs._length;

            if (largeLength < smallLength) {
                large = ref rhs;
                largeLength = rhs._length;

                small = ref lhs;
                smallLength = lhs._length;
            }

            int maxResultLength = smallLength + largeLength;
            Debug.Assert(unchecked((uint)(maxResultLength)) <= MaxBlockCount);

            // Zero out result internal blocks.
            // Buffer.ZeroMemory((byte*)result.GetBlocksPointer(), (uint)maxResultLength * sizeof(uint));
            result._blocks = new uint[maxResultLength];
            // result._blocks.AsSpan(0, maxResultLength).Clear();
            
            int smallIndex = 0;
            int resultStartIndex = 0;

            while (smallIndex < smallLength) {
                // Multiply each block of large BigNum.
                if (small._blocks[smallIndex] != 0) {
                    int largeIndex = 0;
                    int resultIndex = resultStartIndex;

                    ulong carry = 0;

                    do {
                        ulong product = result._blocks[resultIndex] + ((ulong)(small._blocks[smallIndex]) * large._blocks[largeIndex]) + carry;
                        carry = product >> 32;
                        result._blocks[resultIndex] = unchecked((uint)(product));

                        resultIndex++;
                        largeIndex++;
                    }
                    while (largeIndex < largeLength);

                    result._blocks[resultIndex] = (uint)(carry);
                }

                smallIndex++;
                resultStartIndex++;
            }

            if ((maxResultLength > 0) && (result._blocks[maxResultLength - 1] == 0)) {
                result._length = (maxResultLength - 1);
            } else {
                result._length = maxResultLength;
            }
        }

        public static void Pow2(uint exponent, out BigUIntegerBuilder result) {
            uint blocksToShift = DivRem32(exponent, out uint remainingBitsToShift);
            var length = (int)blocksToShift + 1;
            Debug.Assert(unchecked((uint)length) <= MaxBlockCount);
            // if (blocksToShift > 0) {
            //     // Buffer.ZeroMemory((byte*)result.GetBlocksPointer(), blocksToShift * sizeof(uint));
            //     // result._blocks.AsSpan(0, (int)blocksToShift).Clear();
            result._blocks = new uint[length];
            // }
            result._blocks[blocksToShift] = 1U << (int)(remainingBitsToShift);
            result._length = length;
        }

        public static void Exp10(uint exponent, out BigUIntegerBuilder result) {
            // We leverage two arrays - s_Exp10UInt32Table and s_Exp10BigNumTable to speed up the Exp10 calculation.
            //
            // s_Exp10UInt32Table stores the results of 10^0 to 10^7.
            // s_Exp10BigNumTable stores the results of 10^8, 10^16, 10^32, 10^64, 10^128, 10^256, and 10^512
            //
            // For example, let's say exp = 0b111111. We can split the exp to two parts, one is small exp,
            // which 10^smallExp can be represented as uint, another part is 10^bigExp, which must be represented as BigNum.
            // So the result should be 10^smallExp * 10^bigExp.
            //
            // Calculating 10^smallExp is simple, we just lookup the 10^smallExp from s_Exp10UInt32Table.
            // But here's a bad news: although uint can represent 10^9, exp 9's binary representation is 1001.
            // That means 10^(1011), 10^(1101), 10^(1111) all cannot be stored as uint, we cannot easily say something like:
            // "Any bits <= 3 is small exp, any bits > 3 is big exp". So instead of involving 10^8, 10^9 to s_Exp10UInt32Table,
            // consider 10^8 and 10^9 as a bigNum, so they fall into s_Exp10BigNumTable. Now we can have a simple rule:
            // "Any bits <= 3 is small exp, any bits > 3 is big exp".
            //
            // For 0b111111, we first calculate 10^(smallExp), which is 10^(7), now we can shift right 3 bits, prepare to calculate the bigExp part,
            // the exp now becomes 0b000111.
            //
            // Apparently the lowest bit of bigExp should represent 10^8 because we have already shifted 3 bits for smallExp, so s_Exp10BigNumTable[0] = 10^8.
            // Now let's shift exp right 1 bit, the lowest bit should represent 10^(8 * 2) = 10^16, and so on...
            //
            // That's why we just need the values of s_Exp10BigNumTable be power of 2.
            //
            // More details of this implementation can be found at: https://github.com/dotnet/coreclr/pull/12894#discussion_r128890596

            // Validate that `s_Exp10BigNumTable` has exactly enough trailing elements to fill a BigUIntegerBuilder (which contains MaxBlockCount + 1 elements)
            // We validate here, since this is the only current consumer of the array
            // Debug.Assert((s_Exp10BigNumTableIndices[^1] + MaxBlockCount + 2) == s_Exp10BigNumTable.Length);

            BigUIntegerBuilder temp1 = new BigUIntegerBuilder(s_Exp10UInt32Table[exponent & 0x7]);
            ref BigUIntegerBuilder lhs = ref temp1;

            BigUIntegerBuilder temp2 = new BigUIntegerBuilder(0);
            ref BigUIntegerBuilder product = ref temp2;

            exponent >>= 3;
            uint index = 0;

            while (exponent != 0) {
                // If the current bit is set, multiply it with the corresponding power of 10
                if ((exponent & 1) != 0) {
                    // Multiply into the next temporary
                    {
                        ref BigUIntegerBuilder rhs = ref s_Exp10BigNumTable[index];
                        Multiply(ref lhs, ref rhs, ref product);
                    }

                    // Swap to the next temporary
                    ref BigUIntegerBuilder temp = ref product;
                    product = ref lhs;
                    lhs = ref temp;
                }

                // Advance to the next bit
                ++index;
                exponent >>= 1;
            }

            result = new BigUIntegerBuilder(0);
            result.SetValue(ref lhs);
        }

        private static uint AddDivisor(ref BigUIntegerBuilder lhs, int lhsStartIndex, ref BigUIntegerBuilder rhs) {
            int lhsLength = lhs._length;
            int rhsLength = rhs._length;

            Debug.Assert(lhsLength >= 0);
            Debug.Assert(rhsLength >= 0);
            Debug.Assert(lhsLength >= rhsLength);

            // Repairs the dividend, if the last subtract was too much

            ulong carry = 0UL;

            for (int i = 0; i < rhsLength; i++) {
                ref uint lhsValue = ref lhs._blocks[lhsStartIndex + i];

                ulong digit = lhsValue + carry + rhs._blocks[i];
                lhsValue = unchecked((uint)digit);
                carry = digit >> 32;
            }

            return (uint)(carry);
        }

        private static bool DivideGuessTooBig(ulong q, ulong valHi, uint valLo, uint divHi, uint divLo) {
            Debug.Assert(q <= 0xFFFFFFFF);

            // We multiply the two most significant limbs of the divisor
            // with the current guess for the quotient. If those are bigger
            // than the three most significant limbs of the current dividend
            // we return true, which means the current guess is still too big.

            ulong chkHi = divHi * q;
            ulong chkLo = divLo * q;

            chkHi += (chkLo >> 32);
            chkLo &= uint.MaxValue;

            if (chkHi < valHi) {
                return false;
            }

            if (chkHi > valHi) {
                return true;
            }

            if (chkLo < valLo) {
                return false;
            }

            if (chkLo > valLo) {
                return true;
            }

            return false;
        }

        private static uint SubtractDivisor(ref BigUIntegerBuilder lhs, int lhsStartIndex, ref BigUIntegerBuilder rhs, ulong q) {
            int lhsLength = lhs._length - lhsStartIndex;
            int rhsLength = rhs._length;

            Debug.Assert(lhsLength >= 0);
            Debug.Assert(rhsLength >= 0);
            Debug.Assert(lhsLength >= rhsLength);
            Debug.Assert(q <= uint.MaxValue);

            // Combines a subtract and a multiply operation, which is naturally
            // more efficient than multiplying and then subtracting...

            ulong carry = 0;

            for (int i = 0; i < rhsLength; i++) {
                carry += rhs._blocks[i] * q;
                uint digit = unchecked((uint)carry);
                carry >>= 32;

                ref uint lhsValue = ref lhs._blocks[lhsStartIndex + i];

                if (lhsValue < digit) {
                    carry++;
                }

                lhsValue = unchecked(lhsValue - digit);
            }

            return (uint)(carry);
        }

        public void Add(uint value) {
            int length = _length;
            if (length == 0) {
                SetUInt32(value);
                return;
            }

            unchecked {
                _blocks[0] += value;
            }
            if (_blocks[0] >= value) {
                // No carry
                return;
            }

            for (int index = 1; index < length; index++) {
                _blocks[index]++;
                if (_blocks[index] > 0) {
                    // No carry
                    return;
                }
            }

            Debug.Assert(unchecked((uint)(length)) + 1 <= MaxBlockCount);
            _blocks[length] = 1;
            _length = length + 1;
        }

        public uint GetBlock(uint index) {
            Debug.Assert(index < _length);
            return _blocks[index];
        }

        public int GetLength() {
            return _length;
        }

        public bool IsOne() {
            return (_length == 1)
                && (_blocks[0] == 1);
        }

        public bool IsZero() {
            return _length == 0;
        }

        public void Multiply(uint value) {
            Multiply(ref this, value, ref this);
        }

        public void Multiply(ref BigUIntegerBuilder value) {
            BigUIntegerBuilder temp = new BigUIntegerBuilder(0);
            temp.SetValue(ref this);
            Multiply(ref temp, ref value, ref this);
        }

        public void Multiply10() {
            if (IsZero()) {
                return;
            }

            int index = 0;
            int length = _length;
            ulong carry = 0;

            while (index < length) {
                ulong block = (ulong)(_blocks[index]);
                ulong product = (block << 3) + (block << 1) + carry;
                carry = product >> 32;
                _blocks[index] = (uint)(product);

                index++;
            }

            if (carry != 0) {
                Debug.Assert(unchecked((uint)(_length)) + 1 <= MaxBlockCount);
                _blocks[index] = (uint)carry;
                _length++;
            }
        }

        public void MultiplyExp10(uint exponent) {
            if (IsZero()) {
                return;
            }

            Exp10(exponent, out BigUIntegerBuilder poweredValue);

            if (poweredValue._length == 1) {
                Multiply(poweredValue._blocks[0]);
            } else {
                Multiply(ref poweredValue);
            }
        }

        public void SetUInt32(uint value) {
            if (value == 0) {
                SetZero();
            } else {
                _blocks[0] = value;
                _length = 1;
            }
        }

        public void SetUInt64(ulong value) {
            if (value <= uint.MaxValue) {
                SetUInt32((uint)(value));
            } else {
                _blocks[0] = (uint)(value);
                _blocks[1] = (uint)(value >> 32);

                _length = 2;
            }
        }

        public void SetValue(ref BigUIntegerBuilder rhs) {
            int rhsLength = rhs._length;
            // Buffer.Memcpy((byte*)GetBlocksPointer(), (byte*)rhs.GetBlocksPointer(), rhsLength * sizeof(uint));
            _blocks = rhs._blocks.Clone() as uint[];
            _length = rhsLength;
        }

        public void SetZero() {
            _length = 0;
        }

        public void ShiftLeft(uint shift) {
            // Process blocks high to low so that we can safely process in place
            int length = _length;

            if ((length == 0) || (shift == 0)) {
                return;
            }

            uint blocksToShift = DivRem32(shift, out uint remainingBitsToShift);

            // Copy blocks from high to low
            int readIndex = (length - 1);
            int writeIndex = readIndex + (int)(blocksToShift);

            // Check if the shift is block aligned
            if (remainingBitsToShift == 0) {
                Debug.Assert(writeIndex < MaxBlockCount);

                while (readIndex >= 0) {
                    _blocks[writeIndex] = _blocks[readIndex];
                    readIndex--;
                    writeIndex--;
                }

                _length += (int)(blocksToShift);

                // Zero the remaining low blocks
                // Buffer.ZeroMemory((byte*)GetBlocksPointer(), blocksToShift * sizeof(uint));
                _blocks.AsSpan(0, (int)blocksToShift).Clear();
            } else {
                // We need an extra block for the partial shift
                writeIndex++;
                Debug.Assert(writeIndex < MaxBlockCount);

                // Set the length to hold the shifted blocks
                _length = writeIndex + 1;

                // Output the initial blocks
                uint lowBitsShift = (32 - remainingBitsToShift);
                uint highBits = 0;
                uint block = _blocks[readIndex];
                uint lowBits = block >> (int)(lowBitsShift);
                while (readIndex > 0) {
                    _blocks[writeIndex] = highBits | lowBits;
                    highBits = block << (int)(remainingBitsToShift);

                    --readIndex;
                    --writeIndex;

                    block = _blocks[readIndex];
                    lowBits = block >> (int)lowBitsShift;
                }

                // Output the final blocks
                _blocks[writeIndex] = highBits | lowBits;
                _blocks[writeIndex - 1] = block << (int)(remainingBitsToShift);

                // Zero the remaining low blocks
                // Buffer.ZeroMemory((byte*)GetBlocksPointer(), blocksToShift * sizeof(uint));
                _blocks.AsSpan(0, (int)blocksToShift).Clear();

                // Check if the terminating block has no set bits
                if (_blocks[_length - 1] == 0) {
                    _length--;
                }
            }
        }

        public ulong ToUInt64() {
            if (_length > 1) {
                return ((ulong)(_blocks[1]) << 32) + _blocks[0];
            }

            if (_length > 0) {
                return _blocks[0];
            }

            return 0;
        }

        private static uint DivRem32(uint value, out uint remainder) {
            remainder = value & 31;
            return value >> 5;
        }

        public unsafe override string ToString() {
            var b = this._blocks;
            if (b is null) {
                return "0";
            }
            var c = this._length;
            return new BigInteger(new ReadOnlySpan<byte>(Unsafe.AsPointer(ref b.AsSpan(0, c)[0]), checked(sizeof(uint) * c)), true).ToString();
        }
    }
}
