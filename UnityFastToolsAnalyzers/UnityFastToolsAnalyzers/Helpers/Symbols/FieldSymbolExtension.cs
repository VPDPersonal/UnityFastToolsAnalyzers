using Microsoft.CodeAnalysis;

namespace UnityFastToolsAnalyzers.Helpers.Symbols;

public static class FieldSymbolExtension
{
    public static string GetPropertyName(this IFieldSymbol symbol) =>
        GetPropertyNameFromField(symbol.Name);
    
    public static string GetPropertyNameFromField(string name)
    {
        var prefixCount = GetPrefixCount();
        name = name.Remove(0, prefixCount);
        
        var firstSymbol = name[0];
        if (char.IsLower(firstSymbol))
        {
            name = name.Remove(0, 1);
            name = char.ToUpper(firstSymbol) + name;
        }
        
        return name;
        
        // TODO Custom prefix from config
        int GetPrefixCount() =>
            name.StartsWith("_") ? 1 : name.StartsWith("m_") ? 2 : 0;
    }
}