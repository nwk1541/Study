using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private void Start()
    {
        UIMgr.Inst.Open();

        UIMgr.Inst.ShowPanel(UI_PANEL.MAIN);
    }

    private void OnGUI()
    {
        if(GUI.Button(new Rect(0, 0, 100, 100), "Sample1"))
        {
            UIMgr.Inst.ShowPanel(UI_PANEL.SAMPLE1);
        }

        if (GUI.Button(new Rect(0, 100, 100, 100), "Sample2"))
        {
            UIMgr.Inst.ShowPanel(UI_PANEL.SAMPLE1);
        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "Sample3"))
        {
            UIMgr.Inst.ShowPanel(UI_PANEL.SAMPLE1);
        }

        if (GUI.Button(new Rect(0, 300, 100, 100), "Sample4"))
        {
            UIMgr.Inst.ShowPanel(UI_PANEL.SAMPLE1);
        }

        if (GUI.Button(new Rect(0, 400, 100, 100), "ClosePanel"))
        {
            UIMgr.Inst.ClosePanel();
        }
    }
}
