using System;
using System.Linq.Expressions;
using VisusCore.AidStack.OrchardCore.Abstractions;
using YesSql.Sql.Schema;

namespace VisusCore.AidStack.OrchardCore.Extensions;

public static class AlterTableCommandExtensions
{
    public static IAlterTableCommand<TModel> AddColumn<TModel, TProperty>(
        this IAlterTableCommand<TModel> table,
        Expression<Func<TModel, TProperty>> expression,
        Action<IAddColumnCommand> configure = null)
    {
        if (expression is null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (table is null)
        {
            throw new ArgumentNullException(nameof(table));
        }

        var name = (expression.Body as MemberExpression).Member.Name;

        table.AddColumn(name, typeof(TProperty), configure);

        return table;
    }
}
