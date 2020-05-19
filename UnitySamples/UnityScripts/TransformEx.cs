using UnityEngine;

public static class TransformEx
{
    public static Transform FindChildObj(this Transform target, string name)
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

    public static Transform FindChildObj(this Transform target, string name, bool includeInactive = true)
    {
        Transform[] childs = target.GetComponentsInChildren<Transform>(includeInactive);
        foreach (Transform tf in childs)
        {
            if (tf.name == name)
                return tf;
        }

        return null;
    }
}
