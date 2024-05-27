using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnityFastToolsAnalyzers.Helpers.Declarations;

public static class PropertyDeclarationSyntaxExtension
{
    public static bool HasAccessor(this PropertyDeclarationSyntax propertyDeclaration, SyntaxKind kind)
    {
        var accessorList = propertyDeclaration.AccessorList;
        return accessorList != null && accessorList.Accessors.Any(accessor => accessor.Kind() == kind);
    }
}