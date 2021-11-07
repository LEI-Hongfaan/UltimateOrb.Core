// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
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

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UltimateOrb.Runtime.Caching;

namespace UltimateOrb.Reflection.Traits {

    internal static class ContainsReferenceTypes {

        static readonly KeyedCache<Type, bool> cache = new KeyedCache<Type, bool>(31, type => (bool)typeof(RuntimeHelpers).GetMethod(nameof(RuntimeHelpers.IsReferenceOrContainsReferences), 1, Type.EmptyTypes)!.MakeGenericMethod(type).Invoke(null, Array.Empty<object>())!);

        internal static bool IsReferenceOrContainsReferences(Type type) {
            return cache[type];
        }

        internal static bool GetValue(Type type) {
            return (bool)typeof(ContainsReferenceTypes<>).MakeGenericType(type).GetField(nameof(ContainsReferenceTypes<object>.Value))!.GetValue(null)!;
        }

        internal static bool GetValueInternal(Type type) {
            if (type.IsValueType) {
                return IsReferenceOrContainsReferences(type);
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = 0; i < fields.Length; i++) {
                Type fieldType = fields[i].FieldType;
                if (IsReferenceOrContainsReferences(fieldType)) {
                    return true;
                }
            }

            Type? baseType = type.BaseType;
            return null != baseType && GetValue(baseType);
        }
    }

    public static class ContainsReferenceTypes<T> {

        public static readonly bool Value = ContainsReferenceTypes.GetValueInternal(typeof(T));
    }
}
