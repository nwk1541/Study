using System.IO;
using UnityEditor;
using UnityEngine;

namespace Util.Editor
{
    public class AssetBundleMaker
    {
        public static void Make()
        {
            try
            {

            }
            catch
            {

            }
        }

        void BuildAssetBundles(AssetBundleBuild[] assetList = null)
        {
            string outputPath = Application.streamingAssetsPath;

            Init(outputPath);

            // TODO : Fill AssetBundleBuild List...

            BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        }

        void Init(string path)
        {
            // 스트리밍 애셋 폴더 있는지 검사
            if (Directory.Exists(path))
                Directory.CreateDirectory(path);

            // 플랫폼 폴더 있는지 검사
            path = path + Path.DirectorySeparatorChar + EditorUtility.GetPlatformName();
            if (Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}