using UnityEditor;

public class ProjectEditorMenu
{
    [MenuItem("Tools/Convert GameData")]
    public static void ConvertGameData()
    {
        DataConvert.Convert();
    }

    [MenuItem("Assets/Tools/Fill PanelPrefabData")]
    public static void FillPanelPrefabData()
    {
        EditorPanelPrefabData.Fill(Selection.activeGameObject);
    }
}
