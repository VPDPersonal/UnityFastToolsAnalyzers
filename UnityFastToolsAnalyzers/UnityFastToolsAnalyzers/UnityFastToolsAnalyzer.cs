using System.Linq;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnityFastToolsAnalyzers.Descriptions;
using UnityFastToolsAnalyzers.Helpers.Declarations;
using UnityFastToolsAnalyzers.Descriptions.UnityFastTools;

namespace UnityFastToolsAnalyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class UnityFastToolsAnalyzer : DiagnosticAnalyzer
{
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        ImmutableArray.Create(DiagnosticRules.UsageRule, DiagnosticRules.PartialRule, DiagnosticRules.IndexerAttributeRule,
            DiagnosticRules.UnityHandlerPropertyRule, DiagnosticRules.GetComponentPropertyRule);
    
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        
        context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration, SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzeFieldUsageSyntax, SyntaxKind.IdentifierName);
        context.RegisterSyntaxNodeAction(AnalyzeIndexerDeclaration, SyntaxKind.IndexerDeclaration);
        context.RegisterSyntaxNodeAction(AnalyzePropertyDeclaration, SyntaxKind.PropertyDeclaration);
    }
    
    private static void AnalyzeTypeDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not TypeDeclarationSyntax declaration) return;
        
        var isNeedAttributeExist = false;
        
        foreach (var member in declaration.Members)
        {
            if (member is not FieldDeclarationSyntax and not PropertyDeclarationSyntax)
                return;
            
            foreach (var attribute in member.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
            {
                if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol) return;
                var attributeName = attributeSymbol.ContainingType.ToDisplayString();
                
                if (attributeName is
                    not (AttributesDescription.UnityHandlerFull or
                    AttributesDescription.GetComponentFull or
                    AttributesDescription.GetComponentPropertyFull)) continue;
                isNeedAttributeExist = true;
                break;
            }
            
            if (isNeedAttributeExist) break;
        }
        
        if (!isNeedAttributeExist || declaration.Modifiers.Any(SyntaxKind.PartialKeyword)) return;
        
        var identifier = declaration.Identifier;
        var diagnostic = Diagnostic.Create(DiagnosticRules.PartialRule, identifier.GetLocation(), identifier.Text);
        context.ReportDiagnostic(diagnostic);
    }
    
    private static void AnalyzeFieldUsageSyntax(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IdentifierNameSyntax identifierNameSyntax) return;
        if (identifierNameSyntax.Parent is VariableDeclarationSyntax or AttributeSyntax) return;
        if (context.SemanticModel.GetSymbolInfo(identifierNameSyntax).Symbol is not IFieldSymbol fieldSymbol) return;
        if (!fieldSymbol.GetAttributes().Any(attribute => attribute.AttributeClass?.ToDisplayString() == AttributesDescription.GetComponentPropertyFull)) return;
        
        var diagnostic = Diagnostic.Create(DiagnosticRules.UsageRule, identifierNameSyntax.GetLocation(),
            identifierNameSyntax.Identifier.Text);
        
        context.ReportDiagnostic(diagnostic);
    }
    
    private static void AnalyzeIndexerDeclaration(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is not IndexerDeclarationSyntax declaration) return;
        
        var attributes = new[] { AttributesDescription.UnityHandlerFull, AttributesDescription.GetComponentFull };
        
        foreach (var attribute in declaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
        {
            if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol) continue;
            var attributeName = attributeSymbol.ContainingType.ToDisplayString();
            
            if (attributes.Contains(attributeName))
            {
                var diagnostic = Diagnostic.Create(DiagnosticRules.IndexerAttributeRule, attribute.GetLocation(),
                    declaration.ThisKeyword.Text, attributeName);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
    
    private static void AnalyzePropertyDeclaration(SyntaxNodeAnalysisContext context)
    {
        var declaration = (PropertyDeclarationSyntax)context.Node;
        
        foreach (var attribute in declaration.AttributeLists.SelectMany(attributeList => attributeList.Attributes))
        {
            if (context.SemanticModel.GetSymbolInfo(attribute).Symbol is not IMethodSymbol attributeSymbol) continue;
            var attributeName = attributeSymbol.ContainingType.ToDisplayString();
            
            switch (attributeName)
            {
                case AttributesDescription.UnityHandlerFull:
                    {
                        if (declaration.HasAccessor(SyntaxKind.GetAccessorDeclaration)) continue;
                        if (declaration.ExpressionBody != null) continue;
                        
                        var identifier = declaration.Identifier;
                        var diagnostic = Diagnostic.Create(DiagnosticRules.UnityHandlerPropertyRule, identifier.GetLocation(), identifier.Text);
                        context.ReportDiagnostic(diagnostic);
                        break;
                    }

                case AttributesDescription.GetComponentFull:
                    {
                        if (declaration.HasAccessor(SyntaxKind.SetAccessorDeclaration)) continue;
                        
                        var identifier = declaration.Identifier;
                        var diagnostic = Diagnostic.Create(DiagnosticRules.GetComponentPropertyRule, identifier.GetLocation(), identifier.Text);
                        context.ReportDiagnostic(diagnostic);
                        break;
                    }
            }
        }
    }
}