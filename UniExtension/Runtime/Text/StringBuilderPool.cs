#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UniExtension.Pools;

namespace UniExtension.Text
{
    public static class StringBuilderPool
    {
        private static InternalStringBuilderPool? s_pool;

        internal static InternalStringBuilderPool? GetPool()
        {
            return s_pool ??= new();
        }

        internal unsafe class InternalStringBuilderPool
        {
            private LeveledObjectPool<StringBuilder> _pool;

            public InternalStringBuilderPool()
            {
                _pool = new(LevelLength, &GetPartitionCount, &GetCapacityPerPartition);
            }

#if DEBUG
            private object _lock = new();
            private Dictionary<StringBuilder, string> rented = new();
            private Dictionary<StringBuilder, string> released = new();
#endif
            [Conditional("DEBUG")]
            private void ValidateRent(StringBuilder sb)
            {
#if DEBUG
                lock (_lock)
                {
                    if (rented.TryGetValue(sb, out var stack))
                    {
                        throw new ArgumentException($"It's already rented. (from:{stack})");
                    }

                    if (!released.Remove(sb))
                    {
                        throw new ArgumentException($"Not rented object.");
                    }

                    rented[sb] = Environment.StackTrace;
                }
#endif
            }
            [Conditional("DEBUG")]
            private void ValidateRelease(StringBuilder sb)
            {
#if DEBUG
                lock (_lock)
                {
                    if (released.TryGetValue(sb, out var stack))
                    {
                        throw new ArgumentException($"It's already released. (from:{stack})");
                    }

                    if (!rented.Remove(sb))
                    {
                        throw new ArgumentException($"Not rented object.");
                    }

                    released[sb] = Environment.StackTrace;
                }
#endif
            }
            [Conditional("DEBUG")]
            private void ValidateNew(StringBuilder sb)
            {
#if DEBUG
                lock (_lock)
                {
                    released[sb] = "New";
                }
#endif
            }

            public StringBuilder Take(int capacity)
            {
                int level = GetLevel(capacity);

                StringBuilder? sb;

                // 너무 큰 객체를 요구한것이기때문에 마지막것을 사용하도록 한다.
                if (level == LevelLength)
                {
                    sb = _pool.TryPop(LevelLength - 1);
                    if (sb == null)
                    {
                        sb = new(capacity / 2); // 너무 요구사항이 크기때문에 절반만 할당한다.
                        ValidateNew(sb);
                    }
                }
                else
                {
                    sb = _pool.TryPop(level);
                    if (sb == null)
                    {
                        sb = new(capacity);
                        ValidateNew(sb);
                    }
                    else if (capacity > sb.Capacity)
                    {
                        sb.Capacity = capacity;
                    }
                }

                ValidateRent(sb);
                sb.Clear();
                return sb;
            }

            public void Release(StringBuilder sb)
            {
                ValidateRelease(sb);
                int level = GetLevel(sb.Capacity);
                if (level < LevelLength && _pool.TryPush(level, sb))
                {
                    //OnRelease(sb);
                }
                else
                {
                    //OnTrash(sb);
                }
            }

            private static int GetPartitionCount(int level)
            {
                return SharedPoolStatics.PartitionCount;
            }

            private static int GetCapacityPerPartition(int level)
            {
                return LevelLength - level;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int GetLevel(int capacity)
            {
                if (capacity < (1 << 7))
                {
                    return 0;
                }
                else if (capacity < (1 << 8))
                {
                    return 1;
                }
                else if (capacity < (1 << 9))
                {
                    return 2;
                }
                else if (capacity < (1 << 10))
                {
                    return 3;
                }
                else if (capacity < (1 << 11))
                {
                    return 4;
                }
                else if (capacity < (1 << 12))
                {
                    return 5;
                }
                else if (capacity < (1 << 13))
                {
                    return 6;
                }
                else if (capacity < (1 << 14))
                {
                    return 7;
                }
                else if (capacity < 0x80_00_00)
                {
                    return 8;
                }
                else
                {
                    return LevelLength;
                }
            }

            private const int LevelLength = 9;

        }
    }
}
