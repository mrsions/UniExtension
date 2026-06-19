#nullable enable

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UniExtension;
using UnityEngine;

namespace UnityEngine
{
    public static class SetEnableExtension
    {
        public static void TrySetEnable(this Component? comp, bool enabled)
        {
            if (!comp) return;

            if (comp is Behaviour b)
            {
                b.enabled = enabled;
            }
            else if (comp is Renderer r)
            {
                r.enabled = enabled;
            }
            else if (comp is Collider c)
            {
                c.enabled = enabled;
            }
            else if (comp is LODGroup l)
            {
                l.enabled = enabled;
            }
            else if (comp is Cloth cl)
            {
                cl.enabled = enabled;
            }
            else
            {
                SLog.TraceCtx(comp, "Maybe not defined enabled component.");
            }
        }
        public static void TrySetEnable(this Behaviour? comp, bool enabled) { if (comp) comp.enabled = enabled; }
        public static void TrySetEnable(this Renderer? comp, bool enabled) { if (comp) comp.enabled = enabled; }
        public static void TrySetEnable(this Collider? comp, bool enabled) { if (comp) comp.enabled = enabled; }
        public static void TrySetEnable(this LODGroup? comp, bool enabled) { if (comp) comp.enabled = enabled; }
        public static void TrySetEnable(this Cloth? comp, bool enabled) { if (comp) comp.enabled = enabled; }

#if UNITY_EDITOR || ENABLE_GENERIC_EXTENSION
        public static void TrySetEnable<T>(this IEnumerable<T>? enumerable, bool enabled) where T : Component
        {
            if (enumerable == null) return;

            if (enumerable is IList<T> list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i]?.TrySetEnable(enabled);
                }
            }
            else if (enumerable is ICollection<T> col)
            {
                T[] array = ArrayPool<T>.Shared.Rent(col.Count);
                col.CopyTo(array, 0);

                foreach (var arr in array)
                {
                    arr?.TrySetEnable(enabled);
                }
                ArrayPool<T>.Shared.Return(array);
            }
            else
            {
                var it = enumerable.GetEnumerator();
                while (it.MoveNext())
                {
                    it.Current?.TrySetEnable(enabled);
                }
            }
        }
#else
        public static void TrySetEnable(this IEnumerable? enumerable, bool enabled)
        {
            if (enumerable == null) return;

            Assert.IsTrue(TypeUtility.IsUnityCollectionType(enumerable.GetType()), $"`{enumerable.GetType().FullName}` is not unity type.");

            if (enumerable is IList list)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    (list[i] as Component)?.TrySetEnable(enabled);
                }
            }
            else if (enumerable is ICollection col)
            {
                object[] array = ArrayPool<object>.Shared.Rent(col.Count);
                col.CopyTo(array, 0);

                foreach (var arr in array)
                {
                    (arr as Component)?.TrySetEnable(enabled);
                }
            }
            else
            {
                var it = enumerable.GetEnumerator();
                while (it.MoveNext())
                {
                    (it.Current as Component)?.TrySetEnable(enabled);
                }
            }
        }
#endif
    }
}
