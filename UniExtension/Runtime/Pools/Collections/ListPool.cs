using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniExtension;
using System.Threading;
using Unity.Scripting.LifecycleManagement;

namespace UniExtension.Pools
{
    public sealed class ListPool<T> : IObjectPool<List<T>>
    {
        [AutoStaticsCleanup]
        private static ListPool<T>? s_shared;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static RentContext<List<T>> SharedRent()
        {
            s_shared ??= new();
            return s_shared.Rent();
        }

        private readonly Stack<List<T>> m_Pools = new();

        public int InitCount { get; private set; }
        public int ExpandCount { get; set; }

        public ListPool(int initCount = 10, int expandCount = 10)
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

        public List<T> Create()
        {
            return new List<T>(10);
        }

        public RentContext<List<T>> Rent()
        {
            if (m_Pools.Count == 0)
            {
                Expand(ExpandCount);
            }

            return new(this, m_Pools.Pop());
        }

        public void Release(RentContext<List<T>> obj)
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
