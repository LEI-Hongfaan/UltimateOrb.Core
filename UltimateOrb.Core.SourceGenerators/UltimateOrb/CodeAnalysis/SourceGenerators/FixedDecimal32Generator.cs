using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Linq;
using System.Text;
using System;


namespace UltimateOrb.CodeAnalysis.SourceGenerators {
    [Generator]
    public class FixedDecimal32Generator : ISourceGenerator {
        private const string attributeText = @"using System;

namespace UltimateOrb.Numerics.Specialized {

    [AttributeUsage(AttributeTargets.Struct)]
    public class GenerateFixedDecimal32Attribute : Attribute {

        public int ExponentBias { get; }

        public GenerateFixedDecimal32Attribute(int exponentBias) {
            ExponentBias = exponentBias;
        }
    }
}
";
        
        public void Initialize(GeneratorInitializationContext context) {

            context.RegisterForPostInitialization(postInitContext => postInitContext.AddSource("GenerateFixedDecimal32Attribute.cs", attributeText));

        }


        internal const string AttributeMetadataName = "UltimateOrb.Numerics.Specialized.GenerateFixedDecimal32Attribute";

        int? GetAttributeValue(StructDeclarationSyntax structDeclaration, INamedTypeSymbol attributeSymbol, SemanticModel semanticModel) {
            // 查找GenerateXXXAttribute特性
            var attributeSyntax = structDeclaration.AttributeLists.SelectMany(a => a.Attributes).First(at => semanticModel.GetTypeInfo(at).Type.Equals(attributeSymbol, SymbolEqualityComparer.Default));

            var argumentList = attributeSyntax.ArgumentList;

            if (argumentList == null || argumentList.Arguments.Count == 0) {
                return null;
            }

            var argumentExpression = argumentList.Arguments[0].Expression;

            var constantValue = semanticModel.GetConstantValue(argumentExpression);
            if (constantValue.HasValue && constantValue.Value is int value) {
                return value;
            } else {
                return null;
            }
        }

        public void Execute(GeneratorExecutionContext context) {
            // 获取GenerateXXXAttribute的符号
            var attributeSymbol = context.Compilation.GetTypeByMetadataName(AttributeMetadataName);
            if (attributeSymbol == null) {
                return;
            }

            // 遍历所有语法树
            foreach (var syntaxTree in context.Compilation.SyntaxTrees) {
                // 获取语义模型
                var semanticModel = context.Compilation.GetSemanticModel(syntaxTree);

                // 查找所有被GenerateXXXAttribute标记的partial struct声明
                var structDeclarations = syntaxTree.GetRoot().DescendantNodes()
                .OfType<StructDeclarationSyntax>()
                .Where(s => s.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)))
                .Where(s => s.AttributeLists.Any(a => a.Attributes.Any(at => attributeSymbol.Equals(semanticModel.GetTypeInfo(at).Type, SymbolEqualityComparer.Default))));

                // 为每个struct声明生成代码
                foreach (var structDeclaration in structDeclarations) {
                    var structSymbol = semanticModel.GetDeclaredSymbol(structDeclaration);
                    var isReadOnly = structSymbol!.IsReadOnly;
                    var structName = structSymbol.Name;
                    var namespaceName = structSymbol.ContainingNamespace.ToDisplayString();
                    var attributeValue = GetAttributeValue(structDeclaration, attributeSymbol, semanticModel);

                    if (attributeValue is null) {
                        continue;
                    }

                    var accessModifiers = structSymbol.DeclaredAccessibility switch {
                        Accessibility.Internal => "internal ",
                        Accessibility.Private => "private ",
                        Accessibility.Protected => "protected ",
                        Accessibility.ProtectedAndInternal => "private protected ",
                        Accessibility.ProtectedOrInternal => "protected internal ",
                        Accessibility.Public => "public ",
                        _ => "",
                    };
                    
                    var source = $$$"""""
#nullable enable

using System;
using System.Runtime.CompilerServices;

namespace {{{namespaceName}}} {

    {{{accessModifiers}}}readonly partial struct {{{structName}}}
        : IEquatable<{{{structName}}}>, IComparable<{{{structName}}}> {

        public readonly Int32 Bits;

        const Int32 NaNBits = Int32.MinValue;

        const Int32 NegativeInfinityBits = 1 + Int32.MinValue;

        const Int32 PositiveInfinityBits = Int32.MaxValue;

        const int _Scale = {{{checked((int)Math.Round(Math.Pow(10, -attributeValue.Value)))}}};
        const double _ScaleD = _Scale;

        public static {{{structName}}} NaN {

            get => new(NaNBits);
        }

        public static {{{structName}}} NegativeInfinity {

            get => new(NegativeInfinityBits);
        }

        public static {{{structName}}} PositiveInfinity {

            get => new(PositiveInfinityBits);
        }

        public static {{{structName}}} MaxValue {

            get => new(PositiveInfinityBits - 1);
        }

        public static {{{structName}}} MinValue {

            get => new(NegativeInfinityBits + 1);
        }

        public double Value {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Bits switch {
                NaNBits => double.NaN,
                NegativeInfinityBits => double.NegativeInfinity,
                PositiveInfinityBits => double.PositiveInfinity,
                _ => Bits / _ScaleD,
            };
        }

        public double MilliValue {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Bits switch {
                NaNBits => double.NaN,
                NegativeInfinityBits => double.NegativeInfinity,
                PositiveInfinityBits => double.PositiveInfinity,
                _ => Bits,
            };
        }

        {{{structName}}}(Int32 bits) {
            Bits = bits;
        }

        public static {{{structName}}} FromBits(Int32 bits) => new {{{structName}}}(bits);

        public override bool Equals(object? obj) {
            return obj is {{{structName}}} milli && this.Equals(milli);
        }

        public bool Equals({{{structName}}} other) {
            return this.Bits == other.Bits;
        }

        public override int GetHashCode() {
            return Bits;
        }

        public static implicit operator double({{{structName}}} value) {
            return value.Value;
        }

        public static explicit operator {{{structName}}}(double value) {
            var v = Math.Round(value * _Scale);
            if (v <= PositiveInfinityBits) {
                if (NegativeInfinityBits <= v) {
                    return new {{{structName}}}(unchecked((Int32)v));
                } else {
                    return new {{{structName}}}(NegativeInfinityBits);
                }
            } else {
                if (v > PositiveInfinityBits) {
                    return new {{{structName}}}(PositiveInfinityBits);
                } else {
                    return new {{{structName}}}(int.MinValue);
                }
            }
        }

        public static {{{structName}}} operator +({{{structName}}} value) {
            return value;
        }

        public static {{{structName}}} operator -({{{structName}}} value) {
            return new {{{structName}}}(unchecked(-value.Bits));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator +({{{structName}}} first, {{{structName}}} second) {
            if (NaNBits != first.Bits && NaNBits != second.Bits) {
                var inf = 0X0000100000000000;
                long x;
                long y;
                x = PositiveInfinityBits == first.Bits ? inf : NegativeInfinityBits == first.Bits ? -inf : first.Bits;
                y = PositiveInfinityBits == second.Bits ? inf : NegativeInfinityBits == second.Bits ? -inf : second.Bits;
                var r = unchecked(x + y);
                return (0 == r && x != first.Bits) ? NaN : new {{{structName}}}(unchecked((Int32)(r > PositiveInfinityBits ? PositiveInfinityBits : r < NegativeInfinityBits ? NegativeInfinityBits : r)));
            }
            return NaN;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite({{{structName}}} value) {
            return unchecked(1 + value.Bits) > 1 + NegativeInfinityBits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaN({{{structName}}} value) {
            return NaNBits == value.Bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator -({{{structName}}} first, {{{structName}}} second) {
            if (NaNBits != first.Bits && NaNBits != second.Bits) {
                var inf = 0X0000100000000000;
                long x;
                long y;
                x = PositiveInfinityBits == first.Bits ? inf : NegativeInfinityBits == first.Bits ? -inf : first.Bits;
                y = PositiveInfinityBits == second.Bits ? inf : NegativeInfinityBits == second.Bits ? -inf : second.Bits;
                var r = unchecked(x - y);
                return (0 == r && x != first.Bits) ? NaN : new {{{structName}}}(unchecked((Int32)(r > PositiveInfinityBits ? PositiveInfinityBits : r < NegativeInfinityBits ? NegativeInfinityBits : r)));
            }
            return NaN;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator *({{{structName}}} first, {{{structName}}} second) {
            return ({{{structName}}})(first.Value * second.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator *(double first, {{{structName}}} second) {
            return ({{{structName}}})(first * second.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator *({{{structName}}} first, double second) {
            return ({{{structName}}})(first.Value * second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator /({{{structName}}} first, {{{structName}}} second) {
            return 0 == second.Bits ? NaN : ({{{structName}}})(first.Value / second.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} operator /({{{structName}}} first, double second) {
            return ({{{structName}}})(first.Value / second);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static {{{structName}}} op_Multiply_A_1({{{structName}}} first, {{{structName}}} second) {
            if (NaNBits != first.Bits && NaNBits != second.Bits) {
                long x = first.Bits;
                long y = second.Bits;
                var p = unchecked(x * y);
                var (q, r) = Math.DivRem(p, _Scale);
                q += unchecked((long)Math.Round((int)r / (_ScaleD / 2)));

                return (0 == p && (!IsFinite(first) || !IsFinite(second))) ? NaN : new {{{structName}}}(unchecked((Int32)(q > PositiveInfinityBits ? PositiveInfinityBits : q < NegativeInfinityBits ? NegativeInfinityBits : q)));
            }
            return NaN;
        }

        public int CompareTo({{{structName}}} other) {
            return Bits.CompareTo(other.Bits);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==({{{structName}}} left, {{{structName}}} right) {
            return NaNBits != left.Bits && left.Bits == right.Bits;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=({{{structName}}} left, {{{structName}}} right) {
            return !(left == right);
        }

        public static bool operator <({{{structName}}} left, {{{structName}}} right) {
            return left.MilliValue < right.MilliValue;
        }

        public static bool operator <=({{{structName}}} left, {{{structName}}} right) {
            return left.MilliValue <= right.MilliValue;
        }

        public static bool operator >({{{structName}}} left, {{{structName}}} right) {
            return left.MilliValue > right.MilliValue;
        }

        public static bool operator >=({{{structName}}} left, {{{structName}}} right) {
            return left.MilliValue >= right.MilliValue;
        }
    }
}
""""";
                    context.AddSource($"{structName}.generated.cs", SourceText.From(source, Encoding.UTF8));
                }
            }
        }
    }
}
