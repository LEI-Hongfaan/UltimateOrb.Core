# Handoff Document — `Quadruple.TryFormat` / `ToString` and friends

**Author:** DPSK (AI agent)
**Scope:** `TryFormat(Span<char>, out int, ReadOnlySpan<char>, IFormatProvider?)` and every helper in `QuadrupleFormatter` (`System` namespace: `General`, `Fixed`, `Exponential`, `Numeric`, `RoundTrip`, `Currency`, `Percent`, `CustomFormat`) plus `DecimalDigits`, `RoundTripDigits`, `Decoded`, `Writer`, `Ratio`, and the parsing counterpart `QuadrupleNumber`.

**Reading convention:**
- **[REQ]** marks a requirement the user (project owner) stated explicitly.
- **[WORK]** marks my own working notes, deductions, or decisions I made while assisting — they are *my* interpretation, not (yet) sanctioned requirements, and the reviewer should confirm each before treating it as binding.

---

## 1. Purpose and scope

The `Quadruple` struct is an IEEE 754 binary128 type. It already has parsing, arithmetic, and binary interop. This work adds a spec‑compliant `TryFormat` / `ToString` surface for both **standard numeric format strings** (`G`, `F`, `E`, `N`, `R`, `C`, `P`) and **custom numeric format strings** (`0`, `#`, `.`, `,`, `%`, `‰`, `E0`/`E+0`/`E-0`, `\`, `'…'`, `"…"`, `;` sections, literal characters).

The single hardest part is producing correctly-rounded decimal output for a binary value whose significand is 113 bits. Everything in the formatting path must be **exact** — no `double` arithmetic anywhere in the decision chain.

---

## 2. Requirements (user-stated) — **[REQ]**

1. **Correct rounding.** All formatters must produce the correctly rounded result under **round-half-to-even**, matching the documented .NET behaviour of `double`, `Half`, and `Single`.
2. **Both families supported.** Standard numeric format strings *and* custom numeric format strings must both work; the two families interact with `NumberFormatInfo` exactly as they do for the built-in floating-point types.
3. **Modern C# / .NET.** C# 14+, .NET 11+, ICU‑driven `NumberFormatInfo`.
4. **Integer log primitives are available.** `ILog2(BigInteger)` (trivial via `GetBitLength`) and `ILog10(BigInteger)` (provided by the user, with exact refinement after a `BigInteger.Log10` estimate) are the only approved floating-point‑log surrogates. `Math.Floor` on the *product* `log2(v)·log10(2)` is allowed as a **starting estimate** but must be corrected with exact integer comparisons.
5. **No `Quadruple` arithmetic for scaling.** `%`, `‰`, and custom‑format comma scaling must be applied on the `BigInteger` significand or via a decimal‑exponent shift, never by multiplying a `Quadruple`. Reason: `Quadruple.MaxValue × 1000` overflows, and a percent format on the largest finite value must still format correctly.
6. **Precision cap.** A parsed precision specifier `> 999,999,999` must throw `FormatException`. (This is the .NET 7+ rule and applies to all numeric types, including `BigInteger`.)
7. **Checked context.** The project compiles with `<CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>`. Any potentially overflowing `int` operation must either be provably in-range or wrapped in `unchecked`.
8. **No trailing-zero surprises at extreme precision.** When the caller asks for more digits than the value's exact decimal expansion contains, the writer must pad with zeros — it must *not* truncate, and it must not misplace digits.
9. **Both `Quadruple.Epsilon` and ordinary values format correctly.** E.g. `(123.456).ToString("42_70_00") == "42_71_23"` and `Quadruple.Epsilon.ToString("42_70_00") == "42_70_00"` — both are correct outputs for their inputs.
10. **Percent pattern** must respect `NumberFormatInfo.PercentPositivePattern` / `PercentNegativePattern`. On ICU this is typically **1** (`"n%"`), on NLS **0** (`"n %"`); tests must pin the culture or read the pattern from NFI.

---

## 3. Non-goals (for this work stream)

- **Not** implementing `IParsable`/`ISpanParsable` — the parse side already exists (`QuadrupleNumber`).
- **Not** implementing `IConvertible` beyond what already exists.
- **Not** changing the conversion operators (`(double)`, `(decimal)`, etc.).
- **Not** a shortest-round-trip Ryu-style formatter *beyond* what `RoundTripDigits` already does (see §6.7) — the current design is exact and correct, but not the fastest possible.

---

## 4. Architecture at a glance

```
Quadruple.TryFormat(lo, hi, dst, out written, format, provider)
        │
        ├─ Decoded.Decode(lo, hi)                  → sign, significand (BigInteger), exp2, class
        │
        ├─ NaN / ±∞                                → NumberFormatInfo symbol copy
        │
        ├─ format.IsEmpty                          → General(precision = -1)
        ├─ standard specifier                      → General | Fixed | Exponential | Numeric
        │                                             | RoundTrip | Currency | Percent
        └─ anything else                           → CustomFormat
                                                        │
                        all of the above reach ─────────┘
                                │
                                ▼
                        DecimalDigits (exact)
                                │
                                ▼
                        Writer (spans) → dst
```

Helper types:

| Type | Role |
|---|---|
| `Decoded` | Bit‑unpacking → `sign, Significand : BigInteger, Exp2 : int`. Subnormals are normalised to `Sig ∈ [1, 2^112)`, `Exp2 = −16494`. |
| `BigIntLog.ILog2` | `GetBitLength() − 1`. |
| `Writer` | `TryAppend` / `TryAppendZeros` into a `Span<char>`. |
| `Ratio` | Exact rational (`Num/Den`) for the round‑trip basin test. |
| `DecimalDigits` | Exact decimal conversion: `DecimalExponent`, `RoundToScale`, `GetSignificant`, `GetFixed`. |
| `RoundTripDigits` | Shortest round‑tripping decimal via binary search over N ∈ [1, 37]. |
| `QuadrupleFormatter` | Per‑specifier emitters. |
| `QuadrupleNumber` | The parse side (out of scope here). |

---

## 5. The exact-rounding core — `DecimalDigits`

### 5.1 Decoded representation

For **any** finite non‑zero Quadruple,

```
|value| = Significand · 2^Exp2
```

where
- normal: `Significand ∈ [2^112, 2^113)`, `Exp2 ∈ [−16494, 16271]`;
- subnormal: `Significand ∈ [1, 2^112)`, `Exp2 = −16494`.

This is the *only* representation the digit generators ever see; **no sign** is passed in.

### 5.2 Exact `floor(log10(|value|))` — `DecimalExponent`

```csharp
int E = (int)Math.Floor(((long)ILog2(sig) + exp2) * Log10Of2);   // estimate
while (CompareSigExp2To10Pow(sig, exp2, E)     < 0) E--;         // correct
while (CompareSigExp2To10Pow(sig, exp2, E + 1) >= 0) E++;
```

`CompareSigExp2To10Pow(sig, exp2, E)` returns the sign of `sig·2^exp2 − 10^E` using only `BigInteger` shifts and `BigInteger.Pow(5, |E|)`. The estimate is provably within ±1 of the true value, so the loops execute at most once each.

**[WORK]** Note for reviewers: the cast `(int)Math.Floor(...)` is unchecked because the argument is bounded; the shift counts inside `CompareSigExp2To10Pow` are bounded by the caller's cap.

### 5.3 Exact round-half-to-even scaling — `RoundToScale`

```csharp
BigInteger RoundToScale(BigInteger sig, int exp2, int k) {
    int e2 = exp2 + k;
    BigInteger num = sig, den = BigInteger.One;
    if (e2 >= 0) num <<= e2; else den <<= -e2;
    if (k  >= 0) num *= BigInteger.Pow(5,  k);
    else         den *= BigInteger.Pow(5, -k);
    BigInteger q = BigInteger.DivRem(num, den, out var r);
    var twice = r << 1;
    int c = twice.CompareTo(den);
    if (c > 0 || (c == 0 && !q.IsEven)) q += 1;
    return q;
}
```

This computes `round_half_even(|value| · 10^k)` exactly. **This is the only place where a rounding decision is made** in the whole formatting path. It has no floating-point inputs and no truncated intermediates.

### 5.4 Two digit-string producers

- `GetSignificant(sig, exp2, N, out digits, out decExp)` → N significant digits, `decExp = E + 1` (so `|value| = 0.digits × 10^decExp`). Handles a carry that yields an extra digit (`999…9 → 1000…0`).
- `GetFixed(sig, exp2, fracDigits, out digits, out decExp)` → rounded to `fracDigits` decimal places, `decExp = digits.Length − fracDigits`. `digits` may be shorter than `fracDigits + 1`, and may have more digits than `fracDigits + 1` (if the integer part is large); the **caller is responsible for padding / trimming** (see §6.5 for the actual writer behaviour).

### 5.5 **Critical** — cap on computed digits

To prevent a caller‑supplied precision of `10^9` from materialising `5^999999999`, both producers clamp:

```csharp
public const int MaxSignificantComputed = 11_563 + 4;   // §6.5 reviewer note
public const int MaxFractionalComputed  = 16_494 + 4;
```

**Derivation of the constants — [REQ]**

The project owner stated the maximum digits that *any* finite Quadruple can produce:

| case | exact magnitude | max digits | where attained |
|---|---|---|---|
| `exp2 ≥ 0` | integer, `sig·2^exp2` | **4933** | `MaxValue = (2^113 − 1)·2^16271` |
| `exp2 < 0`, largest numerator for fixed exp2 | `sig·5^16494 / 10^16494` | **11563** | `sig = 2^113 − 1, exp2 = −16494` |
| smallest positive, whole number of digits | `1 · 5^16494 / 10^16494` | **11529** | `Epsilon = 2^−16494` |

The project owner's earlier `16494 − 4966 + 1 = 11529` figure is the *Epsilon* count, not the global maximum. The **11563** figure is what the cap must cover. Both bounds are recorded here so reviewers can re-derive them.

**[WORK]** I suggested adding `MaxSignificantComputed = 11_563 + 4` (headroom of 4) and `MaxFractionalComputed = 16_494 + 4`. The +4 is my safety margin; the project owner should confirm.

### 5.6 Interaction of cap and writers

When `compute < requested`, the writer is responsible for padding the remaining positions with `'0'`. Because every Quadruple's exact decimal terminates before position 16494 (fractional) or before 11563 significant digits, the padded positions are provably zero. This is why the cap is harmless.

---

## 6. The shortest-round-trip core — `RoundTripDigits`

### 6.1 The contract

For `R` and default `G`, the output must be
1. the shortest decimal that parses back to the same Quadruple, and
2. correctly rounded.

**Requirement 1 is stronger than correct rounding.** It is not satisfied by computing 36 significant digits and trimming trailing zeros: that is correct-rounding but not necessarily shortest.

### 6.2 The basin

The set of decimals that round (nearest, ties‑even) back to a Quadruple `v = sig·2^exp2` is the open/closed interval delimited by the midpoints with the neighbours `prev` and `next`. Endpoints are **included** iff `sig` is even.

### 6.3 Neighbour computation — **[REQ]** edge cases

- **Smallest positive:** `sig == 1, exp2 == −16494` → no `prev`. Lower bound is `v/2`.
- **Largest finite:** `sig == 2^113 − 1, exp2 == 16271` → no `next`; the "would‑be next" is `1.0…0₂ × 2^16384`, i.e. `(2^112, 16272)`. Lower-bound correction must **not** clobber `pE`.
- **Boundary between subnormal and normal:** `sig == 2^112` with `exp2 > −16494` → `prev = (2^113 − 1, exp2 − 1)`.
- **Boundary below every normal:** `sig == 2^113 − 1` → `next = (2^112, exp2 + 1)`.

The `Neighbours` routine encodes all four cases.

### 6.4 The basin test — `IsInBasin`

All comparisons are exact `BigInteger` cross-multiplications via `Ratio`. There is no floating-point anywhere. Endpoint inclusion uses `sigEven` (parity of the significand) as the tie-breaker, which is correct because `sig` even ⇒ value is exactly halfway between its neighbours ⇒ tie-to-even applies.

### 6.5 Shortest search

```csharp
for (int N = 1; N <= 37; N++) {
    DecimalDigits.GetSignificant(sig, exp2, N, out var d, out var e);
    if (IsInBasin(sig, exp2, BigInteger.Parse(d), e - d.Length)) { …; return; }
}
```

**[WORK]** The 37 cap is `⌈113·log10 2⌉ + 1`. There is currently no early-exit optimisation; the loop always runs the full N = 1..37 for hard cases. A future Ryu-style algorithm can replace this, but the current version is correct.

---

## 7. Emitters — `QuadrupleFormatter`

### 7.1 Precision parsing (**[REQ]** rules)

```
[letter][digits]  →  standard specifier with precision
[letter][anything else]  →  custom specifier
[digits only / first char not a letter]  →  custom specifier
```

**`> 999,999,999` must throw `FormatException`**, not fall through to custom. This is the .NET 7+ change and was one of the fixes applied.

`TryParsePrecision` still exists but is now dead code; remove or keep for reference.

### 7.2 NaN / ±∞

Before any specifier dispatch:

```
IsNaN       → copy NaNSymbol
IsPosInf    → copy PositiveInfinitySymbol
IsNegInf    → copy NegativeInfinitySymbol
```

These bypass the format string entirely, matching the .NET documented behaviour.

### 7.3 `General` (G, g)

- `precision <= 0`: shortest round-trip digits from `RoundTripDigits`.
- `precision > 0`: `GetSignificant` with N = precision, then trailing-zero trim.
- Threshold for fixed vs scientific: **`threshold = 34`** for default G, **`threshold = precision`** for explicit G<prec>.

**[WORK]** The choice `threshold = 34` for default G mirrors `Double` (=15) and `Single` (=7): `floor(significandBits · log10 2)`. I picked this value; the project owner should confirm it matches their intended behaviour (an earlier draft used `digits.Length`, which produced `"1E+03"` for `1000`).

- Scientific branch routes through `WriteScientific`; **fixed branch routes through `WriteFixed`**. This is essential because `WriteFixed` handles `decExp ≤ 0` correctly (emits `"0."` then leading zeros), which the earlier inline code did not.

### 7.4 `Fixed` (F, f)

Straight `GetFixed` → `WriteFixed`. `decExp ≤ 0` handled by `WriteFixed`.

### 7.5 `Exponential` (E, e)

`GetSignificant(N = precision + 1)` → `WriteScientific` with `targetFracDigits = precision`. Pad with zeros via `TryAppendZeros`.

The zero-value branch emits `"0.000…E+000"` with 3 exponent digits (as required by the standard).

### 7.6 `Numeric` (N, n), `Currency` (C, c), `Percent` (P, p)

All three build an internal **body** (with the appropriate separator set) and wrap it in the NFI pattern.

- `Numeric` uses `Number*` separators.
- `Currency` uses `Currency*` and a `CurrencyPositivePattern` / `CurrencyNegativePattern` pattern.
- `Percent` scales **the BigInteger significand by 100**, not the Quadruple (requirement §2.5). Then `GetFixed` → `WriteGrouped`.

Patterns are indexed by the `NumberFormatInfo` property values:
- `CurrPos` / `CurrNeg` / `PctPos` / `PctNeg` are the standard tables. Pattern lengths match the `NumberFormatInfo` documentation.

**Critical requirement [REQ]:** `PercentPositivePattern` is **runtime data**, not a fixed answer. On ICU (modern .NET) the value is typically `1` → `"28.44%"`. On NLS the value is `0` → `"28.44 %"`. Tests must either pin the culture or read the pattern from NFI at test time.

### 7.7 `RoundTrip` (R, r)

`RoundTripDigits.Get` → fixed branch is `WriteFixed(digits, decExp, digits.Length − decExp, …)`; scientific branch is `WriteScientific(…, targetFracDigits = digits.Length − 1, …)`.

The fixed branch was the site of a real bug (see §9.1); routing through `WriteFixed` fixed it.

### 7.8 `CustomFormat` — the custom numeric specifier language

The handler parses the format string into up to three sections (`positive; negative; zero`), scans for placeholders, and emits digits positionally.

**Rules the implementation must honour:**

1. **Section sign suppression.** The auto‑prepended `NegativeSign` is emitted **only** for single‑section formats. Two‑ or three‑section formats require the author to put `-` (or `(…)`) in the negative section. The old code's `neg.IsEmpty` clause was wrong and was removed.
2. **`#` suppression.**
   - **Integer `#`:** emit digit if the position is at or below the highest significant digit; otherwise emit nothing.
   - **Fractional `#`:** emit digit if the position is at or above the lowest significant digit; otherwise emit nothing. But `#` **left** of the last significant fractional digit is emitted as `'0'` (this is the `0003` → `3` vs `.5` → `.5` distinction).
3. **`0` always emits.**
4. **`,` between placeholders** = group separator. **`,` before the decimal point** (with no intervening placeholder) = scale by 1000× per comma. The scale must be applied to the BigInteger significand, not to a Quadruple.
5. **`%` and `‰`** can appear multiple times; each multiplies by 100 or 1000 respectively. **They must be applied to the significand** (§2.5).
6. **`E0`/`E+0`/`E-0`/`e0`/`e+0`/`e-0`** = scientific. The sign character forces the sign to be emitted for both positive and negative exponents; without it, only negative exponents get a sign.
7. **`\` and quoted literals** must bypass all specifier interpretation.
8. **Precision scaling.** When the format requests more digits than the value has, pad with zeros. When it requests fewer, round correctly via `RoundToScale`.

### 7.9 The `WriteFixed` / `WriteGrouped` / `WriteScientific` trio

All three are now written to handle four regions:

| decExp region | behaviour |
|---|---|
| `decExp ≤ 0` | emit `'0'`, separator, `−decExp` zeros, then digits, then pad to `fracDigits`. |
| `0 < decExp < digits.Length` | digits split at `decExp`, no zero padding of either side unless requested. |
| `decExp == digits.Length` | all digits, then pad with `'0'` to `decExp`. |
| `decExp > digits.Length` | all digits, then `decExp − digits.Length` zeros, no separator. |

`WriteGrouped` additionally inserts group separators every `NumberGroupSizes[0]` positions in the integer part.

`WriteScientific` accepts `targetFracDigits` so the caller can request a specific number of fractional digits; the writer pads with zeros via `TryAppendZeros`.

---

## 8. Checked-context considerations — **[REQ]**

The project compiles checked. Every arithmetic site that can overflow `int` must be `unchecked` or provably in‑range. The sites identified and treated:

1. `(int)Math.Floor(log2v * Log10Of2)` in `DecimalExponent` — **unchecked**.
2. `-e2` and `-k` shifts in `RoundToScale` — **unchecked** on the negation (still bounded in practice).
3. `percentMul * 100 + perMilleMul * 1000` in `CustomFormat` — **overflow risk**; use `BigInteger.Pow(100, percentMul) * BigInteger.Pow(1000, perMilleMul)` or fold into a single `long` decimal‑shift.
4. `3 * scaleCommas` — **overflow risk**; same fix.
5. `M_int = intZeros + intHashes` — **overflow risk** for pathological inputs; clamp in the pre-scan.
6. `totalFrac − scaleShift` — **underflow risk**; compute in `long`.
7. `decExp − 1` and its negation inside the custom‑format scientific path — **unchecked**.
8. `fracPlaceholdersSeen + 1` — bounded by section length; safe.

**These items are not all yet fixed in the posted code.** Items 3–7 should be re‑verified before shipping.

---

## 9. Known bugs and fixes — chronological record

### 9.1 Fixed (during this stream)

- **B1:** `General` fixed branch incorrectly placed `digits[0]` at the units position when `decExp ≤ 0`. Symptom: `(0.2844…).ToString("G36")` produced `"2.2844…"`. Fix: route the fixed branch through `WriteFixed`.
- **B2:** `General` / `RoundTrip` fixed branch threw for `decExp < 0` (`digits.AsSpan(negative, …)`). Same fix.
- **B3:** `WriteGrouped` had the same units-place bug and threw for `decExp < 0`. Fix: add the `decExp ≤ 0` branch and drop the `Math.Max(1, decExp)`.
- **B4:** Precision `> 999,999,999` fell through to `CustomFormat` instead of throwing. Fix: throw `FormatException` in the parsing site.
- **B5:** `Neighbours` clobbered `pE` on the `max finite` branch — **[REQ]** fix.
- **B6:** `IsInBasin` had no upper bound when `nS.IsZero`. Fix: bound by the hypothetical next `(2^112, 16272)` — **[REQ]** fix.
- **B7:** DoS via huge precision. Fix: caps on computed digits.
- **B8:** `CustomFormat` treated `#` as `0`. Fix: significant-range suppression.
- **B9:** `CustomFormat` ignored scaling commas. Fix: fold into `scaleShift`.
- **B10:** Empty‑negative‑section sign emission was wrong. Fix: only emit auto‑sign for single‑section.

### 9.2 Open / not fully addressed

- **O1:** `WriteScientific` with `precision + 1 > MaxSignificantComputed` will produce fewer fractional digits than requested. Either refuse or pad — project owner decision needed.
- **O2:** Grouping in custom formats does not use `NumberGroupSizes` directly; it inserts group separators positionally. For typical patterns (`#,###`) this is fine, but non‑uniform group sizes (e.g. `{3,3,0}`) may differ from `N` behaviour.
- **O3:** The `[WORK]` threshold `34` for default G needs confirmation.
- **O4:** Checked‑context items 3–7 in §8 need re‑verification.
- **O5:** `RoundTripDigits.Get` recomputes `DecimalExponent` and `Pow(5, k)` for each N; performance can be improved (not a correctness issue).
- **O6:** TryParsePrecision is dead code.

---

## 10. Common pitfalls — checklist for anyone editing this code

1. **`decExp` can be ≤ 0.** Do *not* index `digits[decExp]` or `digits.AsSpan(decExp, …)` without clamping. Every writer must handle `decExp ≤ 0` first.
2. **`decExp` can exceed `digits.Length`.** The integer part may need trailing zeros *before* the decimal point.
3. **`digits.Length − decExp` may be negative.** `WriteFixed`'s `fracDigits ≤ 0` early-return is deliberate.
4. **Do not multiply a Quadruple by 100/1000.** Always scale the significand.
5. **Do not use `double` in the rounding decision.** `Math.Log10`, `Math.Floor` are *only* starting estimates.
6. **The rounding site is a single function.** If you find yourself rounding elsewhere, stop and use `RoundToScale`.
7. **The basin test is exact.** Endpoint inclusion is parity‑driven; do not accidentally flip the even/odd condition.
8. **`sig` and `exp2` may represent a subnormal.** Subnormal `exp2 = −16494` regardless of the top bit; normal `exp2 = biased − 16495`.
9. **`-e2`, `-k`, `-expVal` overflow in checked context.** Wrap in `unchecked`.
10. **Format strings starting with a non-letter** are custom. Format strings starting with a letter and ending with a non‑digit are custom. Only fully‑numeric specifiers after the letter are standard.
11. **`TryFormat` must return `false` (not throw) on span overflow.** Only precision > 999,999,999 and unknown standard specifiers throw.
12. **NFI property values are runtime data.** `PercentPositivePattern`, `PercentNegativePattern`, `CurrencyPositivePattern`, `CurrencyNegativePattern`, `NumberGroupSizes` etc. may differ by culture and by runtime version (ICU vs NLS).
13. **`NaNSymbol`, `PositiveInfinitySymbol`, `NegativeInfinitySymbol`** are copied verbatim, regardless of the format string.
14. **Zero must produce `"0"` (or `"0.000…"`)**, not `""`. The custom `"#.##"` on zero produces `"."` in some cultures — check your tests, the docs are silent here.
15. **`-0.0` must be preserved** through the sign check. The `Decoded` struct reports `IsNegative = true` for `−0`.

---

## 11. Test vectors

The following are worth pinning. First group is **[REQ]** (from project owner), second group is my suggested extensions **[WORK]**.

### Required

```csharp
// G<prec> at decExp <= 0
AssertAlways.Equal("0.013",
    Quadruple.Parse("0.0125").ToString("G2"));

// Long G output from a hex-float round trip
AssertAlways.Equal("0.284444444444444440743701029025138323",
    Quadruple.Parse("0X1.23456789abcdef0123456789p-2",
                    NumberStyles.Float | NumberStyles.HexFloat).ToString("G36"));
AssertAlways.Equal("0.2844444444444444407437010290251383",
    Quadruple.Parse("0X1.23456789abcdef0123456789p-2",
                    NumberStyles.Float | NumberStyles.HexFloat).ToString("G"));

// P on ICU
AssertAlways.Equal("28.44%",
    Quadruple.Parse("0.2844").ToString("P2",
        CultureInfo.GetCultureInfo("en-US")));
```

### Suggested (my own additions)

```csharp
// G/F/N at decExp <= 0
AssertAlways.Equal("0.5",    Quadruple.Parse("0.5").ToString("G3"));
AssertAlways.Equal("0.05",   Quadruple.Parse("0.05").ToString("G1"));
AssertAlways.Equal("0.1",    Quadruple.Parse("0.1").ToString("F1"));
AssertAlways.Equal("0.000123", Quadruple.Parse("0.000123").ToString("G3"));

// N/C at decExp <= 0
AssertAlways.Equal("0.28",   Quadruple.Parse("0.2844").ToString("N2"));
AssertAlways.Equal("$0.28",  Quadruple.Parse("0.2844")
    .ToString("C2", CultureInfo.GetCultureInfo("en-US")));

// Boundary between fixed and scientific
AssertAlways.Equal("1E-05",  Quadruple.Parse("1e-5").ToString("G"));
AssertAlways.Equal("0.0001", Quadruple.Parse("1e-4").ToString("G"));

// Custom "#" semantics
AssertAlways.Equal(".5",     Quadruple.Parse("0.5").ToString("#.##"));
AssertAlways.Equal(".5",     Quadruple.Parse("0.50").ToString("#.##"));
AssertAlways.Equal("",       Quadruple.Parse("0").ToString("#"));   // empty, not "0"
AssertAlways.Equal("0.00",   Quadruple.Parse("0").ToString("0.00"));

// Custom "#,#,," scaling
AssertAlways.Equal("1,235",  Quadruple.Parse("1234567890").ToString("#,##0,,"));

// Custom E
AssertAlways.Equal("8.6E+4", Quadruple.Parse("86000").ToString("0.###E+0"));
AssertAlways.Equal("8.6E+004", Quadruple.Parse("86000").ToString("0.###E+000"));

// Custom sections
AssertAlways.Equal("zero",   Quadruple.Parse("0").ToString("##.##;neg;zero"));
AssertAlways.Equal("1",      Quadruple.Parse("1").ToString("##.##;(##.##)"));
AssertAlways.Equal("(1)",    Quadruple.Parse("-1").ToString("##.##;(##.##)"));

// Precision cap
Assert.Throws<FormatException>(() => Quadruple.One.ToString("E9999999999"));

// Extreme values
AssertAlways.Equal("0",      Quadruple.Epsilon.ToString("F0"));
AssertAlways.Equal("42_70_00", Quadruple.Epsilon.ToString("42_70_00"));
AssertAlways.Equal("42_71_23", Quadruple.Parse("123.456").ToString("42_70_00"));
```

---

## 12. Reference: standard numeric format strings

From the .NET documentation (as provided by the project owner):

| Specifier | Name | Supported by Quadruple | Behaviour |
|---|---|---|---|
| `B` / `b` | Binary | **No** (integral only) | — |
| `C` / `c` | Currency | Yes | `NumberFormatInfo.Currency*` |
| `D` / `d` | Decimal | **No** (integral only) | — |
| `E` / `e` | Exponential | Yes | default precision 6, minimum 3 exponent digits |
| `F` / `f` | Fixed | Yes | default `NumberDecimalDigits` |
| `G` / `g` | General | Yes | default `34` (see §7.3) |
| `N` / `n` | Number | Yes | grouped, default `NumberDecimalDigits` |
| `P` / `p` | Percent | Yes | ×100, default `PercentDecimalDigits` |
| `R` / `r` | Round-trip | Yes | shortest round-trip |
| `X` / `x` | Hexadecimal | **No** (integral only) | — |

The default behaviour for any other single letter is `FormatException`.

---

## 13. Reference: custom numeric format specifiers

| Token | Meaning |
|---|---|
| `0` | Zero placeholder; always emits a digit. |
| `#` | Digit placeholder; suppressed if non-significant. |
| `.` | Decimal separator (`NumberDecimalSeparator`). |
| `,` | Between placeholders: group separator. Before the point: scale by 1000 per comma. |
| `%` | Multiply by 100, insert `PercentSymbol`. |
| `‰` | Multiply by 1000, insert `PerMilleSymbol`. |
| `E0`/`E+0`/`E-0`/`e0`/`e+0`/`e-0` | Scientific; exponent digit count from zeros; sign character from `+`/`-`. |
| `\` | Escape next character. |
| `'…'` / `"…"` | Literal string. |
| `;` | Section separator (1 = all, 2 = positive+zero; negative, 3 = pos;neg;zero). |
| other | Literal. |

---

## 14. Constants quick reference

| Constant | Value | Where defined | Source |
|---|---|---|---|
| `MaxSignificantComputed` | `11563 + 4` | `DecimalDigits` | [REQ] user-derived, [+4 WORK] |
| `MaxFractionalComputed` | `16494 + 4` | `DecimalDigits` | [REQ] user-derived, [+4 WORK] |
| `MaxN` (round-trip) | `37` | `RoundTripDigits` | [WORK] `⌈113·log10 2⌉ + 1` |
| `threshold` (default G) | `34` | `General` | [WORK] `floor(113·log10 2)` |
| `EXP_BIAS` | `16383` | `Quadruple` | IEEE 754 binary128 |
| `MantissaBits` | `112` | `QuadrupleNumber` | IEEE 754 binary128 |
| `SignificandBits` | `113` | `QuadrupleNumber` | IEEE 754 binary128 |
| `NaNPayloadBits` | `111` | `QuadrupleNumber` | 112 − 1 quiet bit |
| `MaxDecMSB` | `4934` | `QuadrupleNumber` | 4933 + 1 headroom |
| `MinDecMSB` | `−4970` | `QuadrupleNumber` | −4966 − 4 headroom |

---

## 15. Pointers to the code

- **Bit layout:** `Quadruple._Hi64Bits` = `[sign:1][exp:15][mantissa-hi:48]`; `_Lo64Bits` = mantissa-lo (64).
- **`ExtractRawBiasedExponentAndRawSignificand`:** returns the biased exponent and the trailing significand as two 64‑bit halves; used by the public `TryWriteExponent*`, `TryWriteSignificand*`, and by the `Decoded` struct indirectly.
- **`Decoded.Decode(lo, hi)`:** the canonical entry for formatters; produces `(isNegative, isNaN, isPosInf, isNegInf, isZero, Significand, Exp2)`.
- **`BitConverter.QuadrupleToUInt128Bits` / `UInt128BitsToQuadruple`:** the raw-bits bridge used by `IBitwiseOperators` and `BitIncrement`/`BitDecrement`.

---

## 16. Open questions for the next agent

1. Does `threshold = 34` for default G match the project owner's intent, or should it be `digits.Length` (old behaviour)?
2. For `E<prec>` where `prec + 1 > MaxSignificantComputed`, refuse (`return false`) or pad with zeros?
3. For custom `#.##` on `0`, is `"."` or `""` the expected output? (Docs are silent.)
4. Should the `+4` headroom on the two `Max*Computed` constants be kept, or exact bounds used?
5. Are the checked‑context items 3–7 in §8 fully fixed in the current source? Verify.
6. Is `TryParsePrecision` still referenced anywhere? If not, delete.
7. Are the `CurrPos` / `CurrNeg` / `PctPos` / `PctNeg` tables still the authoritative .NET pattern strings for the target runtime (ICU)?

---

## 17. Closing note

Everything in §2, §7.1, §7.3 (threshold), §7.6 (percent patterns), §7.8 (custom rules), §8, and §9.1 (B5, B6) reflects a requirement or a bug the **project owner stated or confirmed**. Everything labelled **[WORK]** — in particular the choice of `34` for the default G threshold, the `+4` margins on the computed-digit caps, and the specific wording of the test extensions in §11 — is my own proposal and should be reviewed before being treated as authoritative.

The core invariant that must be preserved by any future change: **`RoundToScale` is the only function that rounds, and it rounds exactly using `BigInteger` arithmetic on the exact rational `sig·2^exp2·10^k`.** Any change that introduces a floating-point rounding decision, or that rounds in more than one place, violates the requirement in §2.1.

— DPSK