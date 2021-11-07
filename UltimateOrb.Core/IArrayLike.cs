using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Internal {

    [Generator]
    public class AugmentingGenerator : ISourceGenerator {
        public void Initialize(GeneratorInitializationContext context) {
            // Register a factory that can create our custom syntax receiver
            context.RegisterForSyntaxNotifications(() => new MySyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context) {
            // the generator infrastructure will create a receiver and populate it
            // we can retrieve the populated instance via the context
            MySyntaxReceiver syntaxReceiver = (MySyntaxReceiver)context.SyntaxReceiver;

            // get the recorded user class
            ClassDeclarationSyntax userClass = syntaxReceiver.ClassToAugment;
            if (userClass is null) {
                // if we didn't find the user class, there is nothing to do
                return;
            }

            // add the generated implementation to the compilation
            SourceText sourceText = SourceText.From($@"
public partial class {userClass.Identifier}
{{
    private void GeneratedMethod()
    {{
        // generated code
    }}
}}", Encoding.UTF8);
            context.AddSource("UserClass.Generated.cs", sourceText);
        }

        class MySyntaxReceiver : ISyntaxReceiver {
            public ClassDeclarationSyntax ClassToAugment { get; private set; }

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
                // Business logic to decide what we're interested in goes here
                if (syntaxNode is ClassDeclarationSyntax cds &&
                    cds.Identifier.ValueText == "UserClass") {
                    ClassToAugment = cds;
                }
            }
        }
    }
}

namespace UltimateOrb {

    public interface IReadOnlyIndexable<TKey, TValue> {

        public ref readonly TValue this[TKey index] {

            get;
        }

        public Type KeyType {

            get => typeof(TKey);
        }

        public Type ValueType {

            get => typeof(TValue);
        }
    }

    public interface IIndexable<TKey, TValue> : IReadOnlyIndexable<TKey, TValue> {

        ref readonly TValue IReadOnlyIndexable<TKey, TValue>.this[TKey index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[index];
        }

        public new ref TValue this[TKey index] {

            get;
        }
    }

    public interface IArrayLikeBase {

        public int Length {

            get;
        }

        public nint NativeLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Length;
        }

        public long LongLength {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NativeLength;
        }
    }

    


    public interface IArrayLike<T>: IArrayLikeBase {

        public ref T this[int index] {

            get;
        }

        [CLSCompliant(false)]
        public ref T this[uint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((int)index)];
        }

        public ref T this[nint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((int)index)];
        }

        [CLSCompliant(false)]
        public ref T this[nuint index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nint)index)];
        }

        public ref T this[long index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((nint)index)];
        }

        [CLSCompliant(false)]
        public ref T this[ulong index] {

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this[checked((long)index)];
        }
    }
}
