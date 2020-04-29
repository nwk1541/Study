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

            string targetPath = Application.dataPath + Path.DirectorySeparatorChar + "Resources/Prefabs";
            PreProcessOnBuild(targetPath);

            List<AssetBundleBuild> assetBundleList = new List<AssetBundleBuild>();
            for (int idx = 0; idx < objs.Length; idx++)
            {
                Object obj = objs[idx];
                string assetPath = AssetDatabase.GetAssetPath(obj);
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null)
                {
                    Debug.LogErrorFormat("AssetImporter is NULL, NAME : {0}, PATH : {1}", obj.name, assetPath);
                    continue;
                }

                string bundleName = importer.assetBundleName;
                string bundleVariant = ASSET_BUNDLE_VARIANT;

                AssetBundleBuild build = new AssetBundleBuild();
                build.assetBundleName = bundleName;
                build.assetBundleVariant = bundleVariant;
                build.assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName + "." + bundleVariant);
                assetBundleList.Add(build);
            }

            string outputPath = Application.streamingAssetsPath + Path.DirectorySeparatorChar + EditorUtility.GetPlatformName();
            AssetBundleManifest manifest;

            //if (assetBundleList.Count == 0)
                //manifest = BuildPipeline.BuildAssetBundles(outputPath, buildOption, buildTarget);
            //else
                //manifest = BuildPipeline.BuildAssetBundles(outputPath, assetBundleList.ToArray(), buildOption, buildTarget);

            // TODO : Write AssetBundleManifest 
            //File.WriteAllText(outputPath);
            Debug.LogFormat("--- Make AssetBundle Finish, PATH : {0}, OPTION : {1}, PLATFORM : {2}, TOTAL_COUNT : {3}", outputPath, buildOption, buildTarget, assetBundleList.Count);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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

        static void PreProcessOnBuild(string targetPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:Object", new string[] { "Assets/Resources/Prefabs" });

            for (int idx = 0; idx < guids.Length; idx++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[idx]);
                AssetImporter importer = AssetImporter.GetAtPath(path);
                importer.assetBundleName = importer.name;
                importer.assetBundleVariant = ASSET_BUNDLE_VARIANT;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}