// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.CodeAnalysis.Testing.Lightup;
using Microsoft.CodeAnalysis.Testing.Extensions;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Analyzer.Utilities.Extensions;

namespace Test.Utilities {

    public class CSharpCodeFixVerifier<TAnalyzer, TCodeFix, TVerifier> : CodeFixVerifier<TAnalyzer, TCodeFix, CSharpCodeFixTest<TAnalyzer, TCodeFix, TVerifier>, TVerifier>
      where TAnalyzer : DiagnosticAnalyzer, new()
      where TCodeFix : CodeFixProvider, new()
      where TVerifier : IVerifier, new() {
    }
    
    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new() {
        public static DiagnosticResult Diagnostic()
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic();

        public static DiagnosticResult Diagnostic(string diagnosticId)
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic(diagnosticId);

        public static DiagnosticResult Diagnostic(DiagnosticDescriptor descriptor)
            => CSharpCodeFixVerifier<TAnalyzer, TCodeFix, MSTestVerifier>.Diagnostic(descriptor);

        public static async Task VerifyAnalyzerAsync(string source, params DiagnosticResult[] expected) {
            var test = new Test {
                TestCode = source,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync();
        }

        public static async Task VerifyCodeFixAsync(string source, string fixedSource)
            => await VerifyCodeFixAsync(source, DiagnosticResult.EmptyDiagnosticResults, fixedSource);

        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult expected, string fixedSource)
            => await VerifyCodeFixAsync(source, new[] { expected }, fixedSource);

        public static async Task VerifyCodeFixAsync(string source, DiagnosticResult[] expected, string fixedSource) {
            var test = new Test {
                TestCode = source,
                FixedCode = fixedSource,
            };

            test.ExpectedDiagnostics.AddRange(expected);
            await test.RunAsync();
        }
    }

    public static partial class CSharpCodeFixVerifier<TAnalyzer, TCodeFix>
        where TAnalyzer : DiagnosticAnalyzer, new()
        where TCodeFix : CodeFixProvider, new() {
        public class Test : CSharpCodeFixTest<TAnalyzer, TCodeFix, MSTestVerifier> {
            static Test() {
                // If we have outdated defaults from the host unit test application targeting an older .NET Framework, use more
                // reasonable TLS protocol version for outgoing connections.
#pragma warning disable CA5364 // Do Not Use Deprecated Security Protocols
#pragma warning disable CS0618 // Type or member is obsolete
                if (ServicePointManager.SecurityProtocol == (SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls))
#pragma warning restore CS0618 // Type or member is obsolete
#pragma warning restore CA5364 // Do Not Use Deprecated Security Protocols
                {
#pragma warning disable CA5386 // Avoid hardcoding SecurityProtocolType value
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#pragma warning restore CA5386 // Avoid hardcoding SecurityProtocolType value
                }
            }

            internal static readonly ImmutableDictionary<string, ReportDiagnostic> NullableWarnings = GetNullableWarningsFromCompiler();

            public Test() {
                ReferenceAssemblies = AdditionalMetadataReferences.Default;
            }

            private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler() {
                string[] args = { "/warnaserror:nullable" };
                var commandLineArguments = CSharpCommandLineParser.Default.Parse(args, baseDirectory: Environment.CurrentDirectory, sdkDirectory: Environment.CurrentDirectory);
                var nullableWarnings = commandLineArguments.CompilationOptions.SpecificDiagnosticOptions;

                return nullableWarnings;
            }

            public LanguageVersion LanguageVersion { get; set; } = LanguageVersion.CSharp7_3;

            protected override CompilationOptions CreateCompilationOptions() {
                var compilationOptions = base.CreateCompilationOptions();
                return compilationOptions.WithSpecificDiagnosticOptions(
                    compilationOptions.SpecificDiagnosticOptions.SetItems(NullableWarnings));
            }

            protected virtual ImmutableArray<(Project project, Diagnostic diagnostic)> SortDistinctDiagnostics(IEnumerable<(Project project, Diagnostic diagnostic)> diagnostics) {
                var baseResult = diagnostics
                .OrderBy(d => d.diagnostic.Location.GetLineSpan().Path, StringComparer.Ordinal)
                .ThenBy(d => d.diagnostic.Location.SourceSpan.Start)
                .ThenBy(d => d.diagnostic.Location.SourceSpan.End)
                .ThenBy(d => d.diagnostic.Id)
                .ThenBy(d => d.diagnostic.Arguments(), LexicographicComparer.Instance).ToImmutableArray();
                if (typeof(DiagnosticSuppressor).IsAssignableFrom(typeof(TAnalyzer))) {
                    // Include suppressed diagnostics when testing diagnostic suppressors
                    return baseResult;
                }

                // Treat suppressed diagnostics as non-existent. Normally this wouldn't be necessary, but some of the
                // tests include diagnostics reported in code wrapped in '#pragma warning disable'.
                return baseResult.WhereAsArray(diagnostic => !diagnostic.diagnostic.IsSuppressed);
            }

            protected override ParseOptions CreateParseOptions() {
                return ((CSharpParseOptions)base.CreateParseOptions()).WithLanguageVersion(LanguageVersion);
            }
        }

        private sealed class LexicographicComparer : IComparer<IEnumerable<object?>?> {
            public static LexicographicComparer Instance { get; } = new LexicographicComparer();

            public int Compare(IEnumerable<object?>? x, IEnumerable<object?>? y) {
                if (x is null) {
                    return y is null ? 0 : -1;
                } else if (y is null) {
                    return 1;
                }

                using var xe = x.GetEnumerator();
                using var ye = y.GetEnumerator();

                while (xe.MoveNext()) {
                    if (!ye.MoveNext()) {
                        // y has fewer elements
                        return 1;
                    }

                    IComparer elementComparer = Comparer<object>.Default;
                    if (xe.Current is string && ye.Current is string) {
                        // Avoid culture-sensitive string comparisons
                        elementComparer = StringComparer.Ordinal;
                    }

                    try {
                        var elementComparison = elementComparer.Compare(xe.Current, ye.Current);
                        if (elementComparison == 0) {
                            continue;
                        }

                        return elementComparison;
                    } catch (ArgumentException) {
                        // The arguments are not directly comparable, so convert the values to strings and try again
                        var elementComparison = string.CompareOrdinal(xe.Current?.ToString(), ye.Current?.ToString());
                        if (elementComparison == 0) {
                            continue;
                        }

                        return elementComparison;
                    }
                }

                if (ye.MoveNext()) {
                    // x has fewer elements
                    return -1;
                }

                return 0;
            }
        }
    }

    public static class AdditionalMetadataReferences {
        public static ReferenceAssemblies Default { get; } = CreateDefaultReferenceAssemblies();

        public static ReferenceAssemblies DefaultWithoutRoslynSymbols { get; } = ReferenceAssemblies.Default
            .AddAssemblies(ImmutableArray.Create("System.Xml.Data"))
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.CodeAnalysis.Workspaces.Common", "3.0.0")));

        public static ReferenceAssemblies DefaultWithSystemWeb { get; } = ReferenceAssemblies.NetFramework.Net472.Default
            .AddAssemblies(ImmutableArray.Create("System.Web", "System.Web.Extensions"));

        public static ReferenceAssemblies DefaultForTaintedDataAnalysis { get; } = ReferenceAssemblies.NetFramework.Net472.Default
            .AddAssemblies(ImmutableArray.Create("PresentationFramework", "System.Web", "System.Web.Extensions", "System.Xaml"))
            .AddPackages(ImmutableArray.Create(
                new PackageIdentity("System.DirectoryServices", "6.0.1"),
                new PackageIdentity("AntiXSS", "4.3.0"),
                new PackageIdentity("Microsoft.AspNetCore.Mvc", "2.2.0"),
                new PackageIdentity("Microsoft.EntityFrameworkCore.Relational", "2.0.3")));

        public static ReferenceAssemblies DefaultWithSerialization { get; } = ReferenceAssemblies.NetFramework.Net472.Default
            .AddAssemblies(ImmutableArray.Create("System.Runtime.Serialization"));

        public static ReferenceAssemblies DefaultWithAzureStorage { get; } = ReferenceAssemblies.Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("WindowsAzure.Storage", "9.0.0")));

        public static ReferenceAssemblies DefaultWithNewtonsoftJson10 { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Newtonsoft.Json", "10.0.1")));

        public static ReferenceAssemblies DefaultWithNewtonsoftJson12 { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Newtonsoft.Json", "12.0.1")));

        public static ReferenceAssemblies DefaultWithMELogging { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Extensions.Logging", "5.0.0")));

        public static ReferenceAssemblies DefaultWithWilson { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.IdentityModel.Tokens", "6.12.0")));

        public static ReferenceAssemblies DefaultWithWinForms { get; } = ReferenceAssemblies.NetFramework.Net472.WindowsForms;

        public static ReferenceAssemblies DefaultWithWinHttpHandler { get; } = ReferenceAssemblies.NetStandard.NetStandard20
            .AddPackages(ImmutableArray.Create(new PackageIdentity("System.Net.Http.WinHttpHandler", "4.7.0")));

        public static ReferenceAssemblies DefaultWithAspNetCoreMvc { get; } = Default
            .AddPackages(ImmutableArray.Create(
                new PackageIdentity("Microsoft.AspNetCore", "1.1.7"),
                new PackageIdentity("Microsoft.AspNetCore.Mvc", "1.1.8"),
                new PackageIdentity("Microsoft.AspNetCore.Http", "1.1.2")));

        public static ReferenceAssemblies DefaultWithNUnit { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("NUnit", "3.12.0")));

        public static ReferenceAssemblies DefaultWithXUnit { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("xunit", "2.4.1")));

        public static ReferenceAssemblies DefaultWithMSTest { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("MSTest.TestFramework", "2.1.0")));

        public static ReferenceAssemblies DefaultWithAsyncInterfaces { get; } = Default
            .AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.Bcl.AsyncInterfaces", "1.1.0")));

        public static MetadataReference SystemCollectionsImmutableReference { get; } = MetadataReference.CreateFromFile(typeof(ImmutableHashSet<>).Assembly.Location);
        public static MetadataReference SystemComponentModelCompositionReference { get; } = MetadataReference.CreateFromFile(typeof(System.ComponentModel.Composition.ExportAttribute).Assembly.Location);
        public static MetadataReference SystemXmlDataReference { get; } = MetadataReference.CreateFromFile(typeof(System.Data.Rule).Assembly.Location);
        public static MetadataReference CodeAnalysisReference { get; } = MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location);
        public static MetadataReference CSharpSymbolsReference { get; } = MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location);
        public static MetadataReference WorkspacesReference { get; } = MetadataReference.CreateFromFile(typeof(Workspace).Assembly.Location);
#if !NETCOREAPP
        public static MetadataReference SystemWebReference { get; } = MetadataReference.CreateFromFile(typeof(System.Web.HttpRequest).Assembly.Location);
        public static MetadataReference SystemRuntimeSerialization { get; } = MetadataReference.CreateFromFile(typeof(System.Runtime.Serialization.NetDataContractSerializer).Assembly.Location);
#endif
        public static MetadataReference TestReferenceAssembly { get; } = MetadataReference.CreateFromFile(typeof(OtherDll.OtherDllStaticMethods).Assembly.Location);
#if !NETCOREAPP
        public static MetadataReference SystemXaml { get; } = MetadataReference.CreateFromFile(typeof(System.Xaml.XamlReader).Assembly.Location);
        public static MetadataReference PresentationFramework { get; } = MetadataReference.CreateFromFile(typeof(System.Windows.Markup.XamlReader).Assembly.Location);
        public static MetadataReference SystemWeb { get; } = MetadataReference.CreateFromFile(typeof(System.Web.HttpRequest).Assembly.Location);
        public static MetadataReference SystemWebExtensions { get; } = MetadataReference.CreateFromFile(typeof(System.Web.Script.Serialization.JavaScriptSerializer).Assembly.Location);
        public static MetadataReference SystemServiceModel { get; } = MetadataReference.CreateFromFile(typeof(System.ServiceModel.OperationContractAttribute).Assembly.Location);
#endif

        private static ReferenceAssemblies CreateDefaultReferenceAssemblies() {
            var referenceAssemblies = ReferenceAssemblies.Default;

#if !NETCOREAPP
            referenceAssemblies = referenceAssemblies.AddAssemblies(ImmutableArray.Create("System.Xml.Data"));
#endif

            referenceAssemblies = referenceAssemblies.AddPackages(ImmutableArray.Create(new PackageIdentity("Microsoft.CodeAnalysis", "3.0.0")));

#if NETCOREAPP
            referenceAssemblies = referenceAssemblies.AddPackages(ImmutableArray.Create(
                new PackageIdentity("System.Runtime.Serialization.Formatters", "4.3.0"),
                new PackageIdentity("System.Configuration.ConfigurationManager", "4.7.0"),
                new PackageIdentity("System.Security.Cryptography.Cng", "4.7.0"),
                new PackageIdentity("System.Security.Permissions", "4.7.0"),
                new PackageIdentity("Microsoft.VisualBasic", "10.3.0")));
#endif

            return referenceAssemblies;
        }
    }
}


namespace OtherDll {
    /// <summary>
    /// Aids with testing dataflow analysis _not_ doing interprocedural DFA.
    /// </summary>
    /// <remarks>
    /// Since Roslyn doesn't support cross-binary DFA, and this class is
    /// defined in a different binary, using this class from test source code
    /// is a way to test handling of non-interprocedural results in dataflow 
    /// analysis implementations.
    /// </remarks>
    public static class OtherDllStaticMethods {
        public static T? ReturnsInput<T>(T? input)
            where T : class {
            return input;
        }

        public static T? ReturnsDefault<T>(T? input)
            where T : class {
            return default;
        }

        public static string ReturnsRandom(string input) {
            Random r = new Random();
            byte[] bytes = new byte[r.Next(20) + 10];
            r.NextBytes(bytes);
            bytes = bytes.Where(b => b is >= ((byte)' ') and <= ((byte)'~')).ToArray();
            return Encoding.ASCII.GetString(bytes);
        }

        public static void SetsOutputToInput<T>(T? input, out T? output)
            where T : class {
            output = input;
        }

        public static void SetsOutputToDefault<T>(T? input, out T? output)
            where T : class {
            output = default;
        }

        public static void SetsOutputToRandom(string input, out string output) {
            output = ReturnsRandom(input);
        }

        public static void SetsReferenceToInput<T>(T? input, ref T? output)
            where T : class {
            output = input;
        }

        public static void SetsReferenceToDefault<T>(T? input, ref T? output)
            where T : class {
            output = default;
        }

        public static void SetsReferenceToRandom(string input, ref string output) {
            Random r = new Random();
            byte[] bytes = new byte[r.Next(20) + 10];
            r.NextBytes(bytes);
            bytes = bytes.Where(b => b is >= ((byte)' ') and <= ((byte)'~')).ToArray();
            output = Encoding.ASCII.GetString(bytes);
        }
    }
}

namespace Microsoft.CodeAnalysis.Testing.Lightup {
    internal static class LightupHelpers {
        /// <summary>
        /// Generates a compiled accessor method for a property which cannot be bound at compile time.
        /// </summary>
        /// <typeparam name="T">The compile-time type representing the instance on which the property is defined. This
        /// may be a superclass of the actual type on which the property is declared if the declaring type is not
        /// available at compile time.</typeparam>
        /// <typeparam name="TResult">The compile-type type representing the result of the property. This may be a
        /// superclass of the actual type of the property if the property type is not available at compile
        /// time.</typeparam>
        /// <param name="type">The runtime time on which the property is defined. If this value is null, the runtime
        /// time is assumed to not exist, and a fallback accessor returning <paramref name="defaultValue"/> will be
        /// generated.</param>
        /// <param name="propertyName">The name of the property to access.</param>
        /// <param name="defaultValue">The value to return if the property is not available at runtime.</param>
        /// <returns>An accessor method to access the specified runtime property.</returns>
        public static Func<T, TResult> CreatePropertyAccessor<T, TResult>(Type? type, string propertyName, TResult defaultValue) {
            if (propertyName is null) {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (type == null) {
                return CreateFallbackAccessor<T, TResult>(defaultValue);
            }

            if (!typeof(T).GetTypeInfo().IsAssignableFrom(type.GetTypeInfo())) {
                throw new InvalidOperationException($"Type '{type}' is not assignable to type '{typeof(T)}'");
            }

            var property = type.GetTypeInfo().GetDeclaredProperty(propertyName);
            if (property == null) {
                return CreateFallbackAccessor<T, TResult>(defaultValue);
            }

            if (!typeof(TResult).GetTypeInfo().IsAssignableFrom(property.PropertyType.GetTypeInfo())) {
                throw new InvalidOperationException($"Property '{property}' produces a value of type '{property.PropertyType}', which is not assignable to type '{typeof(TResult)}'");
            }

            var parameter = Expression.Parameter(typeof(T), GenerateParameterName(typeof(T)));
            Expression instance =
                type.GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo())
                ? (Expression)parameter
                : Expression.Convert(parameter, type);

            Expression<Func<T, TResult>> expression =
                Expression.Lambda<Func<T, TResult>>(
                    Expression.Convert(Expression.Call(instance, property.GetMethod), typeof(TResult)),
                    parameter);
            return expression.Compile();
        }

        private static string GenerateParameterName(Type parameterType) {
            var typeName = parameterType.Name;
            return char.ToLower(typeName[0]) + typeName.Substring(1);
        }

        private static Func<T, TResult> CreateFallbackAccessor<T, TResult>(TResult defaultValue) {
            TResult FallbackAccessor(T syntax) {
                if (syntax == null) {
                    // Unlike an extension method which would throw ArgumentNullException here, the light-up
                    // behavior needs to match behavior of the underlying property.
                    throw new NullReferenceException();
                }

                return defaultValue;
            }

            return FallbackAccessor;
        }
    }
}

namespace Microsoft.CodeAnalysis.Testing.Extensions {
    internal static class DiagnosticExtensions {
        private static readonly Func<Diagnostic, IReadOnlyList<object?>> s_arguments =
            LightupHelpers.CreatePropertyAccessor<Diagnostic, IReadOnlyList<object?>>(
                typeof(Diagnostic),
                nameof(Arguments),
                defaultValue: new object[0]);

        private static readonly Func<Diagnostic, bool> s_isSuppressed =
            LightupHelpers.CreatePropertyAccessor<Diagnostic, bool>(
                typeof(Diagnostic),
                nameof(IsSuppressed),
                defaultValue: false);

        public static IReadOnlyList<object?> Arguments(this Diagnostic diagnostic)
            => s_arguments(diagnostic);

        public static bool IsSuppressed(this Diagnostic diagnostic)
            => s_isSuppressed(diagnostic);
    }
}

namespace Analyzer.Utilities.Extensions {
    internal static class IEnumerableExtensions {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T value) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            return ConcatImpl(source, value);

            static IEnumerable<T> ConcatImpl(IEnumerable<T> source, T value) {
                foreach (T v in source) {
                    yield return v;
                }

                yield return value;
            }
        }

        public static ISet<T> ToSet<T>(this IEnumerable<T> source) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            return source as ISet<T> ?? new HashSet<T>(source);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, IComparer<T> comparer) {
            return source.OrderBy(t => t, comparer);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, Comparison<T> compare) {
            return source.OrderBy(new ComparisonComparer<T>(compare));
        }

        public static IEnumerable<T> Order<T>(this IEnumerable<T> source) where T : IComparable<T> {
            return source.OrderBy((t1, t2) => t1.CompareTo(t2));
        }

        private static readonly Func<object?, bool> s_notNullTest = x => x != null;

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source) where T : class {
            if (source == null) {
                return ImmutableArray<T>.Empty;
            }

            return source.Where((Func<T?, bool>)s_notNullTest)!;
        }

        public static ImmutableArray<TSource> WhereAsArray<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> selector) {
            var builder = ImmutableArray.CreateBuilder<TSource>();
            bool any = false;
            foreach (var element in source) {
                if (selector(element)) {
                    any = true;
                    builder.Add(element);
                }
            }

            if (any) {
                return builder.ToImmutable();
            } else {
                return ImmutableArray<TSource>.Empty;
            }
        }

        public static void Dispose<T>(this IEnumerable<T?> collection)
            where T : class, IDisposable {
            foreach (var item in collection) {
                item?.Dispose();
            }
        }

        /// <summary>
        /// Determines whether a sequence contains, exactly, <paramref name="count"/> elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for cardinality.</param>
        /// <param name="count">The number of elements to ensure exists.</param>
        /// <returns><see langword="true" /> the source sequence contains, exactly, <paramref name="count"/> elements; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static bool HasExactly<TSource>(this IEnumerable<TSource> source, int count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<TSource> collectionoft) {
                return collectionoft.Count == count;
            }

            if (source is ICollection collection) {
                return collection.Count == count;
            }

            using var enumerator = source.GetEnumerator();
            while (count-- > 0) {
                if (!enumerator.MoveNext()) {
                    return false;
                }
            }

            return !enumerator.MoveNext();
        }

        /// <summary>
        /// Determines whether a sequence contains more than <paramref name="count"/> elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for cardinality.</param>
        /// <param name="count">The number of elements to ensure exists.</param>
        /// <returns><see langword="true" /> the source sequence contains more than <paramref name="count"/> elements; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static bool HasMoreThan<TSource>(this IEnumerable<TSource> source, int count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<TSource> collectionoft) {
                return collectionoft.Count > count;
            }

            if (source is ICollection collection) {
                return collection.Count > count;
            }

            using var enumerator = source.GetEnumerator();
            while (count-- > 0) {
                if (!enumerator.MoveNext()) {
                    return false;
                }
            }

            return enumerator.MoveNext();
        }

        /// <summary>
        /// Determines whether a sequence contains fewer than <paramref name="count"/> elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{TSource}"/> to check for cardinality.</param>
        /// <param name="count">The number of elements to ensure exists.</param>
        /// <returns><see langword="true" /> the source sequence contains less than <paramref name="count"/> elements; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static bool HasFewerThan<TSource>(this IEnumerable<TSource> source, int count) {
            if (source is null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ICollection<TSource> collectionoft) {
                return collectionoft.Count < count;
            }

            if (source is ICollection collection) {
                return collection.Count < count;
            }

            using var enumerator = source.GetEnumerator();
            while (count > 0 && enumerator.MoveNext()) {
                count--;
            }

            return count > 0;
        }

        private sealed class ComparisonComparer<T> : Comparer<T> {
            private readonly Comparison<T> _compare;

            public ComparisonComparer(Comparison<T> compare) {
                _compare = compare;
            }

            public override int Compare([AllowNull] T x, [AllowNull] T y) {
                if (x is null) {
                    return y is null ? 0 : -1;
                } else if (y is null) {
                    return 1;
                }

                return _compare(x, y);
            }
        }
    }
}
