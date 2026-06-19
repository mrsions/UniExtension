using System;
using System.Collections.Generic;


#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace System.Collections.Generic
{
    public static class DictionaryParsingExtension
    {
        /// <summary>
        /// 찾으려는 대상이 기본형 타입을 경우에 무조건 해당 타입으로 결과값을 반환합니다.
        /// </summary>
        public static int GetInt<TKey, TVAlue>(this IDictionary<TKey, TVAlue> dic, TKey key, int def = 0)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(int))
                    return (int)value;
                else if (type == typeof(string))
                    return int.Parse((string)value);
                else if (type == typeof(float))
                    return (int)(float)value;
                else if (type == typeof(bool))
                    return (bool)value ? 1 : 0;
                else if (type == typeof(byte))
                    return (int)(byte)value;
                else if (type == typeof(short))
                    return (int)(short)value;
                else if (type == typeof(long))
                    return (int)(long)value;
                else if (type == typeof(uint))
                    return (int)(uint)value;
                else if (type == typeof(double))
                    return (int)(double)value;
                else if (type == typeof(sbyte))
                    return (int)(sbyte)value;
                else if (type == typeof(ulong))
                    return (int)(ulong)value;
                else if (type == typeof(decimal))
                    return (int)(decimal)value;
                return Convert.ToInt32(dic[key]);
            }
            catch { return def; }
        }
        public static float GetFloat<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, float def = 0)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(float))
                    return (float)value;
                else if (type == typeof(string))
                    return float.Parse((string)value);
                else if (type == typeof(int))
                    return (float)(int)value;
                else if (type == typeof(bool))
                    return (bool)value ? 1 : 0;
                else if (type == typeof(byte))
                    return (float)(byte)value;
                else if (type == typeof(short))
                    return (float)(short)value;
                else if (type == typeof(long))
                    return (float)(long)value;
                else if (type == typeof(uint))
                    return (float)(uint)value;
                else if (type == typeof(double))
                    return (float)(double)value;
                else if (type == typeof(sbyte))
                    return (float)(sbyte)value;
                else if (type == typeof(ushort))
                    return (float)(ushort)value;
                else if (type == typeof(ulong))
                    return (float)(ulong)value;
                else if (type == typeof(decimal))
                    return (float)(decimal)value;
                return Convert.ToSingle(dic[key]);
            }
            catch { return def; }
        }
        public static double GetDouble<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, double def = 0)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(double))
                    return (double)value;
                else if (type == typeof(string))
                    return double.Parse((string)value);
                else if (type == typeof(int))
                    return (double)(int)value;
                else if (type == typeof(float))
                    return (double)(float)value;
                else if (type == typeof(bool))
                    return (bool)value ? 1 : 0;
                else if (type == typeof(byte))
                    return (double)(byte)value;
                else if (type == typeof(short))
                    return (double)(short)value;
                else if (type == typeof(long))
                    return (double)(long)value;
                else if (type == typeof(uint))
                    return (double)(uint)value;
                else if (type == typeof(sbyte))
                    return (double)(sbyte)value;
                else if (type == typeof(ushort))
                    return (double)(ushort)value;
                else if (type == typeof(ulong))
                    return (double)(ulong)value;
                else if (type == typeof(decimal))
                    return (double)(decimal)value;
                return Convert.ToDouble(dic[key]);
            }
            catch { return def; }
        }
        public static bool GetBool<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, bool def = false)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(bool))
                    return (bool)value;
                else if (type == typeof(string))
                    return bool.Parse((string)value);
                else if (type == typeof(int))
                    return (int)value != 0;
                else if (type == typeof(float))
                    return (float)value != 0;
                else if (type == typeof(double))
                    return (double)value != 0;
                else if (type == typeof(byte))
                    return (byte)value != 0;
                else if (type == typeof(short))
                    return (short)value != 0;
                else if (type == typeof(long))
                    return (long)value != 0;
                else if (type == typeof(uint))
                    return (uint)value != 0;
                else if (type == typeof(sbyte))
                    return (sbyte)value != 0;
                else if (type == typeof(ushort))
                    return (ushort)value != 0;
                else if (type == typeof(ulong))
                    return (ulong)value != 0;
                else if (type == typeof(decimal))
                    return (decimal)value != 0;
                return Convert.ToBoolean(value);
            }
            catch { return def; }
        }
        public static long GetLong<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, long def = 0)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(long))
                    return (long)value;
                else if (type == typeof(string))
                    return long.Parse((string)value);
                else if (type == typeof(int))
                    return (long)(int)value;
                else if (type == typeof(float))
                    return (long)(float)value;
                else if (type == typeof(bool))
                    return (bool)value ? 1L : 0L;
                else if (type == typeof(byte))
                    return (long)(byte)value;
                else if (type == typeof(short))
                    return (long)(short)value;
                else if (type == typeof(double))
                    return (long)(double)value;
                else if (type == typeof(uint))
                    return (long)(uint)value;
                else if (type == typeof(sbyte))
                    return (long)(sbyte)value;
                else if (type == typeof(ushort))
                    return (long)(ushort)value;
                else if (type == typeof(ulong))
                    return (long)(ulong)value;
                else if (type == typeof(decimal))
                    return (long)(decimal)value;
                return Convert.ToInt64(value);
            }
            catch { return def; }
        }
        public static ulong GetULong<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, ulong def = 0)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(ulong))
                    return (ulong)value;
                else if (type == typeof(string))
                    return ulong.Parse((string)value);
                else if (type == typeof(int))
                    return (ulong)(int)value;
                else if (type == typeof(float))
                    return (ulong)(float)value;
                else if (type == typeof(bool))
                    return (bool)value ? 1uL : 0uL;
                else if (type == typeof(byte))
                    return (ulong)(byte)value;
                else if (type == typeof(short))
                    return (ulong)(short)value;
                else if (type == typeof(double))
                    return (ulong)(double)value;
                else if (type == typeof(uint))
                    return (ulong)(uint)value;
                else if (type == typeof(sbyte))
                    return (ulong)(sbyte)value;
                else if (type == typeof(ushort))
                    return (ulong)(ushort)value;
                else if (type == typeof(long))
                    return (ulong)(long)value;
                else if (type == typeof(decimal))
                    return (ulong)(decimal)value;
                return Convert.ToUInt64(value);
            }
            catch { return def; }
        }
        public static string GetString<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, string def = null)
        {
            try
            { return dic[key].ToString(); }
            catch { return def; }
        }
        public static DateTime GetDateTime<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key)
        {
            return dic.GetDateTime(key, new DateTime(0));
        }
        public static DateTime GetDateTime<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, DateTime def)
        {
            try
            {
                object value = dic[key];
                var type = value.GetType();
                if (type == typeof(DateTime))
                    return (DateTime)value;
                else if (type == typeof(string))
                {
                    DateTime d;
                    if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss.fff", null, System.Globalization.DateTimeStyles.None, out d)) return d;
                    else if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out d)) return d;
                    else if (DateTime.TryParseExact((string)value, "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out d)) return d;
                    else if (DateTime.TryParseExact((string)value, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out d)) return d;
                    return DateTime.Parse((string)value);
                }
                else if (type == typeof(int))
                    return new DateTime((long)(int)value);
                else if (type == typeof(float))
                    return new DateTime((long)(float)value);
                else if (type == typeof(bool))
                    return new DateTime(0);
                else if (type == typeof(byte))
                    return new DateTime((long)(byte)value);
                else if (type == typeof(short))
                    return new DateTime((long)(short)value);
                else if (type == typeof(long))
                    return new DateTime((long)value);
                else if (type == typeof(uint))
                    return new DateTime((long)(uint)value);
                else if (type == typeof(sbyte))
                    return new DateTime((long)(sbyte)value);
                else if (type == typeof(ushort))
                    return new DateTime((long)(ushort)value);
                else if (type == typeof(ulong))
                    return new DateTime((long)(ulong)value);
                else if (type == typeof(decimal))
                    return new DateTime((long)(decimal)value);
                return Convert.ToDateTime(dic[key]);
            }
            catch { return def; }
        }
        public static void SetDateTime<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, DateTime value)
        {
            var type = typeof(TValue);
            if (type == typeof(DateTime))
            {
                var ndic = (IDictionary<TKey, DateTime>)dic;
                ndic[key] = value;
            }
            else if (type == typeof(string) || type == typeof(object))
            {
                var ndic = (IDictionary<TKey, string>)dic;
                ndic[key] = value.ToString("yyyy-MM-dd HH:mm:ss.fff");
            }
            else if (type == typeof(int))
            {
                var ndic = (IDictionary<TKey, int>)dic;
                ndic[key] = (int)value.Ticks;
            }
            else if (type == typeof(long))
            {
                var ndic = (IDictionary<TKey, long>)dic;
                ndic[key] = value.Ticks;
            }
            else if (type == typeof(uint))
            {
                var ndic = (IDictionary<TKey, uint>)dic;
                ndic[key] = (uint)value.Ticks;
            }
            else if (type == typeof(ulong))
            {
                var ndic = (IDictionary<TKey, ulong>)dic;
                ndic[key] = (ulong)value.Ticks;
            }
            else if (type == typeof(decimal))
            {
                var ndic = (IDictionary<TKey, decimal>)dic;
                ndic[key] = (decimal)value.Ticks;
            }
            throw new ArgumentException($"Not support value type. {type.FullName}");
        }
    }
}