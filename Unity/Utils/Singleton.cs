using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class
{
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find(typeof(T).Name);
                if (go == null)
                {
                    go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent(typeof(T)) as T;
                }
                else
                    instance = go.GetComponent<T>();
            }

            return instance;
        }
    }

    private static T instance = null;
}