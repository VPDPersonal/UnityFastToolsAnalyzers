using Severity = Microsoft.CodeAnalysis.DiagnosticSeverity;
using Descriptor = Microsoft.CodeAnalysis.DiagnosticDescriptor;

namespace UnityFastToolsAnalyzers.Descriptions;

public static class DiagnosticRules
{
    private const string UsageCategory = "Usage";
    private const string DesignCategory = "Design";
    
    public static readonly Descriptor UsageRule = new(
        id: "UFT0001",
        title: "GetComponentProperty field usage detected",
        messageFormat: "Field '{0}' with [GetComponentProperty] should not be used outside its declaration. Use cached property instead.",
        category: UsageCategory,
        defaultSeverity: Severity.Error,
        isEnabledByDefault: true);
    
    public static readonly Descriptor PartialRule = new(
        id: "UFT0002",
        title: "Class should be partial",
        messageFormat: "Class '{0}' should be partial",
        category: DesignCategory,
        defaultSeverity: Severity.Error,
        isEnabledByDefault: true);
    
    public static readonly Descriptor IndexerAttributeRule = new(
        id: "UFT0003",
        title: "Indexer should not have specific attributes",
        messageFormat: "Indexer '{0}' should not have '{1}' attribute",
        category: DesignCategory,
        defaultSeverity: Severity.Error,
        isEnabledByDefault: true);
    
    public static readonly Descriptor UnityHandlerPropertyRule = new(
        id: "UFT0004",
        title: "Property with [UnityHandler] attribute should have a get accessor",
        messageFormat: "Property '{0}' with [UnityHandler] attribute should have a get accessor",
        category: DesignCategory,
        defaultSeverity: Severity.Error,
        isEnabledByDefault: true);
    
    public static readonly Descriptor GetComponentPropertyRule = new(
        id: "UFT0005",
        title: "Property with [GetComponent] attribute should have a setter",
        messageFormat: "Property '{0}' with [GetComponent] attribute should have a setter",
        category: DesignCategory,
        defaultSeverity: Severity.Error,
        isEnabledByDefault: true);
}