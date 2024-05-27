using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityFastToolsAnalyzers.Helpers.Declarations;

public static class CSharpSyntaxNodeExtension
{
    public static string GetNamespaceName(this CSharpSyntaxNode node)
    {
        var parent = node.Parent;
        
        while (parent != null)
        {
            if (parent is BaseNamespaceDeclarationSyntax namespaceDeclaration)
                return namespaceDeclaration.Name.ToString();
            
            parent = parent.Parent;
        }

        return "";
    }
}