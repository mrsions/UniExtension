#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UniExtension.Pools;

namespace UniExtension.Collections
{
    public class PList<T> : List<T>, IDisposable
    {
        internal PList() : base(10) { }
        internal PList(int capacity) : base(capacity) { }

        internal bool Rented;

#if DEBUG
        public string? EditorStackTrace { get; internal set; }
#endif
        public void Dispose()
        {
            PList.InternalPool<T>.Instance.Release(this);
        }
    }

    public class PList
    {
        public static PList<T> Take<T>()
        {
            return InternalPool<T>.Instance.Take(0);
        }

        public static PList<T> Take<T>(int capacity)
        {
            return InternalPool<T>.Instance.Take(capacity);
        }

        public static PList<T> Take<T>(IEnumerable<T> enumerable)
        {
            var list = Take<T>(enumerable.Count());
            list.AddRange(enumerable);
            return list;
        }

        internal unsafe class InternalPool<T> : Singleton<InternalPool<T>>
        {

            private LeveledObjectPool<PList<T>> pool;

            public InternalPool()
            {
                pool = new(LevelLength, &GetPartitionCount, &GetCapacityPerPartition);
            }

            public PList<T> Take(int capacity)
            {
                int level = Math.Min(GetLevel(capacity), LevelLength - 1);
#if !DEBUG
        FIRST:
#endif
                var list = pool.TryPop(level);
                if (list == null)
                {
                    list = new(capacity);
                }
                else if (list.Rented)
                {
#if DEBUG
                    throw new SystemException($"It's already rented. (from:{list.EditorStackTrace})");
#else
                goto FIRST; // 런타임에서는 무시함
#endif
                }
                else if (capacity > list.Capacity)
                {
                    list.Capacity = capacity;
                }

                list.Rented = true;
#if DEBUG
                list.EditorStackTrace = Environment.StackTrace;
#endif
                //OnTake(list);
                return list;
            }

            public void Release(PList<T> list)
            {
                if (!list.Rented)
                {
#if DEBUG
                    throw new SystemException($"It's already released. (from:{list.EditorStackTrace})");
#else
                return; // 런타임에서는 무시함
#endif
                }

#if DEBUG
                list.EditorStackTrace = Environment.StackTrace;
#endif
                list.Rented = false;

                int level = GetLevel(list.Capacity);
                if (level < LevelLength && pool.TryPush(level, list))
                {
                    //OnRelease(list);
                    list.Clear();
                }
                else
                {
                    //OnTrash(list);
                    list.Clear();
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
