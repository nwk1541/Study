#if UNITY_EDITOR || DEVEOPMENT_BUILD
#define DEBUG
#endif

using UnityEngine;
using System.Diagnostics;

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

    [Conditional("DEBUG")]
    public static void Assert(bool cond, string format)
    {
        UnityEngine.Debug.Assert(cond, format);
    }

    [Conditional("DEBUG")]
    public static void AssertFormat(bool cond, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(cond, format, args);
    }
}