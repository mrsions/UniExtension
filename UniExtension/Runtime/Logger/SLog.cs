#nullable enable

using System;
using UnityEngine;

namespace UniExtension
{
public static class SLog
{
    public static void TraceCtx(UnityEngine.Object context, string message)
    {
        if (!SLogger.IsDebug) return;
        Debug.Log(message, context);
    }


    public static void DebugCtx(UnityEngine.Object context, string message)
    {
        if (!SLogger.IsDebug) return;
        Debug.Log(message, context);
    }

    public static void DebugCtx(UnityEngine.Object context, string tag, string message)
    {
        if (!SLogger.IsDebug) return;
        Debug.Log($"[{tag}] {message}", context);
    }

    public static void Exception(Exception exception)
    {
        Debug.LogException(exception);
    }
}
}
