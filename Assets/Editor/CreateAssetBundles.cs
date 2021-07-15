using UnityEditor;
using System.IO;

public class CreateAssetBundles {
    [MenuItem("Assets/Build Android AssetBundles")]
    static void BuildAndroidAssetBundles() {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory)) {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
        AssetDatabase.Refresh();
    }
}
