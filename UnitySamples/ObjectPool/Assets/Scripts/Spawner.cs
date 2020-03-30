using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    Transform parent;
    GameObject go;

    private void Start()
    {
        parent = transform;

        for (int first = 0; first < 10; first++)
            CreateDummy();
    }

    void CreateDummy()
    {
        GameObject dummy = PoolMgr.Inst.LoadAsset("Dummy", parent);
        dummy.AddComponent<Dummy>();
        dummy.SetActive(true);
        go = dummy;
    }

    void Clear()
    {
        PoolMgr.Inst.UnloadAssetAll();
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 50), "Create"))
        {
            CreateDummy();
        }

        if(GUI.Button(new Rect(0, 50, 100, 50), "Clear"))
        {
            Clear();
        }

        if(GUI.Button(new Rect(0, 100, 100, 50), "Change Parent"))
        {
            parent = PoolMgr.Inst.transform;
        }

        if (GUI.Button(new Rect(0, 150, 100, 50), "Remove One"))
        {
            PoolMgr.Inst.UnloadAsset(go);
        }
    }
}
