using System;
using System.Collections.Generic;
using UnityEngine;

public class PanelPrefabData : MonoBehaviour
{
    [Serializable]
    public class CachedObj
    {
        public string objName;
        public GameObject go;
    }

    [SerializeField]
    public List<CachedObj> UIObjs { get { return cachedObjs; } private set { } }
    [SerializeField]
    List<CachedObj> cachedObjs;

#if UNITY_EDITOR
    List<string> cachedUIkeyWords = new List<string>() { "LB", "GO", "IM", "BT" };

    public void FillObjs(Transform origin)
    {
        if (cachedObjs == null)
            cachedObjs = new List<CachedObj>();
        else
            cachedObjs.Clear();

        FindObjsRecv(origin);
    }

    void FindObjsRecv(Transform origin) 
    {
        Transform transform = origin;
        for(int idx = 0; idx < transform.childCount; idx++)
        {
            Transform tf = transform.GetChild(idx);
            string name = tf.name;
            if (cachedUIkeyWords.Contains(name))
            {
                CachedObj obj = new CachedObj();
                obj.objName = name;
                obj.go = tf.gameObject;
                cachedObjs.Add(obj);
            }

            FindObjsRecv(tf);
        }
    }
#endif
}
