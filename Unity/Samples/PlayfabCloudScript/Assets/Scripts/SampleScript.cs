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
        guiStyle.fontSize = 28;

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

        if (GUI.Button(new Rect(0, Screen.height * 0.8f, Screen.width * 0.1f, Screen.height * 0.1f), "HelloWorld"))
        {
            PlayfabManager.OnHelloWorld();
        }

        if (GUI.Button(new Rect(0f, Screen.height * 0.9f, Screen.width * 0.1f, Screen.height * 0.1f), "Login"))
        {
            PlayfabManager.OnLoginPlayfab();
        }
    }
#endif
}
