using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UltimateOrb.CodeAnalysis.SourceGenerators;

namespace UltimateOrb.CodeAnalysis.SourceGenerators {

    /*
    [GeneratExtensions(Namespace = "f")]
    interface IInterface {

    }
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    [System.Diagnostics.Conditional("InterfaceExtensionsGenerator_DEBUG")]
    sealed class GenerateExtensionsAttribute : Attribute {

        public GenerateExtensionsAttribute() {
        }

        public string? Namespace { get; set; }
    }
    */
}

namespace UltimateOrb.Core.SourceGenerators.Tests {

    [TestClass]
    public class InterfaceExtensionsGeneratorTests {

        [TestMethod]
        public void Test_InterfaceExtensionsGenerator() {
            // Create the 'input' compilation that the generator will act on
            Compilation inputCompilation = CreateCompilation(@"
using System;

namespace MyCode {

    [UltimateOrb.CodeAnalysis.SourceGenerators.GenerateExtensions]
    interface IInterfaceA<T> {

        bool Property1 {

            get;
        }

        internal bool Property2 {

            set;
        }

        public bool Property3 {

            get;
            protected set;
        }

        static int Method4() {
            return 123;
        }

        public long Method5() {
            return 123;
        }

        void Method6(ref long arg1, in uint? arg2 , in string? arg3, Guid arg4) {
            return;
        }

        protected void Method7(ref ulong arg1, in uint? arg2, in string? arg3, Guid arg4);

        void Method8(ref nint arg1, in UIntPtr? arg2, in string? arg3, Guid arg4);

    }
}
");

            // directly create an instance of the generator
            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
            var generator = new InterfaceExtensionsGenerator();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the generation pass
            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
            
            // We can now assert things about the resulting compilation:
            Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
            Debug.Assert(outputCompilation.SyntaxTrees.Count() == 2); // we have two syntax trees, the original 'user' provided one, and the one added by the generator
            System.Collections.Immutable.ImmutableArray<Diagnostic> diagnostics1 = outputCompilation.GetDiagnostics();
            Debug.Assert(diagnostics1.IsEmpty); // verify the compilation with the added source has no diagnostics

            // Or we can look at the results directly:
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            // The runResult contains the combined results of all generators passed to the driver
            Debug.Assert(runResult.GeneratedTrees.Length == 1);
            Debug.Assert(runResult.Diagnostics.IsEmpty);

            // Or you can access the individual results on a by-generator basis
            GeneratorRunResult generatorResult = runResult.Results[0];
            Debug.Assert(generatorResult.Generator == generator);
            Debug.Assert(generatorResult.Diagnostics.IsEmpty);
            Debug.Assert(generatorResult.GeneratedSources.Length == 1);
            Debug.Assert(generatorResult.Exception is null);
        }

        private static Compilation CreateCompilation(string source) {
            var references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(Binder).GetTypeInfo().Assembly.Location), "System.Runtime.dll")));
            references.Add(MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(InterfaceExtensionsGenerator).GetTypeInfo().Assembly.Location));

            return CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                references,
                new CSharpCompilationOptions(OutputKind.NetModule));
        }
    }
}
