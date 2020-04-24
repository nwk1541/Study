using UnityEditor;
using UnityEngine;

namespace Util.Editor
{
    public class ProjectEditorMenu
    {
        [MenuItem("Assets/AssetBundle/Make AssetBundles From Selection")]
        [MenuItem("Tools/AssetBundle/Make AssetBundles")]
        public static void MakeAssetBundles()
        {
            AssetBundleMaker.Make();
        }

        [MenuItem("Tools/Utility/Show Unity Native Paths")]
        public static void ShowUnityNativePaths()
        {
            EditorUtility.ShowUnityNativePath();
        }
    }
}