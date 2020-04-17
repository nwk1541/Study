using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DataConvert
{
    readonly static string GAME_DATA = "Data_Game.json";

    public static void Load()
    {
        Dictionary<string, List<object>> result = new Dictionary<string, List<object>>();

        string[] files = Directory.GetFiles(Application.dataPath + "/Data/", "*.json");

        for (int idx = 0; idx < files.Length; idx++)
        {
            string path = files[idx];
            string[] splitStr = files[idx].Split('/');
            string name = splitStr[splitStr.Length - 1];
            name = name.Substring(0, name.Length - 5);

            result.Add(name, new List<object>());
            List<object> origin = result[name];

            FileStream stream = new FileStream(path, FileMode.Open);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            string jsonData = Encoding.UTF8.GetString(data);
            List<object> objs = MiniJSON.Json.Deserialize(jsonData) as List<object>;
            origin.AddRange(objs);
        }

        string json = MiniJSON.Json.Serialize(result);
        string dataPath = Application.dataPath + "/Resources/Data/" + GAME_DATA;
        File.WriteAllText(dataPath, json);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
