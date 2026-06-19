#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UniExtension.Pools;

namespace UniExtension.Collections
{
    public class PDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IDisposable
    {
        internal PDictionary() : base(10)
        {
            capacity = 10;
        }
        internal PDictionary(int capacity) : base(capacity)
        {
            this.capacity = capacity;
        }

        internal bool Rented;
        internal int capacity;

#if DEBUG
        public string? EditorStackTrace { get; internal set; }
#endif
        public void Dispose()
        {
            PDictionary.InternalPool<TKey, TValue>.main.Release(this);
        }
    }

    public class PDictionary
    {
        public static PDictionary<TKey, TValue> Take<TKey, TValue>()
        {
            return InternalPool<TKey, TValue>.main.Take(0);
        }

        public static PDictionary<TKey, TValue> Take<TKey, TValue>(int capacity)
        {
            return InternalPool<TKey, TValue>.main.Take(capacity);
        }

        public static PDictionary<TKey, TValue> Take<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            var dict = Take<TKey, TValue>(enumerable.Count());
            DictionaryExtension.AddRange(dict, enumerable);
            return dict;
        }

        public static PDictionary<TKey, TValue> Take<TKey, TValue>(IDictionary<TKey, TValue> inDict)
        {
            int capacity = 0;
            if (inDict is ICollection col) capacity = col.Count;
            else if (inDict is ICollection<KeyValuePair<TKey, TValue>> colt) capacity = colt.Count;

            var dict = Take<TKey, TValue>(capacity);
            DictionaryExtension.AddRange(dict, inDict);
            return dict;
        }


        internal unsafe class InternalPool<TKey, TValue> : Singleton<InternalPool<TKey, TValue>>
        {

            private LeveledObjectPool<PDictionary<TKey, TValue>> pool;

            public InternalPool()
            {
                pool = new(LevelLength, &GetPartitionCount, &GetCapacityPerPartition);
            }

            public PDictionary<TKey, TValue> Take(int capacity)
            {
                int level = Math.Min(GetLevel(capacity), LevelLength - 1);
#if !DEBUG
        FIRST:
#endif
                var dict = pool.TryPop(level);
                if (dict == null)
                {
                    dict = new(capacity);
                }
                else if (dict.Rented)
                {
#if DEBUG
                    throw new SystemException($"It's already rented. (from:{dict.EditorStackTrace})");
#else
                goto FIRST; // 런타임에서는 무시함
#endif
                }
                else
                {
                    dict.EnsureCapacity(capacity);
                }

                dict.Rented = true;
#if DEBUG
                dict.EditorStackTrace = Environment.StackTrace;
#endif
                return dict;
            }

            public void Release(PDictionary<TKey, TValue> dict)
            {
                if (!dict.Rented)
                {
#if DEBUG
                    throw new SystemException($"It's already released. (from:{dict.EditorStackTrace})");
#else
                return; // 런타임에서는 무시함
#endif
                }

#if DEBUG
                dict.EditorStackTrace = Environment.StackTrace;
#endif
                dict.Rented = false;

                int level = GetLevel(dict.capacity = dict.EnsureCapacity(0));
                if (level < LevelLength && pool.TryPush(level, dict))
                {
                    //OnRelease(dict);
                    dict.Clear();
                }
                else
                {
                    //OnTrash(dict);
                    dict.Clear();
                }
            }

            private static int GetPartitionCount(int level)
            {
                return SharedPoolStatics.PartitionCount;
            }

            private static int GetCapacityPerPartition(int level)
            {
                return SharedPoolStatics.CapacityPerPartition;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int GetLevel(int capacity)
            {
                if (capacity < 100)
                {
                    return 0;
                }
                else if (capacity < 200)
                {
                    return 1;
                }
                else if (capacity < 2_000)
                {
                    return 2;
                }
                else if (capacity < 10_000)
                {
                    return 3;
                }
                else if (capacity < 100_000)
                {
                    return 4;
                }
                else if (capacity < 1_000_000)
                {
                    return 5;
                }
                else
                {
                    return LevelLength;
                }
            }

            private const int LevelLength = 6;
        }
    }
}
