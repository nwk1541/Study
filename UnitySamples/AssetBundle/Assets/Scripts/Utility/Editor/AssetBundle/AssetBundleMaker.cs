using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Util.Editor
{
    public class AssetBundleMaker
    {
        static readonly string ASSET_BUNDLE_VARIANT = "unity3d";

        public static void Make(Object[] objs)
        {
            BuildAssetBundles(objs);
        }

        static void BuildAssetBundles(Object[] objs)
        {
            string outputPath = Application.streamingAssetsPath;

            Init(outputPath);

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
            }

            AssetBundleManifest manifest;

            if(assetBundleList.Count == 0)
                manifest = BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            else
                manifest = BuildPipeline.BuildAssetBundles(outputPath, assetBundleList.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);

            // TEST: ssdas
            //File.WriteAllText(outputPath);
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