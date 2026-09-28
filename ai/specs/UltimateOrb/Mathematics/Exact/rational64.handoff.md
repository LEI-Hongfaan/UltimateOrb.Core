# Rational64 Handoff Document

**Author ID:** DPSK
**Subject:** `UltimateOrb.Mathematics.Exact.Rational64` and friends
**Status:** Work in progress; core arithmetic fixed, some interfaces still stubbed.

---

## 0. How to read this document

Sections are tagged:

- **[REQ]** — a requirement stated explicitly by the project owner in this session.
- **[NOTE]** — my own working note, observation, or suggested convention; not authoritative.
- **[PIT]** — a known pitfall, easily hit by future edits.
- **[OPEN]** — work still pending.

If a **NOTE** conflicts with a **REQ**, the **REQ** wins.

---

## 1. The type

### 1.1 Encoding **[NOTE, but load-bearing]**

`Rational64` is a `readonly struct` with a single `Int64 bits` field:

```
bits  =  (UInt64)(UInt32)e << 32   |   (UInt64)n
n     =  (UInt32)bits                     // non-negative numerator
e     =  (Int32)(bits >> 32)              // encoded signed denominator
```

Decoding:

```
d  =  e >= 0  ?  (Int64)e + 1   :   (Int64)e
```

Encoding a non-zero `d`:

```
e  =  d > 0   ?  (Int32)(d - 1) :   (Int32)d
```

Result: `value = n / d`. Zero is `bits == 0`.

Ranges:

| component | range |
|---|---|
| numerator `n` | `[0, 2^32 - 1]` |
| denominator magnitude `\|d\|` | `[1, 2^31]` |
| encoded `e` | full `Int32` range |

The encoding is **not symmetric**: the numerator has a `2^32` ceiling while the denominator magnitude tops out at `2^31`. This asymmetry is the source of many pitfalls (see §5).

### 1.2 Canonical form **[REQ]**

A `Rational64` is canonical iff:

- `gcd(n, |d|) == 1`, and
- `n == 0` and `bits == 0` (i.e. the only encoding of zero is the all-zero bit pattern).

**All public members expect canonical inputs and produce canonical outputs**, except for members that are explicitly documented as "raw bit manipulation" (`ToInt64Bits`, internal `FromInt64Bits`, the internal `(UInt64)` constructor).

`INumberBase<Rational64>.IsCanonical` is implemented to check exactly this **[already done]**.

### 1.3 `Canonicalize` **[REQ]**

A public `Canonicalize(Rational64) → Rational64` should exist (reduces, normalises sign, maps zero to `bits == 0`) **[OPEN — not yet in the code as of this handoff]**.

---

## 2. Requirements stated by the owner **[REQ]**

1. **Inexact ⇒ throw `OverflowException`** for all operations that are not explicitly named as inexact. Explicitly inexact operations are named `…Inexact` (`ToDoubleInexact`, `ToSingleInexact`, and the plain `operator double`/`single` when so documented).
2. **`SignedDenominator` returns `sign * Denominator`**, and returns `0` if the value is zero. (This is already true.)
3. **Canonical I/O contract** for public members: assume canonical input, produce canonical output, unless the member is a documented raw bit operation.
4. **Provide a `Canonicalize` helper.**
5. **Compile-time context independence**: the file must behave identically whether the surrounding project is compiled with `/checked+` or `/checked-`. Every bit-trick or intentional wrap uses an explicit `unchecked { }`; every range check uses an explicit `checked(...)` cast or a comparison.
6. **No `throw new OverflowException()` on hot paths**: the owner prefers to induce the exception via a guaranteed-to-throw checked arithmetic expression (`_ = checked((Int32)UInt64.MaxValue);`) or via `ThrowHelper.ThrowOn…`. The runtime helper `ThrowHelper.ThrowOnTrue(true)` is used where a boolean predicate naturally expresses the condition.

**[NOTE]** The owner confirmed that `checked` **does** affect unsigned arithmetic. `checked(0u - v)` throws when `v != 0`. This corrects an earlier analysis of mine and matters for `ToInt64`, `ToDoubleExact`, etc.

---

## 3. Contracts and behavioural rules **[NOTE]**

These are the invariants I derived from the encoding plus the requirements. They are not statements the owner dictated, but they are what the code assumes.

- Every binary arithmetic operation that can overflow the encoding throws `OverflowException` rather than truncating.
- For operations with a positive integer exponent, `PowN` inverts the base **first** when the exponent is negative; this avoids a false overflow from the asymmetric bounds (see §5.1).
- `RootN` with a negative `n` also inverts first (already correct in the WIP).
- `Sqrt`/`Cbrt`/`RootN` are exact: they throw when the input is not a perfect power of the relevant degree.
- `Exp`/`Exp2`/`Exp10` are exact: they succeed only in the trivial cases listed below.
- Any `NumberStyles`-aware parsing delegates to `BigRational.Parse` / `TryParse`, then range-checks the result (see §6).

---

## 4. Bugs found and fixed during this session

Each entry: symptom → root cause → fix. Where the fix is in the WIP, a short snippet is included.

### 4.1 `Negate` — wrong for positive inputs

**Symptom:** `-One` returned `Zero`; `-3/2` returned `-3`; `-1/2` returned `-1`.

**Cause:** `c = 0 > c ? ~c : -c;`. The `-c` branch (used when `e >= 0`) decrements the encoded positive denominator, changing magnitude instead of sign. Correct encoding of `-(n/d)` is `~e` **in both branches**.

**Fix (in WIP):**

```csharp
var e = unchecked((Int32)(value.bits >> 32));
e = ~e;
return new Rational64(unchecked(((UInt64)(UInt32)e << 32) | n));
```

### 4.2 `Inverse` — three independent faults

**Symptom:** `Inverse(1/1)` threw; `Inverse(3/1)` returned `1`; `Inverse(3)` returned `2/2`.

**Causes:**
- The guard `checked(d - (UInt32)Int32.MinValue)` threw when `n < 2^31` (inverted predicate).
- Numerator and denominator ended up in the wrong halves of `bits`.
- The `c = -c` on `Int32.MinValue` wraps.

**Fix (in WIP):**

```csharp
if (n == 0u) { UInt32 zero = 0; _ = n / zero; }
_ = checked((Int32)(n - 1u));           // throws when n > 2^31
if (e >= 0) { newN = (UInt32)e + 1u; newE = (Int32)(n - 1u); }
else       { newN = (UInt32)(-(Int64)e); newE = checked((Int32)(-(Int64)n)); }
```

**Verification:** `Inverse(3/1) == 1/3`, `Inverse(1/3) == 3/1`, `Inverse(-1/2^31) == -2^31`, `Inverse(2^31/1) == 1/2^31`. `Inverse((2^31+1)/1)` throws.

### 4.3 `operator %` — reads the divisor from the dividend

**Cause:** both "divisor" locals were loaded from `dividend.bits`.

**[OPEN]** The WIP copy still has the old formula. Recommended replacement uses the correct fields plus the formula `(n1·q2 mod n2·q1) / (q1·q2)` and reduces.

### 4.4 `ToInt64` — dead integrality check, wrong sign path

**Symptom:** `(Int64)(1/2) == 1`; `(Int64)(-22/1) == -4294967274`.

**Causes:**
- `checked(0 - (UInt32)(c ^ e))` promoted both operands to `long`, so the guard never fired.
- The bit reconstruction `((UInt64)c << 32) | n` produced `n - 2^32` for negative `c`, not `-n`.

**Owner-provided fix (now in WIP):**

```csharp
var c = denominator >> 31;                            // 0 or -1
checked(0u - unchecked((UInt32)(c ^ denominator))).Ignore();
return unchecked(0 != c ? -(Int64)numerator : (Int64)numerator);
```

`0u` is required: it keeps the subtraction in `uint` so underflow is detected. **[NOTE]** the original `0` (const literal) does **not** retype to `uint` — C# promotes `int − uint` to `long − long`, so the check was silently dead.

### 4.5 `ToDoubleExact` — inverted predicate, `Int32` truncation at `±2^31`

**Symptom:** `1/2` threw; `1/3` returned `0.333…`; `1/2^31` returned a *negative* value.

**Causes:**
- `ThrowOnTrue(IsPow2(d))` was inverted (throws when the denominator *is* a power of two).
- The divisor `(Int32)c` truncated `c = +2^31` to `Int32.MinValue`.

**Fix (in WIP):** `IsPow2Partial` renamed, and `c` is kept as `Int64` in the division.

### 4.6 `IsPow2` returned `true` for `0`

**Symptom:** latent; would accept `0` as "power of two".

**Fix (in WIP):** renamed to `IsPow2Partial` with a `Debug.Assert(0 != value)`. **[NOTE]** if the owner wants a strict `IsPow2` later, add `value != 0u &&` before the AND.

### 4.7 `Exp2` — relies on `ToInt64`; cleanup done

`ToInt64` is now correct, so `Exp2` works. **[NOTE]** The negative branch uses `unchecked((Int32)absD - 1)` with `absD = 2^31`, relying on two's-complement wrap; `(Int32)(absD - 1u)` is cleaner and avoids depending on wrap.

### 4.8 `SubtractAsRational128` — no zero short-circuit

**[OPEN]** The WIP still lacks the `p == 0` early return that `AddAsRational128` has; `Subtract(x, x)` can produce a non-canonical zero (`bits == 0xFFFFFFFF00000000`). Add:

```csharp
p -= r;
if (p == 0) { bits_hi = 0; return 0; }
```

### 4.9 `GetNextRational64` — non-canonical zero possible

**[OPEN]** When the generated numerator is `0` and the denominator encodes to `-1`, gcd passes and the returned value is `0` in a non-canonical encoding. Guard with `n != 0u`.

### 4.10 `Multiply(Rational64, Int64)` — swallows all exceptions

**[OPEN]** The body is wrapped in `try { … } catch (Exception) { }` with `return default`. This hides `OverflowException` and any other failure. Remove the catch; let the checked arithmetic throw.

### 4.11 `Cbrt` — uses the wrong remainder variable

**[OPEN]** The second `_ = 0u - r.ToUnsignedUnchecked();` references `r` (numerator remainder) instead of `s` (denominator remainder). Also relies on `_ = -sd` in checked context to throw for `Int32.MinValue`. Rewrite to test `s` and to use `ThrowOnTrue`.

### 4.12 `ToSingleExact` — inverted predicate

**[OPEN]** The WIP still has `ThrowOnTrue(IsPow2Partial(d))`; should be `ThrowOnFalse(IsPow2Partial(d))`. Also the significand check `ThrowOnGreaterThan(e, 32 - 24)` has the inequality direction confused; the correct condition is `32 - e > 24`, i.e. `e < 8`.

### 4.13 `FromFraction` — sign loss for `Int32.MinValue` denominator

**[OPEN]** `denominator /= (Int32)d;` with `d == 2^31` casts to `Int32.MinValue` and yields `+1`. Use `denominator = unchecked((Int32)(denominator / (Int64)d));`.

### 4.14 `FromInt64Bits` — reduction applied to `(UInt32)(-denominator)` before division

**[OPEN]** `(UInt32)(-denominator) / d` loses the sign for reduced inputs whose denominator is not already `±1`. Use `denominator / (Int64)d` (64-bit) and re-encode.

### 4.15 `IsComplexNumber` returned `true`

**Fixed (in WIP):** returns `false`.

---

## 5. Pitfalls worth writing on the wall **[PIT]**

### 5.1 Asymmetric bounds bite when inverting after the fact

Because the numerator ceiling is `2^32 - 1` but the denominator magnitude ceiling is `2^31`, computing `x^k` and then taking `Inverse` uses the **wrong** bounds for `x^(-k)`. Concretely: `PowN(1/3, -20)` computed as `Inverse(PowNPositive(1/3, 20))` falsely overflows; `Inverse` first (`3^20/1`) fits.

**Rule:** invert the base first; then apply the asymmetric operation. `PowN` and `RootN` both follow this.

### 5.2 `checked` and unsigned subtraction

`checked(uint - uint)` throws when the mathematical result would be negative. `checked(int - uint)` does **not** — the operands are promoted to `long`, and `long` subtraction is not overflow-prone for the value ranges involved. If you want a range check to fire, both operands must be `uint`.

### 5.3 `Int32.MinValue` is not the negative of `Int32.MaxValue + 1`

Encoding uses the full `Int32` range. `e = Int32.MinValue` encodes `d = -2^31`, which is a legal denominator. Any `-e`, `(UInt32)(-e)`, or `(Int32)(-e)` on that value must be done in `Int64`.

### 5.4 Distinguish the canonical zero from "a numerator of zero"

`bits == 0` is the only canonical zero. `0xFFFFFFFF00000000` (`n = 0`, `d = -1`) and `0x0000000100000000` (`n = 0`, `d = 2`) are non-canonical zeros. Any method that short-circuits on `n == 0` (e.g. `Negate`, `Sqrt`, `RootNPositive`) should return `default`, **not** the original encoding, when its input might be non-canonical.

### 5.5 `ThrowOnTrue(true)` is the slow-path idiom here

Throughout the file, the pattern `Utilities.ThrowHelper.ThrowOnTrue(true);` is used to raise `OverflowException` without a direct `throw`. On the JIT this compiles to a call that never returns on the failure path; that is intentional and preferred by the owner.

### 5.6 `ToInt64` and `ToInt64Nearest` are different contracts

`ToInt64` is exact (throws for non-integer input). `ToInt64Nearest` rounds to nearest, ties away from zero; it must not throw for representable values. Do not use `ToInt64` inside `ToInt64Nearest`.

### 5.7 The `BigRational`-delegated `Parse` needs a range check, not a `try/catch`

`(Rational64)big` should throw `OverflowException` when out of range; `TryParse` uses `TryFromBigRational` (which does the same range check on the `BigInteger` fields) so no exception is raised on the failure path.

---

## 6. `BigRational` interop **[REQ, WIP]**

`Parse`, `TryParse`, `TryFormat` delegate to `BigRational`. The conversion `(Rational64)BigRational` must throw `OverflowException` when:

- `|BigRational.Numerator| > UInt32.MaxValue`, or
- `BigRational.Denominator > 2^31`.

`TryFromBigRational` performs the check directly on `BigInteger` fields (not by catching) and is shared with `Parse`.

Formatting delegates `BigRational.TryFormat` verbatim; output is "p/q" (or BigRational's own format), which is canonical.

---

## 7. Pending work

### 7.1 `Log` family **[OPEN, next task]**

The owner plans to port `Log`, `Log2`, `Log10`, and `Log(x, base)` next. Starting point from the current implementation of `Exp`:

- Rational `Log(y)` is non-zero only when `y == 1` (return `0`); the rest require transcendentals.
- `Log2(y)` is rational iff `y = 2^k` for integer `k`; value is `k`.
- `Log10(y)` is rational iff `y = 10^k` for integer `k`; value is `k`.
- `Log(y, b) = Log(y) / Log(b)`; rational only in the cases above for both operands.

Approach: decode numerator and denominator, check that the reduced value equals `base^k`, return the integer `k` as a `Rational64`.

### 7.2 `TryConvertFrom*` / `TryConvertTo*` stubs **[OPEN]**

The six `INumberBase<Rational64>.TryConvert*` members still throw `NotImplementedException`. At least the primitive-integer and `double`/`float` cases should be wired through the existing `ToRational64(...)` / `ToInt64(...)` conversions and the `ToDoubleExact` / `ToSingleExact` paths.

### 7.3 `TryFormat` and the `TryFormat` interfaces **[OPEN, WIP]**

`ISpanFormattable.TryFormat` and `IUtf8SpanFormattable.TryFormat` delegate to `BigRational`; UTF-8 variant still to be wired if `BigRational` exposes it, otherwise via `Encoding.UTF8`.

### 7.4 Cleanups to complete

- The `operator %` fix (§4.3).
- The `SubtractAsRational128` zero short-circuit (§4.8).
- The `GetNextRational64` non-canonical-zero guard (§4.9).
- Remove the catch in `Multiply(Rational64, Int64)` (§4.10).
- `Cbrt` variable and inequality fixes (§4.11).
- `ToSingleExact` predicate and significand check (§4.12).
- `FromFraction` / `FromInt64Bits` 64-bit division (§4.13, §4.14).
- Add public `Canonicalize` (§1.3).

---

## 8. Verified / expected behaviour (for regression tests)

| call | expected |
|---|---|
| `FromFraction(3, 2)` | `3/2` |
| `FromFraction(2147483648u, Int32.MinValue)` | `-1` (currently wrong — see §4.13) |
| `-One` | `MinusOne` |
| `Inverse(3/1)` | `1/3` |
| `Inverse(-1/2^31)` | `-2^31` |
| `Inverse((2^31 + 1)/1)` | `OverflowException` |
| `(Int64)(UInt32.MaxValue / 1)` | `4294967295` |
| `(Int64)(-(UInt32.MaxValue) / 1)` | `-4294967295` |
| `(Int64)(1/2)` | `OverflowException` |
| `ToDoubleExact(1/2)` | `0.5` |
| `ToDoubleExact(1/3)` | `OverflowException` |
| `ToDoubleExact(1/2^31)` | `4.6566…e-10` (positive) |
| `PowN(2/1, 31)` | `2^31` |
| `PowN(2/1, 32)` | `OverflowException` |
| `PowN(1/2, -31)` | `2^31` |
| `PowN(1/2, -32)` | `OverflowException` |
| `PowN(1/3, -20)` | `3^20` (no false overflow) |
| `Pow(1/1, anything)` | `1/1` |
| `Pow(-1/1, 2/3)` | `1/1` |
| `Pow(-1/1, 1/2)` | `OverflowException` |
| `Pow(4/9, 1/2)` | `2/3` |
| `Pow(4/9, -3/2)` | `27/8` |
| `Hypot(3/5, 4/5)` | `1/1` |
| `Hypot(3/2^30, 4/2^30)` | `5/2^30` |
| `Hypot(1/1, 1/1)` | `OverflowException` (√2) |
| `Sqrt(4/9)` | `2/3` |
| `Sqrt(2/1)` | `OverflowException` |
| `Sqrt(-4/1)` | `OverflowException` |
| `Cbrt(8/27)` | `2/3` |
| `RootN(8/27, 3)` | `2/3` |
| `RootN(8/27, -3)` | `27/8` |
| `RootN(-8/1, 2)` | `OverflowException` (even root of negative) |
| `RootN(2/1, 1000)` | `OverflowException` |
| `Exp(0)` | `1` |
| `Exp(1)` | `OverflowException` |
| `Exp2(0)` | `1` |
| `Exp2(31)` | `2^31` |
| `Exp2(-31)` | `1/2^31` |
| `Exp2(32)` | `OverflowException` |
| `Exp2(1/2)` | `OverflowException` |
| `Exp10(9)` | `10^9` |
| `Exp10(-9)` | `1/10^9` |
| `Exp10(10)` | `OverflowException` |
| `default` | `0`, canonical |
| `IsCanonical(default)` | `true` |
| `IsCanonical(new Rational64(0xFFFFFFFF00000000UL))` | `false` |

---

## 9. Files / locations **[NOTE]**

- Main struct: `UltimateOrb.Mathematics.Exact.Rational64` (single file, `partial struct`).
- Extension module: `UltimateOrb.Mathematics.Exact.Rational64Module` (random generation).
- Dependencies assumed present:
  - `UltimateOrb.Mathematics.NumberTheory.EuclideanAlgorithm` — `GreatestCommonDivisorPartial(UInt32, UInt32)`, `GreatestCommonDivisor(UInt64, UInt64)`, `GreatestCommonDivisor(BigInteger, BigInteger)`.
  - `UltimateOrb.Mathematics.Elementary.Math` — `AbsAsUnsigned(Int64)`, `SqrtRem(UInt32, out UInt32)`, `CbrtRem(Int32, out Int32)`.
  - `UltimateOrb.Utilities.ThrowHelper` — `ThrowOnTrue`, `ThrowOnFalse`, `ThrowOnNonZero`, `ThrowOnGreaterThan`, `ThrowOnNegative`, `ThrowOnInfinite`, `ThrowOnLessThanOrEqual`.
  - `UltimateOrb.Numerics.DoubleArithmetic` — `BigSqrtRem`, `BigRemInternal`.
  - `UltimateOrb.BigRational` — `Parse`, `TryParse`, `TryFormat`, `Numerator`, `SignedDenominator` (used by `TryFromBigRational`).
- `IntegerGenericMathImpl` — external helper providing `MaxMagnitude` / `MinMagnitude` for the `INumberBase` implementations.
- `BooleanIntegerModule.GreaterThanOrEqual` — used by `FromFraction`.

---

## 10. Conventions used in the codebase **[NOTE]**

- `[TargetedPatchingOptOut("")]`, `[MethodImpl(MethodImplOptions.AggressiveInlining)]`, `[Pure]` decorate nearly everything; keep that up for consistency.
- `unchecked { … }` at the method level for bit-manipulation-heavy bodies.
- `Utilities.ThrowHelper.ThrowOn…` instead of `if (…) throw …` on hot paths.
- Debug-only public `(UInt32, Int32)` constructor for tests; shipped API uses `FromFraction`.
- `IsInteger(x)` is implemented as `1 + (UInt32)(x.bits >> 32) <= 1`, i.e. `e ∈ {0, -1}`; this covers both `d = 1` and `d = -1`.

---

## 11. Open questions for the owner **[OPEN]**

1. Should `Exp(0) == One` (returning `1`) be the only success case, or should `Exp` also return `1` for other inputs somehow? Currently only `0` succeeds.
2. Should `Pow(0, 0)` return `1` (matching `Math.Pow`) as implemented, or throw?
3. Should `Log2`/`Log10`/`Log` be implemented as members of `ILogarithmicFunctions<Rational64>` (which currently isn't in the interface list) or as plain `public static` methods? The current interface list includes `IExponentialFunctions`, `IPowerFunctions`, `IRootFunctions`, but not `ILogarithmicFunctions`.
4. UTF-8 parsing/formatting — does `BigRational` expose `IUtf8SpanFormattable` / `IUtf8SpanParsable`, or should we bridge via `Encoding.UTF8`?
5. The remaining `TryConvertFrom*` / `TryConvertTo*` stubs: which conversions are in scope? At minimum, primitive integers, `double`, `float`, and `decimal` are candidates.

---

**End of handoff.**