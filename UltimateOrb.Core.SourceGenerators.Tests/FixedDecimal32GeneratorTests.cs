using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UltimateOrb.CodeAnalysis.SourceGenerators;
using UltimateOrb.Core.CodeAnalyzers.UltimateOrb.CodeAnalyzers;
using Xunit;

namespace UltimateOrb.Core.SourceGenerators.Tests {

    [TestClass]
    public class FixedDecimal32GeneratorTests {

        [TestMethod]
        public void Test_FixedDecimal32Generator() {
            // Create the 'input' compilation that the generator will act on
            Compilation inputCompilation = CreateCompilation($$$"""""
using System;

namespace MyCode {

    [UltimateOrb.Numerics.Specialized.GenerateFixedDecimal32(-3)]
    public readonly partial struct MilliDecimal32 {
    }
}
""""");

            // directly create an instance of the generator
            // (Note: in the compiler this is loaded from an assembly, and created via reflection at runtime)
            var generator = new FixedDecimal32Generator();

            // Create the driver that will control the generation, passing in our generator
            GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

            // Run the generation pass
            // (Note: the generator driver itself is immutable, and all calls return an updated version of the driver that you should use for subsequent calls)
            driver = driver.RunGeneratorsAndUpdateCompilation(inputCompilation, out var outputCompilation, out var diagnostics);
            
            // We can now assert things about the resulting compilation:
            Debug.Assert(diagnostics.IsEmpty); // there were no diagnostics created by the generators
            Debug.Assert(outputCompilation.SyntaxTrees.Count() == 3); // we have three syntax trees, the original 'user' provided one, the attribute definition, and the generated partial struct added by the generator
            System.Collections.Immutable.ImmutableArray<Diagnostic> diagnostics1 = outputCompilation.GetDiagnostics();
            Debug.Assert(diagnostics1.Where(x => x.DefaultSeverity > DiagnosticSeverity.Hidden).Count() <= 0); // verify the compilation with the added source has no diagnostics

            // Or we can look at the results directly:
            GeneratorDriverRunResult runResult = driver.GetRunResult();

            // The runResult contains the combined results of all generators passed to the driver
            Debug.Assert(runResult.GeneratedTrees.Length == 2);
            Debug.Assert(runResult.Diagnostics.IsEmpty);

            // Or you can access the individual results on a by-generator basis
            GeneratorRunResult generatorResult = runResult.Results[0];
            Debug.Assert(generatorResult.Generator == generator);
            Debug.Assert(generatorResult.Diagnostics.IsEmpty);
            Debug.Assert(generatorResult.GeneratedSources.Length == 2);
            Console.Out.WriteLine(generatorResult.GeneratedSources.Last().SourceText.ToString());
            Debug.Assert(generatorResult.GeneratedSources.Last().SourceText.ToString().Contains("MaxValue"));
            Debug.Assert(generatorResult.Exception is null);
        }

        private static Compilation CreateCompilation(string source) {
            var references = new List<MetadataReference>();
            references.Add(MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(Binder).GetTypeInfo().Assembly.Location), "System.Runtime.dll")));
            references.Add(MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(FixedDecimal32Generator).GetTypeInfo().Assembly.Location));
            // references.Add(MetadataReference.CreateFromFile(typeof(GenerateFixedDecimal32Attribute).GetTypeInfo().Assembly.Location));

            return CSharpCompilation.Create("compilation",
                new[] { CSharpSyntaxTree.ParseText(source) },
                references,
                new CSharpCompilationOptions(OutputKind.NetModule));
        }
    }

    [TestClass]
    public class ReplaceOperatorTests {

        [TestMethod]
        public async Task TestReplaceLessThanWithGreaterThan() {
            // 定义一个原始的代码字符串，包含一个使用<的表达式
            var test = $$$""""""
using System;
namespace ConsoleApplication1 {
  class Program {
    static void Main(string[] args) {
      int x = 10;
      if (x < 0) {
        Console.WriteLine("Negative");
      }
    }
  }
}
"""""";

            // 定义一个期望的代码字符串，包含一个使用>的表达式
            var expected = $$$""""""
using System;
namespace ConsoleApplication1 {
  class Program {
    static void Main(string[] args) {
      int x = 10;
      if (0 > x) {
        Console.WriteLine("Negative");
      }
    }
  }
}
"""""";

            // 创建一个诊断对象，指定位置，ID，消息和参数
            var expectedDiagnostic = new DiagnosticResult(ReplaceComparisonOperatorsAnalyzer.DiagnosticId, DiagnosticSeverity.Warning)
                .WithSpan(6, 13, 6, 14)
                .WithArguments("ReplaceComparisonOperators", "<", ">");


            // 使用VerifyCodeFixAsync方法来验证分析器和代码修复器的行为是否符合预期
            await new Test.Utilities.CSharpCodeFixVerifier<ReplaceComparisonOperatorsAnalyzer, ReplaceComparisonOperatorsFixProvider>.Test {
                TestCode = test,
                FixedCode = expected,
                ExpectedDiagnostics = { expectedDiagnostic },
                CodeActionEquivalenceKey = "Replace operator"
            }.RunAsync();
        }

        // ... 其他测试方法 ...
    }
}
