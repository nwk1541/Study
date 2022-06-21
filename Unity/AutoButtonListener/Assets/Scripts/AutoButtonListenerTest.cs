using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class AutoButtonListenerTest : MonoBehaviour
{
    public Button button1;
    public Button button2;

    private Stopwatch watch = new Stopwatch();

    private void Start()
    {
        watch.Reset();
        Debug.Log("MethodInfo.Invoke");
        watch.Start();

        Type type1 = typeof(AutoButtonListenerTest);
        MethodInfo methodInfo1 = type1.GetMethod("OnClickButton1");
        button1.onClick.AddListener(() => methodInfo1.Invoke(this, null));

        watch.Stop();
        Debug.Log(watch.Elapsed.TotalMilliseconds);

        ///////////////////////////////////////////

        watch.Reset();
        Debug.Log("CreateDelegate");
        watch.Start();

        Type type2 = typeof(AutoButtonListenerTest);
        MethodInfo methodInfo2 = type2.GetMethod("OnClickButton2");
        Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, methodInfo2);
        button2.onClick.AddListener(() => action());

        watch.Stop();
        Debug.Log(watch.Elapsed.TotalMilliseconds);
    }

    public void OnClickButton1()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        Debug.Log("OnClickTest1");

        watch.Stop();
        Debug.Log(watch.Elapsed.TotalMilliseconds);
    }

    public void OnClickButton2()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        Debug.Log("OnClickTest2");

        watch.Stop();
        Debug.Log(watch.Elapsed.TotalMilliseconds);
    }

    // 인스펙터에서 직접 이벤트 연결
    public void OnClickButton3()
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        Debug.Log("OnClickTest3");

        watch.Stop();
        Debug.Log(watch.Elapsed.TotalMilliseconds);
    }
}
