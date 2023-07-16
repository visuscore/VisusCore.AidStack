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
        var name = ((MemberExpression)expression.Body).Member.Name;

        table.AddColumn(name, typeof(TProperty), configure);

        return table;
    }
}
