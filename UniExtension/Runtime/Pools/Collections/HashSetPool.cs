using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniExtension;

namespace UniExtension.Pools
{
    public sealed class HashSetPool<T> : IObjectPool<HashSet<T>>
    {
        private static HashSetPool<T>? s_shared;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static RentContext<HashSet<T>> SharedRent()
        {
            s_shared ??= new();
            return s_shared.Rent();
        }

        private readonly Stack<HashSet<T>> m_Pools = new();

        public int InitCount { get; private set; }
        public int ExpandCount { get; set; }

        public HashSetPool(int initCount = 10, int expandCount = 10)
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

        public HashSet<T> Create()
        {
            return new HashSet<T>(10);
        }

        public RentContext<HashSet<T>> Rent()
        {
            if (m_Pools.Count == 0)
            {
                Expand(ExpandCount);
            }

            return new(this, m_Pools.Pop());
        }

        public void Release(RentContext<HashSet<T>> obj)
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
