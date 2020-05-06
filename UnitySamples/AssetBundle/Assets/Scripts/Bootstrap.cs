using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {

    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 100), "LoadCube"))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/samplecube.unity3d");
            GameObject go = bundle.LoadAsset("samplecube") as GameObject;
            Instantiate(go);
        }

        if (GUI.Button(new Rect(0, 100, 100, 100), "LoadCube2"))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/samplecube2.unity3d");
            GameObject go = bundle.LoadAsset("samplecube2") as GameObject;
            Instantiate(go);
        }

        if(GUI.Button(new Rect(0, 200, 100, 100), "LoadDependency"))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/Windows");
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies("samplecube.unity3d");
            foreach (string depend in dependencies)
            {
                string path = Path.Combine(Application.streamingAssetsPath + "/Windows", depend);
                AssetBundle.LoadFromFile(path);
            }
        }

        if(GUI.Button(new Rect(0, 300, 100, 100), "LoadPanel"))
        {
            //AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/")
        }
    }
}
