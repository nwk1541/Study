using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class AutoButtonListener : MonoBehaviour
{
    private void Start()
    {
        Button button = GetComponentInChildren<Button>(true);
        string compare = string.Format("OnClick{0}", button.name.Replace("Button", ""));

        Type type = typeof(AutoButtonListener);
        MethodInfo[] methodInfo = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        MethodInfo target = null;
        
        for(int idx = 0; idx < methodInfo.Length; idx++)
        {
            MethodInfo item = methodInfo[idx];
            if (item.Name == compare)
            {
                target = item;
                break;
            }
        }

        Action action = (Action)Delegate.CreateDelegate(typeof(Action), this, target);
        button.onClick.AddListener(() => action());
    }

    public void OnClickTest()
    {
        Debug.Log("OnClickTest");
    }
}
