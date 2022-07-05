using System.Diagnostics;

public static class Debug
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object msg)
    {
        UnityEngine.Debug.Log(msg);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(object msg, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(msg, context);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object msg)
    {
        UnityEngine.Debug.LogError(msg);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogError(object msg, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(msg, context);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object msg)
    {
        UnityEngine.Debug.LogWarning(msg);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(object msg, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(msg, context);
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool cond, string format)
    {
        UnityEngine.Debug.Assert(cond, format);
    }

    [Conditional("UNITY_EDITOR")]
    public static void AssertFormat(bool cond, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(cond, format, args);
    }
}