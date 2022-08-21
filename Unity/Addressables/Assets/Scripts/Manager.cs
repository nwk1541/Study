using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Manager : MonoBehaviour
{
    private static AsyncOperationHandle opHandle;

    private void Awake()
    {
        AsyncOperationHandle handle = Addressables.InitializeAsync();
        handle.Completed += (op) =>
        {
            opHandle = op;
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

#if UNITY_EDITOR
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
    }
#endif
}
