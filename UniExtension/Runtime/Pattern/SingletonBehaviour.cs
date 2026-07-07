#nullable enable
#line hidden

using System;
using Cysharp.Threading.Tasks;
using SUtils;
using UniExtension;
using UnityEngine;
using UnityEngine.Scripting;
using Unity.Scripting.LifecycleManagement;

namespace UniExtension
{
[Preserve]
public interface ISingletonCreateOpt { }

[Preserve]
public interface ISingletonFindOpt { }

[Preserve]
public interface ISingletonDisposeCheckOpt { }

[Flags]
public enum SingletonOption
{
    None = 0,
    Initialized = 1,
    Create = 2,
    Find = 4,
    DisposeCheck = 8
}

public abstract class SingletonBehaviour<T> : MonoBehaviour
    where T : MonoBehaviour
{
    [AutoStaticsCleanup]
    private static SingletonOption s_options;
    [AutoStaticsCleanup]
    private static T? s_main;

    public static bool HasInstance => s_main != null;
    public static T Instance
    {
        get
        {
            var main = s_main;
            if (main == null)
            {
                main = GetSingleton(ref s_main);
                s_main = main;
            }
            return main;
        }
    }

    [System.Diagnostics.DebuggerHidden]
    public static T GetSingleton(ref T _main)
    {
        if (_main != null && s_options.HasFlag(SingletonOption.DisposeCheck))
        {
            if (!CoroutineHelper.IsMainThread)
            {
                var obj = _main;
                var task = UniTask.Create(async delegate
                {
                    await UniTask.SwitchToMainThread();

                    if (!obj)
                    {
                        obj = null;
                    }
                });
                while (task.Status == UniTaskStatus.Pending) System.Threading.Thread.Sleep(1);
                _main = obj;
            }
            else if (!_main)
            {
                _main = null;
            }
        }

        if (_main == null)
        {
            if (s_options == SingletonOption.None)
            {
                var t = typeof(T);
                //foreach(var i in t.GetInterfaces())
                //{
                //    Debug.Log("GetSingletone " + typeof(T).Name + " interface " + i.Name);
                //}

                s_options = SingletonOption.Initialized;
                if (t.GetInterface(nameof(ISingletonFindOpt)) != null)
                    s_options |= SingletonOption.Find;
                if (t.GetInterface(nameof(ISingletonCreateOpt)) != null)
                    s_options |= SingletonOption.Create;
                if (t.GetInterface(nameof(ISingletonDisposeCheckOpt)) != null)
                    s_options |= SingletonOption.DisposeCheck;
                //Debug.Log("GetSingletone " + typeof(T).Name + " opt " + opt);
            }
#if UNITY_5_3_OR_NEWER
            if (s_options.HasFlag(SingletonOption.Find))
            {
                var found = false;
                if (!CoroutineHelper.IsMainThread)
                {
                    var start = DateTime.UtcNow;
                    UniTask.Void(async () =>
                    {
                        await UniTask.Yield();
                        FindSingleton();
                        found = true;
                    });
                    while (!found) System.Threading.Thread.Sleep(1);
                    var elapsed = (DateTime.UtcNow - start).TotalMilliseconds;
                    if (elapsed > 100)
                    {
                        UnityEngine.Debug.LogError($"Singleton create has been long times. ({elapsed:f0}ms)");
                    }
                    else if (elapsed > 10)
                    {
                        UnityEngine.Debug.LogWarning($"Singleton create has been long times. ({elapsed:f0}ms)");
                    }
                }
                else
                {
                    found = FindSingleton();
                }
                if (found) return _main;
            }
            if (s_options.HasFlag(SingletonOption.Create))
            {
                if (!CoroutineHelper.IsMainThread)
                {
                    var task = UniTask.Create(async delegate
                    {
                        await UniTask.SwitchToMainThread();
                        CreateSingleton();
                    });
                    while (task.Status == UniTaskStatus.Pending) System.Threading.Thread.Sleep(1);
                }
                else
                {
                    CreateSingleton();
                }
                return _main;
            }
#else  
            if (opt.HasFlag(SingletonOption.Create))
            {
                _main = SafeActivator.CreateInstance<T>();
            }
#endif
        }
        return _main;
    }

    private static bool FindSingleton()
    {
        s_main = FindCompatibleObjectOfType();
        //Debug.Log("GetSingletone " + typeof(T).Name + " Find " + _main);
        if (s_main != null) return true;

        var res = Resources.FindObjectsOfTypeAll<T>();
        foreach (var r in res)
        {
            if (r.gameObject.scene.name.IsNotEmpty())
            {
                s_main = r;
                return true;
            }
        }
        return false;
    }

    private static T? FindCompatibleObjectOfType()
    {
#if UNITY_2023_1_OR_NEWER
        return UnityEngine.Object.FindAnyObjectByType<T>();
#else
        return UnityEngine.Object.FindObjectOfType<T>();
#endif
    }

    private static void CreateSingleton()
    {
        s_main = new GameObject(typeof(T).Name).AddComponent<T>();
        //Debug.Log("GetSingletone " + typeof(T).Name + " Create " + _main);
    }

    public static T GetMainDirect()
    {
        return s_main;
    }

    protected virtual void Awake()
    {
        if (SLogger.IsDebug) SLog.DebugCtx(this, "SingletoneBehaviour", "SetMain " + transform.Path());
        s_main = (T)(object)this;
    }

    protected virtual void OnEnable()
    {
        if (s_main == null)
        {
            s_main = this as T;
        }
    }

    protected virtual void OnDestroy()
    {
        if (s_main == this)
        {
            s_main = null;
        }
    }
}
}
