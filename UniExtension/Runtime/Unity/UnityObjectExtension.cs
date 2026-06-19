// Copyright (c) 2016 Sions
// 
// UniExtension version 1.0.0
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#nullable enable

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace UnityEngine
{
public static class UnityObjectExtension
{
    [System.Obsolete("Please use IsLiveOrNotNull()")]
    public static bool IsLive([NotNullWhen(true)] this Object? obj)
    {
        return obj != null && obj;
    }

    public static bool IsLiveOrNotNull([NotNullWhen(true)] this Object? obj)
    {
        return obj != null && obj;
    }
    public static T? GetLiveOrNull<T>(this T? obj)
        where T : UnityEngine.Object
    {
        return obj ? obj : null;
    }
    public static bool IsDestroyedOrNull([NotNullWhen(false)] this Object? obj)
    {
        return obj == null || !obj;
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    [System.Obsolete("Please use IsDestroyedOrNull")]
    public static bool IsDestroyOrNull([NotNullWhen(false)] this Object? obj)
    {
        return IsDestroyedOrNull(obj);
    }

    [System.Obsolete("Please use IsDestroyedOrNull")]
    public static bool IsDeadOrNull([NotNullWhen(false)] this Object? obj)
    {
        return obj.IsDestroyedOrNull();
    }

    [System.Obsolete("Please use IsDestroyedOrNull")]
    public static bool IsDead([NotNullWhen(false)] this Object? obj)
    {
        return obj == null || !obj;
    }

    //[System.Obsolete("This method can be heavy. Please use the target only when it is not specific.")]
    public static bool IsComplexActiveSelf(this Object obj)
    {
        if (obj == null)
        {
            return false;
        }
        else if (obj is GameObject)
        {
            return ((GameObject)obj).activeSelf;
        }
        else if (obj is Behaviour)
        {
            return ((Behaviour)obj).enabled;
        }
        else if (obj is Renderer)
        {
            return ((Renderer)obj).enabled;
        }
        else if (obj is CanvasGroup)
        {
            return ((CanvasGroup)obj).interactable;
        }
#if !DISABLE_CLOTH && ENABLE_CLOTH
        else if (obj is Cloth)
        {
            return ((Cloth)obj).enabled;
        }
#endif
        else if (obj is Transform)
        {
            return ((Transform)obj).gameObject.activeSelf;
        }
        else if (obj is LODGroup)
        {
            return ((LODGroup)obj).enabled;
        }
        return false;
    }
    //[System.Obsolete("This method can be heavy. Please use the target only when it is not specific.")]
    public static void SetComplexActiveSelf(this Object obj, bool enable)
    {
        if (obj != null)
        {
            if (obj is GameObject)
            {
                ((GameObject)obj).SetActive(enable);
            }
            else if (obj is Behaviour)
            {
                ((Behaviour)obj).enabled = enable;
            }
            else if (obj is Renderer)
            {
                ((Renderer)obj).enabled = enable;
            }
            else if (obj is CanvasGroup)
            {
                ((CanvasGroup)obj).interactable = enable;
            }
#if !DISABLE_CLOTH && ENABLE_CLOTH
            else if (obj is Cloth)
            {
                ((Cloth)obj).enabled = enable;
            }
#endif
            else if (obj is Transform)
            {
                ((Transform)obj).gameObject.SetActive(enable);
            }
            else if (obj is LODGroup)
            {
                ((LODGroup)obj).enabled = enable;
            }
        }
    }

    [Conditional("UNITY_EDITOR")]
    public static void EditorRecord(this Object obj, string? tag = null)
    {
#if UNITY_EDITOR
        UnityEditor.Undo.RecordObject(obj, tag ?? "Change" + obj.GetType());
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void EditorDirty(this Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(obj);
#endif
    }

    [Conditional("UNITY_EDITOR")]
    public static void EditorSaveIfDirty(this Object obj)
    {
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.SaveAssetIfDirty(obj);
#endif
    }
}
}
