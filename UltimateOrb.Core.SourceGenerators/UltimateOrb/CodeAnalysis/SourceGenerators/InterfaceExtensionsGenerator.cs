using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Internal {

    delegate void RefAction<T>(ref T arg);

    delegate TResult RefFunc<T, TResult>(ref T arg);

    static partial class Extensions {

        public static void PipeTo<T>(this T source, Action<T> selector) {
            selector.Invoke(source);
        }

        public static TResult PipeTo<T, TResult>(this T source, Func<T, TResult> selector) {
            return selector.Invoke(source);
        }

        public static void PipeTo<T>(this ref T source, RefAction<T> selector) where T : struct {
            selector.Invoke(ref source);
        }

        public static TResult PipeTo<T, TResult>(this ref T source, RefFunc<T, TResult> selector) where T : struct {
            return selector.Invoke(ref source);
        }
    }
}

namespace UltimateOrb.CodeAnalysis.SourceGenerators {
    /*
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional("InterfaceExtensionsGenerator_DEBUG")]
    public class GeneratExtensionsAttribute : Attribute {

        public GeneratExtensionsAttribute() {
        }

        public readonly string? Namespace;
    }
    */

    [Generator]
    public class InterfaceExtensionsGenerator : ISourceGenerator {

        private const string attributeText = @"
using System;

namespace UltimateOrb.CodeAnalysis.SourceGenerators {

    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional(""InterfaceExtensionsGenerator_DEBUG"")]
    public class GeneratExtensionsAttribute : Attribute {

        public GeneratExtensionsAttribute() {
        }

        public readonly string? Namespace;
    }
}
";

        public void Initialize(GeneratorInitializationContext context) {

            context.RegisterForPostInitialization(postInitContext => postInitContext.AddSource("GeneratExtensionsAttribute", attributeText));

            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context) {
            if (context.SyntaxContextReceiver is not SyntaxReceiver receiver) {
                return;
            }

            var attributeSymbol = context.Compilation.GetTypeByMetadataName("UltimateOrb.CodeAnalysis.SourceGenerators.GeneratExtensionsAttribute");

            foreach (var group in receiver.Members.GroupBy(m => m.Parent)) {
                var interf = group.Key!;
                var sm = context.Compilation.GetSemanticModel(interf.SyntaxTree);


            }
        }

        /// <summary>
        /// Created on demand before each generation pass
        /// </summary>
        class SyntaxReceiver : ISyntaxContextReceiver {

            public List<MemberDeclarationSyntax> Members { get; } = new List<MemberDeclarationSyntax>();




            /// <summary>
            /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
            /// </summary>
            public void OnVisitSyntaxNode(GeneratorSyntaxContext context) {
                var sm = context.SemanticModel;

                // any field with at least one attribute is a candidate for property generation
                if (context.Node is TypeDeclarationSyntax type) {
                    var asds = type.AttributeLists.SelectMany(x => x.Attributes, (xs, y) => y).Where(x => {
                        var t = sm.GetTypeInfo(x);
                        var attributeType = t.Type;
                        var fullName = attributeType.ToString();
                        if (fullName != "UltimateOrb.CodeAnalysis.SourceGenerators.GeneratExtensionsAttribute") {
                            return false;
                        }
                        return true;
                    });
                    try {
                        var attr = asds.SingleOrDefault();


                        var attr_ns = attr?.ArgumentList?.Arguments.Where(x => x.NameEquals?.ToString() == "Namespace" || x.NameColon?.ToString() == "namespace").SingleOrDefault()?.Expression;

                        var interfaceSym = sm.GetDeclaredSymbol(type)!;
                        var namespaceFullName = interfaceSym.ContainingNamespace?.MetadataName;
                        var interfaceName = interfaceSym.Name;
                        
                        var asfa = GetBaseName(interfaceName);

                        var interfaceFullName = interfaceSym.ToString();


                        var ns = attr_ns == null ? namespaceFullName : sm.GetConstantValue(attr_ns);

                        var sb = new StringBuilder();
                        sb.Append($@"using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace {namespaceFullName} {{

    public static partial class {interfaceName}Extensions {{
");
                        foreach (var member in type.Members) {
                            var isStatic = member.Modifiers.Any(x => x.IsKind(SyntaxKind.StaticKeyword));
                            if (isStatic) {
                                continue;
                            }
                            if (member is MethodDeclarationSyntax method) {
                                var isVoidReturn = method.ReturnType.IsKind(SyntaxKind.VoidKeyword);

                                ParameterSyntax d = Parameter(default, TokenList(Token(SyntaxKind.ThisKeyword)), default, Identifier("@this"), null);

                                method.WithParameterList(PrependParameter(method.ParameterList, d));
                                // var aaa = InvocationExpression(d.Identifier, ArgumentList(method.ParameterList.Parameters.Select(x => )));
                                // method.WithExpressionBody(ArrowExpressionClause(aaa));
                            } else if (member is PropertyDeclarationSyntax property) {

                            } else if (member is IndexerDeclarationSyntax indexer) {

                            }
                        }
                        sb.Append($@"
    }}
}}
");
                        var saf = sb.ToString();
                    } catch (Exception ex) {
                    }

                }
            }

            private static ParameterListSyntax PrependParameter(ParameterListSyntax parameterList, ParameterSyntax parameter) {
                return parameterList.WithParameters(parameterList.Parameters.Insert(0, parameter));
            }

            private static string? GetBaseName(string? interfaceName) {
                if (!string.IsNullOrEmpty(interfaceName) && interfaceName.Length > 1) {
                    var a = interfaceName[0];
                    var b = interfaceName[0];
                    if (a == 'I' && char.IsUpper(b)) {
                        return interfaceName[1..];
                    }
                }
                return interfaceName;
            }
        }
    }
}
