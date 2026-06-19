#nullable enable
//#line hidden

using System;
using System.Threading;
using System.Collections.Generic;
using UniExtension;
using UniExtension.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
    /// <summary>
    /// dontDestroyScene까지 포함하는 씬매니저
    /// </summary>
    public static class UniSceneManager
    {
        static GameObject? s_dontDestroySceneObject;
        public static GameObject dontDestroySceneObject
        {
            get
            {
                var obj = s_dontDestroySceneObject;
                if (!obj)
                {
#if UNITY_EDITOR
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                    {
                        throw new InvalidOperationException("Can not access from edit mode.");
                    }
#endif

                    obj = s_dontDestroySceneObject = new GameObject(nameof(DontDestroySceneObject), typeof(DontDestroySceneObject));
                    var comp = obj.EnsureComponent<DontDestroySceneObject>();
                    comp.Destoryed += static (self) =>
                    {
                        if (s_dontDestroySceneObject == self.gameObject)
                        {
                            s_dontDestroySceneObject = null;
                        }
                    };
                    GameObject.DontDestroyOnLoad(s_dontDestroySceneObject);

                    SLog.TraceCtx(s_dontDestroySceneObject, "Create GameObject DontDestroySceneObject");
                }
                return obj;
            }
        }

        public static Scene dontDestroyScene
        {
            get
            {

#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    return default;
                }
#endif
                return dontDestroySceneObject.scene;
            }
        }

        public static int sceneCount => SceneManager.sceneCount + (dontDestroyScene.isLoaded ? 1 : 0);

        public static Scene GetSceneAt(int index)
        {
            return InternalGetSceneAt(index, SceneManager.sceneCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Scene InternalGetSceneAt(int index, int sceneCount)
        {
            return index == sceneCount ? dontDestroyScene : SceneManager.GetSceneAt(index);
        }

        public static T? FindObject<T>(Scene scene, bool includeInactive = false, bool includeHideFlags = false)
        {
            if (!scene.isLoaded) return default;

            var roots = scene.GetRootGameObjects();
            for (var i = 0; i < roots.Length; i++)
            {
                var go = roots[i];
                if (includeHideFlags || go.gameObject.hideFlags == HideFlags.None)
                {
                    T result = roots[i].GetComponentInChildren<T>(includeInactive);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return default;
        }

        public static PList<T> FindObjects<T>(Scene scene, bool includeInactive = false, bool includeHideFlags = false)
        {

            var result = PList.Take<T>();
            if (scene.isLoaded)
            {
                using var temp = PList.Take<T>();

                var roots = scene.GetRootGameObjects();
                for (var i = 0; i < roots.Length; i++)
                {
                    var go = roots[i];
                    if (includeHideFlags || go.gameObject.hideFlags == HideFlags.None)
                    {
                        roots[i].GetComponentsInChildren<T>(includeInactive, temp);
                        if (temp.Count > 0)
                        {
                            result.AddRange(temp);
                        }
                    }
                }
            }

            return result;
        }

        public static PList<T> FindObjects<T>(bool includeInactive = true, bool includeHideFlags = false)
        {
            using var result = PList.Take<T>();

            for (int si = 0, len = SceneManager.sceneCount; si < len; si++)
            {
                Scene scene = InternalGetSceneAt(si, len);
                if (!scene.isLoaded) continue;

                using var temp = FindObjects<T>(scene, includeInactive, includeHideFlags);
                result.AddRange(temp);
            }

            return result;
        }

        /// <summary>
        /// inactive 여부와 상관없이 전체 씬을 순회하며 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PList<GameObject> FindObjectsPath(string path)
        {
            var result = PList.Take<GameObject>();

            if (path.IsNullOrWhiteSpace()) return result;

            var names = path.Split("/");
            if (names.Length == 0) return result;

            using var temp = PList.Take<GameObject>();
            var name = names[0] ?? string.Empty;
            for (int si = 0, len = SceneManager.sceneCount; si <= len; si++)
            {
                Scene scene = InternalGetSceneAt(si, len);
                if (!scene.isLoaded) continue;

                scene.GetRootGameObjects(temp);
                for (int j = 0; j < temp.Count; j++)
                {
                    var go = temp[j];
                    if (go.name == name)
                    {
                        result.Add(go);
                    }
                }
            }

            for (int i = 1; i < names.Length; i++)
            {
                name = names[i] ?? string.Empty;

                temp.Clear();
                temp.AddRange(result);
                result.Clear();

                for (int j = 0; j < temp.Count; j++)
                {
                    var tr = temp[j].transform;
                    for (int k = 0; k < tr.childCount; k++)
                    {
                        var child = tr.GetChild(k);
                        if (child.name == name)
                        {
                            result.Add(child.gameObject);
                        }
                    }
                }
            }

            return result;
        }

        public static GameObject? FindObjectPath(string path)
        {
            using var list = FindObjectsPath(path);
            return list.FirstOrDefault();
        }


        /// <summary>
        /// inactive 여부와 상관없이 전체 씬을 순회하며 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PList<GameObject> FindObjectsPath(Scene scene, string path)
        {
            var result = PList.Take<GameObject>();

            if (scene.isLoaded && path.IsNotEmpty())
            {
                var names = path.Split("/");
                if (names.Length == 0) return result;

                using var temp = PList.Take<GameObject>();
                var name = names[0] ?? string.Empty;

                scene.GetRootGameObjects(temp);
                for (int j = 0; j < temp.Count; j++)
                {
                    var go = temp[j];
                    if (go.name == name)
                    {
                        result.Add(go);
                    }
                }

                for (int i = 1; i < names.Length; i++)
                {
                    name = names[i] ?? string.Empty;

                    temp.Clear();
                    temp.AddRange(result);
                    result.Clear();

                    for (int j = 0; j < temp.Count; j++)
                    {
                        var tr = temp[j].transform;
                        for (int k = 0; k < tr.childCount; k++)
                        {
                            var child = tr.GetChild(k);
                            if (child.name == name)
                            {
                                result.Add(child.gameObject);
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static GameObject? FindObjectPath(Scene scene, string path)
        {
            using var list = FindObjectsPath(scene, path);
            return list.FirstOrDefault();
        }

        public static void DestroyAll()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode || !EditorApplication.isPlaying)
            {
                DestroyImmediateAll();
                return;
            }
#endif
            for (int si = 0, len = SceneManager.sceneCount; si <= len; si++)
            {
                Scene scene = InternalGetSceneAt(si, len);
                if (!scene.isLoaded) continue;
                foreach (var go in scene.GetRootGameObjects())
                {
                    GameObject.Destroy(go);
                }
            }
        }

        public static void DestroyImmediateAll()
        {
            for (int si = 0, len = SceneManager.sceneCount; si <= len; si++)
            {
                Scene scene = InternalGetSceneAt(si, len);
                if (!scene.isLoaded) continue;
                foreach (var go in scene.GetRootGameObjects())
                {
                    GameObject.DestroyImmediate(go);
                }
            }
        }

    }
}
