#nullable enable

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using UniExtension.Collections;
using UnityEditor;

namespace System.Collections.Generic
{
    public static class _ListExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? TryGet<T>(this IList<T> list, int index)
        {
            return TryGet(list, index, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T? TryGet<T>(this IList<T> list, int index, T? def)
        {
            return list.IsBounds(index) ? list[index] : def;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBounds<T>(this IList<T> list, int index)
        {
            return 0 <= index && index < list.Count;
        }

        public static bool IsEmpty<T>([NotNullWhen(false)] this IList<T>? list)
        {
            return list == null || list.Count == 0;
        }

        public static bool HasAny<T>([NotNullWhen(true)] this IList<T>? list)
        {
            return list != null && list.Count != 0;
        }

        public static T Pop<T>(this IList<T> list)
        {
            var result = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return result;
        }

        public static T? TryPop<T>(this IList<T> list)
        {
            if (list.Count > 0)
            {
                var result = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return result;
            }
            else
            {
                return default;
            }
        }

        public static void RemoveIf<T>(this IList<T> list, Func<T, bool> predict)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (predict(item))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        public static string Join<T>(this IList<T> list, string delimiter, int start = 0, int length = -1)
        {
            if (start < 0 || start < list.Count) throw new IndexOutOfRangeException("");
            if (length == -1) length = list.Count;

            int end = start + length;
            if (end >= list.Count) throw new IndexOutOfRangeException("");

            var sb = new StringBuilder();
            sb.Append(list[start]);
            for (int i = start + 1; i < end; i++)
            {
                sb.Append(delimiter).Append(list[i]);
            }
            return sb.ToString();
        }

        public static T ToRandom<T>(this IList<T> list, Random? random)
        {
            if (list.Count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            else if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                if (random != null) return list[random.Next(0, list.Count)];
#if UNITY_5_3_OR_NEWER
                return list[UnityEngine.Random.Range(0, list.Count)];
#else
                return list[Random.Shared.Next(0, list.Count)];
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="weightSelector">항목당 weight값</param>
        /// <returns>
        /// weight값을 모두 합쳤을때 0이 나오면 null을 리턴함.
        /// </returns>
        public static T ToRandom<T>(this IList<T> array, Func<T, float> weightSelector)
        {
            if (array == null || array.Count == 0)
                return default;
            if (array.Count == 1)
                return array[0];

            // 10Kb 의 stack이 필요함
            if (array.Count < 2560)
            {
                unsafe
                {
                    float sum = 0;
                    var percent = stackalloc float[array.Count];
                    for (var i = 0; i < array.Count; i++)
                    {
                        sum += weightSelector(array[i]);
                        percent[i] = sum;
                    }

                    if (sum == 0) return default;

                    var random = UnityEngine.Random.value * sum;
                    for (var i = 0; i < array.Count; i++)
                    {
                        if (random < percent[i])
                        {
                            return array[i];
                        }
                    }
                }
            }
            else
            {
                float sum = 0;
                using var percent = PList.Take<float>();
                for (var i = 0; i < array.Count; i++)
                {
                    sum += weightSelector(array[i]);
                    percent.Add(sum);
                }

                if (sum == 0) return default;

                var random = UnityEngine.Random.value * sum;
                for (var i = 0; i < array.Count; i++)
                {
                    if (random < percent[i])
                    {
                        return array[i];
                    }
                }
            }
            return array[array.Count - 1];
        }

        public static bool AddIfHaveNot<T>(this IList<T> list, T value)
        {
            if (!list.Contains(value))
            {
                list.Add(value);
                return true;
            }
            return false;
        }

        public static void AddRangeIfHaveNot<T>(this IList<T> list, IEnumerable<T> value)
        {
            var e = value.GetEnumerator();
            while (e.MoveNext())
            {
                list.AddIfHaveNot(e.Current);
            }
        }

        public static void Put<T>(this IList<T> list, int index, T value)
        {
            if (index >= list.Count)
            {
                for (int i = 0, len = (index - list.Count) + 1; i < len; i++)
                {
                    list.Add(default);
                }
            }
            list[index] = value;
        }

        public static T Pop<T>(this IList<T> list, int index)
        {
            try
            {
                return list[index];
            }
            finally
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// ※경고※ 리스트의 순서가 유지되지않습니다.
        /// 교환식으로 아이템을 삭제합니다. item이 들어있던 위치에 리스트의 마지막 정보가 옮겨져오며 마지막 index를 삭제됩니다.
        /// 때문에 삭제 속도가 빠릅니다. 하지만 리스트의 순서는 유지되지 않습니다.
        /// </summary>
        public static void Remove_ExchangeLast<T>(this IList<T> list, T item)
        {
            var index = list.IndexOf(item);
            if (index != -1)
            {
                list.RemoveAt_ExchangeLast(index);
            }
        }
        /// <summary>
        /// ※경고※ 리스트의 순서가 유지되지않습니다.
        /// 교환식으로 아이템을 삭제합니다. index에 들어있던 위치에 리스트의 마지막 정보가 옮겨져오며 마지막 index를 삭제됩니다.
        /// 때문에 삭제 속도가 빠릅니다. 하지만 리스트의 순서는 유지되지 않습니다.
        /// </summary>
        public static void RemoveAt_ExchangeLast<T>(this IList<T> list, int index)
        {
            var last = list.Count - 1;
            list[index] = list[last];
            list.RemoveAt(last);
        }

        public static int CountExist<T>(this IList<T> list)
        {
            var count = 0;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    count++;
                }
            }
            return count;
        }

        public static T1[] ToArray<T1>(this T1[] array)
        {
            var result = new T1[array.Length];
            for (var i = 0; i < array.Length; i++)
                result[i] = array[i];
            return result;
        }

        public static T2[] ToArray<T1, T2>(this T1[] array)
            where T1 : T2
        {
            var result = new T2[array.Length];
            for (var i = 0; i < array.Length; i++)
                result[i] = array[i];
            return result;
        }
        public static T2[][] ToArray<T1, T2>(this T1[][] array)
            where T1 : T2
        {
            var result = new T2[array.Length][];
            for (var i = 0; i < array.Length; i++)
                result[i] = array[i].ToArray<T1, T2>();
            return result;
        }
        public static T2[][][] ToArray<T1, T2>(this T1[][][] array)
            where T1 : T2
        {
            var result = new T2[array.Length][][];
            for (var i = 0; i < array.Length; i++)
                result[i] = array[i].ToArray<T1, T2>();
            return result;
        }
        public static List<T2> ToList<T1, T2>(this IList<T1> array)
            where T1 : T2
        {
            var result = new List<T2>(array.Count);
            for (var i = 0; i < array.Count; i++)
                result[i] = array[i];
            return result;
        }

        #region Parsing

        public static int GetInt<T>(this IList<T> list, int key, int def = 0)
        {
            try
            {
                object value = list[key];
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
                else if (type == typeof(ushort))
                    return (int)(ushort)value;
                else if (type == typeof(ulong))
                    return (int)(ulong)value;
                else if (type == typeof(decimal))
                    return (int)(decimal)value;
                return Convert.ToInt32(list[key]);
            }
            catch { return def; }
        }
        public static float GetFloat<T>(this IList<T> list, int key, float def = 0)
        {
            try
            {
                object value = list[key];
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
                return Convert.ToSingle(list[key]);
            }
            catch { return def; }
        }
        public static double GetDouble<T>(this IList<T> list, int key, double def = 0)
        {
            try
            {
                object value = list[key];
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
                return Convert.ToDouble(list[key]);
            }
            catch { return def; }
        }
        public static bool GetBool<T>(this IList<T> list, int key, bool def = false)
        {
            try
            {
                object value = list[key];
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
        public static long GetLong<T>(this IList<T> list, int key, long def = 0)
        {
            try
            {
                object value = list[key];
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
        public static string GetString<T>(this IList<T> list, int key, string def = null)
        {
            try
            { return list[key].ToString(); }
            catch { return def ?? string.Empty; }
        }
        #endregion

        #region Filter
        public static int CountIf<T>(this IList<T> dic, Func<T, bool> filter)
        {
            var count = 0;
            var en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                if (filter(en.Current))
                    count++;
            }
            return count;
        }
        public static IEnumerator<T> GetEnumerator<T>(this IList<T> dic, Func<T, bool> filter)
        {
            var en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                if (filter(en.Current))
                {
                    yield return en.Current;
                }
            }
        }
        public static bool Contains<T>(this IList<T> dic, Func<T, bool> filter)
        {
            var en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                if (filter(en.Current))
                    return true;
            }
            return false;
        }

        public static int Max<T>(this IList<T> list, Func<T, int> filter)
        {
            var max = int.MinValue;
            for (int i = 0, len = list.Count; i < len; i++)
            {
                max = Math.Max(max, filter(list[i]));
            }
            return max;
        }
        public static int Min<T>(this IList<T> list, Func<T, int> filter)
        {
            var max = int.MaxValue;
            for (int i = 0, len = list.Count; i < len; i++)
            {
                max = Math.Min(max, filter(list[i]));
            }
            return max;
        }
        public static int Max<T, ST>(this IList<T> list, ST state, Func<T, ST, int> filter)
        {
            var max = int.MinValue;
            for (int i = 0, len = list.Count; i < len; i++)
            {
                max = Math.Max(max, filter(list[i], state));
            }
            return max;
        }
        public static int Min<T, ST>(this IList<T> list, ST state, Func<T, ST, int> filter)
        {
            var max = int.MaxValue;
            for (int i = 0, len = list.Count; i < len; i++)
            {
                max = Math.Min(max, filter(list[i], state));
            }
            return max;
        }
        #endregion

        #region IListExCondition
        public static T TryGetEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        return list[i];
                    }
                }
            }
            return default;
        }
        public static bool RemoveEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        list.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool ContainsEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static int IndexOfEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public static int LastIndexOfEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        public static T PopEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (int i = 0, iLen = list.Count; i < iLen; i++)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        var rst = list[i];
                        list.RemoveAt(i);
                        return rst;
                    }
                }
            }
            return default;
        }
        public static T LastPopEx<T, C>(this IList<T> list, C value)
            where T : IListExCondition<C>
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] is IListExCondition<C>)
                {
                    if (((IListExCondition<C>)list[i]).IsMatch(value))
                    {
                        var rst = list[i];
                        list.RemoveAt(i);
                        return rst;
                    }
                }
            }
            return default;
        }
        #endregion IListExCondition


        public static void SortRandom<T>(this IList<T> array)
        {
            int i, ti;
            var length = array.Count;
            for (i = 0; i < array.Count; i++)
            {
                ti = UnityEngine.Random.Range(i, length);
                (array[i], array[ti]) = (array[ti], array[i]);
            }
        }

        public static void CopyToAfterClear<T>(this IList<T> src, IList<T> dst)
        {
            dst.Clear();
            for (int i = 0, len = src.Count; i < len; i++)
            {
                dst.Add(src[i]);
            }
            src.Clear();
        }

        /// <summary>
        /// 딕셔너리를 제작하는데 key가 중복되면 무시하거나 failedKey 로직을 따른다.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="src"></param>
        /// <param name="keySelector"></param>
        /// <param name="failedKey"></param>
        /// <returns></returns>
        public static Dictionary<K, V> ToDictionaryIgnoreSameKeys<K, V>(this IList<V> src, Func<V, K> keySelector, Action<K, V> failedKey = null)
        {
            var dict = new Dictionary<K, V>();
            for (int i = 0, len = src.Count; i < len; i++)
            {
                var key = keySelector(src[i]);
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, src[i]);
                }
                else if (failedKey != null)
                {
                    failedKey(key, src[i]);
                }
            }
            return dict;
        }

        /// <summary>
        /// 딕셔너리를 제작하는데 key가 null일경우에는 무시하고 제작한다.
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="src"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static Dictionary<K, V> ToDictionaryIgnoreNull<K, V>(this IList<V> src, Func<V, K> keySelector)
            where K : class
        {
            var dict = new Dictionary<K, V>();
            for (int i = 0, len = src.Count; i < len; i++)
            {
                var key = keySelector(src[i]);
                if (key == null) continue;

                dict.Add(key, src[i]);
            }
            return dict;
        }

        [Obsolete("This method is heavy. Are you sure to use GetOrCreateSequences<T>()? Please use region #pragma warning disable CS0618 #pragma warning restore CS0618")]
        public static T GetOrCreateSequences<T>(this IList<T> src, int index)
            where T : new()
        {
            for (var i = src.Count; i <= index; i++)
            {
                src.Add(new T());
            }
            return src[index];
        }


        [Obsolete("Please use GetClamp().", true)]
        public static T GetOrClose<T>(this IList<T> src, int index) => src.GetClamp(index);
        public static T GetClamp<T>(this IList<T> src, int index)
        {
            if (src == null || src.Count == 0) throw new IndexOutOfRangeException("Array is empty.");

            if (index < 0) return src[0];
            else if (index >= src.Count) return src[src.Count - 1];
            else return src[index];
        }
        public static T GetLoop<T>(this IList<T> src, int index)
        {
            if (src == null || src.Count == 0) throw new IndexOutOfRangeException("Array is empty.");

            do
            {
                index += src.Count;
            }
            while (index < 0 && index < src.Count);

            return src[index % src.Count];
        }

        public static T GetLast<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) throw new IndexOutOfRangeException("Array is empty.");

            return src[src.Count - 1];
        }

        public static T GetFirst<T>(this IList<T> src)
        {
            if (src == null || src.Count == 0) throw new IndexOutOfRangeException("Array is empty.");

            return src[0];
        }

        public static int ReferenceIndexOf<T>(this IList<T> array, T target)
        {
            for (var i = 0; i < array.Count; i++)
            {
                if (ReferenceEquals(array[i], target))
                {
                    return i;
                }
            }
            return -1;
        }

        public static List<T> Clone<T>(this List<T> array)
        {
            return new List<T>(array);
        }

        public static List<T> LockClone<T>(this List<T> array)
        {
            lock (array)
            {
                return new List<T>(array);
            }
        }

        public static void LockClone<T>(this List<T> array, List<T> dst)
        {
            dst.Clear();
            lock (array)
            {
                dst.AddRange(array);
            }
        }

        public static void Swap<T>(this IList<T> list, int aIndex, int bIndex)
        {
            var a = list[aIndex];
            list[aIndex] = list[bIndex];
            list[bIndex] = a;
        }

        public static void CompareEvents<T>(this IList<T> prev, IList<T> next, Action<T> add = null, Action<T> remove = null, Action<T> same = null)
        {
            for (int i = 0, iLen = prev.Count; i < iLen; i++)
            {
                var pitem = prev[i];
                var idx = next.IndexOf(pitem);
                // 기존에 있는게 새로운 리스트에 없을 때
                if (idx == -1)
                {
                    remove?.Invoke(pitem);
                }
                else
                {
                    same?.Invoke(pitem);
                }
            }
            for (int i = 0, iLen = next.Count; i < iLen; i++)
            {
                var nitem = next[i];
                var idx = prev.IndexOf(nitem);
                // 새로운 리스트에 있는게 기존 리스트에 없을 때
                if (idx == -1)
                {
                    add?.Invoke(nitem);
                }
            }
        }

        /// <summary>
        /// 특정 길이부터 길이까지로 리스트를 변경한다.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public static void ForceResize<T>(this List<T> list, int offset, int count, Func<T> newHandle = null)
        {
            if (count <= 0) return;

            var removeRange = list.Count - (offset + count);
            if (removeRange > 0)
            {
                list.RemoveRange(list.Count - removeRange, removeRange);
            }
            else if (removeRange < 0)
            {
                for (int i = 0, len = -removeRange; i < len; i++)
                {
                    if (newHandle != null) list.Add(newHandle());
                    else list.Add(default!);
                }
            }

            if (offset > 0)
            {
                list.RemoveRange(0, offset);
            }
        }

        /// <summary>
        /// 입력받은 list를 랜덤하게 섞는다.
        /// Knuth 무작위 알고리즘 사용.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var rndIdx = UnityEngine.Random.Range(0, i + 1); // 0부터 i까지의 범위에서 랜덤한 정수를 선택합니다.

                var temp = list[i];
                list[i] = list[rndIdx];
                list[rndIdx] = temp;
            }
        }
    }
}
