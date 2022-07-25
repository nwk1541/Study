using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Assets
{
    NONE,
    RESOURCES,
    ASSETBUNDLE
}

public class AssetManager : MonoBehaviour
{
    public static AssetManager Instance { get { return instance; } }
    private static AssetManager instance = null;

    public Assets current;

    private void Awake()
    {
        instance = this;

        current = Assets.RESOURCES;
    }

    public void SwitchLoadType(Assets next)
    {
        current = next;
    }

    public T LoadAsset<T>(string path) where T : Object
    {
        T loadedObj = null;

        switch (current)
        {
            case Assets.RESOURCES:
                loadedObj = LoadResourcesAsset<T>(path);
                break;
            case Assets.ASSETBUNDLE:
                // TODO :
                break;
        }

        return loadedObj;
    }

    public T LoadResourcesAsset<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
