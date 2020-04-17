using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    public List<Text> text = new List<Text>();

    void Start()
    {
        LocalGameData lgd = new LocalGameData();
        lgd.LoadData();

        DungeonData._DungeonData dungeon = lgd.GetDungeonData(1);
        text[0].text = "Index : " + dungeon.idx;
        text[1].text = "DungeonName : " + dungeon.name;
        text[2].text = "Difficult : " + dungeon.difficult;
        string temp = string.Empty;
        dungeon.enemies.ForEach((x) => temp += x.ToString());
        text[3].text = "EnemyList : " + temp;
    }
}
