#nullable enable

using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace UniExtension
{
    public interface IPoolable
    {
        void OnCreate();
        void OnRent();
        void OnRelease();
    }

    public interface IObjectPool<T>
        where T : class, new()
    {
        T Create();
        RentContext<T> Rent();
        void Release(RentContext<T> obj);
    }

    public struct RentContext<T> : IDisposable
        where T : class, new() // 생성자 확보를 위해 지정
    {
        internal readonly IObjectPool<T> Pool;
        internal bool m_Released;

        public readonly T Value;

        public bool HasReleased
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_Released;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set => m_Released = value;
        }

        internal RentContext(IObjectPool<T> pool, T value)
        {
            Assert.IsTrue(pool != null);
            Assert.IsTrue(value != null);

            Pool = pool;
            Value = value;
            m_Released = false;
        }

        public void Dispose()
        {
            if (m_Released) return;
            Pool.Release(this);
            m_Released = true;
        }

        internal void ThrowIfReleased()
        {
            if (m_Released)
            {
                throw new InvalidOperationException("Alread released.");
            }
        }
    }

    public sealed class ObjectPool<T> : IObjectPool<T>
        where T : class, new()
    {
        private static ObjectPool<T>? s_shared;

        public static RentContext<T> Shared
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                s_shared ??= new();
                return s_shared.Rent();
            }
        }

        private readonly ConcurrentQueue<T> m_pools = new();

        public int InitCount { get; private set; }
        public int ExpandCount { get; set; }

        public ObjectPool(int initCount = 1, int expandCount = 1)
        {
            InitCount = initCount;
            ExpandCount = expandCount;
            Expand(initCount);
        }

        private void Expand(int initCount)
        {
            for (int i = 0; i < ExpandCount; i++)
            {
                m_pools.Enqueue(Create());
            }
        }

        public T Create()
        {
            var obj = new T();
            (obj as IPoolable)?.OnCreate();
            return obj;
        }

        public RentContext<T> Rent()
        {
            if (m_pools.Count == 0)
            {
                Expand(ExpandCount);
            }

            if (!m_pools.TryDequeue(out var obj))
            {
                obj = Create();
            }

            (obj as IPoolable)?.OnRent();
            return new(this, obj);
        }

        public T Take()
        {
            return Rent().Value;
        }

        public void Release(RentContext<T> obj)
        {
            obj.ThrowIfReleased();

            if (obj.Pool != this)
            {
                throw new ArgumentException("");
            }

            (obj as IPoolable)?.OnRelease();
            m_pools.Enqueue(obj.Value);
        }

        public void Release(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            (obj as IPoolable)?.OnRelease();
            m_pools.Enqueue(obj);
        }
    }
}
