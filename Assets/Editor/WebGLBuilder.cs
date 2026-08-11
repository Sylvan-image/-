using UnityEditor;

public class WebGLBuilder
{
    public static void Build()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = new[] {
            "Assets/Scenes/主场景.unity",
            "Assets/Scenes/Prototype 3.unity"
        };
        options.locationPathName = "Build/WebGL";
        options.target = BuildTarget.WebGL;
        options.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(options);
    }
}
