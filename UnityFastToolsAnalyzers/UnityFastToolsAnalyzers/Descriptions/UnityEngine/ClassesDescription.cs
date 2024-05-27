namespace UnityFastToolsAnalyzers.Descriptions.UnityEngine;

public static class ClassesDescription
{
    public const string Button = nameof(Button);
    public const string ButtonFull = $"{NamespacesDescription.UnityEngineUI}.{Button}";
    
    public const string Toggle = nameof(Toggle);
    public const string ToggleFull = $"{NamespacesDescription.UnityEngineUI}.{Toggle}";
    
    public const string Slider = nameof(Slider);
    public const string SliderFull = $"{NamespacesDescription.UnityEngineUI}.{Slider}";
    
    public const string ScrollRect = nameof(ScrollRect);
    public const string ScrollRectFull = $"{NamespacesDescription.UnityEngineUI}.{ScrollRect}";
}