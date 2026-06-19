#nullable enable
//#line hidden

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace IDaNote.Utils
{
    public static class LogFactory
    {
        private static Dictionary<string, ILog>? s_Loggers;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void cctor()
        {
            s_Loggers = new();
        }

#if ENABLE_STATIC_SINGLETON_LOGGER
        public static ILog Get<T>(T obj)
        {
            return Log<T>.s_Instance ??= new Log<T>(typeof(T).Name, obj as UnityEngine.Object);
        }
        //[ThreadSafe]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ILog Get<T>()
        {
            return Log<T>.s_Instance ??= new Log<T>(typeof(T).Name, null);
        }
#else
        public static ILog Get<T>(T obj)
        {
            ValidEditor();
            return Get(typeof(T).Name);
        }
        public static ILog Get<T>()
        {
            ValidEditor();
            return Get(typeof(T).Name);
        }
#endif


        //[ThreadSafe]
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ILog Get(string tag)
        {
            ValidEditor();
            if (s_Loggers.TryGetValue(tag, out var log))
            {
                return log;
            }
            else
            {
                return s_Loggers[tag] = new Log<object>(tag, null);
            }
        }

        [Conditional("UNITY_EDITOR")]
        [MemberNotNull(nameof(s_Loggers))]
        private static void ValidEditor()
        {
            s_Loggers ??= new();
        }
    }

    public interface ILog
    {
        public const string LOG_FORMAT = "{0} {1,4} [{2}] {3}";

        ILogHandler LogHandler { get; }
        string Tag { get; }
        UnityEngine.Object? Context { get; }
        void Info(object message);
        void Error(object message);
        void Exception(Exception exception);
        void Warning(object message);
    }
    public interface ILogVerb
    {
        void Verb(object message);
    }

    class Log<T> : ILog, ILogVerb
    {
#if ENABLE_STATIC_SINGLETON_LOGGER
        internal static Log<T>? s_Instance;
#endif

        private string m_Tag;
        private UnityEngine.Object? m_Context;

        public string Tag => m_Tag;
        public UnityEngine.Object? Context => m_Context;

        internal ILogHandler m_Handler;

        public Log(string tag, UnityEngine.Object? context)
        {
            m_Tag = tag;
            m_Handler = Debug.unityLogger.logHandler;
            m_Context = context;
        }

        public ILogHandler LogHandler => m_Handler;
        public void Info(object message) => LogUtility.LogFormat(m_Handler, LogType.Log, m_Context, ILog.LOG_FORMAT, "I", Environment.CurrentManagedThreadId, m_Tag, message);
        public void Warning(object message) => LogUtility.LogFormat(m_Handler, LogType.Warning, m_Context, ILog.LOG_FORMAT, "W", Environment.CurrentManagedThreadId, m_Tag, message);
        public void Error(object message) => LogUtility.LogFormat(m_Handler, LogType.Error, m_Context, ILog.LOG_FORMAT, "E", Environment.CurrentManagedThreadId, m_Tag, message);
        public void Exception(Exception exception) => LogUtility.LogFormat(m_Handler, LogType.Exception, m_Context, ILog.LOG_FORMAT, "X", Environment.CurrentManagedThreadId, m_Tag, exception);
        public void Verb(object message)
        {
            throw new NotImplementedException();
        }
    }
}