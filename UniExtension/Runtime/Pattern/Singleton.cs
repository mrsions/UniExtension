#nullable enable
#line hidden

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UniExtension
{
    public abstract class Singleton<T>
        where T : class
    {
        private static T? s_main;

        public static T main
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var main = s_main;
                if (main == null)
                {
                    lock (typeof(Singleton<T>))
                    {
                        main = Volatile.Read(ref s_main);
                        if(main == null)
                        {
                            main = Activator.CreateInstance<T>();
                            Volatile.Write(ref s_main, main);
                        }
                    }
                }
                return main;
            }
        }

        public static bool hasMain
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => s_main != null;
        }

        public Singleton()
        {
            Assert.IsTrue(typeof(T).IsSubclassOf(typeof(Singleton<T>)));

    #if HAS_SYSTEM_RUNTIME_COMPILERSERVICES_UNSAFE
            s_main = Unsafe.As<T>(this);
    #else
            s_main = this as T;
    #endif
        }
    }
}
