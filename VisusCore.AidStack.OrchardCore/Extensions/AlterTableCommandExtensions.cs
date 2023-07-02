using System;
using System.Linq.Expressions;
using YesSql.Sql.Schema;

namespace VisusCore.AidStack.OrchardCore.Extensions;

public static class AlterTableCommandExtensions
{
    public static IAlterTableCommand AddColumn<TModel, TProperty>(
        this IAlterTableCommand table,
        Expression<Func<TModel, TProperty>> expression,
        Action<IAddColumnCommand> configure = null)
    {
        var name = ((MemberExpression)expression.Body).Member.Name;

        table.AddColumn(name, typeof(TProperty), configure);

        return table;
    }
}
