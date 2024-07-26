using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateOrb.Core.SourceGenerators.UltimateOrb.Numerics.Generic {

    public class OperationDescriptor {
        public string OperatorName { get; set; }
        public string Kind { get; set; }
        public List<(string Modifier, string Type, string Name)>? Parameters { get; set; }
        public List<string> ChecknessVariants { get; set; }
    }

    [Generator]
    public class BinaryIntegerInterfaceGenerator : IIncrementalGenerator {
        public void Initialize(IncrementalGeneratorInitializationContext context) {
            var assemblyAttributes = context.SyntaxProvider
                .ForAttributeWithMetadataName("Ultimate.Core.SourceGenerators.GenerateUltimateOrbCoreSourceCodeAttribute",
                    predicate: (node, _) => node is AttributeSyntax,
                    transform: (context, _) => {
                        return context.TargetNode;
                    })
                .Where((x) => true)
                .Collect();

            context.RegisterPostInitializationOutput(context => {

                var source = $$$"""
using System;

namespace Ultimate.Core.SourceGenerators {
    [AttributeUsage(AttributeTargets.Assembly)]
    public class GenerateUltimateOrbCoreSourceCodeAttribute : Attribute {
    }
}
""";
                context.AddSource("GenerateUltimateOrbCoreSourceCodeAttribute.g.cs", source);
            });

            context.RegisterSourceOutput(assemblyAttributes, (spc, assemblies) => {

                if (assemblies.Any()) {
                    var operations = GetOperationDescriptors();
                    foreach (var operation in operations) {
                        GenerateOperation(spc, operation);
                    }
                }
            });
        }

        private List<OperationDescriptor> GetOperationDescriptors() {
            return [
                new() {
                    OperatorName = "BigMul",
                    Kind = "BigBinary",
                    ChecknessVariants = [
                        "Signed", "Unsigned"
                    ]
                },
                new() {
                    OperatorName = "Copy",
                    Kind = "Unary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "Negate",
                    Kind = "Unary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "Increase",
                    Kind = "Unary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "Decrease",
                    Kind = "Unary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "Add",
                    Kind = "Binary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "Subtract",
                    Kind = "Binary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "Multiply",
                    Kind = "Binary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "Divide",
                    Kind = "Binary",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "ShiftLeft",
                    Kind = "Shift",
                    ChecknessVariants = [
                        "", "SignedChecked", "UnsignedChecked"
                    ]
                },
                new() {
                    OperatorName = "ShiftRight",
                    Kind = "Shift",
                    ChecknessVariants = [
                        "Signed", "Unsigned"
                    ]
                },
                new() {
                    OperatorName = "FusedMultiplyAdd",
                    Kind = "FusedMultiply",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "DivRem",
                    Kind = "DivRem",
                    ChecknessVariants = [
                        "Signed", "Unsigned", "Unchecked"
                    ]
                },
                new() {
                    OperatorName = "BitwiseAnd",
                    Kind = "Binary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "BitwiseOr",
                    Kind = "Binary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "BitwiseXor",
                    Kind = "Binary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "BitwiseNot",
                    Kind = "Unary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                new() {
                    OperatorName = "BitwiseAndNot",
                    Kind = "Binary",
                    ChecknessVariants = [
                        ""
                    ]
                },
                // Add more operations as needed
            ];
        }

        private void GenerateOperation(SourceProductionContext context, OperationDescriptor descriptor) {
            var sourceBuilder = new StringBuilder();
            GenerateOperationInterfacesCore(descriptor, sourceBuilder);
            context.AddSource($"IBinaryInteger{descriptor.OperatorName}Provider.g.cs", sourceBuilder.ToString());
        }

        public static void GenerateOperationInterfacesCore(OperationDescriptor descriptor, StringBuilder sourceBuilder) {
            sourceBuilder.AppendLine($$$"""
namespace UltimateOrb.Numerics.Generic {
""");
            var parameters = descriptor.Parameters ?? descriptor.Kind switch {
                "Binary" => [
                    ("out", "T", "result"),
                    ("in", "T", "first"),
                    ("in", "T", "second")
                ],
                "Unary" => [
                    ("out", "T", "result"),
                    ("in", "T", "value")
                ],
                "Shift" => [
                    ("out", "T", "result"),
                    ("in", "T", "value"),
                    ("in", "int", "shiftCount")
                ],
                "BigBinary" => [
                    ("out", "T", "result_lo"),
                    ("out", "T", "result_hi"),
                    ("in", "T", "first"),
                    ("in", "T", "second")
                ],
                "BigDivide" => [
                    ("out", "T", "quotient"),
                    ("in", "T", "dividend_lo"),
                    ("in", "T", "dividend_hi"),
                    ("in", "T", "divisor")
                ],
                "BigRemainder" => [
                    ("out", "T", "remainder"),
                    ("in", "T", "dividend_lo"),
                    ("in", "T", "dividend_hi"),
                    ("in", "T", "divisor"),
                ],
                "FusedMultiply" => [
                    ("out", "T", "result"),
                    ("in", "T", "first"),
                    ("in", "T", "second"),
                    ("in", "T", "remainder")
                ],
                "DivRem" => [
                    ("out", "T", "quotient"),
                    ("out", "T", "remainder"),
                    ("in", "T", "dividend"),
                    ("in", "T", "divisor")
                ],
                _ => throw new InvalidOperationException($"Unknown kind: {descriptor.Kind}")
            };
            foreach (var variant in descriptor.ChecknessVariants) {


                var parameterList = string.Join(", ", parameters.Select(p => $"{p.Modifier} {p.Type} {p.Name}"));

                sourceBuilder.AppendLine($$$"""
    
    [System.Diagnostics.CodeAnalysis.Experimental("UoWIP_GenericMath")]
    public interface IBinaryInteger{{{descriptor.OperatorName}}}{{{variant}}}Provider<TSelf, T>
        where TSelf :
            IBinaryInteger{{{descriptor.OperatorName}}}{{{variant}}}Provider<TSelf, T> {
        public abstract static void {{{descriptor.OperatorName}}}{{{variant}}}({{{parameterList}}});
   }
""");
            }
            sourceBuilder.AppendLine($$$"""
}
""");
        }
    }
}
