using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniExtension;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Scripting;
using Unity.Scripting.LifecycleManagement;

namespace SUtils
{
    [Preserve]
    public class CoroutineWrapper
    {
        internal Coroutine coroutine;
        internal IEnumerator processor;

        public CoroutineWrapper()
        { }

        public CoroutineWrapper(Coroutine coroutine)
        {
            this.coroutine = coroutine;
        }

        public CoroutineWrapper(IEnumerator processor)
        {
            this.processor = processor;
        }

        public CoroutineWrapper(Coroutine coroutine, IEnumerator processor)
        {
            this.coroutine = coroutine;
            this.processor = processor;
        }
    }

    public class ActionWrapper
    {
        [AutoStaticsCleanup]
        static int generator = 0;

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static int GetNewId()
        {
            return ++generator;
        }

        internal int id;
        internal Action action;

        public ActionWrapper(Action act)
            : this()
        {
            action = act;
        }

        public ActionWrapper()
        {
            id = GetNewId();
        }

        public void NewId()
        {
            id = GetNewId();
        }
    }

    public struct InvokeResult
    {
        internal ActionWrapper invoke;
        internal int id;
        public bool IsComplete => invoke == null || id != invoke.id;

        internal InvokeResult(ActionWrapper invoke)
        {
            this.invoke = invoke;
            this.id = invoke.id;
        }

        public void Wait()
        {
            if (invoke != null)
            {
                while (id == invoke.id)
                {
                }
            }
        }

        public async UniTask WaitAsync()
        {
            if (invoke != null)
            {
                while (id == invoke.id)
                {
                    await UniTask.Delay(10);
                }
            }
        }
    }

    public static class CoroutineHelper
    {
        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    MEMBER
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        [AutoStaticsCleanup]
        private static UniExtension.ObjectPool<CoroutineWrapper> coroutinePool = new();
        [AutoStaticsCleanup]
        private static UniExtension.ObjectPool<ActionWrapper> invokePool = new();

        //-- private
        [AutoStaticsCleanup]
        private static object LOCK = new();
        //private static Thread mainThread;
        [AutoStaticsCleanup]
        private static List<CoroutineWrapper> coroutines = new();
        [AutoStaticsCleanup]
        private static List<CoroutineWrapper> coroutines2 = new();
        [AutoStaticsCleanup]
        private static List<ActionWrapper> actions = new();
        [AutoStaticsCleanup]
        private static List<ActionWrapper> actions2 = new();
        [AutoStaticsCleanup]
        private static bool isDirty;
        [AutoStaticsCleanup]
        private static bool _created = false;
        [AutoStaticsCleanup]
        private static CoroutineHelperImpl _main;

        //-- Properties

        /// <summary>
        /// 시스템이 사용중인지 확인함
        /// </summary>
        public static bool Enable
        {
            get
            {
#if UNITY_EDITOR
                if (IsMainThread)
                {
                    return UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode;
                }
                else
                {
                    return UnityApplicationCallback.IsPlaying;
                }
#else
                return true;
#endif
            }
        }

        /// <summary>
        /// 현재 활동중인 메인 객체
        /// </summary>
        [Obsolete("이제 사용하지 않습니다.", true)]
        public static CoroutineHelperImpl main
        {
            get
            {
                if (_main != null)
                {
                    return _main;
                }
                else if (Enable)
                {
                    var go = new GameObject("CoroutineHelper");
                    _main = go.AddComponent<CoroutineHelperImpl>();
#if UNITY_EDITOR
                    _main.hideFlags = HideFlags.HideAndDontSave;
#endif
                    _created = true;
                    SLog.DebugCtx(go, "Create GameObject CoroutineHelper");
                }
                return _main;
            }
        }

        [AutoStaticsCleanup]
        static SynchronizationContext context;
        [AutoStaticsCleanup]
        static int mainThreadId = -1;

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void InitializeOnLoadMethod()
        {
            var st = Stopwatch.StartNew();
            try
            {
                SetupTaskUnity();
            }
            finally
            {
                UnityEngine.Debug.Log($"[EDITORTIME]InitializeOnLoadMethod {st.ElapsedMilliseconds:f0}ms - {nameof(CoroutineHelper)}");
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        static void DidReloadScripts()
        {
            var st = Stopwatch.StartNew();
            try
            {
                SetupTaskUnity();
            }
            finally
            {
                UnityEngine.Debug.Log($"[EDITORTIME]DidReloadScripts {st.ElapsedMilliseconds:f0}ms - {nameof(CoroutineHelper)}");
            }
        }
#endif

        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoadMethod()
        {
            var st = Stopwatch.StartNew();
            try
            {
                SetupTaskUnity();
            }
            finally
            {
                UnityEngine.Debug.Log($"[EDITORTIME]RuntimeInitializeOnLoadMethod {st.ElapsedMilliseconds:f0}ms - {nameof(CoroutineHelper)}");
            }
        }

        static void SetupTaskUnity()
        {
            var st = Stopwatch.StartNew();
            try
            {
                context = SynchronizationContext.Current;
                mainThreadId = Thread.CurrentThread.ManagedThreadId;
                _main = FindCompatibleObjectOfType();
                if (!_main)
                {
                    CreateCoroutineHelper();
                }
            }
            finally
            {
                UnityEngine.Debug.Log($"RuntimeInitializeOnLoadMethod {st.ElapsedMilliseconds:f0}ms - {nameof(CoroutineHelper)}");
            }
        }

        private static CoroutineHelperImpl impl
        {
            get
            {
                if (_main != null)
                {
                    return _main;
                }
                else if (Enable)
                {
                    CreateCoroutineHelper();
                }
                return _main;
            }
        }

        private static void CreateCoroutineHelper()
        {
            var go = new GameObject("CoroutineHelper");
            _main = go.AddComponent<CoroutineHelperImpl>();
#if UNITY_EDITOR
            go.hideFlags = HideFlags.HideAndDontSave;
#endif
            _created = true;
            SLog.DebugCtx(go, "Create GameObject CoroutineHelper");
        }

        private static CoroutineHelperImpl? FindCompatibleObjectOfType()
        {
#if UNITY_2023_1_OR_NEWER
            return UnityEngine.Object.FindAnyObjectByType<CoroutineHelperImpl>();
#else
            return GameObject.FindObjectOfType<CoroutineHelperImpl>();
#endif
        }

        /// <summary>
        /// 코루틴 등록을 사용할 수 있는지 확인합니다.
        /// </summary>
        public static bool Can
        {
            get
            {
                return !_created || impl != null;
            }
        }

        /// <summary>
        /// 현재 쓰레드가 메인쓰레드인지 확인합니다.
        /// </summary>
        public static bool IsMainThread
        {
            get
            {
                //if (mainThread == null)
                //{
                //    if (Environment.StackTrace.Contains(" UnityEditor."))
                //    {
                //        mainThread = Thread.CurrentThread;
                //    }
                //    else
                //    {
                //        throw new NotImplementedException("Not initialized MainThread");
                //    }
                //}
                return Can && mainThreadId == Thread.CurrentThread.ManagedThreadId;
            }
        }

        /// <summary>
        /// 현재 쓰레드가 메인쓰레드인지 확인합니다.
        /// </summary>
        public static void ThrowIfNotMainThread()
        {
            if (!IsMainThread)
            {
                throw new ApplicationException("Must be called main thread.");
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        //
        //                    LIFECYCLE
        //
        ///////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 시스템을 사용하기 위해 원하는 메인스레드에서 실행한다.
        /// </summary>
        public static void Init()
        {
            if (!_main)
            {
                if (Enable)
                {
                    var go = new GameObject("CoroutineHelper");
                    _main = go.AddComponent<CoroutineHelperImpl>();
                    _created = true;
                    SLog.DebugCtx(go, "Create GameObject CoroutineHelper");
                }
            }
        }

        [Conditional("DEBUG")]
        private static void EnsureInit()
        {
            if (mainThreadId == -1)
            {
                Init();
                //throw new NullReferenceException("Failed into early access. please call before call CoroutineHelepr.Init()");
            }
        }

        /// <summary>
        /// 코루틴을 안전하게 시작합니다.
        /// </summary>
        public static CoroutineWrapper Start(IEnumerator e)
        {
            EnsureInit();
            if (IsMainThread)
            {
                var wrapper = coroutinePool.Take();
                wrapper.coroutine = impl.StartCoroutine(e);
                return wrapper;
            }
            else
            {
                return Enqueue(e);
            }
        }

        /// <summary>
        /// 코루틴을 안전하게 시작합니다.
        /// </summary>
        public static CoroutineWrapper Enqueue(IEnumerator e)
        {
            if (e == null) return null;

            lock (LOCK)
            {
                isDirty = true;
                var wrapper = coroutinePool.Take();
                wrapper.processor = e;
                coroutines.Add(wrapper);
                return wrapper;
            }
        }

        /// <summary>
        /// 코루틴을 안전하게 시작합니다.
        /// </summary>
        public static CoroutineWrapper Restart(CoroutineWrapper wrapper, IEnumerator e = null)
        {
            EnsureInit();
            if (wrapper == null)
            {
                return Start(e);
            }

            // 코루틴이 시작하지 않았고 권한을 가져왔을 때도 시작하지 않았다면 
            // 시작 대상을 변경해준다.
            if (wrapper.coroutine == null)
            {
                lock (wrapper)
                {
                    if (wrapper.coroutine == null)
                    {
                        wrapper.processor = e;
                        return wrapper;
                    }
                }
            }

            if (IsMainThread)
            {
                impl.RestartCoroutine(ref wrapper.coroutine, e);
            }
            else
            {
                lock (LOCK)
                {
                    isDirty = true;
                    wrapper.processor = e;
                    coroutines.AddIfHaveNot(wrapper);
                }
            }
            return wrapper;
        }

        public static void Stop(CoroutineWrapper wrapper)
        {
            EnsureInit();
            if (wrapper != null)
            {
                if (wrapper.coroutine == null)
                {
                    lock (wrapper)
                    {
                        if (wrapper.coroutine == null)
                        {
                            wrapper.processor = null;
                            return;
                        }
                    }
                }

                if (IsMainThread)
                {
                    lock (wrapper)
                    {
                        impl.StopCoroutine(wrapper.coroutine);
                        wrapper.coroutine = null;
                        wrapper.processor = null;
                    }
                    return;
                }
                else
                {
                    lock (LOCK)
                    {
                        isDirty = true;
                        wrapper.processor = null;
                        coroutines.AddIfHaveNot(wrapper);
                    }
                }
            }
        }

        public static InvokeResult Invoke(Action action)
        {
            EnsureInit();

            if (action != null)
            {
                if (IsMainThread)
                {
                    action();
                }
                else
                {
                    return Enqueue(action);
                }
            }

            return default;
        }

        public static InvokeResult Enqueue(Action action)
        {
            if (action == null)
            {
                throw new System.ArgumentNullException("action must be not null.");
            }

            var callback = invokePool.Take();
            callback.action = action;

            var result = new InvokeResult(callback);
            Enqueue(callback);
            return result;
        }

        public static void Enqueue(ActionWrapper action)
        {
            if (action == null || action.action == null)
            {
                throw new System.ArgumentNullException("action must be not null.");
            }

            lock (LOCK)
            {
                isDirty = true;
                actions.Add(action);
            }
        }

        //public static Task Run(Action act, CancellationToken cancelToken = default)
        //{
        //    FixUnityThreadAfter();
        //    return Task.Factory.StartNew(act, cancelToken, TaskCreationOptions.PreferFairness, TaskScheduler.FromCurrentSynchronizationContext());
        //}

        //public static Task<T> Run<T>(Func<T> act, CancellationToken cancelToken = default)
        //{
        //    FixUnityThreadAfter();
        //    return Task<T>.Factory.StartNew(act, cancelToken, TaskCreationOptions.PreferFairness, TaskScheduler.FromCurrentSynchronizationContext());
        //}

        //public static Task Run(Func<Task> act, CancellationToken cancelToken = default)
        //{
        //    FixUnityThreadAfter();
        //    return Task<Task>.Factory.StartNew(act, cancelToken, TaskCreationOptions.PreferFairness, TaskScheduler.FromCurrentSynchronizationContext());
        //}

        //public static async Task<T> Run<T>(Func<Task<T>> act)
        //{
        //    await FixUnityThreadAsync();
        //    return await act();
        //}

        //public static UniTask Run(Action act)
        //{
        //    FixUnityThreadAfter();
        //    return UniTask.RunOnThreadPool(act);
        //}

        //public static UniTask<T> Run<T>(Func<T> act)
        //{
        //    FixUnityThreadAfter();
        //    return UniTask.RunOnThreadPool<T>(act);
        //}

        //public static UniTask Run(Func<UniTask> act, CancellationToken cancelToken = default)
        //{
        //    FixUnityThreadAfter();
        //    return UniTask.Create(act);
        //}

        //public static async UniTask<T> Run<T>(Func<UniTask<T>> act)
        //{
        //    FixUnityThreadAfter();
        //    return await UniTask.Create<T>(act);
        //}


        /// <summary>
        /// 현재 유니티쓰레드에서 실행중인지 아닌지를 판단합니다.
        /// 유니티쓰레드에서 실행중이면 바로 await가 풀리고 다음으로 넘어갑니다.
        /// 만약 유니티 쓰레드에서 실행중이지 않으면 언젠가 다시 유니티 쓰레드에서 실행합니다.
        /// 
        /// 언젠가란 Task.Delay가 끝나는 시간으로서, cpu clock에 의해 달라집니다.
        /// 
        /// 높은 fps(60fps 이상)에서는 2~3 프레임 뒤에 실행될 여지가 있습니다. (예상일 뿐)
        /// </summary>
        /// <returns></returns>
        public static UniTask FixUnityThreadAsync()
        {
            if (Thread.CurrentThread.ManagedThreadId != mainThreadId)
            {
                SynchronizationContext.SetSynchronizationContext(context);
                return UniTask.Delay(1);
            }
            else
            {
                return UniTask.CompletedTask;
            }
        }

        /// <summary>
        /// 유니티 쓰레드로 전환합니다.
        /// 대부분 다음 프레임에 실행됩니다.
        /// </summary>
        /// <returns></returns>
        public static Cysharp.Threading.Tasks.YieldAwaitable YieldUnityThread()
        {
            SynchronizationContext.SetSynchronizationContext(context);
            return UniTask.Yield();
        }

        public static bool IsUnityThread()
        {
            return Thread.CurrentThread.ManagedThreadId == mainThreadId;
        }

        public static void FixUnityThreadAfter()
        {
            if (Thread.CurrentThread.ManagedThreadId != mainThreadId)
            {
                SynchronizationContext.SetSynchronizationContext(context);
            }
            UniTask.Yield();
        }

        public class CoroutineHelperImpl : MonoBehaviour
        {

            private void Awake()
            {
                mainThreadId = Thread.CurrentThread.ManagedThreadId;
                DontDestroyOnLoad(gameObject);
            }

            private void OnDestroy()
            {
                if (Enable && _main == this)
                    _main = null;
            }

            private void Update()
            {
                if (isDirty)
                {
                    isDirty = false;
                    Profiler.BeginSample("AddRange");
                    lock (this)
                    {
                        coroutines2.AddRange(coroutines);
                        actions2.AddRange(actions);
                        coroutines.Clear();
                        actions.Clear();
                    }
                    Profiler.EndSample();

                    Profiler.BeginSample("AddCoroutines");
                    for (var i = 0; i < coroutines2.Count; i++)
                    {
                        var e = coroutines2[i];
                        if (e != null)
                        {
                            Profiler.BeginSample("coroutines");
                            if (e.coroutine != null)
                            {
                                Profiler.BeginSample("stop");
                                StopCoroutine(e.coroutine);
                                e.coroutine = null;
                                Profiler.EndSample();
                            }
                            if (e.processor != null)
                            {
                                Profiler.BeginSample("start");
                                e.coroutine = StartCoroutine(e.processor);
                                e.processor = null;
                                Profiler.EndSample();
                            }
                            Profiler.EndSample();
                        }
                    }
                    Profiler.EndSample();
                    Profiler.BeginSample("Actions");
                    for (var i = 0; i < actions2.Count; i++)
                    {
                        var act = actions2[i];
                        Profiler.BeginSample("Actions");
                        try
                        {
                            act.action?.Invoke();
                        }
                        catch (Exception e)
                        {
                            SLog.Exception(e);
                        }
                        Profiler.EndSample();
                        act.NewId();
                        invokePool.Release(act);
                    }
                    Profiler.EndSample();

                    Profiler.BeginSample("Clears");
                    coroutines2.Clear();
                    actions2.Clear();
                    Profiler.EndSample();
                }
            }

            public CoroutineWrapper StartCoroutineMain(IEnumerator e)
            {
                return Start(e);
            }

            public CoroutineWrapper RestartCoroutineMain(CoroutineWrapper wrapper, IEnumerator e)
            {
                return Restart(wrapper, e);
            }

            public void RestartCoroutine(ref Coroutine coroutine, IEnumerator e)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(e);
            }

            public void StopCoroutineMain(CoroutineWrapper wrapper)
            {
                Stop(wrapper);
            }

            [Obsolete("Please use CoroutineHelper.Invoke", true)]
            public void InvokeMainThread(Action callback)
            {
                throw new NotImplementedException("Obsolete this method");
                //if (Thread.CurrentThread == mainThread)
                //{
                //    callback();
                //}
                //else
                //{
                //    isDirty = true;
                //    lock (this)
                //    {
                //        actions.Add(callback);
                //    }
                //}
            }

            [Obsolete("Please use CoroutineHelper.Enqueue", true)]
            public void Invoke(Action callback)
            {
                throw new NotImplementedException("Obsolete this method");
                //isDirty = true;
                //lock (this)
                //{
                //    actions.Add(callback);
                //}
            }
        }
    }
}
