
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
- Quadruple‑precision floating‑point (IEEE 754 binary256)
- IEEE 754 decimal floating‑point types

### What’s New

- **Unified codebase** — UltimateOrb.Int128 now shares the exact same codebase as UltimateOrb.Core.
- **Trimmable assemblies** — All libraries support trimming for AOT.
- **More hardware intrinsics** — Additional intrinsics are used to further accelerate numeric operations.

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

Some portions of the UltimateOrb Core Libraries include source code from
[.NET Runtime](https://github.com/dotnet/runtime) and
[ASP.NET Core](https://github.com/dotnet/aspnetcore).
See [Third party notices](THIRD-PARTY-NOTICES.TXT) for details.
