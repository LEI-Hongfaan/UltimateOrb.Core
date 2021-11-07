/*
MIT License
Copyright (c) 2020 LEI Hongfaan
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using global::System;
using global::System.Runtime.CompilerServices;
using global::System.Runtime.Versioning;

namespace System {

    // ByReference<T> is meant to be used to represent "ref T" fields. It is working
    // around lack of first class support for byref fields in C# and IL. The JIT and
    // type loader has special handling for it that turns it into a thin wrapper around ref T.
    [UltimateOrb.ForceDiscard]
    internal readonly ref struct ByReference<T> {
#pragma warning disable CA1823, 169 // private field '{blah}' is never used
        private readonly IntPtr _value;
#pragma warning restore CA1823, 169

        public ByReference(ref T value) {
            // Implemented as a JIT intrinsic - This default implementation is for
            // completeness and to provide a concrete error if called via reflection
            // or if intrinsic is missed.
            throw new PlatformNotSupportedException();
        }

#pragma warning disable CA1822 // Mark members as static
        public ref T Value {
            // Implemented as a JIT intrinsic - This default implementation is for
            // completeness and to provide a concrete error if called via reflection
            // or if the intrinsic is missed.
            get => throw new PlatformNotSupportedException();
        }
#pragma warning restore CA1822
    }
}
