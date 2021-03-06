﻿using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    static T instance;

    public static T Inst
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    GameObject go = new GameObject(typeof(T).ToString(), typeof(T));
                    instance = go.GetComponent<T>();
                    if (instance == null)
                        instance = go.AddComponent<T>();

                    DontDestroyOnLoad(instance);
                }
            }

            return instance;
        }
    }
}
