using System.Collections.Generic;
using UnityEngine;

public enum UI_PANEL
{
    MAIN,
    SAMPLE1,
    SAMPLE2,
    SAMPLE3,
    SAMPLE4,
}

public class UIMgr : Singleton<UIMgr>
{
    [System.Serializable]
    class ManagedUIObj
    {
        public GameObject panelObj;
        public UI_PANEL panelType;
    }

    readonly string PANEL_VAR = "Panel";
    readonly string CANVAS = "UI/UICanvas";

    public UI_PANEL Current { get; private set; }

    [SerializeField]
    List<ManagedUIObj> managedUIObjs;
    [SerializeField]
    Transform Canvas { get; set; }
    [SerializeField]
    UI_PANEL prevPanel;

    public void Open()
    {
        if (managedUIObjs == null)
            managedUIObjs = new List<ManagedUIObj>();

        Canvas = PoolMgr.Inst.LoadAsset(CANVAS, transform).transform;
        Canvas.gameObject.SetActive(true);

        prevPanel = UI_PANEL.MAIN;
    }

    public void Close()
    {
        managedUIObjs = null;
        Canvas = null;
    }

    public void ShowPanel(UI_PANEL type)
    {
        if (prevPanel != UI_PANEL.MAIN)
            ClosePanel();

        string panelName = PANEL_VAR + type.ToString();
        ManagedUIObj obj = ShowPanel(panelName, type);
        if(obj == null)
        {
            Debug.LogErrorFormat("Can't find 'ManagedUIObj', {0}", type);
            return;
        }

        PanelBase script = obj.panelObj.GetComponent<PanelBase>();
        if(script == null)
        {
            Debug.LogErrorFormat("Panel Script is null, {0}", type);
            return;
        }

        if (!script.isLoaded)
            script.Init();

        script.OpenPanel();

        Current = type;
        obj.panelObj.SetActive(true);
    }

    public void ClosePanel()
    {
        ManagedUIObj uiObj = managedUIObjs.Find((x) => x.panelType == Current);
        if(uiObj == null)
        {
            Debug.LogErrorFormat("Can't find 'ManagedUIOBj', {0}", Current);
            return;
        }

        uiObj.panelObj.SetActive(false);
    }

    ManagedUIObj ShowPanel(string name, UI_PANEL type)
    {
        GameObject panelObj = null;
        ManagedUIObj uiObj = managedUIObjs.Find((x) => x.panelType == type);
        if (uiObj == null)
        {
            string path = string.Format("UI/{0}", name);
            panelObj = PoolMgr.Inst.LoadAsset(path, Canvas);
            // ManagedUIObj 생성
            uiObj = new ManagedUIObj();
            uiObj.panelObj = panelObj;
            uiObj.panelType = type;
            managedUIObjs.Add(uiObj);
        }
        else
            panelObj = uiObj.panelObj;

        // Transform 초기화
        Transform objTf = panelObj.transform;
        objTf.localPosition = Vector3.zero;
        objTf.transform.localScale = Vector3.one;

        return uiObj;
    }
}
