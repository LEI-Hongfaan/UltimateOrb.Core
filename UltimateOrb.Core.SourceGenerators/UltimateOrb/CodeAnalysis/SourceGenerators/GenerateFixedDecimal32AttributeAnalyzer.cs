using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using UltimateOrb.CodeAnalysis.SourceGenerators;

namespace UltimateOrb.CodeAnalysis.Analyzers {

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class GenerateFixedDecimal32AttributeAnalyzer : DiagnosticAnalyzer {

        public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            id: "CA001",
            title: "GenerateFixedDecimal32Attribute parameter must be non-negative",
            messageFormat: "The parameter value of GenerateFixedDecimal32Attribute is {0}, which is negative",
            category: "Usage",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context) {
            context.RegisterSyntaxNodeAction(AnalyzeSyntaxNode, SyntaxKind.StructDeclaration);
        }

        private void AnalyzeSyntaxNode(SyntaxNodeAnalysisContext context) {
            var structDeclaration = (StructDeclarationSyntax)context.Node;
            var semanticModel = context.SemanticModel;
            var attributeSymbol = semanticModel.Compilation.GetTypeByMetadataName(FixedDecimal32Generator.AttributeMetadataName);
            if (attributeSymbol == null) {
                return;
            }
            if (structDeclaration.Modifiers.Any(m => m.IsKind(SyntaxKind.PartialKeyword)) &&
                structDeclaration.AttributeLists.Any(a => a.Attributes.Any(at => attributeSymbol.Equals(semanticModel.GetTypeInfo(at).Type, SymbolEqualityComparer.Default)))) {
                var attributeSyntax = structDeclaration.AttributeLists.SelectMany(a => a.Attributes).First(at => attributeSymbol.Equals(semanticModel.GetTypeInfo(at).Type, SymbolEqualityComparer.Default));
                var argumentList = attributeSyntax.ArgumentList;
                if (argumentList == null || argumentList.Arguments.Count == 0) {
                    return;
                }
                var argumentExpression = argumentList.Arguments[0].Expression;
                var constantValue = semanticModel.GetConstantValue(argumentExpression);
                if (constantValue.HasValue && constantValue.Value is int value) {
                    if (value < 0) {
                        var diagnostic = Diagnostic.Create(Rule, argumentExpression.GetLocation(), value);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
