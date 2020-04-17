using UnityEditor;

public class ProjectEditorMenu
{
    [MenuItem("Tools/Convert GameData")]
    public static void ConvertGameData()
    {
        DataConvert.Load();
    }
}
