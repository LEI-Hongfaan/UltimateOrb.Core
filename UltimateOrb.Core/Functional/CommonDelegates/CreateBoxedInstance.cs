// ==++==
//
//   Copyright (c) LEI Hongfaan.  All rights reserved.  Licensed under the MIT License.
//
// ==--==
// ==++==
//
//   MIT License
//
//   Copyright (c) 2019 LEI Hongfaan
//
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//   of this software and associated documentation files (the "Software"), to deal
//   in the Software without restriction, including without limitation the rights
//   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//   copies of the Software, and to permit persons to whom the Software is
//   furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in all
//   copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//   SOFTWARE.
//
// ==--==
#nullable enable

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Functional.CommonDelegates {

    public static partial class CreateBoxedInstance {

        static LambdaExpression GetExpressionInternalCore(Type type) {
            var createInstance = CreateInstance.GetExpressionInternal(type);
            var boxType = typeof(StrongBox<>).MakeGenericType(type);
            var expression = createInstance.Body;
            Debug.Assert(expression is NewExpression);
            return Expression.Lambda(typeof(Func<>).MakeGenericType(boxType), Expression.New(boxType.GetConstructor(new[] { type }), expression), createInstance.Parameters);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        static Delegate GetValueInternalCore(Type type) {
            var lambda = GetExpressionInternalCore(type);
            return lambda.Compile();
        }
    }
}