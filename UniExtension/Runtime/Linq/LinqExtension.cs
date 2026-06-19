using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniExtension
{
    public static class LinqExtension
    {
        /// <summary>
        /// 열거자의 첫번째 값의 특정 변수를 이용하여 where를 진행한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="e"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereViaFirstElement<T, F>(this IEnumerable<T> e, Func<T, F> select, Func<T, F, bool> where)
        {
            var first = e.FirstOrDefault();
            var val = select(first);

            foreach (var t in e)
            {
                if (where(t, val))
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        /// 열거자의 첫번째 값의 특정 변수를 이용하여 where를 진행한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="e"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static T ToRandom<T>(this IEnumerable<T> e, Func<T, float> getWeight = null)
        {
            if (e == null) return default;

            using (var list = e.ToPList())
            {
                if (list.Count > 1)
                {
                    if (getWeight != null)
                    {
                        float totalWeight = 0;
                        for (var i = 0; i < list.Count; i++)
                        {
                            totalWeight += getWeight(list[i]);
                        }

                        var rnd = UnityEngine.Random.Range(0, totalWeight);

                        totalWeight = 0;
                        for (var i = 0; i < list.Count; i++)
                        {
                            totalWeight += getWeight(list[i]);
                            if (rnd <= totalWeight)
                            {
                                return list[i];
                            }
                        }
                    }
                    else
                    {
                        return list[UnityEngine.Random.Range(0, list.Count)];
                    }
                }
                else if (list.Count == 1)
                {
                    return list[0];
                }
                return default;
            }
        }

        public static T MinObject<T>(this IEnumerable<T> e, Func<T, float> prediction = null)
        {
            var minValue = float.MaxValue;
            T minObj = default;
            foreach (var m in e)
            {
                var v = prediction(m);
                if (minValue == float.MaxValue || v < minValue)
                {
                    minValue = v;
                    minObj = m;
                }
            }
            return minObj;
        }

        public static int MinIndex<T>(this IEnumerable<T> e, Func<T, float> prediction = null)
        {
            var minValue = float.MaxValue;
            var minIndex = -1;
            var i = 0;
            foreach (var m in e)
            {
                var v = prediction(m);
                if (minValue == float.MaxValue || v < minValue)
                {
                    minValue = v;
                    minIndex = i;
                }
                i++;
            }
            return minIndex;
        }

        public static int IndexOf<T>(this IEnumerable<T> e, Func<T, bool> prediction = null)
        {
            var i = 0;
            foreach (var m in e)
            {
                if (prediction(m))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> prediction = null)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (prediction(list[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// array 목록을 머지하는데 사용한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="en"></param>
        /// <returns></returns>
        public static T[] BlockMerge<T>(this IEnumerable<T[]> en)
        {
            var len = en.Sum(v => v.Length);
            var rst = new T[len];

            var offset = 0;
            foreach (var tm in en)
            {
                Array.Copy(tm, 0, rst, offset, tm.Length);
                offset += tm.Length;
            }

            return rst;
        }

        public static Dictionary<K, int> ToDictionaryWithIndex<K, V>(this IEnumerable<V> en, Func<V, K> keySelector)
        {
            var result = new Dictionary<K, int>();
            var index = 0;
            foreach (var e in en)
            {
                var key = keySelector(e);
                result[key] = index++;
            }
            return result;
        }

        public static Dictionary<K, T> ToDictionaryWithIndex<K, V, T>(this IEnumerable<V> en, Func<V, K> keySelector, Func<int, V, T> elemSelector)
        {
            var result = new Dictionary<K, T>();
            var index = 0;
            foreach (var e in en)
            {
                var key = keySelector(e);
                var value = elemSelector(index++, e);
                result[key] = value;
            }
            return result;
        }

        public static IEnumerable<TResult> SelectIndex<TSource, TResult>(this IEnumerable<TSource> e, Func<TSource, int, TResult> selector)
        {
            var i = 0;
            foreach (var m in e)
            {
                yield return selector(m, i++);
            }
        }

        /// <summary
        /// null이 아닌 대상만을 추출한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="e"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static IEnumerable<F> AsCast<T, F>(this IEnumerable<T> e)
        {
            foreach (var t in e)
            {
                if (t is F v) yield return v;
            }
        }

        /// <summary
        /// 조건에 맞게 바로 list를 만든다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="F"></typeparam>
        /// <param name="e"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IEnumerable<T> e, Func<T, bool> predicate)
        {
            var list = new List<T>();
            foreach (var i in e)
            {
                if (predicate(i))
                {
                    list.Add(i);
                }
            }
            return list;
        }

        /// <summary>
        /// 전체 열거자의 루프 계산을 실시한다.
        /// </summary>
        /// <typeparam name="T">입력타입</typeparam>
        /// <typeparam name="R">반환타입</typeparam>
        /// <param name="e"></param>
        /// <param name="prediction">반환타입산출</param>
        /// <param name="merge">반환타입연산</param>
        /// <param name="mergeToFirst">true일 경우 마지막과 첫번째를 연산함</param>
        /// <returns></returns>
        /// <example>
        /// </example>
        public static R Calculate<T, R>(this IEnumerable<T> e, Func<T, T, R> prediction, Func<R, R, R> merge, bool mergeToFirst = false)
        {
            var et = e.GetEnumerator();
            if (!et.MoveNext()) return default;

            var result = default(R);
            var first = et.Current;
            var before = first;
            T current;

            while (et.MoveNext())
            {
                current = et.Current;
                var funcResult = prediction(before, current);
                before = current;
                result = merge(result, funcResult);
            }

            if (mergeToFirst)
            {
                var funcResult = prediction(before, first);
                result = merge(result, funcResult);
            }

            return result;
        }

        /// <summary>
        /// Vector3 열거형의 길이를 구한다.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="closeLength">true일 경우 첫번째와 마지막 포인트의 길이까지 합산한다.</param>
        /// <returns></returns>
        public static float Length(this IEnumerable<Vector3> e, bool closeLength = false)
        {
            return e.Calculate(static (a, b) => Vector3.Distance(a, b), static (a, b) => a + b, closeLength);
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> func)
        {
            foreach (var v in e)
            {
                func(v);
            }
        }
    }
}
