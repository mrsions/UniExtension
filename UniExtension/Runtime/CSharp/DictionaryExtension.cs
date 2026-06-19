namespace System.Collections.Generic
{
    public static class DictionaryExtension
    {
        /// <summary>
        /// key가 dictionary에 없을경우에만 추가합니다.
        /// </summary>
        public static bool TryAdd<K, T>(this IDictionary<K, T> dict, K key, T val)
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, val);
                return true;
            }
            return false;
        }

        /// <summary>
        /// key가 dictionary에 존재할경우에만 덮어씁니다.
        /// </summary>
        /// <returns>이미 키값이 존재하여 교체에 성공했을 때 True를 반환합니다.</returns>
        public static bool UpsertExists<K, T>(this IDictionary<K, T> dic, K key, T value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 딕셔너리를 딕셔너리에 추가합니다. src의 타입 상속관계도 허용합니다.
        /// </summary>
        public static void AddRange<TKey, TVal, TEnum>(this IDictionary<TKey, TVal> dict, TEnum src)
            where TEnum : IEnumerable<KeyValuePair<TKey, TVal>>
        {
            foreach (var pair in src)
            {
                dict.Add(pair.Key, pair.Value);
            }
        }

        public static void TryAddRange<TKey, TVal, TEnum>(this IDictionary<TKey, TVal> dict, TEnum src)
            where TEnum : IEnumerable<KeyValuePair<TKey, TVal>>
        {
            foreach (var pair in src)
            {
                if (!dict.ContainsKey(pair.Key))
                {
                    dict.Add(pair.Key, pair.Value);
                }
            }
        }
        public static void UpsertRangeExists<TKey, TVal, TEnum>(this IDictionary<TKey, TVal> dict, TEnum src)
            where TEnum : IEnumerable<KeyValuePair<TKey, TVal>>
        {
            foreach (var pair in src)
            {
                if (dict.ContainsKey(pair.Key))
                {
                    dict[pair.Key] = pair.Value;
                }
            }
        }

        /// <summary>
        /// 딕셔너리에 딕셔너리를 추가합니다. src의 타입 상속관계도 허용합니다. 또한 Dictionary.Put과 같은 원리로 동작합니다.
        /// </summary>
        public static void UpsertRange<TKey, TVal, TEnum>(this IDictionary<TKey, TVal> dict, TEnum src)
            where TEnum : IEnumerable<KeyValuePair<TKey, TVal>>
        {
            foreach (var pair in src)
            {
                dict[pair.Key] = pair.Value;
            }
        }

        /// <summary>
        /// Get Simply TryGet In Dictionary
        /// </summary>
        /// <returns>If TryGet fails, Return the null</returns>
        public static T Get<K, T>(this IDictionary<K, T> dict, K key)
        {
            if (dict.TryGetValue(key, out var obj))
            {
                return obj;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Get Simply TryGet In Dictionary
        /// </summary>
        /// <returns>If TryGet fails, Return the value 'def'</returns>
        public static T Get<K, T>(this IDictionary<K, T> dict, K key, T def)
        {
            T obj;
            if (dict.TryGetValue(key, out obj))
            {
                return obj;
            }
            else
            {
                return def;
            }
        }

        /// <summary>
        /// 가져올 대상이 없다면 새로운대상을 만들어서 채워넣고 반환합니다.
        /// </summary>
        /// <returns>If TryGet fails, Return the null</returns>
        public static T GetOrNew<K, T>(this IDictionary<K, T> dict, K key)
            where T : class, new()
        {
            T obj;
            if (dict.TryGetValue(key, out obj))
            {
                return obj;
            }
            else
            {
                obj = new T();
                dict[key] = obj;
                return obj;
            }
        }

        /// <summary>
        /// 가져올 대상이 없다면 새로운대상을 만들어서 채워넣고 반환합니다.
        /// * Dotnet5 표준
        /// </summary>
        /// <returns>If TryGet fails, Return the null</returns>
        public static T GetValueOrDefault<K, T>(this IDictionary<K, T> dict, K key)
        {
            if (dict.TryGetValue(key, out var obj))
            {
                return obj;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Get Simply TryGet In Dictionary
        /// </summary>
        /// <returns>If TryGet fails, Return the value 'def'</returns>
        public static T Get<K, T>(this IDictionary<K, T> dict, K key, Func<IDictionary<K, T>, T> def)
        {
            T obj;
            if (dict.TryGetValue(key, out obj))
            {
                return obj;
            }
            else
            {
                return def(dict);
            }
        }
        /// <summary>
        /// Get Simply TryGet In Dictionary
        /// </summary>
        /// <returns>If TryGet fails, Return the value 'def'</returns>
        public static T Get<K, T>(this IDictionary<K, T> dict, K key, Func<T> def)
        {
            T obj;
            if (dict.TryGetValue(key, out obj))
            {
                return obj;
            }
            else
            {
                return def();
            }
        }

        /// <summary>
        /// 특정 조건에 대해서만 갯수를 계산합니다.
        /// </summary>
        public static int CountIf<K, T>(this IDictionary<K, T> dic, Func<T, bool> filter)
        {
            if (filter == null)
                return dic.Count;

            var count = 0;
            var en = dic.GetEnumerator();
            while (en.MoveNext())
            {
                if (filter(en.Current.Value))
                    count++;
            }
            return count;
        }
    }
}