#nullable enable

using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UniExtension
{
    public interface IObjectLevelPoolTrimmable
    {
        void TrimForLowMemory();
    }

    public static class PoolTrimRegistry
    {
    [AutoStaticsCleanup]
    private static readonly object s_Lock = new();
    [AutoStaticsCleanup]
    private static readonly List<WeakReference<IObjectLevelPoolTrimmable>> s_Pools = new();

    static PoolTrimRegistry()
    {
        UnityApplicationCallback.LowMemory += TrimAll;
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        UnityApplicationCallback.LowMemory += TrimAll;
    }

#if UNITY_EDITOR
    private static void OnPlayModeStateChanged(PlayModeStateChange change)
    {
        TrimAll();
    }
#endif

    public static void Register(IObjectLevelPoolTrimmable pool)
    {
        lock (s_Lock)
        {
            s_Pools.Add(new WeakReference<IObjectLevelPoolTrimmable>(pool));
        }
    }

    public static void TrimAll()
    {
        lock (s_Lock)
        {
            for (int i = s_Pools.Count - 1; i >= 0; i--)
            {
                if (s_Pools[i].TryGetTarget(out var pool))
                {
                    pool.TrimForLowMemory();
                }
                else
                {
                    s_Pools.RemoveAt(i);
                }
            }
        }
    }
    }
}
