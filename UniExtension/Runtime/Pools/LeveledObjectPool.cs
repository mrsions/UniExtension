#nullable enable

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UniExtension;
using UnityEngine;

namespace UniExtension.Pools
{
    public unsafe class LeveledObjectPool<T> : IObjectLevelPoolTrimmable where T : class
    {
        [ThreadStatic]
        private static TlsBucketSlot[]? tls_Buckets;
        private int _trimCallbackCreated;
        private readonly TlsLockStack?[] _Buckets;

        private readonly int _LevelLength;

        private readonly delegate*<int, int> _PatitionCount;
        private readonly delegate*<int, int> _CapacityPerPatition;
        //private readonly delegate*<T, void> _OnPop;
        //private readonly delegate*<T, void> _OnPush;
        //private readonly delegate*<T, void> _OnTrash;

        public LeveledObjectPool(int levelLength, delegate*<int, int> partitionCount, delegate*<int, int> capacityPerPatition)
        {
            this._LevelLength = levelLength;
            this._PatitionCount = partitionCount;
            this._CapacityPerPatition = capacityPerPatition;
            //this._OnPop = onPop;
            //this._OnPush = onPush;
            //this._OnTrash = onTrash;
            this._Buckets = new TlsLockStack[_LevelLength];
            PoolTrimRegistry.Register(this);
        }

        private TlsLockStack CreateLevelBucket(int level)
        {
            ref var partition = ref _Buckets[level];
            if (partition is not null) return partition;

            int maxPerPatition = _PatitionCount(level);
            Assert.IsRange(maxPerPatition, 0, int.MaxValue);

            int capacityPerPartition = _CapacityPerPatition(level);
            Assert.IsRange(maxPerPatition, 0, int.MaxValue);

            var inst = new TlsLockStack(maxPerPatition, capacityPerPartition);
            return Interlocked.CompareExchange(ref _Buckets[level], inst, null) ?? inst;
        }

        public T? TryPop(int level)
        {
            Assert.IsRange(level, 0, _LevelLength);

            var tlsBuckets = tls_Buckets;
            if (tlsBuckets is not null)
            {
                ref var slot = ref tlsBuckets[level];
                var obj = slot.Value;
                if (obj is not null)
                {
                    slot.Value = null;
                    slot.MillisecondsTimeStamp = 0;
                    //_OnPop(obj);
                    return obj;
                }
            }

            var buckets = _Buckets;
            if (buckets is not null)
            {
                var result = buckets[level]?.TryPop();
                if (result is not null)
                {
                    //_OnPop(result);
                    return result;
                }
            }

            return null;
        }

        public bool TryPush(int level, T obj)
        {
            Assert.IsRange(level, 0, _LevelLength);
            Assert.IsNotNull(obj);

            if ((uint)level < (uint)_LevelLength)
            {
                TlsBucketSlot[] tlsBuckets = tls_Buckets ?? InitializeTlsBuckets();
                ref var slot = ref tlsBuckets[level];
                if (slot.Value is not null)
                {
                    var partition = _Buckets[level] ?? CreateLevelBucket(level);
                    if (!partition.TryPush(slot.Value))
                    {
                        return false;
                    }
                }

                slot.Value = obj;
                slot.MillisecondsTimeStamp = 0;
                //_OnPush(obj);
                return true;
            }
            return false;
        }

        public bool Trim()
        {
            int currentMilliseconds = Environment.TickCount;
            MemoryPressure pressure = Settings.GetMemoryPressure();

            var levels = _Buckets;
            for (int i = 0; i < _LevelLength; i++)
            {
                levels[i]?.Trim(currentMilliseconds, pressure);
            }

            if (pressure == MemoryPressure.High)
            {
                foreach (var bucketPair in _AllTlsBuckets)
                {
                    var buckets = bucketPair.Key;
                    if (buckets is null) continue;

                    int max = Math.Min(buckets.Length, _LevelLength);
                    for (int i = 0; i < max; i++)
                    {
                        buckets[i].Value = null;
                        buckets[i].MillisecondsTimeStamp = 0;
                    }
                }
            }
            else
            {
                uint millisecondsThreshold = pressure == MemoryPressure.Medium ? 15_000u : 30_000u;

                foreach (var bucketPair in _AllTlsBuckets)
                {
                    var buckets = bucketPair.Key;
                    if (buckets is null) continue;

                    int max = Math.Min(buckets.Length, _LevelLength);
                    for (int i = 0; i < max; i++)
                    {
                        ref var slot = ref buckets[i];
                        if (slot.Value is null)
                        {
                            slot.MillisecondsTimeStamp = 0;
                            continue;
                        }

                        int lastSeen = slot.MillisecondsTimeStamp;
                        if (lastSeen == 0)
                        {
                            slot.MillisecondsTimeStamp = currentMilliseconds;
                        }
                        else if ((uint)(currentMilliseconds - lastSeen) >= millisecondsThreshold)
                        {
                            slot.Value = null;
                            slot.MillisecondsTimeStamp = 0;
                        }
                    }
                }
            }

            return true;
        }

        public void TrimForLowMemory()
        {
            Trim();
        }

        private readonly ConditionalWeakTable<TlsBucketSlot[], object?> _AllTlsBuckets = new();

        private TlsBucketSlot[] InitializeTlsBuckets()
        {
            Assert.IsNull(tls_Buckets);

            var tlsBucket = new TlsBucketSlot[_LevelLength];
            tls_Buckets = tlsBucket;

            _AllTlsBuckets.Add(tlsBucket, null);

            // TODO: 자동정리 기능 구현 필요?
            if (Interlocked.Exchange(ref _trimCallbackCreated, 1) == 0)
            {
                Application.lowMemory += () =>
                {
                    Trim();
                };
            }

            return tlsBucket;
        }

        private struct TlsBucketSlot
        {
            public T? Value;
            public int MillisecondsTimeStamp;
        }

        private sealed class TlsLockStack
        {
            private readonly PartitionStack[] _Partitions;

            public TlsLockStack(int partitionCount, int capacityPerPartition)
            {
                var partitions = new PartitionStack[partitionCount];
                for (int i = 0, len = partitions.Length; i < len; i++)
                {
                    partitions[i] = new(capacityPerPartition);
                }
                _Partitions = partitions;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T? TryPop()
            {
                var partitions = _Partitions;
                int pLength = partitions.Length;
                T? result;

                int index = (int)((uint)Thread.GetCurrentProcessorId() % (uint)pLength);

                for (int i = 0; i < pLength; i++)
                {
                    result = partitions[index].TryPop();

                    if (result is not null) return result;

                    if (++index == partitions.Length) index = 0;
                }

                return null;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryPush(T obj)
            {
                var partitions = _Partitions;
                int pLength = partitions.Length;

                int index = (int)((uint)Thread.GetCurrentProcessorId() % (uint)pLength);
                for (int i = 0; i < pLength; i++)
                {
                    if (partitions[index].TryPush(obj)) return true;
                    if (++index == partitions.Length) index = 0;
                }

                return false;
            }

            public void Trim(int currentMilliseconds, MemoryPressure pressure)
            {
                var partitions = _Partitions;
                for (int i = 0; i < partitions.Length; i++)
                {
                    partitions[i].Trim(currentMilliseconds, pressure);
                }
            }
        }
        private sealed class PartitionStack
        {
            private T?[] _Objects;
            private int _Count = 0;
            private int _LastTrimMilliseconds = 0;

            public PartitionStack(int count)
            {
                _Objects = new T[count];
            }


            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public T? TryPop()
            {
                T?[] objects = _Objects;
                uint objectsLength = (uint)objects.Length;
                T? result = null;

                lock (this)
                {
                    int count = _Count - 1;
                    if ((uint)count < objectsLength)
                    {
                        result = objects[count];
                        objects[count] = null;
                        _Count = count;
                    }
                }

                return result;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool TryPush(T obj)
            {
                T?[] objects = _Objects;
                int objectsLength = objects.Length;
                bool ok = false;

                lock (this)
                {
                    int count = _Count;
                    if (count < objectsLength)
                    {
                        objects[count] = obj;
                        _Count = count + 1;
                        ok = true;
                    }
                }

                return ok;
            }

            public void Trim(int currentMilliseconds, MemoryPressure pressure)
            {
                if (pressure == MemoryPressure.High)
                {
                    ClearInternal();
                    return;
                }

                uint millisecondsThreshold = pressure == MemoryPressure.Medium ? 15_000u : 30_000u;

                lock (this)
                {
                    int count = _Count;
                    if (count <= 0)
                    {
                        _LastTrimMilliseconds = 0;
                        return;
                    }

                    int lastSeen = _LastTrimMilliseconds;
                    if (lastSeen == 0)
                    {
                        _LastTrimMilliseconds = currentMilliseconds;
                        return;
                    }

                    if ((uint)(currentMilliseconds - lastSeen) >= millisecondsThreshold)
                    {
                        Array.Clear(_Objects, 0, count);
                        _Count = 0;
                        _LastTrimMilliseconds = 0;
                    }
                }
            }

            private void ClearInternal()
            {
                lock (this)
                {
                    int count = _Count;
                    if (count <= 0)
                    {
                        _LastTrimMilliseconds = 0;
                        return;
                    }

                    Array.Clear(_Objects, 0, count);
                    _Count = 0;
                    _LastTrimMilliseconds = 0;
                }
            }
        }
    }
}
