using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityFastToolsAnalyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class GetComponentPropertyAnalyzer : DiagnosticAnalyzer
{
    private static readonly DiagnosticDescriptor _usageErrorDescriptor= new(
        id: "UFT0001",
        title: "GetComponentProperty field usage detected",
        messageFormat: "Field '{0}' with [GetComponentProperty] should not be used outside its declaration. Use cached property instead.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true);
    
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(_usageErrorDescriptor);
    
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.IdentifierName);
    }
    
    private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IdentifierNameSyntax identifierNameSyntax) return;
        if (identifierNameSyntax.Parent is VariableDeclarationSyntax or AttributeSyntax) return;
        if (context.SemanticModel.GetSymbolInfo(identifierNameSyntax).Symbol is not IFieldSymbol fieldSymbol) return;
        if (!fieldSymbol.GetAttributes().Any(attribute => attribute.AttributeClass?.Name == "GetComponentPropertyAttribute")) return;
        
        var diagnostic = Diagnostic.Create(_usageErrorDescriptor, identifierNameSyntax.GetLocation(),
            identifierNameSyntax.Identifier.Text);
        
        context.ReportDiagnostic(diagnostic);
    }
}