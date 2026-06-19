#nullable enable

using System.Threading;

namespace UniExtension
{
public static class ThreadUtility
{
    private static readonly int s_mainThreadId = Thread.CurrentThread.ManagedThreadId;

    public static bool IsMainThread()
    {
        return Thread.CurrentThread.ManagedThreadId == s_mainThreadId;
    }
}
}
