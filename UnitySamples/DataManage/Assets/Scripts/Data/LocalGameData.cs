using System.Collections.Generic;

public class LocalGameData
{
    public DungeonData dungeon = new DungeonData();
    public EnemyData enemy = new EnemyData();

    public void LoadData(Dictionary<string, object> gameData)
    {
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

    #region Get
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
