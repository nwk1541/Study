using System.Collections.Generic;
using UnityEngine;

public class LocalGameData : MonoBehaviour
{
    public DungeonData dungeon = new DungeonData();
    public EnemyData enemy = new EnemyData();

    public void LoadData()
    {
        TextAsset textAsset = Resources.Load("Data/Data_Game") as TextAsset;
        if(textAsset == null)
        {
            Debug.LogErrorFormat("'TextAsset - Data_Game.json' is null");
            return;
        }

        Dictionary<string, object> gameData = MiniJSON.Json.Deserialize(textAsset.text) as Dictionary<string, object>;
        Load(gameData);
    }

    void Load(Dictionary<string, object> gameData)
    {
        foreach(var data in gameData)
        {
            string sheetName = data.Key;
            object datas = data.Value;

            if(sheetName == "Dungeon")
                dungeon.Open(datas);
            else if (sheetName == "Enemy")
                enemy.Open(datas);
        }
    }

    #region Get Data
    public DungeonData._DungeonData GetDungeonData(int idx)
    {
        return dungeon.dungeons.Find((x) => x.idx == idx);
    }
    #endregion

    #region Helper
    public static List<int> GetIntListFromData(object param)
    {
        List<int> result = new List<int>();
        string listData = param.ToString();
        listData = listData.Trim('[', ']');
        string[] data = listData.Split(',');

        for(int idx = 0; idx < data.Length; idx++)
            result.Add(int.Parse(data[idx]));

        return result;
    }
    #endregion
}
