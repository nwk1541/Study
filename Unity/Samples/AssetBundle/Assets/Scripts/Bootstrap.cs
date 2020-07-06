using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

public class Bootstrap : MonoBehaviour
{
    public Transform canvas;

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
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/samplepanel.unity3d");
            GameObject go = bundle.LoadAsset("samplepanel") as GameObject;
            go = Instantiate(go);

            go.transform.SetParent(canvas.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        if (GUI.Button(new Rect(0, 400, 100, 100), "LoadPanel2"))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/samplepanel2.unity3d");
            GameObject go = bundle.LoadAsset("samplepanel2") as GameObject;
            go = Instantiate(go);

            go.transform.SetParent(canvas.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        if (GUI.Button(new Rect(0, 500, 100, 100), "LoadPanelDependency"))
        {
            AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/Windows/Windows");
            AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies("samplepanel.unity3d");
            foreach (string depend in dependencies)
            {
                string path = Path.Combine(Application.streamingAssetsPath + "/Windows", depend);
                AssetBundle tmp = AssetBundle.LoadFromFile(path);
            }
        }
    }
}
