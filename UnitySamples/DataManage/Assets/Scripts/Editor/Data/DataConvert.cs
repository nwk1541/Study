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
        // 재 가공한 데이터
        Dictionary<string, object> representData = new Dictionary<string, object>();

        string[] files = Directory.GetFiles(Application.dataPath + "/Data/", "*.json");
        // .json 파일들 모두 수집해서 하나의 json 파일로 가공
        for (int idx = 0; idx < files.Length; idx++)
        {
            string path = files[idx];
            string[] splitStr = files[idx].Split('/');
            string name = splitStr[splitStr.Length - 1];
            name = name.Substring(0, name.Length - 5);
            // 데이터의 Key는 시트의 이름이 된다.
            representData.Add(name, new List<object>());

            FileStream stream = new FileStream(path, FileMode.Open);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();

            string jsonData = Encoding.UTF8.GetString(data);
            representData[name] = MiniJSON.Json.Deserialize(jsonData) as List<object>;
        }

        // 로컬게임데이터 클래스의 내부 데이터를 채우고 json으로 만든다.
        LocalGameData lgd = new LocalGameData();
        lgd.LoadData(representData);

        string json = JsonUtility.ToJson(lgd, true);
        string dataPath = Application.dataPath + "/Resources/Data/" + GAME_DATA;
        File.WriteAllText(dataPath, json);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
