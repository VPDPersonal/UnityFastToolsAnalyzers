namespace UnityFastToolsAnalyzers.Descriptions.UnityFastTools;

public static class AttributesDescription
{
    public const string UnityHandler = $"{nameof(UnityHandler)}Attribute";
    public const string UnityHandlerFull = $"{NamespacesDescription.UnityFastToolsUnityEvents}.{UnityHandler}";
    
    public const string GetComponent = $"{nameof(GetComponent)}Attribute";
    public const string GetComponentFull = $"{NamespacesDescription.UnityFastToolsGetComponents}.{GetComponent}";
    
    public const string GetComponentProperty = $"{nameof(GetComponentProperty)}Attribute";
    public const string GetComponentPropertyFull = $"{NamespacesDescription.UnityFastToolsGetComponents}.{GetComponentProperty}";
}