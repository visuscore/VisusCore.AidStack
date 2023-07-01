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
        while (!stoppingTask.IsCompleted);
    }

    public static async Task AwaitEachAsync<TItem>(
        this IEnumerable<TItem> source,
        Func<TItem, Task> asyncOperation,
        CancellationToken cancellationToken)
    {
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
