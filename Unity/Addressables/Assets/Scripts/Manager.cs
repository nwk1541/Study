using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Networking;
using System.IO;
using TestCDN;
using UnityEngine.AddressableAssets.Initialization;

namespace TestCDN
{
    public class CDNInfo
    {
        public static string URL { get; set; }
        public static string Version { get; set; }
    }
}

public class Manager : MonoBehaviour
{
    public static Manager Instance { get { return instance; } }
    private static Manager instance;

    public TextMeshProUGUI sizeText;

    private List<string> objectLabels = new List<string>() { "Cube", "Sphere", "Capsule" };
    private List<float> objectXPos = new List<float>() { -5f, 0f, 5f };
    private string versionText = string.Empty;
    private List<string> catalogsToUpdate = new List<string>();

    private static IResourceLocator resLocator;

    private void Awake()
    {
        instance = this;
        
        CDNInfo.URL = "https://my-addr-test.s3.ap-northeast-2.amazonaws.com";
    }

    public void Initalize()
    {
        AsyncOperationHandle<IResourceLocator> handle = Addressables.InitializeAsync();
        handle.Completed += (op) =>
        {
            resLocator = op.Result;
        };
    }

    public void DownloadCatalog()
    {
        StartCoroutine(CoDownloadCatalog());
    }

    public IEnumerator CoDownloadCatalog()
    {
        string targetURL = string.Format("{0}/{1}/{2}/", CDNInfo.URL, "StandaloneWindows64", CDNInfo.Version);
        string catalogJsonName = string.Format("catalog_{0}.json", CDNInfo.Version);
        string catalogHashName = string.Format("catalog_{0}.hash", CDNInfo.Version);

        UnityWebRequest req = new UnityWebRequest(targetURL + catalogJsonName, UnityWebRequest.kHttpVerbGET);

        string destPath = Path.Combine(Application.persistentDataPath, catalogJsonName);
        req.downloadHandler = new DownloadHandlerFile(destPath);

        Debug.LogFormat("UnityWebRequest GET : {0}, {1}", targetURL, catalogJsonName);

        yield return req.SendWebRequest();

        Debug.LogFormat("UnityWebRequest : {0}", req.result);

        req = new UnityWebRequest(targetURL + catalogHashName, UnityWebRequest.kHttpVerbGET);

        destPath = Path.Combine(Application.persistentDataPath, catalogHashName);
        req.downloadHandler = new DownloadHandlerFile(destPath);

        yield return req.SendWebRequest();

        Debug.LogFormat("UnityWebRequest : {0}", req.result);
        Debug.LogFormat("Download Catalog Complete");
    }

    public void DownloadAssets(string label, Action onComplete = null)
    {
        AsyncOperationHandle handle = Addressables.DownloadDependenciesAsync(label);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("DownloadAssets : {0}, {1}", op.Status, op.Result);
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                if (onComplete != null)
                    onComplete();
            }

            Addressables.Release(handle);
        };
    }

    public void GetDownloadSize(string label, Action<long> onComplete = null)
    {
        AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync(label);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("GetDownloadSizeAsync : {0}, {1}", op.Status, op.Result);

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                long downloadSize = op.Result;
                if (onComplete != null)
                    onComplete(downloadSize);
            }

            Addressables.Release(handle);
        };
    }

    public void InstantiateAsset(string label, Action<GameObject> onComplete = null)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(label);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("InstantiateAsync : {0}, {1}", op.Status, op.Result);

            if(op.Status == AsyncOperationStatus.Succeeded)
            {
                if (onComplete != null)
                    onComplete(op.Result);
            }
        };
    }

    public void CheckForCatalogUpdates(Action onComplete = null)
    {
        catalogsToUpdate.Clear();

        AsyncOperationHandle<List<string>> handle = Addressables.CheckForCatalogUpdates(false);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("CheckForCatalogUpdates : {0}, {1}", op.Status, op.Result);

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                List<string> res = op.Result;
                foreach (var item in res)
                    Debug.Log(item);

                if (res != null && res.Count > 0)
                    catalogsToUpdate.AddRange(res);
            }

            Addressables.Release(handle);
        };
    }

    public void UpdateCatalog(Action onComplete = null)
    {
        if(catalogsToUpdate.Count <= 0)
        {
            Debug.Log("Not Exist Catalog Update List");
            return;
        }

        AsyncOperationHandle<List<IResourceLocator>> handle = Addressables.UpdateCatalogs(catalogsToUpdate, false);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("UpdateCatalog : {0}, {1}", op.Status, op.Result);

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                List<IResourceLocator> res = op.Result;
                if (res != null && res.Count > 0)
                {
                    catalogsToUpdate.Clear();
                }

                if (onComplete != null)
                    onComplete();
            }

            Addressables.Release(handle);
        };
    }

    public void LoadCatalog(string ext, Action onComplete = null)
    {
        string catalogJsonName = string.Format("catalog_{0}.{1}", CDNInfo.Version, ext);
        string destPath = string.Format("https://my-addr-test.s3.ap-northeast-2.amazonaws.com/StandaloneWindows64/{0}/{1}", CDNInfo.Version, catalogJsonName);

        AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync(destPath);
        handle.Completed += (op) =>
        {
            Debug.LogFormat("LoadCatalog : {0}, {1}", op.Status, op.Result);

            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                if (op.Result != null)
                {
                    resLocator = op.Result;
                    catalogsToUpdate.Add(resLocator.LocatorId);
                }

                if (onComplete != null)
                    onComplete();
            }

            Addressables.Release(handle);
        };
    }

    private void OnGUI()
    {
        float guiWidth = Screen.width * 0.1f;
        float guiHeight = Screen.height * 0.1f;
        Rect guiPos = new Rect(Screen.width * 0.9f, Screen.height * 0.9f, guiWidth, guiHeight);
        versionText = GUI.TextField(guiPos, versionText, 25);

        guiPos = new Rect(0f, Screen.height * 0.9f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "SetVersion"))
        {
            CDNInfo.Version = versionText;
            Debug.LogFormat("SetVersion : {0}", CDNInfo.Version);
        }

        guiPos = new Rect(Screen.width * 0.1f, Screen.height * 0.9f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "Spawn"))
        {
            for(int idx = 0; idx < objectLabels.Count; idx++)
            {
                string label = objectLabels[idx];
                float xPos = objectXPos[idx];
                InstantiateAsset(label, (go) =>
                {
                    Vector2 newPos = new Vector2(xPos, 0f);
                    go.transform.position = newPos;
                });
            }
        }

        guiPos = new Rect(0f, Screen.height * 0.8f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "GetSize"))
        {
            GetDownloadSize("Common", (res) => sizeText.text = res.ToString());
        }

        guiPos = new Rect(Screen.width * 0.1f, Screen.height * 0.8f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "DownloadAssets"))
        {
            DownloadAssets("Common");
        }

        guiPos = new Rect(0f, Screen.height * 0.7f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "DownloadCatalog"))
        {
            DownloadCatalog();
        }

        guiPos = new Rect(0f, Screen.height * 0.6f, guiWidth, guiHeight);
        if(GUI.Button(guiPos, "CheckCatalog"))
        {
            CheckForCatalogUpdates();
        }

        guiPos = new Rect(Screen.width * 0.1f, Screen.height * 0.6f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "UpdateCatalog"))
        {
            UpdateCatalog();
        }

        guiPos = new Rect(0f, Screen.height * 0.5f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "Clear"))
        {
            resLocator = null;
            Addressables.ClearResourceLocators();
        }

        guiPos = new Rect(Screen.width * 0.1f, Screen.height * 0.5f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "Clear2"))
        {
            AddressablesRuntimeProperties.ClearCachedPropertyValues();
        }

        guiPos = new Rect(0f, Screen.height * 0.4f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "LoadCatalogJson"))
        {
            LoadCatalog("json");
        }

        guiPos = new Rect(Screen.width * 0.1f, Screen.height * 0.4f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "LoadCatalogHash"))
        {
            LoadCatalog("hash");
        }

        guiPos = new Rect(0f, Screen.height * 0.3f, guiWidth, guiHeight);
        if (GUI.Button(guiPos, "Init"))
        {
            Initalize();
        }
    }
}
