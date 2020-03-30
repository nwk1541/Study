using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : Singleton<PoolMgr>
{
    [System.Serializable]
    public class ManagedObj
    {
        public GameObject go = null;
        public string name = string.Empty;
        public int objRestCount = 0; // 오브젝트가 활용되지 않은 횟수
        public bool isActive = false; // 현재 하이어라키에 활성화 되어있는지

        public ManagedObj(GameObject go, string name, bool active)
        {
            this.go = go;
            this.name = name;
            isActive = active;
        }
    }

    const int REST_COUNT_MAX = 3;

    [SerializeField]
    List<ManagedObj> loadedObjs;
    [SerializeField]
    List<GameObject> activeObjs;

    public void Open()
    {
        loadedObjs = new List<ManagedObj>();
        activeObjs = new List<GameObject>();
    }

    public void Close()
    {
        loadedObjs = null;
        activeObjs = null;

        Destroy(gameObject);
    }
    
    /// <param name="path">Resources 내에 위치한 경로</param>
    public GameObject LoadAsset(string path)
    {
        return LoadAsset(path, Vector3.zero, Quaternion.identity, Vector3.one, transform);
    }

    /// <param name="path">Resources 내에 위치한 경로</param>
    /// <param name="parent">하이어라키에서 로드하려는 오브젝트의 부모</param>
    public GameObject LoadAsset(string path, Transform parent)
    {
        return LoadAsset(path, Vector3.zero, Quaternion.identity, Vector3.one, parent);
    }

    public GameObject LoadAsset(string path, Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
    {
        // 경로의 맨 끝 이름만 추출
        string[] arrName = path.Split('/');
        string assetName = arrName[arrName.Length - 1];
        
        ManagedObj obj = loadedObjs.Find((x) => x.name == assetName && !x.isActive);
        if (obj == null)
        {
            // 로드된적이 없을경우
            obj = CreateManagedObj(assetName, path);
        }
        else
        {
            // 로드된적이 있는데 이미 활성화 되어있다면 오브젝트 다시 생성
            if (obj.isActive)
                obj = CreateManagedObj(assetName, path);
        }

        obj.isActive = true;
        obj.objRestCount = 0;

        GameObject go = obj.go;
        // 일단 비활성화 하고 사용하려는 곳에서 활성화
        go.name = assetName;
        go.SetActive(false);
        // 각 변수들 초기화
        Transform tf = go.transform;
        tf.SetParent(parent);
        tf.localPosition = position;
        tf.localRotation = rotation;
        tf.localScale = scale;

        activeObjs.Add(go);
        
        return go;
    }

    public void UnloadAssetAll()
    {
        for (int first = activeObjs.Count - 1; first >= 0; first--)
            UnloadAsset(activeObjs[first]);
    }

    public void UnloadAsset(GameObject go)
    {
        if(go == null)
        {
            Debug.LogErrorFormat("PoolMgr UnloadAsset Error, 'go' is NULL");
            return;
        }
        
        // 먼저 비활성화 후 부모 변경
        go.SetActive(false);
        go.transform.SetParent(transform);
        // 액티브 리스트에서 삭제
        activeObjs.Remove(go);

        ManagedObj obj = loadedObjs.Find((x) => x.name == go.name && x.go.GetInstanceID() == go.GetInstanceID());
        RecycleManagedObj(obj);
    }

    ManagedObj CreateManagedObj(string name, string path)
    {
        GameObject go = Instantiate((GameObject)Resources.Load(path));
        ManagedObj obj = new ManagedObj(go, name, false);
        // 새로 만들경우만 리스트에 추가
        loadedObjs.Add(obj);

        return obj;
    }

    void RecycleManagedObj(ManagedObj obj)
    {
        obj.isActive = false;
        obj.objRestCount++;
        // 특정 휴식횟수를 넘었다면 삭제
        if (obj.objRestCount > REST_COUNT_MAX)
        {
            if (loadedObjs.Remove(obj))
                Destroy(obj.go);
        }
    }
}
