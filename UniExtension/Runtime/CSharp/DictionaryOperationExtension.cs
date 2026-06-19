namespace System.Collections.Generic
{
    public static class DictionaryOperationExtension
    {
        public static float AddOrIncrease<K>(this IDictionary<K, float> dict, K key, float value)
        {
            if (dict.TryGetValue(key, out var v)) return dict[key] = v + value;
            else return dict[key] = value;
        }
        public static int AddOrIncrease<K>(this IDictionary<K, int> dict, K key, int value)
        {
            if (dict.TryGetValue(key, out var v)) return dict[key] = v + value;
            else return dict[key] = value;
        }
    }
}