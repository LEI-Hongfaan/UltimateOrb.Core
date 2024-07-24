
# UltimateOrb Core Libraries

[![Join the chat at https://gitter.im/UltimateOrb-Working-Group/PublicMain](https://badges.gitter.im/UltimateOrb-Working-Group/PublicMain.svg)](https://gitter.im/UltimateOrb-Working-Group/PublicMain?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

This repo contains the code to build the UltimateOrb Core libraries, as well as the sources to related tools and unit tests.

[Old version v2](https://github.com/LEI-Hongfaan/UltimateOrb.Core.v2)

## UltimateOrb.Core

[![Version](https://img.shields.io/nuget/vpre/UltimateOrb.Core.svg)](https://www.nuget.org/packages/UltimateOrb.Core)
[![NuGet download count](https://img.shields.io/nuget/dt/UltimateOrb.Core.svg)](https://www.nuget.org/packages/UltimateOrb.Core)

This is the Core part of the UltimateOrb Libraries' adjustment of the Base Class Libraries.

## UltimateOrb.Int128

[![Version](https://img.shields.io/nuget/vpre/UltimateOrb.Int128.svg)](https://www.nuget.org/packages/UltimateOrb.Int128)
[![NuGet download count](https://img.shields.io/nuget/dt/UltimateOrb.Int128.svg)](https://www.nuget.org/packages/UltimateOrb.Int128)

This independent library provides Int128 and UInt128.

Version 2.1.x is built against .NET 6.0.
Version 3.x.x is built against .NET 7.0.

[WIP] Support for Generic Math interfaces.

### What's new ###

* Now UltimateOrb.Int128 uses exactly the same codebase as UltimateOrb.Core. 
* The library is trimmed by ILLink.
* More intrinsics are ultilized to accelate the computations.

### UltimateOrb.XIntN ###
The future name of this library.

Plans:
* It will contains other bigger integer types.
* Special code paths to perform better on Browser (WASM).
* ~Change parameter orders from "least significant bits"-to-"most significant bits" to msb-to-lsb.~
* The structure layout byte orders of fixed-size integer types will be compile-time configurable.

## License

UltimateOrb.Core and UltimateOrb.Int128 are licensed under the [MIT license](LICENSE).

Some portions of UltimateOrb Core Libraries use source code form [.NET Runtime](https://github.com/dotnet/runtime), [ASP.NET Core](https://github.com/dotnet/aspnetcore) and [ASP.NET Core](https://github.com/dotnet/aspnetcore). See [Third party notices](THIRD-PARTY-NOTICES.TXT).
