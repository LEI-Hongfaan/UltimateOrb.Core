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

namespace System.Runtime.CompilerServices {

    /// <summary>
    /// Allows the current assembly to access the internal types of a specified assembly that are ordinarily invisible.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
#if STANDALONE_XINTN_LIBRARY
    internal
#else
    public
#endif
        sealed class IgnoresAccessChecksToAttribute : Attribute {

        /// <summary>
        /// Initializes a new instance of the <see cref="IgnoresAccessChecksToAttribute" /> class with the name of the specified assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the specified assembly.</param>
        public IgnoresAccessChecksToAttribute(string assemblyName) {
            AssemblyName = assemblyName;
        }

        /// <summary>
        /// Gets the name of the specified assembly whose access checks against the current assembly are ignored .
        /// </summary>
        /// <value>A string that represents the name of the specified assembly.</value>
        public string AssemblyName { get; }
    }
}
