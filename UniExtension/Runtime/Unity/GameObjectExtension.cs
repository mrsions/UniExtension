using System;
using System.Collections.Generic;
using UniExtension.Collections;
using UnityEngine;

namespace UnityEngine
{
    public static class GameObjectExtension
    {
        ////[Obsolete("Please use SetActiveTarget()")]
        //[Obsolete("SetActive() has been deprecated. Use SetActiveTarget() instead. (UnityUpgradable) -> SetActiveTarget(*)", true)]
        //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //[System.ComponentModel.Browsable(false)]
        //public static void SetActive(this IList<GameObject> objs, int index) => objs.SetActiveTarget(index);
        //public static void SetActiveTarget(this IList<GameObject> objs, int index)
        //{
        //    if (objs != null)
        //    {
        //        for (var i = 0; i < objs.Count; i++)
        //        {
        //            if (objs[i] != null)
        //            {
        //                objs[i].SetActive(i == index);
        //            }
        //        }
        //    }
        //}
        //public static void SetActiveTarget(this IList<GameObject> objs, int index, bool active)
        //{
        //    if (objs != null)
        //    {
        //        for (var i = 0; i < objs.Count; i++)
        //        {
        //            if (objs[i] != null)
        //            {
        //                objs[i].SetActive((i == index) == active);
        //            }
        //        }
        //    }
        //}
        //[Obsolete("Please use SetActiveTarget()")]
        //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //[System.ComponentModel.Browsable(false)]
        //public static void SetActive(this IList<GameObject> objs, bool active, int offset) => objs.SetActiveTargetOnly(offset, active);
        //[Obsolete("Please use SetActiveTarget()")]
        //[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        //[System.ComponentModel.Browsable(false)]
        //public static void SetActiveTargetOnly(this IList<GameObject> objs, int offset, bool active)
        //{
        //    if (objs != null)
        //    {
        //        for (var i = offset; i < objs.Count; i++)
        //        {
        //            if (objs[i] != null)
        //            {
        //                objs[i].SetActive(active);
        //            }
        //        }
        //    }
        //}

        //[Obsolete("Please use SetActiveAll()")]
        //public static void SetActive(this IList<GameObject> objs, bool active) => objs.SetActiveAll(active);
        //[Obsolete("Please use SetActiveAll()")]
        //public static void SetActiveTargets(this IList<GameObject> objs, bool active) => objs.SetActiveAll(active);
        //public static void SetActiveAll(this IList<GameObject> objs, bool active)
        //{
        //    if (objs == null) return;

        //    for (int i = 0, iLen = objs.Count; i < iLen; i++)
        //    {
        //        var obj = objs[i];
        //        if (obj != null)
        //        {
        //            obj.SetActive(active);
        //        }
        //    }
        //}


        [Obsolete("Please use SetActiveGameObjectAll()")]
        public static void SetGameObjectActive<T>(this IList<T> objs, bool active) where T : Component => objs.SetActiveGameObjectAll(active);
        [Obsolete("Please use SetActiveGameObjectAll()")]
        public static void SetActiveGameObjects<T>(this IList<T> objs, bool active) where T : Component => objs.SetActiveGameObjectAll(active);

        public static void SetActiveGameObjectAll<T>(this IList<T> objs, bool active)
            where T : Component
        {
            if (objs != null)
            {
                for (var i = 0; i < objs.Count; i++)
                {
                    if (objs[i] != null)
                    {
                        objs[i].gameObject.SetActive(active);
                    }
                }
            }
        }

        [Obsolete("Please use SetActiveGameObjects()")]
        public static void SetGameObjectActive<T>(this IList<T> objs, int index, bool active) where T : Component => objs.SetActiveGameObjects<T>(index, active);
        /// <summary>
        /// 특정 index의 GameObject만 입력된 active로 SetActive가 입력되며 나머지는 입력값의 반대로 입력된다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objs"></param>
        /// <param name="index"></param>
        /// <param name="active"></param>
        public static void SetActiveGameObjects<T>(this IList<T> objs, int index, bool active)
            where T : Component
        {
            if (objs == null)
            {
                return;
            }

            for (var i = 0; i < objs.Count; i++)
            {
                objs[i]?.gameObject.SetActive(i == index);
            }
        }

        public static void SetActiveChildren(this GameObject objs, bool active)
        {
            var trans = objs.transform;
            for (int i = 0, len = trans.childCount; i < len; i++)
            {
                trans.GetChild(i).gameObject.SetActive(active);
            }
        }

        public static void ReActive(this GameObject go)
        {
            go.SetActive(false);
            go.SetActive(true);
        }

        /// <summary>
        /// Instantiate 확장
        /// </summary>
        /// <param name="p_prefab">타겟 프리팹</param>
        /// <param name="p_parent">부모 게임오브젝트</param>
        /// <param name="p_position">Position</param>
        /// <param name="p_rotation">Roatation</param>
        /// <param name="p_name">게임오브젝트 이름</param>
        /// <returns></returns>
        public static GameObject InstantiateEx(this GameObject target,
            GameObject original,
            GameObject p_parent,
            Vector3 p_position,
            Quaternion p_rotation,
            string p_name = null)
        {
            var __result = GameObject.Instantiate(original);
            if (p_parent)
            {
                __result.transform.SetParent(p_parent.transform);
                __result.transform.localPosition = p_position;
                __result.transform.localRotation = p_rotation;
                __result.transform.localScale = Vector3.one;
            }
            else
            {
                __result.transform.SetPositionAndRotation(p_position, p_rotation);
            }
            if (p_name != null)
            {
                __result.name = p_name;
            }
            return __result;
        }

        public static void Destroy(this GameObject target)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject.DestroyImmediate(target);
            }
            else
#endif
            {
                GameObject.Destroy(target);
            }
        }

        public static string GetGameObjectPath(GameObject obj)
        {
            var path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        public static bool ContainsLayer(this GameObject go, LayerMask mask)
        {
            return ((1 << go.layer) & mask) != 0;
        }

        public static GameObject LastActive(this IList<GameObject> gos)
        {
            for (var i = gos.Count - 1; i >= 0; i--)
            {
                if (gos[i])
                {
                    return gos[i];
                }
            }
            return null;
        }

        public static GameObject FirstActive(this IList<GameObject> gos)
        {
            for (var i = 0; i < gos.Count; i++)
            {
                if (gos[i])
                {
                    return gos[i];
                }
            }
            return null;
        }
    }
}
