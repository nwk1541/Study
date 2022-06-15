using UnityEngine;

public static class TransformEx
{
    public static Transform FindChildRecursively(this Transform self, string name)
    {
        Transform child = null;
        for (int idx = 0; idx < self.childCount; idx++)
        {
            child = self.GetChild(idx);
            if (child.name == name)
                break;
            else
            {
                child = FindChildRecursively(child, name);
                if (child != null)
                    break;
            }
        }

        return child;
    }
}