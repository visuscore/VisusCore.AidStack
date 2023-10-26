using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VisusCore.AidStack.Extensions;

public static class TaskExtensions
{
    public static async Task WaitTasksUntilStopAsync(this IEnumerable<Task> tasks, Task stoppingTask)
    {
        do
        {
            await Task.WhenAny(
                tasks.Where(task => !task.IsCompleted)
                    .Concat(new[] { stoppingTask }));
        }
        while (stoppingTask?.IsCompleted is not false);
    }

    public static async Task AwaitEachAsync<TItem>(
        this IEnumerable<TItem> source,
        Func<TItem, Task> asyncOperation,
        CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (asyncOperation is null)
        {
            throw new ArgumentNullException(nameof(asyncOperation));
        }

        foreach (var item in source)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            await asyncOperation(item);
        }
    }
}
