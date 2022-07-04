using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance = null;

    private Dictionary<string, Queue<GameObject>> pools = null;

    private void Awake()
    {
        Instance = this;

        pools = new Dictionary<string, Queue<GameObject>>();
    }

    public GameObject Spawn(string name, Transform parent = null)
    {
        return Spawn(name, Vector3.zero, Quaternion.identity, parent);
    }

    public T Spawn<T>(string name, Transform parent = null)
    {
        T spawnedItem = default(T);

        GameObject go = Spawn(name, Vector3.zero, Quaternion.identity, parent);
        spawnedItem = go.GetComponent<T>();

        return spawnedItem;
    }

    public GameObject Spawn(string name, Vector3 pos, Quaternion rotate, Transform parent = null)
    {
        GameObject go = null;
        go = Resources.Load<GameObject>("Prefabs/" + name);
        go = Instantiate(go, pos, rotate, parent);

        return go;
    }

    public T SpawnFromPool<T>(string name, Vector3 pos, Transform parent)
    {
        return SpawnFromPool<T>(name, pos, Quaternion.identity, Vector3.one, parent);
    }

    public T SpawnFromPool<T>(string name, Vector3 pos)
    {
        return SpawnFromPool<T>(name, pos, Quaternion.identity, Vector3.one);
    }

    public T SpawnFromPool<T>(string name, Transform parent = null)
    {
        return SpawnFromPool<T>(name, Vector3.zero, Quaternion.identity, Vector3.one, parent);
    }

    public T SpawnFromPool<T>(string name, Vector3 pos, Quaternion rotate, Vector3 scale, Transform parent = null)
    {
        T poolItem = default(T);

        GameObject go = SpawnFromPool(name, pos, rotate, scale, parent);
        poolItem = go.GetComponent<T>();

        return poolItem;
    }

    public GameObject SpawnFromPool(string name, Vector3 pos, Quaternion rotate, Vector3 scale, Transform parent = null)
    {
        if (!pools.ContainsKey(name))
            pools.Add(name, new Queue<GameObject>());

        Queue<GameObject> itemQueue = pools[name];
        GameObject poolItem = null;

        if (itemQueue.Count <= 0)
            poolItem = Spawn(name, Vector3.zero, Quaternion.identity);
        else
            poolItem = itemQueue.Dequeue();

        poolItem.transform.SetParent(parent);
        poolItem.transform.position = pos;
        poolItem.transform.rotation = rotate;
        poolItem.transform.localScale = scale == default(Vector3) ? Vector3.one : scale;
        poolItem.name = name;
        poolItem.SetActive(true);

        IPooledObject pooledObject = poolItem.GetComponent<IPooledObject>();
        if (pooledObject != null)
            pooledObject.OnSpawn();

        return poolItem;
    }

    public void ReturnToPool(GameObject go)
    {
        string name = go.name;
        Queue<GameObject> itemQueue = pools[name];
        itemQueue.Enqueue(go);

        go.SetActive(false);
        go.transform.SetParent(transform);
    }
}
