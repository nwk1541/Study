using System;
using System.Collections.Generic;

[Serializable]
public class DungeonData
{
    [Serializable]
    public class _DungeonData
    {
        public int idx;
        public string name;
        public float difficult;
        public List<int> enemies;

        public _DungeonData(Dictionary<string, string> data)
        {
            idx = int.Parse(data["Index"]);
            name = data["DungeonName"];
            difficult = float.Parse(data["Difficult"]);
            enemies = LocalGameData.GetIntListFromData(data["EnemyList"]);
        }
    }

    public List<_DungeonData> dungeons = new List<_DungeonData>();

    public void Open(object datas)
    {
        List<object> objs = datas as List<object>;
        for(int idx = 0; idx < objs.Count; idx++)
        {
            Dictionary<string, object> dicObj = objs[idx] as Dictionary<string, object>;
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach(var tmp in dicObj)
            {
                string objVal = tmp.Value.ToString();
                data.Add(tmp.Key, objVal);
            }

            _DungeonData subData = new _DungeonData(data);
            dungeons.Add(subData);
        }
    }
}
