#nullable enable

using Cysharp.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniExtension;
using UnityEngine;

namespace IDaNote.Utils
{
    public static class LogUtility
    {
        internal delegate void LogInfoDelegate(string message, string fileName, int lineNumber, int columnNumber);
        internal static LogInfoDelegate? s_LogInfo;
        internal static LogInfoDelegate? s_LogWarning;
        internal static LogInfoDelegate? s_LogError;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void cctor()
        {
#if UNITY_EDITOR
            s_LogInfo = (from m in typeof(UnityEngine.Debug).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                         where m.Name == "LogInformation" &&
                             m.GetParameters().Length == 4 &&
                             m.GetParameters().FirstOrDefault().ParameterType == typeof(string)
                         select (LogInfoDelegate)Delegate.CreateDelegate(typeof(LogInfoDelegate), m)).FirstOrDefault();

            s_LogError = (from m in typeof(UnityEngine.Debug).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                          where m.Name == "LogCompilerError" &&
                              m.GetParameters().Length == 4 &&
                              m.GetParameters().FirstOrDefault().ParameterType == typeof(string)
                          select (LogInfoDelegate)Delegate.CreateDelegate(typeof(LogInfoDelegate), m)).FirstOrDefault();

            s_LogWarning = (from m in typeof(UnityEngine.Debug).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
                            where m.Name == "LogCompilerWarning" &&
                                m.GetParameters().Length == 4 &&
                                m.GetParameters().FirstOrDefault().ParameterType == typeof(string)
                            select (LogInfoDelegate)Delegate.CreateDelegate(typeof(LogInfoDelegate), m)).FirstOrDefault();
#endif
        }

        internal static LogInfoDelegate? GetLogDelegate(LogType type)
        {
            switch (type)
            {
                case LogType.Error: return s_LogError;
                case LogType.Assert: return s_LogError;
                case LogType.Warning: return s_LogWarning;
                case LogType.Log: return s_LogInfo;
                case LogType.Exception: return s_LogError;
                default: return s_LogInfo;
            }
        }

        const string k_DefineVerboseLogging = "ENABLE_VERBOSE_LOGGING";

        [Conditional(k_DefineVerboseLogging)]
        public static void Verb(this ILog logger, object message)
        {
            LogUtility.LogFormat(logger.LogHandler, (LogType)10, logger.Context, ILog.LOG_FORMAT, "V", Environment.CurrentManagedThreadId, logger.Tag, message);
        }

        public static void Forget(this UniTask task, ILog log)
        {
            InternalForget(task, log).Forget();
            static async UniTaskVoid InternalForget(UniTask task, ILog log)
            {
                try
                {
                    await task;
                }
                catch (Exception ex)
                {
                    log.Exception(ex);
                }
            }
        }

        public static void Forget<T>(this UniTask<T> task, ILog log)
        {
            InternalForget<T>(task, log).Forget();
            static async UniTaskVoid InternalForget<T>(UniTask<T> task, ILog log)
            {
                try
                {
                    await task;
                }
                catch (Exception ex)
                {
                    log.Exception(ex);
                }
            }
        }

        public static async void Forget(this Task task, ILog log)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                log.Exception(ex);
            }
        }

        public static async void Forget<T>(this Task<T> task, ILog log)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                log.Exception(ex);
            }
        }

        public static void LogFormat(ILogHandler logHandler, LogType type, UnityEngine.Object? context, string format, params object[] args)
        {
#if UNITY_EDITOR
            var printer = LogUtility.GetLogDelegate(type);
            if (printer != null)
            {
                Exception? ex = null;

                var sb = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is Exception)
                    {
                        ex = (Exception)args[i];
                        ExceptionUtility.AppendSB(sb, ex);
                        args[i] = sb.ToString();
                        sb.Clear();
                    }
                }
                sb.AppendFormat(format, args);

                if (ex != null && ExceptionUtility.GetFirstFileInfo(ex, out var f, out var l, out var c))
                {
                    printer(sb.ToString(), f, l, c);
                    return;
                }

                var stacktrace = new StackTrace(true);
                ExceptionUtility.GetFirstFileInfo(stacktrace, out f, out l, out c);
                if (IsStacktrace(type))
                {
                    sb.AppendLine().AppendLine();
                    ExceptionUtility.AppendSB(sb, stacktrace);
                }
                printer(sb.ToString(), f, l, c);
            }
            else if (ThreadUtility.IsMainThread())
            {
                var bef = Application.GetStackTraceLogType(LogType.Exception);
                Application.SetStackTraceLogType(type, StackTraceLogType.None);

                if (args[3] is Exception ex)
                {
                    logHandler.LogException(ex, context);
                }
                else
                {
                    logHandler.LogFormat(type, context, format, args);
                }

                Application.SetStackTraceLogType(type, bef);
            }
            else
#endif
            {
                if (args[3] is Exception ex)
                {
                    logHandler.LogException((Exception)args[3], context);
                }
                else
                {
                    logHandler.LogFormat(type, context, format, args);
                }
            }
        }

        private static bool IsStacktrace(LogType type)
        {
            switch (type)
            {
                case LogType.Error:
                case LogType.Assert:
                case LogType.Warning:
                case LogType.Log:
                    return true;
                case LogType.Exception:
                default:
                    return false;
            }
        }
    }
}
