using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace UltimateOrb.CodeAnalysis.SourceGenerators {

    [Generator]
    public class HelloWorldGenerator : ISourceGenerator {

        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context) {
            context.AddSource("MyGeneratedFile.cs", SourceText.From(@"
namespace GeneratedNamespace {

    public class GeneratedClass {

        public static void GeneratedMethod() {
        }
    }
}", Encoding.UTF8));
        }
    }
}
