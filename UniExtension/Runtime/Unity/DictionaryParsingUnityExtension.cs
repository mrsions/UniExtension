#if UNITY_5_3_OR_NEWER
using System.Collections.Generic;
using UnityEngine;

namespace UniExtension
{
    public static class DictionaryParsingUnityExtension
    {
        public static Vector3 GetVector3<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Vector3 def = default)
        {
            try
            {
                object value = dic[key];
                if (value == null)
                    return def;
                if (value is Vector3)
                    return (Vector3)value;
                else if (value is int[])
                {
                    var list = (int[])value;
                    return new Vector3(list[0], list[1], list[2]);
                }
                else if (value is float[])
                {
                    var list = (float[])value;
                    return new Vector3(list[0], list[1], list[2]);
                }
                else if (value is IList<int>)
                {
                    var list = (IList<int>)value;
                    return new Vector3(list[0], list[1], list[2]);
                }
                else if (value is IList<float>)
                {
                    var list = (IList<float>)value;
                    return new Vector3(list[0], list[1], list[2]);
                }
                else if (value is IList<string>)
                {
                    var list = (IList<string>)value;
                    return new Vector3(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2]));
                }
                else if (value is IList<object>)
                {
                    var list = (IList<object>)value;
                    if (list[0] is float)
                        return new Vector3((float)list[0], (float)list[1], (float)list[2]);
                    else
                        return new Vector3(float.Parse(list[0].ToString()), float.Parse(list[1].ToString()), float.Parse(list[2].ToString()));
                }
                else if (value is IDictionary<string, object>)
                {
                    var dic2 = (IDictionary<string, object>)value;
                    return new Vector3(dic2.GetFloat("x"), dic2.GetFloat("y"), dic2.GetFloat("z"));

                }
                return def;
            }
            catch { return def; }
        }
        public static Quaternion GetQuaternion<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            try
            {
                object value = dic[key];
                if (value is Quaternion)
                    return (Quaternion)value;
                else if (value is IList<float>)
                {
                    var list = (IList<float>)value;
                    return new Quaternion(list[0], list[1], list[2], list[3]);
                }
                else if (value is IList<string>)
                {
                    var list = (IList<string>)value;
                    return new Quaternion(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2]), float.Parse(list[3]));
                }
                else if (value is IList<object>)
                {
                    var list = (IList<object>)value;
                    if (list[0] is float)
                        return new Quaternion((float)list[0], (float)list[1], (float)list[2], (float)list[3]);
                    else
                        return new Quaternion(float.Parse(list[0].ToString()), float.Parse(list[1].ToString()), float.Parse(list[2].ToString()), float.Parse(list[3].ToString()));
                }
                else if (value is IDictionary<string, object>)
                {
                    var dic2 = (IDictionary<string, object>)value;
                    return new Quaternion(dic2.GetFloat("x"), dic2.GetFloat("y"), dic2.GetFloat("z"), dic2.GetFloat("w"));

                }
                return Quaternion.identity;
            }
            catch { return Quaternion.identity; }
        }
        public static Quaternion GetQuaternion<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, Quaternion def)
        {
            try
            {
                object value = dic[key];
                if (value is Quaternion)
                    return (Quaternion)value;
                else if (value is IList<float>)
                {
                    var list = (IList<float>)value;
                    return new Quaternion(list[0], list[1], list[2], list[3]);
                }
                else if (value is IList<string>)
                {
                    var list = (IList<string>)value;
                    return new Quaternion(float.Parse(list[0]), float.Parse(list[1]), float.Parse(list[2]), float.Parse(list[3]));
                }
                else if (value is IList<object>)
                {
                    var list = (IList<object>)value;
                    if (list[0] is float)
                        return new Quaternion((float)list[0], (float)list[1], (float)list[2], (float)list[3]);
                    else
                        return new Quaternion(float.Parse(list[0].ToString()), float.Parse(list[1].ToString()), float.Parse(list[2].ToString()), float.Parse(list[3].ToString()));
                }
                else if (value is IDictionary<string, object>)
                {
                    var dic2 = (IDictionary<string, object>)value;
                    return new Quaternion(dic2.GetFloat("x"), dic2.GetFloat("y"), dic2.GetFloat("z"), dic2.GetFloat("w"));

                }
                return def;
            }
            catch { return def; }
        }
    }
}
#endif