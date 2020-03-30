using UnityEngine;

public static class GameObjectEx
{
    public static GameObject FindObject(this GameObject parent, string name, bool includeInactive = true)
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
