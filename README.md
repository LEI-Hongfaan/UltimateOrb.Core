# UltimateOrb Core Libraries

[![Join the chat at https://gitter.im/UltimateOrb-Working-Group/PublicMain](https://badges.gitter.im/UltimateOrb-Working-Group/PublicMain.svg)](https://gitter.im/UltimateOrb-Working-Group/PublicMain?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This repository contains the source code for building the UltimateOrb Core Libraries, along with related tools and unit tests.

For the previous generation of the libraries, see the **[v2 legacy version](https://github.com/LEI-Hongfaan/UltimateOrb.Core.v2)**.

## UltimateOrb.Core

[![Version](https://img.shields.io/nuget/vpre/UltimateOrb.Core.svg)](https://www.nuget.org/packages/UltimateOrb.Core)
[![NuGet download count](https://img.shields.io/nuget/dt/UltimateOrb.Core.svg)](https://www.nuget.org/packages/UltimateOrb.Core)

**UltimateOrb.Core** provides the foundational components of the UltimateOrb library suite. It extends and refines parts of the .NET Base Class Libraries to support advanced numeric and low‑level programming scenarios.

Work in progress:
- (U)Int256
- **Quadruple** — IEEE 754‑2019 `binary128` floating‑point (fully implemented; see the dedicated [Quadruple section](#ultimateorbquadruple--ieee-754-2019-binary128) below).
- IEEE 754 decimal floating‑point types

### What’s New

- **Unified codebase** — UltimateOrb.Int128 now shares the exact same codebase as UltimateOrb.Core.
- **Trimmable assemblies** — All libraries support trimming for AOT.
- **More hardware intrinsics** — Additional intrinsics are used to further accelerate numeric operations.

### Target frameworks

- Version **3.x.x** and later targets **.NET 8.0 or newer**.
- Version **4.x.x** targets **.NET 11.0 or newer**.

### BigRational

**BigRational** is the exact rational-arithmetic type in UltimateOrb.Core, built on top of `System.Numerics.BigInteger`. It represents a ratio of two arbitrarily large integers exactly, with no rounding at any step, and is the numerical backbone of the exact-rational and interval-arithmetic machinery used throughout the library — most notably the `BigRational` + interval-arithmetic test oracle that the [Quadruple](#ultimateorbquadruple--ieee-754-2019-binary128) library uses to prove correct rounding of arithmetic and algebraic functions.

Because the numerator and denominator are arbitrary-precision `BigInteger`s, a `BigRational` can represent:

- Any dyadic rational produced by IEEE 754 arithmetic **exactly** (every `binary16`/`binary32`/`binary64`/`binary128` value, and every finite sum, product, or quotient of them).
- Exact rational **bounds** on an irrational true value — the endpoints of an interval that is guaranteed to contain the true result.
- Exact rational **midpoints** between adjacent representable floating-point values, which is what a correct-rounding proof must decide against.

This makes `BigRational` a natural building block for:

- **Exact-arithmetic reference implementations and test oracles.**
- **Interval arithmetic** where the endpoints are rational bounds on an irrational true value; the correct rounding is proven when the two rounded endpoints coincide.
- **Exact comparisons** of a correctly-rounded floating-point result against its true value, or of two floating-point results against the exact real they both approximate.

Unlike the floating-point types, `BigRational` performs no rounding; every operation is exact (subject only to available memory). It composes naturally with `BigInteger` for the sign, numerator, and denominator, and with the rest of the UltimateOrb numeric suite.

## UltimateOrb.Quadruple — IEEE 754‑2019 `binary128`

[![Version](https://img.shields.io/nuget/vpre/UltimateOrb.Quadruple.svg)](https://www.nuget.org/packages/UltimateOrb.Quadruple)
[![NuGet download count](https://img.shields.io/nuget/dt/UltimateOrb.Quadruple.svg)](https://www.nuget.org/packages/UltimateOrb.Quadruple)

**UltimateOrb.Quadruple** is a managed, portable, and correctly‑rounded implementation of the IEEE 754‑2019 `binary128` interchange format for .NET 7 and later. It is a blittable `readonly struct` with the exact 128‑bit layout (1 sign bit, 15‑bit biased exponent, 112‑bit trailing significand) and implements `IBinaryFloatingPointIeee754<Quadruple>`, integrating fully with generic math and `System.Numerics`. It is **not** a wrapper around a native library, nor a double‑double emulation.

> **Experimental.** The type is marked `[Experimental("UoWIP")]`. Referencing it blocks compilation until you opt in via `<NoWarn>$(NoWarn);UoWIP</NoWarn>` in your `.csproj` or a `#pragma warning disable UoWIP` around the using code.

### Key capabilities

- **Correctly rounded basic arithmetic with all five IEEE 754 rounding modes** — `+`, `-`, `*`, `/`, `%`, `FusedMultiplyAdd`, `IEEERemainder`, `ScaleB`, `Round(x, 0, mode)`, and named equivalents accept explicit `FloatingPointRounding` or `MidpointRounding`.
- **Correctly rounded transcendentals** — `Sin`, `Cos`, `SinCos`, `Tan`, `Asin`, `Acos`, `Atan`, `Atan2`, `Exp`, `ExpM1`, `Log`, `Log2`, `Log10`, `Sqrt`, `Cbrt`, `ReciprocalSqrt`, and `Hypot` all support the same five modes, including `roundTiesToAway` (`MidpointRounding.AwayFromZero`). See the dedicated README for the full correct‑rounding status of every function.
- **Augmented arithmetic (§9.5)** — `AddAugmented`, `SubtractAugmented`, and `MultiplyAugmented` return `(Rounded, Delta)` tuples with full NaN propagation and subnormal‑underflow handling.
- **Best‑in‑class parsing and formatting** — exact round‑half‑to‑even at every precision, minimal shortest round‑trip (`G36`), full standard and custom specifier language, complete `NumberFormatInfo`/`NumberStyles` respect, hex‑float parsing, NaN‑payload parsing with explicit class markers (`NaN(S1)` for signaling, `NaN(Q42)` for quiet), UTF‑8 and UTF‑16 paths, and public `TryParsePartial`.
- **NaN‑payload control** — three configurable modes plus `GetQuietNaNWithPayload` / `GetSignalingNaNWithPayload` / `GetPayload`.
- **Cross‑format interchange** — `FromIeee754InterchangeBinary` and `ToIeee754InterchangeBinaryNarrowing` cover binary16, binary32, binary64, binary128, and binary160/192/224/256.
- **Conversions** — to and from `double`, `float`, `Half`, `decimal`, `BigInteger`, standard integers, `UInt128`/`Int128`, and (on .NET 11+) `BFloat16` and `System.Numerics.Decimal32/64/128`, all correctly rounded.
- **Zero‑GC numeric core and Native AOT compatibility** — every arithmetic and transcendental function performs no heap allocations; the library is Native AOT compatible and works in zero‑GC environments such as UEFI applications.
- **GPU‑compilable via ILGPU** — the basic operators, FMA, comparisons, `Abs`/`CopySign`/`BitIncrement`/`BitDecrement`, and construction/conversion are already compilable to CUDA, OpenCL, and ILGPU‑CPU kernels today; the full API surface becomes GPU‑compilable automatically once ILGPU 2.0 supports static tables and spans.
- **Validated against four reference implementations** — MPFR (via direct P/Invoke or `Sdcb.Arithmetic.Mpfr`), `BigRational` + interval arithmetic, LLVM JIT‑compiled `fp128` stubs, and CORE‑MATH (including its adversarial input generators). The dedicated README documents the full test‑status matrix and the coverage of `AwayFromZero`.

### Requirements at a glance

| Requirement | Detail |
|---|---|
| **.NET 7.0+** | Official floor; full `IBinaryFloatingPointIeee754<T>` surface. |
| **.NET 8** | Adds `IUtf8SpanFormattable` and UTF‑8 parsing/formatting. |
| **.NET 11** | Adds `NumberStyles.HexFloat`, `BFloat16`, and `Decimal32/64/128` conversions (guarded by `#if NET11_0_OR_GREATER`). |
| **Optional** | ILGPU for GPU‑compilable basic operations. No extra UltimateOrb assembly is needed. |
| **No native dependencies** | Normal use requires no native linkage, and **consumers do not need to write or enable `unsafe` code** to use the library. |

### Quick example

```csharp
#pragma warning disable UoWIP

using System.Globalization;
using UltimateOrb;

// Sin: nearest modes agree; directed modes differ.
Quadruple x = Quadruple.One; // or (Quadruple)1
Console.WriteLine(Quadruple.Sin(x, MidpointRounding.ToEven) ==
                  Quadruple.Sin(x, MidpointRounding.AwayFromZero)); // True
Console.WriteLine(Quadruple.Sin(x, MidpointRounding.ToPositiveInfinity) !=
                  Quadruple.Sin(x, MidpointRounding.ToNegativeInfinity)); // True

// Hypot: an exact midpoint makes ToEven and AwayFromZero differ.
Quadruple a = Quadruple.ScaleB(Quadruple.Parse("6853831853265972805019800674599097"), -113);
Quadruple b = Quadruple.ScaleB(Quadruple.Parse("9138442471021296276733058500941296"), -113);

Quadruple hEven = Quadruple.Hypot(a, b, MidpointRounding.ToEven);
Quadruple hAway = Quadruple.Hypot(a, b, MidpointRounding.AwayFromZero);
Console.WriteLine(hEven != hAway); // True

var (r, d) = Quadruple.AddAugmented((Quadruple)1e200, (Quadruple)1e-200);
// r + d == the exact sum, with d representable in binary128

string roundTrip = x.ToString("G36");   // minimal shortest round-trip

// Hex-float parsing — portable syntax on .NET 7 through .NET 10.
// NumberStyles.HexFloat does not itself include AllowHexSpecifier, so the
// flag must be OR-ed in explicitly on .NET 11+ too.
#if NET11_0_OR_GREATER
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexFloat | NumberStyles.AllowHexSpecifier);
#else
Quadruple hexPi = Quadruple.Parse(
    "0x1.921fb54442d16p+1",
    NumberStyles.HexNumber | NumberStyles.Float | NumberStyles.AllowHexSpecifier);
#endif

// NaN payload parsing — class marker inside the parentheses:
Quadruple signaling = Quadruple.Parse("NaN(S1)",  NumberStyles.Float);
Quadruple quiet     = Quadruple.Parse("NaN(Q42)", NumberStyles.Float);
```

> **Constructing a `Hypot` case where `ToEven` and `AwayFromZero` actually differ.** The exact result must land exactly on a midpoint between two adjacent `binary128` values. Since `Hypot(a, b)` is exactly `√(a² + b²)` and both `a` and `b` are dyadic rationals, the exact result is a midpoint only when `a² + b²` is the square of a dyadic odd multiple of half an ulp — a condition on Pythagorean-like triples that is genuinely rare. The library's test suite exercises such cases via `BigRational` + interval arithmetic rather than advertising a specific hand-picked input; the `(3, 4)` example above deliberately does **not** claim a difference, because `hypot(3, 4) = 5` exactly.

### Documentation

The detailed Quadruple README lives at [`doc/UltimateOrb.Quadruple/README.md`](doc/UltimateOrb.Quadruple/README.md) and covers the complete correct‑rounding status catalog, augmented‑arithmetic contract, parsing/formatting tables, ILGPU usage, and the four‑oracle testing methodology in detail.

## UltimateOrb.Int128

[![Version](https://img.shields.io/nuget/vpre/UltimateOrb.Int128.svg)](https://www.nuget.org/packages/UltimateOrb.Int128)
[![NuGet download count](https://img.shields.io/nuget/dt/UltimateOrb.Int128.svg)](https://www.nuget.org/packages/UltimateOrb.Int128)

This standalone library offers implementations of **Int128** and **UInt128**.

- Version **2.1.x** targets **.NET 6.0**
- Version **3.x.x** targets **.NET 6.0 or newer**

### UltimateOrb.XIntN

Planned future name for the extended integer library.

Planned features:
- Additional large integer types
- Optimized execution paths for browser/WASM environments
- Compile‑time configurable byte order for fixed‑size integer types

## License

UltimateOrb.Core and UltimateOrb.Int128 are licensed under the [MIT license](LICENSE).

Some portions of the UltimateOrb Core Libraries include source code from other open-source projects. See [Third party notices](THIRD-PARTY-NOTICES.TXT) for details.
