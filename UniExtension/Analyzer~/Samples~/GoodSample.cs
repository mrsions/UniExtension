#nullable enable

using UniExtension.Collections;

public static class GoodSample
{
    public static void UseList()
    {
        using var list = PList.Take<int>();
        list.Add(1);
    }

    public static void DisposedList()
    {
        var list = PList.Take<int>();
        list.Add(1);
        list.Dispose();
    }

    public static void FinallyDisposed()
    {
        PList<int>? list = null;
        try
        {
            list = PList.Take<int>();
            list.Add(1);
        }
        finally
        {
            list?.Dispose();
        }
    }

    public static PList<int> CreateList()
    {
        var list = PList.Take<int>();
        list.Add(1);
        return list;
    }
}
