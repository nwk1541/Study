using System.Collections.Generic;
using UnityEngine;

public enum UI_PANEL
{
    Main,
    Sample1,
    Sample2,
    Sample3,
    Sample4,
}

public class UIMgr : Singleton<UIMgr>
{
    [System.Serializable]
    class UIPanelParam
    {
        public GameObject panelObj;
        public UI_PANEL panelType;
    }

    readonly string PANEL_VAR = "Panel";
    readonly string CANVAS = "UI/UICanvas";

    public UI_PANEL Current { get; private set; }

    [SerializeField]
    List<UIPanelParam> managedUIObjs;
    [SerializeField]
    Transform Canvas { get; set; }
    [SerializeField]
    UI_PANEL prevPanel;

    public void Open()
    {
        if (managedUIObjs == null)
            managedUIObjs = new List<UIPanelParam>();

        Canvas = PoolMgr.Inst.LoadAsset(CANVAS, transform).transform;
        Canvas.gameObject.SetActive(true);

        prevPanel = UI_PANEL.Main;
    }

    public void Close()
    {
        managedUIObjs = null;
        Canvas = null;
    }

    public void ShowPanel(UI_PANEL type)
    {
        // 패널 오브젝트 가져오기, 없다면 풀매니저에게 요청해서 생성
        string panelName = PANEL_VAR + type.ToString();
        UIPanelParam obj = GetPanel(panelName, type);
        if (obj == null)
        {
            Debug.LogErrorFormat("Can't find 'ManagedUIObj', {0}", type);
            return;
        }

        // 패널 스크립트 가져와서 오버라이드 함수 실행하여 초기화
        PanelBase script = obj.panelObj.GetComponent<PanelBase>();
        if (script == null)
        {
            Debug.LogErrorFormat("Panel Script is null, {0}", type);
            return;
        }

        prevPanel = Current;
        Current = type;

        if (!script.isLoaded)
            script.Init();

        script.OpenPanel();

        obj.panelObj.SetActive(true);
    }

    public void ClosePanel()
    {
        if (Current == UI_PANEL.Main)
            return;

        UIPanelParam uiObj = managedUIObjs.Find((x) => x.panelType == Current);
        if (uiObj == null)
        {
            Debug.LogErrorFormat("Can't find 'ManagedUIOBj', {0}", Current);
            return;
        }

        uiObj.panelObj.SetActive(false);
    }

    UIPanelParam GetPanel(string name, UI_PANEL type)
    {
        GameObject panelObj = null;
        // 리스트에서 찾았는데 없으면 생성
        UIPanelParam uiObj = managedUIObjs.Find((x) => x.panelType == type);
        if (uiObj == null)
        {
            string path = string.Format("UI/{0}", name);
            panelObj = PoolMgr.Inst.LoadAsset(path, Canvas);
            // ManagedUIObj 생성
            uiObj = new UIPanelParam();
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
        // RectTransform 초기화
        RectTransform rect = objTf.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // 최상위로 보일수 있도록
        objTf.SetSiblingIndex(managedUIObjs.Count - 1);

        return uiObj;
    }
}
