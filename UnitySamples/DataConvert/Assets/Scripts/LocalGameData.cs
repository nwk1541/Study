using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameData : MonoBehaviour
{
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

    }
}
