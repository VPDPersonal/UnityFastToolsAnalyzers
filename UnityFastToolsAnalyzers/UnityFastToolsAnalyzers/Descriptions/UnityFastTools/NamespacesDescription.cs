namespace UnityFastToolsAnalyzers.Descriptions.UnityFastTools;

public static class NamespacesDescription
{
    public const string UnityFastTools = nameof(UnityFastTools);
    
    public const string UnityFastToolsGetComponents = $"{UnityFastTools}.GetComponents";
    
    public const string UnityFastToolsUnityEvents = $"{UnityFastTools}.UnityEvents";
    public const string UnityFastToolsUnityEventsAdapters = $"{UnityFastToolsUnityEvents}.Adapters";
    public const string UnityFastToolsUnityEventsAdaptersTargets = $"{UnityFastToolsUnityEventsAdapters}.Targets";
}