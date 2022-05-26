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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UltimateOrb.Functional.CommonDelegates {

    public static partial class CreateInstance {

        static LambdaExpression GetExpressionInternalCore(Type type) {
            var ctors = type.GetConstructors();
            ConstructorInfo? paramlessCtor = null;
            ConstructorInfo? nonreqparamCtor = null;
            int foundNonreqparamCtor = 0;
            var v = default(int);
            ParameterInfo[]? ps = null;
            foreach (var ctor in ctors) {
                var parameters = ctor.GetParameters();
                var length = parameters.Length;
                if (0 == length) {
                    paramlessCtor = ctor;
                    break;
                }
                if (parameters.All(p => p.IsOptional)) {
                    ++foundNonreqparamCtor;
                    if (null == nonreqparamCtor) {
                        nonreqparamCtor = ctor;
                        v = length;
                        ps = parameters;
                    } else {
                        nonreqparamCtor = null;
                        break;
                    }
                }
            }
            ConstructorInfo? constructor = null;
            if (null != paramlessCtor) {
                constructor = paramlessCtor;
            } else if (null != nonreqparamCtor) {
                constructor = nonreqparamCtor;
            } else if (type.IsValueType) {
            } else {
                if (foundNonreqparamCtor > 1) {
                    throw new ArgumentException($@"Type ""{type.FullName}"" has more than one constructor without required parameters. Ambiguous match found.", nameof(type));
                }
                throw new ArgumentException($@"Type ""{type.FullName}"" does not have a constructor without required parameters.", nameof(type));
            }
            LambdaExpression lambda;
            {
                var lambdaDelegateType = typeof(Func<>).MakeGenericType(type);
                var lambdaParameters = Array.Empty<ParameterExpression>();
                Expression lambdaBody;
                if (null == constructor) {
                    lambdaBody = Expression.New(type);
                } else {
                    if (null == ps) {
                        lambdaBody = Expression.New(constructor);
                    } else {
                        var parameters = ps;
                        lambdaBody = Expression.New(constructor, parameters.Select(p => Expression.Constant(p.DefaultValue)));
                    }
                }
                lambda = Expression.Lambda(lambdaDelegateType, lambdaBody, lambdaParameters);
            }
            return lambda;
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        static Delegate GetValueInternalCore(Type type) {
            var lambda = GetExpressionInternal(type);
            return lambda.Compile();
        }
    }
}