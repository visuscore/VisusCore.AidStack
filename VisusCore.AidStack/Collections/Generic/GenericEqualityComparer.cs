using System;
using System.Collections.Generic;

namespace VisusCore.AidStack.Collections.Generic;

public class GenericEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _predicate;
    private readonly Func<T, int> _getHashCode;

    public GenericEqualityComparer(Func<T, T, bool> predicate, Func<T, int> getHashCode = null)
    {
        _predicate = predicate;
        _getHashCode = getHashCode;
    }

    public bool Equals(T x, T y) =>
        _predicate(x, y);

    public int GetHashCode(T obj) =>
        _getHashCode?.Invoke(obj) ?? obj.GetHashCode();
}
