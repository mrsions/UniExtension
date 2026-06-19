#nullable enable

using System.Collections.Generic;
using UniExtension.Collections;

namespace UniExtension
{
    public static class EnumerableExtension
    {
        public static PList<T> ToPList<T>(this IEnumerable<T> source)
        {
            return PList.Take<T>(source);
        }
    }
}
