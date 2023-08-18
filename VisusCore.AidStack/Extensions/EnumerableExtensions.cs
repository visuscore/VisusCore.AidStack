using System.Collections.Generic;
using System.Linq;

namespace VisusCore.AidStack.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> source, int size)
    {
        var values = source.ToArray();
        while (values.Any())
        {
            yield return values.Take(size);
            values = values.Skip(size).ToArray();
        }
    }
}
