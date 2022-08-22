using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Manager : MonoBehaviour
{
    public static Manager Instance { get { return instance; } }
    private static Manager instance;
    private static AsyncOperationHandle opHandle;

    private void Awake()
    {
        instance = this;

        AsyncOperationHandle handle = Addressables.InitializeAsync();
        handle.Completed += (op) =>
        {
            opHandle = op;
        };
    }

    public void GetDownloadSize(Action<long> onComplete = null)
    {
        AsyncOperationHandle<long> handle = Addressables.GetDownloadSizeAsync("Common");
        handle.Completed += (op) =>
        {
            long downloadSize = op.Result;
            if (onComplete != null)
                onComplete(downloadSize);
        };
    }

    private void InstantiateAsset(string label, Action<GameObject> onComplete = null)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(label);
        handle.Completed += (op) =>
        {
            if(onComplete != null)
                onComplete(op.Result);
        };
    }

    private void LoadAsset(string label)
    {

    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), "Cube"))
        {
            InstantiateAsset("Cube", (go) =>
            {
                Vector3 newPos = new Vector3(-3f, 0f, 0f);
                go.transform.position = newPos;
            });
        }

        if (GUI.Button(new Rect(Screen.width * 0.1f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), "Capsule"))
        {
            InstantiateAsset("Capsule", (go) =>
            {
                Vector3 newPos = new Vector3(0f, 0f, 0f);
                go.transform.position = newPos;
            });
        }

        if (GUI.Button(new Rect(Screen.width * 0.2f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), "Sphere"))
        {
            InstantiateAsset("Sphere", (go) =>
            {
                Vector3 newPos = new Vector3(3f, 0f, 0f);
                go.transform.position = newPos;
            });
        }

        if (GUI.Button(new Rect(0f, Screen.height * 0.8f, Screen.width * 0.1f, Screen.height * 0.1f), "Catalog"))
        {
            AsyncOperationHandle<List<string>> handle = Addressables.CheckForCatalogUpdates();
            handle.Completed += (op) =>
            {
                List<string> res = op.Result;
                for (int idx = 0; idx < res.Count; idx++)
                    Debug.Log(res[idx]);
            };

            // AddressableAssetSettings 에서 Disable Catalog Update하면 따로 업데이트 함수 불러줘야 하는듯..
        }
    }
}
