using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniExtension;
using Unity.Scripting.LifecycleManagement;

namespace UniExtension.Pools
{
    public sealed class DictionaryPool<TKey, TValue> : IObjectPool<Dictionary<TKey, TValue>>
        where TKey : notnull
    {
        [AutoStaticsCleanup]
        private static DictionaryPool<TKey, TValue>? s_shared;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static RentContext<Dictionary<TKey, TValue>> SharedRent()
        {
            s_shared ??= new();
            return s_shared.Rent();
        }

        private readonly Stack<Dictionary<TKey, TValue>> m_Pools = new();

        public int InitCount { get; private set; }
        public int ExpandCount { get; set; }

        public DictionaryPool(int initCount = 10, int expandCount = 10)
        {
            InitCount = initCount;
            ExpandCount = expandCount;
            Expand(initCount);
        }

        private void Expand(int initCount)
        {
            for (int i = 0; i < ExpandCount; i++)
            {
                m_Pools.Push(Create());
            }
        }

        public Dictionary<TKey, TValue> Create()
        {
            return new Dictionary<TKey, TValue>(10);
        }

        public RentContext<Dictionary<TKey, TValue>> Rent()
        {
            if (m_Pools.Count == 0)
            {
                Expand(ExpandCount);
            }

            return new(this, m_Pools.Pop());
        }

        public void Release(RentContext<Dictionary<TKey, TValue>> obj)
        {
            if (obj.HasReleased || obj.Pool != this)
            {
                throw new ArgumentException("");
            }

            obj.Value.Clear();
            m_Pools.Push(obj.Value);
        }
    }
}
