using System.Collections;
using System.Collections.Generic;

public class DungeonData
{
    public class _DungeonData
    {
        public int idx;
        public string name;
        public float difficult;
        public List<int> enemies;

        public _DungeonData(Dictionary<string, object> data)
        {
            idx = int.Parse(data["Index"].ToString());
            name = data["DungeonName"] as string;
            difficult = float.Parse(data["Difficult"].ToString());
            enemies = LocalGameData.GetIntListFromData(data["EnemyList"]);
        }
    }

    public List<_DungeonData> dungeons = new List<_DungeonData>();

    public void Open(object datas)
    {
        List<object> objs = datas as List<object>;
        for(int idx = 0; idx < objs.Count; idx++)
        {
            Dictionary<string, object> data = objs[idx] as Dictionary<string, object>;
            _DungeonData subData = new _DungeonData(data);
            dungeons.Add(subData);
        }
    }
}
