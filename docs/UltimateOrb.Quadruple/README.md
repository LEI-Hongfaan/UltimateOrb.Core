# UltimateOrb.Quadruple — IEEE 754-2019 `binary128` for .NET

**UltimateOrb.Quadruple** is a managed, portable, and correctly-rounded implementation of the IEEE 754-2019 `binary128` interchange format for .NET 7 and later. It is not a wrapper around a native library, nor a double-double emulation: it is a blittable `readonly struct` with the exact 128-bit layout mandated by the standard (1 sign bit, 15-bit biased exponent with bias 16383, 112-bit trailing significand), implementing `IBinaryFloatingPointIeee754<Quadruple>` and participating fully in the modern generic-math and `System.Numerics` ecosystem.

The library targets workloads where `double` is not enough — numerical integration, arbitrary-precision *intermediate* arithmetic, porting of quadruple-precision reference data, and compiler/runtime experiments — and where a **correctly rounded**, **reproducible**, and **exhaustively testable** result is required rather than a "close enough" approximation.

> **Experimental.** `Quadruple` is marked `[Experimental("UoWIP")]`, so referencing it from a consuming project produces a `UoWIP` diagnostic that **blocks compilation** until you opt in. See [Experimental status](#experimental-status) for how to acknowledge it.

---

## Highlights

- **True IEEE 754-2019 `binary128`** — exact interchange encoding, so bit patterns are portable to/from `__float128` (GCC), `libquadmath`, POWER9 hardware, and any other conforming implementation.
- **Correctly rounded across the whole basic-arithmetic surface, with all five IEEE 754 rounding modes.** `+`, `-`, `*`, `/`, `%`, `FusedMultiplyAdd`, `IEEERemainder`, `ScaleB`, `Round(x, 0, mode)` — and their named equivalents (`Add`, `Subtract`, `Multiply`, `Divide`, `Remainder`) — accept an explicit rounding mode.
- **Correctly rounded transcendentals** — `Sin`, `Cos`, `SinCos`, `Tan`, `Asin`, `Acos`, `Atan`, `Atan2`, `Exp`, `ExpM1`, `Log`, `Log2`, `Log10`, `Sqrt`, `Cbrt`, `ReciprocalSqrt`, `Hypot` — each with the same five-mode support. See the full status catalog below for what is not yet correctly rounded.
- **Beyond the minimum: `roundTiesToAway`.** IEEE 754-2019 §4.3.3.2 recommends but does not require `roundTiesToAway`. The library exposes it under `MidpointRounding.AwayFromZero` on every correctly-rounded function; the BCL does not surface this mode on `Sin`/`Cos`/`Log`/etc.
- **Augmented arithmetic** — IEEE 754-2019 §9.5 `augmentedAddition`, `augmentedSubtraction`, `augmentedMultiplication`, returning `(Rounded, Delta)` tuples with the standard NaN propagation, the Sterbenz zero-delta sign rule, the subnormal-underflow edge cases, and the required midpoint-toward-zero rounding for the rounded part.
- **The most sophisticated parsing and formatting of any .NET `binary128` library** — exact round-half-to-even at every formatting precision, correct and minimal shortest round-trip, full standard and custom format-specifier language, complete `NumberFormatInfo`/`NumberStyles` respect, hex-float parsing, NaN-payload parsing beyond what `System.Double` offers, UTF-8 and UTF-16 paths, and public `TryParsePartial`. See the dedicated section below.
- **Full NaN-payload control** — three configurable modes (`NAN_PERMISSIVE`, `NAN_PAYLOAD_LEGACY`, and the modern LSB-aligned convention), plus `GetQuietNaNWithPayload`/`GetSignalingNaNWithPayload`/`GetPayload` for explicit payload manipulation.
- **Cross-format interchange** — `FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>` and `ToIeee754InterchangeBinaryNarrowing` handle the entire IEEE 754 binary interchange family: binary16, binary32, binary64, binary128, and the `k ≥ 128, k % 32 == 0` formats (binary160/192/224/256), with correct rounding, mode support, and NaN payload policy alignment in both directions.
- **Conversions to and from the newer .NET numeric types.** When targeting the framework versions that provide them, `Quadruple` also converts to and from `BFloat16` and `System.Numerics.Decimal32` / `Decimal64` / `Decimal128`, with correct rounding. See the conversions section below.
- **Performance on par with native `binary128` libraries.** Despite the abstract encapsulation and the full rounding-mode support, the implementation is competitive with mainstream C, C++, and Rust `binary128` libraries.
- **Native AOT compatible; math functions work in zero-GC environments.** The numeric core — every arithmetic operation and every transcendental function — performs **no GC heap allocations**. The library is **Native AOT compatible**, and the math functions run correctly in **zero-GC environments**, including **UEFI applications**. See the dedicated section below.
- **GPU compatibility via ILGPU** — the basic operations (`+`, `-`, `*`, `/`, `%`, FMA, comparisons, `Abs`/`CopySign`/`BitIncrement`/`BitDecrement`) are expressed entirely in terms the ILGPU compiler already handles today, so they can be JIT-compiled into CUDA, OpenCL, and ILGPU-CPU kernels without any change to the calling code. The transcendental and other higher-level functions will also become GPU-compilable automatically once ILGPU 2.0 finalizes.
- **Validated against four reference implementations** — MPFR (accessible from .NET either directly through P/Invoke or through the `Sdcb.Arithmetic.Mpfr` binding), the combined `BigRational`+interval-arithmetic methodology, JIT-compiled LLVM `fp128` stubs, and CORE-MATH (its own correct-rounding C/C++ `binary128` implementations, plus its adversarial input generators). Each covers a different area; see the test methodology and matrix below.

---

## Performance and runtime footprint

- **No GC heap allocations in the mathematical functions.** Every arithmetic operation and every transcendental function returns a `Quadruple` by value; none allocate. Intermediate work uses `UInt128`/`U256`/`U384` value types and, where a scratch buffer is needed, `stackalloc`. Parsing and formatting do allocate (they return `string` / consume `ReadOnlySpan<T>` in the usual BCL manner), but the numeric core does not.
- **Native AOT compatible; works in zero-GC environments.** The library is designed to be compatible with **Native AOT** compilation. The math functions work correctly in **zero-GC environments** — including **UEFI applications** — because the numeric core does not depend on a managed heap.
- **No significant performance penalty from the abstractions.** Full IEEE 754 rounding-mode support, abstract encapsulation, and generic-math conformance do not meaningfully slow down the hot paths. Measured throughput is on par with mainstream C, C++, and Rust `binary128` implementations.
- **Blittable storage.** A `Quadruple` is a single 128-bit value; it can be used with `Unsafe.As`, `MemoryMarshal`, `Span<T>`, `fixed` buffers, and P/Invoke without marshalling.

---

## Rounding modes and tie handling

IEEE 754-2019 §4.3 defines five rounding attributes. Each is either **nearest** (with a tie-breaking rule) or **directed** (no tie concept — the value always rounds in the same direction, regardless of distance to the two adjacent representable values):

| `MidpointRounding` | IEEE 754 name | Class | Behaviour at an exact midpoint |
|---|---|---|---|
| `ToEven` | `roundTiesToEven` | nearest (default) | midpoint → the even candidate |
| `AwayFromZero` | `roundTiesToAway` | nearest (recommended) | midpoint → the candidate farther from zero |
| `ToPositiveInfinity` | `roundTowardPositive` | directed | n/a — never a midpoint; all values go up |
| `ToNegativeInfinity` | `roundTowardNegative` | directed | n/a — all values go down |
| `ToZero` | `roundTowardZero` | directed | n/a — all values go toward zero |

A "tie" can only occur in a **nearest** mode: it is the case where the exact result lies exactly halfway between two adjacent representable values. **Directed** modes have no tie-breaking rule because they never look at "which of the two is nearer" — they always round in one fixed direction.

The five `MidpointRounding` values above are the modes selectable on the transcendental and algebraic functions. The arithmetic overloads (`Add(a, b, mode)`, `Multiply(a, b, mode)`, `FusedMultiplyAdd(a, b, c, mode)`, etc.) take a `FloatingPointRounding` instead — a proper superset of `MidpointRounding` that contains all five `MidpointRounding` values and, in addition, `ToNearestWithMidpointToEven` (an alias of `ToEven`), `Upward` (an alias of `ToPositiveInfinity`), `Downward` (an alias of `ToNegativeInfinity`), and `ToNearestWithMidpointTowardZero`, the one non-IEEE mode, which exists only because §9.5 augmented arithmetic requires it and is exposed on the arithmetic overloads for symmetry. All five `MidpointRounding` modes work on every correctly-rounded function, whether or not the overload takes a `FloatingPointRounding`.

### When `ToEven` and `AwayFromZero` differ

Both are nearest modes, so they differ only when the exact result is an exact midpoint between two adjacent binary128 values. That can happen for the basic arithmetic operators (`+`, `-`, `*`, `/`, `%`, FMA, `IEEERemainder`, `ScaleB`, `Round(x, 0, mode)`), where exact midpoints are constructible from rational inputs. Among the library's *named math functions*, only five can produce differing results between the two nearest modes: **`Exp10`, `Exp2`, `Hypot`, `Pow`, and `Compound`**. For every other named math function (`Sin`, `Cos`, `SinCos`, `Tan`, `Asin`, `Acos`, `Atan`, `Atan2`, `Exp`, `ExpM1`, `Log`, `Log2`, `Log10`, `Sqrt`, `Cbrt`, `ReciprocalSqrt`), an exact midpoint is unreachable, so `ToEven` and `AwayFromZero` agree on every binary128 input.

A couple of short demonstrations make the difference concrete:

```csharp
// Sin: the two nearest modes agree; the directed modes differ.
Quadruple x = Quadruple.One;
Console.WriteLine(Quadruple.Sin(x, MidpointRounding.ToEven) ==
                  Quadruple.Sin(x, MidpointRounding.AwayFromZero));   // True
Console.WriteLine(Quadruple.Sin(x, MidpointRounding.ToPositiveInfinity) !=
                  Quadruple.Sin(x, MidpointRounding.ToNegativeInfinity)); // True

// Hypot: an exact midpoint makes ToEven and AwayFromZero differ.
Quadruple a = Quadruple.ScaleB(Quadruple.Parse("6853831853265972805019800674599097"), -113);
Quadruple b = Quadruple.ScaleB(Quadruple.Parse("9138442471021296276733058500941296"), -113);

Quadruple hEven = Quadruple.Hypot(a, b, MidpointRounding.ToEven);
Quadruple hAway = Quadruple.Hypot(a, b, MidpointRounding.AwayFromZero);
Console.WriteLine(hEven != hAway); // True
```

This is also why MPFR's and CORE-MATH's lack of `AwayFromZero` support is not a testing gap: for every correctly-rounded function except `Hypot`, an oracle that exercises `ToEven` implicitly exercises `AwayFromZero`, and `Hypot` — the one correctly rounded member of the five — is covered by the `BigRational`+interval methodology, which supports all five modes. The four not-yet-correctly-rounded members (`Exp10`, `Exp2`, `Pow`, `Compound`) are not compared against reference implementations in either nearest mode. See [Coverage of the `AwayFromZero` mode](#coverage-of-the-awayfromzero-mode) below.

---

## Correct-rounding status

Two categories: **correctly rounded** (every input returns the exact IEEE 754 correctly-rounded value in the specified mode) and **not yet correctly rounded** (composed from correctly-rounded primitives, but the composition itself can lose ulps). No other categories.

In the tables below, the mode names (`ToEven`, `AwayFromZero`, `ToPositiveInfinity`, `ToNegativeInfinity`, `ToZero`) are `MidpointRounding` values. The arithmetic overloads take the equivalent `FloatingPointRounding` values — `ToNearestWithMidpointToEven`, `AwayFromZero`, `Upward`, `Downward`, `ToZero` — whose names line up one-to-one with their IEEE 754 counterparts per the table above.

### Correctly rounded

Every function in this section returns the exact correctly rounded binary128 result. The **Modes** column says which rounding modes can be selected; **—** means the operation is exact by its own definition (its direction is implicit in the name, or there is no rounding decision at all).

| Function | Modes |
|---|---|
| `operator +`, `operator -`, `operator *`, `operator /` | `ToEven` (default) |
| `Add(a, b)`, `Subtract(a, b)`, `Multiply(a, b)`, `Divide(a, b)` | `ToEven` (default) |
| `Add(a, b, mode)`, `Subtract(a, b, mode)`, `Multiply(a, b, mode)`, `Divide(a, b, mode)` | all 5 modes (via the `FloatingPointRounding` overload) |
| `operator %` | `ToEven` (default) |
| `Remainder(a, b)`, `Remainder(a, b, mode)` | `ToEven` (default) / all 5 |
| `IEEERemainder(a, b)`, `IEEERemainder(a, b, mode)` | `ToEven` (default) / all 5 |
| `FusedMultiplyAdd(a, b, c)`, `FusedMultiplyAdd(a, b, c, mode)` | `ToEven` (default) / all 5 |
| `ScaleB(x, n)`, `ScaleB(x, n, mode)` | `ToEven` (default) / all 5 |
| `Round(x, 0, mode)` | all 5 |
| `Sqrt(x)`, `Sqrt(x, mode)` | `ToEven` (default) / all 5 |
| `ReciprocalSqrt(x)`, `ReciprocalSqrt(x, mode)` | `ToEven` (default) / all 5 |
| `Cbrt(x)`, `Cbrt(x, mode)` | `ToEven` (default) / all 5 |
| `Hypot(x, y)`, `Hypot(x, y, mode)` | `ToEven` (default) / all 5 |
| `Sin(x)`, `Sin(x, mode)` | `ToEven` (default) / all 5 |
| `Cos(x)`, `Cos(x, mode)` | `ToEven` (default) / all 5 |
| `SinCos(x)`, `SinCos(x, mode)` | `ToEven` (default) / all 5 |
| `Tan(x)`, `Tan(x, mode)` | `ToEven` (default) / all 5 |
| `Asin(x)`, `Asin(x, mode)` | `ToEven` (default) / all 5 |
| `Acos(x)`, `Acos(x, mode)` | `ToEven` (default) / all 5 |
| `Atan(x)`, `Atan(x, mode)` | `ToEven` (default) / all 5 |
| `Atan2(y, x)`, `Atan2(y, x, mode)` | `ToEven` (default) / all 5 |
| `Exp(x)`, `Exp(x, mode)` | `ToEven` (default) / all 5 |
| `ExpM1(x)`, `ExpM1(x, mode)` | `ToEven` (default) / all 5 |
| `Log(x)`, `Log(x, mode)` | `ToEven` (default) / all 5 |
| `Log2(x)`, `Log2(x, mode)` | `ToEven` (default) / all 5 |
| `Log10(x)`, `Log10(x, mode)` | `ToEven` (default) / all 5 |
| `Floor(x)` | — (`TowardNegative` is the definition of the function) |
| `Ceiling(x)` | — (`TowardPositive` is the definition) |
| `Truncate(x)` | — (`TowardZero` is the definition) |
| `BitIncrement(x)`, `BitDecrement(x)` | — (exact adjacent-value operation; no rounding decision) |
| `Abs(x)`, `CopySign(x, y)` | — (bit manipulation; not a rounding operation) |

### Not yet correctly rounded

These functions compose correctly-rounded primitives but do **not** guarantee correct rounding. They are excluded from the reference implementations below; they are validated only for special-case conformance (NaN propagation, signed zero, `±∞`, exact algebraic identities).

| Function | Current implementation | Notes |
|---|---|---|
| `Exp10` | `Exp(LogOf10 * x)` | Composition of two correctly-rounded steps; can lose ~1 ulp near a boundary. |
| `Exp2` | `Exp(LogOf2 * x)` | Same. |
| `Pow(x, y)` | `Exp(Log(x) * y)` | Correctly rounded `pow` not implemented. |
| `Log(x, newBase)` | `Log(x) / Log(newBase)` | **No rigorous error bound.** For `newBase` close to 1 the denominator `Log(newBase)` is near zero, its own ulp is amplified by the quotient, and the result may be several ulps off. Special cases are handled exactly (`x = 0` → `±∞` depending on `newBase ⋛ 1`; `x < 0` → NaN; `newBase = 0` → `±0` depending on `x ⋛ 1`; `newBase < 0` → NaN; `x = 0 & newBase = +∞` → NaN). |
| `LogP1` | Taylor series for `|x| < 2⁻¹³`, else `Log(1 + x)` | Hard-coded term count. |
| `Log10P1` | `LogP1(x) * Log10OfE` | Composes two errors. |
| `Log2P1` | `LogP1(x) * Log2OfE` | Composes two errors. |
| `Sinh` | `(Exp(x) - Exp(-x)) / 2` + large-`x` cutoff | Catastrophic cancellation for small `x`. |
| `Cosh` | `Hypot(1, Sinh(x))` | Composes two errors. |
| `Tanh` | `(Exp(2x) - 1) / (Exp(2x) + 1)` | Loses accuracy for large `|x|`. |
| `Asinh` | `Log(x + Sqrt(x² + 1))` | Cancellation for `x → 0`. |
| `Acosh` | `Log(x + Sqrt(x − 1)·Sqrt(x + 1))` | Same. |
| `Atanh` | `½·Log((1 + x)/(1 − x))` | Same. |
| `SinPi`, `CosPi`, `SinCosPi` | Reduce to `[−0.5, 0.5]`, then `SinCos(π·r)` | `SinCosPi` multiplies by `π` before calling `SinCos`, losing accuracy at the boundary. |
| `TanPi` | `SinPi(x) / CosPi(x)` | Composition of two unrounded primitives. |
| `AsinPi`, `AcosPi`, `AtanPi`, `Atan2Pi` | Divide by `Pi` after base function | 1-ulp-losing division by π on top of the base function's rounding. |
| `Compound(x, n)` | `Exp(LogP1(x) * n)` | Correct rounding requires `(1 + x)^n` via `expm1` with exact `n·log1p(x)`. |
| `RootN(x, n)` | `Exp(Log(x) / n)` (+ `Sqrt`/`Cbrt`/`ReciprocalSqrt` special cases) | Requires Newton–Raphson refinement with a final exactness check. |
| `Round(x, digits, mode)` with `digits ≠ 0` | `Round(x·Exp10(digits), 0, mode)` | The intermediate multiply and the rescale are both correctly rounded, but the composition is not. |

---

## Augmented arithmetic (IEEE 754-2019 §9.5)

```csharp
public static (Quadruple Rounded, Quadruple Delta) AddAugmented      (Quadruple x, Quadruple y);
public static (Quadruple Rounded, Quadruple Delta) SubtractAugmented (Quadruple x, Quadruple y);
public static (Quadruple Rounded, Quadruple Delta) MultiplyAugmented (Quadruple x, Quadruple y);
```

The contract is `Rounded + Delta == x ⊕ y` as exact reals, where `⊕` is `+`, `-`, or `·`. Implementations honour:

- **The midpoint-toward-zero rounding for the rounded part** — mandated by §9.5.
- **NaN propagation** — both outputs become the quiet NaN derived from both inputs, per §9.5.
- **The zero-delta sign rule** — if `Delta` is zero, it carries the sign of `Rounded`. Handled both in the addition/subtraction path and, independently, in the multiplication path.
- **Subnormal-underflow sign** — when the exact product's delta underflows to ±0 *without* being exact, the sign comes from the FMA computation, not from `Rounded`.
- **Infinity/NaN edge cases** — `∞ + (−∞)` → canonical quiet NaN in both outputs; `0 · ∞` likewise; `0 · finite` → `(+0, +0)`; `x · ∞` → `(±∞, ±∞)`.

---

## Conversions

`Quadruple` converts to and from a range of .NET numeric types. Which conversions are available depends on the target framework:

| Type | Direction | Availability |
|---|---|---|
| `double`, `float`, `Half` | ⇄ | .NET 7+ |
| `decimal` | ⇄ | .NET 7+ |
| `BigInteger` | ⇄ | .NET 7+ |
| `System.UInt128`, `System.Int128`, `UltimateOrb.UInt128`, `UltimateOrb.Int128` | ⇄ | .NET 7+ |
| Standard integer types (`byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `nint`, `nuint`, `char`) | ⇄ | .NET 7+ |
| `BFloat16` | ⇄ | .NET 11+ |
| `System.Numerics.Decimal32` | ⇄ | .NET 11+ |
| `System.Numerics.Decimal64` | ⇄ | .NET 11+ |
| `System.Numerics.Decimal128` | ⇄ | .NET 11+ |

All conversions use correct rounding (`MidpointRounding.ToEven`), propagate NaN correctly, and preserve signed zero. The `BFloat16` and `System.Numerics.Decimal*` conversions are guarded by `#if NET11_0_OR_GREATER` in the library, so they are only available when compiling against a framework that provides those types.

For the IEEE 754 binary interchange family, `FromIeee754InterchangeBinary<TFloat, TFloatUIntBits>` and `ToIeee754InterchangeBinaryNarrowing` cover binary16, binary32, binary64, binary128, and the `k ≥ 128, k % 32 == 0` formats (binary160/192/224/256). These also use correct rounding and honour the configured NaN payload policy.

---

## String parsing and formatting

This is the most complete parsing/formatting surface among .NET `binary128` libraries, and compares favourably with `System.Double` itself.

### Rounding guarantees

Every formatting operation — shortest round-trip (`G36`, default `G`), fixed-point (`F`), scientific (`E`/`e`), numeric (`N`), currency (`C`), percent (`P`), and every custom specifier — produces a **correctly rounded** decimal at the requested precision, using `MidpointRounding.ToEven`. There is no heuristic digit count and no accumulated truncation: the printed digits are exactly what IEEE 754 prescribes for that precision.

The default `G` format (and the explicit `G36` specifier) produce a decimal that parses back to exactly the original `Quadruple` under `MidpointRounding.ToEven`, and is **minimal** — no shorter decimal does. Values that round-trip in fewer digits are printed with fewer digits; the maximum for a `binary128` is 36 significant digits.

Fixed-point and scientific formats pad with zeros and suppress trailing zeros *after* rounding, never before, so `x.ToString("F0")` and `Quadruple.Round(x, 0, MidpointRounding.ToEven).ToString("G")` agree bit-for-bit.

### Parsing

| Capability | Surface |
|---|---|
| **Entry points** | `Parse(string?)`, `Parse(ReadOnlySpan<char>)`, `Parse(ReadOnlySpan<byte>)` (UTF-8), plus `TryParse` for each, plus public `TryParsePartial` with `charsConsumed`/`bytesConsumed`. |
| **`NumberStyles`** | `AllowLeadingWhite`, `AllowTrailingWhite`, `AllowLeadingSign`, `AllowTrailingSign`, `AllowParentheses`, `AllowDecimalPoint`, `AllowThousands`, `AllowExponent`, `AllowCurrencySymbol`, `AllowHexSpecifier` — the same set `System.Double` accepts, with the same validity rules. |
| **Hex floats** | `0x<hex>[.<hex>]p<±dd>` syntax. `NumberStyles.AllowHexSpecifier` is required on **every** target. On .NET 11+ the dedicated `NumberStyles.HexFloat` is OR-ed on top of it; on earlier frameworks the `NumberStyles.HexNumber \| NumberStyles.Float` pair replaces `HexFloat`, still in combination with `AllowHexSpecifier`. See the example below. |
| **`NumberFormatInfo`** | `NumberDecimalSeparator`, `NumberGroupSeparator`, `CurrencySymbol`, `NegativeSign`, `PositiveSign`, `NaNSymbol`, `PositiveInfinitySymbol`, `NegativeInfinitySymbol` — all culture-sensitive, including cultures with uneven grouping (e.g. `en-IN`'s 3‑2‑2). |
| **Decimal numbers** | Full integer-part / fraction-part / exponent syntax, group separators inside either part (validated only between digits), correct handling of `-0` and signed zero. |
| **NaN payloads** | `NaN(payload)` is the base form (quiet by default); a `Q` or `S` marker inside the parentheses forces the class: `NaN(Qpayload)` is quiet, `NaN(Spayload)` is signaling. `sNaN(payload)` is the shorthand signaling form. Payloads are folded modulo 2¹¹⁰; the overflow indicator bit is set when the parsed integer had any bit at position ≥ 110, or when the raw fraction would otherwise become all-zero (so `sNaN(0)` decodes as a signaling NaN, not as an infinity). **Not supported by `System.Double`.** |
| **Infinity / NaN symbols** | Read from `NumberFormatInfo`, case-insensitive, with leading `+`/`-` and `NaNSymbol` prefix recognition, and locale-safe handling when the NaN symbol itself starts with `s` or `q`. |
| **Whitespace** | Full Unicode whitespace (`U+0009`–`U+000D`, `U+0085`, `U+00A0`, `U+1680`, `U+2000`–`U+200A`, `U+2028`, `U+2029`, `U+202F`, `U+205F`, `U+3000`, plus `' '`). |
| **Trailing-invalid mode** | `TryParsePartial` exposes `AllowTrailingInvalidCharacters` publicly (in the BCL this is an `INumberBase` explicit-interface member only). |

### Formatting

| Capability | Surface |
|---|---|
| **Entry points** | `ToString()`, `ToString(format)`, `ToString(format, provider)`, `TryFormat(Span<char>, …)` (`ISpanFormattable`), `TryFormat(Span<byte>, …)` (`IUtf8SpanFormattable`, .NET 8+). `ToString` auto-grows via `ArrayPool` from a 64-char stack buffer up to `Array.MaxLength`. |
| **Standard specifiers** | `G` (default and `G<prec>`), `F<prec>`, `E<prec>`/`e<prec>`, `N<prec>`, `R`, `C<prec>`, `P<prec>` — the same set as `System.Double`, with precision up to 999,999,999. |
| **Custom specifiers** | `0`, `#`, `.`, `,` (grouping between two placeholders, *scaling* before `.` with no intervening placeholder), `%` (×100), `‰` (×1000), `E0`/`E+0`/`E-0`/`e0`/`e+0`/`e-0`, `\` escape, `'`/`"` literal quoting, `;` sections (positive / negative / zero). |
| **`NumberFormatInfo`** | All numeric, currency, and percent formatting properties are respected, including grouping and pattern selection. |
| **No precision cap** | Output length is limited only by the destination buffer length (or, for the auto-growing `ToString`, by `Array.MaxLength`). |
| **NaN output** | Emits `NumberFormatInfo.NaNSymbol`, `PositiveInfinitySymbol`, `NegativeInfinitySymbol` — matching `System.Double`. (NaN *payload* parsing is available; the default formatter emits the canonical symbol.) |

`System.Double` and `UltimateOrb.Quadruple` share the same custom-format-specifier language; the systematic differences are (a) `Quadruple` supports NaN-payload parsing that `Double` does not, (b) `Quadruple` exposes `TryParsePartial` publicly, and (c) `Quadruple` can produce up to 36 round-trip digits (`G36`) rather than `Double`'s 17.

### Hex-float example

```csharp
#if NET11_0_OR_GREATER
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexFloat | NumberStyles.AllowHexSpecifier);
#else
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexNumber | NumberStyles.Float | NumberStyles.AllowHexSpecifier);
#endif
```

`AllowHexSpecifier` is the flag that enables the `0x…` hexadecimal significand, and it is required on **every** target. What changes with .NET 11 is only how the rest of the hex-float grammar is named: `NumberStyles.HexFloat` on .NET 11+, `NumberStyles.HexNumber | NumberStyles.Float` on earlier frameworks. Either way, that portion is OR-ed with `AllowHexSpecifier` — never in place of it.

---

## GPU usage via ILGPU

There is **no separate GPU project**: `Quadruple` itself, and the basic arithmetic that backs its operators, are written in a subset of C# that ILGPU can compile today. The same `Quadruple` value type used in CPU code can be used directly inside an ILGPU kernel without conversion, wrappers, or a companion assembly — just add ILGPU, `using ILGPU;`, and write the kernel against `ArrayView<Quadruple>`.

### Currently GPU-compilable

- `+`, `-`, `*`, `/`, `%` (all backed by `Binary128Arithmetic` routines that ILGPU's backend accepts)
- `FusedMultiplyAdd`
- Comparison operators (`==`, `!=`, `<`, `>`, `<=`, `>=`)
- `Abs`, `CopySign`, `BitIncrement`, `BitDecrement`
- Construction from `int`/`uint`/`long`/`ulong`/`float`/`double` and explicit narrowing conversion back

These are bit-identical to the CPU path because both backends execute the same IL and the same `Binary128Arithmetic` methods; the ILGPU JIT compiler lowers the `UInt128` arithmetic to the backend's native 32-/64-bit integer instructions.

### Future GPU-compilable

The higher-level functions — `Sqrt`, `Cbrt`, `Hypot`, `Sin`, `Cos`, `Tan`, `Exp`, `Log`, `Atan2`, `Pow`, and the rest of the correctly-rounded and not-yet-correctly-rounded catalogs — are also pure C#, but they currently rely on constructs that ILGPU 1.x does not yet lower: **static readonly tables** (the coefficient arrays and the Payne–Hanek `2/π` limbs, read through `ReadOnlySpan<ulong>` views) and **spans** more generally. Once **ILGPU 2.0** finalizes its planned support for these constructs, the same `Quadruple` type will become GPU-compilable for the entire API surface **automatically**, with no change to `UltimateOrb.Quadruple` and no additional assembly.

Until then, code that needs to run on a GPU should stay within the currently-compilable subset above, or split the kernel into a host-side call to the higher-level functions and a device-side loop over the basic operations.

### Example

```csharp
using ILGPU;
using ILGPU.Runtime;
using UltimateOrb;

var ctx    = Context.CreateDefault();
using var dev = ctx.CreateCudaAccelerator();   // or OpenCL / CPU

var kernel = dev.LoadStreamKernel<ArrayView<Quadruple>>(AddKernel);
var buf    = dev.Allocate1D<Quadruple>(n);
kernel((int)buf.Length, buf.View);
dev.Synchronize();

static void AddKernel(ArrayView<Quadruple> buf) {
    int i = Grid.IdxX;
    buf[i] = Quadruple.Add(buf[i], Quadruple.One)
           + Quadruple.FusedMultiplyAdd(buf[i], buf[i], Quadruple.One);
}
```

---

## Testing methodology

The tests draw on **four reference implementations**. Each one is applied to the subset of correctly-rounded functions it can address; the four cover different areas, and no single one is a complete oracle for the whole surface.

### a. MPFR

MPFR is used as a correctly-rounded binary128 oracle by configuring it to operate at binary128's precision and exponent range, so that a single MPFR operation with the desired rounding mode produces exactly the correctly-rounded binary128 result — no higher precision is involved:

```csharp
// SubNormalize consults the process-wide MPFR emin/emax.
MpfrFloat.DefaultPrecision = 113;
MpfrFloat.EMin = -16382 - 113 + 2;
MpfrFloat.EMax = 16383 + 1;
```

With these settings in place, the comparison is `library(x, mode) == mpfr_result(x, mode)` — a mode-aware equality at binary128 precision, not a magnitude comparison against a higher-precision intermediate.

MPFR supports four of the five IEEE 754 rounding attributes — `ToEven`, `ToPositiveInfinity`, `ToNegativeInfinity`, `ToZero` — but **not** `AwayFromZero`. See [Coverage of the `AwayFromZero` mode](#coverage-of-the-awayfromzero-mode).

From .NET, MPFR can be reached either directly through P/Invoke or through the **`Sdcb.Arithmetic.Mpfr`** binding — both target the same native MPFR library and expose the same rounding-mode surface. The code above shows the binding's configuration hooks (`MpfrFloat.DefaultPrecision`, `MpfrFloat.EMin`, `MpfrFloat.EMax`) because they are the most convenient way to set MPFR's precision and exponent range from managed code; an equivalent P/Invoke wrapper would configure the same underlying MPFR state. The library is used here only as a test oracle, never at runtime.

### b. `BigRational` + interval arithmetic

A single, combined methodology, not two separate test references:

- `BigRational` supplies **exact** arithmetic — either the exact answer where the answer is rational, or exact rational *bounds* on the true value derived from the same analytical error bound the implementation documents in its source.
- Interval arithmetic then **uses** those `BigRational` endpoints to prove the correct rounding: the test rounds both `lo` and `hi` to `binary128` in the mode being tested, and correct rounding is proven when the two rounded results coincide. If they differ, the input is rejected as inconclusive and re-tested at higher working precision or escalated to MPFR directly.

Because the methodology is built on exact rational arithmetic, it supports **all five** IEEE 754 rounding attributes, including `AwayFromZero`.

Because this methodology requires a tight rational *bound* on the true value (not merely the true value itself), it is applied only to the subset of functions where such bounds are practical to derive. In this library it currently covers:

- All basic arithmetic and augmented arithmetic (where the answer is exactly rational).
- `IEEERemainder`, `ScaleB`, `FusedMultiplyAdd`, `Round(x, 0, mode)` (exact rational).
- `Sqrt` and `Hypot` (algebraic — rational bounds come from Newton iteration on `x²` and `x² + y²`).
- `Exp`, `ExpM1`, `Log`, `Log2`, `Log10` (irrational, but the implementation's own analytical bounds translate directly into `BigRational` intervals).

The trig, inverse-trig, `Cbrt`, and `ReciprocalSqrt` families are **not** covered by this reference implementation: their analytical bounds are either looser or require trigonometric/rational arguments that are not practical to reduce to exact rational comparisons at the required working precision. They are verified by MPFR alone.

### c. JIT-compiled LLVM `fp128` stubs

For the **basic operations** (`+`, `-`, `*`, `/`, `%`, `FusedMultiplyAdd`, `Sqrt`) **and floating-point/integer conversions**, an independent reference is obtained by JIT-compiling LLVM IR stubs that perform the corresponding `llvm.experimental.constrained.*` intrinsic on LLVM's `fp128` type. LLVM's constrained FP intrinsics accept a rounding-mode argument, so each mode is tested independently. This reference is deliberately independent of MPFR and `BigRational`: it uses a different compiler backend and a different (though still IEEE-conformant) implementation of the primitives, which makes it sensitive to classes of error that a single high-precision reference might miss. LLVM's constrained FP intrinsics expose `tonearest`, `downward`, `upward`, and `towardzero`, but **not** `roundTiesToAway`, so `MidpointRounding.AwayFromZero` is not exercised by this reference implementation. (AwayFromZero on these operations is covered by `BigRational`+interval.)

### d. CORE-MATH

For the correctly-rounded functions for which CORE-MATH currently provides a binary128 implementation, the tests draw on **two complementary uses** of the CORE-MATH project:

- **CORE-MATH's own correct-rounding C/C++ binary128 implementations are used as an additional independent oracle.** For each function in this subset, the C/C++ reference implementation from CORE-MATH (`sinq`, `cosq`, `expq`, `expm1q`, `logq`, `cbrtq`, `hypotq`, `sqrtq`, `atan2q`, `acosq`, `asinq`, `atanq`, `rsqrtq`, …) is compiled and its output on the same inputs is compared bit-for-bit with `UltimateOrb.Quadruple`'s result. This shares no code or algorithmic heritage with MPFR, `BigRational`, or LLVM, so it catches classes of error that an oracle built from a single family of algorithms might miss.
- **CORE-MATH adversarial input generators adapted to `binary128` are used as an adversarial test reference.** These target inputs lying extremely close to a rounding boundary — precisely the cases that random sampling never sees and that extra-digit comparison misses. The adapted generators have been cross-checked against the other three reference implementations.

CORE-MATH does **not** support `AwayFromZero`. See [Coverage of the `AwayFromZero` mode](#coverage-of-the-awayfromzero-mode).

Functions outside CORE-MATH's binary128 coverage, and all not-yet-correctly-rounded functions, are **not** exercised by this source.

### Coverage of the `AwayFromZero` mode

MPFR and CORE-MATH do not support IEEE 754 `roundTiesToAway`, and the LLVM stub reference covers only four of the five modes on the basic operations and conversions. This is **not** a coverage gap for the correctly-rounded functions, for the following reason.

`ToEven` and `AwayFromZero` differ only when the exact result is an exact midpoint. That can happen only for:

- The basic arithmetic operators (`+`, `-`, `*`, `/`, `%`, FMA, `IEEERemainder`, `ScaleB`, `Round(x, 0, mode)`), where exact midpoints are constructible from rational inputs.
- The named math functions `Exp10`, `Exp2`, `Hypot`, `Pow`, `Compound`.

For every other named math function (`Sin`, `Cos`, `SinCos`, `Tan`, `Asin`, `Acos`, `Atan`, `Atan2`, `Exp`, `ExpM1`, `Log`, `Log2`, `Log10`, `Sqrt`, `Cbrt`, `ReciprocalSqrt`), an exact midpoint is unreachable for any binary128 input, so `ToEven` and `AwayFromZero` produce identical results. Testing such a function under `ToEven` therefore tests it under `AwayFromZero` as well, and MPFR's and CORE-MATH's `ToEven` coverage suffices for both nearest modes.

Among the five named functions where the two modes can differ:

- `Hypot` is correctly rounded and is covered by the `BigRational`+interval methodology, which exercises all five modes (including `AwayFromZero`).
- `Exp10`, `Exp2`, `Pow`, and `Compound` are not yet correctly rounded and are therefore not compared against any reference implementation in either nearest mode.

The basic arithmetic operators, augmented arithmetic, `IEEERemainder`, `ScaleB`, `Round(x, 0, mode)`, and `Floor`/`Ceiling`/`Truncate` are covered by `BigRational`+interval, which exercises all five modes. FP-int conversions are covered by the LLVM stub reference (four modes) and by `BigRational`+interval (all five modes, since the exact result of an integer-to-`binary128` conversion is rational).

So `AwayFromZero` is exercised by at least one reference implementation for **every correctly-rounded function in the library** — either directly by `BigRational`+interval (for `Hypot`, the basic arithmetic operations, the augmented operations, the exact algebraic ones, and the conversions), or implicitly via `ToEven` equivalence by MPFR and CORE-MATH (for the remaining transcendentals).

### How the four fit together

The four reference implementations are complementary: MPFR is the workhorse for the trig and exponential families; the `BigRational`+interval methodology is exhaustive for the algebraic operations and for functions with a well-characterised analytical bound, and closes specific boundary cases flagged by MPFR as "within one ulp of the midpoint"; the LLVM stub reference provides a backend-independent cross-check of the basic arithmetic primitives and conversions; CORE-MATH adds an independent implementation-based oracle and an adversarial input generator, specifically designed to expose errors that random sampling cannot reach.

### CORE-MATH binary128 coverage is partial

As of the current CORE-MATH release, its binary128 column is populated only for a subset of the standard mathematical functions: `acos`, `asin`, `atan`, `atan2`, `cbrt`, `cos`, `exp`, `exp10`, `exp2`, `expm1`, `hypot`, `log`, `rsqrt`, `sin`, `sqrt`. CORE-MATH does **not** provide reference implementations for basic arithmetic (`+`, `-`, `*`, `/`, `%`, FMA, etc.), for the augmented-arithmetic operations, for FP-int conversions, or for many transcendental functions (`tan`, `log2`, `log10`, `sincos`, the full hyperbolic family, `sinpi`/`cospi`/`tanpi`, the inverse-hyperbolic family, `pow`, `compound`, `erf`, `erfc`, `lgamma`, `tgamma`, etc.). Consequently:

- Among the correctly-rounded library functions, the ones with CORE-MATH binary128 coverage — and therefore also covered by both the CORE-MATH implementation oracle and the CORE-MATH adversarial input generator suite — are `Sin`, `Cos`, `Asin`, `Acos`, `Atan`, `Atan2`, `Cbrt`, `Exp`, `ExpM1`, `Hypot`, `Log`, `Sqrt`, and `ReciprocalSqrt` (CORE-MATH `rsqrt`).
- The remaining correctly-rounded library functions — `SinCos`, `Tan`, `Log2`, `Log10`, `Floor`, `Ceiling`, `Truncate`, FP-int conversions, and the entire basic-arithmetic, FMA, `IEEERemainder`, `ScaleB`, `Round`, and augmented-arithmetic surface — are not exercised by CORE-MATH.
- The not-yet-correctly-rounded library functions are not covered by CORE-MATH at all.

### Analytical bounds in the implementation

Every `Binary128Arithmetic` path documents its own analytical error bound in the source:

- The `Sin` fast path's `SIN_FAST` polynomial has a certified Q128 error `< 2⁻¹¹⁴` on `[0, 3.765e-5]`.
- The Ziv gate's `ZIV_GATE = 16` is set from a derivation showing the fast-path error is `< 15.8` units of `2^(e2-128)`, including reduction, evaluation, and recombination; the derivation is referenced to `sinq_error.md`.
- The 384-bit accurate path's `alternating` polynomial has tail error under `2⁻²⁸²`, well below the 143 bits `round_384` needs.
- `MultiplyHighUnsignedApproximate` has documented slack of "up to four units short, never over" for the 384-bit variant and "up to one unit short, never over" for the 256-bit variant, both with the `round_*` guard bits sized to absorb the slack.

### Test-status matrix

A **✔** means the function is exercised by that reference implementation; **—** means the reference implementation is not applicable to that function (or, for CORE-MATH, that CORE-MATH has no binary128 reference implementation for it); **✗** means the function is correctly rounded and verified by MPFR but *not* covered by that specific reference.

The MPFR column covers four of the five IEEE modes; MPFR does not support `AwayFromZero`. The LLVM column covers the basic operations and FP-int conversions in four of the five modes. CORE-MATH does not support `AwayFromZero`. See [Coverage of the `AwayFromZero` mode](#coverage-of-the-awayfromzero-mode) for why this is not a coverage gap.

| Area | MPFR (4/5 modes) | BigRational + Interval (5/5 modes) | LLVM (basic ops + conversions, 4/5 modes) | CORE-MATH binary128 (impl + adversarial inputs) |
|---|---|---|---|---|
| Add / Sub / Mul / Div / Rem | ✔ | ✔ | ✔ | — (not in CORE-MATH) |
| FMA | ✔ | ✔ | ✔ | — (not in CORE-MATH) |
| Sqrt | ✔ | ✔ | ✔ | ✔ |
| FP-int conversions | ✔ | ✔ | ✔ | — (not in CORE-MATH) |
| IEEERemainder, ScaleB, Round(x, 0, mode) | ✔ | ✔ | — (not a basic op) | — (not in CORE-MATH) |
| Augmented arithmetic | ✔ | ✔ | — (not a basic op) | — (not in CORE-MATH) |
| Floor, Ceiling, Truncate | ✔ | ✔ | — (not a basic op) | — (not in CORE-MATH) |
| Hypot | ✔ | ✔ | — (not a basic op) | ✔ |
| Exp, ExpM1 | ✔ | ✔ | — (not a basic op) | ✔ |
| Log | ✔ | ✔ | — (not a basic op) | ✔ |
| Log2, Log10 | ✔ | ✔ | — (not a basic op) | — (no CORE-MATH binary128) |
| Cbrt, ReciprocalSqrt | ✔ | ✗ | — (not a basic op) | ✔ |
| Sin, Cos, SinCos | ✔ | ✗ | — (not a basic op) | ✔ (Sin, Cos); — (SinCos — CORE-MATH only through binary64) |
| Tan | ✔ | ✗ | — (not a basic op) | — (CORE-MATH only through binary64) |
| Asin, Acos, Atan, Atan2 | ✔ | ✗ | — (not a basic op) | ✔ |

The not-yet-correctly-rounded functions are not covered by any of the four columns; they are validated only for special-case conformance (NaN propagation, signed zero, `±∞`, exact algebraic identities).

---

## Comparison with other .NET quadruple-precision implementations

Each of the projects below is a serious, well-maintained effort in its own right. The goal of this section is to help a reader pick the library that best fits their specific needs — not to rank them.

### vs. QuadrupleLib (`IsaMorphic/QuadrupleLib`)

QuadrupleLib is a modern `binary128` implementation targeting .NET 8+, also implementing `IBinaryFloatingPointIeee754<Quadruple>`. It offers a choice of accelerators (`DefaultAccelerator`, `SoftwareAccelerator`) and correctly implements the core transcendental surface.

- **Per-function rounding modes.** Every correctly-rounded function accepts a `MidpointRounding` (`Sin(x, mode)`, `Log(x, mode)`, …), and every arithmetic operation accepts the richer `FloatingPointRounding` (`Add(a, b, mode)`, …).
- **`roundTiesToAway` (`MidpointRounding.AwayFromZero`) on transcendentals.**
- **Augmented arithmetic (§9.5).** `AddAugmented`, `SubtractAugmented`, `MultiplyAugmented`.
- **Parsing beyond the standard surface.** Hex floats, NaN-payload parsing (including the `Q`/`S` marker forms), public `TryParsePartial`.
- **Formatting beyond the standard surface.** Full custom-specifier language, exact round-half-to-even on every format path, `IUtf8SpanFormattable`.
- **NaN payload control.** `GetPayload`, `GetQuietNaNWithPayload`, `GetSignalingNaNWithPayload`, three payload modes.
- **Cross-format conversion.** `FromIeee754InterchangeBinary` / `ToIeee754InterchangeBinaryNarrowing` for the full IEEE 754 binary interchange family.
- **`decimal` and `BigInteger` conversions**, with correct rounding.
- **Documented testing methodology.** MPFR, `BigRational`+interval, LLVM, and CORE-MATH — see the testing section above.
- **Target framework floor.** .NET 7 vs .NET 8.

**GPU compatibility.** QuadrupleLib's GPU coverage is slightly higher than `UltimateOrb.Quadruple`'s — though the difference is small. The reason is twofold: QuadrupleLib provides a `SoftwareAccelerator` implementation that avoids certain intrinsics ILGPU does not yet support, and its higher-level functions similarly steer clear of some constructs that ILGPU 1.x cannot lower. In contrast, `UltimateOrb.Quadruple`'s higher-level functions currently rely on **static readonly tables** (the coefficient arrays and the Payne–Hanek `2/π` limbs, read through `ReadOnlySpan<ulong>` views) and **spans** more generally, which ILGPU 1.x does not yet lower. Once **ILGPU 2.0** finalizes its planned support for these constructs, the whole `UltimateOrb.Quadruple` surface will become GPU-compilable automatically — but until then, `UltimateOrb.Quadruple`'s GPU-compilable subset is narrower than QuadrupleLib's.

### vs. MissingValues (`crookseta/missing-values`)

MissingValues is a .NET 8+ numeric library that provides `Quad` (128-bit quadruple precision) and `Octo` (256-bit octuple precision) floating-point types, alongside large integer types (`Int256`/`Int512`, `UInt256`/`UInt512`). `Quad` is a 128-bit type with a 113-bit significand and the same exponent range as `binary128` (max exponent 16383, min −16382), while `Octo` extends to a 237-bit significand and an exponent range up to 262143. The library implements generic math interfaces, including `IBinaryFloatingPointIeee754` for `Quad`, and offers basic arithmetic operators, conversions, parsing, and bit-level utilities.

The library does not document correct rounding guarantees for its floating-point operations, nor does it expose the five IEEE 754 rounding attributes; its arithmetic appears to use the default rounding. Transcendental function coverage is not described in its README, and the library is positioned primarily as a large-integer and higher-precision basic-arithmetic solution rather than a correctly-rounded math library.

By contrast, `UltimateOrb.Quadruple` is built around correct rounding, exposes all five rounding attributes on every supported function, provides a broad set of correctly-rounded transcendentals, and includes augmented arithmetic, comprehensive parsing/formatting, and extensive validation against multiple reference implementations.

### vs. NetQD (`rzikm/NetQD`)

NetQD provides **double-double** and **quad-double** arithmetic — algorithms that represent a number as an unevaluated sum of `double`s. This is a fundamentally different design from the IEEE 754 `binary128` interchange format:

- The encoding is a pair (or quad) of `double`s; bit-level interchange with hardware or other languages that use the binary128 interchange format requires an explicit conversion step.
- Double-double's effective significand is roughly 106 bits and quad-double's is roughly 212 bits — neither matches the 113-bit binary128 significand, and the error distributions differ.
- NetQD is designed around its own algorithms; code written against `IBinaryFloatingPointIeee754<T>` uses a different type and calling convention than NetQD's.

If bit-level interchange with `__float128`, `libquadmath`, POWER9 hardware, or any other conforming `binary128` implementation matters, `UltimateOrb.Quadruple` — which has the exact IEEE 754 `binary128` layout — is the appropriate choice. If the workload is well-served by an unevaluated-sum representation and can use NetQD's API directly, NetQD may be an excellent fit.

### vs. `libquadmath` / GCC `__float128`

`libquadmath` is the GCC runtime library for `__float128`, implemented in C. `UltimateOrb.Quadruple` differs in the following ways:

- **Managed, portable, no native linkage.** `libquadmath` requires native linkage (or a P/Invoke wrapper); `UltimateOrb.Quadruple` is pure managed C# and runs on any .NET 7+ platform without native dependencies.
- **Generic-math integration.** `Quadruple` implements `IBinaryFloatingPointIeee754<Quadruple>`, so it composes with `System.Numerics` and works with generic numeric code.
- **Rounding-mode coverage.** `libquadmath`'s C API (`sinq(x)`, `cosq(x)`, `expq(x)`, …) does not carry a rounding-mode parameter; the standard `roundTiesToEven` is implied. `UltimateOrb.Quadruple` exposes all five IEEE 754 attributes on every correctly-rounded function, including `AwayFromZero`.
- **Augmented arithmetic (§9.5).** `UltimateOrb.Quadruple` provides `AddAugmented`, `SubtractAugmented`, `MultiplyAugmented`.
- **NaN-payload control.** `UltimateOrb.Quadruple` exposes `GetPayload`, `GetQuietNaNWithPayload`, `GetSignalingNaNWithPayload`, and three payload-handling modes.
- **Native AOT and zero-GC.** `UltimateOrb.Quadruple` is Native AOT compatible and its math functions work in zero-GC environments, including UEFI applications.

### vs. MPFR

MPFR is the gold standard for arbitrary-precision correctly-rounded floating point, and is used by this library **as a test oracle**, never at runtime. When choosing between MPFR and `UltimateOrb.Quadruple`:

- **Native linkage and per-operation cost.** MPFR is a native library (reachable from .NET through P/Invoke, either hand-written or via a binding such as `Sdcb.Arithmetic.Mpfr`) and, per operation, is generally heavier than a dedicated fixed-format implementation. `UltimateOrb.Quadruple` is entirely managed and tuned for `binary128`.
- **Rounding-mode coverage.** MPFR supports four IEEE 754 rounding attributes (`ToEven`, `ToPositiveInfinity`, `ToNegativeInfinity`, `ToZero`) and does not expose `AwayFromZero`; `UltimateOrb.Quadruple` supports all five.
- **Augmented arithmetic (§9.5).** `UltimateOrb.Quadruple` provides `AddAugmented`, `SubtractAugmented`, `MultiplyAugmented`.
- **Generality.** MPFR supports arbitrary precision (not just `binary128`). If the workload needs to tune precision at runtime above `binary128`, MPFR is the appropriate tool. If fixed `binary128` precision is what the workload needs, `UltimateOrb.Quadruple` is the more focused choice.

If you want to call MPFR from .NET without writing your own P/Invoke layer, the **`Sdcb.Arithmetic.Mpfr`** package is the binding to consider. It exposes `MpfrFloat` as a managed wrapper around the native `mpfr_t` handle and offers the same rounding-mode surface as MPFR itself. `UltimateOrb.Quadruple` remains the better fit when a managed-only, fixed-`binary128`, zero-GC implementation with all five IEEE modes is what the workload needs.

---

## Usage examples

### Correct rounding with an explicit mode

```csharp
#pragma warning disable UoWIP   // acknowledge the [Experimental] status of Quadruple

using UltimateOrb;

Quadruple x = (Quadruple)0.7;

Quadruple sEven = Quadruple.Sin(x);                                   // ToEven (default)
Quadruple sAway = Quadruple.Sin(x, MidpointRounding.AwayFromZero);    // AwayFromZero
Quadruple sUp   = Quadruple.Sin(x, MidpointRounding.ToPositiveInfinity);
Quadruple sDown = Quadruple.Sin(x, MidpointRounding.ToNegativeInfinity);
Quadruple sZero = Quadruple.Sin(x, MidpointRounding.ToZero);
```

### Basic arithmetic with explicit rounding

```csharp
using UltimateOrb;
using UltimateOrb.Numerics;

Quadruple a = Quadruple.Pi;
Quadruple b = Quadruple.E;

// Default operator (ToEven):
Quadruple d0 = a * b;

// Named method, directed modes:
Quadruple d1 = Quadruple.Multiply(a, b, FloatingPointRounding.Upward);
Quadruple d2 = Quadruple.Multiply(a, b, FloatingPointRounding.Downward);

// Named method, midpoint-toward-zero (used internally by augmented arithmetic):
Quadruple d3 = Quadruple.Multiply(a, b, FloatingPointRounding.ToNearestWithMidpointTowardZero);

// MidpointRounding works too:
Quadruple d4 = Quadruple.FusedMultiplyAdd(a, b, Quadruple.One, MidpointRounding.AwayFromZero);
```

### Augmented arithmetic

```csharp
Quadruple a = (Quadruple)1e200;
Quadruple b = (Quadruple)1e-200;

var (r, d) = Quadruple.AddAugmented(a, b);
// r is the midpoint-toward-zero rounded result of a + b;
// d is the exact error: r + d == a + b as real numbers,
// and d is representable in binary128.
```

### Parsing and formatting

```csharp
using System.Globalization;
using UltimateOrb;

// Culture-sensitive parsing:
var nfi = (NumberFormatInfo)CultureInfo.GetCultureInfo("de-DE").NumberFormat.Clone();
Quadruple a = Quadruple.Parse("1.234,567", NumberStyles.Float | NumberStyles.AllowThousands, nfi);

// Hex float — AllowHexSpecifier is required on every target;
// .NET 11+ adds HexFloat on top of it, earlier frameworks use
// HexNumber | Float on top of it.
#if NET11_0_OR_GREATER
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexFloat | NumberStyles.AllowHexSpecifier);
#else
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexNumber | NumberStyles.Float | NumberStyles.AllowHexSpecifier);
#endif

// NaN with an explicit payload and class — the class marker goes inside
// the parentheses, and the default Parse overload is sufficient:
Quadruple qnan = Quadruple.Parse("NaN(Q123)");   // quiet NaN, payload 123
Quadruple snan = Quadruple.Parse("NaN(S42)");    // signaling NaN, payload 42
Quadruple payload = Quadruple.GetPayload(qnan);  // (Quadruple)123

// UTF-8 parse:
Span<byte> utf8 = stackalloc byte[32];
"1.5e100"u8.CopyTo(utf8);
Quadruple c = Quadruple.Parse(utf8);

// Format with any standard or custom specifier — all correctly rounded:
string s1 = Quadruple.Pi.ToString("G36");   // shortest round-trip (explicit, recommended)
string s2 = Quadruple.Pi.ToString("E10");   // scientific, 10 digits
string s3 = Quadruple.Pi.ToString("000.00000");
string s4 = Quadruple.Pi.ToString("#,##0.00##", CultureInfo.InvariantCulture);

// Partial parse with explicit trailing content:
bool ok = Quadruple.TryParsePartial("1.5xyz", NumberStyles.Float,
    CultureInfo.InvariantCulture, out var v, out int consumed);
// ok == true, v == 1.5, consumed == 3
```

### Conversions to and from newer .NET numeric types

```csharp
using UltimateOrb;

Quadruple q = Quadruple.Pi;

#if NET11_0_OR_GREATER
// BFloat16 round-trip:
BFloat16 bf = (BFloat16)q;
Quadruple qb = (Quadruple)bf;

// Decimal32/64/128 round-trip:
System.Numerics.Decimal128 d128 = (System.Numerics.Decimal128)q;
Quadruple qd = (Quadruple)d128;
#endif
```

### Cross-format conversion (all IEEE 754 binary interchange)

```csharp
// Parse a binary64 double into Quadruple with the NaN payload policy applied:
double d = -0.0;
Quadruple q = Quadruple.FromIeee754InterchangeBinary<double, ulong>(d);

// Narrow to binary32 with AwayFromZero:
float f = Quadruple.ToIeee754InterchangeBinaryNarrowing<float, uint>(q);
```

### GPU (ILGPU, no extra assembly)

```csharp
using ILGPU;
using ILGPU.Runtime;
using UltimateOrb;

var ctx = Context.CreateDefault();
using var dev = ctx.CreateCudaAccelerator();   // or OpenCL / CPU

var kernel = dev.LoadStreamKernel<ArrayView<Quadruple>>(AddKernel);
var buf = dev.Allocate1D<Quadruple>(n);
kernel((int)buf.Length, buf.View);
dev.Synchronize();

static void AddKernel(ArrayView<Quadruple> buf) {
    int i = Grid.IdxX;
    buf[i] = Quadruple.Add(buf[i], Quadruple.One);
}
```

---

## Experimental status

`Quadruple` is marked `[Experimental("UoWIP")]`, and that diagnostic **blocks compilation** in consuming projects by default. To acknowledge it, pick one of:

- **Per project (recommended).** Add the ID to `<NoWarn>` in your `.csproj`:

  ```xml
  <Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <TargetFramework>net8.0</TargetFramework>
      <NoWarn>$(NoWarn);UoWIP</NoWarn>
    </PropertyGroup>
  </Project>
  ```

- **Per region.** Wrap the code that uses `Quadruple`:

  ```csharp
  #pragma warning disable UoWIP
  // ... code using Quadruple ...
  #pragma warning restore UoWIP
  ```

The diagnostic ID is exactly `UoWIP` (the string passed to the attribute), not a `SYSLIB*` prefix. `<TreatWarningsAsErrors>` does not change this: both mechanisms suppress the diagnostic identically. Acknowledging the diagnostic does not change the runtime behaviour or the API surface of `Quadruple`.

---

## Requirements

- **.NET 7.0 or later** — the officially supported floor and the minimum target for the full `IBinaryFloatingPointIeee754<Quadruple>` surface. .NET 6.0 and older, and `netstandard2.1`, are **not officially supported**: some code may compile, but filling the gaps (notably `IBinaryFloatingPointIeee754<T>` and the generic-math APIs, absent before .NET 7) is the consumer's responsibility, and those configurations are not covered by the test matrix.
- **.NET 8** — adds `IUtf8SpanFormattable` and UTF-8 parsing/formatting.
- **.NET 11** — adds `NumberStyles.HexFloat` (still combined with `AllowHexSpecifier`), `BFloat16` conversion support, and `System.Numerics.Decimal32` / `Decimal64` / `Decimal128` conversion support. All are guarded by `#if NET11_0_OR_GREATER`.
- **Optional: ILGPU** — for GPU-compilable basic operations today; the full API surface follows once ILGPU 2.0 finalizes. No extra `UltimateOrb` assembly is required.
- **Test-only:** MPFR 4.2+ (reachable via P/Invoke or `Sdcb.Arithmetic.Mpfr`, with `DefaultPrecision`, `EMin`, `EMax` configured as shown in the testing section), an LLVM build with `fp128` support, a C/C++ toolchain with the CORE-MATH reference sources, and `BigInteger` for the `BigRational`+interval methodology.

No native dependencies are required for normal use, no `unsafe` code is required for the public API, and the library is **Native AOT compatible** — the math functions work in **zero-GC environments**, including **UEFI applications**.

**Implementation note — `UInt128` backing type.** On .NET 8.0+ the 128-bit storage is backed by `System.UInt128`. On .NET 7.0 the library prefers `UltimateOrb.UInt128` over `System.UInt128` (an implementation detail; the public API is the same either way).

**Experimental diagnostic.** The `UoWIP` diagnostic produced by `[Experimental("UoWIP")]` must be acknowledged before `Quadruple` can be referenced from a consuming project. See the "Experimental status" section above for the `.csproj` / `#pragma` demos.
