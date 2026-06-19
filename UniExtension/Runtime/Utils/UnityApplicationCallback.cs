#nullable enable

using System;
using UnityEngine;
using Unity.Scripting.LifecycleManagement;

namespace UniExtension
{
public static class UnityApplicationCallback
{
    [field: AutoStaticsCleanup]
    public static event Action? LowMemory;

    static UnityApplicationCallback()
    {
        Application.lowMemory += OnLowMemory;
    }

    public static bool IsPlaying
    {
        get
        {
#if UNITY_EDITOR
            return UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
#else
            return Application.isPlaying;
#endif
        }
    }

    private static void OnLowMemory()
    {
        LowMemory?.Invoke();
    }
}
}
