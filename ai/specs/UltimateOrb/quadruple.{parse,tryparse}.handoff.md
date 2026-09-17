# Quadruple Parse/TryParse Handoff — DPSK

**Agent ID:** DPSK
**Scope of this handoff:** `Parse` / `TryParse` / `TryParsePartial` and their supporting routines inside `QuadrupleNumber` (the parser). Formatting (`TryFormat` and friends) is covered in a sibling handoff.

Throughout this document, **`[REQ]`** marks something you have explicitly required; **`[NOTE]`** marks a design decision or observation I (DPSK) introduced, not a stated requirement; **`[VERIFY]`** marks something that needs a second pair of eyes.

---

## 0. TL;DR

The parser is a from-scratch implementation of `Number.TryParseFloat` for `Quadruple` (IEEE 754 binary128) that never calls into `System.Number`. It is generic over `TChar : unmanaged` and dispatches through the project's own `UtfChar<TChar>` helper (not `IUtfChar<TChar>` — that interface is unavailable in this codebase). It supports:

* Decimal format with full `NumberStyles` coverage.
* Hex-float format (`0x1.8p3`).
* NaN payload syntax `NaN(Q123)` / `NaN(S123)` / `NaN(123)`. Payload is folded to 110 data bits; bit 110 is a payload-overflow indicator; bit 111 is the quiet bit. Oversize payloads jam (never fail); `NaN(S0)` forces the overflow bit on so it does not alias `±Infinity`.

All rounding is exact (`BigInteger` cross-multiplication, half-to-even at a single site). Two known issues remain to be fixed (see §10); the rest of the code has been exercised through review.

---

## 1. Requirements (specified by project owner)

### 1.1 Functional
* `[REQ]` Implement our own `Parse` / `TryParse` / `TryParsePartial`.
* `[REQ]` Do **not** call `System.Number` (`TryStringToNumber`, `NumberToFloat`, `TryParseHexFloatingPoint`, `TryMatchSpecialValueSymbol`, `SpanTrimStart`, `SpanEqualsOrdinalIgnoreCase`, `ThrowFormatException`, etc.) unless we also provide our own implementation.
* `[REQ]` Support a `TChar`-generic surface; UTF-8, UTF-16, and UTF-32 (`Rune`) code units must work. Later we may add `uint` / `Rune` overloads — keep the parser generic.
* `[REQ]` Correctly rounded results (round-to-nearest, ties-to-even), no exceptions.
* `[REQ]` Handle every required `NumberStyles` flag correctly.
* `[REQ]` Parse NaN payloads: `"-NaN(Q4353)"`, `"-NaN(S4353)"`, `"-NaN(4353)"`, etc.
    * `[REQ]` Optional `Q` / `S` selects quiet (default) or signaling.
    * `[REQ]` Payload is an unsigned integer folded modulo 2^110 into fraction bits 109..0. Any bit at position ≥ 110 sets a payload-overflow indicator at fraction bit 110. Fraction bit 111 is the quiet bit.
    * `[REQ]` **No width-reject path.** Oversize input jams; it does not fail. The only NaN-grammar rejects are malformed tokens (missing `)`, no digits, non-digit garbage where a digit or `)` was expected).
    * `[REQ]` `NaN(S0)` forces the payload-overflow indicator on. Without this, quiet=0/data=0/overflow=0 would encode the exact bit pattern of `+Infinity`; forcing bit 110 keeps the sNaN class distinguishable.
    * `[NOTE]` This matches the numeric conversion convention in `FromIeee754InterchangeBinary` / `ToIeee754InterchangeBinaryNarrowing`, which reserves destination fraction bit 110 as the narrowing-overflow indicator. `Quadruple.MaxNaNPayloadAsBigInteger = (1 << 110) − 1` is the exact data-mask constant.
* `[REQ]` Precision of a standard-format specifier above 999,999,999 must throw `FormatException` (this is a formatter concern, listed here because parser and formatter share the parser-side NaN/payload grammar conventions).
* `[REQ]` Codebase target: .NET 11+, C# 14+.
* `[REQ]` Omit the public Parse/TryParse overloads that already live in `Quadruple` — those are in place; the parser only supplies `QuadrupleNumber.TryParseFloat` / `ParseFloat`.

### 1.2 Non-functional
* `[REQ]` No `IUtf8Char<TChar>`; use the project's `UtfChar<TChar>`.
* `[REQ]` No `Number.*` helper calls at runtime (see §1.1).
* `[REQ]` `[VERIFY]` Project is built with `<CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>`; wrap integer negations whose operands could be `int.MinValue` in `unchecked`.

---

## 2. Architecture

```
Quadruple (partial struct)
  ├─ public Parse/TryParse/TryParsePartial overloads        (in-place; unchanged)
  └─ QuadrupleNumber  (internal static class, nested)
        ├─ ParseFloat<TChar>          → throw-on-fail wrapper
        ├─ TryParseFloat<TChar>       → entry point
        │     ├─ TryParseHexFloat<TChar>              (AllowHexSpecifier)
        │     ├─ TryParseDecimalNumber<TChar>         (default path)
        │     └─ TryParseSpecialValue<TChar>          (fallback)
        │           ├─ TryParseNaN<TChar>             (payload grammar)
        │           └─ helpers: MatchSymbol, MatchSymbolIgnoreCaseAt, TailOk
        ├─ BuildQuadruple(isNeg, sig, exp10)          (decimal → bits)
        ├─ BuildQuadrupleFromBinary(isNeg, sig, exp2) (hex → bits)
        │     └─ BuildFromRatio(isNeg, N, D)          (common rounding)
        │           ├─ ComputeBinaryExponent(N, D)
        │           ├─ RoundHalfToEven(N, D, e)
        │           └─ Encode(isNeg, m, e, isSubnormal)
        └─ Low-level helpers (IsWhite, HexValue, TrimStartWhitespace,
                              TryConsumeSign, MatchNegativeSignLength,
                              MatchSymbol, MatchSymbolIgnoreCaseAt,
                              TryMatchSymbolIgnoreCase, TailOk,
                              ThrowFormatException)
```

`UtfChar<TChar>` lives in `UltimateOrb.Internal.System` and provides `CastToUInt32`, `CastFrom`, and the `IsSupported` / `IsUtf8` / `IsUtf16` / `IsUtf32` predicates. All parser entry points funnel comparisons through `UtfChar<TChar>.CastToUInt32(span[i])` and `MemoryMarshal.Cast<TChar, char|byte|Rune>` when the caller's `TChar` is known at JIT time.

---

## 3. Decimal parse path (`TryParseDecimalNumber<TChar>`)

Order of operations, in the current file:

1. Leading whitespace (`AllowLeadingWhite`).
2. `(` (`AllowParentheses`) → sets `isNegative = true`, marks `hadParens`.
3. Leading sign (`AllowLeadingSign`, otherwise skipped if `hadParens`), via `TryConsumeSign`.
4. Leading currency symbol (`AllowCurrencySymbol`).
5. Integer digits, with `AllowThousands` group-separator skip **only between two digit positions** (checks that the char after the separator is a digit).
6. Decimal separator (`AllowDecimalPoint`), then fractional digits, same grouping rule.
7. Exponent (`AllowExponent`), `e`/`E`, optional sign, decimal digits. Saturates at 10,000,000 (a saturating cap, not a hard failure — the value will clamp to ±∞ or ±0 by the MSB estimate in `BuildQuadruple`).
8. Trailing currency symbol (`AllowCurrencySymbol`).
9. Trailing sign (`AllowTrailingSign`) — `-` sets `isNegative = true`; `+` is a no-op.
10. Closing `)` if `hadParens`.
11. Trailing whitespace (`AllowTrailingWhite`).
12. Trailing `'\0'` padding — always consumed.
13. Trailing-remainder check: if any non-`'\0'` remains **and** `Number.AllowTrailingInvalidCharacters` is not set, fail.

Then: `result = BuildQuadruple(isNegative, sig, expAdjust − fracDigits)`.

**Important** `[NOTE]`:
* `sig` is built by appending decimal digits into a `BigInteger` up to `MaxInputDigits` (currently 200,000). Beyond that, additional digits are dropped — they would not affect correctly-rounded results for finite Quadruple, but this is a heuristic cap, not a proof of safety. `[VERIFY]`
* `expAdjust` is a `long` computed as `expValue - fracDigits`. Never overflows for reasonable inputs.
* `TryConsumeSign` tries `NumberFormatInfo.NegativeSign` / `PositiveSign` first, then falls back to ASCII `-` / `+`. That mirrors .NET's behaviour for locales with non-ASCII sign characters.

---

## 4. Hex-float parse path (`TryParseHexFloat<TChar>`)

Grammar: `[ws] [sign] 0x<hex>[.<hex>]p[sign]<dec>[ws] [\0*]`.

* `0x` / `0X` prefix is required.
* `p` / `P` **exponent is required** (this matches .NET `NumberStyles.HexFloat`; there is no default exponent).
* Radix point uses ASCII `.` directly (not `NumberDecimalSeparator`), matching .NET.
* Sign follows the locale's `NegativeSign` / `PositiveSign`, plus ASCII fallback.
* The binary exponent is `binExp − 4 · fracHexDigits`.
* Decimal fraction is closed by feeding `(sig, exp2)` into `BuildQuadrupleFromBinary`.

`BuildQuadrupleFromBinary` normalizes `sig·2^exp2` and then defers to `BuildFromRatio`:

```csharp
int B = (int)(sig.GetBitLength() - 1) + exp2;
if (B > MaxNormalBinExp)  return ±Inf;
if (B < MinSubnormalExp2 - 1) return ±0;
bool isSubnormal = B < MinNormalBinExp;
int  e           = isSubnormal ? MinSubnormalExp2 : B - MantissaBits;
BigInteger m = RoundHalfToEven(N, D, e, out bool carry);
```

---

## 5. Special-value parse (`TryParseSpecialValue<TChar>`)

Triggered **only** when the numeric parse fails, and unconditionally trims leading whitespace (independent of `AllowLeadingWhite` — this matches .NET historical behaviour).

Grammar:
```
[ws] [sign] ( InfinitySymbol | NegativeInfinitySymbol | NaN [ "(" [Q|S] digits ")" ] ) [ws] [\0*]
```

* `Infinity` / `NaN` are compared **case-insensitively** against the locale's symbols.
* `NumberStyles.AllowTrailingInvalidCharacters` stops parsing at the first non-whitespace character after the symbol; otherwise trailing non-whitespace rejects the match.
* `+,−` prefix is tried in addition to the locale sign.

### §5.1 — NaN payload grammar `[REQ]` (full replacement)

```
NaN                       → quiet, payload 0
NaN(Q4353)                → quiet, payload 4353
NaN(S4353)                → signaling, payload 4353
NaN(4353)                 → quiet, payload 4353
```

The NaN fraction field is partitioned as:

| bits     | width | meaning                                    |
|----------|-------|--------------------------------------------|
| 111      | 1     | quiet bit                                  |
| 110      | 1     | payload-overflow indicator                 |
| 109..0   | 110   | payload data                               |

Rules:
* Sign is applied to the top bit of the high word.
* The parsed unsigned integer is folded modulo 2^110 into the data bits; any bit at position ≥ 110 also sets the payload-overflow indicator. The digit scan is bounded only by input length — the accumulator is masked back to 110 bits on every step and cannot grow past ~114 bits regardless of how many digits the caller supplies.
* The quiet bit is set iff `Q` was given, or no `Q` / `S` marker was present.
* The payload-overflow indicator is set iff either (a) the parsed integer had any bit at position ≥ 110, or (b) the fraction would otherwise be all-zero after the quiet-bit decision, i.e. `NaN(S0)`. The latter guards against aliasing `±Infinity`.
* There is **no** width-reject: `NaN(Q<2^200>)` succeeds with the low 110 bits of the value jammed into the data field and bit 110 set.

`[NOTE]` The constants are:

```csharp
private const int NaNPayloadDataBits    = 110;   // fraction bits 109..0
private const int NaNPayloadOverflowBit = 110;   // fraction bit 110
private const int NaNQuietBit           = 111;   // fraction bit 111
```

`[NOTE]` This supersedes DPSK's earlier 111-bit bound and the "`NaN(S0)` is rejected" decision. Both are replaced by the jam-and-flag rule above. Reason: the numeric conversion routines already use bit 110 as the narrowing-overflow indicator, so a parsed `NaN(Q<2^110>)` that put bit 110 into data would be misread downstream as "the payload was narrowed" rather than "the caller wrote this bit explicitly".

---

### §5.2 — Bit layout used at construction (full replacement)

`payload` is a `BigInteger` already reduced to `[0, 2^110)` by the digit scanner; `overflow` is a `bool` set during the scan on the first digit that pushes the accumulator across the 2^110 threshold.

```csharp
BigInteger mantissa = payload & ((BigInteger.One << 110) - 1);   // data: bits 109..0
if (quiet)
    mantissa |= BigInteger.One << 111;                            // quiet: bit 111
if (mantissa.IsZero || overflow)
    mantissa |= BigInteger.One << 110;                            // overflow: bit 110

lo      = (ulong)(mantissa & ulong.MaxValue);
hi_mant = (ulong)((mantissa >> 64) & 0x0000_FFFF_FFFF_FFFF);
hi      = signBit | (InfinityExponent << 48) | hi_mant;
```

`InfinityExponent = 0x7FFF`. The `IsZero` guard is what disambiguates `NaN(S0)` from `±Infinity`: with quiet = 0 and data = 0, leaving overflow at 0 would encode the exact bit pattern of an infinity. This matches the encoding used by `Quadruple.NaN`, `Quadruple.PositiveQuietNaN`, etc., with the extra invariant "fraction ≠ 0 for any NaN".

Example: `-NaN(S0)` → fraction = `1 << 110` = `0x4000_0000_0000_0000_0000`, high word = `0xFFFF4000_00000000`. `+NaN(Q0)` → fraction = `1 << 111` = `0x8000_0000_0000_0000_0000`, high word = `0x7FFF8000_00000000`.

---

## 6. Correct-rounding invariant

**Every** rounding decision in the parser happens inside `BuildFromRatio`:

```csharp
int B = ComputeBinaryExponent(N, D);              // exact floor(log2(N/D))
int e = isSubnormal ? MinSubnormalExp2 : B - MantissaBits;
BigInteger m = RoundHalfToEven(N, D, e, out bool carry);
```

* `ComputeBinaryExponent` compares `N` and `D` bit-lengths, then cross-multiplies via a single left-shift. It is exact and cannot be off by more than 0.
* `RoundHalfToEven` computes the exact quotient and remainder of `N · 2^(−e) / D` and rounds on `2·rem ? den` with a tie-to-even rule. This is the *only* rounding site.
* `carry` signals that the significand renormalized to `2^113`; the caller increments `e` and retries the encode.

**Do not add any other rounding site.** Every bug in earlier revisions came from rounding in two places (parser vs. emission) or using `double` as an intermediary.

`ComputeBinaryExponent` uses this exact trick — worth keeping as a reference:

```csharp
long diff = (long)N.GetBitLength() - (long)D.GetBitLength();
bool geq = diff >= 0 ? N >= (D << (int)diff) : (N << (int)(-diff)) >= D;
return geq ? (int)diff : (int)diff - 1;
```

---

## 7. `NumberStyles` support matrix

| Flag | Handled where | Notes |
|---|---|---|
| `AllowLeadingWhite` | decimal, hex | whitespace set is broader than .NET's `IsWhite` — see §10 |
| `AllowTrailingWhite` | decimal, hex | " |
| `AllowLeadingSign` | decimal, hex | locale `PositiveSign` / `NegativeSign`, ASCII fallback |
| `AllowTrailingSign` | decimal | `TryConsumeSign`; `-` sets sign, `+` ignored |
| `AllowParentheses` | decimal | sets `isNegative`, requires closing `)` |
| `AllowDecimalPoint` | decimal, hex | decimal uses `NumberDecimalSeparator`; hex uses ASCII `.` |
| `AllowThousands` | decimal | only between two digit runs |
| `AllowExponent` | decimal, hex | decimal uses `e` / `E`; hex uses `p` / `P` |
| `AllowCurrencySymbol` | decimal | leading and trailing; `MatchSymbol` against `CurrencySymbol` |
| `AllowHexSpecifier` | hex | requires `AllowExponent` (validated by `ValidateParseStyleFloatingPoint`) |
| `AllowBinarySpecifier` | never for `Quadruple` | `ValidateParseStyleFloatingPoint` throws |
| `AllowTrailingInvalidCharacters` (internal, `0x80000000`) | all | checked only at the "trailing content" test |

`NumberFormatInfo.ValidateParseStyleFloatingPoint` is called by every public entry point with the **original** style; the internal parse adds `AllowTrailingInvalidCharacters` itself. Do **not** validate the augmented style.

---

## 8. Common pitfalls

These are the concrete failure modes encountered (or nearly encountered) during design; each is worth a targeted test.

1. **Two rounding sites.** Any format/parse routine that rounds twice (once for `N`, once at emit) can produce an off-by-1 ULP result. All rounding goes through `RoundHalfToEven`.
2. **Using `double` as an intermediary.** Even one `Math.Floor(log10(...))` for a *decision* loses correctness at the ULP. Use `ILog2` + exact `CompareSigExp2To10Pow` (formatter) or `ComputeBinaryExponent` (parser).
3. **`NaN(S0)` would alias `Infinity`.** Mantissa 0 + exponent 0x7FFF is Infinity, not sNaN. Do **not** reject the input; force the payload-overflow indicator (fraction bit 110) so the fraction is nonzero and the class is unambiguous.
4. **Oversize NaN payload.** `NaN(Q<2^110>)` and any wider input is **jammed**: the data field holds the low 110 bits of the parsed integer, bit 110 is set. Do **not** fail on width. The only rejects in the NaN grammar are malformed tokens.
5. **Leading `+`/`-` on special values.** `+Infinity`, `-NaN(Q1)` are valid. The `+` prefix in particular is easy to forget; ensure the positive-sign branch is symmetric with the negative branch.
6. **`NumberStyles.AllowTrailingInvalidCharacters` must be stripped before `ValidateParseStyleFloatingPoint`.** Otherwise it looks like an invalid flag.
7. **Hex float exponent is required.** `0x10` alone is not a valid hex float. `0x10p0` is.
8. **Hex float uses ASCII `.`** (not `NumberDecimalSeparator`) and `p` (not `e`).
9. **Checked context.** `<CheckForOverflowUnderflow>true</CheckForOverflowUnderflow>` is on. Any `-e`, `-k`, `-expVal` where the operand may be `int.MinValue` must be `unchecked`. Also `(int)Math.Floor(double)` should be `unchecked((int)Math.Floor(...))` if the product's range isn't provable.
10. **`%` and `‰` in custom formats compound multiplicatively.** `%%` = ×10000, not ×200. The current implementation in `QuadrupleFormatter.CustomFormat` uses the additive form `percentMul * 100 + perMilleMul * 1000` and is wrong for multiple markers. `[VERIFY]` — see §10.
11. **Percent / per-mille must not go through `Quadruple` arithmetic.** Multiplying `Quadruple.MaxValue` by 100 overflows. Always scale the `BigInteger` significand and leave `Exp2` alone.
12. **`NaN` payload is compared case-insensitively.** `nan(q1)` is valid.
13. **`TailOk` in `TryParseSpecialValue` is buggy** — see §10.
14. **Whitespace set used by `IsWhite` is wider than .NET's.** `0x1680`, `0x2000..0x200A`, `0x2028`, `0x2029`, `0x202F`, `0x205F`, `0x3000` are accepted here but not by `Number`. If cross-parser consistency matters, narrow the list.
15. **`MatchSymbolIgnoreCaseAt` only lowercases ASCII.** Symbols containing non-ASCII letters won't match case-insensitively against their lowercase variants. `[VERIFY]` acceptable.
16. **`MatchSymbol` stack buffer is `stackalloc byte[64]`** for UTF-8 symbols. Currency symbols are short today, but there is no guard against a long `CurrencySymbol`. Add a fallback or `[VERIFY]` the invariant.
17. **`BuildQuadruple` fast-reject bounds are heuristic.** `MaxDecMSB = 4934` / `MinDecMSB = -4970` are chosen one past `MaxDecimalExponent` / `MinDecimalExponent`. Any change to those must be mirrored here.
18. **`MaxInputDigits = 200_000`** is arbitrary; it caps the `BigInteger` cost but is not correctness-proven. `[VERIFY]` whether any of the target tests exercise a longer input than ~50k digits.

---

## 9. Testing checklist (worth running on the successor's side)

### 9.1 Round-trip and near-tie
- [ ] Every boundary: `±0`, `±Epsilon`, `±MinValue`, `±MaxValue`, `±Infinity`, all four NaN constants.
- [ ] Round-trip: `Parse("R" of every value) == value` (formatter is in the sibling handoff; the parser side is what matters here).
- [ ] `1.00000000000000000000000000000000001` and its mid-point neighbours. The result must be the even significand.
- [ ] `0.99999999999999999999999999999999999…` (36 nines) and (37 nines).
- [ ] `9.9999999999999999999999999999999999e4932` (just below MaxValue, rounds up to ∞).
- [ ] `1e-4966` subnormal boundary; `5e-4967` (rounds to 0); `2.47e-4966` (tie case).

### §9.2 — NaN payload (full replacement of the checklist)
- [ ] `NaN`, `nan`, `-NaN`, `+NaN`.
- [ ] `NaN(0)` → quiet = 1, overflow = 0, data = 0.  Matches `Quadruple.PositiveQuietNaN`.
- [ ] `NaN(Q0)` → identical to `NaN(0)`.
- [ ] `NaN(S0)` → quiet = 0, overflow = 1, data = 0.  **Succeeds**, fraction = `1 << 110`, class is signaling, is *not* `+Infinity`.  Raw high word `0x7FFF4000_00000000`.
- [ ] `-NaN(S0)` → same fraction, sign bit set; high word `0xFFFF4000_00000000`.
- [ ] `NaN(Q1)` → quiet = 1, overflow = 0, data = 1.
- [ ] `NaN(S1)` → quiet = 0, overflow = 0, data = 1.
- [ ] `NaN(Q<2^110 − 1>)` → quiet = 1, overflow = 0, data all 1s.  Payload preserved exactly.
- [ ] `NaN(Q<2^110>)` → quiet = 1, overflow = 1, data = 0.  **Succeeds** (jam).
- [ ] `NaN(Q<2^111 − 1>)` → quiet = 1, overflow = 1, data all 1s.
- [ ] `NaN(Q<2^111>)` → quiet = 1, overflow = 1, data = 0.
- [ ] `NaN(Q<10^100>)` → quiet = 1, overflow = 1, data = low 110 bits of `10^100`.
- [ ] `NaN(Q<200k digits>)` → quiet = 1, overflow = 1, data = low 110 bits; completes without OOM or throw.
- [ ] `NaN(1)`, `NaN(Q1)` — same result.
- [ ] `NaN(S1)`, `-NaN(S1)` — fraction identical, sign bit differs.
- [ ] `nan(q1)`, `NAN(s1)`, `Nan(Q1)` — case-insensitive across all three markers and the digits.
- [ ] Malformed rejects (must **fail**, no jam): `NaN(`, `NaN()`, `NaN(Q)`, `NaN(Qx)`, `NaN(Q1`, `NaN(Q1)x` (without `AllowTrailingInvalidCharacters`).

### 9.3 Special values
- [ ] `Infinity`, `infinity`, `-Infinity`, `+Infinity`.
- [ ] `Infinity ` (trailing space), ` Infinity` (leading).
- [ ] `Infinityx` — fail unless `AllowTrailingInvalidCharacters`.
- [ ] `Infinityx` with `AllowTrailingInvalidCharacters` — succeed, `elementsConsumed == 8`.

### 9.4 Hex float
- [ ] `0x1.8p0` = 1.5; `0x1.8p1` = 3.0; `0x1.8p-1` = 0.75.
- [ ] `0x0p0`, `0x0.0p0`, `0x.0p0`.
- [ ] `0x10` (no exponent) — must fail.
- [ ] `0x1p100000` — saturate to ±∞.
- [ ] `0x1p-100000` — underflow to ±0.
- [ ] Sign: `-0x1p0`, `+0x1p0`.

### 9.5 NumberStyles
- [ ] `AllowParentheses` in combination with each sign style.
- [ ] `AllowThousands` for `en-US` and `de-DE` (thousands separator `.`).
- [ ] `AllowCurrencySymbol` for `en-US`, `fr-FR`, `ja-JP`.
- [ ] `AllowExponent` with a signed and unsigned exponent, saturating.
- [ ] Trailing/leading sign after currency symbol and vice versa.
- [ ] `ValidateParseStyleFloatingPoint` rejects `AllowBinarySpecifier` and `AllowHexSpecifier` without `AllowExponent`.

### 9.6 TChar genericity
- [ ] Same test suite over `char`, `byte` (UTF-8), `Rune`.
- [ ] Locale with non-ASCII currency symbol over `byte` (verify 64-byte `stackalloc` guard).

### 9.7 Fuzz
- [ ] Random bytes of length ≤ 64 → `TryParse` must return without throwing (except for `FormatException` from `ParseFloat`).
- [ ] Random strings of digits of length 1e5 → no OOM, no exceptions, correct `elementsConsumed`.

---

## 10. Known open issues to fix

### 10.1 `TailOk` consumes the wrong count  `[VERIFY]` → fix required

Current:
```csharp
private static bool TailOk<TChar>(ReadOnlySpan<TChar> input, int fromIndex,
                                  bool allowTrailingInvalid, ref int consumed) {
    ReadOnlySpan<TChar> tail = TrimStartWhitespace(input.Slice(fromIndex));
    if (!tail.IsEmpty && !allowTrailingInvalid) return false;
    consumed = consumed - (input.Length - fromIndex) + (input.Length - fromIndex - tail.Length);
    consumed += input.Length - fromIndex - tail.Length;
    return true;
}
```

Let `A = input.Length − fromIndex`, `W = input.Length − fromIndex − tail.Length` (whitespace count after the symbol), so `tail.Length = A − W`. Then:

```
consumed_new = consumed − (A − W) + W + W = consumed + 2W − A − (A − 2W)
```

Working through: `consumed_old` is already `symbol.Length`. Desired final is `symbol.Length + W`. Actual final is `symbol.Length + 2W − A = symbol.Length + W − (A − W) = symbol.Length + W − (non_whitespace_tail_count)`.

**Definite off-by-N.** Fix:
```csharp
int ws = input.Length - fromIndex - tail.Length;
consumed += ws;
return true;
```

### 10.2 Percent / per-mille multiplier is additive, not multiplicative  `[VERIFY]`

`QuadrupleFormatter.CustomFormat`:
```csharp
int multiplier = percentMul * 100 + perMilleMul * 1000;
```
`"%%"` should be ×10000, not ×200. Replace with:
```csharp
BigInteger multiplier =
    BigInteger.Pow(100, percentMul) * BigInteger.Pow(1000, perMilleMul);
// guard against pathological input
if (percentMul + perMilleMul > 100) throw new FormatException(...);
```

### 10.3 `IsWhite` is broader than .NET's

Current list includes `0x1680`, `0x2000..0x200A`, `0x2028`, `0x2029`, `0x202F`, `0x205F`, `0x3000`. .NET's parsing `IsWhite` is only `0x20`, `0x09..0x0D`, `0x85`, `0xA0`. Decide:
* Match .NET exactly (recommended if any test asserts parity), or
* Keep the broader set and document the divergence.

### 10.4 Possible additional `TailOk`-adjacent issues  `[VERIFY]`

* `TryParseSpecialValue` for `+Infinity` uses a hand-rolled `+` check instead of `TryConsumeSign`. If the locale's `PositiveSign` is not `+`, this misses it.
* `MatchSymbolIgnoreCaseAt` in `TryParseNaN` matches the *case-insensitive* form but does not trim trailing whitespace itself; it relies on `TailOk` / the trailing check. Verify this against `"-NaN(Q1) x"` with `AllowTrailingInvalidCharacters`.

### 10.5 `MaxInputDigits` — sanity  `[VERIFY]`

`MaxInputDigits = 200_000` keeps the accumulator bounded but does not have a correctness proof. Reasonable lower bound: `MaxRoundTripDigits (36) + |MaxDecExp| (4934) + |MinDecExponent| (4966) ≈ 10_000`. Consider dropping to `20_000` for memory safety; measured input lengths from the test suite will determine whether this is safe.

### 10.6 NaN payload sign in the `+`-prefixed case  `[VERIFY]`

`TryParseSpecialValue` for the `+` prefix calls `TryParseNaN(after, …, isNegative: false, …)`. That's correct. But make sure the leading `+` path also handles `+Infinity` — the current code does, via `TryMatchSymbolIgnoreCase`. Good.

### 10.7 `TryConsumeSign` ordering  `[NOTE]`

`TryConsumeSign` prefers the locale symbols over ASCII `+`/`-`. If the locale's `PositiveSign` happens to start with `-` (unlikely but legal), the ASCII fallback is still reachable. No action required, but worth a targeted test with a synthetic `NumberFormatInfo`.

### 10.8 Payload sign for `-NaN(...)`  `[VERIFY]`

`-NaN(S1)` should set the sign bit and the payload mantissa, and mark it signaling. Verify against the bit-level tests in §9.2.

---

## 11. Files touched by this workstream

* `Quadruple.cs` — `Parse` / `TryParse` / `TryParsePartial` entry points; `QuadrupleNumber` nested class; `TryExtractComponents`.
* `UtfChar.cs` — `UtfChar<TChar>` helper.
* `BinaryFloatParseAndFormatInfo.cs` — constants used by both formatter and parser.
* `Number.cs` — `NumberStyles.AllowBinarySpecifier`, `AllowTrailingInvalidCharacters`.
* `NumberFormatInfoExtensions.cs` — `ValidateParseStyleFloatingPoint`.
* `QuadrupleFormatter.cs` (nested in `Quadruple`) — not the parser's concern, but `RoundToScale`, `GetSignificant`, `GetFixed` are shared and must stay consistent with the parser's rounding invariants.

---

## 12. Next steps for the successor agent

1. **Fix §10.1 (`TailOk`).** This is a hard correctness bug that will surface on any test with trailing whitespace after a special value.
2. **Fix §10.2 (multiplicative `%` / `‰`).** Low priority if the test suite never exercises `"%%"`, but a spec-conformance issue.
3. **Run the §9 test matrix.** Pay special attention to §9.1 round-trip and §9.2 NaN payload — the jam semantics in particular. Cross-check every parsed NaN against `ToIeee754InterchangeBinaryNarrowing` round-trips: a parsed `NaN(Q<2^110>)` must narrow to a target NaN whose overflow bit is set for any narrower target format, and a parsed `NaN(Q<2^110 − 1>)` must narrow to an exact payload. These two are the regression sentinels for the old 111-bit misalignment.
4. **`[VERIFY]` the checked-context negation paths.** Search the parser for `-e`, `-k`, `-expVal`, `-fracHexDigits` and wrap in `unchecked` where the operand is not provably positive. Grep pattern: `negate *int` from IL or `-.*[a-zA-Z]` in source.
5. **`[VERIFY]` the `AllowHexSpecifier` without `AllowExponent` path.** `ValidateParseStyleFloatingPoint` should reject it, but `TryParseFloat` also needs to not silently route to the decimal path in that case.
6. **Add a fuzz harness** covering all three `TChar` types. The parser must never throw except through `ParseFloat`.
7. **Wire the parser into the project's CI tests** before touching the formatter again — formatter changes are riskier if the parser side is not yet exercised end-to-end.

---

## 13. Conventions to carry forward

* **Never** round outside `RoundHalfToEven`. Everything else is comparison and shifting.
* **Never** use `double` for a decision. `double` is fine for a *hint*; every decision must be backed by an exact integer comparison.
* **Never** call `System.Number.*`. If you need a helper (`TryStringToNumber` equivalent, `TryMatchSpecialValueSymbol`, `SpanTrimStart`), write it in the parser.
* **Never** reject a NaN payload on width. Fold to 110 bits, set bit 110 on overflow, and let `NaN(S0)` force bit 110 so the class never aliases `±Infinity`. The only NaN failures are malformed tokens.
* **Wrap sign flips in `unchecked`** when the operand could be `int.MinValue`.
* **Test one `TChar` at a time.** Generic code that compiles for `Rune` can silently mis-dispatch for `byte`.
* **Keep the parser and formatter rounding in sync.** `RoundToScale` (formatter) and `RoundHalfToEven` (parser) are structurally identical on purpose — if one changes, the other must change with it.

---

*End of handoff — DPSK.*