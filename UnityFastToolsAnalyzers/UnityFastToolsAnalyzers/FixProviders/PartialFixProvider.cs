using System.Linq;
using System.Threading;
using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityFastToolsAnalyzers.FixProviders;

[ExportCodeFixProvider(LanguageNames.CSharp, nameof(PartialFixProvider)), Shared]
public sealed class PartialFixProvider : CodeFixProvider
{
    private const string Title = "Make type partial";
    
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create("UFT0002");
    
    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
    
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics[0];
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var declaration = root?.FindToken(diagnosticSpan.Start).Parent?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().First();
        
        if (declaration != null)
        {
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: Title,
                    createChangedDocument: c => MakeTypePartialAsync(context.Document, declaration, c),
                    equivalenceKey: Title),
                diagnostic);
        }
    }
    
    private async Task<Document> MakeTypePartialAsync(Document document, TypeDeclarationSyntax declaration, CancellationToken cancellationToken)
    {
        var partialWorld = SyntaxFactory.Token(SyntaxKind.PartialKeyword);
        var newModifiers = declaration.Modifiers.Add(partialWorld);
        var newDeclaration = declaration.WithModifiers(newModifiers);
        
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var newRoot = root?.ReplaceNode(declaration, newDeclaration);
        
        return newRoot != null ? document.WithSyntaxRoot(newRoot) : document;
    }
}