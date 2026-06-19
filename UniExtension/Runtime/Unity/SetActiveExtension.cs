#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using uobject = UnityEngine.Object;

namespace UnityEngine
{
    public static class SetActiveExtension
    {
        public static void TrySetActive(this GameObject? obj, bool active)
        {
            if (obj)
            {
                obj.SetActive(active);
            }
        }

        public static void TrySetActive(this Component? obj, bool active)
        {
            if (obj)
            {
                obj.gameObject.SetActive(active);
            }
        }

        public static void TrySetActive(object? obj, bool active)
        {
            if (obj is GameObject go)
            {
                go.SetActive(active);
            }
            else if (obj is Component comp)
            {
                comp.gameObject.SetActive(active);
            }
        }

#if UNITY_EDITOR || ENABLE_GENERIC_EXTENSION
        public static void TrySetActive(this IEnumerable? e, bool active)
        {
            if (e == null) return;

            if (e is GameObject[] arr)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    TrySetActive(arr[i], active);
                }
            }
            else if (e is IList list)
            {
                for (int i = 0, len = list.Count; i < len; i++)
                {
                    TrySetActive(list[i], active);
                }
            }
            else
            {
                var it = e.GetEnumerator();
                while (it.MoveNext())
                {
                    TrySetActive(it.Current, active);
                }
            }
        }

        public static void TrySetActiveOne(this IEnumerable? e, int index)
        {
            if (e == null) return;

            if (e is GameObject[] arr)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    TrySetActive(arr[i], i == index);
                }
            }
            else if (e is IList list)
            {
                for (int i = 0, len = list.Count; i < len; i++)
                {
                    TrySetActive(list[i], i == index);
                }
            }
            else
            {
                int i = 0;
                var it = e.GetEnumerator();
                while (it.MoveNext())
                {
                    TrySetActive(it.Current, i == index);
                }
            }
        }
#else
#endif
        public static void TrySetActive<T>(this IEnumerable<T>? e, bool active)
            where T : Component
        {
            if (e == null) return;

            if (e is T[] arr)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    TrySetActive(arr[i], active);
                }
            }
            else if (e is IList<T> list)
            {
                for (int i = 0, len = list.Count; i < len; i++)
                {
                    TrySetActive(list[i], active);
                }
            }
            else
            {
                var it = e.GetEnumerator();
                while (it.MoveNext())
                {
                    TrySetActive(it.Current, active);
                }
            }
        }

        public static void TrySetActiveOne<T>(this IEnumerable<T>? e, int index)
            where T : Component
        {
            if (e == null) return;

            if (e is T[] arr)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    TrySetActive(arr[i], i == index);
                }
            }
            else if (e is IList<T> list)
            {
                for (int i = 0, len = list.Count; i < len; i++)
                {
                    TrySetActive(list[i], i == index);
                }
            }
            else
            {
                int i = 0;
                var it = e.GetEnumerator();
                while (it.MoveNext())
                {
                    TrySetActive(it.Current, i == index);
                }
            }
        }
    }
}