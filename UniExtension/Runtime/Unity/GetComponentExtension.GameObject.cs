#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UniExtension.Collections;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace UnityEngine
{
    public static partial class GetComponentExtension // GameObject
    {
        #region GameObject
        public static T EnsureComponent<T>(this GameObject self) where T : Component
        {
            var obj = self.GetComponent<T>();
            if (obj != null) return obj;

            return self.AddComponent<T>();
        }

        public static T EnsureComponent<T>(this GameObject self, in string tag) where T : Component
        {
            var obj = self.GetComponent<T>();
            if (obj != null) return obj;

            self.EditorRecord(tag);
            try
            {
                return self.AddComponent<T>();
            }
            finally
            {
                self.EditorDirty();
            }
        }

        public static PList<T> GetComponentsList<T>(this GameObject self)
        {
            var list = PList.Take<T>();
            self.GetComponents<T>(list);
            return list;
        }
        public static PList<T> GetComponentsInChildrenList<T>(this GameObject self, bool includeInactive = true)
        {
            var list = PList.Take<T>();
            self.GetComponentsInChildren<T>(includeInactive, list);
            return list;
        }
        public static PList<T> GetComponentsInParentList<T>(this GameObject self, bool includeInactive = true)
        {
            var list = PList.Take<T>();
            self.GetComponentsInParent<T>(includeInactive, list);
            return list;
        }

        public static bool TryEnsureComponent<T>(this GameObject self, out T result)
            where T : Component
        {
            result = self.GetComponent<T>();
            if (result != null) return false;

            result = self.gameObject.AddComponent<T>();
            return true;
        }

        public static bool TryGetComponent<T>(this GameObject self, out T result)
            where T : class
        {
            result = self.GetComponent<T>();
            return result.IsLive();
        }
        public static bool TryGetComponentInChildren<T>(this GameObject self, [NotNullWhen(true)] out T? result, bool includeInactive = false)
            where T : class
        {
            result = self.GetComponentInChildren<T>(includeInactive);
            return result.IsLive();
        }
        public static bool TryGetComponentInParent<T>(this GameObject self, [NotNullWhen(true)] out T? result)
            where T : class
        {
            result = self.GetComponentInParent<T>();
            return result.IsLive();
        }
        public static bool TryGetComponents<T>(this GameObject self, List<T> result)
            where T : class
        {
            self.GetComponents(result);
            return result.HasAny();
        }
        public static bool TryGetComponentsInChildren<T>(this GameObject self, List<T> result, bool includeInactive = false)
            where T : class
        {
            self.GetComponentsInChildren(includeInactive, result);
            return result.HasAny();
        }
        public static bool TryGetComponentsInParent<T>(this GameObject self, List<T> result, bool includeInactive = false)
            where T : class
        {
            self.GetComponentsInParent(includeInactive, result);
            return result.HasAny();
        }
        #endregion
    }
}
