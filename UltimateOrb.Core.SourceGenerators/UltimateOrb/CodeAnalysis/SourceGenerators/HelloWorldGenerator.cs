using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace UltimateOrb.CodeAnalysis.SourceGenerators {

    [Generator]
    public class HelloWorldGenerator : IIncrementalGenerator {

        public void Initialize(IncrementalGeneratorInitializationContext context) {
            context.RegisterPostInitializationOutput(context => {
                context.AddSource("MyGeneratedFile.cs", SourceText.From(@"
namespace GeneratedNamespace {

    public class GeneratedClass {

        public static void GeneratedMethod() {
        }
    }
}", Encoding.UTF8));
            });
        }
    }
}
