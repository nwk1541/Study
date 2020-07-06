using UnityEngine;
using UnityEditor;

public class EditorPanelPrefabData
{
    public static void Fill(GameObject panel)
    {
        PanelPrefabData data = panel.GetComponent<PanelPrefabData>();
        if (data == null)
            data = panel.AddComponent<PanelPrefabData>();

        data.FillObjs(data.transform);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}