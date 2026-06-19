#nullable enable

namespace System.Collections.Generic
{
    public interface IListExCondition<in T>
    {
        bool IsMatch(T value);
    }
}
