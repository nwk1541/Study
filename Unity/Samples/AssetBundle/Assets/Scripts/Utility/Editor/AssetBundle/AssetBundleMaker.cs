using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Util.Editor
{
    public class AssetBundleMaker
    {
        static readonly string ASSET_BUNDLE_VARIANT = "unity3d";

        static BuildAssetBundleOptions buildOption = BuildAssetBundleOptions.ChunkBasedCompression;
        static BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

        public static void Make(Object[] objs)
        {
            BuildAssetBundles(objs);
        }

        static void BuildAssetBundles(Object[] objs)
        {
            Init(Application.streamingAssetsPath);

            // 선택된게 없다면 전부다 만든다고 가정, 특정 폴더 밑에 있는 모든 리소스 애셋번들로
            bool makeAll = objs == null || objs.Length == 0;
            string[] guids = AssetDatabase.FindAssets("t:Object", new string[] { "Assets/Resources/Prefabs", "Assets/Resources/Atlas" });

            List<AssetBundleBuild> assetBundleList = new List<AssetBundleBuild>();
            int count = makeAll ? guids.Length : objs.Length;

            for (int idx = 0; idx < count; idx++)
            {
                string path = makeAll ? AssetDatabase.GUIDToAssetPath(guids[idx]) : AssetDatabase.GetAssetPath(objs[idx]);
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer == null)
                {
                    Debug.LogErrorFormat("AssetImporter is NULL, PATH : {0}", path);
                    continue;
                }

                string bundleName = Path.GetFileNameWithoutExtension(path);
                string bundleVariant = ASSET_BUNDLE_VARIANT;

                // 애셋번들 이름, 베리언츠 할당
                importer.assetBundleName = bundleName;
                importer.assetBundleVariant = bundleVariant;
                importer.SaveAndReimport();

                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleName;
                build.assetBundleVariant = bundleVariant;
                build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(importer.assetBundleName + "." + bundleVariant);
                assetBundleList.Add(build);
            }

            // 최종 Path
            string outputPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + EditorUtility.GetPlatformName();

            if (assetBundleList.Count == 0)
                BuildPipeline.BuildAssetBundles(outputPath, buildOption, buildTarget);
            else
                BuildPipeline.BuildAssetBundles(outputPath, assetBundleList.ToArray(), buildOption, buildTarget);

            Debug.LogFormat("--- Make AssetBundle Finish, PATH : {0}, OPTION : {1}, PLATFORM : {2}, TOTAL_COUNT : {3}", outputPath, buildOption, buildTarget, assetBundleList.Count);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AssetDatabase.RemoveUnusedAssetBundleNames();

            Debug.Log("--- AssetDatabase Save & Refresh");
        }

        static void Init(string path)
        {
            // 스트리밍 애셋 폴더 있는지 검사
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // 플랫폼 폴더 있는지 검사
            path = path + Path.DirectorySeparatorChar + EditorUtility.GetPlatformName();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}