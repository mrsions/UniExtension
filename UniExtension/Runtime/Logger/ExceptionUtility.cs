#nullable enable

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;

namespace IDaNote.Utils
{
    public static class ExceptionUtility
    {
        public static bool PrintParams = false;
        public static bool PrintOnlyHasSourceCode = true;

        public static void AppendSB(StringBuilder sb, Exception ex)
        {
            if (ex.InnerException != null)
            {
                AppendSB(sb, ex.InnerException);
                sb.AppendLine().Append("<b>rethrow</b> ");
            }

            sb.Append(ToString(ex.GetType())).Append(": ").AppendLine(ex.Message);
            AppendSB(sb, new StackTrace(ex, true));

            if (ex is AggregateException aex)
            {
                for (int i = 0; i < aex.InnerExceptions.Count; i++)
                {
                    Exception? e = aex.InnerExceptions[i];
                    sb.AppendLine("");
                    sb.AppendLine("-----------------------------------------");
                    sb.Append("<b>#").Append(i).Append("</b>");
                    AppendSB(sb, e);
                }
            }
        }

        public static void AppendSB(StringBuilder sb, StackTrace stack)
        {
            var homePath = GetHomePath();

            for (int i = 0, count = stack.FrameCount; i < count; i++)
            {
                StackFrame? frame = stack.GetFrame(i);

                var method = frame.GetMethod();
                var type = method.DeclaringType;

                if (!IsPrintable(method))
                {
                    continue;
                }

                bool hasFile = !string.IsNullOrWhiteSpace(frame.GetFileName());
                if (!hasFile && PrintOnlyHasSourceCode) continue;

                if (i != 0) sb.AppendLine();
                sb.Append(ToString(method)).Append(" (");
                if (PrintParams)
                {
                    bool first = true;
                    foreach (var p in method.GetParameters())
                    {
                        if (first) first = false;
                        else sb.Append(", ");
                        sb.Append(ToString(p.ParameterType));
                    }
                }
                sb.Append(")");

                if (hasFile)
                {
                    var fpath = frame.GetFileName();
                    if (fpath.StartsWith(homePath))
                    {
                        fpath = fpath.Substring(homePath.Length + 1);
                    }

                    
                    sb.Append(" (at <a href=\"").Append(fpath).Append("#").Append(frame.GetFileLineNumber()).Append("\">").Append(Path.GetFileName(fpath)).Append(":").Append(frame.GetFileLineNumber()).Append("</a>)");
                }
            }
        }

        public static bool IsPrintable(MethodBase method)
        {
            if (method == null) return false;

            var type = method.DeclaringType;
            if (type == null) return false;

            if (type.Namespace != null)
            {
                if (type.Namespace == "IDaNote.Utils" && type.Name.StartsWith("Log")) return false; ;
                if (type.Namespace.StartsWith("Cysharp.Threading.Tasks")) return false;
                if (type.Namespace.StartsWith("System.Threading.Tasks")) return false;
            }
            return true;
        }

        public static string ToString(MethodBase method)
        {
            var result = ToString(method.DeclaringType);
            if (result != "") result += ":";

            if (method != null)
            {
                result += method.Name;
            }
            return result;
        }
        public static string ToString(Type type)
        {
            if (type == null) return "";
            if (string.IsNullOrWhiteSpace(type.Namespace))
            {
                return type.Name;
            }
            else
            {
                return type.Namespace + "." + type.Name;
            }
        }

        private static string? s_HomePath;
        private static string GetHomePath()
        {
            return s_HomePath ??= Path.GetFullPath(".");
        }

        public static bool GetFirstFileInfo(StackTrace trace, [NotNullWhen(true)] out string? f, out int l, out int c)
        {
            var homePath = GetHomePath();
            foreach (var frame in trace.GetFrames())
            {
                if (!string.IsNullOrWhiteSpace(frame.GetFileName()))
                {
                    var fpath = frame.GetFileName();
                    if (fpath.StartsWith(homePath))
                    {
                        fpath = fpath.Substring(homePath.Length + 1);
                    }
                    f = fpath;
                    l = frame.GetFileLineNumber();
                    c = frame.GetFileColumnNumber();
                    return true;
                }
            }

            f = null;
            l = 0;
            c = 0;
            return false;
        }

        public static bool GetFirstFileInfo(Exception ex, [NotNullWhen(true)] out string? f, out int l, out int c)
        {
            if (ex.InnerException != null && GetFirstFileInfo(ex.InnerException, out f, out l, out c))
            {
                return true;
            }

            return GetFirstFileInfo(new StackTrace(ex), out f, out l, out c);
        }
    }
}