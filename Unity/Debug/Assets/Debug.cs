#if UNITY_EDITOR || DEVEOPMENT_BUILD
#define DEBUG
#endif

using UnityEngine;
using System.Diagnostics;

/*
 * UnityEngine.Debug를 대체하는 Debug 클래스
 * 유니티 에디터와 Deveopment Build 일때만 동작한다.
 */

public static class Debug
{
    [Conditional("DEBUG")]
    public static void Log(object msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    [Conditional("DEBUG")]
    public static void Log(object msg, Object context)
    {
        UnityEngine.Debug.Log(msg, context);
    }

    [Conditional("DEBUG")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    [Conditional("DEBUG")]
    public static void LogError(object msg)
    {
        UnityEngine.Debug.LogError(msg);
    }

    [Conditional("DEBUG")]
    public static void LogError(object msg, Object context)
    {
        UnityEngine.Debug.LogError(msg, context);
    }

    [Conditional("DEBUG")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(object msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }

    [Conditional("DEBUG")]
    public static void LogWarning(object msg, Object context)
    {
        UnityEngine.Debug.LogWarning(msg, context);
    }

    [Conditional("DEBUG")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }
}