using UnityEngine;

public static class TransformEx
{
    public static Transform FindChildObject(this Transform target, string name)
    {
        Transform obj = target.Find(name);

        if (obj != null)
            return obj;

        foreach (Transform child in target)
        {
            obj = child.Find(name);
            if (obj != null)
                return obj;
        }

        return null;
    }
}
