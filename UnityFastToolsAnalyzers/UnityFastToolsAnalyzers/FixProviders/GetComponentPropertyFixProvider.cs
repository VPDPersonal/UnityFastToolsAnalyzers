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
using UnityFastToolsAnalyzers.Helpers.Symbols;

namespace UnityFastToolsAnalyzers.FixProviders;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(GetComponentPropertyFixProvider)), Shared]
public sealed class GetComponentPropertyFixProvider : CodeFixProvider
{
    private const string Title = "Replace field usage with property";
    
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create("UFT0001");
    
    public override FixAllProvider? GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
    
    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var node = root.FindNode(diagnosticSpan);
        
        context.RegisterCodeFix(
            CodeAction.Create(
                title: Title,
                createChangedDocument: c => ReplaceFieldWithPropertyAsync(context.Document, node, c),
                equivalenceKey: Title),
            diagnostic);
    }
    
    private static async Task<Document> ReplaceFieldWithPropertyAsync(Document document, SyntaxNode node, CancellationToken cancellationToken)
    {
        var identifierNameSyntax = node as IdentifierNameSyntax;
        var fieldName = identifierNameSyntax.Identifier.Text;

        // Determine the property name based on field naming conventions
        var propertyName = "Cached" + FieldSymbolExtension.GetPropertyNameFromField(fieldName);

        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

        // Replace the field name with the property name
        var newIdentifier = SyntaxFactory.IdentifierName(propertyName)
            .WithLeadingTrivia(identifierNameSyntax.GetLeadingTrivia())
            .WithTrailingTrivia(identifierNameSyntax.GetTrailingTrivia());

        var newRoot = root.ReplaceNode(identifierNameSyntax, newIdentifier);
        var newDocument = document.WithSyntaxRoot(newRoot);

        return newDocument;
    }
}