using UnityEngine;

public class GlobalApp : Singleton<GlobalApp>
{
    public LocalGameData Lgd { get; private set; }
    // TODO : public RemoteGameData Rgd { get; private set; }

    public void Open()
    {
        TextAsset textAsset = Resources.Load("Data/Data_Game") as TextAsset;
        Lgd = JsonUtility.FromJson<LocalGameData>(textAsset.text);
    }
}
