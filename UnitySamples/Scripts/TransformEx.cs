using UnityEngine;

public static class TransformEx
{
    public static Transform FindChildObject(this Transform target, string name)
    {
        Transform tf = target.Find(name);

        if (tf != null)
            return tf;

        foreach (Transform child in target)
        {
            tf = child.Find(name);
            if (tf != null)
                return tf;
        }

        return null;
    }

    public static Transform FindChildObject(this Transform target, string name, bool includeInactive = true)
    {
        Transform[] tf = parent.GetComponentsInChildren<Transform>(includeInactive);
        foreach (Transform t in tf)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }

        return null;
    }
}
