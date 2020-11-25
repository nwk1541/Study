using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
#if UNITY_EDITOR
    GUIStyle guiStyle;
    bool isHideOnGUI;

    private void Start()
    {
        guiStyle = new GUIStyle();
        guiStyle.fontSize = 24;

        isHideOnGUI = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            isHideOnGUI = !isHideOnGUI;
    }

    private void OnGUI()
    {
        if (!isHideOnGUI)
            return;

        if(GUI.Button(new Rect(0, Screen.height * 0.5f, Screen.width * 0.1f, Screen.height * 0.1f), "GetTitleData"))
        {
            PlayfabManager.GetTitleData();
        }

        if (GUI.Button(new Rect(0, Screen.height * 0.6f, Screen.width * 0.1f, Screen.height * 0.1f), "GetUserData"))
        {
            PlayfabManager.GetUserData("CFD17CF3CB375A50");
        }

        if (GUI.Button(new Rect(0, Screen.height * 0.7f, Screen.width * 0.1f, Screen.height * 0.1f), "UpdateUserData"))
        {
            PlayfabManager.SetUserData();
        }

        if (GUI.Button(new Rect(0, Screen.height * 0.8f, Screen.width * 0.1f, Screen.height * 0.1f), "HelloWorld"))
        {
            PlayfabManager.HelloWorld();
        }

        if (GUI.Button(new Rect(0f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), "Login"))
        {
            PlayfabManager.LoginPlayfab();
        }
    }
#endif
}
