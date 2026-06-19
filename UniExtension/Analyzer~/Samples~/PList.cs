#nullable enable

namespace UniExtension.Collections;

public class PList<T> : List<T>, IDisposable
{
    internal PList() : base(10) { }
    internal PList(int capacity) : base(capacity) { }
    public PList(IEnumerable<T> collection) : base(collection) { }
    public void Dispose()
    {
    }
}

public class PList
{
    public static PList<T> Take<T>()
    {
        return new();
    }

    public static PList<T> Take<T>(int capacity)
    {
        return new(capacity);
    }

    public static PList<T> Take<T>(IEnumerable<T> enumerable)
    {
        return new(enumerable);
    }
}
