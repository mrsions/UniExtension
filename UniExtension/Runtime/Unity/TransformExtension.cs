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

using System.Collections.Generic;
using System.Text;
using UniExtension.Collections;
using UniExtension;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnityEngine
{
[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData(Transform transform)
    {
        position = transform.localPosition;
        rotation = transform.localRotation;
        scale = transform.localScale;
    }

    public void CopyTo(Transform transform)
    {
        transform.localPosition = position;
        transform.localRotation = rotation;
        transform.localScale = scale;
    }
}

public static class TransformExtension
{
    public static PList<Transform> GetChildren(this Transform trans)
    {
        var list = PList.Take<Transform>();
        for (int i = 0, len = trans.childCount; i < len; i++)
        {
            list.Add(trans.GetChild(i));
        }
        return list;
    }
    public static PList<GameObject> GetChildrenGameObjects(this Transform trans)
    {
        var list = PList.Take<GameObject>();
        for (int i = 0, len = trans.childCount; i < len; i++)
        {
            list.Add(trans.GetChild(i).gameObject);
        }
        return list;
    }

    public static void DestroyChildren(this Transform transform)
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
        {
            transform.DestroyChildrenImmediate();
            return;
        }
#endif
        transform.DestroyChildrenNormal();
    }
    public static void DestroyChildrenNormal(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }
    public static void DestroyChildrenImmediate(this Transform transform)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    public static void DestroyChildrenImmediate(this Transform transform, GameObject exclude)
    {
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            var go = transform.GetChild(i).gameObject;
            if (exclude != go)
            {
                GameObject.DestroyImmediate(go);
            }
        }
    }

    public static Transform FindChildDeep(this Transform trans, string name)
    {
        var tf = trans.Find(name);
        if (tf != null)
            return tf;

        for (int i = 0, len = trans.childCount; i < len; i++)
        {
            tf = trans.GetChild(i).FindChildDeep(name);
            if (tf != null)
                return tf;
        }
        return null;
    }

    public static Transform FindChildTag(this Transform trans, string tag)
    {
        Transform tf;
        for (int i = 0, len = trans.childCount; i < len; i++)
        {
            tf = trans.GetChild(i);
            if (tf.CompareTag(tag))
            {
                return tf;
            }
            else
            {
                tf = tf.FindChildTag(tag);
                if (tf != null)
                    return tf;
            }
        }
        return null;
    }

    public static Transform FindPath(this Transform trans, string path)
    {
        var splits = path.Split('/');
        Transform result = null;
        for (var i = 0; i < splits.Length; i++)
        {
            var name = "";
            while (i < splits.Length)
            {
                name += splits[i];
                result = trans.Find(name);
                if (result)
                {
                    break;
                }
                result = null;
                i++;
                name += "/";
            }
            trans = result;
        }
        return result;
    }

    public static void ResetWorld(this Transform trans)
    {
        trans.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        trans.localScale = Vector3.one;
    }

    public static void ResetLocal(this Transform trans)
    {
        trans.localPosition = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = Vector3.one;
    }

    public static void CopyFrom(this Transform dest, Transform src)
    {
        dest.parent = src.parent;
        dest.SetSiblingIndex(src.GetSiblingIndex());
        dest.localPosition = src.localPosition;
        dest.localRotation = src.localRotation;
        dest.localScale = src.localScale;
    }

    public static bool IsParent(this Transform child, Transform parent)
    {
        if (child == parent) return true;

        while (child != parent && child.parent != null)
        {
            child = child.parent;
        }

        return child == parent;
    }

    public static string Path(this Transform trans, Transform parent = null, StringBuilder buffer = null)
    {
        using (var list = PList.Take<Transform>())
        {
            do
            {
                list.Add(trans);
            }
            while ((trans = trans.parent) != parent && trans != null);

            if (buffer == null) buffer = new StringBuilder();
            for (var i = list.Count - 1; i >= 0; i--)
            {
                buffer.Append(list[i].name);
                if (i != 0) buffer.Append('/');
            }
            return buffer.ToString();
        }
    }

    public static PList<Transform> GetParents(this Transform trans, Transform parent = null)
    {
        var list = PList.Take<Transform>();
        do
        {
            list.Add(trans);
        }
        while ((trans = trans.parent) != parent && trans != null);
        return list;
    }

    public static int GetDepth(this Transform trans, Transform parent = null)
    {
        var count = 0;
        while ((trans = trans.parent) != parent && trans != null)
        {
            count++;
        }
        return count;
    }
    public static string PathAndScene(this Transform trans)
    {
        return $"[{trans.gameObject.scene.name}] {trans.Path()}";
    }

    public static string PathAndSibiling(this Transform trans)
    {
        var path = $"{trans.GetSiblingIndex()}.{trans.name}";
        while ((trans = trans.parent) != null)
        {
            path = $"{trans.GetSiblingIndex()}.{trans.name}/{path}";
        }
        return path;
    }

    public static TransformData GetTransformData(this Transform trans)
    {
        return new TransformData(trans);
    }

    public static float Distance(this Transform a, Transform b)
    {
        return Vector3.Distance(a.position, b.position);
    }
    public static float Distance(this Transform a, Component b)
    {
        return Vector3.Distance(a.position, b.transform.position);
    }
    public static float Distance(this Transform a, GameObject b)
    {
        return Vector3.Distance(a.position, b.transform.position);
    }

    public static void SetPositionAndRotation(this Transform a, Pose pose)
    {
        a.SetPositionAndRotation(pose.position, pose.rotation);
    }
    public static Pose GetPositionAndRotation(this Transform a)
    {
        return new Pose(a.position, a.rotation);
    }

    public static void SetLocalPositionAndRotation(this Transform a, Pose pose)
    {
        a.localPosition = pose.position;
        a.localRotation = pose.rotation;
    }
    public static Pose GetLocalPositionAndRotation(this Transform a)
    {
        return new Pose(a.localPosition, a.localRotation);
    }

    public static void SetLossyScale(this Transform trans, Vector3 lossyScale)
    {
        if (trans.parent)
        {
            var tmatrix = Matrix4x4.TRS(trans.position, trans.rotation, lossyScale);
            var pmatrix = trans.parent.worldToLocalMatrix;
            var matrix = pmatrix * tmatrix;
            trans.localScale = matrix.lossyScale;
        }
        else
        {
            trans.localScale = lossyScale;
        }
    }

    public static void CopyTo(this Transform from, Transform to, bool useParent = true)
    {
        if (useParent)
        {
            to.SetParent(from.parent, false);
        }

        to.localPosition = from.localPosition;
        to.localRotation = from.localRotation;
        to.localScale = from.localScale;
    }

    public static void CopyWorldTo(this Transform from, Transform to, bool useParent = true)
    {
        if (useParent)
        {
            to.SetParent(from.parent, false);
        }

        to.position = from.position;
        to.rotation = from.rotation;
        to.SetLossyScale(from.lossyScale);
    }

    public static void CopyTo(this RectTransform from, RectTransform to)
    {
        var siblingIndex = from.GetSiblingIndex();

        Debug.Log($"--------------------------------------------");
        Debug.Log($"from : {from.Path()}");
        Debug.Log($"to : {to.Path()}");
        Debug.Log($"--------------------------------------------");
        Debug.Log($"from.anchorMin : {from.anchorMin}");
        Debug.Log($"from.anchorMax : {from.anchorMax}");
        Debug.Log($"from.pivot : {from.pivot}");
        Debug.Log($"from.sizeDelta : {from.sizeDelta}");
        Debug.Log($"from.anchoredPosition3D : {from.anchoredPosition3D}");
        Debug.Log($"from.localRotation : {from.localRotation}");
        Debug.Log($"from.localScale : {from.localScale}");

        to.anchorMin = from.anchorMin;
        to.anchorMax = from.anchorMax;
        to.pivot = from.pivot;
        to.anchoredPosition3D = from.anchoredPosition3D;
        to.sizeDelta = from.sizeDelta;
        to.localRotation = from.localRotation;
        to.localScale = from.localScale;

        Debug.Log($"--------------------------------------------");
        Debug.Log($"to.anchorMin : {to.anchorMin}");
        Debug.Log($"to.anchorMax : {to.anchorMax}");
        Debug.Log($"to.pivot : {to.pivot}");
        Debug.Log($"to.sizeDelta : {to.sizeDelta}");
        Debug.Log($"to.anchoredPosition3D : {to.anchoredPosition3D}");
        Debug.Log($"to.localRotation : {to.localRotation}");
        Debug.Log($"to.localScale : {to.localScale}");

        to.SetParent(from.parent, false);
        to.SetSiblingIndex(siblingIndex);

        Debug.Log($"--------------------------------------------");
        Debug.Log($"to.anchorMin : {to.anchorMin}");
        Debug.Log($"to.anchorMax : {to.anchorMax}");
        Debug.Log($"to.pivot : {to.pivot}");
        Debug.Log($"to.sizeDelta : {to.sizeDelta}");
        Debug.Log($"to.anchoredPosition3D : {to.anchoredPosition3D}");
        Debug.Log($"to.localRotation : {to.localRotation}");
        Debug.Log($"to.localScale : {to.localScale}");
    }

}
}
