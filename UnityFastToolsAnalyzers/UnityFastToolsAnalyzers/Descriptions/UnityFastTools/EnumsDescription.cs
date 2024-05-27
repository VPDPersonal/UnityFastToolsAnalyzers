namespace UnityFastToolsAnalyzers.Descriptions.UnityFastTools;

public static class EnumsDescription
{
    public const string Access = nameof(Access);
    public const string AccessFull = $"{NamespacesDescription.UnityFastTools}.{Access}";
    
    public const string WhereGet = nameof(WhereGet);
    public const string WhereGetFull = $"{NamespacesDescription.UnityFastToolsGetComponents}.{WhereGet}";
    
    public const string UnityEventType = nameof(UnityEventType);
    public const string UnityEventTypeFull = $"{NamespacesDescription.UnityFastToolsUnityEvents}.{UnityEventType}";
}