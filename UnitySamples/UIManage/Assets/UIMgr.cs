using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    public enum PanelType
    {
        Title,
        Lobby,
        Result
    }

    const string CANVAS_NAME = "UICanvas";

    public PanelType CurrentPanel { get; private set; }
    public GameObject Canvas { get; private set; }

    PanelType prevPanel;

    public void Open()
    {
        Canvas = transform.Find(CANVAS_NAME).gameObject;
    }

    public void Close()
    {
        Canvas = null;
    }

    public void ShowPanel()
    {

    }

    public void HidePanel()
    {

    }

    //public PanelBase GetPanel(PanelType type)
    //{

    //}
}
