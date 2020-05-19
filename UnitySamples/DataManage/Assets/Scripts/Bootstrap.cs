using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    public List<Text> text = new List<Text>();

    void Awake()
    {
        try
        {
            GlobalApp.Inst.Open();
        }
        catch(Exception ex)
        {
            Debug.LogErrorFormat("{0} - {1} => Msg:{2} StackTrace:{3}", ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message, ex.StackTrace);
        }
    }

    void Start()
    {
        DungeonData._DungeonData dungeon = GlobalApp.Inst.Lgd.GetDungeonData(1);
        text[0].text = "Index : " + dungeon.idx;
        text[1].text = "DungeonName : " + dungeon.name;
        text[2].text = "Difficult : " + dungeon.difficult;
        string temp = string.Empty;
        dungeon.enemies.ForEach((x) => temp += x.ToString());
        text[3].text = "EnemyList : " + temp;

        GameObject panel = Instantiate(Resources.Load("UI/SamplePanel") as GameObject);
        PanelPrefabData data = panel.GetComponent<PanelPrefabData>();
    }
}
