using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace UltimateOrb.Core.CodeAnalyzers.UltimateOrb.CodeAnalyzers {



    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ReplaceComparisonOperatorsAnalyzer : DiagnosticAnalyzer {

        // 定义一个诊断ID和消息
        public const string DiagnosticId = "ReplaceComparisonOperators";

        // You could use LocalizedString but it's a little more complicated for this sample
        private static readonly string Title = "Replace comparison operators to preferred ones";
        private static readonly string MessageFormat = "Replace {0} with {1}";
        private static readonly string Description = "Replace comparison operators to preferred ones.";
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }





        public override void Initialize(AnalysisContext context) {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.RegisterSyntaxNodeAction(
                AnalyzeNode,
                SyntaxKind.LessThanExpression,
                SyntaxKind.GreaterThanOrEqualExpression);
        }

        // 定义一个分析节点的方法
        private void AnalyzeNode(SyntaxNodeAnalysisContext context) {

            var binaryExpr = (BinaryExpressionSyntax)context.Node;

            var operatorToken = binaryExpr.OperatorToken;

            string newOperator;
            switch (operatorToken.Kind()) {
            case SyntaxKind.LessThanToken:
                newOperator = ">";
                break;
            case SyntaxKind.GreaterThanEqualsToken:
                newOperator = "<=";
                break;
            default:
                return;
            }

            var diagnostic = Diagnostic.Create(
                Rule,
                operatorToken.GetLocation(),
                DiagnosticId,
                operatorToken.Text,
                newOperator);
            
            context.ReportDiagnostic(diagnostic);
        }


    }

    [ExportCodeFixProvider(LanguageNames.CSharp)]
    public class ReplaceComparisonOperatorsFixProvider : CodeFixProvider {

        // 获取当前的诊断ID
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
        ImmutableArray.Create(ReplaceComparisonOperatorsAnalyzer.DiagnosticId);

        // 注册一个代码修复动作，根据诊断ID来生成一个新的表达式
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context) {
            // 获取当前的文档和语法根节点
            var document = context.Document;
            var root = await document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // 获取当前的诊断对象和位置
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // 获取当前的二元表达式节点
            var binaryExpr =
            root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf()
            .OfType<BinaryExpressionSyntax>().First();

            // 调用一个方法来生成一个新的表达式节点
            var newBinaryExpr = await ReplaceOperatorAsync(document, binaryExpr, context.CancellationToken).ConfigureAwait(false);

            // 用新的表达式节点替换原来的表达式节点，生成一个新的文档
            var newRoot = root.ReplaceNode(binaryExpr, newBinaryExpr);
            var newDocument = document.WithSyntaxRoot(newRoot);

            // 注册一个代码修复动作，用新的文档替换原来的文档，并提供一个标题和等级
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Replace operator",
                    createChangedDocument: c => Task.FromResult(newDocument),
                    equivalenceKey: "Replace operator"),
                diagnostic);
        }

        // 定义一个方法来生成一个新的表达式节点，根据诊断消息中的参数来确定要使用的新运算符
        private async Task<BinaryExpressionSyntax> ReplaceOperatorAsync(Document document, BinaryExpressionSyntax binaryExpr, CancellationToken cancellationToken) {
            /*
            // 获取当前的语义模型和诊断对象
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var diagnostics = semanticModel.GetDiagnostics(cancellationToken: cancellationToken);
            var diagnostics1 = binaryExpr.GetDiagnostics();
            var diagnostic = semanticModel.GetDiagnostics(binaryExpr.Span, cancellationToken).FirstOrDefault();

            // 获取诊断消息中的参数，即原来和新的运算符
            var args = new[] { "", "" };// diagnostic.Arguments;
            var oldOperator = (string)args[0];
            var newOperator = (string)args[1];
            */
            SyntaxToken newOperatorToken;
            SyntaxKind newSyntaxKind;
            switch (binaryExpr.OperatorToken.Kind()) {
            case SyntaxKind.LessThanToken:
                newSyntaxKind = SyntaxKind.GreaterThanExpression;
                newOperatorToken = SyntaxFactory.Token(SyntaxKind.GreaterThanToken);
                break;
            case SyntaxKind.GreaterThanEqualsToken:
                newSyntaxKind = SyntaxKind.LessThanOrEqualExpression;
                newOperatorToken = SyntaxFactory.Token(SyntaxKind.LessThanEqualsToken);
                break;
            default:
                return binaryExpr;
            }
            var left = binaryExpr.Right.WithTrailingTrivia(binaryExpr.Left.GetTrailingTrivia());
            var right = binaryExpr.Left.WithTrailingTrivia(binaryExpr.Right.GetTrailingTrivia());

            var newBinaryExpr = SyntaxFactory.BinaryExpression(
                newSyntaxKind,
                left,
                newOperatorToken.WithTriviaFrom(binaryExpr.OperatorToken),
                right);

            return newBinaryExpr;
        }
    }
}
