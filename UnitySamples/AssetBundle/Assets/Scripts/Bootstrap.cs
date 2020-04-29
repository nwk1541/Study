using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Start()
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/Windows.manifest");
        AssetBundleManifest manifest = bundle.LoadAsset("Windows") as AssetBundleManifest;
        string[] dependencies = manifest.GetAllDependencies("samplecube");
        foreach(string depend in dependencies)
        {
            string path = Path.Combine(Application.streamingAssetsPath + "/Windows", depend);
            AssetBundle.LoadFromFile(path);
        }
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
    }
}
